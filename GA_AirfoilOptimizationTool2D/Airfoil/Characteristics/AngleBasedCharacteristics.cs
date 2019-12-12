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
        private int nInterpolatedPoints;
        private int nData;
        private double[,] chr;
        private double[,] interpolatedChr;
        private double max;
        private double maxAngle;
        private double min;
        private double minAngle;
        #endregion

        #region Properties
        public int NoInterpolatedPoints 
        {
            get => nInterpolatedPoints;
            set
            {
                nInterpolatedPoints = value;
                InterpolateCharacteristics();
            }
        }
        public Double Max => max;
        public Double MaxAngle => maxAngle;
        public Double Min => min;
        public Double MinAngle => minAngle;
        public Double[,] InterpolatedCharacteristics => interpolatedChr;
        #endregion

        private void InitializeCharaceristics(double[,] characteristics)
        {
            nData = characteristics.GetLength(0);
            this.chr = characteristics;

            searchMaxCharac(ConvertArrayToJuggedArray(this.chr));
            searchMinCharac(ConvertArrayToJuggedArray(this.chr));
            InterpolateCharacteristics();
        }
        public AngleBasedCharacteristics()
        {
            nData = 0;
            nInterpolatedPoints = 200;
        }
        public AngleBasedCharacteristics(double[][] characteristics)
        {
            nInterpolatedPoints = 200;
            InitializeCharaceristics(General.ArrayManager.ConvertJuggedArrayToArray(characteristics));
        }

        public AngleBasedCharacteristics(double[,] characteristics)
        {
            nInterpolatedPoints = 200;
            InitializeCharaceristics(characteristics);
        }

        public AngleBasedCharacteristics(double[,] characteristics, int nInterPoint)
        {
            nInterpolatedPoints = nInterPoint;
            InitializeCharaceristics(characteristics);
        }

        public static double GetMaxValue(double[,] characteristics)
        {
            int maxIndex;
            double max = 0.0;
            searchMax(ConvertArrayToJuggedArray(characteristics), out max, out maxIndex);

            return max;
        }
        public static double GetMinValue(double[,] characteristics)
        {
            int minIndex;
            double min = 0.0;
            searchMax(ConvertArrayToJuggedArray(characteristics), out min, out minIndex);

            return min;
        }

        private void InterpolateCharacteristics()
        {
            // Interpoplate profile with 3-dimensional Spline
            var splinedChr =  General.Interpolation.SplineInterpolation(chr, nInterpolatedPoints);
            interpolatedChr = General.Interpolation.LinearInterpolation(splinedChr, nInterpolatedPoints);
        }
        private void searchMaxCharac(double[][] reference)
        {
            double max;
            int maxIndex;

            searchMax(reference, out max, out maxIndex);

            this.max = max;
            this.maxAngle = reference[maxIndex][0];
        }

        private static void searchMax(double[][] reference, out double max, out int maxIndex)
        {
            max = reference[0][0];
            maxIndex = 0;
            for (int i = 1; i < reference.Length; i++)
            {
                if (reference[i][1] > max)
                {
                    max = reference[i][1];
                    maxIndex = i;
                }
            }
        }

        private void searchMinCharac(double[][] reference)
        {
            double min;
            int minIndex;

            searchMin(reference, out min, out minIndex);

            this.min = min;
            this.minAngle = reference[minIndex][0];
        }

        private static void searchMin(double[][] reference, out double min, out int minIndex)
        {
            min = reference[0][0];
            minIndex = 0;
            for (int i = 1; i < reference.Length; i++)
            {
                if (reference[i][1] < min)
                {
                    min = reference[i][1];
                    minIndex = i;
                }
            }
        }

        private static T[][] ConvertArrayToJuggedArray<T>(T[,] array)
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
        private static T[,] ConvertJuggedArrayToArray<T>(T[][] jArray)
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
