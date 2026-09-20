using Corp.Core.Libraries.DataModels;
using Corp.Core.Libraries.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Corp.Core.Libraries.ServiceAgent
{
    public class TokenServiceAgent
    {
        private static TokenResponse _cachedToken;
        private static DateTime _tokenExpiry = DateTime.MinValue;
        private static readonly object _lock = new object();

        /// <summary>
        /// Generates a Bold Reports authentication token using credentials.
        /// Caches the token and refreshes it when expired.
        /// </summary>
        public string GenerateToken()
        {
            lock (_lock)
            {
                if (_cachedToken != null && DateTime.UtcNow < _tokenExpiry)
                {
                    return "bearer " + _cachedToken.access_token;
                }

                TokenResponse tokenResponse = RequestNewToken();

                if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.access_token))
                {
                    _cachedToken = tokenResponse;

                    int expiresInSeconds;
                    if (int.TryParse(tokenResponse.expires_in, out expiresInSeconds))
                    {
                        _tokenExpiry = DateTime.UtcNow.AddSeconds(expiresInSeconds - 60);
                    }
                    else
                    {
                        _tokenExpiry = DateTime.UtcNow.AddMinutes(55);
                    }

                    return "bearer " + tokenResponse.access_token;
                }

                string errorMessage = tokenResponse != null
                    ? tokenResponse.error_description ?? tokenResponse.error ?? "Unknown error"
                    : "Token response was null";

                throw new ApplicationException("Failed to generate Bold Reports token: " + errorMessage);
            }
        }

        /// <summary>
        /// Returns just the raw access token string without the token type prefix.
        /// </summary>
        public string GetAccessToken()
        {
            lock (_lock)
            {
                if (_cachedToken != null && DateTime.UtcNow < _tokenExpiry)
                {
                    return _cachedToken.access_token;
                }

                GenerateToken();
                return _cachedToken.access_token;
            }
        }

        /// <summary>
        /// Forces a token refresh on next request.
        /// </summary>
        public void InvalidateToken()
        {
            lock (_lock)
            {
                _cachedToken = null;
                _tokenExpiry = DateTime.MinValue;
            }
        }

        private string CreateSignature(string nonce, long timestamp, string secret)
        {
            string payload = $"{nonce}{timestamp}";

            using (var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                return Convert.ToBase64String(hash);
            }
        }

        private TokenResponse RequestNewToken()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string tokenEndpoint = BoldReportsAppSettings.TokenEndpoint;
            string postData;

            if (!string.IsNullOrEmpty(BoldReportsAppSettings.EmbedSecret))
            {
                string nonce = Guid.NewGuid().ToString();
                long timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                string signature = CreateSignature(nonce, timeStamp, BoldReportsAppSettings.EmbedSecret);

                postData = $"grant_type=embed_secret" +
                           $"&embed_nonce={nonce}" +
                           $"&embed_signature={HttpUtility.UrlEncode(signature)}" +
                           $"&embed_timestamp={timeStamp}";
            }
            else
            {
                postData = $"grant_type=password" +
                           $"&username={HttpUtility.UrlEncode(BoldReportsAppSettings.UserName)}" +
                           $"&password={HttpUtility.UrlEncode(BoldReportsAppSettings.Password)}";
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tokenEndpoint);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/json";

            byte[] data = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string responseBody = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<TokenResponse>(responseBody);
            }
        }
    }
}
