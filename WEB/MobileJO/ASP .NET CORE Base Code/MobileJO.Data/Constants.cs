using Microsoft.Extensions.Configuration;
namespace MobileJO.Data
{
    public static class Constants
    {

        public class Token
        {
            public const string Issuer = "BaseCode:Issuer";
            public const string Audience = "BaseCode:Audience";
            public const string SecretKey = "BaseCode:AuthSecretKey";
            public const string ExpirationMinutes = "BaseCode:ExpirationMinutes";
            public const string POST = "POST";
            public const string TokenPath = "TokenPath";
            public const string Username = "username";
            public const string Password = "password";
            public const string RefreshToken = "refresh_token";
            public const string UserID = "user_id";
            public const string MobileLogin = "mobile_login";
        }

        public class EmailConfiguration
        {
            public const string Host = "Email:Host";
            public const string Port = "Email:Port";
            public const string UserNamme = "Email:UserName";
            public const string Password = "Email:Password";
            public const string FromAddress = "Email:FromAddress";
            public const string FromDisplayName = "Email:FromDisplayName";
            public const string EnableSSL = "Email:EnableSSL";
            public const string EnabledDefaultCredentials = "Email:EnabledDefaultCredentials";
        }

        public class ForgotPasswordConfiguration
        {
            public const string EmailRecipient = "ForgotPassword:EmailRecipient";
            public const string EmailCredential = "ForgotPassword:EmailCredential";
            public const string EmailSubjectBrandName = "ForgotPassword:EmailSubjectBrandName";
        }
        public class ConfirmationEmailConfiguration
        {
            public const string EmailRecipient = "ConfirmationEmail:EmailRecipient";
            public const string EmailCredential = "ConfirmationEmail:EmailCredential";
            public const string EmailSubjectBrandName = "ConfirmationEmail:EmailSubjectBrandName";
        }

        public class SortDirection
        {
            public const string Ascending = "Ascending";
            public const string Descending = "Descending";
        }

        public class Claims
        {
            public const string Id = "userId";
            public const string UserName = "userName";
            public const string Role = "userRole";
        }

        public class Common
        {

            public const string BaseCode = "BaseCode";
            public const string OAuth = "/oauth";
            public const string Client = "Client";
            public const string ClientID = "ClientID";
            public const string ClientSecret = "ClientSecret";
            public const string JSONContentType = "application/json";
            public const string OctetContentType = "application/octet-stream";
            public const string Zero = "0";
            public const string Route = "api/[controller]/[action]";
            public const string Bearer = "Bearer";
            public const string NameFormat = "{0} {1}";
            public const string Comma = ", ";
            public const string All = "All";
            public const string DateFormatWithName = "{0} by {1}";
            public const string DateFormat = "MM/dd/yyyy h:mm tt";
            public const string DateOnlyFormat = "MM/dd/yyyy";
            public const string DateOnlyFromTo = "{0} - {1}";
            public const string Check = "X";
            public const string Space = "&nbsp; ";
            public const string GuidFormat = "N";
            public const string AppSettingsJson = "appsettings.json";
            public const string DefaultConnection = "DefaultConnection";
            public const string Attachment = "attachment";
            public const string XFileName = "x-filename";
            public const string DoubleDash = "\\";
            public const string GreaterThan = ">";
            public const char CommaChar = ',';
            public const char Priority = 'P';
            public const string MonthNameDateFormat = "MMMM dd, yyyy";

            // Messages
            public const string BadRequest = "Bad Request";

            public const string ErrorSave = "Failed to save record.";
            public const string SuccessSave = "Record successfully saved.";
            public const string ErrorDelete = "Failed to delete record.";
            public const string SuccessDelete = "Record successfully deleted.";
            public const string NoResults = "No results found.";
            public const string Success = "Success.";
            public const string RecordInvalid = "Record Invalid.";
            public const string ModelStateErrors = "Invalid input.";

