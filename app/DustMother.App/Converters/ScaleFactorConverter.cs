using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DustMother.App.Converters
{
    public class ScaleFactorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var defaultValue = parameter as string ?? null;
            if (string.IsNullOrWhiteSpace(defaultValue))
            {
                throw new ArgumentException("No parameter provided!");
            }
            var baseValue = System.Convert.ToDouble(defaultValue);
            if (value is double scaleFactor)
            {
                return System.Convert.ChangeType(Math.Abs(scaleFactor * baseValue), targetType);
            }
            throw new ArgumentException("wait what.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
