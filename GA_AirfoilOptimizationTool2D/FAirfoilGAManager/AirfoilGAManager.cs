namespace GA_AirfoilOptimizationTool2D.FAirfoilGAManager
{
    public class AirfoilGAManager
    {
        #region Field
        private General.BasisAirfoils basisAirfoils;
        private Airfoil.CombinedAirfoilsGroupManager parentAirfoils;
        private int[] parentsIndex;
        private Airfoil.CombinedAirfoilsGroupManager offspringAirfoilsCombiner;
        private Airfoil.CombinedAirfoilGroup offspringAirfoils;
        private Airfoil.CombinedAirfoilGroup nextGenerations;
        private ExternalAirfoilEvaluation airfoilEvaluation;
        private AirfoilCrossover crossoverExecutor;
        private AirfoilSelection selectionExecutor;
        #endregion

        #region Properties
        public int[] ParentsIndex => parentsIndex;
        public Airfoil.CombinedAirfoilsGroupManager OffspringAirfoilCandidates => offspringAirfoilsCombiner;
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


        public void StartCrossover(Airfoil.CombinedAirfoilsGroupManager parents)
        {
            parentAirfoils = parents;
            var parentsAirfoilsArray = parentAirfoils.GetCombinedAirfoilsArray();

            // Initialize Basis airfoils
            basisAirfoils = new General.BasisAirfoils(parentsAirfoilsArray[0].BasisAirfoils);

            // Execute Crossover
            crossoverExecutor.ExecuteCrossover(parentAirfoils);

            // Read Offsprings' optimization parameters
            var optParams = crossoverExecutor.OptimizationParamters;
            optParams = SwapJuggedArray(optParams);

            // Assign Selected Parents Index
            parentsIndex = crossoverExecutor.ParentsIndex;

            // Create Offspring Airfoils
            offspringAirfoilsCombiner = new Airfoil.CombinedAirfoilsGroupManager(crossoverExecutor.NumberOfCrossovers);
            offspringAirfoilsCombiner.CombineAirfoils(basisAirfoils, ConvertJuggedArrayToArray(optParams));
        }

        public void StartSelection(Airfoil.CombinedAirfoilsGroupManager offsprings)
        {
            // Executes selection to extract airfoil from offsprings
            selectionExecutor.ExecuteSelection(offsprings);

            // Extract selected offsprings
            var selectedAirfoils = selectionExecutor.SelectedAirfoils;
            foreach (var item in selectedAirfoils)
            {
                offspringAirfoils.Add(item.CombinedAirfoil);
            }

            // Create next Generation
            int k = 0;
            var previousGen = parentAirfoils.GetCombinedAirfoilsArray();
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
        }

        private Airfoil.CombinedAirfoilsGroupManager CreateOffspringAirfoils(double[][] optParams)
        {
            Airfoil.CombinedAirfoilsGroupManager offsprings = new Airfoil.CombinedAirfoilsGroupManager(optParams.Length);

            offsprings.CombineAirfoils(basisAirfoils, ConvertJuggedArrayToArray(optParams));
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
