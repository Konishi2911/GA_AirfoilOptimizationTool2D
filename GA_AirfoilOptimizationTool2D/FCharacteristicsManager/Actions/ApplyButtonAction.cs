using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace GA_AirfoilOptimizationTool2D.FCharacteristicsManager.Actions
{
    class ApplyButtonAction : TriggerAction<FrameworkElement>
    {
        public ApplyButtonAction() { }
        protected override void Invoke(object parameter)
        {
            System.Windows.Window.GetWindow(AssociatedObject).Close();
        }
    }
}
