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
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAuthTypeForUserName(string UserName)
        {
            UserAuthTypeResult r = new UserAuthTypeResult();

            // validate service account
            if (Request.Headers["Authorization"] == null) {
                r.ValidRequest = "NO";
                r.Message = "No Basic Authorization Found";
                return Json(r, JsonRequestBehavior.DenyGet);
            }
            string authHeader = Request.Headers["Authorization"];
            ServiceAuthenticator auth = new ServiceAuthenticator();
            bool isValid = auth.ValidateFromBasicAuthHeader(authHeader);

            if (isValid == false)
            {
                r.ValidRequest = "NO";
                r.Message = "Service Account Authorization Failed";
                return Json(r, JsonRequestBehavior.DenyGet);
            }

            // get auth type
            // validate against db to make sure user has not logged out.
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, UserName, true));
                string rtn = DataAccess.ExecuteScalar("usp_GetUserAuthType", parameters);
                r.ValidRequest = "YES";
                r.AuthType = rtn;
                if (rtn == "NTF") { r.Message = "User Not Found in System"; }
                if (rtn == "IOP") { r.Message = "User has OHID"; }
                if (rtn == "PNM") { r.Message = "User has PNM Login"; }
            }
            catch (Exception ex)
            {
                r.ValidRequest = "NO";
                r.AuthType = "";
                r.Message = ex.Message;
            }

            return Json(r, JsonRequestBehavior.DenyGet);

        }

    }
    public class UserAuthTypeResult
    {
        public string ValidRequest { get; set; }
        public string AuthType { get; set; }
        public string Message { get; set; }
    }
}