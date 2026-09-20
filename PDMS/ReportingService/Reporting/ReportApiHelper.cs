using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReportingService.Service.Reporting
{
    public class ReportApiHelper
    {
        public static async Task<T> Get<T>(HttpClient httpClient, string url, string accessToken)
        {
            string resultContent;
            using (var requestMessage = NewGetReqMsg(url, accessToken))
            {
                var result = await httpClient.SendAsync(requestMessage);
                if (!result.IsSuccessStatusCode) return default(T);

                resultContent = await result.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(resultContent)) return default(T);
            }

            return JsonSerializer.Deserialize<T>(resultContent);
        }

        public static async Task<T> Post<T>(HttpClient httpClient, string url, string accessToken, object postBody)
        {
            using (var requestMessage = NewPostReqMsg(url, accessToken))
            {
                requestMessage.Content = new StringContent(
                    JsonSerializer.Serialize(postBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var result = await httpClient.SendAsync(requestMessage);

                var resultContent = await result.Content.ReadAsStringAsync();

                // ✅ DEBUG (CRITICAL)
                Console.WriteLine("URL: " + url);
                Console.WriteLine("STATUS: " + result.StatusCode);
                Console.WriteLine("RESPONSE: " + resultContent);

                if (!result.IsSuccessStatusCode)
                    return default(T);

                if (string.IsNullOrWhiteSpace(resultContent))
                    return default(T);

                return JsonSerializer.Deserialize<T>(resultContent);
            }
        }

        public static async Task<T> Delete<T>(HttpClient httpClient, string url, string accessToken, object postBody)
        {
            string resultContent;
            using (var requestMessage = NewDeleteReqMsg(url, accessToken))
            {
                requestMessage.Content = new StringContent(
                    JsonSerializer.Serialize(postBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var result = await httpClient.SendAsync(requestMessage);
                if (!result.IsSuccessStatusCode) return default(T);

                resultContent = await result.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(resultContent)) return default(T);
            }

            return JsonSerializer.Deserialize<T>(resultContent);
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
