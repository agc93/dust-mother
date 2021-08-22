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
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DustMother.App.Controls
{
    public sealed partial class SaveChangesBar : UserControl
    {
        public SaveChangesBar()
        {
            this.InitializeComponent();
        }



        public bool PendingChanges
        {
            get { return (bool)GetValue(PendingChangesProperty); }
            set { SetValue(PendingChangesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PendingChanges.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PendingChangesProperty =
            DependencyProperty.Register("PendingChanges", typeof(bool), typeof(SaveChangesBar), new PropertyMetadata(false));

        public event EventHandler SaveCurrent;

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (SaveCurrent != null)
            {
                SaveCurrent(this, EventArgs.Empty);
            }
        }



        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SaveCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.Register("SaveCommand", typeof(ICommand), typeof(SaveChangesBar), new PropertyMetadata(null));
    }
}
