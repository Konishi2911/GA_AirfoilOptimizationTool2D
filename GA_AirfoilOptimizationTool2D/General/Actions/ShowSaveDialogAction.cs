using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace GA_AirfoilOptimizationTool2D.General.Actions
{
    class ShowSaveDialogAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            // Create the instance of OpenFileDialog
            Microsoft.Win32.SaveFileDialog _sfd = new Microsoft.Win32.SaveFileDialog();

            // Verify the event parameters
            var saveFileDialogArgs = parameter as General.Messenger.SaveFileMessenger.SaveFileEventArgs;
            if (saveFileDialogArgs == null)
            {
                return;
            }

            // Set file type filter
            _sfd.Filter = saveFileDialogArgs.Filter;

            // Show OpenFileDialog
            var ofdResults = _sfd.ShowDialog();

            // Get the result of the OpenFileDialog
            String _path = null;
            if (ofdResults == true)
            {
                _path = _sfd.FileName;
            }
            else if (ofdResults == false)
            {
                _path = null;
            }
            else
            {
                
            }

            // Notify the result by Callback
            saveFileDialogArgs.SaveFileDialogNotifyResult?.Invoke(_path);
        }
    }
}
