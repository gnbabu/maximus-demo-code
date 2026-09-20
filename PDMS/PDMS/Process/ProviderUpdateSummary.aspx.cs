using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_ProviderUpdateSummary : WorkflowPage
{

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
            Page.Theme = "Modernization";
        else
            Page.Theme = "Default";

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            RegistrationProvider registrationProvider = null;
            if (Session["RegistrationProvider"] != null)
            {
                registrationProvider = (RegistrationProvider)Session["RegistrationProvider"];
            }
            if (registrationProvider != null)
            {
                this.RegistrationId = registrationProvider.RegistrationId;
                this.RegistrationStep = registrationProvider.RegistrationStep;
                this.RegistrationIdSelected = registrationProvider.IsReadOnly ? registrationProvider.RegistrationId : 0;
                this.FillRegistrationData();
                Session.Remove("RegistrationProvider");

            }
            else if (PreviousPage != null)
            {
                this.RegistrationId = PreviousPage.RegistrationId;
                this.RegistrationStep = PreviousPage.RegistrationStep;
                this.RegistrationIdSelected = PreviousPage.IsReadOnly ? PreviousPage.RegistrationId : 0;
                this.FillRegistrationData();
            }
            else if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString.AllKeys.Contains("RegId"))
                    this.RegistrationId = int.Parse(Request["RegId"]);

            }

            // OHPNM-8164/OHPNM-8305 - if this user is a provider administrator or provider agent, let's see if they should be looking at this registration
            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAdministrator) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent))
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

                // see if they can access this registration
                bool UserCanAccessReg = psc.UserCanAccessReg(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), this.RegistrationId);
                if (!UserCanAccessReg)
                {
                    // they shouldn't be here; kick em out
                    Response.Redirect("~/Process/ProviderHomeNew.aspx");
                }
            }
        }
    }

    private void SetWorkflowPanel()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds1 = psc.WF_SelectStepInfo(this.WF_StepID);
        string stepOwnerID = "";
        string loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        if (Helper.HasRows(ds1))
        {
            stepOwnerID = Helper.GetString("STEP_OWNER_ID", ds1.Tables[0].Rows[0]);
        }

        if (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && this.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry &&
    Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) && loggedInUserId.Equals(stepOwnerID))
        {
            Master.SetActionVisibility("Submit for Review", false);
        }
        else if (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && this.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry && loggedInUserId.Equals(stepOwnerID))
        {
            Master.SetActionVisibility("Submit for Review", false);
        }
        else
        {
            Master.SetActionVisibility("Submit Update", false);
        }
        Master.SetActionVisibility("Submit Waiver Update", false);
        Master.SetBacktoSummaryPanelVisibility(false);

        // OHPNM-2723 - BEG - see if registration is ReturnToProviderForSiteVisit; if it's not, turn off Plan of Correction button
        DataSet registrationData = psc.SelectRegistration(this.RegistrationId);
        int registrationStatus = 0;
        if (Helper.HasRows(registrationData))
        {
            if (int.TryParse(Helper.GetData("REGISTRATION_STATUS_TYPE_ID", registrationData.Tables[0].Rows[0]), out registrationStatus))
            {
                if (registrationStatus != CON.RegistrationStatusTypeId.ReturnToProviderForSiteVisit)
                {
                    Master.SetActionVisibility("Plan of Correction", false);
                }

            }
        }
        if (!Helper.IsUpdateCPCContact)
        {
            Master.SetActionVisibility("Submit Contact Info Update", false);
        }
        // OHPNM-2723 - END
    }

    override protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            this.FillRegistrationData();

        Master.LoadProviderInfoHeader();
        var sectionData = LoadSectionData();
        var strAppealsExpr = "REG_SECTION_NAME NOT IN ('Appeals','NPIandMedId','Contract Maintenance')";
        
        SetWorkflowPanel();
        if (sectionData.Tables[0].Select("IS_COMMON_UPDATE = 1 AND IS_VISIBLE = 1").Length > 0)
        {
            var dataTable = sectionData.Tables[0].Select("IS_COMMON_UPDATE = 1 AND IS_VISIBLE = 1").CopyToDataTable();
			
            // OHPNM-7214
            if (this.IsUpdateCPCContactOnly)
            {
                // only show the CPC Contact option
                dataTable = sectionData.Tables[0].Select("REG_SECTION_TYPE_ID = " + CON.SectionTypeID.CpcContactInformation).CopyToDataTable();
            }

            // SAM582 Only Show CMC Contact Info page during CMC contact update
            if (this.WorkflowEventTypeId == CON.WorkflowEventType.CMCUpdate)
            {
                // only show the CPC Contact option
                dataTable = sectionData.Tables[0].Select("REG_SECTION_TYPE_ID = " + CON.SectionTypeID.CMCContactInformation).CopyToDataTable();
            }

            mostCommonUpd.SectionList = dataTable;
            mostCommonUpd.DataBind();
        }

        List<string> pageNames = new DataView(sectionData.Tables[0]).ToTable(true, "REG_PAGE_NAME").AsEnumerable().Select(r => r.Field<string>("REG_PAGE_NAME")).ToList();
        List<DataTable> tablePages = new List<DataTable>();

       
        // OHPNM-7214 -- don't show these if this is a CPC Contact Only Update
        if (!this.IsUpdateCPCContactOnly)
        {
            foreach (var pageName in pageNames)
            {
                var pageNameTable = Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) ? sectionData.Tables[0].Select("IS_COMMON_UPDATE = 0 AND IS_VISIBLE = 1 AND IS_UPDATABLE_SECTION = 1 AND REG_PAGE_NAME = '" + pageName + "'" + " AND " + strAppealsExpr) :
                    sectionData.Tables[0].Select("IS_COMMON_UPDATE = 0 AND IS_VISIBLE = 1 AND IS_UPDATABLE_SECTION = 1 AND REG_PAGE_NAME = '" + pageName + "'");

                if (pageNameTable.Length > 0)
                {
                    tablePages.Add(pageNameTable.CopyToDataTable());
                }
            }

            rptUpdateSections.DataSource = tablePages;
            rptUpdateSections.DataBind();
        }        
    }

    private DataSet LoadSectionData()
    {
        int entityTypeId = 0, providerTypeId = 0, diddReferralId = 0, specialtyTypeID = 0;
        string taxID = string.Empty; ;
        int taxIDTypeID = 0;
        
        Registration.SetEntityProviderTypesDIDD(this.RegistrationId, ref entityTypeId, ref providerTypeId, ref diddReferralId, ref specialtyTypeID, ref taxID, ref taxIDTypeID);
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("ENTITY_TYPE_ID", entityTypeId.ToString());
        parms.Add("PROVIDER_TYPE_ID", providerTypeId.ToString());
        parms.Add("DIDD_REFERRAL_ID", diddReferralId.ToString());
        parms.Add("REG_ID", this.RegistrationId.ToString());
        if(!string.IsNullOrEmpty(CurrentTaskName.ToString())){
            parms.Add("TASK_NAME", CurrentTaskName.ToString());
        }
        else
        {
            parms.Add("TASK_NAME", "");
        }        
        parms.Add("WORKFLOW_ID", this.WF_WorkflowID.ToString());
        if (Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) ||
            Helper.IsLoggedInUserInAdminRole()) parms.Add("ALLOW_CERTIFICATION_VISIBLE", "1");
        parms.Add("USER_ID", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        string roleName = SessionVarRetriever.MyQueueSelectedRoleName;
        if (string.IsNullOrEmpty(roleName))
            roleName = Helper.GetUserRole(HttpContext.Current.User.Identity.Name);
        parms.Add("RoleName", roleName);
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_PAGE_SECTION_SETTING_GETALL", parms);

        
        return ds;

    }


    protected void rptUpdateSections_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataTable dataItem = (DataTable)e.Item.DataItem;
        var sectionUpdateListItem = (UserControls_SectionUpdateList)e.Item.FindControl("sectionItem");
        sectionUpdateListItem.Name = dataItem.Rows[0]["DISPLAY_NAME"].ToString();
        sectionUpdateListItem.IconSrc = "~/Images/" + dataItem.Rows[0]["PAGE_DISPLAY_ICON"].ToString();
        sectionUpdateListItem.SectionList = dataItem;
        sectionUpdateListItem.DataBind();
    }
}