using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using PdmsWebEvents;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace MAXIMUS.Services.PDMS.MembershipProvider
{
    public class CustomSqlMembershipProvider : SqlMembershipProvider
    {
        private bool enablePasswordHistoryCheck;
        public bool EnablePasswordHistoryCheck
        {
            get
            {
                return enablePasswordHistoryCheck;
            }
        }

        private string GetConfigValue(string configValue, string defaultValue)  
        {  
            if (String.IsNullOrEmpty(configValue))  
            {  
                return defaultValue;  
            }  

            return configValue;  
        }  

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            config.Add("connectionString", AppSettings.GetConnectionString());

            enablePasswordHistoryCheck = Convert.ToBoolean(GetConfigValue(config["enablePasswordHistoryCheck"], "true"));
            config.Remove("enablePasswordHistoryCheck");
            NameValueCollection membershipParams = new NameValueCollection(config);
            membershipParams.Remove("enablePasswordHistoryCheck");

            base.Initialize(name, membershipParams);
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            bool IsChanged = false;
            string pass = newPassword;
            MembershipUser user = Membership.GetUser(username);
            Guid userid = new Guid(user.ProviderUserKey.ToString());

            DataSet dsUser = UserController.SelectUserAccountInformation(user.ProviderUserKey.ToString());
            bool isForcePswdReset = false;
            if(ObjectControllerHelper.HasRows(dsUser))
            {
                DataRow drUser = dsUser.Tables[0].Rows[0];
                isForcePswdReset = !ObjectControllerHelper.GetBool("IS_OHID", drUser) && ObjectControllerHelper.GetBool("FORCE_PASSWORD_RESET", drUser) &&
                            !ObjectControllerHelper.GetBool("IS_TEMP_PWD_SENT", drUser);
            }
           
            if (!HttpContext.Current.User.IsInRole(MAXIMUS.Core.Libraries.Constants.UserRoleType.Administrator))
            {
                try
                {
                    if (!isForcePswdReset)
                    {
                        if (!IsOldPasswordMatches(username, oldPassword))
                        {
                            throw new PasswordException("PasswordMatchFailed");
                        }
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (EnablePasswordHistoryCheck)
            {
                if (!CheckPasswordHistory(username, pass))
                {
                    throw new PasswordException("PasswordExists");
                }
            }
            if (isForcePswdReset)
            {
                UserController.SetForcePasswordReset(userid, true, true);
                user.ChangePassword(user.ResetPassword(), newPassword);
                IsChanged = true;
            }
            else
                IsChanged = base.ChangePassword(username, oldPassword, newPassword);
            //save to password history

            if (IsChanged)
            {
                PasswordChangedEvent pcEvt = new PasswordChangedEvent("Password changed", this);
                pcEvt.Raise();
            }

            //MembershipUser user = Membership.GetUser(username);
            if (user != null)
            {
                //Guid userid = new Guid(user.ProviderUserKey.ToString());
                Guid loggedInUserId = userid;
                UserController.CopyPasswordToPwdHistory(userid, loggedInUserId);
            }

            return IsChanged;

        }
        



        private bool CheckPasswordHistory(string username, string pass)
        {
            MembershipUser muser = Membership.GetUser(username);
            Guid UsergUID = new Guid(muser.ProviderUserKey.ToString());
            int months = Convert.ToInt32(AppSettings.Get("PasswordMonths"));
            DataSet exists = UserController.PasswordExistsInPast(months, UsergUID);
            if (muser != null && exists != null && exists.Tables["PASSHISTORY"] != null)
            {
                DataTable dt = exists.Tables["PASSHISTORY"];
                foreach (DataRow row in dt.Rows)
                {

                    if (encryptSHA1(pass, row["Password_Salt"].ToString()) == row["Password"].ToString())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private string encryptSHA1(string pass, string salt)
        {
            var bytes = Encoding.Unicode.GetBytes(pass);
            var src = Convert.FromBase64String(salt);
            var dst = new byte[src.Length + bytes.Length];

            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            var algorithm = HashAlgorithm.Create(Membership.HashAlgorithmType);
            var inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }

        public bool IsOldPasswordMatches(string userName, string oldPassword)
        {
            bool pwdMatches = false;
            try
            {
                DataSet ds = UserController.GetUsermembershipInfoByUserName(userName);
                if (ObjectControllerHelper.HasRows(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    if (encryptSHA1(oldPassword, dr["passwordsalt"].ToString()) == dr["password"].ToString())
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return pwdMatches;
        }      
        
    }
    public class PasswordException : ApplicationException
    {
        public PasswordException(string message)
            : base(message)
        {
        }
    }
                
     
        
}
