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

        /// <summary>
        // LABELS
        /// </summary>
        /// 
        public string gaugeType         { get => Resourcer.getResStrVal("gaugeType"); }
        public string gaugeMode         { get => Resourcer.getResStrVal("gaugeMode"); }
        public string voltHi            { get => Resourcer.getResStrVal("dimVoltHi"); }
        public string voltLo            { get => Resourcer.getResStrVal("dimVoltLo"); }
        public string warnThresTop      { get => Resourcer.getResStrVal("warnThresTop"); }
        public string warnThresBot      { get => Resourcer.getResStrVal("warnThresBot"); }
        public string flashZone         { get => Resourcer.getResStrVal("flashZone"); }
        public string flashEff          { get => Resourcer.getResStrVal("flashEffect"); }
        public string flashInten        { get => Resourcer.getResStrVal("flashIntens"); }
        public string warnBtn           { get => Resourcer.getResStrVal("warnBtn"); }
        public string flashBtn          { get => Resourcer.getResStrVal("flashBtn"); }
        public string outputTopThres    { get => Resourcer.getResStrVal("outputTopThres"); }
        public string outputBotThres    { get => Resourcer.getResStrVal("outputBotThres"); }
        public string activDelay        { get => Resourcer.getResStrVal("activDelay"); }
        public string startDelay        { get => Resourcer.getResStrVal("startDelay"); }
        public string ptrW8             { get => Resourcer.getResStrVal("ptrW8"); }
        public string sensHys           { get => Resourcer.getResStrVal("sensHys"); }
        public string sensCoeff0        { get => Resourcer.getResStrVal("sensCoeff0"); }
        public string sensCoeff1        { get => Resourcer.getResStrVal("sensCoeff1"); }
        public string sensScanRt        { get => Resourcer.getResStrVal("sensScanRt"); }
        public string backLightScanRt   { get => Resourcer.getResStrVal("BacklightScanRt"); }
        public string speedoPPM         { get => Resourcer.getResStrVal("speedoPPM"); }
        public string tachPPR           { get => Resourcer.getResStrVal("tachPPR"); }
        public string lcdTotalAccumulation { get => Resourcer.getResStrVal("lcdTotalAccumulation"); }
        public string enableAccumulation   { get => Resourcer.getResStrVal("enableAccumulation"); }
        public string distanceUnits     { get => Resourcer.getResStrVal("distanceUnits"); }
        public string sensorEnabled     { get => Resourcer.getResStrVal("enableSensor"); }
        public string tripEnabled       { get => Resourcer.getResStrVal("enableTrip"); }
        public string precisionPPR      { get => Resourcer.getResStrVal("precisionPPR"); }
        public string speedoTachOutput  { get => Resourcer.getResStrVal("speedoTachOutput"); }
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


        /// <summary>
        /// VALUES and ITEMSOURCE
        /// </summary>
        public IList<string> Intesnity { get => new List<string> { "LOW", "MEDIUM", "HIGH"}; }
        public IList<string> bootDelays { get => new List<string> { "DISABLE", "15", "30", "45"}; }
        public IList<string> zones { get => new List<string> { "LOW", "HIGH" }; }
        public IList<string> enable { get => new List<string> { "DISABLED", "ENABLED" }; }
        public IList<string> units { get => new List<string> { "MILES", "KILOMETERS" }; }
        public IList<string> dataType { get => new List<string> { "INTEGER", "FLOAT" }; }
        public IList<string> output { get => new List<string> { "NORMAL", "ONE-TIME","REPEATING" }; }

        public IList<KeyValuePair<string, int>> GaugeType
        {
            // ENSURE this list MATCHES GaugeType enum's order from the xaml.cs file!!!!!
            get => new List<KeyValuePair<string, int>> {
            new KeyValuePair<string, int>("DEMO", 255),
            new KeyValuePair<string, int>("FUEL LEVEL (242 TO 33)", 5),
            new KeyValuePair<string, int>("VOLT 18V", 6),
            new KeyValuePair<string, int>("COOL TEMP 100-280°F", 4),
            new KeyValuePair<string, int>("REAR AXLE TEMP 100-280°F", 4),
            new KeyValuePair<string, int>("TRANS OIL TEMP 100-280°", 4),
            new KeyValuePair<string, int>("AIR PRESS 175psi", 0),
            new KeyValuePair<string, int>("OIL PRESS 100psi", 0),
            new KeyValuePair<string, int>("EXHAUST BACK PRESS 60psi", 3),
            new KeyValuePair<string, int>("EXHAUST BACK PRESS 100psi", 0),
            new KeyValuePair<string, int>("TURBO BOOST 40psi", 2),
            new KeyValuePair<string, int>("TURBO BOOST 60psi", 3),
            new KeyValuePair<string, int>("TURBO BOOST 100psi", 0),
            new KeyValuePair<string, int>("FUEL PRESS 30psi (30psi sensor)", 0),
            new KeyValuePair<string, int>("FUEL PRESS 30psi (100psi sensor)", 1),
            new KeyValuePair<string, int>("FUEL PRESS 100psi (40psi sensor)", 2),
            new KeyValuePair<string, int>("FUEL PRESS 100psi (100psi sensor)", 0),
            new KeyValuePair<string, int>("FUEL RAIL PRESS (30kpsi sensor)", 0),
            new KeyValuePair<string, int>("VOLT 36V", 7),
            new KeyValuePair<string, int>("PYROMETER 1600 F", 8), /// 176S Boards for pyros
            new KeyValuePair<string, int>("PYROMETER 2000 F", 8),
            new KeyValuePair<string, int>("FUEL LEVEL (0 TO 30)", 5),
            new KeyValuePair<string, int>("FUEL LEVEL (0 TO 90)", 5),
            new KeyValuePair<string, int>("FUEL LEVEL (10 TO 180)", 5),
            new KeyValuePair<string, int>("FUEL LEVEL (78 TO 10)", 5),
            new KeyValuePair<string, int>("FUEL LEVEL (10 TO 158)", 5),
            new KeyValuePair<string, int>("SPEEDOMETER",9),
            new KeyValuePair<string, int>("TACHOMETER",9),
            new KeyValuePair<string, int>("GPS SPEEDOMETER", 9),
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