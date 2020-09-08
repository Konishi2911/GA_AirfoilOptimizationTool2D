using System;

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

        private OptimizingMode _optimizingMode;

        // Temporary
        private String CsvExportDirectory;
        private int ExportResolution;
        #endregion

        #region Enums
        enum OptimizingMode
        {
            Lift,
            Drag,
            LiftDrag
        }
        #endregion

        #region Properties
        public General.LogMessage LogMessage { get; } = new General.LogMessage();

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

        public int[] ParentsIndex
        {
            get => _parentsIndex;
            set => _parentsIndex = value;
        }
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
            set
            {
                if (OffspringCandidates.BasisAirfoils == _basisAirfoils)
                {
                    this._offsptingCandidates = value;
                }

                // Scan offsprings' characteristics to see if they are set.
                ScanOffspringCharacteristics();

                OnPropertyChanged(nameof(OffspringCandidates));
            }
        }
        public bool CurrentAirfoilsReady { get; private set; }
        public bool OffspringAirfoilsReady { get; private set; }
        #endregion

        #region Events
        public event EventHandler LogMessageAdded;
        public event EventHandler CurrentParameterUpdated;
        public event EventHandler CurrentPopulationUpdated;
        public event EventHandler OffspringCandidatesUpdated;
        #endregion

        public enum ExportAirfoilsGroup
        {
            CurrentPopulation,
            OffspringPopulation
        }

        private AirfoilOptimizationResource()
        {
            _optimizingMode = OptimizingMode.LiftDrag;
            LogMessage.MessageUpdated += AddLogMessage;

            CsvExportDirectory = "..\\..\\..\\Offsprings";
        }

        #region Event Callbacks
        private void AddLogMessage(object sender, EventArgs e)
        {
            LogMessageAdded?.Invoke(sender, e);
        }
        #endregion

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
            _currentPopulations = new Airfoil.CombinedAirfoilsGroup(_basisAirfoils);
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
                _currentPopulations = new Airfoil.CombinedAirfoilsGroup(basisAirfoils);
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
                _currentCoefficient = new Airfoil.CoefficientOfCombination(noBasis, GeneralConstants.NUMBER_OF_AIRFOILS_OF_GENERATION);

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
                _currentPopulations = new Airfoil.CombinedAirfoilsGroup(_basisAirfoils);
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
        public void SetSource
        (
            General.BasisAirfoils basisAirfoils,
            Airfoil.CoefficientOfCombination coefficients,
            System.Collections.Generic.List<String> names,
            System.Collections.Generic.List<Airfoil.Characteristics.AngleBasedCharacteristics> lifts,
            System.Collections.Generic.List<Airfoil.Characteristics.AngleBasedCharacteristics> drags
        )
        {
            // Null check
            if
            (
                lifts == null ||
                drags == null ||
                names == null
            )
            {
                SetSource(basisAirfoils, coefficients);
                return;
            }

            //Format check
            if 
            (
                lifts.Count != coefficients.NoAirfoils || 
                drags.Count != coefficients.NoAirfoils ||
                names.Count != coefficients.NoAirfoils
            )
            {
                SetSource(basisAirfoils, coefficients);
                return;
            }

            SetSource(basisAirfoils, coefficients);

            for (int i = 0; i < _currentPopulations.NoAirfoils; i++)
            {
                _currentPopulations.CombinedAirfoils[i].AirfoilName = names[i];
                _currentPopulations.CombinedAirfoils[i].LiftProfile = lifts[i];
                _currentPopulations.CombinedAirfoils[i].DragProfile = drags[i];
            }
            ScanCurrentCharacteristics();
        }

        public void AddCurrentCharacteristics
        (
            System.Collections.Generic.List<String> names,
            System.Collections.Generic.List<Airfoil.Characteristics.AngleBasedCharacteristics> lifts,
            System.Collections.Generic.List<Airfoil.Characteristics.AngleBasedCharacteristics> drags
        )
        {
            // Null check
            if
            (
                lifts == null ||
                drags == null ||
                names == null ||
                _currentPopulations == null
            )
            {
                return;
            }

            //Format check
            if
            (
                lifts.Count != _currentPopulations.NoAirfoils ||
                drags.Count != _currentPopulations.NoAirfoils ||
                names.Count != _currentPopulations.NoAirfoils
            )
            {
                return;
            }

            for (int i = 0; i < _currentPopulations.NoAirfoils; i++)
            {
                _currentPopulations.CombinedAirfoils[i].AirfoilName = names[i];
                _currentPopulations.CombinedAirfoils[i].LiftProfile = lifts[i];
                _currentPopulations.CombinedAirfoils[i].DragProfile = drags[i];
            }

            ScanCurrentCharacteristics();
        }

        /// <summary>
        /// Set offspring candidates to fields
        /// </summary>
        /// <param name="coefficients"></param>
        public void SetOffspringCandidates(Airfoil.CoefficientOfCombination coefficients)
        {
            // Re-Generate the combined airfoils
            _offsptingCandidates = new Airfoil.CombinedAirfoilsGroup(_basisAirfoils);
            Airfoil.AirfoilsMixer airfoilsMixer = new Airfoil.AirfoilsMixer(_basisAirfoils, coefficients);

            // Combine airfoils
            airfoilsMixer.CombineAirfoils();
            _offsptingCandidates.AddRange(airfoilsMixer.CombinedAirfoils);

            // Fire the event updated SourceData are ready
            OffspringCandidatesUpdated?.Invoke(this, new EventArgs());
        }
        public void SetOffspringCandidates
        (
            Airfoil.CoefficientOfCombination coefficients,
            System.Collections.Generic.List<String> names,
            System.Collections.Generic.List<Airfoil.Characteristics.AngleBasedCharacteristics> lifts,
            System.Collections.Generic.List<Airfoil.Characteristics.AngleBasedCharacteristics> drags
        )
        {
            // Null check
            if
            (
                lifts == null ||
                drags == null ||
                names == null
            )
            {
                SetOffspringCandidates(coefficients);
                return;
            }

            //Format check
            if
            (
                lifts.Count != coefficients.NoAirfoils ||
                drags.Count != coefficients.NoAirfoils ||
                names.Count != coefficients.NoAirfoils
            )
            {
                SetOffspringCandidates(coefficients);
                return;
            }

            SetOffspringCandidates(coefficients);

            for (int i = 0; i < _offsptingCandidates.NoAirfoils; i++)
            {
                _offsptingCandidates.CombinedAirfoils[i].AirfoilName = names[i];
                _offsptingCandidates.CombinedAirfoils[i].LiftProfile = lifts[i];
                _offsptingCandidates.CombinedAirfoils[i].DragProfile = drags[i];
            }
            ScanOffspringCharacteristics();
        }

        public void AddOffspringCharacteristics
        (
            System.Collections.Generic.List<String> names,
            System.Collections.Generic.List<Airfoil.Characteristics.AngleBasedCharacteristics> lifts,
            System.Collections.Generic.List<Airfoil.Characteristics.AngleBasedCharacteristics> drags
        )
        {
            // Null check
            if
            (
                lifts == null ||
                drags == null ||
                names == null ||
                _offsptingCandidates == null
            )
            {
                return;
            }

            //Format check
            if
            (
                lifts.Count != _offsptingCandidates.NoAirfoils ||
                drags.Count != _offsptingCandidates.NoAirfoils ||
                names.Count != _offsptingCandidates.NoAirfoils
            )
            {
                return;
            }

            for (int i = 0; i < _offsptingCandidates.NoAirfoils; i++)
            {
                _offsptingCandidates.CombinedAirfoils[i].AirfoilName = names[i];
                _offsptingCandidates.CombinedAirfoils[i].LiftProfile = lifts[i];
                _offsptingCandidates.CombinedAirfoils[i].DragProfile = drags[i];
            }

            ScanOffspringCharacteristics();
        }

        public void StartCrossovers()
        {
            _airfoilGAManager = new FAirfoilGAManager.AirfoilGAManager();
            _airfoilGAManager.StartCrossover(_currentPopulations);

            _parentsIndex = _airfoilGAManager.ParentsIndex;
            _offsptingCandidates = _airfoilGAManager.OffspringAirfoilCandidates;

            // Fire the event updated SourceData are ready
            OffspringCandidatesUpdated?.Invoke(this, new EventArgs());

            // Export offsring's coefficient to as CSV files
            ExportAsCSV(ExportAirfoilsGroup.OffspringPopulation);
        }

        public void StartSelection()
        {
            _airfoilGAManager ??= new FAirfoilGAManager.AirfoilGAManager(_parentsIndex, _currentPopulations);
            _airfoilGAManager.StartSelection(_offsptingCandidates);
            var NextGeneration = _airfoilGAManager.NextAirfoilGenerations;

            // Update currnet populations with selected next generation
            _currentPopulations = NextGeneration;

            LogMessage.Write("================ Selection Result =================");
            for (int i = 0; i < _airfoilGAManager.SelectedOffspringsNo.Count; i++)
            {
                var num = _airfoilGAManager.SelectedOffspringsNo[i];
                LogMessage.Write("> Selected offspring : " + num);
            }

            // Clear the data of offspring populations.
            _offsptingCandidates = null;
            _offspringCoefficients = null;
            OffspringAirfoilsReady = false;

            // fires the event that notifies current pupulations updated.
            CurrentPopulationUpdated?.Invoke(this, new EventArgs());
        }
        #endregion

        public void ExportAsCSV(ExportAirfoilsGroup exportAirfoils)
        {
            Airfoil.AirfoilManager[] airfoils = null;
            String Tags = null;
            if (exportAirfoils == ExportAirfoilsGroup.CurrentPopulation)
            {
                airfoils = _currentPopulations.CombinedAirfoils;
                Tags = "CurrentPopulation_";
            }
            else if (exportAirfoils == ExportAirfoilsGroup.OffspringPopulation)
            {
                airfoils = _offsptingCandidates.CombinedAirfoils;
                Tags = "Offspring_";
            }


            Airfoil.AirfoilCoordinateExporter CSVexporter = new Airfoil.AirfoilCoordinateExporter(CsvExportDirectory);
            var i = 1;
            foreach (var item in airfoils)
            {
                var offspringAirfoils = item;
                item.AirfoilName = Tags + i;
                CSVexporter.ExportAirfoilCoordinate(item, ExportResolution);
                i++;
            }
        }
        private void ScanOffspringCharacteristics()
        {
            OffspringAirfoilsReady = true;
            if (_optimizingMode == OptimizingMode.Lift)
            {
                foreach (var item in _offsptingCandidates.CombinedAirfoils)
                {
                    OffspringAirfoilsReady &= item.LiftProfile != null;
                }
            }
            else if (_optimizingMode == OptimizingMode.Drag)
            {
                foreach (var item in _offsptingCandidates.CombinedAirfoils)
                {
                    OffspringAirfoilsReady &= item.DragProfile != null;
                }
            }
            else if (_optimizingMode == OptimizingMode.LiftDrag)
            {
                foreach (var item in _offsptingCandidates.CombinedAirfoils)
                {
                    OffspringAirfoilsReady &= item.LiftProfile != null && item.DragProfile != null;
                }
            }
        }
        private void ScanCurrentCharacteristics()
        {
            CurrentAirfoilsReady = true;
            if (_optimizingMode == OptimizingMode.Lift)
            {
                foreach (var item in _currentPopulations.CombinedAirfoils)
                {
                    CurrentAirfoilsReady &= item.LiftProfile != null;
                }
            }
            else if (_optimizingMode == OptimizingMode.Drag)
            {
                foreach (var item in _currentPopulations.CombinedAirfoils)
                {
                    CurrentAirfoilsReady &= item.DragProfile != null;
                }
            }
            else if (_optimizingMode == OptimizingMode.LiftDrag)
            {
                foreach (var item in _currentPopulations.CombinedAirfoils)
                {
                    CurrentAirfoilsReady &= item.LiftProfile != null && item.DragProfile != null;
                }
            }
        }
    }
}
