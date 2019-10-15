using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// Open the working file designated by the file path that is passed as a parameter, analyze them, and store them into the fields in this class.
        /// </summary>
        /// <param name="path">The File Path to Open the working file</param>
        private void OpenFile(String path)
        {

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
            string[] FileStringLines = openedFileString.Split("\r\n".ToCharArray());
            int numberOfLines = FileStringLines.Length;

            String IndexName = null;
            String SubIndexName = null;

            Airfoil.AirfoilManager currentAirfoils = new Airfoil.AirfoilManager();
            Airfoil.AirfoilCoordinate vurrentCoordinate = new Airfoil.AirfoilCoordinate();

            // Scroll Working File
            for (int i = 0; i < numberOfLines; i++)
            {
                // Search SubIndex Symbol
                if (FileStringLines[i].Contains("### "))
                {
                    SubIndexName = FileStringLines[i].Replace("### ", "");
                }
                // Search Index Symbol
                else if (FileStringLines[i].Contains("## "))
                {
                    // Get Index
                    IndexName = FileStringLines[i].Replace("## ", "");
                }

                else
                {
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

                        }
                        else if (SubIndexName == "NAME")
                        {
                            // Set Airfoils Name
                            currentAirfoils.AirfoilName = FileStringLines[i];
                        }
                        else if (SubIndexName == "COORDINATE")
                        {
                           
                        }
                    }
                }
            }
        }
    }
}
