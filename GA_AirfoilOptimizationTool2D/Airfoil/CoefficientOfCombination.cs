﻿using System;
using System.Collections.Generic;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{   
    /// <summary>
    /// Represents coeffieicnts of combination. 
    /// </summary>
    public class CoefficientOfCombination
    {
        #region Fields
        private int noBasisAirfoils;    // Number of basis airfoils
        private int noAirfoils;         // Number of combined airfoils
        private List<List<double>> coefficientCombination;   // double[Number of basis airfoils, Number of airfoils]
        #endregion

        #region Properties
        /// <summary>
        /// Number of Airfoils to combine
        /// </summary>
        public int NoAirfoils => noAirfoils;

        /// <summary>
        /// Number of Basis airfoils
        /// </summary>
        public int NoBasisAirfoils => noBasisAirfoils;
        #endregion

        public CoefficientOfCombination(int noBasis) 
        {
            noBasisAirfoils = noBasis;
        }

        /// <summary>
        /// Initializes a new instance of CoefficientOfCombination with coefficients array.
        /// </summary>
        /// <param name="coefficients"></param>
        public CoefficientOfCombination(double[,] coefficients)
        {
            SetCoefficient(coefficients);
        }

        public double GetCoefficient(int airfoilNumber, int basisAirfoilNumber)
        {
            return coefficientCombination[basisAirfoilNumber][airfoilNumber];
        }
        public double[] GetCoefficients(int airfoilNumber)
        {
            if (noBasisAirfoils != 0 && coefficientCombination != null)
            {
                var coefficients = new double[noBasisAirfoils];
                for (int i = 0; i < noBasisAirfoils; i++)
                {
                    coefficients[i] = coefficientCombination[i][airfoilNumber];
                }
                return coefficients;
            }
            else { return null; }
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
            // Format check
            if (coefficients.GetLength(0) != noBasisAirfoils)
            {
                throw new FormatException("Length of passed coefficients array is invalid");
            }

            // Update Coefficients size
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
            // If different size is detected between coefficients size and number of basis airfoils in this instance, throw exception.
            if (coefficients.Length != noBasisAirfoils)
            {
                throw new FormatException();
            }

            // Add new coefficients
            for (int i = 0; i < noBasisAirfoils; i++)
            {
                coefficientCombination[i].Add(coefficients[i]);
            }

            // Increment number of airfoils
            noAirfoils++;
        }
    }
}
