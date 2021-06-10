using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileJO.API.Utilities;
using MobileJO.Data;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.JobOrder;
using MobileJO.Domain;
using MobileJO.Domain.Contracts;
using MobileJO.Domain.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using Constants = MobileJO.Data.Constants;

namespace MobileJO.API.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.Common.Bearer)]
    [Route(Constants.JobOrder.Route)]
    [ApiController]
    public class JobOrderAPIController : ControllerBase
    {
        private readonly IJobOrderService _jobOrderService;
        private readonly IMapper _mapper;        

        public JobOrderAPIController(IJobOrderService jobOrderService, 
                                     IMapper mapper)
        {
            _jobOrderService = jobOrderService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Handles JobOrderAPI/jobOrderGet web-api call to retrieve a job order.
        /// </summary>
        /// <param name="jobOrderID">Holds job order ID request data</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.GetJobOrder)]
        public HttpResponseMessage GetJobOrder(int jobOrderID)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.RetrieveJobOrder(jobOrderID);                
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles JobOrderAPI/applicationTypes web-api call to retrieve the list of application types.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.GetApplicationsTypes)]
        public HttpResponseMessage GetApplicationTypes()
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.RetrieveApplicationTypes();                
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles JobOrderAPI/accountList web-api call to retrieve the list of clients.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.GetAccounts)]
        public HttpResponseMessage GetAccountsList()
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.RetrieveAccountsList();                
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles JobOrderAPI/billingTypes web-api call to retrieve the list of billing types.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.GetBillingTypes)]
        public HttpResponseMessage GetBillingTypes()
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.RetrieveBillingTypes();                
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles JobOrderAPI/caseList web-api call to retrieve the list of cases.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.GetAssignedCases)]
        public HttpResponseMessage GetCaseList(string assignedTo, int applicationTypeId, int accountId)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.RetrieveAssignedCases(assignedTo, applicationTypeId, accountId);                
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles JobOrderAPI/taggedCaseList web-api call to retrieve the list of tagged cases.
        /// </summary>
        /// <param name="jobOrderID">Holds job order ID request data</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.GetTaggedCases)]
        public HttpResponseMessage GetTaggedCaseList(int jobOrderID)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.RetrieveTaggedCases(jobOrderID);                
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles JobOrderAPI/joBillingTypeList web-api call to retrieve the list of job order billing types.
        /// </summary>
        /// <param name="jobOrderID">Holds job order ID request data</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.GetJOBillingTypes)]
        public HttpResponseMessage GetBillingTypeList(int jobOrderID)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.RetrieveJOBillingTypes(jobOrderID);                
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles JobOrderAPI/joAttachments web-api call to retrieve the list of job order attachments.
        /// </summary>
        /// <param name="jobOrderID">Holds job order ID request data</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.GetJOAttachments)]
        public HttpResponseMessage GetAttachmentsList(int jobOrderID)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {                
                responseData = _jobOrderService.RetrieveJOAttachments(jobOrderID);                
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles JobOrderAPI/saveJO web-api call to save job order details.
        /// </summary>
        /// <param name="jobOrderDetails">Holds job order details request data</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName(Constants.JobOrder.SaveJobOrder)]
        public HttpResponseMessage SaveJobOrderDetails(JobOrderDetailsViewModel jobOrderDetails)
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

                    jobOrderDetails.CreatedBy = id;
                    jobOrderDetails.UpdatedBy = id;
                    jobOrderDetails.CreatedDate = currentDateTime;
                    jobOrderDetails.UpdatedDate = currentDateTime;
                    jobOrderDetails.LastSyncDate = currentDateTime;
                    var attachmentPath = Configuration.Config.GetSection(Constants.Attachment.AttachmentPath).Value;                    

                    responseData = new JobOrderDetailsViewModel { ID = _jobOrderService.Create(jobOrderDetails, attachmentPath) };
                }
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }
            
            return Helper.ComposeResponse(responseCode, responseData);
                        
        }

        /// <summary>
        ///     Handles JobOrderAPI/updateJO web-api call to update job order details.
        /// </summary>
        /// <param name="jobOrderDetails">Holds job order details request data</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName(Constants.JobOrder.UpdateJobOrder)]
        public HttpResponseMessage UpdateJobOrderDetails(JobOrderDetailsViewModel jobOrderDetails)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.BadRequest;

            try
            {
                var validationErrors = new JobOrderHandler(_jobOrderService).CanUpdate(jobOrderDetails);
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

                    jobOrderDetails.UpdatedBy = id;
                    jobOrderDetails.UpdatedDate = currentDateTime;
                    jobOrderDetails.LastSyncDate = currentDateTime;

                    var attachmentPath = Configuration.Config.GetSection(Constants.Attachment.AttachmentPath).Value;
                    _jobOrderService.Update(jobOrderDetails, attachmentPath);
                    
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
        ///     Handles JobOrderAPI/syncJO web-api call to sync job order details.
        /// </summary>
        /// <param name="jobOrderDetails">Holds job order details request data</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName(Constants.JobOrder.SyncJobOrder)]
        public HttpResponseMessage SyncJobOrderDetails(JobOrderDetailsViewModel jobOrderDetails)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.BadRequest;

            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                int id = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);
                DateTime currentDateTime = DateTime.Now;

                jobOrderDetails.CreatedBy = id;
                jobOrderDetails.UpdatedBy = id;
                jobOrderDetails.UpdatedDate  = currentDateTime;
                jobOrderDetails.LastSyncDate = currentDateTime;

                var attachmentPath = Configuration.Config.GetSection(Constants.Attachment.AttachmentPath).Value;
                _jobOrderService.Sync(jobOrderDetails, attachmentPath);
                
                responseData = Constants.Common.RecordSynced;
                responseCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        /// Handles JobOrderAPI/userCaseList web-api call to retrieve all cases assigned to a user
        /// </summary>
        /// <param name="assignedTo"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.UserCasesList)]
        public HttpResponseMessage GetUserCasesList(int assignedTo)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;
            try
            {
                responseData = _jobOrderService.RetrieveUserCases(assignedTo);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(HttpStatusCode.OK, responseData);
        }

        /// <summary>
        ///    FOR TESTING ONLY... This temporary function retrieves list of job order records.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.JobOrderList)]
        public HttpResponseMessage GetJobOrdersList()
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;
            try
            {
                responseData = _jobOrderService.RetrieveJobOrdersList();
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(HttpStatusCode.OK, responseData);
        }

        /// <summary>
        ///       Handles web-api calls that retrieves a Job order record based on the pased id.
        /// </summary>
        /// <param name="id">ID of the Job order record</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.View)]
        public HttpResponseMessage GetJobOrderDetail(int id)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.Find(id);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///       Handles web-api calls that retrieves a list of Job order records.
        /// </summary>
        /// <param name="searchModel">Search filters for finding Job order records</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.List)]
        public HttpResponseMessage GetJobOrderList([FromQuery] JobOrderSearchViewModel searchModel)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.Search(searchModel);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }


        /// <summary>
        ///        Handles web-api calls that retrieves list of tagged cases for a particular job order
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [ActionName(Constants.JobOrder.TaggedCasesList)]
        public HttpResponseMessage GetTaggedCasesList([FromQuery] TaggedCasesViewModel taggedCasesViewModel)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.GetTaggedCases(taggedCasesViewModel);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///        Handles web-api calls that retrieves the details of a specific case based on the passed id
        /// <param name="id">Passed joborder ID from front-end</param>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.ViewCase)]
        public HttpResponseMessage TagCaseDetail(int id)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.FindCase(id);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///       Handles web-api calls that retrieves application types. This will be use in search filter page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.ApplicationType)]
        public HttpResponseMessage GetJOApplicationTypes()
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.GetApplicationType();
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///        Handles web-api calls that retrieves the different types of status. This will be use in search filter page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.JobOrderStatus)]
        public HttpResponseMessage GetCaseStatusList()
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.GetJobOrderStatusList();
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
        [HttpPut]
        [ActionName(Constants.JobOrder.Delete)]
        public HttpResponseMessage Delete(int id)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                var validationResults = new JobOrderHandler(_jobOrderService).CanDelete(id);


                if (validationResults.Any())
                {
                    ModelState.AddModelErrors(validationResults);
                }

                if (ModelState.IsValid)
                {
                    var jobOrder = _jobOrderService.Find(id);
                    if (jobOrder.StatusID == 1)
                    {
                        _jobOrderService.Delete(id);
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
        ///         Handles web-api calls that creates a Revert Request.
        /// </summary>
        /// <param name="id">Passed joborder ID from front-end</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName(Constants.JobOrder.RequestRevert)]
        public HttpResponseMessage RequestRevertJobOrder(int id)
        {

            if (!ModelState.IsValid) return Helper.ComposeResponse(HttpStatusCode.BadRequest, Helper.GetModelStateErrors(ModelState));

            try
            {

                var jobOrder = _jobOrderService.Find(id);


                var validationErrors = new JobOrderHandler(_jobOrderService).CanRequestRevert(id);
                var validationResults = validationErrors as IList<ValidationResult> ?? validationErrors.ToList();

                if (validationResults.Any())
                {
                    ModelState.AddModelErrors(validationResults);
                }

                if (ModelState.IsValid)
                {
                    // 3 is Sent 
                    if (jobOrder.StatusID.Equals(Constants.Common.SentValue))
                    {
                        _jobOrderService.CreateJORevertRequest(id, Convert.ToInt32(jobOrder.UpdatedBy));
                    }

                    if (validationResults.Any())
                    {
                        ModelState.AddModelErrors(validationResults);
                    }
                    return Helper.ComposeResponse(HttpStatusCode.OK, Constants.Common.RequestRevertSent);
                }
                return Helper.ComposeResponse(HttpStatusCode.OK, Constants.Common.CannotRequestRevert);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return Helper.ComposeResponse(HttpStatusCode.BadRequest, Helper.GetModelStateErrors(ModelState));
        }

        /// <summary>
        ///        Handles web-api calls that retrieves the details of a specific case based on the passed id
        /// <param name="id">Passed joborder ID from front-end</param>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.RequestRevertCount)]
        public HttpResponseMessage RequestRevertCount(int id)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.GetRequestCount(id);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        [HttpGet]
        [ActionName(Constants.JobOrder.AttachmentList)]
        public HttpResponseMessage GetAttachmentList(int id)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.GetAttachments(id);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        [HttpGet]
        [ActionName(Constants.JobOrder.BillingList)]
        public HttpResponseMessage GetBillingList(int id)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.GetBillingList(id);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.JOList)]
        public HttpResponseMessage GetUserJobOrders(int createdBy)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.RetrieveUserJobOrders(createdBy);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///    Handles web-api calls to get list of jo cases
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("jo_cases")]
        public HttpResponseMessage GetJOCasesList(string ids)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.GetAllJoCases(ids);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///    Handles web-api calls to get list of jo billing types
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("jo_billing_types")]
        public HttpResponseMessage GetJOBillingTypesList(string ids)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.GetAllJoBillingTypes(ids);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///    Handles web-api calls to get list of jo attachments
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("jo_attachments")]
        public HttpResponseMessage GetJOAttachmentsList(string ids)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.GetAllJoAttachments(ids);
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.JobOrder.JOStatus)]
        public HttpResponseMessage GetJOStatuses()
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                responseData = _jobOrderService.GetAllStatus();
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }
    }
}