using Xamarin.Essentials;
using MobileJO.Core.Contracts;
using MobileJO.Core.Utilities;

namespace MobileJO.Core.Services
{
    public class AppSettings : IAppSettings
    {
        public string PersistentText
        {
            get => Preferences.Get(Constants.AppSettings.PersistentTextKey, Constants.AppSettings.PersistentTextDefaultValue);
            set => Preferences.Set(Constants.AppSettings.PersistentTextKey, value);
        }

        public string AccessToken
        {
            get => Preferences.Get(Constants.AppSettings.AccessToken, null);
            set => Preferences.Set(Constants.AppSettings.AccessToken, value);
        }

        public string RefreshToken
        {
            get => Preferences.Get(Constants.AppSettings.RefreshToken, null);
            set => Preferences.Set(Constants.AppSettings.RefreshToken, value);
        }

        public string UserID
        {
             get => Preferences.Get(Constants.AppSettings.UserID, null);
             set => Preferences.Set(Constants.AppSettings.UserID, value);
        }

        public string UserName
        {
            get => Preferences.Get(Constants.AppSettings.UserName, null);
            set => Preferences.Set(Constants.AppSettings.UserName, value);
        }

        public int LoginAttempts
        {
            get => Preferences.Get(Constants.AppSettings.LoginAttempts, 0);
            set => Preferences.Set(Constants.AppSettings.LoginAttempts, value);
        }

        public string UserTypeID
        {
            get => Preferences.Get(Constants.AppSettings.UserTypeID, null);
            set => Preferences.Set(Constants.AppSettings.UserTypeID, value);
        }

        public string CompanyID
        {
            get => Preferences.Get(Constants.AppSettings.CompanyID, null);
            set => Preferences.Set(Constants.AppSettings.CompanyID, value);
        }

        public string BranchID
        {
            get => Preferences.Get(Constants.AppSettings.BranchID, null);
            set => Preferences.Set(Constants.AppSettings.BranchID, value);
        }
    }
}
