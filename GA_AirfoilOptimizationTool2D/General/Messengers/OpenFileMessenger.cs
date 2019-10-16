using System;

namespace GA_AirfoilOptimizationTool2D.General.Messenger
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

        /// <summary>
        /// This Class is singleton.
        /// </summary>
        public static OpenFileMessenger Instance { get; private set; }

        public static OpenFileMessenger GetNewInstance()
        {
            Instance = new OpenFileMessenger();
            return Instance;
        }
        public OpenFileMessenger()
        {

        }

        public class OpenFileEventArgs : EventArgs
        {
            public String Filter { get; set; }

            public Action<String> OpenFileDialogNotifyResult { get; set; }
        }

        public static String Show(String fileTypeFilter)
        {
            String filePath = null;

            Instance.ShowOpenFileDialog?.Invoke(Instance, new OpenFileMessenger.OpenFileEventArgs()
            {
                Filter = fileTypeFilter,

                //Receive the result by Callback
                OpenFileDialogNotifyResult = result =>
                {
                    filePath = result;
                }
            }) ;

            return filePath;
        }
    }
}
