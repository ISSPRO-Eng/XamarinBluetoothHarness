using Plugin.BluetoothLE;
using Plugin.BluetoothLE.Server;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
namespace XamarinAttributeProgrammer.Helpers
{
    public static class AdvertisementDataExtensions
    {
        public static int GetManufacturerId(this IAdvertisementData advertisementData)
        {
            if (advertisementData?.ManufacturerData == null || advertisementData.ManufacturerData.Length < 2)
            {
                return -1;
            }

            return ((advertisementData.ManufacturerData.GetOrDefault(1) & 0xFF) << 8) + (advertisementData.ManufacturerData.GetOrDefault(0) & 0xFF);
        }

        public static ManufacturerIdConstants GetManufacturer(this IAdvertisementData advertisementData)
        {
            return (ManufacturerIdConstants)advertisementData.GetManufacturerId();
        }
        //public static int GetManufacturerId(this IAdapter adapter)
        //{
        //    if(adapter.GetManufacturerId()==null || adapter.GetManufacturerId()<0)
        //    {
        //        return -1;
        //    }
        //    return ((adapter.GetManufacturerId() & 0xFF) << 8) + (adapter.GetManufacturerId() & 0xFF);

        //}
        //public static ManufacturerIdConstants GetManufacturer(this IAdapter advertisementData)
        //{
        //    return (ManufacturerIdConstants)advertisementData.GetManufacturerId();
        //}
    }
}