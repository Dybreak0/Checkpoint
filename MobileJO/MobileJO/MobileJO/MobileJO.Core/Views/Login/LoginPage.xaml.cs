using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileJO.Core.Views.Login
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : BaseContentPage
    {
		public LoginPage ()
		{
            InitializeComponent();

            //initialize only if needed Activity Indicator
            var tempContent = Content;

            //Assign content to null to fix bug for iOS
            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);

        }

        protected override bool OnBackButtonPressed()
        {
            var appCloser = DependencyService.Get<ICloseApplication>();

            appCloser?.ExitApplication();

            return true;
        }
    }
}