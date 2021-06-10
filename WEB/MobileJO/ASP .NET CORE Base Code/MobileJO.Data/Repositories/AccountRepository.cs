using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.Account;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MobileJO.Data.Repositories
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        ///     Constructor for IUnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public AccountRepository(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager) : base(unitOfWork)
        {
            _userManager = userManager;
        }

        /// <summary>
        ///     Used to retrieve the list of accounts depending on the search filters from client side.
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public ListViewModel FindAll(AccountSearchViewModel searchModel)
        {

            var accounts = GetDbSet<Account>()
                .Where(x => (x.IsActive == true) &&
                            (string.IsNullOrEmpty(searchModel.Name) || x.Name.Contains(Convert.ToString(searchModel.Name))))
                .OrderBy(x => x.Name);

            if (searchModel.PageSize == 0)
                searchModel.PageSize = 1;
            var totalCount = accounts.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / searchModel.PageSize);

            var results = accounts.Skip(searchModel.PageSize * (searchModel.Page - 1))
                .Take(searchModel.PageSize)
                .AsEnumerable()
                .Select(account => new {
                    id = account.ID,
                    name = account.Name,
                    address = account.Address,
                    contact_person = account.ContactPerson,
                    contact_number = account.ContactNo,
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
        ///     Used to retrieve an account's details by account ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Account Find(int id)
        {
            Account account = null;
            if (id > 0)
            {
                account = GetDbSet<Account>()
                .Include(uc => uc.UserCreatedBy)
                .Include(uu => uu.UserUpdatedBy)
                .Where(x => x.ID == id).FirstOrDefault();
            }

            return account;
        }

        /// <summary>
        ///     Used to create a new account record.
        /// </summary>
        /// <param name="account"></param>
        public void Create(Account account)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                account.CreatedDate = DateTime.Now;

                GetDbSet<Account>().Add(account);
                UnitOfWork.SaveChanges();

                transaction.Commit();
            }
        }

        /// <summary>
        ///     Used to update an account record.
        /// </summary>
        /// <param name="account"></param>
        public void Update(Account account)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                var accountDb = Find(account.ID);
                accountDb.Memo = account.Memo;
                accountDb.EmailAddress = account.EmailAddress;
                accountDb.Address = account.Address;
                accountDb.ContactPerson = account.ContactPerson;
                accountDb.ContactNo = account.ContactNo;
                accountDb.UpdatedBy = account.UpdatedBy;
                accountDb.UpdatedDate = DateTime.Now;
                UnitOfWork.SaveChanges();

                transaction.Commit();
            }
        }

        /// <summary>
        ///     Used to delete an account record.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id, int updatedBy)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                var accountDb = Find(id);
                accountDb.IsActive = false;
                accountDb.UpdatedBy = updatedBy;
                accountDb.UpdatedDate = DateTime.Now;
                UnitOfWork.SaveChanges();

                transaction.Commit();
            }
           
        }

        /// <summary>
        ///     Used to check if an account has pending Job Orders.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasPendingJO(int id)
        {
            bool result = false;

            if (id > 0)
            {
                result = GetDbSet<JobOrder>().Any(x => (x.AccountID == id) && 
                                                 (x.Status.Status == Constants.Common.Pending));
            }

            return result;
        }

        /// <summary>
        ///     Used to check if an account exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsAccountExists(string name)
        {
            bool result = false;

            if (string.IsNullOrEmpty(name) == false)
            {
                result = GetDbSet<Account>().Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                                                        x.IsActive == true);
            }

            return result;
        }
        /// <summary>
        ///     Finds account from Accounts
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public Account FindAccount(string accountName)
        {
            var result = GetDbSet<Account>().Where(x => x.Name.ToLower().Equals(accountName.ToLower())).AsNoTracking().FirstOrDefault();
            return result;
        }

        /// <summary>
        ///     Used to create a new account record.
        /// </summary>
        /// <param name="account"></param>
        public int CreateTempAccount(Account account)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    GetDbSet<Account>().Add(account);
                    UnitOfWork.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    var errMsg = e.InnerException.Message;
                    transaction.Rollback();
                }
            }

            return account.ID;
        }
    }
}
