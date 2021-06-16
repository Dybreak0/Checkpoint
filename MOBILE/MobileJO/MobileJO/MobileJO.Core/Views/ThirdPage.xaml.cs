using MobileJO.Core.Base;

namespace MobileJO.Core.Views
{
	
	public partial class ThirdPage : BaseContentPage
	{
		public ThirdPage ()
		{
			InitializeComponent ();

            var tempContent = Content;

            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopAsync();

            return true;
        }
    }
}