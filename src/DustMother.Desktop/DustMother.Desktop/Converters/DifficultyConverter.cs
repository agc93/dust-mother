using System;
using Microsoft.UI.Xaml.Data;

namespace DustMother.App.Converters
{
    public class DifficultyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var intVal = value as int?;
            return PropertyValueConverter.Difficulty.FromValue(intVal);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return PropertyValueConverter.Difficulty.ToValue(value as string);
        }
    }
}
