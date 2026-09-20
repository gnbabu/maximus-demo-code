using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_HouseholdMember : System.Web.UI.UserControl
{
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

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }


    private enum MultiviewMbrChild
    {
        AddressHistory = 0,
        CriminalHistory = 1
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

    #region Properties

    private int HouseholdMemberID
    {
        get
        {
            return ViewState["HouseholdMemberID"] == null ? 0 : Convert.ToInt32(ViewState["HouseholdMemberID"]);
        }
        set
        {
            ViewState["HouseholdMemberID"] = value;
        }
    }

    private bool UpdateParentGrid
    {
        get
        {
            return ViewState["UpdateParentGrid"] == null ? false : Convert.ToBoolean(ViewState["UpdateParentGrid"]);
        }
        set
        {
            ViewState["UpdateParentGrid"] = value;
        }
    }

    private int SelectedAddressHistoryID
    {
        get
        {
            return ViewState["SelectedAddressHistoryID"] == null ? 0 : Convert.ToInt32(ViewState["SelectedAddressHistoryID"]);
        }
        set
        {
            ViewState["SelectedAddressHistoryID"] = value;
        }
    }

    private int SelectedCriminalHistoryID
    {
        get
        {
            return ViewState["SelectedCriminalHistoryID"] == null ? 0 : Convert.ToInt32(ViewState["SelectedCriminalHistoryID"]);
        }
        set
        {
            ViewState["SelectedCriminalHistoryID"] = value;
        }
    }

    private Dictionary<string, string> Errors = new Dictionary<string, string>();


    #endregion

    #region Page Events
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (UpdateParentGrid)
        {
            //call parent save event to refresh the grid
            if (SaveEvent != null)
                SaveEvent();
        }
        else if (CancelEvent != null)
        {
            CancelEvent();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Page.Validate("HouseholdMember");

        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            return;
        }

        bool rtn = SaveData();
        if (!rtn)
            return;

        if (KeepOpenEvent != null)
        {
            KeepOpenEvent();
        }
    }

    protected void btnPopAddrSave_Click(object sender, EventArgs e)
    {
        Page.Validate("MemberAddress");

        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            this.mpeMbrChild.Show();
            return;
        }

        bool rtn = SaveAddressData();
        if (!rtn)
        {
            this.mpeMbrChild.Show();
            return;
        }

        this.GetAddressHistory();
        if (KeepOpenEvent != null)
        {
            KeepOpenEvent();
        }

        this.mpeMbrChild.Hide();
        SelectedAddressHistoryID = 0;
    }

    protected void btnPopCancel_Click(object sender, EventArgs e)
    {
        SelectedCriminalHistoryID = 0;
        SelectedAddressHistoryID = 0;

        if (KeepOpenEvent != null)
        {
            KeepOpenEvent();
        }

        this.mpeMbrChild.Hide();
    }

    protected void btnPopCHSave_Click(object sender, EventArgs e)
    {
        //Add record and update grid
        Page.Validate("MemberCriminalHist");

        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            this.mpeMbrChild.Show();
            return;
        }

        bool rtn = this.SaveCriminalData();
        if (!rtn)
        {
            this.mpeMbrChild.Show();
            return;
        }

        this.GetCriminalHistory();
        if (KeepOpenEvent != null)
        {
            KeepOpenEvent();
        }

        this.mpeMbrChild.Hide();
        SelectedCriminalHistoryID = 0;
    }

    protected void lnkAddAddrHist_Click(object sender, EventArgs e)
    {
        //Add new address history record
        this.lblPopAddrTitle.Text = "Add Member Address History";
        this.InitChildDetail(MultiviewMbrChild.AddressHistory);
        this.ShowChildDetail(MultiviewMbrChild.AddressHistory);
    }

    protected void lnkAddCHist_Click(object sender, EventArgs e)
    {
        //Add new criminal history record
        this.lblPopAddrTitle.Text =  "Add Member Criminal History";
        this.InitChildDetail(MultiviewMbrChild.CriminalHistory);
        this.ShowChildDetail(MultiviewMbrChild.CriminalHistory);
    }

    protected void gvAddressHist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //set title to View/Edit
        int index = Convert.ToInt32(e.CommandArgument);
        int addressHistoryID = (int)this.gvAddressHist.DataKeys[index].Values["REG_HOUSEHOLD_MEMBER_ADDRESS_HISTORY_ID"];
        SelectedAddressHistoryID = addressHistoryID;
        switch (e.CommandName)
        {
            case "EditAddress":
                this.InitChildDetail(MultiviewMbrChild.AddressHistory);
                this.ShowChildDetail(MultiviewMbrChild.AddressHistory);
                DataSet ds = this.GetAddressHistoryDetail(addressHistoryID);
                if (Helper.HasRows(ds))
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    this.SetAddressHistoryDetail(row);
                }
               break;
            case "DeleteAddress":
                this.DeleteAddressHistory(addressHistoryID);
                break;
            default:
                break;
        }
        if (KeepOpenEvent != null)
        {
            KeepOpenEvent();
        }
    }

    protected void gvCriminalHist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text.Length > 150)
            {
                e.Row.Cells[0].Text = string.Concat(e.Row.Cells[0].Text.Substring(0, 150), "...");
            }
        }
    }

    protected void gvCriminialHist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        int criminalHistoryID = (int)this.gvCriminalHist.DataKeys[index].Values["REG_HOUSEHOLD_MEMBER_CRIMINAL_HISTORY_ID"];
        SelectedCriminalHistoryID = criminalHistoryID;
        switch (e.CommandName)
        {
            case "EditCrimHist":
                this.InitChildDetail(MultiviewMbrChild.CriminalHistory);
                this.ShowChildDetail(MultiviewMbrChild.CriminalHistory);
                DataSet ds = this.GetCriminalHistoryDetail(criminalHistoryID);
                if (Helper.HasRows(ds))
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    this.SetCriminalHistoryDetail(row);
                }
                break;
            case "DeleteCrimHist":
                this.DeleteCriminialHistory(criminalHistoryID);
                break;
            default:
                break;
        }
        if (KeepOpenEvent != null)
        {
            KeepOpenEvent();
        }

    }

    #endregion

    #region Public Methods

    public void LoadData(int memberID)
    {
        this.InitFormFields();

        HouseholdMemberID = memberID;
        UpdateParentGrid = false;

        this.SetInitialFieldVisibility();

        if (HouseholdMemberID > 0)
        {
            DataSet ds = this.GetHouseholdMemberInfo();
            this.SetMemberInformation(ds);
        }

        this.SetEditability();

        this.btnSave.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        this.btnPopAddrSave.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnPopAddrSave, null) + ";");
        this.btnPopCHSave.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnPopCHSave, null) + ";");
    }

    #endregion

    #region Private Methods
    private void SetEditability()
    {
        bool enabled = Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.Administrator);
        enabled = enabled && !Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName);  //provider can not modify unless registration is in return to provider or provider data entry
        //?for provider can't you just check if in provider data entry type of task?

        this.EnableAllFields(enabled);
        this.btnSave.Visible = enabled;
        this.btnCancel.Visible = enabled;
   }

    private void EnableAllFields(bool enable)
    {
        txtName.Enabled = enable;
        txtTaxID.Enabled = enable;
        ddlSex.Enabled = enable;
        txtBirthDate.Enabled = enable;
        txtRelationship.Enabled = enable;
        txtPreviousNames.Enabled = enable;
    }

    private void SetInitialFieldVisibility()
    {
        this.lnkAddAddrHist.Visible = !Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) && HouseholdMemberID > 0;
        this.lnkAddCHist.Visible = !Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) && HouseholdMemberID > 0;
        btnSave.Visible = (!Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) || Helper.IsLoggedInUserInAdminRole());
    }

    private void InitFormFields()
    {
        this.ClearErrorMessages();
        this.HouseholdMemberID = 0;
        this.txtName.Text = string.Empty;
        this.txtTaxID.Text = string.Empty;
        this.ddlSex.SelectedIndex = -1;
        this.txtBirthDate.Text = string.Empty;
        this.txtRelationship.Text = string.Empty;
        this.txtPreviousNames.Text = string.Empty;

        if (this.ddlState.Items.Count == 0)
        {
            Helper.LoadDropDownListWithStates(ref ddlState);
        }

        DataTable dt = null;
        this.SetAddressHistory(dt);
        this.SetCriminalHistory(dt);
        
        this.InitAddressHistoryDetail();
        this.InitCriminalHistoryDetail();
    }

    private void InitChildDetail(MultiviewMbrChild mvIndex)
    {
        switch (mvIndex)
        {
            case MultiviewMbrChild.AddressHistory:
                this.InitAddressHistoryDetail();
                this.lblPopAddrTitle.Text = Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) ? "View Member Address History" : "Edit Member Address History";
                this.mpeMbrChild.CancelControlID = "btnPopAddrCancel";
                break;
            case MultiviewMbrChild.CriminalHistory:
                this.InitCriminalHistoryDetail();
                this.lblPopCHTitle.Text = Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) ? "View Member Criminal History" : "Edit Member Criminal History";
                this.mpeMbrChild.CancelControlID = "btnPopCHCancel";
                break;
            default:
                break;
        }
    }

    private void ShowChildDetail(MultiviewMbrChild mvIndex)
    {
        this.mltMbrChild.ActiveViewIndex = (int)mvIndex;
        this.mpeMbrChild.Show();

        if (KeepOpenEvent != null)
        {
            KeepOpenEvent();
        }
    }

    private void HideChildDetail()
    {
        this.vsMemberAddress.Enabled = false;
        this.vsMemberCH.Enabled = false;
        this.vsHouseholdMember.Enabled = true;
        this.mpeMbrChild.Hide();
    }

    private void InitAddressHistoryDetail()
    {
        this.txtCounty.Text = string.Empty;
        this.txtCity.Text = string.Empty;
        this.ddlState.SelectedIndex = 0;
        this.txtFromDate.Text = string.Empty;
        this.txtToDate.Text = string.Empty;
    }

    private void InitCriminalHistoryDetail()
    {
        this.txtOffense.Text = string.Empty;
    }

    private void SetMemberInformation(DataSet ds)
    {
        //Get all member information.
        if (!Helper.HasRows(ds))
            return;

        DataRow row = ds.Tables[0].Rows[0];
        this.HouseholdMemberID = Helper.GetInt("REG_HOUSEHOLD_MEMBER_ID", row);
        this.txtName.Text = Helper.GetString("NAME", row);
        this.txtBirthDate.Text = Helper.FormatDate2(row["BIRTH_DATE"].ToString());
        this.txtTaxID.Text = Helper.GetString("SSN", row);
        this.txtRelationship.Text = Helper.GetString("RELATIONSHIP", row);
        this.txtPreviousNames.Text = Helper.GetString("PREVIOUS_LAST_NAMES", row);
        this.ddlSex.SelectedValue = Helper.GetString("SEX", row);

        DataTable dt = null;
        if (ds.Tables.Count > 1)
        {
           dt  = ds.Tables[1];
           SetAddressHistory(dt);
        }
        if (ds.Tables.Count > 2)
        {
            dt = ds.Tables[2];
            SetCriminalHistory(dt);
        }
    }

    private DataSet GetHouseholdMemberInfo()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_HOUSEHOLD_MEMBER_ID", this.HouseholdMemberID.ToString());
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_HOUSEHOLD_MEMBER_ByID", parms);
        //returns 3 tables:  * from REG_HOUSEHOLD_MEMBER, * FROM REG_HOUSEHOLD_ADDRESS_HISTORY, and * FROM REG_HOUSEHOLD_MEMBER_CRIMINAL_HISTORY

        
        return ds;
    }
    
    private bool SaveData()
    {
        bool rtn = true;
        this.ClearErrorMessages();
        if (this.HouseholdMemberID > 0)
        {
            this.UpdateHHMember();
        }
        else
        {
            this.InsertHHMember();
        }

        if (Errors.Count > 0)
        {
            SetErrorMessages();
            return false;
        }

        this.lnkAddAddrHist.Visible = HouseholdMemberID > 0;
        this.lnkAddCHist.Visible = HouseholdMemberID > 0;
        UpdateParentGrid = true;
        return rtn;
    }

    private Dictionary<string, string> SetCommonParms()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("NAME", txtName.Text.Trim());
        parms.Add("SSN", txtTaxID.Text.Trim());
        parms.Add("BIRTH_DATE", txtBirthDate.Text);
        parms.Add("SEX", ddlSex.SelectedValue);
        parms.Add("RELATIONSHIP", txtRelationship.Text.Trim());
        parms.Add("PREVIOUS_LAST_NAMES", txtPreviousNames.Text.Trim());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        return parms;
    }

    private void InsertHHMember()
    {
        try
        {
            Dictionary<string, string> parms = this.SetCommonParms();
            int memberID = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "HOUSEHOLD_MEMBER", parms);

            this.HouseholdMemberID = memberID;
        }
        catch(Exception ex)
        {
            Errors.Add("InsertHHMember", ex.Message);
        }
    }

    private void UpdateHHMember()
    {
        try
        {
            Dictionary<string, string> parms = this.SetCommonParms();
            parms.Add("REG_HOUSEHOLD_MEMBER_ID", this.HouseholdMemberID.ToString());
            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "HOUSEHOLD_MEMBER", parms);
        }
        catch (Exception ex)
        {
            Errors.Add("UpdateHHMember", ex.Message);
        }
    }

    private bool SaveAddressData()
    {
        bool rtn = true;
        this.ClearErrorMessages();
        if (this.SelectedAddressHistoryID > 0)
        {
            this.UpdateAddressHistory();
        }
        else
        {
            this.InsertAddressHistory();
        }

        if (Errors.Count > 0)
        {
            SetErrorMessages();
            return false;
        }

        return rtn;
    }

    private Dictionary<string, string> SetCommonAddrParms()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_HOUSEHOLD_MEMBER_ID", this.HouseholdMemberID.ToString());
        parms.Add("COUNTY", this.txtCounty.Text.Trim());
        parms.Add("CITY", this.txtCity.Text.Trim());
        parms.Add("STATE", this.ddlState.SelectedValue);
        parms.Add("FROM_DATE", txtFromDate.Text.Trim());
        if (!string.IsNullOrEmpty(this.txtToDate.Text.Trim()))
        {
            parms.Add("TO_DATE", txtToDate.Text.Trim());
        }
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        return parms;
    }


    private void InsertAddressHistory()
    {
        try
        {
            Dictionary<string, string> parms = this.SetCommonAddrParms();
            int addrID = svc.InsertRegistrationDataTable("HOUSEHOLD_MEMBER_ADDRESS_HISTORY", parms);

            this.SelectedAddressHistoryID = addrID;
        }
        catch (Exception ex)
        {
            Errors.Add("InsertAddressHistory", ex.Message);
        }
    }

    private void UpdateAddressHistory()
    {
        try
        {
            Dictionary<string, string> parms = this.SetCommonAddrParms();
            parms.Add("REG_HOUSEHOLD_MEMBER_ADDRESS_HISTORY_ID", this.SelectedAddressHistoryID.ToString());
            svc.UpdateRegistrationDataTable("HOUSEHOLD_MEMBER_ADDRESS_HISTORY", parms);
        }
        catch (Exception ex)
        {
            Errors.Add("UpdateAddrHistory", ex.Message);
        }
    }


    private bool SaveCriminalData()
    {
        bool rtn = true;
        this.ClearErrorMessages();
        if (this.SelectedCriminalHistoryID > 0)
        {
            this.UpdateCriminalHistory();
        }
        else
        {
            this.InsertCriminalHistory();
        }

        if (Errors.Count > 0)
        {
            SetErrorMessages();
            return false;
        }

        return rtn;
    }

    private Dictionary<string, string> SetCommonCHParms()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_HOUSEHOLD_MEMBER_ID", this.HouseholdMemberID.ToString());
        parms.Add("CRIMINAL_HISTORY", this.txtOffense.Text.Trim());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        return parms;
    }

    private void InsertCriminalHistory()
    {
        try
        {
            Dictionary<string, string> parms = this.SetCommonCHParms();
            int chID = svc.InsertRegistrationDataTable("HOUSEHOLD_MEMBER_CRIMINAL_HISTORY", parms);

            this.SelectedCriminalHistoryID = chID;
        }
        catch (Exception ex)
        {
            Errors.Add("InsertCriminalHistory", ex.Message);
        }
    }

    private void UpdateCriminalHistory()
    {
        try
        {
            Dictionary<string, string> parms = this.SetCommonCHParms();
            parms.Add("REG_HOUSEHOLD_MEMBER_CRIMINAL_HISTORY_ID", this.SelectedCriminalHistoryID.ToString());
            svc.UpdateRegistrationDataTable("HOUSEHOLD_MEMBER_CRIMINAL_HISTORY", parms);
        }
        catch (Exception ex)
        {
            Errors.Add("UpdateCriminalHistory", ex.Message);
        }
    }
    private void GetAddressHistory()
    {
        //Called only when need to reload the address history grid
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_HOUSEHOLD_MEMBER_ID", this.HouseholdMemberID.ToString());
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_HOUSEHOLD_MEMBER_ADDRESS_HISTORY", parms);
        DataTable dt = null;
        if (Helper.HasRows(ds))
        {
            dt = ds.Tables[0];
        }
        this.SetAddressHistory(dt);
    }

    private void SetAddressHistory(DataTable dt)
    {
        this.gvAddressHist.DataSource = dt;
        this.gvAddressHist.DataBind();
        if (gvAddressHist.Rows.Count == 0)
        {
            this.gvAddressHist.Columns[5].Visible = false;
            this.gvAddressHist.Columns[6].Visible = false;
        }
        else
        {
            this.gvAddressHist.Columns[5].Visible = true;
            // Read-Only users do not have the ability to delete 
            this.gvAddressHist.Columns[6].Visible = !Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) && Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name);
        }
    }

    private DataSet GetAddressHistoryDetail(int addrHistoryID)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_HOUSEHOLD_MEMBER_ADDRESS_HISTORY_ID", addrHistoryID.ToString());
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_HOUSEHOLD_MEMBER_ADDRESS_HISTORY_ByID", parms);
        return ds;
    }

    private void SetAddressHistoryDetail(DataRow row)
    {
        this.lblPopAddrTitle.Text = Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) ? "View Member Address History" : "Edit Member Address History";
        this.txtCounty.Text = Helper.GetString("COUNTY", row);
        this.txtCity.Text = Helper.GetString("CITY", row);
        this.ddlState.SelectedValue = Helper.GetString("STATE", row);
        this.txtFromDate.Text = Helper.FormatDate2(row["FROM_DATE"].ToString());
        this.txtToDate.Text = Helper.FormatDate2(row["TO_DATE"].ToString());
    }

    private void DeleteAddressHistory(int addrHistoryID)
    {
        svc.DeleteRegistrationData("HOUSEHOLD_MEMBER_ADDRESS_HISTORY", "REG_HOUSEHOLD_MEMBER_ADDRESS_HISTORY_ID", addrHistoryID);
        this.GetAddressHistory();
    }

    private void GetCriminalHistory()
    {
        //Called only when need to reload the criminal history grid
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_HOUSEHOLD_MEMBER_ID", this.HouseholdMemberID.ToString());
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_HOUSEHOLD_MEMBER_CRIMINAL_HISTORY", parms);
        DataTable dt = null;
        if (Helper.HasRows(ds))
        {
            dt = ds.Tables[0];
        }
        this.SetCriminalHistory(dt);
    }

    private void SetCriminalHistory(DataTable dt)
    {
        this.gvCriminalHist.DataSource = dt;
        this.gvCriminalHist.DataBind();
        if (gvCriminalHist.Rows.Count == 0)
        {
            this.gvCriminalHist.Columns[1].Visible = false;
            this.gvCriminalHist.Columns[2].Visible = false;
        }
        else
        {
            this.gvCriminalHist.Columns[1].Visible = true;
            // Read-Only users do not have the ability to delete 
            this.gvCriminalHist.Columns[2].Visible = !Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) && Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name);
        }
    }

    private DataSet GetCriminalHistoryDetail(int criminalHistoryID)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_HOUSEHOLD_MEMBER_CRIMINAL_HISTORY_ID", criminalHistoryID.ToString());
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_HOUSEHOLD_MEMBER_CRIMINAL_HISTORY_ByID", parms);
        return ds;

    }

    private void SetCriminalHistoryDetail(DataRow row)
    {
        this.lblPopAddrTitle.Text = Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) ? "View Member Criminal History" : "Edit Member Criminal History";
        this.txtOffense.Text = Helper.GetString("CRIMINAL_HISTORY", row);
    }


    private void DeleteCriminialHistory(int criminalHistoryID)
    {
        svc.DeleteRegistrationData("HOUSEHOLD_MEMBER_CRIMINAL_HISTORY", "REG_HOUSEHOLD_MEMBER_CRIMINAL_HISTORY_ID", criminalHistoryID);
        this.GetCriminalHistory();
    }


    private void SetErrorMessages()
    {

        if (Errors.Count > 0)
        {
            ClearErrorMessages();
            SetErrors();
        }

        if (ErrorEvent != null)
        {
            //have to keep the pop up open
            ErrorEvent();
            return;
        }
    }

    private void SetErrors()
    {
        foreach (var pair in Errors)
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
        Errors.Clear();
        lblErrorMessages.Text = string.Empty;
    }


    #endregion

    #region Validation
    
    protected void Validate_FutureBirthDate(object sender, ServerValidateEventArgs e)
    {
        DateTime date;

        if (DateTime.TryParse(txtBirthDate.Text.Trim(), out date))
        {
            e.IsValid = date >= DateTime.Now || date < DateTime.Now.AddYears(-100) ? false : true;
        }
    }

    protected void Validate_FutureFromDate(object sender, ServerValidateEventArgs e)
    {
        DateTime date;

        if (DateTime.TryParse(this.txtFromDate.Text.Trim(), out date))
        {
            e.IsValid = date >= DateTime.Now ? false : true;
        }
    }
       
    #endregion

}