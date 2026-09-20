using MAXIMUS.Core.Libraries;
using System.Web.Security;

namespace MAXIMUS.Services.PDMS.MembershipProvider
{
    public class CustomRoleProvider : SqlRoleProvider
    {
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            //var connectionString = config.Get("connectionStringName");
            config.Add("connectionString", AppSettings.GetConnectionString());

            base.Initialize(name, config);
        }
    }
}