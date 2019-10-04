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
