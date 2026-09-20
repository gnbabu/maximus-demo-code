using MAXIMUS.Core.Libraries;
using System;
using System.Security;
using System.Web.Security;

namespace PDMSWebAPI
{
    public static class RolesAuthentication
    {
        public static void Allow(params string[] roles)
        {

            if (System.Diagnostics.Debugger.IsAttached) { return; }  // dont validate service accounts in local dev

            var user = PDMSPrincipal.GetCurrentUser();
            if (user != null)
            {
                foreach (var role in roles)
                {
                    if (user.IsInRole(role))
                    {
                        return;
                    }
                }
            }

            var threadId = user == null ? Guid.NewGuid() : user.UserThreadId;
            Logging Log = new Logging(threadId, "PDMSWebAPI:RolesAuthentication");
            Log.CreateLogEntry(string.Format("User {0} does not have required roles for method.", user?.Identity?.Name), Logging.LogPriority.Error);
            throw new SecurityException("User does not have suffient role permission");
        }

        public static void AddRoles(params string[] roles)
        {
            foreach (var role in roles)
            {
                if (!Roles.RoleExists(role))
                {
                    Roles.CreateRole(role);
                }
            }
        }
    }
}