using System;
using System.Collections.Generic;
using System.IO;

namespace GA_AirfoilOptimizationTool2D.FWorkingFileIO
{
    /// <summary>
    /// This class provides some functions to save or to open working file.
    /// </summary>
    public class WorkingFileIO
    {
        #region Fields
        private int numberOfSameGeneration;
        private int numberOfBaseAirfoils;
        private int numberOfGenerations;
        private List<Airfoil.AirfoilManager> baseAirfoils;
        private List<Airfoil.AirfoilManager> combinedAirfoils;
        private Double[,] coefficientOfCombination;
        #endregion
        public delegate void OpeningFileFinishedEventHandler(object sender, OpeningFileFinishedEventArgs e);
        public event OpeningFileFinishedEventHandler NotifyOpeningFileFinished;
        public class OpeningFileFinishedEventArgs : EventArgs
        {
            public int NumberOfSameGeneration { get; set; }
            public int NumberOfBaseAirfoils { get; set; }
            public int NumberOfGenerations { get; set; }
            public List<Airfoil.AirfoilManager> BaseAirfoils { get; set; }
            public List<Airfoil.AirfoilManager> CombinedAirfoils { get; set; }
            public Double[,] CoefficientOfCombination { get; set; }
        }

        public WorkingFileIO()
        {
            baseAirfoils = new List<Airfoil.AirfoilManager>();
            combinedAirfoils = new List<Airfoil.AirfoilManager>();
        }

        /// <summary>
        /// Open the working file designated by the file path that is passed as a parameter, analyze them, and store them into the fields in this class.
        /// </summary>
        /// <param name="path">The File Path to Open the working file</param>
        public async void OpenFile(String path)
        {
            String openedFileString;
            using (var reader = new StreamReader(path))
            {
                openedFileString = await reader.ReadToEndAsync();
            }
            AnalyzeFile(openedFileString);

            // Notify Opening the file finished.
            var e = new OpeningFileFinishedEventArgs()
            {
                NumberOfSameGeneration = this.numberOfSameGeneration,
                BaseAirfoils = this.baseAirfoils,
                CoefficientOfCombination = this.coefficientOfCombination,
                CombinedAirfoils = this.combinedAirfoils,
                NumberOfBaseAirfoils = this.numberOfBaseAirfoils,
                NumberOfGenerations = this.numberOfGenerations
            };
            NotifyOpeningFileFinished?.Invoke(this, e);
        }
        /// <summary>
        /// Save current state and airfois data as a working file to the designated file path that is passed as a parameter.
        /// </summary>
        /// <param name="path">The File Path to Store the working file</param>
        public async void SaveFile(String path)
        {
            const String NewLine = "\r\n";

            String writingString = null;

            writingString += CreateIndex("NUMBER_OF_SAME_GENERATION");
            writingString += GeneralConstants.NUMBER_OF_AIRFOILS_OF_GENERATION.ToString() + NewLine;
            writingString += EndPart();

            writingString += CreateIndex("NUMBER_OF_BASE_AIRFOILS");
            writingString += OptimizingConfiguration.Instance.BasisAirfoils.NumberOfAirfoils.ToString() + NewLine;
            writingString += EndPart();

            writingString += CreateIndex("BASE_AIRFOILS");
            foreach (var item in OptimizingConfiguration.Instance.BasisAirfoils.AirfoilGroup)
            {
                writingString += CreateSubIndex("NAME");
                writingString += item.AirfoilName + NewLine;
                writingString += EndSubPart();

                writingString += CreateSubIndex("COORDINATE");
                writingString += General.CsvManager.CreateCSV(item.ImportedCoordinate.ToDouleArray()) + NewLine;
                writingString += EndSubPart();
            }
            writingString += EndPart();

            writingString += CreateIndex("COEFFICIENT_OF_COMBINATION");
            writingString += General.CsvManager.CreateCSV(OptimizingConfiguration.Instance.CoefficientOfCombination) + NewLine;
            writingString += EndPart();




            using (var writer = new StreamWriter(path, false, System.Text.Encoding.UTF8))
            {
                await writer.WriteAsync(writingString);
            }
        }

