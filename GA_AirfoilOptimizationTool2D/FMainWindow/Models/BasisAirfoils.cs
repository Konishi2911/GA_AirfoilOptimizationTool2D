using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FMainWindow.Models
{
    public class BasisAirfoils : Airfoil.AirfoilGroupManagerBase
    {
        public static BasisAirfoils Convert(Airfoil.IAirfoilGroupManager airfoilGroup)
        {
            BasisAirfoils temp = new BasisAirfoils
            {
                AirfoilGroup = airfoilGroup.AirfoilGroup,
                NumberOfAirfoils = airfoilGroup.NumberOfAirfoils,
                NumberOfBasisAirfoils = airfoilGroup.NumberOfBasisAirfoils
            };

            return temp;
        }
    }
}
