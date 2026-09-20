using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Widgets;

public partial class Maintenance_DesignStaticPages : System.Web.UI.Page
{
    private const int pageSize = 10;

    private int EditIndex
    {
        get { return (int)ViewState["EditIndex"]; }
        set { ViewState["EditIndex"] = value; }
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            Page.MasterPageFile = "~/MasterPage.master";
            //Page.MasterPageFile = "~/MasterPageNew.master";
            Page.Theme = "Modernization";
        }
        else
        {
            Page.MasterPageFile = "~/MasterPage.master";
            Page.Theme = "Default";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Helper.IsLoggedInUserInAdminRole())
            {
                SessionVarRetriever.MyQueueSelectedRoleName =
                SessionVarRetriever.MyQueueSelectedRoleValue =
                SessionVarRetriever.UserIdSelected = null;
            }
            EditIndex = -1;

        }
        LoadData();
        if (Helper.IsModern())
        {

        }
        string learningDocsPath = AppSettings.Get("LearningDocsFSXPath");
        edCustomEmail.DocumentManager.ViewPaths = new string[] { learningDocsPath };
        edCustomEmail.DocumentManager.UploadPaths = new string[] { learningDocsPath };
        edCustomEmail.DocumentManager.DeletePaths = new string[] { learningDocsPath };
        edCustomEmail.DocumentManager.ContentProviderTypeName = typeof(CustomFileSystemContentProvider).AssemblyQualifiedName;
    }

    private void LoadData()
    {
        PopulatePageDropdown();
    }

    private void SaveData()
    {
        try
        {

        }
        catch (Exception ex)
        {

        }
    }

    private void LoadUser(string username)
    {
        Response.Redirect(@"~/Process/AdminUserAccounts.aspx?username=" + username);
    }

    private bool ValidateData()
    {
        return true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!ValidateData()) return;
        SaveData();
        LoadData();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //TELERIK BUG WORKAROUND
        //Hiding table headers in IE for Data Pager and Command Item in code behind
        List<TableHeaderCell> headersToCheck = new List<TableHeaderCell>();
        foreach (Control c in this.Controls)
        {
            IEnumerable<TableHeaderCell> headersToAdd = GetControls(c).OfType<TableHeaderCell>();
            if (headersToAdd != null)
                headersToCheck.AddRange(headersToAdd);
        }
        foreach (TableHeaderCell th in headersToCheck)
        {
            if (th.Text == "Data pager" || th.Text == "Command item")
                th.Visible = false;
        }
    }

    private IEnumerable<Control> GetControls(Control parent)
    {
        foreach (Control parentControl in parent.Controls)
        {
            yield return parentControl;
            foreach (Control childControl in GetControls(parentControl))
            {
                yield return childControl;
            }
        }
    }

    protected void btnTest_Click(object sender, EventArgs e)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        string content = edCustomEmail.Content;
        string docFSXPath = AppSettings.Get("LearningDocsFSXPath");
       

        if (System.Diagnostics.Debugger.IsAttached)
        {
            content = content.Replace(docFSXPath, "/pages/ShowFiles.aspx?mode=inline&FileName=");
        }
        else
        {
            string envName = AppSettings.Get("EnvironmentName");
            content = content.Replace(docFSXPath, "/" + envName + "/pages/ShowFiles.aspx?mode=inline&FileName=");
        }

        if (!string.IsNullOrEmpty(hdnPagename.Value))
        {
            svc.UpdatePage_Configuration(Convert.ToInt32(hdnPagename.Value), ddlpage.SelectedValue, "", "", "", "", 0, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), 0, content);
        }
        else
        {
            svc.InsertPage_Configuration(ddlpage.SelectedValue, "", "", "", "", 0, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), 0, content);
        }
    }

    private List<string> FindHrefs(string input)
    {
        List<string> lst = new List<string>();
        Regex regex = new Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase);
        Match match;
        for (match = regex.Match(input); match.Success; match = match.NextMatch())
        {
            Regex regex1 = new Regex("FileName\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase);
            //foreach (Group group in match.Groups)
            //{
            Match m1 = regex1.Match(match.Value);
            string s = m1.Value.Replace("FileName=", "");
            lst.Add(s.Replace("\"", ""));
            //}
        }
        return lst;
    }

    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataSet ds = new DataSet();
        ds = psc.SelectPageConfigurationsByPageName(ddlpage.SelectedValue,new Guid());
        foreach (DataRow dr1 in ds.Tables[0].Rows)
        {
            hdnPagename.Value = Helper.GetString("Page_Configuration_ID", dr1);
            edCustomEmail.Content = Helper.GetString("displaytext", dr1);
        }
    }

    private void PopulatePageDropdown()
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["mainDB"].ConnectionString))
        using (SqlCommand cmd = new SqlCommand("usp_SelectREG_PAGE_TYPES", conn)) // Replace with your actual query or stored procedure
        {
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {

                if (ddlpage.Items.FindByValue("") == null)
                {
                    ddlpage.Items.Clear();
                    ddlpage.Items.Add(new ListItem("--Select Page--", ""));
                }


                while (reader.Read())
                {
                    string pageText = reader["PageName"].ToString();  // Replace with actual column name
                    string pageValue = reader["PageValue"].ToString(); // Replace with actual column name
                    if (ddlpage.Items.FindByValue(pageValue) == null)
                    {
                        ddlpage.Items.Add(new ListItem(pageText, pageValue));
                    }

                }
            }
        }
    }
}

