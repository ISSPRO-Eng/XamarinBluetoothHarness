using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XamarinAttributeProgrammer.ViewModels;
using XamarinAttributeProgrammer.Views;

namespace XamarinAttributeProgrammer
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            SetNavBarIsVisible(this, false);
            Routing.RegisterRoute(nameof(ConnectPage), typeof(ConnectPage));
            Routing.RegisterRoute(nameof(ColorSelectionPage), typeof(ColorSelectionPage));
            Routing.RegisterRoute(nameof(AttributesPage), typeof(AttributesPage));
            Routing.RegisterRoute(nameof(OptionsPage), typeof(OptionsPage));
            Routing.RegisterRoute(nameof(DiagnosticPage), typeof(DiagnosticPage));
            Routing.RegisterRoute(nameof(DFUPage), typeof(DFUPage));
        }

    }
}
