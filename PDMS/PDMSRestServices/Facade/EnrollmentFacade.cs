using Corp.Core.Libraries;
using Corp.Core.Libraries.IntelligentAddressVerification;
using Corp.Core.Libraries.ServiceAgent;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using MAXIMUS.Services.PDMS;
using Newtonsoft.Json;
using PDMSRestServices.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Authentication;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static MAXIMUS.Core.Libraries.Constants;
using CON = MAXIMUS.Core.Libraries.Constants;
using Taxonomy = MAXIMUS.Core.Libraries.Taxonomy;

namespace PDMSRestServices.Facade
{
    public class EnrollmentFacade
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
        public EnrollmentFacade()
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

        //public AddressVerificatonDetail GetAddressVerificaton(AddressVerificationRequest address, int addressTypeId)
        //{
        //    int eddrvID = 0;
        //    string opsAV = string.Empty;
        //    string isMatch = string.Empty;
        //    AddressVerificatonDetail ad = new AddressVerificatonDetail();

        //    try
        //    {
        //        DataTable dt = InfoAccessController.CheckAddressVerificationRequired(address, addressTypeId, log);
        //        opsAV = Convert.ToString(dt.Rows[0]["opsAV"]);
        //        isMatch = Convert.ToString(dt.Rows[0]["IsMatch"]);

        //        if (isMatch.Equals("true"))
        //        {
        //            ad.AddressLine = Convert.ToString(dt.Rows[0]["DeliveryLine1"]);
        //            ad.AddressLine2 = Convert.ToString(dt.Rows[0]["SecondaryDesignation"]) + ' ' + Convert.ToString(dt.Rows[0]["SecondaryNumber"]);
        //            ad.City = Convert.ToString(dt.Rows[0]["City"]);
        //            ad.State = Convert.ToString(dt.Rows[0]["State"]);
        //            ad.PostalCode = Convert.ToString(dt.Rows[0]["ZipAddon"]);
        //            ad.County = Convert.ToString(dt.Rows[0]["CountyName"]);
        //            ad.CountyNumber = Convert.ToString(dt.Rows[0]["CountyNumber"]);
        //            ad.ErrorCodes = Convert.ToString(dt.Rows[0]["ErrorCodes"]);
        //            ad.ReturnCodes = Convert.ToString(dt.Rows[0]["ReturnCodes"]);
        //            ad.StreetName = Convert.ToString(dt.Rows[0]["StreetName"]);
        //            ad.StreetNumber = Convert.ToString(dt.Rows[0]["StreetNumber"]);
        //            ad.StreetSuffix = Convert.ToString(dt.Rows[0]["StreetSuffix"]);
        //            ad.PreDirectional = Convert.ToString(dt.Rows[0]["PreDirectional"]);
        //            ad.SearchesLeft = Convert.ToInt32(dt.Rows[0]["SearchesLeft"]);
        //        }
        //        else
        //        {
        //            EnvelopeAddressVerificationServiceReq req = new EnvelopeAddressVerificationServiceReq();
        //            req.username = _userName;
        //            req.password = _password;
        //            req.firmname = CompanyName;
        //            req.urbanization = string.Empty;
        //            req.delivery_line_1 = address.AddressLine;
        //            req.delivery_line_2 = address.AddressLine2;
        //            req.city_state_zip = address.City + " " + address.State + address.PostalCode;
        //            req.ca_codes = string.Empty;
        //            req.ca_filler = string.Empty;
        //            req.batchname = string.Empty;

        //            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(EnvelopeAddressVerificationServiceReq));
        //            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        //            var settings = new XmlWriterSettings();
        //            settings.Indent = true;
        //            settings.OmitXmlDeclaration = true;

        //            var stream2 = new StringWriter();
        //            var writer = XmlWriter.Create(stream2, settings);
        //            x.Serialize(writer, req, emptyNs);
        //            string xml = stream2.ToString();

        //            XmlDocument xmlDocReq = new XmlDocument();
        //            xmlDocReq.LoadXml(xml);


        //            eddrvID = InfoAccessController.InsertUpdateAddressVerificationXMLRecord(xmlDocReq.InnerXml.ToString(), null, opsAV, 0, new Guid(CON.appAdminUserId),
        //                DateTime.Now, 1, log, eddrvID, address.AddressLine, "", address.AddressLine2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
        //                address.PostalCode);

        //            var addressVerificationService = new Corp.Core.Libraries.IntelligentAddressVerification.CorrectAddressWebServiceSoapClient();

        //            WsCorrectAddress result;
        //            if (addressTypeId == 1 || addressTypeId == 26)
        //            {
        //                result = addressVerificationService.wsTigerCA(
        //               _userName,
        //               _password,
        //               CompanyName,
        //               string.Empty,
        //               address.AddressLine,
        //               address.AddressLine2,
        //               address.City + " " + address.State + address.PostalCode,
        //               string.Empty,
        //               string.Empty,
        //               string.Empty).FirstOrDefault();

        //                ad.Longitude = result.GeofLong;
        //                ad.Latitude = result.GeofLat;
        //            }
        //            else
        //            {
        //                result = addressVerificationService.wsCorrectA(
        //               _userName,
        //               _password,
        //               CompanyName,
        //               string.Empty,
        //               address.AddressLine,
        //               address.AddressLine2,
        //               address.City + " " + address.State + address.PostalCode,
        //               string.Empty,
        //               string.Empty,
        //               string.Empty).FirstOrDefault();
        //            }

        //            SoapResponseIS obj = new SoapResponseIS();

        //            obj.AddressLine = ad.AddressLine = result.DeliveryLine1;
        //            obj.AddressLine2 = ad.AddressLine2 = result.SecondaryDesignation + ' ' + result.SecondaryNumber;
        //            obj.City = ad.City = result.City;
        //            obj.State = ad.State = result.State;
        //            obj.PostalCode = ad.PostalCode = result.ZipAddon;
        //            obj.County = ad.County = result.CountyName;
        //            obj.CountyNumber = ad.CountyNumber = result.CountyNumber;
        //            obj.ErrorCodes = ad.ErrorCodes = result.ErrorCodes;
        //            obj.ReturnCodes = ad.ReturnCodes = result.ReturnCodes;
        //            obj.StreetName = ad.StreetName = result.StreetName;
        //            obj.StreetNumber = ad.StreetNumber = result.StreetNumber;
        //            obj.StreetSuffix = ad.StreetSuffix = result.StreetSuffix;
        //            obj.PreDirectional = ad.PreDirectional = result.PreDirectional;
        //            obj.SearchesLeft = ad.SearchesLeft = result.SearchesLeft;
        //            obj.ErrorDesc = result.ErrorDesc;

        //            System.Xml.Serialization.XmlSerializer x2 = new System.Xml.Serialization.XmlSerializer(typeof(SoapResponseIS));
        //            var emptyNs2 = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        //            var settings2 = new XmlWriterSettings();
        //            settings2.Indent = true;
        //            settings2.OmitXmlDeclaration = true;

        //            var stream3 = new StringWriter();
        //            var writer2 = XmlWriter.Create(stream3, settings2);
        //            x2.Serialize(writer2, obj, emptyNs2);
        //            string responseXML = stream3.ToString();

