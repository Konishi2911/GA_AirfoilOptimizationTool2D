using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil.Characteristics
{
    public class AngleBasedCharacteristics
    {
        private int nData;
        private double[][] chr;
        private double[][] interpolatedChr;

        public AngleBasedCharacteristics()
        {
            nData = 0;
        }

        public AngleBasedCharacteristics(double[][] characteristics)
        {
            nData = characteristics.GetLength(0);
        }
    }
}
