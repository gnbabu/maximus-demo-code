using MAXIMUS.Core.Libraries;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Maintenance_ReferenceDataMgmt : System.Web.UI.Page
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

        if (!Page.IsPostBack)
        {
            if (Methods.IsUserInPNMSuperUser(HttpContext.Current.User.Identity.Name))
            {
                ddlReferenceDataSelect.Items.AddRange(new ListItem[]{
                                                new ListItem("RDM Code Set Review", "vwRDMCodeSets"),
                                                new ListItem("External Code Set Review", "vwWPCCodeSets"),
                                                new ListItem("Context4 Code Set Review", "vwC4CodeSets"),
                                                new ListItem("CMS Code Set Review", "vwCMSCodeSets"),
                                                new ListItem("NUBC Code Set Review", "vwNUBCCodeSets"),
                                                new ListItem("PA Procedure Group", "vwPAProcedureGrp")
                                                });
            }
        }
        SetActiveView(ddlReferenceDataSelect.SelectedValue);
    }

    private void SetActiveView(string viewName)
    {
        switch (viewName)
        {
            case "vwRDMCodeSets":
                mltEnrollment.SetActiveView(vwRDMCodeSets);
                break;
            case "vwWPCCodeSets":
                mltEnrollment.SetActiveView(vwWPCCodeSets);
                break;
            case "vwC4CodeSets":
                mltEnrollment.SetActiveView(vwC4CodeSets);
                break;
            case "vwCMSCodeSets":
                mltEnrollment.SetActiveView(vwCMSCodeSets);
                break;
            case "vwNUBCCodeSets":
                mltEnrollment.SetActiveView(vwNUBCCodeSets);
                break;
            case "vwPAProcedureGrp":
                mltEnrollment.SetActiveView(vwPAProcedureGrp);
                break;
        }
    }

    protected void ddlReferenceDataSelect_SelectedIndexChanged(object sender, EventArgs e)
    {

        SetActiveView(ddlReferenceDataSelect.SelectedValue);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(@"~/Maintenance/ReferenceData.aspx");
    }
}