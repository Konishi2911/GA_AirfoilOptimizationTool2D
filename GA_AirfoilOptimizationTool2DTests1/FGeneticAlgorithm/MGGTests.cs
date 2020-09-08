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
    public class MGGTests
    {
        [TestMethod()]
        public void ExecuteSelectionTest()
        {
            Individual[] individuals = new Individual[]
            {
                new Individual(new[] { 0.1, 0.2, 0.3 }, 1),
                new Individual(new[] { 0.2, 0.3, 0.4 }, 2),
                new Individual(new[] { 0.3, 0.4, 0.5 }, 3),
                new Individual(new[] { 0.4, 0.5, 0.6 }, 4),
                new Individual(new[] { 0.5, 0.6, 0.7 }, 5),
                new Individual(new[] { 0.6, 0.7, 0.8 }, 6)
            };

            MGG mggExecutor = new MGG();
            IndividualsGroup individualsGroup = new IndividualsGroup();
            foreach (var item in individuals)
            {
                individualsGroup.AddIndivisual(item);
            }

            for (int i = 0; i < 10000; i++)
            {
                var selectedIndividuals = mggExecutor.ExecuteSelection(individualsGroup);

                Console.WriteLine(mggExecutor.SelectedIndividualsIndex[0].ToString() + "," + mggExecutor.SelectedIndividualsIndex[1].ToString());
            }
        }
    }
}