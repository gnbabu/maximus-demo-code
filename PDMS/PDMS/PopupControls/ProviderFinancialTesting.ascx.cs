using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_ProviderFinancialTesting : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void RefreshData(string transactionID)
    {
        int totalResultCount = 0;
        int startRowIndex = 0;
        DataTable dt = GetData(transactionID, out totalResultCount, gvProviderFinancialTransaction.PageSize, startRowIndex);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvProviderFinancialTransaction.EmptyDataText = "No Transactions found.";
        }
        gvProviderFinancialTransaction.DataSource = dt;
        gvProviderFinancialTransaction.VirtualItemCount = totalResultCount;
        gvProviderFinancialTransaction.DataBind();

    }

    protected void ddlPFTransactionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string val = ddlPFTransactionType.SelectedValue;
        
        if (!string.IsNullOrEmpty(val))
        {
            RefreshData(val);
        }
        else
        {
            gvProviderFinancialTransaction.DataSource = null;
            gvProviderFinancialTransaction.VirtualItemCount = 0;
            gvProviderFinancialTransaction.DataBind();
        }

    }

    protected void gvProviderFinancialTransaction_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        gvProviderFinancialTransaction.PageIndex = e.NewPageIndex;
        gvProviderFinancialTransaction.VirtualItemCount = 1000;
        gvProviderFinancialTransaction.DataBind();
    }

    protected void gvProviderFinancialTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string val = ddlPFTransactionType.SelectedValue;
        
        int totalResultCount = 0;
        int startRowIndex = gvProviderFinancialTransaction.PageSize * e.NewPageIndex;
        DataTable dt = GetData(val, out totalResultCount, gvProviderFinancialTransaction.PageSize, startRowIndex);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvProviderFinancialTransaction.EmptyDataText = "No Transactions found.";
            gvProviderFinancialTransaction.DataSource = null;
            gvProviderFinancialTransaction.DataBind();
        }
        gvProviderFinancialTransaction.PageIndex = e.NewPageIndex;
        gvProviderFinancialTransaction.DataSource = dt;
        gvProviderFinancialTransaction.VirtualItemCount = totalResultCount;
        gvProviderFinancialTransaction.DataBind();
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

    protected void btnGenXMLProvFinancial_Click(object sender, EventArgs e)
    {
        string val = txtPFTXNQID.Text;
        string str = string.Empty;
        string str2 = string.Empty;
        DataSet ds = PassthroughController.Get1099TransactionsPAXMLByID(val);
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
            txtPFGenXML.Text = doc.ToString();
        }
        else
        {
            txtPFGenXML.Text = "Error parsing the Request XML";
        }

        if (IsValidXml(str2))
        {
            doc = XDocument.Parse(str2);
            txtPFResponse.Text = doc.ToString();
        }
        else
        {
            txtPFResponse.Text = "Error parsing the Response XML";
        }
    }

    protected void btn1099Inquiry_Click(object sender, EventArgs e)
    {
        string request = txtPFGenXML.Text;
        string response = txtPFResponse.Text;
        XmlDocument xmlDoc = new XmlDocument();
        if (IsValidXml(request))
        {
            try
            {
                xmlDoc.LoadXml(request);
            }
            catch (XmlException exception)
            {
                txtPFResponse.Text = "Not a valid xml: " + exception.ToString();
            }

            string pnmTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            string providerId = "0000000";
            InfoAccessController.SaveFinancialServiceXMLRecord(providerId, pnmTransactionKey, CON.ProviderFinancial.Inquire1099, xmlDoc.InnerXml.ToString(), new Guid(CON.providerFinancialId));

            FinancialServiceReqRes pf = new FinancialServiceReqRes();
            DataSet ds = pf.make1099WSRequestResponse(providerId, xmlDoc, CON.FinancialServiceSoapAction.Inquire1099, pnmTransactionKey);
            if (ds != null)
            {
                DataSet dsResp = PassthroughController.Get1099TransactionsPAXMLByID(pnmTransactionKey, "PNM");
                DataTable dt = dsResp.Tables[0];
                DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;
                if (dr != null)
                {
                    response = Methods.GetStringValue(dr, "RESPONSE_PAYLOAD");
                }
                txtPFResponse.Text = response;
            }
            else
            {
                txtPFResponse.Text = "Transaction Failed : check logs";
            }
        }
        else
        {
            txtPFResponse.Text = "Error parsing the Request XML";
        }
    }

    protected void btn1099History_Click(object sender, EventArgs e)
    {
        string request = txtPFGenXML.Text;
        string response = txtPFResponse.Text;
        XmlDocument xmlDoc = new XmlDocument();
        if (IsValidXml(request))
        {
            try
            {
                xmlDoc.LoadXml(request);
            }
            catch (XmlException exception)
            {
                txtPFResponse.Text = "Not a valid xml: " + exception.ToString();
            }

            string pnmTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            string providerId = "0000000";
            InfoAccessController.SaveFinancialServiceXMLRecord(providerId, pnmTransactionKey, CON.ProviderFinancial.History1099, xmlDoc.InnerXml.ToString(), new Guid(CON.providerFinancialId));

            FinancialServiceReqRes pf = new FinancialServiceReqRes();
            DataSet ds = pf.makeTransHistWSRequestResponse(providerId, xmlDoc, CON.FinancialServiceSoapAction.InquireTransactionHistory, pnmTransactionKey);
            if (ds != null)
            {
                DataSet dsResp = PassthroughController.Get1099TransactionsPAXMLByID(pnmTransactionKey, "PNM");
                DataTable dt = dsResp.Tables[0];
                DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;
                if (dr != null)
                {
                    response = Methods.GetStringValue(dr, "RESPONSE_PAYLOAD");
                }
                txtPFResponse.Text = response;
            }
            else
            {
                txtPFResponse.Text = "Transaction Failed : check logs";
            }
        }
        else
        {
            txtPFResponse.Text = "Error parsing the Request XML";
        }
    }

    private DataTable GetData(string transactionType, out int totalResultCount, int pageSize, int startRowIndex)
    {
        DataSet ds;
        totalResultCount = 0;

        // If list of IDs has been passed in, display in search results list.
        ds = PassthroughController.Select1099TransactionsByType(transactionType, pageSize, startRowIndex, true, out totalResultCount);

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

}