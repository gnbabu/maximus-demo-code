using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_ProviderNotes : System.Web.UI.UserControl
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

    #region Properties

    private DataSet dsProvNotes = new DataSet();
    private bool _displayReadOnly = false;
    public string IcnNumber = string.Empty;
    public string ICN
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(IcnNumber))
                return IcnNumber;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                IcnNumber = value.Trim();
        }
    }

    public string ClaimID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaim_ID.Value))
                return hdnClaim_ID.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaim_ID.Value = value.Trim();
        }
    }

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaim_Type_ID.Value))
                return hdnClaim_Type_ID.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                hdnClaim_Type_ID.Value = value.Trim();
                SetFieldsVisibility(value);
            }
        }
    }

    public DataSet dataSetProviderNotes
    {
        get
        {
            if (!Helper.HasRows(dsProvNotes))
            {
                if (Helper.HasRows(GetProviderNotes()))
                {
                    return dsProvNotes;
                }
            }
            return dsProvNotes;
        }

        set
        {
            if (value != null)
            {
                GetProvNotes(value);
            }
        }
    }
    public string ReferenceCodeDropDown
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlNoteRefCode.SelectedValue))
                return ddlNoteRefCode.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && ddlNoteRefCode.Items.FindByValue(value) != null)
            {
                ddlNoteRefCode.ClearSelection();
                ddlNoteRefCode.SelectedValue = value.Trim();
            }
        }
    }

    public string ProviderNote
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtProviderNotes.Text))
                return txtProviderNotes.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtProviderNotes.Text = value.Trim();
            else
                txtProviderNotes.Text = string.Empty;
        }
    }
    public bool DisplayReadOnly
    {
        get
        {
            return _displayReadOnly;
        }
        set
        {
            _displayReadOnly = value;
            //BindGrid();
        }
    }
    public string ReferenceCodeDropDownSelectedText
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlNoteRefCode.SelectedItem.Text))
                return ddlNoteRefCode.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlNoteRefCode.ClearSelection();
                ddlNoteRefCode.SelectedItem.Text = value.Trim();
            }
        }
    }
    public Boolean SetReadOnlyFields
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value.ToString()))
                SetReadOnlyFieldsControl(value);
        }
    }

    public DataSet SetProfessionalServiceData
    {
        set
        {
            SetProfessionalPanelData(value);
        }
    }

    #endregion

    public bool HasInputValue()
    {
        bool rtn = false;
        if ((ddlNoteRefCode.SelectedIndex > 0) || !string.IsNullOrEmpty(txtProviderNotes.Text))
        {
            rtn = true;
        }

        return rtn;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //  LoadProviderNotes();

        }
        btnProviderNoteAdd.Attributes.Add("onclick", "ProviderDisableEnableConditionAddButton();");
        SetButtonVisibility();
        if (pnlProvNotes.Visible)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>displaytableProviderNote();</script>", false);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>ProviderNotesClearFields();</script>", false);
        }
        ddlNoteRefCode.Enabled = true;
        txtProviderNotes.Enabled = true;
        if (Session["ClaimStatus"].ToString() == "Pending Submission" && providerNotesOutput.Visible == true)
        {
            hdnProviderClaimStatus.Value = "Pending Submission";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>displaytableProviderNote();</script>", false);
        }
    }
    public void SetButtonVisibility()
    {
        if (DisplayReadOnly == true)
        {
            divProvNotes.Visible = false;
        }
        else
        {
            divProvNotes.Visible = true;
        }
    }

    #region 'GridView Events'
    //protected void gvProviderNote_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if ((e.Row.RowState & DataControlRowState.Edit) > 0)
    //    {
    //        if (hdnClaim_Type_ID.Value == CON.ClaimsType.Institutional)
    //        {
    //            List<SqlParameter> param = new List<SqlParameter>();
    //            DropDownList DropDownReferenceCode = (DropDownList)e.Row.FindControl("DropDownReferenceCode");
    //            Dictionary<string, string> parms = new Dictionary<string, string>();
    //            parms.Add("Claim_Type", ClaimType);
    //            DataSet dsNoteRefCode = svc.SelectPanelsData("NoteReferenceCode", parms);
    //            if (DropDownReferenceCode != null)
    //            {
    //                DropDownReferenceCode.DataSource = dsNoteRefCode;
    //                DropDownReferenceCode.DataTextField = "NoteReferenceCode";
    //                DropDownReferenceCode.DataValueField = "Note_ID";
    //                DropDownReferenceCode.DataBind();
    //            }
    //        }
    //    }

    //    if (e.Row.RowType == DataControlRowType.DataRow && DisplayReadOnly)
    //    {
    //        Button btDelete = null;
    //        if (ClaimType == CON.ClaimsType.Dental && DisplayReadOnly)
    //        {
    //            gvProviderNote.Columns[2].Visible = !DisplayReadOnly;
    //            if (DisplayReadOnly == true)
    //            {
    //                gvProviderNote.Columns[3].Visible = !DisplayReadOnly;
    //                divProvNotes.Visible = !DisplayReadOnly;
    //            }
    //            gvProviderNotesInstl.Columns[3].Visible = !DisplayReadOnly;
    //            gvProviderNotesInstl.Columns[4].Visible = !DisplayReadOnly;
    //            btDelete = (Button)e.Row.Cells[3].FindControl("btnDltDentalGrid");
    //        }
    //        else if (ClaimType == CON.ClaimsType.Institutional)
    //        {
    //            gvProviderNotesInstl.Columns[3].Visible = !DisplayReadOnly;
    //            gvProviderNotesInstl.Columns[4].Visible = !DisplayReadOnly;
    //            if (DisplayReadOnly == true)
    //            {
    //                divProvNotes.Visible = !DisplayReadOnly;
    //                ProvNote.Visible = !DisplayReadOnly;
    //            }
    //            btDelete = (Button)e.Row.Cells[3].FindControl("btnDltProfGrid");
    //        }
    //        else if (ClaimType == CON.ClaimsType.Professional)
    //        {
    //            gvProviderNote.Columns[2].Visible = !DisplayReadOnly;
    //            gvProviderNote.Columns[3].Visible = !DisplayReadOnly;
    //            if (DisplayReadOnly == true)
    //            {
    //                gvProviderNote.Columns[2].Visible = !DisplayReadOnly;
    //                gvProviderNote.Columns[3].Visible = !DisplayReadOnly;
    //                gvProviderNote.Columns[4].Visible = !DisplayReadOnly;
    //                ProvNote.Visible = !DisplayReadOnly;
    //            }
    //            btDelete = (Button)e.Row.Cells[3].FindControl("btnDltProfGrid");
    //        }
    //        if (btDelete != null)
    //            btDelete.Visible = false;
    //    }


    //}

    //protected void gvProviderNote_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    string keyVal;
    //    GridViewRow editingRow = null;
    //    if (hdnClaim_Type_ID.Value == CON.ClaimsType.Dental)
    //    {
    //        keyVal = gvProviderNote.DataKeys[e.NewEditIndex].Value.ToString();
    //        gvProviderNote.EditIndex = e.NewEditIndex;
    //        editingRow = gvProviderNote.Rows[e.NewEditIndex];
    //    }

    //    else if (hdnClaim_Type_ID.Value == CON.ClaimsType.Institutional)
    //    {
    //        keyVal = gvProviderNotesInstl.DataKeys[e.NewEditIndex].Value.ToString();
    //        gvProviderNotesInstl.EditIndex = e.NewEditIndex;
    //        editingRow = gvProviderNotesInstl.Rows[e.NewEditIndex];
    //    }
    //    LoadProviderNotes();
    //}

    //protected void gvProviderNote_RowCancelingEdit(object sender, EventArgs e)
    //{
    //    gvProviderNote.EditIndex = -1;
    //    gvProviderNotesInstl.EditIndex = -1;
    //    LoadProviderNotes();
    //}


    //protected void gvProvidersNote_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    int index = -1;
    //    string newNote;
    //    Dictionary<string, string> parms = new Dictionary<string, string>();

    //    if (hdnClaim_Type_ID.Value == CON.ClaimsType.Dental)
    //    {
    //        index = Convert.ToInt32(gvProviderNote.DataKeys[e.RowIndex].Value);
    //        newNote = e.NewValues["Note"].ToString();
    //        parms.Add("Notes", newNote);
    //        parms.Add("Claim_Type", hdnClaim_Type_ID.Value.ToString());
    //        parms.Add("Note_Reference_Code_ID", null);
    //    }
    //    else if (hdnClaim_Type_ID.Value == CON.ClaimsType.Institutional)
    //    {
    //        index = Convert.ToInt32(gvProviderNotesInstl.DataKeys[e.RowIndex].Value);
    //        DropDownList DropDownReferenceCode = (DropDownList)gvProviderNotesInstl.Rows[e.RowIndex].FindControl("DropDownReferenceCode");

    //        newNote = e.NewValues["Note"].ToString();
    //        parms.Add("Notes", newNote);
    //        parms.Add("Claim_Type", hdnClaim_Type_ID.Value.ToString());
    //        parms.Add("Note_Reference_Code_ID", DropDownReferenceCode.SelectedValue.ToString());
    //    }
    //    parms.Add("Claim_ID", hdnClaim_ID.Value.ToString());
    //    parms.Add("Claims_Providers_Note_ID", index.ToString());
    //    parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
    //    parms.Add("Last_Modified_Date", DateTime.Now.ToString());
    //    try
    //    {
    //        svc.UpdatePanelsData("Claims_Providers_Note", parms);
    //    }
    //    catch (Exception ex) { }

    //    gvProviderNote.EditIndex = -1;
    //    gvProviderNotesInstl.EditIndex = -1;
    //    parms.Clear();
    //    LoadProviderNotes();

    //}


    //protected void gvProviderNote_RowDeleting(object source, GridViewDeleteEventArgs e)
    //{
    //    int index = 0;
    //    Dictionary<string, string> parms = new Dictionary<string, string>();

    //    if (hdnClaim_Type_ID.Value == CON.ClaimsType.Dental)
    //    {
    //        index = Convert.ToInt32(gvProviderNote.DataKeys[e.RowIndex].Value.ToString());
    //    }
    //    else if (hdnClaim_Type_ID.Value == CON.ClaimsType.Institutional)
    //    {
    //        index = Convert.ToInt32(gvProviderNotesInstl.DataKeys[e.RowIndex].Value.ToString());
    //    }
    //    try
    //    {
    //        svc.DeletePanelsData("Claims_Providers_Note", "Claims_Providers_Note_ID", index);
    //    }
    //    catch (Exception ex) { }
    //    LoadProviderNotes();
    //}

    #endregion


    #region 'Panel Level Save: Dental & Institutional'
    protected void btnProviderNoteAdd_Click(object sender, EventArgs e)
    {

        lblErrorText.Text = "";
        if (ValidateProviderNotes())
        {
            int max = 0;
            Dictionary<string, string> parm = new Dictionary<string, string>();
            parm.Add("Claim_ID", hdnClaim_ID.Value.ToString());
            parm.Add("Claim_Type", hdnClaim_Type_ID.Value.ToString());
            dsProvNotes = svc.SelectPanelsData("Claims_Providers_Note", parm);
            if (Helper.HasRows(dsProvNotes))

            {
                max = dsProvNotes.Tables[0].Rows.Count;
            }

            Dictionary<string, string> parms = new Dictionary<string, string>();
            if (hdnClaim_Type_ID.Value == CON.ClaimsType.Dental)  //Dental
            {

                parms.Add("line", (max + 1).ToString("D2"));
                parms.Add("Note_ID", null);
                parms.Add("Claim_Type", hdnClaim_Type_ID.Value.ToString());
            }
            else if (hdnClaim_Type_ID.Value == CON.ClaimsType.Institutional) //Institutional.
            {

                parms.Add("line", (max + 1).ToString("D2"));
                parms.Add("Note_ID", ddlNoteRefCode.SelectedValue.ToString());
                parms.Add("Claim_Type", hdnClaim_Type_ID.Value.ToString());
            }

            InsertProviderNote(parms);
        }

    }

    protected void InsertProviderNote(Dictionary<string, string> parms)
    {
        parms.Add("Note", txtProviderNotes.Text.ToString());
        parms.Add("Claim_ID", hdnClaim_ID.Value.ToString());
        parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("Last_Modified_Date", DateTime.Now.ToString());
        parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("Created_Date_Time", DateTime.Now.ToString());
        parms.Add("IS_BILLING_NOTE", "0");
        try
        {
            svc.InsertPanelsData("Claims_Providers_Note", parms);
        }
        catch (Exception ex)
        {

        }
        parms.Clear();
        // LoadProviderNotes();
        txtProviderNotes.Text = "";
        ddlNoteRefCode.ClearSelection();
    }

    #endregion

    #region 'Page Level Save: Professional'
    public void btnSave_Click(int actionButton)
    {
        if (actionButton == CON.ActionButtonType.Submit)
        {
            if (((!string.IsNullOrEmpty(ClaimType) && !string.IsNullOrEmpty(ClaimID)) || !string.IsNullOrEmpty(IcnNumber)) && ClaimType == CON.ClaimsType.Professional)
            {
                if (ValidateProviderNotes())
                    Save();
                else
                    DisplayErrorMessage("SaveFailure");
            }
        }
        else
        {
            Save();
        }

    }
    private void Save()
    {
        string ProviderNote_ID = "";
        lblErrorText.Text = "";
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet dsRenderData = GetProviderNotes();

        if (Helper.HasRows(dsRenderData))
        {
            //txtProviderNotes.Text = dsRenderData.Tables[0].Rows[0]["Note"].ToString();
            //ddlNoteRefCode.SelectedValue = dsRenderData.Tables[0].Rows[0]["Note_ID"].ToString();
            ProviderNote_ID = dsRenderData.Tables[0].Rows[0]["Claims_Providers_Note_ID"].ToString();

            parms.Add("Note_Reference_Code_ID", ddlNoteRefCode.SelectedValue.ToString());
            parms.Add("Notes", txtProviderNotes.Text);
            parms.Add("Claim_Type", hdnClaim_Type_ID.Value.ToString());
            parms.Add("Claim_ID", hdnClaim_ID.Value.ToString());
            parms.Add("Claims_Providers_Note_ID", ProviderNote_ID);
            parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
            try
            {
                svc.UpdatePanelsData("Claims_Providers_Note", parms);
            }
            catch (Exception ex) { }
            parms.Clear();
            dsRenderData.Clear();

            dsRenderData = GetProviderNotes();
            dsRenderData = GetProviderNotes();
            if (Helper.HasRows(dsRenderData))
            {
                txtProviderNotes.Text = dsRenderData.Tables[0].Rows[0]["Note"].ToString();
                ddlNoteRefCode.SelectedItem.Text = dsRenderData.Tables[0].Rows[0]["NoteReferenceCode"].ToString();
            }
            else
            {
                txtProviderNotes.Text = "";
                ddlNoteRefCode.ClearSelection();
            }
        }
        else
        {
            //SAVE
            parms.Add("Line", null);
            parms.Add("Note_ID", ddlNoteRefCode.SelectedValue.ToString()); //Note_Ref_ID
            parms.Add("Claim_Type", hdnClaim_Type_ID.Value.ToString());
            parms.Add("Note", txtProviderNotes.Text.ToString());
            parms.Add("Claim_ID", hdnClaim_ID.Value.ToString());
            parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
            parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("Created_Date_Time", DateTime.Now.ToString());
            parms.Add("IS_BILLING_NOTE", "0");
            try
            {
                svc.InsertPanelsData("Claims_Providers_Note", parms);
            }
            catch (Exception ex)
            {

            }
            parms.Clear();
            dsRenderData.Clear();

            dsRenderData = GetProviderNotes();
            if (Helper.HasRows(dsRenderData))
            {
                txtProviderNotes.Text = dsRenderData.Tables[0].Rows[0]["Note"].ToString();
                ddlNoteRefCode.SelectedItem.Text = dsRenderData.Tables[0].Rows[0]["NoteReferenceCode"].ToString();
            }
            else
            {
                txtProviderNotes.Text = "";
                ddlNoteRefCode.ClearSelection();
            }
        }
        GetProvNotes(dsRenderData);
    }
    #endregion


    #region 'Validation'
    private bool ValidateProviderNotes()
    {
        Page.Validate("valProviderNote");
        if (Page.IsValid)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void DisplayErrorMessage(string errorType)
    {
        if (errorType == "SaveFailure")
            lblErrorText.Text = "Failed to save the data !";
    }
    #endregion


    //private void BindProviderNoteGrids(DataSet dsProviderNotes)
    //{
    //    if (hdnClaim_Type_ID.Value == CON.ClaimsType.Dental)
    //    {
    //        if (Helper.HasRows(dsProviderNotes))
    //        {
    //            gvProviderNote.DataSource = dsProviderNotes.Tables[0];
    //            gvProviderNote.DataBind();
    //        }
    //        else
    //        {
    //            gvProviderNote.DataSource = null;
    //            gvProviderNote.DataBind();
    //        }
    //        divProviderNotesInstGrid.Visible = false;
    //        divProviderNotesGrid.Visible = true;

    //    }
    //    else if (hdnClaim_Type_ID.Value == CON.ClaimsType.Institutional)
    //    {
    //        if (Helper.HasRows(dsProviderNotes))
    //        {
    //            gvProviderNotesInstl.DataSource = dsProviderNotes.Tables[0];
    //            gvProviderNotesInstl.DataBind();
    //        }
    //        else
    //        {
    //            gvProviderNotesInstl.DataSource = null;
    //            gvProviderNotesInstl.DataBind();
    //        }
    //        divProviderNotesGrid.Visible = false;
    //        divProviderNotesInstGrid.Visible = true;
    //    }
    //    else if (hdnClaim_Type_ID.Value == CON.ClaimsType.Professional)
    //    {
    //        divProviderNotesGrid.Visible = divProviderNotesInstGrid.Visible = false;
    //    }
    //}

    //private void LoadProviderNotes()
    //{
    //    DataSet dsProviderNotes = null;
    //    dsProviderNotes = GetProviderNotes();
    //    try
    //    {
    //        if ((hdnClaim_Type_ID.Value == CON.ClaimsType.Professional) && (Helper.HasRows(dsProviderNotes)))
    //        {
    //            ddlNoteRefCode.SelectedValue = dsProviderNotes.Tables[0].Rows[0]["Note_ID"].ToString();
    //            txtProviderNotes.Text = dsProviderNotes.Tables[0].Rows[0]["Note"].ToString();
    //        }
    //        BindProviderNoteGrids(dsProviderNotes);
    //        ValidateGridCount(dsProviderNotes);
    //    }
    //    catch (Exception e) { }
    //}

    public void LoadNotesReferenceCode(string claimType)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_Type", claimType);
        parms.Add("IS_BILLING_NOTE", "0");
        DataSet dsNoteRefCode = svc.SelectPanelsData("NoteReferenceCode", parms);

        if (Helper.HasRows(dsNoteRefCode))
        {
            Helper.LoadList(ddlNoteRefCode, dsNoteRefCode, "NoteReferenceCode", "Note_ID", true);
        }
    }

    private void ValidateGridCount(DataSet dsProviderNotes)
    {
        if (hdnClaim_Type_ID.Value == CON.ClaimsType.Dental && dsProviderNotes.Tables[0].Rows.Count >= 5)
        {
            lblGridLimitMsg.Visible = true;
            divTxtProviderNotes.Visible = false;
            divDdlReferenceCode.Visible = false;
            btnProviderNoteAdd.Visible = false;
        }
        else
        {
            lblGridLimitMsg.Visible = false;
            divTxtProviderNotes.Visible = true;
            if (hdnClaim_Type_ID.Value != CON.ClaimsType.Dental)
                divDdlReferenceCode.Visible = true;
            else
                divDdlReferenceCode.Visible = false;
            btnProviderNoteAdd.Visible = true;

            if (hdnClaim_Type_ID.Value == CON.ClaimsType.Professional)
            {
                divProviderNotesHeader.Visible = true;
                lblNoteReferenceCode.Visible = lblNote.Visible = divBtnProviderNotes.Visible = false;
            }
        }
        if (hdnClaim_Type_ID.Value == CON.ClaimsType.Institutional && dsProviderNotes.Tables[0].Rows.Count >= 10)
        {
            lblGridLimitMsg.Text = "Maximum of 10 notes can be added";
            lblGridLimitMsg.Visible = true;
            divTxtProviderNotes.Visible = false;
            divDdlReferenceCode.Visible = false;
            btnProviderNoteAdd.Visible = false;
        }
    }



    private DataSet GetProviderNotes()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_ID", hdnClaim_ID.Value.ToString());
        parms.Add("Claim_Type", hdnClaim_Type_ID.Value.ToString());
        parms.Add("IS_BILLING_NOTE", "0");
        dsProvNotes = svc.SelectPanelsData("Claims_Providers_Note", parms);
        return dsProvNotes;
    }

    #region 'Visibility'
    private void SetFieldsVisibility(string claimType)
    {
        try
        {
            if (claimType == CON.ClaimsType.Dental)
            {
                divDdlReferenceCode.Visible = false;
                divBtnProviderNotes.Visible = true;
                // BindProviderNoteGrids(GetProviderNotes());
                providerNotesOutput.Visible = true;
                divProviderNotesHeader.Visible = false;
            }

            else if (claimType == CON.ClaimsType.Professional)
            {
                providerNotesOutput.Visible = false;
                divDdlReferenceCode.Visible = true;
                divBtnProviderNotes.Visible = false;
                divProviderNotesHeader.Visible = true;
                lblNoteReferenceCode.Visible = false;
                lblNote.Visible = false;
                LoadNotesReferenceCode(claimType);
                DataSet dsRenderData = GetProviderNotes();
                if (Helper.HasRows(dsRenderData))
                {
                    //render
                    txtProviderNotes.Text = dsRenderData.Tables[0].Rows[0]["Note"].ToString();
                    ddlNoteRefCode.SelectedValue = dsRenderData.Tables[0].Rows[0]["Note_ID"].ToString();
                }
                else
                {
                    txtProviderNotes.Text = "";
                    ddlNoteRefCode.ClearSelection();
                }
            }
            else
            {
                providerNotesOutput.Visible = true;
                divDdlReferenceCode.Visible = true;
                divBtnProviderNotes.Visible = true;
                divProviderNotesHeader.Visible = false;
                lblNoteReferenceCode.Visible = true;
                lblNote.Visible = true;
                LoadNotesReferenceCode(claimType);
                //  BindProviderNoteGrids(GetProviderNotes());
            }
        }
        catch { }
    }
    #endregion


    #region 'Invoked By Property Setter'
    private void GetProvNotes(DataSet dataSetProvNotes)
    {
        DataSet dsProvNotes = new DataSet();
        if (!string.IsNullOrEmpty(hdnClaim_ID.Value) || !string.IsNullOrEmpty(IcnNumber))
        {
            if (Helper.HasRows(dataSetProvNotes))
            {
                dsProvNotes = dataSetProvNotes;
            }
            else
            {
                dsProvNotes = GetProviderNotes();
            }
        }

        if (Helper.HasRows(dsProvNotes))
        {
            if (hdnClaim_Type_ID.Value == CON.ClaimsType.Dental || hdnClaim_Type_ID.Value == CON.ClaimsType.Institutional)
            {
                DataTable dtProviderNotes = new DataTable();
                dtProviderNotes = dsProvNotes.Tables[0];

                if (dtProviderNotes.Rows.Count > 0)
                {
                    string table = string.Empty;
                    if (hdnClaim_Type_ID.Value == CON.ClaimsType.Dental)
                    {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>*Note</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    }
                    else
                    {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>*Note Reference Code</th><th style='width:10px; scope='col'>*Note</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    }

                    foreach (DataRow dr in dtProviderNotes.Rows)
                    {
                        string notes = dr["Note"].ToString();
                        string providerNoteID = dr["Claims_Providers_Note_ID"].ToString();
                        string Claim_ID = hdnClaim_ID.Value;
                        string sequence = dr["line"].ToString();
                        if (dtProviderNotes.Rows.Count >= 10)
                        {
                            lblErrorText.Text = "Maximum of 10 notes can be added";
                            btnProviderNoteAdd.Visible = false;
                        }
                        else
                        {
                            lblErrorText.Text = "";
                            btnProviderNoteAdd.Visible = true;
                        }

                        if (Session["ClaimStatus"].ToString() == "Pending Submission")
                        {
                            if (hdnClaim_Type_ID.Value == CON.ClaimsType.Dental)
                            {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' style='width:200px' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditProviderNoteLineItem(\"" + providerNoteID + "\",\"" + Claim_ID + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:30px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderNoteLineitem(\"" + providerNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            }
                            else
                            {
                                string noteRefCode = dr["NoteReferenceCode"].ToString(); 
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + noteRefCode + "</span></td><td><span style='width:250px' title='Line' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditProviderNoteLineItem(\"" + providerNoteID + "\",\"" + Claim_ID + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderNoteLineitem(\"" + providerNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            }
                            hdnProviderClaimStatus.Value = "Pending Submission";
                            //providerNotesOutput.Visible = true;

                        }
                        else
                        {
                            if (hdnClaim_Type_ID.Value == CON.ClaimsType.Dental)
                            {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' style='width:200px' class='tNumber'>" + notes + "</span></td></tr >";
                            }
                            else
                            {
                                string noteRefCode = dr["NoteReferenceCode"].ToString();
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + noteRefCode + "</span></td><td><span style='width:250px' title='Line' class='tNumber'>" + notes + "</span></td></tr >";
                            }

                            hdnProviderClaimStatus.Value = "Other";
                            //providerNotesOutput.Visible = false;
                        }
                    }
                    table = table + "</tbody></table>";
                    providerNotesOutput.InnerHtml = table;
                    providerNotesOutput.Visible = true;

                }

            }
            else if (hdnClaim_Type_ID.Value == CON.ClaimsType.Professional)
            {
                providerNotesOutput.Visible = false;
                txtProviderNotes.Text = dsProvNotes.Tables[0].Rows[0]["Note"].ToString();
                ddlNoteRefCode.SelectedValue = GetNoteReferenceCode(dsProvNotes.Tables[0].Rows[0]["NoteReferenceCode"].ToString());
            }
        }
        else
        {
            providerNotesOutput.InnerHtml = "";
        }
    }
    #endregion


    #region 'Clear Fields'
    public void ClearProviderNotesFields()
    {
        txtProviderNotes.Text = "";
        ddlNoteRefCode.ClearSelection();
        providerNotesOutput.InnerHtml = "";
    }

    public void ClearGrid()
    {
        //  gvProviderNote.DataSource = null;
        // gvProviderNote.DataBind();

        //  gvProviderNotesInstl.DataSource = null;
        // gvProviderNotesInstl.DataBind();
    }
    #endregion

    #region 'ReadOnlyFields'
    private void SetReadOnlyFieldsControl(bool value)
    {
        if (ClaimType == CON.ClaimsType.Professional)
            txtProviderNotes.ReadOnly = value;
    }
    #endregion

    #region 'Invoked By Display read only'
    private void BindGrid()
    {
        if (DisplayReadOnly)
            divProvNotes.Visible = !DisplayReadOnly;
        else
            divProvNotes.Visible = !DisplayReadOnly;

        if (Helper.HasRows(dataSetProviderNotes))
        {
            if (hdnClaim_Type_ID.Value == CON.ClaimsType.Dental)
            {
                //  gvProviderNote.DataSource = dataSetProviderNotes;
                //gvProviderNote.DataBind();
            }

            else if (hdnClaim_Type_ID.Value == CON.ClaimsType.Institutional)
            {
                //  gvProviderNotesInstl.DataSource = dataSetProviderNotes.Tables[0];
                // gvProviderNotesInstl.DataBind();
            }
        }
    }
    #endregion

    #region 'Set Service data for professional'
    private void SetProfessionalPanelData(DataSet ds)
    {
        divProviderNotesHeader.Visible = true;
        divProvNotes.Visible = true;
        ddlNoteRefCode.Enabled = false;
        divTxtProviderNotes.Visible = true;
        divBtnProviderNotes.Visible = false;
        txtProviderNotes.Enabled = false;
        divProviderNotesGrid.Visible = false;
        string providerNotesHTML = "<div>";
        if (ClaimType == CON.ClaimsType.Professional && Helper.HasRows(ds))
        {
            txtProviderNotes.Text = Helper.GetString("Note", ds.Tables[0].Rows[0]);
            txtProviderNotes.Visible = true;
            ddlNoteRefCode.SelectedItem.Text = Helper.GetString("NoteReferenceCode", ds.Tables[0].Rows[0]);
            ddlNoteRefCode.Visible = true;
            divProviderNotesHeader.Visible = true;
            providerNotesOutput.Visible = true;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                providerNotesHTML += "<div class=\"col-sm-5\">" + Helper.GetString("NoteReferenceCode", dr) + "</div><div class=\"col-sm-5\">" + Helper.GetString("Note", dr) + "</div>";
            }
        }
        else
        {
            txtProviderNotes.Text = String.Empty;
            divProviderNotesHeader.Visible = true;
        }
        providerNotesHTML += "</div>";
        divProviderNotesGrid.InnerHtml = providerNotesHTML;
        divProviderNotesGrid.Visible = true;
    }
    #endregion

    public void SaveToDbOnAdjustonDental(DataTable dt)
    {
        if (Helper.HasRows(dt))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claim_ID", hdnClaim_ID.Value.ToString());
                parms.Add("Claim_Type", hdnClaim_Type_ID.Value.ToString());
                parms.Add("line", dt.Rows[i]["line"].ToString());
                parms.Add("Note", dt.Rows[i]["Note"].ToString());
                parms.Add("Note_ID", GetNoteReferenceCode(dt.Rows[i]["NoteReferenceCode"].ToString()));
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("IS_BILLING_NOTE", "0");
                try
                {
                    svc.InsertPanelsData("Claims_Providers_Note", parms);
                }
                catch (Exception ex)
                {

                }
                //  LoadProviderNotes();
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                GetProvNotes(null);
            }
        }
    }
    private string GetNoteReferenceCode(string noteReferenceCode)
    {
        string name = string.Empty;
        if (!string.IsNullOrWhiteSpace(noteReferenceCode) && Helper.HasRows(this.WorkflowPage.NoteReferenceCodeTable.Tables[0]))
        {
            var dt1 = this.WorkflowPage.NoteReferenceCodeTable.Tables[0].AsEnumerable().Where(r => r.Field<String>("NoteReferenceCode") == noteReferenceCode);
            if (dt1.Any())
            {
                var newDt = dt1.CopyToDataTable();
                if (Helper.HasRows(newDt))
                {
                    string abbrev = newDt.Rows[0]["NoteReferenceServiceCode"].ToString();
                    name = newDt.Rows[0]["Note_ID"].ToString();
                    if (abbrev == "DCP" && hdnClaim_Type_ID.Value == CON.ClaimsType.Professional)
                    {
                        name = "17";
                    }
                    if(abbrev == "DCP" && hdnClaim_Type_ID.Value == CON.ClaimsType.Institutional)
                    {
                        name = "2";
                    }                   
                }
            }
        }
        return name;
    }
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
}

