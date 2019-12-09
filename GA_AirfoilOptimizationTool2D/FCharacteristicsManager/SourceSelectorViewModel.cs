using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FCharacteristicsManager
{
    class SourceSelectorViewModel
    {
        public PopulationSources Source { get; private set; }
        public string Label { get; private set; }

        public SourceSelectorViewModel(PopulationSources previewMode, string label)
        {
            this.Source = previewMode;
            this.Label = label;
        }

        /// <summary>
        /// The Map of the value of AirfoilRepresentationMode type and the label for display.
        /// </summary>
        private static Dictionary<PopulationSources, string> previewModeMap = new Dictionary<PopulationSources, string>
        {
            { PopulationSources.CurrentPopulation, "Current Airfoil Populations" },
            { PopulationSources.OffspringCandidates, "Candidates of Offspring Airfoils" }
        };

        public static SourceSelectorViewModel Create(PopulationSources mode)
        {
            return new SourceSelectorViewModel(mode, previewModeMap[mode]);
        }

        public static List<SourceSelectorViewModel> Create()
        {
            List<SourceSelectorViewModel> list = new List<SourceSelectorViewModel>();
            foreach (PopulationSources e in System.Enum.GetValues(typeof(PopulationSources)))
            {
                list.Add(Create(e));
            }

            return list;
        }
    }
}
