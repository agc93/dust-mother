using Humanizer;
using System;

namespace DustMother.App.Converters
{
    public class HumanizedOffsetConverter : TemplateConverter<DateTimeOffset>
    {
        public override object Convert(DateTimeOffset value, Type targetType)
        {
            return value.Humanize(DateTimeOffset.Now);
        }
    }
}
