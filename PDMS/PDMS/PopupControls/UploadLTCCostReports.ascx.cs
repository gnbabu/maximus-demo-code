using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MMSWebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using Corp.Core.Libraries;

public partial class PopupControls_UploadLTCCostReports : BaseSectionControl
{
    private const string sectionName = "LTCCostReport";

    #region Upload File
    protected int _MaxFileMegaBytes;
    private string _DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
    private string _ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt,acrbak";
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
    #endregion

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
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        string MedicaidNumber = Request.QueryString["MedicaidNumber"];
        LoadProviderInformation(MedicaidNumber);
        LoadCostReportGrid();
        ShowComponentsBasedonRole();
        if (!Page.IsPostBack)
        {
            LoadFacilityProgramTypeDropDown(MedicaidNumber);
            LoadCostReportTypeDropDown(MedicaidNumber);
            HideSubmitPanel();
            HideRejectionPanel();
        }
       
        //this.upCostReportBackupFile.d = "";
    }
    private void ShowComponentsBasedonRole()
    {
        var isValid = Helper.IsUserInSubRole(Convert.ToInt32(RegIdTxt.Value), Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.UserRoleType.SignApproveLTCCR);
        if (isValid)
        {
            divSaveCostReport.Visible = false;
        }
        else
        {
            divSaveCostReport.Visible = true;
        }

        isValid = Helper.IsUserInSubRole(Convert.ToInt32(RegIdTxt.Value), Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.UserRoleType.PrepareSaveLTCCR);
        if (isValid)
        {
            divRejectCostReport.Visible = false;
            divSubmitCostReport.Visible = false;
        }
        else
        {
            divRejectCostReport.Visible = true;
            divSubmitCostReport.Visible = true;
        }
    }
    private void LoadProviderInformation(string medicaidNumber)
    {
        DataSet ds = svc.SelectProviderByGRPMedicaidID(medicaidNumber);
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
        this.DataList = dtMisc;
        if (Helper.HasRows(dtMisc))
        {
            DataRow dr = dtMisc.Rows[0];
            this.lblMedicaidTxt.Text = Helper.GetString("MEDICAID_ID", dr);
            this.lblNPITxt.Text = Helper.GetString("NPI", dr);
            this.lblProNameTxt.Text = Helper.GetString("NAME", dr);
            this.RegIdTxt.Value = Helper.GetString("REG_ID", dr);
            var provTypeId = Helper.GetString("PROVIDER_TYPE_ID", dr);
            var mmisProviderTypeId = GetMMISProviderTypeID(Convert.ToInt32(provTypeId));
            int variable = 0;
            int.TryParse(mmisProviderTypeId, out variable);

            if (CON.ProviderTypeNumerics.NURSING_FACILITY == variable)
            {
                lblQuality.Text = "Other Document (doc, pdf, xlsx, zip)";
                lblQualityDesc.Text = "Other Document Description";
            }
            else
            {
                lblQuality.Text = "Quality Document (doc, pdf, xlsx, zip)";
                lblQualityDesc.Text = "Quality Document Description";
            }
        }

    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {

        string MedicaidNumber = Request.QueryString["MedicaidNumber"];
        DataSet ds = svc.SelectProviderByGRPMedicaidID(MedicaidNumber);
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
        this.DataList = dtMisc;
        if (Helper.HasRows(dtMisc))
        {
            DataRow dr = dtMisc.Rows[0];
            this.spProvName.Text = Helper.GetString("NAME", dr);
            this.spMedicaid.Text = MedicaidNumber;
            var slectedList = GetSelectedItems();
            foreach (var item in slectedList)
            {
                DataSet ds1 = svc.SelectLTCCostReportById(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), item);
                if (Helper.HasRows(ds))
                {
                    var dr1 = ds1.Tables[0].Rows[0];
                    if (Convert.ToInt32(dr1["SUBMISSION_STATUS"].ToString()) != CON.CostReportSubmissionStatus.Hold && Convert.ToInt32(dr1["SUBMISSION_STATUS"].ToString()) != CON.CostReportSubmissionStatus.Rejected)
                    {
                        this.spCRFrom.Text = dr1["FROM_DATE"].ToString();
                        this.spCRTo.Text = dr1["END_DATE"].ToString();

                        ShowSubmitPanel();
                        HideRejectionPanel();
                        //cpSubmitterApproval.Collapsed = false;
                        //cpSubmitterApproval.ClientState = "false";
                        //pnlsepSubmitterApproval.Visible = true;
                        //cpGrid.Collapsed = true;
                        //cpGrid.ClientState = "false";
                    }
                    else
                    {
                        AddError("Can't be performed action, due to current status is Hold or Rejected.");
                    }

                }
                break;
            }


        }

    }

    public override void LoadControlData() { }
    public override bool SaveData() { return true; }
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
    private void LoadFacilityProgramTypeDropDown(string medicaidNumber)
    {
        DataSet ds = svc.SelectProviderByGRPMedicaidID(medicaidNumber);
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
        this.DataList = dtMisc;
        if (Helper.HasRows(dtMisc))
        {
            DataRow dr = dtMisc.Rows[0];

            var provTypeId = Helper.GetString("PROVIDER_TYPE_ID", dr);
            var mmisProviderTypeId = GetMMISProviderTypeID(Convert.ToInt32(provTypeId));
            DataSet ds1 = svc.GetFacilityProgrameTypes(mmisProviderTypeId);

            if (Helper.HasRows(ds1))
            {
                Helper.LoadDropDown(this.dlFacProgType, ds1.Tables[0], "Facility_Program_Type_NAME", "Facility_Program_Type_VALUE", false);
            }
        }


    }

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
            DataSet ds1 = svc.GetCostReportTypes(mmisProviderTypeId);


            if (Helper.HasRows(ds1))
            {
                Helper.LoadDropDown(this.dlCostReportType, ds1.Tables[0], "REPORT_TYPE_DESC", "REPORT_TYPE_ID", false);
            }
        }
    }

    protected void btnSaveCostReport_Click(object sender, EventArgs e)
    {
        SaveCostReport();
    }

    protected void btnCancelCostReport_Click(object sender, EventArgs e)
    {
        upTrailBalance.Dispose();
        upQualityDocument.Dispose();
        upOtherDocument.Dispose();
        upDepAndAmo.Dispose();
        upCostReportBackupFile.Dispose();
        upCostReportTextFile.Dispose();
        txtCstRptToDate.Text = "";
        txtCstRptFromDate.Text = "";
        txtComments.Text = "";
        txtNote1.Text = "";
        txtNote2.Text = "";
        txtQualityDocument.Text = "";
        dlCostReportType.SelectedIndex = 0;
        dlFacProgType.SelectedIndex = 0;
    }
    public override void LoadData(DataRow dr)
    {
    }

    public override bool ValidateData()
    {
        return true;
    }
    private void AddError(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valLTCCostReport";
        this.Page.Validators.Add(val);
    }

    public override string ValidationGroup
    {
        get { return "valLTCCostReport"; }
    }

    public override string Title
    {
        get { return "LTC Cost Report SEARCH"; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string IdText
    {
        get { return "ucLTCCostReport_" + this.WorkflowPage.RegistrationId; }
    }
    private void SendToCMS(int docID, byte[] fileBytes, string fileName)
    {
        OnBaseInterface onBaseInterface = new OnBaseInterface();
        onBaseInterface.SubmitFile(this.WorkflowPage.RegistrationId, docID, fileBytes, fileName);
    }
    private bool IsValidExtension(EncryptedFileUpload encryptedFile, out string errMsg)
    {
        bool rtn = false;
        errMsg = string.Empty;
        string fileName = encryptedFile.PostedFile.FileName;

        // extarct and store the file extension into another variable
        string fileExtension = System.IO.Path.GetExtension(fileName).Replace(".", string.Empty).ToLower();

        // string type array having list of allowed file type extensions
        string[] validFileExtensions = _ValidFileExtensions.Split(',');
        // loop over the array of valid file extensions to compare them with uploaded file
        foreach (string extension in validFileExtensions)
        {
            if (fileExtension == extension)
            {
                rtn = true;
                break;
            }
        }

        // display the message based on the flag value
        if (!rtn)
        {
            errMsg = "Files with extension <b>\"" + fileExtension + "\"</b> are not allowed.<br />";
            errMsg += "You can upload files with the following extensions only:";
            foreach (string str in validFileExtensions)
            {
                errMsg += " ." + str + ",";
            }
            errMsg = errMsg.Substring(0, errMsg.Length - 1);            // Remove "," at end
        }
        return rtn;
    }
    private bool IsSpecialCharacter(string strFileName)
    {
        string pattern = Helper.GetAppSettingFromDB("RegexPatternForSpecialCharacter", string.Empty);
        if (string.IsNullOrEmpty(pattern)) return true;                 // TRUE if the configuration setting does not exist
        Regex objAlphaPattern = new Regex(pattern);
        return objAlphaPattern.IsMatch(strFileName);
    }

    private bool ValidateFile(EncryptedFileUpload encryptedFile, Label lblStatusMsg)
    {
        var result = false;
        _DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        lblStatusMsg.Text = string.Empty;
        if (encryptedFile.PostedFile == null)
        {
            lblStatusMsg.Text = "Unable to find posted file.";
            return result;
        }
        if (string.IsNullOrEmpty(encryptedFile.PostedFile.FileName))
        {
            lblStatusMsg.Text = "Select a file for upload";
            return result;
        }
        if (string.IsNullOrEmpty(_DestinationPath))
        {
            lblStatusMsg.Text = "ERROR - DestinationPath must have a value";
            return result;
        }
        if (string.IsNullOrEmpty(_ValidFileExtensions))
        {
            lblStatusMsg.Text = "ERROR - ValidFileExtensions must have a value";
            return result;
        }
        string errMsg = string.Empty;
        if (!IsValidExtension(encryptedFile, out errMsg))
        {
            lblStatusMsg.Text = errMsg;
            return result;
        }
        if (!encryptedFile.HasFile)
        {
            lblStatusMsg.Text = "No File has been uploaded.";
            return result;
        }
        if (encryptedFile.PostedFile.ContentLength > (MaxFileMegaBytes * (1000 * 1024)))
        {
            lblStatusMsg.Text = "File cannot be more than " + String.Format("{0:0,0}", (MaxFileMegaBytes * (1000 * 1024))) + " bytes (" +
                String.Format("{0:f}", MaxFileMegaBytes) + " MB) in size.";
            return result;
        }

        if (!IsSpecialCharacter(encryptedFile.FileName))
        {
            lblStatusMsg.Text = "The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): " + Helper.HtmlEncode(encryptedFile.FileName) + " Please remove the Special Character from the file Name before upload. ";
            return result;
        }

        if (!string.IsNullOrEmpty(errMsg))
        {
            lblStatusMsg.Text = Helper.HtmlEncode(errMsg);
            return result;
        }
        result = true;
        return result;
    }
    private int UploadDocument(EncryptedFileUpload encryptedFile, Label lblStatusMsg, string fileDescription = "")
    {
        try
        {
            // Use a different destination if debugging
            //#if DEBUG
            //_DestinationPath = "C:\\Projects\\PDMS\\PDMS\\FileStoreLocal\\";
            //#endif
            //_DestinationPath = "C:\\Projects\\Upload\\";

            if (System.Diagnostics.Debugger.IsAttached)
                _DestinationPath = @"C:\Temp\";

            string newFileName = Helper.CleanFilePath(encryptedFile.FileName);
            byte[] fileBytes = encryptedFile.EncryptedFileBytes;
            // Rename the file
            //filUploadFile.SaveAs(@_DestinationPath + newFileName);
            File.WriteAllBytes(Path.Combine(@_DestinationPath, newFileName), fileBytes);
            //lblStatusMsg.Text = "File Uploaded: " + newFileName;
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.RegIdTxt.Value.ToString());
            int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
            parms.Add("REG_PAGE_TYPE_ID", pageTypeID.ToString());
            parms.Add("REG_PAGE_SECTION", sectionName);
            parms.Add("SCREENING_ACTIVITY_ID", null);
            parms.Add("NAME", newFileName);
            parms.Add("DESCRIPTION", fileDescription);
            parms.Add("FILE_NAME", newFileName);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            //parms.Add("REG_SECTION_UPLOAD_CONTROL_ID", ID);
            int docID = svc.InsertRegistrationData(Convert.ToInt32(this.RegIdTxt.Value), "COST_REPORT_DOCUMENT", parms);
            SendToCMS(docID, fileBytes, newFileName);
            return docID;
        }
        catch (Exception ex)
        {
            lblStatusMsg.Text = "No File uploaded. Error: " + Helper.HtmlEncode(ex.Message);
        }
        return 0;
    }

    private void SaveCostReport()
    {
        if (Validate())
        {
            if (string.IsNullOrEmpty(txtTrackingId.Text.Trim()))
            {
                DataSet dataSet = svc.GetMMISTrackingDetails();
                if (Helper.HasRows(dataSet))
                {
                    DataRow dr = dataSet.Tables[3].Rows[0];
                    txtTrackingId.Text = Helper.GetString("MITSTrackingNumber", dr);
                }
            }

            var CostReportTextFileId = UploadDocument(this.upCostReportTextFile, this.lblCostReportTextFile);
            var CostReportBackupFileId = UploadDocument(this.upCostReportBackupFile, this.lblCostReportBackupFile);
            var TrailBalanceId = UploadDocument(this.upTrailBalance, this.lblTrailBalance);
            var DepAndAmoId = UploadDocument(this.upDepAndAmo, this.lblDepAndAmo);
            var OtherDocumentId = 0;

            if (upOtherDocument.PostedFile != null && !string.IsNullOrEmpty(upOtherDocument.PostedFile.FileName) && upOtherDocument.PostedFile.ContentLength > 0)
            {
                OtherDocumentId = UploadDocument(this.upOtherDocument, this.lblOtherDocument, this.txtOtherDocumentDescription.Text);
            }
            var QualityDocumentId = 0;
            if (upQualityDocument.PostedFile != null && !string.IsNullOrEmpty(upQualityDocument.PostedFile.FileName) && upQualityDocument.PostedFile.ContentLength > 0)
            {
                QualityDocumentId = UploadDocument(this.upQualityDocument, this.lblQualityDocument, this.txtQualityDocument.Text);
            }
            var HomeOfficeCostAllocationId = 0;
            if (upHomeOfficeCostAllocation.PostedFile != null && !string.IsNullOrEmpty(upHomeOfficeCostAllocation.PostedFile.FileName) && upHomeOfficeCostAllocation.PostedFile.ContentLength > 0)
            {
                HomeOfficeCostAllocationId = UploadDocument(this.upHomeOfficeCostAllocation, this.lblHomeOfficeCostAllocation);
            }

            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.RegIdTxt.Value.ToString());
            int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
            parms.Add("REG_PAGE_TYPE_ID", pageTypeID.ToString());
            parms.Add("REG_PAGE_SECTION", sectionName);
            parms.Add("COST_REPORT_TEXT_DOCUMENT_ID", CostReportTextFileId.ToString());
            parms.Add("COST_REPORT_BACKUP_DOCUMENT_ID", CostReportBackupFileId.ToString());
            parms.Add("COST_REPORT_TRAIL_BALANCE_DOCUMENT_ID", TrailBalanceId.ToString());
            parms.Add("DEPRECIATION_DOCUMENT_ID", DepAndAmoId.ToString());
            parms.Add("QUALITY_DOCUMENT_ID", QualityDocumentId.ToString());
            parms.Add("OTHER_DOCUMENT_ID", OtherDocumentId.ToString());
            parms.Add("HOME_OFFICE_COST_DOCUMENT_ID", HomeOfficeCostAllocationId.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            int result = svc.InsertRegistrationData(Convert.ToInt32(this.RegIdTxt.Value), "CostReportDocuments", parms);

            //Insert into cost report table

            parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.RegIdTxt.Value.ToString());
            parms.Add("FACILITY_PROGRAM_TYPE_ID", dlFacProgType.SelectedValue);
            parms.Add("COST_REPORT_TYPE", dlCostReportType.SelectedValue);
            parms.Add("FROM_DATE", txtCstRptFromDate.Text);
            parms.Add("END_DATE", txtCstRptToDate.Text);
            parms.Add("TRACKING_ID", txtTrackingId.Text);
            parms.Add("OPERATION", "INSERT");
            parms.Add("SUBMISSION_STATUS", CON.CostReportSubmissionStatus.Hold.ToString());
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            int result1 = svc.InsertRegistrationData(Convert.ToInt32(this.RegIdTxt.Value), "CostReport", parms);
            txtTrackingId.Text = "";
            txtCstRptFromDate.Text = "";
            txtCstRptToDate.Text = "";
            txtTrackingId.Text = "";
            LoadCostReportGrid();
            cpGrid.Collapsed = false;
            cpeInstructions.Collapsed = true;
            pnlSepInstructions.Visible = false;
            cpeInstructions.ClientState = "false";
            cpGrid.ClientState = "false";
        }
    }

    private bool Validate()
    {
        var isvalid = true;
        if (string.IsNullOrEmpty(txtCstRptFromDate.Text.Trim()))
        {
            lblCRFromdate.Text = "Cost report from date is mandatory.";
            isvalid = false;
        }
        if (string.IsNullOrEmpty(txtCstRptToDate.Text.Trim()))
        {
            lblCRTodate.Text = "Cost report to date is mandatory.";
            isvalid = false;
        }
        //if (ValidateFile(this.upOtherDocument, this.lblOtherDocument))
        //{
        //    if (string.IsNullOrEmpty(txtOtherDocumentDescription.Text.Trim()))
        //    {
        //        AddError("Other document description is mandatory.");
        //    }
        //    isvalid = false;
        //}

        if (!ValidateFile(this.upCostReportTextFile, this.lblCostReportTextFile))
        {
            isvalid = false;
        }
        else
        {
            var result = upCostReportTextFile.FileName.Split('.');
            var filename = result[0];
            var extension = result[1];
            if (extension != "txt")
            {
                this.lblCostReportTextFile.Text = "Incorrect file extension";
                isvalid = false;
            }
            if (filename.Length == 8 && filename.StartsWith("P"))
            {
                int count = filename.ToCharArray().Count(c => c == 'n');
                if (count != 7)
                {
                    this.lblCostReportTextFile.Text = "Incorrect file name";
                    isvalid = false;
                }
            }
            else
            {
                this.lblCostReportTextFile.Text = "Incorrect file name";
                isvalid = false;
            }

        }

        if (!ValidateFile(this.upCostReportBackupFile, this.lblCostReportBackupFile))
        {
            isvalid = false;
        }
        else
        {
            var result = upCostReportBackupFile.FileName.Split('.');
            var filename = result[0];
            var extension = result[1];
            if (extension != "acrbak")
            {
                this.lblCostReportBackupFile.Text = "Incorrect file extension";
                isvalid = false;
            }
            if (filename.Length == 8 && filename.StartsWith("P"))
            {
                int count = filename.ToCharArray().Count(c => c == 'n');
                if (count != 7)
                {
                    this.lblCostReportBackupFile.Text = "Incorrect file name";
                    isvalid = false;
                }
            }
            else
            {
                this.lblCostReportBackupFile.Text = "Incorrect file name";
                isvalid = false;
            }
        }
        if (!ValidateFile(this.upTrailBalance, this.lblTrailBalance))
        {
            isvalid = false;
        }
        if (!ValidateFile(this.upDepAndAmo, this.lblDepAndAmo))
        {
            isvalid = false;
        }


        DateTime value;
        if (!String.IsNullOrEmpty(txtCstRptFromDate.Text))
        {
            if (DateTime.TryParse(txtCstRptFromDate.Text, out value))
            {
                if (DateTime.Compare(DateTime.Now.Date, Convert.ToDateTime(txtCstRptFromDate.Text)) < 0)
                {
                    lblCRFromdate.Text = "* Date From cannot be after todays date.";
                    isvalid = false;
                }
            }
            else
            {
                lblCRFromdate.Text = "* Date From is not a date format.";
                isvalid = false;
            }
        }

        if (!String.IsNullOrEmpty(txtCstRptToDate.Text))
        {
            if (DateTime.TryParse(txtCstRptToDate.Text, out value))
            {
                if (DateTime.Compare(DateTime.Now.Date, Convert.ToDateTime(txtCstRptToDate.Text)) < 0)
                {
                    lblCRTodate.Text = "* Date To cannot be after todays date.";
                    isvalid = false;
                }
            }
            else
            {
                lblCRTodate.Text = "* Date To is not a date format.";
                isvalid = false;
            }
        }

        if (!String.IsNullOrEmpty(txtCstRptFromDate.Text) && !String.IsNullOrEmpty(txtCstRptToDate.Text))
        {
            DateTime enteredFrom = DateTime.Parse(txtCstRptFromDate.Text);
            DateTime enteredTo = DateTime.Parse(txtCstRptToDate.Text);
            if (DateTime.Compare(enteredFrom, enteredTo) > 0)
            {
                lblCRFromdate.Text = "* Choose DateFrom value prior to DateTo value";
                isvalid = false;
            }

        }
        return isvalid;

    }

    private void LoadCostReportGrid()
    {
        DataSet dataSet = svc.SelectRegCostReport(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Convert.ToInt32(this.RegIdTxt.Value));
        if (Helper.HasRows(dataSet))
        {
            cpGrid.Collapsed = false;
            cpGrid.ClientState = "false";
            cpeInstructions.Collapsed = true;
            cpeInstructions.ClientState = "false";
        }
        else
        {
            cpeInstructions.Collapsed = false;
            cpeInstructions.ClientState = "false";
            cpGrid.Collapsed = true;
            cpGrid.ClientState = "false";
        }
        PlaceHolder1.Controls.Clear();
        PlaceHolder1.Controls.Add(DataTableToHTMLTable(dataSet.Tables[0]));
    }
    private void LoadCostReportPage()
    {
        var slectedList = GetSelectedItems();
        foreach (var item in slectedList)
        {
            if (IsValidStatus(item))
            {
                DataSet ds = svc.SelectLTCCostReportById(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), item);
                if (Helper.HasRows(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    this.RegIdTxt.Value = Helper.GetString("REG_ID", dr);
                    this.dlFacProgType.SelectedValue = Helper.GetString("FACILITY_PROGRAM_TYPE_ID", dr);
                    this.dlCostReportType.SelectedValue = Helper.GetString("COST_REPORT_TYPE", dr);
                    this.txtCstRptFromDate.Text = Helper.GetString("FROM_DATE", dr);
                    this.txtCstRptToDate.Text = Helper.GetString("END_DATE", dr);
                    this.txtTrackingId.Text = Helper.GetString("TRACKING_ID", dr);

                }
            }
            else
            {
                AddError("Can't be performed action, due to current status is Approved or Notifed :");
            }

            break;
        }
    }


    public Table DataTableToHTMLTable(DataTable dt)
    {
        Table tbl = new Table();
        tbl.ID = "LTCCostReportGrid";
        TableRow tr = null;
        TableCell cell = null;

        int rows = dt.Rows.Count;
        int cols = dt.Columns.Count;

        // Bug fix OHPNM-1336: As per workflow SignLTC (Assign approve) should able to see only 'Notified', 'Rejected' and 'Approved' Reports.  
        DataRow[] filteredRows = dt.Select();
        int regId = Convert.ToInt32(RegIdTxt.Value);
        string userId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

        bool isSignApproveRole = Helper.IsUserInSubRole(regId, userId, CON.UserRoleType.SignApproveLTCCR);

        if (isSignApproveRole)
        {
            // filtering records only in 'Notified', 'Rejected' and 'Approved' statuses
            string expression = "Submission_Status in (4)";
            filteredRows = dt.Select(expression);
            rows = filteredRows.Count();
        }
        bool isPreparerLTCRole = Helper.IsUserInSubRole(regId, userId, CON.UserRoleType.PrepareSaveLTCCR);
        if (isPreparerLTCRole)
        {
            // filtering records only in 'Hold', 'Rejected'
            string expression = "Submission_Status in (1,2,4)";
            filteredRows = dt.Select(expression);
            rows = filteredRows.Count();
        }

        // table header
        TableHeaderRow htr = new TableHeaderRow();
        TableHeaderCell hcell = null;
        hcell = new TableHeaderCell();
        hcell.Text = "LTC COST REVIEW AND SUBMISSION";
        hcell.ColumnSpan = 11;
        htr.Cells.Add(hcell);
        htr.BackColor = ColorTranslator.FromHtml("#4080bf");
        tbl.Rows.Add(htr);

        htr = new TableHeaderRow();
        htr.BackColor = ColorTranslator.FromHtml("#c6d9ec");

        hcell = new TableHeaderCell();
        hcell.Text = "";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Program Facility Type";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Provider Medicaid ID";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Preparer ID";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Cost ReportType";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Cost Report From Date";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Cost Report To Date";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Submission Status";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "Tracking ID";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "CR Status Date";
        htr.Cells.Add(hcell);

        hcell = new TableHeaderCell();
        hcell.Text = "CR Status Time";
        htr.Cells.Add(hcell);

        tbl.Rows.Add(htr);

        // Table Header

        // Table rows
        for (int j = 0; j < rows; j++)
        {
            tr = new TableRow();
            for (int k = 0; k < cols; k++)
            {
                cell = new TableCell();
                if (k == 0)
                {
                    CheckBox checkBox = new CheckBox();
                    checkBox.ID = filteredRows[j][k].ToString();
                    checkBox.Checked = false;
                    cell.Controls.Add(checkBox);
                    tr.Cells.Add(cell);
                }
                else if (k == 7)
                {
                    cell.Text = GetSubmissionStatus(Convert.ToInt32(filteredRows[j][k]));
                }
                else
                {
                    cell.Text = filteredRows[j][k].ToString();
                }
                tr.Cells.Add(cell);
            }

            tr.BackColor = ColorTranslator.FromHtml("#ecf2f9");
            tbl.Rows.Add(tr);
        }
        // Table rows

        // Table footer
        TableFooterRow ftr = new TableFooterRow();
        hcell = new TableHeaderCell();
        if (Helper.IsUserInSubRole(regId, userId, CON.UserRoleType.PrepareSaveLTCCR))
        {
            if (Helper.HasRows(dt))
            {

                Button Edit = new Button();
                Edit.Click += new EventHandler(BtnEdit_Click);
                Edit.Text = "Edit";
                Edit.BackColor = ColorTranslator.FromHtml("#00ace6");
                hcell.Controls.Add(Edit);

                Button Delete = new Button();
                Delete.BackColor = ColorTranslator.FromHtml("#ff3333");
                Delete.Text = "Delete";
                Delete.Click += new EventHandler(BtnDelete_Click);
                hcell.Controls.Add(Delete);
                Button ViewDetails = new Button();
                ViewDetails.Text = "View Details";
                ViewDetails.BackColor = ColorTranslator.FromHtml("#00ace6");
                ViewDetails.Click += new EventHandler(BtnViewDetails_Click);
                hcell.Controls.Add(ViewDetails);
                Button NotifyAgent = new Button();
                NotifyAgent.Text = "Notify Agent";
                NotifyAgent.BackColor = ColorTranslator.FromHtml("#00ace6");
                NotifyAgent.Click += new EventHandler(BtnNotifyAgent_Click);
                hcell.Controls.Add(NotifyAgent);
            }

        }

        if (isSignApproveRole)
        {
            if (Helper.HasRows(dt))
            {
                Button ViewDetails = new Button();
                ViewDetails.Text = "View Details";
                ViewDetails.BackColor = ColorTranslator.FromHtml("#00ace6");
                ViewDetails.Click += new EventHandler(BtnViewDetails_Click);
                hcell.Controls.Add(ViewDetails);
                Button Submit = new Button();
                Submit.Text = "Submit";
                Submit.BackColor = ColorTranslator.FromHtml("#00ace6");
                Submit.Click += new EventHandler(BtnSubmit_Click);
                hcell.Controls.Add(Submit);
                Button Reject = new Button();
                Reject.Text = "Reject";
                Reject.BackColor = ColorTranslator.FromHtml("#ff3333");
                Reject.Click += new EventHandler(BtnReject_Click);
                hcell.Controls.Add(Reject);
            }
        }

        hcell.ColumnSpan = 11;
        ftr.Cells.Add(hcell);
        tbl.Rows.Add(ftr);
        // Table footer

        return tbl;
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

    protected void BtnEdit_Click(object sender, EventArgs e)
    {
        LoadCostReportPage();
        cpGrid.Collapsed = true;
        cpGrid.ClientState = "false";
        cpeInstructions.Collapsed = false;
        pnlSepInstructions.Visible = true;
        cpeInstructions.ClientState = "false";
    }

    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        var slectedList = GetSelectedItems();
        foreach (var item in slectedList)
        {
            if (IsValidStatus(item))
            {
                var parms = new Dictionary<string, string>();
                parms.Add("REG_ID", this.RegIdTxt.Value.ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("OPERATION", "DELETE");
                parms.Add("REG_COST_REPORT_ID", item.ToString());
                int result1 = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "CostReport", parms);
            }
            else
            {
                AddError("Can't be performed action, due to current status is Approved or Notifed :");
            }
            break;
        }
        LoadCostReportGrid();
        cpGrid.Collapsed = true;
        cpGrid.ClientState = "false";
    }

    private List<int> GetSelectedItems()
    {
        var selectedList = new List<int>();
        var table = PlaceHolder1.FindControl("LTCCostReportGrid") as Table;
        int count = 1;
        foreach (TableRow row in table.Rows)
        {
            if (count > 2 && count < table.Rows.Count)
            {
                var checkBox = (CheckBox)row.Cells[0].Controls[0]; //Assuming the first control of the first cell is always a CheckBox.

                if (checkBox.Checked)
                {
                    selectedList.Add(Convert.ToInt32(checkBox.ID));
                    /* Do Stuff With col2 */
                }
            }
            count = count + 1;
        }
        return selectedList;
    }

    
    protected void BtnReject_Click(object sender, EventArgs e)
    {

        var slectedList = GetSelectedItems();
        foreach (var item in slectedList)
        {
            DataSet ds = svc.SelectLTCCostReportById(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), item);
            if (Helper.HasRows(ds))
            {
                var dr = ds.Tables[0].Rows[0];
                if (Convert.ToInt32(dr["SUBMISSION_STATUS"].ToString()) != CON.CostReportSubmissionStatus.Hold && Convert.ToInt32(dr["SUBMISSION_STATUS"].ToString()) != CON.CostReportSubmissionStatus.Approved)
                {
                    //cpSubmitterRejection.Collapsed = false;
                    //cpSubmitterRejection.ClientState = "false";

                    //pnlsepSubmitterRejection.Visible = true;
                    //cpGrid.Collapsed = true;
                    //cpGrid.ClientState = "false";
                    ShowRejectionPanel();
                    HideSubmitPanel();

                }
                else
                {
                    AddError("Can't be performed action, due to current status is Hold or Approved.");
                }
            }

        }

    }

    protected void BtnNotifyAgent_Click(object sender, EventArgs e)
    {
        var slectedList = GetSelectedItems();
        foreach (var item in slectedList)
        {
            var parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.RegIdTxt.Value.ToString());
            parms.Add("SUBMISSION_STATUS", CON.CostReportSubmissionStatus.Notified.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("OPERATION", "UPDATE");
            parms.Add("REG_COST_REPORT_ID", item.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            int result1 = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "CostReport", parms);
            Notification notification = new Notification();
            notification.SendWorkFlowEngineNotification(this.RegIdTxt.Value.ToString(), "EMAIL_TEMPLATE_NOTIFY_AGENT");
        }
        LoadCostReportGrid();
    }

    protected void btnConfirmRejection_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtComments.Text))
        {
            lblComments.Visible = true;
        }
        else
        {
            lblComments.Visible = false;
            var slectedList = GetSelectedItems();
            foreach (var item in slectedList)
            {
                var parms = new Dictionary<string, string>();
                parms.Add("REG_ID", this.RegIdTxt.Value.ToString());
                parms.Add("SUBMISSION_STATUS", CON.CostReportSubmissionStatus.Rejected.ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("OPERATION", "UPDATE");
                parms.Add("REG_COST_REPORT_ID", item.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                int result1 = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "CostReport", parms);
                this.txtOtherDocumentDescription.Text = "";
            }
            LoadCostReportGrid();
            HideSubmitPanel();
            HideRejectionPanel();
            btnConfirmRejection.Attributes.Add("AutoPostBack", "false");
        }

    }
    private void ShowSubmitPanel()
    {
        cpSubmitterApproval.Collapsed = false;
        cpSubmitterApproval.ClientState = "false";
        pnlSubmitterApproval.Visible = true;

        pnlsepSubmitterApproval.Visible = true;
        cpGrid.Collapsed = true;
        cpGrid.ClientState = "false";
    }
    private void HideSubmitPanel()
    {
        cpSubmitterApproval.Collapsed = true;
        cpSubmitterApproval.ClientState = "false";
        pnlSubmitterApproval.Visible = false;
        pnlsepSubmitterApproval.Visible = false;
        cpGrid.Collapsed = false;
        cpGrid.ClientState = "false";
               
    }
    private void ShowRejectionPanel()
    {
        cpSubmitterRejection.Collapsed = false;
        cpSubmitterRejection.ClientState = "false";
        pnlSubmitterRejection.Visible = true;

        pnlsepSubmitterRejection.Visible = true;
        cpGrid.Collapsed = true;
        cpGrid.ClientState = "false";
    }
    private void HideRejectionPanel()
    {
        cpSubmitterRejection.Collapsed = true;
        cpSubmitterRejection.ClientState = "false";
        pnlsepSubmitterRejection.Visible = false;
        pnlSubmitterRejection.Visible = false;
        cpGrid.Collapsed = false;
        cpGrid.ClientState = "false";
    }
    protected void btnCancelRejection_Click(object sender, EventArgs e)
    {
        //cpSubmitterRejection.Collapsed = true;
        //cpSubmitterRejection.ClientState = "false";
        //pnlsepSubmitterRejection.Visible = false;
        //pnlSubmitterRejection.Visible = false;
        //cpGrid.Collapsed = false;
        //cpGrid.ClientState = "false";
        HideRejectionPanel();
    }
    protected void btnConfirmApproval_Click(object sender, EventArgs e)
    {
        divSign.Visible = true;
        cpCredentail.Collapsed = false;
        cpCredentail.ClientState = "false";
        DataSet ds = svc.SelectRegistrationData(Convert.ToInt32(this.RegIdTxt.Value.ToString()), "UserAccountInfo");
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        if (Helper.HasRows(ds))
        {
            var dt = ds.Tables[0];
            txtSubName.Text = dt.Rows[0]["CONTACT_NAME"].ToString();
        }

    }

    protected void btnCancelApproval_Click(object sender, EventArgs e)
    {
        //cpSubmitterApproval.Collapsed = true;
        //cpSubmitterApproval.ClientState = "false";
        //pnlsepSubmitterApproval.Visible = false;
        //cpGrid.Collapsed = false;
        //cpGrid.ClientState = "false";

        HideSubmitPanel();
    }

    private bool IsValidStatus(int regCostReportId)
    {
        DataSet ds = svc.SelectLTCCostReportById(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), regCostReportId);
        if (Helper.HasRows(ds))
        {
            var dr = ds.Tables[0].Rows[0];
            if (Convert.ToInt32(dr["SUBMISSION_STATUS"].ToString()) == CON.CostReportSubmissionStatus.Approved || Convert.ToInt32(dr["SUBMISSION_STATUS"].ToString()) == CON.CostReportSubmissionStatus.Notified)
            {
                return false;

            }
        }
        return true;
    }

    protected void BtnViewDetails_Click(object sender, EventArgs e)
    {
        var slectedList = GetSelectedItems();
        foreach (var item in slectedList)
        {
            DataSet ds = svc.SelectLTCCostReportById(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), item);
            if (Helper.HasRows(ds))
            {
                var dr = ds.Tables[0].Rows[0];
                lblcost.Text = "Cost Report Type: " + dr["REPORT_TYPE_DESC"].ToString();
                lblmedicaidID.Text = "Provider Medicaid ID: " + dr["MEDICAID_ID"].ToString();
                lblTrackID.Text = "Tracking ID: " + dr["TRACKING_ID"].ToString();
                lblstatus.Text = "Submission Status: " + GetSubmissionStatus(Convert.ToInt32(dr["SUBMISSION_STATUS"].ToString()));
                lblCrDate.Text = "CR Status Date: " + dr["CR_Status_Date"].ToString();
                lblCrTime.Text = "CR Status Time: " + dr["CR_Status_time"].ToString();
                lblReportFrom.Text = "Cost Report From Date: " + dr["FROM_DATE"].ToString();
                lblReportTo.Text = "Cost Report From To: " + dr["END_DATE"].ToString();
                lblName.Text = HttpContext.Current.User.Identity.Name;
                GetRegCostReportDocuments();


            }
            break;
        }
        cpGrid.Collapsed = true;
        cpGrid.ClientState = "false";
        cpViewDetails.Collapsed = false;
        cpViewDetails.ClientState = "false";

    }


    private void GetRegCostReportDocuments()
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CostReportREGXref");
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        if (Helper.HasRows(dtMisc))
        {
            var dr = dtMisc.Rows[0];
            lnktext.Text = GetFileDetails(Helper.GetInt("COST_REPORT_TEXT_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            lnkfile.Text = GetFileDetails(Helper.GetInt("COST_REPORT_BACKUP_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            lnktrail.Text = GetFileDetails(Helper.GetInt("COST_REPORT_TRAIL_BALANCE_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            lnkdepre.Text = GetFileDetails(Helper.GetInt("DEPRECIATION_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            if (Helper.GetInt("HOME_OFFICE_COST_DOCUMENT_ID", dr) > 0)
            {
                lnkhome.Text = GetFileDetails(Helper.GetInt("HOME_OFFICE_COST_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            }
            else
            {
                lnkhome.Text = string.Empty;
            }
            if (Helper.GetInt("QUALITY_DOCUMENT_ID", dr) > 0)
            {
                lnkdoc1.Text = GetFileDetails(Helper.GetInt("QUALITY_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            }
            else
            {
                lnkdoc1.Text = string.Empty;
            }

            if (Helper.GetInt("OTHER_DOCUMENT_ID", dr) > 0)
            {
                lnkdoc2.Text = GetFileDetails(Helper.GetInt("OTHER_DOCUMENT_ID", dr))["FILE_NAME"].ToString();
            }
            else
            {
                lnkdoc2.Text = string.Empty;
            }

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
            filePath = Path.Combine(Helper.GetAppSettingFromDB("FileStorePath", string.Empty), Helper.CleanFilePath(fileName));
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
    private bool IsCaptchaValid()
    {
        Captcha1.ValidateCaptcha(txtCaptcha.Text);

        if (Captcha1.UserValidated)
        {
            return true;
        }
        else
        {
            AddError("* CAPTCHA must be validated.");
            return false;
        }
    }

    private bool HasPassword()
    {
        if (string.IsNullOrEmpty(txtPassword.Text))
        {
            AddError("* Password is required.");
            return false;
        }

        return true;
    }

    private bool IsSigned()
    {
        return Membership.ValidateUser(HttpContext.Current.User.Identity.Name, txtPassword.Text);
    }
    protected void btnSaveSignature_Click(object sender, EventArgs e)
    {
        if (IsCaptchaValid() && HasPassword() && IsSigned())
        {
            var slectedList = GetSelectedItems();
            foreach (var item in slectedList)
            {
                var parms = new Dictionary<string, string>();
                parms.Add("REG_ID", this.RegIdTxt.Value.ToString());
                parms.Add("SUBMISSION_STATUS", CON.CostReportSubmissionStatus.Approved.ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("REG_COST_REPORT_ID", item.ToString());
                parms.Add("OPERATION", "UPDATE");
                int result1 = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "CostReport", parms);
                txtPassword.Text = "";
                txtCaptcha.Text = "";
                LoadCostReportGrid();
                cpGrid.Collapsed = false;
                cpGrid.ClientState = "false";
                break;
            }

            btnSaveSignature.Attributes.Add("AutoPostBack", "true");

        }


    }


    protected void bntCancelSig_Click(object sender, EventArgs e)
    {
        divSign.Visible = false;
        cpCredentail.Collapsed = true;
        cpCredentail.ClientState = "false";
    }


    protected void chkSubmitCr_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSubmitCr.Checked)
        {
            btnconfirmsubmitCr.Enabled = true;
        }
        else
        {
            btnconfirmsubmitCr.Enabled = false;
        }
    }

    protected void chkRejectCr_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRejectCr.Checked)
        {
            btnConfirmRejection.Enabled = true;
        }
        else
        {
            btnConfirmRejection.Enabled = false;
        }
    }
}