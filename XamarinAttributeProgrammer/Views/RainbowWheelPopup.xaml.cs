using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.CommunityToolkit;
using Xamarin.Forms.Xaml;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.ViewModels;

/// <summary>
/// Author: Tam Nguyen
/// This is the color wheel popup. Originally, it updates the LED in real time as the user TAP and drag,
/// but due to lag and multiple error message (too much write in a short time), I decide top just show
/// the color on the backgroup of the popup and only update the LED once the user accepts.
/// Most of the logic is handled via data binding with the view model.
/// </summary>
namespace XamarinAttributeProgrammer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RainbowWheelPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private TaskCompletionSource<string> taskCompletionSource;
        public Task<string> PopupClosedTask { get { return taskCompletionSource.Task; } }

        RainbowWheelPopupViewModel vm;

        public RainbowWheelPopup( bool focus)
        {
            InitializeComponent();
            BindingContext = vm = new RainbowWheelPopupViewModel(focus); // Binding to the view model.
        }
        private async void CloseDialog(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.Text.Equals(Resourcer.Ok))
                taskCompletionSource.SetResult(vm.SelectedColor.ToHex());
            else
                taskCompletionSource.SetResult(null);
            cancelbtn.IsEnabled = okbtn.IsEnabled = false;

            await PopupNavigation.Instance.PopAllAsync();

        }

        protected override void OnAppearing()
        {
            DiagnosticPage.AddToLog("I: Opened color wheel popup.");
            taskCompletionSource = new TaskCompletionSource<string>();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            DiagnosticPage.AddToLog("I: Exiting color wheel popup.");
            taskCompletionSource = new TaskCompletionSource<string>();
            base.OnDisappearing();
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }

    }
}