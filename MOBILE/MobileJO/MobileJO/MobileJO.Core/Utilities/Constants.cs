using PCLAppConfig;

namespace MobileJO.Core.Utilities
{
    public static class Constants
    {
        public static object HTTP { get; internal set; }

        public static class Common
        {
            public const string LoadingKey = "IsBusy";
            public const string JsonHeaderValue = "application/json";
            public const string XWwwFormUrlEncoded = "application/x-www-form-urlencoded";
            public const string JsonHeaderValueFiles = "image/png";
            public const string ButtonText = "ButtonText";
            public const string Service = "Service";
            public const string Repository = "Repository";
            public const string AppDelegate = "AppDelegate";
            public const string Bearer = "Bearer";
            public const string LoginGrant = "grant_type=password&client_id={0}&client_secret={1}";
            public const string GrantTypeRefreshToken = "grant_type=refresh_token&refresh_token={0}&client_id={1}&client_secret={2}";
            public const string StringTrue = "true";
            public const string StringFalse = "false";
            public const string All = "All";
            public const string EmailPatter = "^[\\w-\\.]+@([\\w -]+\\.)+[\\w-]{2,4}$";
            public const string SignatureNameExtension = "signature.png";            
            public const string DateFormat = "MM/dd/yyyy ";
            public const string OK = "OK";
            public const string AttachmentOne = "AttachmentOne";
            public const string AttachmentTwo = "AttachmentTwo";
            public const string AttachmentThree = "AttachmentThree";
            public const string TimePickerFormat = "HH:mm";
            public const string Time = "Bearer";
            public const string ID = "id";
            public const int PageNo = 1;
            public const int PageValue = 7;
            public const int ZeroValue = 0;
            public const int PaddingValue = 5;
            public const string TextRegex = @"^[a-zA-Z0-9@#$%&*()_=.,\s'-]+$";
            public const string JobOrderNumberPrefix = "JO-";
            public const string JobOrderNumberFormat = "000000.##";
            public const string EmailRegex = @"^(([^<>()\[\]\\*.,;:\s@]+(\.[^<>()\[\]\\.,;:\s@]+)*)|(.+))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,24}))$";
            public const string ConformeRegex = @"^[a-zA-Z0-9-]+$";
            public const string Closed = "Closed";
            public const string DateFormatWithName = "{0} by {1}";
            public const string NextStep = "nextStep";
            public const string PreventiveAction = "preventiveAction";
            public const string Remarks = "remarks";
            public const string Attendees = "attendees";
            public const string Billed = "billed";
            public const string Collaterals = "collaterals";
            public const string Warranty = "warranty";
            public const string Aps = "aps";
            public const string WebPos = "webPos";
            public const string Pending = "pending";
            public const string IsFixed = "isFixed";
            public const string IsSatisfied = "isSatisfied";
            public const string ClientRating = "clientRating";
            public const string DateTimeFormat = "MM/dd/yyyy HH:mm:ss";
            public const string MonthNameDateFormat = "MMMM dd, yyyy";
            public const string MediaNameDateFormat = "yyyMMdd_HHmmss";
        }

        public static class AppSettings
        {
            public const string AppConfig = "MobileJO.Core.App.config";
            public const string WebUrl = "WebUrl";
            public const string ClientId = "ClientId";
            public const string SecretId = "SecretId";
            public const string LoginWebUrl = "LoginWebUrl";
            public const string PersistentTextKey = "PersistentTextKey";
            public const string PersistentTextDefaultValue = "Default Value";
            public const string AccessToken = "AccessToken";
            public const string RefreshToken = "RefreshToken";
            public const string UserID = "UserID";
            public const string UserName = "UserName";
            public const string LoginAttempts = "LoginAttempts";
            public const string LoginEnabled = "LoginAttempts";
            public const string UserTypeID = "UserTypeID";
            public const string CompanyID = "CompanyID";
            public const string BranchID = "BranchID";

        }
        
