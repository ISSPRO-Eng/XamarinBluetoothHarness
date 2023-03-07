using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.Resources;
using XamarinAttributeProgrammer.ViewModels;

/// <summary>
/// Author: Tam Nguyen
/// The landing page and also the page to allow dis/connect of the BT devices.
/// This page also allow the user to change the gauge's name and see the device
/// model, sn, pcb type...
/// </summary>
namespace XamarinAttributeProgrammer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectPage : ContentPage
    {
        ConnectViewModel vm; // View Model
        BLEConnections _connection;
        bool first = true; // Only show permission message when first opening app
        public ConnectPage()
        {
            InitializeComponent();
            _connection = App._BLEConnection;

            // Grab the connection obj from parent
            if (_connection == null)
                _connection = App._BLEConnection = new BLEConnections();
            
            BindingContext = vm = new ConnectViewModel(); // Binding this view to the viewmodel for all bindings on the xaml
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            DiagnosticPage.AddToLog("I: Entering Connection page.");


            var locationDuring = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            var locationAlways = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
            if (locationDuring != PermissionStatus.Granted && locationAlways != PermissionStatus.Granted && first == true)
            {
                await DisplayAlert(Resourcer.getResStrVal("permissionTitle"),
                    ("EV3 Attribute Programmer must constantly use your location data in the background to continually" +
                     " search for new devices to connect to. This must be enabled for the functionality of the application. This feature in being" +
                     " used even when the app is closed or not in use."), Resourcer.Ok);
                await DisplayAlert(Resourcer.getResStrVal("alert"),
                    ("Instructions can be found on the extras page tab."), Resourcer.Ok);
                first = false;
            }
            
        }

        // Check for access to the BT, by ensuring location is enable and granted
        private async Task<bool> PermissionsGrantedAsync()
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS ||
                DeviceInfo.Platform == DevicePlatform.macOS)
            {
                // I don't think iOS needs location like android does.
                return true;
            }

            var locationDuring = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            var locationAlways = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
            if (locationDuring != PermissionStatus.Granted && locationAlways != PermissionStatus.Granted)
            {
                await DisplayAlert(Resourcer.getResStrVal("permissionTitle"),
                                        Resourcer.getResStrVal("permissionReqPrior"),
                                        Resourcer.Ok);
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                return status == PermissionStatus.Granted;
            }
            return true;
        }


        // Function to send and request the gauge's info
        private async Task<string> RetrieveGaugeDescriptionAsync()
        {
            byte[][] cmds = GaugeCommands.ProductInfoCmd();
            string gaugeDescription = Resourcer.getResStrVal("connectedInfoMsg");
            var gauge = App.AttributeManager;

            try
            {
                ICharacteristic tx = _connection.TXChannel; // for sending data to the gauge
                ICharacteristic rx = _connection.RXChannel; // for receiving data from gauge

                if (tx == null || rx == null)
                {
                    string msg = Resourcer.getResStrVal("noTXRXChar"); // Don't need to add this to stringValues resx since user wont see it
                    throw new Exception(msg);
                }
                if (!tx.CanWrite)
                {
                    await DisplayAlert(Resourcer.Error,
                                        Resourcer.getResStrVal("noWriteAccess"),
                                        Resourcer.Ok);

                    throw new Exception(Resourcer.getResStrVal("noWriteAccess"));
                }


                foreach (var cmd in cmds)
                {
                    var result = await _connection.TryWriteAsync(cmd);
                    Console.WriteLine(result ? "Msg sent":"Msg failed to send");
                }

                // We do not need to process the response here. That's already been setup via event handler in BLEConnections.GetServiceAndCharacteristic()

                // Request for all the gauge's info, like color and attributes
                var r = await _connection.TryWriteAsync(GaugeCommands.ATTRS);
            }
            catch (Exception ex)
            {
                DiagnosticPage.AddToLog("ERGDA: " + ex.Message);
                Console.WriteLine("Error: " + ex);
            }

            DeviceNameEntry.Text = _connection.LastConnectedDevice.Name;
            _connection.DeviceName = DeviceNameEntry.Text;
               
            return string.Format(gaugeDescription,  gauge.getDevice_SN() ?? Resourcer.getResStrVal("nullValue"), 
                                                    gauge.getDeviceVersion() ?? Resourcer.getResStrVal("nullValue"), 
                                                    gauge.getDevice_PCB_Type()) ?? Resourcer.getResStrVal("nullValue");
        }

        /// Function where the popup of the device list is called.
        private async Task<bool> StartBLESearchAsync()
        {
            try
            {
                _connection = new BLEConnections();
                var popup = new BTDeviceListPopup(_connection);
                await PopupNavigation.Instance.PushAsync(popup); // Show popup
                var result = await popup.PopupClosedTask; // halt until user closes the popup

                if (result.ToString().Equals("Connected"))
                {
                    
                    _connection.BluetoothAdapter.DeviceConnectionLost += OnDeviceConnectionLost; // Assign a event trigger when connection is lost
                    await DisplayAlert(Resourcer.Success,
                                        Resourcer.getResStrVal("connectedMsg"),
                                        Resourcer.Ok);
                    
                    App.AttributeManager.setGUID(_connection.LastConnectedDevice.Id);
                    return true;
                }
                else if (result.ToString().Equals("Failed")) 
                {
                    await DisplayAlert(Resourcer.Error,
                                        Resourcer.getResStrVal("connectionFailed"),
                                        Resourcer.Ok);
                    return false;
                }
                

            } catch (Exception e)
            {
                DiagnosticPage.AddToLog("ESBLESA: " + e.Message);
                Console.WriteLine($"Error in StartBLESearch(): {e.Message}");
            }
            return false;
        }

        // Function that process the dis/connect button 
        private async void ConnectDisconnectBtn_ClickedAsync(object sender, EventArgs e)
        {
            mainBtn.IsVisible = false;
            if (vm.isConnected)
            {
                // If we're connected to a device, then disconnect it
                var connectedDevice = _connection.LastConnectedDevice;
                if (connectedDevice != null)
                    await _connection.BluetoothAdapter.DisconnectDeviceAsync(connectedDevice);
                vm.isConnected = _connection.IsConnected = false;
            }
            else
            {
                if (await PermissionsGrantedAsync()) // Check if we can access the BT
                {

                    bool connected = await StartBLESearchAsync();
                    if (connected)
                    {
                        DiagnosticPage.AddToLog($"I: Established connection with device {_connection.LastConnectedDevice.Name}!");
                        if (_connection.GotWaysToComm()) // Just to ensure, but it should have already checked after the user tap the device from list
                        {
                            string deviceInfo = await RetrieveGaugeDescriptionAsync();
                            vm.ConnectionInfoString = deviceInfo;
                            vm.isConnected = true;
                            _connection.IsConnected = true;
                            DiagnosticPage.AddToLog("I: Established communication with device!");
                            // flash warning light five times in five seconds to show connected device
                            byte[] cmd = null;
                            cmd = GaugeCommands.WARN_TOGGLE;
                            if (cmd != null)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    await _connection.TryWriteAsync(cmd);
                                    await Task.Delay(500);
                                    await _connection.TryWriteAsync(cmd);
                                    await Task.Delay(500);
                                }
                            }
                        }
                        else
                        {
                            vm.IsBusy = false;
                            DiagnosticPage.AddToLog("E: Failed to stablish communication with device!");
                            await DisplayAlert(Resourcer.Error,
                                                Resourcer.getResStrVal("connectionFailed"),
                                                Resourcer.Ok);
                            vm.isConnected = false;
                        }
                    }
                    else
                        vm.isConnected = _connection.IsConnected = connected;
                }
                else
                {
                    DiagnosticPage.AddToLog("E: Fail to connect with device!");
                    // Can't access bt
                    await DisplayAlert(Resourcer.getResStrVal("permissionTitle"),
                                        Resourcer.getResStrVal("permissionReqMsg"),
                                        Resourcer.Ok);
                }
            }
            mainBtn.IsVisible = true;
        }

        static SemaphoreSlim disconnectPopupSemaphore = new SemaphoreSlim(1);
        // Event handling function for when a device drops connection to the app
        private void OnDeviceConnectionLost(object sender, DeviceErrorEventArgs e)
        {
            DiagnosticPage.AddToLog("E: BLE connection dropped unexpectedly");

            // Need to perform the logic on the main thread else the app will crash
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                if (disconnectPopupSemaphore.CurrentCount == 0)
                    return;

                await disconnectPopupSemaphore.WaitAsync(); // Wait here until locks opens
                await Shell.Current.GoToAsync("//ConnectPage"); // Go back to the connection page
                await DisplayAlert(Resourcer.Error,
                                    Resourcer.getResStrVal("connectionLost"),
                                    Resourcer.Ok);
                disconnectPopupSemaphore.Release();
            });
            vm.isConnected = _connection.IsConnected = false;
        }

        private async void UpdateNameBtn_ClickedAsync(object sender, EventArgs e)
        {
            Button sndr = sender as Button;
            string newName = DeviceNameEntry.Text;
            sndr.IsEnabled = false;

            App.AttributeManager.setDeviceName(newName);

            try
            {
                byte[] cmd = GaugeCommands.ReqNameChange(newName);
                await _connection.TryWriteAsync(cmd);
                await DisplayAlert(Resourcer.Success,
                                    Resourcer.getResStrVal("deviceRenamed"),
                                    Resourcer.Ok);
            }
            catch (Exception ex)
            {
                DiagnosticPage.AddToLog("EUNB: " + ex.Message);
                Console.WriteLine("Error: " + ex.Message);
            }

            sndr.IsEnabled = true;
        }
    }
}