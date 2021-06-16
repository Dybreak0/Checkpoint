using MobileJO.Core.Base;
using MobileJO.Core.ViewModels;
using MvvmCross.Forms.Presenters.Attributes;

namespace MobileJO.Core.Views
{
    [MvxModalPresentation]
    public partial class EditBillingTypesPage : BaseContentPage
    {
        public EditBillingTypesPage()
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
            var vm = (EditBillingTypesViewModel)DataContext;

            vm.CloseCommand.Execute();

            return true;
        }
    }
}
