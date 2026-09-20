using System;
using System.Data;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_WaiverServicesdisplay : BaseSectionControl
{
    private const string sectionName = "WaiverServices";

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
    protected void Page_Load(object sender, EventArgs e)
    {
        DataSet ds = svc.GetRegWaiverServices(this.WorkflowPage.RegistrationId);
        LoadODAChoicesGrid(ds);
        LoadODAOthersGrid(ds);
        LoadDODDWaiverServiceGrid(ds);
        LoadODAAssistedWaiverServiceGrid(ds);
        LoadODAPassportWaiverServiceGrid(ds);

        //GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        //TableHeaderCell cell = new TableHeaderCell();
        //cell.Text = "DODD WAIVER";
        //cell.ColumnSpan = 3;
        //row.Controls.Add(cell);
        //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
        //DODDWaiverServiceGrid.HeaderRow.Parent.Controls.AddAt(0, row);

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
        get { return "valWaiverServices"; }
    }

    public override string Title
    {
        get { return "WaiverServices SEARCH"; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string IdText
    {
        get { return "ucWaiverServices_" + this.WorkflowPage.RegistrationId; }
    }

    protected void ODAOthers_DataBound(object sender, EventArgs e)
    {

    }

    private void LoadODAOthersGrid(DataSet dataSet)
    {
        var filterQuery = "SERVICE_CATEGORY_ID = " + CON.WaiverServiceCategoryType.Other;
        dataSet.Tables[0].DefaultView.RowFilter = filterQuery;
        ODAOthers.DataSource = dataSet.Tables[0];
        ODAOthers.DataBind();
    }
    private void LoadODAPassportWaiverServiceGrid(DataSet dataSet)
    {
        var filterQuery = "SERVICE_CATEGORY_ID = " + CON.WaiverServiceCategoryType.PASSPORT;
        dataSet.Tables[0].DefaultView.RowFilter = filterQuery;
        ODAPassportWaiverServiceGrid.DataSource = dataSet.Tables[0];
        ODAPassportWaiverServiceGrid.DataBind();
    }
    private void LoadODAAssistedWaiverServiceGrid(DataSet dataSet)
    {
        var filterQuery = "SERVICE_CATEGORY_ID = " + CON.WaiverServiceCategoryType.AssistedLiving;
        dataSet.Tables[0].DefaultView.RowFilter = filterQuery;
        ODAAssistedWaiverServiceGrid.DataSource = dataSet.Tables[0];
        ODAAssistedWaiverServiceGrid.DataBind();
    }
    private void LoadDODDWaiverServiceGrid(DataSet dataSet)
    {
        var filterQuery = "SERVICE_CATEGORY_ID IS NULL"; //+ CON.WaiverServiceCategoryType.Other;
        dataSet.Tables[0].DefaultView.RowFilter = filterQuery;
        DODDWaiverServiceGrid.DataSource = dataSet.Tables[0];
        DODDWaiverServiceGrid.DataBind();
    }
    private void LoadODAChoicesGrid(DataSet dataSet)
    {
        var filterQuery = "SERVICE_CATEGORY_ID = " + CON.WaiverServiceCategoryType.Choices;
        dataSet.Tables[0].DefaultView.RowFilter = filterQuery;
        ODAChoicesGrid.DataSource = dataSet.Tables[0];
        ODAChoicesGrid.DataBind();
    }
    protected void ODAPassportWaiverServiceGrid_DataBound(object sender, EventArgs e)
    {

    }

    protected void ODAAssistedWaiverServiceGrid_DataBound(object sender, EventArgs e)
    {

    }

    protected void DODDWaiverServiceGrid_DataBound(object sender, EventArgs e)
    {

    }

    protected void ODAChoicesGrid_DataBound(object sender, EventArgs e)
    {

    }
}