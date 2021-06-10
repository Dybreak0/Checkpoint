using MobileJO.Core.Base;
using MobileJO.Core.ViewModels.AssignedCases;
using Xamarin.Forms.Xaml;

namespace MobileJO.Core.Views.AssignedCases
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssignedCasesList : BaseContentPage
    {
        
        public AssignedCasesList()
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
            var vm = (AssignedCasesListViewModel)DataContext;

            vm.GoToMainMenu.Execute();

            return true;
        }

    }
}
