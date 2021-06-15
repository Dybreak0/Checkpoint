using MobileJO.Core.Base;
using MvvmCross.Forms.Presenters.Attributes;

namespace MobileJO.Core.Views
{
    [MvxModalPresentation]
    public partial class BillingTypesPage : BaseContentPage
    {
        public BillingTypesPage()
        {
            InitializeComponent();

            //initialize only if needed Activity Indicator
            var tempContent = Content;

            //Assign content to null to fix bug for iOS
            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);
        }

    }
}
