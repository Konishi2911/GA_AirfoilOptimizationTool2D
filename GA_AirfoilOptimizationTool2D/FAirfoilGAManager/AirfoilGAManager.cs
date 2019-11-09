namespace GA_AirfoilOptimizationTool2D.FAirfoilGAManager
{
    public class AirfoilGAManager
    {
        #region Field
        private General.BasisAirfoils basisAirfoils;
        private Airfoil.CombinedAirfoilsGroup parentAirfoils;
        private int[] parentsIndex;          
        private Airfoil.CombinedAirfoilsGroup offspringAirfoilsCombiner;
        private Airfoil.CombinedAirfoilsGroup offspringAirfoils;
        private Airfoil.CombinedAirfoilsGroup nextAirfoilGenerations;
        private ExternalAirfoilEvaluation airfoilEvaluation;
        private AirfoilCrossover crossoverExecutor;
        private AirfoilSelection selectionExecutor;
        #endregion

        #region Properties
        public int[] ParentsIndex => parentsIndex;
        public Airfoil.CombinedAirfoilsGroup OffspringAirfoilCandidates => offspringAirfoilsCombiner;
        public Airfoil.CombinedAirfoilsGroup NextAirfoilGenerations => nextAirfoilGenerations;
        #endregion

        #region Classes
        private FGeneticAlgorithm.GenerationalChange generationChangeExecutor;
        #endregion

        public AirfoilGAManager()
        {
            // Instantiate
            airfoilEvaluation = new ExternalAirfoilEvaluation();
            crossoverExecutor = new AirfoilCrossover(AirfoilCrossover.CrossoverOperator.UNDX);
            selectionExecutor = new AirfoilSelection(AirfoilSelection.SelectionModel.MGG);
        }

        /// <summary>
        /// Initialize without crossover procedure
        /// </summary>
        /// <param name="parentsIndex"></param>
        public AirfoilGAManager(int[] parentsIndex, Airfoil.CombinedAirfoilsGroup parentAirfoils) : this()
        {
            this.parentsIndex = parentsIndex;
            this.parentAirfoils = parentAirfoils;
        }


        public void StartCrossover(Airfoil.CombinedAirfoilsGroup parents)
        {
            parentAirfoils = parents;
            var parentsAirfoilsArray = parentAirfoils.CombinedAirfoils;

            // Initialize Basis airfoils
            basisAirfoils = new General.BasisAirfoils(parentAirfoils.BasisAirfoils.AirfoilGroup);

            // Execute Crossover
            crossoverExecutor.ExecuteCrossover(parentAirfoils.CoefficientOfCombination);

            // Read Offsprings' optimization parameters
            var optParams = crossoverExecutor.OptimizationParamters;
            optParams = SwapJuggedArray(optParams);
            var optCoef = new Airfoil.CoefficientOfCombination(General.ArrayManager.ConvertJuggedArrayToArray(optParams));

            // Assign Selected Parents Index
            parentsIndex = crossoverExecutor.ParentsIndex;

            // Initialize each fields
            offspringAirfoilsCombiner = new Airfoil.CombinedAirfoilsGroup(basisAirfoils);
            Airfoil.AirfoilsMixer airfoilsMixer = new Airfoil.AirfoilsMixer(basisAirfoils, optCoef);

            // Create Offspring Airfoils
            // Combine airfoils
            airfoilsMixer.CombineAirfoils();

            // Store combined airfoils into CombinedAirfoilsGroup class
            offspringAirfoilsCombiner.AddRange(airfoilsMixer.CombinedAirfoils);
        }

        public void StartSelection(Airfoil.CombinedAirfoilsGroup offsprings)
        {
            // Executes selection to extract airfoil from offsprings
            selectionExecutor.ExecuteSelection(offsprings);

            // Extract selected offsprings
            var selectedAirfoils = selectionExecutor.SelectedAirfoils;
            offspringAirfoils = new Airfoil.CombinedAirfoilsGroup(basisAirfoils);
            for (int i = 0; i < selectedAirfoils.CombinedAirfoils.Length; i++)
            {
                var airfoil = selectedAirfoils.CombinedAirfoils[i];
                var coefficients = selectedAirfoils.CoefficientOfCombination.GetCoefficients(i);

                offspringAirfoils.Add(airfoil, coefficients);
            }

            // Create next Generation
            int k = 0;
            var previousGen = parentAirfoils.CombinedAirfoils;
            Airfoil.CombinedAirfoilsGroup nextGenerations = new Airfoil.CombinedAirfoilsGroup(basisAirfoils);
            for (int i = 0; i < previousGen.Length; i++)
            {
                if (IsEqual(i, parentsIndex))
                {
                    var airfoil = selectedAirfoils.CombinedAirfoils[k];
                    var coefficients = selectedAirfoils.CoefficientOfCombination.GetCoefficients(k);

                    nextGenerations.Add(airfoil, coefficients);
                    ++k;
                }
                else
                {
                    var airfoil = parentAirfoils.CombinedAirfoils[i];
                    var coefficients = selectedAirfoils.CoefficientOfCombination.GetCoefficients(i);

                    nextGenerations.Add(airfoil, coefficients);
                }
            }
            nextAirfoilGenerations = nextGenerations;

            // newest virsion
            /*
            var nextGenerations = new Airfoil.CombinedAirfoilGroup();
            for (int i = 0; i < previousGen.Length; i++)
            {
                if (IsEqual(i, parentsIndex))
                {
                    nextGenerations.Add(selectedAirfoils[k].CombinedAirfoil);
                    ++k;
                }
                else
                {
                    nextGenerations.Add(previousGen[i].CombinedAirfoil);
                }
            }
            */
        }

        private Airfoil.CombinedAirfoilsGroup CreateOffspringAirfoils(Airfoil.CoefficientOfCombination optParams)
        {
            Airfoil.CombinedAirfoilsGroup offsprings = new Airfoil.CombinedAirfoilsGroup(basisAirfoils);
            Airfoil.AirfoilsMixer airfoilsMixer = new Airfoil.AirfoilsMixer(basisAirfoils, optParams);

            airfoilsMixer.CombineAirfoils();
            offspringAirfoils.AddRange(airfoilsMixer.CombinedAirfoils);

            return offsprings;
        }

        private T[][] SwapJuggedArray<T>(T[][] jArray)
        {
            bool isSameSize = true;
            // FormatCheck
            for (int i = 0; i < jArray.Length - 1; i++)
            {
                isSameSize &= jArray[i].Length == jArray[i + 1].Length;
            }

            if (isSameSize == false)
            {
                return null;
            }

            var length = jArray.Length;
            var width = jArray[0].Length;

            T[][] njArray = new T[width][];
            for (int i = 0; i < width; i++)
            {
                njArray[i] = new T[length];
                for (int j = 0; j < length; j++)
                {
                    njArray[i][j] = jArray[j][i];
                }
            }
            return njArray;

        }
        /// <summary>
        /// If invalid format jArray is passed, null is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jArray"></param>
        /// <returns></returns>
        private T[,] ConvertJuggedArrayToArray<T>(T[][] jArray)
        {
            bool isSameSize = true;
            // FormatCheck
            for (int i = 0; i < jArray.Length - 1; i++)
            {
                isSameSize &= jArray[i].Length == jArray[i + 1].Length;
            }

            if (isSameSize == false)
            {
                return null;
            }

            var length = jArray.Length;
            var width = jArray[0].Length;
            var array = new T[length, width];

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    array[i, j] = jArray[i][j];
                }
            }

            return array;
        }

        private bool IsEqual(int value, int[] array)
        {
            bool checker = false;
            foreach (var item in array)
            {
                checker |= item == value;
            }
            return checker;
        }
    }
}
