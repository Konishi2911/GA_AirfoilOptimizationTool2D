using System;
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
        private int noAirfoils;         // Number of airfoils to combine
        private List<double>[] coefficientCombination;   // double[Number of basis airfoils, Number of airfoils]
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

        // Initialize coefficients that have noBasis length and no width.
        public CoefficientOfCombination(int noBasis)
        {
            noBasisAirfoils = noBasis;

            coefficientCombination = new List<double>[noBasisAirfoils];
            for (int i = 0; i < noBasisAirfoils; i++)
            {
                coefficientCombination[i] = new List<double>();
            }
        }
        /// <summary>
        /// Initialize coefficients that have noBasis length and noAirfoils width with 0
        /// </summary>
        /// <param name="noBasis">Number of Basis airfoils</param>
        /// <param name="noAirfoils">Number of airfoils to combine</param>
        public CoefficientOfCombination(int noBasis, int noAirfoils)
        {
            noBasisAirfoils = noBasis;

            coefficientCombination = new List<double>[noBasisAirfoils];
            for (int i = 0; i < noBasisAirfoils; i++)
            {
                coefficientCombination[i] = new List<double>();
                for (int j = 0; j < noAirfoils; j++)
                {
                    coefficientCombination[i].Add(0);
                }
            }
        }

        /// <summary>
        /// use for Clone method only
        /// </summary>
        private CoefficientOfCombination() { }
        /// <summary>
        /// Initializes a new instance of CoefficientOfCombination with coefficients array.
        /// </summary>
        /// <param name="coefficients"></param>
        public CoefficientOfCombination(double[,] coefficients)
        {
            noBasisAirfoils = coefficients.GetLength(0);
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
            return General.ArrayManager.ConvertJuggedArrayToArray(coefficientCombination);
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
            coefficientCombination = ConvertToCoefficientFormat(coefficients);
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

        public CoefficientOfCombination Clone()
        {
            int _n_noBasisAirfoils = noBasisAirfoils;
            int _n_noAirfoils = noAirfoils;
            List<double>[] _n_coefficientCombination = new List<double>[coefficientCombination.Length];

            for (int i = 0; i < coefficientCombination.Length; i++)
            {
                _n_coefficientCombination[i] = new List<double>();
                for (int j = 0; j < coefficientCombination[i].Count; j++)
                {
                    _n_coefficientCombination[i].Add(coefficientCombination[i][j]);
                }
            }

            return new CoefficientOfCombination()
            {
                coefficientCombination = _n_coefficientCombination,
                noBasisAirfoils = _n_noBasisAirfoils,
                noAirfoils = _n_noAirfoils
            };
        }

        private List<double>[] ConvertToCoefficientFormat(double[,] array)
        {
            var jArray = General.ArrayManager.ConvertArrayToJuggedArray(array);
            List<double>[] lArray = new List<double>[jArray.Length];
            for (int i = 0; i < jArray.Length; i++)
            {
                lArray[i] = new List<double>(jArray[i]);
            }

            return lArray;
        }
    }
}
