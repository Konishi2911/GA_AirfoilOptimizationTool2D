using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig
{
    public class AirfoilSelectorViewModel : General.ViewModelBase
    {
        public Airfoil.AirfoilManager SelectedAirfoil { get; private set; }
        public String Label { get; private set; }

        public AirfoilSelectorViewModel(Airfoil.AirfoilManager airfoil, String label)
        {
            this.SelectedAirfoil = airfoil;
            this.Label = label;
        }

        public static AirfoilSelectorViewModel Create(Airfoil.AirfoilManager airfoil, Dictionary<Airfoil.AirfoilManager, String> airfoilsMap)
        {
            return new AirfoilSelectorViewModel(airfoil, airfoilsMap[airfoil]);
        }
    }
}