public class CustomFileSystemContentProvider : FileSystemContentProvider
{
    public CustomFileSystemContentProvider(HttpContext context, string[] searchPatterns, string[] viewPaths, string[] uploadPaths, string[] deletePaths, string selectedUrl, string selectedItemTag)
    : base(context, searchPatterns, viewPaths, uploadPaths, deletePaths, selectedUrl, selectedItemTag)
    {
    }
    public override string StoreFile(UploadedFile file, string path, string name, params string[] arguments)
    {
        try
        {
            string destinationPath = path + name;
            if (File.Exists(destinationPath))
            {
                File.Delete(destinationPath);
            }
            using (var fileStream = new FileStream(destinationPath, FileMode.Create))
            {
                file.InputStream.CopyTo(fileStream);
            }
            return destinationPath;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public override DirectoryItem ResolveDirectory(string path)
    {
        return ReadDirectoryAndLoadFiles(path);
    }

    public override DirectoryItem ResolveRootDirectoryAsTree(string path)
    {
        return ReadDirectoryAndLoadFiles(path);
    }

    public override string MoveFile(string path, string newPath)
    {
        var filepathSplit = newPath.Split('/');
        var newFileName = filepathSplit[filepathSplit.Length - 1];
        string basePath  = AppSettings.Get("LearningDocsFSXPath");
        if (File.Exists(path))
        {
            File.Move(path, basePath + newFileName);
        }
        return string.Empty;
    }

    public override string DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        return string.Empty;
    }
    
    public override bool CheckDeletePermissions(string folderPath)
    {
        return true;
    }

    public override bool CheckReadPermissions(string folderPath)
    {
        return true;
    }

    public override bool CheckWritePermissions(string folderPath)
    {
        return true;
    }

    private static DirectoryItem ReadDirectoryAndLoadFiles(string path)
    {
        //string docFSXPath = AppSettings.Get("LearningDocsFSXPath");

        var existingFiles = new DirectoryInfo(path).GetFiles();
        
        List<FileItem> files = new List<FileItem>();
        foreach (var file in existingFiles)
        {
            FileItem fItem = new FileItem();
            fItem.Name = file.Name;
            fItem.Length = file.Length;
            fItem.Extension = file.Extension;
            fItem.Location = file.FullName;
            fItem.Path = file.FullName;
            fItem.Url = string.Empty;
            fItem.Permissions = PathPermissions.Delete | PathPermissions.Upload | PathPermissions.Read;
            fItem.Tag = string.Empty;
            files.Add(fItem);
        }

        DirectoryItem newDirectory = new DirectoryItem("Documents", path, path, string.Empty, PathPermissions.Upload | PathPermissions.Delete | PathPermissions.Read, files.ToArray(), new DirectoryItem[] { });
        return newDirectory;
 
    }



}