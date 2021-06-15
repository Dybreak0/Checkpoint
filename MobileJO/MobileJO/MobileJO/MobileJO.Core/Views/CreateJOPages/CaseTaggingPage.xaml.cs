using MobileJO.Core.Base;
using System.Collections;
using System.Collections.Generic;

namespace MobileJO.Core.Views
{
    public partial class CaseTaggingPage : BaseContentPage
    {
        public CaseTaggingPage()
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
