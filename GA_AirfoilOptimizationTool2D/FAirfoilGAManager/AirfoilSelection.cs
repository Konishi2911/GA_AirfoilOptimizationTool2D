using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FAirfoilGAManager
{
    public class AirfoilSelection
    {
        private int[] parentsIndex;
        private double[][] optParameters;
        private FGeneticAlgorithm.UNDX_Parameters undxParameters;
        private SelectionModel selectionModel;
        private FGeneticAlgorithm.IndividualsGroup selectedAirfoils;

        public AirfoilSelection(SelectionModel crossoverOperator)
        {
            this.selectionModel = crossoverOperator;
        }

        public enum SelectionModel
        {
            MGG
        }

        public void ExecuteSelection(Airfoil.CombinedAirfoilsGroupManager parentAirfoils)
        {
            var parentsIndividuals = CreateIndividuals(parentAirfoils);

            // MGG Selection
            if (selectionModel == SelectionModel.MGG)
            {
                var mggExecutor = new FGeneticAlgorithm.MGG();

                // Execute selection with MGG
                selectedAirfoils  = mggExecutor.ExecuteSelection(parentsIndividuals);
            }
        }

        private FGeneticAlgorithm.IndividualsGroup CreateIndividuals(Airfoil.CombinedAirfoilsGroupManager airfoilsGroup)
        {
            FGeneticAlgorithm.IndividualsGroup individuals = new FGeneticAlgorithm.IndividualsGroup();

            var airfoils = airfoilsGroup.GetCombinedAirfoilsArray();
            foreach (var item in airfoils)
            {
                double fitness = 1.0;
                individuals.AddIndivisual(new FGeneticAlgorithm.Individual(item.Coefficients, fitness));
            }

            return individuals;
        }
    }
}