            public const string RecordHasPendingJOs = "Record has pending JOs.";
            public const string NotAdmin = "Cannot perform action. User is no longer an administrator.";
            public const string deletedUser = "Your account has been deleted.";
            public const string Pending = "Pending";
            public const string Admin = "admin";

            public const string NoRecordsFound = "No records found.";
            public const string RecordNotExist = "Record does not exist.";
            public const string RecordDoesNotExist = "Record does not exist.";
            public const string InvalidJobOrderId = "Invalid Job order ID";
            public const string RecordSaved = "Record succesfully saved.";
            public const string RecordSynced = "Records succesfully synced.";
            public const string JobOrderNumberFormat = "000000.##";
            public const string SignatureNameExtension = "signature.png";
            public const string Closed = "Closed";

            public const string ForgetPasswordSent = "Email has been successfully sent.";
            public const string RequestRevertSent = "Revert request is sent";
            public const string CannotRequestRevert = "Cannot request revert";
            public const string CannotDelete = "Cannot Delete Job Order if not Pending";
            public const string Desc = "dsc";
            public const string JobOrderNumber = "JobOrderNumber";
            public const int PendingValue = 1;
            public const int SignedValue = 2;
            public const int SentValue = 3;
            public const int RequestRevertValue = 4;

            public const string AllowedToLogin = "AllowedToLogin";
            public const string MaximumEmailReached = "Cannot add more than 50 email addresses.";
            public const int MaxEmailCount = 50;
            public const string RoleID = "RoleID";
            public const string CompanyID = "CompanyID";

            public const string StartDateGreaterThanEndDate = "End Date must be greater than Start Date";
            public const string MissingRequiredField = "Missing required fields";
            public const string MaximumQuestionsReached = "Maximum allowable questions for each template reached.";

            public const string UploadMediaFailed = "Failed to upload the media. Please try again";
            public const string UploadMediaSuccess = "The media has been successfully uploaded.";

            public const string LinkExpired = "Link already expired.";

            public class EmailType
            {
                public const int To = 1;
                public const int Cc = 2;
                public const int Bcc = 3;
            }
        }

        public class User
        {
            public const string InvalidUserNamePassword = "Incorrect username or password.";
            public const string UserNotAdmin = "Login failed. User is not an Admin user.";
            public const string UserExist = "Username already exists.";
            public const string UserNotExist = "User does not exist.";

            public const string Administrator = "Administrator";
            public const string UserUser = "User";
            public const int AdminID = 1;
            public const int CompanyAdminID = 2;
            public const int EmployeeID = 3;
            public const string defaultPassword = "Alliance@12345";

        }

        public class Student
        {
            // Sort Keys
            public const string StudentHeaderId = "stud_id";
            public const string StudentHeaderName = "stud_name";
            public const string StudentHeaderEmail = "stud_email";
            public const string StudentHeaderClass = "stud_class";
            public const string StudentHeaderEnrollYear = "stud_enrollYear";
            public const string StudentHeaderCity = "stud_city";
            public const string StudentHeaderCountry = "stud_country";

            // Messages
            public const string StudentNameExists = "Student name already exists";
            public const string StudentEntryInvalid = "Student entry is not valid!";
            public const string StudentNotExist = "Student does not exist.";
            public const string StudentDoesNotExists = "Student does not exist.";
            public const string StudentSuccessAdd = "Student added successfully.";
            public const string StudentSuccessEdit = "Student is updated successfully.";
            public const string StudentSuccessDelete = "Student is deleted successfully.";
        }
        public class JOStatus
        {
            public const string Pending = "Pending";
            public const string StatusIsSent = "Sent";
            public const string RequestRevert = "RequestRevert";
            public const string Signed = "Signed";
            public const string CannotRevertJO = "Cannot Revert JO";
            public const string RequestToRevert = "4";
        }
        public class Exception
        {
            public const string ErrorProcessing = "An error was encountered while processing the request. Please refresh the page and try again. If the problem still occurs, contact your administrator.";
            public const string IO = "An error was encountered while processing a file upload/download request.";
            public const string DB = "An error was encountered while processing a database request.";
            public const string SMTP = "An error was encountered while processing an email sending request.";
        }

