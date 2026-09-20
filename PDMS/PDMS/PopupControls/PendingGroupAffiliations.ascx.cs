using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using MAXIMUS.Core.Libraries;

public partial class PopupControls_PendingGroupAffiliations : BasePopupControl, IPendingGroupAffiliationsView 
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

    public int RegPendingAffiliationID
    {
        get
        {
            return ViewState["RegPendingAffiliationID"] == null ? 0 : Convert.ToInt32(ViewState["RegPendingAffiliationID"]);
        }
        set
        {
            ViewState["RegPendingAffiliationID"] = value;
        }
    }

    public string MedicaidID
    {
        get
        {
            return ViewState["MedicaidID"] == null ? "" : Convert.ToString(ViewState["MedicaidID"]);
        }
        set
        {
            ViewState["MedicaidID"] = value;
        }
    }
    public string GroupName
    {
        get
        {
            return ViewState["GroupName"] == null ? "" : Convert.ToString(ViewState["GroupName"]);
        }
        set
        {
            ViewState["GroupName"] = value;
        }
    }
    public string TaxId
    {
        get
        {
            return ViewState["TaxId"] == null ? "" : Convert.ToString(ViewState["TaxId"]);
        }
        set
        {
            ViewState["TaxId"] = value;
        }
    }
    #region Parent Page Events

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    #endregion
    private PendingGroupAffiliationsPresenter _presenter;

    public PendingGroupAffiliationsPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new PendingGroupAffiliationsPresenter(this);
            }

            return _presenter;
        }
    }

    public PendingGroupAffiliations Model { get; set; }


    #region Page Events
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        this.ValidateData();
        
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("PendingGroupAffiliations") && !v.IsValid)
                    return;
            }
            catch
            {
                continue;
            }
        }
        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            return;
        }
        DataSet dsIndv = presenter.GetIndvRegistrationData(this.WorkflowPage.RegistrationId);
        DataSet dsGrp = presenter.GetProviderByMedicaidID(txtMedicaidID.Text.Trim());
        PendingGroupAffiliations pendingAff = new PendingGroupAffiliations();
        if(RegPendingAffiliationID != 0)
            pendingAff.RegPendingAffiliationID = RegPendingAffiliationID;

        pendingAff.RegID = this.WorkflowPage.RegistrationId;
        pendingAff.GrpRegID = Helper.GetInt("REG_ID", dsGrp.Tables[0].Rows[0]);
        pendingAff.IndvName = Helper.GetString("NAME", dsIndv.Tables[0].Rows[0]);
        pendingAff.FirstName = Helper.GetString("FIRST_NAME", dsIndv.Tables[0].Rows[0]);
        pendingAff.LastName = Helper.GetString("LAST_NAME", dsIndv.Tables[0].Rows[0]);
        pendingAff.IndvNPI = Helper.GetString("NPI", dsIndv.Tables[0].Rows[0]).Trim();
        pendingAff.grpAffiliationStatus = CON.GroupAffiliationStatusTypeID.PendingApproval; //Grp Affiliation status pending Approval
        pendingAff.GroupName = GroupName; //Here get group name and tax id by medicaid Id
        if (!string.IsNullOrEmpty(txtMedicaidID.Text))
            pendingAff.MedicaidID = txtMedicaidID.Text;
        else
            pendingAff.MedicaidID = "000000000";
        pendingAff.NPI = txtNPI.Text;
        pendingAff.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        Model = pendingAff;
        presenter.SaveRegPendingAffiliation(pendingAff);
        presenter.SaveIndividualRegAffiliation(pendingAff);
        SaveRegAffiliation();
        if (SaveEvent != null)
        {
            SaveEvent();
        }
    }

    private void SaveRegAffiliation()
    {

    }

    private void ValidateData()
    {
        //if (!string.IsNullOrEmpty(txtMedicaidID.Text) && (RegPendingAffiliationID == 0 || (RegPendingAffiliationID != 0 && MedicaidID != txtMedicaidID.Text)))
        //{
        //    DataSet ds = presenter.GetAffiliationByGRPMedicaidID(this.WorkflowPage.RegistrationId, txtMedicaidID.Text, txtTaxID.Text, txtNPI.Text);
        //    if (Helper.HasRows(ds))
        //    {
        //        CustomValidator val = new CustomValidator();
        //        val.IsValid = false;
        //        val.ErrorMessage = "You are already confirmed within this Group – add is not required.";
        //        val.ValidationGroup = "PendingGroupAffiliations";
        //        this.Page.Validators.Add(val);
        //    }
        //}
        if (!string.IsNullOrEmpty(txtMedicaidID.Text) && (RegPendingAffiliationID == 0 || (RegPendingAffiliationID != 0 && MedicaidID != txtMedicaidID.Text)))
        {
            DataSet groupAffStatus = presenter.GetAffiliationByStatusMedicaidId(txtMedicaidID.Text, this.WorkflowPage.RegistrationId, CON.GroupAffiliationStatusTypeID.IndividualRequiresReValidation);
            if (Helper.HasRows(groupAffStatus))
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "Your affiliation is already added to group -Individual requires revalidation status";
                val.ValidationGroup = "PendingGroupAffiliations";
                this.Page.Validators.Add(val);
            }
            DataSet ds = presenter.GetPendingAffiliationByMedicaidID(txtMedicaidID.Text, this.WorkflowPage.RegistrationId);
            if (Helper.HasRows(ds))
            {
                bool isAffPresent = Helper.GetBool("RESULT", ds.Tables[0].Rows[0]);
                if (isAffPresent)
                {
                    CustomValidator val = new CustomValidator();
                    val.IsValid = false;
                    val.ErrorMessage = "You have already added this Group – add is not required.";
                    val.ValidationGroup = "PendingGroupAffiliations";
                    this.Page.Validators.Add(val);
                }
            }
            if(this.WorkflowPage.MMISProviderTypeID == AppSettings.Get("PharmacistProviderMMISTypeID"))
            {
                if (!string.IsNullOrEmpty(txtMedicaidID.Text))
                {
                    DataSet ds1 = svc.SelectRegistrationsByMedicaidID(txtMedicaidID.Text.Trim());
                    if (Helper.HasRows(ds1))
                    {
                        DataTable dt = ds1.Tables[0];
                        if (!AppSettings.Get("AllowedGroupProviderTypesAffiliation").Contains(dt.Rows[0]["mmis_provider_type_id"].ToString()))
                        {
                            CustomValidator val = new CustomValidator();
                            val.IsValid = false;
                            val.ErrorMessage = "You cannot add to this Group.";
                            val.ValidationGroup = "PendingGroupAffiliations";
                            this.Page.Validators.Add(val);
                        }
                    }
                }
            }
        }
        if (!string.IsNullOrEmpty(txtMedicaidID.Text) && !string.IsNullOrEmpty(txtNPI.Text))
        {
            DataSet ds = presenter.GetProviderByMedicaidID(txtMedicaidID.Text.Trim());
            if (Helper.HasRows(ds))
            {
                string npi = Helper.GetString("NPI", ds.Tables[0].Rows[0]).Trim();
                bool CanHaveAffiliates = Helper.GetBool("CanHaveAffiliates", ds.Tables[0].Rows[0]);
                int enrollmentStatusCode = Helper.GetInt("ENROLLMENT_STATUS_CODE", ds.Tables[0].Rows[0]);
                
                if (npi != txtNPI.Text.Trim()
                    || enrollmentStatusCode != CON.EnrollStatus.ACTIVE
                    || CanHaveAffiliates == false)
                {

                    CustomValidator val = new CustomValidator();
                    val.IsValid = false;
                    val.ErrorMessage = "The Medicaid ID and NPI do not match an active group within PNM – please reenter.";
                    val.ValidationGroup = "PendingGroupAffiliations";
                    this.Page.Validators.Add(val);

                }
                else
                { //No error then save taxid and groupname to avoid extra round trip to DB.
                    GroupName = Helper.GetString("NAME", ds.Tables[0].Rows[0]).Trim();
                    TaxId = Helper.GetString("TAX_ID", ds.Tables[0].Rows[0]).Trim();
                }


            }
            else
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "The Medicaid ID entered does not exist in the system – please try again.";
                val.ValidationGroup = "PendingGroupAffiliations";
                this.Page.Validators.Add(val);
            }

        }
        
    }








    protected void btnClose_Click(object sender, EventArgs e)
    {


    }


    #endregion

    #region Public Methods

    public void LoadData(int regPendingAffiliationID)
    {
        if (regPendingAffiliationID == 0) EnableAllFields(true);
        else
        {
            RegPendingAffiliationID = regPendingAffiliationID;
            DataSet ds = presenter.GetPendingAffiliationByID(regPendingAffiliationID);
            if (Helper.HasRows(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
               // txtGroupName.Text = Helper.GetString("GroupName", dr);
                txtMedicaidID.Text = Helper.GetString("Medicaid_ID", dr);
                MedicaidID = txtMedicaidID.Text;
                txtNPI.Text = Helper.GetString("NPI", dr);
               // txtTaxID.Text = Helper.GetString("Tax_ID", dr);
                txtNPI.Enabled = false;
               // txtTaxID.Enabled = false;
            }
            lblError.Visible = false;
        }
    }
    public void DeleteRegPendingAffiliation(int regPendingAffiliationID)
    {
        if(regPendingAffiliationID > 0)
        presenter.DeletePendingAffiliationByID(regPendingAffiliationID);
    }
    #endregion





    public void InitializeFields()
    {
        DataRow provRow = Registration.GetProviderInfo(this.WorkflowPage.RegistrationId);
        string provider_type_id = Helper.GetString("MMIS_Provider_Type_ID", provRow);
        //Below lines were commented out with new enhancement user should n't be allowed to enter group name and tax id.
        //if (Registration.IsPCAProvider(provider_type_id) || Registration.IsPAProvider(provider_type_id))
        //{
        //    this.dvGrpname.Text = "Provider Name";
        //}
        //else
        //{
        //    this.dvGrpname.Text = "Group Name";
        //}        
        //this.txtGroupName.Enabled = true;
        //txtGroupName.Text = "";
        txtMedicaidID.Text = "";
        //txtTaxID.Text = "";
        txtNPI.Text = "";
       // txtTaxID.Enabled = false;
        txtNPI.Enabled = false;
        txtMedicaidID.Enabled = true;
        RegPendingAffiliationID = 0;
        lblError.Visible = false;
        MedicaidID = "";
    }

    private void EnableAllFields(bool enable)
    {
        //bool sentToMMIS = AlreadySentToMMIS();

       // txtGroupName.Enabled = enable;
        txtMedicaidID.Enabled = enable;
       // txtTaxID.Enabled = !enable;
        txtNPI.Enabled = !enable;

    }






    private bool SaveData()
    {
        bool rtn = true;


        return rtn;
    }

    private void InitFormFields()
    {
        
        
        //this.txtGroupName.Text = 
        this.txtMedicaidID.Text = string.Empty;
        //txtTaxID.Text = string.Empty;
        txtNPI.Text = string.Empty;

        
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
    public void InsertRegPendingAffiliation(DataSet ds)
    {
        //;
    }
    public void UpdateRegPendingAffiliation(DataSet ds)
    {
        //;
    }
    public void GetRegHealthCareFacilityAffiliation(DataSet ds)
    {
        //;
    }
    protected void txtMedicaidID_TextChanged(object sender, EventArgs e)
    {
        DataSet ds = presenter.GetProviderByMedicaidID(txtMedicaidID.Text);
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtNPI.Text = Helper.GetString("NPI",dr);
           // txtTaxID.Text = Helper.GetString("Tax_ID", dr);
            txtNPI.Enabled =(!string.IsNullOrEmpty(txtNPI.Text.Trim()))? false: true;
           // txtTaxID.Enabled = false;
        }
        else
        {
            txtNPI.Enabled = true;
           // txtTaxID.Enabled = true;
            lblError.Visible = true;
        }
    }
}