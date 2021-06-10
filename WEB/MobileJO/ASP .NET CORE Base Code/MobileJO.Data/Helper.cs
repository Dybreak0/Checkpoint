using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.EmailJO;
using MobileJO.Data.ViewModels.JobOrder;
using MobileJO.Data.ViewModels.LoanApplication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace MobileJO.Data
{

    public static class Helper
    {
        public static IQueryable<T> OrderByPropertyName<T>(this IQueryable<T> query, string attribute, string direction)
        {
            return ApplyOrdering(query, attribute, direction, "OrderBy");
        }

        public static IQueryable<T> ThenBy<T>(this IQueryable<T> query, string attribute, string direction)
        {
            return ApplyOrdering(query, attribute, direction, "ThenBy");
        }

        private static IQueryable<T> ApplyOrdering<T>(IQueryable<T> query, string attribute, string direction, string orderMethodName)
        {
            try
            {
                if (direction == Constants.SortDirection.Descending) orderMethodName += Constants.SortDirection.Descending;

                var t = typeof(T);

                var param = Expression.Parameter(t);
                var property = t.GetProperty(attribute);

                if (property != null)
                    return query.Provider.CreateQuery<T>(
                        Expression.Call(
                            typeof(Queryable),
                            orderMethodName,
                            new[] { t, property.PropertyType },
                            query.Expression,
                            Expression.Quote(
                                Expression.Lambda(
                                    Expression.Property(param, property),
                                    param))
                        ));
                else
                    return query;
            }
            catch (Exception)
            {
                return query;
            }
        }

        public static HttpResponseMessage ComposeResponse(HttpStatusCode statusCode, object responseData)
        {
            var jsonResponse = JsonConvert.SerializeObject(responseData);
            var resp = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(jsonResponse, Encoding.UTF8, Constants.Common.JSONContentType)
            };           
            
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue(Constants.Common.JSONContentType);

            return resp;
        }

        public static Dictionary<string, string[]> GetModelStateErrors(ModelStateDictionary modelState)
        {
            return modelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
        }


        public static string GetStatusColor(string status)
        {
            string color = string.Empty;

            switch (status)
            {
                case "Pending":
                    color = "#5ac8fa";
                    break;
                case "Sent":
                    color = "#FF9500";
                    break;
                case "Signed":
                    color = "#4CD964";
                    break;
                case "Requested For Revert":
                    color = "#5856D6";
                    break;
            }

            return color;
        }

      

        public static byte[] GetFileFromServer(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static string FormatEmailContentForRevert(string id, string firstName, string lastName)
        {
            var body = string.Empty;

            body = string.Format(Constants.Email.RevertBody,
                                    firstName,
                                    lastName,
                                    id);
            return body;
        }


        public static void GetErrors(Exception ex, out HttpStatusCode responseCode, out object responseData)
        {
            var message = Constants.Exception.ErrorProcessing;

            responseCode = HttpStatusCode.BadRequest;
            responseData = message;
        }

        public static string GetFormattedDate(DateTime date)
        {
            return (date == null) ? string.Empty : date.ToString(Constants.Common.DateFormat);
        }

        public static string GetPriorityColor(string level)
        {
            string color = string.Empty;

            switch(level)
            {
                case Constants.PriorityLevel.Low:
                    color = Constants.Color.Low;
                    break;
                case Constants.PriorityLevel.Medium:
                    color = Constants.Color.Medium;
                    break;
                case Constants.PriorityLevel.High:
                    color = Constants.Color.High;
                    break;
            }

            return color;
        }

        public static void AssembleRecipients(List<EmailAddressViewModel> emailSource, out List<string> toList, out List<string> ccList, out List<string> bccList)
        {
            toList = new List<string>();
            ccList = new List<string>();
            bccList = new List<string>();

            foreach (EmailAddressViewModel email in emailSource)
            {
                if(email.TypeID == 1)
                {
                    toList.Add(email.EmailAddress);
                }
                else if (email.TypeID == 2)
                {
                    ccList.Add(email.EmailAddress);
                }
                else
                {
                    bccList.Add(email.EmailAddress);
                }

            }
        }

        public static string FormatEmailContent(JobOrder jobOrderDetails, EmailJOViewModel emailDetails)
        {
            var body = string.Empty;
            string billed = string.Format(Constants.Email.IsBilled,
                                            (jobOrderDetails.IsBilled) ? Constants.Common.Check : Constants.Common.Space,
                                            (!jobOrderDetails.IsBilled) ? Constants.Common.Check : Constants.Common.Space);

            var billingTypes = new List<string>();
            string duration = string.Empty;

            foreach(JobOrderBillingType billType in jobOrderDetails.JobOrderBillingType)
            {
                billingTypes.Add(billType.BillingType.BillingTypeName);
            }

            var billingTypesFormatted = string.Join(Constants.Common.Comma, billingTypes.ToArray());

            duration = string.Format(Constants.Email.Duration, jobOrderDetails.DateTimeStart.ToShortTimeString(), jobOrderDetails.DateTimeEnd.ToShortTimeString());


            var dateOnlyFromTo = string.Format(Constants.Common.DateOnlyFromTo, jobOrderDetails.DateTimeStart.ToString(Constants.Common.DateOnlyFormat),
                jobOrderDetails.DateTimeEnd.ToString(Constants.Common.DateOnlyFormat));
            body = string.Format(Constants.Email.Body,
                                    dateOnlyFromTo,
                                    jobOrderDetails.JobOrderNumber,
                                    emailDetails.ConformeSlip,
                                    jobOrderDetails.Account.Name,
                                    billed,
                                    billingTypesFormatted,
                                    jobOrderDetails.ReportedByName,
                                    emailDetails.SupportType,
                                    jobOrderDetails.ApplicationTypeName,
                                    jobOrderDetails.JobOrderSubject,
                                    duration,
                                    jobOrderDetails.ActivityDetails,
                                    jobOrderDetails.RootCauseAnalysis,
                                    jobOrderDetails.NextStep,
                                    jobOrderDetails.PreventiveAction,
                                    jobOrderDetails.Remarks);

            return body;
        }
         

        public static HttpResponseMessage ExportToExcel(string content, string fileName)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                if (!string.IsNullOrEmpty(content))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(content.ToString());

                    result.Content = new ByteArrayContent(bytes.ToArray());
                    result.Content.Headers.Add(Constants.Reports.ContentHeader, fileName);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue(Constants.Common.OctetContentType);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(Constants.Reports.ContentDisposition);
                    result.Content.Headers.ContentDisposition.FileName = fileName;
                    result.StatusCode = HttpStatusCode.OK;
                    return result;
                }
                else
                {
                    result = ComposeResponse(HttpStatusCode.NotFound, Constants.Common.NoRecordsFound);
                    return result;
                }
            }
            catch
            {
                result = ComposeResponse(HttpStatusCode.InternalServerError, Constants.Common.BadRequest);
            }

            return result;
        }

        public static bool GetIdentityErrorResult(IdentityResult result, ModelStateDictionary modelState)
        {
            var flag = false;
            foreach (var error in result.Errors)
            {
                flag = true;
                modelState.AddModelError(Constants.Common.ModelStateErrors, error.Description);
            }

            if (result.Succeeded || result.Errors == null)
            {
                flag = false;
            }

            return flag;
        }

        #region Send Email
        /// <summary>
        /// Used to send email notification
        /// </summary>
        /// <param name="toList">List of email address to send email</param>
        /// <param name="ccList">List of email address to send email as CC</param>
        /// <param name="subject">Holds the subject of the email</param>
        /// <param name="body">Holds the content of the email</param>
        /// <returns>Boolean value if email was sent</returns>
        public static bool SendEmailNotification(List<string> toList, List<string> ccList, List<string> bccList, string subject, string body)
        {
            bool isSent = true;
            try
            {
                var mail = new MailMessage();
                var smtpClient = new SmtpClient(Constants.SMTP.SMTPClient);
                smtpClient.EnableSsl = true;
                smtpClient.Port = 587;

                var alternateView = AlternateView.CreateAlternateViewFromString(body, null, Constants.SMTP.Format);
                mail.From = new MailAddress(Constants.SMTP.EmailAddress, Constants.SMTP.EmailName);
                if (toList?.Count > 0)
                {
                    foreach (var email in toList)
                    {
                        mail.To.Add(email);
                    }

                    if (ccList?.Count > 0)
                    {
                        foreach (var email in ccList)
                        {
                            mail.CC.Add(email);
                        }
                    }

                    if(bccList?.Count > 0)
                    {
                        foreach (var email in bccList)
                        {
                            mail.Bcc.Add(email);
                        }
                    }

                    mail.Subject = subject;
                    mail.IsBodyHtml = true;
                    mail.AlternateViews.Add(alternateView);

                    smtpClient.Credentials = new NetworkCredential(Constants.SMTP.EmailAddress, Constants.SMTP.EmailPassword);

                    smtpClient.Send(mail);
                }
                else
                {
                    isSent = false;
                }
            }
            catch(Exception ex)
            {
                isSent = false;
            }
            return isSent;
        }
        #endregion

        public static bool SaveFileToServer(List<FileViewModel> files, string uploadPath)
        {
            bool areValidFiles = true;

            if (!IsValidFile(files))
            {
                areValidFiles = false;
            }

            foreach (var file in files)
            {                
                string path = string.Concat(uploadPath, file.FileName);

                if(File.Exists(path)) { areValidFiles = false; }                               
            }

            if(areValidFiles)
            {                
                foreach (var file in files)
                {
                    string path = string.Concat(uploadPath, file.FileName);
                    File.WriteAllBytes(path, file.FileDataArray);
                }

                return true;
            }

            return false;
        }

        public static void DeleteFiles(List<string> filenames, string uploadPath)
        {
            foreach (var filename in filenames)
            {
                string path = string.Concat(uploadPath, filename);

                if(DoesPathExist(uploadPath))
                {
                    File.Delete(path);
                }                
            }
        }

        public static bool CreateDirectory(string path)
        {
            if(!DoesPathExist(path))
            {
                Directory.CreateDirectory(path);
                return true;
            }

            return false;
        }

        public static bool DoesPathExist(string path)
        {
            if (Directory.Exists(path))
                return true;

            return false;
        }

        public static bool IsValidFile(List<FileViewModel> files)
        {
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(Path.GetInvalidFileNameChars() + "]"));
            
            const int threeMegaBytes = 3145728;
            
            foreach (var file in files)
            {
                int fileSize = file.FileDataArray.Length;

                if ( containsABadCharacter.IsMatch(file.FileName) || file.FileName.Length > 250 || fileSize > threeMegaBytes )
                {
                    return false;
                }
            }
            
            return true;
        }

        public static HttpResponseMessage DownloadFile(string fileName, string filePath)
        {
            HttpResponseMessage result = new HttpResponseMessage();

            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    var path = filePath + fileName;

                    // Check if the directory and file exists
                    if (Directory.Exists(filePath) && File.Exists(path))
                    {
                        byte[] bytes = File.ReadAllBytes(path);

                        // Check if the file has content
                        if (bytes.Length == 0)
                        {
                            result = ComposeResponse(HttpStatusCode.NotFound, Constants.Attachment.FileEmpty);
                        }
                        else
                        {
                            result.Content = new ByteArrayContent(bytes);
                            result.Content.Headers.Add(Constants.Common.XFileName, fileName);
                            result.Content.Headers.ContentType = new MediaTypeHeaderValue(Constants.Common.OctetContentType);
                            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(Constants.Common.Attachment);
                            result.Content.Headers.ContentDisposition.FileName = fileName;
                            result.StatusCode = HttpStatusCode.OK;
                        }                                                
                    }
                    else
                    {
                        result = ComposeResponse(HttpStatusCode.NotFound, Constants.Attachment.FileNotFound);
                    }
                }
                else
                {
                    result = ComposeResponse(HttpStatusCode.NotFound, Constants.Attachment.FileNotFound);
                }
            }
            catch
            {
                result = ComposeResponse(HttpStatusCode.InternalServerError, Constants.Common.BadRequest);
            }

            return result;
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool isNumberNullOrZero(int value)
        {
            return 0 >= value || value.Equals(null) || "".Equals(value.ToString());
        }
        
        /// <summary>
        ///     Sends confirmation email to user.
        /// </summary>
        /// <param name="user">User record</param>
        /// <returns></returns>
        public static void SendConfirmationEmail(string fromEmail, string password, string brandName, User user)
        {

            using (MailMessage mm = new MailMessage(fromEmail, user.EmailAddress))
            {
                mm.Subject = brandName;
                string body = "Dear " + user.FirstName + ",";
                body += "<br /><br />";
                body += "<br /><br />You have been successfully added to Alliance Checkpoint.";
                body += "<br /><br />You can now login ";
                if (user.UserTypeID != Constants.User.EmployeeID) 
                {
                    body += "to " + Constants.Link.ConfirmationEmail;
                }
                body += " with the following credentials:";
                body += "<br /><br />Username :" + user.UserName;
                body += "<br /><br />Password :" + user.Password;
                mm.Body = body;
                mm.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(fromEmail, password);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }

        public static void SendPasswordLink(string fromEmail, string password, string brandName, User user, string link)
        {

            using (MailMessage mm = new MailMessage(fromEmail, user.EmailAddress))
            {
                mm.Subject = brandName;
                string body = "Dear " + user.FirstName + ",";
                body += "<br /><br />";
                body += "<br /><br />Please open the link and fill up the form to reset your password.";
                body += "<br /><br /><a href=" + link + "><b>Password Reset Link</b></a>";
                mm.Body = body;
                mm.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(fromEmail, password);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }

        public static string getMediaAnswer(int questionTypeID, string value)
        {
            if ((int)Constants.QuestionType.Image == questionTypeID
                && !string.IsNullOrEmpty(value)
                && File.Exists(value))
            {
                return Constants.Questionnaire.ImageDataFormat;
            }

            if ((int)Constants.QuestionType.Video == questionTypeID
                && !string.IsNullOrEmpty(value)
                && File.Exists(value))
            {
                return Constants.Questionnaire.VideoDataFormat;
            }

            return value;
        }

        // LOANS
        public static bool SaveLoanFileToServer(List<LoanFileViewModel> files, string uploadPath)
        {
            bool areValidFiles = true;

            if (!IsValidLoanFile(files))
            {
                areValidFiles = false;
            }

            foreach (var file in files)
            {
                string path = string.Concat(uploadPath, file.FileName);

                if (File.Exists(path)) { areValidFiles = false; }
            }

            if (areValidFiles)
            {
                foreach (var file in files)
                {
                    string path = string.Concat(uploadPath, file.FileName);
                    File.WriteAllBytes(path, file.FileDataArray);
                }

                return true;
            }

            return false;
        }

        public static bool IsValidLoanFile(List<LoanFileViewModel> files)
        {
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(Path.GetInvalidFileNameChars() + "]"));

            const int threeMegaBytes = 3145728;

            foreach (var file in files)
            {
                int fileSize = file.FileDataArray.Length;

                if (containsABadCharacter.IsMatch(file.FileName) || file.FileName.Length > 250 || fileSize > threeMegaBytes)
                {
                    return false;
                }
            }

            return true;
        }

        public static HttpResponseMessage LoanExportToExcel(string content, string fileName)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                if (!string.IsNullOrEmpty(content))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(content.ToString());

                    result.Content = new ByteArrayContent(bytes.ToArray());
                    result.Content.Headers.Add(Constants.Loans.ContentHeader, fileName);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue(Constants.Common.OctetContentType);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(Constants.Loans.ContentDisposition);
                    result.Content.Headers.ContentDisposition.FileName = fileName;
                    result.StatusCode = HttpStatusCode.OK;
                    return result;
                }
                else
                {
                    result = ComposeResponse(HttpStatusCode.NotFound, Constants.Common.NoRecordsFound);
                    return result;
                }
            }
            catch
            {
                result = ComposeResponse(HttpStatusCode.InternalServerError, Constants.Common.BadRequest);
            }

            return result;
        }

    }
}
