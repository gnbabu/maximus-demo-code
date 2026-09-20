using MAXIMUS.Core.Libraries;
using System;
using System.Data;

public partial class Workflow_WF_TestMMISInterface : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        string recordCount = (Methods.IsNumeric(txtRecordCount.Text) ? txtRecordCount.Text : "10");
        DataSet ds = new DataSet();
        ds = DataAccess.ExecuteStoredProcedure("usp_Select_SERVICE_LOCATION"); // Jira 2912

        if (ds != null && ds.Tables.Count > 0)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                using (Workflow.Process pr = new Workflow.Process(8, "11111111-1111-1111-1111-111111111111", Guid.NewGuid()))
                {
                    pr.SetProcessParameter("PartyID", row["PartyID"].ToString());
                    pr.SetProcessParameter("ServiceLocationID", row["ServiceLocationID"].ToString());
                }
            }
        }
    }
}