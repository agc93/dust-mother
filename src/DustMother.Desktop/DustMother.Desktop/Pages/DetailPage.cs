using DustMother.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace DustMother.App.Pages
{
    public class DetailsPage<TSave, TModel> : Page where TSave : WingmanSave where TModel : SaveViewModel<TSave>, new()
    {
        public DetailsPage()
        {
            
        }
        public TModel ViewModel { get; set; } = new TModel();

        protected async void OnNavigatedRefresh()
        {
            await this.ViewModel?.CheckFileAccessAsync();
            await this.ViewModel?.Refresh();
        }
    }
}
