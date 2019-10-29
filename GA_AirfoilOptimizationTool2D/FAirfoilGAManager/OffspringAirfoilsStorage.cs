using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FAirfoilGAManager
{
    class OffspringAirfoilsStorage
    {
        #region Fields
        private bool isUnderEvaluation;
        private Airfoil.CombinedAirfoilsGroupManager offspringAirfoils;
        #endregion

        public static OffspringAirfoilsStorage Instance => new OffspringAirfoilsStorage();
        // This class is Singleton
        private OffspringAirfoilsStorage() { }

        #region Properties
        public Airfoil.CombinedAirfoilsGroupManager OffspringAirfoils => offspringAirfoils;
        #endregion
    }
}
