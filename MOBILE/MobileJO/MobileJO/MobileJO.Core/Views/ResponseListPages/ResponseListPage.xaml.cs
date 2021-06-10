using System;
using System.Collections.ObjectModel;
using MobileJO.Core.Base;
using MobileJO.Core.Models;
using MobileJO.Core.ViewModels;

namespace MobileJO.Core.Views.ResponseListPages
{
    public partial class ResponseListPage : BaseContentPage
    {
        public ResponseListPage()
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
            var vm = (ResponseListViewModel)DataContext;
            if (vm.IsBusy) return;
            vm.IsForcedRefresh = true;
            vm.RefreshList.Execute();
        }
    }
}