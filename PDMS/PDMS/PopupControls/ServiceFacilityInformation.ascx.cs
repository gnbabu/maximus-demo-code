using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using Claims = Models.Data.Claims;

public partial class UserControls_ServiceFacilityInformation : System.Web.UI.UserControl
{
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
            if (!string.IsNullOrWhiteSpace(hdnClaimId.Value))
                return hdnClaimId.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimId.Value = value.Trim();
        }
    }
    public string NPIServiceFacilityInformation
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtNPIServiceFacilityLocation.Text))
                return txtNPIServiceFacilityLocation.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtNPIServiceFacilityLocation.Text = value.Trim();
        }
    }
    public string MedicaidIdServiceFacilityInformation
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtMedicaidIdServiceFacilityLocation.Text))
                return txtMedicaidIdServiceFacilityLocation.Text;
            else if (!string.IsNullOrEmpty(hdnServiceFacilityMedId.Value))
                return hdnServiceFacilityMedId.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (string.IsNullOrEmpty(txtNPIServiceFacilityLocation.Text))
                {
                    txtMedicaidIdServiceFacilityLocation.Text = value.Trim();
                }
                hdnServiceFacilityMedId.Value = value.Trim();
            }
        }
    }
    public string NameServiceFacilityInformation
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnServiceFacilityName.Value))
            {
                return hdnServiceFacilityName.Value;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(txtNameServiceFacilityLocation.Text))
                    return txtNameServiceFacilityLocation.Text;
                else if (!string.IsNullOrEmpty(hdnServiceFacilityName.Value))
                {
                    return hdnServiceFacilityName.Value;
                }
                else
                    return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                txtNameServiceFacilityLocation.Text = value.Trim();
                hdnServiceFacilityName.Value = value.Trim();
            }
        }
    }
    public string AddressLine1ServiceFacilityInformation
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtAddressServiceFacilityLocation.Text))
                return txtAddressServiceFacilityLocation.Text;
            else if (!string.IsNullOrEmpty(hdnServiceFacilityAddress1.Value))
                return hdnServiceFacilityAddress1.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                txtAddressServiceFacilityLocation.Text = value.Trim();
                hdnServiceFacilityAddress1.Value = value.Trim();
            }
        }
    }
    public string AddressLine2ServiceFacilityInformation
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtAddressServiceFacilityLocation2.Text))
                return txtAddressServiceFacilityLocation2.Text;
            else if (!string.IsNullOrEmpty(hdnServiceFacilityAddress1.Value))
                return hdnServiceFacilityAddress2.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                txtAddressServiceFacilityLocation2.Text = value.Trim();
                hdnServiceFacilityAddress2.Value = value.Trim();
            }
        }
    }
    public string CityServiceFacilityInformation
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtCityServiceFacilityLocation.Text))
                return txtCityServiceFacilityLocation.Text.Trim();
            else if (!string.IsNullOrEmpty(hdnServiceFacilityCity.Value))
                return hdnServiceFacilityCity.Value.Trim();
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                txtCityServiceFacilityLocation.Text = value.Trim();
                hdnServiceFacilityCity.Value = value.Trim();
            }
        }
    }
    public string StateServiceFacilityInformation
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtStateServiceFacilityLocation.Text))
                return txtStateServiceFacilityLocation.Text;
            else if (!string.IsNullOrEmpty(hdnServiceFacilityState.Value))
                return hdnServiceFacilityState.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                txtStateServiceFacilityLocation.Text = value.Trim();
                hdnServiceFacilityState.Value = value.Trim();
            }
        }
    }
    public string ZipServiceFacilityInformation
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtZipServiceFacilityLocation.Text))
                return txtZipServiceFacilityLocation.Text;
            else if (!string.IsNullOrEmpty(hdnServiceFacilityZip.Value))
                return hdnServiceFacilityZip.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                txtZipServiceFacilityLocation.Text = value.Trim();
                hdnServiceFacilityZip.Value = value.Trim();
            }
        }
    }

    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(txtNPIServiceFacilityLocation.Text))
        {
            rtn = true;
        }

        return rtn;
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
       
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
        }

        if (!string.IsNullOrEmpty(hdnServiceFacilityMedId.Value.ToString()))
        {
            DataSet dsProviderInformation = LoadProviderInformation(hdnServiceFacilityMedId.Value.ToString());
            string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

            if (entityTypeId == "1")
            {
                errServiceFacilityInfo.Text = "Individual provider cannot be entered as facility provider";
                txtNPIServiceFacilityLocation.Text = "";
                ClearField();
                return;
            }
        }
        errServiceFacilityInfo.Text = string.Empty;
        if (!string.IsNullOrEmpty(hdnServiceFacilityMedId.Value.ToString()))
        {
            if (string.IsNullOrEmpty(txtNPIServiceFacilityLocation.Text)) {
                lblServiceFacilityLocationMedicaid.InnerText = hdnServiceFacilityMedId.Value.ToString();
            }
        }
        if (!string.IsNullOrEmpty(hdnServiceFacilityName.Value.ToString()))
        {
            lblServiceFacilityLocationName.InnerText = hdnServiceFacilityName.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnServiceFacilityAddress1.Value.ToString()))
        {
            lblServiceFacilityLocationAddress1.InnerText = hdnServiceFacilityAddress1.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnServiceFacilityAddress2.Value.ToString()))
        {
            lblServiceFacilityLocationAddress2.InnerText = hdnServiceFacilityAddress2.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnServiceFacilityCity.Value.ToString()))
        {
            lblServiceFacilityLocationCity.InnerText = hdnServiceFacilityCity.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnServiceFacilityState.Value.ToString()))
        {
            lblServiceFacilityLocationState.InnerText = hdnServiceFacilityState.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnServiceFacilityZip.Value.ToString()))
        {
            lblServiceFacilityLocationZip.InnerText = hdnServiceFacilityZip.Value.ToString();
        }
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
    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;
    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }
    public void BindGrid()
    {
        GetServiceFacilityInformation();
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
        }
        return;
    }
    protected void NPITextChangedServiceFacilityInformation(TextBox _txtNPI, Label _txtMedicaidId, Label _txtproviderName, Label _txtAddress1, Label _txtAddress2, Label _city, Label _state, Label _zip, Label errorLabel)
    {
        errorLabel.Text = "";
        if (_txtNPI.Text.Trim().Length == 10 || string.IsNullOrEmpty(_txtNPI.Text))
        {
            var isValid = (string.IsNullOrEmpty(_txtNPI.Text) || ValidProviderNPI(_txtNPI.Text.Trim()));
            if (!isValid)
            {
                errorLabel.Text = "NPI not found in the system";
            }
            else
            {
                DataTable dt = GetData(_txtNPI.Text.Trim(), "", "", "");
                if (dt != null)
                {
                    // dt = dt.Select("NPI <> ''").CopyToDataTable();
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        if (_txtMedicaidId != null)
                        {
                            _txtMedicaidId.Text = row["MEDICAID_ID"].ToString();
                            if (!string.IsNullOrEmpty(_txtNPI.Text))
                            {
                                _txtMedicaidId.Visible = false;
                            }
                        }

                        if (_txtproviderName != null)
                        {
                            _txtproviderName.Text = row["FIRST_NAME"].ToString() + " " + row["LAST_OR_BUSINESS_NAME"].ToString();
                        }

                        if (_txtAddress1 != null)
                        {
                            _txtAddress1.Text = row["ADDRESS1"].ToString();
                        }
                        if (_txtAddress2 != null)
                        {
                            _txtAddress2.Text = row["ADDRESS2"].ToString();
                        }
                        if (_state != null)
                        {
                            _state.Text = row["STATE"].ToString();
                        }
                        if (_city != null)
                        {
                            _city.Text = row["CITY"].ToString();
                        }
                        if (_zip != null)
                        {
                            _zip.Text = row["ZIP"].ToString();
                        }
                    }
                }
            }
        }
        else
        {
            errServiceFacilityInfo.Text = "10-digit number is required";
        }
    }

    private DataSet LoadProviderInformation(string medicaidNumber)
    {
        DataSet ds = svc.SelectProviderByGRPMedicaidID(medicaidNumber);
        return ds;
    }
    protected void txtServiceFacilityInformation_TextChangedNPI2(object sender, EventArgs e)
    {
        if (txtNPIServiceFacilityLocation.Text.Length < 10 && !string.IsNullOrEmpty(txtNPIServiceFacilityLocation.Text))
        {
            ClearField();
        }
        else if (!string.IsNullOrWhiteSpace(txtNPIServiceFacilityLocation.Text) && !string.IsNullOrWhiteSpace(txtMedicaidIdServiceFacilityLocation.Text))
        {
            DataTable dt = Claims.GetData(txtNPIServiceFacilityLocation.Text.Trim(), "", "", "");
            if (Helper.HasRows(dt) || !string.IsNullOrWhiteSpace(hdnServiceFacilityMedId.Value))
            {
                if (Helper.HasRows(dt) && dt.Rows.Count >= 2)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { $('#myModalpop').modal('show')});</script>", false);
                }
                else
                {
                    NPITextChangedServiceFacilityInformation(txtNPIServiceFacilityLocation, txtMedicaidIdServiceFacilityLocation, txtNameServiceFacilityLocation, txtAddressServiceFacilityLocation, txtAddressServiceFacilityLocation2, txtCityServiceFacilityLocation, txtStateServiceFacilityLocation, txtZipServiceFacilityLocation, errServiceFacilityInfo);
                    if (!string.IsNullOrEmpty(hdnServiceFacilityMedId.Value.ToString()))
                    {
                        DataSet dsProviderInformation = LoadProviderInformation(hdnServiceFacilityMedId.Value.ToString());
                        string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

                        if (entityTypeId == "1")
                        {
                            errServiceFacilityInfo.Text = "Individual provider cannot be entered as facility provider";
                            txtNPIServiceFacilityLocation.Text = "";
                            ClearField();
                            return;
                        }
                    }
                }

            }
            else
            {
                errServiceFacilityInfo.Text = "NPI is Unknown";
                txtNPIServiceFacilityLocation.Text = "";
                ClearField();
            }
        }
        else if (!string.IsNullOrEmpty(txtNPIServiceFacilityLocation.Text) && string.IsNullOrEmpty(txtMedicaidIdServiceFacilityLocation.Text))
        {
            errServiceFacilityInfo.Text = "NPI is Unknown";
            txtNPIServiceFacilityLocation.Text = "";
            ClearField();
        }
        if (!string.IsNullOrEmpty(errServiceFacilityInfo.Text))
        {
            txtNPIServiceFacilityLocation.Text = "";
            ClearField();
        }
    }    
    private void AssignValidationSummary(string validationSummary)
    {
        //rfvServiceFacilityNPI.ValidationGroup = validationSummary;
        //RegularExpressionValidator7.ValidationGroup = validationSummary;
    }
    public void SaveServiceFacilityLocation(string validationSummary, int action)
    {
        if (action == CON.ActionButtonType.Submit)
        {
            AssignValidationSummary(validationSummary);
            Page.Validate(validationSummary);
            if (Page.IsValid)
            {
                SavetoDB();
            }
        }
        else
        {
            SavetoDB();
        }

    }

    private void SavetoDB()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_ID", hdnClaimId.Value);
        DataSet dsServiceFacilityLocation = svc.SelectPanelsData("Claims_Service_Facility_Information", parms);
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            parms.Add("NPI", txtNPIServiceFacilityLocation.Text);
            parms.Add("Medicaid_ID", hdnServiceFacilityMedId.Value);
            parms.Add("Provider_Name", hdnServiceFacilityName.Value); // txtNameServiceFacilityLocation.Text);
            parms.Add("Address_Line1", hdnServiceFacilityAddress1.Value); // txtAddressServiceFacilityLocation.Text);
            parms.Add("Address_Line2", hdnServiceFacilityAddress2.Value); // txtAddressServiceFacilityLocation2.Text);
            parms.Add("City", hdnServiceFacilityCity.Value); // txtCityServiceFacilityLocation.Text);
            parms.Add("Service_State", hdnServiceFacilityState.Value); // txtStateServiceFacilityLocation.Text.Trim());
            parms.Add("Zip", hdnServiceFacilityZip.Value); // txtZipServiceFacilityLocation.Text);

            parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
            if (!string.IsNullOrEmpty(hdnClaimId.Value)
                && Helper.HasRows(dsServiceFacilityLocation) 
                && Convert.ToInt32(dsServiceFacilityLocation.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(ClaimId))
            {
                hdnServiceFacilityInfoId.Value = dsServiceFacilityLocation.Tables[0].Rows[0]["Claims_Service_Facility_Information_ID"].ToString();
                parms.Add("Claims_Service_Facility_Information_ID", hdnServiceFacilityInfoId.Value.ToString());
                svc.UpdatePanelsData("Claims_Service_Facility_Information", parms);
                GetServiceFacilityInformation();
            }
            else
            {
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                svc.InsertPanelsData("Claims_Service_Facility_Information", parms);
                GetServiceFacilityInformation();
            }
        }
    }

    public void GetServiceFacilityInformation()
    {
        if (!String.IsNullOrEmpty(hdnClaimId.Value))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_id", hdnClaimId.Value);
            DataSet dsServiceFacilityInfo = svc.SelectPanelsData("Claims_Service_Facility_Information", parms);
            if (Helper.HasRows(dsServiceFacilityInfo))
            {
                txtNPIServiceFacilityLocation.Text = dsServiceFacilityInfo.Tables[0].Rows[0]["NPI"].ToString();
                if (string.IsNullOrEmpty(txtNPIServiceFacilityLocation.Text))
                {
                    txtMedicaidIdServiceFacilityLocation.Text = dsServiceFacilityInfo.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                }
                hdnServiceFacilityMedId.Value = dsServiceFacilityInfo.Tables[0].Rows[0]["Medicaid_ID"].ToString();
                txtNameServiceFacilityLocation.Text = dsServiceFacilityInfo.Tables[0].Rows[0]["Provider_Name"].ToString();
                txtAddressServiceFacilityLocation.Text = dsServiceFacilityInfo.Tables[0].Rows[0]["Address_Line1"].ToString();
                txtAddressServiceFacilityLocation2.Text = dsServiceFacilityInfo.Tables[0].Rows[0]["Address_Line2"].ToString();
                txtStateServiceFacilityLocation.Text = dsServiceFacilityInfo.Tables[0].Rows[0]["Service_State"].ToString();
                txtZipServiceFacilityLocation.Text = dsServiceFacilityInfo.Tables[0].Rows[0]["Zip"].ToString();
                txtCityServiceFacilityLocation.Text = dsServiceFacilityInfo.Tables[0].Rows[0]["City"].ToString();
            }
        }
    }


    private DataTable GetData(string npi, string medicaidid, string lastName, string firstName)
    {
        DataTable dt = null;
        try
        {
            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                var ds = psc.SearchProviderNPI(npi, medicaidid, lastName, firstName);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                }
            }
        }
        catch (Exception)
        {
            return dt;
        }
        return dt;
    }

    public bool ValidProviderNPI(string npi)
    {
        try
        {
            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                return psc.VerifyProviderNPI(Convert.ToInt64(npi));
            }
        }
        catch (Exception)
        {
            return false;
        }
    }


    public void ClearServiceFailityInformation()
    {
        txtNPIServiceFacilityLocation.Text = "";
        ClearField();
    }

    private void SetReadOnlyFieldsControl(bool value)
    {
        txtNPIServiceFacilityLocation.Enabled = !value;
        Searchdv.Visible = !value;
        txtNPIServiceFacilityLocation.Visible = true;
    }
    public void ClearField()
    {
        txtMedicaidIdServiceFacilityLocation.Text = "";
        txtAddressServiceFacilityLocation.Text = "";
        txtAddressServiceFacilityLocation2.Text = "";
        txtNameServiceFacilityLocation.Text = "";
        txtCityServiceFacilityLocation.Text = "";
        txtStateServiceFacilityLocation.Text = "";
        txtZipServiceFacilityLocation.Text = "";
        lblServiceFacilityLocationAddress1.InnerText = "";
        lblServiceFacilityLocationAddress2.InnerText = "";
        lblServiceFacilityLocationCity.InnerText = "";
        lblServiceFacilityLocationState.InnerText = "";
        lblServiceFacilityLocationMedicaid.InnerText = "";
        lblServiceFacilityLocationName.InnerText = "";
        lblServiceFacilityLocationZip.InnerText = "";
        hdnClaimId.Value = "";
        hdnServiceFacilityAddress1.Value = "";
        hdnServiceFacilityAddress2.Value = "";
        hdnServiceFacilityCity.Value = "";
        hdnServiceFacilityMedId.Value = "";
        hdnServiceFacilityName.Value = "";
        hdnServiceFacilityState.Value = "";
        hdnServiceFacilityZip.Value = "";
    }







}
