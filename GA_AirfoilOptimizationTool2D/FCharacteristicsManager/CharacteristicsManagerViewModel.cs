using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media;

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

        private List<Airfoil.Characteristics.AngleBasedCharacteristics> liftProfiles;
        private List<Airfoil.Characteristics.AngleBasedCharacteristics> dragProfiles;

        private SourceSelectorViewModel selectedSource;
        private TargetAirfoilSelectorViewModel selectedTargetAirfoil;
        private CharacteristicsSelectorViewModel selectedCharacteristics;

        private System.Windows.Media.Brush liftIndicatorColor;
        private System.Windows.Media.Brush dragIndicatorColor;

        private Chart.AxisStyle xStyle;
        private Chart.AxisStyle yStyle;
        private Point[] characteristicsPlot;
        #endregion

        #region DelegateCommand
        public General.DelegateCommand LiftCoefProfileSelection { get; private set; }
        public General.DelegateCommand DragCoefProfileSelection { get; private set; }
        public General.DelegateCommand ClickApplyButton { get; set; }
        #endregion

        #region Properties
        public System.Windows.Media.Brush LiftIndicatorColor 
        {
            get => liftIndicatorColor;
            private set
            {
                liftIndicatorColor = value;
                OnPropertyChanged(nameof(LiftIndicatorColor));
            }
        }
        public System.Windows.Media.Brush DragIndicatorColor
        {
            get => dragIndicatorColor;
            private set
            {
                dragIndicatorColor = value;
                OnPropertyChanged(nameof(DragIndicatorColor));
            }
        }

        public List<SourceSelectorViewModel> Sources { get; private set; }
        public SourceSelectorViewModel SelectedSource
        {
            get => selectedSource;
            set
            {
                selectedSource = value;
                OnPropertyChanged(nameof(this.SelectedSource));
            }
        }

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

        public List<CharacteristicsSelectorViewModel> Characteristics { get; private set; }
        public CharacteristicsSelectorViewModel SelectedCharacteristics
        {
            get => selectedCharacteristics;
            set
            {
                selectedCharacteristics = value;
                OnPropertyChanged(nameof(this.SelectedCharacteristics));
            }
        }

        public Chart.AxisStyle XStyle
        {
            get => xStyle;
            set
            {
                xStyle = value;
                OnPropertyChanged(nameof(XStyle));
            }
        }
        public Chart.AxisStyle YStyle
        {
            get => yStyle;
            set
            {
                yStyle = value;
                OnPropertyChanged(nameof(YStyle));
            }
        }
        public Point[] CharacteristicsPlot
        {
            get => characteristicsPlot;
            set
            {
                characteristicsPlot = value;
                OnPropertyChanged(nameof(CharacteristicsPlot));
            }
        }
        #endregion

        public CharacteristicsManagerViewModel()
        {
            // Initialize
            nInterpolatedPoints = 100;
            liftProfiles = new List<Airfoil.Characteristics.AngleBasedCharacteristics>();
            dragProfiles = new List<Airfoil.Characteristics.AngleBasedCharacteristics>();

            // Combobox about Source related
            this.Sources = FCharacteristicsManager.SourceSelectorViewModel.Create();
            this.SelectedSource = Sources[0];

            // Combobox about target airfoil related
            currentAirfoilNumber = 0;
            TargetAirfoils = new System.Collections.ObjectModel.ObservableCollection<TargetAirfoilSelectorViewModel>();

            LiftCoefProfileSelection = new General.DelegateCommand(SelectLiftCoefProfile, () => true);
            DragCoefProfileSelection = new General.DelegateCommand(SelectDragCoefProfile, () => true);
            ClickApplyButton = new General.DelegateCommand(ApplyButtonClicked, IsApplyButtonAvailable);

            // Characteristics Chart Related
            XStyle = new Chart.AxisStyle() { Min = 0.0, Max = 10.0 };
            YStyle = new Chart.AxisStyle() { Min = 0.0, Max = 2.0 };
            this.Characteristics = CharacteristicsSelectorViewModel.Create();
            this.SelectedCharacteristics = Characteristics[0];

            // Assign Events
            this.PropertyChanged += This_PropertyChanged;

            CloneTargetAirfoil();

            updateIndicator();

            //// Clone Offsprng Airfoils
            //if (SelectedSource.Source == PopulationSources.CurrentPopulation)
            //{
            //    sourceAirfoils = AirfoilOptimizationResource.Instance.CurrentPopulations;
            //}
            //else if (selectedSource.Source == PopulationSources.OffspringCandidates)
            //{
            //    sourceAirfoils = AirfoilOptimizationResource.Instance.OffspringCandidates;
            //}
            //AssignTargetAirfoils();
        }

        public void CloneTargetAirfoil()
        {
            // Clone Offsprng Airfoils
            if (SelectedSource.Source == PopulationSources.CurrentPopulation)
            {
                sourceAirfoils = AirfoilOptimizationResource.Instance.CurrentPopulations;
            }
            else if (selectedSource.Source == PopulationSources.OffspringCandidates)
            {
                sourceAirfoils = AirfoilOptimizationResource.Instance.OffspringCandidates;
            }
            AssignTargetAirfoils();
        }

        /// <summary>
        /// Assign TargetAirfoils Collection source airfoils to Display on a comboBox.
        /// </summary>
        private void AssignTargetAirfoils()
        {
            TargetAirfoils.Clear();
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
                liftProfiles.Add(liftProfile);
                updateIndicator();
                updateCharacteristicsPlot();
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
                dragProfiles.Add(dragProfile);
                updateIndicator();
                updateCharacteristicsPlot();
            }
        }

        private void ApplyButtonClicked()
        {
            if (SelectedSource.Source == PopulationSources.CurrentPopulation)
            {
                AirfoilOptimizationResource.Instance.CurrentPopulations.AddCharacteristics(liftProfiles, dragProfiles);
            }
            else if (SelectedSource.Source == PopulationSources.OffspringCandidates)
            {
                AirfoilOptimizationResource.Instance.OffspringCandidates = this.sourceAirfoils;
            }
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

                updateIndicator();
                updateCharacteristicsPlot();
            }
            else if ( e.PropertyName == nameof(this.SelectedSource))
            {
                CloneTargetAirfoil();
            }
            else if (e.PropertyName == nameof(this.selectedCharacteristics))
            {
                updateCharacteristicsPlot();
            }
        }
        #endregion

        private void updateIndicator()
        {
            // Check wheather Lift is imported
            if (isLiftDetected(sourceAirfoils.CombinedAirfoils[currentAirfoilNumber]))
            {
                this.LiftIndicatorColor = new System.Windows.Media.SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                this.LiftIndicatorColor = new System.Windows.Media.SolidColorBrush(Colors.Gray);
            }
            // Check wheather Drag is imported
            if (isDragDetected(sourceAirfoils.CombinedAirfoils[currentAirfoilNumber]))
            {
                this.DragIndicatorColor = new System.Windows.Media.SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                this.DragIndicatorColor = new System.Windows.Media.SolidColorBrush(Colors.Gray);
            }
        }

        private bool isLiftDetected(Airfoil.AirfoilManager airfoil)
        {
            return isCharacteristicsDetected(airfoil, 0);
        }
        private bool isDragDetected(Airfoil.AirfoilManager airfoil)
        {
            return isCharacteristicsDetected(airfoil, 1);
        }

        private bool isCharacteristicsDetected(Airfoil.AirfoilManager airfoil, int type)
        {
            if (airfoil == null)
            {
                return false;
            }

            if (type == 0)
            {
                return airfoil.LiftProfile != null;
            }
            else if (type == 1)
            {
                return airfoil.DragProfile != null;
            }

            return false;
        }

        private void updateCharacteristicsPlot()
        {
            List<Point> points = new List<Point>();
            if (SelectedCharacteristics.Source == CharacteristicsSources.Lift &&
                sourceAirfoils.CombinedAirfoils[currentAirfoilNumber].LiftProfile != null)
            {
                XStyle = new Chart.AxisStyle() { Min = 0.0, Max = 10.0 };
                YStyle = new Chart.AxisStyle() { Min = 0.0, Max = 2.0 };

                for (int i = 0;
                    i < sourceAirfoils.CombinedAirfoils[currentAirfoilNumber].LiftProfile.InterpolatedCharacteristics.GetLength(0);
                    ++i)
                {
                    var p = sourceAirfoils.CombinedAirfoils[currentAirfoilNumber].LiftProfile.InterpolatedCharacteristics;
                    points.Add(new Point(p[i, 0], p[i, 1]));
                }
            }
            else if (SelectedCharacteristics.Source == CharacteristicsSources.Drag &&
                    sourceAirfoils.CombinedAirfoils[currentAirfoilNumber].DragProfile != null)
            {
                XStyle = new Chart.AxisStyle() { Min = 0.0, Max = 10.0 };
                YStyle = new Chart.AxisStyle() { Min = 0.0, Max = 0.3 };

                for (int i = 0;
                    i < sourceAirfoils.CombinedAirfoils[currentAirfoilNumber].DragProfile.InterpolatedCharacteristics.GetLength(0);
                    ++i)
                {
                    var p = sourceAirfoils.CombinedAirfoils[currentAirfoilNumber].DragProfile.InterpolatedCharacteristics;
                    points.Add(new Point(p[i, 0], p[i, 1]));
                }
            }
            else if (SelectedCharacteristics.Source == CharacteristicsSources.LiftDrag &&
                    sourceAirfoils.CombinedAirfoils[currentAirfoilNumber].LiftDragProfile != null)
            {
                XStyle = new Chart.AxisStyle() { Min = 0.0, Max = 10.0 };
                YStyle = new Chart.AxisStyle() { Min = 0.0, Max = 60.0 };

                for (int i = 0;
                    i < sourceAirfoils.CombinedAirfoils[currentAirfoilNumber].LiftDragProfile.InterpolatedCharacteristics.GetLength(0);
                    ++i)
                {
                    var p = sourceAirfoils.CombinedAirfoils[currentAirfoilNumber].LiftDragProfile.InterpolatedCharacteristics;
                    points.Add(new Point(p[i, 0], p[i, 1]));
                }
            }
            CharacteristicsPlot = points.ToArray();
        }
    }
}
