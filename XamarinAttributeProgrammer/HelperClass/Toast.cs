using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinAttributeProgrammer.Services;
using XamarinAttributeProgrammer.Views;

namespace XamarinAttributeProgrammer.HelperClass
{
    /// <summary>
    /// This clas is to allow us to display a little banner at the bottom of the screen
    /// with a message (like Android's Toast). Since Apple does not have a built in 
    /// feature like it, we have to use Xamarin Toolkit's ToastOption to render it for
    /// iOS phones.
    /// </summary>
    public class Toast
    {
        private static int _timerShort = 900;//ms
        //private static int _timerLong = 2000;// 2s
        public static async void ShowMessage(ContentPage page, string message)
        {
            var platform = DeviceInfo.Platform;

            try
            {
                if (platform == DevicePlatform.iOS)
                {
                    ToastOptions to = new ToastOptions()
                    {
                        CornerRadius = new Thickness(30,30,30,30),
                        MessageOptions = new MessageOptions()
                        {
                            Message = message,
                            Foreground = Color.White
                        },
                        Duration = new TimeSpan(0, 0, 0, 0, _timerShort)
                    };
                    await page.DisplayToastAsync(to);
                }
                else if (platform == DevicePlatform.Android)
                {
                    DependencyService.Get<IToastMessage>().ShortAlert(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Toast, " + ex.Message);
                DiagnosticPage.AddToLog("ETSM: " + ex.Message);
            }
        }
    }
}
