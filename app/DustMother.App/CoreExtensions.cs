using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using MUXC = Microsoft.UI.Xaml.Controls;

namespace DustMother.App
{
    internal static class CoreExtensions
    {
        internal static T JsonClone<T>(this T value)
        {
            var json = JsonSerializer.Serialize(value);
            return JsonSerializer.Deserialize<T>(json);
        }

        internal static MUXC.NavigationViewItem GetCurrentItemForFrame(this MUXC.NavigationView navigationView, Frame contentFrame)
        {
            return navigationView.MenuItems.Cast<MUXC.NavigationViewItem>().FirstOrDefault(t => (string)t.Tag == contentFrame.Content.GetType().Name);
        }
    }
}
