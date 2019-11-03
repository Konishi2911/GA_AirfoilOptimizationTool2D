using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FAirfoilGAManager
{
    public class FitnessCalculator
    {
        private Airfoil.AirfoilManager airfoil;
        private double fitness;

        public double Fitness => fitness;

        public FitnessCalculator(Airfoil.AirfoilManager airfoil)
        {
            this.airfoil = airfoil;
        }

        public void CalculateFitness()
        {

        }

        private void LiftDragFitness()
        {
            fitness = airfoil.LiftProfile.InterpolatedCharacteristics
        }
        private void LiftFitness()
        {
            fitness = airfoil.LiftProfile.InterpolatedCharacteristics
        }
        private void DragFitness()
        {

        }
    }
}
