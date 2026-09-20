using Corp.Core.Libraries.Helper;
using DocumentFormat.OpenXml.ExtendedProperties;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_Credentialing : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
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


    #region Events

    public delegate void RefreshNavigationTreeEventHandler();
    public event RefreshNavigationTreeEventHandler RefreshNavigationTree;

    #endregion
    public bool CanEdit
    {
        get
        {
            //Must be an Operator and must not be coming from provider search
            return Helper.IsUserInCredentialingRole(HttpContext.Current.User.Identity.Name) && this.WorkflowPage.RegistrationIdSelected == 0;
        }
    }
    
    public bool IsReadOnly
    {
        get
        {
            if (ViewState["IsReadOnly"] == null)
                ViewState["IsReadOnly"] = false;

            return (bool)ViewState["IsReadOnly"];
        }
        set
        {
            ViewState["IsReadOnly"] = value;
        }
    }
    public bool IsCredentialing
    {
        get
        {
            if (ViewState["IsCredentialing"] == null)
                ViewState["IsCredentialing"] = false;

            return (bool)ViewState["IsCredentialing"];
        }
        set
        {
            ViewState["IsCredentialing"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

        }
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(txtactioncomments, true);
            Helper.SetReadOnly(btnSaveComments, true);
            Helper.SetReadOnly(btnCancelComments, true);
        }
        // SAM459
        if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CredentialReconsideration
             && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.CredentialingReconsideration
             && (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CommitteeQualitySpecialist) 
                 || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSupervisor)))
        {
            pnlCredReconsideration.Visible = true;
            if (this.WorkflowPage.ActiveScreeningID > 0)
            {
                pnlAddComments.Visible = true;
                txtactioncomments.Enabled = true;
                btnCancelComments.Enabled = true;
                btnSaveComments.Enabled = true;
            }
            else
            {
                pnlAddComments.Visible = false;
                txtactioncomments.Enabled = false;
                btnCancelComments.Enabled = false;
                btnSaveComments.Enabled = false;
            }
        }

    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        this.IsCredentialing = (this.WorkflowPage.CurrentTaskName == "Provider Credentialing" || this.WorkflowPage.CurrentTaskName == "Committee review" || this.WorkflowPage.CurrentTaskName == "Committee Chairman review");
        ucCredentialHistory.LoadProviderCredentialHistory(this.WorkflowPage.RegistrationId);
        LoadCredentialCommiteeData();
        LoadCredentialComments(this.WorkflowPage.RegistrationId, (this.Page as WorkflowPage).ActiveScreeningID);
        this.SetTakeActionVisibility();
    }
    public override string ValidationGroup
    {
        get { return "valCredentialing"; }
    }

    public override string Title
    {
        get { return "Credentialing Steps"; }
    }

    public override string IdText
    {
        get { return "ucCredentialingSteps_" + this.WorkflowPage.RegistrationId; }
    }
    public override bool ValidateData()
    {
        return true;
    }

    public override bool SaveData()
    {
        return true;
    }
    protected void CredentialActivities_ScreeningActivitySelected()
    {
        this.SetActionButtons();
    }
    private void SetActionButtons()
    {

    }
    public override void LoadData(DataRow dr = null)
    {

    }
    protected void ucCredentialHistory_ScreeningSelected(UserControls_CredentialHistory.CredentialSelectedEventArgs args)
    {
        ucCredentialActivities.ClearSelection();

        if (args.CredentialID >= 0)
        {
            pnlCredentialActivities.Visible = true;
            ucCredentialActivities.LoadProviderCredentialDetails(args.CredentialID, args.StartDate, args.EndDate);
            (this.Page as WorkflowPage).ProviderScreeningID = args.CredentialID;
            (this.Page as WorkflowPage).ActiveScreeningID = args.CredentialID;
            if (RefreshNavigationTree != null)
            {
                RefreshNavigationTree();
            }
            this.SetTakeActionVisibility();
            LoadCredentialCommiteeData(args.CredentialID);
            LoadCredentialComments(this.WorkflowPage.RegistrationId, (this.Page as WorkflowPage).ActiveScreeningID);
        }
        else
        {
            pnlCredentialActivities.Visible = false;
            (this.Page as WorkflowPage).ProviderScreeningID = -1;
            (this.Page as WorkflowPage).ActiveScreeningID = -1;
        }


    }
    protected void ucCredentialResult_CredentialActivityUpdated(EventArgs args)
    {
        ucCredentialActivities.ClearSelection();
        ucCredentialActivities.Refresh();

        //if (LoadAdverseActions != null)
        //{
        //    LoadAdverseActions();
        //}

        if (RefreshNavigationTree != null)
        {
            RefreshNavigationTree();
        }

        this.SetTakeActionVisibility();
    }
    protected void ucScreeningResult_Cancel(EventArgs args)
    {
        ucCredentialActivities.ClearSelection();
    }
    //protected void btnTakeAction_Click(object sender, EventArgs e)
    //{
    //    rblScrReview.SelectedIndex = -1;
    //    pnlScreeningDone.Visible = false;
    //    mpe.Show();
    //}

    protected void ucCredentialActivities_CredentialActivitySelected(UserControls_CredentialActivities.CredentialActivitySelectedEventArgs args)
    {
        if (args.ActivityID > 0 && (int)args.CredentialActivityType > 0)
        {
            mvActivityDetails.SetActiveView(vwCredentialing);
            ucCredentialResult.LoadCredentialResult(args.ActivityID, (int)args.CredentialActivityType, args.DataRankID);
            ucCredentialResult.IsReadOnly = true;

            if (this._setDocumentProperties != null)
            {
                string documentSection = args.CredentialActivityType == Enumerations.CredentialActivityType.UnDefined ? string.Empty : Enum.GetName(typeof(Enumerations.CredentialActivityType), args.CredentialActivityType);
                this._setDocumentProperties(documentSection, (int)args.CredentialActivityType);
            }
        }
        else
        {
            /// Hide the details area.
            mvActivityDetails.ActiveViewIndex = -1;
        }
    }

    void LoadCredentialCommiteeData(int CredId = 0)
    {
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CommitteeQualitySpecialist) ||
            Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingChair))
        {
            pnlCommiteeResult.Visible = true;
            CredentialCommitteeResult.LoadCommitteeData(CredId);
            pnlAddComments.Visible = false;
        }
        else
        {
            pnlCommiteeResult.Visible = false;
            pnlAddComments.Visible = true;
        }
    }

    void LoadCredentialComments(int regID, int credentialingID)
    {
        DataSet dsComments = svc.SelectCredentialingComments(regID, credentialingID);
        if(Helper.HasRows(dsComments))
        {
            dtlComments.DataSource = dsComments;
            dtlComments.DataBind();
        }
        else
        {
            dtlComments.DataSource = null;
            dtlComments.DataBind();
        }
        txtactioncomments.Text = string.Empty;
    }

    protected void btnSaveComments_Click(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(txtactioncomments.Text))
        {
            lblCommentErr.Visible = true;
            lblCommentErr.Text = "Please enter Comments.";
        }
        else
        {
            lblCommentErr.Visible = false;
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("CREDENTIALING_ID", this.WorkflowPage.ActiveScreeningID.ToString());
            parms.Add("Comments", txtactioncomments.Text);
            parms.Add("Last_Modified_Date_Time", DateTime.Now.ToString());
            parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            svc.InsertRegistrationDataTable("CREDENTIALING_COMMENTS", parms);
            ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, "Credential Comments - " + txtactioncomments.Text, null, null, null, this.WorkflowPage.WF_ProcessID);

            LoadCredentialComments(this.WorkflowPage.RegistrationId, this.WorkflowPage.ActiveScreeningID);
        }
    }

    protected void btnCancelComments_Click(object sender, EventArgs e)
    {
        txtactioncomments.Text = string.Empty;
    }
    protected void ucCredentialResult_Cancel(EventArgs args)
    {
        ucCredentialActivities.ClearSelection();
    }

    protected void ucCredentialResult_CreateAdverseAction(PopupControls_AdverseAction.CreateAdverseActionEventArgs args)
    {

        if (!string.IsNullOrEmpty(args.Description))
        {
            //if (LoadAdverseActions != null)
            //{
            //    LoadAdverseActions();
            //}
        }

        ucCredentialActivities.Refresh();

        if (RefreshNavigationTree != null)
        {
            RefreshNavigationTree();
        }
        else
        {
            Refresh(true);
        }

        this.SetTakeActionVisibility();
    }

    private void SetTakeActionVisibility()
    {
        bool isScreeningComplete = !svc.HasPendingCredentialActivities(this.WorkflowPage.RegistrationId);

        bool isTakeActionVisible;       // = IsReadOnly ? false : isScreeningComplete;
        if (!IsReadOnly && isScreeningComplete &&
            (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSpecialist) ||
             Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSupervisor) ||
             Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSpecialist) ||
             Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSupervisor)))
        {
            isTakeActionVisible = true;
        }
        else isTakeActionVisible = false;

        if (this._toggleTakeActionVisibility != null)
        {
            this._toggleTakeActionVisibility(new ToggleTakeActionVisibilityEventArgs(isTakeActionVisible));
        }
    }
    //protected void ucCommitteeMember_MemberActivityUpdated(EventArgs args)
    //{
    //    ucCredentialActivities.ClearSelection();
    //    ucCredentialActivities.Refresh();
    //    LoadCredentialCommiteeData();

    //    if (RefreshNavigationTree != null)
    //    {
    //        RefreshNavigationTree();
    //    }
    //    else
    //    {
    //        //Force Redirect

    //        Refresh(true);
    //    }
    //}
    protected void ucCredentialActivities_CredentialActivityDataBind()
    {

    }
    public void Refresh(bool forceRedirect = false)
    {
        if (forceRedirect)
        {
            string url = "~/Process/Registration.aspx?Step=" + this.WorkflowPage.RegistrationStep.ToString() + "&RegId=" + this.WorkflowPage.RegistrationId.ToString();
            Response.Redirect(url);
        }
    }

    protected void btnUpheld_Click(object sender, EventArgs e)
    {
        svc.WF_TakeAction(this.WorkflowPage.WF_ProcessID, "ODM Decision Upheld", string.Empty);
        Response.Redirect("~/Default.aspx");
    }

    protected void btnNotUpheld_Click(object sender, EventArgs e)
    {
        svc.WF_TakeAction(this.WorkflowPage.WF_ProcessID, "ODM Decision Not Upheld", string.Empty);
        Response.Redirect("~/Default.aspx");
    }

}