using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Event
{
    class OpenFileDialogTrigger : System.Windows.Interactivity.EventTrigger
    {
        public OpenFileDialogTrigger() : base(Messenger.OpenFileMessenger.EventName)
        {
            SourceObject = Messenger.OpenFileMessenger.GetNewInstance();
        }
    }
}
