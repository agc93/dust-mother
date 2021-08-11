using DustMother.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace DustMother.App.Pages
{
    public class DetailsPage<TSave, TModel> : Page where TSave : WingmanSave where TModel : SaveViewModel<TSave>, new()
    {
        public DetailsPage()
        {
            
        }
        public TModel ViewModel { get; set; } = new TModel();
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await this.ViewModel?.CheckFileAccessAsync();
            await this.ViewModel?.Refresh();
        }
    }
}
