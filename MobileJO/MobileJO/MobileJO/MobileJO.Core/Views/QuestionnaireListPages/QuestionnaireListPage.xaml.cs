using MobileJO.Core.Base;
using MobileJO.Core.ViewModels;

namespace MobileJO.Core.Views.QuestionnaireListPages
{
    public partial class QuestionnaireListPage : BaseContentPage
    {
        public QuestionnaireListPage()
        {
            InitializeComponent();

            //initialize only if needed Activity Indicator
            var tempContent = Content;

            //Assign content to null to fix bug for iOS
            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = (QuestionnaireListViewModel)DataContext;
            vm.RefreshList.Execute();
        }
    }
}