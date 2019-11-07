using System;
using System.ComponentModel;
using System.Reflection;

namespace GA_AirfoilOptimizationTool2D
{
    public class OptimizingConfiguration : General.ModelBase
    {
        #region Fields
        private const int numberOfSameGenerations = GeneralConstants.NUMBER_OF_AIRFOILS_OF_GENERATION;

        private General.BasisAirfoils _basisAirfoils;
        private Airfoil.CombinedAirfoilsGroup _currentAirfoils;
        private Airfoil.CoefficientOfCombination _coefficientOfCombination;

        private int[] parentsIndex;
        private Airfoil.CombinedAirfoilsGroup _offspringAirfoilsCandidates;

        private FAirfoilGAManager.AirfoilGAManager airfoilGAManager;

        // Temporary
        private String OffspringsExportDirectory;
        private int ExportResolution;
        #endregion

        #region Properties
        public int[] ParentsIndex
        {
            get => parentsIndex;
            set => parentsIndex = value;
        }

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
        public Airfoil.CombinedAirfoilsGroup CurrentAirfoilsPopulation
        {
            get => _currentAirfoils;
            set
            {
                _currentAirfoils = value;
                OnPropertyChanged(nameof(CurrentAirfoilsPopulation));
            }
        }
        public Airfoil.CoefficientOfCombination CoefficientOfCombination
        {
            get => _coefficientOfCombination;
            set
            {
                _coefficientOfCombination = value;
                OnPropertyChanged(nameof(CoefficientOfCombination));

                System.Diagnostics.Debug.Print("OptimizingConfiguration.CoefficientOfCombination");
            }

        }

        public Airfoil.CombinedAirfoilsGroup OffspringAirfoilsCandidates
        {
            get => _offspringAirfoilsCandidates;
            set
            {
                _offspringAirfoilsCandidates = value;
                OnPropertyChanged(nameof(OffspringAirfoilsCandidates));
                OffspringsAirfoilsReady(this, new EventArgs());
            }
        }

        public bool OffspringAirfoilsReady { get; private set; }
        #endregion

        public event EventHandler SourceDataChanged;
        public event EventHandler OffspringsAirfoilsReady;

        /// <summary>
        /// This Class is Singleton
        /// </summary>
        private OptimizingConfiguration()
        {
            // Instantiate
            //OffspringsExportDirectory = "C:\\Users\\Fluidlab\\Desktop\\Airfoils\\Offsprings";
            Assembly thisAssembry = Assembly.GetEntryAssembly();
            String entryPath = thisAssembry.Location;
            OffspringsExportDirectory = "..\\..\\..\\Offsprings";

            CurrentAirfoilsPopulation = new Airfoil.CombinedAirfoilsGroup(numberOfSameGenerations);

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
                    AddCoefficient(out _coefficientOfCombination);
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

                // Check wheather characteristic of each offspring is ready.
                OffspringAirfoilsReady = true;
                foreach (var item in OffspringAirfoilsCandidates.GetCombinedAirfoilsArray())
                {
                    OffspringAirfoilsReady &= item.CombinedAirfoil.LiftProfile != null;
                }
            }

