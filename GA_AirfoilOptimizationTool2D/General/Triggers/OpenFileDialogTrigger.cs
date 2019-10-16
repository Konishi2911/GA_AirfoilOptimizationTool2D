using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General.Event
{
    class OpenFileDialogTrigger : System.Windows.Interactivity.EventTrigger
    {
        public OpenFileDialogTrigger() : base(General.Messenger.OpenFileMessenger.EventName)
        {
            SourceObject = General.Messenger.OpenFileMessenger.GetNewInstance();
        }
    }
}
