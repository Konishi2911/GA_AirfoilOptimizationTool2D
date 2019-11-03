using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FCharacteristicsManager
{
    class TargetAirfoilSelectorViewModel
    {
        public Airfoil.AirfoilManager SelectedAirfoil { get; private set; }
        public String Label { get; private set; }

        public TargetAirfoilSelectorViewModel(Airfoil.AirfoilManager airfoil, String label)
        {
            this.SelectedAirfoil = airfoil;
            this.Label = label;
        }

        public static TargetAirfoilSelectorViewModel Create(Airfoil.AirfoilManager airfoil, Dictionary<Airfoil.AirfoilManager, String> airfoilsMap)
        {
            return new TargetAirfoilSelectorViewModel(airfoil, airfoilsMap[airfoil]);
        }
    }
}
