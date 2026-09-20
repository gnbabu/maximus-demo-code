using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace Corp.Core.Libraries.Helper
{

    public static class NavigationHelper
    {
        public static (string Title, string Url) GetHome()
        {
            var user = HttpContext.Current.User.Identity.Name;

            // Committee roles → CommitteeQueue
            if (Roles.IsUserInRole(user, CON.UserRole.CredentialingChair) ||
                Roles.IsUserInRole(user, CON.UserRole.CommitteeQualitySpecialist))
            {
                return ("Home", Resolve("~/MesCred/CommitteeQueue.aspx"));
            }

            // CVO roles → CVOQueue
            if (Roles.IsUserInRole(user, CON.UserRole.ODMCredentialing) ||
                Roles.IsUserInRole(user, CON.UserRole.ODMCredentialingSpecialist) ||
                Roles.IsUserInRole(user, CON.UserRole.ODMCredentialingSupervisor) ||
                Roles.IsUserInRole(user, CON.UserRole.CredentialingSupervisor) ||
                Roles.IsUserInRole(user, CON.UserRole.CredentialingQuality) ||
                Roles.IsUserInRole(user, CON.UserRole.CredentialingSpecialist))
            {
                return ("Home", Resolve("~/MesCred/CVOQueue.aspx"));
            }

            // Default → WorkBench
            return ("Home", Resolve("~/MesCred/CvoWorkBench.aspx"));
        }

        private static string Resolve(string url)
        {
            return VirtualPathUtility.ToAbsolute(url);
        }
    }

}
