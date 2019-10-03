using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FMainWindow.Events
{
    class OptConfigDialogTrigger : System.Windows.Interactivity.EventTrigger
    {
        public OptConfigDialogTrigger() : base(Messenger.OptConfigDialogMessenger.EventName)
        {
            SourceObject = Messenger.OptConfigDialogMessenger.Instance;
        }
    }
}
