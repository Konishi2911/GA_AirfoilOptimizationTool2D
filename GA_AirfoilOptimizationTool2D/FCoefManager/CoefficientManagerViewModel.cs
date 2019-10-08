﻿using System;
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

        private Models.CoefficientOfConbination coefficientOfCombination;


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
                if (coefficientOfCombination.NumberOfBasisAirfoils != this.NumberOfBasisAirfoils)
                {
                    coefficientOfCombination.NumberOfBasisAirfoils = NumberOfBasisAirfoils;
                }
            }
        }

        private void CoefOfCombination_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // NumberOFBasisAirfoils
            if (e.PropertyName == nameof(coefficientOfCombination.NumberOfBasisAirfoils))
            {
                if (coefficientOfCombination.NumberOfBasisAirfoils != this.NumberOfBasisAirfoils)
                {
                    NumberOfBasisAirfoils = coefficientOfCombination.NumberOfBasisAirfoils;
                }
            }
        }

        private void CoefficientCollectionSizeUpdated(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    this.Coefficients.Add(item as Models.EachCoefficients);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    this.Coefficients.Remove(item as Models.EachCoefficients);
                }
            }

            //int error = this.Coefficients.Count - (sender as Models.CoefficientOfConbination).Coefficients.Count;

            //if (error < 0)
            //{
            //    for (int i = 0; i < -error; i++)
            //    {
            //        this.Coefficients.Add(new Models.EachCoefficients());
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < error; i++)
            //    {
            //        this.Coefficients.RemoveAt(Coefficients.Count - 1);
            //    }
            //}
        }
        #endregion

        public CoefficientManagerViewModel()
        {
            // Instantiate
            Coefficients = new ObservableCollection<Models.EachCoefficients>();
            coefficientOfCombination = new Models.CoefficientOfConbination();
            //

            // Assign Event Call Backs
            PropertyChanged += this.This_PropertyChanged;
            coefficientOfCombination.PropertyChanged += this.CoefOfCombination_PropertyChanged;
            coefficientOfCombination.CoefficientCollectionSizeUpdated += this.CoefficientCollectionSizeUpdated;
            //

            NumberOfBasisAirfoils = OptimizingConfiguration.Instance.BasisAirfoils.NumberOfBasisAirfoils; 
        }
    }
}