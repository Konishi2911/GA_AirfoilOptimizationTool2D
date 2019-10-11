﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

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
            OptimizingConfiguration.Instance.PropertyChanged += Source_PropertyChanged;

            // If the Coefficient of combination in the source is not empty, push it to the coefficients in this class.
            if (OptimizingConfiguration.Instance.CoefficientOfCombination != null)
            {
                foreach (var item in ConvertDoubleArrayToObservable(OptimizingConfiguration.Instance.CoefficientOfCombination))
                {
                    Coefficients.Add(item);
                }
            }
            // If the basisAirfoils in the source is not empty, create new coefficient collection that size is the number of basisAirfoils in the source.
            else if (OptimizingConfiguration.Instance.BasisAirfoils != null)
            {
                setCoefficientLength(OptimizingConfiguration.Instance.BasisAirfoils.NumberOfBasisAirfoils);
            }
            // If both the coefficient collection and basisAirfoils are empty, it create new coefficient collection that size is zero. 
            else
            {

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
        private void Source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Coefficient Collection Changed
            if (e.PropertyName == nameof(OptimizingConfiguration.CoefficientOfCombination))
            {
                // Copy the coefficient collection from OptimizingConfiguration
                Coefficients = ConvertDoubleArrayToObservable(OptimizingConfiguration.Instance.CoefficientOfCombination);
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
            OptimizingConfiguration.Instance.CoefficientOfCombination = GetDoubleArray();
        }
        #endregion

        private void setCoefficientLength(int length)
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
                throw new FormatException();
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
