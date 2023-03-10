using Plugin.BLE.Abstractions.Exceptions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAttributeProgrammer.HelperClass;
using XamarinAttributeProgrammer.Models;
using XamarinAttributeProgrammer.Resources;
using XamarinAttributeProgrammer.ViewModels;


/// <summary>
/// Author: Tam Nguyen
/// This page allows the user to modify the LED via slider, preset style and hex. The color wheel is
/// another (popup) class, but it's called from this class. 
/// </summary>
namespace XamarinAttributeProgrammer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorSelectionPage : ContentPage
    {

        // Define const for better understanding
        public const bool BACKLIGHT = false;
        public const bool POINTER = true;
        bool first = true;

        private ColorSelectionViewModel csvm;
        private BLEConnections _connection;
        public ColorSelectionPage()
        {
            InitializeComponent();
            BindingContext = csvm = new ColorSelectionViewModel(); // Binding this view to the viewmodel for all bindings on the xaml

            // BLEConnections and GaugeAttributes setup are in the view model
            _connection = App._BLEConnection;
        }

        // Event handler when the the page is in focus
        protected override async void OnAppearing()
        {
            //update preview to match the gauge
            // wait for 20 ms because if we don't it sometimes doesnt get the settings
            // correctly.
            await Task.Delay(20);
            DiagnosticPage.AddToLog("I: Entering Color Selection page.");
            GaugeAttributes att = App.AttributeManager;
            if (first == true)
            {
                await DisplayAlert(Resourcer.getResStrVal("warning"),
                        ("Be sure to save new colors before leaving page."), Resourcer.Ok);
                first = false;
            }
        }

        /// <summary>
        /// Function to handle the clicking event of the "Needle" or "Backlight" button the top of the page.
        /// Set the focus to either the needle or backlight depending on which the user tap.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PriorityColorBtn_Clicked(object sender, EventArgs e)
        {
            Button sendr = sender as Button;
            csvm.FocusColorOn = sendr.Text.Equals("Needle");
        }

        private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            // Reserved for what ever we need to process when any of the sliders are changed
        }

        /// <summary>
        /// Event handler for when the user tap on the needle or the back light of the svgs. This is
        /// where the color wheel popup is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnPreviewTapped(object sender, EventArgs e)
        {
            Button sendr = (Button)sender;
            Console.WriteLine(sendr.ClassId);
            string sendrID = sendr.ClassId;
            string wheelSelection = null;
            RainbowWheelPopup popup;


            if (csvm == null) return;
            if (sendrID.Equals("BackLight"))
            {
                csvm.ChangeColorFocusOn(BACKLIGHT);
                popup = new RainbowWheelPopup(BACKLIGHT);
                await PopupNavigation.Instance.PushAsync(popup); // Show colorwheel
            }
            else if (sendrID.Equals("Needle"))
            {
                csvm.ChangeColorFocusOn(POINTER);
                popup = new RainbowWheelPopup(POINTER);
                await PopupNavigation.Instance.PushAsync(popup); // Show colorwheel
            }
            else
                return; // this shouldnt ever hit...

            // wait for the popup to close and grab the returning value
            wheelSelection = await popup.PopupClosedTask;

            if (wheelSelection == null) return; // user hits cancel

            Color selected = Color.FromHex(wheelSelection);

            csvm.SetColorTo(selected);
            
            DiagnosticPage.AddToLog($"I: Update gauge {sendrID} color to {wheelSelection} from color wheel.");
        }

        // Event handler for validating the hex input
        private void HexInput_Completed(object sender, EventArgs e)
        { 
            string hex = (sender as Entry).Text;
            Regex rx = new Regex("^#?([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$", // Use regex to parse and validate the input
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Find matches (if it fits our expression).
            MatchCollection matches = rx.Matches(hex);

            if (matches.Count > 0)
            {
                Color c = Color.FromHex(hex);
                csvm.SetColorTo(c);
            }
        }

        private async void StyleBtn_ClickedAsync(object sender, EventArgs e)
        {
            Button sndr = sender as Button;
            string id = sndr.ClassId;
            GaugeAttributes ga = App.AttributeManager;
            BindingContext = csvm = new ColorSelectionViewModel();


            try
            {
                switch (id)
                {
                    case "1": // r11 B:W, P:W
                        ga.SetQuad0BackLightColor(0, 0, 40);
                        ga.SetPointerColor(0, 0, 211);
                        break;
                    case "3": // dodge gen 1-2 B: W P: R
                        ga.SetQuad0BackLightColor(0, 0, 40);
                        ga.SetPointerColor(231, 17, 0);
                        break;
                    case "2": // r13 B:Light Blue P: Blue
                        ga.SetQuad0BackLightColor(0, 75, 106);
                        ga.SetPointerColor(0, 11, 73);
                        break;
                    case "4": // dodge gen 3 B: G, P:R
                        ga.SetQuad0BackLightColor(0, 50, 0);
                        ga.SetPointerColor(231, 17, 0);
                        break;
                    case "5": // ford 94-97 B: G, P:W
                        ga.SetQuad0BackLightColor(0, 50, 0);
                        ga.SetPointerColor(0, 0, 0);
                        break;
                    case "6":// gm 2007+ B: Cyan, P:R
                        ga.SetQuad0BackLightColor(0, 67, 30);
                        ga.SetPointerColor(107, 17, 0);
                        break;
                    default:
                        break;
                }
                /*
                await csvm.SetBacklightPreviewTo(ga.getBacklightColorRed(),
                                    ga.getBacklightColorGreen(),
                                    ga.getBacklightColorBlue());
                //await Task.Delay(50);
                await csvm.SetPointerPreviewTo(ga.getPointerColorRed(),
                                    ga.getPointerColorGreen(),
                                    ga.getPointerColorBlue());
                DiagnosticPage.AddToLog($"I: Preset style {id} selected.");
                */
            }
            catch (Exception ex) when (!(ex is CharacteristicReadException))
            {
                DiagnosticPage.AddToLog("ESB: " + ex.Message);
            }
        }

        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
        /// <summary>
        /// This is the Save function. To flash the gauge with new settings,
        /// the applications will generate 6 packets and send each one at a time.
        /// To ensure the data are properly sent, we want to implement a threadlock
        /// (semaphore) to force the packets to take turn so that it acts more like
        /// a TCP than UDP.
        /// </summary>
        private async void SaveBtn_ClickedAsync(object sender, EventArgs e)
        {
            Thread.Sleep(100); // Allow gauge preview to match physical gauge, avoids GATT error being thrown 
            byte[][] cmds = GaugeCommands.FlashCommand();
            try
            {
                DiagnosticPage.AddToLog($"I: Saving new attribute(s) to {_connection.DeviceName}");

                BLEConnections connection = App._BLEConnection;
                for (int i = 0; i < cmds.Length; i++)
                {
                    byte[] cmd = cmds[i];
                    await semaphoreSlim.WaitAsync(); // Wait here until locks opens
                    await connection.TryWriteAsync(cmd);
                    semaphoreSlim.Release();
                }
               
                    await DisplayAlert(Resourcer.Success,
                                       Resourcer.getResStrVal("flashedSuccessMsg"),
                                       Resourcer.Ok);
                
            }
            catch (CharacteristicReadException rex) {
                DiagnosticPage.AddToLog("ESB2: " + rex.Message);
                await DisplayAlert(Resourcer.Error,
                      Resourcer.getResStrVal("errorMsg"),
                      Resourcer.Ok);
            }
            catch (Exception ex)
            {
                DiagnosticPage.AddToLog("ESB2: " + ex.Message);
            }
            finally
            {
                if (semaphoreSlim.CurrentCount == 0)
                {
                    semaphoreSlim.Release();
                }
            }
        }
    }
}