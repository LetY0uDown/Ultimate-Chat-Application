using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WPFLibrary.Converters;

public sealed class BytesToBitmapConverter : IValueConverter
{
    public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
    {
        var bytes = (byte[])value;

        if (bytes is null || bytes.Length == 0)
            return null!;

        BitmapImage image = new();

        using (MemoryStream ms = new(bytes)) {
            image.BeginInit();
            image.StreamSource = ms;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
        }

        return image;
    }

    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}