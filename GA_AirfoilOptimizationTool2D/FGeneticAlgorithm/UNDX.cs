using System;

namespace GA_AirfoilOptimizationTool2D.FGeneticAlgorithm
{
    public class UNDX
    {
        readonly int nCrossover;
        readonly int nParams;
        readonly double alpha;
        readonly double beta;

        General.RandomNumber.RandomNumberGenerator numberGenerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optParameters">Optimizing Parameters Array</param>
        /// <returns></returns>
        public delegate double EvaluationMethod(double[][] optParameters);
        EvaluationMethod evaluatingMethod;

        public UNDX(UNDX_Parameters parameters, EvaluationMethod evaluatingMethod)
        {
            nCrossover = parameters.NumberOfCrossovers;
            nParams = parameters.NumberOfParameters;
            alpha = parameters.Alpha;
            beta = parameters.Beta;

            this.evaluatingMethod = evaluatingMethod;
            numberGenerator = new General.RandomNumber.RandomNumberGenerator();
        }

        /// <summary>
        /// Execute the crossover with UNDX
        /// </summary>
        /// <param name="parents">Individuals of previous generation</param>
        /// <returns>Offsprings</returns>
        public IndividualsGroup GenerateOffsprings(IndividualsGroup parents)
        {
            Airfoil.CombinedAirfoilsGroupManager offsptingAirfoils = new Airfoil.CombinedAirfoilsGroupManager(nCrossover);
            IndividualsGroup offspring = new IndividualsGroup();
            Airfoil.AirfoilManager[] selectedParents = new Airfoil.AirfoilManager[2];
            General.RandomNumber.RandomNumberGenerator randomNumberGenerator = new General.RandomNumber.RandomNumberGenerator();
            General.Statistics.SamplingWithoutReplacement sampling = new General.Statistics.SamplingWithoutReplacement();

            // Random Airfoil Selection
            uint[] pIndex = sampling.GetIndex(3, (uint)parents.NumberOfIndividuals);

            // Create Optimization Parameter Vector

            General.Vector p1 = new General.Vector(nCrossover);
            General.Vector p2 = new General.Vector(nCrossover);
            General.Vector p3 = new General.Vector(nCrossover);

            for (int i = 0; i < nCrossover; i++)
            {
                p1[i] = parents.IndivisualsGroup[(int)pIndex[1]].OptParameters[i];
                p2[i] = parents.IndivisualsGroup[(int)pIndex[2]].OptParameters[i];
                p3[i] = parents.IndivisualsGroup[(int)pIndex[3]].OptParameters[i];
            }

            int n = parents.NumberOfIndividuals;

            double d1 = (p2 - p1).Norm();
            double d2 = (General.Vector.InnerProduct(p3 - p1, p2 - p1) / Math.Pow((p2 - p1).Norm(), 2) * (p2 - p1) - p3).Norm();
            double sigma1 = alpha * d1;
            double sigma2 = beta * d2 / Math.Sqrt(n);
            General.Vector m = (1 / 2) * (p1 + p2);

            // Create new parameter vector for offsprings
            General.Vector[] offspringParameter = new General.Vector[nCrossover];
            double[][] offspringParameterArray = new double[nCrossover][];

            // Generate Random Number with Normal Dist
            // Primary axis
            var pStdOffspringPts = randomNumberGenerator.NormDistRandNumSeq(nCrossover);
            // Secondary axis
            var sStdOffspringPts = randomNumberGenerator.NormDistRandNumSeq(nCrossover);

            double[] pOffspringPts = new double[nCrossover];
            double[] sOffspringPts = new double[nCrossover];

            for (int i = 0; i < nCrossover; i++)
            {
                pOffspringPts[i] = pStdOffspringPts[i] * sigma1;
                sOffspringPts[i] = sStdOffspringPts[i] * sigma2;

                offspringParameter[i] = ;

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
