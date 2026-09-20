using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using NPOI.SS.Formula.Functions;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_HospiceTesting : System.Web.UI.UserControl
{
    string url = AppSettings.Get("HospiceWSURL", CON.WebServiceURI.HospiceService);
    string wsCertificateName = AppSettings.Get("HospiceWSCertificateName");
    string wsHost = AppSettings.Get("HospiceWSHost");
    string wsUserName = AppSettings.Get("HospiceWSUserName");

    string urlAtt = AppSettings.Get("DocumentWSURL", CON.WebServiceURI.DocumentService);
    string wsHostAtt = AppSettings.Get("DocumentWSHost");
    string wsCertificateNameAtt = AppSettings.Get("DocumentWSCertificateName");
    string wsUserNameAtt = AppSettings.Get("DocumentWSUserName");

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void RefreshData(string transactionID)
    {
        int totalResultCount = 0;
        int startRowIndex = 0;
        DataTable dt = GetData(transactionID, out totalResultCount, gvHospiceTransaction.PageSize, startRowIndex);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvHospiceTransaction.EmptyDataText = "No Transactions found.";
        }
        gvHospiceTransaction.DataSource = dt;
        gvHospiceTransaction.VirtualItemCount = totalResultCount;
        gvHospiceTransaction.DataBind();

    }

    protected void ddlHospiceTransactionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string val = ddlHospiceTransactionType.SelectedValue;

        if (!string.IsNullOrEmpty(val))
        {
            RefreshData(val);
        }
        else
        {
            gvHospiceTransaction.DataSource = null;
            gvHospiceTransaction.VirtualItemCount = 0;
            gvHospiceTransaction.DataBind();
        }

    }

    protected void gvHospiceTransaction_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        gvHospiceTransaction.PageIndex = e.NewPageIndex;
        gvHospiceTransaction.VirtualItemCount = 1000;
        gvHospiceTransaction.DataBind();
    }

    protected void gvHospiceTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string val = ddlHospiceTransactionType.SelectedValue;

        int totalResultCount = 0;
        int startRowIndex = gvHospiceTransaction.PageSize * e.NewPageIndex;
        DataTable dt = GetData(val, out totalResultCount, gvHospiceTransaction.PageSize, startRowIndex);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvHospiceTransaction.EmptyDataText = "No Transactions found.";
            gvHospiceTransaction.DataSource = null;
            gvHospiceTransaction.DataBind();
        }
        gvHospiceTransaction.PageIndex = e.NewPageIndex;
        gvHospiceTransaction.DataSource = dt;
        gvHospiceTransaction.VirtualItemCount = totalResultCount;
        gvHospiceTransaction.DataBind();
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

    protected void btnGenXMLHospice_Click(object sender, EventArgs e)
    {
        string val = txtHospiceTXNQID.Text;
        string str = string.Empty;
        string str2 = string.Empty;
        DataSet ds = PassthroughController.GetHospiceTransactionsXMLByID(val);
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
            txtHospiceGenXML.Text = doc.ToString();
        }
        else
        {
            txtHospiceGenXML.Text = "Error parsing the Request XML";
        }

        if (IsValidXml(str2))
        {
            doc = XDocument.Parse(str2);
            txtHospiceResponse.Text = doc.ToString();
        }
        else
        {
            txtHospiceResponse.Text = "Error parsing the Response XML";
        }
    }

    private DataTable GetData(string transactionType, out int totalResultCount, int pageSize, int startRowIndex)
    {
        DataSet ds;
        totalResultCount = 0;

        // If list of IDs has been passed in, display in search results list.
        ds = PassthroughController.SelectHospiceTransactionsByType(transactionType, pageSize, startRowIndex, true, out totalResultCount);

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    protected void btnSubmitHospice_Click(object sender, EventArgs e)
    {
        string request = txtHospiceGenXML.Text;
        string response = txtHospiceResponse.Text;
        XmlDocument xmlDoc = new XmlDocument();
        if (IsValidXml(request))
        {
            try
            {
                xmlDoc.LoadXml(request);
            }
            catch (XmlException exception)
            {
                txtHospiceResponse.Text = "Not a valid xml: " + exception.ToString();
            }
            string client_secret = "";
            string secretName = "HospiceWSPassword_2_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
                string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                secretResult.Wait();

                client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
            }
            catch (Exception ex)
            {
                Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
                client_secret = "NO SECRET CONNECTIVITY";
                client_secret += " ~|~ ";
                client_secret += AppSettings.Get("SecretsRegion");
                client_secret += " ~|~ ";
                client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                client_secret += " ~|~ ";
                client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                client_secret += " ~|~ ";
                client_secret += ex.Message;
                log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            }

            string pnmTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            //HospiceEnrollmentDA.SaveHospiceEnrollmentServiceReqRes("", "AddUpdateHospice", xmlDoc.InnerXml.ToString(), response, "", "", "", "", new Guid(CON.appAdminUserId), "", pnmTransactionKey);
            string ds = ServiceAgentHelper.MakeServiceCall(xmlDoc, url, @"http://service.caremgmt.fi/HospiceService/AddUpdateHospice", wsUserName, client_secret, wsCertificateName, wsHost, "");
            if (ds != null)
            {
                DataSet dsResp = PassthroughController.GetHospiceTransactionsXMLByID(pnmTransactionKey, "PNM");
                DataTable dt = dsResp.Tables[0];
                DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;
                if (dr != null)
                {
                    response = Methods.GetStringValue(dr, "RESPONSE_PAYLOAD");
                }
                txtHospiceResponse.Text = response;
            }
            else
            {
                txtHospiceResponse.Text = "Transaction Failed : check logs";
            }
        }
        else
        {
            txtHospiceResponse.Text = "Error parsing the Request XML";
        }
    }

    protected void btnInquiretHospice_Click(object sender, EventArgs e)
    {
        string request = txtHospiceGenXML.Text;
        string response = txtHospiceResponse.Text;
        XmlDocument xmlDoc = new XmlDocument();
        if (IsValidXml(request))
        {
            try
            {
                xmlDoc.LoadXml(request);
            }
            catch (XmlException exception)
            {
                txtHospiceResponse.Text = "Not a valid xml: " + exception.ToString();
            }

            string client_secret = "";
            string secretName = "HospiceWSPassword_2_OH_PNM_";
            try
            {
                var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                secretResult.Wait();

                client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
            }
            catch (Exception ex)
            {
                Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
                client_secret = "NO SECRET CONNECTIVITY";
                client_secret += " ~|~ ";
                client_secret += AppSettings.Get("SecretsRegion");
                client_secret += " ~|~ ";
                client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                client_secret += " ~|~ ";
                client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                client_secret += " ~|~ ";
                client_secret += ex.Message;
                log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            }

            string pnmTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            //HospiceEnrollmentDA.SaveHospiceEnrollmentServiceReqRes("", "InquireHospice", xmlDoc.InnerXml.ToString(), response, "", "", "", "", new Guid(CON.appAdminUserId), "", pnmTransactionKey);
            string ds = ServiceAgentHelper.MakeServiceCall(xmlDoc, url, @"http://service.caremgmt.fi/HospiceService/InquireHospice", wsUserName, client_secret, wsCertificateName, wsHost, "");
            if (ds != null)
            {
                DataSet dsResp = PassthroughController.GetHospiceTransactionsXMLByID(pnmTransactionKey, "PNM");
                DataTable dt = dsResp.Tables[0];
                DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;
                if (dr != null)
                {
                    response = Methods.GetStringValue(dr, "RESPONSE_PAYLOAD");
                }
                txtHospiceResponse.Text = response;
            }
            else
            {
                txtHospiceResponse.Text = "Transaction Failed : check logs";
            }
        }
        else
        {
            txtHospiceResponse.Text = "Error parsing the Request XML";
        }
    }

    protected void btnSearchHospice_Click(object sender, EventArgs e)
    {
        string request = txtHospiceGenXML.Text;
        string response = txtHospiceResponse.Text;
        XmlDocument xmlDoc = new XmlDocument();
        if (IsValidXml(request))
        {
            try
            {
                xmlDoc.LoadXml(request);
            }
            catch (XmlException exception)
            {
                txtHospiceResponse.Text = "Not a valid xml: " + exception.ToString();
            }

            string client_secret = "";
            string secretName = "HospiceWSPassword_2_OH_PNM_";
            try
            {
                var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                secretResult.Wait();

                client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
            }
            catch (Exception ex)
            {
                Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
                client_secret = "NO SECRET CONNECTIVITY";
                client_secret += " ~|~ ";
                client_secret += AppSettings.Get("SecretsRegion");
                client_secret += " ~|~ ";
                client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                client_secret += " ~|~ ";
                client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                client_secret += " ~|~ ";
                client_secret += ex.Message;
                log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            }

            string pnmTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            //HospiceEnrollmentDA.SaveHospiceEnrollmentServiceReqRes("", "SearchHospice", xmlDoc.InnerXml.ToString(), response, "", "", "", "", new Guid(CON.appAdminUserId), "", pnmTransactionKey);
            string ds = ServiceAgentHelper.MakeServiceCall(xmlDoc, url, @"http://service.caremgmt.fi/HospiceService/SearchHospice", wsUserName, client_secret, wsCertificateName, wsHost, "");
            if (ds != null)
            {
                DataSet dsResp = PassthroughController.GetHospiceTransactionsXMLByID(pnmTransactionKey, "PNM");
                DataTable dt = dsResp.Tables[0];
                DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;
                if (dr != null)
                {
                    response = Methods.GetStringValue(dr, "RESPONSE_PAYLOAD");
                }
                txtHospiceResponse.Text = response;
            }
            else
            {
                txtHospiceResponse.Text = "Transaction Failed : check logs";
            }
        }
        else
        {
            txtHospiceResponse.Text = "Error parsing the Request XML";
        }
    }

    protected void btnSubmitAttachment_Click(object sender, EventArgs e)
    {
        string request = txtHospiceGenXML.Text;
        string response = txtHospiceResponse.Text;
        XmlDocument xmlDoc = new XmlDocument();
        if (IsValidXml(request))
        {
            try
            {
                xmlDoc.LoadXml(request);
            }
            catch (XmlException exception)
            {
                txtHospiceResponse.Text = "Not a valid xml: " + exception.ToString();
            }

            string client_secret = "";
            string secretName = "DocumentWSPassword_OH_PNM_";
            try
            {
                var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                secretResult.Wait();

                client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
            }
            catch (Exception ex)
            {
                Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
                client_secret = "NO SECRET CONNECTIVITY";
                client_secret += " ~|~ ";
                client_secret += AppSettings.Get("SecretsRegion");
                client_secret += " ~|~ ";
                client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                client_secret += " ~|~ ";
                client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                client_secret += " ~|~ ";
                client_secret += ex.Message;
                log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            }

            string pnmTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            //HospiceEnrollmentDA.SaveHospiceEnrollmentServiceReqRes("", "SendAttachment", xmlDoc.InnerXml.ToString(), response, "", "", "", "", new Guid(CON.appAdminUserId), "", pnmTransactionKey);
            string ds = ServiceAgentHelper.MakeServiceCall(xmlDoc, urlAtt, "\"http://mes.gov/sendAttachment\"", wsUserNameAtt, client_secret, wsCertificateNameAtt, wsHostAtt, "");
            if (ds != null)
            {
                DataSet dsResp = PassthroughController.GetHospiceTransactionsXMLByID(pnmTransactionKey, "PNM");
                DataTable dt = dsResp.Tables[0];
                DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;
                if (dr != null)
                {
                    response = Methods.GetStringValue(dr, "RESPONSE_PAYLOAD");
                }
                txtHospiceResponse.Text = response;
            }
            else
            {
                txtHospiceResponse.Text = "Transaction Failed : check logs";
            }
        }
        else
        {
            txtHospiceResponse.Text = "Error parsing the Request XML";
        }
    }
}