using MAXIMUS.Controllers.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_PriorAuthDiagnosisSeach : BasePopupControl
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
    private void BindGrid()
    {
        //var ds = PriorAuthHospitalController.SelectPriorAuthAttachment(2320);

        //get the data from xml
        var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
        var dataTable = ds.Tables["AuthorizationDiagnosis"];
        gvPriorAuthSearchDiagnosis.DataSource = dataTable;
        gvPriorAuthSearchDiagnosis.DataBind();
    }
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            try
            {
                
            }
            catch
            {
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

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

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
            /// <summary>
            /// This Method will Diagnosis
            /// </summary>
            /// 

            Dictionary<string, string> parms = new Dictionary<string, string>();
            object hospitalId = null;
            parms.Add("PRIOR_AUTH_HOSPITAL_ID", hospitalId.ToString());
            //parms.Add("PRIOR_AUTH_HOSPITAL_ID", "1");
            var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            parms.Add("PRIOR_AUTH_DIAGNOSIS_CODE", txtDiagnosisCode.Text.ToString());
            parms.Add("PRIOR_AUTH_DIAGNOSIS_DESC", txtDiagnosisCodeDesc.Text.ToString());
         //   parms.Add("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID", ddlDiagnosisCodeType.SelectedValue.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", id.ToString());
            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", id.ToString());
            _spa.InsertPriorAuthDiagnosis("AUTH_DIAGNOSIS", parms);


            return;
        }
        catch
        {
        }

    }
    protected void gvPriorAuthSearchDiagnosis_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPriorAuthSearchDiagnosis.PageIndex = e.NewPageIndex;
        BindGrid();
        gvPriorAuthSearchDiagnosis.EditIndex = -1;
    }
    protected void gvPriorAuthSearchDiagnosis_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // fetch UserId from DataKeysa        int PriorAttachmentId = Convert.ToInt32(gvPriorAuthAttachment.DataKeys[e.RowIndex]["AttachmentsList_Id"].ToString());

        //Delete in db
        //var ds = this.WorkflowPage.InquireHospiceResponse;
        //var dt = ds.Tables["Attachments"];
        //var dr = dt.Select("DiagnosisCodesList_Id=" + PriorAttachmentId).FirstOrDefault();
        //dr.Delete();

        //BindGrid();
    }
    protected void DiagnosissearchAdd_Click(object sender, EventArgs e)
    {
      
        BindGrid();
    }


    
    private void ValidateData()
    {
        throw new NotImplementedException();
    }


    //public override bool ValidateData()
    public bool ValidateData(string msg)
    {
        //bool isGood = true;
        //RequiredFieldValidator req = new RequiredFieldValidator();
        bool isValid = true;

       
        if (String.IsNullOrEmpty(txtDiagnosisCode.Text))
        {
            AddValidationErrorMessage("*Diagnosis Code is required");
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
    public bool CanUserViewDelete()
    {

        return Registration.CanUserViewDelete(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName);
    }
    //public override bool SaveData()
    //{
    //    _spa = new PDMSService.PDMSServiceClient();
    //    //DataSet ds = _spa.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
    //    //bool doUpdate = Helper.HasRows(ds);

    //    Dictionary<string, string> parms = new Dictionary<string, string>();
    //    parms.Add("PRIOR_AUTH_HOSPITAL_ID", "1");
    //    var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
    //    parms.Add("PRIOR_AUTH_DIAGNOSIS_CODE", txtDiagnosisCode.Text.ToString());
    //    parms.Add("PRIOR_AUTH_DIAGNOSIS_DESC", txtDiagnosisCodeDesc.Text.ToString());
    //    //  parms.Add("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID", ddlDiagnosisCodeType.SelectedValue.ToString());
    //    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
    //    parms.Add("LAST_MODIFIED_USER", id.ToString());
    //    parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
    //    parms.Add("Created_By_User", id.ToString());
    //    _spa.InsertPriorAuthDiagnosis("AUTH_DIAGNOSIS", parms);
    //    return true;

    //}



    protected void ddlICDVersion_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void lnkDiagnosisSearch_Click(object sender, EventArgs e)
    {

    }
}