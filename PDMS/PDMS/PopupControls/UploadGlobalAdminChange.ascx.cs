using System;
using System.IO;

public partial class PopupControls_UploadGlobalAdminChange : System.Web.UI.UserControl
{
    public event EventHandler Saved;

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!fuDoc.HasFile)
        {
            // RequiredFieldValidator also prevents this when validation group used
            return;
        }

        // TODO: validate type/size; store to DB or file share
        var saveDir = Server.MapPath("~/App_Data/Uploads/");
        Directory.CreateDirectory(saveDir);
        var fn = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_") + Path.GetFileName(fuDoc.FileName);
        var full = Path.Combine(saveDir, fn);
        fuDoc.SaveAs(full);

        // notify page
        Saved.Invoke(this, EventArgs.Empty);
    }

    // Expose Cancel button ClientID if you want to wire it from page
    public string CancelButtonClientID
    {
        get
        {
            return btnCancel.ClientID;
        }
    }
}
