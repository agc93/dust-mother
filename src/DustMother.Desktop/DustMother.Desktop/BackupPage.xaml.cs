using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DustMother.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BackupPage : Page
    {
        public BackupPage()
        {
            this.InitializeComponent();
        }

        public BackupPageViewModel ViewModel { get; set; } = new BackupPageViewModel();
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.IsRunning = true;
                await ViewModel.GetAvailableSaves();
                ViewModel.IsRunning = false;
            }
            //ViewModel?.IsRunning = true;
            
            //var navParam = e.Parameter?.ToString();
            //if (!string.IsNullOrWhiteSpace(navParam))
            //{
            //    await ViewModel.BackupSave(navParam);
                
            //}
        }

        private void Backup_All(object sender, RoutedEventArgs e)
        {
            ViewModel?.BackupSave("*");
        }

        private void Backup(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                ViewModel?.BackupSave(btn.Content.ToString());
            }
        }

        private void OpenSaveFolder(object sender, RoutedEventArgs e)
        {
            ViewModel?.OpenSaveFolder();
        }
    }
}
