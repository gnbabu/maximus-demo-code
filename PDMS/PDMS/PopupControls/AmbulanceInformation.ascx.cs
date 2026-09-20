using Corp.Core.Libraries.ServiceAgent;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_AmbulanceInformation : System.Web.UI.UserControl
{
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
    public string AmbulanceInformationPickUpAddressLine1
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtBoxPickupAddressLine1.Text))
                return txtBoxPickupAddressLine1.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtBoxPickupAddressLine1.Text = value.Trim();
        }
    }
    public string AmbulanceInformationPickUpAddressLine2
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtBoxPickupAddressLine2.Text))
                return txtBoxPickupAddressLine2.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtBoxPickupAddressLine2.Text = value.Trim();
        }
    }
    public string AmbulanceInformationPickUpCity
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtBoxPickUpCity.Text))
                return txtBoxPickUpCity.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtBoxPickUpCity.Text = value.Trim();
        }
    }
    public string AmbulanceInformationPickUpState
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlPickupState.SelectedValue))
                return ddlPickupState.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && ddlPickupState.Items.FindByValue(value) != null)
            {
                ddlPickupState.ClearSelection();
                ddlPickupState.SelectedValue = value.Trim();
            }
        }
    }
    public string AmbulanceInformationPickUpZip
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtBoxPickUpZip.Text))
                return txtBoxPickUpZip.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtBoxPickUpZip.Text = value.Trim();
        }
    }
    public string AmbulanceInformationPickUpStateText
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlPickupState.SelectedItem.Text))
                return ddlPickupState.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                ddlPickupState.SelectedItem.Text = value.Trim();
        }
    }

    public string AmbulanceInformationDropOffLocationName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtBoxDropOffLocationName.Text))
                return txtBoxDropOffLocationName.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtBoxDropOffLocationName.Text = value.Trim();
        }
    }
    public string AmbulanceInformationDropOffAddress1
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtBoxDropOffAddressLine1.Text))
                return txtBoxDropOffAddressLine1.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtBoxDropOffAddressLine1.Text = value.Trim();
        }
    }
    public string AmbulanceInformationDropOffAddress2
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtBoxDropOffAddressLine2.Text))
                return txtBoxDropOffAddressLine2.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtBoxDropOffAddressLine2.Text = value.Trim();
        }
    }
    public string AmbulanceInformationDropOffCity
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtBoxDropOffCity.Text))
                return txtBoxDropOffCity.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtBoxDropOffCity.Text = value.Trim();
        }
    }

    public string AmbulanceInformationDropOffStateText
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlDropOffState.SelectedItem.Text))
                return ddlDropOffState.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlDropOffState.ClearSelection();
                ddlDropOffState.SelectedItem.Text = value.Trim();
            }
        }
    }
    public string AmbulanceInformationDropOffState
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlDropOffState.SelectedValue))
                return ddlDropOffState.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlDropOffState.ClearSelection();
                ddlDropOffState.SelectedValue = value.Trim();
            }
        }
    }
    public string AmbulanceInformationDropOffZip
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtBoxDropOffZip.Text))
                return txtBoxDropOffZip.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtBoxDropOffZip.Text = value.Trim();
        }
    }
    public string AmbulanceInformationPatientWeight
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtPatientWeight.Text))
                return txtPatientWeight.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtPatientWeight.Text = value.Trim();
        }
    }
    public string AmbulanceInformationTransportationReasonCode
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlTransportReasonCode.SelectedItem.Text))
                return ddlTransportReasonCode.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                string lookupValue = lookupReasonCode(value);
                ddlTransportReasonCode.ClearSelection();
                if (ddlTransportReasonCode.Items.FindByValue(lookupValue) != null)
                {
                    ddlTransportReasonCode.SelectedValue = lookupValue.Trim();
                } else
                {
                    ddlTransportReasonCode.Items.Add(new ListItem(value, lookupValue));
                    ddlTransportReasonCode.SelectedValue = lookupValue.Trim();
                }
            }
        }
    }

    private string lookupReasonCode(string value)
    {
        string code = value;
        if (value == "A")
        {
            code = "1";
        }
        if (value == "B")
        {
            code = "2";
        }
        if (value == "C")
        {
            code = "3";
        }
        if (value == "D")
        {
            code = "4";
        }
        if (value == "E")
        {
            code = "5";
        }
        return code;
    }

    public string AmbulanceInformationTransportationReasonCodeText
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlTransportReasonCode.SelectedItem.Text))
                return ddlTransportReasonCode.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlTransportReasonCode.ClearSelection();
                ddlTransportReasonCode.SelectedValue = value.Trim();
            }

        }
    }
    public string AmbulanceInformationTransportationDistance
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtTransportDistance.Text))
                return txtTransportDistance.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtTransportDistance.Text = value.Trim();
        }
    }
    public string AmbulanceInformationRoundTripPurpose
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtRoundTripPurpose.Text))
                return txtRoundTripPurpose.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtRoundTripPurpose.Text = value.Trim();
        }
    }
    public string AmbulanceInformationConditionIndicator
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlConditionIndicator.SelectedValue))
                return ddlConditionIndicator.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlConditionIndicator.ClearSelection();
                ddlConditionIndicator.SelectedValue = value.Trim();
            }
        }
    }
    public string AmbulanceInformationConditionIndicatorText
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlConditionIndicator.SelectedItem.Text))
                return ddlConditionIndicator.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                ddlConditionIndicator.ClearSelection();
            ddlConditionIndicator.SelectedItem.Text = value.Trim();
        }
    }
    public string AmbulanceInformationStretecherPurpose
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtStretcherpurpose.Text))
                return txtStretcherpurpose.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtStretcherpurpose.Text = value.Trim();
        }
    }
    public string AmbulanceInformationConditionCode1
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlConditionCode1.SelectedValue))
                return ddlConditionCode1.SelectedItem.Text.Split('-')[0];
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                ddlConditionCode1.SelectedValue = value.Trim();
        }
    }
    public string AmbulanceInformationConditionCode1Text
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlConditionCode1.SelectedItem.Text))
                return ddlConditionCode1.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                ddlConditionCode1.SelectedItem.Text = value.Trim();
        }
    }
    public string AmbulanceInformationConditionCode2Text
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlConditionCode2.SelectedItem.Text))
                return ddlConditionCode2.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlConditionCode2.ClearSelection();
                ddlConditionCode2.SelectedItem.Text = value.Trim();
            }
        }
    }
    public string AmbulanceInformationConditionCode2
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlConditionCode2.SelectedValue))
                return ddlConditionCode2.SelectedItem.Text.Split('-')[0];
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlConditionCode2.ClearSelection();
                ddlConditionCode2.SelectedValue = value.Trim();
            }
        }
    }
    public string AmbulanceInformationConditionCode3
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlConditionCode3.SelectedValue))
                return ddlConditionCode3.SelectedItem.Text.Split('-')[0];
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlConditionCode3.ClearSelection();
                ddlConditionCode3.SelectedValue = value.Trim();
            }
        }
    }
    public string AmbulanceInformationConditionCode4
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlConditionCode4.SelectedValue))
                return ddlConditionCode4.SelectedItem.Text.Split('-')[0];
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlConditionCode4.ClearSelection();
                ddlConditionCode4.SelectedValue = value.Trim();
            }
        }
    }
    public string AmbulanceInformationConditionCode5
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlConditionCode5.SelectedValue))
                return ddlConditionCode5.SelectedItem.Text.Split('-')[0];
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlConditionCode5.ClearSelection();
                ddlConditionCode5.SelectedValue = value.Trim();
            }
        }
    }
    public string AmbulanceInformationConditionCode3Text
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlConditionCode3.SelectedItem.Text))
                return ddlConditionCode3.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlConditionCode3.ClearSelection();
                ddlConditionCode3.SelectedItem.Text = value.Trim();
            }
        }
    }
    public string AmbulanceInformationConditionCode4Text
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlConditionCode4.SelectedItem.Text))
                return ddlConditionCode4.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlConditionCode4.ClearSelection();
                ddlConditionCode4.SelectedItem.Text = value.Trim();
            }
        }
    }
    public string AmbulanceInformationConditionCode5Text
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlConditionCode2.SelectedItem.Text))
                return ddlConditionCode5.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlConditionCode5.ClearSelection();
                ddlConditionCode5.SelectedItem.Text = value.Trim();
            }
        }
    }
    private bool _displayReadOnly;
    public bool DisplayReadOnly
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value.ToString()))
                SetReadOnlyFieldsControl(value);
        }

    }

    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(txtBoxDropOffAddressLine1.Text) || !string.IsNullOrEmpty(txtBoxDropOffAddressLine2.Text) || !string.IsNullOrWhiteSpace(txtBoxDropOffCity.Text)
            || !string.IsNullOrWhiteSpace(txtBoxDropOffZip.Text) || !string.IsNullOrWhiteSpace(ddlDropOffState.SelectedValue) ||
            !string.IsNullOrEmpty(txtBoxPickupAddressLine1.Text) || !string.IsNullOrEmpty(txtBoxPickupAddressLine2.Text) || !string.IsNullOrWhiteSpace(txtBoxPickUpCity.Text)
            || !string.IsNullOrWhiteSpace(txtBoxPickUpZip.Text) || !string.IsNullOrWhiteSpace(ddlPickupState.SelectedValue) || !string.IsNullOrWhiteSpace(txtBoxDropOffLocationName.Text)
            || !string.IsNullOrWhiteSpace(txtPatientWeight.Text) || !string.IsNullOrWhiteSpace(ddlTransportReasonCode.SelectedValue) || !string.IsNullOrWhiteSpace(txtTransportDistance.Text)
            || !string.IsNullOrWhiteSpace(txtRoundTripPurpose.Text) || !string.IsNullOrWhiteSpace(ddlConditionIndicator.SelectedValue) || !string.IsNullOrWhiteSpace(txtStretcherpurpose.Text)
            || !string.IsNullOrWhiteSpace(ddlConditionCode1.SelectedValue))

        {
            rtn = true;
        }

        return rtn;
    }
    public bool HasAddressInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(txtBoxDropOffAddressLine1.Text) || !string.IsNullOrEmpty(txtBoxDropOffAddressLine2.Text) || !string.IsNullOrWhiteSpace(txtBoxDropOffCity.Text)
            || !string.IsNullOrWhiteSpace(txtBoxDropOffZip.Text))
        {
            rtn = true;
        }
        return rtn;
    }
    public string SaveButtonClientID
    {
        get { return (hdnSaveButtonClientID.Value); }
        set { hdnSaveButtonClientID.Value = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(value, true); }
    }
    [DefaultValue(false)]
    public bool isAddressVerified { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            try
            {
                txtBoxDropOffAddressLine1.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
                txtBoxDropOffAddressLine2.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
                txtBoxDropOffCity.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
                ddlDropOffState.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
                txtBoxDropOffZip.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
                BindDropDowns();
                BindGrid();
            }
            catch (Exception ex)
            {

            }
        }
        else
        {
            string addressValidationEnabledString = "0";

            if (this.Visible)
            {
                addressValidationEnabledString = AppSettings.Get("AddressValidationEnabled");
            }

            bool addressValidationEnabled = (addressValidationEnabledString == "1");
            cvAddress.Enabled = addressValidationEnabled;

            btnCancelAddressCorrection.OnClientClick = "$(\"#" + divConfirmAddress.ClientID + "\").dialog(\"close\"); "
                                                       + "return false;";
        }
        if (!string.IsNullOrEmpty(ddlConditionCode1.SelectedValue))
        {
            ddlConditionCode2.Enabled = true;

            if (!string.IsNullOrEmpty(ddlConditionCode2.SelectedValue))
            {
                ddlConditionCode3.Enabled = true;
            }
            if (!string.IsNullOrEmpty(ddlConditionCode3.SelectedValue))
            {
                ddlConditionCode4.Enabled = true;
            }
            if (!string.IsNullOrEmpty(ddlConditionCode4.SelectedValue))
            {
                ddlConditionCode5.Enabled = true;
            }
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
    protected override void OnLoad(EventArgs e)
    {
        txtBoxDropOffAddressLine1.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        txtBoxDropOffAddressLine2.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        txtBoxDropOffCity.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        ddlDropOffState.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        txtBoxDropOffZip.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        //hdnAddressConfirm.Value = "0";
        base.OnLoad(e);
    }
    public void BindGrid()
    {
        GetAmbulanceInformation();
    }
    private void BindDropDowns()
    {
        BindAmbulanceInformationStates();
        BindTansportationReasonCode();
        BindConditionCodes();
    }

    public void GetAmbulanceInformation()
    {
        if (!String.IsNullOrEmpty(hdnClaimId.Value))
        {
            //bool conditionIndicator = false;
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", hdnClaimId.Value);
            DataSet dsAmbulance = svc.SelectPanelsData("claims_ambulance_information", parms);
            LoadConditionCodesOnEdit();
            if (Helper.HasRows(dsAmbulance))
            {
                hdnAmbulanceInfoNo.Value = dsAmbulance.Tables[0].Rows[0]["Claims_Ambulance_Information_ID"].ToString();
                txtBoxPickupAddressLine1.Text = dsAmbulance.Tables[0].Rows[0]["Pick_Up_Address_Line1"].ToString();
                txtBoxPickupAddressLine2.Text = dsAmbulance.Tables[0].Rows[0]["Pick_Up_Address_Line2"].ToString();
                txtBoxPickUpCity.Text = dsAmbulance.Tables[0].Rows[0]["Pick_Up_City"].ToString();
                ddlPickupState.SelectedValue = dsAmbulance.Tables[0].Rows[0]["Pick_Up_State"].ToString();
                txtBoxPickUpZip.Text = dsAmbulance.Tables[0].Rows[0]["Pick_Up_ZIP"].ToString();
                txtBoxDropOffLocationName.Text = dsAmbulance.Tables[0].Rows[0]["Drop_Off_Location_Name"].ToString();
                txtBoxDropOffAddressLine1.Text = dsAmbulance.Tables[0].Rows[0]["Drop_Off_Address_Line1"].ToString();
                txtBoxDropOffAddressLine2.Text = dsAmbulance.Tables[0].Rows[0]["Drop_Off_Address_Line2"].ToString();
                txtBoxDropOffCity.Text = dsAmbulance.Tables[0].Rows[0]["Drop_Off_City"].ToString();
                txtBoxDropOffZip.Text = dsAmbulance.Tables[0].Rows[0]["Drop_Off_Zip"].ToString();
                ddlDropOffState.SelectedValue = dsAmbulance.Tables[0].Rows[0]["Drop_Off_State"].ToString();
                txtPatientWeight.Text = dsAmbulance.Tables[0].Rows[0]["Patient_Weight"].ToString();
                ddlTransportReasonCode.SelectedValue = dsAmbulance.Tables[0].Rows[0]["Transportation_Reason_Code"].ToString();
                txtTransportDistance.Text = dsAmbulance.Tables[0].Rows[0]["Transport_Distance"].ToString();
                txtRoundTripPurpose.Text = dsAmbulance.Tables[0].Rows[0]["Round_Trip_Purpose"].ToString();
                txtStretcherpurpose.Text = dsAmbulance.Tables[0].Rows[0]["Stretcher_Purpose"].ToString();
                ddlConditionIndicator.SelectedValue = dsAmbulance.Tables[0].Rows[0]["Condition_Indicator"].ToString();
                ddlConditionCode1.SelectedValue = dsAmbulance.Tables[0].Rows[0]["Condition_Code1"].ToString();
                ddlConditionCode2.SelectedValue = dsAmbulance.Tables[0].Rows[0]["Condition_Code2"].ToString();
                ddlConditionCode3.SelectedValue = dsAmbulance.Tables[0].Rows[0]["Condition_Code3"].ToString();
                ddlConditionCode4.SelectedValue = dsAmbulance.Tables[0].Rows[0]["Condition_Code4"].ToString();
                ddlConditionCode5.SelectedValue = dsAmbulance.Tables[0].Rows[0]["Condition_Code5"].ToString();

            }
        }
    }
    private void AssignValidationSummary(string validationGroup)
    {
        rfvPickUpAddressLine1.ValidationGroup = validationGroup;
        rfvPickUpCity.ValidationGroup = validationGroup;
        rfvPickUpCity.ValidationGroup = validationGroup;
        rfvPickUpState.ValidationGroup = validationGroup;
        rfvTxtDropOffAddressLine1.ValidationGroup = validationGroup;
        rfvDropOffcity.ValidationGroup = validationGroup;
        rfvDropState.ValidationGroup = validationGroup;
        rfvDropOffZip.ValidationGroup = validationGroup;
        rfvTransportDistance.ValidationGroup = validationGroup;
        rfvTransportCode.ValidationGroup = validationGroup;
        cvAddress.ValidationGroup = validationGroup;
    }
    public void SaveAmbulanceInformation(string validationGroup, int actionButton)
    {
        if (actionButton == CON.ActionButtonType.Submit)
        {
            AssignValidationSummary(validationGroup);
            Page.Validate(validationGroup);
            if (Page.IsValid)
            {

                if (isAddressVerified)
                {
                    if (ValidateAmbulanceInformation())
                    {
                        SaveToDb();
                    }

                }
            }
        }
        else
        {
            SaveToDb();
        }

    }

    private void SaveToDb()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();

        parms.Add("Pick_Up_Address_Line1", txtBoxPickupAddressLine1.Text);
        parms.Add("Pick_Up_Address_Line2", txtBoxPickupAddressLine2.Text);
        parms.Add("Pick_Up_City", txtBoxPickUpCity.Text);
        parms.Add("Pick_Up_State", ddlPickupState.SelectedValue);
        parms.Add("Pick_Up_ZIP", txtBoxPickUpZip.Text);
        parms.Add("Drop_Off_Location_Name", txtBoxDropOffLocationName.Text);
        parms.Add("Drop_Off_Address_Line1", txtBoxDropOffAddressLine1.Text);
        parms.Add("Drop_Off_Address_Line2", txtBoxDropOffAddressLine2.Text);
        parms.Add("Drop_Off_City", txtBoxDropOffCity.Text);
        parms.Add("Drop_Off_Zip", txtBoxDropOffZip.Text);
        parms.Add("Drop_Off_State", ddlDropOffState.SelectedValue);
        parms.Add("Patient_Weight", !string.IsNullOrEmpty(txtPatientWeight.Text) ? txtPatientWeight.Text : null );
        parms.Add("Transportation_Reason_Code", ddlTransportReasonCode.SelectedValue);
        parms.Add("Transport_Distance", txtTransportDistance.Text);
        parms.Add("Stretcher_Purpose", txtStretcherpurpose.Text);
        parms.Add("Condition_Indicator", ddlConditionIndicator.SelectedValue);
        parms.Add("Claim_ID", hdnClaimId.Value);
        parms.Add("Condition_Code1", ddlConditionCode1.SelectedValue);
        parms.Add("Condition_Code2", ddlConditionCode2.SelectedValue);
        parms.Add("Condition_Code3", ddlConditionCode3.SelectedValue);
        parms.Add("Condition_Code4", ddlConditionCode4.SelectedValue);
        parms.Add("Condition_Code5", ddlConditionCode5.SelectedValue);
        parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("Last_Modified_Date", DateTime.Now.ToString());
        parms.Add("Round_Trip_Purpose", txtRoundTripPurpose.Text);
        if (!string.IsNullOrEmpty(hdnClaimId.Value) && !string.IsNullOrEmpty(hdnAmbulanceInfoNo.Value))
        {

            parms.Add("Claims_Ambulance_Information_ID", hdnAmbulanceInfoNo.Value.ToString());
            try
            {
                svc.UpdatePanelsData("Claims_Ambulance_Information", parms);

            }
            catch (Exception ex)
            {

            }
            GetAmbulanceInformation();

        }
        else if (!string.IsNullOrWhiteSpace(hdnClaimId.Value))
        {
            parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("Created_Date_Time", DateTime.Now.ToString());
            try
            {
                svc.InsertPanelsData("Claims_Ambulance_Information", parms);

            }
            catch (Exception ex)
            {

            }
            GetAmbulanceInformation();

        }
        hdnAddressConfirm.Value = "0";

    }
    protected void cvAmbulanceServiceAddress_ServerValidate(object source, ServerValidateEventArgs args)
    {

        string newStreetAddress = string.Empty;
        string newUnitAddress = string.Empty;
        string floorDept = string.Empty;
        string newAddressLine3 = string.Empty;
        string newCity = string.Empty;
        string newState = string.Empty;
        string newCounty = string.Empty;
        string newZip5 = string.Empty;
        string newZip4 = string.Empty;

        string origUnitAddress = txtBoxDropOffAddressLine2.Text.ToUpper().Trim();
        string origStreetAddress = txtBoxDropOffAddressLine1.Text.ToUpper().Trim();
        string origCity = txtBoxDropOffCity.Text.ToUpper().Trim();
        string origState = ddlDropOffState.SelectedItem.Text.ToUpper().Trim();
        // string origCounty = CountyDisplay.ToUpper().Trim();
        string origZip5 = txtBoxDropOffZip.Text;
        string origZip4 = txtBoxDropOffZip.Text;

        int returnCode = 0;
        List<string> errorCodes;
        if (origStreetAddress.ToUpper().Contains("PO B") ||
           origStreetAddress.ToUpper().Contains("P.O. B") ||
           origStreetAddress.ToUpper().Contains("P.O B") ||
           origStreetAddress.ToUpper().Contains("PO. B") ||
           origStreetAddress.ToUpper().Contains("P O B") ||
           origStreetAddress.ToUpper().Contains("P O. B") ||
           origStreetAddress.Trim().ToUpper().StartsWith("BOX"))
        {



            if (!(origStreetAddress.ToUpper().Trim().StartsWith("PO B") ||
                 origStreetAddress.ToUpper().Trim().StartsWith("P.O. B") ||
                 origStreetAddress.ToUpper().Trim().StartsWith("P.O B") ||
                 origStreetAddress.ToUpper().Trim().StartsWith("PO. B") ||
                 origStreetAddress.ToUpper().Trim().StartsWith("P O B") ||
                 origStreetAddress.ToUpper().Trim().StartsWith("P O. B")))
            {
                args.IsValid = false;
                cvAddress.ErrorMessage = "If the address is a PO Box, it must start with 'P.O. Box'";
                return;
            }

            args.IsValid = true;
            return;
        }

        if ((origZip5.ToString() == ""))
        {
            //revTxtBoxDropZip.Enabled = true;
            cvAddress.ErrorMessage = "* Enter 5 digits ZIP)";
            args.IsValid = false;
            return;
        }

        AddressVerificatonDetail detail;
        IntelligentSearchAgent intSearch = new IntelligentSearchAgent();
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        try
        {

            int addressTypeId = 0;
            var addressVerificationRequest = new AddressVerificationRequest()
            {
                AddressLine = origStreetAddress + ", ",
                AddressLine2 = origUnitAddress,

                City = origCity,
                State = origState,
                PostalCode = origZip5
            };
            
            detail = intSearch.GetAddressVerificaton(addressVerificationRequest, addressTypeId, Helper.GetUserId(HttpContext.Current.User.Identity.Name), CON.AddressPage.AmbulanceInformation, 0);
           
            errorCodes = ErrorCodeStringToList(detail.ErrorCodes);

            if (!int.TryParse(detail.ReturnCodes, out returnCode))
            {
                throw new Exception(string.Format(
                    "Return code from address verification web service is invalid. Return code is: '{0}'",
                    detail.ReturnCodes));
            }
        }
        catch (Exception ex)
        {
            if (hdnAddressConfirm.Value == "1")
            {
                divWSError.Style["display"] = "none";
                args.IsValid = true;
            }
            else
            {
                HandleWSException(ex);
                args.IsValid = false;
            }

            return;
        }
        // If no error, check for ReturnText. This is a suggestion, but treat like an error
        if (returnCode == 1)
        {
            bool isCorrected = false;

            if (detail.AddressLine2 != "")
                newUnitAddress = detail.AddressLine2;
            if (detail.StreetName.Trim().ToUpper() == "PO BOX")
                newStreetAddress = detail.StreetName + " " + detail.StreetNumber;
            else
            {
                string preDirectional = (detail.PreDirectional == "") ? "" : detail.PreDirectional + " ";
                newStreetAddress = detail.StreetNumber + " " + preDirectional + detail.StreetName + " " +
                                   detail.StreetSuffix;
            }

            newCity = detail.City;
            newState = detail.State;
            newCounty = detail.County;

            string[] zipParts = detail.PostalCode.Split("-".ToCharArray());
            if (zipParts.Length == 2)
            {
                newZip5 = zipParts[0];
                newZip4 = zipParts[0];
            }
            else if (zipParts.Length == 1)
            {
                newZip5 = zipParts[0];
                newZip4 = "";
            }
            else
            {
                newZip5 = "";
                newZip4 = "";
            }
            if (origUnitAddress.ToUpper() == "") { origUnitAddress = " "; }
            if (origUnitAddress.ToUpper() != newUnitAddress.ToUpper())
                isCorrected = true;

            if (origStreetAddress.ToUpper() == "") { origStreetAddress = " "; }
            if (origStreetAddress.ToUpper() != newStreetAddress.ToUpper())
                isCorrected = true;

            if (origCity.ToUpper() == "") { origCity = " "; }
            if (origCity.ToUpper() != newCity.ToUpper())
                isCorrected = true;

            if (origState.ToUpper() == "") { origState = " "; }
            if (origState.ToUpper() != newState.ToUpper())
                isCorrected = true;

            //if (origCounty.ToUpper() != newCounty.ToUpper())
            //    isCorrected = true;


            if (origZip5.ToUpper() != newZip5.ToUpper())
                isCorrected = true;

            if (origZip4.ToUpper() != newZip4.ToUpper())
                isCorrected = true;

            if (1 != 0)
                cvAddress.ErrorMessage = "";

            // If the user already confirmed the usps correction or no correction was needed
            if (hdnAddressConfirm.Value == "1" || isCorrected == false)
            {
                isAddressVerified = true;
                divConfirmAddress.Style["display"] = "none";
                args.IsValid = true;
            }
            else // the user has not confirmed changes and a correction was needed. Show the changes in a jquery-ui dialog.
            {
                this.paraUSPSAddress.InnerHtml = "";

                //this.paraUSPSAddress.InnerHtml += "<br>";
                if (newStreetAddress != "")
                    this.paraUSPSAddress.InnerHtml += newStreetAddress + "<br>";
                if (txtBoxDropOffAddressLine2.Text != "")
                    this.paraUSPSAddress.InnerHtml += newUnitAddress + "<br>";
                //if (newCounty != "")
                //    this.paraUSPSAddress.InnerHtml += newCounty + "<br>";
                this.paraUSPSAddress.InnerHtml += newCity + ", "
                                                          + newState + " "
                                                          + newZip5 + "-"
                                                           + "<br>";
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "showConfirm",
                    "$(function() {var $divConfirm = $( \"#" + divConfirmAddress.ClientID + "\" );"
                    + "setTimeout(function() {"
                    + "$divConfirm.dialog({ modal: true, position: { my: \"left center\", at: \"right center\", of: \"#mainDiv\" } }); "
                    + "$divConfirm.css(\"display\", \"block\"); "
                    + "$divConfirm.dialog(\"widget\").css(\"z-index\", (maxZIndex() + 10)); "
                    + "}, 100); "
                    + "$(\"#" + SaveButtonClientID + "\").prop(\"disabled\", true); }); ", true);
                args.IsValid = false;

                btnConfirmAddress.OnClientClick = "$(\"#" + hdnAddressConfirm.ClientID + "\").val(\"1\"); "
                                                   + "$(\"#" + divConfirmAddress.ClientID + "\").dialog(\"close\"); "
                                                   + "$(\"#" + SaveButtonClientID + "\").prop(\"disabled\", false); "
                                                   + "sendDataAmbulanceInformation('" + newStreetAddress + "', '"
                                                   + newUnitAddress + "', '"
                                                   + floorDept + "', '"
                                                   + newAddressLine3 + "', '"
                                                   + newCity + "', '"
                                                   + newState + "', '"
                                                   + newCounty + "', '"
                                                   + newZip5 + "', '"
                                                   + newZip4 + "'); "
                                                   + "return false;";
            }
        }
        else if (returnCode > 1 || (returnCode < 0 && errorCodes.Contains("11")))
        {
            divConfirmAddress.Style["display"] = "none";
            cvAddress.ErrorMessage =
                string.Format(
                    "Multiple possible addresses, but no exact match made. Number of possible addresses is {0}",
                    Math.Abs(returnCode));
            args.IsValid = false;
        }
        else if (returnCode == -99 || (returnCode == -1 && errorCodes.Contains("07")))
        {
            divConfirmAddress.Style["display"] = "none";
            cvAddress.ErrorMessage =
                string.Format(
                    "Address validation failed. Could not find a valid destination for a mailing or package.");
            args.IsValid = false;
        }
        else if (returnCode == -3 && errorCodes.Contains("05"))
        {
            divConfirmAddress.Style["display"] = "none";
            cvAddress.ErrorMessage = string.Format("Street name normalized, but no matching address was found.");
            args.IsValid = false;
        }
        else
        {
            divConfirmAddress.Style["display"] = "none";
            cvAddress.ErrorMessage = "Address not found. Details:";
            cvAddress.ErrorMessage += ErrorCodesToHTML(errorCodes);
            args.IsValid = false;
        }
    }


    private List<string> ErrorCodeStringToList(string errorCodesString)
    {
        List<string> errorCodesList = new List<string>();
        string errorCode = "";
        char[] errorCodeCharArray = errorCodesString.ToCharArray();

        foreach (char c in errorCodeCharArray)
        {
            errorCode += c;

            if (errorCode.Length == 2)
            {
                errorCodesList.Add(errorCode);
                errorCode = string.Empty;
            }
        }

        return errorCodesList;
    }

    private string ErrorCodesToHTML(List<string> errorCodes)
    {
        string html = "<ul>";
        string errorString = string.Empty;

        ResourceManager rm = Resources.AddressValidation.ResourceManager;

        foreach (string errorCode in errorCodes)
        {
            errorString = "ADDR_ERROR_CODE_" + errorCode;
            html += "<li>" + errorCode.ToString() + ": " + rm.GetString(errorString) + "</li>";
        }

        html += "</ul>";

        return html;
    }

    protected void HandleWSException(Exception ex)
    {
        this.paraWSError.InnerHtml = ex.Message;
        if (ex.InnerException != null)
        {
            this.paraWSError.InnerHtml += "<br /><br />Details: " + ex.InnerException.Message;
        }

        ScriptManager.RegisterStartupScript(Page, this.GetType(), "showConfirm",
            "$(function() {var $divConfirm = $( \"#" + divWSError.ClientID + "\" );"
            + "setTimeout(function() {"
            + "$divConfirm.dialog({ modal: true, position: { my: \"left center\", at: \"right center\", of: \"#mainDiv\" } }); "
            + "$divConfirm.css(\"display\", \"block\"); "
            + "$divConfirm.dialog(\"widget\").css(\"z-index\", (maxZIndex() + 10)); "
            + "}, 100); "
            + "$(\"#" + SaveButtonClientID + "\").prop(\"disabled\", true); }); ", true);

        btnConfirmWSError.OnClientClick = "$(\"#" + hdnAddressConfirm.ClientID + "\").val(\"1\"); "
                                          + "$(\"#" + divWSError.ClientID + "\").dialog(\"close\"); "
                                          + "$(\"#" + SaveButtonClientID + "\").prop(\"disabled\", false); "
                                          + "return false;";

    }

    private void BindAmbulanceInformationStates()
    {
        Helper.LoadDropDownListWithStates(ref ddlPickupState, true);
        Helper.LoadDropDownListWithStates(ref ddlDropOffState, true);
    }
    private void BindTansportationReasonCode()
    {
        DataSet ds = DataAccess.ExecuteStoredProcedure("usp_Select_Claims_Transportation_Code");
        if (Helper.HasRows(ds))
        {
            Helper.LoadList(ddlTransportReasonCode, ds.Tables[0], "Claims_Transportation_Code_Description", "Claims_Transportation_Code_ID", true);
        }
    }

    private void BindConditionCodes()
    {
        DataSet ds = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Ambulance_Condition_Code");
        if (Helper.HasRows(ds))
        {
            Helper.LoadList(ddlConditionCode1, ds.Tables[0], "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
        }
    }
    protected void ddlConditionCode1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlConditionCode1.SelectedValue != "")
        {
            
            if (ddlConditionCode1.SelectedValue != "" && ddlConditionCode2.SelectedValue != "" && ddlConditionCode3.SelectedValue != "" && ddlConditionCode4.SelectedValue != "" && ddlConditionCode4.SelectedValue != "")
            {
                loadConditionCodeDetails();
            }
            else if (ddlConditionCode1.SelectedValue != "" || ddlConditionCode2.SelectedValue != "" || ddlConditionCode3.SelectedValue != "" || ddlConditionCode4.SelectedValue != "" || ddlConditionCode4.SelectedValue != "")
            {
                Helper.LoadList(ddlConditionCode2, LoadConditionCode2(), "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);                
            }
        }

    }
    protected void ddlConditionCode2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlConditionCode2.SelectedValue != "")
        {

            if (ddlConditionCode1.SelectedValue != "" && ddlConditionCode2.SelectedValue != "" && ddlConditionCode3.SelectedValue != "" && ddlConditionCode4.SelectedValue != "" && ddlConditionCode4.SelectedValue != "")
            {
                loadConditionCodeDetails();
            }
            else if (ddlConditionCode1.SelectedValue != "" || ddlConditionCode2.SelectedValue != "" || ddlConditionCode3.SelectedValue != "" || ddlConditionCode4.SelectedValue != "" || ddlConditionCode4.SelectedValue != "")
            {
                Helper.LoadList(ddlConditionCode3, LoadConditionCode3(), "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
            }
        }

    }
    protected void ddlConditionCode3_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlConditionCode3.SelectedValue != "")
        {
            if (ddlConditionCode1.SelectedValue != "" && ddlConditionCode2.SelectedValue != "" && ddlConditionCode3.SelectedValue != "" && ddlConditionCode4.SelectedValue != "" && ddlConditionCode4.SelectedValue != "")
            {
                loadConditionCodeDetails();
            }
            else if (ddlConditionCode1.SelectedValue != "" || ddlConditionCode2.SelectedValue != "" || ddlConditionCode3.SelectedValue != "" || ddlConditionCode4.SelectedValue != "" || ddlConditionCode4.SelectedValue != "")
            {
                Helper.LoadList(ddlConditionCode4, LoadConditionCode4(), "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
            }
        }

    }
    protected void ddlConditionCode4_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlConditionCode4.SelectedValue != "")
        {
            if (ddlConditionCode1.SelectedValue != "" && ddlConditionCode2.SelectedValue != "" && ddlConditionCode3.SelectedValue != "" && ddlConditionCode4.SelectedValue != "" && ddlConditionCode5.SelectedValue != "")
            {
                loadConditionCodeDetails();
            }
            else if (ddlConditionCode1.SelectedValue != "" || ddlConditionCode2.SelectedValue != "" || ddlConditionCode3.SelectedValue != "" || ddlConditionCode4.SelectedValue != "" || ddlConditionCode4.SelectedValue != "")
            {
                Helper.LoadList(ddlConditionCode5, LoadConditionCode5(), "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
            }
        }

    }
    public DataTable LoadConditionCode2()
    {

        DataSet dsConditionCode = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Ambulance_Condition_Code");
        DataTable dt = new DataTable();
        DataTable selectedTable = new DataTable();
        string wherecode = string.Empty;
        int code1 = 0;
        int code2 = 0;
        int code3 = 0;
        int code4 = 0;
        int code5 = 0;
        if (!string.IsNullOrEmpty(ddlConditionCode1.SelectedValue))
        {
            code1 = Convert.ToInt32(ddlConditionCode1.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode2.SelectedValue))
        {
            code2 = Convert.ToInt32(ddlConditionCode2.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode3.SelectedValue))
        {
            code3 = Convert.ToInt32(ddlConditionCode3.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode4.SelectedValue))
        {
            code4 = Convert.ToInt32(ddlConditionCode4.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode5.SelectedValue))
        {
            code5 = Convert.ToInt32(ddlConditionCode5.SelectedValue);
        }
        if (Helper.HasRows(dsConditionCode))
        {
            dt = dsConditionCode.Tables[0];
            if (code1 != 0 && code2 != 0 && code3 != 0 && code4 != 0 && code5 != 0)
            {
                selectedTable = dt.AsEnumerable()
                                                    .Where(r => r.Field<int>("Claims_Condition_Code_ID") != code1 &&
                                                  r.Field<int>("Claims_Condition_Code_ID") != code3 &&
                                                  r.Field<int>("Claims_Condition_Code_ID") != code4 &&
                                                  r.Field<int>("Claims_Condition_Code_ID") != code5)
                                                    .CopyToDataTable();
            }
            else if (code1 != 0 || code2 != 0 || code3 != 0 || code4 != 0 || code5 != 0)
            {
                selectedTable = dt.AsEnumerable()
                                    .Where(r => r.Field<int>("Claims_Condition_Code_ID") != code1 &&
                                  r.Field<int>("Claims_Condition_Code_ID") != code2 &&
                                  r.Field<int>("Claims_Condition_Code_ID") != code3 &&
                                  r.Field<int>("Claims_Condition_Code_ID") != code4 &&
                                  r.Field<int>("Claims_Condition_Code_ID") != code5)
                                    .CopyToDataTable();
            }

        }
        return selectedTable;
    }
    public DataTable LoadConditionCode3()
    {
        DataSet dsConditionCode = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Ambulance_Condition_Code");
        DataTable dt = new DataTable();
        DataTable selectedTable = new DataTable();
        int code1 = 0;
        int code2 = 0;
        int code3 = 0;
        int code4 = 0;
        int code5 = 0;
        if (!string.IsNullOrEmpty(ddlConditionCode1.SelectedValue))
        {
            code1 = Convert.ToInt32(ddlConditionCode1.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode2.SelectedValue))
        {
            code2 = Convert.ToInt32(ddlConditionCode2.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode3.SelectedValue))
        {
            code3 = Convert.ToInt32(ddlConditionCode3.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode4.SelectedValue))
        {
            code4 = Convert.ToInt32(ddlConditionCode4.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode5.SelectedValue))
        {
            code5 = Convert.ToInt32(ddlConditionCode5.SelectedValue);
        }
        if (Helper.HasRows(dsConditionCode))
        {
            dt = dsConditionCode.Tables[0];

            if (code1 != 0 && code2 != 0 && code3 != 0 && code4 != 0 && code5 != 0)
            {
                selectedTable = dt.AsEnumerable()
                                .Where(r => r.Field<int>("Claims_Condition_Code_ID") != code1 &&
                              r.Field<int>("Claims_Condition_Code_ID") != code2 &&
                              r.Field<int>("Claims_Condition_Code_ID") != code4 &&
                              r.Field<int>("Claims_Condition_Code_ID") != code5)
                                .CopyToDataTable();
            }
            else if (code1 != 0 || code2 != 0 || code3 != 0 || code4 != 0 || code5 != 0)
            {
                selectedTable = dt.AsEnumerable()
                                .Where(r => r.Field<int>("Claims_Condition_Code_ID") != code1 &&
                              r.Field<int>("Claims_Condition_Code_ID") != code2 &&
                              r.Field<int>("Claims_Condition_Code_ID") != code3 &&
                              r.Field<int>("Claims_Condition_Code_ID") != code4 &&
                              r.Field<int>("Claims_Condition_Code_ID") != code5)
                                .CopyToDataTable();
            }
        }
        return selectedTable;


    }
    public DataTable LoadConditionCode4()
    {
        DataSet dsConditionCode = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Ambulance_Condition_Code");
        DataTable dt = new DataTable();
        DataTable selectedTable = new DataTable();
        int code1 = 0;
        int code2 = 0;
        int code3 = 0;
        int code4 = 0;
        int code5 = 0;
        if (!string.IsNullOrEmpty(ddlConditionCode1.SelectedValue))
        {
            code1 = Convert.ToInt32(ddlConditionCode1.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode2.SelectedValue))
        {
            code2 = Convert.ToInt32(ddlConditionCode2.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode3.SelectedValue))
        {
            code3 = Convert.ToInt32(ddlConditionCode3.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode4.SelectedValue))
        {
            code4 = Convert.ToInt32(ddlConditionCode4.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode5.SelectedValue))
        {
            code5 = Convert.ToInt32(ddlConditionCode5.SelectedValue);
        }
        if (Helper.HasRows(dsConditionCode))
        {
            dt = dsConditionCode.Tables[0];
            if (code1 != 0 && code2 != 0 && code3 != 0 && code4 != 0 && code5 != 0)
            {
                selectedTable = dt.AsEnumerable()
                           .Where(r => r.Field<int>("Claims_Condition_Code_ID") != code1 &&
                         r.Field<int>("Claims_Condition_Code_ID") != code2 &&
                         r.Field<int>("Claims_Condition_Code_ID") != code3 &&
                         r.Field<int>("Claims_Condition_Code_ID") != code5)
                           .CopyToDataTable();

            }
            else if (code1 != 0 || code2 != 0 || code3 != 0 || code4 != 0 || code5 != 0)
            {
                selectedTable = dt.AsEnumerable()
                           .Where(r => r.Field<int>("Claims_Condition_Code_ID") != code1 &&
                         r.Field<int>("Claims_Condition_Code_ID") != code2 &&
                         r.Field<int>("Claims_Condition_Code_ID") != code3 &&
                         r.Field<int>("Claims_Condition_Code_ID") != code4 &&
                         r.Field<int>("Claims_Condition_Code_ID") != code5)
                           .CopyToDataTable();
            }

        }

        return selectedTable;


    }
    public DataTable LoadConditionCode5()
    {
        DataSet dsConditionCode = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Ambulance_Condition_Code");
        DataTable dt = new DataTable();
        DataTable selectedTable = new DataTable();
        int code1 = 0;
        int code2 = 0;
        int code3 = 0;
        int code4 = 0;
        int code5 = 0;
        if (!string.IsNullOrEmpty(ddlConditionCode1.SelectedValue))
        {
            code1 = Convert.ToInt32(ddlConditionCode1.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode2.SelectedValue))
        {
            code2 = Convert.ToInt32(ddlConditionCode2.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode3.SelectedValue))
        {
            code3 = Convert.ToInt32(ddlConditionCode3.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode4.SelectedValue))
        {
            code4 = Convert.ToInt32(ddlConditionCode4.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlConditionCode5.SelectedValue))
        {
            code5 = Convert.ToInt32(ddlConditionCode5.SelectedValue);
        }

        if (Helper.HasRows(dsConditionCode))
        {
            dt = dsConditionCode.Tables[0];
            if (code1 != 0 && code2 != 0 && code3 != 0 && code4 != 0 && code5 != 0)
            {
                selectedTable = dt.AsEnumerable()
                           .Where(r => r.Field<int>("Claims_Condition_Code_ID") != code1 &&
                         r.Field<int>("Claims_Condition_Code_ID") != code2 &&
                         r.Field<int>("Claims_Condition_Code_ID") != code3 &&
                         r.Field<int>("Claims_Condition_Code_ID") != code4
                         )
                           .CopyToDataTable();
            }
            else if (code1 != 0 || code2 != 0 || code3 != 0 || code4 != 0 || code5 != 0)
            {
                selectedTable = dt.AsEnumerable()
                           .Where(r => r.Field<int>("Claims_Condition_Code_ID") != code1 &&
                         r.Field<int>("Claims_Condition_Code_ID") != code2 &&
                         r.Field<int>("Claims_Condition_Code_ID") != code3 &&
                         r.Field<int>("Claims_Condition_Code_ID") != code4 &&
                         r.Field<int>("Claims_Condition_Code_ID") != code5)
                           .CopyToDataTable();
            }

        }

        return selectedTable;

    }

    private void LoadConditionCodesOnEdit()
    {
        DataSet dsConditionCode = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Ambulance_Condition_Code");
        if (Helper.HasRows(dsConditionCode))
        {
            Helper.LoadList(ddlConditionCode1, dsConditionCode.Tables[0], "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
            Helper.LoadList(ddlConditionCode2, dsConditionCode.Tables[0], "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
            Helper.LoadList(ddlConditionCode3, dsConditionCode.Tables[0], "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
            Helper.LoadList(ddlConditionCode4, dsConditionCode.Tables[0], "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
            Helper.LoadList(ddlConditionCode5, dsConditionCode.Tables[0], "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
        }
    }
    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valAmbulanceInformation";
        this.Page.Validators.Add(val);
        return false;
    }
    private bool ValidateAmbulanceInformation()
    {
        bool isValid = true;
        if (ddlConditionCode1.SelectedIndex > -1 && ddlConditionIndicator.SelectedIndex <= -1)
        {
            AddValidationErrorMessage(" *Condition indicator is required");
            isValid = false;
        }
        if (ddlConditionIndicator.SelectedItem.Text == "Yes" && (String.IsNullOrWhiteSpace(ddlConditionCode1.SelectedItem.Text)))
        {
            AddValidationErrorMessage("*At least one condition code is required");
            isValid = false;
        }
        return isValid;
    }

    public void AmbulanceInformationClearAllFields()
    {
        txtBoxDropOffAddressLine1.Text = "";
        txtBoxDropOffAddressLine2.Text = "";
        txtBoxDropOffCity.Text = "";
        txtBoxDropOffZip.Text = "";
        ddlDropOffState.ClearSelection();
        ddlPickupState.ClearSelection();
        txtBoxPickupAddressLine1.Text = "";
        txtBoxPickupAddressLine2.Text = "";
        txtBoxPickUpCity.Text = "";
        txtBoxPickUpZip.Text = "";
        txtBoxDropOffLocationName.Text = "";
        txtPatientWeight.Text = "";
        txtRoundTripPurpose.Text = "";
        txtStretcherpurpose.Text = "";
        txtTransportDistance.Text = "";
        ddlConditionIndicator.ClearSelection();
        ddlConditionCode1.ClearSelection();
        ddlConditionCode2.ClearSelection();
        ddlConditionCode3.ClearSelection();
        ddlConditionCode4.ClearSelection();
        ddlConditionCode5.ClearSelection();
        ddlTransportReasonCode.ClearSelection();

    }
    private void SetReadOnlyFieldsControl(bool value)
    {
        txtBoxDropOffAddressLine1.ReadOnly = value;
        txtBoxDropOffAddressLine2.ReadOnly = value;
        txtBoxDropOffCity.ReadOnly = value;
        txtBoxDropOffZip.ReadOnly = value;
        txtBoxPickupAddressLine1.ReadOnly = value;
        txtBoxPickupAddressLine2.ReadOnly = value;
        txtBoxPickUpCity.ReadOnly = value;
        txtBoxPickUpZip.ReadOnly = value;
        txtBoxDropOffLocationName.ReadOnly = value;
        txtPatientWeight.ReadOnly = value;
        txtRoundTripPurpose.ReadOnly = value;
        txtStretcherpurpose.ReadOnly = value;
        txtTransportDistance.ReadOnly = value;
        ddlConditionCode1.Enabled = !value;
        ddlConditionCode2.Enabled = !value;
        ddlConditionCode3.Enabled = !value;
        ddlConditionCode4.Enabled = !value;
        ddlConditionCode5.Enabled = !value;
        ddlConditionIndicator.Enabled = !value;
        ddlTransportReasonCode.Enabled = !value;
        ddlDropOffState.Enabled = !value;
        ddlPickupState.Enabled = !value;



    }

    protected void ddlConditionCode5_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlConditionCode5.SelectedValue != "")
        {
            if (ddlConditionCode1.SelectedValue != "" && ddlConditionCode2.SelectedValue != "" && ddlConditionCode3.SelectedValue != "" && ddlConditionCode4.SelectedValue != "" && ddlConditionCode4.SelectedValue != "")
            {
                loadConditionCodeDetails();
            }
            else if (ddlConditionCode1.SelectedValue != "" || ddlConditionCode2.SelectedValue != "" || ddlConditionCode3.SelectedValue != "" || ddlConditionCode4.SelectedValue != "" || ddlConditionCode4.SelectedValue != "")
            {
                Helper.LoadList(ddlConditionCode5, LoadConditionCode5(), "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
            }
        }

    }
    public DataTable LoadConditionCode1()
    {

        DataSet dsConditionCode = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Ambulance_Condition_Code");
        DataTable dt = new DataTable();
        DataTable selectedTable = new DataTable();
        if (Helper.HasRows(dsConditionCode))
        {
            dt = dsConditionCode.Tables[0];

            string wherecode = string.Empty;
            int code1 = 0;
            int code2 = 0;
            int code3 = 0;
            int code4 = 0;
            int code5 = 0;
            if (!string.IsNullOrEmpty(ddlConditionCode1.SelectedValue))
            {
                code1 = Convert.ToInt32(ddlConditionCode1.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlConditionCode2.SelectedValue))
            {
                code2 = Convert.ToInt32(ddlConditionCode2.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlConditionCode3.SelectedValue))
            {
                code3 = Convert.ToInt32(ddlConditionCode3.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlConditionCode4.SelectedValue))
            {
                code4 = Convert.ToInt32(ddlConditionCode4.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlConditionCode5.SelectedValue))
            {
                code5 = Convert.ToInt32(ddlConditionCode5.SelectedValue);
            }
            if (code1 != 0 && code2 != 0 && code3 != 0 && code4 != 0 && code5 != 0)
            {
                selectedTable = dt.AsEnumerable()
                                                    .Where(r => r.Field<int>("Claims_Condition_Code_ID") != code2 &&
                                                  r.Field<int>("Claims_Condition_Code_ID") != code3 &&
                                                  r.Field<int>("Claims_Condition_Code_ID") != code4 &&
                                                  r.Field<int>("Claims_Condition_Code_ID") != code5)
                                                    .CopyToDataTable();
            }
            else if (code1 != 0 || code2 != 0 || code3 != 0 || code4 != 0 || code5 != 0)
            {
                selectedTable = dt.AsEnumerable()
                                    .Where(r => r.Field<int>("Claims_Condition_Code_ID") != code1 &&
                                  r.Field<int>("Claims_Condition_Code_ID") != code2 &&
                                  r.Field<int>("Claims_Condition_Code_ID") != code3 &&
                                  r.Field<int>("Claims_Condition_Code_ID") != code4 &&
                                  r.Field<int>("Claims_Condition_Code_ID") != code5)
                                    .CopyToDataTable();
            }
        }
        return selectedTable;
    }
    private void loadConditionCodeDetails()
    {
        int originalCode1 = 0;
        if (ddlConditionCode1.SelectedValue != "")
        {
            originalCode1 = Convert.ToInt32(ddlConditionCode1.SelectedValue);

        }
        int originalCode2 = 0;
        if (ddlConditionCode2.SelectedValue != "")
        {

            originalCode2 = Convert.ToInt32(ddlConditionCode2.SelectedValue);

        }
        int originalCode3 = 0;
        if (ddlConditionCode3.SelectedValue != "")
        {

            originalCode3 = Convert.ToInt32(ddlConditionCode3.SelectedValue);

        }
        int originalCode4 = 0;
        if (ddlConditionCode4.SelectedValue != "")
        {
            originalCode4 = Convert.ToInt32(ddlConditionCode4.SelectedValue);
        }
        int originalCode5 = 0;
        if (ddlConditionCode5.SelectedValue != "")
        {
            originalCode5 = Convert.ToInt32(ddlConditionCode5.SelectedValue);
        }

        DataSet ds = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Ambulance_Condition_Code");
        if (Helper.HasRows(ds))
        {
            Helper.LoadList(ddlConditionCode1, LoadConditionCode1(), "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
        }
        if (originalCode1 != 0)
        {
            ddlConditionCode1.SelectedValue = Convert.ToString(originalCode1);
        }
        Helper.LoadList(ddlConditionCode2, LoadConditionCode2(), "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
        if (originalCode2 != 0)
        {
            ddlConditionCode2.SelectedValue = Convert.ToString(originalCode2);
        }
        Helper.LoadList(ddlConditionCode3, LoadConditionCode3(), "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
        if (originalCode3 != 0)
        {
            ddlConditionCode3.SelectedValue = Convert.ToString(originalCode3);
        }
        Helper.LoadList(ddlConditionCode4, LoadConditionCode4(), "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
        if (originalCode4 != 0)
        {
            ddlConditionCode4.SelectedValue = Convert.ToString(originalCode4);
        }
        Helper.LoadList(ddlConditionCode5, LoadConditionCode5(), "Claims_Condition_Code_Description", "Claims_Condition_Code_ID", true);
        if (originalCode5 != 0)
        {
            ddlConditionCode5.SelectedValue = Convert.ToString(originalCode5);
        }
    }
}
