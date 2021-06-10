using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileJO.Data;
using MobileJO.Domain.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using Constants = MobileJO.Data.Constants;

namespace MobileJO.API.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.Common.Bearer, Roles = Constants.Roles.Administrator)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DropdownAPIController : ControllerBase
    {
        private readonly IDropdownService _dropdownService;

        public DropdownAPIController(IDropdownService dropdownService)
        {
            _dropdownService = dropdownService;
        }

        /// <summary>
        ///     This function retrieves list of application types 
        /// </summary>
        /// <returns>HttpResponseMessage containing list of application type</returns>
        [HttpGet]
        [ActionName("getApplicationType")]
        public HttpResponseMessage GetApplicationType()
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                responseData = _dropdownService.GetApplicationType();
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     This function retrieves list of job order status
        /// </summary>
        /// <returns>HttpResponseMessage containing list of job order status</returns>
        [HttpGet]
        [ActionName("getStatus")]
        public HttpResponseMessage GetStatus()
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                responseData = _dropdownService.GetStatus();
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     This function retrieves list of users
        /// </summary>
        /// <returns>HttpResponseMessage containing list of users</returns>
        [HttpGet]
        [ActionName("getUsers")]
        public HttpResponseMessage GetUsers()
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                responseData = _dropdownService.GetUsers();
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     This function retrieves list of users
        /// </summary>
        /// <returns>HttpResponseMessage containing list of users</returns>
        [HttpGet]
        [ActionName("getAccounts")]
        public HttpResponseMessage GetAccounts()
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                responseData = _dropdownService.GetAccounts();
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     This function retrieves list of users
        /// </summary>
        /// <returns>HttpResponseMessage containing list of users</returns>
        [HttpGet]
        [ActionName("getRoles")]
        public HttpResponseMessage GetRoles()
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                responseData = _dropdownService.GetRoles();
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     This function retrieves list of companies
        /// </summary>
        /// <returns>HttpResponseMessage containing list of companies</returns>
        [HttpGet]
        [ActionName("getCompanies")]
        public HttpResponseMessage GetCompanies()
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            int userTypeID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.UserTypeID).Value);
            int companyID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.CompanyID).Value);
            try
            {
                responseData = _dropdownService.GetCompanies(userTypeID, companyID);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     This function retrieves list of branches
        /// </summary>
        /// <returns>HttpResponseMessage containing list of branches</returns>
        [HttpGet]
        [ActionName("getBranches")]
        public HttpResponseMessage GetBranches(int selectedCompanyID)
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                int userTypeID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.UserTypeID).Value);
                int companyID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.CompanyID).Value);

                responseData = _dropdownService.GetBranches(userTypeID, companyID, selectedCompanyID);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }
        /// <summary>
        ///     This function retrieves list of branches
        /// </summary>
        /// <returns>HttpResponseMessage containing list of branches</returns>
        [HttpGet]
        [ActionName("getBranchesByCompanyID")]
        public HttpResponseMessage GetBranchesByCompanyID()
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            int companyID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.CompanyID).Value);
            try
            {
                responseData = _dropdownService.GetBranches(companyID);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     This function retrieves list of users types
        /// </summary>
        /// <returns>HttpResponseMessage containing list of users types</returns>
        [HttpGet]
        [ActionName("getUserTypes")]
        public HttpResponseMessage GetUserTypes()
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();
            var claimsIdentity = User.Identity as ClaimsIdentity;

            int userTypeID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.UserTypeID).Value);

            try
            {
                responseData = _dropdownService.GetUserTypes(userTypeID);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     This function retrieves list of companies by templateID
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns>HttpResponseMessage containing list of companies</returns>
        [HttpGet]
        [ActionName("getCompaniesByTemplateID")]
        public HttpResponseMessage GetCompanies(int templateID)
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            int userTypeID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.UserTypeID).Value);
            int companyID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.CompanyID).Value);

            try
            {
                responseData = _dropdownService.GetCompanies(userTypeID, companyID, templateID);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     This function retrieves list of all companies
        /// </summary>
        /// <returns>HttpResponseMessage containing list of all companies</returns>
        [HttpGet]
        [ActionName("getAllCompanies")]
        public HttpResponseMessage GetAllCompanies()
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                responseData = _dropdownService.GetCompanies();
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     This function retrieves list of all region
        /// </summary>
        /// <returns>HttpResponseMessage containing list of all region</returns>
        [HttpGet]
        [ActionName("getAllRegion")]
        public HttpResponseMessage GetAllRegion()
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                responseData = _dropdownService.GetRegion();
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     This function retrieves list of city by region
        /// </summary>
        /// <returns>HttpResponseMessage containing list of city by region</returns>
        [HttpGet]
        [ActionName("getCityByRegionID")]
        public HttpResponseMessage GetCityByRegionID(int id)
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                responseData = _dropdownService.GetCity(id);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }
    }
}