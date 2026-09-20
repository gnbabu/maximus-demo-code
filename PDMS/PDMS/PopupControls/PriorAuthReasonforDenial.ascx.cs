using MAXIMUS.Controllers.PDMS;
using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_PriorAuthReasonforDenial : BasePopupControl
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
    
    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();



    }
    private void BindGrid()
    {
        //var ds = PriorAuthHospitalController.SelectPriorAuthAttachment(2320);

        //get the data from xml
        var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
        var dataTable = ds.Tables["AuthorizationService"];
        //var dataTable = ds.Tables["AuthorizationService"];
        gvPriorAuthReasonforDenial.DataSource = dataTable;
        gvPriorAuthReasonforDenial.DataBind();
    }
    protected void gvPriorAuthReasonforDenial_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPriorAuthReasonforDenial.PageIndex = e.NewPageIndex;
        BindGrid();
        gvPriorAuthReasonforDenial.EditIndex = -1;
    }

   

    private void ValidateData()
    {

    }



}