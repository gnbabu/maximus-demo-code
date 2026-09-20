using MAXIMUS.Core.Libraries;
using System;
using System.Linq;
using Corp.Core.Libraries.IntelligentAddressVerification;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;
using Corp.Core.Libraries.Properties;
using System.Data;
using static MAXIMUS.Core.Libraries.Constants;
using System.Text.RegularExpressions;
using System.Web;

namespace Corp.Core.Libraries.ServiceAgent
{
    public class IntelligentSearchAgent
    {
        private readonly string _userName;
        private readonly string _password;
        private const string CompanyName = "Maximus";
        private Logging log = null;
        private Guid m_threadId;

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

        public IntelligentSearchAgent()
        {
            _userName = AppSettings.Get("IntelliSearchUserName", "");
            string client_secret = "";
            string secretName = "IntelliSearchPassword_OH_PNM_";
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
                log = new Logging(this.ThreadId);
                log.CreateLogEntry("Intelligent Search Address Verification: Invalid Security Credentials", Logging.LogPriority.Error);
                log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            }
            string wsPassword = client_secret;
            _password = client_secret;
            

            if (string.IsNullOrWhiteSpace(_userName) || string.IsNullOrWhiteSpace(_password) || _password.Contains("NO SECRET CONNECTIVITY"))
            {
                log = new Logging(this.ThreadId);
                log.CreateLogEntry("Intelligent Search Address Verification: Invalid Security Credentials", Logging.LogPriority.Error);
            }
        }
		
