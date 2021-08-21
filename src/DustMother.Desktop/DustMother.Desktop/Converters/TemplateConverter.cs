using Microsoft.UI.Xaml.Data;
using System;

namespace DustMother.App.Converters
{
    public abstract class TemplateConverter<TValue> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var outValue = string.Empty;
            if (value is TValue dto && targetType == typeof(string))
            {
                var converted = Convert(dto, targetType);
                outValue = converted is string strOut ? strOut : converted?.ToString();
            }
            else
            {
                outValue = outValue?.ToString();
            }
            if (parameter is string paramValue)
            {
                var template = paramValue.Replace("[]", "{0}");
                outValue = string.Format(template, outValue);
            }
            return outValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public abstract object Convert(TValue value, Type targetType);
    }
}
