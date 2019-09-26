using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General
{
    static class Interpolation
    {
        public static List<System.Windows.Point> LinearInterpolation(List<System.Windows.Point> point_vector, int splits)
        {
            int no_of_points = point_vector.Count;
            int least_index = 0;
            double[] x = new double[splits];
            double[] y = new double[splits];
            List<System.Windows.Point> new_list = new List<System.Windows.Point>();

            for (int k = 0; k < splits; k++)
            {
                var max = (search_max(point_vector, XorY.X) - search_min(point_vector, XorY.X));
                x[k] = max * k / splits;
            }

            for (int k = 0; k < splits; ++k)
            {
                for (int i = least_index; i < no_of_points - 1; i++)
                {
                    least_index = i;

                    if (point_vector[i].X <= x[k] && x[k] < point_vector[i + 1].X)
                    {
                        y[k] = (point_vector[i + 1].Y - point_vector[i].Y) / (point_vector[i + 1].X - point_vector[i].X) * (x[k] - point_vector[i].X) + point_vector[i].Y;
                        break;
                    }

                    if (i == no_of_points - 2)
                    {
                        least_index = 0;
                    }
                }

            }

            for (int k = 0; k < splits; ++k)
            {
                System.Windows.Point temp = new System.Windows.Point(x[k], y[k]);
                new_list.Add(temp);
            }
            return new_list;
        }

        public static double[,] SplineInterpolation(double[,] point_vector, int splits)
        {
            // Convert Double type array to List<Point> type.
            var iLength = point_vector.GetLength(0);
            var converted_point_vector = new List<System.Windows.Point>();
            for (int i = 0; i < iLength; i++)
            {
                var point = new System.Windows.Point { X = point_vector[i, 0], Y = point_vector[i, 1] };
                converted_point_vector.Add(point);
            }

            // Execute interpolation.
            var result = SplineInterpolation(converted_point_vector, splits);

            // Convert List<Point> type to Double type array.
            var rLength = result.Count;
            var convertedResult = new double[rLength, 2];
            for (int i = 0; i < rLength; i++)
            {
                convertedResult[i, 0] = result[i].X;
                convertedResult[i, 1] = result[i].Y;
            }
            
            return convertedResult;
        }
        public static List<System.Windows.Point> SplineInterpolation(List<System.Windows.Point> point_vector, int _splits)
        {
            int n = point_vector.Count - 1;
            var splits = _splits / n;

            double[] ax = new double[n + 1];
            double[] bx = new double[n + 1];
            double[] cx = new double[n + 1];
            double[] dx = new double[n + 1];

            double[] ay = new double[n + 1];
            double[] by = new double[n + 1];
            double[] cy = new double[n + 1];
            double[] dy = new double[n + 1];

            double[] u = new double[splits];

            General.Matrix A = new General.Matrix(n + 1);
            General.Vector vec_bx = new General.Vector(n + 1);
            General.Vector vec_cx = new General.Vector(n + 1);
            General.Vector vec_by = new General.Vector(n + 1);
            General.Vector vec_cy = new General.Vector(n + 1);

            List<System.Windows.Point> new_list = new List<System.Windows.Point>();

            for (int i = 0; i < n + 1; i++)
            {
                ax[i] = point_vector[i].X;
                ay[i] = point_vector[i].Y;
            }

            A[0, 0] = 1;
            A[n, n] = 1;
            for (int i = 1; i < n; i++)
            {
                A[i, i - 1] = 1;
                A[i, i] = 4;
                A[i, i + 1] = 1;
            }

            vec_bx[0] = 0;
            vec_bx[n] = 0;
            vec_by[0] = 0;
            vec_by[n] = 0;
            for (int i = 1; i < n; i++)
            {
                vec_bx[i] = 3 * (ax[i + 1] - 2 * ax[i] + ax[i - 1]);
                vec_by[i] = 3 * (ay[i + 1] - 2 * ay[i] + ay[i - 1]);
            }

            cx = (A.InverceMatrix() * vec_bx).ToDoubleArray();
            cy = (A.InverceMatrix() * vec_by).ToDoubleArray();

            for (int i = 0; i < n; i++)
            {
                bx[i] = (ax[i + 1] - ax[i]) - (cx[i + 1] + 2 * cx[i]) / 3;
                dx[i] = (cx[i + 1] - cx[i]) / 3;

                by[i] = (ay[i + 1] - ay[i]) - (cy[i + 1] + 2 * cy[i]) / 3;
                dy[i] = (cy[i + 1] - cy[i]) / 3;
            }

            //Generate x point
            for (int k = 0; k < splits; k++)
            {
                u[k] = (double)k / (double)splits;
            }

            for (int i = 0; i < n; i++)
            {
                for (int k = 0; k < splits; k++)
                {
                    var x = ax[i] + bx[i] * u[k] + cx[i] * System.Math.Pow(u[k], 2) + dx[i] * System.Math.Pow(u[k], 3);
                    var y = ay[i] + by[i] * u[k] + cy[i] * System.Math.Pow(u[k], 2) + dy[i] * System.Math.Pow(u[k], 3);

                    new_list.Add(new System.Windows.Point(x, y));
                }
            }
            new_list.Add(point_vector[n]);
            return new_list;
        }

        private static List<System.Windows.Point> Spline(List<System.Windows.Point> point_vector, int splits)
        {
            List<System.Windows.Point> temp1 = new List<System.Windows.Point>();

            General.Vector Y = new General.Vector(point_vector.Count);
            General.Vector a = new General.Vector(point_vector.Count);
            General.Matrix A = new General.Matrix(point_vector.Count);

            double[] x = new double[splits];
            double[] y = new double[splits];

            for (int i = 0; i < point_vector.Count; i++)
            {
                double temp = point_vector[i].X;
                y[i] = point_vector[i].Y;

                for (int j = 0; j < point_vector.Count; j++)
                {
                    A[i, j] = System.Math.Pow(temp, j);
                }
            }
            a = A.InverceMatrix() * Y;

            for (int k = 0; k < splits; k++)
            {
                var max = (search_max(point_vector, XorY.X) - search_min(point_vector, XorY.X));
                x[k] = max * k / splits;
            }
            for (int k = 0; k < splits; k++)
            {
                for (int i = 0; i < point_vector.Count; i++)
                {
                    y[k] += a[i] * System.Math.Pow(x[k], i);
                }
            }

            for (int k = 0; k < splits; ++k)
            {
                System.Windows.Point temp = new System.Windows.Point(x[k], y[k]);
                temp1.Add(temp);
            }

            return temp1;
        }

        public static List<System.Windows.Point> B_Spline(List<System.Windows.Point> point_vector, int splits)
        {
            int no_of_points = point_vector.Count;
            const int N = 4;
            int m = no_of_points + N + 1;

            double[] u = GenerateKnotVector(N, m);
            double[] t = new double[splits];

            List<System.Windows.Point> temp1 = new List<System.Windows.Point>();

            for (int i = 0; i < splits; ++i)
            {
                t[i] = (double)i / (double)splits;
            }

            for (int k = 0; k < splits; k++)
            {
                System.Windows.Point p = new System.Windows.Point();
                for (int i = 0; i < m - N - 1; i++)
                {
                    p.X += point_vector[i].X * BasisFunc(u, i, N, t[k]);
                    p.Y += point_vector[i].Y * BasisFunc(u, i, N, t[k]);
                }
                temp1.Add(p);
            }
            return temp1;
        }

        private static double[] GenerateKnotVector(int n, int m)
        {
            double t_0 = 0.0;
            double t_m = 1.0;
            double[] t = new double[m];

            for (int i = 0; i < m; ++i)
            {
                if (i < n + 1) t[i] = 0;
                else if (i >= m - n - 1) t[i] = 1;
                else t[i] = t_0 + (t_m - t_0) / (m - 1) * i;
            }

            return t;
        }

        private static double BasisFunc(double[] u, int j, int k, double t)
        {
            double b;

            if (k == 0)
            {
                if (u[j] <= t && t < u[j + 1])
                {
                    b = 1;
                }
                else
                {
                    b = 0;
                }
            }
            else
            {
                double temp1 = 0, temp2 = 0;

                if ((u[j + k] - u[j]) != 0)
                {
                    temp1 = (t - u[j]) / (u[j + k] - u[j]);
                }
                if ((u[j + k + 1] - u[j + 1]) != 0)
                {
                    temp2 = (u[j + k + 1] - t) / (u[j + k + 1] - u[j + 1]);
                }

                b = temp1 * BasisFunc(u, j, k - 1, t) + temp2 * BasisFunc(u, j + 1, k - 1, t);
            }

            return b;
        }

        private enum XorY
        {
            X,
            Y
        }

        private static double search_max(List<System.Windows.Point> cdt, XorY xy)
        {
            double max = 0.0;
            bool isFirst = true;

            if (xy == XorY.X)
            {
                foreach (var value in cdt)
                {
                    if (isFirst)
                    {
                        max = value.X;
                        isFirst = false;
                        continue;
                    }
                    if (max <= value.X) max = value.X;
                }
            }
            else if (xy == XorY.Y)
            {
                foreach (var value in cdt)
                {
                    if (isFirst)
                    {
                        max = value.Y;
                        isFirst = false;
                        continue;
                    }
                    if (max <= value.Y) max = value.Y;
                }
            }

            return max;
        }

        private static double search_min(List<System.Windows.Point> cdt, XorY xy)
        {
            double min = 0.0;
            bool isFirst = true;

            if (xy == XorY.X)
            {
                foreach (var value in cdt)
                {
                    if (isFirst)
                    {
                        min = value.X;
                        isFirst = false;
                        continue;
                    }
                    if (min >= value.X) min = value.X;
                }
            }
            else if (xy == XorY.Y)
            {
                foreach (var value in cdt)
                {
                    if (isFirst)
                    {
                        min = value.Y;
                        isFirst = false;
                        continue;
                    }
                    if (min >= value.Y) min = value.Y;
                }
            }

            return min;
        }
    }
}
