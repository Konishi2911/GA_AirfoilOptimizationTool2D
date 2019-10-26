using Microsoft.VisualStudio.TestTools.UnitTesting;
using GA_AirfoilOptimizationTool2D.General.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General.Statistics.Tests
{
    [TestClass()]
    public class SamplingWithoutReplacementTests
    {
        [TestMethod()]
        public void GetIndexTest()
        {
            var n = 3U;
            var total = 10U;
            var expected = 3;

            SamplingWithoutReplacement sampling = new SamplingWithoutReplacement();

            var result = sampling.GetIndex(n, total);

            for (int i = 0; i < result.Length; i++)
            {
                Console.WriteLine(result[i]);
            }
            Assert.AreEqual(expected, result.Length);
        }
    }
}