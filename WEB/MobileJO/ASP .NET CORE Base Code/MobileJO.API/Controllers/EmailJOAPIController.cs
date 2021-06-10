using System;
using AutoMapper;
using MobileJO.Data;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using Constants = MobileJO.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using MobileJO.Data.ViewModels.EmailJO;
using System.Security.Claims;

namespace MobileJO.API.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.Common.Bearer, Roles = Constants.Roles.Administrator)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailJOAPIController : ControllerBase
    {
        private readonly IEmailJOService _emailJOService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public DbContext context { get; private set; }

        /// <summary>
        ///     Constructor for IEmailJOService and IMapper
        /// </summary>
        /// <param name="emailJOService"></param>
        /// <param name="mapper"></param>
        public EmailJOAPIController(IEmailJOService emailJOService, IMapper mapper, IUserService userService)
        {
            _emailJOService = emailJOService;
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Handles web-api calls to get list of email addresses.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("list")]
        public HttpResponseMessage GetEmails()
        {
            var responseCode = new HttpStatusCode();
            var responseData = new object();

            try
            {
                var result = _emailJOService.GetAll();

                if (result == null)
                {
                    responseData = new { message = Constants.Common.NoResults };
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
        ///     Handles web-api calls to save email addresses
        /// </summary>
        /// <param name="emailSetupModel">Contains Email properties</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("save")]
        public HttpResponseMessage PostEmail(EmailSetupViewModel emailSetupModel)
        {
            var responseCode = new HttpStatusCode();
            var responseData = new object();

            var email = _mapper.Map<EmailSetup>(emailSetupModel);

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var name = claimsIdentity.FindFirst(Constants.ClaimTypes.UserName).Value;
            int id = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

            int emailCount = _emailJOService.EmailCount();

            var userDetails = _userService.Find(id);

            if (userDetails.IsActive == false)
            {
                responseCode = HttpStatusCode.OK;
                responseData = new { message = Constants.Common.deletedUser };
            }
            else if(emailSetupModel.emailViewModel.FindAll(x => x.TypeID == Constants.Common.EmailType.To).Count > Constants.Common.MaxEmailCount
                    || emailSetupModel.emailViewModel.FindAll(x => x.TypeID == Constants.Common.EmailType.Cc).Count > Constants.Common.MaxEmailCount
                    || emailSetupModel.emailViewModel.FindAll(x => x.TypeID == Constants.Common.EmailType.Bcc).Count > Constants.Common.MaxEmailCount)
            {
                responseCode = HttpStatusCode.OK;
                responseData = new { message = Constants.Common.MaximumEmailReached };
            }
            else if (userDetails.RoleID == 1)
            {
                try
                {
                    _emailJOService.SaveEmails(JsonConvert.SerializeObject(emailSetupModel));
                    responseData = new { message = Constants.Common.SuccessSave };
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
        ///     Handles web-api calls to send email
        /// </summary>
        /// <param name="emailDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("send")]
        public HttpResponseMessage SendEmailJO(EmailJOViewModel emailDetails)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                int userID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);
                string senderFullname = claimsIdentity.FindFirst(Constants.ClaimTypes.FullName).Value;

                _emailJOService.SendEmailJO(emailDetails, userID, senderFullname);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }
        /// <summary>
        ///     Handles web-api calls to retrieve tagged job orders
        /// </summary>
        /// <param name="createdBy"></param>
        /// <param name="caseID"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("tagged_jo")]
        public HttpResponseMessage GetTaggedCases(int createdBy, int caseID)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _emailJOService.GetTaggedCases(createdBy, caseID);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }


    }
}
