using CustomControls;
using System;
using System.Data;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_ScreeningHouseholdMembers : System.Web.UI.UserControl
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

	#region Events


	public class HouseHoldMemberScreeningSelectedEventArgs : UserControls_ScreeningHistory.ScreeningSelectedEventArgs
	{
		public int RegHHMemberID { get; set; }
		public string Name { get; set; }

        public HouseHoldMemberScreeningSelectedEventArgs(int screeningID, DateTime? screeningStartDate, DateTime? screeningEndDate, int regHHMemberID, string name)
			: base(screeningID, screeningStartDate, screeningEndDate)
		{
			RegHHMemberID = regHHMemberID;
			Name = name;
		}
	}

    public delegate void HouseHoldMemberScreeningSelectedEventHandler(HouseHoldMemberScreeningSelectedEventArgs args);
    public event HouseHoldMemberScreeningSelectedEventHandler HHMemberScreeningSelected;


	#endregion

	protected void Page_Load(object sender, EventArgs e)
	{

	}

	#region Public Methods

	public void LoadHouseholdMembers(int regID)
	{
        gvHHMembers.SelectRow(-1);

        DataSet ds = svc.SelectHouseholdMemberScreeningData(regID);

		if (ds != null && ds.Tables.Count > 0)
		{
			DataTable dtMembers = ds.Tables[0];

            gvHHMembers.DataSource = dtMembers;
            gvHHMembers.DataBind();

			if (dtMembers.Rows.Count == 1)
			{
                gvHHMembers.SelectRow(0);
			}
			else if (dtMembers.Rows.Count > 1)
			{
				/// If there are multiple screening rows, filter by pending status. If there is only one with the filter applied, load that one by default
				dtMembers.DefaultView.RowFilter = string.Format("SCREENING_STATUS_ID = {0}", CON.ScreeningStatusId.InProgress);

				if (dtMembers.DefaultView.Count >= 1)
				{
                    gvHHMembers.SelectRowByDataKey<int>((int)dtMembers.DefaultView.ToTable().Rows[0]["SCREENING_ID"], "SCREENING_ID");
				}
				else
				{
                    gvHHMembers.SelectRow(0);
				}
			}

		}
		else
		{
            gvHHMembers.DataSource = null;
            gvHHMembers.DataBind();

		}
	}

	public bool ScreeningComplete(int screeningID)
	{
		bool isComplete = false;

        GridView gv = this.gvHHMembers;

		if (screeningID == 0)
		{
			return false;
		}

		foreach (GridViewRow row in gv.Rows)
		{
			if (row.RowType == DataControlRowType.DataRow)
			{
				/// TODO: Can probably get this same info by adding SCREENING_STATUS_ID to data keys and checking that instead
				///  of getting the button
				if ((int)gv.DataKeys[row.RowIndex]["SCREENING_ID"] == screeningID)
				{
					LinkButton btn = row.FindControl("btnGASelect") as LinkButton;
					if (btn != null && btn.Text == CON.ScreeningStatusName.Complete)
					{
						isComplete = true;
					}
				}
			}
		}

		return isComplete;
	}

	#endregion

	#region Event Handlers

    protected void gvHHMembers_SelectedIndexChanged(object sender, EventArgs e)
	{
        if (gvHHMembers.SelectedIndex >= 0)
		{
            int regHHMemberID = (int)gvHHMembers.SelectedDataKey.Values["REG_HOUSEHOLD_MEMBER_ID"];
			int screeningID = 0; //If all screenings are complete, this will be null.
            if (gvHHMembers.SelectedDataKey.Values["SCREENING_ID"].ToString() != "")
			{
                screeningID = (int)gvHHMembers.SelectedDataKey.Values["SCREENING_ID"];
			}
			DateTime? startDate = null;
            if (gvHHMembers.SelectedDataKey.Values["START_DATE_TIME"] != DBNull.Value)
			{
                startDate = (DateTime?)gvHHMembers.SelectedDataKey.Values["START_DATE_TIME"];
			}

			DateTime? endDate = null;
            if (gvHHMembers.SelectedDataKey.Values["END_DATE_TIME"] != DBNull.Value)
			{
                endDate = (DateTime?)gvHHMembers.SelectedDataKey.Values["END_DATE_TIME"];
			}

            string name = (string)gvHHMembers.SelectedDataKey.Values["NAME"];

			if (regHHMemberID > 0 && screeningID > 0)
			{
				if (HHMemberScreeningSelected != null)
				{
					HHMemberScreeningSelected(new HouseHoldMemberScreeningSelectedEventArgs(screeningID, startDate, endDate, regHHMemberID, name));
				}
			}
		}
		else
		{
			if (HHMemberScreeningSelected != null)
			{
                HHMemberScreeningSelected(new HouseHoldMemberScreeningSelectedEventArgs(-1, null, null, -1, null));
			}
		}
	}

	#endregion
}

	
