using System;
using System.Collections.Generic;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    public abstract class AirfoilGroupManagerBase : General.ModelBase, Airfoil.IAirfoilGroupManager
    {
        private int numberOfAirfoil;
        private int numberOfBAirfoil;
        private List<Airfoil.AirfoilManager> airfoilGroup;

        public event AirfoilAddedEventHandler AirfoilAdded;
        public event AirfoilRemovedEventHandler AirfoilRemoved;

        #region CustomEventHandler
        public delegate void AirfoilAddedEventHandler(object sender, AirfoilAddedEventArgs e);
        public delegate void AirfoilRemovedEventHandler(object sender, AirfoilRemovedEventArgs e);
        #endregion

        #region CostomEventArgs
        public class AirfoilAddedEventArgs : EventArgs
        {
            public Airfoil.AirfoilManager AddedAirfoil { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="airfoil">Added Airfoil</param>
            public AirfoilAddedEventArgs(Airfoil.AirfoilManager airfoil)
            {
                AddedAirfoil = airfoil;
            }
        }

        public class AirfoilRemovedEventArgs : EventArgs
        {
            public String Lable { get; private set; }
            public Airfoil.AirfoilManager RemovedAirfoil { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="airfoil">Added Airfoil</param>
            public AirfoilRemovedEventArgs(Airfoil.AirfoilManager airfoil, String label)
            {
                this.Lable = label;
                RemovedAirfoil = airfoil;
            }
        }
        #endregion

        public int NumberOfAirfoils
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

        public int NumberOfBasisAirfoils
        {
            get
            {
                return numberOfBAirfoil;
            }
            set
            {
                numberOfBAirfoil = value;
                OnPropertyChanged(nameof(NumberOfBasisAirfoils));
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
            AirfoilGroup.Add(specification);

            // Increment number of airfoils
            ++NumberOfAirfoils;

            // Issue the Event added new airfoil to AirfoilGroupList
            AirfoilAdded?.Invoke(this, new AirfoilAddedEventArgs(specification));
        }

        /// <summary>
        /// Add airfoil.
        /// </summary>
        /// <param name="coordinate"></param>
        public void Add(AirfoilCoordinate coordinate)
        {
            // Add new Airfoil Collection
            Add(new AirfoilManager(coordinate));
        }

        public void Remove(AirfoilManager airfoil, String label)
        {
            // Remove specified airfoil.
            AirfoilGroup.Remove(airfoil);

            // Decrement number of airfoils
            --NumberOfAirfoils;

            // Issue the Event removed specified airfoil from AirfoilGroupList
            AirfoilRemoved?.Invoke(this, new AirfoilRemovedEventArgs(airfoil, label));
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
