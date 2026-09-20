using CustomControls;
using System;
using System.Data;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_SiteVisitScreenings : System.Web.UI.UserControl
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

	public class SiteVisitScreeningSelectedEventArgs : EventArgs
	{
		public int SiteVisitScreeningID { get; set; }
		public int ScreeningActivityID { get; set; }

		public SiteVisitScreeningSelectedEventArgs(int siteVisitScreeningID, int screeningActivityID)
			: base()
		{
			SiteVisitScreeningID = siteVisitScreeningID;
			ScreeningActivityID = screeningActivityID;
		}

	}

	public delegate void SiteVisitScreeningSelectedEventHandler(SiteVisitScreeningSelectedEventArgs args);
	public event SiteVisitScreeningSelectedEventHandler SiteVisitScreeningSelected;

	#endregion
	
	public void LoadSiteVisitScreeningList(int regID, int selectedIndex)
	{
		DataSet ds = svc.SelectSiteVisitScreeningData(regID);
		if (ds != null && ds.Tables.Count > 0)
		{
			DataTable dtSiteVisitScreenings = ds.Tables[0];
			grdSiteVisitScreenings.DataSource = dtSiteVisitScreenings;
			grdSiteVisitScreenings.DataBind();

			if (selectedIndex >= 0)
			{
				grdSiteVisitScreenings.SelectedIndex = selectedIndex;
			}
			else if (grdSiteVisitScreenings.Rows.Count > 0)
			{
				//// If there are multiple screening rows, filter by pending status. If there is only one with the filter applied, load that one by default
				dtSiteVisitScreenings.DefaultView.RowFilter = string.Format("SITE_VISIT_SCREENING_STATUS_ID = {0}", CON.SiteVisitScreeningStatusID.InProgress);

				if (dtSiteVisitScreenings.DefaultView.Count >= 1)
				{
					grdSiteVisitScreenings.SelectRowByDataKey<int>((int)dtSiteVisitScreenings.DefaultView.ToTable().Rows[0]["SITE_VISIT_SCREENING_ID"], "SITE_VISIT_SCREENING_ID");
				}
				else
				{
					grdSiteVisitScreenings.SelectRow(0);
				}
			}
		}
		else
		{
			grdSiteVisitScreenings.DataSource = null;
			grdSiteVisitScreenings.DataBind();
		}
	}

	#region Event Handlers
	

	protected void grdSiteVisitScreenings_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (grdSiteVisitScreenings.SelectedIndex > -1)
		{
			int siteVisitScreeningID = (int)grdSiteVisitScreenings.SelectedDataKey.Values["SITE_VISIT_SCREENING_ID"];
			int screeningActivityID = (int)grdSiteVisitScreenings.SelectedDataKey.Values["SCREENING_ACTIVITY_ID"];

			if (SiteVisitScreeningSelected != null)
			{
				SiteVisitScreeningSelected(new SiteVisitScreeningSelectedEventArgs(siteVisitScreeningID, screeningActivityID));
			}
		}
	}

	#endregion

	public void ClearSelection()
	{
		grdSiteVisitScreenings.SelectRow(-1);
	}
}