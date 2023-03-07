using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.ViewModels;

/// <summary>
/// Author: Tam Nguyen
/// Diagnostic page is used for debugging and trouble shooting. It only supports english, and only updates when re-entering the page
/// To access: Option->Diagnostic
/// 
/// Useful:
/// To debug and track down the error, reference Error Prefix tracking.txt to see where the error is occuring!
/// </summary>
namespace XamarinAttributeProgrammer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiagnosticPage : ContentPage
    {
        public static string _log = "<< Debug log init >>" + Environment.NewLine;
        DiagnosticViewModel vm;
        public DiagnosticPage()
        {
            InitializeComponent();
            BindingContext = vm = new DiagnosticViewModel(); // Bind to the viewmodel
            logBox.Text = _log;
        }

        protected override void OnAppearing()
        {
            logBox.Text = _log;

            logBox.Text = _log;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            // When we leave the page, cancel the periodic refreshing.
            base.OnDisappearing();
        }
        // function to call when we want to add to the message boxResourcer.Warning
        public static void AddToLog(string msg)
        {
            _log += $"{DateTime.Now:HH:mm}  {msg + Environment.NewLine}";
        }

        private async void ClearBtn_Clicked(object sender, EventArgs e)
        {
            var ans = await DisplayAlert(Resourcer.Warning,
                                    Resourcer.getResStrVal("clearLog"),
                                    Resourcer.Ok, Resourcer.Cancel);
            if (ans)
            {
                _log = "";
                logBox.Text = _log;
            }
        }

        private void BackBtn_ClickedAsync(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void CopyBtn_ClickedAsync(object sender, EventArgs e)
        {
            await Clipboard.SetTextAsync(_log);
            await DisplayAlert(Resourcer.Success, 
                                Resourcer.getResStrVal("copySuccess"), 
                                Resourcer.Ok);
        }
    }
}