﻿using System;
using System.Text;

namespace GA_AirfoilOptimizationTool2D.FCharacteristicsManager
{
    class CharacteristicsManagerViewModel : General.ViewModelBase
    {
        #region Fields
        private int nInterpolatedPoints;
        private Airfoil.CombinedAirfoilsGroup sourceAirfoils;
        private Airfoil.Characteristics.AngleBasedCharacteristics liftProfile;
        private Airfoil.Characteristics.AngleBasedCharacteristics dragProfile;
        private Airfoil.Characteristics.AngleBasedCharacteristics momentProfile;
        private Airfoil.Characteristics.AngleBasedCharacteristics ldProfile;
        private int currentAirfoilNumber;

        private TargetAirfoilSelectorViewModel selectedTargetAirfoil;
        #endregion

        #region DelegateCommand
        public General.DelegateCommand LiftCoefProfileSelection { get; private set; }
        public General.DelegateCommand DragCoefProfileSelection { get; private set; }
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
            nInterpolatedPoints = 100;

            currentAirfoilNumber = 0;
            TargetAirfoils = new System.Collections.ObjectModel.ObservableCollection<TargetAirfoilSelectorViewModel>();
            LiftCoefProfileSelection = new General.DelegateCommand(SelectLiftCoefProfile, () => true);
            DragCoefProfileSelection = new General.DelegateCommand(SelectDragCoefProfile, () => true);
            ClickApplyButton = new General.DelegateCommand(ApplyButtonClicked, IsApplyButtonAvailable);

            // Assign Events
            this.PropertyChanged += This_PropertyChanged;

            // Clone Offsprng Airfoils
            sourceAirfoils = AirfoilOptimizationResource.Instance.OffspringCandidates;
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
        private void CalculateLDRatio()
        {
            if (liftProfile != null && dragProfile != null
                && liftProfile.InterpolatedCharacteristics[0, 0] == dragProfile.InterpolatedCharacteristics[0, 0]
                && liftProfile.InterpolatedCharacteristics[nInterpolatedPoints - 1, 0] == dragProfile.InterpolatedCharacteristics[nInterpolatedPoints - 1, 0]
                )
            {
                var profileArray = new double[nInterpolatedPoints, 2];
                for (int i = 0; i < nInterpolatedPoints; i++)
                {
                    profileArray[i, 0] = liftProfile.InterpolatedCharacteristics[i, 0];
                    profileArray[i, 1] = liftProfile.InterpolatedCharacteristics[i, 1] / dragProfile.InterpolatedCharacteristics[i, 1];
                }
                ldProfile = new Airfoil.Characteristics.AngleBasedCharacteristics(profileArray, nInterpolatedPoints);
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
                    liftProfile.NoInterpolatedPoints = nInterpolatedPoints;
                }

                CalculateLDRatio();

                // Apply lift profiles to temporary airfoils collection
                sourceAirfoils.CombinedAirfoils[currentAirfoilNumber].LiftProfile = liftProfile;
            }
        }

        public void SelectDragCoefProfile()
        {
            String pFilePath = General.Messenger.OpenFileMessenger.Show("csv File (*.csv)|*.csv");
            if (pFilePath != null)
            {
                using (var openedCsv = new System.IO.StreamReader(pFilePath, Encoding.UTF8))
                {
                    var profileArray = General.CsvManager.ConvertCsvToArray(openedCsv.ReadToEnd());
                    dragProfile = new Airfoil.Characteristics.AngleBasedCharacteristics(profileArray);
                    dragProfile.NoInterpolatedPoints = nInterpolatedPoints;
                }

                CalculateLDRatio();

                // Apply lift profiles to temporary airfoils collection
                sourceAirfoils.CombinedAirfoils[currentAirfoilNumber].DragProfile = dragProfile;
            }
        }

        private void ApplyButtonClicked()
        {
            AirfoilOptimizationResource.Instance.OffspringCandidates = this.sourceAirfoils;
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
