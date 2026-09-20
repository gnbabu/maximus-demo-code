using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_DMERegisteredAgentHistory : System.Web.UI.UserControl
{
    public void LoadData(DataTable dt)
    {
        gvRegisteredAgentHistory.DataSource = dt;
        gvRegisteredAgentHistory.DataBind();
    }


    protected void gvRegisteredAgentHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[9].Text = Helper.FormatPhone(e.Row.Cells[9].Text);
            e.Row.Cells[10].Text = Helper.FormatPhone(e.Row.Cells[10].Text);
        }
    }
}