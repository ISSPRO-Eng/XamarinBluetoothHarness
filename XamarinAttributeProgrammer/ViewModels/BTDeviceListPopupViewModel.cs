using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using XamarinAttributeProgrammer.Models;

namespace XamarinAttributeProgrammer.ViewModels
{
    public class Device
    {
        IDevice _device;
        
        public IDevice DeviceInterface { get => _device; }
        public string Name { get => _device.Name; }
        public string Id { get => _device.Id.ToString(); }
        public string Rssi { get => _device.Rssi.ToString(); }
        public string Description { get => string.Format("RSSI: {1}\t\tGUID: {0}", Id, Rssi); }

        public Device(IDevice device)
        {
            _device = device;
        }
    }
    public class BTDeviceListPopupViewModel : BaseViewModel
    {
        private ObservableCollection<Device> _availableDevices = new ObservableCollection<Device>();
        private bool _isScanning;
        public ObservableCollection<Device> AvailableDevices { get => _availableDevices; 
                                                               private set { _availableDevices = value; }  }
        public bool isScanning
        {
            get => _isScanning;
            set
            {
                _isScanning = value;
                OnPropertyChanged();
            }
        }
        public string Cancel { get => Resourcer.Cancel; }
        public string Scan { get => Resourcer.getResStrVal("connectBtnTxt"); }
        public BTDeviceListPopupViewModel()
        {
                
        }
    }
}
