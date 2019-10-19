using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General.Messenger
{
    /// <summary>
    /// This Class is singleton.
    /// </summary>
    class SaveFileMessenger
    {
        /// <summary>
        /// The event to start up the trigger OpenFileDialogTrigger
        /// </summary>
        public event EventHandler<EventArgs> ShowSaveFileDialog;

        public static String EventName
        {
            get
            {
                return "ShowSaveFileDialog";
            }
        }

        /// <summary>
        /// This Class is singleton.
        /// </summary>
        public static SaveFileMessenger Instance { get; private set; }

        public static SaveFileMessenger GetNewInstance()
        {
            Instance = new SaveFileMessenger();
            return Instance;
        }
        public SaveFileMessenger()
        {

        }

        public class SaveFileEventArgs : EventArgs
        {
            public String Filter { get; set; }

            public Action<String> SaveFileDialogNotifyResult { get; set; }
        }

        public static String Show(String fileTypeFilter)
        {
            String filePath = null;

            Instance.ShowSaveFileDialog?.Invoke(Instance, new SaveFileMessenger.SaveFileEventArgs()
            {
                Filter = fileTypeFilter,

                //Receive the result by Callback
                SaveFileDialogNotifyResult = result =>
                {
                    filePath = result;
                }
            });

            return filePath;
        }
    }
}
