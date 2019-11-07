using System;
using System.Text;

namespace GA_AirfoilOptimizationTool2D.FCharacteristicsManager
{
    class CharacteristicsManagerViewModel : General.ViewModelBase
    {
        #region Fields
        private Airfoil.CombinedAirfoilsGroup sourceAirfoils;
        private Airfoil.Characteristics.AngleBasedCharacteristics liftProfile;
        private int currentAirfoilNumber;

        private TargetAirfoilSelectorViewModel selectedTargetAirfoil;
        #endregion

        #region DelegateCommand
        public General.DelegateCommand LiftCoefProfileSelection { get; private set; }
        public General.DelegateCommand ClickApplyButton { get; set; }
        #endregion

        #region Properties
        public System.Collections.ObjectModel.ObservableCollection<TargetAirfoilSelectorViewModel> TargetAirfoils { get; private set; }
        public TargetAirfoilSelectorViewModel SelectedTargetAirfoil
        {
            get => selectedTargetAirfoil;
            set
            {
                selectedTargetAirfoil = value;
                OnPropertyChanged(nameof(this.SelectedTargetAirfoil));
            }
        }
        #endregion

        public CharacteristicsManagerViewModel()
        {
            // Initialize
            currentAirfoilNumber = 0;
            TargetAirfoils = new System.Collections.ObjectModel.ObservableCollection<TargetAirfoilSelectorViewModel>();
            LiftCoefProfileSelection = new General.DelegateCommand(SelectLiftCoefProfile, () => true);
            ClickApplyButton = new General.DelegateCommand(ApplyButtonClicked, IsApplyButtonAvailable);

            // Assign Events
            this.PropertyChanged += This_PropertyChanged;

            // Clone Offsprng Airfoils
            sourceAirfoils = OptimizingConfiguration.Instance.OffspringAirfoilsCandidates;
            AssignTargetAirfoils();
        }

        /// <summary>
        /// Assign TargetAirfoils Collection source airfoils to Display on a comboBox.
        /// </summary>
        private void AssignTargetAirfoils()
        {
            for (int i = 0; i < sourceAirfoils.CombinedAirfoils.Length; i++)
            {
                TargetAirfoils.Add(new TargetAirfoilSelectorViewModel(sourceAirfoils.CombinedAirfoils[i], "Airfoil" + (i + 1)));
            }
        }

        #region DelegateCommand Callbacks
        public void SelectLiftCoefProfile()
        {
            String pFilePath = General.Messenger.OpenFileMessenger.Show("csv File (*.csv)|*.csv");
            if (pFilePath != null)
            {
                using (var openedCsv = new System.IO.StreamReader(pFilePath, Encoding.UTF8))
                {
                    var profileArray = General.CsvManager.ConvertCsvToArray(openedCsv.ReadToEnd());
                    liftProfile = new Airfoil.Characteristics.AngleBasedCharacteristics(profileArray);
                }

                // Apply lift profiles to temporary airfoils collection
                sourceAirfoils.CombinedAirfoils[currentAirfoilNumber].LiftProfile = liftProfile;
            }
        }

        private void ApplyButtonClicked()
        {
            OptimizingConfiguration.Instance.OffspringAirfoilsCandidates = this.sourceAirfoils;
        }
        private bool IsApplyButtonAvailable()
        {
            if (sourceAirfoils == null)
            {
                return false;
            }
            return (true);
        }
        #endregion

        #region Event Callbacks
        private void This_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.SelectedTargetAirfoil))
            {
                currentAirfoilNumber = TargetAirfoils.IndexOf(selectedTargetAirfoil);
            }
        }
        #endregion
    }
}
