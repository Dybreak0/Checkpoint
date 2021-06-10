using dotMorten.Xamarin.Forms;
using MobileJO.Core.Base;
using MobileJO.Core.Models;
using MobileJO.Core.ViewModels;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MobileJO.Core.Views
{
    public partial class EditJOFirstPage : BaseContentPage
    {
        public ObservableCollection<Account> AccountsDDL { get; private set; } = new ObservableCollection<Account>();
        public AutoSuggestBox box;
        public EditJOFirstViewModel editJOFirstViewModel;

        public EditJOFirstPage()
        {
            InitializeComponent();

            //initialize only if needed Activity Indicator
            var tempContent = Content;

            //Assign content to null to fix bug for iOS
            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);
        }

        private void SuggestBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            box = (AutoSuggestBox)sender;
            var text = box.Text;

            editJOFirstViewModel = (EditJOFirstViewModel)DataContext;
            if (editJOFirstViewModel == null) return;

            AccountsDDL = editJOFirstViewModel.AccountsDDL;

            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrWhiteSpace(box.Text))
                {
                    box.ItemsSource = null;
                    editJOFirstViewModel.SelectedAccount = null;
                }
                else
                {
                    var suggestions = AccountsDDL.Where(x => CultureInfo.CurrentCulture.CompareInfo.IndexOf(x.Name, box.Text, CompareOptions.IgnoreCase) >= 0);
                    box.ItemsSource = suggestions.ToList();
                }
            }
        }

        private void SuggestBox_QuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            if (editJOFirstViewModel == null) return;

            if (e.ChosenSuggestion == null)
            {
                editJOFirstViewModel.SelectedAccount = null;
            }
            else
            {
                editJOFirstViewModel.SelectedAccount = (Account)e.ChosenSuggestion;
            }
        }
    }
}
