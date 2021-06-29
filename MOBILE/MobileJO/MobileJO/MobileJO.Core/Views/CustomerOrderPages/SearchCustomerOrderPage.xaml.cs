
using MobileJO.Core.Base;

namespace MobileJO.Core.Views.CustomerOrderPages
{
    public partial class SearchCustomerOrderPage : BaseContentPage
    {
        public SearchCustomerOrderPage()
        {
            InitializeComponent();

            //BindingContext = new SearchViewModel();
            //initialize only if needed Activity Indicator
            var tempContent = Content;

            //Assign content to null to fix bug for iOS
            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);
        }
    }

}