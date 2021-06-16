using MobileJO.Core.Base;
using MobileJO.Core.ViewModels;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Xamarin.Forms.Xaml;

namespace MobileJO.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobOrderPage : BaseContentPage, IMvxOverridePresentationAttribute
    {
        public JobOrderPage()
        {
            InitializeComponent();

            var tempContent = Content;

            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);
        }

        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {            
            if (request != null)
            {
                var vmRequest = request as MvxViewModelInstanceRequest;
                if (vmRequest.PresentationValues.ContainsKey("NavigationMode") )
                {
                    return new MvxContentPagePresentationAttribute
                    {
                        WrapInNavigationPage = true,
                        NoHistory = true
                    };
                }
            }

            return null;
        }

        protected override bool OnBackButtonPressed()
        {
            var vm = (JobOrderViewModel)DataContext;

            vm.GoToList.Execute();

            return true;
        }
    }
}