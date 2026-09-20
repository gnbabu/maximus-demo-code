using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_RegistrationQuestion : System.Web.UI.UserControl
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

        this.ucOwnerOtherInfo.KeepPopupOpenEvent += new PopupControls_OwnerOtherInfo.KeepPopupOpenEventHandler(MpeShow);
        this.ucOwnerConviction.KeepPopupOpenEvent += new PopupControls_OwnerConviction.KeepPopupOpenEventHandler(MpeShow);
        this.ucOwnerSubcontractor.KeepPopupOpenEvent += new PopupControls_OwnerSubcontractor.KeepPopupOpenEventHandler(MpeShow);
        if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.OwnerInformation)
        {
            btnSave.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSave.ValidationGroup +
                "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
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

    private void MpeShow()
    {
        //btnSave.Enabled = true;
        mpe.Show();
        hdnChanged.Value = "CHANGED";
    }

    public void LoadOwnerInfo()
    {

    }

    private void LoadOwnerXref()
    {
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
            lblQuestion.Text = Helper.GetString("QUESTION_TEXT", qstRow);
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
        if ((this.WorkflowPage.HasActiveDODDSpecialty && this.WorkflowPage.WaiverServiceUpdateTypeID != 1)
             || (this.WorkflowPage.WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD)) // OHPNM-15326 - DD Non medicaid provider shoud not update Billing payment address in PNM
        {
            this.pageIsReadOnly = true;
        }
        LoadQuestion();
        switch (mltQuestion.ActiveViewIndex)
        {
            case 1:
                LoadGrid(ucOwnerRelationships, "OWNER_XREF", grdOwnerRelationships, btnHistoryRelationships);
                //LoadOwnerXref();
                break;
            case 2:
                LoadGrid(ucOwnerOtherInfo, "OWNER_OTHER", grdOwnerOtherInfo, btnHistoryOtherInfo);
                break;
            case 4:
                LoadGrid(ucOwnerConviction, "OWNER_CONVICTION", grdConviction, btnHistoryConviction);
                break;
            case 3:
                LoadGrid(ucOwnerConviction, "OWNER_CONVICTION", grdConviction, btnHistoryConviction);
                break;
            case 12:
                
                LoadGrid(ucOwnerConvictionOnBehalf, "OWNER_CONVICTION_ON_BEHALF", grdConvictionOnBehalf, btnHistoryConvictionOnBehalf);
                break;
            case 6:
                LoadGrid(ucOwnerResidency, "OWNER_RESIDENCY", grdResidency, btnHistoryResidency);
                break;
           
            case 7:
                LoadGrid(ucOwnerPenalty, "OWNER_SANCTION", grdPenalty, btnHistoryPenalty);
                
                break;
            case 8:
                LoadGrid(ucOwnerOriginal, "ORIGINAL_OWNER", grdOriginalOwner, btnHistoryOriginalOwner);
                break;
            case 9:
                LoadGrid(ucOwnerSubcontractor, "SUBCONTRACTOR", grdSubcontractor, btnHistorySubcontractor);
                
                break;
            case 10:
                LoadGrid(ucOwnerSubcontractor5Years, "SUBCONTRACTOR5YRS", grdSubcontractor5Years, btnHistorySubcontractor5Years);
                break;
            case 11:
                LoadGrid(ucOwnerSupplier, "SUPPLIER", grdSupplier, btnHistorySupplier);
                break;
            case 5:
                LoadGrid(ucOwnerConvictionOnBehalf, "OWNER_CONVICTION_ON_BEHALF", grdConvictionOnBehalf, btnHistoryConvictionOnBehalf);
                break;
            case 13:
                LoadGrid(ucOwnerTransaction, "OWNER_TRANSACTION", grdTransaction, btnHistoryTransaction);
                break;
        }

        // turn off buttons and edit symbols if read-only
        if (this.pageIsReadOnly || !Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            btnAddConviction.Visible =
            btnAddConvictionOnBehalf.Visible =
            btnAddOriginalOwner.Visible =
            btnAddOtherInfo.Visible =
            btnAddPenalty.Visible =
            btnAddSubcontractor.Visible =
            btnAddSubcontractor5Years.Visible =
            btnAddSupplier.Visible =
            btnAddResidency.Visible =
            btnAddRelationships.Visible =
            btnAddTransaction.Visible = false;

            grdOwnerRelationships.Columns[3].Visible = false;
            grdOwnerOtherInfo.Columns[3].Visible = false;
            grdConviction.Columns[2].Visible = false;
            grdConvictionOnBehalf.Columns[2].Visible = false;
            grdResidency.Columns[1].Visible = false;
            grdPenalty.Columns[2].Visible = false;
            grdSubcontractor.Columns[1].Visible = false;
            grdSubcontractor5Years.Columns[3].Visible = false;
            grdSupplier.Columns[3].Visible = false;
            grdOriginalOwner.Columns[4].Visible = false;
            grdTransaction.Columns[3].Visible = false;
        }

    }

    public bool HasChanged()
    {
        return (hdnChanged.Value == "CHANGED");
    }

    private void SaveQuestionAnswer()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.SaveRegistrationQuestion(this.WorkflowPage.RegistrationId, QuestionTypeId, (rblYesNo.SelectedIndex == 0 ? 1 : 0),
            MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed,
            Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
    }

    public void SaveData()
    {
		// OHPNM-14088
		if (this.WorkflowPage.OwnerInfoPageIsReadOnly)
        {
            // this is a read only page; no need to validate or save
            return;
        }

        if (hdnChanged.Value != "CHANGED") return;

        hdnChanged.Value = string.Empty;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        switch (mltQuestion.ActiveViewIndex)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
                if (rblYesNo.SelectedIndex >= 0) SaveQuestionAnswer();
                break;
        }
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
		if (this.WorkflowPage.OwnerInfoPageIsReadOnly)
        {
            // this is a read only page; no need to validate or save
            return;
        }

        if (rblYesNo.SelectedIndex < 0) AddError("*Select Yes or No", ref isGood);
        else
            switch (mltQuestion.ActiveViewIndex)
            {
                case 1:
                    if (rblYesNo.SelectedIndex == 0 && grdOwnerRelationships.Rows.Count == 0) AddError("*Enter Owner Relationship Information", ref isGood);
                    break;
                case 2:
                    if (rblYesNo.SelectedIndex == 0 && grdOwnerOtherInfo.Rows.Count == 0) AddError("*Enter Owner Other Information", ref isGood);
                    break;
                case 3:
                    if (rblYesNo.SelectedIndex == 0 && grdConviction.Rows.Count == 0) AddError("*Enter Owner Conviction Information", ref isGood);
                    break;
                case 4:
                    if (rblYesNo.SelectedIndex == 0 && grdConviction.Rows.Count == 0) AddError("*Enter Owner Conviction Information", ref isGood);
                   
                    break;
                case 5:
                    if (rblYesNo.SelectedIndex == 0 && grdConvictionOnBehalf.Rows.Count == 0) AddError("*Enter Owner Conviction On Behalf of Information", ref isGood);
                
                    break;
                case 6:
                    if (rblYesNo.SelectedIndex == 0 && grdResidency.Rows.Count == 0) AddError("*Enter Owner Residency Information", ref isGood);
                    break;
                
                case 7:
                    if (rblYesNo.SelectedIndex == 0 && grdPenalty.Rows.Count == 0) AddError("*Enter Owner Medicare Sanctions", ref isGood);
                 
                    break;
                case 8:
                    if (rblYesNo.SelectedIndex == 0 && grdOriginalOwner.Rows.Count == 0) AddError("*Enter Original Owner Information", ref isGood);
                    break;
                case 9:
                    if (rblYesNo.SelectedIndex == 0 && grdSubcontractor.Rows.Count == 0) AddError("*Enter Owner Subcontractor Information", ref isGood);
                    /*if (rblYesNo.SelectedIndex == 0)
                    {
                        if (grdSubcontractoOwner.Rows.Count == 0) AddError("*Enter Subcontractor Owner Information", ref isGood);
                        if (TotalGrid() > 100) AddError("*Subcontractor Owner percent total cannot exceed 100", ref isGood);
                    }*/
                    break;
                case 10:
                    if (rblYesNo.SelectedIndex == 0 && grdSubcontractor5Years.Rows.Count == 0) AddError("*Enter Owner Subcontractor 5 Years Information", ref isGood);
                    break;
                case 11:
                    if (rblYesNo.SelectedIndex == 0 && grdSupplier.Rows.Count == 0) AddError("*Enter Owner Supplier Information", ref isGood);
                    break;
                case 12:
                    if (rblYesNo.SelectedIndex == 0 && grdConvictionOnBehalf.Rows.Count == 0) AddError("*Enter Owner Conviction On Behalf of Information", ref isGood);
                    break;
                case 13:
                    if (rblYesNo.SelectedIndex == 0 && grdTransaction.Rows.Count == 0) AddError("*Enter Owner Transaction", ref isGood);
                    break;

            }
    }

    private DataRow GetRow(int index, string tableName)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, tableName);
        return ds.Tables[0].Rows[index];
    }

    private void ShowPopup(BasePopupControl ctl, string title, string valGroup, int index, string tableName, int viewIndex)
    {
        lblTitle.Text = title;
        btnSave.ValidationGroup = valGroup;
        DataRow dr = null;
        if (index >= 0 && ctl.DataList.Rows.Count > 0) dr = ctl.DataList.Rows[index];
        ctl.LoadData(dr);
        hdnChanged.Value = "CHANGED";
        mltPopup.ActiveViewIndex = viewIndex;
        mpe.Show();
    }

    private void SetButtons(string commandName)
    {
        if (commandName == "History")
        {
            btnSave.Visible = false;
            btnCancel.Text = "Close";
            return;
        }

        btnSave.Visible = true;
        btnCancel.Text = "Cancel";

        if (!Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            btnSave.Visible = false;
            btnCancel.Text = "Close";
        }
    }

    private void WorkRow(string commandName, int index)
    {
        SetButtons(commandName);

        switch (commandName)
        {
            case "OwnerRelationships":
                ShowPopup(ucOwnerRelationships, "Owner Relationships", "valOwnerRelationships", index, "OWNER_XREF", 1);
                break;
            case "OwnerOtherInfo":
                ShowPopup(ucOwnerOtherInfo, "Other Ownership", "valOwnerOtherInfo", index, "OWNER_OTHER", 2);
                break;
            case "OwnerConviction":
                ShowPopup(ucOwnerConviction, "Owner Conviction", "valOwnerConviction", index, "OWNER_CONVICTION", 3);
                break;
            case "OwnerConvictions":
                ShowPopup(ucOwnerConviction, "Owner Conviction", "valOwnerConviction", index, "OWNER_CONVICTION", 4);
                break;
            
            case "OwnerConvictionOnBehalf":
                ShowPopup(ucOwnerConvictionOnBehalf, "Owner Conviction On Behalf", "valOwnerConvictionOnBehalf", index, "OWNER_CONVICTION_ON_BEHALF", 14);
                break;
            case "OwnerResidency":
                ShowPopup(ucOwnerResidency, "Owner Residency", "valOwnerResidency", index, "OWNER_RESIDENCT", 6);
                break;
          
            case "OwnerPenalty":
                ShowPopup(ucOwnerPenalty, "Medicare Sanctions", "valOwnerConviction", index, "OWNER_SANCTION", 7);
              
                break;
            case "OriginalOwner":
                ShowPopup(ucOwnerOriginal, "Original Owner", "valOwnerOriginal", index, "ORIGINAL_OWNER", 8);
                break;
            case "OwnerSubcontractor":
                ucOwnerSubcontractor.RegSubContractorTypeId = MAXIMUS.Core.Libraries.Constants.RegistrationSubcontractorType.OwnershipAtLeast5Percent;
                ShowPopup(ucOwnerSubcontractor, "Owner Subcontractor", "valOwnerSubcontractor", index, "SUBCONTRACTOR", 9);
                break;
            case "OwnerSubcontractorOwner":
                ShowPopup(ucOwnerSubcontractorOwner, "Subcontractor Owner", "valSubcontractorOwner", index, "SUBCONTRACTOR_OWNER", 10);
                break;
            case "OwnerSubcontractor5Years":
                ucOwnerSubcontractor5Years.RegSubContractorTypeId = MAXIMUS.Core.Libraries.Constants.RegistrationSubcontractorType.BusinessDoneLast5Years;
                ShowPopup(ucOwnerSubcontractor5Years, "Owner Subcontractor", "valOwnerSubcontractor", index, "SUBCONTRACTOR", 11);
                break;
            case "OwnerSupplier":
                ShowPopup(ucOwnerSupplier, "Owner Supplier", "valOwnerSupplier", index, "SUPPLIER", 12);
                break;
            case "OwnerConvictionOnBehalfs":
                ShowPopup(ucOwnerConvictionOnBehalf, "Owner Conviction On Behalf", "valOwnerConvictionOnBehalf", index, "OWNER_CONVICTION_ON_BEHALF", 14);
                break;
            case "OwnerTransaction":
                ShowPopup(ucOwnerTransaction, "Owner Transaction", "valOwnerTransaction", index, "OWNER_Transaction", 15);
                break;
            default:
                break;
        }
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
            DataRow drv = ((DataRowView)e.Row.DataItem).Row;
            string owner1 = Helper.GetString("REG_OWNER1_ID", drv);
            string owner2 = Helper.GetString("REG_OWNER2_ID", drv);
            string relationship = Helper.GetString("RELATIONSHIP_TYPE_ID", drv);
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds1 = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OWNER");
            if (Helper.HasRows(ds1)) this.WorkflowPage.RegistrationOwnersList = ds1.Tables[0];

            var owner1Rows = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_ID = " + owner1);
            e.Row.Cells[0].Text = (string)owner1Rows[0]["NAME"];

            var owner2Rows = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_ID = " + owner2);
            e.Row.Cells[2].Text = (string)owner2Rows[0]["NAME"];


            DataSet ds;
            ds = psc.SelectRelationshipTypes();
            var relationshipRow = ds.Tables[0].Select("RELATIONSHIP_TYPE_ID = " + relationship);
            e.Row.Cells[1].Text = (string)relationshipRow[0]["RELATIONSHIP_NAME"];
           
        }
        //GridViewRow row = e.Row;
        //row.data
        //ddlRelationship.SelectedValue = Helper.GetString("RELATIONSHIP_TYPE_ID", dr);
        //ddlOwner1.SelectedValue = Helper.GetString("REG_OWNER1_ID", dr);
        //ddlOwner2.SelectedValue = Helper.GetString("REG_OWNER2_ID", dr);

    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        WorkRow(e.CommandName, Convert.ToInt32(e.CommandArgument));
    }

    protected void btnAdd_Click(object sender, CommandEventArgs e)
    {
        WorkRow(e.CommandName, -1);
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        int opt = 0;
        switch (e.CommandName)
        {
            case "OwnerRelationships":
                opt = 1;
                break;
            case "OwnerOtherInfo":
                opt = 2;
                break;
            case "OwnerConviction":
                opt = 4;
                break;
            case "OwnerConvictionOnBehalf":
                opt = 5;
                break;
            case "OwnerResidency":
                opt = 6;
                break;
           
            case "OwnerPenalty":
                opt = 7;
                break;
           
            case "OriginalOwner":
                opt = 8;
                break;
            case "OwnerSubcontractor":
                opt = 9;
                break;
            case "OwnerSubcontractorOwner":
                opt = 10;
                break;
            case "OwnerSubcontractor5Years":
                opt = 11;
                break;
            case "OwnerSupplier":
                opt = 12;
                break;
       
            case "OwnerTransaction":
                opt = 14;
                break;
        }
        if (opt > 0)
        {
            SetButtons("History");
            lblTitle.Text = Helper.PascalCaseParse(e.CommandName) + " History";
            mltPopup.ActiveViewIndex = 13;
            ucOwnerHistory.LoadData(opt);
            mpe.Show();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            mpe.Show();
            return;
        }

        if ((mltPopup.ActiveViewIndex >= 1 && mltPopup.ActiveViewIndex <= 11) || (mltPopup.ActiveViewIndex == 13 || mltPopup.ActiveViewIndex == 14)) SaveQuestionAnswer();
        if (mltPopup.ActiveViewIndex >= 2 && mltPopup.ActiveViewIndex <= 14) this.WorkflowPage.InvalidateAgreements = true;
        //hdnChanged.Value = string.Empty;
        switch (mltPopup.ActiveViewIndex)
        {
            case 1:
                ucOwnerRelationships.SaveData();
                LoadGrid(ucOwnerRelationships, "OWNER_XREF", grdOwnerRelationships, btnHistoryRelationships);
                break;
            case 2:
                ucOwnerOtherInfo.SaveData();
                LoadGrid(ucOwnerOtherInfo, "OWNER_OTHER", grdOwnerOtherInfo, btnHistoryOtherInfo);
                break;
            case 3:
                ucOwnerConviction.SaveData();
                LoadGrid(ucOwnerConviction, "OWNER_CONVICTION", grdConviction, btnHistoryConviction);
                break;
            case 4:
                ucOwnerConviction.SaveData();
               
                LoadGrid(ucOwnerConviction, "OWNER_CONVICTION", grdConviction, btnHistoryConviction);
               
                break;
            case 5:
                ucOwnerConvictionOnBehalf.SaveData();
                LoadGrid(ucOwnerConvictionOnBehalf, "OWNER_CONVICTION_ON_BEHALF", grdConvictionOnBehalf, btnHistoryConvictionOnBehalf);
              
                break;
            case 6:
                ucOwnerResidency.SaveData();
                LoadGrid(ucOwnerResidency, "OWNER_RESIDENCY", grdResidency, btnHistoryResidency);
              
                break;
            case 7:
                ucOwnerPenalty.SaveData();
                LoadGrid(ucOwnerPenalty, "OWNER_SANCTION", grdPenalty, btnHistoryPenalty);
                break;
            case 8:
                ucOwnerOriginal.SaveData();
                LoadGrid(ucOwnerOriginal, "ORIGINAL_OWNER", grdOriginalOwner, btnHistoryOriginalOwner);
                break;
            case 9:
                ucOwnerSubcontractor.SaveData();
                LoadGrid(ucOwnerSubcontractor, "SUBCONTRACTOR", grdSubcontractor, btnHistorySubcontractor);
                break;
            case 10:
                ucOwnerSubcontractorOwner.SaveData();
              
                break;
            case 11:
                ucOwnerSubcontractor5Years.SaveData();
                LoadGrid(ucOwnerSubcontractor5Years, "SUBCONTRACTOR5YRS", grdSubcontractor5Years, btnHistorySubcontractor5Years);
                break;
            case 12:
                ucOwnerSupplier.SaveData();
                LoadGrid(ucOwnerSupplier, "SUPPLIER", grdSupplier, btnHistorySupplier);
                break;
            case 14:
                ucOwnerConvictionOnBehalf.SaveData();
                LoadGrid(ucOwnerConvictionOnBehalf, "OWNER_CONVICTION_ON_BEHALF", grdConvictionOnBehalf, btnHistoryConvictionOnBehalf);
                break;
            case 15:
                ucOwnerTransaction.SaveData();
                LoadGrid(ucOwnerTransaction, "OWNER_TRANSACTION", grdTransaction, btnHistoryTransaction);
                break;
        }
    }
}