using System;
using System.Linq;
using Microsoft.UI.Xaml.Data;

#nullable enable
namespace DustMother.App.Converters
{
    public class BooleanToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var param = (string)parameter;
                var trueCase = param.Split(':').First();
                var falseCase = param.Split(':') is var segments && segments.Length > 1 ? segments[1] : $"Not {trueCase}";
                if (value is bool? && ((bool?)value).HasValue)
                {
                    value = ((bool?)value).Value;
                }
                return (value is bool unlocked)
                    ? unlocked
                        ? trueCase
                        : falseCase
                    : string.Empty;
            } catch
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
#nullable disable