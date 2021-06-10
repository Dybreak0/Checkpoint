using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MobileJO.Data;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.ForgotPassword;
using MobileJO.Domain.Contracts;
using System;
using System.Threading.Tasks;
using Constants = MobileJO.Data.Constants;

namespace MobileJO.Domain.Services
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly IUserRepository _userRepository;
        private readonly IForgotPasswordRepository _forgotRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        /// <summary>
        ///     Constructor for IUserRepository and IMapper
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public ForgotPasswordService(IUserRepository userRepository, IForgotPasswordRepository forgotRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _forgotRepository = forgotRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public ForgotPassword CheckValidity(int id, string token)
        {
            var result = _forgotRepository.CheckValidity(id, token);
            if(null == result)
            {
                throw new NullReferenceException(Constants.Common.RecordDoesNotExist);
            }
            return result;

        }

        public async Task<IdentityResult> resetPassword(ForgotPasswordDetailsViewModel forgotPasswordDetails)
        {
            ForgotPassword forgotPassword = new ForgotPassword
            {
                UserID = forgotPasswordDetails.userId,
                ResetToken = forgotPasswordDetails.token
            };

            int checkRequestCount = _forgotRepository.checkRequestCount(forgotPassword);

            if(0 < checkRequestCount)
            {
                var user = _userRepository.Find(forgotPasswordDetails.userId);
                var result = await _forgotRepository.updatePassword(forgotPasswordDetails, user);

                if (result.Succeeded)
                {
                    _forgotRepository.deleteForgotPassword(forgotPasswordDetails.ID);
                }
                return result;
            }

            throw new NullReferenceException(Constants.Common.RecordDoesNotExist);
            
            
        }

        public int sendEmail(string email)
        {
            int id = 0;

            var user = _userRepository.FindUserByEmail(email);

            if(null == user)
            {
                throw new NullReferenceException(Constants.Common.RecordDoesNotExist);
            }
                
            var currentDateTime = DateTime.Now;
            var expiryDateTime = currentDateTime.AddHours(3);
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/","");

            ForgotPassword forgotPassword = new ForgotPassword
            {
                UserID = user.ID,
                CreatedDate = currentDateTime,
                ResetToken = token,
                ExpiryDate = expiryDateTime
            };

            id = _forgotRepository.CreateForgotPassRequest(forgotPassword);

            if(0 < id)
            {
                string link = string.Format(Constants.Link.ForgotPassword, id.ToString(), token);
                
                Helper.SendPasswordLink(_configuration[Constants.ForgotPasswordConfiguration.EmailRecipient],
                _configuration[Constants.ForgotPasswordConfiguration.EmailCredential],
                _configuration[Constants.ForgotPasswordConfiguration.EmailSubjectBrandName], user, link);
            }
            
            return id; 
        }
    }
}
