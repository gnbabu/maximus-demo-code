using AjaxControlToolkit;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_TaxonomiesHistory : System.Web.UI.UserControl
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

    private bool HideIndexColumn()
    {
        bool rtn = false;
        int cnt = 0;
        int saveIdx = 0;
        foreach (GridViewRow row in grdTaxonomiesHistory.Rows)
        {
            int idx = string.IsNullOrEmpty(row.Cells[0].Text)?0:Convert.ToInt32(row.Cells[0].Text);

            if (idx != saveIdx)
            {
                saveIdx = idx;
                cnt += 1;
            }
        }
        if (cnt <= 1) rtn = true;
        return rtn;
    }

    public void LoadData(int primaryFlag)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PrimaryFlag", primaryFlag.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_TAXONOMYHistory", parms);
        if (Helper.HasRows(ds))
        {
            grdTaxonomiesHistory.DataSource = ds.Tables[0];
            grdTaxonomiesHistory.DataBind();
        }
    }
    protected void grdTaxonomiesHistory_Sorting(object sender, GridViewSortEventArgs e)
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
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_TAXONOMYHistory", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grdTaxonomiesHistory.PageIndex = 0;
            grdTaxonomiesHistory.DataSource = ds.Tables[0];
            grdTaxonomiesHistory.DataBind();
        }
        ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
        mpe.Show();
    }

    protected void grdTaxonomiesHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_TAXONOMYHistory", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grdTaxonomiesHistory.DataSource = ds.Tables[0];
            grdTaxonomiesHistory.PageIndex = e.NewPageIndex;
            grdTaxonomiesHistory.DataBind();
        }
        ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
        mpe.Show();
    }
}