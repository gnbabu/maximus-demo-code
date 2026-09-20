using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class Pages_MMISTransactions : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    private static DataSet mmisHistory { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    public override void LoadData(DataRow dr)
    {
    }
    protected void grdHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdHistory.PageIndex = e.NewPageIndex;
        grdHistory.DataSource = mmisHistory;
        grdHistory.DataBind();
    }

    public override bool SaveData()
    {
        return true;
    }

    public override bool ValidateData()
    {
        return true;
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        mmisHistory = psc.GetMMISTransactionsForRegistration(this.WorkflowPage.RegistrationId);
        grdHistory.DataSource = mmisHistory;
        DataRow regInfo = Registration.GetRegistration(this.WorkflowPage.RegistrationId);

        lblTaxId.Text = Helper.GetString("TAX_ID", regInfo);
        lblNPI.Text = Helper.GetString("NPI", regInfo);

        grdHistory.DataBind();
    }

    public override string ValidationGroup
    {
        get { return "valMMISTransactions"; }
    }

    public override string Title
    {
        get { return "MMIS Transactions"; }
    }

    public override string IdText
    {
        get { return "ucMMISTransactions_" + this.WorkflowPage.RegistrationId; }
    }
}