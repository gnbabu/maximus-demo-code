using MAXIMUS.Controllers.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_PriorAuthDiagnosis : BasePopupControl
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
        gvPriorAuthDiagnosis.DataSource = dataTable;
        gvPriorAuthDiagnosis.DataBind();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.ucPriorAuthDiagnosisSeach.SaveEvent += new PopupControls_PriorAuthDiagnosisSeach.SaveEventHandler(UpdateDiagnosisSeachData);
        //this.ucPriorAuthDiagnosisSeach.CancelEvent += new PopupControls_PriorAuthDiagnosisSeach.CancelEventHandler(CancelPopup);

        if (!IsPostBack)
        {
            try
            {
                //txtDiagnosisCode.Text = "M25511";
                //txtDiagnosisCodeDesc.Text = "Chest Pain and Pain in Right Shoulder";
                //txtDiagnosisline.Text = "01";
                GetDiagnosisCodeType();
                BindGrid();
                //GetSequenceType();
                // GetPresentonAdmission();
            }
            catch
            {
            }
        }


    }
    private void CancelPopup()
    {

        //this.Dia.Hide();
        //this.doc.Hide();

    }

    private void UpdateDiagnosisSeachData()
    {

        //  Dia.Hide();


    }
    protected void DiagnosisAdd_Click(object sender, EventArgs e)
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
    protected void gvPriorAuthDiagnosis_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // fetch UserId from DataKeysa        int PriorAttachmentId = Convert.ToInt32(gvPriorAuthAttachment.DataKeys[e.RowIndex]["AttachmentsList_Id"].ToString());

        //Delete in db
        //var ds = this.WorkflowPage.InquireHospiceResponse;
        //var dt = ds.Tables["Attachments"];
        //var dr = dt.Select("DiagnosisCodesList_Id=" + PriorAttachmentId).FirstOrDefault();
        //dr.Delete();

        //BindGrid();
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    public string DiagnosisCode { get { return txtDiagnosisCode.Text; } }
    public string DiagnosisTypeCode { get { return ddlDiagnosisCodeType.SelectedValue; } }
    //public string ICDType { get { return txtDiagnosisCode.Text; } }


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

    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }
    /// <summary>
    /// This Method will get DIAGNOSIS CODE TYPE
    /// </summary>
    ///
    private void GetDiagnosisCodeType()
    {

        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        ddlDiagnosisCodeType.Items.Clear();
        DataSet dataSet = _spa.GetDiagnosisCodeType();
        DataTable dt = dataSet.Tables[0];

        ddlDiagnosisCodeType.DataSource = dt;
        ddlDiagnosisCodeType.DataValueField = "PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID";
        ddlDiagnosisCodeType.DataTextField = "PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC";
        //ddlDiagnosisCodeType.DataValueField = "DiagDesc";
        //ddlDiagnosisCodeType.DataTextField = "ICD10Diag";
        ddlDiagnosisCodeType.DataBind();
    }
    /// <summary>
    /// This Method will get Sequence CODE TYPE
    /// </summary>
    ///
    ////private void GetSequenceType()
    ////{

    ////    if (_spa == null)
    ////    {
    ////        _spa = new PDMSService.PDMSServiceClient();
    ////    }
    ////    ddlSequence.Items.Clear();
    ////    DataSet dataSet = _spa.GetSequenceType();
    ////    DataTable dt = dataSet.Tables[0];

    ////    ddlSequence.DataSource = dt;
    ////    ddlSequence.DataValueField = "PRIOR_AUTH_CLAIM_SEQUENCE_ID";
    ////    ddlSequence.DataTextField = "PRIOR_AUTH_CLAIM_SEQUENCE_DESC";
    ////    ddlSequence.DataBind();
    ////}
    // 
    /// <summary>
    /// This Method will get PresentonAdmission TYPE
    /// </summary>
    ///
    //private void GetPresentonAdmission()
    //{

    //    if (_spa == null)
    //    {
    //        _spa = new PDMSService.PDMSServiceClient();
    //    }
    //    ddlPresentonAdmission.Items.Clear();
    //    DataSet dataSet = _spa.GetPresentonAdmission();
    //    DataTable dt = dataSet.Tables[0];

    //    ddlPresentonAdmission.DataSource = dt;
    //    ddlPresentonAdmission.DataValueField = "PRIOR_AUTH_CLAIM_SEQUENCE_ID";
    //    ddlPresentonAdmission.DataTextField = "PRIOR_AUTH_CLAIM_SEQUENCE_DESC";
    //    ddlPresentonAdmission.DataBind();
    //}

    //protected void btnAddDiagnosis_Click(object sender, EventArgs e)
    //{
    //    var ds = PriorAuthHospitalController.SelectPriorAuthDiagnosis(2320);
    //    var dt = ds.Tables["DiaData"];
    //    var dr = dt.NewRow();

    //    var diagType =ddlDiagnosisCodeType.SelectedValue;
    //    var desc = txtDiagnosisCodeDesc.Text.Trim();
    //    dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"] = "";
    //    // dr["PRIOR_AUTH_DIAGNOSIS_CODE"] = diagType;
    //    //dr["PRIOR_AUTH_DIAGNOSIS_DESC"] = ucPriorAuthDiagnosis.text.txtDiagnosisCodeDesc;
    //    dr["PRIOR_AUTH_SEQUENCE_ID"] = "1";
    //    dt.Rows.Add(dr);
    //    BindGrid(dt);
    //}

    private void BindGrid(DataTable dt)
    {
        //gvDiagnosis.DataSource = dataTable;
        //gvDiagnosis.DataBind();
    }
    protected void gvPriorAuthDiagnosis_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPriorAuthDiagnosis.PageIndex = e.NewPageIndex;
        BindGrid();
        gvPriorAuthDiagnosis.EditIndex = -1;
    }


    //protected void btnSave_Click(object sender, EventArgs e)
    //{

    //    if (!ValidateData()) return;

    //    SaveData();
    //}
    protected void btnAddDiagnosis_Click(object sender, EventArgs e)
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
            parms.Add("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID", ddlDiagnosisCodeType.SelectedValue.ToString());
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

        //if (String.IsNullOrEmpty(ddlSequence.SelectedValue))
        //{
        //    AddValidationErrorMessage("*Sequence required");
        //    isValid = false;
        //}
        if (String.IsNullOrEmpty(ddlDiagnosisCodeType.SelectedValue))
        {
            AddValidationErrorMessage("*Diagnosis Code Type is required");
            isValid = false;
        }
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

    //public override bool SaveData()
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
    //        /// <summary>
    //        /// This Method will Diagnosis
    //        /// </summary>
    //        /// 

    //        Dictionary<string, string> parms = new Dictionary<string, string>();
    //        parms.Add("PRIOR_AUTH_HOSPITAL_ID", "1");
    //        var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
    //        parms.Add("PRIOR_AUTH_DIAGNOSIS_CODE", txtDiagnosisCode.Text.ToString());
    //        parms.Add("PRIOR_AUTH_DIAGNOSIS_DESC", txtDiagnosisCodeDesc.Text.ToString());
    //        parms.Add("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID", ddlDiagnosisCodeType.SelectedValue.ToString());
    //        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
    //        parms.Add("LAST_MODIFIED_USER", id.ToString());
    //        parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
    //        parms.Add("Created_By_User", id.ToString());
    //        _spa.InsertPriorAuthDiagnosis("AUTH_DIAGNOSIS", parms);


    //        return;
    //    }
    //    catch (Exception ex)
    //    {

    //    }

    //    return true;
    //}
    protected void ddlDiagnosisCodeType_SelectedIndexChanged(object sender, EventArgs e)
    {


    }
    protected void ddlSequence_SelectedIndexChanged(object sender, EventArgs e)
    {


    }
    protected void ddlPresentonAdmission_SelectedIndexChanged(object sender, EventArgs e)
    {


    }

    #region Section
    private enum PopupName { PriorAuthDiagnosisSeach = 0 };
    #endregion
    protected void lnkDiagnosisSearch_Click(object sender, EventArgs e)
    {
        this.LoadData(null);

        //switch (e.CommandName)
        //{
        //    case "DiagnosisSearch":
        //        lblDiagnosissearch.Text = "Search";
        //        ucPriorAuthDiagnosisSeach.LoadData(dr);
        //        mltDiag.ActiveViewIndex = Convert.ToInt32(PopupName.PriorAuthDiagnosisSeach);
        //        //mltPopup.ActiveViewIndex = 0;
        //        Dia.Show();
        //        break;
        //    default:
        //        break;
        //}
    }

    protected void txtclaimDiagnosisCode_TextChanged(object sender, EventArgs e)
    {

    }



    protected void txtDiagnosisCodeDesc_TextChanged(object sender, EventArgs e)
    {

    }
}