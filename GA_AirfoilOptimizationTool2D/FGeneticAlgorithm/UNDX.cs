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

        public UNDX(UNDX_Parameters parameters)
        {
            nCrossover = parameters.NumberOfCrossovers;
            nParams = parameters.NumberOfParameters;
            alpha = parameters.Alpha;
            beta = parameters.Beta;

            numberGenerator = new General.RandomNumber.RandomNumberGenerator();
        }

        /// <summary>
        /// Execute the crossover with UNDX
        /// </summary>
        /// <param name="parents">Individuals of previous generation</param>
        /// <returns>If all crossovers are succesful, offsprings are returned, otherwise null is returned.</returns>
        public double[][] ExecuteCrossover(IndividualsGroup parents)
        {
            Airfoil.CombinedAirfoilsGroupManager offsptingAirfoils = new Airfoil.CombinedAirfoilsGroupManager(nCrossover);
            Airfoil.AirfoilManager[] selectedParents = new Airfoil.AirfoilManager[2];
            General.RandomNumber.RandomNumberGenerator randomNumberGenerator = new General.RandomNumber.RandomNumberGenerator();
            General.Statistics.SamplingWithoutReplacement sampling = new General.Statistics.SamplingWithoutReplacement();

            // Random Airfoil Selection
            uint[] pIndex = sampling.GetIndex(3, (uint)parents.NumberOfIndividuals);

            // Create Optimization Parameter Vector

            General.Vector p1 = new General.Vector(nParams);
            General.Vector p2 = new General.Vector(nParams);
            General.Vector p3 = new General.Vector(nParams);

            for (int i = 0; i < nParams; i++)
            {
                p1[i] = parents.IndivisualsGroup[(int)pIndex[0]].OptParameters[i];
                p2[i] = parents.IndivisualsGroup[(int)pIndex[1]].OptParameters[i];
                p3[i] = parents.IndivisualsGroup[(int)pIndex[2]].OptParameters[i];
            }
            // If same parameter vector are selected, return null to retry selection.
            if (p1.Equals(p2) && p1.Equals(p3))
            {
                return null;
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

            // Create Offspring's parameter vector
            for (int i = 0; i < nCrossover; i++)
            {
                var StdOffspringPts = randomNumberGenerator.NormDistRandNumSeq(nParams);
                offspringParameter[i] = new General.Vector(StdOffspringPts);

                var e1 = 1 / (p2 - p1).Norm() * (p2 - p1);
                var pAxisComp = General.Vector.InnerProduct(offspringParameter[i], e1);

                offspringParameter[i] -= pAxisComp * e1;
                offspringParameter[i] = sigma2 * offspringParameter[i];
                offspringParameter[i] += sigma1 * pAxisComp * e1;

                offspringParameterArray[i] = offspringParameter[i].ToDoubleArray();
            }
            return offspringParameterArray;
        }

    }
}
