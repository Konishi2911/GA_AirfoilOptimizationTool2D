using System;

namespace GA_AirfoilOptimizationTool2D.FOptConfig
{
    class OptConfigBAViewModel : General.ViewModelBase
    {
        private Double numberOfBAirfoils;
        private AirfoilSelectorViewModel selectedAirfoil;
        private OptConfigDelegateCommand airfoilSelection;
        private String airfoilSelectionStatus;

        private Action airfoilSelectionMethod;
        private Func<bool> IsSelectable;

        public OptConfigBAViewModel()
        {
            //Action airfoilSelectionMethod;
            //Func<bool> IsSelectable;

            airfoilSelection = new OptConfigDelegateCommand(airfoilSelectionMethod, IsSelectable);
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
            Microsoft.Win32.OpenFileDialog _ofd = new System.Windows.Forms.OpenFileDialog();
            String _airfoil_path;


        }
    }
}
