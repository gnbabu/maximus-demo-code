using System;
using System.Data;

public partial class UserControls_Dashboard : System.Web.UI.UserControl
{
    //public string Caption
    //{
    //    get { return grpDashboard.Caption; }
    //    set { grpDashboard.Caption = value; }
    //}
    public UserControls_Dashboard()
    {
        Title = "";
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

    public string Title { get; set; }

    public int TableId
    {
        get
        {
            if (ViewState["TableId"] == null) ViewState["TableId"] = 0;
            return Convert.ToInt32(ViewState["TableId"]);
        }
        set { ViewState["TableId"] = value; }
    }

    public Boolean IsAssigned
    {
        get
        {
            if (ViewState["IsAssigned"] == null) ViewState["IsAssigned"] = 0;
            return Convert.ToBoolean(ViewState["IsAssigned"]);
        }
        set { ViewState["IsAssigned"] = value; }
    }

    public void LoadData(DataTable dt)
    {
        gvDashboard.Attributes.Add("aria-label", this.Title + this.StatusLabel);
        gvDashboard.Columns[0].HeaderText = StatusLabel;
        gvDashboard.DataSource = dt;
        gvDashboard.DataBind();
    }

}