using System;

namespace GA_AirfoilOptimizationTool2D.General.Solvers
{
    public class JacobiMethod
    {
        private int _iteration;
        private int _check_interval;
        private double _error;
        private double _f_residue;
        private double[,] _coefficientMatrix;
        private double[] _constantVector;
        private double[] _solution;

        #region Properties
        public double[] Solution => _solution;
        public double Error
        {
            set => _error = value;
        }
        public int MaxIteration
        {
            set => _iteration = value;
        }
        public int CheckInterval
        {
            set => _check_interval = value;
        }
        public int NoIteration { get; private set; }
        public double FinalResidue => _f_residue;
        #endregion

        public JacobiMethod(double[,] coefficient, double[] constant)
        {
            _coefficientMatrix = coefficient;
            _constantVector = constant;
            _iteration = 1000;
            _error = 1E-20;
            _check_interval = 1;
        }

        public void Solve()
        {
            double[] x = new double[_constantVector.Length];
            double[] n_x = new double[_constantVector.Length];

            double resid = 0.0;
            double bNorm = 0.0;
            for (int i = 0; i < _coefficientMatrix.GetLength(0); i++)
            {
                bNorm += Math.Abs(_constantVector[i]) * Math.Abs(_constantVector[i]);
            }

            int k = 0;
            for (; k < _iteration; k++)
            {
                for (int i = 0; i < _coefficientMatrix.GetLength(0); i++)
                {
                    var b = _constantVector[i];
                    for (int j = 0; j < _coefficientMatrix.GetLength(1); j++)
                    {
                        if (i == j) continue;
                        b -= _coefficientMatrix[i, j] * x[j];
                    }
                    n_x[i] = b / _coefficientMatrix[i, i];
                }
                x = n_x;

                if (k % _check_interval == 0)
                {
                    resid = 0.0;
                    for (int i = 0; i < _coefficientMatrix.GetLength(0); i++)
                    {
                        var b = _constantVector[i];
                        for (int j = 0; j < _coefficientMatrix.GetLength(1); j++)
                        {
                            b -= _coefficientMatrix[i, j] * x[j];
                        }
                        resid += Math.Abs(b) * Math.Abs(b);
                    }

                    // Error check
                    if (Math.Sqrt(resid / bNorm) < _error)
                    {
                        break;
                    }
                }
            }
            _solution = n_x;
            NoIteration = k;
            _f_residue = Math.Sqrt(resid / bNorm);
        }
    }
}
