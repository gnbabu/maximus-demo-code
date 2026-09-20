using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_CredentialReviewSearch : System.Web.UI.UserControl
{
  #region Properties
 
    private int _datarank;
    private int _workflowId;
    private int _providerTypeID;
    private string _providerName;
    private string _mmisproviderTypeID;
    public int DataRank
    {
        get
        {
            _datarank = (ddlDataRank!=null && ddlDataRank.SelectedIndex <= 0) ? 0 : int.Parse(ddlDataRank.SelectedItem.Value.ToString());
            return _datarank;
        }
        
    }

    public int WorkflowId 
    {
        get
        {
            _workflowId = (ddlworkflow!=null && ddlworkflow.SelectedIndex <= 0) ? 0 : int.Parse(ddlworkflow.SelectedItem.Value.ToString());
            return _workflowId;
        }
    }
    public int ProviderTypeId 
    {
        get
        {
            _providerTypeID = (ddlProviderType != null && ddlProviderType.SelectedIndex <= 0) ? 0 : int.Parse(ddlProviderType.SelectedItem.Value.ToString());
            return _providerTypeID;
        }
    }
    public string ProviderName
    {
        get
        {
            _providerName = txtGroupName.Text.Trim();
            return _providerName;
        }
    }
    public string MMISProviderTypeId
    {
        get
        {
            _mmisproviderTypeID = (ddlProviderType != null && ddlProviderType.SelectedIndex <= 0) ? string.Empty : (ddlProviderType.SelectedItem.Value.ToString());
            return _mmisproviderTypeID;
        }
    }

    #endregion
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
    #region View Methods





    protected void Page_PreRender(object sender, EventArgs e)
    {
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadFormData();       
    
        }
    }

    private void LoadFormData()
    {

        try
        {
            //Load Data Rank DD
            //LoadDataRank();
            LoadProviderType();
            //LoadWorkflow();
        }
        catch (Exception ex)
        {
        }
    }

    private void LoadProviderType()
    {
     
        // * For now these values are constant as credentialing only enabled for standard application and for all individuals
        // * 
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            DataSet ds = psc.GetCredentialingProviderTypes();
            if (Helper.HasRows(ds))
            {
                Helper.LoadDropDown(ddlProviderType, ds.Tables[0], "PROVIDER_TYPE_NAME", "MMIS_PROVIDER_TYPE_ID", true);
                ddlProviderType.Focus();
            }
            else
                ddlProviderType.Items.Clear();
        }
    }
    private void LoadDataRank()
    {
        //ddlDataRank.Items.Clear();
        //List<ListItem> riskLevel = new List<ListItem>();
        //riskLevel.Add(new ListItem("Low", "1"));
        //riskLevel.Add(new ListItem("Medium", "2"));
        //riskLevel.Add(new ListItem("High", "3"));
        //ddlDataRank.DataSource = riskLevel;
        //ddlDataRank.DataBind();

        //ddlDataRank.Items.Insert(0, new ListItem(string.Empty, string.Empty));

        //using (DataSet ds = svc.SelectDataRankTypes())
        //{
        //    if (Helper.HasRows(ds))
        //    {
        //        DataTable dtDataRank = ds.Tables[0].Select("DISPLAY_NAME <> 'Conditional'").CopyToDataTable();
        //        Helper.LoadDropDown(this.ddlDataRank, dtDataRank, "DISPLAY_NAME", "DATARANK_TYPE_ID", true);
        //    }
        //}
    }
    private void LoadWorkflow()
    {
        //This drop down is a prace holder for now need to replace once we are clear on these requirements and recredentialing comes in to picture.
        //ddlworkflow.Items.Clear();

        //List<ListItem> workflowSource = new List<ListItem>();
        //workflowSource.Add(new ListItem("Registration - New", "1"));
        //workflowSource.Add(new ListItem("Registration - Recredentialing", "18"));
        //ddlworkflow.DataSource = workflowSource;
        //ddlworkflow.DataBind();

        //ddlworkflow.Items.Insert(0, new ListItem(string.Empty, string.Empty));
    }
    #endregion

    #region Private Methods

    public void ClearFormData()
    {
        txtGroupName.Text = string.Empty;
        ddlProviderType.SelectedIndex = ddlDataRank.SelectedIndex = ddlworkflow.SelectedIndex = -1;
    }
    
   
    #endregion
    

    
    public bool isSearchFieldsExists()
    {
        
        bool isSearch=false;


        if (string.IsNullOrEmpty(txtGroupName.Text)
            && (ddlProviderType.SelectedIndex == -1
            && ddlDataRank.SelectedIndex == -1
            && ddlworkflow.SelectedIndex == -1))
        {
            isSearch = false;
        }
        else
        {
            isSearch = true;
        }
        return isSearch;

    }

    
   
}