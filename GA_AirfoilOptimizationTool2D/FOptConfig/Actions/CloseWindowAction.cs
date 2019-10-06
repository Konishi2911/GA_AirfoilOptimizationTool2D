using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Actions
{
    public class CloseWindowAction : TriggerAction<FrameworkElement>
    {
        public CloseWindowAction() { }
        protected override void Invoke(object parameter)
        {
            System.Windows.Window.GetWindow(AssociatedObject).Close();
        }
    }
}
