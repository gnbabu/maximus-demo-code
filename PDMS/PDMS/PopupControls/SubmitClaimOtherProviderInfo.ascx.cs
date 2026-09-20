using System;
using System.Data;
using System.Web.UI;

public partial class PopupControls_SubmitClaimOtherProviderInfo : BasePopupControl
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


        if (!IsPostBack)
        {
            try
            {

                GetOtherPhysician();
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
    /// <summary>
    /// This Method will get OtherPhysician
    /// </summary>
    ///
    private void GetOtherPhysician()
    {

        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        ddlOtherPhysician.Items.Clear();
        DataSet dataSet = _spa.GetOtherPhysician();
        DataTable dt = dataSet.Tables[0];
        Helper.LoadList(ddlOtherPhysician, dt, "PRIOR_AUTH_SUBMIT_CLAIM_OTHERPHYSICIAN_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_OTHERPHYSICIAN_ID", true);
      
    }

    private void ValidateData()
    {

    }
    //protected void Page_Load(object sender, EventArgs e)
    //{

    //}

    protected void ddlOtherPhysician_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}