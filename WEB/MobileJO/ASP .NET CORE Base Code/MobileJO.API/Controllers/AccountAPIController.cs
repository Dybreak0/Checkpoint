using AutoMapper;
using MobileJO.Data;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Account;
using MobileJO.Domain.Contracts;
using MobileJO.Domain.Handlers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Constants = MobileJO.Data.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MobileJO.API.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.Common.Bearer, Roles = Constants.Roles.Administrator)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountAPIController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Constructor for IAccountService and IMapper
        /// </summary>
        /// <param name="accountService"></param>
        /// <param name="mapper"></param>
        public AccountAPIController(IAccountService accountService, IMapper mapper, IUserService userService)
        {
            _accountService = accountService;
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        ///    Handles web-api calls to get list of accounts.
        /// </summary>
        /// <param name="searchModel">Search filters for finding Account records</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("list")]
        public HttpResponseMessage GetAccountList([FromQuery] AccountSearchViewModel searchModel)
        {
            var responseCode = new HttpStatusCode();
            var responseData = new object();

            try
            {
                var result = _accountService.Search(searchModel);

                if (result == null)
                {
                    responseData = new { message = Constants.Common.NoResults};
                }
                else
                {
                    responseData = new { message = Constants.Common.Success, data = result };
                }

                responseCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }


        /// <summary>
        ///     Handles web-api calls to get an account's details.
        /// </summary>
        /// <param name="id">ID of the Account record</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("account")]
        public HttpResponseMessage GetAccountDetails(int id)
        {
            var responseCode = new HttpStatusCode();
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var name = claimsIdentity.FindFirst(Constants.ClaimTypes.UserName).Value;
            int userId = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

            var userDetails = _userService.Find(userId);


            if (userDetails.IsActive == false)
            {
                responseCode = HttpStatusCode.OK;
                responseData = new { message = Constants.Common.deletedUser };
            }
            else if (userDetails.RoleID == 1)
            {
                try
                {
                    if (id > 0)
                    {
                        var result = _accountService.Find(id);

                        if (result == null)
                        {
                            responseData = new { message = Constants.Common.NoResults };
                        }
                        else
                        {
                            responseData = new { message = Constants.Common.Success, data = result };
                        }
                    }
                    else
                    {
                        responseData = new { message = Constants.Common.NoResults };
                    }

                    responseCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    Helper.GetErrors(ex, out responseCode, out responseData);
                }
            }

            else
            {
                responseCode = HttpStatusCode.OK;
                responseData = new { message = Constants.Common.NotAdmin };
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }


        /// <summary>
        ///     Handles web-api calls to create a new account record.
        /// </summary>
        /// <param name="accountViewModel">Contains Account properties</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("add")]
        public HttpResponseMessage PostAccount(AccountViewModel accountViewModel)
        {
            var responseCode = new HttpStatusCode();
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var name = claimsIdentity.FindFirst(Constants.ClaimTypes.UserName).Value;
            int userID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

            var userDetails = _userService.Find(userID);


            if (userDetails.IsActive == false)
            {
                responseCode = HttpStatusCode.OK;
                responseData = new { message = Constants.Common.deletedUser };
            }
            else if (userDetails.RoleID == 1)
            {
                try
                {
                    var account = _mapper.Map<Account>(accountViewModel);
                    var validationResults = new AccountHandler(_accountService).CanAdd(account);

                    if (validationResults.Any())
                    {
                        ModelState.AddModelErrors(validationResults);
                    }

                    if (ModelState.IsValid)
                    {
                        string user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        int id = Int32.Parse(user);
                        account.CreatedBy = id;
                        account.UpdatedBy = id;

                        _accountService.Create(account);
                        responseData = new { message = Constants.Common.SuccessSave };
                    }
                    else
                    {
                        responseData = new { message = Helper.GetModelStateErrors(ModelState) };
                    }

                    responseCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    Helper.GetErrors(ex, out responseCode, out responseData);
                }
            }
            else
            {
                responseCode = HttpStatusCode.OK;
                responseData = new { message = Constants.Common.NotAdmin };
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///    Handles web-api calls to update an account record.
        /// </summary>
        /// <param name="accountViewModel">Contains Account properties</param>
        /// <returns></returns>
        [HttpPut]
        [ActionName("edit")]
        public HttpResponseMessage PutAccount(AccountViewModel accountViewModel)
        {
            var responseCode = new HttpStatusCode();
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var name = claimsIdentity.FindFirst(Constants.ClaimTypes.UserName).Value;
            int userID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

            var userDetails = _userService.Find(userID);


            if (userDetails.IsActive == false)
            {
                responseCode = HttpStatusCode.OK;
                responseData = new { message = Constants.Common.deletedUser };
            }
            else if (userDetails.RoleID == 1)
            {
                try
                {
                    var account = _mapper.Map<Account>(accountViewModel);
                    var validationResults = new AccountHandler(_accountService).CanUpdate(account);

                    if (validationResults.Any())
                    {
                        ModelState.AddModelErrors(validationResults);
                    }

                    if (ModelState.IsValid)
                    {
                        string user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        int id = Int32.Parse(user);
                        account.UpdatedBy = id;

                        _accountService.Update(account);
                        responseData = new { message = Constants.Common.SuccessSave };
                    }
                    else
                    {
                        responseData = new { message = Helper.GetModelStateErrors(ModelState) };
                    }

                    responseCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    Helper.GetErrors(ex, out responseCode, out responseData);
                }
            }

            else
            {
                responseCode = HttpStatusCode.OK;
                responseData = new { message = Constants.Common.NotAdmin };
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }


        /// <summary>
        ///     Handles web-api calls to delete an account.
        /// </summary>
        /// <param name="id">ID of the Account record</param>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("delete")]
        public HttpResponseMessage Delete(int id)
        {
            var responseCode = new HttpStatusCode();
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var name = claimsIdentity.FindFirst(Constants.ClaimTypes.UserName).Value;
            int userID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

            var userDetails = _userService.Find(userID);


            if (userDetails.IsActive == false)
            {
                responseCode = HttpStatusCode.OK;
                responseData = new { message = Constants.Common.deletedUser };
            }
            else if (userDetails.RoleID == 1)
            {
                try
                {
                    if (id > 0)
                    {
                        var validationResults = new AccountHandler(_accountService).CanDelete(id);

                        if (validationResults.Any())
                        {
                            ModelState.AddModelErrors(validationResults);
                        }

                        if (ModelState.IsValid)
                        {
                            string user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                            int updatedBy = Int32.Parse(user);

                            _accountService.Delete(id, updatedBy);
                            responseData = new { message = Constants.Common.SuccessDelete };
                        }
                        else
                        {
                            responseData = new { message = Helper.GetModelStateErrors(ModelState) };
                        }
                    }
                    else
                    {
                        responseData = new { message = Constants.Common.ErrorDelete };
                    }

                    responseCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    Helper.GetErrors(ex, out responseCode, out responseData);
                }
            }
            else
            {
                responseCode = HttpStatusCode.OK;
                responseData = new { message = Constants.Common.NotAdmin };
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }
    }
}
