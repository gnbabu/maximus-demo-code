using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Web.Security;
using MAXIMUS.Core.Libraries;
using System.Reflection;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Linq;

/// <summary>
/// Summary description for RedirectToCMCDashboardHelper
/// </summary>
public class RedirectToCMCDashboardHelper
{
    private Guid m_threadId;
    public RedirectToCMCDashboardHelper()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private Guid ThreadId
    {
        get
        {
            return this.m_threadId;
        }
        set
        {
            this.m_threadId = value;
        }
    }

    public bool MakeCallToCMCRedirectAPI (string medicaidID, ref string CMCDashboardredirectURL)
    {
        bool flag = false;
        string logMsg = String.Format(CON.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
        Logging log = new Logging(this.ThreadId, logMsg);

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        Guid loggedinUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        string ClientID = AppSettings.Get("CMCDashboardClientID");
        string ClientSecret = string.Empty;
        string CMCRedirectAPIURL = AppSettings.Get("CMCDashboardRedirectAPIURL"); //"https://test.apigateway.id.ohio.gov/odm/cmc/submit"; 

        try
        {
            var sec = new SecretsManager(AppSettings.Get("SecretsRegion"));
            var result = sec.GetSuperSecretPassword(AppSettings.Get("CMCAPISecretDictionary"));
            result.Wait();

            ClientSecret = result.Result["CMCAPIClientSecret"];

            if (string.IsNullOrEmpty(ClientSecret))
            {
                log.CreateLogEntry("CMC API Client Secret empty", Logging.LogPriority.Error);
                throw new Exception("CMC API Client Secret empty");
            }
        }
        catch (Exception ex)
        {
            log.CreateLogEntry("Error getting CMC API Client Secret - " + ex.Message + " - " + ex.StackTrace, Logging.LogPriority.Error);
            throw new Exception("CMC API Client Secret empty");
        }

        try
        {
            string taxID = string.Empty;
            string npi = string.Empty;
            string username = string.Empty;
            string OHID = string.Empty;
            string roleName = string.Empty;

            string CMCTableauDashboardToken = MaximusJWT.GetNewToken(HttpContext.Current.User.Identity.Name, loggedinUser.ToString(), System.Configuration.ConfigurationManager.AppSettings["PNMSecretKey"]);
            
            // save token to db
            svc.SaveUserPNMToken(loggedinUser, CMCTableauDashboardToken);

            DataSet ds = svc.SelectCMCProviderDetails(medicaidID, loggedinUser);
            if (Helper.HasRows(ds))
            {
                taxID = ds.Tables[0].Rows[0]["TAX_ID"].ToString();
                npi = ds.Tables[0].Rows[0]["NPI"].ToString();
                OHID = ds.Tables[0].Rows[0]["OHID"].ToString();
                username = ds.Tables[0].Rows[0]["CONTACT_NAME"].ToString();
                roleName = ds.Tables[0].Rows[0]["ROLENAME"].ToString();

                var parameters = new Dictionary<string, string>
                {
                    { "OHID", OHID },
                    { "Username", username },
                    { "NPI", npi  },
                    { "BillingProviderID", medicaidID},
                    { "TaxID", taxID},
                    { "UserRole", roleName },
                    { "AuthToken", CMCTableauDashboardToken },
                };

                string requestBody = JsonConvert.SerializeObject(parameters);

                const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender1, cert, chain, sslPolicyErrors) => { return true; };

                using (var client = new HttpClient(clientHandler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("client_id", string.Format("{0}", ClientID));
                    client.DefaultRequestHeaders.Add("client_secret", string.Format("{0}", ClientSecret));
                    // string info = JsonConvert.SerializeObject(new SMSSubscription() { list_name = "Welcome", subscriptions = new Subscription[] { subscription } });
                    Uri postUrl = new Uri(string.Format(CMCRedirectAPIURL));
                    var httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(postUrl.ToString(), httpContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        flag = true;
                        CMCDashboardredirectURL = response.Headers.GetValues("Location").FirstOrDefault();
                        if(string.IsNullOrEmpty(CMCDashboardredirectURL))
                        {
                            flag = false;
                            log.CreateLogEntry(String.Format("Dashboard redirect url empty from CMC IOP API URL: {0} {1}", (int)response.StatusCode, response.ReasonPhrase), Logging.LogPriority.Error);
                        }
                        client.Dispose();
                    }
                    else
                    {
                        flag = false;
                        CMCDashboardredirectURL = string.Empty;
                        client.Dispose();                        
                        log.CreateLogEntry(String.Format("Error Status from CMC IOP API URL: {0} {1}", (int)response.StatusCode, response.ReasonPhrase), Logging.LogPriority.Error);
                    }
                }

            }
        }
        catch (Exception ex)
        {
            log.CreateLogEntry("Error calling CMC IOP API - " + ex.Message + " - " + ex.StackTrace, Logging.LogPriority.Error);
            return false;            
        }
        return flag;
    }
}