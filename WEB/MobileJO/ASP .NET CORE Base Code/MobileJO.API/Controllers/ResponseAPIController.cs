using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileJO.API.Utilities;
using MobileJO.Data;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Response;
using MobileJO.Domain.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Constants = MobileJO.Data.Constants;

namespace MobileJO.API.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.Common.Bearer, Roles = Constants.Roles.Administrator)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ResponseAPIController : ControllerBase
    {
        private readonly IResponseService _responseService;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Constructor for IResponseService and IMapper
        /// </summary>
        /// <param name="responseService"></param>
        /// <param name="mapper"></param>
        public ResponseAPIController(IResponseService responseService, IMapper mapper)
        {
            _responseService = responseService;
            _mapper = mapper;
        }

        /// <summary>
        ///  Find response details with responseID
        /// </summary>
        /// <param name="responseID"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("find")]
        public HttpResponseMessage Find(int responseID)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                var result = _responseService.Find(responseID);

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
        ///    Handles web-api calls to get list of responses.
        /// </summary>
        /// <param name="searchModel">Search filters for finding Response records</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("list")]
        public HttpResponseMessage GetResponseList([FromQuery] ResponseSearchViewModel searchModel)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            int userTypeID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.UserTypeID).Value);
            string userCompanyID = claimsIdentity.FindFirst(Constants.ClaimTypes.CompanyID).Value;

            if (userTypeID == Constants.User.AdminID)
            {
                userCompanyID = null; //to display all response from all companies
            }

            try
            {

                var result = _responseService.Search(searchModel, userCompanyID);

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
        ///     Handles web-api calls to get response details by response ID.
        /// </summary>
        /// <param name="responseID"></param>
        /// <param name="templateID"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("getResponseByResponseID")]
        public HttpResponseMessage GetResponseByResponseID(int responseID, int templateID)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                int userID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);
                var result = _responseService.SearchByResponseID(responseID, templateID, userID);

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
        ///    Handles web-api calls to get list of responses by template ID.
        /// </summary>
        /// <param name="searchModel">Search filters for finding response records</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("listByTemplateID")]
        public HttpResponseMessage GetResponseListByTemplateID([FromQuery] ResponseSearchViewModel searchModel)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            searchModel.UserTypeID = claimsIdentity.FindFirst(Constants.ClaimTypes.UserTypeID).Value;
            searchModel.UserID = claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value;

            if ((int)Constants.UserType.CompanyAdmin == Convert.ToInt32(searchModel.UserTypeID))
            {
                searchModel.CompanyID = claimsIdentity.FindFirst(Constants.ClaimTypes.CompanyID).Value;
            }

            try
            {

                var result = _responseService.SearchByTemplateID(searchModel);

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
        ///     Handles web-api call to save response details.
        /// </summary>
        /// <param name="responseAnswer">Holds response details request data</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("saveResponse")]
        public HttpResponseMessage SaveResponse(ResponseAnswerViewModel responseAnswer)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;
            if (!ModelState.IsValid)
            {
                return Helper.ComposeResponse(HttpStatusCode.BadRequest, Helper.GetModelStateErrors(ModelState));
            }

            try
            {
                var checkpointAnswersPath = Configuration.Config.GetSection(Constants.CheckpointAnswers.CheckpointAnswersPath).Value;
                responseData = _responseService.CreateUpdateResponse(responseAnswer, checkpointAnswersPath);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);

        }

        /// <summary>
        ///     Handles web-api calls to update a response record
        /// </summary>
        /// <param name="responseViewModel">Contains user properties</param>
        /// <returns></returns>
        [HttpPut]
        [ActionName("editResponse")]
        public async Task<HttpResponseMessage> EditResponseAsync(ResponseEditViewModel responseViewModel)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            int roleID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.RoleID).Value);
            int updatedByID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

            if (roleID == 1)
            {
                try
                {
                    responseViewModel.UpdatedBy = updatedByID;
                    _responseService.UpdateResponse(responseViewModel);
                    responseData = new { message = Constants.Common.SuccessSave };
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


        [HttpPost]
        [ActionName("uploadMedia")]
        public HttpResponseMessage UploadMedia()
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                var httpRequest = HttpContext.Request;
                var files = httpRequest.Form.Files;
                string result = "";

                responseData = new { message = Constants.Common.UploadMediaFailed };

                if (files != null && files.Count > 0)
                {
                    var checkpointAnswersPath = Configuration.Config.GetSection(Constants.CheckpointAnswers.CheckpointAnswersPath).Value;
                    result = _responseService.UploadMedia(files, checkpointAnswersPath);
                }

                if (result != null)
                {
                    responseData = new { message = Constants.Common.UploadMediaSuccess, data = result };
                }

                responseCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
                responseData = new { message = ex.ToString() };
            }
            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles ResponseAPI/downloadResponseSummary web-api call to export response summary to an excel file.
        /// </summary>
        /// <param name="searchModel">Holds response list search filters</param>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        [HttpGet]
        [ActionName(Constants.Response.DownloadResponseSummary)]
        public HttpResponseMessage DownloadResponseSummary([FromQuery] ResponseSearchViewModel searchModel)
        {
            var response = new HttpResponseMessage();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            int userTypeID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.UserTypeID).Value);
            string userCompanyID = claimsIdentity.FindFirst(Constants.ClaimTypes.CompanyID).Value;

            if (userTypeID == Constants.User.AdminID)
            {
                userCompanyID = null; // all responses will be retrieved if logged in is super admin
            }

            try
            {
                response = _responseService.DownloadResponseSummary(searchModel, userCompanyID);
            }
            catch (Exception ex)
            {
                var responseCode = new HttpStatusCode();
                var responseData = new object();

                Helper.GetErrors(ex, out responseCode, out responseData);
                response = Helper.ComposeResponse(responseCode, responseData);
            }

            return response;
        }

        /// <summary>
        ///    Handles web-api calls to get list of responses.
        /// </summary>
        /// <param name="searchModel">Search filters for finding Response records</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("downloadResponseSummaryPDF")]
        public HttpResponseMessage DownloadResponseSummaryPDF([FromQuery] ResponseSearchViewModel searchModel)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            int userTypeID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.UserTypeID).Value);
            string userCompanyID = claimsIdentity.FindFirst(Constants.ClaimTypes.CompanyID).Value;

            if (userTypeID == Constants.User.AdminID)
            {
                userCompanyID = null; //to display all response from all companies
            }

            try
            {

                var result = _responseService.DownloadResponseSummaryPDF(searchModel, userCompanyID);

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
        ///    Handles web-api calls to get list of responses by template ID.
        /// </summary>
        /// <param name="searchModel">Search filters for finding response records</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("getResponseByTemplateID")]
        public HttpResponseMessage GetResponseByTemplateID(int templateID)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                int userID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);
                var result = _responseService.RetrieveResponseQuestionAnswer(templateID, userID);

                if (result == null)
                {
                    responseData = new { message = Constants.Common.NoResults };
                }
                else
                {
                    responseData = result;
                }

                responseCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }
    }
}
