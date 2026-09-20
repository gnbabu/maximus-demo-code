using System;

public partial class PopupControls_MessageBox : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public string ErrorListCssClass
    {
        get { return pnlLabel.CssClass; }
        set { pnlLabel.CssClass = value; }
    }

    public void Show(string message, string title, string postBackURL)
    {
        if (!string.IsNullOrEmpty(postBackURL)) mpeMessageBox.OnOkScript = "location.href='" + postBackURL + "';";
        else mpeMessageBox.OnOkScript = string.Empty;
        lblMessage.Text = message;
        lblTitle.Text = title;
        lblCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss tt");
        mpeMessageBox.Show();
    }

    public void Show(string message, string title)
    {
        Show(message, title, string.Empty);
    }
}
