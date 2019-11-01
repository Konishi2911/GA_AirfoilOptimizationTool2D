using System;
using System.ComponentModel;

namespace GA_AirfoilOptimizationTool2D
{
    public class OptimizingConfiguration : General.ModelBase
    {
        #region Fields
        private const int numberOfSameGenerations = GeneralConstants.NUMBER_OF_AIRFOILS_OF_GENERATION;

        private General.BasisAirfoils _basisAirfoils;
        private Airfoil.CombinedAirfoilsGroupManager _currentAirfoils;
        private Double[,] _coefficientOfCombination;

        private int[] parentsIndex;
        private Airfoil.CombinedAirfoilsGroupManager _offspringAirfoilsCandidates;

        private FAirfoilGAManager.AirfoilGAManager airfoilGAManager;

        // Temporary
        private String OffspringsExportDirectory;
        private int ExportResolution;
        #endregion

        #region Properties
        public General.BasisAirfoils BasisAirfoils
        {
            get => _basisAirfoils;
            set
            {
                _basisAirfoils = value;
                OnPropertyChanged(nameof(BasisAirfoils));
            }
        }
        /// <summary>
        /// This airfoils are displayed on preview windows in the Main window
        /// </summary>
        public Airfoil.CombinedAirfoilsGroupManager CurrentAirfoilsPopulation
        {
            get => _currentAirfoils;
            set
            {
                _currentAirfoils = value;
                OnPropertyChanged(nameof(CurrentAirfoilsPopulation));
            }
        }
        public Double[,] CoefficientOfCombination
        {
            get => _coefficientOfCombination;
            set
            {
                _coefficientOfCombination = value;
                OnPropertyChanged(nameof(CoefficientOfCombination));

                System.Diagnostics.Debug.Print("OptimizingConfiguration.CoefficientOfCombination");
            }

        }

        public Airfoil.CombinedAirfoilsGroupManager OffspringAirfoilsCandidates
        {
            get => _offspringAirfoilsCandidates;
            set
            {
                _offspringAirfoilsCandidates = value;
                OnPropertyChanged(nameof(OffspringAirfoilsCandidates));
                OffspringsAirfoilsReady(this, new EventArgs());
            }
        }
        #endregion

        public event EventHandler SourceDataChanged;
        public event EventHandler OffspringsAirfoilsReady;

        /// <summary>
        /// This Class is Singleton
        /// </summary>
        private OptimizingConfiguration()
        {
            // Instantiate
            OffspringsExportDirectory = "C:\\Users\\Fluidlab\\Desktop\\Airfoils\\Offsprings";

            CurrentAirfoilsPopulation = new Airfoil.CombinedAirfoilsGroupManager(numberOfSameGenerations);

            // Assign Event
            this.PropertyChanged += This_PropertyChanged;
        }
        public static OptimizingConfiguration Instance { get; private set; } = new OptimizingConfiguration();

        #region Event Callbacks
        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Airfoils Source Changed
            if (e.PropertyName == nameof(this.BasisAirfoils) || e.PropertyName == nameof(this.CoefficientOfCombination))
            {
                if (CoefficientOfCombination == null)
                {
                    return;
                }

                // If number of basis airfoils are greater than number of coefficient of combination
                if (BasisAirfoils.NumberOfAirfoils > CoefficientOfCombination.GetLength(0))
                {
                    // Add new row of coefficient at the last of CoefficientOfCombination.
                    AddCoefficient();
                }

                // Re-Generate the combined airfoils
                CurrentAirfoilsPopulation.CombineAirfoils(General.BasisAirfoils.Convert(BasisAirfoils), CoefficientOfCombination);

                // Fire the event updated SourceData are ready
                SourceDataChanged?.Invoke(this, new EventArgs());
            }

            // Offspring Airfoils Candidates Changed
            else if (e.PropertyName == nameof(this.OffspringAirfoilsCandidates))
            {
                // Fire an event offspring airfoils candidates are ready.
                OffspringsAirfoilsReady?.Invoke(this, new EventArgs());
            }
        }
        #endregion

        /// <summary>
        /// Set Sources of current airfoils
        /// </summary>
        /// <param name="baseAirfoils"></param>
        /// <param name="coefficients"></param>
        public void SetSource(General.BasisAirfoils baseAirfoils, Double[,] coefficients)
        {
            this._basisAirfoils = baseAirfoils;
            this._coefficientOfCombination = coefficients;

            if (baseAirfoils != null && coefficients != null)
            {
                // If number of basis airfoils are greater than number of coefficient of combination
                if (BasisAirfoils.NumberOfAirfoils > CoefficientOfCombination.GetLength(0))
                {
                    // Add new row of coefficient at the last of CoefficientOfCombination.
                    AddCoefficient(BasisAirfoils.NumberOfAirfoils - CoefficientOfCombination.GetLength(0));
                }

                // Re-Generate the combined airfoils
                CurrentAirfoilsPopulation.CombineAirfoils(General.BasisAirfoils.Convert(BasisAirfoils), CoefficientOfCombination);

                // Fire the event updated SourceData are ready
                SourceDataChanged?.Invoke(this, new EventArgs());
            }
            else if (baseAirfoils == null && coefficients != null)
            {

            }
            else if (baseAirfoils != null && coefficients == null)
            {
                // Add new row of coefficient at the last of CoefficientOfCombination.
                _coefficientOfCombination = new double[0,10];
                AddCoefficient(BasisAirfoils.NumberOfAirfoils);

                // Re-Generate the combined airfoils
                CurrentAirfoilsPopulation.CombineAirfoils(General.BasisAirfoils.Convert(BasisAirfoils), CoefficientOfCombination);

                // Fire the event updated SourceData are ready
                SourceDataChanged?.Invoke(this, new EventArgs());
            }
        }

        public void StartCrossovers()
        {
            airfoilGAManager = new FAirfoilGAManager.AirfoilGAManager();
            airfoilGAManager.StartCrossover(CurrentAirfoilsPopulation);

            parentsIndex = airfoilGAManager.ParentsIndex;
            OffspringAirfoilsCandidates = airfoilGAManager.OffspringAirfoilCandidates;
            ExportAsCSV();
        }

        public void StartSelection()
        {

        }

        /// <summary>
        /// Add a new row element of the corfficient at the last of the CoefficientCollection
        /// </summary>
        private void AddCoefficient()
        {
            var length = CoefficientOfCombination.GetLength(0);
            var width = CoefficientOfCombination.GetLength(1);

            var newCoefficientCollection = new Double[length + 1, width];

            Array.Copy(CoefficientOfCombination, newCoefficientCollection, length * width);

            // Update coefficientCollection directly (without Event firing).
            _coefficientOfCombination = newCoefficientCollection;
        }
        /// <summary>
        /// Add the number of coefficients specifiedbu the argument
        /// </summary>
        /// <param name="count"></param>
        private void AddCoefficient(int count)
        {
            for (int i = 0; i < count; i++)
            {
                AddCoefficient();
            }
        }

        private void ExportAsCSV()
        {
            var offsprings = OffspringAirfoilsCandidates.GetCombinedAirfoilsArray();
            Airfoil.AirfoilCoordinateExporter CSVexporter = new Airfoil.AirfoilCoordinateExporter(OffspringsExportDirectory);
            var i = 1;
            foreach (var item in offsprings)
            {
                var offspringAirfoils = item.CombinedAirfoil;
                offspringAirfoils.AirfoilName = "Offspring_" + i;
                CSVexporter.ExportAirfoilCoordinate(item.CombinedAirfoil, ExportResolution);
                i++;
            }
        }
    }
}
