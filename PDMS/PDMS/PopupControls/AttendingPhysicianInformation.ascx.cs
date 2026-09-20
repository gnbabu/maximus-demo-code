using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claims = Models.Data.Claims;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_AttendingPhysicianInformation : System.Web.UI.UserControl
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
            if (!string.IsNullOrWhiteSpace(hdnAttendingPhysicianClaimID.Value))
                return hdnAttendingPhysicianClaimID.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnAttendingPhysicianClaimID.Value = value.Trim();
        }
    }

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnAttendingPhysicianClaimType.Value))
                return hdnAttendingPhysicianClaimType.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnAttendingPhysicianClaimType.Value = value.Trim();
        }
    }

    public string AttendingPhysicianNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtAttendingPhysicianNPI.Text))
                return txtAttendingPhysicianNPI.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtAttendingPhysicianNPI.Text = value;
        }
    }

    public string AttendingPhysician_MedicaidID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblAttendingPhysicianMedicaidID.Text))
                return lblAttendingPhysicianMedicaidID.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (string.IsNullOrEmpty(AttendingPhysicianNPI))
                    lblAttendingPhysicianMedicaidID.Text = value;
            }
        }
    }

    public string AttendingPhysician_FirstName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblAttendingPhysicianFirstName.Text))
                return lblAttendingPhysicianFirstName.Text;
            else
                return string.Empty;
        }
        set
        {
            //if (!string.IsNullOrWhiteSpace(value))
                lblAttendingPhysicianFirstName.Text = value;
        }
    }

    public string AttendingPhysician_LastName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblAttendingPhysicianLastName.Text))
                return lblAttendingPhysicianLastName.Text;
            else
                return string.Empty;
        }
        set
        {
            //if (!string.IsNullOrWhiteSpace(value))
                lblAttendingPhysicianLastName.Text = value;
        }
    }
    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(txtAttendingPhysicianNPI.Text))
        {
            rtn = true;
        }

        return rtn;
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetAttendingPhysicianInfoDetails();
        }
        lblAttendingPhysicianError.Text = string.Empty;
        if (txtAttendingPhysicianNPI.Text.Length < 10 && !string.IsNullOrEmpty(txtAttendingPhysicianNPI.Text))
        {
            ClearAttendingPhysicianInfo();
        }
        else if (!string.IsNullOrEmpty(lblAttendingPhysicianMedicaidID.Text.ToString()))
        {
            DataSet dsProviderInformation = Claims.LoadProviderInformation(lblAttendingPhysicianMedicaidID.Text.ToString());
            string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

            if (entityTypeId != "1")
            {
                if (hdnAttendingPhysicianClaimType.Value == CON.ClaimsType.Institutional)
                {
                    lblAttendingPhysicianError.Text = "Non-individual provider cannot be entered as attending provider";
                }

                ClearAttendingPhysicianInfo();
                return;
            }
        }
        if (!string.IsNullOrEmpty(hdnMedicaidAttendingPhysician.Value.ToString()))
        {
            lblAttendingPhysicianMedicaidID.Text = hdnMedicaidAttendingPhysician.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnFirstNameAttendingPhysician.Value.ToString()))
        {
            lblAttendingPhysicianFirstName.Text = hdnFirstNameAttendingPhysician.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnLastNameAttendingPhysician.Value.ToString()))
        {
            lblAttendingPhysicianLastName.Text = hdnLastNameAttendingPhysician.Value.ToString();
        }
        txtAttendingPhysicianNPI.Style.Add("background-color", "white");
    }

    protected void txtAttendingPhysicianNPI_TextChanged(object sender, EventArgs e)
    {
        if (txtAttendingPhysicianNPI.Text.Trim().Length == 10)
        {
            lblAttendingPhysicianError.Text = string.Empty;
            DataTable dt = Claims.GetData(txtAttendingPhysicianNPI.Text.Trim(), "", "", "");
            if (!string.IsNullOrEmpty(txtAttendingPhysicianNPI.Text) && Helper.HasRows(dt))
            {
                if (dt.Rows.Count >= 2)
                {

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { $('#myModalpop').modal('show')});</script>", false);

                }
                else
                {
                    Claims.NPITextChanged(txtAttendingPhysicianNPI.Text.Trim(), lblAttendingPhysicianMedicaidID, null, lblAttendingPhysicianFirstName, lblAttendingPhysicianLastName, null, lblAttendingPhysicianError);
                    if (!string.IsNullOrEmpty(lblAttendingPhysicianMedicaidID.Text.ToString()))
                    {
                        DataSet dsProviderInformation = Claims.LoadProviderInformation(lblAttendingPhysicianMedicaidID.Text.ToString());
                        string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

                        if (entityTypeId != "1")
                        {
                            if (hdnAttendingPhysicianClaimType.Value == CON.ClaimsType.Institutional)
                            {
                                lblAttendingPhysicianError.Text = "Non-individual provider cannot be entered as attending provider";
                            }

                            ClearAttendingPhysicianInfo();
                            return;
                        }
                    }
                }
            }
            else
            {
                lblAttendingPhysicianError.Text = !string.IsNullOrEmpty(txtAttendingPhysicianNPI.Text)? "NPI is Unknown":"";
                ClearAttendingPhysicianInfo();
            }

           
        }
        else if(!string.IsNullOrEmpty(txtAttendingPhysicianNPI.Text) && txtAttendingPhysicianNPI.Text.Length < 10)
        {
            lblAttendingPhysicianError.Text = "10-digit number is required";
            ClearAttendingPhysicianInfo();
        }
        
        if (!string.IsNullOrEmpty(lblAttendingPhysicianError.Text))
        {
            ClearAttendingPhysicianInfo();
        }
       
    }

    private void AssignValidationSummary(string validationSummary)
    {
        //rfvAttendingPhysicianNPI.ValidationGroup = validationSummary;
    }

    public void SaveAttendingPhysicianInfo(string validationSummary , int actionButton)
    {
        if(actionButton == CON.ActionButtonType.Submit)
        {
            AssignValidationSummary(validationSummary);
            Page.Validate(validationSummary);
            if (Page.IsValid)
            {
                Claims.SaveClaimsNPIDetails(txtAttendingPhysicianNPI.Text, lblAttendingPhysicianMedicaidID.Text.Trim(), lblAttendingPhysicianFirstName.Text.Trim(), lblAttendingPhysicianLastName.Text.Trim(), "Claims_Provider_Information", 0, CON.ClaimsPanelNames.AttendingPhysicianInformation, Convert.ToInt32(hdnAttendingPhysicianClaimID.Value));
                GetAttendingPhysicianInfoDetails();

            }
        }
        else
        {
            Claims.SaveClaimsNPIDetails(txtAttendingPhysicianNPI.Text, lblAttendingPhysicianMedicaidID.Text.Trim(), lblAttendingPhysicianFirstName.Text.Trim(), lblAttendingPhysicianLastName.Text.Trim(), "Claims_Provider_Information", 0, CON.ClaimsPanelNames.AttendingPhysicianInformation, Convert.ToInt32(hdnAttendingPhysicianClaimID.Value));
            GetAttendingPhysicianInfoDetails();
        }
        
    }
    public void GetAttendingPhysicianInfoDetails()
    {
        if (!string.IsNullOrEmpty(hdnAttendingPhysicianClaimID.Value))
        {
            DataSet dsAPInfo = svc.SelectDentalClaimData(Convert.ToInt32(hdnAttendingPhysicianClaimID.Value), "claims_provider_information", CON.ClaimsPanelNames.AttendingPhysicianInformation);

            if (Helper.HasRows(dsAPInfo) && Convert.ToInt32(dsAPInfo.Tables[0].Rows[0]["Is_Primary"]) == 0 &&
                Convert.ToInt32(dsAPInfo.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnAttendingPhysicianClaimID.Value) &&
                dsAPInfo.Tables[0].Rows[0]["Claims_Panel_Name"].ToString() == CON.ClaimsPanelNames.AttendingPhysicianInformation)
            {
                txtAttendingPhysicianNPI.Text = dsAPInfo.Tables[0].Rows[0]["NPI"].ToString();
                lblAttendingPhysicianMedicaidID.Text = dsAPInfo.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                lblAttendingPhysicianFirstName.Text = dsAPInfo.Tables[0].Rows[0]["First_Name"].ToString();
                lblAttendingPhysicianLastName.Text = dsAPInfo.Tables[0].Rows[0]["Last_Name"].ToString();
            }
        }
    }

    public void ClearAttendingPhysicianInfo()
    {
        txtAttendingPhysicianNPI.Text = string.Empty;
        lblAttendingPhysicianFirstName.Text = string.Empty;
        lblAttendingPhysicianLastName.Text = string.Empty;
        lblAttendingPhysicianMedicaidID.Text = string.Empty;
        errAttendingPhysicianNPI.Text = string.Empty;
        hdnMedicaidAttendingPhysician.Value=string.Empty;    
          hdnFirstNameAttendingPhysician.Value=string.Empty;
        hdnLastNameAttendingPhysician.Value=string.Empty;

    }


    private void SetReadOnlyFieldsControl(bool value)
    {
        txtAttendingPhysicianNPI.ReadOnly = value;
        txtAttendingPhysicianNPI.Visible = true;
        searchAt.Visible = !value;
    }
}