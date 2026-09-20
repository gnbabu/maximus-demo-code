using MAXIMUS.Core.Libraries;
using PDMSWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;

namespace PDMSWebAPI.Controllers
{
    public class TokenController : Controller
    {
        // GET: Token
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetTestToken(string APIKey, string UserName)
        {          

            string ClientSecret = ConfigurationManager.AppSettings.Get("PNMSecretKey");
            string TestAPIKey = ConfigurationManager.AppSettings.Get("TestAPIKey");

            // TODO: Verify we are not in production
      
            TestTokenResult r = new TestTokenResult();
            r.UserFound = "NO";
            r.AuthToken = "";
            r.ErrorMessage = "";

            // validate service account
            if (this.VerifyServiceAccount() == false)
            {
                r.ErrorMessage = "SERVICE_ACCOUNT_AUTH_FAILED";
                return Json(r, JsonRequestBehavior.DenyGet);
            }

            if (APIKey != TestAPIKey)
            {
                r.ErrorMessage = "Invalid API Key";
                return Json(r, JsonRequestBehavior.DenyGet);
            }

            // get user id for user name
            string spName = "usp_GetUserIDForUserName";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("user_name", DbType.String, UserName, false));
            DataSet ds = new DataSet();
            ds = DataAccess.ExecuteStoredProcedure(spName, parameters, "RS");
            string uid = ds.Tables[0].Rows[0][0].ToString();  // returns userid or blank string

            if (uid == "")
            {
                r.UserFound = "NO";
                r.AuthToken = "";
            } else
            {
                r.UserFound = "YES";
                r.AuthToken = MaximusJWT.GetNewToken(UserName, uid, ClientSecret);
            }

            // save to table
            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, uid, true));
            parameters.Add(SqlParms.CreateParameter("PNMToken", DbType.String, r.AuthToken, true));
            DataAccess.ExecuteStoredProcedure("usp_SaveUserPNMToken", parameters);

            return Json(r, JsonRequestBehavior.DenyGet);

        }

        [HttpPost]
        public JsonResult CheckToken(string AuthToken)
        {            
            string ClientSecret = ConfigurationManager.AppSettings.Get("PNMSecretKey");
            CheckTokenResult r = new CheckTokenResult();
            int t = 0;

            // validate service account
            if (this.VerifyServiceAccount() == false)
            {
                r.IsValid = "NO";
                r.Message = "SERVICE_ACCOUNT_AUTH_FAILED";
                return Json(r, JsonRequestBehavior.DenyGet);
            }

            t = MaximusJWT.ValidateToken(AuthToken, ClientSecret);

            if (t == MaximusJWT.TOKEN_VALID)
            {
                r.IsValid = "YES";
                r.Message = "TOKEN_VALID";

                // validate against db to make sure user has not logged out.
                try
                {
                    string userId = MaximusJWT.GetSingleClaimValue(AuthToken, ClientSecret, MaximusJWT.CLAIM_TYPE_USER_ID);
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, true));
                    parameters.Add(SqlParms.CreateParameter("PNMToken", DbType.String, AuthToken, true));
                    string rtn = DataAccess.ExecuteScalar("usp_CheckUserPNMToken", parameters);
                    if (rtn == "FAILED") {
                        r.IsValid = "NO";
                        r.Message = "TOKEN_USER_LOGGED_OUT";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else {
                r.IsValid = "NO";
                if (t== MaximusJWT.TOKEN_ERROR) { r.Message = "TOKEN_ERROR";  }
                if (t == MaximusJWT.TOKEN_EXPIRED) { r.Message = "TOKEN_EXPIRED"; }
                if (t == MaximusJWT.TOKEN_INVALID) { r.Message = "TOKEN_INVALID"; }
                if (t == MaximusJWT.TOKEN_MISSING) { r.Message = "TOKEN_MISSING"; }
                if (t == MaximusJWT.TOKEN_USER_LOGGED_OUT) { r.Message = "TOKEN_USER_LOGGED_OUT"; }
            }

            return Json(r, JsonRequestBehavior.DenyGet);

        }

        private bool VerifyServiceAccount()
        {
            Logging Log = new Logging(Guid.NewGuid(), "ServiceAuthenticator:Validate");

            if (Request.Headers["Authorization"] == null ) { return false; }

            string authToken = Request.Headers["Authorization"].Replace("Basic ", "");
            string userName = "";
            string password = "";
            ServiceAuthenticator auth = new ServiceAuthenticator();

            // decoding authToken we get decode value in 'Username:Password' format  
            var decodeauthToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

            // spliting decodeauthToken using ':'   
            var arrUserNameandPassword = decodeauthToken.Split(':');
            userName = arrUserNameandPassword[0];
            password = arrUserNameandPassword[1];

            try
            {
                auth.Validate(userName, password);
            } 
            catch
            {
                // Authenicate the user credentials via ADFS here

                Log.CreateLogEntry(String.Format("Invalid user credentials for {0}", arrUserNameandPassword[0]), 305);

                return false;

            }

            // if we get here all is good
            return true;

        }
    }
}