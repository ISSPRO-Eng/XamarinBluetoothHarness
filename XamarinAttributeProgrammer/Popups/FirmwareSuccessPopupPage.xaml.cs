using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace XamarinAttributeProgrammer.Popups
{
    public partial class FirmwareSuccessPopupPage : PopupPage
    {
        public FirmwareSuccessPopupPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            HidePopup();
        }

        private async void HidePopup()
        {
            await Task.Delay(4000);

            if (PopupNavigation.Instance.PopupStack.Contains(this))
                await PopupNavigation.Instance.RemovePageAsync(this);
        }
    }
}