        public static class SpecialCharacters
        {
            public const string Colon = ":";
            public const string Underscore = "_";
            public const string Dash = "-";
            public const string Space = " ";
            public const string SemiColon = "; ";
            public const string EmptyString = "";
            public const char CharSpace = ' ';
            public const char CharComma = ',';
            public const char CharSemiColon = ';';
            public const string ClosingSquareBracket = "]";
            public const string Comma = ",";
        }

        public static class Messages
        {
            public const string ErrorRetrieving = "There is an error while retrieving the data. Please contact administrator.";
            public const string ErrorProcessing = "There has been an error in processing your request. Please try again later.";
            public const string SuccessGet = "Data Retrieval Successful!";
            
            public const string ViewImage = "View Image";
            public const string RevertConfirmation = "Are you sure you want to revert the status of this job order?";
            public const string RevertTitle = "Revert";
            public const string SuccessRevert = "Request for Revert is sent.";
            public const string UnSuccessfulRevert = "Failed to Revert record.";
            public const string DeleteTitle = "Delete";
            public const string DeleteConfirmation = "Are you sure you want to delete this record?"; 
            public const string Yes = "Yes";
            public const string No = "No";
           
            public const string SearchResult = "Search Result";
            public const string SuccessDeletion = "Record successfully deleted.";
            public const string UnSuccessfulDeletion = "Failed to delete record.";
            public const string NoInternet = "No internet connection detected.";
            public const string NoSelectedJo = "At least one JO must be selected in order to proceed to email JO.";
            public const string EmailSent = "Email Sent.";
            public const string BadRequest = "400 (Bad Request)";
            public const string IncorrectLogin = "Incorrect username or password.";
            public const string ErrorRequiredFields = "Please fill up all required fields.";
            public const string NotAllowedToLogin = "Your account has been disabled. Please contact your administrator for further information.";        
            public const string AccountLocked = "You have been temporarily locked out of your account for 30 seconds due to maximum failed login attempts. Please try again later.";
            public const string NoRecordsFound = "No records found.";
            public const string CannotEmail = "This action requires client signatory to proceed.";
            public const string ConfirmEmail = "Are you sure you want to send the email?";
            public const string InvalidEmail = "Invalid email address.";
            public const string ConfirmCancel = "Are you sure you want to cancel? This will discard all the changes in the form.";
            public const string SaveSuccessfulTitle = "Success";
            public const string SaveToLocalSuccess = "Record successfully saved on your local storage only. Please sync your data once the internet is available to save the new job order to the server.";
            public const string SaveToServerSuccess = "Record successfully saved on your local and server storage.";
            public const string ConfirmRemove = "Are you sure you want to remove this attachment?";
            public const string EmptyFileError = "File is empty.";
            public const string InvalidFileSizeError = "Invalid file size.\nMust not be greater than 3MB.";
            public const string InvalidFilenameError = "Invalid file name.\nMust not be greater than 250 characters.";
            public const string UpdateToLocalSuccess = "Record successfully updated on your local storage only. Please sync your data once the internet is available to save the new job order to the server.";
            public const string UpdateToServerSuccess = "Record successfully updated on your local and server storage.";
            public const string SyncSuccess = "Records successfully synced.";
            public const string RecordsUpToDate = "Records are up to date.";
            public const string NotAllowedToDelete = "You can only delete Job Orders with Pending status.";
            public const string NotAllowedToRevert = "You can only revert Job Orders with Sent status.";
            public const string DeleteToLocalSuccess = "Record successfully deleted on your local storage only. Please sync your data once the internet is available to delete job order to the server.";
            public const string Confirmation = "Download";
            public const string DownloadConfirmation = "Are you sure you want to download this attachment?";
            public const string SuccessDownload = "File successfully downloaded.";
            public const string ConfirmSave = "Are you sure you want to save?";
            public const string ConfirmLogout = "Are you sure you want to logout?";
            public const string JOSubjectRequired = "Job Order Subject is required.";
            public const string JOSubjectInvalid = "Job Order Subject contains invalid characters.";
            public const string BranchNameRequired = "Branch Name is required.";
            public const string BranchNameInvalid = "Branch Name contains invalid characters.";
            public const string ActivityDetailsRequired = "Activity Details is required.";
            public const string ActivityDetailsInvalid = "Activity Details contains invalid characters.";
            public const string RootCauseRequired = "Root Cause Analysis is required.";
            public const string RootCauseInvalid = "Root Cause Analysis contains invalid characters.";
            public const string NextStepRequired = "Next Step is required.";
            public const string NextStepInvalid = "Next Step contains invalid characters.";
            public const string PreventiveActionRequired = "Preventive Action is required.";
            public const string PreventiveActionInvalid = "Preventive Action contains invalid characters.";
            public const string RemarksRequired = "Remarks is required.";
            public const string RemarksInvalid = "Remarks contain invalid characters.";
            public const string AttendeesRequired = "Attendees is required.";
            public const string AttendeesInvalid = "Attendees contain invalid characters.";
            public const string CaseRequired = "Please select at least 1 case.";
            public const string BillingTypeRequired = "Please select at least 1 billing type.";
            public const string MaxAttachmentsError = "You have reached the maximum number of attachments.";
            public const string AllowedRequestCount = "JO has already been approved/denied for revert.";
            public const string InvalidConformeNumber = "Invalid conforme slip number.";
            public const string InvalidClientRating = "Client Rating must be 1-5 only.";
            public const string ErrorUploading = "There has been an error while uploading the file. Please try again later.";
            public const string ErrorExistingFile = "*Files with duplicate names won't be \nuploaded.";
            public const string OpenConfirmation = "Are you sure you want to open this attachment?";
            public const string InvalidInputsError = "Please fill up all fields";
            public const string StatusNew = "New";
            public const string FillUpQuestionnaire = "FILL UP ({0}/{1})";
            public const string RecordDoesNotExist = "Record does not exist.";
            public const string UserNotFound = "User not found.";
            public const string ForgetPasswordSent = "Email has been successfully sent.";
            public const string SaveResponseToLocalSuccess = "Record successfully saved on your local storage only. Please sync your data once the internet is available to save the new reponse to the server.";
            public const string FillUpCompanyBranch = "Please select a company and a branch.";
        }

