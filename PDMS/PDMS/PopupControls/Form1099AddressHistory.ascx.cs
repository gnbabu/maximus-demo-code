using AjaxControlToolkit;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_Form1099AddressHistory : BaseSectionControl
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
        // JIRA 4156
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("ADDRESS_TYPE_ID", CON.AddressType.TaxForm1099.ToString());
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_1099_ADDRESS_HISTORY", parms);
        DataTable dt = null;
        if (Helper.HasRows(ds))
        {
            dt = ds.Tables[0];
            grdForm1099AddressHistory.DataSource = dt;
            grdForm1099AddressHistory.DataBind();
        }
    }

    protected void grdForm1099AddressHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[2].Text = Helper.FormatPhone(e.Row.Cells[2].Text);
        }
    }

    public override void LoadData(DataRow row)
    {

    }

    public override bool ValidateData()
    {
        return true;
    }

    public override bool SaveData()
    {
        return true;
    }

    public override string ValidationGroup
    {
        get { return "Form1099AddressHistory"; }
    }

    public override string Title
    {
        get { return "Form 1099 Address History"; }
    }

    public override string IdText
    {
        get { return "ucForm1099AddressHistory_" + this.WorkflowPage.RegistrationId; }
    }

    protected void grdForm1099AddressHistory_Sorting(object sender, GridViewSortEventArgs e)
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
        parms.Add("ADDRESS_TYPE_ID", CON.AddressType.TaxForm1099.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_1099_ADDRESS_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grdForm1099AddressHistory.PageIndex = 0;
            grdForm1099AddressHistory.DataSource = ds.Tables[0];
            grdForm1099AddressHistory.DataBind();
        }
        ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
        mpe.Show();
    }

    protected void grdForm1099AddressHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("ADDRESS_TYPE_ID", CON.AddressType.TaxForm1099.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_1099_ADDRESS_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grdForm1099AddressHistory.DataSource = ds.Tables[0];
            grdForm1099AddressHistory.PageIndex = e.NewPageIndex;
            grdForm1099AddressHistory.DataBind();
        }
        ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
        mpe.Show();
    }

}