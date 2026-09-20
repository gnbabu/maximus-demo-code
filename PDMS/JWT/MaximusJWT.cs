using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MAXIMUS.Core.Libraries;
using System.Data;
using System.Data.SqlClient;

namespace MAXIMUS.Core.Libraries
{
    public static class MaximusJWT
    {
        public const int TOKEN_VALID = 200;
        public const int TOKEN_EXPIRED = 400;
        public const int TOKEN_INVALID = 401;
        public const int TOKEN_USER_LOGGED_OUT = 402;
        public const int TOKEN_MISSING = 404;
        public const int TOKEN_ERROR = 500;
        public static readonly string CLAIM_TYPE_USER_ID = "http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata";
        public static readonly string CLAIM_TYPE_USERNAME = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public static string GetNewToken(string UserName, string UserId, string ClientSecret)
        {
            // Check for Existince of prior token
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("uid", UserId));
            string pnmToken = DataAccess.ExecuteScalar("usp_CheckForValidTokenByUID", parameters);
            // If we have a token string (SP responds with empty string if no token), send that back
            if (pnmToken != "")
            {
                return pnmToken;
            }
            // Only create new token if no token exists
            else
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                // security keys
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ClientSecret));
                SigningCredentials sCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

                // set claims
                ClaimsIdentity claims = new ClaimsIdentity();
                Claim userNameClaim = new Claim(ClaimTypes.NameIdentifier, UserName);
                Claim userIDClaim = new Claim(ClaimTypes.UserData, UserId);
                claims.AddClaim(userNameClaim);
                claims.AddClaim(userIDClaim);

                // TODO: add additional claims derived from user?

                // create descriptor
                SecurityTokenDescriptor tDsc = new SecurityTokenDescriptor();
                tDsc.Issuer = "Maximus";
                tDsc.Audience = "PDMS";
                tDsc.Expires = DateTime.UtcNow.AddHours(24); // 24 hours (so true expiry can auto-renew)
                tDsc.Subject = claims;
                tDsc.SigningCredentials = sCredentials;

                //create jwt
                string tkn = handler.CreateEncodedJwt(tDsc);

                return tkn;
            }
        }

        public static int ValidateToken(string Token2Validate, string ClientSecret)
        {

            if (Token2Validate.Trim() == "") { return TOKEN_MISSING; }

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ClientSecret));

            try {
                SecurityToken validatedToken;
                TokenValidationParameters param = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(1),
                    ValidIssuer = "Maximus",
                    ValidAudience = "PDMS",
                    IssuerSigningKey = securityKey,
                };
                ClaimsPrincipal claims = handler.ValidateToken(Token2Validate, param, out validatedToken);
            }
            catch (SecurityTokenExpiredException) {  // token expired
                return TOKEN_EXPIRED;
            }
            catch (SecurityTokenDecryptionFailedException) {  // problem or token not valid
                return TOKEN_INVALID;
            }
            catch (Exception) {  // other problem
                return TOKEN_INVALID;
            }

            // if we get here, check token in database (this will also auto extend the token by 1 hour if currently valid)
            string Userid = GetSingleClaimValue(Token2Validate, ClientSecret, ClaimTypes.UserData);

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("UserID", Userid));
            parameters.Add(new SqlParameter("PNMToken", Token2Validate));
            string passfail = DataAccess.ExecuteScalar("usp_CheckUserPNMToken", parameters);

            if (passfail == "PASSED")
            {
                return TOKEN_VALID;
            } else
            {
                return TOKEN_EXPIRED;
            }            
        }


        public static List<KeyValuePair<string, string>> GetAllClaims(string Token2Check, string ClientSecret)
        {

            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();

            if (Token2Check.Trim() == "") { return lst; }

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ClientSecret));

            try
            {
                SecurityToken validatedToken;
                TokenValidationParameters param = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(1),
                    ValidIssuer = "Maximus",
                    ValidAudience = "PDMS",
                    IssuerSigningKey = securityKey,
                };
                ClaimsPrincipal principal = handler.ValidateToken(Token2Check, param, out validatedToken);

                foreach (Claim claim in principal.Claims)
                {
                    lst.Add(new KeyValuePair<string, string>(claim.Type, claim.Value));
                }
            }
            catch (Exception)
            {  // other problem
                return lst;
            }

            return lst;

        }

        public static string GetSingleClaimValue(string Token2Check, string ClientSecret, string ClaimType)
        {
            String rtn = "";

            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();

            if (Token2Check.Trim() == "") { return "error"; }

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ClientSecret));

            try
            {
                SecurityToken validatedToken;
                TokenValidationParameters param = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(1),
                    ValidIssuer = "Maximus",
                    ValidAudience = "PDMS",
                    IssuerSigningKey = securityKey,
                };
                ClaimsPrincipal principal = handler.ValidateToken(Token2Check, param, out validatedToken);

                foreach (Claim claim in principal.Claims)
                {
                    if (claim.Type == ClaimType) { rtn = claim.Value;  }
                }
            }
            catch (Exception)
            {  // other problem
                return "error";
            }

            return rtn;

        }

    }
}
