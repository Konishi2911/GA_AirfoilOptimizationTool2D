using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig
{
    public class OptimizingParameterTypeViewModel : General.ViewModelBase
    {
        public OptimizingParameterType OptimizingParameterType { get; private set; }
        public string Label { get; private set; }

        public OptimizingParameterTypeViewModel(OptimizingParameterType parameterMode, string label)
        {
            this.OptimizingParameterType = parameterMode;
            this.Label = label;
        }

        /// <summary>
        /// The Map of the value of OptimizingParameterType type and the label for display.
        /// </summary>
        private static Dictionary<OptimizingParameterType, string> parameterType = new Dictionary<OptimizingParameterType, string>
        {
            { OptimizingParameterType.LiftCoefficient, "Lift Coefficient" },
            { OptimizingParameterType.DragCoefficient, "Drag Coefficient" },
            { OptimizingParameterType.MomentCoefficient, "Moment Coefficient" }
        };

        public static OptimizingParameterTypeViewModel Create(OptimizingParameterType type)
        {
            return new OptimizingParameterTypeViewModel(type, parameterType[type]);
        }

        public static IEnumerable<OptimizingParameterTypeViewModel> Create()
        {
            foreach (OptimizingParameterType e in System.Enum.GetValues(typeof(OptimizingParameterType)))
            {
                yield return Create(e);
            }
        }
    }
}
