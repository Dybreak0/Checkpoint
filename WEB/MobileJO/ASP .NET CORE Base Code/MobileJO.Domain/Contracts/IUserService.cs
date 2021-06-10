using MobileJO.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Common;
using System.Collections.Generic;

namespace MobileJO.Domain.Contracts
{
    public interface IUserService
    {
        ListViewModel Search(UserSearchViewModel searchModel);
        UserViewModel Find(int id);
        void Update(User user);
        void Delete(int id, string userName);
        bool IsUserExists(string userName);
        bool HasPendingJO(int id);
        User FindUser(string userName);
        Task<User> FindUserAsync(string userName, string password);
        Task<IdentityResult> RegisterUser(User user);
        Task<IdentityResult> UpdatePassword(User user);

        bool CanLogin(int id);
    }
}
