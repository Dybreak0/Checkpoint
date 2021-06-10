using Acr.UserDialogs;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels;
using MobileJO.Core.ViewModels.Common;
using MobileJO.Core.ViewModels.FieldViewModels;
using MobileJO.Core.ViewModels.QuestionnaireListViewModels;
using MobileJO.Core.ViewModels.ResponseListViewModels;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.EmailJO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media.Abstractions;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;

namespace MobileJO.Core.Services
{
    public class WebService : IWebService
    {
        private readonly HttpClient _client;
        private readonly IAppSettings _settings;
        private readonly ILocalizeService _localizeService;
        private readonly IUserDialogs _userDialogs;
        private HttpResponseMessage _response;

        /// <summary>
        /// Constructor for Web Service Class
        /// </summary>
        public WebService(IAppSettings settings, ILocalizeService localizeService, IUserDialogs userDialogs)
        {
            _settings = settings;
            _localizeService = localizeService;
            _userDialogs = userDialogs;
            // Initialize if needed Authentication
            _client = new HttpClient { MaxResponseContentBufferSize = Constants.Http.MaxResponseContentBufferSize };
        }

        /// <summary>
        /// HTTP Request function wrapper for Http Calls
        /// </summary>
        /// <param name="url">URL of the API</param>
        /// <param name="action">Action of the API</param>
        /// <param name="method">Method Type</param>
        /// <param name="contentObject">Data to pass</param>
        /// <returns>String data in JSON format</returns>
        private async Task<string> HttpRequest(string url, string module, string action, string method, object contentObject)
        {
            var returnContent = string.Empty;
            try
            {

                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));
                if (!string.IsNullOrEmpty(_settings.AccessToken))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Common.Bearer, _settings.AccessToken);
                }
                var uri = string.Empty;

                if (string.IsNullOrEmpty(module))
                {
                    uri = Constants.Http.LoginWebUrl + action;
                }
                else
                {
                    uri = string.Format(url, module, action);
                }

                var content = (contentObject != null) ? new StringContent(contentObject.ToString(), Encoding.UTF8, Constants.Common.JsonHeaderValue) : null;

                switch (method)
                {
                    case Constants.Http.Get:
                        _response = await _client.GetAsync(uri);
                        break;

                    case Constants.Http.Post:
                        _response = await _client.PostAsync(uri, content);
                        break;

                    case Constants.Http.Put:
                        _response = await _client.PutAsync(uri, content);
                        break;

                    case Constants.Http.Delete:
                        _response = await _client.DeleteAsync(uri);
                        break;
                }

                await ExceptionHandler(method, uri, contentObject);

                if (_response.IsSuccessStatusCode)
                {
                    returnContent = await _response.Content.ReadAsStringAsync();
                }                
                else
                {
                    _response.EnsureSuccessStatusCode();
                }


            }
            catch (Exception ex)
            { 
                throw ex;
            }

            return returnContent;


        }

        private async Task<string> HttpRequestUpload(string url, string module, string action, string method, MediaFile uploadedFile)
        {
            var returnContent = string.Empty;
            try
            {

                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));
                if (!string.IsNullOrEmpty(_settings.AccessToken))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Common.Bearer, _settings.AccessToken);
                }
                var uri = string.Empty;

                if (string.IsNullOrEmpty(module))
                {
                    uri = Constants.Http.LoginWebUrl + action;
                }
                else
                {
                    uri = string.Format(url, module, action);
                }

                var streamContent = (uploadedFile != null) ? new StreamContent(uploadedFile.GetStream()) : null;
                var content = new MultipartFormDataContent();
                content.Add(streamContent, "\"file\"", $"\"{uploadedFile.Path}\"");

                switch (method)
                {
                    case Constants.Http.Get:
                        _response = await _client.GetAsync(uri);
                        break;

                    case Constants.Http.Post:
                        _response = await _client.PostAsync(uri, content);
                        break;

                    case Constants.Http.Put:
                        _response = await _client.PutAsync(uri, content);
                        break;

                    case Constants.Http.Delete:
                        _response = await _client.DeleteAsync(uri);
                        break;
                }

                await ExceptionHandler(method, uri, uploadedFile);

                if (_response.IsSuccessStatusCode)
                {
                    returnContent = await _response.Content.ReadAsStringAsync();
                }
                else
                {
                    _response.EnsureSuccessStatusCode();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnContent;


        }

        private async Task<byte[]> DownloadImageAsync(string url, string module, string action, object contentObject)
        {
            byte[] returnContent = null;

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

            if (!string.IsNullOrEmpty(_settings.AccessToken))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Common.Bearer, _settings.AccessToken);
            }

            var uri = string.Format(url, module, action);
            var content = (contentObject != null) ? new StringContent(contentObject.ToString(), Encoding.UTF8, Constants.Common.JsonHeaderValue) : null;
          
            _response = await _client.GetAsync(uri);
            
            if (_response.IsSuccessStatusCode)
            {
                returnContent = await _response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                _response.EnsureSuccessStatusCode();
            }

            return returnContent;
        }

        private async Task<bool> ExceptionHandler(string method, string uri, object contentObject)
        {
            var appCloser = DependencyService.Get<ICloseApplication>();
            string localizedText;
            var isNoException = false;

            try
            {

                if (_response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var refreshToken = new RefreshTokenModel
                    {
                        UserName = _settings.UserName,
                        RefreshToken = _settings.RefreshToken
                    };

                    isNoException = await RefreshToken(refreshToken);

                    if (isNoException)
                    {
                        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));
                        if (!string.IsNullOrEmpty(_settings.AccessToken))
                        {
                            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Common.Bearer, _settings.AccessToken);
                        }

                        var content = (contentObject != null) ? new StringContent(contentObject.ToString(), Encoding.UTF8, Constants.Common.JsonHeaderValue) : null;

                        switch (method)
                        {
                            case Constants.Http.Get:
                                _response = await _client.GetAsync(uri);
                                break;

                            case Constants.Http.Post:
                                _response = await _client.PostAsync(uri, content);
                                break;

                            case Constants.Http.Put:
                                _response = await _client.PutAsync(uri, content);
                                break;

                            case Constants.Http.Delete:
                                _response = await _client.DeleteAsync(uri);
                                break;
                        }
                    }

                }
                else switch (_response.StatusCode)
                    {
                        case HttpStatusCode.InternalServerError:
                            localizedText = _localizeService.Translate(Constants.Messages.ErrorRetrieving);

                            await _userDialogs.AlertAsync(localizedText);
                            appCloser?.ExitApplication();
                            break;
                        case HttpStatusCode.NotFound:
                            localizedText = _localizeService.Translate(Constants.Messages.ErrorProcessing);

                            await _userDialogs.AlertAsync(localizedText);
                            appCloser?.ExitApplication();
                            break;
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isNoException;
        }

        /// <summary>
        ///     HTTP Request function for AssignedCases List
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns>String data in JSON format</returns>
        public async Task<CasesListViewModel> AssignedCasesList(Dictionary<string, string> searchViewModel)
        {

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

            var action = string.Format(Constants.Api.Method.AssignedCaseList,
                                        Helpers.GetParamString(searchViewModel));

            var jsonAssignedCasesList = await HttpRequest(  Constants.Http.WebUrl,
                                                            Constants.Api.Module.AssignedCasesAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

            try
            {

                return JsonConvert.DeserializeObject<CasesListViewModel>(jsonAssignedCasesList);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///     HTTP Request function for Assigned Case Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns>String data in JSON format</returns>
        public async Task<AssignedCase> AssignedCase(int id)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                string action = string.Format(Constants.Api.Method.ViewCase, id);

                var jsonAssignedCase = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.AssignedCasesAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

                var returnval = JsonConvert.DeserializeObject<AssignedCase>(jsonAssignedCase);
                return returnval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for email sending
        /// </summary>
        /// <param name="emailDetails"></param>
        /// <returns>String data in JSON format</returns>
        public async Task<bool> SendEmailJO(EmailJOModel emailDetails)
        {
            try
            {

                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                var jsonEmailResponse = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.EmailJOAPI,
                                                            Constants.Api.Method.SendEmailJO,
                                                            Constants.Http.Post,
                                                            JsonConvert.SerializeObject(emailDetails));


                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        ///     HTTP Request function for retrieving the tagged job orders
        /// </summary>
        /// <param name="created_by"></param>
        /// <param name="case_id"></param>
        /// <returns>String data in JSON format</returns>
        public async Task<List<SelectJOModel>> SelectJOList(int created_by, int case_id)
        {
            try
            {
                string action = string.Format(Constants.Api.Method.TaggedCase, created_by, case_id);

                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                var jsonSelectJO = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.EmailJOAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);
                return JsonConvert.DeserializeObject<List<SelectJOModel>>(jsonSelectJO);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        ///     HTTP Request function for retrieving all application types
        /// </summary>
        /// <returns>String data in JSON format</returns>
        public async Task<List<string>> GetApplicationType()
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                var jsonSelectJO = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.AssignedCasesAPI,
                                                            Constants.Api.Method.ApplicationType,
                                                            Constants.Http.Get,
                                                            null);
                return JsonConvert.DeserializeObject<List<string>>(jsonSelectJO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for retrieving all Case status types
        /// </summary>
        /// <returns>String data in JSON format</returns>
        public async Task<List<string>> GetCaseStatus()
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                var jsonSelectJO = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.AssignedCasesAPI,
                                                            Constants.Api.Method.CaseStatus,
                                                            Constants.Http.Get,
                                                            null);
                return JsonConvert.DeserializeObject<List<string>>(jsonSelectJO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for checking if the user is allowed to login
        /// </summary>
        /// <returns>bool</returns>
        public async Task<bool> Allowed(int id)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.XWwwFormUrlEncoded));

                var method = string.Format(Constants.Api.Allowed, id);
                var result = await HttpRequest(Constants.Http.WebUrl,
                                                        Constants.Api.Module.UserAPI,
                                                        method,
                                                        Constants.Http.Get,
                                                        null);

                var valid = bool.Parse(result);

                return (valid) ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///     HTTP Request function for login
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Login(UserCredentialsModel user)
        {
            try
            {
                string param = JsonConvert.SerializeObject(user);

                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.XWwwFormUrlEncoded));

                var accessToken = await HttpRequest(Constants.Http.WebUrl,
                                                        Constants.SpecialCharacters.EmptyString,
                                                        Constants.Api.Token,
                                                        Constants.Http.Post,
                                                        param);

                var tokenData = JsonConvert.DeserializeObject<Token>(accessToken);
                _settings.AccessToken = tokenData.AccessToken;
                _settings.RefreshToken = tokenData.RefreshToken;

                var handler = new JwtSecurityTokenHandler();
                var tokens = handler.ReadToken(_settings.AccessToken) as JwtSecurityToken;

                _settings.UserID = tokens.Payload[Constants.Keys.ID].ToString();
                _settings.UserName = tokens.Payload[Constants.Keys.UserName].ToString();
                _settings.UserTypeID = tokens.Payload[Constants.Keys.UserTypeID].ToString();
                _settings.CompanyID = tokens.Payload[Constants.Keys.CompanyID].ToString();
                _settings.BranchID = tokens.Payload[Constants.Keys.BranchID].ToString();

                CrossSecureStorage.Current.SetValue(Constants.AppSettings.AccessToken, _settings.AccessToken);
                CrossSecureStorage.Current.SetValue(Constants.AppSettings.UserName, _settings.UserName);
                CrossSecureStorage.Current.SetValue(Constants.AppSettings.UserID, _settings.UserID);
                CrossSecureStorage.Current.SetValue(Constants.AppSettings.UserTypeID, _settings.UserTypeID);
                CrossSecureStorage.Current.SetValue(Constants.AppSettings.CompanyID, _settings.CompanyID);
                CrossSecureStorage.Current.SetValue(Constants.AppSettings.BranchID, _settings.BranchID);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> ReLogin()
        {
            try
            {
                _settings.AccessToken = string.Empty;

                var data = string.Format(Constants.Common.LoginGrant, HttpUtility.UrlEncode(Constants.Http.ClientId), HttpUtility.UrlEncode(Constants.Http.SecretId));

                var accessToken = await HttpRequest(Constants.Http.WebUrl, Constants.SpecialCharacters.EmptyString, Constants.Api.Token, Constants.Http.Post, data);

                var tokenData = JsonConvert.DeserializeObject<Token>(accessToken);
                _settings.AccessToken = tokenData.AccessToken;
                _settings.RefreshToken = tokenData.RefreshToken;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> RefreshToken(RefreshTokenModel refreshToken)
        {
            try
            {
                string param = JsonConvert.SerializeObject(refreshToken);            

                var accessToken = await HttpRequest(Constants.Http.WebUrl, 
                                                    Constants.SpecialCharacters.EmptyString, 
                                                    Constants.Api.Refresh, 
                                                    Constants.Http.Post,
                                                    param);

                var tokenData = JsonConvert.DeserializeObject<Token>(accessToken);
                _settings.AccessToken = tokenData.AccessToken;
                _settings.RefreshToken = tokenData.RefreshToken;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     HTTP Request function for saving job order
        /// </summary>
        /// <returns></returns>
        public async Task<JobOrderDetailsViewModel> SaveJobOrderDetails(JobOrderDetailsViewModel jobOrderViewModel)
        {
            try
            {

                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                var jsonResponse = await HttpRequest(Constants.Http.WebUrl,
                                                     Constants.Api.Module.JobOrderAPI,
                                                     Constants.Api.Method.SaveJobOrder,
                                                     Constants.Http.Post,
                                                     JsonConvert.SerializeObject(jobOrderViewModel));

                return JsonConvert.DeserializeObject<JobOrderDetailsViewModel>(jsonResponse); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///     HTTP Request function for updating job order
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateJobOrderDetails(JobOrderDetailsViewModel jobOrderViewModel)
        {
            try
            {

                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                var jsonResponse = await HttpRequest(Constants.Http.WebUrl,
                                                     Constants.Api.Module.JobOrderAPI,
                                                     Constants.Api.Method.UpdateJobOrder,
                                                     Constants.Http.Post,
                                                     JsonConvert.SerializeObject(jobOrderViewModel));

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///     HTTP Request function for syncing job order
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SyncJobOrderDetails(JobOrderDetailsViewModel jobOrderDetails)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                var jsonResponse = await HttpRequest(Constants.Http.WebUrl,
                                                     Constants.Api.Module.JobOrderAPI,
                                                     Constants.Api.Method.SyncJobOrder,
                                                     Constants.Http.Post,
                                                     JsonConvert.SerializeObject(jobOrderDetails));

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for retrieving a job order
        /// </summary>
        /// <returns></returns>
        public async Task<JobOrder> GetJobOrder(int jobOrderID)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

            var action = string.Format(Constants.Api.Method.GetJobOrder, jobOrderID);

            var jsonJobOrder = await HttpRequest(Constants.Http.WebUrl,
                                                 Constants.Api.Module.JobOrderAPI,
                                                 action,
                                                 Constants.Http.Get,
                                                 null);
            try
            {
                return JsonConvert.DeserializeObject<JobOrder>(jsonJobOrder);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for retrieving client list
        /// </summary>
        /// <returns></returns>
        public async Task<List<Account>> AccountList()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));


            var jsonAccountList = await HttpRequest(Constants.Http.WebUrl,
                                                    Constants.Api.Module.JobOrderAPI,
                                                    Constants.Api.Method.GetAccounts,
                                                    Constants.Http.Get,
                                                    null);
            try
            {
                List<Account> results = null;

                if (jsonAccountList.EndsWith(Constants.SpecialCharacters.ClosingSquareBracket))
                {
                    results = JsonConvert.DeserializeObject<List<Account>>(jsonAccountList);
                }
                else
                {
                    jsonAccountList = string.Concat(jsonAccountList, Constants.SpecialCharacters.ClosingSquareBracket);
                    results = JsonConvert.DeserializeObject<List<Account>>(jsonAccountList);
                }

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for application types list
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApplicationType>> ApplicationTypeList()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));


            var jsonApplicationList = await HttpRequest(Constants.Http.WebUrl,
                                                        Constants.Api.Module.JobOrderAPI,
                                                        Constants.Api.Method.GetApplicationsTypes,
                                                        Constants.Http.Get,
                                                        null);
            try
            {

                List<ApplicationType> results = null;

                if (jsonApplicationList.EndsWith(Constants.SpecialCharacters.ClosingSquareBracket))
                {
                    results = JsonConvert.DeserializeObject<List<ApplicationType>>(jsonApplicationList);
                }
                else
                {
                    jsonApplicationList = string.Concat(jsonApplicationList, Constants.SpecialCharacters.ClosingSquareBracket);
                    results = JsonConvert.DeserializeObject<List<ApplicationType>>(jsonApplicationList);
                }

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for assigned cases list
        /// </summary>
        /// <returns></returns>
        public async Task<List<AssignedCases>> AssignedCaseList(string assignedTo, int applicationTypeId, int accountId)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

            var action = string.Format(Constants.Api.Method.GetAssignedCases, assignedTo, applicationTypeId, accountId);

            var jsonCaseList = await HttpRequest(Constants.Http.WebUrl,
                                                 Constants.Api.Module.JobOrderAPI,
                                                 action,
                                                 Constants.Http.Get,
                                                 null);
            try
            {
                List<AssignedCases> results = null;

                if (jsonCaseList.EndsWith(Constants.SpecialCharacters.ClosingSquareBracket))
                {
                    return JsonConvert.DeserializeObject<List<AssignedCases>>(jsonCaseList);
                }
                else
                {
                    jsonCaseList = string.Concat(jsonCaseList, Constants.SpecialCharacters.ClosingSquareBracket);
                    results = JsonConvert.DeserializeObject<List<AssignedCases>>(jsonCaseList);
                }

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for billing types list
        /// </summary>
        /// <returns></returns>
        public async Task<List<BillingTypes>> BillingTypeList()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));


            var jsonBillingList = await HttpRequest(Constants.Http.WebUrl,
                                                    Constants.Api.Module.JobOrderAPI,
                                                    Constants.Api.Method.GetBillingTypes,
                                                    Constants.Http.Get,
                                                    null);
            try
            {

                List<BillingTypes> results = null;

                if (jsonBillingList.EndsWith(Constants.SpecialCharacters.ClosingSquareBracket))
                {
                    results = JsonConvert.DeserializeObject<List<BillingTypes>>(jsonBillingList);
                }
                else
                {
                    jsonBillingList = string.Concat(jsonBillingList, Constants.SpecialCharacters.ClosingSquareBracket);
                    results = JsonConvert.DeserializeObject<List<BillingTypes>>(jsonBillingList);
                }

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for tagged cases list
        /// </summary>
        /// <returns></returns>
        public async Task<List<TaggedCase>> TaggedCaseList(int jobOrderID)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

            var action = string.Format(Constants.Api.Method.GetTaggedCases, jobOrderID);

            var jsonTaggedCaseList = await HttpRequest(Constants.Http.WebUrl,
                                                 Constants.Api.Module.JobOrderAPI,
                                                 action,
                                                 Constants.Http.Get,
                                                 null);
            try
            {
                List<TaggedCase> results = null;

                if (jsonTaggedCaseList.EndsWith(Constants.SpecialCharacters.ClosingSquareBracket))
                {
                    return JsonConvert.DeserializeObject<List<TaggedCase>>(jsonTaggedCaseList);
                }
                else
                {
                    jsonTaggedCaseList = string.Concat(jsonTaggedCaseList, Constants.SpecialCharacters.ClosingSquareBracket);
                    results = JsonConvert.DeserializeObject<List<TaggedCase>>(jsonTaggedCaseList);
                }

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for job order billing types selected
        /// </summary>
        /// <returns></returns>
        public async Task<List<JobOrderBillingType>> JobOrderBillingTypeList(int jobOrderID)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

            var action = string.Format(Constants.Api.Method.GetJOBillingTypes, jobOrderID);

            var jsonJOBillingTypeList = await HttpRequest(Constants.Http.WebUrl,
                                                 Constants.Api.Module.JobOrderAPI,
                                                 action,
                                                 Constants.Http.Get,
                                                 null);
            try
            {
                List<JobOrderBillingType> results = null;

                if (jsonJOBillingTypeList.EndsWith(Constants.SpecialCharacters.ClosingSquareBracket))
                {
                    results = JsonConvert.DeserializeObject<List<JobOrderBillingType>>(jsonJOBillingTypeList);
                }
                else
                {
                    jsonJOBillingTypeList = string.Concat(jsonJOBillingTypeList, Constants.SpecialCharacters.ClosingSquareBracket);
                    results = JsonConvert.DeserializeObject<List<JobOrderBillingType>>(jsonJOBillingTypeList);
                }

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for job order attachments selected
        /// </summary>
        /// <returns></returns>
        public async Task<List<Attachment>> AttachmentList(int jobOrderID)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

            var action = string.Format(Constants.Api.Method.GetJOAttachments, jobOrderID);

            var jsonAttachmentList = await HttpRequest(Constants.Http.WebUrl,
                                                 Constants.Api.Module.JobOrderAPI,
                                                 action,
                                                 Constants.Http.Get,
                                                 null);
            try
            {
                List<Attachment> results = null;

                if (jsonAttachmentList.EndsWith(Constants.SpecialCharacters.ClosingSquareBracket))
                {
                    return JsonConvert.DeserializeObject<List<Attachment>>(jsonAttachmentList);
                }
                else
                {
                    jsonAttachmentList = string.Concat(jsonAttachmentList, Constants.SpecialCharacters.ClosingSquareBracket);
                    results = JsonConvert.DeserializeObject<List<Attachment>>(jsonAttachmentList);
                }

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //testing only
        public async Task<List<JobOrder>> JobOrdersList()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));


            var jsonJobOrdersList = await HttpRequest(Constants.Http.WebUrl,
                                                      Constants.Api.Module.JobOrderAPI,
                                                      "jobOrderList",
                                                      Constants.Http.Get,
                                                      null);
            try
            {
                List<JobOrder> results = null;

                if (jsonJobOrdersList.EndsWith("]"))
                {
                    results = JsonConvert.DeserializeObject<List<JobOrder>>(jsonJobOrdersList);
                }
                else
                {
                    jsonJobOrdersList = string.Concat(jsonJobOrdersList, "]");
                    results = JsonConvert.DeserializeObject<List<JobOrder>>(jsonJobOrdersList);
                }

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// HTTP Request function for retrieving cases assigned to user
        /// </summary>
        /// <param name="assignedTo"></param>
        /// <returns></returns>
        public async Task<List<AssignedCases>> UserAssignedCasesList(int assignedTo)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

            var action = string.Format(Constants.Api.Method.GetUserAssignedCases, assignedTo);

            var jsonCaseList = await HttpRequest(Constants.Http.WebUrl,
                                                 Constants.Api.Module.JobOrderAPI,
                                                 action,
                                                 Constants.Http.Get,
                                                 null);
            try
            {
                List<AssignedCases> results = null;

                if (jsonCaseList.EndsWith(Constants.SpecialCharacters.ClosingSquareBracket))
                {
                    return JsonConvert.DeserializeObject<List<AssignedCases>>(jsonCaseList);
                }
                else
                {
                    jsonCaseList = string.Concat(jsonCaseList, Constants.SpecialCharacters.ClosingSquareBracket);
                    results = JsonConvert.DeserializeObject<List<AssignedCases>>(jsonCaseList);
                }

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for retrieving all job order 
        /// </summary>
        /// <returns>String data in JSON format</returns>
        public async Task<PaginationViewModel> GetJobOrderList(Dictionary<string, string> jobOrder)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));
            string action = string.Format(Constants.Api.Method.List, Helpers.GetParamString(jobOrder));

            var jsonJobOrderList = await this.HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.JobOrderAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

            try
            {
                return JsonConvert.DeserializeObject<PaginationViewModel>(jsonJobOrderList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for retrieving all Job order application types
        /// </summary>
        /// <returns>String data in JSON format</returns>
        public async Task<List<string>> GetApplicationTypeJO()
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                var jsonSelectJO = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.JobOrderAPI,
                                                            Constants.Api.Method.ApplicationType,
                                                            Constants.Http.Get,
                                                            null);
                return JsonConvert.DeserializeObject<List<string>>(jsonSelectJO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for retrieving all Job order status types
        /// </summary>
        /// <returns>String data in JSON format</returns>
        public async Task<List<string>> GetJobOrderStatus()
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                var jsonSelectJO = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.JobOrderAPI,
                                                            Constants.Api.Method.JobOrderStatus,
                                                            Constants.Http.Get,
                                                            null);
                return JsonConvert.DeserializeObject<List<string>>(jsonSelectJO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for retrieving Job order details
        /// </summary>
        /// <returns>String data in JSON format</returns>
        public async Task<JobOrder> JobOrderDetail(int id)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                string action = string.Format(Constants.Api.Method.View, id);

                var jsonJobOrders = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.JobOrderAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

                return JsonConvert.DeserializeObject<JobOrder>(jsonJobOrders);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for soft deleting a job order
        /// </summary>
        /// <returns>String data in JSON format</returns>
        public async Task<bool> DeleteJobOrder(int id)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                string action = string.Format(Constants.Api.Method.Delete, id);

                var jsonJobOrders = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.JobOrderAPI,
                                                            action,
                                                            Constants.Http.Put,
                                                            null);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for request revert job order
        /// </summary>
        /// <returns>String data in JSON format</returns>
        public async Task<bool> RevertJobOrder(int id)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                string action = string.Format(Constants.Api.Method.RequestRevert, id);

                var jsonJobOrders = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.JobOrderAPI,
                                                            action,
                                                            Constants.Http.Post,
                                                            null);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for revert request count
        /// </summary>
        /// <returns>String data in JSON format</returns>
        public async Task<RevertModel> GetRevertCount(int id)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                string action = string.Format(Constants.Api.Method.RequestRevertCount, id);

                var jsonJobOrders = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.JobOrderAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

                return JsonConvert.DeserializeObject<RevertModel>(jsonJobOrders);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for retrieving all tagged cases in a particular job order
        /// </summary>
        /// <returns>String data in JSON format</returns>
        public async Task<List<TaggedCaseModel>> TaggedCasesList(Dictionary<string, string> taggedCases)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));
            string action = string.Format(Constants.Api.Method.TaggedCasesList, Helpers.GetParamString(taggedCases));

            var jsonTaggedCasesList = await this.HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.JobOrderAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

            try
            {
                return JsonConvert.DeserializeObject<List<TaggedCaseModel>>(jsonTaggedCasesList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for retrieving tagged case details
        /// </summary>
        /// <returns>String data in JSON format</returns>
        public async Task<TaggedCase> TagCaseDetail(int id)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                string action = string.Format(Constants.Api.Method.ViewCaseDetail, id);

                var jsonTagCaseDetails = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.JobOrderAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

                return JsonConvert.DeserializeObject<TaggedCase>(jsonTagCaseDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        ///     HTTP Request function for retrieving all attachments of a job order
        /// </summary>
        /// <returns>String data in JSON format</returns>
        public async Task<List<AttachmentModel>> GetAttachmentList(int jobOrderID)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                string action = string.Format(Constants.Api.Method.AttachmentList, jobOrderID);

                var jsonSelectJO = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.JobOrderAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

                return JsonConvert.DeserializeObject<List<AttachmentModel>>(jsonSelectJO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for retrieving a file
        /// </summary>
        /// <returns>Byte array data</returns>
        public async Task<byte[]> DownloadFile(Dictionary<string, string> path)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValueFiles));

                string action = string.Format(Constants.Api.Method.ViewAttachment, Helpers.GetParamString(path));

                byte[] jsonDownloadFile = await DownloadImageAsync(Constants.Http.WebUrl,
                                                            Constants.Api.Module.AttachementAPI,
                                                            action,
                                                            null);

                return jsonDownloadFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for retrieving all billing type of a job order
        /// </summary>
        /// <returns>String data in JSON format</returns>
        public async Task<List<JobOrderBillingTypeModel>> GetBillingList(int jobOrderID)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                string action = string.Format(Constants.Api.Method.BillingList, jobOrderID);

                var jsonBillingType = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.JobOrderAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

                return JsonConvert.DeserializeObject<List<JobOrderBillingTypeModel>>(jsonBillingType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobOrder"></param>
        /// <returns></returns>
        public async Task<List<JobOrder>> GetAllUserJobOrders(int userId)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

            string action = string.Format(Constants.Api.Method.GetUserJobOrders, userId);

            var jsonJobOrderList = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.JobOrderAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

            try
            {
                var result = JsonConvert.DeserializeObject<List<JobOrder>>(jsonJobOrderList);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for Assigned Case selected
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>String data in JSON format</returns>
        public async Task<List<AssignedCasesList>> FindCases(string ids)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                string action = string.Format(Constants.Api.Method.FindCases, ids);

                var jsonAssignedCase = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.AssignedCasesAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

                var returnval = JsonConvert.DeserializeObject<List<AssignedCasesList>>(jsonAssignedCase);

                return returnval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TaggedCase>> JOCaseList(string ids)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                string action = string.Format(Constants.Api.Method.JOCases, ids);

                var jsonAssignedCase = await HttpRequest(Constants.Http.WebUrl,
                                                         Constants.Api.Module.JobOrderAPI,
                                                         action,
                                                         Constants.Http.Get,
                                                         null);

                var returnval = JsonConvert.DeserializeObject<List<TaggedCase>>(jsonAssignedCase);

                return returnval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<JobOrderBillingType>> JOBillingTypeList(string ids)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                string action = string.Format(Constants.Api.Method.JOBillingTypes, ids);

                var jsonAssignedCase = await HttpRequest(Constants.Http.WebUrl,
                                                         Constants.Api.Module.JobOrderAPI,
                                                         action,
                                                         Constants.Http.Get,
                                                         null);

                var returnval = JsonConvert.DeserializeObject<List<JobOrderBillingType>>(jsonAssignedCase);

                return returnval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<AttachmentModel>> JOAttachmentList(string ids)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                string action = string.Format(Constants.Api.Method.JOAttachments, ids);

                var jsonAssignedCase = await HttpRequest(Constants.Http.WebUrl,
                                                         Constants.Api.Module.JobOrderAPI,
                                                         action,
                                                         Constants.Http.Get,
                                                         null);

                var returnval = JsonConvert.DeserializeObject<List<AttachmentModel>>(jsonAssignedCase);

                return returnval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<JobOrderStatus>> JOStatusList()
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                var jsonAssignedCase = await HttpRequest(Constants.Http.WebUrl,
                                                         Constants.Api.Module.JobOrderAPI,
                                                         Constants.Api.Method.JOStatus,
                                                         Constants.Http.Get,
                                                         null);

                var returnval = JsonConvert.DeserializeObject<List<JobOrderStatus>>(jsonAssignedCase);

                return returnval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseDataViewModel> GetResponseList(Dictionary<string, string> response)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));
            string action = string.Format(Constants.Api.Method.ResponseListByTemplateID, Helpers.GetParamString(response));

            var jsonResponseList = await this.HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.ResponseAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

            try
            {
                DataViewModel responseData = JsonConvert.DeserializeObject<DataViewModel>(jsonResponseList);

                ResponsePaginationViewModel data = JsonConvert.DeserializeObject<ResponsePaginationViewModel>(JsonConvert.SerializeObject(responseData.Data));

                ResponseDataViewModel resModel = JsonConvert.DeserializeObject<ResponseDataViewModel>(JsonConvert.SerializeObject(data.Data));
                resModel.Pagination = data.Pagination;
                return resModel;
    }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseDetailsModel> GetResponseByResponseID(int responseID, int templateID)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));
            string action = string.Format(Constants.Api.Method.GetResponseByResponseID, responseID, templateID);

            var jsonResponseDetails = await this.HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.ResponseAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);
            try
            {
                DataViewModel responseData = JsonConvert.DeserializeObject<DataViewModel>(jsonResponseDetails);
                ResponseDetailsModel data = JsonConvert.DeserializeObject<ResponseDetailsModel>(responseData.Data.ToString());
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<DropdownViewModel>> GetCompanies()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

            var companyList = await this.HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.DropdownAPI,
                                                            Constants.Api.Method.GetAllCompanies,
                                                            Constants.Http.Get,
                                                            null);

            try
            {

                return JsonConvert.DeserializeObject<List<DropdownViewModel>>(companyList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<QuestionnairePaginationViewModel> GetQuestionnaireList(Dictionary<string, string> template)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));
            string action = string.Format(Constants.Api.Method.List, Helpers.GetParamString(template));

            var jsonTemplateList = await this.HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.QuestionnaireAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

            try
            {
                DataViewModel data = JsonConvert.DeserializeObject<DataViewModel>(jsonTemplateList);

                return JsonConvert.DeserializeObject<QuestionnairePaginationViewModel>(JsonConvert.SerializeObject(data.Data));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DropdownViewModel>> GetCompanies(int templateID)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));
            string action = string.Format(Constants.Api.Method.GetCompaniesByTemplateID, templateID);

            var companyList = await this.HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.DropdownAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

            try
            {

                return JsonConvert.DeserializeObject<List<DropdownViewModel>>(companyList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DropdownViewModel>> GetBranches(int companyID)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));
            string action = string.Format(Constants.Api.Method.GetBranchesByCompanyID, companyID);

            var branchList = await this.HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.DropdownAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

            try
            {
                return JsonConvert.DeserializeObject<List<DropdownViewModel>>(branchList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseAnswerModel> SaveResponse(ResponseAnswerModel responseAnswer)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                var jsonSaveResponseDetails = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.ResponseAPI,
                                                            Constants.Api.Method.SaveResponse,
                                                            Constants.Http.Post,
                                                            JsonConvert.SerializeObject(responseAnswer));

                return JsonConvert.DeserializeObject<ResponseAnswerModel>(jsonSaveResponseDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ForgotPasswordResponseModel> ResetPassword(EmailModel emailModel)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                var jsonResponse = await HttpRequest(Constants.Http.WebUrl,
                                                         Constants.Api.Module.ForgotPasswordAPI,
                                                         Constants.Api.Method.ResetPassword,
                                                         Constants.Http.Post,
                                                         JsonConvert.SerializeObject(emailModel));

                return JsonConvert.DeserializeObject<ForgotPasswordResponseModel>(jsonResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     HTTP Request function for syncing response and answer
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAnswerDetailsViewModel> SyncResponseAndAnswerDetails(ResponseAnswerDetailsViewModel responseAnswerDetailsViewModel)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                var jsonResponse = await HttpRequest(Constants.Http.WebUrl,
                                                     Constants.Api.Module.QuestionnaireAPI,
                                                     Constants.Api.Method.SyncResponseAndAnswer,
                                                     Constants.Http.Post,
                                                     JsonConvert.SerializeObject(responseAnswerDetailsViewModel));

                return JsonConvert.DeserializeObject<ResponseAnswerDetailsViewModel>(jsonResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<QuestionnaireViewModel>> GetAllTemplates()
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                string action = Constants.Api.Method.GetAllTemplatesMobile;

                var jsonTemplate = await HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.QuestionnaireAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

                return JsonConvert.DeserializeObject<List<QuestionnaireViewModel>>(jsonTemplate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ResponseAnswerDetailsViewModel>> GetAllResponseByTemplateID(int templateID)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));
            string action = string.Format("getResponseByTemplateID?templateID={0}", templateID);

            var jsonResponseDetails = await this.HttpRequest(Constants.Http.WebUrl,
                                                            Constants.Api.Module.ResponseAPI,
                                                            action,
                                                            Constants.Http.Get,
                                                            null);

            try
            {
                return JsonConvert.DeserializeObject<List<ResponseAnswerDetailsViewModel>>(jsonResponseDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataViewModel> UploadMedia(MediaFile uploadedFile)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Common.JsonHeaderValue));

                var jsonResponse = await HttpRequestUpload(Constants.Http.WebUrl,
                                                        Constants.Api.Module.ResponseAPI,
                                                        Constants.Api.Method.UploadMedia,
                                                        Constants.Http.Post,
                                                        uploadedFile);
                return JsonConvert.DeserializeObject<DataViewModel>(jsonResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

