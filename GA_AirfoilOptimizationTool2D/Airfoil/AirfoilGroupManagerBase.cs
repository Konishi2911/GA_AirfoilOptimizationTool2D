using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    public abstract class AirfoilGroupManagerBase : General.ModelBase
    {
        private Double numberOfAirfoil;
        private List<Airfoil.AirfoilManager> airfoilGroup;

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

        public AirfoilGroupManagerBase()
        {
            #region Instantiation
            airfoilGroup = new List<AirfoilManager>();
            #endregion
        }

        /// <summary>
        /// Add airfoil.
        /// </summary>
        /// <param name="specification"></param>
        public void Add(AirfoilManager specification)
        {
            // Add new Airfoil Collection
            var _airfoilGroup = AirfoilGroup;
            _airfoilGroup.Add(specification);
            AirfoilGroup = _airfoilGroup;

            // Increment number of airfoils
            ++NumberOfAirfoils;
        }

        /// <summary>
        /// Add airfoil.
        /// </summary>
        /// <param name="coordinate"></param>
        public void Add(AirfoilCoordinate coordinate)
        {
            // Add new Airfoil Collection
            var _airfoilGroup = AirfoilGroup;
            _airfoilGroup.Add(new AirfoilManager(coordinate));
            AirfoilGroup = _airfoilGroup;

            // Increment number of airfoils
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
            if (airfoilGroup.Count == 0)
            {
                return null;
            }
            return airfoilGroup[index];
        }
    }
}
