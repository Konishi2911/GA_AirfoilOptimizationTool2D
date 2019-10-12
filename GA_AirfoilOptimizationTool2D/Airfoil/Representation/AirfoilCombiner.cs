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
    /// <exception cref="FormatException"/>
    public class AirfoilCombiner :General.ModelBase
    {
        private const int NUMBER_OF_DIVISION = GeneralConstants.NUMBER_OF_DIVISION;

        private Double[] _coefficient;
        private Airfoil.AirfoilManager[] _basisAirfoils;
        private Airfoil.AirfoilManager _combinedAirfoil;

        #region Properties
        /// <summary>
        /// The Coefficients of combination
        /// </summary>
        ///     /// <exception cref="FormatException">If the count of Coefficients and BasisAirfoils are not be matched, this exception thrown.</exception>
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
        ///     /// <exception cref="FormatException">If the count of Coefficients and BasisAirfoils are not be matched, this exception thrown.</exception>
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

        /// <summary>
        /// This method updates BasisAirfoils and Coefficients Collection.
        /// </summary>
        /// <param name="coefficient"></param>
        /// <param name="basis"></param>
        /// <exception cref="FormatException"></exception>
        public void UpdateBaseSource(Double[] coefficient, AirfoilManager[] basis)
        {
            try
            {
                this.BasisAirfoils = basis;
            }
            // It ignore the exception because Coefficient collection is not changed yet.
            catch (FormatException) { }

            this.Coefficients = coefficient;
        }

        private AirfoilCoordinate[] ResizeAirfoil(int numberOfBasisAirfoils)
        {
            AirfoilCoordinate[] basisAirfoil = new AirfoilCoordinate[numberOfBasisAirfoils];
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < numberOfBasisAirfoils; i++)
            {
                var x = i;
                tasks.Add(Task.Run(() => basisAirfoil[x] = BasisAirfoils[x].GetResizedAirfoil(NUMBER_OF_DIVISION)));
            }
            Task.WaitAll(tasks.ToArray());

            return basisAirfoil;
        }
        private void CombineAirfoil()
        {
            // Null Check
            if (_coefficient == null)
            {
                return;
            }
            if (_basisAirfoils == null)
            {
                return;
            }

            // Format Check
            if (_coefficient.Length != _basisAirfoils.Length)
            {
                throw new FormatException("Coefficient Size and Basis Airfoils Size are not be matched.");
            }

            var numberOfBasisAirfoils = _basisAirfoils.Length;
            Double[,] combinedAirfoilCoordinate = new Double[NUMBER_OF_DIVISION * 2, 2];
            AirfoilCoordinate combinedAirfoil = new AirfoilCoordinate();

            AirfoilCoordinate[] basisAirfoil = new AirfoilCoordinate[numberOfBasisAirfoils];

            basisAirfoil = ResizeAirfoil(numberOfBasisAirfoils);

            for (int i = 0; i < 2 * NUMBER_OF_DIVISION; i++)
            {
                for (int j = 0; j < numberOfBasisAirfoils; j++)
                {
                    combinedAirfoilCoordinate[i, 0] = basisAirfoil[j][i].X;
                    combinedAirfoilCoordinate[i, 1] += Coefficients[j] * basisAirfoil[j][i].Z;
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
                //Task.Run(CombineAirfoil).Wait();
            }
        }
    }
}
