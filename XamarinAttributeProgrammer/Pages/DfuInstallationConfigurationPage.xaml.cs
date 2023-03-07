using XamarinAttributeProgrammer.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinAttributeProgrammer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DfuInstallationConfigurationPage : ContentPage
    {
        public DfuInstallationConfigurationPage()
        {
            InitializeComponent();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            DfuInstallationConfigurationPageViewModel.Instance.RefreshDfuInstallationConfiguration();
        }

        private void Button_OnClicked(object sender, System.EventArgs e)
        {
            DfuInstallationConfigurationPageViewModel.Instance.Reset();
            Navigation.PopAsync();
        }

        private void BackBtn_ClickedAsync(object sender, System.EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}