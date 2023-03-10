using XamarinAttributeProgrammer.Helpers;

using Plugin.BluetoothLE;

using System;
using System.Collections.Concurrent;
using System.Linq;

namespace XamarinAttributeProgrammer.Models
{
    public class CustomScanResult : Helpers.BindableObject, IScanResult, IEquatable<CustomScanResult>
    {
        public string Name
        {
            get => GetValue("No name");
            private set
            {
                if (!string.IsNullOrEmpty(value))
                    SetValue(value);
            }
        }

        public string IdPlaceholder => GetValue(Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android ? "00:00:00:00:00:00" : Guid.Empty.ToString());

        public string Id
        {
            get => GetValue(IdPlaceholder);
            private set
            {
                if (!string.IsNullOrEmpty(value))
                    SetValue(value);
            }
        }

        public ManufacturerIdConstants Manufacturer
        {
            get => GetValue(ManufacturerIdConstants.None);
            private set
            {
                if (SetValue(value))
                {
                    ManufacturerString = value.GetDescription();
                }
            }
        }

        public string ManufacturerString
        {
            get => GetValue(ManufacturerIdConstants.None.GetDescription());
            private set => SetValue(value);
        }
        
        public CustomScanResult(IScanResult scanResult)
        {
            UpdateFrom(scanResult);
        }

        public void UpdateFrom(IScanResult scanResult)
        {
            UpdateFrom(scanResult.Device, scanResult.Rssi, scanResult.AdvertisementData);
        }

        public void UpdateFrom(IDevice device, int rssi, IAdvertisementData advertisementData)
        {
            Device = device;
            Rssi = rssi;
            AdvertisementData = advertisementData;
            Name = device.Name;
            Id = NativeDeviceIdHelper.GetIdFromNativeDevice?.Invoke(device.NativeDevice);
            Manufacturer = advertisementData.GetManufacturer();
        }

        public IDevice Device
        {
            get => GetValue<IDevice>();
            private set => SetValue(value);
        }

        public int Rssi
        {
            get => GetValue<int>();
            private set => SetValue(value);
        }

        public IAdvertisementData AdvertisementData
        {
            get => GetValue<IAdvertisementData>();
            private set => SetValue(value);
        }

        #region IEquatable

        public bool Equals(CustomScanResult other)
        {
            if (other?.Device?.Uuid == null)
                return false;
            return other.Device.Uuid.ToString() == Device.Uuid.ToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((CustomScanResult) obj);
        }

        public override int GetHashCode()
        {
            return Device.Uuid.GetHashCode();
        }

        public static bool operator ==(CustomScanResult left, CustomScanResult right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CustomScanResult left, CustomScanResult right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}