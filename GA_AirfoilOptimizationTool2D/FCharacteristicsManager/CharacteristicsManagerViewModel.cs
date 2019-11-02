using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FCharacteristicsManager
{
    class CharacteristicsManagerViewModel : General.ViewModelBase
    {
        private Airfoil.CombinedAirfoilsGroupManager sourceAirfoils;
        private Airfoil.Characteristics.AngleBasedCharacteristics liftProfile;

        public General.DelegateCommand LiftCoefProfileSelection { get; private set; }

        public CharacteristicsManagerViewModel()
        {
            LiftCoefProfileSelection = new General.DelegateCommand(SelectLiftCoefProfile, () => true);
        }

        public void SelectLiftCoefProfile()
        {
            // Clone Offsprng Airfoils
            sourceAirfoils = OptimizingConfiguration.Instance.OffspringAirfoilsCandidates.Clone();

            String pFilePath = General.Messenger.OpenFileMessenger.Show("csv File (*.csv)|*.csv");
            using (var openedCsv = new System.IO.StreamReader(pFilePath, Encoding.UTF8))
            {
                var profileArray = General.CsvManager.ConvertCsvToArray(openedCsv.ReadToEnd());
                liftProfile = new Airfoil.Characteristics.AngleBasedCharacteristics(profileArray);
            }

            sourceAirfoils.GetCombinedAirfoilsArray()[0].CombinedAirfoil.LiftProfile = liftProfile;
        }
    }
}
