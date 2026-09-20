using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_FeeScheduleContact : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            Page.Theme = "Modernization";
        }
        else
        {
            Page.Theme = "Default";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {


    }
    DataTable categoryTable = new DataTable();
    DataTable fileTable = new DataTable();
    Logging log = new Logging();
    private void LoadProviders()
    {
        try
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet categoryList = psc.GetCategoryFeeScheduleTypes();

            categoryTable.Columns.Add("Category_Name");
            categoryTable.Columns.Add("FILE_NAME");
            fileTable.Columns.Add("FILE_NAME");
            string feeScheduleFileOutPutPath = AppSettings.Get("FeeScheduleJobFolder");
            feeScheduleFileOutPutPath = feeScheduleFileOutPutPath + "DISPLAY";

            for (int i = 0; i < categoryList.Tables[0].Rows.Count; i++)
            {
                string fileName = categoryList.Tables[0].Rows[i]["FILE_NAME"].ToString().Replace(".csv", "");
                //Files are coming with a date suffix added at the end of the file.
                //so, search * for the files to pick all the files with same name with any suffix
                string[] csvFiles = Directory.GetFiles(feeScheduleFileOutPutPath, fileName + "*.CSV");
                
                if (csvFiles.Length > 0)
                {
                    //Get the first file only after sorting descending
                    string[] csvSorted = csvFiles.OrderByDescending(x => x).ToArray();
                    log.CreateLogEntry("CSV file exists :" + csvSorted[0]);
                    categoryTable.Rows.Add(new object[] { categoryList.Tables[0].Rows[i]["Category_Name"].ToString(), csvSorted[0].Replace(feeScheduleFileOutPutPath + "\\", "") });

                    fileTable.Rows.Add(new object[] { csvSorted[0] });
                }

                string[] pdfFiles = Directory.GetFiles(feeScheduleFileOutPutPath, fileName + "*.pdf");
                if (pdfFiles.Length > 0)
                {
                    //Get the first file only after sorting descending
                    string[] pdfSorted = pdfFiles.OrderByDescending(x => x).ToArray();
                    log.CreateLogEntry("PDF file exists :" + pdfSorted[0]);
                    string categoryName = categoryList.Tables[0].Rows[i]["Category_Name"].ToString();
                    System.Collections.Generic.List<string> listCategory = new System.Collections.Generic.List<string>();

                    foreach (DataRow dr in categoryTable.Rows)
                    {
                        listCategory.Add(dr["Category_Name"].ToString());
                    }

                    if (!(listCategory.Contains(categoryName)))
                    {
                        categoryTable.Rows.Add(new object[] { categoryList.Tables[0].Rows[i]["Category_Name"].ToString(), pdfSorted[0].Replace(feeScheduleFileOutPutPath + "\\", "") });
                    }
                    fileTable.Rows.Add(new object[] { pdfSorted[0] });
                }

                string[] htmlFiles = Directory.GetFiles(feeScheduleFileOutPutPath, fileName + "*.html");
                if (htmlFiles.Length > 0)
                {
                    //Get the first file only after sorting descending
                    string[] htmlSorted = htmlFiles.OrderByDescending(x => x).ToArray();
                    log.CreateLogEntry("HTM file exists :" + htmlSorted[0]);
                    string categoryName = categoryList.Tables[0].Rows[i]["Category_Name"].ToString();
                    System.Collections.Generic.List<string> listCategory = new System.Collections.Generic.List<string>();

                    foreach (DataRow dr in categoryTable.Rows)
                    {
                        listCategory.Add(dr["Category_Name"].ToString());
                    }

                    if (!(listCategory.Contains(categoryName)))
                    {
                        categoryTable.Rows.Add(new object[] { categoryList.Tables[0].Rows[i]["Category_Name"].ToString(), htmlSorted[0].Replace(feeScheduleFileOutPutPath + "\\", "") });
                    }
                    fileTable.Rows.Add(new object[] { htmlSorted[0] });
                }
            }

            log.CreateLogEntry("Rows Count(from db) :" + categoryList.Tables[0].Rows.Count);

            string feeScheduleFileOutPutPath1 = AppSettings.Get("FeeScheduleJobFolder");
            feeScheduleFileOutPutPath1 = feeScheduleFileOutPutPath1 + "DISPLAY";
            log.CreateLogEntry("Feeschedule file path :" + feeScheduleFileOutPutPath1);
            // logging files count
            int i1 = 0;
            System.IO.DirectoryInfo ProdFilesDirectory = new System.IO.DirectoryInfo(feeScheduleFileOutPutPath1);
            foreach (FileInfo productfile in ProdFilesDirectory.GetFiles())
            { i1 = i1 + 1; }
            log.CreateLogEntry("Files Count :" + i1);
            //
            if (Helper.HasRows(categoryTable))
            {
                for (int i = 0; i < categoryTable.Rows.Count; i++)
                {
                    string catname = categoryTable.Rows[i]["Category_Name"].ToString();

                    var firstSpaceIndex = catname.IndexOf(" ");
                    var firstString = firstSpaceIndex > 0 ? catname.Substring(0, firstSpaceIndex) : catname;
                    var secondString = catname.Replace(firstString, "");

                    string secondStringa = Regex.Replace(secondString, @"(\w)|(\s\w)", m => m.Value.ToLower());
                    string secondStringb = Regex.Replace(secondStringa, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
                    catname = firstString + secondStringb;
                    categoryTable.Rows[i]["Category_Name"] = catname;
                }

                //feeScheduleDataList.DataSource = categoryTable;
                //feeScheduleDataList.DataBind();
            }
        }

        catch (Exception ex)
        { log.CreateLogEntry(ex.Message); }
    }

    public bool ExistingPdfFile(int index)
    {
        try
        {
            string feeScheduleFileOutPutPath = AppSettings.Get("FeeScheduleJobFolder");
            feeScheduleFileOutPutPath = feeScheduleFileOutPutPath + "DISPLAY";
            //string pdfFilename = string.Format("{0}{1}", categoryTable.Rows[index]["File_Name"].ToString(), ".pdf");
            string pdfFilename = categoryTable.Rows[index]["File_Name"].ToString().ToLower().Replace(".csv", ".pdf");
            string fileNamePdf = Path.Combine(feeScheduleFileOutPutPath, pdfFilename);
            System.IO.FileInfo pdffile = new System.IO.FileInfo(fileNamePdf);
            if (pdffile.Exists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        catch (Exception ex)
        {
            log.CreateLogEntry(ex.Message);
            return false;
        }
    }

    public bool ExistingHtmlFile(int index)
    {
        try
        {
            string feeScheduleFileOutPutPath = AppSettings.Get("FeeScheduleJobFolder");
            feeScheduleFileOutPutPath = feeScheduleFileOutPutPath + "DISPLAY";
            //string htmFileName = string.Format("{0}{1}", categoryTable.Rows[index]["File_Name"].ToString(), ".html");
            string htmFileName = categoryTable.Rows[index]["File_Name"].ToString().ToLower().Replace(".csv", ".html"); ;
            string fileNameHtml = Path.Combine(feeScheduleFileOutPutPath, htmFileName);

            System.IO.FileInfo htmfile = new System.IO.FileInfo(fileNameHtml);
            if (htmfile.Exists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        catch (Exception ex)
        {
            log.CreateLogEntry(ex.Message);
            return false;
        }
    }

    public bool ExistingCSVFile(int index)
    {
        try
        {
            string feeScheduleFileOutPutPath = AppSettings.Get("FeeScheduleJobFolder");
            feeScheduleFileOutPutPath = feeScheduleFileOutPutPath + "DISPLAY";
            //string csvFileName = string.Format("{0}{1}", categoryTable.Rows[index]["File_Name"].ToString(), ".CSV");
            string csvFileName = categoryTable.Rows[index]["File_Name"].ToString();
            string fileNameCSV = Path.Combine(feeScheduleFileOutPutPath, csvFileName);

            System.IO.FileInfo csvfile = new System.IO.FileInfo(fileNameCSV);
            if (csvfile.Exists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        catch (Exception ex)
        {
            log.CreateLogEntry(ex.Message);
            return false;
        }
    }

    protected void feeScheduleDataList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            string feeScheduleFolderPath = AppSettings.Get("FeeScheduleJobFolder");
            feeScheduleFolderPath = feeScheduleFolderPath + "DISPLAY";
            Label labelfilename = e.Item.FindControl("lblfileName") as Label;
            //string filename = string.Format("{0}.{1}", labelfilename.Text, e.CommandName);
            string filename = labelfilename.Text;
            filename = Helper.CleanFilePath(filename);
            if (e.CommandName == "PDF")
                filename = filename.ToLower().Replace(".csv", ".pdf");
            else if (e.CommandName == "HTML")
                filename = filename.ToLower().Replace(".csv", ".html");
            string formattedFilePath = Path.Combine(feeScheduleFolderPath, filename);
            if (e.CommandName == "CSV" || e.CommandName == "PDF")
            {
                System.IO.FileInfo file = new System.IO.FileInfo(formattedFilePath);
                if (file.Exists)
                {
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "Attachment; Filename=" + file.Name + "");
                    Response.TransmitFile(formattedFilePath);
                    Response.End();
                }
            }
            else if (e.CommandName == "HTML")
            {
                System.IO.FileInfo file = new System.IO.FileInfo(formattedFilePath);
                if (file.Exists)
                {
                    WebClient client = new WebClient();
                    Byte[] fileData = client.DownloadData(file.ToString());
                    client.Dispose();
                    Response.ContentType = "text/html";
                    Response.AddHeader("Content-Length", "Attachment; Filename=" + file.Name + "");
                    Response.BinaryWrite(fileData);
                    string open = "window.open(" + fileData + ")";
                    ClientScript.RegisterStartupScript(GetType(), "script", open, true);
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        { log.CreateLogEntry(ex.Message); }
    }
}