            // Current Airfoils Updates
            else if (e.PropertyName == nameof(CurrentAirfoilsPopulation))
            {
                var currentAirfoils = CurrentAirfoilsPopulation.GetCombinedAirfoilsArray();
                var noAirfoils = currentAirfoils.Length;
                double[,] currentCoefficients = new double[BasisAirfoils.NumberOfAirfoils, noAirfoils];

                for (int i = 0; i < BasisAirfoils.NumberOfAirfoils; i++)
                {
                    for (int j = 0; j < noAirfoils; j++)
                    {
                        currentCoefficients[i, j] = currentAirfoils[j].Coefficients[i];
                    }
                }

                _coefficientOfCombination = currentCoefficients;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// Set Sources of current airfoils
        /// </summary>
        /// <param name="baseAirfoils"></param>
        /// <param name="coefficients"></param>
        public void SetSource(General.BasisAirfoils baseAirfoils, Airfoil.CoefficientOfCombination coefficients)
        {
            this._basisAirfoils = baseAirfoils;
            this._coefficientOfCombination = coefficients;

            if (baseAirfoils != null && coefficients != null)
            {
                // If number of basis airfoils are greater than number of coefficient of combination
                if (BasisAirfoils.NumberOfAirfoils > CoefficientOfCombination.NoBasisAirfoils)
                {
                    // Add new empty row of coefficient at the end of CoefficientOfCombination.
                    int length = BasisAirfoils.NumberOfAirfoils - CoefficientOfCombination.NoBasisAirfoils;
                    for (int i = 0; i < length; i++)
                    {
                        _coefficientOfCombination.AddCoefficient(new double[_coefficientOfCombination.NoBasisAirfoils]);
                    }
                    //AddCoefficient(BasisAirfoils.NumberOfAirfoils - CoefficientOfCombination.NoBasisAirfoils, _coefficientOfCombination);
                }

                // Re-Generate the combined airfoils
                Airfoil.AirfoilsMixer airfoilsMixer = new Airfoil.AirfoilsMixer(BasisAirfoils, CoefficientOfCombination);

                // Combine new airfoils
                airfoilsMixer.CombineAirfoils();
                // Add combined airfoils into the current airfoils population 
                CurrentAirfoilsPopulation.AddRange(airfoilsMixer.CombinedAirfoils);

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
                AddCoefficient(BasisAirfoils.NumberOfAirfoils, _coefficientOfCombination);

                // Re-Generate the combined airfoils
                CurrentAirfoilsPopulation.CombineAirfoils(General.BasisAirfoils.Convert(BasisAirfoils), CoefficientOfCombination);

                // Fire the event updated SourceData are ready
                SourceDataChanged?.Invoke(this, new EventArgs());
            }
        }

        public void SetOffspringCadidates(General.BasisAirfoils baseAirfoils, Double[,] coefficients)
        {
            if (baseAirfoils != null && coefficients != null)
            {
                // If number of basis airfoils are greater than number of coefficient of combination
                if (BasisAirfoils.NumberOfAirfoils > CoefficientOfCombination.GetLength(0))
                {
                    // Add new row of coefficient at the last of CoefficientOfCombination.
                    var coef = _offspringAirfoilsCandidates.CoefficientOfCombination;
                    AddCoefficient(BasisAirfoils.NumberOfAirfoils - CoefficientOfCombination.GetLength(0), coef);
                }

                // Re-Generate the combined airfoils
                _offspringAirfoilsCandidates = new Airfoil.CombinedAirfoilsGroupManager(coefficients.GetLength(1));
                OffspringAirfoilsCandidates.CombineAirfoils(General.BasisAirfoils.Convert(BasisAirfoils), coefficients);

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
            airfoilGAManager ??= new FAirfoilGAManager.AirfoilGAManager(parentsIndex, CurrentAirfoilsPopulation);
            airfoilGAManager.StartSelection(OffspringAirfoilsCandidates);
            var NextGeneration = airfoilGAManager.NextAirfoilGenerations;
            CurrentAirfoilsPopulation = NextGeneration;

            SourceDataChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Add a new row element of the corfficient at the last of the CoefficientCollection
        /// </summary>
        private void AddCoefficient(out double[,] coefficients)
        {
            var length = CoefficientOfCombination.GetLength(0);
            var width = CoefficientOfCombination.GetLength(1);

            var newCoefficientCollection = new Double[length + 1, width];

            Array.Copy(CoefficientOfCombination, newCoefficientCollection, length * width);

            // Update coefficientCollection directly (without Event firing).
            coefficients = newCoefficientCollection;
        }
        /// <summary>
        /// Add the number of coefficients specifiedbu the argument
        /// </summary>
        /// <param name="count"></param>
        private void AddCoefficient(int count, double[,] coefficients)
        {
            for (int i = 0; i < count; i++)
            {
                AddCoefficient(out coefficients);
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
