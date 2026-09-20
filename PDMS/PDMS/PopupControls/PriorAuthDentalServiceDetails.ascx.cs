using MAXIMUS.Controllers.PDMS;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class PopupControls_PriorAuthDentalServiceDetails : BasePopupControl
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
        BindGrid();
        base.LoadData(dr);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();
        //txtdentalLine.Enabled = false;

        //txtDentalApprovedunit.Text = "";
        //txtDentalReqUntFees.Text = "29.93";

        //txtDentalAppUnitFee.Text = "";
        //txtDentalRequestUnt.Text = "01";
        //txtDentalReqFDOS.Text = "05/12/2020";
        //txtdentalStatus.Text = "Pending";
        //txtHCPS.Text = "D723";
        //txtQuadrant.Text = "04";
        //txtDentalReqTDOS.Text = "05/12/2020";
        //txtDentalAppUnitFee.Text = "";

    }
  
    private void BindGrid()
    {
        //var ds = PriorAuthHospitalController.SelectPriorAuthAttachment(2320);

        //get the data from xml
        var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
        var dataTable = ds.Tables["AuthorizationService"];
        gvPriorAuthDentalServicesDetails.DataSource = dataTable;
        gvPriorAuthDentalServicesDetails.DataBind();
      //  ViewState["gridPriorAuthDentalServicesDetails"] = dataTable;
    }
    //private void AddNewRow()
    //{
    //    if (ViewState["gridPriorAuthDentalServicesDetails"] != null)
    //    {
    //        DataTable dt = (DataTable)ViewState["gridPriorAuthDentalServicesDetails"];
    //        DataRow dr = dt.NewRow();
    //        if (dt.Rows.Count > 0)
    //        {
    //            dr["TypeOfServiceCode"] = ddlServiceCodeType.SelectedValue;
    //            dr["ToothNumCode"] = txttoothNumber.Text;
    //            dr["ToothQuadrant"] = txtQuadrant.Text;
    //            dr["RequestedUnits"] = txtDentalRequestUnt.Text;
    //            dr["AuthorizedUnits"] = txtAuthorizedUnits.Text;
    //            dr["RequestedDollars"] = txtRequestedDollars.Text;
    //            //dr["ServiceStartDate"] = txtservicerenFD.Text;
    //            //dr["ServiceEndDate"] = txtservicerenTO.Text;
    //            dr["ServiceStatusCode"] = txtdentalStatus.Text;
    //         //   dr["AmountUsed"] = txtServiceCode.Text;
    //            dt.Rows.Add(dr);
    //            gvPriorAuthDentalServicesDetails.DataSource = dt;
    //            gvPriorAuthDentalServicesDetails.DataBind();
    //            ViewState["gridPriorAuthDentalServicesDetails"] = dt;

    //        }
    //    }
    //    else
    //    {
    //        DataTable dt = new DataTable();
    //        DataRow dr = dt.NewRow();
    //        dr["TypeOfServiceCode"] = ddlServiceCodeType.SelectedValue;
    //        dr["ToothNumCode"] = txttoothNumber.Text;
    //        dr["ToothQuadrant"] = txtQuadrant.Text;
    //        dr["RequestedUnits"] = txtDentalRequestUnt.Text;
    //        dr["AuthorizedUnits"] = txtAuthorizedUnits.Text;
    //        dr["RequestedDollars"] = txtRequestedDollars.Text;
    //        //dr["ServiceStartDate"] = txtservicerenFD.Text;
    //        //dr["ServiceEndDate"] = txtservicerenTO.Text;
    //        dr["ServiceStatusCode"] = txtdentalStatus.Text;
    //        //   dr["AmountUsed"] = txtServiceCode.Text;
    //        dt.Rows.Add(dr);
    //        gvPriorAuthDentalServicesDetails.DataSource = dt;
    //        gvPriorAuthDentalServicesDetails.DataBind();
    //        ViewState["gridPriorAuthDentalServicesDetails"] = dt;
    //    }
    //}

    private void AddNewRow()
    {

        // DataTable dt = new DataTable("AuthorizationService");
        DataTable dt = new DataTable();
        dt.Columns.Add("DetailLineNumber", typeof(System.String));
        dt.Columns.Add("TypeOfServiceCode", typeof(System.String));
        dt.Columns.Add("ToothNumCode", typeof(System.String));
        dt.Columns.Add("ToothQuadrant", typeof(System.String));
        dt.Columns.Add("AuthorizedUnits", typeof(System.String));
        dt.Columns.Add("RequestedDollars", typeof(System.String));
        dt.Columns.Add("ServiceStartDate", typeof(System.String));
        dt.Columns.Add("ServiceEndDate", typeof(System.String));
        dt.Columns.Add("RequestedUnits", typeof(System.String));
        dt.Columns.Add("ServiceStatusCode", typeof(System.String));
        dt.Columns.Add("AmountUsed", typeof(System.String));
        dt.Columns.Add("ServiceCode", typeof(System.String));
        //extra 
        dt.Columns.Add("RenderingProviderID", typeof(System.String));
        dt.Columns.Add("ReferredProviderID", typeof(System.String));
        dt.Columns.Add("PriorAuthorizationFrequencyCode", typeof(System.String));
        //dt.Columns.Add("CaloriesPerDayNum", typeof(System.String));
        dt.Columns.Add("ProcedureCode", typeof(System.String));
        dt.Columns.Add("QuantityUsedAmount", typeof(System.String));
        dt.Columns.Add("QuantityUsedUnits", typeof(System.String));
        dt.Columns.Add("FromProcedureCode", typeof(System.String));
        dt.Columns.Add("DaysNum", typeof(System.String));
        dt.Columns.Add("ThruService", typeof(System.String));
        //   dt.Columns.Add("PriorAuthorizationFrequencyCode", typeof(System.String));
        dt.Columns.Add("ListPrice", typeof(System.String));
        dt.Columns.Add("ToProcedureCode", typeof(System.String));
        dt.Columns.Add("ProcedureTypeCode", typeof(System.String));
        dt.Columns.Add("ICDType", typeof(System.String));
        dt.Columns.Add("ServiceLimit", typeof(System.String));
        dt.Columns.Add("LimitAmount", typeof(System.String));
        dt.Columns.Add("RateAmount", typeof(System.String));
        //dt.Columns.Add("ServiceStartDate", typeof(System.String));
        //dt.Columns.Add("ServiceEndDate", typeof(System.String));
        dt.Columns.Add("ProcModifierCode", typeof(System.String));
        dt.Columns.Add("ProcModifierCode1", typeof(System.String));
        dt.Columns.Add("ProcModifierCode2", typeof(System.String));
        dt.Columns.Add("ProcModifierCode3", typeof(System.String));
        dt.Columns.Add("ToothSurfaceCode", typeof(System.String));
        
       
        dt.Columns.Add("ServiceStatusReasonCode", typeof(System.String));
        dt.Columns.Add("ServicesUsed", typeof(System.String));
       

        dt.Columns.Add("FirstDiagnosisCode", typeof(System.String));
        dt.Columns.Add("LastDiagnosisCode", typeof(System.String));
        //dt.Columns.Add("AuthorizedDollars", typeof(System.String));
        //dt.Columns.Add("RequestedDollars", typeof(System.String));
        dt.Columns.Add("BillDirectFromDate", typeof(System.String));
        dt.Columns.Add("BillDirectToDate", typeof(System.String));
        dt.Columns.Add("ToothExtractionDate", typeof(System.String));
        dt.Columns.Add("InitialPlacement", typeof(System.String));
        dt.Columns.Add("PricingFormula", typeof(System.String));
        dt.Columns.Add("PriorPlacement", typeof(System.String));
       
        dt.Columns.Add("RequestedEffectiveDate", typeof(System.String));
        dt.Columns.Add("NDCCode", typeof(System.String));
        dt.Columns.Add("InpatientProcedure", typeof(System.String));
        dt.Columns.Add("RevenueCode", typeof(System.String));
        dt.Columns.Add("RecordStatusCode", typeof(System.String));
        DataRow dr = dt.NewRow();
        dr["TypeOfServiceCode"] = ddlServiceCodeType.SelectedValue;
        //dt.Rows.Add();
        dr["ServiceCode"] = txtServicecode.Text;
        dr["RequestedUnits"] = txtDentalRequestUnt.Text;
        dr["AuthorizedUnits"] = txtAuthorizedUnits.Text;
        dr["RequestedDollars"] = txtRequestedDollars.Text;
        dr["ServiceStartDate"] = txtDentalReqFDOS.Text;
        dr["ServiceEndDate"] = txtDentalReqTDOS.Text;
        dr["ToothNumCode"] = txttoothNumber.Text;
        dr["RecordStatusCode"] = txtdentalStatus.Text;
        dr["PriorAuthorizationFrequencyCode"] = txtdentalAssociatedPANumber.Text;

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
        catch (Exception ex)
        {
            throw ex;
        }

        BindGrid();

    }
    private void SaveData()
    {
        var Id = Guid.NewGuid().ToString();
        string path = @"C:\Tem\Package5\PDMS\PDMS\ProviderDataManagementSystemService\Documents\SubmitPriorAuthRequestResponse.xml";
        //string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
        //ds.ReadXml(templateActualPath + @"/SubmitPriorAuthRequestResponse.xml");
       
        XElement xelement = XElement.Load(path);
        var auth = xelement.Descendants("Authorization").ElementAt(0);
        auth.Add(new XElement("AuthorizationService",
                    new XElement("DetailLineNumber", Id),
                    new XElement("ServiceCode", txtServicecode.Text),
                    new XElement("RequestedUnits", txtDentalRequestUnt.Text),
                     new XElement("AuthorizedUnits", txtAuthorizedUnits.Text),
                    new XElement("RequestedDollars", txtRequestedDollars.Text),
                    new XElement("ServiceStartDate", txtDentalReqFDOS.Text),
                     new XElement("ServiceEndDate", txtDentalReqTDOS.Text),
                    new XElement("RenderingProviderID", ""),
                    new XElement("ReferredProviderID", ""),
                     new XElement("PriorAuthorizationFrequencyCode", txtdentalAssociatedPANumber.Text),
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
                    new XElement("ToothNumCode", txttoothNumber.Text),
                    new XElement("ServiceStatusCode", ""),
                   new XElement("ServiceStatusReasonCode", ""),
                    new XElement("ServicesUsed", ""),
                    new XElement("AmountUsed", ""),
                    new XElement("FirstDiagnosisCode", ""),
                    new XElement("LastDiagnosisCode", ""),
                      new XElement("AuthorizedDollars", ""),

                    new XElement("BillDirectFromDate", ""),
                      new XElement("BillDirectToDate", ""),
                    new XElement("ToothExtractionDate", ""),
                      new XElement("PricingFormula", ""),
                    new XElement("InitialPlacement", ""),
                      new XElement("PriorPlacement", ""),
                    new XElement("ToothQuadrant", ""),
                    new XElement("RequestedEffectiveDate", ""),
                    new XElement("NDCCode", ""),
                    new XElement("InpatientProcedure", ""),
                    new XElement("RevenueCode", ""),
                    new XElement("RecordStatusCode", txtdentalStatus.Text)));

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



    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    protected void btnCancel_Click(object sender, EventArgs e)
    {

        if (CancelEvent != null)
            CancelEvent();
    }

    protected void DentserviceAdd_Click(object sender, EventArgs e)
    {
        // AddNewRow();
        SaveData();
        BindGrid();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
       
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
    protected void gvPriorAuthDentalServicesDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPriorAuthDentalServicesDetails.PageIndex = e.NewPageIndex;
        BindGrid();
        gvPriorAuthDentalServicesDetails.EditIndex = -1;
    }

    protected void gvPriorAuthDentalServicesDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
       

        //BindGrid();
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
    //        object hospitalId = null;
    //        parms.Add("PRIOR_AUTH_HOSPITAL_ID", hospitalId.ToString());
    //        //parms.Add("PRIOR_AUTH_HOSPITAL_ID", "1");
    //        var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
    //        parms.Add("PRIOR_AUTH_SERVICE_DETAIL_CODE", "");
    //        parms.Add("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", "");
    //        parms.Add("PRIOR_AUTH_SERVICE_DETAIL_HCPCS_CODE", ddlServiceCodeType.Text.ToString());
    //        parms.Add("PRIOR_AUTH_SERVICE_DETAIL_QUADRANT", txtQuadrant.Text.ToString());
    //        parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", txtDentalRequestUnt.Text.ToString());
    //        //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_APPROVED_UNITS", txtDentalApprovedunit.Text.ToString());
    //        //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", txtDentalRequestUnt.Text.ToString());
    //        //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_APPROVED_UNITS_FEE", txtDentalAppUnitFee.Text.ToString());
    //        //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", txtDentalReqFDOS.Text.ToString());
    //        //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", txtDentalReqTDOS.Text.ToString());
    //        //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_TOTAL_FEES", txtDentalAppUnitFee.Text.ToString());
    //        parms.Add("PRIOR_AUTH_STATUS_ID", txtdentalStatus.Text.ToString());

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
        if (String.IsNullOrEmpty(ddlServiceCodeType.Text))
        {
            AddValidationErrorMessage("*HCPS Code is required for detail N");
            isValid = false;
        }
        if (String.IsNullOrEmpty(txttoothNumber.Text))
        {
            AddValidationErrorMessage("*Incorrect tooth number");
            isValid = false;

        }
        if (String.IsNullOrEmpty(txtDentalRequestUnt.Text))
        {
            AddValidationErrorMessage("*Requested Unit");
            isValid = false;

        }
        if (String.IsNullOrEmpty(txtDentalReqFDOS.Text))
        {
            AddValidationErrorMessage("*Does not allow smaller date than today");
            isValid = false;
        }
        if (String.IsNullOrEmpty(txtDentalReqTDOS.Text))
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
    protected void lnkProcedureCodeSearch_Click(object sender, EventArgs e)
    {

    }
    protected void ddlServiceCodeType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void RequestedFDOS_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtDentalReqFDOS.Text, true) && Helper.IsValidDate(txtDentalReqFDOS.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtDentalReqFDOS.Text).Subtract(Convert.ToDateTime(txtDentalReqFDOS.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }
    protected void RequestedTDOS_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtDentalReqTDOS.Text, true) && Helper.IsValidDate(txtDentalReqTDOS.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtDentalReqTDOS.Text).Subtract(Convert.ToDateTime(txtDentalReqTDOS.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }
    //protected void ReportDateofService_ServerValidate(object source, ServerValidateEventArgs args)
    //{
    //    if (Helper.IsValidDate(txtDateofService.Text, true) && Helper.IsValidDate(txtDateofService.Text, true))
    //    {
    //        TimeSpan ts = Convert.ToDateTime(txtDateofService.Text).Subtract(Convert.ToDateTime(txtDateofService.Text));
    //        args.IsValid = (ts.Days < 366 && ts.Days > -366);
    //    }
    //    else
    //        args.IsValid = true;

    //    if (!args.IsValid)
    //    {

    //    }
    //}
    //protected void ReportPaidDate_ServerValidate(object source, ServerValidateEventArgs args)
    //{
    //    if (Helper.IsValidDate(txtPaidDate.Text, true) && Helper.IsValidDate(txtPaidDate.Text, true))
    //    {
    //        TimeSpan ts = Convert.ToDateTime(txtPaidDate.Text).Subtract(Convert.ToDateTime(txtPaidDate.Text));
    //        args.IsValid = (ts.Days < 366 && ts.Days > -366);
    //    }
    //    else
    //        args.IsValid = true;

    //    if (!args.IsValid)
    //    {

    //    }
    //}


    protected void lnkHcpsSearch_Click(object sender, EventArgs e)
    {

    }


}