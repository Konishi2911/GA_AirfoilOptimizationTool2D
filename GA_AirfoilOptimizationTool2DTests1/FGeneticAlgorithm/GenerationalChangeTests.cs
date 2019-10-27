using Microsoft.VisualStudio.TestTools.UnitTesting;
using GA_AirfoilOptimizationTool2D.FGeneticAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FGeneticAlgorithm.Tests
{
    [TestClass()]
    public class GenerationalChangeTests
    {
        [TestMethod()]
        public void ExecuteGenerationChangeTest()
        {
            GenerationalChange generationalChange = new GenerationalChange(evaluationTest, GenerationalChange.GenerationChangeModel.UNDX_MGG);
            IndividualsGroup parents = new IndividualsGroup();
            for (int i = 0; i < 3; i++)
            {
                parents.AddIndivisual(new Individual(new double[] { 0.9 * (i + 1), 0.2 * (i + 1), 0.4 * (i + 1) }, 0.1 * (i + 1)));

                // For Debug
                Console.WriteLine(parents.IndivisualsGroup[i].OptParameters[0] + "\t" + parents.IndivisualsGroup[i].OptParameters[1] + "\t" + parents.IndivisualsGroup[i].OptParameters[2] + "\t");
            }

            var result = generationalChange.ExecuteGenerationChange(parents);

            for (int i = 0; i < 3; i++)
            {
                // For Debug
                Console.WriteLine(result.IndivisualsGroup[i].OptParameters[0] + "\t" + result.IndivisualsGroup[i].OptParameters[1] + "\t" + result.IndivisualsGroup[i].OptParameters[2] + "\t");
            }
        }

        private double[] evaluationTest(double[][] optParams)
        {
            double[] result = new double[optParams.Length];
            for (int i = 0; i < optParams.Length; i++)
            {
                result[i] = 0.4 * i;
            }

            return result;
        }
    }
}