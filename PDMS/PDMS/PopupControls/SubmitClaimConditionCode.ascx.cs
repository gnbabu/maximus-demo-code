using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_SubmitClaimConditionCode : BasePopupControl
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
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();

        if (!IsPostBack)
        {
            try
            {
                //txtProviderNotes.Text = "";
            }
            catch (Exception ex)
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

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;
    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();



    }
    private void BindGrid()
    {
       // var ds = PriorAuthHospitalController.GetAddUPdateClaimsRequestResponse();

       // get the data from xml
        //var ds = PriorAuthHospitalController.GetAddUPdateClaimsRequestResponse();
       // var dataTable = ds.Tables["AuthorizationService"];
        //gvSubmitClaimConditionCode.DataSource = dataTable;
       // gvSubmitClaimConditionCode.DataBind();
    }
    protected void SubmitClaimConditionCodeAdd_Click(object sender, EventArgs e)
    {
       
    }
    protected void gvSubmitClaimConditionCode_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {


        BindGrid();
    }
    protected void gvSubmitClaimConditionCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSubmitClaimConditionCode.PageIndex = e.NewPageIndex;
        BindGrid();
        gvSubmitClaimConditionCode.EditIndex = -1;
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
           


            return;
        }
        catch (Exception ex)
        {

        }

    }
    private void ValidateData()
    {

    }
    protected void lnkConditionCodeSearch_Click(object sender, EventArgs e)
    {

    }

   
}