        //            XmlDocument xmlDocRes = new XmlDocument();
        //            xmlDocRes.LoadXml(responseXML);

        //            InfoAccessController.InsertUpdateAddressVerificationXMLRecord(null, xmlDocRes.InnerXml.ToString(), opsAV, ad.SearchesLeft, new Guid(CON.appAdminUserId),
        //                DateTime.Now, 2, log, eddrvID, "", result.DeliveryLine1, "", result.DeliveryLine2, result.City, result.State, result.ZipAddon, result.CountyNumber,
        //                result.CountyName, result.GeofLat, result.GeofLong, result.ErrorCodes, result.ReturnCodes, result.StreetName, result.StreetNumber, result.StreetSuffix,
        //                result.PreDirectional, result.ErrorDesc, result.SecondaryDesignation, result.SecondaryNumber, "");
        //        }

        //        return ad;
        //    }
        //    catch (WebException ex2)
        //    {
        //        log = new Logging(this.ThreadId);
        //        log.CreateLogEntry("Intelligent Search Address Verification: Error " + ex2.Message + " " + ex2.StackTrace, Logging.LogPriority.Error);
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log = new Logging(this.ThreadId);
        //        log.CreateLogEntry("Intelligent Search Address Verification: Error " + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);
        //        return null;
        //    }
        //}       

        //public AddressVerificatonDetail GetAddressVerificatonWS(AddressVerificationRequest address)
        //{
        //    int eddrvID = 0;
        //    AddressVerificatonDetail ad = new AddressVerificatonDetail();
        //    string opsAV = string.Empty;
        //    string isMatch = string.Empty;

        //    try
        //    {
        //        DataTable dt = InfoAccessController.CheckAddressVerificationRequired(address, CON.AddressType.AddressFromJOB, log);
        //        opsAV = Convert.ToString(dt.Rows[0]["opsAV"]);
        //        isMatch = Convert.ToString(dt.Rows[0]["IsMatch"]);

        //        if (isMatch.Equals("true"))
        //        {
        //            ad.AddressLine = Convert.ToString(dt.Rows[0]["DeliveryLine1"]);
        //            ad.AddressLine2 = Convert.ToString(dt.Rows[0]["SecondaryDesignation"]) + ' ' + Convert.ToString(dt.Rows[0]["SecondaryNumber"]);
        //            ad.City = Convert.ToString(dt.Rows[0]["City"]);
        //            ad.State = Convert.ToString(dt.Rows[0]["State"]);
        //            ad.PostalCode = Convert.ToString(dt.Rows[0]["ZipAddon"]);
        //            ad.County = Convert.ToString(dt.Rows[0]["CountyName"]);
        //            ad.CountyNumber = Convert.ToString(dt.Rows[0]["CountyNumber"]);
        //            ad.ErrorCodes = Convert.ToString(dt.Rows[0]["ErrorCodes"]);
        //            ad.ReturnCodes = Convert.ToString(dt.Rows[0]["ReturnCodes"]);
        //            ad.StreetName = Convert.ToString(dt.Rows[0]["StreetName"]);
        //            ad.StreetNumber = Convert.ToString(dt.Rows[0]["StreetNumber"]);
        //            ad.StreetSuffix = Convert.ToString(dt.Rows[0]["StreetSuffix"]);
        //            ad.PreDirectional = Convert.ToString(dt.Rows[0]["PreDirectional"]);
        //            ad.SearchesLeft = Convert.ToInt32(dt.Rows[0]["SearchesLeft"]);
        //        }
        //        else
        //        {

        //            string _urlI = AppSettings.Get("IntelliSearchWSURLOP", "https://www.intelligentsearch.com/CorrectAddressWS/CorrectAddressWebService.asmx?op=wsCorrectA");
        //            string _hostI = AppSettings.Get("IntelliSearchWSHOST", "www.intelligentsearch.com");

        //            EnvelopeAddressVerificationServiceReq result = new EnvelopeAddressVerificationServiceReq();
        //            result.username = _userName;
        //            result.password = _password;
        //            result.firmname = CompanyName;
        //            result.urbanization = string.Empty;
        //            result.delivery_line_1 = address.AddressLine;
        //            result.delivery_line_2 = address.AddressLine2;
        //            result.city_state_zip = address.City + " " + address.State + address.PostalCode;
        //            result.ca_codes = string.Empty;
        //            result.ca_filler = string.Empty;
        //            result.batchname = string.Empty;

        //            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(EnvelopeAddressVerificationServiceReq));
        //            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        //            var settings = new XmlWriterSettings();
        //            settings.Indent = true;
        //            settings.OmitXmlDeclaration = true;

        //            var stream2 = new StringWriter();
        //            var writer = XmlWriter.Create(stream2, settings);
        //            x.Serialize(writer, result, emptyNs);
        //            string xml = stream2.ToString();

        //            xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\">\r\n  <soap12:Body>" + xml;
        //            xml = xml + "  </soap12:Body>\r\n</soap12:Envelope>";

        //            XmlDocument xmlDocReq = new XmlDocument();
        //            xmlDocReq.LoadXml(xml);
        //            eddrvID = InfoAccessController.InsertUpdateAddressVerificationXMLRecord(xmlDocReq.InnerXml.ToString(), null, opsAV, 0, new Guid(CON.appAdminUserId),
        //                DateTime.Now, 1, log, eddrvID, address.AddressLine, "", address.AddressLine2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
        //                address.PostalCode);

        //            Uri apiUrl = new Uri(_urlI);
        //            WebRequest pmRequest = HttpWebRequest.Create(_urlI);
        //            HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
        //            byte[] bytes;
        //            bytes = System.Text.Encoding.ASCII.GetBytes(xmlDocReq.InnerXml.ToString());
        //            pmHttpRequest.ContentType = "application/soap+xml; charset=utf-8";
        //            pmHttpRequest.KeepAlive = true;
        //            pmHttpRequest.Method = "POST";
        //            pmHttpRequest.SendChunked = true;
        //            pmHttpRequest.UserAgent = ".NET Framework";
        //            pmHttpRequest.Host = _hostI;

        //            Stream requestStream = pmHttpRequest.GetRequestStream();
        //            requestStream.Write(bytes, 0, bytes.Length);
        //            requestStream.Close();
        //            string response = "";

        //            using (WebResponse pmResponse = pmHttpRequest.GetResponse())
        //            {
        //                HttpWebResponse pmHttpResponse = (HttpWebResponse)pmResponse;

        //                HttpStatusCode statuscode = pmHttpResponse.StatusCode;

        //                using (Stream stream = pmResponse.GetResponseStream())
        //                {
        //                    using (StreamReader sr = new StreamReader(stream))
        //                    {
        //                        response = sr.ReadToEnd();
        //                    }
        //                }
        //            }

        //            var xDoc = XDocument.Parse(response);
        //            var xLoginResult = xDoc.Root.Descendants().FirstOrDefault(d => d.Name.LocalName.Equals("WsCorrectAddress"));

