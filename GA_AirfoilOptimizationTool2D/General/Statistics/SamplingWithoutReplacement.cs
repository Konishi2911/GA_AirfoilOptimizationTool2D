using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General.Statistics
{
    public class SamplingWithoutReplacement
    {
        public delegate double RandNumMethod();
        private RandNumMethod randNumMethod;

        public SamplingWithoutReplacement()
        {
            RandomNumber.RandomNumberGenerator randomNumber = new RandomNumber.RandomNumberGenerator();
            randNumMethod = randomNumber.UniformRndNum;
        }
        public SamplingWithoutReplacement(RandNumMethod method)
        {
            randNumMethod = method;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">Number of samples</param>
        /// <param name="total">Number of population</param>
        /// <returns></returns>
        public uint[] GetIndex(uint n, uint total)
        {
            List<uint> vec = new List<uint>();
            for (uint i = 0; i < total; i++)
            {
                vec.Add(i);
            }

            List<uint> sampleVec = new List<uint>();
            for (uint i = 0; i < n; i++)
            {
                int rand = (int)Math.Floor(randNumMethod() * (n - i));
                // Select index
                sampleVec.Add(vec[rand]);
                // Remove selected index
                vec.RemoveAt(rand);
            }

            return vec.ToArray();
        }
    }
}
