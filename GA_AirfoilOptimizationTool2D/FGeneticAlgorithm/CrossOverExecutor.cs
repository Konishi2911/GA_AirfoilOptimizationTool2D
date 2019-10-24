using System;
using System.Collections.Generic;

namespace GA_AirfoilOptimizationTool2D.FGeneticAlgorithm
{
    public class CrossoverExecutor
    {
        private IndividualsGroup parentAirfoilGroup;
        private IndividualsGroup offspringAirfoilGroup;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optParameters"></param>
        /// <returns></returns>
        private delegate double EvaluationMethod(double[][] optParameters);

        public CrossoverExecutor()
        {
        }

        /// <summary>
        /// Execute the crossover with UNDX
        /// </summary>
        /// <param name="parents">Individuals of previous generation</param>
        /// <param name="parameters">UNDX options</param>
        /// <param name="evaluatingMethod">Offspring evaluating Method that returns the fitness.</param>
        /// <returns>Offsprings</returns>
        private IndividualsGroup UNDX(IndividualsGroup parents, UNDX_Parameters parameters, EvaluationMethod evaluatingMethod)
        {
            int nCrossover = parameters.NumberOfCrossovers;
            int nParams = parameters.NumberOfParameters;
            double alpha = parameters.Alpha;
            double beta = parameters.Beta;

            Airfoil.CombinedAirfoilsGroupManager offsptingAirfoils = new Airfoil.CombinedAirfoilsGroupManager(nCrossover);
            IndividualsGroup offspring = new IndividualsGroup();
            Airfoil.AirfoilManager[] selectedParents = new Airfoil.AirfoilManager[2];

            // Random Airfoil Selection
            int pIndex1 = 0;
            int pIndex2 = 0;
            int pIndex3 = 0;

            // Create Optimization Parameter Vector

            General.Vector p1 = new General.Vector(parameters.NumberOfParameters);
            General.Vector p2 = new General.Vector(parameters.NumberOfParameters);
            General.Vector p3 = new General.Vector(parameters.NumberOfParameters);

            for (int i = 0; i < parameters.NumberOfParameters; i++)
            {
                p1[i] = parents.IndivisualsGroup[pIndex1].OptParameters[i];
                p2[i] = parents.IndivisualsGroup[pIndex2].OptParameters[i];
                p3[i] = parents.IndivisualsGroup[pIndex3].OptParameters[i];
            }

            int n = parents.NumberOfIndividuals;

            double d1 = (p2 - p1).Norm();
            double d2 = (General.Vector.InnerProduct(p3 - p1, p2 - p1) / Math.Pow((p2 - p1).Norm(), 2) * (p2 - p1) - p3).Norm();
            double sigma1 = alpha * d1;
            double sigma2 = beta * d2 / Math.Sqrt(n);

            // Create new parameter vector for offsprings
            General.Vector[] offspringParameter = new General.Vector[nCrossover];
            double[][] offspringParameterArray = new double[nCrossover][];
            for (int i = 0; i < nCrossover; i++)
            {

                // Generate Random Number with Normal Dist

                offspringParameter = null;

                // Processing

                offspringParameterArray[i] = offspringParameter[i].ToDoubleArray();
            }

            // Evaluate generated offspring
            double fitness = (double)evaluatingMethod?.Invoke(offspringParameterArray);
            for (int i = 0; i < nCrossover; i++)
            {
                offspring.IndivisualsGroup.Add(new Individual(offspringParameter[i].ToDoubleArray(), fitness));
            }
            return offspring;
        }
    }
}
