using Plugin.Connectivity;

namespace MobileJO.Core.Models
{
    public class NetworkCheck
    {
        public static bool HasInternet()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}