using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileJO.Data;
using MobileJO.Data.ViewModels.RevertJO;
using MobileJO.Domain.Contracts;
using MobileJO.Domain.Handlers;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;

namespace MobileJO.API.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.Common.Bearer, Roles = Constants.Roles.Administrator)]
    [Route(Constants.Common.Route)]
    [ApiController]
    public class RevertJOAPIController : ControllerBase
    {
        private readonly IRevertJOService _revertJOService;
        private readonly IUserService _userService;

        public RevertJOAPIController(IRevertJOService revertJOService, IUserService userService)
        {
            _revertJOService = revertJOService;
            _userService = userService;
        }

        /// <summary>
        ///     Handles RevertJOAPI/getRevertJOList web-api call to get list of job order revert requests
        /// </summary>
        /// <param name="searchModel">holds revert job order request search filters</param>
        /// <returns name="">Represents an HTTP response that includes the status code, data and message</returns>
        [HttpGet]
        [ActionName(Constants.RevertJO.GetRevertJOList)]
        public HttpResponseMessage GetRevertJOList([FromQuery] RevertJOSearchViewModel searchModel)
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                responseData = _revertJOService.SearchRevertJO(searchModel);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles RevertJOAPI/revertJO web-api call to approve/deny a job order revert request
        /// </summary>
        /// <param name="searchModel">Holds job order revert request data</param>
        /// <returns name="">Represents an HTTP response that includes the status code, data and message</returns>
        [HttpPost]
        [ActionName(Constants.RevertJO.RevertJORequest)]
        public HttpResponseMessage RevertJO(RevertJORequestViewModel requestModel)
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claims = User.Identity as ClaimsIdentity;
            var name = claims.FindFirst(Constants.ClaimTypes.UserName).Value;
            int id = Convert.ToInt32(claims.FindFirst(Constants.ClaimTypes.ID).Value);

            var userDetails = _userService.Find(id);

            if (userDetails.IsActive == false)
            {
                responseCode = HttpStatusCode.OK;
                responseData = new { errorMessage = Constants.Common.deletedUser };
            }

            else if (userDetails.RoleID == 1)
            {
                try
               {
                    var validationResult = new RevertJOHandler(_revertJOService).CanRevert(requestModel);
                    if (validationResult == null)
                    {
                        var claimsIdentity = User.Identity as ClaimsIdentity ;
                        requestModel.ApprovedBy = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);
                        var reverted = _revertJOService.RevertJO(requestModel);
                        if (reverted)
                        {
                            if (requestModel.IsApproved)
                            {
                                responseData = new { message = Constants.RevertJO.RequestApproved };
                            }
                            else
                            {
                                responseData = new { message = Constants.RevertJO.RequestDenied };
                            }
                        }
                        else
                        {
                            responseData = new { message = Constants.Common.RecordDoesNotExist };
                        }
                    }
                    else
                    {
                        responseCode = HttpStatusCode.BadRequest;
                        responseData = new { errorMessage = validationResult.Message };
                    }                
                }
                catch (Exception ex)
                {
                    Helper.GetErrors(ex, out responseCode, out responseData);
                }
            }
            else
            {
                responseCode = HttpStatusCode.OK;
                responseData = new { errorMessage = Constants.Common.NotAdmin };
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }
    }
}