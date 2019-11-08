using System;
using System.ComponentModel;
using System.Reflection;

namespace GA_AirfoilOptimizationTool2D
{
    public class AirfoilOptimizationResource : General.ModelBase
    {
        #region Fields
        private General.BasisAirfoils _basisAirfoils;
        private Airfoil.CoefficientOfCombination _currentCoefficient;
        private Airfoil.CombinedAirfoilsGroup _currentPopulations;

        private int[] _parentsIndex;
        private Airfoil.CoefficientOfCombination _offspringCoefficients;
        private Airfoil.CombinedAirfoilsGroup _offsptingCandidates;

        private FAirfoilGAManager.AirfoilGAManager _airfoilGAManager;

        // Temporary
        private String OffspringsExportDirectory;
        private int ExportResolution;
        #endregion

        #region Properties
        public General.BasisAirfoils BasisAirfoils
        {
            get => this._basisAirfoils;
            private set
            {
                this._basisAirfoils = value;

                OnPropertyChanged(nameof(BasisAirfoils));
            }
        }
        public Airfoil.CoefficientOfCombination CurrentCoefficients
        {
            get => this._currentCoefficient;
            private set
            {
                this._currentCoefficient = value;

                OnPropertyChanged(nameof(CurrentCoefficients));
            }
        }
        public Airfoil.CombinedAirfoilsGroup CurrentPopulations
        {
            get => this._currentPopulations;
            private set
            {
                this._currentPopulations = value;

                OnPropertyChanged(nameof(CurrentPopulations));
            }
        }

        public int[] ParentsIndex => _parentsIndex;
        public Airfoil.CoefficientOfCombination OffspringCoefficients
        {
            get => this._offspringCoefficients;
            private set
            {
                this._offspringCoefficients = value;

                OnPropertyChanged(nameof(OffspringCoefficients));
            }
        }
        public Airfoil.CombinedAirfoilsGroup OffspringCandidates
        {
            get => this._offsptingCandidates;
            private set
            {
                this._offsptingCandidates = value;

                OnPropertyChanged(nameof(OffspringCandidates));
            }
        }
        #endregion

        #region Events
        public event EventHandler CurrentParameterUpdated;
        public event EventHandler CurrentPopulationUpdated;
        public event EventHandler OffspringCandidatesUpdated;
        #endregion

        private AirfoilOptimizationResource()
        {
            OffspringsExportDirectory = "..\\..\\..\\Offsprings";
        }
        public static AirfoilOptimizationResource Instance { get; } = new AirfoilOptimizationResource();

