using Corp.Core.Libraries;
using Corp.Core.Libraries.Helper;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MMSWebControls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI.Widgets;

public partial class Process_AgentBulkUpload : System.Web.UI.Page
{
    private DataTable dtBulkAgents = null;
    private Logging log = null;
    private string sortDirection = "";
    private string sortExpression = "";

    private bool displayMPE
    {
        get { return Convert.ToBoolean(ViewState["displayMPE"]); }
        set { ViewState["displayMPE"] = value; }
    }

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

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
            Page.Theme = "Modernization";
        else
            Page.Theme = "Default";

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (displayMPE) { SetMPEVisible(true); }
            else { SetMPEVisible(false); }
        }
        LoadBulkAgents();
        Guid guidUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        ucUploadFile.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        ucUploadFile.ValidFileExtensions = "xlsx,xls";
        ucUploadFile.RedirectToPage = 2;
        ucUploadFile.CancelEvent += new PopupControls_UploadFilePT.CancelEventHandler(View_Cancel);     
    }
    private void View_Cancel()
    {
        SetMPEVisible(false);
    }

    private void SetMPEVisible(bool isDisplayMPE)
    {
        //if (isDisplayMPE)
        //{
        //    displayMPE = true;
        //    mpe.Show();
        //}
        //else
        //{
        //    displayMPE = false;
        //    this.mpe.Hide();
        //}
    }

    private DataTable LoadBulkAgents(string sortFieldDirection = null)
    {
        try
        {
            if (dtBulkAgents == null)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                Guid guidUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
                DataSet dsAffiliates = psc.GetBulkAgentFilesByUser(guidUser);
                dtBulkAgents = dsAffiliates.Tables[0];
            }
            if (sortFieldDirection != null)
            {
                dtBulkAgents.DefaultView.Sort = sortFieldDirection;
                dtBulkAgents = dtBulkAgents.DefaultView.ToTable(true);
            }

            gvAgentUploadHistory.DataSource = dtBulkAgents;
            gvAgentUploadHistory.DataBind();
            return dtBulkAgents;
        }
        catch (Exception ex)
        {
            log.CreateLogEntry("Failed to Get Bulk Agent Files By User"
                                 + " Exception Message " + ex.Message + " Exception Stack = "
                                 + ex.StackTrace, Logging.LogPriority.Error);
            return null;
        }

    }
    //protected void btnUploadFile_Click(object sender, EventArgs e)
    //{
    //    lblTitle.Text = "Agent Bulk Upload";
    //    ucUploadFile.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
    //    ucUploadFile.ValidFileExtensions = "xlsx,xls";
    //    ucUploadFile.RedirectToPage = 2;
    //    SetMPEVisible(true);
    //}

    protected void gvAgentUploadHistory_SortCommand(object sender, GridViewSortEventArgs e)
    {
        SetSortDirection();
        sortExpression = e.SortExpression + " " + sortDirection;
        ViewState["SortExpression"] = sortExpression;
        LoadBulkAgents(sortExpression);
    }

    protected void gvAgentUploadHistory_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = 0;
        if (e.CommandName == "upload")
        {
            index = Convert.ToInt32(e.CommandArgument);
            string filename = this.gvAgentUploadHistory.DataKeys[index].Values["Request_File"].ToString();
            int onBaseDocID = string.IsNullOrEmpty(this.gvAgentUploadHistory.DataKeys[index].Values["ONBASE_DOCUMENT_ID"].ToString()) ? 0 : Convert.ToInt32(this.gvAgentUploadHistory.DataKeys[index].Values["ONBASE_DOCUMENT_ID"].ToString());
            DownloadFileFromOnbase(onBaseDocID, filename);
        }
        else if (e.CommandName == "response")
        {
            index = Convert.ToInt32(e.CommandArgument);
            string filename = this.gvAgentUploadHistory.DataKeys[index].Values["Response_File"].ToString();
            int onBaseDocID = string.IsNullOrEmpty(this.gvAgentUploadHistory.DataKeys[index].Values["RESPONSE_FILE_ONBASE_DOC_ID"].ToString()) ? 0 : Convert.ToInt32(this.gvAgentUploadHistory.DataKeys[index].Values["RESPONSE_FILE_ONBASE_DOC_ID"].ToString());
            DownloadFileFromOnbase(onBaseDocID, filename);
        }
    }
    private void DownloadFileFromOnbase(int onBaseDocID, string fileName)
    {
        try
        {

            OnBaseInterface onBaseInterface = new OnBaseInterface();
            byte[] decryptedFile = onBaseInterface.RetrieveFilebyOnBaseDocID(onBaseDocID.ToString());

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
        catch (SqlException ex)
        {
             if (ex.Message.Contains("Subquery returned more than 1 value"))
            {
                MessageBox2.Show("test data in the database where duplicate filenames exist is causing an error; 'Remove' your file and upload it with a different name", "Error");
            }
            else
            {
                 MessageBox2.Show("a database error has occurred during the download operation", "Error");
            }
        }
        catch (Exception ex)
        {
#if DEBUG
            MessageBox2.Show("Error.: " + ex.Message, "Error");
#endif
            MessageBox2.Show("an error has occurred during the download operation: " + ex.Message, "Error");

        }

    }
    protected void gvAgentUploadHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["SortExpression"] == null)
            sortExpression = "File_Upload_Date " + (string.IsNullOrEmpty(sortDirection) ? "DESC" : sortDirection);
        else
            sortExpression = ViewState["SortExpression"].ToString();
        DataTable dtAffiliates = LoadBulkAgents(sortExpression);
        gvAgentUploadHistory.DataSource = dtAffiliates;
        gvAgentUploadHistory.PageIndex = e.NewPageIndex;
        gvAgentUploadHistory.DataBind();
    }

    protected void gvAgentUploadHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;
        string status = DataBinder.Eval(e.Row.DataItem, "File_Upload_Status").ToString();
        if (status != null && status.ToLower() == "complete" || status.ToLower() == "rejected")
        {
            LinkButton lnkResponsePath = (LinkButton)e.Row.FindControl("lnkResponsePath");
            //lnkResponsePath.Visible = true;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            foreach (TableCell cell in e.Row.Cells)
            {
                if (cell.Text == "REJECTED")
                    cell.Attributes.Add("title", "File was rejected for incorrect formatting. Please download the template and try again.");
            }
        }
    }

    protected void SetSortDirection()
    {
        if (ViewState["SortDirection"] != null && ViewState["SortDirection"].ToString() == "ASC")
        {
            sortDirection = "DESC";
        }
        else
        {
            sortDirection = "ASC";
        }
        ViewState["SortDirection"] = sortDirection;
    }

    //protected void lbTemplate_Click(object sender, EventArgs e)
    //{
    //    DownloadTemplateFile("Bulk_Upload_Agent_Template.xlsx");    
    //}

    //protected void lbRoles_Click(object sender, EventArgs e)
    //{
    //    DownloadTemplateFile("Agent_Roles.xlsx");
    //}

    //private void DownloadTemplateFile(string fileName)
    //{
    //    try
    //    {
    //        //string templateActualPath = AppSettings.Get("PowerAgentTemplateFSXPath"); //HttpRuntime.AppDomainAppPath + @"Documents\\";
    //        //string filepath = templateActualPath + fileName;
    //        //byte[] file = File.ReadAllBytes(filepath);
    //        //HttpContext.Current.Response.Clear();
    //        //HttpContext.Current.Response.Buffer = true;
    //        //HttpContext.Current.Response.ContentType = "application/force-download";
    //        //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
    //        //HttpContext.Current.Response.BinaryWrite(file);
    //        //HttpContext.Current.Response.Flush();
    //        //HttpContext.Current.Response.Close();
    //        //HttpContext.Current.Response.End();
      
    //        string templateActualPath = AppSettings.Get("PowerAgentTemplateFSXPath");
    //        string filepath = Path.Combine(templateActualPath, fileName);

    //        if (System.IO.File.Exists(filepath))
    //        {
    //            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);

    //            Response.Clear();
    //            Response.Buffer = true;
    //            Response.ContentType = "application/octet-stream";
    //            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
    //            Response.BinaryWrite(fileBytes);
    //            Response.Flush();
    //            Response.End();
    //        }
    //        else
    //        {
    //            // Handle file not found
    //            Response.Write("File not found.");
    //        }
    //    }
    //    catch (Exception e) { }
    //}
}