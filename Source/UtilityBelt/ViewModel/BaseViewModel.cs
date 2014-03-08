using System;
using System.ComponentModel;

namespace UtilityBelt.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Notify property changed imnplementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
