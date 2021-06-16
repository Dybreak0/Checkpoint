using MobileJO.Core.Base;
using MobileJO.Core.ViewModels;
using MobileJO.Core.ViewModels.Common;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;

namespace MobileJO.Core.Views
{
	public partial class SearchPage : BaseContentPage
	{
        public SearchPage()
        {
            InitializeComponent();

            //BindingContext = new SearchViewModel();
            //initialize only if needed Activity Indicator
            var tempContent = Content;

            //Assign content to null to fix bug for iOS
            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);

          
        }
    }
  
}