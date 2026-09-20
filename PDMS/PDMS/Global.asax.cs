using PDMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace PDMS
{
    public class Global : HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Get the error details
            HttpException lastErrorWrapper =
                Server.GetLastError() as HttpException;

            if (lastErrorWrapper != null)
            {
                Exception lastError = lastErrorWrapper;
                if (lastErrorWrapper.InnerException != null)
                    lastError = lastErrorWrapper.InnerException;

                HttpContext.Current.Session["LastError"] = lastError;
            }

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            // Bug fix for MS SSRS Blank.gif 500 server error missing parameter IterationId
            // https://connect.microsoft.com/VisualStudio/feedback/details/556989/
            if (HttpContext.Current.Request.Url.PathAndQuery.Contains("/Reserved.ReportViewerWebControl.axd") &&
                !String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ResourceStreamID"]) &&
                String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["IterationId"]) &&
                HttpContext.Current.Request.QueryString["ResourceStreamID"].ToLower().Equals("blank.gif"))
            {
                Context.RewritePath(String.Concat(HttpContext.Current.Request.Url.PathAndQuery, "&IterationId=0"));
            }
        }
    }
}