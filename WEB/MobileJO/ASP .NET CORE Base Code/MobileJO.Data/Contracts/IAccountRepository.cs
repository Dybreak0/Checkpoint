using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.Account;
using System.Linq;
using System.Threading.Tasks;

namespace MobileJO.Data.Contracts
{
    public interface IAccountRepository
    {
        Account Find(int id);
        ListViewModel FindAll (AccountSearchViewModel searchModel);
        void Create(Account account);
        void Update(Account account);
        void Delete(int id, int updatedBy);
        bool IsAccountExists(string name);
        bool HasPendingJO(int id);
        Account FindAccount(string accountName);
        int CreateTempAccount(Account account);
    }
}
