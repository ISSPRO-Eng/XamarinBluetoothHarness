using XamarinAttributeProgrammer.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinAttributeProgrammer.Popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAttributeProgrammer.Models;
using Rg.Plugins.Popup.Extensions;

namespace XamarinAttributeProgrammer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage_PickFirmwarePackageFrame
    {
        public MainPage_PickFirmwarePackageFrame()
        {
            InitializeComponent();
            GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(OnCLicked)
            });
        }

        private async void OnCLicked()
        {
            int file = await FirmwarePackageViewModel.Instance.PickFileAsync();
            if (file == 1)
            {
                await Navigation.PushPopupAsync(new NotAcceptedFilePopupPage());
            }
            
        }
    }
}