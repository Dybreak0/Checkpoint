using AutoMapper;
using MobileJO.Data;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Domain.Contracts;
using MobileJO.Domain.Handlers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Constants = MobileJO.Data.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace MobileJO.API.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.Common.Bearer, Roles = Constants.Roles.Administrator)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        /// <summary>
        ///     Constructor for IUserService and IMapper
        /// </summary>
        /// <param name="accountService"></param>
        /// <param name="mapper"></param>
        public UserAPIController(IUserService userService, IMapper mapper,IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        ///    Handles web-api calls to get list of users.
        /// </summary>
        /// <param name="searchModel">Search filters for finding User records</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        [ActionName("list")]
        public HttpResponseMessage GetUserList([FromQuery] UserSearchViewModel searchModel)
        {
            var responseCode = new HttpStatusCode();
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            int userTypeID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.UserTypeID).Value);

            try
            {
                string queryString = (Request.QueryString).ToString();

                if (!queryString.Contains(Constants.Common.RoleID))
                {
                    searchModel.RoleID = null;
                }

                if (!queryString.Contains(Constants.Common.CompanyID))
                {
                    searchModel.CompanyID = null;
                }

                if ((int)Constants.UserType.SuperAdmin != userTypeID)
                {
                    searchModel.CompanyID = claimsIdentity.FindFirst(Constants.ClaimTypes.CompanyID).Value;
                }

                var result = _userService.Search(searchModel);

                responseData = new { message = Constants.Common.NoResults };

                if (result != null)
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
        ///     Handles web-api calls to get user details.
        /// </summary>
        /// <param name="userID">ID of the user record</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("user")]
        public HttpResponseMessage GetUserDetails(int id)
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
                        var result = _userService.Find(id);

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
        ///     Handles web-api calls to create a new user record.
        /// </summary>
        /// <param name="userViewModel">Contains user properties</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("add")]
        public async Task<HttpResponseMessage> PostUserAsync(UserViewModel userViewModel)
        {
            var responseCode = new HttpStatusCode();
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var name = claimsIdentity.FindFirst(Constants.ClaimTypes.UserName).Value;
            int id = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

            var userDetails = _userService.Find(id);


            if (userDetails.IsActive == false)
            {
                responseCode = HttpStatusCode.OK;
                responseData = new { message = Constants.Common.deletedUser };
            }
            else if (userDetails.RoleID == 1)
            {
                try
                {
                    var user = _mapper.Map<User>(userViewModel);

                    //Claim roleID = claimsIdentity.FindFirst(Constants.Claims.Role);

                    var validationResults = new UserHandler(_userService).CanAdd(user);

                    if (validationResults.Any())
                    {
                        ModelState.AddModelErrors(validationResults);
                    }

                    if (ModelState.IsValid)
                    {
                        string userName = User.FindFirst(ClaimTypes.Name)?.Value;
                        string role = User.FindFirst(ClaimTypes.Role)?.Value;
                        user.CreatedBy = userName;

                        var result = await _userService.RegisterUser(user);
                        var errorResult = Helper.GetIdentityErrorResult(result, ModelState);

                        if (result.Succeeded)
                        {
                            responseData = new { message = Constants.Common.SuccessSave };
                        }
                        else
                        {
                            responseData = new { message = Constants.Common.ErrorSave };
                        }

                    }
                    else
                    {
                        responseData = new { message = Helper.GetModelStateErrors(ModelState) };
                    }

                    responseCode = HttpStatusCode.OK;
                    Helper.SendConfirmationEmail(_configuration[Constants.ConfirmationEmailConfiguration.EmailRecipient],
                        _configuration[Constants.ConfirmationEmailConfiguration.EmailCredential],
                        _configuration[Constants.ConfirmationEmailConfiguration.EmailSubjectBrandName],
                        user);
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
        ///     Handles web-api calls to update a user record.
        /// </summary>
        /// <param name="userViewModel">Contains user properties</param>
        /// <returns></returns>
        [HttpPut]
        [ActionName("edit")]
        public async Task<HttpResponseMessage> PutUserAsync(UserEditViewModel userViewModel)
        {
            var responseCode = new HttpStatusCode();
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var name = claimsIdentity.FindFirst(Constants.ClaimTypes.UserName).Value;
            int id = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

            var userDetails = _userService.Find(id);

            if (userDetails.IsActive == false)
            {
                responseCode = HttpStatusCode.OK;
                responseData = new { message = Constants.Common.deletedUser };
            }
            else if (userDetails.RoleID == 1)
            {
                try
                {
                    var user = _mapper.Map<User>(userViewModel);
                    var oldUser = _userService.Find(user.ID);
                    var validationResults = new UserHandler(_userService).CanUpdate(user);

                    if (validationResults.Any())
                    {
                        ModelState.AddModelErrors(validationResults);
                    }

                    if (ModelState.IsValid)
                    {
                        if (string.IsNullOrEmpty(user.Password))
                        {
                            _userService.Update(user);
                            responseData = new { message = Constants.Common.SuccessSave };
                        }
                        else
                        {
                            string userName = User.FindFirst(ClaimTypes.Name)?.Value;
                            user.UpdatedBy = userName;

                            var result = await _userService.UpdatePassword(user);
                            var errorResult = Helper.GetIdentityErrorResult(result, ModelState);
                            if (result.Succeeded)
                            {
                                _userService.Update(user);
                                responseData = new { message = Constants.Common.SuccessSave };
                            }
                            else
                            {
                                responseData = new { message = Constants.Common.ErrorSave };
                            }
                        }

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
        ///     Handles web-api calls to delete a user.
        /// </summary>
        /// <param name="userID">ID of the user record</param>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("delete")]
        public HttpResponseMessage Delete(int id)
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
                        var validationResults = new UserHandler(_userService).CanDelete(id);
                        if (validationResults.Any())
                        {
                            ModelState.AddModelErrors(validationResults);
                        }

                        if (ModelState.IsValid)
                        {
                            string userName = User.FindFirst(ClaimTypes.Name)?.Value;

                            _userService.Delete(id, userName);
                            responseData = new { message = Constants.Common.SuccessDelete };
                        }
                        else
                        {
                            responseData = new { message = Helper.GetModelStateErrors(ModelState) };
                        }
                    }
                    else
                    {
                        responseData = new { message = Constants.Common.RecordInvalid };
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

        [HttpGet]
        [ActionName("allowed")]
        public bool AllowedLogin(int userID)
        {
            return _userService.CanLogin(userID);
        }
    }
}