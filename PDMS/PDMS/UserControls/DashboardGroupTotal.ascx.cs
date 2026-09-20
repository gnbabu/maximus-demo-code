using System;
using System.Data;

public partial class UserControls_DashboardGroupTotal : System.Web.UI.UserControl
{
    public string Caption
    {
        get { return grpDashboard.Caption; }
        set { grpDashboard.Caption = value; }
    }

    public string StatusLabel
    {
        get
        {
            if (ViewState["StatusLabel"] == null) ViewState["StatusLabel"] = string.Empty;
            return ViewState["StatusLabel"].ToString();
        }
        set { ViewState["StatusLabel"] = value; }
    }

    public int TableId
    {
        get
        {
            if (ViewState["TableId"] == null) ViewState["TableId"] = 0;
            return Convert.ToInt32(ViewState["TableId"]);
        }
        set { ViewState["TableId"] = value; }
    }

    public void LoadData(DataTable dt)
    {
        gvDashboard.Columns[0].HeaderText = StatusLabel;
        gvDashboard.DataSource = dt;
        gvDashboard.DataBind();
    }
}