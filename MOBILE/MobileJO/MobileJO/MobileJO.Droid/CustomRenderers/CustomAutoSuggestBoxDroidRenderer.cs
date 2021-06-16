using dotMorten.Xamarin.Forms;
using dotMorten.Xamarin.Forms.Platform.Android;
using MobileJO.Core.CustomRenderer;
using MobileJO.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomAutoSuggestBox), typeof(CustomAutoSuggestBoxRenderer))]

namespace MobileJO.Droid.CustomRenderers
{
    public class CustomAutoSuggestBoxRenderer : AutoSuggestBoxRenderer
    {

        public CustomAutoSuggestBoxRenderer(Android.Content.Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AutoSuggestBox> e)
        {
            base.OnElementChanged(e);
            
            if(Control != null)
            {
                Control.SetTextSize(Android.Util.ComplexUnitType.Sp, 12);
            }
        }
    }
}