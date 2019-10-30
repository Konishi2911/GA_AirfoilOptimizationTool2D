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
        private List<Airfoil.Representation.AirfoilCombiner> selectedAirfoils;
        #endregion

        #region Properties
        public List<Airfoil.Representation.AirfoilCombiner> SelectedAirfoils => selectedAirfoils;
        #endregion

        public AirfoilSelection(SelectionModel crossoverOperator)
        {
            this.selectionModel = crossoverOperator;
        }

        public enum SelectionModel
        {
            MGG
        }

        public void ExecuteSelection(Airfoil.CombinedAirfoilsGroupManager offspringAirfoils)
        {
            var parentsIndividuals = CreateIndividuals(offspringAirfoils);

            // MGG Selection
            if (selectionModel == SelectionModel.MGG)
            {
                var mggExecutor = new FGeneticAlgorithm.MGG();

                // Execute selection with MGG
                selectedIndividuals  = mggExecutor.ExecuteSelection(parentsIndividuals);
                var selectedIndex = mggExecutor.SelectedIndividualsIndex;

                // Store selected Airfoils' characteristics
                selectedAirfoils = new List<Airfoil.Representation.AirfoilCombiner>();
                for (int i = 0; i < selectedIndex.Length; i++)
                {
                    SelectedAirfoils.Add(offspringAirfoils.GetCombinedAirfoilsArray()[i]);
                }
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
