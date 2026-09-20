using MAXIMUS.Core.Libraries;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace PDMSRestServices.Facade
{
    public static class HelperFacade
    {
        private static string _ValidFileExtensions = "doc,docx,pdf,xls,XLS,xlsm,xlsx,ppt,pptx,mdi,jpe,zip,txt,jpg,jpeg,png,gif,bmp,tif,tiff,pi,ec,zip,csv,xlsm,msg,acrbak";

        public static Guid GetUserId(string username)
        {
            Guid userID = new Guid();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("USERNAME", DbType.String, username, false));
                DataSet userIDDS = DataAccess.ExecuteStoredProcedure("usp_GetUserIdFromUserName", parameters, "GetUserId");
                if (userIDDS != null)
                {
                    DataTable userIDDT = userIDDS.Tables[0];
                    if (userIDDT != null && userIDDT.Rows.Count > 0)
                    {
                        DataRow userIDDR = userIDDT.Rows[0];
                        userID = new Guid(Methods.GetString("USERID", userIDDR));
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return userID;
        }

        public static DataSet CheckIfUserExists(string userid)
        {
            DataSet retUserRoles = null;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("USERID", DbType.Guid, userid, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CheckIfUserExists", parameters, "CheckIfUserExists");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    retUserRoles = ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + " " + ex.StackTrace.ToString());
            }
            return retUserRoles;
        }

        public static TokenValidationParameters GetValidationParameters(string configKey)
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "Sample",
                ValidAudience = "Sample",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configKey)) // The same key as the one that generate the token
            };
        }

        public static DataSet SelectUserAccountInformation(string userId)
        {
            // create parameters objects and fill with values
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, false));

            DataSet ds = new DataSet();
            ds = DataAccess.ExecuteStoredProcedure("usp_SelectUserAccountInformation", parameters, "UserAccountInformation");
            ds.Tables[0].TableName = "UserAccountInformation";
            return ds;
        }



        /// <summary>
        /// Remove Path Transversal patterens, Wildcards, and invalid characters
        /// </summary>
        /// <param name="path"></param>
        /// <param name="allowRootPath">When true paths with a leading slash will be allowed. when false a leading slash will be removed.</param>
        /// /// <param name="allowRootPath">When true paths with a leading double slash will be allowed. when false a leading double slash will be removed.</param>
        /// <returns></returns>
        public static string CleanFilePath(string path, bool allowRootPath = false, bool allowUNCPath = false)
        {
            var PathTrans = @"(^\.\./|(?<=/)\.\./)"; PathTrans = PathTrans.Replace(@"/", Regex.Escape(System.IO.Path.DirectorySeparatorChar.ToString())); //replace slash with platform specific separator character.
            var invalidchars = Regex.Escape(new string(System.IO.Path.GetInvalidPathChars()));
            var driveRoot = "^[a-zA-Z]:" + Regex.Escape(System.IO.Path.DirectorySeparatorChar.ToString());
            var wildcards = "*?";
            var unc = System.IO.Path.DirectorySeparatorChar.ToString() + System.IO.Path.DirectorySeparatorChar.ToString();
            //order of sanitation is vital do not change. removing in different order could create unsafe patterns.
            path = System.Text.RegularExpressions.Regex.Replace(path, "[" + invalidchars + wildcards + "]", ""); //remove invalid chars and wildcards. This must happen first 
            if (allowRootPath)
            {
                var clean = false;
                var limit = 0;
                do
                {
                    var dr = "";
                    var cxPath = path;
                    if (!allowUNCPath && cxPath.StartsWith(unc)) cxPath = cxPath.Substring(unc.Length); //remove double slash unc root
                    var drMatch = System.Text.RegularExpressions.Regex.Match(cxPath, driveRoot); //match drive segment
                    if (drMatch.Success)
                    {
                        cxPath = cxPath.Substring(drMatch.Value.Length); //get path after drive segment
                        dr = drMatch.Value;
                    }
                    cxPath = dr + cxPath.Replace(":", "");

                    if (path == cxPath) clean = true; //eliminate ':' not in drive segment
                    else path = cxPath;
                    if (limit++ == 100) throw new ApplicationException("File path contains too many invalid characters");
                } while (clean == false);
            }
            else
            {
                var clean = false;
                var limit = 0;
                do
                {
                    var cxPath = path;
                    if (!allowUNCPath && cxPath.StartsWith(unc)) cxPath = cxPath.Substring(unc.Length); //remove double slash unc root
                    if (cxPath.StartsWith(System.IO.Path.DirectorySeparatorChar.ToString())) cxPath = cxPath.Substring(1); //remove leading slash
                    cxPath = System.Text.RegularExpressions.Regex.Replace(cxPath, driveRoot, ""); //remove DriveRoot i.e. C:\
                    cxPath = cxPath.Replace(":", "");
                    if (path == cxPath) clean = true; else path = cxPath;
                    if (limit++ == 100) throw new ApplicationException("File path contains too many invalid characters");
                } while (clean == false);
            }
            path = System.Text.RegularExpressions.Regex.Replace(path, PathTrans, ""); //remove path transversals i.e. '../' 
            return path;
        }


        public static bool IsValidExtension(string fileName, out string errMsg)
        {
            bool rtn = false;
            errMsg = string.Empty;

            fileName = CleanFilePath(fileName);

            // extarct and store the file extension into another variable
            string fileExtension = System.IO.Path.GetExtension(fileName).Replace(".", string.Empty).ToLower();

            // string type array having list of allowed file type extensions
            string[] validFileExtensions = _ValidFileExtensions.Split(',');
            // loop over the array of valid file extensions to compare them with uploaded file
            foreach (string extension in validFileExtensions)
            {
                if (fileExtension == extension)
                {
                    rtn = true;
                    break;
                }
            }

            // display the message based on the flag value
            if (!rtn)
            {
                errMsg = "Files with extension <b>\"" + fileExtension + "\"</b> are not allowed.<br />";
                errMsg += "You can upload files with the following extensions only:";
                foreach (string str in validFileExtensions)
                {
                    errMsg += " ." + str + ",";
                }
                errMsg = errMsg.Substring(0, errMsg.Length - 1);            // Remove "," at end
            }
            return rtn;
        }

        public static string HtmlEncode(string unsafeString)
        {
            return WebUtility.HtmlEncode(unsafeString);
        }

        public static DataSet GetAgentAdministrationDropdowns(string loggedinUserID, string selectedProvAdminUserID)
        {
            DataSet ds = new DataSet();
            try
            {
                
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("LoggedInUserID", DbType.Guid, loggedinUserID, false));
                parameters.Add(SqlParms.CreateParameter("SelectedProvAdminUserID", DbType.Guid, selectedProvAdminUserID, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_GetAgentAdministrationDropdowns", parameters, "AgentByProvAdmin");
                ds.Tables[0].TableName = "AgentByProvAdmin";
                return ds;

            }
            catch (Exception ex)
            {
               // throw CoreException.ThrowException(ex);
            }
            return ds;
        }
        
        public static DataSet GetProviderAgentsBySearchCriteria(string medIdList, string agentIdList, string agentRoleList, string loggedinUserID, string selectedProvAdminUserID)
        {
            DataSet ds = new DataSet();
            try
            {                
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MEDICAID_LIST", DbType.String, medIdList, true));
                parameters.Add(SqlParms.CreateParameter("OHID_LIST", DbType.String, agentIdList, true));
                parameters.Add(SqlParms.CreateParameter("SUBROLE_LIST", DbType.String, agentRoleList, true));
                parameters.Add(SqlParms.CreateParameter("LoggedInUserID", DbType.Guid, loggedinUserID, false));
                parameters.Add(SqlParms.CreateParameter("SelectedProvAdminUserID", DbType.Guid, selectedProvAdminUserID, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_GetProviderAgentsBySearchCriteria", parameters, "AgentRolesByProvAdmin");
                ds.Tables[0].TableName = "AgentRolesByProvAdmin";

                return ds;
            }
            catch (Exception ex)
            {
                //throw CoreException.ThrowException(ex);
            }
            return ds;
        }
        public static DataSet GetAgentRolesByProvAdminAndMedID(string medId, string agentOhId, string agentEmail, string userId, string callType)
        {
            DataSet ds = new DataSet(); 
            try
            {
                
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MED_ID", DbType.String, medId, true));
                parameters.Add(SqlParms.CreateParameter("AGENT_OH_ID", DbType.String, agentOhId, true));
                parameters.Add(SqlParms.CreateParameter("AGENT_EMAIL", DbType.String, agentEmail, true));
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, userId, false));
                parameters.Add(SqlParms.CreateParameter("CALL_TYPE", DbType.String, callType, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_GetAgentRolesByProvAdminAndMedID", parameters, "AgentRolesByProvAdminMedID");
                ds.Tables[0].TableName = "AgentRolesByProvAdminMedID";
                return ds;

            }
            catch (Exception ex)
            {
               // throw CoreException.ThrowException(ex);
            }
            return ds;
        }
        
        public static void SaveAgentRolesByProviderAdminAndMedID(string medId, string agentOhId, string userId, string agentRoleList)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medId, true));
                parameters.Add(SqlParms.CreateParameter("AGENT_OH_ID", DbType.String, agentOhId, true));
                parameters.Add(SqlParms.CreateParameter("LOGGEDIN_USER_ID", DbType.Guid, userId, false));
                parameters.Add(SqlParms.CreateParameter("AGENT_ROLE_LIST", DbType.String, agentRoleList, true));
                DataAccess.ExecuteStoredProcedure("usp_SaveAgentRolesByProviderAdminAndMedID", parameters);
            }
            catch (Exception ex)
            {
                //throw CoreException.ThrowException(ex);
            }
        }

        public static void DeleteProviderAgentByMedicaidID(string agentOHID, string medID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medID, true));
                parameters.Add(SqlParms.CreateParameter("AGENT_OH_ID", DbType.String, agentOHID, true));
                DataAccess.ExecuteStoredProcedure("usp_DeleteProviderAgentbyMedID", parameters);
            }
            catch (Exception ex)
            {
                //throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SaveReassignAdministratorByMedicaidID(string adminOHID, string medID, string userID)
        {
            DataSet ds = new DataSet();
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ADMIN_OHID", DbType.String, adminOHID, false));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medID, false));
                parameters.Add(SqlParms.CreateParameter("LOGGED_IN_USER", DbType.Guid, userID, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_SaveReassignAdministratorByMedicaidID", parameters, "AgentRolesByProvAdminMedID");
                ds.Tables[0].TableName = "dtSaveReassignAdmin";
                return ds;

            }
            catch (Exception ex)
            {
                // throw CoreException.ThrowException(ex);
            }
            return ds;
        }

        public static DataSet SelectRegistrationProviderTypesByCategory(int applicationTypeId, int categoryTypeID, int waiverTypeID)
        {
            DataSet lookup = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CategoryTypeID", DbType.Int32, categoryTypeID, true));
                parameters.Add(SqlParms.CreateParameter("ApplicationTypeID", DbType.Int32, applicationTypeId, true));
                parameters.Add(SqlParms.CreateParameter("WaiverTypeID", DbType.Int32, waiverTypeID, true));
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProviderTypesForRegistration", parameters, "ProviderType");
                lookup.Tables[0].TableName = "ProviderType";

                return lookup;
            }
            catch (Exception ex)
            {
                //throw CoreException.ThrowException(ex);
            }
            return lookup;
        }

        public static DataSet SelectStates()
        {
            DataSet states = new DataSet();
            try
            {                
                states = DataAccess.ExecuteStoredProcedure("usp_SelectStates", "States");
                states.Tables[0].TableName = "States";

                return states;
            }
            catch (Exception ex)
            {
                //throw CoreException.ThrowException(ex);
            }
            return states;
        }

        public static DataSet SelectGender()
        {
            DataSet gender = new DataSet();
            try
            {
                gender = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_GENDER", "ProviderGender");
                gender.Tables[0].TableName = "ProviderGender";

                return gender;
            }
            catch (Exception ex)
            {
                //throw CoreException.ThrowException(ex);
            }
            return gender;
        }

    }
}
