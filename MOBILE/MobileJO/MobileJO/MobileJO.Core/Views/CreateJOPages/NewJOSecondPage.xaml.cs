using MobileJO.Core.Base;

namespace MobileJO.Core.Views
{
    public partial class NewJOSecondPage : BaseContentPage
    {
        public NewJOSecondPage()
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
