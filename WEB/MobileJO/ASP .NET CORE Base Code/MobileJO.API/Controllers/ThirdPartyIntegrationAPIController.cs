using Microsoft.AspNetCore.Mvc;
using MobileJO.API.Utilities;
using MobileJO.Data;
using MobileJO.Data.Models;
using MobileJO.Data.Models.TFSIntegration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using MobileJO.Domain.Contracts;
using System.Security.Claims;
using System.Collections.Generic;
using System.Collections;

namespace MobileJO.API.Controllers
{
    [Route(Constants.Common.Route)]
    [ApiController]
    public class ThirdPartyIntegrationAPIController : ControllerBase
    {
        private readonly IAssignedCasesService _assignedCasesService;
        private readonly IUserService _userService;
        private readonly ISyncLogService _syncLogService;
        private readonly IAccountService _accountService;

        public ThirdPartyIntegrationAPIController(IAssignedCasesService assignedCasesService, IUserService userService,
                                                    ISyncLogService syncLogService, IAccountService accountService)
        {
            _assignedCasesService = assignedCasesService;
            _userService = userService;
            _accountService = accountService;
            _syncLogService = syncLogService;
        }

        /// <summary>
        ///     Handles ThirdPartyIntegrationAPI/sync web-api call to sync Assigned Case records from the TFS
        ///     servers of WebPOS and Portfoliod
        /// </summary>
        /// <param></param>
        /// <returns>Represents an HTTP response that includes the status code, data and message</returns>
        /// 
        [HttpGet]
        [ActionName(Constants.TFSIntegration.SyncWithThirdParty)]
        public HttpResponseMessage SyncWithThirdParty(Boolean isNotScheduler)
        {
            var responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                using (var client = new HttpClient())
                {   // Create work item query to retrieve all work item IDS from TFS
                    DateTime dateToday = DateTime.Now;

                    var wiql = new
                    {
                        query = Constants.TFSIntegration.SelectQuery
                    };

                    // Retrieve personal access token from appsetting file. Personal access tokens should be configured in the TFS server.

                    var personalAccessToken = Configuration.Config.GetSection(Constants.TFSIntegration.WebPOSPersonalAccessToken).Value;
                    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format(Constants.TFSIntegration.CredentialsFormat, string.Empty, personalAccessToken)));

