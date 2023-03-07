using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinAttributeProgrammer.Resources
{
    public static class BTUartGattIDs
    {
        // https://infocenter.nordicsemi.com/index.jsp?topic=%2Fcom.nordic.infocenter.sdk5.v14.0.0%2Fble_sdk_app_nus_eval.html
        public static readonly Guid UartGattServiceId = Guid.Parse("6E400001-B5A3-F393-E0A9-E50E24DCCA9E");
        public static readonly Guid UartGattCharacteristicTXId = Guid.Parse("6E400002-B5A3-F393-E0A9-E50E24DCCA9E");  // Nordic rNF RX characteristic GUID
        public static readonly Guid UartGattCharacteristicRXId = Guid.Parse("6E400003-B5A3-F393-E0A9-E50E24DCCA9E"); // Nordic rNF TX characteristic GUID

        public static readonly Guid TX_POWER_UUID = Guid.Parse("00001804-0000-1000-8000-00805f9b34fb");
        public static readonly Guid TX_POWER_LEVEL_UUID = Guid.Parse("00002a07-0000-1000-8000-00805f9b34fb");
        public static readonly Guid CCCD = Guid.Parse("00002902-0000-1000-8000-00805f9b34fb");
        public static readonly Guid FIRMWARE_REVISON_UUID = Guid.Parse("00002a26-0000-1000-8000-00805f9b34fb");
        public static readonly Guid DIS_UUID = Guid.Parse("0000180a-0000-1000-8000-00805f9b34fb");
    }
}
