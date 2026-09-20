using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;


public partial class UserControls_ScreeningActivities : System.Web.UI.UserControl
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
    
    public class ScreeningActivitySelectedEventArgs : EventArgs
	{
		public int ScreeningActivityID { get; set; }
        public Enumerations.ScreeningActivityType ScreeningActivityType { get; set; }
        public int ScreeningActivityStatusID { get; set; }
        public Enumerations.ScreeningEntityType EntityType { get; set; }

        public ScreeningActivitySelectedEventArgs(int screeningActivityID, Enumerations.ScreeningActivityType screeningActivityType, int screeningActivityStatusID, Enumerations.ScreeningEntityType entityType)
			: base()
		{
			ScreeningActivityID = screeningActivityID;
            ScreeningActivityType = screeningActivityType;
			ScreeningActivityStatusID = screeningActivityStatusID;
			EntityType = entityType;
		}

	}
    public int ScreeningDetailsSelectedIndexID
    {
        get
        {
            if (ViewState["ScreeningDetailsSelectedIndexID"] == null)
                ViewState["ScreeningDetailsSelectedIndexID"] = -1;

            return (int)ViewState["ScreeningDetailsSelectedIndexID"];
        }
        set
        {
            ViewState["ScreeningDetailsSelectedIndexID"] = value;
        }
    }
    #region Events

    public delegate void ScreeningActivitySelectedEventHandler(ScreeningActivitySelectedEventArgs args);
    public event ScreeningActivitySelectedEventHandler ScreeningActivitySelected;
    public delegate void ScreeningActivityDataBindEventHandler();
    public event ScreeningActivityDataBindEventHandler ScreeningActivityDataBind;

    protected override void OnLoad(EventArgs e)
    {
        if(IsPostBack && ScreeningDetailsSelectedIndexID > -1)
        {
            grdScreeningDetails.SelectedIndex = ScreeningDetailsSelectedIndexID;
            Refresh();
        }
        base.OnLoad(e);
    }

    protected void grdScreeningDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdScreeningDetails.SelectedIndex > -1)
        {
            int screeningActivityID = (int)grdScreeningDetails.SelectedDataKey.Values["SCREENING_ACTIVITY_ID"];
            int screeningActivityTypeID = (int)grdScreeningDetails.SelectedDataKey.Values["SCREENING_ACTIVITY_TYPE_ID"];
            int screeningActivityStatusID = (int)grdScreeningDetails.SelectedDataKey.Values["SCREENING_ACTIVITY_STATUS_ID"];

            if (!Enum.IsDefined(typeof(Enumerations.ScreeningActivityType), screeningActivityTypeID))
                screeningActivityTypeID = (int)Enumerations.ScreeningActivityType.UnDefined;

            Enumerations.ScreeningActivityType screeningActivityType = (Enumerations.ScreeningActivityType)screeningActivityTypeID;

            if (ScreeningActivitySelected != null)
            {
                ScreeningActivitySelected(new ScreeningActivitySelectedEventArgs(screeningActivityID, screeningActivityType, screeningActivityStatusID, EntityType));
            }
            ScreeningDetailsSelectedIndexID = grdScreeningDetails.SelectedIndex;
        }
        else
        {
            /// Hide the details area.
            if (ScreeningActivitySelected != null)
            {
                ScreeningActivitySelected(new ScreeningActivitySelectedEventArgs(-1, Enumerations.ScreeningActivityType.UnDefined, -1, EntityType));
            }
        }

    }

    private void loadScreeningResult()
    {
        if (this.WorkflowPage.ActiveScreeningActivityID > 0)
        {
            foreach (GridViewRow gvr in grdScreeningDetails.Rows)
            {
                if (grdScreeningDetails.DataKeys[gvr.RowIndex].Value.ToString() == this.WorkflowPage.ActiveScreeningActivityID.ToString())
                {
                    grdScreeningDetails.SelectedIndex = gvr.RowIndex;
                    break;
                }
            }

            int screeningActivityID = (int)grdScreeningDetails.SelectedDataKey.Values["SCREENING_ACTIVITY_ID"];
            int screeningActivityTypeID = (int)grdScreeningDetails.SelectedDataKey.Values["SCREENING_ACTIVITY_TYPE_ID"];
            int screeningActivityStatusID = (int)grdScreeningDetails.SelectedDataKey.Values["SCREENING_ACTIVITY_STATUS_ID"];

            if (!Enum.IsDefined(typeof(Enumerations.ScreeningActivityType), screeningActivityTypeID))
                screeningActivityTypeID = (int)Enumerations.ScreeningActivityType.UnDefined;

            Enumerations.ScreeningActivityType screeningActivityType = (Enumerations.ScreeningActivityType)screeningActivityTypeID;

            if (ScreeningActivitySelected != null)
            {
                ScreeningActivitySelected(new ScreeningActivitySelectedEventArgs(screeningActivityID, screeningActivityType, screeningActivityStatusID, EntityType));
            }
        }
        else
        {
            /// Hide the details area.
            if (ScreeningActivitySelected != null)
            {
                ScreeningActivitySelected(new ScreeningActivitySelectedEventArgs(-1, Enumerations.ScreeningActivityType.UnDefined, -1, EntityType));
            }
        }
    }
    protected void grdScreeningDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Image imgStatus = e.Row.FindControl("imgStatus") as Image;
            if (Helper.IsUserInCredentialingRole(HttpContext.Current.User.Identity.Name))
            {
                LinkButton btnSelectActivity = e.Row.FindControl("btnSelectActivity") as LinkButton;
                Label lblActivityStatus = e.Row.FindControl("lblActivityStatus") as Label;
                if (btnSelectActivity != null && lblActivityStatus != null)
                {
                    btnSelectActivity.Visible = false;
                    lblActivityStatus.Visible = true;
                }
            }
            //for multirole EnrollmentSpecialist Provider Screening details Status made visible.            
            bool IsEnrollmentSpecialistexist = GetMultiUserRoleisEnrolmentSpecialist(HttpContext.Current.User.Identity.Name);
            if (IsEnrollmentSpecialistexist == true)
            {
                LinkButton btnSelectActivity = e.Row.FindControl("btnSelectActivity") as LinkButton;
                Label lblActivityStatus = e.Row.FindControl("lblActivityStatus") as Label;
                if (btnSelectActivity != null && lblActivityStatus != null)
                {
                    btnSelectActivity.Visible = true;
                    lblActivityStatus.Visible = false;
                }
            }


            int screeningActivityStatus = (int)DataBinder.Eval(e.Row.DataItem, "SCREENING_ACTIVITY_STATUS_ID");
            int screeningActivityTypeID = (int)DataBinder.Eval(e.Row.DataItem, "SCREENING_ACTIVITY_TYPE_ID");
            if (imgStatus != null)
            {
                imgStatus.ImageUrl = GetScreeningActivityStatusImageUrl(screeningActivityTypeID, screeningActivityStatus);
            }
            if (screeningActivityTypeID == (int)Enumerations.ScreeningActivityType.SiteVisit ||
                screeningActivityTypeID == (int)Enumerations.ScreeningActivityType.MedicaidIDAssigned ||
                screeningActivityTypeID == (int)Enumerations.ScreeningActivityType.WelcomeLetter)
			{
				LinkButton btnSelectActivity = e.Row.FindControl("btnSelectActivity") as LinkButton;
				Label lblActivityStatus = e.Row.FindControl("lblActivityStatus") as Label;
				if (btnSelectActivity != null && lblActivityStatus != null)
				{
					btnSelectActivity.Visible = false;
					lblActivityStatus.Visible = true;
				}
			}
        }
    }

    protected void lnkNotes_Command(object sender, CommandEventArgs e)
    {
        int screeningActivityID = int.Parse((string)e.CommandArgument);

        if (screeningActivityID > 0)
        {
            DataSet ds = svc.SelectAdverseActions(screeningActivityID);


            if (ds != null && ds.Tables.Count > 0)
            {
                ucAdverseActionList.LoadData(ds.Tables[0]);
                mpe.Show();
            }

        }

    }


    #endregion

	#region Properties

	public int ScreeningID
	{
		get
		{
			if (ViewState["ScreeningID"] == null)
				ViewState["ScreeningID"] = -1;

			return (int)ViewState["ScreeningID"];
		}
		set
		{
			ViewState["ScreeningID"] = value;
		}
	}

	public Enumerations.ScreeningEntityType EntityType
	{
		get
		{
			if (ViewState["EntityType"] == null)
                ViewState["EntityType"] = Enumerations.ScreeningEntityType.Provider;

            return (Enumerations.ScreeningEntityType)ViewState["EntityType"];
		}
		set
		{
			ViewState["EntityType"] = value;
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

    public GridView GridScreeningActivities
    {
        get
        {
            return this.grdScreeningDetails;
        }
    }

    #endregion

    #region Public Methods

    public bool ActionTypeExists(int typeID)
    {
        bool bExists = false;

        GridView gv = this.grdScreeningDetails;

        foreach (GridViewRow row in gv.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if ((int)gv.DataKeys[row.RowIndex]["SCREENING_ACTIVITY_TYPE_ID"] == typeID)
                {
                    bExists = true;
                }

            }
        }

        return bExists;
    }

    public bool HasPendingActivities()
    {
        bool hasPending = false;

        GridView gv = this.grdScreeningDetails;

        foreach (GridViewRow row in gv.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                int activityStatusID = (int)gv.DataKeys[row.RowIndex]["SCREENING_ACTIVITY_STATUS_ID"];
                int activityTypeID = (int)gv.DataKeys[row.RowIndex]["SCREENING_ACTIVITY_TYPE_ID"];
                if (ScreeningHelper.IsPendingActivityStatus(activityStatusID)	&& (!ScreeningHelper.ScreeningActivityIsPostScreeningStep(activityTypeID)))
                {
                    hasPending = true;
                    break;
                }
            }
        }

        return hasPending;
    }


    public void LoadProviderScreeningDetails(int screeningID, DateTime? screeningStartDate, DateTime? screeningEndDate, string name)
    {
        ScreeningID = screeningID;
        LoadScreeningActivities(screeningID);

        spnScreeningStart.InnerHtml = screeningStartDate.HasValue ? screeningStartDate.Value.ToString("MM/dd/yy") : string.Empty;
        spnScreeningEnd.InnerHtml = screeningEndDate.HasValue ? screeningEndDate.Value.ToString("MM/dd/yy") : string.Empty;

        lblName.Text = name;
    }

    public void ClearSelection()
    {
        grdScreeningDetails.SelectRow(-1);
    }

    public void Refresh()
    {
        LoadScreeningActivities(ScreeningID);
        if (ScreeningDetailsSelectedIndexID == -1)
            grdScreeningDetails.SelectedIndex = ScreeningDetailsSelectedIndexID;
        grdScreeningDetails.SelectRow(grdScreeningDetails.SelectedIndex);
    }

    #endregion

    #region Private Methods

    private void LoadScreeningActivities(int screeningID)
	{
		DataSet ds = svc.SelectProviderScreeningActivityData(screeningID);
        DataSet dsDEA = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DEA");
        DataSet dsCDS = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "STATE_CDS_NUMBER");
        //if (Helper.HasRows(dsDEA))
        //{
        //    //DEA exists, show DEA
        //    if (ds != null && ds.Tables.Count > 0)
        //    {
        //        grdScreeningDetails.DataSource = ds.Tables[0];
        //        grdScreeningDetails.DataBind();

        //        if (ScreeningActivityDataBind != null)
        //        {
        //            ScreeningActivityDataBind();
        //        }
        //    }
           
        //}
        //else
        //{
            //DEA doesn't exists, don't show in provider screening.
            if (Helper.HasRows(ds))
            {
                DataTable dt = ds.Tables[0];

                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    string screeningTypeName = Helper.GetString("SCREENING_ACTIVITY_TYPE_NAME", dr);

                    if (this.WorkflowPage.ApplicationTypeID == 11) //TODO credentialing hardcoded
                    {
                        if (screeningTypeName == "NPPES Verification" || screeningTypeName == "DEA Verification" ||
                         screeningTypeName == "Controlled Substance Verification" || screeningTypeName == "License Verification")
                        {
                            dr["SCREENING_METHOD_NAME"] = "Primary Source";
                            dr["SCREENING_METHOD_ID"] = "3";
                        }

                        if (screeningTypeName == "Controlled Substance Verification" || screeningTypeName== "Continuing Education Verification")
                        {
                            dr["Screening_Activity_Status_Name"] = "Verified";
                            dr["Screening_Activity_Status_ID"] = "7";
                        }


                        if (screeningTypeName == "NPPES Verification")
                            dr["Last_Action_Date_Time"] = DBNull.Value;
                         
                        if (screeningTypeName == "Site Visit")
                            dr.Delete();

                        if (screeningTypeName == "Behavioral Health Verification")
                            dr.Delete();
                    }

                    else
                    {
                        if (screeningTypeName == "DEA Verification")
                        {
                            //For OH demo only need to remove later, show as system for screening evrification method
                            //demo start
                            dr["SCREENING_METHOD_NAME"] = "System";
                            dr["SCREENING_METHOD_ID"] = "2";
                            //demo end
                            //DEA doesn't exists, don't show in provider screening.
                            if (!Helper.HasRows(dsDEA))
                            {
                                dr.Delete();
                            }
                        }
                        if (screeningTypeName == "Controlled Substance Verification")
                        {
                            //CDS doesn't exists, don't show in provider screening.
                            if (!Helper.HasRows(dsCDS))
                            {
                                dr.Delete();
                            }
                        }

                        if(screeningTypeName == "License Verification")
                        {
                        DataSet licensedProviderType = svc.SelectProviderTypeWithLicenses(this.WorkflowPage.ProviderTypeID);
                        
                        //If this provider doesn't have license page, then dont display License in provider screening.
                        if (!Helper.HasRows(licensedProviderType))
                            dr.Delete();
                        }
                    }
                }

                ds.AcceptChanges();
                grdScreeningDetails.DataSource = ds.Tables[0];
                grdScreeningDetails.DataBind();

                if (ScreeningActivityDataBind != null)
                {
                    ScreeningActivityDataBind();
                }
            }
        
        //} 
	}

	private string GetScreeningActivityStatusImageUrl(int screeningActivityTypeID, int screeningActivityStatusID)
	{
        if (ScreeningHelper.IsMatchPendingActivityStatus(screeningActivityStatusID))
        {
            //want yellow so the image will stand out for potentially negative response
            return "~/Images/bullet-yellow.png";
        }
        if (ScreeningHelper.IsPendingActivityStatus(screeningActivityStatusID))
        {
            return "~/Images/bullet-blue.png";
        }
        else if (ScreeningHelper.CompletePositiveResultActivityStatus(screeningActivityTypeID, screeningActivityStatusID))
        {
            return "~/Images/check.png";
        }
        else if (ScreeningHelper.CompleteNegativeResultActivityStatus(screeningActivityTypeID, screeningActivityStatusID))
        {
            //want yellow so the image will stand out 
            return "~/Images/bullet-yellow.png";
        }
        else
        {
            return "~/Images/check.png";
        }
	}
    #endregion

    public static bool GetMultiUserRoleisEnrolmentSpecialist(string username)
    {
        string[] rolls = Roles.GetRolesForUser(username);
        bool isEnrollmentSpecialistExist = false;
        //for mutiroles assigned roles are more than 1
        if (rolls.Length > 1)
        {
            for (int i = 0; i < rolls.Length; i++)
            {
                if (rolls[i] == CON.EnrollmentSpecialistRole)
                {
                    isEnrollmentSpecialistExist = true;
                }
            }
        }
        return isEnrollmentSpecialistExist;
    }

}