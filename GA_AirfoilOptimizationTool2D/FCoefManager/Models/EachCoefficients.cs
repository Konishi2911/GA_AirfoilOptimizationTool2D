using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FCoefManager.Models
{
    public class EachCoefficients : General.ModelBase
    {
        private Double[] airfoils = new double[10];

        public Double Airfoil1
        {
            get => airfoils[0];
            set
            {
                airfoils[0] = value;
                OnPropertyChanged(nameof(Airfoil1));
            }
        }
        public Double Airfoil2
        {
            get => airfoils[1];
            set
            {
                airfoils[1] = value;
                OnPropertyChanged(nameof(Airfoil2));
            }
        }
        public Double Airfoil3
        {
            get => airfoils[2];
            set
            {
                airfoils[2] = value;
                OnPropertyChanged(nameof(Airfoil3));
            }
        }
        public Double Airfoil4
        {
            get => airfoils[3];
            set
            {
                airfoils[3] = value;
                OnPropertyChanged(nameof(Airfoil4));
            }
        }
        public Double Airfoil5
        {
            get => airfoils[4];
            set
            {
                airfoils[4] = value;
                OnPropertyChanged(nameof(Airfoil5));
            }
        }
        public Double Airfoil6
        {
            get => airfoils[5];
            set
            {
                airfoils[5] = value;
                OnPropertyChanged(nameof(Airfoil6));
            }
        }
        public Double Airfoil7
        {
            get => airfoils[6];
            set
            {
                airfoils[6] = value;
                OnPropertyChanged(nameof(Airfoil7));
            }
        }
        public Double Airfoil8
        {
            get => airfoils[7];
            set
            {
                airfoils[7] = value;
                OnPropertyChanged(nameof(Airfoil8));
            }
        }
        public Double Airfoil9
        {
            get => airfoils[8];
            set
            {
                airfoils[8] = value;
                OnPropertyChanged(nameof(Airfoil9));
            }
        }
        public Double Airfoil10
        {
            get => airfoils[9];
            set
            {
                airfoils[9] = value;
                OnPropertyChanged(nameof(Airfoil10));
            }
        }

        private Double[] GetArray()
        {
            return airfoils.Clone() as Double[];
        }
    }
}
