using XamarinAttributeProgrammer.Helpers;
using XamarinAttributeProgrammer.ViewModels;
using XamarinAttributeProgrammer.Views;
using XamarinAttributeProgrammer.Popups;
using Plugin.BluetoothLE;
using Laerdal.Dfu.Enums;

using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using Rg.Plugins;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using Xamarin.Forms;
using Acr.Logging;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.Resources;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;

namespace XamarinAttributeProgrammer
{
    public partial class DFUPage : ContentPage
    {
        public DFUPage()
        {
            InitializeComponent();
        }

        private void StartButton_ClickedAsync(object sender, System.EventArgs e)
        {
            DFUPageViewModel.Instance.Start();
        }

        // Reset everything after leaving DFU page
        private async void BackBtn_ClickedAsync(object sender, System.EventArgs e)
        {
            SelectADevicePageViewModel._instance.SelectedDevice = null;
            DfuInstallationConfigurationPageViewModel._instance = new DfuInstallationConfigurationPageViewModel();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public async void DfuState(string dfuState)
        {
            if (dfuState == "Completed")
            {
                SelectADevicePageViewModel._instance.SelectedDevice = null;
                DfuInstallationConfigurationPageViewModel._instance = new DfuInstallationConfigurationPageViewModel();
                await Navigation.PushPopupAsync(new FirmwareSuccessPopupPage());
            }
        }

        public async void DfuError()
        {
            SelectADevicePageViewModel._instance.SelectedDevice = null;
            DfuInstallationConfigurationPageViewModel._instance = new DfuInstallationConfigurationPageViewModel();
            await Navigation.PushPopupAsync(new FirmwareErrorPopupPage());
        }
    }
}