using AjaxControlToolkit;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OtherAddressHistory : BaseSectionControl
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

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
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

    public void LoadData()
    {
	    PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
	    Dictionary<string, string> parms = new Dictionary<string, string>();
	    parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
	    parms.Add("ADDRESS_TYPE_ID", CON.AddressType.Other.ToString());
	    DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_OTHER_ADDRESS_HISTORY", parms);
	    if (Helper.HasRows(ds))
	    {
            grdOtherAddressHistory.DataSource = ds.Tables[0];
            grdOtherAddressHistory.DataBind();
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
        get { return "OtherAddressHistory"; }
    }

    public override string Title
    {
        get { return "Other Address History"; }
    }

    public override string IdText
    {
        get { return "ucOtherAddressHistory_" + this.WorkflowPage.RegistrationId; }
    }

    protected void grdOtherAddressHistory_Sorting(object sender, GridViewSortEventArgs e)
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
        parms.Add("ADDRESS_TYPE_ID", CON.AddressType.Other.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_OTHER_ADDRESS_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grdOtherAddressHistory.PageIndex = 0;
            grdOtherAddressHistory.DataSource = ds.Tables[0];
            grdOtherAddressHistory.DataBind();
        }
        ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
        mpe.Show();
    }

    protected void grdOtherAddressHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("ADDRESS_TYPE_ID", CON.AddressType.Other.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_OTHER_ADDRESS_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grdOtherAddressHistory.DataSource = ds.Tables[0];
            grdOtherAddressHistory.PageIndex = e.NewPageIndex;
            grdOtherAddressHistory.DataBind();
        }
        ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
        mpe.Show();
    }
}