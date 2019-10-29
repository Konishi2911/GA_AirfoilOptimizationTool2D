using System;
using System.Collections.ObjectModel;

namespace GA_AirfoilOptimizationTool2D.FMainWindow
{
    public class MainWindowViewModel : General.ViewModelBase
    {
        private const int NumberOfChildren = 10;

        private General.BasisAirfoils basisAirfoils;
        private General.DelegateCommand openWorkingFile;
        private General.DelegateCommand saveWorkingFile;
        private General.DelegateCommand showOprConfigDialog;
        private General.DelegateCommand showCoefficientManager;
        private General.DelegateCommand updatePreviewWindow;
        private General.ParamDelegateCommand<String> setSpecifications;
        private Airfoil.Representation.AirfoilCombiner[] combinedAirfoils;
        private Double[,] coefficients;
        private ObservableCollection<System.Windows.Point>[] previewCoordinates;
        private System.Data.DataTable airfoilSpecifications;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            // Allocate Event CallBack Functions
            OptimizingConfiguration.Instance.SourceDataChanged += SourceChanged;
            //

            openOptConfigDialog = new Action(OpenOptimizingConfigurationDialog);
            isOptConfigEnabled = new Func<bool>(IsOptConfigDialogEnabled);

            // Assign the delegate Command
            openWorkingFile = new General.DelegateCommand(OpenWorkingFile, () => true);
            saveWorkingFile = new General.DelegateCommand(SaveWorkingFile, () => true);
            showOprConfigDialog = new General.DelegateCommand(openOptConfigDialog, isOptConfigEnabled);
            showCoefficientManager = new General.DelegateCommand(OpenCoefficientManager, IsCoefManagerEnabled);
            updatePreviewWindow = new General.DelegateCommand(UpdateAirfoilPreviews, () => true);
            setSpecifications = new General.ParamDelegateCommand<String>(DisplaySpecifications, () => true);
            
            //

            // Instantiate Fields
            combinedAirfoils = new Airfoil.Representation.AirfoilCombiner[NumberOfChildren];
            for (int i = 0; i < NumberOfChildren; i++)
            {
                combinedAirfoils[i] = new Airfoil.Representation.AirfoilCombiner();
            }

            previewCoordinates = new ObservableCollection<System.Windows.Point>[NumberOfChildren];
            for (int i = 0; i < NumberOfChildren; i++)
            {
                previewCoordinates[i] = new ObservableCollection<System.Windows.Point>();
            }
            //
        }

        public Action openOptConfigDialog;
        public Func<bool> isOptConfigEnabled;

        #region Event CallBacks
        private void SourceChanged(object sender, EventArgs e)
        {
            // Null Check
            if (OptimizingConfiguration.Instance.BasisAirfoils == null) return;
            if (OptimizingConfiguration.Instance.CoefficientOfCombination == null) return;

            // Update baseAirfoil
            this.basisAirfoils = General.BasisAirfoils.Convert(OptimizingConfiguration.Instance.BasisAirfoils);

            // Update coefficients
            this.coefficients = OptimizingConfiguration.Instance.CoefficientOfCombination.Clone() as Double[,];

            // Re-combinate Airfoil
            this.combinedAirfoils = OptimizingConfiguration.Instance.CurrentAirfoilsPopulation.GetCombinedAirfoilsArray();

            // Re-generate the coordinates for airfoil previewing.
            UpdateAirfoilPreviews();
        }

        private void WorkingFileImported(object sender, FWorkingFileIO.WorkingFileIO.OpeningFileFinishedEventArgs e)
        {
            General.BasisAirfoils baseAirfoilsGroup = new General.BasisAirfoils();

            foreach (var item in e.BaseAirfoils)
            {
                baseAirfoilsGroup.Add(item);
            }

            OptimizingConfiguration.Instance.SetSource(baseAirfoilsGroup, e.CoefficientOfCombination);
        }
        #endregion

