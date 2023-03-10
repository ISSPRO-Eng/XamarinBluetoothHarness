using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using XamarinAttributeProgrammer.Models;

namespace XamarinAttributeProgrammer.ViewModels
{
    public class AttributesViewModel : BaseViewModel
    {
        public enum Intensity
        {
            LOW,
            MEDIUM,
            HIGH
        }

        bool _hasChanges = false;
        bool _speedoTach = false;

        /// <summary>
        // LABELS
        /// </summary>
        /// 
        public string gaugeType         { get => Resourcer.getResStrVal("gaugeType"); }
        public string gaugeMode         { get => Resourcer.getResStrVal("gaugeMode"); }
        public string voltHi            { get => Resourcer.getResStrVal("dimVoltHi"); }
        public string voltLo            { get => Resourcer.getResStrVal("dimVoltLo"); }
        public string warnThres     { get => Resourcer.getResStrVal("warnThres"); }
        public string outputThres    { get => Resourcer.getResStrVal("outputThres"); }
        public string ptrW8             { get => Resourcer.getResStrVal("ptrW8"); }
        public string sensHys           { get => Resourcer.getResStrVal("sensHys"); }
        public string sensCoeff0        { get => Resourcer.getResStrVal("sensCoeff0"); }
        public string sensCoeff1        { get => Resourcer.getResStrVal("sensCoeff1"); }
        public string speedoPPM         { get => Resourcer.getResStrVal("speedoPPM"); }
        public string tachPPR           { get => Resourcer.getResStrVal("tachPPR"); }
        public string enableAccumulation   { get => Resourcer.getResStrVal("enableAccumulation"); }
        public string distanceUnits     { get => Resourcer.getResStrVal("distanceUnits"); }
        public string sensorEnabled     { get => Resourcer.getResStrVal("enableSensor"); }
        public string warningSec        { get => Resourcer.Warning; }
        public string driverSec         { get => Resourcer.getResStrVal("driver"); }
        public string advanceSec        { get => Resourcer.getResStrVal("advance"); }
        public string speedtachSec      { get => Resourcer.getResStrVal("speedtachSec"); }
        public string saveBtn           { get => Resourcer.getResStrVal("save"); }
        public string resetBtn         { get =>  Resourcer.getResStrVal("reset"); }
        public string cancelBtn         { get => !_hasChanges ? Resourcer.getResStrVal("reload") : Resourcer.Cancel; }
        public string indicationZone { get => Resourcer.getResStrVal("indicationZone"); }
        public string expandLbl         { get => Resourcer.getResStrVal("expand"); }
        public string minimizeLbl       { get => Resourcer.getResStrVal("mini"); }
        public string backlight         { get => Resourcer.getResStrVal("backlight"); }
        public bool Haschanges
        {
            get => _hasChanges;
            set
            {
                _hasChanges = value;
                OnPropertyChanged();
                OnPropertyChanged(cancelBtn);
            }
        }
        public bool Speedotach
        {
            get => _speedoTach;
            set
            {
                _speedoTach = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// VALUES and ITEMSOURCE
        /// </summary>
        public IList<string> sensorType { get => new List<string> { "Analog", "J1939"}; }
        public IList<string> quadSpeedoTach { get => new List<string> { "Tachometer", "Speedometer" }; }
        public IList<string> quadBoostPyro { get => new List<string> { "Boost", "Pyrometer" }; }
        public IList<string> quadDualPyro { get => new List<string> { "Pryometer", "Pyrometer" }; }
        public IList<string> quadDefFuel { get => new List<string> { "DEF Level", "Fuel Level" }; }
        public IList<string> quadTempFuel { get => new List<string> { "Temp", "Fuel Level" }; }
        public IList<string> quadFour { get => new List<string> { "Temp", "Oil Pressure","Fuel","Voltage" }; }

        public IList<string> zones { get => new List<string> { "LOW", "HIGH" }; }
        public IList<string> enable { get => new List<string> { "DISABLED", "ENABLED" }; }
        public IList<string> units { get => new List<string> { "MILES", "KILOMETERS" }; }
        public IList<string> accumType { get => new List<string> { "HOURMETER", "ODOMETER"}; }
        public IList<string> coeffSlot { get => new List<string> { "0", "1" }; }
        public IList<string> motorWeight { get => new List<string> { "FASTEST", "FAST","SLOW", "SLOWEST" }; }

        public IList<KeyValuePair<string, int>> GaugeType
        {
            // ENSURE this list MATCHES GaugeType enum's order from the xaml.cs file!!!!!
            get => new List<KeyValuePair<string, int>> {
            new KeyValuePair<string, int>("4k Tach & 80 mph Speedo", 255),
            new KeyValuePair<string, int>("4k Tach & 120 mph Speedo", 255),
            new KeyValuePair<string, int>("6k Tach & 120 mph Speedo", 255),
            new KeyValuePair<string, int>("8k Tach & 120 mph Speedo", 255),
            new KeyValuePair<string, int>("8k Tach & 160 mph Speedo", 255),
            new KeyValuePair<string, int>("3k Tach & 200 km Speedo", 255),
            new KeyValuePair<string, int>("3k Tach & 200 km Speedo", 255),
            new KeyValuePair<string, int>("6k Tach & 200 km Speedo", 255),
            new KeyValuePair<string, int>("8k Tach & 200 km Speedo", 255),
            new KeyValuePair<string, int>("30 psi Boost & 1600 F Pyro", 255),
            new KeyValuePair<string, int>("40 psi Boost & 1600 F Pyro", 255),
            new KeyValuePair<string, int>("60 psi Boost & 2000 F Pyro", 255),
            new KeyValuePair<string, int>("2.0 bar Boost & 900 C Pyro", 255),
            new KeyValuePair<string, int>("Dual 1600 F Pyro", 255),
            new KeyValuePair<string, int>("DEF & Fuel level", 255),
            new KeyValuePair<string, int>("Fuel level & 280 temp", 255),
            new KeyValuePair<string, int>("Coolant Temp, Oil Pressure, Voltmeter, Fuel Level", 255),
            new KeyValuePair<string, int>("2 in 1 Demo", 255),
            new KeyValuePair<string, int>("4 in 1 Demo", 255),
            new KeyValuePair<string, int>("GAUGE NONE", -1)
            };
        }
        public IList<string> GaugeModes
        {
            get => new List<string>
            {
                "NORMAL",
                "TEST"
                //"SWEEP DEMO",
                //"WARN DEMO",
                //"MAX SIZE"
            };
        }
        public AttributesViewModel()
        {
        }
    }
}