        private String CreateIndex(String indexName)
        {
            return "## " + indexName + "\r\n";
        }
        private String CreateSubIndex(String indexName)
        {
            return "### " + indexName + "\r\n";
        }
        private String EndPart()
        {
            return "## END" + "\r\n";
        }
        private String EndSubPart()
        {
            return "### END" + "\r\n";
        }
        /// <summary>
        /// Analyze the opened working file that type is String to classify it into each parameter.
        /// </summary>
        /// <param name="openedFileString">The string that is opened working file</param>
        private void AnalyzeFile(String openedFileString)
        {
            // Divide opened file by new line charactor
            string[] FileStringLines = openedFileString.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int numberOfLines = FileStringLines.Length;

            String IndexName = null;
            String PreviousIndexName = null;
            String SubIndexName = null;
            String PreviousSubIndexName = null;

            String currentAirfoilName = null;
            List<Double[]> coordinateArray = new List<double[]>();
            List<Double[]> coefficientArray = new List<double[]>();

            // Scroll Working File
            for (int i = 0; i < numberOfLines; i++)
            {
                // Search SubIndex Symbol
                if (FileStringLines[i].Contains("### "))
                {
                    PreviousSubIndexName = SubIndexName;
                    SubIndexName = FileStringLines[i].Replace("### ", "");
                    if (SubIndexName != "END")
                    {
                        continue;
                    }
                }
                // Search Index Symbol
                else if (FileStringLines[i].Contains("## "))
                {
                    // Set previous Index Name
                    PreviousIndexName = IndexName;
                    // Get Index
                    IndexName = FileStringLines[i].Replace("## ", "");
                    if (IndexName != "END")
                    {
                        continue;
                    }
                }

                // Analyze Body of Text
                if (IndexName == "END")
                {
                    if (PreviousIndexName == "COEFFICIENT_OF_COMBINATION")
                    {
                        coefficientOfCombination = ConvertListToDoubleArray(coefficientArray);
                    }
                }
                else if (IndexName == "NUMBER_OF_SAME_GENERATION")
                {
                    numberOfSameGeneration = int.Parse(FileStringLines[i]);
                }
                else if (IndexName == "NUMBER_OF_BASE_AIRFOILS")
                {
                    numberOfBaseAirfoils = int.Parse(FileStringLines[i]);
                }
                else if (IndexName == "NUMBER_OF_GENERATIONS")
                {
                    numberOfGenerations = int.Parse(FileStringLines[i]);
                }
                else if (IndexName == "BASE_AIRFOILS")
                {
                    // Finalize Procedure
                    if (SubIndexName == "END")
                    {
                        if (PreviousSubIndexName == "COORDINATE")
                        {
                            // Add current Airfoils to baseAirfoil List
                            AddAirfoilToList(currentAirfoilName, coordinateArray, ref baseAirfoils);
                            currentAirfoilName = "";
                            coordinateArray.Clear();
                        }
                    }
                    else if (SubIndexName == "NAME")
                    {
                        // Set Airfoils Name
                        currentAirfoilName = FileStringLines[i];
                    }
                    else if (SubIndexName == "COORDINATE")
                    {
                        var coordinateStr = FileStringLines[i].Split(',');
                        coordinateArray.Add(new double[2] { double.Parse(coordinateStr[0]), double.Parse(coordinateStr[1]) });
                    }
                }
                else if (IndexName == "COEFFICIENT_OF_COMBINATION")
                {
                    var coefficientStr = FileStringLines[i].Split(',');
                    var numberOfSameGeneration = coefficientStr.Length;
                    var coefficientRow = new double[numberOfSameGeneration];

                    for (int j = 0; j < numberOfSameGeneration; j++)
                    {
                        coefficientRow[j] = Double.Parse(coefficientStr[j]);
                    }

                    coefficientArray.Add(coefficientRow);
                }
            }
        }

        /// <summary>
        /// Add the airfoil passed as a parameter to the List passed as a reference parameter.
        /// </summary>
        /// <param name="currentAirfoilName">The name of an Airfoil to add</param>
        /// <param name="coordinateArray"> The coordinate Array of an Airfoil to add</param>
        /// <param name="airfoilList">The reference of the airfoilList where coordinateArray is added</param>
        private void AddAirfoilToList(in string currentAirfoilName, in List<double[]> coordinateArray, ref List<Airfoil.AirfoilManager> airfoilList)
        {
            Airfoil.AirfoilCoordinate currentCoordinate = new Airfoil.AirfoilCoordinate();

            // Convert Coordinate List to Coordinate Array
            double[,] tempArray = ConvertListToDoubleArray(coordinateArray);
            // Import currentCoordinate Array
            currentCoordinate.Import(tempArray);

            // Create current airfoil manager
            Airfoil.AirfoilManager currentAirfoils = new Airfoil.AirfoilManager(currentCoordinate)
            {
                AirfoilName = currentAirfoilName
            };

            // Add currentAirfoil to basisAirfoils
            airfoilList.Add(currentAirfoils);
        }

        private static double[,] ConvertListToDoubleArray(in List<double[]> coordinateArray)
        {
            var length = coordinateArray.Count;
            var width = coordinateArray[0].Length;

            var tempArray = new double[length, width];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tempArray[i, j] = coordinateArray[i][j];
                }
            }

            return tempArray;
        }
    }
}