        public class Reports
        {
            // Action Names
            public const string GetJobOrderDetails = "getJobOrder";
            public const string GetAssignedCaseDetails = "getAssignedCase";
            public const string GetJobOrderReport = "getJobOrderReport";
            public const string GetAssignedCasesReport = "getAssignedCasesReport";
            public const string GetJobOrderClientRatingReport = "getJobOrderClientRatingReport";
            public const string DownloadJobOrderReport = "downloadJobOrderReport";
            public const string DownloadAssignedCasesReport = "downloadAssignedCasesReport";
            public const string DownloadJobOrderClientRatingReport = "downloadJobOrderClientRatingReport";

            // Excel Related Constants
            public const string JobOrderReport = "TS JO Report";
            public const string AssignedCasesReport = "Assigned Cases Report";
            public const string JobOrderClientRatingReport = "JO Client Rating Report.xls";
            public const string InitialExcelFilename = "{0}.xls";
            public const string ContentDisposition = "attachment";
            public const string ContentHeader = "x-filename";
            public const string ExcelTable = "<html><body><table border='1'><tr>{0}</tr>{1}</table></body></html>";
            public const string JobOrderReportExcelTableHeaders = "<th>JO #</th>" +
                                                                  "<th>Case #</th>" +
                                                                  "<th>JO Subject</th>" +
                                                                  "<th>Reported By</th>" +
                                                                  "<th>JO Start Date</th>" +
                                                                  "<th>JO End Date</th>" +
                                                                  "<th>Application Type</th>" +
                                                                  "<th>Status</th>";
            public const string AssignedCasesReportExcelTableHeaders = "<th>Case #</th>" +
                                                                       "<th>Subject</th>" +
                                                                       "<th>Account Name</th>" +
                                                                       "<th>Application Type</th>" +
                                                                       "<th>Status</th>";
            public const string JobOrderClientRatingReportExcelTableHeaders = "<th>JO #</th>" +
                                                                              "<th>Case #</th>" +
                                                                              "<th>Application Type</th>" +
                                                                              "<th>JO Start Date</th>" +
                                                                              "<th>JO End Date</th>" +
                                                                              "<th>Reported By</th>" +
                                                                              "<th>Account Name</th>" +
                                                                              "<th>Client Rating</th>";
            public const string JobOrderReportExcelTableRows = "<tr>" +
                                                               "<td>{0}</td>" +
                                                               "<td>{1}</td>" +
                                                               "<td>{2}</td>" +
                                                               "<td>{3}</td>" +
                                                               "<td>{4}</td>" +
                                                               "<td>{5}</td>" +
                                                               "<td>{6}</td>" +
                                                               "<td>{7}</td>" +
                                                               "</tr>";
            public const string AssignedCasesReportExcelTableRows = "<tr>" +
                                                                    "<td>{0}</td>" +
                                                                    "<td>{1}</td>" +
                                                                    "<td>{2}</td>" +
                                                                    "<td>{3}</td>" +
                                                                    "<td>{4}</td>" +
                                                                    "</tr>";
            public const string JobOrderClientRatingReportExcelTableRows = "<tr>" +
                                                                           "<td>{0}</td>" +
                                                                           "<td>{1}</td>" +
                                                                           "<td>{2}</td>" +
                                                                           "<td>{3}</td>" +
                                                                           "<td>{4}</td>" +
                                                                           "<td>{5}</td>" +
                                                                           "<td>{6}</td>" +
                                                                           "<td>{7}</td>" +
                                                                           "<td>{8}</td>" +
                                                                           "</tr>";
        }

        public class RevertJO
        {
            // Action Names
            public const string GetRevertJOList = "getRevertJOList";
            public const string RevertJORequest = "revertJO";

            // Messages
            public const string RequestApproved = "Request successfully approved.";
            public const string RequestDenied = "Request successfully denied.";
            public const string NotForRevert = "JO has already been approved/denied for revert.";
        }

