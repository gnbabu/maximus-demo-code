using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MMSWebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corp.Core.Libraries;

public partial class PopupControls_HOSPITALSREACH : BaseSectionControl
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
    private const string sectionName = "HospitalCostReportSearch";
    protected void Page_Load(object sender, EventArgs e)
    {
        string MedicaidNumber = this.WorkflowPage.MedicaidID;   // Request.QueryString["MedicaidNumber"];
        LoadProviderInformation(MedicaidNumber);
        LoadSettelementTypeDropdown();
        LoadLetterNameDropDown();
        LoadDateTypeDropDown();
        LoadReportTypeDropDown();
        LoadPeriodTypeDropDown();
    }
    public override string IdText
    {
        get { return "ucHospitalSearch_" + this.WorkflowPage.RegistrationId; }
    }
    private void LoadProviderInformation(string medicaidNumber)
    {
        DataSet ds = svc.SelectProviderByGRPMedicaidID(medicaidNumber);
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
        this.DataList = dtMisc;
        if (Helper.HasRows(dtMisc))
        {
            DataRow dr = dtMisc.Rows[0];
            this.RegIdTxt.Value = Helper.GetString("REG_ID", dr);
        }

    }
    private void LoadSettelementTypeDropdown()
    {
        DataSet ds = svc.GetSettelementTypes();

        if (Helper.HasRows(ds))
        {
            Helper.LoadDropDown(this.ddlSettlementType, ds.Tables[0], "Settlement_TYPE_NAME", "Settlement_TYPE_VALUE", false);
        }
    }

    protected void btnSettelementLetterSearch_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("DocumentID");
        dt.Columns.Add("LetterName");
        dt.Columns.Add("PeriodType");
        dt.Columns.Add("DateSent");

        DataRow dr = dt.NewRow();
        dr["DocumentID"] = "121212121212";
        dr["LetterName"] = "Hospital 2918 Interim Settelement";
        dr["PeriodType"] = "State Fiscal Year 2016";
        dr["DateSent"] = "10/21/2021";
        dt.Rows.Add(dr);
        DataRow dr1 = dt.NewRow();
        dr1["DocumentID"] = "121212121212";
        dr1["LetterName"] = "Hospital 2918 Interim Settelement";
        dr1["PeriodType"] = "State Fiscal Year 2016";
        dr1["DateSent"] = "10/21/2021";
        dt.Rows.Add(dr1);
        DataRow dr2 = dt.NewRow();
        dr2["DocumentID"] = "121212121212";
        dr2["LetterName"] = "Hospital CS HCAP";
        dr2["PeriodType"] = "State Fiscal Year 2016";
        dr2["DateSent"] = "10/21/2021";
        dt.Rows.Add(dr2);
        gridHospitalSettelementLetterSearch.DataSource = dt;
        gridHospitalSettelementLetterSearch.DataBind();
    }
    protected void btnSettelementSearch_Click(object sender, EventArgs e)
    {
        DataSet dsType = new DataSet();
        try
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, Convert.ToInt32(this.RegIdTxt.Value), false));
            parameters.Add(SqlParms.CreateParameter("COST_REPORT_FISCAL_YEAR", DbType.String, ddlPeriodType.SelectedValue, false));
            parameters.Add(SqlParms.CreateParameter("REPORT_TYPE_ID", DbType.Int32, Convert.ToInt32(ddlReportType.SelectedValue), false));
            dsType = DataAccess.ExecuteStoredProcedure("usp_SelectREG_SettlementReport", parameters, "DocumentTypes");
            if (dsType != null && dsType.Tables[0] != null && dsType.Tables[0].Rows.Count > 0)
            {
                gridHospitalSettlementSearchResults.DataSource = dsType.Tables[0];
                gridHospitalSettlementSearchResults.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;


        }


    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DataSet dataSet = svc.HospitalSearchCostReportDetails();
        if (Helper.HasRows(dataSet))
        {
            DataTable dt = dataSet.Tables[3];
            dt.Columns.Add("NPI");
            dt.Columns.Add("DateReceived");
            dt.Columns.Add("ProviderName");
            dt.Columns.Add("DocType");

            foreach (DataRow row in dataSet.Tables[3].Rows)
            {
                var trackingId = Convert.ToString(row["MITSTrackingNumber"]);
                DataSet dsType = new DataSet();
                try
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("TRACKING_ID", DbType.String, trackingId, false));
                    dsType = DataAccess.ExecuteStoredProcedure("usp_SelectREG_HospitalSearchReport", parameters, "DocumentTypes");
                    if (dsType != null && dsType.Tables[0] != null && dsType.Tables[0].Rows.Count > 0)
                    {
                        var dr = dsType.Tables[0].Rows[0];
                        row["NPI"] = dr["NPI"];
                        row["DateReceived"] = DateTime.Now.ToString();
                        row["ProviderName"] = dr["NAME"];
                        row["DocType"] = dr["DOCUMENT_SERVICE_NAME"];
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }
            gvRemittanceAdvicesearch.DataSource = dt;
            gvRemittanceAdvicesearch.DataBind();
        }
    }


    public override bool ValidateData()
    {
        return true;
    }
    public override string ValidationGroup
    {
        get { return "valHospitalCostReportSearch"; }
    }
    public override void LoadControlData() { }
    public override bool SaveData() { return true; }
    public override void LoadData(DataRow dr)
    {
    }
    public override string Title
    {
        get { return "Hospital Cost Report Search"; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    private void LoadLetterNameDropDown()
    {
        DataSet ds = svc.GetLetterTypes();

        if (Helper.HasRows(ds))
        {
            Helper.LoadDropDown(this.ddlLetterType, ds.Tables[0], "LETTER_NAME", "LETTER_ID", false);
        }
    }
    private void LoadDateTypeDropDown()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("DateTypeId");
        dt.Columns.Add("DateTypeName");
        DataRow dr = dt.NewRow();
        dr["DateTypeId"] = "1";
        dr["DateTypeName"] = "Previous Month";
        dt.Rows.Add(dr);
        DataRow dr1 = dt.NewRow();
        dr1["DateTypeId"] = "2";
        dr1["DateTypeName"] = "Previous Year";
        dt.Rows.Add(dr1);
        DataRow dr2 = dt.NewRow();
        dr2["DateTypeId"] = "3";
        dr2["DateTypeName"] = "Custom";
        dt.Rows.Add(dr2);
        DataRow dr3 = dt.NewRow();
        dr3["DateTypeId"] = "4";
        dr3["DateTypeName"] = "Fiscal Year";
        dt.Rows.Add(dr3);

        Helper.LoadDropDown(this.ddlDateType, dt, "DateTypeName", "DateTypeId", false);

    }
    private void LoadReportTypeDropDown()
    {
        DataSet ds = svc.GetReportTypes();

        if (Helper.HasRows(ds))
        {
            Helper.LoadDropDown(this.ddlReportType, ds.Tables[0], "REPORT_NAME", "REPORT_ID", false);
        }
    }
    private void LoadPeriodTypeDropDown()
    {
        DataSet ds = svc.GetPeriodTypes();

        if (Helper.HasRows(ds))
        {
            Helper.LoadDropDown(this.ddlPeriodType, ds.Tables[0], "PERIOD_TYPE_NAME", "PERIOD_TYPE_NAME", false);
        }
    }

    protected void gridHospitalSettlementSearchResults_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        string filename = this.gridHospitalSettlementSearchResults.DataKeys[index].Values["DOCUMENT_ID"].ToString();
        DownloadFile(filename);
    }
    private void DownloadFile(string fileName)
    {
        try
        {
            string filePath = string.Empty;

            fileName = fileName.Replace("\"", string.Empty);
            fileName = fileName.Replace(",", "");
            filePath = Path.Combine(Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName);
#if DEBUG
                   @filePath = @"C:\Temp";
#endif

            //Local dev Test
            //filePath = @"C:\Projects\Upload\" + fileName;

            // If isFileLocal is false, that means file is on onbase. 
            bool isFileLocal = bool.Parse(AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString));

            if (isFileLocal && !File.Exists(filePath))
            {

                var error = "File or directory does not exist.";
                return;
            }


            //filePath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName;
            //FilePath = @"C:\Projects\PDMS2_0_0\PDMS\PDMS\FileStoreLocal\" + fileName;

            bool fileExistsonLocal = System.IO.File.Exists(filePath);
            bool downloadFile = isFileLocal ? fileExistsonLocal : true;

            OnBaseInterface onBaseInterface = new OnBaseInterface();
            byte[] decryptedFile = onBaseInterface.RetrieveFile(filePath);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(decryptedFile);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();

        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {

            //lblErrorMessages.Text = "an error has occurred during the download operation";
        }

    }

    protected void gridHospitalSettlementSearchResults_SelectedIndexChanged(object sender, EventArgs e)
    {
        var te = gridHospitalSettlementSearchResults.SelectedRow.RowIndex;
        int index = Convert.ToInt32(gridHospitalSettlementSearchResults.SelectedRow.Cells[0].Text);
        string filename = this.gridHospitalSettlementSearchResults.DataKeys[index].Values["FileName"].ToString();
        DownloadFile(filename);
    }

    protected void gridHospitalSettlementSearchResults_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType==DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gridHospitalSettlementSearchResults, "Select$" + e.Row.RowIndex);
        }
    }
}