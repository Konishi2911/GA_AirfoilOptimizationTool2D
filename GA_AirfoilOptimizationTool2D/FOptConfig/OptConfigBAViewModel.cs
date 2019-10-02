using System;
using System.Data;

namespace GA_AirfoilOptimizationTool2D.FOptConfig
{
    class OptConfigBAViewModel : General.ViewModelBase
    {
        // Fields =====================================================
        private Double numberOfBAirfoils;
        private Double numberOfLoadedAirfoils;
        private AirfoilSelectorViewModel selectedAirfoil;
        private OptConfigDelegateCommand airfoilSelection;
        private String airfoilSelectionStatus;
        private DataTable airfoilSpecifications;
        private System.Collections.ObjectModel.ObservableCollection<System.Windows.Point> coordinateList;

        /// <summary>
        /// Storing and Managing imported Airfoil.
        /// </summary>
        private Models.ImportedAirfoilGroupManager ImportedAirfoil;
        // ============================================================

        // Command Actions ========================
        private Action airfoilSelectionMethod;
        private Func<bool> isSelectable;
        // ========================================

        #region Binding Data
        // Binding Data of Airfoil Selection ComboBox =================================================================================
        public System.Collections.ObjectModel.ObservableCollection<AirfoilSelectorViewModel> LoadedAirfoils { get; private set; }
        public AirfoilSelectorViewModel SelectedAirfoil
        {
            get
            {
                return selectedAirfoil;
            }
            set
            {
                selectedAirfoil = value;
                OnPropertyChanged(nameof(SelectedAirfoil));

                // Update Specification DataGrid
                AirfoilSpecifications = CreateTable(SelectedAirfoil.SelectedAirfoil);
            }
        }

        // Binding Data of Airfoil Specification DataGrid ============================================================================================
        public System.Data.DataTable AirfoilSpecifications
        {
            get
            {
                return airfoilSpecifications;
            }
            private set
            {
                airfoilSpecifications = value;
                OnPropertyChanged(nameof(AirfoilSpecifications));
            }
        }

        // Binding Data of the Coordinate Point List for Airfoil Preview =========================================
        public System.Collections.ObjectModel.ObservableCollection<System.Windows.Point> CoordinateList
        {
            get
            {
                return coordinateList;
            }
            set
            {
                coordinateList = value;
                OnPropertyChanged(nameof(CoordinateList));
            }
        }
        public Double PreviewWindowWidth { get; set; }
        public Double PreviewWindowHeight { get; set; }
        #endregion

        #region Scripts
        private void UpdateStatusMessage()
        {
            AirfoilSelectionStatus
                = numberOfLoadedAirfoils.ToString() + " airfoil is loaded." + "  "
                + (numberOfBAirfoils - numberOfLoadedAirfoils).ToString() + " airfoils left are required.";
        }

        private void assignEventHandler()
        {
            ImportedAirfoil.PropertyChanged += ImportedAirfoil_PropertyChanged;
            ImportedAirfoil.AirfoilAdded += ImportedAirfoil_AirfoilAdded;
            ImportedAirfoil.AirfoilRemoved += ImportedAirfoil_AirfoilRemoved;
        }

        private DataTable CreateTable(Airfoil.AirfoilManager airfoil)
        {
            var specifications = new System.Data.DataTable();

            specifications.Columns.Add();
            specifications.Columns.Add();

            specifications.Rows.Add("Airfoil Name", airfoil.AirfoilName);
            specifications.Rows.Add("Chord Length", airfoil.ChordLength);
            specifications.Rows.Add("Max Thickness", airfoil.MaximumThickness);
            specifications.Rows.Add("Max Camber", airfoil.MaximumCamber);
            specifications.Rows.Add("L.E. Radius", airfoil.LeadingEdgeRadius);

            return specifications;
        }
        #endregion

        #region Events
        private void ImportedAirfoil_AirfoilRemoved(object sender, Airfoil.AirfoilGroupManagerBase.AirfoilRemovedEventArgs e)
        {
            // Remove Airfoil
            LoadedAirfoils.Remove(new AirfoilSelectorViewModel(e.RemovedAirfoil, e.Lable));
        }

