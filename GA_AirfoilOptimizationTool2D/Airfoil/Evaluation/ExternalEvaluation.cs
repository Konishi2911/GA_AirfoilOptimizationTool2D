namespace GA_AirfoilOptimizationTool2D.Airfoil.Evaluation
{
    /// <summary>
    /// The Airfoils are evaluated at the external evaluation method such as CFD software.
    /// </summary>
    public class ExternalEvaluation
    {
        private CombinedAirfoilsGroup combinedAirfoilsGroup;
        private General.BasisAirfoils basisAirfoils;

        public ExternalEvaluation(General.BasisAirfoils basisAirfoils)
        {
            this.basisAirfoils = basisAirfoils;
        }

        /// <summary>
        /// Execute Evaluation of airfoils that is made from optimized parameters.
        /// </summary>
        /// <param name="optParams"></param>
        /// <returns></returns>
        public double[] EvaluateAirfoils(double[][] optParams)
        {
            double[] fitness = new double[basisAirfoils.NumberOfAirfoils];

            var combinedAirfoilsGroup = AssignParameters(optParams, basisAirfoils);
            var coordinateCSV = CreateCSV(combinedAirfoilsGroup);

            return fitness;
        }

        private string[] CreateCSV(CombinedAirfoilsGroup combinedAirfoilsGroup)
        {
            var combinedAirfoils = combinedAirfoilsGroup.CombinedAirfoils;
            var length = combinedAirfoils.Length;
            string[] coordinateCSV = new string[length];

            for (int i = 0; i < length; i++)
            {
                coordinateCSV[i] = General.CsvManager.CreateCSV(combinedAirfoils[i].InterpolatedCoordinate.ToDouleArray());
            }

            return coordinateCSV;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optParams"></param>
        /// <param name="basisAirfoils"></param>
        /// <returns></returns>
        private CombinedAirfoilsGroup AssignParameters(double[][] optParams, General.BasisAirfoils basisAirfoils)
        {
            // Format Check
            if (optParams.Length == basisAirfoils.NumberOfAirfoils)
            {
                var length = basisAirfoils.NumberOfAirfoils;
                combinedAirfoilsGroup = new CombinedAirfoilsGroup(basisAirfoils);

                // Create optimized Coefficients
                var optCoefficients = new CoefficientOfCombination(General.ArrayManager.ConvertJuggedArrayToArray(optParams));

                // Combine airfoil
                AirfoilsMixer airfoilsMixer = new AirfoilsMixer(basisAirfoils, optCoefficients);
                airfoilsMixer.CombineAirfoils();

                // Assign combined airfoils into the CombinedAirfoilsGroup
                combinedAirfoilsGroup.AddRange(airfoilsMixer.CombinedAirfoils);
            }

            return combinedAirfoilsGroup;
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
                    array[i,j] = jArray[i][j];
                }
            }

            return array;
        }
    }
}
