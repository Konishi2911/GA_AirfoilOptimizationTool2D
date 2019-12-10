using Microsoft.VisualStudio.TestTools.UnitTesting;
using GA_AirfoilOptimizationTool2D.General.RandomNumber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General.RandomNumber.Tests
{
    [TestClass()]
    public class RandomNumberGeneratorTests
    {
        [TestMethod()]
        public void NormDistRandNumSeqTest()
        {
            uint seed = 0;
            int sequenceLength = 10000;
            RandomNumberGenerator randomNumber = new RandomNumberGenerator();

            var result = randomNumber.NormDistRandNumSeq(sequenceLength);

            for (int i = 0; i < result.Length; i++)
            {
                Console.WriteLine(result[i]);
            }

        }

        [TestMethod()]
        public void UniformRndNumTest()
        {
            uint seed = 0;
            int sequenceLength = 1000;
            RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();

            for (int i = 0; i < sequenceLength; i++)
            {
                var result = randomNumberGenerator.UniformRndNum();

                Console.WriteLine(result);
            }
        }
    }
}