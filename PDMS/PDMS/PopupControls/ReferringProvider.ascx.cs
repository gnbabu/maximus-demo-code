using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claims = Models.Data.Claims;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_ReferringProvider : BasePopupControl
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
            if (!string.IsNullOrWhiteSpace(hdnReferringProviderClaimID.Value))
                return hdnReferringProviderClaimID.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnReferringProviderClaimID.Value = value.Trim();
        }
    }

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnReferringProviderClaimType.Value))
                return hdnReferringProviderClaimType.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnReferringProviderClaimType.Value = value.Trim();
        }
    }

    public string ReferringProviderNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtReferringProviderNPI.Text))
                return txtReferringProviderNPI.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtReferringProviderNPI.Text = value;
        }
    }

    public string ReferringProvider_MedicaidID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblRefProviderMedicaidID.Text))
                return lblRefProviderMedicaidID.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (string.IsNullOrEmpty(ReferringProviderNPI))
                {
                    lblRefProviderMedicaidID.Text = value;
                }
                hdnRefMedId.Value = value;
            }
        }
    }

    public string ReferringProvider_FirstName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblReffProviderFirstName.Text))
                return lblReffProviderFirstName.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblReffProviderFirstName.Text = value;
        }
    }

    public string ReferringProvider_LastName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblReffProviderLastName.Text))
                return lblReffProviderLastName.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblReffProviderLastName.Text = value;
        }
    }

    public string PrimaryCareProviderNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtPrimaryCareProviderNPI.Text))
                return txtPrimaryCareProviderNPI.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtPrimaryCareProviderNPI.Text = value;
        }
    }

    public string PrimaryCareProvider_MedicaidID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lbPrimaryCareProvMedicaidID.Text))
                return lbPrimaryCareProvMedicaidID.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (string.IsNullOrEmpty(PrimaryCareProviderNPI))
                {
                    lbPrimaryCareProvMedicaidID.Text = value;
                }
                hdnPrimaryRefMedId.Value = value;
            }
        }
    }

    public string PrimaryCareProvider_FirstName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblPrimaryCareProvFirstName.Text))
                return lblPrimaryCareProvFirstName.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblPrimaryCareProvFirstName.Text = value;
        }
    }

    public string PrimaryCareProvider_LastName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblPrimaryCareProvLastName.Text))
                return lblPrimaryCareProvLastName.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblPrimaryCareProvLastName.Text = value;
        }
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

    public bool ShowPrimaryCareProvider 
    {
        set
        {
            dvPrimaryCareProvider.Visible = value;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (hdnReferringProviderClaimType.Value == CON.ClaimsType.Dental || hdnReferringProviderClaimType.Value == CON.ClaimsType.Professional)
        {
            dvReferringProvider.Visible = true;
            dvPrimaryCareProvider.Visible = true;
            txtPrimaryCareProviderNPI.Enabled = false;
        }

        if (hdnReferringProviderClaimType.Value == CON.ClaimsType.Institutional)
        {
            dvReferringProvider.Visible = true;
            dvPrimaryCareProvider.Visible = false;
        }
        if(!dvPrimaryCareProvider.Visible) dvlblReferringProvider.Visible = false;
        else dvlblReferringProvider.Visible = true;

        if (!IsPostBack)
        {
            GetReferringProviderDetails();
        }
        if (!string.IsNullOrEmpty(txtReferringProviderNPI.Text) && !string.IsNullOrEmpty(lblRefProviderMedicaidID.Text))
        {
            txtPrimaryCareProviderNPI.Enabled = true;
            txtPrimaryCareProviderNPI.BackColor = System.Drawing.Color.White;
        }
        else
        {
            txtPrimaryCareProviderNPI.Enabled = false;
            txtPrimaryCareProviderNPI.BackColor = System.Drawing.Color.LightGray;
        }
        errReferringProviderNPI.Text = hdnErrorMessageRef.Value;
        if (!string.IsNullOrEmpty(errReferringProviderNPI.Text))
        {
            lblRefProviderMedicaidID.Text = string.Empty;
            lblReffProviderFirstName.Text = string.Empty;
            lblReffProviderLastName.Text = string.Empty;
        }
        errReferringProviderNPI.Text = hdnErrorMessageRef.Value = string.Empty;

        errPrimaryCareProviderNPI.Text = hdnErrorMessagePri.Value;
        if (!string.IsNullOrEmpty(errPrimaryCareProviderNPI.Text))
        {
            lbPrimaryCareProvMedicaidID.Text = string.Empty;
            lblPrimaryCareProvLastName.Text = string.Empty;
            lblPrimaryCareProvFirstName.Text = string.Empty;
        }
        errPrimaryCareProviderNPI.Text = hdnErrorMessagePri.Value = string.Empty;

        if(txtReferringProviderNPI.Text.Length<10 && !String.IsNullOrEmpty(txtReferringProviderNPI.Text))
        {
            ClearReferFields();
        }
        else if(txtPrimaryCareProviderNPI.Text.Length < 10 && !string.IsNullOrEmpty(txtPrimaryCareProviderNPI.Text))
        {
            ClearPrimaryFields();
        }
        else if (!string.IsNullOrEmpty(hdnRefMedId.Value))
        {
            DataSet dsProviderInformation = Claims.LoadProviderInformation(hdnRefMedId.Value.ToString());
            string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

            if (entityTypeId != "1")
            {
                if (hdnReferringProviderClaimType.Value == CON.ClaimsType.Dental || hdnReferringProviderClaimType.Value == CON.ClaimsType.Professional || hdnReferringProviderClaimType.Value == CON.ClaimsType.Institutional)
                {
                    errReferringProviderNPI.Text = "Non-individual provider cannot be entered as referring provider";
                }

                ClearReferFields();
                return;
            }
        }
        else if (!string.IsNullOrEmpty(hdnPrimaryRefMedId.Value))
        {
            DataSet dsProviderInformation = Claims.LoadProviderInformation(hdnPrimaryRefMedId.Value.ToString());
            string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

            if (entityTypeId != "1")
            {
                if (hdnReferringProviderClaimType.Value == CON.ClaimsType.Dental || hdnReferringProviderClaimType.Value == CON.ClaimsType.Professional)
                {
                    errPrimaryCareProviderNPI.Text = "Non-individual provider cannot be entered as primary care provider";
                }

                ClearPrimaryFields();
                return;
            }
        }

        if (!string.IsNullOrEmpty(hdnRefMedId.Value.ToString()))
        {
            if (string.IsNullOrEmpty(txtReferringProviderNPI.Text))
            {
                lblRefProviderMedicaidID.Text = hdnRefMedId.Value.ToString();
            }
        }
        if (!string.IsNullOrEmpty(hdnRefFirstName.Value.ToString()))
        {
            lblReffProviderFirstName.Text = hdnRefFirstName.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnRefLastName.Value.ToString()))
        {
            lblReffProviderLastName.Text = hdnRefLastName.Value.ToString();
        }

        if (!string.IsNullOrEmpty(hdnPrimaryRefMedId.Value.ToString()))
        {
            if (string.IsNullOrEmpty(txtPrimaryCareProviderNPI.Text))
            {
                lbPrimaryCareProvMedicaidID.Text = hdnPrimaryRefMedId.Value.ToString();
            }
        }
        if (!string.IsNullOrEmpty(hdnPrimaryRefFirstName.Value.ToString()))
        {
            lblPrimaryCareProvFirstName.Text = hdnPrimaryRefFirstName.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnPrimaryRefLastName.Value.ToString()))
        {
            lblPrimaryCareProvLastName.Text = hdnPrimaryRefLastName.Value.ToString();
        }

    }


    // txtReferringProviderNPI_TextChangedNPI
    protected void txtReferringProviderNPI_TextChanged(object sender, EventArgs e)
    {
        string NPICheckRend = RenderingProviderNPI;
        string MedicaidRend = RenderingProvider_MedicaidID;
        if (txtReferringProviderNPI.Text.Length < 10 && !string.IsNullOrEmpty(txtReferringProviderNPI.Text))
        {
            ClearReferFields();   
        }
       else if ((!string.IsNullOrEmpty(txtReferringProviderNPI.Text.Trim()) && txtReferringProviderNPI.Text.Trim() == NPICheckRend))
        {
            errReferringProviderNPI.Text = "Referring Provider cannot be same as Rendering Provider";
            ClearReferFields();
            return;
        }
        //Claims.NPITextChanged(txtReferringProviderNPI.Text, lblRefProviderMedicaidID, null, lblReffProviderFirstName, lblReffProviderLastName, null, errReferringProviderNPI);
        if (!string.IsNullOrEmpty(txtReferringProviderNPI.Text) && txtReferringProviderNPI.Text.Length == 10)
        {

            DataTable dt = Claims.GetData(txtReferringProviderNPI.Text.Trim(), "", "", "");
            if (Helper.HasRows(dt))
            {
                if (dt.Rows.Count >= 2)
                {

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { $('#myModalpop').modal('show')});</script>", false);

                }
                else
                {
                    Claims.NPITextChanged(txtReferringProviderNPI.Text, lblRefProviderMedicaidID, null, lblReffProviderFirstName, lblReffProviderLastName, null, errReferringProviderNPI);
                    if (!string.IsNullOrEmpty(hdnRefMedId.Value))
                    {
                        DataSet dsProviderInformation = Claims.LoadProviderInformation(hdnRefMedId.Value.ToString());
                        string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

                        if (entityTypeId != "1")
                        {
                            if (hdnReferringProviderClaimType.Value == CON.ClaimsType.Dental || hdnReferringProviderClaimType.Value == CON.ClaimsType.Professional || hdnReferringProviderClaimType.Value == CON.ClaimsType.Institutional)
                            {
                                errReferringProviderNPI.Text = "Non-individual provider cannot be entered as referring provider";
                            }

                            ClearReferFields();
                            return;
                        }
                    }
                }
            }
            
            txtPrimaryCareProviderNPI.Enabled = true;
            txtPrimaryCareProviderNPI.BackColor = System.Drawing.Color.White;
        }
        else if (!string.IsNullOrEmpty(txtReferringProviderNPI.Text) && string.IsNullOrEmpty(lblRefProviderMedicaidID.Text))
        {
            errReferringProviderNPI.Text = "NPI is Unknown";
            ClearReferFields();
        }
        else if(string.IsNullOrEmpty(txtReferringProviderNPI.Text) && string.IsNullOrEmpty(txtPrimaryCareProviderNPI.Text))
        {
            //lblRefProviderMedicaidID.Text = lblReffProviderLastName.Text = lblReffProviderFirstName.Text = string.Empty;
            txtPrimaryCareProviderNPI.Enabled = false;
            txtPrimaryCareProviderNPI.BackColor = System.Drawing.Color.LightGray;
        }

        if (txtReferringProviderNPI.Text == txtPrimaryCareProviderNPI.Text && (!string.IsNullOrEmpty(txtPrimaryCareProviderNPI.Text)))
        {
            divRefErrorMessage.Style["display"] = "block";
            txtReferringProviderNPI.Text = lblRefProviderMedicaidID.Text = lblReffProviderLastName.Text = lblReffProviderFirstName.Text = string.Empty;
        }
        else
        {
            divRefErrorMessage.Style["display"] = "none";
        }
        if (!string.IsNullOrEmpty(errReferringProviderNPI.Text))
        {
            ClearReferFields();
        }
        
        
    }
    // txtPrimaryCareProvider_TextChanged
    protected void txtPrimaryCareProviderNPI_TextChanged(object sender, EventArgs e)
    {
        string NPICheckRend = RenderingProviderNPI;        
        string MedicaidRend = RenderingProvider_MedicaidID;
        if (txtPrimaryCareProviderNPI.Text.Length < 10 && !string.IsNullOrEmpty(txtPrimaryCareProviderNPI.Text))
        {
            ClearPrimaryFields();
        }
        else if ((!string.IsNullOrEmpty(txtPrimaryCareProviderNPI.Text.Trim()) && txtPrimaryCareProviderNPI.Text.Trim() == NPICheckRend))
        {
            errPrimaryCareProviderNPI.Text = "Primary Provider cannot be same as Rendering Provider";
            ClearPrimaryFields();
            return;
        }
        if (txtReferringProviderNPI.Text == txtPrimaryCareProviderNPI.Text)
        {
            divRefErrorMessage.Style["display"] = "block";
            txtPrimaryCareProviderNPI.Text = lbPrimaryCareProvMedicaidID.Text = lblPrimaryCareProvLastName.Text = lblPrimaryCareProvFirstName.Text = string.Empty;

        }
        else
        {

            DataTable dt = Claims.GetData(txtPrimaryCareProviderNPI.Text.Trim(), "", "", "");
            if (Helper.HasRows(dt)&&!string.IsNullOrEmpty(txtPrimaryCareProviderNPI.Text))            {
                if (dt.Rows.Count >= 2)
                {

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { $('#myModalpop').modal('show')});</script>", false);

                }
                else
                {
                    Claims.NPITextChanged(txtPrimaryCareProviderNPI.Text, lbPrimaryCareProvMedicaidID, null, lblPrimaryCareProvFirstName, lblPrimaryCareProvLastName, null, errPrimaryCareProviderNPI);
                    if (!string.IsNullOrEmpty(hdnPrimaryRefMedId.Value.ToString()))
                    {
                        DataSet dsProviderInformation = Claims.LoadProviderInformation(hdnPrimaryRefMedId.Value.ToString());
                        string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

                        if (entityTypeId != "1")
                        {
                            if (hdnReferringProviderClaimType.Value == CON.ClaimsType.Dental || hdnReferringProviderClaimType.Value == CON.ClaimsType.Professional)
                            {
                                errPrimaryCareProviderNPI.Text = "Non-individual provider cannot be entered as primary care provider";
                            }

                            ClearPrimaryFields();
                            return;
                        }
                    }
                }
            }
            

            divRefErrorMessage.Style["display"] = "none";
        }
        if (!string.IsNullOrEmpty(errPrimaryCareProviderNPI.Text))
        {
            ClearPrimaryFields();
        }
        //if (!string.IsNullOrEmpty(txtPrimaryCareProviderNPI.Text) && string.IsNullOrEmpty(lbPrimaryCareProvMedicaidID.Text))
        //{
        //    errPrimaryCareProviderNPI.Text = "NPI is Unknown";
        //    ClearPrimaryFields();
        //}
       
    }

    public void SaveReferringProviderInfo(string validationSummary, int actionButton)
    {
        if(actionButton == CON.ActionButtonType.Submit)
        {
            AssignValidationSummary(validationSummary);
            Page.Validate(validationSummary);
            if (Page.IsValid)
            {
                SaveReferringProviderPanelDetails(txtReferringProviderNPI.Text, lblRefProviderMedicaidID.Text, lblReffProviderFirstName.Text, lblReffProviderLastName.Text, "Claims_Provider_Information", CON.IsPrimary.ReferringProvider, CON.ClaimsPanelNames.ReferringProvider, validationSummary);
            }
        }
        else
        {
            SaveReferringProviderPanelDetails(txtReferringProviderNPI.Text, lblRefProviderMedicaidID.Text, lblReffProviderFirstName.Text, lblReffProviderLastName.Text, "Claims_Provider_Information", CON.IsPrimary.ReferringProvider, CON.ClaimsPanelNames.ReferringProvider, validationSummary);
        }
       
    }

    public void SavePrimaryCareInfo(string validationSummary, int actionButton)
    {
        if(actionButton == CON.ActionButtonType.Submit)
        {
            AssignValidationSummary(validationSummary);
            Page.Validate(validationSummary);
            if (Page.IsValid)
            {
                SaveReferringProviderPanelDetails(txtPrimaryCareProviderNPI.Text, hdnPrimaryRefMedId.Value, lblPrimaryCareProvFirstName.Text, lblPrimaryCareProvLastName.Text, "Claims_Provider_Information", CON.IsPrimary.PrimaryCareProvider, CON.ClaimsPanelNames.ReferringProvider, validationSummary);
            }
        }
        else
        {
            SaveReferringProviderPanelDetails(txtPrimaryCareProviderNPI.Text, hdnPrimaryRefMedId.Value, lblPrimaryCareProvFirstName.Text, lblPrimaryCareProvLastName.Text, "Claims_Provider_Information", CON.IsPrimary.PrimaryCareProvider, CON.ClaimsPanelNames.ReferringProvider, validationSummary);

        }
    }
    private void AssignValidationSummary(string validationSummary)
    {
        if(ClaimType == CON.ClaimsType.Dental || ClaimType == CON.ClaimsType.Professional)
        {
            txtPrimaryCareProviderNPI.ValidationGroup = validationSummary;
            txtReferringProviderNPI.ValidationGroup = validationSummary;
        }
        if(ClaimType == CON.ClaimsType.Institutional)
        {
            txtReferringProviderNPI.ValidationGroup = validationSummary;
        }
       
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
    public bool ValidateReferring()
    {
        if(txtPrimaryCareProviderNPI.Text.Trim() == txtReferringProviderNPI.Text.Trim())
        {
            AddValidationErrorMessage("Referring Provider and Primary Care Provider can not be same");
            return false;
        }
        else
        {
            return true;
        }
        
    }
    protected void SaveReferringProviderPanelDetails(string _txtRefNPI, string _RefMedicaidId, string _RefFirstName, string _RefLastName, string tableName, int isPrimary, string claimsPanelName,string validationSummary)
    {
        if(!string.IsNullOrWhiteSpace(txtReferringProviderNPI.Text) || !string.IsNullOrWhiteSpace(txtPrimaryCareProviderNPI.Text) ||
            !string.IsNullOrWhiteSpace(lblRefProviderMedicaidID.Text) || !string.IsNullOrWhiteSpace(lbPrimaryCareProvMedicaidID.Text))
        {
            AssignValidationSummary(validationSummary);
            Page.Validate(validationSummary);

            Dictionary<string, string> parms = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(hdnReferringProviderClaimID.Value))
                {
                    DataSet dsReferringClaims = svc.SelectDentalClaimData(Convert.ToInt32(hdnReferringProviderClaimID.Value), "claims_provider_information", claimsPanelName, isPrimary);
                    if (Helper.HasRows(dsReferringClaims) && Convert.ToInt32(dsReferringClaims.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnReferringProviderClaimID.Value))
                    {
                        parms.Add("NPI", _txtRefNPI.ToString());
                        parms.Add("Medicaid_ID", _RefMedicaidId.ToString());
                        parms.Add("First_Name", _RefFirstName.ToString());
                        parms.Add("Last_Name", _RefLastName.ToString());
                        parms.Add("Claim_ID", hdnReferringProviderClaimID.Value.ToString());
                        parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                        parms.Add("Is_Primary", isPrimary.ToString());
                        parms.Add("Claims_Panel_Name", claimsPanelName.ToString());
                        svc.UpdatePanelsData(tableName, parms);
                    }
                    else
                    {
                        parms.Add("NPI", _txtRefNPI.ToString());
                        parms.Add("Medicaid_ID", _RefMedicaidId.ToString());
                        parms.Add("First_Name", _RefFirstName.ToString());
                        parms.Add("Last_Name", _RefLastName.ToString());
                        parms.Add("Claim_ID", hdnReferringProviderClaimID.Value.ToString());
                        parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        parms.Add("Created_Date_Time", DateTime.Now.ToString());
                        parms.Add("Is_Primary", isPrimary.ToString());
                        parms.Add("Claims_Panel_Name", claimsPanelName.ToString());
                        svc.InsertPanelsData(tableName, parms);
                    }
                }           
        }      
    }

    public void GetReferringProviderDetails()
    {
        if (!string.IsNullOrEmpty(hdnReferringProviderClaimID.Value))
        {
            DataSet dsReferring = svc.SelectDentalClaimData(Convert.ToInt32(hdnReferringProviderClaimID.Value), "claims_provider_information", CON.ClaimsPanelNames.ReferringProvider);
            if (Helper.HasRows(dsReferring) && Convert.ToInt32(dsReferring.Tables[0].Rows[0]["Is_Primary"]) == CON.IsPrimary.ReferringProvider &&
                Convert.ToInt32(dsReferring.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnReferringProviderClaimID.Value) &&
                dsReferring.Tables[0].Rows[0]["Claims_Panel_Name"].ToString() == CON.ClaimsPanelNames.ReferringProvider)
            {
                txtReferringProviderNPI.Text = dsReferring.Tables[0].Rows[0]["NPI"].ToString();
                if (string.IsNullOrEmpty(txtReferringProviderNPI.Text))
                {
                    lblRefProviderMedicaidID.Text = dsReferring.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                }
                hdnRefMedId.Value = dsReferring.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                lblReffProviderLastName.Text = dsReferring.Tables[0].Rows[0]["Last_Name"].ToString(); 
                lblReffProviderFirstName.Text = dsReferring.Tables[0].Rows[0]["First_Name"].ToString();
            }

            DataSet dsPrimaryCare = svc.SelectDentalClaimData(Convert.ToInt32(hdnReferringProviderClaimID.Value), "claims_provider_information", CON.ClaimsPanelNames.ReferringProvider, CON.IsPrimary.PrimaryCareProvider);
            if (Helper.HasRows(dsPrimaryCare) &&
                Convert.ToInt32(dsPrimaryCare.Tables[0].Rows[0]["Is_Primary"]) == CON.IsPrimary.PrimaryCareProvider &&
                Convert.ToInt32(dsPrimaryCare.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnReferringProviderClaimID.Value) &&
                  dsPrimaryCare.Tables[0].Rows[0]["Claims_Panel_Name"].ToString() == CON.ClaimsPanelNames.ReferringProvider)
            {
                txtPrimaryCareProviderNPI.Text = dsPrimaryCare.Tables[0].Rows[0]["NPI"].ToString();
                if (string.IsNullOrEmpty(txtPrimaryCareProviderNPI.Text))
                {
                    lbPrimaryCareProvMedicaidID.Text = dsPrimaryCare.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                }
                hdnPrimaryRefMedId.Value = dsPrimaryCare.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                lblPrimaryCareProvFirstName.Text = dsPrimaryCare.Tables[0].Rows[0]["First_Name"].ToString();
                lblPrimaryCareProvLastName.Text = dsPrimaryCare.Tables[0].Rows[0]["Last_Name"].ToString();
            }
        }
    }

    private void SetReadOnlyFieldsControl(bool value)
    {
        txtReferringProviderNPI.ReadOnly = value;
        txtPrimaryCareProviderNPI.ReadOnly = value;
        SearchPrimaryCareProviderNPI.Visible = !value;
        SearchReferringProviderNPI.Visible = !value;
      
    }

    public void ClearReferFields()
    {
        txtReferringProviderNPI.Text = string.Empty;

        lblRefProviderMedicaidID.Text = string.Empty;
        lblReffProviderLastName.Text = string.Empty;
        lblReffProviderFirstName.Text = string.Empty;
       hdnRefMedId.Value = string.Empty;
        hdnRefFirstName.Value = string.Empty;
        hdnRefLastName.Value = string.Empty;
    }
    public void ClearPrimaryFields()
    {
        txtPrimaryCareProviderNPI.Text = string.Empty;

        lbPrimaryCareProvMedicaidID.Text = string.Empty;
        lblPrimaryCareProvFirstName.Text = string.Empty;
        lblPrimaryCareProvLastName.Text = string.Empty;
        hdnPrimaryRefMedId.Value = string.Empty;
        hdnPrimaryRefFirstName.Value = string.Empty;
        hdnPrimaryRefLastName.Value = string.Empty;



        // Primary care panel css if referring is not yet enterred.

        if (!string.IsNullOrEmpty(txtReferringProviderNPI.Text))
        {
            txtPrimaryCareProviderNPI.Enabled = true;
            txtPrimaryCareProviderNPI.BackColor = System.Drawing.Color.White;
        }
        else
        {
            txtPrimaryCareProviderNPI.Enabled = false;
            txtPrimaryCareProviderNPI.BackColor = System.Drawing.Color.LightGray;
        }
    }
}