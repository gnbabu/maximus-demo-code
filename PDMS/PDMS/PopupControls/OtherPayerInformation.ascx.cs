using Corp.Core.Libraries.ServiceAgent;
using MAXIMUS.Core.Libraries;
using Models.Data;
using Models.Hospice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_OtherPayerInformation : System.Web.UI.UserControl
{
    public delegate void EventHandler();
    public event EventHandler RefreshOtherPayers;

    public event EventHandler RefreshOtherPayerPaidAmountDropdown;
    private bool fromInquirySvc = false;
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

    public bool FromInquirySvc
    {
        get
        {
            return fromInquirySvc;
        }
        set
        {
            fromInquirySvc = value;
        }
    }

    public string SelfMiddleName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnMiddlename.Value))
                return hdnMiddlename.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnMiddlename.Value = value.Trim();
        }
    }
    public string SelfFirstName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnFrstName.Value))
                return hdnFrstName.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnFrstName.Value = value.Trim();
        }
    }
    public string SelfLastName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnLastName.Value))
                return hdnLastName.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnLastName.Value = value.Trim();
        }
    }
    public string PayerSeq
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnPrimaryPayerSequence.Value))
                return hdnPrimaryPayerSequence.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnPrimaryPayerSequence.Value = value.Trim();
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
            //BindGrid();
        }
    }

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnotherPayer_ClaimType.Value))
                return hdnotherPayer_ClaimType.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnotherPayer_ClaimType.Value = value.Trim();
        }
    }

    [DefaultValue(false)]
    public bool isAddressVerified { get; set; }
    private DataSet dataSetOtherPayerInformation = new DataSet();
    public DataSet dataSetOtherPayer
    {
        get
        {
            if (!Helper.HasRows(dataSetOtherPayerInformation))
            {
                if (Helper.HasRows(FetchOtherPayerInformation()))
                {
                    return dataSetOtherPayerInformation;
                }
            }
            return dataSetOtherPayerInformation;
        }
        set
        {
            if (value != null)
            {
                GetOtherPayerInfo(value);
            }
        }
    }

    private DataSet dsOtherPayerTotalPaidAmount = new DataSet();

    public DataSet datasetOtherPayerTotalPaidAmount
    {
        get
        {
            if (!Helper.HasRows(dsOtherPayerTotalPaidAmount))
            {
                if (Helper.HasRows(GetOtherPayerTotalPaidAmount()))
                {
                    return dsOtherPayerTotalPaidAmount;
                }
            }
            return dsOtherPayerTotalPaidAmount;
        }
        set
        {
            if (value != null)
            {
                dsOtherPayerTotalPaidAmount = value;
            }
        }
    }

    public DataSet GetOtherPayerTotalPaidAmount()
    {
        DataSet dsPaidAmont = new DataSet();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            parms.Add("Claim_id", hdnClaimId.Value);
            dsOtherPayerTotalPaidAmount=dsPaidAmont = svc.SelectPanelsData("get_otherpayerpaidamount", parms);
        }
        return dsPaidAmont;
    }

    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(ddlSubcribersState.SelectedItem.Text) || !string.IsNullOrEmpty(txtSubcribersAddressLine1.Text) || !string.IsNullOrEmpty(txtSubscribersAddressLine2.Text) ||
            !string.IsNullOrEmpty(txtSubcribersCity.Text) || !string.IsNullOrEmpty(txtSubscribersZip.Text))
        {
            rtn = true;
        }
        return rtn;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
        lblCity.Visible = false;
        lblState.Visible = false;
        lblAddress1.Visible = false;
        lblZip.Visible = false;

        btnOtherPayerAdd.Attributes.Add("onclick", "PDisableEnableConditionAddButton();");
        //cvtPaidDate.ValueToCompare = DateTime.Now.ToString("MM/dd/yyyy");
        if (!IsPostBack)
        {
            //BindGrid();
            BindDropDowns();

            txtSubcribersAddressLine1.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
            txtSubscribersAddressLine2.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
            txtSubcribersCity.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
            ddlSubcribersState.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
            txtSubscribersZip.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
            hdnAddressConfirm.Value = "0";
        }

        string addressValidationEnabledString = "0";

        if (this.Visible)
        {
            addressValidationEnabledString = AppSettings.Get("AddressValidationEnabled");
        }
        bool addressValidationEnabled = (addressValidationEnabledString == "1");
        cvOtherPayerAddress.Enabled = addressValidationEnabled;
        btnCancelAddressCorrection.OnClientClick = "$(\"#" + divConfirmAddress.ClientID + "\").dialog(\"close\"); "
                                                   + "return false;";
        AssignValidationSummary("valOtherPayerInformation");
        lblHealthPlanIdDuplicate.Visible = false; 
        lblStreetNormalized.Visible = false;
        lblWrongAddress.Visible = false;
        lblMultipleAddress.Visible = false;
        lblNoAddress.Visible = false;
        lblPoBox.Visible = false;
        SetButtonVisibility();
        if (!string.IsNullOrEmpty(hdnClaimId.Value) && !FromInquirySvc)
        {
            GetOtherPayerInfo(dataSetOtherPayerInformation);
        }
        if (pnlsepOtherPayerInformation.Visible)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>ClearotherPayerInfofields();</script>", false);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>displayOtherPayerInfo();</script>", false);
        }
    }
    public void SetButtonVisibility()
    {
        if (DisplayReadOnly == true)
        {
            divOther.Visible = false;
        }
        else
        {
            divOther.Visible = true;
        }
    }

    

    public void ClearData(string payerResSeq)
    {
        ClearOtherPayerFields();
        //Remove PayerResponsibility Sequence from the payer sequence list
        if (!string.IsNullOrEmpty(payerResSeq))
        {
            ViewState["PayerResSeq"] = payerResSeq;
            GetFilteredPayerSequence();
        }
        else if(string.IsNullOrEmpty(payerResSeq)) { ViewState["PayerResSeq"] = null; }
        BindGrid();
    }
    public void ClearPrimaryPayerResSeq(string payerResSeq)
    {
        ViewState["PayerResSeq"] = payerResSeq;
        GetFilteredPayerSequence();
    }

    public void BindGrid()
    {
        GetOtherPayerInfo(null);
    }
    private void BindDropDowns()
    {
        GetClaimAdjudicationLevel();
        GetInsuranceTypeCode();
        GetPatientRelationship();
        SubscribersState();
        GetClaimFilingIndicator();
        GetFilteredPayerSequence();
    }

    protected void txtOtherCheckForDate_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtPaidDate1.Text))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>validateDate17();</script>", false);
        }
        else
        {
            OtherprayerInforDateRequiredError1.Visible = false;
        }
    }
    private void GetClaimAdjudicationLevel()
    {
        ddlClaimAdjudicationLevel.Items.Clear();
        DataSet dataSetClaimAdjudicationLevel = svc.GetClaimAdjudicationLevel();
        if (Helper.HasRows(dataSetClaimAdjudicationLevel))
        {
            Helper.LoadList(ddlClaimAdjudicationLevel, dataSetClaimAdjudicationLevel, "PRIOR_AUTH_SUBMIT_CLAIM_ADJUDICATION_LEVEL_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_ADJUDICATION_LEVEL_ID", true);
        }
    }

    private void GetInsuranceTypeCode()
    {
        ddlInsuranceTypeCode.Items.Clear();
        DataSet dataSetInsuranceTypeCode = svc.GetInsuranceTypes();
        if (Helper.HasRows(dataSetInsuranceTypeCode))
        {
            dataSetInsuranceTypeCode.Tables[0].Rows.Add(0, "", "");
            Helper.LoadList(ddlInsuranceTypeCode, dataSetInsuranceTypeCode, "PRIOR_AUTH_SUBMIT_CLAIM_INSURANCE_TYPE_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_INSURANCE_TYPE_CODE", false);
        }
    }
    public void SubscribersState()
    {
        Helper.LoadDropDownListWithStates(ref ddlSubcribersState, true);
    }   

    private void GetOtherPayerInfo(DataSet dsOtherPayerInfo)
    {
        DataSet dsOtherPayerInformation = new DataSet();
        hdnOtherPayerInfoClaimStatus.Value = "";
        if (!string.IsNullOrEmpty(hdnClaimId.Value) || !string.IsNullOrEmpty(ICN))
        {
            if (Helper.HasRows(dsOtherPayerInfo))
            {
                dsOtherPayerInformation = dsOtherPayerInfo;
            }
            else
            {
                dsOtherPayerInformation = FetchOtherPayerInformation();
            }
        }

        if (Helper.HasRows(dsOtherPayerInformation))
        {
            DataTable otherPayerTable = dsOtherPayerInformation.Tables[0];
            divDentlOP.InnerHtml = "";
            if (otherPayerTable.Rows.Count > 0)
            {
                string tab = string.Empty;
                
                tab = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 70px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:15px; scope='col'>*Other Payer Name</th><th style='width:15px; scope='col'>*Health Plan Id</th><th style='width:15px; scope='col'>Insured Last Name</th><th style='width:10px; scope='col'>Insured First Name</th><th style='width:15px; scope='col'>*Payer Sequence</th><th style='width:15px; scope='col'>*Adjudication Level</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                foreach (DataRow dr in otherPayerTable.Rows)
                {
                    string otherPayerName = dr["Other_Payer_Name"].ToString();
                    string healthPlanID = dr["Health_Plan_ID"].ToString();
                    string subscriberLastName = dr["Subscriber_Last_Name"].ToString();
                    string subscriberFirstName = dr["Subscriber_First_Name"].ToString();
                    string payerResponsibilitySequence = dr["Payer_Responsibility_Sequence"].ToString();
                    if ((payerResponsibilitySequence == "1") || (payerResponsibilitySequence == "P")) payerResponsibilitySequence = "Primary"; 
                    if ((payerResponsibilitySequence == "2") || (payerResponsibilitySequence == "S")) payerResponsibilitySequence = "Secondary";
                    if ((payerResponsibilitySequence == "3") || (payerResponsibilitySequence == "T")) payerResponsibilitySequence = "Tertiary";
                    if ((payerResponsibilitySequence == "4") || (payerResponsibilitySequence == "A")) payerResponsibilitySequence = "Payer Responsibility Four";
                    if ((payerResponsibilitySequence == "5") || (payerResponsibilitySequence == "B")) payerResponsibilitySequence = "Payer Responsibility Five";
                    if ((payerResponsibilitySequence == "6") || (payerResponsibilitySequence == "C")) payerResponsibilitySequence = "Payer Responsibility Six";
                    if ((payerResponsibilitySequence == "7")|| (payerResponsibilitySequence == "D")) payerResponsibilitySequence = "Payer Responsibility Seven";
                    if ((payerResponsibilitySequence == "8") || (payerResponsibilitySequence == "EB")) payerResponsibilitySequence = "Payer Responsibility Eight";
                    if ((payerResponsibilitySequence == "9") || (payerResponsibilitySequence == "F")) payerResponsibilitySequence = "Payer Responsibility Nine";
                    if ((payerResponsibilitySequence == "10")|| (payerResponsibilitySequence == "G")) payerResponsibilitySequence = "Payer Responsibility Ten";
                    if ((payerResponsibilitySequence == "11") || (payerResponsibilitySequence == "H")) payerResponsibilitySequence = "Payer Responsibility Eleven";
                    if (payerResponsibilitySequence == "12") payerResponsibilitySequence = "Unknown";

                    string claimAdjudicationLevel = dr["PRIOR_AUTH_SUBMIT_CLAIM_ADJUDICATION_LEVEL_DESC"].ToString();
                    if (claimAdjudicationLevel == "1")
                    {
                        claimAdjudicationLevel = "Header";
                    }
                    if (claimAdjudicationLevel == "2")
                    {
                        claimAdjudicationLevel = "Detail";
                    }
                    string otherpayerInfoID = dr["Claims_Other_Payer_Information_ID"].ToString();
                    if (Session["ClaimStatus"] != null)
                    {
                        if (Session["ClaimStatus"].ToString() == "Pending Submission")
                        {
                            tab = tab + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + otherPayerName + "</span></td><td><span title='Line' class='tNumber'>" + healthPlanID + "</span></td><td><span  title='Line' class='tNumber'>" + subscriberLastName + "</span></td><td><span title='Line' class='tNumber'>" + subscriberFirstName + "</span></td><td>" + payerResponsibilitySequence + "</td><td>" + claimAdjudicationLevel + "</td><td><input type='button' value = 'Edit' onClick = 'return EditDentalOtherPayerLineItem(\"" + otherpayerInfoID + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteDentalOtherPayerLineitem(\"" + otherpayerInfoID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            hdnOtherPayerInfoClaimStatus.Value = "Pending Submission";
                        }
                        else
                        {
                            tab = tab + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + otherPayerName + "</span></td><td><span title='Line' class='tNumber'>" + healthPlanID + "</span></td><td><span  title='Line' class='tNumber'>" + subscriberLastName + "</span></td><td><span title='Line' class='tNumber'>" + subscriberFirstName + "</span></td><td>" + payerResponsibilitySequence + "</td><td>" + claimAdjudicationLevel + "</td></tr >";
                            hdnOtherPayerInfoClaimStatus.Value = "Other";
                        }
                    }
                }
                tab = tab + "</tbody></table>";
                divDentlOP.InnerHtml = tab;
            }

            //grdOtherPayer.DataSource = dsOtherPayerInformation;
            //grdOtherPayer.DataBind();
        }
        else
        {
            divDentlOP.InnerHtml = "";
            //grdOtherPayer.DataSource = null;
            //grdOtherPayer.DataBind();
        }
    }
    private DataSet FetchOtherPayerInformation()
    {
        DataSet dsOtherPayerInfo = new DataSet();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_ID", hdnClaimId.Value.ToString());
        dataSetOtherPayerInformation = dsOtherPayerInfo = svc.SelectPanelsData("claims_other_payer_information", parms);
        return dsOtherPayerInfo;
    }

    public void GetFilteredPayerSequence()
    {
        ddlPayerSequence.Items.Clear();
        DataSet dsOtherPayerSequence = FetchFilteredOtherPayerSequence();
        if (Helper.HasRows(dsOtherPayerSequence))
        {
            Helper.LoadList(ddlPayerSequence, dsOtherPayerSequence.Tables[0], "PRIOR_AUTH_SUBMIT_CLAIM_PAYERSEQUENCE_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_PAYERSEQUENCE_ID", true);

            if (ViewState["PayerResSeq"] != null && !string.IsNullOrEmpty(ViewState["PayerResSeq"].ToString()))
            {
                var li = ddlPayerSequence.Items.FindByValue(ViewState["PayerResSeq"].ToString());
                if (li != null)
                    ddlPayerSequence.Items.Remove(li);
            }
        }
    }
    private DataSet FetchFilteredOtherPayerSequence()
    {
        ddlPayerSequence.Items.Clear();
        DataSet dataSet = new DataSet();
        if (!string.IsNullOrWhiteSpace(hdnClaimId.Value))
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimId.Value, true));
            dataSet = DataAccess.ExecuteStoredProcedure("usp_SelectFilteredPayerSequence", parameters, "claims_service_details");
        }
        else
        {
            dataSet = svc.GetFilteredOtherPayerSequence();
        }

        return dataSet;
    }
    protected void OtherPayerResponsibilitySequence_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hdnPrimaryPayerSequence.Value))
        {
            errorMessagePrimarySequence.Visible = false;
            if (!string.IsNullOrEmpty(ddlPayerSequence.SelectedValue))
            {
                if (!(Convert.ToInt32(ddlPayerSequence.SelectedValue) > Convert.ToInt32(hdnPrimaryPayerSequence.Value)))
                {
                    cePaidDate.Enabled = true;
                }
                else if (!string.IsNullOrWhiteSpace(ddlPayerSequence.SelectedValue.ToString()) && ddlPayerSequence.SelectedValue.ToString() == CON.OtherPayerSequence.UNKNOWN)
                {
                    ddlClaimAdjudicationLevel.Enabled = true;
                    ddlClaimAdjudicationLevel.AutoPostBack = true;
                }
                else
                {
                    txtClaimNumber.Text = string.Empty;
                    txtPaidDate1.Text = string.Empty;
                    txtPaidAmount1.Text = string.Empty;
                    txtNonCoveredAmount.Text = string.Empty;
                    ddlClaimAdjudicationLevel.ClearSelection();
                    cePaidDate.Enabled = false;
                    ddlClaimAdjudicationLevel.AutoPostBack = false;
                }
            }
        }
        else
        {
            errorMessagePrimarySequence.Visible = true;
            ddlPayerSequence.ClearSelection();
        }
    }
    public string SaveButtonClientID
    {
        get { return (hdnSaveButtonClientID.Value); }
        set { hdnSaveButtonClientID.Value = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(value, true); }
    }
    protected override void OnLoad(EventArgs e)
    {
        txtSubcribersAddressLine1.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        txtSubscribersAddressLine2.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        txtSubcribersCity.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        ddlSubcribersState.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        txtSubscribersZip.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        hdnAddressConfirm.Value = "0";
        base.OnLoad(e);
    }

    protected void cvAOtherPayerAddress_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string newStreetAddress = string.Empty;
        string newUnitAddress = string.Empty;
        string floorDept = string.Empty;
        string newAddressLine3 = string.Empty;
        string newCity = string.Empty;
        string newState = string.Empty;
        string newCounty = string.Empty;
        string newZip4 = string.Empty;
        string newZip5 = string.Empty;

        string origUnitAddress = txtSubscribersAddressLine2.Text.ToUpper().Trim();
        string origStreetAddress = txtSubcribersAddressLine1.Text.ToUpper().Trim();
        string origCity = txtSubcribersCity.Text.ToUpper().Trim();
        string origState = ddlSubcribersState.SelectedItem.Text.ToUpper().Trim();
        string origZip5 = txtSubscribersZip.Text;

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
                lblPoBox.Visible = true;
                return;
            }

            args.IsValid = true;
            return;
        }
        if (!ValidateAddress())
        {
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
            detail = intSearch.GetAddressVerificaton(addressVerificationRequest, addressTypeId, Helper.GetUserId(HttpContext.Current.User.Identity.Name), CON.AddressPage.OtherPaymentInformation, 0);

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

            if (detail.AddressLine2 != " ")
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
                cvOtherPayerAddress.ErrorMessage = "";

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
                if (txtSubscribersAddressLine2.Text != "")
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
                    + "$divConfirm.dialog({ modal: true, position: { my: \"left center\", at: \"right center\", of: \"#otherPayerAddress\" } }); "
                    + "$divConfirm.css(\"display\", \"block\"); "
                    + "$divConfirm.dialog(\"widget\").css(\"z-index\", (maxZIndex() + 10)); "
                    + "}, 100); "
                    + "$(\"#" + SaveButtonClientID + "\").prop(\"disabled\", true); }); ", true);
                args.IsValid = false;

                btnConfirmAddress.OnClientClick = "$(\"#" + hdnAddressConfirm.ClientID + "\").val(\"1\"); "
                                                  + "$(\"#" + divConfirmAddress.ClientID + "\").dialog(\"close\"); "
                                                  + "$(\"#" + SaveButtonClientID + "\").prop(\"disabled\", false); "
                                                  + "sendDataOtherPayer('" + newStreetAddress + "', '"
                                                  + newUnitAddress + "', '"
                                                  + floorDept + "', '"
                                                  + newAddressLine3 + "', '"
                                                  + newCity + "', '"
                                                  + newState + "', '"
                                                  + newCounty + "', '"
                                                  + newZip4 + "', '"
                                                  + newZip5 + "', '"
                                                 + "'); "
                                                  + "return false;";
                return;
            }
        }
        else if (returnCode > 1 || (returnCode < 0 && errorCodes.Contains("11")))
        {
            divConfirmAddress.Style["display"] = "none";
            lblMultipleAddress.Visible = true;
            args.IsValid = false;
        }
        else if (returnCode == -99 || (returnCode == -1 && errorCodes.Contains("07")))
        {
            divConfirmAddress.Style["display"] = "none";
            lblWrongAddress.Visible = true;
            args.IsValid = false;
        }
        else if (returnCode == -3 && errorCodes.Contains("05"))
        {
            divConfirmAddress.Style["display"] = "none";
            lblStreetNormalized.Visible = true;
            args.IsValid = false;
        }
        else
        {
            divConfirmAddress.Style["display"] = "none";
            lblNoAddress.Text += ErrorCodesToHTML(errorCodes);
            lblNoAddress.Visible = true;
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
            + "$divConfirm.dialog({ modal: true, position: { my: \"left center\", at: \"right center\", of: \"#otherPayerAddress\" } }); "
            + "$divConfirm.css(\"display\", \"block\"); "
            + "$divConfirm.dialog(\"widget\").css(\"z-index\", (maxZIndex() + 10)); "
            + "}, 100); "
            + "$(\"#" + SaveButtonClientID + "\").prop(\"disabled\", true); }); ", true);

        btnConfirmWSError.OnClientClick = "$(\"#" + hdnAddressConfirm.ClientID + "\").val(\"1\"); "
                                          + "$(\"#" + divWSError.ClientID + "\").dialog(\"close\"); "
                                          + "$(\"#" + SaveButtonClientID + "\").prop(\"disabled\", false); "
                                          + "return false;";

    }

    protected void btnOtherPayerCancel_Click(object sender, EventArgs e)
    {
        ClearOtherPayerFields();
        btnOtherPayerAdd.Visible = true;
        btnOtherPayerUpdate.Visible = false;
        btnOtherPayerCancel.Visible = false;
    }
    protected void btnOtherPayerInfoAdd_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {

            if (isAddressVerified)
            {
                if (ValidateData())
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("Other_Payer_Name", txtOtherPayerInformation.Text);
                    parms.Add("Health_Plan_ID", txtHealthPlanID.Text);
                    parms.Add("Claim_Filing_Indicator",ddlClaimFilingIndicator.SelectedValue.ToString());
                    parms.Add("Payer_Responsibility_Sequence", ddlPayerSequence.SelectedValue.ToString());
                    parms.Add("Subscriber_Subscriber_Number", txtSubscriptionNumber.Text);
                    parms.Add("Policy_Number", txtPolicyNumber.Text);
                    parms.Add("Group_Name", txtGroupName.Text);
                    parms.Add("Insurance_Type_Code", ddlInsuranceTypeCode.SelectedValue.ToString());
                    parms.Add("Claim_ID", hdnClaimId.Value);
                    parms.Add("Patient_to_Subscriber", ddlPatientRelationship.SelectedValue.ToString());
                    parms.Add("Subscriber_First_Name", txtInsuredfirstName.Text);
                    parms.Add("Subscriber_Last_Name", txtInsuredLastName.Text);
                    parms.Add("Subscriber_Middle_Name", txtOtherPayerMiddleName.Text);
                    parms.Add("Subscriber_AddressLine1", txtSubcribersAddressLine1.Text);
                    parms.Add("Subscriber_AddressLine2", txtSubscribersAddressLine2.Text);
                    parms.Add("Subscriber_City", txtSubcribersCity.Text);
                    parms.Add("Subscriber_State", ddlSubcribersState.SelectedValue.ToString());
                    parms.Add("Subscriber_ZIP", txtSubscribersZip.Text);
                    parms.Add("Claim_Adjudication_Level", ddlClaimAdjudicationLevel.SelectedValue.ToString());
                    parms.Add("Claim_Number", txtClaimNumber.Text);
                    parms.Add("Paid_Date", txtPaidDate1.Text);
                    parms.Add("Paid_Amount", txtPaidAmount1.Text);
                    parms.Add("Total_Non_Covered_Amount", txtNonCoveredAmount.Text);
                    parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                    parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    if (!string.IsNullOrEmpty(hdnClaimId.Value))
                    {
                        svc.InsertPanelsData("Claims_Other_Payer_Information", parms);
                        GetFilteredPayerSequence();
                        GetClaimFilingIndicator();
                        GetOtherPayerInfo(null);
                        ClearOtherPayerFields();
                        ucHeaderOtherPayerAdjustmentMappingProfessional.GetHealthPlanIDForHeaderOtherPayer();
                        ucHeaderOtherPayerAdjustmentMapping.GetHealthPlanIDForHeaderOtherPayer();
                        ucHeaderOtherPayerAdjustmentMappingInstitutional.GetHealthPlanIDForHeaderOtherPayer();
                    }
                    FieldEnableDisable();

                }
                RefreshOtherPayerPaidAmountDropdown();
            }
        }
    }

    protected void btnOtherPayerUpdate_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            if (isAddressVerified)
            {
                if (ValidateEditData())
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("Claims_Other_Payer_Information_ID", hdnRowNumberOtherPayer.Value);
                    parms.Add("Other_Payer_Name", txtOtherPayerInformation.Text);
                    parms.Add("Health_Plan_ID", txtHealthPlanID.Text);
                    parms.Add("Claim_Filing_Indicator", ddlClaimFilingIndicator.SelectedValue.ToString());
                    parms.Add("Payer_Responsibility_Sequence", ddlPayerSequence.SelectedValue.ToString());
                    parms.Add("Subscriber_Subscriber_Number", txtSubscriptionNumber.Text);
                    parms.Add("Policy_Number", txtPolicyNumber.Text);
                    parms.Add("Group_Name", txtGroupName.Text);
                    parms.Add("Insurance_Type_Code", ddlInsuranceTypeCode.SelectedValue.ToString());
                    parms.Add("Claim_ID", hdnClaimId.Value);
                    parms.Add("Patient_to_Subscriber", ddlPatientRelationship.SelectedValue.ToString());
                    parms.Add("Subscriber_First_Name", txtInsuredfirstName.Text);
                    parms.Add("Subscriber_Last_Name", txtInsuredLastName.Text);
                    parms.Add("Subscriber_Middle_Name", txtOtherPayerMiddleName.Text);
                    parms.Add("Subscriber_AddressLine1", txtSubcribersAddressLine1.Text);
                    parms.Add("Subscriber_AddressLine2", txtSubscribersAddressLine2.Text);
                    parms.Add("Subscriber_City", txtSubcribersCity.Text);
                    parms.Add("Subscriber_State", ddlSubcribersState.SelectedValue.ToString());
                    parms.Add("Subscriber_ZIP", txtSubscribersZip.Text);
                    parms.Add("Claim_Adjudication_Level", ddlClaimAdjudicationLevel.SelectedValue.ToString());
                    parms.Add("Claim_Number", txtClaimNumber.Text);
                    parms.Add("Paid_Date", txtPaidDate1.Text);
                    parms.Add("Paid_Amount", txtPaidAmount1.Text);
                    parms.Add("Total_Non_Covered_Amount", txtNonCoveredAmount.Text);
                    parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                    parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    if (!string.IsNullOrEmpty(hdnClaimId.Value))
                    {
                        svc.UpdatePanelsData("Claims_Other_Payer_Information", parms);
                        GetOtherPayerInfo(null);
                        DataSet dsFilteredPayerSeq = FetchFilteredOtherPayerSequence();
                        ClearOtherPayerFields();
                        ddlPayerSequence.ClearSelection();
                        ddlPayerSequence.SelectedValue = null;
                        ddlPayerSequence.DataBind();
                        Helper.LoadList(ddlPayerSequence, dsFilteredPayerSeq.Tables[0], "PRIOR_AUTH_SUBMIT_CLAIM_PAYERSEQUENCE_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_PAYERSEQUENCE_ID", true);
                        DataSet dsFilteredClaimFilingIndicator = GetFilteredClaimIndicator();
                        ddlClaimFilingIndicator.ClearSelection();
                        ddlClaimFilingIndicator.SelectedValue = null;
                        ddlClaimFilingIndicator.DataBind();
                        Helper.LoadList(ddlClaimFilingIndicator, dsFilteredClaimFilingIndicator.Tables[0], "PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_CODE", true);
                        Dictionary<string, string> pars = new Dictionary<string, string>();
                        pars.Add("Claim_ID", hdnClaimId.Value.ToString());
                        pars.Add("HealthPlanId", hdnHealthPlanIdOnEdit.Value);
                        svc.DeletePanelsDataWithParams("claim_other_payer_details", pars);
                        RefreshOtherPayers();
                        btnOtherPayerAdd.Visible = false;
                        //if (grdOtherPayer.Rows.Count <= 10)
                        //{
                        //    btnOtherPayerAdd.Visible = true;
                        //}

                    }
                    btnOtherPayerUpdate.Visible = false;
                    btnOtherPayerCancel.Visible = false;
                    btnOtherPayerAdd.Visible = true;
                    FieldEnableDisable();
                }
            }
        }
        

       
    }

    public void ClearOtherPayerFields()
    {
        txtOtherPayerInformation.Text = "";
        ddlPatientRelationship.ClearSelection();
        txtHealthPlanID.Text = "";
        txtSubscriptionNumber.Text = "";
        ddlClaimFilingIndicator.SelectedIndex = -1;
        txtInsuredfirstName.Text = "";
        txtInsuredLastName.Text = "";
        ddlPayerSequence.SelectedIndex = -1;
        txtPolicyNumber.Text = "";
        txtGroupName.Text = "";
        ddlInsuranceTypeCode.SelectedValue = "";
        txtOtherPayerMiddleName.Text = "";
        txtSubcribersAddressLine1.Text = "";
        ddlSubcribersState.SelectedValue = "";
        txtSubcribersCity.Text = "";
        txtSubscribersZip.Text = "";
        txtSubscribersAddressLine2.Text = "";
    }
    private void GetPatientRelationship()
    {
        ddlPatientRelationship.Items.Clear();
        DataSet dsPatientRelationship = svc.GetPatientRelationship();
        if (Helper.HasRows(dsPatientRelationship))
        {
            Helper.LoadList(ddlPatientRelationship, dsPatientRelationship.Tables[0], "PRIOR_AUTH_SUBMIT_CLAIM_PATIENTRELATIONSHIP_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_PATIENTRELATIONSHIP_CODE", true);
        }
    }
       

    //public void ClearOtherPayerGrid()
    //{
    //    grdOtherPayer.DataSource = null;
    //    grdOtherPayer.DataBind();
    //}
    //protected void grd_OtherPayer_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
        
    //    if (DisplayReadOnly == true)
    //    {
    //        divOther.Visible = false;
    //        grdOtherPayer.Columns[6].Visible = false;
    //        grdOtherPayer.Columns[7].Visible = false;
    //    }
    //    else
    //    {
    //        divOther.Visible = true;
    //        grdOtherPayer.Columns[6].Visible = true;
    //        grdOtherPayer.Columns[7].Visible = true;
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        grdOtherPayer.Columns[6].Visible = false;
    //        grdOtherPayer.Columns[7].Visible = false;

    //        Button btEdit = (Button)e.Row.Cells[6].FindControl("btnEdit");
    //        Button btDelete = (Button)e.Row.Cells[7].FindControl("btnDelete");
    //        if (btEdit != null)
    //            btEdit.Visible = !DisplayReadOnly;
    //        if (btDelete != null)
    //            btDelete.Visible = !DisplayReadOnly;
    //    }
    //}

    private void AssignValidationSummary(string summary)
    {
        if (HasInputValue())
        {
            cvOtherPayerAddress.Enabled = true;
            cvOtherPayerAddress.ValidationGroup = summary;
            isAddressVerified = false;
        }
        else
        {
            isAddressVerified = true;
            cvOtherPayerAddress.Enabled = false;
        }
    }
    private bool ValidateAddress()
    {
        bool validated = true;
        if (HasInputValue())
        {
            if (txtSubscribersZip.Text.ToString() == "")
            {
                lblZip.Visible = true;
                validated = false;
            }
            if ((txtSubcribersAddressLine1.Text == ""))
            {
                lblAddress1.Visible = true;
                validated = false;
            }
            if ((ddlSubcribersState.SelectedItem.Text == ""))
            {
                lblState.Visible = true;
                 validated = false;
            }
            if (txtSubcribersCity.Text == "")
            {
                lblCity.Visible = true;
                validated = false;
            }
        }
        return validated;
    }
    private void FieldEnableDisable()
    {
        cvOtherPayerAddress.Enabled = false;
        hdnAddressConfirm.Value = "0";
        //cePaidDate.Enabled = false;
    }
    private DataSet GetFilteredClaimIndicator()
    {
        DataSet dsClaimFilingIndicator = new DataSet();
        if (!string.IsNullOrWhiteSpace(hdnClaimId.Value))
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimId.Value, true));
            dsClaimFilingIndicator = DataAccess.ExecuteStoredProcedure("usp_SelectFilteredClaimFilingIndicator", parameters, "PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR");

        }
        else
        {
            dsClaimFilingIndicator = svc.GetClaimFilingIndicator();
        }
        return dsClaimFilingIndicator;
    }

    public void GetClaimFilingIndicator()
    {
        ddlClaimFilingIndicator.Items.Clear();
        DataSet dsClaimFilingIndicator = GetFilteredClaimIndicator();
        if (Helper.HasRows(dsClaimFilingIndicator))
        {
            Helper.LoadList(ddlClaimFilingIndicator, dsClaimFilingIndicator.Tables[0], "PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_CODE", true);
        }
    }
    private bool ValidateEditData()
    {
        bool isValid = true;

        DataSet dsOtherPayer = FetchOtherPayerInformation();
        if (Helper.HasRows(dsOtherPayer))
        {
            
            var dt = dsOtherPayer.Tables[0].AsEnumerable().Where(r => r.Field<String>("Health_Plan_ID") != hdnHealthPlanIdOnEdit.Value &&
            r.Field<String>("Health_Plan_ID") == txtHealthPlanID.Text);
            
            if (dt.Any())
            {
                var newDt = dt.CopyToDataTable();
                if (Helper.HasRows(newDt))
                {
                    lblHealthPlanIdDuplicate.Visible = true;
                    isValid = false;
                }

            }

            


        }
        return isValid;
    }
    private bool ValidateData()
    {
        bool isValid = true;
        lblPolicynoandgroupnameErrormsg.Text = string.Empty;
        if (string.IsNullOrEmpty(txtPolicyNumber.Text) && string.IsNullOrEmpty(txtGroupName.Text))
        {

            lblPolicynoandgroupnameErrormsg.Text = "Either group name or policy number is required";
           return isValid = false;
        }
        if (!string.IsNullOrEmpty(txtPolicyNumber.Text) && !string.IsNullOrEmpty(txtGroupName.Text))
        {

            lblPolicynoandgroupnameErrormsg.Text = "Group name and policy number cannot both be entered";
           return isValid = false;
        }
        DataSet dsOtherPayer = FetchOtherPayerInformation();
        if (Helper.HasRows(dsOtherPayer))
        {
            IEnumerable<DataRow> otherPayerTabele = from row in dsOtherPayer.Tables[0].AsEnumerable()
                                                    where (row.Field<string>("Health_Plan_ID") == txtHealthPlanID.Text)
                                                    select row;
            DataTable dtOtherPayerTable = new DataTable();
            if (otherPayerTabele.Count() > 0)
            {
                dtOtherPayerTable = otherPayerTabele.CopyToDataTable();
            }
            if (Helper.HasRows(dtOtherPayerTable))
            {
                lblHealthPlanIdDuplicate.Visible = true;
                isValid = false;
            }
        }
        return isValid;
    }
    public void GetClaimFilingIndicatorWithoutFilter()
    {
        ddlClaimFilingIndicator.Items.Clear();
        DataSet dsClaimFilingIndicator = svc.GetClaimFilingIndicator();
        if (Helper.HasRows(dsClaimFilingIndicator))
        {
            ddlClaimFilingIndicator.ClearSelection();
            ddlClaimFilingIndicator.SelectedValue = null;
            ddlClaimFilingIndicator.DataBind();
            Helper.LoadList(ddlClaimFilingIndicator, dsClaimFilingIndicator.Tables[0], "PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_CODE", true);
        }
    }
    public void GetPayerSequenceWithoutFilter()
    {
        ddlPayerSequence.Items.Clear();
        DataSet dsPayerSequence = svc.GetPayerSequence();
        if (Helper.HasRows(dsPayerSequence))
        {
            ddlPayerSequence.ClearSelection();
            ddlPayerSequence.SelectedValue = null;
            ddlPayerSequence.DataBind();
            Helper.LoadList(ddlPayerSequence, dsPayerSequence.Tables[0], "PRIOR_AUTH_SUBMIT_CLAIM_PAYERSEQUENCE_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_PAYERSEQUENCE_ID", true);
        }
    }
    protected void ddlClaimAdjudicationLevel_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ddlClaimAdjudicationLevel.SelectedValue.ToString()))
        {
            txtClaimNumber.Text = string.Empty;
            txtPaidAmount1.Text = string.Empty;
            txtNonCoveredAmount.Text = string.Empty;
            txtPaidDate1.Text = string.Empty;
            cePaidDate.Enabled = false;
        }
        else if(Convert.ToInt32(ddlClaimAdjudicationLevel.SelectedValue.ToString()) == CON.ClaimsAdjudicationLevel.Details)
        {
            

            txtPaidAmount1.Text = string.Empty;
            txtNonCoveredAmount.Text = string.Empty;
            txtClaimNumber.Enabled = true;
            txtClaimNumber.ReadOnly = false;
            txtPaidDate1.Enabled = true;
            txtPaidDate1.ReadOnly = false;
            txtNonCoveredAmount.Enabled = false;
            txtNonCoveredAmount.ReadOnly = true;
            txtPaidAmount1.Enabled = false;
            txtPaidAmount1.ReadOnly = false;
            cePaidDate.Enabled = true;
        }
        else if (Convert.ToInt32(ddlClaimAdjudicationLevel.SelectedValue.ToString()) == CON.ClaimsAdjudicationLevel.Header)
        {
            txtPaidDate1.Enabled = true;
            txtPaidDate1.ReadOnly = false;
            txtPaidAmount1.Enabled = true;
            txtPaidAmount1.ReadOnly = false;
            txtClaimNumber.Enabled = true;
            txtClaimNumber.ReadOnly = false;
            txtNonCoveredAmount.Enabled = true;
            txtNonCoveredAmount.ReadOnly = false;
            cePaidDate.Enabled = true;
        }
    }
    public void SaveAdjustData(DataTable dt)
    {
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(SqlParms.CreateParameter("CLAIM_ID", DbType.Int32, hdnClaimId.Value, true));
        DataAccess.ExecuteStoredProcedure("usp_Claims_Clear_Other_Payer_Information", param, "Claims_Other_Payer_Information");

        DataTable dt1 = dt;
        if (Helper.HasRows(dt))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Other_Payer_Name", dt.Rows[i]["Other_Payer_Name"].ToString());
                parms.Add("Health_Plan_ID", dt.Rows[i]["Health_Plan_ID"].ToString());
                parms.Add("Claim_Filing_Indicator", dt.Rows[i]["Claim_Filing_Indicator"].ToString());
                parms.Add("Payer_Responsibility_Sequence", GetPayerResponsibilitySequenceCode(dt.Rows[i]["Payer_Responsibility_Sequence"].ToString()));
                parms.Add("Subscriber_Subscriber_Number", dt.Rows[i]["Subscriber_Subscriber_Number"].ToString());
                parms.Add("Policy_Number", dt.Rows[i]["Policy_Number"].ToString());
                parms.Add("Group_Name", dt.Rows[i]["Group_Name"].ToString());
                parms.Add("Insurance_Type_Code", dt.Rows[i]["Insurance_Type_Code"].ToString());
                parms.Add("Claim_ID", hdnClaimId.Value);
                parms.Add("Patient_to_Subscriber", dt.Rows[i]["Patient_to_Subscriber"].ToString());
                parms.Add("Subscriber_First_Name", dt.Rows[i]["Subscriber_First_Name"].ToString());
                parms.Add("Subscriber_Last_Name", dt.Rows[i]["Subscriber_Last_Name"].ToString());
                parms.Add("Subscriber_Middle_Name", dt.Rows[i]["Subscriber_Middle_Name"].ToString());
                parms.Add("Subscriber_AddressLine1", dt.Rows[i]["Subscriber_AddressLine1"].ToString());
                parms.Add("Subscriber_AddressLine2", dt.Rows[i]["Subscriber_AddressLine2"].ToString());
                parms.Add("Subscriber_City", dt.Rows[i]["Subscriber_City"].ToString());
                parms.Add("Subscriber_State", dt.Rows[i]["Subscriber_State"].ToString());
                parms.Add("Subscriber_ZIP", dt.Rows[i]["Subscriber_ZIP"].ToString());
                if (!string.IsNullOrEmpty(dt.Rows[i]["PRIOR_AUTH_SUBMIT_CLAIM_ADJUDICATION_LEVEL_DESC"].ToString()))
                    parms.Add("Claim_Adjudication_Level", dt.Rows[i]["PRIOR_AUTH_SUBMIT_CLAIM_ADJUDICATION_LEVEL_DESC"].ToString().ToLower() == "header" ? "1" : "2");
                parms.Add("Claim_Number", dt.Rows[i]["Claim_Number"].ToString());
                if (dt.Rows[i]["Paid_Date"].ToString().Equals("00010101")) {
                    parms.Add("Paid_Date", "19000101");
                } else {
                    parms.Add("Paid_Date", dt.Rows[i]["Paid_Date"].ToString());
                }
                parms.Add("Paid_Amount", dt.Rows[i]["Paid_Amount"].ToString());
                parms.Add("Total_Non_Covered_Amount", dt.Rows[i]["Total_Non_Covered_Amount"].ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                if (!string.IsNullOrEmpty(hdnClaimId.Value))
                {
                    svc.InsertPanelsData("Claims_Other_Payer_Information", parms);
                    GetFilteredPayerSequence();
                    GetClaimFilingIndicator();
                    GetOtherPayerInfo(null);
                    ClearOtherPayerFields();
                }
                FieldEnableDisable();
            }
        }
    }
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    private string GetPayerResponsibilitySequenceCode(string payerSeq)
    {
        string name = string.Empty;
        if (!string.IsNullOrWhiteSpace(payerSeq) && Helper.HasRows(this.WorkflowPage.OtherPayerSequenceTable.Tables[0]))
        {
            var dt1 = this.WorkflowPage.OtherPayerSequenceTable.Tables[0].AsEnumerable().Where(r => r.Field<String>("PRIOR_AUTH_SUBMIT_CLAIM_PAYERSEQUENCE_CODE") == payerSeq);
            var newDt = dt1.CopyToDataTable();
            if (Helper.HasRows(newDt))
            {
                name = newDt.Rows[0]["PRIOR_AUTH_SUBMIT_CLAIM_PAYERSEQUENCE_ID"].ToString();
            }
        }
        return name;
    }
    public void SaveAdjustDataProfessioanl(DataTable dt)
    {
        DataTable dt1 = dt;
        if (Helper.HasRows(dt))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Other_Payer_Name", dt.Rows[i]["Other_Payer_Name"].ToString());
                parms.Add("Health_Plan_ID", dt.Rows[i]["Health_Plan_ID"].ToString());
                parms.Add("Claim_Filing_Indicator", GetClaimFilingIndicator(dt.Rows[i]["Claim_Filing_Indicator"].ToString()));
                parms.Add("Payer_Responsibility_Sequence", GetPayerResponsibilitySequenceCode(dt.Rows[i]["Payer_Responsibility_Sequence"].ToString()));
                parms.Add("Subscriber_Subscriber_Number", dt.Rows[i]["Subscriber_Subscriber_Number"].ToString());
                parms.Add("Policy_Number", dt.Rows[i]["Policy_Number"].ToString());
                parms.Add("Group_Name", dt.Rows[i]["Group_Name"].ToString());
                parms.Add("Insurance_Type_Code", dt.Rows[i]["Insurance_Type_Code"].ToString());
                parms.Add("Claim_ID", hdnClaimId.Value);
                parms.Add("Patient_to_Subscriber", dt.Rows[i]["Patient_to_Subscriber"].ToString());
                parms.Add("Subscriber_First_Name", dt.Rows[i]["Subscriber_First_Name"].ToString());
                parms.Add("Subscriber_Last_Name", dt.Rows[i]["Subscriber_Last_Name"].ToString());
                parms.Add("Subscriber_Middle_Name", dt.Rows[i]["Subscriber_Middle_Name"].ToString());
                parms.Add("Subscriber_AddressLine1", dt.Rows[i]["Subscriber_AddressLine1"].ToString());
                parms.Add("Subscriber_AddressLine2", dt.Rows[i]["Subscriber_AddressLine2"].ToString());
                parms.Add("Subscriber_City", dt.Rows[i]["Subscriber_City"].ToString());
                parms.Add("Subscriber_State", dt.Rows[i]["Subscriber_State"].ToString());
                parms.Add("Subscriber_ZIP", dt.Rows[i]["Subscriber_ZIP"].ToString());
                parms.Add("Claim_Number", dt.Rows[i]["Claim_Number"].ToString());
                if (dt.Rows[i]["Paid_Date"].ToString().Equals("00010101"))
                {
                    parms.Add("Paid_Date", "19000101".ToString());
                } else
                {
                    parms.Add("Paid_Date", dt.Rows[i]["Paid_Date"].ToString());
                }
                parms.Add("Paid_Amount", dt.Rows[i]["Paid_Amount"].ToString());
                parms.Add("Total_Non_Covered_Amount", dt.Rows[i]["Total_Non_Covered_Amount"].ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                if (!string.IsNullOrEmpty(hdnClaimId.Value))
                {
                    svc.InsertPanelsData("Claims_Other_Payer_Information", parms);
                    GetFilteredPayerSequence();
                    GetClaimFilingIndicator();
                    GetOtherPayerInfo(null);
                    ClearOtherPayerFields();
                }
                FieldEnableDisable();
            }
        }
    }
    private string GetClaimFilingIndicator(string filingIndicator)
    {
        string claimFilingIndicator = string.Empty;
        if (!string.IsNullOrWhiteSpace(filingIndicator) && Helper.HasRows(this.WorkflowPage.ClaimFilingIndicator.Tables[0]))
        {
            var dt1 = this.WorkflowPage.ClaimFilingIndicator.Tables[0].AsEnumerable().Where(r => r.Field<String>("PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_CODE") == filingIndicator);
            var newDt = dt1.CopyToDataTable();
            if (Helper.HasRows(newDt))
            {
                claimFilingIndicator = newDt.Rows[0]["PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_ID"].ToString();
            }
        }
        return claimFilingIndicator;
    }
   
}