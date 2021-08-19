﻿using DustMother.App.Pages;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DustMother.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConquestPage : ConquestBasePage
    {
        public ConquestPage()
        {
            this.InitializeComponent();
        }

        private void SaveChangesBar_SaveCurrent(object sender, EventArgs e)
        {
            this.ViewModel?.WriteSave();
        }
    }
}
