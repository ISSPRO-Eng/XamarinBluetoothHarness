using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.Resources;

namespace XamarinAttributeProgrammer.ViewModels
{
    public class ConnectViewModel : BaseViewModel
    {

        private string _deviceInfo = "";
        public bool isConnected  //                                             WE ONLY NEED TO CHANGE THIS TO UPDATE EVERYTHING ELSE
        { 
            get { return _isConnected; }
            set
            {
                _isConnected = value;

                // Let all the binded variables know the values is updated
                // and that it needs to update too
                OnPropertyChanged();
                OnPropertyChanged(nameof(StatusString));
                OnPropertyChanged(nameof(ButtonTxtVal));
                OnPropertyChanged(nameof(ConnectionInfoString));
                OnPropertyChanged(nameof(StatusFontcolor));
            }
        }
        public string StatusString 
        {
            get { return isConnected ? getResStrVal("connected"): getResStrVal("disconnected"); }
        }

        public string renameBtnTxt { get => getResStrVal("rename"); }
        public string to { get => getResStrVal("to"); }
        public string ButtonTxtVal
        {
            get { return isConnected ? getResStrVal("disconnectBtnTxt") : getResStrVal("connectBtnTxt"); }
        }
        public string ConnectionInfoString
        {
            get => isConnected ? _deviceInfo : getResStrVal("disconnectedInfoMsg");
            set
            {
                _deviceInfo = value;
                OnPropertyChanged();
            }
        }


        public Color StatusFontcolor
        {
            get { return (Color)(isConnected ? Application.Current.Resources["Pass"] : Application.Current.Resources["Warn"]); }
        }



        private ResourceManager ResMngr = StringValues.ResourceManager;
        private bool _isConnected = false;

        public ConnectViewModel()
        {
            if (ResMngr == null) ResMngr = StringValues.ResourceManager;
            isConnected = false;
            _deviceInfo = getResStrVal("connectedInfoMsg");
        }

        public string getResStrVal(string key) 
        { 
            // Return the value for the key, otherwise return null
            try
            {
                string value = ResMngr.GetString(key) ?? null;
                return value;
            } catch
            {
                return null;
            }
        }
    }
}
