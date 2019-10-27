using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FInputAirfoilCharacteristics.Messenger
{
    public class CharacteristicsImporterMessenger
    {
        public event EventHandler ShowCharacteristicsImporter;

        public static String EventName
        {
            get => nameof(ShowCharacteristicsImporter);
        }

        /// <summary>
        /// This Class is singleton.
        /// </summary>
        public static CharacteristicsImporterMessenger Instance { get; private set; } = new CharacteristicsImporterMessenger();
        private CharacteristicsImporterMessenger()
        {

        }

        public class CoefficientManagerEventArgs : EventArgs
        {
        }

        public static void Show()
        {
            Instance.ShowCharacteristicsImporter?.Invoke(Instance, new CoefficientManagerEventArgs());
        }
    }
}