        #region Binding Properties
        public ObservableCollection<System.Windows.Point> PreviewCoordinate1
        {
            get
            {
                return previewCoordinates[0];
            }
            private set
            {
                previewCoordinates[0] = value;
                OnPropertyChanged(nameof(PreviewCoordinate1));
            }
        }
        public ObservableCollection<System.Windows.Point> PreviewCoordinate2
        {
            get
            {
                return previewCoordinates[1];
            }
            private set
            {
                previewCoordinates[1] = value;
                OnPropertyChanged(nameof(PreviewCoordinate2));
            }
        }
        public ObservableCollection<System.Windows.Point> PreviewCoordinate3
        {
            get
            {
                return previewCoordinates[2];
            }
            private set
            {
                previewCoordinates[2] = value;
                OnPropertyChanged(nameof(PreviewCoordinate3));
            }
        }
        public ObservableCollection<System.Windows.Point> PreviewCoordinate4
        {
            get
            {
                return previewCoordinates[3];
            }
            private set
            {
                previewCoordinates[3] = value;
                OnPropertyChanged(nameof(PreviewCoordinate4));
            }
        }
        public ObservableCollection<System.Windows.Point> PreviewCoordinate5
        {
            get
            {
                return previewCoordinates[4];
            }
            private set
            {
                previewCoordinates[4] = value;
                OnPropertyChanged(nameof(PreviewCoordinate5));
            }
        }
        public ObservableCollection<System.Windows.Point> PreviewCoordinate6
        {
            get
            {
                return previewCoordinates[5];
            }
            private set
            {
                previewCoordinates[5] = value;
                OnPropertyChanged(nameof(PreviewCoordinate6));
            }
        }
        public ObservableCollection<System.Windows.Point> PreviewCoordinate7
        {
            get
            {
                return previewCoordinates[6];
            }
            private set
            {
                previewCoordinates[6] = value;
                OnPropertyChanged(nameof(PreviewCoordinate7));
            }
        }
        public ObservableCollection<System.Windows.Point> PreviewCoordinate8
        {
            get
            {
                return previewCoordinates[7];
            }
            private set
            {
                previewCoordinates[7] = value;
                OnPropertyChanged(nameof(PreviewCoordinate8));
            }
        }
        public ObservableCollection<System.Windows.Point> PreviewCoordinate9
        {
            get
            {
                return previewCoordinates[8];
            }
            private set
            {
                previewCoordinates[8] = value;
                OnPropertyChanged(nameof(PreviewCoordinate9));
            }
        }
        public ObservableCollection<System.Windows.Point> PreviewCoordinate10
        {
            get
            {
                return previewCoordinates[9];
            }
            private set
            {
                previewCoordinates[9] = value;
                OnPropertyChanged(nameof(PreviewCoordinate10));
            }
        }

        public Double PreviewWindowWidth { get; set; }
        public Double PreviewWindowHeight { get; set; }

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
        #endregion

        #region DelegateCommand CallBacks
        // Open Working File
        public void OpenWorkingFile()
        {
            FWorkingFileIO.WorkingFileIO workingFileIO = new FWorkingFileIO.WorkingFileIO();
            String wFilePath = General.Messenger.OpenFileMessenger.Show("WorkingFile (*.wrk)|*.wrk");

            //Assign event callbacks
            workingFileIO.NotifyOpeningFileFinished += WorkingFileImported;

            if (wFilePath != null)
            {
                workingFileIO.OpenFile(wFilePath);
            }
        }

        public void SaveWorkingFile()
        {
            FWorkingFileIO.WorkingFileIO workingFileIO = new FWorkingFileIO.WorkingFileIO();
            String wFilePath = General.Messenger.SaveFileMessenger.Show("WorkingFile (*.wrk)|*.wrk");

            if (wFilePath != null)
            {
                workingFileIO.SaveFile(wFilePath);
            }
        }

        // Open Optimizing Configuration Window
        public void OpenOptimizingConfigurationDialog()
        {
            // Issue the Messenger displaying OptConfig.
            Messenger.OptConfigDialogMessenger.Show();
        }
        public bool IsOptConfigDialogEnabled()
        {
            return true;
        }
        //

