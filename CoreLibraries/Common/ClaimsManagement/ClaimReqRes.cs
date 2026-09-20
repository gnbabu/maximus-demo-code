using MAXIMUS.Core.Libraries;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Corp.Core.Libraries.ClaimsManagement
{
    public abstract class ClaimReqRes
    {
        //Service Config data
        protected string url;
        protected string userName;
        protected string password;
        protected string claimsCertName;

        protected Logging log = null;
        protected Guid m_threadId;
        private Guid ThreadId
        {
            get
            {
                return this.m_threadId;
            }
            set
            {
                this.m_threadId = value;
            }
        }

        public ClaimReqRes()
        {
            ThreadId = Guid.NewGuid();
        }

        protected abstract void SetServieConfigData();

        protected string wssSecurityHeader(string userName, string password)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(m_threadId, logMsg);
            try
            {
                UsernameToken usernameTokenSection = new UsernameToken(userName, password, PasswordOption.SendPlainText);
                string xml = "<soapenv:Header>" +
                    @"<wsse:Security xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">" +
                        usernameTokenSection.GetXml(new XmlDocument()).OuterXml.ToString().Replace("<wsse:Nonce", "<wsse:Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\"") +
                    "</wsse:Security>" +
                  "</soapenv:Header>";
                return xml;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0}", ex.ToString()), Logging.LogPriority.Error);
                return null;
            }
        }
    }
}
