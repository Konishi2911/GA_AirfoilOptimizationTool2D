namespace GA_AirfoilOptimizationTool2D.FAirfoilGAManager
{
    public class AirfoilGAManager
    {
        #region Field
        private General.BasisAirfoils basisAirfoils;
        private Airfoil.CombinedAirfoilsGroupManager parentAirfoils;
        private Airfoil.CombinedAirfoilsGroupManager offspringAirfoils;
        private ExternalAirfoilEvaluation airfoilEvaluation;
        private AirfoilCrossover crossoverExecutor;
        private AirfoilSelection selectionExecutor;
        #endregion

        #region Classes
        private FGeneticAlgorithm.GenerationalChange generationChangeExecutor;
        #endregion

        public AirfoilGAManager()
        {
            // Instantiate
            airfoilEvaluation = new ExternalAirfoilEvaluation();
            crossoverExecutor = new AirfoilCrossover(AirfoilCrossover.CrossoverOperator.UNDX);
        }


        public void StartCrossover(Airfoil.CombinedAirfoilsGroupManager parents)
        {
            parentAirfoils = parents;

            // Execute Crossover
            crossoverExecutor.ExecuteCrossover(parentAirfoils);

            // Read Offsprings' optimization parameters
            var optParams = crossoverExecutor.OptimizationParamters;

            // Create Offspring Airfoils
            Airfoil.CombinedAirfoilsGroupManager offspringAirfoilsCombiner = new Airfoil.CombinedAirfoilsGroupManager(optParams.Length);
            offspringAirfoilsCombiner.CombineAirfoils(basisAirfoils, ConvertJuggedArrayToArray(optParams));

            // Register Created Offspring airfoils into OptimizingConfiguration
            OptimizingConfiguration.Instance.OffspringAirfoilsCandidates = offspringAirfoilsCombiner;
        }

        public void StartSelection(Airfoil.CombinedAirfoilsGroupManager offsprings)
        {
            // Executes selection to extract airfoil from offsprings
            selectionExecutor.ExecuteSelection(offsprings);

            // 
            selectionExecutor

        }

        private Airfoil.CombinedAirfoilsGroupManager CreateOffspringAirfoils(double[][] optParams)
        {
            Airfoil.CombinedAirfoilsGroupManager offsprings = new Airfoil.CombinedAirfoilsGroupManager(optParams.Length);

            offsprings.CombineAirfoils(basisAirfoils, ConvertJuggedArrayToArray(optParams));
            return offsprings;

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
                isSameSize &= jArray[i] == jArray[i + 1];
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
    }
}
