using MobileJO.Core.Base;
using Xamarin.Forms.Xaml;

namespace MobileJO.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : BaseContentPage
    {
        public ForgotPasswordPage()
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