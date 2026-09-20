using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ReportingService.Services.Interfaces;
using ReportingService.Services.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ReportingService.Service.Reporting
{
    public class ReportingApiMiddleware
    {
        private readonly RequestDelegate _nextMiddleware;
        private readonly ILogger<ReportingApiMiddleware> _logger;

        public ReportingApiMiddleware(
            RequestDelegate nextMiddleware,
            ILogger<ReportingApiMiddleware> logger)
        {
            _nextMiddleware = nextMiddleware;
            _logger = logger;
        }


        public async Task Invoke(HttpContext context, HttpClient client, BoldReportsSettings settings, IWorkContext workContext, ICapabilityService capService)
        {
            try
            {
                PathString remainingPath;
                if (context.Request.Path.StartsWithSegments("/reports/api", out remainingPath)
                    && context.User.Identity.IsAuthenticated)
                {
                    await ProxyReportsApiRoute(remainingPath, context, client, settings, workContext, capService);
                }

                if (context.Request.Path.StartsWithSegments("/js/boldreports/common-images", out remainingPath)
                    && context.User.Identity.IsAuthenticated)
                {
                    var path = "/reporting/cdn/css/essentialjs/v2.0/common-images" + remainingPath.Value;
                    await ProxyRequest(settings.BaseUrl + path, context, client, settings);
                }
            }
            catch(SocketException ex)
            {
                _logger.LogError(ex, "Network Error in ReportingApiMiddleware. Cannot connect to Report Server");
            }
            await _nextMiddleware(context);
        }

        protected async Task ProxyReportsApiRoute(PathString remainingPath, HttpContext context, HttpClient client, BoldReportsSettings settings, IWorkContext workContext, ICapabilityService capService)
        {
            
            string ViewerPath = @"/reporting/reportservice/api/Viewer/";
            string DesignerPath = @"/reporting/reportservice/api/Designer/";

            var currUser = await workContext.GetCurrentUserAsync();
            var hasReporting = await capService.UserHasCapabilityAsync(currUser, StandardCapabilityProvider.Reporting.Base);
            var hasReportEdit = await capService.UserHasCapabilityAsync(currUser, StandardCapabilityProvider.Reporting.Edit);
            if (!hasReporting) return;
            Debug.WriteLine("ProxyReportsApiRoute: " + remainingPath.Value);
            remainingPath = remainingPath.Value != null && remainingPath.Value.EndsWith("/")
                            ? remainingPath.Value.Substring(0, remainingPath.Value.Length - 1)
                            : remainingPath.Value;

            switch (remainingPath.Value?.ToLower())
            {
                case "/postformreportaction":
                    await ProxyRequest2(settings.BaseUrl + ViewerPath + "PostFormReportAction", context, client, settings);
                    return;
                case "/postreportaction":
                    await ProxyRequest(settings.BaseUrl + ViewerPath + "PostReportAction", context, client, settings);
                    return;
                case "/uploadreportaction":
                    if (!hasReportEdit) return;
                    await ProxyRequest(settings.BaseUrl + DesignerPath + "UploadReportAction", context, client, settings);
                    return;
                case "/postdesigneraction":
                    if (!hasReportEdit) return;
                    await ProxyRequest(settings.BaseUrl + DesignerPath + "PostDesignerAction", context, client, settings);
                    return;
                case "/postformdesigneraction":
                    if (!hasReportEdit) return;
                    await ProxyRequest(settings.BaseUrl + DesignerPath + "PostFormDesignerAction", context, client, settings);
                    return;
                case "/getresource":
                    await ProxyRequest(settings.BaseUrl + DesignerPath + "GetResource", context, client, settings);
                    return;
                case "/getimage":
                    await ProxyRequest(settings.BaseUrl + DesignerPath + "GetImage", context, client, settings);
                    return;
                default:
                    //Route does not match. Continue pipeline
                    Debug.WriteLine("ProxyReportsApiRoute: No match for " + remainingPath.Value);
                    break;
            }
        }



        protected async Task ProxyRequest2(string targetUrl, HttpContext context, HttpClient _client, BoldReportsSettings settings)
        {
            var req = await HttpMsgFromRequest2(context, targetUrl, settings);
            using (var resp = await _client.SendAsync(req, HttpCompletionOption.ResponseContentRead, context.RequestAborted))
            {
                await ResponseToContext(context, resp);
            }
        }

        protected async Task ProxyRequest(string targetUrl, HttpContext context, HttpClient _client, BoldReportsSettings settings)
        {
            var req = await HttpMsgFromRequest(context, targetUrl, settings);
            using (var resp = await _client.SendAsync(req, HttpCompletionOption.ResponseContentRead, context.RequestAborted))
            {
                await ResponseToContext(context, resp);
            }
        }

        protected async Task<HttpRequestMessage> HttpMsgFromRequest2(HttpContext context, string url, BoldReportsSettings settings)
        {
            // ✅ Query string
            if (!string.IsNullOrWhiteSpace(context.Request.QueryString.Value))
            {
                url += "?" + context.Request.QueryString.Value;
            }

            var rptServerUrl = settings.BaseUrl + "/reporting/api/site/" + settings.SiteName;

            var request = new HttpRequestMessage(new HttpMethod(context.Request.Method), url);

            bool isUrlEncoded = false;

            // ✅ Headers
            foreach (var header in context.Request.Headers)
            {

                if (header.Key == "Content-Type")
                {
                    foreach (var v in header.Value)
                    {
                        if (v != null && v.Contains("urlencoded"))
                        {
                            isUrlEncoded = true;
                            break;
                        }
                    }
                }

                if (header.Key == "Origin" ||
                    header.Key == "Content-Length" ||
                    header.Key == "Host" ||
                    header.Key == "Connection")
                {
                    continue;
                }

                if (header.Key == "WWW-Authenticate" ||
                    header.Key == "Access-Control-Allow-Origin" ||
                    header.Key == "Transfer-Encoding" ||
                    header.Key == "Referrer-Policy")
                {
                    continue;
                }

                if (header.Key.ToLower() == "serverurl")
                {
                    request.Headers.TryAddWithoutValidation("serverurl", new string[] { rptServerUrl });
                }
                else
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            // ✅ Body handling
            if (isUrlEncoded)
            {
                // ✅ Body handling (FINAL FIX)
                context.Request.EnableBuffering();

                var rawBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                context.Request.Body.Position = 0;

                // ✅ Extract token from RAW body (NOT FormReader)
                var match = System.Text.RegularExpressions.Regex.Match(
                    rawBody,
                    @"serviceAuthorizationToken=([^&]+)"
                );

                if (match.Success)
                {
                    var token = System.Web.HttpUtility.UrlDecode(match.Groups[1].Value);

                    if (!string.IsNullOrEmpty(token) &&
                        token.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        token = token.Substring(7);
                    }

                    request.Headers.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                // ✅ Preserve body EXACTLY (only safe replace)
                rawBody = rawBody.Replace(
                    "https://{{MPC_Report_Server}}/reports/server",
                    rptServerUrl
                );

                // ✅ Send exactly as-is
                request.Content = new StringContent(
                    rawBody,
                    System.Text.Encoding.UTF8,
                    "application/x-www-form-urlencoded"
                );
            }
            else
            {
                var rqBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                rqBody = rqBody.Replace("https://{{MPC_Report_Server}}/reports/server", rptServerUrl);

                request.Content = new StringContent(rqBody);
            }

            // ✅ Content-Type
            if (!string.IsNullOrEmpty(context.Request.ContentType))
            {
                var ctParts = context.Request.ContentType.Split(';');

                var mediaType = ctParts[0].Trim();

                var contentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mediaType);

                if (ctParts.Length > 1)
                {
                    var charsetParts = ctParts[1].Split('=');

                    if (charsetParts.Length > 1)
                    {
                        contentType.CharSet = charsetParts[1].Trim();
                    }
                }

                request.Content.Headers.ContentType = contentType;
            }

            // ✅ HARD SET serverurl (FINAL FIX)
            request.Headers.Remove("serverurl");

            request.Headers.TryAddWithoutValidation(
                "serverurl",
                settings.BaseUrl + "/reporting/api/site/" + settings.SiteName
            );

            return request;
        }


        protected async Task<HttpRequestMessage> HttpMsgFromRequest(
            HttpContext context,
            string url,
            BoldReportsSettings settings)
        {
            // ✅ Preserve query string
            url += string.IsNullOrWhiteSpace(context.Request.QueryString.Value)
                ? ""
                : "?" + context.Request.QueryString.Value;

            var rptServerUrl = settings.BaseUrl + "/reporting/api/site/" + settings.SiteName;

            var request = new HttpRequestMessage(
                new HttpMethod(context.Request.Method),
                url);

            bool isUrlEncoded = false;

            // ✅ COPY HEADERS
            foreach (var header in context.Request.Headers)
            {
                var key = header.Key;

                if (key == "Content-Type")
                {
                    isUrlEncoded = header.Value.Any(v =>
                        v != null && v.IndexOf("urlencoded", StringComparison.OrdinalIgnoreCase) >= 0);
                    continue;
                }

                if (key == "Host" || key == "Content-Length" || key == "Connection")
                    continue;

                if (key == "Origin" || key == "Cookie")
                    continue;

                // ✅ FIX AUTH (CRITICAL)
                if (key.ToLower() == "serviceauthorizationtoken")
                {
                    var token = header.Value.ToString();

                    if (!string.IsNullOrEmpty(token) &&
                        token.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        token = token.Substring("bearer ".Length);
                    }

                    // ✅ REQUIRED
                    request.Headers.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    request.Headers.TryAddWithoutValidation(key, header.Value.ToArray());
                    continue;
                }

                request.Headers.TryAddWithoutValidation(key, header.Value.ToArray());
            }

            // ✅ ALWAYS ADD serverurl (CRITICAL FIX FOR YOUR 401)
            request.Headers.Remove("serverurl");
            request.Headers.TryAddWithoutValidation("serverurl", rptServerUrl);

            // ✅ ✅ HANDLE BODY (ONLY ONCE — NO DUPLICATES)

            if (isUrlEncoded)
            {
                context.Request.EnableBuffering();
                context.Request.Body.Position = 0;

                var rqBody = await new FormReader(context.Request.Body).ReadFormAsync();

                var kvpList = rqBody
                    .Where(kvp => kvp.Value.Any(v =>
                        v.Contains("https://{{MPC_Report_Server}}/reports/server")))
                    .ToList();

                foreach (var kvp in kvpList)
                {
                    rqBody.Remove(kvp.Key);
                    rqBody.Add(kvp.Key,
                        kvp.Value.Select(v =>
                            v.Replace("https://{{MPC_Report_Server}}/reports/server", rptServerUrl)).ToArray());
                }

                var dict = rqBody.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

                request.Content = new FormUrlEncodedContent(dict);
            }
            else
            {
                // ✅ CRITICAL FIX — copy body properly

                context.Request.EnableBuffering();

                var memoryStream = new MemoryStream();
                await context.Request.Body.CopyToAsync(memoryStream);

                memoryStream.Position = 0;
                context.Request.Body.Position = 0;

                request.Content = new StreamContent(memoryStream);
            }

            // ✅ PRESERVE CONTENT-TYPE (NO PARSING)
            if (!string.IsNullOrEmpty(context.Request.ContentType))
            {
                request.Content.Headers.TryAddWithoutValidation(
                    "Content-Type",
                    context.Request.ContentType);
            }

            return request;
        }

        protected static async Task ResponseToContext(HttpContext context, HttpResponseMessage response)
        {
            response.Content.Headers.ToList().ForEach(header =>
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            });
            context.Response.StatusCode = (int)response.StatusCode;
            await response.Content.CopyToAsync(context.Response.Body);
        }
    }
}
