using Microsoft.VisualStudio.TestTools.UnitTesting;
using GA_AirfoilOptimizationTool2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GA_AirfoilOptimizationTool2D.Tests
{
    [TestClass()]
    public class AirfoilOptimizationResourceTests
    {
        [TestMethod()]
        [DeploymentItem(@"TestData\DAE-11.csv")]
        public void SetSourceTest()
        {
            StreamReader sr = new StreamReader(@"TestData\DAE-11.csv");
            General.CsvManager.ConvertCsvToArray(sr.ReadToEnd());
            
        }
    }
}