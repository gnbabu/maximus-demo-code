using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_MemberEligibilityTesting : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void RefreshData(string transactionID)
    {
        int totalResultCount = 0;
        int startRowIndex = 0;
        DataTable dt = GetData(transactionID, out totalResultCount, gvMemberEligibilityTransaction.PageSize, startRowIndex);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvMemberEligibilityTransaction.EmptyDataText = "No Transactions found.";
        }
        gvMemberEligibilityTransaction.DataSource = dt;
        gvMemberEligibilityTransaction.VirtualItemCount = totalResultCount;
        gvMemberEligibilityTransaction.DataBind();

    }

    protected void ddlMemberEligibilityTransactionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string val = ddlMemberEligibilityTransactionType.SelectedValue;

        if (!string.IsNullOrEmpty(val))
        {
            RefreshData(val);
        }
        else
        {
            gvMemberEligibilityTransaction.DataSource = null;
            gvMemberEligibilityTransaction.VirtualItemCount = 0;
            gvMemberEligibilityTransaction.DataBind();
        }

    }

    protected void gvMemberEligibilityTransaction_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        gvMemberEligibilityTransaction.PageIndex = e.NewPageIndex;
        gvMemberEligibilityTransaction.VirtualItemCount = 1000;
        gvMemberEligibilityTransaction.DataBind();
    }

    protected void gvMemberEligibilityTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string val = ddlMemberEligibilityTransactionType.SelectedValue;

        int totalResultCount = 0;
        int startRowIndex = gvMemberEligibilityTransaction.PageSize * e.NewPageIndex;
        DataTable dt = GetData(val, out totalResultCount, gvMemberEligibilityTransaction.PageSize, startRowIndex);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvMemberEligibilityTransaction.EmptyDataText = "No Transactions found.";
            gvMemberEligibilityTransaction.DataSource = null;
            gvMemberEligibilityTransaction.DataBind();
        }
        gvMemberEligibilityTransaction.PageIndex = e.NewPageIndex;
        gvMemberEligibilityTransaction.DataSource = dt;
        gvMemberEligibilityTransaction.VirtualItemCount = totalResultCount;
        gvMemberEligibilityTransaction.DataBind();
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

    protected void btnGenXMLMemberEligibility_Click(object sender, EventArgs e)
    {
        string val = txtMemberEligibilityTXNQID.Text;
        string str = string.Empty;
        string str2 = string.Empty;
        DataSet ds = PassthroughController.GetMemEligbTransactionsXMLByID(val);
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
            txtMemberEligibilityGenXML.Text = doc.ToString();
        }
        else
        {
            txtMemberEligibilityGenXML.Text = "Error parsing the Request XML";
        }

        if (IsValidXml(str2))
        {
            doc = XDocument.Parse(str2);
            txtMemberEligibilityResponse.Text = doc.ToString();
        }
        else
        {
            txtMemberEligibilityResponse.Text = "Error parsing the Response XML";
        }
    }

    private DataTable GetData(string transactionType, out int totalResultCount, int pageSize, int startRowIndex)
    {
        DataSet ds;
        totalResultCount = 0;

        // If list of IDs has been passed in, display in search results list.
        ds = PassthroughController.SelectMemEligbTransactionsByType(transactionType, pageSize, startRowIndex, true, out totalResultCount);

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    protected void btnMakeMemberEligibility_Click(object sender, EventArgs e)
    {
        string request = txtMemberEligibilityGenXML.Text;
        string response = txtMemberEligibilityResponse.Text;
        XmlDocument xmlDoc = new XmlDocument();
        if (IsValidXml(request))
        {
            try
            {
                xmlDoc.LoadXml(request);
            }
            catch (XmlException exception)
            {
                txtMemberEligibilityResponse.Text = "Not a valid xml: " + exception.ToString();
            }

            string pnmTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            string providerId = "0000000";
            RecipientEligibilityDA.SaveMemberEligibilityServiceReqRes("", xmlDoc.InnerXml.ToString(), response, new Guid(CON.appAdminUserId), pnmTransactionKey);

            Guid userId = Helper.GetUserId(HttpContext.Current.User.ToString());

            RecipientEligibilitySearchReqRes res = new RecipientEligibilitySearchReqRes();
            var ds = res.eligibilityWSRequestResponse(providerId, "", userId, xmlDoc, CON.EliglibityServiceSoapAction.EligVerifResponse, pnmTransactionKey, null);
            if (ds != null)
            {
                DataSet dsResp = PassthroughController.GetMemEligbTransactionsXMLByID(pnmTransactionKey, "PNM");
                DataTable dt = dsResp.Tables[0];
                DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;
                if (dr != null)
                {
                    response = Methods.GetStringValue(dr, "RESPONSE_PAYLOAD");
                }
                txtMemberEligibilityResponse.Text = response;
            }
            else
            {
                txtMemberEligibilityResponse.Text = "Transaction Failed : check logs";
            }
        }
        else
        {
            txtMemberEligibilityResponse.Text = "Error parsing the Request XML";
        }
    }
}