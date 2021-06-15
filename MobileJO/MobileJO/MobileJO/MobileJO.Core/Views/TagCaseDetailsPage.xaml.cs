using MobileJO.Core.Base;

namespace MobileJO.Core.Views
{
	
	public partial class TagCaseDetailsPage : BaseContentPage
	{
		public TagCaseDetailsPage ()
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