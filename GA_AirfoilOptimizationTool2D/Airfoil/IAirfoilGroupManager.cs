using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    public interface IAirfoilGroupManager
    {
        double NumberOfAirfoils { get; set; }
        List<Airfoil.AirfoilManager> AirfoilGroup { get; set; }

        /// <summary>
        /// Add airfoil.
        /// </summary>
        /// <param name="specification"></param>
        void Add(AirfoilManager specification);

        /// <summary>
        /// Add airfoil.
        /// </summary>
        /// <param name="coordinate"></param>
        void Add(AirfoilCoordinate coordinate);

        void Remove(AirfoilManager airfoil, String label);

        /// <summary>
        /// Get element of airfoil that selected by index.
        /// If the List of airfoils is empty, return null to caller.
        /// </summary>
        /// <param name="index"> index if airfoils </param>
        /// <returns></returns>
        Airfoil.AirfoilManager GetAirfoil(int index);
    }
}
