using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    class AirfoilCoordinateExporter
    {
        private String outputDirectory;

        public AirfoilCoordinateExporter(String outputPath)
        {
            this.outputDirectory = outputPath;
        }

        public void ExportAirfoilCoordinate(AirfoilManager airfoil, int resolution)
        {
            String CSV = General.CsvManager.CreateCSV(airfoil.InterpolatedCoordinate.ToDouleArray());

            String filePath = outputDirectory + "\\" + airfoil.AirfoilName + ".csv";
            using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(filePath, false, Encoding.UTF8))
            {
                streamWriter.Write(CSV);
            }
        }
    }
}
