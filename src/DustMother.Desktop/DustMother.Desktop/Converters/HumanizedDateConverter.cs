using Humanizer;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustMother.App.Converters
{
    public class HumanizedDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset dto)
            {
                return dto.Humanize(DateTimeOffset.Now);
            }
            if (value is DateTime dt)
            {
                return dt.Humanize(parameter is string { }  && parameter.ToString().ToLower() == "utc" ,DateTime.Now);
            }
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
