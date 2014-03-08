using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bugemos
{
    public class ProgressVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool downloading = value is bool && ((bool)value);

            return downloading ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
