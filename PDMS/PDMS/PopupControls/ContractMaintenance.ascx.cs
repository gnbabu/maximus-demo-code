using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_ContractMaintenance : BaseSectionControl
{
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

    public List<string> ErrorMessages { get; set; }

    private string EffectiveDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    LoadContractMaintenancePage();
        //}

    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Contract Maintenance";
    }
    protected void OnDataBound(object sender, EventArgs e)
    {

    }

    private void LoadContractMaintenancePage()
    {
        LoadContractMaintenanceGrid();
    }
    public override bool SaveData()
    {
        return true;
    }

    public override void LoadControlData()
    {
        LoadContractMaintenancePage();
    }

    public override void LoadData(DataRow dr)
    {            

    }

    private void RefreshPage()
    {
        LoadContractMaintenanceGrid();        
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override string ValidationGroup
    {
        get { return "valContractMaintenance"; }
    }

    public override string Title
    {
        get { return "Contract Maintenance"; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string IdText
    {
        get { return "ucContractMaintenance_" + this.WorkflowPage.RegistrationId; }
    }

    private void LoadContractMaintenanceGrid()
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "Contracts");
        ContractMaintananceGrid.DataSource = this.DataList = ds.Tables[0];
        ContractMaintananceGrid.DataBind();
        if (Helper.HasRows(ds))
        {
            CMHeaderConvertedHistoricalData.Visible = true;
        }
        else
        {
            CMHeaderConvertedHistoricalData.Visible = false;
        }
    }
   
}