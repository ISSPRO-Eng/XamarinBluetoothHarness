using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinAttributeProgrammer.Droid;

////////////////////// Class sole purpose is to remove the underline of the picker (combo box)
///https://stackoverflow.com/questions/47206113/is-it-possible-to-give-the-underline-bar-of-a-picker-a-color
[assembly: ExportRenderer(typeof(Picker), typeof(CustomPickerRenderer))]
namespace XamarinAttributeProgrammer.Droid
{
    class CustomPickerRenderer : PickerRenderer
    {
        public CustomPickerRenderer(Context context) : base(context)
        {
             
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.Background = null;
            }
        }
    }
}