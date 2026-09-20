using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Data;
using System.Web;
using System.Web.UI;

public partial class PopupControls_HealthCareAffiliations : BasePopupControl, IHealthCareAffiliationsView
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

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
    public int RegID
    {
        get
        {
            return ViewState["RegID"] == null ? 0 : Convert.ToInt32(ViewState["RegID"]);
        }
        set
        {
            ViewState["RegID"] = value;
        }
    }

    public int RegHealthCareID
    {
        get
        {
            return ViewState["RegHealthCareID"] == null ? 0 : Convert.ToInt32(ViewState["RegHealthCareID"]);
        }
        set
        {
            ViewState["RegHealthCareID"] = value;
        }
    }
    #region Parent Page Events

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;

    public delegate void KeepOpenEventHandler();
    public event KeepOpenEventHandler KeepOpenEvent;

    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    #endregion
    private HealthCareAffiliationsPresenter _presenter;

    public HealthCareAffiliationsPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new HealthCareAffiliationsPresenter(this);
            }

            return _presenter;
        }
    }

    public HealthCareAffiliations Model { get; set; }


    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        rblRestrictedPrivileges.Items[0].Attributes.Add("onclick", "javascript:ToggleVisible('" + txtResponseComment.ClientID + "','Yes');");
        rblRestrictedPrivileges.Items[1].Attributes.Add("onclick", "javascript:ToggleVisible('" + txtResponseComment.ClientID + "','No');");
        txtHospitalPrivilegesReason.Attributes["maxlength"] = "150";
        txtResponseComment.Attributes["maxlength"] = "150";
        LoadAffiliationPrivilegesStatusDefs();
        LoadAffiliationStaffStatusDefs();
        chkIsPrimaryFacility.InputAttributes.Add("aria-label", "This is my Primary Facility");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        this.ValidateData();
        if (rblHospitalPrivileges.SelectedValue == "True")
        {
            Page.Validate("HealthCareAffiliations");
        }

        if (rblHospitalPrivileges.SelectedValue == "True")
        {
            Page.Validate("HealthCareAffiliations");
        }

        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            return;
        }
        HealthCareAffiliations healthCareAff = new HealthCareAffiliations();
        if (RegHealthCareID != 0)
            healthCareAff.RegHealthCareID = RegHealthCareID;

        healthCareAff.RegID = this.WorkflowPage.RegistrationId;
        healthCareAff.FacilityName = txtFacilityName.Text;
        healthCareAff.FacilityMedicaidID = txtMedicaidIDSearch.Text;
        healthCareAff.IsPrimaryFacility = chkIsPrimaryFacility.Checked;
        healthCareAff.StaffCategory = ddlAffiliationPrivilegesStatus.SelectedValue;
        healthCareAff.StatusOfPrivileges = ddlStaffCategoryID.SelectedValue;
        if (!string.IsNullOrEmpty(txtStart_Date.Text))
            healthCareAff.StartDate = Convert.ToDateTime(txtStart_Date.Text);
        else
            healthCareAff.StartDate = DateTime.MinValue;
        if (!string.IsNullOrEmpty(txtEndDate.Text))
            healthCareAff.EndDate = Convert.ToDateTime(txtEndDate.Text);
        else
            healthCareAff.EndDate = DateTime.MinValue;
        if (rblRestrictedPrivileges.SelectedIndex >= 0)
            healthCareAff.IsRestrictedPrivilege = Convert.ToBoolean(rblRestrictedPrivileges.SelectedValue);
        healthCareAff.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        healthCareAff.Reason = txtResponseComment.Text;
        healthCareAff.HospitalPrivilegesReason = txtHospitalPrivilegesReason.Text;
        if (rblInpatientSetting.SelectedIndex >= 0)
            healthCareAff.IsInpatientSetting = Convert.ToBoolean(rblInpatientSetting.SelectedValue);
        if (rblHospitalPrivileges.SelectedIndex >= 0)
            healthCareAff.IsHospitalPrivileges = Convert.ToBoolean(rblHospitalPrivileges.SelectedValue);

        Model = healthCareAff;
        presenter.SaveRegHealthCareAffiliation(healthCareAff);

        if (SaveEvent != null)
        {
            SaveEvent();
        }




    }

    private void ValidateData()
    {

    }


    private DataSet GetPrivilageList()
    {
        DataSet ds = svc.SelectIndividualAffiliationPrivilegesStatus();
        return ds;
    }

    private DataSet GetStaffCategoryList()
    {
        DataSet ds = svc.SelectIndividualAffiliationStaffCategoryStatus();
        return ds;
    }

    private void LoadAffiliationPrivilegesStatusDefs()
    {

        this.ddlStaffCategoryID.DataSource = GetPrivilageList();
        this.ddlStaffCategoryID.DataTextField = "HOSPITAL_PRIVILEGES_DESC";
        this.ddlStaffCategoryID.DataValueField = "HOSPITAL_PRIVILEGES_DESC";
        this.ddlStaffCategoryID.DataBind();
        this.ddlStaffCategoryID.Items.Insert(0, "");
        this.ddlStaffCategoryID.SelectedIndex = 0;
    }


    private void LoadAffiliationStaffStatusDefs()
    {

        this.ddlAffiliationPrivilegesStatus.DataSource = GetStaffCategoryList();
        this.ddlAffiliationPrivilegesStatus.DataTextField = "HOSPITAL_STAFF_PRIVILEGES_DESC";
        this.ddlAffiliationPrivilegesStatus.DataValueField = "HOSPITAL_STAFF_PRIVILEGES_DESC";
        this.ddlAffiliationPrivilegesStatus.DataBind();
        this.ddlAffiliationPrivilegesStatus.Items.Insert(0, "");
        this.ddlAffiliationPrivilegesStatus.SelectedIndex = 0;
    }


    protected void btnClose_Click(object sender, EventArgs e)
    {


    }


    #endregion

    #region Public Methods

    public void LoadData(int regHealthCareAffiliationID)
    {

        if (regHealthCareAffiliationID == 0) EnableAllFields(true);
        else
        {
            RegHealthCareID = regHealthCareAffiliationID;
            DataSet ds = presenter.GetHealthCareAffiliationByID(regHealthCareAffiliationID);
            if (Helper.HasRows(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtFacilityName.Text = Helper.GetString("FacilityName", dr);
                txtMedicaidIDSearch.Text = Helper.GetString("FacilityMedicaidID", dr);
                if (ddlStaffCategoryID.Items.FindByValue(Helper.GetString("StatusOfPrivileges", dr)) != null)
                {
                    ddlStaffCategoryID.ClearSelection();
                    ddlStaffCategoryID.Items.FindByValue(Helper.GetString("StatusOfPrivileges", dr)).Selected = true;
                }
                if (ddlAffiliationPrivilegesStatus.Items.FindByValue(Helper.GetString("StaffCategory", dr)) != null)
                {
                    ddlAffiliationPrivilegesStatus.ClearSelection();
                    ddlAffiliationPrivilegesStatus.Items.FindByValue(Helper.GetString("StaffCategory", dr)).Selected = true;
                }
                else
                {
                    ddlAffiliationPrivilegesStatus.SelectedIndex = 0;
                }
                txtStart_Date.Text = Helper.GetDate("StartDate", dr);
                txtEndDate.Text = Helper.GetDate("EndDate", dr);
                chkIsPrimaryFacility.Checked = Helper.GetBool("Is_Primary_Facility", dr);

                txtResponseComment.Text = Helper.GetString("Reason", dr);
                if (string.IsNullOrEmpty(Helper.GetString("IsRestrictedPrivilege", dr)))
                {
                    rblRestrictedPrivileges.SelectedValue = "False";
                }
                else
                {
                    rblRestrictedPrivileges.SelectedValue = Helper.GetString("IsRestrictedPrivilege", dr);
                }
                if (string.IsNullOrEmpty(Helper.GetString("IsInpatientSetting", dr)))
                {
                    rblInpatientSetting.SelectedValue = "False";
                }
                else
                {
                    rblInpatientSetting.SelectedValue = Helper.GetString("IsInpatientSetting", dr);
                }
                if (string.IsNullOrEmpty(Helper.GetString("IsHospitalPrivileges", dr)))
                {
                    rblHospitalPrivileges.SelectedValue = "False";
                }
                else
                {
                    rblHospitalPrivileges.SelectedValue = Helper.GetString("IsHospitalPrivileges", dr);
                }               
                txtHospitalPrivilegesReason.Text = Helper.GetString("HospitalPrivilegesReason", dr);
            }
        }
    }
    public void DeleteRegHealthCareAffiliation(int regPendingAffiliationID)
    {
        if (regPendingAffiliationID > 0)
            presenter.DeleteHealthCareAffiliationByID(regPendingAffiliationID);
    }
    #endregion





    public void InitializeFields()
    {
        chkIsPrimaryFacility.Checked = false;
        txtFacilityName.Text = string.Empty;
        txtResponseComment.Text = "";
        ddlStaffCategoryID.SelectedIndex = 0;
        txtStart_Date.Text = "";
        txtMedicaidIDSearch.Text = string.Empty;
        ddlAffiliationPrivilegesStatus.SelectedIndex = 0;
        rblRestrictedPrivileges.SelectedIndex = 1;
        rblInpatientSetting.SelectedIndex = 1;
        rblHospitalPrivileges.SelectedIndex = 1;
        txtHospitalPrivilegesReason.Text = "";
        RegHealthCareID = 0;
        lblErrorMessages.Text = string.Empty;

    }

    private void EnableAllFields(bool enable)
    {

    }






    private bool SaveData()
    {
        bool rtn = true;


        return rtn;
    }

    private void InitFormFields()
    {
        this.txtFacilityName.Text = this.txtResponseComment.Text = this.txtHospitalPrivilegesReason.Text = string.Empty;
        ddlStaffCategoryID.SelectedIndex = 0;
        ddlAffiliationPrivilegesStatus.SelectedIndex = 0;


    }

    private void SetFormFields(DataSet affiliationData)
    {
        /*DataRow affiliationRow = affiliationData.Tables[0].Rows[0];

        //this.RegAffiliationID = Helper.GetInt("REG_AFFILIATION_ID", affiliationRow);

        this.txtGroupName.Text = Helper.GetString("FIRST_NAME", affiliationRow);
        this.txtMedicaidID.Text = Helper.GetString("LAST_NAME", affiliationRow);
        txtTaxID.Text = Helper.GetString("SSN", affiliationRow);
        txtNPI.Text = Helper.GetString("NPI", affiliationRow);*/

    }

    public void GetRegHealthCareFacilityAffiliation(DataSet ds)
    {
        //;
    }

    protected void rblRestrictedPrivileges_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblRestrictedPrivileges.SelectedValue.Equals("True"))
        {
            rfvResponseComment.Enabled = true;
        }
        else
        {
            rfvResponseComment.Enabled = false;
        }
    }

    protected void rblHospitalPrivileges_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblHospitalPrivileges.SelectedValue.Equals("False"))
        {
            rfvrfvHospitalPrivileges.Enabled = true;
        }
        else
        {
            rfvrfvHospitalPrivileges.Enabled = false;
        }
    }

    protected void txtMedicaidIDSearch_TextChanged(object sender, EventArgs e)
    {
        DataSet ds = presenter.GetHospitalFacilityByMedicaidID(txtMedicaidIDSearch.Text);
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtFacilityName.Text = Helper.GetString("Name", dr);
            customValidator1.IsValid = true;
        }
        else
        {
            customValidator1.IsValid = false;
        }

    }
}