        // Show Coefficient Manager
        public void OpenCoefficientManager()
        {
            // Issue the Messenger displaying Coefficient Manager.
            Messenger.CoefManagerMessenger.Show();
        }
        public bool IsCoefManagerEnabled()
        {
            return (OptimizingConfiguration.Instance.BasisAirfoils != null);
        }
        //

        // Re-Draw the preview windows
        public void ReDrawPreviewWindow()
        {
            UpdateAirfoilPreviews();
        }
        //

        // Display the Airfoil Specifications
        public  void DisplaySpecifications(String windowNumber)
        {
            if (combinedAirfoils != null)
            {
                // Null check
                foreach (var item in combinedAirfoils)
                {
                    if (item.CombinedAirfoil == null)
                    {
                        return;
                    }
                }
                
                // Create Specifications Table
                AirfoilSpecifications = CreateTable(combinedAirfoils[Convert.ToInt32(windowNumber)].CombinedAirfoil);
            }
        }
        #endregion

        #region CommandProperties
        public General.DelegateCommand OpenWorkingFileCommand
        {
            get => openWorkingFile;
        }
        public General.DelegateCommand SaveWorkingFileCommand
        {
            get => saveWorkingFile;
        }
        public General.DelegateCommand ShowOptConfigDialog
        {
            get { return showOprConfigDialog; }
        }
        public General.DelegateCommand ShowCoefficientManager
        {
            get { return showCoefficientManager; }
        }
        public General.DelegateCommand UpdatePreviewWindows
        {
            get => updatePreviewWindow;
        }
        public General.ParamDelegateCommand<String> SetSpecifications
        {
            get => setSpecifications;
        }
        #endregion

        private void UpdateAirfoilPreviews()
        {
            if (combinedAirfoils[0].CombinedAirfoil != null)
            {
                PreviewCoordinate1 = General.AirfoilPreview.GetPreviewPointList(combinedAirfoils[0].CombinedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
                PreviewCoordinate2 = General.AirfoilPreview.GetPreviewPointList(combinedAirfoils[1].CombinedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
                PreviewCoordinate3 = General.AirfoilPreview.GetPreviewPointList(combinedAirfoils[2].CombinedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
                PreviewCoordinate4 = General.AirfoilPreview.GetPreviewPointList(combinedAirfoils[3].CombinedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
                PreviewCoordinate5 = General.AirfoilPreview.GetPreviewPointList(combinedAirfoils[4].CombinedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
                PreviewCoordinate6 = General.AirfoilPreview.GetPreviewPointList(combinedAirfoils[5].CombinedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
                PreviewCoordinate7 = General.AirfoilPreview.GetPreviewPointList(combinedAirfoils[6].CombinedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
                PreviewCoordinate8 = General.AirfoilPreview.GetPreviewPointList(combinedAirfoils[7].CombinedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
                PreviewCoordinate9 = General.AirfoilPreview.GetPreviewPointList(combinedAirfoils[8].CombinedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
                PreviewCoordinate10 = General.AirfoilPreview.GetPreviewPointList(combinedAirfoils[9].CombinedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
            }
        }

        private T[][] ConvertArrayToJuggedArray<T>(T[,] array)
        {
            var length = array.GetLength(0);
            var width = array.GetLength(1);
            var jArray = new T[length][];

            for (int i = 0; i < length; i++)
            {
                jArray[i] = new T[width];
                for (int j = 0; j < width; j++)
                {
                    jArray[i][j] = array[i, j];
                }
            }

            return jArray;
        }
        private T[] GetRowArray<T>(T[,] array, int columnNumber)
        {
            var length = array.GetLength(0);
            var width = array.GetLength(1);

            var rArray = new T[width][];

            for (int i = 0; i < width; i++)
            {
                rArray[i] = new T[length];
                for (int j = 0; j < length; j++)
                {
                    rArray[i][j] = array[j, i];
                }
            }
            return rArray[columnNumber];
        }

        private System.Data.DataTable CreateTable(Airfoil.AirfoilManager airfoil)
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
    }
}
