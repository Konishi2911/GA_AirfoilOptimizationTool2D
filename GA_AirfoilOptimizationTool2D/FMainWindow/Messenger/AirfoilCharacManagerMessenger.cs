using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FMainWindow.Messenger
{
    public class AirfoilCharacManagerMessenger
    {
        /// <summary>
        /// The event to start up the trigger CoefficientManagerTrigger.
        /// </summary>
        public event EventHandler ShowAirfoilCharacteristicsManager;

        public static String EventName
        {
            get
            {
                return nameof(ShowAirfoilCharacteristicsManager);
            }
        }

        /// <summary>
        /// This Class is singleton.
        /// </summary>
        public static AirfoilCharacManagerMessenger Instance { get; private set; } = new AirfoilCharacManagerMessenger();
        private AirfoilCharacManagerMessenger()
        {

        }

        public class AirfoilCharacManagerEventArgs : EventArgs
        {
        }

        public static void Show()
        {
            Instance.ShowAirfoilCharacteristicsManager?.Invoke(Instance, new AirfoilCharacManagerEventArgs());
        }
    }
}
