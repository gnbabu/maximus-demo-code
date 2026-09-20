using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using ReportingService.Services.Interfaces;
using ReportingService.Services.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using static ReportingService.Services.Security.SecretsMgrExtensions;

namespace ReportingService.Service.Reporting
{
    public class ReportingUserAdminService : BaseReportingService
    {
        protected readonly IUserService _userService;

        public ReportingUserAdminService(
            IWorkContext workContext,
            BoldReportsSettings settings,
            HttpClient httpClient,
            IUserService userService,
            ISecretsMgrRepository secretsMgr,
            IHttpContextAccessor httpContextAccessor
        )
            : base(workContext, settings, httpClient, secretsMgr, httpContextAccessor)
        {
            _userService = userService;
        }


        public virtual async Task<RptUser> GetUser(string emailAddr)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(emailAddr)) return null;

                var emailAddrEnc = HttpUtility.UrlEncode(emailAddr);
                var url = $"/reporting/api/site/{_settings.SiteName}/v1.0/users/{emailAddrEnc}";

                var token = GetIncomingToken(); // ✅ FIX

                using (var requestMessage = NewGetReqMsg(_settings.BaseUrl + url, token))
                {
                    var result = await _client.SendAsync(requestMessage);
                    var resultContent = await result.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(resultContent)) return null;

                    if (result.IsSuccessStatusCode)
                        return JsonSerializer.Deserialize<RptUser>(resultContent);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public virtual async Task<RptGroup[]> GetAllGroups()
        {
            var url = $"/reporting/api/site/{_settings.SiteName}/v1.0/groups";
            var token = GetIncomingToken(); // ✅ FIX

            string resultContent = string.Empty;

            using (var requestMessage = NewGetReqMsg(_settings.BaseUrl + url, token))
            {
                var result = await _client.SendAsync(requestMessage);

                if (!result.IsSuccessStatusCode)
                    return new RptGroup[] { };

                resultContent = await result.Content.ReadAsStringAsync();
            }

            if (string.IsNullOrWhiteSpace(resultContent))
                return new RptGroup[] { };

            var groups = JsonSerializer.Deserialize<Groups>(resultContent);

            return groups?.GroupList ?? new RptGroup[] { };
        }