        public static class Http
        {
            public static string WebUrl => ConfigurationManager.AppSettings[AppSettings.WebUrl];
            public static string ClientId => ConfigurationManager.AppSettings[AppSettings.ClientId];
            public static string SecretId => ConfigurationManager.AppSettings[AppSettings.SecretId];
            public static string LoginWebUrl => ConfigurationManager.AppSettings[AppSettings.LoginWebUrl];
            public const string Get = "GET";
            public const string Post = "POST";
            public const string Put = "PUT";
            public const string Delete = "DELETE";
            public const int MaxResponseContentBufferSize = 2000000;
        }

        public static class EmailJO
        {
            public static string Onsite = "Onsite";
            public static string Offsite = "Offsite";
        }

        public enum EmailType
        {
            To = 1,
            Cc = 2, 
            Bcc = 3
        }

        public static class Api
        {
            public const string Token = "token";
            public const string Allowed = "allowed?userID={0}";
            public const string Refresh = "refresh";

            public static class Module
            {
                public const string AssignedCasesAPI = "AssignedCasesAPI";
                public const string EmailJOAPI = "EmailJOAPI";
                public const string UserAPI = "UserAPI";
                public const string JobOrderAPI = "JobOrderAPI";
                public const string AttachementAPI = "AttachmentAPI";
                public const string DropdownAPI = "DropdownAPI";
                public const string ResponseAPI = "ResponseAPI";
                public const string QuestionnaireAPI = "QuestionnaireAPI";
                public const string ForgotPasswordAPI = "ForgotPasswordAPI";
            }
            public const string GetJobOrderList = "/list";

