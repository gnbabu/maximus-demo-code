using MAXIMUS.Controllers.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_PriorAuthProvidersNotes : BasePopupControl
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
    }
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();

        if (!IsPostBack)
        {
            try
            {
                //txtProviderNotes.Text = "";
            }
            catch
            {

            }
        }

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
            /// This Method will Save Note 
            /// </summary>
            /// 
            Dictionary<string, string> parms = new Dictionary<string, string>();
            //parms = new Dictionary<string, string>();
            object hospitalId = null;
            parms.Add("PRIOR_AUTH_HOSPITAL_ID", hospitalId.ToString());
            //parms.Add("PRIOR_AUTH_HOSPITAL_ID", "1");
            var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            parms.Add("PRIOR_AUTH_Note_PROVIDER_DESC", txtProviderNotes.Text.ToString());
            // parms.Add("PRIOR_AUTH_Note_REVIEWERProvider_DESC", txtRevNote.Text.ToString());
            //parms.Add("PRIOR_AUTH_Note_RESONDENIAL_DESC", txtDenialReason.Text.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", id.ToString());
            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", id.ToString());
            _spa.InsertPriorAuthNote("AUTH_Note", parms);


            return;
        }
        catch 
        {

        }

    }
    protected void NotesAdd_Click(object sender, EventArgs e)
    {
        
        BindGrid();
    }
    private void BindGrid()
    {
        //var ds = PriorAuthHospitalController.SelectPriorAuthAttachment(2320);

        //get the data from xml
        var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
        var dataTable = ds.Tables["ProviderNotes"];
        gvPriorAuthNotes.DataSource = dataTable;
        gvPriorAuthNotes.DataBind();
    }
    protected void gvPriorAuthNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPriorAuthNotes.PageIndex = e.NewPageIndex;
        BindGrid();
        gvPriorAuthNotes.EditIndex = -1;
    }

    protected void gvPriorAuthNotes_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // fetch UserId from DataKeysa        int PriorAttachmentId = Convert.ToInt32(gvPriorAuthAttachment.DataKeys[e.RowIndex]["AttachmentsList_Id"].ToString());

        //Delete in db
        //var ds = this.WorkflowPage.InquireHospiceResponse;
        //var dt = ds.Tables["Attachments"];
        //var dr = dt.Select("DiagnosisCodesList_Id=" + PriorAttachmentId).FirstOrDefault();
        //dr.Delete();

        //BindGrid();
    }

    private void ValidateData()
    {

    }
    

    
}