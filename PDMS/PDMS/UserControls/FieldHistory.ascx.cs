using System;
using System.Data;

public partial class UserControls_FieldHistory : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateFieldHistoryGrid();
        }
    }

    private void PopulateFieldHistoryGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("FieldValue", typeof(string)));
        dt.Columns.Add(new DataColumn("UserID", typeof(string)));
        dt.Columns.Add(new DataColumn("DateSubmitted", typeof(string)));
        dt.Columns.Add(new DataColumn("ProviderStatus", typeof(string)));
        DataRow dr = dt.NewRow();
        dr["FieldValue"] = "Individual";
        dr["UserID"] = "vols1";
        dr["DateSubmitted"] = "5/1/2013";
        dr["ProviderStatus"] = "New";
        dt.Rows.Add(dr);
        DataRow dr1 = dt.NewRow();
        dr1["FieldValue"] = "Group";
        dr1["UserID"] = "LookoutMtn";
        dr1["DateSubmitted"] = "5/21/2013";
        dr1["ProviderStatus"] = "Active";
        dt.Rows.Add(dr1);
        grdHistory.DataSource = dt;
        grdHistory.DataBind();
    }

}