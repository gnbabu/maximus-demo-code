using System;
using System.Data;
using System.Web.UI;
using Claims = Models.Data.Claims;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_AssistantSurgeon : BasePopupControl
{
    #region SVC
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

    #region Properties
    public Boolean SetReadOnlyFields
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value.ToString()))
                SetReadOnlyFieldsControl(value);
        }
    }
    public string ClaimId
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnAssistantSurgeonClaimID.Value))
                return hdnAssistantSurgeonClaimID.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnAssistantSurgeonClaimID.Value = value.Trim();
        }
    }

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnAssistantSurgeonClaimType.Value))
                return hdnAssistantSurgeonClaimType.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnAssistantSurgeonClaimType.Value = value.Trim();
        }
    }

    public string AssistantSurgeonNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtAssistantSurgeonNPI.Text))
                return txtAssistantSurgeonNPI.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtAssistantSurgeonNPI.Text = value;
        }
    }

    //assitant
    private string renprovnpi = string.Empty;
    public string RenderingProvNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(renprovnpi))
                return renprovnpi;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                renprovnpi = value;
        }
    }
    private string renderingprovMedicaid = string.Empty;
    public string RenderingProvider_MedicaidID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(renderingprovMedicaid))
                return renderingprovMedicaid;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                renderingprovMedicaid = value;
        }
    }
    private string supervisingprovnpi = string.Empty;
    public string SupervisingProviderNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(supervisingprovnpi))
                return supervisingprovnpi;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                supervisingprovnpi = value;
        }
    }
    private string supervisingprovMedicaid = string.Empty;
    public string SupervisingProvider_Medicaid
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(supervisingprovMedicaid))
                return supervisingprovMedicaid;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                supervisingprovMedicaid = value;
        }
    }
    public string Assistant_MedicaidID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblAssistantSurgeonMedicaidID.Text))
                return lblAssistantSurgeonMedicaidID.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (string.IsNullOrEmpty(AssistantSurgeonNPI))
                {
                    lblAssistantSurgeonMedicaidID.Text = value;
                }
                hdnAssistSurMedId.Value = value;
            }
        }
    }

    public string Assistant_FirstName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblAssistantSurgeonFirstName.Text))
                return lblAssistantSurgeonFirstName.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblAssistantSurgeonFirstName.Text = value;
        }
    }

    public string Assistant_LastName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblAssistantSurgeonLastName.Text))
                return lblAssistantSurgeonLastName.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblAssistantSurgeonLastName.Text = value;
        }
    }
    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(txtAssistantSurgeonNPI.Text))
        {
            rtn = true;
        }

        return rtn;
    }

    //assistant
    public bool ValidateNPI(string TextAssistantSurgeonNPI = "")
    {
        string NPICheckRen = RenderingProvNPI;
        string NPICheckSuper = SupervisingProviderNPI;
        string MedicaidRen = RenderingProvider_MedicaidID;
        string MedicaidSuper = SupervisingProvider_Medicaid;


        bool result = true;
        lblAssistantSurgeonError.Text = string.Empty;

        if ((!string.IsNullOrEmpty(TextAssistantSurgeonNPI.Trim()) && TextAssistantSurgeonNPI.Trim() == NPICheckRen))
        {
            if (hdnAssistantSurgeonClaimType.Value == CON.ClaimsType.Dental || hdnAssistantSurgeonClaimType.Value == CON.ClaimsType.Professional)
            {
                lblAssistantSurgeonError.Text = "Assistant Surgeon cannot be same as Rendering Provider";
                ClearFields();
                result = false;
            }
        }
        if (!string.IsNullOrEmpty(NPICheckRen.Trim()))
        {
            lblAssistantSurgeonError.Text = "Rendering and Assistant surgeon provider both cannot be entered in the same claim";
            ClearFields();
            result = false;
        }
        if ((!string.IsNullOrEmpty(TextAssistantSurgeonNPI.Trim()) && TextAssistantSurgeonNPI.Trim() == NPICheckSuper))
        {
            if (hdnAssistantSurgeonClaimType.Value == CON.ClaimsType.Dental || hdnAssistantSurgeonClaimType.Value == CON.ClaimsType.Professional)
            {
                lblAssistantSurgeonError.Text = "Assistant Surgeon cannot be same as Supervising Provider";
            }
            if (hdnAssistantSurgeonClaimType.Value == CON.ClaimsType.Institutional)
            {
                lblAssistantSurgeonError.Text = "Other Operating Physician cannot be same as Operating Physician Provider";
            }
            ClearFields();
            result = false;
        }

        return result;
    }



    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            GetAssistantSurgeonProviderDetails();
        }
        lblAssistantSurgeonError.Text = hdnErrorMessageAssistant.Value;
        if (!string.IsNullOrEmpty(lblAssistantSurgeonError.Text))
        {
            lblAssistantSurgeonMedicaidID.Text = string.Empty;
            lblAssistantSurgeonFirstName.Text = string.Empty;
            lblAssistantSurgeonLastName.Text = string.Empty;
        }
        lblAssistantSurgeonError.Text = hdnErrorMessageAssistant.Value = string.Empty;
        if (txtAssistantSurgeonNPI.Text.Length < 10 && !string.IsNullOrEmpty(txtAssistantSurgeonNPI.Text))
        {
            ClearFields();
        }
        else if (!string.IsNullOrEmpty(hdnAssistSurMedId.Value.ToString()))
        {
            DataSet dsProviderInformation = Claims.LoadProviderInformation(hdnAssistSurMedId.Value.ToString());
            string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

            if (entityTypeId != "1")
            {
                if (hdnAssistantSurgeonClaimType.Value == CON.ClaimsType.Dental || hdnAssistantSurgeonClaimType.Value == CON.ClaimsType.Professional)
                {
                    lblAssistantSurgeonError.Text = "Non - individual provider cannot be entered as assistant surgeon";
                }
                if (hdnAssistantSurgeonClaimType.Value == CON.ClaimsType.Institutional)
                {
                    lblAssistantSurgeonError.Text = "Non - individual provider cannot be entered as other operative physician";
                }

                ClearFields();
                return;
            }
        }

        if (!string.IsNullOrEmpty(hdnAssistSurMedId.Value.ToString()))
        {
            if (string.IsNullOrEmpty(txtAssistantSurgeonNPI.Text))
            {
                lblAssistantSurgeonMedicaidID.Text = hdnAssistSurMedId.Value.ToString();
            }
        }
        if (!string.IsNullOrEmpty(hdnAssistFirstName.Value.ToString()))
        {
            lblAssistantSurgeonFirstName.Text = hdnAssistFirstName.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnAssistLastName.Value.ToString()))
        {
            lblAssistantSurgeonLastName.Text = hdnAssistLastName.Value.ToString();
        }
    }

    protected void txtAssistantSurgeonNPI_TextChanged(object sender, EventArgs e)
    {
        if (ValidateNPI(txtAssistantSurgeonNPI.Text))
        {
            lblAssistantSurgeonError.Text = string.Empty;
           // Claims.NPITextChanged(txtAssistantSurgeonNPI.Text, lblAssistantSurgeonMedicaidID, null, lblAssistantSurgeonLastName, lblAssistantSurgeonFirstName, null, lblAssistantSurgeonError);
            
            //if (!string.IsNullOrEmpty(lblAssistantSurgeonError.Text))
            //{
            //    lblAssistantSurgeonError.Text = "10-digit number is required";
            //    ClearFields();
            //}            
            if (!string.IsNullOrWhiteSpace(txtAssistantSurgeonNPI.Text))
            {
                DataTable dt = Claims.GetData(txtAssistantSurgeonNPI.Text.Trim(), "", "", "");
                if (Helper.HasRows(dt))
                {
                    if (dt.Rows.Count >= 2)
                    {

                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { $('#myModalpop').modal('show')});</script>", false);

                    }
                    else
                    {
                    Claims.NPITextChanged(txtAssistantSurgeonNPI.Text, lblAssistantSurgeonMedicaidID, null, lblAssistantSurgeonLastName, lblAssistantSurgeonFirstName, null, lblAssistantSurgeonError);
                        if (!string.IsNullOrEmpty(hdnAssistSurMedId.Value.ToString()))
                        {
                            DataSet dsProviderInformation = Claims.LoadProviderInformation(hdnAssistSurMedId.Value.ToString());
                            string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

                            if (entityTypeId != "1")
                            {
                                if (hdnAssistantSurgeonClaimType.Value == CON.ClaimsType.Dental || hdnAssistantSurgeonClaimType.Value == CON.ClaimsType.Professional)
                                {
                                    lblAssistantSurgeonError.Text = "Non - individual provider cannot be entered as assistant surgeon";
                                }
                                if (hdnAssistantSurgeonClaimType.Value == CON.ClaimsType.Institutional)
                                {
                                    lblAssistantSurgeonError.Text = "Non - individual provider cannot be entered as other operative physician";
                                }

                                ClearFields();
                                return;
                            }
                        }
                    }
                }
                else
                {
                    lblAssistantSurgeonError.Text =!string.IsNullOrWhiteSpace(txtAssistantSurgeonNPI.Text)? "NPI is Unknown":"";
                    ClearFields();
                }
            }
            else if (!string.IsNullOrEmpty(txtAssistantSurgeonNPI.Text) && string.IsNullOrEmpty(lblAssistantSurgeonMedicaidID.Text))
            {
                lblAssistantSurgeonError.Text = !string.IsNullOrEmpty(txtAssistantSurgeonNPI.Text) ? "NPI is Unknown":"";
                ClearFields();

            }
            if (!string.IsNullOrEmpty(lblAssistantSurgeonError.Text))
            {
                ClearFields();
            }
            
           
        }
    }
    private void AssignValidationSummary(string validationSummary)
    {
        rfvAssistantSurgeonNPI.ValidationGroup = validationSummary;
        revRefNPI.ValidationGroup = validationSummary;
    }

    public void SaveAssistantSurgeon(string validationSummary, int actionButton)
    {
        if (actionButton == CON.ActionButtonType.Submit)
        {
            AssignValidationSummary(validationSummary);
            Page.Validate(validationSummary);
            if (Page.IsValid)
            {
                Claims.SaveClaimsNPIDetails(txtAssistantSurgeonNPI.Text, hdnAssistSurMedId.Value, lblAssistantSurgeonFirstName.Text, lblAssistantSurgeonLastName.Text, "Claims_Provider_Information", 0, CON.ClaimsPanelNames.AssistantSurgeonProvider, Convert.ToInt32(hdnAssistantSurgeonClaimID.Value));
                GetAssistantSurgeonProviderDetails();
            }
        }
        else
        {
            Claims.SaveClaimsNPIDetails(txtAssistantSurgeonNPI.Text, hdnAssistSurMedId.Value, lblAssistantSurgeonFirstName.Text, lblAssistantSurgeonLastName.Text, "Claims_Provider_Information", 0, CON.ClaimsPanelNames.AssistantSurgeonProvider, Convert.ToInt32(hdnAssistantSurgeonClaimID.Value));
            GetAssistantSurgeonProviderDetails();
        }

    }

    public void SaveOtherOperatingPhysicianInfo(string validationSummary, int actionButton)
    {
        if (actionButton == CON.ActionButtonType.Submit)
        {
            AssignValidationSummary(validationSummary);
            Page.Validate(validationSummary);
            if (Page.IsValid)
            {
                Claims.SaveClaimsNPIDetails(txtAssistantSurgeonNPI.Text, hdnAssistSurMedId.Value, lblAssistantSurgeonFirstName.Text, lblAssistantSurgeonLastName.Text, "Claims_Provider_Information", 0, CON.ClaimsPanelNames.OtherOperatingPhysicianInformation, Convert.ToInt32(hdnAssistantSurgeonClaimID.Value));
                GetAssistantSurgeonProviderDetails();
            }
        }
        else
        {
            Claims.SaveClaimsNPIDetails(txtAssistantSurgeonNPI.Text, hdnAssistSurMedId.Value, lblAssistantSurgeonFirstName.Text, lblAssistantSurgeonLastName.Text, "Claims_Provider_Information", 0, CON.ClaimsPanelNames.OtherOperatingPhysicianInformation, Convert.ToInt32(hdnAssistantSurgeonClaimID.Value));
            GetAssistantSurgeonProviderDetails();
        }

    }
    public void GetAssistantSurgeonProviderDetails()
    {
        if (!string.IsNullOrEmpty(hdnAssistantSurgeonClaimID.Value))
        {
            DataSet dsAssistant = svc.SelectDentalClaimData(Convert.ToInt32(hdnAssistantSurgeonClaimID.Value), "claims_provider_information", CON.ClaimsPanelNames.AssistantSurgeonProvider);

            if (Helper.HasRows(dsAssistant) && Convert.ToInt32(dsAssistant.Tables[0].Rows[0]["Is_Primary"]) == 0 &&
                Convert.ToInt32(dsAssistant.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnAssistantSurgeonClaimID.Value) &&
                dsAssistant.Tables[0].Rows[0]["Claims_Panel_Name"].ToString() == CON.ClaimsPanelNames.AssistantSurgeonProvider && hdnAssistantSurgeonClaimType.Value == CON.ClaimsType.Dental)
            {
                txtAssistantSurgeonNPI.Text = dsAssistant.Tables[0].Rows[0]["NPI"].ToString();
                if (string.IsNullOrEmpty(txtAssistantSurgeonNPI.Text))
                {
                    lblAssistantSurgeonMedicaidID.Text = dsAssistant.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                }
                hdnAssistSurMedId.Value = dsAssistant.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                lblAssistantSurgeonFirstName.Text = dsAssistant.Tables[0].Rows[0]["First_Name"].ToString();
                lblAssistantSurgeonLastName.Text = dsAssistant.Tables[0].Rows[0]["Last_Name"].ToString();
            }

            DataSet dsOOPhysician = svc.SelectDentalClaimData(Convert.ToInt32(hdnAssistantSurgeonClaimID.Value), "claims_provider_information", CON.ClaimsPanelNames.OtherOperatingPhysicianInformation);
            if (Helper.HasRows(dsOOPhysician) && Convert.ToInt32(dsOOPhysician.Tables[0].Rows[0]["Is_Primary"]) == 0 &&
               Convert.ToInt32(dsOOPhysician.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnAssistantSurgeonClaimID.Value) &&
               dsOOPhysician.Tables[0].Rows[0]["Claims_Panel_Name"].ToString() == CON.ClaimsPanelNames.OtherOperatingPhysicianInformation && hdnAssistantSurgeonClaimType.Value == CON.ClaimsType.Institutional)
            {
                txtAssistantSurgeonNPI.Text = dsOOPhysician.Tables[0].Rows[0]["NPI"].ToString();
                if (string.IsNullOrEmpty(txtAssistantSurgeonNPI.Text))
                {
                    lblAssistantSurgeonMedicaidID.Text = dsOOPhysician.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                }
                hdnAssistSurMedId.Value = dsOOPhysician.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                lblAssistantSurgeonFirstName.Text = dsOOPhysician.Tables[0].Rows[0]["First_Name"].ToString();
                lblAssistantSurgeonLastName.Text = dsOOPhysician.Tables[0].Rows[0]["Last_Name"].ToString();
            }
        }
    }

    private void SetReadOnlyFieldsControl(bool value)
    {
        txtAssistantSurgeonNPI.ReadOnly = value;
        SearchAss.Visible = !value;
        txtAssistantSurgeonNPI.Visible = true;
        LinkButton3.Visible = !value;

    }
    public void ClearFields()
    {
        txtAssistantSurgeonNPI.Text = string.Empty;
        lblAssistantSurgeonMedicaidID.Text = string.Empty;
        lblAssistantSurgeonFirstName.Text = string.Empty;
        lblAssistantSurgeonLastName.Text = string.Empty;
        hdnAssistSurMedId.Value= string.Empty; 
        hdnAssistFirstName.Value= string.Empty;
        hdnAssistLastName.Value= string.Empty;

    }
}