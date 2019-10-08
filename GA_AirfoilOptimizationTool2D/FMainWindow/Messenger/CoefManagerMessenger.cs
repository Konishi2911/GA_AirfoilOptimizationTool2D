using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FMainWindow.Messenger
{
    class CoefManagerMessenger
    {
        /// <summary>
        /// The event to start up the trigger CoefficientManagerTrigger.
        /// </summary>
        public event EventHandler ShowCoefficientManager;

        public static String EventName
        {
            get
            {
                return nameof(ShowCoefficientManager);
            }
        }

        /// <summary>
        /// This Class is singleton.
        /// </summary>
        public static CoefManagerMessenger Instance { get; private set; } = new CoefManagerMessenger();
        private CoefManagerMessenger()
        {

        }

        public class CoefficientManagerEventArgs : EventArgs
        {
        }

        public static void Show()
        {
            Instance.ShowCoefficientManager?.Invoke(Instance, new CoefficientManagerEventArgs());
        }
    }
}
