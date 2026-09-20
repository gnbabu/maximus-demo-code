using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_ClaimProfessionalServiceDetails : BasePopupControl
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
    //public override void LoadData(DataRow dr)
    //{
    //    BindGrid();
    //    base.LoadData(dr);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();

    }
    protected void ddlProffReferralEPSDTService_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlproffUnitsMea_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    private void BindGrid()
    {
        //var ds = PriorAuthHospitalController.SelectPriorAuthAttachment(2320);

        //get the data from xml
        //var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
        //var dataTable = ds.Tables["AuthorizationService"];
        //gvClaimDentalServicesDetails.DataSource = dataTable;
        //gvClaimDentalServicesDetails.DataBind();
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


    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    protected void btnCancel_Click(object sender, EventArgs e)
    {

        if (CancelEvent != null)
            CancelEvent();
    }
    protected void ClaimDentserviceAdd_Click(object sender, EventArgs e)
    {
        ValidateData();


    }
    protected void gvClaimProfessionalServiceDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {


        BindGrid();
    }


    protected void ClaimProfessionalServiceDetailsAdd_Click(object sender, EventArgs e)
    {
        //var ds = PriorAuthHospitalController.SelectPriorAuthAttachment(2320);
        //var dt = ds.Tables["Attachment"];
        //var dr = dt.NewRow();
        //var fileBytes = PriorAttachmentUpload.FileBytes;
        //var docType = ddlDocumentType.SelectedValue;
        //var note = txtNote.Text.Trim();
        //dr["PRIOR_AUTH_DOCUMENT_ID"] = "2122222222";
        //dr["PRIOR_AUTH_DOCUMENT_TYPE_ID"] = docType;
        //dr["PRIOR_AUTH_Note"] = note;
        //dr["PRIOR_AUTH_HOSPITAL_LINE"] = "12344";
        //dt.Rows.Add(dr);
        BindGrid();
    }
    protected void gvClaimProfessionalServiceDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvClaimProfessionalServiceDetails.PageIndex = e.NewPageIndex;
        BindGrid();
        gvClaimProfessionalServiceDetails.EditIndex = -1;
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
            object hospitalId = null;
            parms.Add("PRIOR_AUTH_HOSPITAL_ID", hospitalId.ToString());
            //parms.Add("PRIOR_AUTH_HOSPITAL_ID", "1");
            var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_CODE", "");
            //parms.Add("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", "");
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_HCPCS_CODE", ddlServiceCodeType.Text.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_QUADRANT", txtQuadrant.Text.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", txtDentalRequestUnt.Text.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_APPROVED_UNITS", txtDentalApprovedunit.Text.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", txtDentalRequestUnt.Text.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_APPROVED_UNITS_FEE", txtDentalAppUnitFee.Text.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", txtDentalReqFDOS.Text.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", txtDentalReqTDOS.Text.ToString());
            //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_TOTAL_FEES", txtDentalAppUnitFee.Text.ToString());
            //parms.Add("PRIOR_AUTH_STATUS_ID", txtdentalStatus.Text.ToString());

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", id.ToString());
            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", id.ToString());
            _spa.InsertPriorAuthServiceDetail("AUTH_SERVICE_DETAIL", parms);


            return;
        }
        catch
        {

        }

    }
    //public override bool ValidateData()
    public bool ValidateData(string msg)
    {
        //bool isGood = true;
        //RequiredFieldValidator req = new RequiredFieldValidator();
        bool isValid = true;
        if (String.IsNullOrEmpty(txtproffProcedureCode.Text))
        {
            AddValidationErrorMessage("Procedure code missing for detail N");
            isValid = false;
        }
        if (String.IsNullOrEmpty(txtprofPlaceofserv.Text))
        {
            AddValidationErrorMessage("*");
            isValid = false;

        }
        if (String.IsNullOrEmpty(txtproffBilledUnits.Text))
        {
            AddValidationErrorMessage("*Date of Service not reported for detail N");
            isValid = false;

        }

        if (String.IsNullOrEmpty(txtProffdateofservice.Text))
        {
            AddValidationErrorMessage("*Date of Service not reported for detail N");
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
    protected void lnkPlaceofserviceSearch_Click(object sender, EventArgs e)
    {

    }


    protected void dateofservice_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtProffdateofservice.Text, true) && Helper.IsValidDate(txtProffdateofservice.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtProffdateofservice.Text).Subtract(Convert.ToDateTime(txtProffdateofservice.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }
    //protected void gvClaimDentalServicesDetails_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //}
}