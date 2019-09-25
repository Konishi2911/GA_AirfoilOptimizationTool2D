using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Models
{
    public class AirfoilCsvAnalyzer : CsvAnalyzer
    {
        public AirfoilCoordinate AirfoilCoordinate { get; private set; } = new AirfoilCoordinate();

        public override void Analyze(String filePath)
        { 
            // Get Double type Array by analyzing CSV file
            var dataArray = base.analyze(filePath);
            if (dataArray == null) return;              //null check

            //Create AirfoilCoordinate
            AirfoilCoordinate.Import(dataArray);
        }
    }
}
