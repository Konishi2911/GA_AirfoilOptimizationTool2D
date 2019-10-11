using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D
{
    public class OptimizingConfiguration : General.ModelBase
    {
        private Airfoil.IAirfoilGroupManager _basisAirfoils;
        private Double[,] _coefficientOfCombination;

        /// <summary>
        /// This Class is Singleton
        /// </summary>
        private OptimizingConfiguration()
        {
            // Assign Event
            PropertyChanged += This_PropertyChanged;
        }
        public static OptimizingConfiguration Instance { get; private set; } = new OptimizingConfiguration();

        #region Event Callbacks
        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.BasisAirfoils))
            {
                if (CoefficientOfCombination == null)
                {
                    return;
                }
                if (BasisAirfoils.NumberOfAirfoils > CoefficientOfCombination.GetLength(0))
                {
                    AddCoefficient();
                }
            }
        }
        #endregion

        public Airfoil.IAirfoilGroupManager BasisAirfoils
        {
            get => _basisAirfoils;
            set
            {
                _basisAirfoils = value;
                OnPropertyChanged(nameof(BasisAirfoils));
            }
        }
        public Double[,] CoefficientOfCombination
        {
            get => _coefficientOfCombination;
            set
            {
                _coefficientOfCombination = value;
                OnPropertyChanged(nameof(CoefficientOfCombination));

                System.Diagnostics.Debug.Print("Fired");
            }

        }

        private void AddCoefficient()
        {
            var length = CoefficientOfCombination.GetLength(0);
            var width = CoefficientOfCombination.GetLength(1);

            var newCoefficientCollection = new Double[length + 1, width];

            Array.Copy(CoefficientOfCombination, newCoefficientCollection, length * width);
            CoefficientOfCombination = newCoefficientCollection;
        }
    }
}
