using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MMSWebControls;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using Corp.Core.Libraries;
public partial class PopupControls_MSPCOSTSEARCH : BaseSectionControl
{
    private const string sectionName = "MSPCostReport";

    public override string ValidationGroup
    {
        get { return "valMSPCostReport"; }
    }

    public override string Title
    {
        get { return "MSP Cost Report SEARCH"; }
    }
    public override void LoadControlData() { }
    public override bool SaveData() { return true; }
    public override void LoadData(DataRow dr)
    {
    }
    
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
    private int RegCostReportID
    {
        get
        {
            return ViewState["RegCostReportID"] == null ? 0 : Convert.ToInt32(ViewState["RegCostReportID"]);
        }
        set
        {
            ViewState["RegCostReportID"] = value;
        }
    }

    public override string IdText
    {
        get { return "ucMSPCostReport_" + this.WorkflowPage.RegistrationId; }
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        LoadCostReportStatusDropDown();
        LoadFiscalYearDropDown();
    }
    private void LoadMSPCostReportSearchGrid()
    {
        DataSet dataSet = svc.MSPSearchCostReportDetails();
        PlaceHolder2.Controls.Clear();
        PlaceHolder2.Controls.Add(DataTableToHTMLTable(dataSet.Tables["ResponsePayload"]));

    }
    public Table DataTableToHTMLTable(DataTable dt)
    {
        Table tbl = new Table();
        TableRow tr = null;
        TableCell cell = null;

        int rows = dt.Rows.Count;
        int cols = dt.Columns.Count;

        // table header
        TableHeaderRow htr = new TableHeaderRow();
        TableHeaderCell hcell = null;
        hcell = new TableHeaderCell();
        hcell.Text = "MSP Cost Report Submission History Search Result";
        hcell.ColumnSpan = 9;
        htr.Cells.Add(hcell);
        htr.BackColor = ColorTranslator.FromHtml("#4080bf");
        tbl.Rows.Add(htr);

        htr = new TableHeaderRow();
        htr.BackColor = ColorTranslator.FromHtml("#c6d9ec");


        hcell = new TableHeaderCell();
        hcell.Text = "Medicaid Provider ID";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Provider Name";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "MITS Tracking Number";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Cost Report Status";
        htr.Cells.Add(hcell);


        hcell = new TableHeaderCell();
        hcell.Text = "CR Submission Date";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "CR Submission Time";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "FISCAL YEAR";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Submitter ID";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Preparer ID";
        htr.Cells.Add(hcell);

        tbl.Rows.Add(htr);

        // Table Header

        // Table rows

        // Table rows
        for (int j = 0; j < rows; j++)
        {
            tr = new TableRow();



            cell = new TableCell();

            cell.Text = dt.Rows[j]["IdProvider"].ToString();
            tr.Cells.Add(cell);

            cell = new TableCell();
            DataSet ds = svc.SelectProviderByGRPMedicaidID(dt.Rows[j]["IdProvider"].ToString());
            DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
            this.DataList = dtMisc;
            if (Helper.HasRows(dtMisc))
            {
                DataRow dr = dtMisc.Rows[0];
                cell.Text = Helper.GetString("NAME", dr);
            }
            tr.Cells.Add(cell);


            cell = new TableCell();
            LinkButton LnkBtn = new LinkButton();
            LnkBtn.Click += new EventHandler(LnkBtn_Click);
            LnkBtn.ID = "ViewDetails";
            LnkBtn.Text = dt.Rows[j]["MITSTrackingNumber"].ToString();
            LnkBtn.Attributes.Add("runat", "server");
            cell.Controls.Add(LnkBtn);
            tr.Cells.Add(cell);


            cell = new TableCell();
            cell.Text = dt.Rows[j]["SubmissionStatus"].ToString();
            tr.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dt.Rows[j]["SubmissionDate"].ToString();
            tr.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = Convert.ToDateTime(dt.Rows[j]["SubmissionDate"].ToString()).ToLongTimeString();
            tr.Cells.Add(cell);

            //cell = new TableCell();
            //cell.Text = dt.Rows[j]["CRFromDate"].ToString();
            //tr.Cells.Add(cell);

            //cell = new TableCell();
            //cell.Text = dt.Rows[j]["CRToDate"].ToString();
            //tr.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dt.Rows[j]["CRFiscalYear"].ToString();
            tr.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dt.Rows[j]["SubmitterID"].ToString();
            tr.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dt.Rows[j]["PreparerID"].ToString();
            tr.Cells.Add(cell);

            tr.BackColor = ColorTranslator.FromHtml("#ecf2f9");
            tbl.Rows.Add(tr);
        }


        return tbl;
    }

