using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_SubmitClaimAdditionalProviderInformation_ : BasePopupControl
{
    #region 
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
                GetSubmiclaimtProviderType();
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
    protected void AdditionalProviderinfoAdd_Click(object sender, EventArgs e)
    {
        
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
            //Dictionary<string, string> parms = new Dictionary<string, string>();
            ////parms = new Dictionary<string, string>();
            //object hospitalId = null;
            //parms.Add("PRIOR_AUTH_HOSPITAL_ID", hospitalId.ToString());
            ////parms.Add("PRIOR_AUTH_HOSPITAL_ID", "1");
            //var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            //parms.Add("PRIOR_AUTH_Note_PROVIDER_DESC", txtProviderNotes.Text.ToString());
            //// parms.Add("PRIOR_AUTH_Note_REVIEWERProvider_DESC", txtRevNote.Text.ToString());
            ////parms.Add("PRIOR_AUTH_Note_RESONDENIAL_DESC", txtDenialReason.Text.ToString());
            //parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            //parms.Add("LAST_MODIFIED_USER", id.ToString());
            //parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            //parms.Add("Created_By_User", id.ToString());
            //_spa.InsertPriorAuthNote("AUTH_Note", parms);


            return;
        }
        catch (Exception ex)
        {

        }

    }
    

    private void GetSubmiclaimtProviderType()
    {

        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        ddlProviderType.Items.Clear();
        DataSet dataSet = _spa.GetSubmiclaimtProviderType();
        DataTable dt = dataSet.Tables[0];
        Helper.LoadList(ddlProviderType, dt, "PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC", "PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID", true);
        
    }
    
    private void ValidateData()
    {

    }
    private void BindGrid()
    {

    }
    protected void gvTAdditionalProviderinfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {


        BindGrid();
    }
    protected void gvAdditionalProviderinfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAdditionalProviderinfo.PageIndex = e.NewPageIndex;
        BindGrid();
        gvAdditionalProviderinfo.EditIndex = -1;
    }
    protected void ddlProviderType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlDetails_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}