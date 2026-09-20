using MAXIMUS.Core.Libraries;
using System;
using System.IO;

public partial class Pages_PdfViewer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string filename = Request.QueryString["filename"] + ".pdf";
        string appSettingsKey = Request.QueryString["appSettingsKey"];
        if (string.IsNullOrEmpty(appSettingsKey))
            appSettingsKey = "PDMS_SVC_TemplatesPath";
        string templatesDirectory = AppSettings.Get(appSettingsKey, string.Empty);
        string pdfDirectory = templatesDirectory + "Temporary_Files";
        
        if (!Directory.Exists(pdfDirectory)) Directory.CreateDirectory(pdfDirectory);
        
        if (pdfDirectory.LastIndexOf("\\") != pdfDirectory.Length - 1) pdfDirectory += "\\";
        
        string ToSaveFileTo = pdfDirectory + filename;
        FileInfo file = new FileInfo(ToSaveFileTo);
        if (file.Exists)
        {
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "application/pdf";
            Response.TransmitFile(file.FullName);
            Response.End();
        }
    }
}