using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FAirfoilGAManager
{
    class AirfoilCrossover
    {
        private int[] parentsIndex;
        private double[][] optParameters;
        private FGeneticAlgorithm.UNDX_Parameters undxParameters;
        private CrossoverOperator crossoverOperator;

        public int NumberOfCrossovers => 10;
        public double[][] OptimizationParamters => optParameters;
        public int[] ParentsIndex => parentsIndex;

        public AirfoilCrossover(CrossoverOperator crossoverOperator)
        {
            this.crossoverOperator = crossoverOperator;
        }

        public enum CrossoverOperator
        {
            UNDX
        }

        public void ExecuteCrossover(Airfoil.CombinedAirfoilsGroupManager parentAirfoils)
        {
            var parentsIndividuals = CreateIndividuals(parentAirfoils);

            // UNDX Crossover
            if (crossoverOperator == CrossoverOperator.UNDX)
            {
                // Configure UNDX
                undxParameters = new FGeneticAlgorithm.UNDX_Parameters()
                {
                    Alpha = 0.5,
                    Beta = 0.35,
                    NumberOfCrossovers = NumberOfCrossovers,
                    NumberOfParameters = parentsIndividuals.IndivisualsGroup[0].OptParameters.Length
                };
                var undxExecutor = new FGeneticAlgorithm.UNDX(undxParameters);

                // Execute crossover with UNDX
                optParameters = undxExecutor.ExecuteCrossover(parentsIndividuals);
                parentsIndex = undxExecutor.ParentsIndex;
            }

        }

        private FGeneticAlgorithm.IndividualsGroup CreateIndividuals(Airfoil.CombinedAirfoilsGroupManager airfoilsGroup)
        {
            FGeneticAlgorithm.IndividualsGroup individuals = new FGeneticAlgorithm.IndividualsGroup();

            var airfoils = airfoilsGroup.GetCombinedAirfoilsArray();
            foreach (var item in airfoils)
            {
                double fitness = 1.0;
                individuals.AddIndivisual(new FGeneticAlgorithm.Individual(item.Coefficients, fitness));
            }

            return individuals;
        }
    }
}
