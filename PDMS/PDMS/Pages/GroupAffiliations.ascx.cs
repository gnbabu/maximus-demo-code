using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Pages_GroupAffiliations : BaseSectionControl
{
  
 
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public delegate void SaveDataEventHandler();
    public event SaveDataEventHandler SaveDataEvent;

    private String CurrentSortOrder = "ASC";
    private String CurrentSortField = "Name";

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
    #region grdGroupAffiliations
    DataTable _dtGroupAffiliations;

    bool _isIndividualEditable = false;
    public DataTable dtGroupAffiliations
    {
        get
        {
            if (_dtGroupAffiliations == null)
            {
                _dtGroupAffiliations = this.GetGroupAffiliations();
            }

            return _dtGroupAffiliations;
        }
    }

    public bool isIndividualEditable
    {
        get
        {
            if (!_isIndividualEditable)
                _isIndividualEditable = Registration.AllowGroupToUpdateIndividualMember(this.WorkflowPage.RegistrationId);

            return _isIndividualEditable;
        }
    }

    #endregion
    #region Section
    private enum PopupName { GroupAffiliations = 0, GroupAffiliationsHistory = 1 };
    #endregion


    public System.EventHandler InvalidateAgreements;

    private bool isApplicationInWorkflow = false;

    private void SetButtons()
    {
        txtFilterName1.Attributes.Add("onKeyPress", "doClick('" + btnFilterGrid.ClientID + "',event)");
        txtFilterNPI.Attributes.Add("onKeyPress", "doClick('" + btnFilterGrid.ClientID + "',event)");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        hdnRegId.Value = this.WorkflowPage.RegistrationId.ToString();
        EnableUIFilters();

        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);

        if (pageTypeID == CON.RegistrationPageType.GroupAffiliations)
        {
            SetButtons();

            this.ucGroupAffiliations.SaveEvent += new PopupControls_GroupAffiliationsCtrl.SaveEventHandler(UpdateAffiliations);
            this.ucGroupAffiliations.ValidationEvent += new PopupControls_GroupAffiliationsCtrl.ValidationEventHandler(KeepPopupOpen);
            this.ucGroupAffiliations.ErrorEvent += new PopupControls_GroupAffiliationsCtrl.ErrorEventHandler(KeepPopupOpen);
            this.ucGroupAffiliations.KeepOpenEvent += new PopupControls_GroupAffiliationsCtrl.KeepOpenEventHandler(KeepPopupOpen);
            this.ucGroupAffiliations.CancelEvent += new PopupControls_GroupAffiliationsCtrl.CancelEventHandler(CancelPopup);
            // this.ucGroupAffiliationsHistory.CancelEvent += new PopupControls_GroupAffiliationsHistory.CancelEventHandler(CancelPopup);
        }
        DisplayIndividualMemberEditMessage();

        string logMsg = String.Format("GroupAffiliations, RegID - " + this.WorkflowPage.RegistrationId.ToString() + " by " + HttpContext.Current.User.Identity.Name + ", Role(s) - " + String.Join(", ", Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name)));
        Logging log = new Logging(Guid.NewGuid(), logMsg);
        log.CreateLogEntry(string.Format(logMsg, Logging.LogPriority.Information));
    }

    private void EnableUIFilters()
    {
        this.rblGridActiveStatusID.Enabled = true;
        this.rblAffiliationSearch.Enabled = true;
        this.txtFilterName1.Enabled = true;
        this.txtFilterNPI.Enabled = true;
        this.ddlAffiliationStatus.Enabled = true;
        this.btnFilterGrid.Enabled = true;
        this.btnClearFilter.Enabled = true;
    }

    public override void LoadData(DataRow row = null)
    {
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        //changes for bug DCPDMS-2266 to show page heading based on provider type
        PA.Visible = false;
        PCA.Visible = false;
        Group.Visible = false;
        Header1.Visible = false;
        Header2.Visible = false;
        Header3.Visible = false;
        if (Registration.IsEPDProvider(this.WorkflowPage.RegistrationId) || Registration.IsHomeHealthProvider(this.WorkflowPage.RegistrationId))
        {
            PCA.Visible = true;
            Header3.Visible = true;
        }
        else if (Registration.CanIndividualAddPA(this.WorkflowPage.RegistrationId))
        {
            PA.Visible = true;
            Header2.Visible = true;
        }
        else
        {
            Group.Visible = true;
            Header1.Visible = true;
        }

        if (grdGroupAffiliations.Rows.Count == 0)
        {
            RefreshAffiliates();
        }
        LoadAffiliationStatusDefs();
        // btnGroupHistory.Visible = grdGroupAffiliations.Rows.Count > 0;
        btnAdd.Visible = !Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName);
        lnkExcel.Visible = lnkPDF.Visible = lnkExcel1.Visible = lnkPDF1.Visible = grdGroupAffiliations.Rows.Count > 0;

        // OHPNM-1917
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
            
            Helper.SetReadOnly(this.rblGridActiveStatusID, false);
            Helper.SetReadOnly(this.rblAffiliationSearch, false);
            Helper.SetReadOnly(this.txtFilterName1, false);
            Helper.SetReadOnly(this.txtFilterNPI, false);
            Helper.SetReadOnly(this.ddlAffiliationStatus, false);
            Helper.SetReadOnly(this.btnFilterGrid, false);
            Helper.SetReadOnly(this.btnClearFilter, false);
            lnkExcel.Enabled = true;
            lnkPDF.Enabled = true;
            lnkExcel1.Enabled = true;
            lnkPDF1.Enabled = true;
        }
    }

    private void DisplayIndividualMemberEditMessage()
    {
        if (!Page.IsPostBack)
        {
            updateMsg.Visible = (SessionVarRetriever.IsGroup) ? true : false;
        }
        else
        {
            updateMsg.Visible = false;
        }
        this.updSuccess.Update();
    }

    private void LoadGroupAffiliations()
    {
        grdGroupAffiliations.DataSource = GetSortedData();
        grdGroupAffiliations.DataBind();
    }

    private void LoadAffiliationStatusDefs()
    {

        this.rptStatusDef.DataSource = this.ddlAffiliationStatus.DataSource = GetGroupAffiliationStatuses();
        this.rptStatusDef.DataBind();
        this.ddlAffiliationStatus.DataTextField = "DESCRIPTION";
        this.ddlAffiliationStatus.DataValueField = "DESCRIPTION";
        this.ddlAffiliationStatus.DataBind();
        this.ddlAffiliationStatus.Items.Insert(0, "");
        this.ddlAffiliationStatus.SelectedIndex = 0;
        checkrblAffiliationSearch(false);
    }

    protected void onActiveChanged(object sender, EventArgs e)
    {
        checkrblAffiliationSearch(true);
    }

    protected void onAffiliationViewChange(object sender, EventArgs e)
    {
        if (rblGridActiveStatusID.SelectedValue.Equals("1"))
        {
            txtFilterName1.Text = string.Empty;
            txtFilterNPI.Text = string.Empty;
            this.ddlAffiliationStatus.SelectedIndex = 0;
            SetSearchFiltersInSession();
            ResetSelectedProvider();
            RefreshAffiliates();
        }
        else
        {
            txtFilterName1.Text = string.Empty;
            txtFilterNPI.Text = string.Empty;
            this.ddlAffiliationStatus.SelectedIndex = 0;
            SetSearchFiltersInSession();
            ResetSelectedProvider();
            RefreshAffiliates();
        }
    }

    private void checkrblAffiliationSearch(bool refreshAffiliation)
    {
        if (rblAffiliationSearch.SelectedValue.Equals("1"))
        {
            this.ddlAffiliationStatus.Enabled = false;
            this.ddlAffiliationStatus.SelectedIndex = 0;
            if (refreshAffiliation)
            {
                RefreshAffiliates();
            }
        }
        else
        {
            this.ddlAffiliationStatus.Enabled = true;
            if (refreshAffiliation)
            {
                RefreshAffiliates();
            }
        }
    }

    public override bool SaveData()
    {
        return !Registration.PreviewingRegistrationSection();
    }

    public override bool ValidateData()
    {
        bool isGood = true;

        if (Registration.PreviewingRegistrationSection())
        {
            AddValidationErrorMessage("*Previous registration sections must be completed first.");
            isGood = false;
        }
        else
        {
            if (!GridDataValid()) isGood = false;
            var Nodes = this.WorkflowPage.RegistrationNodes;
            int required = Nodes.Where(s => s.Value.Step == CON.SectionTypeID.Affiliations)
                                .Select(d => d.Value.IsRequired).Max();
            if (required == 1)
            {
                if (grdGroupAffiliations.Rows.Count == 0 && Registration.EntryIsRequired(this.WorkflowPage.RegistrationId, CON.RegistrationPageName.GroupAffiliations, Registration.GetStepText(CON.SectionTypeID.Affiliations)))
                {
                    AddValidationErrorMessage(string.Format("*At least one entry in {0} is required.", CON.RegistrationPageName.GroupAffiliations));
                    isGood = false;
                }
            }
            else
            {
                isGood = true;
            }
        }
        return isGood;
    }
    public override bool ValidateSaveNextData()
    {
        bool isGood = true;
        int mmisProviderTypeId = 0;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        if (Helper.HasRows(ds))
        {
            mmisProviderTypeId = Helper.GetInt("MMIS_PROVIDER_TYPE_ID", ds.Tables[0].Rows[0]);
        }

        if (mmisProviderTypeId == 21 || mmisProviderTypeId == 31)
        {
            if (grdGroupAffiliations.Rows.Count == 0)
            {
                AddValidationErrorMessage("An affiliation is required for enrollment, please add an affiliation.");
                isGood = false;
            }
            else
            {
                var active = false;
                foreach (GridViewRow row in grdGroupAffiliations.Rows)
                {
                    string npi = ((System.Web.UI.WebControls.TableCell)row.Controls[1]).Text;
                    string providerType = ((System.Web.UI.WebControls.TableCell)row.Controls[2]).Text;

                    if (psc.IsEnrollmentActive(npi.Trim(), providerType.Trim()))
                    {
                        active = true;
                        break;
                    }
                }
                if (!active)
                {
                    AddValidationErrorMessage("An Active affiliation is required for enrollment, please add an affiliation.");
                    isGood = false;
                }
            }
        }
        return isGood;
    }
    public override bool HasInputValue()
    {
        //bool rtn = false;
        //if (grdGroupAffiliations != null)
        //{
        //    if (grdGroupAffiliations.Rows.Count > 0)
        //    {
        //        rtn = true;
        //    }
        //}
        //return rtn;
        var Nodes = this.WorkflowPage.RegistrationNodes;
        int required = Nodes.Where(s => s.Value.Step == CON.SectionTypeID.Affiliations)
                            .Select(d => d.Value.IsRequired).Max();
        if (required == 0)
        {
            return true;

        }
        else
        {
            return false;
        }
    }
    private bool GridDataValid()
    {
        bool isValid = true;

        Int64 testlong = 0;
        DateTime testdate;

        string name;
        string npi;
        string startDate;
        string endDate;
        string affiliationStatus;
        string medicaidId;

        if (Helper.HasRows(dtGroupAffiliations))
        {
            foreach (DataRow row in dtGroupAffiliations.Rows)
            {
                name = Helper.GetData("NAME", row);
                npi = Helper.GetData("NPI", row);
                startDate = Helper.GetData("START_DATE", row);
                endDate = Helper.GetData("END_DATE", row);
                affiliationStatus = Helper.GetData("AffiliationStatus", row);

                if (string.IsNullOrEmpty(name))
                    isValid = AddValidationErrorMessage("* Name is required");

                //if (string.IsNullOrEmpty(npi))
                //    isValid = AddValidationErrorMessage("* NPI is required");
                if (!string.IsNullOrEmpty(npi))
                {
                    if (npi[0] == '0')
                        isValid = AddValidationErrorMessage("* NPI leading zero not allowed");
                    else if (!Int64.TryParse(npi, out testlong) || npi.Length != 10)
                        isValid = AddValidationErrorMessage("* NPI requires 10 digits");
                }

                if (string.IsNullOrEmpty(startDate))
                    isValid = AddValidationErrorMessage("* Start Date is required");
                else if (!DateTime.TryParse(startDate, out testdate))
                    isValid = AddValidationErrorMessage("* Start Date is invalid");

                if (!string.IsNullOrEmpty(endDate) && !string.IsNullOrEmpty(startDate))
                    if (!DateTime.TryParse(endDate, out testdate))
                        isValid = AddValidationErrorMessage("* End Date is invalid");
                    else
                    {
                        DateTime d1, d2;
                        if (DateTime.TryParse(startDate, out d1) && DateTime.TryParse(endDate, out d2)
                            && (affiliationStatus.ToUpper() == "CONFIRMED" || affiliationStatus.ToUpper() == "PENDING REMOVAL"))
                            if (d2 < d1)
                            {
                                medicaidId = Helper.GetData("MedicaidID", row);
                                string invalidRowDetail = ((string.IsNullOrWhiteSpace(name)) ? "" : "Name: " + name + " ")
                                                    + ((string.IsNullOrWhiteSpace(npi)) ? "" : "NPI: " + npi + " ")
                                                    + ((string.IsNullOrWhiteSpace(medicaidId)) ? "" : "Medicaid Id: " + medicaidId);

                                string errorMessage = (string.IsNullOrEmpty(invalidRowDetail)) ?
                                                    "* Date span is invalid for Name: N/A, NPI: N/A, Medicaid Id: N/A" :
                                                    "* Date span is invalid for " + invalidRowDetail;
                                isValid = AddValidationErrorMessage(errorMessage);
                            }
                    }
            }
        }
            
        return isValid;
    }

    protected void grdGroupAffiliations_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("GroupAffiliations") || e.CommandName.Equals("DeleteAffiliation") || e.CommandName.Equals("LinkName"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int regAffiliationID = string.IsNullOrEmpty(this.grdGroupAffiliations.DataKeys[index].Values["REG_AFFILIATION_ID"].ToString()) ? 0 : (int)this.grdGroupAffiliations.DataKeys[index].Values["REG_AFFILIATION_ID"];
            int regID = 0;
            string affiliationStatus = string.Empty;
            switch (e.CommandName)
            {
                case "GroupAffiliations":
                    lblTitle.Text = "Edit Group Member";
                    ucGroupAffiliations.LoadData(regAffiliationID);
                    mltPopup.ActiveViewIndex = Convert.ToInt32(PopupName.GroupAffiliations);
                    mpe.Show();
                    break;
                case "DeleteAffiliation":
                    if (regAffiliationID > 0)
                    {
                        string logMsg = String.Format("Affiliation Delete with RegAffiliationID " + regAffiliationID.ToString() + " by " + HttpContext.Current.User.Identity.Name + ", Role(s) - " + String.Join(", ", Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name)));
                        Logging log = new Logging(Guid.NewGuid(), logMsg);
                        log.CreateLogEntry(string.Format(logMsg, Logging.LogPriority.Information));
                        this.DeleteRegAfiliation(regAffiliationID);
                    }
                    break;
                case "LinkName":
                    if (regAffiliationID > 0)
                    {
                        GridViewRow gvRow = grdGroupAffiliations.Rows[index];
                        if (gvRow != null)
                        {
                            HiddenField hdnField = (HiddenField)gvRow.FindControl("HdnIndividualRegId");
                            regID = string.IsNullOrEmpty(hdnField.Value) ? 0 : Convert.ToInt32(hdnField.Value);
                            affiliationStatus = Server.HtmlDecode(gvRow.Cells[5].Text.Trim());
                            if (affiliationStatus == "Active" && regID > 0 && isIndividualEditable)
                            {

                                SessionVarRetriever.IndividualRegID = regID;
                                SessionVarRetriever.GroupUserRegID = this.WorkflowPage.RegistrationId;
                                SessionVarRetriever.IsGroup = false;
                                Response.Redirect("~/Process/Registration.aspx");
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    protected void DisplayTenCN(Object sender, EventArgs e)
    {
        Session["grdPageSize"] = "10";
        grdGroupAffiliations.PageIndex = 0;
        RefreshAffiliates();
    }

    protected void DisplayFiftyCN(Object sender, EventArgs e)
    {
        Session["grdPageSize"] = "50";
        grdGroupAffiliations.PageIndex = 0;
        RefreshAffiliates();
    }

    protected void DisplayHundredCN(Object sender, EventArgs e)
    {
        Session["grdPageSize"] = "100";
        grdGroupAffiliations.PageIndex = 0;
        RefreshAffiliates();
    }

    protected void btnAdd_Click(object sender, CommandEventArgs e)
    {

        switch (e.CommandName)
        {
            case "GroupAffiliations":
                lblTitle.Text = "Add Group Member";
                ucGroupAffiliations.LoadData(0);
                mltPopup.ActiveViewIndex = Convert.ToInt32(PopupName.GroupAffiliations);
                mpe.Show();
                break;
            default:
                break;
        }
    }
	
    private bool showButtonsBecauseOfStatus(int groupAffiliationStatusId)
    {
        if (groupAffiliationStatusId == CON.GroupAffiliationStatusTypeID.IndividualEnrollmentPendingApproval || 
            groupAffiliationStatusId == CON.GroupAffiliationStatusTypeID.PendingApproval || 
            groupAffiliationStatusId == CON.GroupAffiliationStatusTypeID.MemberNotFound)
        {
            // enable delete button because status is 1 (Individual Enrollment Pending Approval), 8 (Pending Approval), or 9 (Member Not Found)
            return true;
        }

        return false;
    }

    private bool hideButtonsBecauseOfStatus(int groupAffiliationStatusId)
    {
        if (groupAffiliationStatusId == CON.GroupAffiliationStatusTypeID.RemovedbyGroup)
        {
            // disable edit button because status is 6 (Removed)
            return true;
        }

        return false;
    }

    protected void grdGroupAffiliations_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int statusID = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "GROUP_AFFILIATION_STATUS_ID"));
            if ((statusID != 4 && statusID != 3))
            {
                e.Row.Cells[0].CssClass = "failureNotification";
                e.Row.Cells[1].CssClass = "failureNotification";
                e.Row.Cells[2].CssClass = "failureNotification";
                e.Row.Cells[3].CssClass = "failureNotification";
                e.Row.Cells[4].CssClass = "failureNotification";
                e.Row.Cells[5].CssClass = "failureNotification";
                e.Row.Cells[6].CssClass = "failureNotification";
                e.Row.Cells[7].CssClass = "failureNotification";
                e.Row.Cells[8].CssClass = "failureNotification";
                e.Row.Cells[9].CssClass = "failureNotification";
            }
            LinkButton btn = (LinkButton)e.Row.FindControl("btnNameLink");
            Label lbl = (Label)e.Row.FindControl("lblNameLink");

            string medicaidID = DataBinder.Eval(e.Row.DataItem, "MedicaidID").ToString();
            string revalDueWindow = AppSettings.Get("RevalidationDueWindow");
            bool validProviderDrillDown = IsValidProviderDrilldownStatus(statusID);
            bool validDeleteStatus = IsDeleteStatus(statusID, medicaidID);

            btn.Style["display"] = validProviderDrillDown ? "block" : "none";
            lbl.Style["display"] = validProviderDrillDown ? "none" : "block";

            if (isGroupAllowedToUpdateIndividual(statusID))
            {

                btn.Style["display"] = "block";
                lbl.Style["display"] = "none";

            }


            // Bug 4022 - Do not display the Edit button if the Affiliation is LOCKED.
            LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
            if (btnEdit != null)
            {
                if (hideButtonsBecauseOfStatus(Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "GROUP_AFFILIATION_STATUS_ID"))))
                {
                    btnEdit.Visible = false;
                }
            }

            LinkButton btnDel = (LinkButton)e.Row.FindControl("btnDelete");
            if (btnDel != null)
            {
                btnDel.Visible = false;

                if (showButtonsBecauseOfStatus(Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "GROUP_AFFILIATION_STATUS_ID")))) {
                    btnDel.Visible = true;
                }
                else if (Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInAdminRole(HttpContext.Current.User.Identity.Name))
                {
                    btnDel.Visible = true;
                }
                else
                {
                    btnDel.Visible = validDeleteStatus;
                }
            }

            if (statusID == CON.GroupAffiliationStatusTypeID.ConfirmGroupMember || statusID == CON.GroupAffiliationStatusTypeID.PendingApproval)
            {
                e.Row.BackColor = System.Drawing.Color.Yellow;
            }
            //RevalidationDate
            if (!string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "RevalidationDate").ToString()))
            {
                DateTime revalidationDatevar = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "RevalidationDate"));

                if (((revalidationDatevar - DateTime.Now).TotalDays > 0 && (revalidationDatevar - DateTime.Now).TotalDays < Convert.ToInt32(revalDueWindow)) && (statusID == CON.GroupAffiliationStatusTypeID.IndividualEnrollmentPendingApproval || statusID == CON.GroupAffiliationStatusTypeID.IndividualRequiresReValidation))
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[7].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[8].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[6].Font.Bold = true;
                }
            }
            DataRowView drv = (DataRowView)e.Row.DataItem;
            int mydatakey = drv["RETRO_REVIEW_REQUIRED_ID"].ToString() == "" ? 0 : Convert.ToInt32(drv["RETRO_REVIEW_REQUIRED_ID"]);
            //if (mydatakey == CON.GroupMemberRetroStatusID.RetroReviewRequired)
            //e.Row.Cells[7].Text = "Confirm Retro";
        }
    }

    protected void grdGroupAffiliations_Sorting(Object sender, GridViewSortEventArgs e)
    {
        if (grdGroupAffiliations.Attributes["CurrentSortDirection"] == "ASC")
        {
            grdGroupAffiliations.Attributes["CurrentSortDirection"] = "DESC";
            CurrentSortOrder = "DESC";
        }
        else
        {
            grdGroupAffiliations.Attributes["CurrentSortDirection"] = "ASC";
            CurrentSortOrder = "ASC";
        }
        CurrentSortField = e.SortExpression;
        ViewState["CurSortField"] = e.SortExpression;
        ViewState["CurrentSortDirection"] = grdGroupAffiliations.Attributes["CurrentSortDirection"].ToString();
        grdGroupAffiliations.Attributes["CurrentSortField"] = CurrentSortField;

        this.ResetSelectedProvider();
        this.RefreshAffiliates();
    }

    protected void grdGroupAffiliations_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdGroupAffiliations.PageIndex = e.NewPageIndex;
        this.ResetSelectedProvider();
        this.RefreshAffiliates();
    }

    private void RefreshAffiliates(string type = "")
    {
        string name = ViewState["txtFilterName1"] != null ? ViewState["txtFilterName1"].ToString() : txtFilterName1.Text.Trim();
        string npi = ViewState["txtFilterNPI"] != null ? ViewState["txtFilterNPI"].ToString() : txtFilterNPI.Text.Trim();
        string affiliationStatus = ViewState["ddlAffiliationStatus"] != null ? ViewState["ddlAffiliationStatus"].ToString() : ddlAffiliationStatus.SelectedValue.ToString();
        string inaffiliationStatus = ViewState["rblGridActiveStatusID"] != null ? ViewState["rblGridActiveStatusID"].ToString() : rblGridActiveStatusID.SelectedValue.ToString();
        int pageSize = 10;
        if (Session["grdPageSize"] != null)
        {
            pageSize = Convert.ToInt32(Session["grdPageSize"]);
        }

        StringBuilder sortColWithDirection = new StringBuilder();
        sortColWithDirection.Append(CurrentSortField);
        if (!String.IsNullOrEmpty(CurrentSortOrder) && CurrentSortOrder.ToLower() == "desc")
            sortColWithDirection.Append(" " + CurrentSortOrder);

        int totalResultCount = 0;
        DataSet ds = null;
        isApplicationInWorkflow = Helper.HasRows(svc.WF_SelectWorkflowByRegId(this.WorkflowPage.RegistrationId));
        if (name == string.Empty && npi == string.Empty && inaffiliationStatus.Equals("0") && affiliationStatus == string.Empty)
        {
            ds = svc.SelectAllAffiliationsByRegId(this.WorkflowPage.RegistrationId, Int32.Parse(inaffiliationStatus), sortColWithDirection.ToString(), pageSize, grdGroupAffiliations.PageIndex * pageSize, true, out totalResultCount);
        }
        else if (name == string.Empty && npi == string.Empty && inaffiliationStatus.Equals("1") && affiliationStatus == string.Empty)
        {
            ds = svc.SelectAllAffiliationsByRegId(this.WorkflowPage.RegistrationId, Int32.Parse(inaffiliationStatus), sortColWithDirection.ToString(), pageSize, grdGroupAffiliations.PageIndex * pageSize, true, out totalResultCount);

        }
        else
        {
            ds = svc.SearchAffiliationsByRegId(this.WorkflowPage.RegistrationId, sortColWithDirection.ToString(), pageSize, grdGroupAffiliations.PageIndex, true,
                name, npi, affiliationStatus, out totalResultCount);
        }
        hdnRowCount.Value = totalResultCount.ToString();
        hdnRowCount2.Text = totalResultCount.ToString();

        //previous page index
        var previousPageIndex = grdGroupAffiliations.PageIndex;
        var previousCurrentPageIndex = grdGroupAffiliations.PageIndex;
        this.grdGroupAffiliations.DataSource = ds;
        this.grdGroupAffiliations.VirtualItemCount = totalResultCount;
        this.grdGroupAffiliations.PageSize = pageSize;

        this.grdGroupAffiliations.PageIndex = previousPageIndex;

        this.grdGroupAffiliations.DataBind();
    }

    private void ResetSelectedProvider()
    {
        if (this.grdGroupAffiliations.SelectedIndex > -1)
            this.grdGroupAffiliations.SelectedIndex = -1;
    }

    private DataSet GetGroupAffiliationStatuses()
    {
        DataSet ds = svc.SelectGroupAffiliationStatuses();
        return ds;
    }

    private void InitDataTable()
    {
        _dtGroupAffiliations = null;
    }

    private DataTable GetGroupAffiliations()
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "AFFILIATION");
        DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;
        return dt;
    }

    private DataView GetSortedData()
    {
        if (dtGroupAffiliations != null)
        {
            DataView sortedView = new DataView(dtGroupAffiliations);

            if (!string.IsNullOrEmpty(dirSortExpression))
            {
                sortedView.Sort = dirSortExpression + " " + (dirGroupAffiliations == SortDirection.Ascending ? "Asc" : "Desc");
            }

            return sortedView;
        }
        return null;
    }

    public SortDirection dirGroupAffiliations
    {
        get
        {
            if (ViewState["dirStateGroupAffiliations"] == null)
            {
                ViewState["dirStateGroupAffiliations"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirStateGroupAffiliations"];
        }
        set
        {
            ViewState["dirStateGroupAffiliations"] = value;
        }
    }

    public String dirSortExpression
    {
        get { return ViewState["SortExpression"] as String ?? ""; }
        set { ViewState["SortExpression"] = value; }
    }

    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        return false;
    }

    protected void btnFilterGrid_Click(object sender, EventArgs e)
    {
        // reset pageIndex to zero when they click the search button, so it starts at the first page (this is in case they were previously moving through the list and were on a page other than the first one)
        grdGroupAffiliations.PageIndex = 0;
		
        SetSearchFiltersInSession();
        ResetSelectedProvider();
        RefreshAffiliates();
    }
    private void SetSearchFiltersInSession()
    {
        ViewState["txtFilterName1"] = txtFilterName1.Text.Trim();
        ViewState["txtFilterNPI"] = txtFilterNPI.Text.Trim();
        ViewState["ddlAffiliationStatus"] = ddlAffiliationStatus.SelectedValue.ToString();
        ViewState["rblGridActiveStatusID"] = rblGridActiveStatusID.SelectedValue.ToString();
    }
    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        txtFilterName1.Text = string.Empty;
        txtFilterNPI.Text = string.Empty;
        this.ddlAffiliationStatus.SelectedIndex = 0;
        SetSearchFiltersInSession();
        ResetSelectedProvider();
        RefreshAffiliates();
    }

    private void UpdateAffiliations()
    {
        //Reload Grid
        ResetSelectedProvider();
        RefreshAffiliates();

        if (InvalidateAgreements != null)
        {
            //Registration.aspx will capture
            InvalidateAgreements(this, new EventArgs());
        }

        this.upGA_Main.Update();
        if (SaveDataEvent != null)
        {
            SaveDataEvent();
        }
        this.mpe.Hide();
    }

    private void KeepPopupOpen()
    {
        this.mpe.Show();
    }

    private void CancelPopup()
    {
        this.mpe.Hide();
    }

    private bool IsValidProviderDrilldownStatus(int affiliationStatusTypeID)
    {
        bool editStatus = false;
        if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
            return editStatus;

        switch (affiliationStatusTypeID)
        {
            case CON.GroupAffiliationStatusTypeID.NotSetYet:
                //case CON.GroupAffiliationStatusTypeID.MemberProfileNotFound:
                editStatus = false;
                break;
            default:
                editStatus = true;
                break;
        }
        return editStatus;
    }

    private bool IsDeleteStatus(int affiliationStatusTypeID, string medicaidID)
    {
        bool deleteStatus = true;
        if (!Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
            return false;

        if (!Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId) && !isApplicationInWorkflow)
            return false;


        switch (affiliationStatusTypeID)
        {
            case CON.GroupAffiliationStatusTypeID.NotSetYet:
            case CON.GroupAffiliationStatusTypeID.MemberNotFound:
            //case CON.GroupAffiliationStatusTypeID.IndividualRegistrationInProgress:
            case CON.GroupAffiliationStatusTypeID.IndividualEnrollmentPendingApproval:
            //case CON.GroupAffiliationStatusTypeID.IndividualRegistrationPendingApproval:
            //case CON.GroupAffiliationStatusTypeID.ConfirmGroupMember:
            //case CON.GroupAffiliationStatusTypeID.GroupConfirmed:
            //case CON.GroupAffiliationStatusTypeID.IndividualRequiresReValidation: OHPNM-11905
                deleteStatus = true; //string.IsNullOrEmpty(medicaidID) Remove Medicaid ID requirement;
                break;
            case CON.GroupAffiliationStatusTypeID.PendingApproval:
                deleteStatus = true;
                break;

            default:
                deleteStatus = false;
                break;
        }


        return deleteStatus;
    }

    private void DeleteRegAfiliation(int regAffiliationID)
    {
        svc.DeleteRegAffiliation(regAffiliationID);
        UpdateAffiliations();
    }
    public override string ValidationGroup
    {
        get { return "valAffiliation"; }
    }

    public override string Title
    {
        get { return "Affiliation Information"; }
    }

    public override string IdText
    {
        get { return "ucAffiliations_" + this.WorkflowPage.RegistrationId; }
    }
    private bool isGroupAllowedToUpdateIndividual(int affiliationStatusTypeID)
    {
        bool retVal = false;
        if (CON.GroupAffiliationStatusTypeID.Active == affiliationStatusTypeID)
        {
            //If active
            retVal = isIndividualEditable;

        }

        return retVal;
    }

    protected void lnkPDF_Click(object sender, EventArgs e)
    {
        int totalResultCount = 0;
        DataSet ds = null;
        ds = svc.SelectAllAffiliationsByRegId(this.WorkflowPage.RegistrationId, 0, string.Empty, 1000000, 0, true, out totalResultCount);
        RadGridExportAffiliations.DataSource = ds;
        RadGridExportAffiliations.DataBind();
        RadGridExportAffiliations.ExportSettings.Pdf.ForceTextWrap = true;
        RadGridExportAffiliations.ExportSettings.Pdf.PageLeftMargin = Unit.Pixel(15);
        RadGridExportAffiliations.ExportSettings.Pdf.PageRightMargin = Unit.Pixel(15);

        RadGridExportAffiliations.MasterTableView.ExportToPdf();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        int totalResultCount = 0;
        DataSet ds = null;
        ds = svc.SelectAllAffiliationsByRegId(this.WorkflowPage.RegistrationId, 0, string.Empty, 1000000, 0, true, out totalResultCount);
        RadGridExportAffiliations.DataSource = ds;
        RadGridExportAffiliations.DataBind();
        RadGridExportAffiliations.MasterTableView.ExportToExcel();
    }

    protected void lnkPDF1_Click(object sender, EventArgs e)
    {
        //int totalResultCount = 0;
        DataSet ds = null;
        ds = svc.GetAffiliateSpecialtyExportData(this.WorkflowPage.RegistrationId);
        RadGridExportSecSpec.DataSource = ds;
        RadGridExportSecSpec.DataBind();
        RadGridExportSecSpec.ExportSettings.Pdf.ForceTextWrap = true;
        RadGridExportSecSpec.ExportSettings.Pdf.PageLeftMargin = Unit.Pixel(15);
        RadGridExportSecSpec.ExportSettings.Pdf.PageRightMargin = Unit.Pixel(15);

        RadGridExportSecSpec.MasterTableView.ExportToPdf();
    }

    protected void lnkExcel1_Click(object sender, EventArgs e)
    {
       // int totalResultCount = 0;
        DataSet ds = null;
        ds = svc.GetAffiliateSpecialtyExportData(this.WorkflowPage.RegistrationId);
        RadGridExportSecSpec.DataSource = ds;
        RadGridExportSecSpec.DataBind();
        RadGridExportSecSpec.MasterTableView.ExportToExcel();
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "GroupAffiliations";
        this.Page.Validators.Add(val);
        isGood = false;
    }
}
