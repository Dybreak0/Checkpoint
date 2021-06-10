using Microsoft.AspNetCore.Identity;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.ForgotPassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileJO.Data.Repositories
{
    public class ForgotPasswordRepository : BaseRepository, IForgotPasswordRepository
    {

        private UserManager<IdentityUser> _userManager;
        /// <summary>
        ///     Constructor for IUnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ForgotPasswordRepository(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager) : base(unitOfWork)
        {
            _userManager = userManager;
        }

        public int checkRequestCount(ForgotPassword forgotPassword)
        {
            var currentDateTime = DateTime.Now;
            return GetDbSet<ForgotPassword>()
                .Where(x => x.UserID == forgotPassword.UserID && x.ResetToken == forgotPassword.ResetToken && x.ExpiryDate >= currentDateTime)
                .Count();
        }

        public ForgotPassword CheckValidity(int id, string token)
        {
            return GetDbSet<ForgotPassword>()
                .Where(x => x.ID == id && x.ResetToken == token)
                .FirstOrDefault();
        }

        public int CreateForgotPassRequest(ForgotPassword forgotPassword)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    int insert_id = 0;

                    GetDbSet<ForgotPassword>().Add(forgotPassword);
                    UnitOfWork.SaveChanges();

                    insert_id = forgotPassword.ID;

                    
                    dbContextTransaction.Commit();
                    return insert_id;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }

        public void deleteForgotPassword(int id)
        {
            try
            {
                var forgotPassword = Context.ForgotPassword.Find(id);
                GetDbSet<ForgotPassword>()
                    .Remove(forgotPassword);
                UnitOfWork.SaveChanges();

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void deleteForgotPasswordBackground()
        {
            var currentDateTime = DateTime.Now;
            List<ForgotPassword> forgotPassword = new List<ForgotPassword>();
            forgotPassword = GetDbSet<ForgotPassword>()
                .Where(x => x.ExpiryDate <= currentDateTime)
                .ToList();
            if(0 < forgotPassword.Count)
            {
                GetDbSet<ForgotPassword>()
                .RemoveRange(forgotPassword);
            }
            UnitOfWork.SaveChanges();
        }

        public async Task<IdentityResult> updatePassword(ForgotPasswordDetailsViewModel forgotPassword, User user)
        {
            try
            {
                var userIdentity = await _userManager.FindByNameAsync(user.UserName);
                
                var newPassword = _userManager.PasswordHasher.HashPassword(userIdentity, forgotPassword.newPassword);
                userIdentity.PasswordHash = newPassword;
                var result = await _userManager.UpdateAsync(userIdentity);

                return result;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
    }
}
