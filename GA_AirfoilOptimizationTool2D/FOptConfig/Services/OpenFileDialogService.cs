using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Services
{
    public class OpenFileDialogService
    {
        public String ShowDialog(String filter)
        {
            // Create the instance of OpenFileDialog
            Microsoft.Win32.OpenFileDialog _ofd = new Microsoft.Win32.OpenFileDialog();

            // Set file type filter
            _ofd.Filter = filter;

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
            return _path;
        }
    }
}
