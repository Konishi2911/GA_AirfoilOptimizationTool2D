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
            var FileStringLines = openedFileString.Split("\r\n".ToCharArray());
        }
    }
}
