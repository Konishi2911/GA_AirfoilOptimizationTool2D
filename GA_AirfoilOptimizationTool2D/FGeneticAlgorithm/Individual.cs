using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FGeneticAlgorithm
{
    /// <summary>
    /// This class represent Optimizing Target
    /// </summary>
    public class Individual
    {
        // Fields
        private double[] optParameters;
        private double fitness;

        //Properties
        public double[] OptParameters => optParameters;
        public double Fitness => fitness;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="optParameters"></param>
        /// <param name="fitness"></param>
        public Individual(double[] optParameters, double fitness)
        {
            this.optParameters = optParameters;
            this.fitness = fitness;
        }
    }
}
