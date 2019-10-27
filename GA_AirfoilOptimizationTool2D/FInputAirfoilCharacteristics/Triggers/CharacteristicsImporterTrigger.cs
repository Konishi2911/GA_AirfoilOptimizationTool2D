using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FInputAirfoilCharacteristics.Triggers
{
    public class CharacteristicsImporterTrigger : System.Windows.Interactivity.EventTrigger
    {
        public CharacteristicsImporterTrigger() : base(Messenger.CharacteristicsImporterMessenger.EventName)
        {
            SourceObject = Messenger.CharacteristicsImporterMessenger.Instance;
        }
    }
}
