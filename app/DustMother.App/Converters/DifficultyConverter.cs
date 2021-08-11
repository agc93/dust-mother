using System;
using Windows.UI.Xaml.Data;

namespace DustMother.App.Converters
{
    public class DifficultyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var intVal = value as int?;
            return intVal switch
            {
                0 => "Easy",
                1 => "Normal",
                2 => "Hard",
                3 => "Mercenary",
                _ => "Unknown"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is string strValue
                ? strValue switch
                {
                    "Easy" => 0,
                    "Normal" => 1,
                    "Hard" => 2,
                    "Mercenary" => 3,
                    _ => -1
                }
                : -1;
        }
    }
}
