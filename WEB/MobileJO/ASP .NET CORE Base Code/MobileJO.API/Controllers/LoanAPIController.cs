using AutoMapper;
using MobileJO.Data;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.LoanApplication;
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
using MobileJO.API.Utilities;
using MobileJO.Domain;
using MobileJO.Domain.Contracts;
using System.Collections.Generic;

namespace MobileJO.API.Controllers
    {
        [Authorize(AuthenticationSchemes = Constants.Common.Bearer, Roles = Constants.Roles.Administrator)]
        [Route("api/[controller]/[action]")]
        [ApiController]
        public class LoanAPIController : ControllerBase
        {
            private readonly ILoanService _loanService;
            private readonly IUserService _userService;
            private readonly IMapper _mapper;

            public LoanAPIController(ILoanService loanService, IUserService userService,
                                         IMapper mapper)
            {
                _loanService = loanService;
                _userService = userService;
                _mapper = mapper;
            }

        /// <summary>
        ///     Handles LoanAPI/saveLoan web-api call to save loan application details.
        /// </summary>
        /// <param name="loanDetails">Holds job order details request data</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("createLoanApplication")]
        public HttpResponseMessage CreateLoanApplication(LoanDetailsViewModel loanDetails)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            if (!ModelState.IsValid)
            {
                return Helper.ComposeResponse(HttpStatusCode.BadRequest, Helper.GetModelStateErrors(ModelState));
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var claimsIdentity = User.Identity as ClaimsIdentity;

                    int id = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);
                    var currentDateTime = DateTime.Now;

                    loanDetails.CreatedBy = id;
                    loanDetails.CreatedDate = currentDateTime;

                    var attachmentPath = Configuration.Config.GetSection(Constants.Attachment.AttachmentPath).Value;

                    responseData = new LoanDetailsViewModel { LoanID = _loanService.Create(loanDetails, attachmentPath) };
                }
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);

        }
        /// <summary>
        ///     Handles LoanAPI/getLoanApplication web-api call to get list of Loan Application records.
        /// </summary>
        /// <param name="searchListViewModel">Holds Loan Application search filters</param>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        [HttpGet]
        [ActionName("listLoanApplication")]
        public HttpResponseMessage ListLoanApplication([FromQuery] LoanSearchViewModel searchListViewModel)
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {

                responseData = _loanService.ListLoanApplication(searchListViewModel);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles LoanAPI/getLoanApplication web-api call to get list of Loan Application records.
        /// </summary>
        /// <param name="searchPendingViewModel">Holds Loan Application search filters</param>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        [HttpGet]
        [ActionName("listPendingLoanApplication")]
        public HttpResponseMessage ListPendingLoanApplication([FromQuery] LoanSearchPendingViewModel searchPendingViewModel)
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                responseData = _loanService.PendingLoanApplication(searchPendingViewModel);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles LoanAPI/getLoanApplication web-api call to retrieve the data of a loan application details record
        /// </summary>
        /// <param name="id">Holds the loan id</param>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        /// 
        [HttpGet]
        [ActionName("getLoanApplication")]
        public HttpResponseMessage GetLoanApplicationDetails(int id)
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
                    responseData = _loanService.FindLoanApplication(id);

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
        ///     Handles LoanAPI/downloadLoanApplication web-api call to export Loan Application records to an excel file.
        /// </summary>
        /// <param name="searchListViewModel">Holds Loan Application search filters</param>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        [HttpGet]
        [ActionName("loanListToExcel")]
        public HttpResponseMessage DownloadLoanApplicationList([FromQuery] LoanSearchViewModel searchListViewModel)
        {
            var response = new HttpResponseMessage();
            try
            {
                response = _loanService.DownloadLoanApplication(searchListViewModel);
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
        ///     Handles JobOrderAPI/updateJO web-api call to update job order details.
        /// </summary>
        /// <param name="loanDetails">Holds job order details request data</param>
        /// <returns></returns>

        [HttpPut]
        [ActionName("updateLoanApplication")]
        public HttpResponseMessage UpdateLoanApplicationDetails(LoanDetailsViewModel loanDetails)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.BadRequest;

            try
            {
                var validationErrors = new LoanHandler(_loanService).CanUpdate(loanDetails);
                var validationResults = validationErrors as IList<ValidationResult> ?? validationErrors.ToList();

                if (validationErrors.Any())
                {
                    ModelState.AddModelErrors(validationResults);
                }

                if (ModelState.IsValid)
                {
                    var claimsIdentity = User.Identity as ClaimsIdentity;

                    int id = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);
                    var currentDateTime = DateTime.Now;

                    loanDetails.UpdatedBy = id;
                    loanDetails.UpdatedDate = currentDateTime;

                    var attachmentPath = Configuration.Config.GetSection(Constants.Attachment.AttachmentPath).Value;
                    _loanService.Update(loanDetails, attachmentPath);

                    responseData = Constants.Common.RecordSaved;
                    responseCode = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///       Handles web-api calls that soft delete a job order
        /// </summary>
        /// <param name="id">Passed joborder ID from front-end</param>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("deleteLoanApplication")]
        public HttpResponseMessage Delete(int id)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                var validationResults = new LoanHandler(_loanService).CanDelete(id);


                if (validationResults.Any())
                {
                    ModelState.AddModelErrors(validationResults);
                }

                if (ModelState.IsValid)
                {
                    var loanApplication = _loanService.FindLoanApplication(id);
                    if (loanApplication.LoanStatus == "Pending")
                    {
                        _loanService.Delete(id);
                        responseData = new { message = Constants.Common.SuccessDelete };
                    }
                    else
                    {
                        responseData = new { message = Constants.Common.CannotDelete };
                    }
                }
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
        [HttpPut]
        [ActionName("updateLoanStatus")]
        public HttpResponseMessage LoanApplicationApproval(LoanApprovalViewModel approvalViewModel)
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            int id = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

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
                    var validationResults = new LoanHandler(_loanService).CanApprove(approvalViewModel);
                    if (validationResults.Any())
                    {
                        ModelState.AddModelErrors(validationResults);
                    }

                    if (ModelState.IsValid)
                    {
                        approvalViewModel.ApprovedBy = id;
                        var isUpdated = _loanService.UpdateLoanStatus(approvalViewModel);
                        if (isUpdated)
                        {
                            if (approvalViewModel.LoanStatus == "Approved")
                            {
                                responseData = new { message = Constants.Loans.LoanApproved };
                            }
                            else
                            {
                                responseData = new { message = Constants.Loans.LoanDenied };
                            }
                        }
                        else
                        {
                            responseData = new { message = Constants.Loans.LoanStatusResponded };
                        }
                    }
                    else
                    {
                        responseCode = HttpStatusCode.BadRequest;
                        responseData = new { errorMessage = validationResults };
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
