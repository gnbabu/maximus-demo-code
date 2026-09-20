using Corp.Core.Libraries;
using Corp.Core.Libraries.IncidentManagementService;
using CustomControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;


public partial class PopupControls_IncidentComplianceReview : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
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
    private DataSet dsCasenum { get; set; }
   // private DataSet dsCaseDtl { get; set; }
    private const string sectionName = "IncidentComplianceReview";
    private string providerName;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
        {
            pnlCaseSummary.Visible = false;
            pnlIncidentdtls.Visible = false;
            pnlIncidentnotes.Visible = false;
            pnlPlanofCorrection.Visible = true;
            pnlPOCByMail.Visible = true;
            btnSaveDoc.Visible = false;            
        }
        else
        {
            pnlPOCByMail.Visible = false;
        }
    }
    public override void LoadData(DataRow row)
    {

    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
       
        dsCasenum = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "INCIDENT_COMPLIANCE_CASE_XREF");
        if (Helper.HasRows(dsCasenum))
        {
            //isEdit = true;
            DataTable dtcaseNumbers = dsCasenum.Tables[0];
            grdCaseNumbers.DataSource = dtcaseNumbers;
            grdCaseNumbers.DataBind();
            
            providerName = dtcaseNumbers.Rows[0].GetString("PROVIDER_NAME");
            if (grdCaseNumbers.SelectedIndex < 0)
                grdCaseNumbers.SelectRowByDataKey<string>((string)dtcaseNumbers.Rows[0]["INCIDENT_CASE_NUMBER"]);

            if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
            {
                chkpocByMail.Checked = (Boolean)dtcaseNumbers.Rows[grdCaseNumbers.SelectedIndex]["POC_BY_MAIL_FLAG"];
            }

        }

        LoadPlaceHolderPOC(0, true, false, sectionName);
        
    }

    protected void grdCaseNumbers_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdCaseNumbers.SelectedIndex >= 0)
        {
            DataTable dtcaseNumbers = dsCasenum.Tables[0];
            string incidentCasenum = (string)dtcaseNumbers.Rows[grdCaseNumbers.SelectedIndex]["INCIDENT_CASE_NUMBER"];
            hdnCaseNumber.Value = incidentCasenum;
            lblRegCaseNum.Text = dtcaseNumbers.Rows[grdCaseNumbers.SelectedIndex]["REG_INCIDENT_COMPLIANCE_CASE_XREF_ID"].ToString();
            Helper.ShowActionsButtions = (int)this.grdCaseNumbers.DataKeys[grdCaseNumbers.SelectedIndex].Values["CASE_STATUS"] == CON.IncidentReviewCaseStatus.New ? true : false;

            btnSaveDoc.Visible = Helper.ShowActionsButtions;
            //txtDateOfPOC.Text = string.IsNullOrEmpty(dtcaseNumbers.Rows[grdCaseNumbers.SelectedIndex]["POC_RECEIVED_DATE"].ToString()) ? string.Empty : dtcaseNumbers.Rows[grdCaseNumbers.SelectedIndex]["POC_RECEIVED_DATE"].ToString();
            //txtDateRevPOC.Text = string.IsNullOrEmpty(dtcaseNumbers.Rows[grdCaseNumbers.SelectedIndex]["REVISED_POC_DATE"].ToString()) ? string.Empty : dtcaseNumbers.Rows[grdCaseNumbers.SelectedIndex]["REVISED_POC_DATE"].ToString();
            LoadIncidentCaseDetails(incidentCasenum);
        }
    }

    private void LoadIncidentCaseDetails(string incidentCaseNum)
    {
        DataSet dsCaseDtl = svc.SelectIncidentCaseDetails(incidentCaseNum);
        if (Helper.HasRows(dsCaseDtl))
        {
            DataTable dtCasedtl = dsCaseDtl.Tables[0];
            grdIncidentnumbers.DataSource = dtCasedtl;
            grdIncidentnumbers.DataBind();
        }
    }

    protected void grdIncidentnumbers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if(!Helper.ShowActionsButtions)
            {
                LinkButton lnkIssueNOD = (LinkButton)e.Row.FindControl("lnkIssueNOD");
                 lnkIssueNOD.Visible = false;
                RadioButtonList rbl = (RadioButtonList)e.Row.FindControl("rblnodNeeded");
                rbl.Enabled = false;
                btnSaveComments.Visible = false;
            }
        }
    }

    protected void grdCaseNumbers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SelectCaseNumber")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            DataTable dtcaseNumbers = dsCasenum.Tables[0];
            grdCaseNumbers.SelectRowByDataKey<string>((string)dtcaseNumbers.Rows[index]["INCIDENT_CASE_NUMBER"]);
            LoadPlaceHolderPOC(0, true, false, sectionName);
        }
    }
    protected void SelectionChanged(object sender, EventArgs e)
    {
        string incidentComplianceCaseDetailId = "";
        Control c = sender as Control;
        GridViewRow selectedRow = c.NamingContainer as GridViewRow; 
        {
            int selectRowIndex = selectedRow.RowIndex;
            incidentComplianceCaseDetailId = selectedRow.Cells[0].Text;
        }
        var response = (sender as RadioButtonList).SelectedValue;
        hdnNodNeeded.Value = response;
        if (hdnNodNeeded.Value == "True")
        {
            txtNODdate.Enabled = true;
            txtCSnotes.Enabled = true;
            btnSaveComments.Enabled = true;
            btnCancelComments.Enabled = true;
            txtnodneed.Text = "True";
        }
        else
        {
            txtNODdate.Enabled = false;
            txtCSnotes.Enabled = false;
            btnSaveComments.Enabled = false;
            btnCancelComments.Enabled = false;
            txtnodneed.Text = "False";
            UpdateRegIncidentComplianceCaseDetail(incidentComplianceCaseDetailId);
        }
    }

    protected void grdIncidentnumbers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "SelectIncident")
        {
            grdIncidentnumbers.SelectRowByDataKey<string>((string)this.grdIncidentnumbers.DataKeys[index].Values["INCIDENT_ID"]);
            hdnIncidentCaseDtlID.Text = string.IsNullOrEmpty(this.grdIncidentnumbers.DataKeys[index].Values["REG_INCIDENT_COMPLIANCE_CASE_DETAIL_ID"].ToString()) ? string.Empty : this.grdIncidentnumbers.DataKeys[index].Values["REG_INCIDENT_COMPLIANCE_CASE_DETAIL_ID"].ToString();
            txtnodneed.Text = string.IsNullOrEmpty(this.grdIncidentnumbers.DataKeys[index].Values["NOD_NEEDED"].ToString()) ? string.Empty : this.grdIncidentnumbers.DataKeys[index].Values["NOD_NEEDED"].ToString();
            txtIncidentID.Text = string.IsNullOrEmpty(this.grdIncidentnumbers.DataKeys[index].Values["INCIDENT_ID"].ToString()) ? string.Empty : this.grdIncidentnumbers.DataKeys[index].Values["INCIDENT_ID"].ToString();
            txtReasonNOD.Text = string.IsNullOrEmpty(this.grdIncidentnumbers.DataKeys[index].Values["NOD_REASON"].ToString()) ? string.Empty : this.grdIncidentnumbers.DataKeys[index].Values["NOD_REASON"].ToString();
            txtNODdate.Text = string.IsNullOrEmpty(this.grdIncidentnumbers.DataKeys[index].Values["NOD_ISSUED_DATE"].ToString()) ? string.Empty : this.grdIncidentnumbers.DataKeys[index].Values["NOD_ISSUED_DATE"].ToString();
            txtCSnotes.Text = string.IsNullOrEmpty(this.grdIncidentnumbers.DataKeys[index].Values["CS_NOTES"].ToString()) ? string.Empty : this.grdIncidentnumbers.DataKeys[index].Values["CS_NOTES"].ToString();
            //if (txtnodneed.Text == "True")
            //{
            //    txtNODdate.Enabled = true;
            //    txtCSnotes.Enabled = true;
            //    btnSaveComments.Enabled = true;
            //    btnCancelComments.Enabled = true;

            //}
            //else
            //{
            //    txtNODdate.Enabled = false;
            //    txtCSnotes.Enabled = false;
            //    btnSaveComments.Enabled = false;
            //    btnCancelComments.Enabled = false;
            //}

        }

        if(e.CommandName == "IssueNOD")
        {
            string response = string.Empty;
            string imsAssID = string.IsNullOrEmpty(this.grdIncidentnumbers.DataKeys[index].Values["IMS_ASSOCIATE_ID"].ToString()) ? string.Empty : this.grdIncidentnumbers.DataKeys[index].Values["IMS_ASSOCIATE_ID"].ToString();
            hdnIncidentCaseDtlID.Text = string.IsNullOrEmpty(this.grdIncidentnumbers.DataKeys[index].Values["REG_INCIDENT_COMPLIANCE_CASE_DETAIL_ID"].ToString()) ? string.Empty : this.grdIncidentnumbers.DataKeys[index].Values["REG_INCIDENT_COMPLIANCE_CASE_DETAIL_ID"].ToString();
            string NODdate = string.IsNullOrEmpty(this.grdIncidentnumbers.DataKeys[index].Values["NOD_ISSUED_DATE"].ToString()) ? string.Empty : this.grdIncidentnumbers.DataKeys[index].Values["NOD_ISSUED_DATE"].ToString();
            //if (Helper.GetAppSetting("Environment", string.Empty) == CON.Environment.E2E || Helper.GetAppSetting("Environment", string.Empty) == CON.Environment.INT02 || Helper.GetAppSetting("Environment", string.Empty) == CON.Environment.OH_UAT)
            //{
                UpdateProviderIncidentStatusInfo upReqInfo = new UpdateProviderIncidentStatusInfo();
                upReqInfo.MedicaidProviderID = this.WorkflowPage.MedicaidID;
                upReqInfo.ProviderName = providerName;
                upReqInfo.ProviderStatus = "Elevated Screening";
                upReqInfo.IMSAssociateID = imsAssID;
                upReqInfo.IMSCaseNumber = hdnCaseNumber.Value;
                upReqInfo.PNMNODID = hdnIncidentCaseDtlID.Text;
                upReqInfo.DateNODIssued = Convert.ToDateTime(NODdate).ToString("yyyy-MM-dd");
                upReqInfo.ActionByDepartment = "Yes";

                IncidentManagementReqRes ims = new IncidentManagementReqRes();
                response = ims.UpdateIncidentProviderStatus(this.WorkflowPage.MedicaidID, hdnCaseNumber.Value, this.WorkflowPage.RegistrationId, "IssueNOD", upReqInfo);

                if (response == "Fail")
                {
                    CustomValidator val = new CustomValidator();
                    val.IsValid = false;
                    val.ErrorMessage = "Failed to send NOD Update to IMS";
                    val.ValidationGroup = "valIncidentCompliance";
                    this.Page.Validators.Add(val);
                }
            //}
        }
    }

    public override bool ValidateData()
    {
        bool isValid = true;

        return isValid;
    }
    private void AddError(string msg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valIncidentCompliance";
        this.Page.Validators.Add(val);
        isGood = false;
    }
    
    public void LoadPlaceHolderPOC(int licensureId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderPOC.Controls.Clear();
        PlaceholderRevisedPOC.Controls.Clear();
        
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        ds = svc.SelecIncidentComplianceDocument(pageTypeID, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.ProviderTypeID, this.WorkflowPage.EntityTypeID, sectionName, this.WorkflowPage.RegistrationId, Convert.ToInt32(lblRegCaseNum.Text), hdnCaseNumber.Value);
        
        int table = ds.Tables.Count;
        int rowCount = 0;
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                rowCount++;
                UserControls_UploadSectionControl ucUploadSectionControl =
                    LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;

                ucUploadSectionControl.Title = Helper.GetString("TITLE", dr);

                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);

               // ucUploadSectionControl.DestinationPath = @"C:\project\temp";

                ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);
                if ((dr.Table.Columns.Contains("DOCUMENT_ID")))
                    ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);
                else
                    ucUploadSectionControl.DocumentId = 0;

                ucUploadSectionControl.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);

                if ((dr.Table.Columns.Contains("FILE_NAME")) && !string.IsNullOrEmpty(Helper.GetString("FILE_NAME", dr)))
                    ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                else
                    ucUploadSectionControl.FileName = null;

                ucUploadSectionControl.SectionName = sectionName;
                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";

                ucUploadSectionControl.RowId = Convert.ToInt32(lblRegCaseNum.Text);

                if (Helper.GetString("TITLE", dr) == "Plan Of Correction")
                {
                    PlaceholderPOC.Controls.Add(ucUploadSectionControl);
                    if (!string.IsNullOrEmpty(Helper.GetString("FILE_NAME", dr)))
                    {
                        DateTime dateOfPOC = Convert.ToDateTime(Helper.GetString("LAST_MODIFIED_DATE_TIME", dr));
                        txtDateOfPOC.Text = dateOfPOC.ToString("MM/dd/yyyy");
                    }
                    LinkButton removeButton = (LinkButton)ucUploadSectionControl.FindControl("LnkButtonDelete");
                    removeButton.OnClientClick = "javascript:document.getElementById('" + this.txtDateOfPOC.ClientID + "').value = ''; ";
                }

                if (Helper.GetString("TITLE", dr) == "Revised Plan of Correction")
                {
                    PlaceholderRevisedPOC.Controls.Add(ucUploadSectionControl);
                    if (!string.IsNullOrEmpty(Helper.GetString("FILE_NAME", dr)))
                    {
                        DateTime dateRevPoc = Convert.ToDateTime(Helper.GetString("LAST_MODIFIED_DATE_TIME", dr));
                        txtDateRevPOC.Text = dateRevPoc.ToString("MM/dd/yyyy");
                    }
                    LinkButton removeButton = (LinkButton)ucUploadSectionControl.FindControl("LnkButtonDelete");
                    removeButton.OnClientClick = "javascript:document.getElementById('" + this.txtDateRevPOC.ClientID + "').value = ''; ";
                }
            }
        }
    }

    protected void UpdateRegIncidentComplianceCaseDetail(string incident_Compliance_Case_Detail_Id) 
    {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms = new Dictionary<string, string>();
        if (hdnNodNeeded.Value == "False")
        {
            parms.Add("REG_INCIDENT_COMPLIANCE_CASE_DETAIL_ID", incident_Compliance_Case_Detail_Id);
            string nodNeed = hdnNodNeeded.Value;
            parms.Add("NOD_NEEDED", nodNeed);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "INCIDENT_COMPLIANCE_CASE_DETAIL", parms);

        }
        else
        {
            parms.Add("REG_INCIDENT_COMPLIANCE_CASE_DETAIL_ID", hdnIncidentCaseDtlID.Text);
            parms.Add("INCIDENT_CASE_NUMBER", hdnCaseNumber.Value);
            parms.Add("INCIDENT_ID", txtIncidentID.Text);
            string nodNeed = hdnNodNeeded.Value;
            parms.Add("NOD_NEEDED", nodNeed);
            parms.Add("NOD_ISSUED_DATE", txtNODdate.Text);
            parms.Add("CS_NOTES", txtCSnotes.Text);
            parms.Add("NOD_REASON", txtReasonNOD.Text);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "INCIDENT_COMPLIANCE_CASE_DETAIL", parms);
        }
           
    }

    protected void btnSaveIncident_Click(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(txtNODdate.Text))
        {
            lblError.Visible = true;
            lblError.Text = "Select a valid NOD date";
            return;
        }
        UpdateRegIncidentComplianceCaseDetail(string.Empty);
    }

    protected void btnCancelIncident_Click(object sender, EventArgs e)
    {

        txtNODdate.Text = "";
        txtCSnotes.Text = "";
    }

    public override string Title
    {
        get { return "Incident Compliance Review"; }
    }

    public override string IdText
    {
        get { return "ucIncidentComplianceReview_" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
        get { return "valIncidentCompliance"; }
    }

    //protected void btnIssueNOD_Click(object sender, EventArgs e)
    //{
    //    string response = string.Empty;
    //    if(Helper.GetAppSetting("Environment",string.Empty) == CON.Environment.E2E || Helper.GetAppSetting("Environment", string.Empty) == CON.Environment.INT02)
    //    {
    //        IncidentManagementReqRes ims = new IncidentManagementReqRes();
    //        response = ims.UpdateIncidentProviderStatus(this.WorkflowPage.MedicaidID,"Continue", providerName, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
    //            "IMSAssociateID", hdnCaseNumber.Value, "PNMNODID","NODISSUEDDATE", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
    //    }
    //}

    //protected void chkpocByMail_CheckedChanged(object sender, EventArgs e)
    //{
    //    svc.UpdatePOCMail(this.WorkflowPage.RegistrationId, chkpocByMail.Checked, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
    //}
    protected void btnSaveDoc_Click(object sender, EventArgs e)
    {
        bool isValid = true;
        foreach (Control ctrl in PlaceholderPOC.Controls)
        {
            UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
            uploadControl.IsRequired = true;

            isValid &= uploadControl.ValidateData("valIncidentCompliance");
        }

        if(isValid)
        {
            SaveDocuments();
            // send update to ims
        }
            
    }
    public override bool SaveData()
    {
        bool isValid = true;
        if (!chkpocByMail.Checked)
        {
            foreach (Control ctrl in PlaceholderPOC.Controls)
            {
                UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                uploadControl.IsRequired = true;

                isValid &= uploadControl.ValidateData("valIncidentCompliance");
            }
        }

        if (isValid)
            SaveDocuments();
        return isValid;
    }

    public void SaveDocuments()
    {
        bool ischkpocByMail = false;
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("REG_INCIDENT_COMPLIANCE_CASE_XREF_ID", lblRegCaseNum.Text);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());


        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        ds = svc.SelecIncidentComplianceDocument(pageTypeID, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.ProviderTypeID, this.WorkflowPage.EntityTypeID, sectionName, this.WorkflowPage.RegistrationId, Convert.ToInt32(lblRegCaseNum.Text), hdnCaseNumber.Value);

        if (Helper.HasRows(ds))
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if ((Helper.GetString("TITLE", dr) == "Plan Of Correction") && !string.IsNullOrEmpty(Helper.GetString("FILE_NAME", dr)))
                {
                    DateTime dateOfPOC = Convert.ToDateTime(Helper.GetString("LAST_MODIFIED_DATE_TIME", dr));
                    txtDateOfPOC.Text = dateOfPOC.ToString("MM/dd/yyyy");
                }

                if ((Helper.GetString("TITLE", dr) == "Revised Plan of Correction") && !string.IsNullOrEmpty(Helper.GetString("FILE_NAME", dr)))
                {
                    DateTime dateRevPoc = Convert.ToDateTime(Helper.GetString("LAST_MODIFIED_DATE_TIME", dr));
                    txtDateRevPOC.Text = dateRevPoc.ToString("MM/dd/yyyy");
                }
            }
        }

        parms.Add("POC_RECEIVED_DATE", txtDateOfPOC.Text);
        parms.Add("REVISED_POC_DATE", txtDateRevPOC.Text);
        if (!Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
        {
            parms.Add("POC_ACCEPTED_DATE", txtDateOfPOC.Text);
        }
        else
        {
            if(pnlPOCByMail.Visible && chkpocByMail.Checked)
            {
                parms.Add("POC_BY_MAIL_FLAG", chkpocByMail.Checked.ToString());
                ischkpocByMail = true;
            }
        }
        svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "INCIDENT_COMPLIANCE_CASE_XREF", parms);

        if (!ischkpocByMail)
            SendDocumentUpdatetoIMS();
    }
    public void SendDocumentUpdatetoIMS()
    {
        DataSet dsCaseDtl = svc.SelectIncidentCaseDetails(hdnCaseNumber.Value);
        if (Helper.HasRows(dsCaseDtl))
        {
            string response = string.Empty;
            string imsAssID = Helper.GetString("IMS_ASSOCIATE_ID", dsCaseDtl.Tables[1].Rows[0]);
            
            //if (Helper.GetAppSetting("Environment", string.Empty) == CON.Environment.E2E || Helper.GetAppSetting("Environment", string.Empty) == CON.Environment.INT02 || Helper.GetAppSetting("Environment", string.Empty) == CON.Environment.OH_UAT)
            //{
                UpdateProviderIncidentStatusInfo upReqInfo = new UpdateProviderIncidentStatusInfo();
                upReqInfo.MedicaidProviderID = this.WorkflowPage.MedicaidID;
                upReqInfo.ProviderName = providerName;
                upReqInfo.ProviderStatus = "Elevated Screening";
                //upReqInfo.IMSAssociateID = imsAssID;
                upReqInfo.IMSCaseNumber = hdnCaseNumber.Value;
                upReqInfo.PNMNODID = hdnIncidentCaseDtlID.Text;
                upReqInfo.ActionByDepartment = "No";
                if (!string.IsNullOrEmpty(Helper.GetString("POC_DUE_DATE", dsCaseDtl.Tables[1].Rows[0])))
                    upReqInfo.PlanOfCorrectionDueDate = Convert.ToDateTime(Helper.GetString("POC_DUE_DATE", dsCaseDtl.Tables[1].Rows[0])).ToString("yyyy-MM-dd");
                if(!string.IsNullOrEmpty(Helper.GetString("POC_RECEIVED_DATE", dsCaseDtl.Tables[1].Rows[0])))
                    upReqInfo.PlanOfCorrectionReceivedDate = Convert.ToDateTime(Helper.GetString("POC_RECEIVED_DATE", dsCaseDtl.Tables[1].Rows[0])).ToString("yyyy-MM-dd");
                if(!string.IsNullOrEmpty(Helper.GetString("POC_ACCEPTED_DATE", dsCaseDtl.Tables[1].Rows[0])))
                    upReqInfo.PlanOfCorrectionAcceptedDate = Convert.ToDateTime(Helper.GetString("POC_ACCEPTED_DATE", dsCaseDtl.Tables[1].Rows[0])).ToString("yyyy-MM-dd");
                if(!string.IsNullOrEmpty(Helper.GetString("REVISED_POC_DATE", dsCaseDtl.Tables[1].Rows[0])))
                    upReqInfo.RevisedPlanOfCorrectionDate = Convert.ToDateTime(Helper.GetString("REVISED_POC_DATE", dsCaseDtl.Tables[1].Rows[0])).ToString("yyyy-MM-dd");
                if(!string.IsNullOrEmpty(Helper.GetString("REVISED_POC_DUE_DATE", dsCaseDtl.Tables[1].Rows[0])))
                    upReqInfo.RevisedPlanOfCorrectionDueDate = Convert.ToDateTime(Helper.GetString("REVISED_POC_DUE_DATE", dsCaseDtl.Tables[1].Rows[0])).ToString("yyyy-MM-dd");

                IncidentManagementReqRes ims = new IncidentManagementReqRes();
                response = ims.UpdateIncidentProviderStatus(this.WorkflowPage.MedicaidID,hdnCaseNumber.Value, this.WorkflowPage.RegistrationId, "DocumentUpdate", upReqInfo);

                if (response == "Fail")
                {
                    CustomValidator val = new CustomValidator();
                    val.IsValid = false;
                    val.ErrorMessage = "Failed to send Document Update to IMS";
                    val.ValidationGroup = "valIncidentCompliance";
                    this.Page.Validators.Add(val);
                }
            //}
        }
    }
    protected void grdCaseNumbers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
    }
}