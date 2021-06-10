using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Domain.Contracts;

namespace MobileJO.Domain.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public Client FindClient(string clientId)
        {
            return _clientRepository.FindClient(clientId);
        }
    }
}
