using dotMorten.Xamarin.Forms;
using dotMorten.Xamarin.Forms.Platform.iOS;
using MobileJO.Core.CustomRenderer;
using MobileJO.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomAutoSuggestBox), typeof(CustomAutoSuggestBoxRenderer))]
namespace MobileJO.iOS.CustomRenderers
{
    public class CustomAutoSuggestBoxRenderer : AutoSuggestBoxRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<AutoSuggestBox> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.InputTextField.BorderStyle = UITextBorderStyle.RoundedRect;
                Control.ShowBottomBorder = false;
                Control.Font = UIFont.SystemFontOfSize(13);
            }
        }
    }
}