using MAXIMUS.Core.Libraries;
using System;
using System.Web.UI;

public partial class UserControls_ChatBox : System.Web.UI.UserControl
{


    protected void Page_Load(object sender, EventArgs e)
    {
        string showChat = AppSettings.Get("PDMS-MEDChat");
        if (showChat == "true")
        {
            this.Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "chat", "<script type=\"text/javascript\" async src=\"https://medchatapp.com/widget/widget.js?api-key=lv-R89GjakeXREC6XXrdVw\"></script>");
        }
    }

}
 