using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_AdminHome : System.Web.UI.Page
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
        //added Page Title for 508 complaint
        Page.Title = Helper.IsUserInAdminRole(HttpContext.Current.User.Identity.Name) ? "Home" : "Dashboard";
    }

    private void LoadTotals()
    {
        // Individual data
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = new DataSet();//psc.GetDashboardStatistics();

        //Provider Summary
        ds = psc.GetDashboardProviderSummary(Helper.GetUserRole(HttpContext.Current.User.Identity.Name));
        SessionVarRetriever.DashBoardProviderSummary.ProviderSummary = ds.Tables[0];



    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        // This forces the browser back button to post to the referrer instead of pulling from cache.
        // This is needed when going back to a page that has UpdatePanels with grid data. 
        // Otherwise, the accordions are expanded but there's no data in them

        Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1.
        Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
        Response.AppendHeader("Expires", "0"); // Proxies.
    }

    private void LoadDashboard()
    {
        //Provider Summary

        ucDashboardProviderSummary.LoadData(SessionVarRetriever.DashBoardProviderSummary.ProviderSummary);



        lblDashboardTitle.InnerText = Helper.IsLoggedInUserInAdminRole() ? "Home" : "Dashboard";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionVarRetriever.ClearRegistrationSessionVars();
        string passedArgument = Request.Params.Get("__EVENTARGUMENT");


        if (!Page.IsPostBack)
        {

            if (SessionVarRetriever.DashBoardProviderSummary.ProviderSummary.Rows.Count == 0)
            {
                LoadTotals();
            }

            if (Helper.IsLoggedInUserInAdminRole())
            {
                SessionVarRetriever.MyQueueSelectedRoleName =
                SessionVarRetriever.MyQueueSelectedRoleValue =
                SessionVarRetriever.UserIdSelected = null;
            }
            LoadDashboard();
        }
        else if (Page.IsPostBack && !string.IsNullOrEmpty(passedArgument))
        {

            switch (passedArgument.ToLower())
            {
                case "provider summary":
                    if (AccProviderSummary.SelectedIndex != 0)
                    {
                        LoadTotals();
                        LoadDashboard();

                        AccProviderSummary.SelectedIndex = 0;
                    }
                    break;
                case "individual": Load_Accordion3(); break;
                case "groups with members": Load_Accordion4(); break;
                case "institutions/facilities": Load_Accordion5(); break;
                case "pharmacy": Load_Accordion6(); break;
                case "organization": Load_AccordionOrganization(); break;
                case "ordering, referring or prescribing": Load_AccordionOrderingReferringPrescribing(); break;
                case "change of operator": Load_AccordionChangeOfOperator(); break;
                case "managed care plan single case": Load_AccordionMCP(); break;
                case "waiver odm": Load_AccordionWaiverODM(); break;
                case "waiver oda": Load_AccordionWaiverODA(); break;
                case "waiver dodd": Load_AccordionWaiverDODD(); break;
                case "non-medicaid dodd": Load_AccordionNonMedicaidDODD(); break;
                case "cpc": Load_AccordionCPC(); break;
            }
        }
    }

    private void Load_Accordion3()
    {
        DataSet ds;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (Accordion3.SelectedIndex != 0)
        {
            //Individual/Sole Proprietors

            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.Individual, CON.WorkflowEventType.NewReg, 0, 0, CON.DashboardTableType.IndividualNewReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.IndividualNewReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.IndividualNewReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.Individual, CON.WorkflowEventType.UpdateReg, 0, 0, CON.DashboardTableType.IndividualUpdateReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.IndividualUpdateReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.IndividualUpdateReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.Individual, CON.WorkflowEventType.RevalReg, 0, 0, CON.DashboardTableType.IndividualRevalidation);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.IndividualRevalidation, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.IndividualRevalidation, ds.Tables[1]);


            ucDashboardIndividualNewReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.IndividualNewReg));
            ucDashboardIndividualUpdateReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.IndividualUpdateReg));
            ucDashboardIndividualRevalidation.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.IndividualRevalidation));
            Accordion3.SelectedIndex = 0;

        }
        else
        {
            Accordion3.SelectedIndex = -1;
        }

    }
    private void Load_Accordion4()
    {
        DataSet ds;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (Accordion4.SelectedIndex != 0)
        {
            //Group with members

            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.Group, CON.WorkflowEventType.NewReg, 0, 0, CON.DashboardTableType.GroupWithMembersNewReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.GroupWithMembersNewReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.GroupWithMembersNewReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.Group, CON.WorkflowEventType.UpdateReg, 0, 0, CON.DashboardTableType.GroupWithMembersUpdateReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.GroupWithMembersUpdateReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.GroupWithMembersUpdateReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.Group, CON.WorkflowEventType.RevalReg, 0, 0, CON.DashboardTableType.GroupWithMembersRevalidation);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.GroupWithMembersRevalidation, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.GroupWithMembersRevalidation, ds.Tables[1]);

            ucDashboardGroupWithMembersNewReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.GroupWithMembersNewReg));
            ucDashboardGroupWithMembersUpdateReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.GroupWithMembersUpdateReg));
            ucDashboardGroupWithMembersRevalidation.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.GroupWithMembersRevalidation));
            Accordion4.SelectedIndex = 0;
        }
        else
        {
            Accordion4.SelectedIndex = -1;
        }
    }
    private void Load_Accordion5()
    {
        DataSet ds;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (Accordion5.SelectedIndex != 0)
        {
            //Institutions/Facilities


            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.EntityFacility, CON.WorkflowEventType.NewReg, 0, 0, CON.DashboardTableType.InstitutionalOrFacilityNewReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.InstitutionalOrFacilityNewReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.InstitutionalOrFacilityNewReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.EntityFacility, CON.WorkflowEventType.UpdateReg, 0, 0, CON.DashboardTableType.InstitutionalOrFacilityUpdateReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.InstitutionalOrFacilityUpdateReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.InstitutionalOrFacilityUpdateReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.EntityFacility, CON.WorkflowEventType.RevalReg, 0, 0, CON.DashboardTableType.InstitutionalOrFacilityRevalidation);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.InstitutionalOrFacilityRevalidation, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.InstitutionalOrFacilityRevalidation, ds.Tables[1]);

            ucDashboardInstitutionsOrFacilitiesNewReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.InstitutionalOrFacilityNewReg));
            ucDashboardInstitutionsOrFacilitiesUpdateReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.InstitutionalOrFacilityUpdateReg));
            ucDashboardInstitutionsOrFacilitiesRevalidation.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.InstitutionalOrFacilityRevalidation));


            Accordion5.SelectedIndex = 0;
        }
        else
        {
            Accordion5.SelectedIndex = -1;
        }
    }
    private void Load_Accordion6()
    {
        DataSet ds;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (Accordion6.SelectedIndex != 0)
        {
            //Pharmacy
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.Pharmacy, CON.WorkflowEventType.NewReg, 0, 0, CON.DashboardTableType.PharmacyNewReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.PharmacyNewReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.PharmacyNewReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.Pharmacy, CON.WorkflowEventType.UpdateReg, 0, 0, CON.DashboardTableType.PharmacyUpdateReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.PharmacyUpdateReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.PharmacyUpdateReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.Pharmacy, CON.WorkflowEventType.RevalReg, 0, 0, CON.DashboardTableType.PharmacyRevalidation);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.PharmacyRevalidation, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.PharmacyRevalidation, ds.Tables[1]);

            UcPharmacyNewReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.PharmacyNewReg));
            UcPharmacyUpdateReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.PharmacyUpdateReg));
            UcPharmacyRevalidation.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.PharmacyRevalidation));
            Accordion6.SelectedIndex = 0;
        }
        else
        {
            Accordion6.SelectedIndex = -1;
        }
    }
    private void Load_AccordionOrganization()
    {
        DataSet ds;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (accOrgainzation.SelectedIndex != 0)
        {
            //Organization
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.Organization, CON.WorkflowEventType.NewReg, 0, 0, CON.DashboardTableType.OrganizationNewReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.OrganizationNewReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.OrganizationNewReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.Organization, CON.WorkflowEventType.UpdateReg, 0, 0, CON.DashboardTableType.OrganizationUpdateReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.OrganizationUpdateReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.OrganizationUpdateReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, CON.ProviderCategoryTypeID.Organization, CON.WorkflowEventType.RevalReg, 0, 0, CON.DashboardTableType.OrganizationRevalidation);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.OrganizationRevalidation, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.OrganizationRevalidation, ds.Tables[1]);

            ucDashboardOrganizationNewReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.OrganizationNewReg));
            ucDashboardOrganizationUpdateReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.OrganizationUpdateReg));
            ucDashboardOrganizationRevalidation.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.OrganizationRevalidation));

            accOrgainzation.SelectedIndex = 0;
        }
        else
        {
            accOrgainzation.SelectedIndex = -1;
        }
    }

    private void Load_AccordionOrderingReferringPrescribing()
    {
        DataSet ds;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (accOrderingReferringPrescribing.SelectedIndex != 0)
        {

            //Ordering Referring Prescribing
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.NewReg, CON.ApplicationType.ORP, 0, CON.DashboardTableType.OrderingReferringPrescribingNewReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.OrderingReferringPrescribingNewReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.OrderingReferringPrescribingNewReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.UpdateReg, CON.ApplicationType.ORP, 0, CON.DashboardTableType.OrderingReferringPrescribingUpdateReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.OrderingReferringPrescribingUpdateReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.OrderingReferringPrescribingUpdateReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.RevalReg, CON.ApplicationType.ORP, 0, CON.DashboardTableType.OrderingReferringPrescribingRevalidation);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.OrderingReferringPrescribingRevalidation, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.OrderingReferringPrescribingRevalidation, ds.Tables[1]);

            ucDashboardOrderingReferringPrescribingNewReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.OrderingReferringPrescribingNewReg));
            ucDashboardOrderingReferringPrescribingUpdateReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.OrderingReferringPrescribingUpdateReg));
            ucDashboardOrderingReferringPrescribingRevalidation.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.OrderingReferringPrescribingRevalidation));

            accOrderingReferringPrescribing.SelectedIndex = 0;
        }
        else
        {
            accOrderingReferringPrescribing.SelectedIndex = -1;
        }
    }

    private void Load_AccordionChangeOfOperator()
    {
        DataSet ds;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (accChangeofOperator.SelectedIndex != 0)
        {
            //Change of Operator

            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.NewReg, CON.ApplicationType.ChangeOfOperator, 0, CON.DashboardTableType.ChangeOfOperatorNewReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.ChangeOfOperatorNewReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.ChangeOfOperatorNewReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.UpdateReg, CON.ApplicationType.ChangeOfOperator, 0, CON.DashboardTableType.ChangeOfOperatorUpdateReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.ChangeOfOperatorUpdateReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.ChangeOfOperatorUpdateReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.RevalReg, CON.ApplicationType.ChangeOfOperator, 0, CON.DashboardTableType.ChangeOfOperatorRevalidation);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.ChangeOfOperatorRevalidation, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.ChangeOfOperatorRevalidation, ds.Tables[1]);

            ucDashboardChangeOfOperatorNewReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.ChangeOfOperatorNewReg));
            ucDashboardChangeOfOperatorUpdateReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.ChangeOfOperatorUpdateReg));
            ucDashboardChangeOfOperatorRevalidation.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.ChangeOfOperatorRevalidation));

            accChangeofOperator.SelectedIndex = 0;
        }
        else
        {
            accChangeofOperator.SelectedIndex = -1;
        }
    }

    private void Load_AccordionMCP()
    {
        DataSet ds;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (accManagedCare.SelectedIndex != 0)
        {
            //Managed Care Plan
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.NewReg, CON.ApplicationType.MCP, 0, CON.DashboardTableType.MCPOnlyNewReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.MCPOnlyNewReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.MCPOnlyNewReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.UpdateReg, CON.ApplicationType.MCP, 0, CON.DashboardTableType.MCPOnlyUpdateReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.MCPOnlyUpdateReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.MCPOnlyUpdateReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.RevalReg, CON.ApplicationType.MCP, 0, CON.DashboardTableType.MCPOnlyRevalidation);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.MCPOnlyRevalidation, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.MCPOnlyRevalidation, ds.Tables[1]);

            ucDashboardManagedCareNewReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.MCPOnlyNewReg));
            ucDashboardManagedCareUpdateReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.MCPOnlyUpdateReg));
            ucDashboardManagedCareRevalidation.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.MCPOnlyRevalidation));

            accManagedCare.SelectedIndex = 0;
        }
        else
        {
            accManagedCare.SelectedIndex = -1;
        }
    }

    private void Load_AccordionWaiverODM()
    {
        DataSet ds;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (accWaiverODM.SelectedIndex != 0)
        {
            //Waiver ODM

            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.NewReg, CON.ApplicationType.Waiver, 0, CON.DashboardTableType.WaiverODMNewReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.WaiverODMNewReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.WaiverODMNewReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.UpdateReg, CON.ApplicationType.Waiver, 0, CON.DashboardTableType.WaiverODMUpdateReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.WaiverODMUpdateReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.WaiverODMUpdateReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.RevalReg, CON.ApplicationType.Waiver, 0, CON.DashboardTableType.WaiverODMRevalidation);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.WaiverODMRevalidation, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.WaiverODMRevalidation, ds.Tables[1]);

            ucDashboardWaiverODMNewReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.WaiverODMNewReg));
            ucDashboardWaiverODMUpdateReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.WaiverODMUpdateReg));
            ucDashboardWaiverODMRevalidation.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.WaiverODMRevalidation));

            accWaiverODM.SelectedIndex = 0;
        }
        else
        {
            accWaiverODM.SelectedIndex = -1;
        }
    }

    private void Load_AccordionWaiverODA()
    {
        DataSet ds;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (accWaiverODA.SelectedIndex != 0)
        {
            //Waiver ODA
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.NewReg, CON.ApplicationType.Waiver, 0, CON.DashboardTableType.WaiverODANewReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.WaiverODANewReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.WaiverODANewReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.UpdateReg, CON.ApplicationType.Waiver, 0, CON.DashboardTableType.WaiverODAUpdateReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.WaiverODAUpdateReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.WaiverODAUpdateReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.RevalReg, CON.ApplicationType.Waiver, 0, CON.DashboardTableType.WaiverODARevalidation);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.WaiverODARevalidation, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.WaiverODARevalidation, ds.Tables[1]);

            ucDashboardWaiverODANewReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.WaiverODANewReg));
            ucDashboardWaiverODAUpdateReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.WaiverODAUpdateReg));
            ucDashboardWaiverODARevalidation.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.WaiverODARevalidation));

            accWaiverODA.SelectedIndex = 0;
        }
        else
        {
            accWaiverODA.SelectedIndex = -1;
        }
    }

    private void Load_AccordionWaiverDODD()
    {
        DataSet ds;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (accWaiverDODD.SelectedIndex != 0)
        {
            //Waiver DODD
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.NewReg, CON.ApplicationType.Waiver, 0, CON.DashboardTableType.WaiverDODDNewReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.WaiverDODDNewReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.WaiverDODDNewReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.UpdateReg, CON.ApplicationType.Waiver, 0, CON.DashboardTableType.WaiverDODDUpdateReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.WaiverDODDUpdateReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.WaiverDODDUpdateReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.RevalReg, CON.ApplicationType.Waiver, 0, CON.DashboardTableType.WaiverDODDRevalidation);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.WaiverDODDRevalidation, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.WaiverDODDRevalidation, ds.Tables[1]);

            ucDashboardWaiverDODDNewReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.WaiverDODDNewReg));
            ucDashboardWaiverDODDUpdateReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.WaiverDODDUpdateReg));
            ucDashboardWaiverDODDRevalidation.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.WaiverDODDRevalidation));

            accWaiverDODD.SelectedIndex = 0;
        }
        else
        {
            accWaiverDODD.SelectedIndex = -1;
        }
    }

    private void Load_AccordionNonMedicaidDODD()
    {
        DataSet ds;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (accNonMedicaidDODD.SelectedIndex != 0)
        {
            //Waiver Non-medicaid DODD
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.NewReg, CON.ApplicationType.Waiver, 0, CON.DashboardTableType.NonMedicaidDODDNewReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.NonMedicaidDODDNewReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.NonMedicaidDODDNewReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.UpdateReg, CON.ApplicationType.Waiver, 0, CON.DashboardTableType.NonMedicaidDODDUpdateReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.NonMedicaidDODDUpdateReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.NonMedicaidDODDUpdateReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.RevalReg, CON.ApplicationType.Waiver, 0, CON.DashboardTableType.NonMedicaidDODDRevalidation);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.NonMedicaidDODDRevalidation, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.NonMedicaidDODDRevalidation, ds.Tables[1]);

            ucDashboardNonMedicaidDODDNewReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.NonMedicaidDODDNewReg));
            ucDashboardNonMedicaidDODDUpdateReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.NonMedicaidDODDUpdateReg));
            ucDashboardNonMedicaidDODDRevalidation.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.NonMedicaidDODDRevalidation));

            accNonMedicaidDODD.SelectedIndex = 0;
        }
        else
        {
            accNonMedicaidDODD.SelectedIndex = -1;
        }
    }

    private void Load_AccordionCPC()
    {
        DataSet ds;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (accCPP.SelectedIndex != 0)
        {
            //CPC
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.NewReg, CON.ApplicationType.CPC, 0, CON.DashboardTableType.CPCNewReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.CPCNewReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.CPCNewReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.UpdateReg, CON.ApplicationType.CPC, 0, CON.DashboardTableType.CPCUpdateReg);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.CPCUpdateReg, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.CPCUpdateReg, ds.Tables[1]);
            ds = psc.GetDashboardStatisticsGroups(CON.WorkflowType.RegistrationNew, 0, CON.WorkflowEventType.RevalReg, CON.ApplicationType.CPC, 0, CON.DashboardTableType.CPCRevalidation);
            SessionVarRetriever.SetDashBoardTotals(CON.DashboardTableType.CPCRevalidation, ds.Tables[0]);
            if (ds.Tables.Count == 2) SessionVarRetriever.SetDashBoardTotalIdList(CON.DashboardTableType.CPCRevalidation, ds.Tables[1]);

            ucDashboardCPCNewReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.CPCNewReg));
            ucDashboardCPCUpdateReg.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.CPCUpdateReg));
            ucDashboardCPCRevalidation.LoadData(SessionVarRetriever.GetDashBoardTotals(CON.DashboardTableType.CPCRevalidation));

            accCPP.SelectedIndex = 0;
        }
        else
        {
            accCPP.SelectedIndex = -1;
        }
    }




}