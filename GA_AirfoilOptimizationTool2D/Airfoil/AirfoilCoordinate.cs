using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    public class AirfoilCoordinate
    {
        private List<Double[]> CoordinateList = new List<double[]>();

        public int Length { get; private set; }

        public class Coordinate
        {
            public Double X { get; set; }
            public Double Z { get; set; }

            public Coordinate(Double[] val)
            {
                X = val[0];
                Z = val[1];
            }
        }

        public AirfoilCoordinate() { }

        /// <summary>
        /// Get UpperLine From CoordinateList
        /// </summary>
        /// <returns></returns>
        public AirfoilCoordinate GetUpperLine()
        {
            var index = (int)GetMinimumIndex(CoordinateList, 0);

            Double[,] temp = new Double[index + 1, 2];
            for (int i = 0; i <= index; ++i)
            {
                temp[i, 0] = CoordinateList[index - i][0];
                temp[i, 1] = CoordinateList[index - i][1];
            }

            AirfoilCoordinate coordinate = new AirfoilCoordinate();
            coordinate.Import(temp);

            return coordinate;
        }

        /// <summary>
        /// Get LowerLine From CoordinateList
        /// </summary>
        /// <returns></returns>
        public AirfoilCoordinate GetLowerLine()
        {
            var index = (int)GetMinimumIndex(CoordinateList, 0);

            Double[,] temp = new Double[index + 1, 2];
            for (int i = index; i < CoordinateList.Count; ++i)
            {
                temp[i - index, 0] = CoordinateList[i][0];
                temp[i - index, 1] = CoordinateList[i][1];
            }

            AirfoilCoordinate coordinate = new AirfoilCoordinate();
            coordinate.Import(temp);

            return coordinate;
        }

        public void Import(Double[,] coordinate)
        {
            try
            {
                // Format Check
                var rowNo = coordinate.GetLength(0);
                var columnNo = coordinate.GetLength(1);
                if (columnNo != 2)
                {
                    throw new FormatException();
                }

                // Add coordinate
                for (int i = 0; i < rowNo; i++)
                {
                    var dataRpw = new Double[2];
                    for (int j = 0; j < columnNo; j++)
                    {
                        dataRpw[j] = coordinate[i, j];
                    }
                    CoordinateList.Add(dataRpw);
                }
                Length = CoordinateList.Count;

            }
            catch (FormatException)
            {

            }
        }

        public Double[,] ToDouleArray()
        {
            // Convert List<Double> type to Double type Array.
            var length = CoordinateList.Count;
            var result = new Double[length, 2];
            for (int i = 0; i < length; i++)
            {
                result[i, 0] = CoordinateList[i][0];
                result[i, 1] = CoordinateList[i][1];
            }
            return result;
        }

        public Coordinate this[int i]
        {
            get
            {
                return new Coordinate(CoordinateList[i]);
            }
        }

        /// <summary>
        /// Return the index corresponding to the minimum value.
        /// </summary>
        /// <param name="vs"></param>
        /// <param name="searchIndex"></param>
        /// <returns></returns>
        public static int? GetMinimumIndex(in List<double[]> vs, int searchIndex)
        {
            // Null check
            if (vs == null)
            {
                return null;
            }

            var minimumIndex = 0;
            var minimum = vs[0][searchIndex];
            for (int i = 1; i < vs.Count; i++)
            {
                if (vs[i][searchIndex] < minimum)
                {
                    minimum = vs[i][searchIndex];
                    minimumIndex = i;
                }
            }
            return minimumIndex;
        }
        public static int? GetMinimumIndex(in AirfoilCoordinate vs, int searchIndex)
        {
            var array = vs.ToDouleArray();
            var list = new List<Double[]>();

            for (int i = 0; i < array.GetLength(0); ++i)
            {
                var temp_array = new Double[2];
                temp_array[0] = array[i, 0];
                temp_array[1] = array[i, 1];

                list.Add(temp_array);
            }

            return GetMinimumIndex(list, searchIndex);
        }

        /// <summary>
        /// Return the index corresponding to the maximum value.
        /// </summary>
        /// <param name="vs"></param>
        /// <param name="searchIndex"></param>
        /// <returns></returns>
        public static int? GetMaximumIndex(in List<double[]> vs, int searchIndex)
        {
            // Null check
            if (vs == null)
            {
                return null;
            }

            var maximumIndex = 0;
            var maximum = vs[0][searchIndex];
            for (int i = 1; i < vs.Count; i++)
            {
                if (maximum < vs[i][searchIndex])
                {
                    maximum = vs[i][searchIndex];
                    maximumIndex = i;
                }
            }
            return maximumIndex;
        }
        public static int? GetMaximumIndex(in AirfoilCoordinate vs, int searchIndex)
        {
            var array = vs.ToDouleArray();
            var list = new List<Double[]>();

            for (int i = 0; i < array.GetLength(0); ++i)
            {
                var temp_array = new Double[2];
                temp_array[0] = array[i, 0];
                temp_array[1] = array[i, 1];

                list.Add(temp_array);
            }

            return GetMaximumIndex(list, searchIndex);
        }


        /// <summary>
        /// Return the minimum value.
        /// </summary>
        /// <param name="vs"></param>
        /// <param name="searchIndex"></param>
        /// <returns></returns>
        public static Double GetMinimumValue(in List<double[]> vs, int searchIndex)
        {
            // Null check
            if (vs == null)
            {
                throw new ArgumentNullException("Inputed List was Null.");
            }

            var minimumIndex = 0;
            var minimum = vs[0][searchIndex];
            for (int i = 1; i < vs.Count; i++)
            {
                if (vs[i][searchIndex] < minimum)
                {
                    minimum = vs[i][searchIndex];
                    minimumIndex = i;
                }
            }
            return minimum;
        }
        public static Double GetMinimumValue(in AirfoilCoordinate vs, int searchIndex)
        {
            var array = vs.ToDouleArray();
            var list = new List<Double[]>();

            for (int i = 0; i < array.GetLength(0); ++i)
            {
                var temp_array = new Double[2];
                temp_array[0] = array[i, 0];
                temp_array[1] = array[i, 1];

                list.Add(temp_array);
            }

            return GetMinimumValue(list, searchIndex);
        }

        /// <summary>
        /// Return the maximum value.
        /// </summary>
        /// <param name="vs"></param>
        /// <param name="searchIndex"></param>
        /// <returns></returns>
        public static Double GetMaximumValue(in List<double[]> vs, int searchIndex)
        {
            // Null check
            if (vs == null)
            {
                throw new ArgumentNullException("Inputed List was Null.");
            }

            var maximumIndex = 0;
            var maximum = vs[0][searchIndex];
            for (int i = 1; i < vs.Count; i++)
            {
                if (maximum < vs[i][searchIndex])
                {
                    maximum = vs[i][searchIndex];
                    maximumIndex = i;
                }
            }
            return maximum;
        }
        public static Double GetMaximumValue(in AirfoilCoordinate vs, int searchIndex)
        {
            var array = vs.ToDouleArray();
            var list = new List<Double[]>();

            for (int i = 0; i < array.GetLength(0); ++i)
            {
                var temp_array = new Double[2];
                temp_array[0] = array[i, 0];
                temp_array[1] = array[i, 1];

                list.Add(temp_array);
            }

            return GetMaximumValue(list, searchIndex);
        }
    }
}
