using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MobileJO.Data;
using MobileJO.Data.ViewModels.EmailJO;
using MobileJO.Data.ViewModels.ForgotPassword;
using MobileJO.Domain.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Constants = MobileJO.Data.Constants;

namespace MobileJO.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ForgotPasswordAPIController : ControllerBase
    {
        private readonly IForgotPasswordService _forgotPasswordService;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Constructor for IUserService and IMapper
        /// </summary>
        /// <param name="accountService"></param>
        /// <param name="mapper"></param>
        public ForgotPasswordAPIController(IForgotPasswordService forgotPasswordService, IMapper mapper)
        {
            _forgotPasswordService = forgotPasswordService;
            _mapper = mapper;
        }
        [HttpPost]
        [ActionName("add")]
        public HttpResponseMessage CreateForgotPasswordRequest(EmailModel emailModel)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();


            try
            {
                if(0 < _forgotPasswordService.sendEmail(emailModel.EmailAddress))
                {
                    responseData = new { message = Constants.Common.ForgetPasswordSent };
                }
                
            }
            
            catch (NullReferenceException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.BadRequest;
                responseData = new { message = ex.Message };
            }


            return Helper.ComposeResponse(responseCode, responseData);
        }

        [HttpPost]
        [ActionName("check_validity")]
        public HttpResponseMessage CheckValidity(ForgotPasswordDetailsViewModel forgotPasswordDetails)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();


            try
            {
                var result = _forgotPasswordService.CheckValidity(forgotPasswordDetails.userId, forgotPasswordDetails.token);

                responseData = new { message = Constants.Common.LinkExpired };

                if (result != null)
                {
                    responseData = new { message = Constants.Common.Success, data = result };
                }
            }

            catch (NullReferenceException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.BadRequest;
                responseData = new { message = ex.Message };
            }


            return Helper.ComposeResponse(responseCode, responseData);
        }

        [HttpPost]
        [ActionName("reset_password")]
        public async Task<HttpResponseMessage> ResetPassword(ForgotPasswordDetailsViewModel forgotPasswordDetails)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();


            try
            {
                    var result = await _forgotPasswordService.resetPassword(forgotPasswordDetails);
                
                    responseData = new { message = Constants.Common.SuccessSave };
            }

            catch (NullReferenceException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.BadRequest;
                responseData = new { message = ex.Message };
            }


            return Helper.ComposeResponse(responseCode, responseData);
        }



    }
}