using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    public abstract class AirfoilGroupeManagerBase : General.ModelBase
    {
        private Double numberOfAirfoil;
        private List<Airfoil.AirfoilManager> airfoils;

        public double NumberOfAirfoils
        {
            get
            {
                return numberOfAirfoil;
            }
            set
            {
                numberOfAirfoil = value;
                OnPropertyChanged(nameof(NumberOfAirfoils));
            }
        }

        /// <summary>
        /// Add airfoil.
        /// </summary>
        /// <param name="specification"></param>
        public void Add(AirfoilManager specification)
        {
            airfoils.Add(specification);
            ++NumberOfAirfoils;
        }

        /// <summary>
        /// Get element of airfoil that selected by index.
        /// If the List of airfoils is empty, return null to caller.
        /// </summary>
        /// <param name="index"> index if airfoils </param>
        /// <returns></returns>
        public Airfoil.AirfoilManager GetAirfoil(int index)
        {
            if (airfoils.Count == 0)
            {
                return null;
            }
            return airfoils[index];
        }
    }
}
