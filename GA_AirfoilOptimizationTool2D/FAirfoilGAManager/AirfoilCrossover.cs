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

        public void ExecuteCrossover(Airfoil.CoefficientOfCombination parentCoefficients)
        {
            var parentsIndividuals = CreateIndividuals(parentCoefficients);

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
                var temp = undxExecutor.ExecuteCrossover(parentsIndividuals);
                optParameters = CorrectInvalidParams(temp);
                parentsIndex = undxExecutor.ParentsIndex;
            }

        }

        private FGeneticAlgorithm.IndividualsGroup CreateIndividuals(Airfoil.CoefficientOfCombination coefficients)
        {
            FGeneticAlgorithm.IndividualsGroup individuals = new FGeneticAlgorithm.IndividualsGroup();
            
            // Pick coefficients array up fromCoefficientOfCombination
            //var temp = coefficients.GetCoefficientArray();
            //var coefficientsArray = General.ArrayManager.ConvertArrayToJuggedArray(temp) as double[][];

            //foreach (var item in coefficientsArray)
            //{
            //    double fitness = 1.0;
            //    individuals.AddIndivisual(new FGeneticAlgorithm.Individual(item, fitness));
            //}

            for (int i = 0; i < coefficients.NoAirfoils; i++)
            {
                double fitness = 1.0;
                var parameters = coefficients.GetCoefficients(i);
                individuals.AddIndivisual(new FGeneticAlgorithm.Individual(parameters, fitness));
            }

            return individuals;
        }

        /// <summary>
        /// Correct parameters to 0 if they are less than 0.
        /// </summary>
        /// <param name="parameters"></param>
        private double[][] CorrectInvalidParams(in double[][] parameters)
        {
            for 
            (
                int i = 0; 
                i < parameters.Length; 
                i++
            )
            {
                for (int j = 0; j < parameters[i].Length; j++)
                {
                    if (parameters[i][j] < 0)
                    {
                        parameters[i][j] = 0;
                    }
                }
            }

            return parameters;
        }
    }
}
