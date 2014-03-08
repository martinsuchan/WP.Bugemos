using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bugemos
{
    public class DateFormatter : IValueConverter
    {
        private static readonly DateTimeFormatInfo cs = new CultureInfo("cs-CZ").DateTimeFormat;

        // Clean up text fields from each SyndicationItem. 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime)) return DependencyProperty.UnsetValue;
            DateTime date = (DateTime) value;

            return date.ToString("f", cs);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}