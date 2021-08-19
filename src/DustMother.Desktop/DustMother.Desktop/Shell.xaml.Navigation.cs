using DustMother.App.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustMother.App
{
    public sealed partial class Shell : Page, INavigation
    {
        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            // Navigates, but does not update the Menu.
            // ContentFrame.Navigate(typeof(HomePage));
            contentFrame.Navigate(typeof(IntroPage));
            NavigationView.Header = "Dust Mother";


        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            SetCurrentNavigationViewItem(args.SelectedItemContainer as NavigationViewItem);
        }

        public List<NavigationViewItem> GetNavigationViewItems()
        {
            var result = new List<NavigationViewItem>();
            var items = NavigationView.MenuItems.Select(i => (NavigationViewItem)i).ToList();
            items.AddRange(NavigationView.FooterMenuItems.Select(i => (NavigationViewItem)i));
            result.AddRange(items);

            foreach (NavigationViewItem mainItem in items)
            {
                result.AddRange(mainItem.MenuItems.Select(i => (NavigationViewItem)i));
            }

            return result;
        }

        public List<NavigationViewItem> GetNavigationViewItems(Type type)
        {
            return GetNavigationViewItems().Where(i => (!string.IsNullOrWhiteSpace(i.Tag?.ToString()) && i.Tag.ToString() == type.FullName) || $"DustMother.App.{i.Tag}" == type.FullName).ToList();
        }

        public List<NavigationViewItem> GetNavigationViewItems(Type type, string title)
        {
            return GetNavigationViewItems(type).Where(ni => ni.Content.ToString() == title).ToList();
        }

        public void SetCurrentNavigationViewItem(NavigationViewItem item)
        {
            if (item == null)
            {
                return;
            }

            if (item.Tag == null)
            {
                return;
            }
            var tagValue = item.Tag.ToString();
            var param = item.Content;
            if (tagValue.Contains(":"))
            {
                var split = tagValue.Split(":");
                tagValue = split.First();
                param = split.Last();
            }

            contentFrame.Navigate(Type.GetType(tagValue.Contains("DustMother") ? tagValue : $"DustMother.App.{tagValue}"), param);
            NavigationView.Header = item.Content;
            NavigationView.SelectedItem = item;
        }

        public NavigationViewItem GetCurrentNavigationViewItem()
        {
            return NavigationView.SelectedItem as NavigationViewItem;
        }
    }
}
