using MobileJO.Core.Base;
using MobileJO.Core.Models;
using MobileJO.Core.ViewModels;
using MobileJO.Core.ViewModels.CreateCOViewModels;
using MvvmCross.Forms.Presenters.Attributes;
using SignaturePad.Forms;
using System;
using System.IO;

namespace MobileJO.Core.Views.CreateCOPages
{
    [MvxModalPresentation]
    public partial class AddUnitDesiredPage : BaseContentPage
    {
        public AddUnitDesiredPage()
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
            var vm = (AddUnitDesiredViewModel)DataContext;

            vm.CloseCommand.Execute();

            return true;
        }
    }
}
