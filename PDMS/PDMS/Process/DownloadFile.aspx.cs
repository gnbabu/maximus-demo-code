using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corp.Core.Libraries.Helper;
using MAXIMUS.Core.Libraries;
using System.IO;

public partial class Process_DownloadFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string fileName = Request.QueryString["fileName"];
        if (string.IsNullOrEmpty(fileName))
        {
            Response.StatusCode = 400; // Bad Request
            Response.End();
            return;
        }

        string templateActualPath = AppSettings.Get("PowerAgentTemplateFSXPath");
        string filepath = System.IO.Path.Combine(templateActualPath, fileName);

        if (!System.IO.File.Exists(filepath))
        {
            Response.StatusCode = 404; // Not Found
            Response.End();
            return;
        }

        byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/force-download";
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
        HttpContext.Current.Response.BinaryWrite(fileBytes);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.Close();
        HttpContext.Current.Response.End();
    }
}
