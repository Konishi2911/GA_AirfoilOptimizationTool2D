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

            //// Random Airfoil Selection

            //int n = 
            //double sigma1 = alpha * d1;
            //double sigma2 = beta * d2 / Math.Sqrt(new);

            return offspring;
        }
    }
}
