using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig
{
    public class OptConfigViewModel : General.ViewModelBase
    {

        // Command Actions ========================
        private Action airfoilSelectionMethod;
        private Func<bool> isSelectable;
        // ========================================

        #region DelegateCommand Actions
        // DelegateCommand Action ==========================================================================
        private void ApplyButtonMethod()
        {
            
        }
        private Boolean IsAirfoilSelectable()
        {
            return false;
        }
        // ===============================================================================================
        #endregion

        public OptConfigViewModel()
        {
        }
    }
}
