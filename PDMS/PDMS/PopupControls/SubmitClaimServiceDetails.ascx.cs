using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_SubmitClaimServiceDetails : BasePopupControl
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
    protected void Page_Load(object sender, EventArgs e)
    {
        {


            if (!IsPostBack)
            {
                try
                {
                    GetServiceCodeType();
                    //txtApprovedUnitFee.Enabled = false;
                    //txtRequestedUnits.Enabled = false;
                    //txtRequestedUnitFee.Enabled = false;
                    //txtApprovedUnitFee.Enabled = false;

                    //txtServiceCode.Text = "021109W";
                    //txtRequestedFDOS.Text = "05/12/2020";
                    //txtRequestedTDOS.Text = "05/12/2020";
                    //txtStatus.Text = "Pending";
                    //txtRequestedUnits.Text = "";
                    //txtApprovedUnitFee.Text = "";
                    //txtApprovedUnits.Text = "";

                    //txtRequestedUnitFee.Text = "";
                }
                catch (Exception ex)
                {

                }
            }

        }
    }
    public override void LoadData(DataRow dr)
    {
        base.LoadData(dr);
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


    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;
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
        ddlProcedureType.Items.Clear();
        DataSet dataSet = _spa.GetServiceCodeType();
        DataTable dt = dataSet.Tables[0];

        ddlProcedureType.DataSource = dt;
        ddlProcedureType.DataValueField = "PRIOR_AUTH_SERVICE_CODE_ID";
        ddlProcedureType.DataTextField = "PRIOR_AUTH_SERVICE_CODE_DESC";
        ddlProcedureType.DataBind();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        _spa = new PDMSService.PDMSServiceClient();
        this.ValidateData();
        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            return;
        }
        try
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_HOSPITAL_ID", "1");
            var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            //parms.Add("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", ddlServiceTypeCode.SelectedValue.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_CODE", txtServiceCode.Text.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", txtRequestedUnits.Text.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_APPROVED_UNITS", txtApprovedUnits.Text.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", txtRequestedUnitFee.Text.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_APPROVED_UNITS_FEE", txtApprovedUnitFee.Text.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", txtRequestedFDOS.Text.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", txtRequestedTDOS.Text.ToString());
            //parms.Add("PRIOR_AUTH_STATUS_ID", txtStatus.Text.ToString());

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", id.ToString());
            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", id.ToString());
            _spa.InsertPriorAuthServiceDetail("AUTH_SERVICE_DETAIL", parms);


            return;
        }
        catch (Exception ex)
        {

        }

    }
    //public override bool ValidateData()
    public bool ValidateData(string msg)
    {
        //bool isGood = true;
        //RequiredFieldValidator req = new RequiredFieldValidator();
        bool isValid = true;
        if (String.IsNullOrEmpty(ddlProcedureType.SelectedValue))
        {
            AddValidationErrorMessage("*Procedure type is required");
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


    protected void ReportFromDos_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtFromDos.Text, true) && Helper.IsValidDate(txtFromDos.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtFromDos.Text).Subtract(Convert.ToDateTime(txtFromDos.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }
    protected void ReportToDos_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtToDos.Text, true) && Helper.IsValidDate(txtToDos.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtToDos.Text).Subtract(Convert.ToDateTime(txtToDos.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }
    protected void ddlProcedureType_SelectedIndexChanged(object sender, EventArgs e)
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
    protected void btnDelete_Click(object sender, EventArgs e)
    {

    }
    
    protected void gvSubmitClaimServiceDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {


        //BindGrid();
    }
   
    protected void gvSubmitClaimServiceDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {


        //BindGrid();
    }

}