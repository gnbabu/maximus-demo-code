using Corp.Core.Libraries.HospiceReference;
using MAXIMUS.Core.Libraries;
using System;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Corp.Core.Libraries
{
    public class HospiceServiceAgent
    {
        public static string SearchHospiceResponse(SearchHospiceRequest searchRequest, string pnmTransactionKey)
        {
            string url = AppSettings.Get("HospiceWSURL", Constants.WebServiceURI.HospiceService);
            //string url = "http://localhost:8095/HospiceSoapVS";
            string wsHost = AppSettings.Get("HospiceWSHost");
            string wsCertificateName = AppSettings.Get("HospiceWSCertificateName");
            string wsUserName = AppSettings.Get("HospiceWSUserName");
            string client_secret = "";
            string secretName = "HospiceWSPassword_OH_PNM_";

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
                Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
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
            secretName = "HospiceWSPassword_2_OH_PNM_";
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
                Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
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

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(SearchHospiceRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var streamRequest = new StringWriter();
            var writer = XmlWriter.Create(streamRequest, settings);
            x.Serialize(writer, searchRequest, emptyNs);
            string xml = streamRequest.ToString();
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                   + Environment.NewLine + ServiceAgentHelper.SecurityHeader(wsUserName, wsPassword) + Environment.NewLine + "<soapenv:Body>" +
                   Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:sear", "http://ohio.gov/SearchRequest");
            var root = xmlDoc.GetElementsByTagName("soapenv:Body")[0];
            SetPrefix("sear", root.ChildNodes[0]);
            var hospicexml = xmlDoc.InnerXml.ToString().Replace("SearchRequest>", "sear:SearchRequest>")
                .Replace("MessageHeader", "sear:MessageHeader")
                .Replace("Payload", "sear:Payload").Replace("<HospiceTrackNo />", "").Replace("<RecipID />", "");
            //hospicexml= hospicexml.ToString().Replace("<HospiceTrackNo />", "");
            hospicexml = RemoveAllXmlNamespace(hospicexml);
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(hospicexml);
            return ServiceAgentHelper.MakeServiceCall(xmlDoc, url, @"http://service.caremgmt.fi/HospiceService/SearchHospice", wsUserName, wsPassword_2, wsCertificateName, wsHost, pnmTransactionKey);
        }
        public static string RemoveAllXmlNamespace(string xmlData)
        {
            string xmlnsPattern = "\\s+xmlns\\s*(:\\w)?\\s*=\\s*\\\"(?<url>[^\\\"]*)\\\"";
            MatchCollection matchCol = Regex.Matches(xmlData, xmlnsPattern);

            foreach (Match m in matchCol)
            {
                xmlData = xmlData.Replace(m.ToString(), "");
            }
            return xmlData;
        }
        private static void SetPrefix(string prefix, XmlNode node)
        {
            node.Prefix = prefix;
            foreach (XmlNode n in node.ChildNodes)
            {
                SetPrefix(prefix, n);
            }
        }
        public static string HospiceInquireRequest(InquireHospiceRequest inquireRequest, string pnmTransactionKey)
        {
            string url = AppSettings.Get("HospiceWSURL", Constants.WebServiceURI.HospiceService);
            //string url = "http://localhost:8095/HospiceSoapVS";
            string wsHost = AppSettings.Get("HospiceWSHost");
            string wsCertificateName = AppSettings.Get("HospiceWSCertificateName");
            string wsUserName = AppSettings.Get("HospiceWSUserName");
            string client_secret = "";
            string secretName = "HospiceWSPassword_OH_PNM_";
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
                Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
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
            secretName = "HospiceWSPassword_2_OH_PNM_";
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
                Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
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


            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(InquireHospiceRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, inquireRequest, emptyNs);
            string xml = stream2.ToString();

            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                 + Environment.NewLine + ServiceAgentHelper.SecurityHeader(wsUserName, wsPassword) + Environment.NewLine + "<soapenv:Body>" +
                 Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:inq", "http://ohio.gov/InquireRequest");
            var root = xmlDoc.GetElementsByTagName("soapenv:Body")[0];
            SetPrefix("inq", root.ChildNodes[0]);
            var hospicexml = xmlDoc.InnerXml.ToString().Replace("InquireRequest>", "inq:InquireRequest>")
                .Replace("MessageHeader", "inq:MessageHeader")
                .Replace("Payload", "inq:Payload");
            hospicexml = RemoveAllXmlNamespace(hospicexml);
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(hospicexml);
            return ServiceAgentHelper.MakeServiceCall(xmlDoc, url, @"http://service.caremgmt.fi/HospiceService/InquireHospice", wsUserName, wsPassword_2, wsCertificateName, wsHost, pnmTransactionKey);
        }
        public static string AddUpdateHospice(AddUpdateHospiceRequest hospiceRequestResponse, string pnmTransactionKey)
        {
            string url = AppSettings.Get("HospiceWSURL", Constants.WebServiceURI.HospiceService);
            //string url = "http://localhost:8095/HospiceSoapVS";
            string wsCertificateName = AppSettings.Get("HospiceWSCertificateName");
            string wsHost = AppSettings.Get("HospiceWSHost");
            string wsUserName = AppSettings.Get("HospiceWSUserName");
            string client_secret = "";
            string secretName = "HospiceWSPassword_OH_PNM_";
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
                Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
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
            secretName = "HospiceWSPassword_2_OH_PNM_";
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
                Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
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


            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(AddUpdateHospiceRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, hospiceRequestResponse, emptyNs);
            string xml = stream2.ToString();
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                + Environment.NewLine + ServiceAgentHelper.SecurityHeader(wsUserName, wsPassword) + Environment.NewLine + "<soapenv:Body>" +
                Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:hos", "http://ohio.gov/HospiceRequestResponse");
            var root = xmlDoc.GetElementsByTagName("soapenv:Body")[0];
            SetPrefix("hos", root.ChildNodes[0]);
            var hospicexml = xmlDoc.InnerXml.ToString().Replace("HospiceRequestResponse>", "hos:HospiceRequestResponse>")
                .Replace("MessageHeader", "hos:MessageHeader")
                .Replace("Payload", "hos:Payload").Replace("<hos:TermDiag2 />", "").Replace("<hos:TermDiag3 />", "").Replace("<hos:DisenrollDate />", "").Replace("<hos:BenUpdateReason />", "").Replace("<hos:IDGPhyWritCertDate>0001-01-01</hos:IDGPhyWritCertDate>", "").Replace("<hos:PhyOralCertDate>0001-01-01</hos:PhyOralCertDate>", "");
            hospicexml = RemoveAllXmlNamespace(hospicexml);
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(hospicexml);
            return ServiceAgentHelper.MakeServiceCall(xmlDoc, url, @"http://service.caremgmt.fi/HospiceService/AddUpdateHospice", wsUserName, wsPassword_2, wsCertificateName, wsHost, pnmTransactionKey);
        }
    }
}