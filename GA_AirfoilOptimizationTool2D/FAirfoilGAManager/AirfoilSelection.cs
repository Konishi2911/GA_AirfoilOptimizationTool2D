using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FAirfoilGAManager
{
    public class AirfoilSelection
    {
        #region Fields
        private SelectionModel selectionModel;
        private FGeneticAlgorithm.IndividualsGroup selectedIndividuals;
        private Airfoil.CombinedAirfoilsGroup selectedAirfoils;
        private List<int> selectedAirfoilsNo;
        #endregion

        #region Properties
        public Airfoil.CombinedAirfoilsGroup SelectedAirfoils => selectedAirfoils;
        public List<int> SelectedAirfoilsNo
        {
            get => selectedAirfoilsNo;
            private set => selectedAirfoilsNo = value;
        }
        #endregion

        public AirfoilSelection(SelectionModel crossoverOperator)
        {
            this.selectionModel = crossoverOperator;
        }

        public enum SelectionModel
        {
            MGG
        }

        public void ExecuteSelection(Airfoil.CombinedAirfoilsGroup offspringAirfoils)
        {
            // Create Indiviuals
            var parentsIndividuals = CreateIndividuals(offspringAirfoils);

            // MGG Selection
            if (selectionModel == SelectionModel.MGG)
            {
                var mggExecutor = new FGeneticAlgorithm.MGG();

                // Execute selection with MGG
                selectedIndividuals  = mggExecutor.ExecuteSelection(parentsIndividuals);
                var selectedIndex = mggExecutor.SelectedIndividualsIndex;

                // Store selected Airfoils' characteristics
                selectedAirfoils = new Airfoil.CombinedAirfoilsGroup(offspringAirfoils.BasisAirfoils);
                for (int i = 0; i < selectedIndex.Length; i++)
                {
                    Airfoil.AirfoilManager airfoil = offspringAirfoils.CombinedAirfoils[selectedIndex[i]];
                    double[] coefficients = offspringAirfoils.CoefficientOfCombination.GetCoefficients(selectedIndex[i]);

                    SelectedAirfoilsNo = new List<int>(selectedIndex);
                    SelectedAirfoils.Add(airfoil, coefficients);
                }
            }
        }

        private FGeneticAlgorithm.IndividualsGroup CreateIndividuals(Airfoil.CombinedAirfoilsGroup airfoilsGroup)
        {
            FGeneticAlgorithm.IndividualsGroup individuals = new FGeneticAlgorithm.IndividualsGroup();

            var airfoils = airfoilsGroup.CombinedAirfoils;
            var fitness = new List<double>();
            foreach (var item in airfoils)
            {
                //double fitness = 1.0;
                // Calculate fintness based on Lift
                FitnessCalculator fitnessCalculator = new FitnessCalculator(item, FitnessCalculator.FitnessMode.LiftDrag);
                fitnessCalculator.CalculateFitness();
                fitness.Add(fitnessCalculator.Fitness);
            }

            var coefficients = airfoilsGroup.CoefficientOfCombination;
            for (int i = 0; i < airfoils.Length; i++)
            {
                individuals.AddIndivisual(new FGeneticAlgorithm.Individual(coefficients.GetCoefficients(i), fitness[i]));
            }

            return individuals;
        }
    }
}
