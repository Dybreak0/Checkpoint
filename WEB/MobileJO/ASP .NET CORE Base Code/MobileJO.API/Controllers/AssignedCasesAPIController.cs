using Microsoft.AspNetCore.Mvc;
using MobileJO.Data;
using MobileJO.Data.ViewModels.AssignedCase;
using MobileJO.Domain.Contracts;
using System;
using System.Net;
using System.Net.Http;

namespace MobileJO.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AssignedCasesAPIController : ControllerBase
    {
        private readonly IAssignedCasesService _assignedCasesService;

        /// <summary>
        ///     Constructor for IAssignedCasesService 
        /// </summary>
        /// <param name="assignedCasesService"></param>
        public AssignedCasesAPIController(IAssignedCasesService assignedCasesService)
        {
            _assignedCasesService = assignedCasesService;
        }

        /// <summary>
        ///    Handles web-api calls to get list of assigned cases.
        /// </summary>
        /// <param name="searchViewModel">Search filters for finding assigned cases</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("list")]
        public HttpResponseMessage GetAssignedCasesList([FromQuery] AssignedCasesSearchViewModel searchViewModel)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _assignedCasesService.Search(searchViewModel);
            }
            catch(Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
            
        }

        /// <summary>
        ///     Handles web-api calls to get an case's details.
        /// </summary>
        /// <param name="id">ID of the case record</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("view")]
        public HttpResponseMessage GetAssignedCaseDetail(int id)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _assignedCasesService.Find(id);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///    Handles web-api calls to get list of application types.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("application_type")]
        public HttpResponseMessage GetApplicationTypes()
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _assignedCasesService.GetApplicationTypes();
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///    Handles web-api calls to get list of case status.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("case_status")]
        public HttpResponseMessage GetCaseStatusList()
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _assignedCasesService.GetCaseStatusList();
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///    Handles web-api calls to get list of case status.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("case_list")]
        public HttpResponseMessage GetCaseStatusList(string ids)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _assignedCasesService.FindCases(ids);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }
    }
}
