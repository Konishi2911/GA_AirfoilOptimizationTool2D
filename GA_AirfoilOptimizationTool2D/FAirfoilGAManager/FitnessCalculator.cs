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
        private FitnessMode fitnessMode;

        public double Fitness => fitness;

        public enum FitnessMode
        {
            Lift,
            Drag,
            LiftDrag
        }

        public FitnessCalculator(Airfoil.AirfoilManager airfoil, FitnessMode fitnessMode)
        {
            this.airfoil = airfoil;
            this.fitnessMode = fitnessMode;
        }

        public void CalculateFitness()
        {
            if (fitnessMode == FitnessMode.Lift)
            {
                LiftFitness();
            }
            else if (fitnessMode == FitnessMode.Drag)
            {
                DragFitness();
            }
            else if (fitnessMode == FitnessMode.LiftDrag)
            {
                LiftDragFitness();
            }
        }

        private void LiftDragFitness()
        {
            fitness = Airfoil.Characteristics.AngleBasedCharacteristics.GetMaxValue(airfoil.LiftDragProfile.InterpolatedCharacteristics);
        }
        private void LiftFitness()
        {
            fitness = Airfoil.Characteristics.AngleBasedCharacteristics.GetMaxValue(airfoil.LiftProfile.InterpolatedCharacteristics);
        }
        private void DragFitness()
        {

        }
    }
}
