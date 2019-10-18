using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General
{
    public static class CsvManager
    {
        public static String CreateCSV<T>(T[,] data)
        {
            String str = null;
            var length = data.GetLength(0);
            var width = data.GetLength(1);

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    str += data[i, j];
                    if (j != width - 1)
                    {
                        str += ",";
                    }
                }
                if (i != length - 1)
                {
                    str += "\r\n";
                }
            }

            return str;
        } 
    }
}
