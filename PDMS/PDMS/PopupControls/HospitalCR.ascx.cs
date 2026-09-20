using MAXIMUS.Core.Libraries;
using Corp.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MMSWebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_HospitalCR : BaseSectionControl
{
    private const string sectionName = "HospitalCostReport";

    #region Upload File
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
    public int OtherDocCounter
    {
        get
        {
            if (ViewState["txbCounter"] != null)
                return (int)ViewState["txbCounter"];
            else
                return 0;
        }
        set { ViewState["txbCounter"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string MedicaidNumber = this.WorkflowPage.MedicaidID;   //Request.QueryString["MedicaidNumber"];
        LoadProviderInformation(MedicaidNumber);
        LoadFacilityProgramTypeDropDown();
        LoadSettelementTypeDropdown();
        if (!Page.IsPostBack)
        {
            OtherDocCounter = 0;
            HideSubmitPanel();
        }
        //create dynamic TextBoxes again and assign ID
        for (int i = 0; i < OtherDocCounter; i++)
        {
            //Create the dynamic TextBox.
            EncryptedFileUpload textbox = new EncryptedFileUpload();

            textbox.ID = "upOtherDoc_" + i.ToString();
            dynamicCtrl.Controls.Add(textbox);

            //Create the dynamic Button to remove the TextBox.
            Button button = new Button();
            button.ID = "btnDelete_" + i.ToString();
            button.Text = "Delete";
            button.BackColor = System.Drawing.Color.Red;
            button.Click += new System.EventHandler(this.btnDelete_Click);
            dynamicCtrl.Controls.Add(button);
            dynamicCtrl.Controls.Add(new LiteralControl("<br />"));
        }
        SetUserInfo();
    }

    private void ShowSubmitPanel()
    {
        cpSubmitterApproval.Collapsed = false;
        cpSubmitterApproval.ClientState = "false";
        pnlSubmitterApproval.Visible = true;

        pnlsepSubmitterApproval.Visible = true;
    }
    private void HideSubmitPanel()
    {
        cpSubmitterApproval.Collapsed = true;
        cpSubmitterApproval.ClientState = "false";
        pnlSubmitterApproval.Visible = false;
        pnlsepSubmitterApproval.Visible = false;

    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        //Reference the Button which was clicked.
        Button button = (sender as Button);

        //Determine the Index of the Button.
        int index = int.Parse(button.ID.Split('_')[1]);

        //Find the TextBox using Index and remove it.
        dynamicCtrl.Controls.Remove(dynamicCtrl.FindControl("upOtherDoc_" + index));

        //Remove the Button.
        dynamicCtrl.Controls.Remove(button);
        OtherDocCounter--;
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
    public override void LoadData(DataRow dr)
    {
    }
    public override string Title
    {
        get { return "Hospital Cost Report"; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string IdText
    {
        get { return "ucHospitalCostReport_" + this.WorkflowPage.RegistrationId; }
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
    private void LoadFacilityProgramTypeDropDown()
    {
        var mmisProviderTypeId = GetMMISProviderTypeID(this.WorkflowPage.ProviderTypeID);
        DataSet ds = svc.GetFacilityProgrameTypes(mmisProviderTypeId);

        if (Helper.HasRows(ds))
        {
            Helper.LoadDropDown(this.dlFacProgType, ds.Tables[0], "Facility_Program_Type_NAME", "Facility_Program_Type_VALUE", false);
        }
    }
    private void LoadSettelementTypeDropdown()
    {
        DataSet ds = svc.GetSettelementTypes();

        if (Helper.HasRows(ds))
        {
            Helper.LoadDropDown(this.dlSettelementType, ds.Tables[0], "Settlement_TYPE_NAME", "Settlement_TYPE_VALUE", false);
        }
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

            string newFileName = encryptedFile.FileName;
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
            int docID = svc.InsertRegistrationData(Convert.ToInt32(this.RegIdTxt.Value), "DOCUMENT", parms);
            SendToCMS(docID, fileBytes, newFileName);
            return docID;
        }
        catch (Exception ex)
        {
            lblStatusMsg.Text = "No File uploaded. Error: " + ex.Message;
        }
        return 0;
    }
    private void SendToCMS(int docID, byte[] fileBytes, string fileName)
    {
        OnBaseInterface onBaseInterface = new OnBaseInterface();
        onBaseInterface.SubmitFile(this.WorkflowPage.RegistrationId, docID, fileBytes, fileName);
    }
    protected void btnSaveCostReport_Click(object sender, EventArgs e)
    {
        if (!ValidateDocuments())
        {
            HideSubmitPanel();
            cpSubmitterApproval.Collapsed = true;
            docValidatedLabel.Visible = true;
            docValidatedLabel.Text = "Please Select at least one document !";
        }
        else
        {
            docValidatedLabel.Visible = false;
            docValidatedLabel.Text = string.Empty;
            SaveCostReport();
            cpSubmitterApproval.Collapsed = false;
            cpSubmitterApproval.ClientState = "false";
            ShowSubmitPanel();
        }
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

    private bool ValidateDocuments()
    {
        var isValid = true;

        if ((upOtherDOC.PostedFile == null || (string.IsNullOrEmpty(upOtherDOC.PostedFile.FileName) && upOtherDOC.PostedFile.ContentLength <= 0)) &&
            (upMedicaidHospitalCostReport.PostedFile == null || (string.IsNullOrEmpty(upMedicaidHospitalCostReport.PostedFile.FileName) && upMedicaidHospitalCostReport.PostedFile.ContentLength <= 0)) &&
            (upMedicareCostReportEC.PostedFile == null || (string.IsNullOrEmpty(upMedicareCostReportEC.PostedFile.FileName) && upMedicareCostReportEC.PostedFile.ContentLength <= 0)) &&
            (upMedicareCostReportPI.PostedFile == null || (string.IsNullOrEmpty(upMedicareCostReportPI.PostedFile.FileName) && upMedicareCostReportPI.PostedFile.ContentLength <= 0)) &&
            (upCertificationStatement.PostedFile == null || (string.IsNullOrEmpty(upCertificationStatement.PostedFile.FileName) && upCertificationStatement.PostedFile.ContentLength <= 0)) &&
            (upIRISReport.PostedFile == null || (string.IsNullOrEmpty(upIRISReport.PostedFile.FileName) && upIRISReport.PostedFile.ContentLength <= 0))
            )
        {
            isValid = false;
        }


        return isValid;
    }
    private void SaveCostReport()
    {
        if (Validate())
        {
            DataSet dataSet = svc.GetMMISTrackingDetails();
            var attachmentId = 1;
            if (Helper.HasRows(dataSet))
            {
                DataRow dr = dataSet.Tables[3].Rows[0];
                txtTrackingId.Text = Helper.GetString("MITSTrackingNumber", dr);
                DataRow dr1 = dataSet.Tables["AttachmentData"].Rows[0];
                attachmentId = Helper.GetInt("AttachmentID", dr1);
            }


            var postback = Convert.ToBoolean(postBack.Value);


            var OtherDOCId = 0;
            if (upOtherDOC.PostedFile != null && !string.IsNullOrEmpty(upOtherDOC.PostedFile.FileName) && upOtherDOC.PostedFile.ContentLength > 0)
            {
                if (postback)
                {
                    OtherDOCId = UploadDocument(this.upOtherDOC, this.LblOtherDOC, this.txtOtherDescr.Text);
                }
                else
                {
                    txtOtherDOCId.Text = (attachmentId++).ToString();
                }

            }
            var MedicareCostReportECId = 0;
            if (upMedicareCostReportEC.PostedFile != null && !string.IsNullOrEmpty(upMedicareCostReportEC.PostedFile.FileName) && upMedicareCostReportEC.PostedFile.ContentLength > 0)
            {
                if (postback)
                {
                    MedicareCostReportECId = UploadDocument(this.upMedicareCostReportEC, this.LblMedicareCostReportEC);
                }
                else
                {
                    txtMedicareCostReportECId.Text = (attachmentId++).ToString();
                }

            }
            var MedicaidHospitalCostReportId = 0;
            if (upMedicaidHospitalCostReport.PostedFile != null && !string.IsNullOrEmpty(upMedicaidHospitalCostReport.PostedFile.FileName) && upMedicaidHospitalCostReport.PostedFile.ContentLength > 0)
            {
                if (postback)
                {
                    MedicaidHospitalCostReportId = UploadDocument(this.upMedicaidHospitalCostReport, this.lblMedicaidHospitalCostReport);
                }
                else
                {
                    txtMedicaidHospitalCostReportId.Text = (attachmentId++).ToString();
                }

            }
            var MedicareCostReportPIId = 0;
            if (upMedicareCostReportPI.PostedFile != null && !string.IsNullOrEmpty(upMedicareCostReportPI.PostedFile.FileName) && upMedicareCostReportPI.PostedFile.ContentLength > 0)
            {
                if (postback)
                {
                    MedicareCostReportPIId = UploadDocument(this.upMedicareCostReportPI, this.LblMedicareCostReportPI);
                }
                else
                {
                    txtMedicareCostReportPIId.Text = (attachmentId++).ToString();
                }

            }


            if (!postback)
            {
                postBack.Value = true.ToString();
                return;
            }
            if (OtherDOCId == 0 && MedicareCostReportECId == 0 && MedicaidHospitalCostReportId == 0 && MedicareCostReportPIId == 0 && OtherDocCounter == 0)
            {
                return;
            }


            for (int i = 1; i < OtherDocCounter; i++)
            {
                var dynamicFile = (EncryptedFileUpload)dynamicCtrl.FindControl("upOtherDoc_" + i);
                if (dynamicFile.PostedFile != null && !string.IsNullOrEmpty(dynamicFile.PostedFile.FileName) && dynamicFile.PostedFile.ContentLength > 0)
                {
                    var dynamicOtherDocumentId = UploadDocument(dynamicFile, this.LblOtherDOC, "");
                    // Insert other document attchement xref
                    InsertDocuementAttachementXref(CON.DocumentType.OtherDOCId, dynamicOtherDocumentId);
                }
            }

            // Insert Document attachement xref
            InsertDocuementAttachementXref(CON.DocumentType.MedicaidHospitalCostReportId, MedicaidHospitalCostReportId);
            InsertDocuementAttachementXref(CON.DocumentType.MedicareCostReportECId, MedicareCostReportECId);
            InsertDocuementAttachementXref(CON.DocumentType.MedicareCostReportPIId, MedicareCostReportPIId);
            InsertDocuementAttachementXref(CON.DocumentType.OtherDOCId, OtherDOCId);

            //Insert into cost report table

            var parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.RegIdTxt.Value.ToString());
            parms.Add("FACILITY_PROGRAM_TYPE_ID", dlFacProgType.SelectedValue);
            parms.Add("SETTELEMENT_TYPE_ID", dlSettelementType.SelectedValue);
            parms.Add("SETTELEMENT_AMOUNT", txtSettelmentAmt.Text);
            parms.Add("TRACKING_ID", txtTrackingId.Text);
            parms.Add("OPERATION", "INSERT");
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            int result1 = svc.InsertRegistrationData(Convert.ToInt32(this.RegIdTxt.Value), "HospitalCostReport", parms);

        }
    }
    private int GetDocumentType(string documentType)
    {

        int retVal = 0;
        DataSet dsType = new DataSet();
        try
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_CODE", DbType.String, documentType, false));
            dsType = DataAccess.ExecuteStoredProcedure("usp_SelectDOCUMENT_TYPE_ByCode", parameters, "DocumentTypes");
            if (dsType != null && dsType.Tables[0] != null && dsType.Tables[0].Rows.Count > 0)
                retVal = Convert.ToInt32(dsType.Tables[0].Rows[0]["DOCUMENT_TYPE_ID"].ToString());
        }
        catch (Exception ex)
        {
            throw ex;


        }

        return retVal;
    }

    private void InsertDocuementAttachementXref(string doctype, int docId)
    {
        var doctypeId = GetDocumentType(doctype);
        if (doctypeId > 0 && docId > 0)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, docId, false));
            parameters.Add(SqlParms.CreateParameter("NOTES", DbType.String, string.Empty, false));
            parameters.Add(SqlParms.CreateParameter("DOCUMENT_RECEIVED_DATE", DbType.DateTime, DateTime.Now, false));
            parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_ID", DbType.Int32, doctypeId, false));
            parameters.Add(SqlParms.CreateParameter("CREATED_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
            parameters.Add(SqlParms.CreateParameter("CREATED_BY_MODIFIED_USER", DbType.String, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), false));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), false));
            DataAccess.ExecuteScalar("insertDOCUMENT_ATTACHMENT_XREF", parameters);
        }
       
    }

    private void SetUserInfo()
    {
        var userId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        DataSet dataSet = svc.SelectUserNameByUserID(userId);
        DataTable dtMisc = Helper.HasRows(dataSet) ? dataSet.Tables[0] : null;
        if (Helper.HasRows(dtMisc))
        {
            DataRow dr = dtMisc.Rows[0];
            txtSubUserId.Text = HttpContext.Current.User.Identity.Name;
            txtSubUserId.Enabled = false;
            txtSubName.Text = Helper.GetString("CONTACT_NAME", dr);
            txtSubName.Enabled = false;
        }
    }



    private bool Validate()
    {
        return true;
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
    private void AddError(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valHospitalCostReport";
        this.Page.Validators.Add(val);
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
    public override bool ValidateData()
    {
        return true;
    }
    public override string ValidationGroup
    {
        get { return "valHospitalCostReport"; }
    }
    private bool IsSigned()
    {
        return Membership.ValidateUser(HttpContext.Current.User.Identity.Name, txtPassword.Text);
    }
    protected void btnSaveSignature_Click(object sender, EventArgs e)
    {
        if (IsCaptchaValid() && HasPassword() && IsSigned())
        {
            var parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.RegIdTxt.Value.ToString());
            parms.Add("SUBMISSION_STATUS", CON.CostReportSubmissionStatus.Approved.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("OPERATION", "UPDATE");
            int result1 = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "MSPCostReport", parms);
        }
    }
    protected void bntCancelSig_Click(object sender, EventArgs e)
    {
        cpSubmitterApproval.Collapsed = true;
        docValidatedLabel.Visible = false;
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

    protected void btnCancelCostReport_Click(object sender, EventArgs e)
    {
        HideSubmitPanel();
        btnCancelCostReport.Attributes.Add("AutoPostback", "true");
    }
}