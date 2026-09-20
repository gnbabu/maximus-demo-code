using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_AccidentInformation : System.Web.UI.UserControl
{
    private bool fromInquirySvc = false;
    public Boolean SetReadOnlyFields
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value.ToString()))
                SetReadOnlyFieldsControl(value);
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
    public string AccidentRelateddto1
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlAccidentrelatedto.SelectedValue))
                return ddlAccidentrelatedto.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlAccidentrelatedto.ClearSelection();
                if (ddlAccidentrelatedto.Items.FindByValue(value.Trim()) != null)
                {
                    ddlAccidentrelatedto.SelectedValue = value.Trim();
                }            }

        }
    }
    public string AccidentRelateddto2
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlAccidentrelatedto1.SelectedValue))
                return ddlAccidentrelatedto1.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlAccidentrelatedto1.ClearSelection();
                ddlAccidentrelatedto1.SelectedIndex = ddlAccidentrelatedto1.Items.IndexOf(ddlAccidentrelatedto1.Items.FindByValue(value.Trim()));
            }

        }
    }
    public string AccidentState
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlAccidentState.SelectedValue))
                return ddlAccidentState.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlAccidentState.ClearSelection();
                ddlAccidentState.SelectedValue = value.Trim();
            }
        }
    }
    public string AccidentCountry
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlAccidentcountry.SelectedValue))
                return ddlAccidentcountry.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlAccidentcountry.ClearSelection();
                ddlAccidentcountry.SelectedItem.Text = value.Trim();
            }
        }
    }
    public string AccidentDate
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtAccidentDate.Text))
                return txtAccidentDate.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtAccidentDate.Text = value.Trim();
        }
    }
    public string AccidentRelatedTo1Text
    {
        get
        {
            if (!ddlAccidentrelatedto.Visible) return null;
            else if (!string.IsNullOrWhiteSpace(ddlAccidentrelatedto.SelectedItem.Text))
                return ddlAccidentrelatedto.SelectedItem.Text;
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlAccidentrelatedto.ClearSelection();
                ddlAccidentrelatedto.SelectedItem.Text = value.Trim();
            }
                
        }
    }
    public string AccidentRelatedTo2Text
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlAccidentrelatedto1.SelectedItem.Text))
                return ddlAccidentrelatedto1.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlAccidentrelatedto1.ClearSelection();
                ddlAccidentrelatedto1.SelectedItem.Text = value.Trim();
            }
                
        }
    }
    public string AccidentStateText
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlAccidentState.SelectedItem.Text))
                return ddlAccidentState.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlAccidentState.ClearSelection();
                ddlAccidentState.SelectedItem.Text = value.Trim();
            }
                
        }
    }
    public string AccidentCountryText
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlAccidentcountry.SelectedItem.Text))
                return ddlAccidentcountry.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                ddlAccidentcountry.ClearSelection();
                ddlAccidentcountry.SelectedItem.Text = value.Trim();
            }
               
        }
    }
    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(ddlAccidentrelatedto.SelectedItem.Text) || !string.IsNullOrEmpty(txtAccidentDate.Text) || 
            !string.IsNullOrEmpty(ddlAccidentState.SelectedItem.Text) || !string.IsNullOrEmpty(ddlAccidentcountry.SelectedItem.Text))
           
        {
            rtn = true;
        }

        return rtn;
    }

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnAccidentInfoClaimType.Value))
                return hdnAccidentInfoClaimType.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                hdnAccidentInfoClaimType.Value = value.Trim();
                DisplayPanelControls();
            }
        }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
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

    private void DisplayPanelControls()
    {
        if (hdnAccidentInfoClaimType.Value == CON.ClaimsType.Dental)
        {
            dvAccRelatedTo.Attributes["style"] = "display:block";
            dvAccState.Attributes["style"] = "display:block";
            dvAccCountry.Attributes["style"] = "display:block";
            dvAccRelatedTo1.Attributes["style"] = "display:block";
            dvAccDate.Attributes["style"] = "display:block";
        }
        if (hdnAccidentInfoClaimType.Value == CON.ClaimsType.Institutional)
        {
            dvAccRelatedTo.Attributes["style"] = "display:none";
            dvAccState.Attributes["style"] = "display:block";
            dvAccCountry.Attributes["style"] = "display:none";
            dvAccRelatedTo1.Attributes["style"] = "display:none";
            dvAccDate.Attributes["style"] = "display:none";
        }
        if (hdnAccidentInfoClaimType.Value == CON.ClaimsType.Professional)
        {
            dvAccRelatedTo.Attributes["style"] = "display:block";
            dvAccState.Attributes["style"] = "display:block";
            dvAccCountry.Attributes["style"] = "display:block";
            dvAccRelatedTo1.Attributes["style"] = "display:block";
            dvAccDate.Attributes["style"] = "display:block";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            try
            {
                if (!FromInquirySvc)
                {
                    BindDropDowns();
                    BindGrid();
                }
                
            }
            catch (Exception ex)
            {

            }
        }
        if (ddlAccidentrelatedto.SelectedValue == "")
        {
            ddlAccidentrelatedto1.Enabled = false;
        }
        else
        {
            ddlAccidentrelatedto1.Enabled = true;
        }
    }
    public void LoadControlData()
    {
        sepAccidentinfo.InnerText = sepAccidentinfo.InnerText.Replace('-', '+');
    }

  
    public void BindGrid()
    {
        GetAccidentInformationPanelInfo();
    }
    public void BindDropDowns()
    {
        GetAccidentrelatedto();
        GetAccidentState();
        GetCountry();
        GetAccidentrelatedto1();
        LoadAccidentRelatedTo();
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

    //private bool ValidateData()
    //{
    //    bool isValid = true;
    //    if(ddlAccidentcountry.SelectedIndex == 0 && ddlAccidentState.SelectedIndex == 0)
    //    {
    //        AddValidationErrorMessage("<div>*Accident State or Accident Country is required</div>");
    //        isValid = false;
    //    }
    //    if (!String.IsNullOrEmpty(ddlAccidentrelatedto.Text) && String.IsNullOrEmpty(ddlAccidentrelatedto1.Text))
    //    {
    //        AddValidationErrorMessage("<div>*Accident Related To is required</div>");
    //        isValid = false;
    //    }
    //    if (ddlAccidentrelatedto.SelectedItem.Text != "EM" && string.IsNullOrEmpty(txtAccidentDate.Text))
    //    {
    //        AddValidationErrorMessage("<div>*Accident date is required</div>");
    //        isValid = false;
    //    }
    //    return isValid;
    //}
    protected void ddlAccidentcountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlAccidentcountry.SelectedItem.Text != "US")
        {
            ddlAccidentState.ClearSelection();
           
        }
       
    }
   private void AssignValidationSummary(string validationGroup)
    {
        //rfvAccidentrelatedto.ValidationGroup = validationGroup;
        //cvAccidentDateNotReported.ValidationGroup = validationGroup;
        //cvState.ValidationGroup = validationGroup;
        //cvCountry.ValidationGroup = validationGroup;


    }
    public void SaveAccidentInformation(string validationGroup,int actionButtonType)
    {
        if(actionButtonType == CON.ActionButtonType.Save)
        {
            SaveToDB();
            
        }
        else
        {
            //AssignValidationSummary(validationGroup);
            Page.Validate(validationGroup);
            if (Page.IsValid)
            {
                SaveToDB();
            }
        }
        
        

    }
    private void SaveToDB()
    {
        //if (ValidateData())
        //{
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Accident_relatedFrom", ddlAccidentrelatedto.SelectedValue.ToString());
            parms.Add("Accident_relatedTo", ddlAccidentrelatedto1.SelectedValue.ToString());
            parms.Add("Accident_State", ddlAccidentState.SelectedValue.ToString());
            parms.Add("Accident_Country", ddlAccidentcountry.SelectedValue.ToString());
            parms.Add("Accident_Date", txtAccidentDate.Text);
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
            parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            if (!string.IsNullOrEmpty(hdnClaimId.Value) && !string.IsNullOrEmpty(hdnAccidentId.Value))
            {
                parms.Add("Claim_ID", hdnClaimId.Value.ToString());
                svc.UpdatePanelsData("Claims_Accident_Information", parms);
            }
            else if (!string.IsNullOrEmpty(hdnClaimId.Value) && string.IsNullOrEmpty(hdnAccidentId.Value))
            {

                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Claim_ID", hdnClaimId.Value.ToString());
                svc.InsertPanelsData("Claims_Accident_Information", parms);

            }
            GetAccidentInformationPanelInfo();
        //}
    }
    private DataSet GetAccidentInformationPanelInfo()
    {
        DataSet dsAccidentInformationPanelInfo = new DataSet();
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", hdnClaimId.Value.ToString());
            dsAccidentInformationPanelInfo = svc.SelectPanelsData("Claims_Accident_Information", parms);
            if (Helper.HasRows(dsAccidentInformationPanelInfo))
            {
                hdnAccidentId.Value = dsAccidentInformationPanelInfo.Tables[0].Rows[0]["Claims_Accident_Information_Id"].ToString();
                ddlAccidentrelatedto.SelectedValue = dsAccidentInformationPanelInfo.Tables[0].Rows[0]["Accident_relatedFrom"].ToString();
                //LoadAccidentRelatedTo();
                ddlAccidentrelatedto1.SelectedValue = dsAccidentInformationPanelInfo.Tables[0].Rows[0]["Accident_relatedTo"].ToString();
                if (ddlAccidentrelatedto.SelectedValue == "")
                {
                    ddlAccidentrelatedto1.Enabled = false;
                }
                else
                {
                    ddlAccidentrelatedto1.Enabled = true;
                }
                if (!DBNull.Value.Equals(dsAccidentInformationPanelInfo.Tables[0].Rows[0]["Accident_Date"]))
                {
                    txtAccidentDate.Text = Convert.ToDateTime(dsAccidentInformationPanelInfo.Tables[0].Rows[0]["Accident_Date"]).ToString("MM/dd/yyyy");

                }
                else
                {
                    txtAccidentDate.Text = string.Empty;
                }
                ddlAccidentState.SelectedValue = dsAccidentInformationPanelInfo.Tables[0].Rows[0]["Accident_State"].ToString();
                GetCountry();
                ddlAccidentcountry.SelectedValue = dsAccidentInformationPanelInfo.Tables[0].Rows[0]["Accident_Country"].ToString();

            }
        }
        return dsAccidentInformationPanelInfo;
    }

    protected void ddlAccidentrelatedto_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAccidentrelatedto.SelectedValue == "")
        {
            ddlAccidentrelatedto1.Enabled = false;
        }
        else
        {
            ddlAccidentrelatedto1.Enabled = true;
        }
        GetAccidentrelatedto1();
        
    }


    protected void ddlAccidentState_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetCountry();
        ddlAccidentcountry.SelectedValue = "US";

    }

    private void GetCountry()
    {
        ddlAccidentcountry.Items.Clear();
        ddlAccidentcountry.Enabled = true;
        DataSet dataSetCountryCode = svc.GetCountryCodes();
        if (Helper.HasRows(dataSetCountryCode))
        {
            DataTable dt = dataSetCountryCode.Tables[0];
            Helper.LoadList(ddlAccidentcountry, dt, "COUNTRY_CODE", "COUNTRY_CODE", true);
            pnlsepAccidentinfo.Focus();
        }
    }


    public void GetAccidentrelatedto1()
    {
        ddlAccidentrelatedto1.Items.Clear();
        DataSet dataSet = svc.GetAccidentrelatedto();
        if (Helper.HasRows(dataSet) && !string.IsNullOrWhiteSpace(ddlAccidentrelatedto.SelectedItem.Text))
        {
            DataView dv = new DataView(dataSet.Tables[0]);
            dv.RowFilter = "PRIOR_AUTH_SUBMIT_CLAIM_ACCIDENTRELATEDTO_ID <> " + ddlAccidentrelatedto.SelectedValue.ToString();
            Helper.LoadList(ddlAccidentrelatedto1, dv.ToTable(), "PRIOR_AUTH_SUBMIT_CLAIM_ACCIDENTRELATEDTO_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_ACCIDENTRELATEDTO_ID", true);
        }
    }
    public void LoadAccidentRelatedTo()
    {
        ddlAccidentrelatedto1.Items.Clear();
        DataSet dataSetAccidentRelatedTo = svc.GetAccidentrelatedto();
        if (Helper.HasRows(dataSetAccidentRelatedTo))
        {
            Helper.LoadList(ddlAccidentrelatedto1, dataSetAccidentRelatedTo.Tables[0], "PRIOR_AUTH_SUBMIT_CLAIM_ACCIDENTRELATEDTO_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_ACCIDENTRELATEDTO_ID", true);
        }
    }


    public void GetAccidentrelatedto()
    {

        ddlAccidentrelatedto.Items.Clear();
        DataSet dataSetAccidentRelatedTo = svc.GetAccidentrelatedto();
        if (Helper.HasRows(dataSetAccidentRelatedTo))
        {
            DataTable dtAccidentRelatedTo = dataSetAccidentRelatedTo.Tables[0];
            Helper.LoadList(ddlAccidentrelatedto, dtAccidentRelatedTo, "PRIOR_AUTH_SUBMIT_CLAIM_ACCIDENTRELATEDTO_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_ACCIDENTRELATEDTO_ID", true);
        }
    }

    private void GetAccidentState()
    {
        ddlAccidentState.Items.Clear();
        DataSet dataSetAccidentState = svc.GetAccidentUSState();
        if (Helper.HasRows(dataSetAccidentState))
        {
            DataTable dtAccidentTable = dataSetAccidentState.Tables[0];
            Helper.LoadList(ddlAccidentState, dtAccidentTable, "STATE_ABBREV", "STATE_ABBREV", true);
        }
    }
    protected void ReportAccidentDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtAccidentDate.Text, true) && Helper.IsValidDate(txtAccidentDate.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtAccidentDate.Text).Subtract(Convert.ToDateTime(txtAccidentDate.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
        {
            args.IsValid = true;
        }
            

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //cvtAccidentDate.ValueToCompare = DateTime.Now.ToShortDateString();

    }
    protected void txtCheckForDate_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtAccidentDate.Text))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>validateDate1();</script>", false);
        }
        else
        {
            birthDateRequiredError1.Visible = false;
        }
    }
    public void ClearAccidentInformation()
    {
        ddlAccidentrelatedto1.ClearSelection();
        ddlAccidentrelatedto.ClearSelection();
        ddlAccidentState.ClearSelection();
        ddlAccidentcountry.ClearSelection();
        txtAccidentDate.Text = "";
    }
    private void SetReadOnlyFieldsControl(bool value)
    {
        
        ddlAccidentrelatedto.Enabled = !value;
        ddlAccidentrelatedto1.Enabled = !value;
        ddlAccidentcountry.Enabled = !value;
        ddlAccidentState.Enabled = !value;
        txtAccidentDate.Enabled = !value;
        ceAccidentDate.EnabledOnClient = !value;
      
    }

    protected void cvState_ServerValidate(object sender, ServerValidateEventArgs args)
    {
         if((hdnAccidentInfoClaimType.Value == CON.ClaimsType.Professional || hdnAccidentInfoClaimType.Value == CON.ClaimsType.Dental) && string.IsNullOrWhiteSpace(ddlAccidentState.SelectedItem.Text) && string.IsNullOrWhiteSpace(ddlAccidentcountry.SelectedItem.Text))
        {
            args.IsValid = false;
        }
    }

    
    protected void cvCountry_ServerValidate(object sender, ServerValidateEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(ddlAccidentState.SelectedItem.Text) && string.IsNullOrWhiteSpace(ddlAccidentcountry.SelectedItem.Text))
        {
            args.IsValid = false;
        }
    }
    
    protected void cvDate_ServerValidate(object sender, ServerValidateEventArgs args)
    {
        if (ddlAccidentrelatedto.SelectedValue != CON.AccidentRelatedTo.Employment && string.IsNullOrEmpty(txtAccidentDate.Text))
        {
            args.IsValid = false;
        }
    }


}