        private void ImportedAirfoil_AirfoilAdded(object sender, Airfoil.AirfoilGroupManagerBase.AirfoilAddedEventArgs e)
        {
            // Get Airfoil Name
            String label;
            if (e.AddedAirfoil.AirfoilName != null)
            {
                label = e.AddedAirfoil.AirfoilName;
            }
            else
            {
                label = "Airfoil" + (numberOfLoadedAirfoils).ToString();
            }

            // If LoadedAirfoil is null, Instantiate LoadedAirfoils
            if (LoadedAirfoils == null)
            {
                LoadedAirfoils = new System.Collections.ObjectModel.ObservableCollection<AirfoilSelectorViewModel>();
            }
            // Add new Airfoil to Airfoil List
            LoadedAirfoils.Add(new AirfoilSelectorViewModel(e.AddedAirfoil, label));

            // Set selected airfoil
            if (LoadedAirfoils.Count != 0)
            {
                SelectedAirfoil = LoadedAirfoils[0];
            }


        }

        private void ImportedAirfoil_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var instance = sender as Models.ImportedAirfoilGroupManager;

            // Number of loaded Airfoil Changed
            if (e.PropertyName == nameof(instance.NumberOfAirfoils))
            {
                // Update numberOfAirfoils.
                numberOfLoadedAirfoils = instance.NumberOfAirfoils;

                // Update status message
                UpdateStatusMessage();
            }
        }
        #endregion

        #region DelegateCommand Actions
        // DelegateCommand Action ==========================================================================
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

            CoordinateList = GetPreviewPointList(SelectedAirfoil.SelectedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
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
        // ===============================================================================================
        #endregion

        public OptConfigBAViewModel()
        {
            #region Instantiate
            // ------------------------------------------------------------

            // SelectBaseAirfoil Button related ============================
            airfoilSelectionMethod = new Action(AirfoilSelectionMethod);
            isSelectable = new Func<bool>(IsAirfoilSelectable);
            airfoilSelection = new OptConfigDelegateCommand(airfoilSelectionMethod, isSelectable);
            //

            // AirfoilSelection ComboBox related ====================================================================
            LoadedAirfoils = new System.Collections.ObjectModel.ObservableCollection<AirfoilSelectorViewModel>();
            //

            // Airfoil Specification DataGrid related =============================================================================
            //

            // Airfoil Preview related ===============================================
            coordinateList = new System.Collections.ObjectModel.ObservableCollection<System.Windows.Point>();
            //

            // Models ====================================================
            ImportedAirfoil = new Models.ImportedAirfoilGroupManager();
            //

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="airfoil">Previewing Airfoil</param>
        /// <param name="height">Preview Window Height</param>
        /// <param name="width">Preview Window Width</param>
        /// <returns></returns>
        private System.Collections.ObjectModel.ObservableCollection<System.Windows.Point> GetPreviewPointList(Airfoil.AirfoilManager airfoil, double height, double width)
        {
            System.Collections.ObjectModel.ObservableCollection<System.Windows.Point> pointList = new System.Collections.ObjectModel.ObservableCollection<System.Windows.Point>();
            var airfoilCoordinates = airfoil.InterpolatedCoordinate;

            double airfoilLength = Airfoil.AirfoilCoordinate.GetMaximumValue(airfoilCoordinates, 0) - Airfoil.AirfoilCoordinate.GetMinimumValue(airfoilCoordinates, 0);
            double airfoilHeight = Airfoil.AirfoilCoordinate.GetMaximumValue(airfoilCoordinates, 1) - Airfoil.AirfoilCoordinate.GetMinimumValue(airfoilCoordinates, 1);

            double magnification = 0.8 * (double)width / (double)airfoilLength;

            // Adjustment variable
            double adjustX = (width - airfoilLength * magnification) / 2 - Airfoil.AirfoilCoordinate.GetMinimumValue(airfoilCoordinates, 0);
            double adjustZ = (height - airfoilHeight * magnification) / 2 + Airfoil.AirfoilCoordinate.GetMaximumValue(airfoilCoordinates, 1) * magnification;

            // Adjust Airfoil Coordinates to Fit Preview Window
            var adjustedCoordinate = Airfoil.AirfoilCoordinate.Scaling(airfoilCoordinates, magnification);
            var temp = new Double[adjustedCoordinate.Length, 2];
            for (int i = 0; i < adjustedCoordinate.Length; i++)
            {
                temp[i, 0] = adjustX + adjustedCoordinate[i].X;
                temp[i, 1] = adjustZ - adjustedCoordinate[i].Z;
            }
            adjustedCoordinate.Import(temp);

            // Convert adjustCoordinate to ObservableCollection 
            for (int i = 0; i < adjustedCoordinate.Length; i++)
            {
                pointList.Add(new System.Windows.Point() { X = adjustedCoordinate[i].X, Y = adjustedCoordinate[i].Z });
            }

            return pointList;
        }
    }
}
