using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General
{
    class Vector : IEquatable<Vector>
    {
        private int _size;
        private double[] _vec;

        public int Length
        {
            get
            {
                return _size;
            }
        }

        public Vector(int length)
        {
            _vec = new double[length];
            _size = _vec.Length;
        }

        public Vector(double[] vs)
        {
            _vec = vs;
            _size = _vec.Length;
        }

        public double this[int i]
        {
            set
            {
                this._vec[i] = value;
            }
            get
            {
                return this._vec[i];
            }
        }

        public static Vector operator +(Vector vec1, Vector vec2)
        {
            int length = vec1.Length;
            double[] temp = new double[length];

            for (int i = 0; i < length; i++)
            {
                temp[i] = vec1[i] + vec2[i];
            }

            return new Vector(temp);
        }

        public static Vector operator -(Vector vec1, Vector vec2)
        {
            int length = vec1.Length;
            double[] temp = new double[length];

            for (int i = 0; i < length; i++)
            {
                temp[i] = vec1[i] - vec2[i];
            }

            return new Vector(temp);
        }

        public static Vector operator *(double val, Vector vec1)
        {
            int length = vec1.Length;
            double[] temp = new double[length];

            for (int i = 0; i < length; i++)
            {
                temp[i] = val * vec1[i];
            }

            return new Vector(temp);
        }

        public static Vector operator *(Matrix mat1, Vector vec1)
        {
            int length = mat1.Length;
            double[] temp = new double[length];

            for (int i = 0; i < length; i++)
            {
                for (int k = 0; k < length; k++)
                {
                    temp[i] += mat1[i, k] * vec1[k];
                }
            }

            return new Vector(temp);
        }

        public double Norm()
        {
            double temp = 0.0;

            for (int i = 0; i < Length; i++)
            {
                temp += System.Math.Pow(_vec[i], 2);
            }

            return System.Math.Sqrt(temp);
        }

        public static double InnerProduct(Vector vec1, Vector vec2)
        {
            int length = vec1.Length;
            double temp = 0.0;

            for (int i = 0; i < length; i++)
            {
                temp += vec1[i] * vec2[i];
            }

            return temp;
        }

        public static Vector[] Orthogonalization(Vector[] vectors)
        {
            var no_of_vectors = vectors.Length;
            Vector[] o_vector = new Vector[no_of_vectors];

            for (int i = 0; i < no_of_vectors; i++)
            {
                o_vector[i] = vectors[i];
                for (int j = 0; j < i; j++)
                {
                    o_vector[i] -= (Vector.InnerProduct(o_vector[j], vectors[i]) / Vector.InnerProduct(o_vector[j], o_vector[j])) * o_vector[j];
                }
            }
            for (int i = 0; i < no_of_vectors; i++)
            {
                o_vector[i] = (1 / o_vector[i].Norm()) * o_vector[i];
            }
            return o_vector;
        }

        public static Vector E(int size, int index)
        {
            if (!(index > size))
            {
                Vector temp = new Vector(size);
                temp[index] = 1;

                return temp;
            }
            else
            {
                return null;
            }
        }

        public double[] ToDoubleArray()
        {
            double[] temp = new double[Length];
            _vec.CopyTo(temp, 0);

            return temp;
        }

        public bool Equals(Vector vec)
        {
            bool result = true;

            if (this.Length == vec.Length)
            {
                for (int i = 0; i < vec.Length && result; i++)
                {
                    result = this._vec[i] == vec[1];
                }
            }
            return result;
        }
    }
}
