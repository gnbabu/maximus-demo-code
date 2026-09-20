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
public partial class PopupControls_SubmissionCR : BaseSectionControl
{
    private const string sectionName = "LTCCostReport";

    protected int _MaxFileMegaBytes;
    private string _DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
    private string _ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
    public int MaxFileMegaBytes
    {
        get
        {
            if (_MaxFileMegaBytes == 0) _MaxFileMegaBytes = 80;       // Default is 5MB
            return _MaxFileMegaBytes;
        }
        set
        {
            _MaxFileMegaBytes = value;
            if (_MaxFileMegaBytes > 80) _MaxFileMegaBytes = 80;
        }
    }
    public override string ValidationGroup
    {
        get { return "valLTCCostReport"; }
    }

    public override string Title
    {
        get { return "LTC Cost Report SEARCH"; }
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cmpValCannotBeFutureSubmission.ValueToCompare = DateTime.Now.ToString("MM/dd/yyyy");
            string MedicaidNumber = this.WorkflowPage.MedicaidID;   //Request.QueryString["MedicaidNumber"];
            LoadCostReportTypeDropDown(MedicaidNumber);
            LoadCostReportStatusDropDown();
            LoadFiscalYearDropDown();
            LoadProviderTypeDropDown();
        }
    }

    private void LoadCostReportSearchGrid()
    {
        DataSet dataSet = svc.SearchCostReportDetails();
        PlaceHolder1.Controls.Clear();
        PlaceHolder1.Controls.Add(DataTableToHTMLTable(dataSet.Tables["ResponsePayload"]));
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
        hcell.Text = "LTC COST REPORT SUBMISSION HISTORY SEARCH RESULTS";
        hcell.ColumnSpan = 15;
        htr.Cells.Add(hcell);
        htr.BackColor = ColorTranslator.FromHtml("#4080bf");
        tbl.Rows.Add(htr);

        htr = new TableHeaderRow();
        htr.BackColor = ColorTranslator.FromHtml("#c6d9ec");
        hcell = new TableHeaderCell();
        hcell.Text = "Provider Type";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Medicaid ID";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Cost Report Type";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Tracking Number";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Cost Report Status";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Cost Report Submission Status";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "CR Submission Date";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "CR Submission Time";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "CR From Date";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "CR To Time";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Fiscal Year";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Submitter ID";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Reviewer Name";
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
            cell.Text = dt.Rows[j]["ProviderType"].ToString();
            tr.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dt.Rows[j]["IdProvider"].ToString();
            tr.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dt.Rows[j]["CostReportType"].ToString();
            tr.Cells.Add(cell);

            cell = new TableCell();
            LinkButton LnkBtn = new LinkButton();
            LnkBtn.Click += new EventHandler(LnkBtn_Click);
            LnkBtn.Text = dt.Rows[j]["MITSTrackingNumber"].ToString();
            cell.Controls.Add(LnkBtn);
            tr.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dt.Rows[j]["CrDocumentNumber"].ToString();
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

            cell = new TableCell();
            cell.Text = dt.Rows[j]["CRFromDate"].ToString();
            tr.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dt.Rows[j]["CRToDate"].ToString();
            tr.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dt.Rows[j]["CRFiscalYear"].ToString();
            tr.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dt.Rows[j]["SubmitterID"].ToString();
            tr.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dt.Rows[j]["ReviewerName"].ToString();
            tr.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = dt.Rows[j]["PreparerID"].ToString();
            tr.Cells.Add(cell);


            tr.BackColor = ColorTranslator.FromHtml("#ecf2f9");
            tbl.Rows.Add(tr);
        }


        return tbl;
    }

    protected void LnkBtn_Click(object sender, EventArgs e)
    {
        DataSet ds = svc.SelectRegCostReport(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Convert.ToInt32(this.RegIdTxt.Value));
        DataSet dataSet = svc.SearchCostReportDetails();


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
                lblrevname.Text = serviceDr["ReviewerName"].ToString();
                lblProvider.Text = serviceDr["ProviderType"].ToString();
                lblcrdoc.Text = serviceDr["CrDocumentNumber"].ToString();
                lblPhone.Text = serviceDr["PreparerPhone"].ToString();
                lblSubEmail.Text = serviceDr["SubmitterEmailID"].ToString();
                lblSubId.Text = serviceDr["SubmitterID"].ToString();
                lblSubName.Text = serviceDr["SubmitterFN"].ToString() + " " + serviceDr["SubmitterLN"].ToString();
                lblSubPhn.Text = serviceDr["SubmitterPhone"].ToString();
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

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    private string GetMMISProviderTypeID(int providerTypeId)
    {
        string rtn = "";
        DataSet ds = svc.GetProviderTypeById(providerTypeId);
        if (Methods.HasRows(ds))
        {
            rtn = Methods.GetStringValue(ds.Tables[0].Rows[0], "MMIS_PROVIDER_TYPE_ID");
        }
        return rtn;
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
            this.DropDownFiscal.Items.Insert(1, CON.FiscalYear.Year.ToString());
            this.DropDownFiscal.Items.Insert(2, CON.FiscalYear.Year1.ToString());
            this.DropDownFiscal.Items.Insert(3, CON.FiscalYear.Year2.ToString());
            this.DropDownFiscal.SelectedIndex = 0;
        }
    }
    //private void LoadCostReportTypeDropDown()
    //{
    //    if (!(this.dlCostReportType.Items.Count > 0))
    //    {
    //        var mmisProviderTypeId = GetMMISProviderTypeID(this.WorkflowPage.ProviderTypeID);
    //        DataSet ds = svc.GetCostReportTypes(mmisProviderTypeId);

    //        if (Helper.HasRows(ds))
    //        {
    //            Helper.LoadDropDown(this.dlCostReportType, ds.Tables[0], "REPORT_TYPE_DESC", "REPORT_TYPE_ID", false);
    //        }
    //    }
    //}

    private void LoadCostReportTypeDropDown(string medicaidNumber)
    {
        DataSet ds = svc.SelectProviderByGRPMedicaidID(medicaidNumber);
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
        this.DataList = dtMisc;
        if (Helper.HasRows(dtMisc))
        {
            DataRow dr = dtMisc.Rows[0];

            var provTypeId = Helper.GetString("PROVIDER_TYPE_ID", dr);
            var mmisProviderTypeId = GetMMISProviderTypeID(Convert.ToInt32(provTypeId));
            var providerTypeId = ddlProvidertype.SelectedValue;
            if(providerTypeId == "" || providerTypeId == null)
            {
                providerTypeId = "1";
            }

            switch (providerTypeId)
            {
                case "1":
                    providerTypeId = "86";
                    break;
                case "2":
                    providerTypeId = "89";
                    break;
                default:
                    break;
            }
                      
            DataSet ds1 = svc.GetCostReportTypes(providerTypeId);
            
            if (Helper.HasRows(ds1))
            {
                Helper.LoadDropDown(this.dlCostReportType, ds1.Tables[0], "REPORT_TYPE_DESC", "REPORT_TYPE_ID", false);
            }
        }
    }
    private void LoadProviderTypeDropDown()
    {
        if (_svc == null)
        {
            _svc = new PDMSService.PDMSServiceClient();
        }
        DataSet dataSet = _svc.GetCRProviderType();
        DataTable dt = dataSet.Tables[0];

        ddlProvidertype.DataSource = dt;
        ddlProvidertype.DataTextField = "CR_Provider_TYPE_NAME";
        ddlProvidertype.DataValueField = "CR_Provider_TYPE_VALUE";
        ddlProvidertype.DataBind();
    }
    public override string IdText
    {
        get { return "ucLTCCostReport_" + this.WorkflowPage.RegistrationId; }
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
    protected void BtnViewDetails_Click(object sender, EventArgs e)
    {

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
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CostReportREGXref");
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        if (Helper.HasRows(dtMisc))
        {
            var dr = dtMisc.Rows[0];
            lnktext.InnerText = GetFileDetails(Helper.GetInt("COST_REPORT_TEXT_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            lnkfile.InnerText = GetFileDetails(Helper.GetInt("COST_REPORT_BACKUP_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            lnktrail.InnerText = GetFileDetails(Helper.GetInt("COST_REPORT_TRAIL_BALANCE_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            lnkdepre.InnerText = GetFileDetails(Helper.GetInt("DEPRECIATION_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            lnkhome.InnerText = GetFileDetails(Helper.GetInt("HOME_OFFICE_COST_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            lnkdoc1.InnerText = GetFileDetails(Helper.GetInt("QUALITY_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            lnkdoc2.InnerText = GetFileDetails(Helper.GetInt("OTHER_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
        }
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

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valLTCCostReport";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ValidateData())
        {
            LoadCostReportSearchGrid();
            cpGrid.Collapsed = false;
        }
       
    }

    protected void ddlProvidertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        string MedicaidNumber = this.WorkflowPage.MedicaidID;   //Request.QueryString["MedicaidNumber"];
        LoadCostReportTypeDropDown(MedicaidNumber);

    }

    protected void btnCancelCostReport_Click(object sender, EventArgs e)
    {
        txtCRDocumentNumber.Text = string.Empty;
        txtMedicaidID.Text = string.Empty;
        txtNote1.Text = string.Empty;
        txtNote2.Text = string.Empty;
        txtSubmissionID.Text = string.Empty;
        txtsubmitdate.Text = string.Empty;
        txtTrackingNumber.Text = string.Empty;
        TextBox1.Text = string.Empty;
        TextBox2.Text = string.Empty;
        TextBox3.Text = string.Empty;
        ddlProvidertype.SelectedIndex = 0;
        dlCostReportType.SelectedIndex = 0;
        Dropdowncostrstatus.SelectedIndex = 0;
        DropDownFiscal.SelectedIndex = 0;
        string MedicaidNumber = this.WorkflowPage.MedicaidID;   //Request.QueryString["MedicaidNumber"];
        LoadCostReportTypeDropDown(MedicaidNumber);

    }
}
