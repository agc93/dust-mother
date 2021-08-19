using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DustMother.App.Converters
{
    public class NotNullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value != null ^
                (parameter as string ?? string.Empty).Equals("Reverse")) ?
                Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            //return (value is Visibility && (Visibility)value == Visibility.Visible) ^
            //    (parameter as string ?? string.Empty).Equals("Reverse");
            throw new NotImplementedException();
        }
    }
}
