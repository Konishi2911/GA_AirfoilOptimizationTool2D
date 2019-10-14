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

        /// <summary>
        /// Storing and Managing coefficient of combinations.
        /// </summary>
        private Models.CoefficientOfConbinationManager coefficientOfCombination;

        /// <summary>
        /// Bind to the DataGrid
        /// </summary>
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
            // NumberOFBasisAirfoils are changed
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
        }

        /// <summary>
        /// If DataGrid are changed by user input, this event are fired.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void This_DataGridValueChanged(object sender, PropertyChangedEventArgs e)
        {
            coefficientOfCombination.UpdateCoefficients(this.Coefficients);
        }
        #endregion

        public CoefficientManagerViewModel()
        {
            // Instantiate
            Coefficients = new ObservableCollection<Models.EachCoefficients>();
            coefficientOfCombination = new Models.CoefficientOfConbinationManager();
            //

            // Assign Event Call Backs
            PropertyChanged += this.This_PropertyChanged;
            coefficientOfCombination.PropertyChanged += this.CoefOfCombination_PropertyChanged;
            coefficientOfCombination.CoefficientCollectionSizeUpdated += this.CoefficientCollectionSizeUpdated;
            //

            InitializeCoefficients(coefficientOfCombination.Coefficients);
            NumberOfBasisAirfoils = coefficientOfCombination.NumberOfBasisAirfoils;
        }

        /// <summary>
        /// Initialize the collection Coefficients
        /// </summary>
        /// <param name="reference"></param>
        private void InitializeCoefficients(in ObservableCollection<Models.EachCoefficients> reference)
        {
            var count = reference.Count;

            foreach (var item in reference)
            {
                // Copy coefficients only from OptimizingConfiguration.
                Models.EachCoefficients element = new Models.EachCoefficients()
                {
                    Airfoil1 = item.Airfoil1,
                    Airfoil2 = item.Airfoil2,
                    Airfoil3 = item.Airfoil3,
                    Airfoil4 = item.Airfoil4,
                    Airfoil5 = item.Airfoil5,
                    Airfoil6 = item.Airfoil6,
                    Airfoil7 = item.Airfoil7,
                    Airfoil8 = item.Airfoil8,
                    Airfoil9 = item.Airfoil9,
                    Airfoil10 = item.Airfoil10,
                };
                // Assign Event callback
                element.PropertyChanged += This_DataGridValueChanged;

                Coefficients.Add(element);
            }
        }
    }
}