        #region Methods
        /// <summary>
        /// Set basis airfoils and coefficients of combination to fields, and combines airfoils by referring basis airfoils and coefficents.
        /// </summary>
        /// <param name="basisAirfoils"></param>
        /// <param name="coefficients"></param>
        public void SetSource(General.BasisAirfoils basisAirfoils, Airfoil.CoefficientOfCombination coefficients)
        {
            // Null checks
            if (basisAirfoils == null)
            {
                throw new ArgumentNullException(nameof(basisAirfoils));
            }
            else if (coefficients == null)
            {
                throw new ArgumentNullException(nameof(coefficients));
            }

            this._basisAirfoils = basisAirfoils;
            this._currentCoefficient = coefficients;

            // Initialization
            _currentPopulations = new Airfoil.CombinedAirfoilsGroup(_basisAirfoils.NumberOfAirfoils);
            //

            // Re-Generate the combined airfoils
            Airfoil.AirfoilsMixer airfoilsMixer = new Airfoil.AirfoilsMixer(basisAirfoils, coefficients);

            // Combine new airfoils
            airfoilsMixer.CombineAirfoils();
            // Add combined airfoils into the current airfoils population 
            _currentPopulations.AddRange(airfoilsMixer.CombinedAirfoils);

            // Fire the event updated coefficient are ready
            CurrentParameterUpdated?.Invoke(this, new EventArgs());
            // Fire the event updated SourceData are ready
            CurrentPopulationUpdated?.Invoke(this, new EventArgs());
        }
        public void SetSource(General.BasisAirfoils basisAirfoils)
        {
            // Null checks
            if (basisAirfoils == null)
            {
                throw new ArgumentNullException(nameof(basisAirfoils));
            }

            if (_currentCoefficient != null)
            {
                //Format check
                if (basisAirfoils.NumberOfAirfoils != _currentCoefficient.NoBasisAirfoils)
                {
                    throw new FormatException();
                }

                this._basisAirfoils = basisAirfoils;
                
                // Initialization
                _currentPopulations = new Airfoil.CombinedAirfoilsGroup(basisAirfoils.NumberOfAirfoils);
                //

                // Re-Generate the combined airfoils
                Airfoil.AirfoilsMixer airfoilsMixer = new Airfoil.AirfoilsMixer(basisAirfoils, _currentCoefficient);

                // Combine new airfoils
                airfoilsMixer.CombineAirfoils();
                // Add combined airfoils into the current airfoils population 
                _currentPopulations.AddRange(airfoilsMixer.CombinedAirfoils);

                // Fire the event updated SourceData are ready
                CurrentPopulationUpdated?.Invoke(this, new EventArgs());
            }
            else
            {
                var noBasis = basisAirfoils.NumberOfAirfoils;

                _basisAirfoils = basisAirfoils;
                _currentCoefficient = new Airfoil.CoefficientOfCombination(noBasis);

                // Fire the event updated coefficient are ready
                CurrentParameterUpdated?.Invoke(this, new EventArgs());
            }
        }
        public void SetSource(Airfoil.CoefficientOfCombination coefficients)
        {
            // Null check
            if (coefficients == null)
            {
                throw new ArgumentNullException(nameof(coefficients));
            }

            if (_basisAirfoils != null)
            {
                // Format Check
                if (_basisAirfoils.NumberOfAirfoils != coefficients.NoBasisAirfoils)
                {
                    throw new FormatException();
                }

                this._currentCoefficient = coefficients;

                // Initialization
                _currentPopulations = new Airfoil.CombinedAirfoilsGroup(_basisAirfoils.NumberOfAirfoils);
                //

                // Re-Generate the combined airfoils
                Airfoil.AirfoilsMixer airfoilsMixer = new Airfoil.AirfoilsMixer(_basisAirfoils, coefficients);

                // Combine new airfoils
                airfoilsMixer.CombineAirfoils();
                // Add combined airfoils into the current airfoils population 
                _currentPopulations.AddRange(airfoilsMixer.CombinedAirfoils);

                // Fire the event updated SourceData are ready
                CurrentPopulationUpdated?.Invoke(this, new EventArgs());
            }
            else
            {
                this._currentCoefficient = coefficients;
            }

            // Fire the event updated coefficient are ready
            CurrentParameterUpdated?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Set offspring candidates to fields
        /// </summary>
        /// <param name="coefficients"></param>
        public void SetOffspringCandidates(Airfoil.CoefficientOfCombination coefficients)
        {
            // Re-Generate the combined airfoils
            _offsptingCandidates = new Airfoil.CombinedAirfoilsGroup();
            Airfoil.AirfoilsMixer airfoilsMixer = new Airfoil.AirfoilsMixer(_basisAirfoils, coefficients);

            // Combine airfoils
            airfoilsMixer.CombineAirfoils();
            _offsptingCandidates.AddRange(airfoilsMixer.CombinedAirfoils);

            // Fire the event updated SourceData are ready
            OffspringCandidatesUpdated?.Invoke(this, new EventArgs());
        }

        public void StartCrossovers()
        {
            _airfoilGAManager = new FAirfoilGAManager.AirfoilGAManager();
            _airfoilGAManager.StartCrossover(_currentPopulations);

            _parentsIndex = _airfoilGAManager.ParentsIndex;
            _offsptingCandidates = _airfoilGAManager.OffspringAirfoilCandidates;
            ExportAsCSV();
        }

        public void StartSelection()
        {
            _airfoilGAManager ??= new FAirfoilGAManager.AirfoilGAManager(_parentsIndex, _currentPopulations);
            _airfoilGAManager.StartSelection(_offsptingCandidates);
            var NextGeneration = _airfoilGAManager.NextAirfoilGenerations;
            _currentPopulations = NextGeneration;

            CurrentPopulationUpdated?.Invoke(this, new EventArgs());
        }
        #endregion

        private void ExportAsCSV()
        {
            var offsprings = _offsptingCandidates.CombinedAirfoils;
            Airfoil.AirfoilCoordinateExporter CSVexporter = new Airfoil.AirfoilCoordinateExporter(OffspringsExportDirectory);
            var i = 1;
            foreach (var item in offsprings)
            {
                var offspringAirfoils = item;
                item.AirfoilName = "Offspring_" + i;
                CSVexporter.ExportAirfoilCoordinate(item, ExportResolution);
                i++;
            }
        }
    }
}