        public class Loans
        {
            public const string LoanApplicationExcelTableRows = "<tr>" +
                                                               "<td>{0}</td>" +
                                                               "<td>{1}</td>" +
                                                               "<td>{2}</td>" +
                                                               "<td>{3}</td>" +
                                                               "<td>{4}</td>" +
                                                               "</tr>";
            public const string ExcelTable = "<html><body><table border='1'><tr>{0}</tr>{1}</table></body></html>";
            public const string ExcelTableHeaders = "<th>APP#</th>" +
                                                            "<th>Client Name</th>" +
                                                            "<th>Date Created</th>" +
                                                            "<th>Created By</th>" +
                                                            "<th>Application Status</th>";
            public const string LoanApplications = "Loan Applications List";
            public const string InitialExcelFilename = "{0}.xls";
            public const string ContentHeader = "x-filename";
            public const string ContentDisposition = "attachment";

            public const string LoanApproved = "Installment Application has been approved.";
            public const string LoanDenied = "Installment Application has been denied.";
            public const string LoanStatusResponded = "Installment Application has already been approved/denied.";
        }
        public enum Status
        {
            Pending = 1,
            Signed = 2,
            Sent = 3,
            RequestedForRevert = 4
        }

        public class Color
        {
            public const string Low = "#4cd964";
            public const string Medium = "#ff9500";
            public const string High = "#ff3b30";
        }

        public class PriorityLevel
        {
            public const string Low = "P3";
            public const string Medium = "P2";
            public const string High = "P1";
        }

        public class Email
        {
            public const string IsBilled = "[{0}] Billed <br> [{1}] Not Billed";
            public const string Subject = "{0} - {1}";
            public const string Duration = "{0} - {1}";
            public const string Body = @"<table border='1' style='font-family:'Courier New';  border: 1px solid black;' >
	                                    <tbody>
		                                    <tr>
			                                    <td colspan = '7' align='center' color='black' bgcolor='#D4D4D4'  ><b>EMAIL JO REPORT</b></td>
		                                    </tr>
		                                    <tr>
			                                    <td colspan = '2' color='black' bgcolor='#D4D4D4'><b>Date</b></td>
			                                    <td>{0}</td>
			
			                                    <td color = 'black' bgcolor='#D4D4D4'><b>JO #</b></td>
			                                    <td>{1}</td>
			
			                                    <td color = 'black' bgcolor='#D4D4D4'><b>Conforme Slip #</b></td>
			                                    <td>{2}</td>
		                                    </tr>
		                                    <tr>
			                                    <td color = 'black' bgcolor='#D4D4D4'><b>Client Name</b></td>
			                                    <td>{3}</td>
			
			                                    <td rowspan = '2' color='black' bgcolor='#D4D4D4'><b>Billing Status</b></td>
			                                    <td rowspan = '2' colspan= '2' >{4}</td>
			                                    <td rowspan = '2' colspan='2'>{5}</td>
		                                    </tr>
		                                    <tr>
			                                    <td color = 'black' bgcolor='#D4D4D4'><b>TS Assigned</b></td>
			                                    <td>{6}</td>
		                                    </tr>
		                                    <tr>
			                                    <td color = 'black' bgcolor='#D4D4D4'><b>Support Type</b></td>
			                                    <td>{7}</td>
			                                    <td color = 'black' bgcolor='#D4D4D4'><b>Product Name</b></td>
			                                    <td colspan = '4' >{8}</td>
		                                    </tr>
		                                    <tr color = 'black' bgcolor='#D4D4D4'>
			                                    <td><b>Subject</b></td>
			                                    <td><b>Duration</b></td>
			                                    <td><b>Task Done</b></td>
			                                    <td><b>Root Cause</b></td>
			                                    <td><b>Next Step</b></td>
			                                    <td><b>Preventive Action</b></td>
			                                    <td><b>Remarks</b></td>
		                                    </tr>
		                                    <tr>
			                                    <td>{9}</td>
			                                    <td>{10}</td>
			                                    <td style='white-space: pre;'>{11}</td>
			                                    <td>{12}</td>
			                                    <td>{13}</td>
			                                    <td>{14}</td>
                                                <td>{15}</td>
		                                    </tr>

