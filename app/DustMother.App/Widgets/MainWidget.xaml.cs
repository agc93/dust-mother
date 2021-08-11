using DustMother.App.Pages;
using DustMother.Core;
using Microsoft.Gaming.XboxGameBar;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MUXC = Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DustMother.App.Widgets
{
    public class MainWidgetPage : WidgetBasePage<SettingsSave>
    {
        public override async Task Refresh()
        {
            await RefreshSaveAsync(r => r.GetSettingsDataAsync(), c => c?.ResolutionScale != null);
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWidget : MainWidgetPage
    {
        

        //public MainWidgetModel ViewModel { get; set; }
        public MainWidget()
        {
            this.InitializeComponent();
            Loaded += MainWidget_Loaded;
        }

        private void MainWidget_Loaded(object sender, RoutedEventArgs e)
        {
            mainWidgetNav.SelectedItem = statsViewItem;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _self = e.Parameter as WidgetHandle;
            //base.OnNavigatedTo(e);
            var readable = await _reader.CheckFileAccessAsync();
            FileReadable = readable;
            //Refresh()
        }

        private void NavigationView_SelectionChanged(MUXC.NavigationView sender, MUXC.NavigationViewSelectionChangedEventArgs args)
        {
            FrameNavigationOptions navOptions = new FrameNavigationOptions
            {
                TransitionInfoOverride = args.RecommendedNavigationTransitionInfo
            };
            if (sender.PaneDisplayMode == MUXC.NavigationViewPaneDisplayMode.Top)
            {
                navOptions.IsNavigationStackEnabled = false;
            }
            if (args.IsSettingsSelected)
            {
                if (typeof(WidgetPage<,>).IsAssignableFrom(contentFrame.CurrentSourcePageType))
                {
                    
                }
                //contentFrame.Navigate(typeof(SampleSettingsPage));
            }
            else
            {
                var selectedItem = (MUXC.NavigationViewItem)args.SelectedItem;
                if (selectedItem != null)
                {
                    string selectedItemTag = ((string)selectedItem.Tag);
                    if (selectedItemTag == "Refresh")
                    {
                        Refresh();
                        var srcTag = mainWidgetNav.GetCurrentItemForFrame(contentFrame);
                        mainWidgetNav.SelectedItem = srcTag;
                        return;
                        selectedItemTag = (string)srcTag.Tag;
                    }
                    string pageName = "DustMother.App.Widgets." + selectedItemTag;
                    Type pageType = Type.GetType(pageName);
                    if (pageType != null)
                    {
                        contentFrame.NavigateToType(pageType, SaveData, navOptions);
                    }
                }
            }
        }
    }
}
