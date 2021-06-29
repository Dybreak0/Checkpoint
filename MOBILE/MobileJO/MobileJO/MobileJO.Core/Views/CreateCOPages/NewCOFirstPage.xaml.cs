using dotMorten.Xamarin.Forms;
using MobileJO.Core.Base;
using MobileJO.Core.Models;
using MobileJO.Core.ViewModels;
using MobileJO.Core.ViewModels.CreateCOViewModels;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MobileJO.Core.Views.CreateCOPages
{
    public partial class NewCOFirstPage : BaseContentPage
    {
        public NewCOFirstPage()
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
