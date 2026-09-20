using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Pages_Screening : BaseSectionControl
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
	public delegate void LoadAdverseActionsEventHandler();
	public event LoadAdverseActionsEventHandler LoadAdverseActions;

	#endregion

	public int PartyID
	{
		get
		{
			if (ViewState["PartyID"] == null)
				ViewState["PartyID"] = -1;

			return (int)ViewState["PartyID"];
		}
		set
		{
			ViewState["PartyID"] = value;
		}
	}

    public int ScreeningEntityType
    {
        get
        {
            if (ViewState["ScreeningEntityType"] == null)
                ViewState["ScreeningEntityType"] = (int)Enumerations.ScreeningEntityType.Provider;

            return (int)ViewState["ScreeningEntityType"];
        }
        set
        {
            ViewState["ScreeningEntityType"] = value;
        }
    }

	public bool IsAffiliationScreening
	{
		get
		{
            return (int)this.ScreeningType == (int)Enumerations.ScreeningEntityType.Affiliation;
		}
	}

    public bool IsOwnerScreening
    {
        get
        {
            return (int)this.ScreeningType == (int)Enumerations.ScreeningEntityType.Owner;
        }
    }

    public bool IsHouseholdMemberScreening
    {
        get
        {
            return (int)this.ScreeningType == (int)Enumerations.ScreeningEntityType.HouseholdMember;
        }
    }

	public bool IsProviderScreening
	{
        get
        {
            return (int)this.ScreeningType == (int)Enumerations.ScreeningEntityType.Provider;
        }
	}

    public bool CanEdit
    {
        get
        {
            //Must be an Operator and must not be coming from provider search
            return Helper.IsUserInScreeningRole(HttpContext.Current.User.Identity.Name) && this.WorkflowPage.RegistrationIdSelected == 0;
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

    
	public bool IsStateReview
	{
		get
		{
			if (ViewState["IsStateReview"] == null)
				ViewState["IsStateReview"] = false;

			return (bool)ViewState["IsStateReview"];
		}
		set
		{
			ViewState["IsStateReview"] = value;
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
   
    #region Page Events

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
            this.IsStateReview = this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.StateReview;
            //Add settings for read-only when coming in as State Administrator
            this.ucScreeningActivities.IsReadOnly = IsReadOnly || !CanEdit;

			if (this.rblScrReview.SelectedIndex >= 0)
			{
				this.pnlScreeningDone.Visible = true;
			}
			else
			{
				pnlScreeningDone.Visible = false;
			}

		}
	}

    #endregion

    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
	{
        this.IsCredentialing = this.WorkflowPage.CurrentTaskName == "Provider Credentialing";

        if (IsAffiliationScreening)
	    {
		    pnlOwnerScreening.Visible = false;
		    pnlScreeningHistory.Visible = false;
		    pnlGroupAffiliations.Visible = true;
            pnlHouseholdScreening.Visible = false;
            ucScreeningActivities.EntityType = Enumerations.ScreeningEntityType.Affiliation;
            //pnlProviderFile.Visible = false;

		    ucScreeningActivitiesTitle.Header = "Group Member Screening Details";
            ucScreeningGroupAffiliations.LoadGroupAffiliations(this.WorkflowPage.RegistrationId);
	    }
        else if (IsOwnerScreening)
        {
            pnlOwnerScreening.Visible = true;
            pnlScreeningHistory.Visible = true;
            pnlGroupAffiliations.Visible = false;
            pnlHouseholdScreening.Visible = false;
            ucScreeningActivities.EntityType = Enumerations.ScreeningEntityType.Owner;
			////pnlProviderFile.Visible = false;
			ucScreeningHistoryTitle.Header = "Screening History";
            ucScreeningActivitiesTitle.Header = "Owner Screening Details";
            ucScreeningHistory.LoadGroupProviderScreeningHistory(this.WorkflowPage.RegistrationId);
           // ucScreeningGroupOwners.LoadGroupOwners(this.WorkflowPage.RegistrationId);
        }
        else if (IsHouseholdMemberScreening)
        {
            pnlOwnerScreening.Visible = false;
            pnlScreeningHistory.Visible = false;
            pnlGroupAffiliations.Visible = false;
            pnlHouseholdScreening.Visible = true;
            ucScreeningActivities.EntityType = Enumerations.ScreeningEntityType.HouseholdMember;

            ucScreeningActivitiesTitle.Header = "Household Member Screening Details";
            ucScreeningHHMbrs.LoadHouseholdMembers(this.WorkflowPage.RegistrationId);
        }
        else
	    {
		    pnlOwnerScreening.Visible = false;
		    pnlScreeningHistory.Visible = true;
		    pnlGroupAffiliations.Visible = false;
            pnlHouseholdScreening.Visible = false;
            ucScreeningActivities.EntityType = Enumerations.ScreeningEntityType.Provider;
            //pnlProviderFile.Visible = false;

            if (IsCredentialing)
            {
                ucScreeningActivitiesTitle.Header = "Provider Credentialing Details";
                ucScreeningHistoryTitle.Header = "Provider Credentialing History";
            }
            else
                ucScreeningActivitiesTitle.Header = "Provider Screening Details";
            ucScreeningHistory.LoadGroupProviderScreeningHistory(this.WorkflowPage.RegistrationId);
	    }

		/// Load Adverse Actions
		/// NOTE: With this piece of code here, it means the Adverse Actions will only be loaded while we are on the screening
		/// related pages. If you want them to show up on all pages, then copy this bit and put it in Page_Load inside the if(!IsPostBack) section.
		if (LoadAdverseActions != null)
		{
			LoadAdverseActions();
		}

		/// Set up Take Action visibility on the Registration screen.
		this.SetTakeActionVisibility();
	}

    public override void LoadData(DataRow dr = null)
    {
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override bool SaveData()
    {
        return true;
    }

	protected void ucScreeningActivities_ScreeningActivityDataBind()
	{
		this.SetActionButtons();
	}

	protected void chkOverride_CheckedChanged(object sender, EventArgs e)
	{

	}

	protected void ucScreeningGroupAffiliations_AffiliationScreeningSelected(UserControls_ScreeningGroupAffiliations.AffiliationScreeningSelectedEventArgs args)
	{
		ucScreeningActivities.ClearSelection();

		if (args.ScreeningID > 0)
		{
			pnlScreeningActivities.Visible = true;

			(this.Page as WorkflowPage).RegAffiliationID = args.RegAffiliationID;
            (this.Page as WorkflowPage).ActiveScreeningID = args.ScreeningID;

			/// Session vars need to be set before this call, which triggers an update to the Take Action button.
			ucScreeningActivities.LoadProviderScreeningDetails(args.ScreeningID, args.ScreeningStartDate, args.ScreeningEndDate, args.Name);

			if (RefreshNavigationTree != null)
			{
				RefreshNavigationTree();
			}
            /// Set up Take Action visibility on the Registration screen.
            this.SetTakeActionVisibility();

		}
		else
		{
			pnlScreeningActivities.Visible = false;
            (this.Page as WorkflowPage).RegAffiliationID = -1;
            (this.Page as WorkflowPage).ActiveScreeningID = -1;
		}

	}

	protected void ucScreeningGroupOwners_OwnerScreeningSelected(UserControls_ScreeningGroupOwners.OwnerScreeningSelectedEventArgs args)
	{
		ucScreeningActivities.ClearSelection();

		if (args.ScreeningID > 0)
		{
			pnlScreeningActivities.Visible = true;

            (this.Page as WorkflowPage).RegOwnerID = args.RegOwnerID;
            (this.Page as WorkflowPage).ActiveScreeningID = args.ScreeningID;
            (this.Page as WorkflowPage).OwnerScreeningID = args.ScreeningID;

            /// Session vars need to be set before this call, which triggers an update to the Take Action button.
            ucScreeningActivities.LoadProviderScreeningDetails(args.ScreeningID, args.ScreeningStartDate, args.ScreeningEndDate, args.Name);

			if (RefreshNavigationTree != null)
			{
				RefreshNavigationTree();
			}
            /// Set up Take Action visibility on the Registration screen.
            this.SetTakeActionVisibility();

		}
		else
		{
			pnlScreeningActivities.Visible = false;
            (this.Page as WorkflowPage).RegOwnerID = -1;
            (this.Page as WorkflowPage).ActiveScreeningID = -1;
		}
	}


    protected void ucScreeningHHMbrs_HHMemberScreeningSelected(UserControls_ScreeningHouseholdMembers.HouseHoldMemberScreeningSelectedEventArgs args)
    {
        ucScreeningActivities.ClearSelection();

        if (args.ScreeningID > 0)
        {
            pnlScreeningActivities.Visible = true;

            (this.Page as WorkflowPage).ActiveScreeningID = args.ScreeningID;


            /// Session vars need to be set before this call, which triggers an update to the Take Action button.
            ucScreeningActivities.LoadProviderScreeningDetails(args.ScreeningID, args.ScreeningStartDate, args.ScreeningEndDate, args.Name);

            if (RefreshNavigationTree != null)
            {
                RefreshNavigationTree();
            }
        }
        else
        {
            pnlScreeningActivities.Visible = false;
            (this.Page as WorkflowPage).ActiveScreeningID = -1;
        }
    }

	protected void ucScreeningResult_Cancel(EventArgs args)
	{
		ucScreeningActivities.ClearSelection();
	}

	protected void ucScreeningHistory_ScreeningSelected(UserControls_ScreeningHistory.ScreeningSelectedEventArgs args)
	{
		ucScreeningActivities.ClearSelection();

		if (args.ScreeningID >= 0)
		{
			pnlScreeningActivities.Visible = true;
			if(IsOwnerScreening)
			{
                ucScreeningGroupOwners.LoadGroupOwners(this.WorkflowPage.RegistrationId, args.ScreeningID);
            }
			else
			{
                ucScreeningActivities.LoadProviderScreeningDetails(args.ScreeningID, args.ScreeningStartDate, args.ScreeningEndDate, null);
            }
			
            (this.Page as WorkflowPage).ProviderScreeningID = args.ScreeningID;
            (this.Page as WorkflowPage).ActiveScreeningID = args.ScreeningID;
		}
		else
		{
			pnlScreeningActivities.Visible = false;
            (this.Page as WorkflowPage).ProviderScreeningID = -1;
            (this.Page as WorkflowPage).ActiveScreeningID = -1;
		}


	}

	protected void ucScreeningActivities_ScreeningActivitySelected(UserControls_ScreeningActivities.ScreeningActivitySelectedEventArgs args)
	{
		if (args.ScreeningActivityID > 0 && (int)args.ScreeningActivityType > 0)
		{
			bool isComplete = false;

			switch (args.EntityType)
			{ // TODO: EDV Why are we taking it from session or view state and not from the event args???
				case Enumerations.ScreeningEntityType.Provider:
                    isComplete = this.ucScreeningHistory.ScreeningComplete(this.WorkflowPage.ActiveScreeningID);
					break;
				case Enumerations.ScreeningEntityType.Owner:
                    isComplete = this.ucScreeningGroupOwners.ScreeningComplete(this.WorkflowPage.ActiveScreeningID);
					break;
				case Enumerations.ScreeningEntityType.Affiliation:
                    isComplete = this.ucScreeningGroupAffiliations.ScreeningComplete(this.WorkflowPage.ActiveScreeningID);
					break;
                case Enumerations.ScreeningEntityType.HouseholdMember:
                    isComplete = this.ucScreeningHHMbrs.ScreeningComplete(this.WorkflowPage.ActiveScreeningID);
                    break;
                default:
					isComplete = true;
					break;
			}

            if (ScreeningHelper.ShowDynamicResultDetails(args.ScreeningActivityType))
			{
                //requires that you create a new or reuse a view on ScreeningResult.ascx
                mvActivityDetails.SetActiveView(vwScreeningMatchDetails);
                (this.Page as WorkflowPage).ActiveScreeningActivityID = args.ScreeningActivityID;
                ucScreeningResult.LoadScreeningResult(args.ScreeningActivityID, (int)args.ScreeningActivityType, args.EntityType);
				ucScreeningResult.IsReadOnly = CanEdit && !isComplete && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderScreening; //SAM763
                if ((int)args.ScreeningActivityType == (int)Enumerations.ScreeningActivityType.CriminalBackgroundCheck)
                    ((DropDownList)(ucScreeningResult.FindControl("ddlMatchResults"))).Enabled = false;
			}
            else
            {
				mvActivityDetails.SetActiveView(vwNonScreeningDetails);
                ucNonScreeningResult.LoadScreeningResult(args.ScreeningActivityID, (int)args.ScreeningActivityType, args.ScreeningActivityStatusID);
				ucNonScreeningResult.IsReadOnly = CanEdit && !isComplete && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderScreening; //SAM763
            }

            if (this._setDocumentProperties != null)
            {
                string documentSection = args.ScreeningActivityType == Enumerations.ScreeningActivityType.UnDefined ? string.Empty : Enum.GetName(typeof(Enumerations.ScreeningActivityType), args.ScreeningActivityType);
                this._setDocumentProperties(documentSection, args.ScreeningActivityID);
            }
		}
		else
		{
			/// Hide the details area.
			mvActivityDetails.ActiveViewIndex = -1;
            (this.Page as WorkflowPage).ActiveScreeningActivityID = -1;
        }
	}
	
    protected void ucScreeningResult_ScreeningActivityUpdated(EventArgs args)
	{
        ucScreeningActivities.ScreeningDetailsSelectedIndexID = -1;
        ucScreeningActivities.Refresh();        
		
		if (LoadAdverseActions != null)
		{
			LoadAdverseActions();
		}

        if (RefreshNavigationTree != null)
        {
            RefreshNavigationTree();
        }

        this.SetTakeActionVisibility();
	}



	protected void ucScreeningResult_CreateAdverseAction(PopupControls_AdverseAction.CreateAdverseActionEventArgs args)
	{

		if (!string.IsNullOrEmpty(args.Description))
		{
			if (LoadAdverseActions != null)
			{
				LoadAdverseActions();
			}
		}
        
		ucScreeningActivities.Refresh();

		if (RefreshNavigationTree != null)
		{
			RefreshNavigationTree();
		}

		this.SetTakeActionVisibility();
	}

	protected void ucNonScreeningResult_ScreeningActivityUpdated(EventArgs args)
	{
		ucScreeningActivities.ClearSelection();
		ucScreeningActivities.Refresh();

		if (RefreshNavigationTree != null)
		{
			RefreshNavigationTree();
		}

		this.SetTakeActionVisibility();
	}

	protected void ucNonScreeningResult_Cancel(EventArgs args)
	{
		ucScreeningActivities.ClearSelection();
	}


	protected void btnTakeAction_Click(object sender, EventArgs e)
	{
		rblScrReview.SelectedIndex = -1;
		pnlScreeningDone.Visible = false;
		mpe.Show();
	}

	protected void btnCancelmpe_Click(object sender, EventArgs e)
	{
		this.mpe.Hide();
	}

	protected void btnMarkComplete_Click(object sender, EventArgs e)
	{
		if (rblScrReview.SelectedIndex == -1)
		{
			//have to select one option - and there is just one to select at this time.
			btnMarkComplete.Enabled = true;
			mpe.Show();
			return;
		}

        if ((this.Page as WorkflowPage).ActiveScreeningID == -1)
		{
			//fatal error
			return;
		}

        svc.UpdateScreeningStatus((this.Page as WorkflowPage).ActiveScreeningID, CON.ScreeningStatusId.Complete, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        this.LoadControlData();
		
		// TODO: Move this to Registration?
		// this.SetWorkActionVisibility(ds);
		
		if (RefreshNavigationTree != null)
		{
			RefreshNavigationTree();
		}

		if (!string.IsNullOrEmpty(txtComments.Text))
		{
            int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
            if (RegPageTypeId == 0)
                RegPageTypeId = this.WorkflowPage.RegistrationStep;
            if (RegPageTypeId == 0)
                RegPageTypeId = this.WorkflowPage.RegistrationStep;
            if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.ProviderScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OwnerScreening ||
                    Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.SiteVisitScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.BackgroundCheck ||
                    Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OrientationInformation)
                svc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.NoteOnApproved, txtComments.Text.Trim(),
                DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
		}


	}

	#region Private Methods

	private void SetTakeActionVisibility()
	{
		string screeningFor = string.Empty;
		
		if (IsAffiliationScreening)
		{
			screeningFor = CON.ScreeningFor.ActiveAffiliation;
		}
        else if (IsOwnerScreening)
        {
            screeningFor = CON.ScreeningFor.Owner;
        }
        else if (IsHouseholdMemberScreening)
        {
            screeningFor = CON.ScreeningFor.HouseholdMember;
        }
        else
		{
			screeningFor = CON.ScreeningFor.Provider;
		}

		bool isScreeningComplete = Registration.ScreeningComplete(this.WorkflowPage.RegistrationId,screeningFor, (this.Page as WorkflowPage).ActiveScreeningID);

		bool isTakeActionVisible = IsReadOnly ? false : isScreeningComplete ? false : !this.ucScreeningActivities.HasPendingActivities();
		if (this._toggleTakeActionVisibility != null)
		{
			this._toggleTakeActionVisibility(new ToggleTakeActionVisibilityEventArgs(isTakeActionVisible));
		}
	}

	private void SetActionButtons()
	{

	}
    
	private bool HasAdverseActions()
	{
		bool hasAdverseActions = false;
		PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectScreeningAdverseActions(this.WorkflowPage.RegistrationId);
		if (Helper.HasRows(ds))
		{
			DataTable Screenings = ds.Tables[0];
			if (Screenings.Rows.Count > 0)
			{
				return true;
			}
		}

		return hasAdverseActions;
	}

	#endregion

    public override string ValidationGroup
    {
        get { return "valScreening"; }
    }

    public override string Title
    {
        get { return "Screening Steps"; }
    }

    public override string IdText
    {
        get { return "ucScreeningSteps_" + this.WorkflowPage.RegistrationId; }
    }
}