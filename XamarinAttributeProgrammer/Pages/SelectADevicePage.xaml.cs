using XamarinAttributeProgrammer.Helpers;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.ViewModels;

using Plugin.BluetoothLE;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinAttributeProgrammer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectADevicePage : ContentPage
    {
        public SelectADevicePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SelectADevicePageViewModel.Instance.XamarinAttributeProgrammer();
        }

        protected override void OnDisappearing()
        {
            // Clear device list on disappearing
            SelectADevicePageViewModel.Instance.ScanResults.Clear();
            base.OnDisappearing();
        }

        private void DevicesListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Navigation.PopAsync();
        }

        private void BackBtn_ClickedAsync(object sender, System.EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}