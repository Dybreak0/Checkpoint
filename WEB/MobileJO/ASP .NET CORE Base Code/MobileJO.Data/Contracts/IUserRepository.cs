using MobileJO.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Common;

namespace MobileJO.Data.Contracts
{
    public interface IUserRepository
    {
        //User Search and CRUD methods
        ListViewModel FindAll(UserSearchViewModel searchModel);
        User Find(int userID);
        void Create(User user);
        void Update(User user);
        void Delete(int id, string userName);
        bool IsUserExists(string userName);
        bool HasPendingJO(int userID);
        User FindUser(string UserName);
        User FindUserByEmail(string email);
        bool CanLogin(int id);
        //User Registration and Authentication
        Task<User> FindUserAsync(string UserName, string password);
        Task<IdentityResult> RegisterUser(User user);
        Task<IdentityResult> UpdatePassword(User user);
        void RegisterInitialUser(User user);
    }
}
