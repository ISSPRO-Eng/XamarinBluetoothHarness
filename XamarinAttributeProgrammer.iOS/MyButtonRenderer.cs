using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinAttributeProgrammer.iOS;

[assembly: ExportRenderer(typeof(Button), typeof(MyButtonRenderer))]
namespace XamarinAttributeProgrammer.iOS
{
    // This class sole purpose is to make buttons show second line
    public class MyButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
                Control.TitleLabel.TextAlignment = UITextAlignment.Center;
            }
        }
    }
}