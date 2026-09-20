using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MAXIMUS.DataExchange.PDMS;
using MMSWebControls;
using System.Threading;
using Corp.Core.Libraries;

public partial class Process_AffiliateUpdate : System.Web.UI.Page
{

    private Logging log = null;
    private string sortDirection = "";
    private string sortExpression = "";
    private DataTable dtAffiliates = null;

    private bool displayMPE
    {
        get { return Convert.ToBoolean(ViewState["displayMPE"]); }
        set { ViewState["displayMPE"] = value; }
    }

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
        LoadAffiliates();
        Guid guidUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        ucUploadFile.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        ucUploadFile.ValidFileExtensions = "xlsx,xls";
        ucUploadFile.CancelEvent += new UserControls_UploadFile.CancelEventHandler(View_Cancel);
    }

    private DataTable LoadAffiliates(string sortFieldDirection = null)
    {
        try
        {
            if (dtAffiliates == null)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                Guid guidUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
                DataSet dsAffiliates = psc.GetAffiliateFilesByUser(guidUser);
                dtAffiliates = dsAffiliates.Tables[0];
            }
            if (sortFieldDirection != null)
            {
                dtAffiliates.DefaultView.Sort = sortFieldDirection;
                dtAffiliates = dtAffiliates.DefaultView.ToTable(true);
            }

            grdAffiliateUpdate.DataSource = dtAffiliates;
            grdAffiliateUpdate.DataBind();
            return dtAffiliates;
        }
        catch (Exception ex)
        {
            log.CreateLogEntry("Failed to Get Affiliate Files By User"
                                 + " Exception Message " + ex.Message + " Exception Stack = "
                                 + ex.StackTrace, Logging.LogPriority.Error);
            return null;
        }

    }

    private void View_Cancel()
    {
        SetMPEVisible(false);
    }

    private void SetMPEVisible(bool isDisplayMPE)
    {
        if (isDisplayMPE)
        {
            displayMPE = true;
            mpe.Show();
        }
        else
        {
            displayMPE = false;
            this.mpe.Hide();
        }
    }

    protected void btnUploadFile_Click(object sender, CommandEventArgs e)
    {
        lblTitle.Text = "Affiliate Update";
        ucUploadFile.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        ucUploadFile.ValidFileExtensions = "xlsx,xls";
        SetMPEVisible(true);
    }
    protected void grdAffiliateUpdate_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["SortExpression"] == null)
            sortExpression = "Affiliate_File_Upload_Date " + (string.IsNullOrEmpty(sortDirection) ? "DESC" : sortDirection);
        else
            sortExpression = ViewState["SortExpression"].ToString();
        DataTable dtAffiliates = LoadAffiliates(sortExpression);
        grdAffiliateUpdate.DataSource = dtAffiliates;
        grdAffiliateUpdate.PageIndex = e.NewPageIndex;
        grdAffiliateUpdate.DataBind();
    }

    protected void grdAffiliateUpdate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = 0;
        if (e.CommandName == "upload")
        {
            index = Convert.ToInt32(e.CommandArgument);
            bool fileDownloaded = false;
            string filename = this.grdAffiliateUpdate.DataKeys[index].Values["Request_File"].ToString();
            DownloadFile(filename, out fileDownloaded);
        }
        else if (e.CommandName == "response")
        {
            index = Convert.ToInt32(e.CommandArgument);
            bool fileDownloaded = false;
            string filename = this.grdAffiliateUpdate.DataKeys[index].Values["Response_File"].ToString();
            DownloadFile(filename, out fileDownloaded);
        }
    }

    protected void grdAffiliateUpdate_SortCommand(object sender, GridViewSortEventArgs e)
    {
        SetSortDirection();
        sortExpression = e.SortExpression + " " + sortDirection;
        ViewState["SortExpression"] = sortExpression;
        LoadAffiliates(sortExpression);
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

    protected void grdAffiliateUpdate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;
        string affiliationStatus = DataBinder.Eval(e.Row.DataItem, "Affiliate_File_Upload_Status").ToString();
        if (affiliationStatus != null && affiliationStatus.ToLower() == "complete" || affiliationStatus.ToLower() == "rejected")
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

    protected void lbAffliateUpdate_Click(Object sender, EventArgs e)
    {
        this.DownloadCR_045file();
    }
    private void DownloadCR_045file()
    {
        try
        {
            string templateActualPath = HttpRuntime.AppDomainAppPath + @"Documents\\";
            string filepath = templateActualPath + "CR045_Template.xlsx";
            byte[] file = File.ReadAllBytes(filepath);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + "CR045_Template.xlsx");
            HttpContext.Current.Response.BinaryWrite(file);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();
        }
        catch (Exception e) { }
    }

    private void DownloadFile(string fileName, out bool fileDownloaded)
    {
        fileDownloaded = false;
        try
        {
            string filePath = string.Empty;
            fileName = fileName.Replace("\"", string.Empty);
            fileName = fileName.Replace(",", "");
            filePath = Path.Combine(Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName);
#if DEBUG
                   @filePath = @"C:\Temp";
#endif

            // If isFileLocal is false, that means file is on onbase. 
            bool isFileLocal = bool.Parse(AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString));

            if (isFileLocal && !File.Exists(filePath))
            {
#if DEBUG
                    MessageBox2.Show("File or directory does not exist.: " + filePath, "Error");
#endif

                MessageBox2.Show("File or directory does not exist.: " + filePath, "Error");
            }
            else
            {
                fileDownloaded = true;
            }

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
        {
        }
        catch (Exception ex)
        {
#if DEBUG
                MessageBox2.Show("Error.: " + ex.Message, "Error");
#endif
            MessageBox2.Show("an error has occurred during the download operation: " + ex.Message, "Error");
        }

    }
}