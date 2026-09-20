using System;
using System.Web;

public partial class PopupControls_ORProviderSearch : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        hdnUserName.Value = HttpContext.Current.User.Identity.Name;
    }
}