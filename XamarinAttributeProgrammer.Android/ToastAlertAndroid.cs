using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XamarinAttributeProgrammer.Droid;
using XamarinAttributeProgrammer.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ToastAlertAndroid))]
namespace XamarinAttributeProgrammer.Droid
{
    public class ToastAlertAndroid : IToastMessage
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}