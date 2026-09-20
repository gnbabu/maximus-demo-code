using System;
using System.Web.UI;

public partial class Process_SubmissionConfirmation : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            Page.Theme = "Modernization";
        }
        else
        {
            Page.Theme = "Default";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

    protected void btnAddProviderType_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Process/NewProvider.aspx?LinkProvider=true");
    }
}