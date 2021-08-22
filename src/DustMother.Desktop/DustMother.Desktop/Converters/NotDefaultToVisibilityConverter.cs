using System;

namespace DustMother.App.Converters
{
    public class NotDefaultToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var srcType = value.GetType();
            return (value != srcType.GetDefault() ^
                (parameter as string ?? string.Empty).Equals("Reverse")) ?
                Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
