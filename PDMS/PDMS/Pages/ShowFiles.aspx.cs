using Corp.Core.Libraries.Helper;
using MAXIMUS.Core.Libraries;
using System;
using System.IO;
using System.Web.UI;

public partial class Pages_ShowFiles : System.Web.UI.Page
{
    string filePath;
    string disposition;
    protected UriBuilder viewUrl;

    protected void Page_Load(object sender, EventArgs e)
    {
        string fileId = Request.QueryString["FileName"];
        /*string category = Request.QueryString["catId"];*/
        disposition = Request.QueryString["mode"];
        viewUrl = new UriBuilder(Request.Url.AbsoluteUri);

        /*if (string.IsNullOrWhiteSpace(fileId)) return;
        if (string.IsNullOrWhiteSpace(category)) return;*/

        if (!string.IsNullOrEmpty(disposition) && disposition.ToLower() == "view")
            viewUrl.Query = viewUrl.Query.Replace("mode=view", "mode=inline").Substring(1);
        /*switch (category.ToLower())
        {
            case "learning":*/

        /*Encryption en = new Encryption();
        byte[] inp = Encoding.Unicode.GetBytes(fileId);
        byte[] s = en.DecryptRijndael(inp);
        LookupLearningDoc(System.Text.Encoding.Unicode.GetString(s, 0, s.Length)); //break;*/
        LookupLearningDoc(fileId);
        //}
    }

    protected void LookupLearningDoc(string fileId)
    {
        if (string.IsNullOrWhiteSpace(fileId)) return;

        string LearningDocsPath = AppSettings.Get("LearningDocsFSXPath");

        filePath = Path.Combine(LearningDocsPath, fileId);
        this.Title = fileId;
    }

    protected override void Render(HtmlTextWriter writer)
    {

        if (string.IsNullOrWhiteSpace(filePath)) { Send404(); return; }
        FileInfo file = new FileInfo(filePath);
        if (file.Exists)
        {
            if (string.IsNullOrWhiteSpace(disposition) || disposition.ToLower() != "view")
            {
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", this.ContentDisposition + "; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = MimeHelper.GetMimeTypeForFileName(file.Name);
                Response.TransmitFile(file.FullName);
                //Do not call Response.End() because it throws an exception (by design), let the Render Method exit naturally.

            }
            else base.Render(writer);
        }
        else Send404();
    }

    protected void Send404()
    {
        Response.StatusCode = 404;
    }

    protected string ContentDisposition
    {
        get
        {
            if (!string.IsNullOrEmpty(disposition) && disposition.ToLower() == "inline") return "inline";
            if (!string.IsNullOrEmpty(disposition) && disposition.ToLower() == "view") return "inline";
            return "attachement";
        }
    }
}