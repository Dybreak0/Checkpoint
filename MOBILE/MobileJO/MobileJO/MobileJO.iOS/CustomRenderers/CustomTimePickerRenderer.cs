using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MobileJO.Core.CustomRenderer;
using MobileJO.iOS.CustomRenderers;

[assembly: ExportRenderer(typeof(TimePicker), typeof(CustomTimePickerRenderer))]
namespace MobileJO.iOS.CustomRenderers
{
    public class CustomTimePickerRenderer : TimePickerRenderer
    {
        private readonly PickerDelegate _delegate = new PickerDelegate();

        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);

            Control.Delegate = _delegate;
        }
    }
}