        //            object result2;
        //            var serializer = new XmlSerializer(typeof(SoapResponseIS));
        //            using (var reader = new StringReader(xLoginResult.ToString()))
        //            {
        //                result2 = serializer.Deserialize(reader);
        //            }

        //            SoapResponseIS obj = (SoapResponseIS)result2;
        //            ad.AddressLine = obj.AddressLine;
        //            ad.AddressLine2 = obj.AddressLine2;
        //            ad.City = obj.City;
        //            ad.State = obj.State;
        //            ad.PostalCode = obj.PostalCode;
        //            ad.ZipAddon = obj.ZipAddon;
        //            ad.CountyName = obj.CountyName;
        //            ad.CountyNumber = obj.CountyNumber;
        //            ad.ErrorCodes = obj.ErrorCodes;
        //            ad.ReturnCodes = obj.ReturnCodes;
        //            ad.StreetName = obj.StreetName;
        //            ad.StreetNumber = obj.StreetNumber;
        //            ad.StreetSuffix = obj.StreetSuffix;
        //            ad.PreDirectional = obj.PreDirectional;
        //            ad.SearchesLeft = obj.SearchesLeft;
        //            ad.ErrorCodes = obj.ErrorCodes;
        //            ad.ErrorDesc = obj.ErrorDesc;
        //            ad.SecondaryDesignation = obj.SecondaryDesignation;
        //            ad.SecondaryNumber = obj.SecondaryNumber;                    


        //            System.Xml.Serialization.XmlSerializer x2 = new System.Xml.Serialization.XmlSerializer(typeof(SoapResponseIS));
        //            var emptyNs2 = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        //            var settings2 = new XmlWriterSettings();
        //            settings2.Indent = true;
        //            settings2.OmitXmlDeclaration = true;

        //            var stream3 = new StringWriter();
        //            var writer2 = XmlWriter.Create(stream3, settings2);
        //            x2.Serialize(writer2, obj, emptyNs2);
        //            string responseXML = stream3.ToString();

        //            XmlDocument xmlDocRes = new XmlDocument();
        //            xmlDocRes.LoadXml(responseXML);

        //            InfoAccessController.InsertUpdateAddressVerificationXMLRecord(null, xmlDocRes.InnerXml.ToString(), opsAV, ad.SearchesLeft, new Guid(CON.appAdminUserId),
        //                DateTime.Now, 2, log, eddrvID, "", obj.AddressLine, "", obj.AddressLine2, ad.City, obj.State, obj.ZipAddon, obj.CountyNumber,
        //                obj.CountyName, "", "", obj.ErrorCodes, obj.ReturnCodes, obj.StreetName, obj.StreetNumber, obj.StreetSuffix, obj.PreDirectional, obj.ErrorDesc,
        //                obj.SecondaryDesignation, obj.SecondaryNumber, "");

        //        }

        //        return ad;
        //    }
        //    catch (WebException ex2)
        //    {
        //        log = new Logging(this.ThreadId);
        //        log.CreateLogEntry("Intelligent Search Address Verification: Error " + ex2.Message + " " + ex2.StackTrace, Logging.LogPriority.Error);
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log = new Logging(this.ThreadId);
        //        log.CreateLogEntry("Intelligent Search Address Verification: Error " + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);
        //        return null;
        //    }
        //}

