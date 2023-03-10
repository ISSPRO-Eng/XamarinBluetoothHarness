using XamarinAttributeProgrammer.Pages;
using XamarinAttributeProgrammer.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinAttributeProgrammer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage_SelectADeviceFrame
    {
        public MainPage_SelectADeviceFrame()
        {
            InitializeComponent();
            GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(OnCLicked)
            });
        }

        private void OnCLicked()
        {
            Navigation.PushAsync(new SelectADevicePage());
        }
    }
}