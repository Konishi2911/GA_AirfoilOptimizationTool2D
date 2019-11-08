using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    /// <summary>
    /// Provides methods to combine basis airfoils and to create new combined airfoils.
    /// </summary>
    public class AirfoilsMixer
    {
        private General.BasisAirfoils _basisAirfoils;
        private CoefficientOfCombination _coefficients;
        private Airfoil.Representation.AirfoilCombiner[] _combinedAirfoils;

        public Airfoil.Representation.AirfoilCombiner[] CombinedAirfoils => _combinedAirfoils;

        public AirfoilsMixer(in General.BasisAirfoils basisAirfoils, in CoefficientOfCombination coefficients)
        {
            _basisAirfoils = basisAirfoils ?? throw new ArgumentNullException();
            _coefficients = coefficients ?? throw new ArgumentNullException();
        }

        public void CombineAirfoils()
        {
            int _numberOfAirfoils = _coefficients.NoAirfoils;
            List<Task> tasks = new List<Task>();
            _combinedAirfoils = new Representation.AirfoilCombiner[_coefficients.NoAirfoils];

            for (int i = 0; i < _numberOfAirfoils; i++)
            {
                _combinedAirfoils[i] = new Representation.AirfoilCombiner();

                var x = i;
                var basis = _basisAirfoils.AirfoilGroup.ToArray();
                var coef = GetRowArray(_coefficients.GetCoefficientArray(), i);

                // Update Source and Re-Combinate Airfoil.
                tasks.Add(Task.Run(() => _combinedAirfoils[x].UpdateBaseSource(coef, basis)));
            }
            Task.WaitAll(tasks.ToArray());
        }

        private T[] GetRowArray<T>(T[,] array, int columnNumber)
        {
            var length = array.GetLength(0);
            var width = array.GetLength(1);

            var rArray = new T[width][];

            for (int i = 0; i < width; i++)
            {
                rArray[i] = new T[length];
                for (int j = 0; j < length; j++)
                {
                    rArray[i][j] = array[j, i];
                }
            }
            return rArray[columnNumber];
        }
    }
}
