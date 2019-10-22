using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FGeneticAlgorithm
{
    public class GAAirfoilsGroup : Airfoil.AirfoilGroupManagerBase
    {
        private int numberOfBasisAirfoils;

        public int NumberOfBasisAirfoils => numberOfBasisAirfoils;
    }
}
