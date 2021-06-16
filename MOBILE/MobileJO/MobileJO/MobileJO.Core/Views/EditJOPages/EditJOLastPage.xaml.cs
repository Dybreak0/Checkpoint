using MobileJO.Core.Base;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace MobileJO.Core.Views
{
    public partial class EditJOLastPage : BaseContentPage
    {
        public EditJOLastPage()
        {
            InitializeComponent();

            //initialize only if needed Activity Indicator
            var tempContent = Content;

            //Assign content to null to fix bug for iOS
            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);
        }

        private void Rating_TextChanged(object sender, TextChangedEventArgs args)
        {
            Entry entry = sender as Entry;
            string val = entry.Text;

            if (!string.IsNullOrEmpty(val) && !Regex.IsMatch(val, "^[0-9]"))
            {
                val = val.Remove(val.Length - 1);
                entry.Text = string.Empty;
            }
        }
    }
}
