using Microsoft.VisualStudio.TestTools.UnitTesting;
using GA_AirfoilOptimizationTool2D.General.Solvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General.Solvers.Tests
{
    [TestClass()]
    public class JacobiMethodTests
    {
        [TestMethod()]
        public void SolveTest()
        {
            var coef = new double[3, 3] { { 5, 2, 3 }, { 2, 6, 1 }, { 5, 2, 7 } };
            var cnst = new double[] { 4, 3, 2 };
            var e_solution = new double[] { 1, 0.25, -0.5 };

            JacobiMethod jacobiMethod = new JacobiMethod(coef, cnst);
            jacobiMethod.Error = 1E-20;
            jacobiMethod.CheckInterval = 10;

            jacobiMethod.Solve();

            var solution = jacobiMethod.Solution;

            var accuracy = true;
            for (int i = 0; i < solution.Length; i++)
            {
                accuracy &= solution[i] == e_solution[i];
            }

            Console.Write("Number of Iteration : ");
            Console.WriteLine(jacobiMethod.NoIteration);
            Console.WriteLine(solution[0]);
            Console.WriteLine(solution[1]);
            Console.WriteLine(solution[2]);
            
            Assert.IsTrue(accuracy);
        }
    }
}