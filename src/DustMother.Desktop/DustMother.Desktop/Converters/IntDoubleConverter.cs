using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustMother.App.Converters
{
    class IntDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int intVal)
            {
                return System.Convert.ToDouble(intVal);
            } else if (value is not null)
            {
                return double.TryParse(value.ToString(), out var dVal) ? dVal : 0;
            } else if (parameter?.ToString() is not null)
            {
                return double.TryParse(parameter.ToString(), out var dVal) ? dVal : 0;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is double dVal)
            {
                return System.Convert.ToInt32(dVal);
            } else if (value is not null)
            {
                return int.TryParse(value.ToString(), out var iVal) ? iVal : -1;
            }
            return null;
        }
    }
}
