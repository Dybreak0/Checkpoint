using MobileJO.Data.Models;
using MobileJO.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Constants = MobileJO.Data.Constants;

namespace MobileJO.Domain.Handlers
{
    public class UserHandler
    {
        private readonly IUserService _userService;

        public UserHandler(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        ///     Determines if User can be added
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> CanAdd(User user)
        {
            var validationErrors = new List<ValidationResult>();

            if (user != null)
            {
                if (_userService.IsUserExists(user.UserName))
                {
                    validationErrors.Add(new ValidationResult(Constants.User.UserExist));
                }
            }
            else
            {
                validationErrors.Add(new ValidationResult(Constants.Common.RecordInvalid));
            }

            return validationErrors;
        }
        /// <summary>
        ///     Determines if yser can be updated
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> CanUpdate(User user)
        {
            var validationErrors = new List<ValidationResult>();

            if (user != null)
            {
                var userDb = _userService.Find(user.ID);

                if ((userDb != null) && (userDb.IsActive == true))
                {
                    if (!userDb.UserName.Equals(user.UserName) && _userService.IsUserExists(user.UserName))
                    {
                        validationErrors.Add(new ValidationResult(Constants.User.UserExist));
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
        ///     Determines if user can be deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> CanDelete(int id)
        {
            var validationErrors = new List<ValidationResult>();
            var user = _userService.Find(id);
            if (user != null)
            {
                if ((id > 0)  && (user.IsActive == true))
                {
                    bool hasPendingJO = _userService.HasPendingJO(id);

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
