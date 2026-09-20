using Corp.Core.Libraries;
using Corp.Core.Libraries.Helper;
using Corp.Core.Libraries.Interface;
using Corp.Core.Libraries.Proxy;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using PdmsWebEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class MasterWorkflowPage : BaseMasterPage
{
    // TODO: EDV Move these properties into workflowpage
    public int EntityTypeID
    {
        get
        {
            return this.WorkflowPage.EntityTypeID;
        }
        set { this.WorkflowPage.EntityTypeID = value; }
    }
    private Guid m_threadId;
    public string SnippetKey;
    private Guid ThreadId
    {
        get
        {
            return this.m_threadId;
        }
        set
        {
            this.m_threadId = value;
        }
    }
    public int WorkflowID
    {
        get
        {
            return this.WorkflowPage.WF_WorkflowID;
        }
        set { this.WorkflowPage.WF_WorkflowID = value; }
    }
    public string WorkflowName
    {
        get
        {
            return this.WorkflowPage.WorkflowName;
        }
        set { this.WorkflowPage.WorkflowName = value; }
    }
    public int TaskID
    {
        get
        {
            return this.WorkflowPage.WF_TaskID;
        }
        set { this.WorkflowPage.WF_TaskID = value; }
    }
    public string TaskType { get; set; }
    public string TaskName
    {
        get
        {
            return this.WorkflowPage.CurrentTaskName;
        }
        set { this.WorkflowPage.CurrentTaskName = value; }

    }
    public int ProviderCategoryTypeID
    {
        get
        {
            return this.WorkflowPage.EntityTypeID;
        }
        set { this.WorkflowPage.EntityTypeID = value; }
    }

    public bool ApplicationFeeRequired
    {
        get
        {
            if (ViewState["ApplicationFeeRequired"] == null)
                ViewState["ApplicationFeeRequired"] = false;

            return (bool)ViewState["ApplicationFeeRequired"];
        }
        set
        {
            ViewState["ApplicationFeeRequired"] = value;
        }
    }


    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public string GroupNameList { get; set; }
    public int StepID
    {
        get
        {
            return this.WorkflowPage.WF_StepID;
        }
        set { this.WorkflowPage.WF_StepID = value; }
    }
    public string StepOwner { get; set; }
    public DateTime StepCreateDate { get; set; }
    public DateTime StepStartDate { get; set; }
    public DateTime? StepEndDate { get; set; }
    public string StepNotes { get; set; }
    public string RedirectURL { get; set; }
    public bool ViewProviderFile { get; set; }
    //public TreeView RegistrationTreeViewControl { get { return this.ucRegTreeView.RegistrationTreeViewControl; } }

    /*public bool RegistrationTreeViewContains(string nodeName)
    {
        bool containsNode = false;
        foreach (TreeNode childNode in RegistrationTreeViewControl.Nodes[0].ChildNodes)
        {
            if (childNode.Text == nodeName)
            {
                containsNode = true;
                break;
            }
        }
        return containsNode;
    }*/

    public int ProcessID
    {
        get
        {
            return this.WorkflowPage.WF_ProcessID;
        }
        set { this.WorkflowPage.WF_ProcessID = value; }
    }
    public string ProcessOwner { get; set; }
    public DateTime ProcessStartDate { get; set; }
    public DateTime? ProcessEndDate { get; set; }
    public int LogCount { get; set; }
    public Guid LogThreadID { get; set; }
    //public UserControls_RegistrationTreeView RegTreeView { get { return ucRegTreeView; } }
    public int TimeOutValue = 0;

    private Dictionary<string, string> TaskParameter
    {
        get
        {
            return this.WorkflowPage.TaskParameter;
        }
        set { this.WorkflowPage.TaskParameter = value; }
    }
    private Dictionary<string, string> ProcessParameter
    {
        get
        {
            return this.WorkflowPage.ProcessParameter;
        }
        set { this.WorkflowPage.ProcessParameter = value; }
    }
    private Dictionary<string, string> StepParameter
    {
        get
        {
            return this.WorkflowPage.StepParameter;
        }
        set { this.WorkflowPage.StepParameter = value; }
    }


    #region SVC
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
    public string ValidationGroup { set { vsProviderInfoHeader.ValidationGroup = value; } }
    #endregion

    private string OHID
    {
        get
        {
            if (string.IsNullOrEmpty(SessionVarRetriever.OhID))
            {
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                DataSet ds = svc.GetUserAccountInformation(Helper.GetUserId(SessionVarRetriever.UserName).ToString());
                if (Helper.HasRows(ds))
                {
                    SessionVarRetriever.OhID = ds.Tables[0].Rows[0]["OhID"].ToString();
                }
                return SessionVarRetriever.OhID;
            }
            else
            {
                return SessionVarRetriever.OhID;
            }
        }
    }

    #region Form Events

    protected void Page_Init(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SnippetKey = ConfigurationManager.AppSettings["ZendeskSnippetKey"];
        string eventTarget = Request["__EVENTTARGET"];
        string eventArgument = Request["__EVENTARGUMENT"];

        if (eventTarget != null && eventArgument != null && eventTarget.Equals("btnbackToSummary") && eventArgument.Equals("OnClick"))
        {
            RedirectToProviderSummary();
        }

        RaisePageLoadEvent();
        LoginName loginName = null;

        ucRegProgressBar.RefreshEvent += new UserControls_RegistrationProgressBar.RefreshEventHandler(ucRegistrationNavigation_RefreshEvent);
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        // DCPDMS-3213 - upped the timeout difference so the telerik and forms authentication timeouts don't happen at the same time.  RBM 8/27/2019
        TimeOutValue = Convert.ToInt32(System.Web.Security.FormsAuthentication.Timeout.TotalMinutes - 2) * 60;
        if (!IsPostBack)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                RadSessionNote.Enabled = true;
                //configure the notification to automatically show 1 min before session expiration
                RadSessionNote.ShowInterval = TimeOutValue * 1000;
                //set the redirect url as a value for an easier and faster extraction in on the client
                RadSessionNote.Value = Page.ResolveClientUrl("~/Account/Logout.aspx");

                //refresh token code
                HttpCookie IOPAccessToken = Request.Cookies["IOPAccessToken"];
                if (IOPAccessToken == null && SessionVarRetriever.IsOhID)
                {
                    RefreshIOPTokens();
                }

                HyperLink hypProfile = (HyperLink)Helper.FindTheControl(LoginView2, "hypProfile");
                if (SessionVarRetriever.IsOhID) hypProfile.Enabled = false;
            }
            else
            {
                RadSessionNote.Enabled = false;
                HyperLink hypSignup = (HyperLink)Helper.FindTheControl(LoginView2, "hypSignup");
                hypSignup.NavigateUrl = System.Configuration.ConfigurationManager.AppSettings["OHredirectUri"];
            }

            loginName = LoginView2.FindControl("LoginName1") as LoginName;

            if (loginName != null)
            {
                loginName.FormatString = SessionVarRetriever.DisplayUserName;
                loginName.ToolTip = OHID;
            }

            // Get version number
            string versionText = "Version: " + Helper.GetAppSettingFromDB("Version");
            string env = Helper.GetAppSettingFromDB("Environment");
            if (!string.IsNullOrEmpty(env))
                versionText += " (" + env + ")";

            var buildVersion = Convert.ToString(ConfigurationManager.AppSettings["BuildVersion"]);
            if (!string.IsNullOrEmpty(buildVersion))
                versionText += " " + buildVersion;
            var buildDate = Convert.ToString(ConfigurationManager.AppSettings["BuildDate"]);
            if (!string.IsNullOrEmpty(buildDate))
                versionText += " " + buildDate;

            lblVersion.Text = versionText;

            SetWorkFlowActions(StepID);

            string showChat = AppSettings.Get("PDMS-MEDChat");
        }
        else
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated && SessionVarRetriever.IsOhID)
            {
                //refresh token code

                HttpCookie IOPAccessToken = Request.Cookies["IOPAccessToken"];
                if (IOPAccessToken == null)
                {
                    RefreshIOPTokens();
                }
            }
        }
        GetStepInfo();
        ucNotes.RefreshEvent += new UserControls_Notes.RefreshEventHandler(ucNotes_RefreshEvent);

        this.ucDisenrollmentView.KeepPopupOpenEvent += new Views_DisenrollmentView.KeepPopupOpenEventHandler(View_KeepPopupOpen);
        // This allows the page header directive to work: <script src="<%# Page.ResolveClientUrl("~/Scripts/jquery-ui.js") %>"></script>
        Page.Header.DataBind();
        btnUpdateOk.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnUpdateOk, null) + ";");
    }

    private void RedirectToProviderSummary()
    {
        RegistrationProvider reg = new RegistrationProvider();
        reg.RegistrationId = this.WorkflowPage.RegistrationId;
        reg.RegistrationStep = this.WorkflowPage.RegistrationStep;
        reg.IsReadOnly = false;

        Session["RegistrationProvider"] = reg;
        Response.Redirect("~/Process/ProviderUpdateSummary.aspx");
    }

    public void RefreshIOPTokens()
    {
        string refreshToken = string.Empty;
        HttpCookie IOPRefreshToken = Request.Cookies["IOPRefreshToken"];
        if (IOPRefreshToken != null)
        {
            refreshToken = IOPRefreshToken.Value.ToString();
            ReceiveTokenOpenId.IOPAccessToken tokenobj;
            ReceiveTokenOpenId IOPrefresh = new ReceiveTokenOpenId();
            tokenobj = IOPrefresh.IOPRefreshTokens(refreshToken);
            int IOP_RefreshTokenTrustedLength = Convert.ToInt32(AppSettings.Get("IOPCookieRefreshTime"));
            if (tokenobj != null)
            {

                Response.Cookies.Remove("IOPAccessToken");
                Response.Cookies.Remove("IOPRefreshToken");
                Response.Cookies.Remove("IOPIDToken");

                // Create the Cookies to the new token object
                HttpCookie IOPAccessToken = new HttpCookie("IOPAccessToken");
                IOPAccessToken.HttpOnly = true;
                IOPAccessToken.Value = tokenobj.access_token;
                IOPAccessToken.Expires = DateTime.Now.Add(new TimeSpan(0, 0, tokenobj.expires_in));
                Response.Cookies.Add(IOPAccessToken);

                HttpCookie IOPRefreshTokenNew = new HttpCookie("IOPRefreshToken");
                IOPRefreshTokenNew.HttpOnly = true;
                IOPRefreshTokenNew.Value = tokenobj.refresh_token;
                IOPRefreshTokenNew.Expires = DateTime.Now.Add(new TimeSpan(IOP_RefreshTokenTrustedLength, 0, 0));
                Response.Cookies.Add(IOPRefreshTokenNew);
            }
            else                    // if no tokens are received during refresh tokens logout the user
            {
                string logoutredirectUri = System.Configuration.ConfigurationManager.AppSettings["OHredirectUri"];
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    HttpContext.Current.Session.Clear();
                    HttpContext.Current.Session.Abandon();                  // Remove the session entries
                    System.Web.Security.FormsAuthentication.SignOut();
                    HttpContext.Current.Session.RemoveAll();
                    Request.Cookies.Clear();
                    Response.Redirect(logoutredirectUri, false);
                }
            }
        }
    }
    protected void btnLogoutOk_Clicked(object sender, EventArgs e)
    {
        //string logoutredirectUri = System.Configuration.ConfigurationManager.AppSettings["OHredirectUri"];

        //if (Request.Cookies["IOPIDToken"] != null)
        //{
        //    HttpCookie IOPIDToken = Request.Cookies["IOPIDToken"];
        //    IOPIDToken.Value = null;
        //    IOPIDToken.Expires = DateTime.Now.AddDays(-365);
        //    Response.SetCookie(IOPIDToken);
        //    Response.Cookies.Remove("IOPIDToken");
        //}

        //if (Request.Cookies["IOPAccessToken"] != null)
        //{
        //    HttpCookie IOPAccessToken = Request.Cookies["IOPAccessToken"];
        //    IOPAccessToken.Value = null;
        //    IOPAccessToken.Expires = DateTime.Now.AddDays(-365);
        //    Response.SetCookie(IOPAccessToken);
        //    Response.Cookies.Remove("IOPAccessToken");
        //}

        //if (Request.Cookies["IOPRefreshToken"] != null)
        //{
        //    HttpCookie IOPRefreshToken = Request.Cookies["IOPRefreshToken"];
        //    IOPRefreshToken.Value = null;
        //    IOPRefreshToken.Expires = DateTime.Now.AddDays(-365);
        //    Response.SetCookie(IOPRefreshToken);
        //    Response.Cookies.Remove("IOPRefreshToken");
        //}

        //HttpCookie aCookie;
        //string cookieName;
        //int limit = Request.Cookies.Count;
        //for (int i = 0; i < limit; i++)
        //{
        //    cookieName = Request.Cookies[i].Name;
        //    aCookie = new HttpCookie(cookieName);
        //    aCookie.Expires = DateTime.Now.AddDays(-365);
        //    Response.Cookies.Add(aCookie);
        //}

        //HttpContext.Current.Session.Clear();
        //HttpContext.Current.Session.Abandon();                  // Remove the session entries
        //System.Web.Security.FormsAuthentication.SignOut();
        //HttpContext.Current.Session.RemoveAll();
        //Request.Cookies.Clear();
        //Response.Redirect(logoutredirectUri, false);
    }
    private void ClearCookies()
    {
        HttpCookie IOPAccessToken = new HttpCookie("IOPAccessToken");
        IOPAccessToken.HttpOnly = true;
        IOPAccessToken.Value = "";
        IOPAccessToken.Expires = DateTime.Now.AddDays(-1);
        Response.Cookies.Add(IOPAccessToken);

        HttpCookie IOPRefreshToken = new HttpCookie("IOPRefreshToken");
        IOPRefreshToken.HttpOnly = true;
        IOPRefreshToken.Value = "";
        IOPRefreshToken.Expires = DateTime.Now.AddDays(-1);
        Response.Cookies.Add(IOPRefreshToken);
    }
    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            if (SessionVarRetriever.IsOhID)
            {
                // Remove the token from the DB on logout
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                //svc.SaveUserIOPToken(Helper.GetUserId(HttpContext.Current.User.Identity.Name), string.Empty, Helper.GetUserId(HttpContext.Current.User.Identity.Name), string.Empty, false);
                Response.Redirect("~/Account/Logout.aspx", false);
                //mpeLogoutMsg.Show();
                //return;
            }
            else
            {
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                svc.SaveUserPNMToken(Helper.GetUserId(HttpContext.Current.User.Identity.Name), string.Empty); // clear haven token
                System.Web.Security.FormsAuthentication.SignOut();
                HttpContext.Current.Session.Abandon();                  // Remove the session entries
                Response.Redirect(Helper.RedirectLoginURL(), false);
            }
            ClearCookies();
        }
    }

    private void RaisePageLoadEvent()
    {
        var dictionary = new Dictionary<string, string>();
        dictionary.Add("REG_ID", this.WorkflowPage.RegistrationIdSelected > 0 ? this.WorkflowPage.RegistrationIdSelected.ToString() : this.WorkflowPage.RegistrationId.ToString());
        dictionary.Add("Step_Id", this.WorkflowPage.WF_StepID.ToString());
        dictionary.Add("Section_Id", this.WorkflowPage.RegistrationStep.ToString());
        PageLoadEvent.Raise(dictionary, "Page Load", this);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
    }

    private void ucRegistrationNavigation_RefreshEvent(int step)
    {
        (this.Page as WorkflowPage).ucRegistrationNavigation_RefreshEvent(step);
    }

    void ucNotes_AddNoteEvent()
    {
        mpeNotesGrid.Show();
    }

    void ucNotes_RefreshEvent()
    {
        mpeNotesGrid.Show();
    }
    // Remove the "Login" menu option since the user is logged into the application

    protected void OnCallbackUpdate(object sender, RadNotificationEventArgs e)
    {

    }


    protected void btnLogOut_Clicked(object sender, EventArgs e)
    {
        string logoutredirectUri = System.Configuration.ConfigurationManager.AppSettings["OHredirectUri"];
        //Response.Redirect("~/Account/Logout.aspx");
        if (Page.IsPostBack)
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (SessionVarRetriever.IsOhID)
                {
                    HttpContext.Current.Session.Clear();
                    HttpContext.Current.Session.Abandon();                  // Remove the session entries
                    System.Web.Security.FormsAuthentication.SignOut();
                    HttpContext.Current.Session.RemoveAll();
                    Request.Cookies.Clear();
                    Response.Redirect(logoutredirectUri, false);
                }
                else
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    HttpContext.Current.Session.Abandon();                  // Remove the session entries
                    Response.Redirect(Helper.RedirectLoginURL(), false);
                }

            }
    }

    protected void mnuTop_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Menu mnu = (Menu)sender;
            if (mnu == null) return;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                RemoveMenuItem(mnu.Items, "Create Account");
                RemoveMenuItem(mnu.Items, "Login");
                RemoveMenuItem(mnu.Items, "Log In");
                if (!Helper.IsLoggedInUserInAdminRole()) RemoveMenuItem(mnu.Items, "My Queue");
            }

            //// Hide Public Search if logged in, hide GroupReview page if not logged in
            //MenuItem mnuItemToRemove = null;
            //foreach (MenuItem item in ((Menu)sender).Items)
            //{
            //    if ((HttpContext.Current.User.Identity.IsAuthenticated && item.NavigateUrl.Contains("PublicSearch.aspx")))
            //    {
            //        mnuItemToRemove = item;
            //    }
            //}
            //if (mnuItemToRemove != null)
            //    mnuLeftNav.Items.Remove(mnuItemToRemove);

        }
    }

    protected void ddlAssignedTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int pid = 0;
        int sid = 0;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

        int regid = this.WorkflowPage.RegistrationIdSelected > 0 ? this.WorkflowPage.RegistrationIdSelected : this.WorkflowPage.RegistrationId;

        //Check if assigning to CredChar and Credentilng risk level is Medium/High then show error msg
        DataSet ds = svc.SelectProviderCredentialingData(regid);
        DataRow[] selectedRows = null;
        if (this.WorkflowPage.ActiveScreeningID > 0)
        {
            selectedRows = ds.Tables[0].Select("credentialing_id = " + this.WorkflowPage.ActiveScreeningID);
        }
        else //see if there is any pending credentialing screenings
        {
            //Get the values from Appsettings, default to 12 - ODM Credentialing Review 
            string credStatus = AppSettings.Get("AssignToCredChairCredentialStatusesToStop", "12");
            // Split and parse to int
            var credStatusList = credStatus.Split(',')
                .Select(s => int.Parse(s.Trim()))
                .ToList();
            // Join for SQL IN condition
            string inCondition = string.Join(",", credStatusList);
            // Usage in DataTable.Select:
            selectedRows = ds.Tables[0].Select(String.Format("CREDENTIALING_STATUS_ID in ({0})", inCondition));
        }

        if (selectedRows != null && selectedRows.Length == 1)
        {
            if (Helper.IsUserInRole(Helper.GetUserName(ddlAssignedTo.SelectedValue), CON.UserRole.CredentialingChair))
            {
                if (selectedRows[0]["RISK_LEVEL_ID"].ToString() != "1")
                {
                    lblAssignToError.Text = "This is a medium or high risk file and must be dispositioned by a Credentials Committee Quality Specialist.";
                    ddlAssignedTo.SelectedIndex = 0;
                    return;
                }
                else
                    lblAssignToError.Text = "";
            }
        }
        else
        {
            lblAssignToError.Text = "";
        }

        DataSet dsG = svc.GetGroupNames(regid);
        if (Helper.HasRows(dsG))
        {
            foreach (DataRow row in dsG.Tables[0].Rows)
            {
                sid = Helper.GetInt("STEP_ID", row);
                pid = Helper.GetInt("PROCESS_ID", row);
            }
        }
        if (sid != 0)
        {
            svc.updateWF_STEP_Owner(sid, ddlAssignedTo.SelectedIndex == 0 ? null : ddlAssignedTo.SelectedValue);
            //Update Assignment User/Date
            svc.updateWF_STEP_Assignment(sid, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            if (Helper.GetUserRole(ddlAssignedTo.SelectedItem.Text) == CON.UserRole.CredentialingSpecialist ||
               Helper.GetUserRole(ddlAssignedTo.SelectedItem.Text) == CON.UserRole.ODMCredentialingSpecialist)
            {
                svc.UpdateCredentialStatus(regid, CON.CredentilaingStatus.InProcess, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            }
        }
    }




    protected void rptWorkflowActions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

        Button btn = (Button)e.Item.FindControl("btnAction");
        if (btn != null)
        {
            // Set the disable after click action. If we dont do this can be BIG problems with multiple click postbacks.
            btn.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btn, null) + ";");
        }
    }

    protected void btnAction_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        if (btn != null)
        {
            bool isQueueTask = false;

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet dstaks = svc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
            isQueueTask = Helper.HasRows(dstaks) ? Helper.GetString("CurrentTaskType", dstaks.Tables[0].Rows[0]) == "Queue" : false;
            if (!isQueueTask)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (btn.Text == CON.RegistrationWorkflowActionName.ApproveProvider && this.WorkflowPage.CurrentTaskName != "Retro Review" && this.WorkflowPage.CurrentTaskName != "Group Member Retro Review")
            {
                //show the popup only for approve provider if override screening is selected
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "APPEAL");
                DataTable dtProvider = new DataTable();

                if (Helper.HasRows(ds))
                {
                    dtProvider = ds.Tables[0];
                    string stateREviewScreeningStatus = Helper.GetString("STATE_REVIEW_SCREENING_STATUS_ID", dtProvider.Rows[0]);
                    if (!string.IsNullOrEmpty(stateREviewScreeningStatus))
                    {
                        //When override screening is selected on take action
                        if (stateREviewScreeningStatus == CON.StateReviewScreeningStatus.OverrideScreening.ToString() && WorkflowID != CON.WorkflowType.PeriodicDatabaseChecks)
                        {
                            //ShowPopup(ucApproveProvider, "Approve Provider", "", 0, 0);
                            ShowPopup(ucApproveProvider, "Approve Provider", "", 0, 0);
                            ucApproveProvider.LoadControls();
                            return;
                        }
                    }
                }

            }
            if (btn.Text == CON.RegistrationWorkflowActionName.ReturnToScreening)
            {
                ShowPopup(ucReturnToScreening, "Return To Screening", "", 0, 1);
                ucReturnToScreening.LoadControls();
                return;
            }
            if (btn.Text == CON.RegistrationWorkflowActionName.NotProcessed)
            {
                ShowPopup(ucReturnToScreening, "Take Action - Not Processed", "ValNotProcessed", 0, 5);
                ucReturnToScreening.LoadControls();
                return;
            }
            if (btn.Text == CON.RegistrationWorkflowActionName.ReturnToSiteVisit)
            {
                ShowPopup(ucReturnToScreening, "Return To Site Visit", "", 0, 1);
                ucReturnToScreening.LoadControls();
                return;
            }
            if (btn.Text == CON.RegistrationWorkflowActionName.ReturnToProviderReview)
            {
                ShowPopup(ucReturnToScreening, "Return To Provider Review", "", 0, 1);
                ucReturnToScreening.LoadControls();
                return;
            }
            if (btn.Text == CON.RegistrationWorkflowActionName.LTEnrollment)
            {
                ShowPopup(ucLTProviderEnrollment, "LT Provider Enrollment", "", 0, 3);
                ucLTProviderEnrollment.LoadControls();
                return;
            }
            // SAM763
            if (btn.Text == "Refer To Compliance")
            {
                ShowPopup(ucComplReason, "Refer to Compliance Reasons", "", 0, 6);
                ucComplReason.LoadControls();
                return;
            }
            if ((btn.Text == CON.RegistrationWorkflowActionName.SubmitForReview || btn.Text == CON.RegistrationWorkflowActionName.SubmitUpdate) && this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg)
            {
                if (WorkflowID == CON.WorkflowType.CPC && !Helper.IsUpdateCPCContact)
                {
                    // Check if added Kids spec then verify if kids attestation selected.
                    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                    DataSet ds = psc.VerifyCPCKidsAttestationSelected(this.WorkflowPage.RegistrationId);

                    if (Helper.HasRows(ds))
                    {
                        int result = Helper.GetInt("Result", ds.Tables[0].Rows[0]);
                        if (result == 1)
                        {
                            AddScreenErrorHeading("Please go to the Attestation page and attest to changes in the application.");
                            return;
                        }
                    }
                }

                if (!ValidateUploadDocuments())
                {
                    return;
                }
                CheckforModifiedSections();
                mpeUpdateChanges.Show();
                return;
            }

            if (btn.Text == "Review Disenrollment Request")
            {
                this.ucDisenrollmentView.IsSuspend = false;
                if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.Administrator))
                    lblpopTitle.Text = "Provider Disenrollment";
                else if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) || Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, this.WorkflowPage.RegistrationId))
                {

                    lblpopTitle.Text = "Request Disenrollment";
                    this.ucDisenrollmentView.IsNotAdminFields = true;
                }

                this.ucDisenrollmentView.InitView(this.WorkflowPage.RegistrationId);
                mpeSaveDisEnrollement.Show();
                return;

            }
            if (btn.Text == CON.RegistrationWorkflowActionName.planofcorrection)
            {
                if (!ValidatePOCUpload())
                {
                    return;
                }
            }
            //if ((btn.Text == "Review Complete") && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.CredentialCommitteeReview)
            //{
            //    if (Helper.GetPeningCommitteememberStatusCount(this.WorkflowPage.RegistrationId) > 1)
            //    {
            //        //Still need to approve by other committe members so just redirect to default page after assigning to next member
            //        AssigntoNextCommitteeMember();
            //        Response.Redirect("~/Process/CredentialQueue.aspx");
            //    }
            //    else
            //    {
            //        //Finishes all the committee members review set the redirect page
            //        RedirectURL = "~/Process/CredentialQueue.aspx";
            //    }
            //}
            //else
            //{
            //    if ((this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.CredentialCommitteeChairReview) &&( (btn.Text == "Review Complete")||(btn.Text == "Deny")))
            //    {
            //        RedirectURL = "~/Process/CredentialQueue.aspx";//to redirect to Queue page.
            //    }
            //}
            if ((btn.Text == "Approve" || btn.Text == "Process Discontinue") && (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderCredentialing || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderCredentialingODM))
            {
                int credentialStatus = 0;
                DataSet ds = svc.SelectProviderCredentialingData(this.WorkflowPage.RegistrationId);
                if (Helper.HasRows(ds))
                    credentialStatus = Convert.ToInt32(ds.Tables[0].Rows[0]["CREDENTIALING_STATUS_ID"]);
                if (credentialStatus == CON.CredentilaingStatus.PendingProcess)
                {
                    AddScreenErrorHeading("Please uncheck Pending Verification before proceeding.");
                    return;
                }

            }
            if ((btn.Text == "Terminate" || btn.Text == "Deny") && this.WorkflowPage.CurrentTaskName.Equals(CON.RegistrationTaskName.ProviderScreening))
            {
                string termReason = string.Empty;
                bool isFedMatch = svc.CheckFedExclusionsMatch(this.WorkflowPage.RegistrationId);
                bool isDODDMatch = svc.CheckDODDAbuserRegistryMatch(this.WorkflowPage.RegistrationId);

                if (isFedMatch && isDODDMatch)
                {
                    termReason = CON.EnrollStatusReason.TERMINATEDFEDEXCLUSIONSYSTEM;
                }
                else if (isDODDMatch)
                {
                    termReason = CON.EnrollStatusReason.STATE_INITIATED_TERMINATION;
                }

                if (btn.Text == "Terminate")
                {
                    DataSet dsTransID = svc.TerminateProvider(this.WorkflowPage.RegistrationId, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EnrollStatus.INACTIVE.ToString(), termReason, true);

                }
                else if (btn.Text == "Deny")
                {
                    DataSet dsTransID = svc.TerminateProvider(this.WorkflowPage.RegistrationId, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EnrollStatus.INACTIVE.ToString(), CON.EnrollStatusReason.DENIED, true);
                }
            }
            if ((btn.Text == "Terminate" || btn.Text == "Deny") && Registration.IsComplianceTaskType(this.WorkflowPage.CurrentTaskName)
                && this.WorkflowPage.WorkflowEventTypeId != CON.WorkflowEventType.Reconsideration && this.WorkflowPage.WorkflowEventTypeId != CON.WorkflowEventType.CredentialReconsideration)
            {
                bool checkTermEntered = svc.CheckHRTermFieldsByRegID(this.WorkflowPage.RegistrationId);
                if (!checkTermEntered)
                {
                    switch (btn.Text)
                    {
                        case "Terminate":
                            AddScreenErrorHeading("Please enter Date of Termination and select Termination Reason");
                            break;
                        case "":
                            AddScreenErrorHeading("Please enter Date of Denial and select Denial Reason");
                            break;
                    }

                    return;
                }
                else
                {
                    DataSet ds = svc.GetHRTermFieldsByRegID(this.WorkflowPage.RegistrationId);
                    DataSet dsRegProv = svc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
                    DateTime termDate = new DateTime();
                    string termReason = string.Empty;
                    if (ObjectControllerHelper.HasRows(ds))
                    {
                        termDate = ObjectControllerHelper.GetDateTime("DATE_TERMINATION", ds.Tables[0].Rows[0]);
                        termReason = ObjectControllerHelper.GetString("TERMINATION_REASON", ds.Tables[0].Rows[0]);
                    }

                    DataSet dsTransID = svc.TerminateProvider(this.WorkflowPage.RegistrationId, termDate, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EnrollStatus.INACTIVE.ToString(), termReason, false);

                    //OHPNM-9814 - If terminating already terminated provider dont send transactions to FI/MITS.
                    if (ObjectControllerHelper.HasRows(dsRegProv))
                    {
                        DateTime terminationDate = ObjectControllerHelper.GetDateTime("TerminationDate", dsRegProv.Tables[0].Rows[0]);

                        if (!ObjectControllerHelper.IsDateNull(terminationDate))
                        {
                            NextStep("Deny");
                            Response.Redirect("~/Default.aspx");
                        }
                    }

                }
            }
            // Jira OHPNM-3251 Commenting out as there is no seperate Reconsideration Workflow.  
            //if ((btn.Text == "Terminate" || btn.Text == "Deny") && (this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.ReconsiderationEnterTR)
            //&& this.WorkflowID == CON.WorkflowType.Reconsideration)
            //{
            //    bool checkTermEntered = svc.CheckRRTermFieldsByRegID(this.WorkflowPage.RegistrationId); ;
            //    if (!checkTermEntered && btn.Text == "Terminate")
            //    {
            //        AddScreenErrorHeading("Please enter Effective Date of Termination and select Termination Reason");
            //        return;
            //    }
            //    else if (checkTermEntered && btn.Text == "Terminate")
            //    {
            //        DataSet ds = svc.GetRRTermFieldsByRegID(this.WorkflowPage.RegistrationId);
            //        DateTime termDate = new DateTime();
            //        string termReason = string.Empty;
            //        if (ObjectControllerHelper.HasRows(ds))
            //        {
            //            termDate = ObjectControllerHelper.GetDateTime("EFFECTIVE_DATE", ds.Tables[0].Rows[0]);
            //            termReason = ObjectControllerHelper.GetString("TERMINATION_REASON", ds.Tables[0].Rows[0]);
            //        }
            //        svc.TerminateProvider(this.WorkflowPage.RegistrationId, termDate, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EnrollStatus.INACTIVE.ToString(), termReason, false);
            //    }
            //}

            if ((btn.Text == "Return To Provider" || btn.Text == "Return to Provider") && (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.SiteVisitCompliance || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderCredentialingODM || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderScreening
                                                     || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.IncidentComplianceReview || this.WorkflowPage.CurrentTaskName == "Provider Credentialing"))
            {
                ShowPopup(ucCredentialRTP, "Return To Provider", "", 0, 4);
                ucCredentialRTP.LoadControls();
                return;
            }

            if (btn.Text == "Approve" && this.WorkflowPage.CurrentTaskName.Equals(CON.RegistrationTaskName.ProviderScreening))
            {
                ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, "Provider Screening Approved",
                             personReviewedBy: HttpContext.Current.User.Identity.Name, finalDisposition: CON.FinalDisposition.Approved, processID: this.WorkflowPage.WF_ProcessID);
            }


            if (btn.Text == "Application Complete" && TaskName == "Provider Review")
            {
                ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, "Provider Review Approved",
                             personReviewedBy: HttpContext.Current.User.Identity.Name, finalDisposition: CON.FinalDisposition.Approved, processID: this.WorkflowPage.WF_ProcessID);
            }

            // OHPNM-7124 - removed code that was here related to the Approve and Accept button; it made no sense and wasn't doing anything

            if ((MainContent.Page as WorkflowPage).OnTaskExit(btn.Text))
            {

                //SaveProcessParameters();
                //SaveStepParameters();
                NextStep(btn.Text);
                if (btn.Text == "Application Complete" && TaskName == "Provider Review" && (this.WorkflowPage.MMISProviderTypeID == "86" || this.WorkflowPage.MMISProviderTypeID == "89" || this.WorkflowPage.MMISProviderTypeID == "88"))
                {
                    return;
                }
                else
                {
                    if (this.WorkflowPage.WF_StepID == CurrentStep(this.WorkflowPage.RegistrationId))
                        Response.Redirect("~/Exception.aspx?Message=" + "Step did not advance, RegistrationId: " +
                            this.WorkflowPage.RegistrationId.ToString() + ". Please contact Provider Services");
                    else
                    {
                        if (!string.IsNullOrEmpty(RedirectURL) && this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CMC && (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CMCEnroll || this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CMCReAttest))
                        {
                            modalCMCSucess.Show();
                            return;
                        }      // Redirect to where needed
                        if (!string.IsNullOrEmpty(RedirectURL)) Response.Redirect(RedirectURL);     // Redirect to where needed                       
                        else Response.Redirect("~/Default.aspx");                                   // Default go Home
                    }
                }


            }
            else
            {
                btn.Enabled = true;                                                        // Enable the button
                if (btn.Text == "End Review" && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.IncidentComplianceReview && this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.IncidentCompliance)
                {
                    AddScreenErrorHeading("Error Updating Status to IMS.");
                    return;
                }
            }
        }
    }
    private void View_KeepPopupOpen()
    {
        this.mpeSaveDisEnrollement.Show();
    }
    private void AssigntoNextCommitteeMember()
    {
        try
        {
            int regid = this.WorkflowPage.RegistrationId;
            string username = Helper.GetNextPeningCommitteemember(regid);
            int pid = 0;
            int sid = 0;
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet dsG = svc.GetGroupNames(regid);
            if (Helper.HasRows(dsG))
            {
                foreach (DataRow row in dsG.Tables[0].Rows)
                {
                    sid = Helper.GetInt("STEP_ID", row);
                    pid = Helper.GetInt("PROCESS_ID", row);
                }
            }
            if (sid != 0)
            {
                //This step for auditing purpose in credentialing.
                svc.updateWF_STEP_OwnerWithStartDate(sid, Helper.GetUserId(username).ToString(), DateTime.Now);
            }

        }
        catch (Exception)
        {
        }
    }

    protected void btnHide_Click(object sender, EventArgs e)
    {
        modalCMCSucess.Hide();
        Response.Redirect("~/Process/SubmissionConfirmation.aspx?RegID=" + this.WorkflowPage.RegistrationId.ToString());
    }

    protected void btnUpdateOk_Click(object sender, EventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        int regID = this.WorkflowPage.RegistrationId;
        string userID = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        SetTaxEntityType();
        if (Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name))
        {
            Registration.UpdateRegistrationStatus(regID, MAXIMUS.Core.Libraries.Constants.RegistrationStatusTypeId.Submitted, userID);
            SetSectionStatus();
            RedirectURL = "~/Process/MyQueue.aspx";
            //SaveProcessParameters();
            //SaveStepParameters();
            NextStep("Submit Update");
        }
        else
        {
            Registration.UpdateRegistrationStatus(regID, MAXIMUS.Core.Libraries.Constants.RegistrationStatusTypeId.Submitted, userID);
            psc.updateWF_STEP_Owner(this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString()); //OHPNM-20506 Update the provadmin/poweragent/provagent that is actually submitting Provider data entry
            RedirectURL = "~/Process/SubmissionConfirmation.aspx?RegID=" + this.WorkflowPage.RegistrationId.ToString();
            //SaveProcessParameters();
            //SaveStepParameters();
            if (WorkflowID == 18)  //TODO credentialing process for DEMO hardcoded
                NextStep("");
            else
                NextStep("Submit Update");
        }

        if (this.WorkflowPage.WF_StepID == CurrentStep(this.WorkflowPage.RegistrationId))
            Response.Redirect("~/Exception.aspx?Message=" + "Step did not advance, RegistrationId: " +
                this.WorkflowPage.RegistrationId.ToString() + ". Please contact Provider Services");
        else
        {
            if (!string.IsNullOrEmpty(RedirectURL)) Response.Redirect(RedirectURL);     // Redirect to where needed
            else Response.Redirect("~/Default.aspx");                                   // Default go Home
        }

    }
    private void SetSectionStatus()
    {
        //change regproviderstatus and regproviderservices status to complete and approved after submitting updates by operator
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SECTION_STATUS");

        if (Helper.HasRows(ds))
        {
            DataTable dt = ds.Tables[0];


            foreach (DataRow dr in dt.Rows)
            {
                RegistrationNode appNode = null;

                if (this.WorkflowPage.RegistrationNodes.Count > 0)
                {
                    if (this.WorkflowPage.RegistrationNodes.ContainsKey(Helper.GetInt("REG_SECTION_TYPE_ID", dr)))
                    {
                        appNode = this.WorkflowPage.RegistrationNodes[Helper.GetInt("REG_SECTION_TYPE_ID", dr)];
                        if (appNode != null || this.WorkflowPage.Page.ToString().Contains("providerupdatesummary_aspx")) //if from providersummary page appnode is empty
                        {
                            if (Helper.GetInt("REG_PROVIDER_STATUS_TYPE_ID", dr) == CON.RegistrationProviderStatusTypeId.Modified && this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg)
                            {
                                svc.SaveRegistrationSectionStatus(this.WorkflowPage.RegistrationId, 0, Helper.GetInt("REG_SECTION_TYPE_ID", dr), CON.RegistrationProviderStatusTypeId.Complete, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                                svc.SaveRegistrationSectionStatus(this.WorkflowPage.RegistrationId, 0, Helper.GetInt("REG_SECTION_TYPE_ID", dr), null, CON.RegistrationProviderServicesStatusTypeId.Approved, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                            }
                        }

                    }

                }


            }
        }
    }
    private void CheckforModifiedSections()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SECTION_STATUS");

        int modifiedCnt = 0;
        if (Helper.HasRows(ds))
        {
            DataTable dt = ds.Tables[0];

            divUpdateSections.InnerHtml = "<p>You have modified the following sections in your application. Click \"Ok\" to complete your submission. Click \"Cancel\" to review your application prior to submission.</p><p style=\"align:left;\">";
            foreach (DataRow dr in dt.Rows)
            {
                bool nodeExists = this.WorkflowPage.RegistrationNodes.ContainsKey(Helper.GetInt("REG_SECTION_TYPE_ID", dr));
                if (nodeExists)
                {

                    RegistrationNode appNode = this.WorkflowPage.RegistrationNodes[Helper.GetInt("REG_SECTION_TYPE_ID", dr)];
                    if (appNode != null)
                    {
                        if (Helper.GetInt("REG_PROVIDER_STATUS_TYPE_ID", dr) == CON.RegistrationProviderStatusTypeId.Modified && this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg)
                        {
                            divUpdateSections.InnerHtml = divUpdateSections.InnerHtml + "<br/>" + Helper.PascalCaseParse(Helper.GetString("DisplayName", dr));
                            modifiedCnt++;
                        }


                    }
                }
            }
            divUpdateSections.InnerHtml = divUpdateSections.InnerHtml + "</p>";

        }
    }

    protected void btnbackToSummary_Click(object sender, EventArgs e)
    {
        // TODO: EDV How the take the user back to Provider Summary??
        //Response.Redirect("~/Process/ProviderUpdateSummary.aspx");
        (this.Page as RegistrationProvider).RegistrationId = this.WorkflowPage.RegistrationId;
        (this.Page as RegistrationProvider).IsReadOnly = false;
    }

    private void SetTaxEntityType()
    {
        string taxEntityTypeId = Helper.GetAppSettingFromDB("TAX_ENTITY_TYPE_ID_Default");
        if (string.IsNullOrEmpty(taxEntityTypeId)) return;                              // No default, outahere

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
        if (!Helper.HasRows(ds)) return;                                                // No service location, outahere

        DataRow row = ds.Tables[0].Rows[0];
        if (!string.IsNullOrEmpty(Helper.GetString("TAX_ENTITY_TYPE_ID", row))) return; // Already set, outahere

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("REG_SERVICE_LOCATION_ID", Helper.GetString("REG_SERVICE_LOCATION_ID", row));
        parms.Add("TAX_ENTITY_TYPE_ID", taxEntityTypeId);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION", parms);
    }
    protected void btnGeneratePDF_Click(object sender, EventArgs e)
    {
        Guid logThreadId = Guid.NewGuid();
        string logMsg = String.Format("btnGeneratePDF_Click", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
        Logging log = new Logging(this.ThreadId, logMsg);
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            log.CreateLogEntry("Creating Object for PDMSServiceClient", Logging.LogPriority.Information);

            string fileName;
            // MH - As per Jon PDF document reference should not be saved. We shoudld give the user an option to save or view the PDf.
            log.CreateLogEntry("Calling GenerateApplication method", Logging.LogPriority.Information);

            bool result = psc.GenerateApplication(this.WorkflowPage.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), false, out fileName);
            log.CreateLogEntry("Calling DownloadFile method", Logging.LogPriority.Information);

            if (!string.IsNullOrEmpty(fileName))
            {
                DownloadFile(fileName, true); // TODO: Change later
                log.CreateLogEntry("Out from DownloadFile method", Logging.LogPriority.Information);
            }
            else
            {
                log.CreateLogEntry("Error while creating the file", Logging.LogPriority.Information);
            }

        }
    }
    protected void btnRtnGroup_Click(object sender, EventArgs e)
    {
        if (SessionVarRetriever.GroupUserRegID > 0)
        {
            SessionVarRetriever.IsGroup = true;
            Response.Redirect("~/process/Registration.aspx");
        }
    }

    private void DownloadFile(string fileName, bool isDiddReferral)
    {
        try
        {
            DownloadRequest downloadRequest = new DownloadRequest();
            downloadRequest.FileName = fileName;
            downloadRequest.IsDiddReferral = isDiddReferral;
            FileTransferServiceClient client = new FileTransferServiceClient();

            using (var fileStream = client.DownloadFile(downloadRequest).FileByteStream)
            {
                SendBinaryResponseToClient(fileStream, "attachment;filename=" + fileName, "application/pdf");
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Exception.aspx?Message=" + ex.Message);
        }
    }
    private void SendBinaryResponseToClient(Stream response, string contentHeader, string contentType)
    {
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();

        Response.Buffer = true;
        Response.ContentType = contentType;
        Response.AddHeader("Content-Disposition", contentHeader);
        response.CopyTo(Response.OutputStream);

        HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        // Technically, I should be doing Response.End(), but due to a bug in ASP.NET 
        // we tried doing comlete request http://support.microsoft.com/kb/312629/en-us
        // but complete request is putting all the page up there. So, we are swallowing the
        // thread abort exception here.
        // Response.End();
        try
        {
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
        }
        catch (Exception ex)
        {
            CoreException.ThrowException(ex);
        }
        finally
        {
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
    private void ShowPopup(BasePopupControl ctl, string title, string valGroup, int index, int viewIndex)
    {
        lblMpeTitle1.Text = title;
        //btnSave.ValidationGroup = valGroup;
        if (title == CON.RegistrationWorkflowActionName.ApproveProvider)
        {
            DataRow dr = null;
            if (index >= 0 && ctl.DataList.Rows.Count > 0) dr = ctl.DataList.Rows[index];
            ctl.LoadData(dr);

        }
        mltPopup.ActiveViewIndex = viewIndex;
        mpe1.Show();
    }

    #endregion
    #region Public Methods

    /// TODO: It MAY make sense to eliminate GetTaskParameter() and instead change GetStepParameter()
    /// to look for a parameter of type "key" associated to the current TaskID if it is unable to find
    /// a parameter of type "key" associated to the current StepID.  This would turn task parameters into
    /// default values of step paramaters.  If we do this, it may also make sense to change 
    /// SetStepParameter() so that you can specify to which step/task in the workflow to store the 
    /// key/value pair (instead of assuming it will be stored in the CURRENT StepID), as a way to pass a
    /// parameter from where the process is now, to the step that is  going to consume the parameter.
    /// 
    /// A better approach may be to have a new kind of parameter (called override task parameter) that
    /// stores both the ProcessID and TaskID in the WF_PARAMETER table.  With this, we could keep 
    /// GetStepParameter() as-is and instead adjust GetTaskParameter() to first look for overridden task
    /// parameters (ones that have the TaskID and the current ProcessID specified) before defaulting to
    /// the task parameters (that have null ProcessID).  Then, we would need to add a method called 
    /// SetTaskParameter(string TaskName, string key, string value) for setting override task parameters.
    /// Default task parameters (that have null ProcessID) would continue to have no Set() method, since
    /// these are defined at the time of workflow definition.
    /// 
    /// These changes could come in handy if, for example, the 4th task in a workflow NORMALLY sends out an
    /// email with the subject of "foo" and the template of "EMAIL_TEMPLATE_FOO" but something that the user
    /// does on the UI page at the 3rd task in the workflow causes a need to change the email subject to "bar"
    /// with the template of "EMAIL_TEMPLATE_BAR".

    public void SetWorkFlowActions(int stepID)
    {

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.WF_SelectStepActions(stepID);
        rptWorkflowActions.DataSource = ds.Tables[0];
        rptWorkflowActions.DataBind();
    }

    public void RefreshTree()
    {


        //ucRegTreeView.Refresh();
        ucRegProgressBar.Refresh();

    }

    public void SelectToolBarStep()
    {
        ucRegProgressBar.SelectStep();
    }

    public void SetBacktoSummaryPanelVisibility(bool visible)
    {
        pnlbackToSummary.Visible = visible;
    }

    public void SetProgressBarVisibility(bool visible)
    {
        DivProgressBar.Visible = visible;
    }

    public void SetWorkflowPanelVisibility(bool visible)
    {
        pnlWorkFlow.Visible = visible;
    }
    public void SetUpdateMessageVisibility(bool visible)
    {
        pnlUpdateMessage.Visible = false;
    }
    //Hide the workflow panel if no action buttons are found
    public void SetWorkflowPanelVisibility()
    {
        bool btnFound = false;

        foreach (RepeaterItem itm in rptWorkflowActions.Items)
        {
            Button btn = (Button)itm.FindControl("btnAction");
            if (btn != null)
            {
                btnFound = true;
                break;
            }
        }

        pnlWorkFlow.Visible = btnFound;
    }

    public void SetGeneratePDFPanelVisibility(bool visible)
    {
        pnlGeneratePDF.Visible = visible;
    }
    public void SetReturnToGroupPanelVisibility(bool visible)
    {
        divReturnToGroup.Visible = visible;
    }
    public void SetActionVisibility(string buttonText, bool visible)
    {
        foreach (RepeaterItem itm in rptWorkflowActions.Items)
        {
            Button btn = (Button)itm.FindControl("btnAction");
            if (btn != null)
            {
                if (btn.Text == buttonText) btn.Visible = visible;
            }
            // OHPNM-7124 removed code that was turning off the Approve button for Contract Maintenance page; it was disabling it on the non-page level
        }
    }

    public void AddActionConfirm(string buttonText, string confirmMessage)
    {
        foreach (RepeaterItem itm in rptWorkflowActions.Items)
        {
            Button btn = (Button)itm.FindControl("btnAction");
            if (btn != null)
            {
                if (btn.Text == buttonText) btn.Attributes.Add("onclick", "javascript:return confirm('" + confirmMessage + "');");
            }
        }
    }

    public void AddAdverseAction(DataRow row)
    {
        AddAdverseAction(row["DESCRIPTION"].ToString());
    }

    public void AddAdverseAction(string description)
    {
        if (this.lblAdverseActions.Text.Length > 0)
        {
            this.lblAdverseActions.Text += "<br />";
        }
        lblAdverseActions.Text += description;
    }

    public void LoadReturnReasons(int registrationId)
    {
        pnlReturnReasons.Controls.Clear();
        bool includePartyErrors = EntityTypeID != CON.ProviderCategoryTypeID.Individual;      // Do not include for Individuals
        int pageTypeId = Registration.GetPageIDFromSectionID(this.WorkflowPage.RegistrationStep);
        DataSet ds = svc.SelectREG_ERRORcustom(registrationId, pageTypeId, this.WorkflowPage.RegistrationStep, false, includePartyErrors);
        //reg_notes
        int regPageTypeId = 0;
        bool isProvider = Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name);

        if (ds.Tables.Count > 0)
        {
            foreach (DataRow row in ds.Tables[0].Rows) AddError(row);
            int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
            if (RegPageTypeId == 0)
                RegPageTypeId = this.WorkflowPage.RegistrationStep;
            DataSet dsNotes1 = svc.SelectRegNotes(registrationId, RegPageTypeId);
            //retrieve the reject reasons based on section type and if it a provider screening or other screening then just retrieve based on page level
            DataRow[] dr1;

            if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.ProviderScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OwnerScreening ||
                Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.SiteVisitScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.BackgroundCheck ||
                Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OrientationInformation)
            {
                dr1 = dsNotes1.Tables[0].Select("PROVIDER_NOTE_TYPE_ID ='" + CON.ProviderNoteTypeId.RegistrationRejection + "'");
            }
            else
            {
                dr1 = dsNotes1.Tables[0].Select("PROVIDER_NOTE_TYPE_ID ='" + CON.ProviderNoteTypeId.RegistrationRejection + "' and (REG_SECTION_TYPE_ID = " + this.WorkflowPage.RegistrationStep
                    + "OR REG_SECTION_TYPE_ID = 99999)"); //added or reg_section_type_id = 99999 to handle cleanup script for cr-32
            }
            foreach (DataRow r1 in dr1)
            {
                Label lbl = new Label();
                lbl.CssClass = "failureNotification";
                lbl.Text = "- ";
                pnlReturnReasons.Controls.Add(lbl);
                AddReturnToScreeningError(r1);
            }

            if (ds.Tables.Count > 1)
            {
                foreach (DataRow row in ds.Tables[1].Rows) AddError(row);
            }
            pnlReturnReasons.Controls.Add(new LiteralControl("<br />"));

        }


        DataSet dsNotes = svc.SelectRegNotes(registrationId, regPageTypeId);
        if (dsNotes.Tables.Count > 0)
        {
            //using reflection to loop through all the constants in the ProviderNoteTypeId class
            foreach (System.Reflection.FieldInfo noteType in typeof(CON.ProviderNoteTypeId).GetFields().Where(x => x.IsStatic && x.IsLiteral))
            {
                if ((!isProvider && (noteType.Name != "Internal" && noteType.Name != "RegistrationRejection" && noteType.Name != "Disenrollment")) || (isProvider && (noteType.Name == "NoteOnApproved")))
                {
                    DataRow[] dr = dsNotes.Tables[0].Select("PROVIDER_NOTE_TYPE_ID ='" + noteType.GetValue(null) + "'"); //CON.ProviderNoteTypeId.ReturnToScreening or ReturnToSiteVisit

                    if (dr != null && dr.Count() > 0)
                    {
                        Label lbl = new Label() { CssClass = "failureNotification" };
                        string desc = "";
                        DescriptionAttribute[] attributes = (DescriptionAttribute[])noteType.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attributes.Length > 0)
                            desc = attributes[0].Description;  //using reflection to get the description attribute in the ProviderNoteTypeId class
                        else
                            desc = noteType.Name;
                        lbl.Text = desc + " Reasons:"; //this is the description written on the CON.ProviderNoteTypeId class
                        pnlReturnReasons.Controls.Add(lbl);
                        pnlReturnReasons.Controls.Add(new LiteralControl("<br />"));
                        foreach (DataRow r1 in dr)
                        {
                            AddReturnToScreeningError(r1);
                        }
                        pnlReturnReasons.Controls.Add(new LiteralControl("<br />"));
                    }
                }
            }
        }

    }

    public void LoadAdverseActions(int regID)
    {
        DataSet ds = svc.SelectScreeningAdverseActions(regID);
        this.lblAdverseActions.Text = string.Empty;

        if (ds.Tables.Count > 0)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                AddAdverseAction(row);
            }
        }
    }

    public void LoadProviderInfoHeader()
    {
        int regId = this.WorkflowPage.RegistrationIdSelected == 0 ? this.WorkflowPage.RegistrationId : this.WorkflowPage.RegistrationIdSelected;
        bool getFromDB = false;
        string wfName = string.Empty;

        bool isProvider = Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name);

        // changed to always visible as part of DCPDMS-2187 - CR 32: Header not displaying all of the provider data 
        //trMedicaid.Style["display"] = 
        //trStateStatus.Style["display"] =
        trRiskLevel.Style["display"] =
        trPDMSStatusDate.Style["display"] = "block";

        //trProviderStatus.Style["display"] = isProvider ? "none" : "block";

        //lblAppStatusLabel.Visible = lblApplicationStatus.Visible = isProvider;

        // changed to always visible as part of DCPDMS-2187 - CR 32: Header not displaying all of the provider data 
        lblMediaidID.Visible = lbl4MedicaidID.Visible = true;
        lblApplicationType.Text = string.Empty;
        if (!string.IsNullOrEmpty(getFromDB ? wfName : WorkflowName)) lblApplicationType.Text = (getFromDB ? wfName : WorkflowName).Replace("Registration - ", string.Empty);

        DataRow row = null;
        if (this.WorkflowPage.RegistrationId > 0)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("UserID", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectRegistrationHeader", parms);
            if (Helper.HasRows(ds))
            {
                row = ds.Tables[0].Rows[0];
                LoadDetails(row);
            }

            LoadReturnReasons(this.WorkflowPage.RegistrationId);
        }

        if (this.WorkflowPage.RegistrationId == 0)
        {
            lblAssignedTo.Visible = ddlAssignedTo.Visible = false;
            lnkRiskLevel.Visible = false;
            return;
        }
        else
        {
            //if in maintainence
            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.Administrator) && (this.WorkflowPage.WF_StepID <= 0 && ((Helper.GetData("TennCareStatusID", row) == CON.RegistrationProgramStatusTypeId.Maintenance.ToString() || (string.IsNullOrEmpty(this.TaskName) && Helper.GetData("TennCareStatusID", row) == CON.RegistrationProgramStatusTypeId.Conversion.ToString())))))
            {
                lnkRiskLevel.Text = lblRiskLevel.Text;
                lnkRiskLevel.Visible = true;
                lblRiskLevel.Visible = false;
            }
            else
            {
                lnkRiskLevel.Visible = false;
                lblRiskLevel.Visible = true;
            }
        }

        if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
        {
            this.trAssignedTo.Visible = false;
            this.trAssignedTo1.Visible = false;
        }



    }
    #endregion

    #region Private Methods

    private void LoadDetails(DataRow row)
    {
        bool isProvider = Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name);

        int RegID = Convert.ToInt32(row[0]);

        string strReasonCodeId = SetupEnrolStatus(RegID);

        //SetupReasonCode(strReasonCodeId);
        //ddlReasonCode.Enabled = true; //  lblEnrolStatusValue.Text == "ACTIVE";  JIRA 2858

        this.ProviderCategoryTypeID = Helper.ConvertStrNullToInt32(row["PROVIDER_CATEGORY_TYPE_ID"]);
        this.ApplicationFeeRequired = Helper.GetBool("IS_FEE_REQUIRED", row);

        lblProviderName.Text = Helper.GetString("PROVIDER_NAME", row);
        lblMediaidID.Text = Helper.GetString("MEDICAID_ID", row);
        //lblTennCareStatus.Text = Helper.GetData("TennCareStatus", row);
        //lblPDMSStatus.Text = !isProvider ? Helper.GetData("PDMSStatus", row) : string.Empty;
        //lblPDMSStatusDate.Text = (Helper.GetDateTime("TennCareStatusDate", row)).ToString("MM/dd/yyyy");
        //lblApplicationStatus.Text = isProvider ? Helper.GetData("PDMSStatus", row) : string.Empty;
        lblApplicationStatus.Text = Helper.GetData("PDMSStatus", row);
        lblRiskLevel.Text = Helper.GetString("PROVIDER_RISK_LEVEL_NAME", row);
        lblProviderType.Text = Helper.GetString("PROVIDER_TYPE_NAME", row);
        lblApplicationType.Text = Helper.GetString("APPLICATION_TYPE_NAME", row);
        lblEnrollmentStatus.Text = Helper.GetString("workflow_event_type_name", row);
        lblBumpUpReason.Text = Helper.GetString("BUMP_UP_REASON_NAME", row);
        lblNPI.Text = Helper.GetString("NPI", row);
        lblEffectiveDate.Text = Helper.GetDate("EffectiveDate", row);
        lblRevalidationDate.Text = Helper.GetDate("REVALIDATION_DATE", row);
        trTimesApplicationReturned.Visible = false;
        rowlblBuildingMedID.Visible = false;
        trLastDateOfDenialOrTermination.Visible = false;
        if (Helper.GetBool("IS_INCIDENT_ALERT_SET", row))
        {
            lblIncidentAlert.Visible = true;
            lblIncidentAlert.Text = "Pending Incident Compliance Review";
        }
        else
            lblIncidentAlert.Visible = false;
        if (!string.IsNullOrEmpty(Helper.GetString("Term_Date", row)))
        {
            lblDateOfLastDenial.Text = (Helper.GetDateTime("Term_Date", row)).ToString("MM/dd/yyyy");
            if (lblEnrollmentStatus.Text != "New")
            {
                lblLastDateOfDenial.Text = "Date of Last Termination";
            }
            trLastDateOfDenialOrTermination.Visible = true;
        }

        if ((Helper.IsUserInLTCWorkerRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) || Helper.IsUserInStateReviewerRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name) || Helper.IsLoggedInUserInAdminRole()) &&
              (Helper.GetString("APPLICATION_TYPE_NAME", row) == CON.ApplicationTypeName.ADHP || Helper.GetString("APPLICATION_TYPE_NAME", row) == CON.ApplicationTypeName.EPDWaiver))
        {
            trTimesApplicationReturned.Visible = true;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectTimesApplicationReturnedToProviderFromLTC(this.WorkflowPage.RegistrationId);
            if (Helper.HasRows(ds))
            {
                lblTimesApplicationReturnedNumber.Text = ds.Tables[0].Rows[0]["cnt"].ToString();
            }
        }

        if ((Helper.IsUserInAdminRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInLTCCHOPRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInProviderServicesRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInReadOnlyRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInLTCInitialRevalidationRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInInternalApplicationsEntryRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name) || Helper.IsUserInTechAdminRole(HttpContext.Current.User.Identity.Name))
            && (Helper.GetInt("PROVIDER_TYPE_ID", row) == 85 || Helper.GetInt("PROVIDER_TYPE_ID", row) == 86 || Helper.GetInt("PROVIDER_TYPE_ID", row) == 87 || Helper.GetInt("PROVIDER_TYPE_ID", row) == 88 || Helper.GetInt("PROVIDER_TYPE_ID", row) == 89 || Helper.GetString("PROVIDER_TYPE_ID", row).ToUpper() == "FF"))
        {
            rowlblBuildingMedID.Visible = true;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectBuildingMedicaidIDByRegID(this.WorkflowPage.RegistrationId);
            if (Helper.HasRows(ds))
            {
                lblBuildingMedID.Text = ds.Tables[0].Rows[0]["BUILDING_MEDICAID_ID"].ToString();
            }
        }

        SetNotesDisplay();

        if (!IsPostBack && !Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
        {
            SetupAssignedTo();
        }

        if (!string.IsNullOrEmpty(lblApplicationStatus.Text) && lblApplicationStatus.Text.ToLower().Contains("provider data entry"))
        {
            ddlAssignedTo.Items.Clear();
        }
    }

    private void SetNotesDisplay()
    {
        btnNotes.Visible = false;
        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInDBHOperatorRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInDBHReviewerRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInLTCWorkerRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInStateReviewerRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInDDSReviewerRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInAppealSpecialist(HttpContext.Current.User.Identity.Name)
            || Helper.IsLoggedInUserInCredentialingQualityAssuranceRole()
            || Helper.IsLoggedInUserInPnmSuperUserRole()
            )
        {
            btnNotes.Visible = true;
            btnNotes.Enabled = false;
            if (this.WorkflowPage.RegistrationId > 0) btnNotes.Enabled = true;

        }

    }

    private string GetGroupNames()
    {
        StringBuilder sb = new StringBuilder();

        DataSet ds = svc.GetGroupNames(this.WorkflowPage.RegistrationId);
        if (Helper.HasRows(ds))
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                sb.Append(row["GROUP_NAME"].ToString() + ",");
            }
        }

        string toReturn = sb.ToString();
        toReturn = toReturn.Length > 0 ? toReturn.Remove(sb.Length - 1) : string.Empty;

        return sb.ToString();
    }

    private void SetupAssignedTo()
    {
        string username = string.Empty;


        if (!string.IsNullOrEmpty(StepOwner))
        {
            MembershipUser user = Membership.GetUser(new Guid(StepOwner));
            if (user != null) username = user.UserName;
        }
        if ((Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.Administrator) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.EnrollmentSpecialist)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ComplianceSpecialist) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.TechAdminMAX)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCInitialReval) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.APMSpecialist))
            && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.ProviderDataEntry)
        {
            ddlAssignedTo.Enabled = true;

            string roleList = string.IsNullOrEmpty(GroupNameList) ? GetGroupNames() : GroupNameList;
            // ProviderServices and Operator are the same role
            roleList = roleList.Replace("providerservices", "providerservices,operator");

            string[] roles = roleList.Split(',');

            DataTable dtRoles = null;
            DataSet ds = null;

            foreach (string role in roles.Distinct())
            {
                if (!role.Trim().Equals(CON.ProviderAdministratorRole))
                {
                    ds = svc.SelectUsersInRoles(role.Trim());
                }
                else
                {
                    ds = svc.SelectUsersInProviderAdminRole(role.Trim(), this.WorkflowPage.RegistrationId);
                }


                if (Helper.HasRows(ds))
                {
                    if (dtRoles == null)
                    {
                        dtRoles = ds.Tables[0].Copy();
                    }
                    else
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            DataRow r = dtRoles.NewRow();
                            r["UserName"] = row["UserName"].ToString();
                            r["UserId"] = row["UserId"].ToString();
                            dtRoles.Rows.Add(r);
                        }
                    }
                }
            }

            if (dtRoles != null)
            {
                var defaultView = dtRoles.DefaultView;
                defaultView.Sort = "UserName asc";
            }

            if (Helper.HasRows(dtRoles) && ddlAssignedTo.Items.Count == 0)
            {
                ddlAssignedTo.DataValueField = "UserId";
                ddlAssignedTo.DataTextField = "UserName";
                ddlAssignedTo.DataSource = dtRoles;
                ddlAssignedTo.DataBind();
                ddlAssignedTo.Items.Insert(0, Helper.IsUserInProviderRoles(username) ? "N/A" : string.Empty);
            }

            // Operator can do streamlined update for provider. If this is the current case, add the operator to the list
            if (Helper.IsUserInOperatorRolls(username) && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry)
            {
                ddlAssignedTo.Items.Insert(0, username);
                ddlAssignedTo.SelectedIndex = 0;
            }

            //if the current task belongs to a provider, set assigned to provider name and disable
            if (Array.IndexOf(roles, CON.UserRoleType.ProviderAdministrator.ToLower()) >= 0 || Array.IndexOf(roles, CON.UserRoleType.ProviderOper.ToLower()) >= 0)
            {
                int regId = this.WorkflowPage.RegistrationIdSelected;
                DataSet dsXref = svc.SelectRegistrationXREF(regId);
                if (Helper.HasRows(dsXref))
                {
                    ddlAssignedTo.SelectedValue = Helper.GetString("UserId", dsXref.Tables[0].Rows[0]);
                }

                ddlAssignedTo.Enabled = false;
            }

            if (ddlAssignedTo.SelectedIndex <= 0)
            {
                if (string.IsNullOrEmpty(StepOwner))
                {
                    ddlAssignedTo.SelectedIndex = 0;
                }
                else
                {
                    ddlAssignedTo.SelectedValue = StepOwner;
                }
            }
        }
        else
        {
            ddlAssignedTo.Enabled = false;
            ddlAssignedTo.Items.Insert(0, "N/A");
            ddlAssignedTo.SelectedIndex = 0;
            trAssignedTo.Visible = false;
            trAssignedTo1.Visible = false;
        }
    }


    //private void SetupReasonCode(string reasonCodeId)
    //{

    //    DataSet ds = svc.SelectEnrollmentStatusReasons();
    //    DataView dv = ds.Tables[0].DefaultView;
    //    dv.RowFilter = "ENROLLMENT_STATUS_REASONS_ID in  (3,49,67,68,27,28," + reasonCodeId + ")";
    //    DataTable dtRoles = dv.ToTable();

    //    if (Helper.HasRows(dtRoles))
    //    {
    //        Helper.LoadDropDown(ddlReasonCode, dtRoles, "ENROLLMENT_STATUS_REASONS_DESC", "ENROLLMENT_STATUS_REASONS_ID", true);
    //        ddlReasonCode.SelectedValue = reasonCodeId;
    //    }

    //}

    private string SetupEnrolStatus(int regId)
    {

        DataSet ds = new DataSet();
        List<SqlParameter> parms = new List<SqlParameter>();
        string strReasonCodeId = string.Empty;

        parms.Add(new SqlParameter("REG_ID", regId));
        ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parms, "Parameters");

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            lblEnrolStatusValue.Text = Convert.ToString(row["ENROLL_STATUS_DESC"]).TrimEnd();
            lblEnrollmentStatusReasonDesc.Text = Convert.ToString(row["ENROLLMENT_STATUS_REASON"]).TrimEnd();

            strReasonCodeId = Convert.ToString(row["ENROLLMENT_STATUS_REASON_ID"]);
            //if (strReasonCodeId != string.Empty && strReasonCodeId != null) {
            //    ddlReasonCode.SelectedValue = strReasonCodeId;
            //};
            if (string.IsNullOrEmpty(lblEnrolStatusValue.Text))
            {
                switch (Helper.GetInt("ENROLLMENT_STATUS_CODE", row))
                {
                    case 1:
                        lblEnrolStatusValue.Text = CON.EnrollStatusCodeDesc.ACTIVE;
                        break;
                    case 2:
                        lblEnrolStatusValue.Text = CON.EnrollStatusCodeDesc.INACTIVE;
                        break;
                    case 3:
                        lblEnrolStatusValue.Text = CON.EnrollStatusCodeDesc.REPORTINGONLY;
                        break;
                }
            }
        }
        else
        {
            lblEnrolStatusValue.Text = string.Empty;
        }

        return strReasonCodeId;
    }


    private void SetupTNFiles()
    {
        //Bug CSS CleanUp
        //string prefix = "http://";        
    }

    private void AddScreenErrorHeading(string errMsg)
    {
        upPnlErrors.Visible = true;

        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);


        //pnlScreenErrors.Visible = true;

        //Label lbl = new Label();
        //lbl.CssClass = "failureNotification";
        //lbl.Text = errMsg;
        //pnlScreenErrors.Controls.Add(lbl);
        //pnlScreenErrors.Controls.Add(new LiteralControl("<br />"));

    }
    private void AddScreenError(string errMsg, ref bool isGood)
    {
        //DivErrors.Visible = true;

        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);


        /*pnlScreenErrors.Visible = true;

        Label lbl = new Label();
        lbl.CssClass = "failureNotification";
        lbl.Text = errMsg;
        pnlScreenErrors.Controls.Add(lbl);
        pnlScreenErrors.Controls.Add(new LiteralControl("<br />"));*/
        isGood = false;
    }
    private void AddError(DataRow row)
    {
        upPnlErrors.Visible = true;
        pnlReturnReasons.Visible = true;
        Label lbl = new Label();
        lbl.CssClass = "failureNotification";
        lbl.Text = row["ERROR_NAME"].ToString() + " (" + row["ERROR_CODE"].ToString() + ")";
        pnlReturnReasons.Controls.Add(lbl);
        pnlReturnReasons.Controls.Add(new LiteralControl("<br />"));
    }

    private void AddReturnToScreeningError(DataRow row)
    {
        upPnlErrors.Visible = true;
        pnlReturnReasons.Visible = false;
        Label lbl = new Label();
        lbl.CssClass = "failureNotification";
        lbl.Text = row["NOTE_TEXT"].ToString();
        pnlReturnReasons.Controls.Add(lbl);
        pnlReturnReasons.Controls.Add(new LiteralControl("<br />"));
    }
    private void SetMenuItemText(MenuItemCollection col, string itemText, string newItemText)
    {
        foreach (MenuItem itm in col)
        {
            if (itm.ChildItems.Count > 0) SetMenuItemText(itm.ChildItems, itemText, newItemText);
            if (itm.Value == itemText)
            {
                itm.Text = newItemText;
                return;
            }
        }
    }

    private void RemoveMenuItem(MenuItemCollection col, string value)
    {
        foreach (MenuItem itm in col)
        {
            if (itm.ChildItems.Count > 0) RemoveMenuItem(itm.ChildItems, value);
            if (itm.Value == value)
            {
                col.Remove(itm);
                return;
            }
        }
    }

    private void GetTaskParameters()
    {
        DataSet ds = new DataSet();
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("TaskID", DbType.Int32, TaskID, true));
        ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectTaskParameters", parameters, "Parameters");

        TaskParameter = new Dictionary<string, string>();
        if (ds.Tables.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TaskParameter.Add(dr["PARAMETER_NAME"].ToString(), dr["PARAMETER_VALUE"].ToString());
            }
        }
    }



    private void SaveStepParameters()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        foreach (KeyValuePair<string, string> parm in StepParameter)
        {
            psc.WF_SaveStepParameter(StepID, parm.Key, parm.Value);
        }
    }

    private void NextStep(string action)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.WF_TakeAction(ProcessID, action, string.Empty);
    }

    private int CurrentStep(int regId)
    {
        int rtn = 0;
        using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
        {
            DataSet ds = svc.GetWFProcessByRegId(regId);
            if (Helper.HasRows(ds)) rtn = Helper.GetInt("CURRENT_STEP_ID", ds.Tables[0].Rows[0]);
        }
        return rtn;
    }

    #endregion
    protected void btnSaveApproveProvider_Click(object sender, EventArgs e)
    {
        if (mltPopup.ActiveViewIndex == 0)
        {
            ucApproveProvider.SaveData();
            if (ucApproveProvider.Page.IsValid)
            {
                mpe1.Hide();
                if ((MainContent.Page as WorkflowPage).OnTaskExit("Approve Provider"))
                {

                    if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.SiteVisitScreening)
                    {
                        if (this.WorkflowPage.WF_StepID != 0)
                        {
                            svc.updateWF_STEP_Owner(this.WorkflowPage.WF_StepID, null);
                        }
                    }
                    //SaveProcessParameters();
                    //SaveStepParameters();
                    NextStep("Approve Provider");
                    if (this.WorkflowPage.WF_StepID == CurrentStep(this.WorkflowPage.RegistrationId))
                        Response.Redirect("~/Exception.aspx?Message=" + "Step did not advance, RegistrationId: " +
                            this.WorkflowPage.RegistrationId.ToString() + ". Please contact Provider Services");
                    else
                    {
                        if (!string.IsNullOrEmpty(RedirectURL)) Response.Redirect(RedirectURL);     // Redirect to where needed
                        else Response.Redirect("~/Default.aspx");                                   // Default go Home
                    }
                }
            }
            else
            {
                mpe1.Show();
            }
        }
        else if (mltPopup.ActiveViewIndex == 1)
        {

            ucReturnToScreening.SaveData(lblMpeTitle1.Text);
            if (ucReturnToScreening.Page.IsValid)
            {
                mpe1.Hide();

                if (lblMpeTitle1.Text == "Return To Screening")
                {
                    if ((MainContent.Page as WorkflowPage).OnTaskExit("Return To Screening"))
                    {

                        //SaveProcessParameters();
                        //SaveStepParameters();
                        NextStep("Return To Screening");
                        if (this.WorkflowPage.WF_StepID == CurrentStep(this.WorkflowPage.RegistrationId))
                            Response.Redirect("~/Exception.aspx?Message=" + "Step did not advance, RegistrationId: " +
                           this.WorkflowPage.RegistrationId.ToString() + ". Please contact Provider Services");
                        else
                        {
                            if (!string.IsNullOrEmpty(RedirectURL)) Response.Redirect(RedirectURL);     // Redirect to where needed
                            else Response.Redirect("~/Default.aspx");                                   // Default go Home
                        }
                    }
                }
                else if (lblMpeTitle1.Text == "Return To Site Visit")
                {
                    if ((MainContent.Page as WorkflowPage).OnTaskExit("Return To Site Visit"))
                    {

                        //SaveProcessParameters();
                        //SaveStepParameters();
                        NextStep("Return To Site Visit");
                        if (this.WorkflowPage.WF_StepID == CurrentStep(this.WorkflowPage.RegistrationId))
                            Response.Redirect("~/Exception.aspx?Message=" + "Step did not advance, RegistrationId: " +
                                this.WorkflowPage.RegistrationId.ToString() + ". Please contact Provider Services");
                        else
                        {
                            if (!string.IsNullOrEmpty(RedirectURL)) Response.Redirect(RedirectURL);     // Redirect to where needed
                            else Response.Redirect("~/Default.aspx");                                   // Default go Home
                        }
                    }
                }
                else if (lblMpeTitle1.Text == "Return To Provider Review")
                {
                    if ((MainContent.Page as WorkflowPage).OnTaskExit("Return To Provider Review"))
                    {

                        //SaveProcessParameters();
                        //SaveStepParameters();
                        NextStep("Return To Provider Review");
                        if (this.WorkflowPage.WF_StepID == CurrentStep(this.WorkflowPage.RegistrationId))
                            Response.Redirect("~/Exception.aspx?Message=" + "Step did not advance, RegistrationId: " +
                                this.WorkflowPage.RegistrationId.ToString() + ". Please contact Provider Services");
                        else
                        {
                            if (!string.IsNullOrEmpty(RedirectURL)) Response.Redirect(RedirectURL);     // Redirect to where needed
                            else Response.Redirect("~/Default.aspx");                                   // Default go Home
                        }
                    }
                }
            }
            else
            {
                mpe1.Show();
            }
        }
        else if (mltPopup.ActiveViewIndex == 2)
        {
            ucProviderRiskLevel.SaveData();
            if (ucProviderRiskLevel.isSaved)
            {
                mpe1.Hide();
                string url = "~/Process/Registration.aspx?Step=" + this.WorkflowPage.RegistrationStep.ToString() + "&RegId=" + this.WorkflowPage.RegistrationId.ToString();
                Response.Redirect(url);
            }
            else
            {
                mpe1.Show();
                return;
            }
        }
        else if (mltPopup.ActiveViewIndex == 3)
        {
            ucLTProviderEnrollment.SaveData();
            if (ucLTProviderEnrollment.Page.IsValid)
            {
                mpe1.Hide();
                if ((MainContent.Page as WorkflowPage).OnTaskExit("Create LT Enrollment"))
                {

                    NextStep("Create LT Enrollment");
                    if (this.WorkflowPage.WF_StepID == CurrentStep(this.WorkflowPage.RegistrationId))
                        Response.Redirect("~/Exception.aspx?Message=" + "Step did not advance, RegistrationId: " +
                            this.WorkflowPage.RegistrationId.ToString() + ". Please contact Provider Services");
                    else
                    {
                        if (!string.IsNullOrEmpty(RedirectURL)) Response.Redirect(RedirectURL);     // Redirect to where needed
                        else Response.Redirect("~/Default.aspx");                                   // Default go Home
                    }
                }
            }
            else
            {
                mpe1.Show();
                return;
            }
        }
        else if (mltPopup.ActiveViewIndex == 4)
        {
            ucCredentialRTP.SaveData();
            if (ucCredentialRTP.Page.IsValid)
            {
                mpe1.Hide();
                if ((MainContent.Page as WorkflowPage).OnTaskExit("Return to Provider"))
                {

                    NextStep("Return to Provider");
                    if (this.WorkflowPage.WF_StepID == CurrentStep(this.WorkflowPage.RegistrationId))
                        Response.Redirect("~/Exception.aspx?Message=" + "Step did not advance, RegistrationId: " +
                            this.WorkflowPage.RegistrationId.ToString() + ". Please contact Provider Services");
                    else
                    {
                        if (!string.IsNullOrEmpty(RedirectURL)) Response.Redirect(RedirectURL);     // Redirect to where needed
                        else Response.Redirect("~/Default.aspx");                                   // Default go Home
                    }
                }
            }
            else
            {
                mpe1.Show();
                return;
            }
        }
        else if (mltPopup.ActiveViewIndex == 5)
        {
            if (!string.IsNullOrWhiteSpace(txtNotProcessedComments.Text))
            {
                PDMSService.PDMSServiceClient pscService = new PDMSService.PDMSServiceClient();
                int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                pscService.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep, CON.ProviderNoteTypeId.Internal, txtNotProcessedComments.Text.Trim(),
                DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                //Check if 'ProcessDiscontinue' on the latest Credential and Update the date to today on it.
                pscService.UpdateCredentialDiscontinueDate(this.WorkflowPage.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                pscService.UpdateRegistrationCustom
                    (new Dictionary<string, string>() { { "REG_ID",this.WorkflowPage.RegistrationId.ToString() },
                    { "REGISTRATION_STATUS_TYPE_ID", CON.RegistrationStatusTypeId.NotProcessed.ToString() },
                    { "REG_PROGRAM_STATUS_TYPE_ID", CON.RegistrationProgramStatusTypeId.NotProcessed.ToString () } });

                // Update PNM Application status to denied.
                pscService.UpdatePNMApplicationStatus(this.WorkflowPage.RegistrationId, CON.PSMApplicationStatusID.DENIED, Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.WorkflowPage.WF_ProcessID);

                pscService.WF_SaveProcessParameter(this.WorkflowPage.WF_ProcessID, CON.ProcessParameter.RegProgramStatusTypeID, CON.RegistrationProgramStatusTypeId.NotProcessed.ToString());

                DataSet dsEnrollment = RegistrationController.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ENROLLMENT");
                if (!ObjectControllerHelper.HasRows(dsEnrollment))
                {
                    DateTime dt = Convert.ToDateTime("12/31/2299");
                    Dictionary<string, string> parmsenroll = new Dictionary<string, string>();
                    parmsenroll.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                    parmsenroll.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parmsenroll.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parmsenroll.Add("CREATE_DATE_TIME", DateTime.Now.ToString());
                    parmsenroll.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parmsenroll.Add("ENROLL_END_DATE_TIME", dt.ToString());
                    parmsenroll.Add("ENROLLMENT_STATUS_CODE", CON.EnrollmentStatusTypeID.InActive.ToString());
                    parmsenroll.Add("ENROLL_STATUS_REASON_ID", CON.EnrollStatusReasonID.InActive.ToString());
                    parmsenroll.Add("ENROLL_START_DATE_TIME", DateTime.Now.ToString());

                    RegistrationController.InsertRegistrationData("ENROLLMENT", parmsenroll);
                }

                //PRGCR313
                if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.ChangeProviderType)
                {
                    RegistrationController.UpdateProviderTypeChangeStatus(this.WorkflowPage.RegistrationId, CON.ProviderTypeChangeRequestStatus.Cancelled, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }

                pscService.WF_TakeAction(this.WorkflowPage.WF_ProcessID, "Not Processed", string.Empty);
                txtNotProcessedComments.Text = string.Empty;
                mpe1.Hide();
                Response.Redirect("~/Default.aspx");
            }

        }
        // SAM763
        else if (mltPopup.ActiveViewIndex == 6)
        {
            ucComplReason.SaveData();
            if (ucComplReason.Page.IsValid)
            {
                mpe1.Hide();
                if ((MainContent.Page as WorkflowPage).OnTaskExit("Refer To Compliance"))
                {

                    NextStep("Refer To Compliance");
                    if (this.WorkflowPage.WF_StepID == CurrentStep(this.WorkflowPage.RegistrationId))
                        Response.Redirect("~/Exception.aspx?Message=" + "Step did not advance, RegistrationId: " +
                            this.WorkflowPage.RegistrationId.ToString() + ". Please contact Provider Services");
                    else
                    {
                        if (!string.IsNullOrEmpty(RedirectURL)) Response.Redirect(RedirectURL);     // Redirect to where needed
                        else Response.Redirect("~/Default.aspx");                                   // Default go Home
                    }
                }
            }
            else
            {
                mpe1.Show();
                return;
            }
        }
        else
        {
            mpe1.Show();
            return;
        }

    }

    protected void ddlRiskLevel_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlRiskLevel_Click(object sender, EventArgs e)
    {
        ucProviderRiskLevel.LoadControls();
        ShowPopup(ucProviderRiskLevel, "Set Provider Risk Level", "", 0, 2);
    }

    protected void btnNotes_Click(object sender, EventArgs e)
    {
        //ucNotes.CurrentPageIndex = 0;
        //ucNotes.EnableViewState = true;
        //if (this.WorkflowPage.RegistrationId > 0)
        //{
        //    ucNotes.LoadNotes(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);
        //}
        //ucNotes.LoadData();
        //mpeNotesGrid.Show();

        ucNotes.RefreshData();
        mpeNotesGrid.EnableViewState = true;
        mpeNotesGrid.Show();
    }

    private bool ValidateUploadDocuments()
    {
        bool rtn = true;
        DataSet ds = null;
        DataSet dsVisibleDocs = null;
        Upload upload = new Upload();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataRow row = Registration.GetRegistration(this.WorkflowPage.RegistrationId);

        if (Registration.HasPrimaryPracticeLocationChange(this.WorkflowPage.RegistrationId))
        {
            //validation for Facility or Business License            
            if (row != null)
            {
                ds = psc.SelectRegSectionUploadDocument(CON.RegistrationPageType.Agreements, this.WorkflowPage.RegistrationId, Helper.GetInt("PROVIDER_TYPE_ID", row), "OtherDocuments", 0);

                dsVisibleDocs = psc.SelectRegSectionUploadControl(CON.RegistrationPageType.Agreements, Helper.GetInt("APPLICATION_TYPE_ID", row), Helper.GetInt("PROVIDER_TYPE_ID", row), Helper.GetInt("ENTITY_TYPE_ID", row), "OtherDocuments", WorkflowPage.RegistrationId);

                //ds = upload.LoadUserSectionControl(CON.RegistrationPageType.Agreements,"OtherDocuments");
                if (!DocIsUploaded(ds, "Facility or Business License", "OtherDocuments") && IsDocumentVisible(dsVisibleDocs, "Facility or Business License"))
                {
                    rtn = false;
                    AddUploadDocumentError("Please Upload File: Facility or Business License");
                }
                //validation for Certificate of Occupancy or ( Lease)      
                ds = psc.SelectRegSectionUploadDocument(CON.RegistrationPageType.LicensesClassifications, this.WorkflowPage.RegistrationId, Helper.GetInt("PROVIDER_TYPE_ID", row), null, 0);
                dsVisibleDocs = psc.SelectRegSectionUploadControl(CON.RegistrationPageType.LicensesClassifications, Helper.GetInt("APPLICATION_TYPE_ID", row), Helper.GetInt("PROVIDER_TYPE_ID", row), Helper.GetInt("ENTITY_TYPE_ID", row), null, WorkflowPage.RegistrationId);
                //ds = upload.LoadUserSectionControl(CON.RegistrationPageType.LicensesClassifications);
                if (!DocIsUploaded(ds, "Certificate of Occupancy or ( Lease)") && IsDocumentVisible(dsVisibleDocs, "Certificate of Occupancy or ( Lease)"))
                {
                    rtn = false;
                    AddUploadDocumentError("Please Upload File: Certificate of Occupancy or ( Lease)");
                }
                //validation for Proof of Liability Insurance  

                DataSet ds1 = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "INSURANCE");
                //if (Helper.HasRows(ds1))
                //{
                //    int insuranceID = Helper.GetInt("REG_INSURANCE_ID", ds1.Tables[0].Rows[0]);
                //    ds = psc.SelectRegSectionUploadDocument(CON.RegistrationPageType.LicensesClassifications, this.WorkflowPage.RegistrationId, Helper.GetInt("PROVIDER_TYPE_ID", row), "Insurance", insuranceID);
                //    dsVisibleDocs = psc.SelectRegSectionUploadControl(CON.RegistrationPageType.LicensesClassifications, Helper.GetInt("APPLICATION_TYPE_ID", row), Helper.GetInt("PROVIDER_TYPE_ID", row), Helper.GetInt("ENTITY_TYPE_ID", row), "Insurance", WorkflowPage.RegistrationId);
                //    //ds = upload.LoadUserSectionControl(CON.RegistrationPageType.LicensesClassifications, "Insurance");
                //    if (!DocIsUploaded(ds, "Proof of Liability Insurance ", "Insurance") && IsDocumentVisible(dsVisibleDocs, "Proof of Liability Insurance "))
                //    {
                //        rtn = false;
                //        AddUploadDocumentError("Please Upload File: Proof of Liability Insurance ");
                //    }
                //}
                //else
                //{
                //    rtn = false;
                //    AddUploadDocumentError("Please Upload File: Proof of Liability Insurance ");
                //}
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, this.WorkflowPage.RegistrationId.ToString(), false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");

                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;

                int applicationTypeID = ObjectControllerHelper.GetInt("ApplicationTypeID", drReg);
            }
        }

        if (row != null)
        {
            ds = psc.SelectRegSectionUploadDocument(CON.RegistrationPageType.LicensesClassifications, this.WorkflowPage.RegistrationId, Helper.GetInt("PROVIDER_TYPE_ID", row), null, 0);
            dsVisibleDocs = psc.SelectRegSectionUploadControl(CON.RegistrationPageType.LicensesClassifications, Helper.GetInt("APPLICATION_TYPE_ID", row), Helper.GetInt("PROVIDER_TYPE_ID", row), Helper.GetInt("ENTITY_TYPE_ID", row), "Specialties", WorkflowPage.RegistrationId);
            //Proof of International Board of Lactation Consultant
            if (!DocIsUploaded(ds, CON.SpecialtyUploadTitles.LactationConsultant)
                && IsDocumentVisible(dsVisibleDocs, CON.SpecialtyUploadTitles.LactationConsultant)
                && IsRequiredDocumentUpload(dsVisibleDocs, CON.SpecialtyUploadTitles.LactationConsultant)
                )
            {
                rtn = false;
                AddUploadDocumentError("Please Upload File: " + CON.SpecialtyUploadTitles.LactationConsultant);
            }

            if (!DocIsUploaded(ds, CON.SpecialtyUploadTitles.DCYFamilyConnects) && IsDocumentVisible(dsVisibleDocs, CON.SpecialtyUploadTitles.DCYFamilyConnects))
            {
                rtn = false;
                AddUploadDocumentError("Please Upload File: " + CON.SpecialtyUploadTitles.DCYFamilyConnects);
            }

            if (!DocIsUploaded(ds, CON.SpecialtyUploadTitles.PediatricRecovery) && IsDocumentVisible(dsVisibleDocs, CON.SpecialtyUploadTitles.PediatricRecovery))
            {
                rtn = false;
                AddUploadDocumentError("Please Upload File: " + CON.SpecialtyUploadTitles.PediatricRecovery);
            }

            if (!DocIsUploaded(ds, CON.SpecialtyUploadTitles.OhioRISEProviderPlan) && IsDocumentVisible(dsVisibleDocs, CON.SpecialtyUploadTitles.OhioRISEProviderPlan))
            {
                rtn = false;
                AddUploadDocumentError("Please Upload File: " + CON.SpecialtyUploadTitles.OhioRISEProviderPlan);
            }
        }
        return rtn;
    }
    private bool IsRequiredDocumentUpload(DataSet ds, string title)
    {
        bool rtn = false;
        if (Helper.HasRows(ds))
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Helper.GetString("title", dr) == title && Helper.GetString("is_required", dr) != "False")
                    rtn = true;
            }

        }
        return rtn;
    }

    private bool IsDocumentVisible(DataSet ds, string title)
    {
        bool rtn = false;
        if (Helper.HasRows(ds))
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Helper.GetString("title", dr) == title)
                    rtn = true;
            }

        }
        return rtn;
    }

    private bool PocIsUploaded(DataSet ds, string title, string sectionName = null)
    {
        bool rtn = false;
        int table = ds.Tables.Count;
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                if (Helper.GetString("TITLE", dr) == title && Helper.GetInt("REG_SECTION_UPLOAD_CONTROL_ID", dr) > 0 && (ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                {
                    if (Helper.GetInt("DOCUMENT_ID", dr) > 0)
                        rtn = true;
                }
            }
        }
        return rtn;
    }
    private bool DocIsUploaded(DataSet ds, string title, string sectionName = null)
    {
        bool rtn = false;
        int table = ds.Tables.Count;
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                if (Helper.GetString("TITLE", dr) == title && Helper.GetInt("REG_SECTION_UPLOAD_CONTROL_ID", dr) > 0 && (ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                {
                    if (Helper.GetInt("DOCUMENT_ID", dr) > 0 && (string.IsNullOrEmpty(sectionName) || (sectionName != "OtherDocuments" && !string.IsNullOrEmpty(sectionName) && Helper.GetInt("ROW_ID", dr) > 0) || sectionName == "OtherDocuments"))
                        rtn = true;
                }
            }
        }
        return rtn;
    }
    private bool ValidatePOCUpload()
    {
        bool rtn = true;
        DataSet ds = null;
        Upload upload = new Upload();

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataRow row = Registration.GetRegistration(this.WorkflowPage.RegistrationId);
        if (row != null)
        {
            ds = psc.SelectRegSectionUploadDocument(CON.RegistrationPageType.SiteVisitScreening, this.WorkflowPage.RegistrationId, Helper.GetInt("PROVIDER_TYPE_ID", row), "Site Visit", 0);

            if (!PocIsUploaded(ds, "Plan of Correction", "Site Visit"))
            {
                rtn = false;
                AddUploadDocumentError("Please Upload File: Plan of Correction");
            }


        }

        return rtn;
    }
    private void AddUploadDocumentError(string errMsg)
    {
        upPnlErrors.Visible = true;

        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);

        //Label lbl = new Label();
        //lbl.CssClass = "failureNotification";
        //lbl.Text = errMsg;
        //pnlScreenErrors.Controls.Add(lbl);
        //pnlScreenErrors.Controls.Add(new LiteralControl("<br />"));

    }

    // todo AP-New code for Assigned TO
    private void GetStepInfo()
    {
        DataSet ds = svc.WF_SelectStepInfo(StepID);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            WorkflowID = Convert.ToInt32(ds.Tables[0].Rows[0]["WORKFLOW_ID"]);
            WorkflowName = ds.Tables[0].Rows[0]["WORKFLOW_NAME"].ToString();
            TaskID = Convert.ToInt32(ds.Tables[0].Rows[0]["TASK_ID"]);
            TaskType = ds.Tables[0].Rows[0]["TASK_TYPE"].ToString();
            TaskName = ds.Tables[0].Rows[0]["TASK_NAME"].ToString();
            GroupNameList = ds.Tables[0].Rows[0]["GROUP_NAME_LIST"].ToString();
            StepID = Convert.ToInt32(ds.Tables[0].Rows[0]["STEP_ID"]);
            StepOwner = ds.Tables[0].Rows[0]["STEP_OWNER_ID"].ToString();
            StepCreateDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["STEP_CREATE_DATE_TIME"]);
            StepStartDate = ds.Tables[0].Rows[0]["STEP_START_DATE_TIME"] != DBNull.Value
                ? Convert.ToDateTime(ds.Tables[0].Rows[0]["STEP_START_DATE_TIME"])
                : StepCreateDate;
            StepEndDate = ds.Tables[0].Rows[0]["STEP_END_DATE_TIME"] != DBNull.Value
                ? (DateTime?)Convert.ToDateTime(ds.Tables[0].Rows[0]["STEP_END_DATE_TIME"])
                : null;
            StepNotes = ds.Tables[0].Rows[0]["STEP_NOTES"].ToString();
            ProcessID = Convert.ToInt32(ds.Tables[0].Rows[0]["PROCESS_ID"]);
            ProcessOwner = ds.Tables[0].Rows[0]["PROCESS_OWNER_ID"].ToString();
            ProcessStartDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PROCESS_START_DATE_TIME"]);
            ProcessEndDate = ds.Tables[0].Rows[0]["PROCESS_END_DATE_TIME"] != DBNull.Value
                ? (DateTime?)Convert.ToDateTime(ds.Tables[0].Rows[0]["PROCESS_END_DATE_TIME"])
                : null;
            if (ds.Tables[0].Rows[0]["LOG_THREAD_NUMBER"] != DBNull.Value)
            {
                LogThreadID = new Guid(ds.Tables[0].Rows[0]["LOG_THREAD_NUMBER"].ToString());
            }


            // Only get task parameters if Task ID is retrieved in Step Info (should always happen.)
            GetTaskParameters();
        }
    }

}
