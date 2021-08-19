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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DustMother.App.Controls
{
    public sealed partial class StatisticCard : UserControl
    {
        public StatisticCard()
        {
            this.InitializeComponent();
        }



        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(StatisticCard), new PropertyMetadata("Value"));



        public int? Value
        {
            get { return (int?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int?), typeof(StatisticCard), new PropertyMetadata(0));



        //public int HeaderSize
        //{
        //    get { return (int)GetValue(HeaderSizeProperty); }
        //    set { SetValue(HeaderSizeProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for HeaderSize.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty HeaderSizeProperty =
        //    DependencyProperty.Register("HeaderSize", typeof(int), typeof(StatisticCard), new PropertyMetadata(36));



        //public int ValueSize
        //{
        //    get { return (int)GetValue(ValueSizeProperty); }
        //    set { SetValue(ValueSizeProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ValueSize.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ValueSizeProperty =
        //    DependencyProperty.Register("ValueSize", typeof(int), typeof(StatisticCard), new PropertyMetadata(80));



        public double ScaleFactor
        {
            get { return (double)GetValue(ScaleFactorProperty); }
            set { SetValue(ScaleFactorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleFactor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleFactorProperty =
            DependencyProperty.Register("ScaleFactor", typeof(double), typeof(StatisticCard), new PropertyMetadata((double)1.0));





    }
}
