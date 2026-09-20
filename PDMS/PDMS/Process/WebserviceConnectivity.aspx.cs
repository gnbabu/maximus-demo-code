using Corp.Core.Libraries;
using Corp.Core.Libraries.HospiceReference;
using Corp.Core.Libraries.PriorAuthServiceReference;
using Corp.Core.Libraries.SI.AttachmentServiceReference;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Microsoft.Web.Services3.Security.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using CON = MAXIMUS.Core.Libraries.Constants;
using ClaimsSearch = Corp.Core.Libraries.FI.ClaimsSearchReference;
using ClaimsDentalReference = Corp.Core.Libraries.FI.ClaimsDentalService;
using ClaimsInstitutionalReference = Corp.Core.Libraries.FI.ClaimsInstitutionalService;
using ClaimsProfessionalReference = Corp.Core.Libraries.FI.ClaimsProfessionalService;
using Corp.Core.Libraries.AcknowledgmentService;
using System.Text.RegularExpressions;
using System.ServiceModel;
using System.Xml.Linq;
using System.Data.SqlClient;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using NPOI.SS.Formula.Functions;
using Amazon.Runtime;
using Microsoft.Web.Services3.Referral;
using System.Web.Services.Description;

public partial class Process_WebserviceConnectivity : RegistrationProvider
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBindGridView();
        }
    }

    private PDMSService.PDMSServiceClient _spa;


    private PDMSService.PDMSServiceClient spa
    {
        get
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            return _spa;
        }
    }

    protected void ddlWebServiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            Page.Theme = "Modernization";
        }
        else
        {
            Page.Theme = "Default";
        }
    }

    private HttpWebRequest CreateWebRequest(string url, string action, string certificatePath, string body)
    {
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        webRequest.ContentType = "text/xml;charset=\"utf-8\"";
        webRequest.Accept = "text/xml";
        webRequest.Method = "POST";
        string certpATH = certificatePath; // "test.cer";

        X509Certificate X509client = X509Certificate.CreateFromCertFile(certpATH);
        webRequest.ClientCertificates.Add(X509client);
        var data = body;
        webRequest.ContentLength = data.Length;
        //webRequest.PreAuthenticate;
        return webRequest;

    }

    public void DataBindGridView()
    {
        gvQueryTransactionQueue.DataSource = null;
        gvQueryTransactionQueue.DataBind();
    }

    public void RefreshData(int transactionID)
    {
        int totalResultCount = 0;
        DataTable dt = GetData(transactionID);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvQueryTransactionQueue.EmptyDataText = "No Reports found.";
        }
        gvQueryTransactionQueue.DataSource = dt;
        gvQueryTransactionQueue.VirtualItemCount = totalResultCount;
        gvQueryTransactionQueue.DataBind();


    }

    private DataTable GetData(int transactionID)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;

        // If list of IDs has been passed in, display in search results list.
        ds = psc.SelectTransactionDetailsByTransactionID(transactionID);

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    protected void btnEnrollRequest_Click(object sender, EventArgs e)
    {
        int tranID = 0;
        if (!string.IsNullOrEmpty(txtTransactionID.Text))
        {
            tranID = Convert.ToInt32(txtTransactionID.Text);
        }
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
        }
        catch (XmlException exception)
        {
            txtResponse.Text = "Not a valid xml: " + exception.ToString();
        }
        makePMWSRequestResponse(xmlDoc, CON.Services.ProviderManagement, CON.ProviderManagementSoapAction.ProviderManagement);
    }

    protected void btnUpdateRequest_Click(object sender, EventArgs e)
    {
        int tranID = 0;
        if (!string.IsNullOrEmpty(txtTransactionID.Text))
        {
            tranID = Convert.ToInt32(txtTransactionID.Text);
        }
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
        }
        catch (XmlException exception)
        {
            txtResponse.Text = "Not a valid xml: " + exception.ToString();
        }
        makePMWSRequestResponse(xmlDoc, CON.Services.ProviderManagement, CON.ProviderManagementSoapAction.UpdateProviderManagement);
    }
    protected void btnGenerateAckReq_Click(object sender, EventArgs e)
    {
        string url = AppSettings.Get("AcknowledgementWSURL", CON.WebServiceURI.AcknowledgementService);
        string wsUserName = AppSettings.Get("AcknowledgementWSUserName");
        string client_secret = "";
        string secretName = "AcknowledgementWSPassword_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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

        Corp.Core.Libraries.AcknowledgmentService.MessageHeader ackMsgHeader = new Corp.Core.Libraries.AcknowledgmentService.MessageHeader();
        ackMsgHeader.BusinessFlow = "PartialProviderManagement";
        ackMsgHeader.RequestorSystem = "PNM";
        ackMsgHeader.RequestorSystemId = "PNM";
        ackMsgHeader.TargetSystem = txtAckTargetSys.Text;
        ackMsgHeader.ModuleTransactionId = txtTransactionID.Text;
        ackMsgHeader.AdditionalModuleTransactionId = "4321";
        ackMsgHeader.SITransactionKey = ProviderManagementHelper.GetUniqueKey(32);
        ackMsgHeader.TimeStamp = ProviderManagementHelper.GetStringDateTime(DateTime.Now);

        Corp.Core.Libraries.AcknowledgmentService.Response[] ackReqResponse = new Corp.Core.Libraries.AcknowledgmentService.Response[100];

        for (int i = 0; i < 1; i++)
        {
            ackReqResponse[i] = new Corp.Core.Libraries.AcknowledgmentService.Response();
            ackReqResponse[i].SequenceNumber = i.ToString();
            ackReqResponse[i].ResponseCode = "1001";
            ackReqResponse[i].ResponseType = "Success";
            ackReqResponse[i].ResponseMessage = "Success";
            i++;
        }

        vendorAcknowledgmentRequest1 ackRequest = new vendorAcknowledgmentRequest1();
        ackRequest.MessageHeader = ackMsgHeader;
        ackRequest.Response = ackReqResponse;

        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(vendorAcknowledgmentRequest1));
        var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        var settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.OmitXmlDeclaration = true;

        var stream2 = new StringWriter();
        var writer = XmlWriter.Create(stream2, settings);
        x.Serialize(writer, ackRequest, emptyNs);
        string xml = stream2.ToString();
        xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://mes.gov/acknowledgment\">"
            + Environment.NewLine + wssSecurityHeader(wsUserName, wsPassword) + Environment.NewLine + "<soapenv:Body>" +
            Environment.NewLine + xml;
        xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);

        txtGenXML.Text = xmlDoc.InnerXml.ToString();
    }
    protected void btnRequestAck_Click(object sender, EventArgs e)
    {
        string url = AppSettings.Get("AcknowledgementWSURL", CON.WebServiceURI.AcknowledgementService);
        string wsUserName = AppSettings.Get("AcknowledgementWSUserName");

        string client_secret = "";
        string secretName = "AcknowledgementWSPassword_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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

        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());            
        }
        catch (XmlException exception)
        {
            txtResponse.Text = "Not a valid xml: " + exception.ToString();
        }
        AcknowledgmentReqRes ack = new AcknowledgmentReqRes();
        var ackresponse = ack.makeWSRequestResponse(xmlDoc, url, wsUserName, wsPassword, ProviderManagementHelper.GetUniqueKey(32).ToString());   //ack.AcknowledgmentTargetVendorResponse(ackRequest);

        txtResponse.Text = ackresponse.ToString();

    }


    protected void btnGenerateXML_Click(object sender, EventArgs e)
    {
        int tranID = 0;
        if (!string.IsNullOrEmpty(txtTransactionID.Text))
        {
            tranID = Convert.ToInt32(txtTransactionID.Text);
            txtGenXML.Text = System.Xml.Linq.XElement.Parse(providerWSManagementEnrollRequest(tranID)).ToString();

            RefreshData(tranID);
        }

    }


    protected void btnGenerateUpdateXML_Click(object sender, EventArgs e)
    {
        int tranID = 0;
        if (!string.IsNullOrEmpty(txtTransactionID.Text))
        {
            tranID = Convert.ToInt32(txtTransactionID.Text);
            txtGenXML.Text = System.Xml.Linq.XElement.Parse(providerWSManagementUpdateRequest(tranID)).ToString();

            RefreshData(tranID);
        }
    }

    protected void btnGeneratePartialPSMXML_Click(object sender, EventArgs e)
    {
        int tranID = 0;
        if (!string.IsNullOrEmpty(txtTransactionID.Text))
        {
            tranID = Convert.ToInt32(txtTransactionID.Text);
            txtGenXML.Text = System.Xml.Linq.XElement.Parse(partialProviderManagementSubmitRequest(tranID, "PSM")).ToString();

            RefreshData(tranID);
        }
    }

    protected void btnGeneratePartialPCWXML_Click(object sender, EventArgs e)
    {
        int tranID = 0;
        if (!string.IsNullOrEmpty(txtTransactionID.Text))
        {
            tranID = Convert.ToInt32(txtTransactionID.Text);
            txtGenXML.Text = System.Xml.Linq.XElement.Parse(partialProviderManagementSubmitRequest(tranID, "PCW")).ToString();

            RefreshData(tranID);
        }
    }

    public Corp.Core.Libraries.PartialProviderManagementReference.InqMessageHeaderSubscriber[] sendToSubscriberService(string sendTo)
    {
        Corp.Core.Libraries.PartialProviderManagementReference.InqMessageHeaderSubscriber[] imq = new Corp.Core.Libraries.PartialProviderManagementReference.InqMessageHeaderSubscriber[1];

        switch (sendTo)
        {
            case CON.TransactionTypeValues.PSM_Partial:
                imq[0] = Corp.Core.Libraries.PartialProviderManagementReference.InqMessageHeaderSubscriber.PSM;
                return imq;
            case CON.TransactionTypeValues.PSM_FULL:
                imq[0] = Corp.Core.Libraries.PartialProviderManagementReference.InqMessageHeaderSubscriber.PSM;
                return imq;
            case CON.TransactionTypeValues.PCW_Partial:
                imq[0] = Corp.Core.Libraries.PartialProviderManagementReference.InqMessageHeaderSubscriber.PCW;
                return imq;
            case CON.TransactionTypeValues.PCW_Full:
                imq[0] = Corp.Core.Libraries.PartialProviderManagementReference.InqMessageHeaderSubscriber.PCW;
                return imq;
            case CON.PartialProviderSubscriberSystems.PSM:
                imq[0] = Corp.Core.Libraries.PartialProviderManagementReference.InqMessageHeaderSubscriber.PSM;
                return imq;
            case CON.PartialProviderSubscriberSystems.PCW:
                imq[0] = Corp.Core.Libraries.PartialProviderManagementReference.InqMessageHeaderSubscriber.PCW;
                return imq;
        }
        return null;
    }

    public string partialProviderManagementSubmitRequest(int transactionID, string sendTo, bool makeRequest = false)
    {
        try
        {

            DataSet ds = InfoAccessController.GetStagingProviderEnrollmentByTransactionID(transactionID);
            if (ds == null)
            {
                return "The data hasn't been staged for this TransactionID: " + transactionID.ToString();
            }
            string partialProviderManagementWSURL = AppSettings.Get("PartialProviderManagementWSURL", CON.WebServiceURI.PartialProviderManagement);
            string providerManagementWSUserName = AppSettings.Get("ProviderManagementWSUserName");
            string client_secret = "";
            string secretName = "ProviderManagementWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string providerManagementWSPassword = client_secret;
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            Corp.Core.Libraries.PartialProviderManagementReference.SubmitPartialProviderRequestPayload enSubmitPartialProviderRequestPayload = new Corp.Core.Libraries.PartialProviderManagementReference.SubmitPartialProviderRequestPayload();
            enSubmitPartialProviderRequestPayload.ProviderInformation = new Corp.Core.Libraries.PartialProviderManagementReference.SubmitPartialProviderInformation
            {
                ProviderDemographics = PartialProviderManagementController.fillProviderDemographic(ds.Tables[6]),
                ProviderApplication = PartialProviderManagementController.fillenrollProviderApplication(ds.Tables[8]),
                ProviderAddress = PartialProviderManagementController.fillProviderAddress(ds.Tables[0]),
                ProviderTaxonomyClassification = PartialProviderManagementController.fillTaxonomyClassification(ds.Tables[19]),
                ProviderType = PartialProviderManagementController.fillProviderType(ds.Tables[20]),
                OwnerRelationship = PartialProviderManagementController.fillOwnerRelationshipLists(ds.Tables[1]),
                ProviderAffiliations = PartialProviderManagementController.fillAffiliationList(ds.Tables[2]),
                ProviderAlternateIdentifiers = PartialProviderManagementController.fillAlternateIdList(ds.Tables[3]),
                ProviderEFTEnrollment = PartialProviderManagementController.fillEFTEnrollmentList(ds.Tables[4]),
                ProviderLanguage = PartialProviderManagementController.fillLanguageList(ds.Tables[5]),

                ProviderAttestations = PartialProviderManagementController.fillProviderAttestationsListAttestations(ds.Tables[9]),
                ProviderBusinessStatus = PartialProviderManagementController.fillProviderBusinessStatusListBusinessStatus(ds.Tables[10]),
                ProviderCHOP = PartialProviderManagementController.fillProviderCHOPListCHOP(ds.Tables[11]),
                ProviderContact = PartialProviderManagementController.fillProviderContactListContact(ds.Tables[12]),
                ProviderManagedEmployees = PartialProviderManagementController.fillProviderManagedEmployeesListManagedEmployee(ds.Tables[13]),
                ProviderOwnerships = PartialProviderManagementController.fillProviderOwnershipList(ds.Tables[31]),
                ProviderProgramAffiliations = PartialProviderManagementController.fillProgramAffiliationsListProgramAffiliation(ds.Tables[15]),
                ProviderReviews = PartialProviderManagementController.fillProviderReviewsListReview(ds.Tables[16]),
                ProviderServiceLocation = PartialProviderManagementController.fillEnrollProviderServiceLocation(ds),
                ProviderServices = PartialProviderManagementController.fillProviderServicesListServices(ds.Tables[18]),
                ProviderDocuments = PartialProviderManagementController.fillPartialProviderDocumentsList(ds.Tables[7])

            };
            
            Corp.Core.Libraries.PartialProviderManagementReference.MessageHeader msgHeader = new Corp.Core.Libraries.PartialProviderManagementReference.MessageHeader();
            msgHeader.BusinessFlow = "SubmitPartialProvider";
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = Corp.Core.Libraries.PartialProviderManagementReference.InqMessageHeaderRequestorSystem.PNM; ;
            msgHeader.ModuleTransactionId = transactionID.ToString();
            msgHeader.SubscriberSystem = sendToSubscriberService(sendTo);
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = ProviderManagementHelper.GetStringDateTime(DateTime.Now);
            //msgHeader.SITransactionKey = sitTransactionKey;

            Corp.Core.Libraries.PartialProviderManagementReference.submitPartialProviderRequest epiPM = new Corp.Core.Libraries.PartialProviderManagementReference.submitPartialProviderRequest();
            epiPM.MessageHeader = msgHeader;
            epiPM.Payload = enSubmitPartialProviderRequestPayload;

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.PartialProviderManagementReference.submitPartialProviderRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, epiPM, emptyNs);
            string xml = stream2.ToString();
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://mes.gov/partialprovidermanagement\">"
                + Environment.NewLine + wssSecurityHeader(providerManagementWSUserName, providerManagementWSPassword) + Environment.NewLine + "<soapenv:Body>" +
                Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return xmlDoc.InnerXml.ToString();
        }
        catch (Exception ex)
        {
            return "Something wrong with generating the xml : " + ex.ToString();
        }
    }

    public string providerWSManagementEnrollRequest(int transactionID)
    {
        try
        {
            DataSet ds = InfoAccessController.GetStagingProviderEnrollmentByTransactionID(transactionID);

            if (ds == null)
            {
                return "The data hasn't been staged for this TransactionID: " + transactionID.ToString();
            }

            string providerManagementWSURL = AppSettings.Get("ProviderManagementWSURL", CON.WebServiceURI.ProviderManagement);
            string providerManagementWSUserName = AppSettings.Get("ProviderManagementWSUserName");
            string client_secret = "";
            string secretName = "ProviderManagementWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string providerManagementWSPassword = client_secret;

            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            Corp.Core.Libraries.ProviderManagementReference.EnrollRequestPayload enrollRequestPayload = new Corp.Core.Libraries.ProviderManagementReference.EnrollRequestPayload();
            enrollRequestPayload.ProviderInformation = new Corp.Core.Libraries.ProviderManagementReference.EnrollProviderInformation
            {
                ProviderDemographics = ProviderManagementController.fillProviderDemographic(ds.Tables[6]),
                ProviderAddress = ProviderManagementController.fillProviderAddress(ds.Tables[0]),
                ProviderTaxonomyClassification = ProviderManagementController.fillTaxonomyClassification(ds.Tables[19]),
                ProviderType = ProviderManagementController.fillProviderType(ds.Tables[20]),
                OwnerRelationship = ProviderManagementController.fillOwnerRelationshipLists(ds.Tables[1]),
                ProviderAffiliations = ProviderManagementController.fillAffiliationList(ds.Tables[2]),
                ProviderAlternateIdentifiers = ProviderManagementController.fillAlternateIdList(ds.Tables[3]),
                ProviderEFTEnrollment = ProviderManagementController.fillEFTEnrollmentList(ds.Tables[4]),
                ProviderLanguage = ProviderManagementController.fillLanguageList(ds.Tables[5]),
                ProviderApplication = ProviderManagementController.fillenrollProviderApplication(ds.Tables[8]),
                ProviderAttestations = ProviderManagementController.fillProviderAttestationsListAttestations(ds.Tables[9]),
                ProviderBusinessStatus = ProviderManagementController.fillProviderBusinessStatusListBusinessStatus(ds.Tables[10]),
                ProviderCHOP = ProviderManagementController.fillProviderCHOPListCHOP(ds.Tables[11]),
                ProviderContact = ProviderManagementController.fillProviderContactListContact(ds.Tables[12]),
                ProviderManagedEmployees = ProviderManagementController.fillProviderManagedEmployeesListManagedEmployee(ds.Tables[13]),
                ProviderOwnerships = ProviderManagementController.fillProviderOwnershipList(ds.Tables[31]),
                ProviderProgramAffiliations = ProviderManagementController.fillProgramAffiliationsListProgramAffiliation(ds.Tables[15]),
                ProviderReviews = ProviderManagementController.fillProviderReviewsListReview(ds.Tables[16]),
                ProviderServiceLocation = ProviderManagementController.fillEnrollProviderServiceLocation(ds),
                ProviderServices = ProviderManagementController.fillProviderServicesListServices(ds.Tables[18])
            };
            Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderSubscriber[] imq = new Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderSubscriber[1];
            imq[0] = Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderSubscriber.MITS;
            Corp.Core.Libraries.ProviderManagementReference.MessageHeader msgHeader = new Corp.Core.Libraries.ProviderManagementReference.MessageHeader();
            msgHeader.BusinessFlow = "EnrollProvider";
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderRequestorSystem.PNM;
            msgHeader.ModuleTransactionId = transactionID.ToString();
            msgHeader.SubscriberSystem = imq;
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = ProviderManagementHelper.GetStringDateTime(DateTime.Now);
            msgHeader.SITransactionKey = sitTransactionKey;

            Corp.Core.Libraries.ProviderManagementReference.enrollProviderRequest epiPM = new Corp.Core.Libraries.ProviderManagementReference.enrollProviderRequest();
            epiPM.MessageHeader = msgHeader;
            epiPM.Payload = enrollRequestPayload;

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.ProviderManagementReference.enrollProviderRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, epiPM, emptyNs);
            string xml = stream2.ToString();

            string wsHeader = wssSecurityHeader(providerManagementWSUserName, providerManagementWSPassword);
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://mes.gov/providermanagement\">"
                + Environment.NewLine + wsHeader + Environment.NewLine + "<soapenv:Body>" +
                Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            return xmlDoc.InnerXml.ToString();
        }
        catch (Exception ex)
        {
            return "Something wrong with generating the xml : " + ex.ToString();
        }
    }


    protected void btnGetTransaction_Click(object sender, EventArgs e)
    {
        int RegID = 0;
        if (!string.IsNullOrEmpty(txtRegID.Text))
        {
            RegID = Convert.ToInt32(txtRegID.Text);
            DataSet ds = spa.SelectTransactionIDsByRegID(RegID);
            if (Helper.HasRows(ds))
            {
                txtTxnResult.Text = ds.Tables[0].Rows[0]["TransactionQueueIDs"].ToString();
            }
        }
    }
    /*
    public static string partialProviderManagementSubmitRequest(int transactionID)
    {
        DataSet ds = new DataSet();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        ds = psc.GetStagingProviderEnrollmentData(transactionID);
        string partialProviderManagementWSURL = AppSettings.Get("PartialProviderManagementWSURL", CON.WebServiceURI.PartialProviderManagement);
        string providerManagementWSUserName = AppSettings.Get("ProviderManagementWSUserName");
        string client_secret = "";
        string secretName = "ProviderManagementWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string providerManagementWSPassword = client_secret;
        string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);



        PartialProviderManagementReqRes.SubmitPartialProviderRequestPayload enSubmitPartialProviderRequestPayload = new PartialProviderManagementReqRes.SubmitPartialProviderRequestPayload();
        enSubmitPartialProviderRequestPayload.ProviderInformation = new PartialProviderManagementReqRes.SubmitPartialProviderInformation
        {
            ProviderDemographics = PartialProviderManagementController.fillProviderDemographic(ds.Tables[6]),
            ProviderApplication = PartialProviderManagementController.fillenrollProviderApplication(ds.Tables[8])
             ,
            ProviderAddress = PartialProviderManagementController.fillProviderAddress(ds.Tables[0]),
            ProviderTaxonomyClassification = PartialProviderManagementController.fillTaxonomyClassification(ds.Tables[19]),
            ProviderType = PartialProviderManagementController.fillProviderType(ds.Tables[20]),
            OwnerRelationship = PartialProviderManagementController.fillOwnerRelationshipLists(ds.Tables[1]),
            ProviderAffiliations = PartialProviderManagementController.fillAffiliationList(ds.Tables[2]),
            ProviderAlternateIdentifiers = PartialProviderManagementController.fillAlternateIdList(ds.Tables[3]),
            ProviderEFTEnrollment = PartialProviderManagementController.fillEFTEnrollmentList(ds.Tables[4]),
            ProviderLanguage = PartialProviderManagementController.fillLanguageList(ds.Tables[5]),

            ProviderAttestations = PartialProviderManagementController.fillProviderAttestationsListAttestations(ds.Tables[9]),
            ProviderBusinessStatus = PartialProviderManagementController.fillProviderBusinessStatusListBusinessStatus(ds.Tables[10]),
            ProviderCHOP = PartialProviderManagementController.fillProviderCHOPListCHOP(ds.Tables[11]),
            ProviderContact = PartialProviderManagementController.fillProviderContactListContact(ds.Tables[12]),
            ProviderManagedEmployees = PartialProviderManagementController.fillProviderManagedEmployeesListManagedEmployee(ds.Tables[13]),
            ProviderOwnerships = PartialProviderManagementController.fillProviderOwnershipList(ds.Tables[31]),
            ProviderProgramAffiliations = PartialProviderManagementController.fillProgramAffiliationsListProgramAffiliation(ds.Tables[15]),
            ProviderReviews = PartialProviderManagementController.fillProviderReviewsListReview(ds.Tables[16]),
            ProviderServiceLocation = PartialProviderManagementController.fillEnrollProviderServiceLocation(ds),
            ProviderServices = PartialProviderManagementController.fillProviderServicesListServices(ds.Tables[18])
            
};

        PartialProviderManagementReqRes.MessageHeader msgHeader = new PartialProviderManagementReqRes.MessageHeader();
        msgHeader.BusinessFlow = "SubmitPartialProvider";
        msgHeader.StateCode = "OH";
        msgHeader.RequestorSystem = "PSM";
        msgHeader.ModuleTransactionId = transactionID.ToString();
        msgHeader.SubscriberSystem = "PNM";
        msgHeader.AdditionalModuleTransactionId = string.Empty;
        msgHeader.RequestTimestamp = ProviderManagementHelper.GetStringDateTime(DateTime.Now);
        msgHeader.SITransactionKey = sitTransactionKey;

        PartialProviderManagementReqRes.submitPartialProviderRequest epiPM = new PartialProviderManagementReqRes.submitPartialProviderRequest();
        epiPM.MessageHeader = msgHeader;
        epiPM.Payload = enSubmitPartialProviderRequestPayload;

        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(PartialProviderManagementReqRes.submitPartialProviderRequest));
        var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        var settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.OmitXmlDeclaration = true;

        var stream2 = new StringWriter();
        var writer = XmlWriter.Create(stream2, settings);
        x.Serialize(writer, epiPM, emptyNs);
        string xml = stream2.ToString();
        xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://mes.gov/partialprovidermanagement\">"
            + Environment.NewLine + wssSecurityHeader(providerManagementWSUserName, providerManagementWSPassword) + Environment.NewLine + "<soapenv:Body>" +
            Environment.NewLine + xml;
        xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        return makeWSRequestResponse(transactionID, xmlDoc, partialProviderManagementWSURL, CON.Services.PartialProviderManagement, providerManagementWSUserName, providerManagementWSPassword,
            CON.ProviderManagementSoapAction.PartialProviderManagement
            );
    }
    */

    public static string wssSecurityHeader(string providerManagementWSUserName, string providerManagementWSPassword)
    {
        try
        {
            UsernameToken usernameTokenSection = new UsernameToken(providerManagementWSUserName, providerManagementWSPassword, PasswordOption.SendPlainText);
            string xml = "<soapenv:Header>" +
                @"<wsse:Security xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">" +
                    usernameTokenSection.GetXml(new XmlDocument()).OuterXml.ToString().Replace("<wsse:Nonce", "<wsse:Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\"") +
                "</wsse:Security>" +
              "</soapenv:Header>";
            return xml;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public void makePMWSRequestResponse(XmlDocument xmlDoc, string service, string soapAction)
    {
        try
        {
            string providerManagementWSURL = AppSettings.Get("ProviderManagementWSURL", CON.WebServiceURI.ProviderManagement);
            string providerManagementWSUserName = AppSettings.Get("ProviderManagementWSUserName");
            string client_secret = "";
            string secretName = "ProviderManagementWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string providerManagementWSPassword = client_secret;

            string providerManagementWSCertificateName = AppSettings.Get("ProviderManagementWSCertificateName");
            string ModuleTransactionId = string.Empty;
            string SITransactionKey = string.Empty;
            string ResponseCode = string.Empty;
            string ResponseDetails = string.Empty;
            string ResponseMessage = string.Empty;
            string ResponseType = string.Empty;
            object result;

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            CertUtil _certs = new CertUtil();
            X509Certificate2 clientCertificate = _certs.GetCertificateByName(providerManagementWSCertificateName);
            string user = providerManagementWSUserName;
            string passcode = providerManagementWSPassword;
            Uri apiUrl = new Uri(providerManagementWSURL);
            WebRequest pmRequest = HttpWebRequest.Create(providerManagementWSURL);
            HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
            pmHttpRequest.ContentType = "text/xml; charset=utf-8";

            pmHttpRequest.KeepAlive = true;
            pmHttpRequest.Method = "POST";
            pmHttpRequest.SendChunked = true;
            pmHttpRequest.UserAgent = ".NET Framework";
            pmHttpRequest.Host = "dp.test.oh.healthinteractive.net:443";
            pmHttpRequest.ClientCertificates.Add(clientCertificate);
            String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + passcode));
            pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
            pmHttpRequest.Headers.Add("SOAPAction", soapAction);
            Stream requestStream = pmHttpRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            var response = "";

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

            txtResponse.Text = response.ToString();


        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                var response = ex.Response as HttpWebResponse;
                if (response != null)
                {
                    //http status code avaliable
                    var respBody = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    txtResponse.Text = respBody.ToString();
                }
                else
                {
                    // no http status code available
                    txtResponse.Text = "Response is null, unable to make connection";
                }
            }
            else
            {
                // no http status code available
                txtResponse.Text = "Response is null, unable to make connection";
            }

        }
        catch (Exception ex)
        {
            txtResponse.Text = "Error: " + ex.ToString();
        }
    }


    public static string makeWSRequestResponse(int transactionID, XmlDocument xmlDoc, string providerManagementWSURL, string service, string providerManagementWSUserName, string providerManagementWSPassword,
    string soapAction)
    {
        try
        {
            string providerManagementWSCertificateName = AppSettings.Get("ProviderManagementWSCertificateName");
            CertUtil _certs = new CertUtil();
            X509Certificate2 clientCertificate = _certs.GetCertificateByName(providerManagementWSCertificateName);
            string user = providerManagementWSUserName;
            string passcode = providerManagementWSPassword;
            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            Uri apiUrl = new Uri(providerManagementWSURL);
            WebRequest pmRequest = HttpWebRequest.Create(providerManagementWSURL);
            HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
            pmHttpRequest.ContentType = "text/xml; charset=utf-8";

            pmHttpRequest.KeepAlive = true;
            //eVerificationRequest.ContentType = "text/xml; encoding='utf-8'";
            pmHttpRequest.Method = "POST";
            pmHttpRequest.SendChunked = true;
            pmHttpRequest.UserAgent = ".NET Framework";
            pmHttpRequest.Host = "dp.test.oh.healthinteractive.net:443";
            pmHttpRequest.ClientCertificates.Add(clientCertificate);
            String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes("pnmhttpssit" + ":" + "vd6cL3/hvWOXu0FZSoWluUxKj457hHmF6VSqmhi4rwg="));
            pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
            pmHttpRequest.Headers.Add("SOAPAction", "\"http://mes.gov/submitPartialProviderReqRes\"");
            Stream requestStream = pmHttpRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            var response = "";

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
            return response.ToString();
        }
        catch (WebException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    
    protected void btnGenerateClaimsSearchXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateClaimsSearchRequestXml()).ToString();
    }
    protected void btnGenerateClaimsInquiryXML_Click(object sender, EventArgs e)
    {
       txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateClaimsInquiryRequestXml()).ToString();
    }
    protected void btnGenerateClaimsAddUpdateXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateClaimsAddUpdateRequestXml()).ToString();
    }
    protected void btnGenerateClaimsInquiryProsessionalXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateClaimsProfessionalInquiryRequestXml()).ToString();
    }
    protected void btnGenerateClaimsAddUpdateProsessionalXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateClaimsAddUpdateProfRequestXml()).ToString();
    }
    protected void btnGenerateClaimsInquiryInstXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateClaimsInstitionalInquiryRequestXml()).ToString();
    }
    protected void btnGenerateClaimsAddUpdateInstXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateClaimsAddUpdateInstRequestXml()).ToString();
    }
    protected void btnGenerateCostReportDueDateSearchXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateCostReportDueDateSearchRequestXml()).ToString();
    }
    protected void btnGenerateCostReportSearchXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateCostReportSearchRequestXml()).ToString();
    }
    protected void btnGenerateCostReportSettlementXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateCostReportSettlementRequestXml()).ToString();
    }
    protected void btnGenerateCostReportSubmitXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateCostReportSubmitRequestXml()).ToString();
    }

    protected void btnGenerateCreateAuthXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateCreateAuthRequestXml()).ToString();
    }
    protected void btnGenerateInquireAuthXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateInquireAuthRequestXml()).ToString();
    }
    protected void btnGenerateSearchAuthXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateSearchAuthRequestXml()).ToString();
    }
    protected void btnGenerateUpdateAuthXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateUpdateAuthRequestXml()).ToString();
    }

    protected void btnGenerateHospiceXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateHospiceXml()).ToString();
    }



    protected void btnLTCHomeProviderFileClean_Click(object sender, EventArgs e)
    {
        try
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("OPS", DbType.Int32, 1, true));
            DataAccess.ExecuteStoredProcedure("usp_LTCHomeProviderFileCleanUp", parameters, "LTCHomeProviderFileClean");
            txtResponse.Text = "Executed Successfully";
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Error: {0} {1}", ex.Message.ToString(), ex.StackTrace.ToString());
        }
    }

    protected void btnLTCHomeFileClean_Click(object sender, EventArgs e)
    {
        try
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("REG_IDS", DbType.String, txtRegID.Text, true));
            parameters.Add(SqlParms.CreateParameter("OPS", DbType.Int32, 2, true));
            DataAccess.ExecuteStoredProcedure("usp_LTCHomeProviderFileCleanUp", parameters, "LTCHomeProviderFileClean");
            txtResponse.Text = "Executed Successfully";
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Error: {0} {1}", ex.Message.ToString(), ex.StackTrace.ToString());
        }
    }

    protected void btnLTCProviderFileClean_Click(object sender, EventArgs e)
    {
        try
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("REG_IDS", DbType.String, txtRegID.Text, true));
            parameters.Add(SqlParms.CreateParameter("OPS", DbType.Int32, 3, true));
            DataAccess.ExecuteStoredProcedure("usp_LTCHomeProviderFileCleanUp", parameters, "LTCHomeProviderFileClean");
            txtResponse.Text = "Executed Successfully";
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Error: {0} {1}", ex.Message.ToString(), ex.StackTrace.ToString());
        }
    }

    protected void btnProcess_Click(object sender, EventArgs e)
    {
        try
        {

            //byte[] obj = Encoding.UTF8.GetBytes(txtGenXML.Text);
            byte[] obj = Convert.FromBase64String(txtGenXML.Text);
            using (SqlConnection conn = new SqlConnection(AppSettings.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_RetrieveGenericAutoReportsDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@REPORT_DETAILS", SqlDbType.VarBinary)
                    {
                        Value = obj
                    });
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            txtResponse.Text = "Process Executed Successfully";
        }
        catch(Exception ex)
        {
            txtResponse.Text = string.Format("Error while generating the xml: {0} {1}", ex.Message.ToString(), ex.StackTrace.ToString());
        }
    }
    protected void btnGenerateHospiceInquireRequestXML_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateHospiceInquireRequestXml()).ToString();
    }
    protected void btnGenerateHospiceAddUpdateXml_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateHospiceAddUpdateXml()).ToString();
    }
    protected void btnGenerateDocumentXml_Click(object sender, EventArgs e)
    {
        txtGenXML.Text = System.Xml.Linq.XElement.Parse(GenerateDocumentRequestXml()).ToString();
    }
    protected void btnHospiceInquire_Click(object sender, EventArgs e)
    {
        string hospiceWSURL = AppSettings.Get("HospiceWSURL", CON.WebServiceURI.HospiceService);
        string wSUserName = AppSettings.Get("HospiceWSUserName");
        string client_secret = "";
        string secretName = "HospiceWSPassword_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword = client_secret;
        client_secret = "";
        secretName = "HospiceWSPassword_2_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword_2 = client_secret;
        string wsCertificateName = AppSettings.Get("HospiceWSCertificateName");
        string wsHost = AppSettings.Get("HospiceWSHost");
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
        }
        catch (XmlException exception)
        {
            txtResponse.Text = "Not a valid xml: " + exception.ToString();
        }
        MakeServiceCall(xmlDoc, hospiceWSURL, "\"/Processes/Hospice/Starter/HospiceService.serviceagent/HospiceEndpoint/" +
            "InquireHospice\"", wSUserName, wSPassword_2, wsCertificateName, wsHost);
    }
    protected void btnHospiceSearch_Click(object sender, EventArgs e)
    {
        string hospiceWSURL = AppSettings.Get("HospiceWSURL", CON.WebServiceURI.HospiceService);
        string wSUserName = AppSettings.Get("HospiceWSUserName");
        string client_secret = "";
        string secretName = "HospiceWSPassword_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword = client_secret;
        client_secret = "";
        secretName = "HospiceWSPassword_2_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword_2 = client_secret;
        string wsCertificateName = AppSettings.Get("HospiceWSCertificateName");
        string wsHost = AppSettings.Get("HospiceWSHost");
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
        }
        catch (XmlException exception)
        {
            txtResponse.Text = "Not a valid xml: " + exception.ToString();
        }
        MakeServiceCall(xmlDoc, hospiceWSURL, "\"/Processes/Hospice/Starter/HospiceService.serviceagent/HospiceEndpoint" +
            "/SearchHospice\"", wSUserName, wSPassword_2, wsCertificateName, wsHost);
    }
    protected void btnClaimsSearch_Click(object sender, EventArgs e)
    {
        Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
        string wsURL = AppSettings.Get("ClaimsWSURL", CON.WebServiceURI.ClaimsService);
       
        string wSUserName = AppSettings.Get("ClaimsWSUserName");
        string client_secret = "";
        string secretName = "ClaimsWSPassword_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword = client_secret;
        client_secret = "";
        secretName = "ClaimsWSPassword_2_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword2 = client_secret;
        string response = string.Empty;

        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
            response = MakeClaimsServiceCall(xmlDoc, wsURL, "http://service.operationmgmt.fi/ClaimsService/SearchClaims", wSUserName, wSPassword2);
            txtResponse.Text = response;
            int passthroughId = (int)PassthroughController.PassthroughTransactionType.ClaimSearch;
         
            PassthroughController.InsertPassthroughTransactionQueue(passthroughId, "", "", "", xmlDoc.InnerXml.ToString(), response, "PNM", "", "",
                DateTime.Now, null, null, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(), 0);

        }
        catch (XmlException exception)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", exception.Message, exception.InnerException, exception.StackTrace.ToString()), Logging.LogPriority.Error);

            txtResponse.Text = response + "Not a valid xml: " + exception.ToString();
        }
        catch(Exception ex)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", ex.Message, ex.InnerException, ex.StackTrace.ToString()), Logging.LogPriority.Error);

            txtResponse.Text = response + "Error in ClaimsSerach Call " + ex.ToString();

        }
       
      

    }
    protected void btnClaimsAddUpdate_Click(object sender, EventArgs e)
    {
        Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
        string wsURL = AppSettings.Get("ClaimsSubmissionWSURL");
        string wSUserName = AppSettings.Get("ClaimsWSUserName");
        string client_secret = "";
        string secretName = "ClaimsWSPassword_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword = client_secret;
        client_secret = "";
        secretName = "ClaimsWSPassword_2_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword2 = client_secret;
        string claimsCertName = AppSettings.Get("ClaimsServiceWSCertificateName");
        XmlDocument xmlDoc = new XmlDocument();
        string response = string.Empty;
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
            response = MakeClaimsServiceCall(xmlDoc, wsURL, "http://service.operationmgmt.fi/ClaimsService/dental/AddUpdateClaims", wSUserName, wSPassword2);
            txtResponse.Text = response;
            int passthroughId = (int)PassthroughController.PassthroughTransactionType.ClaimAddUpdate;
            PassthroughController.InsertPassthroughTransactionQueue(passthroughId, "", "", "", xmlDoc.InnerXml.ToString(), response, "PNM", "", "",
                DateTime.Now, null, null, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(), 0);
           

        }
        catch (XmlException exception)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", exception.Message, exception.InnerException, exception.StackTrace.ToString()), Logging.LogPriority.Error);

            txtResponse.Text = response+ "Not a valid xml: " + exception.ToString();
        }
        catch (Exception ex)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", ex.Message, ex.InnerException, ex.StackTrace.ToString()), Logging.LogPriority.Error);

            txtResponse.Text = response + "Error in Dental Add Update Call " + ex.ToString();

        }
    }
    protected void btnClaimsProfessionalAddUpdate_Click(object sender, EventArgs e)
    {
        Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
        string wsURL =AppSettings.Get("ClaimsSubmissionProfessionalWSURL");
        string wSUserName = AppSettings.Get("ClaimsWSUserName");
        string client_secret = "";
        string secretName = "ClaimsWSPassword_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword = client_secret;
        client_secret = "";
        secretName = "ClaimsWSPassword_2_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword2 = client_secret;
        XmlDocument xmlDoc = new XmlDocument();
        string response = string.Empty;
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
            response = MakeClaimsServiceCall(xmlDoc, wsURL, "http://service.operationmgmt.fi/ClaimsService/professional/AddUpdateClaims", wSUserName, wSPassword2);
            txtResponse.Text = response;
            int passthroughId = (int)PassthroughController.PassthroughTransactionType.ClaimAddUpdate;
            PassthroughController.InsertPassthroughTransactionQueue(passthroughId, "", "", "", xmlDoc.InnerXml.ToString(), response, "PNM", "", "", DateTime.Now, null, null, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(), 1);
            
        }
        catch (XmlException ex)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", ex.Message, ex.InnerException, ex.StackTrace.ToString()), Logging.LogPriority.Error);

            txtResponse.Text = response + "Not a valid xml: " + ex.ToString();
        }
        catch (Exception ex)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", ex.Message, ex.InnerException, ex.StackTrace.ToString()), Logging.LogPriority.Error);

            txtResponse.Text = response + "Error in Professional Add Update Call " + ex.ToString();

        }
    }
    protected void btnClaimsInstituationalAddUpdate_Click(object sender, EventArgs e)
    {
        Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
        string wsURL = AppSettings.Get("ClaimsSubmissionInstitutionalWSURL");
        string wSUserName = AppSettings.Get("ClaimsWSUserName");
        string client_secret = "";
        string secretName = "ClaimsWSPassword_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword = client_secret;
        client_secret = "";
        secretName = "ClaimsWSPassword_2_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword2 = client_secret;
        XmlDocument xmlDoc = new XmlDocument();
        string response = string.Empty;
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
            response = MakeClaimsServiceCall(xmlDoc, wsURL, "http://service.operationmgmt.fi/ClaimsService/institutional/AddUpdateClaims", wSUserName, wSPassword2);
            txtResponse.Text = response;
            int passthroughId = (int)PassthroughController.PassthroughTransactionType.ClaimAddUpdate;
            PassthroughController.InsertPassthroughTransactionQueue(passthroughId, "", "", "", xmlDoc.InnerXml.ToString(), response, "PNM", "", "", DateTime.Now, null, null, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(), 2);
           
        }
        catch (XmlException exception)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", exception.Message, exception.InnerException, exception.StackTrace.ToString()), Logging.LogPriority.Error);

            txtResponse.Text = response + "Not a valid xml: " + exception.ToString();
        }
        catch (Exception ex)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", ex.Message, ex.InnerException, ex.StackTrace.ToString()), Logging.LogPriority.Error);

            txtResponse.Text = response + "Error in Institutional Add Update Call " + ex.ToString();

        }
       
    }
    protected void btnMemberEligibilitySearch_Click(object sender, EventArgs e)
    {
        XmlDocument xmlDoc = new XmlDocument();
        Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
            Guid userId = Helper.GetUserId(HttpContext.Current.User.ToString());
            RecipientEligibilitySearchReqRes resrr = new RecipientEligibilitySearchReqRes();
            var response = resrr.eligibilityWSRequestResponse("100347436699", "", userId, xmlDoc, "http://service.membermgmt.fi/MemberEligibilityInquiryService/InquireMemberEligibility", "rZClsqDdTQy4v36zNHpBRXo71nU8hfYM");
            txtResponse.Text = response.ToString();
        }
        catch (XmlException ex)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", ex.Message, ex.InnerException, ex.StackTrace.ToString()), Logging.LogPriority.Error);
            txtResponse.Text = "Not a valid xml: " + ex.ToString();
        }
        catch (FaultException ex)
        {
            log.CreateLogEntry(string.Format("Fault Exception Message : {0}", ex.ToString()), Logging.LogPriority.Error);
            txtResponse.Text = "Error in Member Eligibility service call - " + ex.ToString();
        }
        catch (Exception ex)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", ex.Message, ex.InnerException, ex.StackTrace.ToString()), Logging.LogPriority.Error);
            txtResponse.Text = "Error in Member Eligibility service call " + ex.ToString();
        }
    }
    protected void btnClaimsInquire_Click(object sender, EventArgs e)
    {
        Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
        string wsURL = AppSettings.Get("ClaimsInquireWSURL");
        string wSUserName = AppSettings.Get("ClaimsWSUserName");
        string client_secret = "";
        string secretName = "ClaimsWSPassword_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword = client_secret;
        client_secret = "";
        secretName = "ClaimsWSPassword_2_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword2 = client_secret;
        string claimsCertName = AppSettings.Get("ClaimsServiceWSCertificateName");
        XmlDocument xmlDoc = new XmlDocument();
        string response = string.Empty;
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());

            response = MakeClaimsServiceCall(xmlDoc, wsURL, "http://service.operationmgmt.fi/ClaimsService/dental/InquireClaim", wSUserName, wSPassword2);
            txtResponse.Text = response;
            int passthroughId = (int)PassthroughController.PassthroughTransactionType.ClaimInquiry;
            PassthroughController.InsertPassthroughTransactionQueue(passthroughId, "", "", "", xmlDoc.InnerXml.ToString(), response, "PNM", "", "", DateTime.Now, null, null, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(), 0);
          

        }
        catch (XmlException exception)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", exception.Message, exception.InnerException, exception.StackTrace.ToString()), Logging.LogPriority.Error);

            txtResponse.Text = response +  "Not a valid xml: " + exception.ToString();
        }
        catch (Exception ex)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", ex.Message, ex.InnerException, ex.StackTrace.ToString()), Logging.LogPriority.Error);

            txtResponse.Text = response + "Error in Dental inquire call " + ex.ToString();

        }

    }
    protected void btnClaimsProfessionalInquire_Click(object sender, EventArgs e)
    {
        Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
        string wsURL = AppSettings.Get("ClaimsInquireProfessionalWSURL");
        string wSUserName = AppSettings.Get("ClaimsWSUserName");
        string client_secret = "";
        string secretName = "ClaimsWSPassword_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword = client_secret;
        client_secret = "";
        secretName = "ClaimsWSPassword_2_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword2 = client_secret;
        XmlDocument xmlDoc = new XmlDocument();
        string response = string.Empty;
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());

            response = MakeClaimsServiceCall(xmlDoc, wsURL, "http://service.operationmgmt.fi/ClaimsService/professional/InquireClaim", wSUserName, wSPassword2);
            txtResponse.Text = response;
            int passthroughId = (int)PassthroughController.PassthroughTransactionType.ClaimInquiry;
            PassthroughController.InsertPassthroughTransactionQueue(passthroughId, "", "", "", xmlDoc.InnerXml.ToString(), response, "PNM", "", "", DateTime.Now, null, null, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(), 1);
            

        }
        catch (XmlException exception)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", exception.Message, exception.InnerException, exception.StackTrace.ToString()), Logging.LogPriority.Error);

            txtResponse.Text = response+ "Not a valid xml: " + exception.ToString();
        }
        catch (Exception ex)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", ex.Message, ex.InnerException, ex.StackTrace.ToString()), Logging.LogPriority.Error);

            txtResponse.Text = response + "Error in professional inquire Call " + ex.ToString();

        }

    }
    protected void btnClaimsInstituationalInquire_Click(object sender, EventArgs e)
    {
        Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
        string wsURL =AppSettings.Get("ClaimsInquireInstitutionalWSURL");
        string wSUserName = AppSettings.Get("ClaimsWSUserName");
        string client_secret = "";
        string secretName = "ClaimsWSPassword_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword = client_secret;
        client_secret = "";
        secretName = "ClaimsWSPassword_2_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword2 = client_secret;
        XmlDocument xmlDoc = new XmlDocument();
        string response = string.Empty;
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
            response = MakeClaimsServiceCall(xmlDoc, wsURL, "http://service.operationmgmt.fi/ClaimsService/institutional/InquireClaim", wSUserName, wSPassword2);
            txtResponse.Text = response;
            int passthroughId = (int)PassthroughController.PassthroughTransactionType.ClaimInquiry;
            PassthroughController.InsertPassthroughTransactionQueue(passthroughId, "", "", "", xmlDoc.InnerXml.ToString(), response, "PNM", "", "", DateTime.Now, null, null, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(), 2);
           
        }
        catch (XmlException exception)
        {

            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", exception.Message, exception.InnerException, exception.StackTrace.ToString()), Logging.LogPriority.Error);

            txtResponse.Text = "Not a valid xml: " + exception.ToString();
        }
        catch (Exception ex)
        {
            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", ex.Message, ex.InnerException, ex.StackTrace.ToString()), Logging.LogPriority.Error);

            txtResponse.Text = response + "Error in Institutional Inquire Call " + ex.ToString();

        }

    }

    protected void btnGenXMLPriorAuthAU_Click(object sender, EventArgs e)
    {
        try
        {
            string priorAuthUserName = AppSettings.Get("PriorAuthServiceWSUserName");
            string client_secret = "";
            string secretName = "PriorAuthServiceWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string priorAuthPassword = client_secret;

            MessageHeaderTypeSubscriber[] mhts = new MessageHeaderTypeSubscriber[1];
            mhts[0] = MessageHeaderTypeSubscriber.FI;

            MessageHeaderType mht = new MessageHeaderType();
            mht.BusinessFlow = MessageHeaderTypeBusinessFlow.AddUpdatePriorAuth;
            mht.SubscriberSystem = mhts;
            mht.StateCode = "OH";
            mht.ModuleTransactionId = "";
            mht.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss"));
            mht.SITransactionKey = "";
            mht.RequestorSystem = MessageHeaderTypeRequestorSystem.PNM;

            ISAType ity = new ISAType();
            ity.ISA01_AuthorizationInformationQualifier = "00";
            ity.ISA03_SecurityInformationQualifier = "00";
            ity.ISA04_SecurityInformation = "          ";
            ity.ISA05_InterchangeIdQualifier = "ZZ";
            ity.ISA06_InterchangeSenderId = "MMISODFS";
            ity.ISA07_InterchangeIdQualifier = "ZZ";
            ity.ISA08_InterchangeReceiverId = "MMISODFS";
            ity.ISA09_InterchangeDate = DateTime.Now.ToString("yyyy-MM-dd");
            ity.ISA10_InterchangeTime = DateTime.Now.ToString("hh:mm:ss");
            ity.ISA11_RepetitionSeparator = "";
            ity.ISA12_InterchangeControlVersionNumber = "";
            ity.ISA13_InterchangeControlNumber = "";
            ity.ISA14_AcknowledgementRequested = "";
            ity.ISA15_InterchangeUsageIndicator = "";
            ity.ISA16_ComponentElementSeparator = "";

            GSType gty = new GSType();
            gty.GS01_FunctionalIdentifierCode = "";
            gty.GS02_ApplicationSendersCode = "";
            gty.GS03_ApplicationReceiversCode = "";
            gty.GS04_Date = "";
            gty.GS05_Time = "";
            gty.GS06_GroupControlNumber = "";
            gty.GS07_ResponsibleAgencyCode = "";
            gty.GS08_VersionReleaseIndustryIdentifierCode = "";

            STType sty = new STType();
            sty.ST01_TransactionSetIdentifierCode = "";
            sty.ST02_TransactionSetControlNumber = "";
            sty.ST03_ImplementationConventionReference = "";


            BHTType bht = new BHTType();
            bht.BHT01_HeirarchicalStructureCode = "";
            bht.BHT02_TransactionSetPurposeCode = "";
            bht.BHT03_SubmitterTransactionIdentifier = "";
            bht.BHT04_TransactionSetCreationDate = "";
            bht.BHT05_TransactionSetCreationTime = "";
            bht.BHT06_TransactionTypeCode = "";

            HLType ulevel = new HLType();
            ulevel.HL01_HeirarchicalIdNumber = "";
            ulevel.HL02_HeirarchicalParentIdNumber = "";
            ulevel.HL03_HeirarchicalLevelCode = "";
            ulevel.HL04_HerarchicalChildCode = "";

            NM1Type nmlevel = new NM1Type();
            nmlevel.NM101_EntityIdentifierCode = "";
            nmlevel.NM102_EntityTypeQualifier = "";
            nmlevel.NM103_UMOLastOrOrganizationName = "";
            nmlevel.NM104_UMOFirstName = "";
            nmlevel.NM105_UMOMiddleName = "";
            nmlevel.NM106_NamePrefix = "";
            nmlevel.NM107_UMONameSuffix = "";
            nmlevel.NM108_IdentificationCodeQualifier = "";
            nmlevel.NM109_UMOIdentifier = "";
            nmlevel.NM110_EntityRelationshipCode = "";
            nmlevel.NM111_EntityIdentifierCode = "";
            nmlevel.NM112_NameLastOrOrganizationName = "";

            UMONameDetails_2010AType udetail = new UMONameDetails_2010AType();
            udetail.UMOName_2010A = nmlevel;

            UMODetails_2000AType ut = new UMODetails_2000AType();
            ut.UMOLevel_2000A = ulevel;
            ut.UMONameDetails_2010A = udetail;

            BHTContainterType bty = new BHTContainterType();
            bty.BeginningOfHierarchicalTransaction = bht;

            SEType sety = new SEType();
            sety.SE01_TransactionSegmentCount = "";

            GEType gety = new GEType();
            gety.GE01_NumberTransactionSetsIncluded = "";

            IEAType iety = new IEAType();
            iety.IEA01_NumberIncludedFunctionalGroups = "";

            PriorAuthRequest278Type p278 = new PriorAuthRequest278Type();
            p278.InterchangeControlHeader = ity;
            p278.FunctionalGroupHeader = gty;
            p278.TransactionSetHeader = sty;
            p278.BHTContainter = bty;
            p278.TransactionSetTrailer = sety;
            p278.FunctionalGroupTrailer = gety;
            p278.InterchangeControlTrailer = iety;


            AddUpdatePriorAuthRequest auReqPay = new AddUpdatePriorAuthRequest();
            auReqPay.MessageHeader = mht;
            auReqPay.PriorAuthRequest278 = p278;

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(AddUpdatePriorAuthRequest));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("pri", "http://service.caremgmt.fi/PriorAuthService");
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var streamRequest = new StringWriter();
            var writer = XmlWriter.Create(streamRequest, settings);
            x.Serialize(writer, auReqPay, ns);
            string xml = streamRequest.ToString();

            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:pri=\"http://service.caremgmt.fi/PriorAuthService\">"
                + Environment.NewLine + wssSecurityHeader(priorAuthUserName, priorAuthPassword) + Environment.NewLine + "<soapenv:Body>" +
                Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            xmlDoc.DocumentElement.SetAttribute("xmlns:pri", "http://service.caremgmt.fi/PriorAuthService");
            var auPrior = xmlDoc.InnerXml.ToString().Replace("AddUpdatePriorAuthRequest xmlns:pri=\"http://service.caremgmt.fi/PriorAuthService\"", "AddUpdatePriorAuthRequest")
                .Replace("AddUpdatePriorAuthRequest", "pri:AddUpdatePriorAuthRequest")
                .Replace("MessageHeader", "pri:MessageHeader")
                .Replace("PriorAuthRequest278", "pri:PriorAuthRequest278");

            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(auPrior);

            txtGenXML.Text = xmlDoc.InnerXml.ToString();
        }catch(Exception ex)
        {
            txtResponse.Text = "Error while generating the xml: " + ex.ToString();
        }
    }

    protected void btnGenXMLPriorAuthInq_Click(object sender, EventArgs e)
    {
        try
        {
            string priorAuthUserName = AppSettings.Get("PriorAuthServiceWSUserName");
            string client_secret = "";
            string secretName = "PriorAuthServiceWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string priorAuthPassword = client_secret;

            MessageHeaderTypeSubscriber[] mhts = new MessageHeaderTypeSubscriber[1];
            mhts[0] = MessageHeaderTypeSubscriber.FI;

            MessageHeaderType mht = new MessageHeaderType();
            mht.BusinessFlow = MessageHeaderTypeBusinessFlow.InquirePriorAuth;
            mht.SubscriberSystem = mhts;
            mht.StateCode = "OH";
            mht.ModuleTransactionId = "";
            mht.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss"));
            mht.SITransactionKey = "";
            mht.RequestorSystem = MessageHeaderTypeRequestorSystem.PNM;

            InquirePriorAuthRequestPayloadType inqReqPay = new InquirePriorAuthRequestPayloadType();
            inqReqPay.PayorType = "1";
            inqReqPay.PriorAuthNumber = "12345678";
            inqReqPay.ProviderId = "0383380";


            InquirePriorAuthRequest inq = new InquirePriorAuthRequest();
            inq.MessageHeader = mht;
            inq.RequestPayload = inqReqPay;

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(InquirePriorAuthRequest));
            
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("pri", "http://service.caremgmt.fi/PriorAuthService");
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var streamRequest = new StringWriter();
            var writer = XmlWriter.Create(streamRequest, settings);
            x.Serialize(writer, inq, ns);
            string xml = streamRequest.ToString();

            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:pri=\"http://service.caremgmt.fi/PriorAuthService\">"
                + Environment.NewLine + wssSecurityHeader(priorAuthUserName, priorAuthPassword) + Environment.NewLine + "<soapenv:Body>" +
                Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            xmlDoc.DocumentElement.SetAttribute("xmlns:pri", "http://service.caremgmt.fi/PriorAuthService");
            var inqPrior = xmlDoc.InnerXml.ToString().Replace("InquirePriorAuthRequest xmlns:pri=\"http://service.caremgmt.fi/PriorAuthService\"", "InquirePriorAuthRequest")
                .Replace("InquirePriorAuthRequest", "pri:InquirePriorAuthRequest")
                .Replace("MessageHeader", "pri:MessageHeader")
                .Replace("RequestPayload", "pri:RequestPayload");

            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(inqPrior);

            txtGenXML.Text = xmlDoc.InnerXml.ToString();
        }catch(Exception ex)
        {
            txtResponse.Text = "Error while generating the xml: " + ex.ToString();
        }
    }

    protected void btnGenerateAttachmentXML_Click(object sender, EventArgs e)
    {
        try
        {
            string url = AppSettings.Get("DocumentWSURL", CON.WebServiceURI.DocumentService);
            string wsHost = AppSettings.Get("DocumentWSHost");
            string wsCertificateName = AppSettings.Get("DocumentWSCertificateName");
            string wsUserName = AppSettings.Get("DocumentWSUserName");
            string client_secret = "";
            string secretName = "DocumentWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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

            InqMessageHeaderSubscriber[] mhts = new InqMessageHeaderSubscriber[1];
            mhts[0] = InqMessageHeaderSubscriber.FI;

            Corp.Core.Libraries.SI.AttachmentServiceReference.MessageHeader mh = new Corp.Core.Libraries.SI.AttachmentServiceReference.MessageHeader();
            mh.SITransactionKey = "abcde12345";
            mh.BusinessFlow = InqMessageHeaderBusinessFlow.sendAttachment;
            mh.SubscriberSystem = mhts;
            mh.StateCode = "OH";
            mh.ModuleTransactionId = "12345";
            mh.RequestTimestamp = DateTime.Now.ToString();
            mh.RequestorSystem = InqMessageHeaderRequestorSystem.PNM;

            SendAttachmentData sadI = new SendAttachmentData();
            sadI.DocXrefType = "1";
            sadI.IndexId = "1";

            SendAttachmentData[] sad = new SendAttachmentData[1];
            sad[0] = sadI;

            SendAttachment sa = new SendAttachment();
            sa.Identifiers = sad;
            sa.DocumentType = "105";
            sa.DocumentName = "hello.txt";
            sa.DocumentExtension = "txt";
            sa.AttachmentData64Binary = System.Convert.FromBase64String("cm9oaXQ=");


            SendAttachment[] saa = new SendAttachment[1];
            saa[0] = sa;

            SendAttachmentInformation sai = new SendAttachmentInformation();
            sai.AttachmentData = saa;

            SendAttachmentPayload sap = new SendAttachmentPayload();
            sap.AttachmentInfo = sai;
            sap.AttachmentInfo.SourceId = "12345";

            sendAttachmentRequest sar = new sendAttachmentRequest();
            sar.MessageHeader = mh;
            sar.Payload = sap;



            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(sendAttachmentRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, sar, emptyNs);
            string xml = stream2.ToString();
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://mes.gov/attachment\">"
                + Environment.NewLine + wssSecurityHeader(wsUserName, wsPassword) + Environment.NewLine + "<soapenv:Body>" +
                Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            txtGenXML.Text = xmlDoc.InnerXml.ToString();
        }
        catch (Exception ex)
        {
            txtResponse.Text = "Error while generating the xml: " + ex.ToString();
        }
    }

    protected void btnGenXMLPriorAuthSer_Click(object sender, EventArgs e)
    {
        try
        {
            string priorAuthUserName = AppSettings.Get("PriorAuthServiceWSUserName");
            string client_secret = "";
            string secretName = "PriorAuthServiceWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string priorAuthPassword = client_secret;

            MessageHeaderTypeSubscriber[] mhts = new MessageHeaderTypeSubscriber[1];
            mhts[0] = MessageHeaderTypeSubscriber.FI;

            MessageHeaderType mht = new MessageHeaderType();
            mht.BusinessFlow = MessageHeaderTypeBusinessFlow.SearchPriorAuth;
            mht.SubscriberSystem = mhts;
            mht.StateCode = "OH";
            mht.ModuleTransactionId = "";
            mht.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss"));
            mht.SITransactionKey = "";
            mht.RequestorSystem = MessageHeaderTypeRequestorSystem.PNM;

            SearchPriorAuthRequestPayloadType spty = new SearchPriorAuthRequestPayloadType();
            spty.PayorType = "1";
            //spty.PriorAuthNumber = "";
            //spty.PatientEventTrackingNumber = "";
            //spty.MemberMedicaidId = "";
            //spty.PAStatus = "";
            //spty.AssignmentTypeCode = "";
            //spty.ServiceTypeCode = "";
            //spty.ICDProcedureCode = "";
            //spty.CPTHCPCSServiceCode = "";
            //spty.RevenueCode = "";
            //spty.DiagnosisCode = "";
            //spty.SubmittedDateSpecified = true;
            //spty.SubmittedDate = DateTime.Now;
            //spty.PAStartDateSpecified = true;
            //spty.PAStartDate = DateTime.Now;
            //spty.PAEndDateSpecified = true;
            //spty.PAEndDate = DateTime.Now;
            spty.OrderingProviderID = "12345";
            spty.RequestingProviderID = "12345";

            SearchPriorAuthRequest sPA = new SearchPriorAuthRequest();
            sPA.MessageHeader = mht;
            sPA.RequestPayload = spty;

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(SearchPriorAuthRequest));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("pri", "http://service.caremgmt.fi/PriorAuthService");
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var streamRequest = new StringWriter();
            var writer = XmlWriter.Create(streamRequest, settings);
            x.Serialize(writer, sPA, ns);
            string xml = streamRequest.ToString();

            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:pri=\"http://service.caremgmt.fi/PriorAuthService\">"
                + Environment.NewLine + wssSecurityHeader(priorAuthUserName, priorAuthPassword) + Environment.NewLine + "<soapenv:Body>" +
                Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            xmlDoc.DocumentElement.SetAttribute("xmlns:pri", "http://service.caremgmt.fi/PriorAuthService");
            var searchPrior = xmlDoc.InnerXml.ToString().Replace("SearchPriorAuthRequest xmlns:pri=\"http://service.caremgmt.fi/PriorAuthService\"", "SearchPriorAuthRequest")
                .Replace("SearchPriorAuthRequest", "pri:SearchPriorAuthRequest")
                .Replace("MessageHeader", "pri:MessageHeader")
                .Replace("RequestPayload", "pri:RequestPayload");

            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(searchPrior);

            txtGenXML.Text = xmlDoc.InnerXml.ToString();
        }catch(Exception ex)
        {
            txtResponse.Text = "Error while generating the xml: " + ex.ToString();
        }
    }

    protected void btnAddUpdateAuth_Click(object sender, EventArgs e)
    {
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
        }
        catch (XmlException exception)
        {
            txtResponse.Text = "Not a valid xml: " + exception.ToString();
        }

        int AddUpdatePriorAuth = 4;
        int transactionID = InfoAccessController.InsertPASSTHROUGH_TRANSACTIONQUEUE(AddUpdatePriorAuth, DateTime.Now, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId));


        PriorAuthServiceReqRes pa = new PriorAuthServiceReqRes();
        string transactionResponse = pa.makePriorAuthWSRequestResponse(transactionID, xmlDoc, CON.PriorAuthServiceResponse.AddUpdatePriorAuth, CON.PriorAuthSearchSoapAction.AddUpdatePriorAuth);

        if (transactionResponse.Equals(CON.TransactionResult.TransactionPassed))
        {
            txtResponse.Text = InfoAccessController.GetResponsePayloadByPassThroughTransactionID(transactionID);
        }
        else
        {
            txtResponse.Text = "Transaction Failed : check logs";
        }
        txtResponse.Text = InfoAccessController.GetResponsePayloadByPassThroughTransactionID(transactionID);
    }

    protected void btnmakeAttachmentRequest_Click(object sender, EventArgs e)
    {
        string url = AppSettings.Get("DocumentWSURL", CON.WebServiceURI.DocumentService);
        string wsCertificateName = AppSettings.Get("DocumentWSCertificateName");
        string wsUserName = AppSettings.Get("DocumentWSUserName");
        string client_secret = "";
        string secretName = "DocumentWSPassword_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        secretName = "DocumentWSPassword_2_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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

        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
        }
        catch (XmlException exception)
        {
            txtResponse.Text = "Not a valid xml: " + exception.ToString();
        }

        string transactionResponse = ServiceAgentHelper.MakeServiceCallNew(xmlDoc, url, CON.AttachmentServiceSoapAction.AttachmentService, wsUserName, wsCertificateName, wsPassword_2);

        txtResponse.Text = transactionResponse;
    }


    protected void btnInquireAuth_Click(object sender, EventArgs e)
    {
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
        }
        catch (XmlException exception)
        {
            txtResponse.Text = "Not a valid xml: " + exception.ToString();
        }

        int InquirePriorAuth = 5;
        int transactionID = InfoAccessController.InsertPASSTHROUGH_TRANSACTIONQUEUE(InquirePriorAuth, DateTime.Now, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId));

        PriorAuthServiceReqRes pa = new PriorAuthServiceReqRes();
        string transactionResponse = pa.makePriorAuthWSRequestResponse(transactionID, xmlDoc, CON.PriorAuthServiceResponse.InquirePriorAuth,
            CON.PriorAuthSearchSoapAction.InquirePriorAuth);

        if (transactionResponse.Equals(CON.TransactionResult.TransactionPassed))
        {
            txtResponse.Text = InfoAccessController.GetResponsePayloadByPassThroughTransactionID(transactionID);
        }
        else
        {
            txtResponse.Text = "Transaction Failed : check logs";
        }
        txtResponse.Text = InfoAccessController.GetResponsePayloadByPassThroughTransactionID(transactionID);
    }
    protected void btnSearchAuth_Click(object sender, EventArgs e)
    {
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
        }
        catch (XmlException exception)
        {
            txtResponse.Text = "Not a valid xml: " + exception.ToString();
        }
        int SearchPriorAuth = 6;
        int transactionID = InfoAccessController.InsertPASSTHROUGH_TRANSACTIONQUEUE(SearchPriorAuth, DateTime.Now, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId));

        PriorAuthServiceReqRes pa = new PriorAuthServiceReqRes();
        string transactionResponse = pa.makePriorAuthWSRequestResponse(transactionID, xmlDoc, CON.PriorAuthServiceResponse.SearchPriorAuth,
            CON.PriorAuthSearchSoapAction.SearchPriorAuth);

        if (transactionResponse.Equals(CON.TransactionResult.TransactionPassed))
        {
            txtResponse.Text = InfoAccessController.GetResponsePayloadByPassThroughTransactionID(transactionID);
        }
        else
        {
            txtResponse.Text = "Transaction Failed : check logs";
        }
        txtResponse.Text = InfoAccessController.GetResponsePayloadByPassThroughTransactionID(transactionID);
    }

    protected void btnHospiceAddUpdate_Click(object sender, EventArgs e)
    {
        string hospiceWSURL = AppSettings.Get("HospiceWSURL", CON.WebServiceURI.HospiceService);
        string wSUserName = AppSettings.Get("HospiceWSUserName");
        string client_secret = "";
        string secretName = "HospiceWSPassword_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword = client_secret;
        client_secret = "";
        secretName = "HospiceWSPassword_2_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword_2 = client_secret;
        string wsCertificateName = AppSettings.Get("HospiceWSCertificateName");
        string wsHost = AppSettings.Get("HospiceWSHost");
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
        }
        catch (XmlException exception)
        {
            txtResponse.Text = "Not a valid xml: " + exception.ToString();
        }
        MakeServiceCall(xmlDoc, hospiceWSURL, "\"/Processes/Hospice/Starter/HospiceService.serviceagent/HospiceEndpoint/" +
            "AddUpdateHospice\"", wSUserName, wSPassword_2, wsCertificateName, wsHost);
    }

    protected void btnDocument_Click(object sender, EventArgs e)
    {
        string DocumentWSURL = AppSettings.Get("DocumentWSURL", CON.WebServiceURI.DocumentService);
        string wSUserName = AppSettings.Get("DocumentWSUserName");
        string client_secret = "";
        string secretName = "DocumentWSPassword_OH_PNM_";
        try
        {
            var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
        string wSPassword = client_secret;
        string wsHost = AppSettings.Get("DocumentWSHost");
        string wsCertificateName = AppSettings.Get("DocumentWSCertificateName");
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.LoadXml(txtGenXML.Text.ToString());
        }
        catch (XmlException exception)
        {
            txtResponse.Text = "Not a valid xml: " + exception.ToString();
        }
        MakeServiceCall(xmlDoc, DocumentWSURL, "\"http://mes.gov/sendAttachment\"", wSUserName, wSPassword, wsCertificateName, wsHost);
    }
    public void MakeServiceCall(XmlDocument xmlDoc, string url, string soapAction,
            string userName, string password, string wsCertificateName, string host)
    {
        try
        {

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            CertUtil _certs = new CertUtil();
            X509Certificate2 clientCertificate = _certs.GetCertificateByName(wsCertificateName);
            string user = userName;
            string passcode = password;
            Uri apiUrl = new Uri(url);
            WebRequest pmRequest = HttpWebRequest.Create(url);
            HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
            pmHttpRequest.ContentType = "text/xml; charset=utf-8";

            pmHttpRequest.KeepAlive = true;
            pmHttpRequest.Method = "POST";
            pmHttpRequest.SendChunked = true;
            pmHttpRequest.UserAgent = ".NET Framework";
            pmHttpRequest.Host = host;
            if (clientCertificate != null)
            {
                pmHttpRequest.ClientCertificates.Add(clientCertificate);
            }
            String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + passcode));
            pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
            pmHttpRequest.Headers.Add("SOAPAction", soapAction);
            Stream requestStream = pmHttpRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            var response = "";

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

            txtResponse.Text = response.ToString();


        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                var response = ex.Response as HttpWebResponse;
                if (response != null)
                {
                    //http status code avaliable
                    var respBody = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    txtResponse.Text = string.Format("Exception Response. Exception Message : {0}, Inner Exception : {1}, Response: {2}, Response: {3}", ex.Message, ex.InnerException, respBody.ToString(), response);
                }
                else
                {
                    // no http status code available
                    txtResponse.Text = string.Format("Response is null, unable to make connection. Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
                }
            }
            else
            {
                // no http status code available
                txtResponse.Text = string.Format("Response is null, unable to make connection. Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);

            }

        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
    }
    private string GenerateClaimsSearchRequestXml()
    {
        try
        {
            string wSUserName =  AppSettings.Get("ClaimsWSUserName");
            string client_secret = "";
            string secretName = "ClaimsWSPassword_2_OH_PNM_";
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
            string wSPassword = client_secret;
            string subscriberSystem = tbSubscriber.Text.Trim();
            string SubscriberId = tbSubscriberId.Text.Trim();

            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            List<ClaimsSearch.MessageHeaderTypeSubscriber> msgHeaderSubList = new List<ClaimsSearch.MessageHeaderTypeSubscriber>();
            msgHeaderSubList.Add(ClaimsSearch.MessageHeaderTypeSubscriber.FI);

            ClaimsSearch.MessageHeaderType msgHeader = new ClaimsSearch.MessageHeaderType
            {
                BusinessFlow = ClaimsSearch.MessageHeaderTypeBusinessFlow.SearchClaims,
                RequestorSystem = ClaimsSearch.MessageHeaderTypeRequestorSystem.PNM,
                RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss")),
                StateCode = "OH",
                SubscriberSystem = msgHeaderSubList.ToArray()
            };

            ClaimsSearch.SearchClaimsRequestPayloadType pt = new ClaimsSearch.SearchClaimsRequestPayloadType();
            pt.ICN = "123456891";
            pt.PayorType = "FFS";
            pt.TotalCharges = 0;

            ClaimsSearch.SearchClaimsRequest searchClaimsRequestObj = new ClaimsSearch.SearchClaimsRequest();
            searchClaimsRequestObj.MessageHeader = msgHeader;
            searchClaimsRequestObj.RequestPayload = pt;

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ClaimsSearch.SearchClaimsRequest));
           
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("sear", "http://service.operationmgmt.fi/ClaimsService/search");

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, searchClaimsRequestObj, ns);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                    + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:sear", "http://service.operationmgmt.fi/ClaimsService/search");
            var claimsXML = xmlDoc.InnerXml.ToString().Replace("SearchClaimsRequest xmlns:sear=\"http://service.operationmgmt.fi/ClaimsService/search\"", "SearchClaimsRequest")
                .Replace("SearchClaimsRequest", "sear:SearchClaimsRequest")
                .Replace("MessageHeader", "sear:MessageHeader")
                .Replace("RequestPayload", "sear:RequestPayload");
            return claimsXML;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }
    private string GenerateCostReportSearchRequestXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("CostReportsWSUserName");
            string client_secret = "";
            string secretName = "CostReportsWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            Corp.Core.Libraries.CostReportReference.MessageHeader msgHeader = new Corp.Core.Libraries.CostReportReference.MessageHeader();
            msgHeader.BusinessFlow = "costReportSearch";
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = "PNM";//PNM
            msgHeader.SubscriberSystem = "MITS";
            msgHeader.ModuleTransactionId = "123";//MITS
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
            msgHeader.SITransactionKey = sitTransactionKey;
            Corp.Core.Libraries.CostReportReference.costReportSearchRequest searchRequest = new Corp.Core.Libraries.CostReportReference.costReportSearchRequest
            {
                MessageHeader = msgHeader,
                Payload = new Corp.Core.Libraries.CostReportReference.cCostReportSearch
                {
                    IdProvider = new List<Corp.Core.Libraries.CostReportReference.cCostReportSearchIdProvider>
                   {
                       new Corp.Core.Libraries.CostReportReference.cCostReportSearchIdProvider
                       {
                           IdProvider = "OH12345"
                       }
                   }.ToArray()
                }
            };

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.CostReportReference.costReportSearchRequest));

            var emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add("cos", "http://mes.gov/costreports");
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, searchRequest, emptyNs);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                    + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:cos", "http://mes.gov/costreports");
            var claimsXML = xmlDoc.InnerXml.ToString().Replace("costReportSearchRequest", "cos:CostReportSearch")
                                .Replace("MessageHeader", "cos:MessageHeader")
                                .Replace("Payload", "cos:Payload");
            return claimsXML;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }
    private string GenerateCreateAuthRequestXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("PriorAuthSearchWSURLWSUserName");
            string client_secret = "";
            string secretName = "PriorAuthSearchWSURLWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            Corp.Core.Libraries.CareManagement.MessageHeader msgHeader = new Corp.Core.Libraries.CareManagement.MessageHeader();
            msgHeader.BusinessFlow = Corp.Core.Libraries.CareManagement.MessageHeaderBusinessFlow.CreateAuthorization;
            //msgHeader.BusinessFlowSpecified = true;
            msgHeader.StateCode = Corp.Core.Libraries.CareManagement.MessageHeaderStateCode.OH;
            msgHeader.RequestorSystem = Corp.Core.Libraries.CareManagement.MessageHeaderRequestorSystem.PNM;
            msgHeader.ModuleTransactionId = "123";//MITS
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
            msgHeader.SITransactionKey = sitTransactionKey;

            var attachments = new Corp.Core.Libraries.CareManagement.Attachments[1];
            for (int i = 0; i < attachments.Length; i++)
            {
                attachments[i] = new Corp.Core.Libraries.CareManagement.Attachments
                {
                    AttachmentDate = "2019-09-09",
                    AttachmentID = "",
                    AttachmentType = "",

                };
            }
            var providerNotes = new Corp.Core.Libraries.CareManagement.ProviderNotes[1];
            for (int i = 0; i < providerNotes.Length; i++)
            {
                providerNotes[i] = new Corp.Core.Libraries.CareManagement.ProviderNotes
                {
                    CreatedBy = "",
                    CreatedDate = "",
                    LastModifiedBy = "",
                    LastModifiedDate = "",
                    Note = "",
                    ODSProviderNoteID = "",

                };
            }

            Corp.Core.Libraries.CareManagement.createAuthorizationRequest request = new Corp.Core.Libraries.CareManagement.createAuthorizationRequest
            {
                MessageHeader = msgHeader,
                Payload = new Corp.Core.Libraries.CareManagement.CreateRequestPayload
                {
                    Authorization = new Corp.Core.Libraries.CareManagement.createAuthorization
                    {
                        AuthorizationInfo = new Corp.Core.Libraries.CareManagement.AuthorizationInfo
                        {
                            PriorAuthorizationID = "PA09845",
                            AssignmentCode = "AA",
                            PriorAuthorizationTypeCode = "TC",
                            MemberID = "AEW",
                            ProviderID = "987890",
                            OrderingProviderID = "1098765",
                            RenderingProviderID = "009879",
                            StatusCode = "Ac",
                            ReasonCode = "SE",
                            AuthorizationDate = "2019-09-09",
                            //  AuthorizationBeginDate = "2019-09-09",
                            //  AuthorizationEndDate = "2019-09-09",
                            ApproverID = "12",
                            AuthorizationSource = "PNM",
                            DateOfBirth = "2019-09-09",
                            ContactNumber = "123",
                            ContactName = "First",
                            AdmissionDate = "2019-09-09",
                            LTCFDischargeDate = "2019-09-09",
                            SpecialIndicator = "Y",
                            PatientEventTrackingNum = "345Bd",
                            RecordStatusCode = "Active"
                        },
                        Attachments = attachments,
                        Items = new object[1],
                        ProviderNotes = providerNotes,



                        //AuthorizationService = new List<Corp.Core.Libraries.CareManagement.AuthorizationService>
                        //{
                        //    new Corp.Core.Libraries.CareManagement.AuthorizationService
                        //    {
                        //         RenderingProviderID="8009",
                        //         PriorAuthorizationFrequencyCode="PC",
                        //         ProcedureCode="AA",
                        //         FromProcedureCode="EE",
                        //         ToProcedureCode="EE",
                        //         ProcedureTypeCode="ER",
                        //         ICDType="HS",
                        //         ServiceLimit="6",
                        //         LimitAmount="8",
                        //         RateAmount="12",
                        //         ServiceStartDate="2019-09-09T12:00:09",
                        //         ServiceEndDate="2019-09-09T12:00:09",
                        //         ProcModifierCode="HY",
                        //         ProcModifierCode1="KI",
                        //         ProcModifierCode2="LO",
                        //         ProcModifierCode3="BB",
                        //         ToothSurfaceCode="QQ",
                        //         ToothNumCode="BC",
                        //         ServiceStatusCode="WER",
                        //         ServiceStatusReasonCode="XS",
                        //         ServicesUsed="7",
                        //         AmountUsed="12",
                        //         DetailLineNumber="10",
                        //         FirstDiagnosisCode="99",
                        //         LastDiagnosisCode="90",
                        //         AuthorizedDollars="123",
                        //         AuthorizedUnits="5",
                        //         BalanceDollars="99",
                        //         BalanceUnits="009",
                        //         BillDirectFromDate="2018-09-09",
                        //         BillDirectToDate="2020-09-07",
                        //         ToothExtractionDate="2020-09-07",
                        //         InitialPlacement="Y",
                        //         ListPrice="1345",
                        //         CaloriesPerDayNum="78",
                        //         DaysNum="9",
                        //         PricingFormula="67yy",
                        //         PriorPlacement="12",
                        //         ToothQuadrant="32",
                        //         QuantityUsedAmount="10",
                        //         QuantityUsedUnits="7",
                        //         RequestedDollars="40",
                        //         RequestedEffectiveDate="2024-09-09",
                        //         RequestedEndDate="2024-09-09",
                        //         RequestedUnits="4",
                        //         TypeOfServiceCode="UU",
                        //         ServiceCode="YT",
                        //         ThruService="55",
                        //         NDCCode="LO",
                        //         InpatientProcedure="YT",
                        //         RevenueCode="FC",
                        //         RecordStatusCode="Active"
                        //    }
                        //}.ToArray(),


                        AuthorizationDiagnosis = new List<Corp.Core.Libraries.CareManagement.AuthorizationDiagnosis>
                        {
                            new Corp.Core.Libraries.CareManagement.AuthorizationDiagnosis
                            {
                                ICDType="ER",
                                DiagnosisCode="de",
                                DiagnosisTypeCode="CR",
                                RecordStatusCode="Active",
                                ODSDiagnosisID="6754",
                                CreatedDate="2020-11-23T20:22:08.008Z",
                                CreatedBy="PNM",
                                LastModifiedDate="2020-11-23T20:22:08.008Z",
                                LastModifiedBy="PNM"
                            }

                        }.ToArray()


                    }


                }
            };
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.CareManagement.createAuthorizationRequest));
            var emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add("car", "http://mes.gov/caremanagement");
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, request, emptyNs);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                    + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:car", "http://mes.gov/caremanagement");
            var claimsXML = xmlDoc.InnerXml.ToString().Replace("createAuthorizationRequest", "car:CreateAuthorization")
                                .Replace("MessageHeader", "car:MessageHeader")
                                .Replace("Payload", "car:Payload");
            return claimsXML;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }

    private string GenerateSearchAuthRequestXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("PriorAuthSearchWSURLWSUserName");
            string client_secret = "";
            string secretName = "PriorAuthSearchWSURLWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            Corp.Core.Libraries.CareManagement.MessageHeader msgHeader = new Corp.Core.Libraries.CareManagement.MessageHeader();
            msgHeader.BusinessFlow = Corp.Core.Libraries.CareManagement.MessageHeaderBusinessFlow.SearchAuthorization;
            //  msgHeader.BusinessFlowSpecified = true;
            msgHeader.StateCode = Corp.Core.Libraries.CareManagement.MessageHeaderStateCode.OH;
            msgHeader.RequestorSystem = Corp.Core.Libraries.CareManagement.MessageHeaderRequestorSystem.PNM;
            msgHeader.ModuleTransactionId = "123";//MITS
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
            msgHeader.SITransactionKey = sitTransactionKey;
            Corp.Core.Libraries.CareManagement.searchAuthorizationRequest request = new Corp.Core.Libraries.CareManagement.searchAuthorizationRequest
            {

                MessageHeader = msgHeader,
                SearchAuthorizationPayload = new Corp.Core.Libraries.CareManagement.SearchAuthorizationPayload
                {
                    MemberID = "12345"
                }
            };

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.CareManagement.searchAuthorizationRequest));
            var emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add("car", "http://mes.gov/caremanagement");
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, request, emptyNs);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                    + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:car", "http://mes.gov/caremanagement");
            var claimsXML = xmlDoc.InnerXml.ToString()
                                 .Replace("searchAuthorizationRequest", "car:SearchAuthorizationRequest")
                                .Replace("MessageHeader", "car:MessageHeader")
                                .Replace("SearchAuthorizationPayload", "car:SearchAuthorizationPayload");
            return claimsXML;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }
    private string GenerateInquireAuthRequestXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("PriorAuthSearchWSURLWSUserName");
            string client_secret = "";
            string secretName = "PriorAuthSearchWSURLWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            Corp.Core.Libraries.CareManagement.MessageHeader msgHeader = new Corp.Core.Libraries.CareManagement.MessageHeader();
            msgHeader.BusinessFlow = Corp.Core.Libraries.CareManagement.MessageHeaderBusinessFlow.InquireAuthorization;
            // msgHeader.BusinessFlowSpecified = true;
            msgHeader.StateCode = Corp.Core.Libraries.CareManagement.MessageHeaderStateCode.OH;
            msgHeader.RequestorSystem = Corp.Core.Libraries.CareManagement.MessageHeaderRequestorSystem.PNM;
            msgHeader.ModuleTransactionId = "123";//MITS
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
            msgHeader.SITransactionKey = sitTransactionKey;
            Corp.Core.Libraries.CareManagement.inquireAuthorizationRequest request = new Corp.Core.Libraries.CareManagement.inquireAuthorizationRequest
            {

                MessageHeader = msgHeader,
                InquireAuthorizationPayload = new Corp.Core.Libraries.CareManagement.InquireAuthorizationPayload
                {
                    PriorAuthorizationID = "2020307001",
                    ProviderID = "0051199"
                }
            };

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.CareManagement.inquireAuthorizationRequest));
            var emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add("car", "http://mes.gov/caremanagement");
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, request, emptyNs);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                    + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:car", "http://mes.gov/caremanagement");
            var claimsXML = xmlDoc.InnerXml.ToString()
                .Replace("inquireAuthorizationRequest", "car:InquireAuthorizationRequest")
                .Replace("MessageHeader", "car:MessageHeader")
                .Replace("InquireAuthorizationPayload", "car:InquireAuthorizationPayload");
            return claimsXML;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }
    private string GenerateUpdateAuthRequestXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("PriorAuthSearchWSURLWSUserName");
            string client_secret = "";
            string secretName = "PriorAuthSearchWSURLWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            Corp.Core.Libraries.CareManagement.MessageHeader msgHeader = new Corp.Core.Libraries.CareManagement.MessageHeader();
            msgHeader.BusinessFlow = Corp.Core.Libraries.CareManagement.MessageHeaderBusinessFlow.UpdateAuthorization;
            //msgHeader.BusinessFlowSpecified = true;
            msgHeader.StateCode = Corp.Core.Libraries.CareManagement.MessageHeaderStateCode.OH;
            msgHeader.RequestorSystem = Corp.Core.Libraries.CareManagement.MessageHeaderRequestorSystem.PNM;
            msgHeader.ModuleTransactionId = "123";//MITS
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
            msgHeader.SITransactionKey = sitTransactionKey;
            Corp.Core.Libraries.CareManagement.updateAuthorizationRequest request = new Corp.Core.Libraries.CareManagement.updateAuthorizationRequest
            {

                MessageHeader = msgHeader,
                Payload = new Corp.Core.Libraries.CareManagement.UpdateRequestPayload
                {
                    Authorization = new Corp.Core.Libraries.CareManagement.updateAuthorization
                    {
                        AuthorizationInfo = new Corp.Core.Libraries.CareManagement.AuthorizationInfo
                        {
                            PriorAuthorizationID = "PA09845",
                            AssignmentCode = "AA",
                            PriorAuthorizationTypeCode = "TC",
                            MemberID = "AEW",
                            ProviderID = "987890",
                            OrderingProviderID = "1098765",
                            RenderingProviderID = "009879",
                            StatusCode = "Ac",
                            ReasonCode = "SE",
                            AuthorizationDate = "2019-09-09",
                            //AuthorizationBeginDate = "2019-09-09",
                            //AuthorizationEndDate = "2019-09-09",
                            ApproverID = "12",
                            AuthorizationSource = "PNM",
                            DateOfBirth = "2019-09-09",
                            ContactNumber = "123",
                            ContactName = "First",
                            AdmissionDate = "2019-09-09",
                            LTCFDischargeDate = "2019-09-09",
                            SpecialIndicator = "Y",
                            PatientEventTrackingNum = "345Bd",
                            RecordStatusCode = "Active"
                        },

                        AuthorizationDiagnosis = new List<Corp.Core.Libraries.CareManagement.AuthorizationDiagnosis>
                        {
                            new Corp.Core.Libraries.CareManagement.AuthorizationDiagnosis
                            {
                                ICDType="ER",
                                DiagnosisCode="de",
                                DiagnosisTypeCode="CR",
                                RecordStatusCode="Active",
                                ODSDiagnosisID="6754",
                                CreatedDate="2020-11-23T20:22:08.008Z",
                                CreatedBy="PNM",
                                LastModifiedDate="2020-11-23T20:22:08.008Z",
                                LastModifiedBy="PNM"
                            }
                        }.ToArray()
                    }
                }
            };

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.CareManagement.updateAuthorizationRequest));
            var emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add("car", "http://mes.gov/caremanagement");
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, request, emptyNs);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                    + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:car", "http://mes.gov/caremanagement");
            var claimsXML = xmlDoc.InnerXml.ToString().Replace("updateAuthorizationRequest", "car:UpdateAuthorization")
                                .Replace("MessageHeader", "car:MessageHeader")
                                .Replace("Payload", "car:Payload");
            return claimsXML;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }


    private string GenerateCostReportDueDateSearchRequestXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("CostReportsWSUserName");
            string client_secret = "";
            string secretName = "CostReportsWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            Corp.Core.Libraries.CostReportReference.MessageHeader msgHeader = new Corp.Core.Libraries.CostReportReference.MessageHeader();
            msgHeader.BusinessFlow = "costReportDueDateSearch";
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = "PNM";//PNM
            msgHeader.SubscriberSystem = "MITS";
            msgHeader.ModuleTransactionId = "123";//MITS
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
            msgHeader.SITransactionKey = sitTransactionKey;
            Corp.Core.Libraries.CostReportReference.costReportDueDateSearchRequest searchRequest = new Corp.Core.Libraries.CostReportReference.costReportDueDateSearchRequest
            {
                MessageHeader = msgHeader,
                Payload = new Corp.Core.Libraries.CostReportReference.cCostReportDueDateSearch
                {
                    IdProvider = "2886935"
                }
            };

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.CostReportReference.costReportDueDateSearchRequest));

            var emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add("cos", "http://mes.gov/costreports");
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, searchRequest, emptyNs);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                    + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:cos", "http://mes.gov/costReports");
            var claimsXML = xmlDoc.InnerXml.ToString().Replace("costReportDueDateSearchRequest", "cos:CostReportDueDateSearch")
                                .Replace("MessageHeader", "cos:MessageHeader")
                                .Replace("Payload", "cos:Payload");
            return claimsXML;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }
    private string GenerateCostReportSubmitRequestXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("CostReportsWSUserName");
            string client_secret = "";
            string secretName = "CostReportsWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            Corp.Core.Libraries.CostReportReference.MessageHeader msgHeader = new Corp.Core.Libraries.CostReportReference.MessageHeader();
            msgHeader.BusinessFlow = "submitCostReport";
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = "PNM";//PN
            msgHeader.SubscriberSystem = "MITS";
            msgHeader.ModuleTransactionId = "123";//MITS
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
            msgHeader.SITransactionKey = sitTransactionKey;
            Corp.Core.Libraries.CostReportReference.submitCostReportRequest request = new Corp.Core.Libraries.CostReportReference.submitCostReportRequest
            {
                MessageHeader = msgHeader,
                Payload = new Corp.Core.Libraries.CostReportReference.cSubmitCostReport
                {
                    IdProvider = "0019974",
                    CostReportType = "A",
                    CRFiscalYear = "2019",
                    AttachmentInfo = new List<Corp.Core.Libraries.CostReportReference.cSubmitCostReportAttachmentInfo>
                    {
                        new Corp.Core.Libraries.CostReportReference.cSubmitCostReportAttachmentInfo
                        {
                            DocumentID = "635",
                            DocumentName = "0019974_CORRECTION ADVISORY_11062020130306.docx"
                        }
                    }.ToArray()
                }

            };

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.CostReportReference.submitCostReportRequest));

            var emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add("cos", "http://mes.gov/costreports");
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, request, emptyNs);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                    + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:cos", "http://mes.gov/costreports");
            var claimsXML = xmlDoc.InnerXml.ToString().Replace("submitCostReportRequest", "cos:SubmitCostReport")
                                .Replace("MessageHeader", "cos:MessageHeader")
                                .Replace("Payload", "cos:Payload");
            return claimsXML;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }
    private string GenerateCostReportSettlementRequestXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("CostReportsWSUserName");
            string client_secret = "";
            string secretName = "CostReportsWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            Corp.Core.Libraries.CostReportReference.MessageHeader msgHeader = new Corp.Core.Libraries.CostReportReference.MessageHeader();
            msgHeader.BusinessFlow = "submitCostSettlementReport";
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = "PNM";//PNM
            msgHeader.SubscriberSystem = "MITS";
            msgHeader.ModuleTransactionId = "123";//MITS
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
            msgHeader.SITransactionKey = sitTransactionKey;
            Corp.Core.Libraries.CostReportReference.submitCostSettlementReportRequest request = new Corp.Core.Libraries.CostReportReference.submitCostSettlementReportRequest
            {
                MessageHeader = msgHeader,
                Payload = new Corp.Core.Libraries.CostReportReference.cSubmitCostSettlementReport
                {
                    IdProvider = "0019974",
                    CostReportType = "A",
                    CostReportFromDate = "2018-07-01",
                    CostReportFiscalYear = "2019",
                    AttachmentInfo = new Corp.Core.Libraries.CostReportReference.cSubmitCostSettlementReportAttachmentInfo
                    {
                        DocumentID = "635",
                        DocumentName = "0019974_CORRECTION ADVISORY_11062020130306.docx",
                        FirstAccessDate = "2020-11-22",
                        ReadIndicator = "Y"
                    }
                }
            };

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.CostReportReference.submitCostSettlementReportRequest));

            var emptyNs = new XmlSerializerNamespaces();
            emptyNs.Add("cos", "http://mes.gov/costreports");
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, request, emptyNs);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                    + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:cos", "http://mes.gov/costreports");
            var claimsXML = xmlDoc.InnerXml.ToString().Replace("submitCostSettlementReportRequest", "cos:SubmitCostSettlementReport")
                                .Replace("MessageHeader", "cos:MessageHeader")
                                .Replace("Payload", "cos:Payload");
            return claimsXML;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }
    private string GenerateClaimsAddUpdateRequestXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("ClaimsWSUserName");
            string client_secret = "";
            string secretName = "ClaimsWSPassword_2_OH_PNM_";
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
            string wSPassword = client_secret;
            string subscriberSystem = tbSubscriber.Text.Trim();
            string SubscriberId = tbSubscriberId.Text.Trim();
       
            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            List<ClaimsDentalReference.MessageHeaderTypeSubscriber> msgHeaderSubList = new List<ClaimsDentalReference.MessageHeaderTypeSubscriber>();

            if (subscriberSystem.ToUpper() == "EDI")
            {
                msgHeaderSubList.Add(ClaimsDentalReference.MessageHeaderTypeSubscriber.EDI);
            }
            else
            {
                msgHeaderSubList.Add(ClaimsDentalReference.MessageHeaderTypeSubscriber.FI);
            }


            ClaimsDentalReference.MessageHeaderType msgHeader = new ClaimsDentalReference.MessageHeaderType
            {
                BusinessFlow = ClaimsDentalReference.MessageHeaderTypeBusinessFlow.AddUpdateClaims,
                RequestorSystem = ClaimsDentalReference.MessageHeaderTypeRequestorSystem.PNM,
                RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss")),
                StateCode = "OH",
                SubscriberSystem = msgHeaderSubList.ToArray()
            };

    
            


            ClaimsDentalReference.ISAType iSAType= new ClaimsDentalReference.ISAType();
            iSAType.ISA01_AuthorizationInformationQualifier = "00";
            iSAType.ISA02_AuthorizationInformation = "          ";
            iSAType.ISA03_SecurityInformationQualifier = "00";
            iSAType.ISA04_SecurityInformation= "          ";
            iSAType.ISA05_InterchangeIdQualifier = "ZZ";
            iSAType.ISA06_InterchangeSenderId = "MMISODFS";
            iSAType.ISA07_InterchangeIdQualifier = "ZZ";
            iSAType.ISA08_InterchangeReceiverId = SubscriberId.ToString();
            iSAType.ISA15_InterchangeUsageIndicator = "T";
            iSAType.ISA12_InterchangeControlVersionNumber = "00501";
            iSAType.ISA09_InterchangeDate = DateTime.Now.ToString("yyyyMMdd");

            iSAType.ISA14_AcknowledgementRequested = "1";

            ClaimsDentalReference.GSType gSType = new ClaimsDentalReference.GSType();
            gSType.GS01_FunctionalIdentifierCode = "HC";
            gSType.GS02_ApplicationSendersCode = "MMISODFS";
            gSType.GS03_ApplicationReceiversCode= SubscriberId.ToString();
            gSType.GS04_Date = DateTime.Now.ToString("yyyyMMdd");
            gSType.GS05_Time= iSAType.ISA10_InterchangeTime = DateTime.Now.ToString("HHMM");
            gSType.GS06_GroupControlNumber = "99";
            gSType.GS07_ResponsibleAgencyCode = "X";
            gSType.GS08_VersionReleaseIndustryIdentifierCode = "005010X224A2";


            ClaimsDentalReference.STType sTType = new ClaimsDentalReference.STType();
            sTType.ST01_TransactionSetIdentifierCode = "837";
            sTType.ST02_TransactionSetControlNumber = "01";

            
            ClaimsDentalReference.BHTType bHTType = new ClaimsDentalReference.BHTType();
            bHTType.BHT01_HierarchicalStructureCode = "0019";
            bHTType.BHT02_TransactionSetPurposeCode = "";
            bHTType.BHT03_ReferenceIdentification = "";
            bHTType.BHT04_Date = DateTime.Now.ToString("yyyyMMdd");
            bHTType.BHT05_Time = DateTime.Now.ToString("HHMMSS");
            bHTType.BHT06_TransactionTypeCode = "CH";

            ClaimsDentalReference.SEType seType = new ClaimsDentalReference.SEType();
            seType.SE01_TransactionSegmentCount = "";
            seType.SE02_TransactionSetControlNumber = "01";

            ClaimsDentalReference.GEType gEType = new  ClaimsDentalReference.GEType();
            gEType.GE02_GroupControlNumber = "09";
            gEType.GE01_NumberTransactionSetsIncluded = "1";


            ClaimsDentalReference.IEAType iEAType = new ClaimsDentalReference.IEAType();
            iEAType.IEA01_NumberIncludedFunctionalGroups = "1";
            iEAType.IEA02_InterchangeControlNumber = "99";

            ClaimsDentalReference.BHTContainterType bHTContainterType = new ClaimsDentalReference.BHTContainterType();
           


            ClaimsDentalReference.DentalClaim837Type dt837 = new ClaimsDentalReference.DentalClaim837Type();
            dt837.TransactionSetHeader = sTType;
            dt837.InterchangeControlHeader = iSAType;
            dt837.InterchangeControlTrailer = iEAType;
            dt837.TransactionSetTrailer = seType;
            dt837.FunctionalGroupHeader = gSType;
            dt837.FunctionalGroupTrailer = gEType;
            dt837.BHTContainter = bHTContainterType;


            ClaimsDentalReference.AddUpdateClaimsRequest addupdateRequest = new ClaimsDentalReference.AddUpdateClaimsRequest();
            addupdateRequest.DentalClaim837 = dt837;
            addupdateRequest.MessageHeader = msgHeader;


            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ClaimsDentalReference.AddUpdateClaimsRequest));
           // var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("den", "http://service.operationmgmt.fi/ClaimsService/dental");

            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, addupdateRequest, ns);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }

            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                  + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                  Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:den", "http://service.operationmgmt.fi/ClaimsService/dental");
            var claimsXML = xmlDoc.InnerXml.ToString().Replace("AddUpdateClaimsRequest xmlns:den=\"http://service.operationmgmt.fi/ClaimsService/dental\"", "AddUpdateClaimsRequest")
                                .Replace("AddUpdateClaimsRequest", "den:AddUpdateClaimsRequest")
                                .Replace("MessageHeader", "den:MessageHeader")
                                .Replace("DentalClaim837", "den:DentalClaim837"); 


          
            return claimsXML;
          
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }
    private string GenerateClaimsAddUpdateProfRequestXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("ClaimsWSUserName");
            string client_secret = "";
            string secretName = "ClaimsWSPassword_2_OH_PNM_";
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
            string wSPassword = client_secret;
            string subscriberSystem = tbSubscriber.Text.Trim();
            string SubscriberId = tbSubscriberId.Text.Trim();

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            List<ClaimsProfessionalReference.MessageHeaderTypeSubscriber> msgHeaderSubList = new List<ClaimsProfessionalReference.MessageHeaderTypeSubscriber>();

            if (subscriberSystem.ToUpper() == "EDI")
            {
                msgHeaderSubList.Add(ClaimsProfessionalReference.MessageHeaderTypeSubscriber.EDI);
            }
            else
            {
                msgHeaderSubList.Add(ClaimsProfessionalReference.MessageHeaderTypeSubscriber.FI);
            }


            ClaimsProfessionalReference.MessageHeaderType msgHeader = new ClaimsProfessionalReference.MessageHeaderType
            {
                BusinessFlow = ClaimsProfessionalReference.MessageHeaderTypeBusinessFlow.AddUpdateClaims,
                RequestorSystem = ClaimsProfessionalReference.MessageHeaderTypeRequestorSystem.PNM,
                RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss")),
                StateCode = "OH",
                SubscriberSystem = msgHeaderSubList.ToArray()
            };





            ClaimsProfessionalReference.ISAType iSAType = new ClaimsProfessionalReference.ISAType();
            iSAType.ISA01_AuthorizationInformationQualifier = "00";
            iSAType.ISA02_AuthorizationInformation = "          ";
            iSAType.ISA03_SecurityInformationQualifier = "00";
            iSAType.ISA04_SecurityInformation = "          ";
            iSAType.ISA05_InterchangeIdQualifier = "ZZ";
            iSAType.ISA06_InterchangeSenderId = "MMISODFS";
            iSAType.ISA07_InterchangeIdQualifier = "ZZ";
            iSAType.ISA08_InterchangeReceiverId = SubscriberId.ToString();
            iSAType.ISA15_InterchangeUsageIndicator = "T";
            iSAType.ISA12_InterchangeControlVersionNumber = "00501";
            iSAType.ISA09_InterchangeDate = DateTime.Now.ToString("yyyyMMdd");

            iSAType.ISA14_AcknowledgementRequested = "1";

            ClaimsProfessionalReference.GSType gSType = new ClaimsProfessionalReference.GSType();
            gSType.GS01_FunctionalIdentifierCode = "HC";
            gSType.GS02_ApplicationSendersCode = "MMISODFS";
            gSType.GS03_ApplicationReceiversCode = SubscriberId.ToString();
            gSType.GS04_Date = DateTime.Now.ToString("yyyyMMdd");
            gSType.GS05_Time = iSAType.ISA10_InterchangeTime = DateTime.Now.ToString("HHMM");
            gSType.GS06_GroupControlNumber = "99";
            gSType.GS07_ResponsibleAgencyCode = "X";
            gSType.GS08_VersionReleaseIndustryIdentifierCode = "005010X224A2";


            ClaimsProfessionalReference.STType sTType = new ClaimsProfessionalReference.STType();
            sTType.ST01_TransactionSetIdentifierCode = "837";
            sTType.ST02_TransactionSetControlNumber = "01";


            ClaimsProfessionalReference.BHTType bHTType = new ClaimsProfessionalReference.BHTType();
            //bHTType.BHT01_HierarchicalStructureCode = "0019";
            //bHTType.BHT02_TransactionSetPurposeCode = "";
            //bHTType.BHT03_ReferenceIdentification = "";
            bHTType.BHT04_Date = DateTime.Now.ToString("yyyyMMdd");
            bHTType.BHT05_Time = DateTime.Now.ToString("HHMMSS");
            bHTType.BHT06_TransactionTypeCode = "CH";

            ClaimsProfessionalReference.SEType seType = new ClaimsProfessionalReference.SEType();
            seType.SE01_TransactionSegmentCount = "";
            seType.SE02_TransactionSetControlNumber = "01";

            ClaimsProfessionalReference.GEType gEType = new ClaimsProfessionalReference.GEType();
            gEType.GE02_GroupControlNumber = "09";
            gEType.GE01_NumberTransactionSetsIncluded = "1";


            ClaimsProfessionalReference.IEAType iEAType = new ClaimsProfessionalReference.IEAType();
            iEAType.IEA01_NumberIncludedFunctionalGroups = "1";
            iEAType.IEA02_InterchangeControlNumber = "99";

            ClaimsProfessionalReference.BHTContainterType bHTContainterType = new ClaimsProfessionalReference.BHTContainterType();



            ClaimsProfessionalReference.ProfessionalClaim837Type dt837 = new ClaimsProfessionalReference.ProfessionalClaim837Type();
            dt837.TransactionSetHeader = sTType;
            dt837.InterchangeControlHeader = iSAType;
            dt837.InterchangeControlTrailer = iEAType;
            dt837.TransactionSetTrailer = seType;
            dt837.FunctionalGroupHeader = gSType;
            dt837.FunctionalGroupTrailer = gEType;
            dt837.BHTContainter = bHTContainterType;


            ClaimsProfessionalReference.AddUpdateClaimsRequest addupdateRequest = new ClaimsProfessionalReference.AddUpdateClaimsRequest();
            addupdateRequest.ProfessionalClaim837 = dt837;
            addupdateRequest.MessageHeader = msgHeader;


            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ClaimsProfessionalReference.AddUpdateClaimsRequest));
           // var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("prof", "http://service.operationmgmt.fi/ClaimsService/professional");

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, addupdateRequest, ns);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }

            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                  + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                  Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:prof", "http://service.operationmgmt.fi/ClaimsService/professional");
            var claimsXML = xmlDoc.InnerXml.ToString().Replace("AddUpdateClaimsRequest xmlns:prof=\"http://service.operationmgmt.fi/ClaimsService/professional\"", "AddUpdateClaimsRequest")
                                  .Replace("AddUpdateClaimsRequest", "prof:AddUpdateClaimsRequest")
                                 .Replace("MessageHeader", "prof:MessageHeader")
                                .Replace("ProfessionalClaim837", "prof:ProfessionalClaim837");
            return claimsXML;

        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }
    private string GenerateClaimsAddUpdateInstRequestXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("ClaimsWSUserName");
            string client_secret = "";
            string secretName = "ClaimsWSPassword_2_OH_PNM_";
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
            string wSPassword = client_secret;
            string subscriberSystem = tbSubscriber.Text.Trim();
            string SubscriberId = tbSubscriberId.Text.Trim();

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            List<ClaimsInstitutionalReference.MessageHeaderTypeSubscriber> msgHeaderSubList = new List<ClaimsInstitutionalReference.MessageHeaderTypeSubscriber>();

            if (subscriberSystem.ToUpper() == "EDI")
            {
                msgHeaderSubList.Add(ClaimsInstitutionalReference.MessageHeaderTypeSubscriber.EDI);
            }
            else
            {
                msgHeaderSubList.Add(ClaimsInstitutionalReference.MessageHeaderTypeSubscriber.FI);
            }


            ClaimsInstitutionalReference.MessageHeaderType msgHeader = new ClaimsInstitutionalReference.MessageHeaderType
            {
                BusinessFlow = ClaimsInstitutionalReference.MessageHeaderTypeBusinessFlow.AddUpdateClaims,
                RequestorSystem = ClaimsInstitutionalReference.MessageHeaderTypeRequestorSystem.PNM,
                RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss")),
                StateCode = "OH",
                SubscriberSystem = msgHeaderSubList.ToArray()
            };





            ClaimsInstitutionalReference.ISAType iSAType = new ClaimsInstitutionalReference.ISAType();
            iSAType.ISA01_AuthorizationInformationQualifier = "00";
            iSAType.ISA02_AuthorizationInformation = "          ";
            iSAType.ISA03_SecurityInformationQualifier = "00";
            iSAType.ISA04_SecurityInformation = "          ";
            iSAType.ISA05_InterchangeIdQualifier = "ZZ";
            iSAType.ISA06_InterchangeSenderId = "MMISODFS";
            iSAType.ISA07_InterchangeIdQualifier = "ZZ";
            iSAType.ISA08_InterchangeReceiverId = SubscriberId.ToString();
            iSAType.ISA15_InterchangeUsageIndicator = "T";
            iSAType.ISA12_InterchangeControlVersionNumber = "00501";
            iSAType.ISA09_InterchangeDate = DateTime.Now.ToString("yyyyMMdd");

            iSAType.ISA14_AcknowledgementRequested = "1";

            ClaimsInstitutionalReference.GSType gSType = new ClaimsInstitutionalReference.GSType();
            gSType.GS01_FunctionalIdentifierCode = "HC";
            gSType.GS02_ApplicationSendersCode = "MMISODFS";
            gSType.GS03_ApplicationReceiversCode = SubscriberId.ToString();
            gSType.GS04_Date = DateTime.Now.ToString("yyyyMMdd");
            gSType.GS05_Time = iSAType.ISA10_InterchangeTime = DateTime.Now.ToString("HHMM");
            gSType.GS06_GroupControlNumber = "99";
            gSType.GS07_ResponsibleAgencyCode = "X";
            gSType.GS08_VersionReleaseIndustryIdentifierCode = "005010X224A2";


            ClaimsInstitutionalReference.STType sTType = new ClaimsInstitutionalReference.STType();
            sTType.ST01_TransactionSetIdentifierCode = "837";
            sTType.ST02_TransactionSetControlNumber = "01";


            ClaimsInstitutionalReference.BHTType bHTType = new ClaimsInstitutionalReference.BHTType();
            bHTType.BHT01_HierarchicalStructureCode = "0019";
            bHTType.BHT02_TransactionSetPurposeCode = "";
            //bHTType.BHT03_ReferenceIdentification = "";
            //bHTType.BHT04_Date = DateTime.Now.ToString("CCYYMMDD");
            //bHTType.BHT05_Time = DateTime.Now.ToString("HHMMSS");
            bHTType.BHT06_TransactionTypeCode = "CH";

            ClaimsInstitutionalReference.SEType seType = new ClaimsInstitutionalReference.SEType();
            seType.SE01_TransactionSegmentCount = "";
            seType.SE02_TransactionSetControlNumber = "01";

            ClaimsInstitutionalReference.GEType gEType = new ClaimsInstitutionalReference.GEType();
            gEType.GE02_GroupControlNumber = "09";
            gEType.GE01_NumberTransactionSetsIncluded = "1";


            ClaimsInstitutionalReference.IEAType iEAType = new ClaimsInstitutionalReference.IEAType();
            iEAType.IEA01_NumberIncludedFunctionalGroups = "1";
            iEAType.IEA02_InterchangeControlNumber = "99";

            ClaimsInstitutionalReference.BHTContainterType bHTContainterType = new ClaimsInstitutionalReference.BHTContainterType();



            ClaimsInstitutionalReference.InstitutionalClaim837Type inst837 = new ClaimsInstitutionalReference.InstitutionalClaim837Type();
            inst837.TransactionSetHeader = sTType;
            inst837.InterchangeControlHeader = iSAType;
            inst837.InterchangeControlTrailer = iEAType;
            inst837.TransactionSetTrailer = seType;
            inst837.FunctionalGroupHeader = gSType;
            inst837.FunctionalGroupTrailer = gEType;
            inst837.BHTContainter = bHTContainterType;


            ClaimsInstitutionalReference.AddUpdateClaimsRequest addupdateRequest = new ClaimsInstitutionalReference.AddUpdateClaimsRequest();
            addupdateRequest.InstitutionalClaim837 = inst837;
            addupdateRequest.MessageHeader = msgHeader;


            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ClaimsInstitutionalReference.AddUpdateClaimsRequest));
           // var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("ins", "http://service.operationmgmt.fi/ClaimsService/institutional");

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, addupdateRequest, ns);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }

            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                  + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                  Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:ins", "http://service.operationmgmt.fi/ClaimsService/institutional");
            var claimsXML = xmlDoc.InnerXml.ToString().Replace("AddUpdateClaimsRequest xmlns:ins=\"http://service.operationmgmt.fi/ClaimsService/institutional\"", "AddUpdateClaimsRequest")
                                                      .Replace("AddUpdateClaimsRequest", "ins:AddUpdateClaimsRequest")
                                                      .Replace("MessageHeader", "ins:MessageHeader")
                                                      .Replace("InstitutionalClaim837", "ins:InstitutionalClaim837");
            return claimsXML;

        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }
    private string GenerateClaimsInquiryRequestXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("ClaimsWSUserName");
            string client_secret = "";
            string secretName = "ClaimsWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            client_secret = "";
            secretName = "ClaimsWSPassword_2_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword2 = client_secret;
            string subscriberSystem = tbSubscriber.Text.Trim();
            string SubscriberId = tbSubscriberId.Text.Trim();
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            List<ClaimsDentalReference.MessageHeaderTypeSubscriber> msgHeaderSubList = new List<ClaimsDentalReference.MessageHeaderTypeSubscriber>();
            if (subscriberSystem.ToUpper() == "EDI")
            {
                msgHeaderSubList.Add(ClaimsDentalReference.MessageHeaderTypeSubscriber.EDI);
            }
            else
            {
                msgHeaderSubList.Add(ClaimsDentalReference.MessageHeaderTypeSubscriber.FI);
            }

            ClaimsDentalReference.MessageHeaderType msgHeader = new ClaimsDentalReference.MessageHeaderType
            {
                BusinessFlow = ClaimsDentalReference.MessageHeaderTypeBusinessFlow.InquireClaim,
                RequestorSystem = ClaimsDentalReference.MessageHeaderTypeRequestorSystem.PNM,
                RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss")),
                StateCode = "OH",
                SubscriberSystem = msgHeaderSubList.ToArray()
            };


            //ClaimsDentalReference.InquireClaimRequestPayloadType ptDentalInquire = new ClaimsDentalReference.InquireClaimRequestPayloadType();
            //ptDentalInquire.ICN = "123456788";
            //ptDentalInquire.PayorType = subscriberSystem.ToUpper() == "EDI"? ClaimsDentalReference.InquireClaimRequestPayloadTypePayorType.MCE: ClaimsDentalReference.InquireClaimRequestPayloadTypePayorType.FFS;
            //ptDentalInquire.ProviderId = "01254368";

            ClaimsDentalReference.InquireClaimRequestPayloadType ptDentalInquire = new ClaimsDentalReference.InquireClaimRequestPayloadType();
            ptDentalInquire.ICN = "123456788";
            ptDentalInquire.PayorType = "";
            ptDentalInquire.ProviderId = "01254368";


            ClaimsDentalReference.InquireClaimRequest inquireClaimRequest = new ClaimsDentalReference.InquireClaimRequest();
            inquireClaimRequest.MessageHeader = msgHeader;
            inquireClaimRequest.RequestPayload = ptDentalInquire;


            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ClaimsDentalReference.InquireClaimRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("den", "http://service.operationmgmt.fi/ClaimsService/dental");

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, inquireClaimRequest, ns);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword2);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }

       

            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                  + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword2) + Environment.NewLine + "<soapenv:Body>" +
                  Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:den", "http://service.operationmgmt.fi/ClaimsService/dental");
            var claimsXML = xmlDoc.InnerXml.ToString().Replace("InquireClaimRequest xmlns:den=\"http://service.operationmgmt.fi/ClaimsService/dental\"", "InquireClaimRequest")
                                                       .Replace("InquireClaimRequest", "den:InquireClaimRequest")
                                                       .Replace("MessageHeader", "den:MessageHeader")
                                                      .Replace("RequestPayload", "den:RequestPayload");
            return claimsXML;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }
    private string GenerateClaimsProfessionalInquiryRequestXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("ClaimsWSUserName");
            string client_secret = "";
            string secretName = "ClaimsWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            client_secret = "";
            secretName = "ClaimsWSPassword_2_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword2 = client_secret;
            string subscriberSystem = tbSubscriber.Text.Trim();
            string SubscriberId = tbSubscriberId.Text.Trim();
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            List<ClaimsProfessionalReference.MessageHeaderTypeSubscriber> msgHeaderSubList = new List<ClaimsProfessionalReference.MessageHeaderTypeSubscriber>();
       

            if (subscriberSystem.ToUpper() == "EDI")
            {
                msgHeaderSubList.Add(ClaimsProfessionalReference.MessageHeaderTypeSubscriber.EDI);
            }
            else
            {
                msgHeaderSubList.Add(ClaimsProfessionalReference.MessageHeaderTypeSubscriber.FI);
            }


            ClaimsProfessionalReference.MessageHeaderType msgHeader = new ClaimsProfessionalReference.MessageHeaderType
            {
                BusinessFlow = ClaimsProfessionalReference.MessageHeaderTypeBusinessFlow.InquireClaim,
                RequestorSystem = ClaimsProfessionalReference.MessageHeaderTypeRequestorSystem.PNM,
                RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss")),
                StateCode = "OH",
                SubscriberSystem = msgHeaderSubList.ToArray()
            };


            ClaimsProfessionalReference.InquireClaimRequestPayloadType ptInquire = new ClaimsProfessionalReference.InquireClaimRequestPayloadType();
            ptInquire.ICN = "123456788";
            ptInquire.PayorType = subscriberSystem.ToUpper() == "EDI" ? "MCE" : "FFS";
            ptInquire.ProviderId = "01254368";



            ClaimsProfessionalReference.InquireClaimRequest inquireClaimRequest = new ClaimsProfessionalReference.InquireClaimRequest();
            inquireClaimRequest.MessageHeader = msgHeader;
            inquireClaimRequest.RequestPayload = ptInquire;


            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ClaimsProfessionalReference.InquireClaimRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("prof", "http://service.operationmgmt.fi/ClaimsService/professional");

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, inquireClaimRequest, ns);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword2);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }

            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                  + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword2) + Environment.NewLine + "<soapenv:Body>" +
                  Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:prof", "http://service.operationmgmt.fi/ClaimsService/professional");
            var claimsXML = xmlDoc.InnerXml.ToString().Replace("InquireClaimRequest xmlns:prof=\"http://service.operationmgmt.fi/ClaimsService/professional\"", "InquireClaimRequest")
                                                      .Replace("InquireClaimRequest", "prof:InquireClaimRequest")
                                                       .Replace("MessageHeader", "prof:MessageHeader")
                                                      .Replace("RequestPayload", "prof:RequestPayload");
            return claimsXML;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }
    private string GenerateClaimsInstitionalInquiryRequestXml()
    {
        try
        {
            string wSUserName =  AppSettings.Get("ClaimsWSUserName");
            string client_secret = "";
            string secretName = "ClaimsWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            client_secret = "";
            secretName = "ClaimsWSPassword_2_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword2 = client_secret;
            string subscriberSystem = tbSubscriber.Text.Trim();
            string SubscriberId = tbSubscriberId.Text.Trim();
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            List<ClaimsInstitutionalReference.MessageHeaderTypeSubscriber> msgHeaderSubList = new List<ClaimsInstitutionalReference.MessageHeaderTypeSubscriber>();

            if (subscriberSystem.ToUpper() == "EDI")
            {
                msgHeaderSubList.Add(ClaimsInstitutionalReference.MessageHeaderTypeSubscriber.EDI);
            }
            else
            {
                msgHeaderSubList.Add(ClaimsInstitutionalReference.MessageHeaderTypeSubscriber.FI);
            }

            ClaimsInstitutionalReference.MessageHeaderType msgHeader = new ClaimsInstitutionalReference.MessageHeaderType
            {
                BusinessFlow = ClaimsInstitutionalReference.MessageHeaderTypeBusinessFlow.InquireClaim,
                RequestorSystem = ClaimsInstitutionalReference.MessageHeaderTypeRequestorSystem.PNM,
                RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss")),
                StateCode = "OH",
                SubscriberSystem = msgHeaderSubList.ToArray()
            };


            ClaimsInstitutionalReference.InquireClaimRequestPayloadType ptInquire = new ClaimsInstitutionalReference.InquireClaimRequestPayloadType();
            ptInquire.ICN = "123456788";
            ptInquire.ProviderId = "01254368";
            ptInquire.PayorType = subscriberSystem.ToUpper() == "EDI" ? "MCE" : "FFS";
          
            ClaimsInstitutionalReference.InquireClaimRequest inquireClaimRequest = new ClaimsInstitutionalReference.InquireClaimRequest();
            inquireClaimRequest.MessageHeader = msgHeader;
            inquireClaimRequest.RequestPayload = ptInquire;


            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ClaimsInstitutionalReference.InquireClaimRequest));
           // var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("ins", "http://service.operationmgmt.fi/ClaimsService/institutional");

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, inquireClaimRequest, ns);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword2);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }

            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                  + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword2) + Environment.NewLine + "<soapenv:Body>" +
                  Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:ins", "http://service.operationmgmt.fi/ClaimsService/institutional");
            var claimsXML = xmlDoc.InnerXml.ToString().Replace("InquireClaimRequest xmlns:ins=\"http://service.operationmgmt.fi/ClaimsService/institutional\"", "InquireClaimRequest")
                                                      .Replace("InquireClaimRequest", "ins:InquireClaimRequest")
                                                      .Replace("MessageHeader", "ins:MessageHeader")
                                                      .Replace("RequestPayload", "ins:RequestPayload");
            return claimsXML;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return string.Empty;
    }
    private string GenerateHospiceXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("HospiceWSUserName");
            string client_secret = "";
            string secretName = "HospiceWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            SearchRequestMessageHeader msgHeader = new SearchRequestMessageHeader();
            msgHeader.BusinessFlow = "SearchHospice";
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = SearchRequestMessageHeaderRequestorSystem.PNM;
            var subsriberSystem = new List<SearchRequestMessageHeaderSubscriber>();
            subsriberSystem.Add(SearchRequestMessageHeaderSubscriber.FI);
            msgHeader.SubscriberSystem = subsriberSystem.ToArray();
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss"));
            msgHeader.SITransactionKey = sitTransactionKey;
            SearchHospiceRequest searchRequest = new SearchHospiceRequest
            {
                MessageHeader = msgHeader,
                Payload = new SearchRequestPayload
                {
                    HospiceTrackNo = 658,
                    ProvMedID = "2422659",
                    RecipID="",
                    ProvNPI="",
                    Offset=1,
                    Count=1
                }
            };

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(SearchHospiceRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, searchRequest, emptyNs);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                    + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:sear", "http://ohio.gov/SearchRequest");
            var root = xmlDoc.GetElementsByTagName("soapenv:Body")[0];
            SetPrefix("sear", root.ChildNodes[0]);
            var hospicexml = xmlDoc.InnerXml.ToString().Replace("SearchRequest>", "sear:SearchRequest>")
                .Replace("MessageHeader", "sear:MessageHeader")
                .Replace("Payload", "sear:Payload");
            hospicexml = RemoveAllXmlNamespace(hospicexml);
            return hospicexml;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }
    private  void SetPrefix(string prefix, XmlNode node)
    {
        node.Prefix = prefix;
        foreach (XmlNode n in node.ChildNodes)
        {
            SetPrefix(prefix, n);
        }
    }
    public  string RemoveAllXmlNamespace(string xmlData)
    {
        string xmlnsPattern = "\\s+xmlns\\s*(:\\w)?\\s*=\\s*\\\"(?<url>[^\\\"]*)\\\"";
        MatchCollection matchCol = Regex.Matches(xmlData, xmlnsPattern);

        foreach (System.Text.RegularExpressions.Match m in matchCol)
        {
            xmlData = xmlData.Replace(m.ToString(), "");
        }
        return xmlData;
    }
    private string GenerateHospiceInquireRequestXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("HospiceWSUserName");
            string client_secret = "";
            string secretName = "HospiceWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            InquireRequestMessageHeader msgHeader = new InquireRequestMessageHeader();
            msgHeader.BusinessFlow = "InquireHospice";
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = InquireRequestMessageHeaderRequestorSystem.PNM;
            var subsriberSystem = new List<InquireRequestMessageHeaderSubscriber>();
            subsriberSystem.Add(InquireRequestMessageHeaderSubscriber.FI);
            msgHeader.SubscriberSystem = subsriberSystem.ToArray();
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss"));
            msgHeader.SITransactionKey = sitTransactionKey;
            InquireHospiceRequest InquireRequest = new InquireHospiceRequest
            {
                MessageHeader = msgHeader,
                Payload = new InquireRequestPayload
                {
                    HospiceTrackNo = 844,
                    ProvMedID = "2422659"
                }
            };

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(InquireHospiceRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, InquireRequest, emptyNs);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                    + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
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
            return hospicexml;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }

    private string GenerateHospiceAddUpdateXml()
    {
        try
        {
            string wSUserName = AppSettings.Get("HospiceWSUserName");
            string client_secret = "";
            string secretName = "HospiceWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            HospiceRequestResponseMessageHeader msgHeader = new HospiceRequestResponseMessageHeader();
            msgHeader.BusinessFlow = "AddUpdateHospice";
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = HospiceRequestResponseMessageHeaderRequestorSystem.PNM;
            var subsriberSystem = new List<HospiceRequestResponseMessageHeaderSubscriber>();
            subsriberSystem.Add(HospiceRequestResponseMessageHeaderSubscriber.FI);
            msgHeader.SubscriberSystem = subsriberSystem.ToArray();
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss"));
            msgHeader.SITransactionKey = sitTransactionKey;
            List<HospiceRequestResponsePayloadServiceCountyState> serviceCountyState = new List<HospiceRequestResponsePayloadServiceCountyState>
            {
                new HospiceRequestResponsePayloadServiceCountyState
                {
                    County= "Coshocton County",
                    CountyEffDate=Convert.ToDateTime("2013-12-18"),
                    CountyEndDate=Convert.ToDateTime("2014-03-17"),
                    State="OH",
                    BenPeriod =1
                }
            };
            HospiceRequestResponsePayloadElectionDisenrollDates electionDisenrollDates = new HospiceRequestResponsePayloadElectionDisenrollDates
            {
                ElectionDate = Convert.ToDateTime("2013-12-18"),
                DisenrollDate = Convert.ToDateTime("2014-02-24"),
            };
            List<HospiceRequestResponsePayloadLongTermCareFacility> longTermCareFacilities = new List<HospiceRequestResponsePayloadLongTermCareFacility>
            {
                new HospiceRequestResponsePayloadLongTermCareFacility
                {
                     HLTCFEffDate=Convert.ToDateTime("2013-12-18"),
                     HLTCFEndDate=Convert.ToDateTime("2014-03-16"),
                     HLTCFProvMedID="0234428",
                      BenPeriod =1
                }
            };
            List<HospiceRequestResponsePayloadBenefitPeriods> payloadBenefitPeriods = new List<HospiceRequestResponsePayloadBenefitPeriods>
            {
                new HospiceRequestResponsePayloadBenefitPeriods
                {
                     PhyNPI="1588633796",
                     PhyOralCertDate = Convert.ToDateTime("2013-12-18"),
                     PhyWritCertDate = Convert.ToDateTime("2013-12-18"),
                     BenPeriod=1,
                     BenPeriodEffDate= Convert.ToDateTime("2013-12-18"),
                     BenPeriodEndDate= Convert.ToDateTime("2014-03-17"),
                     Status="P",
                     IDGPhyNPI="1447202106",
                     IDGPhyOralCertDate= Convert.ToDateTime("2013-12-18"),
                     IDGPhyWritCertDate= Convert.ToDateTime("2013-12-18"),
                     SubmissionDate= Convert.ToDateTime("2014-02-24")
                }
            };
            List<HospiceRequestResponsePayloadDiagnosisCodes> payloadDiagnosisCodes = new List<HospiceRequestResponsePayloadDiagnosisCodes>
            {
                new HospiceRequestResponsePayloadDiagnosisCodes
                {
                    DiagEffDate=Convert.ToDateTime("2013-12-18"),
                    DiagEndDate=Convert.ToDateTime("2014-03-17"),
                    PrimeTermDiag="436",
                     BenPeriod =1
                }
            };
            List<HospiceRequestResponsePayloadProvService> payloadProvServices = new List<HospiceRequestResponsePayloadProvService>
            {
                new HospiceRequestResponsePayloadProvService
                {
                    HospiceProvID="2422659",
                    SpanEffDate=Convert.ToDateTime("2013-12-18"),
                    SpanEndDate=Convert.ToDateTime("2014-03-17"),
                     BenPeriod =1
                }
            };

            AddUpdateHospiceRequest AddUpdateRequest = new AddUpdateHospiceRequest
            {
                MessageHeader = msgHeader,
                Payload = new HospiceRequestResponsePayload
                {
                    HospiceTrackNo = 844,
                    RecipID = "104396079699",
                    ActionType = "MAINT",
                    ConsBirthDate = Convert.ToDateTime("1924-02-08"),
                    ConsFirstName = "EILEEN",
                    ConsLastName = "HALCOMB",
                    ServiceCountyState = serviceCountyState.ToArray(),
                    ElectionDisenrollDates = electionDisenrollDates,
                    LongTermCareFacility = longTermCareFacilities.ToArray(),
                    BenefitPeriods = payloadBenefitPeriods.ToArray(),
                    DiagnosisCodes = payloadDiagnosisCodes.ToArray(),
                    ProvService = payloadProvServices.ToArray()
                }
            };


            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(AddUpdateHospiceRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, AddUpdateRequest, emptyNs);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                    + Environment.NewLine + wssSecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.DocumentElement.SetAttribute("xmlns:hos", "http://ohio.gov/HospiceRequestResponse");
            var root = xmlDoc.GetElementsByTagName("soapenv:Body")[0];
            SetPrefix("hos", root.ChildNodes[0]);
            var hospicexml = xmlDoc.InnerXml.ToString().Replace("HospiceRequestResponse>", "hos:HospiceRequestResponse>")
                .Replace("MessageHeader", "hos:MessageHeader")
                .Replace("Payload", "hos:Payload");
            hospicexml = RemoveAllXmlNamespace(hospicexml);
            return hospicexml;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }
    private string GenerateDocumentRequestXml()
    {
        try
        {
            string url = AppSettings.Get("DocumentWSURL");
            string wsHost = AppSettings.Get("DocumentWSHost");
            string wsCertificateName = AppSettings.Get("DocumentWSCertificateName");
            string wSUserName = AppSettings.Get("DocumentWSUserName");
            string client_secret = "";
            string secretName = "DocumentWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            DataSet ds = new DataSet();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            Corp.Core.Libraries.AttachmentServiceReference.MessageHeader msgHeader = new Corp.Core.Libraries.AttachmentServiceReference.MessageHeader();
            List<Corp.Core.Libraries.AttachmentServiceReference.InqMessageHeaderSubscriber> inqMessageHeaderSubscriber = new List<Corp.Core.Libraries.AttachmentServiceReference.InqMessageHeaderSubscriber>();
            inqMessageHeaderSubscriber.Add(Corp.Core.Libraries.AttachmentServiceReference.InqMessageHeaderSubscriber.MITS);
            msgHeader.BusinessFlow = Corp.Core.Libraries.AttachmentServiceReference.InqMessageHeaderBusinessFlow.sendAttachment;
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = Corp.Core.Libraries.AttachmentServiceReference.InqMessageHeaderRequestorSystem.PNM;
            msgHeader.SubscriberSystem = inqMessageHeaderSubscriber.ToArray();
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
            msgHeader.SITransactionKey = sitTransactionKey;
            List<Corp.Core.Libraries.AttachmentServiceReference.SendAttachmentData> identifiers = new List<Corp.Core.Libraries.AttachmentServiceReference.SendAttachmentData>();
            identifiers.Add(new Corp.Core.Libraries.AttachmentServiceReference.SendAttachmentData
            {
                DocXrefType = "251",
                IndexId = "218277"
            });
            List<Corp.Core.Libraries.AttachmentServiceReference.SendAttachment> attachmentData = new List<Corp.Core.Libraries.AttachmentServiceReference.SendAttachment>
            {
                 new  Corp.Core.Libraries.AttachmentServiceReference.SendAttachment
                 {
                     Identifiers = identifiers.ToArray(),
                     DocumentType="136",
                     DocumentName="LicenseType",
                     DocumentExtension="PDF",
                     AttachmentData64Binary=Convert.FromBase64String("RG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1lRG9jdW1lbnROYW1l")
                 }
            };
            Corp.Core.Libraries.AttachmentServiceReference.sendAttachmentRequest sendAttachment = new Corp.Core.Libraries.AttachmentServiceReference.sendAttachmentRequest
            {
                MessageHeader = msgHeader,
                Payload = new Corp.Core.Libraries.AttachmentServiceReference.SendAttachmentPayload
                {
                    AttachmentInfo = new Corp.Core.Libraries.AttachmentServiceReference.SendAttachmentInformation
                    {
                        AttachmentData = attachmentData.ToArray(),
                        SourceId = "PNM"
                    }
                }
            };

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.AttachmentServiceReference.sendAttachmentRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, sendAttachment, emptyNs);
            string xml = stream2.ToString();
            string wsHeader = wssSecurityHeader(wSUserName, wSPassword);
            if (wsHeader == null)
            {
                txtResponse.Text = "error while creating the wsHeader";
                return string.Empty;
            }

            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"  xmlns:att=\"http://mes.gov/attachment\">"
                   + Environment.NewLine + ServiceAgentHelper.SecurityHeader(wSUserName, wSPassword) + Environment.NewLine + "<soapenv:Body>" +
                   Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            //xmlDoc.DocumentElement.SetAttribute("xmlns:att", "http://ohio.gov/attachment");
            var root = xmlDoc.GetElementsByTagName("soapenv:Body")[0];
            SetPrefix("att", root.ChildNodes[0]);
            var hospicexml = xmlDoc.InnerXml.ToString().Replace("sendAttachmentRequest>", "att:SendAttachment>")
                .Replace("MessageHeader>", "att:MessageHeader>")
                .Replace("Payload>", "att:Payload>")
                .Replace("xmlns=\"http://mes.gov/attachment\" xmlns:att=\"http://mes.gov/attachment\"", "")
                .Replace("xmlns=\"http://mes.gov/attachment\"", "");
            return hospicexml;
        }
        catch (Exception ex)
        {
            txtResponse.Text = string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
        }
        return null;
    }

    public string providerWSManagementUpdateRequest(int transactionID, bool makeRequest = false)
    {
        try
        {
            DataSet ds = InfoAccessController.GetStagingProviderEnrollmentByTransactionID(transactionID);

            if (ds == null)
            {
                return "The data hasn't been staged for this TransactionID: " + transactionID.ToString();
            }

            string providerManagementWSURL = AppSettings.Get("ProviderManagementWSURL", CON.WebServiceURI.ProviderManagement);
            string providerManagementWSUserName = AppSettings.Get("ProviderManagementWSUserName");
            string client_secret = "";
            string secretName = "ProviderManagementWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
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
            string wSPassword = client_secret;
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            Corp.Core.Libraries.ProviderManagementReference.UpdateRequestPayload updateRequestPayload = new Corp.Core.Libraries.ProviderManagementReference.UpdateRequestPayload();
            updateRequestPayload.ProviderInformation = new Corp.Core.Libraries.ProviderManagementReference.UpdateProviderInformation
            {
                ProviderDemographics = ProviderManagementController.fillUpdateProviderDemographic(ds.Tables[6]),
                ProviderAddress = ProviderManagementController.fillUpdateProviderAddress(ds.Tables[0]),
                ProviderTaxonomyClassification = ProviderManagementController.fillTaxonomyClassification(ds.Tables[19]),
                ProviderType = ProviderManagementController.fillProviderType(ds.Tables[20]),
                OwnerRelationship = ProviderManagementController.fillOwnerRelationshipLists(ds.Tables[1]),
                ProviderAffiliations = ProviderManagementController.fillAffiliationList(ds.Tables[2]),
                ProviderAlternateIdentifiers = ProviderManagementController.fillAlternateIdList(ds.Tables[3]),
                ProviderEFTEnrollment = ProviderManagementController.fillEFTEnrollmentList(ds.Tables[4]),
                ProviderLanguage = ProviderManagementController.fillLanguageList(ds.Tables[5]),
                ProviderApplication = ProviderManagementController.fillUpdateProviderApplication(ds.Tables[8]),
                ProviderAttestations = ProviderManagementController.fillProviderAttestationsListAttestations(ds.Tables[9]),
                ProviderBusinessStatus = ProviderManagementController.fillProviderBusinessStatusListBusinessStatus(ds.Tables[10]),
                ProviderCHOP = ProviderManagementController.fillProviderCHOPListCHOP(ds.Tables[11]),
                ProviderContact = ProviderManagementController.fillProviderContactListContact(ds.Tables[12]),
                ProviderManagedEmployees = ProviderManagementController.fillProviderManagedEmployeesListManagedEmployee(ds.Tables[13]),
                ProviderOwnerships = ProviderManagementController.fillUpdateProviderOwnershipList(ds.Tables[31]),
                ProviderProgramAffiliations = ProviderManagementController.fillProgramAffiliationsListProgramAffiliation(ds.Tables[15]),
                ProviderReviews = ProviderManagementController.fillProviderReviewsListReview(ds.Tables[16]),
                ProviderServiceLocation = ProviderManagementController.fillUpdateProviderServiceLocation(ds),
                ProviderServices = ProviderManagementController.fillProviderServicesListServices(ds.Tables[18])
            };
            Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderSubscriber[] imq = new Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderSubscriber[1];
            imq[0] = Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderSubscriber.MITS;

            Corp.Core.Libraries.ProviderManagementReference.MessageHeader msgHeader = new Corp.Core.Libraries.ProviderManagementReference.MessageHeader();
            msgHeader.BusinessFlow = "UpdateProvider";
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderRequestorSystem.PNM;
            msgHeader.ModuleTransactionId = transactionID.ToString();
            msgHeader.SubscriberSystem = imq;
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = ProviderManagementHelper.GetStringDateTime(DateTime.Now);
            msgHeader.SITransactionKey = sitTransactionKey;

            Corp.Core.Libraries.ProviderManagementReference.updateProviderRequest epiPM = new Corp.Core.Libraries.ProviderManagementReference.updateProviderRequest();
            epiPM.MessageHeader = msgHeader;
            epiPM.Payload = updateRequestPayload;

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.ProviderManagementReference.updateProviderRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, epiPM, emptyNs);
            string xml = stream2.ToString();
            client_secret = "";
            secretName = "ProviderManagementWSPassword_OH_PNM_";
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
            string wsHeader = wssSecurityHeader(providerManagementWSUserName, client_secret);

            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://mes.gov/providermanagement\">"
                + Environment.NewLine + wsHeader + Environment.NewLine + "<soapenv:Body>" +
                Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            return xmlDoc.InnerXml.ToString();
        }
        catch (Exception ex)
        {
            return "Something wrong with generating the xml : " + ex.ToString();
        }
    }

    protected void btnIMS_Click(object sender, EventArgs e)
    {
        int operation = 0;
        if (!string.IsNullOrEmpty(txtMedID.Text) && !string.IsNullOrEmpty(txtDate.Text))
        {
            // update registration submit date time
            operation = 1;
        }
        if(!string.IsNullOrEmpty(txtAssociateID.Text) && !string.IsNullOrEmpty(txtDate.Text) 
            &&  !string.IsNullOrEmpty(txtCasenum.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
        {
            // update doc id date and step and process date
            operation = 2;
        }
        if (!string.IsNullOrEmpty(txtAssociateID.Text) && string.IsNullOrEmpty(txtDate.Text)
            && string.IsNullOrEmpty(txtEndDate.Text) && string.IsNullOrEmpty(txtCasenum.Text) && string.IsNullOrEmpty(txtMedID.Text))
        {
            // add back deleted document from audit table;
            operation =3;
        }
        if (operation >0)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(txtMedID.Text))
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, Convert.ToInt32(txtMedID.Text), true));
            if (!string.IsNullOrEmpty(txtAssociateID.Text))
                parameters.Add(SqlParms.CreateParameter("DES_ID", DbType.Int32, Convert.ToInt32(txtAssociateID.Text), true));
            if (!string.IsNullOrEmpty(txtCasenum.Text))
                parameters.Add(SqlParms.CreateParameter("ST_ID", DbType.Int32, Convert.ToInt32(txtCasenum.Text), true));
            if (!string.IsNullOrEmpty(txtDate.Text))
                parameters.Add(SqlParms.CreateParameter("ST_DT", DbType.DateTime, Convert.ToDateTime(txtDate.Text), true));
            if (!string.IsNullOrEmpty(txtEndDate.Text))
                parameters.Add(SqlParms.CreateParameter("ED_DT", DbType.DateTime, Convert.ToDateTime(txtEndDate.Text), true));
            parameters.Add(SqlParms.CreateParameter("OP", DbType.Int32, operation, true));
            DataAccess.ExecuteStoredProcedure("usp_Get_Desgnation", parameters, "IncidentCaseReview");


            txtMedID.Text = txtAssociateID.Text = txtCasenum.Text = txtDate.Text = txtEndDate.Text = string.Empty;
        }
    }

    protected void btnIMSResp_Click(object sender, EventArgs e)
    {
        int operation = 0;
        if (!string.IsNullOrEmpty(txtAssociateID.Text))
        {
            // delete document id
            operation = 4;
        }
        if (!string.IsNullOrEmpty(txtAssociateID.Text) && !string.IsNullOrEmpty(txtCasenum.Text))
        {
            // delete document id and open workflow
            operation = 5;            
        }
        if (operation > 0)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(txtMedID.Text))
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, Convert.ToInt32(txtMedID.Text), true));
            if (!string.IsNullOrEmpty(txtAssociateID.Text))
                parameters.Add(SqlParms.CreateParameter("DES_ID", DbType.Int32, Convert.ToInt32(txtAssociateID.Text), true));
            if (!string.IsNullOrEmpty(txtCasenum.Text))
                parameters.Add(SqlParms.CreateParameter("ST_ID", DbType.Int32, Convert.ToInt32(txtCasenum.Text), true));
            if (!string.IsNullOrEmpty(txtDate.Text))
                parameters.Add(SqlParms.CreateParameter("ST_DT", DbType.DateTime, Convert.ToDateTime(txtDate.Text), true));
            if (!string.IsNullOrEmpty(txtEndDate.Text))
                parameters.Add(SqlParms.CreateParameter("ED_DT", DbType.DateTime, Convert.ToDateTime(txtEndDate.Text), true));
            parameters.Add(SqlParms.CreateParameter("OP", DbType.Int32, operation, true));
            DataAccess.ExecuteStoredProcedure("usp_Get_Desgnation", parameters, "IncidentCaseReview");

            txtMedID.Text = txtAssociateID.Text = txtCasenum.Text = txtDate.Text = txtEndDate.Text = string.Empty;

        }

    }



    public string MakeClaimsServiceCall(XmlDocument xmlDoc, string url, string soapAction,
           string userName, string password)
        
    {
        string serviceResponse = string.Empty;
        try
        {

            ServicePointManager.Expect100Continue = true;
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            string wsCertificateName = AppSettings.Get("ClaimsWSCertificateName");

            CertUtil _certs = new CertUtil();
            X509Certificate2 clientCertificate = _certs.GetCertificateByName(wsCertificateName);
            string user = userName;
            string passcode = password;
            Uri apiUrl = new Uri(url);
            WebRequest pmRequest = HttpWebRequest.Create(url);
            HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
            pmHttpRequest.ContentType = "text/xml; charset=utf-8";

            pmHttpRequest.KeepAlive = true;
            pmHttpRequest.Method = "POST";
            pmHttpRequest.SendChunked = true;
            pmHttpRequest.UserAgent = ".NET Framework";
            pmHttpRequest.Host = AppSettings.Get("ClaimsWSHost");


            if (!System.Diagnostics.Debugger.IsAttached)
            {
                pmHttpRequest.ClientCertificates.Add(clientCertificate);
            }

            String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + passcode));
            pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
            pmHttpRequest.Headers.Add("SOAPAction", soapAction);
            Stream requestStream = pmHttpRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            var response = "";

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

            serviceResponse = response.ToString();


        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                var response = ex.Response as HttpWebResponse;
                if (response != null)
                {
                    //http status code avaliable
                    var respBody = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    serviceResponse = string.Format("Exception Response. Exception Message : {0}, Inner Exception : {1}, Response: {2}, Response: {3}", ex.Message, ex.InnerException, respBody.ToString(), response);
                }
                else
                {
                    // no http status code available
                    serviceResponse = string.Format("Response is null, unable to make connection. Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);
                }
            }
            else
            {
                // no http status code available
                serviceResponse = string.Format("Response is null, unable to make connection. Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException);

            }

        }
        catch (Exception ex)
        {
            serviceResponse = string.Format("Exception Message : {0}, Inner Exception : {1}, Stack Trace:{2}", ex.Message, ex.InnerException,ex.StackTrace.ToString());
        }
        return serviceResponse;
    }
}