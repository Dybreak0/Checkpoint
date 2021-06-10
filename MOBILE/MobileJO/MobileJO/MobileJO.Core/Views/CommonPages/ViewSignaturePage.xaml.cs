using Xamarin.Forms.Xaml;
using MobileJO.Core.Base;
using MobileJO.Core.ViewModels;

namespace MobileJO.Core.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewSignaturePage : BaseContentPage
	{
		public ViewSignaturePage ()
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
            var vm = (ViewSignatureViewModel)DataContext;

            vm.CloseCommand.Execute();

            return true;
        }
    }
}