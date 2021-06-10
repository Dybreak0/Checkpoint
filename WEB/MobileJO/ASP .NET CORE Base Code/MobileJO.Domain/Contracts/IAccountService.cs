using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Account;
using MobileJO.Data.ViewModels.Common;
using System.Linq;

namespace MobileJO.Domain.Contracts
{
    public interface IAccountService
    {
        AccountViewModel Find(int id);
        ListViewModel Search(AccountSearchViewModel searchModel);
        void Create(Account account);
        void Update(Account account);
        void Delete(int id, int updatedBy);
        bool IsAccountExists(string name);
        bool HasPendingJO(int id);
        Account FindAccount(string accountName);
        int CreateTempAccount(Account account);
    }
}
