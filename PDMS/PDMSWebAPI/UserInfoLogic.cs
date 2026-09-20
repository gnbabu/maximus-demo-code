using MAXIMUS.Core.Libraries;
using PDMSWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PDMSWebAPI
{
    public class UserInfoLogic
    {

        public UserDetailModel GetUserInfo(string UserName)
        {

            UserDetailModel u = new UserDetailModel();

            try
            {

                // create parameters objects and fill with values
                string spName = "usp_GetUserInfoForUserName";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, UserName, false));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure(spName, parameters, "RS");

                if (ds.Tables[0].Rows.Count == 0)
                {
                    u.ErrorCode = "0104";
                    u.ErrorMessage = "No User Found for: " + UserName;
                    return u;
                }
                else
                {
                    u.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                    u.AccountExpires = ds.Tables[0].Rows[0]["AccountExpires"].ToString();
                    u.BadPasswordTime = ds.Tables[0].Rows[0]["BadPasswordTime"].ToString();
                    u.BadPasswordCount = ds.Tables[0].Rows[0]["BadPasswordCount"].ToString();
                    u.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    u.DisplayName = ds.Tables[0].Rows[0]["DisplayName"].ToString();
                    u.EmployeeID = ds.Tables[0].Rows[0]["EmployeeID"].ToString();
                    u.GivenName = ds.Tables[0].Rows[0]["GivenName"].ToString();
                    u.LastLogonTimestamp = ds.Tables[0].Rows[0]["LastLogonTimestamp"].ToString();
                    u.LogonCount = ds.Tables[0].Rows[0]["LogonCount"].ToString();
                    u.Mail = ds.Tables[0].Rows[0]["Mail"].ToString();
                    u.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                    u.PwdLastSet = ds.Tables[0].Rows[0]["PwdLastSet"].ToString();
                    u.SN = ds.Tables[0].Rows[0]["SN"].ToString();
                    u.TelephoneNumber = ds.Tables[0].Rows[0]["TelephoneNumber"].ToString();
                    u.WhenChanged = ds.Tables[0].Rows[0]["WhenChanged"].ToString();
                    u.WhenCreated = ds.Tables[0].Rows[0]["WhenCreated"].ToString();
                    u.ErrorInfo = "";
                    u.ErrorCode = "";
                    u.ErrorMessage = "";

                    ds.Dispose();

                    return u;
                }


            }
            catch (Exception ex)
            {
                u.ErrorCode = "0104";
                u.ErrorMessage = ex.Message;
                u.ErrorInfo = ex.StackTrace;
                return u;
            }

            // should never get here


        }

        
        public UserAgentProvAssnModel GetUserAgentProvAssnData(string UserName, string WebRoleNameSearch, string CreatedAfter)
        {
            UserAgentProvAssnModel rtn = new UserAgentProvAssnModel();
            UserAgentProvAssnResultModel r = new UserAgentProvAssnResultModel();
            List<UserAgentProvAssnUserModel> urs = new List<UserAgentProvAssnUserModel>();
            UserAgentProvAssnUserModel u = new UserAgentProvAssnUserModel();
            UserAgentProvRolesModel e = new UserAgentProvRolesModel();
            UserAgentProvAssnProviderModel p = new UserAgentProvAssnProviderModel();
            UserRolesModel urm = new UserRolesModel();
            SqlDataReader rs;
            SqlConnection cn;
            int UserCount = 0;
            int ux = 0;
            int px = 0;

            int RegId = 0;
            string UserId = "";
            string AgentId = "";

            rtn.Result = r;
            rtn.User = urs;

            try
            {
                string connString = AppSettings.GetConnectionString();
                cn = new SqlConnection(connString);

                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_GetUserAgentProvAssnFullForUserOrRole";  // (returns 5 result sets - count, users, roles, providers, subagent roles)

                cmd.Parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, UserName, false));
                cmd.Parameters.Add(SqlParms.CreateParameter("FilterRole", DbType.String, WebRoleNameSearch, false));
                cmd.Parameters.Add(SqlParms.CreateParameter("CreatedAfter", DbType.String, CreatedAfter, false));
                rs = cmd.ExecuteReader();
                
                // check for # users
                if (rs.Read() == false)
                {
                    r.ReturnStatusCode = "0100";
                    r.ReturnStatus = "ERROR";
                    r.ReturnStatusDescription = "Error in getting the user information - no data returned";
                    rtn.Result = r;
                    return rtn;
                }


                UserCount = int.Parse(rs["UsersFound"].ToString());

                if (UserCount == 0)
                {
                    r.ReturnStatusCode = "0100";
                    r.ReturnStatus = "ERROR";
                    r.ReturnStatusDescription = "Error in getting the user information - no users found";
                    rtn.Result = r;
                    return rtn;
                }


                rs.NextResult();

                // load all users

                while (rs.Read())
                {
                    u = new UserAgentProvAssnUserModel();
                    u.UserName = rs["UserName"].ToString();
                    u.UserTypeDesc = rs["UserTypeDesc"].ToString();
                    u.SAKWebUser = rs["SAKWebUser"].ToString();
                    u.DateLastLogon = rs["DateLastLogon"].ToString();
                    u.ContactName = rs["ContactName"].ToString().Trim();
                    u.EmailAddress = rs["EmailAddress"].ToString().Trim();
                    u.PhoneNumber = rs["PhoneNumber"].ToString().Trim();
                    u.ActiveIndicator = rs["ActiveIndicator"].ToString();
                    u.CurrentContractEndDate = rs["CurrentContractEndDate"].ToString();
                    u.UserRoles = new List<UserRolesModel>();
                    u.Provider = new List<UserAgentProvAssnProviderModel>();
                    rtn.User.Add(u);
                }

                rs.NextResult();

                // load all roles 
                while (rs.Read())
                {
                    urm = new UserRolesModel();
                    urm.RoleName = rs["RoleName"].ToString();
                    urm.RoleDescription = rs["Description"].ToString();
                    ux = rtn.User.FindIndex(usr => usr.UserName == rs["UserName"].ToString());
                    if (ux > -1)
                    {
                        rtn.User[ux].UserRoles.Add(urm);
                    }
                }

                rs.NextResult();

                // load all providers

                while (rs.Read())
				{
                    p = new UserAgentProvAssnProviderModel();
                    p.ProvUserName = rs["ProvUserName"].ToString().Trim();
                    p.ProvDateLastLogon = rs["ProvDateLastLogon"].ToString();
                    p.ProvContactName = rs["ProvContactName"].ToString().Trim();
                    p.ProvEmailAddress = rs["ProvEmailAddress"].ToString().Trim();
                    p.ProvPhoneNumber = rs["ProvPhoneNumber"].ToString().Trim();
                    p.SAKDefaultProvider = rs["SAKDefaultProvider"].ToString();
                    p.ProvProviderID = rs["ProvProviderID"].ToString();
                    p.ProvProviderTypeID = rs["ProvProviderTypeID"].ToString();
                    p.ProvActiveIndicator = rs["ProvActiveIndicator"].ToString();
                    p.ProvCurrentcontractEndDate = rs["ProvCurrentcontractEndDate"].ToString();
                    p.RenderingProviderPracticeName = rs["RenderingProviderPracticeName"].ToString().Trim();
                    p.RenderingProviderPracticeAddress = rs["RenderingProviderPracticeAddress"].ToString().Trim();
                    p.RenderingProviderPracticeCity = rs["RenderingProviderPracticeCity"].ToString().Trim();
                    p.RenderingProviderPracticeState = rs["RenderingProviderPracticeState"].ToString().Trim();
                    p.RenderingProviderPracticeZipCode = rs["RenderingProviderPracticeZipCode"].ToString().Trim();
                    p.RenderingProviderPracticeTelephone = rs["RenderingProviderPracticeTelephone"].ToString().Trim();
                    p.RenderingProviderPracticeFax = rs["RenderingProviderPracticeFax"].ToString().Trim();
                    p.RenderingProviderPracticeEmail = rs["RenderingProviderPracticeEmail"].ToString().Trim();
                    p.RenderingProviderPracticeContactName = rs["RenderingProviderPracticeContactName"].ToString().Trim();
                    p.PracticeNPI = rs["PracticeNPI"].ToString().Trim();
                    p.ProviderNPI = rs["ProviderNPI"].ToString().Trim();
                    p.ProviderMCDID = rs["ProviderMCDID"].ToString().Trim();
                    p.PracticeMCDID = rs["PracticeMCDID"].ToString().Trim();
                    p.PracticeMITSAdministrator = rs["PracticeMITSAdministrator"].ToString().Trim();
                    p.ProvRolesAssignedToAgent = new List<UserAgentProvRolesModel>();

                    ux = rtn.User.FindIndex(usr => usr.UserName == rs["UserName"].ToString());
                    if (ux > -1)
                    {
                        rtn.User[ux].Provider.Add(p);
                    }
                }


                rs.NextResult();

                // load sub agent roles

                while (rs.Read())
				{
                    e = new UserAgentProvRolesModel();
                    e.WebRoleCode = rs["WebRoleCode"].ToString();
                    e.WebRoleName = rs["WebRoleName"].ToString();
                    e.WebRoleDescription = rs["WebRoleDescription"].ToString();

                    ux = rtn.User.FindIndex(usr => usr.UserName == rs["UserName"].ToString());
                    if (ux > -1)
                    {
                        px = rtn.User[ux].Provider.FindIndex(pvd => pvd.ProvProviderID == rs["ProvProviderID"].ToString());
                        if (px > -1)
						{
                            rtn.User[ux].Provider[px].ProvRolesAssignedToAgent.Add(e);
						}
                    }

                }

                rs.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                r.ReturnStatusCode = "0200";
                r.ReturnStatus = "ERROR";
                r.ReturnStatusDescription = ex.Message;
                rtn.Result = r;
                return rtn;
            }


            // if we get here all is good
            r.ReturnStatusCode = "0000";
            r.ReturnStatus = "SUCCESS";
            r.ReturnStatusDescription = "Success - " + UserCount + " user(s) found";
            rtn.Result = r;
            return rtn;


        }


    }
}