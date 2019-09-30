using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig
{
    public class AirfoilSpecificationGridViewModel : General.ViewModelBase
    {
        public String ParameterName { get; set; }
        public String Value { get; private set; }

        public AirfoilSpecificationGridViewModel(String name, String value)
        {
            ParameterName = name;
            Value = value;
        }
    }
}
