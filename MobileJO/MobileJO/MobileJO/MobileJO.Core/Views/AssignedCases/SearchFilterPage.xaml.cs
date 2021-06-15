using MobileJO.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileJO.Core.Views.AssignedCases
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchFilterPage : BaseContentPage
    {
		public SearchFilterPage ()
		{
            InitializeComponent();

            //initialize only if needed Activity Indicator
            var tempContent = Content;

            //Assign content to null to fix bug for iOS
            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);
        }
	}
}