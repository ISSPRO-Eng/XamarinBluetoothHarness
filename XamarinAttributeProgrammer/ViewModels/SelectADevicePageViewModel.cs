using XamarinAttributeProgrammer.Helpers;
using XamarinAttributeProgrammer.Models;

using Plugin.BluetoothLE;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace XamarinAttributeProgrammer.ViewModels
{
    public class SelectADevicePageViewModel : BindableObject
    {
        #region Singleton

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static SelectADevicePageViewModel Instance => _instance ?? (_instance = new SelectADevicePageViewModel());

        public static SelectADevicePageViewModel _instance;

        #endregion
        public ObservableCollection<CustomScanResult> ScanResults
        {
            get => GetValue<ObservableCollection<CustomScanResult>>();
            set => SetValue(value);
        }

        public CustomScanResult SelectedDevice
        {
            get => GetValue<CustomScanResult>();
            set
            {
                if (SetValue(value))
                {
                    if (value?.Device?.Uuid != null)
                    {
                        DfuInstallationConfigurationPageViewModel.Instance.DfuInstallation.DeviceId = NativeDeviceIdHelper.GetIdFromNativeDevice?.Invoke(value.Device.NativeDevice);
                    }
                }
            }
        }

        public SelectADevicePageViewModel()
        {
            ScanResults = new ObservableCollection<CustomScanResult>();
            CrossBleAdapter.Current.WhenStatusChanged()
                           .Subscribe(status =>
                            {
                                Debug.WriteLine($"Status : {status}");
                            });
            RequestPermissionAsync();
        }

        public void XamarinAttributeProgrammer()
        {
            if (!CrossBleAdapter.Current.IsScanning)
                CrossBleAdapter.Current.Scan().Subscribe(OnScanResultReceived);
        }

        public void OnScanResultReceived(IScanResult scanResult) 
        {
            var existing = ScanResults.SingleOrDefault(sr => sr.Device.Uuid.ToString() == scanResult.Device.Uuid.ToString());
            // Pull Device Name from BT Scan
            string deviceName = scanResult.Device.Name;

            if (deviceName != null && deviceName != "EV3_DfuTarg")
            {
                if (existing != null)
                    existing.UpdateFrom(scanResult);
                else
                {
                    existing = new CustomScanResult(scanResult);
                    ScanResults.Add(existing);
                    
                }
            }
            
        }

        public async void RequestPermissionAsync()
        {
            if (Xamarin.Forms.Device.RuntimePlatform != Xamarin.Forms.Device.Android)
                return;

            // Permission
            if (await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.LocationAlways>() != Xamarin.Essentials.PermissionStatus.Granted)
            {
                if (await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationAlways>() != Xamarin.Essentials.PermissionStatus.Granted)
                    throw new Exception($"Xamarin.Essentials.Permissions.LocationAlways was not granted");
            }
        }
    }
}