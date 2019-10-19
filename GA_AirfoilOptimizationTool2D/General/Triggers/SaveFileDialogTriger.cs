using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General.Event
{
    class SaveFileDialogTrigger : System.Windows.Interactivity.EventTrigger
    {
        public SaveFileDialogTrigger() : base(General.Messenger.SaveFileMessenger.EventName)
        {
            SourceObject = General.Messenger.SaveFileMessenger.GetNewInstance();
        }
    }
}
