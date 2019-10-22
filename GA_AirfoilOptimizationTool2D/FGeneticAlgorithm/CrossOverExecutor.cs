using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FGeneticAlgorithm
{
    public class CrossoverExecutor
    {
        private GAAirfoilsGroup parentAirfoilGroup;
        private GAAirfoilsGroup offspringAirfoilGroup;

        public CrossoverExecutor()
        {
        }

        /// <summary>
        /// Execute the crossover with UNDX
        /// </summary>
        /// <param name="parents"></param>
        /// <returns>Offsprings</returns>
        private GAAirfoilsGroup UNDX(GAAirfoilsGroup parents)
        {
            GAAirfoilsGroup offspring = new GAAirfoilsGroup();
            Airfoil.AirfoilManager[] selectedParents = new Airfoil.AirfoilManager[2];

            // Random Airfoil Selection
            int pIndex1 = 0;
            int pIndex2 = 0;
            int pIndex3 = 0;

            // Create Optimization Parameter Vector

            General.Vector p1 = new General.Vector(parents.NumberOfBasisAirfoils);
            General.Vector p2 = new General.Vector(parents.NumberOfBasisAirfoils);
            General.Vector p3 = new General.Vector(parents.NumberOfBasisAirfoils);

            for (int i = 0; i < parents.NumberOfBasisAirfoils; i++)
            {
                p1[i] = parents.CoefficientsOfCombination[i, pIndex1];
                p2[i] = parents.CoefficientsOfCombination[i, pIndex2];
                p3[i] = parents.CoefficientsOfCombination[i, pIndex3];
            }

            int n = parents.NumberOfAirfoils;
            double d1 = (p2 - p1).Norm();
            double d2 = (General.Vector.InnerProduct(p3 - p1, p2 - p1) / Math.Pow((p2 - p1).Norm(), 2) * (p2 - p1) - p3).Norm();
            double sigma1 = alpha * d1;
            double sigma2 = beta * d2 / Math.Sqrt(n);

            return offspring;
        }
    }
}
