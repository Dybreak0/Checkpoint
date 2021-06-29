//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Xamarin.Forms;
//using Xamarin.Forms.Xaml;

//namespace MobileJO.Core.Views.CustomerOrderPages
//{
//    [XamlCompilation(XamlCompilationOptions.Compile)]
//    public partial class CustomerOrderListPage : ContentView
//    {
//        public CustomerOrderListPage()
//        {
//            InitializeComponent();
//        }
//    }
//}
using MobileJO.Core.Base;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MobileJO.Core.Views.CustomerOrderPages
{
    public partial class CustomerOrderListPage : BaseContentPage
    {
        public CustomerOrderListPage()
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
            var vm = (CustomerOrderListViewModel)DataContext;

            vm.GoToMainMenu.Execute();

            return true;
        }

    }
}
