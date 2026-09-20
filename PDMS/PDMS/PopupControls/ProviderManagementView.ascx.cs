using Corp.Core.Libraries;
using Corp.Core.Libraries.AttachmentServiceReference;
using Corp.Core.Libraries.ClaimsReference;
using Corp.Core.Libraries.Helper;
using DocumentFormat.OpenXml.Office.PowerPoint.Y2021.M06.Main;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;
using MessageHeader = Corp.Core.Libraries.AttachmentServiceReference.MessageHeader;


public partial class UserControls_ProviderManagementView : System.Web.UI.UserControl, IProviderManagementDetailsView
{
    int elapsedDays;
    protected int _MaxFileMegaBytes;
    private string _DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
    private string _ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
    private PDMSService.PDMSServiceClient _svc;
    public string PaymentInnovationsProviderId = "";
    public string RecipientEligibilityMITSProviderId = "";
    private int RegistrationStatusId, RegProgramStatusTypeId;
    private string RegistrationStatusType;
    private int reg_id;
    private string CPC_Program_Year = AppSettings.Get("CPCProgramYear");
    private static bool IsAddODMorODAMedicaidSvc = false;
    private string AdditionalApplicationStatusID = "";
    private string AdditionalApplicationTypeID = "";
    private bool enableCR537 = Convert.ToBoolean(AppSettings.Get("EnableCR537", "false"));
    private bool IsPowerAgent = false; // SAM768
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
    public int MaxFileMegaBytes
    {
        get
        {
            if (_MaxFileMegaBytes == 0) _MaxFileMegaBytes = 80;       // Default is 5MB
            return _MaxFileMegaBytes;
        }
        set
        {
            _MaxFileMegaBytes = value;
            if (_MaxFileMegaBytes > 80) _MaxFileMegaBytes = 80;
        }
    }
    private class WorkflowLinkOptions
    {
        public const string BeginUpdateRegistration = "BeginUpdateRegistration";
        public const string BeginUpdateGMPRegistration = "BeginUpdateGMPRegistration";
        public const string BeginUpdateOwnership = "BeginUpdateOwnership";
        public const string BeginUpdateServices = "BeginUpdateServices";
        public const string BeginAddAffiliation = "BeginAddAffiliation";
        public const string BeginRevalidation = "BeginRevalidation";
        public const string BeginReactivation = "BeginReactivation";
        public const string BeginReapplication = "BeginReapplication";

        public const string ContinueNewRegistration = "ContinueNewRegistration";
        public const string ContinueUpdateRegistration = "ContinueUpdateRegistration";
        public const string ContinueUpdateOwnership = "ContinueUpdateOwnership";
        public const string ContinueAddAffiliation = "ContinueAddAffiliation";
        public const string ContinueWaiverServices = "ContinueWaiverServices";
        public const string ContinueRevalidation = "ContinueRevalidation";
        public const string ContinueReactivateEnrollment = "ContinueReactivateEnrollment";

        public const string CancelUpdateRegistration = "CancelUpdateRegistration";
        public const string CancelUpdateOwnership = "CancelUpdateOwnership";
        public const string CancelAddAffiliation = "CancelAddAffiliation";
        public const string CancelWaiverServices = "CancelWaiverServices";
        public const string CancelRevalidation = "CancelRevalidation";
        public const string CancelReactivateEnrollment = "CancelReactivateEnrollment";
        public const string CancelNewRegistration = "CancelNewRegistration";
        public const string CancelReapplication = "CancelReapplication";
        public const string CancelCPCRegistration = "CancelCPCRegistration";
    }

    #region Properties
    private ProviderManagementDetailsPresenter _presenter;
    private bool IsRevalidation = false;