                    // Create and setup the client
                    client.BaseAddress = new Uri(Configuration.Config.GetSection(Constants.TFSIntegration.WebPOSServer).Value);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.TFSIntegration.JSONType));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.TFSIntegration.BasicAuthentication, credentials);

                    // Create and setup the http request
                    var method = new HttpMethod(Constants.TFSIntegration.POST);
                    var api = Configuration.Config.GetSection(Constants.TFSIntegration.API).Value;
                    var postValue = new StringContent(JsonConvert.SerializeObject(wiql), Encoding.UTF8, Constants.TFSIntegration.JSONType);

                    var httpRequestMessage = new HttpRequestMessage(method, api) { Content = postValue };
                    var httpResponseMessage = client.SendAsync(httpRequestMessage).Result;

                    // If request successfully retrieves data
                    // Note: TFS should be setup as Basic Authentication so that the PAT access will work
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        WorkItemQueryResult workItemQueryResult = httpResponseMessage.Content.ReadAsAsync<WorkItemQueryResult>().Result;

                        // Create a string of IDs. Will be used to retrieve items from TFS API
                        var idList = new StringBuilder();

                        ArrayList idBatches = new ArrayList();
                        int cnt = 0;
                        int totalChars = 0;
                        String temp = "";
                        foreach (var item in workItemQueryResult.WorkItems)
                        {
                            // NOTE: MAX characted length for this is 2048, 2027 omitted the 21 length of
                            // "?ids=&api-version=4.0" that is also included in 2048 counting
                            // greater than max value (2049+), the API call will return ERROR
                            temp = item.ID.ToString() + Constants.Common.CommaChar;
                            if ((totalChars + temp.Length) <= 2027)
                            {
                                idList.Append(temp);
                                totalChars = idList.ToString().Length;
                            }
                            else {
                                idBatches.Add(idList.ToString());
                                cnt++;
                                idList.Clear();
                                totalChars = 0;
                            }
                        }

                        for (int i = 0; i < idBatches.Count;i++)
                        {
                            // Removing the last Comma(',') in the string
                            string ids = idBatches[i].ToString().TrimEnd(Constants.Common.CommaChar);

                            // Retrieve work items from TFS
                            HttpResponseMessage getworkItems = client.GetAsync(Constants.TFSIntegration.GetAsyncURL + ids + Constants.TFSIntegration.GetAsynVersion).Result;

                            if (getworkItems.IsSuccessStatusCode)
                            {
                                WorkItemDetails workItemQueryFinalResult = getworkItems.Content.ReadAsAsync<WorkItemDetails>().Result;

                                foreach (var item in workItemQueryFinalResult.value)
                                {
                                    responseData = _assignedCasesService.FindByCaseNumber(item.ID.ToString());

                                    var applicationTypeID = 0;

                                    switch (item.Fields.TeamProject)
                                    {
                                        case Constants.Application.Portfolio:
                                            applicationTypeID = 1;
                                            break;
                                        case Constants.Application.WebPOS:
                                            applicationTypeID = 2;
                                            break;
                                        case Constants.Application.HRIS:
                                            applicationTypeID = 3;
                                            break;
                                        default:
                                            applicationTypeID = 0;
                                            break;
                                    }

                                    var userName = item.Fields.AssignedTo != null ? Helper.getBetween(item.Fields.AssignedTo, Constants.Common.DoubleDash, Constants.Common.GreaterThan) : string.Empty;
                                    var createdBy = item.Fields.CreatedBy != null ? Helper.getBetween(item.Fields.CreatedBy, Constants.Common.DoubleDash, Constants.Common.GreaterThan) : string.Empty;
                                    var updatedBy = item.Fields.ChangedBy != null ? Helper.getBetween(item.Fields.ChangedBy, Constants.Common.DoubleDash, Constants.Common.GreaterThan) : string.Empty;
                                    var accountName = item.Fields.AccountName != null ? item.Fields.AccountName : string.Empty;
                                    // This is to get the Username information of the Assigned User.
                                    // The TFS will only return the Name/Username of the user, but we will save the User ID.
                                    var user = _userService.FindUser(userName);
                                    var account = _accountService.FindAccount(accountName);
                                    int accountID = 0;

                                    if(account == null)
                                    {
                                        // Create Temporary Account Details for Non-Registered Account
                                        Account accountData = new Account
                                        {
                                            Name = accountName,
                                            EmailAddress = Constants.Account.TempEmail,
                                            ContactNo = Constants.Account.NA,
                                            ContactPerson = Constants.Account.NA,
                                            Address = Constants.Account.NA,
                                            Memo = Constants.Account.NA,
                                            IsActive = true,
                                            CreatedDate = DateTime.Now,
                                            CreatedBy = Constants.Account.TempID,
                                            UpdatedDate = DateTime.Now,
                                            UpdatedBy = Constants.Account.TempID
                                        };

                                        accountID = _accountService.CreateTempAccount(accountData);
                                    }

                                    AssignedCase assignedCase = new AssignedCase
                                    {
                                        CaseNumber = item.ID.ToString(),
                                        Status = item.Fields.State == null ? string.Empty : item.Fields.State,
                                        ApplicationTypeID = applicationTypeID,
                                        CaseSubject = item.Fields.Title == null ? string.Empty : item.Fields.Title,
                                        Priority = Constants.Common.Priority + item.Fields.Priority.ToString(),
                                        AccountID = account == null ? accountID : account.ID,
                                        Description = item.Fields.ShortDescription == null ? string.Empty : item.Fields.ShortDescription, 
                                        AssignedUserID = user == null ? 1 : user.ID,
                                        CreatedDate = item.Fields.CreatedDate,
                                        CreatedBy = createdBy,
                                        UpdatedDate = item.Fields.CreatedDate,
                                        UpdatedBy = updatedBy
                                    };

                                    // This will check if the request is from Web JO or API Scheduler.
                                    // If from Web JO, it will filter the data was created/updated today.
                                    // else, it will filter the data that was created yesterday.
                                    string curDate = isNotScheduler ? DateTime.Now.ToShortDateString() : DateTime.Now.AddDays(-1).ToShortDateString();
                                    var msg = string.Empty;
                                    if (responseData != null)
                                    {
                                        // Update here
                                        AssignedCase assignedCaseDB = _assignedCasesService.FindByCaseNumber(assignedCase.CaseNumber);
                                        assignedCase.ID = assignedCaseDB.ID;
                                    
                                        msg = _assignedCasesService.Update(assignedCase);
                                    }
                                    else
                                    {
                                        // Save here
                                        msg = _assignedCasesService.Create(assignedCase);
                                    }

                                    if(msg != string.Empty)
                                    {
                                        SyncLog syncLog = new SyncLog
                                        {
                                            CaseNumber = assignedCase.CaseNumber,
                                            DateSync = DateTime.Now,
                                            ErrMsg = msg
                                        };
                                    
                                        _syncLogService.Create(syncLog);
                                    }
                                
                                }

                                responseData = workItemQueryFinalResult;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }
    }
}