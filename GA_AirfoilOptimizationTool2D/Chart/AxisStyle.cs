using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Chart
{
    public class AxisStyle : NotifyPropertyChangedDataClass
    {
        private double min = 0.0;
        public double Min
        {
            get => min;
            set
            {
                if (value < Max)
                {
                    if (SetProperty(ref min, value, nameof(Min)))
                    {
                        OnPropertyChanged(nameof(Width));
                    }
                }
            }
        }

        private double max = 100.0;
        public double Max
        {
            get => max;
            set
            {
                if (value > Min)
                {
                    if (SetProperty(ref max, value, nameof(Max)))
                    {
                        OnPropertyChanged(nameof(Width));
                    }
                }
            }
        }

        public double Width
        {
            get => Max - Min;
        }

        private double step = 10.0;
        public double Step
        {
            get => step;
            set
            {
                if (value > 0.0)
                {
                    SetProperty(ref step, value, nameof(Step));
                }
            }
        }
    }
}
