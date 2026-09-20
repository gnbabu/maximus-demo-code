using CustomControls;
using System;
using System.Data;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_ScreeningGroupAffiliations : System.Web.UI.UserControl
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


	public class AffiliationScreeningSelectedEventArgs : UserControls_ScreeningHistory.ScreeningSelectedEventArgs
	{
		public int RegAffiliationID { get; set; }
		public string Name { get; set; }

		public AffiliationScreeningSelectedEventArgs(int screeningID, DateTime? screeningStartDate, DateTime? screeningEndDate, int regAffiliationID, string name)
			: base(screeningID, screeningStartDate, screeningEndDate)
		{
			RegAffiliationID = regAffiliationID;
			Name = name;
		}
	}

	public delegate void AffiliationScreeningSelectedEventHandler(AffiliationScreeningSelectedEventArgs args);
	public event AffiliationScreeningSelectedEventHandler AffiliationScreeningSelected;
    

	#endregion

	

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
	}

	#region Helper Methods


	public void LoadGroupAffiliations(int regID)
	{
		grdGroupAffiliations.SelectRow(-1);

		DataSet ds = svc.SelectAffiliationsScreeningData(regID);

		if (ds != null && ds.Tables.Count > 0)
		{
			grdGroupAffiliations.DataSource = ds.Tables[0];
			grdGroupAffiliations.DataBind();

            DataTable dtAffiliations = ds.Tables[0];

			if (dtAffiliations.Rows.Count == 1)
			{
				grdGroupAffiliations.SelectRow(0);
			}
			else if (dtAffiliations.Rows.Count > 1)
			{

                //TODO:  this lo
				/// If there are multiple screening rows, filter by pending status. If there is only one with the filter applied, load that one by default
				dtAffiliations.DefaultView.RowFilter = string.Format("SCREENING_STATUS_ID = {0}", CON.ScreeningStatusId.InProgress);

				if (dtAffiliations.DefaultView.Count >= 1)
				{
					grdGroupAffiliations.SelectRowByDataKey<int>((int)dtAffiliations.DefaultView.ToTable().Rows[0]["SCREENING_ID"], "SCREENING_ID");
				}
				else
				{
					grdGroupAffiliations.SelectRow(0);
				}
			}

		}
		else
		{
			grdGroupAffiliations.DataSource = null;
			grdGroupAffiliations.DataBind();

		}
	}

    public bool ScreeningComplete(int screeningID)
    {
        bool isComplete = false;

        GridView gv = this.grdGroupAffiliations;

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


	protected void grdGroupAffiliations_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (grdGroupAffiliations.SelectedIndex >= 0)
		{
			int regAffiliationID = (int)grdGroupAffiliations.SelectedDataKey.Values["REG_AFFILIATION_ID"];
            int screeningID = 0; //If all screenings are complete, this will be null.
            if (grdGroupAffiliations.SelectedDataKey.Values["SCREENING_ID"].ToString() != "")
            {
                 screeningID = (int)grdGroupAffiliations.SelectedDataKey.Values["SCREENING_ID"];
            }
			DateTime? startDate = null;
            if (grdGroupAffiliations.SelectedDataKey.Values["START_DATE_TIME"] != DBNull.Value)
			{
                startDate = (DateTime?)grdGroupAffiliations.SelectedDataKey.Values["START_DATE_TIME"];
			}


			DateTime? endDate = null;
            if (grdGroupAffiliations.SelectedDataKey.Values["END_DATE_TIME"] != DBNull.Value)
			{
                endDate = (DateTime?)grdGroupAffiliations.SelectedDataKey.Values["END_DATE_TIME"];
			}

			string name = (string)grdGroupAffiliations.SelectedDataKey.Values["NAME"];

            if (regAffiliationID > 0 && screeningID > 0)
            {
                if (AffiliationScreeningSelected != null)
                {
					AffiliationScreeningSelected(new AffiliationScreeningSelectedEventArgs(screeningID, startDate, endDate, regAffiliationID, name));
                }
            }
		}
		else
		{
			if (AffiliationScreeningSelected != null)
			{
				AffiliationScreeningSelected(new AffiliationScreeningSelectedEventArgs(-1, null, null, -1, null));
			}
		}
	}
}