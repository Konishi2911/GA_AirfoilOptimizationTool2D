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
                airfoilSelectionStatus = "";
                OnPropertyChanged("AirfoilSelectionStatus");
            }
        }

        private void AirfoilSelectionMethod()
        {
            Microsoft.Win32.OpenFileDialog _ofd = new Microsoft.Win32.OpenFileDialog();
            String _airfoil_path;

            //Show OpenFileDialog
            var ofdResults = _ofd.ShowDialog();


            if (ofdResults == true)
            {
                //Get airfoil data path
                _airfoil_path = _ofd.FileName;
            }
            else if (ofdResults == false)
            {
                //OpenFileDialog was canceled
            }
            else
            {
                //If OpenFileDialog is null
            }
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
