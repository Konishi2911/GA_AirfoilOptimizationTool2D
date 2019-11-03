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
        private double[,] chr;
        private double[,] interpolatedChr;
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
        public Double[,] InterpolatedCharacteristics => interpolatedChr;
        #endregion

        public AngleBasedCharacteristics()
        {
            nData = 0;
        }

        private void InitializeCharaceristics(double[,] characteristics)
        {
            nData = characteristics.GetLength(0);
            this.chr = characteristics;

            searchMaxCharac(ConvertArrayToJuggedArray(this.chr));
            searchMinCharac(ConvertArrayToJuggedArray(this.chr));
            InterpolateCharacteristics();
        }
        public AngleBasedCharacteristics(double[][] characteristics)
        {
            InitializeCharaceristics(ConvertJuggedArrayToArray(characteristics));
        }

        public AngleBasedCharacteristics(double[,] characteristics)
        {
            InitializeCharaceristics(characteristics);
        }

        private void InterpolateCharacteristics()
        {
            // Interpoplate profile with 3-dimensional Spline
            var splinedChr =  General.Interpolation.SplineInterpolation(chr, 200);
            interpolatedChr = General.Interpolation.LinearInterpolation(splinedChr, 100);
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
        private T[,] ConvertJuggedArrayToArray<T>(T[][] jArray)
        {
            bool isSameSize = true;
            // FormatCheck
            for (int i = 0; i < jArray.Length - 1; i++)
            {
                isSameSize &= jArray[i] == jArray[i + 1];
            }

            if (isSameSize == false)
            {
                return null;
            }

            var length = jArray.Length;
            var width = jArray[0].Length;
            var array = new T[length, width];

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    array[i, j] = jArray[i][j];
                }
            }

            return array;
        }
    }
}