	                                    </tbody>
                                    </table>";
            public const string SubjectRevert = "Request for Revert - [{0}]";
            public const string RevertBody = @"<body> 
                                               <p  color='black' bgcolor='#D4D4D4'>Hi Admin,</p><p></br></br></br>		                                   
 	                                           This is to inform you that {0} {1} has requested to revert {2}.</p></br></br></br>
                                               <p>Thank you,</p></br></br></br><p>Alliance Checkpoint</p></body>";
        }

        #region SMTP
        /// <summary>
        /// Constants for SMTP
        /// </summary>
        public static class SMTP
        {
            public const string Format = "text/html";

            public static string SMTPClient
            {
                get
                {
                    return "smtp.gmail.com";
                }
            }

            public static string EmailAddress
            {
                get
                {
                    return "checkpoint.alliance@gmail.com";
                }
            }

            public static string EmailPassword
            {
                get
                {
                    return "Alliance@123";
                }
            }

            public static string GroupEmail
            {
                get
                {
                    return "";
                }
            }

            public static string Email
            {
                get
                {
                    return "checkpoint.alliance@gmail.com";
                }
            }

            public static string EmailName
            {
                get
                {
                    return "Alliance Checkpoint";
                }
            }
        }
        #endregion

        public class Account
        {
            // Messages
            public const string AccountExist = "Name already exist.";
            public const string AccountNotExist = "Account does not exist.";

            // Temporary Data
            public const string TempEmail = "acccountTempEmail@gmail.com";
            public const string NA = "N/A";
            public const int TempID = 1;

        }

        public class ClaimTypes
        {
            // Messages
            public const string UserName = "user_name";
            public const string ID = "id";
            public const string UserId = "user_id";
            public const string FullName = "full_name";
            public const string UserTypeID = "user_type_id";
            public const string CompanyID = "company_id";
            public const string RoleID = "role_id";
            public const string BranchID = "branch_id";
        }

        public class Roles
        {
            // Messages
            public const string Administrator = "Administrator";
            public const string User = "User";

        }

        public class Upload
        {
            public const string Attachment = "Attachments";
            public const string ClientSignature = "ClientSignature";
        }

        public class JobOrder
        {
            // Action Names
            public const string GetJobOrder = "jobOrderGet";
            public const string GetApplicationsTypes = "applicationTypes";
            public const string GetAccounts = "accountsList";
            public const string GetBillingTypes = "billingTypes";
            public const string GetAssignedCases = "caseList";
            public const string GetTaggedCases = "taggedCaseList";
            public const string GetJOBillingTypes = "joBillingTypeList";
            public const string GetJOAttachments = "joAttachments";
            public const string SaveJobOrder = "saveJO";
            public const string UpdateJobOrder = "updateJO";
            public const string SyncJobOrder = "syncJO";
            public const string SaveSignature = "saveSignature";
            public const string UserCasesList = "userCasesList";
            public const string Route = "api/[controller]/[action]";
            public const string JobOrderList = "jobOrderList";
            public const string View = "view";
            public const string List = "list";
            public const string TaggedCasesList = "tagged_cases_list";
            public const string ViewCase = "viewCase";
            public const string ApplicationType = "application_type";
            public const string JobOrderStatus = "job_order_status";
            public const string Delete = "delete";
            public const string RequestRevert = "requestRevert";
            public const string AttachmentList = "attachment_list";
            public const string BillingList = "billing_list";
            public const string JOList = "joList";
            public const string RequestRevertCount = "requestRevertCount";
            public const string Search = "search";
            public const string GetJOTaggedCases = "JoTaggedCaseList";
            public const string JOStatus = "jo_status";
            // Messages

            // Other
            public const string JobOrderNumberPrefix = "JO-";

        }

