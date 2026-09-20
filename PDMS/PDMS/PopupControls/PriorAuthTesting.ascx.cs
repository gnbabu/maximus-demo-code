using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_PriorAuthTesting : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void RefreshData(int transactionID)
    {
        int totalResultCount = 0;
        int startRowIndex = 0;
        DataTable dt = GetData(transactionID, out totalResultCount, gvQueryPassThroughTransaction.PageSize, startRowIndex);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvQueryPassThroughTransaction.EmptyDataText = "No Transactions found.";
        }
        gvQueryPassThroughTransaction.DataSource = dt;
        gvQueryPassThroughTransaction.VirtualItemCount = totalResultCount;
        gvQueryPassThroughTransaction.DataBind();

    }

    protected void ddlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string val = ddlTransactionType.SelectedValue;
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
            gvQueryPassThroughTransaction.DataSource = null;
            gvQueryPassThroughTransaction.VirtualItemCount = 0;
            gvQueryPassThroughTransaction.DataBind();
        }
        
    }

    protected void gvQueryPassThroughTransaction_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        gvQueryPassThroughTransaction.PageIndex = e.NewPageIndex;
        gvQueryPassThroughTransaction.VirtualItemCount = 1000;
        gvQueryPassThroughTransaction.DataBind();
    }

    protected void gvQueryPassThroughTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string val = ddlTransactionType.SelectedValue;
        int valINT = 0;

        if (Int32.TryParse(val, out valINT))
        {
            valINT = Convert.ToInt32(val);
        }
        int totalResultCount = 0;
        int startRowIndex = gvQueryPassThroughTransaction.PageSize * e.NewPageIndex;
        DataTable dt = GetData(valINT, out totalResultCount, gvQueryPassThroughTransaction.PageSize, startRowIndex);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvQueryPassThroughTransaction.EmptyDataText = "No Transactions found.";
            gvQueryPassThroughTransaction.DataSource = null;
            gvQueryPassThroughTransaction.DataBind();
        }
        gvQueryPassThroughTransaction.PageIndex = e.NewPageIndex;
        gvQueryPassThroughTransaction.DataSource = dt;
        gvQueryPassThroughTransaction.VirtualItemCount = totalResultCount;
        gvQueryPassThroughTransaction.DataBind();
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

    protected void btnGenXMLPriorAuthAU_Click(object sender, EventArgs e)
    {
        string val = txtTXNQID.Text;
        string str = string.Empty;
        string str2 = string.Empty;
        int valINT = 0;

        if (Int32.TryParse(val, out valINT))
        {
            valINT = Convert.ToInt32(val);
        }
        DataSet ds = PassthroughController.GetPassThroughTransactionsPAXMLByID(valINT);
        DataTable dt = ds.Tables[0];
        DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;
        if (dr != null)
        {
            str = Methods.GetStringValue(dr, "REQUEST_PAYLOAD");
            str2 = Methods.GetStringValue(dr, "RESPONSE");
        }
        XDocument doc = null;
        if (IsValidXml(str))
        {
            doc = XDocument.Parse(str);
            txtGenXML.Text = doc.ToString();
        }
        else
        {
            txtGenXML.Text = "Error parsing the Request XML";
        }

        if (IsValidXml(str2))
        {
            doc = XDocument.Parse(str2);
            txtResponse.Text = doc.ToString();
        }
        else
        {
            txtResponse.Text = "Error parsing the Response XML";
        }
    }

    protected void btnCreateAuth_Click(object sender, EventArgs e)
    {
        string request = txtGenXML.Text;
        XmlDocument xmlDoc = new XmlDocument();
        if (IsValidXml(request))
        {
            try
            {
                xmlDoc.LoadXml(request);
            }
            catch (XmlException exception)
            {
                txtResponse.Text = "Not a valid xml: " + exception.ToString();
            }

            int AddUpdatePriorAuth = 4;
            int transactionID = InfoAccessController.InsertPASSTHROUGH_TRANSACTIONQUEUE(AddUpdatePriorAuth, DateTime.Now, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId));

            PriorAuthServiceReqRes pa = new PriorAuthServiceReqRes();
            string transactionResponse = pa.makePriorAuthWSRequestResponse(transactionID, xmlDoc, CON.PriorAuthServiceResponse.AddUpdatePriorAuth, CON.PriorAuthSearchSoapAction.AddUpdatePriorAuth);

            if (transactionResponse.Equals(CON.TransactionResult.TransactionPassed))
            {
                txtResponse.Text = InfoAccessController.GetResponsePayloadByPassThroughTransactionID(transactionID);
            }
            else
            {
                txtResponse.Text = "Transaction Failed : check logs";
            }
        }
        else
        {
            txtResponse.Text = "Error parsing the Request XML";
        }
    }

    protected void btnInquireAuth_Click(object sender, EventArgs e)
    {
        string request = txtGenXML.Text;
        XmlDocument xmlDoc = new XmlDocument();
        if (IsValidXml(request))
        {
            try
            {
                xmlDoc.LoadXml(request);
            }
            catch (XmlException exception)
            {
                txtResponse.Text = "Not a valid xml: " + exception.ToString();
            }

            int AddUpdatePriorAuth = 5;
            int transactionID = InfoAccessController.InsertPASSTHROUGH_TRANSACTIONQUEUE(AddUpdatePriorAuth, DateTime.Now, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId));

            PriorAuthServiceReqRes pa = new PriorAuthServiceReqRes();
            string transactionResponse = pa.makePriorAuthWSRequestResponse(transactionID, xmlDoc, CON.PriorAuthServiceResponse.InquirePriorAuth, CON.PriorAuthSearchSoapAction.InquirePriorAuth);

            if (transactionResponse.Equals(CON.TransactionResult.TransactionPassed))
            {
                txtResponse.Text = InfoAccessController.GetResponsePayloadByPassThroughTransactionID(transactionID);
            }
            else
            {
                txtResponse.Text = "Transaction Failed : check logs";
            }
        }
        else
        {
            txtResponse.Text = "Error parsing the Request XML";
        }
    }

    protected void btnSearchAuth_Click(object sender, EventArgs e)
    {
        string request = txtGenXML.Text;
        XmlDocument xmlDoc = new XmlDocument();
        if (IsValidXml(request))
        {
            try
            {
                xmlDoc.LoadXml(request);
            }
            catch (XmlException exception)
            {
                txtResponse.Text = "Not a valid xml: " + exception.ToString();
            }

            int AddUpdatePriorAuth = 6;
            int transactionID = InfoAccessController.InsertPASSTHROUGH_TRANSACTIONQUEUE(AddUpdatePriorAuth, DateTime.Now, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId));

            PriorAuthServiceReqRes pa = new PriorAuthServiceReqRes();
            string transactionResponse = pa.makePriorAuthWSRequestResponse(transactionID, xmlDoc, CON.PriorAuthServiceResponse.SearchPriorAuth, CON.PriorAuthSearchSoapAction.SearchPriorAuth);

            if (transactionResponse.Equals(CON.TransactionResult.TransactionPassed))
            {
                txtResponse.Text = InfoAccessController.GetResponsePayloadByPassThroughTransactionID(transactionID);
            }
            else
            {
                txtResponse.Text = "Transaction Failed : check logs";
            }
        }
        else
        {
            txtResponse.Text = "Error parsing the Request XML";
        }
    }



    private DataTable GetData(int transactionType, out int totalResultCount, int pageSize, int startRowIndex)
    {
        DataSet ds;
        totalResultCount = 0;

        // If list of IDs has been passed in, display in search results list.
        ds = PassthroughController.SelectPassThroughTransactionsByType(transactionType, pageSize, startRowIndex, true, out totalResultCount);

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }
}