using System;
using System.Data;

public partial class PopupControls_BuildingHistory : BaseSectionControl
{
    private const string sectionName = "BuildingServices";

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
    public int RegID
    {
        get
        {

            if (ViewState["RegID"] != null)
            {
                return Convert.ToInt32(ViewState["RegID"]); ;
            }
            else
            {
                ViewState["RegID"] = (Request["RegID"] != null) ? Convert.ToInt32(Request["RegID"]) : 0;
                return Convert.ToInt32(ViewState["RegID"]);
            }
        }
        set
        {
            ViewState["RegID"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        DataSet dsl = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
        DataRow dr = Helper.HasRows(dsl) ? dsl.Tables[0].Rows[0] : null;
        lblMedText.Text = Helper.GetString("MEDICAID_ID", dr);
        lblHomeText.Text = Helper.GetString("LT_HOME_NUMBER", dr);
        lblIIDText.Text= Helper.GetString("dd_facility_number", dr);
        //lblIIDText.Text = string.Empty; // Helper.GetString("MEDICAID_ID", dr);

        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "BuildingHistory");
        BuildingHistoryGrid.DataSource = ds;
        BuildingHistoryGrid.DataBind();
    }
    protected void lnkPDF_Click(object sender, EventArgs e)
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "BuildingHistory");
        RadGridExport.DataSource =ds.Tables[0];
        RadGridExport.DataBind();
        RadGridExport.MasterTableView.ExportToPdf();
    }
    public override bool SaveData()
    {
        return true;
    }

    public override void LoadControlData()
    {

    }
    public override void LoadData(DataRow dr = null)
    {

    }
    public override bool ValidateData()
    {
        return true;
    }

    public override string ValidationGroup
    {
        get { return "valBuildingrServices"; }
    }

    public override string Title
    {
        get { return "BuildingServices SEARCH"; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string IdText
    {
        get { return "ucBuildingServices_" + this.WorkflowPage.RegistrationId; }
    }
}