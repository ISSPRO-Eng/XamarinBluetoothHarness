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
using Xamarin.Forms; 
using Xamarin.Forms.Platform.Android;
using XamarinAttributeProgrammer.Droid;

[assembly: ExportRenderer(typeof(Entry), typeof(ExtendedEntryRenderer))]
namespace XamarinAttributeProgrammer.Droid
{
    // Sole purpose is to add the unfocus event handler to send "done" when you tap outside
    class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.FocusChange += Control_FocusChange;
            }
        }

        private void Control_FocusChange(object sender, FocusChangeEventArgs e)
        {
            if (!e.HasFocus)
                ((IEntryController)Element).SendCompleted();
        }
    }
}