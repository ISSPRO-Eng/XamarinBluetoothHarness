using Foundation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinAttributeProgrammer.iOS;

// Extended the entry for ios to include the "DONE" button
// copy from https://evgenyzborovsky.com/2017/05/05/numeric-keyboard-with-done-button-on-ios-xamarin-forms/

[assembly: ExportRenderer(typeof(Entry), typeof(ExtendedEntryRenderer))]
namespace XamarinAttributeProgrammer.iOS
{
    public class ExtendedEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Element == null)
				return;

			// Check only for Numeric keyboard
			if (this.Element.Keyboard == Keyboard.Numeric)
            {
				this.AddDoneButton();
            }
            this.Element.Unfocused += Element_Unfocused;
		}

        private void Element_Unfocused(object sender, FocusEventArgs e)
        {
			// BS unfocuse event wont trigger the send complete for our entry,
			// So, I'm forcing it to
			((IEntryController)Element).SendCompleted();
		}

        /// <summary>
        /// <para>Add toolbar with +/- and Done button</para>
        /// </summary>
        protected void AddDoneButton()
		{
			var toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f));
			var minusButton = new UIBarButtonItem("+/-", UIBarButtonItemStyle.Plain, delegate
			{
				if (!(Element is Entry entry)) return;
				entry.Text = entry.Text is { } text && text.StartsWith("-")
			   ? entry.Text.Substring(1)
			   : $"-{entry.Text}"; ;
			});


			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
			{
				this.Control.ResignFirstResponder();
				var baseEntry = this.Element.GetType();
				((IEntryController)Element).SendCompleted();
			});

			toolbar.Items = new UIBarButtonItem[] {
				minusButton,
				new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
				doneButton
			};
			this.Control.InputAccessoryView = toolbar;
		}
	}
}