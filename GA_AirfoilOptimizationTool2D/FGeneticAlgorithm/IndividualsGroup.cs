using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FGeneticAlgorithm
{
    public class IndividualsGroup
    {
        private int nIndividuals;
        private List<Individual> individual;

        /// <summary>
        /// Number of individuals
        /// </summary>
        public int NumberOfIndividuals => nIndividuals;
        /// <summary>
        /// List of individuals
        /// </summary>
        public List<Individual> IndivisualsGroup => individual;
    }
}
