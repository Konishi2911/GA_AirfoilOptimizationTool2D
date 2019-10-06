using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D
{
    public class OptimizingConfiguration : General.ModelBase
    {
        private Airfoil.IAirfoilGroupManager _basisAirfoils;

        /// <summary>
        /// This Class is Singleton
        /// </summary>
        private OptimizingConfiguration() { }
        public static OptimizingConfiguration Instance { get; private set; } = new OptimizingConfiguration();

        public Airfoil.IAirfoilGroupManager BasisAirfoils
        {
            get
            {
                return _basisAirfoils;
            }
            set
            {
                _basisAirfoils = value;
            }
        }
    }
}
