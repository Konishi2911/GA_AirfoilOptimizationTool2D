﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    /// <summary>
    /// Represents combined airfoils group. Contains basis airfoils, coefficients of combination and coordinates of combined airfoils.
    /// </summary>
    public class CombinedAirfoilGroup
    {
        private int _numberOfAirfoils;
        private readonly General.BasisAirfoils _basisAirfoils;
        private CoefficientOfCombination _coefficientsOfCombination;
        private List<AirfoilManager> _combinedAirfoils;

        public double[,] CoefficientOfCombination => _coefficientsOfCombination.GetCoefficientArray();
        public AirfoilManager[] CombinedAirfoils => _combinedAirfoils.ToArray<AirfoilManager>();

        public event EventHandler CombinedAirfoilsUpdated;

        public CombinedAirfoilGroup()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="noAirfoils">Number of Combined airfoils</param>
        public CombinedAirfoilGroup(int noAirfoils)
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
    }
}
