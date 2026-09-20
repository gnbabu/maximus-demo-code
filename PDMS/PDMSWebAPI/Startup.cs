using MAXIMUS.Core.Libraries;
using Microsoft.Owin;
using Owin;
using PDMSWebAPI.OWIN;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

[assembly: OwinStartup(typeof(PDMSWebAPI.Startup))]

namespace PDMSWebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureLogging();
            ConfigureAuth(app);

            var ipConfig = AppSettings.Get("PDMSWebAPI_IPWhitelist");
            if (!String.IsNullOrEmpty(ipConfig))
            {
                string[] IPList = ipConfig.Split(',').Select(s => s.ToString()).ToArray();
                var whitelistIps = new HashSet<string>(IPList);
                app.Use(typeof(IpWhiteListMiddleware), whitelistIps);
            }

            var certReq = ConfigurationManager.AppSettings["RequireCert"];
            if (!String.IsNullOrEmpty(certReq) && certReq == "true")
            {
                app.Use(typeof(CertificateAuthenticationMiddleware));
            }
        }

        private void ConfigureLogging()
        {
            // Create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature,
                MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(Guid.NewGuid(), logMsg);
            log.CreateLogEntry("Starting PDMS Web API", Logging.LogPriority.Information);
        }
    }
}
