using Corp.Core.Libraries;
using Corp.Core.Libraries.CareManagement;
using Corp.Core.Libraries.PriorAuthServiceReference;
using DocumentFormat.OpenXml.EMMA;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using Microsoft.IdentityModel.Tokens;
using Models.Data;
using Newtonsoft.Json;
using StructureMap.Query;
//using Models.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Util;
using System.Xml;
using System.Xml.Serialization;
using CON = MAXIMUS.Core.Libraries.Constants;
using MessageHeader = Corp.Core.Libraries.CareManagement.MessageHeader;

public partial class PopupControls_SubmitPriorAuthorization : BaseSectionControl
{
    private const string SENDER_ID = "MMISODJFS";
    private const string ATTACHMENT_FILE_SUFFIX = "AT";

    private const string Malicious_Files = "The file upload is in process  - please remain on this page to see results.";
    private const string Processed_Files = "Below file(s) are queued to send to destination payer.";

    public IList<ProcessedDocument> UploadedFileNames = new List<ProcessedDocument>();

    private void Log(Exception ex)
    {


    }

    public string SaveCodeLNK
    {
        get
        {
            if (ViewState["SaveCodeLNK"] == null)
            {
                ViewState["SaveCodeLNK"] = Guid.NewGuid().ToString();
            }
            return ViewState["SaveCodeLNK"].ToString();
        }
        set
        {
            ViewState["SaveCodeLNK"] = value;
        }
    }

    private string MedicaidId = default(string);
    private string _DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
    private string _ValidFileExtensions = "doc,docx,pdf,xls,XLS,xlsm,xlsx,ppt,pptx,mdi,jpe,zip,txt,jpg,jpeg,png,gif,bmp,tif,tiff,pi,ec,zip,csv,xlsm,msg,acrbak";

    private const string INQUIRY_RESPONSE_RETENTION_DATA = "INQUIRY_RESPONSE_RETENTION_DATA";
    private const string ATTACHMENT_RETENTION_DATA = "ATTACHMENT_RETENTION_DATA";

    private string[] ReceipientMangCarePlanDesc = new string[] {"Amerihealth Medicaid MCE", "Anthem Medicaid MCE", "Buckeye Medicaid MCE",
                                        "CareSource Medicaid MCE", "Humana Medicaid MCE","Molina Medicaid MCE", "Paramount Medicaid MCE", "United Health Medicaid MCE"};
    private string[] destPayerIDArrayEDI = new string[] { "0021920", "0002937", "0021914", "0004202", "0003150", "0021919", "0007316", "0007610", "842435374", "842430000", "D002937", "V004202", "D004202", "T004202", "A004203", "B004203", "C004202", "CSVIS001", "61103", "D007316", "V007316", "T007316", "P007316", "N007316", "88337", "60054" };
    private string[] destPayerIDArrayFI = new string[] { "MMISODJFS" };

    private string alrt_4 = string.Empty;
    private string alrt_5 = string.Empty;
    private string alrt_6 = string.Empty;
    private string alrt_7 = string.Empty;

    public string AttachmentFileName
    {
        get
        {
            DateTime dateTime = DateTime.Now;
            return string.Format("{0}--{1}--{2}-", SENDER_ID, UUID, dateTime.ToString("yyyyMMdd"));
        }
    }

    public string AttachmentFileNameSuffix
    {
        get
        {
            return ATTACHMENT_FILE_SUFFIX;
        }
    }

    public string MedicaidNumber
    {
        get
        {
            return this.WorkflowPage.MedicaidID;
        }
    }

    private string UUID
    {
        set { ViewState["UUID"] = value; }
        get { return ViewState["UUID"].ToString(); }
    }

    public string PreSignedUrl
    {
        set { ViewState["PreSignedUrl"] = value; }
        get { return ViewState["PreSignedUrl"].ToString(); }
    }

    #region spa
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

    private int _priohospitalid = 0;
    private int PrioHospitalId
    {
        get
        {
            return _priohospitalid;
        }
        set
        {
            if (value != _priohospitalid)
            {
                _priohospitalid = value;
            }
        }
    }

    private string _ProviderNPI;
    private string ProviderNPI
    {
        get
        {
            return _ProviderNPI;
        }
        set
        {
            if (value != _ProviderNPI)
            {
                _ProviderNPI = value;
            }
        }
    }

    #endregion

    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();

        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadPASTATUS();
    }

    public void enableButtons()
    {
        string statusField;
        statusField = txtstatus2.Text;

        if (statusField.Equals("Submission Pending"))
        {
            btnSave.Visible = true;
            btnSubmit.Visible = true;
            btnCancelPARequest.Visible = false;
            btnCancel_Revert.Visible = false;
            pnlProviderNote.Enabled = true;
            txtProviderNotes.Enabled = true;
            //btnCancel.Visible = false;
            btnClearAll.Visible = true;
            btnDiagnosisAdd.Enabled = true;
            btnDiagnosisAdd.Visible = true;
            PAInfo.Attributes.Add("class", "col-sm-6 text-left");

            string methodName = "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString();
            CreateAndReturnLogInfoThreadNumber(methodName + "Add Diagnosis button has been enabled for 'Submission Pending' PA status.");
        }
        else if (statusField.Equals("Approved") || statusField.Equals("Partially Approved"))
        {
            btnReSubmit.Visible = false;
            btnSave.Visible = false;
            //btnCancel.Visible = true;
            btnCancelPARequest.Visible = false;
            btnUpdate.Visible = false;
            btnClearAll.Visible = true;
            btnSubmit.Visible = false;
            btnCopy.Visible = true;
            PAInfo.Attributes.Add("class", "col-sm-6 text-left PAApprovedInfoStyle");
            btnCancel_Revert.Visible = false;

            //gvServiceDetail.Enabled = false;
            //gvServiceDetailDental.Enabled = false;
            //gvServiceDetailProfessional.Enabled = false;
            btnDiagnosisAdd.Enabled = false;
            btnServiceDetailAdd1.Visible = false;
            ddlAuthorization.Enabled = false;
            ddlSubCapitaPayerIDs.Enabled = false;
            ddlAssignment.Enabled = false;
            ddlServiceType.Enabled = false;
            pnlProviderNote.Enabled = false;
            txtProviderNotes.Enabled = false;
            pnlDiagnosis.Enabled = false;
            //gvDiagnosis.Enabled = false;
        }
        else if (statusField.Equals("Denied"))
        {
            //gvDiagnosis.Enabled = false;
            btnDiagnosisAdd.Enabled = false;
            //gvServiceDetailDental.Enabled = false;
            btnDentalServiceDetailAdd1.Visible = false;

            btnReSubmit.Visible = true;
            //btnReSubmit.Enabled = false;
            //btnCancel.Visible = false;
            btnClearAll.Visible = true;
            btnCancel_Revert.Visible = false;
            //btnCancel_Revert.Enabled = false;
            //btnEdit.Visible = true;
            //btnEdit.Enabled = true;
            PAInfo.Attributes.Add("class", "col-sm-6 text-left PADeniedInfoStyle");
            btnUpdate.Visible = false;
            btnSave.Visible = true;
            btnCancelPARequest.Visible = false;
            btnSubmit.Visible = false;
            btnCopy.Visible = false;

        }
        else if (statusField.Equals("Pend") || statusField.Equals("InProcess"))
        {
            btnReSubmit.Visible = false;
            PAInfo.Attributes.Add("class", "col-sm-6 text-left");
            btnSubmit.Visible = false;
            btnServiceDetailAdd1.Visible = false;
            btnDentalServiceDetailAdd1.Visible = false;
            btnDiagnosisAdd.Enabled = false;
            //gvServiceDetailDental.Enabled = true;
            btnCancel_Revert.Visible = false;
            btnProfessionalServiceDetailAdd.Visible = false;
            btnCopy.Visible = false;
            //btnUpdate.Style.Add("display","none");
            //btnUpdate.Visible = false;
        }
        else if (statusField.Equals("Closed"))
        {
            btnReSubmit.Visible = false;
            btnSave.Visible = false;
            btnCancelPARequest.Visible = false;
            btnUpdate.Visible = false;
            btnClearAll.Visible = true;
            btnSubmit.Visible = false;
            btnCopy.Visible = false;
            btnCancel_Revert.Visible = false;
            btnCancelPARequest.Visible = false;

            btnDiagnosisAdd.Enabled = false;
            btnServiceDetailAdd1.Visible = false;
            btnDentalServiceDetailAdd1.Visible = false;
            ddlAuthorization.Enabled = false;
            ddlSubCapitaPayerIDs.Enabled = false;
            ddlAssignment.Enabled = false;
            ddlServiceType.Enabled = false;
            pnlProviderNote.Enabled = false;
            txtProviderNotes.Enabled = false;
            pnlDiagnosis.Enabled = false;
        }
        else if (statusField.Equals("Pending Addtl Info"))
        {
            btnReSubmit.Visible = false;
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            btnUpdate.Style.Add("display", "inline");
            btnUpdate.Style.Add("visibility", "visible");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //SaveCodeLNK = Guid.NewGuid().ToString();
            //LoadPageData();
            hdnUserName.Value = HttpContext.Current.User.Identity.Name;
            hdnProviderTypeId.Value = this.WorkflowPage.ProviderTypeID.ToString();
            if (!Page.IsPostBack)
            {
                LoadPageData();
                Session["SelectedPage"] = "SubmitClaimPA";
                UUID = Guid.NewGuid().ToString();
            }
            else
            {
                if (Request.Cookies["selectedOption"] != null && Request.Cookies["selectedOption"].Value != null && (Request.Cookies["selectedOption"].Value != "10002" && Request.Cookies["selectedOption"].Value != "10004")) return;
                LoadPageData();
                if ((Session["SelectedPage"] != null) && (Session["SelectedPage"] != ""))
                {
                    if (Session["SelectedPage"] == "SubmitClaimPA")
                    {
                        string eventTarget = Request["__EVENTTARGET"];
                        string eventArgument = Request["__EVENTARGUMENT"];
                        ProcessUploadAttachment(eventTarget, eventArgument);
                    }
                }
            }

            if (Session["TempDataClear"] != null)
            {
                GetDiagnoisServiceDetails();
                GetServiceDetails();
                GetDentalServiceDetails();
                GetProfessionalServiceDetails();
                Session.Remove("TempDataClear");
                txtCode.Text = string.Empty;
                txtPlaceOfServiceName.Text = string.Empty;
                //pnlProfessionalLine.Visible = false;
                pnlsepProfessionalLine.Visible = false;
                pnlsepline.Visible = false;
                //gvSubmitClaimSearchProcPop.DataSource = null;
                //gvSubmitClaimSearchProcPop.DataBind();
            }

            DisableFutureDates();
            var addAttachmentSubmitPAQS = Request.QueryString["SubmitPAAddAttachment"];

            HandleServerCallBack();

            if (!IsPostBack && addAttachmentSubmitPAQS != "SubmitPAAttachment")
            {
                txtCode.Text = string.Empty;
                txtPlaceOfServiceName.Text = string.Empty;
                pnlsepProfessionalLine.Visible = false;
                Session.Remove(ATTACHMENT_RETENTION_DATA);
                Session.Remove("ProviderNotes");
                //gvSubmitClaimSearchProcPop.Visible = false;
            }
            enableButtons();
            setValidateColor();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void ProcessUploadAttachment(string target, string argument)
    {
        if (target == "btnPAPriorAdd" && argument == "addAttachment")
        {
            if (rblClaimType.SelectedValue == "dental")
                uploadDentalAttachment();
            else uploadAttachment();

            //Generate an unique Guid for every upload document in Attachment Panel.
            UUID = Guid.NewGuid().ToString();
        }
    }
    public class PayloadData
    {
        public string FileName;
        public string ContentType;
    }
    public void HandleServerCallBack()
    {
        try
        {
            if (!string.IsNullOrEmpty(Request.QueryString["payloadData"]))
            {

                string payloadData = Request.QueryString["payloadData"].Replace(" ", "").Replace("(", "").Replace(")", "");
                PayloadData data = JsonConvert.DeserializeObject<PayloadData>(payloadData);
                DateTime expiryTime = DateTime.Now.AddMinutes(2);
                ProcessAttachments processAttachments = new ProcessAttachments();
                var preSignedUrl = processAttachments.generatePreSignedUrl(data.FileName, data.ContentType, expiryTime);
                string responseJson = "{preSignedUrl: '" + preSignedUrl + "'}";
                // System.Threading.Thread.Sleep(1000);
                //Wrap a call to CallBack function with the JSON string as parameter.
                responseJson = string.Format("{0}({1});", Request.QueryString["callback"], responseJson);
                //Send the Response in JSON format to Client.            
                Response.ContentType = "text/json";
                Response.Write(responseJson);
                Response.End();
            }
        }
        catch (Exception ex)
        { }
    }

    public void LoadPageData()
    {
        try
        {
            string PA = default(string);
            string TrackingNo = default(string);

            if (Request.QueryString.AllKeys.Contains("PA"))
                PA = Helper.Decrypt(HttpUtility.UrlDecode(Request.QueryString["PA"]));

            if (Request.QueryString.AllKeys.Contains("TrackingNo"))
                TrackingNo = Helper.Decrypt(HttpUtility.UrlDecode(Request.QueryString["TrackingNo"]));

            if (rblClaimType.SelectedIndex == -1)
            {
                divSubmitPriorAuth.Visible = false;
            }

            //Added this because Diagnosis Panel is only required for Institutional PA type.

            if (rblClaimType.SelectedValue == "dental")
            {
                diMandatoryInst.Visible = false;
            }
            else
                diMandatoryInst.Visible = true;

            BindingNPILabelsFromHiddenFields();

            string MedicaidNumber = this.WorkflowPage.MedicaidID;
            MedicaidId = hdnMedID.Value = MedicaidNumber;
            //txtMedicaidID.Text = MedicaidId;
            LoadProviderInformation(MedicaidNumber);
            int transactionId;
            if (Int32.TryParse(Request.QueryString["TransactionId"], out transactionId))
                LoadProviderInformationByTransaction(new InquirePriorAuthResponse());
            this.ucPriorAuthProvidersNotes.SaveEvent += new PopupControls_PriorAuthProvidersNotes.SaveEventHandler(UpdateNoteData);
            this.ucPriorAuthProvidersNotes.CancelEvent += new PopupControls_PriorAuthProvidersNotes.CancelEventHandler(CancelPopup);
            this.ucPriorAuthServiceDetails.SaveEvent += new PopupControls_PriorAuthServiceDetails.SaveEventHandler(UpdateNoteData);
            this.ucPriorAuthServiceDetails.CancelEvent += new PopupControls_PriorAuthServiceDetails.CancelEventHandler(CancelPopup);
            this.ucPriorAuthDentalServiceDetails.SaveEvent += new PopupControls_PriorAuthDentalServiceDetails.SaveEventHandler(UpdateNoteData);
            this.ucPriorAuthDentalServiceDetails.CancelEvent += new PopupControls_PriorAuthDentalServiceDetails.CancelEventHandler(CancelPopup);
            this.ucPriorAuthDiagnosis.SaveEvent += new PopupControls_PriorAuthDiagnosis.SaveEventHandler(UpdateNoteData);
            this.ucPriorAuthDiagnosis.CancelEvent += new PopupControls_PriorAuthDiagnosis.CancelEventHandler(CancelPopup);
            this.ucPriorAuthAttachment.SaveEvent += new PopupControls_PriorAuthAttachment.SaveEventHandler(UpdateNoteData);
            this.ucPriorAuthAttachment.CancelEvent += new PopupControls_PriorAuthAttachment.CancelEventHandler(CancelPopup);
            //this.CancelEvent += new PopupControls_SubmitPriorAuthorization.CancelEventHandler(ClearAll);
            //cvBirthdate.ValueToCompare = DateTime.Now.ToShortDateString();
            //cvtextraction.ValueToCompare = DateTime.Now.ToShortDateString();
            cvtxtAccidentDate.ValueToCompare = DateTime.Now.ToShortDateString();
            txtorderingprovidernpi.Font.Size = FontUnit.XXSmall;


            btnReSubmit.Visible = false;
            btnCopy.Visible = false;
            btnSave.Visible = false;
            //btnCancel.Visible = false;
            btnClearAll.Visible = true;
            pnlSepDocumentbyMail.Visible = false;
            pnlDocumentbyMail.Visible = false;
            pnlTrackingNumber.Visible = false;
            pnlSepTrackingNumber.Visible = false;

            LoadDestinationPayerIDs(ddlAuthorization.SelectedValue);

            txtMedicaidID.Enabled = false;
            txtOMID.Enabled = false;

            if (!IsPostBack)
            {
                valerrormess.Visible = false;

                txtPriorAttachmentNote.Attributes.Add("maxlength", txtPriorAttachmentNote.MaxLength.ToString());
                txtProviderNotes.Attributes.Add("maxlength", txtProviderNotes.MaxLength.ToString());
                txtPriorDentalAttachmentNote.Attributes.Add("maxlength", txtPriorDentalAttachmentNote.MaxLength.ToString());
                txtDentalProcCodeDescription.Attributes.Add("maxlength", txtDentalProcCodeDescription.MaxLength.ToString());
                txtDentalProvServnote.Attributes.Add("maxlength", txtDentalProvServnote.MaxLength.ToString());
                txtLnprocCodeDesc.Attributes.Add("maxlength", txtLnprocCodeDesc.MaxLength.ToString());
                txtLnProviderServiceNote.Attributes.Add("maxlength", txtLnProviderServiceNote.MaxLength.ToString());
                txtProfessionalProcCodeDescription.Attributes.Add("maxlength", txtProfessionalProcCodeDescription.MaxLength.ToString());
                txtProfessionalProvServnote.Attributes.Add("maxlength", txtProfessionalProvServnote.MaxLength.ToString());
                txtProfessionalServTrackingNo.Attributes.Add("maxlength", txtProfessionalServTrackingNo.MaxLength.ToString());
                txtDentalServTrackingNo.Attributes.Add("maxlength", txtDentalServTrackingNo.MaxLength.ToString());
                txtLnDiagnosisCode.Attributes.Add("maxlength", txtLnDiagnosisCode.MaxLength.ToString());
                txtFacilityTypeDescription.Attributes.Add("maxlength", txtFacilityTypeDescription.MaxLength.ToString());
                txtPlaceOfServiceDesc.Attributes.Add("maxlength", txtPlaceOfServiceDesc.MaxLength.ToString());
                txtDiagnosisCodeDescSearch1.Attributes.Add("maxlength", txtDiagnosisCodeDescSearch1.MaxLength.ToString());
                txtServDetRevCodeDesc.Attributes.Add("maxlength", txtServDetRevCodeDesc.MaxLength.ToString());
                txtHCPCSCode.Attributes.Add("maxlength", txtHCPCSCode.MaxLength.ToString());
                txtCode.Attributes.Add("maxlength", txtCode.MaxLength.ToString());
                txtPlaceOfServiceName.Attributes.Add("maxlength", txtPlaceOfServiceName.MaxLength.ToString());
                txtDentalReqUnits.Attributes.Add("maxlength", txtDentalReqUnits.MaxLength.ToString());
                txtProfessionalReqUnits.Attributes.Add("maxlength", txtProfessionalReqUnits.MaxLength.ToString());
                txtLnServiceTrackingNo.Attributes.Add("maxlength", txtLnServiceTrackingNo.MaxLength.ToString());
                txtNPI.Attributes.Add("maxlength", "10");
                txtProMedicaidID.Attributes.Add("maxlength", "12");
                hdnModified.Value = "true";

                // pnlline.Visible = false;
                pnlsepline.Visible = false;
                //  pnlDentalLine.Visible = false;
                pnlsepDentalLine.Visible = false;

                GetServiceCodeTypeCodeForPriorAuth();
                EnablePanelsBasedOnPriorAuth();

                GetServiceTypeCode();

                if (string.IsNullOrEmpty(PA) && string.IsNullOrEmpty(TrackingNo))
                {
                    GetDestinationPayer();
                    GetServiceCodeType();
                    GetInstitutionalAssignments();
                    GetDischargeStatus();
                }
                GetLvelOfCare();
                GetRequestedUnitMeasures();
                GetServiceDetails();
                GetPriorAttachmentDocumentType();
                GetPricingFormula();
                BindAttachmentGrid();
                GetPlanName();
                GetSpecialIndicator();
                GetManagedcareplan();
                GetPriorPlacement();
                GetPriorNewPlacement();

                //Get Diagnosis details for the Dropdown list
                GetDiagnosisCodeType();
                GetPriorDocumentType();

                //Get Diagnosis details for the grid 
                GetDiagnoisServiceDetails();
                GetServiceDetails();
                GetDentalServiceDetails();
                GetORDERPROVIDERINFOSearch();
                GetServicingProviderInfo();
                GetCertServiceDetails();
                GetOutcomeofreview();
                GetResonDenialNotes();
                GetDocumentbyMail();

                BindDentalAttachments();
                BindDentalServiceDetailsDropdowns();
                GetPriorAttachmentDocumentTypeForDental();
                GetProfessionalServiceDetails();

                hdnFrontEndEdits.Value = "";
                hdnFrontEndEdits_6.Value = "";
                hdnFrontEndEdits_7.Value = "";
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    public void DisableFutureDates()
    {
        ceAccDtService.EndDate = DateTime.Now;
        ceTextBox3.EndDate = DateTime.Now;
        cetxtMenDtInst.EndDate = DateTime.Now;
        cetxtProfOnsetIllness.EndDate = DateTime.Now;
        cetxtLstMensPeriod.EndDate = DateTime.Now;
        cetxtOnsetIllness.EndDate = DateTime.Now;
        cetxtAccidentDate.EndDate = DateTime.Now;
        ceDiagnosisDate.EndDate = DateTime.Now;
        ceBirthdate.EndDate = DateTime.Now;
    }

    public void LoadProviderInformationByTransaction(InquirePriorAuthResponse InquirePriorAuthResponse)
    {
        Session[INQUIRY_RESPONSE_RETENTION_DATA] = InquirePriorAuthResponse;

        if (InquirePriorAuthResponse == null || InquirePriorAuthResponse.ResponsePayload == null || InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278 == null || InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter == null)
            return;

        hdnPAInquiryTrn.Value = "PAInquiry";

        /*int totalUnits = InquirePriorAuthResponse.ResponsePayload.TotalUnits;
        int UsedUnits = InquirePriorAuthResponse.ResponsePayload.UsedUnits;
        int remainingUnits = 0;
        if (totalUnits >= 0 && UsedUnits >= 0)
        {
            remainingUnits = (totalUnits - UsedUnits);
        }*/
        string claimType = "";
        #region Header Initialization
        try
        {
            foreach (var item in InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails)
            {
                var denSrvDts = item.DentalService_2000F;
                var profSrvDts = item.ProfessionalService_2000F;
                var insSrvDts = item.InstitutionalServiceLine_2000F;

                if (denSrvDts != null)
                {
                    rblClaimType.SelectedValue = "dental";
                }
                if (profSrvDts != null)
                {
                    rblClaimType.SelectedValue = "Professional";
                }
                if (insSrvDts != null)
                {
                    rblClaimType.SelectedValue = "Institutional";
                }
            }

            //This check is purely for institutional PA types
            if (rblClaimType.SelectedItem != null && string.IsNullOrEmpty(rblClaimType.SelectedItem.Text) && InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E != null && InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.InstitutionalClaimCode_2000E != null)
            {
                rblClaimType.SelectedValue = "Institutional";
            }

            this.rblClaimType_SelectedIndexChanged(rblClaimType, new EventArgs());

            if (InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E != null &&
    InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.MessageText_2000E != null &&
    InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.MessageText_2000E.MSG01_FreeFormMessageText != null)
            {

                string assignmentType = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.MessageText_2000E.MSG01_FreeFormMessageText;
                assignmentType = assignmentType.Substring(0, 2);

                if (!string.IsNullOrEmpty(assignmentType) && ddlAssignment.Items.FindByValue(assignmentType) != null)// && ddlAssignment.Items.FindByText(assignmentType.Substring(0, 2)) != null)
                {
                    // ("Dental");
                    //ddlAssignment.Items.FindByText(assignmentType).Selected = true;
                    ddlAssignment.SelectedValue = assignmentType;

                    if (_spa == null)
                    {
                        _spa = new PDMSService.PDMSServiceClient();
                    }

                    try
                    {
                        DataSet dataSetAssignments = _spa.GetAssignementByAuth(string.Empty);
                        if (dataSetAssignments != null &&
                            dataSetAssignments.Tables.Count > 0 &&
                            (rblClaimType.SelectedItem == null))
                        {
                            DataRow drAssignment = dataSetAssignments.Tables[0].AsEnumerable().FirstOrDefault(a => a.Field<string>("PRIOR_AUTH_Assignment_Type_MMIS") == assignmentType);
                            if (drAssignment != null)
                            {
                                if (drAssignment["PRIOR_AUTH_Assignment_Type_ServiceCategory"] != DBNull.Value)
                                {
                                    string result = drAssignment["PRIOR_AUTH_Assignment_Type_ServiceCategory"].ToString();
                                    if (!string.IsNullOrEmpty(result) && (result.ToLower() == "dental" || result.ToLower() == "professional" || result.ToLower() == "institutional"))
                                    {
                                        rblClaimType.SelectedValue = result;
                                        this.rblClaimType_SelectedIndexChanged(rblClaimType, new EventArgs());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CreateAndReturnLogThreadNumber(ex, "SubmitPA-LoadServiceDetailsFromInquireResponse-LoadAssignments");
                    }
                }
            }

            string Providermedicaidnumber = string.Empty;

            if (InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberName_2010C != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberName_2010C.NM109_UMOIdentifier != null)
            {
                Providermedicaidnumber = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberName_2010C.NM109_UMOIdentifier;
            }

            string providerNPI = string.Empty; ;
            if (InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterName_2010B.NM109_UMOIdentifier != null)
            {
                providerNPI = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterName_2010B.NM109_UMOIdentifier;

            }

            string providerName = string.Empty;

            if (InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterName_2010B != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterName_2010B.NM104_UMOFirstName != null)
            {
                providerName = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterName_2010B.NM104_UMOFirstName;
            }

            if (InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.UMODetails_2000A != null &&
               InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A != null &&
               InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A.UMOName_2010A != null)
            {
                if (InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A.UMOName_2010A.NM103_UMOLastOrOrganizationName != null)
                {
                    string destinationPayer = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A.UMOName_2010A.NM103_UMOLastOrOrganizationName;

                    if (!string.IsNullOrEmpty(destinationPayer))
                    {
                        GetDestinationPayer();
                        ddlAuthorization.ClearSelection();
                        foreach (ListItem item in ddlAuthorization.Items)
                        {
                            if (item.Text.ToLower() == destinationPayer.ToLower())
                            {
                                ddlAuthorization.SelectedValue = item.Value.ToString();
                                break;
                            }
                        }
                    }
                }

                if (InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A.UMOName_2010A.NM109_UMOIdentifier != null)
                {
                    string destinationPayerId = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A.UMOName_2010A.NM109_UMOIdentifier;

                    if (!string.IsNullOrEmpty(destinationPayerId))
                    {
                        LoadDestinationPayerIDsByMCE_ID(destinationPayerId);
                    }
                }
            }

            #region Service Details
            try
            {
                LoadServiceDetailsFromInquireResponse(InquirePriorAuthResponse.ResponsePayload.PriorAuthRequest278, InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278);
            }
            catch (Exception ex)
            {
                CreateAndReturnLogThreadNumber(ex, "SubmitPA-LoadServiceDetailsFromInquireResponse");
            }
            #endregion

            if (InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails.Count() > 0 &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails[0] != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails[0].DentalService_2000F != null)
            {

                var denSrvDts = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails[0].DentalService_2000F;
                if (denSrvDts != null)
                {
                    GetDentalAssignments();
                    claimType = "1";
                }
            }

            if (InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails.Count() > 0 &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails[0] != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails[0].ProfessionalService_2000F != null)
            {
                var profSrvDts = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails[0].ProfessionalService_2000F;
                if (profSrvDts != null)
                {
                    GetProfessionalAssignments();
                    claimType = "2";
                }
            }

            if (InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails.Count() > 0 &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails[0] != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails[0].InstitutionalServiceLine_2000F != null)
            {
                var insSrvDts = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails[0].InstitutionalServiceLine_2000F;
                if (insSrvDts != null)
                {
                    GetInstitutionalAssignments();
                    claimType = "3";
                }
            }



            if (InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E != null &&
                InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM03_ServiceRespTypeCode != null)
            {
                string serviceType = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM03_ServiceRespTypeCode;
                if (!string.IsNullOrEmpty(serviceType))
                    serviceType = serviceType.TrimStart('0');

                if (!string.IsNullOrEmpty(serviceType))
                {
                    GetServiceCodeType();
                    ddlServiceType.ClearSelection();

                    if (_spa == null)
                    {
                        _spa = new PDMSService.PDMSServiceClient();
                    }
                    DataSet dataSet = _spa.GetServiceCodeType();
                    DataTable dt = dataSet.Tables[0];

                    if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            string serviceTypeCode = item["PRIOR_AUTH_SERVICE_TYPE_CODE"] != DBNull.Value ? Convert.ToString(item["PRIOR_AUTH_SERVICE_TYPE_CODE"]) : string.Empty;
                            if (!string.IsNullOrEmpty(serviceTypeCode))
                                serviceTypeCode = serviceTypeCode.TrimStart('0');
                            string serviceTypeCodeId = item["PRIOR_AUTH_SERVICE_TYPE_ID"] != DBNull.Value ? Convert.ToString(item["PRIOR_AUTH_SERVICE_TYPE_ID"]) : string.Empty;
                            if (!string.IsNullOrEmpty(serviceTypeCodeId) && !string.IsNullOrEmpty(serviceTypeCode) && serviceTypeCode.ToLower() == serviceType.ToLower())
                            {
                                ddlServiceType.SelectedValue = serviceTypeCodeId.ToString();
                                break;
                            }
                        }
                    }
                    //ddlServiceType.SelectedValue = serviceType;
                }
                else
                {
                    CreateAndReturnLogThreadNumber(new Exception(), "SubmitPA-LoadProviderInformationByTranscation");
                }
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-LoadProviderInformationByTranscation");
        }
        #endregion

        #region PA Information        
        try
        {
            LoadPAInformationFromInquireResponse(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278);
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-LoadPAInformationFromInquireResponse");

        }
        #endregion

        #region Load PA Status
        try
        {
            LoadPAStatusInfoFromInquireResponse(InquirePriorAuthResponse);
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-LoadPAStatusInfoFromInquireResponse");
        }
        #endregion        

        #region Recipient Details
        try
        {
            LoadRecipientInformationFromInquireResponse(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278);
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-LoadRecipientInformationFromInquireResponse");
        }
        #endregion

        #region Contact Information
        try
        {
            LoadContactInformationFromInquireResponse(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278);
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-LoadContactInformationFromInquireResponse");
        }
        #endregion

        #region Service Information for dental/institutional/profesional
        try
        {
            LoadServiceInformationFromInquireResponse(InquirePriorAuthResponse.ResponsePayload.PriorAuthRequest278, InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278);
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-LoadServiceInformationFromInquireResponse");
        }
        #endregion

        #region Servicing Provider Information | Ordering Provider Information
        ProviderDetails_2010EARespType[] providerObj;
        string recMedicaidNo;
        try
        {
            LoadServicingProviderInformationFromInquireResponse(InquirePriorAuthResponse, out providerObj, out recMedicaidNo);

            GetOrderingProviderInformationFromInquireResponse(providerObj, recMedicaidNo);
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-LoadServicingProviderInformationFromInquireResponse");
        }
        #endregion

        #region Diagnosis Information
        try
        {
            GetDignosisInformationFromInquireResponse(InquirePriorAuthResponse);
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetDignosisInformationFromInquireResponse");
        }
        #endregion

        #region Provider Notes
        try
        {
            GetProviderNotesFromInquireResponse(InquirePriorAuthResponse);
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetProviderNotesFromInquireResponse");
        }
        #endregion

        #region Attachments
        //LoadAttachmentsFromInquireResponse(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278);
        #endregion

        #region Outcome Of Review
        try
        {
            LoadOutcomeOfReviewFromInquireResponse(InquirePriorAuthResponse);
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-LoadOutcomeOfReviewFromInquireResponse");
        }
        #endregion

        #region reviewer notes
        try
        {
            var providerNotes = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.MessageText_2000E;
            DataTable dtrn = new DataTable();
            dtrn.Columns.Add("num_dtl");
            dtrn.Columns.Add("dsc_note");
            reviewerNotesTbl.Rows.Clear();

            //Load Provider Note as "00"
            if (providerNotes != null && !string.IsNullOrEmpty(providerNotes.MSG01_FreeFormMessageText))
            {
                dtrn.Rows.Add("00", providerNotes.MSG01_FreeFormMessageText.Substring(2));
            }
            //Load Reviewer notes based on service details
            var serviceDetails = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails;
            if (serviceDetails != null && serviceDetails.Count() > 0)
            {
                for (int i = 0; i < serviceDetails.Count(); i++)
                {
                    var msg = serviceDetails[i].MessageText_2000F;
                    if (msg != null && !string.IsNullOrEmpty(msg.MSG01_FreeFormMessageText))
                    {
                        dtrn.Rows.Add((i + 1).ToString().PadLeft(2, '0'), msg.MSG01_FreeFormMessageText);
                    }
                }
            }

            if (dtrn.Rows != null && dtrn.Rows.Count > 0)
            {
                foreach (DataRow revNoteRow in dtrn.Rows)
                {
                    HtmlTableRow htmlTableRow = new HtmlTableRow();
                    HtmlTableCell numberCell = new HtmlTableCell();
                    numberCell.InnerText = revNoteRow["num_dtl"].ToString();
                    numberCell.Attributes.Add("style", "color:black; width:150px;");

                    htmlTableRow.Controls.Add(numberCell);
                    HtmlTableCell msgCell = new HtmlTableCell();
                    msgCell.InnerText = revNoteRow["dsc_note"].ToString();
                    htmlTableRow.Controls.Add(msgCell);
                    reviewerNotesTbl.Rows.Add(htmlTableRow);
                }
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-ReviewerNotes");
        }
        #endregion

        ValidateData();

        //Set Malicious docs
        SetClaimsMaliciousDocs(txtMedicaidBillingNumber.Text, claimType, this.WorkflowPage.MedicaidID);
    }

    private void LoadPAStatusInfoFromInquireResponse(InquirePriorAuthResponse InquirePriorAuthResponse)
    {
        string status = txtstatus2.Text;

        if (!string.IsNullOrEmpty(status))
        {
            if (status == "Pend" || status == "InProcess")
            {
                pnlRecipient.Enabled = false;
                pnlContact.Enabled = false;
                pnlServiceInformation.Enabled = false;
                pnlTrackingNumber.Enabled = false;
                pnlService.Enabled = false;
                pnlServiceProviderInfo.Enabled = false;
                pnlorderproviderinfo.Enabled = false;
                pnlDiagnosisLine.Enabled = false;
                pnlCertHospital.Enabled = false;
                pnlProviderNote.Enabled = false;
                pnlProvidermainPnl.Enabled = false;
                pnlOutcomeOfReview.Enabled = false;
                pnlAttachment.Enabled = false;
                PriorAttachmentUpload.Enabled = false;
                txtAttachmentName.Enabled = false;
                //spnShow.Visible = true;
                pnlDentalAttachment.Enabled = false;
                pnlmissingtooth.Enabled = false;
                pnlDocumentbyMail.Enabled = false;
                pnlreviewernoteprovider.Enabled = false;
                pnlReasonforDenial.Enabled = false;
                Panel1.Enabled = false;
                btnSave.Visible = false;
                btnCancelPARequest.Visible = true;
                //btnCancel.Visible = true;
                btnUpdate.Visible = true;
                btnUpdate.CssClass = "buttonBoxFocus";
                btnUpdate.Style.Add("display", "none");
                //btnUpdate.Style.Add("display", "none");

                btnClearAll.Visible = true;
                ddlSubCapitaPayerIDs.Enabled = false;
                ddlAuthorization.Enabled = false;
                ddlAssignment.Enabled = false;
                ddlServiceType.Enabled = false;
            }
            if (status == "Submission Pending")
            {
                btnSave.Visible = true;
                btnSubmit.Visible = true;
                //btnCancel.Visible = true;
            }
            else if (status == "Approved" || status == "Partially Approved")
            {
                btnCopy.Visible = true;
                PAInfo.Attributes.Remove("class");
                PAInfo.Attributes.Add("class", "col-sm-6 text-left PAInfoStyle");
                pnlRecipient.Enabled = false;
                pnlContact.Enabled = false;
                pnlServiceInformation.Enabled = false;
                pnlTrackingNumber.Enabled = false;
                pnlService.Enabled = false;
                pnlServiceProviderInfo.Enabled = false;
                pnlorderproviderinfo.Enabled = false;
                pnlDiagnosisLine.Enabled = false;
                pnlCertHospital.Enabled = false;
                pnlProviderNote.Enabled = false;
                pnlProvidermainPnl.Enabled = false;
                pnlOutcomeOfReview.Enabled = false;
                pnlAttachment.Enabled = false;
                pnlDentalAttachment.Enabled = false;
                pnlmissingtooth.Enabled = false;
                pnlDocumentbyMail.Enabled = false;
                pnlreviewernoteprovider.Enabled = false;
                pnlReasonforDenial.Enabled = false;
                Panel1.Enabled = false;
                btnSave.Visible = false;
                btnSubmit.Visible = false;
                //btnCancel.Visible = false;
                btnClearAll.Visible = true;
            }
            else if (status == "Denied")
            {
                pnlRecipient.Enabled = false;
                pnlContact.Enabled = false;
                pnlServiceInformation.Enabled = false;
                pnlTrackingNumber.Enabled = false;
                pnlService.Enabled = false;
                pnlServiceProviderInfo.Enabled = false;
                pnlorderproviderinfo.Enabled = false;
                pnlDiagnosisLine.Enabled = false;
                pnlCertHospital.Enabled = false;
                pnlProviderNote.Enabled = false;
                pnlProvidermainPnl.Enabled = false;
                pnlOutcomeOfReview.Enabled = false;
                pnlAttachment.Enabled = false;
                pnlDentalAttachment.Enabled = false;
                pnlmissingtooth.Enabled = false;
                pnlDocumentbyMail.Enabled = false;
                pnlreviewernoteprovider.Enabled = false;
                pnlReasonforDenial.Enabled = false;
                Panel1.Enabled = false;
                btnSave.Visible = true;
                btnSubmit.Visible = false;
                btnCancelPARequest.Visible = false;
                //btnCancel.Visible = false;
                btnClearAll.Visible = true;
                //btnCancel_Revert.Enabled = false;
                btnReSubmit.Visible = true;
                //btnReSubmit.Enabled = false;
                //btnEdit.Visible = true;
                //btnEdit.Enabled = true;
                PAInfo.Attributes.Remove("class");
                PAInfo.Attributes.Add("class", "col-sm-6 text-left PAInfoDenied");
            }
            else if (status == "Closed")
            {
                btnCopy.Visible = false;
                PAInfo.Attributes.Remove("class");
                PAInfo.Attributes.Add("class", "col-sm-6 text-left PAInfoStyle");
                pnlRecipient.Enabled = false;
                pnlContact.Enabled = false;
                pnlServiceInformation.Enabled = false;
                pnlTrackingNumber.Enabled = false;
                pnlService.Enabled = false;
                pnlServiceProviderInfo.Enabled = false;
                pnlorderproviderinfo.Enabled = false;
                pnlDiagnosisLine.Enabled = false;
                pnlCertHospital.Enabled = false;
                pnlProviderNote.Enabled = false;
                pnlProvidermainPnl.Enabled = false;
                pnlOutcomeOfReview.Enabled = false;
                pnlAttachment.Enabled = false;
                pnlDentalAttachment.Enabled = false;
                pnlmissingtooth.Enabled = false;
                pnlDocumentbyMail.Enabled = false;
                pnlreviewernoteprovider.Enabled = false;
                pnlReasonforDenial.Enabled = false;
                Panel1.Enabled = false;
                btnSave.Visible = false;
                btnSubmit.Visible = false;
                btnClearAll.Visible = true;
                //btnCancel.Visible = true;
            }
            else if (status == "Pending Addtl Info")
            {
                btnReSubmit.Visible = false;
                btnSubmit.Visible = false;
                btnUpdate.Visible = true;
                btnUpdate.Style.Add("display", "inline");
                btnUpdate.Style.Add("visibility", "visible");
            }
        }
    }


    private void LoadServicingProviderInformationFromInquireResponse(InquirePriorAuthResponse InquirePriorAuthResponse, out ProviderDetails_2010EARespType[] providerObj, out string recMedicaidNo)
    {
        try
        {
            providerObj = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.ProviderDetails_2010EA;
            if (providerObj != null && providerObj.Count() > 0)
            {
                var providerSrvInfLst = providerObj.FirstOrDefault(m => m.PatientEventProviderName_2010EA.NM101_EntityIdentifierCode == "SJ");

                if (providerSrvInfLst != null && providerSrvInfLst.PatientEventProviderName_2010EA != null)
                {
                    var providerSrvInf = providerSrvInfLst.PatientEventProviderName_2010EA;
                    txtSPNPI.Text = providerSrvInf.NM109_UMOIdentifier;
                    lblSvcProviderFName.Text = providerSrvInf.NM104_UMOFirstName;
                    lblSvcProviderLName.Text = providerSrvInf.NM103_UMOLastOrOrganizationName;

                    string methodName = "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString();
                    CreateAndReturnLogInfoThreadNumber(methodName + "Logging values from response");
                    CreateAndReturnLogInfoThreadNumber(methodName + " - NPI (providerSrvInf.NM109_UMOIdentifier) = " + providerSrvInf.NM109_UMOIdentifier);
                    CreateAndReturnLogInfoThreadNumber(methodName + " - Provider First Name (providerSrvInf.NM104_UMOFirstName) = " + providerSrvInf.NM104_UMOFirstName);
                    CreateAndReturnLogInfoThreadNumber(methodName + " - Provider Last Name (providerSrvInf.NM103_UMOLastOrOrganizationName) = " + providerSrvInf.NM103_UMOLastOrOrganizationName);
                }
                else
                {
                    Exception ex = new Exception("Inquiry PA response for the section PatientEventDetails_2000E.NM101_EntityIdentifierCode with value as SJ is NULL/Empty.");
                    CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
                }
            }
            else
            {
                Exception ex = new Exception("Inquiry PA response for the section PatientEventDetails_2000E.ProviderDetails_2010EA is NULL/Empty.");
                CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            }

            string medicaidNumber = this.WorkflowPage.MedicaidID;
            recMedicaidNo = medicaidNumber;// InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberName_2010C.NM109_UMOIdentifier;
            txtMedicaidID.Text = medicaidNumber;
        }
        catch (Exception ex)
        {
            throw new Exception("Error at method LoadServicingProviderInformationFromInquireResponse", ex);
        }
    }

    private void GetOrderingProviderInformationFromInquireResponse(ProviderDetails_2010EARespType[] providerObj, string recMedicaidNo)
    {
        //var providerObj = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.ProviderDetails_2010EA;
        try
        {
            if (providerObj != null && providerObj.Count() > 0)
            {
                var providerInformationLst = providerObj.FirstOrDefault(m => m.PatientEventProviderName_2010EA.NM101_EntityIdentifierCode == "DK");
                if (providerInformationLst != null && providerInformationLst.PatientEventProviderName_2010EA != null)
                {
                    var providerInformation = providerInformationLst.PatientEventProviderName_2010EA;
                    txtorderingprovidernpi.Text = providerInformation.NM109_UMOIdentifier;
                    txtOMID.Text = recMedicaidNo;
                    hdtxtOMID.Value = recMedicaidNo;
                    hdtxtOMID1.Value = recMedicaidNo;
                    lblOrdProviderFName.Text = providerInformation.NM104_UMOFirstName;
                    lblOrdProviderLName.Text = providerInformation.NM103_UMOLastOrOrganizationName;

                    string methodName = "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString();
                    CreateAndReturnLogInfoThreadNumber(methodName + "Logging values from response");
                    CreateAndReturnLogInfoThreadNumber(methodName + " - Ordering provider NPI (providerInformationLst.PatientEventProviderName_2010EA) = " + providerInformation.NM109_UMOIdentifier);
                    CreateAndReturnLogInfoThreadNumber(methodName + " - Ordering Provider First Name (providerInformation.NM104_UMOFirstName) = " + providerInformation.NM104_UMOFirstName);
                    CreateAndReturnLogInfoThreadNumber(methodName + " - Ordering Provider Last Name (providerInformation.NM103_UMOLastOrOrganizationName) = " + providerInformation.NM103_UMOLastOrOrganizationName);
                }
                else
                {
                    Exception ex = new Exception("Inquiry PA response for the section PatientEventDetails_2000E.NM101_EntityIdentifierCode with value as DK is NULL/Empty.");
                    CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
                }
            }
            else
            {
                Exception ex = new Exception("Inquiry PA response for the section providerObj is NULL/Empty.");
                CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetOrderingProvidingInformationFromINquireResponse method", ex);
        }
    }

    private void GetDignosisInformationFromInquireResponse(InquirePriorAuthResponse InquirePriorAuthResponse)
    {
        try
        {
            int paTypeID = 0;
            switch (rblClaimType.SelectedValue.ToUpper())
            {
                case "DENTAL":
                    paTypeID = CON.PAClaimsType.Dental;
                    break;
                case "PROFESSIONAL":
                    paTypeID = CON.PAClaimsType.Professional;
                    break;
                case "INSTITUTIONAL":
                    paTypeID = CON.PAClaimsType.Institutional;
                    break;
            }
            if (InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E != null)
            {
                DataSet ds = new DataSet();
                List<Models.Data.PriorAuthDiagnosis> diagnoses = new List<Models.Data.PriorAuthDiagnosis>();


                for (int loop = 1; loop <= 12; loop++)
                {
                    Models.Data.PriorAuthDiagnosis priorAuthDiagnosis = ExtractDiagnosisLineItems(InquirePriorAuthResponse, loop);

                    if (priorAuthDiagnosis != null)
                        diagnoses.Add(priorAuthDiagnosis);
                }

                if (diagnoses != null && diagnoses.Count() > 0)
                {
                    DataTable dtDiagnosis = new DataTable("PRIOR_AUTH_PATIENT_DIAGNOSIS_INFORMATION");
                    dtDiagnosis.Columns.Add(new DataColumn("Line", typeof(string)));
                    dtDiagnosis.Columns.Add(new DataColumn("PRIOR_AUTH_DIAGNOSIS_ID", typeof(string)));
                    dtDiagnosis.Columns.Add(new DataColumn("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC", typeof(string)));
                    dtDiagnosis.Columns.Add(new DataColumn("PRIOR_AUTH_DIAGNOSIS_CODE", typeof(string)));
                    dtDiagnosis.Columns.Add(new DataColumn("PRIOR_AUTH_DIAGNOSIS_DESC", typeof(string)));
                    dtDiagnosis.Columns.Add(new DataColumn("PRIOR_AUTH_DIAGNOSIS_DATE", typeof(DateTime)));
                    dtDiagnosis.Columns.Add(new DataColumn("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID", typeof(string)));
                    dtDiagnosis.Columns.Add(new DataColumn("PRIOR_AUTH_DIAGNOSIS_STATUS", typeof(string)));
                    dtDiagnosis.Columns.Add(new DataColumn("PRIOR_AUTH_TYPE", typeof(string)));
                    dtDiagnosis.Columns.Add(new DataColumn("RowState", typeof(string)));
                    dtDiagnosis.Columns.Add(new DataColumn("LAST_MODIFIED_USER", typeof(string)));
                    dtDiagnosis.Columns.Add(new DataColumn("CREATED_BY_USER", typeof(string)));
                    dtDiagnosis.Columns.Add(new DataColumn("MedicaidID", typeof(string)));

                    if (diagnoses != null && diagnoses.Count > 0)
                    {
                        foreach (Models.Data.PriorAuthDiagnosis priorAuthDiagnosis in diagnoses)
                        {

                            string diagnosisCode = string.Empty;
                            string diagnosisCodeType = string.Empty;
                            string diagnosisDate = string.Empty;
                            DateTime? diagnosisDateDT = null;

                            diagnosisCode = priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE;
                            diagnosisCodeType = priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC;
                            diagnosisDateDT = string.IsNullOrEmpty(priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE) ||
                                              (!string.IsNullOrEmpty(priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE) && priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE == CON.DefaultDateString)
                                              ? default(DateTime?) : Convert.ToDateTime(priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE);


                            Dictionary<string, string> procedureCodeMap = new Dictionary<string, string>();
                            procedureCodeMap.Add("ABK", "Principal");
                            procedureCodeMap.Add("ABJ", "Admitting");
                            procedureCodeMap.Add("ABF", "Other");
                            procedureCodeMap.Add("APR", "Patient Reason for Visit");




                            //PRIOR_AUTH_TYPE
                            string PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID = string.Empty;
                            string DiagDesc = string.Empty;
                            if (!string.IsNullOrEmpty(diagnosisCodeType) && !string.IsNullOrEmpty(procedureCodeMap[diagnosisCodeType]))
                            {
                                var spa = new PDMSService.PDMSServiceClient();

                                DataSet dataSet = spa.GetDiagnosisCodeType();
                                DataTable dt = dataSet.Tables[0];


                                var res = dt.Select("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = '" + procedureCodeMap[diagnosisCodeType] + "'");



                                if (res[0]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"] != null)
                                {
                                    PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID = Convert.ToString(res[0]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"]);
                                }


                            }
                            if (!string.IsNullOrEmpty(diagnosisCode))
                            {
                                var result = LookupTableController.GetICDDiagnosis(diagnosisCode, "ICD 10", "");
                                if (result != null && result.Tables.Count > 0 && result.Tables[0] != null && result.Tables[0].Rows.Count > 0 && result.Tables[0].Rows[0]["DiagDesc"] != null)
                                {
                                    DiagDesc = Convert.ToString(result.Tables[0].Rows[0]["DiagDesc"]);
                                }
                            }



                            var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
                            dtDiagnosis.Rows.Add("01"
                                                 , PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID
                                                 , !string.IsNullOrEmpty(diagnosisCodeType) ? procedureCodeMap[diagnosisCodeType] : string.Empty
                                                 , diagnosisCode
                                                 , DiagDesc
                                                 , diagnosisDateDT
                                                 , PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID
                                                 , Convert.ToString(0)
                                                 , Convert.ToString(paTypeID)
                                                 , "added"
                                                 , Convert.ToString(id)
                                                 , Convert.ToString(id)
                                                 , MedicaidId);

                        }
                    }
                    ds.Tables.Add(dtDiagnosis);

                }

                isDiagnosisLineOpened.Value = "false";
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Dictionary<string, string> parms2 = new Dictionary<string, string>();
                    parms2.Add("PRIOR_AUTH_TYPE", row["PRIOR_AUTH_TYPE"].ToString());
                    parms2.Add("PRIOR_AUTH_DIAGNOSIS_CODE", row["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString());
                    parms2.Add("PRIOR_AUTH_DIAGNOSIS_DESC", row["PRIOR_AUTH_DIAGNOSIS_DESC"].ToString());
                    parms2.Add("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID", row["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString());
                    parms2.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms2.Add("LAST_MODIFIED_USER", SaveCodeLNK);
                    parms2.Add("Created_On_Date_Time", DateTime.Now.ToString());
                    parms2.Add("Created_By_User", SaveCodeLNK);
                    parms2.Add("PRIOR_AUTH_DIAGNOSIS_STATUS", row["PRIOR_AUTH_DIAGNOSIS_STATUS"].ToString());
                    parms2.Add("PRIOR_AUTH_DIAGNOSIS_DATE", row["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString());
                    parms2.Add("LINK_SECTIONS", SaveCodeLNK);
                    parms2.Add("MedicaidID", row["MedicaidID"].ToString());
                    parms2.Add("Line", row["Line"].ToString());
                    parms2.Add("RowState", "added");
                    PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("INSERTPRIORAUTH_DIAGNOSIS", parms2);
                }
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private static Models.Data.PriorAuthDiagnosis ExtractDiagnosisLineItems(InquirePriorAuthResponse InquirePriorAuthResponse, int lineItem)
    {
        Models.Data.PriorAuthDiagnosis priorAuthDiagnosis = null;
        string diagnosisCode = string.Empty;
        string diagnosisCodeType = string.Empty;
        string diagnosisDate = string.Empty;
        DateTime diagnosisDateDT = new DateTime();

        if (lineItem == 1)
        {

            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI012_DiagnosisCode))
            {
                priorAuthDiagnosis = new Models.Data.PriorAuthDiagnosis();
                diagnosisCode = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI012_DiagnosisCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE = diagnosisCode;
            }
            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI011_DiagnosisRespTypeCode))
            {
                diagnosisCodeType = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI011_DiagnosisRespTypeCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = diagnosisCodeType;
            }


            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI014_DiagnosisDate)
                && InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI014_DiagnosisDate != CON.DefaultDateString)
            {
                diagnosisDate = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI014_DiagnosisDate;
                diagnosisDate = diagnosisDate.Substring(4, 2) + "/" + diagnosisDate.Substring(6, 2) + "/" + diagnosisDate.Substring(0, 4);
                if (!DateTime.TryParse(diagnosisDate, out diagnosisDateDT)) { }
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE = Convert.ToString(diagnosisDateDT);
            }
        }
        if (lineItem == 2)
        {

            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI022_DiagnosisCode))
            {
                priorAuthDiagnosis = new Models.Data.PriorAuthDiagnosis();
                diagnosisCode = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI022_DiagnosisCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE = diagnosisCode;
            }
            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI021_DiagnosisRespTypeCode))
            {
                diagnosisCodeType = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI021_DiagnosisRespTypeCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = diagnosisCodeType;
            }


            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI024_DiagnosisDate)
                && InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI024_DiagnosisDate != CON.DefaultDateString)
            {
                diagnosisDate = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI024_DiagnosisDate;
                diagnosisDate = diagnosisDate.Substring(4, 2) + "/" + diagnosisDate.Substring(6, 2) + "/" + diagnosisDate.Substring(0, 4);
                if (!DateTime.TryParse(diagnosisDate, out diagnosisDateDT)) { }
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE = Convert.ToString(diagnosisDateDT);
            }
        }
        if (lineItem == 3)
        {

            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI032_DiagnosisCode))
            {
                priorAuthDiagnosis = new Models.Data.PriorAuthDiagnosis();
                diagnosisCode = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI032_DiagnosisCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE = diagnosisCode;
            }
            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI031_DiagnosisRespTypeCode))
            {
                diagnosisCodeType = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI031_DiagnosisRespTypeCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = diagnosisCodeType;
            }


            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI034_DiagnosisDate)
                && InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI034_DiagnosisDate != CON.DefaultDateString)
            {
                diagnosisDate = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI034_DiagnosisDate;
                diagnosisDate = diagnosisDate.Substring(4, 2) + "/" + diagnosisDate.Substring(6, 2) + "/" + diagnosisDate.Substring(0, 4);
                if (!DateTime.TryParse(diagnosisDate, out diagnosisDateDT)) { }
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE = Convert.ToString(diagnosisDateDT);
            }
        }
        if (lineItem == 4)
        {

            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI042_DiagnosisCode))
            {
                priorAuthDiagnosis = new Models.Data.PriorAuthDiagnosis();
                diagnosisCode = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI042_DiagnosisCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE = diagnosisCode;
            }
            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI041_DiagnosisRespTypeCode))
            {
                diagnosisCodeType = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI041_DiagnosisRespTypeCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = diagnosisCodeType;
            }


            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI044_DiagnosisDate)
                && InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI044_DiagnosisDate != CON.DefaultDateString)
            {
                diagnosisDate = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI044_DiagnosisDate;
                diagnosisDate = diagnosisDate.Substring(4, 2) + "/" + diagnosisDate.Substring(6, 2) + "/" + diagnosisDate.Substring(0, 4);
                if (!DateTime.TryParse(diagnosisDate, out diagnosisDateDT)) { }
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE = Convert.ToString(diagnosisDateDT);
            }
        }
        if (lineItem == 5)
        {

            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI052_DiagnosisCode))
            {
                priorAuthDiagnosis = new Models.Data.PriorAuthDiagnosis();
                diagnosisCode = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI052_DiagnosisCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE = diagnosisCode;
            }
            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI051_DiagnosisRespTypeCode))
            {
                diagnosisCodeType = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI051_DiagnosisRespTypeCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = diagnosisCodeType;
            }


            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI054_DiagnosisDate)
                && InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI054_DiagnosisDate != CON.DefaultDateString)
            {
                diagnosisDate = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI054_DiagnosisDate;
                diagnosisDate = diagnosisDate.Substring(4, 2) + "/" + diagnosisDate.Substring(6, 2) + "/" + diagnosisDate.Substring(0, 4);
                if (!DateTime.TryParse(diagnosisDate, out diagnosisDateDT)) { }
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE = Convert.ToString(diagnosisDateDT);
            }
        }
        if (lineItem == 6)
        {

            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI062_DiagnosisCode))
            {
                priorAuthDiagnosis = new Models.Data.PriorAuthDiagnosis();
                diagnosisCode = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI062_DiagnosisCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE = diagnosisCode;
            }
            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI061_DiagnosisRespTypeCode))
            {
                diagnosisCodeType = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI061_DiagnosisRespTypeCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = diagnosisCodeType;
            }


            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI064_DiagnosisDate)
                && InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI064_DiagnosisDate != CON.DefaultDateString)
            {
                diagnosisDate = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI064_DiagnosisDate;
                diagnosisDate = diagnosisDate.Substring(4, 2) + "/" + diagnosisDate.Substring(6, 2) + "/" + diagnosisDate.Substring(0, 4);
                if (!DateTime.TryParse(diagnosisDate, out diagnosisDateDT)) { }
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE = Convert.ToString(diagnosisDateDT);
            }
        }
        if (lineItem == 7)
        {

            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI072_DiagnosisCode))
            {
                priorAuthDiagnosis = new Models.Data.PriorAuthDiagnosis();
                diagnosisCode = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI072_DiagnosisCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE = diagnosisCode;
            }
            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI071_DiagnosisRespTypeCode))
            {
                diagnosisCodeType = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI071_DiagnosisRespTypeCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = diagnosisCodeType;
            }


            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI074_DiagnosisDate)
                && InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI074_DiagnosisDate != CON.DefaultDateString)
            {
                diagnosisDate = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI074_DiagnosisDate;
                diagnosisDate = diagnosisDate.Substring(4, 2) + "/" + diagnosisDate.Substring(6, 2) + "/" + diagnosisDate.Substring(0, 4);
                if (!DateTime.TryParse(diagnosisDate, out diagnosisDateDT)) { }
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE = Convert.ToString(diagnosisDateDT);
            }
        }
        if (lineItem == 8)
        {

            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI082_DiagnosisCode))
            {
                priorAuthDiagnosis = new Models.Data.PriorAuthDiagnosis();
                diagnosisCode = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI082_DiagnosisCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE = diagnosisCode;
            }
            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI081_DiagnosisRespTypeCode))
            {
                diagnosisCodeType = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI081_DiagnosisRespTypeCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = diagnosisCodeType;
            }


            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI084_DiagnosisDate)
                && InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI084_DiagnosisDate != CON.DefaultDateString)
            {
                diagnosisDate = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI084_DiagnosisDate;
                diagnosisDate = diagnosisDate.Substring(4, 2) + "/" + diagnosisDate.Substring(6, 2) + "/" + diagnosisDate.Substring(0, 4);
                if (!DateTime.TryParse(diagnosisDate, out diagnosisDateDT)) { }
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE = Convert.ToString(diagnosisDateDT);
            }
        }
        if (lineItem == 9)
        {

            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI092_DiagnosisCode))
            {
                priorAuthDiagnosis = new Models.Data.PriorAuthDiagnosis();
                diagnosisCode = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI092_DiagnosisCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE = diagnosisCode;
            }
            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI091_DiagnosisRespTypeCode))
            {
                diagnosisCodeType = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI091_DiagnosisRespTypeCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = diagnosisCodeType;
            }


            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI094_DiagnosisDate)
                && InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI094_DiagnosisDate != CON.DefaultDateString)
            {
                diagnosisDate = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI094_DiagnosisDate;
                diagnosisDate = diagnosisDate.Substring(4, 2) + "/" + diagnosisDate.Substring(6, 2) + "/" + diagnosisDate.Substring(0, 4);
                if (!DateTime.TryParse(diagnosisDate, out diagnosisDateDT)) { }
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE = Convert.ToString(diagnosisDateDT);
            }
        }
        if (lineItem == 10)
        {

            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI102_DiagnosisCode))
            {
                priorAuthDiagnosis = new Models.Data.PriorAuthDiagnosis();
                diagnosisCode = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI102_DiagnosisCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE = diagnosisCode;
            }
            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI101_DiagnosisRespTypeCode))
            {
                diagnosisCodeType = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI101_DiagnosisRespTypeCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = diagnosisCodeType;
            }


            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI104_DiagnosisDate)
                && InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI104_DiagnosisDate != CON.DefaultDateString)
            {
                diagnosisDate = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI104_DiagnosisDate;
                diagnosisDate = diagnosisDate.Substring(4, 2) + "/" + diagnosisDate.Substring(6, 2) + "/" + diagnosisDate.Substring(0, 4);
                if (!DateTime.TryParse(diagnosisDate, out diagnosisDateDT)) { }
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE = Convert.ToString(diagnosisDateDT);
            }
        }
        if (lineItem == 11)
        {

            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI112_DiagnosisCode))
            {
                priorAuthDiagnosis = new Models.Data.PriorAuthDiagnosis();
                diagnosisCode = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI112_DiagnosisCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE = diagnosisCode;
            }
            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI111_DiagnosisRespTypeCode))
            {
                diagnosisCodeType = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI111_DiagnosisRespTypeCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = diagnosisCodeType;
            }


            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI114_DiagnosisDate)
                && InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI114_DiagnosisDate != CON.DefaultDateString)
            {
                diagnosisDate = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI114_DiagnosisDate;
                diagnosisDate = diagnosisDate.Substring(4, 2) + "/" + diagnosisDate.Substring(6, 2) + "/" + diagnosisDate.Substring(0, 4);
                if (!DateTime.TryParse(diagnosisDate, out diagnosisDateDT)) { }
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE = Convert.ToString(diagnosisDateDT);
            }
        }
        if (lineItem == 12)
        {

            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI122_DiagnosisCode))
            {
                priorAuthDiagnosis = new Models.Data.PriorAuthDiagnosis();
                diagnosisCode = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI122_DiagnosisCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE = diagnosisCode;
            }
            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI121_DiagnosisRespTypeCode))
            {
                diagnosisCodeType = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI121_DiagnosisRespTypeCode;
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = diagnosisCodeType;
            }


            if (!string.IsNullOrEmpty(InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI124_DiagnosisDate)
                && InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI124_DiagnosisDate != CON.DefaultDateString)
            {
                diagnosisDate = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientDiagnosis_2000E.HI124_DiagnosisDate;
                diagnosisDate = diagnosisDate.Substring(4, 2) + "/" + diagnosisDate.Substring(6, 2) + "/" + diagnosisDate.Substring(0, 4);
                if (!DateTime.TryParse(diagnosisDate, out diagnosisDateDT)) { }
                priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE = Convert.ToString(diagnosisDateDT);
            }
        }


        return priorAuthDiagnosis;
    }



    private void GetProviderNotesFromInquireResponse(InquirePriorAuthResponse InquirePriorAuthResponse)
    {
        try
        {
            if (InquirePriorAuthResponse.ResponsePayload.PriorAuthRequest278.BHTContainter.PatientEventDetails_2000E != null &&
               InquirePriorAuthResponse.ResponsePayload.PriorAuthRequest278.BHTContainter.PatientEventDetails_2000E.MessageText_2000E != null)
            {
                var provNotesObj = InquirePriorAuthResponse.ResponsePayload.PriorAuthRequest278.BHTContainter.PatientEventDetails_2000E.MessageText_2000E;
                if (provNotesObj != null)
                {
                    hdnProvNoteId.Value = provNotesObj.MSG03_Number;
                    hdnProvNoteText.Value = provNotesObj.MSG01_FreeFormMessageText.Substring(2);
                    txtProviderNotes.Text = provNotesObj.MSG01_FreeFormMessageText.Substring(2);

                    string noteid = !string.IsNullOrEmpty(provNotesObj.MSG03_Number) ? provNotesObj.MSG03_Number : "1";

                    try
                    {
                        string methodName = "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString();
                        CreateAndReturnLogInfoThreadNumber(methodName + "Logging values from response");
                        CreateAndReturnLogInfoThreadNumber(methodName + " - Provider Notes (PatientEventDetails_2000E.MessageText_2000E.MSG03_Number) = " + provNotesObj.MSG03_Number);
                        CreateAndReturnLogInfoThreadNumber(methodName + " - Provider Notes (PatientEventDetails_2000E.MessageText_2000E..MSG01_FreeFormMessageText) = " + provNotesObj.MSG01_FreeFormMessageText.Substring(2));
                    }
                    catch (Exception ex)
                    {
                        CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
                    }


                    if (!string.IsNullOrEmpty(provNotesObj.MSG01_FreeFormMessageText.Substring(2)))
                    {
                        Dictionary<string, string> notesData = new Dictionary<string, string>();
                        notesData.Add(noteid, provNotesObj.MSG01_FreeFormMessageText.Substring(2));

                        string ClaimType = rblClaimType.SelectedValue;
                        string patype;
                        if (ClaimType.Equals("dental"))
                        {
                            patype = "0";
                        }
                        else if (ClaimType.Equals("Professional"))
                        {
                            patype = "2";
                        }
                        else
                        {
                            patype = "1";
                        }

                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms.Add("LINK_SECTIONS", SaveCodeLNK);
                        parms.Add("PRIOR_AUTH_SAVE_PROVIDER_MEDICAIDID", MedicaidNumber);
                        parms.Add("PRIOR_AUTH_Authorization_Type_ID", patype);
                        parms.Add("PRIOR_AUTH_NOTES", provNotesObj.MSG01_FreeFormMessageText.Substring(2));
                        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parms.Add("LAST_MODIFIED_USER", SaveCodeLNK);
                        parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
                        parms.Add("Created_By_User", SaveCodeLNK);

                        hdnProvNoteId.Value = _spa.InsertUpdatePriorAuthPanelData("INSERTPRIORAUTH_NOTES", parms).ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetProviderNotesFromInquireResponse method", ex);
        }
    }

    private void LoadOutcomeOfReviewFromInquireResponse(InquirePriorAuthResponse InquirePriorAuthResponse)
    {
        try
        {
            int count = 0;
            DataTable dt = GetReasonDescriptionByCode();
            DataTable dataTable = new DataTable();// rows.CopyToDataTable();

            var outcomeReview = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReview_2000E;
            if (outcomeReview != null)
            {
                string reasonCode = outcomeReview.HCR03_ReviewDecisionReasonCode;

                if (dt != null && dt.Rows.Count > 0)
                {
                    var rows = dt.Copy().AsEnumerable().Where(row => row.Field<string>("PRIOR_AUTH_REASON_CODE_MMIS") == reasonCode);
                    if (rows.Any())// != null && dataTable.Rows.Count > 0)
                    {
                        count++;
                        divOutcomeofReview.Visible = false;
                        outcometbl.Rows.Clear();

                        DataRow row = rows.FirstOrDefault();
                        dataTable.Columns.Add("ODSProviderNoteID", typeof(string));
                        dataTable.Columns.Add("ReasonCode", typeof(string));
                        dataTable.Columns.Add("Note", typeof(string));
                        DataRow dataRow = dataTable.NewRow();
                        dataRow["ODSProviderNoteID"] = "1";
                        dataRow["ReasonCode"] = row["PRIOR_AUTH_REASON_CODE_MMIS"].ToString();
                        dataRow["Note"] = row["PRIOR_AUTH_REASON_CODE_DESC"].ToString();
                        dataTable.Rows.Add(dataRow);

                        HtmlTableRow htmlTableRowHeader = new HtmlTableRow();
                        HtmlTableRow htmlTableRow = new HtmlTableRow();
                        HtmlTableCell numberCell = new HtmlTableCell();

                        HtmlTableCell numberCellHeader1 = new HtmlTableCell();
                        HtmlTableCell numberCellHeader2 = new HtmlTableCell();
                        HtmlTableCell numberCellHeader3 = new HtmlTableCell();
                        htmlTableRowHeader.Style.Add("background-color", "#add8e6");

                        numberCellHeader1.Style.Add("color", "black");
                        numberCellHeader1.Style.Add("font-weight", "bold");
                        numberCellHeader1.Style.Add("width", "150px;");
                        numberCellHeader1.InnerText = "Line";

                        numberCellHeader2.Style.Add("color", "black");
                        numberCellHeader2.Style.Add("font-weight", "bold");
                        numberCellHeader1.Style.Add("width", "150px;");
                        numberCellHeader2.InnerText = "Reason Code";

                        numberCellHeader3.Style.Add("color", "black");
                        numberCellHeader3.Style.Add("font-weight", "bold");
                        numberCellHeader3.InnerText = "Reason Description";

                        htmlTableRowHeader.Controls.Add(numberCellHeader1);
                        htmlTableRowHeader.Controls.Add(numberCellHeader2);
                        htmlTableRowHeader.Controls.Add(numberCellHeader3);

                        numberCell.InnerText = "00";
                        numberCell.Attributes.Add("style", "color:black; width:150px;");
                        htmlTableRow.Controls.Add(numberCell);
                        HtmlTableCell msgCell = new HtmlTableCell();
                        msgCell.InnerText = row["PRIOR_AUTH_REASON_CODE_MMIS"].ToString();
                        msgCell.Attributes.Add("style", "color:black; width:150px;");
                        htmlTableRow.Controls.Add(msgCell);
                        HtmlTableCell reason = new HtmlTableCell();
                        reason.InnerText = row["PRIOR_AUTH_REASON_CODE_DESC"].ToString();
                        htmlTableRow.Controls.Add(reason);
                        outcometbl.Rows.Add(htmlTableRowHeader);
                        outcometbl.Rows.Add(htmlTableRow);
                        // gvOutcomeofreview.DataSource = dataTable;
                        // gvOutcomeofreview.DataBind();
                    }
                }
                //var dataTable = ds.Tables["ReviewerNotes"];
                // DataTable dt = dataSet.Tables[0];
                //gvOutcomeofreview.DataSource = dataTable;
                //gvOutcomeofreview.DataBind();
            }


            if (InquirePriorAuthResponse != null &&
               InquirePriorAuthResponse.ResponsePayload != null &&
               InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278 != null &&
               InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter != null &&
               InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails != null &&
               InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails.Count() > 0)
            {
                var listHealthCareServicesReview_2000F = InquirePriorAuthResponse.ResponsePayload.PriorAuthResponse278.BHTContainter.ServiceDetails.Select(a => a.HealthCareServicesReview_2000F);
                if (listHealthCareServicesReview_2000F != null && listHealthCareServicesReview_2000F.Count() > 0)
                {
                    foreach (var itemHealthCareServicesReview_2000F in listHealthCareServicesReview_2000F)
                    {
                        string reasonCode2000F = outcomeReview.HCR03_ReviewDecisionReasonCode;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            var rows = dt.AsEnumerable().Where(row => row.Field<string>("PRIOR_AUTH_REASON_CODE_MMIS") == reasonCode2000F);
                            if (rows.Any())// != null && dataTable.Rows.Count > 0)
                            {
                                string lineNumber = string.Empty;
                                if (count.ToString().Length == 1)
                                {
                                    lineNumber = "0" + count.ToString();
                                }

                                divOutcomeofReview.Visible = false;

                                DataRow row = rows.FirstOrDefault();

                                if (dataTable == null)
                                {
                                    dataTable.Columns.Add("ODSProviderNoteID", typeof(string));
                                    dataTable.Columns.Add("ReasonCode", typeof(string));
                                    dataTable.Columns.Add("Note", typeof(string));
                                }

                                DataRow dataRow = dataTable.NewRow();
                                dataRow["ODSProviderNoteID"] = lineNumber;
                                dataRow["ReasonCode"] = row["PRIOR_AUTH_REASON_CODE_MMIS"].ToString();
                                dataRow["Note"] = row["PRIOR_AUTH_REASON_CODE_DESC"].ToString();
                                dataTable.Rows.Add(dataRow);

                                HtmlTableRow htmlTableRow = new HtmlTableRow();
                                HtmlTableCell numberCell = new HtmlTableCell();

                                numberCell.InnerText = lineNumber;
                                htmlTableRow.Controls.Add(numberCell);
                                HtmlTableCell msgCell = new HtmlTableCell();
                                msgCell.InnerText = row["PRIOR_AUTH_REASON_CODE_MMIS"].ToString();
                                htmlTableRow.Controls.Add(msgCell);
                                HtmlTableCell reason = new HtmlTableCell();
                                reason.InnerText = row["PRIOR_AUTH_REASON_CODE_DESC"].ToString();
                                htmlTableRow.Controls.Add(reason);
                                outcometbl.Rows.Add(htmlTableRow);

                                count++;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private DataTable GetReasonDescriptionByCode()
    {
        DataTable dt = null;
        try
        {
            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                var ds = psc.GetReasonCodesOPP();// GetFacilityTypes(facilityCode, facilityDesc);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                }
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetReasonDescriptionByCode");
            return dt;
        }
        return dt;
    }

    private void LoadAttachmentsFromInquireResponse(PriorAuthResponse278RespType priorAuthResp)
    {
        var attObj = priorAuthResp.BHTContainter.PatientEventDetails_2000E.AdditionalPatientInformation_2000E;
        if (attObj != null && attObj.Count() > 0)
        {
            DataTable dt = new DataTable("Attachments");
            DataColumn documentId = new DataColumn("DOCUMENT_ID", typeof(Int32));
            DataColumn authNote = new DataColumn("PRIOR_AUTH_Note", typeof(string));
            DataColumn authType = new DataColumn("PRIOR_AUTH_DOCUMENT_TYPE_DESC", typeof(string));
            dt.Columns.Add(documentId);
            dt.Columns.Add(authNote);
            dt.Columns.Add(authType);
            foreach (var item in attObj)
            {
                dt.Rows.Add(Convert.ToInt32(item.PWK06_AttachmentControlNumber),
                           item.PWK09_RequestCategoryCode,
                           item.PWK07_AttachmentDescription);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                //gvAttachment.DataSource = dt;
                //gvAttachment.DataBind();
            }
        }
        else
        {
            //gvAttachment.Visible = false;
        }
    }

    private void LoadServiceDetailsFromInquireResponse(PriorAuthRequest278Type priorAuthRequest, PriorAuthResponse278RespType priorAuthResp)
    {
        DataSet ds = new DataSet();

        #region Dental Data Table
        DataTable dtDentalServiceDetails = new DataTable("PRIOR_AUTH_DENTAL_SERVICE_DETAILS");
        dtDentalServiceDetails.Columns.Add(new DataColumn("Line", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALSERVICE_DETAIL_ID", typeof(int)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROCEDURE_CODE_ID", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_TOOTH_NUMBER_ID", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_TOOTH_NUMBER_CODE", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS", typeof(DateTime)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS", typeof(DateTime)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_STATUS_ID", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR", typeof(string)));

        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS", typeof(DateTime)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS", typeof(DateTime)));

        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS", typeof(string)));

        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE", typeof(string)));

        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM", typeof(string)));

        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", typeof(string)));

        dtDentalServiceDetails.Columns.Add(new DataColumn("RowState", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("MedicaidID", typeof(string)));
        dtDentalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_DENTALSAVE_ID", typeof(string)));
        #endregion

        #region Professional Data Table
        DataTable dtProfessionalServiceDetails = new DataTable("PRIOR_AUTH_PROFESSIONAL_SERVICE_DETAILS");
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROCEDURE_CODE_ID", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS", typeof(DateTime)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS", typeof(DateTime)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_STATUS_ID", typeof(string)));

        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS", typeof(DateTime)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS", typeof(DateTime)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_ID", typeof(string)));

        dtProfessionalServiceDetails.Columns.Add(new DataColumn("Line", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("MedicaidID", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS", typeof(string)));
        dtProfessionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_PROFESSIONALSAVE_ID", typeof(string)));
        #endregion

        #region Institutional Data Table
        DataTable dtInstitutionalServiceDetails = new DataTable("PRIOR_AUTH_PROFESSIONAL_SERVICE_DETAILS");
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_ID", typeof(int)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_REVENUE_CODE", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", typeof(int)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", typeof(DateTime)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", typeof(DateTime)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_STATUS_ID", typeof(string)));

        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("Line", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS", typeof(string)));

        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_REQUESTED_UNITS_ID", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_LEVEL_CARE_ID", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO", typeof(string)));

        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("MedicaidID", typeof(string)));
        dtInstitutionalServiceDetails.Columns.Add(new DataColumn("PRIOR_AUTH_INSTITUTIONALSAVE_ID", typeof(int)));
        //dtInstitutionalServiceDetails.Columns.Add(new DataColumn("RowState", typeof(string)));
        #endregion

        int k = 0;
        foreach (var item in priorAuthResp.BHTContainter.ServiceDetails)
        {
            ServiceDetails_2000FType reqserviceDetail = Helper.TryGetValueAtIndex(priorAuthRequest.BHTContainter.ServiceDetails, k);

            var denSrvDts = item.DentalService_2000F;
            SV3Type denSrvDtsReq = null;
            SV1Type profSrvDtsReq = null;
            SV2Type insSrvDtsReq = null;
            //-->OHPNM-10437
            int totalUnits = 0;
            if (item != null && item.TotalUnits > 0)
            {
                totalUnits = item.TotalUnits;
            }

            int remainingUnits = 0;
            if (item != null && item.RemainingUnits > 0)
            {
                remainingUnits = item.RemainingUnits;
            }

            string reqDates = string.Empty;



            if (priorAuthRequest.BHTContainter.ServiceDetails != null && priorAuthRequest.BHTContainter.ServiceDetails.Count() > 0)
            {

                if (reqserviceDetail != null)
                {
                    denSrvDtsReq = reqserviceDetail.DentalService_2000F;
                    profSrvDtsReq = reqserviceDetail.ProfessionalService_2000F;
                    insSrvDtsReq = reqserviceDetail.InstitutionalServiceLine_2000F;
                    reqDates = reqserviceDetail.ServiceDate_2000F != null ? reqserviceDetail.ServiceDate_2000F.DTP03_AccidentDate : "";
                }
            }
            var profSrvDts = item.ProfessionalService_2000F;
            var insSrvDts = item.InstitutionalServiceLine_2000F;
            var fdosObj = item.ServiceDate_2000F;

            var dentalToothSurfaceInfo = item.ToothInformation_2000F;
            string ToothSurfaceCode1 = string.Empty;
            string ToothSurfaceCode2 = string.Empty;
            string ToothSurfaceCode3 = string.Empty;
            string ToothSurfaceCode4 = string.Empty;
            string ToothSurfaceCode5 = string.Empty;

            if (dentalToothSurfaceInfo != null && dentalToothSurfaceInfo.Length > 0)
            {
                ToothSurfaceCode1 = dentalToothSurfaceInfo[0].TOO031_ToothSurfaceCode;
                ToothSurfaceCode2 = dentalToothSurfaceInfo[0].TOO032_ToothSurfaceCode;
                ToothSurfaceCode3 = dentalToothSurfaceInfo[0].TOO033_ToothSurfaceCode;
                ToothSurfaceCode4 = dentalToothSurfaceInfo[0].TOO034_ToothSurfaceCode;
                ToothSurfaceCode5 = dentalToothSurfaceInfo[0].TOO035_ToothSurfaceCode;
            }

            string fdos = string.Empty;

            if (fdosObj != null)
            {
                fdos = string.IsNullOrEmpty(fdosObj.DTP03_AccidentDate) || fdosObj.DTP03_AccidentDate == "?" ? "" : fdosObj.DTP03_AccidentDate;
            }
            //var AuthDetails = insSrvDts.

            Dictionary<string, string> statusMap = LoadPAStatusMapping();

            var hcsObj = string.Empty;
            if (priorAuthResp != null && priorAuthResp.BHTContainter != null &&
                priorAuthResp.BHTContainter.ServiceDetails[0] != null &&
                item.HealthCareServicesReview_2000F != null &&
                item.HealthCareServicesReview_2000F.HCR01_ActionCode != null)
            {
                hcsObj = item.HealthCareServicesReview_2000F.HCR01_ActionCode;
            }

            string status = !string.IsNullOrEmpty(hcsObj) && statusMap.ContainsKey(hcsObj) ? statusMap[hcsObj] : "";

            if (profSrvDts != null)
            {
                txtProfessionalAuthDollars.Text = profSrvDts.SV108_MonetaryAmount;
                txtProfessionalReqUnits.Text = profSrvDtsReq != null ? profSrvDtsReq.SV104_ServiceUnitCount : "";
                txtDentalReqDollars.Text = profSrvDtsReq != null ? profSrvDtsReq.SV104_ServiceUnitCount : "";
                txtProfessionalAuthUnits.Text = profSrvDts.SV104_ServiceUnitCount;
                txtProfessionalRemainingUnits.Text = remainingUnits.ToString();
            }

            string requestedTdos = string.Empty;
            DateTime requestedTdosDt = new DateTime();
            string requestedFdos = string.Empty;
            DateTime requestedFdosDt = new DateTime();
            if (!string.IsNullOrEmpty(reqDates))
            {
                var dates = reqDates.Split('-');

                requestedFdos = dates[0].Substring(4, 2) + "/" + dates[0].Substring(6, 2) + "/" + dates[0].Substring(0, 4);
                requestedFdosDt = Convert.ToDateTime(requestedFdos);

                requestedTdos = dates[1].Substring(4, 2) + "/" + dates[1].Substring(6, 2) + "/" + dates[1].Substring(0, 4);
                requestedTdosDt = Convert.ToDateTime(requestedTdos);
            }

            string authorizedTdos = string.Empty;
            DateTime authorizedTdosDt = new DateTime();
            string authorizedFdos = string.Empty;
            DateTime authorizedFdosDt = new DateTime();
            string authorizedDollar = string.Empty;



            if (priorAuthResp != null && priorAuthResp.BHTContainter != null
                && item != null
                && item.AuthorizedTDOS_2000F != null
                && item.AuthorizedTDOS_2000F.DTP03_AccidentDate != null)
            {
                authorizedTdos = item.AuthorizedTDOS_2000F.DTP03_AccidentDate.Substring(4, 2) + "/" + item.AuthorizedTDOS_2000F.DTP03_AccidentDate.Substring(6, 2) + "/" + item.AuthorizedTDOS_2000F.DTP03_AccidentDate.Substring(0, 4);
                authorizedTdosDt = Convert.ToDateTime(authorizedTdos);
            }



            if (priorAuthResp != null && priorAuthResp.BHTContainter != null
                && item != null
                && item.AuthorizedFDOS_2000F != null
                && item.AuthorizedFDOS_2000F.DTP03_AccidentDate != null)
            {
                authorizedFdos = item.AuthorizedFDOS_2000F.DTP03_AccidentDate.Substring(4, 2) + "/" + item.AuthorizedFDOS_2000F.DTP03_AccidentDate.Substring(6, 2) + "/" + item.AuthorizedFDOS_2000F.DTP03_AccidentDate.Substring(0, 4);
                authorizedFdosDt = Convert.ToDateTime(authorizedFdos);
            }

            if (!string.IsNullOrEmpty(fdos))
            {
                var authDates = fdos.Split('-');
                authorizedFdos = authDates[0].Substring(4, 2) + "/" + authDates[0].Substring(6, 2) + "/" + authDates[0].Substring(0, 4);
                authorizedFdosDt = Convert.ToDateTime(authorizedFdos);

                authorizedTdos = authDates[1].Substring(4, 2) + "/" + authDates[1].Substring(6, 2) + "/" + authDates[1].Substring(0, 4);
                authorizedTdosDt = Convert.ToDateTime(authorizedTdos);
            }

            txtProfessionalAuthFDOS.Text = string.IsNullOrEmpty(authorizedFdos) ? "" : authorizedFdos;
            txtProfessionalAuthTDOS.Text = string.IsNullOrEmpty(authorizedTdos) ? "" : authorizedTdos;
            txtProfessionalServDetailsStatus.Text = status;

            if (insSrvDts != null)
            {
                txtRequestUnt.Text = insSrvDtsReq != null ? insSrvDtsReq.SV205_ServiceUnitCount : "";
                txtAuthorizedUnits.Text = insSrvDts.SV205_ServiceUnitCount;
                txtRequestedDollars.Text = insSrvDtsReq != null ? insSrvDtsReq.SV203_ServiceLineAmount : "0.00";
                txtAuthorizedDollars.Text = insSrvDts.SV203_ServiceLineAmount;
                authorizedDollar = insSrvDts.SV203_ServiceLineAmount;
                txtLnRemainingUnits.Text = remainingUnits.ToString();
            }

            txtAuthorizedFromDOS.Text = string.IsNullOrEmpty(authorizedFdos) ? "" : authorizedFdos;
            txtAuthorizedToDOS.Text = string.IsNullOrEmpty(authorizedTdos) ? "" : authorizedTdos;
            txtLnStatus.Text = status;

            if (denSrvDts != null)
            {
                txtDentalReqDollars.Text = denSrvDtsReq != null ? denSrvDtsReq.SV302_LineItemChargeAmount : "";
                txtDentalAuthDollars.Text = denSrvDts.SV302_ServiceLineAmount;
                txtDentalReqUnits.Text = denSrvDtsReq != null ? denSrvDtsReq.SV306_ServiceUnitCount : "";
                txtDentalAuthUnits.Text = denSrvDts.SV306_ServiceUnitCount;
                txtDentalRemainingUnits.Text = remainingUnits.ToString();
                txtDentalProcCodeDescription.Text = !string.IsNullOrEmpty(denSrvDts.SV307_Description) ? denSrvDts.SV307_Description : "";
            }

            txtDentalAuthFDOS.Text = string.IsNullOrEmpty(authorizedFdos) ? "" : authorizedFdos; ;
            txtDentalAuthTDOS.Text = string.IsNullOrEmpty(authorizedTdos) ? "" : authorizedTdos; ;
            txtDentalServDetailsStatus.Text = status;

            string providerServiceNote = string.Empty;
            if (priorAuthRequest.BHTContainter.ServiceDetails != null && priorAuthRequest.BHTContainter.ServiceDetails.Count() > 0 && reqserviceDetail != null)
            {
                if (reqserviceDetail.MessageText_2000F != null &&
                    reqserviceDetail.MessageText_2000F.MSG01_FreeFormMessageText != null)
                {
                    providerServiceNote = reqserviceDetail.MessageText_2000F.MSG01_FreeFormMessageText;
                }
            }

            string serviceTrackingNumber = string.Empty;
            if (priorAuthRequest.BHTContainter.ServiceDetails != null &&
                priorAuthRequest.BHTContainter.ServiceDetails.Count() > 0 && reqserviceDetail != null &&

                reqserviceDetail.ServiceTraceNumber_2000F != null &&
                reqserviceDetail.ServiceTraceNumber_2000F.Count() > 0)
            {
                if (reqserviceDetail.ServiceTraceNumber_2000F[0].TRN02_PatientEventTraceNumber != null)
                {
                    serviceTrackingNumber = reqserviceDetail.ServiceTraceNumber_2000F[0].TRN02_PatientEventTraceNumber;
                }
            }

            string levelOfCare = string.Empty;
            if (insSrvDts != null &&
               !string.IsNullOrEmpty(insSrvDts.SV210_LevelOfCareCode))
            {
                Dictionary<int, string> dictLevelOfCareMap = new Dictionary<int, string>();
                dictLevelOfCareMap.Add(1, "Skilled Nursing Facility(SNF)");
                dictLevelOfCareMap.Add(2, "Intermediate Care Facility(ICF)");
                dictLevelOfCareMap.Add(3, "Intermediate Care Facility – Mentally Retarded(ICF-MR)");
                dictLevelOfCareMap.Add(4, "Chronic Disease Hospital(CD)");
                dictLevelOfCareMap.Add(5, "Intermediate Care Facility(ICF) Level II");
                dictLevelOfCareMap.Add(6, "Special Skilled Nursing Facility(SNF)");
                dictLevelOfCareMap.Add(7, "Nursing Facility(NF)");
                dictLevelOfCareMap.Add(8, "Hospice");

                dictLevelOfCareMap.TryGetValue(Convert.ToInt32(insSrvDts.SV210_LevelOfCareCode), out levelOfCare);
            }


            if (denSrvDts != null)
            {
                try
                {
                    int secondsSinceMidnight = Convert.ToInt32(DateTime.Now.Millisecond);
                    Random random = new Random(secondsSinceMidnight);

                    var toothInfo = item.ToothInformation_2000F;

                    string inlayCode = default(string);

                    if (!string.IsNullOrEmpty(denSrvDts.SV305_ProthesisCrownOrInlayCode) && denSrvDts.SV305_ProthesisCrownOrInlayCode == "I")
                    {
                        inlayCode = "3";
                    }
                    else if (!string.IsNullOrEmpty(denSrvDts.SV305_ProthesisCrownOrInlayCode) && denSrvDts.SV305_ProthesisCrownOrInlayCode == "R")
                    {

                        inlayCode = "4";
                    }

                    dtDentalServiceDetails.Rows.Add("01", //Line
                                   (random.Next() + k), //PRIOR_AUTH_DENTALSERVICE_DETAIL_ID
                                   denSrvDts.SV3012_ProcedureCode, //PRIOR_AUTH_PROCEDURE_CODE_ID
                                   toothInfo != null && toothInfo.Count() > 0 && !string.IsNullOrEmpty(toothInfo[0].TOO02_ToothCode) ?
                                   Convert.ToString(item.ToothInformation_2000F[0].TOO02_ToothCode) : "", //PRIOR_AUTH_TOOTH_NUMBER_ID
                                   toothInfo != null && toothInfo.Count() > 0 && !string.IsNullOrEmpty(toothInfo[0].TOO02_ToothCode) ?
                                   Convert.ToString(item.ToothInformation_2000F[0].TOO02_ToothCode) : "", //PRIOR_AUTH_TOOTH_NUMBER_CODE
                                   !string.IsNullOrEmpty(denSrvDts.SV3041_OralCavityDesignationCode) ? denSrvDts.SV3041_OralCavityDesignationCode : "", //PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS
                                   (denSrvDtsReq != null && !string.IsNullOrEmpty(denSrvDtsReq.SV306_ServiceUnitCount)) ? denSrvDtsReq.SV306_ServiceUnitCount : "", //PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS
                                   (denSrvDtsReq != null && !string.IsNullOrEmpty(denSrvDtsReq.SV302_LineItemChargeAmount)) ? denSrvDtsReq.SV302_LineItemChargeAmount : "", //PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR
                                   requestedFdosDt,//fdos, <-- relook //PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS
                                   requestedTdosDt,// fdos, <-- relook //PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS
                                   !string.IsNullOrEmpty(status) ? status : "", //PRIOR_AUTH_STATUS_ID //PRIOR_AUTH_DENTALSERVICE_DETAIL_ID
                                   !string.IsNullOrEmpty(item.DentalService_2000F.SV306_ServiceUnitCount) ? item.DentalService_2000F.SV306_ServiceUnitCount : "",//PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS
                                   !string.IsNullOrEmpty(item.DentalService_2000F.SV302_ServiceLineAmount) ? item.DentalService_2000F.SV302_ServiceLineAmount : "",//PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR
                                   authorizedFdosDt,//PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS
                                   authorizedTdosDt,//PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS
                                   !string.IsNullOrEmpty(inlayCode) ? inlayCode : "",//PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID
                                   !string.IsNullOrEmpty(denSrvDts.SV3042_OralCavityDesignationCode) ? denSrvDts.SV3042_OralCavityDesignationCode : "",//PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS
                                   !string.IsNullOrEmpty(denSrvDts.SV3043_OralCavityDesignationCode) ? denSrvDts.SV3043_OralCavityDesignationCode : "",//PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS
                                   !string.IsNullOrEmpty(denSrvDts.SV3044_OralCavityDesignationCode) ? denSrvDts.SV3044_OralCavityDesignationCode : "",//PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS
                                   !string.IsNullOrEmpty(denSrvDts.SV3045_OralCavityDesignationCode) ? denSrvDts.SV3045_OralCavityDesignationCode : "",//PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS
                                   ToothSurfaceCode1,//PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE
                                   ToothSurfaceCode2,//PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE
                                   ToothSurfaceCode3,//PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE
                                   ToothSurfaceCode4,//PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE
                                   ToothSurfaceCode5,//PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE
                                   remainingUnits, //PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS
                                   serviceTrackingNumber, // PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM
                                   denSrvDtsReq != null && !string.IsNullOrEmpty(denSrvDtsReq.SV307_Description) ? denSrvDtsReq.SV307_Description : "", //PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC
                                   providerServiceNote, //PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE
                                   "updated", // RowState
                                   MedicaidId, //MedicaidID
                                   PriorAuthHospitalController.GetPriorAuthServiceDetailSaveID("Dental", MedicaidId)); //PRIOR_AUTH_DENTALSAVE_ID

                    //gvServiceDetailDental.DataSource = dtDentalServiceDetails;
                    //gvServiceDetailDental.DataBind();
                    //GetDentalServiceDetails();
                    rblClaimType.SelectedValue = "dental";

                }
                catch (Exception ex)
                {
                    CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetPriorAuthServiceDetailSaveID");
                }
            }
            else if (profSrvDts != null)
            {
                try
                {
                    int secondsSinceMidnight = Convert.ToInt32(DateTime.Now.Millisecond);
                    Random random = new Random(secondsSinceMidnight);
                    //gvServiceDetailProfessional
                    int requestedUnitsID = 0;
                    if (!string.IsNullOrEmpty(profSrvDts.SV103_UnitOrBasisForMeasurementCode))
                    {
                        DataSet measurementsDS = LookupTableController.GetRequestedUnitMeasures();
                        DataTable measurementsDT = measurementsDS.Tables[0];
                        DataRow[] res = measurementsDT.Select("PRIOR_AUTH_REQUESTED_UNITS_CODE = '" + profSrvDts.SV103_UnitOrBasisForMeasurementCode + "'");

                        if (res != null)
                        {
                            if (res[0]["PRIOR_AUTH_REQUESTED_UNITS_ID"] != null)
                            {
                                requestedUnitsID = res[0]["PRIOR_AUTH_REQUESTED_UNITS_ID"] != null && res[0]["PRIOR_AUTH_REQUESTED_UNITS_ID"] != DBNull.Value ? Convert.ToInt32(res[0]["PRIOR_AUTH_REQUESTED_UNITS_ID"]) : 0;
                            }
                        }
                    }


                    dtProfessionalServiceDetails.Rows.Add(!string.IsNullOrEmpty(profSrvDts.SV1012_ProcedureCode) ? profSrvDts.SV1012_ProcedureCode : "", //PRIOR_AUTH_PROCEDURE_CODE_ID
                                !string.IsNullOrEmpty(profSrvDts.SV1013_ProcedureModifier) ? profSrvDts.SV1013_ProcedureModifier : "", //PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1
                                !string.IsNullOrEmpty(profSrvDts.SV1014_ProcedureModifier) ? profSrvDts.SV1014_ProcedureModifier : "", //PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2
                                !string.IsNullOrEmpty(profSrvDts.SV1015_ProcedureModifier) ? profSrvDts.SV1015_ProcedureModifier : "", //PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3
                                !string.IsNullOrEmpty(profSrvDts.SV1016_ProcedureModifier) ? profSrvDts.SV1016_ProcedureModifier : "", //PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4
                                (profSrvDtsReq != null && !string.IsNullOrEmpty(profSrvDtsReq.SV104_ServiceUnitCount)) ? profSrvDtsReq.SV104_ServiceUnitCount : "", //PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS
                                requestedUnitsID, //PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID
                                (profSrvDtsReq != null && !string.IsNullOrEmpty(profSrvDtsReq.SV102_ServiceLineAmount)) ? profSrvDtsReq.SV102_ServiceLineAmount : "", //PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR
                                requestedFdosDt, //PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS
                                requestedTdosDt, //PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS
                                !string.IsNullOrEmpty(status) ? status : "", //PRIOR_AUTH_STATUS_ID
                                providerServiceNote, //PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE
                                profSrvDtsReq != null && !string.IsNullOrEmpty(profSrvDtsReq.SV1017_ProcedureCodeDescription) ? profSrvDtsReq.SV1017_ProcedureCodeDescription : "",//PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC                                
                                serviceTrackingNumber, //PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM
                                remainingUnits, //PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS
                                authorizedTdosDt, //PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS
                                authorizedFdosDt, //PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS
                                !string.IsNullOrEmpty(profSrvDts.SV102_ServiceLineAmount) ? profSrvDts.SV102_ServiceLineAmount : "", //PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR
                                !string.IsNullOrEmpty(profSrvDts.SV104_ServiceUnitCount) ? profSrvDts.SV104_ServiceUnitCount : "", //PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS
                                random.Next() + k, //PRIOR_AUTH_PROFFSERVICE_DETAIL_ID
                                "01", //Line
                                MedicaidId,
                                (profSrvDtsReq != null && !string.IsNullOrEmpty(profSrvDtsReq.SV104_ServiceUnitCount)) ? profSrvDtsReq.SV104_ServiceUnitCount : "", //PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS
                                PriorAuthHospitalController.GetPriorAuthServiceDetailSaveID("Professional", MedicaidId)); //PRIOR_AUTH_PROFESSIONALSAVE_ID

                    //gvServiceDetailProfessional.DataSource = dtProfessionalServiceDetails;
                    //gvServiceDetailProfessional.DataBind();
                    rblClaimType.SelectedValue = "Professional";
                    //gvServiceDetailProfessional.Visible = true;
                    // lblprofNoDetailsFound.Visible = false;

                    GetProfessionalServiceDetails();
                }
                catch (Exception ex)
                {
                    CreateAndReturnLogThreadNumber(ex, "SubmitPA-ProfessionalServiceDetailsRowAdding");
                }
            }
            else if (insSrvDts != null)
            {
                try
                {
                    int PRIOR_AUTH_SERVICE_CODE_TYPE_ID = 0;
                    DataSet dataSet = null;

                    try
                    {
                        if (_spa == null)
                        {
                            _spa = new PDMSService.PDMSServiceClient();
                        }
                        dataSet = _spa.GetServiceTypeCode();
                    }
                    catch (Exception ex)
                    {
                        CreateAndReturnLogThreadNumber(ex, "SubmitPA-LoadServiceDetailsFromInquireResponse-GetServiceCodeTypeCode");
                    }

                    if (!string.IsNullOrEmpty(insSrvDts.SV2021_ProductOrServiceIdQualifier) &&
                        dataSet != null &&
                        dataSet.Tables != null &&
                        dataSet.Tables.Count > 0 &&
                        dataSet.Tables[0].Rows.Count > 0)
                    {
                        if (insSrvDts.SV2021_ProductOrServiceIdQualifier.ToLower() == "zz")
                        {
                            var matchingDataRows = dataSet.Tables[0].Select("PRIOR_AUTH_SERVICE_CODE_TYPE_CODE like '%ICD 10 Procedure%'");
                            if (matchingDataRows != null && matchingDataRows.Count() > 0)
                            {
                                PRIOR_AUTH_SERVICE_CODE_TYPE_ID = Convert.ToInt32(matchingDataRows[0]["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"]);
                            }
                        }
                        else if (insSrvDts.SV2021_ProductOrServiceIdQualifier.ToLower() == "hc")
                        {
                            var matchingDataRows = dataSet.Tables[0].Select("PRIOR_AUTH_SERVICE_CODE_TYPE_CODE like '%HCPCS%'");
                            if (matchingDataRows != null && matchingDataRows.Count() > 0)
                            {
                                PRIOR_AUTH_SERVICE_CODE_TYPE_ID = Convert.ToInt32(matchingDataRows[0]["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"]);
                            }
                        }
                    }

                    int requestedUnitsID = 0;
                    // int requestedUnitsFees = 0;
                    if (!string.IsNullOrEmpty(insSrvDts.SV204_UnitOrBasisForMeasurementCode))
                    {
                        DataSet measurementsDS = LookupTableController.GetRequestedUnitMeasures();
                        DataTable measurementsDT = measurementsDS.Tables[0];
                        DataRow[] res = measurementsDT.Select("PRIOR_AUTH_REQUESTED_UNITS_CODE = '" + insSrvDts.SV204_UnitOrBasisForMeasurementCode + "'");

                        if (res != null)
                        {
                            if (res[0]["PRIOR_AUTH_REQUESTED_UNITS_ID"] != null)
                            {
                                requestedUnitsID = res[0]["PRIOR_AUTH_REQUESTED_UNITS_ID"] != null && res[0]["PRIOR_AUTH_REQUESTED_UNITS_ID"] != DBNull.Value ? Convert.ToInt32(res[0]["PRIOR_AUTH_REQUESTED_UNITS_ID"]) : 0;
                            }
                        }

                    }

                    string sv205_ServiceUnitCount = string.Empty;
                    string sv2027_ProcedureCodeDescription = string.Empty;
                    string sv203_ServiceLineAmount = "0.00";

                    if (insSrvDtsReq != null)
                    {
                        if (!string.IsNullOrEmpty(insSrvDtsReq.SV205_ServiceUnitCount))
                        {
                            sv205_ServiceUnitCount = insSrvDtsReq.SV205_ServiceUnitCount;
                        }

                        if (!string.IsNullOrEmpty(insSrvDtsReq.SV2027_ProcedureCodeDescription))
                        {
                            sv2027_ProcedureCodeDescription = insSrvDtsReq.SV2027_ProcedureCodeDescription;
                        }

                        if (!string.IsNullOrEmpty(insSrvDtsReq.SV203_ServiceLineAmount))
                        {
                            sv203_ServiceLineAmount = insSrvDtsReq.SV203_ServiceLineAmount;
                        }
                    }

                    int secondsSinceMidnight = Convert.ToInt32(DateTime.Now.Millisecond);
                    Random random = new Random(secondsSinceMidnight);

                    dtInstitutionalServiceDetails.Rows.Add(random.Next() + k,
                                insSrvDts != null && !string.IsNullOrEmpty(insSrvDts.SV201_ServiceLineRevenueCode) ? insSrvDts.SV201_ServiceLineRevenueCode : "", //PRIOR_AUTH_SERVICE_REVENUE_CODE
                                PRIOR_AUTH_SERVICE_CODE_TYPE_ID,//insSrvDts.SV2021_ProductOrServiceIdQualifier, //PRIOR_AUTH_SERVICE_CODE_TYPE_ID
                                !string.IsNullOrEmpty(insSrvDts.SV2022_ProcedureCode) ? insSrvDts.SV2022_ProcedureCode : "", //PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE
                                totalUnits,//(insSrvDtsReq != null && !string.IsNullOrEmpty(insSrvDtsReq.SV203_ServiceLineAmount)) ? insSrvDtsReq.SV203_ServiceLineAmount : "", //PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE
                                requestedFdosDt, //PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS
                                requestedTdosDt, //PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS
                                !string.IsNullOrEmpty(status) ? status : "", //PRIOR_AUTH_STATUS_ID
                                sv205_ServiceUnitCount, //PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS
                                insSrvDts != null && !string.IsNullOrEmpty(insSrvDts.SV205_ServiceUnitCount) ? insSrvDts.SV205_ServiceUnitCount : "", //PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS
                                sv203_ServiceLineAmount, //PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR
                                authorizedDollar,
                                "01", //Line
                                authorizedFdosDt, //PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS
                                authorizedTdosDt, //PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS
                                requestedUnitsID, //!string.IsNullOrEmpty(insSrvDts.SV204_UnitOrBasisForMeasurementCode) ? insSrvDts.SV204_UnitOrBasisForMeasurementCode : ""  //PRIOR_AUTH_REQUESTED_UNITS_ID
                                                  //requestedUnitsFees,//PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE
                                sv2027_ProcedureCodeDescription,// insSrvDts.SV2027_ProcedureCodeDescription, //PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC
                                providerServiceNote, //PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE
                                insSrvDts != null ? insSrvDts.SV210_LevelOfCareCode : string.Empty, //levelOfCare //PRIOR_AUTH_LEVEL_CARE_ID
                                remainingUnits, //PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS
                                serviceTrackingNumber, //PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO
                                MedicaidId, //MedicaidID
                                PriorAuthHospitalController.GetPriorAuthServiceDetailSaveID("Institutional", MedicaidId));     //PRIOR_AUTH_INSTITUTIONALSAVE_ID                

                    rblClaimType.SelectedValue = "Institutional";
                }
                catch (Exception ex)
                {
                    CreateAndReturnLogThreadNumber(ex, "SubmitPA-InstitutionalServiceDetailsLoad");
                }
            }
            k++;
        }

        try
        {
            if (dtDentalServiceDetails != null && dtDentalServiceDetails.Rows.Count > 0)
            {
                ds.Tables.Add(dtDentalServiceDetails);

                foreach (DataRow row in dtDentalServiceDetails.Rows)
                {
                    Dictionary<string, string> parms2 = new Dictionary<string, string>();
                    parms2.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", row["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_TOOTH_NUMBER_ID", row["PRIOR_AUTH_TOOTH_NUMBER_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS", row["PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS", row["PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS", row["PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS", row["PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS", row["PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE", row["PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE", row["PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE", row["PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE", row["PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE", row["PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_STATUS_ID", row["PRIOR_AUTH_STATUS_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID", row["PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", row["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"].ToString());
                    parms2.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms2.Add("LAST_MODIFIED_USER", SaveCodeLNK);
                    parms2.Add("Created_On_Date_Time", DateTime.Now.ToString());
                    parms2.Add("Created_By_User", SaveCodeLNK);
                    parms2.Add("PRIOR_AUTH_DENTALSAVE_ID", row["PRIOR_AUTH_DENTALSAVE_ID"].ToString());
                    parms2.Add("MedicaidID", row["MedicaidID"].ToString());
                    parms2.Add("Line", row["Line"].ToString());
                    parms2.Add("PRIOR_AUTH_STATUS_TYPE", returnPATypeID());
                    parms2.Add("LINK_SECTIONS", SaveCodeLNK);
                    PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("INSERTPRIORAUTH_DENTALSERVICEDETAILS", parms2);
                }
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-InstitutionalServiceDetailsLoad SaveDB");
        }

        try
        {
            if (dtProfessionalServiceDetails != null && dtProfessionalServiceDetails.Rows.Count > 0)
            {
                foreach (DataRow row in dtProfessionalServiceDetails.Rows)
                {
                    Dictionary<string, string> parms2 = new Dictionary<string, string>();
                    parms2.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", row["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_STATUS_ID", row["PRIOR_AUTH_STATUS_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"].ToString());
                    parms2.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms2.Add("LAST_MODIFIED_USER", SaveCodeLNK);
                    parms2.Add("Created_On_Date_Time", DateTime.Now.ToString());
                    parms2.Add("Created_By_User", SaveCodeLNK);
                    parms2.Add("PRIOR_AUTH_PROFESSIONALSAVE_ID", row["PRIOR_AUTH_PROFESSIONALSAVE_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"].ToString());
                    parms2.Add("MedicaidID", row["MedicaidID"].ToString());
                    parms2.Add("Line", row["Line"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS"].ToString());
                    //parms2.Add("PRIOR_AUTH_SAVE_CODE_ID", row["PRIOR_AUTH_SAVE_CODE_ID"].ToString());
                    parms2.Add("LINK_SECTIONS", SaveCodeLNK);
                    PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("INSERTPRIORAUTH_PROFFSERVICEDETAILS", parms2);
                }
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-InstitutionalServiceDetailsLoad SaveDB");
        }

        try
        {
            if (dtInstitutionalServiceDetails != null && dtInstitutionalServiceDetails.Rows.Count > 0)
            {
                foreach (DataRow row in dtInstitutionalServiceDetails.Rows)
                {
                    Dictionary<string, string> parms2 = new Dictionary<string, string>();
                    parms2.Add("PRIOR_AUTH_SERVICE_REVENUE_CODE", row["PRIOR_AUTH_SERVICE_REVENUE_CODE"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", row["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE", row["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", row["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_REQUESTED_UNITS_ID", row["PRIOR_AUTH_REQUESTED_UNITS_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", row["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", row["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", row["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_STATUS_ID", PAStatusIds(row["PRIOR_AUTH_STATUS_ID"].ToString()));
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC", row["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE", row["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"].ToString());
                    parms2.Add("PRIOR_AUTH_LEVEL_CARE_ID", row["PRIOR_AUTH_LEVEL_CARE_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS", row["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", row["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR", row["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS", row["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS", row["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"].ToString());
                    parms2.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms2.Add("LAST_MODIFIED_USER", SaveCodeLNK);
                    parms2.Add("Created_On_Date_Time", DateTime.Now.ToString());
                    parms2.Add("Created_By_User", SaveCodeLNK);
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO", row["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"].ToString());
                    parms2.Add("PRIOR_AUTH_INSTITUTIONALSAVE_ID", row["PRIOR_AUTH_INSTITUTIONALSAVE_ID"].ToString());
                    parms2.Add("MedicaidID", row["MedicaidID"].ToString());
                    parms2.Add("Line", row["Line"].ToString());
                    parms2.Add("LINK_SECTIONS", SaveCodeLNK);
                    PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("INSERTPRIORAUTH_INSTSERVICEDETAILS", parms2);
                }
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-InstitutionalServiceDetailsLoad SaveDB");
        }

        //This check is purely for institutional PA types
        if (rblClaimType.SelectedItem != null && string.IsNullOrEmpty(rblClaimType.SelectedItem.Text) && priorAuthResp.BHTContainter.PatientEventDetails_2000E.InstitutionalClaimCode_2000E != null)
        {
            rblClaimType.SelectedValue = "Institutional";
        }
    }

    private string PAStatusIds(string status)
    {

        string statusCode = string.Empty;

        switch (status)
        {
            case "Pend":
                statusCode = "0";
                break;
            case "Approved":
                statusCode = "2";
                break;
            case "Denied":
                statusCode = "3";
                break;
            case "Partially Approved":
                statusCode = "4";
                break;
            case "InProcess":
                statusCode = "5";
                break;
            case "Pending Addtl Info":
                statusCode = "6";
                break;
            case "Closed":
                statusCode = "7";
                break;
            default:
                statusCode = "1";
                break;
        }
        return statusCode;
    }

    private void LoadServiceInformationFromInquireResponse(PriorAuthRequest278Type priorAuthReq, PriorAuthResponse278RespType priorAuthResp)
    {
        try
        {
            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E == null) return;
            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.AccidentDate_2000E != null &&
                !string.IsNullOrEmpty(priorAuthResp.BHTContainter.PatientEventDetails_2000E.AccidentDate_2000E.DTP03_AccidentDate))
            {
                string dateStr = priorAuthResp.BHTContainter.PatientEventDetails_2000E.AccidentDate_2000E.DTP03_AccidentDate;
                txtAccidentDate.Text = dateStr.Substring(4, 2) + "/" + dateStr.Substring(6, 2) + "/" + dateStr.Substring(0, 4);
            }


            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E != null
                && priorAuthResp.BHTContainter.PatientEventDetails_2000E.PreviousReviewAuthorizationNumber_2000E != null
                && !string.IsNullOrEmpty(priorAuthResp.BHTContainter.PatientEventDetails_2000E.PreviousReviewAuthorizationNumber_2000E.REF02_RequesterSupplementalIdentification))
            {
                txtPaNum.Text = priorAuthResp.BHTContainter.PatientEventDetails_2000E.PreviousReviewAuthorizationNumber_2000E.REF02_RequesterSupplementalIdentification;
            }
            //else if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.AdministrativeReferenceNumber_2000E != null)
            //    txtPaNum.Text = priorAuthResp.BHTContainter.PatientEventDetails_2000E.AdministrativeReferenceNumber_2000E.REF04_ReferenceIdentifier;


            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.OnsetOfCurrentSymptomsOrIllnessDate_2000E != null &&
                !string.IsNullOrEmpty(priorAuthResp.BHTContainter.PatientEventDetails_2000E.OnsetOfCurrentSymptomsOrIllnessDate_2000E.DTP03_AccidentDate))
            {
                var onSetIllness = priorAuthResp.BHTContainter.PatientEventDetails_2000E.OnsetOfCurrentSymptomsOrIllnessDate_2000E.DTP03_AccidentDate;
                txtOnsetIllness.Text = onSetIllness.Substring(4, 2) + "/" + onSetIllness.Substring(6, 2) + "/" + onSetIllness.Substring(0, 4); ;
                txtProfOnsetIllness.Text = onSetIllness.Substring(4, 2) + "/" + onSetIllness.Substring(6, 2) + "/" + onSetIllness.Substring(0, 4); ;
            }

            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.EstimatedDateOfBirth_2000E != null &&
                !string.IsNullOrEmpty(priorAuthResp.BHTContainter.PatientEventDetails_2000E.EstimatedDateOfBirth_2000E.DTP03_AccidentDate))
            {
                string dateEstDOBStr = priorAuthResp.BHTContainter.PatientEventDetails_2000E.EstimatedDateOfBirth_2000E.DTP03_AccidentDate;
                txtEstBirthDate.Text = dateEstDOBStr.Substring(4, 2) + "/" + dateEstDOBStr.Substring(6, 2) + "/" + dateEstDOBStr.Substring(0, 4);

            }

            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.LastMenstrualPeriodDate_2000E != null &&
                !string.IsNullOrEmpty(priorAuthResp.BHTContainter.PatientEventDetails_2000E.LastMenstrualPeriodDate_2000E.DTP03_AccidentDate))
            {
                string dateLstMenStr = priorAuthResp.BHTContainter.PatientEventDetails_2000E.LastMenstrualPeriodDate_2000E.DTP03_AccidentDate;
                txtLstMensPeriod.Text = dateLstMenStr.Substring(4, 2) + "/" + dateLstMenStr.Substring(6, 2) + "/" + dateLstMenStr.Substring(0, 4);
            }

            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E != null && priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E != null)
            {
                string lvlService = string.Empty;

                if (priorAuthReq != null &&
                    priorAuthReq.BHTContainter != null &&
                    priorAuthReq.BHTContainter.PatientEventDetails_2000E != null &&
                    priorAuthReq.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E != null &&
                    !string.IsNullOrEmpty(priorAuthReq.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM06_LevelOfServiceCode))
                {
                    lvlService = priorAuthReq.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM06_LevelOfServiceCode;
                }

                bool valueApplied = false;

                if (!string.IsNullOrEmpty(lvlService))
                {
                    foreach (ListItem item in ddlLevelService.Items)
                    {
                        string[] arrlvlService = item.Text.ToLower().Trim().Split('-');

                        if (arrlvlService != null && arrlvlService.Length > 0 && arrlvlService[0].Trim() == lvlService.ToLower())
                        {
                            ddlLevelService.SelectedValue = item.Value.ToString();
                            valueApplied = true;
                            break;
                        }
                    }
                }

                if (!valueApplied)
                {
                    lvlService = priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM06_LevelOfServiceCode;

                    if (!string.IsNullOrEmpty(lvlService))
                    {
                        foreach (ListItem item in ddlLevelService.Items)
                        {
                            string[] arrlvlService = item.Text.ToLower().Trim().Split('-');

                            if (arrlvlService != null && arrlvlService.Length > 0 && arrlvlService[0].Trim() == lvlService.ToLower())
                            {
                                ddlLevelService.SelectedValue = item.Value.ToString();
                                break;
                            }
                        }
                    }
                }
            }


            // Dental or Professional
            if (priorAuthReq.BHTContainter.PatientEventDetails_2000E != null && priorAuthReq.BHTContainter.PatientEventDetails_2000E.EventDate_2000E != null &&
                !string.IsNullOrEmpty(priorAuthReq.BHTContainter.PatientEventDetails_2000E.EventDate_2000E.DTP03_AccidentDate))
            {
                string datetxtBoxStr = priorAuthReq.BHTContainter.PatientEventDetails_2000E.EventDate_2000E.DTP03_AccidentDate;
                TextBox3.Text = datetxtBoxStr.Substring(4, 2) + "/" + datetxtBoxStr.Substring(6, 2) + "/" + datetxtBoxStr.Substring(0, 4);
            }


            string facilityTypeCode = !string.IsNullOrEmpty(priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM041_FacilityRespTypeCode) ?
                    priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM041_FacilityRespTypeCode.ToString() : string.Empty;

            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E != null && priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E != null)
            {

                DataTable dt;


                dt = GetPlaceOfServiceData(facilityTypeCode, default(string), true);
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    StringBuilder display = new StringBuilder();
                    display.Append(Convert.ToString(dt.Rows[0]["PRIOR_AUTH_PLACE_OF_SERVICE_MMIS"]).Trim());
                    display.Append("-");
                    display.Append(Convert.ToString(dt.Rows[0]["PRIOR_AUTH_PLACE_OF_SERVICE_DESC"]).Trim());
                    if (display.Length < 12)
                        txtpalceofservice.Text = display.ToString();
                }
            }

            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.AccidentDate_2000E != null &&
                !string.IsNullOrEmpty(priorAuthResp.BHTContainter.PatientEventDetails_2000E.AccidentDate_2000E.DTP03_AccidentDate))
            {
                string datetxtAccDtService = priorAuthResp.BHTContainter.PatientEventDetails_2000E.AccidentDate_2000E.DTP03_AccidentDate;
                txtAccDtService.Text = datetxtAccDtService.Substring(4, 2) + "/" + datetxtAccDtService.Substring(6, 2) + "/" + datetxtAccDtService.Substring(0, 4);
            }

            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.LastMenstrualPeriodDate_2000E != null &&
                !string.IsNullOrEmpty(priorAuthResp.BHTContainter.PatientEventDetails_2000E.LastMenstrualPeriodDate_2000E.DTP03_AccidentDate))
            {
                string datetxtMenDtInst = priorAuthResp.BHTContainter.PatientEventDetails_2000E.LastMenstrualPeriodDate_2000E.DTP03_AccidentDate;
                txtMenDtInst.Text = datetxtMenDtInst.Substring(4, 2) + "/" + datetxtMenDtInst.Substring(6, 2) + "/" + datetxtMenDtInst.Substring(0, 4);

            }

            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.EstimatedDateOfBirth_2000E != null &&
                !string.IsNullOrEmpty(priorAuthResp.BHTContainter.PatientEventDetails_2000E.EstimatedDateOfBirth_2000E.DTP03_AccidentDate))
            {
                string datetxtEstDOB = priorAuthResp.BHTContainter.PatientEventDetails_2000E.EstimatedDateOfBirth_2000E.DTP03_AccidentDate;
                txtEstDOB.Text = datetxtEstDOB.Substring(4, 2) + "/" + datetxtEstDOB.Substring(6, 2) + "/" + datetxtEstDOB.Substring(0, 4);
            }

            if (priorAuthReq.BHTContainter.PatientEventDetails_2000E != null && priorAuthReq.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E != null)
            {
                string lvlServiceinst = string.Empty;
                bool isValueApplied = false;

                if (priorAuthReq != null &&
                   priorAuthReq.BHTContainter != null &&
                   priorAuthReq.BHTContainter.PatientEventDetails_2000E != null &&
                   priorAuthReq.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E != null &&
                   !string.IsNullOrEmpty(priorAuthReq.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM06_LevelOfServiceCode))
                {
                    lvlServiceinst = priorAuthReq.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM06_LevelOfServiceCode;
                }

                if (!string.IsNullOrEmpty(lvlServiceinst))
                {
                    foreach (ListItem item in ddlLvlServiceInst.Items)
                    {
                        string[] arrlvlService = item.Text.ToLower().Split('-');

                        if (arrlvlService != null && arrlvlService.Length > 0 && arrlvlService[0].Trim() == lvlServiceinst.ToLower())
                        {
                            ddlLvlServiceInst.SelectedValue = item.Value.ToString();
                            isValueApplied = true;
                            break;
                        }
                    }
                }

                if (!isValueApplied)
                {
                    if (priorAuthResp != null &&
                       priorAuthResp.BHTContainter != null &&
                       priorAuthResp.BHTContainter.PatientEventDetails_2000E != null &&
                       priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E != null &&
                       !string.IsNullOrEmpty(priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM06_LevelOfServiceCode))
                    {
                        lvlServiceinst = priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM06_LevelOfServiceCode;
                        if (!string.IsNullOrEmpty(lvlServiceinst))
                        {
                            foreach (ListItem item in ddlLvlServiceInst.Items)
                            {
                                string[] arrlvlService = item.Text.ToLower().Split('-');

                                if (arrlvlService != null && arrlvlService.Length > 0 && arrlvlService[0].Trim() == lvlServiceinst.ToLower())
                                {
                                    ddlLvlServiceInst.SelectedValue = item.Value.ToString();
                                    isValueApplied = true;
                                    break;
                                }
                            }
                        }
                    }

                }
            }

            // Institutional
            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E != null)
                txtfacilityType.Text = priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM041_FacilityRespTypeCode;

            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.InstitutionalClaimCode_2000E != null)
            {
                //ddlAdminType.SelectedValue = priorAuthResp.BHTContainter.PatientEventDetails_2000E.InstitutionalClaimCode_2000E.CL101_AdmissionRespTypeCode;
                var admType = priorAuthResp.BHTContainter.PatientEventDetails_2000E.InstitutionalClaimCode_2000E.CL101_AdmissionRespTypeCode;
                foreach (ListItem item in ddlAdminType.Items)
                {
                    if (admType != null && item.Value.ToLower() == admType.ToLower())
                    {
                        ddlAdminType.SelectedValue = item.Value.ToString();
                        break;
                    }
                }
                GetAdmissionSources();
                var admsrc = priorAuthResp.BHTContainter.PatientEventDetails_2000E.InstitutionalClaimCode_2000E.CL102_AdmissionSourceCode;
                foreach (ListItem item in ddlAdminSrc.Items)
                {
                    if (admsrc != null && item.Value.ToLower() == admsrc.ToLower())
                    {
                        ddlAdminSrc.SelectedValue = item.Value.ToString();
                        hdnAdminSrc.Value = item.Value.ToString();
                        break;
                    }
                }
                GetDischargeStatus();
                var dischargestatus = priorAuthResp.BHTContainter.PatientEventDetails_2000E.InstitutionalClaimCode_2000E.CL103_PatientStatusCode;
                foreach (ListItem item in ddlDischargeStatus.Items)
                {
                    if (dischargestatus != null && item.Value.ToLower() == dischargestatus.ToLower())
                    {
                        ddlDischargeStatus.SelectedValue = item.Value.ToString();
                        break;
                    }
                }
            }


            //string srvAdmissionSource = priorAuthResp.BHTContainter.PatientEventDetails_2000E.InstitutionalClaimCode_2000E.CL102_AdmissionSourceCode;

            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.InstitutionalClaimCode_2000E != null)
            {
                //ddlDischargeStatus.SelectedValue = priorAuthResp.BHTContainter.PatientEventDetails_2000E.InstitutionalClaimCode_2000E.CL103_PatientStatusCode;
                var dschrgStat = priorAuthResp.BHTContainter.PatientEventDetails_2000E.InstitutionalClaimCode_2000E.CL103_PatientStatusCode;

                if (dschrgStat != null)
                {
                    foreach (ListItem item in ddlDischargeStatus.Items)
                    {
                        if (item.Text.ToLower() == dschrgStat.ToLower())
                        {
                            ddlDischargeStatus.SelectedValue = item.Value.ToString();
                            break;
                        }
                    }
                }
            }

            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E != null &&
                priorAuthResp.BHTContainter.PatientEventDetails_2000E.DischargeDate_2000E != null &&
                priorAuthResp.BHTContainter.PatientEventDetails_2000E.DischargeDate_2000E.DTP03_AccidentDate != null)
            {
                string datetxtFDischargeDate = priorAuthResp.BHTContainter.PatientEventDetails_2000E.DischargeDate_2000E.DTP03_AccidentDate;
                txtFDischargeDate.Text = datetxtFDischargeDate.Substring(4, 2) + "/" + datetxtFDischargeDate.Substring(6, 2) + "/" + datetxtFDischargeDate.Substring(0, 4);
            }

            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E != null &&
                priorAuthResp.BHTContainter.PatientEventDetails_2000E.AdmissionDate_2000E != null &&
                priorAuthResp.BHTContainter.PatientEventDetails_2000E.AdmissionDate_2000E.DTP03_AccidentDate != null)
            {
                string datetxtFAdmissionDate = priorAuthResp.BHTContainter.PatientEventDetails_2000E.AdmissionDate_2000E.DTP03_AccidentDate;
                txtFAdmissionDate.Text = datetxtFAdmissionDate.Substring(4, 2) + "/" + datetxtFAdmissionDate.Substring(6, 2) + "/" + datetxtFAdmissionDate.Substring(0, 4);

            }

            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E != null &&
                priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E != null &&
                !string.IsNullOrEmpty(priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM10_DelayReasonCode))
            {
                ddlDlyReason.SelectedValue = priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM10_DelayReasonCode;

                var dlyReason = priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E.UM10_DelayReasonCode;
                if (!string.IsNullOrEmpty(dlyReason))
                {
                    foreach (ListItem item in ddlDlyReason.Items)
                    {
                        if (dlyReason != null && item.Value.ToLower() == dlyReason.ToLower())
                        {
                            ddlDlyReason.SelectedValue = item.Value.ToString();
                            break;
                        }
                    }

                    foreach (ListItem item in ddlDelayedInst.Items)
                    {
                        if (dlyReason != null && item.Value.ToLower() == dlyReason.ToLower())
                        {
                            ddlDelayedInst.SelectedValue = item.Value.ToString();
                            break;
                        }
                    }
                }
            }

            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E != null
                && priorAuthResp.BHTContainter.PatientEventDetails_2000E.PreviousReviewAuthorizationNumber_2000E != null
                && !string.IsNullOrEmpty(priorAuthResp.BHTContainter.PatientEventDetails_2000E.PreviousReviewAuthorizationNumber_2000E.REF02_RequesterSupplementalIdentification))
            {
                txtPANumInst.Text = priorAuthResp.BHTContainter.PatientEventDetails_2000E.PreviousReviewAuthorizationNumber_2000E.REF02_RequesterSupplementalIdentification;
            }

            //OHPNM-11221:Associated PA field automatically displaying the PA number when it is not entered by provider.

            //else if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.AdministrativeReferenceNumber_2000E != null)
            //    txtPANumInst.Text = priorAuthResp.BHTContainter.PatientEventDetails_2000E.AdministrativeReferenceNumber_2000E.REF04_ReferenceIdentifier;

            var hcrObj = new UMRespType();
            if (priorAuthResp != null && priorAuthResp.BHTContainter != null && priorAuthResp.BHTContainter.PatientEventDetails_2000E != null && priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E != null)
            {
                hcrObj = priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReviewInformation_2000E;

            }

            #region Place of Service
            if (hcrObj != null && hcrObj.UM042_FacilityCodeQualifier != null && hcrObj.UM042_FacilityCodeQualifier == "B" && (hcrObj.UM041_FacilityRespTypeCode != null || !string.IsNullOrEmpty(facilityTypeCode)))
            {
                txtpalceofservice.Text = !string.IsNullOrEmpty(hcrObj.UM041_FacilityRespTypeCode) ? hcrObj.UM041_FacilityRespTypeCode : facilityTypeCode;

                DataTable PlaceOfServiceDT = GetPlaceOfServiceData(txtpalceofservice.Text.ToString(), default(string), true);
                if (PlaceOfServiceDT != null && PlaceOfServiceDT.Rows != null && PlaceOfServiceDT.Rows.Count > 0)
                {
                    StringBuilder display = new StringBuilder();
                    display.Append(Convert.ToString(PlaceOfServiceDT.Rows[0]["PRIOR_AUTH_PLACE_OF_SERVICE_MMIS"]).Trim());
                    display.Append("-");
                    display.Append(Convert.ToString(PlaceOfServiceDT.Rows[0]["PRIOR_AUTH_PLACE_OF_SERVICE_DESC"]).Trim());
                    if (display.Length < 12)
                        txtpalceofservice.Text = display.ToString();

                }
            }


            #endregion

            #region Facility Type
            if (hcrObj != null && hcrObj.UM042_FacilityCodeQualifier != null && hcrObj.UM042_FacilityCodeQualifier == "A" && hcrObj.UM041_FacilityRespTypeCode != null)
                txtfacilityType.Text = hcrObj.UM041_FacilityRespTypeCode;

            #endregion
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void LoadContactInformationFromInquireResponse(PriorAuthResponse278RespType priorAuthResp)
    {
        string contNameObj = string.Empty;
        try
        {
            if (priorAuthResp != null &&
                priorAuthResp.BHTContainter != null &&
                priorAuthResp.BHTContainter.RequesterDetails_2000B != null &&
                priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B != null &&
                priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterContactInformation_2010B != null &&
                priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterContactInformation_2010B.PER02_UMOContactName != null)
            {
                contNameObj = priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterContactInformation_2010B.PER02_UMOContactName;
            }

            if (!string.IsNullOrEmpty(contNameObj))
            {
                string[] nameArr = contNameObj.Split(' ');

                if (nameArr != null && nameArr.Length > 0)
                {
                    txtContactLastName.Text = nameArr[0];
                    txtContactName.Text = nameArr.Length > 1 ? nameArr[1] : AppSettings.Get("DefaultContactLastName", "N/A");
                }
            }

            if (priorAuthResp != null
                && priorAuthResp.BHTContainter != null
                && priorAuthResp.BHTContainter.RequesterDetails_2000B != null
                && priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B != null
                && priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterContactInformation_2010B != null
                && priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterContactInformation_2010B.PER05_CommunicationNumberQualifier != null
                && priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterContactInformation_2010B.PER05_CommunicationNumberQualifier.ToLower() == "ex"
                && priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterContactInformation_2010B.PER06_UMOContactCommunicationNumber != null)
            {
                txtExt.Text = priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterContactInformation_2010B.PER06_UMOContactCommunicationNumber;
            }

            if (priorAuthResp != null && priorAuthResp.BHTContainter != null &&
                priorAuthResp.BHTContainter.RequesterDetails_2000B != null &&
                priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B != null &&
                priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterContactInformation_2010B != null &&
                priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterContactInformation_2010B.PER03_CommunicationNumberQualifier != null &&
                priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterContactInformation_2010B.PER03_CommunicationNumberQualifier.ToLower() == "te" &&
                priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterContactInformation_2010B.PER04_UMOContactCommunicationNumber != null)
            {
                string contactNumber = priorAuthResp.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterContactInformation_2010B.PER04_UMOContactCommunicationNumber;
                txtContactNumber.Text = contactNumber;
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void LoadPAInformationFromInquireResponse(PriorAuthResponse278RespType priorAuthResp)
    {
        Dictionary<string, string> statusMap = LoadPAStatusMapping();

        var hcsObj = string.Empty;
        var prevAuth = string.Empty;
        var certExpObj = new DTPRespType();
        var certObj = new DTPRespType();
        var certEffObj = new DTPRespType();


        if (priorAuthResp != null && priorAuthResp.BHTContainter != null && priorAuthResp.BHTContainter.PatientEventDetails_2000E != null)
        {
            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReview_2000E != null)
            {
                if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReview_2000E.HCR01_ActionCode != null)
                {
                    hcsObj = priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReview_2000E.HCR01_ActionCode;
                }

            }
            if (string.IsNullOrEmpty(hcsObj))
            {
                if (priorAuthResp.BHTContainter.ServiceDetails[0].HealthCareServicesReview_2000F != null)
                {
                    if (priorAuthResp.BHTContainter.ServiceDetails[0].HealthCareServicesReview_2000F.HCR01_ActionCode != null)
                    {
                        hcsObj = priorAuthResp.BHTContainter.ServiceDetails[0].HealthCareServicesReview_2000F.HCR01_ActionCode;
                    }
                }

            }
            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReview_2000E != null)
            {
                if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReview_2000E.HCR02_ReviewIdentificationNumber != null)
                {
                    prevAuth = priorAuthResp.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReview_2000E.HCR02_ReviewIdentificationNumber;
                }
            }

            if (string.IsNullOrEmpty(prevAuth) && priorAuthResp.BHTContainter.PatientEventDetails_2000E.AdministrativeReferenceNumber_2000E != null &&
                        priorAuthResp.BHTContainter.PatientEventDetails_2000E.AdministrativeReferenceNumber_2000E.REF02_RequesterSupplementalIdentification != null)
            {
                prevAuth = priorAuthResp.BHTContainter.PatientEventDetails_2000E.AdministrativeReferenceNumber_2000E.REF02_RequesterSupplementalIdentification;
            }

            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.CertificationExpirationDate_2000E != null)
            {
                certExpObj = priorAuthResp.BHTContainter.PatientEventDetails_2000E.CertificationExpirationDate_2000E;
            }
            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.CertificationIssueDate_2000E != null)
            {
                certObj = priorAuthResp.BHTContainter.PatientEventDetails_2000E.CertificationIssueDate_2000E;
            }
            if (priorAuthResp.BHTContainter.PatientEventDetails_2000E.CertificationEffectiveDate_2000E != null)
            {
                certEffObj = priorAuthResp.BHTContainter.PatientEventDetails_2000E.CertificationEffectiveDate_2000E;
            }
        }

        txtstatus2.Text = !string.IsNullOrEmpty(hcsObj) && statusMap.ContainsKey(hcsObj) ? statusMap[hcsObj] : "";
        txtPANumber2.Text = string.IsNullOrEmpty(prevAuth) ? "" : prevAuth;
        txtPACreation2.Text = string.IsNullOrEmpty(certObj.DTP03_AccidentDate) ? "" : certObj.DTP03_AccidentDate.Substring(4, 2) + "/" + certObj.DTP03_AccidentDate.Substring(6, 2) + "/" + certObj.DTP03_AccidentDate.Substring(0, 4);
        txtExpDate2.Text = string.IsNullOrEmpty(certExpObj.DTP03_AccidentDate) ? "" : certExpObj.DTP03_AccidentDate.Substring(4, 2) + "/" + certExpObj.DTP03_AccidentDate.Substring(6, 2) + "/" + certExpObj.DTP03_AccidentDate.Substring(0, 4);
        txtEffDate2.Text = string.IsNullOrEmpty(certEffObj.DTP03_AccidentDate) ? "" : certEffObj.DTP03_AccidentDate.Substring(4, 2) + "/" + certEffObj.DTP03_AccidentDate.Substring(6, 2) + "/" + certEffObj.DTP03_AccidentDate.Substring(0, 4);

        enableButtons();

        //pnlSepMaliciousAttachmentsInfo.Visible = true;
        //pnlMaliciousAttachments.Visible = true;
    }

    private void LoadRecipientInformationFromInquireResponse(PriorAuthResponse278RespType priorAuthResp)
    {
        try
        {
            hdnModified.Value = "true";

            if (priorAuthResp.BHTContainter != null &&
                priorAuthResp.BHTContainter.SubscriberDetails_2000C != null &&
                priorAuthResp.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C != null)
            {
                string recMedicaidNo = string.Empty;
                var nameObj = priorAuthResp.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberName_2010C;
                if (nameObj != null)
                {
                    txtfrstmi2.Text = nameObj.NM104_UMOFirstName;
                    txtMiddleName.Text = nameObj.NM105_UMOMiddleName;
                    txtLastName2.Text = nameObj.NM103_UMOLastOrOrganizationName;
                    recMedicaidNo = nameObj.NM109_UMOIdentifier;
                    txtMedicaidBillingNumber.Text = recMedicaidNo;
                }

                var birthObj = priorAuthResp.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberDemographicInformation_2010C;
                if (birthObj != null)
                {
                    txtBirthDate.Text = birthObj.DMG02_SubscriberBirthDate.Substring(4, 2) + "/" + birthObj.DMG02_SubscriberBirthDate.Substring(6, 2) + "/" + birthObj.DMG02_SubscriberBirthDate.Substring(0, 4);

                    // this.txtBirthDate_TextChanged(txtBirthDate.Text, new EventArgs());

                    string genderCode = birthObj.DMG03_SubscriberGenderCode;
                    txtGender2.Text = genderCode == "F" ? "Female" : genderCode == "M" ? "Male" : "Unknown";
                }

                var addressObj = priorAuthResp.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberAddress_2010C;
                if (addressObj != null)
                {
                    txtAddress1.Text = addressObj.N301_SubscriberAddressLine;
                    lblAddress2.Text = addressObj.N302_SubscriberAddressLine;
                }

                var zipObj = priorAuthResp.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberCityStateZipCode_2010C;
                if (zipObj != null)
                {
                    txtCity.Text = zipObj.N401_SubscriberCityName;
                    txtState.Text = zipObj.N402_SubscriberStateCode;
                    txtZipCode.Text = zipObj.N403_SubscriberPostalZoneOrZipCode;
                }
            }

            if (priorAuthResp != null &&
                priorAuthResp.BHTContainter != null &&
                priorAuthResp.BHTContainter.PatientEventDetails_2000E != null &&
                priorAuthResp.BHTContainter.PatientEventDetails_2000E.PatientEventTrackingNumber_2000E != null)
            {
                var trckObj = priorAuthResp.BHTContainter.PatientEventDetails_2000E.PatientEventTrackingNumber_2000E[0];
                txtPatientTrckNum.Text = trckObj != null ? trckObj.TRN02_PatientEventTraceNumber : "";
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void LoadProviderInformation(string medicaidNumber)
    {
        try
        {
            DataSet ds = spa.SelectProviderByGRPMedicaidID(medicaidNumber);
            DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
            this.DataList = dtMisc;
            if (Helper.HasRows(dtMisc))
            {
                DataRow dr = dtMisc.Rows[0];
                this.RegIdTxt.Value = Helper.GetString("REG_ID", dr);
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void btnSearchFacilityType_Click(object sender, EventArgs e)
    {
        try
        {
            this.lblFacilitySResult.Visible = true;
            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SessionVarRetriever.DashBoardTableId = 0;
            RefreshFacilityData();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void lnkMedicaidId_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;

        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        //txtNPI1.Text
        hdnNPI.Value = ((System.Web.UI.WebControls.LinkButton)grdrow.FindControl("lnkNPI")).Text;
        //txtMedicaidID.Text 
        hdnMedicaidId.Value = cmdArgument;
        //txtProviderName1.Text 
        hdnProviderName.Value = grdrow.Cells[3].Text + " " + grdrow.Cells[2].Text;

        //  gvSubmitClaimSearchPage.DataSource = null;
        txtNPI.Text = ((System.Web.UI.WebControls.LinkButton)grdrow.FindControl("lnkNPI")).Text;
        txtProMedicaidID.Text = hdnMedicaidId.Value;
        txtBusinessLastName.Text = grdrow.Cells[3].Text;
        txtFirstName.Text = grdrow.Cells[2].Text;
        //gvPriorAuthSearchPage.DataSource = null;
        //gvPriorAuthSearchPage.DataBind();
        errProviderNPI.Visible = false;
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "modal", "closemodal()", true);
    }

    protected void lnkMedicaidIdORD_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;

        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        //txtNPI1.Text
        hdnNPI.Value = ((System.Web.UI.WebControls.LinkButton)grdrow.FindControl("lnkNPIORD")).Text;
        //txtMedicaidID.Text 
        hdnMedicaidId.Value = cmdArgument;
        //txtProviderName1.Text 
        hdnProviderName.Value = grdrow.Cells[3].Text + " " + grdrow.Cells[2].Text;

        //  gvSubmitClaimSearchPage.DataSource = null;
        txtNPIORD.Text = ((System.Web.UI.WebControls.LinkButton)grdrow.FindControl("lnkNPIORD")).Text;
        txtProMedicaidIDORD.Text = hdnMedicaidId.Value;
        txtBusinessLastNameORD.Text = grdrow.Cells[3].Text;
        txtFirstNameORD.Text = grdrow.Cells[2].Text;
        //gvPriorAuthSearchPageORD.DataSource = null;
        //gvPriorAuthSearchPageORD.DataBind();
        lblErrOrderProvidingNPI.Visible = false;
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "modal", "closemodal()", true);
    }

    protected void lnkFacilityTypeCode_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        string cmdArgument = btn.CommandArgument;
        txtfacilityType.Text = (btn.Text + " - " + cmdArgument);
        txtPlaceOfServiceName.Text = cmdArgument;
        hdnfacilityType.Value = (btn.Text + " - " + cmdArgument);
        txtFacilityTypeCode.Text = grdrow.Cells[0].Text;
        txtFacilityTypeDescription.Text = grdrow.Cells[1].Text;
        //gvPriorAuthFacilitySearchPage.DataSource = null;
        //gvPriorAuthFacilitySearchPage.DataBind();
        this.lblFacilitySResult.Visible = false;
        // this.gvPriorAuthFacilitySearchPage.Visible = false;
        if (rblClaimType.SelectedItem.Text == "Institutional" && ddlAssignment.SelectedValue == "59")
        {
            if (btn.Text == "011")
            {
                ddlServiceTypeCode.Items.FindByText("ICD 10 Procedure").Selected = true;
            }
            else if (btn.Text == "013")
            {
                ddlServiceTypeCode.Items.FindByText("HCPCS").Selected = true;
            }
        }
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "modal", "closeFacModal()", true);
    }

    protected void lnkFacilityTypeSrch_Click(object sender, EventArgs e)
    {
        //gvPriorAuthFacilitySearchPage.DataSource = null;
        //gvPriorAuthFacilitySearchPage.DataBind();
        txtFacilityTypeCode.Text = "";
        txtFacilityTypeDescription.Text = "";
        this.lblFacilitySResult.Visible = false;
        // this.gvPriorAuthFacilitySearchPage.Visible = false;
        ScriptManager.RegisterStartupScript(this.Page, GetType(), "modelBox", "$('#FacilityTypeSearchModal').modal('show');", true);
    }

    private string PAStatus(string medicalNumber)
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }

        DataSet dataSet = _spa.GetStatusByDetails(this.ProviderNPI, medicalNumber);
        DataTable dt = dataSet.Tables[0];
        if (Helper.HasRows(dt))
        {
            DataRow dr = dt.Rows[0];
            return Helper.GetString("REGISTRATION_STATUS_TYPE", dr);
        }

        return string.Empty;
    }
    private void CancelPopup()
    {

        this.mpe.Hide();
        this.mpedoc.Hide();
        //this.doc.Hide();

    }

    private void UpdateNoteData()
    {

        mpe.Hide();


    }
    private void UpdateDocumentbymailData()
    {
        // doc.Hide();
    }
    /// <summary>
    /// This Method will get DiagnosisCodeType
    /// </summary>
    ///
    private void GetDiagnosisCodeType()
    {

        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        ddlDiagnosisCodeType.Items.Clear();
        try
        {
            DataSet dataSet = _spa.GetDiagnosisCodeType();
            DataTable dt = dataSet.Tables[0];
            //PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC
            ViewState[DIAGNOSIS_CODE_TYPE] = dt;
            Helper.LoadList(ddlDiagnosisCodeType, dt, "PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC", "PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID", false);
            ddlDiagnosisCodeType.Items.Insert(0, new ListItem("--- Please select ---", String.Empty));
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }
    public const string DIAGNOSIS_CODE_TYPE = "diagnosis_code_type";
    private void BindClaimDiagnosis(string code, string icdVersion, string diagnosisDes)
    {
        try
        {
            var result = LookupTableController.GetICDDiagnosis(code, icdVersion, diagnosisDes);
        }
        catch (Exception ex)
        {
            throw new Exception("Error at BindClaimDiagnosis method", ex);
        }
    }
    protected void btnDiagSearch_Click(object sender, EventArgs e)
    {
        try
        {
            this.gvClaimDiagnosisSearch.CurrentPageIndex = 0;
            BindClaimDiagnosis(txtDiagnosisCodeSearch.Text.Trim(), lblICDVersion.Text, txtDiagnosisCodeDescSearch.Text.Trim());
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtNPI.Text.ToString()) && txtNPI.Text.Length < 10)
            {
                lblErrorSearch.Text = "NPI should be 10-digit number.";
                //gvPriorAuthSearchPage.Visible = false;
                lblSResult.Visible = false;
                return;
            }

            if (!string.IsNullOrEmpty(txtProMedicaidID.Text.ToString()) && txtProMedicaidID.Text.Length < 7)
            {
                lblErrorSearch.Text = "Medicaid ID should be 7-digit number.";
                lblSResult.Visible = false;
                //gvPriorAuthSearchPage.Visible = false;
                return;
            }

            bool isNPIEmpty = (string.IsNullOrEmpty(txtNPI.Text.ToString()));
            bool isMedicaidIdEmpty = (string.IsNullOrEmpty(txtProMedicaidID.Text.ToString()));
            bool IsNameEmpty = ((string.IsNullOrEmpty(txtFirstName.Text.ToString())) && (string.IsNullOrEmpty(txtBusinessLastName.Text.ToString())));
            if (isNPIEmpty && isMedicaidIdEmpty && IsNameEmpty)
            {
                lblErrorSearch.Text = "NPI, Medicaid ID, Business/Last Name or First Name is required";
                // gvPriorAuthSearchPage.Visible = false;
                lblSResult.Visible = false;
                return;
            }
            // gvPriorAuthSearchPage.Visible = true;
            //Clear these out prior to doing the Search
            lblErrorSearch.Text = string.Empty;
            this.lblSResult.Visible = true;
            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SessionVarRetriever.DashBoardTableId = 0;
            //  this.gvPriorAuthSearchPage.CurrentPageIndex = 0;
            RefreshData();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void btnSearchORD_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtNPIORD.Text.ToString()) && txtNPIORD.Text.Length < 10)
            {
                lblErrorSearchORD.Text = "NPI should be 10-digit number.";
                //  gvPriorAuthSearchPageORD.Visible = false;
                lblSResultORD.Visible = false;
                return;
            }

            if (!string.IsNullOrEmpty(txtProMedicaidIDORD.Text.ToString()) && txtProMedicaidIDORD.Text.Length < 7)
            {
                lblErrorSearchORD.Text = "Medicaid ID should be 7-digit number.";
                lblSResultORD.Visible = false;
                //gvPriorAuthSearchPageORD.Visible = false;
                return;
            }

            bool isNPIEmpty = (string.IsNullOrEmpty(txtNPIORD.Text.ToString()));
            bool isMedicaidIdEmpty = (string.IsNullOrEmpty(txtProMedicaidIDORD.Text.ToString()));
            bool IsNameEmpty = ((string.IsNullOrEmpty(txtFirstNameORD.Text.ToString())) && (string.IsNullOrEmpty(txtBusinessLastNameORD.Text.ToString())));
            if (isNPIEmpty && isMedicaidIdEmpty && IsNameEmpty)
            {
                lblErrorSearchORD.Text = "NPI, Medicaid ID, Business/Last Name or First Name is required";
                //    gvPriorAuthSearchPageORD.Visible = false;
                lblSResultORD.Visible = false;
                return;
            }
            //  gvPriorAuthSearchPageORD.Visible = true;
            //Clear these out prior to doing the Search
            lblErrorSearchORD.Text = string.Empty;
            this.lblSResultORD.Visible = true;
            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SessionVarRetriever.DashBoardTableId = 0;
            //  this.gvPriorAuthSearchPageORD.CurrentPageIndex = 0;
            RefreshDataORD();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void lnkNPI_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;

        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        //txtNPI1.Text
        hdnNPI.Value = cmdArgument;
        //txtMedicaidID.Text 
        hdnMedicaidId.Value = ((System.Web.UI.WebControls.LinkButton)grdrow.FindControl("lnkMedicaidId")).Text;
        //txtProviderName1.Text 
        hdnProviderName.Value = grdrow.Cells[3].Text + " " + grdrow.Cells[2].Text;

        //  gvSubmitClaimSearchPage.DataSource = null;
        txtNPI.Text = hdnNPI.Value;
        txtProMedicaidID.Text = ((System.Web.UI.WebControls.LinkButton)grdrow.FindControl("lnkMedicaidId")).Text;
        txtBusinessLastName.Text = grdrow.Cells[3].Text;
        txtFirstName.Text = grdrow.Cells[2].Text;
        errProviderNPI.Text = "";
        lblSvcProviderFName.Text = grdrow.Cells[2].Text;
        lblSvcProviderLName.Text = grdrow.Cells[3].Text;
        txtMedicaidID.Text = ((System.Web.UI.WebControls.LinkButton)grdrow.FindControl("lnkMedicaidId")).Text;
        //gvPriorAuthSearchPage.DataSource = null;
        //gvPriorAuthSearchPage.DataBind();
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "modal", "closemodal()", true);
        try
        {
            checkServiceActiveEnrollSpan(txtMedicaidID.Text.Trim());
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void lnkNPIORD_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;

        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        //txtNPI1.Text
        hdnNPI.Value = cmdArgument;
        //txtMedicaidID.Text 
        hdnMedicaidId.Value = ((System.Web.UI.WebControls.LinkButton)grdrow.FindControl("lnkMedicaidIdORD")).Text;
        //txtProviderName1.Text 
        hdnProviderName.Value = grdrow.Cells[3].Text + " " + grdrow.Cells[2].Text;

        //  gvSubmitClaimSearchPage.DataSource = null;
        txtNPIORD.Text = hdnNPI.Value;
        txtProMedicaidIDORD.Text = ((System.Web.UI.WebControls.LinkButton)grdrow.FindControl("lnkMedicaidIdORD")).Text;
        txtBusinessLastNameORD.Text = grdrow.Cells[3].Text;
        txtFirstNameORD.Text = grdrow.Cells[2].Text;
        lblErrOrderProvidingNPI.Text = "";
        lblOrdProviderFName.Text = grdrow.Cells[2].Text;
        lblOrdProviderLName.Text = grdrow.Cells[3].Text;
        txtOMID.Text = ((System.Web.UI.WebControls.LinkButton)grdrow.FindControl("lnkMedicaidIdORD")).Text;
        //gvPriorAuthSearchPageORD.DataSource = null;
        //gvPriorAuthSearchPageORD.DataBind();
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "modal", "closemodal()", true);
        try
        {
            checkOrderingActiveEnrollSpan(txtOMID.Text.Trim());
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private DataTable GetData(string npi, string medicaidid, string lastName, string firstName)
    {
        DataTable dt = null;
        try
        {
            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                var ds = psc.SearchProviderNPI(npi, medicaidid, lastName, firstName);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                }
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
        return dt;
    }

    private DataTable GetFacilityTypeData(string facilityCode, string facilityDesc, bool textChangevent = false)
    {
        DataTable dt = null;
        try
        {
            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                if (textChangevent == true)
                {
                    var ds = psc.GetFacilityTypeByFacilityCode(facilityCode, facilityDesc);
                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                    }
                }
                else
                {
                    var ds = psc.GetFacilityTypes(facilityCode, facilityDesc);
                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetFaccilityTypeData");
            return dt;
        }
        return dt;
    }


    protected void gvPriorAuthFacilitySearchPage_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            RefreshFacilityData();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    public void RefreshFacilityData()
    {
        string facilityCode = txtFacilityTypeCode.Text;
        string facilityDesc = txtFacilityTypeDescription.Text;
        DataTable dt = GetFacilityTypeData(facilityCode, facilityDesc);
        // this.gvPriorAuthFacilitySearchPage.Visible = true;
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Rows.Count <= 0)
            {
                // gvPriorAuthFacilitySearchPage.DataSource = new DataSet();
                return;
            }

            dt = dt.Select().CopyToDataTable();
            //gvPriorAuthFacilitySearchPage.DataSource = dt;
            //gvPriorAuthFacilitySearchPage.DataBind();
        }
        else
        {
            //gvPriorAuthFacilitySearchPage.DataSource = dt;
            //gvPriorAuthFacilitySearchPage.DataBind();
        }
    }


    protected void gvPriorAuthSearchPage_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            RefreshData();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void gvNPICodeSearch_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            RefreshData();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void gvPriorAuthSearchPageORD_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            RefreshDataORD();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void gvPriorAuthSearchPageORD_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            RefreshDataORD();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    public void RefreshData()
    {
        string npi = txtNPI.Text.Trim();
        string medicadeId = txtProMedicaidID.Text.Trim();
        string firstName = txtFirstName.Text.Trim();
        string lastname = txtBusinessLastName.Text.Trim();
        try
        {
            DataTable dt = GetData(npi, medicadeId, lastname, firstName);

            if (dt != null)
            {
                if (dt.Rows.Count <= 0)
                {
                    //return;
                    //gvPriorAuthSearchPage.DataSource = dt;
                    //gvPriorAuthSearchPage.DataBind();
                }
                else
                {
                    DataRow[] drow = dt.Select("NPI <> ''");
                    if (drow != null && drow.Length > 0)
                    {
                        dt = dt.Select("NPI <> ''").CopyToDataTable();
                    }

                    //gvPriorAuthSearchPage.DataSource = dt;
                    //gvPriorAuthSearchPage.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    public void RefreshDataORD()
    {
        string npi = txtNPIORD.Text.Trim();
        string medicadeId = txtProMedicaidIDORD.Text.Trim();
        string firstName = txtFirstNameORD.Text.Trim();
        string lastname = txtBusinessLastNameORD.Text.Trim();
        DataTable dt = GetData(npi, medicadeId, lastname, firstName);

        if (dt != null)
        {
            if (dt.Rows.Count <= 0)
            {
                //return;
                //gvPriorAuthSearchPageORD.DataSource = dt;
                //gvPriorAuthSearchPageORD.DataBind();
            }
            else
            {
                DataRow[] drow = dt.Select("NPI <> ''");
                if (drow != null && drow.Length > 0)
                {
                    dt = dt.Select("NPI <> ''").CopyToDataTable();
                }

                //gvPriorAuthSearchPageORD.DataSource = dt;
                //gvPriorAuthSearchPageORD.DataBind();
            }
        }

    }

    protected void txtProviderNPI_TextChangedNPI(object sender, EventArgs e)
    {
        hdnModified.Value = "true";
        string medicaidId = txtMedicaidID.Text.Trim();
        errProviderNPI.Text = string.Empty;

        try
        {
            if (!string.IsNullOrEmpty(txtSPNPI.Text.Trim()) && txtSPNPI.Text.Length == 10)
            {
                var isValid = ValidProviderNPI(txtSPNPI.Text.Trim());
                if (!isValid)
                {
                    //AddValidationErrorMessage("Invalid NPI");
                    NPITextChanged(txtSPNPI, txtMedicaidID, null, lblSvcProviderFName, lblSvcProviderLName, null, errProviderNPI);
                    txtMedicaidID.Text = string.Empty;
                    lblSvcProviderFName.Text = string.Empty;
                    lblSvcProviderLName.Text = string.Empty;
                    return;
                }

                NPITextChanged(txtSPNPI, txtMedicaidID, null, lblSvcProviderFName, lblSvcProviderLName, null, errProviderNPI);

                if (!string.IsNullOrEmpty(txtMedicaidID.Text.Trim()))
                {
                    checkServiceActiveEnrollSpan(txtMedicaidID.Text.Trim());
                }
            }
            else
            {
                txtMedicaidID.Text = "";
                lblSvcProviderFName.Text = "";
                lblSvcProviderLName.Text = "";
                errProviderNPI.Text = "10 - digit number is required";
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void txtorderingprovidernpi_TextChangedNPI(object sender, EventArgs e)
    {
        hdnModified.Value = "true";
        NPITextChanged(txtorderingprovidernpi, txtOMID, null, lblOrdProviderFName, lblOrdProviderLName, null, lblErrOrderProvidingNPI);

        if (string.IsNullOrEmpty(txtorderingprovidernpi.Text) || string.IsNullOrWhiteSpace(txtorderingprovidernpi.Text))
        {
            txtOMID.Text = string.Empty;
            lblOrdProviderFName.Text = string.Empty;
            lblOrdProviderLName.Text = string.Empty;
        }

        //string medicaidId = txtMedicaidID.Text.Trim();
        //if (!string.IsNullOrEmpty(medicaidId))
        //{
        //    GetActiveSpecialities(medicaidId);
        //}
    }

    public void checkServiceActiveEnrollSpan(string medicaidId)
    {
        string regID = string.Empty;
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            DataSet dataSet = _spa.GetServiceActiveEnrollSpan(medicaidId);
            DataTable dt = dataSet.Tables[0];

            if (Helper.HasRows(dt))
            {
                DataRow dr = dt.Rows[0];
                regID = Helper.GetString("REG_ID", dr);
            }
            else
            {
                AddValidationErrorMessage("*Service Provider no longer affiliated with this Agency.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at CheckServiceActiveEnrollSpan method", ex);
        }
    }

    public void checkOrderingActiveEnrollSpan(string medicaidId)
    {
        string regID = string.Empty;
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            DataSet dataSet = _spa.GetServiceActiveEnrollSpan(medicaidId);
            DataTable dt = dataSet.Tables[0];

            if (Helper.HasRows(dt))
            {
                DataRow dr = dt.Rows[0];
                regID = Helper.GetString("REG_ID", dr);
            }
            else
            {
                AddValidationErrorMessage("*Ordering Provider no longer affiliated with this Agency.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at CheckOrderingActiveEnrollSpan method", ex);
        }
    }

    public void GetActiveSpecialities(string medicaidId)
    {
        string regID = string.Empty;
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            DataSet dataSet = _spa.GetActiveSpecialities(medicaidId);
            DataTable dt = dataSet.Tables[0];

            if (Helper.HasRows(dt))
            {
                DataRow dr = dt.Rows[0];
                regID = Helper.GetString("REG_ID", dr);
            }
            else
            {
                AddValidationErrorMessage("*Service Provider no longer affiliated with this Agency.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetActiveSpecialities method", ex);
        }
    }

    public bool ValidProviderNPI(string npi)
    {
        try
        {
            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                return psc.VerifyProviderNPI(Convert.ToInt64(npi));
            }
        }
        catch (Exception)
        {
            return false;
        }
    }
    protected void NPITextChanged(TextBox _txtNPI, Label _txtMedicaidId, TextBox _txtproviderName, Label _txtFirstName, Label _txtLastName, TextBox _txtMi, Label errorLabel)
    {
        errorLabel.Text = "";
        if (!string.IsNullOrEmpty(_txtNPI.Text))
        {
            if (_txtNPI.Text.Trim().Length == 10)
            {
                var isValid = ValidProviderNPI(_txtNPI.Text.Trim());
                if (!isValid)
                {
                    if (errorLabel != null)
                    {
                        errorLabel.Text = "NPI not found in the system";
                        errorLabel.Visible = true;
                    }
                    if (_txtMedicaidId != null)
                        _txtMedicaidId.Text = string.Empty;
                    if (_txtproviderName != null)
                        _txtproviderName.Text = string.Empty;
                    if (_txtFirstName != null)
                        _txtFirstName.Text = default(string);
                    if (_txtLastName != null)
                        _txtLastName.Text = default(string);

                }
                else
                {
                    DataTable dt = GetData(_txtNPI.Text.Trim(), "", "", "");
                    if (dt != null)
                    {
                        if (dt.Rows.Count <= 0)
                        {
                            if (errProviderNPI != null)
                                errProviderNPI.Text = "No data found in the system";
                            if (_txtMedicaidId != null)
                                _txtMedicaidId.Text = string.Empty;
                            if (_txtproviderName != null)
                                _txtproviderName.Text = string.Empty;
                            if (_txtFirstName != null)
                                _txtFirstName.Text = default(string);
                            if (_txtLastName != null)
                                _txtLastName.Text = default(string);
                            return;
                        }
                        dt = dt.Select("NPI <> ''").CopyToDataTable();
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows.Count > 1)
                            {
                                if (_txtNPI.ID == "txtorderingprovidernpi")
                                {
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "modal", "invokeModalPopUp('btnSearch1');", true);
                                }
                                else
                                {
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "modal", "invokeModalPopUp('btnSearch6');", true);
                                }
                                txtNPI.Text = _txtNPI.Text;
                                //gvPriorAuthSearchPage.DataSource = dt;
                                //gvPriorAuthSearchPage.DataBind();
                                return;
                            }
                            DataRow row = dt.Rows[0];
                            errProviderNPI.Visible = false;
                            if (_txtMedicaidId != null)
                            {
                                _txtMedicaidId.Text = row["MEDICAID_ID"].ToString();
                            }

                            if (_txtproviderName != null)
                            {
                                _txtproviderName.Text = row["FIRST_NAME"].ToString() + " " + row["LAST_OR_BUSINESS_NAME"].ToString();
                            }

                            if (_txtFirstName != null)
                            {
                                _txtFirstName.Text = row["FIRST_NAME"].ToString();
                            }
                            if (_txtLastName != null)
                            {
                                _txtLastName.Text = row["LAST_OR_BUSINESS_NAME"].ToString();
                            }

                        }

                    }
                }
            }
            else
            {
                errorLabel.Text = "10-digit number is required";
                if (_txtMedicaidId != null)
                {
                    _txtMedicaidId.Text = string.Empty;
                }
                if (_txtproviderName != null)
                {
                    _txtproviderName.Text = string.Empty;
                }
                if (_txtFirstName != null)
                    _txtFirstName.Text = default(string);
                if (_txtLastName != null)
                    _txtLastName.Text = default(string);
            }
        }
        else
        {
            if (_txtMedicaidId != null)
            {
                _txtMedicaidId.Text = string.Empty;
            }
            if (_txtproviderName != null)
            {
                _txtproviderName.Text = string.Empty;
            }
            if (_txtFirstName != null)
            {
                _txtFirstName.Text = string.Empty;
            }
            if (_txtLastName != null)
            {
                _txtLastName.Text = string.Empty;
            }
        }
    }
    protected void lnkICD10Diag_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;
        string commandName = btn.CommandName;

        if (!string.IsNullOrEmpty(cmdArgument))
        {
            txtLnDiagnosisCode.Text = cmdArgument.TrimEnd();
            lblIcdVersionError.Text = "";
        }
        txtDiagnosisCodeDescription.Text = commandName;
        mpeDiagnosisSearch1.Hide();
    }

    protected void btnPADiagDelete_Click(object sender, EventArgs e)
    {

    }
    protected void btnPADiagnosis_Click(object sender, EventArgs e)
    {
        //int index = NumControls;
        //CreateControls((index + 1).ToString());
        //NumControls++;
    }

    /// <summary>
    /// This Method will get assignements 
    /// </summary>
    private void GetAssignments()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            ddlAssignment.Items.Clear();
            DataSet dataSet = string.IsNullOrEmpty(rblClaimType.SelectedItem.Value) ? null : _spa.GetAssignementByAuth(rblClaimType.SelectedItem.Value);
            if (dataSet == null) return;
            DataTable dt = dataSet.Tables[0];
            Helper.LoadList(ddlAssignment, dt, "PRIOR_AUTH_Assignment_Type_DESC", "PRIOR_AUTH_Assignment_Type_MMIS", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetAssignments method", ex);
        }
    }

    private void GetInstitutionalAssignments()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            ddlAssignment.Items.Clear();
            DataSet dataSet = _spa.GetAssignementByAuth("Institutional");
            if (dataSet == null) return;
            DataTable dt = dataSet.Tables[0];
            Helper.LoadList(ddlAssignment, dt, "PRIOR_AUTH_Assignment_Type_DESC", "PRIOR_AUTH_Assignment_Type_MMIS", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetInstitutionalAssignments method", ex);
        }
    }

    private void GetDentalAssignments()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }

        ddlAssignment.Items.Clear();
        try
        {
            DataSet dataSet = _spa.GetAssignementByAuth("dental");
            if (dataSet == null) return;
            DataTable dt = dataSet.Tables[0];
            Helper.LoadList(ddlAssignment, dt, "PRIOR_AUTH_Assignment_Type_DESC", "PRIOR_AUTH_Assignment_Type_MMIS", true);
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void GetProfessionalAssignments()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            ddlAssignment.Items.Clear();
            DataSet dataSet = _spa.GetAssignementByAuth("Professional");
            if (dataSet == null) return;
            DataTable dt = dataSet.Tables[0];
            Helper.LoadList(ddlAssignment, dt, "PRIOR_AUTH_Assignment_Type_DESC", "PRIOR_AUTH_Assignment_Type_MMIS", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetProfessionalAssignments method", ex);
        }
    }

    private void GetDischargeStatus()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        ddlDischargeStatus.Items.Clear();
        try
        {
            DataSet dataSet = _spa.GetDischargeStatus();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable dt = dataSet.Tables[0];
                Helper.LoadList(ddlDischargeStatus, dt, "PRIOR_AUTH_SUBMIT_CLAIM_DISCHARGESTATUS_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_DISCHARGESTATUS_CODE", true);
            }
            else
            {
                string logHeader = "GetDischargeStatus DDL - Submit PA";
                string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                logMsg += "dataset is empty";

                //Logging log = new Logging(new Guid(CON.WebPageLogGuid.PriorAuthWebPageLog), logMsg);
                //log.CreateLogEntry(logHeader, Logging.LogPriority.Error);
                CreateAndReturnLogThreadNumber(new Exception(), "SubmitPA-GetDischargeStatus");
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    /// <summary>
    /// This Method will get authoriazation 
    /// </summary>
    private void GetDestinationPayer()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        ddlAuthorization.Items.Clear();
        try
        {
            DataSet dataSet = _spa.GetDestinationPayer();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable dt = dataSet.Tables[0];
                Session["DestinationPayer"] = dt;
                Helper.LoadList(ddlAuthorization, dt, "PRIOR_AUTH_DESTINATION_PAYER_DESC", "PRIOR_AUTH_DESTINATION_PAYER_ID", true);
            }
            else
            {
                string logHeader = "GetDestinationPayer DDL - Submit PA";
                string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                logMsg += "dataset is empty";
                CreateAndReturnLogThreadNumber(new Exception(), "SubmitPA-GetDestinationPayer");
                //Logging log = new Logging(new Guid(CON.WebPageLogGuid.PriorAuthWebPageLog), logMsg);
                //log.CreateLogEntry(logHeader, Logging.LogPriority.Error);
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void GetServicingProviderInfo()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
            var dataTable = ds.Tables["AuthorizationInfo"];

            //gvServicingProviderInfo.DataSource = dataTable;
            //gvServicingProviderInfo.DataBind();
            //gvPriorAuthSearchDiagnosis.DataSource = dataTable;
            //gvPriorAuthSearchDiagnosis.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetServicingProviderInfo method", ex);
        }
    }

    private void GetDiagnosis()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        try
        {
            var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
            var dataTable = ds.Tables["AuthorizationDiagnosis"];

            //gvDiagnosis.DataSource = dataTable;
            //gvDiagnosis.DataBind();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void GetDiagnoisServiceDetails()
    {
        string logHeader = string.Format("SubmitPriorAuthorization -> GetDiagnoisServiceDetails");
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            DataSet ds = LoadDiagnosisDataToSession();

            if (ds != null && ds.Tables.Count > 0)
            {
                var dataTable = ds.Tables[0];

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows.Count >= 12)
                    {
                        btnDiagnosisAdd.Visible = false;
                    }
                    else
                    {
                        btnDiagnosisAdd.Visible = true;
                    }

                    // gvDiagnosis.Visible = true;

                    DataTable dtCopy = dataTable.Copy();

                    if (dtCopy.Columns.IndexOf("RowState") > 0)
                    {
                        DataRow[] dtRows = dtCopy.Select("RowState='delete'");
                        if (dtRows != null && dtRows.Count() > 0)
                        {
                            foreach (DataRow dr in dtRows)
                            {
                                dtCopy.Rows.Remove(dr);
                            }
                        }
                    }


                    dtCopy.DefaultView.Sort = "PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID ASC";

                    //gvDiagnosis.DataSource = dtCopy;
                    //gvDiagnosis.DataBind();
                    // lblDiagnosisNoData.Visible = false;
                }
                else
                {
                    // lblDiagnosisNoData.Visible = true;
                    //gvDiagnosis.Visible = false;
                    btnDiagnosisAdd.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(new Exception(), "SubmitPA-GetDiagnosisServiceDetails");
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private string returnPATypeID()
    {
        string ClaimType = rblClaimType.SelectedValue;
        string patype;
        if (ClaimType.Equals("dental"))
        {
            patype = "0";
        }
        else if (ClaimType.Equals("Professional"))
        {
            patype = "2";
        }
        else
        {
            patype = "1";
        }
        return patype;
    }

    private DataSet LoadDiagnosisDataToSession()
    {
        DataSet ds = new DataSet();
        try
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms.Add("MedicaidID", MedicaidNumber);
            parms.Add("LINK_SECTIONS", SaveCodeLNK);
            ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_DIAGNOSIS", parms);
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
        return ds;
    }

    private void GetServiceDetails()
    {
        string logHeader = string.Format("SubmitPriorAuthorization -> GetServiceDetails");
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            DataSet ds = null; DataTable dataTable = null;
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms.Add("LINK_SECTIONS", SaveCodeLNK);
            ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
            dataTable = ds.Tables[0];

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (dataTable.Columns.IndexOf("RowState") < 0)
                {
                    DataColumn dcRowState = new DataColumn("RowState", typeof(string));
                    dataTable.Columns.Add(dcRowState);
                }

                dataTable = ds.Tables[0].Copy();
                DataRow[] dtRows = dataTable.Select("RowState='" + ROWSTATE_DELETE + "'");
                foreach (DataRow dr in dtRows)
                {
                    dataTable.Rows.Remove(dr);
                }
                //gvServiceDetail.DataSource = dataTable;
                //gvServiceDetail.DataBind();
                //gvServiceDetail.Visible = true;
                // lblservDetailsNoData.Visible = false;

                if (dataTable.Rows.Count == 0)
                {
                    // gvServiceDetail.Visible = false;
                    // lblservDetailsNoData.Visible = true;
                    btnServiceDetailAdd1.Visible = true;
                }
            }
            else
            {
                // gvServiceDetail.Visible = false;
                //  lblservDetailsNoData.Visible = true;
            }

            if (hdnPAInquiryTrn.Value.Equals("PAInquiry") && dataTable.Rows.Count != 0)
            {
                // gvServiceDetail.Visible = true;
                //  lblservDetailsNoData.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetServiceDetails method", ex);
        }
    }
    private void GetDentalServiceDetails()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        DataSet ds = null;
        DataTable dataTable = null;
        try
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms.Add("LINK_SECTIONS", SaveCodeLNK);
            ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
            dataTable = ds.Tables[0];

            if (dataTable.Columns.IndexOf("RowState") < 0)
            {
                DataColumn dcRowState = new DataColumn("RowState", typeof(string));
                dataTable.Columns.Add(dcRowState);
            }


            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataTable dtCopy = dataTable.Copy();
                DataRow[] dtRows = dtCopy.Select("RowState='" + ROWSTATE_DELETE + "'");
                foreach (DataRow dr in dtRows)
                {
                    dtCopy.Rows.Remove(dr);
                }

                //gvServiceDetailDental.DataSource = dtCopy;
                //gvServiceDetailDental.DataBind();
                //gvServiceDetailDental.Visible = true;
                //lblDentalservDetailsNoData.Visible = false;
            }
            else
            {
                //gvServiceDetailDental.Visible = false;
                //lblDentalservDetailsNoData.Visible = true;
                btnDentalServiceDetailAdd1.Visible = true;
            }

            if (hdnPAInquiryTrn.Value.Equals("PAInquiry") && dataTable.Rows.Count != 0)
            {
                //gvServiceDetailDental.Visible = true;
                //lblDentalservDetailsNoData.Visible = false;
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }
    private void GetORDERPROVIDERINFOSearch()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
            var dataTable = ds.Tables["AuthorizationInfo"];

        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetOrderProviderInfo method", ex);
        }
    }

    private void GetCertServiceDetails()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            //DataSet dataSet = _spa.GetPrioHospitalData("SERVICE_DETAIL", PrioHospitalId);
            //DataTable dt = dataSet.Tables[0];
            var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
            if (ds != null && ds.Tables.Count > 0)
            {
                var dataTable = ds.Tables["AuthorizationService"];

                gvCertHospital.DataSource = dataTable;
                gvCertHospital.DataBind();
            }
            else
            {
                string logHeader = "GetCertServiceDetails DDL - Submit PA";
                string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                logMsg += "dataset is empty";
                CreateAndReturnLogThreadNumber(new Exception(), "SubmitPA-GetCertServiceDetails");
                //Logging log = new Logging(new Guid(CON.WebPageLogGuid.PriorAuthWebPageLog), logMsg);
                //log.CreateLogEntry(logHeader, Logging.LogPriority.Error);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetCertServiceDetails method", ex);
        }
    }
    private void GetAttachment()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            var ds = PriorAuthHospitalController.SelectPriorAuthAttachment(rblClaimType.SelectedValue, MedicaidId);
            var dataTable = ds.Tables["Attachments"];

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                //gvAttachment.DataSource = dataTable;
                //gvAttachment.DataBind();
                //gvAttachment.Visible = true;
            }
            else
            {
                // gvAttachment.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetAttachment method", ex);
        }
    }
    private void GetOutcomeofreview()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
            var dataTable = ds.Tables["ReviewerNotes"];
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetOutcomeOfReview method", ex);
        }
    }

    private void GetResonDenialNotes()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
            var dataTable = ds.Tables["AuthorizationService"];

            gvReDenial.DataSource = dataTable;
            gvReDenial.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetReasonDenialNotes method", ex);
        }
    }

    private void GetDocumentbyMail()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        try
        {
            DataSet dataSet = _spa.GetPriorAuthDocumentMail("DOCUMENT_MAIL", CON.PriorAuthHospital.HospitalId, this.WorkflowPage.RegistrationId);
            DataTable dt = dataSet.Tables[0];
            gvDocumentbyMail.DataSource = dt;
            gvDocumentbyMail.DataBind();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void GetPriorDocumentType()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            ddlDoctype.Items.Clear();
            DataSet dataSet = _spa.GetPriorDocumentType();
            DataTable dt = dataSet.Tables[0];
            ddlDoctype.DataSource = dt;
            ddlDoctype.DataValueField = "DOCUMENT_TYPE_ID";
            ddlDoctype.DataTextField = "DOCUMENT_TYPE_DESC";
            ddlDoctype.DataBind();
            ddlDoctype.Items.Insert(0, new ListItem("--- select Document Type ---", String.Empty));
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetPriorDocumentType method", ex);
        }
    }

    public class StatusDetails
    {
        public string ICD10Diag { get; set; }
        public string ICDVersion { get; set; }
        public string DiagDesc { get; set; }
    }

    /// <summary>
    /// This Method will get authoriazation 
    /// </summary>
    private void GetAuthorization()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            ddlAuthorization.Items.Clear();
            DataSet dataSet = _spa.GetAuthorization();
            DataTable dt = dataSet.Tables[0];
            Helper.LoadList(ddlAuthorization, dt, "PRIOR_AUTH_Authorization_TYPE_DESC", "PRIOR_AUTH_Authorization_TYPE_ID", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetAuthorization method", ex);
        }
    }
    /// <summary>
    /// This Method will get Plan Name 
    /// </summary>
    private void GetPlanName()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            ddlplanname.Items.Clear();
            DataSet dataSet = _spa.GetPlanName();
            DataTable dt = dataSet.Tables[0];
            Helper.LoadList(ddlplanname, dt, "PRIOR_AUTH_PLAN_NAME_DESC", "PRIOR_AUTH_PLAN_NAME_ID", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetPlanName method", ex);
        }
    }
    /// <summary>
    /// This Method will get Plan Name 
    /// </summary>
    private void GetManagedcareplan()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            ddlCBMMXP.Items.Clear();
            DataSet dataSet = _spa.GetManagedcareplan();
            DataTable dt = dataSet.Tables[0];
            Helper.LoadList(ddlCBMMXP, dt, "PRIOR_AUTH_COVERED_BY_MEDICAID_MANAGED_CAREPLAN_TYPE", "PRIOR_AUTH_COVERED_BY_MEDICAID_MANAGED_CAREPLAN_ID", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetManagedCarePlan method", ex);
        }
    }


    /// <summary>
    /// This Method will get Special Indicator
    /// </summary>
    private void GetSpecialIndicator()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            ddlSpecialIndicator.Items.Clear();
            DataSet dataSet = _spa.GetSpecialIndicator();
            DataTable dt = dataSet.Tables[0];
            Helper.LoadList(ddlSpecialIndicator, dt, "PRIOR_AUTH_SPECIAL_INDICATOR_DESC", "PRIOR_AUTH_SPECIAL_INDICATOR_ID", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetSpecialIndicator method", ex);
        }
    }

    /// <summary>
    /// This Method will get Prior Placement

    /// </summary>
    private void GetPriorPlacement()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            DataSet dataSet = _spa.GetPriorPlacement();
            DataTable dt = dataSet.Tables[0];
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetPriorPlacement method", ex);
        }
    }
    /// <summary>
    /// This Method will get Prior NewPlacement
    /// </summary>
    private void GetPriorNewPlacement()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            DataSet dataSet = _spa.GetPriorNewPlacement();
            DataTable dt = dataSet.Tables[0];
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetPriorNewPlacement method", ex);
        }
    }




    public override bool SaveData()
    {
        _spa = new PDMSService.PDMSServiceClient();
        DataSet ds = _spa.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        bool doUpdate = Helper.HasRows(ds);

        Dictionary<string, string> parms = new Dictionary<string, string>();
        /// < summary >
        /// This Method will save Hospital
        /// </ summary >

        parms.Add("PRIOR_AUTH_PLAN_NAME_ID", ddlplanname.SelectedValue.ToString());
        parms.Add("PRIOR_AUTH_MEDICAID_MANGED_CARE", ddlCBMMXP.SelectedValue.ToString());
        parms.Add("PRIOR_AUTH_SPECIAL_INDICATOR_ID", ddlSpecialIndicator.SelectedValue.ToString());
        parms.Add("PRIOR_AUTH_ADMISSION_DATE", txtAdmissionDate.Text.ToString());
        parms.Add("PRIOR_AUTH_CONTACT_INFORMATION_NAME", txtContactName.Text.ToString());
        parms.Add("PRIOR_AUTH_CONTACT_INFORMATION_PHONE_NUMBER_EXT", txtContactNumber.Text.ToString());
        parms.Add("PRIOR_AUTH_TRACKING_NUMBER", txtTrackingNumber.Text.ToString());
        parms.Add("PRIOR_AUTH_SERVICING_PROVIDER_INFORMATION_Medicaid_ID", txtMedicaidID.Text.ToString());
        parms.Add("PRIOR_AUTH_SERVICING_PROVIDER_INFORMATION_NPI", txtSPNPI.Text.ToString());
        //parms.Add("PRIOR_AUTH_SERVICING_PROVIDER_INFORMATION_NAME", txtSPName.Text.ToString());
        parms.Add("PRIOR_AUTH_ORDERING_PROVIDER_INFORMATION_Medicaid_ID", txtOMID.Text.ToString());
        parms.Add("PRIOR_AUTH_ORDERING_PROVIDER_INFORMATION_NPI", txtorderingprovidernpi.Text.ToString());
        //parms.Add("PRIOR_AUTH_ORDERING_PROVIDER_INFORMATION_NAME", txtOPNAME.Text.ToString());
        //parms.Add("PRIOR_AUTH_DENTAL_PROSTHODONTICS_ID", ddlEdentulist.SelectedValue.ToString());
        //parms.Add("PRIOR_AUTH_DENTAL_NEW_PLACEMNT_ID", ddlNewPlacement.SelectedValue.ToString());
        //parms.Add("PRIOR_AUTH_DENTAL_PRIOR_PLACEMNT_ID", ddlPriorPlacement.SelectedValue.ToString());

        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        parms.Add("LAST_MODIFIED_USER", id.ToString());
        parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
        parms.Add("Created_By_User", id.ToString());
        var hospitalId = _spa.InsertPriorAuthHospital("AUTH_HOSPITAL", parms);



        /// <summary>
        /// This Method will save DENTAL_PROSTHODONTICS
        /// </summary>
        /// 

        parms = new Dictionary<string, string>();
        parms.Add("PRIOR_AUTH_HOSPITAL_ID", hospitalId.ToString());
        //parms.Add("PRIOR_AUTH_DENTAL_PROSTHODONTICS_RECIPIENT_EDENTULIST", ddlEdentulist.SelectedValue.ToString());
        //parms.Add("PRIOR_AUTH_DENTAL_PROSTHODONTICS_DATE_EXTRACTION", txtextraction.Text.ToString());
        //parms.Add("PRIOR_AUTH_DENTAL_NEW_PLACEMENT_ID", ddlNewPlacement.SelectedValue.ToString());
        //parms.Add("PRIOR_AUTH_DENTAL_PRIOR_PLACEMENT_ID", ddlPriorPlacement.SelectedValue.ToString());

        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", id.ToString());
        parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
        parms.Add("Created_By_User", id.ToString());
        _spa.InsertPriorAuthDentalProsthodontics("AUTH_Prosthodontics", parms);
        return true;

    }

    private void LoadPASTATUS()
    {

    }

    private Dictionary<string, string> LoadPAStatusMapping()
    {
        Dictionary<string, string> statusMap = new Dictionary<string, string>();
        statusMap.Add("A1", "Approved");
        statusMap.Add("A2", "Partially Approved");
        statusMap.Add("A3", "Denied");
        statusMap.Add("A4", "Pend");
        statusMap.Add("C", "Closed");
        statusMap.Add("NA", "InProcess");
        statusMap.Add("A5", "Pending Addtl Info");
        return statusMap;
    }

    protected void gvPriorAuthNotes_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // fetch UserId from DataKeysa        int PriorAttachmentId = Convert.ToInt32(gvPriorAuthAttachment.DataKeys[e.RowIndex]["AttachmentsList_Id"].ToString());


        //BindGrid();
    }

    public override void LoadData(DataRow row = null)
    {
        //base.LoadData(dr);
        // ucPriorAuthServiceDetails.LoadState();
    }

    public bool CanUserViewDelete()
    {
        return Registration.CanUserViewDelete(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName);
    }

    int failureValidations = 0;
    int maxValidations = 20;

    public void setValidateColor()
    {
        foreach (ListItem li in ddlAuthorization.Items)
        {
            li.Attributes.Add("style", "background-color:white");
        }
        foreach (ListItem li in ddlAssignment.Items)
        {
            li.Attributes.Add("style", "background-color:white");
        }
        foreach (ListItem li in ddlServiceType.Items)
        {
            li.Attributes.Add("style", "background-color:white");
        }

        lstControl = getControls();

        foreach (Control oControl in lstControl)
        {
            if (oControl.GetType() == typeof(TextBox))
            {
                var txtBox = ((TextBox)(oControl));
                if (txtBox.Text != string.Empty)
                {
                    txtBox.Attributes.Add("style", "background-color:white");
                    txtBox.Width = 200;
                }
            }
            //check for all the DropDownList controls on the page and reset it to the very first item e.g. "-- Select One --"
            else if (oControl.GetType() == typeof(DropDownList))
            {
                if (((DropDownList)(oControl)).DataSource != null && ((DropDownList)(oControl)).Items.Count > 0 && ((DropDownList)(oControl)).SelectedIndex != 0)
                {
                    ((DropDownList)(oControl)).Attributes.Add("style", "background-color:white");
                    ((DropDownList)(oControl)).Width = 200;
                }
            }
        }
    }

    public override bool ValidateData()
    {
        bool isValid = true;

        //Common submission errors for all PA types
        if (string.IsNullOrEmpty(ddlAuthorization.SelectedValue) && failureValidations <= maxValidations)
        {
            ++failureValidations;
            ddlAuthorization.Attributes.Add("style", "background-color:yellow");
            ddlAuthorization.Width = 200;
            foreach (ListItem li in ddlAuthorization.Items)
            {
                li.Attributes.Add("style", "background-color:white");
            }
            AddValidationErrorMessage("<a href='#' class='errMsg' id='noCpe_noPnl_ddlAuthorization' onclick='javascript:focusError(this.id)'>*Destination payer is required</a>");
            isValid = false;
        }

        if (string.IsNullOrEmpty(ddlAssignment.SelectedValue) && failureValidations <= maxValidations)
        {
            ++failureValidations;
            ddlAssignment.Attributes.Add("style", "background-color:yellow");
            ddlAssignment.Width = 200;
            foreach (ListItem ali in ddlAssignment.Items)
            {
                ali.Attributes.Add("style", "background-color:white");
            }
            AddValidationErrorMessage("<a href='#' class='errMsg' id='noCpe_noPnl_ddlAssignment' onclick='javascript:focusError(this.id)'>*Assignment is required</a>");
            isValid = false;
        }

        if (string.IsNullOrEmpty(ddlSubCapitaPayerIDs.SelectedValue) && failureValidations <= maxValidations)
        {
            ++failureValidations;
            ddlSubCapitaPayerIDs.Attributes.Add("style", "background-color:yellow");
            ddlSubCapitaPayerIDs.Width = 200;
            foreach (ListItem ali in ddlSubCapitaPayerIDs.Items)
            {
                ali.Attributes.Add("style", "background-color:white");
            }
            AddValidationErrorMessage("<a href='#' class='errMsg' id='noCpe_noPnl_ddlSubCapitaPayerIDs' onclick='javascript:focusError(this.id)'>*Destination Payer ID is required</a>");
            isValid = false;
        }

        if (string.IsNullOrEmpty(ddlServiceType.SelectedValue) && failureValidations <= maxValidations)
        {
            ++failureValidations;
            ddlServiceType.Attributes.Add("style", "background-color:yellow");
            ddlServiceType.Width = 200;
            foreach (ListItem sli in ddlServiceType.Items)
            {
                sli.Attributes.Add("style", "background-color:white");
            }
            AddValidationErrorMessage("<a href='#' class='errMsg' id='noCpe_noPnl_ddlServiceType' onclick='javascript:focusError(this.id)'>*Service Type is required</a>");
            isValid = false;
        }

        if (String.IsNullOrEmpty(txtMedicaidBillingNumber.Text) && failureValidations <= maxValidations)
        {
            ++failureValidations;
            txtMedicaidBillingNumber.Attributes.Add("style", "background-color:yellow");
            txtMedicaidBillingNumber.Width = 200;
            AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeRecipient_pnlsepRecipient_txtMedicaidBillingNumber' onclick='javascript:focusError(this.id)'>*Medicaid Billing Number is required</a>");
            isValid = false;
        }

        if (String.IsNullOrEmpty(txtBirthDate.Text) && failureValidations <= maxValidations)
        {
            ++failureValidations;
            txtBirthDate.Attributes.Add("style", "background-color:yellow");
            txtBirthDate.Width = 200;
            AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeRecipient_pnlsepRecipient_txtBirthDate' onclick='javascript:focusError(this.id)'>*Recipient date of birth is required</a>");
            isValid = false;
        }

        if (String.IsNullOrEmpty(txtContactName.Text) && failureValidations <= maxValidations)
        {
            ++failureValidations;
            txtContactName.Attributes.Add("style", "background-color:yellow");
            txtContactName.Width = 200;
            AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeContact_pnlSepContact_txtContactName' onclick='javascript:focusError(this.id)'>*Contact First Name is required</a>");
            isValid = false;
        }

        if (String.IsNullOrEmpty(txtContactLastName.Text) && failureValidations <= maxValidations)
        {
            ++failureValidations;
            txtContactLastName.Attributes.Add("style", "background-color:yellow");
            txtContactLastName.Width = 200;
            AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeContact_pnlSepContact_txtContactLastName' onclick='javascript:focusError(this.id)'>*Contact Last Name is required</a>");
            isValid = false;
        }

        var count = (txtContactNumber.Text.Trim()).Count(Char.IsDigit);
        if (count == 0 && failureValidations <= maxValidations)
        {
            ++failureValidations;
            txtContactNumber.Attributes.Add("style", "background-color:yellow");
            txtContactNumber.Width = 200;
            AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeContact_pnlSepContact_txtContactNumber' onclick='javascript:focusError(this.id)'>*Contact Number is required</a>");
            isValid = false;
        }
        else if (count != 10 && failureValidations <= maxValidations)
        {
            ++failureValidations;
            txtContactNumber.Attributes.Add("style", "background-color:yellow");
            txtContactNumber.Width = 200;
            AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeContact_pnlSepContact_txtContactNumber' onclick='javascript:focusError(this.id)'>*Contact Number is incorrect</a>");
            isValid = false;
        }

        List<string> ddlAssignmentList = new List<string>() { "01", "02", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "19", "23", "40", "46" };
        if (ddlAssignmentList.Contains(ddlAssignment.SelectedValue)
            && string.IsNullOrEmpty(txtorderingprovidernpi.Text) && !string.IsNullOrEmpty(ddlAssignment.SelectedValue)
            && failureValidations <= maxValidations)
        {
            if (string.IsNullOrEmpty(hdtxtOMID1.Value))
            {
                ++failureValidations;
                txtorderingprovidernpi.Attributes.Add("style", "background-color:yellow");
                txtorderingprovidernpi.Width = 200;
                AddValidationErrorMessage("<a href='#' class='errMsg' id='noCpe_noPnl_ddlAssignment' onclick='javascript:focusError(this.id)'>*Ordering Provider information is required</a>");
                isValid = false;
            }
        }

        if ((string.IsNullOrEmpty(txtSPNPI.Text) || txtSPNPI.Text.Length != 10) && failureValidations <= maxValidations)
        {
            ++failureValidations;
            txtSPNPI.Attributes.Add("style", "background-color:yellow");
            txtSPNPI.Width = 200;
            AddValidationErrorMessage("<a href='#' class='errMsg' id='CPEService_pnlSepService_txtSPNPI' onclick='javascript:focusError(this.id)'>*Service Provider 10-digit number is required</a>");
            isValid = false;
        }

        //Institutional PA Submission Errors
        if (GetClaimTypeID() == Convert.ToInt32(CON.ClaimsType.Institutional) && failureValidations <= maxValidations)
        {
            //(pnlDiagnosisLine.Enabled == true && pnlDiagnosisLine.Visible == true && ddlDiagnosisCodeType.Visible == true && string.IsNullOrEmpty(ddlDiagnosisCodeType.SelectedValue)
            if (!ddlAssignment.SelectedValue.Contains("03,18,20") && !string.IsNullOrEmpty(ddlAssignment.SelectedValue)
                && isDiagnosisLineOpened.Value == "true" && failureValidations <= maxValidations)
            {
                if (!string.IsNullOrEmpty(txtPANumber2.Text) && txtPANumber2.Text.StartsWith(CON.PANumber_StartsWith))
                {
                    ++failureValidations;
                    ddlDiagnosisCodeType.Attributes.Add("style", "background-color:yellow");
                    ddlDiagnosisCodeType.Width = 200;
                    AddValidationErrorMessage("<a href='#' class='errMsg' id='noCpe_noPnl_ddlAssignment' onclick='javascript:focusError(this.id)'>*Diagnosis Code Type is required</a>");
                    isValid = false;
                }
            }

            if (ddlAssignment.SelectedValue.Contains("34,35,55") && !string.IsNullOrEmpty(ddlAssignment.SelectedValue)
                && string.IsNullOrEmpty(ddlServiceTypeCode.SelectedValue) && failureValidations <= maxValidations)
            {
                if (!string.IsNullOrEmpty(txtPANumber2.Text) && !txtPANumber2.Text.StartsWith(CON.PANumber_StartsWith) && ddlAssignment.SelectedItem.Text.ToLower() != CON.PA_Instititional_Proc_Code_Psychiatric_Inpatient)
                {
                    ++failureValidations;
                    txtorderingprovidernpi.Attributes.Add("style", "background-color:yellow");
                    txtorderingprovidernpi.Width = 200;
                    AddValidationErrorMessage("<a href='#' class='errMsg' id='noCpe_noPnl_ddlAssignment' onclick='javascript:focusError(this.id)'>*Procedure Code Type is required</a>");
                    isValid = false;
                }
            }

            if (string.IsNullOrEmpty(txtfacilityType.Text) && failureValidations <= maxValidations)
            {
                ++failureValidations;
                txtfacilityType.Attributes.Add("style", "background-color:yellow");
                txtfacilityType.Width = 200;
                AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeServiceInformation_pnlsepServiceInformation_txtfacilityType' onclick='javascript:focusError(this.id)'>*Facility type is invalid</a>");
                isValid = false;
            }

            if (ddlLevelService.SelectedValue == "-1" && failureValidations <= maxValidations)
            {
                ++failureValidations;
                ddlLevelService.Attributes.Add("style", "background-color:yellow");
                ddlLevelService.Width = 200;
                foreach (ListItem sli in ddlLevelService.Items)
                {
                    sli.Attributes.Add("style", "background-color:white");
                }
                AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeServiceInformation_pnlsepServiceInformation_ddlLevelService' onclick='javascript:focusError(this.id)'>*Level of Service is required.</a>");
                isValid = false;
            }

            if (ddlAdminType.SelectedValue == "-1" && failureValidations <= maxValidations)
            {
                ++failureValidations;
                ddlAdminType.Attributes.Add("style", "background-color:yellow");
                ddlAdminType.Width = 200;
                foreach (ListItem sli in ddlAdminType.Items)
                {
                    sli.Attributes.Add("style", "background-color:white");
                }
                AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeServiceInformation_pnlsepServiceInformation_ddlAdminType' onclick='javascript:focusError(this.id)'>*Admission Type is required.</a>");
                isValid = false;
            }

            if (ddlAdminSrc.SelectedValue == "-1" && failureValidations <= maxValidations)
            {
                ++failureValidations;
                ddlAdminSrc.Attributes.Add("style", "background-color:yellow");
                ddlAdminSrc.Width = 200;
                foreach (ListItem sli in ddlAdminSrc.Items)
                {
                    sli.Attributes.Add("style", "background-color:white");
                }
                AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeServiceInformation_pnlsepServiceInformation_ddlAdminSrc' onclick='javascript:focusError(this.id)'>*Admission Source is required.</a>");
                isValid = false;
            }

            if (ddlDischargeStatus.SelectedValue == "" && failureValidations <= maxValidations)
            {
                if (!(ddlAssignment.SelectedValue.Contains("35") && !string.IsNullOrEmpty(ddlAssignment.SelectedValue)))
                {
                    ++failureValidations;
                    ddlDischargeStatus.Attributes.Add("style", "background-color:yellow");
                    ddlDischargeStatus.Width = 200;
                    foreach (ListItem sli in ddlDischargeStatus.Items)
                    {
                        sli.Attributes.Add("style", "background-color:white");
                    }
                    AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeServiceInformation_pnlsepServiceInformation_ddlDischargeStatus' onclick='javascript:focusError(this.id)'>*Discharge Status is required.</a>");
                    isValid = false;
                }
            }

            if (!ddlAssignment.SelectedValue.Contains("35") && string.IsNullOrEmpty(txtFAdmissionDate.Text) && failureValidations <= maxValidations)
            {
                ++failureValidations;
                txtFAdmissionDate.Attributes.Add("style", "background-color:yellow");
                txtFAdmissionDate.Width = 200;
                AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeServiceInformation_pnlsepServiceInformation_txtFAdmissionDate' onclick='javascript:focusError(this.id)'>*Admission date is required</a>");
                isValid = false;
            }

            var assignmentType = new List<string>() { "34", "35", "55", "59" };
            if ((assignmentType.Contains(ddlAssignment.SelectedValue) && ddlAuthorization.SelectedValue.Contains("1"))
                && failureValidations <= maxValidations)
            {
                if (_spa == null)
                {
                    _spa = new PDMSService.PDMSServiceClient();
                }

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
                parms.Add("LINK_SECTIONS", SaveCodeLNK);
                var ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);

                if (ds != null && ds.Tables.Count > 0)
                {
                    var dataTable = ds.Tables[0];

                    if (dataTable == null || dataTable.Rows.Count <= 0)
                    {
                        ++failureValidations;
                        AddValidationErrorMessage("<a href='#' class='errMsg' id='CPEService_pnlSepService_txtSPNPI' onclick='javascript:focusError(this.id)'>*Service detail information is required</a>");
                        isValid = false;
                    }
                }
            }



            //Validation to check if FDOS and TDOS is not empty when service details are provided
            Dictionary<string, string> parms4 = new Dictionary<string, string>();
            parms4.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms4.Add("LINK_SECTIONS", SaveCodeLNK);
            var dsInstServiceDetails = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms4);
            if (dsInstServiceDetails != null && dsInstServiceDetails.Tables.Count > 0 && dsInstServiceDetails.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow instServiceDetailRow in dsInstServiceDetails.Tables[0].Rows)
                {
                    if (instServiceDetailRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] != null && instServiceDetailRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] != DBNull.Value && string.IsNullOrEmpty(instServiceDetailRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"].ToString()))
                    {
                        ++failureValidations;
                        AddValidationErrorMessage("Requested FDOS is required");
                        isValid = false;
                    }

                    if (instServiceDetailRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] != null && instServiceDetailRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] != DBNull.Value && string.IsNullOrEmpty(instServiceDetailRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"].ToString()))
                    {
                        ++failureValidations;
                        AddValidationErrorMessage("Requested TDOS is required");
                        isValid = false;
                    }
                }
            }

            List<PriorAuthAttachment> attachments = FetchAttachments();

            string enablePriorAuthAttachment = Helper.GetAppSettingFromDB("enablePriorAuthAttachment", string.Empty);
            if (enablePriorAuthAttachment.Equals("true") && GetClaimTypeID() == Convert.ToInt32(CON.ClaimsType.Institutional)
                && (FetchAttachments() == null || FetchAttachments().Count <= 0) && failureValidations <= maxValidations && string.IsNullOrEmpty(txtPANumber2.Text))
            {
                ++failureValidations;
                //gvAttachment.Attributes.Add("style", "background-color:yellow");
                AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeAttachment_pnlSepAttachment_PriorAttachmentUpload' onclick='javascript:focusError(this.id)'>*A document must be added</a>");
                isValid = false;
            }

            if (string.IsNullOrEmpty(ddlDischargeStatus.SelectedValue) && !string.IsNullOrEmpty(ddlDischargeStatus.SelectedValue)
                && failureValidations <= maxValidations)
            {
                ++failureValidations;
                ddlDischargeStatus.Attributes.Add("style", "background-color:yellow");
                ddlDischargeStatus.Width = 200;
                AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeServiceInformation_pnlServiceInformation_ddlDischargeStatus' onclick='javascript:focusError(this.id)'>*Patient discharge status is required</a>");
                isValid = false;
            }

        }

        //Professional PA Submission Errors
        if (GetClaimTypeID() == Convert.ToInt32(CON.ClaimsType.Professional) && failureValidations <= maxValidations)
        {
            if (!string.IsNullOrEmpty(txtpalceofservice.Text) && failureValidations <= maxValidations)
            {

                string posSTR = txtpalceofservice.Text.ToString();
                string[] strArray = posSTR.Split('-');

                if (strArray.Length > 1)
                {
                    string firstItem = strArray[0];
                    string SecondItem = strArray[1];
                    DataTable dt = GetPlaceOfServiceData(firstItem, SecondItem, false);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        ++failureValidations;
                        txtpalceofservice.Attributes.Add("style", "background-color:yellow");
                        txtpalceofservice.Width = 200;
                        AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeServiceInformation_pnlsepServiceInformation_txtpalceofservice' onclick='javascript:focusError(this.id)'>*Place of Service is invalid</a>");
                        isValid = false;
                    }
                }
                else
                {
                    DataTable dt = GetPlaceOfServiceData(posSTR, default(string), true);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        ++failureValidations;
                        txtpalceofservice.Attributes.Add("style", "background-color:yellow");
                        txtpalceofservice.Width = 200;
                        AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeServiceInformation_pnlsepServiceInformation_txtpalceofservice' onclick='javascript:focusError(this.id)'>*Place of Service is invalid</a>");
                        isValid = false;
                    }
                }

            }
            else
            {
                //Check for dula service and skip the validation
                if (ddlAssignment.SelectedValue != "61")
                {
                    ++failureValidations;
                    txtpalceofservice.Attributes.Add("style", "background-color:yellow");
                    txtpalceofservice.Width = 200;
                    AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeServiceInformation_pnlsepServiceInformation_txtpalceofservice' onclick='javascript:focusError(this.id)'>*Place of Service is invalid</a>");
                    isValid = false;
                }
                else
                {
                    txtpalceofservice.Attributes.Add("style", "none");
                }
            }

            //if (pnlProfessionalLine.Visible && failureValidations <= maxValidations)
            //{
            //    ++failureValidations;
            //    AddValidationErrorMessage("Service Line should be added.");
            //    isValid = false;
            //}

            //Get invoking the professional service details function to active the session
            GetProfessionalServiceDetails();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms.Add("LINK_SECTIONS", SaveCodeLNK);
            var dsProf = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
            if (dsProf != null)
            {
                var dataTable = dsProf.Tables[0];
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    //Validation to check if FDOS and TDOS is not empty when service details are provided
                    foreach (DataRow profServiceDetailRow in dataTable.Rows)
                    {
                        if (profServiceDetailRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"] != null && profServiceDetailRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"] != DBNull.Value && string.IsNullOrEmpty(profServiceDetailRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"].ToString()))
                        {
                            ++failureValidations;
                            AddValidationErrorMessage("Requested FDOS is required");
                            isValid = false;
                        }

                        if (profServiceDetailRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"] != null && profServiceDetailRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"] != DBNull.Value && string.IsNullOrEmpty(profServiceDetailRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"].ToString()))
                        {
                            ++failureValidations;
                            AddValidationErrorMessage("Requested TDOS is required");
                            isValid = false;
                        }
                    }
                }
                else
                {
                    ++failureValidations;
                    AddValidationErrorMessage("Service detail panel is required");
                    isValid = false;
                }
            }
            else
            {
                ++failureValidations;
                AddValidationErrorMessage("Service detail panel is required");
                isValid = false;
            }
        }

        //Dental PA Submission Errors
        if (GetClaimTypeID() == Convert.ToInt32(CON.ClaimsType.Dental) && failureValidations <= maxValidations)
        {
            if (string.IsNullOrEmpty(txtpalceofservice.Text) && failureValidations <= maxValidations)
            {
                ++failureValidations;
                txtpalceofservice.Attributes.Add("style", "background-color:yellow");
                txtpalceofservice.Width = 200;
                AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeServiceInformation_pnlsepServiceInformation_txtpalceofservice' onclick='javascript:focusError(this.id)'>*Place of Service is required</a>");
                isValid = false;
            }

            if ((string.IsNullOrEmpty(txtSPNPI.Text) || txtSPNPI.Text.Length != 10) && failureValidations <= maxValidations)
            {
                ++failureValidations;
                txtSPNPI.Attributes.Add("style", "background-color:yellow");
                txtSPNPI.Width = 200;
                AddValidationErrorMessage("<a href='#' class='errMsg' id='CPEService_pnlSepService_txtSPNPI' onclick='javascript:focusError(this.id)'>*Service Provider 10-digit number is required</a>");
                isValid = false;
            }

            //Get invoking the dental Service details function to active the session.
            GetDentalServiceDetails();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms.Add("LINK_SECTIONS", SaveCodeLNK);
            var dsDen = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
            if (dsDen != null)
            {
                var dataTable = dsDen.Tables[0];
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    //Validation to check if FDOS and TDOS is not empty when service details are provided
                    foreach (DataRow dentalServiceDetailRow in dataTable.Rows)
                    {
                        if (dentalServiceDetailRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"] != null && dentalServiceDetailRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"] != DBNull.Value && string.IsNullOrEmpty(dentalServiceDetailRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"].ToString()))
                        {
                            ++failureValidations;
                            AddValidationErrorMessage("Requested FDOS is required");
                            isValid = false;
                        }

                        if (dentalServiceDetailRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"] != null && dentalServiceDetailRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"] != DBNull.Value && string.IsNullOrEmpty(dentalServiceDetailRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"].ToString()))
                        {
                            ++failureValidations;
                            AddValidationErrorMessage("Requested TDOS is required");
                            isValid = false;
                        }
                    }
                }
                else
                {
                    ++failureValidations;
                    AddValidationErrorMessage("Service detail panel is required");
                    isValid = false;
                }
            }
            else
            {
                ++failureValidations;
                AddValidationErrorMessage("Service detail panel is required");
                isValid = false;
            }
        }

        DateTime dateTime;
        if ((!(String.IsNullOrEmpty(txtMenDtInst.Text)) && DateTime.TryParse(txtMenDtInst.Text, out dateTime) && (Convert.ToDateTime(txtMenDtInst.Text) > DateTime.Now) && failureValidations <= maxValidations))
        {
            AddValidationErrorMessage(" Last  Menstrual Period cannot be greater than todays date.");
            isValid = false;
        }
        if ((!(String.IsNullOrEmpty(txtProfOnsetIllness.Text)) && DateTime.TryParse(txtProfOnsetIllness.Text, out dateTime) && (Convert.ToDateTime(txtProfOnsetIllness.Text) > DateTime.Now) && failureValidations <= maxValidations))
        {
            AddValidationErrorMessage(" Date of onset of Illness cannot be greater than todays date.");
            isValid = false;
        }
        if ((!(String.IsNullOrEmpty(TextBox3.Text)) && DateTime.TryParse(TextBox3.Text, out dateTime) && (Convert.ToDateTime(TextBox3.Text) > DateTime.Now) && failureValidations <= maxValidations))
        {
            AddValidationErrorMessage(" Date of Patient Event cannot be greater than todays date.");
            isValid = false;
        }
        //if ((!(String.IsNullOrEmpty(txtEstDOB.Text)) && DateTime.TryParse(txtEstDOB.Text, out dateTime) && (Convert.ToDateTime(txtEstDOB.Text) > DateTime.Now) && failureValidations <= maxValidations))
        //{
        //    AddValidationErrorMessage(" Estimated Date Of Birth cannot be greater than todays date.");
        //    isValid = false;
        //}
        if ((!(String.IsNullOrEmpty(txtAccDtService.Text)) && DateTime.TryParse(txtAccDtService.Text, out dateTime) && (Convert.ToDateTime(txtAccDtService.Text) > DateTime.Now) && failureValidations <= maxValidations))
        {
            AddValidationErrorMessage(" Accident Date cannot be greater than todays date.");
            isValid = false;
        }

        return isValid;
    }

    public bool ValidatePAAttachments()
    {
        bool isValid = true;

        isValid &= ValidateAttachment(Convert.ToInt32(CON.ClaimsType.Professional), "pnlAttachment");
        isValid &= ValidateAttachment(Convert.ToInt32(CON.ClaimsType.Dental), "pnlDentalAttachment");

        return isValid;
    }

    private bool ValidateAttachment(int claimType, string errorPanelId)
    {
        if (GetClaimTypeID() != claimType || failureValidations > maxValidations || !string.IsNullOrEmpty(txtPANumber2.Text))
            return true;

        List<PriorAuthAttachment> attachments = FetchAttachments();
        bool isAttachmentInvalid = attachments == null || attachments.Count == 0;

        if (isAttachmentInvalid)
        {
            ++failureValidations;
            ddlDiagnosisCodeType.Attributes.Add("style", "background-color:yellow");
            ddlDiagnosisCodeType.Width = 200;
            AddValidationErrorMessage("<a href='#' class='errMsg' id='" + errorPanelId + "' onclick='javascript:focusError(this.id)'>*Attachment is required.</a>");
            return false;
        }

        return true;
    }

    private List<PriorAuthAttachment> FetchAttachments()
    {
        List<PriorAuthAttachment> attachmetns = new List<PriorAuthAttachment>();

        string attachmentsJson = hdnPriorAuthAttachments.Value;
        attachmetns = JsonConvert.DeserializeObject<List<PriorAuthAttachment>>(attachmentsJson);

        return attachmetns;
    }




    // Helper Method: Highlights Diagnosis Code Field
    private void HighlightDiagnosisCodeField()
    {
        ddlDiagnosisCodeType.Attributes.Add("style", "background-color:yellow");
        ddlDiagnosisCodeType.Width = 200;
    }



    public bool ValidatePANumberPopulated()
    {
        bool isValid = true;

        //Common submission errors for all PA types
        if (String.IsNullOrEmpty(txtPatientTrckNum.Text) && failureValidations <= maxValidations)
        {
            ++failureValidations;
            AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeRecipient_pnlsepRecipient_txtPatientTrckNum' onclick='javascript:focusError(this.id)'>*Patient Tracking Number is required to save your prior authorization</a>");
            isValid = false;
        }
        if (string.IsNullOrEmpty(ddlAuthorization.SelectedValue) && failureValidations <= maxValidations)
        {
            ++failureValidations;
            AddValidationErrorMessage("<a href='#' class='errMsg' id='noCpe_noPnl_ddlAuthorization' onclick='javascript:focusError(this.id)'>*Destination payer is required</a>");
            isValid = false;
        }
        return isValid;
    }

    public override string Title
    {
        get { return "Submit Prior Authorization"; }
    }

    public override string IdText
    {
        get { return "ucSubmitPriorAuthorization_" + this.WorkflowPage.RegistrationId; }
    }

    protected void grdDiagnosis_SelectedIndexChanged(object sender, EventArgs e)
    {
        // gvDiagnosis.PageIndex = e.NewPageIndex;
        //BindGrid();

        //gvDiagnosis.EditIndex = -1;
    }

    //protected void grdServiceDetailSearch_Rowcommand(Object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "selectlinkbtn")
    //    {
    //        int index = Convert.ToInt32(e.CommandArgument);
    //        GridViewRow selectedRow = gvServiceDetail.Rows[index];

    //    }
    //}

    protected void grdServiceDetailSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetCertServiceDetails();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }
    protected void grdServiceDetailSearch_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GetCertServiceDetails();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void gvPriorAuthAttachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GetAttachment();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }
    protected void grdProviderNote_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void grdReasonforDenial_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void OnDataBound(object sender, EventArgs e)
    {
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        TableHeaderCell cell = new TableHeaderCell();
        cell.Text = ddlAssignment.SelectedItem.Text;
        cell.ColumnSpan = 3;
        row.Controls.Add(cell);
        //gvServiceDetail.HeaderRow.Parent.Controls.AddAt(0, row);
    }
    protected void grdServiceDentalDetailSearch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void AddErrorMessage(string message)
    {
        //lblErrorMessages.Text = string.Format("{0}{1}<br />", lblErrorMessages.Text, message);

    }

    private void ClearErrorMessages()
    {
        valerrormess.ShowSummary = false;
        valerrormess.Visible = false;
    }

    protected void grdAttachment_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  gvAttachment.PageIndex = e.NewPageIndex;
        //BindGrid();
        // gvAttachment.EditIndex = -1;
    }

    protected void gvAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DownloadDocument")
        {
            string fileName = e.CommandArgument.ToString();
            ProcessAttachments attachments = new ProcessAttachments();
            attachments.GetAttachment(fileName);
        }
    }

    protected void grdDocumentbyMail_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void gvOutcomeofreview_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void grdRevNotesProvider_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void grdReDenial_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void grdCertHospital_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void grdServicingProviderInfo_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlAssignment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlAssignment.Attributes.Add("style", "background-color:white");
            ddlSubCapitaPayerIDs.Attributes.Add("style", "background-color:white");


            if (ddlAssignment.SelectedValue != null)
            {
                if (valerrormess.Visible)
                {
                    if (!ReValidateData())
                    {
                        return;
                    }
                }
                //ddlAuthorization_SelectedIndexChanged(null, null);
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void DisablePanels()
    {
        if (rblClaimType.SelectedValue != null)
        {
            switch (rblClaimType.SelectedValue.ToUpper())
            {
                case "DENTAL":
                    pnlsepProfessionalServiceDetail.Visible = false;
                    pnlProfessionalServiceDetail.Visible = false;
                    pnlsepServiceDetail.Visible = false;
                    pnlServiceDetail.Visible = false;
                    pnlCertHospital.Visible = false;
                    pnlsepCertHospital.Visible = false;
                    pnlsepDentalServiceDetail.Visible = true;
                    pnlDentalServiceDetail.Visible = true;
                    break;
                case "PROFESSIONAL":
                    pnlsepProfessionalServiceDetail.Visible = true;
                    pnlProfessionalServiceDetail.Visible = true;
                    pnlCertHospital.Visible = false;
                    pnlsepCertHospital.Visible = false;
                    pnlsepDentalServiceDetail.Visible = false;
                    pnlDentalServiceDetail.Visible = false;
                    pnlsepServiceDetail.Visible = false;
                    pnlServiceDetail.Visible = false;
                    break;
                case "INSTITUTIONAL":
                    pnlsepServiceDetail.Visible = true;
                    pnlServiceDetail.Visible = true;
                    pnlCertHospital.Visible = false;
                    pnlsepCertHospital.Visible = false;
                    pnlsepDentalServiceDetail.Visible = false;
                    pnlDentalServiceDetail.Visible = false;
                    pnlsepProfessionalServiceDetail.Visible = false;
                    pnlProfessionalServiceDetail.Visible = false;
                    break;
            }
        }

        //pnlProvidermainPnl.Visible = false;
        //pnlReviewerNotes.Visible = false;
        //pnlsepProsthodontics.Visible = false;
        //pnlProsthodontics.Visible = false;
        pnlSepService.Visible = true;
        pnlService.Visible = true;
        //pnlsepDiagnosissearch.Visible = false;
        //pnlDiagnosissearch.Visible = false;
        // pnlline.Visible = false;
        //pnlsepline.Visible = false;
        // pnlDentalLine.Visible = false;
        pnlsepDentalLine.Visible = false;
        //pnlsepReasonforDenial.Visible = false;
        //sepReasonforDenial.Visible = false;
        // pnlServiceDetail.Visible = false;
    }

    protected void ddlSubCapitaPayerIDs_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubCapitaPayerIDs.Attributes.Add("style", "background-color:white");

        string MCEID = ddlSubCapitaPayerIDs.SelectedValue;
        txtDestinationpayerID.Value = MCEID;
    }

    protected void ddlAuthorization_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlAuthorization.Attributes.Add("style", "background-color:white");

            if (!string.IsNullOrEmpty(ddlAuthorization.SelectedValue))
            {

                if (Session["DestinationPayer"] != null)
                {
                    DataTable destpayer = (DataTable)Session["DestinationPayer"];
                    if (destpayer != null && destpayer.Rows.Count > 0)
                    {



                        var dataRow = destpayer.AsEnumerable().Where(x => x.Field<int>("PRIOR_AUTH_DESTINATION_PAYER_ID") == Convert.ToInt32(ddlAuthorization.SelectedValue)).FirstOrDefault();
                        if (dataRow != null)
                        {
                            string destPayerID = Convert.ToString(dataRow["PRIOR_AUTH_DESTINATION_PAYER_ID"]);
                            LoadDestinationPayerIDs(destPayerID);
                        }
                    }
                }
                string errormessage = "Invalid Assignment for Prior Authorization Type";
                DisablePanels();

                if (ddlAssignment.SelectedValue == "34" || ddlAssignment.SelectedValue == "35" || ddlAssignment.SelectedValue == "55" || ddlAssignment.SelectedValue == "59" || ddlAssignment.SelectedValue == "37")
                {
                    pnlsepServiceDetail.Visible = true;
                    pnlServiceDetail.Visible = true;
                    GetServiceDetails();
                    btnServiceDetailAdd1.Enabled = true;
                    if (ddlAssignment.SelectedValue == "37")
                    {
                        //foreach (GridViewRow gRow in gvServiceDetail.Rows)
                        //{
                        //    if (gRow.RowType == DataControlRowType.DataRow)
                        //        gRow.Enabled = false;
                        //}
                        btnServiceDetailAdd1.Enabled = false;
                    }
                }
                else if (ddlAuthorization.SelectedValue == "3" && ddlAssignment.SelectedValue == "37")
                {
                    pnlCertHospital.Visible = true;
                    pnlsepCertHospital.Visible = true;
                    GetCertServiceDetails();
                }
                else if (ddlAuthorization.SelectedValue == "1" && (ddlAssignment.SelectedValue == "03"
                    || ddlAssignment.SelectedValue == "20"))//Dental
                {
                    pnlsepDentalServiceDetail.Visible = true;
                    pnlDentalServiceDetail.Visible = true;
                    GetDentalServiceDetails();

                }

                if (ddlAssignment.SelectedValue == "59" || ddlAssignment.SelectedValue == "60")
                {


                }
                if (valerrormess.Visible)
                {
                    if (!ReValidateData())
                    {
                        return;
                    }
                }
            }
            else
            {
                txtDestinationpayerID.Value = "";
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void LoadDestinationPayerIDs(string destPayerID)
    {
        int destPayerIDINT = 0;
        string str = txtDestinationpayerID.Value;
        if (int.TryParse(destPayerID, out destPayerIDINT))
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            DataSet ds2 = _spa.GetDestinationPayer();
            if (Session["DestinationPayer"] == null && ds2 != null && ds2.Tables.Count > 0)
            {
                DataTable dt2 = ds2.Tables[0];
                Session["DestinationPayer"] = dt2;
            }

            ddlSubCapitaPayerIDs.Items.Clear();
            ddlSubCapitaPayerIDs.SelectedIndex = -1;
            try
            {
                DataSet dataSet = _spa.GetSubCapitaPayerIDs(destPayerIDINT);
                DataTable dt = dataSet.Tables[0];
                Helper.LoadList(ddlSubCapitaPayerIDs, dt, "PRIOR_AUTH_SUB_DESTINATION_PAYER_DESC_LRG", "MCE_ID", true);

                if (ddlSubCapitaPayerIDs.Items.Count >= 2)
                {
                    ddlSubCapitaPayerIDs.SelectedIndex = 1;
                }
                ddlSubCapitaPayerIDs.ClearSelection();
                ddlSubCapitaPayerIDs.SelectedValue = str;

            }
            catch (Exception ex)
            {
                string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
                IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            }
        }
    }

    private void LoadDestinationPayerIDsByMCE_ID(string MCE_ID)
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        ddlSubCapitaPayerIDs.Items.Clear();
        try
        {
            ddlSubCapitaPayerIDs.SelectedIndex = -1;
            DataSet dataSet = _spa.GetSubCapitaPayerIDsByMCE_ID(MCE_ID);
            DataTable dt = dataSet.Tables[0];
            Helper.LoadList(ddlSubCapitaPayerIDs, dt, "PRIOR_AUTH_SUB_DESTINATION_PAYER_DESC_LRG", "MCE_ID", true);

            ddlSubCapitaPayerIDs.SelectedValue = MCE_ID;
            txtDestinationpayerID.Value = MCE_ID;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    public override string ValidationGroup
    {
        get { return "valProviderInfoHeader"; }
        // get { return "valProviderHeader"; }
    }


    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        return false;
    }

    protected void ddlplanname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlplanname.SelectedValue != null)
            {
                string errormessage = "*Plan Name";
                if (ddlplanname.SelectedValue == "1")
                    AddValidationErrorMessage(errormessage);
                else if (ddlplanname.SelectedValue == "2")
                    AddValidationErrorMessage(errormessage);

            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }
    protected void ddlSpecialIndicator_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlCBMMXP_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // string isUsedInMMIS = (bool)valuesToUpdate["IS_USED_IN_MMIS"] == true ? "Y" : "N";
            ddlplanname.Attributes.Remove("disabled");
            if (ddlAssignment.SelectedValue == "59" || ddlAssignment.SelectedValue == "60")
            {
                if (ddlCBMMXP.SelectedValue == "2")
                {
                    CBMMXPPannel();
                    ddlplanname.SelectedIndex = 0;
                    ddlplanname.Attributes.Add("disabled", "disabled");
                }
            }
            else
            {
                if (ddlCBMMXP.SelectedValue != null)
                {
                    string errormessage = "Recipient is covered by managed care program";
                    if (ddlCBMMXP.SelectedValue == "1")
                        AddValidationErrorMessage(errormessage);
                    else if (ddlCBMMXP.SelectedValue == "2")
                        CBMMXPPannel();
                    AddValidationErrorMessage(errormessage);
                    //  DivPriorAuth.visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }
    private void CBMMXPPannel()
    {
        pnlsepServiceDetail.Enabled = false;
        pnlServiceDetail.Enabled = false;
        pnlCertHospital.Enabled = false;
        pnlsepCertHospital.Enabled = false;
        pnlsepDentalServiceDetail.Enabled = false;
        pnlDentalServiceDetail.Enabled = false;
        pnlReviewerNotes.Enabled = false;
        pnlProvidermainPnl.Enabled = false;
        //pnlsepProsthodontics.Enabled = false;
        //pnlProsthodontics.Enabled = false;
        pnlSepService.Enabled = false;
        pnlService.Enabled = false;
        //pnlsepDiagnosissearch.Enabled = false;
        //pnlDiagnosissearch.Enabled = false;
        // pnlline.Enabled = false;
        pnlsepline.Enabled = false;
        // pnlDentalLine.Enabled = false;
        pnlsepDentalLine.Enabled = false;
        pnlSepContact.Enabled = false;
        pnlContact.Enabled = false;
        pnlSepTrackingNumber.Enabled = false;
        pnlTrackingNumber.Enabled = false;
        //pnlsepReasonforDenial.Visible = false;
        //sepReasonforDenial.Visible = false;
        // pnlServiceDetail.Visible = false;

        //DivPriorAuth.Enable = false;
    }


    protected void ddlEdentulist_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlPriorPlacement_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlNewPlacement_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddluploadattachment_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlDoctype_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlDiagnosisCodeType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private bool ReValidateData()
    {
        if (!ValidateData())
        {
            valerrormess.Visible = true;
            return false;
        }
        else
        {
            valerrormess.Visible = false;
            return true;
        }
    }
    protected void ddlServiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlServiceType.Attributes.Add("style", "background-color:white");
            if (valerrormess.Visible)
            {
                if (!ReValidateData())
                {
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void View_CancelPriorAuthDentalServiceDetails()
    {
        //this.mpe.Hide();
        //this.den.Hide();
    }
    private void View_SavePriorAuthDentalServiceDetails()
    {

        this.LoadData();
        //  this.mpe.Hide();
        //this.Den.Update();
        //this.den.Hide();
    }
    private void View_CancelPriorAuthServiceDetails()
    {
        //this.sev.Hide();
    }
    private void View_SavePriorAuthServiceDetails()
    {
        // this.LoadData();
        //this.sev.Hide();
    }

    private void View_CancelPriorAuthProvidersNotes()
    {
        this.mpe.Hide();
    }
    private void View_SavePriorAuthProvidersNotes()
    {
        this.LoadData();
        this.mpe.Hide();
    }
    private void View_CancelPriorAuthDiagnosis()
    {

    }
    private void View_SavePriorAuthDiagnosis()
    {
        this.LoadData();
    }

    #region Section
    private enum PopupName { PriorAuthProvidersNotes = 0, PriorAuthServiceDetails = 1, PriorAuthDentalServiceDetails = 2, PriorAuthDiagnosis = 3, PriorAuthDocumentbyMail = 0, PriorAuthAttachment = 1 };
    #endregion
    protected void btnAdd_Click(object sender, CommandEventArgs e)
    {
        try
        {
            this.LoadData(null);
            DataRow dr = null;

            switch (e.CommandName)
            {
                case "Diagnosis":
                    lblNoteTitle.Text = "*DIAGNOSIS";
                    // ucPriorAuthDiagnosis.LoadData(dr);
                    mltPopup.ActiveViewIndex = Convert.ToInt32(PopupName.PriorAuthDiagnosis);
                    //mltPopup.ActiveViewIndex = 0;
                    mpe.Show();
                    break;
                case "ProvidersNotes":
                    lblNoteTitle.Text = "Provider’s Notes";
                    //if (index >= 0 && ctl.DataList.Rows.Count > 0) dr = ctl.DataList.Rows[index];
                    //ucPriorAuthProvidersNotes.LoadData(dr);
                    ucPriorAuthProvidersNotes.LoadData(dr);
                    mltPopup.ActiveViewIndex = Convert.ToInt32(PopupName.PriorAuthProvidersNotes);
                    // mltPopup.ActiveViewIndex = 0;
                    mpe.Show();
                    break;
                case "ServiceDetails":
                    lblNoteTitle.Text = "*SERVICE DETAILS";
                    ucPriorAuthServiceDetails.LoadData(dr);
                    mltPopup.ActiveViewIndex = Convert.ToInt32(PopupName.PriorAuthServiceDetails);
                    //mltPopup.ActiveViewIndex = 0;
                    mpe.Show();
                    break;
                case "DentalServiceDetails":
                    lblNoteTitle.Text = "*Dental SERVICE DETAILS";
                    ucPriorAuthDentalServiceDetails.LoadData(dr);
                    mltPopup.ActiveViewIndex = Convert.ToInt32(PopupName.PriorAuthDentalServiceDetails);
                    //mltDocumentbyMail.ActiveViewIndex = 0;
                    mpe.Show();
                    // BindGrid();
                    break;
                case "Attachment":
                    lblDocumentbyMail.Text = "Attachment";
                    ucPriorAuthAttachment.LoadData(dr);
                    //mltDocumentbyMail.ActiveViewIndex = Convert.ToInt32(PopupName.PriorAuthAttachment);
                    mltDocumentbyMail.ActiveViewIndex = 1;
                    mpedoc.Show();
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }
    protected void lbtnAddAttachment_click(object sender, CommandEventArgs e)
    {

    }
    private void BindGrid(DataTable dataTable)
    {

    }

    //protected void lbtnPrintCoverpage_Click(object sender, CommandEventArgs e)
    //{

    //}
    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancelPriorAuth_Click(object sender, EventArgs e)
    {
        mdlCancelPARequest1.Show();
    }


    protected void btnCancelPrior_Click(object sender, EventArgs e)
    {
        mdlCancelPARequest.Show();
        CloseCleanAll();
    }

    protected void btnCancel_Revert_Click(object sender, EventArgs e)
    {
        try
        {
            InquirePriorAuthResponse iq = (InquirePriorAuthResponse)Session[INQUIRY_RESPONSE_RETENTION_DATA];
            LoadProviderInformationByTransaction(iq);
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-BtnCancelRevert");
            IntuitivePriorAuthMessageBoxID.Show(string.Format("An error has occurred while cancel revert click event. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void btnDocumentMail_Click(object sender, EventArgs e)
    {
        _spa = new PDMSService.PDMSServiceClient();

        try
        {
            Dictionary<string, object> parms = new Dictionary<string, object>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("PRIOR_AUTH_HOSPITAL_ID", "1");
            var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            parms.Add("PRIOR_AUTH_DOCUMENT_TYPE_ID", ddlDoctype.SelectedValue.ToString());
            parms.Add("PRIOR_AUTH_DOCUMENT_MAIL_DESC", txtDocNote.Text.ToString());

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", id.ToString());
            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", id.ToString());
            _spa.InsertPriorAuthDocumentByMail("AUTH_Document_Mail", parms);
            GetDocumentbyMail();
            txtDocNote.Text = "";
            ddlDoctype.SelectedValue = "";
            return;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-BtnDocumentMailClick");
            IntuitivePriorAuthMessageBoxID.Show(string.Format("An error has occurred while document mail click event. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void gvDocumentbyMail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int priorAuthDocumentId = (int)gvDocumentbyMail.DataKeys[index].Values["PRIOR_AUTH_DOCUMENT_MAIL_ID"];
            switch (e.CommandName)
            {
                case "DeleteDocumentMail":
                    _spa = new PDMSService.PDMSServiceClient();
                    _spa.DeleteDocumentMailById(priorAuthDocumentId);
                    GetDocumentbyMail();
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void ClearAll()
    {
        //CreateAuthSA();
        //UpdateAuthSA();
        //InquireAuthSA();
        ClearErrorMessages();
        divSubmitPriorAuth.Visible = false;
        switch (rblClaimType.SelectedValue.ToUpper())
        {
            case "DENTAL":
                ClearDentalDropdownSelection();
                ClearDentalServiceDetailsControls();
                break;
            case "PROFESSIONAL":
                ClearProfessionalDropdownSelection();
                ClearProfessionalServiceDetailsControls();
                break;
            case "INSTITUTIONAL":
                //ClearServiceDetailsControls();
                //clearAllControls();
                lstControl = getControls();
                if (lstControl != null && lstControl.Count > 0)
                {
                    ClearAllPageControls();
                }

                break;
        }

        lblSvcProviderLName.Text = "";
        lblSvcProviderFName.Text = "";
        lblOrdProviderFName.Text = "";
        lblOrdProviderLName.Text = "";

        hdlblSvcProviderLName.Value = "";
        hdtxtMedicaidID.Value = "";
        hdtxtOMID.Value = "";
        hdlblSvcProviderFName.Value = "";
        hdlblOrdProviderFName.Value = "";
        hdlblOrdProviderLName.Value = "";

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string enablePriorAuthSubmitPopUp = Helper.GetAppSettingFromDB("enablePriorAuthSubmitPopUp", string.Empty);
            string enablePriorAuthAttachment = Helper.GetAppSettingFromDB("enablePriorAuthAttachment", string.Empty);


            if (hdnModified.Value != "true")
            {
                MessageBox2.Show("“You must edit prior authorization information before submitting.", "Error");
            }

            var isSubValid = true;
            bool isACKed = false;

            if (Session["isACKed"] != null)
            {
                isACKed = Convert.ToBoolean(Session["isACKed"].ToString());
            }
            else
            {
                isACKed = false;
            }

            if (!ValidateData())
            {
                valerrormess.Visible = true;
                isSubValid = false;
                Session["isSubValid"] = false;
            }

            if (!ValidateRecipientInformationData())
            {
                valerrormess.Visible = true;
                isSubValid = false;
                Session["isSubValid"] = false;
            }

            if (!FrontEndEdits())
            {
                isSubValid = false;
                Session["isSubValid"] = false;
            }

            if (enablePriorAuthAttachment.Equals("true"))
            {
                if (!ValidatePAAttachments())
                {
                    isSubValid = false;
                    Session["isSubValid"] = false;
                }
            }

            if (!isSubValid)
                return;

            if (enablePriorAuthSubmitPopUp.Equals("true"))
            {
                if (!isACKed)
                {
                    if (!WarningMessages())
                    {
                        mdlWarningAcknowledgment.Show();
                    }
                    else
                    {
                        makeAUTransaction();
                    }
                }
                else
                {
                    makeAUTransaction();
                }
            }
            else
            {
                makeAUTransaction();
            }


        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
        finally
        {
            btnSubmit.Enabled = true;
        }
    }


    private void makeAUTransaction()
    {


        try
        {




            Session["isSubValid"] = true;
            makePriorAuthAddUpdateRequest();

            hdnFrontEndEdits.Value = "";
            hdnFrontEndEdits_6.Value = "";
            hdnFrontEndEdits_7.Value = "";
            hdnProvNoteText.Value = "";
            txtProviderNotes.Text = "";
            hdnDiagnosiAddCopy.Value = "false";
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-MakeAUT");
            IntuitivePriorAuthMessageBoxID.Show(string.Format("An error has occurred, please save your prior authorization for future submission. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    public bool ValidateRecipientInformationData()
    {
        bool isValid = true;

        //string firstName = string.Empty;
        //string lastName = string.Empty;
        //string addressLine1 = string.Empty;

        //firstName = Page.Request.Form[txtfrstmi2.UniqueID];
        //lastName = Page.Request.Form[txtLastName2.UniqueID];
        //addressLine1 = Page.Request.Form[txtAddress1.UniqueID];


        //string frstName = txtfrstmi2.Text;
        //string lstName = txtLastName2.Text;
        //string addrs = txtAddress1.Text;

        bool recipientStatus = Convert.ToBoolean(hdnRecipientCallStatus.Value);

        //(string.IsNullOrEmpty(txtMedicaidBillingNumber.Text) ||
        //    string.IsNullOrEmpty(firstName) ||
        //    string.IsNullOrEmpty(lastName) ||
        //    string.IsNullOrEmpty(txtBirthDate.Text) ||
        //    string.IsNullOrEmpty(addressLine1))

        //Common submission errors for all PA types
        if (!recipientStatus && failureValidations <= maxValidations)
        {
            ++failureValidations;
            ddlAuthorization.Attributes.Add("style", "background-color:yellow");
            ddlAuthorization.Width = 200;
            foreach (ListItem li in ddlAuthorization.Items)
            {
                li.Attributes.Add("style", "background-color:white");
            }

            AddValidationErrorMessage("<a href='#' class='errMsg' id='noCpe_noPnl_ddlAuthorization' onclick='javascript:focusError(this.id)'>*Recipient information is required</a>");
            isValid = false;
        }

        return isValid;
    }

    private string getFileList(IList<ProcessedDocument> UploadedFileNames)
    {
        string processedFileList = Processed_Files;
        string maliciousFileList = Malicious_Files;

        if (UploadedFileNames.Where(c => c.isMalicious == false).Any())
        {
            processedFileList = string.Format("<p style='color:green;'> {0} </p><ul>", processedFileList);

            foreach (var item in UploadedFileNames.Where(c => c.isMalicious == false))
            {
                processedFileList = string.Format("{0}<li>{1}</li>", processedFileList, item.orginalFileName);
            }
            processedFileList = string.Format("{0} </ul>", processedFileList);
        }

        //Malicious file exists
        if (UploadedFileNames.Where(c => c.isMalicious == true).Any())
        {
            maliciousFileList = string.Format("<p style='color:red;'> {0} </p><ul>", maliciousFileList);

            foreach (var item in UploadedFileNames.Where(c => c.isMalicious == true))
            {
                maliciousFileList = string.Format("{0}<li>{1}</li>", maliciousFileList, item.orginalFileName);
            }
            maliciousFileList = string.Format("{0} </ul>", maliciousFileList);
        }

        return string.Format("<p>{0}</p> <p>{1}</p>", processedFileList, maliciousFileList);
    }

    private void ProcessAttachmentControlFile(int transactionID)
    {
        string tradingPartnerID = GetTradingPartnerID();
        string documentType = string.Empty;
        string timeStamp = string.Empty;
        string originalFileName = string.Empty;
        string fileName = string.Empty;
        string documentId = string.Empty;
        DataSet ds = null;
        DataTable dataTable = null;

        string methodName = "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString();

        List<PriorAuthAttachment> dentalAttachments = FetchAttachments();



        dataTable = Helper.ConvertToDataTable(dentalAttachments);

        ProcessAttachments attachments = new ProcessAttachments();

        foreach (DataRow dr in dataTable.Rows)
        {
            documentType = dr["PRIOR_AUTH_SUB_DOCUMENT_TYPE_DESC"].ToString();
            originalFileName = dr["ORIGINAL_DOCUMENT_NAME"].ToString();
            fileName = dr["DOCUMENT_NAME"].ToString();
            documentId = dr["DOCUMENT_ID"].ToString();

            if (string.IsNullOrEmpty(fileName))
            {
                CreateAndReturnLogInfoThreadNumber(methodName + "File name can not be empty. fileName: " + fileName + ", originalFileName: " + originalFileName);
            }

            /*if (!string.IsNullOrEmpty(fileName) && !attachments.IsFileExistsInS3(fileName))
            {
                UploadedFileNames.Add(new ProcessedDocument(originalFileName, fileName, true));
            }
            else
            {
            UploadedFileNames.Add(new ProcessedDocument(originalFileName, fileName, false));
            }

            SendControlFileToAWS(fileName, documentId, tradingPartnerID, documentType, timeStamp);*/

            //Update Documents to be sent to SI
            ProcessAttachments att = new ProcessAttachments();
            att.UpdateFileStatusToSend(string.IsNullOrEmpty(documentId) ? 0 : Convert.ToInt32(documentId), transactionID);
        }
    }

    private bool FrontEndEdits()
    {

        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }

        bool isValid = true;
        try
        {
            string reqMedID = this.WorkflowPage.MedicaidID;
            string RequestNPI = string.Empty;
            string RequestName = string.Empty;

            if (!string.IsNullOrEmpty(MedicaidId))
            {
                DataSet ds = _spa.SelectProviderByGRPMedicaidID(MedicaidId);
                DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
                this.DataList = dtMisc;
                if (Helper.HasRows(dtMisc))
                {
                    DataRow dr = dtMisc.Rows[0];
                    RequestNPI = Helper.GetString("NPI", dr);
                    RequestName = Helper.GetString("NAME", dr);
                }
            }
            if (!ValidateRequestProvEnrollStatus(reqMedID))
            {
                isValid = false;
                AddValidationErrorMessage("Provider not eligible to request the service for the requested timeframe");
            }

            //Business Rule 4 -Validate Serviceprovider enrollment status
            if (!ValidateServProvEnrollStatus(txtSPNPI.Text))
            {
                isValid = false;
                AddValidationErrorMessage(string.Format("Service Provider {0} no longer affiliated with this agency.", txtSPNPI.Text));
            }

            if (GetClaimTypeID() == Convert.ToInt32(CON.ClaimsType.Institutional))
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
                parms.Add("LINK_SECTIONS", SaveCodeLNK);
                DataSet ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                if (ds != null)
                {
                    var dataTable = ds.Tables[0];

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        int line = 0;
                        for (int row = 0; row < dataTable.Rows.Count; row++)
                        {
                            line = line + 1;
                            var instFDOS = dataTable.Rows[row]["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"].ToString();
                            var instTDOS = dataTable.Rows[row]["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"].ToString();
                            var lineNumber = line;
                            if (!ValidateBusinessRules(instFDOS, instTDOS, lineNumber))
                            {
                                isValid = false;
                            }

                            int cnt = PriorAuthHospitalController.GetPriorAuthAssignmentProcGrp(ddlAssignment.SelectedValue, dataTable.Rows[row]["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"].ToString());
                            if (cnt == 0)
                            {
                                AddValidationErrorMessage(string.Format("Procedure Code {0} is not valid for PA Assignment. ", dataTable.Rows[row]["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"].ToString()));
                                isValid = false;
                            }
                        }
                    }
                }
            }
            else if (GetClaimTypeID() == Convert.ToInt32(CON.ClaimsType.Professional))
            {

                Dictionary<string, string> parms2 = new Dictionary<string, string>();
                parms2.Add("PRIOR_AUTH_TYPE", returnPATypeID());
                parms2.Add("LINK_SECTIONS", SaveCodeLNK);
                DataSet ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms2);
                if (ds != null)
                {
                    var dataTable = ds.Tables[0];

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        int line = 0;

                        for (int row = 0; row < dataTable.Rows.Count; row++)
                        {
                            line = line + 1;
                            var profFDOS = dataTable.Rows[row]["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"].ToString();
                            var profTDOS = dataTable.Rows[row]["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"].ToString();
                            var lineNumber = line;
                            if (!ValidateBusinessRules(profFDOS, profTDOS, lineNumber))
                            {
                                isValid = false;
                            }

                            int cnt = PriorAuthHospitalController.GetPriorAuthAssignmentProcGrp(ddlAssignment.SelectedValue, dataTable.Rows[row]["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString());
                            if (cnt == 0)
                            {
                                AddValidationErrorMessage(string.Format("Procedure Code {0} is not valid for PA Assignment. ", dataTable.Rows[row]["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString()));
                                isValid = false;
                            }
                        }
                    }
                }
            }
            else
            {
                Dictionary<string, string> parms3 = new Dictionary<string, string>();
                parms3.Add("PRIOR_AUTH_TYPE", returnPATypeID());
                parms3.Add("LINK_SECTIONS", SaveCodeLNK);
                DataSet ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms3);
                if (ds != null)
                {
                    var dataTable = ds.Tables[0];

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        int line = 0;

                        for (int row = 0; row < dataTable.Rows.Count; row++)
                        {
                            line = line + 1;
                            var denFDOS = dataTable.Rows[row]["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"].ToString();
                            var denTDOS = dataTable.Rows[row]["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"].ToString();
                            var lineNumber = line;
                            if (!ValidateBusinessRules(denFDOS, denTDOS, lineNumber))
                            {
                                isValid = false;
                            }

                            int cnt = PriorAuthHospitalController.GetPriorAuthAssignmentProcGrp(ddlAssignment.SelectedValue, dataTable.Rows[row]["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString());
                            if (cnt == 0)
                            {
                                AddValidationErrorMessage(string.Format("Procedure Code {0} is not valid for PA Assignment. ", dataTable.Rows[row]["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString()));
                                isValid = false;
                            }
                        }
                    }
                }
            }

            var hasDiagnisData = false;

            Dictionary<string, string> parms9 = new Dictionary<string, string>();
            parms9.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms9.Add("MedicaidID", MedicaidNumber);
            parms9.Add("LINK_SECTIONS", SaveCodeLNK);
            DataSet dsDTD = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_DIAGNOSIS", parms9);

            if (dsDTD != null && Helper.HasRows(dsDTD))
            {
                hasDiagnisData = true;
            }

            List<string> checkValues = new List<string> { "03", "20", "18" };
            if (!checkValues.Contains(ddlAssignment.SelectedValue) && !hasDiagnisData)
            {
                AddValidationErrorMessage("<a href='#' class='errMsg' id='cpeDiagnosis_pnlsepDiagnosis_lblDiagnosisNoData' onclick='javascript:focusPanel(this.id)'>*Diagnosis code is required.</a>");
                isValid = false;
            }

            //OHPNM-16868 
            if (!string.IsNullOrEmpty(alrt_4))
            {
                ++failureValidations;
                AddValidationErrorMessage("Recipient is not eligible for services on the requested service date indicated on the PA.");
                isValid = false;
                alrt_4 = string.Empty;
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-BtnCopyClick");
            return isValid;
        }
        return isValid;
    }

    private DataSet GetServProvDateOfService()
    {
        DataSet ds = new DataSet();
        try
        {
            if (txtSPNPI.Text != null)
            {
                ds = PriorAuthHospitalController.GetServProvDateOfService(txtSPNPI.Text, txtMedicaidID.Text);
                return ds;
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetServiceProviderDateOfService");
            return ds;
        }
        return ds;
    }

    private DataSet GetOrderingProvDateOfService()
    {
        DataSet ds = new DataSet();
        try
        {
            if (txtOMID.Text != null)
            {
                ds = PriorAuthHospitalController.GetOrdProvDateOfService(txtOMID.Text);
                return ds;
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetOrderingProviderDateOfService");
            return ds;
        }
        return ds;
    }

    private DataSet GetRequestingProvDateOfService()
    {
        DataSet ds = new DataSet();
        try
        {
            if (txtOMID.Text != null)
            {
                ds = PriorAuthHospitalController.GetOrdProvDateOfService(this.WorkflowPage.MedicaidID);
                return ds;
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetOrderingProviderDateOfService");
            return ds;
        }
        return ds;
    }

    private bool ValidateRequestProvEnrollStatus(string RequestProviderMedID)
    {
        bool isValid = false;
        try
        {
            if (RequestProviderMedID != null)
            {
                isValid = PriorAuthHospitalController.ValidateReqProvEnrollStatus(RequestProviderMedID);
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
        return isValid;
    }

    private bool ValidateServProvEnrollStatus(string ServiceProvider)
    {
        bool isValid = true;
        try
        {
            if (txtSPNPI.Text != null)
            {
                isValid = PriorAuthHospitalController.ValidateSvcProvEnrollStatus(ServiceProvider);
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
        return isValid;
    }

    private bool WarningMessages()
    {
        bool isValid = true;
        var claimType = GetClaimTypeID();
        DataSet dssd = new DataSet();
        int columnIndex = 0;
        DataTable dataTablesd;
        lblWarningAcknowledgment.Text = string.Empty;
        string paFEEdit = Helper.GetAppSettingFromDB("PriorAuthFEEditOH16868", "true");

        if (claimType == Convert.ToInt32(CON.ClaimsType.Dental))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms.Add("LINK_SECTIONS", SaveCodeLNK);
            dssd = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
            dataTablesd = dssd.Tables[0];
            columnIndex = dataTablesd.Columns.IndexOf("PRIOR_AUTH_PROCEDURE_CODE_ID");
        }
        else if (claimType == Convert.ToInt32(CON.ClaimsType.Professional))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms.Add("LINK_SECTIONS", SaveCodeLNK);
            dssd = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
            dataTablesd = dssd.Tables[0];
            columnIndex = dataTablesd.Columns.IndexOf("PRIOR_AUTH_PROCEDURE_CODE_ID");
        }
        else
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms.Add("LINK_SECTIONS", SaveCodeLNK);
            dssd = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
            dataTablesd = dssd.Tables[0];
            columnIndex = dataTablesd.Columns.IndexOf("PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE");
        }

        foreach (DataRow dr in dataTablesd.Rows)
        {
            int cnt = PriorAuthHospitalController.GetPriorAuthAssignmentProcGrp(ddlAssignment.SelectedValue, dr[columnIndex].ToString());
            if (cnt != 999) //Arbitary value so we know there is no records in lookup table with that Assignment Type
            {
                //True, when there is a match for each service details procedure code + assignment type in [PA_ASSIGN_PROCEDURE_GRP]
                //Shows a warning message
                if (claimType == Convert.ToInt32(CON.ClaimsType.Dental) || claimType == Convert.ToInt32(CON.ClaimsType.Professional))
                {
                    lblWarningAcknowledgment.Text = lblWarningAcknowledgment.Text + "\u2022 This service may not require prior authorization. Please ensure the requested service or procedure " + dr[columnIndex].ToString() + " requires prior authorization. <br />";
                }
                else
                {
                    lblWarningAcknowledgment.Text = lblWarningAcknowledgment.Text + "\u2022 This service may not require prior authorization. Please ensure the requested service or procedure " + dr[columnIndex].ToString() + " requires prior authorization.  <br />";
                }
            }
        }
        if (paFEEdit.Equals("true"))
        {
            if (!string.IsNullOrEmpty(alrt_5))
            {
                alrt_5 = alrt_5.TrimEnd(new char[] { ',' });
                List<string> uniques = alrt_5.Split(',').Distinct().ToList();
                string newStr = string.Join(",", uniques);
                lblWarningAcknowledgment.Text = lblWarningAcknowledgment.Text + string.Format("\u2022 Recipient may not be eligible for Medicaid services on the requested service date indicated on the PA Service Line of {0}. <br />", newStr);
                alrt_5 = string.Empty;
            }
        }
        if (!string.IsNullOrEmpty(alrt_6))
        {
            alrt_6 = alrt_6.TrimEnd(new char[] { ',' });
            lblWarningAcknowledgment.Text = lblWarningAcknowledgment.Text + string.Format("\u2022 Recipient is enrolled in Managed Care during the requested timeframe on the PA Service Line of - {0}. <br />", alrt_6);
            alrt_6 = string.Empty;
        }
        if (!string.IsNullOrEmpty(alrt_7))
        {
            string message = string.Empty;
            alrt_7 = alrt_7.TrimEnd(new char[] { ',' });
            lblWarningAcknowledgment.Text = lblWarningAcknowledgment.Text + string.Format("\u2022 The Recipient is enrolled in managed care during the requested time frame. Please verify the Recipient was enrolled in FFS Medicaid on the date of admission before completing this request for Service Line -  {0}. <br />", alrt_7);
            alrt_7 = string.Empty;
        }

        if (lblWarningAcknowledgment.Text.Length > 1)
        {
            isValid = false;
        }
        return isValid;
    }


    private bool ValidateBusinessRules(string profFDOS, string profTDOS, int lineNumber)
    {
        bool flag = true;
        int flagCounter = 0;
        int flagCNT = 0;
        int enrollingFlagCounter = 0;
        DataSet enrollData = GetServProvDateOfService();
        DataSet enrollOrderingData = GetOrderingProvDateOfService();
        DataSet enrollRequestingData = GetRequestingProvDateOfService();
        bool enableCR537 = Convert.ToBoolean(AppSettings.Get("EnableCR537", "false"));
        RecipientEligibilitySearchResponse res = null;

        if (Session["RecipientEligibilityResponse"] != null)
            res = (RecipientEligibilitySearchResponse)Session["RecipientEligibilityResponse"];
        else
        {
            string medicaidID = this.WorkflowPage.MedicaidID;
            Guid userId = Helper.GetUserId(HttpContext.Current.User.Identity.Name.ToString());
            res = new RecipientEligibilitySearchReqRes().SearchRequest(string.Empty, medicaidID, userId, txtBirthDate.Text, DateTime.Now.AddMonths(-48).ToString("MM/dd/yyyy"), DateTime.Now.ToString("MM/dd/yyyy"), string.Empty, txtMedicaidBillingNumber.Text.Trim(), "PriorAuthEligibility");
            Session["RecipientEligibilityResponse"] = res;
        }

        if (enableCR537 && enrollRequestingData != null && enrollRequestingData.Tables.Count > 0 && (enrollRequestingData.Tables[0].Rows.Count > 0 || enrollRequestingData.Tables[1].Rows.Count > 0))
        {
            if (enrollRequestingData.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < enrollRequestingData.Tables[1].Rows.Count; i++)
                {
                    if ((Convert.ToDateTime(profFDOS) >= Convert.ToDateTime(enrollRequestingData.Tables[1].Rows[i][0].ToString())
                                            && Convert.ToDateTime(profFDOS) <= Convert.ToDateTime(enrollRequestingData.Tables[1].Rows[i][1].ToString()))
                                            || (Convert.ToDateTime(profTDOS) >= Convert.ToDateTime(enrollRequestingData.Tables[1].Rows[i][0].ToString())
                                            && Convert.ToDateTime(profTDOS) <= Convert.ToDateTime(enrollRequestingData.Tables[1].Rows[i][1].ToString())))
                    {
                        AddValidationErrorMessage(string.Format("The date of service entered is a suspended span and does not allow PA submission on the PA Service Line of {0}", lineNumber));
                        flag = false;
                    }
                }
            }
        }

        //Business rule 5
        if (!string.IsNullOrEmpty(txtSPNPI.Text) && !string.IsNullOrWhiteSpace(txtSPNPI.Text))
        {
            if (enrollData != null && enrollData.Tables.Count > 0 && (enrollData.Tables[0].Rows.Count > 0 || enrollData.Tables[1].Rows.Count > 0))
            {
                if (enableCR537 && enrollData.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < enrollData.Tables[1].Rows.Count; i++)
                    {
                        if ((Convert.ToDateTime(profFDOS) >= Convert.ToDateTime(enrollData.Tables[1].Rows[i][0].ToString())
                                            && Convert.ToDateTime(profFDOS) <= Convert.ToDateTime(enrollData.Tables[1].Rows[i][1].ToString()))
                                            || (Convert.ToDateTime(profTDOS) >= Convert.ToDateTime(enrollData.Tables[1].Rows[i][0].ToString())
                                            && Convert.ToDateTime(profTDOS) <= Convert.ToDateTime(enrollData.Tables[1].Rows[i][1].ToString())))
                        {
                            AddValidationErrorMessage(string.Format("The date of service entered is a suspended span and does not allow PA submission on the PA Service Line of {0}", lineNumber));
                            flag = false;
                        }
                    }
                }

                if (enrollData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < enrollData.Tables[0].Rows.Count; i++)
                    {
                        if (!((Convert.ToDateTime(profFDOS) >= Convert.ToDateTime(enrollData.Tables[0].Rows[i][0].ToString())
                                                && Convert.ToDateTime(profFDOS) <= Convert.ToDateTime(enrollData.Tables[0].Rows[i][1].ToString())
                                                && (Convert.ToDateTime(profTDOS) >= Convert.ToDateTime(enrollData.Tables[0].Rows[i][0].ToString())
                                                && Convert.ToDateTime(profTDOS) <= Convert.ToDateTime(enrollData.Tables[0].Rows[i][1].ToString())))))
                        {
                            AddValidationErrorMessage(string.Format("Provider not eligible to provide the service as of today's date on the PA Service Line of {0}", lineNumber));
                            flag = false;
                        }
                    }
                }
            }
            else
            {
                AddValidationErrorMessage(string.Format("Provider not eligible to provide the service as of today's date on the PA Service Line of {0}", lineNumber));
                flag = false;
            }
        }

        //Business rule X
        if (!string.IsNullOrEmpty(txtOMID.Text) && !string.IsNullOrWhiteSpace(txtOMID.Text))
        {
            if (enrollOrderingData != null && enrollOrderingData.Tables.Count > 0 && (enrollOrderingData.Tables[0].Rows.Count > 0 || enrollOrderingData.Tables[1].Rows.Count > 0))
            {
                if (enableCR537 && enrollOrderingData.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < enrollOrderingData.Tables[1].Rows.Count; i++)
                    {
                        if ((Convert.ToDateTime(profFDOS) >= Convert.ToDateTime(enrollOrderingData.Tables[1].Rows[i][0].ToString())
                                            && Convert.ToDateTime(profFDOS) <= Convert.ToDateTime(enrollOrderingData.Tables[1].Rows[i][1].ToString()))
                                            || (Convert.ToDateTime(profTDOS) >= Convert.ToDateTime(enrollOrderingData.Tables[1].Rows[i][0].ToString())
                                            && Convert.ToDateTime(profTDOS) <= Convert.ToDateTime(enrollOrderingData.Tables[1].Rows[i][1].ToString())))
                        {
                            AddValidationErrorMessage(string.Format("The date of service entered is a suspended span and does not allow PA submission on the PA Service Line of {0}", lineNumber));
                            flag = false;
                        }
                    }
                }
                if (enrollOrderingData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < enrollOrderingData.Tables[0].Rows.Count; i++)
                    {
                        if (!((Convert.ToDateTime(profFDOS) >= Convert.ToDateTime(enrollOrderingData.Tables[0].Rows[i][0].ToString())
                                                && Convert.ToDateTime(profFDOS) <= Convert.ToDateTime(enrollOrderingData.Tables[0].Rows[i][1].ToString())
                                                && (Convert.ToDateTime(profTDOS) >= Convert.ToDateTime(enrollOrderingData.Tables[0].Rows[i][0].ToString())
                                                && Convert.ToDateTime(profTDOS) <= Convert.ToDateTime(enrollOrderingData.Tables[0].Rows[i][1].ToString())))))
                        {
                            AddValidationErrorMessage(string.Format("The Ordering Provider is not an active provider in the Ohio Medicaid Program for the requested timeframe or the Ordering Provider is not an active Provider in the Ohio Medicaid Program for the authorized timeframe indicated on the PA Service Line of {0}", lineNumber));
                            enrollingFlagCounter++;
                            flag = false;
                        }
                    }
                }
            }
            else
            {
                AddValidationErrorMessage(string.Format("The Ordering Provider is not an active provider in the Ohio Medicaid Program for the requested timeframe or the Ordering Provider is not an active Provider in the Ohio Medicaid Program for the authorized timeframe indicated on the PA Service Line of {0}", lineNumber));
                enrollingFlagCounter++;
                flag = false;
            }
        }

        if (res != null && res.ErrorDetails.Count == 0)
        {

            if (res != null && res.BenifitAssignmentPlans != null && res.BenifitAssignmentPlans.Count > 0)
            {
                for (int assPlan = 0; assPlan < res.BenifitAssignmentPlans.Count; assPlan++)
                {
                    var baEffectiveDate = res.BenifitAssignmentPlans[assPlan].EffectiveDate;
                    var baEndDate = res.BenifitAssignmentPlans[assPlan].EndDate;
                    DateTime baEndDateDT = DateTime.Now;
                    if (baEndDate.HasValue)
                    {
                        string baEndDateST = baEndDate.ToString();
                        baEndDateDT = DateTime.Parse(baEndDateST);
                    }
                    var baPlan = res.BenifitAssignmentPlans[assPlan].AssignmentPlan;

                    try
                    {                     //Business rule 1
                        if (res.ManagedCarePlans != null && !string.IsNullOrEmpty(res.ManagedCarePlans[assPlan].PlanDescription) && res.ManagedCarePlans[assPlan].PlanDescription.Contains("MyCare"))
                        {
                            if (((Convert.ToDateTime(profFDOS) >= res.ManagedCarePlans[assPlan].EffectiveDate
                                && Convert.ToDateTime(profFDOS) <= res.ManagedCarePlans[assPlan].EndDate)
                                && (Convert.ToDateTime(profTDOS) >= res.ManagedCarePlans[assPlan].EffectiveDate
                                && Convert.ToDateTime(profTDOS) <= res.ManagedCarePlans[assPlan].EndDate)))
                            {
                                AddValidationErrorMessage(string.Format("Recipient is active in MyCare Ohio program. Contact plan for authorization on the PA Service Line of {0}", lineNumber));
                                flag = false;
                            }
                        }
                    }
                    catch (Exception ex) { }


                    try
                    {
                        if (baEndDate != null)
                        {
                            //if false then revert the warning message
                            string paFEEdit = Helper.GetAppSettingFromDB("PriorAuthFEEditOH16868", "false");
                            if (paFEEdit.Equals("true"))
                            {
                                if ((Convert.ToDateTime(profFDOS) >= baEffectiveDate && Convert.ToDateTime(profFDOS) <= baEndDateDT.AddDays(365)
                                && Convert.ToDateTime(profTDOS) >= baEffectiveDate && Convert.ToDateTime(profTDOS) <= baEndDateDT.AddDays(365)))
                                {
                                    alrt_5 += lineNumber + ",";
                                }
                                if ((Convert.ToDateTime(profFDOS) > baEffectiveDate && Convert.ToDateTime(profTDOS) > baEndDateDT.AddDays(365)))
                                {
                                    flagCNT++;
                                }
                            }
                            else
                            {
                                if (!(Convert.ToDateTime(profFDOS) >= baEffectiveDate && Convert.ToDateTime(profFDOS) <= baEndDate
                                && Convert.ToDateTime(profTDOS) >= baEffectiveDate && Convert.ToDateTime(profTDOS) <= baEndDate))
                                {
                                    flagCounter++;
                                }
                            }


                            if (baPlan.Contains("ACT") && ddlAssignment.SelectedValue == "47"
                                && (((Convert.ToDateTime(profFDOS) >= baEffectiveDate && Convert.ToDateTime(profFDOS) <= baEndDate)
                                && (Convert.ToDateTime(profTDOS) >= baEffectiveDate && Convert.ToDateTime(profTDOS) <= baEndDate))))
                            {
                                AddValidationErrorMessage(string.Format("Recipient is active in ACT during the requested timeframe on the PA Service Line of {0}", lineNumber));
                                flag = false;
                            }
                        }
                        else
                        {
                            if ((Convert.ToDateTime(profFDOS) < baEffectiveDate || Convert.ToDateTime(profTDOS) < baEffectiveDate))
                            {
                                flagCounter++;
                                flag = false;
                            }
                        }
                    }
                    catch (Exception ex) { }


                    try
                    {                     //BusinessRule 6
                        if ((ddlAssignment.SelectedItem.Value == "46" || ddlAssignment.SelectedItem.Value == "59" || ddlAssignment.SelectedItem.Value == "60")
                            && (res.ManagedCarePlans != null && !string.IsNullOrEmpty(res.ManagedCarePlans[assPlan].PlanDescription)
                            && ReceipientMangCarePlanDesc.Contains(res.ManagedCarePlans[assPlan].PlanDescription)))
                        {
                            if (Convert.ToDateTime(profFDOS) >= res.ManagedCarePlans[assPlan].EffectiveDate
                                && Convert.ToDateTime(profFDOS) <= res.ManagedCarePlans[assPlan].EndDate
                                && Convert.ToDateTime(profTDOS) >= res.ManagedCarePlans[assPlan].EffectiveDate
                                && Convert.ToDateTime(profTDOS) <= res.ManagedCarePlans[assPlan].EndDate
                                && hdnFrontEndEdits_6.Value != "1")
                            {
                                alrt_6 += lineNumber + ",";
                            }
                        }
                    }
                    catch (Exception ex) { }


                    try
                    {                     //BusinessRule 7
                        if (ddlAuthorization.SelectedItem.Text.Equals("Ohio Department of Medicaid")
                            && res.ManagedCarePlans != null && !string.IsNullOrEmpty(res.ManagedCarePlans[assPlan].PlanDescription)
                            && ReceipientMangCarePlanDesc.Contains(res.ManagedCarePlans[assPlan].PlanDescription)
                            && ddlAssignment.SelectedItem.Value == "34")
                        {
                            if (Convert.ToDateTime(profFDOS) >= res.ManagedCarePlans[assPlan].EffectiveDate
                                && Convert.ToDateTime(profFDOS) <= res.ManagedCarePlans[assPlan].EndDate
                                && Convert.ToDateTime(profTDOS) >= res.ManagedCarePlans[assPlan].EffectiveDate
                                && Convert.ToDateTime(profTDOS) <= res.ManagedCarePlans[assPlan].EndDate
                                && hdnFrontEndEdits_7.Value != "1")
                            {
                                alrt_7 += lineNumber + ",";
                            }
                        }
                    }
                    catch (Exception ex) { }

                }

                try
                {
                    if (flagCounter == res.BenifitAssignmentPlans.Count) //OHPNM-7830
                    {
                        AddValidationErrorMessage(string.Format("Recipient is not eligible for Medicaid services on the requested service date indicated on the PA Service Line of {0}", lineNumber));
                        flag = false;
                    }
                    if (flagCNT == res.BenifitAssignmentPlans.Count)
                    {
                        alrt_4 += lineNumber + ",";
                        flag = false;
                    }
                }
                catch (Exception ex) { }


                try
                {
                    if (enrollOrderingData != null && enrollOrderingData.Tables.Count > 0 && enrollingFlagCounter == enrollOrderingData.Tables[0].Rows.Count && enrollingFlagCounter > 0) // OHPNM-8392
                    {
                        AddValidationErrorMessage(string.Format("The Ordering Provider is not an active provider in the Ohio Medicaid Program for the requested timeframe or the Ordering Provider is not an active Provider in the Ohio Medicaid Program for the authorized timeframe indicated on the PA Service Line of {0}", lineNumber));
                        flag = false;
                    }
                }
                catch (Exception ex) { }


            }
            else if (res != null && res.BenifitAssignmentPlans.Count == 0)
            {
                AddValidationErrorMessage(string.Format("Recipient is not eligible for Medicaid services on the requested service date indicated on the PA Service Line of {0}", lineNumber));
                flag = false;
            }

            //OHPNM-17086 - commented on purpose.
            //try
            //{             //BusinessRule 8
            //    if ((ddlAuthorization.SelectedItem.Text.Equals("Ohio Department of Medicaid"))
            //                && (res != null && res.LTCFPlacements != null && res.LTCFPlacements.Count > 0))
            //    {
            //        for (int ltcPlan = 0; ltcPlan < res.LTCFPlacements.Count; ltcPlan++)
            //        {
            //            var baEffectiveDate = res.LTCFPlacements[ltcPlan].EffectiveDate;
            //            var baEndDate = res.LTCFPlacements[ltcPlan].EndDate;

            //            if (Convert.ToDateTime(profFDOS) >= res.LTCFPlacements[ltcPlan].EffectiveDateMedicaidCoverage
            //                && Convert.ToDateTime(profFDOS) <= res.LTCFPlacements[ltcPlan].EndDateMedicaidCoverage
            //                && Convert.ToDateTime(profTDOS) >= res.LTCFPlacements[ltcPlan].EffectiveDateMedicaidCoverage
            //                && Convert.ToDateTime(profTDOS) <= res.LTCFPlacements[ltcPlan].EndDateMedicaidCoverage)
            //            {
            //                AddValidationErrorMessage(string.Format("According to the Medicaid Recipient file, the Recipient resides in a long-term care facility for the service date authorized. The procedure code entered on the Prior Authorization request is reimbursable through the facility's cost report on the PA Service Line of {0}", lineNumber));
            //                flag = false;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex) { }

        }
        return flag;
    }



    private void CreateAuthSA()
    {
        try
        {
            var modTranactId = "";
            var responseCode = "";
            var responseType = "";
            var odsAuthId = "";
            var responseMessage = "";
            var responseDetails = "";
            var createdBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

            //get the data from xml
            var dataset = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
            var dtAttachment = (dataset.Tables["Attachments"] != null) ? dataset.Tables["Attachments"] : null;
            var dtAuthDiagnosis = (dataset.Tables["AuthorizationDiagnosis"] != null) ? dataset.Tables["AuthorizationDiagnosis"] : null;
            var dtAuthInfo = (dataset.Tables["AuthorizationInfo"] != null) ? dataset.Tables["AuthorizationInfo"] : null;
            var dtProvNote = (dataset.Tables["ProviderNotes"] != null) ? dataset.Tables["ProviderNotes"] : null;
            var dtAuthService = (dataset.Tables["AuthorizationService"] != null) ? dataset.Tables["AuthorizationService"] : null;

            //For Validation
            string memberId = Convert.ToString(dtAuthInfo.Rows[0]["MemberID"]);
            string provId = Convert.ToString(dtAuthInfo.Rows[0]["ProviderID"]);
            string referredProvId = Convert.ToString(dtAuthInfo.Rows[0]["MemberID"]);
            DateTime AuthEndDate = Convert.ToDateTime(dtAuthInfo.Rows[0]["AuthorizationEndDate"]);
            string renderProvId = Convert.ToString(dtAuthInfo.Rows[0]["RenderingProviderID"]);//i dont find it 
            DateTime AuthBeginDate = Convert.ToDateTime(dtAuthInfo.Rows[0]["AuthorizationBeginDate"]);
            DateTime serviceEndDate = Convert.ToDateTime(dtAuthService.Rows[0]["ServiceEndDate"]);
            DateTime serviceBeginDate = Convert.ToDateTime(dtAuthService.Rows[0]["ServiceStartDate"]);

            if (ValidateCreateAuthSA(memberId, provId, referredProvId, renderProvId, AuthEndDate
                , AuthBeginDate, serviceEndDate, serviceBeginDate))
            {

                #region Message header

                // I have to  ask where this one we have to  fill up.. 
                var dt = (dataset.Tables["MessageHeader"] != null) ? dataset.Tables["MessageHeader"] : null;

                var messageHeader = new MessageHeader()
                {
                    ModuleTransactionId = dt.Rows[0]["ModuleTransactionId"].ToString(),
                    AdditionalModuleTransactionId = dt.Rows[0]["AdditionalModuleTransactionId"].ToString(),
                    //BusinessFlow = dt.Rows[0]["BusinessFlow"],
                    //BusinessFlowSpecified = "",

                    RequestorSystem = (MessageHeaderRequestorSystem)dt.Rows[0]["RequestorSystem"],
                    RequestTimestamp = dt.Rows[0]["RequestTimestamp"].ToString(),
                    SITransactionKey = dt.Rows[0]["SITransactionKey"].ToString(),
                    StateCode = (MessageHeaderStateCode)dt.Rows[0]["StateCode"],
                    SubscriberSystem = (MessageHeaderSubscriberSystem)dt.Rows[0]["SubscriberSystem"]
                };


                #endregion

                #region Attachments & AuthorizationDiagnosis

                var numAttachments = dtAttachment.Rows.Count;
                var attachments = new Attachments[numAttachments];

                for (int i = 0; i < numAttachments; i++)
                {
                    var item = new Attachments();
                    item.AttachmentID = Convert.ToString(dtAttachment.Rows[0]["AttachmentID"]);
                    item.AttachmentType = Convert.ToString(dtAttachment.Rows[0]["AttachmentType"]);
                    // item.CreatedBy = createdBy.ToString();
                    //item.CreatedDate = DateTime.Now.ToString();
                    // item.LastModifiedBy = createdBy.ToString();
                    //item.LastModifiedDate = DateTime.Now.ToString();
                    // item.ODSAttachmentID = Convert.ToString(dtAttachment.Rows[0]["ODSAttachmentID"]);
                    item.AttachmentDate = DateTime.Now.ToString();
                    attachments[i] = item;
                }

                var numAuthDiagnosis = dtAuthDiagnosis.Rows.Count;
                var authDiagnosis = new AuthorizationDiagnosis[numAuthDiagnosis];
                for (int i = 0; i < numAuthDiagnosis; i++)
                {
                    var item = new AuthorizationDiagnosis();
                    item.ICDType = Convert.ToString(dtAuthDiagnosis.Rows[0]["ICDType"]);//ddlICDVersion.selectvalue; // ddlICDVersion is cmg  PriorAuthDiagnosisSeach
                    item.DiagnosisCode = Convert.ToString(dtAuthDiagnosis.Rows[0]["DiagnosisCode"]);// txtDiagnosisCode PriorAuthDiagnosis.ascx
                    item.CreatedBy = createdBy.ToString();
                    item.CreatedDate = DateTime.Now.ToString();
                    item.LastModifiedBy = createdBy.ToString();
                    item.LastModifiedDate = DateTime.Now.ToString();
                    item.DiagnosisTypeCode = Convert.ToString(dtAuthDiagnosis.Rows[0]["DiagnosisTypeCode"]); //PriorAuthDiagnosis.ascx ddlDiagnosisCodeType 
                    item.ODSDiagnosisID = Convert.ToString(dtAuthDiagnosis.Rows[0]["ODSDiagnosisID"]); //txtDiagnosisline
                    item.RecordStatusCode = Convert.ToString(dtAuthDiagnosis.Rows[0]["RecordStatusCode"]);
                    authDiagnosis[i] = item;
                }

                #endregion

                #region AuthorizationInfo & Provide Notes

                var authInfo = new AuthorizationInfo()
                {

                    RecordStatusCode = Convert.ToString(dtAuthInfo.Rows[0]["RecordStatusCode"]),
                    AdmissionDate = Convert.ToString(dtAuthInfo.Rows[0]["AdmissionDate"]),
                    // AdmissionDate = Convert.ToDateTime(dtAuthInfo.Rows[0]["AdmissionDate"]),//txtAdmissionDate.Text.ToString(),
                    ApproverID = Convert.ToString(dtAuthInfo.Rows[0]["ApproverID"]), // Reg_ID is cmg from Table 
                    AssignmentCode = Convert.ToString(dtAuthInfo.Rows[0]["AssignmentCode"]),
                    //ddlCBMMXP.SelectedValue.ToString(),
                    IsPriorAuthEditable = "",
                    // AuthorizationBeginDate = Convert.ToString(dtAuthInfo.Rows[0]["AuthorizationBeginDate"]),//lblPACreation2.Text.ToString(),
                    AuthorizationDate = Convert.ToString(dtAuthInfo.Rows[0]["AuthorizationDate"]),//lblPACreation2.Text.ToString(),
                                                                                                  // AuthorizationEndDate = Convert.ToString(dtAuthInfo.Rows[0]["AuthorizationEndDate"]),
                    AuthorizationSource = Convert.ToString(dtAuthInfo.Rows[0]["AuthorizationSource"]),//this one is coming from  dbo.PRIOR_AUTH_COVERED_BY_MEDICAID_CARE_ORGANIZATION table
                    ContactName = Convert.ToString(dtAuthInfo.Rows[0]["ContactName"]),//txtContactName.Text.ToString(),
                    ContactNumber = Convert.ToString(dtAuthInfo.Rows[0]["ContactNumber"]),// txtContactNumber.Text.ToString(),
                    DateOfBirth = Convert.ToString(dtAuthInfo.Rows[0]["DateOfBirth"]), //txtBirthDate.Text.ToString(),
                    LTCFDischargeDate = Convert.ToString(dtAuthInfo.Rows[0]["LTCFDischargeDate"]),//txtDischargeDate.Text.ToString(),
                    MemberID = Convert.ToString(dtAuthInfo.Rows[0]["MemberID"]),
                    OrderingProviderID = Convert.ToString(dtAuthInfo.Rows[0]["OrderingProviderID"]), //txtorderingprovidernpi.Text.ToString(),
                    PatientEventTrackingNum = Convert.ToString(dtAuthInfo.Rows[0]["PatientEventTrackingNum"]),//txtTrackingNumber.Text.ToString(),
                    PriorAuthorizationID = Convert.ToString(dtAuthInfo.Rows[0]["PriorAuthorizationID"]),//lblPANumber.Text.ToString(),
                    PriorAuthorizationTypeCode = Convert.ToString(dtAuthInfo.Rows[0]["PriorAuthorizationTypeCode"]), // this one is coming from dbo.PRIOR_AUTH_STATUS table 
                    ProviderID = Convert.ToString(dtAuthInfo.Rows[0]["ProviderID"]),//txtMedicaidBillingNumber.Text.ToString(),

                    ReasonCode = Convert.ToString(dtAuthInfo.Rows[0]["ReasonCode"]),//gvOutcomeofreview 
                    RenderingProviderID = Convert.ToString(dtAuthInfo.Rows[0]["RenderingProviderID"]),//txtSPNPI.Text.ToString(),
                    SpecialIndicator = Convert.ToString(dtAuthInfo.Rows[0]["SpecialIndicator"]),//ddlSpecialIndicator.SelectedValue.ToString(),
                    StatusCode = Convert.ToString(dtAuthInfo.Rows[0]["StatusCode"]),//lblstatus2.Text.ToString()

                };

                var numProvNote = dtProvNote.Rows.Count;
                var provNotes = new Corp.Core.Libraries.CareManagement.ProviderNotes[numProvNote];
                for (int i = 0; i < numProvNote; i++)
                {
                    var item = new Corp.Core.Libraries.CareManagement.ProviderNotes();
                    item.Note = Convert.ToString(dtProvNote.Rows[0]["Note"]);//gvProviderNote
                    item.ODSProviderNoteID = Convert.ToString(dtProvNote.Rows[0]["ODSProviderNoteID"]);//gvProviderNote
                    item.CreatedBy = createdBy.ToString();
                    item.CreatedDate = DateTime.Now.ToString();
                    item.LastModifiedBy = createdBy.ToString();
                    item.LastModifiedDate = DateTime.Now.ToString();
                    provNotes[i] = item;
                }

                #endregion

                #region AuthorizationService

                var items = new Object[1];

                var authService = new AuthorizationService()
                {
                    AmountUsed = Convert.ToString(dtAuthService.Rows[0]["AmountUsed"]), // Total Fees Fron dental screen4.10.1
                    AuthorizedDollars = Convert.ToString(dtAuthService.Rows[0]["AuthorizedDollars"]), //gvServiceDetail
                    AuthorizedUnits = Convert.ToString(dtAuthService.Rows[0]["AuthorizedUnits"]), //gvServiceDetail
                    BalanceDollars = Convert.ToString(dtAuthService.Rows[0]["BalanceDollars"]), // 
                    BalanceUnits = Convert.ToString(dtAuthService.Rows[0]["BalanceUnits"]),//
                    BillDirectFromDate = Convert.ToString(dtAuthService.Rows[0]["BillDirectFromDate"]),//
                    BillDirectToDate = Convert.ToString(dtAuthService.Rows[0]["BillDirectToDate"]),//not found in DODD design
                                                                                                   //CaloriesPerDayNum = Convert.ToString(dtAuthService.Rows[0]["CaloriesPerDayNum"]),
                                                                                                   //  not found in Dodd design 
                    DaysNum = Convert.ToString(dtAuthService.Rows[0]["DaysNum"]), //not found in DODD design
                    DetailLineNumber = Convert.ToString(dtAuthService.Rows[0]["DetailLineNumber"]), // all service screen  gvServiceDetail
                    FirstDiagnosisCode = Convert.ToString(dtAuthService.Rows[0]["FirstDiagnosisCode"]),
                    FromProcedureCode = Convert.ToString(dtAuthService.Rows[0]["FromProcedureCode"]),
                    ICDType = Convert.ToString(dtAuthService.Rows[0]["ICDType"]),
                    InitialPlacement = Convert.ToString(dtAuthService.Rows[0]["InitialPlacement"]), // dental screen page 4.9 New Placement
                    InpatientProcedure = Convert.ToString(dtAuthService.Rows[0]["InpatientProcedure"]),
                    LastDiagnosisCode = Convert.ToString(dtAuthService.Rows[0]["LastDiagnosisCode"]),
                    LimitAmount = Convert.ToString(dtAuthService.Rows[0]["LimitAmount"]),
                    ListPrice = Convert.ToString(dtAuthService.Rows[0]["ListPrice"]),
                    NDCCode = Convert.ToString(dtAuthService.Rows[0]["NDCCode"]),
                    PricingFormula = Convert.ToString(dtAuthService.Rows[0]["PricingFormula"]),
                    PriorAuthorizationFrequencyCode = Convert.ToString(dtAuthService.Rows[0]["PriorAuthorizationFrequencyCode"]),
                    PriorPlacement = Convert.ToString(dtAuthService.Rows[0]["PriorPlacement"]),
                    ProcedureCode = Convert.ToString(dtAuthService.Rows[0]["ProcedureCode"]),
                    ProcedureTypeCode = Convert.ToString(dtAuthService.Rows[0]["ProcedureTypeCode"]),
                    ProcModifierCode = Convert.ToString(dtAuthService.Rows[0]["ProcModifierCode"]),
                    ProcModifierCode1 = Convert.ToString(dtAuthService.Rows[0]["ProcModifierCode1"]),
                    ProcModifierCode2 = Convert.ToString(dtAuthService.Rows[0]["ProcModifierCode2"]),
                    ProcModifierCode3 = Convert.ToString(dtAuthService.Rows[0]["ProcModifierCode3"]),
                    QuantityUsedAmount = Convert.ToString(dtAuthService.Rows[0]["QuantityUsedAmount"]),
                    QuantityUsedUnits = Convert.ToString(dtAuthService.Rows[0]["QuantityUsedUnits"]),
                    RateAmount = Convert.ToString(dtAuthService.Rows[0]["RateAmount"]),
                    RecordStatusCode = Convert.ToString(dtAuthService.Rows[0]["RecordStatusCode"]),
                    RenderingProviderID = Convert.ToString(dtAuthService.Rows[0]["RenderingProviderID"]),
                    RequestedDollars = Convert.ToString(dtAuthService.Rows[0]["RequestedDollars"]),//gvServiceDetailDental
                    RequestedEffectiveDate = Convert.ToString(dtAuthService.Rows[0]["RequestedEffectiveDate"]),
                    RequestedEndDate = Convert.ToString(dtAuthService.Rows[0]["RequestedEndDate"]),
                    RequestedUnits = Convert.ToString(dtAuthService.Rows[0]["RequestedUnits"]),
                    RevenueCode = Convert.ToString(dtAuthService.Rows[0]["RevenueCode"]),
                    ServiceCode = Convert.ToString(dtAuthService.Rows[0]["ServiceCode"]),
                    ServiceEndDate = Convert.ToString(dtAuthService.Rows[0]["ServiceEndDate"]),// not sure Request_FDOS
                    ServiceLimit = Convert.ToString(dtAuthService.Rows[0]["ServiceLimit"]),
                    ToProcedureCode = Convert.ToString(dtAuthService.Rows[0]["ToProcedureCode"]),
                    ServiceStartDate = Convert.ToString(dtAuthService.Rows[0]["ServiceStartDate"]),
                    ServiceStatusCode = Convert.ToString(dtAuthService.Rows[0]["ServiceStatusCode"]),
                    ServiceStatusReasonCode = Convert.ToString(dtAuthService.Rows[0]["ServiceStatusReasonCode"]),
                    ServicesUsed = Convert.ToString(dtAuthService.Rows[0]["ServicesUsed"]),
                    ThruService = Convert.ToString(dtAuthService.Rows[0]["ThruService"]),
                    ToothExtractionDate = Convert.ToString(dtAuthService.Rows[0]["ToothExtractionDate"]),//gvServiceDetailDental
                    ToothNumCode = Convert.ToString(dtAuthService.Rows[0]["ToothNumCode"]),//gvServiceDetailDental
                    ToothQuadrant = Convert.ToString(dtAuthService.Rows[0]["ToothQuadrant"]),//gvServiceDetailDental
                    ToothSurfaceCode = Convert.ToString(dtAuthService.Rows[0]["ToothSurfaceCode"]),//gvServiceDetailDental
                    TypeOfServiceCode = Convert.ToString(dtAuthService.Rows[0]["TypeOfServiceCode"])  //gvServiceDetailDental
                };

                items[0] = authService;

                #endregion

                var createAuth = new createAuthorization
                {
                    AuthorizationDiagnosis = authDiagnosis,
                    Attachments = attachments,
                    AuthorizationInfo = authInfo,
                    ProviderNotes = provNotes,
                    Items = items
                };
                var payload = new CreateRequestPayload();
                payload.Authorization = createAuth;
                var cms = new CareManagementSoapPortTypeClient();
                cms.createAuthorization(messageHeader, payload, out modTranactId, out responseCode,
                    out responseType, out odsAuthId, out responseMessage, out responseDetails);
                if (responseCode.Equals("200"))
                {
                    // we dont have error message custom message Dont know how we getting responsecode will come is still waiting for this.
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at CreateAuthSA method", ex);
        }
    }

    private bool ValidateCreateAuthSA(string memberId, string provId, string referredProvId, string renderProvId, DateTime AuthEndDate, DateTime AuthBeginDate,
          DateTime serviceEndDate, DateTime serviceBeginDate)
    {
        var flag = true;
        if (string.IsNullOrEmpty(memberId))
        {
            AddValidationErrorMessage("Member Id is not present in the System");
            flag = false;
        }
        if (string.IsNullOrEmpty(provId))
        {
            AddValidationErrorMessage("Provider Id is not present in the System");
            flag = false;
        }
        if (string.IsNullOrEmpty(referredProvId))
        {
            AddValidationErrorMessage("Provider Id is not present in the System");
            flag = false;
        }
        if (string.IsNullOrEmpty(provId))
        {
            AddValidationErrorMessage("Provider Id is not present in the System");
            flag = false;
        }
        if (string.IsNullOrEmpty(renderProvId))
        {
            AddValidationErrorMessage("Rendering Provider Id is not present in the System");
            flag = false;
        }
        if (DateTime.Compare(AuthBeginDate, AuthEndDate) > 1)
        {
            AddValidationErrorMessage("Auth End Date should not be lesser than Auth Begin Date");
            flag = false;
        }
        //if (string.IsNullOrEmpty(AuthBeginDate))
        //{
        //    AddValidationErrorMessage("Provider Id is not present in the System");
        //    flag = false;
        //}
        //if (string.IsNullOrEmpty(refRequest))
        //{
        //    AddValidationErrorMessage("The Referral Authorization is already present in the System.");
        //    flag = false;
        //}
        //if (string.IsNullOrEmpty(servRequest))
        //{
        //    AddValidationErrorMessage("The Service Authorization is already present in the System");
        //    flag = false;
        //}
        //if (string.IsNullOrEmpty(treatmentPlan))
        //{
        //    AddValidationErrorMessage("The TreatmentPlan Authorization is already present in the System.");
        //    flag = false;
        //}
        //if (string.IsNullOrEmpty(PATypeCide))
        //{
        //    AddValidationErrorMessage("Incorrect PA Type Code");
        //    flag = false;
        //}
        if (DateTime.Compare(serviceBeginDate, serviceEndDate) > 1)
        {
            AddValidationErrorMessage("Service End Date should not be lesser than Service Begin Date");
            flag = false;
        }
        return flag;
    }

    private void UpdateAuthSA()
    {

        var modTranactId = "";
        var responseCode = "";
        var responseType = "";
        var odsAuthId = "";
        var responseMessage = "";
        var responseDetails = "";
        var createdBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        try
        {
            //get the data from xml
            var dataset = PriorAuthHospitalController.GetUpdatePriorAuthRequestResponse();
            //var dtAttachment = (dataset.Tables["Attachments"] != null) ? dataset.Tables["Attachments"] : null;
            var dtAuthDiagnosis = (dataset.Tables["AuthorizationDiagnosis"] != null) ? dataset.Tables["AuthorizationDiagnosis"] : null;
            var dtAuthInfo = (dataset.Tables["AuthorizationInfo"] != null) ? dataset.Tables["AuthorizationInfo"] : null;
            var dtProvNote = (dataset.Tables["ProviderNotes"] != null) ? dataset.Tables["ProviderNotes"] : null;
            var dtAuthService = (dataset.Tables["AuthorizationService"] != null) ? dataset.Tables["AuthorizationService"] : null;

            //For Validation
            string memberId = Convert.ToString(dtAuthInfo.Rows[0]["MemberID"]);
            string provId = Convert.ToString(dtAuthInfo.Rows[0]["ProviderID"]);
            string referredProvId = Convert.ToString(dtAuthInfo.Rows[0]["MemberID"]);
            DateTime AuthEndDate = Convert.ToDateTime(dtAuthInfo.Rows[0]["AuthorizationEndDate"]);
            string renderProvId = Convert.ToString(dtAuthInfo.Rows[0]["RenderingProviderID"]);//i dont find it 
            DateTime AuthBeginDate = Convert.ToDateTime(dtAuthInfo.Rows[0]["AuthorizationBeginDate"]);
            //   string refRequest = Convert.ToString(dtAuthService.Rows[0]["ReferredProviderID"]); //i Dont find itnot part of Treatmentplan
            //string servRequest = Convert.ToString(dtAuthInfo.Rows[0][""]);// i dont find it 
            //string treatmentPlan = Convert.ToString(dtAuthInfo.Rows[0][""]);// i dont find it 
            //string PATypeCide = Convert.ToString(dtAuthInfo.Rows[0]["Prov_typ_CD"]);// i Dont find itnot part of Treatmentplan
            DateTime serviceEndDate = Convert.ToDateTime(dtAuthService.Rows[0]["ServiceEndDate"]);
            DateTime serviceBeginDate = Convert.ToDateTime(dtAuthService.Rows[0]["ServiceStartDate"]);

            if (ValidateUpdateAuthSA(memberId, provId, referredProvId, renderProvId, AuthEndDate, AuthBeginDate, serviceEndDate, serviceBeginDate))
            {

                #region Message header

                // I have to  ask where this one we have to  fill up.. 
                var dt = (dataset.Tables["MessageHeader"] != null) ? dataset.Tables["MessageHeader"] : null;
                var messageHeader = new MessageHeader()
                {
                    ModuleTransactionId = dt.Rows[0]["ModuleTransactionId"].ToString(),
                    AdditionalModuleTransactionId = dt.Rows[0]["AdditionalModuleTransactionId"].ToString(),
                    //BusinessFlow = dt.Rows[0]["BusinessFlow"],
                    //BusinessFlowSpecified = "",
                    RequestorSystem = (MessageHeaderRequestorSystem)dt.Rows[0]["RequestorSystem"],
                    RequestTimestamp = dt.Rows[0]["RequestTimestamp"].ToString(),
                    SITransactionKey = dt.Rows[0]["SITransactionKey"].ToString(),
                    StateCode = (MessageHeaderStateCode)dt.Rows[0]["StateCode"],
                    SubscriberSystem = (MessageHeaderSubscriberSystem)dt.Rows[0]["SubscriberSystem"]
                };

                #endregion

                #region AuthorizationDiagnosis

                var numAuthDiagnosis = dtAuthDiagnosis.Rows.Count;
                var authDiagnosis = new AuthorizationDiagnosis[numAuthDiagnosis];
                for (int i = 0; i < numAuthDiagnosis; i++)
                {
                    var item = new AuthorizationDiagnosis();
                    item.ICDType = Convert.ToString(dtAuthDiagnosis.Rows[0]["ICDType"]);//ddlICDVersion.selectvalue; // ddlICDVersion is cmg  PriorAuthDiagnosisSeach
                    item.DiagnosisCode = Convert.ToString(dtAuthDiagnosis.Rows[0]["DiagnosisCode"]);// txtDiagnosisCode PriorAuthDiagnosis.ascx
                    item.CreatedBy = createdBy.ToString();
                    item.CreatedDate = DateTime.Now.ToString();
                    item.LastModifiedBy = createdBy.ToString();
                    item.LastModifiedDate = DateTime.Now.ToString();
                    item.DiagnosisTypeCode = Convert.ToString(dtAuthDiagnosis.Rows[0]["DiagnosisTypeCode"]); //PriorAuthDiagnosis.ascx ddlDiagnosisCodeType 
                    item.ODSDiagnosisID = Convert.ToString(dtAuthDiagnosis.Rows[0]["ODSDiagnosisID"]); //txtDiagnosisline
                    item.RecordStatusCode = Convert.ToString(dtAuthDiagnosis.Rows[0]["RecordStatusCode"]);
                    authDiagnosis[i] = item;
                }

                #endregion

                #region AuthorizationInfo & Provide Notes

                var authInfo = new AuthorizationInfo()
                {
                    RecordStatusCode = Convert.ToString(dtAuthInfo.Rows[0]["RecordStatusCode"]),
                    AdmissionDate = Convert.ToString(dtAuthInfo.Rows[0]["AdmissionDate"]),
                    // AdmissionDate = Convert.ToDateTime(dtAuthInfo.Rows[0]["AdmissionDate"]),//txtAdmissionDate.Text.ToString(),
                    ApproverID = Convert.ToString(dtAuthInfo.Rows[0]["ApproverID"]), // Reg_ID is cmg from Table 
                    AssignmentCode = Convert.ToString(dtAuthInfo.Rows[0]["AssignmentCode"]),
                    //ddlCBMMXP.SelectedValue.ToString(),
                    IsPriorAuthEditable = "",
                    //  AuthorizationBeginDate = Convert.ToString(dtAuthInfo.Rows[0]["AuthorizationBeginDate"]),//lblPACreation2.Text.ToString(),
                    AuthorizationDate = Convert.ToString(dtAuthInfo.Rows[0]["AuthorizationDate"]),//lblPACreation2.Text.ToString(),
                                                                                                  //  AuthorizationEndDate = Convert.ToString(dtAuthInfo.Rows[0]["AuthorizationEndDate"]),
                    AuthorizationSource = Convert.ToString(dtAuthInfo.Rows[0]["AuthorizationSource"]),//this one is coming from  dbo.PRIOR_AUTH_COVERED_BY_MEDICAID_CARE_ORGANIZATION table
                    ContactName = Convert.ToString(dtAuthInfo.Rows[0]["ContactName"]),//txtContactName.Text.ToString(),
                    ContactNumber = Convert.ToString(dtAuthInfo.Rows[0]["ContactNumber"]),// txtContactNumber.Text.ToString(),
                    DateOfBirth = Convert.ToString(dtAuthInfo.Rows[0]["DateOfBirth"]), //txtBirthDate.Text.ToString(),
                    LTCFDischargeDate = Convert.ToString(dtAuthInfo.Rows[0]["LTCFDischargeDate"]),//txtDischargeDate.Text.ToString(),
                    MemberID = Convert.ToString(dtAuthInfo.Rows[0]["MemberID"]),
                    OrderingProviderID = Convert.ToString(dtAuthInfo.Rows[0]["OrderingProviderID"]), //txtorderingprovidernpi.Text.ToString(),
                    PatientEventTrackingNum = Convert.ToString(dtAuthInfo.Rows[0]["PatientEventTrackingNum"]),//txtTrackingNumber.Text.ToString(),
                    PriorAuthorizationID = Convert.ToString(dtAuthInfo.Rows[0]["PriorAuthorizationID"]),//lblPANumber.Text.ToString(),
                    PriorAuthorizationTypeCode = Convert.ToString(dtAuthInfo.Rows[0]["PriorAuthorizationTypeCode"]), // this one is coming from dbo.PRIOR_AUTH_STATUS table 
                    ProviderID = Convert.ToString(dtAuthInfo.Rows[0]["ProviderID"]),//txtMedicaidBillingNumber.Text.ToString(),

                    ReasonCode = Convert.ToString(dtAuthInfo.Rows[0]["ReasonCode"]),//gvOutcomeofreview 
                    RenderingProviderID = Convert.ToString(dtAuthInfo.Rows[0]["RenderingProviderID"]),//txtSPNPI.Text.ToString(),
                    SpecialIndicator = Convert.ToString(dtAuthInfo.Rows[0]["SpecialIndicator"]),//ddlSpecialIndicator.SelectedValue.ToString(),
                    StatusCode = Convert.ToString(dtAuthInfo.Rows[0]["StatusCode"]),//lblstatus2.Text.ToString()

                };

                var numProvNote = dtProvNote.Rows.Count;
                var provNotes = new Corp.Core.Libraries.CareManagement.ProviderNotes[numProvNote];
                for (int i = 0; i < numProvNote; i++)
                {
                    var item = new Corp.Core.Libraries.CareManagement.ProviderNotes();
                    item.Note = Convert.ToString(dtProvNote.Rows[0]["Note"]);//gvProviderNote
                    item.ODSProviderNoteID = Convert.ToString(dtProvNote.Rows[0]["ODSProviderNoteID"]);//gvProviderNote
                    item.CreatedBy = createdBy.ToString();
                    item.CreatedDate = DateTime.Now.ToString();
                    item.LastModifiedBy = createdBy.ToString();
                    item.LastModifiedDate = DateTime.Now.ToString();
                    provNotes[i] = item;
                }

                #endregion

                #region AuthorizationService

                var items = new Object[1];

                var authService = new AuthorizationService()
                {
                    AmountUsed = Convert.ToString(dtAuthService.Rows[0]["AmountUsed"]), // Total Fees Fron dental screen4.10.1
                    AuthorizedDollars = Convert.ToString(dtAuthService.Rows[0]["AuthorizedDollars"]), //gvServiceDetail
                    AuthorizedUnits = Convert.ToString(dtAuthService.Rows[0]["AuthorizedUnits"]), //gvServiceDetail
                    BalanceDollars = Convert.ToString(dtAuthService.Rows[0]["BalanceDollars"]), // 
                    BalanceUnits = Convert.ToString(dtAuthService.Rows[0]["BalanceUnits"]),//
                    BillDirectFromDate = Convert.ToString(dtAuthService.Rows[0]["BillDirectFromDate"]),//
                    BillDirectToDate = Convert.ToString(dtAuthService.Rows[0]["BillDirectToDate"]),//not found in DODD design
                    CaloriesPerDayNum = Convert.ToString(dtAuthService.Rows[0]["CaloriesPerDayNum"]),//  not found in Dodd design 
                    DaysNum = Convert.ToString(dtAuthService.Rows[0]["DaysNum"]), //not found in DODD design
                    DetailLineNumber = Convert.ToString(dtAuthService.Rows[0]["DetailLineNumber"]), // all service screen  gvServiceDetail
                    FirstDiagnosisCode = Convert.ToString(dtAuthService.Rows[0]["FirstDiagnosisCode"]),
                    FromProcedureCode = Convert.ToString(dtAuthService.Rows[0]["FromProcedureCode"]),
                    ICDType = Convert.ToString(dtAuthService.Rows[0]["ICDType"]),
                    InitialPlacement = Convert.ToString(dtAuthService.Rows[0]["InitialPlacement"]), // dental screen page 4.9 New Placement
                    InpatientProcedure = Convert.ToString(dtAuthService.Rows[0]["InpatientProcedure"]),
                    LastDiagnosisCode = Convert.ToString(dtAuthService.Rows[0]["LastDiagnosisCode"]),
                    LimitAmount = Convert.ToString(dtAuthService.Rows[0]["LimitAmount"]),
                    ListPrice = Convert.ToString(dtAuthService.Rows[0]["ListPrice"]),
                    NDCCode = Convert.ToString(dtAuthService.Rows[0]["NDCCode"]),
                    PricingFormula = Convert.ToString(dtAuthService.Rows[0]["PricingFormula"]),
                    PriorAuthorizationFrequencyCode = Convert.ToString(dtAuthService.Rows[0]["PriorAuthorizationFrequencyCode"]),
                    PriorPlacement = Convert.ToString(dtAuthService.Rows[0]["PriorPlacement"]),
                    ProcedureCode = Convert.ToString(dtAuthService.Rows[0]["ProcedureCode"]),
                    ProcedureTypeCode = Convert.ToString(dtAuthService.Rows[0]["ProcedureTypeCode"]),
                    ProcModifierCode = Convert.ToString(dtAuthService.Rows[0]["ProcModifierCode"]),
                    ProcModifierCode1 = Convert.ToString(dtAuthService.Rows[0]["ProcModifierCode1"]),
                    ProcModifierCode2 = Convert.ToString(dtAuthService.Rows[0]["ProcModifierCode2"]),
                    ProcModifierCode3 = Convert.ToString(dtAuthService.Rows[0]["ProcModifierCode3"]),
                    QuantityUsedAmount = Convert.ToString(dtAuthService.Rows[0]["QuantityUsedAmount"]),
                    QuantityUsedUnits = Convert.ToString(dtAuthService.Rows[0]["QuantityUsedUnits"]),
                    RateAmount = Convert.ToString(dtAuthService.Rows[0]["RateAmount"]),
                    RecordStatusCode = Convert.ToString(dtAuthService.Rows[0]["RecordStatusCode"]),
                    RenderingProviderID = Convert.ToString(dtAuthService.Rows[0]["RenderingProviderID"]),
                    RequestedDollars = Convert.ToString(dtAuthService.Rows[0]["RequestedDollars"]),//gvServiceDetailDental
                    RequestedEffectiveDate = Convert.ToString(dtAuthService.Rows[0]["RequestedEffectiveDate"]),
                    RequestedEndDate = Convert.ToString(dtAuthService.Rows[0]["RequestedEndDate"]),
                    RequestedUnits = Convert.ToString(dtAuthService.Rows[0]["RequestedUnits"]),
                    RevenueCode = Convert.ToString(dtAuthService.Rows[0]["RevenueCode"]),
                    ServiceCode = Convert.ToString(dtAuthService.Rows[0]["ServiceCode"]),
                    ServiceEndDate = Convert.ToString(dtAuthService.Rows[0]["ServiceEndDate"]),// not sure Request_FDOS
                    ServiceLimit = Convert.ToString(dtAuthService.Rows[0]["ServiceLimit"]),
                    ToProcedureCode = Convert.ToString(dtAuthService.Rows[0]["ToProcedureCode"]),
                    ServiceStartDate = Convert.ToString(dtAuthService.Rows[0]["ServiceStartDate"]),
                    ServiceStatusCode = Convert.ToString(dtAuthService.Rows[0]["ServiceStatusCode"]),
                    ServiceStatusReasonCode = Convert.ToString(dtAuthService.Rows[0]["ServiceStatusReasonCode"]),
                    ServicesUsed = Convert.ToString(dtAuthService.Rows[0]["ServicesUsed"]),
                    ThruService = Convert.ToString(dtAuthService.Rows[0]["ThruService"]),
                    ToothExtractionDate = Convert.ToString(dtAuthService.Rows[0]["ToothExtractionDate"]),//gvServiceDetailDental
                    ToothNumCode = Convert.ToString(dtAuthService.Rows[0]["ToothNumCode"]),//gvServiceDetailDental
                    ToothQuadrant = Convert.ToString(dtAuthService.Rows[0]["ToothQuadrant"]),//gvServiceDetailDental
                    ToothSurfaceCode = Convert.ToString(dtAuthService.Rows[0]["ToothSurfaceCode"]),//gvServiceDetailDental
                    TypeOfServiceCode = Convert.ToString(dtAuthService.Rows[0]["TypeOfServiceCode"])  //gvServiceDetailDental
                };

                items[0] = authService;

                #endregion

                var updateAuth = new updateAuthorization
                {
                    AuthorizationDiagnosis = authDiagnosis,
                    AuthorizationInfo = authInfo,
                    ProviderNotes = provNotes,
                    Items = items
                };
                var payload = new UpdateRequestPayload();
                payload.Authorization = updateAuth;
                var cms = new CareManagementSoapPortTypeClient();
                cms.updateAuthorization(messageHeader, payload, out modTranactId, out responseCode,
                    out responseType, out odsAuthId, out responseMessage, out responseDetails);
                if (responseCode.Equals("200"))
                {
                    // we dont have error message custom message Dont know how we getting responsecode will come is still waiting for this.
                }
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private bool ValidateUpdateAuthSA(string memberId, string provId, string referredProvId, string renderProvId, DateTime AuthEndDate, DateTime AuthBeginDate,
      DateTime serviceEndDate, DateTime serviceBeginDate)
    {
        var flag = true;
        if (string.IsNullOrEmpty(memberId))
        {
            AddValidationErrorMessage("Member Id is not present in the System");
            flag = false;
        }
        if (string.IsNullOrEmpty(provId))
        {
            AddValidationErrorMessage("Provider Id is not present in the System");
            flag = false;
        }
        if (string.IsNullOrEmpty(referredProvId))
        {
            AddValidationErrorMessage("Provider Id is not present in the System");
            flag = false;
        }
        if (string.IsNullOrEmpty(provId))
        {
            AddValidationErrorMessage("Provider Id is not present in the System");
            flag = false;
        }
        if (string.IsNullOrEmpty(renderProvId))
        {
            AddValidationErrorMessage("Rendering Provider Id is not present in the System");
            flag = false;
        }
        if (DateTime.Compare(AuthBeginDate, AuthEndDate) > 1)
        {
            AddValidationErrorMessage("Auth End Date should not be lesser than Auth Begin Date");
            flag = false;
        }
        if (DateTime.Compare(serviceBeginDate, serviceEndDate) > 1)
        {
            AddValidationErrorMessage("Service End Date should not be lesser than Service Begin Date");
            flag = false;
        }
        return flag;
    }

    private void InquireAuthSA()
    {
        var modTranactId = "";
        var responseCode = "";
        var responseType = "";
        var odsAuthId = "";
        var responseMessage = "";

        #region Message header

        var dataset = PriorAuthHospitalController.GetUpdatePriorAuthRequestResponse();

        // I have to  ask where this one we have to  fill up.. 
        var dt = (dataset.Tables["MessageHeader"] != null) ? dataset.Tables["MessageHeader"] : null;
        var messageHeader = new MessageHeader()
        {
            ModuleTransactionId = dt.Rows[0]["ModuleTransactionId"].ToString(),
            AdditionalModuleTransactionId = dt.Rows[0]["AdditionalModuleTransactionId"].ToString(),
            //BusinessFlow = dt.Rows[0]["BusinessFlow"],
            //BusinessFlowSpecified = "",
            RequestorSystem = (MessageHeaderRequestorSystem)dt.Rows[0]["RequestorSystem"],
            RequestTimestamp = dt.Rows[0]["RequestTimestamp"].ToString(),
            SITransactionKey = dt.Rows[0]["SITransactionKey"].ToString(),
            StateCode = (MessageHeaderStateCode)dt.Rows[0]["StateCode"],
            SubscriberSystem = (MessageHeaderSubscriberSystem)dt.Rows[0]["SubscriberSystem"]
        };

        #endregion

        var payload = new InquireAuthorizationPayload();
        payload.InputLimit = "";
        payload.InputOffset = "";
        payload.PriorAuthorizationID = "";
        payload.RecordStatusCode = "";

        var responseDetails = new InquireAuthorizationInformation[100];
        var cms = new CareManagementSoapPortTypeClient();
        cms.inquireAuthorization(messageHeader, payload, out modTranactId, out responseCode,
            out responseType, out odsAuthId, out responseMessage, out responseDetails);
        if (responseCode.Equals("200"))
        {
            var authDiagnosis = new InquireAuthorizationInformationAuthorizationDiagnosis[100];
            var provNotes = new InquireAuthorizationInformationProviderNotes[100];
            var authInfo = new InquireAuthorizationInformationAuthorizationInfo[100];
            var attachments = new InquireAuthorizationInformationAttachments[100];
            var authService = new InquireAuthorizationInformationAuthorizationService[100];
            var inqurieAuth = new InquireAuthorizationInformation
            {
                AuthorizationDiagnosis = authDiagnosis,
                AuthorizationInfo = authInfo,
                ProviderNotes = provNotes,
                Attachments = attachments,
                AuthorizationService = authService
            };
            foreach (var item in provNotes)
            {
                var ods = item.ODSProviderNoteID;
            }
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            string PATrackingNumber = txtPatientTrckNum.Text;
            lblpriorautherror.Text = "";

            int paTypeID = 0;
            switch (rblClaimType.SelectedValue.ToUpper())
            {
                case "DENTAL":
                    paTypeID = CON.PAClaimsType.Dental;
                    break;
                case "PROFESSIONAL":
                    paTypeID = CON.PAClaimsType.Professional;
                    break;
                case "INSTITUTIONAL":
                    paTypeID = CON.PAClaimsType.Institutional;
                    break;
            }

            if (!ValidatePANumberPopulated()) return;

            if (!ValidateOrderingProviderInfo()) return;

            if (string.IsNullOrEmpty(hdnTrackingNo.Value))
            {
                var isExistpriorauth = ValidatePriorAuthSaveDetail(paTypeID, txtPatientTrckNum.Text);
                if (!isExistpriorauth)
                {
                    //display error
                    lblpriorautherror.Text = "Error: medicaid Id- " + MedicaidId + " with Patient Tracking# " + txtPatientTrckNum.Text + " already exists";
                    return;
                }
            }
            //Main PA Table - Generate a new GUID
            Guid lnkRecord = new Guid();

            //Sub Section PA Tables (Guid)
            switch (rblClaimType.SelectedValue.ToUpper())
            {
                case "DENTAL":
                    lnkRecord = PriorAuthHospitalController.savePAMainRecord(MedicaidId, CON.PAClaimsType.Dental, PATrackingNumber);
                    SavePriorAuthDental(lnkRecord);
                    //Sub save diagnosis panel (Guid)
                    SaveDiagnosisDataToDb(lnkRecord);
                    //save service detail panel (Guid)
                    SaveDentalServiceDetailsDataToDb(lnkRecord);
                    //save attachment panel
                    SaveAttachmentDataToDB(lnkRecord);
                    break;
                case "PROFESSIONAL":
                    lnkRecord = PriorAuthHospitalController.savePAMainRecord(MedicaidId, CON.PAClaimsType.Professional, PATrackingNumber);
                    SavePriorAuthProfessional(lnkRecord);
                    //Sub save diagnosis panel (Guid)
                    SaveDiagnosisDataToDb(lnkRecord);
                    //save service detail panel (Guid)
                    SaveProfessionalServiceDetailsDataToDb(lnkRecord);
                    //save attachment panel
                    SaveAttachmentDataToDB(lnkRecord);
                    break;
                case "INSTITUTIONAL":
                    lnkRecord = PriorAuthHospitalController.savePAMainRecord(MedicaidId, CON.PAClaimsType.Institutional, PATrackingNumber);
                    SavePriorAuthInstitutional(lnkRecord);
                    //Sub save diagnosis panel (Guid)
                    SaveDiagnosisDataToDb(lnkRecord);
                    //save service detail panel (Guid)
                    SaveInstitutionalServiceDetailsDataToDb(lnkRecord);
                    //save attachment panel
                    SaveAttachmentDataToDB(lnkRecord);
                    break;
            }



            lblpriorautherror.Text = "PA request has been saved.";
            hdnTrackingNo.Value = txtPatientTrckNum.Text;
            txtPatientTrckNum.Enabled = false;
            Helper.PurgeSessionData();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-BtnSaveClick");
            IntuitivePriorAuthMessageBoxID.Show(string.Format("An error has occurred while save click event. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }





    private bool ValidateOrderingProviderInfo()
    {
        List<string> ddlAssignmentLst = new List<string>() { "01", "02", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "19", "23", "40", "46" };
        if (ddlAssignmentLst.Contains(ddlAssignment.SelectedValue)
            && string.IsNullOrEmpty(txtorderingprovidernpi.Text) && !string.IsNullOrEmpty(ddlAssignment.SelectedValue)
            && failureValidations <= maxValidations)
        {
            if (string.IsNullOrEmpty(hdtxtOMID1.Value))
            {
                ++failureValidations;
                AddValidationErrorMessage("Ordering Provider Details is Missing.");
                return false;
            }
        }
        return true;
    }

    protected void btnReSubmitConfirmYes_Click(object sender, EventArgs e)
    {
        //webservice Call;
    }

    protected void btnReSubmitConfirmNo_Click(object sender, EventArgs e)
    {
        mdlpnlReSubmitConfirm.Hide();
    }

    public bool checkChangesMade()
    {
        if (pnlRecipient.Enabled == true)
        {

        }
        return false;
    }


    protected void btnReSubmit_Click(object sender, EventArgs e)
    {
        bool changesMade = false;
        if (hdnModified.Value.Equals("true"))
        {
            changesMade = true;
        }

        if (pnlRecipient.Enabled == false)
        {
            pnlRecipient.Enabled = true;
            pnlContact.Enabled = true;
            pnlServiceInformation.Enabled = true;
            pnlTrackingNumber.Enabled = true;
            pnlService.Enabled = true;
            pnlServiceProviderInfo.Enabled = true;
            pnlorderproviderinfo.Enabled = true;
            pnlDiagnosisLine.Enabled = true;
            pnlCertHospital.Enabled = true;
            pnlProviderNote.Enabled = true;
            pnlProvidermainPnl.Enabled = true;
            pnlOutcomeOfReview.Enabled = true;
            pnlAttachment.Enabled = true;
            pnlDentalAttachment.Enabled = true;
            pnlmissingtooth.Enabled = true;
            pnlDocumentbyMail.Enabled = true;
            pnlreviewernoteprovider.Enabled = true;
            pnlReasonforDenial.Enabled = true;
            Panel1.Enabled = true;
            btnSubmit.Visible = false;
            btnCancelPARequest.Visible = false;
            //btnCancel.Visible = false;
            btnCancel_Revert.Visible = true;
            btnClearAll.Visible = false;
            //gvDiagnosis.Enabled = true;
            btnDiagnosisAdd.Enabled = true;
            // gvServiceDetailDental.Enabled = true;
            btnDentalServiceDetailAdd1.Visible = true;

            //btnClearAll.Visible = true;

            btnReSubmit.Visible = true;
            PAInfo.Attributes.Remove("class");
            txtstatus2.Text = "Submission Pending";
            txtstatus2.ReadOnly = false;
            txtPANumber2.ReadOnly = false;
            txtPACreation2.ReadOnly = false;
            txtEffDate2.ReadOnly = false;
            txtExpDate2.ReadOnly = false;
            txtPANumber2.Text = "";
            txtPACreation2.Text = "";
            txtEffDate2.Text = "";
            txtExpDate2.Text = "";
            PAInfo.Attributes.Add("class", "col-sm-6 text-left");
        }
        else if (pnlRecipient.Enabled == true && !changesMade)
        {
            Response.Write("<script>alert('You must edit prior authorization information before submitting');</script>");
        }
        else if (pnlRecipient.Enabled == true && changesMade)
        {
            var isSubValid = true;

            if (!ValidateData())
            {
                valerrormess.Visible = true;
                isSubValid = false;
            }
            if (!FrontEndEdits())
            {
                isSubValid = false;
            }

            if (!ValidatePAAttachments())
            {
                isSubValid = false;
            }

            if (!isSubValid)
                return;

            try
            {
                makePriorAuthAddUpdateRequest();
            }
            catch (Exception ex)
            {
                string logNumber = CreateAndReturnLogThreadNumber(ex, "Resubmit");
                IntuitivePriorAuthMessageBoxID.Show(string.Format("An error has occurred, please save your prior authorization for future submission. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            }
        }
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        ddlAssignment.Enabled = true;
        ddlServiceType.Enabled = true;
        pnlRecipient.Enabled = true;
        pnlContact.Enabled = true;
        pnlServiceInformation.Enabled = true;
        pnlTrackingNumber.Enabled = true;
        pnlService.Enabled = true;
        pnlServiceProviderInfo.Enabled = true;
        pnlorderproviderinfo.Enabled = true;
        pnlDiagnosisLine.Enabled = true;
        pnlCertHospital.Enabled = true;
        pnlProviderNote.Enabled = true;
        pnlProvidermainPnl.Enabled = true;
        pnlOutcomeOfReview.Enabled = true;
        pnlAttachment.Enabled = true;
        pnlDentalAttachment.Enabled = true;
        pnlmissingtooth.Enabled = true;
        pnlDocumentbyMail.Enabled = true;
        pnlreviewernoteprovider.Enabled = true;
        pnlReasonforDenial.Enabled = true;

        //gvDiagnosis.Enabled = true;
        btnDiagnosisAdd.Enabled = true;
        hdnDiagnosiAddCopy.Value = "true";

        //gvServiceDetailDental.Enabled = true;
        btnDentalServiceDetailAdd1.Visible = true;
        // gvServiceDetailProfessional.Enabled = true;
        // gvServiceDetail.Enabled = true;

        btnCopy.Visible = false;
        btnSubmit.Visible = true;
        btnClearAll.Visible = true;
        btnSave.Visible = true;
        //btnCancel.Visible = false;

        txtstatus2.Text = "Submission Pending";
        txtPANumber2.Text = "";
        txtPACreation2.Text = "";
        txtEffDate2.Text = "";
        txtExpDate2.Text = "";
        PAInfo.Attributes.Add("class", "col-sm-6 text-left");

        var claimType = rblClaimType.SelectedValue.ToUpper();
        if (claimType == "DENTAL")
        {
            try
            {
                SetDentalServiceDetailsAuthorizedFieldsToEmpty();
            }
            catch (Exception ex)
            {
                CreateAndReturnLogThreadNumber(ex, "SubmitPA-btnCopy_Click-SetDentalServiceDetailsAuthorizedFieldsToEmpty");
            }
        }
        //  lblDentalservDetailsNoData.Visible = true;
        else if (claimType == "PROFESSIONAL")
        {
            try
            {
                SetProfessionalServiceDetailsAuthorizedFieldsToEmpty();
            }
            catch (Exception ex)
            {
                CreateAndReturnLogThreadNumber(ex, "SubmitPA-btnCopy_Click-SetProfessionalServiceDetailsAuthorizedFieldsToEmpty");
            }
        }
        //  lblprofNoDetailsFound.Visible = true;
        else if (claimType == "INSTITUTIONAL")
        {
            btnServiceDetailAdd1.Visible = true;

            try
            {
                SetInstitutionalServiceDetailsAuthorizedFieldsToEmpty();
            }
            catch (Exception ex)
            {
                CreateAndReturnLogThreadNumber(ex, "SubmitPA-btnCopy_Click-SetInstitutionalServiceDetailsAuthorizedFieldsToEmpty");
            }
        }

        try
        {
            if (lblstatus2.Text == "Edit")
            {
                CreateAuthSA();
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-BtnCopyClick");
            IntuitivePriorAuthMessageBoxID.Show(string.Format("An error has occurred while copy click event. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void SetInstitutionalServiceDetailsAuthorizedFieldsToEmpty()
    {
        DataSet dsInstServiceDetails = null;
        DataSet dsFinalInstServiceDetails = new DataSet();
        DataTable dtInstitutionalServiceDetails = null;
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
        parms.Add("LINK_SECTIONS", SaveCodeLNK);
        dsInstServiceDetails = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
        dtInstitutionalServiceDetails = dsInstServiceDetails.Tables[0];
        if (dtInstitutionalServiceDetails != null && dtInstitutionalServiceDetails.Rows.Count > 0)
        {
            foreach (DataRow drInstServiceDetail in dsInstServiceDetails.Tables[0].Rows)
            {
                if (drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"] != null &&
                    drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"].ToString()))
                {
                    drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"] = string.Empty;
                }

                if (drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"] != null &&
                    drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString()))
                {
                    drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"] = DBNull.Value;
                }

                if (drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"] != null &&
                    drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"].ToString()))
                {
                    drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"] = DBNull.Value;
                }

                if (drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"] != null &&
                    drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"].ToString()))
                {
                    drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"] = DBNull.Value;
                }

                if (drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_STATUS"] != null &&
                    drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_STATUS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_STATUS"].ToString()))
                {
                    drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_STATUS"] = string.Empty;
                }

                if (drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"] != null &&
                    drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"].ToString()))
                {
                    drInstServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"] = null;
                }

                if (drInstServiceDetail["PRIOR_AUTH_STATUS_ID"] != null &&
                    drInstServiceDetail["PRIOR_AUTH_STATUS_ID"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drInstServiceDetail["PRIOR_AUTH_STATUS_ID"].ToString()))
                {
                    drInstServiceDetail["PRIOR_AUTH_STATUS_ID"] = null;
                }
            }

            DataTable dtFinalInstServDetails = dtInstitutionalServiceDetails.Copy();
            dsFinalInstServiceDetails.Tables.Add(dtFinalInstServDetails);
            foreach (DataRow row in dtFinalInstServDetails.Rows)
            {
                Dictionary<string, string> parms2 = new Dictionary<string, string>();
                parms2.Add("PRIOR_AUTH_SERVICE_REVENUE_CODE", row["PRIOR_AUTH_SERVICE_REVENUE_CODE"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", row["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE", row["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", row["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"].ToString());
                parms2.Add("PRIOR_AUTH_REQUESTED_UNITS_ID", row["PRIOR_AUTH_REQUESTED_UNITS_ID"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", row["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", row["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", row["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"].ToString());
                parms2.Add("PRIOR_AUTH_STATUS_ID", row["PRIOR_AUTH_STATUS_ID"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC", row["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE", row["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"].ToString());
                parms2.Add("PRIOR_AUTH_LEVEL_CARE_ID", row["PRIOR_AUTH_LEVEL_CARE_ID"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS", row["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", row["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR", row["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS", row["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS", row["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"].ToString());
                parms2.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms2.Add("LAST_MODIFIED_USER", SaveCodeLNK);
                parms2.Add("Created_On_Date_Time", DateTime.Now.ToString());
                parms2.Add("Created_By_User", SaveCodeLNK);
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO", row["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_STATUS", row["PRIOR_AUTH_SERVICE_DETAIL_STATUS"].ToString());
                parms2.Add("PRIOR_AUTH_INSTITUTIONALSAVE_ID", row["PRIOR_AUTH_INSTITUTIONALSAVE_ID"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSAVE_ID", row["PRIOR_AUTH_DENTALSAVE_ID"].ToString());
                parms2.Add("PRIOR_AUTH_PROFESSIONALSAVE_ID", row["PRIOR_AUTH_PROFESSIONALSAVE_ID"].ToString());
                parms2.Add("MedicaidID", row["MedicaidID"].ToString());
                parms2.Add("Line", row["Line"].ToString());
                parms2.Add("PRIOR_AUTH_SAVE_CODE_ID", row["PRIOR_AUTH_SAVE_CODE_ID"].ToString());
                parms2.Add("LINK_SECTIONS", SaveCodeLNK);
                PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("INSERTPRIORAUTH_INSTSERVICEDETAILS", parms2);
            }
        }
    }

    private void SetProfessionalServiceDetailsAuthorizedFieldsToEmpty()
    {
        DataSet dsProfServiceDetails = null;
        DataSet dsFinalProfServiceDetails = new DataSet();
        DataTable dtProfessionalServiceDetails = null;
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
        parms.Add("LINK_SECTIONS", SaveCodeLNK);
        dsProfServiceDetails = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
        dtProfessionalServiceDetails = dsProfServiceDetails.Tables[0];
        if (dtProfessionalServiceDetails != null && dtProfessionalServiceDetails.Rows.Count > 0)
        {
            foreach (DataRow drProfServiceDetail in dsProfServiceDetails.Tables[0].Rows)
            {
                if (drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"] != null &&
                    drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"].ToString()))
                {
                    drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"] = string.Empty;
                }

                if (drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != null &&
                    drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString()))
                {
                    drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"] = DBNull.Value;
                }

                if (drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"] != null &&
                    drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"].ToString()))
                {
                    drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"] = DBNull.Value;
                }

                if (drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"] != null &&
                    drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"].ToString()))
                {
                    drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"] = DBNull.Value;
                }

                if (drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"] != null &&
                    drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"].ToString()))
                {
                    drProfServiceDetail["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"] = DBNull.Value;
                }

                if (drProfServiceDetail["PRIOR_AUTH_STATUS_ID"] != null &&
                    drProfServiceDetail["PRIOR_AUTH_STATUS_ID"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drProfServiceDetail["PRIOR_AUTH_STATUS_ID"].ToString()))
                {
                    drProfServiceDetail["PRIOR_AUTH_STATUS_ID"] = DBNull.Value;
                }
            }

            DataTable dtProfFinalServDetails = dtProfessionalServiceDetails.Copy();
            dsFinalProfServiceDetails.Tables.Add(dtProfFinalServDetails);
            foreach (DataRow row in dtProfFinalServDetails.Rows)
            {
                Dictionary<string, string> parms2 = new Dictionary<string, string>();
                parms2.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", row["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"].ToString());
                parms2.Add("PRIOR_AUTH_STATUS_ID", row["PRIOR_AUTH_STATUS_ID"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"].ToString());
                parms2.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms2.Add("LAST_MODIFIED_USER", SaveCodeLNK);
                parms2.Add("Created_On_Date_Time", DateTime.Now.ToString());
                parms2.Add("Created_By_User", SaveCodeLNK);
                parms2.Add("PRIOR_AUTH_PROFESSIONALSAVE_ID", row["PRIOR_AUTH_PROFESSIONALSAVE_ID"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"].ToString());
                parms2.Add("MedicaidID", row["MedicaidID"].ToString());
                parms2.Add("Line", row["Line"].ToString());
                parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS", row["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS"].ToString());
                parms2.Add("PRIOR_AUTH_SAVE_CODE_ID", row["PRIOR_AUTH_SAVE_CODE_ID"].ToString());
                parms2.Add("LINK_SECTIONS", SaveCodeLNK);
                PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("INSERTPRIORAUTH_PROFFSERVICEDETAILS", parms2);
            }
        }
    }

    private void SetDentalServiceDetailsAuthorizedFieldsToEmpty()
    {

        DataSet dsDentalServiceDetails = null;
        DataSet dsFinalDentalServiceDetails = new DataSet();
        DataTable dtDentalServiceDetails = null;
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
        parms.Add("LINK_SECTIONS", SaveCodeLNK);
        dsDentalServiceDetails = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
        dtDentalServiceDetails = dsDentalServiceDetails.Tables[0];
        if (dtDentalServiceDetails != null && dtDentalServiceDetails.Rows.Count > 0)
        {
            foreach (DataRow drDentalServiceDetail in dsDentalServiceDetails.Tables[0].Rows)
            {
                if (drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS"] != null &&
                    drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS"].ToString()))
                {
                    drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS"] = string.Empty;
                }

                if (drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != null &&
                    drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString()))
                {
                    drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR"] = DBNull.Value;
                }

                if (drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"] != null &&
                    drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"].ToString()))
                {
                    drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"] = DBNull.Value;
                }

                if (drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"] != null &&
                    drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"].ToString()))
                {
                    drDentalServiceDetail["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"] = DBNull.Value;
                }

                if (drDentalServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"] != null &&
                    drDentalServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drDentalServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"].ToString()))
                {
                    drDentalServiceDetail["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"] = string.Empty;
                }

                if (drDentalServiceDetail["PRIOR_AUTH_STATUS_ID"] != null &&
                    drDentalServiceDetail["PRIOR_AUTH_STATUS_ID"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drDentalServiceDetail["PRIOR_AUTH_STATUS_ID"].ToString()))
                {
                    drDentalServiceDetail["PRIOR_AUTH_STATUS_ID"] = string.Empty;
                }
            }

            DataTable dtDentalFinalServDetails = dtDentalServiceDetails.Copy();
            dsFinalDentalServiceDetails.Tables.Add(dtDentalFinalServDetails);
            foreach (DataRow row in dtDentalFinalServDetails.Rows)
            {
                Dictionary<string, string> parms2 = new Dictionary<string, string>();
                parms2.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", row["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString());
                parms2.Add("PRIOR_AUTH_TOOTH_NUMBER_ID", row["PRIOR_AUTH_TOOTH_NUMBER_ID"].ToString());
                parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS", row["PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS", row["PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS", row["PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS", row["PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS", row["PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE", row["PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE", row["PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE", row["PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE", row["PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE", row["PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"].ToString());
                parms2.Add("PRIOR_AUTH_STATUS_ID", row["PRIOR_AUTH_STATUS_ID"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID", row["PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS", row["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", row["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"].ToString());
                parms2.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms2.Add("LAST_MODIFIED_USER", SaveCodeLNK);
                parms2.Add("Created_On_Date_Time", DateTime.Now.ToString());
                parms2.Add("Created_By_User", SaveCodeLNK);
                parms2.Add("PRIOR_AUTH_DENTALSAVE_ID", row["PRIOR_AUTH_DENTALSAVE_ID"].ToString());
                parms2.Add("MedicaidID", row["MedicaidID"].ToString());
                parms2.Add("Line", row["Line"].ToString());
                parms2.Add("PRIOR_AUTH_STATUS_TYPE", returnPATypeID());
                parms2.Add("LINK_SECTIONS", SaveCodeLNK);
                PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("INSERTPRIORAUTH_DENTALSERVICEDETAILS", parms2);
            }
        }
    }

    protected void btnPrintCoverAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = Server.MapPath("~/Documents/MITSEDMS_Cover_IT4" + ".pdf");
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void btnDiagnosisEdit_Click(object sender, CommandEventArgs e)
    {
        try
        {
            GetDiagnosisCodeType();

            int index = Convert.ToInt32(e.CommandName);

            if (rblClaimType.SelectedItem != null)
                lblDiagnosisErrorMessage.Text = string.Empty;

            int lineNumber = Convert.ToInt32(e.CommandArgument);

            string seqNum = Convert.ToString(e.CommandName);

            lblservDetlbl.Text = seqNum;

            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            DataSet ds = LoadDiagnosisDataToSession();


            var dataTable = ds.Tables[0];

            int diagnCount = ds != null ? ds.Tables[0].Rows.Count : 0;

            DataRow currentRow = null;
            foreach (DataRow dr in dataTable.Rows)
            {
                if (Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_ID"]) == Convert.ToString(lineNumber))
                {
                    currentRow = dr;
                    break;
                }
            }

            hdDiagLineNum.Value = Convert.ToString(currentRow["PRIOR_AUTH_DIAGNOSIS_ID"]);
            txtLnDiagnosisCode.Text = Convert.ToString(currentRow["PRIOR_AUTH_DIAGNOSIS_CODE"]);
            txtDiagnosisCodeDescription.Text = Convert.ToString(currentRow["PRIOR_AUTH_DIAGNOSIS_DESC"]);
            //ddlDiagnosisCodeType.Items.FindByValue(Convert.ToString(currentRow["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"])).Selected = true;
            ddlDiagnosisCodeType.SelectedValue = ddlDiagnosisCodeType.Items.FindByValue(Convert.ToString(currentRow["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"])).Value;

            if (currentRow["PRIOR_AUTH_DIAGNOSIS_DATE"] != null && currentRow["PRIOR_AUTH_DIAGNOSIS_DATE"] != DBNull.Value
                && Convert.ToDateTime(currentRow["PRIOR_AUTH_DIAGNOSIS_DATE"]).ToShortDateString() != "1/1/1900")
                txtDiagnosisDate.Text = Convert.ToDateTime(currentRow["PRIOR_AUTH_DIAGNOSIS_DATE"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);

            cpeDiagnosis.Collapsed = false;
            cpeDiagnosis.ClientState = "false";
            btnDiagnosisAdd.Visible = false;
            //dvDiagnosisAdd.Visible = true;
            pnlDiagnosisLine.Visible = true;
            //btnDiagnosisUpdateAdd.Enabled = true;
            //  btnDiagnosisEditCancel.Enabled = true;
            txtDiagnosisDate.Enabled = true;
            lnkDiagnosisSearch.Enabled = true;

            if (ddlDiagnosisCodeType.SelectedItem.Text == "Principal")
            {
                ddlDiagnosisCodeType.Enabled = false;
                return;
            }
            if (ddlDiagnosisCodeType.SelectedItem.Text == "Admitting")
            {
                ListItem removeItem = ddlDiagnosisCodeType.Items.FindByText("Principal");
                ddlDiagnosisCodeType.Items.Remove(removeItem);
                ddlDiagnosisCodeType.Enabled = true;
                return;
            }

            if (index == 2)
            {
                ListItem Principal = ddlDiagnosisCodeType.Items.FindByText("Principal");
                ddlDiagnosisCodeType.Items.Remove(Principal);
                ddlDiagnosisCodeType.Enabled = true;
            }
            else if (diagnCount >= 2 && ddlDiagnosisCodeType.SelectedItem.Text != "Admitting")
            {
                ListItem Principal = ddlDiagnosisCodeType.Items.FindByText("Principal");
                ddlDiagnosisCodeType.Items.Remove(Principal);
                ListItem Admitting = ddlDiagnosisCodeType.Items.FindByText("Admitting");
                ddlDiagnosisCodeType.Items.Remove(Admitting);
                ddlDiagnosisCodeType.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void StartDateCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {

    }

    protected void gvDiagnosis_RowEditing(object sender, GridViewEditEventArgs e)
    {
        // gvDiagnosis.EditIndex = e.NewEditIndex;
        //loadData();
    }
    protected void gvDiagnosis_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //string stor_id = gvDiagnosis.DataKeys[e.RowIndex].Values["ODSDiagnosisID"].ToString();
        //TextBox DiagnosisTypeCode = (TextBox)gvDiagnosis.Rows[e.RowIndex].FindControl("txtDiagnosisTypeCode");
        //TextBox DiagnosisCode = (TextBox)gvDiagnosis.Rows[e.RowIndex].FindControl("txtDiagnosisCode");
        //TextBox DiagnosisCodeDesc = (TextBox)gvDiagnosis.Rows[e.RowIndex].FindControl("txtDiagnosisCodeDesc");
        //TextBox DiagnosisDate = (TextBox)gvDiagnosis.Rows[e.RowIndex].FindControl("txtDiagnosisDate");


        //gvDiagnosis.EditIndex = -1;
        //loadData();
    }

    protected void gvDiagnosis_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            //  gvDiagnosis.EditIndex = -1;
            GetDiagnoisServiceDetails();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void gvDiagnosis_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ODSDiagnosisID = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ODSDiagnosisID"));
            Button lnkbtnresult = (Button)e.Row.FindControl("ButtonDelete");
            if (lnkbtnresult != null)
            {
                lnkbtnresult.Attributes.Add("onclick", "javascript:return deleteConfirm('" + ODSDiagnosisID + "')");
            }
        }
    }
    protected void gvDiagnosis_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddNew"))
        {
            //TextBox ODSDiagnosisID = (TextBox)gvDiagnosis.FooterRow.FindControl("inODSDiagnosisID");
            //TextBox DiagnosisTypeCode = (TextBox)gvDiagnosis.FooterRow.FindControl("inDiagnosisTypeCode");
            //TextBox DiagnosisCode = (TextBox)gvDiagnosis.FooterRow.FindControl("inDiagnosisCode");
            //TextBox DiagnosisDesc = (TextBox)gvDiagnosis.FooterRow.FindControl("inDiagnosisDesc");
            //TextBox DiagnosisDate = (TextBox)gvDiagnosis.FooterRow.FindControl("inDiagnosisDate");
        }
    }

    private void BindGridwithDummy()
    {
        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new System.Data.DataColumn("ODSDiagnosisID", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("DiagnosisTypeCode", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("DiagnosisCode", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("DiagnosisCodeDesc", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("txtDiagnosisDate", typeof(String)));


        dr = dt.NewRow();
        dr[0] = "1";  //Adds the Dummy Data in the Row
        dr[1] = "";  //Adds the Dummy Data in the Row
        dr[2] = "";  //Adds the Dummy Data in the Row
        dr[3] = "";  //Adds the Dummy Data in the Row
        dr[4] = "";  //Adds the Dummy Data in the Row
        dt.Rows.Add(dr);


        // Show the DataTable values in the GridView

        //gvDiagnosis.DataSource = dt;
        //gvDiagnosis.DataBind();

    }


    protected void lnkDiagnosisCodeSearch_Click(object sender, EventArgs e)
    {

    }
    protected void btnDiagnosisDetailAdd_Click(object sender, EventArgs e)
    {
        //pnlsepDiagnosissearch.Visible = true;
        //pnlDiagnosissearch.Visible = true;

    }

    protected void btnDiagDelete_Click(object sender, EventArgs e)
    {

    }
    protected void textbox_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        string str = txt.ID;
    }
    public int NumControls
    {
        get { return (int)ViewState["NumControls"]; }
        set { ViewState["NumControls"] = value; }
    }

    protected void lnkSerProviderInfoSearch_Click(object sender, EventArgs e)
    {

    }


    protected void lnkOrdProvinfoSearch_Click(object sender, EventArgs e)
    {

    }
    //#region Section
    //private enum PopupName1{ PriorAuthDiagnosisSeach = 0 };
    //#endregion
    protected void lnkDiagnosisSearch_Click(object sender, EventArgs e)
    {

    }

    protected void ReportBirthDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtBirthDate.Text, true) && Helper.IsValidDate(txtBirthDate.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtBirthDate.Text).Subtract(Convert.ToDateTime(txtBirthDate.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }

    protected void ReportAdmissionDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtAdmissionDate.Text, true) && Helper.IsValidDate(txtAdmissionDate.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtAdmissionDate.Text).Subtract(Convert.ToDateTime(txtAdmissionDate.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        if (((System.Web.UI.Control)sender).ID == "lnkservicedetail")
        {
            // pnlline.Visible = true;
            pnlsepline.Visible = true;
            //pnlDentalLine.Visible = false;
            pnlsepDentalLine.Visible = false;
            GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
            txtRequestUnt.Text = grdrow.Cells[3].Text;
            txtAuthorizedUnits.Text = grdrow.Cells[4].Text;
            txtRequestedDollars.Text = grdrow.Cells[5].Text;
            txtAuthorizedDollars.Text = grdrow.Cells[6].Text;
            txtReqFDOS.Text = grdrow.Cells[7].Text;
            txtReqTDOS.Text = grdrow.Cells[8].Text;
            txtAuthorizedFromDOS.Text = grdrow.Cells[9].Text;
            txtAuthorizedToDOS.Text = grdrow.Cells[10].Text;

        }
        else
        {
            // pnlDentalLine.Visible = true;
            pnlsepDentalLine.Visible = true;

            //  pnlline.Visible = false;
            pnlsepline.Visible = false;
        }
        if (((System.Web.UI.Control)sender).ID == "lnkdental")
        {
            //pnlDentalLine.Visible = true;
            pnlsepDentalLine.Visible = true;
            // pnlline.Visible = false;
            pnlsepline.Visible = false;
            GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
            // ddlServiceTypeCode.Text = grdrow.Cells[1].Text;
            //txtServiceCod.Text = grdrow.Cells[2].Text;
            //txttoothNumber.Text = grdrow.Cells[3].Text;
            //txtQuadrant.Text = grdrow.Cells[4].Text;
            //txtdentalStatus.Text = grdrow.Cells[0].Text;
            //txtdentalAssociatedPANumber.Text = grdrow.Cells[0].Text;
            //txtDentalRequestUnt.Text = grdrow.Cells[5].Text;
            //txtRequestDollar.Text = grdrow.Cells[7].Text;
            txtDentalReqFDOS.Text = grdrow.Cells[9].Text;
            txtDentalReqTDOS.Text = grdrow.Cells[10].Text;
            //txtAuthorizedUnit.Text = grdrow.Cells[6].Text;
            //txtAuthDoll.Text = grdrow.Cells[8].Text;
            //txtAuthTdo.Text = grdrow.Cells[11].Text;
            //txtAuthFDOS.Text = grdrow.Cells[12].Text;
        }



        //cpeLine.Visible = true;
    }
    protected void LinkButton4_Click(object sender, EventArgs e)
    {
        if (((System.Web.UI.Control)sender).ID == "lnkOrdProvinfoSearch")
        {
            //pnlORDERPROVIDERINFOSearch.Visible = true;
            //sepORDERPROVIDERINFOSearch.Visible = true;
            //sepDiagnosissearch.Visible = false;
            //pnlDiagnosissearch.Visible = false;
            // pnlline.Visible = false;
            pnlsepline.Visible = false;
            // pnlDentalLine.Visible = false;
            pnlsepDentalLine.Visible = false;
            //  GetORDERPROVIDERINFOSearch();
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            //DataSet dataSet = _spa.GetPrioHospitalData("SERVICE_DETAIL", PrioHospitalId);
            //DataTable dt = dataSet.Tables[0];
            var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
            var dataTable = ds.Tables["AuthorizationInfo"];

            //gvORDERPROVIDERINFOSearch.DataSource = dataTable;
            //gvORDERPROVIDERINFOSearch.DataBind();
            //GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
            if (dataTable.Rows.Count > 0)
            {
                txtNPI.Text = dataTable.Rows[0]["OrderingProviderID"].ToString();
                txtProMedicaidID.Text = dataTable.Rows[0]["MemberID"].ToString();
                //txtBusinessLastName.Text = dataTable.Rows[0][""].ToString();
                //txtFirstName.Text = dataTable.Rows[0][""].ToString();
            }


        }

        //this.LoadData(null);
        //DataRow dr = null;
        //lblServiceSearch.Text = "ORDERING PROVIDER INFORMATION Search";

        //mltservicesearch.ActiveViewIndex = 0;

        //mpesearch.Show();



    }


    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        if (((System.Web.UI.Control)sender).ID == "lnkserviceprovider")
        {
            SepServiceProviderInfo.Visible = true;
            pnlServiceProviderInfo.Visible = true;
            //pnlORDERPROVIDERINFOSearch.Visible = false;
            //sepORDERPROVIDERINFOSearch.Visible = false;
            //sepDiagnosissearch.Visible = false;
            //pnlDiagnosissearch.Visible = false;
            //  pnlline.Visible = false;
            pnlsepline.Visible = false;
            // pnlDentalLine.Visible = false;
            pnlsepDentalLine.Visible = false;
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
            var dataTable = ds.Tables["AuthorizationInfo"];
            gvServiceProviderInfoSearch.DataSource = dataTable;
            gvServiceProviderInfoSearch.DataBind();

            if (dataTable.Rows.Count > 0)
            {
                txtserviceNPI.Text = dataTable.Rows[0]["RenderingProviderID"].ToString();
                txtServiceMedicaidID.Text = dataTable.Rows[0]["MemberID"].ToString();
                //txtservicebussiness.Text = dataTable.Rows[0][""].ToString();
                //txtserviceFirstName.Text = dataTable.Rows[0][""].ToString();
            }
        }


        //this.LoadData(null);
        //DataRow dr = null;
        //lblServiceSearch.Text = "*Servicing Provider Information Search";

        //mltservicesearch.ActiveViewIndex = 0;

        //mpesearch.Show();



    }
    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        if (((System.Web.UI.Control)sender).ID == "lnkDiagnosis")
        {
            //sepDiagnosissearch.Visible = true;
            //pnlDiagnosissearch.Visible = true;
            // pnlline.Visible = false;
            pnlsepline.Visible = false;
            // pnlDentalLine.Visible = false;
            pnlsepDentalLine.Visible = false;
            GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
            //  txtDiagnosisCode.Text = grdrow.Cells[3].Text;
            // ddlICDVersion.SelectedValue = grdrow.Cells[2].Text;
            // txtDiagnosisCodeDesc.Text = grdrow.Cells[4].Text;

        }
        else
        {
            //sepDiagnosissearch.Visible = true;
            //pnlDiagnosissearch.Visible = true;
            // pnlline.Visible = false;
            pnlsepline.Visible = false;
            // pnlDentalLine.Visible = false;
        }
        this.LoadData(null);
        //DataRow dr = null;
        lbldiagnosissearch.Text = "*Diagnosis Code Seach";
        mltdiagnosissearch.ActiveViewIndex = 0;
        mpediagnosissearch.Show();

    }

    protected void lnkServiceCodeSearch_Click(object sender, EventArgs e)
    {
        this.LoadData(null);
        //lblDentalHSCPSCodeSearch.Text = "*Service Code Search";
        //mltDentalHSCPSCodeSearch.ActiveViewIndex = 0;

        mpeServiceDetailsRevenueCodeSearch.Show();
    }

    protected void htnRevenueCodeClosePopup_Click(object sender, EventArgs e)
    {
        txtServDetRevCodeDesc.Text = string.Empty;
        txtHCPCSCode.Text = string.Empty;
        //gvHSCPSCodeSearch.DataSource = null;
        //gvHSCPSCodeSearch.DataBind();
        //gvHSCPSCodeSearch.EmptyDataText = string.Empty;
    }

    protected void ddlServiceTypeCode_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void grdORDERPROVIDERINFOSearch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void grdServiceProviderInfoSearch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// This Method will get Service Code Type
    /// </summary>
    private void GetServiceCodeType()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            DataSet dataSet = _spa.GetServiceCodeType();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable dt = dataSet.Tables[0];

                ddlServiceType.Items.Clear();
                Helper.LoadList(ddlServiceType, dt, "PRIOR_AUTH_SERVICE_TYPE_DESC", "PRIOR_AUTH_SERVICE_TYPE_ID", false);
                ddlServiceType.Items.Insert(0, new ListItem("--- Please select ---", String.Empty));
            }
            else
            {
                string logHeader = "GetServiceCodeType DDL - Submit PA";
                string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                logMsg += "dataset is empty";
                CreateAndReturnLogThreadNumber(new Exception(), "SubmitPA-GetServiceCodeType");
                //Logging log = new Logging(new Guid(CON.WebPageLogGuid.PriorAuthWebPageLog), logMsg);
                //log.CreateLogEntry(logHeader, Logging.LogPriority.Error);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetServiceCodeType method", ex);
        }
    }

    private void GetServiceTypeCode() //Ap 3/1/2022
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            DataSet dataSet = _spa.GetServiceTypeCode();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable dt = dataSet.Tables[0];

                ddlServiceTypeCode.Items.Clear();
                Helper.LoadList(ddlServiceTypeCode, dt, "PRIOR_AUTH_SERVICE_CODE_TYPE_CODE", "PRIOR_AUTH_SERVICE_CODE_TYPE_ID", false);
                ddlServiceTypeCode.Items.Insert(0, new ListItem("--- Please select ---", String.Empty));
            }
            else
            {
                string logHeader = "GetServiceTypeCode DDL - Submit PA";
                string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                logMsg += "dataset is empty";
                CreateAndReturnLogThreadNumber(new Exception(), "SubmitPA-GetServiceCodeTypeCode");
                //Logging log = new Logging(new Guid(CON.WebPageLogGuid.PriorAuthWebPageLog), logMsg);
                //log.CreateLogEntry(logHeader, Logging.LogPriority.Error);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetServiceTypeCode method", ex);
        }
    }

    private void GetServiceCodeTypeCodeForPriorAuth()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            DataSet dataSet = _spa.GetServiceCodeTypeForPriorAuth();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable dt = dataSet.Tables[0];
                ddlServiceTypeCode.Items.Clear();
                Helper.LoadList(ddlServiceTypeCode, dt, "PRIOR_AUTH_SERVICE_CODE_DESC", "PRIOR_AUTH_SERVICE_CODE_ID", false);
                ddlServiceTypeCode.Items.Insert(0, new ListItem("--- Please select ---", String.Empty));
            }
            else
            {
                string logHeader = "GetServiceCodeTypeCodeForPriorAuth DDL - Submit PA";
                string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                logMsg += "dataset is empty";
                CreateAndReturnLogThreadNumber(new Exception(), "SubmitPA-GetServiceCodeTypeCodeForPriorAuth");
                //Logging log = new Logging(new Guid(CON.WebPageLogGuid.PriorAuthWebPageLog), logMsg);
                //log.CreateLogEntry(logHeader, Logging.LogPriority.Error);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetServiceCodeTypeCodeForPriorAuth method", ex);
        }
    }

    /// <summary>
    /// This Method will getPricingFormula
    /// </summary>
    private void GetPricingFormula()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            DataSet dataSet = _spa.GetPricingFormula();
            DataTable dt = dataSet.Tables[0];
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetPricingFormula method", ex);
        }
    }
    protected void RequestedFDOS_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtDentalReqFDOS.Text, true) && Helper.IsValidDate(txtDentalReqFDOS.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtDentalReqFDOS.Text).Subtract(Convert.ToDateTime(txtDentalReqFDOS.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }
    protected void RequestedTDOS_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtDentalReqTDOS.Text, true) && Helper.IsValidDate(txtDentalReqTDOS.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtDentalReqTDOS.Text).Subtract(Convert.ToDateTime(txtDentalReqTDOS.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }
    protected void ddlPricingFormula_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void lnkHcpsSearch_Click(object sender, EventArgs e)
    {

        this.LoadData(null);
        //lblDentalHSCPSCodeSearch.Text = "*Service Code Search";
        //mltDentalHSCPSCodeSearch.ActiveViewIndex = 0;
        mpeServiceDetailsRevenueCodeSearch.Show();

    }
    protected void ddlICDVersion_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    //protected void gvPriorAuthSearchDiagnosis_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    gvPriorAuthSearchDiagnosis.PageIndex = e.NewPageIndex;
    //    BindGrid();
    //    gvPriorAuthSearchDiagnosis.EditIndex = -1;
    //}
    protected void DiagnosissearchAdd_Click(object sender, EventArgs e)
    {

        //BindGrid();
    }

    private void PARecipientInformation(string medicaidbillingnumber, string dateofBirth)
    {
        DataSet ds = RegistrationController.SelectProviderByGRPMedicaidID(this.WorkflowPage.MedicaidID);
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
        if (Helper.HasRows(dtMisc))
        {
            DataRow dr = dtMisc.Rows[0];
            var npi = Helper.GetString("NPI", dr);
            try
            {
                RecipientEligibilitySearchResponse recipientInfo = null;
                //if the data has changed either Billing# or DOB then make the call
                if (txtMedicaidBillingNumberPrevious.Value != medicaidbillingnumber || txtBirthDatePrevious.Value != dateofBirth)
                {
                    string medicaidID = this.WorkflowPage.MedicaidID;
                    Guid userId = Helper.GetUserId(HttpContext.Current.User.Identity.Name.ToString());
                    recipientInfo = new RecipientEligibilitySearchReqRes().SearchRequest(string.Empty, medicaidID, userId, dateofBirth, DateTime.Now.AddMonths(-48).ToString("MM/dd/yyyy"), DateTime.Now.ToString("MM/dd/yyyy"), string.Empty, medicaidbillingnumber, "PriorAuthEligibility");
                }
                else
                {
                    //Check if session exist then load from session else make the call
                    if (Session["RecipientEligibilityResponse"] != null)
                    {
                        recipientInfo = (RecipientEligibilitySearchResponse)Session["RecipientEligibilityResponse"];
                    }
                    else
                    {
                        string medicaidID = this.WorkflowPage.MedicaidID;
                        Guid userId = Helper.GetUserId(HttpContext.Current.User.Identity.Name.ToString());
                        recipientInfo = new RecipientEligibilitySearchReqRes().SearchRequest(string.Empty, medicaidID, userId, dateofBirth, DateTime.Now.AddMonths(-48).ToString("MM/dd/yyyy"), DateTime.Now.ToString("MM/dd/yyyy"), string.Empty, medicaidbillingnumber, "PriorAuthEligibility");
                    }
                }

                if (recipientInfo != null && recipientInfo.RecipientInfo != null)
                {
                    Session["RecipientEligibilityResponse"] = recipientInfo;
                    txtMedicaidBillingNumberPrevious.Value = medicaidbillingnumber;
                    txtBirthDatePrevious.Value = dateofBirth;

                    txtLastName2.Text = recipientInfo.RecipientInfo.LastName;
                    txtfrstmi2.Text = recipientInfo.RecipientInfo.FirstName;
                    txtGender2.Text = recipientInfo.RecipientInfo.Gender;
                    txtAddress1.Text = recipientInfo.RecipientInfo.AddressLine1;
                    lblAddress2.Text = recipientInfo.RecipientInfo.AddressLine2;
                    txtMiddleName.Text = (recipientInfo.RecipientInfo.MiddleName != null) ? recipientInfo.RecipientInfo.MiddleName : string.Empty;
                    txtCity.Text = recipientInfo.RecipientInfo.City;
                    txtState.Text = recipientInfo.RecipientInfo.StateCode;
                    txtZipCode.Text = recipientInfo.RecipientInfo.ZipCode5;
                    if (recipientInfo.ErrorDetails != null && recipientInfo.ErrorDetails.Count > 0)
                    {
                        StringBuilder errorMsg = new StringBuilder();
                        foreach (var ed in recipientInfo.ErrorDetails)
                        {
                            if (errorMsg.Length > 0)
                            {
                                errorMsg.Append("; ");
                                errorMsg.Append(ed.Code + " : " + ed.Description);
                            }
                            else
                                errorMsg.Append(ed.Code + " : " + ed.Description);
                        }
                        lblErrorMsg.Text = errorMsg.ToString();
                    }
                    else
                        lblErrorMsg.Text = "";
                }
                else
                {
                    ClearRecipientInfo();
                    lblErrorMsg.Text = "Error: An error occurred while processing the request";
                }
            }
            catch (FaultException ex)
            {
                lblErrorMsg.Text = "Error: An error occurred while processing the request - " + ex.Message;
                ClearRecipientInfo();
            }
            catch (Exception ex)
            {
                lblErrorMsg.Text = "Error: An error occurred while processing the request";
                ClearRecipientInfo();
            }
        }
    }

    private void ClearRecipientInfo()
    {
        txtfrstmi2.Text = string.Empty;
        txtLastName2.Text = string.Empty;
        txtGender2.Text = string.Empty;
        txtAddress1.Text = string.Empty;
        lblAddress2.Text = string.Empty;
        txtCity.Text = string.Empty;
        txtState.Text = string.Empty;
        txtZipCode.Text = string.Empty;
        txtMiddleName.Text = string.Empty;
    }


    protected void txtMedicaidBillingNumber_TextChanged(object sender, EventArgs e)
    {

        hdnModified.Value = "true";
        var isValid = ValidateMedicaidBillingNumber();
        if (isValid)
        {
            //checking both fields are not null else it may throw exception 
            if (!string.IsNullOrEmpty(txtMedicaidBillingNumber.Text.Trim()) &&
                !string.IsNullOrEmpty(txtBirthDate.Text.Trim()))
            {
                DateTime dtTemp;
                if (DateTime.TryParseExact(txtBirthDate.Text, "MM/dd/yyyy", null, DateTimeStyles.None, out dtTemp))
                {
                    PARecipientInformation(txtMedicaidBillingNumber.Text.Trim(), txtBirthDate.Text.Trim());
                    lblMBErrorMSG.Text = string.Empty;
                    lblMBErrorMSG.Visible = false;
                }
            }
            lblMBErrorMSG.Text = string.Empty;
            lblMBErrorMSG.Visible = false;

        }
        else
        {
            ClearRecipientInfo();
        }
        if (valerrormess.Visible)
        {
            if (!ReValidateData())
            {
                return;
            }
        }
    }

    public bool ValidateMedicaidBillingNumber()
    {
        bool isValid = true;
        string medicaidId = txtMedicaidBillingNumber.Text;

        if (string.IsNullOrEmpty(txtMedicaidBillingNumber.Text.Trim()))
        {
            lblMBErrorMSG.Visible = true;
            lblMBErrorMSG.Text = "*12-digit number is required";
            isValid = false;
        }
        else if ((txtMedicaidBillingNumber.Text.Trim()).Length != 12)
        {
            lblMBErrorMSG.Visible = true;
            lblMBErrorMSG.Text = "*12-digit number is required";
            isValid = false;
        }

        return isValid;
    }

    protected void txtBirthDate_TextChanged(object sender, EventArgs e)
    {
        hdnModified.Value = "true";
        lblDOBErrMsg.Text = string.Empty;

        if (!string.IsNullOrEmpty(txtBirthDate.Text.Trim()))
        {
            DateTime dtTemp;
            if (!DateTime.TryParseExact(txtBirthDate.Text, "MM/dd/yyyy", null, DateTimeStyles.None, out dtTemp))
            {
                lblDOBErrMsg.Visible = true;
                lblDOBErrMsg.Text = "Date of Birth should be valid date";
                ClearRecipientInfo();
                return;
            }

            var isValid = ValidateBirthDate();
            if (isValid)
            {
                //checking both fields are not null else it may throw exception 
                if (!string.IsNullOrEmpty(txtMedicaidBillingNumber.Text.Trim()) &&
                    !string.IsNullOrEmpty(txtBirthDate.Text.Trim()))
                {
                    if (ValidateMedicaidBillingNumber())
                    {
                        PARecipientInformation(txtMedicaidBillingNumber.Text.Trim(), txtBirthDate.Text.Trim());
                    }
                }

            }
            else
            {
                lblDOBErrMsg.Visible = true;
                lblDOBErrMsg.Text = "*Date of birth cannot be greater than today's date.";
                ClearRecipientInfo();
            }

        }
        else
        {
            ClearRecipientInfo();
        }
        if (valerrormess.Visible)
        {
            if (!ReValidateData())
            {
                return;
            }
        }
    }

    public bool ValidateBirthDate()
    {
        bool isValid = true;

        if (String.IsNullOrEmpty(txtBirthDate.Text))
        {
            AddValidationErrorMessage("*Recipient date of birth is required.");
            isValid = false;
        }

        if (!String.IsNullOrEmpty(txtBirthDate.Text) && Convert.ToDateTime(txtBirthDate.Text) > DateTime.Now)
        {
            AddValidationErrorMessage("*Date of birth cannot be greater than today's date.");
            isValid = false;
        }
        return isValid;
    }

    #region Service Details ProcedureCode/Revenue Code Search Popups

    protected void gvServiceDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DateTime reqFDOS = (DateTime)DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS");
            DateTime reqTDOS = (DateTime)DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS");
            string serviceCode = DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_SERVICE_CODE_TYPE_ID").ToString();

            TextBox txtservicedetailsReqFDOS = (e.Row.FindControl("txtservicedetailsReqFDOS") as TextBox);
            TextBox txtservicedetailsReqTDOS = (e.Row.FindControl("txtservicedetailsReqTDOS") as TextBox);
            Label lblSerDetprocCodeType = (e.Row.FindControl("lblSerDetprocCodeType") as Label);

            ListItem listItem = ddlServiceTypeCode.Items.FindByValue(Convert.ToString(serviceCode));

            if (listItem != null && lblSerDetprocCodeType != null)
            {
                lblSerDetprocCodeType.Text = Convert.ToString(listItem.Text);
            }

            if (txtservicedetailsReqFDOS != null)
            {
                txtservicedetailsReqFDOS.Text = reqFDOS.ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
            }

            if (txtservicedetailsReqTDOS != null)
            {
                txtservicedetailsReqTDOS.Text = reqTDOS.ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
            }
            string statusCode = DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_STATUS_ID").ToString();
            string statusId = DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_STATUS_ID").ToString();
            TextBox txtServiceStatusCode = (e.Row.FindControl("txtServiceStatusCode") as TextBox);
            txtServiceStatusCode.Text = statusId == "1" ? "Submission Pending" : statusId == "2" ? "Approved" : statusId == "3" ? "Denied" : statusCode;

            Label lblServiceLinelabel = e.Row.FindControl("lblServiceLine") as Label;
            if (lblServiceLinelabel != null)
            {
                lblServiceLinelabel.Enabled = true;
            }
        }
    }

    private void ClearServiceDetailsControls()
    {
        hdLineNumber.Value = "";
        txtServicecode.Text = "";
        txtRequestUnt.Text = "";
        txtAuthorizedUnits.Text = "";
        txtRequestedDollars.Text = "";
        txtAuthorizedDollars.Text = "";
        txtReqFDOS.Text = "";
        txtReqTDOS.Text = "";
        txtAuthorizedFromDOS.Text = "";
        txtAuthorizedToDOS.Text = "";
        if (ddlServiceTypeCode != null && ddlServiceTypeCode.Items.Count > 0)
            ddlServiceTypeCode.Items[0].Selected = true;
        txtLnProcedureCode.Text = "";
        txtRequestUnt.Text = "";
        if (ddLnRequestUnits != null && ddLnRequestUnits.Items.Count > 0)
            ddLnRequestUnits.Items[0].Selected = true;
        txtAuthorizedUnits.Text = "";
        txtLnprocCodeDesc.Text = "";
        txtLnProviderServiceNote.Text = "";
        ddLnLevelOfCare.Items[0].Selected = true;
        txtLnRemainingUnits.Text = "";
        txtLnServiceTrackingNo.Text = "";
        txtLnStatus.Text = "";
    }
    protected void btnSearchProcCodeSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text) && string.IsNullOrWhiteSpace(txtPlaceOfServiceName.Text))
            {
                lblProcCodeError.Visible = true;
                lblProcCodeError.Text = "Procedure code and/or description is required";
                mpeSubmitPriorAuthSearchProc.Show();
                return;
            }
            else
            {
                lblProcCodeError.Visible = false;
                lblProcCodeError.Text = "";
            }
            // Clear these out prior to doing the Search
            this.lblSResult.Visible = true;
            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SessionVarRetriever.DashBoardTableId = 0;
            //  this.gvSubmitClaimSearchProcPop.CurrentPageIndex = 0;
            RefreshProcedureCodeSearchData();
            mpeSubmitPriorAuthSearchProc.Show();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }
    public void RefreshProcedureCodeSearchData()
    {
        string logHeader = string.Format("SubmitPriorAuthorization -> RefreshProcedureCodeSearchData()");
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

        try
        {
            DataTable dt = null;  //GetProcdeureCodeData(gvSubmitClaimSearchProcPop.PageSize);
            //gvSubmitClaimSearchProcPop.DataSource = dt;
            //gvSubmitClaimSearchProcPop.DataBind();
            //gvSubmitClaimSearchProcPop.Visible = true;
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetProcdeureCodeData");
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }
    private DataTable GetProcdeureCodeData(int pageSize)
    {
        try
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            string sortColWithDirection = default(string); //this.gvSubmitClaimSearchProcPop.GridViewSortDirection == SortDirection.Descending ? gvSubmitClaimSearchProcPop.GridViewSortColumn + " DESC" : gvSubmitClaimSearchProcPop.GridViewSortColumn;

            // If list of IDs has been passed in, display in search results list.
            //write sp for getting records
            DataTable dt = GetProcedureCodeData();

            if (Helper.HasRows(dt))
            {
                return dt;
            }
            else return new DataTable();
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetProcedureCodeData");
            return new DataTable();
        }
    }
    private DataTable GetProcedureCodeData()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            // DataSet dataSet = _spa.GetProcedureCodePopupSearch(txtCode.Text, txtPlaceOfServiceName.Text);
            DataTable dataSet = LookupTableController.GetProcedureCodeServiceDetail(txtCode.Text, txtPlaceOfServiceName.Text);
            if (dataSet != null && dataSet.Rows.Count > 0)
                return dataSet;
            else
                return new DataTable();
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetProcedureCodeData");
            return new DataTable();
        }
    }
    protected void gvSubmitClaimSearchPop_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            RefreshProcedureCodeSearchData();
            //gvSubmitClaimSearchProcPop.PageIndex = e.NewPageIndex;
            //gvSubmitClaimSearchProcPop.EditIndex = -1;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }
    protected void gvSubmitClaimSearchPop_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            RefreshProcedureCodeSearchData();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }
    private void GetLvelOfCare()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            ddLnLevelOfCare.Items.Clear();
            DataSet dataSet = _spa.GetLevelOfCare();
            DataTable dt = dataSet.Tables[0];

            dt.Columns.Add("PRIOR_AUTH_LEVEL_CARE_TEXT", typeof(System.String));

            foreach (DataRow dr in dt.Rows)
            {
                dr["PRIOR_AUTH_LEVEL_CARE_TEXT"] = dr["PRIOR_AUTH_LEVEL_CARE_ID"] + "-" + dr["PRIOR_AUTH_LEVEL_CARE_DESC"];
            }

            ddLnLevelOfCare.DataSource = dt;
            ddLnLevelOfCare.DataValueField = "PRIOR_AUTH_LEVEL_CARE_ID";
            ddLnLevelOfCare.DataTextField = "PRIOR_AUTH_LEVEL_CARE_TEXT";
            ddLnLevelOfCare.DataBind();
            ddLnLevelOfCare.Items.Insert(0, new ListItem(string.Empty, string.Empty));
            ddLnLevelOfCare.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetLevelOfCare method", ex);
        }
    }

    private void GetRequestedUnitMeasures()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            ddLnRequestUnits.Items.Clear();
            ddProffSDMeasurements.Items.Clear();
            DataSet dataSet = _spa.GetRequestedUnitMeasures();
            DataTable dt = dataSet.Tables[0];
            dt.Columns.Add("PRIOR_AUTH_REQUESTED_UNITS", typeof(System.String));

            foreach (DataRow dr in dt.Rows)
            {
                dr["PRIOR_AUTH_REQUESTED_UNITS"] = dr["PRIOR_AUTH_REQUESTED_UNITS_CODE"] + "-" + dr["PRIOR_AUTH_REQUESTED_UNITS_DESC"];
            }

            DataTable newSortedDt = dataSet.Tables[0].AsEnumerable().Where(row => row.Field<String>("PRIOR_AUTH_REQUESTED_UNITS_DESC") != "Days").CopyToDataTable();
            DataView dataView = newSortedDt.DefaultView;
            dataView.Sort = "PRIOR_AUTH_REQUESTED_UNITS desc";
            ddLnRequestUnits.DataSource = dataView;

            ddLnRequestUnits.DataValueField = "PRIOR_AUTH_REQUESTED_UNITS_ID";
            ddLnRequestUnits.DataTextField = "PRIOR_AUTH_REQUESTED_UNITS";
            ddLnRequestUnits.DataBind();

            newSortedDt = dataSet.Tables[0].AsEnumerable().Where(row => row.Field<String>("PRIOR_AUTH_REQUESTED_UNITS_DESC") != "Days").CopyToDataTable();
            dataView = newSortedDt.DefaultView;
            dataView.Sort = "PRIOR_AUTH_REQUESTED_UNITS desc";

            ddProffSDMeasurements.DataSource = dataView;
            ddProffSDMeasurements.DataValueField = "PRIOR_AUTH_REQUESTED_UNITS_ID";
            ddProffSDMeasurements.DataTextField = "PRIOR_AUTH_REQUESTED_UNITS";
            ddProffSDMeasurements.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetRequestedUnitMeasures method", ex);
        }
    }
    private DataTable GetRevenueCodeData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        string sortColWithDirection = ""; //this.gvHSCPSCodeSearch.GridViewSortDirection == SortDirection.Descending ? gvHSCPSCodeSearch.GridViewSortColumn + " DESC" : gvHSCPSCodeSearch.GridViewSortColumn;
        try
        {
            // If list of IDs has been passed in, display in search results list.
            //write sp for getting records
            DataTable dt = GetRevenueCodeData();

            if (Helper.HasRows(dt))
            {
                return dt;
            }
            else return new DataTable();
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetRevenueCodeData");
            return new DataTable();
        }
    }
    private DataTable GetRevenueCodeData()
    {
        string logHeader = string.Format("SubmitPriorAuthorization -> GetRevenueCodeData()");
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            DataSet dataSet = _spa.GetRevenueCodePopupSearch(txtHCPCSCode.Text, txtServDetRevCodeDesc.Text);

            if (dataSet != null && dataSet.Tables.Count > 0)
                return dataSet.Tables[0];
            else
                return new DataTable();
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetRevenueCodeData");
            return new DataTable();
        }
    }

    public void RefreshRevenueCodeSearchData()
    {
        string logHeader = string.Format("SubmitPriorAuthorization -> RefreshRevenueCodeSearchData()");
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

        try
        {
            DataTable dt = null;// GetRevenueCodeData(gvHSCPSCodeSearch.PageSize);
            //gvHSCPSCodeSearch.DataSource = dt;
            //gvHSCPSCodeSearch.DataBind();
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetRevenueCodeData");
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }

    }
    protected void btnRevCodeSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtHCPCSCode.Text) && string.IsNullOrWhiteSpace(txtServDetRevCodeDesc.Text))
            {
                lblRevenueCodeError.Visible = true;
                lblRevenueCodeError.Text = "Revenue code and/or description is required";
                mpeServiceDetailsRevenueCodeSearch.Show();
                return;
            }
            else
            {
                lblRevenueCodeError.Visible = false;
                lblRevenueCodeError.Text = "";
            }

            //  gvHSCPSCodeSearch.EmptyDataText = string.Empty;

            this.lblSResult.Visible = true;
            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SessionVarRetriever.DashBoardTableId = 0;
            // this.gvHSCPSCodeSearch.CurrentPageIndex = 0;
            RefreshRevenueCodeSearchData();
            mpeServiceDetailsRevenueCodeSearch.Show();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }
    protected void lnkServDetailsPopUpprocCode_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;

        if (Convert.ToString(Session["ProcCodeFrom"]) == "Institutional")
        {
            txtLnProcedureCode.Text = cmdArgument;
            lblServDetailsProcCode.Visible = false;
            lblServDetailsProcCode.Text = "";
        }
        if (Convert.ToString(Session["ProcCodeFrom"]) == "Dental")
        {
            txtDentalSDProcCode.Text = cmdArgument;
            lblDentalProcMessage.Visible = false;
            lblDentalProcMessage.Text = "";
        }
        if (Convert.ToString(Session["ProcCodeFrom"]) == "Professional")
        {
            txtProfessionalSDProcCode.Text = cmdArgument;
            lblProfProcMessage.Visible = false;
            lblProfProcMessage.Text = "";
        }

        mpeSubmitPriorAuthSearchProc.Hide();
    }

    protected void lnkServDetailsRevCode_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;
        txtServicecode.Text = cmdArgument;
        txtHCPCSCode.Text = string.Empty;
        txtServDetRevCodeDesc.Text = string.Empty;
        //gvHSCPSCodeSearch.DataSource = null;
        //gvHSCPSCodeSearch.DataBind();
        mpeServiceDetailsRevenueCodeSearch.Hide();
    }

    protected void btnServDetAddEditCancel_Click(object sender, EventArgs e)
    {
        ddlServiceTypeCode.ClearSelection();
        ddLnRequestUnits.ClearSelection();
        ddLnLevelOfCare.ClearSelection();
        //pnlline.Visible = false;
        pnlsepline.Visible = false;
        cpeServiceDetail.Collapsed = false;
        cpeServiceDetail.ClientState = "false";
        ClearServiceDetailsControls();
    }

    protected void lnkPlaceofServiceSearchDetail1_Click(object sender, EventArgs e)
    {
        // this.gvSubmitClaimSearchProcPop.CurrentPageIndex = 0;
        //RefreshProcedureCodeSearchData();
        Session["ProcCodeFrom"] = "Institutional";
        mpeSubmitPriorAuthSearchProc.Show();
    }


    private void GetPriorAttachmentDocumentType()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            ddlPriorAuthDocType.Items.Clear();
            DataSet dataSet = _spa.GetClaimDocumentTypeByClaimTransactionType(4);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable dt = dataSet.Tables[0];

                ddlPriorAuthDocType.DataSource = dt;
                ddlPriorAuthDocType.DataValueField = "DOCUMENT_TYPE_ID";
                ddlPriorAuthDocType.DataTextField = "DOCUMENT_TYPE_DESC";
                ddlPriorAuthDocType.DataBind();
                ddlPriorAuthDocType.Items.Insert(0, new ListItem("--- select Document Type ---", String.Empty));
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetPriorAttachmentDocumentType method", ex);
        }
    }

    #endregion

    protected void btnAttachmentDelete_Command(object sender, CommandEventArgs e)
    {
        try
        {
            int documentId = Convert.ToInt32(e.CommandArgument);
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            _spa.DeletePriorAuthAttachment(documentId);

            DataSet dsValues = (DataSet)Session[ATTACHMENT_RETENTION_DATA];
            if (dsValues != null)
            {
                DataTable dtAttachments = dsValues.Tables[0];
                if (dtAttachments.Columns.IndexOf("RowState") < 0)
                {
                    DataColumn dcRowState = new DataColumn("RowState", typeof(string));
                    dtAttachments.Columns.Add(dcRowState);
                }

                DataRow dr = dsValues.Tables[0].Select("PRIOR_AUTH_SUB_ATTACHMENT_ID='" + documentId + "'")[0];
                if (dr["RowState"].ToString() == ROWSTATE_ADD)
                {
                    dsValues.Tables[0].Rows.Remove(dr);
                }
                else
                {
                    dr["RowState"] = ROWSTATE_DELETE;
                }
                Session[ATTACHMENT_RETENTION_DATA] = dsValues;
            }

            BindAttachmentGrid();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void uploadAttachment()
    {
        lblInstUploadErrMsg.Text = string.Empty;
        lblInstDocTypeErrMsg.Text = string.Empty;

        //if (gvAttachment.Rows.Count == 10)
        //{
        //    lblAttachmentErrorMsg.Visible = true;
        //    lblAttachmentErrorMsg.Text = "Maximum of 10 attachments can be submitted";
        //    return;
        //}
        if (string.IsNullOrEmpty(txtMedicaidBillingNumber.Text))
        {
            lblAttachmentErrorMsg.Visible = true;
            lblAttachmentErrorMsg.Text = "Medicaid Billing number is required.";
            return;
        }
        if (ddlPriorAuthDocType.SelectedIndex == 0)
        {
            lblInstDocTypeErrMsg.Visible = true;
            lblInstDocTypeErrMsg.Text = "Document type is required.";
        }

        else
        {
            lblInstDocTypeErrMsg.Visible = false;
            lblInstDocTypeErrMsg.Text = "";
        }

        if (lblInstDocTypeErrMsg.Visible || lblInstUploadErrMsg.Visible)
        {
            return;
        }

        if (ValidateAttachment("Institutional"))
        {
            return;
        }

        if (string.IsNullOrEmpty(PriorAttachmentUpload.FileName))
        {
            lblInstDocTypeErrMsg.Visible = true;
            lblInstDocTypeErrMsg.Text = "Document name is required.";
        }
        else
        {
            lblInstDocTypeErrMsg.Visible = false;
            lblInstDocTypeErrMsg.Text = "";
        }

        lblAttachmentErrorMsg.Text = "";
        var fileBytes = PriorAttachmentUpload.FileBytes;
        var docType = ddlPriorAuthDocType.SelectedValue;
        var note = txtPriorAttachmentNote.Text.Trim();
        string tradingPartnerID = GetTradingPartnerID();
        int documentId = UploadPriorAuthAttachment(lblAttachmentStatusMsg, PriorAttachmentUpload, lblAttachmentErrorMsg, txtPriorAttachmentNote);
        string fileName = String.Format("{0}{1}--{2}--{3}{4}", AttachmentFileName, hdnTime.Value, AttachmentFileNameSuffix, tradingPartnerID, Path.GetExtension(PriorAttachmentUpload.FileName));

        if (documentId <= 0)
        {
            lblAttachmentErrorMsg.Visible = true;
            return;
        }

        string methodName = "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString();
        CreateAndReturnLogInfoThreadNumber(methodName + " fileName: " + fileName + ", originalFileName: " + PriorAttachmentUpload.FileName);

        SaveRecordToPriorAuthAttachment(documentId, fileName, rblClaimType.SelectedValue, ddlPriorAuthDocType, note, PriorAttachmentUpload.FileName);
        txtPriorAttachmentNote.Text = "";
        ddlPriorAuthDocType.SelectedIndex = 0;
        BindAttachmentGrid();
    }

    private void BindAttachmentGrid()
    {
        string logHeader = string.Format("SubmitPriorAuthorization -> GetAttachment Session");
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            DataSet ds = null; DataTable dataTable = null;
            if (Session[ATTACHMENT_RETENTION_DATA] == null)
            {
                ds = PriorAuthHospitalController.SelectPriorAuthAttachment(rblClaimType.SelectedValue, MedicaidId);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Columns.IndexOf("RowState") < 0)
                {
                    DataColumn dcRowState = new DataColumn("RowState", typeof(string));
                    ds.Tables[0].Columns.Add(dcRowState);
                }
                Session[ATTACHMENT_RETENTION_DATA] = ds;
                dataTable = ds.Tables[0];
            }
            else
            {
                ds = (DataSet)Session[ATTACHMENT_RETENTION_DATA];
                dataTable = ds.Tables[0];
            }



            if (dataTable != null && dataTable.Rows.Count > 0)
            {

                if (dataTable.Columns.IndexOf("RowState") < 0)
                {
                    DataColumn dcRowState = new DataColumn("RowState", typeof(string));
                    dataTable.Columns.Add(dcRowState);
                }

                DataTable dtCopy = ds.Tables[0].Copy();
                DataRow[] dtRows = dtCopy.Select("RowState='" + ROWSTATE_DELETE + "'");
                foreach (DataRow dr in dtRows)
                {
                    dtCopy.Rows.Remove(dr);
                }
                //gvAttachment.DataSource = dtCopy;
                //gvAttachment.DataBind();
                //gvAttachment.Visible = true;
            }
            else
            {
                //gvAttachment.DataSource = null;
                //gvAttachment.DataBind();
                //gvAttachment.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at BindAttachmentGrid method", ex);
        }

    }

    private void BindDentalAttachments()
    {
        string logHeader = string.Format("SubmitPriorAuthorization -> GetAttachment Session");
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            DataSet ds = null; DataTable dataTable = null;
            if (Session[ATTACHMENT_RETENTION_DATA] == null)
            {
                ds = PriorAuthHospitalController.SelectPriorAuthAttachment(rblClaimType.SelectedValue, MedicaidId);
                if (ds != null && ds.Tables[0].Columns.IndexOf("RowState") < 0)
                {
                    DataColumn dcRowState = new DataColumn("RowState", typeof(string));
                    ds.Tables[0].Columns.Add(dcRowState);
                }
                Session[ATTACHMENT_RETENTION_DATA] = ds;
                dataTable = ds.Tables[0];
            }
            else
            {
                ds = (DataSet)Session[ATTACHMENT_RETENTION_DATA];
                dataTable = ds.Tables[0];
            }



            if (dataTable != null && dataTable.Rows.Count > 0)
            {

                if (dataTable.Columns.IndexOf("RowState") < 0)
                {
                    DataColumn dcRowState = new DataColumn("RowState", typeof(string));
                    dataTable.Columns.Add(dcRowState);
                }

                DataTable dtCopy = ds.Tables[0].Copy();
                DataRow[] dtRows = dtCopy.Select("RowState='" + ROWSTATE_DELETE + "'");
                foreach (DataRow dr in dtRows)
                {
                    dtCopy.Rows.Remove(dr);
                }
                //gvDentalAttachment.DataSource = dtCopy;
                //gvDentalAttachment.DataBind();
                //gvDentalAttachment.Visible = true;
            }
            else
            {
                //gvDentalAttachment.DataSource = null;
                //gvDentalAttachment.DataBind();
                //gvDentalAttachment.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at BindDentalAttachments method", ex);
        }
    }

    private void SaveRecordToPriorAuthAttachment(int documentId, string fileName, string authType, DropDownList ddlPriorAuthDocType, string note, string originalFileName)
    {
        try
        {
            string methodName = "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString();
            string providerNPI = string.Empty;
            DataSet dsNPI = spa.SelectProviderByGRPMedicaidID(MedicaidNumber);
            DataTable dtMisc = Helper.HasRows(dsNPI) ? dsNPI.Tables[0] : null;
            if (Helper.HasRows(dtMisc))
            {
                DataRow dr = dtMisc.Rows[0];
                providerNPI = Helper.GetString("NPI", dr);
            }

            Dictionary<string, object> parms = new Dictionary<string, object>();
            var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            parms.Add("PRIOR_AUTH_SUB_DOCUMENT_TYPE_ID", Convert.ToString(ddlPriorAuthDocType.SelectedItem.Value));
            parms.Add("PRIOR_AUTH_SUB_TRACKING_NUMBER", txtPatientTrckNum.Text);
            parms.Add("PRIOR_AUTH_SUB_DOCUMENT_ID", Convert.ToString(documentId));
            parms.Add("PRIOR_AUTH_SUB_Note", note);

            var claimType = authType.ToUpper();
            if (claimType == "DENTAL")
                parms.Add("PRIOR_AUTH_SUB_ATTACHMENT_AUTH_TYPE", "1");
            else if (claimType == "PROFESSIONAL")
                parms.Add("PRIOR_AUTH_SUB_ATTACHMENT_AUTH_TYPE", "2");
            else if (claimType == "INSTITUTIONAL")
                parms.Add("PRIOR_AUTH_SUB_ATTACHMENT_AUTH_TYPE", "3");

            parms.Add("PRIOR_AUTH_SUB_DOCUMENT_TYPE_DESC", Convert.ToString(ddlPriorAuthDocType.SelectedItem.Text));
            parms.Add("DOCUMENT_ID", !string.IsNullOrEmpty(Convert.ToString(documentId)) ? documentId : 0);
            parms.Add("PRIOR_AUTH_SUB_ATTACHMENT_STATUS", "1");
            parms.Add("LAST_MODIFIED_USER", id.ToString());
            parms.Add("MedicaidId", Convert.ToString(MedicaidId));
            parms.Add("Created_On_Date_Time", DateTime.Now);
            parms.Add("Created_By_User", id.ToString());
            Random random = new Random();
            parms.Add("PRIOR_AUTH_SUB_ATTACHMENT_ID", random.Next());
            parms.Add("DOCUMENT_NAME", fileName);
            parms.Add("ORIGINAL_DOCUMENT_NAME", originalFileName);

            CreateAndReturnLogInfoThreadNumber(methodName + "fileName: " + fileName + ", and  originalFileName: " + originalFileName);

            DataSet ds = (DataSet)Session[ATTACHMENT_RETENTION_DATA];

            if (ds != null && ds.Tables[0].Columns.IndexOf("PRIOR_AUTH_SUB_DOCUMENT_TYPE_ID") < 0)
            {
                DataColumn dcRowState = new DataColumn("PRIOR_AUTH_SUB_DOCUMENT_TYPE_ID", typeof(int));
                ds.Tables[0].Columns.Add(dcRowState);
            }

            if (ds != null && ds.Tables[0].Columns.IndexOf("PRIOR_AUTH_SUB_DOCUMENT_ID") < 0)
            {
                DataColumn dcRowState = new DataColumn("PRIOR_AUTH_SUB_DOCUMENT_ID", typeof(int));
                ds.Tables[0].Columns.Add(dcRowState);
            }

            DataRow dataRowForAdd = ds.Tables[0].NewRow();

            foreach (var key in parms)
            {
                if (key.Value == null)
                    dataRowForAdd[key.Key] = DBNull.Value;

                dataRowForAdd[key.Key] = key.Value;
            }
            dataRowForAdd["RowState"] = ROWSTATE_ADD;

            ds.Tables[0].Rows.Add(dataRowForAdd);
            Session[ATTACHMENT_RETENTION_DATA] = ds;
        }
        catch (Exception ex)
        {
            throw CoreException.ThrowException(ex);
        }
    }

    private int UploadPriorAuthAttachment(Label lblAttStatusMsg, MMSWebControls.EncryptedFileUpload PriorAttUpload, Label lblAttErrMsg, TextBox txtAttNote)
    {
        lblAttStatusMsg.Text = string.Empty;
        lblAttErrMsg.Text = string.Empty;
        int docID = 0;
        int padocID = 0;

        if (PriorAttUpload.PostedFile == null)
        {
            lblAttErrMsg.Text = "Unable to find posted file.";

        }
        if (string.IsNullOrWhiteSpace(PriorAttUpload.PostedFile.FileName))
        {
            lblAttErrMsg.Text = "Select a file for upload.";
        }
        if (string.IsNullOrWhiteSpace(_DestinationPath))
        {
            lblAttErrMsg.Text = "ERROR - DestinationPath not defined. Contact system administrator.";

        }
        if (string.IsNullOrWhiteSpace(_ValidFileExtensions))
        {
            lblAttErrMsg.Text = "ERROR - ValidFileExtensions must have a value. Contact system administrator.";

        }
        string errMsg = string.Empty;
        if (!IsValidExtension(PriorAttUpload, out errMsg))
        {
            lblAttErrMsg.Text = errMsg;
        }

        if (PriorAttUpload.PostedFile.ContentLength < 1)
        {
            lblAttErrMsg.Text = "File cannot be 0Kb.";
        }

        if (PriorAttUpload.PostedFile.ContentLength > (MaxFileMegaBytes * (1000 * 1024)))
        {
            lblAttErrMsg.Text = "File cannot be more than " + String.Format("{0:0,0}", (MaxFileMegaBytes * (1000 * 1024))) + " bytes (" +
                String.Format("{0:f}", MaxFileMegaBytes) + " MB) in size.";

        }

        if (!IsSpecialCharacter(PriorAttUpload.FileName))
        {
            lblAttErrMsg.Text = "The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): " + Helper.HtmlEncode(PriorAttUpload.FileName) + " Please remove the Special Character from the file Name before upload. ";

        }
        if (!string.IsNullOrEmpty(lblAttErrMsg.Text))
        {
            return docID;
        }

        try
        {
            string fileName = Helper.CleanFilePath(PriorAttUpload.FileName);
            string tradingPartnerID = GetTradingPartnerID();
            string newFileName = String.Format("{0}{1}--{2}--{3}{4}", AttachmentFileName, hdnTime.Value, AttachmentFileNameSuffix, tradingPartnerID, Path.GetExtension(PriorAttachmentUpload.FileName));
            if (rblClaimType.SelectedValue == "dental")
            {
                fileName = Helper.CleanFilePath(priorDentalAttachmentUpload.FileName);
                newFileName = String.Format("{0}{1}--{2}--{3}{4}", AttachmentFileName, hdnTime.Value, AttachmentFileNameSuffix, tradingPartnerID, Path.GetExtension(priorDentalAttachmentUpload.FileName));
            }
            byte[] fileBytes = rblClaimType.SelectedValue == "dental" ? priorDentalAttachmentUpload.FileBytes : PriorAttUpload.FileBytes;
            string fileDescp = txtAttNote.Text.Trim();

            //docID = SaveUploadedFile(fileName, newFileName, fileDescp);
            padocID = SavePAUploadedFile(fileName, newFileName);//OHPNM-10741

            //ContinueWaitingUntilScanCompletes(newFileName);
            /*if (docID != 0)

            {
                SendToCMS(docID, fileBytes, newFileName);
                lblAttStatusMsg.Text = "File Uploaded: " + Helper.HtmlEncode(newFileName);
            }*/
        }
        catch (Exception ex)
        {
            lblAttErrMsg.Text = "No File uploaded. Error: " + Helper.HtmlEncode(ex.Message);
        }
        return padocID;
    }

    private string GetTradingPartnerID()
    {
        var tradingPartnerID = txtDestinationpayerID.Value.ToString();
        tradingPartnerID = tradingPartnerID != null ? tradingPartnerID : string.Empty;

        //Return the PartnerID
        return tradingPartnerID;
    }

    private void SendToCMS(int docID, byte[] fileBytes, string fileName)
    {
        OnBaseInterface onBaseInterface = new OnBaseInterface();
        onBaseInterface.SubmitFile(docID, fileBytes, fileName);
    }

    private void SendControlFileToAWS(string newFileName, string documentId, string tradingPartnerID, string documentType, string timeStamp)
    {
        if (!string.IsNullOrEmpty(newFileName))
        {
            PAAttachments_ControlFile controlFile = PAPopulateAttachmentControlFile(newFileName, documentId, tradingPartnerID, documentType, timeStamp);
            XmlSerializer x = new XmlSerializer(typeof(PAAttachments_ControlFile));

            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);

            x.Serialize(writer, controlFile, emptyNs);
            string xml = "<?xml version=\"1.0\" encoding=\"UTF - 8\"?>" + Environment.NewLine;
            xml = xml + stream2.ToString();
            Guid g = Guid.NewGuid();

            string fileName = Path.GetFileNameWithoutExtension(newFileName).Replace("--AT--", "--CF--") + ".xml";
            byte[] title = new UTF8Encoding(true).GetBytes(xml);
            MemoryStream stream = new MemoryStream(title);
            ProcessAttachments attachments = new ProcessAttachments();
            attachments.sendXMLDocument(stream, fileName);
            attachments.UpdateFileStatus(string.IsNullOrEmpty(documentId) ? 0 : Convert.ToInt32(documentId));
        }
        else
        {
            string methodName = "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString();

            CreateAndReturnLogInfoThreadNumber(methodName + " File was not uploaded as the newFileName was empty.");
        }
    }

    private PAAttachments_ControlFile PAPopulateAttachmentControlFile(string newFileName, string documentID, string tradingPartnerID, string documentType, string timeStamp)
    {
        PAAttachments_ControlFile controlFile = new PAAttachments_ControlFile();

        //Pass in Dynamic values into the PA Control File.
        controlFile.EDITransaction_Type = "PA";
        controlFile.PayerRequested = "No";
        controlFile.Member_ID = txtMedicaidBillingNumber.Text.ToString();
        controlFile.PA_number = null;
        controlFile.Attachment_ControlNumber = documentID;
        controlFile.Provider_ID = this.WorkflowPage.MedicaidID;
        controlFile.Provider_NPI = this.WorkflowPage.NPI;
        controlFile.SenderID = "MMISODJFS";
        controlFile.ReceiverID = tradingPartnerID;
        controlFile.DocumentName = newFileName;
        controlFile.DocumentType = documentType;
        controlFile.UUID = newFileName.Split(new string[] { "--" }, StringSplitOptions.None)[1].ToString();
        controlFile.Timestamp = Convert.ToDateTime(timeStamp).ToString("yyyy-MM-ddTHH:mm:ssZ");

        //Return the controlFile properties
        return controlFile;
    }

    private int SaveUploadedFile(string fileName, string newFileName, string fileDescription)
    {
        int docID = 0;
        try
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Guid userID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            Guid createdBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            DateTime lastModifiedDate = DateTime.Now;
            DateTime createdDate = DateTime.Now;
            int status = CON.DelegateDocumentUploadStatus.Processing;
            docID = psc.InsertDelegateDocument(userID, fileName, newFileName, fileDescription, status, lastModifiedUser, lastModifiedDate, createdBy, createdDate);
            return docID;
        }
        catch (Exception ex)
        {
            lblAttachmentStatusMsg.Text = "File entries not saved. Error: " + Helper.HtmlEncode(ex.Message);
            return docID;
        }
    }

    //OHPNM-10741
    private string generateDocumentNumber()
    {
        string providerNPI = string.Empty;

        DataSet ds = spa.SelectProviderByGRPMedicaidID(MedicaidNumber);
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
        if (Helper.HasRows(dtMisc))
        {
            DataRow dr = dtMisc.Rows[0];
            providerNPI = Helper.GetString("NPI", dr);
        }
        return providerNPI + DateTime.Now.ToString(" yyyy-mm-dd hh:mm:ss");
    }
    //OHPNM-10741
    private int SavePAUploadedFile(string fileName, string newFileName)
    {
        int docID = 0;
        try
        {
            string tradingPartnerID = GetTradingPartnerID();
            Guid uuid = Guid.NewGuid();
            int transactiontypeId = CON.BillingServicetypeId.PA;
            string payerRequested = "Yes";
            string paNumber = txtPANumber2.Text;
            string memberId = txtMedicaidBillingNumber.Text;
            string providerid = this.WorkflowPage.MedicaidID;
            string providerNPI = this.WorkflowPage.NPI;
            string senderid = SENDER_ID;
            string claim_number = null;
            string originalDocName = null;
            bool toSend = false;
            string documentname = newFileName;
            var last_modified_date_time = DateTime.Now;
            string receiverId = !string.IsNullOrWhiteSpace(ddlSubCapitaPayerIDs.SelectedItem.Text) && ddlSubCapitaPayerIDs.SelectedItem.Text.Length > 0 ? ddlSubCapitaPayerIDs.SelectedItem.Value : string.Empty;
            string OutboundIdentifier = generateDocumentNumber();
            int claimtypeid = 0;
            int documenttypeid = 0;
            switch (rblClaimType.SelectedValue.ToUpper())
            {
                case "DENTAL":
                    claimtypeid = 1;
                    documenttypeid = Convert.ToInt32(ddlPriorDentalAuthDocType.SelectedValue);
                    originalDocName = priorDentalAttachmentUpload.FileName;

                    break;
                case "PROFESSIONAL":
                    claimtypeid = 2;
                    documenttypeid = Convert.ToInt32(ddlPriorAuthDocType.SelectedValue);
                    originalDocName = PriorAttachmentUpload.FileName;

                    break;
                case "INSTITUTIONAL":
                    claimtypeid = 3;
                    documenttypeid = Convert.ToInt32(ddlPriorAuthDocType.SelectedValue);
                    originalDocName = PriorAttachmentUpload.FileName;

                    break;
            }
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            docID = psc.InsertPAOutBoundDocumentUploads(transactiontypeId, payerRequested, memberId, claimtypeid, claim_number, paNumber,
                 providerid, providerNPI, senderid, receiverId, documenttypeid,
                 documentname, uuid, toSend, new Guid(CON.appAdminUserId), OutboundIdentifier, originalDocName);
            return docID;
        }
        catch (Exception ex)
        {
            lblAttachmentStatusMsg.Text = "File entries not saved. Error: " + Helper.HtmlEncode(ex.Message);
            return docID;
        }
    }

    public int MaxFileMegaBytes
    {
        get { return (ViewState["_MaxFileMegaBytes"] == null || Convert.ToInt32(ViewState["_MaxFileMegaBytes"]) == 0) ? 10 : Convert.ToInt32(ViewState["_MaxFileMegaBytes"]); }
        set
        {
            ViewState["_MaxFileMegaBytes"] = value;
        }
    }
    private bool IsValidExtension(MMSWebControls.EncryptedFileUpload PriorAttUpload, out string errMsg)
    {
        bool rtn = false;
        errMsg = string.Empty;

        string fileName = Helper.CleanFilePath(PriorAttUpload.PostedFile.FileName);

        // extarct and store the file extension into another variable
        string fileExtension = System.IO.Path.GetExtension(fileName).Replace(".", string.Empty).ToLower();

        // string type array having list of allowed file type extensions
        string[] validFileExtensions = _ValidFileExtensions.Split(',');
        // loop over the array of valid file extensions to compare them with uploaded file
        foreach (string extension in validFileExtensions)
        {
            if (fileExtension == extension)
            {
                rtn = true;
                break;
            }
        }

        // display the message based on the flag value
        if (!rtn)
        {
            errMsg = "Files with extension <b>\"" + fileExtension + "\"</b> are not allowed.<br />";
            errMsg += "You can upload files with the following extensions only:";
            foreach (string str in validFileExtensions)
            {
                errMsg += " ." + str + ",";
            }
            errMsg = errMsg.Substring(0, errMsg.Length - 1);            // Remove "," at end
        }
        return rtn;
    }
    private bool IsSpecialCharacter(string strFileName)
    {
        string pattern = Helper.GetAppSettingFromDB("RegexPatternForSpecialCharacter", string.Empty);
        if (string.IsNullOrWhiteSpace(pattern)) return true;                 // TRUE if the configuration setting does not exist
        Regex objAlphaPattern = new Regex(pattern);
        return objAlphaPattern.IsMatch(strFileName);
    }
    private string RenameFileMethod(string dir, string input)
    {
        string rtn = input;
        int idx = 0;
        while (System.IO.File.Exists(dir + rtn))
        {
            idx += 1;
            int pos = input.LastIndexOf(".");
            if (pos == -1) rtn = input + "_" + idx.ToString();
            else rtn = input.Substring(0, pos) + "_" + idx.ToString() + input.Substring(pos);
        }
        return rtn;
    }
    protected void gvAttachment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void gvAttachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            //GridViewRow row = (GridViewRow)gvAttachment.Rows[e.RowIndex];
            //Label lbldeleteid = (Label)row.FindControl("lblAttachmentId");
            //BindAttachmentGrid();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }
    protected void gvAttachment_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public string DestinationPath
    {
        get { return ViewState["_DestinationPath"] == null ? null : Helper.CleanFilePath(ViewState["_DestinationPath"].ToString(), true); }
        set
        {
            ViewState["_DestinationPath"] = value;
        }
    }
    public string ValidFileExtensions
    {
        get { return ViewState["_ValidFileExtensions"] == null ? null : ViewState["_ValidFileExtensions"].ToString(); }
        set
        {
            ViewState["_ValidFileExtensions"] = value;
        }
    }
    public void LoadClaimData(string medicaidId, string trackingNo)
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            var ds = _spa.SearchPriorTrackingSubmitPA(medicaidId, trackingNo);
            Guid linkSections = new Guid();
            if (ds != null)
            {
                var dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count == 1)
                {
                    linkSections = new Guid(Convert.ToString(dt.Rows[0]["LINK_SECTIONS"]));
                    SaveCodeLNK = linkSections.ToString();
                    tempUniqueId.Value = SaveCodeLNK;
                    var claimType = Convert.ToInt32(dt.Rows[0]["PriorAuthType"]);
                    if (claimType == 1)
                    {
                        rblClaimType.SelectedIndex = 0;  //dental
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
                        parms.Add("LINK_SECTIONS", SaveCodeLNK);
                        ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                    }
                    else if (claimType == 2)
                    {
                        rblClaimType.SelectedIndex = 1; //Professional
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
                        parms.Add("LINK_SECTIONS", SaveCodeLNK);
                        ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                    }
                    else if (claimType == 3)
                    {
                        rblClaimType.SelectedIndex = 2; //Institutional
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
                        parms.Add("LINK_SECTIONS", SaveCodeLNK);
                        ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                    }

                    SelectClaimType();
                    //Clear prior error
                    lblpriorautherror.Text = "";
                    hdnTrackingNo.Value = trackingNo;
                    txtPatientTrckNum.Enabled = false;

                    // load all field data
                    txtMedicaidBillingNumber.Text = Convert.ToString(dt.Rows[0]["Billing_Number"]);
                    txtPANumber2.Text = Convert.ToString(dt.Rows[0]["PA_NUMBER"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["ICD_ProcedureCode"])))
                        DropDownList1.SelectedValue = Convert.ToString(dt.Rows[0]["ICD_ProcedureCode"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["ProcedureCode"])))
                        ddlServiceTypeCode.SelectedValue = Convert.ToString(dt.Rows[0]["ProcedureCode"]);

                    txtLnDiagnosisCode.Text = Convert.ToString(dt.Rows[0]["DiagnosisCode"]);
                    txtServicecode.Text = Convert.ToString(dt.Rows[0]["RevenueCode"]);
                    txtExpDate2.Text = Convert.ToString(dt.Rows[0]["PA_Expiration_Date"]);
                    //Load Recipient Info here
                    txtLastName2.Text = Convert.ToString(dt.Rows[0]["LAST_NAME"]);
                    txtfrstmi2.Text = Convert.ToString(dt.Rows[0]["FIRST_NAME"]);
                    txtMiddleName.Text = Convert.ToString(dt.Rows[0]["MIDDLE_NAME"]);
                    txtBirthDate.Text = Convert.ToString(dt.Rows[0]["BIRTH_DATE"]);
                    txtAddress1.Text = Convert.ToString(dt.Rows[0]["ADDRESS_LINE1"]);
                    lblAddress2.Text = Convert.ToString(dt.Rows[0]["ADDRESS_LINE2"]);
                    txtCity.Text = Convert.ToString(dt.Rows[0]["CITY"]);
                    txtState.Text = Convert.ToString(dt.Rows[0]["STATE"]);
                    txtZipCode.Text = Convert.ToString(dt.Rows[0]["ZIP_CODE"]);
                    txtGender2.Text = Convert.ToString(dt.Rows[0]["GENDER"]);
                    txtPatientTrckNum.Text = Convert.ToString(dt.Rows[0]["TRACKING_NUMBER"]);
                    //Load Contact Info here
                    txtContactName.Text = Convert.ToString(dt.Rows[0]["CONTACT_FIRST_NAME"]);
                    txtContactLastName.Text = Convert.ToString(dt.Rows[0]["CONTACT_LAST_NAME"]);
                    txtContactNumber.Text = Convert.ToString(dt.Rows[0]["CONTACT_NUMBER"]);
                    if (Convert.ToString(dt.Rows[0]["CONTACT_EXT"]) == "0")
                        txtExt.Text = "";
                    else
                        txtExt.Text = Convert.ToString(dt.Rows[0]["CONTACT_EXT"]);

                    txtProviderNotes.Text = Convert.ToString(dt.Rows[0]["NOTES"]);
                    hdnProvNoteText.Value = Convert.ToString(dt.Rows[0]["NOTES"]);
                    hdnProvNoteId.Value = Convert.ToString(dt.Rows[0]["ID"]);
                    //Dental/Professional Service Info here
                    if (claimType == 1 || claimType == 2)
                    {
                        txtpalceofservice.Text = Convert.ToString(dt.Rows[0]["SERVICE_PLACE"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["SERVICE_PLACE"])))
                        {
                            this.txtpalceofservice_TextChanged(txtpalceofservice, new EventArgs());
                        }

                        txtAccDtService.Text = dt.Rows[0]["ACCIDENT_DATE"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["ACCIDENT_DATE"]).ToString("MM/dd/yyyy") : "";
                        TextBox3.Text = dt.Rows[0]["PATIENT_EVENT_DATE"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["PATIENT_EVENT_DATE"]).ToString("MM/dd/yyyy") : "";
                        txtProfOnsetIllness.Text = dt.Rows[0]["ILLNESS_DATE"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["ILLNESS_DATE"]).ToString("MM/dd/yyyy") : "";
                        txtMenDtInst.Text = dt.Rows[0]["LAST_MENSTRUAL_PERIOD_DATE"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["LAST_MENSTRUAL_PERIOD_DATE"]).ToString("MM/dd/yyyy") : "";
                        txtEstDOB.Text = dt.Rows[0]["EST_DOB"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["EST_DOB"]).ToString("MM/dd/yyyy") : "";
                        ddlLvlServiceInst.SelectedValue = Convert.ToString(dt.Rows[0]["SERVICE_LEVEL"]);
                        ddlDelayedInst.SelectedValue = Convert.ToString(dt.Rows[0]["DELAY_REASON"]);
                        if (Convert.ToString(dt.Rows[0]["ASSOCIATED_PA_NO"]) == "0")
                            txtPANumInst.Text = "";
                        else
                            txtPANumInst.Text = Convert.ToString(dt.Rows[0]["ASSOCIATED_PA_NO"]);

                        linkSections = new Guid(Convert.ToString(dt.Rows[0]["LINK_SECTIONS"]));

                        FetchSavedAttachments(linkSections, claimType);

                        DataSet dsProf = PriorAuthHospitalController.GetPriorServiceDetailsBySectionID(claimType, linkSections);
                    }
                    //Institutional Service info here
                    if (claimType == 3)
                    {
                        txtfacilityType.Text = Convert.ToString(dt.Rows[0]["SERVICE_PLACE"]);
                        txtFAdmissionDate.Text = dt.Rows[0]["ADMISSION_DATE"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["ADMISSION_DATE"]).ToString("MM/dd/yyyy") : "";
                        //ddlAdminType.AutoPostBack = false;
                        if (ddlAdminType.Items.FindByValue(Convert.ToString(dt.Rows[0]["ADMISSION_TYPE"])) != null)
                            ddlAdminType.SelectedValue = Convert.ToString(dt.Rows[0]["ADMISSION_TYPE"]);
                        GetAdmissionSources();
                        if (ddlAdminSrc.Items.FindByValue(Convert.ToString(dt.Rows[0]["ADMISSION_SOURCE"])) != null)
                            ddlAdminSrc.SelectedValue = Convert.ToString(dt.Rows[0]["ADMISSION_SOURCE"]);
                        txtFDischargeDate.Text = dt.Rows[0]["DISCHARGE_DATE"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["DISCHARGE_DATE"]).ToString("MM/dd/yyyy") : "";

                        GetDischargeStatus();

                        if (ddlDischargeStatus.Items.FindByValue(Convert.ToString(dt.Rows[0]["DISCHARGE_STATUS"])) != null)
                            ddlDischargeStatus.SelectedValue = Convert.ToString(dt.Rows[0]["DISCHARGE_STATUS"]);
                        txtLstMensPeriod.Text = dt.Rows[0]["LAST_MENSTRUAL_PERIOD_DATE"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["LAST_MENSTRUAL_PERIOD_DATE"]).ToString("MM/dd/yyyy") : "";
                        txtEstBirthDate.Text = dt.Rows[0]["EST_DOB"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["EST_DOB"]).ToString("MM/dd/yyyy") : "";
                        txtOnsetIllness.Text = dt.Rows[0]["ILLNESS_DATE"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["ILLNESS_DATE"]).ToString("MM/dd/yyyy") : "";
                        txtAccidentDate.Text = dt.Rows[0]["ACCIDENT_DATE"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["ACCIDENT_DATE"]).ToString("MM/dd/yyyy") : "";
                        if (ddlLevelService.Items.FindByValue(Convert.ToString(dt.Rows[0]["SERVICE_LEVEL"])) != null)
                            ddlLevelService.SelectedValue = Convert.ToString(dt.Rows[0]["SERVICE_LEVEL"]);
                        if (ddlDlyReason.Items.FindByValue(Convert.ToString(dt.Rows[0]["DELAY_REASON"])) != null)
                            ddlDlyReason.SelectedValue = Convert.ToString(dt.Rows[0]["DELAY_REASON"]);
                        if (Convert.ToString(dt.Rows[0]["ASSOCIATED_PA_NO"]) == "0")
                            txtPaNum.Text = "";
                        else
                            txtPaNum.Text = Convert.ToString(dt.Rows[0]["ASSOCIATED_PA_NO"]);
                        linkSections = new Guid(Convert.ToString(dt.Rows[0]["LINK_SECTIONS"]));

                        DataSet dsInst = PriorAuthHospitalController.GetPriorServiceDetailsBySectionID(claimType, linkSections);

                        FetchSavedAttachments(linkSections, claimType);
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["PAYER_ID"])))
                    {
                        GetDestinationPayer();
                        ddlAuthorization.SelectedValue = Convert.ToString(dt.Rows[0]["PAYER_ID"]);
                        if (Session["DestinationPayer"] != null)
                        {
                            DataTable destpayer = (DataTable)Session["DestinationPayer"];
                            if (destpayer != null && destpayer.Rows.Count > 0)
                            {
                                var dataRow = destpayer.AsEnumerable().Where(x => x.Field<int>("PRIOR_AUTH_DESTINATION_PAYER_ID") == Convert.ToInt32(ddlAuthorization.SelectedValue)).FirstOrDefault();
                                if (dataRow != null)
                                {
                                    string MCEID = Convert.ToString(dataRow["MCE_ID"]);
                                    txtDestinationpayerID.Value = MCEID;
                                    string destPayerID = Convert.ToString(dataRow["PRIOR_AUTH_DESTINATION_PAYER_ID"]);
                                    LoadDestinationPayerIDs(destPayerID);
                                }
                            }
                        }
                    }

                    txtEffDate2.Text = Convert.ToString("");
                    var strAssignment = Convert.ToString(dt.Rows[0]["Assignment_Type"]);
                    if (!string.IsNullOrEmpty(strAssignment) && strAssignment.Length == 1)
                        strAssignment = "0" + strAssignment;
                    if (!string.IsNullOrEmpty(strAssignment) && ddlAssignment.Items.FindByValue(strAssignment) != null)
                        ddlAssignment.SelectedValue = strAssignment;//Convert.ToString(dt.Rows[0]["PRIOR_AUTH_Assignment_Type_MMIS"]);


                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["PRIOR_AUTH_SERVICE_TYPE_ID"])) && !Convert.ToString(dt.Rows[0]["PRIOR_AUTH_SERVICE_TYPE_ID"]).Equals("0"))
                    {
                        GetServiceCodeType();
                        ddlServiceType.SelectedValue = Convert.ToString(dt.Rows[0]["PRIOR_AUTH_SERVICE_TYPE_ID"]);
                    }

                    //Load Service Provider info here
                    txtSPNPI.Text = Convert.ToString(dt.Rows[0]["SVC_PROV_NPI"]);
                    txtMedicaidID.Text = Convert.ToString(dt.Rows[0]["SVC_PROV_MEDICAID_ID"]);
                    lblSvcProviderFName.Text = Convert.ToString(dt.Rows[0]["SVC_PROV_FIRST_NAME"]);
                    lblSvcProviderLName.Text = Convert.ToString(dt.Rows[0]["SVC_PROV_LAST_NAME"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["SVC_PROV_NPI"])))
                    {
                        this.txtProviderNPI_TextChangedNPI(txtSPNPI, new EventArgs());
                    }
                    //Load Ordering Provider info here
                    txtorderingprovidernpi.Text = Convert.ToString(dt.Rows[0]["ORD_PROV_NPI"]);
                    txtOMID.Text = Convert.ToString(dt.Rows[0]["ORD_PROV_MEDICAID_ID"]);
                    lblOrdProviderFName.Text = Convert.ToString(dt.Rows[0]["ORD_PROV_FIRST_NAME"]);
                    lblOrdProviderLName.Text = Convert.ToString(dt.Rows[0]["ORD_PROV_LAST_NAME"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["ORD_PROV_NPI"])))
                    {
                        this.txtorderingprovidernpi_TextChangedNPI(txtorderingprovidernpi, new EventArgs());
                    }

                    linkSections = new Guid(Convert.ToString(dt.Rows[0]["LINK_SECTIONS"]));

                    txtstatus2.Text = "Submission Pending";
                    btnSave.Visible = true;
                    btnSubmit.Visible = true;

                    //Set Malicious docs
                    SetClaimsMaliciousDocs(txtMedicaidBillingNumber.Text, claimType.ToString(), this.WorkflowPage.MedicaidID);
                }
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void FetchSavedAttachments(Guid linkSections, int claimType)
    {
        hdnPriorAuthAttachments.Value = "";

        DataSet dsAttach = PriorAuthHospitalController.GetPriorAttachmentsBySectionID(claimType, linkSections);

        if (dsAttach != null && dsAttach.Tables.Count > 0)
        {
            DataTable attachmentTable = dsAttach.Tables[0];

            if (attachmentTable.Rows.Count > 0)
            {
                List<PriorAuthAttachment> priorAuthAttachmentsDental = ConvertDataTableToList(attachmentTable);
                hdnPriorAuthAttachments.Value = JsonConvert.SerializeObject(priorAuthAttachmentsDental);
            }
        }
    }

    private void SelectClaimType()
    {
        if (rblClaimType.SelectedIndex != -1 && !string.IsNullOrEmpty(rblClaimType.SelectedItem.Value))
        {
            GetAssignments();
            rblClaimType.Enabled = false;
            divSubmitPriorAuth.Visible = true;
            if (rblClaimType.SelectedItem.Value == "Institutional")
            {
                InstitService.Style.Remove("display");
                ProfService.Attributes.CssStyle.Add("display", "none");
                cpeAttachment.Collapsed = false;
                cpeAttachment.ClientState = "false";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "paneldisplay", "CollapseExpandAttachment();", true);

            }
            else if (rblClaimType.SelectedItem.Value == "dental"
                || rblClaimType.SelectedItem.Value == "Professional")
            {
                InstitService.Attributes.CssStyle.Add("display", "none");
                ProfService.Style.Remove("display");
                cpeAttachment.Collapsed = true;
                cpeAttachment.ClientState = "true";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "paneldisplay", "CollapseExpandAttachment();", true);
            }
            else
            {
                cpeAttachment.Collapsed = true;
                cpeAttachment.ClientState = "true";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "paneldisplay", "CollapseExpandAttachment();", true);
            }
        }
        EnablePanelsBasedOnPriorAuth();
        BindDentalAttachments();
        GetAttachment();
        GetDiagnoisServiceDetails();
        ClearRecipientInfo();
        BindAttachmentGrid();
    }

    protected void rblClaimType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Text = "";
            if (rblClaimType.SelectedIndex != -1 && !string.IsNullOrEmpty(rblClaimType.SelectedItem.Value))
            {
                SaveCodeLNK = Guid.NewGuid().ToString();
            }
            SelectClaimType();
            if (!txtPatientTrckNum.Enabled)
            {
                hdnTrackingNo.Value = "";
                txtPatientTrckNum.Enabled = true;
                lblpriorautherror.Text = "";
                //Clear service/ordering provider NPI details
                txtSPNPI.Text = "";
                txtMedicaidID.Text = "";
                lblSvcProviderFName.Text = "";
                lblSvcProviderLName.Text = "";
                //Ordering Provider info here
                txtorderingprovidernpi.Text = "";
                txtOMID.Text = "";
                lblOrdProviderFName.Text = "";
                lblOrdProviderLName.Text = "";
            }
            if (rblClaimType.SelectedIndex != -1 && !string.IsNullOrEmpty(rblClaimType.SelectedItem.Value))
            {
                SaveCodeLNK = Guid.NewGuid().ToString();
                tempUniqueId.Value = SaveCodeLNK;
                if (rblClaimType.SelectedItem.Value == "dental")
                {
                    AttchmentMandatory.Visible = true;
                    GetDentalServiceDetails();
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
                    parms.Add("LINK_SECTIONS", SaveCodeLNK);
                    var ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                    var dataTable = ds.Tables[0];
                    if (dataTable.Rows.Count == 0)
                    {
                        // lblDentalservDetailsNoData.Visible = true;
                    }
                    btnDentalServiceDetailAdd1.Visible = true;
                }
                if (rblClaimType.SelectedItem.Value == "Professional")
                {
                    AttchmentMandatory.Visible = true;
                    GetProfessionalServiceDetails();
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
                    parms.Add("LINK_SECTIONS", SaveCodeLNK);
                    var ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                    var dataTable = ds.Tables[0];
                    if (dataTable.Rows.Count == 0)
                    {
                        // lblprofNoDetailsFound.Visible = true;
                    }
                    btnProfessionalServiceDetailAdd.Visible = true;
                }
                if (rblClaimType.SelectedItem.Value == "Institutional")
                {
                    AttchmentMandatory.Visible = true;
                    GetServiceDetails();
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
                    parms.Add("LINK_SECTIONS", SaveCodeLNK);
                    var ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                    var dataTable = ds.Tables[0];
                    if (dataTable.Rows.Count == 0)
                    {
                        // lblservDetailsNoData.Visible = true;
                    }
                    btnServiceDetailAdd1.Visible = true;
                }
            }

            if (txtstatus2.Text == "")
            {
                txtstatus2.Text = "Submission Pending";
            }

            pnlSepMaliciousAttachmentsInfo.Visible = true;
            pnlMaliciousAttachments.Visible = true;

            enableButtons();

        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void SetClaimsMaliciousDocs(string claimId, string claimType, string medicaidId)
    {
        PAMaliciousAttachments.ClaimId = claimId;
        PAMaliciousAttachments.ClaimType = claimType;
        PAMaliciousAttachments.ClaimTypeCode = "PA";
        PAMaliciousAttachments.MedicaidId = medicaidId;
        if (!string.IsNullOrEmpty(claimId) && !string.IsNullOrEmpty(medicaidId))
            PAMaliciousAttachments.RefreshGrid();
        else
            PAMaliciousAttachments.ClearGrid();
    }

    protected void PriorAuthType_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetAssignments();
    }
    protected void txtContactName_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtContactName.Text))
        {
            if (valerrormess.Visible)
            {
                if (!ReValidateData())
                {
                    return;
                }
            }
        }
    }

    protected void txtContactLastName_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtContactNumber.Text))
        {
            if (valerrormess.Visible)
            {
                if (!ReValidateData())
                {
                    return;
                }
            }
        }
    }

    protected void txtContactNumber_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtContactNumber.Text))
        {
            if (valerrormess.Visible)
            {
                if (!ReValidateData())
                {
                    return;
                }
            }
        }
    }
    protected void txtfacilityType_TextChanged(object sender, EventArgs e)
    {
        hdnModified.Value = "true";
        lblfacilityTypeError.Text = string.Empty;
        DataTable dt;
        if (!string.IsNullOrEmpty(txtfacilityType.Text.Trim()))
        {
            if (txtfacilityType.Text.Contains("-"))
            {
                string[] FacilityType = txtfacilityType.Text.Split('-');
                dt = GetFacilityTypeData(FacilityType[0].Trim(), FacilityType[1].Trim(), false);
                txtFacilityTypeCode.Text = FacilityType[0].Trim();
                txtFacilityTypeDescription.Text = FacilityType[1].Trim();
            }
            else
            {
                var facilityType = new String(txtfacilityType.Text.ToString().Where(Char.IsDigit).ToArray());
                dt = GetFacilityTypeData(facilityType, "", false);
                txtFacilityTypeCode.Text = txtfacilityType.Text.Trim();
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "modelBox", "$('#FacilityTypeSearchModal').modal('show');", true);
                    //this.gvPriorAuthFacilitySearchPage.DataSource = dt;
                    //this.gvPriorAuthFacilitySearchPage.DataBind();
                    //this.gvPriorAuthFacilitySearchPage.Visible = true;
                    //this.gvPriorAuthFacilitySearchPage.CurrentPageIndex = 0;

                }
                else
                {
                    string code = Convert.ToString(dt.Rows[0]["PRIOR_AUTH_FACILITY_TYPE_CODE_MMIS"]);
                    string desc = Convert.ToString(dt.Rows[0]["PRIOR_AUTH_FACILITY_TYPE_CODE_DESC"]);
                    txtfacilityType.Text = code + " - " + desc;
                    lblfacilityTypeError.Text = "";
                    lblfacilityTypeError.Visible = false;
                }
            }
            else
            {
                lblfacilityTypeError.Visible = true;
                lblfacilityTypeError.Text = "Facility type is invalid";
            }
        }
        else
        {
            lblfacilityTypeError.Text = string.Empty;
        }
        if (valerrormess.Visible)
        {
            if (!ReValidateData())
            {
                return;
            }
        }
    }

    protected void facilitySearch_Click(object sender, EventArgs e)
    {
    }

    protected void ddlAdminType_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            GetAdmissionSources();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void GetAdmissionSources()
    {
        ddlAdminSrc.Enabled = true;
        if (ddlAdminSrc.Items.Count > 0)
            ddlAdminSrc.Items.Clear();

        if (ddlAdminType.SelectedItem.Text == "4 - Newborn")
        {
            ddlAdminSrc.Items.Add(new ListItem("", "-1"));
            ddlAdminSrc.Items.Add(new ListItem("5 - Born Inside Hospital", "5"));
            ddlAdminSrc.Items.Add(new ListItem("6 - Born Outside Hospital", "6"));
        }
        else
        {
            ddlAdminSrc.Items.Add(new ListItem("", "-1"));
            ddlAdminSrc.Items.Add(new ListItem("1 - Physician Referral", "1"));
            ddlAdminSrc.Items.Add(new ListItem("2 - Clinic Referral", "2"));
            ddlAdminSrc.Items.Add(new ListItem("3 - HMO Referral", "3"));
            ddlAdminSrc.Items.Add(new ListItem("4 - Transfer from Hospital", "4"));
            ddlAdminSrc.Items.Add(new ListItem("5 - Transfer from SNF", "5"));
            ddlAdminSrc.Items.Add(new ListItem("6 - Transfer from Another Health Care Facility", "6"));
            ddlAdminSrc.Items.Add(new ListItem("7 - Emergency Room", "7"));
            ddlAdminSrc.Items.Add(new ListItem("8 - Court/Law Enforcement", "8"));
            ddlAdminSrc.Items.Add(new ListItem("9 - Information Not Available", "9"));
            ddlAdminSrc.Items.Add(new ListItem("D - Transfer from one Distinct Unit of the Hospital to Another Distinct Unit of the Same Hospital Resulting in a Separate Claim to the Payer", "D"));
            ddlAdminSrc.Items.Add(new ListItem("E - Transfer from Ambulatory Surgery Center", "E"));
            ddlAdminSrc.Items.Add(new ListItem("F - Transfer from a Hospice Facility", "F"));
            ddlAdminSrc.Items.Add(new ListItem("G - Transfer from a Designated Disaster Alternate Care Site", "G"));
        }
    }

    protected void lnkplaceofservicesrch_Click(object sender, EventArgs e)
    {
        //gvPriorAuthFacilitySearchPage.DataSource = null;
        //gvPriorAuthFacilitySearchPage.DataBind();
        txtFacilityTypeCode.Text = "";
        txtFacilityTypeDescription.Text = "";
        //gvPlaceOfSearch.DataSource = null;
        //gvPlaceOfSearch.DataBind();
        txtPlaceOfServiceCode.Text = "";
        txtPlaceOfServiceDesc.Text = "";
        this.lblSrchPlc.Visible = false;
        // gvPlaceOfSearch.Visible = false;
        mpeServiceinfoPlaceofService.Show();
    }
    private void UpdateCommonPanels()
    {
        cpeRecipient.Collapsed = false;
        cpeRecipient.ClientState = "false";
        cpeContact.Collapsed = false;
        cpeContact.ClientState = "false";
        cpeServiceInformation.Collapsed = false;
        cpeServiceInformation.ClientState = "false";
        CPEService.Collapsed = false;
        CPEService.ClientState = "false";
        cpeorderproviderinfo.Collapsed = true;
        cpeorderproviderinfo.ClientState = "true";
        cpeDiagnosis.Collapsed = true;
        cpeDiagnosis.ClientState = "true";
        cpeServiceDetail.Collapsed = true;
        cpeServiceDetail.ClientState = "true";
        Cpeprovidernote.Collapsed = true;
        Cpeprovidernote.ClientState = "true";
        cpePnlReviewerNotesExt.Collapsed = true;
        cpePnlReviewerNotesExt.ClientState = "true";
        cpeOutcomeOfReview.Collapsed = true;
        cpeOutcomeOfReview.ClientState = "true";
    }
    private void EnablePanelsBasedOnPriorAuth()
    {
        UpdateCommonPanels();
        switch (rblClaimType.SelectedValue.ToUpper())
        {
            case "DENTAL":
                upDentalServiceDetail.Visible = true;
                upProfessionalServiceDetail.Visible = false;
                uptPnlServiceDetails.Visible = false;
                pnlDentalServiceDetail.Visible = true;
                pnlsepDentalServiceDetail.Visible = true;
                pnlSepDentalAttachment.Visible = true;
                pnlDentalAttachment.Visible = true;
                pnlSepAttachment.Visible = false;
                pnlAttachment.Visible = false;
                this.cpeDentalServiceDetails.Collapsed = false;
                cpeDentalServiceDetails.ClientState = "false";
                //collpasing or enabling changes
                cpeTrackingNumber.Collapsed = true;
                cpeTrackingNumber.ClientState = "true";
                cpeDentalAttachment.Collapsed = true;
                cpeDentalAttachment.ClientState = "true";
                ddlLevelService.Enabled = true;
                divSubmitPriorAuth.Visible = true;
                break;
            case "PROFESSIONAL":
                upDentalServiceDetail.Visible = false;
                upProfessionalServiceDetail.Visible = true;
                pnlDentalServiceDetail.Visible = false;
                uptPnlServiceDetails.Visible = false;
                pnlSepDentalAttachment.Visible = false;
                pnlDentalAttachment.Visible = false;
                pnlSepAttachment.Visible = true;
                pnlAttachment.Visible = true;
                ddlLevelService.Enabled = true;
                divSubmitPriorAuth.Visible = true;
                cpeProfessionalServiceDetail.Collapsed = false;
                cpeProfessionalServiceDetail.ClientState = "false";

                pnlsepProfessionalServiceDetail.Visible = true;
                pnlProfessionalServiceDetail.Visible = true;
                break;
            case "INSTITUTIONAL":
                upDentalServiceDetail.Visible = false;
                pnlDentalServiceDetail.Visible = false;
                upProfessionalServiceDetail.Visible = false;
                uptPnlServiceDetails.Visible = true;
                pnlSepDentalAttachment.Visible = false;
                pnlDentalAttachment.Visible = false;
                pnlSepAttachment.Visible = true;
                pnlAttachment.Visible = true;

                pnlsepServiceDetail.Visible = true;
                pnlServiceDetail.Visible = true;

                cpeAttachment.Collapsed = false;
                cpeAttachment.ClientState = "false";
                pnlSepTrackingNumber.Visible = false;
                pnlTrackingNumber.Visible = false;
                divSubmitPriorAuth.Visible = true;

                //  pnlline.Visible = false;
                pnlsepline.Visible = false;
                cpeServiceDetail.Collapsed = false;
                cpeServiceDetail.ClientState = "false";

                break;
        }
    }

    protected void btnDentalAttachmentDelete_Command(object sender, CommandEventArgs e)
    {
        try
        {
            string lineNumber = Convert.ToString(e.CommandArgument);

            DataSet dsValues = (DataSet)Session[ATTACHMENT_RETENTION_DATA];

            if (dsValues != null)
            {
                DataTable dtAttachmentDental = dsValues.Tables[0];
                if (dtAttachmentDental.Columns.IndexOf("RowState") < 0)
                {
                    DataColumn dcRowState = new DataColumn("RowState", typeof(string));
                    dtAttachmentDental.Columns.Add(dcRowState);
                }

                DataRow dr = dsValues.Tables[0].Select("PRIOR_AUTH_SUB_ATTACHMENT_ID='" + lineNumber + "'")[0];
                dsValues.Tables[0].Rows.Remove(dr);
                Session[ATTACHMENT_RETENTION_DATA] = dsValues;

                //gvDentalAttachment.DataSource = dsValues;
                //gvDentalAttachment.DataBind();
                //gvDentalAttachment.Visible = true;

                if (dsValues.Tables[0].Rows.Count == 0)
                {
                    //gvDentalAttachment.Visible = false;
                }
            }
            else
            {
                //gvDentalAttachment.DataSource = null;
                //gvDentalAttachment.DataBind();
                //gvDentalAttachment.Visible = false;
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    //protected void gvDentalAttachment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{

    //}

    protected void gvDentalAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DownloadDocument")
        {
            string fileName = e.CommandArgument.ToString();
            ProcessAttachments attachments = new ProcessAttachments();
            attachments.GetAttachment(fileName);
        }
    }

    private void uploadDentalAttachment()
    {
        lblDentalUploadErrMsg.Text = string.Empty;
        lblDentalDocTypeErrMsg.Text = string.Empty;

        //if (gvDentalAttachment.Rows.Count == 10)
        //{
        //    lblDentalAttachmentErrorMsg.Visible = true;
        //    lblDentalAttachmentErrorMsg.Text = "Maximum of 10 attachments can be submitted";
        //    return;
        //}
        if (String.IsNullOrEmpty(txtMedicaidBillingNumber.Text))
        {
            lblDentalAttachmentErrorMsg.Visible = true;
            lblDentalAttachmentErrorMsg.Text = "Medicaid Billing Number is required";
            return;
        }
        if (ddlPriorDentalAuthDocType.SelectedIndex == 0)
        {
            lblDentalDocTypeErrMsg.Visible = true;
            lblDentalDocTypeErrMsg.Text = "Document type is required";
        }
        else
        {
            lblDentalDocTypeErrMsg.Visible = false;
            lblDentalDocTypeErrMsg.Text = "";
        }

        if (lblDentalDocTypeErrMsg.Visible || lblDentalUploadErrMsg.Visible)
        {
            return;
        }

        if (ValidateAttachment("DentalProf"))
        {
            return;
        }

        if (string.IsNullOrEmpty(priorDentalAttachmentUpload.FileName))
        {
            lblDentalDocTypeErrMsg.Visible = true;
            lblDentalDocTypeErrMsg.Text = "Document name is required";
        }
        else
        {
            lblDentalDocTypeErrMsg.Visible = false;
            lblDentalDocTypeErrMsg.Text = "";
        }

        var fileBytes = priorDentalAttachmentUpload.FileBytes;
        string tradingPartnerID = GetTradingPartnerID();
        string fileName = String.Format("{0}{1}--{2}--{3}{4}", AttachmentFileName, hdnTime.Value, AttachmentFileNameSuffix, tradingPartnerID, Path.GetExtension(priorDentalAttachmentUpload.FileName));
        var docType = ddlPriorDentalAuthDocType.SelectedValue;
        var note = txtPriorDentalAttachmentNote.Text.Trim();
        int documentId = UploadPriorAuthAttachment(lblDentalAttachmentStatusMsg, priorDentalAttachmentUpload, lblDentalAttachmentErrorMsg, txtPriorDentalAttachmentNote);

        if (documentId <= 0)
        {
            lblDentalAttachmentErrorMsg.Visible = true;
            return;
        }

        SaveRecordToPriorAuthAttachment(documentId, fileName, rblClaimType.SelectedValue, ddlPriorDentalAuthDocType, note, priorDentalAttachmentUpload.FileName);
        txtPriorDentalAttachmentNote.Text = "";
        ddlPriorDentalAuthDocType.SelectedIndex = 0;
        BindDentalAttachments();
    }

    #region Dental and Professional
    private void GetPriorAttachmentDocumentTypeForDental()
    {
        string logHeader = string.Format("SubmitPriorAuthorization -> GetPriorAttachmentDocumentTypeForDental()");
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            ddlPriorDentalAuthDocType.Items.Clear();
            DataSet dataSet = _spa.GetClaimDocumentTypeByClaimTransactionType(4);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable dt = dataSet.Tables[0];

                ddlPriorDentalAuthDocType.DataSource = dt;
                ddlPriorDentalAuthDocType.DataValueField = "DOCUMENT_TYPE_ID";
                ddlPriorDentalAuthDocType.DataTextField = "DOCUMENT_TYPE_DESC";
                ddlPriorDentalAuthDocType.DataBind();
                ddlPriorDentalAuthDocType.Items.Insert(0, new ListItem("--- select Document Type ---", String.Empty));
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetPriorAttachmentDocumentTypeForDental method", ex);
        }
    }

    private void BindDentalServiceDetailsDropdowns()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            ddDentalProsthsis.Items.Clear();

            ddToothSurface1.Items.Clear();
            ddToothSurface2.Items.Clear();
            ddToothSurface3.Items.Clear();
            ddToothSurface4.Items.Clear();
            ddToothSurface5.Items.Clear();

            ddDentalOralCavity1.Items.Clear();

            if (ddDentalOralCavity2 != null && ddDentalOralCavity2.Visible)
            {
                ddDentalOralCavity2.Items.Clear();
            }

            if (ddDentalOralCavity3 != null && ddDentalOralCavity3.Visible)
            {
                ddDentalOralCavity3.Items.Clear();
            }

            if (ddDentalOralCavity4 != null && ddDentalOralCavity4.Visible)
            {
                ddDentalOralCavity4.Items.Clear();
            }

            if (ddDentalOralCavity5 != null && ddDentalOralCavity4.Visible)
            {
                ddDentalOralCavity5.Items.Clear();
            }

            ddDentalToothNumber.Items.Clear();

            DataSet dsOralCavity = _spa.GetPriorAuthDentalOralCavity();
            DataSet dsProsthesis = _spa.GetPriorAuthDentalProsthesis();
            DataSet dsToothSurface = _spa.GetPriorAuthDentalToothSurface();
            DataSet dsToothNumbers = _spa.GetPriorAuthDentalToothNumber();

            DataTable dtOralCavity = dsOralCavity.Tables[0];
            DataTable dtProsthesis = dsProsthesis.Tables[0];
            DataTable dtToothSurface = dsToothSurface.Tables[0];
            DataTable dtToothNumber = dsToothNumbers.Tables[0];

            ddDentalProsthsis.DataSource = dtProsthesis;
            ddDentalProsthsis.DataValueField = "PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID";
            ddDentalProsthsis.DataTextField = "PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_DESC";
            ddDentalProsthsis.DataBind();
            ddDentalProsthsis.Items.Insert(0, new ListItem("--select--", "0"));

            ddDentalToothNumber.DataSource = dtToothNumber;
            ddDentalToothNumber.DataValueField = "PRIOR_AUTH_TOOTH_NUMBER_ID";
            ddDentalToothNumber.DataTextField = "PRIOR_AUTH_TOOTH_NUMBER_CODE";
            ddDentalToothNumber.DataBind();
            ddDentalToothNumber.Items.Insert(0, new ListItem("--select--", "0"));

            ddDentalOralCavity1.DataSource = dtOralCavity;
            ddDentalOralCavity1.DataValueField = "PRIOR_AUTH_DENTAL_ORAL_CAVITY_MMIS";
            ddDentalOralCavity1.DataTextField = "PRIOR_AUTH_DENTAL_ORAL_CAVITY_MMIS";
            ddDentalOralCavity1.DataBind();
            ddDentalOralCavity1.Items.Insert(0, new ListItem("--select--", "0"));

            if (ddDentalOralCavity2 != null && ddDentalOralCavity2.Visible)
            {
                ddDentalOralCavity2.DataSource = dtOralCavity;
                ddDentalOralCavity2.DataValueField = "PRIOR_AUTH_DENTAL_ORAL_CAVITY_MMIS";
                ddDentalOralCavity2.DataTextField = "PRIOR_AUTH_DENTAL_ORAL_CAVITY_MMIS";
                ddDentalOralCavity2.DataBind();
                ddDentalOralCavity2.Items.Insert(0, new ListItem("--select--", "0"));
            }

            if (ddDentalOralCavity3 != null && ddDentalOralCavity3.Visible)
            {
                ddDentalOralCavity3.DataSource = dtOralCavity;
                ddDentalOralCavity3.DataValueField = "PRIOR_AUTH_DENTAL_ORAL_CAVITY_MMIS";
                ddDentalOralCavity3.DataTextField = "PRIOR_AUTH_DENTAL_ORAL_CAVITY_MMIS";
                ddDentalOralCavity3.DataBind();
                ddDentalOralCavity3.Items.Insert(0, new ListItem("--select--", "0"));
            }

            if (ddDentalOralCavity4 != null && ddDentalOralCavity4.Visible)
            {
                ddDentalOralCavity4.DataSource = dtOralCavity;
                ddDentalOralCavity4.DataValueField = "PRIOR_AUTH_DENTAL_ORAL_CAVITY_MMIS";
                ddDentalOralCavity4.DataTextField = "PRIOR_AUTH_DENTAL_ORAL_CAVITY_MMIS";
                ddDentalOralCavity4.DataBind();
                ddDentalOralCavity4.Items.Insert(0, new ListItem("--select--", "0"));
            }

            if (ddDentalOralCavity5 != null && ddDentalOralCavity5.Visible)
            {
                ddDentalOralCavity5.DataSource = dtOralCavity;
                ddDentalOralCavity5.DataValueField = "PRIOR_AUTH_DENTAL_ORAL_CAVITY_MMIS";
                ddDentalOralCavity5.DataTextField = "PRIOR_AUTH_DENTAL_ORAL_CAVITY_MMIS";
                ddDentalOralCavity5.DataBind();
                ddDentalOralCavity5.Items.Insert(0, new ListItem("--select--", "0"));
            }

            ddToothSurface1.DataSource = dtToothSurface;
            ddToothSurface1.DataValueField = "PRIOR_AUTH_DENTALTOOTH_SURFACE_CODE";
            ddToothSurface1.DataTextField = "PRIOR_AUTH_DENTALTOOTH_SURFACE_CODE";
            ddToothSurface1.DataBind();
            ddToothSurface1.Items.Insert(0, new ListItem("--select--", "0"));

            if (ddToothSurface2 != null && ddToothSurface2.Visible)
            {
                ddToothSurface2.DataSource = dtToothSurface;
                ddToothSurface2.DataValueField = "PRIOR_AUTH_DENTALTOOTH_SURFACE_CODE";
                ddToothSurface2.DataTextField = "PRIOR_AUTH_DENTALTOOTH_SURFACE_CODE";
                ddToothSurface2.DataBind();
                ddToothSurface2.Items.Insert(0, new ListItem("--select--", "0"));
            }

            if (ddToothSurface3 != null && ddToothSurface3.Visible)
            {
                ddToothSurface3.DataSource = dtToothSurface;
                ddToothSurface3.DataValueField = "PRIOR_AUTH_DENTALTOOTH_SURFACE_CODE";
                ddToothSurface3.DataTextField = "PRIOR_AUTH_DENTALTOOTH_SURFACE_CODE";
                ddToothSurface3.DataBind();
                ddToothSurface3.Items.Insert(0, new ListItem("--select--", "0"));
            }

            if (ddToothSurface4 != null && ddToothSurface4.Visible)
            {
                ddToothSurface4.DataSource = dtToothSurface;
                ddToothSurface4.DataValueField = "PRIOR_AUTH_DENTALTOOTH_SURFACE_CODE";
                ddToothSurface4.DataTextField = "PRIOR_AUTH_DENTALTOOTH_SURFACE_CODE";
                ddToothSurface4.DataBind();
                ddToothSurface4.Items.Insert(0, new ListItem("--select--", "0"));
            }

            if (ddToothSurface5 != null && ddToothSurface5.Visible)
            {
                ddToothSurface5.DataSource = dtToothSurface;
                ddToothSurface5.DataValueField = "PRIOR_AUTH_DENTALTOOTH_SURFACE_CODE";
                ddToothSurface5.DataTextField = "PRIOR_AUTH_DENTALTOOTH_SURFACE_CODE";
                ddToothSurface5.DataBind();
                ddToothSurface5.Items.Insert(0, new ListItem("--select--", "0"));
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at BindDentalServiceDetailsDropDowns method", ex);
        }
    }

    #endregion
    protected void gvServiceDetailDental_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DateTime reqFDOS = (DateTime)DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS");
            DateTime reqTDOS = (DateTime)DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS");

            Label lblDentalSDToothNumber = (e.Row.FindControl("lblDentalSDToothNumber") as Label);
            Label lblDentalSDOralCavity = (e.Row.FindControl("lblDentalSDOralCavity") as Label);

            int toothNumber = (int)DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_TOOTH_NUMBER_ID");
            string cavity = (string)DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS");

            if (toothNumber == null || toothNumber <= 0)
            {
                lblDentalSDToothNumber.Text = "";
            }
            else
            {
                lblDentalSDToothNumber.Text = Convert.ToString(toothNumber);
            }

            if (string.IsNullOrEmpty(cavity) || cavity == "0")
            {
                lblDentalSDOralCavity.Text = "";
            }
            else
            {
                lblDentalSDOralCavity.Text = cavity;
            }


            TextBox txtDentalSDReqFDOS = (e.Row.FindControl("txtDentalSDReqFDOS") as TextBox);
            TextBox txtDentalSDReqTDOS = (e.Row.FindControl("txtDentalSDReqTDOS") as TextBox);
            Label lblDentalprocCodeType = (e.Row.FindControl("lblDentalprocCodeType") as Label);
            if (txtDentalSDReqFDOS != null)
            {
                txtDentalSDReqFDOS.Text = reqFDOS.ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
            }
            if (txtDentalSDReqTDOS != null)
            {
                txtDentalSDReqTDOS.Text = reqTDOS.ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
            }




            string statusId = (string)DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_STATUS_ID");
            TextBox txtDentalSDStatusCode1 = (e.Row.FindControl("txtDentalSDStatusCode") as TextBox);
            txtDentalSDStatusCode1.Text = statusId == "1" ? "Submission Pending" : statusId == "2" ? "Approved" : statusId == "3" ? "Denied" : statusId;
        }
    }

    private void ClearDentalDropdownSelection()
    {
        ddDentalToothNumber.ClearSelection();
        ddDentalProsthsis.ClearSelection();

        ddDentalOralCavity1.ClearSelection();

        if (ddDentalOralCavity2 != null && ddDentalOralCavity2.Visible)
        {
            ddDentalOralCavity2.ClearSelection();
        }

        if (ddDentalOralCavity3 != null && ddDentalOralCavity3.Visible)
        {
            ddDentalOralCavity3.ClearSelection();
        }

        if (ddDentalOralCavity4 != null && ddDentalOralCavity4.Visible)
        {
            ddDentalOralCavity4.ClearSelection();
        }

        if (ddDentalOralCavity5 != null && ddDentalOralCavity5.Visible)
        {
            ddDentalOralCavity5.ClearSelection();
        }

        ddToothSurface1.ClearSelection();

        if (ddToothSurface2 != null && ddToothSurface2.Visible)
        {
            ddToothSurface2.ClearSelection();
        }
        if (ddToothSurface3 != null && ddToothSurface3.Visible)
        {
            ddToothSurface3.ClearSelection();
        }

        if (ddToothSurface4 != null && ddToothSurface4.Visible)
        {
            ddToothSurface4.ClearSelection();
        }

        if (ddToothSurface5 != null && ddToothSurface5.Visible)
        {
            ddToothSurface5.ClearSelection();
        }
    }
    protected void btnDentalServiceDetailAdd1_Click(object sender, EventArgs e)
    {
        ClearDentalDropdownSelection();
        txtDentalSDProcCode.Text = string.Empty;
        // btnServDentalUpdate.Text = "Add";
        lblservDentalLineNumber.Text = "";
        //pnlDentalLine.Visible = true;
        pnlsepDentalLine.Visible = true;
        //GetDiagnoisServiceDetails();
    }
    protected void btnServDentalAddEditCancel_Click(object sender, EventArgs e)
    {
        // pnlDentalLine.Visible = false;
        pnlsepDentalLine.Visible = false;
        cpeDentalServiceDetails.Collapsed = false;
        cpeDentalServiceDetails.ClientState = "false";
        ClearDentalServiceDetailsControls();
    }

    protected void btnDiagnosisAddEditCancel_Click(object sender, EventArgs e)
    {
        //pnlselDiagnosisLine.Visible = false;
        //pnlDiagnosisLine.Visible = false;
        //ddlDiagnosisCodeType.SelectedIndex = 0;
        //txtLnDiagnosisCode.Text = string.Empty;
        //txtDiagnosisCodeDescription.Text = string.Empty;
        // GetDiagnoisServiceDetails();
        ddlDiagnosisCodeType.SelectedIndex = 0;
        txtLnDiagnosisCode.Text = "";
        txtDiagnosisDate.Text = string.Empty;
        txtDiagnosisCodeDescription.Text = string.Empty;
        // dvDiagnosisAdd.Visible = false;
        //pnlDiagnosisLine.Visible = false;
        btnDiagnosisAdd.Visible = true;
        lblIcdVersionError.Text = "";

        //SortablePagingGridView2.Visible = false;
        //SortablePagingGridView2.DataSource = null;
        //SortablePagingGridView2.DataBind();
        //SortablePagingGridView2.EmptyDataText = string.Empty;
    }

    public void DiagnosisUpdatedSave(string SaveId, string diagnosisCode, string diagnosisCodeDesc, string diagonsisType, string diagUpdateDate, string createdby, string priorAuthtype, string MedicaidId, string action)
    {
        try
        {
            DataSet dsDiagnosis = new DataSet();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms.Add("MedicaidID", MedicaidNumber);
            parms.Add("LINK_SECTIONS", SaveCodeLNK);
            dsDiagnosis = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_DIAGNOSIS", parms);

            DataTable dtDiagnosis = dsDiagnosis.Tables[0];
            if (dtDiagnosis.Columns.IndexOf("RowState") < 0)
            {
                DataColumn dcRowState = new DataColumn("RowState", typeof(string));
                dtDiagnosis.Columns.Add(dcRowState);
            }

            if (action == "Add")
            {
                DataRow drDiagnosisRow = dsDiagnosis.Tables[0].NewRow();
                drDiagnosisRow["RowState"] = ROWSTATE_ADD;

                if (Convert.ToInt32(priorAuthtype) == Convert.ToInt32(CON.ClaimsType.Dental))
                {
                    drDiagnosisRow["PRIOR_AUTH_INSTITUTIONALSAVE_ID"] = Convert.ToString(0);
                    drDiagnosisRow["PRIOR_AUTH_DENTALSAVE_ID"] = SaveId;
                    drDiagnosisRow["PRIOR_AUTH_PROFESSIONALSAVE_ID"] = Convert.ToString(0);
                }
                if (Convert.ToInt32(priorAuthtype) == Convert.ToInt32(CON.ClaimsType.Professional))
                {
                    drDiagnosisRow["PRIOR_AUTH_INSTITUTIONALSAVE_ID"] = Convert.ToString(0);
                    drDiagnosisRow["PRIOR_AUTH_DENTALSAVE_ID"] = Convert.ToString(0);
                    drDiagnosisRow["PRIOR_AUTH_PROFESSIONALSAVE_ID"] = SaveId;
                }
                if (Convert.ToInt32(priorAuthtype) == Convert.ToInt32(CON.ClaimsType.Institutional))
                {
                    drDiagnosisRow["PRIOR_AUTH_INSTITUTIONALSAVE_ID"] = SaveId;
                    drDiagnosisRow["PRIOR_AUTH_DENTALSAVE_ID"] = Convert.ToString(0);
                    drDiagnosisRow["PRIOR_AUTH_PROFESSIONALSAVE_ID"] = Convert.ToString(0);
                }


                //drDiagnosisRow["PRIOR_AUTH_INSTITUTIONALSAVE_ID"] = SaveId;
                //drDiagnosisRow["PRIOR_AUTH_DENTALSAVE_ID"] = Convert.ToString(0);
                //drDiagnosisRow["PRIOR_AUTH_PROFESSIONALSAVE_ID"] = Convert.ToString(0);
                drDiagnosisRow["PRIOR_AUTH_TYPE"] = priorAuthtype;
                drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_CODE"] = diagnosisCode;
                drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_DESC"] = diagnosisCodeDesc;
                drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"] = diagonsisType;

                DataTable dtDiagnosisCode = (DataTable)ViewState[DIAGNOSIS_CODE_TYPE];
                DataRow diagnosisCodeDescription = dtDiagnosisCode.Select("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID='" + diagonsisType + "'")[0];
                drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC"] = diagnosisCodeDescription["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC"];


                drDiagnosisRow["LAST_MODIFIED_USER"] = createdby;
                drDiagnosisRow["CREATED_BY_USER"] = createdby;
                drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_STATUS"] = Convert.ToString(0);

                if (diagUpdateDate == string.Empty)
                    drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_DATE"] = DBNull.Value;
                else
                    drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_DATE"] = diagUpdateDate;

                drDiagnosisRow["MedicaidID"] = MedicaidId;
                if (ViewState["tmp_diagonisid"] == null)
                    ViewState["tmp_diagonisid"] = 1;

                int tmp_DiagonisID = (int)ViewState["tmp_diagonisid"];
                tmp_DiagonisID = tmp_DiagonisID + 1;
                drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_ID"] = tmp_DiagonisID;
                ViewState["tmp_diagonisid"] = tmp_DiagonisID;

                dtDiagnosis.Rows.Add(drDiagnosisRow);
            }
            else
            {
                DataView dataView = dtDiagnosis.AsDataView();
                dataView.RowFilter = "PRIOR_AUTH_DIAGNOSIS_ID='" + SaveId + "'";

                if (dataView.Count > 0)
                {
                    if (dataView[0]["RowState"] == DBNull.Value || dataView[0]["RowState"] == null)
                    {
                        dataView[0]["RowState"] = ROWSTATE_UPDATE;
                    }

                    if (dataView[0]["PRIOR_AUTH_DIAGNOSIS_ID"] == DBNull.Value || dataView[0]["PRIOR_AUTH_DIAGNOSIS_ID"] == null)
                    {
                        int tmp_DiagonisID = (int)ViewState["tmp_diagonisid"];
                        dataView[0]["PRIOR_AUTH_DIAGNOSIS_ID"] = tmp_DiagonisID + 1;
                        ViewState["tmp_diagonisid"] = tmp_DiagonisID;
                    }

                    //dataView[0]["PRIOR_AUTH_INSTITUTIONALSAVE_ID"] = Convert.ToString(0);
                    //dataView[0]["PRIOR_AUTH_DENTALSAVE_ID"] = Convert.ToString(0);
                    //dataView[0]["PRIOR_AUTH_PROFESSIONALSAVE_ID"] = Convert.ToString(0);
                    dataView[0]["PRIOR_AUTH_TYPE"] = priorAuthtype;
                    dataView[0]["PRIOR_AUTH_DIAGNOSIS_CODE"] = diagnosisCode;
                    dataView[0]["PRIOR_AUTH_DIAGNOSIS_DESC"] = diagnosisCodeDesc;
                    dataView[0]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"] = diagonsisType;
                    dataView[0]["LAST_MODIFIED_USER"] = createdby;
                    dataView[0]["PRIOR_AUTH_DIAGNOSIS_STATUS"] = Convert.ToString(0);


                    DataTable dtDiagnosisCode = (DataTable)ViewState[DIAGNOSIS_CODE_TYPE];
                    DataRow diagnosisCodeDescription = dtDiagnosisCode.Select("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID='" + diagonsisType + "'")[0];
                    dataView[0]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC"] = diagnosisCodeDescription["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC"];

                    if (diagUpdateDate == string.Empty)
                        dataView[0]["PRIOR_AUTH_DIAGNOSIS_DATE"] = DBNull.Value;
                    else
                        dataView[0]["PRIOR_AUTH_DIAGNOSIS_DATE"] = diagUpdateDate;

                    dataView[0]["MedicaidID"] = MedicaidId;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at DiagnosisUpdatedSave method", ex);
        }
    }

    public void UpdateDiagnosisToDb()
    {
        try
        {
            Dictionary<string, string> parms2 = new Dictionary<string, string>();
            parms2.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms2.Add("MedicaidID", MedicaidNumber);
            parms2.Add("LINK_SECTIONS", SaveCodeLNK);
            DataSet dsDiagnosis = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_DIAGNOSIS", parms2);

            foreach (DataRow dbRow in dsDiagnosis.Tables[0].Rows)
            {
                if (dbRow["PRIOR_AUTH_DIAGNOSIS_ID"] == null || dbRow["PRIOR_AUTH_DIAGNOSIS_ID"] == DBNull.Value)
                { continue; }

                if (dbRow["RowState"].ToString() == ROWSTATE_ADD)//Add
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();

                    parms.Add("PRIOR_AUTH_INSTITUTIONALSAVE_ID", Convert.ToString(dbRow["PRIOR_AUTH_INSTITUTIONALSAVE_ID"]));
                    parms.Add("PRIOR_AUTH_DENTALSAVE_ID", Convert.ToString(dbRow["PRIOR_AUTH_DENTALSAVE_ID"]));
                    parms.Add("PRIOR_AUTH_PROFESSIONALSAVE_ID", Convert.ToString(dbRow["PRIOR_AUTH_PROFESSIONALSAVE_ID"]));
                    parms.Add("PRIOR_AUTH_DIAGNOSIS_CODE", Convert.ToString(dbRow["PRIOR_AUTH_DIAGNOSIS_CODE"]));
                    parms.Add("PRIOR_AUTH_DIAGNOSIS_DESC", Convert.ToString(dbRow["PRIOR_AUTH_DIAGNOSIS_DESC"]));
                    parms.Add("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID", Convert.ToString(dbRow["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"]));
                    parms.Add("PRIOR_AUTH_DIAGNOSIS_DATE", Convert.ToString(dbRow["PRIOR_AUTH_DIAGNOSIS_DATE"]));
                    parms.Add("PRIOR_AUTH_DIAGNOSIS_STATUS", Convert.ToString(dbRow["PRIOR_AUTH_DIAGNOSIS_STATUS"]));
                    parms.Add("CREATED_BY_USER", Convert.ToString(dbRow["CREATED_BY_USER"]));
                    parms.Add("PRIOR_AUTH_TYPE", Convert.ToString(dbRow["PRIOR_AUTH_TYPE"]));
                    parms.Add("MedicaidID", Convert.ToString(dbRow["MedicaidID"]));
                    //parms.Add("LAST_MODIFIED_USER", Convert.ToString(dbRow["CREATED_BY_USER"]));


                    int value = PriorAuthHospitalController.InsertPriorAuthServiceDetail("AUTH_DIAGNOSIS", parms);

                }
                if (dbRow["RowState"].ToString() == ROWSTATE_UPDATE)//Add
                {
                    int value = PriorAuthHospitalController.UpdatePriorDiagnosisServiceDetail(
                        (int)dbRow["PRIOR_AUTH_DIAGNOSIS_ID"],
                        (int)dbRow["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"],
                        dbRow["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString(),
                        dbRow["PRIOR_AUTH_DIAGNOSIS_DESC"].ToString(), Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(),
                        dbRow["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString());
                }
                if (dbRow["RowState"].ToString() == ROWSTATE_DELETE)
                {
                    _spa.DeletePriorDiagnosisServiceDetail(Convert.ToInt32(dbRow["PRIOR_AUTH_DIAGNOSIS_ID"]));
                }
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    public void SaveDiagnosisDataToDb(Guid lnkRecord)
    {
        Dictionary<string, string> parms2 = new Dictionary<string, string>();
        parms2.Add("PRIOR_AUTH_TYPE", returnPATypeID());
        parms2.Add("MedicaidID", MedicaidNumber);
        parms2.Add("LINK_SECTIONS", SaveCodeLNK);
        DataSet dsDiagnosis = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_DIAGNOSIS", parms2);

        //RowState Column is missing on re-save. Added as a workaround to proceed further
        if (dsDiagnosis.Tables[0].Columns.IndexOf("RowState") < 0)
        {
            DataColumn dcRowState = new DataColumn("RowState", typeof(string));
            dsDiagnosis.Tables[0].Columns.Add(dcRowState);
        }

        foreach (DataRow dbRow in dsDiagnosis.Tables[0].Rows)
        {
            if (dbRow["PRIOR_AUTH_DIAGNOSIS_ID"] == null || dbRow["PRIOR_AUTH_DIAGNOSIS_ID"] == DBNull.Value)
            { continue; }

            if (dbRow["RowState"].ToString() == ROWSTATE_ADD)//Add
            {
                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Add("PRIOR_AUTH_INSTITUTIONALSAVE_ID", Convert.ToString(dbRow["PRIOR_AUTH_INSTITUTIONALSAVE_ID"]));
                parms.Add("PRIOR_AUTH_DENTALSAVE_ID", Convert.ToString(dbRow["PRIOR_AUTH_DENTALSAVE_ID"]));
                parms.Add("PRIOR_AUTH_PROFESSIONALSAVE_ID", Convert.ToString(dbRow["PRIOR_AUTH_PROFESSIONALSAVE_ID"]));
                parms.Add("PRIOR_AUTH_DIAGNOSIS_CODE", Convert.ToString(dbRow["PRIOR_AUTH_DIAGNOSIS_CODE"]));
                parms.Add("PRIOR_AUTH_DIAGNOSIS_DESC", Convert.ToString(dbRow["PRIOR_AUTH_DIAGNOSIS_DESC"]));
                parms.Add("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID", Convert.ToString(dbRow["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"]));
                if (!String.IsNullOrEmpty(Convert.ToString(dbRow["PRIOR_AUTH_DIAGNOSIS_DATE"])))
                    parms.Add("PRIOR_AUTH_DIAGNOSIS_DATE", Convert.ToDateTime(dbRow["PRIOR_AUTH_DIAGNOSIS_DATE"]));
                else
                    parms.Add("PRIOR_AUTH_DIAGNOSIS_DATE", null);

                parms.Add("PRIOR_AUTH_DIAGNOSIS_STATUS", Convert.ToString(dbRow["PRIOR_AUTH_DIAGNOSIS_STATUS"]));
                parms.Add("PRIOR_AUTH_TYPE", Convert.ToString(dbRow["PRIOR_AUTH_TYPE"]));
                parms.Add("MedicaidID", Convert.ToString(dbRow["MedicaidID"]));

                parms.Add("LAST_MODIFIED_USER", new Guid(CON.appAdminUserId));
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
                parms.Add("CREATED_BY_USER", new Guid(CON.appAdminUserId));
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now);
                parms.Add("LINK_SECTIONS", lnkRecord);

                int value = PriorAuthHospitalController.InsertPriorAuthDiagnosisData("insertPRIOR_AUTH_SUB_DIAGNOSIS", parms);

            }
        }
    }


    public void SaveAttachmentDataToDB(Guid lnkRecord)
    {
        try
        {
            DataTable attachmentsDt = Helper.ConvertToDataTable(FetchAttachments());

            if (attachmentsDt == null) return;

            foreach (DataRow dbRow in attachmentsDt.Rows)
            {
                if (dbRow["PRIOR_AUTH_SUB_ATTACHMENT_ID"] == DBNull.Value) continue;
                //if (dbRow["RowState"].ToString() != ROWSTATE_ADD) continue;

                var parms = new Dictionary<string, object>
                    {
                        { "PRIOR_AUTH_SUB_DOCUMENT_TYPE_ID", dbRow["PRIOR_AUTH_SUB_DOCUMENT_TYPE_ID"] },
                        { "PRIOR_AUTH_SUB_TRACKING_NUMBER", dbRow["PRIOR_AUTH_SUB_TRACKING_NUMBER"] },
                        { "PRIOR_AUTH_SUB_DOCUMENT_ID", dbRow["PRIOR_AUTH_SUB_DOCUMENT_ID"] },
                        { "PRIOR_AUTH_SUB_Note", dbRow["PRIOR_AUTH_SUB_Note"] },
                        { "PRIOR_AUTH_SUB_ATTACHMENT_AUTH_TYPE", dbRow["PRIOR_AUTH_SUB_ATTACHMENT_AUTH_TYPE"] },
                        { "PRIOR_AUTH_SUB_DOCUMENT_TYPE_DESC", dbRow["PRIOR_AUTH_SUB_DOCUMENT_TYPE_DESC"] },
                        { "DOCUMENT_ID", dbRow["DOCUMENT_ID"] },
                        { "PRIOR_AUTH_SUB_ATTACHMENT_STATUS", dbRow["PRIOR_AUTH_SUB_ATTACHMENT_STATUS"] },
                        { "MedicaidId", dbRow["MedicaidId"] },
                        { "LAST_MODIFIED_USER", new Guid(CON.appAdminUserId) },
                        { "CREATED_BY_USER", new Guid(CON.appAdminUserId) },
                        { "LINK_SECTIONS", lnkRecord },
                        { "DOCUMENT_NAME", GetStringValue(dbRow, "DOCUMENT_NAME") },
                        { "ORIGINAL_DOCUMENT_NAME", GetStringValue(dbRow, "ORIGINAL_DOCUMENT_NAME") }
                    };

                PriorAuthHospitalController.InsertPriorAuthAttachmentData("insertPRIOR_AUTH_SUB_ATTACHMENT", parms);
            }
        }
        catch (Exception ex)
        {
            LogError(ex, "SubmitPA-SaveAttachmentDataToDB");
        }
    }



    private string GetStringValue(DataRow row, string columnName)
    {
        return row[columnName] != DBNull.Value ? row[columnName].ToString() : string.Empty;
    }


    private void LogError(Exception ex, string methodName)
    {
        string logNumber = CreateAndReturnLogThreadNumber(ex, methodName);
        IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. Reference ID: {0}", logNumber), "Error", string.Format("{0} {1}", ex.Message, ex.StackTrace));
    }


    protected void btnDiagnosisAddLine_Click(object sender, EventArgs e)
    {
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "MyFunction()", true);

        // BindingNPILabelsFromHiddenFields();
        try
        {
            if (string.IsNullOrEmpty(lblIcdVersionError.Text))
            {
                if (string.IsNullOrEmpty(lblIcdVersionError.Text))
                {
                    if (ddlDiagnosisCodeType.SelectedItem.Text.ToUpper() == "--- PLEASE SELECT ---")
                    {
                        lblDiagnosisSaveError.Visible = true;
                        lblDiagnosisSaveError.Text = "Please select DiagnosisType...!.";
                        return;
                    }

                    string diagUpdateDate = txtDiagnosisDate.Text;
                    var createdBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name);


                    txtDiagnosisDate.Text = string.Empty;

                    if (ValidateDiagnosisLineItem())
                    {
                        return;
                    }

                    if (rblClaimType.SelectedItem == null)
                    {
                        lblDiagnosisErrorMessage.Text = "Please select Claim Option !";
                        lblDiagnosisErrorMessage.Visible = true;
                        return;
                    }
                    else
                    {
                        lblDiagnosisErrorMessage.Text = string.Empty;
                        lblDiagnosisErrorMessage.Visible = false;
                    }

                    string diagnosisDate = txtDiagnosisDate.Text;

                    if (!string.IsNullOrEmpty(diagUpdateDate) && Convert.ToDateTime(diagUpdateDate) > DateTime.Today)
                    {
                        lblDiagnosisErrorMessage.Text = "Diagnosis date should not be future date.";
                        lblDiagnosisErrorMessage.Visible = true;
                        return;
                    }
                    else
                    {
                        lblDiagnosisErrorMessage.Text = "";
                        lblDiagnosisErrorMessage.Visible = false;
                    }

                    int saveId = PriorAuthHospitalController.GetPriorAuthDiagnosisSaveID(rblClaimType.SelectedItem.Text);
                    int diagonsisType = Convert.ToInt32(ddlDiagnosisCodeType.SelectedValue);
                    string diagnosisCode = txtLnDiagnosisCode.Text;
                    string diagnosisCodeDesc = txtDiagnosisCodeDescription.Text;

                    if (saveId <= 0)
                    {
                        saveId = saveId + 1;
                    }
                    DiagnosisUpdatedSave(Convert.ToString(saveId), diagnosisCode, diagnosisCodeDesc, Convert.ToString(diagonsisType), diagUpdateDate, Convert.ToString(createdBy), Convert.ToString(GetClaimTypeID()), MedicaidId, "Add");

                    txtDiagnosisDate.Text = "";
                    ddlDiagnosisCodeType.ClearSelection();
                    txtLnDiagnosisCode.Text = "";
                    txtDiagnosisCodeDescription.Text = "";
                    ddlDiagnosisCodeType.SelectedIndex = 0;
                    txtLnDiagnosisCode.Text = string.Empty;
                    txtDiagnosisCodeDescription.Text = string.Empty;
                    GetDiagnoisServiceDetails();
                    ddlDiagnosisCodeType.Enabled = true;
                    cpeDiagnosis.Collapsed = true;
                    cpeDiagnosis.ClientState = "false";
                    isDiagnosisLineOpened.Value = "false";

                }
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void BindingNPILabelsFromHiddenFields()
    {
        if (string.IsNullOrEmpty(lblSvcProviderFName.Text) || lblSvcProviderFName.Text != hdlblSvcProviderFName.Value)
            lblSvcProviderFName.Text = hdlblSvcProviderFName.Value != null ? hdlblSvcProviderFName.Value : "";

        if (string.IsNullOrEmpty(lblSvcProviderLName.Text) || lblSvcProviderLName.Text != hdlblSvcProviderLName.Value)
            lblSvcProviderLName.Text = hdlblSvcProviderLName.Value != null ? hdlblSvcProviderLName.Value : "";

        if (string.IsNullOrEmpty(txtMedicaidID.Text) || txtMedicaidID.Text != hdtxtMedicaidID.Value)
            txtMedicaidID.Text = hdtxtMedicaidID.Value != null ? hdtxtMedicaidID.Value : "";

        if (string.IsNullOrEmpty(lblOrdProviderFName.Text) || lblOrdProviderFName.Text != hdlblOrdProviderFName.Value)
            lblOrdProviderFName.Text = hdlblOrdProviderFName.Value != null ? hdlblOrdProviderFName.Value : "";

        if (string.IsNullOrEmpty(lblOrdProviderLName.Text) || lblOrdProviderLName.Text != hdlblOrdProviderLName.Value)
            lblOrdProviderLName.Text = hdlblOrdProviderLName.Value != null ? hdlblOrdProviderLName.Value : "";

        if (string.IsNullOrEmpty(txtOMID.Text) || txtOMID.Text != hdtxtOMID.Value)
            txtOMID.Text = hdtxtOMID.Value != null ? hdtxtOMID.Value : "";
    }

    protected void btnDiagnosisUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(lblIcdVersionError.Text))
            {

                if (ddlDiagnosisCodeType.SelectedItem.Text.ToUpper() == "--- PLEASE SELECT ---")
                {
                    lblDiagnosisSaveError.Visible = true;
                    lblDiagnosisSaveError.Text = "Please select DiagnosisType...!.";
                    return;
                }


                string diagUpdateDate = txtDiagnosisDate.Text;
                var createdBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

                int disgid = Convert.ToInt32(hdDiagLineNum.Value);
                string diagUpdateCode = txtLnDiagnosisCode.Text;
                string diagUpdateCodeDesc = txtDiagnosisCodeDescription.Text;
                int diagUpdateType = Convert.ToInt32(ddlDiagnosisCodeType.SelectedValue);
                if (!string.IsNullOrEmpty(diagUpdateDate) && Convert.ToDateTime(diagUpdateDate) > DateTime.Today)
                {
                    lblDiagnosisErrorMessage.Text = "Diagnosis date should not be future date.";
                    return;
                }
                DiagnosisUpdatedSave(disgid.ToString(), diagUpdateCode, diagUpdateCodeDesc, diagUpdateType.ToString(), diagUpdateDate, createdBy.ToString(), Convert.ToString(GetClaimTypeID()), MedicaidId, "update");

                txtDiagnosisDate.Text = "";
                ddlDiagnosisCodeType.ClearSelection();
                txtLnDiagnosisCode.Text = "";
                txtDiagnosisCodeDescription.Text = "";
                // pnlDiagnosisLine.Visible = false;
                ddlDiagnosisCodeType.SelectedIndex = 0;
                txtLnDiagnosisCode.Text = string.Empty;
                txtDiagnosisCodeDescription.Text = string.Empty;
                GetDiagnoisServiceDetails();
                ddlDiagnosisCodeType.Enabled = true;
                cpeDiagnosis.Collapsed = true;
                cpeDiagnosis.ClientState = "false";
                isDiagnosisLineOpened.Value = "false";
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private bool ValidateDiagnosisLineItem()
    {
        try
        {
            string codeType = ddlDiagnosisCodeType.SelectedItem.Text;

            DataSet ds = LoadDiagnosisDataToSession().Copy();
            if (ds.Tables[0].Columns.IndexOf("RowState") > 0)
            {
                DataRow[] rowsToRemove = ds.Tables[0].Select("RowState='delete'");
                if (rowsToRemove != null && rowsToRemove.Count() > 0)
                {
                    foreach (DataRow dataRow in rowsToRemove)
                    {
                        ds.Tables[0].Rows.Remove(dataRow);
                    }
                }
            }


            if (ds != null && ds.Tables.Count > 0)
            {
                var dataTable = ds.Tables[0];
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows.Count >= 12)
                    {
                        btnDiagnosisAdd.Visible = false;

                        return true;
                    }
                    else
                    {
                        btnDiagnosisAdd.Visible = true;
                    }
                    //if (codeType.ToUpper() == "--- PLEASE SELECT ---")
                    //{
                    //    lblDiagnosisSaveError.Visible = true;
                    //    lblDiagnosisSaveError.Text = "Please select DiagnosisType...!.";
                    //    return true;
                    //}
                    if (codeType.ToUpper() == "PRINCIPAL")
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            if (Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"]) == Convert.ToString(ddlDiagnosisCodeType.SelectedValue))
                            {
                                lblDiagnosisSaveError.Visible = true;
                                lblDiagnosisSaveError.Text = "Principal diagnosis type cannot be added more than one.";
                                return true;
                            }
                        }
                    }
                    if (codeType.ToUpper() == "ADMITTING")
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            if (Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"]) == Convert.ToString(ddlDiagnosisCodeType.SelectedValue))
                            {
                                lblDiagnosisSaveError.Visible = true;
                                lblDiagnosisSaveError.Text = "Admitting diagnosis type cannot be added more than one.";
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    if (ddlDiagnosisCodeType.SelectedItem.Text.ToUpper() != "PRINCIPAL")
                    {
                        lblDiagnosisSaveError.Visible = true;
                        lblDiagnosisSaveError.Text = "Default line 01 should be Principal.";
                        return true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw CoreException.ThrowException(ex);
        }

        lblDiagnosisSaveError.Visible = false;
        lblDiagnosisSaveError.Text = "";
        return false;
    }

    private void EnableDiagnosisCodeDropdown()
    {
        try
        {
            var ds = LoadDiagnosisDataToSession();

            DataTable diagnosis = ds.Tables[0];
            bool isPrincipalAdded = false;
            bool isAdmittingAdded = false;

            foreach (DataRow dr in diagnosis.Rows)
            {
                if (Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"]) == "1")
                {
                    isPrincipalAdded = true;
                }
                if (Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"]) == "2")
                {
                    isAdmittingAdded = true;
                }
            }

            int diagnCount = ds != null ? ds.Tables[0].Rows.Count : 0;
            GetDiagnosisCodeType();
            if (diagnCount == 0)
            {

                ddlDiagnosisCodeType.Items.FindByText("Principal").Selected = true;
                ddlDiagnosisCodeType.Enabled = false;
            }
            if (diagnCount == 1)
            {
                if (!isPrincipalAdded)
                {
                    ddlDiagnosisCodeType.Items.FindByText("Principal").Selected = true;
                    ddlDiagnosisCodeType.Enabled = false;
                }
                else if (!isAdmittingAdded)
                {
                    ListItem removeItem = ddlDiagnosisCodeType.Items.FindByText("Principal");
                    ddlDiagnosisCodeType.Items.Remove(removeItem);
                    ddlDiagnosisCodeType.Enabled = true;
                }
            }
            if (diagnCount >= 2 && isPrincipalAdded && isAdmittingAdded)
            {
                ListItem Principal = ddlDiagnosisCodeType.Items.FindByText("Principal");
                ddlDiagnosisCodeType.Items.Remove(Principal);
                ListItem Admitting = ddlDiagnosisCodeType.Items.FindByText("Admitting");
                ddlDiagnosisCodeType.Items.Remove(Admitting);

                ListItem selectedListItem = ddlDiagnosisCodeType.Items.FindByText("--- Please select ---");
                ddlDiagnosisCodeType.SelectedIndex = ddlDiagnosisCodeType.Items.IndexOf(ddlDiagnosisCodeType.Items.FindByValue("--- Please select ---"));

                ddlDiagnosisCodeType.Enabled = true;

            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at EnableDiagnosisCodeDropdown method", ex);
        }
    }

    protected void btnDiagnosisAdd_Click(object sender, EventArgs e)
    {
        // pnlselDiagnosisLine.Visible = true;
        ddlDiagnosisCodeType.ClearSelection();
        //dvDiagnosisAdd.Visible = true;
        pnlDiagnosisLine.Visible = true;
        //btnDiagnosisUpdateAdd.Text = "Add";
        lblDiagnosisDateErrMsg.Text = string.Empty;
        EnableDiagnosisCodeDropdown();
    }

    private void ClearDentalServiceDetailsControls()
    {
        txtDentalSDProcCode.Text = "";
        txtDentalReqUnits.Text = "";
        txtDentalAuthUnits.Text = "";
        txtDentalProcCodeDescription.Text = "";

        ddDentalToothNumber.Items[0].Selected = true;
        ddDentalProsthsis.Items[0].Selected = true;

        ddDentalOralCavity1.Items[0].Selected = true;
        if (ddDentalOralCavity2 != null && ddDentalOralCavity2.Visible)
        {
            ddDentalOralCavity2.Items[0].Selected = true;
        }

        if (ddDentalOralCavity3 != null && ddDentalOralCavity3.Visible)
        {
            ddDentalOralCavity3.Items[0].Selected = true;
        }

        if (ddDentalOralCavity4 != null && ddDentalOralCavity4.Visible)
        {
            ddDentalOralCavity4.Items[0].Selected = true;
        }

        if (ddDentalOralCavity5 != null && ddDentalOralCavity5.Visible)
        {
            ddDentalOralCavity5.Items[0].Selected = true;
        }

        ddToothSurface1.Items[0].Selected = true;

        if (ddToothSurface2 != null && ddToothSurface2.Visible)
        {
            ddToothSurface2.Items[0].Selected = true;
        }

        if (ddToothSurface3 != null && ddToothSurface3.Visible)
        {
            ddToothSurface3.Items[0].Selected = true;
        }

        if (ddToothSurface4 != null && ddToothSurface4.Visible)
        {
            ddToothSurface4.Items[0].Selected = true;
        }

        if (ddToothSurface5 != null && ddToothSurface5.Visible)
        {
            ddToothSurface5.Items[0].Selected = true;
        }

        txtDentalProvServnote.Text = "";
        txtDentalReqDollars.Text = "";
        txtDentalAuthDollars.Text = "";
        txtDentalReqFDOS.Text = "";
        txtDentalAuthFDOS.Text = "";
        txtDentalReqTDOS.Text = "";
        txtDentalAuthTDOS.Text = "";
        txtDentalRemainingUnits.Text = "";
        txtDentalServTrackingNo.Text = "";
        txtDentalServDetailsStatus.Text = "";
    }

    private const string ROWSTATE_ADD = "added";
    private const string ROWSTATE_UPDATE = "updated";
    private const string ROWSTATE_DELETE = "deleted";


    protected void lnkDentalSDProcCodeSearchLink_Click(object sender, EventArgs e)
    {
        //RefreshProcedureCodeSearchData();
        mpeSubmitPriorAuthSearchProc.Show();
        Session["ProcCodeFrom"] = "Institutional";
        Session["ProcCodeFrom"] = "Dental";
    }

    protected void gvServiceDetailProfessional_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DateTime reqFDOS = (DateTime)DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS");
            DateTime reqTDOS = (DateTime)DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS");
            TextBox txtProfessionalSDReqUnitMeasurement = (e.Row.FindControl("txtProfessionalSDReqUnitMeasurement") as TextBox);

            TextBox txtProfessionalReqFDOS = (e.Row.FindControl("txtProfessionalSDReqFDOS") as TextBox);
            TextBox txtProfessionalReqTDOS = (e.Row.FindControl("txtProfessionalSDReqTDOS") as TextBox);

            if (DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID") != null)
            {
                string val = DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID").ToString();
                int valINT = 0;
                int reqUnits = 0;
                if (int.TryParse(val, out valINT))
                {
                    reqUnits = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"));

                }
                ListItem listItem = ddProffSDMeasurements.Items.FindByValue(Convert.ToString(reqUnits));
                if (listItem != null && txtProfessionalSDReqUnitMeasurement != null)
                {
                    txtProfessionalSDReqUnitMeasurement.Text = listItem.Text;
                }
            }

            if (txtProfessionalReqFDOS != null)
            {
                txtProfessionalReqFDOS.Text = reqFDOS.ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
            }
            if (txtProfessionalReqTDOS != null)
            {
                txtProfessionalReqTDOS.Text = reqTDOS.ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
            }

            string statusId = (string)DataBinder.Eval(e.Row.DataItem, "PRIOR_AUTH_STATUS_ID");
            TextBox txtProfessionalSDStatusCode1 = (e.Row.FindControl("txtProfessionalSDStatusCode") as TextBox);
            txtProfessionalSDStatusCode1.Text = statusId == "1" ? "Submission Pending" : statusId == "2" ? "Approved" : statusId == "3" ? "Denied" : statusId;
        }
    }

    protected void gvServiceDetailProfessional_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void ClearProfessionalDropdownSelection()
    {
        ddProffSDMeasurements.ClearSelection();
    }

    protected void lnkProfessionalSDProcCodeSearchLink_Click(object sender, EventArgs e)
    {
        // RefreshProcedureCodeSearchData();
        Session["ProcCodeFrom"] = "Professional";
        //gvSubmitClaimSearchProcPop.DataSource = null;
        //gvSubmitClaimSearchProcPop.DataBind();
        mpeSubmitPriorAuthSearchProc.Show();
    }

    protected void btnServProfessionalAddEditCancel_Click(object sender, EventArgs e)
    {
        //  pnlProfessionalLine.Visible = false;
        pnlsepProfessionalLine.Visible = false;
        cpeProfessionalServiceDetail.Collapsed = false;
        cpeProfessionalServiceDetail.ClientState = "false";
        ClearProfessionalServiceDetailsControls();
    }
    private void ClearProfessionalServiceDetailsControls()
    {
        txtProfessionalSDProcCode.Text = "";
        txtProfessionalReqUnits.Text = "";
        txtProfessionalAuthUnits.Text = "";
        txtProfessionalProcCodeDescription.Text = "";
        txtProfessionalProvServnote.Text = "";
        txtProfessionalReqDollars.Text = "";
        txtProfessionalAuthDollars.Text = "";
        txtProfessionalReqFDOS.Text = "";
        txtProfessionalAuthFDOS.Text = "";
        txtProfessionalReqTDOS.Text = "";
        txtProfessionalAuthTDOS.Text = "";
        txtProfessionalRemainingUnits.Text = "";
        txtProfessionalServTrackingNo.Text = "";
        txtProfessionalServDetailsStatus.Text = "";
    }
    private void GetProfessionalServiceDetails()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            DataSet ds = null; DataTable dataTable = null;
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms.Add("LINK_SECTIONS", SaveCodeLNK);
            ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
            dataTable = ds.Tables[0];

            if (dataTable.Columns.IndexOf("RowState") < 0)
            {
                DataColumn dcRowState = new DataColumn("RowState", typeof(string));
                dataTable.Columns.Add(dcRowState);
            }


            if (dataTable != null && dataTable.Rows.Count > 0)
            {

                DataRow[] dtRows = dataTable.Select("RowState='" + ROWSTATE_DELETE + "'");
                foreach (DataRow dr in dtRows)
                {
                    dataTable.Rows.Remove(dr);
                }


                //gvServiceDetailProfessional.DataSource = dataTable;
                //gvServiceDetailProfessional.DataBind();
                //gvServiceDetailProfessional.Visible = true;
                //  lblprofNoDetailsFound.Visible = false;

                if (dataTable.Rows.Count == 0)
                {
                    // gvServiceDetailProfessional.Visible = false;
                    // lblprofNoDetailsFound.Visible = true;
                    btnProfessionalServiceDetailAdd.Visible = true;
                }

            }
            else
            {
                // gvServiceDetailProfessional.Visible = false;
                // lblprofNoDetailsFound.Visible = true;

            }

            if (hdnPAInquiryTrn.Value.Equals("PAInquiry") && dataTable.Rows.Count != 0)
            {
                //gvServiceDetailProfessional.Visible = true;
                // lblprofNoDetailsFound.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetProfessionalServiceDetails method", ex);
        }
    }

    public static int intPRIOR_AUTH_PROFFSERVICE_DETAIL_ID = 5000;

    private string ValidateServiceDetailDates(string fromDos, string ToDos)
    {
        string messsage = string.Empty;
        try
        {
            if (!string.IsNullOrEmpty(fromDos) && !string.IsNullOrEmpty(ToDos))
            {
                if (DateTime.Compare(Convert.ToDateTime(fromDos), Convert.ToDateTime(ToDos)) > 0)
                {
                    messsage = "* Choose DateFrom value prior to DateTo value";
                }
                if (Convert.ToDateTime(fromDos).Date > Convert.ToDateTime(ToDos).Date)
                {
                    messsage = "Todos is must be greater than FromDOS";
                }
            }
            DateTime date48MonthBack = DateTime.Today.AddMonths(-48);
            if (!string.IsNullOrEmpty(fromDos))
            {
                if (Convert.ToDateTime(fromDos).Date < date48MonthBack)
                {
                    messsage = "* System Allow up to 48 months back";
                }
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
        return messsage;
    }

    private bool SavePriorAuthInstitutional(Guid lnlRecord)
    {
        bool status = false;
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            int institutionalSaveId = _spa.GetPriorAuthServiceDetailSaveID(rblClaimType.SelectedValue, MedicaidId);
            int attachmentId = _spa.GetPriorAuthAttachmentSaveID(rblClaimType.SelectedValue);

            DateTime DOB;
            DateTime? DOBnull = null;
            if (DateTime.TryParse(txtBirthDate.Text, out DOB))
            {
                DOBnull = DOB;
            }

            DateTime FAdmissionDate;
            DateTime? FAdmissionDatenull = null;
            if (DateTime.TryParse(txtFAdmissionDate.Text, out FAdmissionDate))
            {
                FAdmissionDatenull = FAdmissionDate;
            }

            DateTime FDischargeDate;
            DateTime? FDischargeDatenull = null;
            if (DateTime.TryParse(txtFDischargeDate.Text, out FDischargeDate))
            { FDischargeDatenull = FDischargeDate; }

            DateTime EstBirthDate;
            DateTime? EstBirthDatenull = null;
            if (DateTime.TryParse(txtEstBirthDate.Text, out EstBirthDate))
            { EstBirthDatenull = EstBirthDate; }

            DateTime AccidentDate;
            DateTime? AccidentDatenull = null;
            if (DateTime.TryParse(txtAccidentDate.Text, out AccidentDate))
            { AccidentDatenull = AccidentDate; }

            DateTime LastMensDate;
            DateTime? LastMensDatenull = null;
            if (DateTime.TryParse(txtLstMensPeriod.Text, out LastMensDate))
            { LastMensDatenull = LastMensDate; }

            DateTime IllnessDate;
            DateTime? IllnessDatenull = null;
            if (DateTime.TryParse(txtOnsetIllness.Text, out IllnessDate))
            { IllnessDatenull = IllnessDate; }

            Dictionary<string, object> parms = new Dictionary<string, object>();
            var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

            var claimType = rblClaimType.SelectedValue.ToUpper();
            if (claimType == "DENTAL")
                parms.Add("PRIOR_AUTH_Authorization_Type_ID", "1");
            else if (claimType == "PROFESSIONAL")
                parms.Add("PRIOR_AUTH_Authorization_Type_ID", "2");
            else if (claimType == "INSTITUTIONAL")
                parms.Add("PRIOR_AUTH_Authorization_Type_ID", "3");

            parms.Add("LINK_SECTIONS", lnlRecord);
            parms.Add("PRIOR_AUTH_INSTSAVE_PROVIDER_MEDICAIDID", Convert.ToString(MedicaidId));
            parms.Add("PRIOR_AUTH_INSTSAVE_PROVIDER_NPI", Convert.ToString(txtNPI.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_PROVIDER_NAME", Convert.ToString(""));//txtSPName.Text
            parms.Add("PRIOR_AUTH_DESTINATION_PAYER_ID", Convert.ToString(ddlAuthorization.SelectedValue));
            parms.Add("PRIOR_AUTH_Assignment_Type_ID", Convert.ToString(ddlAssignment.SelectedValue));
            parms.Add("PRIOR_AUTH_SERVICE_TYPE_ID", Convert.ToString(ddlServiceType.SelectedValue));

            //recipient info params here
            parms.Add("PRIOR_AUTH_INSTSAVE_Medicaid_Billing_Number", Convert.ToString(txtMedicaidBillingNumber.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_BIRTH_DATE", DOBnull);
            parms.Add("PRIOR_AUTH_INSTSAVE_FIRST_NAME", Convert.ToString(txtfrstmi2.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_MIDDLE_INITIAL", Convert.ToString(txtMiddleName.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_LAST_NAME", Convert.ToString(txtLastName2.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_PATRACKING_NUMBER", Convert.ToString(txtPatientTrckNum.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_GENDER", Convert.ToString(txtGender2.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_ADDRESS_Line1", Convert.ToString(txtAddress1.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_ADDRESS_Line2", Convert.ToString(lblAddress2.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_CONTACT_CITY", Convert.ToString(txtCity.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_CONTACT_STATE", Convert.ToString(txtState.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_CONTACT_ZIP", Convert.ToString(txtZipCode.Text));

            //contact info params here
            parms.Add("PRIOR_AUTH_INSTSAVE_CONTACT_FIRST_NAME", Convert.ToString(txtContactName.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_CONTACT_LAST_NAME", Convert.ToString(txtContactLastName.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_CONTACT_NUMBER", Convert.ToString(new string(txtContactNumber.Text.Where(x => char.IsDigit(x)).ToArray())));
            parms.Add("PRIOR_AUTH_INSTSAVE_CONTACT_EXTENSION", Convert.ToString(txtExt.Text));

            //service info params here
            var result = Regex.Match(txtfacilityType.Text, @"\d+").Value;
            parms.Add("PRIOR_AUTH_FACILITY_TYPE_ID", result == "" ? null : Convert.ToString(result));
            parms.Add("PRIOR_AUTH_INSTSAVE_ADMISSION_DATE", FAdmissionDatenull);
            parms.Add("PRIOR_AUTH_INSTSAVE_DISCHARGE_DATE", FDischargeDatenull);
            parms.Add("PRIOR_AUTH_INSTSAVE_DISCHARGE_STATUS_ID", Convert.ToString(ddlDischargeStatus.SelectedValue));

            parms.Add("PRIOR_AUTH_INSTSAVE_MENSTRUAL_PERIOD", LastMensDatenull);
            parms.Add("PRIOR_AUTH_INSTSAVE_ACCIDENT_DATE", EstBirthDatenull);
            parms.Add("PRIOR_AUTH_INSTSAVE_ESTIMATE_DATEBIRTH", IllnessDatenull);
            parms.Add("PRIOR_AUTH_INSTSAVE_DATEONSET_ILLNESS", AccidentDatenull);

            parms.Add("PRIOR_AUTH_INSTSAVE_LEVELSERVICE_ID", Convert.ToString(ddlLevelService.SelectedValue));
            parms.Add("PRIOR_AUTH_INSTSAVE_ADMISSIONTYPE_ID", Convert.ToString(ddlAdminType.SelectedValue));
            parms.Add("PRIOR_AUTH_INSTSAVE_ADMISSIONSOURCE_ID", Convert.ToString(hdnAdminSrc.Value));
            parms.Add("PRIOR_AUTH_DELAY_REASON_ID", Convert.ToString(ddlDlyReason.SelectedValue));
            parms.Add("PRIOR_AUTH_INSTSAVE_ASSOCIATED_PA_NO", Convert.ToString(txtPaNum.Text));

            //service provider info params here
            parms.Add("PRIOR_AUTH_INSTSAVE_SERVICE_PROVIDER_NPI", Convert.ToString(txtSPNPI.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_SERVICE_PROVIDER_Medicaid_ID", Convert.ToString(txtMedicaidID.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_SERVICE_PROVIDER_FIRST_NAME", Convert.ToString(lblSvcProviderFName.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_SERVICE_PROVIDER_LAST_NAME", Convert.ToString(lblSvcProviderLName.Text));

            //ordering provider info params here
            parms.Add("PRIOR_AUTH_INSTSAVE_ORDERING_PROVIDER_Medicaid_ID", Convert.ToString(txtOMID.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_ORDERING_PROVIDER_NPI", Convert.ToString(txtorderingprovidernpi.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_ORDERING_PROVIDER_FIRST_NAME", Convert.ToString(lblOrdProviderFName.Text));
            parms.Add("PRIOR_AUTH_INSTSAVE_ORDERING_PROVIDER_LAST_NAME", Convert.ToString(lblOrdProviderLName.Text));

            parms.Add("PRIOR_AUTH_INSTDIAGNOSIS_ID", lnlRecord);
            parms.Add("PRIOR_AUTH_INSTSERVICE_DETAIL_ID", lnlRecord);
            parms.Add("PRIOR_AUTH_INST_NOTES", hdnProvNoteText.Value);
            parms.Add("PRIOR_AUTH_INSTATTACHMENT_ID", lnlRecord);

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
            parms.Add("LAST_MODIFIED_USER", id);
            parms.Add("Created_On_Date_Time", DateTime.Now);
            parms.Add("Created_By_User", id);

            parms.Add("PRIOR_AUTH_SUB_DESTINATION_PAYER_MCE_ID", txtDestinationpayerID.Value);

            int SaveId = PriorAuthHospitalController.SavePriorAuth(CON.PAClaimsType.Institutional, parms);
        }
        catch (Exception ex)
        {
            throw CoreException.ThrowException(ex);
        }
        return status;
    }
    private bool ValidatePriorAuthSaveDetail(int paType, string patientTrackingNo)
    {
        try
        {
            int priorAuthSaveId = PriorAuthHospitalController.GetPriorAuthDetailSaveID(paType, MedicaidId, patientTrackingNo);
            if (priorAuthSaveId == 0) return true;
            else return false;

        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            throw ex;
        }
    }

    private bool SavePriorAuthDental(Guid lnlRecord)
    {
        bool status = false;
        try
        {
            Dictionary<string, object> parms = new Dictionary<string, object>();
            var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

            DateTime DOB;
            DateTime? DOBnull = null;
            if (DateTime.TryParse(txtBirthDate.Text, out DOB))
            {
                DOBnull = DOB;
            }

            DateTime FAdmissionDate;
            DateTime? FAdmissionDatenull = null;
            if (DateTime.TryParse(txtFAdmissionDate.Text, out FAdmissionDate))
            {
                FAdmissionDatenull = FAdmissionDate;
            }

            DateTime FDischargeDate;
            DateTime? FDischargeDatenull = null;
            if (DateTime.TryParse(txtFDischargeDate.Text, out FDischargeDate))
            { FDischargeDatenull = FDischargeDate; }

            DateTime EstBirthDate;
            DateTime? EstBirthDatenull = null;
            if (DateTime.TryParse(txtEstDOB.Text, out EstBirthDate))
            { EstBirthDatenull = EstBirthDate; }

            DateTime AccidentDate;
            DateTime? AccidentDatenull = null;
            if (DateTime.TryParse(txtAccDtService.Text, out AccidentDate))
            { AccidentDatenull = AccidentDate; }

            DateTime LastMensDate;
            DateTime? LastMensDatenull = null;
            if (DateTime.TryParse(txtMenDtInst.Text, out LastMensDate))
            { LastMensDatenull = LastMensDate; }

            DateTime IllnessDate;
            DateTime? IllnessDatenull = null;
            if (DateTime.TryParse(txtProfOnsetIllness.Text, out IllnessDate))
            { IllnessDatenull = IllnessDate; }

            DateTime PatientEventDate;
            DateTime? PatientEventDatenull = null;
            if (DateTime.TryParse(TextBox3.Text, out PatientEventDate))
            {
                PatientEventDatenull = PatientEventDate;
            }

            var claimType = rblClaimType.SelectedValue.ToUpper();

            if (claimType == "DENTAL")
                parms.Add("PRIOR_AUTH_Authorization_Type_ID", "1");
            else if (claimType == "PROFESSIONAL")
                parms.Add("PRIOR_AUTH_Authorization_Type_ID", "2");
            else if (claimType == "INSTITUTIONAL")
                parms.Add("PRIOR_AUTH_Authorization_Type_ID", "3");

            parms.Add("LINK_SECTIONS", lnlRecord);

            parms.Add("PRIOR_AUTH_DENTALSAVE_PROVIDER_MEDICAIDID", Convert.ToString(MedicaidId));
            parms.Add("PRIOR_AUTH_DENTALSAVE_PROVIDER_NPI", Convert.ToString(txtNPI.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_PROVIDER_NAME", Convert.ToString(""));//txtSPName.Text
            parms.Add("PRIOR_AUTH_DESTINATION_PAYER_ID", Convert.ToString(ddlAuthorization.SelectedValue));
            parms.Add("PRIOR_AUTH_Assignment_Type_ID", Convert.ToString(ddlAssignment.SelectedValue));
            parms.Add("PRIOR_AUTH_SERVICE_TYPE_ID", Convert.ToString(ddlServiceType.SelectedValue));

            //Add recipient info params here
            parms.Add("PRIOR_AUTH_DENTALSAVE_Medicaid_Billing_Number", Convert.ToString(txtMedicaidBillingNumber.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_BIRTH_DATE", DOBnull);
            parms.Add("PRIOR_AUTH_DENTALSAVE_FIRST_NAME", Convert.ToString(txtfrstmi2.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_MIDDLE_INITIAL", Convert.ToString(txtMiddleName.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_LAST_NAME", Convert.ToString(txtLastName2.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_PATRACKING_NUMBER", Convert.ToString(txtPatientTrckNum.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_GENDER", Convert.ToString(txtGender2.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_ADDRESS_Line1", Convert.ToString(txtAddress1.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_ADDRESS_Line2", Convert.ToString(lblAddress2.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_CONTACT_CITY", Convert.ToString(txtCity.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_CONTACT_STATE", Convert.ToString(txtState.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_CONTACT_ZIP", Convert.ToString(txtZipCode.Text));

            //Add Contact info params here
            parms.Add("PRIOR_AUTH_DENTALSAVE_CONTACT_FIRST_NAME", Convert.ToString(txtContactName.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_CONTACT_LAST_NAME", Convert.ToString(txtContactLastName.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_CONTACT_NUMBER", Convert.ToString(new string(txtContactNumber.Text.Where(x => char.IsDigit(x)).ToArray())));
            parms.Add("PRIOR_AUTH_DENTALSAVE_CONTACT_EXTENSION", Convert.ToString(txtExt.Text));

            //Add Service info param here
            var result = Regex.Match(txtpalceofservice.Text, @"\d+").Value;
            parms.Add("PRIOR_AUTH_PLACE_OF_SERVICE_ID", result == "" ? null : Convert.ToString(result));
            parms.Add("PRIOR_AUTH_DENTALSAVEMENSTRUAL_ACCIDENT_DATE", AccidentDatenull);
            parms.Add("PRIOR_AUTH_DENTALSAVE_DATE_LASTMENSTRUAL_PERIOD", LastMensDatenull);
            parms.Add("PRIOR_AUTH_DENTALSAVEMENSTRUAL_ESTIMATE_DATEBIRTH", EstBirthDatenull);
            parms.Add("PRIOR_AUTH_DENTALSAVEMENSTRUAL_DATEONSET_ILLNESS", IllnessDatenull);
            parms.Add("PRIOR_AUTH_DENTALSAVE_PATIENT_EVENT_DATE", PatientEventDatenull);
            parms.Add("PRIOR_AUTH_DELAY_REASON_ID", Convert.ToString(ddlDelayedInst.SelectedValue));
            parms.Add("PRIOR_AUTH_DENTALSAVE_ASSOCIATED_PA_NO", Convert.ToString(txtPANumInst.Text));

            //Service Provider NPI Params here
            parms.Add("PRIOR_AUTH_DENTALSAVE_SERVICE_PROVIDER_NPI", Convert.ToString(txtSPNPI.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_SERVICE_PROVIDER_Medicaid_ID", Convert.ToString(txtMedicaidID.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_SERVICE_PROVIDER_FIRST_NAME", Convert.ToString(lblSvcProviderFName.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_SERVICE_PROVIDER_LAST_NAME", Convert.ToString(lblSvcProviderLName.Text));

            //Ordering provider NPI Params info 
            parms.Add("PRIOR_AUTH_DENTALSAVE_ORDERING_PROVIDER_Medicaid_ID", Convert.ToString(txtOMID.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_ORDERING_PROVIDER_NPI", Convert.ToString(txtorderingprovidernpi.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_ORDERING_PROVIDER_FIRST_NAME", Convert.ToString(lblOrdProviderFName.Text));
            parms.Add("PRIOR_AUTH_DENTALSAVE_ORDERING_PROVIDER_LAST_NAME", Convert.ToString(lblOrdProviderLName.Text));

            parms.Add("PRIOR_AUTH_DIAGNOSIS_ID", lnlRecord);
            parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_ID", lnlRecord);
            parms.Add("PRIOR_AUTH_DENTAL_NOTES", hdnProvNoteText.Value);
            parms.Add("PRIOR_AUTH_ATTACHMENT_ID", lnlRecord);

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", id);
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", id);

            parms.Add("PRIOR_AUTH_SUB_DESTINATION_PAYER_MCE_ID", txtDestinationpayerID.Value);

            int SaveId = PriorAuthHospitalController.SavePriorAuth(CON.PAClaimsType.Dental, parms);
        }
        catch (Exception ex)
        {
            throw CoreException.ThrowException(ex);
        }
        return status;
    }

    private bool SavePriorAuthProfessional(Guid lnlRecord)
    {
        bool status = false;
        try
        {
            Dictionary<string, object> parms = new Dictionary<string, object>();
            var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

            int professionalSDSaveId = _spa.GetPriorAuthServiceDetailSaveID(rblClaimType.SelectedValue, MedicaidId);
            int attachmentId = _spa.GetPriorAuthAttachmentSaveID(rblClaimType.SelectedValue);

            DateTime PatientEventDate;
            DateTime? PatientEventDatenull = null;
            if (DateTime.TryParse(TextBox3.Text, out PatientEventDate))
            {
                PatientEventDatenull = PatientEventDate;
            }

            DateTime DOB;
            DateTime? DOBnull = null;
            if (DateTime.TryParse(txtBirthDate.Text, out DOB))
            {
                DOBnull = DOB;
            }

            DateTime FAdmissionDate;
            DateTime? FAdmissionDatenull = null;
            if (DateTime.TryParse(txtFAdmissionDate.Text, out FAdmissionDate))
            {
                FAdmissionDatenull = FAdmissionDate;
            }

            DateTime FDischargeDate;
            DateTime? FDischargeDatenull = null;
            if (DateTime.TryParse(txtFDischargeDate.Text, out FDischargeDate))
            { FDischargeDatenull = FDischargeDate; }

            DateTime EstBirthDate;
            DateTime? EstBirthDatenull = null;
            if (DateTime.TryParse(txtEstDOB.Text, out EstBirthDate))
            { EstBirthDatenull = EstBirthDate; }

            DateTime AccidentDate;
            DateTime? AccidentDatenull = null;
            if (DateTime.TryParse(txtAccDtService.Text, out AccidentDate))
            { AccidentDatenull = AccidentDate; }

            DateTime LastMensDate;
            DateTime? LastMensDatenull = null;
            if (DateTime.TryParse(txtMenDtInst.Text, out LastMensDate))
            { LastMensDatenull = LastMensDate; }

            DateTime IllnessDate;
            DateTime? IllnessDatenull = null;
            if (DateTime.TryParse(txtProfOnsetIllness.Text, out IllnessDate))
            { IllnessDatenull = IllnessDate; }

            var claimType = rblClaimType.SelectedValue.ToUpper();
            if (claimType == "DENTAL")
                parms.Add("PRIOR_AUTH_Authorization_Type_ID", "1");
            else if (claimType == "PROFESSIONAL")
                parms.Add("PRIOR_AUTH_Authorization_Type_ID", "2");
            else if (claimType == "INSTITUTIONAL")
                parms.Add("PRIOR_AUTH_Authorization_Type_ID", "3");

            parms.Add("LINK_SECTIONS", lnlRecord);
            parms.Add("PRIOR_AUTH_PROFSAVE_PROVIDER_MEDICAIDID", Convert.ToString(MedicaidId));
            parms.Add("PRIOR_AUTH_PROFSAVE_PROVIDER_NPI", Convert.ToString(txtNPI.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_PROVIDER_NAME", Convert.ToString(""));//txtSPName.Text

            parms.Add("PRIOR_AUTH_DESTINATION_PAYER_ID", Convert.ToString(ddlAuthorization.SelectedValue));
            parms.Add("PRIOR_AUTH_Assignment_Type_ID", Convert.ToString(ddlAssignment.SelectedValue));
            parms.Add("PRIOR_AUTH_SERVICE_TYPE_ID", Convert.ToString(ddlServiceType.SelectedValue));

            //Add recipient info params here
            parms.Add("PRIOR_AUTH_PROFSAVE_Medicaid_Billing_Number", Convert.ToString(txtMedicaidBillingNumber.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_BIRTH_DATE", DOBnull);
            parms.Add("PRIOR_AUTH_PROFSAVE_FIRST_NAME", Convert.ToString(txtfrstmi2.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_MIDDLE_INITIAL", Convert.ToString(txtMiddleName.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_LAST_NAME", Convert.ToString(txtLastName2.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_PATRACKING_NUMBER", Convert.ToString(txtPatientTrckNum.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_GENDER", Convert.ToString(txtGender2.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_ADDRESS_Line1", Convert.ToString(txtAddress1.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_ADDRESS_Line2", Convert.ToString(lblAddress2.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_CONTACT_CITY", Convert.ToString(txtCity.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_CONTACT_STATE", Convert.ToString(txtState.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_CONTACT_ZIP", Convert.ToString(txtZipCode.Text));

            //parms.Add("PRIOR_AUTH_PROFESSIONALSAVE_CONTACT_EXT_ZIP", Convert.ToString(txtZipCode.Text));
            //Add Contact info params here
            parms.Add("PRIOR_AUTH_PROFSAVE_CONTACT_FIRST_NAME", Convert.ToString(txtContactName.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_CONTACT_LAST_NAME", Convert.ToString(txtContactLastName.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_CONTACT_NUMBER", Convert.ToString(new string(txtContactNumber.Text.Where(x => char.IsDigit(x)).ToArray())));
            parms.Add("PRIOR_AUTH_PROFSAVE_CONTACT_EXTENSION", Convert.ToString(txtExt.Text));

            //Add Service Info Params here
            var result = Regex.Match(txtpalceofservice.Text, @"\d+").Value;
            parms.Add("PRIOR_AUTH_PROFSAVE_PLACEOFSERVICE_ID", result == "" ? null : Convert.ToString(result));
            parms.Add("PRIOR_AUTH_PROFSAVE_ACCIDENT_DATE", AccidentDatenull);
            parms.Add("PRIOR_AUTH_PROFSAVE_LEVELSERVICE_ID", Convert.ToString(ddlLvlServiceInst.SelectedValue));
            parms.Add("PRIOR_AUTH_PROFSAVE_DATEONSET_ILLNESS", IllnessDatenull);
            parms.Add("PRIOR_AUTH_PROFSAVE_MENSTRUAL_PERIOD", LastMensDatenull);
            parms.Add("PRIOR_AUTH_PROFSAVE_PATIENT_EVENT_DATE", PatientEventDatenull);
            parms.Add("PRIOR_AUTH_PROFSAVE_ESTIMATE_DATEBIRTH", EstBirthDatenull);

            parms.Add("PRIOR_AUTH_DELAY_REASON_ID", Convert.ToString(ddlDelayedInst.SelectedValue));
            parms.Add("PRIOR_AUTH_PROFSAVE_ASSOCIATED_PA_NO", Convert.ToString(txtPANumInst.Text));

            //Add Service Provider Info Params here
            parms.Add("PRIOR_AUTH_PROFSAVE_SERVICE_PROVIDER_NPI", Convert.ToString(txtSPNPI.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_SERVICE_PROVIDER_Medicaid_ID", Convert.ToString(txtMedicaidID.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_SERVICE_PROVIDER_FIRST_NAME", Convert.ToString(lblSvcProviderFName.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_SERVICE_PROVIDER_LAST_NAME", Convert.ToString(lblSvcProviderLName.Text));

            //Add Ordering Provider Info Params here
            parms.Add("PRIOR_AUTH_PROFSAVE_ORDERING_PROVIDER_Medicaid_ID", Convert.ToString(txtOMID.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_ORDERING_PROVIDER_NPI", Convert.ToString(txtorderingprovidernpi.Text));
            parms.Add("PRIOR_AUTH_PROFSAVE_ORDERING_PROVIDER_FIRST_NAME", Convert.ToString(lblOrdProviderFName.Text)); //txtOPNAME.Text
            parms.Add("PRIOR_AUTH_PROFSAVE_ORDERING_PROVIDER_LAST_NAME", Convert.ToString(lblOrdProviderLName.Text)); //txtOPNAME.Text

            parms.Add("PRIOR_AUTH_PROFDIAGNOSIS_ID", lnlRecord);
            parms.Add("PRIOR_AUTH_PROFSERVICE_DETAIL_ID", lnlRecord);
            parms.Add("PRIOR_AUTH_PROF_NOTES", hdnProvNoteText.Value);
            parms.Add("PRIOR_AUTH_PROFATTACHMENT_ID", lnlRecord);

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
            parms.Add("LAST_MODIFIED_USER", id);
            parms.Add("Created_On_Date_Time", DateTime.Now);
            parms.Add("Created_By_User", id);

            parms.Add("PRIOR_AUTH_SUB_DESTINATION_PAYER_MCE_ID", txtDestinationpayerID.Value);

            int SaveId = PriorAuthHospitalController.SavePriorAuth(CON.PAClaimsType.Professional, parms);
        }
        catch (Exception ex)
        {
            throw CoreException.ThrowException(ex);
        }
        return status;
    }

    protected void lnkFaciltySrch_Click(object sender, EventArgs e)
    {

    }

    private int GetClaimTypeID()
    {
        int claimType = 0;
        switch (rblClaimType.SelectedValue)
        {
            case "dental":
                claimType = Convert.ToInt32(CON.ClaimsType.Dental);
                break;
            case "Professional":
                claimType = Convert.ToInt32(CON.ClaimsType.Professional);
                break;
            case "Institutional":
                claimType = Convert.ToInt32(CON.ClaimsType.Institutional);
                break;
        }
        return claimType;
    }

    protected void lnkDiagnosisSearch_Click1(object sender, EventArgs e)
    {
        this.LoadData(null);
        txtDiagnosisCodeSearch1.Text = "";
        txtDiagnosisCodeDescSearch1.Text = "";
        //SortablePagingGridView2.Visible = false;
        //SortablePagingGridView2.DataSource = null;
        //SortablePagingGridView2.DataBind();
        //SortablePagingGridView2.EmptyDataText = string.Empty;
        mpeDiagnosisSearch1.Show();
    }

    protected void btndiagnoseSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtDiagnosisCodeSearch1.Text) && string.IsNullOrWhiteSpace(txtDiagnosisCodeDescSearch1.Text))
            {
                lbldiagnosiscodeError.Visible = true;
                lbldiagnosiscodeError.Text = "Either diagnosis code or description is required";
                mpeDiagnosisSearch1.Show();
                return;
            }
            else
            {
                lbldiagnosiscodeError.Visible = false;
                lbldiagnosiscodeError.Text = "";
                this.gvClaimDiagnosisSearch.CurrentPageIndex = 0;
                BindClaimDiagnosis(txtDiagnosisCodeSearch1.Text.Trim(), lblICDVersion.Text, txtDiagnosisCodeDescSearch1.Text.Trim());
                mpeDiagnosisSearch1.Show();
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void txtFAdmissionDate_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblAdmissionErrMsg, txtFAdmissionDate, "Admission Date should be valid date");
        hdnModified.Value = "true";

        DateTime admissionDate = Convert.ToDateTime(txtFAdmissionDate.Text);
        if (admissionDate > DateTime.Now)
        {
            lblDateError.Visible = true;
            lblDateError.Text = "Select a valid smaller date than today";
        }
        else
        {
            lblDateError.Visible = false;
            lblDateError.Text = "";
        }
        if (valerrormess.Visible)
        {
            if (!ReValidateData())
            {
                return;
            }
        }
    }

    protected void txtFDischargeDate_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblDischargeErrMsg, txtFDischargeDate, "Discharge date should be valid date");
    }

    protected void txtLstMensPeriod_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblMensErrMsg, txtLstMensPeriod, "Date of Last Menstrual Period should be valid date");
    }

    protected void txtEstBirthDate_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblEstErrMsg, txtEstBirthDate, "Estimated Date of Birth should be valid date");
    }

    protected void txtOnsetIllness_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblIllnessErrMsg, txtOnsetIllness, "Date of Onset of Illness should be valid date");
    }

    protected void txtAccidentDate_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblAccidentErrMsg, txtAccidentDate, "Accident Date should be valid date");
    }

    protected void txtAccDtService_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblAccidentServiceErrMsg, txtAccDtService, "Accident Date should be valid date");
    }

    protected void TextBox3_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblTextBoxErrMsg, TextBox3, "Date Of Patient Event should be valid date");
    }

    protected void txtProfOnsetIllness_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblOnsetIllnessErrMsg, txtProfOnsetIllness, "Date Of Onset of Illiness should be valid date");
    }

    protected void txtMenDtInst_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblMenErrMsg, txtMenDtInst, "Date Of Last Menstrual Period should be valid date");
    }

    protected void txtEstDOB_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblEstDOBErrMsg, txtEstDOB, "Estimated Date of Birth should be valid date");
    }
    protected void txtDiagnosisDate_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblDiagnosisDateErrMsg, txtDiagnosisDate, "DiagnosisDate should be valid date");
    }
    protected void txtReqFDOS_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblReqFDOSErrMsg, txtReqFDOS, "Requested FDOS should be valid date");
    }
    protected void txtDentalReqFDOS_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblDentalReqFDOSErrMsg, txtDentalReqFDOS, "Requested FDOS should be valid date");
    }
    protected void txtProfessionalReqFDOS_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblProfessionalReqFDOSErrMsg, txtProfessionalReqFDOS, "Requested FDOS should be valid date");
    }
    protected void txtProfessionalReqTDOS_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblProfessionalReqTDOSErrMsg, txtProfessionalReqTDOS, "Requested TDOS should be valid date");
    }
    protected void txtAdmissionDate_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblAdmissionDateErrMsg, txtAdmissionDate, "Admission Date should be valid date");
    }
    private void DateValidation(Label label, TextBox txtDate, string errMsg)
    {
        try
        {
            hdnModified.Value = "true";
            label.Text = string.Empty;

            if (!string.IsNullOrEmpty(txtDate.Text.Trim()))
            {
                DateTime dtTemp;
                if (!DateTime.TryParseExact(txtDate.Text, "MM/dd/yyyy", null, DateTimeStyles.None, out dtTemp))
                {
                    label.Visible = true;
                    label.Text = errMsg;

                    return;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at DateValidation method", ex);
        }
    }

    protected void txtReqTDOS_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblReqTDOSErrMsg, txtReqTDOS, "Requested TDOS should be valid date");
    }
    protected void txtDentalReqTDOS_TextChanged(object sender, EventArgs e)
    {
        DateValidation(lblDentalReqTDOSErrMsg, txtDentalReqTDOS, "Requested TDOS should be valid date");
    }

    protected void txtLnDiagnosisCode_TextChanged(object sender, EventArgs e)
    {
        hdnModified.Value = "true";
        lblIcdVersionError.Text = "";
        if (!string.IsNullOrEmpty(txtLnDiagnosisCode.Text))
        {
            // var dsICD = LookupTableController.GetICDDiagnosisCode(txtLnDiagnosisCode.Text.Trim(), lblICDVersion.Text, "");

            var dsICD = LookupTableController.GetICDDiagnosis(txtLnDiagnosisCode.Text.Trim().ToUpper(), lblICDVersion.Text, "", true);

            if (dsICD != null && Helper.HasRows(dsICD))
            {
                var dtICD = dsICD.Tables[0];
                dtICD = dtICD.Select("ICD10Diag <> ''").CopyToDataTable();
                if (dtICD.Rows.Count > 0)
                {
                    DataRow row = dtICD.Rows[0];
                    lblICDVersion.Text = lblICDVersion.Text;
                    txtDiagnosisCodeDescription.Text = row[2] != null ? Convert.ToString(row[2]) : "";
                    lblIcdVersionError.Text = "";
                    txtLnDiagnosisCode.Text = txtLnDiagnosisCode.Text.Trim().ToUpper();
                }
            }
            else
            {
                lblIcdVersionError.Text = "Incorrect Diagnosis Code";
                txtDiagnosisCodeDescription.Text = "";
            }
        }
        else
        {
            lblICDVersion.Text = "ICD 10";
        }
    }

    public DataTable DynamicProcedureCodeSearchData(string procCode)
    {
        string logHeader = string.Format("SubmitPriorAuthorization -> RefreshProcedureCodeSearchData()");
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
        DataTable dt = null;
        try
        {
            DataTable dataSet = LookupTableController.GetProcedureCodeServiceDetail(procCode, "", true);
            if (dataSet != null && dataSet.Rows.Count > 0)
                dt = dataSet;
            else
                dt = new DataTable();
        }
        catch (Exception ex)
        {
            throw new Exception("Error at DynamicProcedureCodeSearchData method", ex);
        }
        return dt;
    }

    public DataTable DynamicRevenueCodeSearchData(string procCode)
    {
        string logHeader = string.Format("SubmitPriorAuthorization -> RefreshProcedureCodeSearchData()");
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
        DataTable dt = null;
        try
        {
            DataSet dataSet = LookupTableController.GetRevenueCodePopupSearch(procCode, "", true);
            if (dataSet != null && dataSet.Tables.Count > 0)
                dt = dataSet.Tables[0];
            else
                dt = new DataTable();
        }
        catch (Exception ex)
        {
            throw new Exception("Error at DynamicProcedureCodeSearchData method", ex);
        }
        return dt;
    }

    protected void txtServicecode_TextChanged(object sender, EventArgs e)
    {
        hdnModified.Value = "true";
        DataTable dt = DynamicRevenueCodeSearchData(txtServicecode.Text);
        if (dt != null && dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            string code = Convert.ToString(dr["PRIOR_AUTH_REVENUE_CODE_MMIS"]);
            lblServDetailsrevenueCode.Visible = false;
            lblServDetailsrevenueCode.Text = "";
        }
        else
        {
            if (!string.IsNullOrEmpty(txtServicecode.Text) && !string.IsNullOrWhiteSpace(txtServicecode.Text))
            {
                lblServDetailsrevenueCode.Visible = true;
                lblServDetailsrevenueCode.Text = "Revenue code is invalid";
            }
        }
    }

    protected void txtLnProcedureCode_TextChanged(object sender, EventArgs e)
    {
        hdnModified.Value = "true";
        DataTable dt = DynamicProcedureCodeSearchData(txtLnProcedureCode.Text);
        if (dt != null && dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            string code = Convert.ToString(dr["PRIOR_AUTH_PROCEDURE_CODE_MMIS"]);
            lblServDetailsProcCode.Visible = false;
            lblServDetailsProcCode.Text = "";
        }
        else
        {
            lblServDetailsProcCode.Visible = true;
            lblServDetailsProcCode.Text = "Procedure code is invalid";
        }
    }

    protected void txtDentalSDProcCode_TextChanged(object sender, EventArgs e)
    {
        hdnModified.Value = "true";
        DataTable dt = DynamicProcedureCodeSearchData(txtDentalSDProcCode.Text);
        if (dt != null && dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            string code = Convert.ToString(dr["PRIOR_AUTH_PROCEDURE_CODE_MMIS"]);
            lblDentalProcMessage.Visible = false;
            lblDentalProcMessage.Text = "";
        }
        else
        {
            lblDentalProcMessage.Visible = true;
            lblDentalProcMessage.Text = "Procedure code is invalid";
        }
    }

    protected void txtProfessionalSDProcCode_TextChanged(object sender, EventArgs e)
    {
        hdnModified.Value = "true";
        DataTable dt = DynamicProcedureCodeSearchData(txtProfessionalSDProcCode.Text);
        if (dt != null && dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            string code = Convert.ToString(dr["PRIOR_AUTH_PROCEDURE_CODE_MMIS"]);
            lblProfProcMessage.Visible = false;
            lblProfProcMessage.Text = "";
        }
        else
        {
            lblProfProcMessage.Visible = true;
            lblProfProcMessage.Text = "Procedure code is invalid";
        }
    }

    //Recursively get all the formControls underneath the current one, be it Page, UserControl or whatever.
    public List<Control> lstControl = new List<Control>();
    protected void GetAllControlsInWebPage(Control oControl)
    {
        foreach (Control childControl in oControl.Controls)
        {
            lstControl.Add(childControl); //lstControl - Global variable
            if (childControl.HasControls())
                GetAllControlsInWebPage(childControl);
        }
    }

    private List<Control> getControls() // Add all Lables to a list
    {
        List<Control> lLabels = new List<Control>();

        foreach (Control oControl in Page.Controls)
        {
            GetAllControlsInWebPage(oControl);
        }
        foreach (Control oControl in lstControl)
        {
            if (oControl.GetType() == typeof(TextBox))
            {
                lLabels.Add((TextBox)oControl);
            }
            else if (oControl.GetType() == typeof(DropDownList))
            {
                lLabels.Add((DropDownList)oControl);
            }
            else if (oControl.GetType() == typeof(CheckBox))
            {
                lLabels.Add((CheckBox)oControl);
            }
            else if (oControl.GetType() == typeof(CheckBoxList))
            {
                lLabels.Add((CheckBoxList)oControl);
            }
            else if (oControl.GetType() == typeof(RadioButton))
            {
                lLabels.Add((RadioButton)oControl);
            }
            else if (oControl.GetType() == typeof(RadioButtonList))
            {
                lLabels.Add((RadioButtonList)oControl);
            }
        }

        return lLabels;
    }

    protected void ClearAllPageControls()
    {
        lstControl = getControls();

        foreach (Control oControl in lstControl)
        {

            if (oControl.GetType() == typeof(TextBox))
            {
                var txtBox = ((TextBox)(oControl));
                txtBox.BackColor = Color.White;
                txtBox.Text = "";
                txtBox.Attributes.Clear();
            }
            //check for all the DropDownList controls on the page and reset it to the very first item e.g. "-- Select One --"
            else if (oControl.GetType() == typeof(DropDownList))
            {
                if (((DropDownList)(oControl)).DataSource != null || ((DropDownList)(oControl)).Items.Count > 0)
                    ((DropDownList)(oControl)).SelectedIndex = 0;
                ((DropDownList)(oControl)).Attributes.Add("style", "background-color:white");
                ((DropDownList)(oControl)).Width = 200;
            }
            //check for all the CheckBox controls on the page and unchecked the selection
            else if (oControl.GetType() == typeof(CheckBox))
            {
                ((CheckBox)(oControl)).Checked = false;
            }
            //check for all the CheckBoxList controls on the page and unchecked all the selections
            else if (oControl.GetType() == typeof(CheckBoxList))
            {
                ((CheckBoxList)(oControl)).ClearSelection();
            }
            //check for all the RadioButton controls on the page and unchecked the selection
            else if (oControl.GetType() == typeof(RadioButton))
            {
                ((RadioButton)(oControl)).Checked = false;
            }
            //check for all the RadioButtonList controls on the page and unchecked the selection
            else if (oControl.GetType() == typeof(RadioButtonList))
            {
                ((RadioButtonList)(oControl)).ClearSelection();
                ((RadioButtonList)(oControl)).Enabled = true;
                ((RadioButtonList)(oControl)).SelectedIndex = -1;
            }
        }
        if (ddlAuthorization.SelectedValue == "1")
            pnlsepDentalServiceDetail.Visible = true;
        lblDOBErrMsg.Text = string.Empty;
        lblMBErrorMSG.Text = string.Empty;
        // lblDOBErrMsg.Visible = false;
        //lblMBErrorMSG.Visible = false;
        lblInstUploadErrMsg.Text = string.Empty;
        lblInstDocTypeErrMsg.Text = string.Empty;
        lblDentalUploadErrMsg.Text = string.Empty;
        lblDentalDocTypeErrMsg.Text = string.Empty;
        lblAdmissionErrMsg.Text = string.Empty;
        lblDischargeErrMsg.Text = string.Empty;
        lblMensErrMsg.Text = string.Empty;
        lblEstErrMsg.Text = string.Empty;
        lblIllnessErrMsg.Text = string.Empty;
        lblAccidentErrMsg.Text = string.Empty;
        lblAccidentServiceErrMsg.Text = string.Empty;
        lblTextBoxErrMsg.Text = string.Empty;
        lblOnsetIllnessErrMsg.Text = string.Empty;
        lblMenErrMsg.Text = string.Empty;
        lblEstDOBErrMsg.Text = string.Empty;
        lblPlaceofErrorSearch.Text = string.Empty;
        errProviderNPI.Text = string.Empty;
        lblIcdVersionError.Text = "";

        txtMedicaidID.Text = "";
        lblSvcProviderFName.Text = "";
        lblSvcProviderLName.Text = "";

        txtOMID.Text = "";
        lblOrdProviderFName.Text = "";
        lblOrdProviderLName.Text = "";

        hdlblSvcProviderFName.Value = "";
        hdlblSvcProviderLName.Value = "";
        hdlblOrdProviderFName.Value = "";
        hdlblOrdProviderLName.Value = "";
        hdtxtMedicaidID.Value = "";
        hdtxtOMID.Value = "";

    }

    private DataTable provHeader(Guid guid, string reqType)
    {

        DataTable objDT = new DataTable("PriorAuthHeader");
        try
        {
            string MedicaidNumber = this.WorkflowPage.MedicaidID;
            string NPI = string.Empty;
            string REG_ID = string.Empty;
            string paSCCode = string.Empty;

            DataSet ds = spa.SelectProviderByGRPMedicaidID(MedicaidNumber);

            DataSet dataSetSC = _spa.GetServiceCodeType();
            if (dataSetSC != null)
            {
                DataTable dt = dataSetSC.Tables[0];
                if (!string.IsNullOrEmpty(ddlServiceType.SelectedValue) && (dt != null && dt.Rows.Count > 0))
                {
                    DataRow dr = dt.Select("PRIOR_AUTH_SERVICE_TYPE_ID = " + ddlServiceType.SelectedValue.Trim()).FirstOrDefault();
                    if (dr != null)
                    {
                        if (dr.Table.Columns.Contains("PRIOR_AUTH_SERVICE_TYPE_CODE"))
                        {
                            paSCCode = dr["PRIOR_AUTH_SERVICE_TYPE_CODE"].ToString();
                        }
                    }
                }
            }


            DataTable provInfo = Helper.HasRows(ds) ? ds.Tables[0] : null;
            provInfo.Columns.Add("PriorAuthType", typeof(System.String));
            provInfo.Columns.Add("PriorAuthDestinationPayer", typeof(System.String));
            provInfo.Columns.Add("PriorAuthAssignment", typeof(System.String));
            provInfo.Columns.Add("PriorAuthServiceType", typeof(System.String));
            provInfo.Columns.Add("PriorAuthProviderNotes", typeof(System.String));
            provInfo.Columns.Add("PriorAuthDestinationPayerID", typeof(System.String));
            provInfo.Columns.Add("PriorAuthDestinationPayerMCEID", typeof(System.String));
            provInfo.Columns.Add("PARequestType", typeof(System.String));
            provInfo.Columns.Add("PANumber", typeof(System.String));
            if (Helper.HasRows(provInfo))
            {
                foreach (DataRow row in provInfo.Rows)
                {
                    row["PriorAuthType"] = rblClaimType.SelectedValue;
                    string[] tokens = ddlSubCapitaPayerIDs.SelectedItem.Text.Split(new string[] { " - " }, StringSplitOptions.None);
                    string dstSubPayerName = string.Empty;
                    if (tokens.Count() > 1)
                    {
                        dstSubPayerName = tokens[1].Trim();
                    }

                    row["PriorAuthDestinationPayer"] = dstSubPayerName;
                    row["PriorAuthAssignment"] = ddlAssignment.SelectedValue;
                    row["PriorAuthServiceType"] = paSCCode;
                    row["PriorAuthProviderNotes"] = txtProviderNotes.Text;

                    row["PriorAuthDestinationPayerID"] = txtDestinationpayerID.Value;

                    row["PARequestType"] = reqType;

                    DataTable destinationPayers = (DataTable)Session["DestinationPayer"];

                    string filter = "PRIOR_AUTH_DESTINATION_PAYER_ID = '" + Convert.ToString(ddlAuthorization.SelectedValue) + "'";

                    DataRow[] dr = destinationPayers.Select(filter);
                    string PriorAuthDestinationPayerID = (string)dr[0]["MCE_ID"];
                    row["PriorAuthDestinationPayerMCEID"] = PriorAuthDestinationPayerID;
                    row["PANumber"] = txtPANumber2.Text;
                }
            }
            objDT = provInfo.Copy();
            objDT.TableName = "PriorAuthHeader";
            return objDT;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            throw ex;
        }
    }

    private DataTable provRecipientInfo(Guid guid)
    {
        DataTable objDT = new DataTable("PriorAuthRecipientInfo");
        try
        {
            DataTable provInfo = new DataTable();
            provInfo.Columns.Add("MedicaidBillingNumber", typeof(System.String));
            provInfo.Columns.Add("LastName", typeof(System.String));
            provInfo.Columns.Add("FirstName", typeof(System.String));
            provInfo.Columns.Add("MiddleName", typeof(System.String));
            provInfo.Columns.Add("DOB", typeof(System.String));
            provInfo.Columns.Add("PatientTrackingNumber", typeof(System.String));
            provInfo.Columns.Add("Gender", typeof(System.String));
            provInfo.Columns.Add("AddressLine1", typeof(System.String));
            provInfo.Columns.Add("AddressLine2", typeof(System.String));
            provInfo.Columns.Add("City", typeof(System.String));
            provInfo.Columns.Add("State", typeof(System.String));
            provInfo.Columns.Add("ZipCode", typeof(System.String));
            DataRow rowDR = provInfo.NewRow();
            if (!Helper.HasRows(provInfo))
            {
                provInfo.Rows.Add(rowDR);
            }
            foreach (DataRow row in provInfo.Rows)
            {
                row["MedicaidBillingNumber"] = txtMedicaidBillingNumber.Text;
                row["LastName"] = txtLastName2.Text;
                row["FirstName"] = txtfrstmi2.Text;
                row["MiddleName"] = txtMiddleName.Text;

                row["DOB"] = txtBirthDate.Text;
                row["PatientTrackingNumber"] = txtPatientTrckNum.Text;
                row["Gender"] = txtGender2.Text;
                row["AddressLine1"] = txtAddress1.Text;

                row["AddressLine2"] = lblAddress2.Text;
                row["City"] = txtCity.Text;
                row["State"] = txtState.Text;
                row["ZipCode"] = txtZipCode.Text;

            }
            objDT = provInfo.Copy();
            objDT.TableName = "PriorAuthRecipientInfo";
            return objDT;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            throw ex;
        }
    }

    private DataTable provRequestorContactInfo(Guid guid)
    {
        DataTable objDT = new DataTable("PriorAuthRequestorContactInfo");
        try
        {
            DataTable provInfo = new DataTable();
            provInfo.Columns.Add("ContactFirstName", typeof(System.String));
            provInfo.Columns.Add("ContactLastName", typeof(System.String));
            provInfo.Columns.Add("ContactContactNumber", typeof(System.String));
            provInfo.Columns.Add("ContactContactExt", typeof(System.String));
            DataRow rowDR = provInfo.NewRow();
            if (!Helper.HasRows(provInfo))
            {
                provInfo.Rows.Add(rowDR);
            }
            foreach (DataRow row in provInfo.Rows)
            {
                row["ContactFirstName"] = txtContactName.Text;
                row["ContactLastName"] = txtContactLastName.Text;
                row["ContactContactNumber"] = txtContactNumber.Text;
                row["ContactContactExt"] = txtExt.Text;
            }
            objDT = provInfo.Copy();
            objDT.TableName = "PriorAuthRequestorContactInfo";
            return objDT;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            throw ex;
        }
    }

    private DataTable provDentalServiceInformation(Guid guid)
    {
        string mmisDischargeStatus = string.Empty;
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        try
        {
            DataSet dataSet = _spa.GetDischargeStatus();
            if (dataSet != null)
            {
                DataTable dt = dataSet.Tables[0];
                if (!string.IsNullOrEmpty(ddlDischargeStatus.SelectedValue))
                {
                    DataRow dr = dt.Select("PRIOR_AUTH_DISCHARGE_STATUS_MMIS = " + ddlDischargeStatus.SelectedValue).First();

                    if (dr.Table.Columns.Contains("PRIOR_AUTH_DISCHARGE_STATUS_MMIS"))
                    {
                        mmisDischargeStatus = dr["PRIOR_AUTH_DISCHARGE_STATUS_MMIS"].ToString();
                    }
                }
            }
            DataTable objDT = new DataTable("PriorAuthServiceInformation");
            DataTable provInfo = new DataTable();
            provInfo.Columns.Add("PlaceofService", typeof(System.String));
            provInfo.Columns.Add("FacilityType", typeof(System.String));
            provInfo.Columns.Add("AdmissionDate", typeof(System.String));
            provInfo.Columns.Add("DischargeDate", typeof(System.String));
            provInfo.Columns.Add("DischargeStatus", typeof(System.String));
            provInfo.Columns.Add("DateOfLastMenstrualPeriod", typeof(System.String));
            provInfo.Columns.Add("EstimatedDateOfBirth", typeof(System.String));
            provInfo.Columns.Add("DateOfOnsetOfIllness", typeof(System.String));
            provInfo.Columns.Add("AccidentDate", typeof(System.String));
            provInfo.Columns.Add("LevelOfService", typeof(System.String));
            provInfo.Columns.Add("AdmissionType", typeof(System.String));
            provInfo.Columns.Add("AdmissionSource", typeof(System.String));
            provInfo.Columns.Add("DelayReason", typeof(System.String));
            provInfo.Columns.Add("AssociatedPANo", typeof(System.String));
            provInfo.Columns.Add("DateOfPatientEvent", typeof(System.String));
            DataRow rowDR = provInfo.NewRow();
            if (!Helper.HasRows(provInfo))
            {
                provInfo.Rows.Add(rowDR);
            }
            foreach (DataRow row in provInfo.Rows)
            {
                string strPOS = string.Empty;
                if (txtpalceofservice.Text.Contains("-"))
                {
                    string[] PlaceofService = txtpalceofservice.Text.Trim().Split('-');
                    if (PlaceofService.Count() > 1)
                    {
                        strPOS = PlaceofService[0].Trim();
                    }
                }
                row["PlaceofService"] = strPOS;
                row["FacilityType"] = txtfacilityType.Text;
                row["AdmissionDate"] = txtFAdmissionDate.Text;
                row["DischargeDate"] = txtFDischargeDate.Text;
                row["DischargeStatus"] = mmisDischargeStatus;
                row["DateOfLastMenstrualPeriod"] = txtMenDtInst.Text;
                row["EstimatedDateOfBirth"] = txtEstDOB.Text;
                row["DateOfOnsetOfIllness"] = txtProfOnsetIllness.Text;
                row["AccidentDate"] = txtAccDtService.Text;
                row["LevelOfService"] = ddlLvlServiceInst.SelectedValue;
                row["AdmissionType"] = ddlAdminType.SelectedValue;
                row["AdmissionSource"] = ddlAdminSrc.SelectedValue;
                row["DelayReason"] = ddlDelayedInst.SelectedValue;
                row["AssociatedPANo"] = txtPANumInst.Text;
                row["DateOfPatientEvent"] = TextBox3.Text;
            }
            objDT = provInfo.Copy();
            objDT.TableName = "PriorAuthServiceInformation";
            return objDT;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            throw ex;
        }
    }

    private DataTable provProfServiceInformation(Guid guid)
    {
        string mmisDischargeStatus = string.Empty;
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        try
        {
            DataSet dataSet = _spa.GetDischargeStatus();
            if (dataSet != null)
            {
                DataTable dt = dataSet.Tables[0];
                if (!string.IsNullOrEmpty(ddlDischargeStatus.SelectedValue))
                {
                    DataRow dr = dt.Select("PRIOR_AUTH_DISCHARGE_STATUS_MMIS = " + ddlDischargeStatus.SelectedValue).First();

                    if (dr.Table.Columns.Contains("PRIOR_AUTH_DISCHARGE_STATUS_MMIS"))
                    {
                        mmisDischargeStatus = dr["PRIOR_AUTH_DISCHARGE_STATUS_MMIS"].ToString();
                    }
                }
            }
            DataTable objDT = new DataTable("PriorAuthServiceInformation");
            DataTable provInfo = new DataTable();
            provInfo.Columns.Add("PlaceofService", typeof(System.String));
            provInfo.Columns.Add("FacilityType", typeof(System.String));
            provInfo.Columns.Add("AdmissionDate", typeof(System.String));
            provInfo.Columns.Add("DischargeDate", typeof(System.String));
            provInfo.Columns.Add("DischargeStatus", typeof(System.String));
            provInfo.Columns.Add("DateOfLastMenstrualPeriod", typeof(System.String));
            provInfo.Columns.Add("EstimatedDateOfBirth", typeof(System.String));
            provInfo.Columns.Add("DateOfOnsetOfIllness", typeof(System.String));
            provInfo.Columns.Add("AccidentDate", typeof(System.String));
            provInfo.Columns.Add("LevelOfService", typeof(System.String));
            provInfo.Columns.Add("AdmissionType", typeof(System.String));
            provInfo.Columns.Add("AdmissionSource", typeof(System.String));
            provInfo.Columns.Add("DelayReason", typeof(System.String));
            provInfo.Columns.Add("AssociatedPANo", typeof(System.String));
            provInfo.Columns.Add("DateOfPatientEvent", typeof(System.String));
            DataRow rowDR = provInfo.NewRow();
            if (!Helper.HasRows(provInfo))
            {
                provInfo.Rows.Add(rowDR);
            }
            foreach (DataRow row in provInfo.Rows)
            {
                string strPOS = string.Empty;
                if (txtpalceofservice.Text.Contains("-"))
                {
                    string[] PlaceofService = txtpalceofservice.Text.Trim().Split('-');
                    if (PlaceofService.Count() > 1)
                    {
                        strPOS = PlaceofService[0].Trim();
                    }
                }
                row["PlaceofService"] = strPOS;
                row["FacilityType"] = txtfacilityType.Text;
                row["AdmissionDate"] = txtFAdmissionDate.Text;
                row["DischargeDate"] = txtFDischargeDate.Text;
                row["DischargeStatus"] = mmisDischargeStatus;
                row["DateOfLastMenstrualPeriod"] = txtMenDtInst.Text;
                row["EstimatedDateOfBirth"] = txtEstDOB.Text;
                row["DateOfOnsetOfIllness"] = txtProfOnsetIllness.Text;
                row["AccidentDate"] = txtAccDtService.Text;
                row["LevelOfService"] = ddlLvlServiceInst.SelectedValue;
                row["AdmissionType"] = ddlAdminType.SelectedValue;
                row["AdmissionSource"] = ddlAdminSrc.SelectedValue;
                row["DelayReason"] = ddlDelayedInst.SelectedValue;
                if (rblClaimType.SelectedValue.ToUpper().Equals("INSTITUTIONAL"))
                {
                    row["AssociatedPANo"] = txtPaNum.Text;
                }
                else
                    row["AssociatedPANo"] = txtPANumInst.Text;
                row["DateOfPatientEvent"] = TextBox3.Text;
            }
            objDT = provInfo.Copy();
            objDT.TableName = "PriorAuthServiceInformation";
            return objDT;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            throw ex;
        }


    }

    private DataTable provInstServiceInformation(Guid guid)
    {
        string mmisDischargeStatus = string.Empty;
        try
        {
            if (!string.IsNullOrEmpty(ddlDischargeStatus.SelectedValue))
            {
                mmisDischargeStatus = ddlDischargeStatus.SelectedValue.Trim();
            }
            DataTable objDT = new DataTable("PriorAuthServiceInformation");
            DataTable provInfo = new DataTable();
            provInfo.Columns.Add("FacilityType", typeof(System.String));
            provInfo.Columns.Add("AdmissionDate", typeof(System.String));
            provInfo.Columns.Add("DischargeDate", typeof(System.String));
            provInfo.Columns.Add("DischargeStatus", typeof(System.String));
            provInfo.Columns.Add("DateOfLastMenstrualPeriod", typeof(System.String));
            provInfo.Columns.Add("EstimatedDateOfBirth", typeof(System.String));
            provInfo.Columns.Add("DateOfOnsetOfIllness", typeof(System.String));
            provInfo.Columns.Add("AccidentDate", typeof(System.String));
            provInfo.Columns.Add("LevelOfService", typeof(System.String));
            provInfo.Columns.Add("AdmissionType", typeof(System.String));
            provInfo.Columns.Add("AdmissionSource", typeof(System.String));
            provInfo.Columns.Add("DelayReason", typeof(System.String));
            provInfo.Columns.Add("AssociatedPANo", typeof(System.String));
            provInfo.Columns.Add("DateOfPatientEvent", typeof(System.String));
            DataRow rowDR = provInfo.NewRow();
            if (!Helper.HasRows(provInfo))
            {
                provInfo.Rows.Add(rowDR);
            }
            foreach (DataRow row in provInfo.Rows)
            {
                row["FacilityType"] = txtfacilityType.Text;
                row["AdmissionDate"] = txtFAdmissionDate.Text;
                row["DischargeDate"] = txtFDischargeDate.Text;
                row["DischargeStatus"] = mmisDischargeStatus;
                row["DateOfLastMenstrualPeriod"] = txtLstMensPeriod.Text;
                row["EstimatedDateOfBirth"] = txtEstBirthDate.Text;
                row["DateOfOnsetOfIllness"] = txtOnsetIllness.Text;
                row["AccidentDate"] = txtAccidentDate.Text;
                row["LevelOfService"] = ddlLevelService.SelectedValue;
                row["AdmissionType"] = ddlAdminType.SelectedValue;
                row["AdmissionSource"] = hdnAdminSrc.Value;
                row["DelayReason"] = ddlDlyReason.SelectedValue;
                row["AssociatedPANo"] = txtPaNum.Text;
                row["DateOfPatientEvent"] = TextBox3.Text;
            }
            objDT = provInfo.Copy();
            objDT.TableName = "PriorAuthServiceInformation";
            return objDT;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            throw ex;
        }
    }

    private DataTable provServicingProvider(Guid guid)
    {
        DataTable objDT = new DataTable("PriorAuthServicingProvider");
        try
        {
            DataTable provInfo = new DataTable();
            provInfo.Columns.Add("ServiceProvider", typeof(System.String));
            provInfo.Columns.Add("MEDICAID_ID", typeof(System.String));
            provInfo.Columns.Add("LastName", typeof(System.String));
            provInfo.Columns.Add("FirstName", typeof(System.String));
            DataRow rowDR = provInfo.NewRow();
            if (!Helper.HasRows(provInfo))
            {
                provInfo.Rows.Add(rowDR);
            }
            foreach (DataRow row in provInfo.Rows)
            {
                row["ServiceProvider"] = txtSPNPI.Text;
                row["MEDICAID_ID"] = txtMedicaidID.Text;
                row["LastName"] = lblSvcProviderLName.Text;
                row["FirstName"] = lblSvcProviderFName.Text;
            }
            objDT = provInfo.Copy();
            objDT.TableName = "PriorAuthServicingProvider";
            return objDT;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            throw ex;
        }
    }

    private DataTable provOrderingProvider(Guid guid)
    {
        DataTable objDT = new DataTable("PriorAuthOrderingProvider");
        try
        {
            DataTable provInfo = new DataTable();
            provInfo.Columns.Add("OrderingProvider", typeof(System.String));
            provInfo.Columns.Add("MEDICAID_ID", typeof(System.String));
            provInfo.Columns.Add("LastName", typeof(System.String));
            provInfo.Columns.Add("FirstName", typeof(System.String));
            DataRow rowDR = provInfo.NewRow();
            if (!Helper.HasRows(provInfo))
            {
                provInfo.Rows.Add(rowDR);
            }
            foreach (DataRow row in provInfo.Rows)
            {
                row["OrderingProvider"] = txtorderingprovidernpi.Text;
                row["MEDICAID_ID"] = txtOMID.Text;
                row["LastName"] = lblOrdProviderLName.Text;
                row["FirstName"] = lblOrdProviderFName.Text;
            }
            objDT = provInfo.Copy();
            objDT.TableName = "PriorAuthOrderingProvider";
            return objDT;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            throw ex;
        }
    }

    private DataTable provDiagnosisProvider(Guid guid)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
        parms.Add("MedicaidID", MedicaidNumber);
        parms.Add("LINK_SECTIONS", SaveCodeLNK);
        DataSet dss = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_DIAGNOSIS", parms);
        try
        {
            DataTable dataTable = new DataTable();
            if (dss != null && dss.Tables.Count > 0)
                dataTable = dss.Tables[0];
            DataTable objDT = new DataTable("PriorAuthDiagnosisProvider");
            objDT = dataTable.Copy();
            objDT.TableName = "PriorAuthDiagnosisProvider";
            return objDT;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            throw ex;
        }
    }

    private DataTable provServiceDetails(Guid guid)
    {
        try
        {
            DataSet dss = new DataSet();

            switch (rblClaimType.SelectedValue.ToUpper())
            {
                case "DENTAL":
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
                    parms.Add("LINK_SECTIONS", SaveCodeLNK);
                    dss = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                    break;
                case "PROFESSIONAL":
                    Dictionary<string, string> parms2 = new Dictionary<string, string>();
                    parms2.Add("PRIOR_AUTH_TYPE", returnPATypeID());
                    parms2.Add("LINK_SECTIONS", SaveCodeLNK);
                    dss = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms2);
                    break;
                case "INSTITUTIONAL":
                    Dictionary<string, string> parms3 = new Dictionary<string, string>();
                    parms3.Add("PRIOR_AUTH_TYPE", returnPATypeID());
                    parms3.Add("LINK_SECTIONS", SaveCodeLNK);
                    dss = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms3);
                    break;
            }
            DataTable dataTable = new DataTable();
            if (dss != null && dss.Tables.Count > 0)
                dataTable = dss.Tables[0];

            DataTable objDT = new DataTable("PriorAuthServiceDetails");
            objDT = dataTable.Copy();
            objDT.TableName = "PriorAuthServiceDetails";

            //Remove deleted rows from data table before sending it to FI
            try
            {
                if (objDT != null && objDT.Rows != null && objDT.Rows.Count > 0)
                {
                    if (objDT.Columns.IndexOf("RowState") > 0)
                    {
                        DataRow[] dtRows = objDT.Select("RowState='deleted'");
                        if (dtRows != null && dtRows.Count() > 0)
                        {
                            foreach (DataRow dr in dtRows)
                            {
                                objDT.Rows.Remove(dr);
                            }

                            objDT.AcceptChanges();
                        }
                    }
                }
            }
            catch (Exception ex1)
            {
                string logNumber = CreateAndReturnLogThreadNumber(ex1, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
                IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex1.Message + " " + ex1.StackTrace);
            }
            return objDT;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            throw ex;
        }
    }


    private DataTable provProviderNotes(Guid guid)
    {
        DataTable objDT = new DataTable("PriorAuthProviderNotes");
        try
        {
            DataTable provInfo = new DataTable();
            provInfo.Columns.Add("MESSAGE", typeof(System.String));
            DataRow rowDR = provInfo.NewRow();
            if (!Helper.HasRows(provInfo))
            {
                provInfo.Rows.Add(rowDR);
            }

            //OHPNM-9763
            //This check is added to make sure the provider note text variable has updated value always.
            string providerTxt = string.IsNullOrEmpty(hdnProvNoteText.Value) ? txtProviderNotes.Text : hdnProvNoteText.Value;

            foreach (DataRow row in provInfo.Rows)
            {
                row["MESSAGE"] = providerTxt;
            }
            objDT = provInfo.Copy();
            objDT.TableName = "PriorAuthProviderNotes";
            return objDT;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            throw ex;
        }
    }

    private DataTable provPriorAuthAttachment(Guid guid)
    {
        DataTable objDT = new DataTable("PriorAuthAttachments");
        try
        {
            string MedicaidNumber = this.WorkflowPage.MedicaidID;

            if (string.IsNullOrEmpty(MedicaidNumber))
            {
                throw new InvalidOperationException("Medicaid ID cannot be null or empty.");
            }

            DataSet ds = new DataSet();
            ds = PriorAuthHospitalController.SelectPriorAuthAttachment(rblClaimType.SelectedValue, MedicaidNumber);
            DataTable provInfo = new DataTable();
            if (ds != null && ds.Tables.Count > 0 && Helper.HasRows(ds))
            {
                provInfo = ds.Tables[0];
            }
            else
            {
                List<PriorAuthAttachment> dentalAttachments = FetchAttachments();
                if (dentalAttachments != null)
                {
                    provInfo = Helper.ConvertToDataTable(dentalAttachments);
                }
                else
                {
                    provInfo = new DataTable();
                }
            }
            objDT = provInfo.Copy();
            objDT.TableName = "PriorAuthAttachments";
            return objDT;
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
            throw ;
        }
    }

    private DataTable provPriorRequestType(int transactionID, string reqType = "")
    {
        DataTable objDT = new DataTable("PriorAuthRequestType");
        DataTable provInfo = new DataTable();
        provInfo.Columns.Add("RequestType", typeof(System.String));
        provInfo.Columns.Add("transactionID", typeof(System.String));
        DataRow rowDR = provInfo.NewRow();
        if (!Helper.HasRows(provInfo))
        {
            provInfo.Rows.Add(rowDR);
        }
        foreach (DataRow row in provInfo.Rows)
        {
            if (reqType.Equals("3"))
            {
                row["RequestType"] = "01";
            }
            else
            {
                row["RequestType"] = "13";
            }
            row["transactionID"] = transactionID.ToString();
        }
        objDT = provInfo.Copy();
        objDT.TableName = "PriorAuthRequestType";
        return objDT;
    }

    private void makePriorAuthAddUpdateRequest(string reqType = "")
    {
        //unique Guid for improvment log/ for individual method
        var guid = Guid.NewGuid();
        string enableAttachment = Helper.GetAppSettingFromDB("enablePriorAuthAttachment", string.Empty);
        int logTransactionId = 0;
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }

        try
        {
            DataSet priorAuthDS = new DataSet();
            //section1 //table0 PriorAuthHeader
            priorAuthDS.Tables.Add(provHeader(guid, reqType));

            //section2 //table1 PriorAuthRecipientInfo
            priorAuthDS.Tables.Add(provRecipientInfo(guid));

            //section3 //table2 PriorAuthRequestorContactInfo
            priorAuthDS.Tables.Add(provRequestorContactInfo(guid));

            //section4 //table3 PriorAuthServiceInformation
            switch (rblClaimType.SelectedValue.ToUpper())
            {
                case "DENTAL":
                    priorAuthDS.Tables.Add(provDentalServiceInformation(guid));
                    break;
                case "PROFESSIONAL":
                    priorAuthDS.Tables.Add(provProfServiceInformation(guid));
                    break;
                case "INSTITUTIONAL":
                    priorAuthDS.Tables.Add(provInstServiceInformation(guid));
                    break;
            }

            //section5 //table4 PriorAuthServicingProvider
            priorAuthDS.Tables.Add(provServicingProvider(guid));

            //section6 //table5 PriorAuthOrderingProvider
            priorAuthDS.Tables.Add(provOrderingProvider(guid));

            //section7 //table6 PriorAuthDiagnosisScreen
            priorAuthDS.Tables.Add(provDiagnosisProvider(guid));

            //section8 //table7 PriorAuthServiceDetailScreen
            priorAuthDS.Tables.Add(provServiceDetails(guid));

            //section9 //table8 PriorAuthProviderNotes
            priorAuthDS.Tables.Add(provProviderNotes(guid));

            //section10 //table9 PriorAuthAttachments
            priorAuthDS.Tables.Add(provPriorAuthAttachment(guid));

            //section11 //table10 RequestType
            int AddUpdatePriorAuth = 4;
            int transactionID = InfoAccessController.InsertPASSTHROUGH_TRANSACTIONQUEUE(AddUpdatePriorAuth, DateTime.Now, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId));
            logTransactionId = transactionID;
            priorAuthDS.Tables.Add(provPriorRequestType(transactionID, reqType));
            string transactionResponse = string.Empty;
            PriorAuthServiceReqRes pa = new PriorAuthServiceReqRes();

            //select TransactionType
            string destPayerID = txtDestinationpayerID.Value;
            if (destPayerIDArrayFI.Contains(destPayerID))
            {
                transactionResponse = pa.makePriorAuthAddUpdateRequest(transactionID, priorAuthDS, CON.PriorAuthSubscriber.FI);
            }
            else if (destPayerIDArrayEDI.Contains(destPayerID))
            {
                transactionResponse = pa.makePriorAuthAddUpdateRequest(transactionID, priorAuthDS, CON.PriorAuthSubscriber.EDI);
            }
            else
            {
                transactionResponse = CON.TransactionResult.TransactionFailed;
            }

            string xmlResponse = string.Empty;
            if (enableAttachment.Equals("true"))
            {
                ProcessAttachmentControlFile(transactionID);
            }
            if (transactionResponse.Equals(CON.TransactionResult.TransactionPassed))
            {
                xmlResponse = InfoAccessController.GetResponsePayloadByPassThroughTransactionID(transactionID);

                //Load the XmlDocument then get element tag name values from the XmlDocument.
                XmlDocument resXmlDoc = new XmlDocument();
                resXmlDoc.LoadXml(xmlResponse);

                //PANUmber and PAStatus will only come in response for FI not EDI
                //OHPNM-8564
                if (destPayerIDArrayFI.Contains(destPayerID))
                {
                    string priorAuthNumber = string.Empty;
                    string priorAuthStatus = string.Empty;
                    if (resXmlDoc != null && resXmlDoc.GetElementsByTagName("PriorAuthNumber").Count > 0)
                    {
                        priorAuthNumber = resXmlDoc.GetElementsByTagName("PriorAuthNumber")[0].InnerText;
                    }

                    if (resXmlDoc != null && resXmlDoc.GetElementsByTagName("PriorAuthStatus").Count > 0)
                    {
                        priorAuthStatus = resXmlDoc.GetElementsByTagName("PriorAuthStatus")[0].InnerText;
                    }

                    if (string.IsNullOrEmpty(priorAuthStatus) || string.IsNullOrEmpty(priorAuthNumber))
                    {
                        Logging log = new Logging();
                        log.CreateLogEntry(string.Format("PriorAuth Number and PriorAuth Status are missing from Response, for Transaction Id : {0} ", transactionID.ToString()), Logging.LogPriority.Error);

                        throw new NullReferenceException("PriorAuth Number and PriorAuth Status are missing from Response");
                    }
                    else
                    {
                        DataSet ds = _spa.GetPriorAuthStatusCodes(priorAuthStatus);
                        DataTable dtPrior = Helper.HasRows(ds) ? ds.Tables[0] : null;
                        this.DataList = dtPrior;

                        if (Helper.HasRows(dtPrior))
                        {
                            DataRow dr = dtPrior.Rows[0];
                            priorAuthStatus = Helper.GetString("PRIOR_AUTH_STATUS_CODE_DESC", dr);
                        }

                        //Display PopUp Message for Successful submission of the transaction.
                        lblTXNResponse.Text = "Transaction   Successfully Submitted!" + "<br />"
                            + "Prior Authorization Status: " + priorAuthStatus + "<br />"
                            + "Prior Authorization Number: " + priorAuthNumber;
                    }
                }
                else if (destPayerIDArrayEDI.Contains(destPayerID))
                {
                    //Display PopUp Message for Successful submission of the transaction.
                    lblTXNResponse.Text = "Transaction Successfully Submitted!";
                }

                modalPATXNSuccess.Show();
            }
            else
            {
                DataTable dt = new DataTable();
                xmlResponse = InfoAccessController.GetResponsePayloadByPassThroughTransactionID(transactionID);
                XmlDocument xmlDoc2 = new XmlDocument();
                try
                {
                    xmlDoc2.LoadXml(xmlResponse);
                }
                catch (XmlException exception)
                {
                    throw exception;
                }

                DataSet ds = new DataSet();
                XmlNodeReader xmlread = new XmlNodeReader(xmlDoc2);
                ds.ReadXml(xmlread);

                if (ds.Tables.Contains("ErrorDetails"))
                {
                    dt = ds.Tables["ErrorDetails"];
                }

                lblTXNResponseID.Text = "Transaction " + transactionID.ToString() + " failed transformation";
                grdTXNResponse.DataSource = dt;
                grdTXNResponse.DataBind();
                lblSupport.Text = AppSettings.Get("PriorAuthEDISupportText", "Please contact IHD support at 1-800-686-1516 or <a href='mailto: IHD@medicaid.ohio.gov'>IHD@medicaid.ohio.gov</a>");
                modalPATXNFailure.Show();
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private bool ValidateAttachment(string paType)
    {
        bool status = false;
        try
        {
            var ds = PriorAuthHospitalController.SelectPriorAuthAttachment(rblClaimType.SelectedValue, MedicaidId);
            var dataTable = ds.Tables["Attachments"];
            if (dataTable != null && dataTable.Rows.Count > 0 && dataTable.Rows.Count >= 10)
            {
                if (paType == "Institutional")
                {
                    lblAttachmentErrorMsg.Visible = true;
                    lblAttachmentErrorMsg.Text = "Maximum 10 attachments can be submitted.";
                    return true;
                }
                else if (paType == "DentalProf")
                {
                    lblDentalAttachmentErrorMsg.Visible = true;
                    lblDentalAttachmentErrorMsg.Text = "Maximum 10 attachments can be submitted.";
                    return true;
                }
            }
            else
            {
                if (paType == "Institutional")
                {
                    lblAttachmentErrorMsg.Visible = false;
                    lblAttachmentErrorMsg.Text = "";
                    return false;
                }
                else if (paType == "DentalProf")
                {
                    lblDentalAttachmentErrorMsg.Visible = false;
                    lblDentalAttachmentErrorMsg.Text = "";
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            throw CoreException.ThrowException(ex);
        }
        return status;
    }





    protected void gvServiceDetailDental_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gvServiceDetailProfessional_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    private DataTable GetPlaceOfServiceData(string facilityCode, string facilityDesc, bool textChangevent = false)
    {
        DataTable dt = null;
        try
        {
            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                if (textChangevent == true)
                {
                    // var ds = ProviderController.GetPlaceOfserviceByCodeDynamic(facilityCode,);
                    var ds = ProviderController.GetPlaceOfserviceByCode(facilityCode, facilityDesc);

                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                    }
                }
                else
                {
                    var ds = ProviderController.GetPlaceOfserviceByCode(facilityCode, facilityDesc);
                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetPlaceofServiceData");
            return dt;
        }
        return dt;
    }
    protected void btnPlaceOfServiceSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtPlaceOfServiceCode.Text) && string.IsNullOrWhiteSpace(txtPlaceOfServiceDesc.Text))
            {
                lblPlaceOfServiceSearchError.Visible = true;
                lblPlaceOfServiceSearchError.Text = "Either place of code or description is required";
                mpeServiceinfoPlaceofService.Show();
                return;
            }
            else
            {
                lblPlaceOfServiceSearchError.Visible = false;
                lblPlaceOfServiceSearchError.Text = "";
                lblSrchPlc.Visible = true;
                //gvPlaceOfSearch.Visible = true;
                //this.gvPlaceOfSearch.CurrentPageIndex = 0;
                DataTable dt = GetPlaceOfServiceData(txtPlaceOfServiceCode.Text.Trim(), txtPlaceOfServiceDesc.Text.Trim());
                //gvPlaceOfSearch.DataSource = dt;
                //gvPlaceOfSearch.DataBind();
                mpeServiceinfoPlaceofService.Show();
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }
    protected void lnkPlaceOfServiceCode_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string desc = btn.CommandArgument;
        string code = btn.CommandName;
        txtpalceofservice.Text = code + "-" + desc;
        //gvPlaceOfSearch.DataSource = null;
        //gvPlaceOfSearch.DataBind();
        lblPlaceofErrorSearch.Visible = false;
        lblSrchPlc.Visible = false;
        //gvPlaceOfSearch.Visible = false;
        mpeServiceinfoPlaceofService.Hide();
    }

    protected void txtpalceofservice_TextChanged(object sender, EventArgs e)
    {
        hdnModified.Value = "true";
        lblPlaceofErrorSearch.Text = string.Empty;
        txtpalceofservice.BackColor = Color.White;

        DataTable dt;
        if (!string.IsNullOrEmpty(txtpalceofservice.Text.Trim()))
        {
            if (txtpalceofservice.Text.Contains("-"))
            {
                string[] PlaceofService = txtpalceofservice.Text.Trim().Split('-');
                dt = GetPlaceOfServiceData(PlaceofService[0].Trim(), PlaceofService[1].Trim(), true);
                txtPlaceOfServiceCode.Text = PlaceofService[0].Trim();
                txtPlaceOfServiceDesc.Text = PlaceofService[1].Trim();
            }
            else
            {
                dt = GetPlaceOfServiceData(txtpalceofservice.Text, default(string), true);
                txtPlaceOfServiceCode.Text = txtpalceofservice.Text.Trim();
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 1)
                {
                    mpeServiceinfoPlaceofService.Show();
                }
                else
                {
                    string code = Convert.ToString(dt.Rows[0]["PRIOR_AUTH_PLACE_OF_SERVICE_MMIS"]);
                    string desc = Convert.ToString(dt.Rows[0]["PRIOR_AUTH_PLACE_OF_SERVICE_DESC"]);
                    txtpalceofservice.Text = code + "-" + desc;
                    lblPlaceofErrorSearch.Text = "";
                    lblPlaceofErrorSearch.Visible = false;
                }
            }
            else
            {
                lblPlaceofErrorSearch.Visible = true;
                lblPlaceofErrorSearch.Text = "Place of Service is invalid";
            }
        }
        else
        {
            lblPlaceofErrorSearch.Text = string.Empty;
        }
    }

    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        CloseCleanAll();
    }

    protected void btnCancelPARequYes1_Click(object sender, EventArgs e)
    {
        CloseCleanAll();
    }

    protected void CloseCleanAll()
    {
        //diagnosis add/update panel
        // pnlDiagnosisLine.Visible = false;

        //dental add/update panel
        //  pnlDentalLine.Visible = false;
        pnlsepDentalLine.Visible = false;

        //professional add/update panel
        pnlsepProfessionalLine.Visible = false;
        // pnlProfessionalLine.Visible = false;

        //institutional add/update panel
        pnlsepServiceDetail.Visible = false;
        pnlServiceDetail.Visible = false;

        rblClaimType.ClearSelection();
        rblClaimType.Enabled = true;
        divSubmitPriorAuth.Visible = false;
        ClearAllPageControls();
        if (Session["isSubValid"] != null)
        {
            Session.Remove("isSubValid");
        }

        if (Session["isACKed"] != null)
        {
            Session.Remove("isACKed");
        }

        if (Session["ATTACHMENT_RETENTION_DATA"] != null)
        {
            Session.Remove("ATTACHMENT_RETENTION_DATA");
            //gvAttachment.DataSource = null;
            //gvAttachment.DataBind();
            //gvDentalAttachment.DataSource = null;
            //gvDentalAttachment.DataBind();
        }
        else
        {
            //gvAttachment.DataSource = null;
            //gvAttachment.DataBind();
            //gvDentalAttachment.DataSource = null;
            //gvDentalAttachment.DataBind();
        }

        txtProviderNotes.Enabled = true;
        //Hide all errors/warnings here
        valerrormess.Visible = false;
        lblpriorautherror.Text = "";

        //enable panels
        pnlRecipient.Enabled = true;
        pnlContact.Enabled = true;
        pnlServiceInformation.Enabled = true;
        pnlTrackingNumber.Enabled = true;
        pnlService.Enabled = true;
        pnlServiceProviderInfo.Enabled = true;
        pnlorderproviderinfo.Enabled = true;
        pnlDiagnosisLine.Enabled = true;
        pnlCertHospital.Enabled = true;
        pnlProviderNote.Enabled = true;
        pnlProvidermainPnl.Enabled = true;
        pnlOutcomeOfReview.Enabled = true;
        pnlAttachment.Enabled = true;
        pnlDentalAttachment.Enabled = true;
        pnlmissingtooth.Enabled = true;
        pnlDocumentbyMail.Enabled = true;
        pnlreviewernoteprovider.Enabled = true;
        pnlReasonforDenial.Enabled = true;

        ddlAuthorization.Enabled = true;
        ddlAssignment.Enabled = true;
        ddlServiceType.Enabled = true;

        btnDiagnosisAdd.Enabled = true;
        hdnProvNoteText.Value = "";

        btnUpdate.Visible = false;
        hdnPriorAuthAttachments.Value = "";
    }

    protected void txtRequestUnt_TextChanged(object sender, EventArgs e)
    {
        hdnModified.Value = "true";
        ValidatedRequestedUnits(txtRequestUnt, requnitsError);
    }

    private bool ValidatedRequestedUnits(TextBox txtReq, Label lblReqError = null)
    {
        bool isError = false;
        if (!string.IsNullOrEmpty(txtReq.Text))
        {
            long requestedUnits = long.Parse(txtReq.Text);
            if (requestedUnits < 1)
            {
                if (lblReqError != null)
                {
                    lblReqError.Text = "Invalid requested Units";
                    lblReqError.Visible = true;
                }
                isError = true;
            }
            else
            {
                if (lblReqError != null)
                {
                    lblReqError.Text = "";
                    lblReqError.Visible = false;
                }
            }
        }
        return isError;
    }

    protected void txtDentalReqUnits_TextChanged(object sender, EventArgs e)
    {
        hdnModified.Value = "true";
        ValidatedRequestedUnits(txtDentalReqUnits, lblDentalReqUnitsError);
    }

    protected void txtProfessionalReqUnits_TextChanged(object sender, EventArgs e)
    {
        hdnModified.Value = "true";
        ValidatedRequestedUnits(txtProfessionalReqUnits, lblProfessionalReqUnitsError);
    }

    protected void gvServiceDetailProfessional_PageIndexChanged(object sender, EventArgs e)
    {

    }

    protected void gvServiceDetailDental_PageIndexChanged(object sender, EventArgs e)
    {

    }

    protected void gvServiceDetail_PageIndexChanged(object sender, EventArgs e)
    {

    }

    protected void gvServiceDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GetServiceDetails();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void gvServiceDetailProfessional_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //gvServiceDetailProfessional.PageIndex = e.NewPageIndex;
            GetProfessionalServiceDetails();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void gvServiceDetailDental_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //gvServiceDetailDental.PageIndex = e.NewPageIndex;
            GetDentalServiceDetails();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void facCloseModalId_ServerClick(object sender, EventArgs e)
    {
        //gvPriorAuthFacilitySearchPage.DataSource = null;
        //gvPriorAuthFacilitySearchPage.DataBind();
        txtFacilityTypeCode.Text = "";
        txtFacilityTypeDescription.Text = "";
        this.lblFacilitySResult.Visible = false;
        //gvPriorAuthFacilitySearchPage.Visible = false;
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "modal", "closeFacModal()", true);
    }

    protected void btnCancelPARequest_Click(object sender, EventArgs e)
    {

        mdlCancelPARequest.Show();
    }

    protected void btnCancelPARequYes_Click(object sender, EventArgs e)
    {

        try
        {
            string reqType = "3";
            makePriorAuthAddUpdateRequest(reqType);
            CloseCleanAll();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "CancelPA");
            //LogErrror(ex,"CancelPA");
            IntuitivePriorAuthMessageBoxID.Show(string.Format("An error has occurred, please save your prior authorization for future submission. Reference Id : {0} ", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void btnUpdatePARequestNo_Click(object sender, EventArgs e)
    {
        try
        {
            InquirePriorAuthResponse iq = (InquirePriorAuthResponse)Session[INQUIRY_RESPONSE_RETENTION_DATA];
            LoadProviderInformationByTransaction(iq);
            mdlpnlUpdatePARequest.Hide();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-BtnUpdatePARequestNoClick");
            IntuitivePriorAuthMessageBoxID.Show(string.Format("An error has occurred while update PA request No click event. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        mdlpnlUpdatePARequest.Show();
    }

    protected void btnUpdatePARequestYes_Click(object sender, EventArgs e)
    {
        var isSubValid = true;

        if (!ValidateData())
        {
            valerrormess.Visible = true;
            isSubValid = false;
        }
        if (!FrontEndEdits())
        {
            isSubValid = false;
        }

        if (!ValidatePAAttachments())
        {
            isSubValid = false;
        }

        if (!isSubValid)
            return;

        try
        {
            string reqType = "S";
            makePriorAuthAddUpdateRequest(reqType);
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "UpdatePA");
            //LogErrror(ex,"UpdatePA");
            IntuitivePriorAuthMessageBoxID.Show(string.Format("An error has occurred, please save your prior authorization for future submission. Reference Id : {0} ", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void btnCancelPARequNo_Click(object sender, EventArgs e)
    {

        mdlCancelPARequest.Hide();
    }

    protected void btnWarningAcknowledgmentYes_Click(object sender, EventArgs e)
    {
        Session["isACKed"] = true;
        mdlWarningAcknowledgment.Hide();
    }

    protected void btnWarningAcknowledgmentNo_Click(object sender, EventArgs e)
    {
        Session["isACKed"] = false;
        mdlWarningAcknowledgment.Hide();
    }


    public void SaveProfessionalServiceDetailsDataToDb(Guid lnkRecord)
    {
        try
        {
            Dictionary<string, string> parms2 = new Dictionary<string, string>();
            parms2.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms2.Add("LINK_SECTIONS", SaveCodeLNK);
            DataSet dsProffServiceDetails = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms2);

            foreach (DataRow dbRow in dsProffServiceDetails.Tables[0].Rows)
            {
                if (dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"] == null || dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"] == DBNull.Value)
                { continue; }

                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", Convert.ToString(dbRow["PRIOR_AUTH_PROCEDURE_CODE_ID"]));
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1", Convert.ToString(dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"]));
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2", Convert.ToString(dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"]));
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3", Convert.ToString(dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"]));
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4", Convert.ToString(dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"]));
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS", Convert.ToString(dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"]));
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR", Convert.ToString(dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"]));

                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS", dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"]);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS", dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"]);
                parms.Add("PRIOR_AUTH_STATUS_ID", dbRow["PRIOR_AUTH_STATUS_ID"]);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC", dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC"]);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"]);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM", dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM"]);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS", dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"]);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR", dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"]);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS", dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"]);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS", dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"]);

                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID", dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"]);

                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS", dbRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"]);
                parms.Add("MedicaidID", dbRow["MedicaidID"]);
                parms.Add("Line", dbRow["Line"]);

                parms.Add("LAST_MODIFIED_USER", new Guid(CON.appAdminUserId));
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
                parms.Add("CREATED_BY_USER", new Guid(CON.appAdminUserId));
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now);
                parms.Add("LINK_SECTIONS", lnkRecord);

                int value = PriorAuthHospitalController.InsertPriorAuthServiceDetailsData("insertPRIOR_AUTH_PROFFSERVICE_DETAIL", parms);

            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }


    public void SaveDentalServiceDetailsDataToDb(Guid lnkRecord)
    {
        try
        {
            Dictionary<string, string> parms2 = new Dictionary<string, string>();
            parms2.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms2.Add("LINK_SECTIONS", SaveCodeLNK);
            DataSet dsProffServiceDetails = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms2);

            foreach (DataRow dbRow in dsProffServiceDetails.Tables[0].Rows)
            {
                if (dbRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_ID"] == null || dbRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_ID"] == DBNull.Value)
                { continue; }

                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Add("PRIOR_AUTH_TOOTH_NUMBER_ID", Convert.ToString(dbRow["PRIOR_AUTH_TOOTH_NUMBER_ID"]));
                parms.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", Convert.ToString(dbRow["PRIOR_AUTH_PROCEDURE_CODE_ID"]));
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS", Convert.ToString(dbRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS"]));
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS", Convert.ToString(dbRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS"]));
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS", Convert.ToString(dbRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS"]));
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS", Convert.ToString(dbRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS"]));
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS", Convert.ToString(dbRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS"]));

                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE", dbRow["PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE"]);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE", dbRow["PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE"]);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE", dbRow["PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE"]);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE", dbRow["PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE"]);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE", dbRow["PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE"]);

                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS", dbRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS"]);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR", dbRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR"]);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS", dbRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"]);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS", dbRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"]);
                parms.Add("PRIOR_AUTH_STATUS_ID", dbRow["PRIOR_AUTH_STATUS_ID"]);

                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC", dbRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC"]);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", dbRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"]);
                parms.Add("PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID", dbRow["PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID"]);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM", dbRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM"]);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS", dbRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS"]);

                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR", dbRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR"]);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS", dbRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"]);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS", dbRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"]);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", dbRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"]);

                parms.Add("MedicaidID", dbRow["MedicaidID"]);
                parms.Add("Line", dbRow["Line"]);

                parms.Add("LAST_MODIFIED_USER", new Guid(CON.appAdminUserId));
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
                parms.Add("CREATED_BY_USER", new Guid(CON.appAdminUserId));
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now);
                parms.Add("LINK_SECTIONS", lnkRecord);

                int value = PriorAuthHospitalController.InsertPriorAuthServiceDetailsData("insertPRIOR_AUTH_DENTALSERVICE_DETAIL", parms);
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    public static string GetToothInfoByToothID(int toothID)
    {
        string tooth = string.Empty;

        if (toothID <= 0)
        {
            return "";
        }
        try
        {
            List<Models.Data.DentalToothInfo> toothInfo = LoadDentalToothInfo();
            if (toothInfo != null && toothInfo.Count > 0)
            {
                Models.Data.DentalToothInfo DentalToothInfo = toothInfo.Where(t => t.PRIOR_AUTH_TOOTH_NUMBER_ID == toothID).FirstOrDefault();
                tooth = DentalToothInfo != null ? DentalToothInfo.PRIOR_AUTH_TOOTH_NUMBER_CODE : "";
            }
        }
        catch (Exception ex)
        {

        }
        return tooth;
    }

    private static List<Models.Data.DentalToothInfo> LoadDentalToothInfo()
    {
        //string key = "DentalToothInfo";
        List<Models.Data.DentalToothInfo> toothInfo = new List<Models.Data.DentalToothInfo>();

        //if (!DictionaryCache.Exists(key))
        //{

        PDMSService.PDMSServiceClient service = new PDMSService.PDMSServiceClient();

        DataSet dsToothNumbers = service.GetPriorAuthDentalToothNumber();
        DataTable dtToothNumber = dsToothNumbers.Tables[0];

        if (dtToothNumber != null && dtToothNumber.Rows.Count > 0)
        {
            foreach (DataRow currentRow in dtToothNumber.Rows)
            {
                Models.Data.DentalToothInfo DentalToothInfo = new Models.Data.DentalToothInfo();

                DentalToothInfo.PRIOR_AUTH_TOOTH_NUMBER_ID = currentRow["PRIOR_AUTH_TOOTH_NUMBER_ID"] != null && currentRow["PRIOR_AUTH_TOOTH_NUMBER_ID"] != DBNull.Value ? Convert.ToInt32(currentRow["PRIOR_AUTH_TOOTH_NUMBER_ID"]) : 0;
                DentalToothInfo.PRIOR_AUTH_TOOTH_NUMBER_CODE = currentRow["PRIOR_AUTH_TOOTH_NUMBER_CODE"] != null && currentRow["PRIOR_AUTH_TOOTH_NUMBER_CODE"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_TOOTH_NUMBER_CODE"]) : "";

                toothInfo.Add(DentalToothInfo);
            }

            // DictionaryCache.Add<List<Models.Data.DentalToothInfo>>(key, toothInfo);
        }
        //}
        //else
        //{
        //    toothInfo = DictionaryCache.Get<List<Models.Data.DentalToothInfo>>(key);
        //}
        return toothInfo;
    }

    public void SaveInstitutionalServiceDetailsDataToDb(Guid lnkRecord)
    {
        try
        {
            Dictionary<string, string> parms2 = new Dictionary<string, string>();
            parms2.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms2.Add("LINK_SECTIONS", SaveCodeLNK);
            DataSet dsProffServiceDetails = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms2);

            foreach (DataRow dbRow in dsProffServiceDetails.Tables[0].Rows)
            {
                if (dbRow["PRIOR_AUTH_SERVICE_DETAIL_ID"] == null || dbRow["PRIOR_AUTH_SERVICE_DETAIL_ID"] == DBNull.Value)
                { continue; }

                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Add("PRIOR_AUTH_SERVICE_REVENUE_CODE", dbRow["PRIOR_AUTH_SERVICE_REVENUE_CODE"]);
                parms.Add("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", dbRow["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"]);
                parms.Add("PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE", dbRow["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"]);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", dbRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"]);
                parms.Add("PRIOR_AUTH_REQUESTED_UNITS_ID", dbRow["PRIOR_AUTH_REQUESTED_UNITS_ID"]);

                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", dbRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"]);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", dbRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"]);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", dbRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"]);
                parms.Add("PRIOR_AUTH_STATUS_ID", dbRow["PRIOR_AUTH_STATUS_ID"]);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC", dbRow["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"]);

                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE", dbRow["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"]);
                parms.Add("PRIOR_AUTH_LEVEL_CARE_ID", dbRow["PRIOR_AUTH_LEVEL_CARE_ID"]);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS", dbRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"]);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", dbRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"]);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR", dbRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"]);

                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS", dbRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"]);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS", dbRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"]);

                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO", dbRow["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"]);
                parms.Add("MedicaidID", dbRow["MedicaidID"]);
                parms.Add("Line", dbRow["Line"]);

                parms.Add("LINK_SECTIONS", lnkRecord);

                parms.Add("LAST_MODIFIED_USER", new Guid(CON.appAdminUserId));
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
                parms.Add("CREATED_BY_USER", new Guid(CON.appAdminUserId));
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now);

                int value = PriorAuthHospitalController.InsertPriorAuthServiceDetailsData("insertPRIOR_AUTH_SERVICE_DETAIL", parms);
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
            IntuitivePriorAuthMessageBoxID.Show(string.Format("Something went wrong. The Administrator has been notified. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void btnCloseSuccess_Click(object sender, EventArgs e)
    {
        CloseCleanAll();
    }

    protected void lnkServiceLine_Click(object sender, EventArgs e)
    {
        try
        {
            ddlServiceTypeCode.ClearSelection();
            ddLnRequestUnits.ClearSelection();
            ddLnLevelOfCare.ClearSelection();
            GetServiceCodeTypeCodeForPriorAuth();
            // btnServDetailsUpdateAdd.Text = "Update";
            LinkButton btn = (LinkButton)sender;
            List<string> args = btn.CommandArgument.ToString().Split(',').ToList<string>();

            string lineNumber = args[0];// Convert.ToString(e.CommandArgument);

            // pnlline.Visible = true;
            pnlsepline.Visible = true;
            //   pnlDentalLine.Visible = false;
            pnlsepDentalLine.Visible = false;

            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            DataSet ds; DataTable dataTable = null;
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms.Add("LINK_SECTIONS", SaveCodeLNK);
            ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
            dataTable = ds != null ? ds.Tables[0] : null;

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow currentRow = null;

                dataTable = ds.Tables[0];
                int gridSno = 0;
                foreach (DataRow dr in dataTable.Rows)
                {
                    gridSno++;
                    if (Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_ID"]) == lineNumber)
                    {
                        currentRow = dr;
                        break;
                    }

                }
                // txtservicedetailsReqFDOS.Text = reqFDOS.ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                // GridViewRow grdrow = (GridViewRow)((Button)sender).NamingContainer;
                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_ID"] != null &&
                   currentRow["PRIOR_AUTH_SERVICE_DETAIL_ID"] != DBNull.Value)
                {
                    hdLineNumber.Value = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_ID"]);
                }

                if (currentRow["PRIOR_AUTH_SERVICE_REVENUE_CODE"] != null &&
                   currentRow["PRIOR_AUTH_SERVICE_REVENUE_CODE"] != DBNull.Value)
                {
                    txtServicecode.Text = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_REVENUE_CODE"]);
                }

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"] != null &&
                   currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"] != DBNull.Value)
                {
                    txtRequestUnt.Text = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"]);
                }

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"] != null &&
                   currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"] != DBNull.Value)
                {
                    txtAuthorizedUnits.Text = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"]);
                }

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"] != null &&
                   currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"] != DBNull.Value)
                {
                    txtRequestedDollars.Text = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"]);
                }

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"] != null &&
                   currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"] != DBNull.Value &&
                   !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString())
                    && Convert.ToDouble(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"]) != 0)
                {
                    txtAuthorizedDollars.Text = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"]);
                }
                else
                    txtAuthorizedDollars.Text = string.Empty;

                lblservDetlbl.Text = Convert.ToString(gridSno);// Convert.ToString(currentRow["Line"]); 

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] != null && currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] != DBNull.Value
                && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/1900")
                    txtReqFDOS.Text = Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] != null && currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] != DBNull.Value
                && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/1900")
                    txtReqTDOS.Text = Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"] != null && currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"] != DBNull.Value
                && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"]).ToShortDateString() != "1/1/1900"
                && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"]) >= Convert.ToDateTime("1/1/1900"))
                    txtAuthorizedFromDOS.Text = Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"] != null && currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"] != DBNull.Value
                && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"]).ToShortDateString() != "1/1/1900"
                    && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"]) >= Convert.ToDateTime("1/1/1900"))
                    txtAuthorizedToDOS.Text = Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);

                if (currentRow["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"] != null &&
                   currentRow["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"] != DBNull.Value &&
                   !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"].ToString()))
                {
                    ddlServiceTypeCode.Items.FindByValue(Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"])).Selected = true;
                }

                if (currentRow["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"] != null &&
                  currentRow["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"] != DBNull.Value &&
                  !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"].ToString()))
                {
                    txtLnProcedureCode.Text = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"]);
                }

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"] != null &&
                  currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"] != DBNull.Value &&
                  !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"].ToString()))
                {
                    txtRequestUnt.Text = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"]);
                }

                if (currentRow["PRIOR_AUTH_REQUESTED_UNITS_ID"] != null &&
                  currentRow["PRIOR_AUTH_REQUESTED_UNITS_ID"] != DBNull.Value &&
                  !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_REQUESTED_UNITS_ID"].ToString()))
                {
                    ddLnRequestUnits.Items.FindByValue(Convert.ToString(currentRow["PRIOR_AUTH_REQUESTED_UNITS_ID"])).Selected = true;
                }
                //txtAuthorizedUnits.Text = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"]);

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"] != null &&
                  currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"] != DBNull.Value &&
                  !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"].ToString()))
                {
                    txtLnprocCodeDesc.Text = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"]);
                }

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"] != null &&
                  currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"] != DBNull.Value &&
                  !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"].ToString()))
                {
                    txtLnProviderServiceNote.Text = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"]);
                }

                if (currentRow["PRIOR_AUTH_LEVEL_CARE_ID"] != null &&
                  currentRow["PRIOR_AUTH_LEVEL_CARE_ID"] != DBNull.Value &&
                   !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_LEVEL_CARE_ID"].ToString()) &&
                  Convert.ToInt32(currentRow["PRIOR_AUTH_LEVEL_CARE_ID"]) != 0)
                {
                    ddLnLevelOfCare.Items.FindByValue(Convert.ToString(currentRow["PRIOR_AUTH_LEVEL_CARE_ID"])).Selected = true;
                }
                else
                {
                    ddLnLevelOfCare.Items[0].Selected = true;
                }

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"] != null &&
                    currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"].ToString()) &&
                    Convert.ToInt32(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"]) != 0)
                {
                    txtLnRemainingUnits.Text = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"]);
                }
                else
                    txtLnRemainingUnits.Text = string.Empty;

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"] != null && currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"] != DBNull.Value
                    && Convert.ToInt32(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"]) != 0)
                {
                    txtAuthorizedUnits.Text = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"]);
                }
                else
                    txtAuthorizedUnits.Text = string.Empty;

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"] != null &&
                 currentRow["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"] != DBNull.Value &&
                 !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"].ToString()))
                {
                    txtLnServiceTrackingNo.Text = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"]);
                }

                int value = 0;

                if (currentRow["PRIOR_AUTH_STATUS_ID"] != null &&
                 currentRow["PRIOR_AUTH_STATUS_ID"] != DBNull.Value &&
                 !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_STATUS_ID"].ToString()))
                {
                    string x = currentRow["PRIOR_AUTH_STATUS_ID"].ToString();
                    if (!string.IsNullOrEmpty(x))
                    {

                        if (int.TryParse(x, out value))
                        {
                            if (Convert.ToInt32(currentRow["PRIOR_AUTH_STATUS_ID"]) == 1)
                            {
                                txtLnStatus.Text = "Submission Pending";
                            }
                            else if (Convert.ToInt32(currentRow["PRIOR_AUTH_STATUS_ID"]) == 2)
                            {
                                txtLnStatus.Text = "Approved";
                            }
                            else if (Convert.ToInt32(currentRow["PRIOR_AUTH_STATUS_ID"]) == 3)
                            {
                                txtLnStatus.Text = "Denied";
                            }
                        }
                    }
                }

                txtAuthorizedUnits.Style.Add("background-color", "lightgray");
                txtLnStatus.Style.Add("background-color", "lightgray");
                txtAuthorizedDollars.Style.Add("background-color", "lightgray");
                txtAuthorizedFromDOS.Style.Add("background-color", "lightgray");
                txtAuthorizedToDOS.Style.Add("background-color", "lightgray");
                txtLnRemainingUnits.Style.Add("background-color", "lightgray");
            }
            else
            {
                string logHeader = "Edit Service Details - Institutional PA";
                string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                logMsg += "Session[SERVICE_DETAILS] is Empty";

                Logging log = new Logging(new Guid(CON.WebPageLogGuid.PriorAuthWebPageLog), logMsg);
                log.CreateLogEntry(logHeader, Logging.LogPriority.Error);
            }

            InstitutionalServiceReset(true);
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-BtnServiceDetailEditClick");
            IntuitivePriorAuthMessageBoxID.Show(string.Format("An error has occurred while service detail edit click event. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    protected void lnkProfessionalServiceLine_Click(object sender, EventArgs e)
    {
        try
        {
            ClearProfessionalDropdownSelection();
            // btnServProfessionalUpdate.Text = "Update";
            LinkButton btn = (LinkButton)sender;
            List<string> args = btn.CommandArgument.ToString().Split(',').ToList<string>();

            string lineNumber = args[0];// Convert.ToString(e.CommandArgument);

            // pnlProfessionalLine.Visible = true;
            pnlsepProfessionalLine.Visible = true;
            // pnlline.Visible = false;
            pnlsepline.Visible = false;
            ClearProfessionalDropdownSelection();

            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            DataRow currentRow = null;
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", returnPATypeID());
            parms.Add("LINK_SECTIONS", SaveCodeLNK);
            DataSet ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);

            int gridSno = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                gridSno++;
                if (dr != null &&
                    dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"] != null &&
                    Convert.ToString(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"]) == lineNumber)
                {
                    currentRow = dr;
                    break;
                }
            }

            hdLineNumber.Value = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"]) : string.Empty;

            txtProfessionalReqUnits.Text = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"]) : string.Empty;
            txtProfessionalAuthUnits.Text = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"]) : string.Empty;
            txtProfessionalReqDollars.Text = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"]) : string.Empty;
            txtProfessionalAuthDollars.Text = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"]) : string.Empty;
            lblservProfessionalLineNumber.Text = Convert.ToString(gridSno);// Convert.ToString(currentRow["Line"]);

            if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != null &&
                currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != DBNull.Value &&
                !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString()) &&
                Convert.ToDouble(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"]) != 0)
            {
                txtProfessionalAuthDollars.Text = Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"]);
            }
            else
            {
                txtProfessionalAuthDollars.Text = string.Empty;
            }

            if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"] != null &&
                currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"] != DBNull.Value &&
                !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"].ToString()) &&
                Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/1900")
                txtProfessionalReqFDOS.Text = Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);

            if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"] != null &&
                currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"] != DBNull.Value &&
                !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"].ToString()) &&
                Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/1900")
                txtProfessionalReqTDOS.Text = Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);

            if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"] != null &&
                currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"] != DBNull.Value &&
                !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"].ToString()) &&
                Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"]).ToShortDateString() != "1/1/1900" &&
                Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"]) >= Convert.ToDateTime("1/1/1900"))
                txtProfessionalAuthFDOS.Text = Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);

            if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"] != null &&
                currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"] != DBNull.Value &&
                !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"].ToString()) &&
                Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"]).ToShortDateString() != "1/1/1900" &&
                Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"]) >= Convert.ToDateTime("1/1/1900"))
                txtProfessionalAuthTDOS.Text = Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);

            txtProfessionalSDProcCode.Text = Convert.ToString(currentRow["PRIOR_AUTH_PROCEDURE_CODE_ID"]);

            if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"] != null &&
                currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"] != DBNull.Value &&
                !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"].ToString()) &&
                Convert.ToInt32(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"]) != 0)
            {
                txtProfessionalRemainingUnits.Text = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"]) : string.Empty;
            }
            else
                txtProfessionalRemainingUnits.Text = string.Empty;
            //txtProfessionalRemainingUnits.Text = Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"]);
            txtProfessionalServTrackingNo.Text = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM"]) : string.Empty;

            string x = currentRow["PRIOR_AUTH_STATUS_ID"] != null && currentRow["PRIOR_AUTH_STATUS_ID"] != DBNull.Value ? currentRow["PRIOR_AUTH_STATUS_ID"].ToString() : string.Empty;
            int value = 0;
            if (int.TryParse(x, out value))
            {
                if (currentRow["PRIOR_AUTH_STATUS_ID"] != null &&
                    currentRow["PRIOR_AUTH_STATUS_ID"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_STATUS_ID"].ToString()) &&
                    Convert.ToInt32(currentRow["PRIOR_AUTH_STATUS_ID"]) == 1)
                {
                    txtProfessionalServDetailsStatus.Text = "Submission Pending";
                }
                else if (Convert.ToInt32(currentRow["PRIOR_AUTH_STATUS_ID"]) == 2)
                {
                    txtProfessionalServDetailsStatus.Text = "Approved";
                }
                else if (Convert.ToInt32(currentRow["PRIOR_AUTH_STATUS_ID"]) == 3)
                {
                    txtProfessionalServDetailsStatus.Text = "Denied";
                }
            }
            txtProfessionalProcCodeDescription.Text = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC"]) : string.Empty;
            txtProfessionalProvServnote.Text = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"]) : string.Empty;

            txtModifier1.Text = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"]) : string.Empty;
            txtModifier2.Text = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"]) : string.Empty;
            txtModifier3.Text = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"]) : string.Empty;
            txtModifier4.Text = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"]) : string.Empty; ;

            ddProffSDMeasurements.ClearSelection();
            GetRequestedUnitMeasures();

            if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"] != null &&
               currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"] != DBNull.Value)
            {
                IEnumerable<ListItem> listItems = ddProffSDMeasurements.Items.OfType<ListItem>();

                foreach (var item in listItems)
                {
                    string[] arrItemName = item.Text.Split('-');

                    if (arrItemName != null && arrItemName.Length > 0 && arrItemName[0] == Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"]).Trim())
                    {
                        ddProffSDMeasurements.Items.FindByValue(item.Value).Selected = true;
                        break;
                    }
                }
            }

            txtProfessionalAuthUnits.Style.Add("background-color", "lightgray");
            txtProfessionalAuthDollars.Style.Add("background-color", "lightgray");
            txtProfessionalAuthFDOS.Style.Add("background-color", "lightgray");
            txtProfessionalAuthTDOS.Style.Add("background-color", "lightgray");
            txtProfessionalRemainingUnits.Style.Add("background-color", "lightgray");
            txtProfessionalServDetailsStatus.Style.Add("background-color", "lightgray");

            ProfessionalServiceReset(true);
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SubmitPA-BtnProfessionalServiceDetailEditClick");
            IntuitivePriorAuthMessageBoxID.Show(string.Format("An error has occurred while professional service detail edit click event. Reference Id : {0}", logNumber), "Error", ex.Message + " " + ex.StackTrace);
        }
    }

    private void DentalServiceReset(bool disabled)
    {
        if (disabled)
        {
            lnkDentalSDProcCodeSearchLink.Enabled = false;
            txtDentalSDProcCode.Style.Add("background-color", "lightgray");
            txtDentalReqUnits.Style.Add("background-color", "lightgray");
            txtDentalProcCodeDescription.Style.Add("background-color", "lightgray");
            txtDentalReqDollars.Style.Add("background-color", "lightgray");
            ddDentalToothNumber.Enabled = false;
            ddDentalOralCavity1.Enabled = false;
            ddDentalOralCavity2.Enabled = false;
            ddDentalOralCavity3.Enabled = false;
            ddDentalOralCavity4.Enabled = false;
            ddDentalOralCavity5.Enabled = false;
            ddToothSurface1.Enabled = false;
            ddToothSurface2.Enabled = false;
            ddToothSurface3.Enabled = false;
            ddToothSurface4.Enabled = false;
            ddToothSurface5.Enabled = false;
            txtDentalProvServnote.Style.Add("background-color", "lightgray");
            ddDentalProsthsis.Enabled = false;
            txtDentalReqFDOS.Style.Add("background-color", "lightgray");
            txtDentalReqTDOS.Style.Add("background-color", "lightgray");
            txtDentalServTrackingNo.Style.Add("background-color", "lightgray");
            //btnServDentalUpdate.Visible = false;
            txtDentalSDProcCode.Enabled = false;
            txtDentalReqUnits.Enabled = false;
            txtDentalProcCodeDescription.Enabled = false;
            txtDentalReqDollars.Enabled = false;
            txtDentalProvServnote.Enabled = false;
            txtDentalReqFDOS.Enabled = false;
            txtDentalReqTDOS.Enabled = false;
            txtDentalServTrackingNo.Enabled = false;
        }
        else
        {
            // btnServDentalUpdate.Visible = true;
            txtDentalSDProcCode.Enabled = true;
            txtDentalReqUnits.Enabled = true;
            txtDentalProcCodeDescription.Enabled = true;
            txtDentalReqDollars.Enabled = true;
            txtDentalProvServnote.Enabled = true;
            txtDentalReqFDOS.Enabled = true;
            txtDentalReqTDOS.Enabled = true;
            txtDentalServTrackingNo.Enabled = true;
        }
    }

    private void InstitutionalServiceReset(bool disabled)
    {
        if (disabled)
        {
            txtServicecode.Style.Add("background-color", "lightgray");
            txtServicecode.Enabled = false;

            txtLnProcedureCode.Style.Add("background-color", "lightgray");
            txtLnProcedureCode.Enabled = false;

            txtRequestUnt.Style.Add("background-color", "lightgray");
            txtRequestUnt.Enabled = false;

            txtLnprocCodeDesc.Style.Add("background-color", "lightgray");
            txtLnprocCodeDesc.Enabled = false;

            txtLnProviderServiceNote.Style.Add("background-color", "lightgray");
            txtLnProviderServiceNote.Enabled = false;

            txtLnServiceTrackingNo.Style.Add("background-color", "lightgray");
            txtLnServiceTrackingNo.Enabled = false;

            txtRequestedDollars.Style.Add("background-color", "lightgray");
            txtRequestedDollars.Enabled = false;

            txtReqFDOS.Style.Add("background-color", "lightgray");
            txtReqFDOS.Enabled = false;

            txtReqTDOS.Style.Add("background-color", "lightgray");
            txtReqTDOS.Enabled = false;

            //  btnServDetailsUpdateAdd.Visible = false;

            lnkPlaceofServiceSearchDetail1.Enabled = false;

            lnkServiceCode.Enabled = false;

            ddlServiceTypeCode.Enabled = false;

            ddLnRequestUnits.Enabled = false;

            ddLnLevelOfCare.Enabled = false;
        }
        else
        {
            //   btnServDetailsUpdateAdd.Visible = true;
            txtServicecode.Enabled = true;
            txtLnProcedureCode.Enabled = true;
            txtRequestUnt.Enabled = true;
            txtLnprocCodeDesc.Enabled = true;
            txtLnProviderServiceNote.Enabled = true;
            txtLnServiceTrackingNo.Enabled = true;
            txtRequestedDollars.Enabled = true;
            txtReqTDOS.Enabled = true;
            lnkPlaceofServiceSearchDetail1.Enabled = true;
            lnkServiceCode.Enabled = true;
            ddlServiceTypeCode.Enabled = true;
            ddLnRequestUnits.Enabled = true;
            ddLnLevelOfCare.Enabled = true;
        }
    }

    private void ProfessionalServiceReset(bool disabled)
    {
        if (disabled)
        {
            txtProfessionalSDProcCode.Style.Add("background-color", "lightgray");
            txtProfessionalSDProcCode.Enabled = false;

            txtProfessionalProcCodeDescription.Style.Add("background-color", "lightgray");
            txtProfessionalProcCodeDescription.Enabled = false;

            txtModifier1.Style.Add("background-color", "lightgray");
            txtModifier1.Enabled = false;

            txtModifier2.Style.Add("background-color", "lightgray");
            txtModifier2.Enabled = false;

            txtModifier3.Style.Add("background-color", "lightgray");
            txtModifier3.Enabled = false;

            txtModifier4.Style.Add("background-color", "lightgray");
            txtModifier4.Enabled = false;

            txtProfessionalReqUnits.Style.Add("background-color", "lightgray");
            txtProfessionalReqUnits.Enabled = false;

            txtProfessionalReqDollars.Style.Add("background-color", "lightgray");
            txtProfessionalReqDollars.Enabled = false;

            txtProfessionalReqFDOS.Style.Add("background-color", "lightgray");
            txtProfessionalReqFDOS.Enabled = false;

            txtProfessionalReqTDOS.Style.Add("background-color", "lightgray");
            txtProfessionalReqTDOS.Enabled = false;

            txtProfessionalServTrackingNo.Style.Add("background-color", "lightgray");
            txtProfessionalServTrackingNo.Enabled = false;

            //btnServProfessionalUpdate.Visible = false;

            lnkProfessionalSDProcCodeSearchLink.Enabled = false;

            ddProffSDMeasurements.Enabled = false;
        }
        else
        {
            // btnServProfessionalUpdate.Enabled = true;
            txtProfessionalSDProcCode.Enabled = true; ;
            txtProfessionalProcCodeDescription.Enabled = true;
            txtModifier1.Enabled = true;
            txtModifier2.Enabled = true;
            txtModifier3.Enabled = true;
            txtModifier4.Enabled = true;
            txtProfessionalProvServnote.Enabled = true;
            txtProfessionalReqUnits.Enabled = true;
            txtProfessionalReqDollars.Enabled = true;
            txtProfessionalReqFDOS.Enabled = true;
            txtProfessionalReqTDOS.Enabled = true;
            txtProfessionalServTrackingNo.Enabled = true;
            lnkProfessionalSDProcCodeSearchLink.Enabled = true;
            ddProffSDMeasurements.Enabled = true;
        }
    }

    public static string CreateAndReturnLogThreadNumber(Exception ex, string errorKey = "")
    {
        string logid = HttpContext.Current.Session["LogKey"] != null ? HttpContext.Current.Session["LogKey"].ToString() : CON.appAdminUserId;
        Logging logging = new Logging(new Guid(logid));
        string logMessage = errorKey + " " + logging.GetRecursiveException(ex);
        logging.CreateLogEntry(logMessage, CON.WebPageProcessName.SubmitPriorAuthorization);
        return logging.ThreadId.ToString();
    }

    public static string CreateAndReturnLogInfoThreadNumber(string logMessage = "")
    {
        string logid = HttpContext.Current.Session["LogKey"] != null ? HttpContext.Current.Session["LogKey"].ToString() : CON.appAdminUserId;
        Logging logging = new Logging(new Guid(logid));
        logging.CreateLogEntry(logMessage, CON.WebPageProcessName.SubmitPriorAuthorization);
        return logging.ThreadId.ToString();
    }

    public List<PriorAuthAttachment> ConvertDataTableToList(DataTable dt)
    {
        var attachments = new List<PriorAuthAttachment>();

        if (dt == null || dt.Rows.Count == 0)
            return attachments;

        foreach (DataRow row in dt.Rows)
        {
            var attachment = new PriorAuthAttachment
            {

                PRIOR_AUTH_SUB_DOCUMENT_TYPE_ID = GetOriginalValue<int>(row, "PRIOR_AUTH_SUB_DOCUMENT_TYPE_ID"),
                PRIOR_AUTH_SUB_TRACKING_NUMBER = GetOriginalValue<string>(row, "PRIOR_AUTH_SUB_TRACKING_NUMBER"),
                PRIOR_AUTH_SUB_DOCUMENT_ID = GetOriginalValue<string>(row, "PRIOR_AUTH_SUB_DOCUMENT_ID"),
                PRIOR_AUTH_SUB_Note = GetOriginalValue<string>(row, "PRIOR_AUTH_SUB_Note"),
                PRIOR_AUTH_SUB_ATTACHMENT_AUTH_TYPE = GetOriginalValue<int>(row, "PRIOR_AUTH_SUB_ATTACHMENT_AUTH_TYPE"),
                PRIOR_AUTH_SUB_DOCUMENT_TYPE_DESC = GetOriginalValue<string>(row, "PRIOR_AUTH_SUB_DOCUMENT_TYPE_DESC"),
                DOCUMENT_ID = GetOriginalValue<string>(row, "DOCUMENT_ID"),
                PRIOR_AUTH_SUB_ATTACHMENT_STATUS = GetOriginalValue<string>(row, "PRIOR_AUTH_SUB_ATTACHMENT_STATUS"),
                MedicaidId = GetOriginalValue<string>(row, "MedicaidId"),
                DOCUMENT_NAME = GetOriginalValue<string>(row, "DOCUMENT_NAME"),
                ORIGINAL_DOCUMENT_NAME = GetOriginalValue<string>(row, "ORIGINAL_DOCUMENT_NAME"),
                PRIOR_AUTH_SUB_ATTACHMENT_ID = GetOriginalValue<int>(row, "PRIOR_AUTH_SUB_ATTACHMENT_ID")

            };

            attachments.Add(attachment);
        }

        return attachments;
    }

    public T GetOriginalValue<T>(DataRow row, string columnName)
    {
        return row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
            ? (T)Convert.ChangeType(row[columnName], typeof(T))
            : default(T); // Safer approach for older C# versions
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {

        pnlRecipient.Enabled = true;
        pnlContact.Enabled = true;
        pnlServiceInformation.Enabled = true;
        pnlTrackingNumber.Enabled = true;
        pnlService.Enabled = true;
        pnlServiceProviderInfo.Enabled = true;
        pnlorderproviderinfo.Enabled = true;
        pnlDiagnosisLine.Enabled = true;
        pnlCertHospital.Enabled = true;
        pnlProviderNote.Enabled = true;
        pnlProvidermainPnl.Enabled = true;
        pnlOutcomeOfReview.Enabled = true;
        pnlAttachment.Enabled = true;
        pnlDentalAttachment.Enabled = true;
        pnlmissingtooth.Enabled = true;
        pnlDocumentbyMail.Enabled = true;
        pnlreviewernoteprovider.Enabled = true;
        pnlReasonforDenial.Enabled = true;
        Panel1.Enabled = true;
        btnSubmit.Visible = false;
        btnCancelPARequest.Visible = false;
        //btnCancel.Visible = false;

        //gvDiagnosis.Enabled = true;
        btnDiagnosisAdd.Enabled = true;
        //gvServiceDetailDental.Enabled = true;
        btnDentalServiceDetailAdd1.Visible = true;

        //btnClearAll.Visible = true;
        btnReSubmit.Enabled = true;
        btnCancel_Revert.Enabled = true;
        //btnEdit.Enabled = false;


    }
}