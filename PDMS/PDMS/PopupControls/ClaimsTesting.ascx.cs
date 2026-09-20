using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_ClaimsTesting : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void RefreshData(int transactionID)
    {
        int totalResultCount = 0;
        int startRowIndex = 0;
        DataTable dt = GetData(transactionID, out totalResultCount, gvClaimsTransaction.PageSize, startRowIndex);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvClaimsTransaction.EmptyDataText = "No Transactions found.";
        }
        gvClaimsTransaction.DataSource = dt;
        gvClaimsTransaction.VirtualItemCount = totalResultCount;
        gvClaimsTransaction.DataBind();

    }

    protected void ddlClaimsTransactionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string val = ddlClaimsTransactionType.SelectedValue;
        int valINT = 0;

        if (Int32.TryParse(val, out valINT))
        {
            valINT = Convert.ToInt32(val);
        }
        if (valINT > 0)
        {
            RefreshData(valINT);
        }
        else
        {
            gvClaimsTransaction.DataSource = null;
            gvClaimsTransaction.VirtualItemCount = 0;
            gvClaimsTransaction.DataBind();
        }

    }

    protected void gvClaimsTransaction_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        gvClaimsTransaction.PageIndex = e.NewPageIndex;
        gvClaimsTransaction.VirtualItemCount = 1000;
        gvClaimsTransaction.DataBind();
    }

    protected void gvClaimsTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string val = ddlClaimsTransactionType.SelectedValue;

        int totalResultCount = 0;
        int startRowIndex = gvClaimsTransaction.PageSize * e.NewPageIndex;
        int valINT = 0;

        if (Int32.TryParse(val, out valINT))
        {
            valINT = Convert.ToInt32(val);
        }
        DataTable dt = GetData(valINT, out totalResultCount, gvClaimsTransaction.PageSize, startRowIndex);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvClaimsTransaction.EmptyDataText = "No Transactions found.";
            gvClaimsTransaction.DataSource = null;
            gvClaimsTransaction.DataBind();
        }
        gvClaimsTransaction.PageIndex = e.NewPageIndex;
        gvClaimsTransaction.DataSource = dt;
        gvClaimsTransaction.VirtualItemCount = totalResultCount;
        gvClaimsTransaction.DataBind();
    }

    public static bool IsValidXml(string xml)
    {
        try
        {
            XDocument.Parse(xml);
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected void btnGenXMLClaims_Click(object sender, EventArgs e)
    {
        string val = txtClaimsTXNQID.Text;
        string str = string.Empty;
        string str2 = string.Empty;
        int valINT = 0;

        if (Int32.TryParse(val, out valINT))
        {
            valINT = Convert.ToInt32(val);
        }
        DataSet ds = PassthroughController.GetPassThroughTransactionsClaimsXMLByID(valINT);
        DataTable dt = ds.Tables[0];
        DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;
        if (dr != null)
        {
            str = Methods.GetStringValue(dr, "REQUEST_PAYLOAD");
            str2 = Methods.GetStringValue(dr, "RESPONSE_PAYLOAD");
        }
        XDocument doc = null;
        if (IsValidXml(str))
        {
            doc = XDocument.Parse(str);
            txtClaimsGenXML.Text = doc.ToString();
        }
        else
        {
            txtClaimsGenXML.Text = "Error parsing the Request XML";
        }

        if (IsValidXml(str2))
        {
            doc = XDocument.Parse(str2);
            txtClaimsResponse.Text = doc.ToString();
        }
        else
        {
            txtClaimsResponse.Text = "Error parsing the Response XML";
        }
    }

    private DataTable GetData(int transactionType, out int totalResultCount, int pageSize, int startRowIndex)
    {
        DataSet ds;
        totalResultCount = 0;

        // If list of IDs has been passed in, display in search results list.
        ds = PassthroughController.SelectPassThroughClaimsTransactionsByType(transactionType, pageSize, startRowIndex, true, out totalResultCount);

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

}