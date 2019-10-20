using System;
using System.Data;

namespace GA_AirfoilOptimizationTool2D.FOptConfig
{
    class OptConfigBAViewModel : General.ViewModelBase
    {
        // Fields =====================================================
        private int numberOfBAirfoils;
        private int numberOfLoadedAirfoils;
        private AirfoilSelectorViewModel selectedAirfoil;
        private OptConfigDelegateCommand airfoilSelection;
        private OptConfigDelegateCommand updatePreviewWindows;
        private String airfoilSelectionStatus;
        private DataTable airfoilSpecifications;
        private System.Collections.ObjectModel.ObservableCollection<System.Windows.Point> coordinateList;

        /// <summary>
        /// Storing and Managing imported Airfoils. (This class is Singleton.)
        /// </summary>
        private Models.ImportedAirfoilGroupManager ImportedAirfoil;
        // ============================================================

        // Command Actions ========================
        private Action airfoilSelectionMethod;
        private Func<bool> isSelectable;
        // ========================================

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

                // Update Preview
                UpdatePreview();
            }
        }

        #region Binding Data
        // Binding Data of Airfoil Selection ComboBox =================================================================================
        public System.Collections.ObjectModel.ObservableCollection<AirfoilSelectorViewModel> LoadedAirfoils { get; private set; }

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
            this.PropertyChanged += This_PropertyChanged;
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

        private void This_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.NumberOfBasisAirfoils))
            {
                ImportedAirfoil.NumberOfBasisAirfoils = NumberOfBasisAirfoils;

                UpdateStatusMessage();
            }
            else if (e.PropertyName == nameof(this.NumberOfLoadedAirfoils))
            {
                ImportedAirfoil.NumberOfAirfoils = NumberOfLoadedAirfoils;
                
                UpdateStatusMessage();
            }
        }
        #endregion

        #region DelegateCommand Actions
        // DelegateCommand Action ==========================================================================
        private void AirfoilSelectionMethod()
        {
            Models.AirfoilCsvAnalyzer airfoilCsvAnalyzer = Models.AirfoilCsvAnalyzer.GetInstance();
            String _airfoil_path;

            // Issue the Messenger displaying OpenFileDialog
            _airfoil_path = General.Messenger.OpenFileMessenger.Show("CSV File (*.csv)|*.csv");

            if (_airfoil_path == null)
            {
                return;
            }

            // Analyze the CSV file located in _airfoil_path
            var result = airfoilCsvAnalyzer.Analyze(_airfoil_path);

            // Registrate imported Airfoil to the AirfoilGroupManager.
            ImportedAirfoil.Add(result);
        }
        private void UpdatePreview()
        {
            if (SelectedAirfoil != null && SelectedAirfoil.SelectedAirfoil != null)
            {
                // Update Preview
                CoordinateList = General.AirfoilPreview.GetPreviewPointList(SelectedAirfoil.SelectedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
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
            updatePreviewWindows = new OptConfigDelegateCommand(UpdatePreview, () => true);
            //

            // Read Loaded Airfoil
            if (OptimizingConfiguration.Instance.BasisAirfoils == null)
            {
                ImportedAirfoil = Models.ImportedAirfoilGroupManager.GetNewInstance();
            }
            else
            {
                ImportedAirfoil = Models.ImportedAirfoilGroupManager.GetNewInstance();
                ImportedAirfoil.AirfoilGroup = new System.Collections.Generic.List<Airfoil.AirfoilManager>(OptimizingConfiguration.Instance.BasisAirfoils.AirfoilGroup);
                ImportedAirfoil.NumberOfAirfoils = OptimizingConfiguration.Instance.BasisAirfoils.NumberOfAirfoils;
                ImportedAirfoil.NumberOfBasisAirfoils = OptimizingConfiguration.Instance.BasisAirfoils.NumberOfBasisAirfoils;
            }
            #endregion

            // Assign EventHandler
            assignEventHandler();

            #region Initialize Fields
            if (ImportedAirfoil.NumberOfAirfoils == 0)
            {
                NumberOfBasisAirfoils = 1;

                // AirfoilSelection ComboBox related ====================================================================
                LoadedAirfoils = new System.Collections.ObjectModel.ObservableCollection<AirfoilSelectorViewModel>();
                //

                // Airfoil Specification DataGrid related =============================================================================
                //

                // Airfoil Preview related ===============================================
                coordinateList = new System.Collections.ObjectModel.ObservableCollection<System.Windows.Point>();
                //

                // Models ====================================================
                //
            }
            else
            {
                NumberOfLoadedAirfoils = ImportedAirfoil.NumberOfAirfoils;
                NumberOfBasisAirfoils = ImportedAirfoil.NumberOfBasisAirfoils;

                // AirfoilSelection ComboBox related ====================================================================
                LoadedAirfoils = new System.Collections.ObjectModel.ObservableCollection<AirfoilSelectorViewModel>();
                for (int i = 0; i < ImportedAirfoil.NumberOfAirfoils; i++)
                {
                    // Get Airfoil Name
                    String label;
                    if (ImportedAirfoil.AirfoilGroup[i].AirfoilName != null)
                    {
                        label = ImportedAirfoil.AirfoilGroup[i].AirfoilName;
                    }
                    else
                    {
                        label = "Airfoil" + (NumberOfLoadedAirfoils).ToString();
                    }

                    LoadedAirfoils.Add(new AirfoilSelectorViewModel(ImportedAirfoil.AirfoilGroup[i], label));
                }
                SelectedAirfoil = LoadedAirfoils[0];
                //

                // Airfoil Preview related ===============================================
                CoordinateList = General.AirfoilPreview.GetPreviewPointList(SelectedAirfoil.SelectedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
                //
            }

            // ------------------------------------------------------------

            // Substitute Initial Value.
            AirfoilSelectionStatus
                = numberOfLoadedAirfoils.ToString() + " airfoil is loaded." + "  "
                + (numberOfBAirfoils - numberOfLoadedAirfoils).ToString() + " airfoils left are required.";
            #endregion
        }

        /// <summary>
        /// Number of Basis Airfoils
        /// </summary>
        public int NumberOfBasisAirfoils
        {
            get
            {
                return numberOfBAirfoils;
            }
            set
            {
                this.numberOfBAirfoils = value;
                OnPropertyChanged(nameof(NumberOfBasisAirfoils));
            }
        }
        /// <summary>
        /// Number of Loaded Airfoils
        /// </summary>
        public int NumberOfLoadedAirfoils
        {
            get
            {
                return numberOfLoadedAirfoils;
            }
            set
            {
                this.numberOfLoadedAirfoils = value;
                OnPropertyChanged(nameof(this.NumberOfLoadedAirfoils));
            }
        }

        public OptConfigDelegateCommand AirfoilSelection
        {
            get
            {
                return airfoilSelection;
            }
        }
        public OptConfigDelegateCommand UpdatePreviewWindows
        {
            get => updatePreviewWindows;
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
    }
}
