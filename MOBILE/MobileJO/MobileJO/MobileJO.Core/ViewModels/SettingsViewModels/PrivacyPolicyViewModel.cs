using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;

namespace MobileJO.Core.ViewModels.SettingsViewModels
{
    public class PrivacyPolicyViewModel : BaseViewModel
    {
        public PrivacyPolicyViewModel(IUserDialogs userDialogs, ILocalizeService localizeService) 
            : base(userDialogs, localizeService)
        {
        }
    }
}
