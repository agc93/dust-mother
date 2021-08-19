using System;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;

#nullable enable
namespace DustMother.App.Converters
{
    public class PurchasedGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool purchased)
                ? purchased
                    ? "☑" : "🛒"
                : "❔";
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