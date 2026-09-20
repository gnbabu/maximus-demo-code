using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Views_ProviderSummaryView : System.Web.UI.UserControl, IProviderManagerView
{
    public static class ReferralStatus
    {
        public const string Converted = "Provider Is Pending Conversion";
        public const string NotFound = "Provider Location Not Found";
    }
    public static class ReferralStatusID
    {
        public const int Maintenance = 1;
        public const int NotFound = 2;
        public const int PendingConversion = 3;
        public const int Unavailable = 4;
        public const int MultiplesFound = 5;
        public const int ActiveWorkflow = 5;
    }

    public static class ReferralReviewStatusID
    {
        public const int Pending = 1;
        public const int Complete = 2;
    }

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

    public ProviderManagerData Model { get; set; }

    private Guid UserID
    {
        get
        {
            return Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        }
    }

    private string TaxID
    {
        get
        {
            return this.lblTaxID.Text.Trim();
        }
        set
        {
            this.lblTaxID.Text = value;
        }
    }

    private int? _taxIDTypeID;
    private int TaxIDTypeID
    {
        get
        {
            if (_taxIDTypeID.HasValue)
                return _taxIDTypeID.Value;
            else
            {
                DataSet dsUAI;
                using(PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
                {
                    dsUAI = svc.GetUserAccountInformation(UserID.ToString());
                }
                if (Helper.HasRows(dsUAI))
                    _taxIDTypeID = Helper.ConvertStrNullToInt32(dsUAI.Tables[0].Rows[0]["TAX_ID_TYPE_ID"]);
                else
                    _taxIDTypeID = -1;

                return _taxIDTypeID.Value;
            }
        }

        set
        {
            _taxIDTypeID = value;
        }
    
    }

    public string ConvertedProviderActionType
    {
        get
        {
            return ViewState["ConvertedProviderActionType"].ToString();
        }

        set
        {
            ViewState["ConvertedProviderActionType"] = value;
        }
    }

    public string EnrollmentStatusCode
    {
        get
        {
            return ViewState["EnrollmentStatusCode"].ToString();
        }

        set
        {
            ViewState["EnrollmentStatusCode"] = value;
        }
    }


    public ProviderManagerData ConvertedProviderKeyData
    {
        get
        {
            return (ProviderManagerData)ViewState["ConvertedProviderKeyData"];
        }

        set
        {
            ViewState["ConvertedProviderKeyData"] = value;
        }
    }

    public Guid ConvertedProviderModifiedUserID
    {
        get
        {
            return (Guid)ViewState["ConvertedProviderModifiedUserID"];
        }

        set
        {
            ViewState["ConvertedProviderModifiedUserID"] = value;
        }

    }
    

    #endregion

    #region Parent Page Events
    public delegate void AddProviderEventHandler(ProviderManagerData providerData);
    public event AddProviderEventHandler AddProviderEvent;

    public delegate void SelectMyProviderEventHandler(ProviderManagerData providerData);
    public event SelectMyProviderEventHandler SelectMyProviderEvent;

    public delegate void SelectConvertedProviderEventHandler(ProviderManagerData providerData);
    public event SelectConvertedProviderEventHandler SelectConvertedProviderEvent;

    public delegate void ErrorEventHandler(Dictionary<string, string> lstErrors);
    public event ErrorEventHandler ErrorEvent;
    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            presenter.Init();
            presenter.SetApplicationTypes();
        }

    }

    protected override void OnPreRender(EventArgs e)
    {
        //Select the first provider and load its detail
        if (!IsPostBack)
        {
            if (this.gvMyProviders.Rows.Count > 0 && this.gvMyProviders.SelectedIndex == -1)
            {
                this.gvMyProviders.SelectRow(0);
            }
        }
        base.OnPreRender(e);
    }
    
    protected void btnAddProvider_Click(object sender, EventArgs e)
    {
        if (AddProviderEvent != null)
        {
            //Load ProviderManagerData object with key fields and send back to parent
            ProviderManagerData keyData = LoadKeyFields();
            //BaseWorkflow bw = new RegistrationNew();

            using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
            {
                keyData.WorkflowIDRequested = svc.GetWorkflowInstance(keyData.ApplicationTypeID, keyData.ProviderCategoryTypeID, 
                    keyData.ProviderTypeID, keyData.ReferralTypeID, false, false,keyData.ConvertedProvider, false); 
            }
             
            AddProviderEvent(keyData);
        }
    }

    protected void btnAddGroupMember_Click(object sender, EventArgs e)
    {
        if (AddProviderEvent != null)
        {
            //Load ProviderManagerData object with key fields and send back to parent
            ProviderManagerData keyData = LoadKeyFields();
            using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
            {
                keyData.WorkflowIDRequested = svc.GetWorkflowInstance(keyData.ApplicationTypeID, keyData.ProviderCategoryTypeID, keyData.ProviderTypeID, 
                    keyData.ReferralTypeID, false, false, keyData.ConvertedProvider, true);
            }
            AddProviderEvent(keyData);
        }

    }

    protected void btnAddServices_Click(object sender, EventArgs e)
    {
        //TODO:  will be same as provider add click - but need to let the pop up know this is for services and display
        //display popup
        if (AddProviderEvent != null)
        {
            //Load ProviderManagerData object with key fields and send back to parent
            ProviderManagerData keyData = LoadKeyFields();
            using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
            {
                keyData.WorkflowIDRequested = svc.GetWorkflowInstance(keyData.ApplicationTypeID, keyData.ProviderCategoryTypeID, keyData.ProviderTypeID,
                    keyData.ReferralTypeID, false, true, keyData.ConvertedProvider, false);
            }
            AddProviderEvent(keyData);
        }

    }


    protected void btnSaveSvcApp_Click(object sender, EventArgs e)
    {
        //TODO:  verify entered application # is valid
        //DataSet ds = psc.SelectDIDDReferralByApplicationNo(txtApplicationNumber.Text, taxID);
        //if (!Helper.HasRows(ds)) return 0;
        //return Helper.GetInt("DIDD_REFERRAL_ID", ds.Tables[0].Rows[0]);

    }

    protected void gvMyProviders_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshMyProviders();
    }

    protected void gvMyProviders_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshMyProviders();
    }

    protected void gvMyProviders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("ManageProvider"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            gvMyProviders.SelectRow(index);
            //turn off highlight in other grid
            gvGroupMbr.SelectRow(-1);
        }
    }

    protected void gvMyProviders_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (gvMyProviders.SelectedIndex > -1)
        {
            if (SelectMyProviderEvent != null)
            {
                int regID = (int)gvMyProviders.SelectedDataKey.Values["RegID"];
                ProviderManagerData keyData = LoadKeyFields();
                keyData.RegID = regID;
                SelectMyProviderEvent(keyData);
            }
        }
    }

    protected void gvGroupMbr_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("ManageGroupMemberProfile"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            gvGroupMbr.SelectRow(index);
            //turn off highlight in other grid
            gvMyProviders.SelectRow(-1);
        }
    }
    protected void gvGroupMbr_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (gvGroupMbr.SelectedIndex > -1)
        {
            if (SelectMyProviderEvent != null)
            {
                int regID = (int)gvGroupMbr.SelectedDataKey.Values["RegID"];
                ProviderManagerData keyData = LoadKeyFields();
                keyData.RegID = regID;
                SelectMyProviderEvent(keyData);
            }
        }
    }

    protected void gvGroupMbr_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshMyGroupMemberProviders();
    }
    
    protected void gvGroupMbr_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshMyGroupMemberProviders();
    }

    protected void gvConvertedProviders_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshConvertedProviders();
    }

    protected void gvConvertedProviders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {   
        	if (e.CommandName.Equals("SelectProvider") || e.CommandName.Equals("TransferProvider"))
        	{
            	int index = Convert.ToInt32(e.CommandArgument);
            	int IsPendingRegistration = string.IsNullOrEmpty(this.gvConvertedProviders.DataKeys[index].Values["IsPendingRegistration"].ToString()) ? 0 : (int)this.gvConvertedProviders.DataKeys[index].Values["IsPendingRegistration"];
            	if (IsPendingRegistration == 1)
            	{
                	if (SelectConvertedProviderEvent != null)
			        {
            	        int regID = (int)this.gvConvertedProviders.DataKeys[index].Values["RegID"];
                	    string revalDate = gvConvertedProviders.Rows[index].Cells[7].Text;
                    	bool revalDue = false;
                    	revalDue = false;// Helper.IsRevalDue(revalDate);

                    	ProviderManagerData keyData = LoadKeyFields();
                    	keyData.ProviderCategoryTypeID = string.IsNullOrEmpty(this.gvConvertedProviders.DataKeys[index].Values["ProviderCategoryTypeId"].ToString()) ? 0 : (int)this.gvConvertedProviders.DataKeys[index].Values["ProviderCategoryTypeId"];
                    	this.gvConvertedProviders.SelectRow(index);
                    	keyData.RegID = regID;
                    	keyData.ConvertedProvider = true;
                    	keyData.ApplicationTypeID = (int)gvConvertedProviders.DataKeys[index].Values["ApplicationTypeID"];

    	                using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
        	            {
            	            keyData.WorkflowIDRequested = svc.GetWorkflowInstance(keyData.ApplicationTypeID, keyData.ProviderCategoryTypeID, 
                	            keyData.ProviderTypeID, keyData.ReferralTypeID, revalDue, false, keyData.ConvertedProvider, false);
	                    }
    	                SelectConvertedProviderEvent(keyData);
        	        }
            	}
        	}
        	if (e.CommandName.Equals("TransferProvider"))
        	{
            
           		// show popup message to transfer or assign to other user
            	int index1 = Convert.ToInt32(e.CommandArgument);
            	int IsPendingRegistration1 = string.IsNullOrEmpty(this.gvConvertedProviders.DataKeys[index1].Values["IsPendingRegistration"].ToString()) ? 0 : (int)this.gvConvertedProviders.DataKeys[index1].Values["IsPendingRegistration"];
            	if (IsPendingRegistration1 == 0)
	            {
    	            Guid userID;
        	        Guid.TryParse(this.gvConvertedProviders.DataKeys[index1].Values["AssignedUserId"].ToString(), out userID);
            	    MembershipUser usr = Membership.GetUser(userID);
                	if (Helper.IsUserInRole(usr.UserName, CON.UserRole.ProviderOper))
                	{
                    	hidIndex.Value = index1.ToString();
                    	lblNewReg.Visible = true;
                    	string msg1 = "The registration you selected is being managed by  " + this.gvConvertedProviders.DataKeys[index1].Values["AssignedUserName"].ToString() + ", are you sure you want to take ownership?";
                    	lblNewReg.Text = msg1;
                    	mpe.Show();
	                }
	                else
    	            {
        	            hidIndex.Value = "";
            	        string msg = "Registration is being managed by  " + this.gvConvertedProviders.DataKeys[index1].Values["AssignedUserName"].ToString() + ". Please contact MAXIMUS  Provider Screening and  Enrollment Customer Service at 1-844-218-9700 to transfer the registration to another user.";
                	    
                    	MessageBox2.Show(UserControls_MessageModal.MessageModalMode.Continue, "Information", msg);
                	}
        	    }
			}
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvConvertedProviders_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

    }
    
    protected void gvConvertedProviders_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshConvertedProviders();
    }


    protected void gvReferrals_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshPendingReferrals();

    }


    protected void gvReferrals_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshPendingReferrals();
    }

    protected void gvReferrals_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("AcceptReferral"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int partyID = string.IsNullOrEmpty(this.gvReferrals.DataKeys[index].Values["PartyID"].ToString()) ? 0 : (int)this.gvReferrals.DataKeys[index].Values["PartyID"] ;
            int regID = string.IsNullOrEmpty(this.gvReferrals.DataKeys[index].Values["RegID"].ToString()) ? 0 : (int)this.gvReferrals.DataKeys[index].Values["RegID"];
            int referralStatusID = string.IsNullOrEmpty(this.gvReferrals.DataKeys[index].Values["ReferralStatusID"].ToString()) ? 0 : (int)this.gvReferrals.DataKeys[index].Values["ReferralStatusID"];
            int referralID = string.IsNullOrEmpty(this.gvReferrals.DataKeys[index].Values["ReferralID"].ToString()) ? 0 : (int)this.gvReferrals.DataKeys[index].Values["ReferralID"];
            int referralTypeID = string.IsNullOrEmpty(this.gvReferrals.DataKeys[index].Values["ReferralType"].ToString()) ? 0 : (int)this.gvReferrals.DataKeys[index].Values["ReferralType"];
            string zipCode = this.gvReferrals.DataKeys[index].Values["ZipCode"].ToString();
            string zipExt = this.gvReferrals.DataKeys[index].Values["ZipExt"].ToString();
            string revalDate = this.gvReferrals.Rows[index].Cells[5].Text;
            bool revalDue = false;
            revalDue = Helper.IsRevalDue(revalDate);

            ProviderManagerData keyData = LoadKeyFields();
            keyData.ReferralTypeID = referralTypeID;
            using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
            {
                keyData.WorkflowIDRequested = svc.GetWorkflowInstance(keyData.ApplicationTypeID, keyData.ProviderCategoryTypeID,
                                keyData.ProviderTypeID, keyData.ReferralTypeID, revalDue, false, keyData.ConvertedProvider, false);
            }

            keyData.PartyID = partyID;
            keyData.RegID = regID;
            keyData.ConvertedProvider = regID > 0;
            keyData.ReferralID = referralID;
            keyData.ZipCode = zipCode;
            keyData.ZipExt = zipExt;
            
            gvReferrals.SelectRow(index);
            if (referralStatusID == ReferralStatusID.PendingConversion)
            {
                if (SelectConvertedProviderEvent != null)
                {
                    keyData.RegID = regID;
                    SelectConvertedProviderEvent(keyData);
                }
            }
            else
            {
                if (AddProviderEvent != null)
                {
                    //Load ProviderManagerData object with key fields and send back to parent
                    AddProviderEvent(keyData);
                }
            }
        }
    }

    protected void gvReferrals_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        int referralStatusID = string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "ReferralStatusID").ToString()) ? 0 : (int)(DataBinder.Eval(e.Row.DataItem, "ReferralStatusID"));
        int reviewStatusID = string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "ReviewStatusTypeID").ToString()) ? 0 : (int)(DataBinder.Eval(e.Row.DataItem, "ReviewStatusTypeID"));
        if (referralStatusID == ReferralStatusID.MultiplesFound 
            || referralStatusID == ReferralStatusID.Unavailable 
            || referralStatusID == ReferralStatusID.ActiveWorkflow
            || reviewStatusID == ReferralReviewStatusID.Pending) //converted referral review status, new
        {
            LinkButton btn = (LinkButton)e.Row.FindControl("btnAcceptReferral");
            if (btn != null)
                btn.Visible = false;
        }
    }



    #endregion

    #region Public Events
    //public void InitView(string taxID)
    //{
    //    this.TaxID = taxID;
    //    presenter.Init(UserID, TaxID);
    //}

    public void InitView(string taxID, int taxIDType)
    {
        this.TaxID = taxID;
        this.TaxIDTypeID = taxIDType;
        presenter.Init(UserID, TaxID, taxIDType);
    }

    public void EnablePage(bool enable)
    {
        this.gvConvertedProviders.Enabled = enable;
        this.gvMyProviders.Enabled = enable;
        if (enable && gvConvertedProviders.Rows.Count > 0 && this.gvConvertedProviders.SelectedIndex > -1)
        {
            this.gvConvertedProviders.SelectRow(-1);
        }
        if (enable && gvReferrals.Rows.Count > 0 && this.gvReferrals.SelectedIndex > -1)
        {
            this.gvReferrals.SelectRow(-1);
        }
        this.btnAddProvider.Enabled = enable;
    }

    public void ResetGridSelections()
    {
        if (gvMyProviders.Rows.Count > 0 && this.gvMyProviders.SelectedIndex > -1)
        {
            this.gvMyProviders.SelectRow(-1);
        }
        if (gvConvertedProviders.Rows.Count > 0 && this.gvConvertedProviders.SelectedIndex > -1)
        {
            this.gvConvertedProviders.SelectRow(-1);
        }
        if (gvReferrals.Rows.Count > 0 && this.gvReferrals.SelectedIndex > -1)
        {
            this.gvReferrals.SelectRow(-1);
        }
        if (gvGroupMbr.Rows.Count > 0 && this.gvGroupMbr.SelectedIndex > -1)
        {
            this.gvGroupMbr.SelectRow(-1);
        }
    }
     
   
    #endregion

    #region Presenter Events

    public void SetMyProviders(DataSet ds, int totalRowCount)
    {
        this.gvMyProviders.DataSource = ds;
        this.gvMyProviders.VirtualItemCount = totalRowCount;
        this.gvMyProviders.DataBind();
    }
    public void SetAllProviders(DataSet ds, int totalRowCount)
    {
    }

    public void SetMyGroupMemberProfiles(DataSet ds, int totalRowCount)
    {
        if (TaxIDTypeID == CON.TaxIDType.EIN && totalRowCount > 0)
        {
            btnAddGroupMember.Visible = false;
            dvGMPHint.Visible = false;
        }
        else
        {
            btnAddGroupMember.Visible = true;
            dvGMPHint.Visible = true;
        }

        this.gvGroupMbr.DataSource = ds;
        this.gvGroupMbr.VirtualItemCount = totalRowCount;
        this.gvGroupMbr.DataBind();
    }

    public void SetMyPendingReferrals(DataSet ds, int totalRowCount)
    {
        this.gvReferrals.DataSource = ds;
        this.gvReferrals.VirtualItemCount = totalRowCount;
        this.gvReferrals.DataBind();


        this.divReferrals.Visible = Helper.HasRows(ds);
    }

    public void SetPendingProviders(DataSet ds, int totalRowCount)
    {
        this.gvConvertedProviders.DataSource = ds;
        this.gvConvertedProviders.VirtualItemCount =  totalRowCount;
        this.gvConvertedProviders.DataBind();

        this.divConvProviders.Visible = Helper.HasRows(ds);
    }
    
    public void SetErrorMessages()
    {
        if (presenter.hasErrors)
        {
            if (ErrorEvent != null)
                ErrorEvent(presenter.ErrorList);
        }
    }

    public void SetApplicationTypes(DataSet ds)
    {
        this.rblApplicationTypes.DataSource = ds;
        this.rblApplicationTypes.DataBind();
    }
    #endregion

    #region Private Methods

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

    private ProviderManagerData LoadKeyFields()
    {
        ProviderManagerData keyData = new ProviderManagerData();
        keyData.UserID = UserID;
        keyData.TaxID = TaxID;
        keyData.TaxIDTypeID = TaxIDTypeID;
        keyData.ApplicationTypeID = Helper.ConvertStringToInt32(rblApplicationTypes.SelectedValue);
        keyData.ApplicationTypeName = "Standard Application";

        return keyData;
    }


    
    private void RefreshMyProviders()
    {
        InitModel();
        string sortColWithDirection = this.gvMyProviders.GridViewSortDirection == SortDirection.Descending ? gvMyProviders.GridViewSortColumn + " DESC" : gvMyProviders.GridViewSortColumn;
        presenter.RequestMyProviderList(sortColWithDirection, gvMyProviders.PageSize, gvMyProviders.CurrentRowIndex, true);
    }

    private void RefreshMyGroupMemberProviders()
    {
        InitModel();
        string sortColWithDirection = this.gvGroupMbr.GridViewSortDirection == SortDirection.Descending ? gvGroupMbr.GridViewSortColumn + " DESC" : gvGroupMbr.GridViewSortColumn;
        presenter.RequestMyGroupMemberProfileList(sortColWithDirection, gvGroupMbr.PageSize, gvGroupMbr.CurrentRowIndex, true);
    }

    private void RefreshConvertedProviders()
    {
        InitModel();
        string sortColWithDirection = gvConvertedProviders.GridViewSortDirection == SortDirection.Descending ? gvConvertedProviders.GridViewSortColumn + " DESC" : gvConvertedProviders.GridViewSortColumn;
        presenter.RequestPendingProviderList(sortColWithDirection, gvConvertedProviders.PageSize, gvConvertedProviders.CurrentRowIndex, true);
    }

    private void RefreshPendingReferrals()
    {
        InitModel();
        string sortColWithDirection = this.gvReferrals.GridViewSortDirection == SortDirection.Descending ? gvReferrals.GridViewSortColumn + " DESC" : gvReferrals.GridViewSortColumn;
        presenter.RequestMyPendingReferralsList(sortColWithDirection, gvReferrals.PageSize, gvReferrals.CurrentRowIndex, true);
    }

    
    #endregion


    protected void gvConvertedProviders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (!string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "AllowManage").ToString()) && (bool)(DataBinder.Eval(e.Row.DataItem, "AllowManage")) == true)
            {

                if (!string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "IsPendingRegistration").ToString()) && (int)(DataBinder.Eval(e.Row.DataItem, "IsPendingRegistration")) == 1)
                {
                    e.Row.FindControl("btnTransferProvider").Visible = true;
                    e.Row.FindControl("btnManageProvider").Visible = true;
                    e.Row.FindControl("lblManageProvider").Visible = false;
                }
                else
                {
                    e.Row.FindControl("btnTransferProvider").Visible = true;
                    e.Row.FindControl("btnManageProvider").Visible = false;
                    e.Row.FindControl("lblManageProvider").Visible = true;
                }
            }
            else
            {
                e.Row.FindControl("btnTransferProvider").Visible = false;
                e.Row.FindControl("btnManageProvider").Visible = false;
                e.Row.FindControl("lblManageProvider").Visible = true;
            }
        }
    }
    protected void btnProcessYes_Click(object sender, EventArgs e)
    {
        int index;
        if (!string.IsNullOrEmpty(hidIndex.Value))
        {
            index = Convert.ToInt32(hidIndex.Value);
            int regid = (int)this.gvConvertedProviders.DataKeys[index].Values["RegID"];
            string currentUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            psc.UpdateRegistrationUserXref(regid.ToString(), currentUserId, DateTime.Now, currentUserId);
            Response.Redirect("~/Process/ProviderHomeNew.aspx");
        }
    }
    protected void btnProcessNo_Click(object sender, EventArgs e)
    {
        this.mpe.Hide();
    }
    protected void gvMyProviders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;
        string registrationStatusType = e.Row.Cells[1].Text;
        if (registrationStatusType == CON.RegistrationStatusType.TerminateProvider)
        {
            e.Row.Cells[9].Text = "";
        }
    }
}