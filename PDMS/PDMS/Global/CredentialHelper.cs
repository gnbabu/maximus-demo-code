using CON = MAXIMUS.Core.Libraries.Constants;
using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Web.Script.Serialization;
using System.Web;
using System.Configuration;
/// <summary>
/// Helper methods to credential application
/// </summary>
public class CredentialHelper
{
    public CredentialHelper()
    {

    }
    public class AMAAccessToken
    {
        public string access_token = "";
        public string toke_type;
        public int expires_in;
        public string scope;
    }
    public class APIToken
    {
        public string AccessToken = "";
        public string RefreshToken = "";
    }

    public class GetUserInfoModel
    {
        public string UserId;
        public string UserName;
        public string DisplayUserName;
        public string OHID;
        public string Code;
        public string Description;
    }

    public static bool CompletePositiveResultActivityStatus(int activityTypeID, int dataRankId)
    {
        // a positive result to the screening
        bool isPositiveResultStatus = false;

        switch (dataRankId)
        {
            case CON.ActivityDataRankId.Pass:
                isPositiveResultStatus = true;
                break;

            default:
                isPositiveResultStatus = false;
                break;
        }

        return isPositiveResultStatus;
    }
    public static bool IsPendingActivityStatus(int activityTypeID, int dataRankId)
    {
        bool IsPendingStatus = false;

        switch (dataRankId)
        {
            case CON.ActivityDataRankId.pending:

                IsPendingStatus = true;
                break;
            default:
                IsPendingStatus = false;
                break;
        }

        return IsPendingStatus;
    }
    public string createTokenRequest(string username)
    {

        try
        {
            string accessToken = ApplicationCache.AMAAccessToken(username);
            return accessToken;

        }
        catch (Exception ex)
        {
            Logging log = new Logging(new Guid(), "");
            log.CreateLogEntry("Failed to Create Token Request "
                                 + " Exception Message " + ex.Message + " Exception Stack = "
                                 + ex.StackTrace, Logging.LogPriority.Error);
            //exception;
            return string.Empty;
        }
    }
    public APIToken CreateAPIAccessToken()
    {
        try
        {
            string apipass = string.Empty;
            string secretsEnabled = AppSettings.Get("EnableUserValidation");
            if (secretsEnabled.Equals("1") || secretsEnabled.Equals("2"))
            {
                string[] result = AppSettings.Get("RestAPISecretRegionDictionaryKey").Split(',')
                       .Select(item => item.Trim())
                       .ToArray();
                var secret = new AmazonSecretsManager(result[0]);
                var secretResult = secret.GetSuperSecretPassword(result[1]);
                secretResult.Wait();
                apipass = secretResult.Result[result[2]];
            }
            else
            {
                apipass = ConfigurationManager.AppSettings["PNMSecretKey"].ToString();
            }

                string apiusr = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            string APIURL = ConfigurationManager.AppSettings["PDMSWebAPI"].ToString() + "Login/";
            string APItokenURL = APIURL + "login";
            WebRequest request = WebRequest.Create(APItokenURL);
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    Username = apiusr,
                    Password = apipass
                });

