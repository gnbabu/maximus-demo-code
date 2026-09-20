using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Runtime.CompilerServices;

/// <summary>
/// Summary description for BaseSectionControl
/// </summary>
public abstract class BaseSectionControl : System.Web.UI.UserControl
{
    public BaseSectionControl()
    {
    }

    public System.EventHandler InvalidateAgreements;

    public delegate void ToggleTakeActionVisibilityEventHandler(ToggleTakeActionVisibilityEventArgs args);
    protected ToggleTakeActionVisibilityEventHandler _toggleTakeActionVisibility;
    public event ToggleTakeActionVisibilityEventHandler ToggleTakeActionVisibility
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        add
        {
            this._toggleTakeActionVisibility += value;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        remove
        {
            this._toggleTakeActionVisibility -= value;
        }
    }

    public delegate void RefreshNavigationTreeEventHandler();
    protected RefreshNavigationTreeEventHandler _refreshNavigationTree;
    public event RefreshNavigationTreeEventHandler RefreshNavigationTree
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        add
        {
            this._refreshNavigationTree += value;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        remove
        {
            this._refreshNavigationTree -= value;
        }
    }

    public delegate void LoadAdverseActionsEventHandler();
    protected LoadAdverseActionsEventHandler _loadAdverseActions;
    public event LoadAdverseActionsEventHandler LoadAdverseActions
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        add
        {
            this._loadAdverseActions += value;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        remove
        {
            this._loadAdverseActions -= value;
        }
    }

    public delegate void SetDocumentPropertiesEventHandler(string sectionName, int screeningActivityID);
    protected SetDocumentPropertiesEventHandler _setDocumentProperties;
    public event SetDocumentPropertiesEventHandler SetDocumentProperties
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        add
        {
            this._setDocumentProperties += value;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        remove
        {
            this._setDocumentProperties -= value;
        }
    }

    /// <summary>
    /// This is used for loading the data like the grids etc.
    /// </summary>
    public abstract void LoadControlData();

    /// <summary>
    /// This is used for loading the inset like editing a particular item in the grid etc. 
    /// </summary>
    /// <param name="dr"></param>
    public abstract void LoadData(DataRow dr = null);

    public abstract bool SaveData();

    public abstract bool ValidateData();
    public virtual bool ValidateSaveNextData()
    {
        // If ValidateSaveNextData is not implemented by the derived section,
        // By default, we say we dont have any fields that have values. 
        // This will allow for optional sections to be skipping validation
        return true;
    }

    public abstract string Title
    {
        get;
    }

    public abstract string IdText
    {
        get;
    }

    public abstract string ValidationGroup
    {
        get;
    }

    public virtual bool HasInputValue()
    {
        // If HasInputValue is not implemented by the derived section,
        // By default, we say we dont have any fields that have values. 
        // This will allow for optional sections to be skipping validation
        return false;
    }

    public Enumerations.ScreeningEntityType ScreeningType { get; set; }

    public DataTable DataList
    {
        get
        {
            string id = this.IdText + "_DataList";
            if (ViewState[id] == null) return null;
            return (DataTable)ViewState[id];
        }
        set 
        {
            string id = this.IdText + "_DataList";
            ViewState[id] = value; 
        }
    }

    protected RecipientInformation FindRecipient( string medicaidbillingnumber, string medicaidID, Guid userId, string dateofBirth)
    {
        RecipientEligibilitySearchReqRes recipientEligibility = new RecipientEligibilitySearchReqRes();
        RecipientInformation recipientInfo  = recipientEligibility.GetRecipientInformation(medicaidbillingnumber, medicaidID, userId, dateofBirth, "BaseSectionControl");
        return recipientInfo;
    }
	
    public Boolean inMaintenance(int registrationId)
    {
        // get reference to service client
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        // get registration work flow current step
        int currentStepID = -1;
        DataSet wfProcessDs = psc.GetWFProcessByRegId(registrationId);
        if (Helper.HasRows(wfProcessDs))
        {
            currentStepID = Helper.GetInt("CURRENT_STEP_ID", wfProcessDs.Tables[0].Rows[0]);
        }

        // get registration program status type id
        int registrationProgramStatusTypeID = -1;
        DataSet regDs = psc.SelectRegistrationByRegID(registrationId);

        if (Helper.HasRows(regDs))
        {
            registrationProgramStatusTypeID = Helper.GetInt("RegProgramStatusTypeID", regDs.Tables[0].Rows[0]);
        }

        if (registrationProgramStatusTypeID == 2 && currentStepID == 0)
        {
            // in maintenance
            return true;
        }

        return false;
    }
}