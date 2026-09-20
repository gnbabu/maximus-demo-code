using Codaxy.WkHtmlToPdf;
using Corp.Core.Libraries;
using Corp.Core.Libraries.Interface;
using Corp.Core.Libraries.Proxy;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class PopupControls_ProviderFinancial : BaseSectionControl
{
    private decimal sysEA = (decimal)0.0;
    private decimal manualEA = (decimal)0.0;
    private decimal claimRA = (decimal)0.0;
    private decimal nonClaimRA = (decimal)0.0;
    private decimal voidAmt = (decimal)0.0;
    private decimal ficaAmt = (decimal)0.0;
    private decimal backUpAmt = (decimal)0.0;
    private decimal netEA = (decimal)0.0;
    private static DataTable ds1099;

    #region svc
    private PDMSService.PDMSServiceClient _svc;
    private PDMSService.PDMSServiceClient svc
    {
        get
        {
            if (_svc == null)
            {
                _svc = new PDMSService.PDMSServiceClient();
            }

            return _svc;
        }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (_svc==null)
        {
            _svc = new PDMSService.PDMSServiceClient();
        }
        if(!Page.IsPostBack)
        {
            DataSet dataSet = _svc.GetActivityTypes();
            DataTable dt = dataSet.Tables[0];

            ddlActivityType.DataSource = dt;
            ddlActivityType.DataTextField = "ACTIVITY_TYPE_NAME";
            ddlActivityType.DataValueField = "ACTIVITY_TYPE_VALUE";
            ddlActivityType.DataBind();

            var years = Enumerable.Range(DateTime.Now.Year - 10, 10).OrderByDescending(x => x);

            ddlYear.Items.Clear();
            ddlYear.Items.Add("--Year--");
            ddlYear.Items.Add(DateTime.Now.Year.ToString());
            foreach (var year in years)
            {
                ddlYear.Items.Add(year.ToString());
            }
            ddlYear.Enabled = false;
        }
    }

    protected void ddlActivityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        // btnPrint.Visible = false;
        if (ddlActivityType.SelectedValue == "1")
        {
            ddlYear.Enabled = false;
            ddlYear.SelectedIndex = -1;
            pnlTransHist.Visible = false;
            pnl1099.Visible = false;
        }
        else
        {
            ddlYear.Enabled = true;
            pnlTransHist.Visible = false;
            pnl1099.Visible = false;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // btnPrint.Visible = false;
        lblErrPF.Visible = false;
        lblErrPF.Text = string.Empty;
        if(ddlActivityType.SelectedIndex == 0)
        {
            lblErrPF.Visible = true;
            lblErrPF.Text = "Select a Activity Type";
            return;
        }

        string NPIOrMedicaidId = _svc.GetNPIOrMedicaidIdByRegId(this.WorkflowPage.RegistrationId);

        if (ddlActivityType.SelectedValue == "1")
        {
            pnlTransHist.Visible = true;
            pnl1099.Visible = false;
            SearchTransactionHistory(this.WorkflowPage.MedicaidID, NPIOrMedicaidId);
        }
        else
        {
            pnlTransHist.Visible = false;
            if(ddlYear.SelectedIndex == 0)
            {
                lblErrPF.Visible = true;
                lblErrPF.Text = "Select a Year";
                return;
            }
            pnl1099.Visible = true;
            Search1099History(this.WorkflowPage.RegistrationId, NPIOrMedicaidId);
        }
    }



    public void Search1099History(int regID, string NPIOrMedicaidId)
    {

        string logHeader = string.Format("Provider Financial Get 1099 History for Medicaid ID - ") + this.WorkflowPage.MedicaidID + "Regid" + regID;
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
        Logging log = new Logging(Guid.NewGuid(), logMsg);
        try
        {

            lbl1099Err.Visible = false;
            lbl1099Err.Text = string.Empty;
            pnl1099dtl.Visible = true;
            log.CreateLogEntry("Get1099DataSet method being called is with regid  " + regID + "and NPIOrMedicaidId" + NPIOrMedicaidId, Logging.LogPriority.Information);
            DataSet dsresponse = Get1099DataSet(regID, NPIOrMedicaidId);
            log.CreateLogEntry("For RgId" + regID + "Get1099DataSet method returns " + dsresponse, Logging.LogPriority.Information);

            DataTable dtInquire1099Response = null;
            if (dsresponse != null)
            {
                dtInquire1099Response = dsresponse.Tables["Inquire1099Response"];
                log.CreateLogEntry("For RgId" + regID + "Get1099DataSet method returns in table structure " + dtInquire1099Response, Logging.LogPriority.Information);
            }
            string responseMessage = string.Empty;
            string responseType = string.Empty;

            if (dtInquire1099Response != null && Helper.HasRows(dtInquire1099Response))
            {
                if (dtInquire1099Response.Columns.Contains("ResponseType"))
                    responseType = Helper.GetString("ResponseType", dtInquire1099Response.Rows[0]);
                log.CreateLogEntry("ResponseType " + responseType, Logging.LogPriority.Information);

                if (dtInquire1099Response.Columns.Contains("ResponseMessage"))
                    responseMessage = Helper.GetString("ResponseMessage", dtInquire1099Response.Rows[0]);
                log.CreateLogEntry("ResponseMessage " + responseMessage, Logging.LogPriority.Information);

            }

            if (responseType == "Success")
            {
                log.CreateLogEntry("responseType Success Method" + responseMessage, Logging.LogPriority.Information);

                DataTable dtInformation1099 = dsresponse.Tables["Information1099"];
                string taxIDresponse = string.Empty;
                if (dtInformation1099 != null && Helper.HasRows(dtInformation1099))
                {
                    if (dtInformation1099.Columns.Contains("TaxId"))
                        taxIDresponse = Helper.GetString("TaxId", dtInformation1099.Rows[0]);
                    DataTable dtData1099 = dsresponse.Tables["Data1099"];

                    // OHPNM-4362 - they want GUI to match latest version of 5.09 DSD; TaxId needs to be removed from the middle of page and put into its own column in the search results
                    if (dtData1099 != null && Helper.HasRows(dtData1099))
                    {
                        dtData1099.Columns.Add(new DataColumn("TaxId", typeof(string)));
                        if (!dtData1099.Columns.Contains("Payer_Name"))
                        {
                            dtData1099.Columns.Add(new DataColumn("Payer_Name", typeof(string)));
                        }
                        if (!dtData1099.Columns.Contains("Payer_Address1"))
                        {
                            dtData1099.Columns.Add(new DataColumn("Payer_Address1", typeof(string)));
                        }
                        if (!dtData1099.Columns.Contains("Payer_Address2"))
                        {
                            dtData1099.Columns.Add(new DataColumn("Payer_Address2", typeof(string)));
                        }
                        if (!dtData1099.Columns.Contains("Payer_City"))
                        {
                            dtData1099.Columns.Add(new DataColumn("Payer_City", typeof(string)));
                        }
                        if (!dtData1099.Columns.Contains("Payer_State"))
                        {
                            dtData1099.Columns.Add(new DataColumn("Payer_State", typeof(string)));
                        }
                        if (!dtData1099.Columns.Contains("Payer_Zip"))
                        {
                            dtData1099.Columns.Add(new DataColumn("Payer_Zip", typeof(string)));
                        }

                        foreach (DataRow row in dtData1099.Rows)
                        {
                            row["TaxId"] = taxIDresponse;
                            if (!string.IsNullOrEmpty(row["IssuedDate"].ToString()))
                            {
                                //ohpnm-7138 resolved the IssueDate dateformat  for yyyy-MM-dd,yyyy/MM/dd and yyyyMMdd.
                                string IssuedDate = row["IssuedDate"].ToString();
                                if (IssuedDate.Contains("-"))
                                {
                                    row["IssuedDate"] = DateTime.ParseExact(IssuedDate, "yyyy-MM-dd", null).ToString("MM/dd/yyyy");
                                }
                                else if (IssuedDate.Contains("/"))
                                {
                                    row["IssuedDate"] = DateTime.ParseExact(IssuedDate, "yyyy/MM/dd", null).ToString("MM/dd/yyyy");
                                }
                                else
                                {
                                    row["IssuedDate"] = DateTime.ParseExact(IssuedDate, "yyyyMMdd", null).ToString("MM/dd/yyyy");
                                }
                            }
                            if (string.IsNullOrEmpty(row["Payer_Name"].ToString()))
                            {
                                row["Payer_Name"] = "OHIO DEPARTMENT OF MEDICAID";
                            }
                            if ((string.IsNullOrEmpty(row["Payer_Address1"].ToString())) && (string.IsNullOrEmpty(row["Payer_Address2"].ToString())))
                            {
                                row["Payer_Address2"] = "P.O. Box 182709";
                            }
                            if (string.IsNullOrEmpty(row["Payer_Address1"].ToString()))
                            {
                                row["Payer_Address1"] = "Bureau of Provider Services";
                            }
                            if (string.IsNullOrEmpty(row["Payer_City"].ToString()))
                            {
                                row["Payer_City"] = "COLUMBUS";
                            }
                            if (string.IsNullOrEmpty(row["Payer_State"].ToString()))
                            {
                                row["Payer_State"] = "OH";
                            }
                            if (string.IsNullOrEmpty(row["Payer_Zip"].ToString()))
                            {
                                row["Payer_Zip"] = "43218-2709";
                            }
                        }
                        //Make it display based on the config
                        //if (AppSettings.Get("EnablePrint1099", string.Empty) == "true")
                        //{
                        //    btnPrint.Visible = true;
                        //    btnPrint.Enabled = true;
                        //}
                        //else
                        //{
                        //    btnPrint.Visible = true;
                        //    btnPrint.Enabled = false;
                        //}
                    }

                    ds1099 = dtData1099;
                    grd1099.DataSource = dtData1099;
                    grd1099.DataBind();

                }

                else
                {
                    lbl1099Err.Visible = true;
                    lbl1099Err.Text = "No Records Found" + responseMessage;
                    pnl1099dtl.Visible = false;
                    //btnPrint.Visible = false;
                }
            }
            else
            {
                lbl1099Err.Visible = true;
                lbl1099Err.Text = "Failed to retrieve records from downstream system : " + responseMessage;
                pnl1099dtl.Visible = false;

            }
        }
        catch (WebException ex)
        {
            lbl1099Err.Visible = true;
            lbl1099Err.Text = ex.Message;
            pnl1099dtl.Visible = false;
        }
        catch (Exception ex)
        {
            lbl1099Err.Visible = true;
            lbl1099Err.Text = ex.Message;
            pnl1099dtl.Visible = false;

            log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.Message), Logging.LogPriority.Error);
        }
    }

    protected void grd1099_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.RowType == DataControlRowType.DataRow)
        {
            sysEA += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "SystemEarningsAmount"));
            manualEA += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ManualEarningsAmount"));
            claimRA += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ClaimRefundAmount"));
            nonClaimRA += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NonClaimRefundsAmount"));
            voidAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "VoidAmount"));
            ficaAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "FicaAmount"));
            backUpAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "BackupWithholdingAmount"));
            netEA += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NetEarningsAmount"));
        }
        if(e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "Page Totals:";
            e.Row.Cells[1].Text = "";
            e.Row.Cells[2].Text = String.Format("{0:c}", sysEA);
            e.Row.Cells[3].Text = String.Format("{0:c}", manualEA);
            e.Row.Cells[4].Text = String.Format("{0:c}", claimRA);
            e.Row.Cells[5].Text = String.Format("{0:c}", nonClaimRA);
            e.Row.Cells[6].Text = String.Format("{0:c}", voidAmt);
            e.Row.Cells[7].Text = String.Format("{0:c}", ficaAmt);
            e.Row.Cells[8].Text = String.Format("{0:c}", backUpAmt);
            e.Row.Cells[9].Text = String.Format("{0:c}", netEA);
        }
    }

    protected void grd1099_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd1099.PageIndex = e.NewPageIndex;
        grd1099.DataSource = ds1099;
        grd1099.DataBind();

    }
    public void SearchTransactionHistory(string medicaidID, string NPIOrMedicaidId)
    {
        string logHeader = string.Format("Provider Financial Get Transaction History for Medicaid ID - ") + this.WorkflowPage.MedicaidID;
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
        Logging log = new Logging(Guid.NewGuid(), logMsg);
        try
        {
            lblTransHistErr.Visible = false;
            lblTransHistErr.Text = string.Empty;
            pnlTransHistDetial.Visible = true;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            FinancialServiceReqRes fsr = new FinancialServiceReqRes();

            DataSet dsResponse = fsr.GetTransactionHistory(medicaidID, NPIOrMedicaidId);

            if (dsResponse != null && dsResponse.Tables.Contains("InquireTransactionHistoryResponse"))
            {
                DataTable dtInquireTransactionHistoryResponse = dsResponse.Tables["InquireTransactionHistoryResponse"];
                string responseMessage = string.Empty;
                string responseType = string.Empty;

                if (Helper.HasRows(dtInquireTransactionHistoryResponse))
                {
                    if (dtInquireTransactionHistoryResponse.Columns.Contains("ResponseType"))
                        responseType = Helper.GetString("ResponseType", dtInquireTransactionHistoryResponse.Rows[0]);
                    if (dtInquireTransactionHistoryResponse.Columns.Contains("ResponseMessage"))
                        responseMessage = Helper.GetString("ResponseMessage", dtInquireTransactionHistoryResponse.Rows[0]);
                }

                DataTable dtClaimActivitySummary = dsResponse.Tables["ClaimActivitySummary"];

                if (responseType == "Success")
                {
                    if (Helper.HasRows(dtClaimActivitySummary))
                    {
                        DataRow drClaimActivitySummary = dtClaimActivitySummary.Rows[0];
                        if (dtClaimActivitySummary.Columns.Contains("ClaimsPaidCurMonth"))
                            lbl1.Text = Helper.GetString("ClaimsPaidCurMonth", drClaimActivitySummary);
                        if (dtClaimActivitySummary.Columns.Contains("AmountPaidCurMonth"))
                            lbl2.Text = "$" + Helper.GetString("AmountPaidCurMonth", drClaimActivitySummary);
                        if (dtClaimActivitySummary.Columns.Contains("ClaimsDeniedCurMonth"))
                            lbl3.Text = Helper.GetString("ClaimsDeniedCurMonth", drClaimActivitySummary);
                        if (dtClaimActivitySummary.Columns.Contains("ClaimsPaidPast12Months"))
                            lbl4.Text = Helper.GetString("ClaimsPaidPast12Months", drClaimActivitySummary);
                        if (dtClaimActivitySummary.Columns.Contains("AmountPaidPast12Months"))
                            lbl5.Text = "$" + Helper.GetString("AmountPaidPast12Months", drClaimActivitySummary);
                        if (dtClaimActivitySummary.Columns.Contains("ClaimsDeniedPast12Months"))
                            lbl6.Text = Helper.GetString("ClaimsDeniedPast12Months", drClaimActivitySummary);
                        if (dtClaimActivitySummary.Columns.Contains("SuspendedClaims"))
                            lbl7.Text = Helper.GetString("SuspendedClaims", drClaimActivitySummary);
                        if (dtClaimActivitySummary.Columns.Contains("ClaimsInFinalDisposition"))
                            lbl8.Text = Helper.GetString("ClaimsInFinalDisposition", drClaimActivitySummary);
                        // convert from this format that is in the test data "2019-10-12" to this format "10/12/2019"
                        if (dtClaimActivitySummary.Columns.Contains("RecentPaymentDate"))
                            lbl9.Text = Helper.GetDate("RecentPaymentDate", drClaimActivitySummary);
                        if (dtClaimActivitySummary.Columns.Contains("RecentPaymentType"))
                            lbl10.Text = Helper.GetString("RecentPaymentType", drClaimActivitySummary);
                        if (dtClaimActivitySummary.Columns.Contains("RecentPaymentAmount"))
                            lbl11.Text = "$" + Helper.GetString("RecentPaymentAmount", drClaimActivitySummary);
                        if (dtClaimActivitySummary.Columns.Contains("TotalCreditBalanceAmount"))
                            lbl12.Text = "$" + Helper.GetString("TotalCreditBalanceAmount", drClaimActivitySummary);
                        if (dtClaimActivitySummary.Columns.Contains("AmountTowardCreditBalance"))
                            lbl13.Text = "$" + Helper.GetString("AmountTowardCreditBalance", drClaimActivitySummary); 
                    }
                    else
                    {
                        lblTransHistErr.Visible = true;
                        lblTransHistErr.Text = "No Records Found : " + responseMessage;
                        pnlTransHistDetial.Visible = false;
                    }
                }
                else
                {
                    lblTransHistErr.Visible = true;
                    lblTransHistErr.Text = "Failed to retrieve records from downstream system. Please try again after some time.";
                    pnlTransHistDetial.Visible = false;
                }
            }
            else
            {
                lblTransHistErr.Visible = true;
                lblTransHistErr.Text = "Failed to retrieve records from downstream system. Please try again after some time.";
                pnlTransHistDetial.Visible = false;
            }
        }
        catch (WebException ex)
        {
            lblTransHistErr.Visible = true;
            lblTransHistErr.Text = "Failed to retrieve records from downstream system. Please try again after some time.";
            pnlTransHistDetial.Visible = false;
        }
        catch (Exception ex)
        {
            lblTransHistErr.Visible = true;
            lblTransHistErr.Text = "Failed to retrieve records from downstream system. Please try again after some time.";
            pnlTransHistDetial.Visible = false;
            log.CreateLogEntry(string.Format("{0} {1} {2}", logHeader, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }

    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Guid logThreadId = Guid.NewGuid();
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            string fileName;

            string NPIOrMedicaidId = _svc.GetNPIOrMedicaidIdByRegId(this.WorkflowPage.RegistrationId);

            DataSet dsresponse = Get1099DataSet(this.WorkflowPage.RegistrationId, NPIOrMedicaidId);
            DataTable dtInquire1099Response = null;
            if (dsresponse != null)
                dtInquire1099Response = dsresponse.Tables["Inquire1099Response"];
            string responseMessage = string.Empty;
            string responseType = string.Empty;

            if (dtInquire1099Response != null && Helper.HasRows(dtInquire1099Response))
            {
                if (dtInquire1099Response.Columns.Contains("ResponseType"))
                    responseType = Helper.GetString("ResponseType", dtInquire1099Response.Rows[0]);
                if (dtInquire1099Response.Columns.Contains("ResponseMessage"))
                    responseMessage = Helper.GetString("ResponseMessage", dtInquire1099Response.Rows[0]);
            }

            if (responseType == "Success")
            {
                DataTable dtInformation1099 = dsresponse.Tables["Information1099"];
                DataTable dtData1099 = dsresponse.Tables["Data1099"];
                string taxIDResponse = string.Empty;
                string taxWithheldResponse = string.Empty;
                string paymentdResponse = string.Empty;
                if (dtInformation1099 != null && Helper.HasRows(dtInformation1099))
                {
                    if (dtInformation1099.Columns.Contains("TaxId"))
                        taxIDResponse = Helper.GetString("TaxId", dtInformation1099.Rows[0]);
                    if (dtInformation1099.Columns.Contains("MedPymt"))
                    {
                        decimal medPymt = Helper.GetDecimal("MedPymt", dtInformation1099.Rows[0]);
                        paymentdResponse = String.Format("{0:C2}", medPymt);
                    }
                        
                    if (dtInformation1099.Columns.Contains("Fitw"))
                    {
                        decimal fitw = Helper.GetDecimal("Fitw", dtInformation1099.Rows[0]);
                        taxWithheldResponse = String.Format("{0:C2}", fitw);
                    }
                        
                }
                bool boolHasPayerData = false;
                string payerName = string.Empty;
                string payerAddress1 = string.Empty;
                string payerAddress2 = string.Empty;
                string payerCity = string.Empty;
                string payerState = string.Empty;
                string payerZip = string.Empty;
                if (dtData1099 != null && Helper.HasRows(dtData1099))
                {
                    if (dtData1099.Columns.Contains("Payer_Name") && dtData1099.Columns.Contains("Payer_Address1") && dtData1099.Columns.Contains("Payer_City"))
                    {
                        boolHasPayerData = true;
                        if (dtData1099.Columns.Contains("Payer_Name"))
                        {
                            payerName = Helper.GetString("Payer_Name", dtData1099.Rows[0]);
                        }
                        if (dtData1099.Columns.Contains("Payer_Address1"))
                        {
                            payerAddress1 = Helper.GetString("Payer_Address1", dtData1099.Rows[0]);
                        }
                        if (dtData1099.Columns.Contains("Payer_Address2"))
                        {
                            payerAddress2 = Helper.GetString("Payer_Address2", dtData1099.Rows[0]);
                        }
                        if (dtData1099.Columns.Contains("Payer_City"))
                        {
                            payerCity = Helper.GetString("Payer_City", dtData1099.Rows[0]);
                        }
                        if (dtData1099.Columns.Contains("Payer_State"))
                        {
                            payerState = Helper.GetString("Payer_State", dtData1099.Rows[0]);
                        }
                        if (dtData1099.Columns.Contains("Payer_ZIP"))
                        {
                            payerZip = Helper.GetString("Payer_ZIP", dtData1099.Rows[0]);
                        }
                    }
                }

                string year = ddlYear.SelectedItem.Text;
                string appPath = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("Process"));
                bool result = false;
                if (boolHasPayerData)
                {
                    result = psc.Generate1099WithPayerInfo(this.WorkflowPage.RegistrationId, taxIDResponse, taxWithheldResponse, paymentdResponse, year, HttpContext.Current.User.Identity.Name, appPath, out fileName, payerName, payerAddress1, payerAddress2, payerCity, payerState, payerZip);
                    // result = psc.Generate1099(this.WorkflowPage.RegistrationId, taxIDResponse, taxWithheldResponse, paymentdResponse, year, HttpContext.Current.User.Identity.Name, appPath, out fileName);
                }
                else
                {
                    result = psc.Generate1099(this.WorkflowPage.RegistrationId, taxIDResponse, taxWithheldResponse, paymentdResponse, year, HttpContext.Current.User.Identity.Name, appPath, out fileName);
                }

            }            
        }

    }

    private void DownloadFile(string fileName)
    {
        try
        {
            DownloadRequest downloadRequest = new DownloadRequest();
            downloadRequest.FileName = fileName;
            downloadRequest.IsDiddReferral = true;
            FileTransferServiceClient client = new FileTransferServiceClient();
            string fileNameOnly = fileName.Substring(fileName.LastIndexOf("\\") + 1);
            using (var fileStream = client.DownloadFile(downloadRequest).FileByteStream)
            {
                SendBinaryResponseToClient(fileStream, "attachment;filename=" + fileNameOnly, "application/pdf");
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Exception.aspx?Message=" + ex.Message);
        }
    }

    private void SendBinaryResponseToClient(Stream response, string contentHeader, string contentType)
    {
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();

        Response.Buffer = true;
        Response.ContentType = contentType;
        Response.AddHeader("Content-Disposition", contentHeader);
        response.CopyTo(Response.OutputStream);

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
    public override bool SaveData()
    {
        return true;
    }

    public override void LoadControlData()
    {

    }

    public override void LoadData(DataRow dr)
    {
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override string ValidationGroup
    {
        get { return "valProviderFinancial"; }
    }

    public override string Title
    {
        get { return "PROVIDER FINANCIAL SEARCH"; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string IdText
    {
        get { return "ucProviderFinancial_" + this.WorkflowPage.RegistrationId; }
    }  
    
    private DataSet Get1099DataSet(int regID, string NPIOrMedicaidId)
    {
        string logHeader = string.Format("Provider Financial  Get1099DataSet for Medicaid ID - ") + this.WorkflowPage.MedicaidID;
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
        Logging log = new Logging(Guid.NewGuid(), logMsg);
        string taxID = string.Empty, taxIdType = string.Empty, reqYear = string.Empty;
        log.CreateLogEntry("SelectRegistrationByRegID method call for regid " + regID, Logging.LogPriority.Information);
        DataSet dsReg = svc.SelectRegistrationByRegID(regID);
        dsReg.Tables[0].TableName = "RegData";
        DataRow drReg = Helper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
        log.CreateLogEntry("For regid" + regID + "with RegData" + dsReg.Tables[0].Rows[0], Logging.LogPriority.Information);

        if (drReg != null)
        {
            taxID = Helper.GetString("TaxID", drReg);
            log.CreateLogEntry("For regid" + regID + "with TaxID" + drReg, Logging.LogPriority.Information);
            taxIdType = Helper.GetString("TaxIDTypeID", drReg) == "16" ? "F" : "S";
            log.CreateLogEntry("For regid" + regID + "with TaxIDTypeID" + taxIdType, Logging.LogPriority.Information);
            reqYear = ddlYear.SelectedItem.Text;
            log.CreateLogEntry("For regid" + regID + "with reqYear" + reqYear, Logging.LogPriority.Information);
        }

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
        log.CreateLogEntry("For regid" + regID + "with SecurityProtocol" + ServicePointManager.SecurityProtocol, Logging.LogPriority.Information);

        FinancialServiceReqRes fsr = new FinancialServiceReqRes();
        log.CreateLogEntry("SelectRegistrationByRegID method call for regid " + regID+ "and MedicaidID="
            + this.WorkflowPage.MedicaidID +"taxid="+ taxID+"and taxIdType"+ taxIdType+" and reqyear="+ reqYear+ "and NPIOrMedicaidId"+ NPIOrMedicaidId, Logging.LogPriority.Information);

        return fsr.Get1099SearchHistory(this.WorkflowPage.MedicaidID, taxID, taxIdType, reqYear, NPIOrMedicaidId);
        
    }

    protected void grd1099_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string[] arg = new string[10];
        arg = e.CommandArgument.ToString().Split(';');
        if (arg.Length >= 10)
        {
            string taxIDResponse = arg[0];
            decimal paymentAmount = decimal.Parse(arg[1]);
            decimal ficaWitholding = decimal.Parse(arg[2]);
            decimal backupWitholding = decimal.Parse(arg[3]);
            string payerName = arg[4];
            string payerAddress1 = arg[5];
            string payerAddress2 = arg[6];
            string payerCity = arg[7];
            string payerState = arg[8];
            string payerZip = arg[9];

            decimal totalWitholding = ficaWitholding + backupWitholding;
            string paymentdResponse = String.Format("{0:C2}", paymentAmount);
            string taxWithheldResponse = String.Format("{0:C2}", totalWitholding);

            string year = ddlYear.SelectedItem.Text;
            string appPath = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("Process"));
            bool result = false;

            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                string fileName;
                result = psc.Generate1099WithPayerInfo(this.WorkflowPage.RegistrationId, taxIDResponse, taxWithheldResponse, paymentdResponse, year, HttpContext.Current.User.Identity.Name, appPath, out fileName, payerName, payerAddress1, payerAddress2, payerCity, payerState, payerZip);

                if (!string.IsNullOrEmpty(fileName))
                {
                    DownloadFile(fileName);
                }
            }
        }
    }
}