using Corp.Core.Libraries.Helper;
using DocumentFormat.OpenXml.ExtendedProperties;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Pages_Background : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    #region svc
    private const string sectionName = "BackgroundCheck";
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
    private static int Complete_cnt = 0, row_cnt = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadBackgroundCheck();
    }

    public override void LoadData(DataRow dr = null)
    {
    }

    public override bool SaveData()
    {
        return true;
    }

    public override bool ValidateData()
    {
        return true;
    }

    protected void rgFCBC_ItemUpdated(object sender, Telerik.Web.UI.GridUpdatedEventArgs e)
    {

    }
    protected void rgFCBC_EditCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
        }
    }

    private string GetPerformedByType(string value)
    {
        string ret = "";
        switch (value)
        {
            case "1":
                ret = CON.BackgroundPerformedByType.ODM.ToString();
                break;
            case "2":
                ret = CON.BackgroundPerformedByType.ODA.ToString();
                break;
            case "3":
                ret = CON.BackgroundPerformedByType.DODD.ToString();
                break;
            case "4":
                ret = null;
                break;
            default:
                break;

        }
        return ret;
    }

    protected void rgFCBC_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        GridEditableItem item = e.Item as GridEditableItem;

        ValidateData(item);
        if (Page.IsValid)
        {
            int id = Convert.ToInt32(item.GetDataKeyValue("REG_BACKGROUND_CHECK_ID"));
            int ownerID = Helper.ConvertStrNullToInt32(item.GetDataKeyValue("REG_OWNER_ID"));
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            if (ownerID == -1)
                parms.Add("REG_OWNER_ID", null);
            else
                parms.Add("REG_OWNER_ID", ownerID.ToString());
            //rblBackgroundCheckReq
            RadioButtonList rblBackgroundCheckReq = item.FindControl("rblBackgroundCheckReq") as RadioButtonList;
            parms.Add("IsBackgroundCheckRequired", (rblBackgroundCheckReq.SelectedIndex == 0 ? 1 : 0).ToString());
            Panel pnlBKCNotRequiredVal = item.FindControl("pnlBKCNotRequired") as Panel;
            Panel pnlBKCRequiredVal = item.FindControl("pnlBKCRequired") as Panel;
            if (pnlBKCNotRequiredVal.Enabled)
            {
                DropDownList ddlReasonBackgroundCheckNotReq = item.FindControl("ddlReasonBackgroundCheckNotReq") as DropDownList;
                parms.Add("ReasonBackgroundCheckNotReq", ddlReasonBackgroundCheckNotReq.SelectedValue.ToString());
                parms.Add("BACKGROUND_DATE", null);
                parms.Add("BACKGROUND_STATUS_TYPE_ID", CON.BackgroundStatusType.Yes.ToString());
                parms.Add("BACKGROUND_RESULT_TYPE_ID", CON.BackgroundResultType.Passed.ToString());
                parms.Add("BACKGROUND_PERFORMED_BY_TYPE_ID", GetPerformedByType(ddlReasonBackgroundCheckNotReq.SelectedValue));
                parms.Add("IssueFBILetter", "0");
                parms.Add("IsEnrolledRapBack", "0");
            }
            else if (pnlBKCRequiredVal.Enabled)
            {
                Dictionary<string, string> newValues = new Dictionary<string, string>();
                item.ExtractValues(newValues);
                TextBox txtBDate = item.FindControl("txtBackgroundDate") as TextBox;
                if (!string.IsNullOrWhiteSpace(txtBDate.Text.ToString()) && Convert.ToDateTime(txtBDate.Text) != DateTime.MinValue)
                {
                    parms.Add("BACKGROUND_DATE", Convert.ToDateTime(txtBDate.Text).ToShortDateString());
                }
                else
                {
                    parms.Add("BACKGROUND_DATE", null);
                }
                parms.Add("ReasonBackgroundCheckNotReq", String.Empty);
                string statusID = newValues.FirstOrDefault(v => v.Key == "BACKGROUND_STATUS_TYPE_ID").Value;
                parms.Add("BACKGROUND_STATUS_TYPE_ID", statusID);

                if (statusID == "1" || statusID == "2") Complete_cnt += 1;

                string resultID = newValues.FirstOrDefault(v => v.Key == "BACKGROUND_RESULT_TYPE_ID").Value;
                parms.Add("BACKGROUND_RESULT_TYPE_ID", resultID);

                string performedByID = newValues.FirstOrDefault(v => v.Key == "BACKGROUND_PERFORMED_BY_TYPE_ID").Value;
                performedByID = (performedByID == "") ? null : performedByID;
                parms.Add("BACKGROUND_PERFORMED_BY_TYPE_ID", performedByID);

                RadioButtonList rblIssueFBILetter = item.FindControl("rblIssueFBILetter") as RadioButtonList;
                RadioButtonList rblEnrolledInRapBack = item.FindControl("rblEnrolledInRapBack") as RadioButtonList;
                parms.Add("IssueFBILetter", rblIssueFBILetter.SelectedValue.ToString());
                parms.Add("IssueFBILetter_OnDate", rblIssueFBILetter.SelectedValue.Equals("True") ? DateTime.Now.ToString() : null);
                parms.Add("IsEnrolledRapBack", rblEnrolledInRapBack.SelectedValue.ToString());
                parms.Add("IsEnrolledRapBack_OnDate", rblEnrolledInRapBack.SelectedValue.Equals("True") ? DateTime.Now.ToString() : null);
            }
            //


            //
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            TextBox txtComments = item.FindControl("txtComments") as TextBox;
            parms.Add("Comments", txtComments.Text.ToString());
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            if (id > 0)
            {
                parms.Add("REG_BACKGROUND_CHECK_ID", id.ToString());
                svc.UpdateRegistrationDataTable("BACKGROUND_CHECKcustom", parms);
            }
            else
            {
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                svc.InsertRegistrationDataTable("BACKGROUND_CHECKcustom", parms);
            }

            try
            {
                if (!string.IsNullOrEmpty(txtComments.Text.ToString()))
                {
                    ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, "Fingerprint and Background Check Information - " + txtComments.Text.ToString(), null, null, null, this.WorkflowPage.WF_ProcessID);
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            LoadBackgroundCheck();
        }
        else
            e.Canceled = true;
    }

    private void ValidateData(GridEditableItem item)
    {
        //GridEditableItem item = e.Item as GridEditableItem;

        bool isValid = true;
        Panel pnlBKCNotRequiredVal = item.FindControl("pnlBKCNotRequired") as Panel;
        Panel pnlBKCRequiredVal = item.FindControl("pnlBKCRequired") as Panel;
        if (pnlBKCNotRequiredVal.Enabled)
        {
            DropDownList ddlReasonBackgroundCheckNotReq = item.FindControl("ddlReasonBackgroundCheckNotReq") as DropDownList;
            if (String.IsNullOrEmpty(ddlReasonBackgroundCheckNotReq.SelectedItem.Value))
            {
                AddError("Choose the reason for Background Check not Required Reason.", ref isValid);
            }
        }
        else if (pnlBKCRequiredVal.Enabled)
        {
            DropDownList ddlBackgroundComplete = item.FindControl("ddlBackgroundComplete") as DropDownList;
            DropDownList ddlBackgroundResult = item.FindControl("ddlBackgroundResult") as DropDownList;
            DropDownList ddlBackgroundPerformedBy = item.FindControl("ddlBackgroundPerformedBy") as DropDownList;
            TextBox txtBDate = item.FindControl("txtBackgroundDate") as TextBox;
            Dictionary<string, string> newValues = new Dictionary<string, string>();
            item.ExtractValues(newValues);
            string statusID = newValues.FirstOrDefault(v => v.Key == "BACKGROUND_STATUS_TYPE_ID").Value;
            string resultID = newValues.FirstOrDefault(v => v.Key == "BACKGROUND_RESULT_TYPE_ID").Value;
            string performedByID = newValues.FirstOrDefault(v => v.Key == "BACKGROUND_PERFORMED_BY_TYPE_ID").Value;
            if (resultID == "" && statusID != CON.BackgroundStatusType.No.ToString())
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "Result is required.";
                val.ValidationGroup = "BackgroundValidation";
                this.Page.Validators.Add(val);
            }
            if (statusID != CON.BackgroundStatusType.No.ToString() && resultID == CON.BackgroundResultType.Pending.ToString())
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "Choose No as Background complete Status.";
                val.ValidationGroup = "BackgroundValidation";
                this.Page.Validators.Add(val);
            }
            if (statusID != CON.BackgroundStatusType.Yes.ToString() && (resultID == CON.BackgroundResultType.Passed.ToString() || resultID == CON.BackgroundResultType.Failed.ToString()))
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "Choose Yes as Background complete Status.";
                val.ValidationGroup = "BackgroundValidation";
                this.Page.Validators.Add(val);
            }
            if (statusID != CON.BackgroundStatusType.No.ToString() && resultID == CON.BackgroundResultType.FailedToAppear.ToString())
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "Choose No as Background complete Status.";
                val.ValidationGroup = "BackgroundValidation";
                this.Page.Validators.Add(val);
            }
            if (statusID == CON.BackgroundStatusType.Yes.ToString() && (resultID == CON.BackgroundResultType.Passed.ToString() || resultID == CON.BackgroundResultType.Failed.ToString() || resultID == "0"))
            {
                if (string.IsNullOrEmpty(txtBDate.Text))
                {
                    CustomValidator val = new CustomValidator();
                    val.IsValid = false;
                    val.ErrorMessage = "Date Background Check Completed is required.";
                    val.ValidationGroup = "BackgroundValidation";
                    this.Page.Validators.Add(val);
                }
            }
            if (performedByID == "" && resultID != CON.BackgroundResultType.Pending.ToString() && resultID != CON.BackgroundResultType.FailedToAppear.ToString() && resultID != "")
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "Performed By is required.";
                val.ValidationGroup = "BackgroundValidation";
                this.Page.Validators.Add(val);
            }
        }
    }

    private void AddError(string msg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "BackgroundValidation";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    protected void rgFCBC_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "BACKGROUND_CHECK");
        if (Helper.HasRows(ds))
        {
            rgFCBC.DataSource = ds;
        }
        else
            rgFCBC.DataSource = null;
    }

    protected void rgFCBC_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

        }
    }

    protected void rgFCBC_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.InitInsertCommandName) //"Add new" button clicked
        {
            GridEditCommandColumn editColumn = (GridEditCommandColumn)rgFCBC.MasterTableView.GetColumn("EditCommandColumn");
            editColumn.Visible = false;
        }
        else if (e.CommandName == RadGrid.RebindGridCommandName && e.Item.OwnerTableView.IsItemInserted)
        {
            e.Canceled = true;
        }
        else
        {
            GridEditCommandColumn editColumn = (GridEditCommandColumn)rgFCBC.MasterTableView.GetColumn("EditCommandColumn"); 

            if (!editColumn.Visible)
                editColumn.Visible = true;
        }
    }

    private void LoadBackgroundCheck()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        //DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "BACKGROUND_CHECK");
		DataTable dataTable = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "BACKGROUND_CHECK").Tables[0].Select("VERSIONHISTORY IS NULL").CopyToDataTable();
		//rgFCBC.DataSource = ds;
		//rgFCBC.DataBind();
		Complete_cnt = 0; row_cnt = 0;
        if (Helper.HasRows(dataTable))
        {
            row_cnt = dataTable.Rows.Count;
            DataRow[] dr = dataTable.Select("BACKGROUND_RESULT_TYPE_ID IN (1)");
            Complete_cnt = dr.Length;
            if (dataTable.Rows.Count == dr.Length)
                SetTakeActionVisibility(true);
            else
                SetTakeActionVisibility(false);
        }
        else
            SetTakeActionVisibility(false);
    }

    protected void rgFCBC_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem itemBP = (GridDataItem)e.Item;
            string performedByODA = itemBP["BACKGROUND_PERFORMED_BY_TYPE"].Text;
            string performedByDODD = itemBP["BACKGROUND_PERFORMED_BY_TYPE"].Text;
            itemBP["EditCommandColumn"].Visible = (Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name) || Helper.IsUserInAdminRole(HttpContext.Current.User.Identity.Name)) ? true : false;

            if (performedByODA.Equals(CON.BackgroundPerformedByType.ODA.ToString()) || performedByDODD.Equals(CON.BackgroundPerformedByType.DODD.ToString()))
            {
                GridDataItem itemBD = e.Item as GridDataItem;
                TableCell cellDate = itemBD["BACKGROUND_DATE"];
                cellDate.Text = "";
            }
            if (itemBP["IsBackgroundCheckRequired"].Text == "True")
            {
                GridDataItem itemBD = e.Item as GridDataItem;
                TableCell cellDate = itemBD["IsBackgroundCheckRequired"];
                cellDate.Text = "Yes";
            }
            else
            {
                GridDataItem itemBD = e.Item as GridDataItem;
                TableCell cellDate = itemBD["IsBackgroundCheckRequired"];
                cellDate.Text = "No";
            }


        }
        if ((e.Item is GridEditFormItem) && e.Item.IsInEditMode)
        {

            var gridEditFormItem = (GridEditFormItem)e.Item;
            string stcellDate = gridEditFormItem["IsBackgroundCheckRequired"].Text;
            string versionHist = gridEditFormItem["VersionHistory"].Text;
            if (stcellDate.Equals("Yes"))
            {
                Panel lbl = (Panel)gridEditFormItem.FindControl("pnlBKCRequired");
                lbl.Visible = true;
                lbl.Enabled = true;
            }
            else
            {
                Panel lbl = (Panel)gridEditFormItem.FindControl("pnlBKCNotRequired");
                lbl.Visible = true;
                lbl.Enabled = true;
            }


                if (this.WorkflowPage.CurrentTaskName != CON.BackgroundCheckTaskNames.FingerprintandBackgroundPending)
                {
                    GridEditableItem item = e.Item as GridEditableItem;
                    Button btn = item.FindControl("btnPoorQualityFingerPrints") as Button;
                    btn.Visible = false;
                }

            if (versionHist == "True")
            {
                Button btnUdt = (Button)gridEditFormItem.FindControl("btnUpdate");
                Button btnIss = (Button)gridEditFormItem.FindControl("btnIssueSecondBackgroundCheckNotice");
                Button btnord = (Button)gridEditFormItem.FindControl("btnOrderBackgroundCheckWithoutFingerprints");
                Button btnrjt = (Button)gridEditFormItem.FindControl("btnRejectBCReport");
                btnUdt.Visible = false;
                btnIss.Visible = false;
                btnord.Visible = false;
                btnrjt.Visible = false;
            }
        }

    }

    private void SetTakeActionVisibility(bool vis)
    {
        bool isTakeActionVisible = false;
        isTakeActionVisible = vis;

        if (this._toggleTakeActionVisibility != null)
        {
            this._toggleTakeActionVisibility(new ToggleTakeActionVisibilityEventArgs(isTakeActionVisible));
        }
    }

    protected void ddlReasonBackgroundNotReq_Init(object sender, EventArgs e)
    {
        DropDownList ddList = (DropDownList)sender;

        // Load drop down list of background statuses
        DataSet dsBackgroundStatus = svc.SelectReasonBackgroundCheckNotRequired();
        if (Helper.HasRows(dsBackgroundStatus))
        {
            Helper.LoadDropDown(ddList, dsBackgroundStatus.Tables[0], "BACKGROUNDCHECK_REASONS_NAME", "BACKGROUNDCHECK_REASONS_ID", true);
            ddList.Items.Add(new ListItem("", "0"));
            //removeed the empty string from dropdown - OHPNM - 8947
            if (ddList.Items.Count > 0)
            {
                for (int i = 0; i < ddList.Items.Count; i++)
                {
                    if (ddList.Items[i].Text == "")
                    {
                        if (i != 0)
                            ddList.Items.RemoveAt(i);
                    }
                }
            }
        }
    }

    protected void ddlBackgroundComplete_Init(object sender, EventArgs e)
    {
        DropDownList ddList = (DropDownList)sender;

        // Load drop down list of background statuses
        DataSet dsBackgroundStatus = svc.SelectBackgroundStatusTypes();
        if (Helper.HasRows(dsBackgroundStatus))
        {
            Helper.LoadDropDown(ddList, dsBackgroundStatus.Tables[0], "BACKGROUND_STATUS_TYPE", "BACKGROUND_STATUS_TYPE_ID", true);
            ddList.Items.Add(new ListItem("", "0"));
        }

    }
    protected void ddlBackgroundResult_Init(object sender, EventArgs e)
    {
        DropDownList ddList = (DropDownList)sender;

        // Load drop down list of background results
        DataSet dsBackgroundResult = svc.SelectBackgroundResultTypes();
        if (Helper.HasRows(dsBackgroundResult))
        {
            Helper.LoadDropDown(ddList, dsBackgroundResult.Tables[0], "BACKGROUND_RESULT_TYPE", "BACKGROUND_RESULT_TYPE_ID", true);
            ddList.Items.Add(new ListItem("", "0"));
        }
    }

    /// <summary>
    /// OHPNM-10799 -  On Background Result dropdown change show RejectBCReport button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBackgroundResult_IndexChanged(object sender, EventArgs e)
    {
        SetBtnRejectBCReportVisibility(sender);
    }

    /// <summary>
    ///  On Page load show or hide RejectBCReport button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBackgroundResult_DataBound(object sender, EventArgs e)
    {
        SetBtnRejectBCReportVisibility(sender);
    }

    /// <summary>
    /// Set visibility for RejectBCReport button based on the Background Result selection
    /// </summary>
    /// <param name="sender"></param>
    private void SetBtnRejectBCReportVisibility(object sender)
    {
        DropDownList ddList = (DropDownList)sender;
        Button btnRejectBCReport = (Button)Helper.FindTheControl(this, "btnRejectBCReport");
        if (ddList.SelectedItem.Text == "Failed to Schedule or Appear")
        {
            btnRejectBCReport.Visible = false;
        }
        else
        {
            btnRejectBCReport.Visible = true;
        }

    }


    protected void PollDistribPointsTypeOptions_DataBinding(object sender, EventArgs e)
    {
        string chek = "";
        /*
        GridEditableItem item = (GridEditableItem)(sender as RadioButtonList).NamingContainer;
        string emp = item["IsBackgroundCheckRequired"].Text.ToString();
        string check = ((RadioButtonList)sender).SelectedValue;
        if (emp.Equals("True"))
        {
            Panel lbl = (Panel)item.FindControl("pnlBKCRequired");
            lbl.Visible = true;
            lbl.Enabled = true;
        }
        else
        {
            Panel lbl = (Panel)item.FindControl("pnlBKCNotRequired");
            lbl.Visible = true;
            lbl.Enabled = true;
        }
        */
    }

    protected void PollDistribPointsTypeOptions_SelectedIndexChanged(object sender, EventArgs e)
    {

        GridEditableItem item = (GridEditableItem)(sender as RadioButtonList).NamingContainer;
        string emp = item["BACKGROUND_PERFORMED_BY_TYPE"].Text.ToString();
        RadioButtonList rblBackgroundCheckReq = (RadioButtonList)item.FindControl("rblBackgroundCheckReq");

        if (rblBackgroundCheckReq.SelectedValue.Equals("True"))
        {
            Panel pnlBKCNotRequired = (Panel)item.FindControl("pnlBKCNotRequired");
            Panel pnlBKCRequired = (Panel)item.FindControl("pnlBKCRequired");
            pnlBKCNotRequired.Visible = false;
            pnlBKCNotRequired.Enabled = false;
            pnlBKCRequired.Visible = true;
            pnlBKCRequired.Enabled = true;
        }
        else
        {
            Panel pnlBKCNotRequired = (Panel)item.FindControl("pnlBKCNotRequired");
            Panel pnlBKCRequired = (Panel)item.FindControl("pnlBKCRequired");
            pnlBKCNotRequired.Visible = true;
            pnlBKCNotRequired.Enabled = true;
            pnlBKCRequired.Visible = false;
            pnlBKCRequired.Enabled = false;
        }
    }

    protected void ddlBackgroundPerformedBy_Init(object sender, EventArgs e)
    {
        DropDownList ddList = (DropDownList)sender;

        // Load drop down list of background performed by sources
        DataSet dsBackgroundPerformedBy = svc.SelectBackgroundPerformedByTypes();
        if (Helper.HasRows(dsBackgroundPerformedBy))
        {
            Helper.LoadDropDown(ddList, dsBackgroundPerformedBy.Tables[0], "BACKGROUND_PERFORMED_BY_TYPE", "BACKGROUND_PERFORMED_BY_TYPE_ID", true);
        }
    }

    protected void btnIssueSecondBackgroundCheckNotice_Click(object sender, EventArgs e)
    {
        Button Button = (Button)sender;
        GridEditableItem item = (GridEditableItem)Button.NamingContainer;
        int regBCID = Convert.ToInt32(item.GetDataKeyValue("REG_BACKGROUND_CHECK_ID"));
        int regOwnerID = Helper.ConvertStrNullToInt32(item.GetDataKeyValue("REG_OWNER_ID"));
        if (regOwnerID > 0)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectRegOwnerByOwnerID(regOwnerID);
            if (Helper.HasRows(ds))
            {
                string ownerName = "";
                ownerName = item.GetDataKeyValue("NAME").ToString();
                DataRow dr = ds.Tables[0].Rows[0];
                if (!string.IsNullOrEmpty(Helper.GetData("NAME", dr)))
                {                    
                    string pdmsurl = AppSettings.Get("PDMS-URL");
                    Guid requestGuid = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
                    RadioButtonList rblIssueFBILetterValue = (RadioButtonList)item.FindControl("rblIssueFBILetter");
                    //RadioButtonList rblIssueFBILetterValue = rgFCBC.MasterTableView.Items[0].EditFormItem.EditFormCell.FindControl("rblIssueFBILetter") as RadioButtonList;
                    string fileName = rblIssueFBILetterValue.SelectedItem.Text == "Yes" ? "SecondBackgroundNoticeWithFBI.txt" : "SecondBackgroundNoticeWithOutFBI.txt";
                    string subject = rblIssueFBILetterValue.SelectedItem.Text == "Yes" ? "SecondBackgroundNoticeWithFBI" : "SecondBackgroundNoticeWithOutFBI";

                    // afraid the empty recipients may be causing failure
                    psc.NotifyBackgroundCheckWithoutPrints(subject, ownerName, fileName, string.Empty, string.Empty, pdmsurl, WorkflowPage.RegistrationId, requestGuid);
                }

            }
        }
    }
    protected void btnPoorQualityFingerPrints_Click(object sender, EventArgs e)
    {
        Button Button = (Button)sender;
        GridEditableItem item = (GridEditableItem)Button.NamingContainer;

        int id = Convert.ToInt32(item.GetDataKeyValue("REG_BACKGROUND_CHECK_ID"));
        int ownerID = Helper.ConvertStrNullToInt32(item.GetDataKeyValue("REG_OWNER_ID"));
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        if (ownerID == -1)
            parms.Add("REG_OWNER_ID", null);
        else
            parms.Add("REG_OWNER_ID", ownerID.ToString());
        parms.Add("IsPoorQualityPrints", "1");
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        if (id > 0)
        {
            parms.Add("REG_BACKGROUND_CHECK_ID", id.ToString());
            svc.UpdateRegistrationDataTable("BACKGROUND_CHECKcustom", parms);
        }

    }
    protected void btnOrderBackgroundCheckWithoutFingerprints_Click(object sender, EventArgs e)
    {
        Button Button = (Button)sender;
        GridEditableItem item = (GridEditableItem)Button.NamingContainer;
        int regBCID = Convert.ToInt32(item.GetDataKeyValue("REG_BACKGROUND_CHECK_ID"));
        int regOwnerID = Helper.ConvertStrNullToInt32(item.GetDataKeyValue("REG_OWNER_ID"));
        if (regOwnerID > 0)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectRegOwnerByOwnerID(regOwnerID);
            if (Helper.HasRows(ds))
            {
                string ownerName = "";
                DataRow dr = ds.Tables[0].Rows[0];
                if (!string.IsNullOrEmpty(Helper.GetData("NAME", dr)))
                {
                    ownerName = Helper.GetString("NAME", dr).ToString();

                    string pdmsurl = AppSettings.Get("PDMS-URL");
                    Guid requestGuid = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
                    psc.NotifyBackgroundCheckWithoutPrints("BackgroundCheckWithoutPrints", ownerName, "BackgroundCheckWithoutPrints.txt", string.Empty, string.Empty, pdmsurl, WorkflowPage.RegistrationId, requestGuid);
                }

            }
        }
    }

    protected void btnRejectBCReport(object sender, EventArgs e)
    {
        Button Button = (Button)sender;
        GridEditableItem item = (GridEditableItem)Button.NamingContainer;
        int regBCID = Convert.ToInt32(item.GetDataKeyValue("REG_BACKGROUND_CHECK_ID"));
        int regOwnerID = Helper.ConvertStrNullToInt32(item.GetDataKeyValue("REG_OWNER_ID"));
        if (regOwnerID > 0)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectRegOwnerByOwnerID(regOwnerID);
            if (Helper.HasRows(ds))
            {
                string ownerName = "";
                DataRow dr = ds.Tables[0].Rows[0];
                if (!string.IsNullOrEmpty(Helper.GetData("NAME", dr)))
                {
                    ownerName = Helper.GetString("NAME", dr).ToString();

                    string pdmsurl = AppSettings.Get("PDMS-URL");
                    Guid requestGuid = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
                    psc.NotifyBackgroundCheckWithoutPrints("Rejection of Self-Supplied Background Check", ownerName, "BackgroundCheckRejectNotice.txt", string.Empty, string.Empty, pdmsurl, WorkflowPage.RegistrationId, requestGuid);
                }

            }
        }
    }

    public override string ValidationGroup
    {
        get { return "valBackground"; }
    }

    public override string Title
    {
        get { return "Background Check"; }
    }

    public override string IdText
    {
        get { return "ucBackground_" + this.WorkflowPage.RegistrationId; }
    }
}