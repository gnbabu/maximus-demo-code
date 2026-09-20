using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Con = MAXIMUS.Core.Libraries.Constants;
public partial class Reports_ReportCriteria : System.Web.UI.Page
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
        //don't show page unless user is logged in
        if (!User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/default.aspx");
        }
        if (!IsPostBack)
        {
            if (Helper.IsLoggedInUserInAdminRole())
            {
                SessionVarRetriever.MyQueueSelectedRoleName =
                SessionVarRetriever.MyQueueSelectedRoleValue =
                SessionVarRetriever.UserIdSelected = null;
            }
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet dr = new DataSet();

            dr = psc.SelectReports();
            // add the To email address(es)

            DataSet roles = new DataSet();

            string ServiceProviderAgreements = "(ReportSession = '" + Con.ReportSession.ServiceProviderAgreements + "')";
            DataTable ServiceProviderAgreementsTable = new DataTable();
            if (dr.Tables[0].Select(ServiceProviderAgreements).Length > 0)
            {
                ServiceProviderAgreementsTable = dr.Tables[0].Select(ServiceProviderAgreements).CopyToDataTable();
            }
            string MonthlyDatabaseChecks = "(ReportSession = '" + Con.ReportSession.MonthlyDatabaseChecks + "')";
            DataTable MonthlyDatabaseChecksTable = new DataTable();
            if (dr.Tables[0].Select(MonthlyDatabaseChecks).Length > 0)
            {
                MonthlyDatabaseChecksTable = dr.Tables[0].Select(MonthlyDatabaseChecks).CopyToDataTable();
            }
            string SiteVisits = "(ReportSession = '" + Con.ReportSession.SiteVisits + "')";
            DataTable SiteVisitsTable = new DataTable();
            if (dr.Tables[0].Select(SiteVisits).Length > 0)
            {
                SiteVisitsTable = dr.Tables[0].Select(SiteVisits).CopyToDataTable();
            }
            string ApplicationFee = "(ReportSession = '" + Con.ReportSession.ApplicationFee + "')";
            DataTable ApplicationFeeTable = new DataTable();
            if (dr.Tables[0].Select(ApplicationFee).Length > 0)
            {
                ApplicationFeeTable = dr.Tables[0].Select(ApplicationFee).CopyToDataTable();
            }
            string StatusOfProviderRegistrations = "(ReportSession = '" + Con.ReportSession.StatusOfProviderRegistrations + "')";
            DataTable StatusOfProviderRegistrationsTable = new DataTable();
            if (dr.Tables[0].Select(StatusOfProviderRegistrations).Length > 0)
            {
                StatusOfProviderRegistrationsTable = dr.Tables[0].Select(StatusOfProviderRegistrations).CopyToDataTable();
            }

            string FacilitiesCHOPsandClosures = "(ReportSession = '" + Con.ReportSession.FacilitiesCHOPsandClosures + "')";
            DataTable FacilitiesCHOPsandClosuresTable = new DataTable();
            if (dr.Tables[0].Select(FacilitiesCHOPsandClosures).Length > 0)
            {
                FacilitiesCHOPsandClosuresTable = dr.Tables[0].Select(FacilitiesCHOPsandClosures).CopyToDataTable();
            }

            string ProviderListingByAffiliation = "(ReportSession = '" + Con.ReportSession.ProviderListingByAffiliation + "')";
            DataTable dtProviderTable = new DataTable();
            if (dr.Tables[0].Select(ProviderListingByAffiliation).Length > 0)
            {
                dtProviderTable = dr.Tables[0].Select(ProviderListingByAffiliation).CopyToDataTable();
            }

            string CredentialingReports = "(ReportSession = '" + Con.ReportSession.CredentialingReports + "')";
            DataTable dtCredentialingReportsTable = new DataTable();
            if (dr.Tables[0].Select(CredentialingReports).Length > 0)
            {
                dtCredentialingReportsTable = dr.Tables[0].Select(CredentialingReports).CopyToDataTable();
            }

            string networkAdequacyReports = "(ReportSession = '" + Con.ReportSession.NetworkAdequacyreports + "')";
            DataTable dtNetwordAdequacyReportsTable = new DataTable();           

            string toggleNA = AppSettings.Get("ToggleNetworkAdequacyReports", "true");

            if (dr.Tables[0].Select(networkAdequacyReports).Length > 0)
            {
                dtNetwordAdequacyReportsTable = dr.Tables[0].Select(networkAdequacyReports).CopyToDataTable();
            }

            if (toggleNA.Equals("false"))
            {
                dtNetwordAdequacyReportsTable = null;
            }

            string odmStaffReports = "(ReportSession = '" + Con.ReportSession.ODMStaffReports + "')";
            DataTable dtodmStaffReportsTable = new DataTable();
            if (dr.Tables[0].Select(odmStaffReports).Length > 0)
            {
                dtodmStaffReportsTable = dr.Tables[0].Select(odmStaffReports).CopyToDataTable();
            }

            if (Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name))//OHPNM-5941
            {
                //OHPNM-8998
                if (!(Helper.IsUserCredentialSpecialist(HttpContext.Current.User.Identity.Name) ||
                    Helper.IsUserCredentialingSupervisor(HttpContext.Current.User.Identity.Name) ||
                     Helper.IsLoggedInUserInODMCredentialingQualityAssuranceRole() ||
                     Helper.IsUserODMCredentialingSpecialistLTC(HttpContext.Current.User.Identity.Name) ||
                      Helper.IsUserODMCredentialingSupervisor(HttpContext.Current.User.Identity.Name)))
                dtCredentialingReportsTable = dtCredentialingReportsTable.AsEnumerable().Where(r => r.Field<string>("ReportName") != Con.ReportSession.CredentialingCleanFileReport).
                                                Where(r => r.Field<string>("ReportName") != Con.ReportSession.CredentialingFlaggedFilesReport).CopyToDataTable();
            }

            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, Con.UserRoleType.ODMStateAdministrator) ||
                Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, Con.UserRoleType.APMSpecialist))
            {
                string CPCReports = "(ReportSession = '" + Con.ReportSession.CPCReports + "')";
                DataTable dtCPCReportsTable = new DataTable();
                if (dr.Tables[0].Select(CPCReports).Length > 0)
                {
                    dtCPCReportsTable = dr.Tables[0].Select(CPCReports).CopyToDataTable();

                    dtCPCReportsTable.Columns.Add("SideTextContent", typeof(String));

                    DataRow[] row = dtCPCReportsTable.Select("ReportName= 'CPCEnrollment'");
                    //since only one record with this agent_id, we take first record of array -> row[0]
                    row[0]["SideTextContent"] = "Program Year : " + DateTime.Today.AddYears(1).ToString("yyyy");
                }

                grReportCPC.DataSource = dtCPCReportsTable;
                grReportCPC.DataBind();
            }
            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, Con.UserRoleType.ODMStateAdministrator) ||
                Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, Con.UserRoleType.APMSpecialist))
            {
                string CMCReports = "(ReportSession = '" + Con.ReportSession.CMCReports + "')";
                DataTable dtCMCReportsTable = new DataTable();
                if (dr.Tables[0].Select(CMCReports).Length > 0)
                {
                    dtCMCReportsTable = dr.Tables[0].Select(CMCReports).CopyToDataTable();
                }

                grReportCMC.DataSource = dtCMCReportsTable;
                grReportCMC.DataBind();
            }

           if(Helper.OtherReportsVisibiltyToNetworkAdequacyReportRoles())
            {
                grReportNameMonthly.DataSource = ServiceProviderAgreementsTable;
                grReportNameMonthly.DataBind();
                grReportNameQuartely.DataSource = MonthlyDatabaseChecksTable;
                grReportNameQuartely.DataBind();
                grReportNameYearly.DataSource = SiteVisitsTable;
                grReportNameYearly.DataBind();
                grApplicationFee.DataSource = ApplicationFeeTable;
                grApplicationFee.DataBind();
                grStatusOfProviderRegistrations.DataSource = StatusOfProviderRegistrationsTable;
                grStatusOfProviderRegistrations.DataBind();
                grFacilitiesCHOPsandClosures.DataSource = FacilitiesCHOPsandClosuresTable;
                grFacilitiesCHOPsandClosures.DataBind();

                gvProviderListing.DataSource = dtProviderTable;
                gvProviderListing.DataBind();
                gvCredentialingReports.DataSource = dtCredentialingReportsTable;
                gvCredentialingReports.DataBind();

            }

           if(Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, Con.UserRoleType.ODMStateAdministrator))
           {
                gvODMStaffReports.DataSource = dtodmStaffReportsTable;
                gvODMStaffReports.DataBind();
           }

           

            if (false || (Helper.IsUserInNetworkAdequacyReportRole(HttpContext.Current.User.Identity.Name) ||
                Helper.IsUserInTechAdminRole(HttpContext.Current.User.Identity.Name) ||
                Helper.IsUserInAdminRole(HttpContext.Current.User.Identity.Name)) )
            {
                gvNetworkAdequacyReports.DataSource = dtNetwordAdequacyReportsTable;
                gvNetworkAdequacyReports.DataBind();
            }
            else
            {
                if (Helper.IsUserInNetworkAdequacyReportRolesByPlanName(HttpContext.Current.User.Identity.Name))
                {
                    DataTable dtNetwordAdequacyReportsTableByRole = dtNetwordAdequacyReportsTable.Copy();
                    dtNetwordAdequacyReportsTableByRole.Rows.Clear();
                    foreach (DataRow dataRow  in dtNetwordAdequacyReportsTable.Rows)
                    {
                       string rolesByReport= ObjectControllerHelper.GetString("Roles", dataRow);
                        if(!string.IsNullOrWhiteSpace(rolesByReport))
                        {
                            var reportRoles = rolesByReport.Split(',');
                            foreach(var item in reportRoles)
                            {
                                if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, item ))
                                {
                                    dtNetwordAdequacyReportsTableByRole.ImportRow(dataRow);
                                    break;
                                }
                            }
                        }
                       
                    }
                    if(dtNetwordAdequacyReportsTableByRole != null && dtNetwordAdequacyReportsTableByRole.Rows.Count >0)
                    {
                        gvNetworkAdequacyReports.DataSource = dtNetwordAdequacyReportsTableByRole;
                        gvNetworkAdequacyReports.DataBind();
                    }
                    
                }
            }

            if (AppSettings.Get("EnableSAM684") == "true" &&
                Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, Con.UserRoleType.ODMStateAdministrator) ||
                Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, Con.UserRoleType.ComplianceSpecialist))
            {
                string complianceMonitoringReports = "(ReportSession = '" + Con.ReportSession.ComplianceMonitoringReports + "')";
                DataTable dtComplianceMonitoring = new DataTable();
                if (dr.Tables[0].Select(complianceMonitoringReports).Length > 0)
                {
                    dtComplianceMonitoring = dr.Tables[0].Select(complianceMonitoringReports).CopyToDataTable();
                    gvComplianceMonitoring.DataSource = dtComplianceMonitoring;
                    gvComplianceMonitoring.DataBind();
                }
            }
        }
    }


    protected void grReportName_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ReportName")
        {

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            String en = psc.GetEnvironment();
            String strother = String.Empty; //this for the other parameters
            string UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
            Response.Redirect("~/Reports/reportviewer.aspx?ReportName=" + e.CommandArgument.ToString()
                .Replace(",", "%2c").Replace("&", "%26") + "&environ=" + en + "&ReportParameter1=" + UserID + strother);
        }
    }


}