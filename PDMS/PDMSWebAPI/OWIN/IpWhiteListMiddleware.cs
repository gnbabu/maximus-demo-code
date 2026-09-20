using Microsoft.Owin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDMSWebAPI.OWIN
{
    public class IpWhiteListMiddleware : OwinMiddleware
    {
        private readonly HashSet<string> _whitelistIps;
        public IpWhiteListMiddleware(OwinMiddleware next, HashSet<string> whitelistIps) : base(next)
        {
            _whitelistIps = whitelistIps;
        }

        ///<summary>  
        /// The Invoke() method is invocked from startup class of OWIN for security.  
        ///</summary>  
        ///<param name="context"></param>  
        ///<returns></returns>  

        public async override Task Invoke(IOwinContext context)
        {
            if (_whitelistIps != null && 
                _whitelistIps.Count > 0 && 
                !_whitelistIps.Contains(context.Request.RemoteIpAddress) && 
                !IsLocal(context.Request))
            {
                //context.Response.StatusCode = 404;  

                var response = context.Response;
                var request = context.Request;
                response.OnSendingHeaders(state =>
                {
                    var resp = (OwinResponse)state;
                    resp.StatusCode = 200;
                    resp.ReasonPhrase = "IP address is not registered"; // if you're going to change the status code  
                                                                        // you probably should also change the reason phrase  

                }, response);
                return;
            }
            await Next.Invoke(context);
        }

        private bool IsLocal(IOwinRequest request)
        {
            return request.Host.Value.StartsWith("localhost");
        }
    }
}