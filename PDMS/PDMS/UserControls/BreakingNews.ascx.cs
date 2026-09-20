using MAXIMUS.Core.Libraries;
using System;

public partial class UIControls_BreakingNews : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string newsContents = AppSettings.Get("BreakingNews");
            string ProviderTransmittals = AppSettings.Get("ProviderTransmittalNews");
            //newsContents = "<ul><li>Item 1</li><li>Item 2</li></ul>";
            divNewsItems.InnerHtml = newsContents;
        }
    }
}
