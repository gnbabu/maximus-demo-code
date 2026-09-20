using MAXIMUS.Services.PDMS.StructureMap;
using StructureMap;
using System;

namespace MAXIMUS.Services.PDMS
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ObjectFactory.Initialize(x =>
            {
                x.UseDefaultStructureMapConfigFile = false;
                x.AddRegistry(new StructureMapRegistry());
                x.Scan(y => y.LookForRegistries());
            });

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}