    private void LoadProviderInformation(string medicaidNumber)
    {
        DataSet ds = svc.SelectProviderByGRPMedicaidID(medicaidNumber);
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
        this.DataList = dtMisc;
        if (Helper.HasRows(dtMisc))
        {
            DataRow dr = dtMisc.Rows[0];
            this.RegIdTxt.Value = Helper.GetString("REG_ID", dr);
        }

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ValidateData())
        {
            LoadMSPCostReportSearchGrid();
            cpGrid.Collapsed = false;
        }
    }
    protected void LnkBtn_Click(object sender, EventArgs e)
    {
        DataSet ds = svc.SelectRegMSPCostReport(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Convert.ToInt32(this.RegIdTxt.Value));
        DataSet dataSet = svc.MSPSearchCostReportDetails();


        if (Helper.HasRows(ds))
        {
            var dr = ds.Tables[0].Rows[0];
            lblcost.Text = dr["REPORT_TYPE_DESC"].ToString();

            lblTrackID.Text = dr["TRACKING_ID"].ToString();
            lblstatus.Text = GetSubmissionStatus(Convert.ToInt32(dr["SUBMISSION_STATUS"].ToString()));
            lblCrDate.Text = dr["CR_Status_Date"].ToString();
            lblCrTime.Text = dr["CR_Status_time"].ToString();
            lblReportFrom.Text = dr["FROM_DATE"].ToString();
            lblReportTo.Text = dr["END_DATE"].ToString();

            GetRegCostReportDocuments();

            if (Helper.HasRows(dataSet))
            {
                var serviceDr = dataSet.Tables["ResponsePayload"].Rows[0];
                //lblrevname.Text = serviceDr["ReviewerName"].ToString();
                //lblProvider.Text = serviceDr["ProviderType"].ToString();
                //lblcrdoc.Text = serviceDr["CrDocumentNumber"].ToString();
                lblPhone.Text = serviceDr["PreparerPhone"].ToString();
                //lblSubEmail.Text = serviceDr["SubmitterEmailID"].ToString();
                //lblSubId.Text = serviceDr["SubmitterID"].ToString();
                //lblSubName.Text = serviceDr["SubmitterFN"].ToString() + " " + serviceDr["SubmitterLN"].ToString();
                //lblSubPhn.Text = serviceDr["SubmitterPhone"].ToString();
                lblEmail.Text = serviceDr["PreparerEmailID"].ToString();
                lblId.Text = serviceDr["PreparerID"].ToString();
                lblName.Text = serviceDr["PreparerFN"].ToString() + " " + serviceDr["PreparerLN"].ToString();
                lblmedicaidID.Text = dr["IdProvider"].ToString();

            }
        }
        cpViewDetails.Collapsed = false;
        cpGrid.Collapsed = true;
        pnlViewDetails.Visible = true;
    }
    private string GetSubmissionStatus(int submissionSatusId)
    {
        string status = string.Empty;
        if (submissionSatusId == CON.CostReportSubmissionStatus.Approved)
        {
            status = "Approved";
        }
        else if (submissionSatusId == CON.CostReportSubmissionStatus.Rejected)
        {
            status = "Rejected";
        }
        else if (submissionSatusId == CON.CostReportSubmissionStatus.Notified)
        {
            status = "Notified";
        }
        else
        {
            status = "Hold";
        }
        return status;
    }
    private void GetRegCostReportDocuments()
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "MSPCostReportREGXref");
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        if (Helper.HasRows(dtMisc))
        {
            var dr = dtMisc.Rows[0];
            lnkPreAud.Text = GetFileDetails(Helper.GetInt("PRE_AUDITED_MSPR_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            lnkPostAud.Text = GetFileDetails(Helper.GetInt("POST_AUDITED_MSPR_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            lnkAttest.Text = GetFileDetails(Helper.GetInt("ATTESTATION_FINDINGS_REPORT_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            lnkAgreed.Text = GetFileDetails(Helper.GetInt("AGREED_UPONPROCE_REPORT_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            if (Helper.GetInt("OTHER_DOCUMENT_ID", dr) > 0)
            {
                lnkdoc2.Text = GetFileDetails(Helper.GetInt("OTHER_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            }
            if (Helper.GetInt("TRANSPORTATION_T1_REPORT_DOCUMENT_ID", dr) > 0)
            {
                lnkT1.Text = GetFileDetails(Helper.GetInt("TRANSPORTATION_T1_REPORT_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            }
            if (Helper.GetInt("TRANSPORTATION_T2_REPORT_DOCUMENT_ID", dr) > 0)
            {
                lnkT2.Text = GetFileDetails(Helper.GetInt("TRANSPORTATION_T2_REPORT_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            }
        }
    }
    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valLTCCostReport";
        this.Page.Validators.Add(val);
        isGood = false;
    }
    public override bool ValidateData()
    {
        bool isValid = true;

        DateTime value;
        if (!String.IsNullOrEmpty(txtsubmitdate.Text))
        {
            if (DateTime.TryParse(txtsubmitdate.Text, out value))
            {
                if (DateTime.Compare(DateTime.Now.Date, Convert.ToDateTime(txtsubmitdate.Text)) < 0)
                {
                    AddError("* Date From cannot be after todays date.", ref isValid);
                }
            }
            else
            {
                AddError("* Date From is not a date format.", ref isValid);
            }
        }




        return isValid;
    }

    private void LoadCostReportStatusDropDown()
    {
        if (!(this.Dropdowncostrstatus.Items.Count > 0))
        {
            this.Dropdowncostrstatus.Items.Insert(0, "select status");
            this.Dropdowncostrstatus.Items.Insert(1, CON.CostReportSubmissionResponseStatus.All.ToString());
            this.Dropdowncostrstatus.Items.Insert(2, CON.CostReportSubmissionResponseStatus.Obsoleted.ToString());
            this.Dropdowncostrstatus.Items.Insert(3, CON.CostReportSubmissionResponseStatus.Pending.ToString());
            this.Dropdowncostrstatus.Items.Insert(4, CON.CostReportSubmissionResponseStatus.Clear.ToString());
            this.Dropdowncostrstatus.SelectedIndex = 0;
        }
    }

    private void LoadFiscalYearDropDown()
    {
        if (!(this.DropDownFiscal.Items.Count > 0))
        {
            this.DropDownFiscal.Items.Insert(0, "select year");
            var currentDate = DateTime.Now;
            var EndDate = DateTime.Now.AddYears(4);
            var i = 1;
            while (currentDate.Year < EndDate.Year)
            {
                var fiscalYear = currentDate.ToShortDateString() + "-";
                currentDate = currentDate.AddMonths(6);
                fiscalYear += currentDate.ToShortDateString();
                this.DropDownFiscal.Items.Insert(i, fiscalYear);
            }
           
            this.DropDownFiscal.SelectedIndex = 0;
        }

    }
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    private DataRow GetFileDetails(int documentId)
    {
        DataSet ds = svc.SelectCostReportDocument(documentId);

        if (Helper.HasRows(ds.Tables[0]))
        {
            return ds.Tables[0].Rows[0];
        }
        return null;
    }
    protected void OnFileDownload(object sender, EventArgs e)
    {

        LinkButton linkButton = (LinkButton)sender;
        DownloadFile(linkButton.Text);

        return;

    }

    private void DownloadFile(string fileName)
    {
        try
        {
            string filePath = string.Empty;

            fileName = fileName.Replace("\"", string.Empty);
            fileName = fileName.Replace(",", "");
            filePath = Path.Combine(Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName);
#if DEBUG
                   @filePath = @"C:\Temp";
#endif

            //Local dev Test
            //filePath = @"C:\Projects\Upload\" + fileName;

            // If isFileLocal is false, that means file is on onbase. 
            bool isFileLocal = bool.Parse(AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString));

            if (isFileLocal && !File.Exists(filePath))
            {

                var error = "File or directory does not exist.";
                return;
            }


            //filePath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName;
            //FilePath = @"C:\Projects\PDMS2_0_0\PDMS\PDMS\FileStoreLocal\" + fileName;

            bool fileExistsonLocal = System.IO.File.Exists(filePath);
            bool downloadFile = isFileLocal ? fileExistsonLocal : true;

            OnBaseInterface onBaseInterface = new OnBaseInterface();
            byte[] decryptedFile = onBaseInterface.RetrieveFile(filePath);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(decryptedFile);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();

        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {

            //lblErrorMessages.Text = "an error has occurred during the download operation";
        }

    }

    protected void btnCancelCostReport_Click(object sender, EventArgs e)
    {
        MEDID.Text = "";
        txtsubmitdate.Text = "";
        txtSubmissionID.Text = "";
        TextBox2.Text = "";
        txtTrackingNumber.Text = "";
        DropDownFiscal.SelectedIndex = 0;
        Dropdowncostrstatus.SelectedIndex = 0;
        ddlPageSize.SelectedIndex = 0;

    }
}