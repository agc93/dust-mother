using DustMother.Core;
using DustMother.App.Pages;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DustMother.App.Widgets
{
    public class CampaignWidgetPage : WidgetPage<CampaignSave, CampaignWidgetViewModel>
    {
        public CampaignWidgetPage()
        {
            ViewModel.SetContext(this);
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CampaignWidget : CampaignWidgetPage
    {
        public CampaignWidget()
        {
            this.InitializeComponent();
        }
    }
    
}