        public virtual async Task<RptGroup[]> GetUserGroups(RptUser user)
        {
            var url = $"/reporting/api/site/{_settings.SiteName}/v1.0/users/{user.Email}/groups";
            var token = GetIncomingToken();

            if (token == null) return null;

            using (var requestMessage = NewGetReqMsg(_settings.BaseUrl + url, token))
            {
                var result = await _client.SendAsync(requestMessage);
                if (!result.IsSuccessStatusCode) return new RptGroup[] { };
                var resultContent = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    if (string.IsNullOrWhiteSpace(resultContent)) return new RptGroup[] { };
                    var groups = JsonSerializer.Deserialize<Groups>(resultContent);
                    return groups?.GroupList ?? new RptGroup[] { };
                }
                return new RptGroup[] { };
            }

        }

        public virtual async Task<bool> AssignGroup(RptUser user, RptGroup group)
        {
            if (user == null) return false;
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Username)) return false;

            var url = "/reporting/api/site/" + _settings.SiteName + "/v1.0/groups/" + group.Id + "/users";
            var token = GetIncomingToken();

            if (token == null) return false;

            using (var requestMessage = NewPostReqMsg(_settings.BaseUrl + url, token))
            {
                var ids = new { Id = new int[] { user.UserId } };
                var json = JsonSerializer.Serialize(ids);

                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await _client.SendAsync(requestMessage);
                result.EnsureSuccessStatusCode();
            }

            return true;
        }

        public virtual async Task<bool> UnassignGroup(RptUser user, RptGroup group)
        {
            if (user == null) return false;
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Username)) return false;

            var url = "/reporting/api/site/" + _settings.SiteName + "/v1.0/groups/" + group.Id + "/users";
            var token = GetIncomingToken();

            if (token == null) return false;

            using (var requestMessage = NewDeleteReqMsg(_settings.BaseUrl + url, token))
            {
                var ids = new { Id = new int[] { user.UserId } };
                var json = JsonSerializer.Serialize(ids);

                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await _client.SendAsync(requestMessage);
                result.EnsureSuccessStatusCode();
            }

            return true;
        }


        /// <summary>
        /// Syncronize MPC User with Bold Reports User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task<bool> SyncUserAsync(User user)
        {
            var specialChars = new Regex(@"[^a-zA-Z0-9-_ ]+", RegexOptions.None, TimeSpan.FromSeconds(1));

            if (user == null || user.Id < 1 || string.IsNullOrWhiteSpace(user.Email))
                return true;

            // ✅ No service token anymore
            var token = GetIncomingToken();

            // get BR Groups
            var brGrps = await GetAllGroups();

            // get user roles
            var userRoles = await _userService.GetUserRolesAsync(user.Id);
            foreach (var ur in userRoles)
                ur.Name = ur.Name.Trim();

            var urIndex = userRoles.ToDictionary(x => x.Name.ToLower(), x => x);

            // User.Roles not in BR.Groups -> return
            var rptGrps = brGrps.Where(x => urIndex.ContainsKey(x.Name.ToLower()));
            var rptGrpsIndex = rptGrps.ToDictionary(x => x.Name.ToLower(), x => x);

            if (!rptGrps.Any())
                return true;

            // Get BR User
            var rptUser = await GetUser(user.Email);

            // Add BR User
            if (rptUser == null)
            {
                var newUser = new RptNewUser
                {
                    Email = user.Email,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    Lastname = user.LastName,
                    ExternalId = user.UserGuid.ToString()
                };

                rptUser = await CreateUser(newUser);
            }

            // Get BR User Groups
            var usrBrGrps = await GetUserGroups(rptUser);
            var usrBrGrpsIndex = usrBrGrps.ToDictionary(x => x.Name.ToLower(), x => x);

            // add groups
            var addGrps = rptGrps.Where(x => x.Id > 1 && !usrBrGrpsIndex.ContainsKey(x.Name.ToLower()));

            // remove groups
            var delGroup = usrBrGrps.Where(x => x.Id > 1 && !rptGrpsIndex.ContainsKey(x.Name.ToLower()));

            foreach (var g in addGrps)
                await AssignGroup(rptUser, g);

            foreach (var g in delGroup)
                await UnassignGroup(rptUser, g);

            return false;
        }

        protected virtual async Task<RptUser> CreateUser(RptNewUser user)
        {
            var jwt = GenerateJSONWebToken(user);
            if (!await CreateJwtUser(jwt, _settings.SiteName)) return null;
            var u = await GetUser(user.Email);
            return u;
        }

        protected string GenerateJSONWebToken(RptNewUser user)
        {
            var secrets = _secretsMgr.GetSecret<BoldReportsSettings.Secrets>(_settings.SecretsMgrKey);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrets.JwtSigningKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var claims = new[] {
                new Claim("sub", user.ExternalId),
                new Claim("email", user.Email),
                new Claim("first_name", user.FirstName),
                new Claim("last_name", user.Lastname),
            };

            var token = new JwtSecurityToken(claims: claims,
                        expires: DateTime.Now.AddMinutes(120),
                        signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// User is added to Reporting System
        /// </summary>
        /// <param name="token"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private async Task<bool> CreateJwtUser(string newUsertoken, string siteId)
        {
            newUsertoken = System.Web.HttpUtility.UrlEncode(newUsertoken);
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_settings.BaseUrl}/sso/jwt/callback?jwt={newUsertoken}&site_identifier={siteId}");
            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public string BoldUIUrl(RptUser user, User mpcUser)
        {
            var jwtUser = new RptNewUser()
            {
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                Lastname = user.Lastname,
                ExternalId = mpcUser.UserGuid.ToString()
            };
            var jwt = GenerateJSONWebToken(jwtUser);
            return $"{_settings.BaseUrl}/sso/jwt/callback?jwt={jwt}&site_identifier={_settings.SiteName}";
        }
    }
}
