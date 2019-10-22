using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Models
{
    public sealed class ImportedAirfoilGroupManager : Airfoil.AirfoilGroupManagerBase
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

        private  static ImportedAirfoilGroupManager Instance { get; set; } = new ImportedAirfoilGroupManager();
        /// <summary>
        /// This class is Singleton
        /// </summary>
        private ImportedAirfoilGroupManager() { }
        public static ImportedAirfoilGroupManager GetCurrentInstance()
        {
            return Instance;
        }
        public static ImportedAirfoilGroupManager GetNewInstance()
        {
            Instance = new ImportedAirfoilGroupManager();
            return Instance;
        }

        public T Convert<T>() where T : Airfoil.IAirfoilGroupManager, new()
        {
            T temp = new T();

            if (typeof(T) == typeof(General.BasisAirfoils))
            {
                (temp as General.BasisAirfoils).NumberOfAirfoils = this.NumberOfAirfoils;
                (temp as General.BasisAirfoils).NumberOfBasisAirfoils = this.NumberOfBasisAirfoils;
                (temp as General.BasisAirfoils).AirfoilGroup = new List<Airfoil.AirfoilManager>(this.AirfoilGroup);
            }

            (temp as Airfoil.IAirfoilGroupManager).NumberOfAirfoils = this.NumberOfAirfoils;
            (temp as Airfoil.IAirfoilGroupManager).AirfoilGroup = new List<Airfoil.AirfoilManager>(this.AirfoilGroup);

            return temp;
        }
    }
}
