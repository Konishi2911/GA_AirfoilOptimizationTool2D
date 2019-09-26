using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General
{
    class Matrix
    {
        private int _size;
        private double[,] _matrix;

        public Matrix()
        {

        }

        public int Length
        {
            get
            {
                return _size;
            }
        }

        public Matrix(int length)
        {
            _matrix = new double[length, length];
            _size = (Int32)System.Math.Sqrt(_matrix.Length);
        }

        public Matrix(double[,] vs)
        {
            _matrix = vs;
            _size = (Int32)System.Math.Sqrt(_matrix.Length);
        }

        public double this[int i, int j]
        {
            set
            {
                this._matrix[i, j] = value;
            }
            get
            {
                return this._matrix[i, j];
            }
        }

        public static Matrix operator +(Matrix mat1, Matrix mat2)
        {
            int length = mat1.Length;
            double[,] temp = new double[length, length];

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    temp[i, j] = mat1[i, j] + mat2[i, j];
                }
            }

            return new Matrix(temp);
        }

        public static Matrix operator -(Matrix mat1, Matrix mat2)
        {
            int length = mat1.Length;
            double[,] temp = new double[length, length];

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    temp[i, j] = mat1[i, j] - mat2[i, j];
                }
            }

            return new Matrix(temp);
        }

        public static Matrix operator *(Matrix mat1, Matrix mat2)
        {
            int length = mat1.Length;
            double[,] temp = new double[length, length];

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int k = 0; k < length; k++)
                    {
                        temp[i, j] += mat1[i, k] + mat2[k, j];
                    }
                }
            }

            return new Matrix(temp);
        }

        public Matrix InverceMatrix()
        {
            double pivot = 0.0;
            double val = 0.0;
            double divisor = 0.0;
            double[,] temp = new double[_size, _size];
            double[,] matrix = new double[_size, _size];

            Array.Copy(_matrix, matrix, _matrix.Length);

            for (int i = 0; i < _size; ++i)
            {
                temp[i, i] = 1;
            }

            for (int i = 0; i < _size; i++)
            {
                divisor = matrix[i, i];

                //Standardization
                for (int j = i; j < _size; j++)
                {
                    matrix[i, j] /= divisor;
                }
                for (int j = 0; j < _size; j++)
                {
                    temp[i, j] /= divisor;
                }

                //Elemination
                for (int k = i + 1; k < _size; k++)
                {
                    pivot = matrix[k, i];
                    for (int j = i; j < _size; j++)
                    {
                        matrix[k, j] -= matrix[i, j] * pivot;
                    }
                    for (int j = 0; j < _size; j++)
                    {
                        temp[k, j] -= temp[i, j] * pivot;
                    }
                }
            }
            for (int j = _size - 1; j >= 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    val = matrix[i, j];        //tempolary value

                    matrix[i, j] -= (matrix[j, j] * val);
                    for (int k = 0; k < _size; k++)
                    {
                        temp[i, k] -= (temp[j, k] * val);
                    }
                }
            }
            return new Matrix(temp);
        }
    }
}
