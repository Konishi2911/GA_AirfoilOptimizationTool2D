using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Actions
{
    class ShowFileDialogAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            // Create the instance of OpenFileDialog
            Microsoft.Win32.OpenFileDialog _ofd = new Microsoft.Win32.OpenFileDialog();

            // Verify the event parameters
            var openFileDialogArgs = parameter as Messenger.OpenFileMessenger.OpenFileEventArgs;
            if (openFileDialogArgs == null)
            {
                return;
            }

            // Set file type filter
            _ofd.Filter = openFileDialogArgs.Filter;

            // Show OpenFileDialog
            var ofdResults = _ofd.ShowDialog();

            // Get the result of the OpenFileDialog
            String _path = null;
            if (ofdResults == true)
            {
                _path = _ofd.FileName;
            }
            else if (ofdResults == false)
            {
                _path = null;
            }
            else
            {
                
            }

            // Notify the result by Callback
            openFileDialogArgs.OpenFileDialogNotifyResult?.Invoke(_path);
        }
    }
}
