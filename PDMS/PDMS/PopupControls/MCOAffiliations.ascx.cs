using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Pages_MCOAffiliations : BaseSectionControl
{
    public delegate void SaveDataEventHandler();
    public event SaveDataEventHandler SaveDataEvent;
    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public string _SortField
    {
        get
        {
            return (string)ViewState["SortField"] ?? "DateOfAction"; // default sort 
        }
        set
        {
            ViewState["SortField"] = value;
        }
    }

    private bool _ExportHistory
    {
        get
        {
            return Convert.ToBoolean(ViewState["ExportHistory"]);
        }
        set
        {
            ViewState["ExportHistory"] = value;
        }
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
    #region grdMCOAffiliations
    DataTable _dtMCOAffiliations;

    public DataTable dtMCOAffiliations
    {
        get
        {
            /*if (_dtMCOAffiliations == null)
            {
                _dtMCOAffiliations = this.GetMCPAffiliations();
            }*/

            return _dtMCOAffiliations;
        }
    }

    
    #endregion
    #region Section
    private enum PopupName { MCOAffiliations = 0, MCOAffiliationsHistory = 1 };
    #endregion

    new public System.EventHandler InvalidateAgreements;

    private void SetButtons()
    {

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (pageTypeID == CON.RegistrationPageType.MCOAffiliations)
        {
            SetButtons();

            //this.ucMCOAffiliationsPopUp.SaveEvent += new PopupControls_MCOAffiliationsPopUp.SaveEventHandler(UpdateAffiliations);
            this.ucMCOAffiliationsPopUp.ValidationEvent += new PopupControls_MCOAffiliationsPopUp.ValidationEventHandler(KeepPopupOpen);
            this.ucMCOAffiliationsPopUp.ErrorEvent += new PopupControls_MCOAffiliationsPopUp.ErrorEventHandler(KeepPopupOpen);
            this.ucMCOAffiliationsPopUp.KeepOpenEvent += new PopupControls_MCOAffiliationsPopUp.KeepOpenEventHandler(KeepPopupOpen);
            this.ucMCOAffiliationsPopUp.CancelEvent += new PopupControls_MCOAffiliationsPopUp.CancelEventHandler(CancelPopup);
        }
        if (!IsPostBack)
        {

            LoadMCPAffiliations();
            _dtMCOAffiliations = null;
        }    
}
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "MCP Affiliation";

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = new DataSet();
        ds = psc.SelectRegistrationDataWithParams("usp_SelectMCP_AFFILIATION_HISTORY", parms);

        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.PageIndex = 0;
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
        }
    }
    public override void LoadControlData()
    {
       // LoadMCPAffiliations();
        LoadMcoAffiliationCarePlanDetails();
        RefreshAffiliates();

        
        //btnHistory.Visible = grdMCOAffiliations.Rows.Count > 0;
        btnAdd.Visible = !Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName);
    }

    public override void LoadData(DataRow dr = null)
    {
        if (rblIsContracting.SelectedValue != "1")
        {
            divplans.Visible = false;
        }
        if (dr != null)
        {
            if (!string.IsNullOrEmpty(Helper.GetString("CAREPLAN_IDS", dr)))
            {
                string[] CAREPLAN_IDS = Helper.GetString("CAREPLAN_IDS", dr).Split(',');
                foreach (var CAREPLAN_ID in CAREPLAN_IDS)
                {
                    var chkItem = chkPossibleParticipation.Items.FindByValue(CAREPLAN_ID.ToString().Trim());
                    if (chkItem != null)
                    {
                        chkItem.Selected = true;
                    }

                }
            }

            if (!string.IsNullOrEmpty(Helper.GetString("IS_CAREPLAN_INTRESTED", dr)))
            {
                rblIsContracting.SelectedValue = Helper.GetBool("IS_CAREPLAN_INTRESTED", dr) ? "1" : "0";

                divplans.Visible = (rblIsContracting.SelectedValue == "1") ? true : false;

               
            }
            
        }

        if (!Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            rblIsContracting.Enabled = false;
            chkPossibleParticipation.Enabled = false;
        }
        else
        {
            rblIsContracting.Enabled = true;
            chkPossibleParticipation.Enabled = true;
        }
        //OHPNM-8503 enabling the radio buttons only during enrollment.

        if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.NewReg)
        {
            rblIsContracting.Enabled = true;
        }
        else
        {
            rblIsContracting.Enabled = false;
        }
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
    }

    public void LoadData()
    {
        
        RefreshAffiliates();

        //btnHistory.Visible = grdMCOAffiliations.Rows.Count > 0;
        btnAdd.Visible = !Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName);
    }

    private void LoadMCPAffiliations()
    {
        //grdMCOAffiliations.DataSource = GetSortedData();
        //grdMCOAffiliations.DataBind();
    }

    private void LoadAffiliationStatusDefs()
    {
        //this.rptStatusDef.DataSource = GetMCOAffiliationstatuses();
        //this.rptStatusDef.DataBind();
    }

    private void ClearPlansParticipation()
    {
        foreach( ListItem li in chkPossibleParticipation.Items)
        {
            if (li.Selected)
                li.Selected = false;

        }
    }
    public override bool SaveData()
    {
        if (rblIsContracting.SelectedValue == "1" && string.IsNullOrEmpty(chkPossibleParticipation.SelectedValue))
        {
            lblerrormsg.Text = "No selection is made.";
            return false;
        }
        var chkCarePlanItems = string.Empty;
        if(!string.IsNullOrEmpty(chkPossibleParticipation.SelectedValue))
        {

            foreach (ListItem item in chkPossibleParticipation.Items)
            {
                if(item.Selected)
                {
                    chkCarePlanItems = chkCarePlanItems + item.Value + ",";
                }

            }
            chkCarePlanItems = chkCarePlanItems.Remove(chkCarePlanItems.LastIndexOf(","));



        }
        DataSet dsPaper = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "MCP_AFFILIATION_CAREPLAN");
        if (!string.IsNullOrEmpty(rblIsContracting.SelectedValue) || !string.IsNullOrEmpty(chkPossibleParticipation.SelectedValue))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();            
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("IS_CAREPLAN_INTRESTED", rblIsContracting.SelectedValue);
            parms.Add("CAREPLAN_ID", chkCarePlanItems);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            if (Helper.HasRows(dsPaper))
            {
                DataRow drpaper = dsPaper.Tables[0].Rows[0];
                string REG_MCP_AFFILIATION_CAREPLAN_id = Helper.GetString("REG_MCP_AFFILIATION_CAREPLAN_ID", drpaper);
                parms.Add("REG_MCP_AFFILIATION_CAREPLAN_ID", REG_MCP_AFFILIATION_CAREPLAN_id);
                svc.UpdateRegistrationDataTable("MCP_AFFILIATION_CAREPLANCustom", parms);
            }
            else
            {
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                svc.InsertRegistrationDataTable("MCP_AFFILIATION_CAREPLANCustom", parms);
            }
        }
        return !Registration.PreviewingRegistrationSection();
    }
    
    private void LoadMcoAffiliationCarePlanDetails()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "MCP_AFFILIATION_CAREPLAN");
        DataTable dtPrimaryPracticeLocation = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;                
        if (Helper.HasRows(dtPrimaryPracticeLocation))
        {
            this.LoadData(dtPrimaryPracticeLocation.Rows[0]);
            
        }
        else
        {
            this.LoadData(null);
        }
    }

    public override bool ValidateData()
    {
        bool isGood = true;

        //if (Registration.PreviewingRegistrationSection())
        //{
        //    AddValidationErrorMessage("*Previous registration sections must be completed first.");
        //    isGood = false;
        //}
        //else
        //{
        //    if (!GridDataValid()) isGood = false;

        //    if (grdMCOAffiliations.Rows.Count == 0 && Registration.EntryIsRequired(this.WorkflowPage.RegistrationId,CON.RegistrationPageName.MCOAffiliations, Registration.GetSectionNameFromStepNumber(CON.SectionTypeID.Affiliations)))
        //    {
        //        AddValidationErrorMessage(string.Format("*At least one entry in {0} is required.", CON.RegistrationPageName.MCOAffiliations));
        //        isGood = false;
        //    }
        //}

        return isGood;
    }

    public override bool HasInputValue()
    {
        //bool rtn = false;
        //if (grdMCOAffiliations != null)
        //{
        //    if (grdMCOAffiliations.Rows.Count > 0)
        //    {
        //        rtn = true;
        //    }
        //}
        //rtn = true;
        return true;
    }

    protected void rblIsContracting_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblIsContracting.SelectedValue =="1")
        {
            divplans.Visible = true;
        }
        else if (rblIsContracting.SelectedValue == "0")
        {
            divplans.Visible = false;
            ClearPlansParticipation();
        }
        upGA_Main.Update();
    }

    private bool GridDataValid()
    {
        bool isValid = true;
 
        
        DateTime testdate;


        string name;

        string startDate;
        string endDate;

        if (Helper.HasRows(dtMCOAffiliations))
            foreach (DataRow row in dtMCOAffiliations.Rows)
            {
                name = Helper.GetData("NAME", row);
                
                startDate = Helper.GetData("START_DATE", row);
                endDate = Helper.GetData("END_DATE", row);

                if (string.IsNullOrEmpty(name))
                    isValid = AddValidationErrorMessage("* Name is required");



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
                        if (DateTime.TryParse(startDate, out d1) && DateTime.TryParse(endDate, out d2))
                            if (d2 < d1)
                                isValid = AddValidationErrorMessage("* Date span is invalid");
                    }
            }


        return isValid;
    }
   

    protected void btnAdd_Click(object sender, CommandEventArgs e)
    {
        
        switch (e.CommandName)
        {
            case "MCOAffiliations":
                lblTitle.Text = "Add MCO Affiliation";
                ucMCOAffiliationsPopUp.LoadData(0,"");
                mltPopup.SetActiveView(vwGroupAffiliations);
                mpe.Show();
                break;
            default:
                break;
        }
    }

    protected string MaskTaxID(string taxID)
    {
        string hiddenString = taxID.Length > 4 ? taxID.Substring(taxID.Length - 4).PadLeft(taxID.Length, '*') : taxID;

        return hiddenString;
    }

    private void RefreshAffiliates()
    {
        //_dtMCOAffiliations = null;
        LoadMCPAffiliations();
    }

    private void ResetSelectedProvider()
    {
        /*if (this.grdMCOAffiliations.SelectedIndex > -1)
            this.grdMCOAffiliations.SelectedIndex = -1;*/
    }

    private DataSet GetMCOAffiliationstatuses()
    {
        DataSet ds = svc.SelectMCOAffiliationStatuses(); 
        return ds;
    }

    private void InitDataTable()
    {
        _dtMCOAffiliations = null;
    }

    /*private DataTable GetMCPAffiliations()
    {
        int pageSize = grdMCOAffiliations.PageSize;
        int totalResultCount = 0;
        string sortColWithDirection = this.grdMCOAffiliations.GridViewSortDirection == SortDirection.Descending ? grdMCOAffiliations.GridViewSortColumn + " DESC" : grdMCOAffiliations.GridViewSortColumn;
        DataSet ds;//= svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "MCO_AFFILIATION")
        ds = svc.SelectMCPAffiliationsByRegId(this.WorkflowPage.RegistrationId, sortColWithDirection, pageSize, grdMCOAffiliations.CurrentRowIndex, true,
         out totalResultCount);
        DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;
        grdMCOAffiliations.VirtualItemCount = totalResultCount;
        return dt;
    }*/

    private DataView GetSortedData()
    {
        if (dtMCOAffiliations != null)
        {
            DataView sortedView = new DataView(dtMCOAffiliations);

            if (!string.IsNullOrEmpty(dirSortExpression))
            {
                sortedView.Sort = dirSortExpression + " " + (dirMCOAffiliations == SortDirection.Ascending ? "Asc" : "Desc");
            }

            return sortedView;
        }
        return null;
    }

    public SortDirection dirMCOAffiliations
    {
        get
        {
            if (ViewState["dirStateMCOAffiliations"] == null)
            {
                ViewState["dirStateMCOAffiliations"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirStateMCOAffiliations"];
        }
        set
        {
            ViewState["dirStateMCOAffiliations"] = value;
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
        ResetSelectedProvider();
        RefreshAffiliates();
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {


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
            case CON.MCOAffiliationstatusTypeID.NotSetYet:
            //case CON.MCOAffiliationstatusTypeID.MemberProfileNotFound:
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
        bool deleteStatus = false;
        if (!Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
            return false;

        if (!Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId))
            return false;

        switch (affiliationStatusTypeID)
        {
            case CON.MCOAffiliationstatusTypeID.NotSetYet:
            //case CON.MCOAffiliationstatusTypeID.MemberProfileNotFound:
            //case CON.MCOAffiliationstatusTypeID.IndividualRegistrationInProgress:
            case CON.MCOAffiliationstatusTypeID.ProviderEnrollmentPendingApproval:
            case CON.MCOAffiliationstatusTypeID.ProviderRequiresReEnrollment:
            //case CON.MCOAffiliationstatusTypeID.IndividualRegistrationPendingApproval:

            case CON.MCOAffiliationstatusTypeID.Confirmed:
                deleteStatus = true; //string.IsNullOrEmpty(medicaidID) Remove Medicaid ID requirement;
                break;
            default:
                deleteStatus = false;
                break;
        }


        return deleteStatus;
    }

    private void DeleteRegMCOAfiliation(int regMCOAffiliationID)
    {
        svc.DeleteRegMCOAffiliation(regMCOAffiliationID);
        UpdateAffiliations();
    }

    public override string ValidationGroup
    {
        get { return "valMCOAffiliation"; }
    }

    public override string Title
    {
        get { return "MCO Affiliation"; }
    }

    public override string IdText
    {
        get { return "ucMCOAffiliation_" + this.WorkflowPage.RegistrationId; }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.CancelHistoryPopup();
    }

    private void CancelHistoryPopup()
    {
        this.mpeHistory.Hide();
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        mpeHistory.Show();
    }

    protected void lnkHistoryExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectMCP_AFFILIATION_HISTORY", parms);
            if (Helper.HasRows(ds))
            {
                grdHistoryExport.DataSource = ds.Tables[0];
                grdHistoryExport.DataBind();
                grdHistoryExport.MasterTableView.ExportToExcel();
            }
        }
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (_SortField.Equals(e.SortExpression))
        {
            _SortField = _SortField + " DESC";
        }
        else
        {
            _SortField = e.SortExpression;
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectMCP_AFFILIATION_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.PageIndex = 0;
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
            mpeHistory.Show();
        }
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectMCP_AFFILIATION_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.DataSource = ds.Tables[0];
            grd.PageIndex = e.NewPageIndex;
            grd.DataBind();
            mpeHistory.Show();
        }
    }
}
