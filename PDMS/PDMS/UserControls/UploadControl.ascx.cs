using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_UploadControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        uploadLabel.Text = Helper.HtmlEncode(UploadDocumentMessage);
        if (IsRequired)
        {
            CustomValidator.Enabled = true;
            CustomValidator.ErrorMessage = UploadValidationError;
        }
        else 
        {
            CustomValidator.Enabled = false;
        }
    }

    public bool IsRequired { get; set; }

    public string UploadValidationError { get; set; }

    public string UploadDocumentMessage { get; set; }


    protected void CustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (AsyncUpload1.UploadedFiles.Count < 1)
            args.IsValid = false;
        else
            args.IsValid = true;
    }

    protected void AsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
    {
        ////e.IsValid = !CheckUploadedFileValidity();
        //if (e.IsValid)
        //{
        //    byte[] buffer = new byte[e.File.ContentLength];
        //    using (Stream str = e.File.InputStream)
        //    {
        //        str.Read(buffer, 0, e.File.ContentLength);
        //        var attachment = createAttachment(buffer);
        //        // more code
        //    }
        //}

    }
}