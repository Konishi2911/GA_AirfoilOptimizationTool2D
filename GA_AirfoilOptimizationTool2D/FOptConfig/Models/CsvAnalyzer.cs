using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Models
{
    public class CsvAnalyzer
    {
        private static CsvAnalyzer Instance;
        private String filePath;
        private String csvStr;

        private static Double[,] DataArray { get; set; }

        public virtual void Analyze(String filePath)
        {
            DataArray = analyze(filePath);
        }

        protected Double[,] analyze(String filePath)
        {
            if (filePath == null) return null; 

            this.filePath = filePath;

            loadFile();

            var strLine = rowSeparator();

            return getDoubleArray(strLine);
        }

        public static CsvAnalyzer GetInstance()
        {
            if (Instance == null)
            {
                Instance = new CsvAnalyzer();
            }
            return Instance;
        }
        protected  CsvAnalyzer() { }

        private void loadFile()
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(filePath);
            csvStr = sr.ReadToEnd();
            sr.Close();
        }

        private String[] rowSeparator()
        {
            var newLine = new String[] { "\r\n" };
            return csvStr.Split(newLine, StringSplitOptions.RemoveEmptyEntries);
        }

        private Double[,] getDoubleArray(String[] strLine)
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
