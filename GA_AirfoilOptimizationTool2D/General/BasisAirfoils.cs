using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General
{
    public class BasisAirfoils : Airfoil.AirfoilGroupManagerBase
    {
        private int numberOfBasisAirfoils;

        public int NumberOfBasisAirfoils
        {
            get => numberOfBasisAirfoils;
            set
            {
                numberOfBasisAirfoils = value;
                OnPropertyChanged(nameof(this.NumberOfBasisAirfoils));
            }
        }

        public BasisAirfoils() : base()
        { 
        }
        public BasisAirfoils(ICollection<Airfoil.AirfoilManager> airfoils) : this()
        {
            NumberOfAirfoils = airfoils.Count;
            foreach (var item in airfoils)
            {
                this.AirfoilGroup.Add(item);
            }
        }

        public static BasisAirfoils Convert(Airfoil.IAirfoilGroupManager airfoilGroup)
        {
            BasisAirfoils temp = new BasisAirfoils
            {
                AirfoilGroup = airfoilGroup.AirfoilGroup,
                NumberOfAirfoils = airfoilGroup.NumberOfAirfoils,
            };

            return temp;
        }

        public object Clone()
        {
            var temp = new BasisAirfoils
            {
                AirfoilGroup = this.AirfoilGroup,
                NumberOfAirfoils = this.NumberOfAirfoils,
                NumberOfBasisAirfoils = this.NumberOfBasisAirfoils
            };

            return temp;
        }
    }
}
