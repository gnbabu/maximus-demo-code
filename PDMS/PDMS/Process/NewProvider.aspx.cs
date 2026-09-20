using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Web.Security;
public partial class Process_NewProvider : RegistrationProvider, IProviderAddView
{
    public static class EntityType
    {
        public const string Individual = "Individual";
        public const string Organization = "Organization";
    }
    #region Properties
    private ProviderAddPresenter _presenter;
    public ProviderAddPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new ProviderAddPresenter(this);
            }
            return _presenter;
        }
    }
    public ProviderManagementData Model { get; set; }
    [SerializableAttribute()]
    public class InitialRegKeyFields
    {
        public int ProviderTypeID { get; set; }
        public string NPI { get; set; }
        public int TaxIDTypeID { get; set; }
        public int SpecialtyTypeID { get; set; }
        public int TaxonomyTypeID { get; set; }
        public string ZipCode { get; set; }
        public string ZipExt { get; set; }
        public int WorkflowID { get; set; }
        public DateTime RequestedEffectiveDate { get; set; }
        public int ReferralID { get; set; }
        public int PaperRequestID { get; set; }
        public int ApplicationTypeID { get; set; }
        public int WaiverTypeID { get; set; }
        public InitialRegKeyFields(int providerTypeID, string npi, int taxIDTypeID, int specialtyTypeID, int taxonomyTypeID,
            string zipCode, string zipExt, int workflowID, DateTime requestedEffectiveDate, int referralID, int paperRequestID,
            int applicationTypeID, string applicationTypeName, int waiverTypeId)
            : base()
        {
            ProviderTypeID = providerTypeID;
            NPI = npi;
            TaxIDTypeID = taxIDTypeID;
            SpecialtyTypeID = specialtyTypeID;
            TaxonomyTypeID = taxonomyTypeID;
            ZipCode = zipCode;
            ZipExt = zipExt;
            WorkflowID = workflowID;
            RequestedEffectiveDate = requestedEffectiveDate;
            ReferralID = referralID;
            PaperRequestID = paperRequestID;
            ApplicationTypeID = ApplicationTypeID;
            WaiverTypeID = waiverTypeId;
        }
    }
    private InitialRegKeyFields InitialKeyFields
    {
        get
        {
            return ViewState["InitialKeyFields"] == null ? null : ViewState["InitialKeyFields"] as InitialRegKeyFields;
        }
        set
        {
            ViewState["InitialKeyFields"] = value;
        }
    }
    public int WorkflowIDRequested
    {
        get
        {
            return ViewState["WorkflowIDRequested"] == null ? CON.WorkflowType.RegistrationNew : Convert.ToInt32(ViewState["WorkflowIDRequested"]);
        }
        set
        {
            ViewState["WorkflowIDRequested"] = value;
        }
    }
    public bool AdminKeyFieldEditRequest
    {
        get
        {
            if (ViewState["AdminEditKeyFieldDataRequest"] != null)
            {
                //return true;
                return (Boolean)ViewState["AdminEditKeyFieldDataRequest"];
            }
            else
            {
                ViewState["AdminEditKeyFieldDataRequest"] = (Request["AdminEditKeyFieldDataRequest"] != null) ? true : false;
                return (Boolean)ViewState["AdminEditKeyFieldDataRequest"];
            }
        }
        set
        {
            ViewState["AdminEditKeyFieldDataRequest"] = value;
        }
    }
    public bool KeyFieldEditRequest
    {
        get
        {
            if (ViewState["EditKeyFieldDataRequest"] != null)
            {
                //return true;
                return (Boolean)ViewState["EditKeyFieldDataRequest"];
            }
            else
            {
                ViewState["EditKeyFieldDataRequest"] = (Request["EditKeyFieldDataRequest"] != null) ? true : false;
                return (Boolean)ViewState["EditKeyFieldDataRequest"];
            }
        }
        set
        {
            ViewState["EditKeyFieldDataRequest"] = value;
        }
    }
    public bool IsCredentialingProcess
    {
        get
        {
            if (ViewState["IsCredentialingProcess"] != null)
            {
                //return true;
                return (Boolean)ViewState["IsCredentialingProcess"];
            }
            else
            {
                ViewState["IsCredentialingProcess"] = (Request["IsCredentialingProcess"] != null) ? true : false;
                return (Boolean)ViewState["IsCredentialingProcess"];
            }
        }
        set
        {
            ViewState["IsCredentialingProcess"] = value;
        }
    }
    public bool ProviderTypeChangeRequest
    {
        get
        {
            if (ViewState["ProviderTypeChangeRequest"] != null)
            {
                //return true;
                return (Boolean)ViewState["ProviderTypeChangeRequest"];
            }
            else
            {
                ViewState["ProviderTypeChangeRequest"] = (Request["ProviderTypeChangeRequest"] != null) ? true : false;
                return (Boolean)ViewState["ProviderTypeChangeRequest"];
            }
        }
        set
        {
            ViewState["ProviderTypeChangeRequest"] = value;
        }
    }
    public int ProviderTypeID { get; set; }
    public int ReferralID
    {
        get
        {
            return ViewState["ReferralID"] == null ? 0 : Convert.ToInt32(ViewState["ReferralID"]);
        }
        set
        {
            ViewState["ReferralID"] = value;
        }
    }
    public bool SaveAndSubmitFromWaiverType
    {
        get
        {
            return ViewState["SaveAndSubmitFromWaiverType"] == null ? false : Convert.ToBoolean(ViewState["SaveAndSubmitFromWaiverType"]);
        }
        set
        {
            ViewState["SaveAndSubmitFromWaiverType"] = value;
        }
    }
    public int ReferralTypeID
    {
        get
        {
            return ViewState["ReferralTypeID"] == null ? 0 : Convert.ToInt32(ViewState["ReferralTypeID"]);
        }
        set
        {
            ViewState["ReferralTypeID"] = value;
        }
    }
    public int RegID
    {
        get
        {
            if (ViewState["RegID"] != null)
            {
                return Convert.ToInt32(ViewState["RegID"]); ;
            }
            else
            {
                ViewState["RegID"] = (Request["RegID"] != null) ? Convert.ToInt32(Request["RegID"]) : 0;
                return Convert.ToInt32(ViewState["RegID"]);
            }
        }
        set
        {
            ViewState["RegID"] = value;
        }
    }
    public int PaperRequestQueueID
    {
        get
        {
            return ViewState["PaperRequestQueueID"] == null ? 0 : Convert.ToInt32(ViewState["PaperRequestQueueID"]);
        }
        set
        {
            ViewState["PaperRequestQueueID"] = value;
        }
    }
    public int SpecialtyTypeID
    {
        get
        {
            return ViewState["SpecialtyTypeID"] == null ? 0 : Convert.ToInt32(ViewState["SpecialtyTypeID"]);
        }
        set
        {
            ViewState["SpecialtyTypeID"] = value;
        }
    }
    public int TaxonomyTypeID
    {
        get
        {
            return ViewState["TaxonomyTypeID"] == null ? 0 : Convert.ToInt32(ViewState["TaxonomyTypeID"]);
        }
        set
        {
            ViewState["TaxonomyTypeID"] = value;
        }
    }
    public bool EditingNPI
    {
        get
        {
            return ViewState["EditingNPI"] == null ? false : Convert.ToBoolean(ViewState["EditingNPI"]);
        }
        set
        {
            ViewState["EditingNPI"] = value;
        }
    }
    public DateTime? OriginalNPIEndDate
    {
        get
        {
            return ViewState["NPIEndDate"] as DateTime?;
        }
        set
        {
            ViewState["NPIEndDate"] = value;
        }
    }
    public int ApplicationTypeID
    {
        get
        {
            return ViewState["ApplicationTypeID"] == null ? 0 : Convert.ToInt32(ViewState["ApplicationTypeID"]);
        }
        set
        {
            ViewState["ApplicationTypeID"] = value;
        }
    }
    public int WaiverTypeID
    {
        get
        {
            return ViewState["WaiverTypeID"] == null ? 0 : Convert.ToInt32(ViewState["WaiverTypeID"]);
        }
        set
        {
            ViewState["WaiverTypeID"] = value;
        }
    }
    public DateTime? EndDate
    {
        get
        {
            return ViewState["EndDate"] as DateTime?;
        }
        set
        {
            ViewState["EndDate"] = value;
        }
    }
    public int TaxIDTypeID
    {
        get
        {
            if (ViewState["TaxIDTypeID"] != null)
                return Convert.ToInt32(ViewState["TaxIDTypeID"]);
            else
            {
                DataSet dsUAI;
                dsUAI = svc.GetUserAccountInformation(UserID.ToString());
                if (Helper.HasRows(dsUAI))
                    ViewState["TaxIDTypeID"] = Helper.ConvertStrNullToInt32(dsUAI.Tables[0].Rows[0]["TAX_ID_TYPE_ID"]);
                else
                    ViewState["TaxIDTypeID"] = -1;
                return Convert.ToInt32(ViewState["TaxIDTypeID"]);
            }
        }
        set
        {
            ViewState["TaxIDTypeID"] = value;
        }
    }
    public string TaxID
    {
        get
        {
            if (ViewState["TaxID"] != null)
                return Convert.ToString(ViewState["TaxID"]);
            else
            {
                DataSet dsUAI;
                dsUAI = svc.GetUserAccountInformation(UserID.ToString());
                if (Helper.HasRows(dsUAI))
                    ViewState["TaxID"] = dsUAI.Tables[0].Rows[0]["TAX_ID"].ToString();
                else
                    ViewState["TaxID"] = "";
                return Convert.ToString(ViewState["TaxID"]);
            }
        }
        set
        {
            ViewState["TaxID"] = value;
        }
    }
    public Boolean ConvertedProvider
    {
        get
        {
            return ViewState["ConvertedProvider"] == null ? false : Convert.ToBoolean(ViewState["ConvertedProvider"]);
        }
        set
        {
            ViewState["ConvertedProvider"] = value;
        }
    }
    public string MMISProviderTypeID { get; set; }
    public DataTable dtApplicationTypes
    {
        get
        {
            return (DataTable)ViewState["ApplicationTypes"];
        }
        set
        {
            ViewState["ApplicationTypes"] = value;
        }
    }
    public DataTable dtWaiverTypes
    {
        get
        {
            return (DataTable)ViewState["WaiverTypes"];
        }
        set
        {
            ViewState["WaiverTypes"] = value;
        }
    }
    public DataTable dtCategories
    {
        get
        {
            return (DataTable)ViewState["Categories"];
        }
        set
        {
            ViewState["Categories"] = value;
        }
    }
    private Guid UserID
    {
        get
        {
            if (ViewState["UserID"] != null)
            {
                return (Guid)ViewState["UserID"];
            }
            else
            {
                ViewState["UserID"] = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
                return (Guid)ViewState["UserID"];
            }
        }
        set
        {
            ViewState["UserID"] = value;
        }
    }
    public Boolean IsLinkProvider
    {
        get
        {
            if (ViewState["IsLinkProvider"] != null)
            {
                return (Boolean)ViewState["IsLinkProvider"];
            }
            else
            {
                ViewState["IsLinkProvider"] = (Request["LinkProvider"] != null) ? true : false;
                return (Boolean)ViewState["IsLinkProvider"];
            }
        }
        set
        {
            ViewState["IsLinkProvider"] = value;
        }
    }
    public bool CheckNPIRequired
    {
        get
        {
            return ViewState["CheckNPIRequired"] == null ? false : Convert.ToBoolean(ViewState["CheckNPIRequired"]);
        }
        set
        {
            ViewState["CheckNPIRequired"] = value;
        }
    }
    public bool IsAddODMorODAMedicaid
    {
        get
        {
            if (ViewState["AddODMorODAMedicaid"] != null)
            {
                //return true;
                return (Boolean)ViewState["AddODMorODAMedicaid"];
            }
            else
            {
                ViewState["AddODMorODAMedicaid"] = (Request["AddODMorODAMedicaid"] != null) ? true : false;
                return (Boolean)ViewState["AddODMorODAMedicaid"];
            }
        }
        set
        {
            ViewState["AddODMorODAMedicaid"] = value;
        }
    }
	
    public int WorkflowEventTypeId
    {
        get
        {
            if (ViewState["WorkflowEventTypeId"] == null) ViewState["WorkflowEventTypeId"] = 0;
            return (int)ViewState["WorkflowEventTypeId"];
        }
        set { ViewState["WorkflowEventTypeId"] = value; }
    }
	
    #endregion
    #region Parent Page Events
    //public delegate void InsertEventHandler(ProviderManagementData data);
    //public event InsertEventHandler InsertEvent;
    //public delegate void CancelEventHandler();
    //public event CancelEventHandler CancelEvent;
    public delegate void ValidationEventHandler();
    //public event ValidationEventHandler ValidationEvent;
    public delegate void KeepPopupOpenEventHandler();
    public event KeepPopupOpenEventHandler KeepPopupOpenEvent;
    public delegate void KeyFieldUpdateEventHandler();
    //public event KeyFieldUpdateEventHandler KeyFieldUpdateEvent;
    #endregion
    #region svc
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
    #endregion
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
            Page.Theme = "Modernization";
        else
            Page.Theme = "Default";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            this.Page.Title = (string)GetGlobalResourceObject("BrandingResource", "PortalName");
            ClearErrorMessages();
            ProviderManagerData data = LoadProviderFields();
            InitView(data);
            divNewProvider.Visible = true;
            divRegChoice.Visible = false;
            
            if (Session["ApplicationTypeID"] != null)
            {
                txtApplicationTypeID.Text = Convert.ToString(Session["ApplicationTypeID"]);
                Session["ApplicationTypeID"] = null;
                txtApplicationTypeID_TextChanged(this.txtApplicationTypeID, null);
            }

        }
        if (ApplicationTypeID == CON.ApplicationType.Internal)
        {
            lblNPI.InnerText = "NPI (if applicable)";
        }
        else
        {
            lblNPI.InnerText = "NPI*";
        }
        rblTaxIDType_SelectedIndexChanged(null, null);
    }
    protected void Page_PreRender(object sender, EventArgs e)//OHPNM-3551
    {
        SetupButtonDisablesOfMultiClick();
    }
    private void SetupButtonDisablesOfMultiClick()
    {
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
    }
    private ProviderManagerData LoadProviderFields()
    {
        ProviderManagerData keyData = new ProviderManagerData();
        UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        keyData.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        keyData.TaxID = TaxID;
        keyData.TaxIDTypeID = TaxIDTypeID;
        keyData.ApplicationTypeID = -1;
        keyData.ApplicationTypeName = "";
        return keyData;
    }
    private void LoadLinkProviderDetails()
    {
        // TODO: EDV how to get linked provider info
        ProviderManagementData data = new ProviderManagementData();
        DataSet ds = svc.SelectRegistrationByRegID(this.RegID);
        data.LoadObjectFromDataset(ds);
        SetExistingProviderData(data);
    }
    protected void Validate_AppNumber(object source, ServerValidateEventArgs args)
    {
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (this.IsAddODMorODAMedicaid || (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAdministrator) || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent)))
        {
            Response.Redirect("~/Process/ProviderHomeNew.aspx");
        }
        else
        {
             Response.Redirect("~/Process/GroupReview.aspx");
            divNewProvider.Visible = true;
            divRegChoice.Visible = false;
        }        
        //if (CancelEvent != null)
        //{
        //    CancelEvent();
        //}
    }
    protected void btnKFECancel_Click(object sender, EventArgs e)
    {
        this.mpeKFEVerify.Hide();
        if (KeepPopupOpenEvent != null)
        {
            KeepPopupOpenEvent();
        }
    }
    protected void btnCredCancel_Click(object sender, EventArgs e)
    {
        this.mpeKFEVerify.Hide();
        if (KeepPopupOpenEvent != null)
        {
            KeepPopupOpenEvent();
        }
    }
    protected void Validate_NPIEdit(object sender, ServerValidateEventArgs args)
    {
        if (EditingNPI)
        {
            if (txtNPI.Text == txtOldNPI.Text)
                args.IsValid = false;
            else
                args.IsValid = true;
        }
        else
        {
            args.IsValid = true;
        }
    }
    protected void Validate_TaxIDType(object sender, ServerValidateEventArgs args)
    {
        if (txtCategoryID.Text == Enumerations.ProviderCategoryType.IndividualSolo.ToString("D") && Enumerations.TaxIdTypeId.SSN.ToString("D") != args.Value)
        {
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;
        }
    }
    public void SetApplicationTypes(DataView applicationTypes, DataView waiverTypes)
    {
        //DataSet ds = svc.GetApplicationTypes();
        applicationTypes.RowFilter = "ISNULL(IsVisible,0) = 1";
        dtApplicationTypes = applicationTypes.ToTable();
        dtWaiverTypes = waiverTypes.ToTable();
        applicationTypes.RowFilter = "ISNULL(IsVisible,0) = 1 AND ISNULL(IsWaiver,0) = 0";
        DataTable dt = applicationTypes.ToTable();
        rptApplication.DataSource = dt.Select().Take(4).CopyToDataTable();
        rptApplication.DataBind();
        //dt = applicationTypes.ToTable();
        //applicationTypes.RowFilter = "ISNULL(IsVisible,0) = 1 AND ISNULL(IsWaiver,0) = 0";
        //rptApplicationMore.DataSource = dt.Select().Skip(4).Take(4).CopyToDataTable();
        //rptApplicationMore.DataBind();
        waiverTypes.RowFilter = "ISNULL(IsVisible,0) = 1 AND ISNULL(IsWaiver,0) = 1";
        dt = waiverTypes.ToTable();
        rptApplicationWaiver.DataSource = dt;
        rptApplicationWaiver.DataBind();
    }
    protected void txtApplicationTypeID_TextChanged(object sender, EventArgs e)
    {
        //ApplicationTypeID = Convert.ToInt32(Session["ApplicationTypeID"]);
        ApplicationTypeID = Convert.ToInt32(txtApplicationTypeID.Text);
        string desc = "";
        DataRow[] selectedRows = dtApplicationTypes.Select("APPLICATION_TYPE_ID = " + ApplicationTypeID.ToString());
        if (selectedRows.Count() > 0)
        {
            desc = selectedRows[0]["APPLICATION_TYPE_NAME"].ToString();
            hdnIsWaiver.Value = selectedRows[0]["IsWaiver"].ToString();
            //TODO need to change it to be better functionality currently doing for DEMO AT
            //if (hdnIsWaiver.Value != "True")
            //{
            //    if (Convert.ToInt32(selectedRows[0]["Application_Type_ID"].ToString()) > 7)
            //    hdnIsWaiver.Value = "More";
            //}
        }
        txtApplicationType.Text = desc;
        if (ApplicationTypeID == CON.ApplicationType.Waiver)
        {
            trWaiverType.Visible = true;
            txtWaiverTypeID.Text = hdntxtWaiverType.Value;
            txtWaiverTypeID_TextChanged(sender, e);
        }
        else
        {
            trWaiverType.Visible = false;
        }
        if (ApplicationTypeID != CON.ApplicationType.Waiver)
            this.SetApplicationDependentFields();

        if (this.IsAddODMorODAMedicaid)
        {            
           ProviderManagementData data = new ProviderManagementData();
            DataSet ds = svc.SelectRegistrationByRegID(this.RegID);
            data.LoadObjectFromDataset(ds);
            SetExistingProviderData(data);
            SetDisplayForAddODMorODAMedicaid();
        }
    }
    protected void txtCategoryID_TextChanged(object sender, EventArgs e)
    {
        string desc = "";
        if (dtCategories != null && txtCategoryID.Text != "")
        {
            DataRow[] selectedRows = dtCategories.Select("PROVIDER_CATEGORY_TYPE_ID = " + txtCategoryID.Text);
            if (selectedRows.Count() > 0)
                desc = selectedRows[0]["PROVIDER_CATEGORY_TYPE_NAME"].ToString();
        }
        txtCategory.Text = desc;
        this.SetCategoryDependentFields();
        ddlProviderType.Enabled = true;
    }
    protected void ddlProviderType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GetProviderTypeDependentFields();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "callJSFunction", "setApplicationTypeChangeEditability();", true);
        if (ddlProviderType.SelectedValue != null && ddlProviderType.SelectedValue != "0")
        {
            this.divDDFacilityNumber.Visible = (ddlProviderType.SelectedItem.Text.ToString().Trim().Substring(0,ddlProviderType.SelectedItem.Text.ToString().Trim().IndexOf('-')-1) == CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR);
        }
        else
        {
            this.divDDFacilityNumber.Visible = false;
        }

    }
    protected void ddlSpecialty_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GetSpecialtyDependentFields();
    }
    protected void rblEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.SetWaiverDropDownValues();
        if (this.ReferralID > 0)
        {
            this.SetTaxIDBasedOnOrganization();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool isKeyFieldEditRequest = false;
        if (this.WaiverTypeID == 4)
        {
            valZipReqd.Enabled = false;
            valZipFormat.Enabled = false;
            valZipExtRqd.Enabled = false;
            valZipExtFormat.Enabled = false;
            valTaxonmyCmp.Enabled = false;
        }
        if (!Page.IsValid)
        {
        }
        if (this.KeyFieldEditRequest || this.AdminKeyFieldEditRequest)
        {
            this.Model = this.LoadModelFromInitialKeyFields();
            isKeyFieldEditRequest = true;
        }
        else
        {
            this.Model = this.LoadModelFromForm();
        }
        //OHPNM-8396 -- Same NPI same Prov Type cannot be registered again. So show err message.
        List<SqlParameter> parameters = new List<SqlParameter>();

        parameters.Add(SqlParms.CreateParameter("NPI", DbType.Int32, this.Model.NPI, true));
        //parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, this.Model.ProviderTypeID, true));
        parameters.Add(SqlParms.CreateParameter("REGID", DbType.Int32, this.Model.RegID, true));
        parameters.Add(SqlParms.CreateParameter("MMIS_PROVIDER_TYPE_ID", DbType.String , this.Model.MMISProviderTypeID , true));
        parameters.Add(SqlParms.CreateParameter("isKeyFieldEditRequest", DbType.Boolean, isKeyFieldEditRequest, true));

        DataSet dsNPIAlreadyPresent = DataAccess.ExecuteStoredProcedure("usp_SelectNPIWithProviderType", parameters, "NPIProviderTypeIDCheck");
        if (Helper.HasRows(dsNPIAlreadyPresent))
        {
            foreach (DataRow dr in dsNPIAlreadyPresent.Tables[0].Rows)
            {
                // OHPNM-15626 Enrollment Status Code is evaluated by the SP, remove the check in the code
                //if (dr["ENROLLMENT_STATUS_CODE"] == DBNull.Value || (dr["ENROLLMENT_STATUS_CODE"] != DBNull.Value && ( (Convert.ToInt16(dr["ENROLLMENT_STATUS_CODE"]) == 1) || (Convert.ToInt16(dr["ENROLL_STATUS_CODE"]) == 3) ) ))
                // {
                // OHPNM-15626 there is no scenario where a NPI / Provider Type can be re-used
                if (Convert.ToString(dr["MMIS_PROVIDER_TYPE_ID"]) == this.Model.MMISProviderTypeID)
                {
                    lblErrorMessages.Text = "This NPI and Provider type already exists in the system. If you have questions, please contact the Integrated Help Desk at 1-800-686-1516, Option 2, Option 2";
                    return;
                }
                else
                {
                    List<SqlParameter> parameters1 = new List<SqlParameter>();

                    parameters1.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, dr["REG_ID"], true));
                    parameters1.Add(SqlParms.CreateParameter("UserID", DbType.String, this.Model.UserID, true));
                    // OHPNM-15626 this is an old check (see reference to TennCare Status), but is a sanity check
                    // only allow the NPI addition if the NPI is claimed but the REG_ID owning said NPI is not active
                    DataSet dsNPIRegStatus = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationStatuses", parameters1, "NPIRegStatus");
                    if (Helper.HasRows(dsNPIRegStatus))
                    {
                        if (dsNPIRegStatus.Tables[0].Rows[0]["TennCare Status"].ToString() == CON.PDMSCareStatus
                            && (dsNPIRegStatus.Tables[0].Rows[0][1].ToString() == CON.PDMSApplicationStatusComplete || dsNPIRegStatus.Tables[0].Rows[0][1].ToString() == CON.PDMSApplicationStatusApproved)
                            && dsNPIRegStatus.Tables[0].Rows[0]["Effective Date"] == DBNull.Value)
                        {
                            //do nothing here as the NPI can be re-used
                        }
                        else
                        {
                            lblErrorMessages.Text = "This NPI already exists in the system. If you have questions, please contact the Integrated Help Desk at 1-800-686-1516, Option 2, Option 2";
                            return;
                        }
                    } 
                    // OHPNM-15626 we should never have an instance where if statement returns no rows, but just in case, we already know we have an NPI that should not be allowed
                    else
                    {
                        lblErrorMessages.Text = "This NPI already exists in the system. If you have questions, please contact the Integrated Help Desk at 1-800-686-1516, Option 2, Option 2";
                        return;
                    }
                }
                // }
            }
            
        }

        presenter.ValidateProviderInformation(ProviderTypeChangeRequest);

        if (presenter.ErrorList.Any(x => x.Value == "Taxonomy is required."))
        {
            vsNewProvider.Visible = true; //validation summary
            trTaxonomyNPPES.Visible = true; // taxonomy div
        }

        if (!presenter.hasErrors)
        {
            if (this.WaiverTypeID == 1 || this.WaiverTypeID == 2 || this.WaiverTypeID == 3 || this.WaiverTypeID == 4)
            {
                if (!SaveAndSubmitFromWaiverType)
                {
                    mpeSaveAndSubmit.Show();
                    SaveAndSubmitFromWaiverType = true;
                    return;
                }
                else
                {
                    SetValidationSuccess();
                    SaveAndSubmitFromWaiverType = false;
                }

            }
            else
            {
                SetValidationSuccess();
            }

        }
        else
        {
            return;
        }
    }
    private void SetExitingProviderData()
    {
        if (!string.IsNullOrEmpty(txtExistingMedicaidId.Text))
        {
            DataSet ds = svc.SelectExitingProviderByMedicaidID(txtExistingMedicaidId.Text.Trim());
            if (Helper.HasRows(ds))
            {
                DataTable dt = ds.Tables[0];
                txtExistingProviderNPI.Text = Convert.ToString(dt.Rows[0]["NPI"]);
                txtExistingProviderName.Text = Convert.ToString(dt.Rows[0]["NAME"]);
            }
        }
    }
    protected void btnKFEYes_Click(object sender, EventArgs e)
    {
        //reload the model
        this.Model = this.LoadModelFromInitialKeyFields();
        //continue with updates or delete/insert
        
        if (presenter.hasErrors)
        {
            SetErrorMessages();
        }
        else
        {
            SetValidationSuccess();
        }
    }
    protected void btnCredYes_Click(object sender, EventArgs e)
    {
        //reload the model
        this.Model = this.LoadModelFromForm();
        this.Model.ApplicationTypeID = 11; //TODO HardCoded for showing Credential Application
        this.Model.ProviderTypeID = 251; //podiatrist for applicationtypeid=11
        this.Model.WorkflowID = 18;
        this.Model.WorkflowIDRequested = 18;
        presenter.InsertProvider();
        if (presenter.hasErrors)
        {
            SetErrorMessages();
        }
        else
        {
            ProviderManagementData data = this.Model;
            divNewProvider.Visible = false;
            (this.Page as RegistrationProvider).RegistrationId = data.RegID;
            (this.Page as RegistrationProvider).IsReadOnly = false;
            Response.Redirect("~/Process/Registration.aspx?RegId=" + data.RegID);
        }
    }
    protected void lnkEditNPI_Click(object sender, EventArgs e)
    {
        if (!EditingNPI)
        {
            trOldNPI.Visible = true;
            trOldNPIStartDate.Visible = true;
            trOldNPIEndDate.Visible = true;
            txtOldNPIEndDate.Visible = true;
            trNPIStartDate.Visible = true;
            trNPIEndDate.Visible = true;
            txtNPIEndDate.Visible = true;
            txtNPIStartDate.Visible = true;
            txtNPI.Enabled = true;
            txtNPI.CssClass = "formField";
            EditingNPI = true;
        }
        else
        {
            trOldNPI.Visible = false;
            trOldNPIStartDate.Visible = false;
            trOldNPIEndDate.Visible = false;
            txtOldNPIEndDate.Visible = false;
            trNPIStartDate.Visible = false;
            trNPIEndDate.Visible = false;
            txtNPIEndDate.Visible = false;
            txtNPIStartDate.Visible = false;
            txtNPI.Enabled = false;
            txtNPI.CssClass = "formFieldReadOnly";
            //Rolled back edited values
            txtNPI.Text = txtOldNPI.Text;
            EditingNPI = false;
            txtOldNPIEndDate.Text = OriginalNPIEndDate.HasValue ? OriginalNPIEndDate.Value.ToShortDateString() : string.Empty;
        }
    }
    protected void lnkEditEndDate_Click(object sender, EventArgs e)
    {
        trNPIEndDate.Visible = true;
        txtNPIEndDate.Visible = true;
        trNPIStartDate.Visible = false;
        txtNPI.Enabled = false;
        txtNPI.CssClass = "formFieldReadOnly";
    }
    #region Public Events
    public void InitView(ProviderManagerData keyData)
    {
        if (this.RegID == 0)
        {
            // this is a new registration
            this.WorkflowEventTypeId = 1;
            this.setHiddenWorkflowEventTypeId();
        }
        //this.RegID = this.RegistrationId;
        //this.RegID = keyData.RegID;
        this.WorkflowIDRequested = keyData.WorkflowIDRequested;
        this.ReferralID = keyData.ReferralID;
        this.PaperRequestQueueID = keyData.PaperRequestQueueID;
        this.txtTaxID.Text = keyData.TaxID;
        //OHPNM - 5509 og in as a Provider Admin that has a provider already in Maintenance. Select the 'New Provider' button. When the user gets to New Provider Registration, the Tax ID is prepopulated and should not be.
        if (keyData.WorkflowIDRequested == 0)
        {
            this.txtTaxID.Text = "";
        }
        //this.KeyFieldEditRequest = keyData.KeyFieldEditRequest;
        this.ReferralTypeID = keyData.ReferralTypeID;
        this.ApplicationTypeID = keyData.ApplicationTypeID;
        this.TaxIDTypeID = keyData.TaxIDTypeID;
        if (this.KeyFieldEditRequest || this.AdminKeyFieldEditRequest || this.IsAddODMorODAMedicaid)
            keyData.RegID = this.RegID;
        this.InitFormData();
        this.SetDefaultFields(keyData);
        presenter.Init(keyData);
        if (ProviderTypeChangeRequest && RegID > 0)
        {
            ProviderManagementData data = new ProviderManagementData();
            DataSet ds = svc.SelectRegistrationByRegID(this.RegID);
            data.LoadObjectFromDataset(ds);
            this.MMISProviderTypeID = data.MMISProviderTypeID;
            SetExistingProviderData(data);
        }
       else 
       { 
        if (IsLinkProvider)
        {
            LoadLinkProviderDetails();
        }
        else
        {
            if (RegID > 0 && !this.IsAddODMorODAMedicaid)
            {
                if (KeyFieldEditRequest || (this.AdminKeyFieldEditRequest && keyData.ConvertedProvider == false))
                {
                    ProviderManagementData data = new ProviderManagementData();
                    DataSet ds = svc.SelectRegistrationByRegID(this.RegID);
                    data.LoadObjectFromDataset(ds);
                    SetExistingProviderData(data);
                }
                else
                {
                    presenter.RequestConvertedRegData();
                }
            }
            else if (this.PaperRequestQueueID > 0)
            {
                presenter.RequestPaperRequestData();
            }
            else if (this.ReferralID > 0)
            {
                presenter.RequestReferralData();
            }
            else if (Helper.ConvertStringToInt32(this.txtCategoryID.Text) > 0)
            {
                this.SetCategoryDependentFields();
            }
            if (keyData.ReferralTypeID == CON.DiddReferralType.AssistedLiving)
            {
                this.SetAssistedLivingDefaults();
            }
            if (keyData.ConvertedProvider && !keyData.AdminKeyFieldEditRequest)
            {
                this.LockDownKeyIdentifierFields();
                this.ConvertedProvider = keyData.ConvertedProvider;
            }
            if (this.KeyFieldEditRequest)
            {
                this.SetDisplayForKeyFieldEdit();
            }
            if (this.AdminKeyFieldEditRequest)
            {
                this.SetDisplayForAdminKeyFieldEdit();
            }
            if (this.ReferralID > 0)
            {
                this.SetTaxIDBasedOnOrganization();
            }
            // BO: This is not the right way to do this, but I don't have time to mess with it. For now, only set the application type text 
            // to the keyData ApplicationTypeName if it's empty. It won't be empty during New Reg, but will be empty during Edit Key Provider Identifiers.
            //if (keyData.ApplicationTypeName != string.Empty)
            //    this.txtApplicationType.Text = keyData.ApplicationTypeName;
            //if (keyData.ApplicationTypeID > 0)
            //    this.txtApplicationTypeID.Text = keyData.ApplicationTypeID.ToString();
            //TODO: ProviderAddPresenter NPIRequired() would be a better check but it's tied to taxonomy which is edited on form and private so needs to be done in future
            bool isNFocus = keyData.ReferralID > 0 && keyData.ReferralTypeID != CON.DiddReferralType.AssistedLiving;
            if ((keyData.WorkflowIDRequested == CON.WorkflowType.RegistrationUpdateProvider ||
            keyData.WorkflowIDRequested == CON.WorkflowType.RegistrationRevalidation ||
            (keyData.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Conversion)) && !isNFocus)
            {
                this.SetDisplayForNPIEdit();
            }
        }

     }
    }
    #endregion
    #region Presenter Events
    public void SetProviderCategories(DataView categories)
    {
        if (Helper.HasRows(categories.Table))
            dtCategories = categories.Table;
        if (txtApplicationType.Text.Trim() == "Ordering, Referring, Prescribing")
        {
            categories.RowFilter = "provider_category_type_id = 1";
        }
        else
        {
            categories.RowFilter = "provider_category_type_id <> 0";
        }
        this.rptCategory.DataSource = categories;
        this.rptCategory.DataBind();
        SetCategoryDefault();
    }
    public void SetProviderTypes(DataView types)
    {
        types.RowFilter = "MMIS_PROVIDER_TYPE_ID <> 'LT'";//OHPNM-3199 : Internal Applications - Remove Building Option

        if(this.IsAddODMorODAMedicaid)
        {
            string ptTypes = AppSettings.Get("ProviderTypesAllowedforNonMedDODDtoMedicaid", "'16', '25', '26', '45', '55', '60','28', '38', '50', '65', '70', '71', '72', '76', '82', '83', '88', '89','55', '45', '74'"); 

            types.RowFilter = "MMIS_PROVIDER_TYPE_ID in (" + ptTypes + ")";
        }
        if (ProviderTypeChangeRequest)
        {
            string ptTypes = AppSettings.Get("ProviderTypesAllowedToChangeType", "'37', '42', '47', '52', '54', '96'");

            types.RowFilter = "MMIS_PROVIDER_TYPE_ID in (" + ptTypes + ") and MMIS_PROVIDER_TYPE_ID <> '" + this.MMISProviderTypeID + "'";
            //   types.Find() Write logic to remove currently selected provider type
        }

        types.Table.Columns.Add("IdWithName", typeof(string), "MMIS_PROVIDER_TYPE_ID + ' - ' + PROVIDER_TYPE_NAME");

        this.ddlProviderType.DataSource = types;
        this.ddlProviderType.DataValueField = "PROVIDER_TYPE_ID";
        this.ddlProviderType.DataTextField = "IdWithName";
        this.ddlProviderType.DataBind();
        this.ddlProviderType.Items.Insert(0, new ListItem("", "0"));
    }
    public void SetSpecialtyTypes(DataSet data)
    {
        this.ddlSpecialty.DataSource = data;

        data.Tables[0].Columns.Add("IdWithName", typeof(string), "MMIS_SPECIALTY_TYPE_ID + ' - ' + SPECIALTY_TYPE_NAME");

        this.ddlSpecialty.DataValueField = "SPECIALTY_TYPE_ID";
        this.ddlSpecialty.DataTextField = "IdWithName";
        this.ddlSpecialty.DataBind();
        this.ddlSpecialty.Items.Insert(0, new ListItem("", "0"));
    }
    public void SetTaxonomyTypes(DataView dv)
    {
        if (Helper.HasRows(dv.ToTable()))
        {
            this.ddlTaxonomy.DataSource = dv.ToTable(true, "TAXONOMY_TYPE_ID", "TaxonomyNameWithCode"); // Jira 2926
            this.ddlTaxonomy.DataValueField = "TAXONOMY_TYPE_ID";
            this.ddlTaxonomy.DataTextField = "TaxonomyNameWithCode";
            this.ddlTaxonomy.DataBind();
            this.ddlTaxonomy.Items.Insert(0, new ListItem("", "0"));
        }
    }

    private void LoadTaxonomyByNPI(string npi)
    {
        if (!string.IsNullOrEmpty(npi))
        {
            NPPESAPIResult result = svc.ValidNPIinNPPESApi(Convert.ToInt64(npi));
            if (result.result_count > 0)
            {
                SetTaxonomyTypesFromNPPES(result.results[0], true);
                trTaxonomyNPPES.Visible = true;
            }
        }

    }
    public void SetTaxonomyTypesFromNPPES(Result rs, bool showTaxonomoyDropdown)
    {
        if (!showTaxonomoyDropdown && rs == null)
        {
            this.ddlTaxonomyNPPES.DataSource = null;
            this.ddlTaxonomyNPPES.DataValueField = "TAXONOMYcode";
            this.ddlTaxonomyNPPES.DataTextField = "TaxonomyNameWithCode";
            this.ddlTaxonomyNPPES.DataBind();
            trTaxonomyNPPES.Visible = false;
            this.hdnName.Value = "";
            this.hdnNPI.Value = "";
            return;
        }
        DataView dv = null;
        //some taxonomies by provider type is filtered CR 63: Remove Taxonomy Code 251E00000X for EPD Waiver
        DataTable dtFilterTaxonomyType = null;//dsTaxonomyTypeToExclude();
        DataTable dt = NPPESAPIHelper.LoadTaxonomyFromNPPES(rs, dtFilterTaxonomyType);
        if (Methods.HasRows(dt))
        {
            dv = dt.AsDataView();
            this.ddlTaxonomyNPPES.DataSource = dv;
            this.ddlTaxonomyNPPES.DataValueField = "TAXONOMYcode";
            this.ddlTaxonomyNPPES.DataTextField = "TaxonomyNameWithCode";
            this.ddlTaxonomyNPPES.DataBind();
            this.ddlTaxonomyNPPES.Items.Insert(0, new ListItem("", "0"));
            this.valTaxonmyCmp.Enabled = true;
            this.valTaxonmyCmp.Visible = true;
            this.trTaxonomy.Visible = false;
            //We are storing the NPI and Name in hidden variables here since if this code is hit it means the NPPES API is validated
            // and we dont want to call it again unless Name and NPI is changed
            int ProviderCategoryTypeID = this.txtCategoryID.Text != "" ? Convert.ToInt32(this.txtCategoryID.Text) : 0;
            if (ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual || ProviderCategoryTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile)
            {
                string Name = string.Concat(txtFirstName.Text.Trim(), string.IsNullOrEmpty(txtMI.Text.Trim()) ? string.Empty : " ", txtMI.Text.Trim(), string.IsNullOrEmpty(txtLastName.Text.Trim()) ? string.Empty : " ", txtLastName.Text.Trim());
                this.hdnName.Value = Name;
            }
            this.hdnNPI.Value = txtNPI.Text;
        }
        else
        {
            this.ddlTaxonomyNPPES.DataSource = null;
            this.ddlTaxonomyNPPES.DataValueField = "TAXONOMYcode";
            this.ddlTaxonomyNPPES.DataTextField = "TaxonomyNameWithCode";
            this.ddlTaxonomyNPPES.DataBind();
            presenter.ErrorList.Add(presenter.NextValidationKey(), NPPESAPIHelper.ErrorMessageNoTaxonomy);
        }
        this.trTaxonomyNPPES.Visible = true;
    }
    public void SetPracticeTypes(DataView data)
    {
        this.ddlPracticeType.Items.Clear();
        this.ddlPracticeType.DataSource = data;
        this.ddlPracticeType.DataValueField = "TYPE_OF_PRACTICE_ID";
        this.ddlPracticeType.DataTextField = "TYPE_OF_PRACTICE_NAME";
        this.ddlPracticeType.DataBind();
        if (this.ddlPracticeType.Items.Count > 1)
        {
            this.ddlPracticeType.Items.Insert(0, new ListItem("", "-1"));
        }
        SetPracticeTypeDefault();
    }
    public void SetDeleteOnlySuccess()
    {
    }
    public void SetRecreateSuccess()
    {
    }
    public void SetKeyFieldUpdateResults()
    {
    }
    public void SetMultipleMedicaidsFound(DataTable dt)
    {
        //bind to a repeater - pulled over from Register
        this.rblMedaidIds.Items.Clear();
        foreach (DataRow row in dt.Rows)
        {
            rblMedaidIds.Items.Add(new ListItem(row["MedicaidID"].ToString(), row["RegID"].ToString()));
        }
        this.divMultipleMedicaidIds.Visible = true;
        this.cvMedicaidIDs.Enabled = true;
    }
    public void SetConvertedProviderData(ProviderManagementData data)
    {
        SetExistingRegData(data);
        //enabled editing if 0000 or null for bug 6453
        string zipExt = data.ZipExt;
        if (string.IsNullOrEmpty(zipExt) || zipExt == "0000")
        {
            this.txtZipCodeExt.Enabled = true;
        }
    }
    public void SetExistingProviderData(ProviderManagementData data)
    {
        //Called when editing key fields, need to save off initial key field values
        InitialKeyFields = new InitialRegKeyFields(data.ProviderTypeID, data.NPI, data.TaxIDTypeID, data.SpecialtyTypeID,
                data.TaxonomyTypeID, data.ZipCode, data.ZipExt, data.WorkflowID, data.RequestedEffectiveDate.Value, data.ReferralID, data.PaperRequestQueueID,
                data.ApplicationTypeID, data.ApplicationTypeName, data.WaiverTypeID);
        SetExistingRegData(data);
    }
    public void SetPaperRequestData(PaperRequestQueueData data)
    {
        this.SpecialtyTypeID = data.SpecialtyTypeID;
        this.TaxonomyTypeID = data.TaxonomyTypeID;
        this.ReferralID = data.ReferralID;
        this.ApplicationTypeID = data.ApplicationTypeID;
        this.txtProviderName.Text = data.ProviderName;
        this.txtApplicationTypeID.Text = data.ApplicationTypeID.ToString();
        // NEW
        presenter.RequestProviderCategories(data.WorkflowRequestedID, data.ApplicationTypeID, 0);
        //this.ddlCategory.SelectedValue = Helper.ValueExistsInDropDown(this.ddlCategory, data.ProviderCategoryTypeID.ToString()) ? data.ProviderCategoryTypeID.ToString() : "0";
        this.txtCategoryID.Text = data.ProviderCategoryTypeID.ToString();
        if (this.ReferralID > 0)
        {
            if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual)
            {
                this.rblEntityType.SelectedIndex = 0;
            }
            else if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.EntityFacility)
            {
                this.rblEntityType.SelectedIndex = 1;
            }
        }
        this.txtCategoryID_TextChanged(null, null);
        if (ddlProviderType.Items.Count > 0)
        {
            this.ddlProviderType.SelectedValue = Helper.ValueExistsInDropDown(this.ddlProviderType, data.ProviderTypeID.ToString()) ? data.ProviderTypeID.ToString() : "0";
            this.ddlProviderType_SelectedIndexChanged(null, null);
        }
        if (this.ddlSpecialty.Items.Count > 0)
        {
            this.ddlSpecialty.SelectedValue = Helper.ValueExistsInDropDown(this.ddlSpecialty, data.SpecialtyTypeID.ToString()) ? data.SpecialtyTypeID.ToString() : "0";
            this.ddlSpecialty_SelectedIndexChanged(null, null);
        }
        if (this.ddlTaxonomy.Items.Count > 0)
        {
            this.ddlTaxonomy.SelectedValue = Helper.ValueExistsInDropDown(this.ddlTaxonomy, data.TaxonomyTypeID.ToString()) ? data.TaxonomyTypeID.ToString() : "0";
        }
        this.txtTaxID.Text = data.TaxID;
        this.txtNPI.Text = data.NPI;
        txtOldNPI.Text = data.NPI;
        this.txtZipCode.Text = data.ZipCode;
        this.txtZipCodeExt.Text = data.ZipExt;
        txtFirstName.Text = string.Empty;
        txtMI.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtBirthDate.Text = string.Empty;
        this.ddlGender.SelectedIndex = -1;
        this.rblTaxIDType.SelectedIndex = -1;
    }
    private string GetMMISProviderTypeID(int providerTypeId)
    {
        string rtn = "";
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetProviderTypeById(providerTypeId);
        if (Methods.HasRows(ds))
        {
            rtn = Methods.GetStringValue(ds.Tables[0].Rows[0], "MMIS_PROVIDER_TYPE_ID");
        }
        return rtn;
    }
    public void SetExistingReferralData(ReferralData data)
    {
        this.txtFirstName.Text = data.FirstName;
        this.txtLastName.Text = data.LastName;
        this.txtProviderName.Text = data.GroupName;
        if (data.ReferralTypeID != CON.DiddReferralType.AssistedLiving)
        {
            this.rblEntityType.SelectedIndex = string.IsNullOrEmpty(data.GroupName) ? 0 : 1;
            SetWaiverDropDownValues();
        }
    }
    public void SetValidationSuccess()
    {
        if (this.KeyFieldEditRequest)
        {
            presenter.UpdateProviderKeyFields();
            if (EditingNPI)
                SaveNPIHistory();

            if (presenter.hasErrors)
            {
                SetErrorMessages();
                if (KeepPopupOpenEvent != null)
                {
                    KeepPopupOpenEvent();
                }
            }
            else
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet ds = psc.SelectRegistrationByRegID(this.RegID);

                //psc.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", this.RegID.ToString() }, { "WAIVER_SERVICE_UPDATE_TYPE_ID", CON.WaiverServiceUpdateType.OperatorUpdate.ToString() } });
                UserControls_ProviderManagementView view = LoadControl("~/PopupControls/ProviderManagementView.ascx") as UserControls_ProviderManagementView;
                view.InitView(new ProviderManagerData() { RegID = this.RegID });
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                int workflowID = svc.GetWorkflowInstance(view.KeyFields.ApplicationTypeID, view.KeyFields.ProviderCategoryTypeID,
                            view.KeyFields.ProviderTypeID, view.KeyFields.ReferralTypeID, true, false, false, false);
                DataSet dsNewReg = svc.SelectRegistrationByRegID(this.RegID);
                if(dsNewReg.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsNewReg.Tables[0].Rows[0];
                    int CurrentStepID = Methods.GetIntValue(dr["CurrentStepID"]);
                    if (CurrentStepID == 0 || CurrentStepID == -1)
                        view.BeginUpdateRegistration(workflowID, false, false, false, false, CON.WaiverServiceUpdateType.ODM,0,this.KeyFieldEditRequest);
                }
                
            }
            DataRow row = Registration.GetRegistration(RegID);
            if (row != null)
            {
                if (Helper.GetInt("WORKFLOW_EVENT_TYPE_ID", row) == CON.WorkflowEventType.UpdateReg)
                {
                    Response.Redirect("~/Process/ProviderUpdateSummary.aspx?RegId=" + this.RegID);
                }
                else
                {
                    Response.Redirect("~/Process/Registration.aspx?RegId=" + this.RegID);
                }
            }
            //for key field edits must verify update
            /*this.ltlKFEConfirm.Text = KeyFieldUpdatesRequireDelete() ? Resources.BrandingResource.KEY_FIELD_EDIT_DELETE_CONFIRMATION_MESSAGE
                : Resources.BrandingResource.KEY_FIELD_EDIT_UPDATE_CONFIRMATION_MESSAGE;
            this.mpeKFEVerify.Show();
            if (KeepPopupOpenEvent != null)
            {
                KeepPopupOpenEvent();
            }*/
            return;
        }
        else if (this.AdminKeyFieldEditRequest)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectRegistrationByRegID(this.RegID);

            psc.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", this.RegID.ToString() }, { "WAIVER_SERVICE_UPDATE_TYPE_ID", CON.WaiverServiceUpdateType.OperatorUpdate.ToString() } });
            UserControls_ProviderManagementView view = LoadControl("~/PopupControls/ProviderManagementView.ascx") as UserControls_ProviderManagementView;
            view.InitView(new ProviderManagerData() { RegID = this.RegID });
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            int workflowID = 1; // force it to workflow 1, since that's the only one that can happen
            view.BeginUpdateRegistration(workflowID, false, false, false, false, CON.WaiverServiceUpdateType.ODM, 0, false, this.AdminKeyFieldEditRequest);

            PDMSService.PDMSServiceClient pscl = new PDMSService.PDMSServiceClient();
            DataSet dst = pscl.SelectRegistrationByRegID(this.RegID);
            int NewProcessID = Convert.ToInt32(dst.Tables[0].Rows[0]["ProcessID"].ToString());
            Registration.SetProviderSectionNodeStatusId(this.RegID, 1, CON.RegistrationProviderStatusTypeId.Modified);
            pscl.WF_TakeAction(NewProcessID, "Submit Update", string.Empty);
            // save key fields after starting workflow, so screening can occur on the modified data
            presenter.UpdateProviderAdminKeyFields();
            // if we have errors, show to the user
            if (presenter.hasErrors)
            {
                SetErrorMessages();
                if (KeepPopupOpenEvent != null)
                {
                    KeepPopupOpenEvent();
                }
            }
            Response.Redirect("~/Process/GroupReview.aspx");
            return;
        }
        // This change was implemented for Arizona Demo Commenting it. RG 
        //if (this.IsCredentialingProcess)
        //{
        //    //for key field edits must verify update
        //    //this.ltlCredConfirm.Text = KeyFieldUpdatesRequireDelete() ? Resources.BrandingResource.KEY_FIELD_EDIT_DELETE_CONFIRMATION_MESSAGE
        //    //    : Resources.BrandingResource.KEY_FIELD_EDIT_UPDATE_CONFIRMATION_MESSAGE;
        //    this.mpeCredential.Show();
        //    if (KeepPopupOpenEvent != null)
        //    {
        //        KeepPopupOpenEvent();
        //    }
        //    return;
        //}
        if (IsLinkProvider)
        {
            presenter.InsertLinkedProvider();
        }
        else if(IsAddODMorODAMedicaid)
        {
            presenter.UpdateToMedicaidProvider();
        }
        else
        {
            if (this.Model.RegID > 0 && !ProviderTypeChangeRequest)
            {
                //Set re-enrollment due date on conversion to 90 days from now
                //this.Model.EndDate = DateTime.Now.AddDays(90);
                presenter.ConnectConvertedProvider();
                if (EditingNPI)
                    SaveNPIHistory();
            }
            else
            {
                if (ProviderTypeChangeRequest)
                    presenter.InsertProvider(this.Model.RegID);
                else
                    presenter.InsertProvider();
                if (!string.IsNullOrEmpty(txtExistingMedicaidId.Text))
                {
                    presenter.InsertExitingMedicaidId();
                    presenter.InsertPrimaryServiceAddress();
                }
            }
        }
        if (!presenter.hasErrors)
        {
            ProviderManagementData data = this.Model;
            divNewProvider.Visible = false;
            (this.Page as RegistrationProvider).RegistrationId = data.RegID;
            (this.Page as RegistrationProvider).IsReadOnly = false;
            if (Model.ApplicationTypeID == CON.ApplicationType.Waiver)           /*AKAS*/
            {
                if (Model.WaiverTypeID == 1)
                {
                    Response.Redirect("~/Process/Registration.aspx?RegId=" + data.RegID);
                }
                else
                {
                    //call partial webservice
                    //get passed / failure response
                    //use that to call below code.
                    string strCheck = AppSettings.Get("MakeWSRequestCallToSI", "false");
                    bool makeRequest = false;
                    if (strCheck.ToLower().Equals("true"))
                    {
                        makeRequest = true;
                    }
                    int sId = 0;
                    string mId = string.Empty;
                    string txnResult = string.Empty;
                    string partialTTService = string.Empty;
                    string partialSubService = string.Empty;
                    int transactionType = 0;
                    if (makeRequest)
                    {
                        if (Model.ApplicationTypeID == CON.ApplicationType.Waiver && Model.WaiverTypeID == CON.WaiverApplicationTypeID.ODA)
                        {
                            transactionType = (int)TransactionController.TransactionTypeNew.SendPCWPartial;
                            partialTTService = CON.TransactionTypeValues.PCW_Partial;
                            partialSubService = CON.PartialProviderSubscriberSystems.PCW;
                        }
                        else
                        {
                            transactionType = (int)TransactionController.TransactionTypeNew.SendPSMPartial;
                            partialTTService = CON.TransactionTypeValues.PSM_Partial;
                            partialSubService = CON.PartialProviderSubscriberSystems.PSM;
                        }

                        DataSet ds1 = RegistrationController.SelectMedicaidId(data.RegID);
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            mId = ds1.Tables[0].Rows[0]["MEDICAID_ID"].ToString();
                            sId = Convert.ToInt32(ds1.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"]);
                        }
                        if (transactionType > 0)
                        {
                            int tqId = TransactionController.InsertTransactionQueue(
                                 transactionType, data.RegID, sId, DateTime.Now, null, null, DateTime.Now, CON.appWorkflowUserId);
                            //SetProcessParameter(CON.ProcessParameter.TransactionQueueID, tqId.ToString());
                            //SaveProcessParameters();
                            PartialProviderRequestResponse pprr = new PartialProviderRequestResponse();
                            RegistrationController.PopulateStagingData(tqId, partialTTService, string.Empty);
                            //ServicePointManager.Expect100Continue = true;
                            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                            txnResult = pprr.partialProviderManagementSubmitRequest(tqId, partialSubService, true);
                            if (txnResult.Equals(CON.TransactionResult.TransactionPassed))
                            {
                                TransactionController.UpdateTransactionQueue(tqId, DateTime.Now, DateTime.Now, DateTime.Now, CON.appWorkflowUserId);
                                WaiverRedirect(data.RegID.ToString(), data.DDContractNumber.ToString());
                            }
                            else
                            {
                                TransactionController.UpdateTransactionQueue(tqId, DateTime.Now, DateTime.Now, DateTime.Now, CON.appWorkflowUserId);
                                Response.Redirect("~/Process/ProviderDetailsNew.aspx?regID=" + data.RegID);
                            }

                        }
                    }
                    else
                    {
                        if (Model.ApplicationTypeID == CON.ApplicationType.Waiver && Model.WaiverTypeID == CON.WaiverApplicationTypeID.ODA)
                        {
                            transactionType = (int)TransactionController.TransactionTypeNew.SendPCWPartial;
                            partialTTService = CON.TransactionTypeValues.PCW_Partial;
                            partialSubService = CON.PartialProviderSubscriberSystems.PCW;
                        }
                        else
                        {
                            transactionType = (int)TransactionController.TransactionTypeNew.SendPSMPartial;
                            partialTTService = CON.TransactionTypeValues.PSM_Partial;
                            partialSubService = CON.PartialProviderSubscriberSystems.PSM;
                        }

                        DataSet ds1 = RegistrationController.SelectMedicaidId(data.RegID);
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            mId = ds1.Tables[0].Rows[0]["MEDICAID_ID"].ToString();
                            sId = Convert.ToInt32(ds1.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"]);
                        }
                        if (transactionType > 0)
                        {
                            //Lower Envs webservice calls are disabled.
                            string ResponseCode = "1000";
                            string ResponseDetails = "Call to DODD";
                            string ResponseMessage = "Call Successful";
                            string ResponseType = "Success";
                            string response = "Success";
                            string SITransactionKey = ProviderManagementHelper.GetUniqueKey(32);
                            string pnmTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

                            int tqId = TransactionController.InsertTransactionQueue(
                                 transactionType, data.RegID, sId, DateTime.Now, null, null, DateTime.Now, CON.appWorkflowUserId);

                            TransactionController.UpdateTransactionQueue(tqId, DateTime.Now, DateTime.Now, DateTime.Now, CON.appWorkflowUserId);
                            InfoAccessController.SaveSoapResponseCodeException(tqId, SITransactionKey, ResponseCode,
                                ResponseDetails, ResponseMessage, ResponseType, DateTime.Now, new Guid(CON.appWorkflowUserId),
                                response.ToString(), pnmTransactionKey);
                        }
                        Response.Redirect("~/Process/ProviderDetailsNew.aspx?regID=" + data.RegID.ToString());
                        //WaiverRedirect(data.RegID.ToString(), data.DDContractNumber.ToString());
                    }
                }
            }
            else
            {
                Response.Redirect("~/Process/Registration.aspx?RegId=" + data.RegID);
            }
        }
    }
    public void WaiverRedirect(string regID, string ddContractNum)
    {
        string Reg64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(regID));
        string Cnt64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ddContractNum));
        string Src64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("pnm"));
        string Tgt64 = "";
        string url = "";
        if (Model.WaiverTypeID == 2)
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
    public void SetErrorMessages()
    {
        if (presenter.hasErrors)
        {
            ClearErrorMessages();
            SetValidationErrors();
        }
    }
    #endregion
    #region Private Events
    private void SetPaperEntryFieldAppearance()
    {
        if (this.PaperRequestQueueID == 0)
        {
            trTaxIDType.Visible = true;
            rblTaxIDType.Enabled = true;
            trMedicaid.Visible = false;
        }
        else
        {
            //for bug number 6077
            trTaxIDType.Visible = true;
            rblTaxIDType.Enabled = true;
            trMedicaid.Visible = true;
        }
    }
    private void SetDefaultFields(ProviderManagerData keyData)
    {
        if (keyData.TaxIDTypeID > 0)
        {
            this.rblTaxIDType.SelectedValue = keyData.TaxIDTypeID.ToString();
            this.rblTaxIDType.Enabled = false;
        }
        else
            this.rblTaxIDType.SelectedIndex = -1;
        this.txtZipCode.Text = keyData.ZipCode;
        this.txtZipCodeExt.Text = keyData.ZipExt;
    }
    private void InitFormData()
    {
        this.ResetFieldEditability();
        this.SetPaperEntryFieldAppearance();
        txtNPI.Text = txtMedicaidID.Text = txtZipCode.Text = txtZipCodeExt.Text = txtProviderName.Text = txtApplicationNumber.Text = txtOldNPI.Text = string.Empty;
        this.ddlTaxonomy.Items.Clear();
        this.ddlSpecialty.Items.Clear();
        this.ddlProviderType.Items.Clear();
        this.txtCategoryID.Text = "-1";
        this.ddlPracticeType.Items.Clear();
        this.txtApplicationTypeID.Text = "";
        txtFirstName.Text =
        txtMI.Text = txtLastName.Text =
        txtBirthDate.Text = string.Empty;
        //txtApplicationType.Text = string.Empty;
        BindGender();
        this.ddlGender.SelectedIndex = -1;
        this.txtEffectiveDate.Text = DateTime.Now.ToShortDateString();
        this.trRED.Visible = this.RegID == 0 && this.WorkflowIDRequested != CON.WorkflowType.GroupMemberProfile;//cannot edit for existing/converted providers or group member profiles
        this.SetVisibilityByProviderCategory(WorkflowIDRequested == CON.WorkflowType.GroupMemberProfile ? CON.ProviderCategoryTypeID.Individual : CON.ProviderCategoryTypeID.Group);
        this.SetEditabilityByWorkflowRequestType();
        bool isNPIAPIEnabled = AppSettings.Get("NPI-Registry-Enabled").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase) ? true : false;
        //this drop down taxonomy is displayed only for converted providers or for KeyFieldEditRequest for provider or admin
        if (string.IsNullOrEmpty(this.txtNPI.Text.Trim()))
        {
            this.trTaxonomy.Visible = false;
            this.trTaxonomyNPPES.Visible = false;
        }
        else
        {
            this.trTaxonomy.Visible = this.RegID > 0 || !isNPIAPIEnabled;
            //LoadTaxonomyByNPI(this.txtNPI.Text.Trim());
        }

        //CheckNPIRequired will call NPPESAPI we need to check when new provider or converted/existing or when user changed NPI for KeyFieldEditRequest or AdminKeyFieldEditRequest
        CheckNPIRequired = this.RegID == 0 || (this.RegID > 0 && !this.KeyFieldEditRequest) || !isNPIAPIEnabled;
        this.hdnName.Value = "";
        this.hdnNPI.Value = "";
        // this is displayed dynamically from NPPES API in SetTaxonomyTypesFromNPPES
        this.SetDynamicValidationMessages();
        //this.divKeyFieldEditInfo.Visible = KeyFieldEditRequest;
        trNPIStartDate.Visible = false;
        trNPIEndDate.Visible = false;
        trOldNPI.Visible = false;
        trOldNPIStartDate.Visible = false;
        trOldNPIEndDate.Visible = false;
        txtOldNPIEndDate.Visible = false;
        this.txtNPIStartDate.Visible = false;
        this.txtNPIEndDate.Visible = false;
        this.txtNPIStartDate.Text = DateTime.Now.ToShortDateString();
        this.txtNPIEndDate.Text = DateTime.Now.ToShortDateString();
        lnkEditNPI.Visible = false;
        lnkEditEndDate.Visible = false;
        EditingNPI = false;
        OriginalNPIEndDate = null;
        txtEffectiveDate.Enabled = false;
        divDDContractNumber.Visible = false;
        divDDFacilityNumber.Visible = false;
    }
    private void SetDynamicValidationMessages()
    {
        this.valAppNbrRqd.ErrorMessage = string.Format("* {0}", Resources.BrandingResource.REFERRAL_NUMBER_REQUIRED);
        this.valApplicationNbr.ErrorMessage = string.Format("* {0}", Resources.BrandingResource.REFERRAL_NUMBER_NOT_FOUND);
    }
    private ProviderManagementData LoadModelFromForm()
    {
        ProviderManagementData data = new ProviderManagementData();
        data.ExitingMedicaId = txtExistingMedicaidId.Text;
        data.RegID = RegID;
        data.WorkflowIDRequested = WorkflowIDRequested;
        data.ReferralID = ReferralID;
        data.PaperRequestQueueID = PaperRequestQueueID;
        //data.OrganizationName = txtOrgName.Text;            /*akash*/
        data.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        data.DDContractNumber = this.txtDDContractNumber.Text;
        data.DDFacilityNumber = this.txtDDFacilityNumber.Text;
        var nursingLicense = RadiochkNursingLicense.SelectedIndex == -1 ? string.Empty : RadiochkNursingLicense.SelectedValue;
        if (nursingLicense == "YES")
        {
            data.SetNursingLicense(true);
        }
        data.ApplicationTypeID = this.ApplicationTypeID;
        data.WaiverTypeID = this.WaiverTypeID;
        //data.ApplicationTypeName = txtApplicationType.Text;
        data.ProviderCategoryTypeID = Convert.ToInt32(txtCategoryID.Text) > -1 ? Convert.ToInt32(txtCategoryID.Text) : 0;
        if (ddlProviderType.Items.Count > 0)
        {
            data.ProviderTypeID = ddlProviderType.SelectedIndex > -1 ? Convert.ToInt32(ddlProviderType.SelectedValue) : 0;
            data.MMISProviderTypeID = presenter.GetMMISProviderTypeID(data.ProviderTypeID);
        }
        if (ddlSpecialty.Items.Count > 0)
        {
            data.SpecialtyTypeID = ddlSpecialty.SelectedIndex > -1 ? Convert.ToInt32(ddlSpecialty.SelectedValue) : 0;
        }
        if (this.ApplicationTypeID == CON.ApplicationType.Waiver)
        {
            if (WaiverTypeID == CON.WaiverType.ODA)
            {
                if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual || data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.EntityFacility)
                {
                    if (CON.ProviderType.Waivered_Services_Individual == data.ProviderTypeName ||
                        CON.ProviderType.NON_AGENCY_NURSE_RN_OR_LPN == data.ProviderTypeName ||
                        CON.ProviderType.WAIVERED_SERVICES_ORGANIZATION == data.ProviderTypeName)
                    {
                        data.SpecialtyTypeID = CON.SpecialtyTypeID.ODA_WAIVER;
                    }
                    if (CON.ProviderType.HOME_AND_COMMUNITY_BASED_ODA_ASSISTED_LIVING == data.ProviderTypeName)
                    {
                        data.SpecialtyTypeID = CON.SpecialtyTypeID.ODA_WAIVER;
                    }
                }

            }
            if (WaiverTypeID == CON.WaiverType.DODD)
            {
                if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual || data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.EntityFacility)
                {
                    if (CON.ProviderType.Non_Agency_Personal_Care_Aide == data.ProviderTypeName ||
                        CON.ProviderType.WAIVERED_SERVICES_ORGANIZATION == data.ProviderTypeName)
                    {
                        data.SpecialtyTypeID = CON.SpecialtyTypeID.DODD_WAIVER;
                    }
                }
            }
        }
        if (trTaxonomyNPPES.Visible || data.CheckNPIRequired) //if NPI changed
        {
            if (ddlTaxonomyNPPES.Items.Count > 0)
            {
                data.TaxonomyCode = ddlTaxonomyNPPES.SelectedIndex > -1 ? ddlTaxonomyNPPES.SelectedValue : "0";
                data.TaxonomyDetail = ddlTaxonomyNPPES.SelectedIndex > -1 ? ddlTaxonomyNPPES.SelectedItem.Text.Replace("(" + ddlTaxonomyNPPES.SelectedValue + ")", "").Trim() : "";
                data.TaxonomyTypeID = 0;
            }
        }

        if (ddlTaxonomy.Items.Count > 0)
        {
            data.TaxonomyTypeID = SetTaxonomySelectedValue();

        }
        data.TypeOfPracticeID = null;
        data.DBA = null;
        data.ProviderName = txtProviderName.Text.Trim();
        data.FirstName = txtFirstName.Text.Trim();
        data.MiddleInitial = txtMI.Text.Trim();
        data.LastName = txtLastName.Text.Trim();
        if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual || data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile)
        {
            data.ProviderName = data.IndividualFullName;
        }
        data.TaxID = txtTaxID.Text;
        data.IsPaperApplication = PaperRequestQueueID == 0 ? false : true;
        data.TaxIDTypeID = rblTaxIDType.SelectedIndex == -1 ? 0 : Convert.ToInt32(rblTaxIDType.SelectedValue);
        data.NPI = this.txtNPI.Text.Trim();
        if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual)
            data.PracticeLocation = string.Empty;
        else
            data.PracticeLocation = txtProviderName.Text.Trim();
        data.ZipCode = this.txtZipCode.Text.Trim();
        data.ZipExt = this.txtZipCodeExt.Text.Trim();
        data.MedicaidID = this.txtMedicaidID.Text.Trim();
        data.Gender = ddlGender.SelectedIndex == -1 ? string.Empty : ddlGender.SelectedValue;
        if (!string.IsNullOrEmpty(txtBirthDate.Text))
        {
            data.BirthDate = Convert.ToDateTime(txtBirthDate.Text);
        }
        if (!string.IsNullOrEmpty(txtEffectiveDate.Text))
        {
            DateTime now = DateTime.Now;
            DateTime effectiveDate;
            DateTime.TryParse(txtEffectiveDate.Text, out effectiveDate);
            data.RequestedEffectiveDate = effectiveDate;
        }
        //Update start date if editing NPI. Keep Old Start Date if not.
        string startDateText = EditingNPI ? txtNPIStartDate.Text : txtOldNPIStartDate.Text;
        DateTime npiStartDate;
        DateTime.TryParse(startDateText, out npiStartDate);
        data.NPIStartDate = npiStartDate;
        //Update end date if editing npi. Keep same npi if not editing, end date can be nullable
        if (EditingNPI)
        {
            string endDateText = txtNPIEndDate.Text;
            DateTime npiEndDate;
            if (DateTime.TryParse(endDateText, out npiEndDate))
                data.NPIEndDate = npiEndDate;
            else
                data.NPIEndDate = null;
        }
        else
        {
            data.NPIEndDate = OriginalNPIEndDate;
        }
        data.RegCreateDateTime = DateTime.Now;
        data.ReferralNumber = this.txtApplicationNumber.Text.Trim();
        data.LastModifiedDate = DateTime.Now;
        data.LastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        data.WorkflowID = WorkflowIDRequested;
        data.RegistrationStatusTypeID = CON.RegistrationStatusTypeId.Pending;
        this.cvMedicaidIDs.Enabled = false;
        this.divMultipleMedicaidIds.Visible = false;
        if (this.EndDate != null)
            data.EndDate = this.EndDate;
        // RG - Commenting it as this was added for Arizona Demo
        //if (data.WorkflowID == 1 && data.ProviderTypeID == 18)
        //{
        //    this.IsCredentialingProcess = true; //TODO hardcoding for credentialing
        //}
        data.CheckNPIRequired = this.CheckNPIRequired =  //check to see if we need to check from API again
          ((data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual
              || data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile)
              && this.hdnName.Value != data.ProviderName) || this.hdnNPI.Value != data.NPI;
        if (chkRetroCoverage.Visible && chkRetro.Visible && chkRetro.Checked && !string.IsNullOrEmpty(txtEffectiveDate.Text) && txtEffectiveDate.Enabled)
            data.RetroEffectiveDate = true;
        else
            data.RetroEffectiveDate = false;
        return data;
    }
    /// <summary>
    /// Used during key field edits when change a key field that requires a delete of existing registration and insert of new
    /// </summary>
    /// <returns></returns>
    private ProviderManagementData LoadModelFromInitialKeyFields()
    {
        ProviderManagementData data = new ProviderManagementData();
        data.KeyFieldEditRequest = this.KeyFieldEditRequest;
        data.AdminKeyFieldEditRequest = this.AdminKeyFieldEditRequest;
        data.DDContractNumber = this.txtDDContractNumber.Text;
        data.DDFacilityNumber = this.txtDDFacilityNumber.Text;
        var nursingLicense = RadiochkNursingLicense.SelectedIndex == -1 ? string.Empty : RadiochkNursingLicense.SelectedValue;
        if (nursingLicense == "YES")
        {
            data.SetNursingLicense(true);
        }
        data.ApplicationTypeID = this.ApplicationTypeID;
        data.WaiverTypeID = this.WaiverTypeID;
        //Saved off values that are not editable by user for this functionality
        data.RegID = RegID;
        data.WorkflowIDRequested = InitialKeyFields.WorkflowID;
        data.ReferralID = InitialKeyFields.ReferralID;
        data.PaperRequestQueueID = InitialKeyFields.PaperRequestID;
        data.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        // data.OrganizationName = txtOrgName.Text;
        //Pull from form
        data.ProviderCategoryTypeID = Convert.ToInt32(txtCategoryID.Text) > -1 ? Convert.ToInt32(txtCategoryID.Text) : 0;
        if (ddlProviderType.Items.Count > 0)
        {
            data.ProviderTypeID = ddlProviderType.SelectedIndex > -1 ? Convert.ToInt32(ddlProviderType.SelectedValue) : 0;
            data.MMISProviderTypeID = presenter.GetMMISProviderTypeID(data.ProviderTypeID);
        }
        if (ddlSpecialty.Items.Count > 0)
        {
            data.SpecialtyTypeID = ddlSpecialty.SelectedIndex > -1 ? Convert.ToInt32(ddlSpecialty.SelectedValue) : 0;
        }
        if (ddlTaxonomy.Items.Count > 0)
        {
            data.TaxonomyTypeID = ddlTaxonomy.SelectedIndex > -1 ? Convert.ToInt32(ddlTaxonomy.SelectedValue) : 0;
        }
        data.TypeOfPracticeID = null;
        data.DBA = null;
        data.ProviderName = txtProviderName.Text.Trim();
        data.FirstName = txtFirstName.Text.Trim();
        data.MiddleInitial = txtMI.Text.Trim();
        data.LastName = txtLastName.Text.Trim();
        data.ExitingMedicaId = txtExistingMedicaidId.Text.Trim();
        if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual || data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile)
        {
            data.ProviderName = data.IndividualFullName;
        }
        data.TaxID = txtTaxID.Text;
        data.IsPaperApplication = PaperRequestQueueID == 0 ? false : true;
        data.TaxIDTypeID = rblTaxIDType.SelectedIndex == -1 ? 0 : Convert.ToInt32(rblTaxIDType.SelectedValue);
        data.NPI = this.txtNPI.Text.Trim();
        if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual)
            data.PracticeLocation = string.Empty;
        else
            data.PracticeLocation = txtProviderName.Text.Trim();
        data.ZipCode = this.txtZipCode.Text.Trim();
        data.ZipExt = this.txtZipCodeExt.Text.Trim();
        data.MedicaidID = this.txtMedicaidID.Text.Trim();
        data.Gender = ddlGender.SelectedIndex == -1 ? string.Empty : ddlGender.SelectedValue;
        if (!string.IsNullOrEmpty(txtBirthDate.Text))
        {
            data.BirthDate = Convert.ToDateTime(txtBirthDate.Text);
        }
        if (!string.IsNullOrEmpty(txtEffectiveDate.Text))
        {
            DateTime now = DateTime.Now;
            DateTime effectiveDate;
            DateTime.TryParse(txtEffectiveDate.Text, out effectiveDate);
            data.RequestedEffectiveDate = effectiveDate;
            //bug 8055 - retro_effective_date field is removed and instead it is calculated based on the new field reg_create_date_time
            /*if (txtEffectiveDate.Enabled)
            {
                int months = Convert.ToInt32(AppSettings.Get("RetroEffectiveDateWindow"));
                data.RetroEffectiveDate = effectiveDate.AddDays(months) < now ? true : false;
            }*/
        }
        //Update start date if editing NPI. Keep Old Start Date if not.
        string startDateText = EditingNPI ? txtNPIStartDate.Text : txtOldNPIStartDate.Text;
        DateTime npiStartDate;
        DateTime.TryParse(startDateText, out npiStartDate);
        data.NPIStartDate = npiStartDate;
        //Update end date if editing npi. Keep same npi if not editing, end date can be nullable
        if (EditingNPI)
        {
            string endDateText = txtNPIEndDate.Text;
            DateTime npiEndDate;
            if (DateTime.TryParse(endDateText, out npiEndDate))
                data.NPIEndDate = npiEndDate;
            else
                data.NPIEndDate = null;
        }
        else
        {
            data.NPIEndDate = OriginalNPIEndDate;
        }
        data.ReferralNumber = this.txtApplicationNumber.Text.Trim();
        data.LastModifiedDate = DateTime.Now;
        data.LastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        data.WorkflowID = WorkflowIDRequested;
        data.RegistrationStatusTypeID = CON.RegistrationStatusTypeId.Pending;
        data.CheckNPIRequired = this.CheckNPIRequired =  //check to see if we need to check from API again
           ((data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual
               || data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile)
               && this.hdnName.Value != data.ProviderName) || this.hdnNPI.Value != data.NPI;
        if (data.CheckNPIRequired || ddlTaxonomyNPPES.Visible == true)
        {
            if (ddlTaxonomyNPPES.Items.Count > 0)
            {
                data.TaxonomyCode = ddlTaxonomyNPPES.SelectedIndex > -1 ? ddlTaxonomyNPPES.SelectedValue : "0";
                data.TaxonomyDetail = ddlTaxonomyNPPES.SelectedIndex > -1 ? ddlTaxonomyNPPES.SelectedItem.Text.Replace("(" + ddlTaxonomyNPPES.SelectedValue + ")", "").Trim() : "";
                data.TaxonomyTypeID = SetTaxonomySelectedValue();
            }
        }
        this.cvMedicaidIDs.Enabled = false;
        this.divMultipleMedicaidIds.Visible = false;
        if (chkRetroCoverage.Visible && chkRetro.Visible && chkRetro.Checked && !string.IsNullOrEmpty(txtEffectiveDate.Text) && txtEffectiveDate.Enabled)
            data.RetroEffectiveDate = true;
        else
            data.RetroEffectiveDate = false;
        return data;
    }

    private int SetTaxonomySelectedValue()
    {
        int taxnomyTypeId = 0;
        if (ddlTaxonomy.Items.Count > 0)
        {
            foreach (ListItem item in ddlTaxonomy.Items)
            {
                var _text = item.Text;
                var isvalid = _text.Contains(ddlTaxonomyNPPES.SelectedValue);
                if (isvalid)
                {
                    taxnomyTypeId = Convert.ToInt32(item.Value);
                    ddlTaxonomy.SelectedValue = item.Value;
                    break;
                }
            }
        }
        return taxnomyTypeId;
    }


    private string SetTaxonomyCodeFromTypeId(int taxonomyTypeId, string taxonomyCode)
    {
        string taxnomyCode = null;
        if (string.IsNullOrEmpty(taxonomyCode))
        {
            if (ddlTaxonomy.Items.Count > 0)
            {
                foreach (ListItem item in ddlTaxonomy.Items)
                {
                    if (Convert.ToInt32(item.Value) == taxonomyTypeId)
                    {
                        taxnomyCode = item.Text;
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(taxnomyCode) && ddlTaxonomyNPPES.Items.Count > 0)
            {
                var arrTaxnomyCode = taxnomyCode.Split('(');
                var _taxnomycode = arrTaxnomyCode[1].TrimEnd(')');
                foreach (ListItem item in ddlTaxonomyNPPES.Items)
                {
                    var isexist = _taxnomycode.Trim().Equals(item.Value.Trim());
                    if (isexist)
                    {
                        taxnomyCode = item.Value;
                        break;
                    }
                }
            }
        }
        if (!string.IsNullOrEmpty(taxonomyCode) && ddlTaxonomyNPPES.Items.Count > 0)
        {
            foreach (ListItem item in ddlTaxonomyNPPES.Items)
            {
                var isexist = taxonomyCode.Trim().Equals(item.Value.Trim());
                if (isexist)
                {
                    taxnomyCode = item.Value;
                    break;
                }
            }
        }
        
        return taxnomyCode;
    }

    private void SetValidationErrors()
    {
        foreach (var pair in presenter.ErrorList)
        {
            AddErrorMessage(pair.Value);
        }
    }
    private void AddErrorMessage(string message)
    {
        lblErrorMessages.Text = string.Format("{0}{1}<br />", lblErrorMessages.Text, message);
    }
    private void ClearErrorMessages()
    {
        lblErrorMessages.Text = string.Empty;
    }
    private void SetEditabilityByWorkflowRequestType()
    {
        switch (this.WorkflowIDRequested)
        {
            case CON.WorkflowType.GroupMemberProfile:
                this.SetEditabilityOnWaiverServicesFields(true);
                this.SetVisibilityOnWaiverServicesFields(false);
                this.SetValidationOnWaiverServiceOnlyFields(false);
                this.SetValidationOnNonGroupMemberProfileFields(false);
                this.SetEditabilityOnGroupMemberProfileFields(false);
                break;
            case CON.WorkflowType.RegistrationDIDDReferral:
                this.SetEditabilityOnWaiverServicesFields(false);
                this.SetVisibilityOnWaiverServicesFields(true);
                this.SetValidationOnWaiverServiceOnlyFields(true);
                break;
            default:
                this.SetEditabilityOnGroupMemberProfileFields(true);
                this.SetValidationOnNonGroupMemberProfileFields(true);
                this.SetEditabilityOnWaiverServicesFields(true);
                this.SetValidationOnWaiverServiceOnlyFields(false);
                this.SetVisibilityOnWaiverServicesFields(this.ReferralID > 0);
                break;
        }
    }
    private void SetEditabilityOnAssistedLivingFields()
    {
        //this.ddlCategory.Enabled = false; to do
        this.ddlProviderType.Enabled = false;
        this.ddlSpecialty.Enabled = false;
        this.SetValidationOnNonGroupMemberProfileFields(true);
        // SetEditabilityOnWaiverServicesFields
        this.trEntityType.Visible = false;
        this.trCategory.Visible = true;
        this.trProviderType.Visible = true;
        this.txtZipCode.Enabled = true;
        this.txtZipCodeExt.Enabled = true;
        // SetValidationOnWaiverServiceOnlyFields
        this.cvPracticeType.Enabled = true;
        this.valAppNbrRqd.Enabled = true;
        this.valApplicationNbr.Enabled = true;
        // SetVisibilityOnWaiverServicesFields
        this.trNPI.Visible = true;
        this.trTaxonomy.Visible = true;
        this.trSpecialty.Visible = true;
        this.trReferral.Visible = true;
    }
    private void SetAssistedLivingDefaults()
    {
        txtCategoryID.Text = CON.ProviderCategoryTypeID.EntityFacility.ToString();
        this.SetCategoryDependentFields();
        // Set provider type
        string providerTypeName = this.SelectAppSetting("NursingHomeProviderTypeName");
        if (!string.IsNullOrEmpty(providerTypeName))
        {
            ddlProviderType.SelectedValue = FindDropDownValueWithoutId(ddlProviderType, providerTypeName);
        }
        this.GetProviderTypeDependentFields();

        // Set specialty
        string providerSpecialtyName = SelectAppSetting("AssistedLivingServicesSpecialtyName");
        if (!string.IsNullOrEmpty(providerSpecialtyName))
        {
            ddlSpecialty.SelectedValue = FindDropDownValueWithoutId(ddlSpecialty, providerTypeName);
        }
        
        this.GetSpecialtyDependentFields();
        this.SetEditabilityOnAssistedLivingFields();
    }
    private void ResetFieldEditability()
    {
        //this.ddlCategory.Enabled = true; to do
        this.ddlProviderType.Enabled = true;
        this.ddlSpecialty.Enabled = true;
        this.ddlTaxonomy.Enabled = true;
        this.txtNPI.Enabled = true;
        this.txtNPI.CssClass = "formField";
    }
    private void LockDownKeyIdentifierFields()
    {
        //this.ddlCategory.Enabled = false; to do
        this.ddlProviderType.Enabled = false;
        this.ddlSpecialty.Enabled = false;
        this.txtMedicaidID.Enabled = string.IsNullOrEmpty(this.txtMedicaidID.Text.Trim()) ? true : false;
        this.txtMedicaidID.CssClass = this.txtMedicaidID.Enabled ? "formField" : "formFieldReadOnly";
        //Some providers will come over without a valid taxonomy, i.e. Not Defined.
        this.ddlTaxonomy.Enabled = this.ddlTaxonomy.SelectedIndex > 0 ? false : true;
        //Some providers will be coming over with null NPI values, however their provider type might now require an NPI.
        this.txtNPI.Enabled = string.IsNullOrEmpty(this.txtNPI.Text.Trim()) ? true : false;
        this.txtNPI.CssClass = this.txtNPI.Enabled ? "formField" : "formFieldReadOnly";
        this.rblTaxIDType.Enabled = false;
    }
    private void SetDisplayForNPIEdit()
    {
        if (this.KeyFieldEditRequest)
        {
            //These values should already be set when editing key fields
            this.ddlProviderType.Enabled = false;
            this.ddlSpecialty.Enabled = false;
            this.ddlTaxonomy.Enabled = false;
            this.rblTaxIDType.Enabled = false;
        }
        this.txtNPI.Enabled = string.IsNullOrWhiteSpace(this.txtNPI.Text) ? true : false;
        this.txtNPI.CssClass = this.txtNPI.Enabled ? "formField" : "formFieldReadOnly";
        //Links that conversion and revalidation should have access to
        this.lnkEditNPI.Visible = !this.txtNPI.Enabled;
        this.trNPI.Visible = true;  //could be hidden for some waiver services that need to edit npi
        //Always disable for now
        this.lnkEditEndDate.Visible = false;
        EditingNPI = false;
    }
    private void SetDisplayForAdminKeyFieldEdit()
    {
        //key fields that can be edited.
        this.ddlProviderType.Enabled = false;
        this.ddlSpecialty.Enabled = false;
        this.ddlTaxonomy.Enabled = true;
        
        if (Convert.ToInt32(rblTaxIDType.SelectedValue) > 0)
        {
            this.rblTaxIDType.Enabled = false;
        }
        this.txtTaxID.Enabled = true;
        this.txtNPI.Enabled = true;
        this.txtFirstName.Enabled = true;
        this.txtLastName.Enabled = true;
        this.txtProviderName.Enabled = true;
        //Only conversion/revalidation/update should have access to
        this.lnkEditNPI.Visible = false;
        this.lnkEditEndDate.Visible = false;
        this.txtTaxID.CssClass = this.txtTaxID.CssClass.Replace("formFieldReadOnly", "");
        this.rblEntityType.Enabled = false;
        //this.ddlCategory.Enabled = false;
        this.txtApplicationNumber.Enabled = false;
        this.txtApplicationNumber.CssClass = this.txtApplicationNumber.Enabled ? "formField" : "formFieldReadOnly";
        //OHPNM-763 pschwarz 10/30/2020: flipped to enabled for the admin field edit (otherwise save errors)
        this.txtZipCode.Enabled = true;
        this.txtZipCodeExt.Enabled = true;
        //hide
        /*
        this.trRED.Visible = false;
        this.trGender.Visible = false;
        this.trDOB.Visible = false;
        this.trMedicaid.Visible = false;
        */
    }
    private void SetDisplayForKeyFieldEdit()
    {
        //key fields that can be edited.
        if (this.KeyFieldEditRequest)
        {
            if (this.ddlProviderType.SelectedValue != "0")
                this.ddlProviderType.Enabled = false;
            if (!string.IsNullOrEmpty(this.txtNPI.Text.Trim()))
                this.txtNPI.Enabled = false;
            if (!string.IsNullOrEmpty(this.txtZipCode.Text.Trim()))
                txtZipCode.Enabled = false;
            if (!string.IsNullOrEmpty(this.txtZipCodeExt.Text.Trim()))
                txtZipCodeExt.Enabled = false;
            if (!string.IsNullOrEmpty(this.txtBirthDate.Text.Trim()))
                txtBirthDate.Enabled = false;
            // OHPNM-8783
            this.ddlGender.Enabled = true;
            this.rblTaxIDType.Enabled = false;
        }
        else
        {
            this.ddlProviderType.Enabled = true;
            this.txtNPI.Enabled = true;
            this.rblTaxIDType.Enabled = true;
            this.txtZipCode.Enabled = true;
            this.txtZipCodeExt.Enabled = true;
            this.ddlGender.Enabled = true;
            txtBirthDate.Enabled = true;
        }

        this.ddlSpecialty.Enabled = true;
        this.ddlTaxonomy.Enabled = true;
        
        
        //Only conversion/revalidation/update should have access to
        this.lnkEditNPI.Visible = false;
        this.lnkEditEndDate.Visible = false;
        this.rblEntityType.Enabled = false;
        //this.ddlCategory.Enabled = false;to do
        this.txtApplicationNumber.Enabled = false;
        this.txtApplicationNumber.CssClass = this.txtApplicationNumber.Enabled ? "formField" : "formFieldReadOnly";
        
        //hide
        /*
        this.trRED.Visible = false;
        this.trGender.Visible = false;
        this.trDOB.Visible = false;
        this.trMedicaid.Visible = false;
        */
    }
    private void SetDisplayForAddODMorODAMedicaid()
    {
        txtCategory.Enabled = false;
        txtCategoryID.Enabled = false;
        txtFirstName.Enabled = false;
        txtLastName.Enabled = false;
        txtMI.Enabled = false;
        txtBirthDate.Enabled = false;
        txtDDContractNumber.Enabled = false;
        txtProviderName.Enabled = false;
        txtTaxID.Enabled = false;
        ddlGender.Enabled = true;
        rblTaxIDType.Enabled = false;
        if (!string.IsNullOrEmpty(txtZipCode.Text))
        {
            txtZipCode.Enabled = false;
            txtZipCodeExt.Enabled = false;
        }
    }
    private string SelectAppSetting(string appSettingKey)
    {
        return AppSettings.Get(appSettingKey, string.Empty);
    }
    private void SetEditabilityOnGroupMemberProfileFields(bool isEditable)
    {
        //this.ddlCategory.Enabled = isEditable;to do
        this.txtEffectiveDate.Enabled = isEditable;
    }
    private void SetEditabilityOnWaiverServicesFields(bool isEditable)
    {
        this.trEntityType.Visible = !isEditable; //only visible for waiver services
        this.trCategory.Visible = isEditable;
        this.trProviderType.Visible = isEditable;
        //this.ddlCategory.Enabled = isEditable;to do
        this.ddlProviderType.Enabled = isEditable;
        this.txtZipCode.Enabled = isEditable;
        this.txtZipCodeExt.Enabled = isEditable;
        //enabled editing if 0000 or null for bug 6453
        if (string.IsNullOrEmpty(this.txtZipCodeExt.Text.Trim()) || this.txtZipCodeExt.Text.Trim() == "0000")
        {
            this.txtZipCodeExt.Enabled = true;
        }
    }
    private void SetVisibilityOnWaiverServicesFields(bool isVisible)
    {
        this.trNPI.Visible = !isVisible; //hide
        this.trTaxonomy.Visible = !isVisible; //hide
        this.trSpecialty.Visible = !isVisible; //hide
        this.trReferral.Visible = isVisible; //show for referral
    }
    private void SetVisibilityByProviderCategory(int categoryID)
    {
        switch (categoryID)
        {
            case CON.ProviderCategoryTypeID.Individual:
            case CON.ProviderCategoryTypeID.GroupMemberProfile:
                this.SetVisibilityOnIndividualOnlyFields(true);
                this.SetVisibilityOnGroupOnlyFields(false);
                this.SetValidationOnIndividualOnlyFields(true);
                this.SetValidationOnGroupOnlyFields(false);
                break;
            case CON.ProviderCategoryTypeID.Pharmacy:
                this.SetVisibilityOnIndividualOnlyFields(false);
                this.SetVisibilityOnGroupOnlyFields(true);
                this.SetValidationOnIndividualOnlyFields(false);
                this.SetValidationOnGroupOnlyFields(true);
                break;
            default:
                this.SetVisibilityOnIndividualOnlyFields(false);
                this.SetVisibilityOnGroupOnlyFields(true);
                this.SetValidationOnIndividualOnlyFields(false);
                this.SetValidationOnGroupOnlyFields(true);
                break;
        }
    }
    private void SetVisibilityByProviderType()
    {
        var providertype = ddlProviderType.SelectedItem.Text.Trim();
        var providertypeName = providertype.Substring(providertype.IndexOf('-') + 1);
       
        switch (providertypeName.Trim())
        {
            case CON.ProviderType.HOME_AND_COMMUNITY_BASED_ODA_ASSISTED_LIVING:
            case CON.ProviderType.Non_Agency_Home_Care_Attendant:
            case CON.ProviderType.Non_Agency_Personal_Care_Aide:
            case CON.ProviderType.OTHER_ACCREDITED_HOME_HEALTH_AGENCY:
            case CON.ProviderType.Private_Duty_Nurse:
            case CON.ProviderType.WAIVERED_SERVICES_ORGANIZATION:
            case CON.ProviderType.Waivered_Services_Individual:
                chkRetro.Visible = false;
                chkRetroCoverage.Visible = false;
                retroHelpLink.Visible = false;
                trEPID.Visible = false;
                trEPName.Visible = false;
                trEPNPI.Visible = false;
                break;
            case CON.ProviderType.NURSING_FACILITY:
            case CON.ProviderType.NON_STATE_OPERATED_ICF_MR:
                int appID = ApplicationTypeID;
                if (appID == 3)
                {
                    trEPID.Visible = true;
                    trEPName.Visible = true;
                    trEPNPI.Visible = true;
                    txtExistingProviderName.ReadOnly = true;
                    txtExistingProviderNPI.ReadOnly = true;
                }
                if (this.KeyFieldEditRequest || this.AdminKeyFieldEditRequest)
                {
                    //OHPNM - 13232
                    trEPID.Visible = false;
                    trEPName.Visible = false;
                    trEPNPI.Visible = false;
                }
                break;
            case CON.ProviderType.UNPAID_SUPPORT_BROKER:
                trGender.Visible = false;
                this.cvddlGender.Enabled = false;
                break;
            default:
                if (ApplicationTypeID != CON.ApplicationType.Waiver && ApplicationTypeID != CON.ApplicationType.ChangeOfOperator && ApplicationTypeID != CON.ApplicationType.ORP)
                {
                    chkRetro.Visible = true;
                    chkRetroCoverage.Visible = true;
                    retroHelpLink.Visible = true;
                }
                trEPID.Visible = false;
                trEPName.Visible = false;
                trEPNPI.Visible = false;
                break;
                //Set after load/selection of provider type
        }
    }
    private void SetEditabilityByProviderType()
    {
        //Set after load/selection of provider type     
    }
    private void SetVisibilityOnLocationFields(bool isVisible)
    {
        this.trZipCode.Visible = isVisible;
        this.trZipExt.Visible = isVisible;
    }
    private void SetVisibilityOnIndividualOnlyFields(bool isVisible)
    {
        this.trFN.Visible = isVisible;
        this.trMI.Visible = isVisible;
        this.trLN.Visible = isVisible;
        this.trGender.Visible = isVisible;
        this.trDOB.Visible = isVisible;
        if (WaiverTypeID == 2 || WaiverTypeID == 3)
        {
            this.divNursingLicense.Visible = isVisible;
        }
        else
        {
            this.divNursingLicense.Visible = false;
        }
        //if (WaiverTypeID == 4)
        //{
        //    this.trTaxonomy.Visible = isVisible;
        //    this.trZipCode.Visible = isVisible;
        //    this.trNPI.Visible = isVisible;
        //    this.trZipExt.Visible = isVisible;
        //}
    }
    private void SetVisibilityOnGroupOnlyFields(bool isVisible)
    {
        this.trOrgName.Visible = isVisible;
        this.trOrgNameHint.Visible = isVisible;
    }
    private void SetValidationOnIndividualOnlyFields(bool enabled)
    {
        this.cvPracticeType.Enabled = enabled;
        this.valFNReqd.Enabled = enabled;
        this.valLNReqd.Enabled = enabled;
        //Tax ID Type has only 2 values and one is defaulted to turned on on load, so no validation.
        this.cvddlGender.Enabled = enabled;
        this.cvDOBFormat.Enabled = enabled;
        this.valDOBReqd.Enabled = enabled;
        //if (WaiverTypeID == 4)
        //{
        //    this.valTaxonmyCmp.Enabled = enabled;
        //    this.valZipReqd.Enabled = enabled;
        //    this.valZipFormat.Enabled = enabled;
        //    this.valZipExtRqd.Enabled = enabled;
        //    this.valZipExtFormat.Enabled = enabled;
        //    this.valNPI.Enabled = enabled;
        //}
    }
    private void SetValidationOnGroupOnlyFields(bool enabled)
    {
        this.valNameReqd.Enabled = enabled;
    }
    private void SetValidationOnWaiverServiceOnlyFields(bool enabled)
    {
        this.cvPracticeType.Enabled = !enabled;
        this.valAppNbrRqd.Enabled = enabled;
        this.valApplicationNbr.Enabled = enabled;
    }
    private void SetValidationOnNonGroupMemberProfileFields(bool enabled)
    {
        this.valZipExtRqd.Enabled = enabled;
        this.valZipExtFormat.Enabled = enabled;
        this.valZipReqd.Enabled = enabled;
        this.valZipFormat.Enabled = enabled;
    }
    private void SetCategoryDefault()
    {
        //if (this.ddlCategory.Items.Count == 0)
        //    return;
        //hide: npi, default then hide taxonomy code, default then hide provider category and type, hid type of practice
        switch (WorkflowIDRequested)
        {
            case CON.WorkflowType.GroupMemberProfile:
                this.txtCategoryID.Text = CON.ProviderCategoryTypeID.GroupMemberProfile.ToString();
                this.rblTaxIDType.SelectedValue = CON.TaxIDType.SSN.ToString();
                this.rblTaxIDType.Enabled = false;
                break;
            default:
                break;
        }
    }
    private void SetPracticeTypeDefault()
    {
        if (this.ddlPracticeType.Items.Count > 0)
        {
            this.ddlPracticeType.SelectedIndex = 0;
        }
    }
    private void SetApplicationDependentFields()
    {
        int appID = ApplicationTypeID;
        if (appID > 0)
        {
            this.txtCategoryID.Text = "-1";
            this.txtCategory.Text = "";
            this.ddlProviderType.Items.Clear();
            this.ddlSpecialty.Items.Clear();
            this.ddlTaxonomy.Items.Clear();
            this.ddlPracticeType.Items.Clear();
            Model = this.LoadKeyFields();
            int iProviderType = this.ddlProviderType.SelectedValue == "" ? 0 : Convert.ToInt32(this.ddlProviderType.SelectedValue);
            WorkflowIDRequested = svc.GetWorkflowInstance(this.ApplicationTypeID, Convert.ToInt32(this.txtCategoryID.Text),
              iProviderType, this.ReferralTypeID, false, false, this.ConvertedProvider, false);
            presenter.RequestProviderCategories(WorkflowIDRequested, ApplicationTypeID, WaiverTypeID);
            divNursingLicense.Visible = false;  //akash
            trRED.Visible = true;
            if (appID == CON.ApplicationType.Waiver || appID == CON.ApplicationType.ChangeOfOperator)
            {
                chkRetro.Visible = false;
                chkRetroCoverage.Visible = false;
                retroHelpLink.Visible = false;
            }
            else
            {
                chkRetro.Visible = true;
                chkRetroCoverage.Visible = true;
                retroHelpLink.Visible = true;
            }
        }
        if (appID == CON.ApplicationType.Waiver)
        {
            if (WaiverTypeID == 2 || WaiverTypeID == 3)
            {
                divNursingLicense.Visible = true;
            }
            else
            {
                divNursingLicense.Visible = false;
            }
        }
        if (appID == 5)
        {
            trRED.Visible = false;
        }
        if (appID == CON.ApplicationType.Standard)
        {
            divDDFacilityNumber.Visible = true;
            divDDContractNumber.Visible = true;
        }
        if (WaiverTypeID == 1) // Jira 3041			
        {
            //divDDFacilityNumber.Visible = true;
            divDDContractNumber.Visible = true;
        }
        if (WaiverTypeID == 3 || WaiverTypeID == 4)  // OHPNM-8033 
        {
            divDDContractNumber.Visible = false;
        }
        if (appID == CON.ApplicationType.ChangeOfOperator || WaiverTypeID == 2 || WaiverTypeID == 3 || WaiverTypeID == 4)
        {
            this.div10DayMessage.Visible = false;
        }
    }
    private void SetCategoryDependentFields()
    {
        int categoryID = 0;
        int waiverID = 0;
        if (this.txtCategoryID.Text != "")
            categoryID = Convert.ToInt32(this.txtCategoryID.Text);
        if (this.txtWaiverTypeID.Text != "")
            waiverID = Convert.ToInt32(this.txtWaiverTypeID.Text);
        if (categoryID > 0)
        {
            this.ddlProviderType.Items.Clear();
            this.ddlSpecialty.Items.Clear();
            this.ddlTaxonomy.Items.Clear();
            this.ddlPracticeType.Items.Clear();
            Model = this.LoadKeyFields();
            presenter.RequestCategoryDependentFields(this.ApplicationTypeID, categoryID, "", WorkflowIDRequested, waiverID);

            //this.rblTaxIDType.SelectedIndex = categoryID == 1 ? 1 : 0;
            //OHPNM - 8275
            if (categoryID == 1)
            {
                this.rblTaxIDType.SelectedIndex = 1;
                rblTaxIDType.Enabled = false;
            }
            else
            {
                this.rblTaxIDType.SelectedIndex = 0;
                rblTaxIDType.Enabled = false;
            }

            if (Model.ProviderTypeName == CON.ProviderType.NON_STATE_OPERATED_ICF_MR)
            {
                this.divDDFacilityNumber.Visible = true;
            }
            else
            {
                this.divDDFacilityNumber.Visible = false;
            }
        }
        this.SetVisibilityByProviderCategory(categoryID);
    }
    private void GetProviderTypeDependentFields()
    {
        int categoryID = Helper.ConvertStringToInt32(this.txtCategoryID.Text);
        int providerTypeID = Helper.ConvertStringToInt32(this.ddlProviderType.SelectedValue);
        if (providerTypeID > 0)
        {
            this.ddlSpecialty.Items.Clear();
            this.ddlTaxonomy.Items.Clear();
            this.ddlPracticeType.Items.Clear();
            Model = this.LoadKeyFields();
            presenter.RequestSpecialities(providerTypeID);
            presenter.RequestPracticeTypes(categoryID, providerTypeID);
            presenter.RequestTaxonomies(providerTypeID, 0); /* Don't filter taxonomies on specialty. Just on provider type*/
        }

        this.SetVisibilityByProviderType();
        this.SetEditabilityByProviderType();
    }
    private void GetSpecialtyDependentFields()
    {
        int providerTypeID = Helper.ConvertStringToInt32(this.ddlProviderType.SelectedValue);
        int specialtyID = Helper.ConvertStringToInt32(this.ddlSpecialty.SelectedValue);
        if (specialtyID > 0 && providerTypeID > 0)
        {
            this.ddlTaxonomy.Items.Clear();
            Model = this.LoadKeyFields();
            presenter.RequestTaxonomies(providerTypeID, 0); /* Don't filter taxonomies on specialty. Just on provider type*/
        }
    }
    private void SetTaxIDBasedOnOrganization()
    {
        switch (rblEntityType.SelectedValue)
        {
            case EntityType.Organization:
                //Organizations can have EIN or SSN
                this.rblTaxIDType.Enabled = true;
                break;
            case EntityType.Individual:
                //Individuals can only have SSN
                this.rblTaxIDType.SelectedValue = CON.TaxIDType.SSN.ToString();
                this.rblTaxIDType.Enabled = false;
                break;
            default:
                break;
        }
    }
    private void SetWaiverDropDownValues()
    {        //Field created for Waiver Services - will drive the defaulted values for category and provider type (will only be one provider type).
        //entity type, itself, is not saved anywhere.
        switch (rblEntityType.SelectedValue)
        {
            case EntityType.Organization:
                this.txtCategoryID.Text = CON.ProviderCategoryTypeID.EntityFacility.ToString();
                this.SetCategoryDependentFields();
                break;
            case EntityType.Individual:
                this.txtCategoryID.Text = CON.ProviderCategoryTypeID.Individual.ToString();
                this.SetCategoryDependentFields();
                break;
            default:
                break;
        }
        int providerTypeID = 0;
        if (ddlProviderType.Items.Count > 0)
        {
            this.ddlProviderType.SelectedIndex = 1;
            providerTypeID = Convert.ToInt32(this.ddlProviderType.SelectedValue);
            presenter.RequestSpecialities(providerTypeID);
            int categoryID = Convert.ToInt32(this.txtCategoryID.Text);
            presenter.RequestPracticeTypes(categoryID, providerTypeID);
        }
        if (this.ddlSpecialty.Items.Count > 0)
        {
            this.ddlSpecialty.SelectedIndex = 1;
            presenter.RequestTaxonomies(providerTypeID, Convert.ToInt32(this.ddlSpecialty.SelectedValue));
        }
        if (this.ddlTaxonomy.Items.Count > 0)
        {
            this.ddlTaxonomy.SelectedIndex = 1;
        }
    }
    private void SetExistingRegData(ProviderManagementData data)
    {
        this.RegID = data.RegID;
        this.txtApplicationNumber.Text = data.ReferralNumber;
        if (!this.IsAddODMorODAMedicaid)
        {
            this.txtApplicationType.Text = data.ApplicationTypeName;
            this.txtApplicationTypeID.Text = data.ApplicationTypeID.ToString();
            this.txtApplicationTypeID_TextChanged(this.txtApplicationTypeID, null);
            this.txtWaiverType.Text = data.WaiverTypeName;
            this.txtWaiverTypeID.Text = data.WaiverTypeID.ToString();
            this.txtWaiverTypeID_TextChanged(this.txtWaiverTypeID, null);
        }
        this.SpecialtyTypeID = data.SpecialtyTypeID;
        this.TaxonomyTypeID = data.TaxonomyTypeID;
        this.txtProviderName.Text = data.ProviderName.Trim();
        if (!IsLinkProvider) //need to select provider category type for linkprovider
        {
            this.txtCategoryID.Text = data.ProviderCategoryTypeID.ToString();
            txtCategoryID_TextChanged(this.txtCategoryID.Text, null);
        }
        if (ddlProviderType.Items.Count > 0 && !IsLinkProvider && !this.IsAddODMorODAMedicaid) //not showing the provider type for linkprovider
        {
            this.ddlProviderType.SelectedValue = Helper.ValueExistsInDropDown(this.ddlProviderType, data.ProviderTypeID.ToString()) ? data.ProviderTypeID.ToString() : "0";
        }
        if (!ProviderTypeChangeRequest)
            ddlProviderType_SelectedIndexChanged(this.ddlProviderType, null);
        if (this.ddlSpecialty.Items.Count > 0)
        {
            this.ddlSpecialty.SelectedValue = Helper.ValueExistsInDropDown(this.ddlSpecialty, data.SpecialtyTypeID.ToString()) ? data.SpecialtyTypeID.ToString() : "0";
        }
        ddlSpecialty_SelectedIndexChanged(ddlSpecialty, null);

        if (!string.IsNullOrEmpty(data.NPI))
        {
            LoadTaxonomyByNPI(data.NPI);
            this.ddlTaxonomyNPPES.SelectedValue = SetTaxonomyCodeFromTypeId(data.TaxonomyTypeID,data.TaxonomyCode);
        }


        if (this.ddlTaxonomy.Items.Count > 0 && !IsLinkProvider) //not showing the provider type for linkprovider
        {
            this.ddlTaxonomy.SelectedValue = Helper.ValueExistsInDropDown(this.ddlTaxonomy, data.TaxonomyTypeID.ToString()) ? data.TaxonomyTypeID.ToString() : "0";
        }
        this.txtTaxID.Text = data.TaxID.Trim();
        this.txtNPI.Text = data.NPI.Trim();
        txtOldNPI.Text = data.NPI.Trim();
        this.txtZipCode.Text = data.ZipCode.Trim();
        this.txtZipCodeExt.Text = data.ZipExt.Trim();
        txtNPIStartDate.Text = txtNPIEndDate.Text = string.Empty;
        txtOldNPIEndDate.Text = data.NPIEndDate.HasValue ? data.NPIEndDate.Value.ToShortDateString() :
            string.Empty;
        OriginalNPIEndDate = data.NPIEndDate;
        txtOldNPIStartDate.Text = data.NPIStartDate.HasValue ? data.NPIStartDate.Value.ToShortDateString() :
            string.Empty;
        txtFirstName.Text = data.FirstName.Trim();
        txtMI.Text = data.MiddleInitial.Trim();
        txtLastName.Text = data.LastName.Trim();
        txtBirthDate.Text = data.BirthDate.HasValue ? data.BirthDate.Value.ToShortDateString() : string.Empty;
        if (ddlPracticeType.Items.Count > 0 && Helper.ValueExistsInDropDown(this.ddlPracticeType,
            data.TypeOfPracticeID.HasValue ? data.TypeOfPracticeID.Value.ToString() : string.Empty))
        {
            this.ddlPracticeType.SelectedValue = data.TypeOfPracticeID.Value.ToString();
        }
        if (data.Gender.Length == 1)
        {
            this.ddlGender.SelectedValue = data.Gender;
        }
        else
        {
            this.ddlGender.SelectedIndex = -1;
        }
        if (data.TaxIDTypeID > 0)
        {
            this.rblTaxIDType.SelectedIndex = data.TaxIDTypeID == CON.TaxIDType.EIN ? 0 : data.TaxIDTypeID == CON.TaxIDType.SSN ? 1 : 0;
            this.rblTaxIDType.Enabled = false;
        }
        this.txtMedicaidID.Text = data.MedicaidID;
        this.SetVisibilityByProviderCategory(data.ProviderCategoryTypeID);
        if (this.ReferralID > 0)
        {
            if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual)
            {
                this.rblEntityType.SelectedIndex = 0;
            }
            else if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.EntityFacility)
            {
                this.rblEntityType.SelectedIndex = 1;
            }
        }
        this.EndDate = data.RevalidationDate;
        this.txtDDContractNumber.Text = data.DDContractNumber;

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "callJSFunction", "setChangeEditability();", true);

        if (ProviderTypeChangeRequest)
        {
            ddlProviderType.Enabled = true;
            lnkApplicationType.Visible = false;
            lnkCategoryType.Visible = false;

        }
        else
        {
            if (this.KeyFieldEditRequest)
            {
                lnkApplicationType.Visible = false;
                lnkCategoryType.Visible = false;
                if (!string.IsNullOrEmpty(this.txtTaxID.Text.Trim()))
                    txtTaxID.Enabled = false;
                if (!string.IsNullOrEmpty(this.txtZipCode.Text.Trim()))
                    txtZipCode.Enabled = false;
                if (!string.IsNullOrEmpty(this.txtZipCodeExt.Text.Trim()))
                    txtZipCodeExt.Enabled = false;
                if (!string.IsNullOrEmpty(this.txtBirthDate.Text.Trim()))
                    txtBirthDate.Enabled = false;
                if (!string.IsNullOrEmpty(this.txtNPI.Text.Trim()))
                    this.txtNPI.Enabled = false;
                if (this.ddlProviderType.SelectedValue != "0")
                    this.ddlProviderType.Enabled = false;
            }
            else
            {
                lnkApplicationType.Visible = true;
                lnkCategoryType.Visible = true;
                txtTaxID.Enabled = true;
                txtZipCode.Enabled = true;
                txtZipCodeExt.Enabled = true;
                txtBirthDate.Enabled = true;
                this.txtNPI.Enabled = true;
                this.ddlProviderType.Enabled = true;
            }
        }
        
       

        this.WorkflowEventTypeId = data.WorkflowEventType;
        this.setHiddenWorkflowEventTypeId();
    }
    private void BindGender()
    {
        DataSet dsGender = svc.SelectReferenceDataWithoutParam("usp_SelectPROVIDER_GENDER");
        DataTable dtfilteredgender = dsGender.Tables[0].AsEnumerable()
                     .Where(r => r.Field<string>("PROVIDER_GENDER_INITIAL") != "B")
                     .CopyToDataTable();
        Helper.LoadList(ddlGender, dtfilteredgender, "PROVIDER_GENDER_NAME", "PROVIDER_GENDER_INITIAL", true);

    }
    private bool KeyFieldUpdatesRequireDelete()
    {
        bool updateNeeded = false;
        int providerTypeID = ddlProviderType.SelectedIndex > -1 ? Convert.ToInt32(ddlProviderType.SelectedValue) : 0;
        int specialtyTypeID = ddlSpecialty.SelectedIndex > -1 ? Convert.ToInt32(ddlSpecialty.SelectedValue) : 0;
        int taxonomyTypeID = ddlTaxonomy.SelectedIndex > -1 ? Convert.ToInt32(ddlTaxonomy.SelectedValue) : 0;
        int typeOfPracticeID = ddlPracticeType.SelectedIndex > -1 ? Convert.ToInt32(ddlPracticeType.SelectedValue) : -1;
        int taxIDTypeID = Convert.ToInt32(rblTaxIDType.SelectedValue);
        if (providerTypeID != InitialKeyFields.ProviderTypeID
            || specialtyTypeID != InitialKeyFields.SpecialtyTypeID
            || taxIDTypeID != InitialKeyFields.TaxIDTypeID)
        {
            updateNeeded = true;
        }
        return updateNeeded;
    }
    private ProviderManagementData LoadKeyFields()
    {
        ProviderManagementData keyData = new ProviderManagementData();
        keyData.RegID = RegID;
        keyData.WorkflowIDRequested = WorkflowIDRequested;
        keyData.ReferralID = ReferralID;
        keyData.ReferralTypeID = ReferralTypeID;
        keyData.TaxIDTypeID = TaxIDTypeID;
        keyData.ProviderCategoryTypeID = Convert.ToInt32(this.txtCategoryID.Text) > -1 ? Convert.ToInt32(this.txtCategoryID.Text) : 0;
        if (ddlProviderType.Items.Count > 0)
        {
            keyData.ProviderTypeID = ddlProviderType.SelectedIndex > -1 ? Convert.ToInt32(ddlProviderType.SelectedValue) : 0;
            keyData.MMISProviderTypeID = presenter.GetMMISProviderTypeID(keyData.ProviderTypeID);
        }
        if (ddlSpecialty.Items.Count > 0)
        {
            keyData.SpecialtyTypeID = ddlSpecialty.SelectedIndex > -1 ? Convert.ToInt32(ddlSpecialty.SelectedValue) : 0;
        }
        return keyData;
    }
    private void SaveNPIHistory()
    {
        ProviderManagementData pmd = this.Model;
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.Model.RegID.ToString());
        parms.Add("NPI", txtOldNPI.Text);
        if (!string.IsNullOrWhiteSpace(txtOldNPIStartDate.Text))
        {
            DateTime npiStartDate;
            DateTime.TryParse(txtOldNPIStartDate.Text, out npiStartDate);
            parms.Add("NPI_START_DATE", npiStartDate.ToShortDateString());
        }
        if (!string.IsNullOrWhiteSpace(txtOldNPIEndDate.Text))
        {
            DateTime npiEndDate;
            DateTime.TryParse(txtOldNPIEndDate.Text, out npiEndDate);
            parms.Add("NPI_END_DATE", npiEndDate.ToShortDateString());
        }
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
        parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        svc.InsertRegistrationDataTable("NPI_HISTORY", parms);
    }
    private bool HasNPIWaitingForMMIS()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(Model.RegID, "NPI_HISTORY");
        if (!Helper.HasRows(ds))
            return false;
        else
        {
            var nullMMISStatus = ds.Tables[0].AsEnumerable().Where(s => s.Field<string>("MMIS_STATUS") == null);
            if (nullMMISStatus == null || nullMMISStatus.CopyToDataTable().Rows.Count == 0)
                return false;
            else
                return true;
        }
    }
    private void removeIndividualSoloCategoryFromDDL()
    {
    }
    #endregion
    protected void chkRetro_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRetro.Checked)
        {
            txtEffectiveDate.Enabled = true;
        }
        else
        {
            txtEffectiveDate.Enabled = false;
        }
    }
    protected void txtWaiverTypeID_TextChanged(object sender, EventArgs e)
    {
        if(!string.IsNullOrEmpty(txtWaiverTypeID.Text))
            WaiverTypeID = Convert.ToInt32(txtWaiverTypeID.Text);
        string desc = "";
        DataRow[] selectedRows = dtWaiverTypes.Select("WAIVER_TYPE_ID = " + WaiverTypeID.ToString());
        if (selectedRows.Count() > 0)
        {
            desc = selectedRows[0]["WAIVER_TYPE_NAME"].ToString();
            //hdnIsWaiver.Value = selectedRows[0]["IsWaiver"].ToString();
            //TODO need to change it to be better functionality currently doing for DEMO AT
            //if (hdnIsWaiver.Value != "True")
            //{
            //    if (Convert.ToInt32(selectedRows[0]["Application_Type_ID"].ToString()) > 7)
            //    hdnIsWaiver.Value = "More";
            //}
        }
        txtWaiverType.Text = desc;
        this.SetApplicationDependentFields();
        if (this.IsAddODMorODAMedicaid)
        {
            ProviderManagementData data = new ProviderManagementData();
            DataSet ds = svc.SelectRegistrationByRegID(this.RegID);
            data.LoadObjectFromDataset(ds);
            SetExistingProviderData(data);
            SetDisplayForAddODMorODAMedicaid();
        }
    }
    protected void rptApplication_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HtmlGenericControl lbl = (HtmlGenericControl)e.Item.FindControl("spn");
        HtmlGenericControl spninfo = (HtmlGenericControl)e.Item.FindControl("spninfo");
        HtmlGenericControl divrptAppItem = (HtmlGenericControl)e.Item.FindControl("divrptAppItem");
        HtmlGenericControl divHeading = (HtmlGenericControl)e.Item.FindControl("divHeading");
        HtmlGenericControl divFooter = (HtmlGenericControl)e.Item.FindControl("divFooter");
        HtmlButton SelectAppType = (HtmlButton)e.Item.FindControl("SelectAppType");
        string appTypeID = lbl.Attributes["data-apptype"];
        if (lbl.Attributes["data-apptype"] == CON.ApplicationType.MCP.ToString())
        {
            lbl.InnerHtml = "Single Case agreement definition: out-of-network provider, including emergency care provider, who provides Medicaid-covered services for a specific plan member or outside the plan’s network on a limited basis when the provider is unwilling to enroll";
        }
        else
            spninfo.Visible = false;
        if (this.IsAddODMorODAMedicaid)
        {
            if (lbl.Attributes["data-apptype"] != CON.ApplicationType.Standard.ToString())
            {
                divrptAppItem.Attributes.Remove("class");
                divrptAppItem.Attributes.Add("class", "panel panel-info");
                divHeading.Style.Add("background-color", "grey");
                SelectAppType.Attributes.Add("disabled", "disabled");
                SelectAppType.Attributes.Remove("class");
                SelectAppType.Attributes.Add("class", "panelButton1");
                divFooter.Style.Add("background-color", "grey");
                ////pnlrptAppItem.Visible = false;
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "callJSFunction", "DisableApplicationTypeTile('" + appTypeID + "');", true);
            }
        }
    }
    protected void rptApplicationWaiver_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HtmlGenericControl lbl = (HtmlGenericControl)e.Item.FindControl("spnW");
        
        HtmlGenericControl divrptAppItem = (HtmlGenericControl)e.Item.FindControl("divrptWaiver");
        HtmlGenericControl divHeading = (HtmlGenericControl)e.Item.FindControl("divHeading1");
        HtmlGenericControl divFooter = (HtmlGenericControl)e.Item.FindControl("divfooter1");
        HtmlButton SelectAppType = (HtmlButton)e.Item.FindControl("SelectAppType1");
        string waiverTypeID = lbl.Attributes["data-apptype"];

        if (this.IsAddODMorODAMedicaid)
        {
            if (waiverTypeID == CON.WaiverApplicationTypeID.DODD.ToString() || waiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD.ToString())
            {
                divrptAppItem.Attributes.Remove("class");
                divrptAppItem.Attributes.Add("class", "panel panel-info");
                divHeading.Style.Add("background-color", "grey");
                SelectAppType.Attributes.Add("disabled", "disabled");
                SelectAppType.Attributes.Remove("class");
                SelectAppType.Attributes.Add("class", "panelButton1");
                divFooter.Style.Add("background-color", "grey");
                ////pnlrptAppItem.Visible = false;
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "callJSFunction", "DisableApplicationTypeTile('" + appTypeID + "');", true);
            }
        }
    }
    protected void rptCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HtmlGenericControl lbl = (HtmlGenericControl)e.Item.FindControl("spnProviderCategory");
        HtmlButton button = (HtmlButton)e.Item.FindControl("btnCat");
        if (WaiverTypeID == 4)
        {
            if (lbl.Attributes["data-apptype"] == CON.ProviderCategoryTypeID.EntityFacility.ToString())
            {
                button.Attributes.Add("title", "Agency provider means an entity, including a county board, that directly employs at least one person in addition to the chief executive officer for the purpose of providing services for which the entity must be certified.For licensure an agency is simply an entity, including a county board, that is the licensee or operator of a certified waiver home or ICF.");
            }
            if (lbl.Attributes["data-apptype"] == CON.ProviderCategoryTypeID.Individual.ToString())
            {
                button.Attributes.Add("title", "Independent or Individual provider means a self - employed person who provides services for which he or she must be certified for and does not employ, either directly or through contract, anyone else to provide the services.For Licensure this would be the self employed person who is the licensee of a certified waiver home or ICF.");
            }
            trZipCode.Visible = false;
            trZipExt.Visible = false;
            trNPI.Visible = false;
            trTaxonomyNPPES.Visible = false;
        }
        if (ApplicationTypeID == CON.ApplicationType.Waiver)
        {
            if (lbl.Attributes["data-apptype"] == CON.ProviderCategoryTypeID.Organization.ToString())
            {
                button.Attributes.Add("title", "Agency provider means an entity, including a county board, that directly employs at least one person in addition to the chief executive officer for the purpose of providing services for which the entity must be certified.");
            }
            if (lbl.Attributes["data-apptype"] == CON.ProviderCategoryTypeID.Individual.ToString())
            {
                button.Attributes.Add("title", "'Independent or Individual provider' means a self-employed person who provides services for which he or she must be certified for and does not employ, either directly or through contract, anyone else to provide the services.");
            }
        }
        else
        {
            if (lbl.Attributes["data-apptype"] == CON.ProviderCategoryTypeID.Individual.ToString())
            {
                button.Attributes.Add("title", "'Independent or Individual provider' means a self-employed person who provides services for which he or she must be certified for and does not employ, either directly or through contract, anyone else to provide the services.");
            }
        }
    }
   
    protected void txtProviderMedicID_TextChangedMedicID(object sender, EventArgs e)
    {
        SetExitingProviderData();
    }

    private string FindDropDownValueWithoutId(DropDownList ddl, string text)
    {
        string retVal = string.Empty;

        if (!string.IsNullOrEmpty(text))
        {
            foreach (ListItem item in ddl.Items)
            {
                string name = item.Text.Trim();
                if (name.IndexOf('-') > 0)
                {
                    name = name.Substring(name.IndexOf('-') + 1);
                }
                if (name.Equals(text, StringComparison.OrdinalIgnoreCase)){
                    retVal = item.Value;
                }
            }
        }

        return retVal;
    }

    protected void rblTaxIDType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblTaxIDType.SelectedValue == "15")
        {
            reTaxIDEIN.Enabled = false;
            reTaxID.Enabled = true;
        }
        else
        {
            reTaxIDEIN.Enabled = true;
            reTaxID.Enabled = false;
        }
    }
	
    private void setHiddenWorkflowEventTypeId()
    {
        // store on page, so Javascipt has easy access to it
        this.workflowEventTypeId_hidden.Value = this.WorkflowEventTypeId.ToString();
    }
    protected void txtNPI_TextChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtNPI.Text) && ApplicationTypeID == CON.ApplicationType.Internal)
        {
            ClearErrorMessages();
            vsNewProvider.Visible = false; //validation summary
            trTaxonomyNPPES.Visible = false; // taxonomy div
        }
    }
}
