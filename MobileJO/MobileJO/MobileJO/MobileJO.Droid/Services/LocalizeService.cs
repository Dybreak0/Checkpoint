using MobileJO.Core.Contracts;
using MobileJO.Core.Utilities;
using System.Diagnostics;
using System.Globalization;

namespace MobileJO.Droid.Services
{
    public class LocalizeService : ILocalizeService
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netLanguage = androidLocale.ToString().Replace(Constants.SpecialCharacters.Underscore, Constants.SpecialCharacters.Dash); // turns pt_BR into pt-BR
            try
            {
                return new CultureInfo(netLanguage);
            }
            catch (CultureNotFoundException e)
            {
                Debug.WriteLine(e.Message);
            }

            return new CultureInfo(Constants.CultureInfo.English);
        }
    }
}