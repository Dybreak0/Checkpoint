using MobileJO.Core.Base;

namespace MobileJO.Core.Views
{
    public partial class SecondPage : BaseContentPage
    {
        public SecondPage()
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
            Navigation.PopAsync();

            return true;
        }
    }
}
