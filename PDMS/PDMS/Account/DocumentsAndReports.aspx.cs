using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_DocumentsAndReports : System.Web.UI.Page
{
    #region (svc) PDMS Service
    PDMSService.PDMSServiceClient _svc;
    PDMSService.PDMSServiceClient svc
    {
        get
        {
            if (_svc == null)
                _svc = new PDMSService.PDMSServiceClient();

            return _svc;
        }
    }
    #endregion

    private void RecordException(Exception ex, string methodName, bool throwEx)
    {
        string errMsg = ex.Message + " [" + methodName + "]";
        if (throwEx) throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception(errMsg));
        else CoreException.ThrowException(new Exception(errMsg));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ServerError.Visible = false;
        if (!IsPostBack)
        {

            #region "TN Proprietory eRA"
            var minDate = DateTime.Now.AddDays(int.Parse(AppSettings.Get("eRA-DateRangeMin"))).ToString("MM/dd/yyyy");
            var maxDate = DateTime.Now.AddDays(int.Parse(AppSettings.Get("eRA-DateRangeMax"))).ToString("MM/dd/yyyy");

            string script = "$(document).ready(function () { $('[id*=btnSubmit]').click(); });";
            ClientScript.RegisterStartupScript(this.GetType(), "load", script, true);

            txtFromDate.Text = DateTime.Now.AddDays(-7).ToString("MM/dd/yyyy");
            txtToDate.Text = DateTime.Now.ToString("MM/dd/yyyy");

            spanMID.Visible =
            txtMedicaidId.Visible =
            rvMedicaid.Enabled = !(Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name));
            ddlx12eRAMedicaidIdList.Visible = (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name));
            txtMedcaidIdx12.Visible = !(Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name));

            FromRangeValidator.MinimumValue = minDate;
            FromRangeValidator.MaximumValue = maxDate;
            FromRangeValidator.ErrorMessage = "Date should be between " + minDate + " and " + maxDate;
            ToRangeValidator.ErrorMessage = "Date should be between " + minDate + " and " + maxDate;
            ToRangeValidator.MaximumValue = maxDate;
            ToRangeValidator.MinimumValue = minDate;

            logAccessEntry(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), "", "", "", 0, MAXIMUS.Core.Libraries.Enumerations.LogAccessType.PageLoad, getNPIFromUserID(Helper.GetUserId(HttpContext.Current.User.Identity.Name)));
            #endregion

            #region "X12 eRA"

            ddlx12eRAMedicaidIdList.DataSource = svc.GetMedicaidIdsByUserName(HttpContext.Current.User.Identity.Name);
            ddlx12eRAMedicaidIdList.DataBind();
            btnX12eRADownload.ForeColor = System.Drawing.Color.White;
            #endregion
        }
    }

    #region "TN Proprietry eRA"
    //TODO:  get npi by userid will not work for one user to multiple provider environment
    private string getNPIFromUserID(Guid userId)
    {
        string NPI = null;
        if (ViewState["NPI"] != null)
            NPI = ViewState["NPI"].ToString();
        else
        {
            NPI = Helper.GetNPI(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString()).ToString();
            ViewState["NPI"] = NPI;
        }
        return NPI;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gvDocs.Visible = true;
        SetViewState(GetReportList());

        int reportDocId = 0;
        logAccessEntry(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), "", txtMedicaidId.Text.Trim(), "", reportDocId, MAXIMUS.Core.Libraries.Enumerations.LogAccessType.SearchClick, getNPIFromUserID(Helper.GetUserId(HttpContext.Current.User.Identity.Name)));

        gvDocs.DataSource = GetViewState();
        gvDocs.DataBind();

    }

    private void logAccessEntry(string UserId, string documentId, string provideId, string docFormat, int log_Access_TypeId, MAXIMUS.Core.Libraries.Enumerations.LogAccessType access, string NPI)
    {

        svc.LogAccess(UserId,
                        documentId,
                        provideId,
                       docFormat,
                        log_Access_TypeId, access, NPI);


    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = DateTime.Now.AddDays(-7).ToString("MM/dd/yyyy");
        txtToDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
        txtMedicaidId.Text = string.Empty;
        GridViewSortDirection = string.Empty;
        GridViewSortExpression = string.Empty;
        SetViewState(null);
        gvDocs.Visible = false;
    }

    private DataTable GetReportList()
    {
        Page.Validate("vGroupeRA");
        if (!Page.IsValid) return null;

        DataTable documentsList = new DataTable();
        documentsList.Columns.Add("DocType");
        documentsList.Columns.Add("DocTypeId");
        documentsList.Columns.Add("ReportDate");
        documentsList.Columns.Add("MedicaidId");
        documentsList.Columns.Add("DocId");
        documentsList.Columns.Add("ProviderId");

        try
        {
            List<string> providerId = (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name)) ? svc.GetMedicaidIdsByUserName(HttpContext.Current.User.Identity.Name) : svc.GetMedicaidIdsByMedicaidId(txtMedicaidId.Text.Trim());// ; put here to make debugging across different VPN connections easier.
            if (providerId.Count > 0)
            {
                //TNReport.GetReportList(documentsList, providerId.ToArray(), txtFromDate.Text, txtToDate.Text);
            }
            else
            {
                if (!Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
                    gvDocs.EmptyDataText = "No matching records found.";
                else
                    gvDocs.EmptyDataText = "You may not retrieve reports until Registration is complete.";
            }
        }
        catch (WebException ex)
        {
            RecordException(ex, MethodInfo.GetCurrentMethod().Name, false);
            ShowError(ex.Message);
        }
        catch (EndpointNotFoundException ex)
        {
            RecordException(ex, MethodInfo.GetCurrentMethod().Name, false);
            ShowError(ex.Message);
        }
        catch (Exception ex)
        {
            RecordException(ex, MethodInfo.GetCurrentMethod().Name, false);
            if (ex.Message.Contains("The system cannot find the file specified"))
            {
                ShowError("Please try again.");
            }
            else
                ShowError(ex.Message);
        }

        return documentsList;
    }

    private DataTable GetViewState()
    {
        //Gets the ViewState
        return (DataTable)ViewState["myDataSet"];
    }

    private void SetViewState(DataTable myDataSet)
    {
        //Sets the ViewState
        ViewState["myDataSet"] = myDataSet;
    }

    //Gets or Sets the GridView SortDirection Property
    private string GridViewSortDirection
    {
   
        get { return ViewState["SortDirection"] == null ? "ASC" : ViewState["SortDirection"].ToString(); }
        
        set
        {
            ViewState["SortDirection"] = value;
        }
    }
    //Gets or Sets the GridView SortExpression Property
    private string GridViewSortExpression
    {
        get
        {
            return ViewState["SortExpression"] == null ? "MedicaidID" : ViewState["SortExpression"].ToString();
        }
        set
        {
            ViewState["SortExpression"] = value;
        }
    }

    //Toggles between the Direction of the Sorting
    private string GetSortDirection()
    {
        switch (GridViewSortDirection)
        {
            case "ASC":
                GridViewSortDirection = "DESC";
                break;
            case "DESC":
                GridViewSortDirection = "ASC";
                break;
        }
        return GridViewSortDirection;
    }

    protected void myGridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable myDataSet = GetViewState();
        GridViewSortExpression = e.SortExpression;

        //Gets the Pageindex of the GridView.
        int iPageIndex = gvDocs.PageIndex;
        gvDocs.DataSource = SortDataTable(myDataSet, false);
        gvDocs.DataBind();
        gvDocs.PageIndex = iPageIndex;
    }

    //Sorts the ResultSet based on the SortExpression and the Selected Column.
    protected DataView SortDataTable(DataTable myDataTable, bool isPageIndexChanging)
    {
        if (myDataTable != null)
        {
            DataView myDataView = new DataView(myDataTable);
            if (GridViewSortExpression != string.Empty)
            {
                if (isPageIndexChanging)
                {
                    myDataView.Sort = string.Format("{0} {1}",
                    GridViewSortExpression, GridViewSortDirection);
                }
                else
                {
                    myDataView.Sort = string.Format("{0} {1}",
                    GridViewSortExpression, GetSortDirection());
                }
            }
            return myDataView;
        }
        else
        {
            return new DataView();
        }
    }


    protected void gvDocs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable myDataSet = GetViewState();
        gvDocs.DataSource = SortDataTable(myDataSet, true);
        gvDocs.PageIndex = e.NewPageIndex;
        gvDocs.DataBind();
    }

    protected void gvDocs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = 0;
        int.TryParse(e.CommandArgument.ToString(), out index);

        switch (e.CommandName)
        {
            case "GetReportText":
                logAccessEntry(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), gvDocs.DataKeys[index].Values["DocId"].ToString(), gvDocs.DataKeys[index].Values["MedicaidID"].ToString(),
                    "Text", Convert.ToInt32(gvDocs.DataKeys[index].Values["DocTypeId"]), MAXIMUS.Core.Libraries.Enumerations.LogAccessType.Download, getNPIFromUserID(Helper.GetUserId(HttpContext.Current.User.Identity.Name)));
                GetReportText(gvDocs.DataKeys[index].Values["DocId"].ToString());
                break;
            case "GetReportPDF":
                logAccessEntry(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), gvDocs.DataKeys[index].Values["DocId"].ToString(), gvDocs.DataKeys[index].Values["MedicaidID"].ToString(),
                    "PDF", Convert.ToInt32(gvDocs.DataKeys[index].Values["DocTypeId"]), MAXIMUS.Core.Libraries.Enumerations.LogAccessType.Download, getNPIFromUserID(Helper.GetUserId(HttpContext.Current.User.Identity.Name)));
                GetReportPDF(gvDocs.DataKeys[index].Values["DocId"].ToString());
                break;
            default:
                break;
        }
    }

    private void GetReportText(string docId)
    {
        try
        {
            #region TN VPN
            //var textResponse = TNReport.GetTexteRAForDocumentId(docId);
            #endregion

            //SendBinaryResponseToClient(textResponse, "attachment;filename=eRA.txt", "application/octet-stream");

        }
        catch (WebException ex)
        {
            RecordException(ex, MethodInfo.GetCurrentMethod().Name, false);
            ShowError(ex.Message);
        }
        catch (EndpointNotFoundException ex)
        {
            RecordException(ex, MethodInfo.GetCurrentMethod().Name, false);
            ShowError(ex.Message);
        }
        catch (Exception ex)
        {
            RecordException(ex, MethodInfo.GetCurrentMethod().Name, false);
            ShowError(ex.Message);
        }
    }

    private void ShowError(string exMessage)
    {
        this.ServerError.Visible = true;
        this.ServerError.Text = "Failed to Connect to eRA Service:" + exMessage;
    }

    private void GetReportPDF(string docId)
    {
        try
        {
            #region TN VPN
            //var pdfRequestResponse = TNReport.GetPdfeRAForDocumentId(docId);
            #endregion

            //SendBinaryResponseToClient(pdfRequestResponse, "attachment;filename=eRA.pdf", "application/pdf");
        }
        catch (WebException ex)
        {
            RecordException(ex, MethodInfo.GetCurrentMethod().Name, false);
            ShowError(ex.Message);
        }
        catch (EndpointNotFoundException ex)
        {
            RecordException(ex, MethodInfo.GetCurrentMethod().Name, false);
            ShowError(ex.Message);
        }
        catch (Exception ex)
        {
            RecordException(ex, MethodInfo.GetCurrentMethod().Name, false);
            ShowError(ex.Message);
        }
    }

    private void SendBinaryResponseToClient(byte[] responseBytes, string contentHeader, string contentType)
    {
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();

        Response.Buffer = true;
        Response.ContentType = contentType;
        Response.AddHeader("Content-Disposition", contentHeader);
        Response.BinaryWrite(responseBytes);
        HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        // Technically, I should be doing Response.End(), but due to a bug in ASP.NET 
        // we tried doing comlete request http://support.microsoft.com/kb/312629/en-us
        // but complete request is putting all the page up there. So, we are swallowing the
        // thread abort exception here.
        // Response.End();
        try
        {
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
        }
        catch (Exception ex)
        {
            CoreException.ThrowException(ex);
        }
        finally
        {
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }

    #endregion


    #region "X12 eRA"
    protected void btnX12eRADownload_Click(object sender, EventArgs e)
    {
        try
        {
            var medicaidId = ddlx12eRAMedicaidIdList.Visible ? ddlx12eRAMedicaidIdList.Text : txtMedcaidIdx12.Text;
            //var x12TransactionResponse = TNReport.GetX12TransactionsForMedicaidID(medicaidId);
            //SendBinaryResponseToClient(x12TransactionResponse, "attachment;filename=eRA.pdf", "application/pdf");
    }
        catch (WebException ex)
        {
            RecordException(ex, MethodInfo.GetCurrentMethod().Name, false);
            ShowError(ex.Message);
        }
        catch (EndpointNotFoundException ex)
        {
            RecordException(ex, MethodInfo.GetCurrentMethod().Name, false);
            ShowError(ex.Message);
        }
        catch (Exception ex)
        {
            RecordException(ex, MethodInfo.GetCurrentMethod().Name, false);
            ShowError(ex.Message);
        }
    }

    #endregion
}
