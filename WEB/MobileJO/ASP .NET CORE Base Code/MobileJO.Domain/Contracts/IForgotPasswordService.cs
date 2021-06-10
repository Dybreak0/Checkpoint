using Microsoft.AspNetCore.Identity;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.ForgotPassword;
using System.Threading.Tasks;

namespace MobileJO.Domain.Contracts
{
    public interface IForgotPasswordService
    {
        int sendEmail(string email);
        Task<IdentityResult> resetPassword(ForgotPasswordDetailsViewModel forgotPassword);
        ForgotPassword CheckValidity(int id, string token);
    }
}
