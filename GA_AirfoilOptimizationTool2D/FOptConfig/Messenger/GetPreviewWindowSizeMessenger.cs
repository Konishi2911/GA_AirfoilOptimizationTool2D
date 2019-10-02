using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Messenger
{
    class GetPreviewWindowSizeMessenger
    {
        public event EventHandler<EventArgs> GetPreviewWindowSize;

        /// <summary>
        /// This Class is Singleton.
        /// </summary>
        public static GetPreviewWindowSizeMessenger Instance { get; private set; } = new GetPreviewWindowSizeMessenger();

        public static String EventName
        {
            get
            {
                return "GetPreviewWindowSizeMessenger";
            }
        }

        public class GetPreviewWindowSizeEventArgs : EventArgs
        {
            public Action<Double[]> GetWindowSizeNotifyResult { get; set; }
        }
        /// <summary>
        /// Get Preview Window Size
        /// </summary>
        /// <returns>Index 0 is width, Index 1 is height</returns>
        public static Double[] GetSize()
        {
            Double[] windowSize = new Double[2];

            Instance.GetPreviewWindowSize?.Invoke(Instance, new GetPreviewWindowSizeMessenger.GetPreviewWindowSizeEventArgs()
            {
                GetWindowSizeNotifyResult = result =>
                {
                    windowSize = result;
                }
            });

            return windowSize;
        }
    }
}
