using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    /// <summary>
    /// Represents combined airfoils group. Contains basis airfoils, coefficients of combination and coordinates of combined airfoils.
    /// </summary>
    public class CombinedAirfoilGroup : AirfoilGroupManagerBase
    {
        private General.BasisAirfoils basisAirfoils;
        private double[][] coefficientsOfCombination;

    }
}
