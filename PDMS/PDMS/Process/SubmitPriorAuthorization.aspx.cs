using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Corp.Core.Libraries.PriorAuthServiceReference;
using System.Xml.Serialization;
using System.IO;
using Quartz.Impl.Triggers;


public partial class Process_SubmitPriorAuthorization : WorkflowPage
{
    private PDMSService.PDMSServiceClient _svc;
    private PDMSService.PDMSServiceClient svc
    {
        get
        {
            if (_svc == null)
            {
                _svc = new PDMSService.PDMSServiceClient();
            }

            return _svc;
        }
    }
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
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
    protected void Page_Load(object sender, EventArgs e)
    {

        CredentialHelper.APIToken APIToken = ApplicationCache.RestAPIAccessToken();
        HiddenField hdnAPIToken = (HiddenField)Page.Master.FindControl("hdnAccessToken");
        HiddenField hdnAPIRefreshToken = (HiddenField)Page.Master.FindControl("hdnRefreshToken");
        hdnAPIToken.Value = APIToken.AccessToken;
        hdnAPIRefreshToken.Value = APIToken.RefreshToken;


        if (!IsPostBack)
        {

            if (Session["RegId"] != null)
            {
                this.RegistrationId = Convert.ToInt32(Session["RegId"]);
            }
            string PA = default(string);
            string PayorId = default(string);
            string NPI = default(string);
            string MedicaidId = default(string);
            string TrackingNo = default(string);

            if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString.AllKeys.Contains("PA"))
                {
                    //PA = Convert.ToString(Request["PA"].ToString());
                    PA = Helper.Decrypt(HttpUtility.UrlDecode(Request.QueryString["PA"]));
                }
                if (Request.QueryString.AllKeys.Contains("PayorId"))
                {
                    PayorId = Helper.Decrypt(HttpUtility.UrlDecode(Request.QueryString["PayorId"]));
                    //PayorId = Convert.ToString(Request["PayorId"]);
                }
                if (Request.QueryString.AllKeys.Contains("NPI"))
                {
                    NPI = Helper.Decrypt(HttpUtility.UrlDecode(Request.QueryString["NPI"]));
                    //NPI = Convert.ToString(Request["NPI"]);
                }
                if (Request.QueryString.AllKeys.Contains("TrackingNo"))
                {
                    TrackingNo = Helper.Decrypt(HttpUtility.UrlDecode(Request.QueryString["TrackingNo"]));
                    //TrackingNo = Convert.ToString(Request["TrackingNo"]);
                }
                if (Request.QueryString.AllKeys.Contains("MedicaidId"))
                {
                    MedicaidId = Helper.Decrypt(HttpUtility.UrlDecode(Request.QueryString["MedicaidId"]));
                }
                if (Session["MedID"] != null)
                {
                    if (string.IsNullOrEmpty(MedicaidId) && !string.IsNullOrEmpty(Convert.ToString(Session["MedID"])))
                    {
                        MedicaidId = Convert.ToString(Session["MedID"]);
                    }
                }
            }
            this.FillRegistrationData();

            if (!string.IsNullOrEmpty(PA) && !string.IsNullOrEmpty(PayorId))
            {
                InquirePriorAuth(PA, PayorId, NPI, MedicaidId);
            }
            if (!string.IsNullOrEmpty(TrackingNo) && !string.IsNullOrEmpty(MedicaidId))
            {
                TrackingNumberSearchClick(MedicaidId, TrackingNo);
            }

