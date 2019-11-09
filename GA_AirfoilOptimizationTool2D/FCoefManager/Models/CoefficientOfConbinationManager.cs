using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace GA_AirfoilOptimizationTool2D.FCoefManager.Models
{
    class CoefficientOfConbinationManager : General.ModelBase
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
            private set
            {
                _coefficients = value;
                OnPropertyChanged(nameof(Coefficients));
            }
        }
        #endregion 

        public event NotifyCollectionChangedEventHandler CoefficientCollectionSizeUpdated;
        public CoefficientOfConbinationManager()
        {
            Coefficients = new ObservableCollection<EachCoefficients>();

            this.PropertyChanged += This_PropertyChanged;
            Coefficients.CollectionChanged += Coordinates_CollectionChanged;
            AirfoilOptimizationResource.Instance.CurrentParameterUpdated += Source_ParameterChanged;

            // If the Coefficient of combination in the source is not empty, push it to the coefficients in this class.
            if (AirfoilOptimizationResource.Instance.CurrentCoefficients != null)
            {
                foreach (var item in ConvertDoubleArrayToObservable(AirfoilOptimizationResource.Instance.CurrentCoefficients.GetCoefficientArray()))
                {
                    Coefficients.Add(item);
                }
            }
            // If the basisAirfoils in the source is not empty, create new coefficient collection that size is the number of basisAirfoils in the source.
            else if (AirfoilOptimizationResource.Instance.BasisAirfoils != null)
            {
                setCoefficientLength(AirfoilOptimizationResource.Instance.BasisAirfoils.NumberOfAirfoils);
            }
            // If both the coefficient collection and basisAirfoils are empty, it create new coefficient collection that size is zero. 
            else
            {

            }
        }

        public void UpdateCoefficients(ObservableCollection<EachCoefficients> reference)
        {
            // Adjust Coefficients Count to be same count as reference.
            if (Coefficients.Count != reference.Count)
            {
                setCoefficientLength(reference.Count);
            }

            for (int i = 0; i < reference.Count; i++)
            {
                // Copy coefficients only from OptimizingConfiguration.
                if (Coefficients[i].Airfoil1 != reference[i].Airfoil1) Coefficients[i].Airfoil1 = reference[i].Airfoil1;
                if (Coefficients[i].Airfoil2 != reference[i].Airfoil2) Coefficients[i].Airfoil2 = reference[i].Airfoil2;
                if (Coefficients[i].Airfoil3 != reference[i].Airfoil3) Coefficients[i].Airfoil3 = reference[i].Airfoil3;
                if (Coefficients[i].Airfoil4 != reference[i].Airfoil4) Coefficients[i].Airfoil4 = reference[i].Airfoil4;
                if (Coefficients[i].Airfoil5 != reference[i].Airfoil5) Coefficients[i].Airfoil5 = reference[i].Airfoil5;
                if (Coefficients[i].Airfoil6 != reference[i].Airfoil6) Coefficients[i].Airfoil6 = reference[i].Airfoil6;
                if (Coefficients[i].Airfoil7 != reference[i].Airfoil7) Coefficients[i].Airfoil7 = reference[i].Airfoil7;
                if (Coefficients[i].Airfoil8 != reference[i].Airfoil8) Coefficients[i].Airfoil8 = reference[i].Airfoil8;
                if (Coefficients[i].Airfoil9 != reference[i].Airfoil9) Coefficients[i].Airfoil9 = reference[i].Airfoil9;
                if (Coefficients[i].Airfoil10 != reference[i].Airfoil10) Coefficients[i].Airfoil10 = reference[i].Airfoil10;
            }

        }

        #region EventCallBacks
        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Number of Basis Airfoil Changed
            if (e.PropertyName == nameof(this.NumberOfBasisAirfoils))
            {
                //setCoordinateLength(NumberOfBasisAirfoils);
            }
        }
        private void Source_ParameterChanged(object sender, EventArgs e)
        {
            // Copy the coefficient collection from OptimizingConfiguration
            UpdateCoefficients(ConvertDoubleArrayToObservable(AirfoilOptimizationResource.Instance.CurrentCoefficients.GetCoefficientArray()));
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
            Airfoil.CoefficientOfCombination coef = new Airfoil.CoefficientOfCombination(GetDoubleArray());
            AirfoilOptimizationResource.Instance.SetSource(coef);
        }
        #endregion

        private void setCoefficientLength(int length)
        {
            var count = _coefficients.Count;
            // If number of airfoils are diminished.
            if (length < _coefficients.Count)
            {
                for (int i = 0; i < count - length; i++)
                {
                    _coefficients.RemoveAt(_coefficients.Count - 1);
                }
            }
            // If number of airfoils are increased.
            else
            {
                for (int i = 0; i < length - count; i++)
                {
                    _coefficients.Add(new EachCoefficients());
                }
            }
        }
        /// <summary>
        /// This method returns Two-Dimensional Double type array that is coefficients, which is new object.
        /// </summary>
        private Double[,] GetDoubleArray()
        {
            var length = _coefficients.Count;

            var temp = new Double[length, 10];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    temp[i, j] = _coefficients[i].GetArray()[j];
                }
            }
            return temp;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"/>
        private ObservableCollection<EachCoefficients> ConvertDoubleArrayToObservable(Double[,] array)
        {
            var length = array.GetLength(0);
            var width = array.GetLength(1);

            // Format Check
            if (width != GeneralConstants.NUMBER_OF_AIRFOILS_OF_GENERATION)
            {
                throw new FormatException("width of array of coefficient is invalid");
            }

            var oCollection = new ObservableCollection<EachCoefficients>();
            for (int i = 0; i < length; i++)
            {
                oCollection.Add(new EachCoefficients()
                {
                    Airfoil1 = array[i, 0],
                    Airfoil2 = array[i, 1],
                    Airfoil3 = array[i, 2],
                    Airfoil4 = array[i, 3],
                    Airfoil5 = array[i, 4],
                    Airfoil6 = array[i, 5],
                    Airfoil7 = array[i, 6],
                    Airfoil8 = array[i, 7],
                    Airfoil9 = array[i, 8],
                    Airfoil10 = array[i, 9],
                });
            }

            return oCollection;
        }
    }
}
