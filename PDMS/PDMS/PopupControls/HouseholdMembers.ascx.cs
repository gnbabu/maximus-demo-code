using System;
using System.Data;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Pages_HouseholdMembers : System.Web.UI.UserControl
{
    public System.EventHandler InvalidateAgreements;

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

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.HouseholdMembers)
        {
            this.ucMemberDetail.SaveEvent += new PopupControls_HouseholdMember.SaveEventHandler(UpdateMembers);
            this.ucMemberDetail.ValidationEvent += new PopupControls_HouseholdMember.ValidationEventHandler(KeepPopupOpen);
            this.ucMemberDetail.ErrorEvent += new PopupControls_HouseholdMember.ErrorEventHandler(KeepPopupOpen);
            this.ucMemberDetail.KeepOpenEvent += new PopupControls_HouseholdMember.KeepOpenEventHandler(KeepPopupOpen);
            this.ucMemberDetail.CancelEvent += new PopupControls_HouseholdMember.CancelEventHandler(CancelPopup);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (this.WorkflowPage.InvalidateAgreements)
        {
            if (InvalidateAgreements != null) InvalidateAgreements(this, new EventArgs());
            this.WorkflowPage.InvalidateAgreements = false;
        }

    }

    protected void gvMembers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        int regHHMemberID = string.IsNullOrEmpty(this.gvMembers.DataKeys[index].Values["REG_HOUSEHOLD_MEMBER_ID"].ToString()) ? 0 : (int)this.gvMembers.DataKeys[index].Values["REG_HOUSEHOLD_MEMBER_ID"];
        switch (e.CommandName)
        {
            case "EditMember":
                lblTitle.Text = Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) ? "View Member Information" : "Edit Member Information";
                this.ucMemberDetail.LoadData(regHHMemberID);
                this.mpeDetail.Show();
                break;
            case "DeleteMember":
                this.DeleteMember(regHHMemberID);
                break;
            default:
                break;
        }
    }

    protected void gvMembers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
            if (lnkEdit != null)
            {
                lnkEdit.ToolTip = Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) ? "View Details" : "Edit Details";
            }
            LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
            if (lnkDelete != null)
            {
                if (Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName)) lnkDelete.Visible = false;
                else lnkDelete.Attributes.Add("onclick", "javascript:return confirm('Member will be deleted. Are you sure?');");
            }
        }
    }

    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        this.ucMemberDetail.LoadData(0);
        this.mpeDetail.Show();
    }

    protected void lnkHistory_Click(object sender, EventArgs e)
    {
        //what does history show
    }
    #endregion

    #region Public Methods
    public bool SaveData()
    {
        return true;
    }
    public void LoadData()
    {
        LoadMembers();
        lnkAdd.Visible = Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName);
        this.cpeInstructions.Collapsed = Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName);
        this.sepInstructions.Header = Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) ? "+ Instructions" : "- Instructions";
    }

    public bool ValidateData()
    {
        bool isGood = true;
        //bug6104
        /*if (gvMembers.Rows.Count == 0) AddError("*House hold member information is required.", ref isGood);*/
        return isGood;
    }

    #endregion

    #region Private Methods

    private void LoadMembers()
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "HOUSEHOLD_MEMBER");
        if (Helper.HasRows(ds))
        {
            DataTable dt = ds.Tables[0];
            this.gvMembers.DataSource = dt;
        }
        else
        {
            this.gvMembers.DataSource = ds;
        }
        this.gvMembers.DataBind();
        // Read-Only users do not have the ability to delete owners
        this.gvMembers.Columns[5].Visible = Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName);
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    private void DeleteMember(int memberID)
    {
        //NEEDS to be custom so can delete child records.
        svc.DeleteRegistrationData("HOUSEHOLD_MEMBERCustom", "REG_HOUSEHOLD_MEMBER_ID", memberID);
        this.LoadMembers();
    }

    private void KeepPopupOpen()
    {
        this.mpeDetail.Show();
    }

    private void CancelPopup()
    {
        this.mpeDetail.Hide();
    }

    private void UpdateMembers()
    {
        LoadMembers();
        this.mpeDetail.Hide();
    }
    #endregion
}