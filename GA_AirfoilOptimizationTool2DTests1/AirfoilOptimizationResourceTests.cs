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
        public void SetSource_BasisArfoilsOnly()
        {
            Airfoil.CoefficientOfCombination coefficients;
            General.BasisAirfoils basisAirfoils;
            InitializeTestVariables(out coefficients, out basisAirfoils);

            AirfoilOptimizationResource.Instance.SetSource(basisAirfoils);

            DisplayResourceValue();

            Assert.AreEqual(2, AirfoilOptimizationResource.Instance.BasisAirfoils.NumberOfAirfoils);
            Assert.AreEqual(2, AirfoilOptimizationResource.Instance.CurrentCoefficients?.NoBasisAirfoils);
        }

        private void DisplayResourceValue()
        {
            Output(AirfoilOptimizationResource.Instance.BasisAirfoils, "BasisAirfoil");
            Output(AirfoilOptimizationResource.Instance.BasisAirfoils?.NumberOfAirfoils, "BasisAirfoils.NumberOfAirfoils");
            Console.WriteLine();
            Output(AirfoilOptimizationResource.Instance.CurrentPopulations, "CurrentPopulations");
            Output(AirfoilOptimizationResource.Instance.CurrentPopulations?.NoAirfoils, "CurrentPopulations.NoAirfoils");
            Output(AirfoilOptimizationResource.Instance.CurrentPopulations?.NoBasisAirfoils, "CurrentPopulations.NoBasisAirfois");
            Console.WriteLine();
            Output(AirfoilOptimizationResource.Instance.CurrentCoefficients, "CurrentPopulations");
            Output(AirfoilOptimizationResource.Instance.CurrentCoefficients?.NoAirfoils, "CurrentCoefficient.NoAirfoils");
            Output(AirfoilOptimizationResource.Instance.CurrentCoefficients?.NoBasisAirfoils, "CurrentCoefficient.NoBasisAirfois");
        }

        [TestMethod()]
        [DeploymentItem(@"TestData\DAE-11.csv")]
        [DeploymentItem(@"TestData\NACA4412.csv")]
        public void SetSourceTest_CoefficientOnly()
        {
            Airfoil.CoefficientOfCombination coefficients;
            General.BasisAirfoils basisAirfoils;
            InitializeTestVariables(out coefficients, out basisAirfoils);

            AirfoilOptimizationResource.Instance.SetSource(coefficients);

            DisplayResourceValue();

            Assert.AreEqual(2, AirfoilOptimizationResource.Instance.CurrentCoefficients?.NoBasisAirfoils);
            Assert.AreEqual(3, AirfoilOptimizationResource.Instance.CurrentCoefficients?.NoAirfoils);
        }

        [TestMethod()]
        [DeploymentItem(@"TestData\DAE-11.csv")]
        [DeploymentItem(@"TestData\NACA4412.csv")]
        public void SetSourceTest_BothOfBAsisCoefficient()
        {
            Airfoil.CoefficientOfCombination coefficients;
            General.BasisAirfoils basisAirfoils;
            InitializeTestVariables(out coefficients, out basisAirfoils);

            AirfoilOptimizationResource.Instance.SetSource(basisAirfoils, coefficients);

            DisplayResourceValue();

            Assert.AreEqual(2, AirfoilOptimizationResource.Instance.BasisAirfoils.NumberOfAirfoils);
            Assert.AreEqual(2, AirfoilOptimizationResource.Instance.CurrentCoefficients?.NoBasisAirfoils);
            Assert.AreEqual(3, AirfoilOptimizationResource.Instance.CurrentCoefficients?.NoAirfoils);
            Assert.AreNotEqual(null, AirfoilOptimizationResource.Instance.CurrentPopulations);
            Assert.AreEqual(2, AirfoilOptimizationResource.Instance.CurrentPopulations?.NoBasisAirfoils);
            Assert.AreEqual(3, AirfoilOptimizationResource.Instance.CurrentPopulations?.NoAirfoils);
        }

        private static void InitializeTestVariables(out Airfoil.CoefficientOfCombination coefficients, out General.BasisAirfoils basisAirfoils)
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

            var coef = new double[][] { new[] { 0.7, 0.3, 0.6 }, new[] { 0.3, 0.7, 0.4 } };
            coefficients = new Airfoil.CoefficientOfCombination(General.ArrayManager.ConvertJuggedArrayToArray(coef));
            basisAirfoils = new General.BasisAirfoils(new[] { DAE11, NACA4412 });
        }

        private void Output(object item, String tag)
        {
            Console.Write(tag + " : ");
            Console.WriteLine(item ?? "NULL");
        }
    }
}