		public AddressVerificatonDetail GetAddressVerificaton(AddressVerificationRequest address, int addressTypeId, Guid username, string PageTitle, int RegId)
        {
            int eddrvID = 0;
            string opsAV = string.Empty;
            string isMatch = string.Empty;
            AddressVerificatonDetail ad = new AddressVerificatonDetail();

            try
            {
                DataTable dt = InfoAccessController.CheckAddressVerificationRequired(address, addressTypeId, log);
                opsAV = Convert.ToString(dt.Rows[0]["opsAV"]);
                isMatch = Convert.ToString(dt.Rows[0]["IsMatch"]);

                if (isMatch.Equals("true"))
                {
                    ad.AddressLine = Convert.ToString(dt.Rows[0]["DeliveryLine1"]);
                    ad.AddressLine2 = Convert.ToString(dt.Rows[0]["SecondaryDesignation"]) + ' ' + Convert.ToString(dt.Rows[0]["SecondaryNumber"]);
                    ad.City = Convert.ToString(dt.Rows[0]["City"]);
                    ad.State = Convert.ToString(dt.Rows[0]["State"]);
                    ad.PostalCode = Convert.ToString(dt.Rows[0]["ZipAddon"]);
                    ad.County = Convert.ToString(dt.Rows[0]["CountyName"]);
                    ad.CountyNumber = Convert.ToString(dt.Rows[0]["CountyNumber"]);
                    ad.ErrorCodes = Convert.ToString(dt.Rows[0]["ErrorCodes"]);
                    ad.ReturnCodes = Convert.ToString(dt.Rows[0]["ReturnCodes"]);
                    ad.StreetName = Convert.ToString(dt.Rows[0]["StreetName"]);
                    ad.StreetNumber = Convert.ToString(dt.Rows[0]["StreetNumber"]);
                    ad.StreetSuffix = Convert.ToString(dt.Rows[0]["StreetSuffix"]);
                    ad.PreDirectional = Convert.ToString(dt.Rows[0]["PreDirectional"]);
                    ad.SearchesLeft = Convert.ToInt32(dt.Rows[0]["SearchesLeft"]);
                    eddrvID = Convert.ToInt32(dt.Rows[0]["OUTBOUND_EADDRV_REQ_RES_ID"]);
                }
                else
                {
                    EnvelopeAddressVerificationServiceReq req = new EnvelopeAddressVerificationServiceReq();
                    req.username = _userName;
                    req.password = _password;
                    req.firmname = CompanyName;
                    req.urbanization = string.Empty;
                    req.delivery_line_1 = address.AddressLine;
                    req.delivery_line_2 = address.AddressLine2;
                    req.city_state_zip = address.City + " " + address.State + address.PostalCode;
                    req.ca_codes = string.Empty;
                    req.ca_filler = string.Empty;
                    req.batchname = string.Empty;

                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(EnvelopeAddressVerificationServiceReq));
                    var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    var settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.OmitXmlDeclaration = true;

                    var stream2 = new StringWriter();
                    var writer = XmlWriter.Create(stream2, settings);
                    x.Serialize(writer, req, emptyNs);
                    string xml = stream2.ToString();

                    XmlDocument xmlDocReq = new XmlDocument();
                    xmlDocReq.LoadXml(xml);


                    eddrvID = InfoAccessController.InsertUpdateAddressVerificationXMLRecord(xmlDocReq.InnerXml.ToString(), null, opsAV, 0, new Guid(CON.appAdminUserId),
                        DateTime.Now, 1, log, eddrvID, address.AddressLine, "", address.AddressLine2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        address.PostalCode);

                    var addressVerificationService = new IntelligentAddressVerification.CorrectAddressWebServiceSoapClient();

                    WsCorrectAddress result;
                    if (addressTypeId == 1 || addressTypeId == 26)
                    {
                        result = addressVerificationService.wsTigerCA(
                       _userName,
                       _password,
                       CompanyName,
                       string.Empty,
                       address.AddressLine,
                       address.AddressLine2,
                       address.City + " " + address.State + address.PostalCode,
                       string.Empty,
                       string.Empty,
                       string.Empty).FirstOrDefault();

                        ad.Longitude = result.GeofLong;
                        ad.Latitude = result.GeofLat;
                    }
                    else
                    {
                        result = addressVerificationService.wsCorrectA(
                       _userName,
                       _password,
                       CompanyName,
                       string.Empty,
                       address.AddressLine,
                       address.AddressLine2,
                       address.City + " " + address.State + address.PostalCode,
                       string.Empty,
                       string.Empty,
                       string.Empty).FirstOrDefault();
                    }

                    SoapResponseIS obj = new SoapResponseIS();

                    obj.AddressLine = ad.AddressLine = result.DeliveryLine1;
                    obj.AddressLine2 = ad.AddressLine2 = result.SecondaryDesignation + ' ' + result.SecondaryNumber;
                    obj.City = ad.City = result.City;
                    obj.State = ad.State = result.State;
                    obj.PostalCode = ad.PostalCode = result.ZipAddon;
                    obj.County = ad.County = result.CountyName;
                    obj.CountyNumber = ad.CountyNumber = result.CountyNumber;
                    obj.ErrorCodes = ad.ErrorCodes = result.ErrorCodes;
                    obj.ReturnCodes = ad.ReturnCodes = result.ReturnCodes;
                    obj.StreetName = ad.StreetName = result.StreetName;
                    obj.StreetNumber = ad.StreetNumber = result.StreetNumber;
                    obj.StreetSuffix = ad.StreetSuffix = result.StreetSuffix;
                    obj.PreDirectional = ad.PreDirectional = result.PreDirectional;
                    obj.SearchesLeft = ad.SearchesLeft = result.SearchesLeft;
                    obj.ErrorDesc = result.ErrorDesc;

                    System.Xml.Serialization.XmlSerializer x2 = new System.Xml.Serialization.XmlSerializer(typeof(SoapResponseIS));
                    var emptyNs2 = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    var settings2 = new XmlWriterSettings();
                    settings2.Indent = true;
                    settings2.OmitXmlDeclaration = true;

                    var stream3 = new StringWriter();
                    var writer2 = XmlWriter.Create(stream3, settings2);
                    x2.Serialize(writer2, obj, emptyNs2);
                    string responseXML = stream3.ToString();

                    XmlDocument xmlDocRes = new XmlDocument();
                    xmlDocRes.LoadXml(responseXML);

                    InfoAccessController.InsertUpdateAddressVerificationXMLRecord(null, xmlDocRes.InnerXml.ToString(), opsAV, ad.SearchesLeft, new Guid(CON.appAdminUserId),
                        DateTime.Now, 2, log, eddrvID, "", result.DeliveryLine1, "", result.DeliveryLine2, result.City, result.State, result.ZipAddon, result.CountyNumber,
                        result.CountyName, result.GeofLat, result.GeofLong, result.ErrorCodes, result.ReturnCodes, result.StreetName, result.StreetNumber, result.StreetSuffix,
                        result.PreDirectional, result.ErrorDesc, result.SecondaryDesignation, result.SecondaryNumber, "");
                }

                InfoAccessController.AddAddressVerificationLog(RegId,username, PageTitle, isMatch.Equals("true")? "DB" : "API", eddrvID);

                return ad;
            }
            catch (WebException ex2)
            {
                log = new Logging(this.ThreadId);
                log.CreateLogEntry("Intelligent Search Address Verification: Error " + ex2.Message + " " + ex2.StackTrace, Logging.LogPriority.Error);
                return null;
            }
            catch (Exception ex)
            {
                log = new Logging(this.ThreadId);
                log.CreateLogEntry("Intelligent Search Address Verification: Error " + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);
                return null;
            }
        }

        public AddressVerificatonDetail GetAddressVerificatonWS(AddressVerificationRequest address)
        {
            int eddrvID = 0;
            AddressVerificatonDetail ad = new AddressVerificatonDetail();
            string opsAV = string.Empty;
            string isMatch = string.Empty;

            try
            {
                DataTable dt = InfoAccessController.CheckAddressVerificationRequired(address, CON.AddressType.AddressFromJOB, log);
                opsAV = Convert.ToString(dt.Rows[0]["opsAV"]);
                isMatch = Convert.ToString(dt.Rows[0]["IsMatch"]);

                if (isMatch.Equals("true"))
                {
                    ad.AddressLine = Convert.ToString(dt.Rows[0]["DeliveryLine1"]);
                    ad.AddressLine2 = Convert.ToString(dt.Rows[0]["SecondaryDesignation"]) + ' ' + Convert.ToString(dt.Rows[0]["SecondaryNumber"]);
                    ad.City = Convert.ToString(dt.Rows[0]["City"]);
                    ad.State = Convert.ToString(dt.Rows[0]["State"]);
                    ad.PostalCode = Convert.ToString(dt.Rows[0]["ZipAddon"]);
                    ad.County = Convert.ToString(dt.Rows[0]["CountyName"]);
                    ad.CountyNumber = Convert.ToString(dt.Rows[0]["CountyNumber"]);
                    ad.ErrorCodes = Convert.ToString(dt.Rows[0]["ErrorCodes"]);
                    ad.ReturnCodes = Convert.ToString(dt.Rows[0]["ReturnCodes"]);
                    ad.StreetName = Convert.ToString(dt.Rows[0]["StreetName"]);
                    ad.StreetNumber = Convert.ToString(dt.Rows[0]["StreetNumber"]);
                    ad.StreetSuffix = Convert.ToString(dt.Rows[0]["StreetSuffix"]);
                    ad.PreDirectional = Convert.ToString(dt.Rows[0]["PreDirectional"]);
                    ad.SearchesLeft = Convert.ToInt32(dt.Rows[0]["SearchesLeft"]);
                }
                else
                {

                    string _urlI = AppSettings.Get("IntelliSearchWSURLOP", "https://www.intelligentsearch.com/CorrectAddressWS/CorrectAddressWebService.asmx?op=wsCorrectA");
                    string _hostI = AppSettings.Get("IntelliSearchWSHOST", "www.intelligentsearch.com");

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                   
                    EnvelopeAddressVerificationServiceReq result = new EnvelopeAddressVerificationServiceReq();
                    result.username = _userName;
                    result.password = _password;
                    result.firmname = CompanyName;
                    result.urbanization = string.Empty;
                    result.delivery_line_1 = address.AddressLine;
                    result.delivery_line_2 = address.AddressLine2;
                    result.city_state_zip = address.City + " " + address.State + address.PostalCode;
                    result.ca_codes = string.Empty;
                    result.ca_filler = string.Empty;
                    result.batchname = string.Empty;

                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(EnvelopeAddressVerificationServiceReq));
                    var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    var settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.OmitXmlDeclaration = true;

                    var stream2 = new StringWriter();
                    var writer = XmlWriter.Create(stream2, settings);
                    x.Serialize(writer, result, emptyNs);
                    string xml = stream2.ToString();

                    xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\">\r\n  <soap12:Body>" + xml;
                    xml = xml + "  </soap12:Body>\r\n</soap12:Envelope>";

                    XmlDocument xmlDocReq = new XmlDocument();
                    xmlDocReq.LoadXml(xml);
                    eddrvID = InfoAccessController.InsertUpdateAddressVerificationXMLRecord(xmlDocReq.InnerXml.ToString(), null, opsAV, 0, new Guid(CON.appAdminUserId),
                        DateTime.Now, 1, log, eddrvID, address.AddressLine, "", address.AddressLine2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        address.PostalCode);

                    Uri apiUrl = new Uri(_urlI);
                    WebRequest pmRequest = HttpWebRequest.Create(_urlI);
                    HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
                    byte[] bytes;
                    bytes = System.Text.Encoding.ASCII.GetBytes(xmlDocReq.InnerXml.ToString());
                    pmHttpRequest.ContentType = "application/soap+xml; charset=utf-8";
                    pmHttpRequest.KeepAlive = true;
                    pmHttpRequest.Method = "POST";
                    pmHttpRequest.SendChunked = true;
                    pmHttpRequest.UserAgent = ".NET Framework";
                    pmHttpRequest.Host = _hostI;

                    Stream requestStream = pmHttpRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                    string response = "";

                    using (WebResponse pmResponse = pmHttpRequest.GetResponse())
                    {
                        HttpWebResponse pmHttpResponse = (HttpWebResponse)pmResponse;

                        HttpStatusCode statuscode = pmHttpResponse.StatusCode;

                        using (Stream stream = pmResponse.GetResponseStream())
                        {
                            using (StreamReader sr = new StreamReader(stream))
                            {
                                response = sr.ReadToEnd();
                            }
                        }
                    }

                    var xDoc = XDocument.Parse(response);
                    var xLoginResult = xDoc.Root.Descendants().FirstOrDefault(d => d.Name.LocalName.Equals("WsCorrectAddress"));

                    object result2;
                    var serializer = new XmlSerializer(typeof(SoapResponseIS));
                    using (var reader = new StringReader(xLoginResult.ToString()))
                    {
                        result2 = serializer.Deserialize(reader);
                    }

                    SoapResponseIS obj = (SoapResponseIS)result2;
                    ad.AddressLine = obj.AddressLine;
                    ad.AddressLine2 = obj.AddressLine2;
                    ad.City = obj.City;
                    ad.State = obj.State;
                    ad.PostalCode = obj.PostalCode;
                    ad.ZipAddon = obj.ZipAddon;
                    ad.CountyName = obj.CountyName;
                    ad.CountyNumber = obj.CountyNumber;
                    ad.ErrorCodes = obj.ErrorCodes;
                    ad.ReturnCodes = obj.ReturnCodes;
                    ad.StreetName = obj.StreetName;
                    ad.StreetNumber = obj.StreetNumber;
                    ad.StreetSuffix = obj.StreetSuffix;
                    ad.PreDirectional = obj.PreDirectional;
                    ad.SearchesLeft = obj.SearchesLeft;
                    ad.ErrorCodes = obj.ErrorCodes;
                    ad.ErrorDesc = obj.ErrorDesc;
                    ad.SecondaryDesignation = obj.SecondaryDesignation;
                    ad.SecondaryNumber = obj.SecondaryNumber;



                    System.Xml.Serialization.XmlSerializer x2 = new System.Xml.Serialization.XmlSerializer(typeof(SoapResponseIS));
                    var emptyNs2 = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    var settings2 = new XmlWriterSettings();
                    settings2.Indent = true;
                    settings2.OmitXmlDeclaration = true;

                    var stream3 = new StringWriter();
                    var writer2 = XmlWriter.Create(stream3, settings2);
                    x2.Serialize(writer2, obj, emptyNs2);
                    string responseXML = stream3.ToString();

                    XmlDocument xmlDocRes = new XmlDocument();
                    xmlDocRes.LoadXml(responseXML);

                    InfoAccessController.InsertUpdateAddressVerificationXMLRecord(null, xmlDocRes.InnerXml.ToString(), opsAV, ad.SearchesLeft, new Guid(CON.appAdminUserId),
                        DateTime.Now, 2, log, eddrvID, "", obj.AddressLine, "", obj.AddressLine2, ad.City, obj.State, obj.ZipAddon, obj.CountyNumber,
                        obj.CountyName, "", "", obj.ErrorCodes, obj.ReturnCodes, obj.StreetName, obj.StreetNumber, obj.StreetSuffix, obj.PreDirectional, obj.ErrorDesc,
                        obj.SecondaryDesignation, obj.SecondaryNumber, "");

                }

                return ad;
            }
            catch (WebException ex2)
            {
                log = new Logging(this.ThreadId);
                log.CreateLogEntry("Intelligent Search Address Verification: Error " + ex2.Message + " " + ex2.StackTrace, Logging.LogPriority.Error);
                return null;
            }
            catch (Exception ex)
            {
                log = new Logging(this.ThreadId);
                log.CreateLogEntry("Intelligent Search Address Verification: Error " + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);
                return null;
            }
        }

    }

}
