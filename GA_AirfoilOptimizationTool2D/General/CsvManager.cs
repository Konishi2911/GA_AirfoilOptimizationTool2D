using System;
using System.Collections.Generic;

namespace GA_AirfoilOptimizationTool2D.General
{
    public static class CsvManager
    {
        public static String CreateCSV<T>(T[] data, bool isVartical)
        {
            T[,] array;
            if (isVartical == false)
            {
                array = new T[1, data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    array[0, i] = data[i];
                }
            }
            else
            {
                array = new T[data.Length, 1];
                for (int i = 0; i < data.Length; i++)
                {
                    array[i, 0] = data[i];
                }
            }

            return CreateCSV(array);
        }
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

        public static Double[,] ConvertCsvToArray(String csv)
        {
            // Null Check
            if (csv == null)
            {
                return null;
            }
            var lineCsv = rowSeparator(csv);
            return getDoubleArray(lineCsv);
        }

        private static String[] rowSeparator(String source)
        {
            var newLine = new String[] { "\r\n" };
            return source.Split(newLine, StringSplitOptions.RemoveEmptyEntries);
        }

        private static Double[,] getDoubleArray(String[] strLine)
        {
            var strList = new List<List<String>>();
            var dataList = new List<List<Double>>();
            var delimiter = new String[] { "," };

            // Create String type Array
            foreach (var row in strLine)
            {
                var strItems = row.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                strList.Add(new List<String>(strItems));
            }
            // -------------------------

            // Convert String type List to Double type List
            foreach (var row in strList)
            {
                var dataRow = new List<Double>();
                foreach (var item in row)
                {
                    dataRow.Add(Convert.ToDouble(item));
                }
                dataList.Add(dataRow);
            }
            // ---------------------------------------------

            // Convert DataList To DataArray
            var numberOfRow = dataList.Count;
            var numberOfColumn = dataList[0].Count;
            var dataArray = new Double[numberOfRow, numberOfColumn];

            try
            {
                for (int i = 0; i < numberOfRow; i++)
                {
                    var dataRow = dataList[i].ToArray();
                    if (dataRow.Length != numberOfColumn)
                    {
                        throw new FormatException("Invalid CSV Format");
                    }

                    for (int j = 0; j < numberOfColumn; j++)
                    {
                        dataArray[i, j] = dataRow[j];
                    }
                }
            }
            catch (FormatException)
            {

            }
            // ---------------------------------

            return dataArray;
        }
    }
}
