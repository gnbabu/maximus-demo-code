using MAXIMUS.Controllers.PDMS;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class PopupControls_PriorAuthServiceDetails : BasePopupControl
{
    #region spa
    private PDMSService.PDMSServiceClient _spa;
    private PDMSService.PDMSServiceClient spa
    {
        get
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            return _spa;
        }
    }
    #endregion
    public override void LoadData(DataRow dr)
    {
        base.LoadData(dr);
        BindGrid();
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    public string ServiceCode { get { return txtServiceCode.Text; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();

        if (!IsPostBack)
        {
            try
            {
                GetServiceCodeType();
                GetPricingFormula();
                //txtApprovedUnitFee.Enabled = false;
                //txtRequestedUnits.Enabled = false;
                //txtRequestedUnitFee.Enabled = false;
                //txtApprovedUnitFee.Enabled = false;

                //txtServiceCode.Text = "021109W";
                //txtReqFDOS.Text = "05/12/2020";
                //txtRequestedTDOS.Text = "05/12/2020";
                //txtStatus.Text = "Pending";
                //txtRequestedUnits.Text = "";
                //txtApprovedUnitFee.Text = "";
                //txtApprovedUnits.Text = "";

                //txtRequestedUnitFee.Text = "";
            }
            catch
            {

            }
        }

    }
    private void BindGrid()
    {
        //var ds = PriorAuthHospitalController.SelectPriorAuthAttachment(2320);

        //get the data from xml
        var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
        var dataTable = ds.Tables["AuthorizationService"];
        gvPriorAuthServicesDetails.DataSource = dataTable;
        gvPriorAuthServicesDetails.DataBind();
        //ViewState["gridPriorAuthServicesDetails"] = dataTable;
    }

    private void AddNewRow()
    {

       // DataTable dt = new DataTable("AuthorizationService");
        DataTable dt = new DataTable();
        dt.Columns.Add("DetailLineNumber", typeof(System.String));
        dt.Columns.Add("TypeOfServiceCode",typeof(System.String));
        dt.Columns.Add("ServiceCode", typeof(System.String));
        dt.Columns.Add("RequestedUnits", typeof(System.String));
        dt.Columns.Add("AuthorizedUnits", typeof(System.String));
        dt.Columns.Add("RequestedDollars", typeof(System.String));
        dt.Columns.Add("ServiceStartDate", typeof(System.String));
        dt.Columns.Add("ServiceEndDate", typeof(System.String));
        //extra 
        dt.Columns.Add("RenderingProviderID", typeof(System.String));
        dt.Columns.Add("ReferredProviderID", typeof(System.String));
        dt.Columns.Add("PriorAuthorizationFrequencyCode", typeof(System.String));
        dt.Columns.Add("CaloriesPerDayNum", typeof(System.String));
        dt.Columns.Add("ProcedureCode", typeof(System.String));
        dt.Columns.Add("QuantityUsedAmount", typeof(System.String));
        dt.Columns.Add("QuantityUsedUnits", typeof(System.String));
        dt.Columns.Add("FromProcedureCode", typeof(System.String));
        dt.Columns.Add("DaysNum", typeof(System.String));
        dt.Columns.Add("ThruService", typeof(System.String));
        dt.Columns.Add("ListPrice", typeof(System.String));
        dt.Columns.Add("ToProcedureCode", typeof(System.String));
        dt.Columns.Add("ProcedureTypeCode", typeof(System.String));
        dt.Columns.Add("ICDType", typeof(System.String));
        dt.Columns.Add("ServiceLimit", typeof(System.String));
        dt.Columns.Add("LimitAmount", typeof(System.String));
        dt.Columns.Add("RateAmount", typeof(System.String));
        dt.Columns.Add("ProcModifierCode", typeof(System.String));
        dt.Columns.Add("ProcModifierCode1", typeof(System.String));
        dt.Columns.Add("ProcModifierCode2", typeof(System.String));
        dt.Columns.Add("ProcModifierCode3", typeof(System.String));
        dt.Columns.Add("ToothSurfaceCode", typeof(System.String));
        dt.Columns.Add("ToothNumCode", typeof(System.String));
        dt.Columns.Add("ServiceStatusCode", typeof(System.String));
        dt.Columns.Add("ServiceStatusReasonCode", typeof(System.String));
        dt.Columns.Add("ServicesUsed", typeof(System.String));
        dt.Columns.Add("AmountUsed", typeof(System.String));
       
        dt.Columns.Add("FirstDiagnosisCode", typeof(System.String));
        dt.Columns.Add("LastDiagnosisCode", typeof(System.String));
        dt.Columns.Add("BillDirectFromDate", typeof(System.String));
        dt.Columns.Add("BillDirectToDate", typeof(System.String));
        dt.Columns.Add("ToothExtractionDate", typeof(System.String));
        dt.Columns.Add("InitialPlacement", typeof(System.String));
        dt.Columns.Add("PricingFormula", typeof(System.String));
        dt.Columns.Add("PriorPlacement", typeof(System.String));
        dt.Columns.Add("ToothQuadrant", typeof(System.String));
        dt.Columns.Add("RequestedEffectiveDate", typeof(System.String));
        dt.Columns.Add("NDCCode", typeof(System.String));
        dt.Columns.Add("InpatientProcedure", typeof(System.String));
        dt.Columns.Add("RevenueCode", typeof(System.String));
        dt.Columns.Add("RecordStatusCode", typeof(System.String));
        DataRow dr = dt.NewRow();
        dr["TypeOfServiceCode"] = ddlServiceTypeCode.SelectedValue;
        //dt.Rows.Add();
        dr["ServiceCode"] = txtServiceCode.Text;
        dr["RequestedUnits"] = txtRequestUnt.Text;
        dr["AuthorizedUnits"] = txtAuthorizedUnits.Text;
        dr["RequestedDollars"] = txtRequestedDollars.Text;
        dr["ServiceStartDate"] = txtservicerenFD.Text;
        dr["ServiceEndDate"] = txtservicerenTO.Text;
        dr["BillDirectFromDate"] = txtBilleddirFD.Text;
        dr["BillDirectToDate"] = txtBilleddirTD.Text;
        
        dt.Rows.Add(dr);
        try
        {
            var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
            ds.Tables["AuthorizationService"].Rows.Add(dt);
            //ds.Tables.Add(dt);
            ds.AcceptChanges();

            //string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
            //ds.ReadXml(templateActualPath + @"/SubmitPriorAuthRequestResponse.xml");
            ds.WriteXml(@"C:\Tem\Package5\PDMS\PDMS\ProviderDataManagementSystemService\Documents\SubmitPriorAuthRequestResponse.xml");
        }
        catch
        {
            throw;
        }
        
        BindGrid();
       
    }
    private void SaveData()
    {
        var Id = Guid.NewGuid().ToString();
        string path = @"C:\Tem\Package5\PDMS\PDMS\ProviderDataManagementSystemService\Documents\SubmitPriorAuthRequestResponse.xml";
        //string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
        //ds.ReadXml(templateActualPath + @"/SubmitPriorAuthRequestResponse.xml");
      //  ds.WriteXml(@"C:\Tem\Package5\PDMS\PDMS\ProviderDataManagementSystemService\Documents\SubmitPriorAuthRequestResponse.xml");
        XElement xelement = XElement.Load(path);
        var auth = xelement.Descendants("Authorization").ElementAt(0);
        auth.Add(new XElement("AuthorizationService",
                    new XElement("DetailLineNumber", Id),
                    new XElement("ServiceCode",txtServiceCode.Text),
                    new XElement("RequestedUnits", txtRequestUnt.Text),
                     new XElement("AuthorizedUnits", txtAuthorizedUnits.Text),
                    new XElement("RequestedDollars", txtRequestedDollars.Text),
                    new XElement("ServiceStartDate", txtservicerenFD.Text),
                     new XElement("ServiceEndDate", txtservicerenTO.Text),
                    new XElement("RenderingProviderID", ""),
                    new XElement("ReferredProviderID", ""),
                     new XElement("PriorAuthorizationFrequencyCode", ""),
                    new XElement("CaloriesPerDayNum", ""),
                     new XElement("ProcedureCode", ""),
                    new XElement("QuantityUsedAmount", ""),
                     new XElement("QuantityUsedUnits", ""),
                     new XElement("DaysNum", ""),
                    new XElement("ThruService", ""),
                     new XElement("ListPrice", ""),
                    new XElement("ToProcedureCode", ""),
                    new XElement("ProcedureTypeCode", ""),
                    new XElement("ICDType", ""),
                    new XElement("LimitAmount", ""),
                    new XElement("RateAmount", ""),
                    new XElement("ProcModifierCode", ""),
                   new XElement("ProcModifierCode1", ""),
                    new XElement("ProcModifierCode2", ""),
                    new XElement("ProcModifierCode3", ""),
                    new XElement("ToothSurfaceCode", ""),
                    new XElement("ToothNumCode", ""),
                    new XElement("ServiceStatusCode", ""),
                   new XElement("ServiceStatusReasonCode", ""),
                    new XElement("ServicesUsed", ""),
                    new XElement("AmountUsed", ""),
                    new XElement("FirstDiagnosisCode", ""),
                    new XElement("LastDiagnosisCode", ""),
                      new XElement("AuthorizedDollars", ""),
                   
                    new XElement("BillDirectFromDate", txtBilleddirFD.Text),
                      new XElement("BillDirectToDate", txtBilleddirTD.Text),
                    new XElement("ToothExtractionDate", ""),
                      new XElement("PricingFormula", ""),
                    new XElement("InitialPlacement", ""),
                      new XElement("PriorPlacement", ""),
                    new XElement("ToothQuadrant", ""),
                    new XElement("RequestedEffectiveDate", ""),
                    new XElement("NDCCode", ""),
                    new XElement("InpatientProcedure", ""),
                    new XElement("RevenueCode", ""),
                    new XElement("RecordStatusCode", "")));

        xelement.Save(path);
        //txtName.Text = string.Empty;
        //txtAddress.Text = string.Empty;
        //txtAge.Text = string.Empty;
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


    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;


    /// <summary>
    /// This Method will get Service Code Type
    /// </summary>
    private void GetServiceCodeType()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        ddlServiceTypeCode.Items.Clear();
        DataSet dataSet = _spa.GetServiceCodeType();
        DataTable dt = dataSet.Tables[0];

        ddlServiceTypeCode.DataSource = dt;
        ddlServiceTypeCode.DataValueField = "PRIOR_AUTH_SERVICE_CODE_ID";
        ddlServiceTypeCode.DataTextField = "PRIOR_AUTH_SERVICE_CODE_DESC";
        ddlServiceTypeCode.DataBind();
    }

    /// <summary>
    /// This Method will getPricingFormula
    /// </summary>
    private void GetPricingFormula()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        ddlServiceTypeCode.Items.Clear();
        DataSet dataSet = _spa.GetPricingFormula();
        DataTable dt = dataSet.Tables[0];
        Helper.LoadList(ddlPricingFormula, dt, "PRIOR_AUTH_SERVICE_PRICING_FORMULA_DESC", "PRIOR_AUTH_SERVICE_PRICING_FORMULA_ID", true);
        //ddlPricingFormula.DataSource = dt;
        //ddlPricingFormula.DataValueField = "PRIOR_AUTH_SERVICE_PRICING_FORMULA_ID";
        //ddlPricingFormula.DataTextField = "PRIOR_AUTH_SERVICE_PRICING_FORMULA_DESC";
        //ddlPricingFormula.DataBind();
    }
    protected void serviceAdd_Click(object sender, EventArgs e)
    {
       
       
       // AddNewRow();
        SaveData();
        BindGrid();


    }
    protected void gvPriorAuthServicesDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPriorAuthServicesDetails.PageIndex = e.NewPageIndex;
        BindGrid();
        gvPriorAuthServicesDetails.EditIndex = -1;
    }

    protected void gvPriorAuthServicesDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        

        //BindGrid();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hello", "showPopup();", true);
        Button btn = (Button)sender;
        string cmdArgument = btn.CommandArgument;
        var id = Convert.ToString(btn.CommandArgument);
        DeleteData(id);
        BindGrid();
    }
    

    private void DeleteData(string id)
    {
        string path = @"C:\Tem\Package5\PDMS\PDMS\ProviderDataManagementSystemService\Documents\SubmitPriorAuthRequestResponse.xml";
        //string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
        //ds.ReadXml(templateActualPath + @"/SubmitPriorAuthRequestResponse.xml");
        XElement xelement = XElement.Load(path);
        var auth = xelement.Descendants("Authorization").ElementAt(0);
        var Servicenode = from AddauthServices in auth.Elements("AuthorizationService")
                           where (string)AddauthServices.Element("DetailLineNumber").Value == id
                           select AddauthServices;
        Servicenode.Remove();
        xelement.Save(path);
    }
    protected void lnkServiceCodeSearch_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    _spa = new PDMSService.PDMSServiceClient();
    //    this.ValidateData();
    //    if (!Page.IsValid)
    //    {
    //        if (ValidationEvent != null)
    //        {
    //            ValidationEvent();
    //        }
    //        return;
    //    }
    //    try
    //    {
    //        Dictionary<string, string> parms = new Dictionary<string, string>();
    //        parms.Add("PRIOR_AUTH_HOSPITAL_ID", "1");
    //        var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
    //        parms.Add("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", ddlServiceTypeCode.SelectedValue.ToString());
    //        parms.Add("PRIOR_AUTH_SERVICE_DETAIL_CODE", txtServiceCode.Text.ToString());
    //        parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", txtRequestUnt.Text.ToString());
    //        //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_APPROVED_UNITS", txtApprovedUnits.Text.ToString());
    //        //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", txtRequestedUnitFee.Text.ToString());
    //        //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_APPROVED_UNITS_FEE", txtApprovedUnitFee.Text.ToString());
    //        parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", txtReqFDOS.Text.ToString());
    //        parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", txtReqTDOS.Text.ToString());
    //        //parms.Add("PRIOR_AUTH_STATUS_ID", txtStatus.Text.ToString());

    //        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
    //        parms.Add("LAST_MODIFIED_USER", id.ToString());
    //        parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
    //        parms.Add("Created_By_User", id.ToString());
    //        _spa.InsertPriorAuthServiceDetail("AUTH_SERVICE_DETAIL", parms);


    //        return;
    //    }
    //    catch (Exception ex)
    //    {

    //    }

    //}
    //public override bool ValidateData()
    public bool ValidateData(string msg)
    {
        //bool isGood = true;
        //RequiredFieldValidator req = new RequiredFieldValidator();
        bool isValid = true;
        if (String.IsNullOrEmpty(ddlServiceTypeCode.SelectedValue))
        {
            AddValidationErrorMessage("*Service Type Code is required for detail N");
            isValid = false;
        }
        if (String.IsNullOrEmpty(ddlPricingFormula.SelectedValue))
        {
            AddValidationErrorMessage("*Pricing Formula is required ");
            isValid = false;
        }
        if (String.IsNullOrEmpty(txtNumberdays.Text))
        {
            AddValidationErrorMessage("*Number of Days ");
            isValid = false;
        }
        if (String.IsNullOrEmpty(txtServiceCode.Text))
        {
            AddValidationErrorMessage("*Service code is required for detail N");
            isValid = false;

        }
        if (String.IsNullOrEmpty(txtReqFDOS.Text))
        {
            AddValidationErrorMessage("*Does not allow smaller date than today");
            isValid = false;
        }
        if (String.IsNullOrEmpty(txtReqTDOS.Text))
        {
            AddValidationErrorMessage("*Does not allow smaller date than today");
            isValid = false;
        }
        return isValid;
    }


    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        //isGood = false;
        return false;
    }
    private void ValidateData()
    {

    }
    protected void ddlServiceTypeCode_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlProcedureType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlReferralEPSDT_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlunitofMeasurment2_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlPricingFormula_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    


    protected void lnkProcedureCodeSearch_Click(object sender, EventArgs e)
    {

    }
    protected void lnkRevenueCodeSearch_Click(object sender, EventArgs e)
    {

    }
    protected void lnkProfessionalProcedureCodeSearch_Click(object sender, EventArgs e)
    {

    }


    protected void lnkPlaceOfServiceSearch_Click(object sender, EventArgs e)
    {

    }



    //protected void ReporteDateofservice_ServerValidate(object source, ServerValidateEventArgs args)
    //{
    //    if (Helper.IsValidDate(txtDateofservice.Text, true) && Helper.IsValidDate(txtDateofservice.Text, true))
    //    {
    //        TimeSpan ts = Convert.ToDateTime(txtDateofservice.Text).Subtract(Convert.ToDateTime(txtDateofservice.Text));
    //        args.IsValid = (ts.Days < 366 && ts.Days > -366);
    //    }
    //    else
    //        args.IsValid = true;

    //    if (!args.IsValid)
    //    {

    //    }
    //}





}