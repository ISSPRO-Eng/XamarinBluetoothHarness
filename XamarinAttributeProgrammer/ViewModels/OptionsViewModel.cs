using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace XamarinAttributeProgrammer.ViewModels
{
    public class OptionsViewModel : BaseViewModel
    {
        public string AppVersion { get => "v" + VersionTracking.CurrentVersion; }
        public string BuildVersion { get => VersionTracking.CurrentBuild; }
        public bool GaugeConnected { get => App._BLEConnection.IsConnected; }
        public OptionsViewModel()
        {

        }
    }
}
