using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    class CoefficientOfCombination
    {
        private int noBasisAirfoils;    // Number of basis airfoils
        private int noAirfoils;         // Number of combined airfoils
        private List<List<double>> coefficientCombination;   // double[Number of basis airfoils, Number of airfoils]


        public double GetCoefficient(int airfoilNumber, int basisAirfoilNumber)
        {
            return coefficientCombination[airfoilNumber][basisAirfoilNumber];
        }
        public double[,] GetCoefficientArray()
        {
            return General.ArrayManager.ConvertListToArray(coefficientCombination);
        }
        /// <summary>
        /// Set coefficients
        /// </summary>
        /// <param name="coefficients">[ Number of basis airfoils , Number of airfoils ]</param>
        public void SetCoefficient(double[,] coefficients)
        {
            // Update Coefficients size
            noBasisAirfoils = coefficients.GetLength(0);
            noAirfoils = coefficients.GetLength(1);

            // Update Coefficients
            coefficientCombination = General.ArrayManager.ConvertArrayToList(coefficients);
        }
        /// <summary>
        /// Add coefficients of new airfoil at end of the coefficient collection
        /// </summary>
        /// <param name="coefficients"></param>
        public void AddCoefficient(double[] coefficients)
        {
            // Increment number of airfoils
            noAirfoils++;

            // Add new coefficients
            for (int i = 0; i < noBasisAirfoils; i++)
            {
                coefficientCombination[i].Add(coefficients[i]);
            }
        }
    }
}
