using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_HomeOfficeAddressHistory : BaseSectionControl
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
    //private int _start = 0;
    //private int _end = 10;
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
	    if (!IsPostBack)
		    LoadData();
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }

    public override void LoadControlData()
    {
        LoadData();
    }

    protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
	    grd.PageIndex = e.NewPageIndex;
	    LoadData();
    }

    public void LoadData()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("ADDRESS_TYPE_ID", CON.AddressType.HomeOffice.ToString());
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_ADDRESS_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
        }
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override bool SaveData()
    {
        return true;
    }

    public override void LoadData(DataRow row)
    {
    }


    public override string ValidationGroup
    {
        get { return "HomeOfficeHistory"; }
    }

    public override string Title
    {
        get { return "Home Office Address History"; }
    }

    public override string IdText
    {
        get { return "ucHomeOfficeHistory_" + this.WorkflowPage.RegistrationId; }
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
        parms.Add("ADDRESS_TYPE_ID", CON.AddressType.HomeOffice.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_ADDRESS_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.PageIndex = 0;
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
        }
        ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
        mpe.Show();
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("ADDRESS_TYPE_ID", CON.AddressType.HomeOffice.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_ADDRESS_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.DataSource = ds.Tables[0];
            grd.PageIndex = e.NewPageIndex;
            grd.DataBind();
        }
        ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
        mpe.Show();
    }
}