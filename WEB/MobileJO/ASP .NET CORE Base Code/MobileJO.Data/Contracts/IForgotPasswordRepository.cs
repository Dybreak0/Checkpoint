using Microsoft.AspNetCore.Identity;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.ForgotPassword;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MobileJO.Data.Contracts
{
    public interface IForgotPasswordRepository
    {
        int CreateForgotPassRequest(ForgotPassword forgotPassword);
        int checkRequestCount(ForgotPassword forgotPassword);
        Task<IdentityResult> updatePassword(ForgotPasswordDetailsViewModel forgotPassword, User user);
        void deleteForgotPassword(int id);
        void deleteForgotPasswordBackground();
        ForgotPassword CheckValidity(int id, string token);
    }
}
