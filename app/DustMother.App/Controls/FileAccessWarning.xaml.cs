using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DustMother.App.Controls
{
    public sealed partial class FileAccessWarning : UserControl
    {
        public FileAccessWarning()
        {
            this.InitializeComponent();
        }

        public string FileAccessWarningDetail =>
            "Dust Mother couldn't read your game save file! Generally, this means that we don't have permission to open the file.";

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:appsfeatures-app"));
        }
    }
}
