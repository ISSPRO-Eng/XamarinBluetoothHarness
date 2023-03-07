using Laerdal.Dfu.EventArgs;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.Views;

namespace XamarinAttributeProgrammer
{
    public partial class App : Application
    {
        public static GaugeAttributes AttributeManager = new GaugeAttributes();
        public static BLEConnections _BLEConnection;

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
