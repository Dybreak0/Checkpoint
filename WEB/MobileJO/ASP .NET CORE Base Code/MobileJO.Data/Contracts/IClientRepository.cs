using MobileJO.Data.Models;

namespace MobileJO.Data.Contracts
{
    public interface IClientRepository
    {
        Client FindClient(string clientId);
    }
}
