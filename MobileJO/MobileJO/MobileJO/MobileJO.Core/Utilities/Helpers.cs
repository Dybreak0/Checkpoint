using MobileJO.Core.Models;
using Plugin.SecureStorage;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace MobileJO.Core.Utilities
{
    public class Helpers
    {
        /// <summary>
        ///    Concatenates the parameters for GET methods
        /// </summary>
        /// <param name="oParam"></param>
        /// <returns></returns>
        public static string GetParamString(Dictionary<string,string> oParam)
        {
            StringBuilder sParams = new StringBuilder();
            int iCount = 0;
            foreach (KeyValuePair<string, string> item in oParam)
            {
                sParams.Append(item.Key);
                sParams.Append("=");
                sParams.Append(System.Net.WebUtility.UrlEncode(item.Value));
                iCount += 1;
                if (iCount < oParam.Count)
                {
                    sParams.Append("&");
                }
            }

            return sParams.ToString();
        }

        /// <summary>
        ///    Checks if the user has valid session.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public static bool HasValidSession()
        {
            return (CrossSecureStorage.Current.HasKey(Constants.AppSettings.AccessToken) ? true : false);
        }

        /// <summary>
        ///    Assembles all email according to their type
        /// </summary>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <returns></returns>
        public static List<Email> AssembleEmail(string to, string cc, string bcc)
        {
            char[] delimiter = new char[] 
            {
                Constants.SpecialCharacters.CharComma,
                Constants.SpecialCharacters.CharSemiColon
            };

            var toList  = to.Split(delimiter).ToList();
            var ccList  = (!string.IsNullOrEmpty(cc))  ? cc.Split(delimiter).ToList() : null;
            var bccList = (!string.IsNullOrEmpty(bcc)) ? bcc.Split(delimiter).ToList(): null;

            var emailList = new List<Email>();

            if(toList.Count > 0)
            {
                foreach (string item in toList)
                {
                    emailList.Add(new Email { TypeID = (int)Constants.EmailType.To, EmailAddress = item.Trim() });
                }

            }
            
            if(ccList != null)
            {
                foreach (string item in ccList)
                {
                    emailList.Add(new Email { TypeID = (int)Constants.EmailType.Cc, EmailAddress = item.Trim() });
                }
            }

            if(bccList != null)
            {
                foreach (string item in bccList)
                {
                    emailList.Add(new Email { TypeID = (int)Constants.EmailType.Bcc, EmailAddress = item.Trim() });
                }
            }

            return emailList;
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

        public static string GetPriorityColor(string level)
        {
            string color = string.Empty;

            switch (level)
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

        public static string GetFormattedDate(DateTime date)
        {
            return (date == null) ? string.Empty : date.ToString(Constants.Common.DateFormat);
        }

        public static string GetStatusColor(int status)
        {
            string color = string.Empty;

            switch (status)
            {
                case 1:
                    color = "#5ac8fa";
                    break;
                case 2:
                    color = "#4CD964";
                    break;
                case 3:
                    color = "#FF9500";
                    break;
                case 4:
                    color = "#5856D6";
                    break;
            }

            return color;
        }

        public static string GetResponseStatusText(bool status)
        {
            return (status ? "Submitted" : "Pending");
        }

        public static string GetResponseStatusColor(bool status)
        {
            return (status ? "#4CD964" : "#5AC8FA");
        }
    }
}
