using MAXIMUS.Models.Data.PDMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_DisclosureQuestion : System.Web.UI.UserControl
{
    public string Section { get; set; }
    public string QuestionTypeId { get; set; }
    public int Option
    {
        get { return mltQuestion.ActiveViewIndex; }
        set { mltQuestion.ActiveViewIndex = value; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    bool pageIsReadOnly = false;

    protected void Page_Load(object sender, EventArgs e)
    {       
        if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.OwnerInformation)
        {
            //btnSave.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSave.ValidationGroup +
                //"')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
            foreach (ListItem li in rblYesNo.Items) li.Attributes.Add("onclick", "javascript:QuestionTogglePanel('" +
                rblYesNo.ClientID + "','" + pnlQuestion.ClientID + "','" + hdnChanged.ClientID + "');");
            if (rblYesNo.SelectedIndex == 0) pnlQuestion.Attributes.Add("style", "display:block;");
            else pnlQuestion.Attributes.Add("style", "display:none;");
        }
        if (this.pageIsReadOnly)
        {
            // turn off radio buttons if read only
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (rblYesNo.SelectedIndex == 0)
            pnlQuestion.Attributes.Add("style", "display:block;");
        else
            pnlQuestion.Attributes.Add("style", "display:none;");
    }

    private void LoadGrid(BasePopupControl ctl, string tableName, GridView grd, ImageButton btnHistory)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        if (tableName == "SUBCONTRACTOR")
        {
            ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, tableName);
        }
        else if (tableName == "SUBCONTRACTOR5YRS")
        {
            ds = psc.SelectRegSubcontractors(this.WorkflowPage.RegistrationId,
            MAXIMUS.Core.Libraries.Constants.RegistrationSubcontractorType.BusinessDoneLast5Years);
        }
        else if (tableName == "OWNER_CONVICTION")
        {
            ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, tableName);
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    string Ownerid = dr["REG_OWNER_ID"].ToString();
            //    DataSet dset = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OWNER"); ;
            //foreach (DataRow drow in dset.Tables[0].Rows)
            //{
            //    if (Helper.GetString("REG_OWNER_ID", drow) == Ownerid)
            //    {
            //        if (Helper.GetString("REG_OWNER_TYPE_ID", drow) == CON.OwnerType.ManagingEmployee || Helper.GetString("REG_OWNER_TYPE_ID", drow) == CON.OwnerType.Person)
            //        {
            //            string maskedSSN = MaskSSN(dr["TAX_ID"].ToString());
            //            dr["TAX_ID"] = maskedSSN;
            //            break;
            //        }
            //    }
            //}

            //}
            //ds.Tables[0].AcceptChanges();
        }
        else
        {
            ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, tableName);
        }

        if (Helper.HasRows(ds))
        {
            grd.DataSource = ctl.DataList = ds.Tables[0];
        }
        else
        {
            grd.DataSource = ctl.DataList = null;
        }
        grd.DataBind();
        btnHistory.Visible = grd.Rows.Count > 0;
    }

    private string MaskSSN(string unMaskSSN)
    {
        string MaskedSSN = "";
        string unMaskedSSN = unMaskSSN.Insert(5, "-").Insert(3, "-");
        if (String.IsNullOrEmpty(unMaskedSSN))
            return string.Empty;

        if (unMaskedSSN.Length <= 5)
            return unMaskedSSN;

        string last5 = unMaskedSSN.Substring(unMaskedSSN.Length - 5, 5);
        var maskedChars = new StringBuilder();
        for (int i = 0; i < unMaskedSSN.Length - 5; i++)
        {
            maskedChars.Append(unMaskedSSN[i] == '-' ? "-" : "#");
        }
        MaskedSSN = maskedChars + last5;
        return MaskedSSN;

    }

    private void LoadQuestion()
    {
        DataRow qstRow = null;
        foreach (DataRow row in ApplicationCache.QuestionTypes().Rows)
        {
            if (Helper.GetString("QUESTION_TYPE_ID", row) == QuestionTypeId)
            {
                qstRow = row;
                break;
            }
        }
        if (qstRow != null)
        {

            // Example: fetch the stored HTML from the DB
            var htmlFromDb = Helper.GetString("QUESTION_TEXT", qstRow);

            // If the DB stored HTML as escaped entities (e.g., &lt;p&gt;), decode it:
            htmlFromDb = Server.HtmlDecode(htmlFromDb);

            // Wrap to preserve whitespace while rendering formatting
            lblQuestion.Text = string.Format(
                "<div class=\"preserve-whitespace\">{0}</div>",
                htmlFromDb
            );

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "QUESTION");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (Helper.GetString("QUESTION_TYPE_ID", row) == QuestionTypeId)
                {
                    rblYesNo.SelectedIndex = (Helper.GetInt("RESPONSE", row) > 0 ? 0 : 1);
                    break;
                }
            }
        }
        foreach (ListItem li in rblYesNo.Items) li.Attributes.Add("onclick", "javascript:QuestionTogglePanel('" +
            rblYesNo.ClientID + "','" + pnlQuestion.ClientID + "','" + hdnChanged.ClientID + "');");
        if (rblYesNo.SelectedIndex == 0) pnlQuestion.Attributes.Add("style", "display:block;");
        else pnlQuestion.Attributes.Add("style", "display:none;");

    }

    public void LoadData()
    {        
        LoadQuestion();
        switch (mltQuestion.ActiveViewIndex)
        {
            case 1:
                //LoadGrid(ucOwnerRelationships, "DISCLOSURES", grdOwnerRelationships, btnHistoryRelationships);
                //LoadOwnerXref();
                break;
            case 2:
                //LoadGrid(ucOwnerOtherInfo, "OWNER_OTHER", grdOwnerOtherInfo, btnHistoryOtherInfo);
                break;
            case 3:
                //LoadGrid(ucOwnerOtherInfo, "OWNER_OTHER", grdOwnerOtherInfo, btnHistoryOtherInfo);
                break;
            case 4:
                //LoadGrid(ucOwnerConviction, "OWNER_CONVICTION", grdConviction, btnHistoryConviction);
                break;
            case 5:
                //LoadGrid(ucOwnerConvictionOnBehalf, "OWNER_CONVICTION_ON_BEHALF", grdConvictionOnBehalf, btnHistoryConvictionOnBehalf);
                break;
            case 6:
                //LoadGrid(ucOwnerResidency, "OWNER_RESIDENCY", grdResidency, btnHistoryResidency);
                break;
            case 7:
                //LoadGrid(ucOwnerPenalty, "OWNER_SANCTION", grdPenalty, btnHistoryPenalty);
                break;
            case 8:
                //LoadGrid(ucOwnerOriginal, "ORIGINAL_OWNER", grdOriginalOwner, btnHistoryOriginalOwner);
                break;
            case 9:
                //LoadGrid(ucOwnerSubcontractor, "SUBCONTRACTOR", grdSubcontractor, btnHistorySubcontractor);
                break;
            case 10:
                //LoadGrid(ucOwnerSubcontractor5Years, "SUBCONTRACTOR5YRS", grdSubcontractor5Years, btnHistorySubcontractor5Years);
                break;
            case 11:
                //LoadGrid(ucOwnerSupplier, "SUPPLIER", grdSupplier, btnHistorySupplier);
                break;
            case 12:
                //LoadGrid(ucOwnerConvictionOnBehalf, "OWNER_CONVICTION_ON_BEHALF", grdConvictionOnBehalf, btnHistoryConvictionOnBehalf);
                break;
            case 13:
                //LoadGrid(ucOwnerTransaction, "OWNER_TRANSACTION", grdTransaction, btnHistoryTransaction);
                break;
            case 14:
                //LoadGrid(ucOwnerTransaction, "OWNER_TRANSACTION", grdTransaction, btnHistoryTransaction);
                break;
            case 15:
                //LoadGrid(ucOwnerTransaction, "OWNER_TRANSACTION", grdTransaction, btnHistoryTransaction);
                break;
            case 16:
                //LoadGrid(ucOwnerTransaction, "OWNER_TRANSACTION", grdTransaction, btnHistoryTransaction);
                break;
        }
    }

    public bool HasChanged()
    {
        return (hdnChanged.Value == "CHANGED");
    }

    private void SaveQuestionAnswer()
    {
    //    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
    //    psc.SaveRegistrationQuestion(this.WorkflowPage.RegistrationId, QuestionTypeId, (rblYesNo.SelectedIndex == 0 ? 1 : 0),
    //        MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed,
    //        Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
    }

    public void SaveData()
    {
        //// OHPNM-14088
        //if (this.WorkflowPage.OwnerInfoPageIsReadOnly)
        //{
        //    // this is a read only page; no need to validate or save
        //    return;
        //}

        //if (hdnChanged.Value != "CHANGED") return;

        //hdnChanged.Value = string.Empty;
        //PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        //switch (mltQuestion.ActiveViewIndex)
        //{
        //    case 1:
        //    case 2:
        //    case 3:
        //    case 4:
        //    case 5:
        //    case 6:
        //    case 7:
        //    case 8:
        //    case 9:
        //    case 10:
        //    case 11:
        //    case 12:
        //    case 13:
        //    case 14:
        //    case 15:
        //        if (rblYesNo.SelectedIndex >= 0) SaveQuestionAnswer();
        //        break;
        //}
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg + " (\"" + Section + "\")";
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    private int TotalGrid()
    {
        int rtn = 0;
        /*foreach (GridViewRow row in grdSubcontractoOwner.Rows)
        {
            try
            {
                rtn += Convert.ToInt32(row.Cells[1].Text);
            }
            catch { }
        }*/
        return rtn;
    }

    public void ValidateData(ref bool isGood)
    {
        //if (this.WorkflowPage.OwnerInfoPageIsReadOnly)
        //{
        //    // this is a read only page; no need to validate or save
        //    return;
        //}

        //if (rblYesNo.SelectedIndex < 0) AddError("*Select Yes or No", ref isGood);
        //else
        //    switch (mltQuestion.ActiveViewIndex)
        //    {
        //        //case 1:
        //        //    if (rblYesNo.SelectedIndex == 0 && grdOwnerRelationships.Rows.Count == 0) AddError("*Enter Owner Relationship Information", ref isGood);
        //        //    break;
        //    }
    }

    private DataRow GetRow(int index, string tableName)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, tableName);
        return ds.Tables[0].Rows[index];
    }



    private void AddDisclosures(string commandName)
    {
        // Common fields from the active panel
        var reg_disclosure = new REG_DISCLOSURES
        {
            REG_ID = this.WorkflowPage.RegistrationId,
            QUESTION_TYPE_ID = 1,
            INCIDENT_DATE = GetText(commandName, "txtDate"),
            STATE_CODE = GetSelected(commandName, "ddlState"),
            PROGRAM_AFFECTED = GetText(commandName, "txtProgram"),
            AGENCY_TAKING_ACTION = GetText(commandName, "txtAgency"),
            ACTION_TAKEN = GetText(commandName, "txtAction"),
            EXPLANATION_DETAILS = GetText(commandName, "txtExplanation")
        };

        var list = new List<REG_DISCLOSURES> { reg_disclosure };

        switch (commandName)
        {
            case "vwOpt1":
                //vwOpt1_grdRegDisclosures.DataSource = list;
                //vwOpt1_grdRegDisclosures.DataBind();

                // Clear inputs in vwOpt1
                ClearViewControls("vwOpt1");               // generic clear
                                                           // or ClearDisclosureFields("vwOpt1");     // targeted clear by naming convention
                break;

            case "vwOpt2":
                //vwOpt2_grdRegDisclosures.DataSource = list;
                //vwOpt2_grdRegDisclosures.DataBind();

                ClearViewControls("vwOpt2");
                break;

            case "vwOpt3":               

                ClearViewControls("vwOpt3");
                break;

            case "vwOpt4":
               
                ClearViewControls("vwOpt4");
                break;

            case "vwOpt5":
                
                ClearViewControls("vwOpt5");
                break;

            case "vwOpt6":
                
                ClearViewControls("vwOpt6");
                break;

            case "vwOpt7":
               
                ClearViewControls("vwOpt7");
                break;

            case "vwOpt8":
               
                ClearViewControls("vwOpt8");
                break;

            case "vwOpt9":
               
                ClearViewControls("vwOpt9");
                break;

            case "vwOpt10":
                
                ClearViewControls("vwOpt10");
                break;

            case "vwOpt11":
               
                ClearViewControls("vwOpt11");
                break;

            case "vwOpt12":
                
                ClearViewControls("vwOpt12");
                break;

            case "vwOpt13":
               
                ClearViewControls("vwOpt13");
                break;

            //case "vwOpt14":
            //    vwOpt14_grdRegDisclosures.DataSource = list; // or vwOpt14_grdRegDisclosures if you add it
            //    vwOpt14_grdRegDisclosures.DataBind();

            //    ClearViewControls("vwOpt14");
            //    break;

            //case "vwOpt15":
            //    vwOpt15_grdRegDisclosures.DataSource = list;
            //    vwOpt15_grdRegDisclosures.DataBind();

            //    ClearViewControls("vwOpt15");
            //    break;

            //case "vwOpt16":
            //    vwOpt16_grdRegDisclosures.DataSource = list;
            //    vwOpt16_grdRegDisclosures.DataBind();
            //    ClearViewControls("vwOpt16");
            //    break;

            default:
                break;
        }
    }

    private void ClearViewControls(string viewId)
    {
        SetText(viewId, "txtDate", string.Empty);
        SetSelectedValue(viewId, "ddlState", null);
        SetText(viewId, "txtProgram", string.Empty);
        SetText(viewId, "txtAgency", string.Empty);
        SetText(viewId, "txtAction", string.Empty);
        SetText(viewId, "txtExplanation", string.Empty);

        // Per-view extras:
        // vwOpt6 has ddlProgram:
        SetSelectedValue(viewId, "ddlProgram", null);
        // vwOpt7 has txtAgreement:
        SetText(viewId, "txtAgreement", string.Empty);
        // vwOpt9/10/11/12/13 have court/case/charge/county:
        SetText(viewId, "txtCourt", string.Empty);
        SetText(viewId, "txtCauseNumber", string.Empty);
        SetText(viewId, "txtCharge", string.Empty);
        SetSelectedValue(viewId, "ddlCounty", null);
    }


    private T GetViewControl<T>(string viewId, string controlId) where T : Control
    {
        var view = mltQuestion.FindControl(viewId) as View;
        return view?.FindControl(controlId) as T;
    }

    private string GetText(string viewId, string fieldName) =>
        GetViewControl<TextBox>(viewId, $"{viewId}_{fieldName}")?.Text ?? string.Empty;

    private string GetSelected(string viewId, string fieldName) =>
        GetViewControl<DropDownList>(viewId, $"{viewId}_{fieldName}")?.SelectedValue ?? string.Empty;


    /// <summary>
    /// Sets the Text of a TextBox inside the specified View.
    /// </summary>
    /// <returns>true if set; false if control not found.</returns>
    private bool SetText(string viewId, string fieldName, string newValue)
    {
        var tb = GetViewControl<TextBox>(viewId, $"{viewId}_{fieldName}");
        if (tb == null) return false;

        tb.Text = newValue ?? string.Empty;
        return true;
    }


    /// <summary>
    /// Sets the selected item of a DropDownList by Value.
    /// </summary>
    /// <param name="viewId">View ID (e.g., "vwOpt1").</param>
    /// <param name="fieldName">Field ID suffix (e.g., "ddlState").</param>
    /// <param name="value">The ListItem.Value to select.</param>
    /// <param name="addIfMissing">If true, adds a new ListItem(value, value) when not found.</param>
    /// <returns>true if selection set; false otherwise.</returns>
    private bool SetSelectedValue(string viewId, string fieldName, string value, bool addIfMissing = false)
    {
        var ddl = GetViewControl<DropDownList>(viewId, $"{viewId}_{fieldName}");
        if (ddl == null) return false;

        var safeValue = value?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(safeValue))
        {
            // Clear selection when empty value provided
            ddl.ClearSelection();
            return true;
        }

        var item = ddl.Items.FindByValue(safeValue);
        if (item != null)
        {
            ddl.ClearSelection();
            item.Selected = true;
            return true;
        }

        if (addIfMissing)
        {
            ddl.Items.Add(new ListItem(safeValue, safeValue));
            ddl.ClearSelection();
            ddl.Items.FindByValue(safeValue).Selected = true;
            return true;
        }

        // Not found and not added
        return false;
    }



    protected void grdOwnerOtherInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRow drv = ((DataRowView)e.Row.DataItem).Row;
            string owner1 = Helper.GetString("REG_OWNER_ID", drv);
            string owner2 = Helper.GetString("OWNER_OTHER_ID", drv);
            string relationship = Helper.GetString("RELATIONSHIP_TYPE_ID", drv);
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds1 = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OWNER");
            if (Helper.HasRows(ds1)) this.WorkflowPage.RegistrationOwnersList = ds1.Tables[0];

            if (owner1 != null && !string.IsNullOrEmpty(owner1))
            {
                var owner1Rows = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_ID = " + owner1);
                //e.Row.Cells[0].Text = (string)owner1Rows[0]["NAME"];
                e.Row.Cells[0].Text = owner1Rows.Length > 0 ? (string)owner1Rows[0]["NAME"] : "";
            }
            else
            {
                e.Row.Cells[0].Text = "";
            }
            if (owner2 != null && !string.IsNullOrEmpty(owner2))
            {
                var owner2Rows = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_ID = " + owner2);
                //e.Row.Cells[2].Text = (string)owner2Rows[0]["NAME"];
                e.Row.Cells[2].Text = owner2Rows.Length > 0 ? (string)owner2Rows[0]["NAME"] : "";
            }
            else
            {
                e.Row.Cells[2].Text = "";
            }
            if (relationship != null && !string.IsNullOrEmpty(relationship))
            {
                DataSet ds;
                ds = psc.SelectRelationshipTypes();
                var relationshipRow = ds.Tables[0].Select("RELATIONSHIP_TYPE_ID = " + relationship);
                //e.Row.Cells[1].Text = (string)relationshipRow[0]["RELATIONSHIP_NAME"];
                e.Row.Cells[1].Text = relationshipRow.Length > 0 ? (string)relationshipRow[0]["RELATIONSHIP_NAME"] : "";
            }
        }
        //GridViewRow row = e.Row;
        //row.data
        //ddlRelationship.SelectedValue = Helper.GetString("RELATIONSHIP_TYPE_ID", dr);
        //ddlOwner1.SelectedValue = Helper.GetString("REG_OWNER1_ID", dr);
        //ddlOwner2.SelectedValue = Helper.GetString("REG_OWNER2_ID", dr);

    }

    protected void grdOwnerConvictionBehalf_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRow drv = ((DataRowView)e.Row.DataItem).Row;
            string owner1 = Helper.GetString("REG_OWNER_ID", drv);
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds1 = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OWNER");
            if (Helper.HasRows(ds1))
                this.WorkflowPage.RegistrationOwnersList = ds1.Tables[0];
            var owner1Rows = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_ID = " + owner1);

            if (owner1Rows.Length > 0)
            {
                e.Row.Cells[0].Text = (string)owner1Rows[0]["NAME"];
                e.Row.Cells[1].Text = Helper.GetString("Explanation", drv);
            }

        }

    }
    protected void grdOwnerConviction_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRow drv = ((DataRowView)e.Row.DataItem).Row;
            string owner1 = Helper.GetString("REG_OWNER_ID", drv);
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds1 = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OWNER");
            if (Helper.HasRows(ds1)) this.WorkflowPage.RegistrationOwnersList = ds1.Tables[0];

            var owner1Rows = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_ID = " + owner1);
            e.Row.Cells[0].Text = (string)owner1Rows[0]["NAME"];

            e.Row.Cells[1].Text = Helper.GetString("Explanation", drv);

        }

    }
    protected void grdOwnerPenalty_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRow drv = ((DataRowView)e.Row.DataItem).Row;
            string owner1 = Helper.GetString("REG_OWNER_ID", drv);
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds1 = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OWNER");
            if (Helper.HasRows(ds1)) this.WorkflowPage.RegistrationOwnersList = ds1.Tables[0];

            var owner1Rows = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_ID = " + owner1);
            e.Row.Cells[0].Text = (string)owner1Rows[0]["NAME"];

            e.Row.Cells[1].Text = Helper.GetString("Explanation", drv);

        }

    }
    protected void grdOwnerRelationShips_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //DataRow drv = ((DataRowView)e.Row.DataItem).Row;
            //string owner1 = Helper.GetString("REG_OWNER1_ID", drv);
            //string owner2 = Helper.GetString("REG_OWNER2_ID", drv);
            //string relationship = Helper.GetString("RELATIONSHIP_TYPE_ID", drv);
            //PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            //DataSet ds1 = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OWNER");
            //if (Helper.HasRows(ds1)) this.WorkflowPage.RegistrationOwnersList = ds1.Tables[0];

            //var owner1Rows = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_ID = " + owner1);
            //e.Row.Cells[0].Text = (string)owner1Rows[0]["NAME"];

            //var owner2Rows = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_ID = " + owner2);
            //e.Row.Cells[2].Text = (string)owner2Rows[0]["NAME"];


            //DataSet ds;
            //ds = psc.SelectRelationshipTypes();
            //var relationshipRow = ds.Tables[0].Select("RELATIONSHIP_TYPE_ID = " + relationship);
            //e.Row.Cells[1].Text = (string)relationshipRow[0]["RELATIONSHIP_NAME"];

        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        var grid = (GridView)sender;

        // The unique key passed via CommandArgument (QUESTION_TYPE_ID)
        var keyArg = Convert.ToInt32(e.CommandArgument);

        switch (e.CommandName)
        {
            case "EditDisclosure_vwOpt1":
                HandleEdit(grid, keyArg, e.CommandSource, e.CommandName.Split('_')[1]);
                break;

            case "DeleteDisclosure_vwOpt1":
                HandleDelete(grid, keyArg, e.CommandSource);
                break;
        }

    }


    private void HandleEdit(GridView grid, int questionTypeId, object commandSource, string panel)
    {
        // Locate the row that raised the event
        var button = commandSource as Control;
        var row = button?.NamingContainer as GridViewRow;

        // Read values from bound cells (index by column order)
        // NOTE: adjust indices to match your columns or use DataKeys pattern below
       SetText(panel, "txtDate", SafeCellText(row, 0));
        SetSelectedValue(panel, "ddlState", SafeCellText(row, 1));
        SetText(panel, "txtProgram", SafeCellText(row, 2));
        SetText(panel, "txtAgency", SafeCellText(row, 3));
        SetText(panel, "txtAction", SafeCellText(row, 4));
        SetText(panel, "txtExplanation", SafeCellText(row, 5));

        var incidentDate = SafeCellText(row, 0);
        var stateCode = SafeCellText(row, 1);
        var programAffected = SafeCellText(row, 2);
        var agencyTakingAction = SafeCellText(row, 3);
        var actionTaken = SafeCellText(row, 4);
        var explanationDetails = SafeCellText(row, 5);

        // Example: pass to your update routine
        UpdateDisclosures("EditDisclosure_vwOpt1", questionTypeId, new REG_DISCLOSURES
        {
            INCIDENT_DATE = incidentDate,
            STATE_CODE = stateCode,
            PROGRAM_AFFECTED = programAffected,
            AGENCY_TAKING_ACTION = agencyTakingAction,
            ACTION_TAKEN = actionTaken,
            EXPLANATION_DETAILS = explanationDetails
        });
    }

    private void HandleDelete(GridView grid, int questionTypeId, object commandSource)
    {
        // If you only need the key for delete:
        UpdateDisclosures("DeleteDisclosure_vwOpt1", questionTypeId, null);
    }

    private void UpdateDisclosures(string commandName, int questionTypeId, REG_DISCLOSURES disclosures = null)
    {
        // Delete or edit by id only
    }

    private static string SafeCellText(GridViewRow row, int cellIndex)
    {
        if (row == null || cellIndex < 0 || cellIndex >= row.Cells.Count) return string.Empty;
        return (row.Cells[cellIndex].Text ?? string.Empty).Trim();
    }


    protected void btnAdd_Click(object sender, CommandEventArgs e)
    {
        AddDisclosures(e.CommandName);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        
    }
}

public class REG_DISCLOSURES
{
    public int REG_ID { get; set; }
    public int QUESTION_TYPE_ID { get; set; }
    public bool IS_SANCTIONED { get; set; } = true;

    public string INCIDENT_DATE { get; set; }
    public string STATE_CODE { get; set; }
    public string PROGRAM_AFFECTED { get; set; }
    public string AGENCY_TAKING_ACTION { get; set; }
    public string ACTION_TAKEN { get; set; }
    public string EXPLANATION_DETAILS { get; set; }
    public Guid Created_By_User { get; set; }
}