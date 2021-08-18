using MahApps.Metro.IconPacks;
using System;
using Microsoft.UI.Xaml.Data;

#nullable enable
namespace DustMother.App.Converters
{
    public class UnlockedBoxIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool? && ((bool?)value).HasValue)
            {
                value = ((bool?)value).Value;
            }
            return (value is bool unlocked)
                ? unlocked
                    ? PackIconBoxIconsKind.RegularLockOpenAlt
                    : PackIconBoxIconsKind.RegularLock
                : PackIconBoxIconsKind.RegularQuestionMark;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is PackIconBoxIconsKind symbol)
            {
                bool? result = symbol switch
                {
                    PackIconBoxIconsKind.RegularLockOpenAlt => true,
                    PackIconBoxIconsKind.RegularLock => false,
                    _ => null
                };
                return result;
            }
            return null;
        }
    }

    public class PurchasedBoxIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool? && ((bool?)value).HasValue)
            {
                value = ((bool?)value).Value;
            }
            return (value is bool unlocked)
                ? unlocked
                    ? PackIconBoxIconsKind.RegularCheckCircle
                    : PackIconBoxIconsKind.RegularDollarCircle
                : PackIconBoxIconsKind.RegularHelpCircle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is PackIconBoxIconsKind symbol)
            {
                bool? result = symbol switch
                {
                    PackIconBoxIconsKind.RegularCheckCircle => true,
                    PackIconBoxIconsKind.RegularDollarCircle => false,
                    _ => null
                };
                return result;
            }
            return null;
        }
    }
}
#nullable disable