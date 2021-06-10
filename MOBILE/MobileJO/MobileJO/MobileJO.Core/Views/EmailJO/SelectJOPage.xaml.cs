using MobileJO.Core.Base;
using Xamarin.Forms.Xaml;

namespace MobileJO.Core.Views.EmailJO
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectJOPage : BaseContentPage
	{
		public SelectJOPage ()
		{
            InitializeComponent();

            //initialize only if needed Activity Indicator
            var tempContent = Content;

            //Assign content to null to fix bug for iOS
            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);

        }

        private void Select_Clicked(object sender, System.EventArgs e)
        {
        }
    }
}