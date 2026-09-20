using Amazon.Runtime.Credentials.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using ReportingService.Services.Extensions;
using ReportingService.Services.Security;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using static ReportingService.Services.Security.SecretsMgrExtensions;

namespace ReportingService.Service.Reporting
{
    public class BaseReportingService
    {
        protected readonly BoldReportsSettings _settings;
        protected readonly IWorkContext _workContext;
        protected readonly HttpClient _client;
        protected readonly ISecretsMgrRepository _secretsMgr;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseReportingService(
            IWorkContext workContext,
            BoldReportsSettings settings,
            HttpClient httpClient,
            ISecretsMgrRepository secretsMgr,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _workContext = workContext;
            _settings = settings;
            _client = httpClient;
            _secretsMgr = secretsMgr;
            _httpContextAccessor = httpContextAccessor;
        }

        protected string GetIncomingToken()
        {
            var context = _httpContextAccessor.HttpContext;

            // ✅ Try standard Authorization first
            var authHeader = context?.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(authHeader))
            {
                // ✅ Fallback to ServiceAuthorizationToken (your AJAX)
                authHeader = context?.Request.Headers["ServiceAuthorizationToken"].ToString();
            }

            if (string.IsNullOrWhiteSpace(authHeader))
                throw new UnauthorizedAccessException("Missing Authorization header");

            return authHeader.Replace("Bearer ", "");
        }


        public virtual async Task<RptAuthToken> GetUserAuthToken(string userName)
        {
            
            var secrets = await _secretsMgr.GetSecretAsync<BoldReportsSettings.Secrets>(_settings.SecretsMgrKey);
            string tokenUrlPath = $"/reporting/api/site/{_settings.SiteName}/token";

            var nonce = Guid.NewGuid().ToString();
            var epochTime = DateTime.UtcNow.ToUnixTime();
            string embedMessage = "embed_nonce=" + nonce + "&user_email=" + userName + "&timestamp=" + epochTime;
            string signature = CalcReqSignature(embedMessage.ToLower(), secrets.EmbedSecret);


            var content = new FormUrlEncodedContent(new[]
            {
               new KeyValuePair<string, string>("grant_type", "embed_secret"),
               new KeyValuePair<string, string>("username", userName.ToLower()),
               new KeyValuePair<string, string>("embed_nonce", nonce),
               new KeyValuePair<string, string>("timestamp", epochTime),
               new KeyValuePair<string, string>("expires_in", "1800"),
               new KeyValuePair<string, string>("embed_signature", signature)
            });

            string resultContent = string.Empty;

            var request = new HttpRequestMessage(HttpMethod.Post, _settings.BaseUrl + tokenUrlPath);
            request.Content = content;
            var result = await _client.SendAsync(request);
            if (!result.IsSuccessStatusCode) return null;
            resultContent = await result.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(resultContent)) return null;
            var _token = JsonSerializer.Deserialize<RptAuthToken>(resultContent);
            if (_token == null || _token.error != null) return null;
            _token.issued_at = DateTime.UtcNow;
            _token.expires_at = _token.issued_at.AddSeconds(_token.expires_in);
            var rUser = await GetUserV1(_token.access_token, _token.email);
            _token.UserId = rUser?.UserId ?? 0;
            return _token;
        }

        protected async Task<RptUser> GetUserV1(string access_token, string emailAddr)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(emailAddr)) return null;
                var emailAddrEnc = HttpUtility.UrlEncode(emailAddr);
                var url = $"/reporting/api/site/{_settings.SiteName}/v1.0/users/{emailAddrEnc}";
                var resultContent = string.Empty;

                if (access_token == null) return null;

                using (var requestMessage = NewGetReqMsg(_settings.BaseUrl + url, access_token))
                {
                    var result = await _client.SendAsync(requestMessage);
                    resultContent = await result.Content.ReadAsStringAsync();
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

        public async Task<RptAuthToken> GetCurrentUserToken()
        {
            var user = await _workContext.GetCurrentUserAsync();
            return await GetUserAuthToken(user.Email);
        }

        private static string CalcReqSignature(string embedMessage, string secretcode)
        {
            var encoding = new UTF8Encoding();
            var keyBytes = encoding.GetBytes(secretcode);
            var messageBytes = encoding.GetBytes(embedMessage);
            using (var hmacsha1 = new HMACSHA256(keyBytes))
            {
                var hashMessage = hmacsha1.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashMessage);
            }
        }

        protected static HttpRequestMessage NewGetReqMsg(string url, string authToken)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            return requestMessage;
        }
        protected static HttpRequestMessage NewPostReqMsg(string url, string authToken)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            return requestMessage;
        }

        protected static HttpRequestMessage NewDeleteReqMsg(string url, string authToken)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            return requestMessage;
        }
    }
}
