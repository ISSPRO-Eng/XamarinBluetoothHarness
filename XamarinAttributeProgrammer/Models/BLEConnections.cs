using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XamarinAttributeProgrammer.Resources;
using XamarinAttributeProgrammer.Views;
using XamarinEssentials = Xamarin.Essentials;

namespace XamarinAttributeProgrammer.Models
{
    public class BLEConnections
    {
        private readonly IAdapter _bluetoothAdapter;
        private ObservableCollection<IDevice> _gattDevices = new ObservableCollection<IDevice>();
        private static IService _service = null;
        private static ICharacteristic _TXcharacteristic = null;
        private static ICharacteristic _RXcharacteristic = null;
        private static IDescriptor _RXdescriptor = null;
        private static bool _isConnected = false;
        private static string _connectedDeviceName = "";
       

        public IAdapter BluetoothAdapter { get => _bluetoothAdapter; }
        public ObservableCollection<IDevice> GattDevices { get => _gattDevices; } // List of available to connect to.
        public IDevice LastConnectedDevice { get; set; }
        public ICharacteristic TXChannel { get => _TXcharacteristic; }
        public ICharacteristic RXChannel { get => _RXcharacteristic; }
        public IDescriptor RXDescriptor { get => _RXdescriptor; }
        public bool IsConnected 
        { 
            get => _isConnected; 
            set => _isConnected = value; 
        }
        public string DeviceName { get => _connectedDeviceName; set => _connectedDeviceName = value; }

        public BLEConnections()
        {
            _bluetoothAdapter = CrossBluetoothLE.Current.Adapter;
            _bluetoothAdapter.DeviceDiscovered += (sender, foundBleDevice) =>
            {
                if (foundBleDevice.Device != null && !string.IsNullOrEmpty(foundBleDevice.Device.Name))
                    _gattDevices.Add(foundBleDevice.Device);
            };
        }

        public bool CanWrite()
        {
            return _TXcharacteristic != null && _TXcharacteristic.CanWrite;
        }

        public async Task<bool> GetServiceAndCharacteristicAsync()
        {
            _service = await LastConnectedDevice.GetServiceAsync(BTUartGattIDs.UartGattServiceId);
            //var services = await LastConnectedDevice.GetServicesAsync();
            if (_service == null)
                return false;

            _TXcharacteristic = await _service.GetCharacteristicAsync(BTUartGattIDs.UartGattCharacteristicTXId);
            _RXcharacteristic = await _service.GetCharacteristicAsync(BTUartGattIDs.UartGattCharacteristicRXId);


            if (_RXcharacteristic != null)
            {

                await Task.Delay(15);
                _RXdescriptor = await _RXcharacteristic.GetDescriptorAsync(BTUartGattIDs.CCCD);// Needed for reading response
                // Add event handler for message recieved
                _RXcharacteristic.ValueUpdated += (o, args) =>
                {
                    var receivedBytes = args.Characteristic.Value;
                    string response = Encoding.UTF8.GetString(receivedBytes, 0, receivedBytes.Length);
                    DiagnosticPage.AddToLog("<< " + response);
                    Console.WriteLine("Response: " + response);
                    GaugeCommands.ProcessGaugeResponse(response);
                };
                await Task.Delay(15);

                await _RXcharacteristic.StartUpdatesAsync();
            }

            return _TXcharacteristic != null || _RXcharacteristic != null;
        }

        public bool GotWaysToComm()
        {
            if (_service == null) return false;
            if (TXChannel == null && RXChannel == null) return false;
            return true;
        }

        public async Task<bool> TryWriteAsync(byte[] cmd)
        {
            try
            {
                if ((_TXcharacteristic != null && _TXcharacteristic.CanWrite) ||
                (_TXcharacteristic == null && GotWaysToComm()))
                {
                    var result = await _TXcharacteristic.WriteAsync(cmd);
                    DiagnosticPage.AddToLog(">> " + Encoding.UTF8.GetString(cmd));
                    return result;
                }
                else throw new AccessViolationException("TXChannel does not have write access.");
            }
            catch (CharacteristicReadException rex) // we don't care about this exception.
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
