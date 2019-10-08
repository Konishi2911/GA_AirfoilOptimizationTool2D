using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FMainWindow.Events
{
    class CoefManagerTrigger : System.Windows.Interactivity.EventTrigger
    {
        public CoefManagerTrigger() : base(Messenger.CoefManagerMessenger.EventName)
        {
            SourceObject = Messenger.CoefManagerMessenger.Instance;
        }
    }
}
