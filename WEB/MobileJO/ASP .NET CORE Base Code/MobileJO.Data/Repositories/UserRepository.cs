using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels;
using System.Collections.Generic;

namespace MobileJO.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private UserManager<IdentityUser> _userManager;

        /// <summary>
        ///     Constructor for IUnitOfWork and IMapper
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="userManager"></param>
        public UserRepository(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager) 
            : base(unitOfWork)
        {
            _userManager = userManager;
        }

        /// <summary>
        ///     Used to retrieve the list of users depending on the search filters from client side
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public ListViewModel FindAll(UserSearchViewModel searchModel)
        {
            var users = GetDbSet<User>()
                 .Include(x => x.Company)
                 .Where(x => (x.IsActive == true) &&
                             (string.IsNullOrEmpty(searchModel.UserName) || x.UserName.Contains(Convert.ToString(searchModel.UserName))) &&
                             (string.IsNullOrEmpty(searchModel.RoleID) || x.RoleID == Convert.ToInt32(searchModel.RoleID)) &&
                             (string.IsNullOrEmpty(searchModel.CompanyID) || x.CompanyID == Convert.ToInt32(searchModel.CompanyID)))
                             .OrderBy(x => x.UserName);

            if (searchModel.Page == 0)
                searchModel.Page = 1;
            var totalCount = users.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / searchModel.PageSize);

            var results = users.Skip(searchModel.PageSize * (searchModel.Page - 1))
                .Take(searchModel.PageSize)
                .AsEnumerable()
                .Select(user => new {
                    id = user.ID,
                    user_name = user.UserName,
                    first_name = user.FirstName,
                    last_name = user.LastName,
                    role_id = user.RoleID,
                    company_name = user.Company.CompanyName
                })
                .ToList();

            var pagination = new
            {
                pages = totalPages,
                size = totalCount
            };

            return new ListViewModel { Pagination = pagination, Data = results };
        }
    

        /// <summary>
        ///     Used to retrieve a user's details by user's id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User Find(int id)
        {
            User user = null;
            if (id > 0)
            {
                user = GetDbSet<User>().Find(id);
            }

            return user;
        }

        /// <summary>
        ///     Used to create a new user record.
        /// </summary>
        /// <param name="user"></param>
        public void Create(User user)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                user.CreatedDate = DateTime.Now;

                GetDbSet<User>().Add(user);
                UnitOfWork.SaveChanges();

                transaction.Commit();
            }
           
        }

        /// <summary>
        ///     Used to update a user record.
        /// </summary>
        /// <param name="user"></param>
        public void Update(User user)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                var userDb = Find(user.ID);
                userDb.FirstName = user.FirstName;
                userDb.LastName = user.LastName;
                userDb.RoleID = user.RoleID;
                userDb.UserTypeID = user.UserTypeID;
                userDb.CompanyID = user.CompanyID;
                userDb.BranchID = user.BranchID;
                userDb.AllowedToLogin = user.AllowedToLogin;
                userDb.Memo = user.Memo;
                userDb.Address = user.Address;
                userDb.EmailAddress = user.EmailAddress;
                userDb.TelephoneNo = user.TelephoneNo;
                userDb.MobileNo = user.MobileNo;
                userDb.UpdatedBy = user.UpdatedBy;
                userDb.UpdatedDate = DateTime.Now;
                UnitOfWork.SaveChanges();

                transaction.Commit();
            }

            
        }

        /// <summary>
        ///     Used to delete a user record.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id, string userName)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                var userDb = Find(id);
                userDb.UpdatedBy = userName;
                userDb.UpdatedDate = DateTime.Now;
                userDb.IsActive = false;
                UnitOfWork.SaveChanges();

                transaction.Commit();
            }
           
        }

        /// <summary>
        ///     Used to check if a user exists.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsUserExists(string userName)
        {
            bool result = false;

            if (string.IsNullOrEmpty(userName) == false)
            {
                result = GetDbSet<User>().Any(x => x.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            }

            return result;
        }

        /// <summary>
        ///     Used to check if a user has pending job orders.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool HasPendingJO(int userID)
        {
            bool result = false;
            User user = Find(userID);

            if (userID > 0)
            {
                result = GetDbSet<JobOrder>().Any(x => (x.CreatedBy == user.ID) &&
                                                 (x.Status.Status == Constants.Common.Pending));
            }

            return result;
        }

        /// <summary>
        ///     Finds user from Users
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User FindUser(string userName)
        {
            var userDB = GetDbSet<User>().Where(x => x.UserName.ToLower().Equals(userName.ToLower())).AsNoTracking().FirstOrDefault();
            return userDB;
        }

        /// <summary>
        ///     Authenticates user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> FindUserAsync(string userName, string password)
        {
            var userDB = GetDbSet<User>().Where(x => x.UserName.Equals(userName, StringComparison.Ordinal)).AsNoTracking().FirstOrDefault();
            var user = await _userManager.FindByNameAsync(userName);
            var isPasswordOK = await _userManager.CheckPasswordAsync(user, password);
            if ((user == null) || (isPasswordOK == false))
            {
                userDB = null;
            }
            return userDB;
        }

        /// <summary>
        ///     Registers user to ASPNetIdenity
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterUser(User user)
        {
            var userIdentity = new IdentityUser
            {
                UserName = user.UserName,
                Email = user.EmailAddress
            };
            var result = await _userManager.CreateAsync(userIdentity, Constants.User.defaultPassword);
            var newPassword = _userManager.PasswordHasher.HashPassword(userIdentity, user.Password);

            userIdentity.PasswordHash = newPassword;

            if (result.Succeeded)
            {
                var userId = userIdentity.Id;

                if (user.RoleID == Constants.User.AdminID)
                {
                    result = await _userManager.AddToRoleAsync(userIdentity, Constants.User.Administrator);
                }
                var userEntity = new User
                {
                    UserID = userId,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleID = user.RoleID,
                    AllowedToLogin = user.AllowedToLogin,
                    Memo = user.Memo,
                    Address = user.Address,
                    EmailAddress = user.EmailAddress,
                    TelephoneNo = user.TelephoneNo,
                    CompanyID = user.CompanyID,
                    BranchID = user.BranchID,
                    UserTypeID = user.UserTypeID,
                    MobileNo = user.MobileNo,
                    CreatedBy = user.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                Create(userEntity);
            }
            return result;
        }

        /// <summary>
        ///     Updates registered user password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityResult> UpdatePassword(User user)
        {
            var userIdentity = await _userManager.FindByNameAsync(user.UserName);
            var newPassword = _userManager.PasswordHasher.HashPassword(userIdentity, user.Password);

            userIdentity.PasswordHash = newPassword;

            return await _userManager.UpdateAsync(userIdentity);
            
        }

        public bool CanLogin(int id)
        {
            var allowed = GetDbSet<User>()
                            .Any(x => x.ID == id && x.AllowedToLogin == true);

            return (allowed) ? true : false;
        }

        /// <summary>
        ///     Registers user to ASPNetIdenity
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public void RegisterInitialUser(User user)
        {
            var userIdentity = new IdentityUser
            {
                UserName = user.UserName,
                Email = user.EmailAddress
            };

            var result = _userManager.CreateAsync(userIdentity, user.Password).Result;

            if (result.Succeeded)
            {
                var userId = userIdentity.Id;

                if (user.RoleID == Constants.User.AdminID)
                {
                    _userManager.AddToRoleAsync(userIdentity, Constants.User.Administrator).Wait();
                }
                var userEntity = new User
                {
                    UserID = userId,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleID = user.RoleID,
                    AllowedToLogin = user.AllowedToLogin,
                    Memo = user.Memo,
                    Address = user.Address,
                    EmailAddress = user.EmailAddress,
                    TelephoneNo = user.TelephoneNo,
                    CompanyID = user.CompanyID,
                    BranchID = user.BranchID,
                    UserTypeID = user.UserTypeID,
                    MobileNo = user.MobileNo,
                    CreatedBy = user.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                Create(userEntity);
            }
        }

        public User FindUserByEmail(string email)
        {
            return  GetDbSet<User>().Where(x => x.EmailAddress.ToLower().Equals(email.ToLower()) && x.IsActive).AsNoTracking().FirstOrDefault();
        }
    }
   
}