        public class Attachment
        {
            // Messages
            public const string FileNotFound = "File cannot be found.";
            public const string FileEmpty = "File is empty.";

            public const string DownloadAttachment = "downloadAttachment";
            public const string DownloadImage = "downloadImage";
            public const string AttachmentPath = "Attachment:AttachmentPath";
            public const string FullFilePath = "{0}\\{1}\\{2}\\{3}\\";
        }

        public class TFSIntegration
        {
            public const string WebPOSPersonalAccessToken = "TFS_WebPOS:PersonalAccessToken";
            public const string PortfolioPersonalAccessToken = "TFS_Portfolio:PersonalAccessToken";
            public const string WebPOSServer = "TFS_WebPOS:Server";
            public const string PortfolioServer = "TFS_Portfolio:Server";

            public const string CredentialsFormat = "{0}:{1}";
            public const string JSONType = "application/json";
            public const string BasicAuthentication = "Basic";
            public const string POST = "POST";
            public const string SyncWithThirdParty = "sync";
            public const string API = "URL:API";
            public const string SelectQuery = "SELECT [State] FROM WorkItems WHERE [Work Item Type] = 'Product Backlog Item' OR [Work Item Type] = 'Bug' ORDER BY [System.CreatedDate] DESC";
            public const string GetAsyncURL = "_apis/wit/workitems?ids=";
            public const string GetAsynVersion = "&api-version=4.0";
        }

        public class Application
        {
            public const string Portfolio = "Portfolio";
            public const string WebPOS = "WebPOS";
            public const string HRIS = "HRIS Notes";
        }

        public class Response
        {
            public const string DownloadResponseSummary = "downloadResponseSummary";
            public const string ResponseReport = "Response Report";
            public const string ExcelTableResponse = "<html><body><table border='1'><tr>{0}</tr>{1}</table></body></html>";
            public const string ResponseExcelTableHeaders = "<th>Title</th>" +
                                                            "<th>Company - Branch</th>" +
                                                            "<th>Category</th>" +
                                                            "<th>Date Submitted</th>" +
                                                            "<th>Submitted By</th>" +
                                                            "<th>Status</th>";

            public const string ResponseExcelTableRows = "<tr>" +
                                                        "<td>{0}</td>" +
                                                        "<td>{1}</td>" +
                                                        "<td>{2}</td>" +
                                                        "<td>{3}</td>" +
                                                        "<td>{4}</td>" +
                                                        "<td>{5}</td>" +
                                                        "</tr>";
        }
        public enum UserType
        {
            SuperAdmin = 1,
            CompanyAdmin = 2,
            Employee = 3
        }

        public enum RoleType
        {
            Administrator = 1,
            User = 2
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

        public class Link
        {
            public const string ForgotPassword = "http://localhost:8080/#/changePassword/{0}/{1}";
            public const string ConfirmationEmail = "http://localhost:8080/#/login";

            //For Production 
            //public const string ForgotPassword = "http://alliance-checkout.com.ph:55735/#/changePassword/{0}/{1}";
        }
        
        public class Questionnaire
        {
            public const string SaveTemplate = "saveTemplate";
            public const string CannotDeleteActiveTemplate = "Cannot delete active template.";
            public const string Delete = "delete";
            public const string GetBranches = "get_branches";
            public const string GetQuestionTypes = "get_question_types";
            public const string GetAllTemplatesMobile = "getAllTemplatesMobile";
            public const string SyncResponseAndAnswer = "syncResponseAndAnswer";
            public const string MediaFullFilePath = "{0}\\{1}";
            public const string ImageDataFormat = "data:image/jpg;base64,";
            public const string VideoDataFormat = "data:video/mp4;base64,";
            public const int MaxQuestionsAllowed = 10;
        }

        public class CheckpointAnswers
        {
            public const string CheckpointAnswersPath = "CheckpointAnswersPath";
            public const string FullFilePath = "{0}\\{1}\\";
            public const string ImagePrefix = "IMG";
            public const string VideoPrefix = "VID";
        }
    }
}
