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

public partial class PopupControls_ClaimsProviderBillingNotes : System.Web.UI.UserControl
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

    public DataSet dataSetProviderBillingNotes
    {
        get
        {
            if (!Helper.HasRows(dsProvNotes))
            {
                if (Helper.HasRows(GetProviderBillingNotes()))
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
            if (!string.IsNullOrWhiteSpace(ddlBillingNoteRefCode.SelectedValue))
                return ddlBillingNoteRefCode.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && ddlBillingNoteRefCode.Items.FindByValue(value) != null)
            {
                ddlBillingNoteRefCode.ClearSelection();
                ddlBillingNoteRefCode.SelectedValue = value.Trim();
            }
        }
    }

    public string ProviderBillingNote
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtProviderBillingNotes.Text))
                return txtProviderBillingNotes.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtProviderBillingNotes.Text = value.Trim();
            else
                txtProviderBillingNotes.Text = string.Empty;
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
            if (!string.IsNullOrWhiteSpace(ddlBillingNoteRefCode.SelectedItem.Text))
                return ddlBillingNoteRefCode.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlBillingNoteRefCode.ClearSelection();
                ddlBillingNoteRefCode.SelectedItem.Text = value.Trim();
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

    //public DataSet SetProfessionalServiceData
    //{
    //    set
    //    {
    //        SetProfessionalPanelData(value);
    //    }
    //}

    #endregion

    public bool HasInputValue()
    {
        bool rtn = false;
        if ((ddlBillingNoteRefCode.SelectedIndex > 0) || !string.IsNullOrEmpty(txtProviderBillingNotes.Text))
        {
            rtn = true;
        }

        return rtn;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //  LoadProviderBillingNotes();

        }
        btnProviderBillingNoteAdd.Attributes.Add("onclick", "ProviderDisableEnableConditionAddButton();");
        SetButtonVisibility();
        if (providerBillingNotesOutput.Visible == true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>displaytableProviderBillingNote();</script>", false);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>ProviderBillingNotesClearFields();</script>", false);
        }
        ddlBillingNoteRefCode.Enabled = true;
        txtProviderBillingNotes.Enabled = true;
        if (Session["ClaimStatus"] != null)
        {
            if (Session["ClaimStatus"].ToString() == "Pending Submission" && providerBillingNotesOutput.Visible == true)
            {
                hdnProviderClaimStatus.Value = "Pending Submission";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>displaytableProviderBillingNote();</script>", false);
            }
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

    public void LoadNotesReferenceCode(string claimType)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_Type", claimType);
        parms.Add("IS_BILLING_NOTE", "1");
        DataSet dsNoteRefCode = svc.SelectPanelsData("NoteReferenceCode", parms);

        if (Helper.HasRows(dsNoteRefCode))
        {
            Helper.LoadList(ddlBillingNoteRefCode, dsNoteRefCode, "NoteReferenceCode", "Note_ID", true);
        }
    }

    private DataSet GetProviderBillingNotes()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_ID", hdnClaim_ID.Value.ToString());
        parms.Add("Claim_Type", hdnClaim_Type_ID.Value.ToString());
        parms.Add("IS_BILLING_NOTE", "1");
        dsProvNotes = svc.SelectPanelsData("Claims_Providers_Note", parms);
        return dsProvNotes;
    }

    #region 'Visibility'
    private void SetFieldsVisibility(string claimType)
    {
        try
        {
          
            if(claimType == CON.ClaimsType.Institutional)
            {
                providerBillingNotesOutput.Visible = true;
                divDdlReferenceCode.Visible = true;
                divBtnProviderBillingNotes.Visible = true;
                divProviderBillingNotesHeader.Visible = false;
                lblNoteReferenceCode.Visible = true;
                lblNote.Visible = true;
                LoadNotesReferenceCode(claimType);
                //  BindProviderBillingNoteGrids(GetProviderBillingNotes());
            }
            else
            {
                providerBillingNotesOutput.Visible = false;
                divDdlReferenceCode.Visible = false;
                divBtnProviderBillingNotes.Visible = false;
                divProviderBillingNotesHeader.Visible = false;
                lblNoteReferenceCode.Visible = false;
                lblNote.Visible = false;
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
                dsProvNotes = GetProviderBillingNotes();
            }
        }

        if (Helper.HasRows(dsProvNotes))
        {
            if (hdnClaim_Type_ID.Value == CON.ClaimsType.Dental || hdnClaim_Type_ID.Value == CON.ClaimsType.Institutional)
            {
                DataTable dtProviderBillingNotes = new DataTable();
                dtProviderBillingNotes = dsProvNotes.Tables[0];

                if (dtProviderBillingNotes.Rows.Count > 0)
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

                    foreach (DataRow dr in dtProviderBillingNotes.Rows)
                    {
                        string notes = dr["Note"].ToString();
                        string ProviderBillingNoteID = dr["Claims_Providers_Note_ID"].ToString();
                        string Claim_ID = hdnClaim_ID.Value;
                        string sequence = dr["line"].ToString();
                        if (dtProviderBillingNotes.Rows.Count >= 10)
                        {
                            lblErrorText.Text = "Maximum of 10 notes can be added";
                            btnProviderBillingNoteAdd.Visible = false;
                        }
                        else
                        {
                            lblErrorText.Text = "";
                            btnProviderBillingNoteAdd.Visible = true;
                        }
                        if (Session["ClaimStatus"] != null)
                        {
                            if (Session["ClaimStatus"].ToString() == "Pending Submission")
                            {
                                if (hdnClaim_Type_ID.Value == CON.ClaimsType.Dental)
                                {
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' style='width:200px' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditProviderBillingNoteLineItem(\"" + ProviderBillingNoteID + "\",\"" + Claim_ID + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:30px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderBillingNoteLineitem(\"" + ProviderBillingNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                }
                                else
                                {
                                    string noteRefCode = dr["NoteReferenceCode"].ToString();
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + noteRefCode + "</span></td><td><span style='width:250px' title='Line' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditProviderBillingNoteLineItem(\"" + ProviderBillingNoteID + "\",\"" + Claim_ID + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderBillingNoteLineitem(\"" + ProviderBillingNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                }
                                hdnProviderClaimStatus.Value = "Pending Submission";
                                //providerBillingNotesOutput.Visible = true;

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
                                //providerBillingNotesOutput.Visible = false;
                            }
                        }

                    }
                    table = table + "</tbody></table>";
                    providerBillingNotesOutput.InnerHtml = table;
                    providerBillingNotesOutput.Visible = true;

                }

            }
            else if (hdnClaim_Type_ID.Value == CON.ClaimsType.Professional)
            {
                providerBillingNotesOutput.Visible = false;
                txtProviderBillingNotes.Text = dsProvNotes.Tables[0].Rows[0]["Note"].ToString();
                ddlBillingNoteRefCode.SelectedValue = GetNoteReferenceCode(dsProvNotes.Tables[0].Rows[0]["NoteReferenceCode"].ToString());
            }
        }
        else
        {
            providerBillingNotesOutput.InnerHtml = "";
        }
    }
    #endregion
    public void ClearProviderBillingNotesFields()
    {
        txtProviderBillingNotes.Text = "";
        ddlBillingNoteRefCode.ClearSelection();
        providerBillingNotesOutput.InnerHtml = "";
    }

    #region 'ReadOnlyFields'
    private void SetReadOnlyFieldsControl(bool value)
    {
        if (ClaimType == CON.ClaimsType.Professional)
            txtProviderBillingNotes.ReadOnly = value;
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
                parms.Add("IS_BILLING_NOTE", "1");
                try
                {
                    svc.InsertPanelsData("Claims_Providers_Note", parms);
                }
                catch (Exception ex)
                {

                }
                //  LoadProviderBillingNotes();
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
                    if (abbrev == "DCP" && hdnClaim_Type_ID.Value == CON.ClaimsType.Institutional)
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