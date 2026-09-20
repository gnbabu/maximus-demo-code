using Common.DataModels;
using System;
using System.Web.UI;

public partial class Process_Resources : System.Web.UI.Page
{
    protected LearningCategoryDocs[] CategoryDocs;

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
        if (!IsPostBack)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

            //CategoryDocs = psc.GetLearningDocs();
            
        }
        // SessionVarRetriever.RegistrationId = 0;
        this.Page.Title = (string)GetGlobalResourceObject("BrandingResource", "PortalName") + " - Resources";
        //RenderPage.Render("Resources", phAutoGenerate);
        RenderPage.Rendercontent("Learning", phAutoGenerate);
    }
}