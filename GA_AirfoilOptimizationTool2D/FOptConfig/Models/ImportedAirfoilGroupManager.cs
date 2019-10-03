using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Models
{
    public sealed class ImportedAirfoilGroupManager : Airfoil.AirfoilGroupManagerBase
    {
        public static ImportedAirfoilGroupManager Instance { get; } = new ImportedAirfoilGroupManager();
        /// <summary>
        /// This class is Singleton
        /// </summary>
        private ImportedAirfoilGroupManager() { }
    }
}
