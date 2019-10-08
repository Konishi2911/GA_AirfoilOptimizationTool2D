using System;
using System.Collections.ObjectModel;

namespace GA_AirfoilOptimizationTool2D.FMainWindow
{
    class MainWindowViewModel : General.ViewModelBase
    {
        private const int NumberOfChildren = 10;

        private Models.BasisAirfoils basisAirfoils;
        private General.DelegateCommand showOprConfigDialog;
        private General.DelegateCommand showCoefficientManager;
        private Models.AirfoilSynthesizer[] synthesizedAirfoils;
        private Double[,] coefficients;
        private ObservableCollection<System.Windows.Point>[] previewCoordinates;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            // Allocate Event CallBack Functions
            OptimizingConfiguration.Instance.PropertyChanged += SourceChanged;
            //

            openOptConfigDialog = new Action(OpenOptimizingConfigurationDialog);
            isOptConfigEnabled = new Func<bool>(IsOptConfigDialogEnabled);
            // Assign the delegate Command
            showOprConfigDialog = new General.DelegateCommand(openOptConfigDialog, isOptConfigEnabled);
            showCoefficientManager = new General.DelegateCommand(OpenCoefficientManager, IsCoefManagerEnabled);
            //

            // Instantiate Fields
            synthesizedAirfoils = new Models.AirfoilSynthesizer[NumberOfChildren];
            for (int i = 0; i < NumberOfChildren; i++)
            {
                synthesizedAirfoils[i] = new Models.AirfoilSynthesizer();
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
        private void SourceChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (coefficients == null)
            {
                return;
            }

            if (e.PropertyName == nameof(OptimizingConfiguration.BasisAirfoils))
            {
                // Update baseAirfoil
                this.basisAirfoils = Models.BasisAirfoils.Convert(OptimizingConfiguration.Instance.BasisAirfoils);

                // Re-synthesize Airfoil
                for (int i = 0; i < NumberOfChildren; i++)
                {
                    synthesizedAirfoils[i].SynthesizeAirfoil(basisAirfoils.AirfoilGroup.ToArray(), ConvertArrayToJuggedArray(coefficients)[i]);
                }
                // Re-generate the coordinates for airfoil previewing.
                UpdateAirfoilPreviews();
            }
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
        #endregion

        #region DelegateCommand CallBacks
        public void OpenOptimizingConfigurationDialog()
        {
            // Issue the Messenger displaying OptConfig.
            Messenger.OptConfigDialogMessenger.Show();
        }
        public bool IsOptConfigDialogEnabled()
        {
            return true;
        }

        public void OpenCoefficientManager()
        {
            // Issue the Messenger displaying Coefficient Manager.
            Messenger.CoefManagerMessenger.Show();
        }
        public bool IsCoefManagerEnabled()
        {
            return (OptimizingConfiguration.Instance.BasisAirfoils != null);
        }
        #endregion
        public General.DelegateCommand ShowOptConfigDialog
        {
            get { return showOprConfigDialog; }
        }
        public General.DelegateCommand ShowCoefficientManager
        {
            get { return showCoefficientManager; }
        }

        private void UpdateAirfoilPreviews()
        {
            PreviewCoordinate1 = General.AirfoilPreview.GetPreviewPointList(synthesizedAirfoils[0].SynthesizedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
            PreviewCoordinate2 = General.AirfoilPreview.GetPreviewPointList(synthesizedAirfoils[1].SynthesizedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
            PreviewCoordinate3 = General.AirfoilPreview.GetPreviewPointList(synthesizedAirfoils[2].SynthesizedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
            PreviewCoordinate4 = General.AirfoilPreview.GetPreviewPointList(synthesizedAirfoils[3].SynthesizedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
            PreviewCoordinate5 = General.AirfoilPreview.GetPreviewPointList(synthesizedAirfoils[4].SynthesizedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
            PreviewCoordinate6 = General.AirfoilPreview.GetPreviewPointList(synthesizedAirfoils[5].SynthesizedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
            PreviewCoordinate7 = General.AirfoilPreview.GetPreviewPointList(synthesizedAirfoils[6].SynthesizedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
            PreviewCoordinate8 = General.AirfoilPreview.GetPreviewPointList(synthesizedAirfoils[7].SynthesizedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
            PreviewCoordinate9 = General.AirfoilPreview.GetPreviewPointList(synthesizedAirfoils[8].SynthesizedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
            PreviewCoordinate10 = General.AirfoilPreview.GetPreviewPointList(synthesizedAirfoils[9].SynthesizedAirfoil, PreviewWindowHeight, PreviewWindowWidth);
        }

        private T[][]ConvertArrayToJuggedArray<T>(T[,] array)
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
    }
}
