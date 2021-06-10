using MobileJO.Data.Models;
using MobileJO.Domain.Contracts;
using System.Collections.Generic;
using Constants = MobileJO.Data.Constants;

namespace MobileJO.Domain.Handlers
{
    public class AccountHandler
    {
        private readonly IAccountService _accountService;

        public AccountHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        ///     Determines if account can be added
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> CanAdd(Account account)
        {
            var validationErrors = new List<ValidationResult>();

            if (account != null)
            {
                if (_accountService.IsAccountExists(account.Name))
                {
                    validationErrors.Add(new ValidationResult(Constants.Account.AccountExist));
                }
            }
            else
            {
                validationErrors.Add(new ValidationResult(Constants.Common.RecordInvalid));
            }

            return validationErrors;
        }

        /// <summary>
        ///     Determines if account can be deleted
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> CanUpdate(Account account)
        {
            var validationErrors = new List<ValidationResult>();

            if (account != null)
            {
                var accountDb = _accountService.Find(account.ID);

                if ((accountDb != null) && (accountDb.IsActive == true))
                {
                    if (!accountDb.Name.Equals(account.Name) && _accountService.IsAccountExists(account.Name))
                    {
                        validationErrors.Add(new ValidationResult(Constants.Account.AccountExist));
                    }
                }
                else
                {
                    validationErrors.Add(new ValidationResult(Constants.Common.RecordNotExist));
                }
            }
            else
            {
                validationErrors.Add(new ValidationResult(Constants.Common.RecordInvalid));
            }

            return validationErrors;
        }

        /// <summary>
        ///     Determines if account can be deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> CanDelete(int id)
        {
            var validationErrors = new List<ValidationResult>();

            if (id > 0) {
                var account = _accountService.Find(id);

                if ((account != null) && (account.IsActive == true))
                {
                    bool hasPendingJO = _accountService.HasPendingJO(id);

                    if (hasPendingJO)
                    {
                        validationErrors.Add(new ValidationResult(Constants.Common.RecordHasPendingJOs));
                    }
                }
                else
                {
                    validationErrors.Add(new ValidationResult(Constants.Common.RecordNotExist));
                }
            }
            else
            {
                validationErrors.Add(new ValidationResult(Constants.Common.RecordInvalid));
            }

            return validationErrors;
        }
    }
}
