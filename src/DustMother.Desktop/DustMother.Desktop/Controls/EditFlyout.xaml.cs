using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
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
    [ContentProperty(Name = "BodyContent")]
    public sealed partial class EditFlyout : UserControl
    {
        public EditFlyout()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets additional content for the UserControl
        /// </summary>
        public object BodyContent
        {
            get { return (object)GetValue(BodyContentProperty); }
            set { SetValue(BodyContentProperty, value); }
        }
        public static readonly DependencyProperty BodyContentProperty =
            DependencyProperty.Register("BodyContent", typeof(object), typeof(EditFlyout),
              new PropertyMetadata(null));
    }
}
