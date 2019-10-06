using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General.Event
{
    class GetPreviewWindowSizeTrigger : System.Windows.Interactivity.EventTrigger
    {
        public GetPreviewWindowSizeTrigger() : base(Messenger.GetPreviewWindowSizeMessenger.EventName)
        {
            SourceObject = Messenger.GetPreviewWindowSizeMessenger.Instance;
        }
    }
}
