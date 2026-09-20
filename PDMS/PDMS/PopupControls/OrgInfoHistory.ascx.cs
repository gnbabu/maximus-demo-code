using System.Data;

public partial class PopupControls_OrgInfoHistory : System.Web.UI.UserControl
{
    public void LoadData(DataTable dt)
    {
        grdOrgInfoHistory.DataSource = dt;
        grdOrgInfoHistory.DataBind();
    }
}