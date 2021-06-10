using MobileJO.Core.Base;
using Xamarin.Forms.Xaml;

namespace MobileJO.Core.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignaturePage : BaseContentPage
	{
		public SignaturePage ()
		{
			InitializeComponent ();

            var tempContent = Content;

            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);
        }
	}
}