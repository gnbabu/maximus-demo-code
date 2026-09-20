using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace PDMSWebAPI
{
    public class PDMSPrincipal : IPrincipal
    {
        private string[] _roles;

        public PDMSPrincipal(MembershipUser user, string[] roles)
        {
            Identity = new PDMSIdentity(user);
            _roles = roles;
        }

        private Guid userThreadId;
        public Guid UserThreadId
        {
            get
            {
                if (userThreadId == null)
                {
                    userThreadId = Guid.NewGuid();
                }
                return userThreadId;
            }
            set
            {
                userThreadId = value;
            }
        }

        public IIdentity Identity { get; internal set; }

        public bool IsInRole(string role)
        {
            return _roles.Contains(role);
        }

        public static PDMSPrincipal SetCurrentUser(PDMSPrincipal user)
        {
            var cTime = DateTime.Now.AddMinutes(11);
            var cExp = System.Web.Caching.Cache.NoSlidingExpiration;
            var cPri = System.Web.Caching.CacheItemPriority.Normal;

            HttpContext.Current.Cache.Insert("User", user, null, cTime, cExp, cPri, null);

            return user;
        }

        public static PDMSPrincipal GetCurrentUser()
        {
            var user = HttpContext.Current.Cache["User"] as PDMSPrincipal;
            return user;
        }


        private class PDMSIdentity : IIdentity
        {
            private MembershipUser user;

            public PDMSIdentity(MembershipUser user)
            {
                this.user = user;
            }

            public string Name => user.UserName;

            public string AuthenticationType => "Custom";

            public bool IsAuthenticated => true;
        }
    }
}