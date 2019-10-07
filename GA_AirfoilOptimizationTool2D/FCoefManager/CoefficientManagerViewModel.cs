using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FCoefManager
{
    class CoefficientManagerViewModel : General.ViewModelBase
    {
        private int numberOfBasisAirfoils;
        private ObservableCollection<Models.EachCoefficients> _coefficients;

        public ObservableCollection<Models.EachCoefficients> Coefficients
        {
            get { return _coefficients; }
            set { _coefficients = value; }
        }
        public int NumberOfBasisAirfoils
        {
            get
            {
                return numberOfBasisAirfoils;
            }
            private set
            {
                numberOfBasisAirfoils = value;
                OnPropertyChanged(nameof(NumberOfBasisAirfoils));
            }
        }

        #region EventCallBacks
        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // NumberOFBasisAirfoils
            if (e.PropertyName == nameof(this.NumberOfBasisAirfoils))
            {
                // If BasisAirfoils are diminished.
                if (NumberOfBasisAirfoils < Coefficients.Count)
                {
                    for (int i = 0; i < Coefficients.Count - NumberOfBasisAirfoils; i++)
                    {
                        Coefficients.RemoveAt(Coefficients.Count - 1);
                    }
                }
                //If BasisAirfoils are increased.
                else
                {
                    for (int i = 0; i < NumberOfBasisAirfoils - Coefficients.Count; i++)
                    {
                        Coefficients.Add(new Models.EachCoefficients());
                    }
                }
            }
        }
        #endregion

        public CoefficientManagerViewModel()
        {
            // Instantiate
            Coefficients = new ObservableCollection<Models.EachCoefficients>();
            //

            NumberOfBasisAirfoils = OptimizingConfiguration.Instance.BasisAirfoils.NumberOfBasisAirfoils; 
        }
    }
}
