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

        public Double ChordLength { get { return chordLength; } }
        public Double MaximumThickness { get { return maximumThickness; } }
        public Double MaximumCamber { get { return maximumCamber; } }
        public Double LeadingEdgeRadius { get { return leadingEdgeRadius; } }
        #endregion

        private void InitializeComponent()
        {
            InterpolatedCoordinate = new AirfoilCoordinate();
        }
        public AirfoilManager()
        {
            InitializeComponent();
        }

        public AirfoilManager(AirfoilCoordinate coordinate)
        {
            InitializeComponent();

            _importedCoordinate = coordinate;
            airfoilInterpolation();
        }

        /// <summary>
        /// Interpolate airfoil coordinate with three dimensional Spline.
        /// </summary>
        private void airfoilInterpolation()
        {
            var Result = General.Interpolation.SplineInterpolation(_importedCoordinate.ToDouleArray(), NumberOfControlPoint);
            InterpolatedCoordinate.Import(Result);
        }
    }
}
