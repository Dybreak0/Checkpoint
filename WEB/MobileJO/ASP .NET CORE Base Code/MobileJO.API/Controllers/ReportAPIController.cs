using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileJO.Data;
using MobileJO.Data.ViewModels.Reports;
using MobileJO.Domain.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;

namespace MobileJO.API.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.Common.Bearer, Roles = Constants.Roles.Administrator)]
    [Route(Constants.Common.Route)]
    [ApiController]
    public class ReportAPIController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IUserService _userService;

        public ReportAPIController(IReportService reportService, IUserService userService)
        {
            _reportService = reportService;
            _userService = userService;
        }

        /// <summary>
        ///     Handles ReportAPI/getJobOrder web-api call to retrieve the data of a job order record
        /// </summary>
        /// <param name="id">Holds the job order id</param>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        /// 
        [HttpGet]
        [ActionName(Constants.Reports.GetJobOrderDetails)]
        public HttpResponseMessage GetJobOrderDetails(int id)
         {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claims = User.Identity as ClaimsIdentity;
            var name = claims.FindFirst(Constants.ClaimTypes.UserName).Value;
            int userID = Convert.ToInt32(claims.FindFirst(Constants.ClaimTypes.ID).Value);

            var userDetails = _userService.Find(userID);

            if (userDetails.IsActive == false)
            {
                responseCode = HttpStatusCode.OK;
                responseData = Constants.Common.deletedUser;
            }

            else if (userDetails.RoleID == 1)
            {
                try
                {
                    responseData = _reportService.FindJobOrder(id);

                    if (responseData == null)
                    {
                        responseData = Constants.Common.RecordNotExist;
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
                responseData = Constants.Common.NotAdmin;
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles ReportAPI/getAssignedCase web-api call to retrieve the data of an assigned case record
        /// </summary>
        /// <param name="id">Holds the assigned case id</param>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        [HttpGet]
        [ActionName(Constants.Reports.GetAssignedCaseDetails)]
        public HttpResponseMessage GetAssignedCaseDetails(int id)
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claims = User.Identity as ClaimsIdentity;
            var name = claims.FindFirst(Constants.ClaimTypes.UserName).Value;
            int userID = Convert.ToInt32(claims.FindFirst(Constants.ClaimTypes.ID).Value);

            var userDetails = _userService.Find(userID);

            if (userDetails.IsActive == false)
            {
                responseCode = HttpStatusCode.OK;
                responseData = Constants.Common.deletedUser;
            }

            else if (userDetails.RoleID == 1)
            {
                try
                {
                    responseData = _reportService.FindAssignedCase(id);

                    if (responseData == null)
                    {
                        responseData = Constants.Common.RecordNotExist;
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
                responseData = Constants.Common.NotAdmin;
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles ReportsAPI/getJobOrderReport web-api call to get list of job order records.
        /// </summary>
        /// <param name="searchModel">Holds job order report search filters</param>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        [HttpGet]
        [ActionName(Constants.Reports.GetJobOrderReport)]
        public HttpResponseMessage GetJobOrderReport([FromQuery]JobOrderReportSearchViewModel searchModel)
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();
            
            try
            {

                responseData = _reportService.SearchJobOrder(searchModel);
            }
            catch(Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles ReportsAPI/searchAssignedCasesReport web-api call to get list of assigned case records.
        /// </summary>
        /// <param name="searchModel">Holds assigned case report search filters</param>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        [HttpGet]
        [ActionName(Constants.Reports.GetAssignedCasesReport)]
        public HttpResponseMessage GetAssignedCasesReport([FromQuery]AssignedCasesReportSearchViewModel searchModel)
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                responseData = _reportService.SearchAssignedCases(searchModel);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles ReportsAPI/getJobOrderClientRatingReport web-api call to get list of job order client rating records.
        /// </summary>
        /// <param name="searchModel">Holds job order client rating report search filters</param>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        [HttpGet]
        [ActionName(Constants.Reports.GetJobOrderClientRatingReport)]
        public HttpResponseMessage GetJobOrderClientRatingReport([FromQuery]JobOrderClientRatingReportSearchViewModel searchModel)
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                responseData = _reportService.SearchJobOrderClientRating(searchModel);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles ReportsAPI/downloadJobOrderReport web-api call to export job order records to an excel file.
        /// </summary>
        /// <param name="searchModel">Holds job order report search filters</param>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        [HttpGet]
        [ActionName(Constants.Reports.DownloadJobOrderReport)]
        public HttpResponseMessage DownloadJobOrderReport([FromQuery]JobOrderReportSearchViewModel searchModel)
        {            
            var response = new HttpResponseMessage();


            try
            {
                response = _reportService.DownloadJobOrder(searchModel);
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
        ///     Handles ReportsAPI/downloadAssignedCasesReport web-api call to export assigned case records to an excel file.
        /// </summary>
        /// <param name="searchModel">Holds assigned case report search filters</param>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        [HttpGet]
        [ActionName(Constants.Reports.DownloadAssignedCasesReport)]
        public HttpResponseMessage DownloadAssignedCasesReport([FromQuery]AssignedCasesReportSearchViewModel searchModel)
        {
            var response = new HttpResponseMessage();

            try
            {
                response = _reportService.DownloadAssignedCases(searchModel);
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
        ///     Handles ReportsAPI/downloadJobOrderClientRatingReport web-api call to export job order client rating records to an excel file.
        /// </summary>
        /// <param name="searchModel">Holds job order client rating report search filters</param>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        [HttpGet]
        [ActionName(Constants.Reports.DownloadJobOrderClientRatingReport)]
        public HttpResponseMessage DownloadJobOrderClientRatingReport([FromQuery]JobOrderClientRatingReportSearchViewModel searchModel)
        {
            var response = new HttpResponseMessage();

            try
            {
                response = _reportService.DownloadJobOrderClientRating(searchModel);
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
    }
}