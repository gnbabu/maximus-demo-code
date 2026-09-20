using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Reflection;
using CON = MAXIMUS.Core.Libraries;

public partial class Reports_reportviewer : System.Web.UI.Page
{
    String reportName = "";
    String DisplayName = "";
    String SideLableContent = "";
    string qsReportParameter = string.Empty;

    private static CON.Logging log = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        string logMsg = String.Format(CON.Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
        log = new CON.Logging(Guid.NewGuid(), logMsg);
        log.CreateLogEntry("ReportViewer Page_Load starts.", CON.Logging.LogPriority.Error);
       
        //don't show page unless user is logged in
        if (!User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/default.aspx");
        }
        if (!IsPostBack)
        {
            if (Request["ReportName"] != null)
            {
                string qsReportName = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(Request["ReportName"].ToString(),true); 
               
                log.CreateLogEntry(String.Format("{0}-{1}", "ReportViewer ReportName", qsReportName), CON.Logging.LogPriority.Error);
                //Changed the delimeter to "|" to support "," in the URL - 4976
                var qsReportNames = qsReportName.Split('|');
                reportName = qsReportNames[0];
                DisplayName = qsReportNames[1];
                if (qsReportNames.Length > 2)
                {
                    SideLableContent = qsReportNames[2];
                }
                lbltitle.Text = DisplayName;
                lblSideContent.Text = SideLableContent;
            }
            if(Request["ReportParameter1"] != null)
            {
                qsReportParameter  = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(Request["ReportParameter1"].ToString(), true);
            }            

            loadreport(reportName, qsReportParameter);
        }
    }
    private void loadreport(String reportName , string ReportParameter1 ="")
    {
        string logMsg = String.Format(CON.Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
        log = new CON.Logging(Guid.NewGuid(), logMsg);
        try
        {
            log.CreateLogEntry("Reports loadreport Starts.", CON.Logging.LogPriority.Error);
            ReportViewer1.ShowReportBody = true;
            ServerReport serverReport = ReportViewer1.ServerReport;
            /*ReportParameterCollection reportParameter = new ReportParameterCollection();
            reportParameter = setReportParameter(reportName);*/
            ReportViewer1.ProcessingMode = ProcessingMode.Remote;
            string serverURL = CON.AppSettings.Get("ReportServerURL", string.Empty);
            serverReport.ReportServerUrl = new Uri(serverURL);
            serverReport.ReportPath = "/" + CON.AppSettings.Get("ReportServerFolderPath", string.Empty) + "/" + Helper.CleanFilePath(reportName);
            
            if(!string.IsNullOrWhiteSpace(ReportParameter1))
            {
                List<ReportParameter> paramList = new List<ReportParameter>();
                paramList.Add(new ReportParameter("ReportParameter1", ReportParameter1, false));
                ReportViewer1.ServerReport.SetParameters(paramList);
            }
            //ReportViewer1.ServerReport.SetParameters(reportParameter);
            log.CreateLogEntry(String.Format("{0}-{1}", "ReportViewer ReportServerUrl", serverReport.ReportServerUrl), CON.Logging.LogPriority.Error);
            log.CreateLogEntry(String.Format("{0}-{1}", "ReportViewer ReportPath", serverReport.ReportPath), CON.Logging.LogPriority.Error);
            ReportViewer1.DataBind();
            ReportViewer1.ServerReport.Refresh();
            serverReport.Refresh();
        }
        catch (Exception ex)
        {            
            log.CreateLogEntry(string.Format("{0} {1}", "Reports Error : ",ex.Message + ex.InnerException), CON.Logging.LogPriority.Error);
        }
      }
    //removed setReportParameter() - need not set default parameters


    protected void BtnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Reports/reportCriteria.aspx");

    }

    protected void ImageButtonHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

    protected void rv_SubmittingParameterValues(object sender, EventArgs e)
    {
        if (Request["ReportName"] != null)
        {
            string queryString = Request["ReportName"].ToString();//reportName = Request["ReportName"].ToString();
            reportName = queryString.Split('|')[0];
        }
        if (Request["ReportParameter1"] != null)
        {
            qsReportParameter = Request["ReportParameter1"].ToString();
        }
        ReportViewer1.ShowReportBody = true;
        loadreport(reportName,qsReportParameter );


    }
}