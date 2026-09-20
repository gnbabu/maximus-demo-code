using MAXIMUS.Core.Libraries;
using System;
using System.Configuration;
using System.DirectoryServices.Protocols;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web.Security;
using System.Web;
using static MAXIMUS.Core.Libraries.Logging;


namespace PDMSWebAPI
{
    public class ServiceAuthenticator : UserNamePasswordValidator
    {
        Logging Log = new Logging(Guid.NewGuid(), "ServiceAuthenticator:Validate");
        public string MethodUsed = "";
        public override void Validate(string userName, string password)
        {
            Log.CreateLogEntry("Validating user " + userName, LogPriority.Information);

            // if (System.Diagnostics.Debugger.IsAttached) { return; }  // dont validate service accounts in local dev

            // validate arguments
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            // check cache for service account auth
                        
            var px = HttpContext.Current.Cache["PDMSAuthCache_" + userName];

            if (px is null) { px = "NOT_SET"; }
            
            if (px.ToString() != password)  // password is null or does not match - validate via ldap (will be null when cache expires)
			{
                if (ValidateUserByConfiguredMethod(userName, password)) 
                {
                    // cache for X minutes
                    var minToCache = ConfigurationManager.AppSettings["CacheLdapForMinutes"];
                    var cTime = DateTime.Now.AddMinutes(double.Parse(minToCache));
                    var cExp = System.Web.Caching.Cache.NoSlidingExpiration;
                    var cPri = System.Web.Caching.CacheItemPriority.Normal;
                    HttpContext.Current.Cache.Insert("PDMSAuthCache_" + userName, password, null, cTime, cExp, cPri, null);
                } 
                else 
                {
                    Log.CreateLogEntry(String.Format("Invalid user credentials for {0} ({1})", userName, this.MethodUsed), LogPriority.Error);
                    throw new AccessViolationException(String.Format("Invalid user credentials for  {0} ({1})", userName, this.MethodUsed));
                }
            }

            // get user to check roles (check cache first)
            var uc = HttpContext.Current.Cache["PDMSUserCache_" + userName] as PDMSPrincipal;

            if (uc is null)
			{
                var user = Membership.GetUser(userName);
                if (user == null)
                {
                    Log.CreateLogEntry(string.Format("User {0} not found", userName), LogPriority.Error);
                    throw new ArgumentException("User not found");
                }

                SetCurrentUser(user);

                // cache for X minutes
                var minToCache = ConfigurationManager.AppSettings["CacheLdapForMinutes"];
                var cTime = DateTime.Now.AddMinutes(double.Parse(minToCache));
                var cExp = System.Web.Caching.Cache.NoSlidingExpiration;
                var cPri = System.Web.Caching.CacheItemPriority.Normal;
                HttpContext.Current.Cache.Insert("PDMSUserCache_" + userName, PDMSPrincipal.GetCurrentUser(), null, cTime, cExp, cPri, null);

            } else  // user in cache, skip db lookup
			{
                PDMSPrincipal.SetCurrentUser(uc);
            }

        }

        public bool ValidateFromBasicAuthHeader(string BasicAuthHeader)
        {
            Logging Log = new Logging(Guid.NewGuid(), "ServiceAuthenticator:Validate");

            string authToken = BasicAuthHeader.Replace("Basic ", "");
            string userName = "";
            string password = "";

            // decoding authToken we get decode value in 'Username:Password' format  
            var decodeauthToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

            // spliting decodeauthToken using ':'   
            var arrUserNameandPassword = decodeauthToken.Split(':');
            userName = arrUserNameandPassword[0];
            password = arrUserNameandPassword[1];

            try
            {
                this.Validate(userName, password);
            }
            catch (Exception ex)
            {

                // Authenicate the user credentials via ADFS here

                Log.CreateLogEntry(String.Format("Invalid user credentials for {0}", arrUserNameandPassword[0]), 305);

                return false;

            }

            // if we get here all is good
            return true;

        }

        private bool ValidateUserByConfiguredMethod(string userName, string password)
        {            

            var bypassLdap = ConfigurationManager.AppSettings["BypassLdap"];
            if (String.IsNullOrEmpty(bypassLdap)) { bypassLdap = "NO"; }

            if (bypassLdap == "YES")
            {
                this.MethodUsed = "DB";
                if (System.Diagnostics.Debugger.IsAttached) { return true; }  // dont validate service accounts in local dev
                return MembershipValidate(userName, password);
            } else
			{
                this.MethodUsed = "LDAP";
                return LdapValidate(userName, password);
            }

        }

        private bool MembershipValidate(string userName, string password)
        {
            var validated = Membership.ValidateUser(userName, password);
            if (validated)
            {
                Log.CreateLogEntry(String.Format("User {0} validated via ASP.Net Membership", userName), LogPriority.Information);
            }
            else
            {
                Log.CreateLogEntry(String.Format("User {0} validation failed via ASP.Net Membership", userName), LogPriority.Error);
            }
            return validated;
        }

        private bool LdapValidate(string userName, string password)
        {
            try
            {
                var server = ConfigurationManager.AppSettings["LdapServer"]?.ToString();
                var port = ConfigurationManager.AppSettings["LdapPort"]?.ToString();

                LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(server, int.Parse(port)));
                con.SessionOptions.SecureSocketLayer = true;
                con.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback(ServerCallback);
                con.AuthType = AuthType.Negotiate;
                con.SessionOptions.ProtocolVersion = 3;
                con.Credential = new NetworkCredential(userName, password);
                con.Bind();

                Log.CreateLogEntry(String.Format("User {0} validated via LDAP", userName), LogPriority.Information);
                return true;
            }
            catch (Exception ex)
            {
                Log.CreateLogEntry(String.Format("LDAP validation failure for {0}: {1}", userName, ex.Message), LogPriority.Error);
                return false;
            }
        }

        private void SetCurrentUser(MembershipUser user)
        {
            // Look up roles in PDMS memberships
            var roles = Roles.GetRolesForUser(user.UserName);
            string rolesList = string.Join(";", roles);
            Log.CreateLogEntry(string.Format("User {0} roles: {1}", user.UserName, rolesList), LogPriority.Information);

            var principal = new PDMSPrincipal(user, roles);
            principal.UserThreadId = Log.ThreadId;

            PDMSPrincipal.SetCurrentUser(principal);
        }


        private bool ServerCallback(LdapConnection connection, X509Certificate certificate)
        {
            return true;
        }

    }
}