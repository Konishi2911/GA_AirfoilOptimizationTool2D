using System;

namespace GA_AirfoilOptimizationTool2D.FOptConfig
{
    class OptConfigBAViewModel : General.ViewModelBase
    {
        private Double numberOfBAirfoils;
        private Double numberOfLoadedAirfoils;
        private AirfoilSelectorViewModel selectedAirfoil;
        private OptConfigDelegateCommand airfoilSelection;
        private String airfoilSelectionStatus;
        private Models.ImportedAirfoilGroupManager ImportedAirfoil;

        private Action airfoilSelectionMethod;
        private Func<bool> isSelectable;

        private void UpdateStatusMessage()
        {
            AirfoilSelectionStatus
                = numberOfLoadedAirfoils.ToString() + " airfoil is loaded." + "  "
                + (numberOfBAirfoils - numberOfLoadedAirfoils).ToString() + " airfoils left are required.";
        }

        #region Events
        private void assignEventHandler()
        {
            ImportedAirfoil.PropertyChanged += ImportedAirfoil_PropertyChanged;
        }

        private void ImportedAirfoil_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Number of loaded Airfoil Changed
            if (e.PropertyName == nameof(ImportedAirfoil.NumberOfAirfoils))
            {
                // Update numberOfAirfoils.
                numberOfLoadedAirfoils = ImportedAirfoil.NumberOfAirfoils;

                // Update status message
                UpdateStatusMessage();
            }
        }
        #endregion

        public OptConfigBAViewModel()
        {
            #region Instantiate
            // ------------------------------------------------------------
            airfoilSelectionMethod = new Action(AirfoilSelectionMethod);
            isSelectable = new Func<bool>(IsAirfoilSelectable);

            airfoilSelection = new OptConfigDelegateCommand(airfoilSelectionMethod, isSelectable);
            ImportedAirfoil = new Models.ImportedAirfoilGroupManager();
            // ------------------------------------------------------------
            #endregion

            #region Initialize Fields
            // Substitute Initial Value.
            NumberOfAirfoil = 1;
            AirfoilSelectionStatus
                = numberOfLoadedAirfoils.ToString() + " airfoil is loaded." + "  "
                + (numberOfBAirfoils - numberOfLoadedAirfoils).ToString() + " airfoils left are required.";
            #endregion

            // Assign EventHandler
            assignEventHandler();
        }

        public Double NumberOfAirfoil
        {
            get
            {
                return numberOfBAirfoils;
            }
            set
            {
                this.numberOfBAirfoils = value;

                UpdateStatusMessage();
                OnPropertyChanged("NumberOfBAirfoil");
            }
        }

        public OptConfigDelegateCommand AirfoilSelection
        {
            get
            {
                return airfoilSelection;
            }
        }

        public String AirfoilSelectionStatus
        {
            get
            {
                return airfoilSelectionStatus;
            }
            set
            {
                airfoilSelectionStatus = value;
                OnPropertyChanged("AirfoilSelectionStatus");
            }
        }

        private void AirfoilSelectionMethod()
        {
            Microsoft.Win32.OpenFileDialog _ofd = new Microsoft.Win32.OpenFileDialog();
            Models.AirfoilCsvAnalyzer airfoilCsvAnalyzer = Models.AirfoilCsvAnalyzer.GetInstance();
            String _airfoil_path;

            // Issue the Messenger displaying OpenFileDialog
            _airfoil_path = FOptConfig.Messenger.OpenFileMessenger.Show();

            // Analyze the CSV file located in _airfoil_path
            var result = airfoilCsvAnalyzer.Analyze(_airfoil_path);

            // Registrate imported Airfoil to the AirfoilGroupManager.
            ImportedAirfoil.Add(result);
        }

        private Boolean IsAirfoilSelectable()
        {
            if (numberOfLoadedAirfoils == numberOfBAirfoils)
            {
                return false;
            }
            else if (numberOfLoadedAirfoils < numberOfBAirfoils)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
