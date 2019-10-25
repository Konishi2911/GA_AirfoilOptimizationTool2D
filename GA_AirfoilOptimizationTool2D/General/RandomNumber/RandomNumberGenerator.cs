using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General.RandomNumber
{
    class RandomNumberGenerator
    {
        private readonly uint seed;
        private MersenneTwister mersenneTwister;
        private BoxMuller boxMuller;

        public RandomNumberGenerator() : this((uint)DateTime.Now.Millisecond + (uint)DateTime.Now.Second)
        {

        }
        public RandomNumberGenerator(uint seed)
        {
            this.seed = seed;
            mersenneTwister = new MersenneTwister(seed);
            boxMuller = new BoxMuller(seed, mersenneTwister.genrand_real3);
        }

        public double UniformRndNum()
        {
            return mersenneTwister.genrand_real3();
        }
        public double NormDistRandNum()
        {
            return boxMuller.getRand();
        }
        public double[] NormDistRandNumSeq(int n)
        {
            double[] sequence = new double[n];
            for (int i = 0; i < n; i++)
            {
                sequence[i] = UniformRndNum();
            }
            return sequence;
        }
    }
}
