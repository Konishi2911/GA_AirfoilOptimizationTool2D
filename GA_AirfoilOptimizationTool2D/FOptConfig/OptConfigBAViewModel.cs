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

        private Action airfoilSelectionMethod;
        private Func<bool> isSelectable;

        public OptConfigBAViewModel()
        {
            airfoilSelectionMethod = new Action(AirfoilSelectionMethod);
            isSelectable = new Func<bool>(IsAirfoilSelectable);

            airfoilSelection = new OptConfigDelegateCommand(airfoilSelectionMethod, isSelectable);

            NumberOfAirfoil = 1;
            AirfoilSelectionStatus 
                = numberOfLoadedAirfoils.ToString() + " airfoil is loaded." + "  " 
                + (numberOfBAirfoils - numberOfLoadedAirfoils).ToString() + " airfoils left are remaining.";
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
            Models.AirfoilCsvAnalyzer airfoilCsvAnalyzer = new Models.AirfoilCsvAnalyzer();
            String _airfoil_path;

            // Issue the Messenger displaying OpenFileDialog
            _airfoil_path = FOptConfig.Messenger.OpenFileMessenger.Show();

            // Analyze the CSV file located in _airfoil_path
            airfoilCsvAnalyzer.Analyze(_airfoil_path);
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
