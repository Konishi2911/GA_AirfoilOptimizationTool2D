using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Models
{
    /// <summary>
    /// This class is singleton.
    /// </summary>
    public class AirfoilCsvAnalyzer : CsvAnalyzer
    {
        private static AirfoilCsvAnalyzer Instance;
        private Airfoil.AirfoilCoordinate airfoilCoordinate;

        public Airfoil.AirfoilCoordinate AirfoilCoordinate
        {
            get
            {
                return airfoilCoordinate;
            }
            private set
            {
                airfoilCoordinate = value;
            }
        }

        public new Airfoil.AirfoilCoordinate Analyze(String filePath)
        {
            //instantiate
            AirfoilCoordinate = new Airfoil.AirfoilCoordinate();

            // Get Double type Array by analyzing CSV file
            var dataArray = base.analyze(filePath);
            if (dataArray == null) return null;              //null check

            //Create AirfoilCoordinate
            AirfoilCoordinate.Import(dataArray);
            return AirfoilCoordinate;
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
