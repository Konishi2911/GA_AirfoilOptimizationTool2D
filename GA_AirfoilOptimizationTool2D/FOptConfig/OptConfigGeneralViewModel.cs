using System;
using System.Collections.Generic;
using System.Linq;

namespace GA_AirfoilOptimizationTool2D.FOptConfig
{
    class OptConfigGeneralViewModel : General.ViewModelBase
    {
        private FOptConfig.AirfoilRepresentationModeViewModel selectedAirfoilRepresentationMode;
        private FOptConfig.OptimizingParameterTypeViewModel selectedOptimizingParameter;

        public IEnumerable<FOptConfig.AirfoilRepresentationModeViewModel> AirfoilRepresentationModes { get; private set; }
        public FOptConfig.AirfoilRepresentationModeViewModel SelectedAirfoilRepresentationMode
        {
            get
            {
                return this.selectedAirfoilRepresentationMode;
            }
            set
            {
                this.selectedAirfoilRepresentationMode = value;
                this.OnPropertyChanged("SelectedAirfoilRepresentationMode");
            }
        }

        public IEnumerable<FOptConfig.OptimizingParameterTypeViewModel> OptimizingParameterTypes { get; private set; }
        public FOptConfig.OptimizingParameterTypeViewModel SelectedOptimizingParameter
        {
            get
            {
                return this.selectedOptimizingParameter;
            }
            set
            {
                this.selectedOptimizingParameter = value;
                this.OnPropertyChanged("SelectedOptimizingParameter");
            }
        }

        public OptConfigGeneralViewModel()
        {
            this.AirfoilRepresentationModes = FOptConfig.AirfoilRepresentationModeViewModel.Create();
            this.SelectedAirfoilRepresentationMode = this.AirfoilRepresentationModes.First();

            this.OptimizingParameterTypes = FOptConfig.OptimizingParameterTypeViewModel.Create();
            this.SelectedOptimizingParameter = this.OptimizingParameterTypes.First();
        }
    }
}
