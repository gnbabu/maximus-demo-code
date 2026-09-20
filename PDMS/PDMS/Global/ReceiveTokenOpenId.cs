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

//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using Microsoft.IdentityModel.Tokens;
//using System.Linq;

/// <summary>
/// Summary description for ReceiveTokenOpenId
/// </summary>
public class ReceiveTokenOpenId
{
    
    private Guid m_threadId;
    public ReceiveTokenOpenId()
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
    public IOPAccessToken GetAccessToken(string code, ref string uname, ref string displayUname, ref bool IsIOPTimeOutException, ref bool isValidToken)
    {
        string logMsg = String.Format(CON.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
        Logging log = new Logging(this.ThreadId, logMsg);

        string client_secret = "";
        IOPAccessToken tokenResponse = new IOPAccessToken();
        string UresponseContent = "";
        string responseContent = "";
        IOPISVUserClaims userInfoResponse = new IOPISVUserClaims();
        int IOP_ResponseTimeOut = Convert.ToInt32(AppSettings.Get("IOPResponseTimeOut"));
        int IOP_ReadWriteTimeOut = Convert.ToInt32(AppSettings.Get("IOPReadWriteResponseTimeOut"));

        try
        {
            var sec = new SecretsManager(AppSettings.Get("SecretsRegion"));
            var result = sec.GetSuperSecretPassword(AppSettings.Get("IOPSecretDictionary"));
            result.Wait();

            client_secret = result.Result["IOPClientSecret"];            

            if (string.IsNullOrEmpty(client_secret))
            {
                //log.CreateLogEntry("IOP Client token empty", Logging.LogPriority.Error);
                throw new System.Exception("IOP client_token empty");
            }
        }
        catch(System.Exception ex)
        {
            log.CreateLogEntry("Error getting IOP Client token - " + ex.Message + " - " + ex.StackTrace, Logging.LogPriority.Error);
            throw new System.Exception("IOP client_token empty");
        }
        try
        {
            
            string tokenEndpoint = System.Configuration.ConfigurationManager.AppSettings["tokenEndpoint"];
            
            string redirectUri = System.Configuration.ConfigurationManager.AppSettings["RedirectUri"];
           
            string client_id = System.Configuration.ConfigurationManager.AppSettings["ClientId"];
            
            string grant_type = "authorization_code";

            byte[] data = Encoding.UTF8.GetBytes("grant_type=" + grant_type + "&code=" + code + "&state=OH" + "&redirect_uri=" + redirectUri);
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(client_id + ":" + client_secret));

            WebRequest request = WebRequest.Create(tokenEndpoint);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Authorization", "Basic " + encoded);
            request.Timeout = IOP_ResponseTimeOut;
            ((HttpWebRequest)request).ReadWriteTimeout = IOP_ReadWriteTimeOut;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

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
            tokenResponse = JsonConvert.DeserializeObject<IOPAccessToken>(responseContent);
            //log.CreateLogEntry("Got IOP token response - " + responseContent, Logging.LogPriority.Error);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.Timeout)
            {
                log.CreateLogEntry("Time Out getting IOP token response- " + ex.Message + " - " + ex.StackTrace, Logging.LogPriority.Error);
                IsIOPTimeOutException = true;                
            }
            else
            {
                IsIOPTimeOutException = false;
                log.CreateLogEntry("Error getting IOP token response- " + ex.Message + " - " + ex.StackTrace, Logging.LogPriority.Error);                
            }
            throw ex;
        }
        catch (System.Exception ex)
        {
            log.CreateLogEntry("Error getting IOP token response - " + ex.Message + " - " + ex.StackTrace, Logging.LogPriority.Error);
            throw ex;
        }

        if (tokenResponse.access_token != null)
        {
            try
            {
                string userInfoEndpoint = System.Configuration.ConfigurationManager.AppSettings["userInfoEndpoint"];
                
                WebRequest Urequest = WebRequest.Create(userInfoEndpoint);
                Urequest.Method = "GET";
                Urequest.Headers.Add("Authorization", "Bearer " + tokenResponse.access_token);
                Urequest.Timeout = IOP_ResponseTimeOut;
                ((HttpWebRequest)Urequest).ReadWriteTimeout = IOP_ReadWriteTimeOut;

                using (WebResponse Uresponse = Urequest.GetResponse())
                {
                    using (Stream stream = Uresponse.GetResponseStream())
                    {
                        using (StreamReader sr991 = new StreamReader(stream))
                        {
                            UresponseContent = sr991.ReadToEnd();
                        }
                    }
                }

                userInfoResponse = JsonConvert.DeserializeObject<IOPISVUserClaims>(UresponseContent);
                //string userresponse = JsonConvert.SerializeObject(userInfoResponse).ToString();
               // log.CreateLogEntry("Got IOP UserInfo response - " + UresponseContent, Logging.LogPriority.Error);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    IsIOPTimeOutException = true;
                    log.CreateLogEntry("Time Out getting IOP UserClaim response- " + ex.Message + " - " + ex.StackTrace, Logging.LogPriority.Error);
                }
                else
                {
                    IsIOPTimeOutException = false;
                    log.CreateLogEntry("Error Getting IOP UserClaim response- " + ex.Message + " - " + ex.StackTrace, Logging.LogPriority.Error);
                }
                    
                throw ex;
            }
            catch (System.Exception ex)
            {
                log.CreateLogEntry("Error getting IOP UserClaim - " + ex.Message + " - " + ex.StackTrace, Logging.LogPriority.Error);
                throw ex;
            }

            if (userInfoResponse == null)
            {
               // log.CreateLogEntry("IOP User claims is null", Logging.LogPriority.Error);
                throw new System.Exception("IOP User claims is null");
            }
            else
            {
                try
                {
                    PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                    string IOPUserName = string.Empty;
                    string userid = string.Empty;
                    if (!string.IsNullOrEmpty(userInfoResponse.preferred_username))
                    {
                        IOPUserName = userInfoResponse.preferred_username.ToString();       
                    }                        

                    if (!string.IsNullOrEmpty(userInfoResponse.userid)) // OHID
                    {
                         userid = userInfoResponse.userid.ToString();
                    }
                    try
                    {
                         displayUname = userInfoResponse.firstname.ToString() + " " + userInfoResponse.lastname.ToString();
                    }
                    catch (System.Exception ex)
                    {
                        log.CreateLogEntry("Error getting IOP display name -" + userInfoResponse.firstname + " - " + userInfoResponse.lastname + " - " + ex.Message + " - "+ex.StackTrace, Logging.LogPriority.Error);
                        throw new System.Exception("IOP User claims is null");
                    }
					
					MembershipUser userID = Membership.GetUser(userid);
                    Guid aspnet_userid = new Guid();
                    if (userID == null)
                    {
						List<SqlParameter> param = new List<SqlParameter>();
						param.Add(SqlParms.CreateParameter("LoginUserName", DbType.String, userid, true));
						DataSet ds = new DataSet();
						ds = DataAccess.ExecuteStoredProcedure("usp_GetUserIsActive", param, "IsUserActive");
						int isActive = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]);

                        if (isActive == 1) // User is inactive
                        {
                            aspnet_userid = Helper.GetUserId(userid);
                            List<SqlParameter> param1 = new List<SqlParameter>();
                            param1.Add(SqlParms.CreateParameter("UserName", DbType.String, userid, false));
                            DataAccess.ExecuteStoredProcedure("aspnet_Users_Activate_Custom", param1);

                        }
                        else
                        {
                            string password = Membership.GeneratePassword(10, 2);
                            MembershipUser user = Membership.CreateUser(userid, password);
                            if (!string.IsNullOrEmpty(userInfoResponse.email))
                                user.Email = userInfoResponse.email.ToString();
                            Membership.UpdateUser(user);
                            aspnet_userid = Helper.GetUserId(user.UserName);

                          //  log.CreateLogEntry("Created user for IOP User id - " + userid, Logging.LogPriority.Error);
                            svc.InsertUserAccountInformation(aspnet_userid.ToString(), displayUname, null, string.Empty, string.Empty, string.Empty, 0, 0, string.Empty, 0,
                                string.Empty, string.Empty, 0, 0, null, null, DateTime.Now, aspnet_userid.ToString(), true, userid, null, IOPUserName, regId: null);
							//log.CreateLogEntry("Create UserAccountInformation for IOP User id - " + userid, Logging.LogPriority.Error);

							//OHPNM-11445 Updating Created_by_user & created_Date_time & last_modifed_user & last_modified_Date_time for aspnet_users and aspnet_Membership
							List<SqlParameter> parameters = new List<SqlParameter>();
							//string loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
							parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, userid, false));
							parameters.Add(SqlParms.CreateParameter("Created_by_user", DbType.Guid, aspnet_userid, false));
							DataAccess.ExecuteStoredProcedure("aspnet_Users_CreateUser_Membership_Custom", parameters);
						}
                       
                    }                    
                    else
                    {
                        aspnet_userid = Helper.GetUserId(userid);
                        //OHPNM-18473 update OHID email if an updated one is sent
                        if (!string.IsNullOrEmpty(userInfoResponse.email) && userID.Email != userInfoResponse.email.ToString())
                        {
                            userID.Email = userInfoResponse.email.ToString();
                            Membership.UpdateUser(userID);
                            //svc.UpdateUserAccountInformation(aspnet_userid.ToString(), displayUname, null, string.Empty, string.Empty, DateTime.Now, aspnet_userid.ToString(), false, null);
                        }
                        if (!string.IsNullOrEmpty(displayUname))
                        {
                            DataSet ds = svc.GetUserAccountInformation(aspnet_userid.ToString());
                            if (ds != null && ds.Tables[0].Rows.Count > 0)
                            {
                                string contactName = ds.Tables[0].Rows[0]["Name"].ToString();
                                if (contactName != displayUname)
                                {
                                    svc.UpdateUserAccountInformation(aspnet_userid.ToString(), displayUname, null, string.Empty, string.Empty, DateTime.Now, aspnet_userid.ToString(), false, null);
                                }                                   
                            }
                        }
                    }
                        
                    uname = userid;
                    List<SqlParameter> sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("UserID", DbType.String, aspnet_userid, false));
                    sqlParms.Add(SqlParms.CreateParameter("IOP_ACCESS_TOKEN", DbType.String, tokenResponse.access_token, false));
                    isValidToken = Convert.ToString(DataAccess.ExecuteStoredProcedure("usp_ValidateUserIOPToken", sqlParms, "IsValid", SqlDbType.VarChar, 100)).Equals("true");

                    svc.SaveUserIOPToken(aspnet_userid, responseContent, aspnet_userid, UresponseContent,true, tokenResponse.access_token,tokenResponse.refresh_token,tokenResponse.id_token);
                   // log.CreateLogEntry("Done SaveUserIOPToken for IOP User id - " + userid, Logging.LogPriority.Error);
                }
                catch (System.Exception ex)
                {
                    log.CreateLogEntry("Error creating IOP User- " + ex.Message + " - " + ex.StackTrace, Logging.LogPriority.Error);
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("IOP_REFRESH_TOKEN", DbType.String, code, true));
                    parameters.Add(SqlParms.CreateParameter("EXCEPTION_MESSAGE", DbType.String, ex.Message, false));
                    parameters.Add(SqlParms.CreateParameter("STACK_TRACE", DbType.String, ex.StackTrace, false));
                    DataAccess.ExecuteStoredProcedure("usp_INSERT_USER_IOP_TOKEN_EXCEPTION", parameters);
                    
                    throw ex;
                }
            }

        }
        else
        {
            throw new System.Exception("Access token is null");
        }
        return tokenResponse;
    }    

    public IOPAccessToken IOPRefreshTokens(string refreshToken)
    {
        string client_id = System.Configuration.ConfigurationManager.AppSettings["ClientId"];
        string client_secret = "";
        var sec = new SecretsManager(AppSettings.Get("SecretsRegion"));
        var result = sec.GetSuperSecretPassword(AppSettings.Get("IOPSecretDictionary"));
        result.Wait();

        client_secret = result.Result["IOPClientSecret"];

        if (string.IsNullOrEmpty(client_secret))
        {
            throw new System.Exception("client_token empty");
        }
        string tokenEndpoint = System.Configuration.ConfigurationManager.AppSettings["tokenEndpoint"];
        string grant_type = "refresh_token";

        byte[] data = Encoding.UTF8.GetBytes("grant_type=" + grant_type + "&refresh_token=" + refreshToken);

        String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(client_id + ":" + client_secret));
        int IOP_ResponseTimeOut = Convert.ToInt32(AppSettings.Get("IOPResponseTimeOut"));
        int IOP_ReadWriteTimeOut = Convert.ToInt32(AppSettings.Get("IOPReadWriteResponseTimeOut"));

        WebRequest request = WebRequest.Create(tokenEndpoint);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.Headers.Add("Authorization", "Basic " + encoded);
        request.Timeout = IOP_ResponseTimeOut;
        ((HttpWebRequest)request).ReadWriteTimeout = IOP_ReadWriteTimeOut;

        using (Stream stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        string responseContent = "";

        try
        {
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
        }
        catch (WebException ex)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("IOP_REFRESH_TOKEN", DbType.String, refreshToken, true));
            parameters.Add(SqlParms.CreateParameter("EXCEPTION_MESSAGE", DbType.String, ex.Message, false));
            parameters.Add(SqlParms.CreateParameter("STACK_TRACE", DbType.String, ex.StackTrace, false));
            DataAccess.ExecuteStoredProcedure("usp_INSERT_USER_IOP_TOKEN_EXCEPTION", parameters);

            throw ex;
        }
        IOPAccessToken tokenResponse = new IOPAccessToken();
        tokenResponse = JsonConvert.DeserializeObject<IOPAccessToken>(responseContent);

        return tokenResponse;
    }

    public class IOPAccessToken
    {
        public string access_token;
        public string token_type;
        public string id_token;
        public int expires_in;
        public string refresh_token;
    }

    public class IOPUserClaims
    {
        public string sub;
        public string firstname;
        public string lastname;
        public string email;
        public string mobile;
        public string eidmlast4ssn;
        public string telephoneNumber;
        public string eidmaliases;
        public string preferred_username;
        public string initials;
        public string suffix;
        public string eidmuserid;
        public string eidmbirthdate;
    }

    public class IOPISVUserClaims
    {
        public string preferred_username;
        public string userid;
        public string firstname;
        public string lastname;
        public string email;
        public string mobile;
        public string eidmlast4ssn;
        public string telephoneNumber;
        public string initials;
        public string suffix;
        public string eidmbirthdate;
    }
}