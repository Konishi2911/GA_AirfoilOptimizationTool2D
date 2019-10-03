using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FMainWindow
{
    class MainWindowViewModel:General.ViewModelBase
    {
        private General.DelegateCommand showOprConfigDialog;

        public MainWindowViewModel()
        {
            var ImportedAirfoil = FOptConfig.Models.ImportedAirfoilGroupManager.Instance;

            openOptConfigDialog = new Action(OpenOptimizingConfigurationDialog);
            isOptConfigEnabled = new Func<bool>(IsOptConfigDialogEnabled);
            showOprConfigDialog = new General.DelegateCommand(openOptConfigDialog, isOptConfigEnabled);
        }

        public Action openOptConfigDialog;
        public Func<bool> isOptConfigEnabled;

        public General.DelegateCommand ShowOptConfigDialog
        {
            get { return showOprConfigDialog; }
        }

        public void OpenOptimizingConfigurationDialog()
        {
            // Issue the Messenger displaying OpenFileDialog.
            Messenger.OptConfigDialogMessenger.Show();
        }
        public bool IsOptConfigDialogEnabled()
        {
            return true;
        }
    }
}
