using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Bugemos
{
    public class ImageSrc : IValueConverter
    {
        private static readonly Dictionary<string, BitmapImage> images = new Dictionary<string, BitmapImage>();

        // Clean up text fields from each SyndicationItem. 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string address = value as string;
            if (address == null) return DependencyProperty.UnsetValue;

            if (images.ContainsKey(address))
            {
                return images[address];
            }

            BitmapImage bi = new BitmapImage();
            using (IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoFile.FileExists(address))
                {
                    using (IsolatedStorageFileStream imgStream = isoFile.OpenFile(address, FileMode.Open, FileAccess.Read))
                    {
                        try
                        {
                            bi.SetSource(imgStream);
                            images[address] = bi;
                        }
                        catch (Exception e)
                        {
                            return DependencyProperty.UnsetValue;
                        }
                    }
                }
            }

            return bi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}