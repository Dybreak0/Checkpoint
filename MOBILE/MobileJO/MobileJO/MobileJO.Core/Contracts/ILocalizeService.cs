using System.Globalization;

namespace MobileJO.Core.Contracts
{
    public interface ILocalizeService
    {
        CultureInfo GetCurrentCultureInfo();
    }
}
