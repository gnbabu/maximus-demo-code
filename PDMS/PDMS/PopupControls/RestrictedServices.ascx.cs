using AjaxControlToolkit;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_RestrictedServices : BaseSectionControl
{
    public string _SortField
    {
        get
        {
            return (string)ViewState["SortField"] ?? "Index"; // default sort 
        }
        set
        {
            ViewState["SortField"] = value;
        }
    }

    public delegate void SaveDataEventHandler();
    public event SaveDataEventHandler SaveDataEvent;
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
    #region grdRestrcitedServices
    DataTable _dtRestrcitedServices;

    public DataTable dtRestrcitedServices
    {
        get
        {
            if (_dtRestrcitedServices == null)
            {
                _dtRestrcitedServices = this.GetRestrcitedServices();
            }

            return _dtRestrcitedServices;
        }
    }

   

    #endregion
    #region Section
    private enum PopupName { RestrcitedService = 0, RestrcitedServiceHistory = 1 };
    #endregion

    private bool _ExportHistory
    {
        get
        {
            return Convert.ToBoolean(ViewState["ExportHistory"]);
        }
        set
        {
            ViewState["ExportHistory"] = value;
        }
    }

    private DataTable GetRestrcitedServices()
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "RESTRICTION");
        DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;

        return dt;
    }

    private void SetButtons()
    {

    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (pageTypeID == CON.RegistrationPageType.Services)
        {
            this.ucRestrictedService.SaveEvent += new PopupControls_RestrictedService.SaveEventHandler(UpdateRSServices);
            this.ucRestrictedService.ValidationEvent += new PopupControls_RestrictedService.ValidationEventHandler(KeepPopupOpen);
            this.ucRestrictedService.ErrorEvent += new PopupControls_RestrictedService.ErrorEventHandler(KeepPopupOpen);
            this.ucRestrictedService.KeepOpenEvent += new PopupControls_RestrictedService.KeepOpenEventHandler(KeepPopupOpen);
            this.ucRestrictedService.CancelEvent += new PopupControls_RestrictedService.CancelEventHandler(CancelPopup);
        }
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Restricted Service";
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_GetRestrictedServicesHistory", parms);
        grd.DataSource = ds.Tables[0];
        grd.DataBind();
    }
    public override void LoadControlData()
    {
        RefreshRestrictedService();
        LoadRestrcitedServices();
    }

    public override void LoadData(DataRow row = null)
    {
        this.RefreshRestrictedService();
    }

    public void LoadData()
    {
        //changes for bug DCPDMS-2266 to show page heading based on provider type
       // LoadRestrcitedServices();
        RefreshRestrictedService();
        btnHistory.Visible = grdRestrcitedServices.Rows.Count > 0 && Helper.IsUserInRestrictedServiceViewRole(HttpContext.Current.User.Identity.Name);
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        var ds = psc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        if (Helper.HasRows(ds))
        {
            int currentStatus = Convert.ToInt32(ds.Tables[0].Rows[0]["RegProgramStatusTypeID"]);
            // OHPNM-18201 - internal user should be able to edit and add new restricted service regardless of enrollment status as long as the provider is not in workflow
            btnAdd.Visible = ((Helper.IsUserInRestrictedServiceUpdateRole(HttpContext.Current.User.Identity.Name)) && this.WorkflowPage.CurrentTaskName == "");
        }
    }

    private void LoadRestrcitedServices()
    {
        grdRestrcitedServices.DataSource = GetSortedData();
        grdRestrcitedServices.DataBind();

        btnHistory.Visible = grdRestrcitedServices.Rows.Count > 0 && Helper.IsUserInRestrictedServiceViewRole(HttpContext.Current.User.Identity.Name);
        btnAdd.Visible = Helper.IsUserInRestrictedServiceUpdateRole(HttpContext.Current.User.Identity.Name);

        // OHPNM-18201 - internal user should be able to edit and add new restricted service regardless of enrollment status as long as the provider is not in workflow
        if (!(Helper.IsUserInRestrictedServiceUpdateRole(HttpContext.Current.User.Identity.Name)) || this.WorkflowPage.CurrentTaskName != "")
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            var ds = psc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
            if (Helper.HasRows(ds))
            {
                int currentStatus = Convert.ToInt32(ds.Tables[0].Rows[0]["RegProgramStatusTypeID"]);
                if (currentStatus != CON.RegistrationProgramStatusTypeId.Maintenance)
                {
                    var editColumn = ((DataControlField)grdRestrcitedServices.Columns
                        .Cast<DataControlField>()
                        .Where(fld => fld.HeaderText == "Edit")
                        .SingleOrDefault());
                    if (editColumn != null)
                    {
                        grdRestrcitedServices.Columns.Remove(editColumn);
                    }
                }
            }
        }
    }

   
    public override bool SaveData()
    {
        return !Registration.PreviewingRegistrationSection();
    }

    public override bool ValidateData()
    {
        bool isGood = true;
       // if (!GridDataValid()) isGood = false;
       

        return isGood;
    }

    public override bool HasInputValue()
    {
        bool rtn = false;
        if (grdRestrcitedServices != null)
        {
            if (grdRestrcitedServices.Rows.Count > 0)
            {
                rtn = true;
            }
        }
        return rtn;
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.CancelPopup();
    }

    protected void grdRestrcitedServices_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Sort"))
        {
            return;
        }

        int index = Convert.ToInt32(e.CommandArgument);
        int regRestrictionID = string.IsNullOrEmpty(this.grdRestrcitedServices.DataKeys[index].Values["REG_RESTRICTION_ID"].ToString()) ? 0 : (int)this.grdRestrcitedServices.DataKeys[index].Values["REG_RESTRICTION_ID"];
       
        switch (e.CommandName)
        {
            case "RestrictedService":
              
                if (regRestrictionID > 0)
                {
                    lblTitle.Text = "Edit Restricted Service";
                    ucRestrictedService.LoadData(SelectedRestrictedService(regRestrictionID));
                    mltPopupRS.SetActiveView(vwRestrictedService);
                    mpeRS.Show();
                }
                break;

            case "DeleteRestrictedService":
                if (regRestrictionID > 0)
                {
                    this.DeleteRegRestriction(regRestrictionID);
                }
                break;


            default:
                break;
        }
    }
    private void UpdateRSServices()
    {
        this.RefreshRestrictedService();
        this.upRS_Main.Update();

        this.mpeRS.Hide();
    }
    protected void btnAdd_Click(object sender, CommandEventArgs e)
    {
        lblTitle.Text = "Add Restricted Service";
               
        ucRestrictedService.LoadData(null);
        mltPopupRS.SetActiveView(vwRestrictedService);
            
        mpeRS.Show();
    }
    private void DeleteRegRestriction(int regRestrictionID)
    {
        svc.DeleteRegRestriction(regRestrictionID);
        this.RefreshRestrictedService();
    }
    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        mpeRSHistory.Show();
    }

    protected void grdRestrcitedServices_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");
            string sentToSI = DataBinder.Eval(e.Row.DataItem, "SENT_TO_SI") == null ?string.Empty : DataBinder.Eval(e.Row.DataItem, "SENT_TO_SI").ToString();
            bool bsentToSI = (string.IsNullOrEmpty(sentToSI)) ? false : (sentToSI == "1" || sentToSI.ToLower() == "true") ? true : false;
            if (btnDelete!=null)
            {
                btnDelete.Style["display"] = !bsentToSI ? "block" : "none";

            }
        }
    }

    protected void grdRestrcitedServices_Sorting(Object sender, GridViewSortEventArgs e)
    {
        this.ResetSelectedProvider();
        this.RefreshRestrictedService();
    }

    protected void grdRestrcitedServices_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.ResetSelectedProvider();
        this.RefreshRestrictedService();
    }

    private void RefreshRestrictedService()
    {
        _dtRestrcitedServices = null;
        LoadRestrcitedServices();
       // upRS_Main.Update();

    }

    private void ResetSelectedProvider()
    {
        if (this.grdRestrcitedServices.SelectedIndex > -1)
            this.grdRestrcitedServices.SelectedIndex = -1;
    }

  

    private void InitDataTable()
    {
        _dtRestrcitedServices = null;
    }

   
    private DataView GetSortedData()
    {
        if (dtRestrcitedServices != null)
        {
            DataView sortedView = new DataView(dtRestrcitedServices);

            if (!string.IsNullOrEmpty(dirSortExpression))
            {
                sortedView.Sort = dirSortExpression + " " + (dirRestrictedService == SortDirection.Ascending ? "Asc" : "Desc");
            }

            return sortedView;
        }
        return null;
    }

    public SortDirection dirRestrictedService
    {
        get
        {
            if (ViewState["dirRestrictedService"] == null)
            {
                ViewState["dirRestrictedService"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirRestrictedService"];
        }
        set
        {
            ViewState["dirRestrictedService"] = value;
        }
    }

    public String dirSortExpression
    {
        get { return ViewState["SortExpression"] as String ?? ""; }
        set { ViewState["SortExpression"] = value; }
    }

    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        return false;
    }

    protected void btnFilterGrid_Click(object sender, EventArgs e)
    {
        ResetSelectedProvider();
        RefreshRestrictedService();
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {


        ResetSelectedProvider();
        RefreshRestrictedService();
    }

  

    private void KeepPopupOpen()
    {
        this.mpeRS.Show();
    }

    private void CancelPopup()
    {
        this.mpeRS.Hide();
        LoadRestrcitedServices();
    }

   private DataRow SelectedRestrictedService(int id)
    {
        DataRow dr = null;

       if(dtRestrcitedServices.Rows.Count>0)
        {
            dr = dtRestrcitedServices.Select("REG_RESTRICTION_ID =" + id).FirstOrDefault();

        }


        return dr;
    }
   
  

    public override string ValidationGroup
    {
        get { return "valRestrictedServices"; }
    }

    public override string Title
    {
        get { return "Restricted Servcies"; }
    }

    public override string IdText
    {
        get { return "ucRestrictedServices_" + this.WorkflowPage.RegistrationId; }
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = svc.SelectRegistrationDataWithParams("usp_GetRestrictedServicesHistory", parms);
            if (Helper.HasRows(ds))
            {
                grdHistoryExport.DataSource = ds.Tables[0];
                grdHistoryExport.DataBind();
                grdHistoryExport.MasterTableView.ExportToExcel();
            }
        }
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (_SortField.Equals(e.SortExpression))
        {
            _SortField = _SortField + " DESC";
        }
        else
        {
            _SortField = e.SortExpression;
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_GetRestrictedServicesHistory", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.PageIndex = 0;
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
        }
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_GetRestrictedServicesHistory", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.DataSource = ds.Tables[0];
            grd.PageIndex = e.NewPageIndex;
            grd.DataBind();
        }
    }
}