    public ProviderManagementDetailsPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new ProviderManagementDetailsPresenter(this);
            }

            return _presenter;
        }
    }

    public ProviderManagementData Model { get; set; }

    [SerializableAttribute()]
    public class RegKeyFields
    {
        public int RegID { get; set; }
        public int ReferralID { get; set; }
        public int ReferralTypeID { get; set; }
        public int WorkflowID { get; set; }
        public int PaperRequestQueueID { get; set; }
        public int ProcessID { get; set; }
        public int CurrentStepID { get; set; }
        public int CurrentTaskID { get; set; }
        public string CurrentTaskClassName { get; set; }
        public string FormCompletionName { get; set; }
        public string FormCompletionPhone { get; set; }
        public DateTime? ChangeEffectiveDate { get; set; }
        public DateTime? RequestedEffectiveDate { get; set; }
        public string MedicaidID { get; set; }
        public int RegistrationProgramStatusID { get; set; }
        public int ApplicationTypeID { get; set; }
        public int ProviderCategoryTypeID { get; set; }
        public int ProviderTypeID { get; set; }
        public DateTime? RevalidationDate { get; set; }
        public bool IsTerminated { get; set; }
        public DateTime? EndDate { get; set; }
        public string EnrollmentStatusCode { get; set; }
        public int WorkflowEventType { get; set; }

        public int WaiverTypeID { get; set; }
        public bool IsCredentialingProvider { get; set; }
        public string EnrollmentStatusReason { get; set; }
        public string MMISCPCPracticeTypeID { get; set; }

        public string NPI { get; set; }


        public RegKeyFields(int regID, int referralID, int workflowID, int paperRequestQueueID, int processID, int stepID, int taskID, string taskClassName, string completedBy, string phoneNbr,
            DateTime? changeEffectiveDate, DateTime? requestedEffectiveDate, int referralTypeID, string medicaidID, int registrationProgramStatusID, int applicationTypeID, int providerCategoryTypeID,
            int providerTypeID, DateTime? revalidationDate, bool isTerminated, DateTime? endDate, string enrollmentStatusCode, int workflowEventType, int waiverTypeID, bool isCredentialingProvider,
            string enrollmentStatusReason, string MMIScpcPracticeTypeID, string npi)
            : base()
        {
            RegID = regID;
            ReferralID = referralID;
            WorkflowID = workflowID;
            PaperRequestQueueID = paperRequestQueueID;
            ProcessID = processID;
            CurrentStepID = stepID;
            CurrentTaskID = taskID;
            CurrentTaskClassName = taskClassName; //page to open to
            FormCompletionName = completedBy;
            FormCompletionPhone = phoneNbr;
            if (changeEffectiveDate.HasValue)
                ChangeEffectiveDate = changeEffectiveDate.Value;
            if (requestedEffectiveDate.HasValue)
                RequestedEffectiveDate = requestedEffectiveDate.Value;
            ReferralTypeID = referralTypeID;
            MedicaidID = medicaidID;
            RegistrationProgramStatusID = registrationProgramStatusID;
            ApplicationTypeID = applicationTypeID;
            ProviderCategoryTypeID = providerCategoryTypeID;
            ProviderTypeID = providerTypeID;
            if (revalidationDate.HasValue)
                RevalidationDate = revalidationDate;
            IsTerminated = isTerminated;
            if (endDate.HasValue)
                EndDate = endDate;
            if (!string.IsNullOrEmpty(enrollmentStatusCode))
                EnrollmentStatusCode = enrollmentStatusCode;

            WorkflowEventType = workflowEventType;
            WaiverTypeID = waiverTypeID;
            IsCredentialingProvider = isCredentialingProvider;
            EnrollmentStatusReason = enrollmentStatusReason;
            MMISCPCPracticeTypeID = MMIScpcPracticeTypeID;
            NPI = npi;
        }
    }

    public RegKeyFields KeyFields
    {
        get
        {
            return ViewState["KeyFields"] == null ? null : ViewState["KeyFields"] as RegKeyFields;
        }
        set
        {
            ViewState["KeyFields"] = value;
        }
    }

    private int ReferralID
    {
        get
        {
            return ViewState["ReferralID"] == null ? 0 : (int)ViewState["ReferralID"];
        }
        set
        {
            ViewState["ReferralID"] = value;
        }
    }

    private int ReferralTypeID
    {
        get
        {
            return ViewState["ReferralTypeID"] == null ? 0 : (int)ViewState["ReferralTypeID"];
        }
        set
        {
            ViewState["ReferralTypeID"] = value;
        }
    }

    private int PaperRequestQueueID
    {
        get
        {
            return ViewState["PaperRequestQueueID"] == null ? 0 : (int)ViewState["PaperRequestQueueID"];
        }
        set
        {
            ViewState["PaperRequestQueueID"] = value;
        }
    }

    private int RegID
    {
        get
        {
            return ViewState["RegID"] == null ? 0 : (int)ViewState["RegID"];
        }
        set
        {
            ViewState["RegID"] = value;
        }
    }
    private int RevalDueWindow
    {
        get
        {
            return ViewState["RevalDueWindow"] == null ? 0 : (int)ViewState["RevalDueWindow"];
        }
        set
        {
            ViewState["RevalDueWindow"] = value;
        }
    }

    private bool IsNewRegistration
    {
        get
        {
            return string.IsNullOrEmpty(KeyFields.MedicaidID) ? true : false;
        }
    }
    private string MMISProviderTypeId
    {
        get
        {
            return ViewState["MMISProviderTypeId"] == null ? string.Empty : (string)ViewState["MMISProviderTypeId"];
        }
        set
        {
            ViewState["MMISProviderTypeId"] = value;
        }
    }
    private bool HasActiveODASpecialty = false;
    private bool HasActiveDODDSpecialty = false;
    private bool HasDODDSpecialty = false;
    private bool HasActiveODMSpecialty = false;
    private bool HasODMSpecialty = false;
    private bool HasODASpecialty = false;
    private bool HasDDContractNum = false;
    #endregion


    #region Parent Page Events
    public delegate void EditProviderKeyFieldsEventHandler(ProviderManagerData providerData);
    public event EditProviderKeyFieldsEventHandler EditProviderKeyFieldsEvent;

    public delegate void CancelWorkflowEventHandler();
    public event CancelWorkflowEventHandler CancelWorkflowEvent;

    public delegate void CancelExistingServicesEventHandler();
    public event CancelExistingServicesEventHandler CancelExistingServicesEvent;

    public delegate void ErrorEventHandler(Dictionary<string, string> lstErrors);
    public event ErrorEventHandler ErrorEvent;

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void KeepOpenEventHandler();
    public event KeepOpenEventHandler KeepOpenEvent;

    public delegate void AddODMorODAMedicaidEventHandler(ProviderManagerData providerData);
    public event AddODMorODAMedicaidEventHandler AddODMorODAMedicaidEvent;
    public delegate void ProviderTypeChangeHandler(ProviderManagerData providerData);
    public event ProviderTypeChangeHandler ProviderTypeChangeEvent;
    #endregion


    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        Session["IsPreviousPageClicked"] = "true";
        Response.Redirect("~/Process/ProviderHomeNew.aspx");
       
    }
    #region Page Events

    protected void Page_PreRender(object sender, EventArgs e)//OHPNM-3551
    {
        SetupButtonDisablesOfMultiClick();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        // OHPNM-8164 - if this user is a provider administrator, let's see if they should be looking at this registration
        if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAdministrator) || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent))
        {
            // see if they can access this registration
            bool UserCanAccessReg = svc.UserCanAccessReg(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), this.KeyFields.RegID);
            if (!UserCanAccessReg)
            {
                // they shouldn't be here; kick em out
                Response.Redirect("~/Process/ProviderHomeNew.aspx");
            }
        }
        bool enableCR629 = Convert.ToBoolean(AppSettings.Get("EnableCR629", "false"));
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.CheckIfCPCPrimary(this.KeyFields.RegID);
        if(enableCR629 && Helper.HasRows(ds) && (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name,CON.ProviderAdministratorRole) ||
            Helper.IsUserInSubRoles(this.KeyFields.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.CPCAgentSubRole)))
        {
            divCPCUpload.Visible = true;
        }
        else
        {
            divCPCUpload.Visible = false;
        }
        IsPowerAgent = Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, this.KeyFields.RegID);
        if (!IsPostBack)
        {
            DisplayElapsedDays();
            AddDisEntrollmentOptions();
            SetDODDVisibility();
            presenter.Init();
            EnableDisableToolTip();
            LoadCurrentAndPreviousApplicationData(this.KeyFields.RegID);
            LoadRequestReConsiderationDetails(this.KeyFields.RegID);
            Displayerror();
            lnkContinueUpdateRegistration.Attributes.Add("onClick", "return false;");

            txtDateofReconsidertaionRequest.Text = DateTime.Now.ToShortDateString();
            
            /*DataSet dataSet = svc.GetCPCAttachmentsDocType();
            if (Helper.HasRows(dataSet))
            {
                DataTable dt = dataSet.Tables[0];

                Helper.LoadDropDown(ddlTypeID, dt, "DOCUMENT_TYPE_DESC", "DOCUMENT_TYPE_ID", false);



            }*/
            
        }
        Session["ViewProviderFile"] = false;
    }
    private void Displayerror()
    {
        lblErrorNPI.Text = Helper.HtmlEncode(ValidationConstants.ProviderManagerData.NoNPIOrTaxonomyFound);
    }
    private void LoadRequestReConsiderationDetails(int regID)
    {
        txtDateofReconsidertaionRequest.Text = string.Empty;
    }
    private void LoadCurrentAndPreviousApplicationData(int regID)
    {
        DataSet ds = svc.SelectCurrentAndPreviousApplicationsByRegID(regID);
        if (ds != null && ds.Tables.Count > 0)
        {
            DataTable dtAppdetails = ds.Tables[0];

            /*
            so for call reg_applications except for the last one, 
            if "accepted" we show "Approved / Complete"
            if "denied" we show "Denied"
            if "not processed" we show "cancelled"

            for latest application, if app status is not processed, and WF is ended, we can show cancelled
            if wf is not complete and reg status is approved; show as processing
            if wf is complete, show what final reg_status is
            */

            if (dtAppdetails.Rows.Count > 0)
            {
                for (int rowCnt = 0; rowCnt < dtAppdetails.Rows.Count; rowCnt++)
                {
                    DataRow currentRow = dtAppdetails.Rows[rowCnt];

                
                    if (rowCnt == dtAppdetails.Rows.Count - 1)
                    {
                        // this is the last REG_APPLICATION; use the status of the REGISTRATION instead of the REG_APPLICATION data
                        // for latest application, if app status is not processed, and WF is ended, we can show cancelled
                        // if wf is not complete and reg status is approved; show as processing
                        // if wf is complete, show what final reg_status is

                        // System.Diagnostics.Debug.WriteLine("on final row: " + currentRow["PNMApplicationStatus"].ToString() + "/" + currentRow["WorkflowComplete"].ToString());
                        if (currentRow["WorkflowComplete"].ToString().Equals("N")) {
                            // workflow is not complete; if registration status is 'Approved,' assume we are still processing it
                            if (this.RegistrationStatusId == CON.RegistrationStatusTypeId.Approved)
                            {
                                currentRow["PNMApplicationStatus"] = "Processing";
                            }
                            else
                            {
                                // shouldn't be here, but in case we are, just show registration status
                                currentRow["PNMApplicationStatus"] = this.RegistrationStatusType;
                            }
                        }
                        else
                        {
                            // workflow is complete
                            // if REG_APPLICATION is "NOT PROCESSED", show cancelled
                            if (currentRow["PNMApplicationStatus"].ToString().Equals("NOT PROCESSED"))
                            {
                                
                                    currentRow["PNMApplicationStatus"] = "Cancelled";
                            }
                            else if(currentRow["PNMApplicationStatus"].ToString().Equals("DENIED"))
                            {
                                if (((this.RegistrationStatusId == CON.RegistrationStatusTypeId.NotProcessed || this.RegProgramStatusTypeId == CON.RegistrationProgramStatusTypeId.NotProcessed)
                                    && currentRow["REGISTRATION_STATUS_TYPE_ID"].ToString() == CON.RegistrationStatusTypeId.NotProcessed.ToString()) ||
                                     (currentRow["Program"].ToString().Equals("ODA") && currentRow["OtherAgencyApplicationStatus"].ToString().Equals("Recommended Certification") && currentRow["REG_PROGRAM_STATUS_TYPE_ID"].ToString() == CON.RegistrationProgramStatusTypeId.NotProcessed.ToString())
                                    || (currentRow["Program"].ToString().Equals("DD") && currentRow["OtherAgencyApplicationStatus"].ToString().Equals("Closed By ODM") && currentRow["REG_PROGRAM_STATUS_TYPE_ID"].ToString() == CON.RegistrationProgramStatusTypeId.NotProcessed.ToString()))
                                {
                                    currentRow["PNMApplicationStatus"] = "Not Processed";
                                }

                                else
                                    currentRow["PNMApplicationStatus"] = this.RegistrationStatusType;
                            }
                            // if REG_APPLICATION is "APPROVED" show "Approved / Complete"
                            else if (currentRow["PNMApplicationStatus"].ToString().Equals("ACCEPTED"))
                            {
                                currentRow["PNMApplicationStatus"] = "Approved / Complete";
                            }
                            else
                            {
                                // otherwise, just show final registration status
                                currentRow["PNMApplicationStatus"] = this.RegistrationStatusType;
                            }
                        }
                    }
                    else
                    {
                        // this isn't the very latest REG_APPLICATION, so infer the status from the REG_APPLICATION status
                        // REG_APPLICATION status "ACCEPTED" should be "Approved / Complete"
                        // REG_APPLICATION status "DENIED" should be "Denied"
                        // REG_APPLICATION status "NOT PROCESSED" should be "Cancelled"
                        // System.Diagnostics.Debug.WriteLine("on early row: " + currentRow["PNMApplicationStatus"].ToString());
                        if (currentRow["PNMApplicationStatus"].ToString().Equals("DENIED"))
                        {
                            if (((this.RegistrationStatusId == CON.RegistrationStatusTypeId.NotProcessed || this.RegProgramStatusTypeId == CON.RegistrationProgramStatusTypeId.NotProcessed)
                                    && currentRow["REGISTRATION_STATUS_TYPE_ID"].ToString() == CON.RegistrationStatusTypeId.NotProcessed.ToString()) ||
                                     (currentRow["Program"].ToString().Equals("ODA") && currentRow["OtherAgencyApplicationStatus"].ToString().Equals("Recommended Certification") && currentRow["REG_PROGRAM_STATUS_TYPE_ID"].ToString() == CON.RegistrationProgramStatusTypeId.NotProcessed.ToString())
                                    || (currentRow["Program"].ToString().Equals("DD") && currentRow["OtherAgencyApplicationStatus"].ToString().Equals("Closed By ODM") && currentRow["REG_PROGRAM_STATUS_TYPE_ID"].ToString() == CON.RegistrationProgramStatusTypeId.NotProcessed.ToString()))
                            {
                                currentRow["PNMApplicationStatus"] = "Not Processed";
                            }

                            else
                                currentRow["PNMApplicationStatus"] = "Denied";
                        }
                        else if (currentRow["PNMApplicationStatus"].ToString().Equals("ACCEPTED"))
                        {
                            currentRow["PNMApplicationStatus"] = "Approved / Complete";
                        }
                        else if (currentRow["PNMApplicationStatus"].ToString().Equals("NOT PROCESSED"))
                        {                           
                             currentRow["PNMApplicationStatus"] = "Cancelled";
                        }

                    }
                }
            }
            grdCurrrentandPrev.DataSource = dtAppdetails;
            grdCurrrentandPrev.DataBind();
        }
    }
    private void EnableDisableToolTip()
    {

        string UpdateRegistrationtoolTip = string.Empty;

        if ((HasActiveODASpecialty || HasActiveODMSpecialty) && !HasActiveDODDSpecialty)
        {
            UpdateRegistrationtoolTip = UpdateRegistrationtoolTip + (string)GetGlobalResourceObject("BrandingResource", "ODM_UPDATE_TOOLTIP");
        }
        if (HasActiveDODDSpecialty)
        {
            UpdateRegistrationtoolTip = (!string.IsNullOrEmpty(UpdateRegistrationtoolTip) ? UpdateRegistrationtoolTip + Environment.NewLine : "")
                                        + (string)GetGlobalResourceObject("BrandingResource", "EXCLUDING_OWNERSHIP_TOOLTIP");
        }
        int revalHelpTextWindow = Convert.ToInt32(AppSettings.Get("RevalidationHelpTextWindow")) * -1;
        if (this.KeyFields.EndDate.HasValue && this.KeyFields.EndDate.Value.AddDays(revalHelpTextWindow) <= DateTime.Today)
        {
            UpdateRegistrationtoolTip = (!string.IsNullOrEmpty(UpdateRegistrationtoolTip) ? UpdateRegistrationtoolTip + Environment.NewLine : "")
                                       + (string)GetGlobalResourceObject("BrandingResource", "REVALIDATION_DUE_HELP_TEXT");
        }
        lnkUpdateRegistration.ToolTip = UpdateRegistrationtoolTip;

        UpdateRegistrationtoolTip = string.Empty;
        UpdateRegistrationtoolTip = UpdateRegistrationtoolTip + (string)GetGlobalResourceObject("BrandingResource", "DODD_PSM_TOOLTIP");

        if (this.KeyFields.EndDate.HasValue && this.KeyFields.EndDate.Value.AddDays(revalHelpTextWindow) <= DateTime.Today)
        {
            UpdateRegistrationtoolTip = (!string.IsNullOrEmpty(UpdateRegistrationtoolTip) ? UpdateRegistrationtoolTip + Environment.NewLine : "")
                                       + (string)GetGlobalResourceObject("BrandingResource", "REVALIDATION_DUE_HELP_TEXT");
        }
        lnkBeginDODDUpdate.ToolTip = UpdateRegistrationtoolTip;

        UpdateRegistrationtoolTip = string.Empty;
        if (this.KeyFields.EndDate.HasValue && this.KeyFields.EndDate.Value.AddDays(revalHelpTextWindow) <= DateTime.Today)
        {
            UpdateRegistrationtoolTip = (!string.IsNullOrEmpty(UpdateRegistrationtoolTip) ? UpdateRegistrationtoolTip + Environment.NewLine : "")
                                       + (string)GetGlobalResourceObject("BrandingResource", "REVALIDATION_DUE_ODA_HELP_TEXT");
        }
        lnkAddODAServices.ToolTip = UpdateRegistrationtoolTip;

        UpdateRegistrationtoolTip = string.Empty;
        if (MMISProviderTypeId == CON.MMISProviderType.Licensee || MMISProviderTypeId == CON.MMISProviderType.Operator
            || MMISProviderTypeId == CON.MMISProviderType.Unpaid_Support_Broker || MMISProviderTypeId == CON.MMISProviderType.Supported_Living)
        {
            UpdateRegistrationtoolTip = (!string.IsNullOrEmpty(UpdateRegistrationtoolTip) ? UpdateRegistrationtoolTip + Environment.NewLine : "")
                                       + (string)GetGlobalResourceObject("BrandingResource", "NONMED_DODD_UPDATE_TOOLTIP");
        }
        lnkAddODMorODAMedSvc.ToolTip = UpdateRegistrationtoolTip;

        if (HasDODDSpecialty && !HasODMSpecialty)
        {
            lnkBtnRemittanceRedirection.ToolTip = (string)GetGlobalResourceObject("BrandingResource", "RemittanceAdvice_HELP_TEXT");
            lnkClaims.ToolTip = (string)GetGlobalResourceObject("BrandingResource", "Claims_DODD_HELP_TEXT");
            lnkBeginReapplication.ToolTip = (string)GetGlobalResourceObject("BrandingResource", "DODD_REAPPLICATION_TOOLTIP");
        }
    }
    private string GetMMISProviderTypeID(int providerTypeId)
    {
        string rtn = "";
        DataSet ds = svc.GetProviderTypeById(providerTypeId);
        if (Methods.HasRows(ds))
        {
            rtn = Methods.GetStringValue(ds.Tables[0].Rows[0], "MMIS_PROVIDER_TYPE_ID");
        }
        return rtn;
    }

    protected void lnkContinueWaiverWorkflow_Click(object sender, EventArgs e)
    {
        if (this.KeyFields == null || this.KeyFields.WaiverTypeID == CON.WaiverApplicationTypeID.ODM)
        {
            presenter.ErrorList.Add("Error", ValidationConstants.ProviderManagerData.RouteToUrlNotSpecified);
            this.SetErrorMessages();
            return;
        }

        DataSet ds = svc.SelectRegistrationByRegID(this.KeyFields.RegID);
        string ddContractNum = string.Empty;
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddContractNum = ds.Tables[0].Rows[0]["dd_contract_number"].ToString();
        }

        string Reg64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.KeyFields.RegID.ToString()));
        string Cnt64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ddContractNum));
        string Src64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("pnm"));
        string Tgt64 = "";
        string url = "";

        if (this.KeyFields.WaiverTypeID == CON.WaiverApplicationTypeID.ODA)
        {
            url = AppSettings.Get("ODA-Application-RedirectURL");  // https://identifier.devapps.dodd.ohio.gov/?rgd=###_RGD_B64_###&ctn=###_CTN_B64_###&src=###_SRC_B64_###&tgt=###_TGT_B64_###
            Tgt64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("oda"));
        }
        else
        {
            url = AppSettings.Get("DODD-Application-RedirectURL");  // https://identifier.qaapps.dodd.ohio.gov/?rgd=###_RGD_B64_###&ctn=###_CTN_B64_###&src=###_SRC_B64_###&tgt=###_TGT_B64_###
            Tgt64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("psm"));
        }

        url = url.Replace("###_RGD_B64_###", Reg64);
        url = url.Replace("###_CTN_B64_###", Cnt64);
        url = url.Replace("###_SRC_B64_###", Src64);
        url = url.Replace("###_TGT_B64_###", Tgt64);
        Response.Redirect(url);
    }
    protected void lnkContinueWorkflow_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        if (this.KeyFields == null || string.IsNullOrWhiteSpace(this.KeyFields.CurrentTaskClassName))
        {
            presenter.ErrorList.Add("Error", ValidationConstants.ProviderManagerData.RouteToUrlNotSpecified);
            this.SetErrorMessages();
            return;
        }
        (this.Page as RegistrationProvider).RegistrationId = this.KeyFields.RegID;
        (this.Page as RegistrationProvider).IsReadOnly = false;

        if (this.KeyFields.CurrentTaskClassName.Contains("Step="))
        {
            string queryString = this.KeyFields.CurrentTaskClassName.Split('?')[1];
            var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
            (this.Page as RegistrationProvider).RegistrationStep = int.Parse(queryDictionary["Step"]);
        }
        if (validateNPI(KeyFields.RegID))
        {

            Response.Redirect("~/process/Registration.aspx?RegId=" + KeyFields.RegID);
        }
        else
        {
            Response.Redirect("~/Process/NewProvider.aspx?EditKeyFieldDataRequest=true&RegID=" + KeyFields.RegID.ToString());
        }
    }

    protected void lnkContinueUpdateWorkflow_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        if (validateNPI(KeyFields.RegID))
        {
            btnContinueUpdateOk.Visible = true;
            mpeContinueUpdate.Show();
        }
        else
        {
            mpeError.Show();
        }
    }
    protected void lnkAddODMorODAMedSvc_Click(object sender, EventArgs e)
    {
        if (AddODMorODAMedicaidEvent != null)
        {
            //set key fields needed, call event to load pop up.
            var keyData = new ProviderManagerData
            {
                RegID = this.KeyFields.RegID,
                WorkflowIDRequested = KeyFields.WorkflowID,
                ReferralID = KeyFields.ReferralID,
                ReferralTypeID = KeyFields.ReferralTypeID,
                IsAddODMorODAMedicaid = true,
                KeyFieldEditRequest = false,
                RegistrationProgramStatusTypeID = KeyFields.RegistrationProgramStatusID,
                ApplicationTypeID = KeyFields.ApplicationTypeID,
                WaiverTypeId = KeyFields.WaiverTypeID
            };
            AddODMorODAMedicaidEvent(keyData);
        }
    }
    protected void btnContinueUpdateOk_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        if (this.KeyFields == null || string.IsNullOrWhiteSpace(this.KeyFields.CurrentTaskClassName))
        {
            presenter.ErrorList.Add("Error", ValidationConstants.ProviderManagerData.RouteToUrlNotSpecified);
            this.SetErrorMessages();
            return;
        }

        (this.Page as RegistrationProvider).RegistrationId = this.KeyFields.RegID;
        (this.Page as RegistrationProvider).IsReadOnly = false;

        if (this.KeyFields.CurrentTaskClassName.Contains("Step="))
        {
            string queryString = this.KeyFields.CurrentTaskClassName.Split('?')[1];
            var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
            (this.Page as RegistrationProvider).RegistrationStep = int.Parse(queryDictionary["Step"]);
        }

        if (IsAddODMorODAMedicaidSvc)
            Response.Redirect("~/process/Registration.aspx?RegId=" + KeyFields.RegID);
        else
            Response.Redirect("~/Process/ProviderUpdateSummary.aspx?RegId=" + this.KeyFields.RegID);
    }

    protected void btnUpdateOk_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        int workflowID = 0;
        bool reValFlag = (this.KeyFields.ApplicationTypeID == CON.ApplicationType.ChangeOfOperator) ? true : false;

        workflowID = svc.GetWorkflowInstance(KeyFields.ApplicationTypeID, KeyFields.ProviderCategoryTypeID,
                    KeyFields.ProviderTypeID, KeyFields.ReferralTypeID, reValFlag, false, false, false);

        BeginUpdateRegistration(workflowID, false, false, false, false, CON.WaiverServiceUpdateType.ODM);

        // allowing Conversion without NPI providers to begin update workflow
        if (this.KeyFields.ApplicationTypeID == CON.ApplicationType.Waiver &&
           this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Conversion)
        {
            Response.Redirect("~/Process/ProviderUpdateSummary.aspx?RegId=" + this.KeyFields.RegID);
            return;
        }
        if (validateNPI(this.KeyFields.RegID))
        {
            Response.Redirect("~/Process/ProviderUpdateSummary.aspx?RegId=" + this.KeyFields.RegID);
        }
        else
        {
            mpeError.Show();
        }
    }

    protected void lnkCancelWorkflow_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;

        Model = new ProviderManagementData
        {
            RegID = this.KeyFields.RegID,
            RegistrationStatusTypeID = CON.RegistrationStatusTypeId.Deleted,
            LastModifiedDate = DateTime.Now,
            LastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name)
        };

        switch (lnk.CommandName)
        {
            case WorkflowLinkOptions.CancelUpdateRegistration:
                presenter.CancelRegistration(this.KeyFields.ProcessID, lnk.CommandName);
                Response.Redirect("~/Process/ProviderHomeNew.aspx");
                break;
            case WorkflowLinkOptions.CancelUpdateOwnership:
            case WorkflowLinkOptions.CancelRevalidation://OHPNM-15875:This is for Cancelling Conver from ORP
                presenter.CancelRegistration(this.KeyFields.ProcessID, lnk.CommandName);
                Response.Redirect("~/Process/ProviderHomeNew.aspx");
                break;
            case WorkflowLinkOptions.CancelWaiverServices:
            case WorkflowLinkOptions.CancelNewRegistration:
            case WorkflowLinkOptions.CancelReapplication:
                presenter.CancelRegistration(this.KeyFields.ProcessID);
                Response.Redirect("~/Process/ProviderHomeNew.aspx"); //OHPNM-3622
                break;
            case WorkflowLinkOptions.CancelAddAffiliation:
                //just does cancel workflow
                presenter.CancelWorkflow();
                break;
            case WorkflowLinkOptions.CancelReactivateEnrollment:
                //tbd
                break;
            case WorkflowLinkOptions.CancelCPCRegistration:
                presenter.CancelCPCRegistration(this.KeyFields.ProcessID, this.KeyFields.WorkflowEventType);
                Response.Redirect("~/Process/ProviderHomeNew.aspx"); //OHPNM-3622
                break;
            default:
                break;
        }
    }

    protected void lnkBeginUpdateWorkflow_Click(object sender, EventArgs e)
    {

        btnUpdateOk.Visible = true;
        mpeUpdateRegistration.Show();

    }

    protected void lnkBeginWorkflow_Click(object sender, EventArgs e)
    {
        int workflowID = 0;

        mpeUpdateRegistration.Hide();
        LinkButton lnk = sender as LinkButton;
        bool reValidation = false;
        switch (lnk.CommandName)
        {
            case WorkflowLinkOptions.BeginUpdateRegistration:
                workflowID = CON.WorkflowType.RegistrationUpdateProvider;
                break;
            case WorkflowLinkOptions.BeginReapplication:
                if (!validateNPI(KeyFields.RegID))
                {
                    lnkUpdateKeyFields_Click(sender, e);
                }
                workflowID = CON.WorkflowType.RegistrationRevalidation;
                reValidation = true;
                break;
            case WorkflowLinkOptions.BeginRevalidation:
                if (!validateNPI(KeyFields.RegID))
                {
                    lnkUpdateKeyFields_Click(sender,e);
                }
                workflowID = CON.WorkflowType.RegistrationRevalidation;
                reValidation = true;
                break;
            case WorkflowLinkOptions.BeginAddAffiliation:
                workflowID = CON.WorkflowType.UpdateAffiliates;
                break;
            case WorkflowLinkOptions.BeginUpdateServices:
                workflowID = CON.WorkflowType.RegistrationDIDDReferral;
                break;
            case WorkflowLinkOptions.BeginReactivation:
                workflowID = CON.WorkflowType.RegistrationReactivation;
                break;
            case WorkflowLinkOptions.BeginUpdateGMPRegistration:
                workflowID = CON.WorkflowType.GroupMemberProfile;
                break;

            default:
                break;
        }
        bool isProviderReactivation = false;
        bool isProviderReapplication = false;
        Helper.IsUpdateCPCContact = false;
        workflowID = svc.GetWorkflowInstance(KeyFields.ApplicationTypeID, KeyFields.ProviderCategoryTypeID,
            KeyFields.ProviderTypeID, KeyFields.ReferralTypeID, reValidation, false, false, false);

        if (lnk.CommandName == WorkflowLinkOptions.BeginReactivation)
        {
            isProviderReactivation = true;
        }
        if (lnk.CommandName == WorkflowLinkOptions.BeginReapplication)
        {
            isProviderReapplication = true;
        }
        IsRevalidation = reValidation;
        BeginUpdateRegistration(workflowID, false, isProviderReactivation, isProviderReapplication);

        if (validateNPI(KeyFields.RegID))
        {

            Response.Redirect("~/process/Registration.aspx?RegId=" + KeyFields.RegID);
        }
        else
        {
            Response.Redirect("~/Process/NewProvider.aspx?EditKeyFieldDataRequest=true&RegID=" + KeyFields.RegID.ToString());
        }

    }

    protected void lnkViewProviderFile_Click(object sender, EventArgs e)
    {
        (this.Page as RegistrationProvider).RegistrationId = this.KeyFields.RegID;
        (this.Page as RegistrationProvider).IsReadOnly = true;
        Session["canEdit"] = false;
        Helper.IsUpdateCPCContact = false;
        Session["ViewProviderFile"] = true;
    }

    protected void lnkUpdateProviderFile_Click(object sender, EventArgs e)
    {
        (this.Page as RegistrationProvider).RegistrationId = this.KeyFields.RegID; // probably don't need?
        (this.Page as RegistrationProvider).IsReadOnly = true; // probably don't need?
        Helper.IsUpdateCPCContact = true;

        int workflowID = svc.GetWorkflowInstance(KeyFields.ApplicationTypeID, KeyFields.ProviderCategoryTypeID,
                    KeyFields.ProviderTypeID, KeyFields.ReferralTypeID, false, false, false, false);

        BeginUpdateRegistration(workflowID, false, false, false, false, CON.WaiverServiceUpdateType.ODM, CON.WorkflowEventType.UpdateReg);

        Response.Redirect("~/Process/ProviderUpdateSummary.aspx?RegId=" + this.KeyFields.RegID);
    }
    protected void lnkCreateCPCIndividual_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        presenter.CreateNewCPCRegistration(this.KeyFields.RegID, CON.CPCType.Individual, CPC_Program_Year, Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.KeyFields.MMISCPCPracticeTypeID);

        ProviderFeedHelper.InsertProviderFeedNotes(this.KeyFields.RegID, 0, HttpContext.Current.User.Identity.Name, "Create CPC Individual", personReviewedBy: HttpContext.Current.User.Identity.Name, finalDisposition: CON.FinalDisposition.NotSubmitted, processID: this.KeyFields.ProcessID);
    }

    protected void lnkCreatePracticePartnership_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        presenter.CreateNewCPCRegistration(this.KeyFields.RegID, CON.CPCType.Convener, CPC_Program_Year, Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.KeyFields.MMISCPCPracticeTypeID);

        ProviderFeedHelper.InsertProviderFeedNotes(this.KeyFields.RegID, 0, HttpContext.Current.User.Identity.Name, "Create Practice Partnership", personReviewedBy: HttpContext.Current.User.Identity.Name, finalDisposition: CON.FinalDisposition.NotSubmitted, processID: this.KeyFields.ProcessID);
    }
    protected void lnkContinueCPCApplication_Click(object sender, EventArgs e)
    {
        IsCPCUpdateContact(this.KeyFields.ProcessID);
        if (this.KeyFields.WorkflowEventType == CON.WorkflowEventType.UpdateReg)
        {
            Response.Redirect("~/Process/ProviderUpdateSummary.aspx?RegId=" + this.KeyFields.RegID);
        }
        else
        {
            Response.Redirect("~/Process/Registration.aspx?RegId=" + this.KeyFields.RegID);
        }
    }

    protected void lnkContinueCMCApplication_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCMCContact = false;
        if (this.KeyFields.WorkflowEventType == CON.WorkflowEventType.CMCUpdate)
        {
            Response.Redirect("~/Process/ProviderUpdateSummary.aspx?RegId=" + this.KeyFields.RegID);
        }
        else
        {
            Response.Redirect("~/Process/Registration.aspx?RegId=" + this.KeyFields.RegID);
        }
    }

    protected void lnkReattestCPCIndividualorPracticePartnership_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        int workflowID = svc.GetWorkflowInstance(KeyFields.ApplicationTypeID, KeyFields.ProviderCategoryTypeID,
                    KeyFields.ProviderTypeID, KeyFields.ReferralTypeID, false, false, false, false);

        BeginUpdateRegistration(workflowID, false, false, false, false, CON.WaiverServiceUpdateType.ODM, CON.WorkflowEventType.CPCReattest);

        ProviderFeedHelper.InsertProviderFeedNotes(this.KeyFields.RegID, 0, HttpContext.Current.User.Identity.Name, "Re-attest CPC Individual or Practice Partnership", personReviewedBy: HttpContext.Current.User.Identity.Name, processID: this.KeyFields.ProcessID);
    }

    protected void lnkReattestCMC_Click(object sender, EventArgs e)
    {
        //Helper.IsUpdateCMCContact = false;
        //int workflowID = svc.GetWorkflowInstance(KeyFields.ApplicationTypeID, KeyFields.ProviderCategoryTypeID,
        //            KeyFields.ProviderTypeID, KeyFields.ReferralTypeID, false, false, false, false);

        //BeginUpdateRegistration(workflowID, false, false, false, false, CON.WaiverServiceUpdateType.ODM, CON.WorkflowEventType.CMCReAttest);

        try
        {
            Helper.IsUpdateCPCContact = false;

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            //INSERT INTO VERSION Tables....
            DataSet ds = svc.InsertIntoVersionTables(this.KeyFields.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            DataRow dr;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dr = ds.Tables[0].Rows[0];
                if (!string.IsNullOrEmpty(Methods.GetStringValue(dr["ErrorMessage"])))
                {
                    Logging log = new Logging(Guid.NewGuid(), string.Empty);
                    log.CreateLogEntry(string.Format("{0} {1}", "Setting up Workflow", Methods.GetStringValue(dr["ErrorMessage"])), Logging.LogPriority.Error);
                    return;
                }
            }
            if (this.KeyFields.CurrentStepID == 0 || this.KeyFields.CurrentStepID == -1)
            {
                // create new Wf
                int entryTaskID = 850;
                Workflow.Process pr = new Workflow.Process(CON.WorkflowType.CMC, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Guid.NewGuid(), entryTaskID);
                if (pr == null)
                {
                    lblStatusMsg.Visible = true;
                    lblStatusMsg.Text = "CMC Re-Attest Workflow Initiation was not successful";
                    return;
                }

                int ProcessID = pr.ProcessID;
                int CurrentStepID = pr.CurrentStepID;
                int CurrentTaskID = pr.TaskID;
                int WorkflowID = pr.WorkflowID;

                // Save the Registration ID as a process parameter of the Workflow

                svc.WF_SaveProcessParameter(ProcessID, "REGISTRATION_ID", this.KeyFields.RegID.ToString());
                svc.InsertRegApplicationRecord(this.KeyFields.RegID, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId), ProcessID);
                svc.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", RegID.ToString() }, { "REGISTRATION_STATUS_TYPE_ID", CON.RegistrationStatusTypeId.Pending.ToString() }, { "REG_PROGRAM_STATUS_TYPE_ID", CON.RegistrationProgramStatusTypeId.New.ToString() }, { "WORKFLOW_EVENT_TYPE_ID", "9" } });

                svc.WF_SaveProcessParameter(ProcessID, "WORKFLOW_EVENT_TYPE_ID", "9");

                ProviderFeedHelper.InsertProviderFeedNotes(this.KeyFields.RegID, 0, HttpContext.Current.User.Identity.Name, "Re-attest CMC Provider", personReviewedBy: HttpContext.Current.User.Identity.Name, processID: ProcessID);
            }

            (this.Page as RegistrationProvider).RegistrationId = RegID;
            (this.Page as RegistrationProvider).IsReadOnly = false;
            (this.Page as RegistrationProvider).RegistrationStep = CON.SectionTypeID.CMCContactInformation;
            Response.Redirect("~/Process/Registration.aspx?RegId=" + RegID.ToString());
            
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkBeginCPCEnrollmentUpdate_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        int workflowID = svc.GetWorkflowInstance(KeyFields.ApplicationTypeID, KeyFields.ProviderCategoryTypeID,
                    KeyFields.ProviderTypeID, KeyFields.ReferralTypeID, false, false, false, false);

        BeginUpdateRegistration(workflowID, false, false, false, false, CON.WaiverServiceUpdateType.ODM, CON.WorkflowEventType.UpdateReg);

        Response.Redirect("~/Process/ProviderUpdateSummary.aspx?RegId=" + this.KeyFields.RegID);
    }

    protected void lnkBeginCMCEnrollmentUpdate_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCMCContact = false;
        int workflowID = CON.WorkflowType.CMC;

        BeginUpdateRegistration(workflowID, false, false, false, false, CON.WaiverServiceUpdateType.ODM, CON.WorkflowEventType.CMCUpdate);

        Response.Redirect("~/Process/ProviderUpdateSummary.aspx?RegId=" + this.KeyFields.RegID);
    }

    protected void lnkUpdateKeyFields_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        if (EditProviderKeyFieldsEvent != null)
        {
            //set key fields needed, call event to load pop up.
            var keyData = new ProviderManagerData
            {
                RegID = this.KeyFields.RegID,
                WorkflowIDRequested = KeyFields.WorkflowID,
                ReferralID = KeyFields.ReferralID,
                ReferralTypeID = KeyFields.ReferralTypeID,
                KeyFieldEditRequest = true,
                RegistrationProgramStatusTypeID = KeyFields.RegistrationProgramStatusID,
                ApplicationTypeID = KeyFields.ApplicationTypeID,
                WaiverTypeId = KeyFields.WaiverTypeID
            };
            EditProviderKeyFieldsEvent(keyData);
        }
    }

    protected void lnkLinkProvider_Click(object sender, EventArgs e)
    {
        //  TODO: EDV Pass on REG ID to ProviderDetails.
        Response.Redirect("~/Process/NewProvider.aspx?LinkProvider=true");
    }

    protected void lnkDisEnrollment_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        mpeSaveDisEnrollement.Show();
    }
    protected void lnkInitiateChop_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        //mpeClosureNotice.Show();
        mpeDaysNotice.Show();
    }
    protected void lnkProviderTypeChange_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        if (ProviderTypeChangeEvent != null)
        {
            //set key fields needed, call event to load pop up.
            var keyData = new ProviderManagerData
            {
                RegID = this.KeyFields.RegID,
                WorkflowIDRequested = KeyFields.WorkflowID,
                ReferralID = KeyFields.ReferralID,
                ReferralTypeID = KeyFields.ReferralTypeID,
                ProviderTypeChangeRequest = true,
                RegistrationProgramStatusTypeID = KeyFields.RegistrationProgramStatusID,
                ApplicationTypeID = KeyFields.ApplicationTypeID,
                WaiverTypeId = KeyFields.WaiverTypeID
            };
            ProviderTypeChangeEvent(keyData);
        }
    }
	
    protected void lnkCancelProviderTypeChange_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        Model = new ProviderManagementData
        {
            RegID = this.KeyFields.RegID,
            RegistrationStatusTypeID = CON.RegistrationStatusTypeId.Deleted,
            LastModifiedDate = DateTime.Now,
            LastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name)
        };

        presenter.CancelRegistration(this.KeyFields.ProcessID, lnk.CommandName);
        //svc.UpdateProviderTypeChangeStatus(this.RegID, CON.ProviderTypeChangeRequestStatus.Cancelled,DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        Response.Redirect("~/Process/ProviderHomeNew.aspx");       
         
    }

    protected void btnDisEnrollmentSave_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        Page.Validate("DisenrollProvider");

        if(!Page.IsValid)
        {
            return;
        }
        int reg_id = this.KeyFields.RegID;
        int cnt = 0;
        //Jira OHPNM-897: 10/28/2020 Pschwarz added disenrollment date
        DateTime disenrollDate;
        DateTime.TryParse(this.txtTermDate.Text.Trim(), out disenrollDate);

        for (int i = 0; i < disEnrollmentOptions.Items.Count; i++)
        {
            if (disEnrollmentOptions.Items[i].Selected)
            {
                cnt++;
                svc.InsertProviderDisenrollment(reg_id, disEnrollmentOptions.Items[i].Value, disenrollDate, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            }
        }
        
        if (cnt == 0)
        {
            lblError.Visible = true;
            return;
        }
        else
        {
            int workflowID = 0;
            workflowID = svc.GetWorkflowInstance(KeyFields.ApplicationTypeID, KeyFields.ProviderCategoryTypeID,
                        KeyFields.ProviderTypeID, KeyFields.ReferralTypeID, false, false, false, false);
            BeginUpdateRegistration(workflowID, false, false, false, true);

            ResetDisenrollPop();
        }
    }

    protected void btnDisEnrollmentCancel_Click(object sender, EventArgs e)
    {
        ResetDisenrollPop();
    }

    protected void btnReconsiderationCancel_Click(object sender, EventArgs e)
    {
        mpeRequestReconsideration.Hide();
    }

    protected void lnkBtn_ProviderReports(object sender, CommandEventArgs e)
    {
        RedirectToPage(CON.SectionTypeID.RetrieveReports);
    }
    protected void lnkBtn_Attachments(object sender, CommandEventArgs e)
    {
        RedirectToPage(CON.SectionTypeID.UploadAttachments);
    }

    protected void lnkBtn_ORPSearch(object sender, CommandEventArgs e)
    {
        RedirectToPage(CON.SectionTypeID.ORPProviderSearch);
    }
    private void RedirectToPage(int pageID)
    {
        string url = string.Empty;

        if (pageID == CON.SectionTypeID.CostReports)
        {
            PDMSService.PDMSServiceClient _spa = new PDMSService.PDMSServiceClient();

            DataSet dataSet = _spa.SelectServiceLocationByMedicaidID(this.lblMedicaidID.Text.Trim());
            DataTable dt = dataSet.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                url = ValidateProviderTypeAccessToCostReports(dt, url);
            }
        }
        else
        {
            Session["StepId"] = pageID;
            Session["RegId"] = this.RegID;
            if (pageID == CON.SectionTypeID.SearchPriorAuthorization)
            {
                url = "~/Process/SearchPriorAuthorization.aspx";
            }
            if (pageID == CON.SectionTypeID.SearchClaimV2)
            {
                url = "~/Process/SearchClaims.aspx";
            }
            if (pageID == CON.SectionTypeID.ProviderFinancial)
            {
                url = "~/Process/ProviderFinancials.aspx";
            }
            if (pageID == CON.SectionTypeID.SearcheRA)
            {
                url = "~/Process/ERemittanceAdvice.aspx";
            }
            if (pageID == CON.SectionTypeID.SearchEligibilityV2)
            {
                url = "~/Process/SearchEligibility.aspx";
            }
            if (pageID == CON.SectionTypeID.HospiceEnrollment)
            {
                url = "~/Process/HospiceEnrollment.aspx";
            }
            if (pageID == CON.SectionTypeID.UploadAttachments)
            {
                url = "~/Process/StandaloneUploadAttachments.aspx";
            }
            if (pageID == CON.SectionTypeID.RetrieveReports)
            {
                url = "~/Process/ProviderRetrieveReports.aspx";
            }
            if (pageID == CON.SectionTypeID.ORPProviderSearch)
            {
                url = "~/Process/ORProviderSearch.aspx";
            }
        }

        if (!string.IsNullOrEmpty(url))
        {
            Response.Redirect(url);
        }
    }

    private string ValidateProviderTypeAccessToCostReports(DataTable dt, string url)
    {
        //validate provider type access to cost reports
        var regId = Convert.ToInt32(dt.Rows[0]["REG_ID"].ToString());

        if (regId > 0)
        {
            var providerTypeId = Helper.GetProviderTypeIdByRegId(regId);
            if (providerTypeId > 0)
            {
                if (providerTypeId == CON.ProviderTypeNumerics.NURSING_FACILITY
                    || providerTypeId == CON.ProviderTypeNumerics.STATE_OPERATED_ICF_MR
                    || providerTypeId == CON.ProviderTypeNumerics.NONSTATE_OPERATED_ICF_MR
                    || providerTypeId == 12 || providerTypeId == 5 || providerTypeId == 28
                       || providerTypeId == 1 || providerTypeId == 2 || providerTypeId == 4)
                {
                    url = AppSettings.Get("CostReportsLink");
                }
            }
        }

        return url;
    }

    private void ResetDisenrollPop()
    {
        this.txtTermDate.Text = "";
        lblError.Visible = false;
        for (int i = 0; i < disEnrollmentOptions.Items.Count; i++)
        {
            disEnrollmentOptions.Items[i].Selected = false;
        }
        mpeSaveDisEnrollement.Hide();
        //reload the page 
        Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
    }

    #endregion

    #region Public Events
    public void InitView(ProviderManagerData keyData)
    {
        this.ReferralID = keyData.ReferralID;
        this.ReferralTypeID = keyData.ReferralTypeID;
        this.RegID = keyData.RegID;
        this.PaperRequestQueueID = keyData.PaperRequestQueueID;
        presenter.Init(keyData.RegID);
        this.ucCommView.InitView(keyData);
    }
    public void EnablePage(bool enable)
    {
        this.lnkUpdateRegistration.Enabled = enable;
        this.lnkUpdateOwnershipInfo.Enabled = enable;
        this.lnkUpdateServices.Enabled = enable;
        this.lnkAddAffiliation.Enabled = enable;
        this.lnkUpdateGroupMemberProfile.Enabled = enable;
        this.lnkViewProviderFile.Enabled = enable;
        this.lnkContinueGroupMbrProfile.Enabled = enable;
        this.lnkContinueRegistration.Enabled = enable;
        this.lnkBeginRevalidation.Enabled = enable;
        this.lnkContinueUpdateRegistration.Enabled = enable;
        this.lnkCancelUpdateRegistration.Enabled = enable;
        this.lnkContinueUpdateOwnership.Enabled = enable;
        this.lnkCancelUpdateOwnership.Enabled = enable;
        this.lnkContinueAddAffiliation.Enabled = enable;
        this.lnkCancelAddAffiliation.Enabled = enable;
        this.lnkContinueAddServices.Enabled = enable;
        this.lnkCancelAddServices.Enabled = enable;
        this.lnkContinueRevalidation.Enabled = enable;
        this.lnkCancelConvertORP.Enabled = enable;
        this.lnkContinueReactivateEnrollment.Enabled = enable;
        this.lnkCancelReactivateEnrollment.Enabled = enable;
        this.lnkUpdateKeyFields.Enabled = enable;
    }
    #endregion

    #region Presenter Events

    public void SetProviderDetails(ProviderManagementData details)
    {
        if (details == null)
            return;

        KeyFields = new RegKeyFields(details.RegID, details.ReferralID, details.WorkflowID, this.PaperRequestQueueID, details.ProcessID, details.CurrentStepID, details.CurrentTaskID, details.CurrentTaskClassName,
            details.FormCompletionName, details.FormCompletionPhone, details.ChangeEffectiveDate, details.RequestedEffectiveDate, details.ReferralTypeID, details.MedicaidID, details.RegistrationProgramStatusTypeID,
            details.ApplicationTypeID, details.ProviderCategoryTypeID, details.ProviderTypeID, details.RevalidationDate, details.TerminationDate.HasValue, details.EndDate, details.EnrollmentStatusCode, details.WorkflowEventType,
            details.WaiverTypeID, details.IsCredentialingProvider, details.enrollmentStatusReason, details.MMISCPCPracticeTypeID, details.NPI);

        this.RevalDueWindow = details.RevalidationDueWindow;
        this.lblProviderName.Text = Helper.HtmlEncode(details.ProviderName);

        this.lblEffectiveDate.Text = Helper.HtmlEncode(details.ChangeEffectiveDate.HasValue ? Helper.FormatDate2(details.ChangeEffectiveDate.ToString()) : string.Empty);
        this.lblRevalidationDate.Text = Helper.HtmlEncode(details.RevalidationDate.HasValue ? Helper.FormatDate2(details.RevalidationDate.ToString()) : string.Empty);
        this.lblTermDate.Text = Helper.HtmlEncode(details.TerminationDate.HasValue ? Helper.FormatDate2(details.TerminationDate.ToString()) : string.Empty);

        this.lblRegistrationStatus.Text = Helper.HtmlEncode(Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) ? details.RegProgramStatusTypeExternal : details.RegProgramStatusTypeInternal);
        this.lblApplicationStatus.Text = Helper.HtmlEncode(details.RegistrationStatusType);
        this.lblMedicaidID.Text = Helper.HtmlEncode(details.MedicaidID);
        this.lblDODDContractNumber.Text = Helper.HtmlEncode(details.DDContractNumber);
        HasDDContractNum = string.IsNullOrEmpty(details.DDContractNumber) ? false : true;
        this.lblDODDCertStartDate.Text = Helper.HtmlEncode(Helper.FormatDate2(details.DODDStartDate.ToString()));
        this.lblDODDCertEndDate.Text = Helper.HtmlEncode(Helper.FormatDate2(details.DODDEndDate.ToString()));

        this.lblEnrollmentStatus.Text = Helper.HtmlEncode(details.EnrollmentStatusCodeDescription);

        this.divMoratoriaInfo.Visible = details.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.EnforceMoratoria;
        this.lblMoratoriaBeginDate.Text = Helper.HtmlEncode(details.MoratoriaBeginDate.HasValue ? Helper.FormatDate2(details.MoratoriaBeginDate.ToString()) : string.Empty);
        this.lblMoratoriaEndDate.Text = Helper.HtmlEncode(details.MoratoriaEndDate.HasValue ? Helper.FormatDate2(details.MoratoriaEndDate.ToString()) : string.Empty);

        HasActiveODASpecialty = details.HasActiveODASpecialty;
        HasDODDSpecialty = details.HasDODDSpecialty;
        HasActiveDODDSpecialty = details.HasActiveDODDSpecialty;
        HasODMSpecialty = details.HasODMSpecialty;
        HasActiveODMSpecialty = details.HasActiveODMSpecialty;
        HasODASpecialty = details.HasODASpecialty;
        IsAddODMorODAMedicaidSvc = details.IsAddODMorODAMedicaid;
        MMISProviderTypeId = GetMMISProviderTypeID(KeyFields.ProviderTypeID);
        IsPowerAgent = Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, this.KeyFields.RegID);
        this.TurnOffWorkflowLinks();
        this.SetWorkflowLinks(details);

        // haven handoff data
        this.PaymentInnovationsProviderId = details.MedicaidID;
        this.RecipientEligibilityMITSProviderId = details.MedicaidID;
       

    }

    public void BeginNewWorkflow(ProviderManagementData data)
    {
        (this.Page as RegistrationProvider).RegistrationId = data.RegID;
        (this.Page as RegistrationProvider).IsReadOnly = false;
    }

    public void CompleteCancelWorkflow()
    {
        //Let parent page know to reload page.
        if (CancelWorkflowEvent != null)
        {
            CancelWorkflowEvent();
        }

    }

    public void SetErrorMessages()
    {
        if (presenter.hasErrors)
        {
            if (ErrorEvent != null)
                ErrorEvent(presenter.ErrorList);
        }
    }

    #endregion

    #region Private Methods
    private void SetupButtonDisablesOfMultiClick()
    {
        btnDisEnrollmentSave.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnDisEnrollmentSave, null) + ";");
        btnUpdateOk.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnUpdateOk, null) + ";");
        btnDaysNoticeUpload.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnDaysNoticeUpload, null) + ";");
        btnClosureNoticeUpload.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnClosureNoticeUpload, null) + ";");
        btnContinueUpdateOk.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnContinueUpdateOk, null) + ";");
    }
    private void SetWorkflowLinks(ProviderManagementData details)
    {
        this.divOptKeyFields.Visible = false;
        //Special Case: IF enrollment has been denied, terminated or suspended
        if ((details.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Denied
            || details.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Disenrolled
            || details.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Suspended
            || details.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.EnforceMoratoria
            ) && (!details.InProviderDataEntry))
        {
            this.divOptViewOnly.Visible = true;
        }

        // these links all need to have a medicaid ID and specific roles to view
        if (!string.IsNullOrEmpty(details.MedicaidID))
        {
            // check all sub-roles or ProviderAgent
            if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent) == true && !IsPowerAgent)
            {
                if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.RecipientEligibilitySubRoles))
                {
                    divRecipientEligibility.Visible = true;
                }
                if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.RemittanceRedirectionSubRoles)
                    && !(HasODASpecialty && !HasODMSpecialty))
                {
                    divRemittanceRedirection.Visible = true;
                    if (HasDODDSpecialty && !HasODMSpecialty)
                    {
                        lnkBtnRemittanceRedirection.Enabled = false;
                    }
                }
                if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.FinancialRedirectionSubRoles) && details.ApplicationTypeID != CON.ApplicationType.ORP
                     && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty))
                {
                    divViewFinancials.Visible = true;
                }               

                if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.HospiceSubRoles)
                    && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty))
                {
                    divHospice.Visible = true;
                }
                if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.ClaimsSubRoles)
                    && !(HasODASpecialty && !HasODMSpecialty))
                {
                    divClaims.Visible = true;
                    if (HasDODDSpecialty && !HasODMSpecialty)
                    {
                        lnkClaims.Enabled = false;
                    }
                }
                if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.CostReportSubRoles)
                    && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty))
                {
                    divCostReports.Visible = true;
                }
                if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.ProviderReportsSubRoles)
                     && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty))
                {
                    divProviderReports.Visible = true;
                }
                /*if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.AttachmentsSubRoles)
                    && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty)
                    && string.Equals(AppSettings.Get("ShowUploadAttachmentIconPNM3BPA"), "1"))
                {
                   divAttachments.Visible = true;
                }*/
                if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.ORPSearchSubRoles)
                    && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty)
                    && string.Equals(AppSettings.Get("SAM748"), "true"))
                {
                    divORPSearch.Visible = true;
                }
                if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.PriorAuthorizationSubRoles)
                    && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty))
                {
                    divPriorAuthorization.Visible = true;
                }

                if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.PaymentInnovationReportSubRoles)
                    && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty))
                {
                    divPaymentInnovationReports.Visible = true;
                }

                if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.CPCAgentSubRole)
                    && CheckIfActiveCMCProvider(details.RegID))
                {
                    divCMCDashboard.Visible = true;
                }

            }
            else
            {
                if ((Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.PaymentInnovationReportSubRoles) || IsPowerAgent)
                    && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty))
                {
                    divPaymentInnovationReports.Visible = true;
                }
                // check all non-sub roles (Internal and Provider)
                if (Helper.IsUserInInternalRole(CON.RecipientEligibilityInternalRoles) || IsPowerAgent)
                {
                    divRecipientEligibility.Visible = true;
                }
                if ((Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) == true || IsPowerAgent) && string.Equals(AppSettings.Get("SAM748"), "true"))
                {
                    divORPSearch.Visible = true;
                }
                if ((Helper.IsUserInInternalRole(CON.RemittanceRedirectionInternalRoles) || IsPowerAgent) && !(HasODASpecialty && !HasODMSpecialty))
                {
                    divRemittanceRedirection.Visible = true;
                    if (HasDODDSpecialty && !HasODMSpecialty)
                    {
                        lnkBtnRemittanceRedirection.Enabled = false;
                    }
                }
                if ((Helper.IsUserInInternalRole(CON.FinancialRedirectionInternalRoles) || IsPowerAgent) && details.ApplicationTypeID != CON.ApplicationType.ORP
                    && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty))
                {
                    divViewFinancials.Visible = true;
                }
                if ((Helper.IsUserInInternalRole(CON.PaymentInnovationReportInternalRoles) || IsPowerAgent) && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty))
                {
                    divPaymentInnovationReports.Visible = true;
                }
                if (Helper.IsUserInInternalRole(CON.CorrespondenceInternalRoles) || IsPowerAgent)
                {
                    divCorrespondence.Visible = true;
                }
                if ((Helper.IsUserInInternalRole(CON.HospiceInternalRoles)|| IsPowerAgent) && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty))
                {
                    divHospice.Visible = true;
                }
                if ((Helper.IsUserInInternalRole(CON.ClaimsInternalRoles)|| IsPowerAgent) && !(HasODASpecialty && !HasODMSpecialty))
                {
                    divClaims.Visible = true;
                    if (HasDODDSpecialty && !HasODMSpecialty)
                    {
                        lnkClaims.Enabled = false;
                    }
                }
                if ((Helper.IsUserInInternalRole(CON.CostReportInternalRoles) || IsPowerAgent) && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty))
                {
                    divCostReports.Visible = true;
                }
                if (Helper.IsUserInInternalRole(CON.ProviderReportsInternalRoles) || IsPowerAgent)
                {
                    divProviderReports.Visible = true;
                }
                /*if (Helper.IsUserInInternalRole(CON.AttachmentsInternalRoles) && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty)
                    && string.Equals(AppSettings.Get("ShowUploadAttachmentIconPNM3BPA"), "1"))
                {
                    divAttachments.Visible = true;
                }*/
                if ((Helper.IsUserInInternalRole(CON.PriorAuthorizationInternalRoles) || IsPowerAgent) && !(HasODASpecialty && !HasODMSpecialty) && !(HasDODDSpecialty && !HasODMSpecialty))
                {
                    divPriorAuthorization.Visible = true;
                }
                if (Helper.IsUserInInternalRole(CON.ViewProviderInternalRoles) || IsPowerAgent)
                {
                    divOptViewOnly.Visible = true;
                    lnkViewProviderFile.Visible = true;
                    lnkReactivateProvider.Visible = false;
                }
                if((Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) == true || IsPowerAgent)
                    && CheckIfActiveCMCProvider(details.RegID))
                {
                    divCMCDashboard.Visible = true;
                }
            }

            //OHPNM-5436 correspondance should be displayed for provider agents regardless of medicaid id 
            if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent) == true && !IsPowerAgent)
            {
                if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.CorrespondenceSubRoles))
                {
                    divCorrespondence.Visible = true;
                }
            }


            // turn off cost reports unless they are certain provider types ... TODO - this really needs to be somewhere else; GUI code shouldn't have business logic like this
            List<string> CostReportsAllowedProviderTypes = new List<string>() { "86", "88", "89", "12", "05", "28", "01", "02", "04" };
            if (!CostReportsAllowedProviderTypes.Contains(details.MMISProviderTypeID))
            {
                divCostReports.Visible = false;
            }

            // turn off Provider Reports unless they are certain provider types
            if (!Helper.ProviderTypeAllowedToViewLink(details.MMISProviderTypeID, CON.ProviderReportsAllowedProviderTypes))
            {
                divProviderReports.Visible = false;
            }

            //TO DO: Remove this block and add type 86 under  ProviderReportsAllowedProviderTypes (above line) once SAM684 is enabled
            if (AppSettings.Get("EnableSAM684") == "true" && details.MMISProviderTypeID == "86")
            {
                divProviderReports.Visible = true;
            }
        }

        if (string.IsNullOrEmpty(details.MedicaidID) && !string.IsNullOrEmpty(details.RegID.ToString()))
        {
            if (Helper.IsUserInInternalRole(CON.CorrespondenceInternalRoles))
            {
                divCorrespondence.Visible = true;
            }
        }
        if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EnrollmentAgentSubRole) == true ||
            Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) == true || IsPowerAgent)
        {
            divOptViewOnly.Visible = true;
            lnkViewProviderFile.Visible = true;
            lnkReactivateProvider.Visible = false;
        }
        else
        {
            divOptViewOnly.Visible = true;
            lnkViewProviderFile.Visible = false;
            lnkReactivateProvider.Visible = false;
        }
        IsCPCUpdateContact(details.ProcessID);
        if (details.CurrentStepID == 0 || details.CurrentStepID == -1)  // It will be -1 if the provider is a converted provider.
        {
            //in maintenance mode
            ShowMaintenanceModeLinks(details);
        }
        else if (details.InProviderDataEntry)
        {
            ShowProviderDataEntryModeLinks(details);
        }
        else
        {
            ShowReconsiderationLink(details);
            divDODDUpdate.Visible = Helper.IsProviderTypeInDODDForWaiverServiceUpdate(MMISProviderTypeId) && details.CurrentStepID > 0 && details.WorkflowEventType == CON.WorkflowEventType.UpdateReg && details.WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD;

            if (CheckAdditionalApplicationExist(details.RegID))
            {
                // there is an additional application from the DODD sister agency for this reg_application; don't let them cancel at this point
                this.lnkCancelUpdateDoddRegistration.Visible = false;
            }

            // if the dodd continue/cancel links div is visible, check the WF_PARAMETERS to make sure it should be shown (we don't want to show it if the workflow was started by an automated exclusion process; those look like DODD updates be really aren't)
            if (divDODDUpdate.Visible)
            {
                TurnOffDODDUpdateCancelLinksIfNotDoddUpdate(details);
            }
        }
        //Show additional reval help text for ODM providers in reval period adding DODD/ODA initial services
        if (details.RevalidationDate.HasValue)
        {
            if (((details.WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD && Helper.IsDODDInitialApplication(details.RegID))
                        || (details.WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.ODA && Helper.IsODAInitialApplication(details.RegID))) && !details.IsCredentialingProvider
                       && (details.RevalidationDate.Value.AddDays(RevalDueWindow) <= DateTime.Today))
            {
                divRevalHelpText.Visible = true;
            }
        }
        var status = Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) ? details.RegProgramStatusTypeExternal : details.RegProgramStatusTypeInternal;
        var applicationStatus = details.RegistrationStatusType;
        this.RegistrationStatusId = details.RegistrationStatusTypeID;
        this.RegistrationStatusType = details.RegistrationStatusType;
        this.RegProgramStatusTypeId = details.RegistrationProgramStatusTypeID;
        this.reg_id = details.RegID;

        var enrollStatus = details.EnrollmentStatusCode;
        if ((applicationStatus == "Submitted" && status == "Active") 
            || (details.RegistrationProgramStatusTypeID.ToString() == CON.RegistrationProgramStatusTypeId.Conversion.ToString() 
                    && (details.CurrentStepID == -1 || details.CurrentStepID ==0 ) && enrollStatus == CON.EnrollStatus.ACTIVE.ToString()))
        {
            var mmisProviderId = GetMMISProviderTypeID(details.ProviderTypeID);
            if (mmisProviderId == CON.ProviderTypeNumerics.NURSING_FACILITY.ToString())
            {

                DataSet ds = svc.GetWFProcessByRegId(RegID);

                if (Helper.HasRows(ds))
                {
                    int currentStepID = Helper.GetInt("CURRENT_STEP_ID", ds.Tables[0].Rows[0]);
                    if (currentStepID > 0)
                    {
                        this.divInitiateCHOP.Visible = false;
                        this.divSubmit90DayClosure.Visible = false;
                        DisableEnableLinksIfActiveRiskAlert(false);
                    }
                    else
                    {
                        this.divInitiateCHOP.Visible = true;
                        this.divSubmit90DayClosure.Visible = true;
                    }

                }
                else         //Converted Provider      
                {
                    if (details.CurrentStepID == -1)
                    {
                        this.divInitiateCHOP.Visible = true;
                        this.divSubmit90DayClosure.Visible = true;
                    }
                }

            }
        }
        SetCMClinkVisibility(details);

        if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent) == true && !IsPowerAgent)
        {
            this.pnlEnrollmentActionLinks.Visible = false;
            //this.lnkAddODAServices.Visible = false;

            //CR241 - ProviderAgent with EnrollmentAgent subrole should be able to only Update or Revalidate the registration
            if (Helper.IsUserInSubRoles(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EnrollmentAgentSubRole))
            {
                this.pnlEnrollmentActionLinks.Visible = true;
                this.divOptNoActiveWorkflowGMP.Visible = false;
                this.divOptNewGroupMbrProfile.Visible = false;
                this.divOptNewRegistration.Visible = false;
                this.divReapplicationProvider.Visible = false;
                this.divOptUpdateOwnership.Visible = false;
                this.divOptAddAffiliation.Visible = false;
                this.divOptServicesReferral.Visible = false;
                this.divOptReactivateEnrollment.Visible = false;
                this.divOptKeyFields.Visible = true;
                this.divLinkProvider.Visible = false;
                this.divDisEnrollment.Visible = false;
                this.divRequestReconsideration.Visible = false;
                this.divInitiateCHOP.Visible = false;
                this.divSubmit90DayClosure.Visible = false;
                this.lnkUpdateOwnershipInfo.Visible = false;
                this.lnkUpdateServices.Visible = false;
                this.lnkAddAffiliation.Visible = false;
                this.lnkConvertFromORP.Visible = false;
                this.lnkBeginReapplication.Visible = false;
                this.lnkAddODMorODAMedSvc.Visible = false;
                this.divOptProviderTypeChange.Visible = false;
            }
        }

        if(details.RegistrationStatusTypeID == CON.RegistrationStatusTypeId.NotProcessed)
        {
            divOptODMUpdate.Visible = false;
            //lnkBeginDODDUpdate.Visible = false;
            
            //lnkAddODAServices.Visible = false;
            lnkRequestReconsideration.Visible = false;
        }

        // SAM537 Dont let suspended provider edit key identifiers
        if (enableCR537 && details.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Suspended)
            this.divOptKeyFields.Visible = false;
    }

    private bool CheckIfActiveCMCProvider(int regID)
    {
        bool flag = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.CheckIfActiveCMCProvider(regID);
        if (Helper.HasRows(ds))
        {
            flag = true;
        }
        return flag;
    }

    private bool CheckIfPSMPCWError(int regID)
    {
        bool isErrpr = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        string txnResult = string.Empty;
        DataSet ds = new DataSet();
        ds = psc.SelectRescentTransactionIDByRegID(regID);
        if (ds.Tables[0].Rows.Count != 0)
        {
            txnResult = ds.Tables[0].Rows[0]["SI_RESPONSE_CODE"].ToString();
        }
        if (!(txnResult.Equals(CON.ResponseCodes.WAIVER_SI_SUCCESS) || txnResult.Equals(CON.ResponseCodes.WAIVER_SI_SUCCESS_ACK)))
        {
            isErrpr = true;
        }
        return isErrpr;
    }
    private bool CheckAdditionalApplicationExist(int regID)
    {
        bool isAdditionalExists = false;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataSet ds = new DataSet();
        ds = psc.SelectAdditionalApplications(regID);
        if (ds.Tables[0].Rows.Count != 0)
        {
            isAdditionalExists = true;

            // OHPNM-6734 - go ahead and grab the REG_APPLICATION.Application_Status = 52 and REG_ADDITIONAL_APPLICATION.APPLICATION_TYPE_ID = 24 in case it needs to be used by others
            AdditionalApplicationStatusID = ds.Tables[0].Rows[0]["Application_Status"].ToString();
            AdditionalApplicationTypeID = ds.Tables[0].Rows[0]["APPLICATION_TYPE_ID"].ToString();
        }
        return isAdditionalExists;
    }
    private void ShowProviderDataEntryModeLinks(ProviderManagementData details)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        bool UserCanAccessReg = psc.UserCanAccessReg(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), details.RegID);

        if (details.WorkflowID != CON.WorkflowType.CPC)
        {
            //open up the applicable Continue link
            switch (details.WorkflowEventType)
            {
                case CON.WorkflowEventType.NewReg:
                    this.divOptNewRegistration.Visible = true;
                    this.divOptKeyFields.Visible = details.RegistrationProgramStatusTypeID != CON.RegistrationProgramStatusTypeId.Conversion && IsNewRegistration;
                    this.lnkCancelNewRegistration.Visible = details.RegistrationProgramStatusTypeID != CON.RegistrationProgramStatusTypeId.Conversion;

                    if (details.ApplicationTypeID == CON.ApplicationType.Waiver)
                    {

                        if (details.WaiverTypeID == CON.WaiverApplicationTypeID.ODM)
                        {
                            lnkContinueDODDRegistration.Visible = false;
                            lnkContinueODARegistration.Visible = false;
                        }
                        else if (details.WaiverTypeID == CON.WaiverApplicationTypeID.ODA)
                        {
                            lnkContinueRegistration.Visible = false;
                            lnkContinueDODDRegistration.Visible = false;
                            lnkCancelNewRegistration.Visible = false;
                            divOptKeyFields.Visible = false;
                            if (CheckIfPSMPCWError(details.RegID))
                            {
                                divWaiverErrorDisplay.Visible = true;
                            }
                            else
                            {
                                lnkContinueODARegistration.Visible = true;
                                //OHPNM-13420 : Prod - Cancel ODA Option Re-enable
                                if (CheckAdditionalApplicationExist(details.RegID))
                                {
                                    // there is an additional application from the sister agency for this reg_application; don't let them cancel at this point
                                    lnkCancelNewRegistration.Visible = false;
                                }
                                else
                                {
                                    lnkCancelNewRegistration.Visible = true;
                                }
                            }

                        }
                        else if (details.WaiverTypeID == CON.WaiverApplicationTypeID.DODD || details.WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD)
                        {
                            lnkContinueRegistration.Visible = false;
                            lnkContinueODARegistration.Visible = false;


                            divOptKeyFields.Visible = false;
                            if (CheckIfPSMPCWError(details.RegID))
                            {
                                divWaiverErrorDisplay.Visible = true;
                            }
                            else
                            {
                                lnkContinueDODDRegistration.Visible = true;
                            }
                            #region OHPNM-8598

                            lnkCancelNewRegistration.Visible = false;

                            #endregion
                        }
                    }


                    break;
                case CON.WorkflowEventType.UpdateReg:
                    this.divOptKeyFields.Visible = true;
                    this.divOptUpdateProvider.Visible = UserCanAccessReg; // SAM768 Provider admin , power agent or any provider agent with Enrollment agent access should be able to continue/cancel.

                    //get the waiver service update for this registration
                    // and then display
                    int waiverUpdateTypeId = details.WaiverServiceUpdateTypeID;

                    if (((waiverUpdateTypeId == CON.WaiverServiceUpdateType.DODD && Helper.IsDODDInitialApplication(details.RegID))
                         || (waiverUpdateTypeId == CON.WaiverServiceUpdateType.ODA && Helper.IsODAInitialApplication(details.RegID))) && !details.IsCredentialingProvider
                        && (details.RevalidationDate.Value.AddDays(RevalDueWindow) <= DateTime.Today))
                    {
                        divRevalHelpText.Visible = true;
                    }
                    if (details.IsAddODMorODAMedicaid)  //Non Medicaid DODD adding ODM/ODA Medicaid services
                    {
                        if (details.ApplicationTypeID == CON.ApplicationType.Standard ||
                                                        (details.ApplicationTypeID == CON.ApplicationType.Waiver && waiverUpdateTypeId == CON.WaiverServiceUpdateType.ODM))
                        {
                            lnkContinueUpdateRegistration.Visible = true;
                            divOptKeyFields.Visible = false;
                            divDODDUpdate.Visible = false;
                            divODAUpdate.Visible = false;
                        }
                        else if (details.ApplicationTypeID == CON.ApplicationType.Waiver && waiverUpdateTypeId == CON.WaiverServiceUpdateType.ODA && !HasActiveODASpecialty)
                        {
                            divODAUpdate.Visible = true;
                            lnkContinueUpdateRegistration.Visible = false;
                            divOptKeyFields.Visible = false;
                            divDODDUpdate.Visible = false;
                        }
                    }
                    else
                    {
                        if (waiverUpdateTypeId == CON.WaiverServiceUpdateType.DODD)
                        {
                            if (CheckIfPSMPCWError(details.RegID))
                            {
                                divWaiverErrorDisplay.Visible = true;
                            }
                            else
                            {
                                divDODDUpdate.Visible = true;
                                divODAUpdate.Visible = false;
                            }
                        }
                        else if (waiverUpdateTypeId == CON.WaiverServiceUpdateType.ODA)
                        {
                            if (CheckIfPSMPCWError(details.RegID))
                            {
                                divWaiverErrorDisplay.Visible = true;
                            }
                            else
                            {
                                divDODDUpdate.Visible = false;
                                divODAUpdate.Visible = true;
                                //OHPNM-13420 : Prod - Cancel ODA Option Re-enable
                                divCancelUpdateODA.Visible = true;
                            }
                        }
                        if (waiverUpdateTypeId != CON.WaiverServiceUpdateType.ODM)
                        {
                            // OHPNM-16234 - If Provider Reactivation and WF event type is Update Reg and if RTPed then show the Continue Update link
                            if ((details.IsProviderReactivation && details.RegistrationStatusTypeID == CON.RegistrationStatusTypeId.ReturnToProvider) ||
                                 waiverUpdateTypeId == CON.WaiverServiceUpdateType.OperatorUpdate)
                            {
                                lnkContinueUpdateRegistration.Visible = true;
                            }
                            else
                                lnkContinueUpdateRegistration.Visible = false;
                            divOptKeyFields.Visible = false;
                        }
                        
                    }

                    if (details.IsInUpdateProcess > 0)
                        this.lnkCancelUpdateRegistration.Visible = true;
                    else
                        this.lnkCancelUpdateRegistration.Visible = false;

                    // OHPNM-5721
                    if (waiverUpdateTypeId == CON.WaiverServiceUpdateType.DODD || waiverUpdateTypeId == CON.WaiverServiceUpdateType.ODA)
                    {
                        // this is a DODD or ODA update; disable the Cancel Update Registration link if it's visible since that's not applicable
                        if (this.lnkCancelUpdateRegistration.Visible)
                        {
                            this.lnkCancelUpdateRegistration.Visible = false;
                        }

                        if (CheckAdditionalApplicationExist(details.RegID))
                        {
                            // there is an additional application from the sister agency for this reg_application; don't let them cancel at this point
                            this.lnkCancelUpdateDoddRegistration.Visible = false;
                            this.lnkCancelUpdateOdaRegistration.Visible = false;
                        }
                        else if (waiverUpdateTypeId == CON.WaiverServiceUpdateType.DODD)
                        {
                            // we are still in provider data entry, and the sister agency hasn't sent an application yet, and this is a DODD update; turn off the cancel ODA link in case it's currently visible
                            this.lnkCancelUpdateOdaRegistration.Visible = false;
                        }
                        else
                        {
                            // we are still in provider data entry, and the sister agency hasn't sent an application yet, and this is an ODA update; turn off the cancel DODD link in case it's currently visible
                            this.lnkCancelUpdateDoddRegistration.Visible = false;
                        }
                    }

                    // if the dodd continue/cancel links div is visible, check the WF_PARAMETERS to make sure it should be shown (we don't want to show it if the workflow was started by an automated exclusion process; those look like DODD updates be really aren't)
                    if (divDODDUpdate.Visible)
                    {
                        TurnOffDODDUpdateCancelLinksIfNotDoddUpdate(details);
                    }                    

                    break;
                case CON.WorkflowEventType.RevalReg:
                    this.divOptKeyFields.Visible = true;
                    if (!details.IsProviderReapplication)
                    {                        
                        this.divOptRevalidation.Visible = UserCanAccessReg; // SAM768 Provider admin , power agent or any provider agent with Enrollment agent access should be able to continue/cancel.
                        
                        string checkORPflag = svc.GetORPFlagforRegID(details.RegID);

                        if (checkORPflag == "True")
                        {
                            this.lnkContinueRevalidation.Text = "Continue ORP Conversion";
                            //this.lnkCancelConvertORP.Text = "Cancel Convert from ORP";
                            divCancelConvertORP.Visible = true;
                        }
                    }
                    else
                    {
                        if(Helper.IsEligibleForReapplicationOrReactivation(details.NPI, details.RegID,details.MMISProviderTypeID))
                        {
                            this.divReapplicationProvider.Visible = true;
                        }                       
                    }

                    //Cancel revalidation is not allowed.
                    break;
                case CON.WorkflowEventType.ChangeProviderType:
                    var newReg = svc.SelectProviderTypeChangeRequestByNewReg(this.RegID, this.KeyFields.ProcessID);
                    if (Helper.HasRows(newReg))
                    {
                        if (newReg.Tables[0].Select("STATUS=0").Any())
                            divOptContinueOrCancelProviderTypeChange.Visible = true;
                    }
                    break;
                case CON.WorkflowEventType.SiteVisitEvent:  //SAM538 New Site Visit Event
                    divOptNewRegistration.Visible = true;
                    lnkContinueRegistration.Visible = true;
                    lnkContinueDODDRegistration.Visible = false;
                    lnkContinueODARegistration.Visible = false;
                    lnkCancelNewRegistration.Visible = false;
                    break;
                //TODO:  Reactivation--initially coded to be always hidden
                default:                    
                    break;
            }

            // OHPNM-20057 Disable Cancel Provider Update If registration status = Return to Provider For Site Visit
            if (!string.IsNullOrEmpty(details.RegistrationStatusType) && details.RegistrationStatusType == "Return to Provider For Site Visit")
            {
                this.lnkCancelUpdateDoddRegistration.Visible = false;
                this.lnkCancelUpdateOdaRegistration.Visible = false;
                this.lnkCancelUpdateRegistration.Visible = false;
                this.lnkCancelNewRegistration.Visible = false;
            }

            if (details.WorkflowID == CON.WorkflowType.IncidentCompliance)
            {
                this.divOptNewRegistration.Visible = true;
            }
            //OHPNM-19509 - Hide cancel link for provider if RTP in a Credential event WF.
            if(details.WorkflowID == CON.WorkflowType.CredentialingApplication)
            {
                divDODDUpdate.Visible = false;
                divODAUpdate.Visible = false;
                lnkCancelUpdateRegistration.Visible = false;
                lnkCancelNewRegistration.Visible = false;
                lnkCancelReapplicationProvider.Visible = false;
                divCancelConvertORP.Visible = false;
            }
        }
        else
        {
            SetCPCVisibility();
            SetCPClinkVisibility(details);
            SetCMClinkVisibility(details);
        }


    }
    private bool IsrevalidationNeeded(ProviderManagementData details)
    {
        bool revalidationRequired = false;
        if (details.RevalidationDate.HasValue)
        {
            if ((details.RevalidationDate.Value.AddDays(RevalDueWindow) <= DateTime.Today && details.EnrollmentStatusCode != CON.EnrollmentStatusTypeID.InActive.ToString()) ||
               (details.enrollmentStatusReason == CON.EnrollStatusReason.FAILURETOREVALIDATE && details.IsCredentialingProvider == false && details.EnrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive.ToString()))
            {
                revalidationRequired = true;
            }

            //OHPNM-14113 - Temporarily show Begin Revalidation link for Credentialed providers past their revalidation date
            if ((details.IsCredentialingProvider && details.RevalidationDate < DateTime.Today && AppSettings.Get("ShowRevalLinkCredProviders") == "false") 
                || details.WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD)            
            {
                revalidationRequired = false;
            }

            // OHPNM-16029 - Show "Begin Revalidation" link to PT types 01,02,03,28,86,88,89 even after their reval date is passed as these get restricted services and be active
            //SAM635 ADD 59,74 AND 10 TO THIS LIST
            if ((MMISProviderTypeId == "01" || MMISProviderTypeId == "02" || MMISProviderTypeId == "03" ||
                MMISProviderTypeId == "28" || MMISProviderTypeId == "86" || MMISProviderTypeId == "88" ||
                MMISProviderTypeId == "89" || MMISProviderTypeId == "59" || MMISProviderTypeId == "74" ||   
                MMISProviderTypeId == "10") && details.RevalidationDate < DateTime.Today && details.EnrollmentStatusCode != CON.EnrollmentStatusTypeID.InActive.ToString())
            {
                revalidationRequired = true;
            }
        }
        return revalidationRequired;
    }
    private void ShowMaintenanceModeLinks(ProviderManagementData details)
    {
        if (!Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent)) this.divOptViewOnly.Visible = true;
        if(IsPowerAgent) this.divOptViewOnly.Visible = true;
        this.divOptKeyFields.Visible = true;
        bool isTerminated = details.TerminationDate.HasValue;
        //reval due window is a negative #
        bool revalidationNeeded = IsrevalidationNeeded(details); //false
        bool hasReferral = details.ReferralID > 0 && (details.ReferralTypeID != CON.DiddReferralType.AssistedLiving);//false
        bool canAddAffiliates = false;// 3207 Add group member link disabled
                                      //details.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Group && details.ProviderCategoryTypeID != CON.ProviderCategoryTypeID.GroupMemberProfile && !hasReferral;
        this.divOptRevalidationNeeded.Visible = revalidationNeeded;
        this.lnkReactivateProvider.Enabled = false;
        string MMISProviderTypeId = GetMMISProviderTypeID(KeyFields.ProviderTypeID);
        switch (details.WorkflowID)
        {
            case CON.WorkflowType.GroupMemberProfile:
                this.divOptNoActiveWorkflowGMP.Visible = true;
                break;
            default:
                this.divOptNoActiveWorklowNonGMP.Visible = this.divOptODMUpdate.Visible = true;
                this.divOptRevalidationNeeded.Visible = revalidationNeeded;
                this.lnkConvertFromORP.Visible = (details.ApplicationTypeID == CON.ApplicationType.ORP && Helper.IsEligibleConvertFromORP(this.RegID)) ? true : false; // SAM719 Allow Termed ORP providers to start conversion to std wf

                this.lnkUpdateOwnershipInfo.Visible = false;//waiting on reqts !revalidationNeeded && !hasReferral;                
                ShowReconsiderationLink(details);
                this.lnkAddAffiliation.Visible = canAddAffiliates;
                this.lnkUpdateServices.Visible = !revalidationNeeded && hasReferral;
                this.lnkReactivateProvider.Visible = false;
                SetUpdateLinksVisibility(revalidationNeeded, hasReferral, details);
                SetCPCVisibility();
                SetCPClinkVisibility(details);
                SetCMClinkVisibility(details);
                lnkAddODMorODAMedSvc.Visible = this.KeyFields.WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD && HasDDContractNum ? true : false;
                break;
        }
		
        // OHPNM-8007 - If "Begin Revalidation" link is visible and DODD only provider and provider is terminated for a reason other than 'Failure to Revalidate', do not display the Begin Revalidation link. (note: lines 1557/1558 above seems like it should be taking care of this)
        if (this.divOptRevalidationNeeded.Visible && (details.HasDODDSpecialty && !details.HasODASpecialty && !details.HasODMSpecialty) && isTerminated && details.enrollmentStatusReason != CON.EnrollStatusReason.FAILURETOREVALIDATE)
        {
            // turn off "Begin Revalidation" link
            this.divOptRevalidationNeeded.Visible = false;
        }
        var oldReg = svc.SelectProviderTypeChangeRequestByOldReg(this.RegID);
        Helper.IsProviderTypeChangeWfInProgress = false;
        if (Helper.HasRows(oldReg))
        {
            if (oldReg.Tables[0].Select("STATUS=0").Any())
            {
                Helper.IsProviderTypeChangeWfInProgress = true;
            }
        }
        
        if (Helper.IsShowProviderTypeChangeLink(MMISProviderTypeId) && details.EnrollmentStatusCode == CON.EnrollmentStatusTypeID.Active.ToString()
            && details.ApplicationTypeID == CON.ApplicationType.Standard && details.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual
             && (!Helper.HasRows(oldReg) || !Helper.IsProviderTypeChangeWfInProgress))
        {
            divOptProviderTypeChange.Visible = true;
        }
        // OHPNM-17591 - Do not allow originating provider that started the change PT WF to not start another WF until ProviderTypeChange wf is complete.
        if (Helper.IsProviderTypeChangeWfInProgress) 
        {
            this.divOptRevalidationNeeded.Visible = false;
            this.divOptODMUpdate.Visible = false;
            this.divOptNoActiveWorklowNonGMP.Visible = false;
            this.divOptKeyFields.Visible = false;
            this.divDisEnrollment.Visible = false;
            divCPTHelpText.Visible = true;
        }

    }
    public void SetUpdateLinksVisibility(bool revalidationNeeded, bool hasReferral, ProviderManagementData details)
    {
        if (this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Terminated
             || this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Denied
             || this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Conversion
              || this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Disenrolled
             || this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Maintenance
             || this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.NotProcessed
             )
        {
            if ((this.KeyFields.CurrentStepID == 0 || this.KeyFields.CurrentStepID == -1))
            {
                lnkBeginDODDUpdate.Visible = !revalidationNeeded && Helper.IsProviderTypeInDODDForWaiverServiceUpdate(MMISProviderTypeId);

                // OHPNM-8678 - turn off ODA links if they aren't in the acceptable list of ODA provider types OHPNM-16231 - Donot show ODA link if they have ODA specialty even if end dated.
                if (Helper.IsProviderTypeInODAForWaiverServiceUpdate(MMISProviderTypeId) && !HasODASpecialty && !string.IsNullOrEmpty(this.KeyFields.MedicaidID) && !revalidationNeeded)
                {
                    lnkAddODAServices.Visible = true;
                }
                else
                {
                    lnkAddODAServices.Visible = false;
                }

                divDODDUpdate.Visible = false;
                divODAUpdate.Visible = false;

                if (this.KeyFields.EnrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive.ToString())
                {
                    this.lnkUpdateRegistration.Visible = (HasActiveDODDSpecialty || HasActiveODASpecialty) ? true : false; // Begin ODM Enrollment Update Link
                    //OHPNM-16766 SAM479 - If a provider does not have an NPI they should see the reapplication link
                    if (!String.IsNullOrEmpty(details.NPI) && !Helper.IsEligibleForReapplicationOrReactivation(details.NPI, details.RegID,details.MMISProviderTypeID))//OHPNM-13835- Allow reactivate only when there is no active provider with NPI
                    {
                        lnkBeginReapplication.Visible = false;
                    }
                    else
                    {
                        if (details.IsCredentialingProvider)
                        {
                            lnkBeginReapplication.Visible = !IsNewRegistration ? true : false;
                        }                        
                        else
                        {
                            // 1.Don't show for new registrations or Denied providers
                            // 2.Don't show for terminated providers that failed to revalidate
                            
                            if(IsNewRegistration || this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Denied ||
                               (this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Terminated && 
                                    details.enrollmentStatusReason == CON.EnrollStatusReason.FAILURETOREVALIDATE))
                            {
                                lnkBeginReapplication.Visible = false;
                            }
                            else
                            {
                                if (((this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Terminated ||
                                        this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.NotProcessed ||
                                        this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Conversion || 
                                        this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Disenrolled)
                                       && details.enrollmentStatusReason != CON.EnrollStatusReason.FAILURETOREVALIDATE)                                       
                                   )
                                {
                                    lnkBeginReapplication.Visible = true;
                                }
                                else
                                    lnkBeginReapplication.Visible = false;
                            }
                        }
                    }

                    if (this.KeyFields.WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD)  //Non - Medicaid Provider
                    {
                        lnkBeginDODDUpdate.Visible = this.KeyFields.EnrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive.ToString() && HasDDContractNum ? true : false;
                        lnkBeginReapplication.Visible = false;
                    }
                    else
                    {
                        // CR318 - OHPNM-13524 Do not show DD update link on terminated PT96
                        if (MMISProviderTypeId == CON.MMISProviderType.Behavioral_Health_Para_Professionals)
                        {
                            lnkBeginDODDUpdate.Visible = false;
                            this.lnkUpdateRegistration.Visible = false;
                        }
                        else
                            lnkBeginDODDUpdate.Visible = this.KeyFields.EnrollmentStatusCode == CON.EnrollmentStatusTypeID.InActive.ToString()
                                       && Helper.IsProviderTypeInDODDForWaiverServiceUpdate(MMISProviderTypeId) && !string.IsNullOrEmpty(this.KeyFields.MedicaidID) ? true : false;
                    }                       

                                    
                }
                else
                {
                    //OHPNM-15326 - Display Begin ODM Update link for DD Non-Medicaid providers
                    this.lnkUpdateRegistration.Visible = (!revalidationNeeded && !hasReferral && (!string.IsNullOrEmpty(this.KeyFields.MedicaidID) || details.WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD)); // Begin ODM Enrollment Update Link
                    
                    List<string> RevalidationLinkAllowedCredentialProviderTypes = new List<string>() { "01", "02", "03", "28", "86", "88", "89", "59", "74", "10" }; //SAM635 ADD 59,74 AND 10 TO THIS LIST
                    if (RevalidationLinkAllowedCredentialProviderTypes.Contains(details.MMISProviderTypeID)
                        && details.enrollmentStatusReason == CON.EnrollStatusReason.FAILURETOREVALIDATE && details.RevalidationDate < DateTime.Today)
                    {
                        divOptRevalidationNeeded.Visible = true;
                    }
                }

                if (details.RevalidationDate.HasValue)
                {
                    if (DateTime.Today >= details.RevalidationDate.Value.AddDays(-RevalDueWindow) && DateTime.Today < details.RevalidationDate.Value
                        && revalidationNeeded)
                    {
                        this.lnkUpdateRegistration.Visible = false;
                        lnkBeginDODDUpdate.Visible = false;
                        lnkAddODAServices.Visible = false;
                    }
                }
            }
            // OHPNM-6734 START - For SSA providers, application type of waiver or if it has ODM or ODA specialties, that have been denied on initial applications, they should not see the Add ODA Services, Begin Reapplication or Begin DODD Update links
            if (details.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Denied && details.WorkflowEventType == CON.WorkflowEventType.NewReg)
            {
                // make sure we have additional application
                if (CheckAdditionalApplicationExist(details.RegID))
                {
                    // see if this is an "Initial" application (App_Status = 52) and if its type is "Recommended Denial" (application_type = 24)
                    if (AdditionalApplicationStatusID.Equals(CON.ApplicationStatus.Recommended_Denial_ID) && AdditionalApplicationTypeID.Equals(CON.AdditionalApplicationType.Initial_ID))
                    {
                        // disable begin application, dodd update, and add oda services
                        lnkBeginReapplication.Visible = false;
                        lnkBeginDODDUpdate.Visible = false;
                        lnkAddODAServices.Visible = false;
                    }
                }
            }
            // OHPNM-6734 END         
           
        }
        //SAM537 Show ODM Update link on Suspended providers
        //OHPNM-11183 - Display "Begin DODD Enrollment Profile Update" even if the provider is Suspended with ODM (PROG-CR346) 
        if (this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Suspended)
        {
            SetSuspendedProvLinksVisibility();
        }
        SetDODDVisibility();
    }

    //SAM537 Show ODM Update link on Suspended providers
    private void SetSuspendedProvLinksVisibility()
    {            
        lnkBeginDODDUpdate.Visible = Helper.IsProviderTypeInDODDForWaiverServiceUpdate(MMISProviderTypeId);
        this.lnkUpdateRegistration.Visible = enableCR537;
        this.divOptODMUpdate.Visible = enableCR537;
        //divOptUpdateProvider.Visible = false;
        divODAUpdate.Visible = false;
        divOptRevalidationNeeded.Visible = false;
        divOptNoActiveWorkflowGMP.Visible = false;
        divOptNewGroupMbrProfile.Visible = false;
        divOptNewRegistration.Visible = false;            
        divReapplicationProvider.Visible = false;
        divOptUpdateOwnership.Visible = false;        
        divCPCInfolink.Visible = false;
        lnkBeginReapplication.Visible = false;
        divDisEnrollment.Visible = false;
        this.divPseStatus.Visible = false;
        this.divApplicationStatus.Visible = false;
        this.divOptKeyFields.Visible = false;
        this.divDisEnrollment.Visible = false;
        this.lnkUpdateOwnershipInfo.Visible = false;
        this.lnkUpdateServices.Visible = false;
        this.lnkAddAffiliation.Visible = false;
        this.lnkConvertFromORP.Visible = false;
        this.lnkBeginReapplication.Visible = false;
        this.lnkAddODAServices.Visible = false;
        this.lnkAddODMorODAMedSvc.Visible = false;

    }
    public void ShowReconsiderationLink(ProviderManagementData details)
    {        
        if ((details.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Denied ||
            details.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Terminated ||
            (details.TerminationDate.HasValue && details.EnrollmentStatusCode == CON.EnrollStatus.INACTIVE.ToString()))
            && (AppSettings.Get("ReconsiderationEnrollmentReasons").Contains(details.enrollmentStatusReason.ToString())) && (details.CurrentStepID == 0 || details.CurrentStepID == -1))
        {
            if (details.EndDate <= DateTime.Now.Date && details.EndDate >= DateTime.Now.AddDays(Convert.ToInt32(AppSettings.Get("ReconsiderationWindow")) * -1).Date)
            {
                divRequestReconsideration.Visible = lnkRequestReconsideration.Visible = true;
            }
        }
    }
    private void TurnOffWorkflowLinks()
    {
        this.lnkUpdateRegistration.Visible = false;
        this.divOptNoActiveWorkflowGMP.Visible = false;
        this.divOptRevalidationNeeded.Visible = false;
        this.lnkAddAffiliation.Visible = false;
        this.lnkUpdateServices.Visible = false;
        this.divOptNewGroupMbrProfile.Visible = false;
        this.divOptUpdateProvider.Visible = false;
        this.divOptUpdateOwnership.Visible = false;
        this.divOptServicesReferral.Visible = false;
        this.divOptAddAffiliation.Visible = false;
        this.divOptRevalidation.Visible = false;
        this.divOptRevalidation.Visible = false;
        this.divOptViewOnly.Visible = false;
        DataRow provRow = Registration.GetProviderInfo(this.KeyFields.RegID);
        string provider_type_id = Helper.GetString("MMIS_Provider_Type_ID", provRow);


        DataSet ds = svc.GetWFProcessByRegId(RegID);

        bool isInitiateCHOP = false;
        if (Helper.HasRows(ds))
        {
            int currentStepID = Helper.GetInt("CURRENT_STEP_ID", ds.Tables[0].Rows[0]);
            if (currentStepID > 0)
            {                
                isInitiateCHOP = false;                
            }
            else
            {              
                isInitiateCHOP = true;
            }

        }

        this.divInitiateCHOP.Visible = ((provider_type_id.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR) || provider_type_id.Equals(CON.LTCProviderTypes.NursingFacility) ||
            provider_type_id.Equals(CON.LTCProviderTypes.STATE_OPERATED_ICF_MR)) && this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Maintenance && isInitiateCHOP);
        this.divSubmit90DayClosure.Visible = provider_type_id.Equals(CON.LTCProviderTypes.NursingFacility) && this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Maintenance;

        if (this.KeyFields.ApplicationTypeID != CON.ApplicationType.CPC)
        {
            this.divCPCInfolink.Visible = false;
            this.divCPCMessage.Visible = false;
            this.divCreateCPCIndividual.Visible = false;
            this.divCreatePracticePartnership.Visible = false;
            this.divContinueCPCApplication.Visible = false;
            this.divUpdateCPCcontact.Visible = false;
            this.divReattestCPCIndividualorPracticePartnership.Visible = false;
            this.divBeginCPCEnrollmentUpdate.Visible = false;
        }
    }

    public void BeginUpdateRegistration(int workflowID, bool? changeApplicationTypeID = false, bool? isProviderReactivation = false, bool? isProviderReapplication = false, bool isProviderDisenrolling = false, int WaiverServiceUpdate = 0, int workfloweventtype = 0, bool keyFieldEditRequest = false, bool adminKeyFieldEditRequest = false)
    {

        Model = new ProviderManagementData();
        Model.RegID = this.KeyFields.RegID;
        Model.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        Model.ReferralID = this.KeyFields.ReferralID;
        Model.ReferralTypeID = this.KeyFields.ReferralTypeID;
        Model.WorkflowID = workflowID;
        Model.IsOwnershipUpdate = false;    // workflowID == CON.WorkflowType.RegistrationUpdateOwner
        Model.PaperRequestQueueID = this.KeyFields.PaperRequestQueueID;
        Model.EnrollmentStatusCode = this.KeyFields.EnrollmentStatusCode;

        if (this.KeyFields.ChangeEffectiveDate.HasValue)
        {
            Model.ChangeEffectiveDate = this.KeyFields.ChangeEffectiveDate.Value;
        }
        if (this.KeyFields.RequestedEffectiveDate.HasValue)
        {
            Model.RequestedEffectiveDate = this.KeyFields.RequestedEffectiveDate.Value;
        }
        Model.IsRevalidation = IsRevalidation;
        Model.ApplicationTypeID = this.KeyFields.ApplicationTypeID;
        Model.WaiverTypeID = this.KeyFields.WaiverTypeID;
        Model.IsCredentialingProvider = this.KeyFields.IsCredentialingProvider;
        Model.enrollmentStatusReason = this.KeyFields.EnrollmentStatusReason;
        Model.FormCompletionName = this.KeyFields.FormCompletionName;
        Model.FormCompletionPhone = this.KeyFields.FormCompletionPhone;

        Model.LastModifiedDate = DateTime.Now;
        Model.LastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        Model.RevalidationDate = KeyFields.RevalidationDate;
        //bool ConvertedProvider = (KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Conversion);
        bool revalidationNeeded = false;

        if (KeyFields.RevalidationDate.HasValue)
        {
            revalidationNeeded = IsrevalidationNeeded(Model);
        }
        Model.WorkflowEventType = !adminKeyFieldEditRequest && !isProviderDisenrolling && revalidationNeeded && this.KeyFields.ApplicationTypeID != CON.ApplicationType.CPC &&
                                      (WaiverServiceUpdate != CON.WaiverServiceUpdateType.DODD && WaiverServiceUpdate != CON.WaiverServiceUpdateType.ODA)
                                  ? CON.WorkflowEventType.RevalReg : CON.WorkflowEventType.UpdateReg;
        if (changeApplicationTypeID == true)
        {
            Model.WorkflowEventType = CON.WorkflowEventType.RevalReg;
            //svc.UpdateORPFlag(this.KeyFields.RegID, true);
        }
        if (workfloweventtype == CON.WorkflowEventType.Reconsideration)
        {
            Model.WorkflowEventType = CON.WorkflowEventType.Reconsideration;
        }
        Model.IsProviderReactivation = (isProviderReactivation == null || isProviderReactivation == false) ? false : true;
        if (keyFieldEditRequest && this.KeyFields.EnrollmentStatusCode == CON.EnrollStatus.INACTIVE.ToString() && this.KeyFields.IsTerminated)
        {
            Model.IsProviderReapplication = true;
        }
        else
        {
            Model.IsProviderReapplication = (isProviderReapplication == null || isProviderReapplication == false) ? false : true;
        }
        if (Model.IsProviderReapplication)
        {
            Model.WorkflowEventType = CON.WorkflowEventType.RevalReg;
        }
        if (workfloweventtype == CON.WorkflowEventType.CPCReattest)
        {
            Model.WorkflowEventType = CON.WorkflowEventType.CPCReattest;
        }
        if (workfloweventtype == CON.WorkflowEventType.CMCUpdate)
        {
            Model.WorkflowEventType = CON.WorkflowEventType.CMCUpdate;
        }
        if (workfloweventtype == CON.WorkflowEventType.CMCReAttest)
        {
            Model.WorkflowEventType = CON.WorkflowEventType.CMCReAttest;
        }
        //SAM537 only create update workflow on suspended provider 
        if(enableCR537 && WaiverServiceUpdate == CON.WaiverServiceUpdateType.ODM && this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Suspended)
        {
            Model.WorkflowEventType = CON.WorkflowEventType.UpdateReg;
        }
        Model.MMISCPCPracticeTypeID = this.KeyFields.MMISCPCPracticeTypeID;
        Model.IsAddODMorODAMedicaid = IsAddODMorODAMedicaidSvc;
		
        // OHPNM-8896 - check one more time to make sure we aren't in an active workflow
        DataSet ds = svc.GetWFProcessByRegId(RegID);
        bool okToStartAWorkflow = true;
        if (Helper.HasRows(ds))
        {
            int currentStepID = Helper.GetInt("CURRENT_STEP_ID", ds.Tables[0].Rows[0]);
            if (currentStepID > 0)
            {
                okToStartAWorkflow = false;
            }
        }
        if (okToStartAWorkflow)
        {
            presenter.TransferLiveToReg(changeApplicationTypeID, isProviderDisenrolling, WaiverServiceUpdate, CPC_Program_Year, Helper.IsUpdateCPCContact,this.KeyFields.IsTerminated);
        }

        // OHPNM-2519
        (this.Page as RegistrationProvider).RegistrationId = this.KeyFields.RegID;

        try
        {
            ProviderFeedHelper.InsertProviderFeedNotes(this.KeyFields.RegID, 0, HttpContext.Current.User.Identity.Name, "Workflow Started", finalDisposition: CON.FinalDisposition.NotSubmitted);
        }
        catch (Exception ex)
        {
            Logging log = new Logging(Guid.NewGuid(), string.Empty);
            log.CreateLogEntry("Error while adding note in provide feed table for update registration" + ex.ToString(), Logging.LogPriority.Error);
        }
    }


    #endregion

    protected void lnkConvertfromORP_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        BeginUpdateRegistration(CON.ApplicationType.Standard, true, false);
    }
   
    protected void BtnAdd_CostReport(object sender, CommandEventArgs e)
    {
        string url = string.Empty;
        url = AppSettings.Get("CostReportsLink");
        Response.Redirect(url);
    }

    protected void BtnAdd_Correspondence(object sender, CommandEventArgs e)
    {
        //
        Session["StepId"] = Convert.ToInt32(CON.SectionTypeID.Correspondence);
        Session["RegId"] = this.RegID;
        Response.Redirect(string.Format("~/Process/ProviderCorrespondence.aspx"));
    }
    protected void BtnAdd_Remittance(object sender, CommandEventArgs e)
    {
        Session["StepId"] = Convert.ToInt32(CON.SectionTypeID.SearcheRA);
        Session["RegId"] = this.RegID;
        Response.Redirect(string.Format("~/Process/ERemittanceAdvice.aspx"));
    }

    private void AddDisEntrollmentOptions()
    {

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataSet ds = new DataSet();
        ds = psc.SelectProviderDisenrollmentxref();
        if (ds.Tables[0].Rows.Count != 0)
        {
            disEnrollmentOptions.DataSource = ds;
            disEnrollmentOptions.DataTextField = "PROVIDER_DISENROLLMENT_NAME";
            disEnrollmentOptions.DataValueField = "PROVIDER_DISENROLLMENT_XREF_ID";
            disEnrollmentOptions.DataBind();
        }

        DataSet pds = new DataSet();
        int regid = this.KeyFields.RegID;

        //OHPNM-859 pschwarz 11/4/2020: changed from status type to the status type  id
        divDisEnrollment.Visible = lnkDisEnrollment.Visible = ((this.KeyFields.EnrollmentStatusCode == CON.EnrollmentStatusTypeID.Active.ToString() || this.KeyFields.EnrollmentStatusCode == CON.EnrollmentStatusTypeID.ORPActive.ToString())
            && (this.KeyFields.CurrentStepID == 0 || this.KeyFields.CurrentStepID == -1) && this.KeyFields.ApplicationTypeID != CON.ApplicationType.CPC
            && (!(Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent) && !IsPowerAgent))) ? true : false;

        //OHPNM-13416:All the ICF's should not display Disenrollment link. OHPNM-16042 -PT 86 should not have disenroll option
        if (MMISProviderTypeId == CON.MMISProviderType.NON_STATE_OPERATED_ICF_MR || MMISProviderTypeId == CON.MMISProviderType.STATE_OPERATED_ICF_MR 
            || MMISProviderTypeId == CON.MMISProviderType.NURSING_FACILITY || this.KeyFields.RegistrationProgramStatusID == CON.RegistrationProgramStatusTypeId.Suspended)
        {
            divDisEnrollment.Visible = lnkDisEnrollment.Visible = false;
        }

        if(Helper.IsProviderTypeChangeWfInProgress)
            divDisEnrollment.Visible = lnkDisEnrollment.Visible = false;

    }

    private void SetDODDVisibility()
    {
        divDODD.Visible = (this.KeyFields.WaiverTypeID == CON.WaiverApplicationTypeID.DODD || this.KeyFields.WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD)
                             || (Helper.IsProviderTypeInDODDForWaiverServiceUpdate(MMISProviderTypeId) && HasDDContractNum);
        // OHPNM-11125 Don't show DODD Cert dates for Provider types 88 or 89
        if (divDODD.Visible)
        {
            if (MMISProviderTypeId == "88" || MMISProviderTypeId == "89")
            {
                divDODDCertStartDate.Visible = false;
                divDODDCertEndDate.Visible = false;
            }
        }
    }

    private void SetCPCVisibility()
    {
        if (this.KeyFields.ApplicationTypeID == CON.ApplicationType.CPC)
        {
            divCPCInfolink.Visible = true;
            divDODDUpdate.Visible = false;
            divODAUpdate.Visible = false;
            divOptRevalidationNeeded.Visible = false;
            divOptNoActiveWorkflowGMP.Visible = false;
            divOptNewGroupMbrProfile.Visible = false;
            divOptNewRegistration.Visible = false;
            divOptUpdateProvider.Visible = false;
            divReapplicationProvider.Visible = false;
            divOptUpdateOwnership.Visible = false;
            divOptNoActiveWorklowNonGMP.Visible = false;
            divOptODMUpdate.Visible = false;
            lnkBeginReapplication.Visible = false;
            divDisEnrollment.Visible = false;
            this.divPseStatus.Visible = false;
            this.divApplicationStatus.Visible = false;
	    this.divRequestReconsideration.Visible = lnkRequestReconsideration.Visible = false;
        }
        //this.setCPClinkVisibility();

    }


    private void SetCPClinkVisibility(ProviderManagementData details)
    {
        bool isCPCEnrollmentOrCPCLinkReenabled = false;

        if ((Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) || IsPowerAgent ||
            (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent)
              && Helper.IsUserInSubRole(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.UserRoleType.CPCAgent)))
            )
        {
            DataSet ds = svc.GetCPCLinkVisibility(details.RegID, details.CurrentStepID, details.MMISProviderTypeID, details.ProviderCategoryTypeID, CPC_Program_Year, details.MMISCPCPracticeTypeID);

            if (Helper.isCPCEnrollmentPeriod() || (!Helper.isCPCEnrollmentPeriod() && Helper.isCPCLinkReenabled(details.RegID)))
            {
                isCPCEnrollmentOrCPCLinkReenabled = true;
            }

            if (Helper.HasRows(ds))
            {
                // OHPNM-8359 - for links other than the contact update, we have to be in open CPC enrollment or CPCLinkReenabled needs to be true
                if (isCPCEnrollmentOrCPCLinkReenabled)
                { 
                    this.divCreateCPCIndividual.Visible = Helper.GetBool("SHOW_CREATE_CPC_IND_LINK", ds.Tables[0].Rows[0]);
                    this.divCreatePracticePartnership.Visible = Helper.GetBool("SHOW_CREATE_CPC_PP_LINK", ds.Tables[0].Rows[0]);
                    this.divReattestCPCIndividualorPracticePartnership.Visible = Helper.GetBool("SHOW_REATTEST_CPC_LINK", ds.Tables[0].Rows[0]);
                    this.divBeginCPCEnrollmentUpdate.Visible = Helper.GetBool("SHOW_UPDATE_CPC_REG_LINK", ds.Tables[0].Rows[0]) && !this.divReattestCPCIndividualorPracticePartnership.Visible;
                }

                // OHPNM-8359 - it's always OK to show this update contact link as long as the stored procedure says it is
                this.divUpdateCPCcontact.Visible = Helper.GetBool("SHOW_UPDATE_CPC_CONTACT_LINK", ds.Tables[0].Rows[0]);

                if (this.divCreateCPCIndividual.Visible || this.divCreatePracticePartnership.Visible || this.divReattestCPCIndividualorPracticePartnership.Visible
                    || this.divBeginCPCEnrollmentUpdate.Visible || this.divUpdateCPCcontact.Visible)
                {
                    divCPCInfolink.Visible = true;
                }

                if(!this.divCPCCounts.Visible)
                {
                    this.divCPCCounts.Visible = Helper.GetInt("TOTAL_MEM_CNT", ds.Tables[0].Rows[0]) > 0 ? true : false;
                }
                
                this.divCPCEnrollmentCounts.Visible = Helper.GetInt("TOTAL_MEM_CNT", ds.Tables[0].Rows[0]) > 0 ? true : false;

                if (this.divCPCEnrollmentCounts.Visible)
                {
                    lblCPCMemcnt.Text = Helper.GetInt("TOTAL_MEM_CNT", ds.Tables[0].Rows[0]).ToString();
                    lblCPCPedMemCnt.Text = Helper.GetInt("TOTAL_PED_MEM_CNT", ds.Tables[0].Rows[0]).ToString();
                }
            }

            if ((details.WorkflowID == CON.WorkflowType.CPC && details.InProviderDataEntry))
            {
                this.divContinueCPCApplication.Visible = details.WorkflowEventType != CON.WorkflowEventType.UpdateReg ? true : false; 
                this.divContinueCPCUpdate.Visible = details.WorkflowEventType == CON.WorkflowEventType.UpdateReg && !Helper.IsUpdateCPCContact ? true : false;
                this.divContinueCPCContactUpdate.Visible = details.WorkflowEventType == CON.WorkflowEventType.UpdateReg && Helper.IsUpdateCPCContact ? true : false;
                divCancelCPCUpdate.Visible = details.WorkflowEventType == CON.WorkflowEventType.UpdateReg ? true : false;

                if (isCPCEnrollmentOrCPCLinkReenabled)
                {
                    this.divUpdateCPCcontact.Visible = false;
                    divCPCInfolink.Visible = true;
                    divCancelCPCApplication.Visible = details.WorkflowEventType != CON.WorkflowEventType.UpdateReg ? true : false;                    
                }
            }
        }
        if ((Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) || IsPowerAgent ||
            (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent)
              && Helper.IsUserInSubRole(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.UserRoleType.CPCAgent)))
              && !Helper.isCPCEnrollmentPeriod())
        {
            if (divCPCInfolink.Visible == true && !isCPCEnrollmentOrCPCLinkReenabled)
            {
                divCPCMessage.Visible = true;
            }
        }
    }

    private void SetCMClinkVisibility(ProviderManagementData details)
    {
        if ((Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) || IsPowerAgent ||
            (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent) && Helper.IsUserInSubRole(details.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.UserRoleType.CPCAgent))))
        {
            DataSet ds = svc.GetCMCLinkVisibility(details.RegID, details.CurrentStepID, details.MMISProviderTypeID, details.ProviderCategoryTypeID);
            if (Helper.isCMCEnrollmentPeriod() || (!Helper.isCMCEnrollmentPeriod() && Helper.isCMCLinkReenabled(details.RegID)))
            {                
                if (Helper.HasRows(ds))
                {
                    this.divInitiateCMCEnroll.Visible = Helper.GetBool("SHOW_ENROLL_CMC_REG_LINK", ds.Tables[0].Rows[0]);
                    this.divReattestCMCProvider.Visible = Helper.GetBool("SHOW_REATTEST_CMC_REG_LINK", ds.Tables[0].Rows[0]);
                    this.divUpdateCMCContact.Visible = Helper.GetBool("SHOW_UPDATE_CMC_CONTACT_LINK", ds.Tables[0].Rows[0]);
                }
                if (details.WorkflowID == CON.WorkflowType.CMC && details.InProviderDataEntry)
                {
                    this.divContinueCMCApp.Visible = details.WorkflowEventType != CON.WorkflowEventType.CMCUpdate; 
                    this.divContinueCMCUpdateApp.Visible = details.WorkflowEventType == CON.WorkflowEventType.CMCUpdate;
                    this.divCancelCMCApplication.Visible = details.WorkflowEventType != CON.WorkflowEventType.CMCUpdate;
                    this.divCancelCMCUpdate.Visible = details.WorkflowEventType == CON.WorkflowEventType.CMCUpdate;
                }
            }
            else
            {
                this.divReattestCMCProvider.Visible = false;
                this.divInitiateCMCEnroll.Visible = false;
                this.divUpdateCMCContact.Visible = false;
            }
            //OHPNM-18429 - Always show CMC Contact update link
            if (!details.InProviderDataEntry)
            {
                this.divUpdateCMCContact.Visible = Helper.GetBool("SHOW_UPDATE_CMC_CONTACT_LINK", ds.Tables[0].Rows[0]);
            }
            if (details.WorkflowID == CON.WorkflowType.CMC && details.InProviderDataEntry && details.WorkflowEventType == CON.WorkflowEventType.CMCUpdate)
            {
                this.divContinueCMCApp.Visible = details.WorkflowEventType != CON.WorkflowEventType.CMCUpdate;
                this.divContinueCMCUpdateApp.Visible = details.WorkflowEventType == CON.WorkflowEventType.CMCUpdate;
                this.divCancelCMCApplication.Visible = details.WorkflowEventType != CON.WorkflowEventType.CMCUpdate;
                this.divCancelCMCUpdate.Visible = details.WorkflowEventType == CON.WorkflowEventType.CMCUpdate;
            }

            if (Helper.isCMCInvited(details.RegID) || Helper.IsCMCEnrolled(details.RegID))
                divCMCInfoLink.Visible = true;
            else
                divCMCInfoLink.Visible = false;

            if (Helper.isCMCInvited(details.RegID) && !Helper.isCMCEnrollmentPeriod())
                divCMCMessage.Visible = true;
            else
                divCMCMessage.Visible = false;

            if (Helper.HasRows(ds))
            {
                if(!this.divCPCCounts.Visible)
                {
                    this.divCPCCounts.Visible = Helper.GetInt("TOT_QUALIFY_CNT", ds.Tables[0].Rows[0]) > 0 ? true : false;
                }

                this.divCMCEnrollmentCounts.Visible = Helper.GetInt("TOT_QUALIFY_CNT", ds.Tables[0].Rows[0]) > 0 ? true : false;

                if (this.divCMCEnrollmentCounts.Visible)
                {
                    lblCMCcnt.Text = Helper.GetInt("TOT_QUALIFY_CNT", ds.Tables[0].Rows[0]).ToString();
                }
            }
        }


    }

    //private bool isEnrollmentPeriod()
    //{
    //    bool isEnrollmentPeriod = true; //Finally needs to Change it to false -- Ramya;
    //    string CPCEnrollmentPeriodStartDate = AppSettings.Get("CPCEnrollmentPeriodStartDate", string.Empty) + "/" + DateTime.Now.Year;
    //    string CPCEnrollmentPeriodEndDate = AppSettings.Get("CPCEnrollmentPeriodEndDate", string.Empty) + "/" + DateTime.Now.Year;
    //    DateTime beginDate = DateTime.ParseExact(CPCEnrollmentPeriodStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    //    DateTime endDate = DateTime.ParseExact(CPCEnrollmentPeriodEndDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    //    DateTime todayDate = DateTime.Now.Date;
    //    if (todayDate >= beginDate && todayDate <= endDate)
    //    {
    //        isEnrollmentPeriod = true;
    //    }
    //    return isEnrollmentPeriod;
    //}

    //private bool isCPCLinkReenabled(int regId)
    //{
    //    bool isReenabled = false;
    //    DataSet ds = svc.SelectRegistration(regId);
    //    bool enableAttestationLink = Helper.HasRows(ds) ? Helper.GetBool("IS_CPC_LINK_REENABLED", ds.Tables[0].Rows[0]) : false;
    //    DateTime reconsiderationBeginDate = DateTime.ParseExact(AppSettings.Get("CPCreconsiderationBeginDate",string.Empty), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    //    DateTime reconsiderationEndDate = DateTime.ParseExact(AppSettings.Get("CPCreconsiderationEndDate", "1"), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    //    if (enableAttestationLink && (DateTime.Now.Date <= reconsiderationEndDate))
    //        isReenabled = true;

    //    return isReenabled;  
    //}

    private void DisplayElapsedDays()
    {
        //By Default show standard application elapsed time in provider home page as per discussion.
        int ApplicationTypeId = CON.ApplicationType.Standard;
        try
        {
            if (ApplicationTypeId > 0)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                int ElapsedDays = psc.GetElapsedDaysByApplicationType(ApplicationTypeId);
                lblCREDays.Text = Helper.HtmlEncode(string.Format(CON.RegApplicationDueTimeMessage, ElapsedDays.ToString()));
                lblCUEDays.Text = Helper.HtmlEncode(lblCREDays.Text);
            }
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }

    private bool IsValidExtension(out string errMsg, string fileName)
    {
        bool rtn = false;
        errMsg = string.Empty;

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
        if (string.IsNullOrEmpty(pattern)) return true;                 // TRUE if the configuration setting does not exist
        Regex objAlphaPattern = new Regex(pattern);
        return objAlphaPattern.IsMatch(strFileName);
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "ReconsiderationProvider";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    private bool ValidateReconsideration()
    {
        bool rtn = true;
        DateTime temp;
        if (string.IsNullOrEmpty(txtDateofReconsidertaionRequest.Text.Trim()))
        {
            AddError("* Reconsideration Effective Date is required.", ref rtn);
            return rtn;
        }
        if (!DateTime.TryParse(txtDateofReconsidertaionRequest.Text, out temp))
        {
            AddError("* A valid Reconsideration Effective Date is required (mm/dd/yyyy).", ref rtn);
            return rtn;
        }
        if (encRequestReconsiderationDoc.PostedFile == null)
        {
            AddError("* Unable to find posted file.", ref rtn);
            return rtn;
        }
        if (string.IsNullOrEmpty(encRequestReconsiderationDoc.PostedFile.FileName))
        {
            AddError("* Select a file for upload", ref rtn);
            return rtn;
        }
        if (string.IsNullOrEmpty(_DestinationPath))
        {
            AddError("* ERROR - DestinationPath must have a value", ref rtn);
            return rtn;
        }
        if (string.IsNullOrEmpty(_ValidFileExtensions))
        {
            AddError("* ERROR - ValidFileExtensions must have a value", ref rtn);
            return rtn;
        }
        string errMsg = string.Empty;
        if (!IsValidExtension(out errMsg, encRequestReconsiderationDoc.PostedFile.FileName))
        {
            AddError("* Unable to find posted file.", ref rtn);
            return rtn;
        }
        /*if (!encRequestReconsiderationDoc.HasFile)
        {
            AddError("* No File has been uploaded.", ref rtn);
            return rtn;
        }*/
        if (encRequestReconsiderationDoc.PostedFile.ContentLength > (MaxFileMegaBytes * (1000 * 1024)))
        {
            AddError("File cannot be more than " + String.Format("{0:0,0}", (MaxFileMegaBytes * (1000 * 1024))) + " bytes (" + String.Format("{0:f}", MaxFileMegaBytes) + " MB) in size.", ref rtn);
            return rtn;
        }

        if (!IsSpecialCharacter(encRequestReconsiderationDoc.FileName))
        {
            AddError("The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): " + encRequestReconsiderationDoc.FileName + " Please remove the Special Character from the file Name before upload. ", ref rtn);
            return rtn;
        }
        if (!string.IsNullOrEmpty(errMsg))
        {
            AddError("* " + errMsg, ref rtn);
            return rtn;
        }
        return rtn;
    }

    protected void btnReconsiderationSave_Click(object sender, EventArgs e)
    {
        bool rtn = true;
        if (ValidateReconsideration())
        {
            Helper.IsUpdateCPCContact = false;
            if (encRequestReconsiderationDoc.PostedFile != null)
            {
                _DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
                lblreconMsg.Text = string.Empty;

                try
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                        _DestinationPath = @"C:\Temp\";

                    string newFileName = Helper.CleanFilePath(encRequestReconsiderationDoc.FileName);
                    byte[] fileBytes = encRequestReconsiderationDoc.EncryptedFileBytes;

                    int controlID = svc.GetRegSectionUploadControlIDByRegID(RegID, CON.RegistrationPageName.Reconsideration, CON.UploadControlDocumentTitles.RequestReconsideration);

                    File.WriteAllBytes(Path.Combine(@_DestinationPath, newFileName), fileBytes);
                    lblreconMsg.Text = "File Uploaded: " + Helper.HtmlEncode(newFileName);
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", RegID.ToString());
                    parms.Add("REG_PAGE_TYPE_ID", CON.RegistrationPageType.Identification.ToString());
                    parms.Add("REG_PAGE_SECTION", "Reconsideration");
                    parms.Add("SCREENING_ACTIVITY_ID", null);
                    parms.Add("NAME", newFileName);
                    parms.Add("DESCRIPTION", "Request Reconsideration");
                    parms.Add("FILE_NAME", newFileName);
                    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("REG_SECTION_UPLOAD_CONTROL_ID", controlID.ToString());
                    parms.Add("ROW_ID", "");
                    int docID = svc.InsertRegistrationData(Convert.ToInt32(RegID), "DOCUMENT", parms);
                    SendToCMS(docID, fileBytes, newFileName);


                    if (docID > 0 && RegID > 0)
                    {
                        CreateRequestReconsideration(RegID);
                        AddError("We have received your reconsideration request.", ref rtn);
                        lnkRequestReconsideration.Visible = false;

                        (this.Page as RegistrationProvider).RegistrationId = RegID;
                        (this.Page as RegistrationProvider).IsReadOnly = false;
                        (this.Page as RegistrationProvider).RegistrationStep = CON.SectionTypeID.RECONSIDERATION;

                        string url = "~/Process/Registration.aspx?Step=" + CON.SectionTypeID.RECONSIDERATION.ToString() + "&RegId=" + RegID.ToString();
                        Response.Redirect(url, false);
                       

                    }

                    return;
                }
                catch (Exception ex)
                {
                    AddError("* No File uploaded. Error: " + ex.Message, ref rtn);
                }
            }
            else
            {
                AddError("* No Request Date or File uploaded.", ref rtn);
            }
        }
        if (Page.IsValid) { mpeRequestReconsideration.Hide(); }
        return;
    }

    private int CreateRequestReconsideration(int regID)
    {
        int reconsiderationId = 0;
        try
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", RegID.ToString());
            parms.Add("RECONSIDERATION_REQUEST_DATE", txtDateofReconsidertaionRequest.Text);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            reconsiderationId = svc.InsertRegistrationData(Convert.ToInt32(RegID), "RECONSIDERATION", parms);

            int workflowID = CON.WorkflowType.RegistrationNew;
            BeginUpdateRegistration(workflowID, false, false, false, false, 0, CON.WorkflowEventType.Reconsideration);

        }
        catch (Exception ex)
        {
            throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("ReconsiderationInsert_SaveData - " + ex.Message));

        }
        return reconsiderationId;
    }
    protected void btnClosureNoticeUpload_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        DataSet ds = svc.GetWFProcessByRegId(RegID);

        if (Helper.HasRows(ds))
        {
            int currentStepID = Helper.GetInt("CURRENT_STEP_ID", ds.Tables[0].Rows[0]);
            if (currentStepID > 0)
            {
                //Already A workflow is in Progress
                lblStatusMsg.Text = "Already another Closure Notice is in-Progress";
                goto Failure;

            }
        }

        _DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        lblStatusMsg.Text = string.Empty;
        if (fileClosureNoticeUploadFile.PostedFile == null)
        {
            lblStatusMsg.Text = "Unable to find posted file.";
            goto Failure;
        }
        if (string.IsNullOrEmpty(fileClosureNoticeUploadFile.PostedFile.FileName))
        {
            lblStatusMsg.Text = "Select a file for upload";
            goto Failure;
        }
        if (string.IsNullOrEmpty(_DestinationPath))
        {
            lblStatusMsg.Text = "ERROR - DestinationPath must have a value";
            goto Failure;
        }
        if (string.IsNullOrEmpty(_ValidFileExtensions))
        {
            lblStatusMsg.Text = "ERROR - ValidFileExtensions must have a value";
            goto Failure;
        }
        string errMsg = string.Empty;
        if (!IsValidExtension(out errMsg, fileClosureNoticeUploadFile.PostedFile.FileName))
        {
            lblStatusMsg.Text = Helper.HtmlEncode(errMsg);
            goto Failure;
        }
        if (!fileClosureNoticeUploadFile.HasFile)
        {
            lblStatusMsg.Text = "No File has been uploaded.";
            goto Failure;
        }
        if (fileClosureNoticeUploadFile.PostedFile.ContentLength > (MaxFileMegaBytes * (1000 * 1024)))
        {
            lblStatusMsg.Text = "File cannot be more than " + String.Format("{0:0,0}", (MaxFileMegaBytes * (1000 * 1024))) + " bytes (" +
                String.Format("{0:f}", MaxFileMegaBytes) + " MB) in size.";
            goto Failure;
        }

        if (!IsSpecialCharacter(fileClosureNoticeUploadFile.FileName))
        {
            lblStatusMsg.Text = "The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): " + Helper.HtmlEncode(fileClosureNoticeUploadFile.FileName) + " Please remove the Special Character from the file Name before upload. ";
            goto Failure;
        }

        if (!string.IsNullOrEmpty(errMsg))
        {
            lblStatusMsg.Text = Helper.HtmlEncode(errMsg);
            goto Failure;
        }

        try
        {
            // Use a different destination if debugging
            //#if DEBUG
            //_DestinationPath = "C:\\Projects\\PDMS\\PDMS\\FileStoreLocal\\";
            //#endif
            //_DestinationPath = "C:\\Projects\\Upload\\";

            if (System.Diagnostics.Debugger.IsAttached)
                _DestinationPath = @"C:\Temp\";


            string newFileName = Helper.CleanFilePath(fileClosureNoticeUploadFile.FileName);
            byte[] fileBytes = fileClosureNoticeUploadFile.EncryptedFileBytes;
            int controlID = svc.GetRegSectionUploadControlIDByRegID(RegID, CON.UploadControlDocumentTitles.ClosureNotice, CON.UploadControlDocumentTitles.ClosureNotice);
            // Rename the file
            File.WriteAllBytes(Path.Combine(@_DestinationPath, newFileName), fileBytes);
            lblStatusMsg.Text = "File Uploaded: " + Helper.HtmlEncode(newFileName);
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", RegID.ToString());
            parms.Add("REG_PAGE_TYPE_ID", CON.RegistrationPageType.Identification.ToString());
            parms.Add("REG_PAGE_SECTION", "ClosureNotice");
            parms.Add("SCREENING_ACTIVITY_ID", null);
            parms.Add("NAME", newFileName);
            parms.Add("DESCRIPTION", "90 Days Closure Notice");
            parms.Add("FILE_NAME", newFileName);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("REG_SECTION_UPLOAD_CONTROL_ID", controlID.ToString());
            parms.Add("ROW_ID", "");
            int docID = svc.InsertRegistrationData(Convert.ToInt32(RegID), "DOCUMENT", parms);
            SendToCMS(docID, fileBytes, newFileName);

            if (docID > 0 && RegID > 0)
            {
                CreateRiskClosureWorkflow(RegID);
                presenter.Init(RegID);
                LoadCurrentAndPreviousApplicationData(RegID);
                LblRiskAlertStatus.Visible = true;
                LblRiskAlertStatus.Text = "Closure has been initiated.";
                DisableEnableLinksIfActiveRiskAlert(false);
                this.divInitiateCHOP.Visible = false;
            }

            return;
        }
        catch (Exception ex)
        {
            lblStatusMsg.Text = "No File uploaded. Error: " + Helper.HtmlEncode(ex.Message);
        }
    Failure:
        mpeClosureNotice.Show();
        return;
    }

    private void CreateRiskClosureWorkflow(int regID)
    {
        try
        {
            // create new Wf
            int entryTaskID = Convert.ToInt32(Helper.GetAppSettingFromDB("ODMClosureEntryTaskID", "0"));
            Workflow.Process pr = new Workflow.Process(CON.WorkflowType.RiskAlertClosure, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Guid.NewGuid(), entryTaskID);
            if (pr == null)
            {
                lblStatusMsg.Visible = true;
                lblStatusMsg.Text = "Closure Initiation is not successful";
                return;
            }

            int ProcessID = pr.ProcessID;
            int CurrentStepID = pr.CurrentStepID;
            int CurrentTaskID = pr.TaskID;
            int WorkflowID = pr.WorkflowID;

            // Save the Registration ID as a process parameter of the Workflow
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            svc.WF_SaveProcessParameter(ProcessID, "REGISTRATION_ID", regID.ToString());
            svc.WF_TakeAction(ProcessID, "Submit For Review", string.Empty);
            svc.InsertRegApplicationRecord(regID, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId), ProcessID);
        }
        catch (Exception ex)
        {

        }

    }

    protected void btnInitiateCMCWorkflow_Click(object sender, EventArgs e)
    {
        Helper.IsUpdateCPCContact = false;
        CreateCMCWorkflow(RegID);
    }

    private void CreateCMCWorkflow(int regID)
    {
        try
        {
            if (this.KeyFields.CurrentStepID == 0 || this.KeyFields.CurrentStepID == -1)
            {
                // create new Wf
                int entryTaskID = 850;

                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                //INSERT INTO VERSION Tables....
                DataSet ds = svc.InsertIntoVersionTables(this.KeyFields.RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                DataRow dr;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dr = ds.Tables[0].Rows[0];
                    if (!string.IsNullOrEmpty(Methods.GetStringValue(dr["ErrorMessage"])))
                    {
                        Logging log = new Logging(Guid.NewGuid(), string.Empty);
                        log.CreateLogEntry(string.Format("{0} {1}", "Setting up Workflow", Methods.GetStringValue(dr["ErrorMessage"])), Logging.LogPriority.Error);
                        return;
                    }
                }
                Workflow.Process pr = new Workflow.Process(CON.WorkflowType.CMC, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Guid.NewGuid(), entryTaskID);
                if (pr == null)
                {
                    lblStatusMsg.Visible = true;
                    lblStatusMsg.Text = "CMC Workflow Initiation was not successful";
                    return;
                }

                int ProcessID = pr.ProcessID;
                int CurrentStepID = pr.CurrentStepID;
                int CurrentTaskID = pr.TaskID;
                int WorkflowID = pr.WorkflowID;

                // Save the Registration ID as a process parameter of the Workflow

                svc.WF_SaveProcessParameter(ProcessID, "REGISTRATION_ID", regID.ToString());
                svc.InsertRegApplicationRecord(regID, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId), ProcessID);
                svc.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", RegID.ToString() }, { "REGISTRATION_STATUS_TYPE_ID", CON.RegistrationStatusTypeId.Pending.ToString() }, { "REG_PROGRAM_STATUS_TYPE_ID", CON.RegistrationProgramStatusTypeId.New.ToString() }, { "WORKFLOW_EVENT_TYPE_ID", "7" } });

                svc.WF_SaveProcessParameter(ProcessID, "WORKFLOW_EVENT_TYPE_ID", "7");

                ProviderFeedHelper.InsertProviderFeedNotes(RegID, 0, HttpContext.Current.User.Identity.Name, "Initiate CMC Enrollment", personReviewedBy: HttpContext.Current.User.Identity.Name, processID: ProcessID);
            }
           
            (this.Page as RegistrationProvider).RegistrationId = RegID;
            (this.Page as RegistrationProvider).IsReadOnly = false;
            (this.Page as RegistrationProvider).RegistrationStep = CON.SectionTypeID.CMCContactInformation;
            Response.Redirect("~/Process/Registration.aspx?RegId=" + RegID.ToString());
        }
        catch (Exception ex)
        {

        }

    }



    private void CreateRiskAlertCHOPWorkflow(int regID)
    {
        try
        {
            // create new Wf
            Workflow.Process pr = new Workflow.Process(CON.WorkflowType.RiskAlertCHOP, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Guid.NewGuid());
            if (pr == null)
            {
                lblStatusMsg.Visible = true;
                lblStatusMsg.Text = "RiskAlert Chop initiation is not successful";
                return;
            }

            int ProcessID = pr.ProcessID;
            int CurrentStepID = pr.CurrentStepID;
            int CurrentTaskID = pr.TaskID;
            int WorkflowID = pr.WorkflowID;

            // Save the Registration ID as a process parameter of the Workflow
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            svc.WF_SaveProcessParameter(ProcessID, "REGISTRATION_ID", regID.ToString());
            svc.WF_TakeAction(ProcessID, "Submit", string.Empty);
            svc.InsertRegApplicationRecord(regID, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId), ProcessID);
        }
        catch (Exception ex)
        {

        }

    }

    private void SendToCMS(int docID, byte[] fileBytes, string fileName)
    {
        OnBaseInterface onBaseInterface = new OnBaseInterface();
        onBaseInterface.SubmitFile(RegID, docID, fileBytes, fileName);
    }

    protected void btnClosureNoticeCancel_Click(object sender, EventArgs e)
    {
        lblStatusMsg.Text = "No Request Date or File uploaded.";
    }

    protected void btnDaysNoticeUpload_Click(object sender, EventArgs e)
    {
        _DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        lblDaysStatusMsg.Text = string.Empty;
        if (fileDaysNoticeUploadFile.PostedFile == null)
        {
            lblDaysStatusMsg.Text = "Unable to find posted file.";
            goto Failure;
        }
        if (string.IsNullOrEmpty(fileDaysNoticeUploadFile.PostedFile.FileName))
        {
            lblDaysStatusMsg.Text = "Select a file for upload";
            goto Failure;
        }
        if (string.IsNullOrEmpty(_DestinationPath))
        {
            lblDaysStatusMsg.Text = "ERROR - DestinationPath must have a value";
            goto Failure;
        }
        if (string.IsNullOrEmpty(_ValidFileExtensions))
        {
            lblDaysStatusMsg.Text = "ERROR - ValidFileExtensions must have a value";
            goto Failure;
        }
        string errMsg = string.Empty;
        if (!IsValidExtension(out errMsg, fileDaysNoticeUploadFile.PostedFile.FileName))
        {
            lblDaysStatusMsg.Text = Helper.HtmlEncode(errMsg);
            goto Failure;
        }
        if (!fileDaysNoticeUploadFile.HasFile)
        {
            lblDaysStatusMsg.Text = "No File has been uploaded.";
            goto Failure;
        }
        if (fileDaysNoticeUploadFile.PostedFile.ContentLength > (MaxFileMegaBytes * (1000 * 1024)))
        {
            lblDaysStatusMsg.Text = "File cannot be more than " + String.Format("{0:0,0}", (MaxFileMegaBytes * (1000 * 1024))) + " bytes (" +
                String.Format("{0:f}", MaxFileMegaBytes) + " MB) in size.";
            goto Failure;
        }

        if (!IsSpecialCharacter(fileDaysNoticeUploadFile.FileName))
        {
            lblDaysStatusMsg.Text = "The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): " + Helper.HtmlEncode(fileDaysNoticeUploadFile.FileName) + " Please remove the Special Character from the file Name before upload. ";
            goto Failure;
        }

        if (!string.IsNullOrEmpty(errMsg))
        {
            lblDaysStatusMsg.Text = Helper.HtmlEncode(errMsg);
            goto Failure;
        }

        try
        {
            // Use a different destination if debugging
            //#if DEBUG
            //_DestinationPath = "C:\\Projects\\PDMS\\PDMS\\FileStoreLocal\\";
            //#endif
            //_DestinationPath = "C:\\Projects\\Upload\\";

            if (System.Diagnostics.Debugger.IsAttached)
                _DestinationPath = @"C:\Temp\";

            string newFileName = Helper.CleanFilePath(fileDaysNoticeUploadFile.FileName);
            byte[] fileBytes = fileDaysNoticeUploadFile.EncryptedFileBytes;
            // Rename the file
            File.WriteAllBytes(Path.Combine(@_DestinationPath, newFileName), fileBytes);
            lblStatusMsg.Text = "File Uploaded: " + Helper.HtmlEncode(newFileName);
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", RegID.ToString());
            parms.Add("REG_PAGE_TYPE_ID", CON.RegistrationPageType.Identification.ToString());
            parms.Add("REG_PAGE_SECTION", "45DaysNotice");
            parms.Add("SCREENING_ACTIVITY_ID", null);
            parms.Add("NAME", newFileName);
            parms.Add("DESCRIPTION", " 45 Days Notice");
            parms.Add("FILE_NAME", newFileName);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("@ROW_ID", "");

            //Get Reg Section Upload controlId
            var rsucDS = svc.SelectRegSectionUploadControlByRegIDAndPageSection(RegID, "45DaysNotice");
            if (Helper.HasRows(rsucDS))
            {
                var RegSectionId = Convert.ToInt32(rsucDS.Tables[0].Rows[0]["REG_SECTION_UPLOAD_CONTROL_ID"]);
                parms.Add("REG_SECTION_UPLOAD_CONTROL_ID", RegSectionId.ToString());
            }

            int docID = svc.InsertRegistrationData(Convert.ToInt32(RegID), "DOCUMENT", parms);

            SendToCMS(docID, fileBytes, newFileName);

            if (docID > 0 && RegID > 0)
            {
                CreateRiskAlertCHOPWorkflow(RegID);
                presenter.Init(RegID);
                LoadCurrentAndPreviousApplicationData(RegID);
                LblRiskAlertStatus.Visible = true;
                LblRiskAlertStatus.Text = "CHOP has been initiated.";
                DisableEnableLinksIfActiveRiskAlert(false);
                this.divSubmit90DayClosure.Visible = false;
            }

            return;
        }
        catch (Exception ex)
        {
            lblDaysStatusMsg.Text = "No File uploaded. Error: " + Helper.HtmlEncode(ex.Message);
        }
    Failure:
        mpeDaysNotice.Show();
        return;
    }

    protected void linkSubmit90DayClosure_Click(object sender, EventArgs e)
    {
        mpeDaysNotice.Show();
    }

    protected void lnkDODDContinue_Click(object sender, EventArgs e)
    {
        int waiverServicesUpdateType = CON.WaiverServiceUpdateType.DODD;
        RedirectToWaiver(waiverServicesUpdateType);
    }

    protected void lnkODAContinue_Click(object sender, EventArgs e)
    {
        int waiverServicesUpdateType = CON.WaiverServiceUpdateType.ODA;
        RedirectToWaiver(waiverServicesUpdateType);
    }

    protected void lnkRequestReconsideration_Click(object sender, EventArgs e)
    {
        mpeRequestReconsideration.Show();
    }

    private bool HasDoDDservice(bool isODA = false)
    {
        bool rtn = false;
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.KeyFields.RegID.ToString());

        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_WAIVER_SERVICES", parms);
        if (Helper.HasRows(ds))
        {
            var dr = from row in ds.Tables[0].AsEnumerable()
                     where isODA ? !string.IsNullOrEmpty(row.Field<int>("WAIVER_SERVICES_ID").ToString()) : string.IsNullOrEmpty(row.Field<int>("WAIVER_SERVICES_ID").ToString())
                     select row;
            if (dr.Any())
            {
                rtn = true;
            }
        }
        return rtn;
    }

    private void DisableEnableLinksIfActiveRiskAlert(bool value)
    {
        lnkUpdateRegistration.Visible = value;
        lnkBeginReapplication.Visible = value;
        divDisEnrollment.Visible = value;
    }

    private void SendPartialTransaction(int waiverServiceTypeUpdate)
    {
        //call partial webservice
        //get passed / failure response
        //use that to call below code.
        string makeRequest = AppSettings.Get("MakeWSRequestCallToSI");
        int sId = 0;
        string mId = string.Empty;
        string txnResult = string.Empty;
        string partialTTService = string.Empty;
        string partialSubService = string.Empty;
        int transactionType = 0;

        if (makeRequest.ToLower().Equals("true"))
        {
            if (waiverServiceTypeUpdate == CON.WaiverServiceUpdateType.ODA)
            {
                transactionType = (int)TransactionController.TransactionTypeNew.SendPCWFull;
                partialTTService = CON.TransactionTypeValues.PCW_Full;
                partialSubService = CON.PartialProviderSubscriberSystems.PCW;
            }
            else
            {
                transactionType = (int)TransactionController.TransactionTypeNew.SendPSMFull;
                partialTTService = CON.TransactionTypeValues.PSM_FULL;
                partialSubService = CON.PartialProviderSubscriberSystems.PSM;
            }

            DataSet ds1 = RegistrationController.SelectMedicaidId(this.KeyFields.RegID);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                mId = ds1.Tables[0].Rows[0]["MEDICAID_ID"].ToString();
                sId = Convert.ToInt32(ds1.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"]);
            }
            if (transactionType > 0)
            {
                int tqId = TransactionController.InsertTransactionQueue(
                     transactionType, this.KeyFields.RegID, sId, DateTime.Now, null, null, DateTime.Now, CON.appWorkflowUserId);

                PartialProviderRequestResponse pprr = new PartialProviderRequestResponse();
                RegistrationController.PopulateStagingData(tqId, partialTTService, string.Empty);


                txnResult = pprr.partialProviderManagementSubmitRequest(tqId, partialSubService, true);

                if (txnResult.Equals(CON.TransactionResult.TransactionPassed))
                {
                    TransactionController.UpdateTransactionQueue(tqId, DateTime.Now, DateTime.Now, DateTime.Now, CON.appWorkflowUserId);
                    RedirectToWaiver(waiverServiceTypeUpdate);
                }
                else
                {
                    TransactionController.UpdateTransactionQueue(tqId, DateTime.Now, DateTime.Now, DateTime.Now, CON.appWorkflowUserId);
                    Response.Redirect("~/Process/ProviderDetailsNew.aspx?regID=" + this.KeyFields.RegID);
                }

            }
        }
        else
        {
            RedirectToWaiver(waiverServiceTypeUpdate);
        }
    }
    private void RedirectToWaiver(int waiverServiceTypeUpdate)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationByRegID(this.KeyFields.RegID);
        string ddContractNum = string.Empty;
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddContractNum = ds.Tables[0].Rows[0]["dd_contract_number"].ToString();
        }

        string Reg64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.KeyFields.RegID.ToString()));
        string Cnt64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ddContractNum));
        string Src64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("pnm"));
        string Tgt64 = "";
        string url = "";

        if (waiverServiceTypeUpdate == CON.WaiverServiceUpdateType.ODA)
        {
            url = AppSettings.Get("ODA-Application-RedirectURL");  // https://identifier.devapps.dodd.ohio.gov/?rgd=###_RGD_B64_###&ctn=###_CTN_B64_###&src=###_SRC_B64_###&tgt=###_TGT_B64_###
            Tgt64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("oda"));
        }
        else
        {
            url = AppSettings.Get("DODD-Application-RedirectURL");  // https://identifier.qaapps.dodd.ohio.gov/?rgd=###_RGD_B64_###&ctn=###_CTN_B64_###&src=###_SRC_B64_###&tgt=###_TGT_B64_###
            Tgt64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("psm"));
        }

        url = url.Replace("###_RGD_B64_###", Reg64);
        url = url.Replace("###_CTN_B64_###", Cnt64);
        url = url.Replace("###_SRC_B64_###", Src64);
        url = url.Replace("###_TGT_B64_###", Tgt64);
        Response.Redirect(url);
    }

    private void UpdateWaiverProviderWithWorkflow(int WaiverTypeUpdate)
    {
        try
        {
            int workflowID = 0;
            bool reValFlag = (this.KeyFields.ApplicationTypeID == CON.ApplicationType.ChangeOfOperator) ? true : false;
            Helper.IsUpdateCPCContact = false;
            workflowID = svc.GetWorkflowInstance(KeyFields.ApplicationTypeID, KeyFields.ProviderCategoryTypeID,
                        KeyFields.ProviderTypeID, KeyFields.ReferralTypeID, reValFlag, false, false, false);

            BeginUpdateRegistration(workflowID, false, false, false, false, WaiverTypeUpdate);

            //Update Waiver type update for registration.

            svc.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", this.KeyFields.RegID.ToString() }, { "WAIVER_SERVICE_UPDATE_TYPE_ID", WaiverTypeUpdate.ToString() } });

            SendPartialTransaction(WaiverTypeUpdate);
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkBeginDODDUpdate_Click(object sender, EventArgs e)
    {
        int waiverUpdateType = CON.WaiverServiceUpdateType.DODD;
        UpdateWaiverProviderWithWorkflow(waiverUpdateType);
    }

    protected void lnkAddODAServices_Click(object sender, EventArgs e)
    {
        int waiverUpdateType = CON.WaiverServiceUpdateType.ODA;
        UpdateWaiverProviderWithWorkflow(waiverUpdateType);
    }
    private bool validateNPI(int regID)
    {
        bool rtn = true;
        DataSet ds = svc.SelectRegistrationByRegID(regID);
        ProviderManagementData details = new ProviderManagementData();
        details.LoadObjectFromDataset(ds);

        if (string.IsNullOrEmpty(details.NPI))
        {
            rtn = false;
            return rtn;
        }
        return rtn;
    }

    protected void btnErrorOk_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Process/ProviderDetailsNew.aspx?regID=" + this.KeyFields.RegID);
    }

    protected void lnkNavigateMITS_Command(object sender, CommandEventArgs e)
    {
        //If AppsettingsValue is true then go to PNM
        string val =  AppSettings.Get("RedirectToPNM3BClaims");
            if (string.Equals(val, "1") || string.Equals(val, "2")) //Go to PNM
            {
                RedirectToPage(CON.SectionTypeID.SubmitClaim);
            }
            else
            {
                NavigatetoMITS();
            }     
        //NavigatetoMITS();
    }

    protected void lnkNavigateMITS_PriorAuth(object sender, CommandEventArgs e)
    {
        //If AppsettingsValue is true then go to PNM
        if (string.Equals(AppSettings.Get("RedirectToPNM3BPA"), "1")) //Go to PNM
        {
            RedirectToPage(CON.SectionTypeID.SearchPriorAuthorization);
        }
        else
        {
            NavigatetoMITS();
        }
        //NavigatetoMITS();
    }

    protected void lnkNavigateMITS_Claims(object sender, CommandEventArgs e)
    {
        //If AppsettingsValue is true then go to PNM
        if (string.Equals(AppSettings.Get("RedirectToPNM3BClaims"), "1")) //Go to PNM
        {
            RedirectToPage(CON.SectionTypeID.SearchClaim);
        }
        else if (string.Equals(AppSettings.Get("RedirectToPNM3BClaims"), "2")) //Go to PNM
        {
            RedirectToPage(CON.SectionTypeID.SearchClaimV2);
        }
        else
        {
            NavigatetoMITS();
        }
        //NavigatetoMITS();
    }


    private void NavigatetoMITS()
    {
        if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSAdmin) ||
                    Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSBusinessRelations) ||
                    Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSProviderAssistance) ||
                    Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSProviderEnrollment) ||
                    Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSUATSuperuser) ||
                    Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) ||
                    Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent))
        {
            Response.Redirect("~/Process/RecipientEligibilityMITS.aspx?MedicaidId=" + this.KeyFields.MedicaidID);
        }
    }

    protected void lnkBtn_Hospice(object sender, CommandEventArgs e)
    {
        //If AppsettingsValue is true then go to PNM
        if (string.Equals(AppSettings.Get("RedirectToPNM3BHospice"), "1")) //Go to PNM
            RedirectToPage(CON.SectionTypeID.HospiceEnrollment);
        else
        {
            NavigatetoMITS();
        }
       
    }
    protected void BtnAdd_Financial(object sender, CommandEventArgs e)
    {
        //If AppsettingsValue is true then go to PNM
        if (string.Equals(AppSettings.Get("RedirectToPNM3BProviderFinancial"), "1")) //Go to PNM
        {
            Session["StepId"] = Convert.ToInt32(CON.SectionTypeID.ProviderFinancial);
            Session["RegId"] = this.RegID;
            Response.Redirect(string.Format("~/Process/ProviderFinancials.aspx"));
        }
        else
        {
            NavigatetoMITS();
        }
        
    }
    protected void lnkNavigateMAS_Command(object sender, CommandEventArgs e)
    {
        if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) ||
            Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent))
        {
            NavigateToMAS(this.KeyFields.MedicaidID);
        }
    }

    protected void lnkNavigate_MemberEligibility(object sender, CommandEventArgs e)
    {
        if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSAdmin) ||
            Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSBusinessRelations) ||
            Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSProviderAssistance) ||
            Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSProviderEnrollment) ||
            Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSUATSuperuser) ||
            Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) ||
            Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent))
        {
            var flag = AppSettings.Get("RedirectToPNM3BMemberEligbility");
            if (flag == "1") //Go to PNM
            {
                Session["StepId"] = Convert.ToInt32(CON.SectionTypeID.SearchEligibility);
                Session["RegId"] = this.RegID;
                Response.Redirect("~/Process/RecipientEligibility.aspx");
            }else if (flag == "2")
            {
                Session["StepId"] = Convert.ToInt32(CON.SectionTypeID.SearchEligibilityV2);
                Session["RegId"] = this.RegID;
                Response.Redirect("~/Process/SearchEligibility.aspx");
            }
            else
                Response.Redirect("~/Process/RecipientEligibilityMITS.aspx?MedicaidId=" + this.KeyFields.MedicaidID);
        }
        
    }

    protected void lnkNavigate_PaymentInnovations(object sender, CommandEventArgs e)
    {
        Response.Redirect("~/Process/PaymentInnovationReports.aspx?MedicaidId=" + this.KeyFields.MedicaidID);
    }

    private void NavigateToMAS(string medicaidId)
    {
        string ReturnURL = "";
        string url = "";
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

        // check for Haven Redirect
        HttpCookie MASRedirectURL = Request.Cookies["MASRedirectURL"];
        if (MASRedirectURL != null)
        {
            try
            {
                if (MASRedirectURL.Value.Length > 4)
                {  // this is a redirect on an expired or missing token from an embedded PDFlink
                    ReturnURL = Server.UrlEncode(MASRedirectURL.Value);
                }
                // get rid of cookie so we don't go back
                MASRedirectURL.Value = null;
                MASRedirectURL.Expires = DateTime.Now.AddDays(-10);
                Response.SetCookie(MASRedirectURL);
                Response.Cookies.Remove("MASRedirectURL");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // haven handoff data
        var MASUrl = AppSettings.Get("MayerStauferRedirectURL", "https://ohiofi.mslc.com/SSO/Login");
        var jwtoken = MaximusJWT.GetNewToken(HttpContext.Current.User.Identity.Name, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), System.Configuration.ConfigurationManager.AppSettings["PNMSecretKey"]);

        // save token to db
        svc.SaveUserPNMToken(Helper.GetUserId(HttpContext.Current.User.Identity.Name), jwtoken);

        url = MASUrl + "?AuthToken=" + jwtoken + "&ProviderId=" + medicaidId + "&ReturnURL=" + ReturnURL;
        Response.Redirect(url);
    }

    private void TurnOffDODDUpdateCancelLinksIfNotDoddUpdate(ProviderManagementData details)
    {
        // OHPNM-8547 - The DODD Workflow update might have gotten auto triggered into provider screening due to a match in a government maintained exclusion DB (SAM, OIG/LEI, Medicare, Medicaid, SSDMF, NPPES)
        // If that's the case, turn off the DODD continue/cancel options since they no longer apply.  We need to check what the WaiverServiceUpdateTypeID is from the workflow process parameters and not the registration, since it might have changed w/o the registration changing.
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.WF_SelectProcessParameters(details.ProcessID);
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            
            string parameterValue = (dr[CON.ProcessParameter.WaiverServiceUpdateTypeId].ToString());
           
            if (parameterValue == null || parameterValue != "1")
            {
                // turn off continue and cancel links since WaiverServiceUpdateTypeId is either null or isn't 1
                divDODDUpdate.Visible = false;
            }
        }
    }

    private void IsCPCUpdateContact(int ProcessID)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.WF_SelectProcessParameters(ProcessID);
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            Helper.IsUpdateCPCContact = string.IsNullOrEmpty(dr[CON.ProcessParameter.UpdateCpcContact].ToString()) ? false : Convert.ToBoolean(dr[CON.ProcessParameter.UpdateCpcContact]); 
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        int sendToTypeID = CON.SendToTypeID.Provider;
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("SendToTypeID", DbType.Int32, sendToTypeID, false));
        parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, this.KeyFields.RegID, false));
        DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectEmailRecipients", parameters, "EmailRecipients");
        string email = "";
        if(ds != null)
        {
            if(ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    email = ds.Tables[0].Rows[0]["EmailAddress"].ToString();
                }
            }
        }
        var fileBytes = UploadAttachments.FileBytes;
        var month = DateTime.Now.ToString("yyyyMM");
        var currentDate = DateTime.Now.ToString("yyyyMMdd");
        var type = txtTypeID.Text.ToString();
        var fileExtension = Path.GetExtension(UploadAttachments.FileName).ToLower();
        var fileName = type + "_" + this.KeyFields.MedicaidID + "_" + month + "_" + currentDate + fileExtension;
        SendAttachment attachment = new SendAttachment
        {
            DocumentType = txtTypeID.Text,
            DocumentName = fileName,
            DocumentExtension = Path.GetExtension(UploadAttachments.FileName).Replace(@".", ""),
            EmailID = email,
            AttachmentData64Binary = fileBytes,
            BenPeriod = Convert.ToInt32(1)
            //Identifiers = new SendAttachmentData // I will assign this property in sumbmit button click event when final response comes
        };
        List<SendAttachmentData> identifiers = new List<SendAttachmentData>();
        identifiers.Add(new SendAttachmentData
        {
            DocXrefType = "REGID",
            IndexId = this.RegID.ToString()
        });
        identifiers.Add(new SendAttachmentData
        {
            DocXrefType = "NPI",
            IndexId = this.KeyFields.NPI.ToString()
        });
        identifiers.Add(new SendAttachmentData
        {
            DocXrefType = "PID",
            IndexId = this.KeyFields.MedicaidID.ToString()
        });
        attachment.Identifiers = identifiers.ToArray();
        
        List<SendAttachment> att = new List<SendAttachment>();
        att.Add(attachment);
        SendAttachments(att);
    }
    private void SendAttachments(List<SendAttachment> attachments)
    {
        DataSet dshospice = new DataSet();
        try
        {
            List<InqMessageHeaderSubscriber> inqMessageHeaderSubscriber = new List<InqMessageHeaderSubscriber>();
            inqMessageHeaderSubscriber.Add(InqMessageHeaderSubscriber.APM);
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            MessageHeader msgHeader = new MessageHeader();
            msgHeader.BusinessFlow = InqMessageHeaderBusinessFlow.sendAttachment;
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = InqMessageHeaderRequestorSystem.PNM;
            msgHeader.SubscriberSystem = inqMessageHeaderSubscriber.ToArray();
            msgHeader.ModuleTransactionId = string.Empty;
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
            msgHeader.SITransactionKey = sitTransactionKey;

            sendAttachmentRequest sendAttachment = new sendAttachmentRequest
            {
                MessageHeader = msgHeader,
                Payload = new SendAttachmentPayload
                {
                    AttachmentInfo = new SendAttachmentInformation
                    {
                        AttachmentData = attachments.ToArray(),
                        SourceId = "PNM"
                    }
                }
            };
            var response = DocumentServiceAgent.SendGenericAttachmentRequest(sendAttachment);
            var xDoc = XDocument.Parse(response);
            string responseNS = "http://mes.gov/attachment";
            XNamespace ns = XNamespace.Get(responseNS);
            var xServiceResult = xDoc.Root.Descendants(ns + "SendAttachmentResponse").FirstOrDefault();
            var xServiceResult1 = xServiceResult.Descendants(ns + "Response").FirstOrDefault();
            string ResponseType = "";
            ResponseType = (string)xServiceResult1.Element(ns + "ResponseType");
            if (ResponseType == "Success")
                ucMessageBox.Show("File sent to APM.", "File Upload");
            else
                ucMessageBox.Show("Upload Failure", "File Upload");
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            settings.XmlResolver = null;
            XmlReader xmlReader = XmlReader.Create(new StringReader(response.ToString()), settings);
            dshospice.ReadXml(xmlReader);
            //return ConvertDatasetToSendAttachmentResponse(dshospice);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkBtn_CMCDashboard(object sender, CommandEventArgs e)
    {
        string cmcdashboardRedirectURL = string.Empty;
        RedirectToCMCDashboardHelper cmc = new RedirectToCMCDashboardHelper();
        bool flag = cmc.MakeCallToCMCRedirectAPI(this.KeyFields.MedicaidID.ToString(), ref cmcdashboardRedirectURL);
        if (flag)
        {
            divCMCRedirectErrMsg.Visible = false;
            Response.Redirect(cmcdashboardRedirectURL);
        }
        else
        {
            divCMCRedirectErrMsg.Visible = true;
        }
    }
}
