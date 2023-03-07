using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using XamarinAttributeProgrammer.ViewModels;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.Resources;

/// <summary>
/// Author: Tam Nguyen
/// Self explainatory...
/// </summary>
namespace XamarinAttributeProgrammer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OptionsPage : ContentPage
    {
        string _storeLink = "https://isspro.com/category_gs.php?cid=102";
        string _contactLink = "https://isspro.com/page_Contact_Us.php";
        string _privacyPolicyLink = "https://www.isspro.com/page_Privacy.php"; /// Added ISSPRO privacy policy for Google Play Store
        string _instructionLink = "https://issuu.com/isspro/docs/attribute_programmer_user_instructions";
        /// </summary>
        OptionsViewModel vm;
        public OptionsPage()
        {
            InitializeComponent();
            BindingContext = vm = new OptionsViewModel();
        }

        protected override void OnAppearing()
        {
            clearBtn.IsVisible = App._BLEConnection.IsConnected;
            DiagnosticPage.AddToLog("I: Entering Option Page");
            base.OnAppearing();
        }
        
        protected override async void OnDisappearing()
        {
            await Application.Current.MainPage.Navigation.PopAsync(); //Remove the page currently on top.
            base.OnDisappearing();
        }

        private async void StoreBtn_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                DiagnosticPage.AddToLog($"I: Redirect user to store page ({_storeLink})");
                await Browser.OpenAsync(new Uri(_storeLink), BrowserLaunchMode.External);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        private async void ContactBtn_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                {
                    DiagnosticPage.AddToLog($"I: Redirect user to contact page ({_contactLink})");
                    await Browser.OpenAsync(new Uri(_contactLink), BrowserLaunchMode.SystemPreferred);
                }
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        private async void DiagnosticBtn_ClickedAsync(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("DiagnosticPage");
        }

        private async void PrivacyPolicyBtn_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                {
                    DiagnosticPage.AddToLog($"I: Redirect user to privacy policy ({_privacyPolicyLink})");
                    await Browser.OpenAsync(new Uri(_privacyPolicyLink), BrowserLaunchMode.SystemPreferred);
                }
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        private async void InstructionBtn_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                {
                    DiagnosticPage.AddToLog($"I: Redirect user to instructions ({_instructionLink})");
                    await Browser.OpenAsync(new Uri(_instructionLink), BrowserLaunchMode.SystemPreferred);
                }
            }
            catch (Exception ex)
            {
                DiagnosticPage.AddToLog("INST: " + ex.Message);
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        private async void FirmwareBtn_ClickedAsync(object sender, EventArgs e)
        {
                // check for permissions
                if (await PermissionsGrantedAsync())
                {
                    await Shell.Current.GoToAsync("DFUPage");
                    DiagnosticPage.AddToLog($"I: Redirect user to DFU OTA Page");
                }
                else
                {
                    // Can't access bt
                    await DisplayAlert(Resourcer.getResStrVal("permissionTitle"),
                                            Resourcer.getResStrVal("permissionLocation"),
                                            Resourcer.Ok);
            }
        }

        private async Task<bool> PermissionsGrantedAsync()
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS ||
                DeviceInfo.Platform == DevicePlatform.macOS)
            {
                // I don't think iOS needs location like android does.
                return true;
            }

            var locationAlways = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
            if (locationAlways != PermissionStatus.Granted)
            {
                await DisplayAlert(Resourcer.getResStrVal("permissionTitle"),
                                        Resourcer.getResStrVal("permissionReqPrior"),
                                        Resourcer.Ok);
                var status = await Permissions.RequestAsync<Permissions.LocationAlways>();
                if (status == PermissionStatus.Granted)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private async void ClearBtn_ClickedAsync(object sender, EventArgs e)
        {
            var connection = App._BLEConnection;

            if (!connection.IsConnected)
            {
                clearBtn.IsVisible = App._BLEConnection.IsConnected;
                return;
            }

            bool ans = await DisplayAlert(Resourcer.Warning, 
                                        Resourcer.getResStrVal("clearAllAttWarning"), 
                                        Resourcer.Ok, Resourcer.Cancel);

            DiagnosticPage.AddToLog("I: Requesting factory reset!!");
            if (ans)
            {
                try
                {
                    byte[] reset = GaugeCommands.FACT_RESET;
                    await connection.TryWriteAsync(reset);
                }
                catch (Exception ex)
                {
                    DiagnosticPage.AddToLog("ECBC: " + ex.Message);
                }
                
            }
        }

    }
}