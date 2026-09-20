using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Pages_SubstituteW4Form : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    
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

    #region dtProvider
    DataTable _dtProvider;

    public DataTable dtProvider
    {
        get
        {
            if (_dtProvider == null)
            {
                DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
                _dtProvider = Helper.HasRows(ds) ? ds.Tables[0] : null;
            }

            return _dtProvider;
        }
    }
    #endregion

    public System.EventHandler InvalidateAgreements;

    public void LoadData()
    {
        if (!Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName))
        {
            if (!Registration.PageIsValidated(this.WorkflowPage.RegistrationId, MAXIMUS.Core.Libraries.Constants.RegistrationPageType.Identification))
                MessageBox2.Show("The Identification page is a prerequisite to this page. Please fill out the Identification page.", "Prerequiste");
        }

        

        
        DataSet ds = svc.GetMaritalStatus();
        
        foreach (DataRow row in ds.Tables["MaritalStatus"].Rows)
        {
            ListItem item = new ListItem(row["Marital_Status_Name"].ToString(), row["Marital_Status_ID"].ToString());
            ddlMaritalStatus.Items.Add(item);
        }
        ddlMaritalStatus.Items.Insert(0, new ListItem(string.Empty, string.Empty));
        LoadProvider();
    }

    public bool SaveData()
    {

        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
        if (!Helper.HasRows(ds)) return true;

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("REG_SERVICE_LOCATION_ID", ds.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"].ToString());
        if (ddlMaritalStatus.SelectedValue != null && ddlMaritalStatus.SelectedValue != "")
        {
            parms.Add("Marital_Status_ID", ddlMaritalStatus.SelectedValue);
        }
        else
        {
            parms.Add("Marital_Status_ID", null);
        }
        if (chkDifferentLastName.Checked)
        {
            parms.Add("last_Name_CHANGED", "Y");
        }
        else
        {
            parms.Add("last_Name_CHANGED", "N");
        }
        if(txtNoofAllowances.Text != null && txtNoofAllowances.Text != "")
        {
            parms.Add("CLAIM_NOOF_ALLOWANCES", txtNoofAllowances.Text);
        }
        if (txtAdditionalAmount.Text != null && txtAdditionalAmount.Text != "" && Helper.StripNonNumerics(txtAdditionalAmount.Text) != "" && Helper.StripNonNumerics(txtAdditionalAmount.Text) != null)
        {
            decimal Amount;
            
            string amt = System.Text.RegularExpressions.Regex.Replace(txtAdditionalAmount.Text, "[^0-9.]", string.Empty);
            bool IsDecimal = decimal.TryParse(amt, out Amount);
            parms.Add("ADDITIONAL_AMOUNT", Amount.ToString());
        }
        if (chkExempt.Checked)
        {
            parms.Add("ISEXEMPT", "Y");
        }
        else
        {
            parms.Add("ISEXEMPT","N");
        }
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION", parms);

        if (InvalidateAgreements != null) InvalidateAgreements(this, new EventArgs());

        return true;
    }

    public bool ValidateData()
    {
        bool isGood = true;

        if (!Registration.PageIsValidated(this.WorkflowPage.RegistrationId, MAXIMUS.Core.Libraries.Constants.RegistrationPageType.Identification))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "*The Identification page is a prerequisite to this page. Please fill out the Identification page.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }

        if (Registration.PreviewingRegistrationSection())
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "*Previous registration sections must be completed first.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            return false;
        }

        if (string.IsNullOrEmpty(ddlMaritalStatus.SelectedValue))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "*Marital Status is required.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }

        //Bug 5486:  removed reqt for upload
        //if (!CheckUploadedDocuments())
        //{
        //    CustomValidator val = new CustomValidator();
        //    val.IsValid = false;
        //    val.ErrorMessage = "*Please upload a finished W-4 document.";
        //    val.ValidationGroup = "valProviderInfoHeader";
        //    this.Page.Validators.Add(val);
        //    isGood = false;
        //}

        return isGood;
    }

    private bool CheckUploadedDocuments()
    {
        // Check if there are documents uploaded per license
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        // If the page is not visible no reason to check further
        if (!Registration.PageIsVisible(this.WorkflowPage.RegistrationId, Registration.GetStepText(CON.RegistrationPageType.SubstituteW4Form), this.WorkflowPage.IsWaiverServiceProvider)) return true;

        DataSet dsDoc = psc.SelectRegDocuments(this.WorkflowPage.RegistrationId, CON.RegistrationPageType.SubstituteW4Form, string.Empty,
            CON.RegistrationPageType.Certification.ToString(), null);
        bool isGood = true;
        if (!Helper.HasRows(dsDoc)) isGood = false;

        return isGood;
    }


    #region Load
    private void LoadProvider()
    {
        if (!Helper.HasRows(dtProvider))
            return;

        bool editable = Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName); // Provider, admin and operator can edit. // labels are enabled because we will set text to the label

        RS01.Enabled =
        RS02.Enabled =

        ddlMaritalStatus.Enabled = editable;

        RS01.Text = dtProvider.Rows[0]["NAME"].ToString();
        RS02.Text = dtProvider.Rows[0]["DBA"].ToString();
       
        RS03.Text = dtProvider.Rows[0]["TAX_ID"].ToString();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
        if (Helper.HasRows(ds))
        {
            if (ds.Tables[0].Rows[0]["Marital_Status_ID"] != null &&
                !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Marital_Status_ID"].ToString()))
                ddlMaritalStatus.SelectedValue = ds.Tables[0].Rows[0]["Marital_Status_ID"].ToString();
            if (ds.Tables[0].Rows[0]["last_Name_CHANGED"] != null &&
                !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["last_Name_CHANGED"].ToString()))
            {
                if (ds.Tables[0].Rows[0]["last_Name_CHANGED"].ToString()== "Y")
                    chkDifferentLastName.Checked = true;
                else
                    chkDifferentLastName.Checked = false;
            }
            if (ds.Tables[0].Rows[0]["CLAIM_NOOF_ALLOWANCES"] != null &&
                !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CLAIM_NOOF_ALLOWANCES"].ToString()))
                txtNoofAllowances.Text = ds.Tables[0].Rows[0]["CLAIM_NOOF_ALLOWANCES"].ToString();
            if (ds.Tables[0].Rows[0]["ADDITIONAL_AMOUNT"] != null &&
                !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ADDITIONAL_AMOUNT"].ToString()))
                txtAdditionalAmount.Text = ds.Tables[0].Rows[0]["ADDITIONAL_AMOUNT"].ToString();
            if (ds.Tables[0].Rows[0]["ISEXEMPT"] != null &&
                !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ISEXEMPT"].ToString()))
                 {
                    if (ds.Tables[0].Rows[0]["ISEXEMPT"].ToString()== "Y")
                        chkExempt.Checked =true;
                    else
                        chkExempt.Checked = false;
                 }
        }
        chkDifferentLastName.Enabled = chkExempt.Enabled = txtAdditionalAmount.Enabled = txtNoofAllowances.Enabled = editable;
        
    }
    #endregion

}