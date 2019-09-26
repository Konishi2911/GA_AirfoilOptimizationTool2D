using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Models
{
    public class AirfoilCsvAnalyzer : CsvAnalyzer
    {
        private static AirfoilCsvAnalyzer Instance;
        public Airfoil.AirfoilCoordinate AirfoilCoordinate { get; private set; } = new Airfoil.AirfoilCoordinate();

        public override void Analyze(String filePath)
        { 
            // Get Double type Array by analyzing CSV file
            var dataArray = base.analyze(filePath);
            if (dataArray == null) return;              //null check

            //Create AirfoilCoordinate
            AirfoilCoordinate.Import(dataArray);
        }

        public static new AirfoilCsvAnalyzer GetInstance()
        {
            if (Instance == null)
            {
                Instance = new AirfoilCsvAnalyzer();
            }
            return Instance;
        }
        private AirfoilCsvAnalyzer()
        {

        }
    }
}
