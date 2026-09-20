using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Globalization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_ProviderHomeNew : RegistrationProvider, IProviderManagerView
{

    #region Properties
    private ProviderManagerPresenter _presenter;

    public ProviderManagerPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new ProviderManagerPresenter(this);
            }

            return _presenter;
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

    public ProviderManagerData Model { get; set; }


    public int TaxIDTypeID
    {
        get
        {
            return ViewState["TaxIDTypeID"] == null ? 0 : Convert.ToInt32(ViewState["TaxIDTypeID"]);
        }
        set
        {
            ViewState["TaxIDTypeID"] = value;
        }
    }

    public string TaxID
    {
        get
        {
            return ViewState["TaxID"] == null ? "" : ViewState["TaxID"].ToString();
        }
        set
        {
            ViewState["TaxID"] = value;
        }
    }


    private Guid UserID
    {
        get
        {
            return Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        }
    }
    public DataTable StatusTypes
    {
        get
        {
            return ViewState["StatusTypes"] == null ? null : (DataTable)(ViewState["StatusTypes"]);
        }
        set
        {
            ViewState["StatusTypes"] = value;
        }
    }

    public DataTable ProviderTypes
    {
        get
        {
            return ViewState["ProviderTypes"] == null ? null : (DataTable)(ViewState["ProviderTypes"]);
        }
        set
        {
            ViewState["ProviderTypes"] = value;
        }
    }

    public DataTable SpecialtyTypeNames
    {
        get
        {
            return ViewState["SpecialtyTypeNames"] == null ? null : (DataTable)(ViewState["SpecialtyTypeNames"]);
        }
        set
        {
            ViewState["SpecialtyTypeNames"] = value;
        }
    }

    public DataSet ConvertedProviders
    {
        get
        {
            return ViewState["ConvertedProviders"] == null ? null : (DataSet)(ViewState["ConvertedProviders"]);
        }
        set
        {
            ViewState["ConvertedProviders"] = value;
        }
    }

    public int ApplicationTypeID
    {
        get
        {
            return ViewState["ApplicationTypeID"] == null ? 0 : Convert.ToInt32(ViewState["ApplicationTypeID"]);
        }
        set
        {
            ViewState["ApplicationTypeID"] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
            Page.Theme = "Modernization";
        else
            Page.Theme = "Default";

    }

    protected void Page_Init(object sender, EventArgs e)
    {
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //InitView();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //OHPNM-3487 
        Session["ViewProviderFile"] = false;
        if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InternalApplicationsEntry))
        {
            btnInternalApplication.Visible = true;
        }

        DisplayElapsedDays();
      
        if (!Page.IsPostBack)
        {
            InitView();
            GetAlertsByTaxID();
        }       

        //OHPNM-9803
        bool IsPreviousPageClicked;        
        IsPreviousPageClicked = Session["IsPreviousPageClicked"] == null ? false : true;
        if (IsPreviousPageClicked && Session["hdnPreviousPageClick"]!=null)
        {
            gvMyProviders.CurrentPageIndex = Convert.ToInt32(Session["hdnPreviousPageClick"]);
            InitView();
            // clearing sessions once previous page is clicked.
            Session["IsPreviousPageClicked"] = null;
        }

        ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkBtnExcel);
        ScriptManager.GetCurrent(Page).RegisterPostBackControl(lnkBtnPDF);
    }
    #endregion

    #region Presenter Events
    private void SetButtonDisplay(bool showSwitchAdminButton)
    {
        bool isPowerAgentForSelctedProvAdmin = Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, 0);
        bool HasPowerAgentAccessMgmtRole = Helper.HasPowerAgentAccessMgmtRole(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, 0);


        btnMyProviders.Visible = true;
        if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator)
            || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.DeemedPresumptive) 
            || Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), string.Empty, 0))
        {
            btnPendingAgents.Visible = true;
            btnSelectProviders.Visible = true;
            btnAgentBulkUpload.Visible = Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator)
                                           || (isPowerAgentForSelctedProvAdmin && HasPowerAgentAccessMgmtRole && !string.IsNullOrEmpty(SessionVarRetriever.SelectedProviderAdminUserID));
            btnSwitchAdmin.Visible = spnSelectedProvAdmin.Visible = showSwitchAdminButton && !string.IsNullOrEmpty(SessionVarRetriever.SelectedProviderAdminUserID);


            btnAccountAdmin.Visible = Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator)
                                           || (isPowerAgentForSelctedProvAdmin && HasPowerAgentAccessMgmtRole && !string.IsNullOrEmpty(SessionVarRetriever.SelectedProviderAdminUserID))
                                           || (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent) && Helper.IsUserInSubRoles(0, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.CostReportManagementAgent));
            
            DataSet ds = svc.GetUserAccountInformation(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            btnDDAcountAdmin.Visible = ds.Tables[0].Rows[0]["USER_TYPE"].ToString() == "DODD Admin" ? true : false;

            btnPowerAgentMgmt.Visible = Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator)
                                           || isPowerAgentForSelctedProvAdmin;
        }
        else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent)
               && Helper.IsUserInSubRoles(0, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.CostReportManagementAgent)) // SAM573 Show AccountAdminitration button for CostReportManagementAgent
        {
            btnAccountAdmin.Visible = true;
            btnSwitchAdmin.Visible = spnSelectedProvAdmin.Visible = false;
            btnPowerAgentMgmt.Visible = false;
        }
        else
        {
            btnPendingAgents.Visible = false;
            btnSelectProviders.Visible = false;
            btnAccountAdmin.Visible = false;
            btnDDAcountAdmin.Visible = false;
            btnSwitchAdmin.Visible = spnSelectedProvAdmin.Visible = false;
            btnPowerAgentMgmt.Visible = false;
        }
        gvMyProviders.HeaderContextMenu.ItemCreated += new RadMenuEventHandler(HeaderContextMenu_ItemCreated);

        btnPendingAgents.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnPendingAgents, null) + ";");
        btnSelectProviders.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSelectProviders, null) + ";");
        btnAccountAdmin.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnAccountAdmin, null) + ";");
        btnDDAcountAdmin.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnDDAcountAdmin, null) + ";");
        
        btnAffiliateUpdate.Visible = ((Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) || isPowerAgentForSelctedProvAdmin)
                                          && Helper.IsApprovedDelegate(SessionVarRetriever.SelectedProviderAdminUserID)); // SAM768 For power agents show only if the selected provider admin is a approved delegate.

        btnNewStreamLinedProvider.Visible = ((Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) || isPowerAgentForSelctedProvAdmin)
                                          && Helper.IsApprovedDelegate(SessionVarRetriever.SelectedProviderAdminUserID)) || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InternalApplicationsEntry);


    }
    public void InitView()
    {        
        if (Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(),string.Empty,0)) //logged in user is a Power Agent
        {
            DataSet ds = svc.GetProviderAdminsForPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            if (Helper.HasRows(ds) && ds.Tables[0].Rows.Count > 1)
            {
                if (string.IsNullOrEmpty(SessionVarRetriever.SelectedProviderAdminUserID))
                {
                    BindProviderAdminRadDropDown(ds);
                    mpeProviderAdminSelect.Show();
                }
                else
                {
                    mpeProviderAdminSelect.Hide();
                    divAdminProviders.Visible = true;
                    divErrPowerAgent.Visible = false;
                    GetAssignedProviders(SessionVarRetriever.SelectedProviderAdminUserID);
                    spnProvAdminTxt.InnerText = "Now displaying for Administrator " + SessionVarRetriever.SelectedProviderAdminUserName;
                    SetButtonDisplay(true);
                }
                
            }
            else
            {
                SessionVarRetriever.SelectedProviderAdminUserID = ds.Tables[0].Rows[0]["PROVIDER_ADMIN_USER_ID"].ToString();
                mpeProviderAdminSelect.Hide();
                divAdminProviders.Visible = true;
                divErrPowerAgent.Visible = false;
                GetAssignedProviders(SessionVarRetriever.SelectedProviderAdminUserID);
                SetButtonDisplay(false);
            }
        }       
        else
        {
            divAdminProviders.Visible = true;
            divErrPowerAgent.Visible = false;
            GetAssignedProviders(UserID.ToString());
            SessionVarRetriever.SelectedProviderAdminUserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
            SetButtonDisplay(false);
        }

    }

    private void BindProviderAdminRadDropDown(DataSet ds)
    {
        RadProvAdminddl.Sort = RadComboBoxSort.None;
        RadProvAdminddl.Items.Clear();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            RadProvAdminddl.Items.Insert(0, new RadComboBoxItem(dr["PROVIDER_ADMIN_NAME"].ToString(), dr["PROVIDER_ADMIN_USER_ID"].ToString()));
        }
    }
    public void GetAssignedProviders(string userID)
    {
        gvMyProviders.DataSource = new Object[0];

        DataSet ds = svc.GetUserAccountInformation(userID);
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            string taxID = Helper.GetString("TAX_ID", dr);
            this.TaxIDTypeID = Helper.GetInt("TAX_ID_TYPE_ID", dr);

            this.TaxID = taxID;
            lblTaxID.Text = this.TaxID;
            presenter.Init(new Guid(userID), TaxID, TaxIDTypeID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        }
    }

    private void GetAlertsByTaxID()
    {
        string taxID = this.TaxID;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetAlertsByTaxID(taxID);
        string navigateTo = "~/Process/ProviderDetailsNew.aspx?regID=";
        string regID = string.Empty;

        foreach (DataTable dt in ds.Tables)
        {
            if (Helper.HasRows(dt))
            {
                string providerName = dt.Rows[0]["name"].ToString();
                regID = dt.Rows[0]["reg_id"].ToString();
                string address = dt.Rows[0]["contact_address1"].ToString();
                string endDate = Convert.ToDateTime(dt.Rows[0]["end_date"]).ToString("MM/dd/yyyy");

                if (dt.TableName == "AddlInfo")
                {
                    spnAddlInfo.InnerHtml = "<strong>Immediate Action!</strong> The " + providerName + " at " + address + " requires additional information by " + endDate + ".";
                    hlnkAddlInfo.NavigateUrl = navigateTo + regID;
                    pnlAlertAddlInfo.Visible = true;
                }
                else if (dt.TableName == "ReEnrollment")
                {
                    spnEnrollment.InnerHtml = "<strong>Immediate Action!</strong> The " + providerName + " at " + address + " is due for enrollment by " + endDate + ".";
                    hlnkRenewal.NavigateUrl = navigateTo + regID;
                    pnlAlertEnrollmentAlert.Visible = true;
                }
                else
                {
                    spnAlertWarning.InnerHtml = "<strong>Heads up!</strong> The " + providerName + " at " + address + " requires license renewal by " + endDate + ".";
                    hlnkAlertWarning.NavigateUrl = navigateTo + regID;
                    pnlAlertWarning.Visible = true;
                }

            }

        }

    }


    public void SetUserInformation(DataSet ds)
    {
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            string taxID = Helper.GetString("TAX_ID", dr);
            this.TaxID = taxID;
            this.TaxIDTypeID = Helper.GetInt("TAX_ID_TYPE_ID", dr);


        }
    }

    public void SetApplicationTypes(DataSet ds)
    {

    }

    public void SetMyGroupMemberProfiles(DataSet ds, int totalRowCount)
    {
    }

    public void SetMyPendingReferrals(DataSet ds, int totalRowCount)
    {
    }

    public void SetPendingProviders(DataSet ds, int totalRowCount)
    {

    }

    public void SetErrorMessages()
    {
        if (presenter.hasErrors)
        {
            View_ErrorEvent(presenter.ErrorList);
        }
    }
    public void SetMyProviders(DataSet ds, int totalRowCount)
    {
    }

    public void SetAllProviders(DataSet ds, int totalRowCount)
    {
        this.gvMyProviders.DataSource = ds;
        this.gvMyProviders.VirtualItemCount = totalRowCount;
        // this.gvMyProviders.DataBind();
        if (Helper.HasRows(ds))
        {
            StatusTypes = ds.Tables[0].DefaultView.ToTable(true, new String[] { "RegistrationStatusType" });
            ProviderTypes = ds.Tables[0].DefaultView.ToTable(true, new String[] { "ProviderTypeName" });
            SpecialtyTypeNames = ds.Tables[0].DefaultView.ToTable(true, new String[] { "SpecialtyTypeName" });

            if (ds.Tables[0].Rows.Count >= 1 && Helper.IsMITSuser(UserID.ToString()))
            {
                btnNewProvider.Visible = AppSettings.Get("AllowMITSUserAddMultipleProviders").ToString() == "true" ? true : false;
            }
        }

        if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent) && !Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, 0))
            btnNewProvider.Visible = false;

    }


    #endregion

    #region Private Methods



    private void HeaderContextMenu_ItemCreated(object sender, RadMenuEventArgs e)
    {
        if (e.Item.Text.ToLower() == "group by")
        {
            e.Item.Style.Add("display", "none");
            (sender as RadContextMenu).Items[e.Item.Index - 1].Remove();
        }
        if (e.Item.Text.ToLower() == "ungroup")
        {
            e.Item.Style.Add("display", "none");
            (sender as RadContextMenu).Items[e.Item.Index - 1].Remove();
        }

    }
    private void View_ErrorEvent(Dictionary<string, string> lstErrors)
    {
        ClearErrorMessages();
        SetValidationErrors(lstErrors);
    }

    private void SetValidationErrors(Dictionary<string, string> lstErrors)
    {
        foreach (var pair in lstErrors)
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
        lblErrorMessages.Text = string.Empty;
    }


    #endregion


    private void InitModel()
    {
        if (Model == null)
        {
            presenter.Init();
            Model.UserID = this.UserID;
            Model.TaxID = this.TaxID;
            Model.TaxIDTypeID = this.TaxIDTypeID;
        }
    }

    protected void gvMyProviders_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        gvMyProviders.CurrentPageIndex = e.NewPageIndex;
        Session["hdnPreviousPageClick"] = gvMyProviders.CurrentPageIndex.ToString();
        InitView();
    }


    protected void gvMyProviders_Sorting(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        InitView();
    }

    protected void gvMyProviders_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator)
            || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.DeemedPresumptive) 
            || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent))
        {
            if(!string.IsNullOrEmpty(SessionVarRetriever.SelectedProviderAdminUserID))
                InitView();
        }
    }


    protected void RadComboBoxStatus_Init(object sender, EventArgs e)
    {
        RadComboBox combo = sender as RadComboBox;
        if (StatusTypes != null)
        {
            DataRow row = StatusTypes.NewRow();
            row[0] = "All";
            StatusTypes.Rows.InsertAt(row, 0);
            combo.DataSource = StatusTypes;
            combo.DataBind();
        }
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>clearGridTextBoxes();</script>", false);

    }

    protected void RadComboBoxProviderType_Init(object sender, EventArgs e)
    {
        RadComboBox combo = sender as RadComboBox;
        if (ProviderTypes != null)
        {
            DataRow row = ProviderTypes.NewRow();
            row[0] = "All";
            ProviderTypes.Rows.InsertAt(row, 0);
            combo.DataSource = ProviderTypes;
            combo.DataBind();
        }
    }

    protected void RadComboBoxSpecialtyType_Init(object sender, EventArgs e)
    {
        RadComboBox combo = sender as RadComboBox;
        if (SpecialtyTypeNames != null)
        {
            DataRow row = SpecialtyTypeNames.NewRow();
            row[0] = "All";
            SpecialtyTypeNames.Rows.InsertAt(row, 0);
            combo.DataSource = SpecialtyTypeNames;
            combo.DataBind();
        }
    }

    protected void gvMyProviders_DataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
    }
    protected void gvMyProviders_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridDataItem item = e.Item as GridDataItem;
        if (item != null)
        {
            (item).Selected = true;

            int regID = (int)(item).GetDataKeyValue("RegID");
            //string userID = (string)(item).GetDataKeyValue("UserID");

            if (e.CommandName.Equals("ManageProvider") || e.CommandName.Equals("ManageRegID"))
            {
                string logMsg = String.Format("ManageProvider or ManageRegID - " + regID.ToString() + " by " + HttpContext.Current.User.Identity.Name + ", Role(s) - " + String.Join(", ", Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name)));
                Logging log = new Logging(Guid.NewGuid(), logMsg);
                log.CreateLogEntry(string.Format(logMsg, Logging.LogPriority.Information));
                Response.Redirect("~/Process/ProviderDetailsNew.aspx?regID=" + regID);
            }
        }
    }
    protected void btnSwitchAdmin_Click(object sender, EventArgs e)
    {
        DataSet ds = svc.GetProviderAdminsForPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        if (Helper.HasRows(ds) && ds.Tables[0].Rows.Count > 1)
        {
            BindProviderAdminRadDropDown(ds);
            mpeProviderAdminSelect.Show();
        }
    }
    protected void btnProvAdminOK_Click(object sender, EventArgs e)
    {
        if (RadProvAdminddl.SelectedIndex == -1)
        {
            lblErrMsg.Visible = true;
        }
        else
        {
            lblErrMsg.Visible = false;
            SessionVarRetriever.SelectedProviderAdminUserID = RadProvAdminddl.SelectedValue.ToString();
            SessionVarRetriever.SelectedProviderAdminUserName = RadProvAdminddl.SelectedItem.Text;
            mpeProviderAdminSelect.Hide();
            divAdminProviders.Visible = true;
            InitView();
            gvMyProviders.Rebind();
        }
    }
    protected void btnProvAdminCancel_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionVarRetriever.SelectedProviderAdminUserID))
        {
            divAdminProviders.Visible = false;
            divErrPowerAgent.Visible = true;
            lblErrMsg.Visible = false;
            SessionVarRetriever.SelectedProviderAdminUserID = string.Empty;
        }
        mpeProviderAdminSelect.Hide();
    }
    
    protected void btnAccountAdmin_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Process/ProviderAdminAgentMaintenance.aspx");
    }

    protected void btnAgentBulkUpload_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Process/AgentBulkUpload.aspx");
    }

    protected void btnPowerAgentMgmt_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Process/PowerAgentAccessMgmt.aspx");
    }
    protected void btnAffiliateUpdate_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Process/AffiliateUpdate.aspx");
    }

    protected void btnDDAcountAdmin_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Process/CEOUserAgentAccountsAdministration.aspx");
    }
    protected void gvMyProviders_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (gvMyProviders.SelectedIndexes.Count > -1)
        {

            int regID = (int)(gvMyProviders.SelectedItems[0] as GridDataItem).GetDataKeyValue("RegID");
            //  TODO: EDV Pass on REG ID to ProviderDetails.

            Response.Redirect("~/Process/ProviderDetailsNew.aspx?regID=" + regID);
        }
    }

    private void DisplayElapsedDays()
    {
        //By Default show standard application elapsed time in provider home page as per discussion.
        int ApplicationTypeId = CON.ApplicationType.Standard;
        try
        {
            if (ApplicationTypeId > 0)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                int ElapsedTimeHours = psc.GetElapsedTimeLimitByApplicationType(ApplicationTypeId);
                int DaysToCompleteRegistration = Helper.ConvertHoursToDays(ElapsedTimeHours);
                lblElapsedDays.Text = DaysToCompleteRegistration.ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }

    protected void btnInternalApplication_Click(object sender, EventArgs e)
    {

        int ApplicationTypeID = CON.ApplicationType.Internal;
        Session["ApplicationTypeID"] = ApplicationTypeID;
        //  ApplicationTypeID = 6;
        Response.Redirect("~/Process/NewProvider.aspx?ApplicationType");
    }

    protected void btnPendingAgents_Click(object sender, EventArgs e)
    {
        pnlPendingAgents.Visible = true;
        pnlSelectProviders.Visible = false;
        DataSet ds = svc.SelectPendingAgentsByProviderAdmin(UserID.ToString());
        gvPendingAgents.DataSource = ds;
        gvPendingAgents.DataBind();
        btnPendingAgents.Enabled = false;
    }

    protected void btnSelectProvidersVoid_Click(object sender, EventArgs e)
    {
        //does nothing; switch it back to btnSelectProviders_Click once we have a finalized soultion from ODM
    }

    protected void btnSelectProviders_Click(object sender, EventArgs e)
    {
        string[] userRoles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);
        DataSet ds = svc.GetUserAccountInformation(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        pnlPendingAgents.Visible = false;
        if (userRoles.Contains("ProviderAgent") || userRoles.Contains("ProviderAdministrator"))
        {
            if (ds.Tables[0].Rows[0]["USER_TYPE"].ToString() == "DODD Agent")
            {
                pnlSelProvDODD.Visible = true;
                pnlSelectProviders.Visible = false;
            }
            else
            {
                pnlSelectProviders.Visible = true;
                pnlSelProvDODD.Visible = false;
            }
        }
        //if (userRoles.Contains("DODDAgent") || userRoles.Contains("CEOCertified"))
        //{
        //    pnlSelProvDODD.Visible = true;
        //    pnlSelectProviders.Visible = false;
        //}
        btnSelectProviders.Enabled = false;
    }

    protected void btnMyProviders_Click(object sender, EventArgs e)
    {
        pnlPendingAgents.Visible = false;
        pnlSelectProviders.Visible = false;
        if (btnSelectProviders.Visible) btnSelectProviders.Enabled = true;
        if (btnPendingAgents.Visible) btnPendingAgents.Enabled = true;
    }

    protected void btnSelectAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvPendingAgents.Rows)
        {
            CheckBox cbSelect = (CheckBox)row.FindControl("chkAgent");
            cbSelect.Checked = true;
        }
    }

    protected void btnSavePendingAgent_Click(object sender, EventArgs e)
    {
        var selectedUsers = string.Empty;
        StringBuilder selectedUsers1 = new StringBuilder();
        foreach (GridViewRow row in gvPendingAgents.Rows)
        {
            CheckBox cbSelect = (CheckBox)row.FindControl("chkAgent");
            if (cbSelect.Checked)
            {
                if (selectedUsers1.Length > 0)
                    selectedUsers1.Append("," + Helper.GetUserId(gvPendingAgents.DataKeys[row.RowIndex]["UserId"].ToString()));
                else
                    selectedUsers1.Append(Helper.GetUserId(gvPendingAgents.DataKeys[row.RowIndex]["UserId"].ToString()));
            }
        }
        if (selectedUsers1.Length > 0)
            selectedUsers = selectedUsers1.ToString().ToUpper();

        svc.SavePendingAgentsByProviderAdmin(selectedUsers, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        DataSet ds = svc.SelectPendingAgentsByProviderAdmin(UserID.ToString());
        gvPendingAgents.DataSource = ds;
        gvPendingAgents.DataBind();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        pnlPendingAgents.Visible = false;
        pnlSelectProviders.Visible = false;
        btnSelectProviders.Enabled = true;
        btnPendingAgents.Enabled = true;
        lbl_ProvAdminError.Text = "";
        txt_MedicaidID.Text = "";
        txt_NPI.Text = "";
        txt_TaxID.Text = "";
        txt_FacilityNo.Text = "";
        txt_ContractNo.Text = "";
    }

    protected void btnSelectSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        if (txtMedID.Text != hdnSelMedID.Value || txtTax.Text != hdnSelTaxID.Value)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Information does not match our records.";
            val.ValidationGroup = "valSelectProv";
            this.Page.Validators.Add(val);
            mpeSelProv.Show();
            return;
        }

        mpeSelProv.Hide();
    }
    protected void btnSelCancel_Click(object sender, EventArgs e)
    {
        mpeSelProv.Hide();
    }

    protected void btn_Cancel_DODD_Click(object sender, EventArgs e)
    {
        pnlPendingAgents.Visible = false;
        pnlSelProvDODD.Visible = false;
        pnlSelectProviders.Visible = false;
        btnSelectProviders.Enabled = true;
        btnPendingAgents.Enabled = true;
        txt_MedicaidID.Text = "";
        txt_NPI.Text = "";
        txt_TaxID.Text = "";
        txt_FacilityNo.Text = "";
        txt_ContractNo.Text = "";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int validate = 0;
        if (!string.IsNullOrEmpty(txt_MedicaidID.Text))
        {
            if (txt_MedicaidID.Text.Length < 7)
            {
                txt_MedicaidID.Text = txt_MedicaidID.Text.ToString().PadLeft(7, '0');
            }
        }
        if (!string.IsNullOrEmpty(txt_MedicaidID.Text))
            validate++;
        if (!string.IsNullOrEmpty(txt_NPI.Text))
            validate++;
        if (!string.IsNullOrEmpty(txt_TaxID.Text))
            validate++;

        if (validate < 2)
        {
            return;
        }
        bool isProvAgent = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetAvailableProvider(txt_NPI.Text, txt_MedicaidID.Text, txt_TaxID.Text, string.Empty, string.Empty);
        if (Helper.HasRows(ds))
        {
            string[] userRoles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);
            isProvAgent = userRoles.Contains("ProviderAgent") ? true : false;


            if (ds.Tables[0].Rows[0]["IS_OHID"].ToString().ToUpper() == "TRUE" && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OHID"].ToString())
                && userRoles.Contains("ProviderAdministrator"))            // IOP User association validation
            {
                lbl_ProvAdminError.Text = "* Provider is already associated to an another Provider Administrator with OH|ID. Contact Provider Enrollment.";
                lbl_ProvAdminError.Visible = true;
                return;
            }
            else if (ds.Tables[0].Rows[0]["IS_OHID"].ToString().ToUpper() == "FALSE" && string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OHID"].ToString()) 
                && isProvAgent)         // If No IOP User assicated with provider
            {
                lbl_ProvAdminError.Text = "* Provider is not associated with any Provider Administrator with OH|ID. Contact Provider Enrollment.";
                lbl_ProvAdminError.Visible = true;
                return;
            }
            else if (ds.Tables[0].Rows[0]["IS_OHID"].ToString().ToUpper() == "TRUE" && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OHID"].ToString()) 
                && isProvAgent)                                      // IOP associated Check OR Non-IOP associated for Provider Agent
            {
                svc.InsertNewProviderAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(),
                          ds.Tables[0].Rows[0]["UserID"].ToString(), Convert.ToInt32(ds.Tables[0].Rows[0]["RegID"].ToString()), false,
                                Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                pnlSelectProviders.Visible = false;
                btnSelectProviders.Enabled = true;
                Mod_ProviderAdminConfirm.Show();
                txt_MedicaidID.Text = "";
                txt_NPI.Text = "";
                txt_TaxID.Text = "";
                txt_FacilityNo.Text = "";
                txt_ContractNo.Text = "";
            }
            else if (ds.Tables[0].Rows[0]["UX_REG_ID"].ToString() != "0" && ds.Tables[0].Rows[0]["IS_OHID"].ToString().ToUpper() == "FALSE" && userRoles.Contains("ProviderAdministrator"))                              // Non-IOP Associated registration of Provider Admin -- Association with IOP user ProviderAdministrator
            {
                svc.UpdateRegistrationUserXref(ds.Tables[0].Rows[0]["RegID"].ToString(),
                    Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                InitView();
                pnlSelectProviders.Visible = false;
                btnSelectProviders.Enabled = true;
                Mod_ProviderAdminConfirm.Show();
                txt_MedicaidID.Text = "";
                txt_NPI.Text = "";
                txt_TaxID.Text = "";
                txt_FacilityNo.Text = "";
                txt_ContractNo.Text = "";
            }
            else if (ds.Tables[0].Rows[0]["UX_REG_ID"].ToString() == "0" && userRoles.Contains("ProviderAdministrator"))       // Provider registration check for ProviderAdministrator
            {
                svc.OnlyInsertRegistrationUserXref(ds.Tables[0].Rows[0]["RegID"].ToString(),
                      Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                InitView();
                pnlSelectProviders.Visible = false;
                btnSelectProviders.Enabled = true;
                Mod_ProviderAdminConfirm.Show();
                txt_MedicaidID.Text = "";
                txt_NPI.Text = "";
                txt_TaxID.Text = "";
                txt_FacilityNo.Text = "";
                txt_ContractNo.Text = "";
            }
        }
        else
        {
            lbl_ProvAdminError.Text = "* Information does not match our records, please re-enter.";
            lbl_ProvAdminError.Visible = true;
            Mod_ProviderAdminConfirm.Hide();
            return;
        }
    }

    protected void btn_Save_DODD_Click(object sender, EventArgs e)
    {
        int validate = 0;
        if (!string.IsNullOrEmpty(txt_FacilityNo.Text))
            validate++;
        if (!string.IsNullOrEmpty(txt_ContractNo.Text))
            validate++;

        if (validate == 0)
        {
            return;
        }

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetAvailableProvider(string.Empty, string.Empty, string.Empty, txt_FacilityNo.Text, txt_ContractNo.Text);
        if (Helper.HasRows(ds))
        {
            svc.InsertNewProviderAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(),
                        ds.Tables[0].Rows[0]["UserID"].ToString(), Convert.ToInt32(ds.Tables[0].Rows[0]["RegID"].ToString()), false,
                            Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            pnlSelProvDODD.Visible = false;
            btnSelectProviders.Enabled = true;
            txt_MedicaidID.Text = "";
            txt_NPI.Text = "";
            txt_TaxID.Text = "";
            txt_FacilityNo.Text = "";
            txt_ContractNo.Text = "";
        }
        else
        {
            lbl_ProvDODDError.Text = "* Information does not match our records, please re-enter.";
            lbl_ProvDODDError.Visible = true;
            return;
        }
    }

    protected void Btn_Ok_Click(object sender, EventArgs e)
    {
        Mod_ProviderAdminConfirm.Hide();
        Response.Redirect("~/Process/ProviderHomeNew.aspx");
    }

    protected void lnkBtnExcel_Click(object sender, EventArgs e)
    {
        string sortColumn = "ProviderName";
        int pageSize = 15;
        int startRowIndex = 0;
        int totalResultCount = 0;
        bool getTotalRowCount = true;
        DataSet ds = svc.SelectAllRegistrationsByUserID(Helper.GetUserId(HttpContext.Current.User.Identity.Name), 0, CON.ProviderCategoryTypeID.GroupMemberProfile, sortColumn, pageSize, startRowIndex, getTotalRowCount, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), out totalResultCount);
        if (Helper.HasRows(ds))
        {
            RadGridExportData.DataSource = ds.Tables[0];
            RadGridExportData.DataBind();
            RadGridExportData.MasterTableView.ExportToExcel();
        }
    }

    protected void lnkBtnPDF_Click(object sender, EventArgs e)
    {
        string sortColumn = "ProviderName";
        int pageSize = 15;
        int startRowIndex = 0;
        int totalResultCount = 0;
        bool getTotalRowCount = true;
        DataSet ds = svc.SelectAllRegistrationsByUserID(Helper.GetUserId(HttpContext.Current.User.Identity.Name), 0, CON.ProviderCategoryTypeID.GroupMemberProfile, sortColumn, pageSize, startRowIndex, getTotalRowCount, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), out totalResultCount);
        if (Helper.HasRows(ds))
        {
            RadGridExportData.DataSource = ds.Tables[0];
            RadGridExportData.DataBind();
            RadGridExportData.MasterTableView.ExportToPdf();
        }
    }
}


