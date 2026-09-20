using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Pages_SiteVisit : BaseSectionControl
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

    /// <summary>
    /// The flow of when these records get inserted will probably change post demo. 
    /// SCREENING_ACTIVITY with for this REG_IDs SCREENING_ID and SCREENING_ACTIVITY_TYPE_ID = 11
    /// -for demo seeded when main screenings were completed and provider risk level warrented site visit
    ///     LOAD SITE_VISIT_SCREENING grid by SCREENING_ACTIVITY_ID
    ///     -for demo seeded when screening activity was seeded
    ///         LOAD SITE_VISIT grid by SITE_VISIT_SCREENING_ID
    ///         -for demo, both pre-and post-enrollment site visit types were seeded when screening activity was seeded
    ///             LOAD SITE_VISIT_ATTEMPT grid by SITE_VISIT_ID
    ///                 -can add here, for demo all other records were seeded.
    ///                 -if site visit attempts grid is empty, will open Site Visit Attempt Details ready for input
    /// </summary>
    /// 

    public class SiteVisitSelectedEventArgs : EventArgs
    {
        public int SiteVisitID { get; set; }
        public DateTime SiteVisitStartDate { get; set; }
        public string SiteVisitTypeName { get; set; }

        public SiteVisitSelectedEventArgs(int siteVisitID, DateTime siteVisitStartDate, string siteVisitTypeName)
            : base()
        {
            SiteVisitID = siteVisitID;
            SiteVisitStartDate = siteVisitStartDate;
            SiteVisitTypeName = siteVisitTypeName;
        }
    }
    
    public class SiteVisitAttemptSelectedEventArgs : EventArgs
    {
        public int SiteVisitAttemptID { get; set; }

        public SiteVisitAttemptSelectedEventArgs(int siteVisitAttemptID)
            : base()
        {
            SiteVisitAttemptID = siteVisitAttemptID;
        }
    }

    #region Properties

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

    public int SiteVisitID
    {
        get
        {
            if (ViewState["SiteVisitID"] == null)
                ViewState["SiteVisitID"] = -1;

            return (int)ViewState["SiteVisitID"];
        }
        set
        {
            ViewState["SiteVisitID"] = value;
        }
    }

    public int ScreeningActivityID
    {
        get
        {
            if (ViewState["ScreeningActivityID"] == null)
                ViewState["ScreeningActivityID"] = -1;

            return (int)ViewState["ScreeningActivityID"];
        }
        set
        {
            ViewState["ScreeningActivityID"] = value;
        }
    }


	#endregion

    #region Events

    public delegate void SiteVisitSelectedEventHandler(SiteVisitSelectedEventArgs args);
    public event SiteVisitSelectedEventHandler SiteVisitSelected;

    public delegate void SiteVisitAttemptSelectedEventHandler(SiteVisitAttemptSelectedEventArgs args);
    public event SiteVisitAttemptSelectedEventHandler SiteVisitAttemptSelected;

    #endregion
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
	{
        if (this.WorkflowPage.RegistrationId > 0)
		{
            this.IsReadOnly = (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.SiteVisitPCG || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.SiteVisitCompliance) && Helper.IsUserInSiteVisitOperatorRole(HttpContext.Current.User.Identity.Name) ? false : true;
            this.IsStateReview = this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.StateReview;

            ucSiteVisits.LoadSiteVisitList(this.WorkflowPage.RegistrationId, -1);
            SetTakeActionVisibility();
		}
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

    private void LoadSiteVisitScreenings(int regID)
    {
        //ucSiteVisitScreenings.LoadSiteVisitScreeningList(regID, -1);
   }
    
    private void ClearSiteVisitScreeningSelection()
    {
        //ucSiteVisitScreenings.ClearSelection();
    }

    private void ClearSiteVisitAttemptSelection()
    {
        //this.ucSiteVisitAttempts.ClearSelection();
    }

    protected void ucSiteVisits_SiteVisitSelected(UserControls_SiteVisits.SiteVisitSelectedEventArgs args)
    {
        //ucSiteVisitAttempts.IsReadOnly = IsReadOnly;
        //ucSiteVisitAttempts.LoadSiteVisitAttempts(args.SiteVisitID, -1, true);
        int siteVisitAttemptStatus = 0;
        int siteVisitAttemptCount = 0;
        int SiteVisitAttemptID = ucSiteVisits.GetSelectedSiteVisitAttempt(ref siteVisitAttemptStatus,ref siteVisitAttemptCount);
        ScreeningActivityID = SiteVisitAttemptID;
        ucAttemptDetails.LoadSiteVisitAttempt(args.SiteVisitID, SiteVisitAttemptID, ScreeningActivityID, args.SiteVisitStartDate, args.SiteVisitTypeName, siteVisitAttemptStatus,siteVisitAttemptCount);
         if (this._setDocumentProperties!= null)
         {
             this._setDocumentProperties(Enumerations.ScreeningActivityType.SiteVisit.ToString(), ScreeningActivityID);
         }

    }

    protected void ucSiteVisits_SiteVisitUpdated()
    {

        ucSiteVisits.LoadSiteVisitList(this.WorkflowPage.RegistrationId, -1);
        

    }
    /*protected void ucSiteVisitAttempts_SiteVisitAttemptSelected(UserControls_SiteVisitAttempts.SiteVisitAttemptSelectedEventArgs args)
    {
        ucAttemptDetails.IsReadOnly = IsReadOnly;
        SiteVisitID = args.SiteVisitID;
        ucAttemptDetails.LoadSiteVisitAttempt(args.SiteVisitID, args.SiteVisitAttemptID, ScreeningActivityID);
    }*/

    protected void ucAttemptDetails_SiteVisitAttemptCancel()
    {
        //ucSiteVisitAttempts.ClearSelection();
        ucAttemptDetails.SiteVisitAttemptVisible = false;
    }

    protected void ucAttemptDetails_SiteVisitAttemptUpdated()
    {
        //ucSiteVisitAttempts.LoadSiteVisitAttempts(SiteVisitID, -1, false);
        //ucSiteVisitAttempts.ClearSelection();

        SetTakeActionVisibility();
    }

    #region Private methods

    private void SiteVisitScreeningUpdated()
    {
        if (this.WorkflowPage.RegistrationId > 0)
        {
            ClearSiteVisitScreeningSelection();
            LoadSiteVisitScreenings(this.WorkflowPage.RegistrationId);
        }
    }

    private void SetCreateAdverseActionVisibility(int? siteVisitRecommendationID)
    {
        //TODO:  need reqts defined for this
        //if (siteVisitRecommendationID.HasValue && siteVisitRecommendationID == CON.SiteVisitRecommendationID.Failed)
        //{
        //    ucAdverseActionHeader.CreateAdverseActionButtonVisible = true;
        //}
        //else
        //{
        //    ucAdverseActionHeader.CreateAdverseActionButtonVisible = false;
        //}
    }

    private void SetTakeActionVisibility()
    {
        int sitevisitstatus = Registration.SiteVisitComplete(this.WorkflowPage.RegistrationId);
        bool isSiteVisitComplete = false;
        if (sitevisitstatus == CON.ScreeningStatusId.InProgress)
        {
            isSiteVisitComplete = false;
        }
        else
        {
            isSiteVisitComplete = true;
        }
        bool isTakeActionVisible = false;
        isTakeActionVisible = IsReadOnly ? false : IsStateReview ? true : isSiteVisitComplete ? true : (sitevisitstatus == CON.ScreeningStatusId.Failed) ? true : false;
        
        if (this._toggleTakeActionVisibility!= null)
        {
            this._toggleTakeActionVisibility(new ToggleTakeActionVisibilityEventArgs(isTakeActionVisible));
        }
    }


    #endregion

    public override string ValidationGroup
    {
        get { return "valWorkflowSteps"; }
    }

    public override string Title
    {
        get { return "Workflow Steps"; }
    }

    public override string IdText
    {
        get { return "ucWorkflowSteps_" + this.WorkflowPage.RegistrationId; }
    }

}