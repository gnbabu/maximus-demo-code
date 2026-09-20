using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using Models.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_AdditionalProviderInfoPanel : System.Web.UI.UserControl
{
    private bool fromInquirySvc = false;
    #region Properties

    DataSet dsAdditionalProviderinfo = new DataSet();
    public DataSet AdditionalProviderinfo
    {
        get
        {
            if (!Helper.HasRows(dsAdditionalProviderinfo))
            {
                if (Helper.HasRows(FetchAdditionalProviderInformation()))
                {
                    return dsAdditionalProviderinfo;
                }
            }
            return dsAdditionalProviderinfo;
        }
        set
        {
            if (value != null && Helper.HasRows(value))
            {
                SetAdditionalProviders(value);
                //GetAdditionalProviderData(value);
            }
        }
    }
    public string ClaimID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimIdAdditional.Value))
                return hdnClaimIdAdditional.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimIdAdditional.Value = value.Trim();
        }
    }
    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimType_Additional.Value))
                return hdnClaimType_Additional.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimType_Additional.Value = value.Trim();
        }
    }
    public string AdditionalProviderNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtProviderNPI.Text))
                return txtProviderNPI.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtProviderNPI.Text = value;
        }
    }


    public string AdditionalProviderLastname
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblProviderLastName.Text))
                return lblProviderLastName.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblProviderLastName.Text = value;
        }
    }
    public string AdditionalProviderFirstname
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblProviderFirstName.Text))
                return lblProviderFirstName.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblProviderFirstName.Text = value;
        }
    }
    public string AdditionalProviderMiddleName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblProviderMI.Text))
                return lblProviderMI.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblProviderMI.Text = value;
        }
    }
    #endregion
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

    public string BillingNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnAdditionalBillingNPI.Value))
                return hdnAdditionalBillingNPI.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnAdditionalBillingNPI.Value = value.Trim();
        }
    }
    private string renderingprovnpi = string.Empty;
    public string RenderingProviderNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(renderingprovnpi))
                return renderingprovnpi;
            
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                renderingprovnpi = value;            
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
    private string servicefacilityprovnpi = string.Empty;
    public string ServiceFacilityProviderNPI
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(servicefacilityprovnpi))
                return servicefacilityprovnpi;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                servicefacilityprovnpi = value;
        }
    }

    private string servicefacilityprovMedicaid = string.Empty;
    public string ServiceFacilityProvider_Medicaid
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(servicefacilityprovMedicaid))
                return servicefacilityprovMedicaid;
           
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                servicefacilityprovMedicaid = value;
        }
    }
    private string refprovnpi = string.Empty;
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
    private ClaimsServiceAgent ClaimService = null;


    #region svc
    private PDMSService.PDMSServiceClient _svc;
    #endregion
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
            //GetAdditionalProviderData(null);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (hdnClaimType_Additional.Value == CON.ClaimsType.Dental)
        {
            divAdditionalMedicaidIDProf.Visible = false;
            // gvAdditionalProviderinfo.Columns[3].Visible = false;
        }
        if (hdnClaimType_Additional.Value == CON.ClaimsType.Institutional)
        {
            divAdditionalMedicaidIDProf.Visible = false;
            //gvAdditionalProviderinfo.Columns[3].Visible = false;
        }
        if (hdnClaimType_Additional.Value == CON.ClaimsType.Professional)
        {
            divAdditionalMedicaidIDProf.Visible = true;
            //gvAdditionalProviderinfo.Columns[3].Visible = true;
        }

        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(hdnClaimIdAdditional.Value))
            {
                GetServiceLineNoFromServiceDetailPanel();
                GetSubmiclaimtProviderType();
                GetAdditionalProviderData(null);
            }
        }
        // btnAddAdditionalProviderinfo.Attributes.Add("onclick", "AdditionalDisableEnableConditionAddButton();");
        lblErrorAdditionalPanal.Text = string.Empty;

        // SetButtonVisibility();
        if (!string.IsNullOrEmpty(hdnAddtionalProviderInfoNPI.Value.ToString()))
        {
            lblAdditionalMedicaidID.Text = hdnAddtionalProviderInfoNPI.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnAdditionalFirstName.Value.ToString()))
        {
            lblProviderFirstName.Text = hdnAdditionalFirstName.Value.ToString();
        }
        if (!string.IsNullOrEmpty(hdnAdditionalLastName.Value.ToString()))
        {
            lblProviderLastName.Text = hdnAdditionalLastName.Value.ToString();
        }
        if(Session["ClaimStatus"] != null)
        {
            if (Session["ClaimStatus"].ToString() == "Pending Submission" && divAdditionalProviderInfo.Visible == true)
            {
                hdnAdditionalClaimStatusDental.Value = "Pending Submission";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>displayDentalAdditionalTable();</script>", false);
            }
        }
    }
    public void SetButtonVisibility()
    {


    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (ddlAdditonalProviderDetail.SelectedIndex < 0 || ddlAdditonalProviderDetail.SelectedIndex == 0)
        {
            GetServiceLineNoFromServiceDetailPanel();
        }
        if (ddlProviderType.SelectedIndex <= 0)
        {
            GetSubmiclaimtProviderType();
        }
        if (hdnClaimType_Additional.Value == CON.ClaimsType.Professional)
        {
            divAdditionalMedicaidIDProf.Visible = true;
        }
        else
        {
            divAdditionalMedicaidIDProf.Visible = false;
        }
    }

    public void ResetAdditionalProviderPanel()
    {
        txtProviderNPI.Text = "";
        ddlAdditonalProviderDetail.ClearSelection();
        ddlProviderType.ClearSelection();
        lblProviderLastName.Text = string.Empty;
        lblProviderFirstName.Text = string.Empty;
        lblProviderMI.Text = string.Empty;
        if (hdnClaimType_Additional.Value == CON.ClaimsType.Professional)
            lblAdditionalMedicaidID.Text = "";
        divAdditionalProviderInfo.InnerHtml = "";
        hdnClaimIdAdditional.Value = "";
        hdnClaimType_Additional.Value = "";
        hdnAddtionalProviderInfoNPI.Value = "";
        hdnAdditionalFirstName.Value = "";
        hdnAdditionalLastName.Value = "";
    }

    public void ClearGrid()
    {
        //gvAdditionalProviderinfo.DataSource = null;
        //gvAdditionalProviderinfo.DataBind();
    }
    protected bool IsAlreadyAddedSameNPIforProviderTypes(string serviceLineNoGrid = "", string providerTypeGrid = "", string textPanelNPI = "")
    {
        bool result = false;
        string billingNPI = BillingNPI;
        string provtypeddl = ddlProviderType.SelectedItem.Text;
        string servLineddl = ddlAdditonalProviderDetail.SelectedItem.Text;
        string npitxt = txtProviderNPI.Text;

        //foreach (GridViewRow rows in gvAdditionalProviderinfo.Rows)
        //{
        //    Label lblProvType = (Label)rows.FindControl("lblProviderType");
        //    Label lblServiceNo = (Label)rows.FindControl("lblServiceLineno");
        //    DropDownList drpdownProviderType = (rows.FindControl("drpdownProviderType") as DropDownList);
        //    DropDownList drpdownServiceLineno = (rows.FindControl("drpdownServiceLineno") as DropDownList);
        //    Label lblProviderNPI = (Label)rows.FindControl("lblProviderNPI");

        //    String drpdownProvider = lblProvType == null ? providerTypeGrid : lblProvType.Text;
        //    String drpdownService = lblServiceNo == null ? serviceLineNoGrid : lblServiceNo.Text;
        //    String NPInumber = lblProviderNPI == null ? textPanelNPI : lblProviderNPI.Text;

        //    // for gridview edit check
        //    if (!string.IsNullOrEmpty(serviceLineNoGrid) && !string.IsNullOrEmpty(providerTypeGrid) && textPanelNPI!=null)
        //    {
        //        provtypeddl = providerTypeGrid;
        //        servLineddl = serviceLineNoGrid;
        //        npitxt = textPanelNPI;
        //    }

        //    if (servLineddl == drpdownService && npitxt == NPInumber &&
        //        ((provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && drpdownProvider == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider) ||
        //        (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider && drpdownProvider == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider)))
        //    {
        //        lblErrorAdditionalPanal.Text = "Supervising provider and Rendering provider cannot be same";
        //        result = true;
        //    }
        //    if (servLineddl == drpdownService && npitxt == NPInumber &&
        //        ((provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon && drpdownProvider == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider) ||
        //        (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider && drpdownProvider == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon)))
        //    {
        //        lblErrorAdditionalPanal.Text = "Assistant surgeon and Supervising provider Cannot be same.";
        //        result = true;
        //    }
        //    if (servLineddl == drpdownService && npitxt == NPInumber &&
        //       ((provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon && drpdownProvider == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider) ||
        //       (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && drpdownProvider == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon)))
        //    {
        //        lblErrorAdditionalPanal.Text = "Assistant surgeon and Rendering provider Cannot be same.";
        //        result = true;
        //    }
        //    if (servLineddl == drpdownService && npitxt == NPInumber &&
        //       ((provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.OperatingPhysician && drpdownProvider == CON.ClaimsAdditionalPanelProviderTypes.OtherOperatingPhysician) ||
        //       (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.OtherOperatingPhysician && drpdownProvider == CON.ClaimsAdditionalPanelProviderTypes.OperatingPhysician)))
        //    {
        //        lblErrorAdditionalPanal.Text = "Operating physician and Other operating physician NPI Cannot be same.";
        //        result = true;
        //    }
        //    if (servLineddl == drpdownService && npitxt == NPInumber &&
        //       ((provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && drpdownProvider == CON.ClaimsAdditionalPanelProviderTypes.ReferringProvider) ||
        //       (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.ReferringProvider && drpdownProvider == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider)))
        //    {
        //        lblErrorAdditionalPanal.Text = "Rendering and Referring Provider NPI Cannot be same.";
        //        result = true;
        //    }
        //    if (servLineddl == drpdownService && npitxt == NPInumber &&
        //       ((provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && drpdownProvider == CON.ClaimsAdditionalPanelProviderTypes.PrimaryCareProvider) ||
        //       (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.PrimaryCareProvider && drpdownProvider == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider)))
        //    {
        //        lblErrorAdditionalPanal.Text = "Rendering Provider Cannot be same as Primary Care Provider";
        //        result = true;
        //    }
        //    if (result)
        //    {
        //        return result;
        //    }
        //    result = ValidateGridandPanelCommonErrorMessage(provtypeddl, servLineddl, npitxt, false, billingNPI);  // Grid Level
        //}
        //for panel level
        result = ValidateGridandPanelCommonErrorMessage(provtypeddl, servLineddl, npitxt, false, billingNPI);
        return result;
    }
    protected bool ValidateGridandPanelCommonErrorMessage(string provtypeddl, string servLineddl, string npitxt, bool result, string billingNPI)
    {
        // Compare primary and referring - // Panel level check
        if (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.PrimaryCareProvider)
        {
            DataTable dtPriRef = FetchAdditionalProviderInformationWithServiceLine(servLineddl).Tables[0];
            DataRow[] refrows = dtPriRef.Select("Provider_Type ='" + CON.ClaimsAdditionalPanelProviderTypes.ReferringProvider + "'");
            if (refrows.Length == 0)
            {
                lblErrorAdditionalPanal.Text = "Primary care provider(referral) cannot be entered without entering referring provider";
                return result = true;
            }
        }
        // compare billing NPI with Rendering
        if ((provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && npitxt == billingNPI))
        {
            lblErrorAdditionalPanal.Text = "Rendering and Billing NPI Cannot be same.";
            result = true;
        }
        //check if header panels already have data
        if (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && (string.IsNullOrEmpty(RenderingProviderNPI) && string.IsNullOrEmpty(RenderingProvider_MedicaidID)))
        {
            lblErrorAdditionalPanal.Text = "Rendering Provider information should be reported in the header.";
            result = true;
        }
        if (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon && (string.IsNullOrEmpty(AssistantProviderNPI) && string.IsNullOrEmpty(Assistant_MedicaidID)))
        {
            lblErrorAdditionalPanal.Text = "Assistant Surgeon Provider information should be reported in the header.";
            result = true;
        }
        if (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider && (string.IsNullOrEmpty(SupervisingProviderNPI) && string.IsNullOrEmpty(SupervisingProvider_Medicaid)))
        {
            lblErrorAdditionalPanal.Text = "Supervising Provider information should be reported in the header.";
            result = true;
        }
        if (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.ServiceFacilityProvider && (string.IsNullOrEmpty(ServiceFacilityProviderNPI) && string.IsNullOrEmpty(ServiceFacilityProvider_Medicaid)))
        {
            lblErrorAdditionalPanal.Text = "Service Facility information should be reported in the header.";
            result = true;
        }
        if (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.ReferringProvider && (string.IsNullOrEmpty(ReferringProviderNPI) && string.IsNullOrEmpty(ReferringProvider_Medicaid)))
        {
            lblErrorAdditionalPanal.Text = "Referring Provider information should be reported in the header.";
            result = true;
        }
        if (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.PrimaryCareProvider && (string.IsNullOrEmpty(PrimaryProviderNPI) && string.IsNullOrEmpty(PrimaryProvider_Medicaid)))
        {
            lblErrorAdditionalPanal.Text = "Primary Care Provider information should be reported in the header.";
            result = true;
        }
        if (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.OperatingPhysician && (string.IsNullOrEmpty(SupervisingProviderNPI) && string.IsNullOrEmpty(SupervisingProvider_Medicaid)))
        {
            lblErrorAdditionalPanal.Text = "Operating Physician Provider information should be reported in the header.";
            result = true;
        }
        if (provtypeddl == CON.ClaimsAdditionalPanelProviderTypes.OtherOperatingPhysician && (string.IsNullOrEmpty(AssistantProviderNPI) && string.IsNullOrEmpty(Assistant_MedicaidID)))
        {
            lblErrorAdditionalPanal.Text = "Other Operating Physician Provider information should be reported in the header.";
            result = true;
        }
        return result;
    }
    protected bool IsAdditionalGridAlreadyAddedSameData(string serviceLineNoGrid = "", string providerTypeGrid = "")
    {
        bool result = false;
        //foreach (GridViewRow rows in gvAdditionalProviderinfo.Rows)
        //{
        //    Label lblProvType = (Label)rows.FindControl("lblProviderType");
        //    Label lblServiceNo = (Label)rows.FindControl("lblServiceLineno");
        //    DropDownList drpdownProviderType = (rows.FindControl("drpdownProviderType") as DropDownList);
        //    DropDownList drpdownServiceLineno = (rows.FindControl("drpdownServiceLineno") as DropDownList);

        //    String drpdownProvider = lblProvType == null ? providerTypeGrid : lblProvType.Text;
        //    String drpdownService = lblServiceNo == null ? serviceLineNoGrid : lblServiceNo.Text;
        //    if (ddlAdditonalProviderDetail.SelectedItem.Text == drpdownService &&
        //        ddlProviderType.SelectedItem.Text == drpdownProvider)
        //    {
        //        result = true;
        //        return result;
        //    }
        //    else
        //    {
        //        result = false;
        //    }

        //    // for gridview check
        //    if (!string.IsNullOrEmpty(serviceLineNoGrid) && !string.IsNullOrEmpty(providerTypeGrid) && lblProvType != null && lblServiceNo != null)
        //    {
        //        if (serviceLineNoGrid == drpdownService && providerTypeGrid == drpdownProvider)
        //        {
        //            result = true;
        //            return result;
        //        }
        //        else
        //        {
        //            result = false;
        //        }
        //    }
        //}
        return result;
    }
    protected void btnAddAdditionalProviderinfo_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_id", hdnClaimIdAdditional.Value);
            lblErrorAdditionalPanal.Text = "";
            DataTable dtNPI = GetData(txtProviderNPI.Text, "", "", "");
            if (!Helper.HasRows(dtNPI))
            {
                GetAdditionalProviderData(null);
                return;
            }
            if (IsAlreadyAddedSameNPIforProviderTypes())
            {
                GetAdditionalProviderData(null);
                return;
            }
            if (IsAdditionalGridAlreadyAddedSameData())
            {
                lblErrorAdditionalPanal.Text = "Same provider type and Service Line number cannot be added again. ";
                return;
            }
            else
            {
                //Inserting on DB table
                if (!string.IsNullOrEmpty(hdnClaimIdAdditional.Value) && !string.IsNullOrEmpty(ddlAdditonalProviderDetail.SelectedItem.Text))
                {
                    parms.Add("Service_Line", ddlAdditonalProviderDetail.SelectedItem.Text.ToString());
                    parms.Add("Provider_Type", ddlProviderType.SelectedItem.Text);
                    parms.Add("Provider_NPI", txtProviderNPI.Text);
                    parms.Add("Last_Name", lblProviderLastName.Text);
                    parms.Add("First_Name", lblProviderFirstName.Text);
                    parms.Add("Middle_Name", lblProviderMI.Text);
                    if (hdnClaimType_Additional.Value == CON.ClaimsType.Professional)
                        parms.Add("Medicaid_ID", lblAdditionalMedicaidID.Text);
                    parms.Add("Last_Modified_Date", null);
                    parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    svc.InsertPanelsData("Claims_Additional_Provider_Information_Service", parms);
                }
            }
            //Displaying binded data from db 
            GetAdditionalProviderData(null);
            ResetAdditionalProviderPanel();
        }
    }
    //protected void gvAdditionalProviderinfo_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (hdnClaimType_Additional.Value == CON.ClaimsType.Dental
    //        || hdnClaimType_Additional.Value == CON.ClaimsType.Institutional)
    //    {
    //        divAdditionalMedicaidIDProf.Visible = false;
    //        gvAdditionalProviderinfo.Columns[3].Visible = false;
    //    }
    //    if (hdnClaimType_Additional.Value == CON.ClaimsType.Professional)
    //    {
    //        divAdditionalMedicaidIDProf.Visible = true;
    //        gvAdditionalProviderinfo.Columns[3].Visible = true;
    //    }

    //    if (e.Row.RowType == DataControlRowType.Header && DisplayReadOnly)
    //    {
    //        e.Row.Cells[6].Visible = !DisplayReadOnly;
    //    }

    //    if (e.Row.RowType == DataControlRowType.DataRow )
    //    {
    //        Button btEdit = (Button)e.Row.Cells[6].FindControl("btnEdit");
    //        Button btDelete = (Button)e.Row.Cells[6].FindControl("btnDelete");
    //        if (btEdit != null)
    //            btEdit.Visible = !DisplayReadOnly;
    //        if (btDelete != null)
    //            btDelete.Visible = !DisplayReadOnly;
    //        if (DisplayReadOnly == true)
    //        {
    //            gvAdditionalProviderinfo.Columns[6].Visible = false;
    //            gvAdditionalProviderinfo.Columns[7].Visible = false;
    //            divAddProv.Visible = false;
    //        }
    //        else if (DisplayReadOnly == false)
    //        {
    //            gvAdditionalProviderinfo.Columns[6].Visible = true;
    //            gvAdditionalProviderinfo.Columns[7].Visible = true;
    //            divAddProv.Visible = true;
    //        }
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //    (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
    //    {
    //        DropDownList drpdownProviderType = (e.Row.FindControl("drpdownProviderType") as DropDownList);
    //        DropDownList drpdownServiceLineno = (e.Row.FindControl("drpdownServiceLineno") as DropDownList);

    //        DataTable dtProvType = GetSubmiclaimtProviderType();
    //        drpdownProviderType.DataSource = dtProvType;
    //        drpdownProviderType.DataTextField = "PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC";
    //        drpdownProviderType.DataValueField = "PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC";
    //        drpdownProviderType.DataBind();

    //        dsAdditionalProviderinfo = new DataSet();
    //        List<SqlParameter> parameters = new List<SqlParameter>();
    //        if (!string.IsNullOrEmpty(hdnClaimIdAdditional.Value))
    //        {
    //            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimIdAdditional.Value, true));
    //            dsAdditionalProviderinfo = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", parameters, "Claims_Service_Details");
    //            var dtServiceline = dsAdditionalProviderinfo.Tables[0];
    //            drpdownServiceLineno.DataSource = dtServiceline;
    //            drpdownServiceLineno.DataTextField = "Service_Line";
    //            drpdownServiceLineno.DataValueField = "Service_Line";
    //            drpdownServiceLineno.DataBind();
    //        }
    //    }



    //}
    //protected void gvAdditionalProviderinfo_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    string keyVal = gvAdditionalProviderinfo.DataKeys[e.RowIndex].Value.ToString();
    //    //string npiNumber = e.NewValues["Provider_NPI"].ToString();
    //    TextBox npiNumber = (TextBox)gvAdditionalProviderinfo.Rows[e.RowIndex].FindControl("txtGridProviderNPI");
    //    string lastname = gvAdditionalProviderinfo.Rows[e.RowIndex].Cells[3].Text.ToString();
    //    string firstname = gvAdditionalProviderinfo.Rows[e.RowIndex].Cells[4].Text.ToString();
    //    string middlename = gvAdditionalProviderinfo.Rows[e.RowIndex].Cells[6].Text.ToString();
    //    lblErrorAdditionalPanal.Text = "";
    //    DataTable dtNPI = GetData(npiNumber.Text, "", "", "");
    //    if (!Helper.HasRows(dtNPI))
    //    {
    //        lblErrorAdditionalPanal.Text = "NPI is not found in the system";
    //        gvAdditionalProviderinfo.EditIndex = -1;
    //        GetAdditionalProviderData(null);
    //        return;
    //    }

    //    DropDownList drpdownServiceLineno = (DropDownList)gvAdditionalProviderinfo.Rows[e.RowIndex].FindControl("drpdownServiceLineno");
    //    DropDownList drpdownProviderType = (DropDownList)gvAdditionalProviderinfo.Rows[e.RowIndex].FindControl("drpdownProviderType");
    //    gvAdditionalProviderinfo.EditIndex = -1;

    //    if (IsAlreadyAddedSameNPIforProviderTypes(drpdownServiceLineno.Text, drpdownProviderType.Text,npiNumber.Text))
    //    {
    //        gvAdditionalProviderinfo.EditIndex = -1;
    //        GetAdditionalProviderData(null);
    //        return;
    //    }
    //    if (IsAdditionalGridAlreadyAddedSameData(drpdownServiceLineno.Text, drpdownProviderType.Text))
    //    {
    //        lblErrorAdditionalPanal.Text = "Same provider type and Service Line number cannot be added again. ";
    //        GetAdditionalProviderData(null);
    //        return;
    //    }

    //    if (!string.IsNullOrEmpty(hdnClaimIdAdditional.Value))
    //    {
    //        Dictionary<string, string> parms = new Dictionary<string, string>();
    //        parms.Add("Claims_Additional_Provider_Information_Service_ID", keyVal);
    //        parms.Add("Claim_id", hdnClaimIdAdditional.Value);
    //        parms.Add("Service_Line", drpdownServiceLineno.SelectedItem.Text);
    //        parms.Add("Provider_Type", drpdownProviderType.SelectedItem.Text);
    //        parms.Add("Provider_NPI", npiNumber.Text);
    //        if (hdnClaimType_Additional.Value == CON.ClaimsType.Professional)
    //            parms.Add("Medicaid_ID", dtNPI.Rows[0].Field<string>("Medicaid_ID"));
    //        parms.Add("Last_Name", dtNPI.Rows[0].Field<string>("Last_or_business_name"));
    //        parms.Add("First_Name", dtNPI.Rows[0].Field<string>("First_Name"));
    //        parms.Add("Middle_Name", middlename.Contains("nbsp") ? " " : middlename);
    //        parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
    //        svc.UpdatePanelsData("Claims_Additional_Provider_Information_Service", parms);

    //    }
    //    GetAdditionalProviderData(null);

    //}

    //protected void gvAdditionalProviderinfo_RowCancelingEdit(object sender, EventArgs e)
    //{
    //    gvAdditionalProviderinfo.EditIndex = -1;
    //    GetAdditionalProviderData(null);
    //}
    //protected void gvAdditionalProviderinfo_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    string keyVal = gvAdditionalProviderinfo.DataKeys[e.NewEditIndex].Value.ToString();
    //    string RowID = gvAdditionalProviderinfo.Rows[e.NewEditIndex].ClientID.Split('_')[5];
    //    hdnAdditionalGridSearchRowID.Value = RowID;

    //    gvAdditionalProviderinfo.EditIndex = e.NewEditIndex;
    //    GridViewRow editingRow = gvAdditionalProviderinfo.Rows[e.NewEditIndex];
    //    DropDownList drpdownProviderType = (editingRow.FindControl("drpdownProviderType") as DropDownList);
    //    DropDownList drpdownServiceLineno = (editingRow.FindControl("drpdownServiceLineno") as DropDownList);

    //    if (drpdownProviderType != null)
    //    {
    //        DataTable dtProvType = GetSubmiclaimtProviderType();
    //        drpdownProviderType.DataSource = dtProvType;
    //        drpdownProviderType.DataTextField = "PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC";
    //        drpdownProviderType.DataValueField = "PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC";
    //        drpdownProviderType.DataBind();

    //        dsAdditionalProviderinfo = new DataSet();
    //        List<SqlParameter> parameters = new List<SqlParameter>();
    //        if (!string.IsNullOrEmpty(hdnClaimIdAdditional.Value))
    //        {
    //            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimIdAdditional.Value, true));
    //            dsAdditionalProviderinfo = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", parameters, "Claims_Service_Details");
    //            var dtServiceLine = dsAdditionalProviderinfo.Tables[0];
    //            drpdownServiceLineno.DataSource = dtServiceLine;
    //            drpdownServiceLineno.DataTextField = "Service_Line";
    //            drpdownServiceLineno.DataValueField = "Service_Line";
    //            drpdownServiceLineno.DataBind();
    //        }

    //    }

    //    GetAdditionalProviderData(null);
    //}

    //protected void gvAdditionalProviderinfo_RowDelete(object source, GridViewDeleteEventArgs e)
    //{
    //    int keyVal = Convert.ToInt32(gvAdditionalProviderinfo.DataKeys[e.RowIndex].Value);
    //    Dictionary<string, string> parms = new Dictionary<string, string>(); try
    //    {
    //        svc.DeletePanelsData("Claims_Additional_Provider_Information_Service", "Claims_Additional_Provider_Information_Service_ID", keyVal);
    //    }
    //    catch (Exception ex) { }
    //    GetAdditionalProviderData(null);
    //}

    protected void txtAdditionalProviderNPI_TextChangedNPI(object sender, EventArgs e)
    {
        if (hdnClaimType_Additional.Value == CON.ClaimsType.Professional)
        {
            DataTable dt = Claims.GetData(txtProviderNPI.Text.Trim(), "", "", "");
            if (Helper.HasRows(dt))
            {
                if (dt.Rows.Count >= 2)
                {

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { $('#myModalpop').modal('show')});</script>", false);

                }
                else
                {
                    Claims.NPITextChanged(txtProviderNPI.Text, lblAdditionalMedicaidID, null, lblProviderFirstName, lblProviderLastName, lblProviderMI, errProviderNPI);
                }
            }
            else
            {
                errProviderNPI.Text = "NPI is Unknown";
                ResetAdditionalProviderPanel();
            }
        }
        else
        {
            DataTable dt = Claims.GetData(txtProviderNPI.Text.Trim(), "", "", "");
            if (Helper.HasRows(dt) && !string.IsNullOrEmpty(txtProviderNPI.Text))
            {

                //  var dte = dt.AsEnumerable().Where(x => x.NPI == null);
                if (dt.Rows.Count >= 2)
                {

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { $('#myModalpop').modal('show')});</script>", false);

                }
                else
                {
                    Claims.NPITextChanged(txtProviderNPI.Text, "", null, lblProviderFirstName, lblProviderLastName, lblProviderMI, errProviderNPI);
                }
            }
            else
            {
                errProviderNPI.Text = !string.IsNullOrEmpty(txtProviderNPI.Text) ? "NPI is Unknown" : "";
                ResetAdditionalProviderPanel();
            }
        }
    }

    private void SetAddtionalProviderPanelData(List<AdditionalProvider> additionalProviders)
    {
        ClaimService.AdditionalProviders = additionalProviders;
        //gvAdditionalProviderinfo.DataSource = additionalProviders;
        //gvAdditionalProviderinfo.DataBind();
    }

    private void SetAdditionalProviders(DataSet dsAdditionalProviders)
    {
        hdnAdditionalClaimStatusDental.Value = "";
        dsAdditionalProviderinfo = new DataSet();
        if (Helper.HasRows(dsAdditionalProviders))
        {
            dsAdditionalProviderinfo = dsAdditionalProviders;
        }
        else
        {
            if (!string.IsNullOrEmpty(hdnClaimIdAdditional.Value))
            {

                dsAdditionalProviderinfo = FetchAdditionalProviderInformation();
            }
        }

        if (Helper.HasRows(dsAdditionalProviderinfo))
        {
            DataTable dtAdditionalProviderInfo = new DataTable();

            dtAdditionalProviderInfo = dsAdditionalProviderinfo.Tables[0];


            if (dtAdditionalProviderInfo.Rows.Count > 0)
            {
                string table = string.Empty;

                if(hdnClaimType_Additional.Value == "2")
                    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Provider Type</th><th style='width:10px; scope='col'>Provider NPI</th><th style='width:10px; scope='col'>Medicaid ID</th><th style='width:10px; scope='col'>Last Name</th><th style='width:10px; scope='col'>First Name</th><th style='width:30px; scope='col'>Middle Name</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                else
                    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Provider Type</th><th style='width:10px; scope='col'>Provider NPI</th><th style='width:10px; scope='col'>Last Name</th><th style='width:10px; scope='col'>First Name</th><th style='width:30px; scope='col'>Middle Name</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                foreach (DataRow dr in dtAdditionalProviderInfo.Rows)
                {
                    string ServiceLine = dr["Service_Line"].ToString();
                    string ProviderType = dr["Provider_Type"].ToString();
                    string ProviderNPI = dr["Provider_NPI"].ToString();                   
                    string LastName = dr["Last_Name"].ToString();
                    string FirstName = dr["First_Name"].ToString();
                    string MiddleName = dr["Middle_Name"].ToString();
                    string ClaimID = hdnClaimIdAdditional.Value;
                    string Claims_Additional_Provider_Information_Service_ID= dr["Claims_Additional_Provider_Information_Service_ID"].ToString();
                    string ClaimType = hdnClaimType_Additional.Value;

                    if (Session["ClaimStatus"] != null)
                    {
                        if (Session["ClaimStatus"].ToString() == "Pending Submission")
                        {
                            if (hdnClaimType_Additional.Value == "2")
                            {
                                string MedicaidID = dr["Medicaid_ID"].ToString();
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='ServiceLine' class='tNumber'>" + ServiceLine + "</span></td><td><span title='ProviderType' class='tNumber'>" + ProviderType + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + ProviderNPI + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + MedicaidID + "</span></td><td><span title='LastName' class='tNumber'>" + LastName + "</span></td><td><span title='FirstName' class='tNumber'>" + FirstName + "</span></td><td><span title='MiddleName' class='tNumber'>" + MiddleName + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + ClaimID + "\",\"" + ClaimType + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + ClaimID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            }
                            else
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='ServiceLine' class='tNumber'>" + ServiceLine + "</span></td><td><span title='ProviderType' class='tNumber'>" + ProviderType + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + ProviderNPI + "</span></td><td><span title='LastName' class='tNumber'>" + LastName + "</span></td><td><span title='FirstName' class='tNumber'>" + FirstName + "</span></td><td><span title='MiddleName' class='tNumber'>" + MiddleName + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + ClaimID + "\",\"" + ClaimType + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + ClaimID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            hdnAdditionalClaimStatusDental.Value = "Pending Submission";
                        }
                        else
                        {
                            if (hdnClaimType_Additional.Value == "2")
                            {
                                string MedicaidID = dr["Medicaid_ID"].ToString();
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='ServiceLine' class='tNumber'>" + ServiceLine + "</span></td><td><span title='ProviderType' class='tNumber'>" + ProviderType + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + ProviderNPI + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + MedicaidID + "</span></td><td><span title='LastName' class='tNumber'>" + LastName + "</span></td><td><span title='FirstName' class='tNumber'>" + FirstName + "</span></td><td><span title='MiddleName' class='tNumber'>" + MiddleName + "</span></td></tr >";
                            }
                            else
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='ServiceLine' class='tNumber'>" + ServiceLine + "</span></td><td><span title='ProviderType' class='tNumber'>" + ProviderType + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + ProviderNPI + "</span></td><td><span title='LastName' class='tNumber'>" + LastName + "</span></td><td><span title='FirstName' class='tNumber'>" + FirstName + "</span></td><td><span title='MiddleName' class='tNumber'>" + MiddleName + "</span></td></tr >";
                            hdnAdditionalClaimStatusDental.Value = "Other";
                        }
                    }
                }
                table = table + "</tbody></table>";
                divAdditionalProviderInfo.InnerHtml = table;
                divAdditionalProviderInfo.Visible = true;

            }
        }
        else
        {
            //gvOccurenceSpanInformation.DataSource = null;
            //gvOccurenceSpanInformation.DataBind();
            divAdditionalProviderInfo.InnerHtml = "";
        }
    }
    

    public void GetAdditionalProviderData(DataSet dsAdditionalInformation)
    {
        DataSet dsAdditionInfo = new DataSet();
        if (!string.IsNullOrEmpty(hdnClaimIdAdditional.Value))
        {

            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_id", hdnClaimIdAdditional.Value);
            DataSet dsAdditional = svc.SelectPanelsData("claims_additional_provider_information_service", parms);

            if (Helper.HasRows(dsAdditional) &&
                Convert.ToInt32(dsAdditional.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimIdAdditional.Value))

                if (Helper.HasRows(dsAdditionalInformation))
                {
                    dsAdditionInfo = dsAdditionalInformation;
                }
                else
                {
                    hdnSequenceidAdditionalPanel.Value = dsAdditional.Tables[0].Rows[0]["Claims_Additional_Provider_Information_Service_ID"].ToString();
                    dsAdditionInfo = FetchAdditionalProviderInformation();
                }
            // gvAdditionalProviderinfo.DataSource = dsAdditionInfo;
        }
        if (Helper.HasRows(dsAdditionInfo))
        {
            dsAdditionInfo.Tables[0].DefaultView.Sort = "Service_Line ASC";
            //gvAdditionalProviderinfo.DataSource = dsAdditionInfo;
            //gvAdditionalProviderinfo.DataBind();
        }
    }

    protected DataSet FetchAdditionalProviderInformation()
    {
        DataSet dsAdditional = new DataSet();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_id", hdnClaimIdAdditional.Value);
        dsAdditionalProviderinfo = dsAdditional = svc.SelectPanelsData("claims_additional_provider_information_service", parms);
        return dsAdditional;
    }
    protected DataSet FetchAdditionalProviderInformationWithServiceLine(string serviceline)
    {
        DataSet dsAdditionalServiceLine = new DataSet();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_id", hdnClaimIdAdditional.Value);
        parms.Add("Service_Line", serviceline);
        dsAdditionalServiceLine = svc.SelectPanelsData("claims_additional_provider_information_filter_serviceline", parms);
        return dsAdditionalServiceLine;
    }
    private void RemoveAlreadyAddedProviderType(DropDownList ddlproviderType, string serviceLine)
    {
        // Removing already added provider for detail/ServiceLine number.        
        DataTable dtAdditional = FetchAdditionalProviderInformationWithServiceLine(serviceLine).Tables[0];

        var Service_Line = dtAdditional.AsEnumerable()
            .Select(row => row.Field<string>("Provider_Type"));
        if (Helper.HasRows(dtAdditional))
        {
            foreach (string row in Service_Line)
            {
                ddlproviderType.Items.Remove(ddlproviderType.Items.FindByText(row));
            }
        }
    }

    private DataTable GetSubmiclaimtProviderType()
    {
        ddlProviderType.Items.Clear();
        DataSet dataSetProvType = svc.GetSubmiclaimtProviderType();
        DataTable dtProvType = dataSetProvType.Tables[0];
        if (hdnClaimType_Additional.Value == CON.ClaimsType.Dental)
        {
            var row_ClaimTypes = from row in dtProvType.AsEnumerable()
                                 where row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 1 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 2 ||
                                 row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 3 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 4
                                 select row;
            DataTable dt_ProviderType_Dental = row_ClaimTypes.CopyToDataTable();
            Helper.LoadList(ddlProviderType, dt_ProviderType_Dental, "PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC", "PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID", true);
            dtProvType = dt_ProviderType_Dental;
        }
        if (hdnClaimType_Additional.Value == CON.ClaimsType.Institutional)
        {
            var row_Institutional = from row in dtProvType.AsEnumerable()
                                    where row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 8 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 9 ||
                                    row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 1 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 5
                                    select row;
            DataTable dt_ProviderType_Institutional = row_Institutional.CopyToDataTable();
            Helper.LoadList(ddlProviderType, dt_ProviderType_Institutional, "PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC", "PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID", true);
            dtProvType = dt_ProviderType_Institutional;
        }
        if (hdnClaimType_Additional.Value == CON.ClaimsType.Professional)
        {
            var row_Professional = from row in dtProvType.AsEnumerable()
                                   where row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 5 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 6 ||
                                   row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 1 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 3 ||
                                   row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 4 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 7
                                   select row;
            DataTable dt_ProviderType_Professional = row_Professional.CopyToDataTable();
            Helper.LoadList(ddlProviderType, dt_ProviderType_Professional, "PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC", "PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID", true);
            dtProvType = dt_ProviderType_Professional;
        }
        if (ddlAdditonalProviderDetail.SelectedIndex > 0)
        {
            RemoveAlreadyAddedProviderType(ddlProviderType, ddlAdditonalProviderDetail.SelectedItem.Text);
        }
        return dtProvType;
    }

    private DataSet GetServiceLineNoFromServiceDetailPanel()
    {
        ddlAdditonalProviderDetail.Items.Clear();
        DataSet dsServicLine = new DataSet();
        if (!string.IsNullOrEmpty(hdnClaimIdAdditional.Value))
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimIdAdditional.Value, true));
            dsServicLine = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", param, "Claims_Service_Details");
            if (Helper.HasRows(dsServicLine) &&
                Convert.ToInt32(dsServicLine.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimIdAdditional.Value))
            {
                DataTable dtServiceLine = dsServicLine.Tables[0];
                //foreach (DataRow row in dtServiceLine.Rows)
                //{
                //    string abc = row["Service_Line"].ToString().Length == 1 ? row["Service_Line"].ToString().PadLeft(2, '0') : row["Service_Line"].ToString();
                //}
                //dtServiceLine.AcceptChanges();
                Helper.LoadList(ddlAdditonalProviderDetail, dsServicLine, "Service_Line", "Claims_Service_Details_ID", true);
        
            }
        }

        return dsServicLine;
    }
    private DataTable GetData(string npi, string medicaidid, string lastName, string firstName)
    {
        DataTable dtNPI = null;
        try
        {
            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                var dsNPI = psc.SearchClaimProviderNPI(npi, medicaidid, lastName, firstName);
                if (dsNPI != null)
                {
                    dtNPI = dsNPI.Tables[0];
                }
            }
        }
        catch
        {
            return dtNPI;
        }
        return dtNPI;
    }

    protected void ddlAdditonalProviderDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSubmiclaimtProviderType();
    }
    public void SaveToDbOnAdjust(DataTable dt)
    {
        if (Helper.HasRows(dt))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claim_id", hdnClaimIdAdditional.Value);
                parms.Add("Service_Line", dt.Rows[i]["Service_Line"].ToString());
                parms.Add("Provider_Type", dt.Rows[i]["Provider_Type"].ToString());
                parms.Add("Provider_NPI", dt.Rows[i]["Provider_NPI"].ToString());

                parms.Add("Last_Name", dt.Rows[i]["Last_Name"].ToString());
                parms.Add("First_Name", dt.Rows[i]["First_Name"].ToString());
                parms.Add("Middle_Name", dt.Rows[i]["Middle_Name"].ToString());
                if (hdnClaimType_Additional.Value == CON.ClaimsType.Professional)
                {
                    parms.Add("Medicaid_ID", dt.Rows[i]["Medicaid_ID"].ToString());
                }
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                try
                {
                    svc.InsertPanelsData("Claims_Additional_Provider_Information_Service", parms);
                }
                catch
                    (Exception ex)
                {

                }

                //DataSet ds = new DataSet();
                //ds.Tables.Add(dt);
                
            }
            SetAdditionalProviders(null);
        }
    }

}