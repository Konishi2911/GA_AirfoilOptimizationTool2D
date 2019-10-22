using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FGeneticAlgorithm
{
    public class GAAirfoilsGroup : Airfoil.AirfoilGroupManagerBase
    {
        private int numberOfBasisAirfoils;

        private General.BasisAirfoils basisAirfoils;
        private double[,] coefficients;

        public int NumberOfBasisAirfoils => numberOfBasisAirfoils;

        public General.BasisAirfoils BasisAirfoils => basisAirfoils;
        public double[,] CoefficientsOfCombination => coefficients;
    }
}