            public static class Method
            {
                public const string List = "list?{0}";
                public const string View = "view?id={0}";
                public const string Delete = "delete?id={0}";
                public const string RequestRevert = "requestRevert?id={0}";
                public const string RequestRevertCount = "requestRevertCount?id={0}";
                public const string GetCount = "getCount?id={0}";
                public const string ApplicationType = "application_type";
                public const string JobOrderStatus = "job_order_status";
                public const string TaggedCasesList = "tagged_cases_list?{0}";
                public const string AttachmentList = "attachment_list?id={0}";
                public const string BillingList = "billing_list?id={0}";
                public const string ViewCaseDetail = "viewCase?id={0}";
                public const string ViewAttachment = "downloadAttachment?{0}";
                public const string AssignedCaseList = "list?{0}";
                public const string ViewCase = "view?id={0}";
                public const string SendEmailJO = "send";
                public const string TaggedCase = "tagged_jo?createdBy={0}&caseID={1}";
                public const string CaseStatus = "case_status";
                public const string GetJobOrder = "jobOrderGet?jobOrderID={0}";
                public const string GetApplicationsTypes = "applicationTypes";
                public const string GetAccounts = "accountsList";
                public const string GetBillingTypes = "billingTypes";
                public const string GetAssignedCases = "caseList?assignedTo={0}&applicationTypeId={1}&accountId={2}";
                public const string GetTaggedCases = "taggedCaseList?jobOrderID={0}";
                public const string GetJOBillingTypes = "joBillingTypeList?jobOrderID={0}";
                public const string GetJOAttachments = "joAttachments?jobOrderID={0}";
                public const string SaveJobOrder = "saveJO";
                public const string UpdateJobOrder = "updateJO";
                public const string SyncJobOrder = "syncJO";
                public const string SaveSignature = "saveSignature";
                public const string GetUserAssignedCases = "userCasesList?assignedTo={0}";
                public const string GetUserJobOrders = "joList?createdBy={0}";                
                public const string FindCases = "case_list?ids={0}";
                public const string JOCases = "jo_cases?ids={0}";
                public const string JOBillingTypes = "jo_billing_types?ids={0}";
                public const string JOAttachments = "jo_attachments?ids={0}";
                public const string JOStatus = "jo_status";
                public const string SaveResponse = "saveResponse";
                public const string GetResponseByResponseID = "getResponseByResponseID?responseID={0}&templateID={1}";
                public const string GetCompaniesByTemplateID = "getCompaniesByTemplateID?templateID={0}";
                public const string ResponseListByTemplateID = "listByTemplateID?{0}";
                public const string GetAllCompanies = "getAllCompanies";
                public const string GetBranchesByCompanyID = "getBranchesByCompanyID?companyID={0}";
                public const string ResetPassword = "add";
                public const string GetCompanies = "getCompanies";
                public const string GetAllTemplatesMobile = "getAllTemplatesMobile";
                public const string SyncResponseAndAnswer = "syncResponseAndAnswer";
                public const string UploadMedia = "uploadMedia";
            }

        }

        public static class Behavior
        {
            public const string EventName = "EventName";
            public const string Command = "Command";
            public const string CommandParameter = "CommandParameter";
            public const string Converter = "Converter";
            public const string OnEvent = "OnEvent";
        }

        public static class CultureInfo
        {
            public const string English = "en";
            public const string EnglishUs = "en-US";
        }

