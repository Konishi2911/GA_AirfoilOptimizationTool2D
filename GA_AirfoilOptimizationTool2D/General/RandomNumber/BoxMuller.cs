using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General.RandomNumber
{
    public class BoxMuller
    {
        /// <summary>
        /// The method of generating uniform random number
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public delegate double UniformRandNumMethod();
        private UniformRandNumMethod uniformMethod;

        public BoxMuller(uint seed, UniformRandNumMethod method)
        {
            uniformMethod = method;
        }

        public double getRand()
        {
            double x = uniformMethod();
            double y = uniformMethod();

            return Math.Sqrt(-2 * Math.Log(x)) * Math.Cos(2 * Math.PI * y);
        }
    }
}
