using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace GA_AirfoilOptimizationTool2D.FMainWindow.Actions
{
    class ShowOptConfigDialogAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            (new OptConfig()).ShowDialog();
        }
    }
}
