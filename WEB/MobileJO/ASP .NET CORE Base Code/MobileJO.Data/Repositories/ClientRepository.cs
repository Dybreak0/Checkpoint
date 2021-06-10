using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using System.Linq;

namespace MobileJO.Data.Repositories
{
    public class ClientRepository : BaseRepository, IClientRepository
    {
        public ClientRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Client FindClient(string clientId)
        {
            return GetDbSet<Client>().FirstOrDefault(x => x.ClientID == clientId);
        }
    }
}
