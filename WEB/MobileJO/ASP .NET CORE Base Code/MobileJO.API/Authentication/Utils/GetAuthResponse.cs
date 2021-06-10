using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using MobileJO.API.Authentication;
using MobileJO.API.ViewModels.Common;
using MobileJO.Data;
using MobileJO.Data.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MobileJO.API.Utilities
{
    public class GetAuthResponse
    {
        /// <summary>
        ///     Get options from appsettings.json file
        /// </summary>
        /// <returns></returns>
        public static TokenProviderOptions GetOptions()
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.Config.GetSection(Constants.Token.SecretKey).Value));

            return new TokenProviderOptions
            {
                Path = Configuration.Config.GetSection(Constants.Token.TokenPath).Value,
                Audience = Configuration.Config.GetSection(Constants.Token.Audience).Value,
                Issuer = Configuration.Config.GetSection(Constants.Token.Issuer).Value,
                Expiration = TimeSpan.FromMinutes(Convert.ToInt32(Configuration.Config.GetSection(Constants.Token.ExpirationMinutes).Value)),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };

        }

        /// <summary>
        ///     Generates response to authentication
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="db"></param>
        /// <param name="userName"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public static LoginResponseDataViewModel Execute(ClaimsIdentity identity, BaseCodeEntities db, IdentityUser user, RefreshToken refreshToken = null)
        {
            LoginResponseDataViewModel response = new LoginResponseDataViewModel();

            try
            {
            var options = GetOptions();

                var now = DateTime.UtcNow;

                if (refreshToken == null)
                {
                    refreshToken = new RefreshToken()
                    {
                        Username = user.UserName,
                        Token = Guid.NewGuid().ToString(Constants.Common.GuidFormat),
                    };
                    db.InsertNew(refreshToken);
                }

                refreshToken.IssuedUtc = now;
                refreshToken.ExpiresUtc = now.Add(options.Expiration);
                db.SaveChanges();

                var jwt = new JwtSecurityToken(
                    issuer: options.Issuer,
                    audience: options.Audience,
                    claims: identity.Claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.Add(options.Expiration),
                    signingCredentials: options.SigningCredentials
                );

                IdentityModelEventSource.ShowPII = true;

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                response = new LoginResponseDataViewModel
                {
                    AccessToken = encodedJwt,
                    RefreshToken = refreshToken.Token
                };
            }
            catch (Exception) {
                response = new LoginResponseDataViewModel();
            }
           
            return response;
        }
    }
}
