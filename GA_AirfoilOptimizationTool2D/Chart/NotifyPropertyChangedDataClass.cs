using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Chart
{
    public abstract class NotifyPropertyChangedDataClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h != null)  
            {
                h(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public virtual bool SetProperty<T>(ref T target, T value, string propertyName)
        {
            if (target == null)
            {
                if (value == null)
                {
                    return false;
                }
            }
            else
            {
                if (target.Equals(value))
                {
                    return false;
                }
            }

            target = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
