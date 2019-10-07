using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FCoefManager.Models
{
    class CoordinateOfConbination : General.ModelBase
    {
        private ObservableCollection<EachCoefficients> _coordinates;

        public ObservableCollection<EachCoefficients> Coordinates
        {
            get
            {
                return _coordinates;
            }
            set
            {
                _coordinates = value;
                OnPropertyChanged(nameof(Coordinates));
            }
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }
    }
}
