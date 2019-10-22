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

        private OptConfigDelegateCommand applyButton;

        #region DelegateCommand Actions
        // DelegateCommand Action ==========================================================================
        private void ApplyButtonMethod()
        {
            OptimizingConfiguration.Instance.BasisAirfoils = Models.ImportedAirfoilGroupManager.GetCurrentInstance().Convert<General.BasisAirfoils>();
        }
        private Boolean IsAvailableApplyButton()
        {
            var currentInstance = Models.ImportedAirfoilGroupManager.GetCurrentInstance();
            if (currentInstance == null || currentInstance.NumberOfAirfoils == 0)
            {
                return false;
            }
            return (currentInstance.NumberOfAirfoils == currentInstance.NumberOfBasisAirfoils);
        }
        // ================================================================================================
        #endregion

        public OptConfigViewModel()
        {
            applyButton = new OptConfigDelegateCommand(ApplyButtonMethod, IsAvailableApplyButton);
        }

        public OptConfigDelegateCommand ApplyButtonCommand
        {
            get
            {
                return applyButton;
            }
        }
    }
}