                streamWriter.Write(json);
            }
            string responseContent = "";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr99 = new StreamReader(stream))
                    {
                        responseContent = sr99.ReadToEnd();
                    }
                }
            }
            APIToken ac = new APIToken();
            ac = JsonConvert.DeserializeObject<APIToken>(responseContent);


            return ac;
        }
        catch (Exception ex)
        {
            Logging log = new Logging(new Guid(), "");
            log.CreateLogEntry("RESTAPI Token creation error:" + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);
            return new APIToken();
        }
    }

    public APIToken CreateAPIAccessToken(string username)
    {
        try
        {
            string apipass = string.Empty;
            string secretsEnabled = AppSettings.Get("EnableUserValidation");
            if (secretsEnabled.Equals("1") || secretsEnabled.Equals("2"))
            {
                string[] result = AppSettings.Get("RestAPISecretRegionDictionaryKey").Split(',')
                       .Select(item => item.Trim())
                       .ToArray();
                var secret = new AmazonSecretsManager(result[0]);
                var secretResult = secret.GetSuperSecretPassword(result[1]);
                secretResult.Wait();
                apipass = secretResult.Result[result[2]];
            }
            else
            {
                apipass = ConfigurationManager.AppSettings["PNMSecretKey"].ToString();
            }

            string apiusr = Helper.GetUserId(username).ToString();
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            string APIURL = ConfigurationManager.AppSettings["PDMSWebAPI"].ToString() + "Login/";
            string APItokenURL = APIURL + "login";
            WebRequest request = WebRequest.Create(APItokenURL);
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    Username = apiusr,
                    Password = apipass
                });

                streamWriter.Write(json);
            }
            string responseContent = "";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr99 = new StreamReader(stream))
                    {
                        responseContent = sr99.ReadToEnd();
                    }
                }
            }
            APIToken ac = new APIToken();
            ac = JsonConvert.DeserializeObject<APIToken>(responseContent);


            return ac;
        }
        catch (Exception ex)
        {
            Logging log = new Logging(new Guid(), "");
            log.CreateLogEntry("RESTAPI Token creation error:" + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);
            return new APIToken();
        }
    }
    public string createToken(string username)
    {
        try
        {
            var sec = new SecretsManager(AppSettings.Get("SecretsRegion"));
            var result = sec.GetSuperSecretPassword(AppSettings.Get("AMASecretDictionary"));
            result.Wait();

            string client_id = result.Result["AMAClientIdentifier"];
            string client_secret = result.Result["AMAClientSecret"];

            if (string.IsNullOrEmpty(client_id) || string.IsNullOrEmpty(client_secret))
            {
                throw new Exception("client_id or client_secret empty");
            }
            string grant_type = "client_credentials";

            string grantstring = "grant_type=" + grant_type + "&client_id=" + client_id + "&client_secret=" + client_secret;
            byte[] data = Encoding.ASCII.GetBytes(grantstring);
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            string AMAURL = AppSettings.Get("AMAURL");
            string tokenmethod = "/oauth2/endpoint/eprofilesprovider/token";
            string AMA_TokenRequestURL = AMAURL + tokenmethod;
            WebRequest request = WebRequest.Create(AMA_TokenRequestURL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("X-Location", "MAXIMUS");
            request.Headers.Add("X-CredentialProviderUserId", username);
            request.Headers.Add("X-SourceSystem", AppSettings.Get("BrandName") + AppSettings.Get("Environment"));

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }




            string responseContent = "";
            AMAAccessToken ac = new AMAAccessToken();
            using (WebResponse response = request.GetResponse())
            {
                HttpWebResponse webres = (HttpWebResponse)response;
                if (webres.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader sr99 = new StreamReader(stream))
                        {
                            responseContent = sr99.ReadToEnd();
                        }
                    }
                    ac = JsonConvert.DeserializeObject<AMAAccessToken>(responseContent);
                }

            }




            return ac.access_token;
        }
        catch (Exception ex)
        {
            Logging log = new Logging(new Guid(), "");
            log.CreateLogEntry("AMA Token creation error:" + ex.Message
             , Logging.LogPriority.Error);
            return string.Empty;
        }
    }
    public static bool CompleteNegativeResultActivityStatus(int activityTypeID, int dataRankId)
    {

        bool isNegativeResultStatus = false;

        switch (dataRankId)
        {
            case CON.ActivityDataRankId.Fail:
            case CON.ActivityDataRankId.Unclear:
            case CON.ActivityDataRankId.Unconfirmed:
            case CON.ActivityDataRankId.Conditional:
                isNegativeResultStatus = true;
                break;

            default:
                isNegativeResultStatus = false;
                break;
        }

        return isNegativeResultStatus;
    }
    public static int GetAMAStatusId(string AMAStatusCode)
    {
        int AMAStatusId = 0;
        switch (AMAStatusCode)
        {
            case CON.AMAStatusCode.SuccessfulCode:
            case CON.AMAStatusCode.Ok:
                AMAStatusId = CON.AMAStatusID.Successful;
                break;
            case CON.AMAStatusCode.Unauthorized:
            case CON.AMAStatusCode.UnauthorizedCode:
                AMAStatusId = CON.AMAStatusID.Unauthorized;
                break;
            case CON.AMAStatusCode.NoContent:
            case CON.AMAStatusCode.NoContentCode:
                AMAStatusId = CON.AMAStatusID.NoContent;
                break;
            case CON.AMAStatusCode.InternalServerError:
            case CON.AMAStatusCode.InternalServerErrorCode:
                AMAStatusId = CON.AMAStatusID.InternalServerError;
                break;
            case CON.AMAStatusCode.ProfileNotFoundByEntityID:
            case CON.AMAStatusCode.ProfileNotFoundByEntityIDCode:
            case CON.AMAStatusCode.Accepted:
                AMAStatusId = CON.AMAStatusID.ProfileNotFoundByEntityID;
                break;
            case CON.AMAStatusCode.ProfileOrderwasnotPlacedByEntityID:
            case CON.AMAStatusCode.ProfileOrderwasnotPlacedByEntityIDCode:
            case CON.AMAStatusCode.Conflict:
                AMAStatusId = CON.AMAStatusID.ProfileOrderwasnotPlacedByEntityID;
                break;
            case CON.AMAStatusCode.BadRequest:
            case CON.AMAStatusCode.BadRequestCode:
                AMAStatusId = CON.AMAStatusID.BadRequest;
                break;

        }
        return AMAStatusId;
    }

}
public class XMLResponseStatus
{
    public string xmlResponse;
    public string status;
}