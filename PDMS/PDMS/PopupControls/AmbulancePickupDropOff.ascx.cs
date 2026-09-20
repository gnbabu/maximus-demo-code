using Corp.Core.Libraries;
using Corp.Core.Libraries.ServiceAgent;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_AmbulancePickupDropOff : System.Web.UI.UserControl
{

    private string pickUpAddressLine1 = string.Empty;
    private string pickUpAddressLine2 = string.Empty;
    private string pickUpCity = string.Empty;
    private string pickUpState = string.Empty;
    private string pickUpZip = string.Empty;
    private string dropUpAddressLine1 = string.Empty;
    private string dropUpAddressLine2 = string.Empty;
    private string dropUpCity = string.Empty;
    private string dropUpState = string.Empty;
    private string dropUpZip = string.Empty;
    public string AmbulanceInformationPickUpAddressLine1
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(pickUpAddressLine1))
                return pickUpAddressLine1;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                pickUpAddressLine1 = value.Trim();
        }
    }
    public string AmbulanceInformationDropOffAddressLine1
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(dropUpAddressLine1))
                return dropUpAddressLine1;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                dropUpAddressLine1 = value.Trim();
        }
    }
    public string AmbulanceInformationPickUpAddressLine2
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(pickUpAddressLine2))
                return pickUpAddressLine2;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                pickUpAddressLine2 = value.Trim();
        }
    }
    public string AmbulanceInformationDropAddressLine2
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(dropUpAddressLine2))
                return dropUpAddressLine2;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                dropUpAddressLine2 = value.Trim();
        }
    }
    public string AmbulanceInformationPickUpCity
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(pickUpCity))
                return pickUpCity;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                pickUpCity = value.Trim();
        }
    }
    public string AmbulanceInformationDropOffCity
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(dropUpCity))
                return dropUpCity;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                dropUpCity = value.Trim();
        }
    }
    public string AmbulanceInformationPickUpState
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(pickUpState))
                return pickUpState;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                pickUpState = value.Trim();
        }
    }
    public string AmbulanceInformationDropState
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(dropUpState))
                return dropUpState;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                dropUpState = value.Trim();
        }
    }
    public string AmbulanceInformationPickUpZip
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(pickUpZip))
                return pickUpZip;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                pickUpZip = value.Trim();
        }
    }
    public string AmbulanceInformationDropOffZip
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(dropUpZip))
                return dropUpZip;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                dropUpZip = value.Trim();
        }
    }
    [DefaultValue(false)]
    public bool HasAmbulanceHasValue { get; set; }
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
    private ClaimsServiceAgent ClaimService = null;
    private DataSet dataSetAmbulancePickUpDropOffInformation = new DataSet();

    public DataSet dataSetAmbulancePickUpDropOff
    {
        get
        {
            if (!Helper.HasRows(dataSetAmbulancePickUpDropOffInformation))
            {
                if (Helper.HasRows(GetAmbulancePickUpDropOffInformation()))
                {
                    return dataSetAmbulancePickUpDropOffInformation;
                }
            }
            return dataSetAmbulancePickUpDropOffInformation;
        }
        set
        {
            if (value != null)
            {
                LoadAmbulanceGridInformation(null);
            }
        }
    }
    private bool _displayReadOnly;

    public bool DisplayReadOnly
    {
        get
        {
            return _displayReadOnly;
        }
        set
        {
            _displayReadOnly = value;
            BindGrid();
        }
    }
    public string ICNNumber = string.Empty;
    public string ICN
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ICNNumber))
                return ICNNumber;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                ICNNumber = value.Trim();
        }
    }
    public string SaveButtonClientID
    {
        get { return (hdnSaveButtonClientID.Value); }
        set { hdnSaveButtonClientID.Value = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(value, true); }
    }
    [DefaultValue(false)]
    public bool isAddressVerified { get; set; }
    [DefaultValue(false)]
    public bool isAddressValidationFailed { get; set; }
    protected override void OnLoad(EventArgs e)
    {
        txtAmbulanceServiceDropPickUpAddressLine1.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        txtAmbulanceServiceDropPickUpAddressLine2.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        txtBoxAmbulanceServicePickUpCity2.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        ddlAmbulanceServicePickUpState2.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        txtBoxAmbulanceServiceZip2.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        base.OnLoad(e);
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            try
            {
                BindGrid();
                BindDropDowns();
                txtAmbulanceServiceDropPickUpAddressLine1.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
                txtAmbulanceServiceDropPickUpAddressLine2.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
                txtBoxAmbulanceServicePickUpCity2.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
                ddlAmbulanceServicePickUpState2.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
                txtBoxAmbulanceServiceZip2.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
            }
            catch (Exception ex)
            {

            }
        }
        else
        {
            BindDropDowns();
            //btnAddAmbulanceService.Attributes.Add("onclick", "AmbDisableEnableConditionAddButton;");
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
        //if (DisplayReadOnly == true)
        //{
        //    divAmbulanceServicePanel.Visible = false;
        //}
        if (Session["ClaimStatus"] != null)
        {
            if (Session["ClaimStatus"].ToString() == "Pending Submission" && ambulancedropOffDiv.Visible == true)
            {
                hdnClaimstatusAmbulance.Value = "Pending Submission";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>displayAmbulanceDropOffTable();</script>", false);
            }
        }

        if (ambulancedropOffDiv.Visible == true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>clearAmbulanceDropOffFields();</script>", false);
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

    public void BindGrid()
    {
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            LoadAmbulanceGridInformation(null);
        }
    }
    private void BindDropDowns()
    {
        GetServiceLineForAmbulanceService();
        AmbulanceDropAndPickUp();
    }



    protected void btnAdd_AmbulanceService(object sender, EventArgs e)
    {
        if (isAddressVerified)
        {
            if (ValidateAmbulanceAddress())
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Service_Line", ddlAmbulanceServiceLine.SelectedValue.ToString());
                parms.Add("Pick_Up_Address_Line1", txtAmbulanceServicePickUpAddressLine1.Text.Trim());
                parms.Add("Pick_Up_Address_Line2", txtAmbulanceServicePickUpAddressLine2.Text.Trim());
                parms.Add("Pick_Up_City", txtBoxAmbulanceServicePickUpCity1.Text.Trim());
                parms.Add("Pick_Up_State", ddlAmbulanceServicePickUpState1.SelectedValue.ToString().Trim());
                parms.Add("Pick_Up_ZIP", txtBoxAmbulanceServiceZip1.Text.Trim());
                parms.Add("Drop_Off_Location_Name", txtAmbulanceServiceDropOffLocationName.Text.Trim());
                parms.Add("Drop_Off_Address_Line1", txtAmbulanceServiceDropPickUpAddressLine1.Text.Trim());
                parms.Add("Drop_Off_Address_Line2", txtAmbulanceServiceDropPickUpAddressLine2.Text.Trim());
                parms.Add("Drop_Off_City", txtBoxAmbulanceServicePickUpCity2.Text.Trim());
                parms.Add("Drop_Off_State", ddlAmbulanceServicePickUpState2.SelectedValue.ToString().Trim());
                parms.Add("Drop_Off_Zip", txtBoxAmbulanceServiceZip2.Text.Trim());
                parms.Add("Last_Modified_date", DateTime.Now.ToString());
                parms.Add("Last_Modified_user", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString().Trim());
                parms.Add("Claim_ID", hdnClaimId.Value);
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                if (!string.IsNullOrEmpty(hdnClaimId.Value))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(hdnClaimId.Value))
                        {
                            svc.InsertPanelsData("claims_ambulance_pick_up_drop_off_location", parms);
                            hdnAddressConfirm.Value = "0";
                        }

                    }
                    catch (Exception ex)
                    {

                    }

                    //LoadAmbulanceGridInformation(null);

                  //  error1.Visible = false;
                 //   error2.Visible = false;
                   // error3.Visible = false;
                }
            }
            ClearAmbulanceServiceFields();

        }


    }


    private void GetServiceLineForAmbulanceService()
    {

        ddlAmbulanceServiceLine.Items.Clear();
        DataSet dsAmbulanceServiceLine = new DataSet();
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimId.Value, true));
            dsAmbulanceServiceLine = DataAccess.ExecuteStoredProcedure("usp_Select_ServiceLine_For_AmbulancePickupDropOffbyClaimId", parameters, "claims_service_details");
            if (Helper.HasRows(dsAmbulanceServiceLine))
            {

                Helper.LoadList(ddlAmbulanceServiceLine, dsAmbulanceServiceLine.Tables[0], "Service_Line", "Service_Line", true);

            }
        }

    }

    public void ClearAmbulanceServiceFields()
    {
        ddlAmbulanceServiceLine.ClearSelection();
        txtAmbulanceServiceDropOffLocationName.Text = "";
        txtAmbulanceServicePickUpAddressLine1.Text = "";
        txtAmbulanceServicePickUpAddressLine2.Text = "";
        txtAmbulanceServiceDropPickUpAddressLine1.Text = "";
        txtAmbulanceServiceDropPickUpAddressLine2.Text = "";
        txtBoxAmbulanceServicePickUpCity1.Text = "";
        txtBoxAmbulanceServicePickUpCity2.Text = "";
        txtBoxAmbulanceServiceZip1.Text = "";
        ddlAmbulanceServicePickUpState1.ClearSelection();
        txtBoxAmbulanceServiceZip2.Text = "";
        ddlAmbulanceServicePickUpState2.ClearSelection();
        ambulancedropOffDiv.InnerText = "";
        hdnClaimId.Value = "";
    }
    protected void btnCancel_AmbulanceService(object sender, EventArgs e)
    {
        ClearAmbulanceServiceFields();
    }

    protected void btnUpdate_AmbulanceService(object sender, EventArgs e)
    {
        if (isAddressVerified)
        {
            if (ValidateAmbulanceAddress())
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claims_Ambulance_Pick_Up_Drop_Off_Location_ID", hdnRowNumberAmbulanceService.Value.ToString());
                parms.Add("Service_Line", ddlAmbulanceServiceLine.SelectedValue.ToString());
                parms.Add("Pick_Up_Address_Line1", txtAmbulanceServicePickUpAddressLine1.Text.Trim());
                parms.Add("Pick_Up_Address_Line2", txtAmbulanceServicePickUpAddressLine2.Text.Trim());
                parms.Add("Pick_Up_City", txtBoxAmbulanceServicePickUpCity1.Text.Trim());
                parms.Add("Pick_Up_State", ddlAmbulanceServicePickUpState1.SelectedValue.ToString().Trim());
                parms.Add("Pick_Up_ZIP", txtBoxAmbulanceServiceZip1.Text.Trim());
                parms.Add("Drop_Off_Location_Name", txtAmbulanceServiceDropOffLocationName.Text.Trim());
                parms.Add("Drop_Off_Address_Line1", txtAmbulanceServiceDropPickUpAddressLine1.Text.Trim());
                parms.Add("Drop_Off_Address_Line2", txtAmbulanceServiceDropPickUpAddressLine2.Text.Trim());
                parms.Add("Drop_Off_City", txtBoxAmbulanceServicePickUpCity2.Text.Trim());
                parms.Add("Drop_Off_State", ddlAmbulanceServicePickUpState2.SelectedValue.ToString().Trim());
                parms.Add("Drop_Off_Zip", txtBoxAmbulanceServiceZip2.Text.Trim());
                parms.Add("Last_Modified_date", DateTime.Now.ToString());
                parms.Add("Last_Modified_user", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString().Trim());
                parms.Add("Claim_ID", hdnClaimId.Value);
                if (!string.IsNullOrEmpty(hdnClaimId.Value))
                {
                    svc.UpdatePanelsData("Claims_Ambulance_Pick_Up_Drop_Off_Location", parms);
                    //LoadAmbulanceGridInformation(null);

                }
                //btnAddAmbulanceService.Visible = true;
                //btnUpdateAmbulanceService.Visible = false;
                //btnCancelAmbulanceService.Visible = false;
                //hdnAddressConfirm.Value = "0";
                //error1.Visible = false;
                //error2.Visible = false;
                //error3.Visible = false;
            }
            ClearAmbulanceServiceFields();
        }
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

        string origUnitAddress = txtAmbulanceServiceDropPickUpAddressLine2.Text.ToUpper().Trim();
        string origStreetAddress = txtAmbulanceServiceDropPickUpAddressLine1.Text.ToUpper().Trim();
        string origCity = txtBoxAmbulanceServicePickUpCity2.Text.ToUpper().Trim();
        string origState = ddlAmbulanceServicePickUpState2.SelectedItem.Text.ToUpper().Trim();
        // string origCounty = CountyDisplay.ToUpper().Trim();
        string origZip5 = txtBoxAmbulanceServiceZip2.Text;
        //string origZip4 = txtBoxAmbulanceServiceZip2.Text;

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
            revTxtBoxAmbulanceServiceZip2.Enabled = true;
            cvAddress.ErrorMessage = "*Enter 5 digits ZIP)";
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
            detail = intSearch.GetAddressVerificaton(addressVerificationRequest, addressTypeId, Helper.GetUserId(HttpContext.Current.User.Identity.Name),CON.AddressPage.AmbulancePickupAndDrop , 0);

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
                newZip4 = zipParts[1];
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

            if (origUnitAddress.ToUpper() != newUnitAddress.ToUpper())
                isCorrected = true;
            if (origStreetAddress.ToUpper() != newStreetAddress.ToUpper())
                isCorrected = true;
            if (origCity.ToUpper() != newCity.ToUpper())
                isCorrected = true;
            if (origState.ToUpper() != newState.ToUpper())
                isCorrected = true;
            if (origZip5.ToUpper() != newZip5.ToUpper())
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
                if (newStreetAddress != " ")
                    this.paraUSPSAddress.InnerHtml += newStreetAddress + "<br>";
                if (txtAmbulanceServiceDropPickUpAddressLine2.Text != "")
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
                                                   + "sendDataAmbulanceService('" + newStreetAddress + "', '"
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
    private void LoadAmbulanceGridInformation(DataSet dsAmbulance)
    {
        DataSet dsAmbulancePickUpDropOff = new DataSet();
        if (!string.IsNullOrEmpty(hdnClaimId.Value) || !string.IsNullOrWhiteSpace(ICN))
        {
            if (Helper.HasRows(dsAmbulance))
            {
                dsAmbulancePickUpDropOff = dsAmbulance;
            }
            else
            {
                dsAmbulancePickUpDropOff = GetAmbulancePickUpDropOffInformation();
            }

        }
        if (Helper.HasRows(dsAmbulancePickUpDropOff))
        {
            dataSetAmbulancePickUpDropOffInformation = dsAmbulancePickUpDropOff;
             
            DataTable dtAmbulanceInfo = new DataTable();

            dtAmbulanceInfo = dsAmbulancePickUpDropOff.Tables[0];


            if (dtAmbulanceInfo.Rows.Count > 0)
            {
                string table = string.Empty;

                table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Pick Up Address Line 1</th><th style='width:10px; scope='col'>Pick Up City</th><th style='width:10px; scope='col'>Pick Up State</th><th style='width:10px; scope='col'>Pick Up Zip</th><th style='width:10px; scope='col'>Drop Off Address Line 1</th><th style='width:10px; scope='col'>Drop Off City</th><th style='width:10px; scope='col'>Drop Off State</th><th style='width:10px; scope='col'>Drop Off Zip</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                foreach (DataRow dr in dtAmbulanceInfo.Rows)
                {
                    string serviceLine = dr["Service_Line"].ToString();
                    string pickUpAddressLine1 = dr["Pick_Up_Address_Line1"].ToString();
                    string pickUpAddressLine2 = dr["Pick_Up_Address_Line2"].ToString();
                    string pickUpCity = dr["Pick_Up_City"].ToString();
                    string pickUpState = dr["Pick_Up_State"].ToString();
                    string pickUpZip = dr["Pick_Up_ZIP"].ToString();
                    string dropOffLocationName = dr["Drop_Off_Location_Name"].ToString();
                    string dropOffLocationAddressline1 = dr["Drop_Off_Address_Line1"].ToString();
                    string dropOffLocationAddressline2 = dr["Drop_Off_Address_Line2"].ToString();
                    string dropOffLocationCity = dr["Drop_Off_City"].ToString();
                    string dropOffLocationState = dr["Drop_Off_State"].ToString();
                    string dropOffLocationZip = dr["Drop_Off_Zip"].ToString();
                    string Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = dr["Claims_Ambulance_Pick_Up_Drop_Off_Location_ID"].ToString();
                    string Claim_ID = dr["Claim_ID"].ToString();

                    if (Session["ClaimStatus"] != null)
                    {
                        if (Session["ClaimStatus"].ToString() == "Pending Submission")
                        {
                            // table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='ServiceLine' class='tNumber'>" + serviceLine + "</span></td><td><span title='PickupAddressLine1' class='tNumber'>" + pickUpAddressLine1 + "</span></td><td><span title='PickUpCity' class='tNumber'>" + pickUpCity + "</span></td><td><span title='PickUpState' class='tNumber'>" + pickUpState + "</span></td><td><span title='PickUpZip' class='tNumber'>" + pickUpZip + "</span></td><td><span title='DropOffAddressLine1' class='tNumber'>" + dropOffLocationAddressline1 + "</span></td><td><span title='DropOffCity' class='tNumber'>" + dropOffLocationCity + "</span></td><td><span title='DropOffState' class='tNumber'>" + dropOffLocationState + "</span></td><td><span title='DropOffZip' class='tNumber'>" + dropOffLocationZip + "</span></td><td>< input type = 'button' value = 'Edit' onClick = 'return EditAmbulanmceDropOffItem(\"" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type = 'button' value='Delete' onclick='return DeleteAmbulanmceDropOffLineitem(\"" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + serviceLine + "</span></td><td><span title='Line' class='tNumber'>" + pickUpAddressLine1 + "</span></td><td><span  title='Line' class='tNumber'>" + pickUpCity + "</span></td><td><span title='Line' class='tNumber'>" + pickUpState + "</span></td><td>" + pickUpZip + "</td><td>" + dropOffLocationAddressline1 + "</td><td>" + dropOffLocationCity + "</td><td>" + dropOffLocationState + "</td><td>" + dropOffLocationZip + "</td><td><input type='button' value = 'Edit' onClick = 'return EditAmbulanmceDropOffItem(\"" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAmbulanmceDropOffLineitem(\"" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            hdnClaimstatusAmbulance.Value = "Pending Submission";
                            divAmbulanceServicePanel.Visible = true;
                        }
                        else
                        {
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + serviceLine + "</span></td><td><span title='Line' class='tNumber'>" + pickUpAddressLine1 + "</span></td><td><span  title='Line' class='tNumber'>" + pickUpCity + "</span></td><td><span title='Line' class='tNumber'>" + pickUpState + "</span></td><td>" + pickUpZip + "</td><td>" + dropOffLocationAddressline1 + "</td><td>" + dropOffLocationCity + "</td><td>" + dropOffLocationState + "</td><td>" + dropOffLocationZip + "</td></tr>";
                            hdnClaimstatusAmbulance.Value = "Other";
                            // divAmbulanceServicePanel.Visible = false;
                        }
                    }

                }
                table = table + "</tbody></table>";
                ambulancedropOffDiv.InnerHtml = table;
            }
        }
        else
        {
            ambulancedropOffDiv.InnerHtml = "";
        }
    }

    private DataSet GetAmbulancePickUpDropOffInformation()
    {
        DataSet dsAmbulanceInfoInfo = new DataSet();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(hdnClaimId.Value))
        {
            parms.Add("Claim_ID", hdnClaimId.Value.ToString());
            dataSetAmbulancePickUpDropOffInformation = dsAmbulanceInfoInfo = svc.SelectPanelsData("claims_ambulance_pick_up_drop_off_location", parms);
        }
        return dsAmbulanceInfoInfo;

    }
    private void AmbulanceDropAndPickUp()
    {
        Helper.LoadDropDownListWithStates(ref ddlAmbulanceServicePickUpState1, true);
        Helper.LoadDropDownListWithStates(ref ddlAmbulanceServicePickUpState2, true);
    }

    private bool ValidateAmbulanceAddress()
    {
        DataSet dsAmbulance = new DataSet();
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", hdnClaimId.Value);
            dsAmbulance = svc.SelectPanelsData("claims_ambulance_information", parms);
        }
        bool isValid = true;


        if (txtAmbulanceServicePickUpAddressLine1.Text.Equals(AmbulanceInformationPickUpAddressLine1) &&
            txtAmbulanceServicePickUpAddressLine2.Text.Equals(AmbulanceInformationPickUpAddressLine2) &&
            txtBoxAmbulanceServicePickUpCity1.Text.Equals(AmbulanceInformationPickUpCity) &&
            ddlAmbulanceServicePickUpState1.SelectedValue.Equals(AmbulanceInformationPickUpState) &&
            txtBoxAmbulanceServiceZip1.Text.Equals(AmbulanceInformationPickUpZip))

        {
            //error2.Visible = true;
            isValid = false;
        }
        if (txtAmbulanceServiceDropPickUpAddressLine1.Text.Equals(AmbulanceInformationDropOffAddressLine1) &&
            txtAmbulanceServiceDropPickUpAddressLine2.Text.Equals(AmbulanceInformationDropAddressLine2) &&
            txtBoxAmbulanceServicePickUpCity2.Text.Equals(AmbulanceInformationDropOffCity) &&
            ddlAmbulanceServicePickUpState2.SelectedValue.Equals(AmbulanceInformationDropState) &&
            txtBoxAmbulanceServiceZip2.Text.Equals(AmbulanceInformationDropOffZip))

        {
           // error1.Visible = true;
            isValid = false;
        }
        if (!HasAmbulanceHasValue && ddlAmbulanceServiceLine.SelectedIndex > -1)
        {
           // error3.Visible = true;
            isValid = false;
        }
        return isValid;

    }

    public void ClearAmbulancePickupDropOffGrid()
    {
       // grdAmbulanceService.DataSource = null;
       // grdAmbulanceService.DataBind();
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!(ddlAmbulanceServiceLine.SelectedIndex > 0))
        {
            GetServiceLineForAmbulanceService();
        }
    }
    public void SavetoDbonAdjust(DataTable dt)
    {
        if (Helper.HasRows(dt))
        {
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Service_Line", dt.Rows[i]["Service_Line"].ToString());
                parms.Add("Pick_Up_Address_Line1", dt.Rows[i]["Pick_Up_Address_Line1"].ToString());
                parms.Add("Pick_Up_Address_Line2", dt.Rows[i]["Pick_Up_Address_Line2"].ToString());
                parms.Add("Pick_Up_City", dt.Rows[i]["Pick_Up_City"].ToString());
                parms.Add("Pick_Up_State", dt.Rows[i]["Pick_Up_State"].ToString());
                parms.Add("Pick_Up_ZIP", dt.Rows[i]["Pick_Up_ZIP"].ToString());
                parms.Add("Drop_Off_Location_Name", dt.Rows[i]["Drop_Off_Location_Name"].ToString());
                parms.Add("Drop_Off_Address_Line1", dt.Rows[i]["Drop_Off_Address_Line1"].ToString());
                parms.Add("Drop_Off_Address_Line2", dt.Rows[i]["Drop_Off_Address_Line2"].ToString());
                parms.Add("Drop_Off_City", dt.Rows[i]["Drop_Off_City"].ToString());
                parms.Add("Drop_Off_State", dt.Rows[i]["Drop_Off_State"].ToString());
                parms.Add("Drop_Off_Zip", dt.Rows[i]["Drop_Off_Zip"].ToString());
                parms.Add("Last_Modified_date", DateTime.Now.ToString());
                parms.Add("Last_Modified_user", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString().Trim());
                parms.Add("Claim_ID", hdnClaimId.Value);
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                try
                {
                    if (!string.IsNullOrEmpty(hdnClaimId.Value))
                    {
                        svc.InsertPanelsData("claims_ambulance_pick_up_drop_off_location", parms);
                        hdnAddressConfirm.Value = "0";
                    }

                }
                catch (Exception ex)
                {

                }

                LoadAmbulanceGridInformation(null);
            }
        }
    }



}
