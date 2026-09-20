using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_PriorAuthDocumentbyMail : BasePopupControl
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

    private int _priohospitalid = 0;
    private int PrioHospitalId
    {
        get
        {
            return _priohospitalid;
        }
        set
        {
            if (value != _priohospitalid)
            {
                _priohospitalid = value;
            }
        }
    }

    #endregion
    public override void LoadData(DataRow dr)
    {
        base.LoadData(dr);
        GetPriorDocumentType();
        GetPriorClaimsDocumentType();
        BindGrid();
    }
    private void BindGrid()
    {
        string reqMedID = Request.QueryString["MedicaidNumber"];
        string regID = LoadProviderInformation(reqMedID);
        var ds = PriorAuthHospitalController.SelectPriorAuthDocumentByMail(2,  Convert.ToInt32(regID));
        var dataTable = ds.Tables["DocumentbyMail"];
        //DataSet dataSet = _spa.GetPrioHospitalData("DocumentbyMail", PrioHospitalId);
        //  DataTable dt = dataTable.Tables[0];
        gvPriorAuthDocumentbyMail.DataSource = dataTable;
        gvPriorAuthDocumentbyMail.DataBind();
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

                GetPriorDocumentType();
                GetPriorClaimsDocumentType();
                BindGrid();
            }
            catch
            {

            }
        }
    }
    //private void BindGrid()
    //{
    //    var lstRecords = GetData();
    //    //DataSet dataSet = _spa.GetPrioHospitalData("DocumentbyMail", PrioHospitalId);
    //    //DataTable dt = dataSet.Tables[0];
    //    gvPriorAuthDocumentbyMail.DataSource = lstRecords;
    //    gvPriorAuthDocumentbyMail.DataBind();
    //}
    private DataTable GetData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("PRIOR_AUTH_DOCUMENT_MAIL_ID"));
        dt.Columns.Add(new DataColumn("LineItem"));
        dt.Columns.Add(new DataColumn("DocumentType"));
        dt.Columns.Add(new DataColumn("Note"));

        // dt.Rows.Add("1", "01", "pdf", "note");
        return dt;
    }
    protected void gvPriorAuthDocumentbyMail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPriorAuthDocumentbyMail.PageIndex = e.NewPageIndex;
        BindGrid();
        gvPriorAuthDocumentbyMail.EditIndex = -1;
    }

    protected void gvPriorAuthDocumentbyMail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // fetch UserId from DataKeys
        int PRIOR_AUTH_DOCUMENT_MAIL_ID = Convert.ToInt32(gvPriorAuthDocumentbyMail.DataKeys[e.RowIndex]["PRIOR_AUTH_DOCUMENT_MAIL_ID"].ToString());

        //Delete in db
        BindGrid();
    }
    private void ValidateData()
    {

    }
    protected void DocumentbyMailAdd_Click(object sender, EventArgs e)
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
            /// This Method will Save Document By mail
            /// </summary>
            Dictionary<string, object> parms = new Dictionary<string, object>();
            //object hospitalId = null;
            //  parms.Add("PRIOR_AUTH_HOSPITAL_ID", hospitalId.ToString());

            parms.Add("PRIOR_AUTH_HOSPITAL_ID", "1");
            var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            parms.Add("PRIOR_AUTH_DOCUMENT_TYPE_ID", ddlDoctype.SelectedValue.ToString());
            parms.Add("PRIOR_AUTH_DOCUMENT_MAIL_DESC", txtDocNote.Text.ToString());

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", id.ToString());
            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", id.ToString());
            _spa.InsertPriorAuthDocumentByMail("AUTH_Document_Mail", parms);

            return;
        }
        catch
        {

        }

    }

    ///// <summary>
    ///// This Method will get Prior DocumentType
    ///// </summary>
    private void GetPriorDocumentType()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }

        ddlDoctype.Items.Clear();
        DataSet dataSet = _spa.GetPriorDocumentType();
        DataTable dt = dataSet.Tables[0];
        ddlDoctype.DataSource = dt;
        ddlDoctype.DataValueField = "DOCUMENT_TYPE_ID";
        ddlDoctype.DataTextField = "DOCUMENT_TYPE_DESC";
        ddlDoctype.DataBind();
    }

    private string LoadProviderInformation(string medicaidNumber)
    {
        string RegID = default(string);
        try
        {
            DataSet ds = spa.SelectProviderByGRPMedicaidID(medicaidNumber);
            DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
            this.DataList = dtMisc;
            if (Helper.HasRows(dtMisc))
            {
                DataRow dr = dtMisc.Rows[0];
                RegID = Helper.GetString("REG_ID", dr);
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-" + System.Reflection.MethodInfo.GetCurrentMethod().ToString());
        }
        return RegID;
    }
    public static string CreateAndReturnLogThreadNumber(Exception ex, string errorKey = "")
    {
        string logid = HttpContext.Current.Session["LogKey"] != null ? HttpContext.Current.Session["LogKey"].ToString() : CON.appAdminUserId;
        Logging logging = new Logging(new Guid(logid));
        string logMessage = errorKey + " " + logging.GetRecursiveException(ex);
        logging.CreateLogEntry(logMessage, CON.WebPageProcessName.SubmitPriorAuthorization);
        return logging.ThreadId.ToString();
    }

    /// <summary>
    /// This Method will get Prior DocumentType
    /// </summary>
    private void GetPriorClaimsDocumentType()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }





        ddlDoctype.Items.Clear();
        DataSet dataSet = _spa.GetAssignements();
        if (dataSet == null) return;
        DataTable dt = dataSet.Tables[0];
        Helper.LoadList(ddlDoctype, dt, "PRIOR_AUTH_Assignment_Type_DESC", "PRIOR_AUTH_Assignment_Type_MMIS", true);


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

    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;
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
    //        /// <summary>
    //        /// This Method will Save Document By mail
    //        /// </summary>
    //        Dictionary<string, string> parms = new Dictionary<string, string>();
    //        object hospitalId = null;
    //        parms.Add("PRIOR_AUTH_HOSPITAL_ID", hospitalId.ToString());
    //        parms.Add("PRIOR_AUTH_HOSPITAL_ID", "1");
    //        //parms.Add("PRIOR_AUTH_HOSPITAL_ID", "1");
    //        var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
    //        parms.Add("PRIOR_AUTH_DOCUMENT_TYPE_ID", ddlDoctype.SelectedValue.ToString());
    //        parms.Add("PRIOR_AUTH_DOCUMENT_MAIL_DESC", txtDocNote.Text.ToString());

    //        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
    //        parms.Add("LAST_MODIFIED_USER", id.ToString());
    //        parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
    //        parms.Add("Created_By_User", id.ToString());
    //        _spa.InsertPriorAuthDocumentByMail("AUTH_DocumentByMail", parms);

    //        return;
    //    }
    //    catch (Exception ex)
    //    {

    //    }

    //}
    public bool ValidateData(string msg)
    {
        //bool isGood = true;
        //RequiredFieldValidator req = new RequiredFieldValidator();
        bool isValid = true;
        if (String.IsNullOrEmpty(ddlDoctype.SelectedValue))
        {
            AddValidationErrorMessage("*Document type is required");
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


    protected void btnPrintCoverAdd_Click(object sender, EventArgs e)
    {

    }

}