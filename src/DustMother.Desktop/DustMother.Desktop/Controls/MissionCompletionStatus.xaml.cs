using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using DustMother.Core;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DustMother.App.Controls
{
    public sealed partial class MissionCompletionStatus : UserControl
    {
        public MissionCompletionStatus()
        {
            this.InitializeComponent();
        }



        public MissionCompletion Mission
        {
            get { return (MissionCompletion)GetValue(MissionProperty); }
            set { SetValue(MissionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mission.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MissionProperty =
            DependencyProperty.Register("Mission", typeof(MissionCompletion), typeof(MissionCompletionStatus), new PropertyMetadata(null));


    }
}
