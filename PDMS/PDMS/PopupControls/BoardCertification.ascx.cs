using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
public partial class PopupControls_BoardCertification : BaseSectionControl, IBoardCertificationsView 
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

    public int RegBoardCertificationID
    {
        get
        {
            return ViewState["RegBoardCertificationID"] == null ? 0 : Convert.ToInt32(ViewState["RegBoardCertificationID"]);
        }
        set
        {
            ViewState["RegBoardCertificationID"] = value;
        }
    }
    #region Parent Page Events

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    #endregion
    private BoardCertificationsPresenter _presenter;

    public BoardCertificationsPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new BoardCertificationsPresenter(this);
            }

            return _presenter;
        }
    }

    public BoardCertifications Model { get; set; }
    public bool CanUserViewDelete()
    {

        return Registration.CanUserViewDelete(this.WorkflowPage.RegistrationId,this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName);
    }

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        {
            LoadBoardCertification();
            if (ddlBoardSpecialty.Items.Count == 0)
                LoadBoardSpecialty();
        }
        //OHPNM-3487 - on click of View provider file Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(btnAddBoardCertification, true);
                btnAddBoardCertification.Visible = false;
            }
        }

        chkIsPrimaryBoard.InputAttributes.Add("aria-label", "Designate as Primary Board Certification");

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }



    public override bool ValidateData()
    {
        Page.Validate("valBoardCertifications");
        if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 &&
            rblBoardCertified.SelectedValue == "0")
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "Board Certification is required.";
            val.ValidationGroup = "valBoardCertifications";
            this.Page.Validators.Add(val);
            

        }
        if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 0 &&
            rblBoardCertified.SelectedValue == "0")
        {
            return true;
        }

            
            for (int i = 0; i < Page.Validators.Count; i++)
            {
                BaseValidator v;
                try
                {
                    v = Page.Validators[i] as BaseValidator;
                    if (v != null && v.ValidationGroup.Equals("valBoardCertifications") && !v.IsValid)
                        return false;
                }
                catch
                {
                    continue;
                }
            }

        return true;
    }



    #endregion

    #region Public Methods

    public override void LoadData(DataRow dr)
    {
        if (dr != null)
        {
            rblBoardCertified.SelectedValue = "1";
            divBoardCertification.Visible = true;

            var isPrimary = Helper.GetBool("IsPrimary", dr);
            if(isPrimary)
            {
                chkIsPrimaryBoard.Checked = true;
                chkIsPrimaryBoard.Enabled = false;
                lblPrimaryBoardMessage.Text = GetGlobalResourceObject("BrandingResource", "PrimaryBoardCertification").ToString();
            }
            else
            {
                divIsPrimary.Visible = false;
                chkIsPrimaryBoard.Checked = false;
                chkIsPrimaryBoard.Enabled = false;
            }

            txtEffectiveDate.Text = Helper.GetDate("Effective_Date", dr);
            
            ddlBoardCertification.SelectedValue = Helper.GetString("BOARD_CERTIFICATION_id", dr);
            LoadBoardSpecialty();
            ddlBoardSpecialty.SelectedValue = Helper.GetString("BOARD_SPECIALTY_id", dr);
            txtBoardSpecialty.Text = Helper.GetString("BOARD_SPECIALTY", dr);
            if(ddlBoardCertification.SelectedItem.Text == "Other Board Certification")
            {
                txtBoardSpecialty.Visible = true;
                ddlBoardSpecialty.Visible = false;
                valtxtBoardSpecialty.ControlToValidate = "txtBoardSpecialty";
            }
            else
            {
                ddlBoardSpecialty.Visible = true;
                txtBoardSpecialty.Visible = false;
                valtxtBoardSpecialty.ControlToValidate = "ddlBoardSpecialty";
            }
            txtExpirationDate.Text = Helper.GetDate("Expiration_Date", dr);
            RegBoardCertificationID = Helper.GetInt("REG_Board_Certification_ID", dr);
            txtCertificationNumber.Text = Helper.GetString("CERTIFICATION_NUMBER", dr);
        }
        else
        {
            ddlBoardSpecialty.SelectedValue = "";
            txtBoardSpecialty.Text = "";
            txtExpirationDate.Text = "";
            ddlBoardCertification.SelectedValue = "";
            txtEffectiveDate.Text = "";
            RegBoardCertificationID = 0;
        }
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
    }

    private void LoadBoardCerts()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "BOARD_CERTIFICATION");
        DataTable dtBoardCertification = Helper.HasRows(ds) ? ds.Tables[0] : null;
        grdBoardCertification.DataSource = this.DataList = dtBoardCertification;
        grdBoardCertification.DataBind();
        btnAddBoardCertification.Visible = true;
    }


    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        DataRow dr;
        dr = this.DataList.Rows[index];

        if (e.CommandName == "DeleteBoardRow")
        {
            bool IsDeleted = DeleteBoardCerts(dr);
            if (IsDeleted)
                LoadBoardCerts();
        }
        else
        {
            BoardDetail.Visible = true;
            if (Helper.HasRows(this.DataList))
                this.LoadData(dr);
            else
                this.LoadData(null);
        }
    }


    private bool DeleteBoardCerts(DataRow dr)
    {
        bool isValid = true;

        if (this.DataList.Rows.Count == 1 && Registration.EntryIsRequired(this.WorkflowPage.RegistrationId, CON.RegistrationPageName.Certification, Registration.GetSectionNameFromStepNumber(CON.SectionTypeID.BoardCertification)))
        //if only one record and required page then prevent deletion or else the page will not turn back to blue once it is green and will cause issues for page submission
        {
            AddError("* This is required section. This board Certification cannot be deleted.", ref isValid, ValidationGroup);
            return isValid;
        }
        if (dr != null)
        {
            int REG_BOARD_CERTIFICATION_ID = Helper.GetInt("REG_BOARD_CERTIFICATION_ID", dr);
            //int BOARD_CERTIFICATION_ID = Helper.GetInt("BOARD_CERTIFICATION_ID", dr);
            bool IsPrimary = Helper.GetBool("IsPrimary", dr);
            if (REG_BOARD_CERTIFICATION_ID > 0 && !IsPrimary)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                psc.DeleteRegistrationData("BOARDCERTIFICATION", "REG_BOARD_CERTIFICATION_ID", REG_BOARD_CERTIFICATION_ID);
            }
            else
            {
                AddError("* The Board Certification can not be deleted.", ref isValid, ValidationGroup);
                return isValid;
            }
        }
        return isValid;
    }

   
    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        BoardDetail.Visible = true;

        this.LoadData(null);
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        // TODO: EDV Here is where we should show/hide history
    }


    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Board Certification";
    }
    public override void LoadControlData()
    {
        LoadBoardCerts();
        if(ddlBoardCertification.Items.Count == 0)
        LoadBoardCertification();
        if (Registration.IsPAProvider(this.WorkflowPage.ProviderTypeID))
        {
            valtxtBoardSpecialty.Enabled = false;
            spBoardSpecialty.InnerHtml = "Board Specialty";
        }
        if (this.WorkflowPage.MMISProviderTypeID == CON.MMISProviderType.BEHAVIOR_ANALYST) 
        {
            this.lblPT53HelpText.Visible = true;
        }
        else
            this.lblPT53HelpText.Visible = false;

    }

    #endregion

    public void InitializeFields()
    {
        
        ddlBoardCertification.SelectedValue = string.Empty;
        ddlBoardSpecialty.SelectedValue = string.Empty;
        txtExpirationDate.Text = string.Empty;
        txtBoardSpecialty.Text = string.Empty;
        txtCertificationNumber.Text = string.Empty;

        RegBoardCertificationID = 0;
        
    }

    private void EnableAllFields(bool enable)
    {

    }

    public override bool HasInputValue()
    {
        bool isRequired = false;
        if (BoardDetail.Visible && (rblBoardCertified.SelectedValue == "" || rblBoardCertified.SelectedValue == "1" || !string.IsNullOrEmpty(txtEffectiveDate.Text) || !string.IsNullOrEmpty(txtExpirationDate.Text) || !string.IsNullOrEmpty(ddlBoardCertification.Text) || !string.IsNullOrEmpty(ddlBoardSpecialty.Text)))
        {
            isRequired = true;
        }
        else if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 0)
        {
            isRequired = true;
        }
        
        return isRequired;
    }
    
    public override bool SaveData()
    {
        bool rtn = false;
        if (BoardDetail.Visible)
        {


            BoardCertifications Board = new BoardCertifications();
            if (RegBoardCertificationID != 0)
                Board.RegBoardCertificationID = RegBoardCertificationID;

            Board.RegID = this.WorkflowPage.RegistrationId;
            if (chkIsPrimaryBoard.Visible == true && chkIsPrimaryBoard.Checked)
            {
                Board.IsPrimary = true;
            }
            if (divBoardCertification.Visible == true)
            {
                Board.BoardCertificationID = Convert.ToInt32(ddlBoardCertification.SelectedValue);
            }
            if (Registration.IsPAProvider(this.WorkflowPage.ProviderTypeID) || ddlBoardSpecialty.Visible == false)
            {
                Board.BoardSpecialtyID = 0;
            }
            else
            {
                Board.BoardSpecialtyID = Convert.ToInt32(ddlBoardSpecialty.SelectedValue);
            }

            if (!string.IsNullOrEmpty(txtEffectiveDate.Text))
                Board.EffectiveDate = Convert.ToDateTime(txtEffectiveDate.Text);
            else
                Board.EffectiveDate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(txtExpirationDate.Text))
                Board.ExpirationDate = Convert.ToDateTime(txtExpirationDate.Text);
            else
                Board.ExpirationDate = DateTime.MinValue;
            Board.CertificationNumber = txtCertificationNumber.Text;
            Board.BoardSpecialty = txtBoardSpecialty.Text;

            Board.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            Board.CreatedUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name);


            Model = Board;
            presenter.SaveRegBoardCertification(Board);

            //hide checkbox here
            //chkIsPrimaryBoard.Visible = false;
            //lblPrimaryBoard.Visible = false;
            //lblPrimaryBoardMessage.Visible = false;
            rtn = true;
        }
        else if (grdBoardCertification.Rows.Count > 0 || this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 0)
        {
            rtn = true;
        }
        else
        {
            rtn = false;
        }
        return rtn;
    }

    private void InitFormFields()
    {



        this.ddlBoardCertification.SelectedValue = ddlBoardSpecialty.SelectedValue = "";
            txtEffectiveDate.Text = txtExpirationDate.Text = txtCertificationNumber.Text = txtBoardSpecialty.Text = string.Empty;


        
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

    public void GetRegBoardCertification(DataSet ds)
    {
        //;
    }
    
    public override string ValidationGroup
    {
        get { return "valBoardCertifications"; }
    }

    public override string Title
    {
        get { return "Board Certification"; }
    }

    public override string IdText
    {
        get { return "ucBoardCertification_" + this.WorkflowPage.RegistrationId; }
    }
    private void AddError(string errMsg, ref bool isGood, string validationGroup)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = validationGroup;
        this.Page.Validators.Add(val);
        isGood = false;
    }
    

    

    protected void ddlBoardCertification_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlBoardCertification.SelectedItem.Text == "Other Board Certification")
        {
            txtBoardSpecialty.Visible = true;
            ddlBoardSpecialty.Visible = false;
            valtxtBoardSpecialty.ControlToValidate = "txtBoardSpecialty";
        }
        else
        {
            valtxtBoardSpecialty.ControlToValidate = "ddlBoardSpecialty";
            LoadBoardSpecialty();
        }
    }
    private void LoadBoardCertification()
    {
        
        int providerTypeID = 0;

        // Load drop down list of BoardCertification statuses
        providerTypeID = this.WorkflowPage.ProviderTypeID;
        DataSet dsBoardCertification = svc.SelectBoardCertificationTypeByProviderTypeID(providerTypeID);
        if (Helper.HasRows(dsBoardCertification))
        {
            Helper.LoadDropDown(ddlBoardCertification, dsBoardCertification.Tables[0], "BOARD_CERTIFICATION_NAME", "BOARD_CERTIFICATION_ID", true);
        }
    }
    private void   LoadBoardSpecialty()
    {
        if (!string.IsNullOrEmpty(ddlBoardCertification.SelectedValue))
        {
            
            int BoardCertificationID = 0;
            BoardCertificationID = Convert.ToInt32(ddlBoardCertification.SelectedValue);
            // Load drop down list of Board Specialty
            DataSet dsBoardSpecialty = svc.SelectBoardSpecialtyTypeByBoardCertificationID(BoardCertificationID);
            if (Helper.HasRows(dsBoardSpecialty))
            {
                Helper.LoadDropDown(ddlBoardSpecialty, dsBoardSpecialty.Tables[0], "BOARD_SPECIALTY_NAME", "BOARD_SPECIALTY_ID", true);
            }
        }
    }
    public bool HasBoardPrimary
    {
        get
        {
            return HasBoardPrimaryCert();
        }
    }
    protected bool HasBoardPrimaryCert()
    {
        bool rtn = false;

        if (Helper.HasRows(this.DataList))
        {
            DataRow[] rowsPrimary = this.DataList.Select("IsPrimary = 1");
            if (rowsPrimary != null && rowsPrimary.Length > 0)
                rtn = true;
        }

        return rtn;
    }

    protected void rblBoardCertified_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblBoardCertified.SelectedValue == "0")
        {
            chkIsPrimaryBoard.Visible = false;
            lblPrimaryBoardMessage.Visible = false;
            divBoardCertification.Visible = false;
            valFNReqd.Enabled = valtxtBoardSpecialty.Enabled = RequiredFieldValidator1.Enabled = RequiredFieldValidator2.Enabled = false;
            spBoardSpecialty.InnerText = "Board Specialty";
            spnboardCertification.InnerText = "Board Certification";
            spnEffectiveDate.InnerText = "Effective Date";
            spnExpirationDate.InnerText = "Expiration Date";
        }
        else
        {
            divBoardCertification.Visible = true;
            if (HasBoardPrimary)
            {
                divIsPrimary.Visible = false;
                chkIsPrimaryBoard.Enabled = false; 
            }
            else
            {
                divIsPrimary.Visible = true;
                lblPrimaryBoardMessage.Text = GetGlobalResourceObject("BrandingResource", "PrimaryBoardCertification").ToString();
                chkIsPrimaryBoard.Checked = true;
            }

            valFNReqd.Enabled = valtxtBoardSpecialty.Enabled = RequiredFieldValidator1.Enabled = RequiredFieldValidator2.Enabled = true;
            spBoardSpecialty.InnerText = "Board Specialty*";
            spnboardCertification.InnerText = "Board Certification*";
            spnEffectiveDate.InnerText = "Effective Date*";
            spnExpirationDate.InnerText = "Expiration Date*";
        }
    }

    protected void ValidatePrimaryBoard(object source, ServerValidateEventArgs args)
    {
        args.IsValid = chkIsPrimaryBoard.Checked;
    }
}