        public DataSet GetRegistrationData(int regId, string tableName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                // Only add the RegistrationId if it is greater than -1
                if (regId > -1) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));
                if (tableName == "OtherServiceLocations")
                {
                    parameters.Add(SqlParms.CreateParameter("ADDRESS_TYPE_ID", DbType.Int32, CON.AddressType.AlternateServiceLocation, true));
                }
                if (tableName == "Application")
                {

                    parameters.Add(SqlParms.CreateParameter("Primay_AddressTypeId", DbType.Int32, CON.AddressType.PrimaryContact, true));
                    parameters.Add(SqlParms.CreateParameter("Primay_ServiceTypeId", DbType.Int32, CON.AddressType.PrimaryPractice, true));
                    parameters.Add(SqlParms.CreateParameter("BillingTypeId", DbType.Int32, CON.AddressType.Billing, true));
                    parameters.Add(SqlParms.CreateParameter("CorrespondenceTypeId", DbType.Int32, CON.AddressType.Correspondence, true));
                    parameters.Add(SqlParms.CreateParameter("OtherTypeId", DbType.Int32, CON.AddressType.Other, true));
                    parameters.Add(SqlParms.CreateParameter("W9TypeId", DbType.Int32, CON.AddressType.FaxFormW9, true));
                    parameters.Add(SqlParms.CreateParameter("RemittanceTypeId", DbType.Int32, CON.AddressType.Remittance, true));
                    parameters.Add(SqlParms.CreateParameter("Home_Office_AddressId", DbType.Int32, CON.AddressType.HomeOffice, true));
                    parameters.Add(SqlParms.CreateParameter("One_AddressId", DbType.Int32, CON.AddressType.TaxForm1099, true));
                    parameters.Add(SqlParms.CreateParameter("ProfessionalAddressId", DbType.Int32, CON.AddressType.ProfessionalLicenseAddress, true));
                    parameters.Add(SqlParms.CreateParameter("LongTermCareAddressId", DbType.Int32, CON.AddressType.NursingFacility, true));
                    parameters.Add(SqlParms.CreateParameter("BHQuestions", DbType.String, "BH", true));
                }
                string storedProc = "";
                switch (tableName.ToLower())
                {
                    case "provider": storedProc = "usp_SelectREG_PROVIDER"; break;
                    case "application": storedProc = "usp_SelectREG_Application"; break;
                    case "question": storedProc = "usp_SelectREG_QUESTION"; break;
                    case "owner_xref_history": storedProc = "usp_SelectREG_OWNER_XREF_History"; break;
                    case "owner_other": storedProc = "usp_SelectREG_OWNER_OTHER"; break;
                    case "owner_conviction": storedProc = "usp_SelectREG_OWNER_CONVICTION"; break;
                    case "appeal": storedProc = "usp_SelectREG_APPEAL"; break;
                    case "application_fee": storedProc = "usp_SelectREG_APPLICATION_FEE"; break;
                    case "enrollment": storedProc = "usp_SelectREG_ENROLLMENT"; break;
                    case "ach_request_contact_custom": storedProc = "usp_SelectREG_ACH_REQUEST_CONTACT_CUSTOM"; break;
                    case "ach_request": storedProc = "usp_SelectREG_ACH_REQUEST"; break;
                    case "ach_request_history": storedProc = "usp_SelectREG_ACH_REQUEST_History"; break;
                    case "service_location": storedProc = "usp_SelectREG_SERVICE_LOCATION"; break;
                    case "section_status": storedProc = "usp_SelectREG_SECTION_STATUS"; break;
                    case "page_status": storedProc = "usp_SelectREG_PAGE_STATUS"; break;
                    case "specialty": storedProc = "usp_SelectREG_SPECIALTY"; break;
                    case "specialty_active": storedProc = "usp_SelectREG_SPECIALTY_Active"; break;
                    case "contracts": storedProc = "usp_SelectREG_CONTRACTS"; break;
                    case "insurance": storedProc = "usp_SelectREG_INSURANCE"; break;
                    case "affiliation": storedProc = "usp_SelectREG_affiliation"; break;
                    case "incident_compliance_case_xref": storedProc = "usp_SelectREG_INCIDENT_COMPLIANCE_CASE_XREF"; break;
                    case "hearing_rights": storedProc = "usp_SelectREG_hearing_rights"; break;
                    case "ach_reliacard": storedProc = "usp_SelectREG_ACH_RELIACARD"; break;
                    case "ach_fee_information": storedProc = "usp_SelectREG_ACH_FEE_INFORMATION"; break;
                    case "ach_contact": storedProc = "usp_SelectREG_ACH_CONTACTCustom"; break;
                    case "ach_contact_history": storedProc = "usp_SelectREG_ACH_CONTACT_History"; break;
                    case "ach_edison": storedProc = "usp_SelectREG_ACH_EDISON"; break;
                    case "addtl_specialty": storedProc = "usp_SelectREG_ADDTL_SPECIALTY"; break;
                    case "specialty_history": storedProc = "usp_SelectREG_SPECIALTY_History"; break;
                    case "addtl_taxonomy": storedProc = "usp_SelectREG_ADDTL_TAXONOMY"; break;
                    case "taxonomy_history": storedProc = "usp_SelectREG_TAXONOMY_History"; break;
                    case "appeal_notice": storedProc = "usp_SelectREG_APPEAL_NOTICE"; break;
                    case "background_check": storedProc = "usp_SelectREG_BACKGROUND_CHECK"; break;
                    case "behavioralhealthinfo": storedProc = "usp_SelectREG_BehavioralHealthInfo"; break;
                    case "service_location_history": storedProc = "usp_SelectREG_SERVICE_LOCATION_History"; break;
                    case "board_certification": storedProc = "usp_SelectREG_BOARD_CERTIFICATION"; break;
                    case "board_certification_history": storedProc = "usp_SelectREG_BOARD_CERTIFICATION_History"; break;
                    case "buildinghistory": storedProc = "usp_SelectREG_BuildingHistory"; break;
                    case "category_of_service_info": storedProc = "usp_SelectREG_CATEGORY_OF_SERVICE_INFO"; break;
                    case "state_cds_number": storedProc = "usp_SelectREG_STATE_CDS_NUMBER"; break;
                    case "state_cds_number_history": storedProc = "usp_SelectREG_STATE_CDS_NUMBER_HISTORY"; break;
                    case "certification": storedProc = "usp_SelectREG_CERTIFICATION"; break;
                    case "provider_history": storedProc = "usp_SelectREG_PROVIDER_History"; break;
                    case "dea": storedProc = "usp_SelectREG_DEA"; break;
                    case "deahistory": storedProc = "usp_SelectREG_DEAHistory"; break;
                    case "certification_history": storedProc = "usp_SelectREG_CERTIFICATION_History"; break;
                    case "clia": storedProc = "usp_SelectREG_CLIA"; break;
                    case "cliahistory": storedProc = "usp_SelectREG_CLIAHistory"; break;
                    case "chop_parent": storedProc = "usp_SelectREG_CHOP_PARENT"; break;
                    case "submitdate": storedProc = "usp_SelectREG_SubmitDate"; break;
                    case "cpr_certification": storedProc = "usp_SelectREG_CPR_Certification"; break;
                    case "firstaid_certification": storedProc = "usp_SelectREG_FIRSTAID_CERTIFICATION"; break;
                    case "credentialing": storedProc = "usp_SelectREG_CREDENTIALING"; break;
                    case "licenses": storedProc = "usp_SelectREG_LICENSES"; break;
                    case "dme_accreditation_agency": storedProc = "usp_SelectREG_DME_ACCREDITATION_AGENCY"; break;
                    case "dme_personnel_info": storedProc = "usp_SelectREG_DME_PERSONNEL_INFO"; break;
                    case "dme_background_chk_professional_info": storedProc = "usp_SelectREG_DME_BACKGROUND_CHK_PROFESSIONAL_INFO"; break;
                    case "dme_product_category_info": storedProc = "usp_SelectREG_DME_PRODUCT_CATEGORY_INFO"; break;
                    case "dme_registered_agent": storedProc = "usp_SelectREG_DME_REGISTERED_AGENT"; break;
                    case "dme_registered_agent_history": storedProc = "usp_SelectREG_DME_REGISTERED_AGENT_History"; break;
                    case "dme_product_category_info_history": storedProc = "usp_SelectREG_DME_PRODUCT_CATEGORY_INFO_History"; break;
                    case "educationhistory": storedProc = "usp_SelectREG_EDUCATIONHistory"; break;
                    case "enrollmenthistory": storedProc = "usp_SelectREG_EnrollmentHistory"; break;
                    case "delegate_credentialing": storedProc = "usp_SelectREG_DELEGATE_CREDENTIALING"; break;
                    case "household_member": storedProc = "usp_SelectREG_HOUSEHOLD_MEMBER"; break;
                    case "licenseshistory": storedProc = "usp_SelectREG_LICENSESHistory"; break;
                    case "malpractice_claim_history": storedProc = "usp_SelectREG_MALPRACTICE_CLAIM_History"; break;
                    case "mcp_affiliation_careplan": storedProc = "usp_SelectREG_MCP_AFFILIATION_CAREPLAN"; break;
                    case "mco_affiliation": storedProc = "usp_SelectREG_MCO_AFFILIATION"; break;
                    case "medicaid": storedProc = "usp_SelectREG_MEDICAID"; break;
                    case "medicaidhistory": storedProc = "usp_SelectREG_MEDICAID_HISTORY"; break;
                    case "medicare": storedProc = "usp_SelectREG_MEDICARE"; break;
                    case "medicarehistory": storedProc = "usp_SelectREG_MEDICARE_HISTORY"; break;
                    case "mspcostreportregxref": storedProc = "usp_SelectREG_MSPCostReportREGXref"; break;
                    case "provider_notecustom": storedProc = "usp_SelectREG_PROVIDER_NOTEcustom"; break;
                    case "nursing_professional_certification": storedProc = "usp_SelectREG_Nursing_Professional_Certification"; break;
                    case "owner_paper_provider": storedProc = "usp_SelectREG_owner_paper_provider"; break;
                    case "orientation": storedProc = "usp_SelectREG_ORIENTATION"; break;
                    case "evv_training": storedProc = "usp_SelectREG_EVV_TRAINING"; break;
                    case "owner_type": storedProc = "usp_SelectREG_OWNER_TYPE"; break;
                    case "owner": storedProc = "usp_SelectREG_OWNER"; break;
                    case "owner_history": storedProc = "usp_SelectREG_OWNER_History"; break;
                    case "subcontractor": storedProc = "usp_SelectREG_SUBCONTRACTOR"; break;
                    case "pharmacy_provider_info": storedProc = "usp_SelectREG_PHARMACY_PROVIDER_INFO"; break;
                    case "pharmacist_info": storedProc = "usp_SelectREG_PHARMACIST_INFO"; break;
                    case "npi_history": storedProc = "usp_SelectREG_NPI_HISTORY"; break;
                    case "event_info": storedProc = "usp_SelectREG_EVENT_INFO"; break;
                    case "reconsideration": storedProc = "usp_SelectREG_RECONSIDERATION"; break;
                    case "groupmemberrequiresretroreview": storedProc = "usp_SelectREG_GroupMemberRequiresRetroReview"; break;
                    case "reimbursement": storedProc = "usp_SelectREG_REIMBURSEMENT"; break;
                    case "restriction": storedProc = "usp_SelectREG_RESTRICTION"; break;
                    case "satellite_location_history": storedProc = "usp_SelectREG_SATELLITE_LOCATION_History"; break;
                    case "background_verification": storedProc = "usp_SelectREG_BACKGROUND_VERIFICATION"; break;
                    case "costreportregxref": storedProc = "usp_SelectREG_CostReportREGXref"; break;
                    case "useraccountinfo": storedProc = "usp_SelectREG_UserAccountInfo"; break;
                    case "dds": storedProc = "usp_SelectREG_DDS"; break;
                    case "specialtyservices": storedProc = "usp_SelectREG_SPECIALTYServices"; break;
                    case "providerworkhistory": storedProc = "usp_SelectREG_ProviderWorkHistory"; break;
                    case "ltc_risk_alert": storedProc = "usp_SelectREG_LTC_RISK_ALERT"; break;
                    case "owner_xref": storedProc = "usp_SelectREG_OWNER_XREF"; break;
                    case "owner_conviction_on_behalf": storedProc = "usp_SelectREG_owner_conviction_on_behalf"; break;
                    case "owner_sanction": storedProc = "usp_SelectREG_owner_sanction"; break;
                    case "original_owner": storedProc = "usp_SelectREG_original_owner"; break;
                    case "subcontractor5yrs": storedProc = "usp_SelectREG_SUBCONTRACTOR5yrs"; break;
                    case "owner_residency": storedProc = "usp_SelectREG_owner_residency"; break;
                    case "owner_transaction": storedProc = "usp_SelectREG_owner_transaction"; break;
                    case "subcontractor_history": storedProc = "usp_SelectREG_subcontractor_history"; break;
                    case "owner_sanction_history": storedProc = "usp_SelectREG_owner_sanction_history"; break;
                    case "owner_residency_history": storedProc = "usp_SelectREG_owner_residency_history"; break;
                    case "owner_conviction_history": storedProc = "usp_SelectREG_owner_conviction_history"; break;
                    case "owner_other_history": storedProc = "usp_SelectREG_owner_other_history"; break;
                    case "healthcareaffiliationhistory": storedProc = "usp_SelectREG_HEALTH_CARE_FACILITY_AFFILIATION_HISTORY"; break;
                    case "credentialeddelegatehistory": storedProc = "usp_SelectREG_DELEGATE_CREDENTIALING_HISTORY"; break;
                    case "groupaffiliationhistory": storedProc = "usp_SelectREG_AFFILIATION_HISTORY"; break;
                    case "grouptoindivudalaffiliationhistory": storedProc = "usp_SelectREG_AFFILIATION_HISTORY_GROUP"; break;
                    case "getproviderfeednotesdata": storedProc = "usp_SelectRegProviderFeedNotes"; break;
                    case "getproviderfeedaddnotedata": storedProc = "usp_SelectProviderFeedAddNoteDetails"; break;
                    case "provider_note_hist": storedProc = "usp_SelectREG_PROVIDER_HistNotes"; break;
                    case "orginfohistory": storedProc = "usp_SelectREG_ORGINFO_HISTORY"; break;
                    case "otherservicelocations": storedProc = "usp_SelectREG_ADDRESS_HISTORY"; break;
                    case "loaddisclosure": storedProc = "usp_Get_REG_DISCLOSURES"; break;
                    default: throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }
                DataSet ds = new DataSet();
                Logging log = new Logging();
                //log.CreateLogEntry("Stored proc being called is " + storedProc, Logging.LogPriority.Error);
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "RegistrationData_" + tableName);
                ds.Tables[0].TableName = "RegistrationData";
                return ds;
            }
            catch (Exception ex)
            {
                log = new Logging(this.ThreadId);
                log.CreateLogEntry("GetRegistrationData: Error " + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);
                return null;
            }
        }

        public DataTable GetORPProviderSearchResults(string providerNPI, DateTime dos)
        {
            DataTable retUserRoles = null;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PROVIDER_NPI", DbType.String, providerNPI, false));
                parameters.Add(SqlParms.CreateParameter("DOS", DbType.DateTime, dos, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetORPProviderSearchResults", parameters, "GetORPProviderSearchResults");
                if (ObjectControllerHelper.HasRows(ds))
                {
                    if (ds.Tables[0] != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ds.Tables[0].TableName = "ORProviderSearchResults";
                            retUserRoles = ds.Tables[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return retUserRoles;
        }


        //public List<RegApplicationStatus> LoadRegistrationApplicationStatus(int regID)
        //{
        //    List<RegApplicationStatus>  regApplications = new List<RegApplicationStatus>();
        //    DataSet dsRegistration = RegistrationController.SelectRegistrationByRegID(regID);
        //    //ProviderManagementData details = new ProviderManagementData();
        //    details.LoadObjectFromDataset(dsRegistration);
        //    DataSet ds = RegistrationController.SelectCurrentAndPreviousApplicationsByRegID(regID);
        //    if (ds != null && ds.Tables.Count > 0)
        //    {
        //        DataTable dtAppdetails = ds.Tables[0];

        //        /*
        //        so for call reg_applications except for the last one, 
        //        if "accepted" we show "Approved / Complete"
        //        if "denied" we show "Denied"
        //        if "not processed" we show "cancelled"

        //        for latest application, if app status is not processed, and WF is ended, we can show cancelled
        //        if wf is not complete and reg status is approved; show as processing
        //        if wf is complete, show what final reg_status is
        //        */

        //        if (dtAppdetails.Rows.Count > 0)
        //        {
        //            for (int rowCnt = 0; rowCnt < dtAppdetails.Rows.Count; rowCnt++)
        //            {
        //                DataRow currentRow = dtAppdetails.Rows[rowCnt];

        //                if (rowCnt == dtAppdetails.Rows.Count - 1)
        //                {
        //                    // this is the last REG_APPLICATION; use the status of the REGISTRATION instead of the REG_APPLICATION data
        //                    // for latest application, if app status is not processed, and WF is ended, we can show cancelled
        //                    // if wf is not complete and reg status is approved; show as processing
        //                    // if wf is complete, show what final reg_status is

        //                    // System.Diagnostics.Debug.WriteLine("on final row: " + currentRow["PNMApplicationStatus"].ToString() + "/" + currentRow["WorkflowComplete"].ToString());
        //                    if (currentRow["WorkflowComplete"].ToString().Equals("N"))
        //                    {
        //                        // workflow is not complete; if registration status is 'Approved,' assume we are still processing it
        //                        if (details.RegistrationStatusTypeID == CON.RegistrationStatusTypeId.Approved)
        //                        {
        //                            currentRow["PNMApplicationStatus"] = "Processing";
        //                        }
        //                        else
        //                        {
        //                            // shouldn't be here, but in case we are, just show registration status
        //                            currentRow["PNMApplicationStatus"] = details.RegistrationStatusType;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        // workflow is complete
        //                        // if REG_APPLICATION is "NOT PROCESSED", show cancelled
        //                        if (currentRow["PNMApplicationStatus"].ToString().Equals("NOT PROCESSED"))
        //                        {

        //                            currentRow["PNMApplicationStatus"] = "Cancelled";
        //                        }
        //                        else if (currentRow["PNMApplicationStatus"].ToString().Equals("DENIED"))
        //                        {
        //                            if (((details.RegistrationStatusTypeID == CON.RegistrationStatusTypeId.NotProcessed || details.RegistrationStatusTypeID == CON.RegistrationProgramStatusTypeId.NotProcessed)
        //                                && currentRow["REGISTRATION_STATUS_TYPE_ID"].ToString() == CON.RegistrationStatusTypeId.NotProcessed.ToString()) ||
        //                                 (currentRow["Program"].ToString().Equals("ODA") && currentRow["OtherAgencyApplicationStatus"].ToString().Equals("Recommended Certification") && currentRow["REG_PROGRAM_STATUS_TYPE_ID"].ToString() == CON.RegistrationProgramStatusTypeId.NotProcessed.ToString())
        //                                || (currentRow["Program"].ToString().Equals("DD") && currentRow["OtherAgencyApplicationStatus"].ToString().Equals("Closed By ODM") && currentRow["REG_PROGRAM_STATUS_TYPE_ID"].ToString() == CON.RegistrationProgramStatusTypeId.NotProcessed.ToString()))
        //                            {
        //                                currentRow["PNMApplicationStatus"] = "Not Processed";
        //                            }

        //                            else
        //                                currentRow["PNMApplicationStatus"] = details.RegistrationStatusType;
        //                        }
        //                        // if REG_APPLICATION is "APPROVED" show "Approved / Complete"
        //                        else if (currentRow["PNMApplicationStatus"].ToString().Equals("ACCEPTED"))
        //                        {
        //                            currentRow["PNMApplicationStatus"] = "Approved / Complete";
        //                        }
        //                        else
        //                        {
        //                            // otherwise, just show final registration status
        //                            currentRow["PNMApplicationStatus"] = details.RegistrationStatusType;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    // this isn't the very latest REG_APPLICATION, so infer the status from the REG_APPLICATION status
        //                    // REG_APPLICATION status "ACCEPTED" should be "Approved / Complete"
        //                    // REG_APPLICATION status "DENIED" should be "Denied"
        //                    // REG_APPLICATION status "NOT PROCESSED" should be "Cancelled"
        //                    // System.Diagnostics.Debug.WriteLine("on early row: " + currentRow["PNMApplicationStatus"].ToString());
        //                    if (currentRow["PNMApplicationStatus"].ToString().Equals("DENIED"))
        //                    {
        //                        if (((details.RegistrationStatusTypeID == CON.RegistrationStatusTypeId.NotProcessed || details.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.NotProcessed)
        //                                && currentRow["REGISTRATION_STATUS_TYPE_ID"].ToString() == CON.RegistrationStatusTypeId.NotProcessed.ToString()) ||
        //                                 (currentRow["Program"].ToString().Equals("ODA") && currentRow["OtherAgencyApplicationStatus"].ToString().Equals("Recommended Certification") && currentRow["REG_PROGRAM_STATUS_TYPE_ID"].ToString() == CON.RegistrationProgramStatusTypeId.NotProcessed.ToString())
        //                                || (currentRow["Program"].ToString().Equals("DD") && currentRow["OtherAgencyApplicationStatus"].ToString().Equals("Closed By ODM") && currentRow["REG_PROGRAM_STATUS_TYPE_ID"].ToString() == CON.RegistrationProgramStatusTypeId.NotProcessed.ToString()))
        //                        {
        //                            currentRow["PNMApplicationStatus"] = "Not Processed";
        //                        }

        //                        else
        //                            currentRow["PNMApplicationStatus"] = "Denied";
        //                    }
        //                    else if (currentRow["PNMApplicationStatus"].ToString().Equals("ACCEPTED"))
        //                    {
        //                        currentRow["PNMApplicationStatus"] = "Approved / Complete";
        //                    }
        //                    else if (currentRow["PNMApplicationStatus"].ToString().Equals("NOT PROCESSED"))
        //                    {
        //                        currentRow["PNMApplicationStatus"] = "Cancelled";
        //                    }

        //                }
        //            }

        //            regApplications = MapToRegApplicationStatusList(dtAppdetails);
        //        }
        //    }
        //   return regApplications;
        //}

        public List<RegApplicationStatus> MapToRegApplicationStatusList(DataTable dt)
        {
            var applicationStatusList = new List<RegApplicationStatus>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var status = new RegApplicationStatus
                    {
                        RegId = row["REG_ID"] != DBNull.Value ? Convert.ToInt32(row["REG_ID"]) : 0,
                        ProcessId = row["ProcessID"] != DBNull.Value ? Convert.ToInt32(row["ProcessID"]) : 0,
                        ApplicationStatus = row["PNMApplicationStatus"]?.ToString() ?? string.Empty
                    };

                    applicationStatusList.Add(status);
                }
            }

            return applicationStatusList;
        }

        public static bool ValidateForNPITypeFromDB(string npiValue)
        {
            bool isValidNPI = false;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.Int64, npiValue, false));

                DataSet dsNPPES = new DataSet();
                dsNPPES = DataAccess.ExecuteStoredProcedure("NPI", "usp_Search_NPPES_EntityType", parameters, "NPI");
                dsNPPES.Tables[0].TableName = "NPI";

                if (Helper.HasRows(dsNPPES))
                {
                    int nppesTypeID = Methods.GetIntValue(dsNPPES.Tables[0].Rows[0]["Entity_Type"]);
                    if (nppesTypeID == 1)
                    {
                        isValidNPI = true;
                    }
                }

                return isValidNPI;
            }
            catch (Exception ex)
            {

            }
            return isValidNPI;
        }

        public static bool ValidateNPIUniqueness(int regId, string npi)
        {
            bool isUnique = false;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));

                int activeProviderCount = Convert.ToInt32(DataAccess.ExecuteScalar("usp_ValidateNPIUniqueness", parameters));

                if (activeProviderCount == 0)
                {
                    isUnique = true;
                }
                else
                {
                    isUnique = false;
                }
                return isUnique;
            }
            catch (Exception ex)
            {

            }
            return isUnique;
        }

        public static DataTable LoadTaxonomyFromNPPES(Result rs)
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("TaxonomyName");
            dt.Columns.Add("TaxonomyCode");
            dt.Columns.Add("TaxonomyNameWithCode");

            foreach (Taxonomy t in rs.taxonomies)
            {
                bool contains = false;
                if (!contains)
                {
                    DataRow tax = dt.NewRow();
                    tax["TaxonomyNameWithCode"] = t.desc + " (" + t.code + ")";
                    tax["TaxonomyName"] = t.desc;
                    tax["TaxonomyCode"] = t.code;
                    dt.Rows.Add(tax);

                    //if taxonomy group exists and is not duplicated then add
                    if (!string.IsNullOrEmpty(t.taxonomy_group))
                    {
                        if (t.taxonomy_group.Contains(" ") && dt.Select("TaxonomyNameWithCode = '" + t.taxonomy_group + "'").Count() == 0)
                        {
                            tax = dt.NewRow();

                            string taxName = t.taxonomy_group.Remove(0, t.taxonomy_group.IndexOf(' ') + 1).Trim();

                            tax["TaxonomyName"] = taxName;
                            tax["TaxonomyCode"] = t.taxonomy_group.Replace(taxName, "");
                            tax["TaxonomyNameWithCode"] = tax["TaxonomyName"] + " (" + tax["TaxonomyCode"] + ")";
                            dt.Rows.Add(tax);
                        }
                    }
                }
            }
            return dt.DefaultView.ToTable(true, "TaxonomyNameWithCode", "TaxonomyName", "TaxonomyCode");
        }
        public static int InsertNewStreamlinedProviderRegistration(PDMSRestServices.Models.ProviderRegistrationData provData)
        {
            int regId = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserID", DbType.Guid, provData.UserID, false));
                parameters.Add(SqlParms.CreateParameter("ProvAdminUserID", DbType.Guid, provData.SelectedProvAdminUserID, false));
                parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, provData.FirstName, false));
                parameters.Add(SqlParms.CreateParameter("MiddleInitial", DbType.String, provData.MiddleInitial, false));
                parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, provData.LastName, false));
                parameters.Add(SqlParms.CreateParameter("Gender", DbType.String, provData.Gender, false));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, provData.TaxID, false));
                parameters.Add(SqlParms.CreateParameter("TaxIDTypeID", DbType.Int32, provData.TaxIDTypeID, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, provData.NPI, true));
                parameters.Add(SqlParms.CreateParameter("MMISProviderTypeID", DbType.String, provData.MMISProviderTypeID, false));
                if (provData.BirthDate.HasValue)
                    parameters.Add(SqlParms.CreateParameter("BirthDate", DbType.Date, provData.BirthDate, true));
                if (provData.RequestedEffectiveDate.HasValue)
                    parameters.Add(SqlParms.CreateParameter("RequestedEffectiveDate", DbType.Date, provData.RequestedEffectiveDate, true));

                parameters.Add(SqlParms.CreateParameter("TaxonomyCode", DbType.String, provData.TaxonomyCode, true));
                parameters.Add(SqlParms.CreateParameter("TaxonomyName", DbType.String, provData.TaxonomyName, true));
                parameters.Add(SqlParms.CreateParameter("SpecialtyTypeID", DbType.Int32, provData.SpecialtyTypeID, true));
                parameters.Add(SqlParms.CreateParameter("AddressContactName", DbType.String, provData.AddressContactName, true));
                parameters.Add(SqlParms.CreateParameter("Address1", DbType.String, provData.AddressLine1, true));
                parameters.Add(SqlParms.CreateParameter("Address2", DbType.String, provData.AddressLine2, true));
                parameters.Add(SqlParms.CreateParameter("City", DbType.String, provData.City, true));
                parameters.Add(SqlParms.CreateParameter("State", DbType.String, provData.State, true));
                parameters.Add(SqlParms.CreateParameter("CountyName", DbType.String, provData.CountyName, true));
                parameters.Add(SqlParms.CreateParameter("CountyCode", DbType.String, provData.CountyCode, true));
                parameters.Add(SqlParms.CreateParameter("Zip", DbType.String, provData.ZipCode, true));
                parameters.Add(SqlParms.CreateParameter("ZipExt", DbType.String, provData.ZipExt, true));
                parameters.Add(SqlParms.CreateParameter("Phone", DbType.String, provData.PhoneNumber, true));
                parameters.Add(SqlParms.CreateParameter("Email", DbType.String, provData.EmailAddress, true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedDateTime", DbType.DateTime, DateTime.Now.ToString(), true));
                parameters.Add(SqlParms.CreateParameter("LastModifiedUser", DbType.Guid, provData.UserID, true));
                parameters.Add(SqlParms.CreateParameter("Longitude", DbType.String, provData.Longitude, true));
                parameters.Add(SqlParms.CreateParameter("Latitude", DbType.String, provData.Latitude, true));
                parameters.Add(SqlParms.CreateParameter("OverrideAddress", DbType.Int32, provData.OverrideAddress, true));

                regId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertNewStreamlinedProviderRegistration", parameters));
            }
            catch (Exception ex)
            {

            }
            return regId;
        }

        public AddressVerificatonDetail GetAddressVerificatonWSTiger(AddressVerificationRequest address)
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
                    eddrvID = Convert.ToInt32(dt.Rows[0]["OUTBOUND_EADDRV_REQ_RES_ID"]);
                }
                else
                {

                    string _urlI = AppSettings.Get("IntelliSearchWSURLOPTiger", "https://www.intelligentsearch.com/CorrectAddressWS/CorrectAddressWebService.asmx?op=wsTigerCA");
                    string _hostI = AppSettings.Get("IntelliSearchWSHOST", "www.intelligentsearch.com");

                    PDMSRestServices.Models.USPSAddressVerificationServiceRequest result = new PDMSRestServices.Models.USPSAddressVerificationServiceRequest();
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

                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(PDMSRestServices.Models.USPSAddressVerificationServiceRequest));
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
                    var serializer = new XmlSerializer(typeof(PDMSRestServices.Models.USPSSoapResponseIS));
                    using (var reader = new StringReader(xLoginResult.ToString()))
                    {
                        result2 = serializer.Deserialize(reader);
                    }

                    PDMSRestServices.Models.USPSSoapResponseIS obj = (PDMSRestServices.Models.USPSSoapResponseIS)result2;
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


                    System.Xml.Serialization.XmlSerializer x2 = new System.Xml.Serialization.XmlSerializer(typeof(PDMSRestServices.Models.USPSSoapResponseIS));
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

        public DataSet GetAllDynamicControlsData()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = DataAccess.ExecuteStoredProcedure("usp_Select_ALL_DYNAMIC_FIELD_CONFIGURATION", "Dyanmic_Controls_Data");
            }
            catch (Exception ex)
            {
                log = new Logging(this.ThreadId);
                log.CreateLogEntry("Get all Dynamic Controls Data: Error " + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);

            }
            return ds;
        }
        public DataSet GetUIControlVisibilityByRegId(int regId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("reg_id ", DbType.Int64, regId, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_GetUIControlVisibilityByRegID", parameters, "UIControlsData");
            }
            catch (Exception ex)
            {
                log = new Logging(this.ThreadId);
                log.CreateLogEntry("Get all Dynamic Controls Data: Error " + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);

            }
            return ds;
        }

        public void UpdateUIControlVisibilityData(UpdateUIControls data)
        {
            try
            {
                if (data != null && data.Ids != null && data.Ids.Count >= 0)
                {
                    foreach (int id in data.Ids)
                    {
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("ConfigId", DbType.Int64, id, true));
                        parameters.Add(SqlParms.CreateParameter("Action", DbType.String, data.Action, true));
                        DataAccess.ExecuteStoredProcedure("usp_UpdateUIControlVisibilityStatus", parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                log = new Logging(this.ThreadId);
                log.CreateLogEntry("Update UI Control Visibility Data: Error " + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);
            }
        }


        public void UpdateDynamicControlsData(UpdateDynamicControls data)
        {
            string ids = string.Join(",", data.Ids.Select(id => id.ToString()));
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DYNAMIC_FIELD_CONFIGURATION_ID ", DbType.String, ids, true));
                parameters.Add(SqlParms.CreateParameter("IS_ACTIVE ", DbType.Int32, data.Status, true));

                DataAccess.ExecuteStoredProcedure("usp_UPDATE_DYNAMIC_FIELD_CONFIGURATION", parameters);
            }
            catch (Exception ex)
            {
                log = new Logging(this.ThreadId);
                log.CreateLogEntry("Update Dynamic Controls Status Data: Error " + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);

            }
        }

        public void DeleteUIConfigById(DeleteUI ids)
        {
            if (ids != null && ids.Ids != null && ids.Ids.Count() > 0)
            {
                foreach (var configId in ids.Ids)
                {
                    var param = new List<SqlParameter>
                        {
                            SqlParms.CreateParameter("ConfigId", DbType.Int32, configId, true)
                        };
                    DataAccess.ExecuteStoredProcedure("usp_DeleteUIControlVisibilityConfig", param);
                }
            }
        }

        public void DeleteDynamicFieldConfigById(DeleteDynamicFields ids)
        {
            if (ids != null && ids.Ids != null && ids.Ids.Count() > 0)
            {
                foreach (var configId in ids.Ids)
                {
                    var param = new List<SqlParameter>
                        {
                            SqlParms.CreateParameter("ConfigId", DbType.Int32, configId, true)
                        };
                    DataAccess.ExecuteStoredProcedure("usp_DeleteDynamicFieldConfigById", param);
                }
            }
        }

        public void UpdateDynamicFieldConfiguration(UpdateDynamicControls data)
        {
            try
            {
                if (data != null && data.Ids != null && data.Ids.Count >= 0)
                {
                    foreach (int id in data.Ids)
                    {
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("ConfigId", DbType.Int64, id, true));
                        parameters.Add(SqlParms.CreateParameter("Action", DbType.String, data.Status, true));
                        DataAccess.ExecuteStoredProcedure("usp_UpdateDYNAMICFIELDCONFIGURATIONStatus", parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                log = new Logging(this.ThreadId);
                log.CreateLogEntry("Update Dynamic Fields Configuration Data: Error " + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);
            }
        }

        public int SaveRegDisclosure(DisclosureDto data)
        {
            try
            {
                var parameters = new List<SqlParameter>();

                // Input params (insert or update)


                object regDisclosureId = (data.REG_DISCLOSURES_ID > 0) ? (object)data.REG_DISCLOSURES_ID : DBNull.Value;
                parameters.Add(SqlParms.CreateParameter("REG_DISCLOSURE_ID", DbType.Int32, regDisclosureId, true));


                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, data.REG_ID, false));
                parameters.Add(SqlParms.CreateParameter("REG_QUESTION_ID", DbType.Int32, data.REG_QUESTION_ID, false));
                parameters.Add(SqlParms.CreateParameter("QUESTION_TYPE_ID", DbType.String, data.QUESTION_TYPE_ID, false));

                parameters.Add(SqlParms.CreateParameter("IS_SANCTIONED", DbType.Boolean, DBNull.Value, false));

                var incidentDate = ParseIncidentDate(data.INCIDENT_DATE);
                parameters.Add(SqlParms.CreateParameter("INCIDENT_DATE", DbType.DateTime,
                    incidentDate.HasValue ? (object)incidentDate.Value : DBNull.Value, true));

                parameters.Add(SqlParms.CreateParameter("STATE_CODE", DbType.String, DbNullable(data.STATE_CODE), true));
                parameters.Add(SqlParms.CreateParameter("PROGRAM_AFFECTED", DbType.String, DbNullable(data.PROGRAM_AFFECTED), true));
                parameters.Add(SqlParms.CreateParameter("AGENCY_TAKING_ACTION", DbType.String, DbNullable(data.AGENCY_TAKING_ACTION), true));
                parameters.Add(SqlParms.CreateParameter("ACTION_TAKEN", DbType.String, DbNullable(data.ACTION_TAKEN), true));
                parameters.Add(SqlParms.CreateParameter("EXPLANATION_DETAILS", DbType.String, DbNullable(data.EXPLANATION_DETAILS), true));
                parameters.Add(SqlParms.CreateParameter("COURT_ID", DbType.String, DbNullable(data.COURT_ID), true));
                parameters.Add(SqlParms.CreateParameter("COUNTRY_ID", DbType.String, DbNullable(data.COUNTRY_ID), true));
                parameters.Add(SqlParms.CreateParameter("COUNTY_ID", DbType.String, DbNullable(data.COUNTY_ID), true));
                parameters.Add(SqlParms.CreateParameter("CHARGE", DbType.String, DbNullable(data.CHARGE), true));
                parameters.Add(SqlParms.CreateParameter("AGREEMENT", DbType.String, DbNullable(data.AGREEMENT), true));
                parameters.Add(SqlParms.CreateParameter("CASE_NUMBER", DbType.String, DbNullable(data.CASE_NUMBER), true));
                parameters.Add(SqlParms.CreateParameter("IS_SELECTED", DbType.String, DbNullable(data.IS_SELECTED), true));

                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, CON.appAdminUserId, false));

                // Output param
                var outParam = new SqlParameter("OUT_REG_DISCLOSURE_ID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                parameters.Add(outParam);

                DataAccess.ExecuteStoredProcedure("usp_Save_REG_DISCLOSURES", parameters);

                return outParam.Value != DBNull.Value ? Convert.ToInt32(outParam.Value) : 0;
            }
            catch (Exception ex)
            {
                log?.CreateLogEntry("Save registration disclosures: Error " + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);
                throw;
            }
        }

        public void DeleteRegDisclosure(int regDisclosureId)
        {
            if (regDisclosureId > 0)
            {
                var param = new List<SqlParameter>
                    {
                        SqlParms.CreateParameter("REG_DISCLOSURES_ID", DbType.Int32, regDisclosureId, false)
                    };
                DataAccess.ExecuteStoredProcedure("usp_Delete_REG_DISCLOSURES", param);
            }
        }

        private static DateTime? ParseIncidentDate(string mmddyyyy)
        {
            if (string.IsNullOrWhiteSpace(mmddyyyy)) return null;
            if (DateTime.TryParseExact(mmddyyyy, "MM/dd/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var dt))
            {
                return dt;
            }
            return null; // or throw new FormatException(...)
        }

        private static object DbNullable(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : s.Trim();
        }


    }
}
