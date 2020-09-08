using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FCharacteristicsManager
{
    class CharacteristicsSelectorViewModel
    {
        public CharacteristicsSources Source { get; private set; }
        public string Label { get; private set; }

        public CharacteristicsSelectorViewModel(CharacteristicsSources previewMode, string label)
        {
            this.Source = previewMode;
            this.Label = label;
        }

        /// <summary>
        /// The Map of the value of AirfoilRepresentationMode type and the label for display.
        /// </summary>
        private static Dictionary<CharacteristicsSources, string> previewModeMap = new Dictionary<CharacteristicsSources, string>
        {
            { CharacteristicsSources.Lift, "Lift Coefficient" },
            { CharacteristicsSources.Drag, "Drag Coefficient" },
            { CharacteristicsSources.LiftDrag, "Lift-Drag Ratio" }
        };

        public static CharacteristicsSelectorViewModel Create(CharacteristicsSources source)
        {
            return new CharacteristicsSelectorViewModel(source, previewModeMap[source]);
        }

        public static List<CharacteristicsSelectorViewModel> Create()
        {
            List<CharacteristicsSelectorViewModel> list = new List<CharacteristicsSelectorViewModel>();
            foreach (CharacteristicsSources e in System.Enum.GetValues(typeof(CharacteristicsSources)))
            {
                list.Add(Create(e));
            }

            return list;
        }
    }
}
