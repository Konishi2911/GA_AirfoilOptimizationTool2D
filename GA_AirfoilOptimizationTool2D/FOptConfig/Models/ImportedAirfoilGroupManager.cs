using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Models
{
    public class ImportedAirfoilGroupManager : Airfoil.AirfoilGroupManagerBase
    {
        private List<Airfoil.AirfoilManager> airfoilGroup;

        public List<Airfoil.AirfoilManager> AirfoilGroup
        {
            get
            {
                return airfoilGroup;
            }
            set
            {
                // Issue PropertyChanged Condition.
                airfoilGroup = value;
                OnPropertyChanged(nameof(AirfoilGroup));
            }
        }

        public ImportedAirfoilGroupManager() { }
    }
}
