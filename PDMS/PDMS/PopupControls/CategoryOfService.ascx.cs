using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PopupControls_CategoryOfService : BaseSectionControl 
{
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

    public int RegCategoryOfServiceID
    {
        get
        {
            return ViewState["RegCategoryOfServiceID"] == null ? 0 : Convert.ToInt32(ViewState["RegCategoryOfServiceID"]);
        }
        set
        {
            ViewState["RegCategoryOfServiceID"] = value;
        }
    }


    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
    }

    public override bool ValidateData()
    {
        return true;
    }

    #endregion

    #region Public Methods

    public override void LoadData(DataRow dr)
    {

    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadCategoryOfService();
    }

    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
    }

    private void LoadCategoryOfService()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CATEGORY_OF_SERVICE_INFO");
        if (Helper.HasRows(ds))
            rgCategoryOfService.DataSource = ds;
        else
        {
            int diddReferralId = 0, entityTypeId = 0, providerTypeId = 0, specialtyTypeID = 0, taxIDTypeID = 0;
            string taxID = string.Empty;        // Defined here but not needed and not used for visible definition
            Registration.SetEntityProviderTypesDIDD(this.WorkflowPage.RegistrationId, ref entityTypeId, ref providerTypeId, ref diddReferralId, ref specialtyTypeID, ref taxID, ref taxIDTypeID);
            DataSet ds1 = psc.SelectCategoryOfServiceType(providerTypeId);
            if (Helper.HasRows(ds1))
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                    parms.Add("CATEGORY_OF_SERVICE_TYPE_ID", dr["category_of_service_type_id"].ToString());
                    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                    psc.InsertRegistrationDataTable("CATEGORY_OF_SERVICE_INFO", parms);
                }
                ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CATEGORY_OF_SERVICE_INFO");
                rgCategoryOfService.DataSource = ds;

            }
        }
        rgCategoryOfService.DataBind();
        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))
        {
            btnAddCOS.Visible = false;
            rgCategoryOfService.MasterTableView.GetColumn("EditCommandColumn").Display = true;
        }
        else
        {
            btnAddCOS.Visible = false;
            rgCategoryOfService.MasterTableView.GetColumn("EditCommandColumn").Display = false;
        }
    }


    protected void rgCategoryOfService_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        GridEditableItem item = e.Item as GridEditableItem;
        TextBox txtEDate = item.FindControl("txtEndDate") as TextBox;
        int id = Convert.ToInt32(item.GetDataKeyValue("REG_CATEGORY_OF_SERVICE_INFO_ID"));
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", item.GetDataKeyValue("REG_ID").ToString());
        if (!string.IsNullOrWhiteSpace(txtEDate.Text.ToString()) && Convert.ToDateTime(txtEDate.Text) != DateTime.MinValue)
        {
            parms.Add("EndDate", Convert.ToDateTime(txtEDate.Text).ToShortDateString());
        }
        else
        {
            parms.Add("EndDate", null);
        }
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        if (id > 0)
        {
            parms.Add("REG_CATEGORY_OF_SERVICE_INFO_ID", id.ToString());
            svc.UpdateRegistrationDataTable("CATEGORY_OF_SERVICE_INFO", parms);
        }
    }

    protected void rgCategoryOfService_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CATEGORY_OF_SERVICE_INFO");
        if (Helper.HasRows(ds))
            rgCategoryOfService.DataSource = ds;
        else rgCategoryOfService.DataSource = null;
    }

    protected void rgCategoryOfService_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            (rgCategoryOfService.MasterTableView.GetColumn("MAX_CATEGORY_OF_SERVICE_TYPE_ID") as GridBoundColumn).ReadOnly = true;
            (rgCategoryOfService.MasterTableView.GetColumn("CATEGORY_OF_SERVICE_TYPE_NAME_ID") as GridBoundColumn).ReadOnly = true;
            (rgCategoryOfService.MasterTableView.GetColumn("StartDate") as GridBoundColumn).ReadOnly = true;
        }
    }

    #endregion

    public override bool SaveData()
    {
        return true;
    }

    public override string ValidationGroup
    {
        get { return "valCategoryOfService"; }
    }

    public override string Title
    {
        get { return "Category Of Service"; }
    }

    public override string IdText
    {
        get { return "ucCategoryOfService_" + this.WorkflowPage.RegistrationId; }
    }
}