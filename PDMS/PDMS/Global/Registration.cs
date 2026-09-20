using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

/// <summary>
/// Registration global methods
/// </summary>
public class Registration
{
    private Registration()
    {
    }

    //TODO:  Marked for Change:  should not be getting this from user account information.
    private static int GetDIDDReferralId()
    {
        int rtn = 0;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetUserAccountInformation(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        if (Helper.HasRows(ds)) rtn = Helper.GetInt("DIDD_REFERRAL_ID", ds.Tables[0].Rows[0]);
        return rtn;
    }

    public static bool CheckExistsRegIDByRegId(int regId)
    {
        bool _isExists = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        string returnMsg = psc.CheckRegistrationExistsByRegId(regId);
        if (returnMsg == "Exists")
            _isExists = true;

        return _isExists;
    }

    public static DataRow GetRegistration(int regID)
    {
        DataRow rtn = null;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistration(regID);
        if (Helper.HasRows(ds)) rtn = ds.Tables[0].Rows[0];
        return rtn;
    }

    public static DataRow GetProviderInfo(int regID)
    {
        DataRow rtn = null;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(regID, "PROVIDER");
        if (Helper.HasRows(ds)) rtn = ds.Tables[0].Rows[0];
        return rtn;
    }

    // Check that Registration belongs to me
    public static bool IsMine(int regID, bool isStateReview)
    {
        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) ||
            Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name))
            return true;

        bool rtn = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationByRegID(regID);
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if (dr["UserID"] != DBNull.Value)
            {
                Guid UserID = new Guid(Methods.GetStringValue(dr["UserID"]));
                rtn = UserID == Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            }
        }
        return rtn;
    }

    public static void UpdateRegistrationStatus(int regId, int status, string userId)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", regId.ToString());
        parms.Add("REGISTRATION_STATUS_TYPE_ID", status.ToString());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", userId);
        if (status == CON.RegistrationStatusTypeId.Submitted)
            parms.Add("SUBMIT_DATE_TIME", DateTime.Now.ToString());
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.UpdateRegistration(parms);
    }

    public static int GetSectionStatusId(int regId, int step)
    {
        int rtn = 0;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(regId, "SECTION_STATUS");
        if (Helper.HasRows(ds))
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Helper.GetInt("REG_SECTION_TYPE_ID", dr) == step)
                {
                    if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))
                        rtn = Helper.GetInt("REG_PROVIDER_SERVICES_STATUS_TYPE_ID", dr);
                    else rtn = Helper.GetInt("REG_PROVIDER_STATUS_TYPE_ID", dr);
                }
            }
        }
        return rtn;
    }
    public static int GetPageStatusId(int regId, int step)
    {
        int rtn = 0;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(regId, "PAGE_STATUS");
        if (Helper.HasRows(ds))
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Helper.GetInt("REG_PAGE_TYPE_ID", dr) == step)
                {
                    if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))
                        rtn = Helper.GetInt("REG_PROVIDER_SERVICES_STATUS_TYPE_ID", dr);
                    else rtn = Helper.GetInt("REG_PROVIDER_STATUS_TYPE_ID", dr);
                }
            }
        }
        return rtn;
    }

    // Return TRUE if the page is in a pending status
    public static bool IsPagePending(int regId,int step)
    {
        int val = GetPageStatusId(regId, step);
        return (val == 0 || val == CON.RegistrationProviderServicesStatusTypeId.Pending) ;
    }

    public static void SetProviderSectionNodeStatusId(int regId, int StepId, int statusId)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        psc.SaveRegistrationSectionStatus(regId, 0, StepId, statusId, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        
    }

    public static void SetProviderSectionNodeStatusIdForCredentialing(int regId, int StepId, int statusId)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        psc.SaveRegistrationSectionStatus(regId, 0, StepId, statusId, 1, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

    }

    public static void SetSectionNodeStatusId(int regId, int registrationStep, int statusId)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCCHOP))
        {
            psc.SaveRegistrationSectionStatus(regId, 0, registrationStep, null, statusId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            if(statusId == CON.RegistrationProviderServicesStatusTypeId.Approved)
            {
                
                psc.SaveRegistrationSectionStatus(regId, 0, registrationStep, CON.RegistrationProviderStatusTypeId.Complete, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                
            }
            if (statusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider)
            {
                psc.SaveRegistrationSectionStatus(regId, 0, registrationStep, CON.RegistrationProviderStatusTypeId.NotComplete, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            }
        }
    }

    public static void SetNodeStatusId(int regId, int stepId, int statusId)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) || Helper.IsUserInCredentialingRole(HttpContext.Current.User.Identity.Name))
            psc.SaveRegistrationSectionStatus(regId, 0, stepId, null, statusId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        else psc.SaveRegistrationSectionStatus(regId, 0, stepId, statusId, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
    }

    // Given the step return the step name
    public static string GetStepText(int step)
    {
        return MAXIMUS.Core.Libraries.Constants.SectionTypeKeyValue.GetSectionDisplayName(step);
    }

    public static string GetSectionText(int step)
    {
        return MAXIMUS.Core.Libraries.Constants.SectionTypeKeyValue.GetSectionDisplayName(step);
    }

    // Given the sequence return the step step
    public static int GetSequenceStep(Dictionary<int, RegistrationNode> registrationNodes, int sequence)
    {
        RegistrationNode appNode = registrationNodes.Where(itm => itm.Value.Sequence == sequence).FirstOrDefault().Value;
        if (appNode != null) return appNode.Step;
        return 1;
    }

    public static bool PreviewingRegistrationSection()
    {
        //bug 2234 made this functionality obsolete. Can complete in any order.
        if (true) return false;
    }

    // Return TRUE if the data is in Review
    public static bool InReview(int registrationIdSelected, int registrationStep, int taskId, string currentTaskName, string commandName = "")
    {
        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) && (!Registration.CanUserEditRegistration(registrationIdSelected, currentTaskName, commandName))) return true;
        if (registrationIdSelected > 0) return true;
        // If it is a Return to Provider (Account Info) only the ACH, Substitute W9, and Agreements can be modified

        List<string> TaskIDs = new List<string>();
        TaskIDs.AddRange(Helper.GetAppSettingFromDB("RTPAccountInfoTaskIDs").Split(','));
        if (
            registrationStep != CON.RegistrationPageType.ApplicationFee &&
            registrationStep != CON.RegistrationPageType.Agreements &&
            TaskIDs.Find(itm => itm == taskId.ToString()) != null) return true;

        return false;
    }


    public static bool IsPRTFProvider(int regId)
    {
        bool prtfProvider = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(regId, "PROVIDER");
        if (Helper.HasRows(ds))
        {
            DataTable reg = ds.Tables[0];
            if (reg.Rows.Count > 0)
            {
                DataRow dr = reg.Rows[0];
                // Get provider type
                string PRTFProviderMMISTypeName = SelectAppSetting("PRTFProviderMMISTypeIDs");
                string providerTypeName = Helper.GetString("MMIS_Provider_Type_ID", dr);
                // 
                List<string> PRTFIDs = new List<string>();
                PRTFIDs.AddRange(PRTFProviderMMISTypeName.Split(','));
                if (PRTFIDs.Find(itm => itm == providerTypeName) != null)
                {
                    prtfProvider = true;
                }
                /*if (PRTFProviderMMISTypeName == providerTypeName)
                {
                    
                }*/
            }
        }
        return prtfProvider;
    }

    public static bool IsCDSPageRequired(int regId)
    {
        bool isRequired = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        isRequired  = svc.CheckCDSNumberSectionRequired(regId);
        return isRequired;
    }

    public static string GetMMIStProviderType(int regId)
    {
	    var providerType = string.Empty;
	    PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
	    DataSet ds = svc.SelectRegistrationData(regId, "PROVIDER");
	    if (Helper.HasRows(ds))
	    {
		    providerType = Helper.GetString("MMIS_Provider_Type_ID", ds.Tables[0].Rows[0]);
	    }
	    return providerType;
    }

    public static bool IsASARSProvider(int regId)
    {
        bool asarsProvider = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(regId, "PROVIDER");
        if (Helper.HasRows(ds))
        {
            DataTable reg = ds.Tables[0];
            if (reg.Rows.Count > 0)
            {
                DataRow dr = reg.Rows[0];
                // Get provider type
                string ASARSProviderMMISTypeName = SelectAppSetting("ASARSProviderMMISTypeIDs");
                string providerTypeName = Helper.GetString("MMIS_Provider_Type_ID", dr);
                // 


                List<string> ASARSIDs = new List<string>();
                ASARSIDs.AddRange(ASARSProviderMMISTypeName.Split(','));
                if (ASARSIDs.Find(itm => itm == providerTypeName) != null)
                {
                    asarsProvider = true;
                }
                /*if (PRTFProviderMMISTypeName == providerTypeName)
                {
                    
                }*/
            }
        }
        return asarsProvider;


    }
    public static bool IsMHRSProvider(int regId)
    {
        bool mhrsProvider = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(regId, "PROVIDER");
        if (Helper.HasRows(ds))
        {
            DataTable reg = ds.Tables[0];
            if (reg.Rows.Count > 0)
            {
                DataRow dr = reg.Rows[0];
                // Get provider type
                string MHRSProviderMMISTypeName = SelectAppSetting("MHRSProviderMMISTypeIDs");
                string providerTypeName = Helper.GetString("MMIS_Provider_Type_ID", dr);
                // 


                List<string> MHRSIDs = new List<string>();
                MHRSIDs.AddRange(MHRSProviderMMISTypeName.Split(','));
                if (MHRSIDs.Find(itm => itm == providerTypeName) != null)
                {
                    mhrsProvider = true;
                }
                /*if (PRTFProviderMMISTypeName == providerTypeName)
                {
                    
                }*/
            }
        }
        return mhrsProvider;


    }
    public static bool IsDMEProvider(int regId)
    {

        DataRow row = Registration.GetRegistration(regId);

        bool dmeProvider = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(regId, "PROVIDER");
        if (Helper.HasRows(ds))
        {
            DataTable reg = ds.Tables[0];
            if (reg.Rows.Count > 0)
            {
                DataRow dr = reg.Rows[0];
                // Get provider type
                //string DMEProviderTypeName = SelectAppSetting("DMEProviderTypeName");
                //string providerTypeName = Helper.GetString("ProviderTypeName", dr);

                string DMEProviderTypeID = SelectAppSetting("DMEProviderMMISTypeID");
                string providerTypeID = Helper.GetString("MMIS_Provider_Type_ID", dr);


                if (DMEProviderTypeID == providerTypeID)
                {
                    dmeProvider = true;
                }
            }
        }
        return dmeProvider;

        
    }

    public static bool IsPCAProvider(string providerTypeID)
    {

        bool PCAProvider = false;

        // Get provider type
        string PCAProviderTypeID = SelectAppSetting("PCAProviderMMISTypeID");

        if (PCAProviderTypeID == providerTypeID)
        {
            PCAProvider = true;
        }

        return PCAProvider;


    }
    public static bool IsLabProvider(int providerTypeID)
    {
        bool LabProvider = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        string LabProviderTypeID = SelectAppSetting("IndeendentLabProvider");
        if (LabProviderTypeID == GetMMISProviderTypeID(providerTypeID))
        {
            LabProvider = true;
        }

        return LabProvider;
    }

    public static bool IsPAProvider(int providerTypeID)
    {


        bool PAProvider = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        //DataSet ds = svc.SelectRegistrationByRegID(SessionVarRetriever.RegistrationId);

        string PAProviderTypeID = SelectAppSetting("PAProviderMMISTypeID");
        


        if (PAProviderTypeID == GetMMISProviderTypeID(providerTypeID))
        {
            PAProvider = true;
        }

        return PAProvider;


    }
    public static bool IsILProvider(int providerTypeID)
    {


        bool ILProvider = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        //DataSet ds = svc.SelectRegistrationByRegID(SessionVarRetriever.RegistrationId);

        string ILProviderTypeID = SelectAppSetting("ILProviderMMISTypeID");



        if (ILProviderTypeID == GetMMISProviderTypeID(providerTypeID))
        {
            ILProvider = true;
        }

        return ILProvider;


    }

    public static bool IsIDDDWaiverProvider(int regId)
    {

        DataRow row = Registration.GetRegistration(regId);

        bool idddWaiverProvider = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(regId, "PROVIDER");
        if (Helper.HasRows(ds))
        {
            DataTable reg = ds.Tables[0];
            if (reg.Rows.Count > 0)
            {
                DataRow dr = reg.Rows[0];
                // Get provider type
                string IDDDWaiverProviderTypeID = SelectAppSetting("IDDDWaiverProviderMMISTypeID");
                string providerTypeID = Helper.GetString("MMIS_Provider_Type_ID", dr);


                if (IDDDWaiverProviderTypeID == providerTypeID)
                {
                    idddWaiverProvider = true;
                }
            }
        }
        return idddWaiverProvider;


    }

    private static string SelectAppSetting(string appSettingKey)
    {
            return AppSettings.Get(appSettingKey, string.Empty);

    }

    // Return true if the Registration is in Provider data entry step
    // Return true if the Registration is in Provider review step
    // Return true if the registration is individual and being modified by group user when coming from affiliation update
    public static bool CanUserEditRegistration(int registrationIdSelected, string currentTaskName, string commandName = "")
    {
        if (Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name) && commandName == "ReviewRow")
        {
            bool isCurrentOwner = false;
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet ds = svc.SelectRegistrationByRegID(registrationIdSelected);
            if (Helper.HasRows(ds))
            {
                DataTable reg = ds.Tables[0];
                if (reg.Rows.Count > 0)
                {
                    DataRow dr = reg.Rows[0];
                    var currentStepOwner = Methods.GetStringValue(dr, "CurrentStepOwner");
                    if (currentStepOwner == HttpContext.Current.User.Identity.Name)
                    {
                        isCurrentOwner = true;
                    }
                }
            }
            if (isCurrentOwner)
                return true;
            else
                return false;
        }

        // Current logged in user is provider , current task is provider data entry and they didn't end up here as a result of provider search
        if (((Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) || Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name))
            && currentTaskName == CON.RegistrationTaskName.ProviderDataEntry
            && registrationIdSelected == 0) || // Current logged in user is either admin or operator, current task is provider review and they didn't end up here as a result of provider search
            ((Helper.IsLoggedInUserInAdminRole() || (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) && registrationIdSelected == 0))
            && currentTaskName == CON.RegistrationTaskName.ProviderReview))
        {
            return true;
        }
        if ((Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InternalApplicationsEntry)
            && currentTaskName == CON.RegistrationTaskName.ProviderDataEntry) || (Helper.IsLoggedInUserInAdminRole()))
        {
            return true;
        }
        if ((Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) && registrationIdSelected == 0 && SessionVarRetriever.IndividualRegID > 0 && SessionVarRetriever.GroupUserRegID > 0))
        {
            return true;
        }
        if (Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name) ||
            Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name))
        { 
            return true;
        }
        return false;
    }
   
    public static bool IsCurrentAssignedUserForTask(int regId, string userName, string taskName)
    {
        bool rtn = false;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectCurrentWFTaskInfo(regId);

        if (Helper.HasRows(ds))
        { 
            DataRow dr = ds.Tables[0].Rows[0];
            string currentUserName = Helper.GetString("UserName", dr);
            string currentTaskName = Helper.GetString("CurrentTaskName", dr);

            rtn = (userName == currentUserName && taskName == currentTaskName);
        }

        return rtn;
    }

    public static bool IsCurrentAssignedUserForStep(Guid userId, int stepId)
    {
        bool rtn = false;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
       // DataSet ds = psc.SelectWFStepInfo(stepId);
        DataSet ds = psc.VerifyCurrentAssignedUserForStep(userId, stepId);

        if (Helper.HasRows(ds))
        {
            //DataRow dr = ds.Tables[0].Rows[0];
            //string ownerId = Helper.GetString("OWNER_ID", dr);

            //rtn = (userId.ToString() == ownerId);
            rtn = true;
        }

        return rtn;
    }

    public static void SecurityQuestions(ref System.Web.UI.WebControls.DropDownList ddl)
    {
        //DataTable dt = new DataTable();
        //dt.Columns.Add(new DataColumn("ID", typeof(int)));
        //dt.Columns.Add(new DataColumn("TEXT", typeof(string)));
        //dt.Rows.Add(0, string.Empty);
        //dt.Rows.Add(1, "In what city did you meet your spouse / significant other?");
        //dt.Rows.Add(2, "What is your oldest sibling's birthday month and year?");
        //dt.Rows.Add(3, "What is your oldest siblings middle name?");
        //dt.Rows.Add(4, "What is your maternal grandmother's maiden name?");
        //dt.Rows.Add(5, "What is your mother's middle name?");
        //dt.Rows.Add(6, "What was the first concert you attended?");
        //dt.Rows.Add(7, "What was your High School mascot?");
        //dt.Rows.Add(8, "What year did you graduate from High School?");
        //dt.Rows.Add(9, "In what city were you born?");

        //ddl.DataSource = dt;
        //ddl.DataValueField = "ID";
        //ddl.DataTextField = "TEXT";
        //ddl.DataBind();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet dsSecQuest = psc.GetSecurityQuestions();
        if (Helper.HasRows(dsSecQuest))
        {
            Helper.LoadDropDown(ddl, dsSecQuest.Tables[0], "SECURITY_QUESTION_TYPE", "SECURITY_QUESTION_TYPE_ID", true);
        }
    }

    public static bool AgreementsIsValidated(int regId)
    {
        int nodeStatus;
        bool toReturn = false;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(regId, "SECTION_STATUS");

        if (Helper.HasRows(ds))
        {
            DataTable dt = ds.Tables[0];
            if (dt.Select("SectionName = 'Agreements'").Length > 0)
            {
                if (int.TryParse(dt.Select("SectionName = 'Agreements'").CopyToDataTable().Rows[0]["REG_PROVIDER_STATUS_TYPE_ID"].ToString(), out nodeStatus))
                {
                    toReturn = nodeStatus == CON.RegistrationProviderStatusTypeId.Complete;
                }
            }
        }

        return toReturn;
    }
    public static bool CMCSectionIsValidated(int regId, int pageint)
    {
        int nodeStatus;
        bool toReturn = false;

        //if (!StepExistsInNodes(pageint)) return true;       // If the step does not exist then validation is TRUE

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegSectionStatusCMC(regId, CON.WorkflowType.CMC, "SECTION_STATUS");

        if (!Helper.HasRows(ds))
        {
            return false;
        }

        // TODO: EDV Remove the .SElect and reate a new function to get the sections status from teh databse directly.
        DataTable dt = ds.Tables[0];
        if (dt.Select("REG_SECTION_TYPE_ID = '" + pageint + "'").Length > 0)
        {
            if (int.TryParse(dt.Select("REG_SECTION_TYPE_ID = '" + pageint + "'").CopyToDataTable().Rows[0]["REG_PROVIDER_STATUS_TYPE_ID"].ToString(), out nodeStatus))
            {
                toReturn = nodeStatus == CON.RegistrationProviderStatusTypeId.Complete;
            }
        }

        return toReturn;
    }
    public static bool SectionIsValidated(int regId, int pageint)
    {
        int nodeStatus;
        bool toReturn = false;

        //if (!StepExistsInNodes(pageint)) return true;       // If the step does not exist then validation is TRUE

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(regId, "SECTION_STATUS");

        if (!Helper.HasRows(ds))
        {
            return false;
        }

        // TODO: EDV Remove the .SElect and reate a new function to get the sections status from teh databse directly.
        DataTable dt = ds.Tables[0];
        if (dt.Select("REG_SECTION_TYPE_ID = '" + pageint + "'").Length > 0)
        {
            if (int.TryParse(dt.Select("REG_SECTION_TYPE_ID = '" + pageint + "'").CopyToDataTable().Rows[0]["REG_PROVIDER_STATUS_TYPE_ID"].ToString(), out nodeStatus))
            {
                toReturn = nodeStatus == CON.RegistrationProviderStatusTypeId.Complete;
            }
        }

        return toReturn;
    }
    public static bool PageIsValidated(int regId, int pageint)
    {
        int nodeStatus;
        bool toReturn = false;
        string pagestring = Registration.PageStringforStatusTable(pageint);
        if (string.IsNullOrEmpty(pagestring))
            return true;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(regId, "PAGE_STATUS");

        if (!Helper.HasRows(ds))
        {
            return false;
        }

        DataTable dt = ds.Tables[0];
        if (dt.Select("PageName = '" + pagestring + "'").Length > 0)
        {
            if (int.TryParse(dt.Select("PageName = '" + pagestring + "'").CopyToDataTable().Rows[0]["REG_PROVIDER_STATUS_TYPE_ID"].ToString(), out nodeStatus))
            {
                return nodeStatus == CON.RegistrationProviderStatusTypeId.Complete;
            }
        }

        return toReturn;
    }

    private static string PageStringforStatusTable(int pageint)
    {
        switch (pageint)
        {
            case CON.RegistrationPageType.ACHAuthorization:
                return "ACHAuthorization";
            case CON.RegistrationPageType.GroupAffiliations:
                return "GroupAffiliations";
            case CON.RegistrationPageType.Identification:
                return "Identification";
            case CON.RegistrationPageType.LicensesClassifications:
                return "LicensesClassifications";
            case CON.RegistrationPageType.OwnerInformation:
                return "OwnerInformation";
            case CON.RegistrationPageType.PracticeLocations:
                return "PracticeLocations";
            case CON.RegistrationPageType.ApplicationFee:
                return "ApplicationFee";
            case CON.RegistrationPageType.SubstituteW4Form:
                return "SubstituteW4Form";
            case CON.RegistrationPageType.HouseholdMembers:
                return "HouseholdMembers";
            case CON.RegistrationPageType.GroupAndFacilityAffiliation:
                return "Group And Facility Affiliation";
            default:
                return string.Empty;
        }
    }

    public static string GetStatus(string userId, int regId)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationStatuses(userId, regId);
        if (!Helper.HasRows(ds)) return string.Empty;
        return Helper.GetString("TennCare Status", ds.Tables[0].Rows[0]);
    }

    public static bool CheckRegistrationComplete(int regId, Dictionary<int, RegistrationNode> registrationNodes, string currentTaskName, int workflowEventType, ref bool isRegistrationComplete, int workflowID, bool IsAddODMorODAMedSvc, bool IsReactivation, bool isRevertSuspensionWF)
    {
        bool approved = false;
        DataSet ds;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        if (workflowID == CON.WorkflowType.CMC)
        {
            ds = svc.SelectRegSectionStatusCMC(regId, workflowID, "SECTION_STATUS");
        }
        else
        {
            ds = svc.SelectRegistrationData(regId, "SECTION_STATUS");
        }
        //Check only visible sections not all sections for application complete
        DataView dvSections = ds.Tables[0].DefaultView;
        dvSections.RowFilter = "IS_VISIBLE = 1";
        DataTable dt = dvSections.ToTable();
        isRegistrationComplete = false;
        //int modifiedCnt = 0;
        //int rtpCnt = 0;
        string enableCR313 = AppSettings.Get("EnableCR313");
        if (Helper.HasRows(dt))
        {
            
           // int cnt = 0, apr = 0, reqCnt = 0;
            
            int completedCount = 0, approvedCount = 0, completedRequiredCount = 0, modifiedCount = 0, returnedCount = 0, isrequiredNotApproved=0,isrequiredReview=0;
            bool isProvider = false;

            if (Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name) || (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCCHOP) && workflowID == CON.WorkflowType.CHOP))
            {
                //If Operator or Admin and not in Provider Data Entry
                completedCount = dt.AsEnumerable()
                                    .Where(x => registrationNodes.ContainsKey(x["REG_SECTION_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_SECTION_TYPE_ID"]) : -999)  //only nodes shown in registration nodes
                                            && (x["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"]) : -999) != CON.RegistrationProviderServicesStatusTypeId.Pending)
                                    .Count();

                approvedCount = dt.AsEnumerable()
                                   .Where(x => registrationNodes.ContainsKey(x["REG_SECTION_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_SECTION_TYPE_ID"]) : -999)
                                       && (x["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"]) : -999) == CON.RegistrationProviderServicesStatusTypeId.Approved)
                                   .Count();

                isrequiredNotApproved = dt.AsEnumerable()
                                   .Where(
                                   x => (x["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"] == DBNull.Value)
                                       && (x["IS_REQUIRED"] != DBNull.Value ? Convert.ToInt32(x["IS_REQUIRED"]) : -999) == 1

                                   )
                                   .Count();

                if (workflowEventType == CON.WorkflowEventType.UpdateReg && !IsAddODMorODAMedSvc)
                {
                    isrequiredReview = dt.AsEnumerable()
                                            .Where(x => (x["REG_PROVIDER_STATUS_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_PROVIDER_STATUS_TYPE_ID"]) : -999) == CON.RegistrationProviderStatusTypeId.Modified)
                                            .Count();
                }
            }
            else if ((Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) && currentTaskName != CON.RegistrationTaskName.ProviderDataEntry)) 
            {
                completedCount = dt.AsEnumerable()
                                    .Where(x => registrationNodes.ContainsKey(x["REG_SECTION_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_SECTION_TYPE_ID"]) : -999)  //only nodes shown in registration nodes
                                            && (x["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"]) : -999) != CON.RegistrationProviderServicesStatusTypeId.Pending)
                                    .Count();

                approvedCount = dt.AsEnumerable()
                                   .Where(x => registrationNodes.ContainsKey(x["REG_SECTION_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_SECTION_TYPE_ID"]) : -999)
                                       && (x["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"]) : -999) == CON.RegistrationProviderServicesStatusTypeId.Approved)
                                   .Count();

                isrequiredNotApproved = dt.AsEnumerable()
                                   .Where(
                                   x => (x["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"] == DBNull.Value)
                                       && (x["IS_REQUIRED"] != DBNull.Value ? Convert.ToInt32(x["IS_REQUIRED"]) : -999) == 1

                                   )
                                   .Count();

                returnedCount = dt.AsEnumerable()
                                           .Where(x => (x["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"]) : -999) == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider)
                                           .Count();

                if (workflowEventType == CON.WorkflowEventType.UpdateReg && !IsAddODMorODAMedSvc)
                {
                    isrequiredReview = dt.AsEnumerable()
                                            .Where(x => (x["REG_PROVIDER_STATUS_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_PROVIDER_STATUS_TYPE_ID"]) : -999) == CON.RegistrationProviderStatusTypeId.Modified)
                                            .Count();
                    modifiedCount = dt.AsEnumerable()
                                            .Where(x => (x["REG_PROVIDER_STATUS_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_PROVIDER_STATUS_TYPE_ID"]) : -999) == CON.RegistrationProviderStatusTypeId.Modified)
                                            .Count();
											
                    // If in Update then isrequiredNotApproved = ModifiedCount since we only need to check the modifed paged 
                    isrequiredNotApproved = modifiedCount;
                }
            }
            else
            {  //if provider
                isProvider = true;
                completedCount = dt.AsEnumerable()
                                    .Where(x => (x["REG_PROVIDER_STATUS_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_PROVIDER_STATUS_TYPE_ID"]) : -999) == CON.RegistrationProviderStatusTypeId.Complete)
                                    .Count();

                completedRequiredCount = dt.AsEnumerable()
                                            .Where(x => (x["REG_PROVIDER_STATUS_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_PROVIDER_STATUS_TYPE_ID"]) : -999) == CON.RegistrationProviderStatusTypeId.Complete
                                                     && (x["IS_REQUIRED"] != DBNull.Value ? Convert.ToInt32(x["IS_REQUIRED"]) : -999) == 1)
                                            .Count();

                if (workflowEventType == CON.WorkflowEventType.UpdateReg && !IsAddODMorODAMedSvc)
                    modifiedCount = dt.AsEnumerable()
                                            .Where(x => (x["REG_PROVIDER_STATUS_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_PROVIDER_STATUS_TYPE_ID"]) : -999) == CON.RegistrationProviderStatusTypeId.Modified)
                                            .Count();


                returnedCount = dt.AsEnumerable()
                                            .Where(x => (x["REG_PROVIDER_STATUS_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_PROVIDER_STATUS_TYPE_ID"]) : -999) == CON.RegistrationProviderStatusTypeId.NotComplete
                                                       && (x["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"]) : -999) == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider)
                                            .Count();
            }

             

            // 09/16/2014 MHH - Changed to use the total node count to determine if the page is finished or not. The node count
            //                  equals the number of nodes that are displayed in the left bottom navigation.
            int requiredNodes = registrationNodes.Where(s => s.Value.IsRequired == 1).Count();
            
            int node_cnt = registrationNodes.Where(s => s.Value.Step != 99 && s.Value.Step != 101 && s.Value.Step != 69 && s.Value.Step != 72).Count(); // 72 for resticted services//99 == Workflow Step  // JIRA 3276: 101 == Transaction Queue //69 == Contract Maintenance

            if (currentTaskName == CON.RegistrationTaskName.ProviderReview)
            {
                node_cnt = registrationNodes.Where(s => s.Value.ExcludeSectionFromProvReviewCount == 0).Count();
                //node_cnt = registrationNodes.Where(s => s.Value.Step != 99 && s.Value.Step != 101 && s.Value.Step != 69 && s.Value.Step != 68 && s.Value.Step != 72 && s.Value.Step != 61 && s.Value.Step != 10000 && s.Value.Step != 66 && s.Value.Step != 67).Count();
            }
            // TODO: EDV this logic needs to account for application fee, workfow steps etc. This logic should get better. Dont depend on magic numbers

            if (workflowID == 18) //TODO faking for credentialing process
            {
                if (completedCount >= requiredNodes - 1)
                {
                    isRegistrationComplete = true;
                    if (currentTaskName != CON.RegistrationTaskName.ProviderDataEntry)
                        approved = true;
                }
            }
            else if (completedCount > 0 && registrationNodes.Count > 0 && node_cnt >0 &&
                (completedCount >= node_cnt || completedRequiredCount >= requiredNodes) && isrequiredNotApproved==0 && returnedCount == 0)
            { 
                isRegistrationComplete = true;

                if (approvedCount >= node_cnt ) 
                    approved = true;
            }
            
            else if (dt.Select("REG_PROVIDER_SERVICES_STATUS_TYPE_ID IS NULL").Length <= 0)
            {
                approved = false;
                if(currentTaskName != CON.RegistrationTaskName.ProviderDataEntry)
                    isRegistrationComplete = true;
            }

            if (isProvider)
            {
                //Check if Datatable has all the required forms from the registrationNodes
                var allStepsComplete = true;
                var regNodes = registrationNodes.Where(s => s.Value.IsRequired == 1).ToList();
                foreach (var rn in regNodes)
                {
                    DataRow dataRow = dt.AsEnumerable().FirstOrDefault(r => Convert.ToString(r["DisplayName"]).Contains(rn.Value.MenuPath) &&
                        r["REG_PROVIDER_STATUS_TYPE_ID"] != DBNull.Value
                        && Convert.ToInt16(r["REG_PROVIDER_STATUS_TYPE_ID"]) == CON.RegistrationProviderStatusTypeId.Complete);
                    if (dataRow == null)
                    {
                        allStepsComplete = false;
                        isRegistrationComplete = false;
                    }
                }
                if (allStepsComplete)
                    isRegistrationComplete = true;

                // SAM537
                if(isRevertSuspensionWF && completedCount > 0 && returnedCount == 0)
                    isRegistrationComplete = true;
            }

            //if (completedRequiredCount >= requiredNodes && returnedCount == 0)
            //    isRegistrationComplete = true;

            // OHPNM-10264 - if this is a reactivation in review, there aren't pages to approve except the NPIandMedID page; if that page hasn't been approved, then disable the approve button
            if (workflowEventType == CON.WorkflowEventType.UpdateReg && IsReactivation && currentTaskName == CON.RegistrationTaskName.ProviderReview && !IsAddODMorODAMedSvc && enableCR313 == "true")
            {
                if (completedCount > 0 && registrationNodes.Count > 0 && node_cnt > 0 &&
                completedCount >= node_cnt && isrequiredNotApproved == 0 && returnedCount == 0)
                {
                    isRegistrationComplete = true;

                    if (approvedCount >= node_cnt)
                        approved = true;
                }
            }
            else if (workflowEventType == CON.WorkflowEventType.UpdateReg && approvedCount == dt.Rows.Count && isrequiredNotApproved == 0)
            {
                approved = true;
                isRegistrationComplete = true;
            }
            else if (workflowEventType == CON.WorkflowEventType.UpdateReg && (isrequiredReview == 0 && isrequiredReview <= approvedCount) && currentTaskName == CON.RegistrationTaskName.ProviderReview && !IsAddODMorODAMedSvc)
            {
                isRegistrationComplete = true;
                if (isrequiredNotApproved == 0)
                {
                    approved = true;
                }
            }
            else if (approvedCount == dt.Rows.Count && returnedCount == 0 && currentTaskName == CON.RegistrationTaskName.ProviderReview 
                     && (workflowEventType != CON.WorkflowEventType.UpdateReg && !IsReactivation))
            {
                //this can be optimized TO do Item for later-dn't want create new bugs at this moment so adding one more else if
                approved = true;
                isRegistrationComplete = true;
            }
            else if ((approvedCount== dt.Rows.Count-1)&& (CON.sectionName=="Agreements"))
            {
                approved = true;
            }
            else if (workflowEventType == CON.WorkflowEventType.Reconsideration && currentTaskName == CON.RegistrationTaskName.ProviderReview)
            {
                int rowsCount = dt.AsEnumerable().Where(x => registrationNodes.ContainsKey(x["REG_SECTION_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_SECTION_TYPE_ID"]) : -999)
                                && (x["REG_SECTION_TYPE_ID"] != DBNull.Value ? Convert.ToInt32(x["REG_SECTION_TYPE_ID"]) : -999) != CON.SectionTypeID.RECONSIDERATION)
                                .Count();

                if (approvedCount >= rowsCount && returnedCount == 0)
                {
                    approved = true;
                    isRegistrationComplete = true;
                }
            }
            else if (workflowID == 27 && currentTaskName == CON.RegistrationTaskName.ProviderReview) //There is no Provider Review for CMC
            {
                isRegistrationComplete = true;
                approved = true;
            }

            if (modifiedCount > 0 && returnedCount == 0)
                    isRegistrationComplete = true;

            // RG - CR313 changes commenting as everything above should correctly show the approve/ApplicationComplete button for Provider Review
            //if ((node_cnt - 1) == approvedCount && isrequiredNotApproved == 0 && returnedCount == 0 && workflowID != CON.WorkflowType.CPC)
            //    approved = true;
        }
        
        return approved;
    }

    public static bool IsPageApproved(int regId, int stepId)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(regId, "PAGE_STATUS");
        if (!Helper.HasRows(ds)) return false;
        bool rtn = false;
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            if (Helper.GetInt("REG_PAGE_TYPE_ID", row) == stepId)
            {
                if (Helper.GetInt("REG_PROVIDER_SERVICES_STATUS_TYPE_ID", row) ==
                    CON.RegistrationProviderServicesStatusTypeId.Approved) rtn = true;
                break;
            }
        }
        return rtn;
    }

    public static bool IsSectionApproved(int regId, int stepId)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(regId, "SECTION_STATUS");
        if (!Helper.HasRows(ds)) return false;
        bool rtn = false;
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            if (Helper.GetInt("REG_SECTION_TYPE_ID", row) == stepId)
            {
                if (Helper.GetInt("REG_PROVIDER_SERVICES_STATUS_TYPE_ID", row) ==
                    CON.RegistrationProviderServicesStatusTypeId.Approved) rtn = true;
                break;
            }
        }
        return rtn;
    }
    public static bool IsConversionProvider(int programStatusTypeID)
    {
        //above session var is not always set at this time - need to do an 
            //overall look at what key fields we can grab at load of registration to avoid hitting the db for them every time we open a page.
        return programStatusTypeID == CON.RegistrationProgramStatusTypeId.Conversion;
    }

    public static bool IsValidNppesNpi(int regId, ref string errorMessage)
    {
        /*
        Category 1: Individual - if the NPI exists on NPPES database then it must be Entity_Type = 1 NPI
        Category 2: Group/Institution - if the NPI exists on NPPES database then it must be Entity_Type = 2 NPI
        Category 3: Facility - if the NPI exists on NPPES database then it must be Entity_Type = 2 NPI
        Category 5: Pharmacy - if the NPI exists on NPPES database then it must be Entity_Type = 2 NPI
        Category 6: (Group Member Profiles) - if the NPI exists on NPPES database then it must be Entity Type = 1 NPI
         * */
    
        errorMessage = string.Empty;
        //This function belongs in here because it is used at the page level for Provider validation and for validation at Provider Services
        bool isProvider = Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name);
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet dsProv = svc.SelectRegistrationData(regId, "PROVIDER");

        if (!Helper.HasRows(dsProv))
        {
            errorMessage = "Provider has no registration. REG_ID = " + regId;
            return false;
        }

        string npi = Helper.GetString("NPI", dsProv.Tables[0].Rows[0]);
        bool isNursingFacility = Helper.GetString("PROVIDER_TYPE_NAME", dsProv.Tables[0].Rows[0]).Trim() == CON.ProviderType.NURSING_FACILITY;

        if (string.IsNullOrEmpty(npi) && !isProvider)
        {
            errorMessage = string.Empty;
            return true; //Do not throw this error because it is the wrong error for this issue.
        }

        // OHPNM-9469 - JLB ensure NPI is not already associated with an active REG_ID
        int workflowEventTypeID = Helper.GetInt("WORKFLOW_EVENT_TYPE_ID", dsProv.Tables[0].Rows[0]);
        bool isNPIUnique = svc.ValidateNPIUniqueness(regId, npi, isNursingFacility);
        if (!isNPIUnique && workflowEventTypeID != CON.WorkflowEventType.ChangeProviderType)
        {
            errorMessage = "The NPI is already active with another registration or is currently being processed.If you have questions, please contact the Integrated Help Desk at 1 - 800 - 686 - 1516, Option 2, Option 2'";
            return false;
        }

        DataSet dsNPPES = svc.Search_NPPES_EntityType(npi);

        int npiValue;
        int.TryParse(npi.Trim(), out npiValue);

        MAXIMUS.Core.Libraries.NPPESAPIResult result = svc.ValidNPIinNPPESApi(npiValue);       

        if (!Helper.HasRows(dsNPPES) && result.result_count==0)
        {
            errorMessage = isProvider ? string.Empty : "Our records indicate the NPI entered is not in our NPI database.";
            return isProvider; //If provider, it is ok that NPPES does not have NPI yet - don't throw error. Throw error if not provider.
        }

        //for group member profile and individual NPI should be type1 - bug5398
        DataRow row = dsProv.Tables[0].Rows[0];
        int entityTypeID = Helper.GetInt("ENTITY_TYPE_ID", row);
        int nppesTypeID = 0;
        if (result.result_count > 0)
        {
            nppesTypeID = Convert.ToInt32(result.results[0].enumeration_type.Split('-')[1]);
        }
       
        if (entityTypeID == CON.ProviderCategoryTypeID.Individual || entityTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile)
        {
            if (nppesTypeID != 1)
            {
                errorMessage = "The NPI entered must be a Type 1 NPI.";
                return false; //entity type must be 1
            }
        }
        else
        {
            if (nppesTypeID != 2)
            {
                errorMessage = "The NPI entered must be a Type 2 NPI.";
                return false; //entity type must be 2
            }
        }
        return true;
    }

    // Mark the Registration errors as closed
    public static void MarkErrorsAsClosed(int regId, int pageTypeId, int stepId)
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            DataSet ds = psc.SelectREG_ERRORcustom(regId, pageTypeId, stepId, false, true);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    psc.UpdateErrorRegistration(Helper.GetInt("REG_ERROR_ID", row), CON.ErrorStatusType.Closed, null, null, null,
                        Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }
                if (ds.Tables.Count > 1)                // We need to close the PARTY_ERRORs as closed
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        psc.UpdateError(Helper.GetInt("ERROR_ID", row), CON.ErrorStatusType.Closed, null,
                            CON.ResolvingActionType.UpdateRecord, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    }
                }
            }
        }
    }

    public static bool SectionIsVisible(int regId, int entityTypeId, int providerTypeId, int diddReferralId, string pageName, string pageSection, string taskName = "")
    {
        // These pages are only visible if you have a DIDD Referral Id
        

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataSet ds1 = psc.SelectDIDDReferralService(diddReferralId);
        
        int applicationTypeID = 0;
        //int diddserviceid = 0;


        ds1 = psc.SelectRegistrationData(regId, "PROVIDER");
        if (Helper.HasRows(ds1))
        {
            applicationTypeID = Helper.GetInt("APPLICATION_TYPE_ID", ds1.Tables[0].Rows[0]);
        }

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("ENTITY_TYPE_ID", entityTypeId.ToString());
        if (providerTypeId > 0) parms.Add("PROVIDER_TYPE_ID", providerTypeId.ToString());
        parms.Add("REG_PAGE_NAME", pageName);
        if (!string.IsNullOrEmpty(pageSection)) parms.Add("REG_PAGE_SECTION", pageSection);
        if (!string.IsNullOrEmpty(taskName)) parms.Add("TASK_NAME", taskName);
        if (applicationTypeID > 0) parms.Add("APPLICATION_TYPE_ID", applicationTypeID.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_PAGE_SECTION_SETTING", parms);
        // If there is not an entry in the table then there is no exclusion and thereby Page is visible
        if (!Helper.HasRows(ds)) return true;

        return Helper.GetBool("IS_VISIBLE", ds.Tables[0].Rows[0]);
    }

    // Return true if the page or page section is visible
    public static bool PageIsVisible(int regId, int entityTypeId, int providerTypeId, int diddReferralId, string pageName, string pageSection, bool isWaiverProvider, string taskName = "")
    {
        // These pages are only visible if you have a DIDD Referral Id
        if (isWaiverProvider && (pageName == "Services" || pageName == "Contracts")) return true;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataSet ds1 = psc.SelectDIDDReferralService(diddReferralId);
        int waivertypeCode = 0;
        int applicationTypeID = 0;
        //int diddserviceid = 0;
        int[] waiverTypes = new int[] { 5665, 1691, 2500, 6700, 4456, 4475, 1113, 9233, 2456 };
        int[] substituteW9FiscalYearEndSpecialties = new int[] {66, 69,70 ,92};
        if (isWaiverProvider && (pageName.Contains("Substitute W4") || (pageName.Contains("Substitute W9") && !pageSection.Contains("FiscalYearEnd"))))
        {
            if (Helper.HasRows(ds1))
            {
                int[] diddServices = new int[ds1.Tables[0].Rows.Count];
                int counter = 0;
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    diddServices[counter] = Helper.GetInt("DIDD_SERVICE_ID", dr);
                    counter++;
                }
                //diddserviceid = Helper.GetInt("DIDD_SERVICE_ID", ds1.Tables[0].Rows[0]);

                if (diddServices.Length != 0)
                {
                    foreach (int diddserviceid in diddServices)
                    {
                        ds1 = psc.SelectDIDDServicesByDIDD_Service_ID(diddserviceid);
                        waivertypeCode = Helper.GetInt("WAIVER_TYPE_CODE", ds1.Tables[0].Rows[0]);

                        if (Array.IndexOf(waiverTypes, waivertypeCode) >= 0)
                        {
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }

        ds1 = psc.SelectRegistrationData(regId, "PROVIDER");
        if (Helper.HasRows(ds1))
        {
            applicationTypeID = Helper.GetInt("APPLICATION_TYPE_ID", ds1.Tables[0].Rows[0]);
        }

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("ENTITY_TYPE_ID", entityTypeId.ToString());
        if (providerTypeId > 0) parms.Add("PROVIDER_TYPE_ID", providerTypeId.ToString());
        parms.Add("REG_PAGE_NAME", pageName);
        if (!string.IsNullOrEmpty(pageSection)) parms.Add("REG_PAGE_SECTION", pageSection);
        if (!string.IsNullOrEmpty(taskName)) parms.Add("TASK_NAME", taskName);
        if (applicationTypeID > 0) parms.Add("APPLICATION_TYPE_ID", applicationTypeID.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_PAGE_SETTING", parms);
        // If there is not an entry in the table then there is no exclusion and thereby Page is visible
        if (!Helper.HasRows(ds)) return true;

        return Helper.GetBool("IS_VISIBLE", ds.Tables[0].Rows[0]);
    }

    public static bool PageIsVisible(int regId, string pageName, bool isWaiverProvider)
    {
        int EntityTypeId = 0;
        int ProviderTypeId = 0;
        int DIDDReferralId = 0;
        int SpecialtyTypeID = 0;
        string TaxID = string.Empty;
        int TaxIDTypeID = 0;

        SetEntityProviderTypesDIDD(regId, ref EntityTypeId, ref ProviderTypeId, ref DIDDReferralId, ref SpecialtyTypeID, ref TaxID, ref TaxIDTypeID);
        return PageIsVisible(regId, EntityTypeId, ProviderTypeId, DIDDReferralId, pageName, string.Empty, isWaiverProvider, TaxID);
    }


    public static bool EntryIsRequired(int regId, string pageName, string sectionName)
    {
        int EntityTypeId = 0;
        int ProviderTypeId = 0;
        int DIDDReferralId = 0;
        int SpecialtyTypeID = 0;
        string TaxID = string.Empty;
        int TaxIDTypeID = 0;

        SetEntityProviderTypesDIDD(regId, ref EntityTypeId, ref ProviderTypeId, ref DIDDReferralId, ref SpecialtyTypeID, ref TaxID, ref TaxIDTypeID);
        return EntryIsRequired(regId, EntityTypeId, ProviderTypeId, DIDDReferralId, pageName, sectionName);
    }

    private static bool EntryIsRequired(int regId, int entityTypeId, int providerTypeId, int diddReferralId, string pageName, string pageSection)
    {
        int applicationTypeID = 0;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();

        DataSet ds = psc.SelectRegistrationData(regId, "PROVIDER");
        if (Helper.HasRows(ds))
        {
            applicationTypeID = Helper.GetInt("APPLICATION_TYPE_ID", ds.Tables[0].Rows[0]);
        }

        parms.Add("ENTITY_TYPE_ID", entityTypeId.ToString());

        if (providerTypeId > 0)
        {
            parms.Add("PROVIDER_TYPE_ID", providerTypeId.ToString());
        }
        parms.Add("REG_PAGE_NAME", pageName);

        if (!string.IsNullOrEmpty(pageSection))
        {
            parms.Add("REG_PAGE_SECTION", pageSection);
        }

        if (applicationTypeID > 0)
        {
            parms.Add("APPLICATION_TYPE_ID", applicationTypeID.ToString());
        }
        ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_PAGE_SETTING", parms);

        // If there is not an entry in the table then there is no exclusion and thereby Page is visible and entry is required
        if (!Helper.HasRows(ds))
        {
            return true;
        }

        return Helper.GetBool("IS_REQUIRED", ds.Tables[0].Rows[0]);
    }


    public static void SetEntityProviderTypesDIDD(int regId, ref int EntityTypeId, ref int ProviderTypeId, ref int DIDDReferralId, ref int SpecialtyTypeID, ref string TaxID, ref int TaxIDTypeID)
    {
        if (EntityTypeId > 0 && ProviderTypeId > 0) return;
        DataRow dr = Registration.GetRegistration(regId);

        //TODO:  should not need to reget these fields.  If dr is null, should be a fatal error.
        if (dr == null) return;

        EntityTypeId = Helper.GetInt("ENTITY_TYPE_ID", dr);
        ProviderTypeId = Helper.GetInt("PROVIDER_TYPE_ID", dr);
        DIDDReferralId = Helper.GetInt("DIDD_REFERRAL_ID", dr);
        SpecialtyTypeID = Helper.GetInt("SPECIALTY_TYPE_ID", dr);
        TaxID = Helper.GetString("TAX_ID", dr);
        TaxIDTypeID = Helper.GetInt("TAX_ID_TYPE_ID", dr);
    }

    public static bool RequireNPI(int providerTypeId)
    {
        bool rtn = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetProviderTypeById(providerTypeId);
        if (Helper.HasRows(ds))
        {
            if (Helper.GetInt("REQUIRE_NPI", ds.Tables[0].Rows[0]) > 0) rtn = true;
        }
        return rtn;
    }

    // Get the page action setting for the user
    public static DataRow GetTakeActionRow(string pageName, int taskID)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_PAGE_NAME", pageName);
        parms.Add("TASK_ID", taskID.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectWF_TASK_PAGE_ACTION", parms);
        if (!Helper.HasRows(ds)) return null;
        return ds.Tables[0].Rows[0];
    }

    public static DataSet GetSignatureHistoryByRegID(int regID)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectSignatureHistoryByRegID(regID);
        return ds;
    }

    public static DataSet GetContractHistoryByRegID(int regID)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectContractHistoryByRegID(regID);
        return ds;
    }
    
    public static DataSet GetCurrentContractInformation(int regID)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectCurrentContractInfoByRegID(regID);
        return ds;
    }

    public static string GenerateDocument(int workflowID, int regID, out string fileName)
    {
        fileName = string.Empty;
        Guid logThreadId = Guid.NewGuid();
        string logMsg = String.Format("{0}::{1}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
        Logging log = new Logging(logThreadId, logMsg);

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        //TODO: THis is not used in NE. we have to get the code for TN and merge here so we download the contract

        //Need to generate the document then show it.
        bool result = false;
        switch (workflowID)
        {
                //need to modify to return filename
            case CON.WorkflowType.RegistrationDIDDReferral:
                result = svc.GenerateDIDDContract(regID, out fileName);
                break;
            case CON.WorkflowType.RegistrationNew:
                result = svc.GenerateICFIIDContract(regID, out fileName);
                break;
            default:
                break; 
        }

        return fileName;
    }

    public static bool InsertAccountCreationSpecialtyTaxonomy(int regId, int taxonomyTypeID, int specialtyTypeID)
    {
        int regTaxonomyId = 0;

        //todo:  make sure do not already have a primary.
        try
        {
            // Update the Taxonomy
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", regId.ToString());
            parms.Add("TAXONOMY_TYPE_ID", taxonomyTypeID.ToString());
            parms.Add("PRIMARY_FLAG", "1");
            parms.Add("START_DATE", DateTime.Now.ToShortDateString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            regTaxonomyId = psc.InsertRegistrationData(regId, "TAXONOMYCustom", parms);

            // Update the Specialty
            parms = new Dictionary<string, string>();
            parms.Add("REG_ID", regId.ToString());
            parms.Add("PRIMARY_FLAG", "1");
            parms.Add("SPECIALTY_TYPE_ID", specialtyTypeID.ToString());
            parms.Add("SPECIALTY_BOARD_CERTIFIED", "Y");
            parms.Add("START_DATE", DateTime.Now.ToShortDateString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            parms.Add("REG_TAXONOMY_ID", regTaxonomyId.ToString());
            psc.InsertRegistrationData(regId, "SPECIALTYCustom2", parms);
            return true;
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
        }
        return false;
    }

    // Return TRUE if the DIDD Registration has been submitted
    public static bool DIDDHasBeenSubmitted(int refId)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetRegistrationStatusForDIDDReferral(refId);
        if (!Helper.HasRows(ds)) return false;
        bool rtn = false;
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            if (Helper.GetString("RegistrationStatusType", row) != "Not Submitted")
            {
                rtn = true;
                break;
            }
        }
        return rtn;
    }

    public static DataSet GetDIDDRegistrationStatuses(int referralID)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetRegistrationStatusForDIDDReferral(referralID);
        return ds;
    }

    public static DataSet GetScreeningStatuses(int regId, int? providerScreeningID = null)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectScreeningStatus(providerScreeningID != null ? providerScreeningID.Value: -1,
            regId > 0 ? regId : -1);
        return ds;
    }

    public static bool ScreeningComplete(int regId, string screeningFor, int providerScreeningID = -1)
    {
        //screeningFor:  Group, ActiveAffiliation, AllAffiliations, Owner, Site Visit
        bool isComplete = false;
        DataSet dsActivities = providerScreeningID == -1 ? GetScreeningStatuses(regId, null) : GetScreeningStatuses(regId, providerScreeningID);

        if (Helper.HasRows(dsActivities))
        {
            DataTable Screenings = dsActivities.Tables[0];
            if (Screenings.Rows.Count == 0)
            {
                return true;
            }

            if (screeningFor == string.Empty)
            {
                var recsA = (from screening in Screenings.AsEnumerable()
                             where screening.Field<Int32>("SCREENING_STATUS_ID") == 1
                           && screening.Field<string>("SCREENING_TYPE") != CON.ScreeningFor.SiteVist  //06/03/2014 TM:  must exclude this for primary screening, but not for site visits.
                             select screening);

                if (recsA.AsDataView().Count == 0)
                {
                    isComplete = true;
                }
            } 
            else if (screeningFor == CON.ScreeningFor.ActiveAffiliation)
            {
                var recsA = (from screening in Screenings.AsEnumerable()
                        where screening.Field<Int32>("SCREENING_STATUS_ID") == 1
                            // TODO: EDV No Affiliation Screening so, cannot test it
                           //&& screening.Field<Int32>("SCREENING_ID") == SessionVarRetriever.ActiveScreeningID
                           && screening.Field<string>("SCREENING_TYPE") == CON.ScreeningFor.Affiliation
                        select screening);

                if (recsA.AsDataView().Count == 0)
                {
                    isComplete = true;
                }
            }
            else
            {
                var recs = (from screening in Screenings.AsEnumerable()
                        where screening.Field<Int32>("SCREENING_STATUS_ID") == 1
                           && screening.Field<string>("SCREENING_TYPE") == screeningFor
                        select screening);

                if (recs.AsDataView().Count == 0)
                {
                    isComplete = true;
                }
            }

        }

        return isComplete;
    }

    public static bool ScreeningResultsComplete(DataSet dsActivities)
    {
        bool isComplete = false;
        if (Helper.HasRows(dsActivities))
        {
            DataTable Screenings = dsActivities.Tables[0];
            if (Screenings.Rows.Count == 0)
            {
                return true;
            }

            var recs = (from screening in Screenings.AsEnumerable()
                        where screening.Field<Int32>("SCREENING_RESULT_ID") == 1
                        select screening);

            if (recs.AsDataView().Count == 0)
            {
                isComplete = true;
            }
        }

        return isComplete;
    }

    public static bool ScreeningIncludesSiteVisit(DataSet ds)
	{
		bool hasSiteVisit = false;
		if (Helper.HasRows(ds))
		{
			DataTable Screenings = ds.Tables[0];
			if (Screenings.Rows.Count == 0)
			{
				return false;
			}

			Screenings.DefaultView.RowFilter = string.Format("SCREENING_TYPE = '{0}'", CON.ScreeningFor.SiteVist);

			if (Screenings.DefaultView.Count > 0)
			{
				hasSiteVisit = true;
			}
		}

		return hasSiteVisit;
	}

    public static int SiteVisitComplete(int regId)
	{

        int isComplete = CON.ScreeningStatusId.InProgress;

        DataSet dsActivities = Registration.GetScreeningStatuses(regId);
        if (Helper.HasRows(dsActivities))
		{
			DataTable Screenings = dsActivities.Tables[0];
			DataView dv = Screenings.DefaultView;
            DataView dvfailed = Screenings.DefaultView;
            string screeningStatusID = string.Empty;
			//dv.RowFilter = string.Format("SCREENING_TYPE = '{0}' AND SCREENING_STATUS_ID = {1} AND SCREENING_RESULT_ID = {2}", CON.ScreeningFor.SiteVist, CON.ScreeningStatusId.Complete,CON.ScreeningResultId.Approved);
            //dvfailed.RowFilter = string.Format("SCREENING_TYPE = '{0}' AND SCREENING_STATUS_ID = {1} AND SCREENING_RESULT_ID = {2}", CON.ScreeningFor.SiteVist, CON.ScreeningStatusId.Failed, CON.ScreeningResultId.Approved);
			foreach (DataRow dr in Screenings.Rows)
            {
                if (dr["SCREENING_TYPE"].ToString() == CON.ScreeningFor.Provider)
                {
                    screeningStatusID = dr["SCREENING_STATUS_ID"].ToString();
                }
                if (dr["SCREENING_TYPE"].ToString() == CON.ScreeningFor.SiteVist)
                {
                    if (dr["SCREENING_STATUS_ID"].ToString() == CON.ScreeningStatusId.Complete.ToString() && 
                        (screeningStatusID == CON.ScreeningResultId.Approved.ToString() || dr["SCREENING_RESULT_ID"].ToString() == CON.ScreeningResultId.Approved.ToString()))
                    {
                        isComplete = CON.ScreeningStatusId.Complete;
                    }
                    if (dr["SCREENING_STATUS_ID"].ToString() == CON.ScreeningStatusId.Failed.ToString() && dr["SCREENING_RESULT_ID"].ToString() == CON.ScreeningResultId.Approved.ToString())
                    {
                        isComplete = CON.ScreeningStatusId.Failed;
                    }
                }
            }
            /*if (dv.Count == 1)
			{
                isComplete = CON.ScreeningStatusId.Complete;
			}
            if (dvfailed.Count == 1)
            {
                isComplete = CON.ScreeningStatusId.Failed;
            }*/
		}

		return isComplete;
	}

    public static bool ScreeningComplete(int regId)
    {
        //All screenings are complete
        bool isComplete = false;
        DataSet dsActivities = GetScreeningStatuses(regId);

        if (Helper.HasRows(dsActivities))
        {
            DataTable Screenings = dsActivities.Tables[0];
            if (Screenings.Rows.Count == 0)
            {
                return true;
            }

            var recs = (from screening in Screenings.AsEnumerable()
						where screening.Field<Int32>("SCREENING_STATUS_ID") == 1
						&& screening.Field<string>("SCREENING_TYPE") != CON.ScreeningFor.SiteVist
                        select screening);

            if (recs.AsDataView().Count == 0)
            {
                isComplete = true;
            }
        }

        return isComplete;
    }


    public static bool HasFailActions(int regId)
    {
        bool hasMatches = false;

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectScreeningFailedActivities(regId);

        if (Helper.HasRows(ds))
        {
            hasMatches = true;
        }

        return hasMatches;
    }

    private static List<string> m_registrationTaskType = new List<string>();
    private static List<string> m_complianceTaskType = new List<string>();
    private static List<string> m_registrationScreeningTaskType = new List<string>();
    private static List<string> m_registrationAppealTaskType = new List<string>();
    private static List<string> m_siteVisitTaskType = new List<string>();
    private static List<string> m_stateAdminTaskType = new List<string>();

    private static List<string> RegistrationTaskType()
    {
        if (m_registrationTaskType.Count == 0)
        {
            m_registrationTaskType.Add(CON.RegistrationTaskName.ProviderDataEntry);
            m_registrationTaskType.Add(CON.RegistrationTaskName.ProviderDataEntryAccountInfo);
            m_registrationTaskType.Add(CON.RegistrationTaskName.ProviderReview);
            m_registrationTaskType.Add(CON.RegistrationTaskName.AccountReview);
            m_registrationTaskType.Add(CON.RegistrationTaskName.AdminReview);
            m_registrationTaskType.Add(CON.RegistrationTaskName.FandAReview);
        }
        return m_registrationTaskType;
    }

    private static List<string> ComplianceTaskType()
    {
        if (m_complianceTaskType.Count == 0)
        {
            m_complianceTaskType.Add(CON.ComplianceTaskName.UploadRR);
            m_complianceTaskType.Add(CON.ComplianceTaskName.UploadPAO);
            m_complianceTaskType.Add(CON.ComplianceTaskName.RecordDateOfMailReturn);
            m_complianceTaskType.Add(CON.ComplianceTaskName.UploadAO);
            m_complianceTaskType.Add(CON.ComplianceTaskName.RecordHearingStatus);
            m_complianceTaskType.Add(CON.ComplianceTaskName.RecordDateAOMailed);
            m_complianceTaskType.Add(CON.ComplianceTaskName.TerminationReason);
            m_complianceTaskType.Add(CON.ComplianceTaskName.CSUploadRR);
            m_complianceTaskType.Add(CON.ComplianceTaskName.CSUploadPI);
            m_complianceTaskType.Add(CON.ComplianceTaskName.CSEnterTR);
            m_complianceTaskType.Add(CON.ComplianceTaskName.ReconsiderationEnterTR);
        }
        return m_complianceTaskType;
    }

    private static List<string> RegistrationScreeningTaskType()
    {
        if (m_registrationScreeningTaskType.Count == 0)
        {
            m_registrationScreeningTaskType.Add(CON.RegistrationTaskName.ProviderScreening);
            m_registrationScreeningTaskType.Add(CON.RegistrationTaskName.StateReview);
        }
        return m_registrationScreeningTaskType;
    }

    private static List<string> RegistrationAppealTaskType()
    {
        if (m_registrationAppealTaskType.Count == 0)
        {
            m_registrationAppealTaskType.Add(CON.RegistrationTaskName.ProcessAppeal);
            m_registrationAppealTaskType.Add(CON.RegistrationTaskName.ProcessAppealSiteVisit);
        }
        return m_registrationAppealTaskType;
    }

    private static List<string> StateAdminTaskType()
    {
        if (m_stateAdminTaskType.Count == 0)
        {
            m_stateAdminTaskType.Add(CON.RegistrationTaskName.StateReview);
            m_stateAdminTaskType.Add(CON.RegistrationTaskName.StateReviewSiteVisit);
            m_stateAdminTaskType.Add(CON.RegistrationTaskName.ProcessAppeal);
            m_stateAdminTaskType.Add(CON.RegistrationTaskName.ProcessAppealSiteVisit);
            m_stateAdminTaskType.Add(CON.RegistrationTaskName.GroupMemberRetroReview);
            m_stateAdminTaskType.Add(CON.RegistrationTaskName.PendingApproval);
            m_stateAdminTaskType.Add(CON.RegistrationTaskName.PendingDenial);
            m_stateAdminTaskType.Add(CON.RegistrationTaskName.PendingTerminate);
        }
        return m_stateAdminTaskType;
    }

    private static List<string> SiteVisitTaskType()
    {
        if (m_siteVisitTaskType.Count == 0)
        {
            m_siteVisitTaskType.Add(CON.RegistrationTaskName.SiteVisitPCG);
            m_siteVisitTaskType.Add(CON.RegistrationTaskName.SiteVisitCompliance);
        }
        return m_siteVisitTaskType;
    }

    public static bool IsRegistrationTaskType(string taskName)
    {
        return RegistrationTaskType().Contains(taskName);
    }

    public static bool IsRegistrationScreeningTaskType(string taskName)
    {
        return RegistrationScreeningTaskType().Contains(taskName);
    }

    public static bool IsRegistrationAppealTaskType(string taskName)
    {
        return RegistrationAppealTaskType().Contains(taskName);
    }

    public static bool IsComplianceTaskType(string taskName)
    {
        return ComplianceTaskType().Contains(taskName);
    }

    public static bool IsStateAdminTaskType(string taskName)
    {
        return StateAdminTaskType().Contains(taskName);
    }

    public static bool IsSiteVisitTaskType(string taskName)
    {
        return SiteVisitTaskType().Contains(taskName);
    }

    public static bool IsVisibleToWaiverservice(int[] waiverTypes, int DIDDReferralId)
    {
        int waivertypeCode = 0;
        PDMSService.PDMSServiceClient psc1 = new PDMSService.PDMSServiceClient();
        DataSet ds1 = psc1.SelectDIDDReferralService(DIDDReferralId);
        if (Helper.HasRows(ds1))
        {
            int[] diddServices = new int[ds1.Tables[0].Rows.Count];
            int counter = 0;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                diddServices[counter] = Helper.GetInt("DIDD_SERVICE_ID", dr);
                counter++;
            }
            if (diddServices.Length != 0)
            {
                foreach (int diddserviceid in diddServices)
                {
                    ds1 = psc1.SelectDIDDServicesByDIDD_Service_ID(diddserviceid);
                    waivertypeCode = Helper.GetInt("WAIVER_TYPE_CODE", ds1.Tables[0].Rows[0]);

                    if (Array.IndexOf(waiverTypes, waivertypeCode) >= 0)
                    {
                        return true;
                    }

                }
            }
        }
        return false;
    }


    public static bool IsTerminateRequest(string action)
    {
        return action == "Deny" || (action == "Terminate") || action == "Terminate Provider";
    }

    public static bool IsIndividual(int regId)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(regId, "PROVIDER");
        if (Helper.HasRows(ds))
        {
            //dtProvider = ds.Tables[0];
            DataRow dr = ds.Tables[0].Rows[0];
            int entityTypeID = Helper.GetInt("ENTITY_TYPE_ID", dr);
            return entityTypeID == CON.ProviderCategoryTypeID.Individual;
        }
        return false;
    }

    public static int GetRegPageTypeId(int registrationStep)
    {
        int RegPageTypeId = 0;
        if(registrationStep > 0)
        {
            Dictionary<int, KeyValuePair<string, int>> dict = CON.SectionTypeKeyValue.SectionType;
            KeyValuePair<string,int> stepInfo = dict.FirstOrDefault(x => x.Key == registrationStep).Value;
            RegPageTypeId = stepInfo.Value;
        }
        return RegPageTypeId;
    }

    public static string GetSectionNameByUrl(string url)
    {
        string sectionName = string.Empty;
        string stepId = string.Empty;

        string[] queryParts = url.ToLower().Split("?".ToCharArray());
        foreach (string queryPart in queryParts)
        {
            string[] queryParam = queryPart.Split("=".ToCharArray());
            if (queryParam[0] == "step")
                stepId = queryParam[1];
        }

        //string stepId = url.Replace("/Process/Registration.aspx?Step=", "");
        int sectionId = Convert.ToInt32(stepId);
        Dictionary<int, KeyValuePair<string, int>> dict = CON.SectionTypeKeyValue.SectionType;
        KeyValuePair<string, int> stepInfo = dict.FirstOrDefault(x => x.Key == sectionId).Value;
        sectionName = stepInfo.Key;

        return sectionName;
    }

    public static int GetPageIDFromSectionID(int sectionNumber)
    {
        int pageID = 0;

        Dictionary<int, KeyValuePair<string, int>> dict = CON.SectionTypeKeyValue.SectionType;
        pageID = dict.FirstOrDefault(x => x.Key == sectionNumber).Value.Value;

        return pageID;
    }

    public static bool CanIndividualAddPA(int regID)
    {
        bool IndividualProvider = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationByRegID(regID);
        if (Helper.HasRows(ds))
        {
            DataTable reg = ds.Tables[0];
            if (reg.Rows.Count > 0)
            {
                DataRow dr = reg.Rows[0];
                // Get provider type
                string CanIndividualAddPAProviderTypeName = SelectAppSetting("CanIndividualAddPAProviderTypeName");
                int providerTypeID = Helper.GetInt("ProviderTypeID", dr);
                string providerTypeName = GetMMISProviderTypeID(providerTypeID);
                //Get category type
                int EntityTypeID = 0;
                EntityTypeID = Helper.GetInt("ProviderCategoryTypeID", dr);


                List<string> IndividualPIDs = new List<string>();
                IndividualPIDs.AddRange(CanIndividualAddPAProviderTypeName.Split(','));
                if (IndividualPIDs.Find(itm => itm == providerTypeName) != null)
                {
                    if (EntityTypeID == CON.ProviderCategoryTypeID.Individual)
                        IndividualProvider = true;
                }
            }
        }
        return IndividualProvider;
    }
    public static bool IsEPDProvider(int regID)
    {
        bool EPDProvider = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationByRegID(regID);
        if (Helper.HasRows(ds))
        {
            DataTable reg = ds.Tables[0];
            if (reg.Rows.Count > 0)
            {
                DataRow dr = reg.Rows[0];
                // Get provider type
                string EPDProviderTypeName = SelectAppSetting("EPDProviderMMISTypeID");
                int providerTypeID = Helper.GetInt("ProviderTypeID", dr);
                string providerTypeName = GetMMISProviderTypeID(providerTypeID);

                List<string> EPDPIDs = new List<string>();
                EPDPIDs.AddRange(EPDProviderTypeName.Split(','));
                if (EPDPIDs.Find(itm => itm == providerTypeName) != null)
                {
                    EPDProvider = true;
                }
            }
        }
        return EPDProvider;
    }

    public static bool IsHomeHealthProvider(int regID)
    {
        bool HomeHealthProvider = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationByRegID(regID);
        if (Helper.HasRows(ds))
        {
            DataTable reg = ds.Tables[0];
            if (reg.Rows.Count > 0)
            {
                DataRow dr = reg.Rows[0];
                // Get provider type
                string HomeHealthProviderTypeName = SelectAppSetting("HomeHealthProviderMMISTypeID");
                int providerTypeID = Helper.GetInt("ProviderTypeID", dr);
                string providerTypeName = GetMMISProviderTypeID(providerTypeID);
               
                List<string> HHPIDs = new List<string>();
                HHPIDs.AddRange(HomeHealthProviderTypeName.Split(','));
                if (HHPIDs.Find(itm => itm == providerTypeName) != null)
                {
                    HomeHealthProvider = true;
                }
            }
        }
        return HomeHealthProvider;
    }

    public static bool IsHospiceProvider(int regId)
    {
        bool hospiceProvider = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationByRegID(regId);
        if (Helper.HasRows(ds))
        {
            DataTable reg = ds.Tables[0];
            if (reg.Rows.Count > 0)
            {
                DataRow dr = reg.Rows[0];
                // Get provider type
                string nursingHomeProviderTypeName = SelectAppSetting("NursingHomeProviderTypeName");
                string providerTypeName = Helper.GetString("ProviderTypeName", dr);

                // Get specialty
                string hospiceSpecialtyName = SelectAppSetting("HospiceSpecialtyName");
                string specialtyName = Helper.GetString("SpecialtyTypeName", dr);

                if (nursingHomeProviderTypeName == providerTypeName && hospiceSpecialtyName == specialtyName)
                {
                    hospiceProvider = true;
                }
            }
        }
        return hospiceProvider;
    }
    public static string GetMMISProviderTypeID(int providerTypeId)
    {
        string rtn = "";
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetProviderTypeById(providerTypeId);
        if (Helper.HasRows(ds))
        {
            rtn = Helper.GetString("MMIS_PROVIDER_TYPE_ID", ds.Tables[0].Rows[0]);
        }
        return rtn;
    }
    public static string GetSectionNameFromStepNumber(int stepNumber)
    {
        Dictionary<int, KeyValuePair<string, int>> dict = CON.SectionTypeKeyValue.SectionType;
        string sectionName = dict.FirstOrDefault(x => x.Key == stepNumber).Value.Key;

        return sectionName;
    }
    public static bool HasPrimaryPracticeLocationChange(int regId)
    {
        bool rtn = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rtn = psc.HasPrimaryPracticeLocationChange(regId);
        return rtn;
    }
    public static bool HasNewOwnersOrChangeInOwnerInformation(int regId)
    {
        bool rtn = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rtn = psc.HasNewOwnersOrChangeInOwnerInformation(regId);
        return rtn;
    }
    public static void UpdateAutoApproveSectionsInRegistration(int regId)
    {
        DataTable dtApproved = null;
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            dtApproved = psc.GetRegistrationAutoApproveSections(regId).Tables[0];
            if (Helper.HasRows(dtApproved))
            {                
                int pagesectionid = 0;
                foreach (DataRow dr in dtApproved.Rows)
                {                   
                    pagesectionid = Helper.GetInt("REG_SECTION_TYPE_ID", dr);
                    if (pagesectionid > 0)
                    {
                        SetSectionNodeStatusId(regId, pagesectionid, CON.RegistrationProviderServicesStatusTypeId.Approved);
                    }

                }
            }            
        }
    }


    public static int GetNextUnApprovedSectionSequence(Dictionary<int, RegistrationNode> registrationNodes, int sequence)
    {
        int retSeq = sequence;
        List<RegistrationNode> unapprovedNodes = registrationNodes.Where(s => s.Value.StatusId != CON.RegistrationProviderStatusTypeId.Complete || s.Value.ProviderStatusId != CON.RegistrationProviderStatusTypeId.Complete).Select(x => x.Value).ToList();

        int idx = unapprovedNodes.FindIndex(n => n.Sequence == sequence);
        if (unapprovedNodes.Count > idx + 1 )
        {
            if (idx == -1)
            {
                foreach (RegistrationNode rn in unapprovedNodes)
                {
                    if (rn.Sequence >= sequence)
                    {
                        retSeq = rn.Sequence;
                        break;
                    }
                }
            }
            else
                retSeq = unapprovedNodes[idx + 1].Sequence;
        }
        else if ((idx + 1) == unapprovedNodes.Count)
            retSeq = idx;
        return retSeq;
    }

    public static bool AllowGroupToUpdateIndividualMember(int regId)
    {
        bool rtn = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationByRegID(regId);
        if (Helper.HasRows(ds))
        {
            DataTable reg = ds.Tables[0];
            if (reg.Rows.Count > 0)
            {
                DataRow dr = reg.Rows[0];
                int applicationTypeID = Helper.GetInt("ApplicationTypeID", dr);
                int EntityTypeID = 0;
                EntityTypeID = Helper.GetInt("ProviderCategoryTypeID", dr);

                if (EntityTypeID == CON.ProviderCategoryTypeID.Group && applicationTypeID == CON.ApplicationType.Standard)
                {
                    rtn = true;
                }
            }
        }
        return rtn;
    }

    public static bool CanUserViewDelete(int registrationId,int registrationIdSelected, string currentTaskName)
    {
        bool retVal = false;
        int regid = registrationIdSelected > 0 ? registrationIdSelected : registrationId;
        try
        {
            if(CanUserEditRegistration(registrationIdSelected, currentTaskName))
            {
                PDMSService.PDMSServiceClient psc1 = new PDMSService.PDMSServiceClient();
                DataSet ds1 = psc1.SelectRegistrationByRegID(regid);

                if (Helper.HasRows(ds1) && Helper.GetInt("RegistrationProgramStatusTypeID", ds1.Tables[0].Rows[0]) != CON.RegistrationProgramStatusTypeId.Conversion)
                {
                    retVal = true;
                }
            }

        }
        catch(Exception)
        {
            retVal = false;
        }

        return retVal;
    }
    public static bool CanProviderSpecialtyViewEVV(int regId)
    {
        bool EVVSpecialty = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(regId, "specialty_Active");
        if (Helper.HasRows(ds))
        {
            DataTable reg = ds.Tables[0];
            if (reg.Rows.Count > 0)
            {
                foreach (DataRow dr in reg.Rows)
                {
                // Get specialty type
                string EVVProviderSpecialtyMMISTypeName = SelectAppSetting("MMISSpecialtyTypeIDsCanUploadEVVTraining");
                string MMISSpecialtyTypeID = Helper.GetString("MMIS_Specialty_Type_ID", dr);
                // 
                List<string> SpecIDs = new List<string>();
                SpecIDs.AddRange(EVVProviderSpecialtyMMISTypeName.Split(','));
                if (SpecIDs.Find(itm => itm == MMISSpecialtyTypeID) != null)
                {
                    EVVSpecialty = true;
                        return EVVSpecialty;
                    }
                }

            }
        }
        return EVVSpecialty;
    }
    public static string GetPrimaryPracticeLocationState(int regID)
    {
        string rtn = "";
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectProviderAddressInfo(regID, 1);
        if (Helper.HasRows(ds))
        {
            rtn = Helper.GetString("STATE", ds.Tables[0].Rows[0]);
        }
        return rtn;
    }

}