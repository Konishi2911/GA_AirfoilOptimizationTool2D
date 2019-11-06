using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    /// <summary>
    /// The Class that manages the specifications of an Airfoil.
    /// </summary>
    public class AirfoilManager
    {
        #region Fields
        private const int NumberOfControlPoint = 600;
        private const int NumberOfDivision = GeneralConstants.NUMBER_OF_DIVISION;

        private String airfoilName;

        private Airfoil.AirfoilCoordinate _importedCoordinate;
        private Airfoil.AirfoilCoordinate _interpolatedCoordinate;
        private Airfoil.AirfoilCoordinate _upperCoordinate;
        private Airfoil.AirfoilCoordinate _lowerCoordinate;

        private Double chordLength;
        private Double maximumThickness;
        private Double maximumCamber;
        private Double leadingEdgeRadius;

        private Characteristics.AngleBasedCharacteristics liftProfile;
        private Characteristics.AngleBasedCharacteristics dragProfile;
        private Characteristics.AngleBasedCharacteristics momentProfile;
        #endregion

        #region Properties
        public String AirfoilName
        {
            get
            {
                return airfoilName;
            }
            set
            {
                airfoilName = value;
            }
        }
        public Airfoil.AirfoilCoordinate ImportedCoordinate
        {
            get => _importedCoordinate;
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
        public Airfoil.AirfoilCoordinate ResizedCoordinate { get; private set; }

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

        public Characteristics.AngleBasedCharacteristics LiftProfile
        {
            get => liftProfile;
            set
            {
                liftProfile = value;
            }
        }
        public Characteristics.AngleBasedCharacteristics DragProfile
        {
            get => dragProfile;
            set
            {
                dragProfile = value;
            }
        }
        #endregion

        private void InitializeComponent()
        {
            InterpolatedCoordinate = new AirfoilCoordinate();
            _upperCoordinate = new AirfoilCoordinate();
            _lowerCoordinate = new AirfoilCoordinate();
            ResizedCoordinate = new AirfoilCoordinate();
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

            var tempCoordinate = RefineCoordinate(coordinate);
            _importedCoordinate = RefineCoordinate(tempCoordinate);

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

            // Combinate the Coordinates Upper and Lower
            Double[,] resizedCoordinate = new double[UpperCoordinate.Length + LowerCoordinate.Length, 2];
            for (int i = 0; i < UpperCoordinate.Length; i++)
            {
                resizedCoordinate[i, 0] = UpperCoordinate[i].X;
                resizedCoordinate[i, 1] = UpperCoordinate[i].Z;
            }
            for (int i = UpperCoordinate.Length; i < UpperCoordinate.Length + LowerCoordinate.Length - 1; i++)
            {
                resizedCoordinate[i, 0] = LowerCoordinate[i - UpperCoordinate.Length].X;
                resizedCoordinate[i, 1] = LowerCoordinate[i - UpperCoordinate.Length].Z;
            }
            ResizedCoordinate.Import(resizedCoordinate);

            // Calculate Specifications
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

        public AirfoilCoordinate GetResizedAirfoil(int x_splits)
        {
            AirfoilCoordinate upperCoordinate = new AirfoilCoordinate();
            AirfoilCoordinate lowerCoordinate = new AirfoilCoordinate();
            AirfoilCoordinate resizedCoordinate = new AirfoilCoordinate();


            var upperLine = InterpolatedCoordinate.GetUpperLine();
            var lowerLine = InterpolatedCoordinate.GetLowerLine();

            // Divide an interpolated Airfoil into UpperCoordinate and LoerCoordinate.
            upperCoordinate.Import(General.Interpolation.LinearInterpolation(upperLine.ToDouleArray(), x_splits));
            lowerCoordinate.Import(General.Interpolation.LinearInterpolation(lowerLine.ToDouleArray(), x_splits));

            // Combinate the Coordinates Upper and Lower
            Double[,] resizedCoordinateArray = new double[UpperCoordinate.Length + LowerCoordinate.Length, 2];
            for (int i = 0; i < UpperCoordinate.Length; i++)
            {
                resizedCoordinateArray[i, 0] = UpperCoordinate[UpperCoordinate.Length - i - 1].X;
                resizedCoordinateArray[i, 1] = UpperCoordinate[UpperCoordinate.Length - i - 1].Z;
            }
            for (int i = UpperCoordinate.Length; i < UpperCoordinate.Length + LowerCoordinate.Length; i++)
            {
                resizedCoordinateArray[i, 0] = LowerCoordinate[i - UpperCoordinate.Length].X;
                resizedCoordinateArray[i, 1] = LowerCoordinate[i - UpperCoordinate.Length].Z;
            }
            resizedCoordinate.Import(resizedCoordinateArray);

            return resizedCoordinate;
        }

        /// <summary>
        /// This method returns a object of AirfoilCoordinate removed continuously duplicated value.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        private AirfoilCoordinate RefineCoordinate(AirfoilCoordinate coordinate)
        {
            var refined = new List<Double[]>();
            AirfoilCoordinate.Coordinate newValue;
            AirfoilCoordinate.Coordinate oldValue;

            // Null check
            if (coordinate == null)
            {
                return null;
            }

            oldValue = null;
            for (int i = 0; i < coordinate.Length; i++)
            {
                newValue = coordinate[i];
                if (oldValue == null || !(newValue.X == oldValue.X && newValue.Z == oldValue.Z))
                {
                    var temp = new Double[] { newValue.X, newValue.Z };
                    refined.Add(temp);
                    // Set previous Coordinate
                    oldValue = coordinate[i];
                }
            }

            // Convert List to two dimensional Double Type Array
            var refinedCoordinateArray = new Double[refined.Count, 2];
            for (int i = 0; i < refined.Count; i++)
            {
                refinedCoordinateArray[i, 0] = refined[i][0];
                refinedCoordinateArray[i, 1] = refined[i][1];
            }

            AirfoilCoordinate refinedCoordinate = new AirfoilCoordinate();
            refinedCoordinate.Import(refinedCoordinateArray);
            return refinedCoordinate;
        }

        private AirfoilCoordinate StandardizeAirfoilCoordinates(AirfoilCoordinate coordinate)
        {
            var standardized = new List<double[]>();
            double chordLength = AirfoilCoordinate.GetMaximumValue(coordinate, 0) - AirfoilCoordinate.GetMinimumValue(coordinate, 0);
            AirfoilCoordinate newValue = new AirfoilCoordinate();

            // Null check
            if (coordinate == null)
            {
                return null;
            }

            for (int i = 0; i < coordinate.Length; i++)
            {
                var temp = new double[] { coordinate[i].X / chordLength, coordinate[i].Z / chordLength };
                standardized.Add(temp);
            }

            // Convert List to two dimensional Double Type Array
            var refinedCoordinateArray = new Double[standardized.Count, 2];
            for (int i = 0; i < standardized.Count; i++)
            {
                refinedCoordinateArray[i, 0] = standardized[i][0];
                refinedCoordinateArray[i, 1] = standardized[i][1];
            }

            newValue.Import(refinedCoordinateArray);
            return newValue;
        }
    }
}
