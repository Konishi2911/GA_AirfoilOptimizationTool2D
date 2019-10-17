using System;
using System.ComponentModel;

namespace GA_AirfoilOptimizationTool2D
{
    public class OptimizingConfiguration : General.ModelBase
    {
        private const int numberOfSameGenerations = 10;

        private Airfoil.IAirfoilGroupManager _basisAirfoils;
        private Airfoil.CombinedAirfoilsGroupManager _combinedAirfoils;
        private Double[,] _coefficientOfCombination;

        public event EventHandler SourceDataChanged;

        /// <summary>
        /// This Class is Singleton
        /// </summary>
        private OptimizingConfiguration()
        {
            // Instantiate
            CombinedAirfoils = new Airfoil.CombinedAirfoilsGroupManager(numberOfSameGenerations);

            // Assign Event
            this.PropertyChanged += This_PropertyChanged;
        }
        public static OptimizingConfiguration Instance { get; private set; } = new OptimizingConfiguration();

        #region Event Callbacks
        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.BasisAirfoils) || e.PropertyName == nameof(this.CoefficientOfCombination))
            {
                if (CoefficientOfCombination == null)
                {
                    return;
                }

                // If number of basis airfoils are greater than number of coefficient of combination
                if (BasisAirfoils.NumberOfAirfoils > CoefficientOfCombination.GetLength(0))
                {
                    // Add new row of coefficient at the last of CoefficientOfCombination.
                    AddCoefficient();
                }

                // Re-Generate the combined airfoils
                CombinedAirfoils.CombineAirfoils(FMainWindow.Models.BasisAirfoils.Convert(BasisAirfoils), CoefficientOfCombination);

                // Fire the event updated SourceData are ready
                SourceDataChanged?.Invoke(this, new EventArgs());
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
        public Airfoil.CombinedAirfoilsGroupManager CombinedAirfoils
        {
            get => _combinedAirfoils;
            set
            {
                _combinedAirfoils = value;
                OnPropertyChanged(nameof(CombinedAirfoils));
            }
        }
        public Double[,] CoefficientOfCombination
        {
            get => _coefficientOfCombination;
            set
            {
                _coefficientOfCombination = value;
                OnPropertyChanged(nameof(CoefficientOfCombination));

                System.Diagnostics.Debug.Print("OptimizingConfiguration.CoefficientOfCombination");
            }

        }

        public void SetSource(Airfoil.IAirfoilGroupManager baseAirfoils, Double[,] coefficients)
        {
            this._basisAirfoils = baseAirfoils;
            this._coefficientOfCombination = coefficients;

            if (baseAirfoils != null && coefficients != null)
            {
                // If number of basis airfoils are greater than number of coefficient of combination
                if (BasisAirfoils.NumberOfAirfoils > CoefficientOfCombination.GetLength(0))
                {
                    // Add new row of coefficient at the last of CoefficientOfCombination.
                    AddCoefficient(BasisAirfoils.NumberOfAirfoils - CoefficientOfCombination.GetLength(0));
                }

                // Re-Generate the combined airfoils
                CombinedAirfoils.CombineAirfoils(FMainWindow.Models.BasisAirfoils.Convert(BasisAirfoils), CoefficientOfCombination);

                // Fire the event updated SourceData are ready
                SourceDataChanged?.Invoke(this, new EventArgs());
            }
            else if (baseAirfoils == null && coefficients != null)
            {

            }
            else if (baseAirfoils != null && coefficients == null)
            {
                // Add new row of coefficient at the last of CoefficientOfCombination.
                _coefficientOfCombination = new double[0,10];
                AddCoefficient(BasisAirfoils.NumberOfAirfoils);

                // Re-Generate the combined airfoils
                CombinedAirfoils.CombineAirfoils(FMainWindow.Models.BasisAirfoils.Convert(BasisAirfoils), CoefficientOfCombination);

                // Fire the event updated SourceData are ready
                SourceDataChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Add a new row element of the corfficient at the last of the CoefficientCollection
        /// </summary>
        private void AddCoefficient()
        {
            var length = CoefficientOfCombination.GetLength(0);
            var width = CoefficientOfCombination.GetLength(1);

            var newCoefficientCollection = new Double[length + 1, width];

            Array.Copy(CoefficientOfCombination, newCoefficientCollection, length * width);

            // Update coefficientCollection directly (without Event firing).
            _coefficientOfCombination = newCoefficientCollection;
        }
        /// <summary>
        /// Add the number of coefficients specifiedbu the argument
        /// </summary>
        /// <param name="count"></param>
        private void AddCoefficient(int count)
        {
            for (int i = 0; i < count; i++)
            {
                AddCoefficient();
            }
        }
    }
}
