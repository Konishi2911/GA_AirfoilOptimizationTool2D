using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FMainWindow.Messenger
{
    class OptConfigDialogMessenger
    {
        /// <summary>
        /// The event to start up the trigger OptConfigDialogTrigger
        /// </summary>
        public event EventHandler ShowOptconfigDialog;

        public static String EventName
        {
            get
            {
                return nameof(ShowOptconfigDialog);
            }
        }

        /// <summary>
        /// This Class is singleton.
        /// </summary>
        public static OptConfigDialogMessenger Instance { get; private set; } = new OptConfigDialogMessenger();
        private OptConfigDialogMessenger()
        {

        }

        public class OptConfigDialogEventArgs : EventArgs
        {
            public String Filter { get; set; }

            public Action<String> OptConfigDialogNotifyResult { get; set; }
        }

        public static void Show()
        {
            Instance.ShowOptconfigDialog?.Invoke(Instance, new OptConfigDialogMessenger.OptConfigDialogEventArgs());
        } 
    }
}
