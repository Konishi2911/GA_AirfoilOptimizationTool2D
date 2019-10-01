using System;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    /// <summary>
    /// The Class that manages the specifications of an Airfoil.
    /// </summary>
    public class AirfoilManager
    {
        #region Fields
        private const int NumberOfControlPoint = 400;
        private const int NumberOfDivision = 100;

        private String airfoilName;

        private Airfoil.AirfoilCoordinate _importedCoordinate;
        private Airfoil.AirfoilCoordinate _interpolatedCoordinate;
        private Airfoil.AirfoilCoordinate _upperCoordinate;
        private Airfoil.AirfoilCoordinate _lowerCoordinate;

        private Double chordLength;
        private Double maximumThickness;
        private Double maximumCamber;
        private Double leadingEdgeRadius;
        #endregion

        #region Properties
        public String AirfoilName
        {
            get
            {
                return airfoilName;
            }
            private set
            {
                airfoilName = value;
            }
        }
        public Airfoil.AirfoilCoordinate InterpolatedCoordinate
        {
            get
            {
                return _interpolatedCoordinate;
            }
            private set
            {
                _interpolatedCoordinate = value;
            }
        }

        public Airfoil.AirfoilCoordinate UpperCoordinate { get { return _upperCoordinate; } }
        public Airfoil.AirfoilCoordinate LowerCoordinate { get { return _lowerCoordinate; } }

        public Double ChordLength
        {
            get
            {
                return chordLength;
            }
            private set
            {
                chordLength = value;
            }
        }
        public Double MaximumThickness { get { return maximumThickness; } }
        public Double MaximumCamber { get { return maximumCamber; } }
        public Double LeadingEdgeRadius
        {
            get
            {
                return leadingEdgeRadius;
            }
            private set
            {
                leadingEdgeRadius = value;
            }
        }
        #endregion

        private void InitializeComponent()
        {
            InterpolatedCoordinate = new AirfoilCoordinate();
            _upperCoordinate = new AirfoilCoordinate();
            _lowerCoordinate = new AirfoilCoordinate();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AirfoilManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="coordinate"></param>
        public AirfoilManager(AirfoilCoordinate coordinate)
        {
            InitializeComponent();

            _importedCoordinate = coordinate;

            // Interpolate Airfoil with three dimensional Spline.
            airfoilInterpolation();

            // 
            CalculateSpecifications();
        }

        /// <summary>
        /// Interpolate airfoil coordinate with three dimensional Spline.
        /// Substitute interpolated Airfoil coordinate to InterpolatedCoordinate.
        /// </summary>
        private void airfoilInterpolation()
        {
            var Result = General.Interpolation.SplineInterpolation(_importedCoordinate.ToDouleArray(), NumberOfControlPoint);
            InterpolatedCoordinate.Import(Result);
        }

        /// <summary>
        /// Calculate each Specifications and Substitute to Member Variables
        /// </summary>
        private void CalculateSpecifications()
        {
            var upperLine = InterpolatedCoordinate.GetUpperLine();
            var lowerLine = InterpolatedCoordinate.GetLowerLine();

            // Divide an interpolated Airfoil into UpperCoordinate and LoerCoordinate.
            UpperCoordinate.Import(General.Interpolation.LinearInterpolation(upperLine.ToDouleArray(), NumberOfDivision));
            LowerCoordinate.Import(General.Interpolation.LinearInterpolation(lowerLine.ToDouleArray(), NumberOfDivision));

            ChordLength = GetChordLength(UpperCoordinate, LowerCoordinate);
            LeadingEdgeRadius = GetLeadingEdgeRadius(UpperCoordinate, LowerCoordinate);
        }

        /// <summary>
        /// Calculate ChordLength from upper line coordinate list.
        /// </summary>
        /// <param name="upper"></param>
        /// <param name="lower"></param>
        /// <returns></returns>
        private static Double GetChordLength(in AirfoilCoordinate upper, in AirfoilCoordinate lower)
        {
            //Null Check
            if (upper == null || lower == null)
            {
                throw new ArgumentNullException("Upper Coordinate or Loewr Corrdinate is Null.");
            }
            // Format Check
            if (upper.Length != lower.Length)
            {
                throw new FormatException("The upper and lower Coordinate length did not match.");
            }

            return AirfoilCoordinate.GetMaximumValue(upper, 0) - AirfoilCoordinate.GetMinimumValue(upper, 0);
        }

        /// <summary>
        /// Calcualte
        /// </summary>
        /// <param name="upper"></param>
        /// <param name="lower"></param>
        /// <returns></returns>
        public static Double GetLeadingEdgeRadius(in AirfoilCoordinate upper, in AirfoilCoordinate lower)
        {
            //Null Check
            if (upper == null || lower == null)
            {
                throw new ArgumentNullException("Upper Coordinate or Loewr Corrdinate is Null.");
            }
            // Format Check
            if (upper.Length != lower.Length)
            {
                throw new FormatException("The upper and lower Coordinate length did not match.");
            }
            //Continuity Check
            if (upper[0].X != lower[0].X)
            {
                throw new FormatException("The upper and lower Coordinatei are Discontinuity.");
            }

            var x1 = upper[0].X;
            var x2 = upper[1].X;
            var x3 = lower[1].X;
            var z1 = upper[0].Z;
            var z2 = upper[1].Z;
            var z3 = lower[0].X;

            General.Matrix A = new General.Matrix(2);
            A[0, 0] = x1 - x2;
            A[0, 1] = z1 - z2;
            A[1, 0] = x1 - x3;
            A[1, 1] = z1 - z3;

            General.Vector b = new General.Vector(2);
            b[0] = (x1 * x1 - x2 * x2) + (z1 * z1 - z2 * z2);
            b[1] = (x1 * x1 - x3 * x3) + (z1 * z1 - z3 * z3);

            General.Vector X = new General.Vector(2);

            X = 0.5 * (A.InverceMatrix() * b);

            return Math.Sqrt(X[0] * X[0] + X[1] * X[1]);
        }
    }
}