        public static class Keys
        {
            public const string ID = "id";
            public const string JobOrderIDs = "job_order_ids";
            public const string CreatedBy = "created_by";
            public const string CaseID = "case_id";
            public const string CaseStatus = "case_status";
            public const string ApplicationType = "application_type";
            public const string CaseNumber = "case_number";
            public const string UserName = "user_name";
            public const string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
            public const string ServerJobOrderID = "server_id";
            public const string LocalJobOrderID = "local_id";
            public const string UserTypeID = "user_type_id";
            public const string CompanyID = "company_id";
            public const string BranchID = "branch_id";
        }

       

       
        public static class Params
        {
            public const string ID = "ID";
            public const string FileName = "FileName";
            public const string AttachmentType = "AttachmentType";
            public const string JobOrderNumber = "JobOrderNumber";
            public const string ApplicationType = "ApplicationType";
            public const string JobOrderStatus = "Status";
            public const string CreatedBy = "CreatedBy";
            public const string IsDeleted = "IsDeleted";
            public const string Page = "Page";
            public const string PageSize = "PageSize";
            public const string Attachments = "Attachments";
            public const string Signature = "ClientSignature";
            public const string StatusAll = "All";
            public const string Pending = "Pending";
            public const string Signed = "Signed";
            public const string Sent = "Sent";
            public const string RequestRevert = "Requested For Revert";
            public const string WebPOS = "WebPOS";
            public const string Portfolio = "Portfolio";
            public const int WebPOSValue = 2;
            public const int PortfolioValue = 1;
            public const int PendingValue = 1;
            public const int SignedValue = 2;
            public const int SentValue = 3;
            public const int RequestRevertValue = 4;         
            public const string AssignedTo = "AssignedTo";
            public const string CaseID = "CaseID";
            public const string CaseStatus = "Status";
            public const string CaseNumber = "CaseNumber";
            public const string UserName = "UserName";
            public const string FromLogin = "FromLogin";
            public const string FirstPage = "FirstPage";
            public const string SecondPage = "SecondPage";
            public const string LastPage = "LastPage";
            public const string EditedCasesSelected = "EditedCasesSelected";
            public const string Cases = "Cases";
            public const string EditedCases = "EditedCases";
            public const string SelectedJobOrder = "SelectedJobOrder";
            public const string BillingTypes = "BillingTypes";
            public const string EditedBillingTypes = "EditedBillingTypes";
            public const string SignatureBytes = "SignatureBytes";
            public const string DefaultCases = "DefaultCases";
            public const string SignatureName = "SignatureName";
            public const string SignaturePath = "SignaturePath";
            public const string CaseSearch = "CaseSearch";
            public const string NewCaseSearch = "NewCaseSearch";
            public const string EditCaseSearch = "EditCaseSearch";
            public const string DeleteMenuItem = "DELETE";
            public const string RevertMenuItem = "REVERT";
            public const string Empty = "";
            public const string SignAgain = "sign_again";
            public const string IsEdit = "is_edit";
            public const string FromSecondPage = "from_second_page";
            public const string ResponseID = "responseID";
            public const string Title = "title";
            public const string Category = "category";
            public const string CompanyID = "companyID";
            public const string BranchID = "branchID";
            public const string TemplateID = "templateID";
            public const string IsMobile = "isMobile";
            public const string LocalResponseID = "localResponseID";
            public const string IsForcedFillUp = "isForcedFillUp";
        }

        public static class Modal
        {
            public const string Warning = "WARNING";
            public const string InfoMessage = "MESSAGE";
            public const string Confirmation = "CONFIRM";
            public const string InvalidFile = "INVALID FILE";
        }

        public static class Uploads
        {
            public const string SignatureTargetFolder = "/ClientSignature";
            public const string AttachmentsTargetFolder = "/Attachments";
            public const string ImagePrefix = "IMG";
            public const string VideoPrefix = "VID";
            public const string ImageFormat = ".jpg";
            public const string VideoFormat = ".mp4";
            public const int VideoCompressionQuality = 50;
            public const int VideoMaxSizeInBytes = 100000000; //100MB
        }

        public enum Status
        {
            Pending = 1,
            Signed = 2,
            Sent = 3,
            RequestedForRevert = 4

        }

        public static class Sqlite
        {
            public const string LocalDatabase = "SQLiteDb.db3";
        }

        public class PriorityLevel
        {
            public const string Low = "P3";
            public const string Medium = "P2";
            public const string High = "P1";
        }

        public class Color
        {
            public const string Low = "#4cd964";
            public const string Medium = "#ff9500";
            public const string High = "#ff3b30";
        }
        public class CheckpointAnswers
        {
            public const string ImagePrefix = "IMG";
            public const string VideoPrefix = "VID";
        }
        public enum UserType
        {
            SuperAdmin = 1,
            CompanyAdmin = 2,
            Employee = 3
        }

        public enum QuestionType
        {
            Text = 1,
            Checkbox = 2,
            Radio = 3,
            Video = 5,
            Image = 6,
            Slider = 7
        }
    }
}
