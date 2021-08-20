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

namespace DustMother.App.Controls
{
    public sealed partial class NumberInput : UserControl
    {
        public NumberInput()
        {
            this.InitializeComponent();
        }



        public int? Value
        {
            get { return (int?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumberInput), new PropertyMetadata(null));



        public bool LargeNumber
        {
            get { return (bool)GetValue(LargeNumberProperty); }
            set { SetValue(LargeNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LargeNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LargeNumberProperty =
            DependencyProperty.Register("LargeNumber", typeof(bool), typeof(NumberInput), new PropertyMetadata(false));




        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(NumberInput), new PropertyMetadata(null));




    }
}
