using MAXIMUS.Core.Libraries;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Maintenance_ReferenceData : System.Web.UI.Page
{
    private string ActiveReferenceDataView
    {
        get
        {
            return ViewState["ActiveReferenceDataView"] as string;
        }
        set { ViewState["ActiveReferenceDataView"] = value; }
    }

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
            if (Methods.IsUserInTechAdmin(HttpContext.Current.User.Identity.Name))
            {
                ddlReferenceDataSelect.Items.AddRange(new ListItem[]{
                                                new ListItem("Delegates", "vwDelegates")
                                                });
            }
            if (Methods.IsUserInPNMSuperUser(HttpContext.Current.User.Identity.Name))
            {
                ddlReferenceDataSelect.Items.AddRange(new ListItem[]{
                                                new ListItem("AppSettings", "vwAppSettings"),
                                                new ListItem("Automated Reports", "vwAutomatedReports"),
                                                new ListItem("Data-Fix Tables", "vwDataFixTables"),
                                                new ListItem("WebAPI Testing", "vwWebAPITesting"),
                                                new ListItem("Reference Data Management (RDM)", "vwRDMCodeSets")
                                                //new ListItem("RDM Code Set Review", "vwRDMCodeSets"),
                                                //new ListItem("External Code Set Review", "vwWPCCodeSets"),
                                                //new ListItem("Context4 Code Set Review", "vwC4CodeSets"),
                                                //new ListItem("CMS Code Set Review", "vwCMSCodeSets"),
                                                //new ListItem("NUBC Code Set Review", "vwNUBCCodeSets"),
                                                //new ListItem("PA Procedure Group", "vwPAProcedureGrp")
                                                });
            }
            else
            {
                ddlReferenceDataSelect.Items.AddRange(new ListItem[]{
                                                new ListItem("Application Type", "vwApplicationType"),
                                                new ListItem("Paper Request Document Type", "vwPaperRequestDocumentType"),
                                                new ListItem("Provider Category Type", "vwProviderCategoryType"),
                                                new ListItem("Provider Type", "vwProviderType"),
                                                new ListItem("Provider Type Fee", "vwProviderTypeFee"),
                                                new ListItem("Reg Page Setting", "vwRegPageSetting"),
                                                new ListItem("Reg Page Setting Action", "vwRegPageSettingAction"),
                                                new ListItem("Reg Page Type","vwRegPageType"),
                                                new ListItem("Reg Section Upload Control", "vwRegSectionUploadControl"),
                                                new ListItem("Specialty Type", "vwSpecialtyType"),
                                                new ListItem("Taxonomy Type", "vwTaxonomyType")
                                                });
            }
            
        }
        SetActiveView(ddlReferenceDataSelect.SelectedValue);
    }

    private void SetActiveView(string viewName)
    {
        switch (viewName)
        {
            case "vwApplicationType":
                mltEnrollment.SetActiveView(vwApplicationType);
                break;
            case "vwAutomatedReports":
                mltEnrollment.SetActiveView(vwAutomatedReports);
                break;
            case "vwPaperRequestDocumentType":
                mltEnrollment.SetActiveView(vwPaperRequestDocumentType);
                break;
            case "vwProviderCategoryType":
                mltEnrollment.SetActiveView(vwProviderCategoryType);
                break;
            case "vwProviderType":
                mltEnrollment.SetActiveView(vwProviderType);
                break;
            //case "vwPAProcedureGrp":
            //    mltEnrollment.SetActiveView(vwPAProcedureGrp);
            //    break;
            case "vwProviderTypeFee":
                mltEnrollment.SetActiveView(vwProviderTypeFee);
                break;
            case "vwRegPageSetting":
                mltEnrollment.SetActiveView(vwRegPageSetting);
                break;
            case "vwRegPageSettingAction":
                mltEnrollment.SetActiveView(vwRegPageSettingAction);
                break;
            case "vwRegPageType":
                mltEnrollment.SetActiveView(vwRegPageType);
                break;
            case "vwRegSectionUploadControl":
                mltEnrollment.SetActiveView(vwRegSectionUploadControl);
                break;
            case "vwSpecialtyType":
                mltEnrollment.SetActiveView(vwSpecialtyType);
                break;
            case "vwTaxonomyType":
                mltEnrollment.SetActiveView(vwTaxonomyType);
                break;
            case "vwDelegates":
                mltEnrollment.SetActiveView(vwDelegates);
                break;
            case "vwAppSettings":
                mltEnrollment.SetActiveView(vwAppSettings);
                break;
            case "vwDataFixTables":
                mltEnrollment.SetActiveView(vwDataFixTables);
                break;
            case "vwWebAPITesting":
                mltEnrollment.SetActiveView(vwWebAPITesting);
                break;
            case "vwRDMCodeSets":
                Response.Redirect(@"~/Maintenance/ReferenceDataMgmt.aspx");
                //mltEnrollment.SetActiveView(vwRDMCodeSets);
                break;
            //case "vwWPCCodeSets":
            //    mltEnrollment.SetActiveView(vwWPCCodeSets);
            //    break;
            //case "vwC4CodeSets":
            //    mltEnrollment.SetActiveView(vwC4CodeSets);
            //    break;
            //case "vwCMSCodeSets":
            //    mltEnrollment.SetActiveView(vwCMSCodeSets);
            //    break;
            //case "vwNUBCCodeSets":
            //    mltEnrollment.SetActiveView(vwNUBCCodeSets);
            //    break;
            default:
                mltEnrollment.SetActiveView(vwDelegates);
                break;
        }
        
    }

    protected void ddlReferenceDataSelect_SelectedIndexChanged(object sender, EventArgs e)
    {

        SetActiveView(ddlReferenceDataSelect.SelectedValue);
    }
}