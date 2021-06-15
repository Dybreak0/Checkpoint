using MobileJO.Core.Base;

namespace MobileJO.Core.Views
{
    public partial class EditCaseTaggingPage : BaseContentPage
    {
        public EditCaseTaggingPage()
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