            if (string.IsNullOrWhiteSpace(PA) && string.IsNullOrEmpty(TrackingNo) && !string.IsNullOrEmpty(MedicaidId))
            {
                this.WorkflowPage.MedicaidID = MedicaidId;
            }
        }

        if (!IsPostBack && Request.QueryString["payloadData"] == null)
        {
            string MedicaidNumber = this.WorkflowPage.MedicaidID;

            DeleteAttachmentsForProvider(MedicaidNumber);

            if (Request.Cookies["selectedOption"] != null)
            {
                Request.Cookies["selectedOption"].Expires = DateTime.Now.AddDays(-30);
                Request.Cookies["selectedOption"].Value = null;
            }
        }
    }

    public void InquirePriorAuth(string priorAuthNUmber, string payorId, string npi, string medicaidId)
    {
        string logHeader = string.Format("PriorAuthInquire Get Transaction History for Medicaid ID - ");
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);


        DataSet dsresponse = null;
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
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            int InquirePriorAuth = 5;
            int transactionID = InfoAccessController.InsertPASSTHROUGH_TRANSACTIONQUEUE(InquirePriorAuth, DateTime.Now, DateTime.Now, DateTime.Now, new Guid(MAXIMUS.Core.Libraries.Constants.appAdminUserId));

            MessageHeaderTypeSubscriber[] mhts = new MessageHeaderTypeSubscriber[1];
            mhts[0] = MessageHeaderTypeSubscriber.FI;

            MessageHeaderType mht = new MessageHeaderType();
            mht.BusinessFlow = MessageHeaderTypeBusinessFlow.InquirePriorAuth;
            mht.SubscriberSystem = mhts;
            mht.StateCode = "OH";
            mht.ModuleTransactionId = transactionID.ToString();
            mht.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
            mht.SITransactionKey = sitTransactionKey;
            mht.RequestorSystem = MessageHeaderTypeRequestorSystem.PNM;

            InquirePriorAuthRequestPayloadType inqReqPay = new InquirePriorAuthRequestPayloadType();
            inqReqPay.PayorType = payorId;
            inqReqPay.PriorAuthNumber = priorAuthNUmber;
            inqReqPay.ProviderId = npi;


            InquirePriorAuthRequest inq = new InquirePriorAuthRequest();
            inq.MessageHeader = mht;
            inq.RequestPayload = inqReqPay;

            string response = new PriorAuthServiceReqRes().priorAuthInquiryOperation(transactionID, inq);
            if (MAXIMUS.Core.Libraries.Constants.TransactionResult.TransactionFailed == response)
            {
                Response.Redirect(string.Format("~/Process/SearchPriorAuthorization.aspx?errorCode={0}",
                 MAXIMUS.Core.Libraries.Constants.ErrorCodes.TransactionFailed));
                return;
            }

            string res = InfoAccessController.GetResponsePayloadByPassThroughTransactionID(transactionID);
            if (string.IsNullOrEmpty(res))
            {
                Response.Redirect(string.Format("~/Process/SearchPriorAuthorization.aspx?errorCode={0}&MedicaidId={1}",
                MAXIMUS.Core.Libraries.Constants.ErrorCodes.MBResponseNull, medicaidId));
                //MessageBox2.Show("PA inquiry results are empty.", "Error");
                return;
            }
            var xDocResp = XDocument.Parse(res);
            var serializerResp = new XmlSerializer(typeof(EnvelopeInqResponse));
            EnvelopeInqResponse sam = (EnvelopeInqResponse)serializerResp.Deserialize(new StringReader(xDocResp.ToString()));

            if (sam.Body.InquirePriorAuthResponse != null)
                uc1SubmitPriorAuthorization.LoadProviderInformationByTransaction(sam.Body.InquirePriorAuthResponse);

            // SearchLinkClick(MAXIMUS.Core.Libraries.Constants.SectionTypeID.SubmitPriorAuthorization, sam.Body.InquirePriorAuthResponse);

        }
        catch (ThreadAbortException ex1)
        {
            Response.Redirect(string.Format("~/Process/SearchPriorAuthorization.aspx?errorCode={0}&MedicaidId={1}",
                MAXIMUS.Core.Libraries.Constants.ErrorCodes.NoRecords, medicaidId));
        }
        catch (Exception exx)
        {
            Response.Redirect(string.Format("~/Process/SearchPriorAuthorization.aspx?errorCode={0}&MedicaidId={1}",
                 MAXIMUS.Core.Libraries.Constants.ErrorCodes.ResponseNull, medicaidId));
        }
    }

    public void TrackingNumberSearchClick(string medicaidId, string trackingNo)
    {
        uc1SubmitPriorAuthorization.LoadClaimData(medicaidId, trackingNo);
    }

    private void DeleteAttachmentsForProvider(string medicaidNumber)
    {
        svc.DeleteAttachmentsForMedicaidID(medicaidNumber);
    }
    public string GetPreSignedUrl(string fileName, string contentType)
    {
        DateTime expiryTime = DateTime.Now.AddMinutes(20);
        ProcessAttachments processAttachments = new ProcessAttachments();
        return processAttachments.generatePreSignedUrl(fileName, contentType, expiryTime);
    }
}

