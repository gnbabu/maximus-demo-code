using Corp.Core.Libraries.ClaimsReference;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.UI;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Corp.Core.Libraries
{

    public partial class ClaimsServiceAgent
    {
        private static string url = AppSettings.Get("HospiceWSURL", Constants.WebServiceURI.HospiceService);
        private static string wsCertificateName = AppSettings.Get("HospiceWSCertificateName");
        private static string wsHost = AppSettings.Get("HospiceWSHost");
        private static string wsUserName = AppSettings.Get("HospiceWSUserName");

        private readonly StateBag ViewState = null;

        public ClaimsServiceAgent(StateBag bag)
        {
            ViewState = bag;
        }

        public static string SubmitClaim(XDocument claim, String claimType = "Dental")
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(claim.ToString());

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            CertUtil _certs = new CertUtil();
            X509Certificate2 clientCertificate = _certs.GetCertificateByName(AppSettings.Get("ClaimsWSCertificateName"));

            string soapAction = AppSettings.Get("ClaimsSubmissionWSURL");
            string endpoint = AppSettings.Get("ClaimsSubmissionWSURL");
            string claimsSubmitWebAPITesting = AppSettings.Get("ClaimsWebAPITesting", "false");

            if (claimType == "Dental")
            {
                soapAction = "http://service.operationmgmt.fi/ClaimsService/dental/AddUpdateClaims";
                endpoint = AppSettings.Get("ClaimsSubmissionWSURL");
            }
            if (claimType == "Institutional")
            {
                soapAction = "http://service.operationmgmt.fi/ClaimsService/institutional/AddUpdateClaims";
                endpoint = AppSettings.Get("ClaimsSubmissionInstitutionalWSURL");
            }
            if (claimType == "Professional")
            {
                soapAction = "http://service.operationmgmt.fi/ClaimsService/professional/AddUpdateClaims";
                endpoint = AppSettings.Get("ClaimsSubmissionProfessionalWSURL");
            }

            WebRequest pmRequest = HttpWebRequest.Create(endpoint);
            HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
            pmHttpRequest.ContentType = "text/xml; charset=utf-8";

            pmHttpRequest.KeepAlive = true;
            pmHttpRequest.Method = "POST";
            pmHttpRequest.SendChunked = true;
            pmHttpRequest.UserAgent = ".NET Framework";
            if (clientCertificate != null)
            {
                pmHttpRequest.ClientCertificates.Add(clientCertificate);
            }
            string client_secret = "";
            string secretName = "ClaimsWSPassword_OH_PNM_";
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
            String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(AppSettings.Get("ClaimsWSUserName") + ":" + client_secret));
            pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
            pmHttpRequest.Headers.Add("SOAPAction", soapAction);
            string response = "";

            if (claimsSubmitWebAPITesting.Equals("false"))
            {
                Stream requestStream = pmHttpRequest.GetRequestStream();

                if (requestStream != null)
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
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
                }
            }
            else
            {
                DataSet dswa = null;
                if (soapAction.ToLower().Contains("dental"))
                {
                    dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("submitdentalClaimsWebAPI");
                }
                if (soapAction.ToLower().Contains("institutional"))
                {
                    dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("submitInstClaimsWebAPI");
                }
                if (soapAction.ToLower().Contains("professional"))
                {
                    dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("submitProfClaimsWebAPI");
                }

                if (Methods.HasRows(dswa))
                {
                    DataTable dtwa = dswa.Tables[0];
                    DataRow dr = dtwa.Rows.Count > 0 ? dtwa.Rows[0] : null;
                    response = Methods.GetString("APIXML", dr);
                }
            }

            return response;
        }

        private static void SetSecurityOptions()
        {
            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
        }

        private static MessageHeader CreateMessageHeader()
        {
            return new MessageHeader
            {
                BusinessFlow = "AddUpdateClaims",
                SubscriberSystem = "MITS",
                AdditionalModuleTransactionId = string.Empty,
                ModuleTransactionId = "",
                RequestTimestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss"),
                SITransactionKey = ProviderManagementHelper.GetUniqueKey(32),
            };
        }

        public List<AdditionalProvider> AdditionalProviders
        {
            get
            {
                return ViewState["AdditionalProvider"] == null ? new List<AdditionalProvider>() : (List<AdditionalProvider>)ViewState["AdditionalProvider"];
            }
            set
            {
                ViewState["AdditionalProvider"] = value;
            }

        }

        public List<string> ValidateAdditionalProviders()
        {
            // Additional Provider
            List<string> errorList = new List<string>();
            foreach (AdditionalProvider row in AdditionalProviders)
            {
                if (string.IsNullOrEmpty(row.cde_provider_type))
                {
                    errorList.Add("Provider type required for detail " + row.num_dtl + " in additional provider information panel");
                }

                if (string.IsNullOrEmpty(row.cde_party_id))
                {
                    errorList.Add("Provider NPI required for detail " + row.num_dtl + " in additional provider information panel");
                }

                if (string.IsNullOrEmpty(row.nam_last))
                {
                    errorList.Add("Additional provider is unknown for detail " + row.num_dtl + " in additional provider information panel");
                }
            }

            return errorList;
        }



        public List<OtherPayerPaidAmount> OtherPayerPaidAmounts
        {
            get
            {
                return ViewState["OtherPayerPaid"] == null ? new List<OtherPayerPaidAmount>() : (List<OtherPayerPaidAmount>)ViewState["OtherPayerPaid"];
            }
            set
            {
                ViewState["OtherPayerPaid"] = value;
            }

        }

        public List<string> ValidateOtherPayerPaidAmount()
        {
            List<string> errorList = new List<string>();


            foreach (OtherPayerPaidAmount pay in OtherPayerPaidAmounts)
            {
                // Need to add validation for other payer paid 
            }

            return errorList;
        }
        public List<ClaimsAdjustment> ClaimAdjustments
        {
            get
            {
                return ViewState["ClaimsAdjustment"] == null ? new List<ClaimsAdjustment>() : (List<ClaimsAdjustment>)ViewState["ClaimsAdjustment"];
            }
            set
            {
                ViewState["ClaimsAdjustment"] = value;
            }

        }
        public List<ClaimsAdjustment> OtherPayerClaimAdjustments
        {
            get
            {
                return ViewState["ClaimsAdjustment"] == null ? new List<ClaimsAdjustment>() : (List<ClaimsAdjustment>)ViewState["ClaimsAdjustment"];
            }
            set
            {
                ViewState["ClaimsAdjustment"] = value;
            }

        }

        public List<Attachment> Attachments
        {
            get
            {
                return ViewState["Attachments"] == null ? new List<Attachment>() : (List<Attachment>)ViewState["Attachments"];
            }
            set
            {
                ViewState["Attachments"] = value;
            }

        }

        public List<Attachment> ByMailDocuments
        {
            get
            {
                return ViewState["MailDocuments"] == null ? new List<Attachment>() : (List<Attachment>)ViewState["MailDocuments"];
            }
            set
            {
                ViewState["MailDocuments"] = value;
            }

        }

        public List<ProviderNote> ProviderNotes
        {
            get
            {
                return ViewState["ProviderNotes"] == null ? new List<ProviderNote>() : (List<ProviderNote>)ViewState["ProviderNotes"];
            }
            set
            {
                ViewState["ProviderNotes"] = value;
            }

        }

        public List<NDCDetail> NDCDetails
        {
            get
            {
                return ViewState["NdcDetails"] == null ? new List<NDCDetail>() : (List<NDCDetail>)ViewState["NdcDetails"];
            }
            set
            {
                ViewState["NdcDetails"] = value;
            }

        }



    }



}