using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    public class AirfoilCoordinate
    {
        public List<Double[]> CoordinateList = new List<double[]>();

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

    }
}
