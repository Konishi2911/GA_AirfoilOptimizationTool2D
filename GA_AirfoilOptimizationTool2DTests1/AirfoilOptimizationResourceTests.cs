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
        [DeploymentItem(@"TestData\NACA4412.csv")]
        public void SetSourceTest()
        {
            StreamReader sr1 = new StreamReader(@"DAE-11.csv");
            StreamReader sr2 = new StreamReader(@"NACA4412.csv");

            var dae = General.CsvManager.ConvertCsvToArray(sr1.ReadToEnd());
            var naca = General.CsvManager.ConvertCsvToArray(sr2.ReadToEnd());

            var daeCoordinate = new Airfoil.AirfoilCoordinate();
            var nacaCoordinate = new Airfoil.AirfoilCoordinate();

            daeCoordinate.Import(dae);
            nacaCoordinate.Import(naca);

            var DAE11 = new Airfoil.AirfoilManager(daeCoordinate);
            var NACA4412 = new Airfoil.AirfoilManager(daeCoordinate);

            var coef = new double[][] { new[] { 0.7, 0.3 }, new[] { 0.4, 0.6 } };
            var coefficients = new Airfoil.CoefficientOfCombination(General.ArrayManager.ConvertJuggedArrayToArray(coef));

            General.BasisAirfoils basisAirfoils = new General.BasisAirfoils(new[] { DAE11, NACA4412 });

            AirfoilOptimizationResource.Instance.SetSource(basisAirfoils);
            Assert.AreEqual(2, AirfoilOptimizationResource.Instance.BasisAirfoils.NumberOfAirfoils);

            AirfoilOptimizationResource.Instance.SetSource(coefficients);
        }
    }
}