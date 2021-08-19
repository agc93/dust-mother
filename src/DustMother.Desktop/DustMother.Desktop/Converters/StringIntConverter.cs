using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustMother.App.Converters
{
    public class IntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int intVal)
            {
                return intVal.ToString();
            }
            //return string.Empty;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string symbol)
            {
                return int.TryParse(symbol, out var symValue) ? symValue : null;
            }
            return null;
        }
    }
    class StringIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string symbol)
            {
                bool? result = symbol switch
                {
                    "☑" => true,
                    "🛒" => false,
                    _ => null
                };
                return result;
            }
            return null;
        }
    }
}
