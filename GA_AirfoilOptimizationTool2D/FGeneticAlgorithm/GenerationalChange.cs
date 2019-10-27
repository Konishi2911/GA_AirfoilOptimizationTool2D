using System;
using System.Collections.Generic;

namespace GA_AirfoilOptimizationTool2D.FGeneticAlgorithm
{
    public class GenerationalChange
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="optParameters"></param>
        /// <returns></returns>
        public delegate double[] EvaluationMethod(double[][] optParameters);
        EvaluationMethod evaluationMethod;

        private GenerationChangeMode generationChangeMode;

        public enum GenerationChangeMode
        {
            UNDX_MGG
        }

        public GenerationalChange(EvaluationMethod evaluationMethod)
        {
            this.evaluationMethod = evaluationMethod;
        }

        public IndividualsGroup ExecuteGenerationChange(IndividualsGroup parents)
        {
            if (generationChangeMode == GenerationChangeMode.UNDX_MGG)
            {
                return UNDX_MGG(parents, evaluationMethod);
            }
            else
            {
                return null;
            }
        }

        private IndividualsGroup UNDX_MGG(IndividualsGroup parents, EvaluationMethod evaluationMethod)
        {
            UNDX_Parameters UNDXConfiguration = new UNDX_Parameters()
            {
                Alpha = 0.5,
                Beta = 0.35,
                NumberOfCrossovers = 10,
                NumberOfParameters = parents.IndivisualsGroup[0].OptParameters.Length
            };
            UNDX undxExecutor = new UNDX(UNDXConfiguration);

            // Execute Crossove with UNDX
            var parameters = undxExecutor.ExecuteCrossover(parents);

            // Calculate Characteristics
            var fitnesses = evaluationMethod(parameters);

            // Create Individuals
            var offsprings = CreateIndividuals(parameters, fitnesses);
        }

        private IndividualsGroup CreateIndividuals(double[][] parameters, double[] fitnesses)
        {
            // Evaluate generated offspring
            IndividualsGroup offspring = new IndividualsGroup();
            var nCrossover = parameters.Length;

            for (int i = 0; i < nCrossover; i++)
            {
                offspring.AddIndivisual(new Individual(parameters[i], fitnesses[i]));
            }
            return offspring;
        }
    }
}
