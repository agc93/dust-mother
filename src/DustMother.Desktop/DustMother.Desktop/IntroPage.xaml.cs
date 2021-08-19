using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DustMother.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IntroPage : Page
    {
        public IntroPage()
        {
            this.InitializeComponent();
            IntroHelpText = _helpText;
            WarningHelpText = _warningText;
        }

        public string IntroHelpText
        {
            get { return (string)GetValue(IntroHelpTextProperty); }
            set { SetValue(IntroHelpTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IntroHelpText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntroHelpTextProperty =
            DependencyProperty.Register("IntroHelpText", typeof(string), typeof(Shell), new PropertyMetadata(_helpText));



        public string WarningHelpText
        {
            get { return (string)GetValue(WarningHelpTextProperty); }
            set { SetValue(WarningHelpTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WarningHelpText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WarningHelpTextProperty =
            DependencyProperty.Register("WarningHelpText", typeof(string), typeof(Shell), new PropertyMetadata(_warningText));



        private static string _helpText = "Dust Mother can be used to open and view almost everything stored in your Project Wingman save file, including some content not available in-game. Use the navigation headers at the top of the window to view your statistics or data from your Campaign or Conquest progress.";

        private static string _warningText = "Anything involving your game save file obviously comes with some risk of damage, corruption or other issues, so we recommend backing up your save file!";
    }
}
