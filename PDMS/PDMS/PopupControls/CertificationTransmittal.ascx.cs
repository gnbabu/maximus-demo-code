using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_CertificationTransmittal : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    private void LoadDropDowns()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet ds = null;

        if (ddlAccreditationStatus.Items.Count == 0)
        {
            ds = psc.SelectRegistrationDataWithParams("usp_SelectCERTIFICATION_ACCREDITATION_STATUS_TYPE", parms);
            Helper.LoadList(ddlAccreditationStatus, ds.Tables[0], "NAME", "CERTIFICATION_ACCREDITATION_STATUS_TYPE_ID", true);
        }
        if (ddlTypeOfAction.Items.Count == 0)
        {
            ds = psc.SelectRegistrationDataWithParams("usp_SelectCERTIFICATION_ACTION_TYPE", parms);
            Helper.LoadList(ddlTypeOfAction, ds.Tables[0], "NAME", "CERTIFICATION_ACTION_TYPE_ID", true);
        }
        if (ddlEligibility.Items.Count == 0)
        {
            ds = psc.SelectRegistrationDataWithParams("usp_SelectCERTIFICATION_ELIGIBILITY_TYPE", parms);
            Helper.LoadList(ddlEligibility, ds.Tables[0], "NAME", "CERTIFICATION_ELIGIBILITY_TYPE_ID", true);
        }
        if (ddlLTCBedBreakdown.Items.Count == 0)
        {
            ds = psc.SelectRegistrationDataWithParams("usp_SelectCERTIFICATION_LTC_BED_BREAKDOWN_TYPE", parms);
            Helper.LoadList(ddlLTCBedBreakdown, ds.Tables[0], "NAME", "CERTIFICATION_LTC_BED_BREAKDOWN_TYPE_ID", true);
        }
    }

    public override void LoadData(DataRow dr)
    {
        hdnCertificationId.Value = txtCertEffDate.Text = txtCertEndDate.Text = txtComments.Text = txtOwnershipChangeDate.Text = txtSurveyDate.Text = string.Empty;
        nbTotalCertBeds.Text = nbTotalFacBeds.Text = string.Empty;
        LoadDropDowns();
        ddlAccreditationStatus.SelectedIndex = ddlEligibility.SelectedIndex = ddlLTCBedBreakdown.SelectedIndex = ddlTypeOfAction.SelectedIndex = -1;

        if (dr != null)
        {
            hdnCertificationId.Value = Helper.GetString("REG_CERTIFICATION_ID", dr);
            txtCertEffDate.Text = Helper.GetDate("CERTIFICATION_EFFECTIVE_DATE", dr);
            txtCertEndDate.Text = Helper.GetDate("CERTIFICATION_END_DATE", dr);
            txtComments.Text = Helper.GetString("COMMENTS", dr);
            txtOwnershipChangeDate.Text = Helper.GetDate("OWNERSHIP_CHANGE_DATE", dr);
            txtSurveyDate.Text = Helper.GetDate("SURVEY_DATE", dr);
            nbTotalCertBeds.Text = Helper.GetString("TOTAL_CERTIFIED_BEDS", dr);
            nbTotalFacBeds.Text = Helper.GetString("TOTAL_FACILITY_BEDS", dr);
            ddlAccreditationStatus.SelectedValue = Helper.GetString("CERTIFICATION_ACCREDITATION_STATUS_TYPE_ID", dr);
            ddlEligibility.SelectedValue = Helper.GetString("CERTIFICATION_ELIGIBILITY_TYPE_ID", dr);
            ddlLTCBedBreakdown.SelectedValue = Helper.GetString("CERTIFICATION_LTC_BED_BREAKDOWN_TYPE_ID", dr);
            ddlTypeOfAction.SelectedValue = Helper.GetString("CERTIFICATION_ACTION_TYPE_ID", dr);
        }
    }

    public void SaveData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();

        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        if (!string.IsNullOrEmpty(ddlTypeOfAction.SelectedValue)) parms.Add("CERTIFICATION_ACTION_TYPE_ID", ddlTypeOfAction.SelectedValue);
        if (!string.IsNullOrEmpty(txtSurveyDate.Text)) parms.Add("SURVEY_DATE", txtSurveyDate.Text);
        if (!string.IsNullOrEmpty(txtCertEffDate.Text)) parms.Add("CERTIFICATION_EFFECTIVE_DATE", txtCertEffDate.Text);
        if (!string.IsNullOrEmpty(txtCertEndDate.Text)) parms.Add("CERTIFICATION_END_DATE", txtCertEndDate.Text);
        if (!string.IsNullOrEmpty(ddlEligibility.SelectedValue)) parms.Add("CERTIFICATION_ELIGIBILITY_TYPE_ID", ddlEligibility.SelectedValue);
        if (!string.IsNullOrEmpty(txtOwnershipChangeDate.Text)) parms.Add("OWNERSHIP_CHANGE_DATE", txtOwnershipChangeDate.Text);
        if (!string.IsNullOrEmpty(ddlAccreditationStatus.SelectedValue)) parms.Add("CERTIFICATION_ACCREDITATION_STATUS_TYPE_ID", ddlAccreditationStatus.SelectedValue);
        if (!string.IsNullOrEmpty(nbTotalFacBeds.Text)) parms.Add("TOTAL_FACILITY_BEDS", nbTotalFacBeds.Text);
        if (!string.IsNullOrEmpty(nbTotalCertBeds.Text)) parms.Add("TOTAL_CERTIFIED_BEDS", nbTotalCertBeds.Text);
        if (!string.IsNullOrEmpty(ddlLTCBedBreakdown.SelectedValue)) parms.Add("CERTIFICATION_LTC_BED_BREAKDOWN_TYPE_ID", ddlLTCBedBreakdown.SelectedValue);
        if (!string.IsNullOrEmpty(txtComments.Text)) parms.Add("COMMENTS", txtComments.Text);
        parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        if (!string.IsNullOrEmpty(hdnCertificationId.Value))
        {
            // Update
            parms.Add("REG_CERTIFICATION_ID", hdnCertificationId.Value);
            psc.UpdateRegistrationDataTable("CERTIFICATION", parms);
        }
        else
        {
            // Insert
            psc.InsertRegistrationDataTable("CERTIFICATION", parms);
        }
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valCertTrans";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    public bool ValidateData()
    {
        if (ddlTypeOfAction.SelectedItem.Text == "Initial")
        {
            bool isGood = true;
            if (string.IsNullOrEmpty(ddlEligibility.SelectedValue)) AddError("* Please select Eligibility", ref isGood);
            if (string.IsNullOrEmpty(txtCertEffDate.Text)) AddError("* Please select a Certification Effective Date", ref isGood);
            if (string.IsNullOrEmpty(txtCertEndDate.Text)) AddError("* Please select a Certification End Date", ref isGood);
            return isGood;
        }
        return true;
    }
}