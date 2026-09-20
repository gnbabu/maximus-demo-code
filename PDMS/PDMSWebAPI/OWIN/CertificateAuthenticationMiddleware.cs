using Microsoft.Owin;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PDMSWebAPI.OWIN
{
    public class CertificateAuthenticationMiddleware : OwinMiddleware
    {
        const string OwinCertFunc = "ssl.LoadClientCertAsync";
        const string OwinCert = "ssl.ClientCertificate";
        const string OwinCertError = "ssl.ClientCertificateErrors";
        public CertificateAuthenticationMiddleware(OwinMiddleware next) : base(next)
        { }

        ///<summary>  
        /// The Invoke() method is invocked from startup class of OWIN for security.  
        ///</summary>  
        ///<param name="context"></param>  
        ///<returns></returns>  

        public async override Task Invoke(IOwinContext context)
        {
            if (context.Environment.Keys.Contains(OwinCertFunc))
            {
                try
                {
                    var task = (context.Environment[OwinCertFunc] as Func<Task>);
                    await Task.Run(task);
                    if (context.Environment.Keys.Contains(OwinCert))
                    {
                        var cert = context.Environment[OwinCert] as X509Certificate;
                        if (cert != null) context.Request.Environment.Add("ClientInfo", cert.Subject);
                        else
                        {
                            context.Response.StatusCode = 403;
                            return;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 403;
                        return;
                    }
                    if (context.Environment.Keys.Contains(OwinCertError))
                    {
                        context.Response.StatusCode = 403;
                        return;
                    }
                }
                catch
                {
                    context.Response.StatusCode = 403;
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = 403;
                return;
            }
            await Next.Invoke(context);
        }
    }
}