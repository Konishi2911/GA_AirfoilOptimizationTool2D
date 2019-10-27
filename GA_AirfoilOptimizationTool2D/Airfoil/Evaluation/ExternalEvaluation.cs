namespace GA_AirfoilOptimizationTool2D.Airfoil.Evaluation
{
    /// <summary>
    /// The Airfoils are evaluated at the external evaluation method such as CFD software.
    /// </summary>
    public class ExternalEvaluation
    {
        private CombinedAirfoilsGroupManager combinedAirfoilsGroup;
        private General.BasisAirfoils basisAirfoils;

        public ExternalEvaluation(General.BasisAirfoils basisAirfoils)
        {
            this.basisAirfoils = basisAirfoils;
        }

        public double[] EvaluateAirfoils(double[][] optParams)
        {
            double[] fitness = new double[basisAirfoils.NumberOfAirfoils];

            var combinedAirfoilsGroup = AssignParameters(optParams, basisAirfoils);
            var coordinateCSV = CreateCSV(combinedAirfoilsGroup);



            return fitness;
        }

        private string[] CreateCSV(CombinedAirfoilsGroupManager combinedAirfoilsGroup)
        {
            var combinedAirfoils = combinedAirfoilsGroup.GetCombinedAirfoilsArray();
            var length = combinedAirfoils.Length;
            string[] coordinateCSV = new string[length];

            for (int i = 0; i < length; i++)
            {
                coordinateCSV[i] = General.CsvManager.CreateCSV(combinedAirfoils[i].CombinedAirfoil.InterpolatedCoordinate.ToDouleArray());
            }

            return coordinateCSV;
        }

        private CombinedAirfoilsGroupManager AssignParameters(double[][] optParams, General.BasisAirfoils basisAirfoils)
        {
            // Format Check
            if (optParams.Length == basisAirfoils.NumberOfAirfoils)
            {
                var length = basisAirfoils.NumberOfAirfoils;
                combinedAirfoilsGroup = new CombinedAirfoilsGroupManager(length);

                // Combine Airfoils
                for (int i = 0; i < length; i++)
                {
                    combinedAirfoilsGroup.CombineAirfoils(basisAirfoils, ConvertJuggedArrayToArray(optParams));
                }
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
