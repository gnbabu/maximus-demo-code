using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_IntuitivePriorAuthMessageBox : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        pnlShowMoreErrorMessage.Visible = false;
    }

    public string ErrorListCssClass
    {
        get { return pnlLabel.CssClass; }
        set { pnlLabel.CssClass = value; }
    }

    public void Show(string message, string title, string postBackURL, string fullDescp)
    {
        if (!string.IsNullOrEmpty(postBackURL)) modalPAIntuitiveMPE.OnOkScript = "location.href='" + postBackURL + "';";
        else modalPAIntuitiveMPE.OnOkScript = string.Empty;
        lblMessage.Text = message;
        lblTitle.Text = title;
        lblShowMoreErrorMessage.Text = fullDescp;
        lblCurrentDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss tt");
        modalPAIntuitiveMPE.Show();
    }

    public void Show(string message, string title, string fullDescp)
    {
        Show(message, title, string.Empty, fullDescp);
    }

    protected void btnShowMoreErrorMessage_Click(object sender, EventArgs e)
    {
        if (pnlShowMoreErrorMessage.Visible == true)
        {
            pnlShowMoreErrorMessage.Visible = false;
        }
        else
        {
            pnlShowMoreErrorMessage.Visible = true;
        }
    }
}