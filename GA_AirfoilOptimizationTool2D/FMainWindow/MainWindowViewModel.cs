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
        private General.IShowUserDialogService<FOptConfig.OptConfigViewModel> _showOptConfigDialogService;
        private FOptConfig.Models.ImportedAirfoilGroupManager importedAirfoil;

        public MainWindowViewModel()
        {
            importedAirfoil = FOptConfig.Models.ImportedAirfoilGroupManager.Instance;

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
            var optConfigDialog = new FOptConfig.OptConfigViewModel(new FOptConfig.Services.OpenFileDialogService());
            //this._showOptConfigDialogService.ShowDialog(optConfigDialog);

            // Issue the Messenger displaying OpenFileDialog.
            Messenger.OptConfigDialogMessenger.Show();
        }
        public bool IsOptConfigDialogEnabled()
        {
            return true;
        }
    }
}
