using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MobileJO.Core.CustomRenderer;
using MobileJO.iOS.CustomRenderers;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace MobileJO.iOS.CustomRenderers
{
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        private readonly PickerDelegate _delegate = new PickerDelegate();

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null) return;
            
            var element = (CustomDatePicker)Element;
            var hasBackground = element.HasBackground;

            if (hasBackground)
            {
                Control.Background = null;
            }

            Control.Delegate = _delegate;
        }

    }
}