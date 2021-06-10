

using MobileJO.Data;
using MobileJO.Data.Models;
using MobileJO.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MobileJO.API.Authentication
{
    public class ClaimsProvider
    {
        private readonly IUserService _userService;

        /// <summary>
        ///     Constructor for IUserService
        /// </summary>
        /// <param name="userService"></param>
        public ClaimsProvider(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        ///     Used to retreive claimsIdentity for access token generation
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GetClaimsIdentityToken(string username, string password, BaseCodeEntities db)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();

            try {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || (db == null))
                {
                    claimsIdentity = null;
                }
                else {
                    var user = await _userService.FindUserAsync(username, password);
                    if (user == null || (user.IsActive == false))
                    {
                        claimsIdentity = null;
                    }
                    else
                    {
                        claimsIdentity = CreateClaimsIdentity(user, db);
                    }
                }
            }
            catch (Exception) {
                claimsIdentity = null;
            }
            
            return await Task.FromResult(claimsIdentity);
        }

        /// <summary>
        ///      Used to retreive claimsIdentity for refresh token generation
        /// </summary>
        /// <param name="username"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GetClaimsIdentityRefresh(string username, BaseCodeEntities db)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();

            try {
                if (string.IsNullOrEmpty(username) || (db == null))
                {
                    claimsIdentity = null;
                }
                else {
                    var user = db.User.SingleOrDefault(i => i.UserName == username);
                    if ((user == null) || (user.IsActive == false))
                    {
                        claimsIdentity = null;
                    }
                    else
                    {
                        claimsIdentity = CreateClaimsIdentity(user, db);
                    }
                }
            }
            catch (Exception) {
                claimsIdentity = null;
            }
            
            return await Task.FromResult(claimsIdentity);
        }

        /// <summary>
        ///     Adds claims to claimsIdentity
        /// </summary>
        /// <param name="user"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public ClaimsIdentity CreateClaimsIdentity(User user, BaseCodeEntities db)
        {
            var claims = new List<Claim>();

            try
            {
                if (user == null)
                {
                    claims = null;
                }
                else
                {
                    //var now = DateTime.UtcNow;
                    //if(user.RoleID == 1)
                    //{
                    //    claims.Add(new Claim(ClaimTypes.Role, Constants.Roles.Administrator));
                    //}
                    //else if(user.RoleID == 2)
                    //{
                    //    claims.Add(new Claim(ClaimTypes.Role, Constants.Roles.User));
                    //}
                    claims.Add(new Claim(ClaimTypes.Role, Constants.Roles.Administrator));
                    claims.Add(new Claim(Constants.ClaimTypes.UserName, user.UserName));
                    claims.Add(new Claim(Constants.ClaimTypes.FullName, string.Format(Constants.Common.NameFormat, user.FirstName, user.LastName)));
                    claims.Add(new Claim(Constants.ClaimTypes.ID, user.ID.ToString()));
                    claims.Add(new Claim(Constants.ClaimTypes.UserId, user.UserID));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    claims.Add(new Claim(Constants.ClaimTypes.UserTypeID, user.UserTypeID.ToString()));
                    claims.Add(new Claim(Constants.ClaimTypes.CompanyID, user.CompanyID.ToString()));
                    claims.Add(new Claim(Constants.ClaimTypes.RoleID, user.RoleID.ToString()));
                    claims.Add(new Claim(Constants.ClaimTypes.BranchID, user.BranchID.ToString()));
                }
            }
            catch (Exception) {
                claims = null;
            }
            
            return new ClaimsIdentity(claims);
        }
    }
}
