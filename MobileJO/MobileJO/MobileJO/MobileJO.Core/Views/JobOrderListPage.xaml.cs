
using MobileJO.Core.Base;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MobileJO.Core.Views
{
    public partial class JobOrderListPage : BaseContentPage
    {
        public JobOrderListPage()
        {
            InitializeComponent();
           
            //initialize only if needed Activity Indicator
            var tempContent = Content;

            //Assign content to null to fix bug for iOS
            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);

        }


        private void MenuItem_Clicked(object sender, System.EventArgs e)
        {
            MenuItem btn = (MenuItem)sender;           

            int localID = ((JobOrderModel)btn.BindingContext).ID;
            int serverID = ((JobOrderModel)btn.BindingContext).ServerID;
            string status = ((JobOrderModel)btn.BindingContext).StatusID;
            string textMenuItem = btn.Text;
            var vm = (JobOrderListViewModel)DataContext;

            var param = new Dictionary<string, string> {
                { Constants.Keys.LocalJobOrderID, localID.ToString() },
                { Constants.Keys.ServerJobOrderID, serverID.ToString() },
                { Constants.Params.JobOrderStatus, status }
            };

            if (textMenuItem.Equals(Constants.Params.DeleteMenuItem))
            {
                vm.DeleteCommand.Execute(param);

               
            }
            if (textMenuItem.Equals(Constants.Params.RevertMenuItem))
            {
                vm.RevertCommand.Execute(param);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            var vm = (JobOrderListViewModel)DataContext;

            vm.GoToMainMenu.Execute();

            return true;
        }

    }
}
