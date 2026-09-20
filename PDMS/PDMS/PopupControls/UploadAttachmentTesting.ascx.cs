using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class PopupControls_UploadAttachmentTesting : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void RefreshData(string memberid,string providerid)
    {
        int totalResultCount = 0;
        int startRowIndex = 0;
        DataTable dt = GetData(memberid, providerid, out totalResultCount, gvUploadAttachmentStatus.PageSize, startRowIndex);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvUploadAttachmentStatus.EmptyDataText = "No Uploads found.";
        }
        gvUploadAttachmentStatus.DataSource = dt;
        gvUploadAttachmentStatus.VirtualItemCount = totalResultCount;
        gvUploadAttachmentStatus.DataBind();

    }
    protected void btnGetUploadStatus_Click(object sender, EventArgs e)
    {
        string memberid = txtMemberID.Text;
        string providerid = txtProviderID.Text;
        string str2 = string.Empty;

        RefreshData(memberid, providerid);

    }







    private DataTable GetData(string memberid, string providerid, out int totalResultCount, int pageSize, int startRowIndex)
    {
        DataSet ds;
        totalResultCount = 0;

        // If list of IDs has been passed in, display in search results list.
        ds = UploadAttachmentController.GetUploadAttachmentStatus(memberid,providerid,  pageSize, startRowIndex, true, out totalResultCount);

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }
}