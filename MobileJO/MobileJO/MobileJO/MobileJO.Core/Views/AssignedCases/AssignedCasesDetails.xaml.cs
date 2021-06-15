using MobileJO.Core.Base;
using Xamarin.Forms.Xaml;

namespace MobileJO.Core.Views.AssignedCases
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AssignedCasesDetails : BaseContentPage
	{
		public AssignedCasesDetails()
		{
			InitializeComponent ();

            //initialize only if needed Activity Indicator
            var tempContent = Content;

            //Assign content to null to fix bug for iOS
            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);
        }
	}
}