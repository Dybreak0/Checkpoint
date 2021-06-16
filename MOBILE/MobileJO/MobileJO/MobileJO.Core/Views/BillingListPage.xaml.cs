using MobileJO.Core.Base;


namespace MobileJO.Core.Views
{
	
	public partial class BillingListPage : BaseContentPage
	{
		public BillingListPage ()
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