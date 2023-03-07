using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.Resources;
using XamarinAttributeProgrammer.ViewModels;
using Device = XamarinAttributeProgrammer.ViewModels.Device;

/// <summary>
/// Author: Tam Nguyen
/// This class is the popup after clicking "Scan". It will show all the available device
/// ready to be connect to. This does not restrict to only ISSPRO gauges, but will only connect
/// to ISSPRO gauges, or rather gauges that uses nordic's BLE chip
/// To access: Connect -> Connect button
/// </summary>
namespace XamarinAttributeProgrammer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BTDeviceListPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private TaskCompletionSource<string> taskCompletionSource;
        public Task<string> PopupClosedTask { get { return taskCompletionSource.Task; } }
        BTDeviceListPopupViewModel _vm;
        ObservableCollection<Device> AvailableDevices;
        BLEConnections _BLEconnection;
        IAdapter _BTAdapter;

        public bool isBusy = false;
        public bool isConnected = false;
        public BTDeviceListPopup(BLEConnections connection)
        {
            InitializeComponent();
            taskCompletionSource = new TaskCompletionSource<string>();
            BindingContext = _vm = new BTDeviceListPopupViewModel(); // Binding this view to the viewmodel for all bindings on the xaml
            _BLEconnection = connection;
            _BTAdapter = _BLEconnection.BluetoothAdapter;
            AvailableDevices = _vm.AvailableDevices;
            isBusy = _vm.IsBusy = false;

            //this.Disappearing += (sender, e) => { this.OnDisappearing(); };

            // Not awaiting since we want it to run in the background
            _ = InitScanAsync();
        }

        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        /// <summary>
        /// Function that calls the refresh/rescan feature of getting the devices.
        /// I have implemented to use semaphore to ONLY allow scan if there is not a
        /// scan in progress.
        /// </summary>
        /// <returns></returns>
        private async Task InitScanAsync()
        {
            if (semaphoreSlim.CurrentCount == 0) return; // there is currently a scan in progress
            await semaphoreSlim.WaitAsync(); // Else, lock up the semaphore

            ScanBtn.IsEnabled = false; // Binding does not work for popup....

            ScanBtn.Text = Resourcer.getResStrVal("scanning");
            DeviceListView.EmptyView = Resourcer.getResStrVal("scanning") + "..";
            try
            {

                // This is will add a new discovered device everytime one is found. Performs Async
                _BTAdapter.DeviceDiscovered += (s, a) =>
                {
                    IDevice device = a.Device;
                    Device deviceObj = new Device(device);

                    // Only add if there is a name and not already on the list
                    bool ignore = false;
                    if (!(ignore = string.IsNullOrEmpty(device.Name)))
                    {
                        ignore = AvailableDevices.AsEnumerable() // In our Enumerable (list)
                                                 .Where(x => x.Id == deviceObj.Id) // see if there is device with a matching ID
                                                 .Count() > 0; // if there are,
                    }
                    if (!ignore)
                    {
                        _vm.AvailableDevices.Add(deviceObj);
                        Console.WriteLine(device.Name);
                    }
                };

                await _BTAdapter.StartScanningForDevicesAsync();

            }
            catch (Exception e)
            {
                DiagnosticPage.AddToLog("EIS: " + e.Message);
                Console.WriteLine($"Error in InitScan(): {e.Message}");
            }
            finally
            {
                // set the display text if there was no devices
                DeviceListView.EmptyView = Resourcer.getResStrVal("nodevices");
                // Open this semaphore to allow rescan
                semaphoreSlim.Release();
                if (!ScanBtn.IsEnabled)
                {
                    ScanBtn.IsEnabled = true;
                    ScanBtn.Text = Resourcer.getResStrVal("connectBtnTxt");
                    ScanBtn.BackgroundColor = Color.FromHex("2196F3");
                }
            }
        }
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            taskCompletionSource.SetResult("Cancel");
            await PopupNavigation.Instance.PopAllAsync();
        }

        private async void Rescan_ClickedAsync(object sender, EventArgs e)
        {
            _vm.IsBusy = true;
            if (!_BTAdapter.IsScanning)
                await InitScanAsync();
            _vm.IsBusy = false;
        }

        // Event when user selects a device from the list
        private async void SelectedDevice_TappedAsync(object sender, SelectionChangedEventArgs e)
        {
            //TODO add busy indicator
            isBusy = true;
            IDevice selectedDev = (e.CurrentSelection.FirstOrDefault() as Device)?.DeviceInterface;
            await _BTAdapter.StopScanningForDevicesAsync();

            // IF we're already connected to this device
            if (selectedDev.State == Plugin.BLE.Abstractions.DeviceState.Connected)
            {
                _BLEconnection.LastConnectedDevice = selectedDev;
                //Dismiss("Connected"); // close out
                await PopupNavigation.Instance.PopAllAsync();
            }
            else
            {
                try
                {
                    var conParams = new ConnectParameters(false, true); // we don't want it to auto reconnect (false)
                    await _BTAdapter.ConnectToDeviceAsync(selectedDev, conParams);
                    _BLEconnection.LastConnectedDevice = selectedDev;

                    bool result = await _BLEconnection.GetServiceAndCharacteristicAsync();

                    if (!result)
                    {
                        Console.WriteLine("Can't get service and characteristics");
                        DiagnosticPage.AddToLog("ESD: Can't get service and characteristics");
                    }

                    taskCompletionSource.SetResult(result ? "Connected" : "Failed");
                    await PopupNavigation.Instance.PopAllAsync();
                }
                catch (Exception ex)
                {
                    DiagnosticPage.AddToLog("ESD: "+ex.Message);
                    Console.WriteLine($"Error in SelectedDevice_Tapped(): {ex.Message}");
                }
            }
        }

    }
}