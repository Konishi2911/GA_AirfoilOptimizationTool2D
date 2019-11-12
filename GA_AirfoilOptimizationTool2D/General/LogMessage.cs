using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General
{
    public class LogMessage
    {
        private string _messages;

        public string Messages => _messages;

        public event EventHandler MessageUpdated;

        public void Write(string message)
        {
            _messages += message;
            MessageUpdated?.Invoke(this, new EventArgs());
        }
    }
}
