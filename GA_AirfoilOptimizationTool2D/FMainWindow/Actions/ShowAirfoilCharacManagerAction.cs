using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FMainWindow.Actions
{
    public class ShowAirfoilCharacManagerAction : System.Windows.Interactivity.TriggerAction<System.Windows.DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            (new FCharacteristicsManager.CharacteristicsManager()).ShowDialog();
        }
    }
}
