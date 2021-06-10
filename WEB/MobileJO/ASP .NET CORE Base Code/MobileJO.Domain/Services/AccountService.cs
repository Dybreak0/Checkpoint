using System.Linq;
using AutoMapper;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Account;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Domain.Contracts;


namespace MobileJO.Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Constructor for IAccountRepository and IMapper
        /// </summary>
        /// <param name="accountRepository"></param>
        /// <param name="mapper"></param>
        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        /// <summary>
        ///     Calls Account repository method FindAll().
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public ListViewModel Search(AccountSearchViewModel searchModel)
        {
            return _accountRepository.FindAll(searchModel);
        }

        /// <summary>
        ///     Calls Account repository method Find().
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccountViewModel Find(int id)
        {
            AccountViewModel accountViewModel = null;
        
            if (id > 0)
            {
                var account = _accountRepository.Find(id);

                if (account != null)
                {
                    accountViewModel = _mapper.Map<AccountViewModel>(account);
                }
            }

            return accountViewModel;
        }

        /// <summary>
        ///     Calls Account repository method Create().
        /// </summary>
        /// <param name="account"></param>
        public void Create(Account account)
        {
            _accountRepository.Create(account);
        }

        /// <summary>
        ///     Calls Account repository method Update().
        /// </summary>
        /// <param name="account"></param>
        public void Update(Account account)
        {
            _accountRepository.Update(account);
        }

        /// <summary>
        ///     Calls Account repository method Delete().
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id, int updatedBy)
        {
            _accountRepository.Delete(id, updatedBy);
        }

        /// <summary>
        ///     Calls Account repository method  IsAccountExists().
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsAccountExists(string name)
        {
            bool result = false;

            if (name != null)
            {
                result = _accountRepository.IsAccountExists(name);
            }

            return result;
        }

        /// <summary>
        ///     Calls Account repository method HasPendingJO().
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasPendingJO(int id)
        {
            bool result = false;

            if (id > 0)
            {
                result = _accountRepository.HasPendingJO(id);
            }

            return result;
        }

        /// <summary>
        ///     Calls User repository method FindAccount().
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public Account FindAccount(string accountName)
        {
            return _accountRepository.FindAccount(accountName);
        }

        /// <summary>
        ///     Calls Account repository method Create().
        /// </summary>
        /// <param name="account"></param>
        public int CreateTempAccount(Account account)
        {
           return _accountRepository.CreateTempAccount(account);           
        }

    }
}
