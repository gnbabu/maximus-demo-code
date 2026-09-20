using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claims = Models.Data.Claims;
using CON = MAXIMUS.Core.Libraries.Constants;


public partial class PopupControls_RenderingProviderInformationPanel : System.Web.UI.UserControl
{
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
            if (!string.IsNullOrWhiteSpace(hdnClaimIdRendering.Value))
                return hdnClaimIdRendering.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimIdRendering.Value = value.Trim();
        }
    }

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimType_Rendering.Value))
                return hdnClaimType_Rendering.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimType_Rendering.Value = value.Trim();
        }
    }
    public string RenderingProviderNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtRenderingProvNPI.Text))
                return txtRenderingProvNPI.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtRenderingProvNPI.Text = value;
        }
    }
    private string refprovnpi=string.Empty;
    public string ReferringProviderNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(refprovnpi))
                return refprovnpi;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                refprovnpi = value;
        }
    }
    private string refprovMedicaid = string.Empty;
    public string ReferringProvider_Medicaid
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(refprovMedicaid))
                return refprovMedicaid;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                refprovMedicaid = value;
        }
    }
    private string primaryprovnpi = string.Empty;
    public string PrimaryProviderNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(primaryprovnpi))
                return primaryprovnpi;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                primaryprovnpi = value;
        }
    }
    private string primaryprovMedicaid = string.Empty;
    public string PrimaryProvider_Medicaid
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(primaryprovMedicaid))
                return primaryprovMedicaid;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                primaryprovMedicaid = value;
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
    private string assistantprovnpi = string.Empty;
    public string AssistantProviderNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(assistantprovnpi))
                return assistantprovnpi;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                assistantprovnpi = value;
        }
    }
    private string assistantprovMedicaid = string.Empty;
    public string Assistant_MedicaidID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(assistantprovMedicaid))
                return assistantprovMedicaid;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                assistantprovMedicaid = value;
        }
    }
    public string BillingNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnBillingNPI.Value))
                return hdnBillingNPI.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnBillingNPI.Value = value.Trim();
        }
    }
    public string RenderingProvider_MedicaidID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblRenderingMedicaidID.Text))
            {
                return lblRenderingMedicaidID.Text;
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                lblRenderingMedicaidID.Text = value;
                hdnRendMedId.Value = value;
            }
        }
    }

    public string RenderingProvider_FirstName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblRenderingFirstName.Text))
                return lblRenderingFirstName.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblRenderingFirstName.Text = value;
        }
    }

    public string RenderingProvider_LastName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblRenderingLastName.Text))
                return lblRenderingLastName.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblRenderingLastName.Text = value;
        }
    }
    public string EmptyProperty
    {
        get
        {
            return string.Empty;
        }
        set
        {
            string test = string.Empty;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetRenderingProviderDetails();
        }
        lblRenderingErrorMessage.Text= hdnErrorMessageRender.Value;
        if(!string.IsNullOrEmpty(lblRenderingErrorMessage.Text) && txtRenderingProvNPI.Text.Length < 10 && !string.IsNullOrEmpty(txtRenderingProvNPI.Text))
        {
            lblRenderingMedicaidID.Text = string.Empty;
            lblRenderingLastName.Text = string.Empty;
            lblRenderingFirstName.Text = string.Empty;
        }
        lblRenderingErrorMessage.Text = hdnErrorMessageRender.Value = string.Empty;
        if (txtRenderingProvNPI.Text.Length < 10 && !string.IsNullOrEmpty(txtRenderingProvNPI.Text))
        {
            ClearFields();
        }
        else if (!string.IsNullOrEmpty(lblRenderingMedicaidID.Text.ToString()))
        {
            DataSet dsProviderInformation = Claims.LoadProviderInformation(hdnRendMedId.Value.ToString());
            if (Helper.HasRows(dsProviderInformation))
            {
                string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

                if (entityTypeId != "1" && hdnClaimType_Rendering.Value == CON.ClaimsType.Institutional)
                {
                    lblRenderingErrorMessage.Text = "Non-individual provider cannot be entered as rendering provider";
                    ClearFields();
                    return;
                }
            }
        }
        if (!string.IsNullOrEmpty(hdnRendMedId.Value.ToString()))
        {
            lblRenderingMedicaidID.Text = hdnRendMedId.Value.ToString();
        }

        if (!string.IsNullOrEmpty(hdnRendFirstName.Value.ToString()))
        {
            lblRenderingFirstName.Text = hdnRendFirstName.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnRendLastName.Value.ToString()))
        {
            lblRenderingLastName.Text = hdnRendLastName.Value.ToString();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
       
    }
    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(lblRenderingMedicaidID.Text))
        {
            rtn = true;
        }

        return rtn;
    }

    protected void GetRenderingProviderDetails()
    {
        if (!string.IsNullOrEmpty(hdnClaimIdRendering.Value))
        {
            DataSet dsRendering = svc.SelectDentalClaimData(Convert.ToInt32(hdnClaimIdRendering.Value), "claims_provider_information", CON.ClaimsPanelNames.RenderingProvider);

            if (Helper.HasRows(dsRendering) && Convert.ToInt32(dsRendering.Tables[0].Rows[0]["Is_Primary"]) == 0 &&
                Convert.ToInt32(dsRendering.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimIdRendering.Value) &&
                dsRendering.Tables[0].Rows[0]["Claims_Panel_Name"].ToString() == CON.ClaimsPanelNames.RenderingProvider)
            {
                txtRenderingProvNPI.Text = dsRendering.Tables[0].Rows[0]["NPI"].ToString();
                lblRenderingMedicaidID.Text = dsRendering.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                hdnRendMedId.Value = dsRendering.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                lblRenderingFirstName.Text = dsRendering.Tables[0].Rows[0]["First_Name"].ToString();
                lblRenderingLastName.Text = dsRendering.Tables[0].Rows[0]["Last_Name"].ToString();
            }
        }
    }
    public bool ValidateNPI(string TextRenderingProvNPI="")
    {
        string NPICheckRef = ReferringProviderNPI;
        string NPICheckPri = PrimaryProviderNPI;
        string NPICheckSuper = SupervisingProviderNPI;
        string NPICheckAssist = AssistantProviderNPI;
        string billingNPI = BillingNPI;

        string MedicaidRef = ReferringProvider_Medicaid;
        string MedicaidPri = PrimaryProvider_Medicaid;
        string MedicaidSuper = SupervisingProvider_Medicaid;
        string MedicaidAssist = Assistant_MedicaidID;

        bool result = true;
        lblRenderingErrorMessage.Text = string.Empty;

        if ((!string.IsNullOrEmpty(TextRenderingProvNPI.Trim()) && TextRenderingProvNPI.Trim() == NPICheckRef))
        {
            lblRenderingErrorMessage.Text = "Rendering Provider cannot be same as Referring Provider";
            ClearFields();
            result=false;
        }
        if ((!string.IsNullOrEmpty(TextRenderingProvNPI.Trim()) && TextRenderingProvNPI.Trim() == NPICheckPri))
        {
            lblRenderingErrorMessage.Text = "Rendering Provider cannot be same as Primary Provider";
            ClearFields();
            result = false;
        }
        if ((!string.IsNullOrEmpty(TextRenderingProvNPI.Trim()) && TextRenderingProvNPI.Trim() == NPICheckSuper))
        {
            if (hdnClaimType_Rendering.Value == CON.ClaimsType.Dental || hdnClaimType_Rendering.Value == CON.ClaimsType.Professional)
            {
                lblRenderingErrorMessage.Text = "Rendering Provider cannot be same as Supervising Provider";
                ClearFields();
                result = false;
            }
        }
        if ((!string.IsNullOrEmpty(TextRenderingProvNPI.Trim()) && TextRenderingProvNPI.Trim() == NPICheckAssist))
        {
            if (hdnClaimType_Rendering.Value == CON.ClaimsType.Dental || hdnClaimType_Rendering.Value == CON.ClaimsType.Professional)
            {
                lblRenderingErrorMessage.Text = "Rendering Provider cannot be same as Assistant Provider";
                ClearFields();
                result = false;
            }
        }
        if (!string.IsNullOrEmpty(AssistantProviderNPI.Trim()))
        {
            lblRenderingErrorMessage.Text = "Rendering and Assistant surgeon provider both cannot be entered in the same claim";
             ClearFields();
             result = false;
        }
        if (!string.IsNullOrEmpty(TextRenderingProvNPI.Trim()) && TextRenderingProvNPI.Trim() == billingNPI)
        {
            lblRenderingErrorMessage.Text = "Rendering provider ID should only be entered if it is different than billing provider ID";
            ClearFields();
            result = false;
        }
        return result;
    }
    protected void txtRenderingProvNPI_TextChangedNPI2(object sender, EventArgs e)
    {
        if (ValidateNPI(txtRenderingProvNPI.Text))
        {
            lblRenderingErrorMessage.Text = string.Empty;
            
            if (!string.IsNullOrEmpty(lblRenderingErrorMessage.Text))
            {
                ClearFields();
            }
           else if(!string.IsNullOrEmpty(txtRenderingProvNPI.Text))
           {
                DataTable dt = Claims.GetData(txtRenderingProvNPI.Text.Trim(), hdnRendMedId.Value, "", "");
                if (Helper.HasRows(dt))
                {
                    if (dt.Rows.Count >= 2)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { $('#myModalpop').modal('show')});</script>", false);

                    }
                    else
                    {
                        Claims.NPITextChanged(txtRenderingProvNPI.Text, hdnRendMedId.Value, null, lblRenderingFirstName, lblRenderingLastName, null, lblRenderingErrorMessage, CON.renderingProvider);

                        if (!string.IsNullOrEmpty(hdnRendMedId.Value))
                        {
                            DataSet dsProviderInformation = Claims.LoadProviderInformation(hdnRendMedId.Value.ToString());
                            string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

                            if (entityTypeId != "1" && hdnClaimType_Rendering.Value == CON.ClaimsType.Institutional)
                            {
                                lblRenderingErrorMessage.Text = "Non-individual provider cannot be entered as rendering provider";
                                ClearFields();
                                return;
                            }
                        }
                    }
                }
                else
                {
                    lblRenderingErrorMessage.Text = "NPI is Unknown";
                    ClearFields();
                }
            }
           else  if (!string.IsNullOrEmpty(txtRenderingProvNPI.Text) && string.IsNullOrEmpty(lblRenderingMedicaidID.Text))
            {
                lblRenderingErrorMessage.Text = "NPI is Unknown";
                ClearFields();
            }
            if (!string.IsNullOrEmpty(lblRenderingErrorMessage.Text))
            {
                ClearFields();
            }
        }
    }
    private void AssignValidationSummary(string validationSummary)
    {
        //rfvRenderingProvNPI.ValidationGroup = validationSummary;
        //revRefNPI.ValidationGroup = validationSummary;
    }
    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "validateClaims";
        this.Page.Validators.Add(val);
        return false;
    }
    private bool ValidateRenderingProvider(string txtRenderingNPI, string txtSupervisingNPI)
    {
        if((!string.IsNullOrWhiteSpace(txtRenderingNPI) && !string.IsNullOrWhiteSpace(txtSupervisingNPI)) && (txtRenderingNPI == txtSupervisingNPI))
        {
            AddValidationErrorMessage("Rendering and Supervising cannot have same NPI");
            return false;
        }
        else
        {
            return true;
        }
        
    }

    public void SaveRenderingInfo(string validationSummary, string txtRenderingNPI, string txtSupervisingNPI,int actionButton)
    {
        if(actionButton == CON.ActionButtonType.Submit)
        {
            AssignValidationSummary(validationSummary);
            Page.Validate(validationSummary);
            if (Page.IsValid && ValidateRenderingProvider(txtRenderingNPI, txtSupervisingNPI))
            {
                SaveToDB();
            }
        }
        else
        {
            SaveToDB();
        }
       
        
    }
    private void SaveToDB()
    {
        if (!string.IsNullOrEmpty(hdnClaimIdRendering.Value))
        {
            Claims.SaveClaimsNPIDetails(txtRenderingProvNPI.Text, lblRenderingMedicaidID.Text, lblRenderingFirstName.Text,
            lblRenderingLastName.Text, "claims_provider_information", 0, CON.ClaimsPanelNames.RenderingProvider, Convert.ToInt32(hdnClaimIdRendering.Value));
            GetRenderingProviderDetails();
        }
    }
    private void SetReadOnlyFieldsControl(bool value)
    {
        txtRenderingProvNPI.ReadOnly = value;
        Searchd.Visible = !value;
        txtRenderingProvNPI.Visible = true;


    }

    public void ClearFields()
    {
        txtRenderingProvNPI.Text = string.Empty;
        lblRenderingMedicaidID.Text = string.Empty;
        lblRenderingFirstName.Text = string.Empty;
        lblRenderingLastName.Text = string.Empty;
        hdnRendFirstName.Value = string.Empty;
        hdnRendLastName.Value = string.Empty;
        hdnRendMedId.Value = string.Empty;

    }
}