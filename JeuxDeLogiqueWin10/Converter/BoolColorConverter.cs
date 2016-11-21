using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace JeuxDeLogiqueWin10.Converter
{
    /// <summary>
    /// Converter pour mettre en rouge les champs double
    /// </summary>
    public class BoolColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = value as bool?;

            if (val.HasValue && val.Value)
            {
                return new SolidColorBrush(Color.FromArgb(255, 145, 145, 145));
            }
            return new SolidColorBrush(Color.FromArgb(255, 191, 28, 28));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
