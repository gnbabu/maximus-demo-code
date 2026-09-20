using MAXIMUS.Controllers.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Skins;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_RegEnrollment : System.Web.UI.UserControl
{
    #region svc
    private PDMSService.PDMSServiceClient _svc;
    public bool IsIndividual = false;

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

    public string currentAction
    {
        get
        {
            if (ViewState["currentAction"] == null) ViewState["currentAction"] = string.Empty;
            return ViewState["currentAction"].ToString();
        }
        set { ViewState["currentAction"] = value; }
    }

    public string previousRegEnrollmentId
    {
        get
        {
            if (ViewState["previousRegEnrollmentId"] == null) ViewState["currentAction"] = string.Empty;
            return ViewState["previousRegEnrollmentId"].ToString();
        }
        set { ViewState["previousRegEnrollmentId"] = value; }
    }

    public string mostRecentRegEnrollmentId
    {
        get
        {
            if (ViewState["mostRecentRegEnrollmentId"] == null) ViewState["mostRecentRegEnrollmentId"] = string.Empty;
            return ViewState["mostRecentRegEnrollmentId"].ToString();
        }
        set { ViewState["mostRecentRegEnrollmentId"] = value; }
    }

    public int WorkflowEventType { get; set; }
    public bool IsReapplication { get; set; }
    public bool IsReactivation { get; set; }
    public bool IsRevalRegOfTerminatedProvider { get; set; }

    public bool disableEnrollmentSpanActions { get; set; }

    private DateTime? ProviderEffectiveDate
    {
        get
        {
            if (ViewState["ProviderEffectiveDate"] != null)
                return Convert.ToDateTime(ViewState["ProviderEffectiveDate"]);
            else
                return null;
        }
        set
        {
            ViewState["ProviderEffectiveDate"] = value;
        }
    }
    public int OldRegID
    {
        get
        {
            return ViewState["OldRegID"] == null ? 0 : Convert.ToInt32(ViewState["OldRegID"]);
        }
        set
        {
            ViewState["OldRegID"] = value;
        }
    }

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

    public int ProcessID
    {
        get
        {
            return ViewState["ProcessID"] == null ? 0 : Convert.ToInt32(ViewState["ProcessID"]);
        }
        set
        {
            ViewState["ProcessID"] = value;
        }
    }

    public string LoadedFrom
    {
        get
        {
            if (ViewState["LoadedFrom"] == null) ViewState["LoadedFrom"] = string.Empty;
            return ViewState["LoadedFrom"].ToString();
        }
        set { ViewState["LoadedFrom"] = value; }
    }

    protected void rgApplicationType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (LoadedFrom != null & LoadedFrom != "")
        {
            refreshDataSource();
        }
    }

    protected void refreshDataSource()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", RegID.ToString());

        // Enrollment Specialist, Compliance Specialist, ODM State Admin can make changes to this page in certain scenarios. 
        if (Helper.IsUserInRoleForUpdatingSpans(HttpContext.Current.User.Identity.Name)
            && (ProcessID > 0 && this.WorkflowPage.WF_StepID > 0))
        {
            if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && !this.WorkflowPage.IsReactivation)
                parms.Add("TABLE_SELECTION", "REG_ENROLLMENT");

            else
                // use staging table instead; the data in the staging table will eventually make it into REG_ENROLLMENT in WFPromoteToActive
                parms.Add("TABLE_SELECTION", "REG_NPI_MEDID_ENROLLMENT_SPAN");
        }
        else
        {
            parms.Add("TABLE_SELECTION", "REG_ENROLLMENT");
        }

        parms.Add("PROCESS_ID", ProcessID.ToString());

        DataSet ds = svc.SelectRegistrationDataWithParams("usp_Select_RegEnrollmentsForProvider", parms);
        rgApplicationType.DataSource = ds;
        lblFailureMsg.Text = "";

        //Display Requested Effective Date
        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["REQUESTED_EFFECTIVE_DATE"] != DBNull.Value)
        {
            lblRequestedEffectiveDate.Text = "Requested Effective Date " + Convert.ToDateTime(ds.Tables[0].Rows[0]["REQUESTED_EFFECTIVE_DATE"]).ToString("MM/dd/yyyy");
        }

        bool isConvertFrmORPWF = false;
        DataSet dsParam = svc.WF_SelectProcessParameters(ProcessID);
        if (Helper.HasRows(dsParam))
        {
            // OHPNM-18353 - Enroll Spl to set the ORP end date span
            isConvertFrmORPWF = string.IsNullOrEmpty(dsParam.Tables[0].Rows[0][CON.ProcessParameter.IsConvertFrmORPWF].ToString()) ? false : Convert.ToBoolean(dsParam.Tables[0].Rows[0][CON.ProcessParameter.IsConvertFrmORPWF].ToString());
        }

        if (Helper.HasRows(ds) && isConvertFrmORPWF)
        {
            DataRow dr = ds.Tables[0].AsEnumerable()
                                              .Where(r => r.Field<string>("ENROLL_STATUS_DESC") == CON.EnrollStatusCodeDesc.REPORTINGONLY.ToString() &&
                                                          r.Field<int>("REG_ID") == this.WorkflowPage.RegistrationId)
                                              .OrderByDescending(r => r["ENROLL_END_DATE_TIME"]).FirstOrDefault();
            if (dr != null)
                hdnEnrollIdORP.Value = dr["REG_NPI_MEDID_ENROLLMENT_SPAN_ID"].ToString();

            DataRow dr1 = ds.Tables[0].AsEnumerable()
                                             .Where(r => r.Field<string>("ENROLL_STATUS_DESC") == CON.EnrollStatusCodeDesc.INACTIVE.ToString() &&
                                                         r.Field<int>("REG_ID") == this.WorkflowPage.RegistrationId)
                                             .OrderByDescending(r => r["ENROLL_END_DATE_TIME"]).FirstOrDefault();
            if (dr1 != null)
                hdnEnrollIdORPInactive.Value = dr1["REG_NPI_MEDID_ENROLLMENT_SPAN_ID"].ToString();           
        }
    }

    protected void rgApplicationType_PreRender(object sender, EventArgs e)
    {
        GridHeaderItem headerItem = (GridHeaderItem)rgApplicationType.MasterTableView.GetItems(GridItemType.Header)[0];
        Image img = new Image();
        img.ImageUrl = "~/Images/help.jpg";
        img.CssClass = "help-img";
        //img.ImageAlign = "Middle";
        headerItem["EnrollmentNpiNumber"].Controls.AddAt(1, img);

        GridHeaderItem headerItem1 = (GridHeaderItem)rgApplicationType.MasterTableView.GetItems(GridItemType.Header)[0];
        Image img1 = new Image();
        img1.ImageUrl = "~/Images/help.jpg";
        img1.CssClass = "help-img";
        headerItem1["EnrollmentPTType"].Controls.AddAt(1, img1);
    }

    protected void rgApplicationType_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

        TextBox txtRegEnrollmentId = (TextBox)e.Item.FindControl("txtRegEnrollmentId");
        TextBox txtProviderEffectiveDate = (TextBox)e.Item.FindControl("txtProviderEffectiveDate");
        TextBox txtProviderEndDate = (TextBox)e.Item.FindControl("txtProviderEndDate");
        TextBox txtStatus = (TextBox)e.Item.FindControl("txtStatus");
        TextBox txtRegId = (TextBox)e.Item.FindControl("txtRegId");


        Dictionary<string, string> paramsForUpdate = new Dictionary<string, string>();
        paramsForUpdate.Add("REG_NPI_MEDID_ENROLLMENT_SPAN_ID", txtRegEnrollmentId.Text);
        paramsForUpdate.Add("REG_ID", txtRegId.Text);
        paramsForUpdate.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        paramsForUpdate.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        bool isValid = ValidateDataItem(txtProviderEffectiveDate.Text, txtProviderEndDate.Text);

        if (!isValid)
        {
            e.Canceled = true;
        }
        else
        {
            if (txtProviderEffectiveDate.Text == "")
            {
                paramsForUpdate.Add("ENROLL_START_DATE_TIME", null);
            }
            else
            {
                paramsForUpdate.Add("ENROLL_START_DATE_TIME", txtProviderEffectiveDate.Text);
            }

            if (txtProviderEndDate.Text == "")
            {
                paramsForUpdate.Add("ENROLL_END_DATE_TIME", null);
            }
            else
            {
                paramsForUpdate.Add("ENROLL_END_DATE_TIME", txtProviderEndDate.Text);
            }

            paramsForUpdate.Add("PROCESS_ID", ProcessID.ToString());

            // update the current row
            svc.UpdateRegistrationDataWithParams("updatereg_npi_medid_enrollment_span", paramsForUpdate);

            // if this is Gap logic, and this is in the inactive row and it's the last row in the table, then we need to add a new active row using the end date they just edited
            if (txtRegId.Text == RegID.ToString() && (((currentAction == CON.EnrollmentSpanOptions.Gap || currentAction == CON.EnrollmentSpanOptions.ConvertFromORPWFGap) && txtStatus.Text == "INACTIVE")
                   || (currentAction == CON.EnrollmentSpanOptions.ConvertFromORPWFNoGap && txtStatus.Text == "REPORTING ONLY")))
            {
                // need to add an active row for the gap
                DateTime newProviderEffectiveDate = Convert.ToDateTime(txtProviderEndDate.Text);
                newProviderEffectiveDate = newProviderEffectiveDate.AddDays(1);
                svc.InsertRegNPIEnrollment(RegID, ProcessID, newProviderEffectiveDate.ToString());

                if (currentAction == CON.EnrollmentSpanOptions.ConvertFromORPWFNoGap && IsRevalRegOfTerminatedProvider)
                {
                    RegistrationController.DeleteRegNPIEnrollment(Convert.ToInt32(hdnEnrollIdORPInactive.Value));
                }
            }
            svc.WF_SaveProcessParameter(this.WorkflowPage.WF_ProcessID, CON.ProcessParameter.NpiMedIDSelection, currentAction);
            refreshDataSource();
        }
    }

    private bool ValidateDataItem(string startDate, string endDate)
    {
        bool isValid = true;
        if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
        {
            if (Convert.ToDateTime(startDate) > Convert.ToDateTime(endDate))
            {
                //AddValidationErrorMessage("* End Date Cannot be less than Effective Date");
                //lblFailureMsg.Visible = true;
                lblFailureMsg.Text = "* End Date Cannot be less than Effective Date";
                isValid = false;
            }

        }
        return isValid;
    }

    protected void rgApplicationType_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            // header row; add tooltips
            GridHeaderItem header = (GridHeaderItem)e.Item;
            header["EnrollmentNpiNumber"].ToolTip = "The NPI is a core data element in your provider file. A change to this field must be made using the homepage link ‘Edit Key Provider Identifiers.";
            header["EnrollmentPTType"].ToolTip = "The Provider Type is a core data element in your provider file. A change to this field must be made using the homepage link ‘Edit Key Provider Identifiers. You will be required to restart your provider application.";
        }

        if (e.Item is GridDataItem)
        {
            // row that's not being edited
            GridDataItem dataItem = (GridDataItem)e.Item;

            String status = dataItem["EnrollentStatus"].Text;
            String regid = dataItem["EnrollmentRegId"].Text;
            String regEnrollmentId = dataItem["REG_NPI_MEDID_ENROLLMENT_SPAN_ID"].Text;
            String providerEffectiveDate = ((Label)dataItem["EnrollmentEffDate"].FindControl("lblEffDate")).Text;
            String providerEndDate = ((Label)dataItem["EnrollmentEffDate"].FindControl("lblEndDate")).Text;

            if (!Helper.IsUserInRoleForUpdatingSpans(HttpContext.Current.User.Identity.Name) || (currentAction == CON.EnrollmentSpanOptions.ChangeEffectiveDate))
            {
                // make sure radio buttons are off for internal roles that shouldn't be editing this page
                rblEnrollmentSpanActions.Enabled = false;
                ((ImageButton)dataItem["EditCommandColumn"].Controls[0]).Visible = false;// to disable the edit button   
                ((Image)dataItem["EnrollmentEffDate"].FindControl("imgEffDate")).Visible = false;// to disable the calendar button
                ((Image)dataItem["EnrollmentEndDate"].FindControl("imgEndDate")).Visible = false;// to disable the calendar button
            }
            else
            {
                if (string.IsNullOrEmpty(this.WorkflowPage.CurrentTaskName) || this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.ProviderReview)
                {
                    // make sure radio buttons are off for internal roles that shouldn't be editing this page
                    rblEnrollmentSpanActions.Enabled = false;
                    ((ImageButton)dataItem["EditCommandColumn"].Controls[0]).Visible = false;// to disable the edit button   
                    ((Image)dataItem["EnrollmentEffDate"].FindControl("imgEffDate")).Visible = false;// to disable the calendar button
                    ((Image)dataItem["EnrollmentEndDate"].FindControl("imgEndDate")).Visible = false;// to disable the calendar button
                }
            }

            // disable edit link if this is NoGap or this is an update that isn't reactivation or reapplication
            if (currentAction == CON.EnrollmentSpanOptions.NoGap ||
                (this.currentAction == "") ||
                (!Helper.IsUserInRoleForUpdatingSpans(HttpContext.Current.User.Identity.Name)) ||
                (WorkflowEventType == CON.WorkflowEventType.UpdateReg && !(IsReactivation || IsReapplication)))
            {
                // don't let them edit anything
                ((ImageButton)dataItem["EditCommandColumn"].Controls[0]).Visible = false;// to disable the edit button   
            }
            // only allow editing on the New and Old Reg ID records for this Process id
            if (currentAction == CON.EnrollmentSpanOptions.ProviderTypeChange && (regid != RegID.ToString() && regid != OldRegID.ToString()))
            {
                // don't let them edit anything
                ((ImageButton)dataItem["EditCommandColumn"].Controls[0]).Visible = false;// to disable the edit button   
            }
            // only allow editing on the New and Old Reg ID records for this Process id
            if ((currentAction == CON.EnrollmentSpanOptions.Gap || currentAction == CON.EnrollmentSpanOptions.ConvertFromORPWFGap || currentAction == CON.EnrollmentSpanOptions.ConvertFromORPWFNoGap)
                && regid != RegID.ToString())
            {
                // don't let them edit anything
                ((ImageButton)dataItem["EditCommandColumn"].Controls[0]).Visible = false;// to disable the edit button   
            }
            // disable effective date calendar icon if they will not eventually be able to edit the effective date
            if (!enableProviderEffectiveDate(regid, status, regEnrollmentId, providerEndDate))
            {
                ((Image)dataItem["EnrollmentEffDate"].FindControl("imgEffDate")).Visible = false;// to disable the calendar button
            }

            // disable end date calendar icon if they will not eventually be able to edit the end date
            if (!enableProviderEndDate(regid, status, regEnrollmentId, providerEndDate))
            {
                ((Image)dataItem["EnrollmentEndDate"].FindControl("imgEndDate")).Visible = false;// to disable the calendar button
            }

            // store off latest ProviderEffectiveDate for ChangeEffectiveDate action
            if (currentAction == CON.EnrollmentSpanOptions.ChangeEffectiveDate)
            {
                ((ImageButton)dataItem["EditCommandColumn"].Controls[0]).Visible = false;// to disable the edit button 
                ProviderEffectiveDate = Convert.ToDateTime(providerEffectiveDate);
            }
        }

        if ((e.Item is GridEditFormItem) && e.Item.IsInEditMode)
        {
            // row that's being edited
            var gridEditFormItem = (GridEditFormItem)e.Item;

            TextBox txtRegEnrollmentId = (TextBox)gridEditFormItem.FindControl("txtRegEnrollmentId");
            TextBox txtNPI = (TextBox)gridEditFormItem.FindControl("txtNpi");
            TextBox txtPTType = (TextBox)gridEditFormItem.FindControl("txtPTType");
            TextBox txtRegId = (TextBox)gridEditFormItem.FindControl("txtRegId");
            TextBox txtMedicaidId = (TextBox)gridEditFormItem.FindControl("txtMedicaidId");
            TextBox txtProviderEffectiveDate = (TextBox)gridEditFormItem.FindControl("txtProviderEffectiveDate");
            TextBox txtProviderEndDate = (TextBox)gridEditFormItem.FindControl("txtProviderEndDate");
            TextBox txtUsername = (TextBox)gridEditFormItem.FindControl("txtUsername");
            TextBox txtStatus = (TextBox)gridEditFormItem.FindControl("txtStatus");

            Helper.SetReadOnlyForSpecificControl(txtNPI, true, "formFieldReadOnly");
            Helper.SetReadOnlyForSpecificControl(txtPTType, true, "formFieldReadOnly");
            Helper.SetReadOnlyForSpecificControl(txtRegId, true, "formFieldReadOnly");
            Helper.SetReadOnlyForSpecificControl(txtMedicaidId, true, "formFieldReadOnly");
            Helper.SetReadOnlyForSpecificControl(txtUsername, true, "formFieldReadOnly");
            Helper.SetReadOnlyForSpecificControl(txtStatus, true, "formFieldReadOnly");

            String status = gridEditFormItem["EnrollentStatus"].Text;
            String regid = gridEditFormItem["EnrollmentRegId"].Text;

            // if this is NewApp or Gap's active row or ProviderTypeChange Active row or Change Effective Date Active Row, let them edit the provider effective date
            if (!enableProviderEffectiveDate(regid, status, txtRegEnrollmentId.Text, txtProviderEndDate.Text))
            {
                Helper.SetReadOnlyForSpecificControl(txtProviderEffectiveDate, true, "formFieldReadOnly");
            }

            // if this is Gap, the REG_IDs match, and it's the INACTIVE one and it's either the last or previous enrollment record, then make the end date editable
            // or it's the change provider one and this is the old provider's active row
            if (!enableProviderEndDate(regid, status, txtRegEnrollmentId.Text, txtProviderEndDate.Text))
            {
                // make end date not editable for this provider
                Helper.SetReadOnlyForSpecificControl(txtProviderEndDate, true, "formFieldReadOnly");
            }
        }
    }

    private bool enableProviderEffectiveDate(String regid, String status, String txRegEnrollmentId, string providerEndDate)
    {
        // if this is NewApp or Gap's active row or ProviderTypeChange Active row or Change Effective Date Active Row, let them edit the provider effective date
        if (
            (currentAction == CON.EnrollmentSpanOptions.NewApp && regid == RegID.ToString() && (status == "ACTIVE" || status == "REPORTING ONLY")) ||
            ((currentAction == CON.EnrollmentSpanOptions.Gap || currentAction == CON.EnrollmentSpanOptions.ConvertFromORPWFGap) && regid == RegID.ToString() && string.IsNullOrEmpty(providerEndDate)) ||
            (currentAction == CON.EnrollmentSpanOptions.ProviderTypeChange && regid == RegID.ToString() && (status == "ACTIVE" || status == "REPORTING ONLY"))
           )
        {
            // enable provider effective date for new applications (by not disabling them)
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool enableProviderEndDate(String regid, String status, String txRegEnrollmentId, string providerEndDate)
    {
        // if this is Gap, the REG_IDs match, and it's the INACTIVE one and it's either the last or previous enrollment record, then make the end date editable
        // or it's the change provider one and this is the old provider's active row
        if (
            ((currentAction == CON.EnrollmentSpanOptions.Gap || currentAction == CON.EnrollmentSpanOptions.ConvertFromORPWFGap) && regid == RegID.ToString() && !string.IsNullOrEmpty(providerEndDate) && status == "INACTIVE")
            || (currentAction == CON.EnrollmentSpanOptions.ProviderTypeChange && regid == OldRegID.ToString() && status == "ACTIVE")
            || (currentAction == CON.EnrollmentSpanOptions.ConvertFromORPWFNoGap && regid == RegID.ToString()
                 && !string.IsNullOrEmpty(providerEndDate) && status == "REPORTING ONLY" && txRegEnrollmentId == hdnEnrollIdORP.Value)
            )
        {
            // gap, make inactive end date editable for this provider
            return true;
        }
        else
        {
            // make end date not editable for this provider
            return false;
        }
    }

    public void InitViewForAdmin(int regID)
    {
        RegID = regID;
        ProcessID = 0;
        LoadedFrom = "ChangeEffectiveDate";

        this.currentAction = CON.EnrollmentSpanOptions.ChangeEffectiveDate;
        rgApplicationType.Rebind();
    }

    public void InitViewForProvider(int regID, int processId)
    {
        RegID = regID;
        ProcessID = processId;
        LoadedFrom = "ProviderScreen";

        LoadRegData();
    }

    private void LoadRegData()
    {
        var regData = svc.SelectRegistrationByRegID(RegID);

        WorkflowEventType = string.IsNullOrEmpty(regData.Tables[0].Rows[0]["WORKFLOW_EVENT_TYPE_ID"].ToString()) ? 0 : Convert.ToInt32(regData.Tables[0].Rows[0]["WORKFLOW_EVENT_TYPE_ID"].ToString());
        IsReapplication = Helper.GetBool("ISReapplication", regData.Tables[0].Rows[0]);
        IsReactivation = Helper.GetBool("IsProviderReactivation", regData.Tables[0].Rows[0]);
        DateTime oldtermDate = Helper.GetDateTime("TerminationDate", regData.Tables[0].Rows[0]);
        IsRevalRegOfTerminatedProvider = (!Helper.IsDateNull(oldtermDate) && WorkflowEventType == CON.WorkflowEventType.RevalReg); // if it has a term date and is a revalreg

        var newReg = svc.SelectProviderTypeChangeRequestByNewReg(this.RegID, ProcessID);
        if (Helper.HasRows(newReg))
        {
            OldRegID = Convert.ToInt32(newReg.Tables[0].Rows[0]["OLD_REG_ID"]);
        }
        BindDropDowns();

        refreshDataSource();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    private void BindDropDowns()
    {
        BindEnrollmentSpanActions();
    }

    private void BindEnrollmentSpanActions()
    {
        DataSet dsEnrollActions = svc.GetNpiMedIDEnrollmentSpanActions();
        string rblListItemName = string.Empty;
        string rblListItemValue = string.Empty;
        bool setrblActions = false;
        disableEnrollmentSpanActions = false;

        // if we haven't added the items yet, add them now
        if (rblEnrollmentSpanActions.Items.Count == 0)
        {

            foreach (DataRow dr in dsEnrollActions.Tables[0].Rows)
            {
                rblListItemName = dr["NPI_MEDID_ENROLLMENT_SPAN_ACTIONS_DESCRIPTION"].ToString();
                rblListItemValue = dr["NPI_MEDID_ENROLLMENT_SPAN_ACTIONS_CODE"].ToString();
                rblEnrollmentSpanActions.Items.Add(new ListItem(rblListItemName, rblListItemValue));
            }
        }

        if (LoadedFrom == "ChangeEffectiveDate")
        {
            setrblActions = true;
            currentAction = CON.EnrollmentSpanOptions.ChangeEffectiveDate;
        }
        else
        {
            string enrollmentSelection = string.Empty;
            bool isRevertSuspensionWF = false;
            bool isConvertFrmORPWF = false;
            DataSet ds = svc.WF_SelectProcessParameters(ProcessID);
            if (Helper.HasRows(ds))
            {
                enrollmentSelection = ds.Tables[0].Rows[0][CON.ProcessParameter.NpiMedIDSelection].ToString();
                isRevertSuspensionWF = string.IsNullOrEmpty(ds.Tables[0].Rows[0][CON.ProcessParameter.IsRevertSuspension].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0][CON.ProcessParameter.IsRevertSuspension].ToString());
                // OHPNM-18353 - Enroll Spl to set the ORP end date span
                isConvertFrmORPWF = string.IsNullOrEmpty(ds.Tables[0].Rows[0][CON.ProcessParameter.IsConvertFrmORPWF].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0][CON.ProcessParameter.IsConvertFrmORPWF].ToString());
            }

            if (!string.IsNullOrEmpty(enrollmentSelection) && string.IsNullOrEmpty(currentAction))
            {
                rblEnrollmentSpanActions.SelectedIndex = rblEnrollmentSpanActions.Items.IndexOf(rblEnrollmentSpanActions.Items.FindByValue(enrollmentSelection));
                currentAction = enrollmentSelection;
                //rgApplicationType.Rebind();
                //rgApplicationType.MasterTableView.ClearEditItems();
            }

            switch (WorkflowEventType)
            {
                case CON.WorkflowEventType.NewReg:
                    currentAction = CON.EnrollmentSpanOptions.NewApp;
                    refreshDataSource(); // we need to call this first for NewApp to get it to copy over any REG_ENROLLMENT from the other REG_IDs with same NPI first
                    addEnrollmentIfNeeded();
                    setrblActions = true;
                    break;
                case CON.WorkflowEventType.UpdateReg:
                case CON.WorkflowEventType.RevalReg:
                    if (isConvertFrmORPWF)
                    {
                        foreach (ListItem itm in rblEnrollmentSpanActions.Items)
                        {
                            if (isConvertFrmORPWF)
                            {
                                if (IsRevalRegOfTerminatedProvider && (itm.Value == CON.EnrollmentSpanOptions.ConvertFromORPWFNoGap || itm.Value == CON.EnrollmentSpanOptions.ConvertFromORPWFGap))
                                    itm.Enabled = true;
                                else if (!IsRevalRegOfTerminatedProvider && itm.Value == CON.EnrollmentSpanOptions.ConvertFromORPWFNoGap)
                                    itm.Enabled = true;
                                else
                                    itm.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        if (IsReactivation || IsReapplication || IsRevalRegOfTerminatedProvider)
                        {
                            foreach (ListItem itm in rblEnrollmentSpanActions.Items)
                            {
                                if (itm.Value != CON.EnrollmentSpanOptions.Gap && itm.Value != CON.EnrollmentSpanOptions.NoGap)
                                    itm.Enabled = false;
                                else if (isRevertSuspensionWF && itm.Value == CON.EnrollmentSpanOptions.Gap)
                                {
                                    itm.Enabled = false;
                                    currentAction = CON.EnrollmentSpanOptions.NoGap;
                                }
                                else
                                    itm.Enabled = true;
                            }
                        }
                        else
                        {
                            foreach (ListItem itm in rblEnrollmentSpanActions.Items)
                            {
                                itm.Enabled = false;
                            }
                            disableEnrollmentSpanActions = true;
                        }
                    }
                    setrblActions = false;
                    break;
                case CON.WorkflowEventType.ChangeProviderType:
                    currentAction = CON.EnrollmentSpanOptions.ProviderTypeChange;
                    refreshDataSource(); // we need to call this first for providerTypeChange to get it to copy over any REG_ENROLLMENT from the old REG_ID first
                    addEnrollmentIfNeeded();
                    setrblActions = true;
                    break;
                case CON.WorkflowEventType.Reconsideration:
                    currentAction = CON.EnrollmentSpanOptions.NoGap;
                    setrblActions = true;
                    break;
                case CON.WorkflowEventType.CredentialReconsideration:
                    currentAction = CON.EnrollmentSpanOptions.NoGap;
                    setrblActions = true;
                    break;
                default:
                    currentAction = string.Empty;
                    setrblActions = true;
                    break;
            }

        }
        if (setrblActions)
        {
            if (!string.IsNullOrEmpty(currentAction))
            {
                foreach (ListItem itm in rblEnrollmentSpanActions.Items)
                {
                    if (itm.Value.ToUpper() == currentAction.ToUpper())
                    {
                        itm.Selected = true;
                    }
                    else
                    {
                        itm.Enabled = false;
                    }
                }
            }
            else
            {
                foreach (ListItem itm in rblEnrollmentSpanActions.Items)
                {
                    itm.Enabled = false;
                }
            }
        }
    }

    protected void rblEnrollmentSpanActions_SelectedIndexChanged(object sender, EventArgs e)
    {
        currentAction = rblEnrollmentSpanActions.SelectedValue;

        // refresh the table so we can react to the selected action
        rgApplicationType.Rebind();

        // close the edit table too if it's open; that would be weird to leave it open
        rgApplicationType.MasterTableView.ClearEditItems();
    }

    public int SaveData()
    {
        int result = svc.ValidateNPIandMEDidData(RegID, ProcessID, currentAction);

        if (currentAction == string.Empty && !disableEnrollmentSpanActions)
            result = 6;

        if (result == 0)
            // save off the radio button they selected, so we can use it in WFPromoteToActive later on        
            svc.WF_SaveProcessParameter(this.WorkflowPage.WF_ProcessID, CON.ProcessParameter.NpiMedIDSelection, currentAction);

        return result;
    }

    private void addEnrollmentIfNeeded()
    {
        if (Helper.IsUserInRoleForUpdatingSpans(HttpContext.Current.User.Identity.Name)
            && ProcessID > 0 && this.WorkflowPage.WF_StepID > 0)
            svc.InsertRegNPIEnrollment(RegID, ProcessID, null);
    }

    public DateTime? getLatestEffectiveDate()
    {
        // for changeEffectiveDate, return their latest effective date; we really can't get this from the REG_NPI_MEDID_ENROLLMENT_SPAN table since it's using processId = 0 for this action
        // we need to return what's on the table on the screen, so retroEffectiveView can see if it's OK to use to save to REG_ENROLLMENT table
        return ProviderEffectiveDate;
    }
}