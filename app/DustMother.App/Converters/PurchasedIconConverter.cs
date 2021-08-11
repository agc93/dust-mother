using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

#nullable enable
namespace DustMother.App.Converters
{
    public class PurchasedIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool purchased)
                ? purchased
                    ? Symbol.Accept : Symbol.Shop
                : Symbol.Help;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Symbol symbol)
            {
                bool? result = symbol switch
                {
                    Symbol.Accept => true,
                    Symbol.Shop => false,
                    _ => null
                };
                return result;
            }
            return null;
        }
    }

    public class CompletedIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool? && ((bool?)value).HasValue)
            {
                value = ((bool?)value).Value;
            } 
            return (value is bool purchased)
                ? purchased
                    ? Symbol.Accept : Symbol.Cancel
                : Symbol.Help;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Symbol symbol)
            {
                bool? result = symbol switch
                {
                    Symbol.Accept => true,
                    Symbol.Cancel => false,
                    _ => null
                };
                return result;
            }
            return null;
        }
    }
}
#nullable disable