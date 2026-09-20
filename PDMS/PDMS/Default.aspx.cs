using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class _Default : System.Web.UI.Page
{
     
    protected void Page_Load(object sender, EventArgs e)
    {
        Guid threadId = Guid.NewGuid();
        string apiusr = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();



        if (!Page.IsPostBack)
        {
            // check for Haven Redirect
            HttpCookie HavenRedirectURL = Request.Cookies["HavenRedirectURL"];
            if (HavenRedirectURL != null)
            {
                if (HavenRedirectURL.Value.Length > 4)
                {  // this is a redirect on an expired or missing token from an embedded PDFlink
                    Response.Redirect("~/Process/PaymentInnovationReports.aspx");
                }
            }

            HttpCookie MITSRedirectURL = Request.Cookies["MITSRedirectURL"];
            if (MITSRedirectURL != null)
            {
                if (MITSRedirectURL.Value.Length > 4)
                {  // this is a redirect on an expired or missing token from an embedded PDFlink
                    Response.Redirect("~/Process/RecipientEligibilityMITS.aspx");
                }
            }

            List<KeyValuePair<Guid, string>> listCurrentRoles = Helper.GetUserRolesByUser(HttpContext.Current.User.Identity.Name.ToString());
            if (listCurrentRoles.Count > 0)
            {
                SessionVarRetriever.ClearRegistrationSessionVars();
                if (Helper.IsLoggedInUserInAdminRole()) Response.Redirect("~/Process/GroupReview.aspx");
                else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderOper)) Response.Redirect("~/Process/ProviderOperatorHome.aspx");
                else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) ||
                         Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent) ||
                         Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.DeemedPresumptive))
                {
                    Response.Redirect("~/Process/ProviderHomeNew.aspx");
                }
                else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InternalApplicationsEntry))
                {
                    Response.Redirect("~/Process/ProviderHomeNew.aspx");
                }
                else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.DCSAdministrator)) Response.Redirect("~/Process/DCSHome.aspx");
                else if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.SiteVisitAdministrator))
                {
                    Response.Redirect("~/Process/SiteVisitAssignments.aspx");
                }
                else if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.SiteVisitOperator))
                {
                    SessionVarRetriever.MyQueueSelectedRoleName = CON.UserRole.SiteVisitOperator;
                    Response.Redirect("~/Process/MySiteVisitQueue.aspx");
                }
                else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.DIDDOperator)) Response.Redirect("~/Process/WaiverProviderSearch.aspx");
                else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingChair) || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CommitteeQualitySpecialist))
                {
                    Response.Redirect("~/Process/CredentialQueue.aspx");
                }
                else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingChair) || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CommitteeQualitySpecialist))
                {
                    Response.Redirect("~/Process/GenericHomePage.aspx");
                }
                else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.RecipientEligibility)
                        || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InfantMortalityLeadEntityAgent)
                        || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ReadOnly) || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderServices)
                         || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderRelations) || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderRelationsManagement)
                          || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSAdmin)
                          || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSBusinessRelations)
                          || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSProviderAssistance)
                          || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSProviderEnrollment)
                          || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSUATSuperuser)
                          || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InternalPaymentInnovation))
                {
                    Response.Redirect("~/Process/GenericHomePage.aspx");
                }
                else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.PnmSuperUser))
                {
                    Response.Redirect("~/Process/TransactionSearch.aspx");
                }
                //If user only have NA report roles then we are navigating them directly to Reports page as they have access only to Reports page 
                else if (Helper.IsUserInNetworkAdequacyReportRole(HttpContext.Current.User.Identity.Name) ||
                    Helper.IsUserInNetworkAdequacyReportRolesByPlanName(HttpContext.Current.User.Identity.Name))
                {
                    Response.Redirect("~/Reports/reportCriteria.aspx");
                }

                else Response.Redirect("~/Process/MyQueue.aspx");
            }
            else
            {
                //if (SessionVarRetriever.IsNewUser)
                Response.Redirect("~/Account/ProviderRoleSelection.aspx", false);
            }
        }
    }
}
