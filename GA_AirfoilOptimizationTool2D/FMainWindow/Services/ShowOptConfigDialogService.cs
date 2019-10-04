using System.Windows;

namespace GA_AirfoilOptimizationTool2D.FMainWindow.Services
{
    public class ShowOptConfigDialogService<TViewModel> : General.IShowUserDialogService<TViewModel>
    {
        public Window Owner { get; set; }

        public bool? ShowDialog(TViewModel context)
        {
            var OptConfig = new OptConfig()
            {
                Owner = this.Owner,
                DataContext = context
            };

            return OptConfig.ShowDialog();
        }
    }
}
