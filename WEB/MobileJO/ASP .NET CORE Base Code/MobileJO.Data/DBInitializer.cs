using Microsoft.AspNetCore.Identity;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using System;

namespace MobileJO.Data
{
    public class DBInitializer
    {
        private readonly IUserRepository _userRepository;
        public DBInitializer(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void SeedIdentityUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {            
                User user = new User
                {
                    UserName = "admin",
                    EmailAddress = "admin3@email.com",
                    ID = 1,
                    UserID = "",
                    Password = "Alliance@12345",
                    FirstName = "Alliance Checkpoint",
                    LastName = "Admin",
                    RoleID = 1,
                    Memo = "",
                    AllowedToLogin = true,
                    IsActive = true,
                    Address = "Alliance Software Inc., Buildcomm Center, Sumilon Road, Cebu Business Park, Cebu City",
                    TelephoneNo = "238-6595",
                    MobileNo = "09123456789",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    UpdatedBy = "admin",
                    UpdatedDate = DateTime.Now,
                    CompanyID = 1,
                    UserTypeID = 1,
                    BranchID = 1
                };

                _userRepository.RegisterInitialUser(user);
            }
        }
    }
}
