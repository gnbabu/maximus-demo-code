using System;
using System.Data;

public partial class Pages_EventHistory : System.Web.UI.UserControl
{
    DataTable _dt;

    public DataTable dt
    {
        get
        {
            if (_dt == null)
            {
                _dt = new DataTable();
                _dt.Columns.Add(new DataColumn("Date"));
                _dt.Columns.Add(new DataColumn("Status"));
                _dt.Columns.Add(new DataColumn("Action"));
                _dt.Columns.Add(new DataColumn("Reason"));
                _dt.Columns.Add(new DataColumn("From"));
                _dt.Columns.Add(new DataColumn("To"));
                _dt.Columns.Add(new DataColumn("Subject"));

                DataRow dr1 = _dt.NewRow();
                dr1["Date"] = "1/2/2013";
                dr1["Status"] = "Open";
                dr1["Action"] = string.Empty;
                dr1["Reason"] = string.Empty;
                dr1["From"] = "PDMS Portal";
                dr1["To"] = "tn_provider1@medicalplace.com";
                dr1["Subject"] = "Provider Submission";
                _dt.Rows.Add(dr1);

                DataRow dr2 = _dt.NewRow();
                dr2["Date"] = "3/4/2013";
                dr2["Status"] = "In Review";
                dr2["Action"] = "Updated";
                dr2["Reason"] = "Primary changed";
                dr2["From"] = "Provider Services Manager";
                dr2["To"] = "tn_provider_admin@medicalplace.com";
                dr2["Subject"] = "Provider Change";
                _dt.Rows.Add(dr2);
            }

            return _dt;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            grdHistory.DataSource = dt;
            grdHistory.DataBind();
        }
    }
}