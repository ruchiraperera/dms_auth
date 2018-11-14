using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using auth_service.Models;
using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;

namespace WebApi.Jwt
{
    public static class JwtManager
    {
        /// <summary>
        /// Use the below code to generate symmetric Secret Key
        ///     var hmac = new HMACSHA256();
        ///     var key = Convert.ToBase64String(hmac.Key);
        /// </summary>
        private static string Secret = ConfigurationManager.AppSettings["jwtKey"].ToString();

        public static string GenerateToken(string username,int userid,string role, tbl_permission _permission , int expireMinutes = 20)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

           
            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = ConfigurationManager.AppSettings["aud"],
                Issuer = "self",
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.NameIdentifier, userid.ToString()),
                            new Claim(ClaimTypes.Role,role),
                            new Claim("permission", JsonExtensions.SerializeToJson(_permission))
                }),
                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),                
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(Secret);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                
                return principal;
            }

            catch (Exception)
            {
                return null;
            }
        }
    }
}