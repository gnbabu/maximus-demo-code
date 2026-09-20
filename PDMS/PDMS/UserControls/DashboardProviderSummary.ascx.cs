using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class UserControls_DashboardProviderSummary : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string StatusLabel { get; set; }

    public void LoadData(DataTable dt)
    {
        gvDashboard.Attributes.Add("aria-label", StatusLabel);
        gvDashboard.Columns[0].HeaderText = "Provider Summary";
        gvDashboard.DataSource = dt;
        gvDashboard.DataBind();
    }

    protected void gvDashboard_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType != DataControlRowType.Header && 
        //    e.Row.RowType != DataControlRowType.Footer)
        //{
        //    HyperLink hl = (HyperLink)e.Row.FindControl("hlNewInProgress");
        //    hl.NavigateUrl = "~/Process/GroupReview.aspx?TableId=" + ((int)CON.DashboardTableType.IndividualNewReg).ToString() + "&StatusID=99&Ordinal=99";
        //}
    }
}