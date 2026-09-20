using CustomControls;
using System;
using System.Data;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_SiteVisits : System.Web.UI.UserControl
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

    public class SiteVisitSelectedEventArgs : EventArgs
    {
        public int SiteVisitID { get; set; }
        public DateTime SiteVisitStartDate { get; set; }
        public string SiteVisitTypeName { get; set; }

        public SiteVisitSelectedEventArgs(int siteVisitID, DateTime SiteVisitStartDate, string SiteVisitTypeName)
            : base()
        {
            SiteVisitID = siteVisitID;
        }

    }



    public delegate void SiteVisitSelectedEventHandler(SiteVisitSelectedEventArgs args);
    public event SiteVisitSelectedEventHandler SiteVisitSelected;

    public delegate void SiteVisitUpdatedEventHandler();
    public event SiteVisitUpdatedEventHandler SiteVisitUpdated;

   #endregion


    protected void grdSiteVisits_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdSiteVisits.SelectedIndex > -1)
        {
            int siteVisitID = (int)grdSiteVisits.SelectedDataKey.Values["SITE_VISIT_ID"];

            if (SiteVisitSelected != null)
            {
                SiteVisitSelected(new SiteVisitSelectedEventArgs(siteVisitID, (DateTime)grdSiteVisits.SelectedDataKey.Values["START_DATE"], (string)grdSiteVisits.SelectedDataKey.Values["SITE_VISIT_TYPE_NAME"]));
            }
        }
    }

    public void LoadSiteVisitList(int RegID, int selectedIndex)
    {
        if (RegID <= 0)
            return;
        
        DataSet ds1 = svc.SelectRegistrationByRegID(RegID);
        int status;
        string currentTaskName = string.Empty;
        if (Helper.HasRows(ds1))
        {
            if (int.TryParse(Helper.GetData("RegistrationStatusTypeID", ds1.Tables[0].Rows[0]), out status))
            {
                if (status == CON.RegistrationStatusTypeId.ReturnToProviderForSiteVisit)
                {
                    grdSiteVisits.Visible  = false;
                    sepSiteVisitSummary.Visible = false;
                    lblOrgScrCompleteDate.Visible = false;
                    lblOrgScrCompleteDate1.Visible = false;
                }
            }
            currentTaskName = Helper.GetString("CurrentTaskName", ds1.Tables[0].Rows[0]);
        }

        DataSet ds = svc.SelectSiteVisitData(RegID);
        DataTable dtSiteVisits = ds.Tables[0];

        if (ds != null && ds.Tables.Count > 0)
        {
            //stored proc based on complete date asc, so it will set on last one in loop
            if (Helper.HasRows(ds)&& dtSiteVisits.Rows[0]["COMPLETED_DATE"] != null )
            {
                lblOrgScrCompleteDate.Visible = true;
                lblOrgScrCompleteDate1.Visible = true;
                lblOrgScrCompleteDate1.Text = Helper.FormatDate2(dtSiteVisits.Rows[0]["COMPLETED_DATE"].ToString());
            }
            else
            {
                lblOrgScrCompleteDate.Visible = false;
                lblOrgScrCompleteDate1.Visible = false;
            }
            grdSiteVisits.DataSource = dtSiteVisits;
            grdSiteVisits.DataBind();
            //grdSiteVisits.Columns[4].Visible = Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name) && currentTaskName == CON.RegistrationTaskName.SiteVisitCompliance;
                        
            if(dtSiteVisits.Rows.Count > 0)
            {
                //OHPNM-13650 - Compliance Specialist always should see the latest site visit attempt created.
                grdSiteVisits.SelectRow(0);
            }           
        }
        else
        {
            grdSiteVisits.DataSource = null;
            grdSiteVisits.DataBind();
        }
    }


    public int  GetSelectedSiteVisitAttempt(ref int attemptStatus, ref int SitevisitAttemptCount)
    {
        int noattempt = 0;
        if (grdSiteVisits.SelectedDataKey.Values["SITE_VISIT_ATTEMPT_STATUS_ID"].ToString() != "")
        {
            attemptStatus = (int)grdSiteVisits.SelectedDataKey.Values["SITE_VISIT_ATTEMPT_STATUS_ID"];
        }

        if (grdSiteVisits.SelectedDataKey.Values["SITE_VISIT_ATTEMPT_ID"].ToString() != "")
        {
            noattempt=(int)grdSiteVisits.SelectedDataKey.Values["SITE_VISIT_ATTEMPT_ID"];
        }
        SitevisitAttemptCount = grdSiteVisits.Rows.Count;
        return noattempt;
    }

    public void ClearSelection()
    {
        grdSiteVisits.SelectRow(-1);
    }
    

}