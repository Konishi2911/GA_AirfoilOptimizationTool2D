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
        private int numberOfBaseAirfoils;
        private List<Airfoil.AirfoilManager> baseAirfoils;
        #endregion

        public WorkingFileIO()
        {
            baseAirfoils = new List<Airfoil.AirfoilManager>();
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
        }
        /// <summary>
        /// Save current state and airfois data as a working file to the designated file path that is passed as a parameter.
        /// </summary>
        /// <param name="path">The File Path to Store the working file</param>
        private void SaveFile(String path)
        {

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
            Airfoil.AirfoilManager currentAirfoils = new Airfoil.AirfoilManager();
            Airfoil.AirfoilCoordinate currentCoordinate = new Airfoil.AirfoilCoordinate();
            List<Double[]> coordinateArray = new List<double[]>();

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

                }
                else if (IndexName == "NUMBER_OF_BASE_AIRFOILS")
                {
                    numberOfBaseAirfoils = int.Parse(FileStringLines[i]);
                }
                else if (IndexName == "BASE_AIRFOILS")
                {
                    if (SubIndexName == "END")
                    {
                        if (PreviousSubIndexName == "COORDINATE")
                        {
                            // Convert Coordinate List to Coordinate Array
                            double[,] tempArray = ConvertListToDoubleArray(coordinateArray);
                            // Import currentCoordinate Array
                            currentCoordinate.Import(tempArray);

                            // Create current airfoil manager
                            currentAirfoils = new Airfoil.AirfoilManager(currentCoordinate);
                            currentAirfoils.AirfoilName = currentAirfoilName;

                            // Add currentAirfoil to basisAirfoils
                            baseAirfoils.Add(currentAirfoils);
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
            }
        }

        private static double[,] ConvertListToDoubleArray(in List<double[]> coordinateArray)
        {
            var tempArray = new double[coordinateArray.Count, 2];
            for (int j = 0; j < coordinateArray.Count; j++)
            {
                tempArray[j, 0] = coordinateArray[j][0];
                tempArray[j, 1] = coordinateArray[j][1];
            }

            return tempArray;
        }
    }
}
