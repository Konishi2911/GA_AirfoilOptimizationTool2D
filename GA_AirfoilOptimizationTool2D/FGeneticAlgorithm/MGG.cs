﻿using System.Collections.Generic;

namespace GA_AirfoilOptimizationTool2D.FGeneticAlgorithm
{
    public class MGG
    {
        public int[] SelectedIndividualsIndex { get; set; }

        public MGG()
        {
            SelectedIndividualsIndex = new int[2];
        }

        /// <summary>
        /// Selected Individuals are returned
        /// </summary>
        /// <param name="targetIndividuals"></param>
        /// <returns></returns>
        public IndividualsGroup ExecuteSelection(IndividualsGroup _targetIndividuals)
        {
            IndividualsGroup selectedIndividuals = new IndividualsGroup();
            IndividualsGroup targetIndividuals = new IndividualsGroup(_targetIndividuals);

            var elite = EliteSelection(targetIndividuals);
            SelectedIndividualsIndex[0] = targetIndividuals.IndexOf(elite);

            targetIndividuals.IndivisualsGroup.Remove(elite);
            var roulette = RouletteSelection(targetIndividuals);

            if (SelectedIndividualsIndex[0] <= targetIndividuals.IndexOf(roulette))
            {
                SelectedIndividualsIndex[1] = targetIndividuals.IndexOf(roulette) + 1;
            }
            else
            {
                SelectedIndividualsIndex[1] = targetIndividuals.IndexOf(roulette);
            }

            selectedIndividuals.AddIndivisual(elite);
            selectedIndividuals.AddIndivisual(roulette);

            return selectedIndividuals;
        }

        private Individual EliteSelection(in IndividualsGroup population)
        {
            int length = population.NumberOfIndividuals;
            double maximumFitness;
            Individual elite;

            maximumFitness = population.IndivisualsGroup[0].Fitness;
            elite = population.IndivisualsGroup[0];
            for (int i = 1; i < length; i++)
            {
                if (maximumFitness < population.IndivisualsGroup[i].Fitness)
                {
                    maximumFitness = population.IndivisualsGroup[i].Fitness;
                    elite = population.IndivisualsGroup[i];
                }
            }
            return elite;
        }
        private Individual RouletteSelection(in IndividualsGroup population)
        {
            // Clone Individuals from population
            List<Individual> individuals = new List<Individual>(population.IndivisualsGroup);
            double totalFitness = 0.0;
            General.RandomNumber.RandomNumberGenerator randomNumberGenerator = new General.RandomNumber.RandomNumberGenerator();

            foreach (var item in individuals)
            {
                totalFitness += item.Fitness;
            }

            var roulette = randomNumberGenerator.UniformRndNum() * totalFitness;
            double currentFitness = 0.0;
            Individual selectedIndividual = null;
            for (int i = 0; i < individuals.Count; i++)
            {
                var previousFitness = currentFitness;
                currentFitness = individuals[i].Fitness + previousFitness;
                if (currentFitness > roulette)
                {
                    selectedIndividual = individuals[i];
                    break;
                }
            }

            return selectedIndividual;
        }
    }
}
