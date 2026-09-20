using System;
using System.Web.UI;

public partial class Process_OwnerSearch : RegistrationProvider
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
        //Set the title same as setTitle function to avoid 508 complaint
        Page.Title = Resources.BrandingResource.OWNER_SEARCH_TITLE;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private bool SearchIds()
    {
        if (!string.IsNullOrEmpty(SessionVarRetriever.DashBoardRegistrationIds)) return true;
        if (SessionVarRetriever.DashBoardTableId > 0) return true;
        return false;
    }

    protected void btnReturnToGroupAffiliations_Click(object sender, EventArgs e)
    {
        // TODO: EDV What functionality is this??
        //Response.Redirect("Registration.aspx?Step=4");
    }

    private void SetTitle()
    {
        lblTitle.Text = Resources.BrandingResource.OWNER_SEARCH_TITLE;
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        if (SearchIds())
        {
            SessionVarRetriever.ReturnToDashboard = false;
            Response.Redirect("~/Default.aspx");
            return;
        }

        pnlReturn.Visible = SearchIds();
        SetTitle();
    }


}