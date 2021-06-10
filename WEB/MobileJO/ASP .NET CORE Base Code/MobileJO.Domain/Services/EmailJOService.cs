using AutoMapper;
using Microsoft.Extensions.Configuration;
using MobileJO.Data;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.EmailJO;
using MobileJO.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace MobileJO.Domain.Services
{
    public class EmailJOService : IEmailJOService
    {
        private readonly IEmailJORepository _emailJORepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        /// <summary>
        ///     Constructor for IEmailJORepository and mapper
        /// </summary>
        /// <param name="emailJORepository"></param>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        public EmailJOService(IEmailJORepository emailJORepository, IMapper mapper, IConfiguration configuration)
        {
            _emailJORepository = emailJORepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        ///     Calls EmailJO repository method GetAll().
        /// </summary>
        /// <returns></returns>
        public ListViewModel GetAll()
        {
            return _emailJORepository.GetAll();
        }

        /// <summary>
        ///    Calls EmailJO repository method SaveEmails().
        /// </summary>
        /// <param name="emailsJson"></param>
        public void SaveEmails(string emailsJson)
        {
            _emailJORepository.SaveEmails(emailsJson);
        }

        /// <summary>
        ///    Calls EmailJO repository method SearchJobOrder().
        /// </summary>
        /// <param name="jobOrderNos"></param>
        public List<JobOrder> SearchJobOrder(List<string> jobOrderNos)
        {
            return _emailJORepository.SearchJobOrder(jobOrderNos);
        }

        /// <summary>
        ///    Initiates email sending.
        /// </summary>
        /// <param name="emailDetails"></param>
        public void SendEmailJO(EmailJOViewModel emailDetails, int userID, string senderFullname)
        {
            var jobOrderDetails = _emailJORepository.SearchJobOrder(emailDetails.JobOrderNos);
            var emailRecipients = new List<Data.ViewModels.Common.EmailViewModel>();

            var toList = new List<string>();
            var ccList = new List<string>();
            var bccList = new List<string>();

            if (emailDetails.UseDefaultAddress)
            {
                var defaultEmails = _emailJORepository.GetAllEmailRecipient();
                Helper.AssembleRecipients(defaultEmails,
                                            out toList,
                                            out ccList,
                                            out bccList);

            }
            else
            {
                Helper.AssembleRecipients(emailDetails.Recipient,
                                            out toList,
                                            out ccList,
                                            out bccList);
            }

            foreach (JobOrder jobOrder in jobOrderDetails)
            {
                string subject = string.Format(Constants.Email.Subject, jobOrder.JobOrderSubject, senderFullname);
                string body = Helper.FormatEmailContent(jobOrder, emailDetails);
                bool isSent = SendEmailNotification(toList, ccList, bccList, subject, body);

                if(isSent)
                   _emailJORepository.UpdateJobOrder(jobOrder.ID, userID);                
            }

        }

        /// <summary>
        ///    Calls EmailJO repository method GetTaggedCases().
        /// </summary>
        /// <param name="createdBy"></param>
        /// <param name="caseID"></param>
        public List<SelectJOViewModel> GetTaggedCases(int createdBy, int caseID)
        {
            return _emailJORepository.GetTaggedCases(createdBy, caseID);
        }

        public void SendEmailJORevert(string id, User user)
        {
            var firstName = user.FirstName;
            var lastName = user.LastName;

            var toList = new List<string>();
            var ccList = new List<string>();

            var adminEmails = _emailJORepository.GetAllAdminEmailRecipient();

            foreach(var email in adminEmails)
            {
                if(toList.Count == 0)
                {
                    toList.Add(email);                    
                }
                else
                {
                    ccList.Add(email);
                }
            }

            string subject = string.Format(Constants.Email.SubjectRevert, id);
            string body = Helper.FormatEmailContentForRevert(id, firstName, lastName);
            SendEmailNotification(toList, ccList, null, subject, body);

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
        private bool SendEmailNotification(List<string> toList, List<string> ccList, List<string> bccList, string subject, string body)
        {
            bool isSent = true;

            try
            {
                var mail = new MailMessage();

                var smtpClient = new SmtpClient(_configuration[Constants.EmailConfiguration.Host]);

                smtpClient.EnableSsl = bool.Parse(_configuration[Constants.EmailConfiguration.EnableSSL]);
                smtpClient.Port = int.Parse(_configuration[Constants.EmailConfiguration.Port]);

                var alternateView = AlternateView.CreateAlternateViewFromString(body,
                                                                                null,
                                                                                Constants.SMTP.Format);

                mail.From = new MailAddress(_configuration[Constants.EmailConfiguration.FromAddress],
                                            _configuration[Constants.EmailConfiguration.FromDisplayName]);

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

                    if (bccList?.Count > 0)
                    {
                        foreach (var email in bccList)
                        {
                            mail.Bcc.Add(email);
                        }
                    }

                    mail.Subject = subject;
                    mail.IsBodyHtml = true;
                    mail.AlternateViews.Add(alternateView);

                    smtpClient.Credentials = new NetworkCredential(_configuration[Constants.EmailConfiguration.FromAddress],
                                                                   _configuration[Constants.EmailConfiguration.Password]);

                    smtpClient.Send(mail);
                }
                else
                {
                    isSent = false;
                }
            }
            catch (Exception ex)
            {
                isSent = false;
            }
            return isSent;
        }
        #endregion

        /// <summary>
        ///     Calls EmailJO repository method EmailCount().
        /// </summary>
        /// <returns></returns>
        public int EmailCount()
        {
            return _emailJORepository.GetEmailCount();
        }
    }
}
