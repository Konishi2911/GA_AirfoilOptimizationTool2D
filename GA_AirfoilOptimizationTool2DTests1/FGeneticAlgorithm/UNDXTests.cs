using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GA_AirfoilOptimizationTool2D.FGeneticAlgorithm.Tests
{
    [TestClass()]
    public class UNDXTests
    {
        [TestMethod()]
        public void ExecuteCrossoverTest()
        {
            UNDX_Parameters undxParameters = new UNDX_Parameters() { Alpha = 0.5, Beta = 0.35, NumberOfCrossovers = 10, NumberOfParameters = 3 };
            IndividualsGroup individualsGroup = new IndividualsGroup();
            UNDX undx = new UNDX(undxParameters);
            for (int i = 0; i < 3; i++)
            {
                individualsGroup.AddIndivisual(new Individual(new double[] { 0.9 * (i + 1), 0.2 * (i + 1), 0.4 * (i + 1) }, 0.1 * (i + 1)));

                // For Debug
                Console.WriteLine(individualsGroup.IndivisualsGroup[i].OptParameters[0] + "\t" + individualsGroup.IndivisualsGroup[i].OptParameters[1] + "\t" + individualsGroup.IndivisualsGroup[i].OptParameters[2] + "\t");
            }


            var result = undx.ExecuteCrossover(individualsGroup);


            // Displaying Result
            for (int i = 0; i < result.Length; i++)
            {
                Console.Write(i + ".   ");
                for (int j = 0; j < result[i].Length; j++)
                {
                    Console.Write(result[i][j] + "\t");
                }
                Console.Write("\r\n");
            }

        }
    }
}