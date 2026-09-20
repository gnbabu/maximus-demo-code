using System;
using System.Data;
using System.Web;

public partial class UserControls_NonScreeningResult : System.Web.UI.UserControl
{
    /// <summary>
    /// Used for manual screenings who do not have a detail interface custom to the screening.
    /// i.e. License screening is manual, but has a custom interface.
    /// </summary>
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


	#region Properties

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


    public bool IsReadOnly
    {
        set
        {
            this.ddlMatchResults.Enabled = value;
            this.btnUpdate.Enabled = value;
        }
    }

	public delegate void CancelEventHandler(EventArgs args);
	public event CancelEventHandler Cancel;

	public delegate void ScreeningActivityUpdateEventHandler(EventArgs args);
	public event ScreeningActivityUpdateEventHandler ScreeningActivityUpdated;



	#endregion

	protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void LoadScreeningResult(int screeningActivityID, int screeningActivityTypeID, int screeningActivityStatusID)
	{
		ScreeningActivityID = screeningActivityID;

        SetUpResultDropDownValues(screeningActivityTypeID);
		ddlMatchResults.SelectedValue = screeningActivityStatusID.ToString();
	}

	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		int screeningActivityStatusID = int.Parse(ddlMatchResults.SelectedValue);
		/// Update the Screening Activity Status
		svc.UpdateScreeningActivityStatus(ScreeningActivityID, screeningActivityStatusID, string.Empty, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

		if (ScreeningActivityUpdated != null)
		{
			ScreeningActivityUpdated(new EventArgs());
		}
	}

	protected void btnCancelNonScreeningResult_Click(object sender, EventArgs e)
	{
		if (Cancel != null)
		{
			Cancel(new EventArgs());
		}
	}


    private void SetUpResultDropDownValues(int screeningActivityTypeID)
    {
        ddlMatchResults.Items.Clear();

        DataSet ds = svc.SelectScreeningActivityStatusByActivityTypeID(screeningActivityTypeID);
        if (!Helper.HasRows(ds))
        {
            //throw new ArgumentException(string.Format("Unsupported Screening Activity Type ID: {0}", screeningActivityTypeID));
        }

        Helper.LoadDropDown(this.ddlMatchResults, ds.Tables[0], "SCREENING_ACTIVITY_STATUS_NAME", "SCREENING_ACTIVITY_STATUS_ID", false);
    }
}