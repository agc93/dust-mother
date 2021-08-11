using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DustMother.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel { get; set; } = new MainPageViewModel();
        public MainPage()
        {
            this.InitializeComponent();
            IntroHelpText = MainPage._helpText;
            WarningHelpText = MainPage._warningText;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await this.ViewModel?.CheckFileAccessAsync();
            if (ViewModel?.FileReadable == true)
            {
                await this.ViewModel?.Refresh();
            }
        }

        private async void NavigationView_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            FrameNavigationOptions navOptions = new FrameNavigationOptions
            {
                TransitionInfoOverride = args.RecommendedNavigationTransitionInfo
            };
            //if (sender.PaneDisplayMode == MUXC.NavigationViewPaneDisplayMode.Top)
            //{
            //    navOptions.IsNavigationStackEnabled = false;
            //}
            if (args.IsSettingsSelected)
            {
                //contentFrame.Navigate(typeof(SampleSettingsPage));
                throw new NotImplementedException();
            }
            else
            {
                var selectedItem = (Microsoft.UI.Xaml.Controls.NavigationViewItem)args.SelectedItem;
                if (selectedItem != null)
                {
                    string selectedItemTag = ((string)selectedItem.Tag);
                    if (selectedItemTag.StartsWith("_Backup"))
                    {
                        //var saveType = selectedItemTag.Split(':').Last();
                        //await ViewModel.BackupSave(saveType);
                        selectedItemTag = (string)(sender.GetCurrentItemForFrame(contentFrame)?.Tag);
                    }
                    string pageName = "DustMother.App." + selectedItemTag;
                    Type pageType = Type.GetType(pageName);
                    if (pageType != null)
                    {
                        contentFrame.Navigate(pageType);
                        //contentFrame.NavigateToType(pageType, ViewModel.SaveData, navOptions);
                    }
                }
            }
        }



        public string IntroHelpText
        {
            get { return (string)GetValue(IntroHelpTextProperty); }
            set { SetValue(IntroHelpTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IntroHelpText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntroHelpTextProperty =
            DependencyProperty.Register("IntroHelpText", typeof(string), typeof(MainPage), new PropertyMetadata(_helpText));



        public string WarningHelpText
        {
            get { return (string)GetValue(WarningHelpTextProperty); }
            set { SetValue(WarningHelpTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WarningHelpText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WarningHelpTextProperty =
            DependencyProperty.Register("WarningHelpText", typeof(string), typeof(MainPage), new PropertyMetadata(_warningText));



        private static string _helpText = "Dust Mother can be used to open and view almost everything stored in your Project Wingman save file, including some content not available in-game. Use the navigation headers at the top of the window to view your statistics or data from your Campaign or Conquest progress.";

        private static string _warningText = "Anything involving your game save file obviously comes with some risk of damage, corruption or other issues, so we recommend backing up your save file!";


    }
}
