using MobileJO.Data.Models;

namespace MobileJO.Domain.Contracts
{
    public interface IClientService
    {
        Client FindClient(string clientId);
    }
}
