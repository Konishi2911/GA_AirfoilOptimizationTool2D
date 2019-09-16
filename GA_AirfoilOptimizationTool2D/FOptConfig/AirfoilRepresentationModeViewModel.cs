using System.Collections.Generic;

namespace GA_AirfoilOptimizationTool2D.FOptConfig
{
    public class AirfoilRepresentationModeViewModel : General.ViewModelBase
    {
        public AirfoilRepresentationMode AirfoilRepresentationMode { get; private set; }
        public string Label { get; private set; }

        public AirfoilRepresentationModeViewModel(AirfoilRepresentationMode representationMode, string label)
        {
            this.AirfoilRepresentationMode = representationMode;
            this.Label = label;
        }

        /// <summary>
        /// The Map of the value of AirfoilRepresentationMode type and the label for display.
        /// </summary>
        private static Dictionary<AirfoilRepresentationMode, string> representationModeMap = new Dictionary<AirfoilRepresentationMode, string>
        {
            { AirfoilRepresentationMode.BasisAirfoilMode, "Basis Airfoil Mode" },
            { AirfoilRepresentationMode.BasisEquationMode, "Basis Equation Mode" },
            { AirfoilRepresentationMode.SplineMode, "Spline Mode" }
        };

        public static AirfoilRepresentationModeViewModel Create(AirfoilRepresentationMode mode)
        {
            return new AirfoilRepresentationModeViewModel(mode, representationModeMap[mode]);
        }

        public static IEnumerable<AirfoilRepresentationModeViewModel> Create()
        {
            foreach (AirfoilRepresentationMode e in System.Enum.GetValues(typeof(AirfoilRepresentationMode)))
            {
                yield return Create(e);
            }
        }
    }
}
