using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FMainWindow
{
    public class PreviewWindowSelecterViewModel : General.ViewModelBase
    {
        public PreviewWindowMode PreviewMode { get; private set; }
        public string Label { get; private set; }

        public PreviewWindowSelecterViewModel(PreviewWindowMode previewMode, string label)
        {
            this.PreviewMode = previewMode;
            this.Label = label;
        }

        /// <summary>
        /// The Map of the value of AirfoilRepresentationMode type and the label for display.
        /// </summary>
        private static Dictionary<PreviewWindowMode, string> previewModeMap = new Dictionary<PreviewWindowMode, string>
        {
            { PreviewWindowMode.CurrentPopulation, "Current Airfoil Populations" },
            { PreviewWindowMode.OffspringCandidates, "Candidates of Offspring Airfoils" }
        };

        public static PreviewWindowSelecterViewModel Create(PreviewWindowMode mode)
        {
            return new PreviewWindowSelecterViewModel(mode, previewModeMap[mode]);
        }

        public static List<PreviewWindowSelecterViewModel> Create()
        {
            List<PreviewWindowSelecterViewModel> list = new List<PreviewWindowSelecterViewModel>();
            foreach (PreviewWindowMode e in System.Enum.GetValues(typeof(PreviewWindowMode)))
            {
                list.Add(Create(e));
            }

            return list;
        }
    }
}
