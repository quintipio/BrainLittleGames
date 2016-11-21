using System;
using Windows.UI.Xaml.Data;

namespace JeuxDeLogiqueWin10.Converter
{
    /// <summary>
    /// Conveter de double? double
    /// </summary>
    public class DoubleDoubleNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = value as double?;
            return val ?? 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (double) value;
        }
    }
}
