using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Converter
{
    public class PointCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value.GetType() == typeof(ObservableCollection<System.Windows.Point>) && targetType == typeof(PointCollection))
            {
                var pointCollection = new PointCollection();
                foreach (var point in value as ObservableCollection<System.Windows.Point>)
                {
                    pointCollection.Add(point);
                }
                return pointCollection;
            }
            return null;
        }

        /// <summary>
        /// Do not be used.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
