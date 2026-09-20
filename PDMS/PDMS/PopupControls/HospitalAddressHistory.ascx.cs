using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_HospitalAddressHistory : BaseSectionControl
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
        parms.Add("ADDRESS_TYPE_ID", CON.SectionTypeID.HospitalAddress.ToString());
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_ADDRESS_SECTION_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            grdHospitalAddressHistory.DataSource = ds.Tables[0];
            grdHospitalAddressHistory.DataBind();
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
        get { return "HospitalHistory"; }
    }

    public override string Title
    {
        get { return "Hospital Address History"; }
    }

    public override string IdText
    {
        get { return "ucHospitalHistory_" + this.WorkflowPage.RegistrationId; }
    }

    protected void grdHospitalAddressHistory_Sorting(object sender, GridViewSortEventArgs e)
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
        parms.Add("ADDRESS_TYPE_ID", CON.SectionTypeID.HospitalAddress.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_ADDRESS_SECTION_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grdHospitalAddressHistory.PageIndex = 0;
            grdHospitalAddressHistory.DataSource = ds.Tables[0];
            grdHospitalAddressHistory.DataBind();
        }
        ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
        mpe.Show();
    }

    protected void grdHospitalAddressHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("ADDRESS_TYPE_ID", CON.SectionTypeID.HospitalAddress.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_ADDRESS_SECTION_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grdHospitalAddressHistory.DataSource = ds.Tables[0];
            grdHospitalAddressHistory.PageIndex = e.NewPageIndex;
            grdHospitalAddressHistory.DataBind();
        }
        ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
        mpe.Show();
    }
}