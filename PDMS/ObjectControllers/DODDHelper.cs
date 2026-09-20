using MAXIMUS.Controllers.PDMS.AbuserService;
using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq.Expressions;
using System.Web;
using System.Web.Security;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.Controllers.PDMS
{
    public static class DODDHelper
    {
        const int DODDScreeningActivityId = 31;
        static DODDHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static bool CheckDODDAbsuerRegistry(int regId, string taxId, string lastName, string dob, int regOwnerId)
        {
            bool IsRejected = false;
       
            IsRejected = GetDODDVerificationResult(taxId, lastName, dob, regId, regOwnerId);
            return IsRejected;
        }

        public static void UpdateDODDActivityStatus(int regId, bool isRejected)
        {
            int activityStatus = isRejected ? CON.ScreeningActivityStatusId.Failed : CON.ScreeningActivityStatusId.Confirmed;
            ScreeningController.UpdateScreeningActivityStatus(regId, DODDScreeningActivityId, "", DateTime.Now, CON.appPDMSAdminUserId); 
        }

        private static DataRow GetProviderInfo(int regID)
        {
            DataRow rtn = null;
            //PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient();
            DataSet ds = RegistrationController.SelectRegistrationData(regID, "PROVIDER");
            if (HasRows(ds)) rtn = ds.Tables[0].Rows[0];
            return rtn;
        }

        private static bool HasRows(DataSet ds)
        {
            return (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0);
        }

        private static string GetString(string elementName, DataRow dr)
        {
            string returnValue = "";
            if (!dr.IsNull(elementName))
            {
                returnValue = HttpUtility.HtmlDecode(dr[elementName].ToString());
                returnValue = returnValue.Replace("''", "'");
            }

            return returnValue;
        }

        private static int GetInt(string elementName, DataRow dr)
        {
            int returnValue = 0;
            if (!dr.IsNull(elementName))
            {
                returnValue = Convert.ToInt32(dr[elementName]);
            }
            return returnValue;
        }

        private static bool GetDODDVerificationResult(string taxID, string lastName, string dob,int regID, int regOwnerId)
        {
            bool rejected = false;
            try
            {
                if (!string.IsNullOrEmpty(taxID))
                {                    
                    Abuser abuser = new Abuser();
                    DODDVerificationRequest request = new DODDVerificationRequest()
                    {
                        SSN = taxID.ToString(),
                        LastName = lastName,
                        DOB = dob
                    };
                    var resList = abuser.GetAbuserListBySSNAndLastName(taxID, lastName, "");
                    if(resList != null && resList.Length > 0)
                    {
                        string strRequest = JsonConvert.SerializeObject(request);
                        string strResponse = JsonConvert.SerializeObject(resList[0]).ToString();
                                            
                        InsertDODDverificationRequestResult(regID, strRequest, lastName, dob, taxID, strResponse, regOwnerId);
                         rejected = true;
                    }                   
                }              
            }

            catch (Exception ex)
            {
                
                Logging log = new Logging(); 
                log.CreateLogEntry("Time out in DODDAbuser" + ex.InnerException, Logging.LogPriority.Error);
                
            }

            return rejected;
        }

        private static int InsertDODDverificationRequestResult(int regId, string request, string lastName, string DOB, string SSN,string response, int regOwnerId)
        {
            int regLicenseVerificationID = 0;
            try
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", regId.ToString());
                parms.Add("LAST_NAME", lastName);
                parms.Add("SSN", SSN.ToString());
                parms.Add("DOB", DOB);
                parms.Add("DODD_REQUEST", request);
                parms.Add("DODD_RESPONSE", response);
                parms.Add("DODD_VERIFIED_DATE", DateTime.Now.ToString());
                parms.Add("CREATED_BY_USER", CON.appPDMSAdminUserId);
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", CON.appPDMSAdminUserId);
                parms.Add("REG_OWNER_ID", regOwnerId.ToString());
                regLicenseVerificationID = RegistrationController.InsertRegistrationData("DODD_VERIFICATION", parms);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return regLicenseVerificationID;

        }

        private static int UpdateDODDverificationRequestResult(string lastName, int SSN, int regID, string response)
        {
            int regLicenseVerificationID = 0;
            try
            {

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("LAST_NAME", lastName);
                parms.Add("SSN", SSN.ToString());
                parms.Add("REG_ID", regID.ToString());
                parms.Add("DODD_RESPONSE", response.ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", CON.appPDMSAdminUserId);
                RegistrationController.UpdateRegistrationData("DODD_VERIFICATION", parms);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return regLicenseVerificationID;
        }
        private static Guid GetUserId(string username)
        {
            MembershipUser usr = Membership.GetUser(username);
            if (usr == null) return Guid.Empty;
            else return new Guid(usr.ProviderUserKey.ToString());
        }
    
        //For Testing
        public static string GetFileContents(string fileName)
        {
            string strFileContents = string.Empty;
            MAXIMUS.Core.Libraries.Logging log = new Core.Libraries.Logging();
            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);

                if (!fileInfo.Exists)
                {

                    log.CreateLogEntry("File not found: " + fileName);
                    throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("File not found: " + fileName));
                }
                using (StreamReader srFile = new StreamReader(fileName))
                {
                    strFileContents = srFile.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Exception in GetFileContents: " + ex.Message);
            }
            return strFileContents;
        }

        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }
    }
}
