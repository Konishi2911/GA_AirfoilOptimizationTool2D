using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FMainWindow.Events
{
    class AirfoilCharacteristicsManagerTrigger : System.Windows.Interactivity.EventTrigger
    {
        public AirfoilCharacteristicsManagerTrigger() : base(Messenger.AirfoilCharacManagerMessenger.EventName)
        {
            SourceObject = Messenger.AirfoilCharacManagerMessenger.Instance;
        }
    }
}
