using Corp.Core.Libraries.AttachmentServiceReference;
using MAXIMUS.Core.Libraries;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using System.Xml;
using System.Xml.Serialization;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace Corp.Core.Libraries
{

    public class DocumentServiceAgent
    {
        public static string SendGenericAttachmentRequest(sendAttachmentRequest sendAttachment)
        {
            string url = AppSettings.Get("DocumentWSURL", Constants.WebServiceURI.DocumentService);
            string wsHost = AppSettings.Get("DocumentWSHost");
            string wsCertificateName = AppSettings.Get("DocumentWSCertificateName");
            string wsUserName = AppSettings.Get("DocumentWSUserName");
            string client_secret = "";
            string secretName = "DocumentWSPassword_OH_PNM_";
            try
            {
                var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                secretResult.Wait();

                client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
            }
            catch (Exception ex)
            {
                Logging log = new Logging(Guid.NewGuid(), "");
                client_secret = "NO SECRET CONNECTIVITY";
                client_secret += " ~|~ ";
                client_secret += AppSettings.Get("SecretsRegion");
                client_secret += " ~|~ ";
                client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                client_secret += " ~|~ ";
                client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                client_secret += " ~|~ ";
                client_secret += ex.Message;

                log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            }
            string wsPassword = client_secret;

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(sendAttachmentRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var streamRequest = new StringWriter();
            var writer = XmlWriter.Create(streamRequest, settings);
            x.Serialize(writer, sendAttachment, emptyNs);
            string xml = streamRequest.ToString();
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"  xmlns:att=\"http://mes.gov/attachment\">"
                   + Environment.NewLine + ServiceAgentHelper.SecurityHeader(wsUserName, wsPassword) + Environment.NewLine + "<soapenv:Body>" +
                   Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            //xmlDoc.DocumentElement.SetAttribute("xmlns:att", "http://ohio.gov/attachment");
            var root = xmlDoc.GetElementsByTagName("soapenv:Body")[0];
            SetPrefix("att", root.ChildNodes[0]);
            var documentxml = xmlDoc.InnerXml.ToString().Replace("sendAttachmentRequest>", "att:SendAttachment>")
                .Replace("MessageHeader>", "att:MessageHeader>")
                .Replace("Payload>", "att:Payload>")
                .Replace("xmlns=\"http://mes.gov/attachment\" xmlns:att=\"http://mes.gov/attachment\"", "")
                .Replace("xmlns=\"http://mes.gov/attachment\"", "");
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(documentxml);
            return ServiceAgentHelper.MakeServiceCallToSendAttachment(xmlDoc, url, "\"http://mes.gov/sendAttachment\"", wsUserName, wsPassword,
                wsCertificateName, wsHost, "Generic");
        }
        public static string SendAttachmentRequest(sendAttachmentRequest sendAttachment, string pnmTransactionKey)
        {
            string url = AppSettings.Get("DocumentWSURL", Constants.WebServiceURI.DocumentService);
            string wsHost = AppSettings.Get("DocumentWSHost");
            string wsCertificateName = AppSettings.Get("DocumentWSCertificateName");
            string wsUserName = AppSettings.Get("DocumentWSUserName");
            string client_secret = "";
            string secretName = "DocumentWSPassword_OH_PNM_";
            try
            {
                var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                secretResult.Wait();

                client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
            }
            catch (Exception ex)
            {
                Logging log = new Logging(Guid.NewGuid(), "");
                client_secret = "NO SECRET CONNECTIVITY";
                client_secret += " ~|~ ";
                client_secret += AppSettings.Get("SecretsRegion");
                client_secret += " ~|~ ";
                client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                client_secret += " ~|~ ";
                client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                client_secret += " ~|~ ";
                client_secret += ex.Message;

                log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            }
            string wsPassword = client_secret;

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(sendAttachmentRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var streamRequest = new StringWriter();
            var writer = XmlWriter.Create(streamRequest, settings);
            x.Serialize(writer, sendAttachment, emptyNs);
            string xml = streamRequest.ToString();
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"  xmlns:att=\"http://mes.gov/attachment\">"
                   + Environment.NewLine + ServiceAgentHelper.SecurityHeader(wsUserName, wsPassword) + Environment.NewLine + "<soapenv:Body>" +
                   Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            //xmlDoc.DocumentElement.SetAttribute("xmlns:att", "http://ohio.gov/attachment");
            var root = xmlDoc.GetElementsByTagName("soapenv:Body")[0];
            SetPrefix("att", root.ChildNodes[0]);
            var documentxml = xmlDoc.InnerXml.ToString().Replace("sendAttachmentRequest>", "att:SendAttachment>")
                .Replace("MessageHeader>", "att:MessageHeader>")
                .Replace("Payload>", "att:Payload>")
                .Replace("xmlns=\"http://mes.gov/attachment\" xmlns:att=\"http://mes.gov/attachment\"","")
                .Replace("xmlns=\"http://mes.gov/attachment\"", "");
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(documentxml);
            return ServiceAgentHelper.MakeServiceCall(xmlDoc, url, "\"http://mes.gov/sendAttachment\"", wsUserName, wsPassword,
                wsCertificateName, wsHost, pnmTransactionKey);
        }
        private static void SetPrefix(string prefix, XmlNode node)
        {
            node.Prefix = prefix;
            foreach (XmlNode n in node.ChildNodes)
            {
                SetPrefix(prefix, n);
            }
        }
        public static string SendAttachmentRequestNew(sendAttachmentRequest sendAttachment)
        {
             Guid ThreadId = Guid.NewGuid();
            string logHeader = string.Format("SendAttachment: Entering SendAttachmentRequestNew: ");
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(ThreadId, logMsg);

            string url = AppSettings.Get("DocumentWSURL", Constants.WebServiceURI.DocumentService);
            string wsCertificateName = AppSettings.Get("DocumentWSCertificateName");
            string wsUserName = AppSettings.Get("DocumentWSUserName");
            string client_secret = "";
            string secretName = "DocumentWSPassword_OH_PNM_";
            try
            {
                var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                secretResult.Wait();

                client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
            }
            catch (Exception ex)
            {
                client_secret = "NO SECRET CONNECTIVITY";
                client_secret += " ~|~ ";
                client_secret += AppSettings.Get("SecretsRegion");
                client_secret += " ~|~ ";
                client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                client_secret += " ~|~ ";
                client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                client_secret += " ~|~ ";
                client_secret += ex.Message;

                log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            }
            string wsPassword = client_secret;

            client_secret = "";
            secretName = "DocumentWSPassword_2_OH_PNM_";
            try
            {
                var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                secretResult.Wait();

                client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
            }
            catch (Exception ex)
            {
                client_secret = "NO SECRET CONNECTIVITY";
                client_secret += " ~|~ ";
                client_secret += AppSettings.Get("SecretsRegion");
                client_secret += " ~|~ ";
                client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                client_secret += " ~|~ ";
                client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                client_secret += " ~|~ ";
                client_secret += ex.Message;

                log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            }
            string wsPassword_2 = client_secret;

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(sendAttachmentRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, sendAttachment, emptyNs);
            string xml = stream2.ToString();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);


            return ServiceAgentHelper.MakeServiceCallNew(xmlDoc, url, CON.AttachmentServiceSoapAction.AttachmentService, wsUserName, wsCertificateName, wsPassword_2);
        }

    }
}
