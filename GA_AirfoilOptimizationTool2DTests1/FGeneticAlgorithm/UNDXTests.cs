using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GA_AirfoilOptimizationTool2D.FGeneticAlgorithm.Tests
{
    [TestClass()]
    public class UNDXTests
    {
        [TestMethod()]
        public void GenerateOffspringsTest()
        {
            UNDX_Parameters undxParameters = new UNDX_Parameters() { Alpha = 0.35, Beta = 0.5, NumberOfCrossovers = 10, NumberOfParameters = 3 };
            IndividualsGroup individualsGroup = new IndividualsGroup();
            UNDX undx = new UNDX(undxParameters, (opt) => 0.8);
            for (int i = 0; i < 3; i++)
            {
                individualsGroup.AddIndivisual(new Individual(new double[] { 0.4 * i, 0.2 * i, 0.4 * i }, 0.1 * i));
            }


            var expect = 0.8;

            var result = undx.GenerateOffsprings(individualsGroup);

            Console.WriteLine(result.NumberOfIndividuals);
        }
    }
}