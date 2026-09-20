using System;
using System.Data;

public partial class UserControls_SiteVisitAttempts : System.Web.UI.UserControl
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

	public class SiteVisitAttemptSelectedEventArgs : EventArgs
	{
        public int SiteVisitID { get; set; }
        public int SiteVisitAttemptID { get; set; }

		public SiteVisitAttemptSelectedEventArgs(int siteVisitID, int siteVisitAttemptID)
			: base()
		{
            SiteVisitID = siteVisitID;
            SiteVisitAttemptID = siteVisitAttemptID;
        }

	}

	public delegate void SiteVisitAttemptSelectedEventHandler(SiteVisitAttemptSelectedEventArgs args);
	public event SiteVisitAttemptSelectedEventHandler SiteVisitAttemptSelected;



	#endregion

    #region Properties

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
    #endregion

    public void LoadSiteVisitAttempts(int siteVisitID, int selectedIndex, bool selectFirstIfFound)
	{
        //TODO:  do we need an add button/image?
        SiteVisitID = siteVisitID;

        LoadSiteVisitAttemptData(siteVisitID);

        if (grdSiteVisitAttempts.Rows.Count > 0)
        {
            if (selectedIndex >= 0)
            {
                grdSiteVisitAttempts.SelectedIndex = selectedIndex;
            }
            else if (selectFirstIfFound && grdSiteVisitAttempts.Rows.Count > 0)
            {
                grdSiteVisitAttempts.SelectRow(0);
            }
        }
	}
    
    public void ClearSelection()
    {
        grdSiteVisitAttempts.SelectRow(-1);
    }

    protected void grdSiteVisitAttempts_SelectedIndexChanged(object sender, EventArgs e)
	{
        int siteVisitAttemptID = 0;
        if (grdSiteVisitAttempts.SelectedIndex > -1)
		{
            siteVisitAttemptID = (int)grdSiteVisitAttempts.SelectedDataKey.Values["SITE_VISIT_ATTEMPT_ID"];
            
		}

        if (SiteVisitAttemptSelected != null)
        {
            SiteVisitAttemptSelected(new SiteVisitAttemptSelectedEventArgs(SiteVisitID, siteVisitAttemptID));
        }
    }

    private void LoadSiteVisitAttemptData(int siteVisitID)
    {
        if (siteVisitID <= 0)
            return;

        DataSet ds = svc.SelectSiteVisitAttemptData(siteVisitID);
        if (Helper.HasRows(ds))
        {
            grdSiteVisitAttempts.DataSource = ds.Tables[0];

            grdSiteVisitAttempts.DataBind();
        }
        else
        {
            grdSiteVisitAttempts.DataSource = null;
            grdSiteVisitAttempts.DataBind();
            grdSiteVisitAttempts.SelectRow(-1);
        }
    }

}