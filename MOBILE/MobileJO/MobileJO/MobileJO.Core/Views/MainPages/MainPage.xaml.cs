using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using Xamarin.Forms;

namespace MobileJO.Core.Views
{ 
	public partial class MainPage : BaseContentPage
	{
		public MainPage()
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