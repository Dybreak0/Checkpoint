using MobileJO.Core.Base;
using Xamarin.Forms.Xaml;

namespace MobileJO.Core.Views.QuestionnaireListPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchQuestionnairePage : BaseContentPage
    {
        public SearchQuestionnairePage()
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