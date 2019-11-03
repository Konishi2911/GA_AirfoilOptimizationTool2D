using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil.Characteristics
{
    public class AngleBasedCharacteristics
    {
        #region Fields
        private int nData;
        private double[][] chr;
        private double[][] interpolatedChr;
        private double max;
        private double maxAngle;
        private double min;
        private double minAngle;
        #endregion

        #region Properties
        public Double Max => max;
        public Double MaxAngle => maxAngle;
        public Double Min => min;
        public Double MinAngle => minAngle;
        #endregion

        public AngleBasedCharacteristics()
        {
            nData = 0;
        }

        private void InitializeCharaceristics(double[][] characteristics)
        {
            nData = characteristics.GetLength(0);
            this.chr = characteristics;
            searchMaxCharac(this.chr);
            searchMinCharac(this.chr);  
        }
        public AngleBasedCharacteristics(double[][] characteristics)
        {
            InitializeCharaceristics(characteristics);
        }

        public AngleBasedCharacteristics(double[,] characteristics)
        {
            var jArray = ConvertArrayToJuggedArray(characteristics);
            InitializeCharaceristics(jArray);
        }

        private void searchMaxCharac(double[][] reference)
        {
            var max = reference[0][0];
            var maxIndex = 0;
            for (int i = 1; i < reference.Length; i++)
            {
                if (reference[i][1] > max)
                {
                    max = reference[i][1];
                    maxIndex = i;
                }
            }

            this.max = max;
            this.maxAngle = reference[maxIndex][0];
        }
        private void searchMinCharac(double[][] reference)
        {
            var min = reference[0][0];
            var minIndex = 0;
            for (int i = 1; i < reference.Length; i++)
            {
                if (reference[i][1] < min)
                {
                    min = reference[i][1];
                    minIndex = i;
                }
            }

            this.min = min;
            this.minAngle = reference[minIndex][0];
        }
        private T[][] ConvertArrayToJuggedArray<T>(T[,] array)
        {
            var length = array.GetLength(0);
            var width = array.GetLength(1);
            var jArray = new T[length][];

            for (int i = 0; i < length; i++)
            {
                jArray[i] = new T[width];
                for (int j = 0; j < width; j++)
                {
                    jArray[i][j] = array[i, j];
                }
            }

            return jArray;
        }
    }
}
