using Corp.Core.Libraries;
using Corp.Core.Libraries.Helper;
using Corp.Core.Libraries.IncidentManagementService;
using DocumentFormat.OpenXml.ExtendedProperties;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MAXIMUS.Models.Data.PDMS;
using MigraDoc.DocumentObjectModel.Internals;
using NPOI.OpenXmlFormats.Wordprocessing;
using PdfSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_Registration : WorkflowPage
{
    // TODO: EDV Remove thse view state variables
    public bool OwnershipChangedFlag { get; set; }
    private DataTable dtProvider = null;
    int currProgramStatusTypeID = 0;
    int currentStatus = 0;
    private bool _DisplayConfirmAdd = false;
    private bool IsPowerAgent = false; // SAM768 
    private bool isEnrollmentSpecialist { get { return Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.EnrollmentSpecialist); } }

    public int EntityTypeId
    {
        get
        {
            return this.EntityTypeID;
        }
        set { this.EntityTypeID = value; }
    }

    public int ProviderTypeId
    {
        get
        {
            return this.ProviderTypeID;
        }
        set { this.ProviderTypeID = value; }
    }

    public int SpecialtyTypeID
    {
        get
        {
            if (ViewState["SpecialtyTypeID"] == null) ViewState["SpecialtyTypeID"] = 0;
            return Convert.ToInt32(ViewState["SpecialtyTypeID"]);
        }
        set { ViewState["SpecialtyTypeID"] = value; }
    }

    public int MMISSpecialtyType
    {
        get
        {
            if (ViewState["MMISSpecialtyType"] == null) ViewState["MMISSpecialtyType"] = 0;
            return Convert.ToInt32(ViewState["MMISSpecialtyType"]);
        }
        set { ViewState["MMISSpecialtyType"] = value; }
    }

    private int GridViewPageSize
    {
        get { return this.gvConvertedDocs.PageSize == 0 ? 10 : this.gvConvertedDocs.PageSize; }
    }

    private int GridViewCurrentPage
    {
        get { return ViewState["GridViewCurrentPage"] == null ? 0 : (int)ViewState["GridViewCurrentPage"]; }
        set { ViewState["GridViewCurrentPage"] = value; }
    }

    //TODO: EDV This should be taken from constants. we should get rid of this enum
    private enum Section
    {
        SpecialtiesTaxonomies = 0,
        SpecialtiesTaxonomiesHistory,
        AdditionalSpecialtiesTaxonomies,
        AdditionalSpecialtiesTaxonomiesHistory,
        Specialties,
        SpecialtiesHistory,
        AdditionalSpecialties,
        AdditionalSpecialtiesHistory,
        Taxonomies,
        TaxonomiesHistory,
        AdditionalTaxonomies,
        AdditionalTaxonomiesHistory,
        Licenses,
        LicensesHistory,
        Certifications,
        CertificationsHistory,
        CertSecondGrid,
        CertSecondGridHistory,
        Medicare,
        MedicareHistory,
        Medicaid,
        MedicaidHistory,
        Pharmacy,
        PharmacyHistory,
        NumberOfBeds,
        Insurance,
        DentalLicense,
        VisionProviders,
        NursingProfessionalCertification,
        CPRCertification,
        CategoryOfService,
        PharmacyProviders,
        PharmacyPharmacist,
        ReimbursementRates
    };

    public static object GetPropValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName).GetValue(src, null);
    }

    //TODO: EDV Refactor this. Most of the logic is gone. Do we need this. Isnt registrationStep enough???
    public int Step
    {
        get
        {
            if (ViewState["Step"] == null) ViewState["Step"] = 0;
            return Convert.ToInt32(ViewState["Step"]);
        }
        set
        {
            string title = Registration.GetSectionText(value);
            if (title == "Provider Screening" & ApplicationTypeID == 11) //TODO hardcoded for credentialing
                title = "Provider Credentialing";

            ucRegistrationNavigation.Title = ucRegistrationNavigationBottom.Title = title;
            ucRegistrationNavigationBottom.ShowRegistrationId = true;
            if (value == CON.RegistrationPageType.ApplicationFee && this.CurrentTaskName == CON.RegistrationTaskName.ApplicationFeeReview)
            {
                this.RegistrationStep = CON.RegistrationPageType.ApplicationFee;
                if (this.RegistrationNodes.ContainsKey(this.RegistrationStep))
                {
                    this.RegistrationSequence = this.RegistrationNodes[this.RegistrationStep].Sequence;
                }
            }
            else
            {
                this.RegistrationStep = value;
                if (this.RegistrationNodes.ContainsKey(this.RegistrationStep))
                {
                    this.RegistrationSequence = this.RegistrationNodes[this.RegistrationStep].Sequence;
                }
            }
            if (value == CON.SectionTypeID.WorkflowSteps || value == CON.SectionTypeID.MMISTransactions || value == CON.SectionTypeID.TransactionQueue)  // JIRA 3276
            {
                ucUploadDocument.Visible = false;
            }
            ViewState["Step"] = value;
        }
    }


    private bool IsReadOnly
    {
        get
        {
            return this.RegistrationIdSelected == 0 ? false : true;
        }
    }

    public bool _ValidationHasProcessed;
    public bool ValidationHasProcessed
    {
        get
        {
            if (ViewState["ValidationHasProcessed"] == null)
            {
                ViewState.Add("ValidationHasProcessed", false);
                _ValidationHasProcessed = (bool)ViewState["ValidationHasProcessed"];
            }

            return _ValidationHasProcessed;
        }

        set
        {
            _ValidationHasProcessed = value;
        }
    }

    public int PrimarySpecialtyTypeID
    {
        get
        {
            if (ViewState["PrimarySpecialtyTypeID"] == null) ViewState["PrimarySpecialtyTypeID"] = 0;
            return (int)ViewState["PrimarySpecialtyTypeID"];
        }
        set { ViewState["PrimarySpecialtyTypeID"] = value; }
    }
    public bool ShowCredential
    {
        get
        {
            if (ViewState["ShowCredential"] == null) ViewState["ShowCredential"] = false;
            return (bool)ViewState["ShowCredential"];
        }
        set { ViewState["ShowCredential"] = value; }
    }

    protected bool IsStateReview
    {
        get
        {
            if (ViewState["IsStateReview"] == null)
                ViewState["IsStateReview"] = false;

            return (bool)ViewState["IsStateReview"];
        }
        set
        {
            ViewState["IsStateReview"] = value;
        }
    }

    public void LoadPlaceHolder(int pageStep)
    {
        Upload upload = new Upload();
        string sectionName = Registration.GetStepText(pageStep);
        int pageID = Registration.GetPageIDFromSectionID(pageStep);

        DataSet ds = null;

        // TODO: EDV Remove hardcoding, what is 44?? Can it go into the section?
        if (this.RegistrationStep == 44)
        {
            sectionName = "OtherDocuments";
            bool canviewEVV = false;
            canviewEVV = Registration.CanProviderSpecialtyViewEVV(this.RegistrationId);
            ds = upload.LoadUserSectionControl(this.RegistrationId, this.ApplicationTypeID, this.EntityTypeId, this.ProviderTypeId, pageID, sectionName);
            if (Helper.HasRows(ds.Tables["RegSectionUploadControl"]))
            {
                if (!canviewEVV)
                {
                    var query = ds.Tables["RegSectionUploadControl"].AsEnumerable().Where(r => r.Field<string>("TITLE").Trim() == "EVV Training");
                    if (query != null)
                    {
                        foreach (var row in query.ToList())
                            row.Delete();
                        ds.Tables["RegSectionUploadControl"].AcceptChanges();
                    }
                }
            }
        }
        else if (sectionName == "Education" || sectionName == "Work History" || sectionName == "Reconsideration")
        {
            ds = upload.LoadUserSectionControl(this.RegistrationId, this.ApplicationTypeID, this.EntityTypeId, this.ProviderTypeId, pageID, sectionName);
        }
        else
        {
            ds = upload.LoadUserSectionControl(this.RegistrationId, this.ApplicationTypeID, this.EntityTypeId, this.ProviderTypeId, pageID);
        }
        LoadUploadSection(ds, true);
    }

    private void LoadUploadSection(DataSet ds, bool isPageLevelUpload)
    {
        //grdSpecialties
        DataSet SpecialitiesInfo = UcSpeciality1.SpecialitiesInfo;
        for (int i = 0; i <= SpecialitiesInfo.Tables.Count - 1; i++)
        {
            foreach (DataRow dr1 in SpecialitiesInfo.Tables[i].Rows)
            {
                if (!_DisplayConfirmAdd == true)
                {
                    _DisplayConfirmAdd = Helper.GetString("MMIS_SPECIALTY_TYPE_ID", dr1).Equals("ORR") ? true : false;
                }
            }
        }
        int table = ds.Tables.Count;
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                if (dr["TITLE"].ToString() != CON.MmisTypeForORR)
                {


                    //IDWithFile.Add(Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr));
                    UserControls_UploadSectionControl ucUploadSectionControl =
                        LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;

                    ucUploadSectionControl.Title = Helper.GetString("TITLE", dr);

                    ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                    ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);
                    if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                        ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);

                    if ((ds.Tables[i].Columns.Contains("IS_REQUIRED")))
                        ucUploadSectionControl.IsRequired = Convert.ToBoolean(Helper.GetString("IS_REQUIRED", dr));

                    if ((ds.Tables[i].Columns.Contains("FILE_NAME")))
                        ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                    else
                        ucUploadSectionControl.FileName = null;

                    ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";

                    //check if a control already exist with same ID
                    //var placeHolder = PlaceholderUploadSectionControl.FindControl("");
                    PlaceholderUploadSectionControl.Controls.Add(ucUploadSectionControl);
                }
                else if ((_DisplayConfirmAdd == true) && (dr["TITLE"].ToString() == CON.MmisTypeForORR))
                {


                    //IDWithFile.Add(Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr));
                    UserControls_UploadSectionControl ucUploadSectionControl =
                        LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;

                    ucUploadSectionControl.Title = Helper.GetString("TITLE", dr);

                    ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                    ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);
                    if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                        ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);

                    if ((ds.Tables[i].Columns.Contains("IS_REQUIRED")))
                        ucUploadSectionControl.IsRequired = Convert.ToBoolean(Helper.GetString("IS_REQUIRED", dr));
                    if (isPageLevelUpload && this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg)
                    {
                        ucUploadSectionControl.IsRequired = true;
                    }

                    if ((ds.Tables[i].Columns.Contains("FILE_NAME")))
                        ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                    else
                        ucUploadSectionControl.FileName = null;

                    ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";

                    //check if a control already exist with same ID
                    //var placeHolder = PlaceholderUploadSectionControl.FindControl("");
                    PlaceholderUploadSectionControl.Controls.Add(ucUploadSectionControl);
                }
            }
        }
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())

            Page.Theme = "Modernization";
        else
            Page.Theme = "Default";

        if (!Page.IsPostBack)
            this.FillRegistrationData();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            if (PreviousPage != null)
            {
                this.RegistrationId = PreviousPage.RegistrationId;
                this.RegistrationStep = PreviousPage.RegistrationStep;
                this.CommandName = PreviousPage.CommandName;
                this.RegistrationIdSelected = PreviousPage.IsReadOnly ? PreviousPage.RegistrationId : 0;
            }
            else if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString.AllKeys.Contains("RegId"))
                    this.RegistrationId = int.Parse(System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(Request["RegId"].ToString(), true));
                if (Request.QueryString.AllKeys.Contains("Step"))
                    this.RegistrationStep = int.Parse(Request["Step"]);

            }

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            IsPowerAgent = Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, this.RegistrationId); // SAM768
            // OHPNM-8164/OHPNM-8305 - if this user is a provider administrator or provider agent, let's see if they should be looking at this registration
            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAdministrator) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent))
            {
                // see if they can access this registration
                bool UserCanAccessReg = psc.UserCanAccessReg(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), this.RegistrationId);
                if (!UserCanAccessReg)
                {
                    // they shouldn't be here; kick em out
                    Response.Redirect("~/Process/ProviderHomeNew.aspx");
                }
            }
            //
            if (this.RegistrationStep == CON.RegistrationPageType.ProviderCredentialing)
            {
                ShowCredential = Helper.ShowCredential(this.RegistrationId);
            }
            if (!Page.IsPostBack)
                this.FillRegistrationData();

            SessionVarRetriever.WorkFlowRegID = this.RegistrationId;  // LS 05/01/2020 - This line is TEMPORARY, until all the address pages are redone.

            HttpContext.Current.Trace.Write("Registration.asxp Page_Load Start ");

            ucRegistrationNavigation.SaveDataEvent += new UserControls_RegistrationNavigation.SaveDataEventHandler(ucRegistrationNavigation_SaveDataEvent);
            ucRegistrationNavigation.ValidateDataEvent += new UserControls_RegistrationNavigation.ValidateDataEventHandler(ucRegistrationNavigation_ValidateDataEvent);
            ucRegistrationNavigation.ValidateSaveNextDataEvent += new UserControls_RegistrationNavigation.ValidateSaveNextDataEventHandler(ucRegistrationNavigation_ValidateSaveNextDataEvent);
            ucRegistrationNavigation.ValidateIsRequiredDataEvent += new UserControls_RegistrationNavigation.ValidateIsRequiredDataEventHandler(ucRegistrationNavigation_ValidateIsRequiredDataEvent);
            ucRegistrationNavigation.RefreshEvent += new UserControls_RegistrationNavigation.RefreshEventHandler(ucRegistrationNavigation_RefreshEvent);
            ucRegistrationNavigation.ValidateDataPSEvent += new UserControls_RegistrationNavigation.ValidateDataPSEventHandler(ucRegistrationNavigation_ValidateDataPSEvent);
            ucRegistrationNavigation.SaveDataPSEvent += new UserControls_RegistrationNavigation.SaveDataPSEventHandler(ucRegistrationNavigation_SaveDataPSEvent);
            ucRegistrationNavigationBottom.SaveDataEvent += new UserControls_RegistrationNavigation.SaveDataEventHandler(ucRegistrationNavigation_SaveDataEvent);
            ucRegistrationNavigationBottom.ValidateDataEvent += new UserControls_RegistrationNavigation.ValidateDataEventHandler(ucRegistrationNavigation_ValidateDataEvent);
            ucRegistrationNavigationBottom.RefreshEvent += new UserControls_RegistrationNavigation.RefreshEventHandler(ucRegistrationNavigation_RefreshEvent);
            ucRegistrationNavigationBottom.ValidateDataPSEvent += new UserControls_RegistrationNavigation.ValidateDataPSEventHandler(ucRegistrationNavigation_ValidateDataPSEvent);
            ucRegistrationNavigationBottom.SaveDataPSEvent += new UserControls_RegistrationNavigation.SaveDataPSEventHandler(ucRegistrationNavigation_SaveDataPSEvent);
            ucRegistrationNavigationBottom.ValidateIsRequiredDataEvent += new UserControls_RegistrationNavigation.ValidateIsRequiredDataEventHandler(ucRegistrationNavigation_ValidateIsRequiredDataEvent);

            ucUploadDocument.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
            // @"C:\Projects\Upload"; 
            ucUploadDocument.RenameFileMethod += new UserControls_UploadDocument.RenameFile(ucUploadDocument_RenameFileMethod);
            ucUploadDocument.SuccessEvent += new UserControls_UploadDocument.SuccessEventHandler(ucUploadDocument_SuccessEvent);
            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ReadOnly)) ucUploadDocument.SetUploadButtonEnabled = false;

            btnSave.Visible = false;

            if (this.RegistrationStep != 0)
            {
                LoadPlaceHolder(this.RegistrationStep);
                LoadData(this.RegistrationStep);
            }

            HttpContext.Current.Trace.Write("Registration.asxp Page_Load End ");

            Master.SetGeneratePDFPanelVisibility(true);

            CredentialHelper.APIToken APIToken = ApplicationCache.RestAPIAccessToken();
            HiddenField hdnAPIToken = (HiddenField)Page.Master.FindControl("hdnAccessToken");
            HiddenField hdnAPIRefreshToken = (HiddenField)Page.Master.FindControl("hdnRefreshToken");
            hdnAPIToken.Value = APIToken.AccessToken;
            hdnAPIRefreshToken.Value = APIToken.RefreshToken;
        }
        catch (Exception ex)
        {
        }
        PDMSService.PDMSServiceClient svmc = new PDMSService.PDMSServiceClient();
        var dsStatus = svmc.SelectRegistrationByRegID(this.RegistrationId);
        if (Helper.HasRows(dsStatus))
        {
            DataRow row = dsStatus.Tables[0].Rows[0];
            currentStatus = row.Field<int>("RegProgramStatusTypeID");
        }

        if ((Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAdministrator) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAgent))
            && Master.TaskName == CON.ProviderDataEntry)
        {
            if (HasUnsavedData.Value == "TrueSpeciality")
            {
                HasUnsavedData.Value = "True";
            }
        }

        /*ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "openTab",
            "window.open('" + url + "', '_blank');",
            true
        );*/


        this.spnHelpSection.InnerText = Registration.GetSectionText(Step==0?1:Step);
    }

    void LoadConvertedDocuments(int pageNumber)
    {
        if (this.RegistrationId > 0 && !Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
        {
            PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient();
            var convertedDocuments = client.SelectConvertedDocuments(this.RegistrationId, pageNumber, gvConvertedDocs.PageSize);
            if (Helper.HasRows(convertedDocuments) && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ComplianceSpecialist))
            {
                setConvertedDocumentsVisibility(CanNotEdit, convertedDocuments);
            }
            else if (Helper.HasRows(convertedDocuments))
            {
                setConvertedDocumentsVisibility(!CanNotEdit, convertedDocuments);
            }
        }
    }

    void setConvertedDocumentsVisibility(bool isVisible, DataSet convertedDocuments)
    {
        convertedDocs.Visible = isVisible;
        int searchResultsTotalRows = Helper.GetInt("TOTAL", convertedDocuments.Tables[0].Rows[0]);
        gvConvertedDocs.VirtualItemCount = searchResultsTotalRows;
        gvConvertedDocs.DataSource = convertedDocuments.Tables[0];
        gvConvertedDocs.DataBind();
    }

    void LoadPaperDocuments()
    {
        if (this.RegistrationId > 0)
        {
            PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient();
            var paperDocuments = client.SelectPaperDocuments(this.RegistrationId);

            if (Helper.HasRows(paperDocuments))
            {
                paperDocs.Visible = true;
                gvPaperDocuments.DataSource = paperDocuments;
                gvPaperDocuments.DataBind();
            }
        }
    }

    string ucRegistrationNavigation_ValidateDataPSEvent(int step)
    {
        string rtn = string.Empty;
        switch (step)
        {
            default:
                break;
        }
        return rtn;
    }

    private void ucRegistrationNavigation_SaveDataPSEvent(int step, int action)
    {
        switch (step)
        {
            default:
                break;
        }
    }

    protected override void Page_PreRender(object sender, EventArgs e)
    {
        HttpContext.Current.Trace.Write("Registration.asxp Page_Prerender Start ");
        if (!Page.IsPostBack)
        {
            LoadGroupIndividualMemberData();
            this.FillRegistrationData();
            LoadConvertedDocuments(1);
        }

        if (!Page.IsPostBack)
        {

            if (Request["ViewProviderFile"] != null)
            {
                ((MasterWorkflowPage)this.Master).ViewProviderFile = true;
            }

            IsStateReview = ((MasterWorkflowPage)this.Master).TaskName == CON.RegistrationTaskName.StateReview;
        }

        int stepID = 0;
        if (this.BumpStep != 0)
            stepID = Convert.ToInt32(this.BumpStep);
        else
            stepID = this.RegistrationStep;


        LoadPaperDocuments();

        if (!Page.IsPostBack)
        {
            if (Master.ProcessID == 0)
            {
                // If there is no Workflow then this must be a "View only" look at the Registration from the Provider Home Page
                if (this.RegistrationIdSelected > 0)
                {
                    this.RegistrationId = this.RegistrationIdSelected;
                    DataRow row = Registration.GetRegistration(this.RegistrationId);
                    this.WorkflowEventTypeId = Helper.GetInt("WORKFLOW_EVENT_TYPE_ID", row);
                    this.IsWaiverServiceProvider = Helper.IsWaiverServiceProvider(this.RegistrationId);
                    ucRegistrationNavigationBottom.ShowRegistrationId = true;
                }
                else
                {
                    if (SessionVarRetriever.IndividualRegID == 0 && SessionVarRetriever.GroupUserRegID == 0)
                    {
                        Response.Redirect("~/Default.aspx");
                        return;
                    }

                }
            }
            else
            {
                string workflowName = Master.WorkflowName;
                string taskName = Master.TaskName;
                ucRegistrationNavigationBottom.ShowRegistrationId = true;
            }
        }


        // Bind the left panel tree
        if (!Page.IsPostBack)
            Master.RefreshTree();
        // If the current step is NOT in the possible steps set it to the first step found
        if (this.RegistrationStep != 0)
        {
            if (this.WF_WorkflowID == CON.WorkflowType.CPC && this.RegistrationStep == 1)
                Step = CON.SectionTypeID.CpcContactInformation;
            else if (this.WF_WorkflowID == CON.WorkflowType.CMC && this.RegistrationStep == 1)
            {
                if (Session["ViewProviderFile"] != null && Convert.ToBoolean(Session["ViewProviderFile"]))
                    this.RegistrationStep = 1;
                else
                    Step = CON.SectionTypeID.CMCContactInformation;
            }
            else
                Step = this.RegistrationStep;
        }

        if (!Page.IsPostBack)
        {

            if (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && this.CurrentTaskName == CON.RegistrationTaskName.ProviderReview)
            {
                List<RegistrationNode> modifiedNodes = this.RegistrationNodes.Where(s => s.Value.StatusId == CON.RegistrationProviderStatusTypeId.Modified || s.Value.ProviderStatusId == CON.RegistrationProviderStatusTypeId.Modified).Select(x => x.Value).ToList();
                if (modifiedNodes.Count > 0)
                {
                    Step = modifiedNodes[0].Step;
                }
                else
                {
                    List<RegistrationNode> modifiedNode = this.RegistrationNodes.Where(s => s.Value.StatusId == 1 || s.Value.ProviderStatusId == 1).Select(x => x.Value).ToList();
                    if (modifiedNode.Count > 0)
                        Step = modifiedNode[0].Step;
                }

            }
            else if ((this.WorkflowEventTypeId != CON.WorkflowEventType.UpdateReg) && (this.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry) && !Helper.RegistrationInReturnedToProvider(this.RegistrationId))
            {
                List<RegistrationNode> RegNodes = this.RegistrationNodes.Where(s => s.Value.StatusId != CON.RegistrationProviderStatusTypeId.Modified && s.Value.StatusId != CON.RegistrationProviderStatusTypeId.Complete).Select(x => x.Value).ToList();
                if (RegNodes.Count > 0)
                    Step = RegNodes[0].Step;
                else if (RegistrationNodes.Count == 2 && Helper.RegistrationInReturnedToProviderFromSiteVisit(this.RegistrationId))
                    Step = MAXIMUS.Core.Libraries.Constants.RegistrationPageType.SiteVisitScreening;
            }
            else if (this.CurrentTaskName == CON.RegistrationTaskName.ProviderReview)
            {
                List<RegistrationNode> RegNodes1 = this.RegistrationNodes.Where(s => s.Value.StatusId != CON.RegistrationProviderServicesStatusTypeId.Approved && !(s.Value.StatusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider && s.Value.ProviderStatusId == CON.RegistrationProviderStatusTypeId.NotComplete) &&
                (s.Key != CON.SectionTypeID.WorkflowSteps) &&
                (s.Key != CON.SectionTypeID.TransactionQueue)  // JIRA 3276
                ).Select(x => x.Value).ToList();
                if (RegNodes1.Count > 0)
                    Step = RegNodes1[0].Step;
            }
            else if (this.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry && Helper.RegistrationInReturnedToProvider(this.RegistrationId))
            {
                List<RegistrationNode> rtpNodes = this.RegistrationNodes.Where(s => s.Value.StatusId == CON.RegistrationProviderStatusTypeId.NotComplete && s.Value.StatusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider).Select(x => x.Value).ToList();
                if (rtpNodes.Count > 0)
                    Step = rtpNodes[0].Step;
                if (rtpNodes.Count == 0)
                {
                    List<RegistrationNode> RegNodes = this.RegistrationNodes.Where(s => s.Value.StatusId != CON.RegistrationProviderStatusTypeId.Modified && s.Value.StatusId != CON.RegistrationProviderStatusTypeId.Complete).Select(x => x.Value).ToList();
                    if (RegNodes.Count > 0)
                        Step = RegNodes[0].Step;
                }
            }
            if (Step == 0)
            {
                if (this.WF_WorkflowID == CON.WorkflowType.IncidentCompliance)
                    Step = CON.SectionTypeID.IncidentComplianceReview;
                else if (this.WF_WorkflowID == CON.WorkflowType.CPC)
                    Step = CON.SectionTypeID.CpcContactInformation;
                else if (this.WF_WorkflowID == CON.WorkflowType.CMC && this.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry)
                    Step = CON.SectionTypeID.CMCContactInformation;
                else if (this.WF_WorkflowID == CON.WorkflowType.CMC && this.CurrentTaskName != CON.RegistrationTaskName.ProviderDataEntry)
                    Step = this.RegistrationNodes.First().Key;
                else if (Step == 0 && this.RegistrationNodes.Count > 0)
                {
                    Step = this.RegistrationNodes.First().Key;
                }
                else
                    Step = CON.SectionTypeID.OrgInfo;

            }
        }
        if (!Page.IsPostBack)
            Master.LoadProviderInfoHeader();
        SetVisibility();

        Master.SelectToolBarStep();

        if (!Page.IsPostBack)
            LoadData(Step);

        //only show old upload control for Provider screening =13, Owner=15, Fingerprint=22 Orienation =23 DME=24
        // If we're not showing it, we shouldn't load it, either.
        //Added credentialing step in screening nodes
        int step = this.RegistrationStep;
        // TODO: EDV Remove hardcoding
        List<int> screeningSteps = new List<int>(new int[] { 13, 15, 22, 23, 16, 44, 51 });

        if (this.RegistrationId > 0 && (screeningSteps.Contains(this.RegistrationStep)))
        {
            // TODO: EDV Remove hardcoding, what is 44??
            if (this.RegistrationStep != 44)
            {
                pnlUploadDocsfull.Visible = true;
                ucUploadDocument.Visible = true;
                ucUploadDocument.LoadData(this.RegistrationId, this.RegistrationStep);
            }
        }
        // Show any denial documents submitted by the DHCF Reviewer (if any)
        else if (this.RegistrationId > 0 && this.RegistrationStep == CON.RegistrationPageType.Agreements)
        {
        }


        if (!Page.IsPostBack)
            SetWorkflowPanel();

        // Make sure sequence is set correctly; OHPNM-20089 if the sequence is not well defined, use the first
        RegistrationNode Nodes = null;
        try
        {
            if (this.RegistrationNodes.ContainsKey(Step))
            {
                Nodes = this.RegistrationNodes[Step];
            }
        }
        catch
        {
            if (this.RegistrationNodes.Count > 0)
            {
                Nodes = this.RegistrationNodes[this.RegistrationNodes.First().Key];
            }
        }

        if ((this.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry &&
            Step != CON.SectionTypeID.WorkflowSteps &&
            Step != CON.SectionTypeID.TransactionQueue &&  // JIRA 3276
            Step != CON.SectionTypeID.MMISTransactions) || currentStatus == CON.RegistrationProgramStatusTypeId.Conversion
            )
        {
            if (Nodes != null && Nodes.IsRequired == 1)
            {
                ucRegistrationNavigation.RequiredText = "This is a required section.";
            }
            else
            {
                ucRegistrationNavigation.RequiredText = "This is not a required section. To skip this section click on Next button.";
            }
        }

        stepID = this.RegistrationStep;

        if (!Page.IsPostBack)
        {
            PlaceholderUploadSectionControl.Controls.Clear();
            LoadPlaceHolder(this.RegistrationStep);
            pnlUploadDocsfull.Visible = true;
            ucUploadDocument.Visible = true;
            //if (this.RegistrationStep == CON.SectionTypeID.ClosureNotice)
            //    ucUploadDocument.LoadData(this.RegistrationId, CON.RegistrationPageType.Identification, "ClosureNotice");


            if (this.RegistrationStep == CON.SectionTypeID.DaysNotice)
                ucUploadDocument.LoadData(this.RegistrationId, CON.RegistrationPageType.Identification, "45DaysNotice");
            else
                ucUploadDocument.LoadData(this.RegistrationId, this.RegistrationStep);
        }


        SetButtons(this.CommandName);

        if ((this.WorkflowName == "Incident Compliance") && (Master.TaskName == "Incident Compliance Review") && Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name))
        {
            SetWorkflowPanel();
        }

        if (Nodes != null)
        {
            string ImgUrl = "~/images/" + Nodes.MenuPath.Replace(" ", "") + "_Lg.png";
            if (File.Exists(Server.MapPath(ImgUrl)))
                this.sectionIcon.ImageUrl = ImgUrl;
            else
                this.sectionIcon.ImageUrl = "~/images/ProviderInformation_Lg.png";
        }

        if ((Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAdministrator) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAgent)) && Master.TaskName == CON.ProviderDataEntry)
        {
            //if (!string.IsNullOrEmpty(hdnRegStep.Value) && hdnRegStep.Value != this.RegistrationSequence.ToString())
            //{
            if (HasUnsavedData.Value == "True")
            {
                HasUnsavedData.Value = "";
            }
            //}
            //hdnRegStep.Value = this.RegistrationSequence.ToString();
        }

        HttpContext.Current.Trace.Write("Registration.asxp Page_Prerender End ");
    }

    private void LoadGroupIndividualMemberData()
    {
        int IndividualRegId = 0;
        try
        {
            if (SessionVarRetriever.IndividualRegID > 0 && !(SessionVarRetriever.IsGroup))
            {
                IndividualRegId = SessionVarRetriever.IndividualRegID;
                if (IndividualRegId > 0)
                {
                    this.RegistrationId = IndividualRegId;
                    //Redirect to first step and also make the registration editable.
                    this.RegistrationStep = 1;
                    Master.SetReturnToGroupPanelVisibility(true);

                    (this.Page as RegistrationProvider).IsReadOnly = false;
                }
            }
            else if (SessionVarRetriever.GroupUserRegID > 0 && SessionVarRetriever.IsGroup)
            {

                this.RegistrationId = SessionVarRetriever.GroupUserRegID;
                //Redirect to affiliations step 
                Master.SetReturnToGroupPanelVisibility(false);
                SessionVarRetriever.IndividualRegID = 0;
                this.RegistrationStep = CON.SectionTypeID.Affiliations;
                (this.Page as RegistrationProvider).IsReadOnly = false;

            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
    }

    protected void ShowPendingStatus(object sender, EventArgs e)
    {
        Master.RefreshTree();
        Master.SetWorkflowPanelVisibility(false);                   // Take away any workflow options if present
    }

    /*
     * Consolidated Storyboard and FDD Quesions - DR130
     * Updates (all updates) should require the user to resign the Agreements page.
     */
    protected void InvalidateAgreements(object sender, EventArgs e)
    {
        if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
        {
            //*Assumption was that the last step is always agreements.  When Contracts is the last step, do not invalidate agreements
            if (this.RegistrationStep == CON.RegistrationPageType.Contracts)
                return;

            if (this.WorkflowEventTypeId != CON.WorkflowEventType.UpdateReg)
                Registration.SetProviderSectionNodeStatusId(this.RegistrationId, CON.SectionTypeID.Agreements, CON.RegistrationProviderStatusTypeId.NotComplete);

            //Master.RefreshTree();                                      // Refresh left side menu options tree --ohpnm 3658
            Master.SetWorkflowPanelVisibility(false);                   // Take away any workflow options if present
            // If this is a Return to Provider (RTP) we need to sync the Provider Services status
            DataRow row = Registration.GetRegistration(this.RegistrationId);
            if (row != null && Helper.GetInt("REGISTRATION_STATUS_TYPE_ID", row) == CON.RegistrationStatusTypeId.ReturnToProvider)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                psc.SyncRegistrationPageStatus(this.RegistrationId, this.RegistrationStep,
                    Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            }
        }
    }


    private void LoadData(int step)
    {
        //ucUploadDocument.DocumentSection = string.Empty;
        //delete after

        Dictionary<int, KeyValuePair<string, int>> dict = CON.SectionTypeKeyValue.SectionType;
        int stepNumber = dict.FirstOrDefault(x => x.Key == step).Key;
        string sectionName = dict.FirstOrDefault(x => x.Key == step).Value.Key;
        int pageTypeId = dict.FirstOrDefault(x => x.Key == step).Value.Value;
        string controlPath = string.Empty;

        Enumerations.ScreeningEntityType screeningType = Enumerations.ScreeningEntityType.None;
        switch (step)//stepNumber
        {
            case CON.SectionTypeID.WaiverServiceDisplay:
                controlPath = "~/PopupControls/WaiverServicesdisplay.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("WaiverServices");
                break;
            case CON.SectionTypeID.DaysNotice:
                controlPath = "~/PopupControls/DaysNotice.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("45DaysNotice");
                break;
            case CON.SectionTypeID.ClosureNotice:
                controlPath = "~/PopupControls/ClosureNotice.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("ClosureNotice");
                break;
            case CON.SectionTypeID.ChangeOperatorInfo:
                controlPath = "~/PopupControls/ChangeOperatorInfo.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("ChangeOperatorInformation");
                break;
            case CON.SectionTypeID.ContractMaintenance:
                controlPath = "~/PopupControls/ContractMaintenance.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("Contract Maintenance");
                break;
            case CON.SectionTypeID.BuildingHistory:
                controlPath = "~/PopupControls/BuildingHistory.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("BuildingHistory");
                break;
            case CON.SectionTypeID.RECONSIDERATION:
                controlPath = "~/PopupControls/Reconsideration.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("Reconsideration");
                break;
            case CON.SectionTypeID.ERemittanceAdvice:
                controlPath = "~/PopupControls/ERemittanceAdvice.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                break;
            case CON.SectionTypeID.PrimaryServiceAddress:
                controlPath = "~/PopupControls/PrimaryServiceAddress.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("PrimaryServiceAddress");
                break;
            case CON.SectionTypeID.HospitalAddress:
                controlPath = "~/PopupControls/HospitalAddress.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = pnlConvertedDocs.Visible = false;
                convertedDocs.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("HospitalAddress");
                break;
            case CON.SectionTypeID.NursingFacilityAddress:
                controlPath = "~/PopupControls/NursingFacilityAddress.ascx";
                btnHistory.Visible = true;
                pnlUploadDocsfull.Visible = pnlConvertedDocs.Visible = false;
                convertedDocs.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("NursingFacilityAddress");
                break;
            case CON.SectionTypeID.Address1099Form:
                controlPath = "~/PopupControls/Form1099Address.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = pnlConvertedDocs.Visible = false;
                convertedDocs.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("Address1099Form");
                break;
            case CON.SectionTypeID.HomeOfficeAddress:
                controlPath = "~/PopupControls/HomeOfficeAddress.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = pnlConvertedDocs.Visible = false;
                convertedDocs.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("HomeOfficeAddress");
                break;
            case CON.SectionTypeID.OrgInfo:
                controlPath = "~/PopupControls/OrgInfo.ascx";
                PlaceholderUploadSectionControl.Visible = true;
                ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("OrgInfo");
                break;
            case CON.SectionTypeID.Disclosures:
                controlPath = "~/PopupControls/Disclosures.ascx";
                PlaceholderUploadSectionControl.Visible = true;
                ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("Disclosures");
                break;
            case CON.SectionTypeID.PrimaryContactAddress:
                controlPath = "~/PopupControls/PrimaryContactAddress.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("PrimaryContactInfo");
                break;
            case CON.SectionTypeID.Specialties:
                controlPath = "~/PopupControls/Specialties.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                PlaceholderUploadSectionControl.Visible = true;
                ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("Specialties");
                break;
            case CON.SectionTypeID.SpecialtiesTaxonomies:
                controlPath = "~/PopupControls/Taxonomies.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("SpecialtiesTaxonomies");
                break;
            case CON.SectionTypeID.CertificationsCLIA:
                controlPath = "~/PopupControls/CertSecondGrid.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("CertificationsCLIA");
                break;
            case CON.SectionTypeID.Insurance:
                controlPath = "~/PopupControls/Insurance.ascx";
                ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("Insurance");
                break;
            case CON.SectionTypeID.Licenses:
                controlPath = "~/PopupControls/Licenses.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false; 
                lblContentRender.Text = RenderPage.RenderDynamicContent("Licenses");
                break;
            case CON.SectionTypeID.FederalDEA:
                controlPath = "~/PopupControls/Certifications.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("FederalDEA");
                break;
            case CON.SectionTypeID.NursingProfessionalCertification:
                controlPath = "~/PopupControls/NursingProfessionalCertification.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("NursingProfessionalCertification");
                break;
            case CON.SectionTypeID.NumberOfBeds:
                controlPath = "~/PopupControls/TotalNumberBeds.ascx";
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("NumberOfBeds");
                break;
            case CON.SectionTypeID.CPRCertification:
                controlPath = "~/PopupControls/CPRCertification.ascx";
                pnlUploadDocsfull.Visible = true; //multi upload documents
                ucSep1.Visible = false; //label of "Upload Documents"
                PlaceholderUploadSectionControl.Visible = false; //single upload documents
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("CPRCertification");
                break;
            case CON.SectionTypeID.DentalLicense:
                controlPath = "~/PopupControls/DentalLicense.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("DentalLicense");
                break;
            case CON.SectionTypeID.StateCDSNumber:
                controlPath = "~/PopupControls/CDS.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("StateCDSNumber");
                break;
            case CON.SectionTypeID.VisionProviders:
                controlPath = "~/PopupControls/VisionProviders.ascx";
                btnHistory.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("VisionProviders");
                break;
            case CON.SectionTypeID.CategoryOfService:
                controlPath = "~/PopupControls/CategoryOfService.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("CategoryOfService");
                break;
            case CON.SectionTypeID.BehaviouralHealthInformation:
                controlPath = "~/PopupControls/BehavioralHealthInfo.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("BehaviouralHealthInformation");
                break;
            case CON.SectionTypeID.ReimbursementRates:
                controlPath = "~/PopupControls/Reimbursement.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("ReimbursementRates");
                break;
            case CON.SectionTypeID.Miscellaneous:
                controlPath = "~/PopupControls/MiscellaneousSection.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("Miscellaneous");
                break;
            case CON.SectionTypeID.BillingPaymentAddress:
                if (pageTypeId == CON.RegistrationPageType.PracticeLocations)
                {
                    controlPath = "~/PopupControls/BillingPaymentAddress.ascx";
                    PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                    pnlUploadDocsfull.Visible = true;
                    btnHistory.Visible = false;
                    lblContentRender.Text = RenderPage.RenderDynamicContent("BillingPaymentContactInformation");
                }
                break;
            case CON.SectionTypeID.CorrespondenceAddress:
                controlPath = "~/PopupControls/CorrespondenceAddress.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("CorrespondenceInformation");
                break;
            case CON.SectionTypeID.RemittanceAddress:
                controlPath = "~/PopupControls/RemittanceInformation.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("RemittanceAddress");
                break;
            case CON.SectionTypeID.OtherAddress:
                controlPath = "~/PopupControls/OtherAddress.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = false;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("OtherAddress");
                break;
            case CON.SectionTypeID.OfficeHours: // doesn't exist anymore
                if (this.EntityTypeID == CON.ProviderCategoryTypeID.Individual)
                {
                    controlPath = "~/PopupControls/OfficeHoursIndividual.ascx"; // doesn't exist anymore
                }
                else
                {
                    controlPath = "~/PopupControls/OfficeHours.ascx"; // doesn't exist anymore
                }
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false; // JIRA 4087 as per package 1.19 
                lblContentRender.Text = RenderPage.RenderDynamicContent("OfficeInformation");
                break;
            case CON.SectionTypeID.GroupAndFacilityAffiliations:
                controlPath = "~/PopupControls/GroupAndFacilityAffiliationsView.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("GroupAndFacilityAffiliations");
                break;
            case CON.SectionTypeID.Affiliations:
                controlPath = "~/Pages/GroupAffiliations.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("Affiliations");
                break;
            case CON.SectionTypeID.OwnerInformation:
                controlPath = "~/PopupControls/OwnerInformation.ascx";
                // TODO: EDV Move this panel styling into owner information page
                pnldetails.Attributes.Add("style", "background-color: #ebecee !important; border: none !important; border-top: none !important; border-left: none !important; box-shadow: none !important; padding: 0px !important;");
                //PlaceholderUploadSectionControl.Visible = false;
                PlaceholderUploadSectionControl.Visible = true;
                ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("OwnerInformation");
                break;
            case CON.SectionTypeID.W9Form:
                controlPath = "~/PopupControls/FormW9.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("W9Form");
                break;
            case CON.SectionTypeID.ApplicationFee:
                controlPath = "~/Pages/ApplicationFee.ascx";
                PlaceholderUploadSectionControl.Visible = true;
                ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("ApplicationFee");
                break;
            case CON.SectionTypeID.DME:
                controlPath = "~/PopupControls/DME.ascx";
                ucSep1.Visible = PlaceholderUploadSectionControl.Visible = false;
                ucUploadDocument.Visible = true;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("DME");
                break;
            case CON.SectionTypeID.WaiverServices:
                controlPath = "~/PopupControls/WaiverServices.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                break;
            case CON.SectionTypeID.MalpracticeClaimsHistory:
                controlPath = "~/PopupControls/MalpracticeClaim.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("MalpracticeClaimsHistory");
                break;
            case CON.SectionTypeID.Agreements:
                controlPath = "~/PopupControls/Agreements.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = true;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("Agreements");
                break;
            case CON.SectionTypeID.OtherDocuments:
                controlPath = "~/PopupControls/OtherDocUploadSectionControl.ascx";
                PlaceholderUploadSectionControl.Visible = true;
                ucSep1.Visible = false;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("OtherDocuments");
                break;
            case CON.RegistrationPageType.ProviderScreening:
                screeningType = Enumerations.ScreeningEntityType.Provider;
                controlPath = "~/PopupControls/Screening.ascx";
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("ProviderScreening");
                break;
            case CON.RegistrationPageType.AffiliationScreening:
                screeningType = Enumerations.ScreeningEntityType.Affiliation;
                controlPath = "~/PopupControls/Screening.ascx";
                ucUploadDocument.DocumentSection = string.Empty;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("IndividualMemberScreening");
                break;
            case CON.RegistrationPageType.OwnerScreening:
                screeningType = Enumerations.ScreeningEntityType.Owner;
                controlPath = "~/PopupControls/Screening.ascx";
                ucUploadDocument.DocumentSection = "Owner Screening";
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("OwnerScreening");
                break;
            /*case CON.RegistrationPageType.HouseholdMemberScreening:
                screeningType = Enumerations.ScreeningEntityType.HouseholdMember;
                controlPath = "~/PopupControls/Screening.ascx";
                ucUploadDocument.DocumentSection = string.Empty;
                break;*/
            case CON.RegistrationPageType.SiteVisitScreening:
                controlPath = "~/PopupControls/SiteVisit.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;   // !CanNotEdit - Jira 2532;
                pnlUploadDocs.Visible = true;       // !CanNotEdit - Jira 2532;
                spPageHeader.Visible = true;        // !CanNotEdit - Jira 2532;
                convertedDocs.Visible = !CanNotEdit;
                dvboxcontainer.Visible = !CanNotEdit;
                ucSep1.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("SiteVisitScreening");
                break;
            case CON.RegistrationPageType.OrientationInformation:
                controlPath = "~/PopupControls/Orientation.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("OrientationSessionScreening");
                break;
            case CON.RegistrationPageType.BackgroundCheck:
                controlPath = "~/PopupControls/Background.ascx";
                ucUploadDocument.DocumentSection = "Fingerprint Check";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("FingerprintCheck");
                break;
            case CON.SectionTypeID.PharmacyProviders:
                controlPath = "~/PopupControls/PharmacySection.ascx";
                //((Button)ucRegistrationNavigation.FindControl("btnSave")).ValidationGroup = "valPharmacyProvider";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("PharmacyProviders");
                break;

            case CON.SectionTypeID.ACHAuthorization:
                controlPath = "~/PopupControls/ACHAuthorization.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                PlaceholderUploadSectionControl.Visible = false;    // SAM768 do not show upload control on EFT page.
                ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = false;  // SAM768 do not show upload control on EFT page.
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("ACHAuthorization");
                break;

            case CON.SectionTypeID.SatellitePracticeLocations:
                controlPath = "~/PopupControls/SatellitePracticeLocations.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = false;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("SatelliteLocations");
                break;
            case CON.SectionTypeID.WorkflowSteps:
                controlPath = "~/Pages/WorkflowSteps.ascx";
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("WorkflowSteps");
                break;
            case CON.SectionTypeID.MMISTransactions:
                controlPath = "~/PopupControls/MMISTransactions.ascx";
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("OrgInfo");
                break;

            case CON.SectionTypeID.TransactionQueue:  // JIRA 2961
                controlPath = "~/PopupControls/TransactionQueueProvider.ascx";
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("TransactionQueue");
                break;

            case CON.SectionTypeID.MCOAffiliation:
                controlPath = "~/PopupControls/MCOAffiliations.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("MCOAffiliation");
                break;
            case CON.SectionTypeID.Appeals:
                controlPath = "~/PopupControls/Appeals.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("Appeals");
                break;
            case CON.SectionTypeID.EmploymentHistory:
                // controlPath = "~/PopupControls/EmploymentHistory.ascx";
                controlPath = "~/PopupControls/WorkHistoryDetails.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("EmploymentHistory");
                break;

            case CON.SectionTypeID.WorkHistory:
                // controlPath = "~/PopupControls/WorkHistory.ascx";
                controlPath = "~/PopupControls/Education.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("Education");
                break;
            case CON.SectionTypeID.CredentialingContact:
                controlPath = "~/PopupControls/CredentialingContact.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("CredentialingContact");
                break;
            case CON.SectionTypeID.ProviderCredentialing:
                controlPath = "~/PopupControls/credentialing.ascx";
                ucUploadDocument.DocumentSection = "Provider Credentialing";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("ProviderCredentialing");
                break;
            case CON.SectionTypeID.BoardCertification:
                controlPath = "~/PopupControls/BoardCertification.ascx";
                btnHistory.Visible = true;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("BoardCertification");
                break;
            case CON.SectionTypeID.NursingFacilityVentilator:
                controlPath = "~/PopupControls/NursingFacilityVentilator.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("NursingFacilityVentilator");
                break;
            case CON.SectionTypeID.HearingRights:
                controlPath = "~/PopupControls/119HearingRights.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("HearingRights");
                break;
            case CON.SectionTypeID.RestrictedService:
                controlPath = "~/PopupControls/RestrictedServices.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("RestrictedService");
                break;
            case CON.SectionTypeID.IncidentComplianceReview:
                controlPath = "~/PopupControls/IncidentComplianceReview.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("IncidentComplianceReview");
                break;
            case CON.SectionTypeID.CpcContactInformation:
                controlPath = "~/PopupControls/CPCContactInformation.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("CPCContactInformation");
                break;
            case CON.SectionTypeID.CpcSpecialties:
                controlPath = "~/PopupControls/CPCSpecialties.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("CPCSpecialties");
                break;
            case CON.SectionTypeID.PracticePartnership:
                controlPath = "~/PopupControls/PracticePartnership.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("PracticePartnership");
                break;
            case CON.SectionTypeID.CPCAttestation:
                controlPath = "~/PopupControls/CPCAttestation.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("CPCAttestation");
                break;
            case CON.SectionTypeID.CMCAttestation:
                controlPath = "~/PopupControls/CMCAttestation.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("CMCAttestation");
                break;
            case CON.SectionTypeID.CMCSpecialties:
                controlPath = "~/PopupControls/CMCSpecialties.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("CMCSpecialties");
                break;
            case CON.SectionTypeID.CMCContactInformation:
                controlPath = "~/PopupControls/CPCContactInformation.ascx";
                btnHistory.Visible = false;
                pnlUploadDocsfull.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("CMCContactInformation");
                break;
            case CON.SectionTypeID.NPIandMedId:
                controlPath = "~/PopupControls/NPIandMedId.ascx";
                PlaceholderUploadSectionControl.Visible = ucSep1.Visible = false;
                pnlUploadDocsfull.Visible = false;
                btnHistory.Visible = false;
                lblContentRender.Text = RenderPage.RenderDynamicContent("NPIandMedId");
                break;
            case CON.SectionTypeID.ProviderFeed:
                controlPath = "~/PopupControls/ProviderFeed.ascx";
                btnHistory.Visible = true;
                pnlUploadDocsfull.Visible = true;
                lblContentRender.Text = RenderPage.RenderDynamicContent("ProviderFeed");
                break;
            default:
                break;
        }
        this.spnHelpSection.InnerText = Registration.GetSectionText(Step == 0 ? 1 : Step);
        string sectionName1 = Registration.GetSectionText(Step == 0 ? 1 : Step).Replace(" ", "").Replace("&", "").Replace(",","");
        string url = "HelpInstructions.aspx?SectionName=" + "Help"+sectionName1;

        btnhelp.OnClientClick = "window.open('" + url + "', '_blank'); return false;";

        CreatePlaceholderForControlPath(controlPath, screeningType);
        btnHistory.CommandName = sectionName;
        btnHistory.CommandArgument = step.ToString();
    }

    public override void ucRegistrationNavigation_RefreshEvent(int step)
    {
        Step = step;
        LoadData(Step);
        Master.LoadReturnReasons(this.RegistrationId);
        Master.RefreshTree();

        PlaceholderUploadSectionControl.Controls.Clear();
        LoadPlaceHolder(Step);
        if (this.RegistrationId > 0 && this.RegistrationStep > 0)
        {
            ucUploadDocument.Visible = true;
            ucUploadDocument.LoadData(this.RegistrationId, this.RegistrationStep);
        }
        Master.SetWorkFlowActions(this.WF_StepID);
        SetWorkflowPanel();
    }

    private bool ValidateData(int step)
    {
        return GetSectionControlFromPlaceHolder().ValidateData();
    }
    private bool ValidateSaveNextData()
    {
        return GetSectionControlFromPlaceHolder().ValidateSaveNextData();
    }

    private bool ValidateIsRequiredData(int step)
    {
        // TODO: EDV We need to check if group affiliations is behaving correctly
        // becuase it does the valdiate data instead of HasInputValue here. 
        bool isRequired = false;
        RegistrationNode Nodes = null;
        if (this.RegistrationNodes.ContainsKey(step))
        {
            Nodes = this.RegistrationNodes[step];
        }

        if (step == CON.SectionTypeID.Appeals || (Nodes != null && !Convert.ToBoolean(Nodes.IsRequired)))
            isRequired = GetSectionControlFromPlaceHolder().HasInputValue();//done
        else
            isRequired = true;

        return isRequired;
    }

    void ucRegistrationNavigation_ValidateIsRequiredDataEvent(int step, ref bool isRequired)
    {
        isRequired = ValidateIsRequiredData(step);
    }

    void ucRegistrationNavigation_ValidateDataEvent(int step, ref bool isValid)
    {
        isValid = ValidateData(step);
        // OHPNM-2177 - if the page they are on is the Required Document page and it doesn't validate, be sure to clear any green checkmarks; we should probably do that for all pages, but Jira only spells out this page
        if (!isValid && (Step == CON.SectionTypeID.OtherDocuments))
        {
            Registration.SetProviderSectionNodeStatusId(this.RegistrationId, step, CON.RegistrationProviderStatusTypeId.NotComplete);
            RefreshNavigationTree();
        }

        // BZ #4041 (PSR) - Removing code causing scroll bars to return to their previous position after post-back
        //Page.MaintainScrollPositionOnPostBack = isValid;
        if (PlaceholderUploadSectionControl.Visible)
        {
            foreach (Control ctrl in PlaceholderUploadSectionControl.Controls)
            {
                UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                isValid &= uploadControl.ValidateData("valProviderInfoHeader");
            }
        }

        ValidationHasProcessed = true;
    }
    void ucRegistrationNavigation_ValidateSaveNextDataEvent(int step, ref bool isValid)
    {
        isValid = ValidateSaveNextData();
    }

    private bool ucRegistrationNavigation_SaveDataEvent(int step)
    {
        if (this.RegistrationId == 0)
        {
            //FATAL ERROR
            this.mpeError.Show();
            return false;
        }

        return GetSectionControlFromPlaceHolder().SaveData();
    }

    string ucUploadDocument_RenameFileMethod(string dir, string input)
    {
        string rtn = input;
        int idx = 0;
        while (System.IO.File.Exists(dir + rtn))
        {
            idx += 1;
            int pos = input.LastIndexOf(".");
            if (pos == -1) rtn = input + "_" + idx.ToString();
            else rtn = input.Substring(0, pos) + "_" + idx.ToString() + input.Substring(pos);
        }
        return rtn;
    }

    int ucUploadDocument_SuccessEvent(string fileName)
    {
        int docId = -1;
        string currentDocumentSection = ucUploadDocument.DocumentSection;

        try
        {
            string saveName = fileName;
            int pos = fileName.LastIndexOf("\\");
            if (pos != -1) saveName = fileName.Substring(pos + 1);

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

            int? screeningActivityID = null;
            int Upload_RegPageTypeID = -1;
            string Upload_DocumentSectionName = "";
            if (ucUploadDocument.ScreeningActivityID > 0)
                screeningActivityID = ucUploadDocument.ScreeningActivityID;

            //upload for process appeal
            if (this.CurrentTaskName == CON.RegistrationTaskName.ProcessAppeal)
            {
                this.ucUploadDocument.DocumentSection = CON.RegistrationTaskName.ProcessAppeal;
            }
            else if (this.CurrentTaskName == CON.RegistrationTaskName.ProcessAppealSiteVisit)
            {
                this.ucUploadDocument.DocumentSection = CON.RegistrationTaskName.ProcessAppealSiteVisit;
            }

            // TODO: EDV Remove hardcoding, what is 44??
            if (this.RegistrationStep == 44)
            {
                Upload_RegPageTypeID = 8;
                Upload_DocumentSectionName = "OtherDocuments";
            }
            else if (this.RegistrationStep == 37)
            {
                Upload_RegPageTypeID = 24;
                Upload_DocumentSectionName = "DME";
            }
            else if (this.RegistrationStep == 51)
            {
                Upload_RegPageTypeID = 28;
                Upload_DocumentSectionName = ucUploadDocument.DocumentSection;
            }
            else if (this.RegistrationStep == 17)
            {
                Upload_RegPageTypeID = 17;
                Upload_DocumentSectionName = CON.RegistrationPageName.ApplicationFee;
            }
            else
            {
                Upload_RegPageTypeID = this.RegistrationStep;
                Upload_DocumentSectionName = ucUploadDocument.DocumentSection;
            }

            docId = psc.InsertRegDocument(this.RegistrationId, Upload_RegPageTypeID, Upload_DocumentSectionName, ucUploadDocument.DocumentName,
                ucUploadDocument.DocumentDescription, saveName, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), screeningActivityID, this.CurrentTaskName, this.lblTitle.Text);
            ucUploadDocument.DocumentName = ucUploadDocument.DocumentDescription = string.Empty;

            // Commented this out, since I changed the variable used when loading data.
            //// In the case of a specific screening (E.g OIG/LEIE, the DocumentSection will the name of the screening type)
            //// We want to show all screening types in the grid, so need to change the DocumentSection to "Provider Screening"
            //// and change it back to the current DocumentSection afterward.
            //if (this.CurrentTaskName == CON.RegistrationTaskName.ProviderScreening)
            //{
            //    ucUploadDocument.DocumentSection = CON.RegistrationTaskName.ProviderScreening;
            //}


            // TODO: EDV Remove hardcoding, what is 44??
            if (this.RegistrationStep == 44)
            {
                ucUploadDocument.LoadData(this.RegistrationId, this.RegistrationStep, Upload_DocumentSectionName);
            }
            else
            {
                ucUploadDocument.LoadData(this.RegistrationId, this.RegistrationStep);
            }

            // If we're going to hit the database and load data, let's show the data...  Right???
            pnlUploadDocsfull.Visible = true;
            ucUploadDocument.Visible = true;

            // This was already happening above, based on the scenario.
            // Why would you reload it again, and force it to be loaded differently in the case of that scenario???
            //ucUploadDocument.LoadData(this.RegistrationId, this.RegistrationStep);
            ucUploadDocument.DocumentSection = currentDocumentSection;

            if (!Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))
            {
                // Since the provider uploaded a document, set the status to incomplete
                Registration.SetNodeStatusId(this.RegistrationId, this.RegistrationStep, CON.RegistrationProviderStatusTypeId.NotComplete);
                try
                {
                    Master.RefreshTree();
                }
                catch
                {
                    // TODO: Write to log
                }
            }
        }
        catch
        {
            // Should this log to the event log?
        }
        return docId;
    }

    private void SetWorkflowPanel()
    {
        Master.SetWorkflowPanelVisibility(false);
        bool showDenyBc = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        bool isDoDDIntialApp = psc.CheckIfDODDInitialApplication(this.RegistrationId);
        bool isODAIntialApp = psc.CheckIfODAInitialApplication(this.RegistrationId);

        if (this.RegistrationIdSelected == 0)
        {
            //get financial review status and LTC review status
            int finReviewStatus = 0;
            int LTCReviewStatus = 0;
            int DBHReviewStatus = 0;
            int DDSReviewStatus = 0;
            int DHCFReviewStatus = 0;
            int DHCFApplicationFeeReviewStatus = 0;
            int StateApplicationFeeReviewStatus = 0;
            int DBHReviewerReviewStatus = 0;
            int DDSReviewerReviewStatus = 0;
            int workflowEventTypeID = 0;



            DataSet ds = psc.SelectRegPageStatus(this.RegistrationId);
            bool HasWentThroughScreening = false;
            DataSet dsScreening = psc.SelectProviderScreeningByProcessID(Master.ProcessID);
            if (Helper.HasRows(dsScreening))
            {
                HasWentThroughScreening = true;
            }
            if (Helper.HasRows(ds))
            {
                DataRow[] dr = ds.Tables[0].Select("REG_PAGE_TYPE_ID = " + CON.RegistrationPageType.ProviderScreening.ToString());
                if (this.WF_WorkflowID == CON.WorkflowType.ADHP && this.CurrentTaskName == CON.RegistrationTaskName.LTCReview)
                    dr = ds.Tables[0].Select("REG_PAGE_TYPE_ID = " + CON.RegistrationPageType.Identification.ToString());

                if (this.WF_WorkflowID == CON.WorkflowType.EPD && this.CurrentTaskName == CON.RegistrationTaskName.LTCReview)
                    dr = ds.Tables[0].Select("REG_PAGE_TYPE_ID = " + CON.RegistrationPageType.Identification.ToString());

                if (this.WF_WorkflowID == CON.WorkflowType.RegistrationNew && (this.CurrentTaskName == CON.RegistrationTaskName.DDSReview || this.CurrentTaskName == CON.RegistrationTaskName.DDSReviewerReview))
                    dr = ds.Tables[0].Select("REG_PAGE_TYPE_ID = " + CON.RegistrationPageType.Agreements.ToString());
                if (this.CurrentTaskName == CON.RegistrationTaskName.ReviewApproval || this.CurrentTaskName == CON.RegistrationTaskName.ReviewDenial || this.CurrentTaskName == CON.RegistrationTaskName.ReviewTermination || this.CurrentTaskName == CON.RegistrationTaskName.DenialTerminationNotification)
                    dr = ds.Tables[0].Select("REG_PAGE_TYPE_ID = " + CON.RegistrationPageType.Agreements.ToString());
                if (this.CurrentTaskName == CON.RegistrationTaskName.ReviewApplicationFee || this.CurrentTaskName == CON.RegistrationTaskName.ApplicationFeeReview || this.CurrentTaskName == CON.RegistrationTaskName.ApplicationFeeDenialReview)
                    dr = ds.Tables[0].Select("REG_PAGE_TYPE_ID = " + CON.RegistrationPageType.ApplicationFee.ToString());


                if (dr.Length > 0)
                {
                    finReviewStatus = Helper.GetInt("Financial_REVIEW_STATUS_ID", dr[0]);
                    LTCReviewStatus = Helper.GetInt("LTC_REVIEW_STATUS_ID", dr[0]);
                    DBHReviewStatus = Helper.GetInt("DBH_REVIEW_STATUS_ID", dr[0]);
                    DDSReviewStatus = Helper.GetInt("DDS_REVIEW_STATUS_ID", dr[0]);
                    DHCFReviewStatus = Helper.GetInt("DHCF_REVIEW_STATUS_ID", dr[0]);
                    DHCFApplicationFeeReviewStatus = Helper.GetInt("DHCF_APPLICATION_FEE_REVIEW_STATUS_ID", dr[0]);
                    StateApplicationFeeReviewStatus = Helper.GetInt("STATE_APPLICATION_FEE_REVIEW_STATUS_ID", dr[0]);
                    DBHReviewerReviewStatus = Helper.GetInt("DBH_REVIEWER_REVIEW_STATUS_ID", dr[0]);
                    DDSReviewerReviewStatus = Helper.GetInt("DDS_REVIEWER_REVIEW_STATUS_ID", dr[0]);
                }
            }

            // pre hide some buttons
            Master.SetActionVisibility("Submit Update", false);
            Master.SetActionVisibility("Approve", false);
            Master.SetActionVisibility("Approved", false);
            Master.SetActionVisibility("Not Processed", false);

            if (Helper.IsUserInAccountingRolls(HttpContext.Current.User.Identity.Name))
            {
                // If Accounting has finished review and on the ACH page
                if (
                    Registration.GetPageStatusId(this.RegistrationId, CON.RegistrationPageType.ApplicationFee) != CON.RegistrationProviderServicesStatusTypeId.Pending &&
                    this.RegistrationStep == CON.RegistrationPageType.ApplicationFee)
                {
                    // Hide the buttons based upon if approved or not
                    if (
                        Registration.IsPageApproved(this.RegistrationId, CON.RegistrationPageType.ApplicationFee))
                    {
                        Master.SetActionVisibility("Return to Provider", false);
                        Master.SetActionVisibility("Return to Provider Services", false);
                        Master.SetActionVisibility("Return to TennCare Accounting", false);
                    }
                    else
                    {
                        Master.SetActionVisibility("Forward to F&A", false);
                        Master.SetActionVisibility("Approve (w/o F&A Review)", false);
                        Master.SetActionVisibility("Approved", false);
                    }
                    Master.SetWorkflowPanelVisibility(true);
                }
            }
            else if (Helper.IsUserInDIDDRoles(HttpContext.Current.User.Identity.Name))
            {
                // DIDD Operator
                if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.DIDDOperator) &&
                    !Registration.IsPagePending(this.RegistrationId, CON.RegistrationPageType.Services))
                {
                    // Hide the buttons based upon if approved or not
                    if (Registration.IsPageApproved(this.RegistrationId, CON.RegistrationPageType.Services))
                        Master.SetActionVisibility("Return to Provider", false);
                    else Master.SetActionVisibility("Approve", false);
                    Master.SetWorkflowPanelVisibility(true);
                }
                else if (!Registration.IsPagePending(this.RegistrationId, CON.RegistrationPageType.Contracts))
                {
                    // DIDDCommissioner, ProviderServicesCommissioner
                    Master.SetWorkflowPanelVisibility(true);
                }
            }
            else if (Registration.IsRegistrationScreeningTaskType(Master.TaskName))
            {
                this.SetScreeningWorkActionVisibility();
            }
            else if (Master.TaskName == "Application Fee Review" && !Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name))
            {
                //added for bug5987
                if (Registration.IsSectionApproved(this.RegistrationId, CON.RegistrationPageType.ApplicationFee) && Step == CON.RegistrationPageType.ApplicationFee)
                {
                    if (Registration.GetSectionStatusId(this.RegistrationId, CON.RegistrationPageType.ApplicationFee) == CON.RegistrationProviderServicesStatusTypeId.Approved)
                    {
                        Master.SetWorkflowPanelVisibility(true);
                        Master.SetActionVisibility("Approve Application Fee", true);
                        Master.SetActionVisibility("Return to Provider", false);
                    }
                }
                else
                {
                    Master.SetWorkflowPanelVisibility(false);
                    if (Registration.GetSectionStatusId(this.RegistrationId, CON.RegistrationPageType.ApplicationFee) == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider)
                    {
                        Master.SetWorkflowPanelVisibility(true);
                        Master.SetActionVisibility("Approve Application Fee", false);
                        Master.SetActionVisibility("Return to Provider", true);
                    }
                }
            }
            else if ((Master.TaskName == "DBH Provider Review") && Helper.IsUserInDBHOperatorRole(HttpContext.Current.User.Identity.Name))
            {
                if (DBHReviewStatus == CON.DBHReviewStatusID.Approve)
                {
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetActionVisibility("Approve", true);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                }
                else if (DBHReviewStatus == CON.DBHReviewStatusID.Deny || DBHReviewStatus == CON.DBHReviewStatusID.Terminate)
                {
                    if (DBHReviewStatus == CON.DBHReviewStatusID.Terminate)
                    {
                        Master.SetActionVisibility("Deny", false);
                        Master.SetActionVisibility("Terminate", true);
                    }
                    else
                    {
                        Master.SetActionVisibility("Deny", true);
                        Master.SetActionVisibility("Terminate", false);
                    }
                    Master.SetActionVisibility("Approve", false);
                    Master.SetWorkflowPanelVisibility(true);
                }
            }
            else if ((Master.TaskName == "DBH Reviewer Review") && Helper.IsUserInDBHReviewerRole(HttpContext.Current.User.Identity.Name))
            {
                Master.SetWorkflowPanelVisibility(true);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToScreening, true);
                Master.SetActionVisibility("Approve", false);
                Master.SetActionVisibility("Deny", false);
                Master.SetActionVisibility("Terminate", false);

                if (DBHReviewerReviewStatus == CON.DBHReviewStatusID.Approve)
                {
                    Master.SetActionVisibility("Approve", true);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                }
                else if (DBHReviewerReviewStatus == CON.DBHReviewStatusID.Deny || DBHReviewerReviewStatus == CON.DBHReviewStatusID.Terminate)
                {
                    if (DBHReviewerReviewStatus == CON.DBHReviewStatusID.Terminate)
                    {
                        Master.SetActionVisibility("Deny", false);
                        Master.SetActionVisibility("Terminate", true);
                    }
                    else
                    {
                        Master.SetActionVisibility("Deny", true);
                        Master.SetActionVisibility("Terminate", false);
                    }
                    Master.SetActionVisibility("Approve", false);
                }
            }
            else if (Master.TaskName == "Financial Review" && Helper.IsUserInORFAWorkerRole(HttpContext.Current.User.Identity.Name))
            {
                if (finReviewStatus == CON.FinancialReviewStatusID.Completed)
                {
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetActionVisibility("Approve", true);
                }
            }
            else if (Master.TaskName == "LTC (Readiness Review)" && Helper.IsUserInLTCWorkerRole(HttpContext.Current.User.Identity.Name))
            {
                if (LTCReviewStatus == CON.LTCReviewStatusID.Approve)
                {
                    if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                    {
                        Master.SetActionVisibility("Approve", false);
                        //as per CR-32
                        DataSet dsReg = psc.SelectRegistrationByRegID(this.RegistrationId);
                        if (Helper.HasRows(dsReg))
                        {
                            DataRow rowTerm = dsReg.Tables[0].Rows[0];
                            string termStatus = Helper.GetString("EnrollmentStatusCode", rowTerm);
                            if (termStatus != CON.EnrollmentStatusCode.Active)
                            {
                                Master.SetActionVisibility("Refer to State", true);
                                Master.SetActionVisibility("Send to MMIS", false);
                            }
                            else
                            {
                                DataRow provRow = Registration.GetProviderInfo(this.RegistrationId);
                                if (provRow != null)
                                {
                                    int applicationTypeID = Helper.GetInt("APPLICATION_TYPE_ID", provRow);

                                    Master.SetActionVisibility("Refer to State", false);

                                    Master.SetActionVisibility("Send to MMIS", true);
                                    Master.SetActionVisibility("Approve", false);

                                }
                            }
                        }

                    }
                    else
                    {
                        Master.SetActionVisibility("Approve", true);
                        Master.SetActionVisibility("Refer to State", false);
                        Master.SetActionVisibility("Send to MMIS", false);
                    }
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                    Master.SetWorkflowPanelVisibility(true);
                }
                else if (LTCReviewStatus == CON.LTCReviewStatusID.Deny || LTCReviewStatus == CON.LTCReviewStatusID.Terminate)
                {
                    if (!string.IsNullOrEmpty(this.MedicaidID))
                    {
                        Master.SetActionVisibility("Terminate", true);
                        Master.SetActionVisibility("Deny", false);
                    }
                    else
                    {
                        Master.SetActionVisibility("Deny", true);
                        Master.SetActionVisibility("Terminate", false);
                    }
                    Master.SetActionVisibility("Approve", false);
                    Master.SetActionVisibility("Refer to State", false);
                    Master.SetActionVisibility("Send to MMIS", false);
                    Master.SetWorkflowPanelVisibility(true);
                }
                else
                {
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetActionVisibility("Approve", false);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                    Master.SetActionVisibility("Refer to State", false);
                    Master.SetActionVisibility("Send to MMIS", false);
                }

                if ((workflowEventTypeID == CON.WorkflowEventType.UpdateReg && !HasWentThroughScreening) || (workflowEventTypeID != CON.WorkflowEventType.UpdateReg && !HasWentThroughScreening))
                {
                    Master.SetActionVisibility("Return To Screening", false);
                    Master.SetActionVisibility("Return To Provider Review", true);
                }
                else
                {
                    Master.SetActionVisibility("Return To Screening", true);
                    Master.SetActionVisibility("Return To Provider Review", false);
                }
            }
            else if ((Master.TaskName == "Review Approval" || Master.TaskName == "Review Denial" || Master.TaskName == "Review Termination" || Master.TaskName == "Denial/Termination Notification") && Helper.IsUserInStateReviewerRole(HttpContext.Current.User.Identity.Name))
            {
                if (DHCFReviewStatus == CON.DHCFReviewStatusID.Approve)
                {
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetActionVisibility("Approve", true);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                }
                else if (DHCFReviewStatus == CON.DHCFReviewStatusID.Deny || DHCFReviewStatus == CON.DHCFReviewStatusID.Terminate)
                {
                    if (DHCFReviewStatus == CON.DHCFReviewStatusID.Terminate)
                    {
                        Master.SetActionVisibility("Terminate", true);
                        Master.SetActionVisibility("Deny", false);
                    }
                    else
                    {
                        Master.SetActionVisibility("Deny", true);
                        Master.SetActionVisibility("Terminate", false);
                    }
                    Master.SetActionVisibility("Approve", false);
                    Master.SetWorkflowPanelVisibility(true);
                }
                else
                {
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetActionVisibility("Approve", false);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                }

                if ((workflowEventTypeID == CON.WorkflowEventType.UpdateReg && !HasWentThroughScreening) || (workflowEventTypeID != CON.WorkflowEventType.UpdateReg && !HasWentThroughScreening))
                {
                    Master.SetActionVisibility("Return To Screening", false);
                    Master.SetActionVisibility("Return To Provider Review", true);
                    Master.SetWorkflowPanelVisibility();
                }
                else
                {
                    Master.SetActionVisibility("Return To Provider Review", false);
                    Master.SetActionVisibility("Return To Screening", true);
                    Master.SetWorkflowPanelVisibility();
                }

            }
            else if ((Master.TaskName == "Review Application Fee") && Helper.IsUserInStateReviewerRole(HttpContext.Current.User.Identity.Name))
            {
                if (DHCFApplicationFeeReviewStatus == CON.DHCFApplicationFeeReviewStatusID.Approve)
                {
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetActionVisibility("Recommend Hardship Approval", true);
                    Master.SetActionVisibility("Recommend Hardship Denial", false);
                }
                else if (DHCFApplicationFeeReviewStatus == CON.DHCFApplicationFeeReviewStatusID.Deny)
                {
                    Master.SetActionVisibility("Recommend Hardship Denial", true);
                    Master.SetActionVisibility("Recommend Hardship Approval", false);
                    Master.SetWorkflowPanelVisibility(true);
                }
            }
            else if ((Master.TaskName == "Application Fee Review" || Master.TaskName == "Application Fee Denial Review") && Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name))
            {
                if (StateApplicationFeeReviewStatus == CON.StateApplicationFeeReviewStatusID.Approve)
                {
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetActionVisibility("Approve Application Fee", true);
                    Master.SetActionVisibility("Deny Application Fee", false);
                }
                else if (StateApplicationFeeReviewStatus == CON.StateApplicationFeeReviewStatusID.Deny)
                {
                    Master.SetActionVisibility("Deny Application Fee", true);
                    Master.SetActionVisibility("Approve Application Fee", false);
                    Master.SetWorkflowPanelVisibility(true);
                }
            }
            else if (((Master.TaskName == CON.BackgroundCheckTaskNames.FingerprintandBackgroundEntry) || Master.TaskName == CON.BackgroundCheckTaskNames.FingerprintandBackgroundComplete
                        || Master.TaskName == CON.BackgroundCheckTaskNames.FingerprintandBackgroundPending) && Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))
            {

                Master.SetWorkflowPanelVisibility(true);
                //DataSet ds2 = psc.SelectRegistrationData(this.RegistrationId, "BACKGROUND_CHECK");
                DataTable dataTable = psc.SelectRegistrationData(this.RegistrationId, "BACKGROUND_CHECK").Tables[0].Select("VERSIONHISTORY IS NULL").CopyToDataTable();
                // this commented code below is from OHPNM-4547; those requirements don't make sense, so Kristen and Michelle are reviewing; why would we enable the send notices button under the conditions in the Jira?
                // enabling that button prevents us from ever showing the background check complete button
                // DataRow[] drSendNotice = ds2.Tables[0].Select("IsBackgroundCheckRequired = 1  or (IsBackgroundCheckRequired = 0 and BACKGROUND_STATUS_TYPE_ID IN (1))");
                // putting back the old logic to allow OPHNM-4848 to move forward
                DataRow[] drSendNotice = dataTable.Select("BACKGROUND_RESULT_TYPE_ID IN (3, 0) AND BACKGROUND_STATUS_TYPE_ID IN (2)");
                DataRow[] drFailed = dataTable.Select("BACKGROUND_RESULT_TYPE_ID IN (2,4)");
                DataRow[] drPass = dataTable.Select("BACKGROUND_RESULT_TYPE_ID IN (1)");
                DataRow[] drPoorQualityPrints = dataTable.Select("BACKGROUND_RESULT_TYPE_ID IN (5)");
                DataRow[] drPoorQualityPrintsNew = dataTable.Select("IsPoorQualityPrints = true");

                //SAM505
                bool showSendBackgroundNoticebutton = true;
                DataSet ds1 = psc.WF_SelectProcessParameters(this.WF_ProcessID);
                if (Helper.HasRows(ds1))
                {
                    DataRow dr = ds1.Tables[0].Rows[0];
                    showSendBackgroundNoticebutton = dr[CON.ProcessParameter.IsAddBCIRTPEmail].ToString() == CON.BCITextRTPEmail.Sent ? false : true;
                }

                if (drSendNotice.Length > 0)
                {
                    if (!showSendBackgroundNoticebutton)
                    {
                        Master.SetActionVisibility("Send All Background Check Notices", false);
                    }
                    else
                    {
                        Master.SetActionVisibility("Send All Background Check Notices", true);
                    }

                    Master.SetActionVisibility("Background Check Complete", false);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);

                }
                else if (drFailed.Length > 0)
                {


                    if (this.WorkflowEventTypeId == CON.WorkflowEventType.NewReg || this.WorkflowEventTypeId == CON.WorkflowEventType.ChangeProviderType)
                    {
                        showDenyBc = true;
                    }
                    Master.SetActionVisibility("Send All Background Check Notices", false);
                    Master.SetActionVisibility("Background Check Complete", false);

                    Master.SetActionVisibility("Terminate", !showDenyBc);
                    Master.SetActionVisibility("Return To Provider", false);
                }
                else if (drPoorQualityPrints.Length > 0)
                {
                    Master.SetActionVisibility("Send All Background Check Notices", false);
                    Master.SetActionVisibility("Background Check Complete", false);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                }
                else if (drPass.Length > 0 && drPass.Length == dataTable.Rows.Count)
                {
                    var regPagesStatus = psc.SelectRegPageStatus(this.RegistrationId);
                    if (Helper.HasRows(regPagesStatus))
                    {
                        var bgCheckRow = from row in regPagesStatus.Tables[0].AsEnumerable()
                                         where Helper.GetInt("REG_PAGE_TYPE_ID", row) == CON.RegistrationPageType.BackgroundCheck
                                         select row;

                        if (bgCheckRow != null && bgCheckRow.Count() > 0 && Helper.GetString("ProviderServicesStatus", bgCheckRow.FirstOrDefault()) == "Approved")
                            Master.SetActionVisibility("Background Check Complete", true);
                        else
                            Master.SetActionVisibility("Background Check Complete", false);
                    }
                    else
                        Master.SetActionVisibility("Background Check Complete", false);

                    Master.SetActionVisibility("Send All Background Check Notices", false);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                }
                else
                {
                    Master.SetActionVisibility("Send All Background Check Notices", false);
                    Master.SetActionVisibility("Background Check Complete", false);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                }
                Master.SetActionVisibility("Confirm Poor Quality Fingerprints", false);
                if (drPoorQualityPrintsNew.Length > 0)
                {
                    Master.SetActionVisibility("Confirm Poor Quality Fingerprints", true);
                }

                if (Master.TaskName == CON.BackgroundCheckTaskNames.FingerprintandBackgroundPending && Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))
                {
                    Master.SetActionVisibility("Send To Application Disposition", false);
                }

            }
            else if ((Master.TaskName == "Orientation Session Screening") && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.Operator))
            {
                Master.SetWorkflowPanelVisibility(true);
                DataSet ds1 = psc.SelectRegistrationData(this.RegistrationId, "ORIENTATION");
                int OrientationStatusTypeID = 0;
                if (Helper.HasRows(ds1))
                    OrientationStatusTypeID = Helper.GetInt("ORIENTATION_STATUS_TYPE_ID", ds1.Tables[0].Rows[0]);
                if (Registration.GetPageStatusId(this.RegistrationId, CON.RegistrationPageType.OrientationInformation) == CON.RegistrationProviderServicesStatusTypeId.Approved && OrientationStatusTypeID == CON.OrientationStatusType.Completed)
                {
                    Master.SetActionVisibility("Approve", true);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                }
                else if (Registration.GetPageStatusId(this.RegistrationId, CON.RegistrationPageType.OrientationInformation) == CON.RegistrationProviderServicesStatusTypeId.Approved && OrientationStatusTypeID == CON.OrientationStatusType.Failed)
                {
                    Master.SetActionVisibility("Approve", false);
                    if (!string.IsNullOrEmpty(this.MedicaidID))
                        Master.SetActionVisibility("Terminate", true);
                    else
                        Master.SetActionVisibility("Deny", true);
                }
                else
                {
                    Master.SetActionVisibility("Approve", false);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                }
            }
            else if ((Master.TaskName == CON.RegistrationTaskName.DDSReview) && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.DDSOperatorRole))
            {
                Master.SetWorkflowPanelVisibility(true);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToProviderReview, false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToScreening, false);
                if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg && !HasWentThroughScreening)
                {
                    Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToProviderReview, true);
                }
                else
                {
                    Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToScreening, true);
                }
                Master.SetActionVisibility("Approve", false);
                Master.SetActionVisibility("Refer to State", false);
                Master.SetActionVisibility("Deny", false);
                Master.SetActionVisibility("Terminate", false);
                Master.SetActionVisibility("Send to MMIS", false);

                if (DDSReviewStatus == CON.DDSReviewStatusID.Approve)
                {
                    if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg)
                    {
                        Master.SetActionVisibility("Approve", false);
                        //as per CR-32
                        DataSet dsReg = psc.SelectRegistrationByRegID(this.RegistrationId);
                        if (Helper.HasRows(dsReg))
                        {
                            DataRow rowTerm = dsReg.Tables[0].Rows[0];
                            string termStatus = Helper.GetString("EnrollmentStatusCode", rowTerm);
                            if (termStatus != CON.EnrollmentStatusCode.Active)
                            {
                                Master.SetActionVisibility("Refer to State", true);
                                Master.SetActionVisibility("Send to MMIS", false);
                            }
                            else
                            {
                                Master.SetActionVisibility("Refer to State", false);
                                if (Registration.HasPrimaryPracticeLocationChange(this.RegistrationId))
                                {
                                    Master.SetActionVisibility("Approve", true);
                                    Master.SetActionVisibility("Send to MMIS", false);
                                }
                                else
                                {
                                    Master.SetActionVisibility("Send to MMIS", true);
                                    Master.SetActionVisibility("Approve", false);
                                }
                            }
                        }
                    }
                    else
                    {
                        Master.SetActionVisibility("Approve", true);
                        Master.SetActionVisibility("Refer to State", false);
                        Master.SetActionVisibility("Send to MMIS", false);
                    }
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                }
                else if (DDSReviewStatus == CON.DDSReviewStatusID.Deny || DDSReviewStatus == CON.DDSReviewStatusID.Terminate)
                {
                    if (!string.IsNullOrEmpty(this.MedicaidID))
                    {
                        Master.SetActionVisibility("Deny", false);
                        Master.SetActionVisibility("Terminate", true);
                    }
                    else
                    {
                        Master.SetActionVisibility("Deny", true);
                        Master.SetActionVisibility("Terminate", false);
                    }
                    Master.SetActionVisibility("Approve", false);
                    Master.SetActionVisibility("Refer to State", false);
                    Master.SetActionVisibility("Send to MMIS", false);
                }

            }
            else if ((Master.TaskName == "DDS Reviewer Review") && Helper.IsUserInDDSReviewerRole(HttpContext.Current.User.Identity.Name))
            {
                Master.SetWorkflowPanelVisibility(true);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToProviderReview, false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToScreening, false);
                if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg && !HasWentThroughScreening)
                {
                    Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToProviderReview, true);
                }
                else
                {
                    Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToScreening, true);
                }
                Master.SetActionVisibility("Approve", false);
                Master.SetActionVisibility("Deny", false);
                Master.SetActionVisibility("Terminate", false);
                if (DDSReviewerReviewStatus == CON.DDSReviewStatusID.Approve)
                {
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetActionVisibility("Approve", true);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                }
                else if (DDSReviewerReviewStatus == CON.DDSReviewStatusID.Deny || DDSReviewerReviewStatus == CON.DDSReviewStatusID.Terminate)
                {
                    if (!string.IsNullOrEmpty(this.MedicaidID))
                    {
                        Master.SetActionVisibility("Deny", false);
                        Master.SetActionVisibility("Terminate", true);
                    }
                    else
                    {
                        Master.SetActionVisibility("Deny", true);
                        Master.SetActionVisibility("Terminate", false);
                    }
                    Master.SetActionVisibility("Approve", false);
                    Master.SetWorkflowPanelVisibility(true);
                }
            }
            else if ((Master.TaskName == "Record Termination") && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.Operator))
            {
                // Check for termination date and status
                DataSet dsReg = psc.SelectRegistrationByRegID(this.RegistrationId);
                if (Helper.HasRows(dsReg))
                {
                    DataRow rowTerm = dsReg.Tables[0].Rows[0];
                    string termDate = Helper.GetString("TerminationDate", rowTerm);
                    string termStatus = Helper.GetString("EnrollmentStatusCode", rowTerm);

                    if (!string.IsNullOrEmpty(termDate) && !string.IsNullOrEmpty(termStatus))
                    {
                        Master.SetWorkflowPanelVisibility(true);
                        Master.SetActionVisibility("Record Termination", true);
                    }
                    else
                    {
                        Master.SetWorkflowPanelVisibility(false);
                    }
                }
                else
                    Master.SetWorkflowPanelVisibility(false);
            }
            else if (Master.TaskName == "Deny Hardship Request" && Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name))
            {
                Master.SetWorkflowPanelVisibility(true);
            }
            else if (Master.TaskName == "Upload Initial Notice of Denial" || Master.TaskName == "Upload Initial Notice of Termination")
            {
                Master.SetWorkflowPanelVisibility(false);
                Master.SetActionVisibility("Pending Denial/Termination", false);
                if (this.RegistrationStep == CON.SectionTypeID.Appeals && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.AppealSpecialist))
                {
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetActionVisibility("Pending Denial/Termination", false);
                    if (IsInitialNoticeSent())
                    {
                        Master.SetActionVisibility("Pending Denial/Termination", true);
                    }
                }
            }
            else if (Master.TaskName == "Upload Final Notice of Denial")
            {
                Master.SetWorkflowPanelVisibility(false);
                Master.SetActionVisibility("Deny", false);
                if (this.RegistrationStep == CON.SectionTypeID.Appeals && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.AppealSpecialist))
                {
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetActionVisibility("Deny", false);
                    if (IsFinalNoticeSent())
                    {
                        Master.SetActionVisibility("Deny", true);
                    }
                }
            }
            else if (Master.TaskName == "Upload Final Notice of Termination")
            {
                Master.SetWorkflowPanelVisibility(false);
                Master.SetActionVisibility("Terminate", false);
                if (this.RegistrationStep == CON.SectionTypeID.Appeals && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.AppealSpecialist))
                {
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetActionVisibility("Terminate", false);
                    if (IsFinalNoticeSent())
                    {
                        Master.SetActionVisibility("Terminate", true);
                    }
                }
            }
            else if (Registration.IsComplianceTaskType(Master.TaskName) && Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name))
            {
                this.SetComplianceWorkActionVisibility();
            }
            else if (Master.TaskName == "Transaction Monitoring")
            {
                Master.SetWorkflowPanelVisibility(true);
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                DataSet dsReg = svc.SelectRegistrationByRegID(this.RegistrationId);
                DataRow provRow = dsReg.Tables[0].Rows.Count > 0 ? dsReg.Tables[0].Rows[0] : null;
                if (provRow != null)
                {
                    int applicationTypeID = Helper.GetInt("ApplicationTypeID", provRow);
                    int waiverTypeID = Helper.GetInt("WaiverTypeID", provRow);
                    int WaiverServiceUpdateTypeID = Helper.GetInt("WaiverServiceUpdateTypeID", provRow);
                    if ((applicationTypeID == CON.ApplicationType.Waiver && (waiverTypeID == CON.WaiverApplicationTypeID.ODA || waiverTypeID == CON.WaiverApplicationTypeID.DODD))
                        || (WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD || WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.ODA)
                       )
                    {
                        Master.SetActionVisibility("Resend Transaction", false);
                        if (waiverTypeID == CON.WaiverApplicationTypeID.ODA || WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.ODA)
                        {
                            Master.SetActionVisibility("ReSend DODD Transaction", false);
                        }
                        else
                        {
                            Master.SetActionVisibility("ReSend ODA Transaction", false);
                        }
                    }
                    else
                    {
                        Master.SetActionVisibility("Resend Waiver Transaction", false);
                    }
                    if (Helper.GetString("EnrollmentStatusCode", provRow) == CON.EnrollStatus.INACTIVE.ToString())
                    {
                        Master.SetActionVisibility("Return To Review", false);
                    }
                }

            }
            else if (Master.TaskName == "Contract Maintenance")
            {
                Master.SetWorkflowPanelVisibility(true);
                DataRow provRow = Registration.GetProviderInfo(this.RegistrationId);
                int applicationTypeID = Helper.GetInt("APPLICATION_TYPE_ID", provRow);
                if (applicationTypeID == CON.ApplicationType.Standard || applicationTypeID == CON.ApplicationType.ORP || applicationTypeID == CON.ApplicationType.Waiver || applicationTypeID == CON.ApplicationType.ChangeOfOperator)
                {
                    if (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg)
                    {
                        Master.SetActionVisibility("Accept", true);
                        Master.SetActionVisibility("Approve", false);
                    }
                    else
                    {
                        Master.SetActionVisibility("Approve", true);
                        Master.SetActionVisibility("Accept", false);
                    }
                }
                else
                {
                    Master.SetActionVisibility("Approve", true);
                    Master.SetActionVisibility("Accept", false);
                }
            }
            /*else if (Registration.IsSiteVisitTaskType(Master.TaskName))
            {
                
            }*/
            else
            {
                if ((Master.TaskName == CON.BackgroundCheckTaskNames.FingerprintandBackgroundPending || Master.TaskName == CON.BackgroundCheckTaskNames.FingerprintAndBackgroundReSubmit) && Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))
                {
                    Master.SetActionVisibility("Send To Application Disposition", false);
                }

                bool isRegistrationComplete = false;

                bool approved = Registration.CheckRegistrationComplete(this.RegistrationId, this.RegistrationNodes, this.CurrentTaskName, this.WorkflowEventTypeId, ref isRegistrationComplete, this.WF_WorkflowID, this.IsAddODMorODAMedSvc, this.IsReactivation, Helper.IsRevertSuspensionWF(this.WF_ProcessID));
                if (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && Registration.IsCurrentAssignedUserForStep(Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.WF_StepID) && this.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry)
                {
                    Master.SetUpdateMessageVisibility(true);
                }
                else if (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && currentStatus == CON.RegistrationProgramStatusTypeId.Conversion)
                {
                    Master.SetUpdateMessageVisibility(true);
                }
                else
                {
                    Master.SetUpdateMessageVisibility(false);
                }

                // get registration status
                int status = 0;
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                ds = svc.SelectRegistration(this.RegistrationId);
                if (Helper.HasRows(ds))
                {
                    int.TryParse(Helper.GetData("REGISTRATION_STATUS_TYPE_ID", ds.Tables[0].Rows[0]), out status);
                }

                // OHPNM-2723 - disable plan of correction button
                if (status != CON.RegistrationStatusTypeId.ReturnToProviderForSiteVisit)
                {
                    Master.SetActionVisibility("Plan of Correction", false);
                }

                if (this.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry && (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) || Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name)))
                {
                    // OHPNM-2544 - since they are in ProviderDataEntry mode, need to enable top workflow panel to show any buttons that are enabled
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetGeneratePDFPanelVisibility(true);
                    Master.SetActionVisibility("Submit Update", false);//OHPNM-3649

                    if (status == CON.RegistrationStatusTypeId.ReturnToProviderForSiteVisit)
                    {
                        Master.SetActionVisibility("Submit for Review", false);
                    }
                }
                if (isRegistrationComplete)
                {
                    Master.SetWorkflowPanelVisibility(true);
                    if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
                    {
                        Master.SetGeneratePDFPanelVisibility(true);
                        Master.SetActionVisibility("Submit Update", false);

                        if (this.WF_WorkflowID != CON.WorkflowType.CMC && this.WorkflowEventTypeId != CON.WorkflowEventType.UpdateReg && status != CON.RegistrationStatusTypeId.ReturnToProviderForSiteVisit)
                            mpeChangesSaved.Show();

                        if (this.WF_WorkflowID == CON.WorkflowType.CPC && this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg)
                        {
                            Master.SetActionVisibility("Submit for Review", false);
                            Master.SetActionVisibility("Submit Update", true);
                        }
                        if (!Helper.IsUpdateCPCContact)
                        {
                            Master.SetActionVisibility("Submit Contact Info Update", false);
                        }
                        if (Helper.IsUpdateCPCContact)
                        {
                            Master.SetActionVisibility("Submit for Review", false);
                            Master.SetActionVisibility("Submit Update", false);
                            Master.SetActionVisibility("Submit Contact Info Update", true);
                        }
                        if (this.WF_WorkflowID == CON.WorkflowType.CMC && (this.WorkflowEventTypeId == CON.WorkflowEventType.CMCEnroll || this.WorkflowEventTypeId == CON.WorkflowEventType.CMCReAttest))
                        {
                            //modalCMC.Show();
                            Master.SetActionVisibility("Submit for Review", true);
                        }
                    }
                    else
                    {
                        if (approved)
                        {
                            Master.SetActionVisibility("Return to Provider", false);
                            Master.SetActionVisibility("Deny", false);
                            Master.SetActionVisibility("Approve", false);
                            Master.SetActionVisibility("Disapprove", false);
                            Master.SetActionVisibility("Create LT Enrollment", false);

                            if ((this.MMISProviderTypeID == "86" || this.MMISProviderTypeID == "89" || this.MMISProviderTypeID == "88") && !(this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg || this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg))
                            {
                                if (this.ApplicationTypeID == CON.ApplicationType.ChangeOfOperator && this.WF_WorkflowID == CON.WorkflowType.CHOP)
                                {
                                    Master.SetActionVisibility("Application Complete", true);
                                }
                                else
                                {
                                    Master.SetActionVisibility("Application Complete", false);
                                    Master.SetActionVisibility("Approve Enrollment", false);
                                    Master.SetActionVisibility("Create LT Enrollment", false);
                                }

                            }
                            if (this.MMISProviderTypeID == "99" && this.WF_WorkflowID == CON.WorkflowType.CPC)
                            {
                                Master.SetActionVisibility("Deny", true);
                                Master.SetActionVisibility("Approve", true);
                                Master.SetActionVisibility("Return to Provider", false);
                            }
                            if (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && Master.TaskName.Equals(CON.RegistrationTaskName.ProviderReview) && this.IsAddODMorODAMedSvc)
                            {
                                Master.SetActionVisibility("Approve", true);
                                Master.SetActionVisibility("Application Complete", false);
                            }
                        }
                        else
                        {
                            Master.SetActionVisibility("Return to Provider", true);
                            Master.SetActionVisibility("Approve", false);

                            Master.SetActionVisibility("Approve Provider", false);
                            Master.SetActionVisibility("Application Complete", false);
                            Master.SetActionVisibility("Approve Profile", false);
                            if ((this.MMISProviderTypeID == "86" || this.MMISProviderTypeID == "89" || this.MMISProviderTypeID == "88") &&
                                (this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg || this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg || Helper.IsLoggedInUserInODMCredentialingQualityAssuranceRole() || Helper.IsLoggedInUserInCredentialingQualityAssuranceRole()))
                            {
                                Master.SetActionVisibility("Create LT Enrollment", false);
                            }
                            DataSet ds1 = psc.WF_SelectStepInfo(this.WF_StepID);
                            string stepOwnerID = "";
                            if (Helper.HasRows(ds1))
                            {
                                stepOwnerID = Helper.GetString("STEP_OWNER_ID", ds1.Tables[0].Rows[0]);
                            }
                            if (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && this.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry &&
                                Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) && SessionVarRetriever.UserIdSelected == stepOwnerID && !this.isEnrollmentSpecialist)
                            {
                                Master.SetActionVisibility("Submit for Review", false);
                            }
                            else
                            {
                                Master.SetActionVisibility("Submit Update", false);
                            }
                            if (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && this.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry && this.WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.OperatorUpdate &&
                                SessionVarRetriever.UserIdSelected == stepOwnerID && (Helper.IsUserInLTCCHOPRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInLTCInitialRevalidationRole(HttpContext.Current.User.Identity.Name)))
                            {
                                Master.SetActionVisibility("Submit Update", true);
                                Master.SetActionVisibility("Submit for Review", false);
                            }
                            if (this.MMISProviderTypeID == "99" && this.WF_WorkflowID == CON.WorkflowType.CPC)
                            {
                                Master.SetActionVisibility("Deny", true);
                                Master.SetActionVisibility("Approve", false);
                            }
                        }
                        if (Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name))
                        {
                            if (workflowEventTypeID == CON.WorkflowEventType.UpdateReg && !HasWentThroughScreening)
                            {
                                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToScreening, false);
                                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToProviderReview, true);
                            }
                            else
                            {
                                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToScreening, true);
                                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToProviderReview, false);
                            }
                        }

                    }

                }
                else if (this.CurrentTaskName == CON.RegistrationTaskName.RetroReview)
                {
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToScreening, true);
                    Master.SetActionVisibility("Return to Provider", false);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Approve", false);
                    Master.SetActionVisibility("Approve Provider", false);
                    Master.SetActionVisibility("Application Complete", false);
                    Master.SetActionVisibility("Approve Profile", false);
                }
                else
                {
                    // OHPNM-2544 - the registration is not done, make sure Submit for Review button is not enabled
                    Master.SetActionVisibility("Submit for Review", false);

                    // OHPNM-7214 - the CPC contact update is not done, make sure the submit button is off
                    Master.SetActionVisibility("Submit Contact Info Update", false);
                }
                if (Registration.IsSiteVisitTaskType(Master.TaskName))
                {
                    this.SetSiteVisitScreeningWorkActionVisibility();
                }
            }
        }
        else if (((MasterWorkflowPage)this.Master).ViewProviderFile == true)
        {
            if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
            {
                Master.SetGeneratePDFPanelVisibility(true);
                Master.SetWorkflowPanelVisibility(false);
                Master.SetBacktoSummaryPanelVisibility(false);
            }
        }
        //need a generic method for canIworkThis.
        if (Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name) && (Registration.IsStateAdminTaskType(this.CurrentTaskName)))
        {
            Master.SetWorkflowPanelVisibility(false);
            this.SetStateReviewWorkActionVisibility();
        }
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingChair) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CommitteeQualitySpecialist))
        {
            Master.SetWorkflowPanelVisibility(false);
            //string credentialUpdated=string.Empty;
            //  using(PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
            //  {
            //      credentialUpdated = svc.GetReviewStatusByCommiteeMember(this.RegistrationId, HttpContext.Current.User.Identity.Name.ToString());
            //  }

            //      if (credentialUpdated == CON.CommitteeCredentialActivityStatusId.Pending.ToString())
            //      {
            //          Master.SetWorkflowPanelVisibility(true);
            //          Master.SetActionVisibility("Review Complete", false);
            //          Master.SetActionVisibility("Deny", false);
            //      }
            //      else if (credentialUpdated == CON.CommitteeCredentialActivityStatusId.Fail)
            //      {
            //          Master.SetWorkflowPanelVisibility(true);

            //          if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, "CredentialCommitteeChair"))
            //          {
            //              Master.SetActionVisibility("Review Complete", false);
            //              Master.SetActionVisibility("Deny", true);
            //          }
            //      }
            //      else
            //      {
            //          //pass or approve with restrictions
            //          Master.SetWorkflowPanelVisibility(true);
            //          Master.SetActionVisibility("Review Complete", true);
            //          Master.SetActionVisibility("Deny", false);
            //      }

        }
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSpecialist) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSpecialist))
        {
            if ((this.CurrentTaskName == CON.RegistrationTaskName.ProviderCredentialing) || (this.CurrentTaskName == CON.RegistrationTaskName.ProviderCredentialingODM))
            {
                Master.SetWorkflowPanelVisibility(false);
                using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
                {
                    Master.SetWorkflowPanelVisibility(true);
                    if (svc.HasPendingCredentialActivities(this.RegistrationId))
                    {
                        Master.SetActionVisibility("Process Discontinue", true);
                        Master.SetActionVisibility("Return to Provider", true);
                        Master.SetActionVisibility("Approve", false);
                    }
                    else
                    {
                        if (svc.IsRiskLevelAssigned(this.RegistrationId))
                        {
                            Master.SetActionVisibility("Approve", true);
                            Master.SetActionVisibility("Return to Provider", true);
                            Master.SetActionVisibility("Process Discontinue", true);
                        }
                        else
                        {
                            Master.SetActionVisibility("Process Discontinue", false);
                            Master.SetActionVisibility("Approve", false);
                            Master.SetActionVisibility("Return to Provider", false);
                        }
                    }
                }
            }
        }
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSupervisor) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSupervisor))
        {
            if ((this.CurrentTaskName == CON.RegistrationTaskName.CredentialingSupervisorReview) || (this.CurrentTaskName == CON.RegistrationTaskName.ODMCredentialingSupervisorReview))
            {
                //OHPNM 3692
                Master.SetWorkflowPanelVisibility(true);
                Master.SetActionVisibility("Process Discontinue", true);
                Master.SetActionVisibility("Approve", true);
                Master.SetActionVisibility("Return To Credential Specialist", true);
                if (string.IsNullOrEmpty(this.MedicaidID))
                {
                    RegMaster.SetActionVisibility("Admin Terminate", false);
                }
                else
                {
                    RegMaster.SetActionVisibility("Admin Terminate", true);
                }
            }
        }
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSupervisor) && (this.CurrentTaskName == CON.RegistrationTaskName.CredentialingSupervisorReview))
        {
            //Check if reg is in New Enroll
            if (this.WorkflowEventTypeId == CON.WorkflowEventType.NewReg || this.WorkflowEventTypeId == CON.WorkflowEventType.ChangeProviderType)
            {
                RegMaster.SetActionVisibility("Admin Deny", false);
            }
        }

        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingQuality))
        {
            //Master.SetWorkflowPanelVisibility(false);
            if (this.CurrentTaskName == CON.RegistrationTaskName.CredentialingQualityReview)
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, this.RegistrationId, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_Credentialing", sqlParms, "RegData");

                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                int credentialRiskLevel = ObjectControllerHelper.GetInt("RISK_LEVEL_ID", drReg);

                if (credentialRiskLevel == 1)
                {
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetActionVisibility("Send to ODM Chair", true);
                    Master.SetActionVisibility("Send to Committee", false);
                    Master.SetActionVisibility("QA Approve", false);
                    Master.SetActionVisibility("QA Fail", true);
                }
                else
                {
                    Master.SetWorkflowPanelVisibility(true);
                    Master.SetActionVisibility("QA Approve", true);
                    Master.SetActionVisibility("Send to ODM Chair", false);
                    Master.SetActionVisibility("Send to Committee", false);
                    Master.SetActionVisibility("QA Fail", true);

                }


            }
        }
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialing))
        {
            //Master.SetWorkflowPanelVisibility(false);
            if (this.CurrentTaskName == CON.RegistrationTaskName.ODMCredentialingReview)
            {
                int credentialStatus = 0;
                Master.SetWorkflowPanelVisibility(true);
                using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
                {
                    DataSet ds = svc.SelectProviderCredentialingData(this.RegistrationId);
                    if (Helper.HasRows(ds))
                    {
                        credentialStatus = Convert.ToInt32(ds.Tables[0].Rows[0]["CREDENTIALING_STATUS_ID"]);
                        if (credentialStatus == CON.CredentilaingStatus.ProcessDiscontinued)
                            Master.SetActionVisibility("Enrollment Approved", true);
                        else
                            Master.SetActionVisibility("Enrollment Approved", false);
                    }

                }
                Master.SetActionVisibility("Return to Credentialing", true);
                Master.SetActionVisibility("Approve for Committee", true);
            }
        }


        if ((Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.LTCInitialRevalidation) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.TechAdmin)) && Master.WorkflowID == CON.WorkflowType.RiskAlertClosure)
        {
            Master.SetWorkflowPanelVisibility(false);

            SetRiskClosureActionVisibility();

        }
        if ((Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCCHOP) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.TechAdmin) || Helper.IsLoggedInUserInCredentialingQualityAssuranceRole()) && Master.WorkflowID == CON.WorkflowType.RiskAlertCHOP)
        {
            Master.SetWorkflowPanelVisibility(false);

            SetRiskCHOPActionVisibility();

        }

        // OHPNM-6853 - moved these two higher to get the isAppPagesApproved data for next section in regards to ProviderReview
        bool isRegistrationCompleted = false;
        bool isAllPagesApproved = Registration.CheckRegistrationComplete(this.RegistrationId, this.RegistrationNodes, this.CurrentTaskName, this.WorkflowEventTypeId, ref isRegistrationCompleted, this.WF_WorkflowID, this.IsAddODMorODAMedSvc, this.IsReactivation, Helper.IsRevertSuspensionWF(this.WF_ProcessID));

        // OHPNM-12268
        if ((this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg) &&
            ((Master.WorkflowID != CON.WorkflowType.PeriodicDatabaseChecks && !Registration.IsComplianceTaskType(this.CurrentTaskName) && this.CurrentTaskName != "") || currentStatus == CON.RegistrationProgramStatusTypeId.Conversion))
        {
            if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.Administrator))
                Master.SetBacktoSummaryPanelVisibility(!this.IsAddODMorODAMedSvc);
            else
                Master.SetBacktoSummaryPanelVisibility(false);

            //if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
            //    Master.SetProgressBarVisibility(false);
            //Hide Back to summary for site visit operator as per kelly's request in update registration
            if (Helper.IsUserInSiteVisitOperatorRole(HttpContext.Current.User.Identity.Name))
            {
                Master.SetBacktoSummaryPanelVisibility(false);
            }
            if (Master.TaskName.Equals(CON.RegistrationTaskName.PlaceRiskAlertClosure)
                || (Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ComplianceSpecialist) || Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name)
                  || (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.APMSpecialist) && this.WF_WorkflowID == CON.WorkflowType.CPC)) &&
                (Master.TaskName.Equals(CON.RegistrationTaskName.ProviderReview) || Master.TaskName.Equals(CON.RegistrationTaskName.ProviderScreening)
                   || Master.TaskName.Equals(CON.RegistrationTaskName.ContractMaintenance)
                   || Registration.IsSiteVisitTaskType(Master.TaskName)))
            {
                Master.SetActionVisibility("Application Complete", false);
                if (Master.TaskName.Equals(CON.RegistrationTaskName.ProviderScreening))
                {
                    // provider screening takes care of enabling the workflow panel visibility if it needs it
                    SetScreeningWorkActionVisibility();
                }
                else
                {
                    if (!this.IsAddODMorODAMedSvc && isAllPagesApproved)
                    {
                        Master.SetWorkflowPanelVisibility(true); // OHPNM-7124 and OHPNM-6853 - need to turn button panel on to show Approve button, if applicable
                        Master.SetActionVisibility("Approve", true);
                    }

                    if (Master.TaskName.Equals(CON.RegistrationTaskName.ProviderReview))
                    {
                        using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
                        {
                            DataSet dsDisenroll = svc.SelectProviderDisenrollment(this.RegistrationId);

                            if (!Helper.HasRows(dsDisenroll) || (dsDisenroll.Tables[0].Rows[0]["IS_PROCESSED"] != DBNull.Value && dsDisenroll.Tables[0].Rows[0]["IS_PROCESSED"] != null && Convert.ToBoolean(dsDisenroll.Tables[0].Rows[0]["IS_PROCESSED"])))
                            {
                                Master.SetActionVisibility("Review Disenrollment Request", false);
                            }

                        }
                    }

                }
                Master.SetActionVisibility("Deny", true);
                Master.SetBacktoSummaryPanelVisibility(false);
                if ((this.MMISProviderTypeID == CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR || this.MMISProviderTypeID == CON.LTCProviderTypes.STATE_OPERATED_ICF_MR || this.MMISProviderTypeID == CON.LTCProviderTypes.NursingFacility) &&
                    (Master.TaskName.Equals(CON.RegistrationTaskName.ProviderReview) &&
                    this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg) &&
                    this.ApplicationTypeID == CON.ApplicationType.ChangeOfOperator)
                    Master.SetActionVisibility("Approve", false);

            }
            else if (Master.TaskName != "Provider Credentialing" && Master.TaskName != "Provider Credentialing ODM" && Master.TaskName != CON.ComplianceTaskName.RecordHearingStatus)
            {
                Master.SetActionVisibility("Approve", false);
                Master.SetActionVisibility("Disapprove", false);
            }
            if (Registration.IsSiteVisitTaskType(Master.TaskName))
            {
                this.SetSiteVisitScreeningWorkActionVisibility();
            }
        }

        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ComplianceSpecialist) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.Administrator))
        {
            if (Master.TaskName == CON.BackgroundCheckTaskNames.FingerprintandBackgroundEntry || Master.TaskName == CON.BackgroundCheckTaskNames.FingerprintandBackgroundComplete
                        || Master.TaskName == CON.BackgroundCheckTaskNames.FingerprintandBackgroundPending)
            {
                Master.SetActionVisibility("Deny", showDenyBc);
            }

        }
        else
        {
            if (!(Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.APMSpecialist) && this.MMISProviderTypeID == "99" && this.WF_WorkflowID == CON.WorkflowType.CPC))
                Master.SetActionVisibility("Deny", false);
        }

        if ((this.WorkflowName == "Incident Compliance") && (Master.TaskName == "Incident Compliance Review") && Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name))
        {
            Master.SetWorkflowPanelVisibility(false);

            if (Helper.ShowActionsButtions)
            {
                Master.SetActionVisibility("End Review", true);
                Master.SetActionVisibility("Return to Provider", true);
                Master.SetActionVisibility("Terminate", true);
                Master.SetWorkflowPanelVisibility(true);
            }
            else
            {
                Master.SetActionVisibility("End Review", false);
                Master.SetActionVisibility("Return to Provider", false);
                Master.SetActionVisibility("Terminate", false);
            }
        }
        Master.SetActionVisibility("Submit Waiver Update", false);
		bool IsProviderDisenrolling = false;
		
        if ((Master.TaskName == "Provider Review") && (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAdministrator) || IsPowerAgent ||
            Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.EnrollmentSpecialist)) || (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.Administrator) && (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg)))
        {
            using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
            {

                DataSet ds = psc.WF_SelectProcessParameters(Master.ProcessID);
                if (Helper.HasRows(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    IsProviderDisenrolling = string.IsNullOrEmpty(dr[CON.ProcessParameter.IsProviderDisenrolling].ToString()) ? false : Convert.ToBoolean(dr[CON.ProcessParameter.IsProviderDisenrolling]);
                }

                DataSet dsDisenroll = svc.SelectProviderDisenrollment(this.RegistrationId);
                // OHPNM-7014 - if provider has disenrollment data and it's a disenrollment from the provider
                if (Helper.HasRows(dsDisenroll) && IsProviderDisenrolling)
                {
                    string TermDateRequested = Helper.GetString("DISENROLLMENT_DATE", dsDisenroll.Tables[0].Rows[0]);
                    if (!string.IsNullOrEmpty(TermDateRequested))
                    {
                        bool isProcessed = Helper.GetBool("IS_PROCESSED", dsDisenroll.Tables[0].Rows[0]);
                        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAdministrator) || IsPowerAgent)
                        {
                            Master.SetActionVisibility("Review Disenrollment Request", false);
                            Master.SetActionVisibility("Application Complete", false);
                        }
                        else
                        {
                            //OHPNM-9390
                            if (!isProcessed)
                                Master.SetActionVisibility("Review Disenrollment Request", true);
                            Master.SetActionVisibility("Refer To Compliance", false);
                            Master.SetActionVisibility("Application Complete", false);
                            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Approve, false);
                            Master.SetWorkflowPanelVisibility(true);
                        }
                    }
                }
                else
                {
                    Master.SetActionVisibility("Review Disenrollment Request", false);
                }
            }

        }

        //OHPNM-8106 - SSA Application cannot RTP
        if ((Master.TaskName == "Provider Review" || Master.TaskName == "Provider Screening") && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.EnrollmentSpecialist))
        {
            DataSet dsSSA = psc.SelectAdditionalApplications(this.RegistrationId);
            if (dsSSA.Tables[0].Rows.Count != 0)
            {
                Master.SetActionVisibility("Return to Provider", false);
            }
        }

        //RG 11/21/2023 OHPNM-14215 - commenting as there is code below to make Approve Enrollment true for this condition and because of this the ApplicationComplete/Approve button is not getting shown even though they approve all the pages
        //if (isAllPagesApproved && (Master.TaskName == "Provider Review") && (this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg))
        //{
        //    Master.SetWorkflowPanelVisibility(false);
        //    if (!Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.LTCInitialRevalidation))
        //    {
        //        Master.SetWorkflowPanelVisibility(true);
        //        Master.SetActionVisibility("Approve Enrollment", true);
        //    }
        //}
        int returnCnt = this.RegistrationNodes.Where(s => s.Value.StatusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider).Count();
        if ((Master.TaskName == "Provider Review") && (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg))
        {
            Master.SetWorkflowPanelVisibility(true);

            // OHPNM-14004 - added code to not show the Refer to Compliance and Return to Provider buttons if they are disenrolling
            if (returnCnt > 0 && !IsProviderDisenrolling)
            {
                Master.SetActionVisibility("Refer To Compliance", true);
                Master.SetActionVisibility("Return to Provider", true);
                Master.SetActionVisibility("Approve", false);
            }
            else
            {
                Master.SetActionVisibility("Refer To Compliance", false);
                Master.SetActionVisibility("Return to Provider", false);
            }
            if (!isAllPagesApproved && (Master.TaskName == "Provider Review") && (this.MMISProviderTypeID == "86" || this.MMISProviderTypeID == "88" || this.MMISProviderTypeID == "89"))
            {
                Master.SetActionVisibility("Approve Enrollment", false);
            }
        }

        if (isAllPagesApproved && (Master.TaskName == "Provider Review") && (this.WorkflowEventTypeId == CON.WorkflowEventType.NewReg || this.WorkflowEventTypeId == CON.WorkflowEventType.ChangeProviderType))
        {
            if (!Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.LTCInitialRevalidation))
            {
                Master.SetWorkflowPanelVisibility(true);
                Master.SetActionVisibility("Refer To Compliance", false);
            }
        }

        // if we are in provider review, and all pages have been approved, and this is a reconsideration, and it's MMIS provider type 86, 88, or 89, we need to enable the Approve Enrollment button
        if (isAllPagesApproved && (Master.TaskName == "Provider Review") && (this.WorkflowEventTypeId == CON.WorkflowEventType.Reconsideration || this.WorkflowEventTypeId == CON.WorkflowEventType.CredentialReconsideration)
            && (this.MMISProviderTypeID == CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR || this.MMISProviderTypeID == CON.LTCProviderTypes.STATE_OPERATED_ICF_MR || this.MMISProviderTypeID == CON.LTCProviderTypes.NursingFacility))
        {
            Master.SetWorkflowPanelVisibility(true);
            Master.SetActionVisibility("Approve Enrollment", true);
        }
        if (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
        {
            Master.SetBacktoSummaryPanelVisibility(true);
        }
        else
        {
            Master.SetBacktoSummaryPanelVisibility(false);
        }

        if (isAllPagesApproved && (Master.TaskName == "Provider Review") && (this.MMISProviderTypeID == "86" || this.MMISProviderTypeID == "89" || this.MMISProviderTypeID == "88") && (this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg))
        {
            Master.SetWorkflowPanelVisibility(true);
            Master.SetActionVisibility("Approve Enrollment", true);
            Master.SetActionVisibility("Create LT Enrollment", false);
            Master.SetActionVisibility("Application Complete", false);
        }

        //OHPNM-11908
        if (isAllPagesApproved && (Master.TaskName == "Provider Review") && (this.MMISProviderTypeID == "76") && (this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg)
            && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.EnrollmentSpecialist))
        {
            Master.SetWorkflowPanelVisibility(true);
            Master.SetActionVisibility("Refer To Compliance", false);
            Master.SetActionVisibility("Application Complete", true);
            Master.SetActionVisibility("Approve Enrollment", false);
            Master.SetActionVisibility("Return to Provider", false);
            Master.SetActionVisibility("Approve", false);
        }

        if (isAllPagesApproved && (Master.TaskName == "Provider Review") && (this.MMISProviderTypeID == "86" || this.MMISProviderTypeID == "89" || this.MMISProviderTypeID == "88") && (this.WorkflowEventTypeId == CON.WorkflowEventType.NewReg))
        {
            Master.SetWorkflowPanelVisibility(true);
            Master.SetActionVisibility("Refer To Compliance", false);
            Master.SetActionVisibility("Application Complete", true);
            Master.SetActionVisibility("Approve Enrollment", false);
            Master.SetActionVisibility("Create LT Enrollment", false);

            if (this.MMISProviderTypeID == "86" || this.MMISProviderTypeID == "89" || this.MMISProviderTypeID == "88")
            {

                DataSet ds = psc.WF_SelectProcessParameters(Master.ProcessID);
                if (Helper.HasRows(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    bool HasApplicationCompleteBeenPressed = string.IsNullOrEmpty(dr[CON.ProcessParameter.ApplicationComplete].ToString()) ? false : true; // it just has to have a value in it to be "true"

                    if (HasApplicationCompleteBeenPressed)
                    {
                        Master.SetActionVisibility("Application Complete", false);
                        Master.SetActionVisibility("Create LT Enrollment", true);
                    }
                }
            }

        }
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]) && (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAdministrator)
                 || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAgent)))
            {
                Master.SetWorkflowPanelVisibility(false);
                Master.SetBacktoSummaryPanelVisibility(false);
            }
        }
        //Check for preventing return to provider for ODA or DODD type updates 
        if (((this.ApplicationTypeID == CON.ApplicationType.Waiver) && (this.WaiverTypeID == CON.WaiverApplicationTypeID.ODA || this.WaiverTypeID == CON.WaiverApplicationTypeID.DODD)
            && this.WorkflowEventTypeId == CON.WorkflowEventType.NewReg)
            || (this.WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.DODD || this.WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.ODA))
        {
            Master.SetActionVisibility("Return to Provider", false);
        }

        /*  OHPNM-9388:
            If registration is in a wf and -

            If in System or Email task do not show Action buttons
            If in a Queue task and is not assigned to any user do not show Action buttons
            If in a Queue task and assigned to user, only show Action buttons for this user.
        */
        Guid loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        if (this.WF_TaskType == "System" || this.WF_TaskType == "Email" || (this.WF_TaskType == "Queue" && (!Registration.IsCurrentAssignedUserForStep(loggedInUserId, this.WF_StepID) || this.WF_StepOwner == "")))
        {
            // disable workflow buttons
            Master.SetWorkflowPanelVisibility(false);
        }


        // OHPNM-8886 - If this is a reconsideration workflow then you can't return to provider
        if (this.WorkflowEventTypeId == CON.WorkflowEventType.Reconsideration || this.WorkflowEventTypeId == CON.WorkflowEventType.CredentialReconsideration)
        {
            Master.SetActionVisibility("Return to Provider", false);
        }


        if (((this.CurrentTaskName == CON.RegistrationTaskName.CredentialingSupervisorReview) || (this.CurrentTaskName == CON.RegistrationTaskName.ODMCredentialingSupervisorReview))
            && (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSupervisor) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSupervisor)
                || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, "ODMStateAdministrator")
                || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.EnrollmentSpecialist))
            && Registration.IsCurrentAssignedUserForStep(Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.WF_StepID))
        {
            if ((((this.WorkflowEventTypeId != CON.WorkflowEventType.RevalReg || this.IsProviderReactivation || this.IsReapplication)
                 && this.WorkflowEventTypeId != CON.WorkflowEventType.UpdateReg)
                 || (this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg && !IsReapplication && !IsReactivation && AppSettings.Get("SAM615Enabled") == "true")
                || (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && (!isDoDDIntialApp && !isDoDDIntialApp) && (WaiverServiceUpdateTypeID != CON.WaiverServiceUpdateType.DODD && WaiverServiceUpdateTypeID != CON.WaiverServiceUpdateType.ODA) && AppSettings.Get("SAM615Enabled") == "true"))
                 && this.WorkflowEventTypeId != CON.WorkflowEventType.Reconsideration && this.WorkflowEventTypeId != CON.WorkflowEventType.CredentialReconsideration
                 )
            {
                Master.SetActionVisibility("Not Processed", true);
                Master.SetWorkflowPanelVisibility(true);
            }
            else
            {
                Master.SetActionVisibility("Not Processed", false);
            }
        }

        // OHPNM-13337/OHPNM-13336  --> only for Provider review and New Enrollment.
        if (Master.TaskName.Equals(CON.RegistrationTaskName.ProviderReview) && (this.WorkflowEventTypeId == CON.WorkflowEventType.NewReg || this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg || this.WorkflowEventTypeId == CON.WorkflowEventType.ChangeProviderType))
        {
            if (!isAllPagesApproved) // if all pages not approved dont show below buttons itself.
            {
                Master.SetActionVisibility("Refer To Compliance", false);
                Master.SetActionVisibility("Application Complete", false);
                Master.SetActionVisibility("Approve Enrollment", false);
                Master.SetActionVisibility("Return to Provider", false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.LTEnrollment, false);
            }
            //checking rejected pages count
            //int returnCnt = this.RegistrationNodes.Where(s => s.Value.StatusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider).Count();
            if (returnCnt > 0)
            {
                Master.SetWorkflowPanelVisibility(true);
                Master.SetActionVisibility("Refer To Compliance", true);
                Master.SetActionVisibility("Return to Provider", true);
                Master.SetActionVisibility("Application Complete", false);
                Master.SetActionVisibility("Approve Enrollment", false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.LTEnrollment, false);
            }
            else
            {
                Master.SetActionVisibility("Refer To Compliance", false);
                Master.SetActionVisibility("Return to Provider", false);
            }

        }
        //Show Not Processed button Provider Review
        if (Master.TaskName.Equals(CON.RegistrationTaskName.ProviderReview) &&
            (Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name)
           || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, "ODMStateAdministrator")
           || Helper.IsUserInLTCCHOPRole(HttpContext.Current.User.Identity.Name)
           || Helper.IsUserInLTCInitialRevalidationRole(HttpContext.Current.User.Identity.Name)) //LTC workflow
          && Registration.IsCurrentAssignedUserForStep(Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.WF_StepID))
        {
            if ((this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg && (IsReapplication))
                || (this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg && !IsReapplication && !IsReactivation && AppSettings.Get("SAM615Enabled") == "true")
                || (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && (isDoDDIntialApp || isODAIntialApp))
                || (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && (!isDoDDIntialApp && !isDoDDIntialApp) && (WaiverServiceUpdateTypeID != CON.WaiverServiceUpdateType.DODD && WaiverServiceUpdateTypeID != CON.WaiverServiceUpdateType.ODA) && AppSettings.Get("SAM615Enabled") == "true")
                || this.WorkflowEventTypeId == CON.WorkflowEventType.NewReg || this.WorkflowEventTypeId == CON.WorkflowEventType.ChangeProviderType
                || IsProviderReactivation)
            {
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.NotProcessed, true);
                Master.SetWorkflowPanelVisibility(true);
            }

        }
        //Show Not Processed button BCII workflow
        if ((this.CurrentTaskName == CON.BackgroundCheckTaskNames.FingerprintandBackgroundEntry
                  || this.CurrentTaskName == CON.BackgroundCheckTaskNames.FingerprintandBackgroundPending)
          && Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name)
          && Registration.IsCurrentAssignedUserForStep(Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.WF_StepID))
        {
            if ((this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg && IsReapplication)
                || (this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg && !IsReapplication && !IsReactivation && AppSettings.Get("SAM615Enabled") == "true")
               || (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && (isDoDDIntialApp || isODAIntialApp))
               || (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && (!isDoDDIntialApp && !isDoDDIntialApp) && (WaiverServiceUpdateTypeID != CON.WaiverServiceUpdateType.DODD && WaiverServiceUpdateTypeID != CON.WaiverServiceUpdateType.ODA) && AppSettings.Get("SAM615Enabled") == "true")
               || this.WorkflowEventTypeId == CON.WorkflowEventType.NewReg || this.WorkflowEventTypeId == CON.WorkflowEventType.ChangeProviderType)
            {
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.NotProcessed, true);
                Master.SetWorkflowPanelVisibility(true);
            }
        }
        //Show Not Processed button Site Visit
        if (Registration.IsSiteVisitTaskType(this.CurrentTaskName)
              && (
                    Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name)
                    || Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, "ODMStateAdministrator")
                    || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSpecialist)
                    || Helper.IsUserInSiteVisitOperatorRole(HttpContext.Current.User.Identity.Name)
              )
              && Registration.IsCurrentAssignedUserForStep(Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.WF_StepID))
        {
            if ((this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg && IsReapplication)
                || (this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg && !IsReapplication && !IsReactivation && AppSettings.Get("SAM615Enabled") == "true")
               || (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && (isDoDDIntialApp || isODAIntialApp))
               || (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && (!isDoDDIntialApp && !isODAIntialApp) && (WaiverServiceUpdateTypeID != CON.WaiverServiceUpdateType.DODD && WaiverServiceUpdateTypeID != CON.WaiverServiceUpdateType.ODA) && AppSettings.Get("SAM615Enabled") == "true")
               || this.WorkflowEventTypeId == CON.WorkflowEventType.NewReg || this.WorkflowEventTypeId == CON.WorkflowEventType.ChangeProviderType)
            {
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.NotProcessed, true);
                Master.SetWorkflowPanelVisibility(true);
            }
        }
        //OHPNM-14372
        if ((this.WorkflowEventTypeId == CON.WorkflowEventType.NewReg || this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg || this.WorkflowEventTypeId == CON.WorkflowEventType.ChangeProviderType)
             && Master.TaskName == "Provider Review" && this.WF_WorkflowID == CON.WorkflowType.RegistrationNew)
        {
            Master.SetActionVisibility("Approve", false);
        }
    }

    private void UpdateProviderStatusNotComplete(int actionType)
    {
        Guid changedBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.RegistrationId, "PAGE_STATUS");
        DataSet ds1 = psc.SelectRegistrationData(this.RegistrationId, "SECTION_STATUS");
        if (!Helper.HasRows(ds)) return;
        if (actionType == CON.RegistrationStatusTypeId.ReturnToProviderServices)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                // If not approved then mark the Provider status and Not Complete
                int regPageTypeID = 0;
                regPageTypeID = Helper.GetInt("REG_PAGE_TYPE_ID", row);
                ViewState["PAGE"] = regPageTypeID;
                if (Helper.GetInt("REG_PROVIDER_SERVICES_STATUS_TYPE_ID", row) == CON.RegistrationProviderServicesStatusTypeId.Approved
                    && (regPageTypeID == CON.RegistrationPageType.ProviderScreening || regPageTypeID == CON.RegistrationPageType.OwnerScreening || regPageTypeID == CON.RegistrationPageType.AffiliationScreening
                    || regPageTypeID == CON.RegistrationPageType.HouseholdMemberScreening))
                {
                    psc.SaveRegistrationPageStatus(this.RegistrationId,
                        Helper.GetInt("REG_PAGE_TYPE_ID", row), CON.RegistrationProviderStatusTypeId.NotComplete, CON.RegistrationProviderServicesStatusTypeId.Pending, changedBy.ToString(), null, null, null, null, null, null, null, null, null, null);
                }
            }
            return;
        }
        if (actionType == CON.RegistrationStatusTypeId.ReturnToSiteVisit)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                // If not approved then mark the Provider status and Not Complete
                int regPageTypeID = 0;
                regPageTypeID = Helper.GetInt("REG_PAGE_TYPE_ID", row);
                if (Helper.GetInt("REG_PROVIDER_SERVICES_STATUS_TYPE_ID", row) == CON.RegistrationProviderServicesStatusTypeId.Approved
                    && (regPageTypeID == CON.RegistrationPageType.SiteVisitScreening))
                {
                    psc.SaveRegistrationPageStatus(this.RegistrationId,
                        Helper.GetInt("REG_PAGE_TYPE_ID", row), CON.RegistrationProviderStatusTypeId.NotComplete, CON.RegistrationProviderServicesStatusTypeId.Pending, changedBy.ToString(), null, null, null, null, null, null, null, null, null, null);
                }
            }
            return;
        }
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            // If not approved then mark the Provider status and Not Complete
            if (Helper.GetInt("REG_PROVIDER_SERVICES_STATUS_TYPE_ID", row) != CON.RegistrationProviderServicesStatusTypeId.Approved)
            {
                psc.SaveRegistrationPageStatus(this.RegistrationId,
                    Helper.GetInt("REG_PAGE_TYPE_ID", row), CON.RegistrationProviderStatusTypeId.NotComplete, null, changedBy.ToString(), null, null, null, null, null, null, null, null, null, null);
            }
        }
        if (!Helper.HasRows(ds1)) return;
        foreach (DataRow row in ds1.Tables[0].Rows)
        {
            // If not approved then mark the Provider status and Not Complete
            if (Helper.GetInt("REG_PROVIDER_SERVICES_STATUS_TYPE_ID", row) == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider && Helper.GetInt("REG_PROVIDER_STATUS_TYPE_ID", row) != CON.RegistrationProviderStatusTypeId.NotComplete)
            {
                Registration.SetProviderSectionNodeStatusId(this.RegistrationId, Helper.GetInt("REG_SECTION_TYPE_ID", row), CON.RegistrationProviderStatusTypeId.NotComplete);

            }
        }
        //if ((actionType == CON.RegistrationStatusTypeId.ReturnToProvider) && (this.CurrentTaskName != CON.RegistrationTaskName.ProviderCredentialing || this.CurrentTaskName != CON.RegistrationTaskName.ProviderCredentialingODM))//&& Master.RegistrationTreeViewContains(CON.RegistrationPageName.Contracts)
        //{
        //    psc.ResetContractSignatures(this.RegistrationId, DateTime.Now, changedBy);
        //}
    }

    private void SetSiteVisitScreeningWorkActionVisibility()
    {
        Master.SetWorkflowPanelVisibility(true);
        int isScreeningCompleteid = Registration.SiteVisitComplete(this.RegistrationId);
        bool isScreeningComplete = false;
        if (isScreeningCompleteid == CON.ScreeningStatusId.Complete || this.WF_WorkflowID == CON.WorkflowType.SiteVistEvent)
            isScreeningComplete = true;



        DataRow row = Registration.GetRegistration(this.RegistrationId);
        bool showDeny = true;
        if (row != null)
        {
            if (Helper.GetInt("WORKFLOW_EVENT_TYPE_ID", row) == CON.WorkflowEventType.RevalReg || Helper.GetInt("WORKFLOW_EVENT_TYPE_ID", row) == CON.WorkflowEventType.UpdateReg)
            {
                showDeny = false;
            }
            else
                showDeny = true;
        }

        if (isScreeningCompleteid == CON.ScreeningStatusId.Failed)
        {
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReferToState, true);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Approve, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ApproveUpdate, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Deny, showDeny);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Terminate, !showDeny);
        }
        else
        {
            if (Registration.IsSiteVisitTaskType(Master.TaskName))
            {
                Master.SetActionVisibility("Refer To State", false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Deny, false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Terminate, false);
            }
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();


        DataSet ds2 = psc.SelectSiteVisitDataByRegID(this.RegistrationId);
        bool SiteVisitWFBtnsDisplay = false;
        bool inProgressSiteVisitsExists = false;
        bool enrolledProvider = psc.CheckEnrolledProvider(this.RegistrationId);
        if (Helper.HasRows(ds2))
        {
            DataRow[] drComments = ds2.Tables[0].Select("COMMENTS = '' OR COMMENTS IS NULL");
            DataRow[] drSPComments = ds2.Tables[0].Select("SPECIALIST_COMMENTS = '' OR SPECIALIST_COMMENTS IS NULL");
            DataRow[] drRecommendations = ds2.Tables[0].Select("SITE_VISIT_RECOMMENDATION_ID IS NULL");
            DataRow drFinding = ds2.Tables[0].Select().OrderByDescending(dr => dr["SITE_VISIT_ATTEMPT_ID"]).FirstOrDefault();
            DataRow[] drInProgressAttempts = ds2.Tables[0].Select(string.Format("SITE_VISIT_ATTEMPT_STATUS_ID = {0}", CON.SiteVisitAttemptStatusID.Inprogress));
            if ((Helper.IsUserInSiteVisitOperatorRole(HttpContext.Current.User.Identity.Name)
                || (Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name) && this.CurrentTaskName == CON.RegistrationTaskName.SiteVisitPCG))
                && drRecommendations.Length == 0)
            {
                SiteVisitWFBtnsDisplay = true;
            }
            if (Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name) && drFinding != null && !string.IsNullOrEmpty(drFinding["SITE_VISIT_FINDINGS_ID"].ToString()))
            {
                SiteVisitWFBtnsDisplay = true;
            }
            if (drInProgressAttempts.Any())
            {
                inProgressSiteVisitsExists = true;
            }
        }
        if (isScreeningComplete && SiteVisitWFBtnsDisplay)
        {
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Approve, true);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ApproveUpdate, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.sitevisitcomplete, inProgressSiteVisitsExists ? true : false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Terminate, enrolledProvider);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Deny, !enrolledProvider);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.NotProcessed, inProgressSiteVisitsExists ? true : false);
        }
        else if (isScreeningComplete && !SiteVisitWFBtnsDisplay)
        {
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Approve, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ApproveUpdate, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.sitevisitcomplete, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Terminate, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Deny, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.NotProcessed, false);
        }
        if (Helper.IsUserInSiteVisitOperatorRole(HttpContext.Current.User.Identity.Name) && !isScreeningComplete)
        {
            Master.SetWorkflowPanelVisibility(true);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReferToState, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Approve, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Deny, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Terminate, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ApproveUpdate, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.NotProcessed, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.sitevisitcomplete, (SiteVisitWFBtnsDisplay && inProgressSiteVisitsExists) ? true : false);
        }
        if (Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name))
        {
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.IssueNotice, false);
            if (!isScreeningComplete)
            {
                Master.SetWorkflowPanelVisibility(true);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.OrderNewSiteVisit, true);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Approve, true);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Deny, !enrolledProvider);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Terminate, enrolledProvider);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.NotProcessed, true);
            }
            else
            {
                DataSet dssr = psc.SelectServiceRemovedCheckBox(this.RegistrationId);
                if (dssr != null && dssr.Tables.Count > 0 && dssr.Tables[0].Rows.Count > 0)
                {
                    bool serviceRemoved = Helper.GetBool("SERVICE_REMOVED", dssr.Tables[0].Rows[0]);
                    Master.SetActionVisibility(CON.RegistrationWorkflowActionName.IssueNotice, serviceRemoved);
                }
            }
        }
        Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToProviderReview, true);
    }

    private void SetScreeningWorkActionVisibility()
    {
        bool isScreeningComplete = Registration.ScreeningComplete(this.RegistrationId, string.Empty, ProviderScreeningID);
        Master.SetWorkflowPanelVisibility(isScreeningComplete);

        //if need to show Approve only if no fail actions or show Refer To State when find a fail action, open this part up 
        DataSet ds = Registration.GetScreeningStatuses(this.RegistrationId, ProviderScreeningID);
        bool isResultRecorded = Registration.ScreeningResultsComplete(ds);
        bool isVisible = IsReadOnly ? false : IsStateReview ? true : isResultRecorded ? false : isScreeningComplete ? true : false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        bool isExclusionMatch = psc.CheckDODDAbuserRegistryMatch(this.RegistrationId) || Registration.HasFailActions(this.RegistrationId) || psc.CheckFedExclusionsMatch(this.RegistrationId) || psc.CheckNPPESInactiveMatch(this.RegistrationId);
        bool enrolledProvider = psc.CheckEnrolledProvider(this.RegistrationId);

        if (isVisible && !IsStateReview && !IsReadOnly)
        {
            bool hasFailActions = Registration.HasFailActions(this.RegistrationId);
            DataRow row = Registration.GetRegistration(this.RegistrationId);
            if (row != null) //&& (Helper.GetInt("WORKFLOW_EVENT_TYPE_ID", row) == CON.WorkflowEventType.UpdateReg || Helper.GetInt("WORKFLOW_EVENT_TYPE_ID", row) == CON.WorkflowEventType.RevalReg))
            {
                Master.SetActionVisibility("Review Disenrollment Request", hasFailActions);
                Master.SetActionVisibility("Return to Provider", hasFailActions);
                Master.SetActionVisibility("Refer To Compliance", hasFailActions);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Approve, !hasFailActions);
            }
            if (Master.WorkflowID == CON.WorkflowType.PeriodicDatabaseChecks)
            {
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Terminate, hasFailActions);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Approve, !hasFailActions);
            }

            //check if it is a DDS provider and show refer to dds on has fialactions
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReferToDDS, false);
            if (psc.IsDDSProvider(this.RegistrationId))
            {
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Deny, false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Terminate, false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Approve, false);

                //Should always have ability to Refer to DDS for approval of waiver services
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReferToDDS, true);
            }
        }

        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) && !isScreeningComplete)
        {
            Master.SetWorkflowPanelVisibility(true);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Approve, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Terminate, false);
            Master.SetActionVisibility("Return to Provider", true);
            Master.SetActionVisibility("Refer To Compliance", false);


        }
    }

    private void SetComplianceWorkActionVisibility()
    {
        Master.SetWorkflowPanelVisibility(true);
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        bool enrolledProvider = psc.CheckEnrolledProvider(this.RegistrationId);
        DataSet ds = psc.SelectRegistrationData(this.RegistrationId, "HEARING_RIGHTS");
        bool isDoDDIntialApp = psc.CheckIfDODDInitialApplication(this.RegistrationId);
        bool isODAIntialApp = psc.CheckIfODAInitialApplication(this.RegistrationId);
        bool showDenyOrTerminate = false;
        string dt = "";
        if (Helper.HasRows(ds))
        {
            if (!string.IsNullOrEmpty(Helper.GetString("DATE_TERMINATION", ds.Tables[0].Rows[0])) && !string.IsNullOrEmpty(Helper.GetData("HEARING_STATUS", ds.Tables[0].Rows[0])))
            {
                var terminationReasonDDL = Helper.GetString("TERMINATION_REASON", ds.Tables[0].Rows[0]);
                if (!string.IsNullOrEmpty(terminationReasonDDL))
                {
                    showDenyOrTerminate = true;
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetString("DATE_PROPOSED_ADJUDICATION_ORDER", ds.Tables[0].Rows[0])))
            {
                dt = Helper.GetDate("DATE_PROPOSED_ADJUDICATION_ORDER", ds.Tables[0].Rows[0]);
            }
        }
        bool isUploadPAO = IsPAOUploaded();
        if (Master.TaskName == CON.ComplianceTaskName.CSUploadRR)
        {
            Master.SetActionVisibility("Pending Reconsideration Review", true);
        }
        else if (Master.TaskName == CON.ComplianceTaskName.CSUploadPI)
        {
            Master.SetActionVisibility("ODM Decision Not Upheld", true);
            Master.SetActionVisibility("ODM Decision Upheld", true);
            //Master.SetActionVisibility("Terminate", enrolledProvider);
        }
        else if (Master.TaskName == CON.ComplianceTaskName.ReconsiderationEnterTR)
        {
            Master.SetActionVisibility("Deny", !enrolledProvider);
            Master.SetActionVisibility("Terminate", enrolledProvider);
        }
        else if (Master.TaskName == CON.ComplianceTaskName.UploadPAO) //application disposition
        {
            if (dt != "" && isUploadPAO)
                Master.SetActionVisibility("PAO Complete", true);
            else
                Master.SetActionVisibility("PAO Complete", false);
            Master.SetActionVisibility("Deny", !enrolledProvider && showDenyOrTerminate);
            Master.SetActionVisibility("Terminate", enrolledProvider && showDenyOrTerminate && !string.IsNullOrEmpty(this.MedicaidID));
            Master.SetActionVisibility("Return to Provider", true);

            if ((((this.WorkflowEventTypeId != CON.WorkflowEventType.RevalReg || this.IsReapplication)
                && this.WorkflowEventTypeId != CON.WorkflowEventType.UpdateReg )
                || (this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg && !IsReapplication && !IsReactivation && AppSettings.Get("SAM615Enabled") == "true")
                || (this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && (!isDoDDIntialApp && !isDoDDIntialApp) && (WaiverServiceUpdateTypeID != CON.WaiverServiceUpdateType.DODD && WaiverServiceUpdateTypeID != CON.WaiverServiceUpdateType.ODA) && AppSettings.Get("SAM615Enabled") == "true")
                )
                && this.WorkflowEventTypeId != CON.WorkflowEventType.Reconsideration 
                && this.WorkflowEventTypeId != CON.WorkflowEventType.CredentialReconsideration)
            {
                Master.SetActionVisibility("Not Processed", true);
                Master.SetWorkflowPanelVisibility(true);
            }
            else
            {
                Master.SetActionVisibility("Not Processed", false);
            }

            // SAM763 Show Confirm Compliance Approval only when compliance reason is reapplication/reactivation approval 
            DataSet dsPro = psc.WF_SelectProcessParameters(this.WF_ProcessID);
            if (Helper.HasRows(dsPro))
            {
                DataRow dr = dsPro.Tables[0].Rows[0];
                int referToComplianceReasonID = string.IsNullOrEmpty(dr[CON.ProcessParameter.ReferToComplianceReasonID].ToString()) ? 0 : Convert.ToInt32(dr[CON.ProcessParameter.ReferToComplianceReasonID]);

                if (referToComplianceReasonID == CON.ReferToComplianceReasons.ReapplicationReactivationApproval)
                    Master.SetActionVisibility("Confirm Compliance Approval", true);
                else
                    Master.SetActionVisibility("Confirm Compliance Approval", false);

                // SAM538 display Return to Site Visit button on the Application Disposition Page when a Compliance Reason is equal to Site Visit
                if (referToComplianceReasonID == CON.ReferToComplianceReasons.SiteVisit)
                    Master.SetActionVisibility("Return to Site Visit", true);
                else
                    Master.SetActionVisibility("Return to Site Visit", false);
            }

        }
        else if (Master.TaskName == CON.ComplianceTaskName.RecordDateOfMailReturn)
        {
            dt = string.Empty;
            if (Helper.HasRows(ds))
            {
                if (!string.IsNullOrEmpty(Helper.GetString("DATE_PAO_RETURNED", ds.Tables[0].Rows[0])))
                {
                    dt = Helper.GetDate("DATE_PAO_RETURNED", ds.Tables[0].Rows[0]);
                }
                if (!string.IsNullOrEmpty(Helper.GetString("DATE_PAO_RESENT", ds.Tables[0].Rows[0])))
                {
                    dt = Helper.GetDate("DATE_PAO_RESENT", ds.Tables[0].Rows[0]);
                }
            }
            if (dt != "")
                Master.SetActionVisibility("PAO Mailing Complete", true);
            else
                Master.SetActionVisibility("PAO Mailing Complete", false);
        }
        else if (Master.TaskName == CON.ComplianceTaskName.UploadAO)
        {
            if (Helper.HasRows(ds))
            {
                if (!string.IsNullOrEmpty(Helper.GetString("DATE_TERMINATION", ds.Tables[0].Rows[0]))
                    && Helper.GetInt("HEARING_STATUS", ds.Tables[0].Rows[0]) == CON.HearingStatus.NotRequested
                    && !string.IsNullOrEmpty(Helper.GetString("DATE_ADJUDICATION_ORDER", ds.Tables[0].Rows[0])))
                {
                    if (Helper.GetInt("TERMINATION_REASON", ds.Tables[0].Rows[0]) > 0)
                    {
                        showDenyOrTerminate = true;
                    }
                    Master.SetActionVisibility("Deny", !enrolledProvider && showDenyOrTerminate);
                    Master.SetActionVisibility("Terminate", enrolledProvider && showDenyOrTerminate);
                }
                else
                {
                    Master.SetActionVisibility("Deny", !enrolledProvider && showDenyOrTerminate);
                    Master.SetActionVisibility("Terminate", enrolledProvider && showDenyOrTerminate);
                }

                if (!string.IsNullOrEmpty(Helper.GetString("DATE_HEARING_REQUEST", ds.Tables[0].Rows[0])) &&
                    Helper.GetInt("HEARING_STATUS", ds.Tables[0].Rows[0]) == CON.HearingStatus.InProgress)
                {
                    Master.SetActionVisibility("Appeal Filed", true);
                }
                else
                {
                    Master.SetActionVisibility("Appeal Filed", false);
                }
            }
        }
        else if (Master.TaskName == CON.ComplianceTaskName.CSEnterTR || Master.TaskName == CON.ComplianceTaskName.TerminationReason)
        {
            Master.SetActionVisibility("Deny", !enrolledProvider && showDenyOrTerminate);
            Master.SetActionVisibility("Terminate", enrolledProvider && showDenyOrTerminate);
        }
        else if (Master.TaskName == CON.ComplianceTaskName.RecordHearingStatus)
        {
            Master.SetActionVisibility("Approve", true);
            Master.SetActionVisibility("ODM Decision Upheld", true);
            Master.SetActionVisibility("Deny", !enrolledProvider);
            Master.SetActionVisibility("Terminate", enrolledProvider);
        }
        else
        {
            Master.SetActionVisibility("Approve", true);
            Master.SetActionVisibility("Deny", !enrolledProvider && showDenyOrTerminate);
            Master.SetActionVisibility("Terminate", enrolledProvider && showDenyOrTerminate);
        }
    }

    private void SetRiskCHOPActionVisibility()
    {

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.RegistrationId, "LTC_RISK_ALERT");
        Master.SetWorkflowPanelVisibility(true);

        if (Helper.HasRows(ds) && Helper.HasRows(ds.Tables[0]))
        {
            var finalDate = Helper.GetString("CHOP_Final_Date", ds.Tables[0].Rows[0]);
            var withDrawn = Helper.GetString("CHOP_Withdrawn", ds.Tables[0].Rows[0]);

            if (!string.IsNullOrEmpty(finalDate))
            {
                Master.SetActionVisibility("Place Risk Alert", false);
                Master.SetActionVisibility("Save", true);
            }
            else
            {
                Master.SetActionVisibility("Place Risk Alert", true);
                Master.SetActionVisibility("Save", false);
            }
            if (Helper.IsLoggedInUserInCredentialingQualityAssuranceRole())
            {
                Master.SetActionVisibility("Place Risk Alert", false);
            }

            //if (withDrawn != null)
            ////    Master.SetActionVisibility("Confirm Withdraw", false);
            //else
            //    //Master.SetActionVisibility("Confirm Withdraw", true);
        }
        else
        {

            Master.SetWorkflowPanelVisibility(false);
        }


    }
    private void SetRiskClosureActionVisibility()
    {

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.RegistrationId.ToString());
        parms.Add("PROCESS_ID", Master.ProcessID.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_ClosureNotice", parms);
        Master.SetWorkflowPanelVisibility(true);
        Master.SetActionVisibility("Send Acknowledge Letter", true);
        Master.SetActionVisibility("Save", true);
        Master.SetActionVisibility("Remove Risk Alert", true);
        if (Helper.HasRows(ds) && Helper.HasRows(ds.Tables[0]))
        {
            var EffectiveDate = Helper.GetDate("Closure_Effective_Date", ds.Tables[0].Rows[0]);
            if (!string.IsNullOrEmpty(EffectiveDate))
                Master.SetActionVisibility("Approve", true);
            else
                Master.SetActionVisibility("Approve", false);

        }

    }
    private void SetStateReviewWorkActionVisibility()
    {
        bool actionVisible = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.RegistrationId, "APPEAL");
        DataTable dtProvider = new DataTable();
        int StateReviewStatus = 0;

        DataSet ds2 = psc.SelectRegPageStatus(this.RegistrationId);
        if (Helper.HasRows(ds2))
        {
            DataRow[] dr = ds2.Tables[0].Select("REG_PAGE_TYPE_ID = " + CON.RegistrationPageType.Agreements.ToString());
            if (dr.Length > 0)
            {
                StateReviewStatus = Helper.GetInt("STATE_REVIEW_STATUS_ID", dr[0]);
            }
        }
        if ((Master.TaskName == "Pending Approval" || Master.TaskName == "Pending Denial" || Master.TaskName == "Pending Terminate") && this.RegistrationStep == CON.SectionTypeID.Agreements)
        {

            //PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds1 = psc.SelectAgreementInitials(this.RegistrationId);
            string initials = string.Empty;

            if (Helper.HasRows(ds1))
            {
                DataTable dtAgreementInitials = ds1.Tables[0];
                DataRow[] dataRows = dtAgreementInitials.Select(string.Format("QUESTION_TYPE_ID = '{0}'", "AP28"));
                if (dataRows.Length > 0 && !string.IsNullOrEmpty(Helper.GetString("INITIALS", dataRows[0])))
                {
                    initials = Helper.GetString("INITIALS", dataRows[0]);
                }
            }

            if (initials != string.Empty)
            {
                if (StateReviewStatus == CON.StateReviewStatusID.Approve)
                {

                    Master.SetActionVisibility("Approve", true);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                    actionVisible = true;
                }
                else if (StateReviewStatus == CON.StateReviewStatusID.Deny)
                {

                    Master.SetActionVisibility("Deny", true);
                    Master.SetActionVisibility("Approve", false);
                    Master.SetActionVisibility("Terminate", false);
                    actionVisible = true;
                }
                else if (StateReviewStatus == CON.StateReviewStatusID.Terminate)
                {

                    Master.SetActionVisibility("Approve", false);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", true);
                    actionVisible = true;
                }
                else
                {
                    Master.SetActionVisibility("Approve", false);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                    actionVisible = true;
                }
            }
            else if (StateReviewStatus > 0)
            {
                if (StateReviewStatus == CON.StateReviewStatusID.Deny)
                {

                    Master.SetActionVisibility("Deny", true);
                    Master.SetActionVisibility("Approve", false);
                    Master.SetActionVisibility("Terminate", false);
                    actionVisible = true;
                }
                else if (StateReviewStatus == CON.StateReviewStatusID.Terminate)
                {

                    Master.SetActionVisibility("Approve", false);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", true);
                    actionVisible = true;
                }
                else
                {
                    Master.SetActionVisibility("Approve", false);
                    Master.SetActionVisibility("Deny", false);
                    Master.SetActionVisibility("Terminate", false);
                    actionVisible = true;
                }
            }
            else
            {
                Master.SetActionVisibility("Approve", false);
                Master.SetActionVisibility("Deny", false);
                Master.SetActionVisibility("Terminate", false);
                actionVisible = true;
            }

            DataRow row = Registration.GetRegistration(this.RegistrationId);
            bool HasWentThroughScreening = false;
            DataSet dsScreening = psc.SelectProviderScreeningByProcessID(Master.ProcessID);
            if (Helper.HasRows(dsScreening))
            {
                HasWentThroughScreening = true;
            }
            if (row != null && Helper.GetInt("WORKFLOW_EVENT_TYPE_ID", row) == CON.WorkflowEventType.UpdateReg && !HasWentThroughScreening)
            {
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToScreening, false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToProviderReview, true);
            }
            else
            {
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToScreening, true);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToProviderReview, false);
            }
        }
        if (Helper.HasRows(ds))
        {
            dtProvider = ds.Tables[0];
            int partyID = Helper.GetInt("PARTY_ID", dtProvider.Rows[0]);
            int appealStatusID = Helper.GetInt("APPEAL_STATUS_ID", dtProvider.Rows[0]);
            string ImmediateYorN = Helper.GetString("IMMEDIATE_DENY_OR_TERMINATE", dtProvider.Rows[0]);
            string SendToInterfaceYorN = Helper.GetString("SEND_TO_MMIS", dtProvider.Rows[0]);
            string taskName = Helper.GetString("TASK_NAME", dtProvider.Rows[0]);

            if (taskName == CON.RegistrationTaskName.ProcessAppeal || taskName == CON.RegistrationTaskName.ProcessAppealSiteVisit)
            {
                //Init buttons applicable only to process appeal step
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Deny, false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Terminate, false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Approve, false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.SendToInterface, false);
                //Master.SetActionVisibility(CON.RegistrationWorkflowActionName.DenyProvider, false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.TerminateProvider, false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ApproveProvider, false);
                if (appealStatusID > 0)
                {
                    //Applicable to Process Appeal only.
                    //
                    if (partyID > 0 && ImmediateYorN == CON.ImmediateDenyOrTerminate.Immediate.ToString() && SendToInterfaceYorN == "Y" && appealStatusID == CON.AppealStatus.NoticeSent)
                    {
                        Master.SetActionVisibility(CON.RegistrationWorkflowActionName.SendToInterface, true);
                        actionVisible = true;
                    }
                    else
                    {
                        if (appealStatusID == CON.AppealStatus.NoAppeal || appealStatusID == CON.AppealStatus.AppealLost_ProcessDenial_Termination)
                        {
                            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Deny, partyID == 0);
                            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Terminate, partyID > 0);
                            //Master.SetActionVisibility(CON.RegistrationWorkflowActionName.DenyProvider, partyID == 0);
                            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.TerminateProvider, partyID > 0);
                            actionVisible = true;
                        }
                        else if (appealStatusID == CON.AppealStatus.NoticeSent || appealStatusID == CON.AppealStatus.ProviderRequestedAppeal)
                        {
                            //do nothing just show process appeal button
                        }
                        else if (appealStatusID == CON.AppealStatus.AppealWon_ReinstateProvider)
                        {
                            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.Approve, true);
                            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ApproveProvider, true);
                            actionVisible = true;
                        }
                    }
                }
            }
            else
            {
                //Init buttons applicable only to State Review step
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ApproveProvider, false);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.InitiateAppeal, false);
                //Master.SetActionVisibility(CON.RegistrationWorkflowActionName.DenyProvider, false); //?change to Send to Process Appeal
                //Master.SetActionVisibility(CON.RegistrationWorkflowActionName.TerminateProvider, false); //? change to Deny/Term Provider
                string stateREviewScreeningStatus = Helper.GetString("STATE_REVIEW_SCREENING_STATUS_ID", dtProvider.Rows[0]);
                if (!string.IsNullOrEmpty(stateREviewScreeningStatus))
                {
                    //When override screening is selected on take action
                    if (stateREviewScreeningStatus == CON.StateReviewScreeningStatus.OverrideScreening.ToString())
                    {
                        Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ApproveProvider, true);
                        actionVisible = true;
                    }
                    else if (stateREviewScreeningStatus == CON.StateReviewScreeningStatus.Confirm.ToString())
                    {
                        //Initiate Appeal
                        /*if (appealStatusID > 0)
                        {*/
                        Master.SetActionVisibility(CON.RegistrationWorkflowActionName.InitiateAppeal, true);
                        /*Master.SetActionVisibility(CON.RegistrationWorkflowActionName.DenyProvider, partyID == 0);
                        Master.SetActionVisibility(CON.RegistrationWorkflowActionName.TerminateProvider, partyID > 0);*/
                        actionVisible = true;
                        /*}*/
                    }
                }

                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToScreening, true);
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToSiteVisit, true);
            }
        }
        else if (this.CurrentTaskName == CON.RegistrationTaskName.GroupMemberRetroReview)
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet ds1 = svc.SelectRegistrationData(this.RegistrationId, "AFFILIATION");
            DataTable dt = null;
            string filter =
                //bug 2338 - Do not show Removed by Group or Removed by Individual
                "(AffiliationStatus <> '" + CON.GroupAffiliationStatusType.RemovedbyGroup + "'"
                + " AND AffiliationStatus <> '" + CON.GroupAffiliationStatusType.RemovedbyIndividual + "')"
                + " OR END_DATE IS NOT NULL";

            if (Helper.HasRows(ds1))
            {
                DataRow[] rows = ds1.Tables[0].Select(filter, "NAME ASC");
                if (rows.Length > 0)
                {
                    dt = rows.CopyToDataTable();
                }
                else
                {
                    dt = new DataTable();
                }
            }
            else
            {
                dt = new DataTable();
            }
            DataRow[] rows1 = dt.Select("RETRO_REVIEW_REQUIRED_ID = '" + CON.GroupMemberRetroStatusID.RetroReviewCompleted + "'");
            if (rows1.Length > 0)
            {
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ApproveProvider, true);
                actionVisible = true;
            }
            else
            {
                Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ApproveProvider, false);
            }
        }
        else if (!Helper.HasRows(ds) && (this.CurrentTaskName == CON.RegistrationTaskName.StateReview || this.CurrentTaskName == CON.RegistrationTaskName.StateReviewSiteVisit))
        {
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToScreening, true);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.InitiateAppeal, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ApproveProvider, false);
            Master.SetActionVisibility(CON.RegistrationWorkflowActionName.ReturnToSiteVisit, true);
            actionVisible = true;
        }
        Master.SetWorkflowPanelVisibility(actionVisible);

    }

    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
    }

    private bool AddError(string errMsg, string validationGroup)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = validationGroup;
        this.Page.Validators.Add(val);
        return false;
    }

    // If the TAX_ENTITY_TYPE_ID is not set in the REG_SERVICE_LOCATION then set it to the default from AppSettings
    private void SetTaxEntityType()
    {
        string taxEntityTypeId = Helper.GetAppSettingFromDB("TAX_ENTITY_TYPE_ID_Default");
        if (string.IsNullOrEmpty(taxEntityTypeId)) return;                              // No default, outahere

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.RegistrationId, "SERVICE_LOCATION");
        if (!Helper.HasRows(ds)) return;                                                // No service location, outahere

        DataRow row = ds.Tables[0].Rows[0];
        if (!string.IsNullOrEmpty(Helper.GetString("TAX_ENTITY_TYPE_ID", row))) return; // Already set, outahere

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.RegistrationId.ToString());
        parms.Add("REG_SERVICE_LOCATION_ID", Helper.GetString("REG_SERVICE_LOCATION_ID", row));
        parms.Add("TAX_ENTITY_TYPE_ID", taxEntityTypeId);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        svc.UpdateRegistrationData(this.RegistrationId, "SERVICE_LOCATION", parms);
    }

    public override bool OnTaskExit(string action)
    {
        int regID = this.RegistrationId;

        PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient(); ;
        DataSet ds1 = null;
        ds1 = client.SelectRegistrationByRegID(RegistrationId);
        string Current_TaskName = "";
        if (Helper.HasRows(ds1))
        {
            DataRow dr = ds1.Tables[0].Rows[0];
            Current_TaskName = Methods.GetStringValue(dr, "CurrentTaskName");
            int WF_TaskID = Methods.GetIntValue(dr["CurrentTaskID"]);
            //OHPNM-12268 if the step is either Provider or Owner review but the task is not in "Provider Screening" (TaskID-4)
            //Redirecting user to login page if they click brower back button from "Provider Review" to navigate to "Provider/Owner Screening" click approve button.
            if ((this.RegistrationStep == CON.RegistrationPageType.ProviderScreening || this.RegistrationStep == CON.RegistrationPageType.OwnerScreening) && Current_TaskName == CON.RegistrationTaskName.ProviderReview)
            {
                if ((action == "Approve") && (this.WorkflowEventTypeId == CON.WorkflowEventType.NewReg || this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg))
                {
                    Response.Redirect("~/Process/MyQueue.aspx");
                }

            }
        }

        string userID = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

        if (action == "Send To Interface")
        {
            //check for upload document -todo

        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) || Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInORFAWorkerRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInLTCWorkerRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInDBHOperatorRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInStateReviewerRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInDBHReviewerRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInDDSReviewerRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.AppealSpecialist)
            || Helper.IsUserInCredentialingRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.LTCInitialRevalidation)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.TechAdmin)
             || Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.LTCCHOP)) //Ram
        {
            // Accounting has Workflow move to next step
            if (Helper.IsUserInAccountingRolls(HttpContext.Current.User.Identity.Name))
            {
                if (action == "Application Complete" || action == "Approved" || action == "Approve (w/o F&A Review)")
                {

                    Registration.MarkErrorsAsClosed(this.RegistrationId, 0, 0);         // Mark ALL open errors closed

                    // Set the REG_ACH_EDISON start date
                    DataSet ds = psc.SelectRegistrationData(this.RegistrationId, "ACH_EDISON");
                    if (Helper.HasRows(ds))
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms.Add("REG_ACH_EDISON_ID", Helper.GetString("REG_ACH_EDISON_ID", dr));
                        parms.Add("REG_ID", regID.ToString());
                        parms.Add("VENDOR_NUMBER", Helper.GetString("VENDOR_NUMBER", dr));
                        parms.Add("LOCATION_CODE", Helper.GetString("LOCATION_CODE", dr));
                        parms.Add("SEQUENCE_NUMBER", Helper.GetString("SEQUENCE_NUMBER", dr));
                        parms.Add("START_DATE", DateTime.Now.ToString());
                        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parms.Add("LAST_MODIFIED_USER", userID);
                        psc.UpdateRegistrationDataTable("ACH_EDISON", parms);
                    }
                }
                else if (action == "Return to Provider")
                {
                    // Mark registration status as Return to Provider
                    Registration.UpdateRegistrationStatus(regID, CON.RegistrationStatusTypeId.ReturnToProvider, userID);
                    UpdateProviderStatusNotComplete(CON.RegistrationStatusTypeId.ReturnToProvider);
                }
                else if (action == "Return to Provider Services")
                {
                    // Accounting is returning to Provider Services
                    // Removed due to DCPDMS-3065 
                    //Registration.UpdateRegistrationStatus(regID, CON.RegistrationStatusTypeId.ReturnToProviderServices, userID);
                }
                else if (action == "Forward to F&A")
                {
                    // If forwarding, set the ACH Page Status to "Pending"
                    psc.SaveRegistrationPageStatus(regID, CON.RegistrationPageType.ApplicationFee, null, CON.RegistrationProviderServicesStatusTypeId.Pending, userID, null, null, null, null, null, null, null, null, null, null);
                }
                return true;
            }


            if (this.WF_WorkflowID == CON.WorkflowType.RiskAlertCHOP || this.WF_WorkflowID == CON.WorkflowType.RiskAlertClosure)
            {
                bool isValid = false;
                ucRegistrationNavigation_ValidateDataEvent(this.RegistrationStep, ref isValid);
                if (isValid)
                    ucRegistrationNavigation_SaveDataEvent(this.RegistrationStep);
                else
                    return false;
            }

            int statusId = 0;
            int pageStatusTypeId = 0;
            int resultId = CON.ScreeningResultId.Pending;
            // With each action a status change will occur
            switch (action)
            {
                case "Application Complete":
                case "Approve":
                case "Approve Update":
                case "Resubmit Provider":
                case "Approve Profile":
                case "Approve Provider":
                case "Refer to DBH":
                case "Refer to DDS":
                case "Send to MMIS":
                case "Wait for Background Check Results":
                case "Background Check Complete":
                case "Refer To LTC/DHCF":
                case "Recommend Hardship Approval":
                case "Review Complete":
                    resultId = CON.ScreeningResultId.Approved;
                    pageStatusTypeId = CON.RegistrationProviderServicesStatusTypeId.Approved;
                    Registration.MarkErrorsAsClosed(this.RegistrationId, 0, 0);         // Mark ALL open errors closed

                    if ((this.RegistrationStep == CON.RegistrationPageType.ProviderReview || Current_TaskName == CON.RegistrationTaskName.ProviderReview) && action == "Approve")
                    {
                        ProviderFeedHelper.InsertProviderFeedNotes(this.RegistrationId, 0, HttpContext.Current.User.Identity.Name, "Provider Review Approved", personReviewedBy: HttpContext.Current.User.Identity.Name, finalDisposition: CON.FinalDisposition.Approved, processID: this.WF_ProcessID);
                    }
                    break;
                case "Approve Application Fee":
                    statusId = CON.RegistrationStatusTypeId.ApplicationFeeApproved;
                    pageStatusTypeId = CON.RegistrationProviderServicesStatusTypeId.Approved;
                    Registration.MarkErrorsAsClosed(this.RegistrationId, 0, 0);         // Mark ALL open errors closed
                    break;
                case "Deny":
                    //case "Deny Provider":
                    // commented out for DCPDMS-3065 - status is set to Denied in WFCompleteWorkflow now.
                    //statusId = CON.RegistrationStatusTypeId.Denied;
                    resultId = CON.ScreeningResultId.Denied;
                    break;
                case "Return to Provider":
                case "Return To Provider":
                    statusId = CON.RegistrationStatusTypeId.ReturnToProvider;
                    pageStatusTypeId = CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider;
                    if (this.CurrentTaskName == CON.RegistrationTaskName.SiteVisitCompliance)
                    {
                        statusId = CON.RegistrationStatusTypeId.ReturnToProviderForSiteVisit;
                    }
                    if (this.CurrentTaskName == CON.RegistrationTaskName.ProviderScreening)
                    {
                        resultId = CON.ScreeningResultId.Denied;
                    }

                    break;
                case "Close Sanction Review":
                    statusId = CON.RegistrationStatusTypeId.CloseSanctionReview;
                    break;
                case "Reinstate Provider":
                    statusId = CON.RegistrationStatusTypeId.ReinstateProvider;
                    break;
                case "Suspend Provider":
                    statusId = CON.RegistrationStatusTypeId.Suspended;
                    break;
                case "Terminate Provider":
                case "Send To Interface":
                case "Terminate":
                case "Record Termination":
                    // Removed due to DCPDMS-3065
                    // statusId = CON.RegistrationStatusTypeId.TerminateProvider;
                    break;
                case "Update Provider":
                    statusId = CON.RegistrationStatusTypeId.UpdateProvider;
                    break;
                case "Save":
                    statusId = CON.RegistrationStatusTypeId.Save;

                    if (this.WF_WorkflowID == CON.WorkflowType.RiskAlertClosure)
                    {
                        statusId = CON.RegistrationStatusTypeId.PendingClosure;
                    }
                    if (this.WF_WorkflowID == CON.WorkflowType.RiskAlertCHOP)
                    {
                        statusId = CON.RegistrationStatusTypeId.PendingCHOP;
                    }
                    break;
                case "Refer To State":
                    statusId = CON.RegistrationStatusTypeId.StateReview;
                    break;
                case "Initiate Appeal":
                    statusId = CON.RegistrationStatusTypeId.StateReview;
                    break;
                case "Return To Screening":
                    // Removed as part of DCPDMS-3065 
                    //statusId = CON.RegistrationStatusTypeId.ReturnToProviderServices;
                    resultId = CON.ScreeningResultId.Pending;
                    break;
                case "Return To Site Visit":
                    statusId = CON.RegistrationStatusTypeId.ReturnToSiteVisit;
                    resultId = CON.ScreeningResultId.Pending;
                    break;
                case "Return To Provider Review":
                    // Returned as part of DCPDMS-3213 RBM 8/15/2019
                    //  statusId = CON.RegistrationStatusTypeId.ReturnToProviderReview;
                    resultId = CON.ScreeningResultId.Denied;
                    break;
                case "Return To State Review":
                    statusId = CON.RegistrationStatusTypeId.ReturnToStateReview;
                    break;
                case "Not Processed":
                    statusId = CON.RegistrationStatusTypeId.NotProcessed;
                    break;
                case "Pending Denial/Termination":
                    // commented out for DCPDMS-3065 - status is set to Denied in WFCompleteWorkflow now.
                    //statusId = CON.RegistrationStatusTypeId.Deny;
                    resultId = CON.ScreeningResultId.Pending;
                    break;

            }

            // OHPNM-7175 - if reconsideration and provider review and there is no NEW_REG_ENROLL_SPECIALIST_APPROVAL_DTE, then we need to add one
            bool needToUpdateEnrollSpecialistApprovalDate = false;
            if (Master.TaskName == CON.RegistrationTaskName.ProviderReview)
            {
                DataSet dsReg = psc.SelectRegistrationByRegID(this.RegistrationId);
                if (Helper.HasRows(dsReg))
                {
                    DataRow drReg = dsReg.Tables[0].Rows[0];
                    DateTime EnrollmentSpecialistApprovalDate = ObjectControllerHelper.GetDateTime("NEW_REG_ENROLL_SPECIALIST_APPROVAL_DTE", drReg);
                    DateTime TermDate = ObjectControllerHelper.GetDateTime("TerminationDate", drReg);

                    if (this.WorkflowEventTypeId == CON.WorkflowEventType.Reconsideration || this.WorkflowEventTypeId == CON.WorkflowEventType.CredentialReconsideration)
                    {
                        if (ObjectControllerHelper.IsDateNull(EnrollmentSpecialistApprovalDate))
                        {
                            needToUpdateEnrollSpecialistApprovalDate = true;
                        }
                    }

                    //SAM763 When a provider is going from inactive to active set Compliance Reason as Reapplication/reactivation approval.
                    if ((action == "Application Complete" || action == "Approve" || action == "Approve Enrollment") && (
                        this.IsReactivation || this.IsReapplication
                        || (this.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg || this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg) && !Helper.IsDateNull(TermDate)))
                    {
                        psc.WF_SaveProcessParameter(this.WF_ProcessID, CON.ProcessParameter.ReferToComplianceReasonID, CON.ReferToComplianceReasons.ReapplicationReactivationApproval.ToString());
                    }

                }
            }

            //if new reg and provider review need to update the needToUpdateEnrollSpecialistApprovalDate
            if (needToUpdateEnrollSpecialistApprovalDate == false && ((this.WorkflowEventTypeId == CON.WorkflowEventType.NewReg || this.WorkflowEventTypeId == CON.WorkflowEventType.ChangeProviderType) && action == "Application Complete"))
            {
                needToUpdateEnrollSpecialistApprovalDate = true;
            }

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            // if we need update the NEW_REG_ENROLL_SPECIALIST_APPROVAL_DTE field and they aren't a credentialed provider
            if (needToUpdateEnrollSpecialistApprovalDate && !this.IsCredentialingProvider)
            {

                Dictionary<string, string> parms1 = new Dictionary<string, string>();

                parms1.Add("REG_ID", this.RegistrationId.ToString());
                parms1.Add("NEW_REG_ENROLL_SPECIALIST_APPROVAL_DTE", DateTime.Now.ToString());
                svc.UpdateRegistration(parms1);

            }

            // Only Non-DIDD roles can set the overall Registration status
            if (!Helper.IsUserInDIDDRoles(HttpContext.Current.User.Identity.Name) && this.WF_WorkflowID != CON.WorkflowType.PeriodicDatabaseChecks && statusId != 0)
                Registration.UpdateRegistrationStatus(regID, statusId, userID);

            if (resultId == CON.ScreeningResultId.Approved && (action == "Approve" || action == "Approve Update") && !Helper.IsUserInCredentialingRole(HttpContext.Current.User.Identity.Name))
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", regID.ToString());
                parms.Add("SCREENING_COMPLETE_DATE", DateTime.Now.Date.ToShortDateString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", userID);
                psc.UpdateRegistration(parms);
            }

            if (Master.TaskName == CON.RegistrationTaskName.ProviderScreening)
            {
                if (action != CON.RegistrationWorkflowActionName.ReferToState)
                {
                    psc.UpdateScreeningResult(this.ProviderScreeningID, resultId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }
            }
            if ((this.CurrentTaskName == CON.RegistrationTaskName.ProviderCredentialing && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSpecialist))
                || (this.CurrentTaskName == CON.RegistrationTaskName.ProviderCredentialingODM && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSpecialist)))
            {
                if (action == "Process Discontinue")
                {
                    psc.UpdateCredentialStatus(regID, CON.CredentilaingStatus.ProcessDiscontinued, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }
                if (action == "Approve")
                {
                    int riskLevel = 0;
                    DataSet ds = psc.SelectProviderCredentialingData(regID);
                    if (Helper.HasRows(ds))
                    {
                        riskLevel = Convert.ToInt32(ds.Tables[0].Rows[0]["RISK_LEVEL_ID"]);
                        if (riskLevel == 1)
                            psc.UpdateCredentialStatus(regID, CON.CredentilaingStatus.ChairReview, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        else
                            psc.UpdateCredentialStatus(regID, CON.CredentilaingStatus.QualityReview, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    }
                }
                if (action == "Return to Provider")
                {
                    psc.UpdateCredentialStatus(regID, CON.CredentilaingStatus.Incomplete, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    psc.SaveRegistrationSectionStatusRTP(this.RegistrationId, 0, CON.SectionTypeID.EmploymentHistory, CON.RegistrationProviderStatusTypeId.NotComplete, CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    psc.SaveRegistrationSectionStatusRTP(this.RegistrationId, 0, CON.SectionTypeID.GroupAndFacilityAffiliations, CON.RegistrationProviderStatusTypeId.NotComplete, CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    psc.SaveRegistrationSectionStatusRTP(this.RegistrationId, 0, CON.SectionTypeID.Affiliations, CON.RegistrationProviderStatusTypeId.NotComplete, CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    psc.SaveRegistrationSectionStatusRTP(this.RegistrationId, 0, CON.SectionTypeID.Insurance, CON.RegistrationProviderStatusTypeId.NotComplete, CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    psc.SaveRegistrationSectionStatusRTP(this.RegistrationId, 0, CON.SectionTypeID.OtherDocuments, CON.RegistrationProviderStatusTypeId.NotComplete, CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    //psc.SaveRegistrationSectionStatusRTP(this.RegistrationId, 0, CON.SectionTypeID.SpecialtiesTaxonomies, CON.RegistrationProviderStatusTypeId.NotComplete, CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }
            }
            if ((this.CurrentTaskName == CON.RegistrationTaskName.CredentialingSupervisorReview && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSupervisor))
                || (this.CurrentTaskName == CON.RegistrationTaskName.ODMCredentialingSupervisorReview && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSupervisor)))
            {
                if (action == "Admin Deny" || action == "Admin Terminate")
                {
                    psc.UpdateCredentialStatus(regID, action == "Admin Deny" ? CON.CredentilaingStatus.AdministrativeDenial : CON.CredentilaingStatus.AdministrativeTermination,
                        Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                    //psc.SaveRegistrationProgramStatus(regID, action == "Admin Deny" ? CON.RegistrationProgramStatusTypeId.Denied : CON.RegistrationProgramStatusTypeId.Terminated, userID);
                    //Dictionary<string, string> prms = new Dictionary<string, string>();
                    //prms.Add("REG_ID", regID.ToString());
                    //prms.Add("TERM_DATE", DateTime.Now.ToString("MM/dd/yyyy"));
                    //psc.UpdateRegistrationDataTable("PROVIDERCustom", prms);
                    psc.TerminateProvider(regID, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EnrollStatus.INACTIVE.ToString(), CON.EnrollStatusReason.STATE_INITIATED_TERMINATION, true, false);
                }

                if (action == "Process Discontinue")
                {
                    psc.UpdateCredentialStatus(regID, CON.CredentilaingStatus.ProcessDiscontinued, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }

                if (action == "Approve")
                {
                    int riskLevel = 0;
                    DataSet ds = psc.SelectProviderCredentialingData(regID);
                    if (Helper.HasRows(ds))
                    {
                        riskLevel = Convert.ToInt32(ds.Tables[0].Rows[0]["RISK_LEVEL_ID"]);
                        if (riskLevel == 1)
                            psc.UpdateCredentialStatus(regID, CON.CredentilaingStatus.ChairReview, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        else
                            psc.UpdateCredentialStatus(regID, CON.CredentilaingStatus.QualityReview, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    }
                }

                if (action == "Return To Credential Specialist" || action == "Return To ODM Credential Specialist")
                {
                    psc.UpdateCredentialStatus(regID, CON.CredentilaingStatus.InProcess, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }

            }
            if (this.CurrentTaskName == CON.RegistrationTaskName.CredentialingQualityReview)
            {
                if (action == "QA Approve")
                {
                    psc.UpdateCredentialStatus(regID, CON.CredentilaingStatus.ODMCredentialingReview, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }

                if (action == "QA Fail")
                {
                    psc.UpdateCredentialStatus(regID, CON.CredentilaingStatus.CredentialingSupervisorReview, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }

            }
            if (this.CurrentTaskName == CON.RegistrationTaskName.ODMCredentialingReview)
            {
                if (action == "Approve for Committee")
                {
                    int riskLevel = 0;
                    DataSet ds = psc.SelectProviderCredentialingData(regID);
                    if (Helper.HasRows(ds))
                    {
                        riskLevel = Convert.ToInt32(ds.Tables[0].Rows[0]["RISK_LEVEL_ID"]);
                        if (riskLevel == 1)
                            psc.UpdateCredentialStatus(regID, CON.CredentilaingStatus.ChairReview, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        else
                            psc.UpdateCredentialStatus(regID, CON.CredentilaingStatus.CommitteeReview, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    }

                }

                if (action == "Return to Credentialing")
                {
                    psc.UpdateCredentialStatus(regID, CON.CredentilaingStatus.CredentialingSupervisorReview, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }

                if (action == "Enrollment Approved")
                {
                    psc.UpdateCredentialStatus(regID, CON.CredentilaingStatus.ProcessDiscontinued, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }

            }
            if (this.CurrentTaskName == CON.RegistrationTaskName.CredentialCommitteeChairReview)
            {
                if (action == CON.RegistrationWorkflowActionName.Deny || action == CON.RegistrationWorkflowActionName.ReviewComplete)
                {
                    psc.UpdateCredentialResult(this.RegistrationId, resultId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                }
            }
            if (this.CurrentTaskName == CON.RegistrationTaskName.FinancialReview && action == "Approve")
            {
                psc.SaveRegistrationPageStatus(this.RegistrationId, CON.RegistrationPageType.ProviderScreening, null, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.FinancialReviewStatusID.Completed, null, null, null, null, null, null, null, null, null);
            }

            if (action == "Application Complete")
            {
                Master.SetWorkflowPanelVisibility(true);
                Master.SetActionVisibility("Refer To Compliance", false);
                Master.SetActionVisibility("Application Complete", false);
                Master.SetActionVisibility("Approve Enrollment", false);
                Master.SetActionVisibility("Create LT Enrollment", true);
                if (this.MMISProviderTypeID == "86" || this.MMISProviderTypeID == "89" || this.MMISProviderTypeID == "88")
                {
                    psc.WF_SaveProcessParameter(Master.ProcessID, CON.ProcessParameter.ApplicationComplete, "Pressed");
                }

            }
            if (action == "Return To Screening" && this.WF_WorkflowID == CON.WorkflowType.PeriodicDatabaseChecks)
            {
                psc.UpdateScreeningStatus_PeriodicWF(this.RegistrationId, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            }
            if (action == "Return To Site Visit")
            {
                Registration.UpdateRegistrationStatus(regID, CON.RegistrationStatusTypeId.ReturnToSiteVisit, userID);
                UpdateProviderStatusNotComplete(CON.RegistrationStatusTypeId.ReturnToSiteVisit);
                DataSet ds = Registration.GetScreeningStatuses(this.RegistrationId);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (this.RegistrationStep == CON.RegistrationPageType.SiteVisitScreening)
                    {
                        //delete the site visit recommendation and update teh site_visit_status
                    }
                }
            }
            if (action == CON.RegistrationWorkflowActionName.sitevisitcomplete)
            {
                psc.UpdateSiteVisitAttemptStatus(regID, CON.SiteVisitAttemptStatusID.Completed);
            }
            if (action == "Return To Provider Review")
            {
                // Removed as part of DCPDMS-3213 RBM 8/15/2019
                // Registration.UpdateRegistrationStatus(regID, CON.RegistrationStatusTypeId.ReturnToProviderReview, userID);
                UpdateProviderStatusNotComplete(CON.RegistrationStatusTypeId.ReturnToProviderReview);
                DataSet ds = Registration.GetScreeningStatuses(this.RegistrationId);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (this.RegistrationStep != CON.RegistrationPageType.SiteVisitScreening)
                    {
                        if (dr["SCREENING_TYPE"].ToString() != "SITE VISIT")
                        {
                            psc.UpdateScreeningStatus(Convert.ToInt32(dr["SCREENING_ID"]), CON.ScreeningStatusId.Complete, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                            /*if (Master.TaskName == CON.RegistrationTaskName.RetroReview && action == "Return To Screening")
                                psc.UpdateScreeningResult(Convert.ToInt32(dr["SCREENING_ID"]), resultId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());*/

                            //update rest of the screening status to denied DCPDMS1974
                            //psc.UpdateScreeningPreviousStatus(regID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                        }
                    }
                }
                if (Registration.IsSiteVisitTaskType(Master.TaskName))
                {
                    if (action != CON.RegistrationWorkflowActionName.ReferToState)
                    {
                        psc.UpdateScreeningResult(this.ProviderScreeningID, resultId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    }
                }

            }
            if (action == "Return to Provider Credentialing")
            {
                //DataSet ds = psc.SelectProviderCredentialingData(this.RegistrationId);
                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    psc.UpdateCredentialStatus(Convert.ToInt32(dr["credentialing_id"]), CON.ScreeningStatusId.Complete, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                //    psc.UpdateCredentialResult(this.RegistrationId, resultId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                //}
            }
            if (action == "Return to Provider" || action == "Deny")
            {
                int actionType = action == "Return to Provider" ? CON.RegistrationStatusTypeId.ReturnToProvider : CON.RegistrationStatusTypeId.Deny;
                UpdateProviderStatusNotComplete(actionType);
                if (action == "Deny")
                {
                    if (this.CurrentTaskName == CON.RegistrationTaskName.PendingDenial &&
                    Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name))
                    {
                        // Update the State Program Status
                        // DCPDMS-3072 - Moved to change the program status only when the state administrator denies it.
                        psc.SaveRegistrationProgramStatus(regID, CON.RegistrationProgramStatusTypeId.Denied, userID);
                        Dictionary<string, string> prms = new Dictionary<string, string>();
                        prms.Add("REG_ID", regID.ToString());
                        prms.Add("TERM_DATE", DateTime.Now.ToString("MM/dd/yyyy"));
                        psc.UpdateRegistrationDataTable("PROVIDERCustom", prms);
                    }
                    if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.APMSpecialist) && this.MMISProviderTypeID == "99"
                        && this.WF_WorkflowID == CON.WorkflowType.CPC && this.CurrentTaskName == CON.RegistrationTaskName.ProviderReview)
                    {
                        psc.DenyCPCFromProviderReview(regID, DataAccess.GetAppSetting(CON.AppSettingsKeyName.CPCProgramYear), this.WF_ProcessID, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.WorkflowEventTypeId);
                    }
                }
            }

            //set / update provider status and other registration dates like end_date, enrollment status
            //Only applicable for Appeal step
            if (this.CurrentTaskName == CON.RegistrationTaskName.ProcessAppeal)
            {
                SetProviderStatus(action);
            }

            if (action == "Return To State Reviewer")
            {
                Registration.UpdateRegistrationStatus(regID, CON.RegistrationStatusTypeId.ReturnToStateReview, userID);
            }

            //SAM538
            if (action == "Deny" || action == "Terminate")
            {
                if (this.CurrentTaskName == "Compliance Site Visit Review Step (Compliance)")
                {
                    psc.WF_SaveProcessParameter(this.WF_ProcessID, CON.ProcessParameter.ReferToComplianceReasonID, CON.ReferToComplianceReasons.SiteVisit.ToString());
                }
                if (this.CurrentTaskName == "Fingerprint and Background ReSubmit" || this.CurrentTaskName == "Record Whether background check needed" || this.CurrentTaskName == "Fingerprint and Background Check Pending")
                {
                    psc.WF_SaveProcessParameter(this.WF_ProcessID, CON.ProcessParameter.ReferToComplianceReasonID, CON.ReferToComplianceReasons.BCIComplianceReview.ToString());
                }
            }
            if (action == "Site Visit Not Needed" && this.CurrentTaskName == CON.RegistrationTaskName.SiteVisitCompliance && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ComplianceSpecialist))
            {
                //SAM538 Update Site visit findings to  ODM determined Site Visit Not Needed
                psc.UpdateSiteVisitAttempt(this.SiteVisitAttemptID, CON.SiteVisitStatusID.Completed, null, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, null, null, null, 7, null, null, null);
            }
            //SAM538 set registration status to approved on approval from compliance site visit step
            if (action == "Approve" && this.WF_WorkflowID == CON.WorkflowType.SiteVistEvent && this.CurrentTaskName == CON.RegistrationTaskName.SiteVisitCompliance)
            {
                Registration.UpdateRegistrationStatus(regID, CON.RegistrationStatusTypeId.Approved, userID);
            }

            if (this.CurrentTaskName == CON.RegistrationTaskName.IncidentComplianceReview && this.WF_WorkflowID == CON.WorkflowType.IncidentCompliance)
            {
                DataSet dsCasenum = psc.SelectRegistrationData(regID, "INCIDENT_COMPLIANCE_CASE_XREF");
                string response = string.Empty;
                if (action == "End Review")
                {
                    //Call the IMS Service 

                    foreach (DataRow dr in dsCasenum.Tables[1].Rows)
                    {
                        //if (Helper.GetAppSetting("Environment", string.Empty) == CON.Environment.E2E || Helper.GetAppSetting("Environment", string.Empty) == CON.Environment.INT02 || Helper.GetAppSetting("Environment", string.Empty) == CON.Environment.OH_UAT)
                        //{
                        UpdateProviderIncidentStatusInfo upReqInfo = new UpdateProviderIncidentStatusInfo();
                        upReqInfo.MedicaidProviderID = Helper.GetString("MEDICAID_ID", dr);
                        upReqInfo.ProviderName = Helper.GetString("PROVIDER_NAME", dr);
                        upReqInfo.ProviderStatus = "Continue";
                        if (!string.IsNullOrEmpty(Helper.GetString("PAODate", dr)))
                            upReqInfo.PAOMailedDate = Convert.ToDateTime(Helper.GetString("PAODate", dr)).ToString("yyyy-MM-dd");
                        if (!string.IsNullOrEmpty(Helper.GetString("HearingReqDate", dr)))
                            upReqInfo.HearingRequestedDate = Convert.ToDateTime(Helper.GetString("HearingReqDate", dr)).ToString("yyyy-MM-dd");
                        if (!string.IsNullOrEmpty(Helper.GetString("HearingReqDate", dr)))
                            upReqInfo.HearingRequestDueDate = Convert.ToDateTime(Helper.GetString("HearingReqDate", dr)).AddDays(30).ToString("yyyy-MM-dd");
                        if (!string.IsNullOrEmpty(Helper.GetString("SettlementDate", dr)))
                            upReqInfo.SettlementReachedDate = Convert.ToDateTime(Helper.GetString("SettlementDate", dr)).ToString("yyyy-MM-dd");
                        if (!string.IsNullOrEmpty(Helper.GetString("AODate", dr)))
                            upReqInfo.AODateIssued = Convert.ToDateTime(Helper.GetString("AODate", dr)).ToString("yyyy-MM-dd");
                        if (!string.IsNullOrEmpty(Helper.GetBool("PAIUncliamed", dr).ToString()))
                            upReqInfo.PAIUnclaimed = Helper.GetBool("PAIUncliamed", dr) ? "True" : "False";

                        IncidentManagementReqRes ims = new IncidentManagementReqRes();
                        response = ims.UpdateIncidentProviderStatus(Helper.GetString("MEDICAID_ID", dr), string.Empty, regID, "EndReview", upReqInfo);
                        //}
                        //else
                        //    response = "Success";

                        if (response == "Success")
                        {
                            //Update Case Status
                            Dictionary<string, string> parms = new Dictionary<string, string>();
                            parms = new Dictionary<string, string>();
                            parms.Add("REG_ID", regID.ToString());
                            parms.Add("REG_INCIDENT_COMPLIANCE_CASE_XREF_ID", Helper.GetString("REG_INCIDENT_COMPLIANCE_CASE_XREF_ID", dr));
                            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                            parms.Add("CASE_STATUS", CON.IncidentReviewCaseStatus.ReviewComplete.ToString());
                            psc.UpdateRegistrationData(regID, "INCIDENT_COMPLIANCE_CASE_XREF", parms);
                        }
                        else
                        {
                            return false;
                        }
                    }

                    //Remove Alert
                    if (response == "Success")
                        psc.UpdateIncidentAlertFlag(regID, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name));

                }

                if (action == "Return to Provider")
                {
                    foreach (DataRow dr in dsCasenum.Tables[1].Rows)
                    {
                        string POCdueDate = string.Empty;
                        string POCreceivedDate = string.Empty;
                        string POCacceptDate = string.Empty;
                        string RevPOCDate = string.Empty;
                        string RevPOCdueDate = string.Empty;

                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms = new Dictionary<string, string>();
                        parms.Add("REG_ID", regID.ToString());
                        parms.Add("REG_INCIDENT_COMPLIANCE_CASE_XREF_ID", Helper.GetString("REG_INCIDENT_COMPLIANCE_CASE_XREF_ID", dr));
                        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                        if (string.IsNullOrEmpty(Helper.GetString("POC_RECEIVED_DATE", dr)))
                        {
                            POCdueDate = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
                            parms.Add("POC_DUE_DATE", POCdueDate);
                        }

                        if (string.IsNullOrEmpty(Helper.GetString("REVISED_POC_DATE", dr)))
                        {
                            RevPOCdueDate = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
                            parms.Add("REVISED_POC_DUE_DATE", RevPOCdueDate);
                            // RevPOCDate = string.Empty;
                        }

                        psc.UpdateRegistrationData(regID, "INCIDENT_COMPLIANCE_CASE_XREF", parms);
                    }

                    UpdateProviderStatusNotComplete(CON.RegistrationStatusTypeId.ReturnToProvider);
                }

            }
            if (this.CurrentTaskName == CON.RegistrationTaskName.PlaceRiskAlertClosure && this.WF_WorkflowID == CON.WorkflowType.RiskAlertClosure)
            {
                if (action == "Approve")
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", this.RegistrationId.ToString());
                    parms.Add("PROCESS_ID", Master.ProcessID.ToString());
                    DataSet dsClosure = psc.SelectRegistrationDataWithParams("usp_SelectREG_ClosureNotice", parms);

                    if (Helper.HasRows(dsClosure))
                    {
                        DateTime dtEffectiveDate = Helper.GetDateTime("Closure_Effective_Date", dsClosure.Tables[0].Rows[0]);
                        if (dtEffectiveDate != null)
                            psc.TerminateProvider(regID, dtEffectiveDate, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EnrollStatus.INACTIVE.ToString(), CON.EnrollStatusReason.CLOSED.ToString(), true);
                    }
                }
                if (action == "Remove Risk Alert")
                {
                    Registration.UpdateRegistrationStatus(regID, CON.RegistrationStatusTypeId.Submitted, userID);
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", this.RegistrationId.ToString());
                    parms.Add("PROCESS_ID", Master.ProcessID.ToString());
                    svc.DeleteRegistrationDataWithParams("usp_deletereg_ltc_risk_alert", parms);
                }
            }
        }
        if ((Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAdministrator) ||
            Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAgent)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InternalApplicationsEntry)) &&
            this.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry)
        {
            // Provider is Submitting the Registration
            SetTaxEntityType();
            Registration.UpdateRegistrationStatus(regID, MAXIMUS.Core.Libraries.Constants.RegistrationStatusTypeId.Submitted, userID);
            Registration.SetNodeStatusId(this.RegistrationId, this.RegistrationStep, CON.RegistrationProviderStatusTypeId.Complete);
            psc.updateWF_STEP_Owner(this.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());   //OHPNM-20506 Update the provadmin/poweragent/provagent that is actually submitting Provider data entry
            Master.RedirectURL = "~/Process/SubmissionConfirmation.aspx?RegID=" + this.RegistrationId.ToString();
        }

        return true;
    }

    private void SetProviderStatus(string action)
    {
        //Can this be moved to promote to Active?

        int regID = this.RegistrationId;
        string userID = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        int pgmStatusTypeID = 0;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        //PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.RegistrationId, "APPEAL");
        DataTable dtProvider = new DataTable();

        if (Helper.HasRows(ds))
        {
            dtProvider = ds.Tables[0];
            int partyID = Helper.GetInt("PARTY_ID", dtProvider.Rows[0]);
            int appealStatusID = Helper.GetInt("APPEAL_STATUS_ID", dtProvider.Rows[0]);
            currProgramStatusTypeID = Helper.GetInt("REG_PROGRAM_STATUS_TYPE_ID", dtProvider.Rows[0]);
            string ImmediateYorN = Helper.GetString("IMMEDIATE_DENY_OR_TERMINATE", dtProvider.Rows[0]);
            string effectiveDate = Helper.GetDate("CHANGE_EFFECTIVE_DATE", dtProvider.Rows[0]);
            string requestedEffectiveDate = Helper.GetDate("REQUESTED_EFFECTIVE_DATE", dtProvider.Rows[0]);

            //set end date when approved
            if (action == "Approve" || action == "Approve Provider")
            {
                //if previously terminated program status, move back to maintenance.
                pgmStatusTypeID = (currProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.PendingTermination || currProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Terminated)
                        ? CON.RegistrationProgramStatusTypeId.Maintenance : currProgramStatusTypeID;
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", this.RegistrationId.ToString());
                //set revalidation due date
                if (partyID > 0)
                {
                    string revalDelta = DataAccess.GetAppSetting(CON.AppSettingsKeyName.RevalidationDueDateDelta);
                    int dueDelta = string.IsNullOrEmpty(revalDelta) ? 1 : Convert.ToInt32(revalDelta);
                    parms.Add("END_DATE", Convert.ToDateTime(effectiveDate).AddYears(dueDelta).ToString());
                }
                parms.Add("RESET_TERM", "1");
                parms.Add("TERM_DATE", null);
                parms.Add("TERM_REASON_ID", null);
                parms.Add("ENROLLMENT_STATUS_CODE", CON.EnrollmentStatusCode.Active);
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                psc.UpdateRegistrationDataTable("PROVIDERCustom", parms);


            }
            else if (action == "Deny" || (action == "Terminate") || action == "Terminate Provider" || action == "Send To Interface")
            {
                //setting enrollment status for mmis
                Dictionary<string, string> dictParms = new Dictionary<string, string>();
                dictParms.Add("REG_ID", regID.ToString());

                //if previously terminated program status, move back to maintenance.
                pgmStatusTypeID = (action == "Deny") ? CON.RegistrationProgramStatusTypeId.Denied : CON.RegistrationProgramStatusTypeId.Terminated;
                if (action == "Deny")
                {
                    dictParms.Add("ENROLLMENT_STATUS_CODE", CON.EnrollmentStatusCode.TerminatedEmergency);
                }
                else
                {
                    dictParms.Add("ENROLLMENT_STATUS_CODE", CON.EnrollmentStatusCode.TerminatedTechnical);
                }
                dictParms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                dictParms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                //Term date and reason are collected in Process Appeal save when select term selected.
                psc.UpdateRegistrationDataTable("PROVIDERCustom", dictParms);
            }
            if (pgmStatusTypeID > 0)
                psc.SaveRegistrationProgramStatus(regID, pgmStatusTypeID, userID);
        }

    }

    public MasterWorkflowPage RegMaster
    {
        get { return this.Master as MasterWorkflowPage; }
    }

    //Screening and Site Vist Screening use these at this time
    protected void RefreshNavigationTree()
    {
        Master.RefreshTree();
    }

    protected void Section_LoadAdverseActions()
    {
        if (this.RegistrationId > 0)
        {
            Master.LoadAdverseActions(this.RegistrationId);
        }
    }

    protected void Section_ToggleTakeActionVisibility(ToggleTakeActionVisibilityEventArgs args)
    {
        this.ucRegistrationNavigation.SetTakeActionVisibility(args.IsTakeActionVisible);
    }

    protected void Section_SetDocumentProperties(string sectionName, int screeningActivityID)
    {
        this.ucUploadDocument.DocumentSection = sectionName;
        if (screeningActivityID > 0)
            this.ucUploadDocument.ScreeningActivityID = screeningActivityID;
    }

    protected void btnErrorOk_Click(object sender, EventArgs e)
    {
        //TEMP while work out any issues with the new one user to many registrations changes
        //can make this return to home page instead of remaining onthis page.
    }

    protected void gvConvertedDocs_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void gvConvertedDocs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        ImageButton img = (ImageButton)e.Row.FindControl("imgView");
        if (img != null)
        {
            OnBaseInterface ob = new OnBaseInterface();
            int OnBaseId = int.Parse(DataBinder.Eval(e.Row.DataItem, "ONBASE_DOCUMENT_ID").ToString());
            string isEnycrypted = "";
            if (Boolean.Parse(DataBinder.Eval(e.Row.DataItem, "IS_CONVERSION").ToString()) == true) { isEnycrypted = "&UseEncryption=Y"; }
            string url = "../ViewFile.aspx?OnBaseId=" + ob.ObfuscateId(OnBaseId) + isEnycrypted;
            img.OnClientClick = "window.open('" + url + "');return false;";
        }

    }

    protected void grdvConvertedDocs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridViewCurrentPage = Convert.ToInt32(e.NewPageIndex + 1);
            this.LoadConvertedDocuments(GridViewCurrentPage);
        }
        catch (Exception ex)
        {
            throw CoreException.ThrowException(new Exception("Exception Occurred, please try again"));
        }
    }

    protected void gvPaperDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void gvPaperDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        ImageButton img = (ImageButton)e.Row.FindControl("imgView");
        if (img != null)
        {
            OnBaseInterface ob = new OnBaseInterface();
            int OnBaseId = int.Parse(DataBinder.Eval(e.Row.DataItem, "ONBASE_DOCUMENT_ID").ToString());
            string isEnycrypted = "";
            if (Boolean.Parse(DataBinder.Eval(e.Row.DataItem, "IS_CONVERSION").ToString()) == true) { isEnycrypted = "&UseEncryption=Y"; }
            string url = "../ViewFile.aspx?OnBaseId=" + ob.ObfuscateId(OnBaseId) + isEnycrypted;
            img.OnClientClick = "window.open('" + url + "');return false;";
        }

    }

    protected void PlaceholderUploadSectionControl_PreRender(object sender, EventArgs e)
    {

    }

    protected void imgView_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton img = (ImageButton)sender;
        OnBaseInterface onbaseInterface = new OnBaseInterface();
        var downloadData = onbaseInterface.DownloadOnbaseFile(img.CommandArgument);

        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "attachment");
        Response.Buffer = true;
        HttpContext.Current.Response.BinaryWrite(downloadData);

        HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.

    }

    private void CreatePlaceholderForControlPath(string controlPath, Enumerations.ScreeningEntityType screeningType)
    {
        BaseSectionControl ctrl = LoadControl(controlPath) as BaseSectionControl;
        lblTitle.Text = ctrl.Title;
        ((Button)ucRegistrationNavigation.FindControl("btnSave")).ValidationGroup = ctrl.ValidationGroup;
        ctrl.ID = ctrl.IdText;
        ctrl.InvalidateAgreements += new EventHandler(InvalidateAgreements);
        ctrl.ToggleTakeActionVisibility += Section_ToggleTakeActionVisibility;
        ctrl.ScreeningType = screeningType;
        ctrl.RefreshNavigationTree += RefreshNavigationTree;
        ctrl.LoadAdverseActions += Section_LoadAdverseActions;
        ctrl.SetDocumentProperties += Section_SetDocumentProperties;
        //ctrl.LoadControlData();

        AddSectionToPlaceHolder(ctrl, 0);
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>openModalDialog();</script>", false);

        // SAM537 Suspended provider's provider admin can start ODM update and only change primary contact addr, billing addr, correspondence addr,1099 address and home office addr
        bool enableCR537 = Convert.ToBoolean(AppSettings.Get("EnableCR537", "false"));
        if (enableCR537 && this.IsSuspendedProvider && this.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && this.WF_StepID > 0 && this.WaiverServiceUpdateTypeID == CON.WaiverServiceUpdateType.ODM && !Helper.IsRevertSuspensionWF(this.WF_ProcessID)
             && !(this.RegistrationStep == CON.SectionTypeID.PrimaryContactAddress) && !(this.RegistrationStep == CON.SectionTypeID.BillingPaymentAddress)
             && !(this.RegistrationStep == CON.SectionTypeID.CorrespondenceAddress) && !(this.RegistrationStep == CON.SectionTypeID.Address1099Form) && !(this.RegistrationStep == CON.SectionTypeID.HomeOfficeAddress) && !(this.RegistrationStep == CON.SectionTypeID.RestrictedService))
        {
            Helper.SetReadOnly(ctrl, true, "formFieldReadOnly");
        }
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        SetButtons("History");
        string controlPath = string.Empty;
        switch (Convert.ToInt32(e.CommandArgument))//stepNumber
        {
            // TBD this section need be replaced primary location history has been moved to control level than in registration page.
            case CON.SectionTypeID.PrimaryContactAddress:
                controlPath = "~/PopupControls/PrimaryContactAddressHistory.ascx";
                break;
            case CON.SectionTypeID.PrimaryServiceAddress:
                controlPath = "~/PopupControls/PrimaryServiceAddressHistory.ascx";
                break;
            case CON.SectionTypeID.BillingPaymentAddress:
                controlPath = "~/PopupControls/BillingPaymentAddressHistory.ascx";
                break;
            case CON.SectionTypeID.CorrespondenceAddress:
                controlPath = "~/PopupControls/CorrespondenceAddressHistory.ascx";
                break;
            case CON.SectionTypeID.RemittanceAddress:
                controlPath = "~/PopupControls/RemittanceInformationHistory.ascx";
                break;
            case CON.SectionTypeID.FederalDEA:
                controlPath = "~/PopupControls/CertificationsHistory.ascx";
                break;
            case CON.SectionTypeID.OtherAddress:
                controlPath = "~/PopupControls/OtherAddressHistory.ascx";
                break;
            case CON.SectionTypeID.HomeOfficeAddress:
                controlPath = "~/PopupControls/HomeOfficeAddressHistory.ascx";
                break;
            case CON.SectionTypeID.HospitalAddress:
                controlPath = "~/PopupControls/HospitalAddressHistory.ascx";
                break;
            case CON.SectionTypeID.NursingFacilityAddress:
                controlPath = "~/PopupControls/NursingFacilityAddressHistory.ascx";
                break;
            case CON.SectionTypeID.Insurance:
                controlPath = "~/PopupControls/InsuranceHistory.ascx";
                break;
            case CON.SectionTypeID.CredentialingContact:
                controlPath = "~/PopupControls/CredentialingContactHistory.ascx";
                break;

            // JIRA  4087 
            case CON.SectionTypeID.Address1099Form:
                controlPath = "~/PopupControls/Form1099AddressHistory.ascx";
                break;
            case CON.SectionTypeID.MalpracticeClaimsHistory:
                controlPath = "~/PopupControls/MalpracticeClaimHistory.ascx";
                break;
            //Jira 4156
            case CON.SectionTypeID.StateCDSNumber:
                controlPath = "~/PopupControls/CDSHistory.ascx";
                break;
            case CON.SectionTypeID.BoardCertification:
                controlPath = "~/PopupControls/BoardCertificationHistory.ascx";
                break;
            case CON.SectionTypeID.Licenses:
                controlPath = "~/PopupControls/LicensesHistory.ascx";
                break;
        }

        if (!string.IsNullOrEmpty(controlPath))
        {
            CreatePlaceholderForControlPath(controlPath);
            mpeShowHistory.Show();
        }
    }
    private void CreatePlaceholderForControlPath(string controlPath)
    {
        if (!string.IsNullOrEmpty(controlPath))
        {
            BaseSectionControl ctrl = LoadControl(controlPath) as BaseSectionControl;
            lblHistoryTitle.Text = ctrl.Title;
            ((Button)ucRegistrationNavigation.FindControl("btnSave")).ValidationGroup = ctrl.ValidationGroup;
            ctrl.ID = ctrl.IdText;
            ctrl.InvalidateAgreements += new EventHandler(InvalidateAgreements);
            ctrl.ToggleTakeActionVisibility += Section_ToggleTakeActionVisibility;
            ctrl.ScreeningType = Enumerations.ScreeningEntityType.None;
            ctrl.RefreshNavigationTree += RefreshNavigationTree;
            ctrl.LoadAdverseActions += Section_LoadAdverseActions;
            ctrl.SetDocumentProperties += Section_SetDocumentProperties;
            //ctrl.LoadControlData();

            AddSectionToPlaceHolder(ctrl, 1);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        // TODO: EDV Do we need this?
        if (!Page.IsValid)
        {
            //mpe.Show();
            return;
        }

        if (!GetSectionControlFromPlaceHolder().ValidateData())
        {
            btnSave.Enabled = true;
            //mpe.Show();
            return;
        }
    }

    private void SetButtons(string commandName = "")
    {
        // Save is disabled by default
        btnSave.Visible = false;
        btnCancel.Text = "Close";

        if (Registration.CanUserEditRegistration(this.RegistrationId, this.CurrentTaskName, commandName))
        {
            btnSave.Visible = true;
            btnCancel.Text = "Cancel";
        }
    }

    private void SetVisibility()
    {
        if (EntityTypeId == 0)
        {
            int diddReferralId = 0, entityTypeId = 0, providerTypeId = 0, specialtyTypeID = 0, taxIDTypeID = 0;
            string taxID = string.Empty;        // Defined here but not needed and not used for visible definition
            Registration.SetEntityProviderTypesDIDD(this.RegistrationId, ref entityTypeId, ref providerTypeId, ref diddReferralId, ref specialtyTypeID, ref taxID, ref taxIDTypeID);
            EntityTypeId = entityTypeId;
            ProviderTypeId = providerTypeId;
            DIDDReferralId = diddReferralId;
            SpecialtyTypeID = specialtyTypeID;
        }
    }

    BaseSectionControl GetSectionControlFromPlaceHolder()
    {
        if (sectionPH.Controls.Count > 0)
        {
            return sectionPH.Controls[0] as BaseSectionControl;
        }
        else
            return null;
    }

    void AddSectionToPlaceHolder(Control controlToAdd, int showHistory)
    {
        if (showHistory == 0)
        {
            sectionPH.Controls.Clear();
            sectionPH.Controls.Add(controlToAdd);
        }

        if (showHistory == 1)
        {
            phSectionHistory.Controls.Clear();
            phSectionHistory.Controls.Add(controlToAdd);
        }
    }
    private bool IsInitialNoticeSent()
    {
        bool rtn = false;
        DataSet ds = new DataSet();
        ds = null;
        string sectionName = "Appeals";
        Upload upload = new Upload();
        //   ds = upload.LoadUserSectionControl(8, sectionName);
        ds = upload.LoadUserSectionControl(this.RegistrationId, this.ApplicationTypeID, this.EntityTypeId, this.ProviderTypeId, 8, sectionName);
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                if (Helper.GetString("TITLE", dr) == "Initial Notice")
                {
                    if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")) && Helper.GetInt("DOCUMENT_ID", dr) > 0)
                    {
                        rtn = true;
                        break;
                    }
                }

            }
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds1 = psc.SelectRegistrationData(this.RegistrationId, "APPEAL_NOTICE");
        if (rtn == true && Helper.HasRows(ds1))
        {
            DataTable dtAppealNotices = Helper.HasRows(ds1) ? ds1.Tables[0] : null;
            if (string.IsNullOrEmpty(dtAppealNotices.Rows[0]["INITIAL_NOTICE_DATE"].ToString()))
                rtn = false;
        }
        return rtn;
    }
    private bool IsFinalNoticeSent()
    {
        bool rtn = false;
        DataSet ds = new DataSet();
        ds = null;
        string sectionName = "Appeals";
        Upload upload = new Upload();
        ds = upload.LoadUserSectionControl(this.RegistrationId, this.ApplicationTypeID, this.EntityTypeId, this.ProviderTypeId, 8, sectionName);
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                if (Helper.GetString("TITLE", dr) == "Final Notice")
                {
                    if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")) && Helper.GetInt("DOCUMENT_ID", dr) > 0)
                    {
                        rtn = true;
                        break;
                    }
                }

            }
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds1 = psc.SelectRegistrationData(this.RegistrationId, "APPEAL_NOTICE");
        if (rtn == true && Helper.HasRows(ds1))
        {
            DataTable dtAppealNotices = Helper.HasRows(ds1) ? ds1.Tables[0] : null;
            if (string.IsNullOrEmpty(dtAppealNotices.Rows[0]["FINAL_NOTICE_DATE"].ToString()))
                rtn = false;
            ds = psc.SelectRegistrationData(this.RegistrationId, "PROVIDER");
            string enrollmentStatusCode = string.Empty;
            if (!string.IsNullOrEmpty(Helper.GetString("Enrollment_Status_Code", ds.Tables[0].Rows[0])))
                enrollmentStatusCode = Helper.GetString("ENROLLMENT_STATUS_CODE", ds.Tables[0].Rows[0]);
            if ((string.IsNullOrEmpty(enrollmentStatusCode) || enrollmentStatusCode == CON.EnrollmentStatusCode.Active || enrollmentStatusCode == CON.EnrollmentStatusCode.ReferringProviderOnly || enrollmentStatusCode == CON.EnrollmentStatusCode.MCORenderingProvidersOnly) && this.CurrentTaskName == CON.RegistrationTaskName.UploadFinalNoticeofTermination)
                rtn = false;
            if (String.IsNullOrEmpty(Helper.GetString("Term_Date", ds.Tables[0].Rows[0])))
                rtn = false;
        }

        /*DataSet ds = psc.SelectRegistrationData(SessionVarRetriever.RegistrationId, "APPEAL_NOTICE");
        if (Helper.HasRows(ds))
        {
            DataTable dtAppealNotices = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
            txtDateOfInitialNotice.Text = Helper.FormatDate2(dtAppealNotices.Rows[0]["INITIAL_NOTICE_DATE"].ToString());
            txtDateOfFinalNotice.Text = Helper.FormatDate2(dtAppealNotices.Rows[0]["FINAL_NOTICE_DATE"].ToString());
            ds = psc.SelectRegistration(SessionVarRetriever.RegistrationId);
            ddlTermReason.SelectedItem.Value = Helper.GetString("ENROLLMENT_STATUS_CODE", ds.Tables[0].Rows[0]);
            txtDateOfDenialOrTermination.Text = Helper.GetDate("Term_Date", ds.Tables[0].Rows[0]);
        }*/
        return rtn;
    }
    public bool CanNotEdit
    {
        get
        {
            //Must be a Site Visit Operator and must not be coming from provider search
            return Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name);
        }
    }
    #region viewstate
    protected override void SavePageStateToPersistenceMedium(object viewState)
    {
        // Call Base Method to Not Change Normal Process
        base.SavePageStateToPersistenceMedium(viewState);
        // Retrieve ViewState and Write Out to Page
        LosFormatter format = new LosFormatter();
        StringWriter writer = new StringWriter();
        format.Serialize(writer, viewState);
        string vsRaw = writer.ToString();
        //Response.Write("=========================================================================================<br/>");
        //Response.Write("ViewState Raw: " + Server.HtmlEncode(vsRaw));
        //Response.Write("<br/>=========================================================================================<br/>");
        // Decode ViewState and Write Out to Page
        byte[] buffer = Convert.FromBase64String(vsRaw);
        string vsText = Encoding.ASCII.GetString(buffer);
        //Response.Write("ViewState Text: " + Server.HtmlEncode(vsText));
        //Response.Write("<br/>=========================================================================================<br/>");
        // Parse ViewState -- Turn On Page Tracing
        //ParseViewState(viewState, 0);
    }
    private void ParseViewState(object vs, int level)
    {
        if (vs == null)
        {
            Trace.Warn(level.ToString(), Spaces(level) + "null");
        }
        else if (vs.GetType() == typeof(System.Web.UI.Triplet))
        {
            Trace.Warn(level.ToString(), Spaces(level) + "Triplet");
            ParseViewState((Triplet)vs, level);
        }
        else if (vs.GetType() == typeof(System.Web.UI.Pair))
        {
            Trace.Warn(level.ToString(), Spaces(level) + "Pair");
            ParseViewState((Pair)vs, level);
        }
        else if (vs.GetType() == typeof(System.Collections.ArrayList))
        {
            Trace.Warn(level.ToString(), Spaces(level) + "ArrayList");
            ParseViewState((System.Collections.ArrayList)vs, level);
        }
        else if (vs.GetType().IsArray)
        {
            Trace.Warn(level.ToString(), Spaces(level) + "Array");
            ParseViewState((Array)vs, level);
        }
        else if (vs.GetType() == typeof(System.String))
        {
            Trace.Warn(level.ToString(), Spaces(level) + "'" + vs.ToString() + "'");
        }
        else if (vs.GetType().IsPrimitive)
        {
            Trace.Warn(level.ToString(), Spaces(level) + vs.ToString());
        }
        else if (vs.GetType() == typeof(System.Web.UI.IndexedString))
        {
            Trace.Warn(level.ToString(), Spaces(level) + vs.ToString());
        }
        else if (vs.GetType() == typeof(System.Collections.Specialized.HybridDictionary))
        {
            var dict = (System.Collections.Specialized.HybridDictionary)vs;
            foreach (var item in dict.Keys)
            {
                Trace.Warn(level.ToString(), Spaces(level) + item.ToString() + " " + dict[item].ToString());
            }
        }
        else
        {
            Trace.Warn(level.ToString(), Spaces(level) + vs.GetType().ToString());
        }
    }
    private void ParseViewState(Triplet vs, int level)
    {
        ParseViewState(vs.First, level + 1);
        ParseViewState(vs.Second, level + 1);
        ParseViewState(vs.Third, level + 1);
    }
    private void ParseViewState(Pair vs, int level)
    {
        ParseViewState(vs.First, level + 1);
        ParseViewState(vs.Second, level + 1);
    }

    private void ParseViewState(Array vs, int level)
    {
        foreach (object item in vs)
        {
            ParseViewState(item, level + 1);
        }
    }

    private void ParseViewState(System.Collections.ArrayList vs, int level)
    {
        foreach (object item in vs)
        {
            ParseViewState(item, level + 1);
        }
    }
    private string Spaces(int count)
    {
        string spaces = "";
        for (int index = 0; index < count; index++)
        {
            spaces += "   ";
        }
        return spaces;
    }

    #endregion
    private bool IsPAOUploaded()
    {
        bool rtn = false;
        DataSet ds = new DataSet();
        ds = null;
        string sectionName = "HearingRights";
        Upload upload = new Upload();
        //   ds = upload.LoadUserSectionControl(8, sectionName);
        ds = upload.LoadUserSectionControl(this.RegistrationId, this.ApplicationTypeID, this.EntityTypeId, this.ProviderTypeId, 8, sectionName);
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                if (Helper.GetString("TITLE", dr) == "Proposed Adjudication Order")
                {
                    if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")) && Helper.GetInt("DOCUMENT_ID", dr) > 0)
                    {
                        rtn = true;
                        break;
                    }
                }

            }
        }

        return rtn;
    }
}
