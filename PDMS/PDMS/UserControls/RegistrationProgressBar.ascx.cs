using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_RegistrationProgressBar : System.Web.UI.UserControl
{
    private int DIDDReferralId;
    private int EntityTypeId;
    private int ProviderTypeId;
    private int SpecialtyTypeID;
    private string TaxID;
    private int TaxIDTypeID;
    private DataTable dtPageData = null;
    private DataTable dtSectionData = null;

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.Visible = (HttpContext.Current.User.Identity.IsAuthenticated);

            rrMenu.Width = 450;

        }


    }

    public delegate void RefreshEventHandler(int step);
    public event RefreshEventHandler RefreshEvent;

    protected void Page_PreRender(object sender, EventArgs e)
    {
        // LoadToolbar();
    }




    private string UpdateButtonText(string ImageUrl)
    {

        string rtn = "Start";

        if (ImageUrl.Contains("check.png"))
            rtn = "View";

        if (ImageUrl.Contains("icon-circle-slash.png"))
            rtn = "Update";

        if (ImageUrl.Contains("bullet-red.png") || (ImageUrl.Contains("bullet-yellow.png")))
            rtn = "View";

        return rtn;
    }


    private string GetCssName(int statusId, string statusType)
    {
        string rtn = ""; //~/Images/RightArrow.png

        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))
        {
            if (statusId == CON.RegistrationProviderServicesStatusTypeId.Approved) rtn = "completebg";
            else if (statusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider) rtn = "inProcessbg";
            if (statusId == CON.RegistrationProviderStatusTypeId.Modified) rtn = "needAttentionbg";
        }
        else if (Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name))
        {
            if (statusId == CON.RegistrationProviderServicesStatusTypeId.Approved) rtn = "completebg";
            else if (statusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider) rtn = "inProcessbg";
            if (statusId == CON.RegistrationProviderStatusTypeId.Modified && statusType == CON.StatusType.RegistrationProviderStatusTypeId) rtn = "needAttentionbg";
        }
        else
        {
            if (statusId == CON.RegistrationProviderStatusTypeId.Complete) rtn = "completebg";
            else if (statusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider && Helper.RegistrationInReturnedToProvider(this.WorkflowPage.RegistrationId)) rtn = "inProcessbg";
            else if (statusId == CON.RegistrationProviderStatusTypeId.Modified) rtn = "needAttentionbg";
        }


        return rtn;
    }

    private string GetImageUrl(int statusId, bool isEditable, string statusType)
    {
        string rtn = "~/Images/blank.png"; 

        if (!isEditable)
        {
            rtn = "~/Images/icon-circle-slash.png";
        }
        else if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))
        {
            if (statusId == CON.RegistrationProviderServicesStatusTypeId.Approved) rtn = "~/Images/StepCheck.png";
            else if (statusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider) rtn = "~/Images/InProcess.png";
            if (statusId == CON.RegistrationProviderStatusTypeId.Modified) rtn = "~/Images/bullet-red.png";
        }
        else if (Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name))
        {
            if (statusId == CON.RegistrationProviderServicesStatusTypeId.Approved) rtn = "~/Images/StepCheck.png";
            else if (statusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider) rtn = "~/Images/InProcess.png";
            if (statusId == CON.RegistrationProviderStatusTypeId.Modified && statusType == CON.StatusType.RegistrationProviderStatusTypeId) rtn = "~/Images/bullet-red.png";
        }
        else
        {
            if (statusId == CON.RegistrationProviderStatusTypeId.Complete) rtn = "~/Images/StepCheck.png";
            else if (statusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider && Helper.RegistrationInReturnedToProvider(this.WorkflowPage.RegistrationId)) rtn = "~/Images/InProcess.png";
            else if (statusId == CON.RegistrationProviderStatusTypeId.Modified) rtn = "~/Images/bullet-red.png";
        }


        return rtn;
    }

    public void RemoveNode(TreeNode theNode)
    {
        System.Web.UI.WebControls.TreeNode parent = theNode.Parent;
        if (parent != null)
        {
            parent.ChildNodes.Remove(theNode);
        }
    }
    private void LoadRegistrationNodes()
    {
        int seq = 0;
        this.WorkflowPage.RegistrationNodes = null;
        Dictionary<int, KeyValuePair<string, int>> dict = CON.SectionTypeKeyValue.SectionType;
        DataView dvAppeal = new DataView(dtSectionData);
        //Hide appeal in provider view.
        if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) || !(IsRegistrationWentThroughAppeals()))
        {

            dvAppeal.RowFilter = "SECTION_DISPLAY_NAME <> 'Appeals'";

            dtSectionData = dvAppeal.ToTable();
        }
        if(!(Helper.IsUserInRestrictedServiceViewRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInRestrictedServiceUpdateRole(HttpContext.Current.User.Identity.Name)))
        {
            DataView dvRs = new DataView(dtSectionData);
            dvRs.RowFilter = "SECTION_DISPLAY_NAME <> 'Restricted Service'";
            dtSectionData = dvRs.ToTable();

            //DataView dvCM = new DataView(dtSectionData);
            //dvCM.RowFilter = "SECTION_DISPLAY_NAME <> 'Contract Maintenance'";
            //dtSectionData = dvCM.ToTable();
        }
        if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) && Helper.IsUpdateCPCContact)
        {
            // only show them the CPC Contact Info page
            dvAppeal.RowFilter = "REG_SECTION_TYPE_ID = " + CON.SectionTypeID.CpcContactInformation;
            dtSectionData = dvAppeal.ToTable();
        }
        if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) && this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CMCUpdate && !(Session["ViewProviderFile"] != null && Convert.ToBoolean(Session["ViewProviderFile"])))
        {
            // only show them the CPC Contact Info page
            dvAppeal.RowFilter = "REG_SECTION_TYPE_ID = " + CON.SectionTypeID.CMCContactInformation;
            dtSectionData = dvAppeal.ToTable();
        }

        //OHPNM-10426
        ////I/U: ODM State Administrator, Credentialing Specialist, Credentialing Supervisor, ODM Credentialing Quality Assurance, ODM Credentialing Specialist (LTC), ODM Credentialing Supervisor 
        //if (!(Helper.IsUserInAdminRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserCredentialSpecialist(HttpContext.Current.User.Identity.Name) || Helper.IsUserCredentialingSupervisor(HttpContext.Current.User.Identity.Name) || Helper.IsLoggedInUserInCredentialingQualityAssuranceRole() || Helper.IsUserODMCredentialingSpecialistLTC(HttpContext.Current.User.Identity.Name) || Helper.IsUserODMCredentialingSupervisor(HttpContext.Current.User.Identity.Name)))
        //{
        //    dvAppeal.RowFilter = "SECTION_DISPLAY_NAME <> 'Provider Credentialing'";
        //    dtSectionData = dvAppeal.ToTable();
        //}

        foreach (DataRow dr in dtSectionData.Rows)
        {
            int statusId = 0;
            if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry && !string.IsNullOrEmpty(dr["REG_PROVIDER_STATUS_TYPE_ID"].ToString()))
                statusId = Convert.ToInt32(dr["REG_PROVIDER_STATUS_TYPE_ID"].ToString());

            else if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview && !string.IsNullOrEmpty(dr["REG_PROVIDER_STATUS_TYPE_ID"].ToString()) &&
               Convert.ToInt32(dr["REG_PROVIDER_STATUS_TYPE_ID"].ToString()) == CON.RegistrationProviderStatusTypeId.Modified)
                statusId = Convert.ToInt32(dr["REG_PROVIDER_STATUS_TYPE_ID"].ToString());

            else if (!string.IsNullOrEmpty(dr["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"].ToString()))
                statusId = Convert.ToInt32(dr["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"].ToString());

           

            int providerStatusId = 0;

            if (!string.IsNullOrEmpty(dr["REG_PROVIDER_STATUS_TYPE_ID"].ToString()))
                providerStatusId = Convert.ToInt32(dr["REG_PROVIDER_STATUS_TYPE_ID"].ToString());
            //if (!string.IsNullOrEmpty(dr["SEQUENCE_ID"].ToString()))
            //seq = Convert.ToInt32(dr["SEQUENCE_ID"].ToString());
            int IsRequired = 0;
			int IsEditable = 0;
            int ExcludeSectionFromProvReviewCount = 0;

            if (!string.IsNullOrEmpty(dr["IS_REQUIRED"].ToString()))
                IsRequired = Convert.ToBoolean(dr["IS_REQUIRED"].ToString()) ? 1 : 0;

            if (!string.IsNullOrEmpty(dr["IS_EDITABLE"].ToString()))
                IsEditable = Convert.ToBoolean(dr["IS_EDITABLE"].ToString()) ? 1 : 0;

            if (!string.IsNullOrEmpty(dr["EXCLUDE_SECTION_FRM_PROVIDER_REVIEW_NODE_COUNT"].ToString()))
                ExcludeSectionFromProvReviewCount = Convert.ToBoolean(dr["EXCLUDE_SECTION_FRM_PROVIDER_REVIEW_NODE_COUNT"].ToString()) ? 1 : 0;

            if (dr["REG_SECTION_TYPE_ID"] != null && !string.IsNullOrEmpty(dr["REG_SECTION_TYPE_ID"].ToString()) && dr["IS_VISIBLE"].ToString() == "True")
            {
                seq++;

                string url = dict.FirstOrDefault(x => x.Key == Convert.ToInt32(dr["REG_SECTION_TYPE_ID"])).Value.Key;

                //SAM763 configure medical malpractice, work history, professional liability insurance, and education pages to automatically display the green checkmark during Provider Review. 
                if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview && this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.RegistrationNew)
                {
                    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

                    DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SECTION_STATUS");
                    DataView dvSS = new DataView(ds.Tables[0]);
                    DataTable dtss = new DataTable();
                    
                    if (Convert.ToInt32(dr["REG_SECTION_TYPE_ID"].ToString()) == CON.SectionTypeID.MalpracticeClaimsHistory)
                    {
                        dvSS.RowFilter = "REG_SECTION_TYPE_ID = " + CON.SectionTypeID.MalpracticeClaimsHistory;

                        dtss = dvSS.ToTable();
                        if (!Helper.HasRows(dtss))
                        {
                            psc.SaveRegistrationSectionStatus(this.WorkflowPage.RegistrationId, 0, CON.SectionTypeID.MalpracticeClaimsHistory, CON.RegistrationProviderStatusTypeId.Complete, CON.RegistrationProviderServicesStatusTypeId.Approved, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                            statusId = CON.RegistrationProviderServicesStatusTypeId.Approved;
                        }
                    }
                    if (Convert.ToInt32(dr["REG_SECTION_TYPE_ID"].ToString()) == CON.SectionTypeID.WorkHistory)
                    {
                        dvSS.RowFilter = "REG_SECTION_TYPE_ID = " + CON.SectionTypeID.WorkHistory;

                        dtss = dvSS.ToTable();
                        if (!Helper.HasRows(dtss))
                        {
                            psc.SaveRegistrationSectionStatus(this.WorkflowPage.RegistrationId, 0, CON.SectionTypeID.WorkHistory, CON.RegistrationProviderStatusTypeId.Complete, CON.RegistrationProviderServicesStatusTypeId.Approved, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                            statusId = CON.RegistrationProviderServicesStatusTypeId.Approved;
                        }
                    }
                    if (Convert.ToInt32(dr["REG_SECTION_TYPE_ID"].ToString()) == CON.SectionTypeID.Insurance)
                    {
                        dvSS.RowFilter = "REG_SECTION_TYPE_ID = " + CON.SectionTypeID.Insurance;

                        dtss = dvSS.ToTable();
                        if (!Helper.HasRows(dtss))
                        {
                            psc.SaveRegistrationSectionStatus(this.WorkflowPage.RegistrationId, 0, CON.SectionTypeID.Insurance, CON.RegistrationProviderStatusTypeId.Complete, CON.RegistrationProviderServicesStatusTypeId.Approved, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                            statusId = CON.RegistrationProviderServicesStatusTypeId.Approved;
                        }
                    }
                    if (Convert.ToInt32(dr["REG_SECTION_TYPE_ID"].ToString()) == CON.SectionTypeID.EmploymentHistory)
                    {
                        dvSS.RowFilter = "REG_SECTION_TYPE_ID = " + CON.SectionTypeID.EmploymentHistory;

                        dtss = dvSS.ToTable();
                        if (!Helper.HasRows(dtss))
                        {
                            psc.SaveRegistrationSectionStatus(this.WorkflowPage.RegistrationId, 0, CON.SectionTypeID.EmploymentHistory, CON.RegistrationProviderStatusTypeId.Complete, CON.RegistrationProviderServicesStatusTypeId.Approved, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                            statusId = CON.RegistrationProviderServicesStatusTypeId.Approved;
                        }
                    }
                }

                if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CMC &&
                (this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.ProviderDataEntry ||
                this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.ProviderReview ||
                this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.TransactionMonitoring)
                && dr["IS_VISIBLE"].ToString() == "True")
                {
                    string displayMenu = dr["SECTION_DISPLAY_NAME"].ToString();
                    providerStatusId = Convert.ToInt32(dr["REG_PROVIDER_STATUS_TYPE_ID"].ToString() == "" ? "0" : dr["REG_PROVIDER_STATUS_TYPE_ID"].ToString());

                    this.WorkflowPage.RegistrationNodes.Add(Convert.ToInt32(dr["REG_SECTION_TYPE_ID"].ToString()), new RegistrationNode(seq, displayMenu, Convert.ToInt32(dr["REG_SECTION_TYPE_ID"].ToString()), statusId, IsRequired, providerStatusId, IsEditable, ExcludeSectionFromProvReviewCount));
                }
                else if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CMC &&
                this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry)
                {
                    string displayMenu = dr["SECTION_DISPLAY_NAME"].ToString();
                    providerStatusId = Convert.ToInt32(dr["REG_PROVIDER_STATUS_TYPE_ID"].ToString() == "" ? "0" : dr["REG_PROVIDER_STATUS_TYPE_ID"].ToString());

                    this.WorkflowPage.RegistrationNodes.Add(Convert.ToInt32(dr["REG_SECTION_TYPE_ID"].ToString()), new RegistrationNode(seq, displayMenu, Convert.ToInt32(dr["REG_SECTION_TYPE_ID"].ToString()), statusId, IsRequired, providerStatusId, IsEditable, ExcludeSectionFromProvReviewCount));
                }
                if (this.WorkflowPage.WF_WorkflowID != CON.WorkflowType.CMC)
                {
                    string displayMenu = dr["SECTION_DISPLAY_NAME"].ToString();
                    if (IsCredentialingProcess())
                    {
                        if (displayMenu == "Provider Screening")
                            displayMenu = "Provider Credentialing";
                        if (displayMenu != "Site Visit Screening" && displayMenu != "Orientation Session Screening")
                            this.WorkflowPage.RegistrationNodes.Add(Convert.ToInt32(dr["REG_SECTION_TYPE_ID"].ToString()), new RegistrationNode(seq, displayMenu, Convert.ToInt32(dr["REG_SECTION_TYPE_ID"].ToString()), statusId, IsRequired, providerStatusId, IsEditable, ExcludeSectionFromProvReviewCount));
                    }
                    else
                        this.WorkflowPage.RegistrationNodes.Add(Convert.ToInt32(dr["REG_SECTION_TYPE_ID"].ToString()), new RegistrationNode(seq, displayMenu, Convert.ToInt32(dr["REG_SECTION_TYPE_ID"].ToString()), statusId, IsRequired, providerStatusId, IsEditable, ExcludeSectionFromProvReviewCount));
                }               

            }
            else if (dr["IS_VISIBLE"].ToString() == "True")
            {
                seq++;
                var constants =
                    from fieldInfo in typeof(CON.RegistrationPageType).GetFields()
                    where (fieldInfo.Attributes & FieldAttributes.Literal) != 0 && Convert.ToInt32(fieldInfo.GetRawConstantValue()) == Convert.ToInt32(dr["REG_PAGE_TYPE_ID"])
                    select fieldInfo.Name;

                string url = Registration.GetStepText(Convert.ToInt32(dr["REG_PAGE_TYPE_ID"].ToString()));//constants.First()
                string displayMenu = dr["SECTION_DISPLAY_NAME"].ToString();
                if (IsCredentialingProcess())
                {
                    if (displayMenu == "Provider Screening")
                        displayMenu = "Provider Credentialing";
                    if (displayMenu != "Site Visit Screening" && displayMenu != "Orientation Session Screening")
                        this.WorkflowPage.RegistrationNodes.Add(Convert.ToInt32(dr["REG_PAGE_TYPE_ID"].ToString()), new RegistrationNode(seq, displayMenu, Convert.ToInt32(dr["REG_PAGE_TYPE_ID"].ToString()), statusId, IsRequired, providerStatusId, IsEditable, ExcludeSectionFromProvReviewCount));
                }
                else
                    this.WorkflowPage.RegistrationNodes.Add(Convert.ToInt32(dr["REG_PAGE_TYPE_ID"].ToString()), new RegistrationNode(seq, displayMenu, Convert.ToInt32(dr["REG_PAGE_TYPE_ID"].ToString()), statusId, IsRequired, providerStatusId, IsEditable, ExcludeSectionFromProvReviewCount));

            }
        }

        if (!Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) 
            && this.WorkflowPage.MMISProviderTypeID != CON.LTCFaciitiesProviderTypeID.BuildingandWingIDforLTCFacilitesOnly)
        {
            try
            {
                if (!Helper.IsUserInInternalApplicationsEntryRole(HttpContext.Current.User.Identity.Name))
                {
                    this.WorkflowPage.RegistrationNodes.Add(99, new RegistrationNode(seq, "Workflow Steps", 99, 0, 0, 0, 0, 1));
                    this.WorkflowPage.RegistrationNodes.Add(10000, new RegistrationNode(seq, "Provider Feed", 10000, 0, 0, 0, 0, 1));
                }
            }
            catch (Exception ex)
            {

            }
        }

        // JIRA 2961
		// OHPNM-4078 - removing for now and OHPNM-4865 adding it back in
        if (!Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) && 
            this.WorkflowPage.MMISProviderTypeID != CON.LTCFaciitiesProviderTypeID.BuildingandWingIDforLTCFacilitesOnly &&
            Helper.IsUserInRestrictedServiceViewRole(HttpContext.Current.User.Identity.Name)) 
        {
            try
            {
                this.WorkflowPage.RegistrationNodes.Add(101, new RegistrationNode(seq, "Transaction Queue", 101, 0, 0, 0, 0, 1));
            }
            catch (Exception ex)
            {

            }
        }

    }

    private bool IsCredentialingProcess()
    {
        //if (this.WorkflowPage.ApplicationTypeID == 11)
        //    return true;
        //else
        return false;
    }
    private void LoadSectionData(int entityTypeId, int providerTypeId, int diddReferralId)
    {
        
        int workflowID = this.WorkflowPage.WF_WorkflowID;
        string currentTaskName = this.WorkflowPage.CurrentTaskName;
        if (Session["ViewProviderFile"] != null && Convert.ToBoolean(Session["ViewProviderFile"]) && this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CMC)
        {
            currentTaskName = string.Empty;
            workflowID = 1;
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("ENTITY_TYPE_ID", entityTypeId.ToString());
        parms.Add("PROVIDER_TYPE_ID", providerTypeId.ToString());
        parms.Add("DIDD_REFERRAL_ID", diddReferralId.ToString());
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        if(!string.IsNullOrEmpty(this.WorkflowPage.CurrentTaskName) && !string.IsNullOrEmpty(currentTaskName))
         parms.Add("TASK_NAME", currentTaskName);
        parms.Add("WORKFLOW_ID", workflowID.ToString());
        if (Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) || Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name) ||
            Helper.IsLoggedInUserInAdminRole()) parms.Add("ALLOW_CERTIFICATION_VISIBLE", "1");
        parms.Add("USER_ID", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("RoleName", SessionVarRetriever.MyQueueSelectedRoleName.ToString()==""? Helper.GetUserRole(HttpContext.Current.User.Identity.Name).ToString():SessionVarRetriever.MyQueueSelectedRoleName.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_PAGE_SECTION_SETTING_GETALL", parms);

        dtSectionData = ds.Tables[0];
    }

    private int GetStatusId(string pageName, ref string statusType)
    {
        foreach (DataRow row in dtSectionData.Rows)
        {
            if (Helper.GetString("REG_SECTION_NAME", row) == pageName)
            {
                if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) || Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInDBHReviewerRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInStateReviewerRole(HttpContext.Current.User.Identity.Name))
                {
                    if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) && Helper.GetInt("REG_PROVIDER_STATUS_TYPE_ID", row) == CON.RegistrationProviderStatusTypeId.Modified)
                    {
                        statusType = CON.StatusType.RegistrationProviderStatusTypeId;
                        return Helper.GetInt("REG_PROVIDER_STATUS_TYPE_ID", row);
                    }
                    statusType = CON.StatusType.RegistrationProviderServicesStatusTypeId;
                    return Helper.GetInt("REG_PROVIDER_SERVICES_STATUS_TYPE_ID", row);
                }
                else
                {
                    if (Helper.GetInt("REG_PROVIDER_STATUS_TYPE_ID", row) == CON.RegistrationProviderStatusTypeId.NotComplete && Helper.GetInt("REG_PROVIDER_SERVICES_STATUS_TYPE_ID", row) == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider)
                    {
                        statusType = CON.StatusType.RegistrationProviderServicesStatusTypeId;
                        return Helper.GetInt("REG_PROVIDER_SERVICES_STATUS_TYPE_ID", row);
                    }
                    statusType = CON.StatusType.RegistrationProviderStatusTypeId;
                    return Helper.GetInt("REG_PROVIDER_STATUS_TYPE_ID", row);
                }
            }
        }
        return 0;
    }

    private bool PageIsVisible(string pageName, string taxID, int taxIDTypeID)
    {
        if (this.WorkflowPage.IsWaiverServiceProvider && (pageName == "Substitute W4 Form" || pageName == "Substitute W9 Form"))
        {
            if (pageName == "Substitute W9 Form")
            {
                //always show w9 for organization
                if (EntityTypeId != CON.ProviderCategoryTypeID.Individual)
                    return true;

                //always show w9 for individual filing with EIN tax type
                if (EntityTypeId == CON.ProviderCategoryTypeID.Individual && taxIDTypeID == CON.TaxIDType.EIN)
                    return true;
            }

            if (pageName == "Substitute W4 Form")
            {
                //never show w4 for organization
                if (EntityTypeId != CON.ProviderCategoryTypeID.Individual)
                    return false;

                //never show w4 for individual filing with EIN tax type
                if (EntityTypeId == CON.ProviderCategoryTypeID.Individual && taxIDTypeID == CON.TaxIDType.EIN)
                    return false;
            }

            //BEGIN SERVICE SPECIFIC SETTINGS FOR HIDE/SHOW OF W9/W4.
            //Never show both
            PDMSService.PDMSServiceClient psc1 = new PDMSService.PDMSServiceClient();
            DataSet dsServices = psc1.SelectDIDDReferralService(DIDDReferralId);
            if (Helper.HasRows(dsServices))
            {
                DataTable dt = FilterServicesForW4Display(dsServices);
                bool showW4 = dt == null ? false : true;
                if (pageName == "Substitute W4 Form")
                    return showW4;
                else if (pageName == "Substitute W9 Form")
                    return !showW4;
            }
            else
            {
                //default, but should never have an nfocus registration with 0 services
                return false;
            }
        }

        bool rtn = false;

        foreach (DataRow row in dtSectionData.Rows)
        {
            if (Helper.GetString("REG_PAGE_NAME", row) == pageName)
            {
                bool isVisible = Helper.GetBool("IS_VISIBLE", row);
                rtn = isVisible;
                break;
            }
        }

        return rtn;
    }

    private DataTable FilterServicesForW4Display(DataSet ds)
    {
        //TODO, these are hard-coded and should be in an appsetting
        string selectPart = "WAIVER_TYPE_CODE IN (5665, 1691, 2500, 6700, 4456, 4475, 1113, 9233, 2456)";

        DataTable dt = ds.Tables[0];
        if (dt.Select(selectPart).Count() > 0)
        {
            return dt.Select(selectPart).CopyToDataTable();
        }
        else
        {
            return null;
        }
    }

    private bool PageIsEditable(string pageName)
    {
        bool rtn = false;
        //if (pageName == "Substitute W4 Form") return true;
        foreach (DataRow row in dtPageData.Rows)
        {
            if (Helper.GetString("REG_PAGE_NAME", row) == pageName)
            {
                bool isVisible = Helper.GetBool("IS_EDITABLE", row);
                rtn = isVisible;
                break;
            }
        }

        return rtn;
    }

    // Rebind and refresh progressbar
    public void Refresh()
    {
        LoadToolbar();
    }

    protected void rtProgress_ButtonClick(object sender, Telerik.Web.UI.RadToolBarButtonEventArgs e)
    {
    }


    private void LoadJumpTo()
    {

    }

    private void LoadToolbar()
    {
        rrMenu.Items.Clear();

        if (!Helper.HasRows(dtSectionData))
        {
            Registration.SetEntityProviderTypesDIDD(this.WorkflowPage.RegistrationId, ref EntityTypeId, ref ProviderTypeId, ref DIDDReferralId, ref SpecialtyTypeID, ref TaxID, ref TaxIDTypeID);
            LoadSectionData(EntityTypeId, ProviderTypeId, DIDDReferralId);
            LoadRegistrationNodes();
        }
        AddToolBarButtons();
    }

    public void SelectStep()
    {
        // OHPNM-20089 force to Provider Information if we end up with an unknown current step
        int currentStep = 0;
        if (this.WorkflowPage.RegistrationStep != null)
            currentStep = this.WorkflowPage.RegistrationStep;
        var rtb = (RadToolBarButton)rrMenu.FindItemByValue(currentStep.ToString());
        if (rtb != null)
        {
            rtb.Checked = true;
            hndCurrentItem.Value = rtb.Value;
        } else
        {
            hndCurrentItem.Value = "0";
        }

        if (RadJumpTo.Items.Any())
        {
            RadJumpTo.DataBind();
            if (RadJumpTo.Items.Contains(new RadComboBoxItem(currentStep.ToString())))
            {
                RadJumpTo.SelectedValue = currentStep.ToString();
            }
            string jumpto_text = "Primary Service Address";
            if (rtb != null)
            {
                jumpto_text = rtb.Text;
            }
            RadJumpTo.Text = jumpto_text.IndexOf('<') > 0 ? jumpto_text.Substring(0, jumpto_text.IndexOf('<')) : jumpto_text;
        }
    }

    protected void RadJumpTo_DataBound(object sender, EventArgs e)
    {

        //set the initial footer label

        ((Literal)RadJumpTo.Footer.FindControl("RadComboItemsCount")).Text = Convert.ToString(RadJumpTo.Items.Count);

    }


    protected void RadJumpTo_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {

        //set the Text and Value property of every item

        //here you can set any other properties like Enabled, ToolTip, Visible, etc.

        string jumpto_text = ((DataRowView)e.Item.DataItem)["Text"].ToString();

        e.Item.Text = jumpto_text.IndexOf('<') > 0 ? jumpto_text.Substring(0, jumpto_text.IndexOf('<')) : jumpto_text;

        e.Item.Value = ((DataRowView)e.Item.DataItem)["Value"].ToString();

    }


    private DataTable getJumpToTable()
    {
        DataTable jumpToTable = new DataTable();
        jumpToTable.Columns.Add("ID");
        jumpToTable.Columns.Add("Value");
        jumpToTable.Columns.Add("Text");
        jumpToTable.Columns.Add("Icon");
        jumpToTable.Columns.Add("Status");


        return jumpToTable;
    }

    private void AddToolBarButtons()
    {
        int ctr = 0;
        List<RegistrationNode> nodes = HideScreeningNodes();
        DataTable jumpToTable = getJumpToTable();

        foreach (RegistrationNode node in nodes)
        {
            RadToolBarButton rtb = new RadToolBarButton();
            Label lblRequired = new Label();
            string statusType = "";
            bool isEditable = true;

            lblRequired.Text = "<span style='color:#C80000'>*</span>";
            string buttonText = node.MenuPath;
            if (node.MenuPath == "Affiliations")
                if (Registration.IsEPDProvider(this.WorkflowPage.RegistrationId) || Registration.IsHomeHealthProvider(this.WorkflowPage.RegistrationId))
                    buttonText = "PCA Aides";
                else if (Registration.CanIndividualAddPA(this.WorkflowPage.RegistrationId))
                    buttonText = "Physician Assistants";

            if (node.MenuPath == "CDS Number" && Registration.IsCDSPageRequired(this.WorkflowPage.RegistrationId))
                node.IsRequired = 1;

            rtb.Text = buttonText + (node.IsRequired == 1 ? lblRequired.Text : "");
            rtb.Value = node.Step.ToString();
            //rtb.NavigateUrl = "~/Process/Registration.aspx?Step=" + node.Step.ToString();

            string ImgUrl = "~/images/" + node.MenuPath.Replace(" ", "") + ".png";
            if (File.Exists(Server.MapPath(ImgUrl)))
                rtb.ImageUrl = ImgUrl;
            else
                rtb.ImageUrl = "~/images/ProviderInformation.png";

            rtb.ImagePosition = ToolBarImagePosition.AboveText;
            rtb.CheckOnClick = true;
            rtb.CssClass = GetCssName(node.StatusId, statusType);
            rtb.Enabled = isEditable;
            rtb.PostBack = true;
            rtb.EnableViewState = false;

            rrMenu.Items.Add(rtb);

            jumpToTable.Rows.Add(new String[] { node.Sequence.ToString(), rtb.Value, rtb.Text, rtb.ImageUrl, GetImageUrl(node.StatusId, isEditable, statusType) });

            ctr++;
            if (ctr < nodes.Count)
            {
                ////add arrows
                RadToolBarButton rtbArrow = new RadToolBarButton();
                rtbArrow.ImageUrl = "~/Images/RightArrow.png";//GetImageUrl(node.StatusId, isEditable, statusType); //"~/images/RightArrow.png";
                rtbArrow.ToolTip = "Arrow button";
                rtbArrow.HoveredCssClass = rtbArrow.CheckedCssClass = "noHover";
                rtbArrow.ClickedCssClass = rtbArrow.FocusedCssClass = "noHover";
                rtbArrow.EnableViewState = false;
                rtbArrow.PostBack = false;
                rrMenu.Items.Add(rtbArrow);
            }

        }


        RadJumpTo.DataSource = jumpToTable;
		RadJumpTo.DataBind();
		RadJumpTo.EnableViewState = false;
    }

    private RadToolBar SetToolBar()
    {
        RadToolBar rt = new RadToolBar();
        rt.Skin = "PDMSModern";
        rt.CausesValidation = false;
        //rt.BackColor = System.Drawing.Color.White;
        //rt.BorderColor = System.Drawing.Color.White;
        //rt.Skin = "Office2010Blue";
        rt.EnableEmbeddedSkins = false;
        return (rt);

    }
    private List<RegistrationNode> HideScreeningNodes()
    {
        var keysToBeFiltered = new HashSet<int>
        {
            CON.RegistrationPageType.ProviderScreening,
            CON.RegistrationPageType.OwnerScreening,
            CON.RegistrationPageType.SiteVisitScreening ,
            CON.RegistrationPageType.HouseholdMemberScreening,
            CON.RegistrationPageType.BackgroundCheck,
            CON.RegistrationPageType.OrientationInformation,
            CON.SectionTypeID.ProviderCredentialing
        };

        var SiteVisistkeysToBeFiltered = new HashSet<int>
        {
            CON.RegistrationPageType.ProviderScreening,
            CON.RegistrationPageType.OwnerScreening,
            CON.RegistrationPageType.HouseholdMemberScreening,
            CON.RegistrationPageType.BackgroundCheck,
            CON.RegistrationPageType.OrientationInformation,
            CON.SectionTypeID.ProviderCredentialing
        };

        var OwnerScreeningPagesToBeFiltered = new HashSet<int>
        {
            CON.RegistrationPageType.OwnerScreening
        };

        var keysToBeFilteredinProviderReview = new HashSet<int>
        {
            CON.RegistrationPageType.SiteVisitScreening ,
            CON.RegistrationPageType.HouseholdMemberScreening,
            CON.RegistrationPageType.BackgroundCheck,
            CON.RegistrationPageType.OrientationInformation,
            CON.SectionTypeID.ProviderCredentialing
        };

        List<RegistrationNode> nodes = new List<RegistrationNode>();

        DataRow row = Registration.GetRegistration(this.WorkflowPage.RegistrationId);
        bool isReturnForSiteVisit = false;
        if (row != null && Helper.GetInt("REGISTRATION_STATUS_TYPE_ID", row) == CON.RegistrationStatusTypeId.ReturnToProviderForSiteVisit)
        {
            isReturnForSiteVisit = true;
        }
        if(Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) && isReturnForSiteVisit == true)
        {
            SiteVisistkeysToBeFiltered.ToList().ForEach(x => this.WorkflowPage.RegistrationNodes.Remove(x));
            for(int i=0;i<this.WorkflowPage.RegistrationNodes.Count;i++)
            {
                this.WorkflowPage.RegistrationNodes.ElementAt(i).Value.Sequence = i + 1;
            }
            nodes.AddRange(this.WorkflowPage.RegistrationNodes.Select(x => x.Value));
        }
        else if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name)) 
        {
            keysToBeFiltered.ToList().ForEach(x => this.WorkflowPage.RegistrationNodes.Remove(x));
            for (int i = 0; i < this.WorkflowPage.RegistrationNodes.Count; i++)
            {
                this.WorkflowPage.RegistrationNodes.ElementAt(i).Value.Sequence = i + 1;
            }
            nodes.AddRange(this.WorkflowPage.RegistrationNodes.Select(x => x.Value));
        }
        //OHPNM-18964 - Show Provider and Owner Screening pages in Provider Review
        else if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview)
        {
            keysToBeFilteredinProviderReview.ToList().ForEach(x => this.WorkflowPage.RegistrationNodes.Remove(x));
            for (int i = 0; i < this.WorkflowPage.RegistrationNodes.Count; i++)
            {
                this.WorkflowPage.RegistrationNodes.ElementAt(i).Value.Sequence = i + 1;
            }
            nodes.AddRange(this.WorkflowPage.RegistrationNodes.Select(x => x.Value));
        }
        else if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry)
        {
            // OHPNM-3198 - user is in provider data entry; make sure ownerscreening (15,15) page does not s
            OwnerScreeningPagesToBeFiltered.ToList().ForEach(x => this.WorkflowPage.RegistrationNodes.Remove(x));
            for (int i = 0; i < this.WorkflowPage.RegistrationNodes.Count; i++)
            {
                this.WorkflowPage.RegistrationNodes.ElementAt(i).Value.Sequence = i + 1;
            }
            nodes.AddRange(this.WorkflowPage.RegistrationNodes.Select(x => x.Value));
        }
        else
        {
            for (int i = 0; i < this.WorkflowPage.RegistrationNodes.Count; i++)
            {
                this.WorkflowPage.RegistrationNodes.ElementAt(i).Value.Sequence = i + 1;
            }
            nodes.AddRange(this.WorkflowPage.RegistrationNodes.Select(x => x.Value));
        }
        return (nodes);
    }


    private bool IsVisible(string pgName)
    {
        bool rtn = false;
        foreach (DataRow row in dtSectionData.Rows)
        {
            if (Helper.GetString("REG_SECTION_NAME", row) == pgName)
            {
                bool isVisible = Helper.GetBool("IS_VISIBLE", row);
                rtn = isVisible;
                break;
            }
        }
        return rtn;
    }

    protected void rrMenu_ItemClick(object sender, RadRotatorEventArgs e)
    {
        //string itemIndex = e.Item.Index.ToString();
        //hndCurrentItem.Value = itemIndex;

        //Telerik.Web.UI.RadRotatorItem frame = e.Item;
        //var RadToolBarItem = frame.FindControl(string.Format("RadToolBar{0}", itemIndex));
        //RadToolBarButton RadToolBarButtonItem = (RadToolBarButton)RadToolBarItem.FindControl(string.Format("RadToolBarButton{0}", itemIndex));

        //RadToolBarButtonItem.Checked = true;

        //if (int.Parse(hndCurrentItem.Value) > 0)
        //   rrMenu.InitialItemIndex = int.Parse(hndCurrentItem.Value);

        //string script = "function f(){showItemByIndex(" + e.Item.Index + ") ;Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "key", script, true);
    }

    protected void rrMenu_ButtonClick(object sender, RadToolBarEventArgs e)
    {
        //Remove session variable for addresses
        if (Session["rblContactTypeVal"] != null)
        {
            if (!string.IsNullOrEmpty(Session["rblContactTypeVal"] as string))
            {
                Session.Remove("rblContactTypeVal");
            }
        }

        if (e.Item.Value != string.Empty)
        {
            this.WorkflowPage.RegistrationStep = int.Parse(e.Item.Value);
            string jumpto_text = e.Item.Text;
            RadJumpTo.Text = jumpto_text.IndexOf('<') > 0 ? jumpto_text.Substring(0, jumpto_text.IndexOf('<')) : jumpto_text;
            RadJumpTo.SelectedValue = this.WorkflowPage.RegistrationStep.ToString();
            this.WorkflowPage.ActiveScreeningID = 0;
        }
        RefreshWorkflowPage();
    }
    protected void RadJumpTo_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //Remove session variable for addresses
        if (Session["rblContactTypeVal"] != null)
        {
            if (!string.IsNullOrEmpty(Session["rblContactTypeVal"] as string))
            {
                Session.Remove("rblContactTypeVal");
            }
        }

        // TODO: EDV The JumpTo is behaving weird. we will come back to it later
        if (!string.IsNullOrEmpty(e.Value))
        {
            this.WorkflowPage.RegistrationStep = int.Parse(e.Value);
            RefreshWorkflowPage();
        }
    }

    public void RefreshWorkflowPage(bool forceRedirect = false)
    {
        // There is no force redirect. It is always refresh
        if (RefreshEvent != null) RefreshEvent(this.WorkflowPage.RegistrationStep);
    }
    private bool IsRegistrationWentThroughAppeals()
    {
        bool rtn = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        int RegID = this.WorkflowPage.RegistrationId;
        if (RegID > 0)
        {
            DataSet ds = psc.IsRegistrationWentThroughAppeals(RegID);

            if (Helper.HasRows(ds))
            {
                if (Helper.HasRows(ds.Tables[0]))
                {
                    rtn = true;


                }

            }
        }
        return rtn;
    }
}

