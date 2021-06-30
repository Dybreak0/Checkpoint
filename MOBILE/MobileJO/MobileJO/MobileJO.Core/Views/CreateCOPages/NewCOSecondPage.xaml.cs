
using MobileJO.Core.Base;
using MobileJO.Core.ViewModels.CreateCOViewModels;

namespace MobileJO.Core.Views.CreateCOPages
{
    public partial class NewCOSecondPage : BaseContentPage
    {
        public NewCOSecondPage()
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