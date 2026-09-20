using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claims = Models.Data.Claims;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_SupervisingProviderPanel : System.Web.UI.UserControl
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
            if (!string.IsNullOrWhiteSpace(hdnClaimIdSuperVising.Value))
                return hdnClaimIdSuperVising.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimIdSuperVising.Value = value.Trim();
        }
    }

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimType_SuperVising.Value))
                return hdnClaimType_SuperVising.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimType_SuperVising.Value = value.Trim();
        }
    }

    public string SupervisingProviderNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtSupervisingProviderNPI.Text))
                return txtSupervisingProviderNPI.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtSupervisingProviderNPI.Text = value;
        }
    }

    public string SupervisingProvider_MedicaidID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblSupervisingProviderMediID.Text))
                return lblSupervisingProviderMediID.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (string.IsNullOrEmpty(SupervisingProviderNPI))
                {
                    lblSupervisingProviderMediID.Text = value;
                }
                hdnMedicaidSuper.Value = value;
            }
        }
    }

    public string SupervisingProvider_FirstName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblSupervisingFirstName.Text))
                return lblSupervisingFirstName.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblSupervisingFirstName.Text = value;
        }
    }

    public string SupervisingProvider_LastName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblSupervisingLastName.Text))
                return lblSupervisingLastName.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblSupervisingLastName.Text = value;
        }
    }
    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(lblSupervisingProviderMediID.Text))
        {
            rtn = true;
        }

        return rtn;
    }
    private string rendprovnpi = string.Empty;
    public string RenderingProviderNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(rendprovnpi))
                return rendprovnpi;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                rendprovnpi = value;
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
    private string assistantnpi = string.Empty;
    public string AssistantNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(assistantnpi))
                return assistantnpi;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                assistantnpi = value;
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

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {

                if (!string.IsNullOrEmpty(hdnClaimIdSuperVising.Value))
                {
                    GetSupervisingProviderDetails();
                }
            }
            catch (Exception ex)
            {

            }
        }
        lblSupervisingError.Text = hdnErrorMessageSuper.Value;
        if (!string.IsNullOrEmpty(lblSupervisingError.Text))
        {
            lblSupervisingProviderMediID.Text = string.Empty;
            lblSupervisingLastName.Text = string.Empty;
            lblSupervisingFirstName.Text = string.Empty;
        }
        lblSupervisingError.Text = hdnErrorMessageSuper.Value = string.Empty;
        if (txtSupervisingProviderNPI.Text.Length < 10 && !string.IsNullOrEmpty(txtSupervisingProviderNPI.Text))
        {
            ClearSupervisingProviderPanel();
        }


        if (!string.IsNullOrEmpty(hdnMedicaidSuper.Value.ToString()))
        {
            if (string.IsNullOrEmpty(txtSupervisingProviderNPI.Text))
            {
                lblSupervisingProviderMediID.Text = hdnMedicaidSuper.Value.ToString();
            }
        }
        if (!string.IsNullOrEmpty(hdnFirstNameSuper.Value.ToString()))
        {
            lblSupervisingFirstName.Text = hdnFirstNameSuper.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnLastNameSuper.Value.ToString()))
        {
            lblSupervisingLastName.Text = hdnLastNameSuper.Value.ToString();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
       
    }
    public bool ValidateNPI(string TextSupervisingProvNPI = "")
    {
        string NPICheckRend = RenderingProviderNPI;
        string assistantNPI = AssistantNPI;
        string MedicaidAssistant= Assistant_MedicaidID;
        string MedicaidRend = RenderingProvider_MedicaidID;

        bool result = true;
        lblSupervisingError.Text = string.Empty;
        //if(string.IsNullOrEmpty(NPICheckRend) && string.IsNullOrEmpty(MedicaidRend))
        //{
        //    return result;
        //}
        if ((!string.IsNullOrEmpty(TextSupervisingProvNPI.Trim()) && TextSupervisingProvNPI.Trim() == NPICheckRend))
        {
            if (hdnClaimType_SuperVising.Value == CON.ClaimsType.Dental || hdnClaimType_SuperVising.Value == CON.ClaimsType.Professional)
            {
                lblSupervisingError.Text = "Supervising Provider cannot be same as Rendering Provider";
                ClearSupervisingProviderPanel();
                result = false;
            }
        }

        if ((!string.IsNullOrEmpty(TextSupervisingProvNPI.Trim()) && TextSupervisingProvNPI.Trim() == assistantNPI))
        {
            if (hdnClaimType_SuperVising.Value == CON.ClaimsType.Dental || hdnClaimType_SuperVising.Value == CON.ClaimsType.Professional)
            {
                lblSupervisingError.Text = "Assistant surgeon and supervising provider cannot be same";
            }
            if (hdnClaimType_SuperVising.Value == CON.ClaimsType.Institutional)
            {
                lblSupervisingError.Text = "Operating Physician cannot be same as Other Operating Physician Provider";
                
            }
            ClearSupervisingProviderPanel();
            return result = false;
        }
        return result;
    }
    public void GetSupervisingProviderDetails()
    {
        if (!string.IsNullOrEmpty(hdnClaimIdSuperVising.Value))
        {
            DataSet dsSupervising;
            if (hdnClaimType_SuperVising.Value == CON.ClaimsType.Institutional) // if institutional show operating physician panel
            {
                dsSupervising = svc.SelectDentalClaimData(Convert.ToInt32(hdnClaimIdSuperVising.Value), "claims_provider_information", CON.ClaimsPanelNames.OperatingPhysicianInfo);
            }
            else
            {
                dsSupervising = svc.SelectDentalClaimData(Convert.ToInt32(hdnClaimIdSuperVising.Value), "claims_provider_information", CON.ClaimsPanelNames.SupervisingProvider);
            }
            if (Helper.HasRows(dsSupervising) && Convert.ToInt32(dsSupervising.Tables[0].Rows[0]["Is_Primary"]) == 0 &&
                Convert.ToInt32(dsSupervising.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimIdSuperVising.Value) &&
                dsSupervising.Tables[0].Rows[0]["Claims_Panel_Name"].ToString() == CON.ClaimsPanelNames.SupervisingProvider)
            {
                txtSupervisingProviderNPI.Text = dsSupervising.Tables[0].Rows[0]["NPI"].ToString();
                if (string.IsNullOrEmpty(txtSupervisingProviderNPI.Text))
                {
                    lblSupervisingProviderMediID.Text = dsSupervising.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                }
                hdnMedicaidSuper.Value = dsSupervising.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                lblSupervisingFirstName.Text = dsSupervising.Tables[0].Rows[0]["First_Name"].ToString();
                lblSupervisingLastName.Text = dsSupervising.Tables[0].Rows[0]["Last_Name"].ToString();
            }
        }
    }
    private void AssignValidationSummary(string validationSummary)
    {
        //rfvSupervisingProviderNPI.ValidationGroup = validationSummary;
        //revSupervisingProviderNPI.ValidationGroup = validationSummary;
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
    private bool ValidateSuperVisingProvider(string txtSupervisingNPI, string txtRenderingNPI)
    {
        if ((!string.IsNullOrWhiteSpace(txtRenderingNPI) && !string.IsNullOrWhiteSpace(txtSupervisingNPI)) && (txtRenderingNPI == txtSupervisingNPI))
        {
            AddValidationErrorMessage("Rendering and Supervising cannot have same NPI");
            return false;
        }
        else
        {
            return true;
        }

    }
    public void SaveSupervisingProviderInfo(string validationSummary,string txtSupervisingNPI, string txtRenderingNPI,int actionButton)
    {
        if(actionButton == CON.ActionButtonType.Submit)
        {
            AssignValidationSummary(validationSummary);
            Page.Validate(validationSummary);
            if (Page.IsValid && ValidateSuperVisingProvider(txtSupervisingNPI, txtRenderingNPI))
            {
                SaveToDb();
            }
        }
        else
        {
            if(ValidateSuperVisingProvider(txtSupervisingNPI, txtRenderingNPI))
            {
                SaveToDb();
            }
        }
     
        
    }
    private void SaveToDb()
    {
        if (!string.IsNullOrEmpty(hdnClaimIdSuperVising.Value))
        {
            if (hdnClaimType_SuperVising.Value == CON.ClaimsType.Institutional) // if institutional save operating physician panel
            {
                Claims.SaveClaimsNPIDetails(txtSupervisingProviderNPI.Text, hdnMedicaidSuper.Value, lblSupervisingFirstName.Text,
                           lblSupervisingLastName.Text, "claims_provider_information", 0, CON.ClaimsPanelNames.OperatingPhysicianInfo, Convert.ToInt32(hdnClaimIdSuperVising.Value));

            }
            else
            {
                Claims.SaveClaimsNPIDetails(txtSupervisingProviderNPI.Text, hdnMedicaidSuper.Value, lblSupervisingFirstName.Text,
                lblSupervisingLastName.Text, "claims_provider_information", 0, CON.ClaimsPanelNames.SupervisingProvider, Convert.ToInt32(hdnClaimIdSuperVising.Value));

            }
        }
    }
    protected void txtSupervisingProvNPI_TextChangedNPI2(object sender, EventArgs e)
    {
      if (ValidateNPI(txtSupervisingProviderNPI.Text))
         {
            lblSupervisingError.Text = string.Empty;            
            if (!string.IsNullOrWhiteSpace(txtSupervisingProviderNPI.Text))
            {
                DataTable dt = Claims.GetData(txtSupervisingProviderNPI.Text.Trim(), "", "", "");
                if (Helper.HasRows(dt))
                {
                    if (dt.Rows.Count >= 2)
                    {

                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { $('#myModalpop').modal('show')});</script>", false);

                    }
                    else
                    {
                        Claims.NPITextChanged(txtSupervisingProviderNPI.Text, lblSupervisingProviderMediID, null, lblSupervisingFirstName, lblSupervisingLastName, null, lblSupervisingError, CON.supervisingProvider);

                        if (!string.IsNullOrEmpty(hdnMedicaidSuper.Value.ToString()))
                        {
                            DataSet dsProviderInformation = Claims.LoadProviderInformation(hdnMedicaidSuper.Value.ToString());
                            string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

                            if (entityTypeId != "1")
                            {
                                if (hdnClaimType_SuperVising.Value == CON.ClaimsType.Dental || hdnClaimType_SuperVising.Value == CON.ClaimsType.Professional)
                                {
                                    lblSupervisingError.Text = "Non - individual provider cannot be entered as supervising provider";
                                }
                                if (hdnClaimType_SuperVising.Value == CON.ClaimsType.Institutional)
                                {
                                    lblSupervisingError.Text = "Non - individual provider cannot be entered as operative physician";
                                }

                                ClearSupervisingProviderPanel();
                                return;
                            }
                        }
                    }
                }
                else
                {
                    lblSupervisingError.Text = "NPI is Unknown";
                    ClearSupervisingProviderPanel();
                }
            }
            else if (!string.IsNullOrEmpty(txtSupervisingProviderNPI.Text) && string.IsNullOrEmpty(lblSupervisingProviderMediID.Text))
            {
                lblSupervisingError.Text = "NPI is Unknown";
                ClearSupervisingProviderPanel();
            }
            if (!string.IsNullOrEmpty(lblSupervisingError.Text))
            {
                ClearSupervisingProviderPanel();
            }            
        }
           
    }
    public void ClearSupervisingProviderPanel()
    {
        txtSupervisingProviderNPI.Text = string.Empty;
        lblSupervisingProviderMediID.Text = string.Empty;
        lblSupervisingFirstName.Text = string.Empty;
        lblSupervisingLastName.Text = string.Empty;
        hdnFirstNameSuper.Value= string.Empty;
        hdnLastNameSuper.Value= string.Empty;
        hdnMedicaidSuper.Value= string.Empty;
    }
    private void SetReadOnlyFieldsControl(bool value)
    {
        txtSupervisingProviderNPI.ReadOnly = value;
        Searchdiv.Visible = !value;
       // txtSupervisingProviderNPI.Enabled = value;
       // btnSearch8.Visible = !value;
    }
}