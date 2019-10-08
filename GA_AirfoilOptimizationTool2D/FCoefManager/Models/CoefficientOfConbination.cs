using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FCoefManager.Models
{
    class CoefficientOfConbination : General.ModelBase
    {
        private int numberOfBasisAirfoil;
        private ObservableCollection<EachCoefficients> _coefficients;

        #region Properties
        public int NumberOfBasisAirfoils
        {
            get
            {
                return numberOfBasisAirfoil;
            }
            set
            {
                numberOfBasisAirfoil = value;
                OnPropertyChanged(nameof(NumberOfBasisAirfoils));
            }
        }
        public ObservableCollection<EachCoefficients> Coefficients
        {
            get
            {
                return _coefficients;
            }
            set
            {
                _coefficients = value;
                OnPropertyChanged(nameof(Coefficients));
            }
        }
        #endregion 

        public event NotifyCollectionChangedEventHandler CoefficientCollectionSizeUpdated;

        public CoefficientOfConbination()
        {
            Coefficients = new ObservableCollection<EachCoefficients>();

            this.PropertyChanged += This_PropertyChanged;
            Coefficients.CollectionChanged += Coordinates_CollectionChanged;
        }

        #region EventCallBacks
        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Number of Basis Airfoil Changed
            if (e.PropertyName == nameof(this.NumberOfBasisAirfoils))
            {
                setCoordinateLength(NumberOfBasisAirfoils);
            }
        }
        private void Coordinates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Notify Collection of coefficient updated.
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (EachCoefficients item in e.NewItems)
                {
                    item.PropertyChanged += Coordinates_CollectionPropertyChanged;
                }
                CoefficientCollectionSizeUpdated?.Invoke(this, new NotifyCollectionChangedEventArgs(e.Action, e.NewItems));
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (EachCoefficients item in e.OldItems)
                {
                    item.PropertyChanged -= Coordinates_CollectionPropertyChanged;
                }
                CoefficientCollectionSizeUpdated?.Invoke(this, new NotifyCollectionChangedEventArgs(e.Action, e.OldItems));
            }
        }
        private void Coordinates_CollectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }
        #endregion

        private void setCoordinateLength(int length)
        {
            // If number of airfoils are diminished.
            if (length < _coefficients.Count)
            {
                for (int i = 0; i <= _coefficients.Count - length; i++)
                {
                    _coefficients.RemoveAt(_coefficients.Count - 1);
                }
            }
            // If number of airfoils are increased.
            else
            {
                for (int i = 0; i <= length - _coefficients.Count; i++)
                {
                    _coefficients.Add(new EachCoefficients());
                }
            }
        }
        private void GetDoubleArray()
        {
            var length = _coefficients.Count;

            var temp = new Double[length, 10];
        }
    }
}
