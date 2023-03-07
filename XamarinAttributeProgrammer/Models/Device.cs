using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XamarinAttributeProgrammer.Models
{
    public class Device
    {
        private Guid _id;
        public string Id { get => _id.ToString();
            set
            {
                _id = Guid.Parse(value);
            }
        }

        public string Name { get; set; }

        public int Rssi { get; set; }
        public IDevice DeviceInterface { get; set; }

        public Device(IDevice device)
        {
            DeviceInterface = device;
            _id = device.Id;
            Name = device.Name ?? "N/A";
            Rssi = device.Rssi;
        }
    }
}
