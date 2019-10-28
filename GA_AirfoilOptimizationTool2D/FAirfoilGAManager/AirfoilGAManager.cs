namespace GA_AirfoilOptimizationTool2D.FAirfoilGAManager
{
    public class AirfoilGAManager
    {
        #region Field
        private General.BasisAirfoils basisAirfoils;
        private Airfoil.CombinedAirfoilsGroupManager parentAirfoils;
        private Airfoil.CombinedAirfoilsGroupManager offspringAirfoils;
        private ExternalAirfoilEvaluation airfoilEvaluation;
        #endregion

        #region Classes
        private FGeneticAlgorithm.GenerationalChange generationChangeExecutor;
        #endregion

        public AirfoilGAManager()
        {
            // Instantiate
            airfoilEvaluation = new ExternalAirfoilEvaluation();
        }

        public void StartOptimization(Airfoil.CombinedAirfoilsGroupManager parents)
        {
            parentAirfoils = parents;

            generationChangeExecutor = new FGeneticAlgorithm.GenerationalChange()
        }

        /// <summary>
        /// The fitness of each airfoil are calculated by external evaluation method
        /// </summary>
        /// <param name="optimizatinoParameters"></param>
        /// <returns></returns>
        private double[] AirfoilsEvaluationMethod(double[][] optimizatinoParameters)
        {
            offspringAirfoils = CreateOffspringAirfoils(optimizatinoParameters);
            airfoilEvaluation.StartEvaluation();

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
