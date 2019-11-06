using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    /// <summary>
    /// Represents combined airfoils group. Contains basis airfoils, coefficients of combination and coordinates of combined airfoils.
    /// </summary>
    public class CombinedAirfoilsGroup
    {
        private int _numberOfAirfoils;
        private readonly General.BasisAirfoils _basisAirfoils;
        private CoefficientOfCombination _coefficientsOfCombination;
        private List<AirfoilManager> _combinedAirfoils;

        public CoefficientOfCombination CoefficientOfCombination => _coefficientsOfCombination;
        public AirfoilManager[] CombinedAirfoils => _combinedAirfoils.ToArray<AirfoilManager>();
        public General.BasisAirfoils BasisAirfoils => _basisAirfoils;

        public event EventHandler CombinedAirfoilsUpdated;

        public CombinedAirfoilsGroup()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="noAirfoils">Number of Combined airfoils</param>
        public CombinedAirfoilsGroup(int noAirfoils)
        {

        }

        public void Add(Representation.AirfoilCombiner combinedAirfoil)
        {
            var basisAirfoils = combinedAirfoil.BasisAirfoils;
            var coefficients = combinedAirfoil.Coefficients;

            // Format Check
            var noBasisAirfoils = _basisAirfoils.AirfoilGroup.Count;
            if (noBasisAirfoils != basisAirfoils.Length)
            {
                throw new FormatException("The combined airfoil that has different basis airfoils from defined in this instance are passed.");
            }

            // Add coefficients of new airfoil
            _coefficientsOfCombination.AddCoefficient(coefficients);

            // Add new combined airfoil
            _combinedAirfoils.Add(combinedAirfoil.CombinedAirfoil);

            // Increment number of combined airfoils
            _numberOfAirfoils++;
        }
        public void Add(AirfoilManager airfoil, double[] coefficients)
        {
            // Format Check (Check number of basis airfoils)
            var noBasisAirfoils = _basisAirfoils.AirfoilGroup.Count;
            if (noBasisAirfoils != coefficients.Length)
            {
                throw new FormatException("The combined airfoil that has different basis airfoils from defined in this instance are passed.");
            }

            // Add coefficients of new airfoil
            _coefficientsOfCombination.AddCoefficient(coefficients);

            // Add new combined airfoil
            _combinedAirfoils.Add(airfoil);

            // Increment number of combined airfoils
            _numberOfAirfoils++;
        }

        /// <summary>
        /// Add combined airfoils collection.
        /// </summary>
        /// <param name="combinedAirfoils"></param>
        public void AddRange(ICollection<Representation.AirfoilCombiner> combinedAirfoils)
        {
            foreach (var item in combinedAirfoils)
            {
                Add(item);
            }
        }
    }
}
