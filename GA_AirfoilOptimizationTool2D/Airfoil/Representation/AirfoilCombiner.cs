using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil.Representation
{
    /// <summary>
    /// This class provides some functions such as the function to combine basis airfoils for generating new airfoil.
    /// </summary>
    public class AirfoilCombiner :General.ModelBase
    {
        private const int NUMBER_OF_DIVISION = 100;

        private Double[] _coefficient;
        private Airfoil.AirfoilManager[] _basisAirfoils;
        private Airfoil.AirfoilManager _combinedAirfoil;

        #region Properties
        /// <summary>
        /// The Coefficients of combination
        /// </summary>
        public Double[] Coefficients
        {
            get => _coefficient;
            set
            {
                _coefficient = value;
                OnPropertyChanged(nameof(Coefficients));
            }
        }
        /// <summary>
        /// Basis Airfoils
        /// </summary>
        public Airfoil.AirfoilManager[] BasisAirfoils
        {
            get => _basisAirfoils;
            set
            {
                _basisAirfoils = value;
                OnPropertyChanged(nameof(BasisAirfoils));
            }
        }
        /// <summary>
        /// The Airfoil which is combined basisAirfoils times coefficient of combination. This property is instantiated by executing the Method CombineAirfoil
        /// </summary>
        public Airfoil.AirfoilManager CombinedAirfoil
        {
            get => _combinedAirfoil;
            set
            {
                _combinedAirfoil = value;
                OnPropertyChanged(nameof(CombinedAirfoil));
            }
        }
        #endregion

        public AirfoilCombiner()
        {
            // Assign Event Callbacks
            this.PropertyChanged += This_PropertyChanged;
        }

        private void CombineAirfoil()
        {
            // Null Check
            if (_coefficient == null)
            {
                throw new ArgumentNullException("Coefficient is NULL.");
            }
            if (_basisAirfoils == null)
            {
                throw new ArgumentNullException("Basis Airfoil is NULL.");
            }

            // Format Check
            if (_coefficient.Length != _basisAirfoils.Length)
            {
                throw new FormatException("Coefficient Size and Basis Airfoils Size are not be matched.");
            }

            var numberOfBasisAirfoils = _basisAirfoils.Length;
            Double[,] combinedAirfoilCoordinate = new Double[numberOfBasisAirfoils, 2];
            AirfoilCoordinate combinedAirfoil = new AirfoilCoordinate();

            for (int i = 0; i < NUMBER_OF_DIVISION; i++)
            {
                for (int j = 0; j < numberOfBasisAirfoils; j++)
                {
                    var basisAirfoil = BasisAirfoils[j].GetResizedAirfoil(NUMBER_OF_DIVISION);

                    combinedAirfoilCoordinate[i, 0] = basisAirfoil[i].X;
                    combinedAirfoilCoordinate[i, 0] += Coefficients[j] * basisAirfoil[i].Z;
                }
            }
            combinedAirfoil.Import(combinedAirfoilCoordinate);

            CombinedAirfoil = new AirfoilManager(combinedAirfoil);
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.BasisAirfoils) || e.PropertyName == nameof(this.Coefficients))
            {
                // Re-combine Airfoils
                CombineAirfoil();
            }
        }
    }
}
