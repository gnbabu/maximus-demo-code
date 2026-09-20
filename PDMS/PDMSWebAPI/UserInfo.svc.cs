using MAXIMUS.Core.Libraries;
using PDMSWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.ServiceModel;
using System.Web.Security;

namespace PDMSWebAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserInfo" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserInfo.svc or UserInfo.svc.cs at the Solution Explorer and start debugging.
    public class UserInfo : IUserInfo
    {
        Logging Log;

        public UserInfo()
        {
            var user = PDMSPrincipal.GetCurrentUser();
            if (user == null)
            {
                Log = new Logging(Guid.NewGuid(), "PDMSWebAPI:UserInfo");
            }
            else
            {
                Log = new Logging(user.UserThreadId, "PDMSWebAPI:UserInfo");
            }
        }

        public UserDetailModel GetUserInfoTKN(string AuthToken)
        {
            Log.CreateLogEntry(string.Format("Calling GetUserInfoTKN for {0}", AuthToken), Logging.LogPriority.Information);
            RolesAuthentication.Allow("WebAPI:UserInfo", "WebAPI:UserInfo-GetUserInfoTKN");

            UserDetailModel rtn = new UserDetailModel();

            string sKey = ConfigurationManager.AppSettings.Get("PNMSecretKey");

            int vld = MaximusJWT.ValidateToken(AuthToken, sKey);
            if (vld == MaximusJWT.TOKEN_VALID)
            {
                UserInfoLogic lg = new UserInfoLogic();
                string UserName = MaximusJWT.GetSingleClaimValue(AuthToken, sKey, ClaimTypes.NameIdentifier);
                rtn = lg.GetUserInfo(UserName);
                Log.CreateLogEntry("GetUserInfoTKN success", Logging.LogPriority.Information);
                return rtn;
            } else
            {
                // invalid token
                rtn.ErrorCode = "0101";
                rtn.ErrorMessage = "INVALID TOKEN";
                rtn.ErrorInfo = "INVALID TOKEN";
                Log.CreateLogEntry(String.Format("GetUserInfo error: {0}", rtn.ErrorMessage), Logging.LogPriority.Error);
                return rtn;
            }

        }
        public UserDetailModel GetUserInfo(string UserName, string EncryptedPassword)
        {
            Log.CreateLogEntry(string.Format("Calling GetUserInfo for {0}", UserName), Logging.LogPriority.Information);
            RolesAuthentication.Allow("WebAPI:UserInfo", "WebAPI:UserInfo-GetUserInfo");
           
            byte[] data = System.Convert.FromBase64String(EncryptedPassword);
            string pwd = System.Text.UTF8Encoding.UTF8.GetString(data);

            bool isValid = Membership.ValidateUser(UserName, pwd);

            UserDetailModel rtn = new UserDetailModel();

            string sKey = ConfigurationManager.AppSettings.Get("PNMSecretKey");

            if (isValid == true)
            {
                UserInfoLogic lg = new UserInfoLogic();
                rtn = lg.GetUserInfo(UserName);
                Log.CreateLogEntry("GetUserInfo success", Logging.LogPriority.Information);
                return rtn;
            }
            else
            {
                // invalid token
                rtn.ErrorCode = "0101";
                rtn.ErrorMessage = "Communication Error in connecting with Maximus Active Directory";
                rtn.ErrorInfo = "Communication Error in connecting with Maximus Active Directory";
                Log.CreateLogEntry(String.Format("GetUserInfo error: {0}", rtn.ErrorMessage), Logging.LogPriority.Error);
                return rtn;
            }

        }

        public UserAgentProvAssnModel GetUserAgentProvAssnDataTKN(string AuthToken)
        {
            Log.CreateLogEntry("Calling GetUserAgentProvAssnDataTKN AuthToken: " + AuthToken, Logging.LogPriority.Information);
            RolesAuthentication.Allow("WebAPI:UserInfo", "WebAPI:UserInfo-GetUserAgentProvAssnDataTKN");

            UserAgentProvAssnModel rtn = new UserAgentProvAssnModel();
            List<string> UserList = new List<string>();

            string sKey = ConfigurationManager.AppSettings.Get("PNMSecretKey");

            int vld = MaximusJWT.ValidateToken(AuthToken, sKey);
            if (vld == MaximusJWT.TOKEN_VALID)
            {
                UserInfoLogic lg = new UserInfoLogic();
                string UserName = MaximusJWT.GetSingleClaimValue(AuthToken, sKey, ClaimTypes.NameIdentifier);                
                rtn = lg.GetUserAgentProvAssnData(UserName, "", "");
                Log.CreateLogEntry("GetUserAgentProvAssnDataTKN success", Logging.LogPriority.Information);
                return rtn;
            }
            else
            {
                UserAgentProvAssnResultModel r = new UserAgentProvAssnResultModel();
                List<UserAgentProvAssnUserModel> u = new List<UserAgentProvAssnUserModel>();
                r.ReturnStatusCode = "0100";
                r.ReturnStatus = "ERROR";
                r.ReturnStatusDescription = "Error in getting the user information";
                rtn.Result = r;
                rtn.User = u;                
                Log.CreateLogEntry(String.Format("GetUserAgentProvAssnDataTKN error: {0}", r.ReturnStatusDescription), Logging.LogPriority.Error);
                return rtn;
            }

        }

        public UserAgentProvAssnModel GetUserAgentProvAssnData(string UserName, string WebRoleNameSearch, string FilterRole, string CreatedAfter = "")
        {

            string EncryptedPassword = "";
            string pwd = EncryptedPassword;   // TODO: Decrypt Password (need key?)
            bool isValid = true; // Membership.ValidateUser(UserName, pwd);

            UserAgentProvAssnModel rtn = new UserAgentProvAssnModel();


            /*
            OHPNM-4596 PRGCR089 - DEV - Update the inbound web service to include M&S Changes
            add <xs:element name="CreatedAfter" type="string" minOccurs="0"/>
            if UserName is blank, FilterRole is set and created after is set, then a list of all users that have the Agent Role (at the provider) level will be returned in a loop
            NOTE: This will only be enabled for users that have the Service Account Role: WebAPI:UserInfo-GetAllUsersForAgentRole
            */

            UserInfoLogic lg = new UserInfoLogic();

            if (isValid == true)
            {                
                rtn = lg.GetUserAgentProvAssnData(UserName, WebRoleNameSearch, CreatedAfter);
                Log.CreateLogEntry("GetUserAgentProvAssnData success", Logging.LogPriority.Information);
                return rtn;
            }
            else
            {
                UserAgentProvAssnResultModel r = new UserAgentProvAssnResultModel();
                List<UserAgentProvAssnUserModel> ur = new List<UserAgentProvAssnUserModel>();
                r.ReturnStatusCode = "0100";
                r.ReturnStatus = "ERROR";
                r.ReturnStatusDescription = "Error in getting the user information";
                rtn.Result = r;
                rtn.User = ur;                
                Log.CreateLogEntry(String.Format("GetUserAgentProvAssnData error: {0}", r.ReturnStatusDescription), Logging.LogPriority.Error);
                return rtn;
            }

        }

        public string EchoSoapRequest(int input)
        {
            RolesAuthentication.Allow("WebAPI:UserInfo", "WebAPI:UserInfo-EchoSoapRequest");
            var rawRequest = OperationContext.Current.RequestContext.RequestMessage.ToString();
            return rawRequest;
        }
    }
}
