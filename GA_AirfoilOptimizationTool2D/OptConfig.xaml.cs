using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GA_AirfoilOptimizationTool2D
{
    /// <summary>
    /// OptConfig.xaml の相互作用ロジック
    /// </summary>
    public partial class OptConfig : Window
    {
        public OptConfig()
        {
            InitializeComponent();

            this.DataContext = new FOptConfig.OptConfigViewModel(new FOptConfig.Services.OpenFileDialogService());
        }
    }
}
