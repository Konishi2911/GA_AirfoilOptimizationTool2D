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
        private List<Individual> individuals;

        public IndividualsGroup()
        {
            individuals = new List<Individual>();
        }

        /// <summary>
        /// Number of individuals
        /// </summary>
        public int NumberOfIndividuals => nIndividuals;
        /// <summary>
        /// List of individuals
        /// </summary>
        public List<Individual> IndivisualsGroup => individuals;

        public void AddIndivisual(Individual individual)
        {
            individuals.Add(individual);
            ++nIndividuals;
        }

        public int IndexOf(Individual item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            return individuals.IndexOf(item);
        }
    }
}
