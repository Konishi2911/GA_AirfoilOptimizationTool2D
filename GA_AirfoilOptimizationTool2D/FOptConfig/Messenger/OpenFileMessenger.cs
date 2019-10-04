using System;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Messenger
{
    /// <summary>
    /// This Class is singleton.
    /// </summary>
    class OpenFileMessenger
    {
        /// <summary>
        /// The event to start up the trigger OpenFileDialogTrigger
        /// </summary>
        public event EventHandler<EventArgs> ShowOpenFileDialog;

        public static String EventName
        {
            get
            {
                return "ShowOpenFileDialog";
            }
        }

        private static OpenFileMessenger _instance;
        /// <summary>
        /// This Class is singleton.
        /// </summary>
        public static OpenFileMessenger Instance
        {
            get
            {
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        public static OpenFileMessenger GetNewInstance()
        {
            _instance = new OpenFileMessenger();
            return _instance;
        }
        public OpenFileMessenger()
        {

        }

        public class OpenFileEventArgs : EventArgs
        {
            public String Filter { get; set; }

            public Action<String> OpenFileDialogNotifyResult { get; set; }
        }

        public static String Show()
        {
            String filePath = null;

            Instance.ShowOpenFileDialog?.Invoke(Instance, new OpenFileMessenger.OpenFileEventArgs()
            {
                Filter = "CSV File (*.csv)|*.csv",

                //Receive the result by Callback
                OpenFileDialogNotifyResult = result =>
                {
                    filePath = result;
                }
            });

            return filePath;
        }
    }
}
