using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Views_CallReportingView : System.Web.UI.UserControl, ICallReportView
{
    #region Properties
    private CallReportPresenter _presenter;

    public CallReportPresenter Presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new CallReportPresenter(this);
            }

            return _presenter;
        }
    }

    public CallTrackingData Model { get; set; }

    #endregion

    #region Public Methods

    public void InitView()
    {
        Presenter.Init();
        InitFormFields();
    }
    #endregion

    #region Page Events
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        RefreshData();
        lnkExcel.Visible = lnkPDF.Visible = gvCalls.MasterTableView.Items.Count > 0;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtStartDate.Text = txtEndDate.Text = txtNPI.Text = txtMedicaidID.Text = txtCallID.Text = string.Empty;
        ddlSource.SelectedIndex = ddlReason.SelectedIndex = ddlResolution.SelectedIndex = -1;
        lnkExcel.Visible = lnkPDF.Visible = false;
        this.gvCalls.DataSource = null;
        this.gvCalls.DataBind();
    }

    protected void gvCalls_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void gvCalls_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {

    }
    protected void gvCalls_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }
    protected void gvCalls_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {

    }
    protected void gvCalls_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "ViewCallDetails")
        { 
            System.Collections.Generic.Dictionary<string, string> keyValues = new System.Collections.Generic.Dictionary<string,string>();
            ((Telerik.Web.UI.GridEditableItem)(e.Item)).ExtractValues(keyValues);
            string callID = keyValues["CallID"];
            if (callID.Trim() != string.Empty)
            {
                DataSet ds = Presenter.GetCallDetails(int.Parse(callID));
                if (ObjectControllerHelper.HasRows(ds))
                {
                    lblActionTakenText.Text = ObjectControllerHelper.GetString("ResolutionName", ds.Tables[0].Rows[0]);
                    txtCallDetails.Text = ObjectControllerHelper.GetString("CallDetails", ds.Tables[0].Rows[0]);
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "showConfirm",
                        "$(function() {var $divCallDetails = $( \"#" + divCallDetails.ClientID + "\" );"
                        + "setTimeout(function() {"
                        + "$divCallDetails.dialog({ modal: true, width: 600}); "
                        + "$divCallDetails.css(\"display\", \"block\"); "
                        + "$divCallDetails.dialog(\"widget\").css(\"z-index\", (maxZIndex() + 10)); "
                        + "}, 100); "
                        + "}); ", true);
                }
            }
        }
    }

    protected void lnkPDF_Click(object sender, EventArgs e)
    {
        this.Model = this.LoadModelFromForm();
        Presenter.RequestCallsByDateRange(MAXIMUS.Core.Libraries.Constants.CallReportingOutputType.PDF);

        RadGridExport.MasterTableView.ExportToPdf();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        this.Model = this.LoadModelFromForm();
        Presenter.RequestCallsByDateRange(MAXIMUS.Core.Libraries.Constants.CallReportingOutputType.Excel);

        RadGridExport.MasterTableView.ExportToExcel();
    }
    #endregion

    #region View Methods

    public void SetCallSources(DataSet data)
    {
        this.ddlSource.Items.Clear();

        this.ddlSource.DataSource = data;
        this.ddlSource.DataValueField = "CALLTRACKING_SOURCE_ID";
        this.ddlSource.DataTextField = "NAME";
        this.ddlSource.DataBind();
        if (this.ddlSource.Items.Count > 1)
        {
            this.ddlSource.Items.Insert(0, new ListItem("", "0"));
        }
    }

    public void SetCallReasons(DataSet data)
    {
        this.ddlReason.Items.Clear();

        this.ddlReason.DataSource = data;
        this.ddlReason.DataValueField = "CALLTRACKING_SUBJECT_ID";
        this.ddlReason.DataTextField = "NAME";
        this.ddlReason.DataBind();
        if (this.ddlReason.Items.Count > 1)
        {
            this.ddlReason.Items.Insert(0, new ListItem("", "0"));
        }
    }

    public void SetCallResolutions(DataSet data)
    {
        this.ddlResolution.Items.Clear();

        this.ddlResolution.DataSource = data;
        this.ddlResolution.DataValueField = "CALLTRACKING_RESOLUTION_ID";
        this.ddlResolution.DataTextField = "NAME";
        this.ddlResolution.DataBind();
        if (this.ddlResolution.Items.Count > 1)
        {
            this.ddlResolution.Items.Insert(0, new ListItem("", "0"));
        }
    }

    public void SetCallData(DataSet ds, int outputType)
    {
        if (outputType == MAXIMUS.Core.Libraries.Constants.CallReportingOutputType.Grid)
        {
            this.gvCalls.DataSource = ds;
            //this.gvCalls.VirtualItemCount = totalRowCount;
            this.gvCalls.DataBind();
        }
        else 
        {
            if (ds.Tables[0].Rows.Count > 1000)
            {
                this.RadGridExport.DataSource = ds.Tables[0].Rows.Cast<System.Data.DataRow>().Take(1000);
            }
            else
                this.RadGridExport.DataSource = ds;
            this.RadGridExport.DataBind();

        }

        if (ObjectControllerHelper.HasRows(ds))
        {
            lnkExcel.Visible = lnkPDF.Visible = true;
            hdnRowCount.Value = ds.Tables[0].Rows.Count.ToString();
        }
    }

    public void SetErrorMessages()
    {
        if (Presenter.hasErrors)
        {
            ClearErrorMessages();
            SetValidationErrors();
        }
    }
    #endregion

    #region Private Methods
    private void InitFormFields()
    {
        this.txtStartDate.Text = string.Empty;
        this.txtEndDate.Text = string.Empty;

        if (this.ddlSource.Items.Count > 0)
            this.ddlSource.SelectedIndex = 0;
        if (this.ddlReason.Items.Count > 0)
            this.ddlReason.SelectedIndex = 0;
        if (this.ddlResolution.Items.Count > 0)
            this.ddlResolution.SelectedIndex = 0;

        this.txtCallID.Text = string.Empty;
        this.txtNPI.Text = string.Empty;
        this.txtMedicaidID.Text = string.Empty;

        //Initialize Grid
        InitGrid();

    }

    private void InitGrid()
    {
        //can add different export types - and can modify this format based on type selected.  Defaulting to xlsc
        this.gvCalls.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), "Xlsx");
    }

    private void RefreshData()
    {
        lblErrorMessages.Text = string.Empty;
        if (!Page.IsValid)
        {
            return;
        }

        this.Model = this.LoadModelFromForm();
        Presenter.RequestCallsByDateRange(MAXIMUS.Core.Libraries.Constants.CallReportingOutputType.Grid);
        if (Presenter.hasErrors)
        {
            SetValidationErrors();
        }
    }

    private CallTrackingData LoadModelFromForm()
    {
        CallTrackingData data = new CallTrackingData();

        if (!string.IsNullOrEmpty(this.txtStartDate.Text))
        {
            data.StartTime = Convert.ToDateTime(this.txtStartDate.Text);
        }

        if (!string.IsNullOrEmpty(this.txtEndDate.Text))
        {
            data.EndTime = Convert.ToDateTime(this.txtEndDate.Text);
        }

        data.SourceID = this.ddlSource.SelectedIndex > -1 ? Convert.ToInt32(this.ddlSource.SelectedValue) : 0;
        data.SubjectID = this.ddlReason.SelectedIndex > -1 ? Convert.ToInt32(this.ddlReason.SelectedValue) : 0;
        data.ResolutionID = this.ddlResolution.SelectedIndex > -1 ? Convert.ToInt32(this.ddlResolution.SelectedValue) : 0;

        data.CallTrackingID = (this.txtCallID.Text.Trim() != string.Empty) ? int.Parse(this.txtCallID.Text.Trim()) : 0;

        if (!string.IsNullOrEmpty(this.txtNPI.Text))
        {
            data.NPI = this.txtNPI.Text.Trim();
        }

        if (!string.IsNullOrEmpty(this.txtMedicaidID.Text))
        {
            data.MedicaidID = this.txtMedicaidID.Text.Trim();
        }

        return data;
    }

    private void SetValidationErrors()
    {
        foreach (var pair in Presenter.ErrorList)
        {
            AddErrorMessage(pair.Value);
        }
    }

    private void AddErrorMessage(string message)
    {
        lblErrorMessages.Text = string.Format("{0}{1}<br />", lblErrorMessages.Text, message);
    }

    private void ClearErrorMessages()
    {
        lblErrorMessages.Text = string.Empty;
    }

    private void ExportData(string exportType)
    {
        //just handles export to excel now
        this.gvCalls.ExportSettings.Excel.Format = GridExcelExportFormat.Xlsx;
        this.gvCalls.ExportSettings.IgnorePaging = true;
        this.gvCalls.ExportSettings.ExportOnlyData = true;
        this.gvCalls.ExportSettings.OpenInNewWindow = true;
        this.gvCalls.MasterTableView.ExportToExcel();
    }
    #endregion

}
