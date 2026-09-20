using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_CredentialActivities : System.Web.UI.UserControl
{
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

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public class CredentialActivitySelectedEventArgs : EventArgs
    {
        public int ActivityID { get; set; }
        public Enumerations.CredentialActivityType CredentialActivityType { get; set; }
        public int DataRankID { get; set; }


        public CredentialActivitySelectedEventArgs(int credentialActivityID, Enumerations.CredentialActivityType credentialActivityType, int credentialActivityRank)
            : base()
        {
            ActivityID = credentialActivityID;
            CredentialActivityType = credentialActivityType;
            DataRankID = credentialActivityRank;
            
        }

    }
    
    #region Events

    public delegate void CredentialActivitySelectedEventHandler(CredentialActivitySelectedEventArgs args);
    public event CredentialActivitySelectedEventHandler CredentialActivitySelected;
    public delegate void CredentialActivityDataBindEventHandler();
    public event CredentialActivityDataBindEventHandler CredentialActivityDataBind;

    protected override void OnLoad(EventArgs e)
    {

        base.OnLoad(e);

        if (CredentialDetailsSelectedIndexID > 0)
        {
            grdCredentialDetails.SelectedIndex = CredentialDetailsSelectedIndexID;
            grdCredentialDetails.SelectRow(grdCredentialDetails.SelectedIndex);
        }
    }

    protected void grdCredentialDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdCredentialDetails.SelectedIndex > -1)
        {
            int activityID = (int)grdCredentialDetails.SelectedDataKey.Values["CREDENTIAL_ACTIVITY_ID"];
            int activityTypeID = (int)grdCredentialDetails.SelectedDataKey.Values["ACTIVITY_TYPE_ID"];
            int activityDataRank = (int)grdCredentialDetails.SelectedDataKey.Values["DATARANK_TYPE_ID"];

            if (!Enum.IsDefined(typeof(Enumerations.CredentialActivityType), activityTypeID))
                activityTypeID = (int)Enumerations.CredentialActivityType.UnDefined;

            Enumerations.CredentialActivityType credentialActivityType = (Enumerations.CredentialActivityType)activityTypeID;
             
            if (CredentialActivitySelected != null)
            {
                CredentialActivitySelected(new CredentialActivitySelectedEventArgs(activityID, credentialActivityType, activityDataRank));
            }
            CredentialDetailsSelectedIndexID = grdCredentialDetails.SelectedIndex;
        }
        else
        {

            /// Hide the details area.
            if (CredentialActivitySelected != null)
            {
                CredentialActivitySelected(new CredentialActivitySelectedEventArgs(-1, Enumerations.CredentialActivityType.UnDefined, -1));
            }
        }

    }

    protected void grdCredentialDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
          
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Image imgStatus = e.Row.FindControl("imgStatus") as Image;
            Label lblNotes = e.Row.FindControl("lblNotes") as Label;
            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.SiteVisitAdministrator))
            {
                LinkButton linkButton = e.Row.FindControl("btnSelectActivity") as LinkButton;
                linkButton.Enabled = false;
            }              
            int dataRankId = (int)DataBinder.Eval(e.Row.DataItem, "DATARANK_TYPE_ID");
            int activityTypeID = (int)DataBinder.Eval(e.Row.DataItem, "ACTIVITY_TYPE_ID");
            var activityTypeName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "SCREENING_ACTIVITY_TYPE_NAME"));

            if (imgStatus != null)
            {
                imgStatus.ImageUrl = GetScreeningActivityStatusImageUrl(activityTypeID, dataRankId);
            }
            if (this.WorkflowPage.MMISProviderTypeID != "01" && activityTypeName != null && activityTypeName.ToLower() == "maternity license")
            {
                e.Row.Visible = false;
            }
        }
    }

    //protected void lnkNotes_Command(object sender, CommandEventArgs e)
    //{
    //    int screeningActivityID = int.Parse((string)e.CommandArgument);

    //    if (screeningActivityID > 0)
    //    {
    //        DataSet ds = svc.SelectAdverseActions(screeningActivityID);


    //        if (ds != null && ds.Tables.Count > 0)
    //        {
    //           // ucAdverseActionList.LoadData(ds.Tables[0]);
    //            mpe.Show();
    //        }

    //    }

    //}


    #endregion

	#region Properties

	public int CredentialID
	{
		get
		{
            if (ViewState["CredentialID"] == null)
                ViewState["CredentialID"] = -1;

            return (int)ViewState["CredentialID"];
		}
		set
		{
            ViewState["CredentialID"] = value;
		}
	}

    public int CredentialDetailsSelectedIndexID
	{
		get
		{
            if (ViewState["CredentialDetailsSelectedIndexID"] == null)
                ViewState["CredentialDetailsSelectedIndexID"] = -1;

            return (int)ViewState["CredentialDetailsSelectedIndexID"];
		}
		set
		{
            ViewState["CredentialDetailsSelectedIndexID"] = value;
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

    public GridView GridCredentialActivities
    {
        get
        {
            return this.grdCredentialDetails;
        }
    }

    #endregion

    #region Public Methods

    public bool ActionTypeExists(int typeID)
    {
        bool bExists = false;

        GridView gv = this.grdCredentialDetails;

        foreach (GridViewRow row in gv.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if ((int)gv.DataKeys[row.RowIndex]["ACTIVITY_TYPE_ID"] == typeID)
                {
                    bExists = true;
                }

            }
        }

        return bExists;
    }

    //public bool HasPendingActivities()
    //{
    //    bool hasPending = false;

    //    GridView gv = this.grdScreeningDetails;

    //    foreach (GridViewRow row in gv.Rows)
    //    {
    //        if (row.RowType == DataControlRowType.DataRow)
    //        {
    //            int activityRank = (int)gv.DataKeys[row.RowIndex]["ACTIVITY_DATARANK_ID"];
    //            int activityTypeID = (int)gv.DataKeys[row.RowIndex]["ACTIVITY_TYPE_ID"];
    //            if (ScreeningHelper.IsPendingActivityStatus(activityRank) && (!ScreeningHelper.ScreeningActivityIsPostScreeningStep(activityTypeID)))
    //            {
    //                hasPending = true;
    //                break;
    //            }
    //        }
    //    }

    //    return hasPending;
    //}


    public void LoadProviderCredentialDetails(int credentialID, DateTime? StartDate, DateTime? EndDate)
    {
        CredentialID = credentialID;
        int reg_id = this.WorkflowPage.RegistrationId;
        LoadCredentialActivities(reg_id);
        


    }

    public void ClearSelection()
    {
        grdCredentialDetails.SelectRow(-1);
    }

    public void Refresh()
    {
        LoadCredentialActivities(this.WorkflowPage.RegistrationId);
        grdCredentialDetails.SelectRow(grdCredentialDetails.SelectedIndex);
    }

    #endregion

    #region Private Methods

    private void LoadCredentialActivities(int reg_id)
	{
        DataSet ds = svc.SelectProviderCredentialActivityData(reg_id, CredentialID);
        DataSet dsDEA = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DEA");
        DataSet dsCDS = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "STATE_CDS_NUMBER");
       
            if (Helper.HasRows(ds))
            {
                DataTable dt = ds.Tables[0];

                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    string screeningTypeName = Helper.GetString("SCREENING_ACTIVITY_TYPE_NAME", dr);
  
                }

                ds.AcceptChanges();
                grdCredentialDetails.DataSource = ds.Tables[0];
                grdCredentialDetails.DataBind();

                if (CredentialActivityDataBind != null)
                {
                    CredentialActivityDataBind();
                }
            }
        
        
	}

	private string GetScreeningActivityStatusImageUrl(int ActivityTypeID, int dataRankId)
	{
        
        
        if (CredentialHelper.CompleteNegativeResultActivityStatus(ActivityTypeID,dataRankId))
        {
            //want yellow so the image will stand out for potentially negative response
            return "~/Images/bullet-yellow.png";
        }
        if (CredentialHelper.IsPendingActivityStatus(ActivityTypeID, dataRankId))
        {
            return "~/Images/bullet-blue.png";
        }
        else if (CredentialHelper.CompletePositiveResultActivityStatus(ActivityTypeID, dataRankId))
        {
            return "~/Images/check.png";
        }
        else
        {
            return "~/Images/check.png";
        }
	}

   
    
    #endregion

	
}