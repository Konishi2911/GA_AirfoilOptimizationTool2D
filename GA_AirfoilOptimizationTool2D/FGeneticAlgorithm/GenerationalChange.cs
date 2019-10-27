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

        private readonly GenerationChangeModel generationChangeModel;

        public enum GenerationChangeModel
        {
            UNDX_MGG
        }

        public GenerationalChange(EvaluationMethod evaluationMethod, GenerationChangeModel generationChangeModel)
        {
            this.evaluationMethod = evaluationMethod;
            this.generationChangeModel = generationChangeModel;
        }

        public IndividualsGroup ExecuteGenerationChange(IndividualsGroup parents)
        {
            if (generationChangeModel == GenerationChangeModel.UNDX_MGG)
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
            MGG mggExecutor = new MGG();

            // Execute Crossove with UNDX
            var parameters = undxExecutor.ExecuteCrossover(parents);
            var parentsIndex = undxExecutor.ParentsIndex;

            // Calculate Characteristics
            var fitnesses = evaluationMethod(parameters);

            // Create Individuals
            var offsprings = CreateIndividuals(parameters, fitnesses);

            // Create New Individuals
            var newIndividuals = mggExecutor.ExecuteSelection(offsprings);

            // Return new population
            return UpdatePopulation(parents, (int[])parentsIndex, newIndividuals);
        }

        /// <summary>
        /// Create Individuals from parameters and fitnesses
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="fitnesses"></param>
        /// <returns></returns>
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
        private IndividualsGroup UpdatePopulation(in IndividualsGroup previousParents, int[] replacementIndex, IndividualsGroup newIndividuals)
        {
            var length = previousParents.NumberOfIndividuals;

            IndividualsGroup newPopulation = new IndividualsGroup();
            int k = 0;
            for (int i = 0; i < length; i++)
            {
                if (CompareWithArray(i, replacementIndex))
                {
                    newPopulation.AddIndivisual(newIndividuals.IndivisualsGroup[k]);
                    ++k;
                }
                else
                {
                    newPopulation.AddIndivisual(previousParents.IndivisualsGroup[i]);
                }
            }

            return newPopulation;
        }

        /// <summary>
        /// This method check that value is equal to the elements in array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="array"></param>
        /// <return>true : The same element are found</returns>
        private bool CompareWithArray<T>(T value, T[] array)
        {
            bool isEqual = false;
            for (int i = 0; i < array.Length; i++)
            {
                isEqual |= array[i].Equals(value); 
            }

            return isEqual;
        }
    }
}
