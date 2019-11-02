using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil.Characteristics
{
    public class AngleBasedCharacteristics
    {
        private int nData;
        private double[][] chr;
        private double[][] interpolatedChr;

        public AngleBasedCharacteristics()
        {
            nData = 0;
        }

        private void InitializeCharaceristics(double[][] characteristics)
        {
            nData = characteristics.GetLength(0);
            this.chr = characteristics;
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
