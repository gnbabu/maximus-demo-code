using MAXIMUS.Controllers.PDMS;
using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_PriorAuthAttachment : BasePopupControl
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
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();
         GetPriorDocumentType();
        GetPriorClaimsDocumentType();

        // LoadPrioAuthDocumentTypeDropDown();
    }
    public override void LoadData(DataRow dr)
    {
        base.LoadData(dr);
        BindGrid();
        GetPriorDocumentType();
        GetPriorClaimsDocumentType();

    }

    private void BindGrid()
    {
        //var ds = PriorAuthHospitalController.SelectPriorAuthAttachment(2320);

        //get the data from xml
        var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
        var dataTable = ds.Tables["Attachments"];
        gvPriorAuthAttachment.DataSource = dataTable;
        gvPriorAuthAttachment.DataBind();
    }

    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;


    protected void btnAdd_Click(object sender, EventArgs e)
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    protected void gvPriorAuthAttachment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPriorAuthAttachment.PageIndex = e.NewPageIndex;
        BindGrid();
        gvPriorAuthAttachment.EditIndex = -1;
    }

    protected void gvPriorAuthAttachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      

        BindGrid();
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

        ddlDocumentType.Items.Clear();
        DataSet dataSet = _spa.GetPriorDocumentType();
        DataTable dt = dataSet.Tables[0];
        ddlDocumentType.DataSource = dt;
        ddlDocumentType.DataValueField = "DOCUMENT_TYPE_ID";
        ddlDocumentType.DataTextField = "DOCUMENT_TYPE_DESC";
        ddlDocumentType.DataBind();
    }

    //private void GetPriorDocumentType()
    //{
    //    if (_spa == null)
    //    {
    //        _spa = new PDMSService.PDMSServiceClient();
    //    }

    //    ddlDocumentType.Items.Clear();
    //    DataSet dataSet = _spa.GetPriorDocumentType();
    //    DataTable dt = dataSet.Tables[0];
    //   // Helper.LoadList(ddlDocumentType, dt, "PRIOR_AUTH_DOCUMENT_MAIL_ID", "PRIOR_AUTH_DOCUMENT_MAIL_DESC", true);
    //    ddlDocumentType.DataSource = dt;
    //    ddlDocumentType.DataValueField = "PRIOR_AUTH_DOCUMENT_MAIL_ID";
    //    ddlDocumentType.DataTextField = "PRIOR_AUTH_DOCUMENT_MAIL_DESC";
        
    //    ddlDocumentType.DataBind();
    //}
    
    /// <summary>
    /// This Method will get Prior DocumentType
    /// </summary>
    private void GetPriorClaimsDocumentType()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }

        ddlDocumentTypeclaims.Items.Clear();
        DataSet dataSet = _spa.GetClaimDocumentTypeByClaimTransactionType(4);
        DataTable dt = dataSet.Tables[0];
        // Helper.LoadList(ddlDocumentType, dt, "DOCUMENT_TYPE_ID", "DOCUMENT_TYPE_DESC", true);
        ddlDocumentTypeclaims.DataSource = dt;

        ddlDocumentTypeclaims.DataValueField = "DOCUMENT_TYPE_ID";
        ddlDocumentTypeclaims.DataTextField = "DOCUMENT_TYPE_DESC";
        ddlDocumentTypeclaims.DataBind();
    }

    protected void ddlDocumentTypeclaims_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlDocumentType.Visible = false;
        ddlDocumentTypeclaims.Visible = true;
    }
    
    protected void ddlDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlDocumentTypeclaims.Visible = false;
        ddlDocumentType.Visible = true;


    }

    protected void gvPriorAuthAttachment_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}


