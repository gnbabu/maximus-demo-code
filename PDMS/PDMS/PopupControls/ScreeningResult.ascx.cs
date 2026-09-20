using CustomControls;
using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_ScreeningResult : System.Web.UI.UserControl
{

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    #region svc

    private PDMSService.PDMSServiceClient _svc;
    private PDMSService.PDMSServiceClient svc
    {
        get
        {
            if (_svc == null)
            {
                _svc = new PDMSService.PDMSServiceClient();
            }

            return _svc;
        }
    }

    #endregion

    private const string CustomValidationGroupName = "valProviderInfoHeader";
    private const string NotesLabel = "Screening Notes";

    #region Properties

    public int ScreeningActivityID
    {
        get
        {
            if (ViewState["ScreeningActivityID"] == null)
                ViewState["ScreeningActivityID"] = -1;

            return (int)ViewState["ScreeningActivityID"];
        }
        set
        {
            ViewState["ScreeningActivityID"] = value;
        }
    }

    public int ScreeningActivityTypeID
    {
        get
        {
            if (ViewState["ScreeningActivityTypeID"] == null)
                ViewState["ScreeningActivityTypeID"] = -1;

            return (int)ViewState["ScreeningActivityTypeID"];
        }
        set
        {
            ViewState["ScreeningActivityTypeID"] = value;
        }
    }

    private int AffiliationRegID
    {
        get
        {
            if (ViewState["AffiliationRegID"] == null)
                ViewState["AffiliationRegID"] = 0;

            return (int)ViewState["AffiliationRegID"];
        }
        set
        {
            ViewState["AffiliationRegID"] = value;
        }
    }

    private Enumerations.ScreeningEntityType ScreeningEntityType
    {
        get
        {
            if (ViewState["ScreeningEntityType"] == null)
                ViewState["ScreeningEntityType"] = Enumerations.ScreeningEntityType.Provider;

            int screeningEntityTypeID = (int)ViewState["ScreeningEntityType"];

            return !Enum.IsDefined(typeof(Enumerations.ScreeningEntityType), screeningEntityTypeID) ? Enumerations.ScreeningEntityType.Provider
                : (Enumerations.ScreeningEntityType)screeningEntityTypeID;
        }
        set
        {
            ViewState["ScreeningEntityType"] = value;
        }
    }
    private Enumerations.ResultChangeType ResultChange
    {
        get
        {
            if (ViewState["ResultChange"] == null)
                ViewState["ResultChange"] = Enumerations.ResultChangeType.None;

            return (Enumerations.ResultChangeType)ViewState["ResultChange"];
        }
        set
        {
            ViewState["ResultChange"] = value;
        }
    }

    private bool SearchPerformed
    {
        get
        {
            if (ViewState["SearchPerformed"] == null)
                ViewState["SearchPerformed"] = false;

            return (bool)ViewState["SearchPerformed"];
        }
        set
        {
            ViewState["SearchPerformed"] = value;
        }
    }
    public string ExternalUrlDescription
    {
        get
        {
            if (ViewState["ExternalUrlDescription"] == null)
                ViewState["ExternalUrlDescription"] = string.Empty;

            return (string)ViewState["ExternalUrlDescription"];
        }
        set
        {
            ViewState["ExternalUrlDescription"] = value;
        }
    }
    

    public bool IsReadOnly
    {
        set
        {
            this.ddlMatchResults.Enabled = value;
            this.btnConfirm.Enabled = value;
            this.txtAdverseAction.Enabled = value;
        }
    }

    #endregion


    #region Events

    public delegate void CancelEventHandler(EventArgs args);
    public event CancelEventHandler Cancel;

    public delegate void ScreeningActivityUpdateEventHandler(EventArgs args);
    public event ScreeningActivityUpdateEventHandler ScreeningActivityUpdated;


    //public delegate void CreateAdverseActionEventHandler(PopupControls_AdverseAction.CreateAdverseActionEventArgs args);
    //public event CreateAdverseActionEventHandler CreateAdverseAction;


    #endregion
    
    public void LoadScreeningResult(int screeningActivityID, int screeningActivityTypeID, Enumerations.ScreeningEntityType entityType)
    {
        ScreeningActivityID = screeningActivityID;
        ScreeningActivityTypeID = screeningActivityTypeID;
        ScreeningEntityType = entityType;
        AffiliationRegID = 0;
        int providerTypeID = 0;
        //ucAdverseActionHeader.ScreeningActivityID = screeningActivityID;
        lbltxtPrimaryPracticeState.Visible = false;
        lblPrimaryPracticeState.Visible = false;

        if (screeningActivityID > 0)
        {
            SetUpResultDropDownValues(screeningActivityTypeID);
            
            SearchPerformed = false;

            string tableName = string.Empty;
            this.lblTaxIDLabel.Text = "Tax ID";

            if(!Enum.IsDefined(typeof(Enumerations.ScreeningActivityType), screeningActivityTypeID))
            {
                throw new Exception("Invalid ActivityScreeningTypeID");
            }

            DataSet ds = svc.SelectProviderScreeningActivityMatchData(screeningActivityTypeID, screeningActivityID,
                entityType == Enumerations.ScreeningEntityType.Provider || entityType == Enumerations.ScreeningEntityType.HouseholdMember ? this.WorkflowPage.RegistrationId : -1,
                entityType == Enumerations.ScreeningEntityType.Affiliation ? (this.Page as WorkflowPage).RegAffiliationID : -1,
                entityType == Enumerations.ScreeningEntityType.Owner ? (this.Page as WorkflowPage).RegOwnerID : -1);

            List<SqlParameter> parameters = new List<SqlParameter>();
            if (screeningActivityID > -1) parameters.Add(SqlParms.CreateParameter("SCREENING_ACTIVITY_ID", DbType.Int32, screeningActivityID, true));
            if (entityType == Enumerations.ScreeningEntityType.Provider) parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, this.WorkflowPage.RegistrationId, true));            
            if (entityType == Enumerations.ScreeningEntityType.Owner) parameters.Add(SqlParms.CreateParameter("REG_OWNER_ID", DbType.Int32, (this.Page as WorkflowPage).RegOwnerID, true));
            DataSet dsRegData = DataAccess.ExecuteStoredProcedure("usp_SelectSCREENING_RESULT_PROVIDER_DATA", parameters, "ScreeningActivityRegData");

            if(dsRegData != null && dsRegData.Tables.Count > 0 && dsRegData.Tables[0].Rows.Count > 0)
            {
                DataTable dtRegData = dsRegData.Tables[0];
                DataRow drProvider = dtRegData.Rows[0];

                lblPerformedBy.Text = !string.IsNullOrWhiteSpace(drProvider.GetString("PERFORMED_BY_USERNAME")) ? drProvider.GetString("PERFORMED_BY_USERNAME") : HttpContext.Current.User.Identity.Name.ToString();
                txtAdverseAction.Text = drProvider.GetString("DESCRIPTION");

                DateTime? screeningDate = drProvider.GetValue<DateTime?>("SCREENING_PERFORMED_DATE_TIME");
                lblScreeningDate.Text = screeningDate.HasValue ? screeningDate.Value.ToString("MM/dd/yy") : string.Empty;

                int screeningActivityStatusID = drProvider.GetValue<int>("SCREENING_ACTIVITY_STATUS_ID");

                if (ddlMatchResults.Items.FindByValue(screeningActivityStatusID.ToString()) != null)
                {
                    ddlMatchResults.SelectedValue = screeningActivityStatusID.ToString();
                    if (screeningActivityTypeID == (int)Enumerations.ScreeningActivityType.CriminalBackgroundCheck)
                        ddlMatchResults.Enabled = false;
                }

                //SetCreateAdverseActionVisibility(screeningActivityStatusID);

                string externalSearchUrl = drProvider.GetString("EXTERNAL_CHECK_URL");
                string externalSearchUrlDescription = drProvider.GetString("EXTERNAL_CHECK_URL_DESCRIPTION");
                string extraExternalSearchUrl = drProvider.GetString("EXTRA_EXTERNAL_CHECK_URL");
                string extraExternalSearchUrlDescription = drProvider.GetString("EXTRA_EXTERNAL_CHECK_URL_DESCRIPTION");

                if (lnkExtraReference.Controls.Count > 0) lnkExtraReference.Controls.Clear();
                if (lnkReference.Controls.Count > 0) lnkReference.Controls.Clear();

                if (!string.IsNullOrEmpty(externalSearchUrl) && screeningActivityTypeID != (int)Enumerations.ScreeningActivityType.LicenseVerification)
                {
                    //lnkExternalSearch.Visible = false;
                    //this.lnkExternalSearch.Text = string.Empty;
                    HyperLink dynLink = new HyperLink();
                    dynLink.ID = "DynLnk";
                    dynLink.Text = externalSearchUrlDescription;
                    dynLink.NavigateUrl = externalSearchUrl;
                    dynLink.Target = "_blank";
                    dynLink.CssClass = "formLabel";
                    lnkReference.Controls.Add(dynLink);
                    btnConfirm.Enabled = true;
                }

                //TODO:  hide show based on license data, for now always show
                if (!string.IsNullOrEmpty(extraExternalSearchUrl))
                {
                    HyperLink dynLink = new HyperLink();
                    dynLink.ID = "ExtraDynLnk";
                    dynLink.Text = extraExternalSearchUrlDescription;
                    dynLink.NavigateUrl = extraExternalSearchUrl;
                    dynLink.Target = "_blank";
                    dynLink.CssClass = "formLabel";
                    lnkExtraReference.Controls.Add(dynLink);
                    btnConfirm.Enabled = true;
                }

                lblTaxID.Text = drProvider.GetString("TAX_ID");
                lblNPI.Text = drProvider.GetString("NPI");
                lblOrganizationName.Text = drProvider.GetString("ORGANIZATION_NAME");
                lblIndividualName.Text = drProvider.GetString("INDIVIDUAL_NAME");
                if (Helper.ContainsColumn("AFFILIATION_REG_ID", dtRegData))
                {
                    AffiliationRegID = Helper.GetInt("AFFILIATION_REG_ID", drProvider);
                }
                if (screeningActivityID == (int)Enumerations.ScreeningActivityType.SiteVisitVerification)
                    providerTypeID = Helper.GetInt("PROVIDER_TYPE_ID", drProvider);

                if (screeningActivityTypeID == (int)Enumerations.ScreeningActivityType.LicenseVerification)
                {
                    lbltxtPrimaryPracticeState.Visible = true;
                    lblPrimaryPracticeState.Visible = true;
                    lblPrimaryPracticeState.Text = drProvider.GetString("LICENSE_STATE");
                }

                if(screeningActivityTypeID == (int)Enumerations.ScreeningActivityType.PECOSVerfication
                        || screeningActivityTypeID == (int)Enumerations.ScreeningActivityType.SAVEVerification)
                {
                    PopulateScreeningSpecificData(dtRegData, screeningActivityTypeID);
                }
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dtMatch = ds.Tables[0];
             //   DataRow drMatch = dtMatch.Rows[0];

                if (screeningActivityTypeID != (int)Enumerations.ScreeningActivityType.PECOSVerfication
                        && screeningActivityTypeID != (int)Enumerations.ScreeningActivityType.SAVEVerification)
                {
                    if (screeningActivityTypeID == (int)Enumerations.ScreeningActivityType.SiteVisitVerification)
                        SetUpSiteVisitStatusDropDownValues(providerTypeID);
                    if (screeningActivityTypeID == (int)Enumerations.ScreeningActivityType.CriminalBackgroundCheck)
                        SetUpBackgroundCheckDropDownValues(providerTypeID);
                    if (screeningActivityTypeID == (int)Enumerations.ScreeningActivityType.DODDAbuserRegistry)
                    {
                        DataTable dtDODD = ds.Tables[2];
                        PopulateScreeningSpecificData(dtDODD, screeningActivityTypeID);
                    }
                    else
                        PopulateScreeningSpecificData(dtMatch, screeningActivityTypeID);
                }

            }
        }
    }


    private void SetUpResultDropDownValues(int screeningActivityTypeID)
    {
        ddlMatchResults.Items.Clear();

        DataSet ds = svc.SelectScreeningActivityStatusByActivityTypeID(screeningActivityTypeID);
        if (!Helper.HasRows(ds))
        {
            throw new ArgumentException(string.Format("Unsupported Screening Activity Type ID: {0}", screeningActivityTypeID));
        }

        Helper.LoadDropDown(this.ddlMatchResults, ds.Tables[0], "SCREENING_ACTIVITY_STATUS_NAME", "SCREENING_ACTIVITY_STATUS_ID", false);
        lblResultDropDownLabel.Text = ScreeningHelper.DatabaseCheckActivityType(screeningActivityTypeID) ? "Match Result": "Verification Result";

    }

    private void SetUpBackgroundCheckDropDownValues(int providerTypeID)
    {
        //TODO: Enable criminal bg verification
        ddlBackgroundVerificationStatus.Items.Clear();
        DataSet dsVerificationStatuses = svc.SelectBackgroundVerificationStatusTypes();
        Helper.LoadDropDown(ddlBackgroundVerificationStatus, dsVerificationStatuses.Tables[0], "BACKGROUND_VERIFICATION_STATUS_TYPE", "BACKGROUND_VERIFICATION_STATUS_TYPE_ID", true);



    }
    private void SetUpSiteVisitStatusDropDownValues(int providerTypeID)
    {


        ddlSiteVisitVerificationStatus.Items.Clear();
        DataSet dsVerificationStatuses = svc.SelectSiteVisitScreeningStatuses(providerTypeID, 1);
        Helper.LoadDropDown(ddlSiteVisitVerificationStatus, dsVerificationStatuses.Tables[0], "SITE_VISIT_SCREENING_STATUS_NAME", "SITE_VISIT_SCREENING_STATUS_ID", true);

    }

    //private void SetCreateAdverseActionVisibility(int screeningActivityStatusID)
    //{
    //    if (screeningActivityStatusID == CON.ScreeningActivityStatusId.Match ||
    //        screeningActivityStatusID == CON.ScreeningActivityStatusId.Failed)
    //    {
    //        ucAdverseActionHeader.CreateAdverseActionButtonVisible = true;
    //    }
    //    else
    //    {
    //        ucAdverseActionHeader.CreateAdverseActionButtonVisible = false;
    //    }
    //}

    private void PopulateScreeningSpecificData(DataTable dtMatch, int screeningActivityTypeID)
    {
        switch (screeningActivityTypeID)
        {
            case (int)Enumerations.ScreeningActivityType.OIGLEIEVerification:
                mvScreeningSpecificResults.SetActiveView(vwScreeningResultsGrid);
                //PopulateOIGLEIEMatchResults(dtMatch);
                this.PopulateScreeningMatchResultsGrid(dtMatch);
                break;
            case (int)Enumerations.ScreeningActivityType.SSDMFVerification:
                mvScreeningSpecificResults.SetActiveView(vwSSDMFResults);
                PopulateSSDMFMatchResults(dtMatch);
                break;
            case (int)Enumerations.ScreeningActivityType.SAMVerification:
                mvScreeningSpecificResults.SetActiveView(vwScreeningResultsGrid);
                //PopulateSAMMatchResults(dtMatch);
                this.PopulateScreeningMatchResultsGrid(dtMatch);
                break;
            case (int)Enumerations.ScreeningActivityType.MCSISVerification:
                mvScreeningSpecificResults.SetActiveView(vwScreeningResultsGrid);
                //PopulateMCSISMatchResults(dtMatch);
                this.PopulateScreeningMatchResultsGrid(dtMatch);
                break;
            case (int)Enumerations.ScreeningActivityType.NEMEPLVerification:
                mvScreeningSpecificResults.SetActiveView(vwNEMEPLResults);
                this.PopulateNEMEPLMatchResults(dtMatch);
                break;
            case (int)Enumerations.ScreeningActivityType.NPIVerification:
                mvScreeningSpecificResults.SetActiveView(vwNPPESResults);
                this.PopulateNPPESMatchResults(dtMatch);
                break;
            case (int)Enumerations.ScreeningActivityType.LicenseVerification:
                mvScreeningSpecificResults.SetActiveView(vwLicenseResults);
                this.PopulateLicenseRegData();
                break;
            case (int)Enumerations.ScreeningActivityType.PECOSVerfication:
                mvScreeningSpecificResults.SetActiveView(vwPECOSResults);
                this.PopulatePECOSMatchResults(dtMatch);
                break;
            case (int)Enumerations.ScreeningActivityType.SAVEVerification:
                mvScreeningSpecificResults.SetActiveView(vwSAVEResults);
                this.PopulateSAVEMatchResults(dtMatch);
                break;
            case(int)Enumerations.ScreeningActivityType.DEAVerification:
                mvScreeningSpecificResults.SetActiveView(vwDEAResults);
                this.PopulateDEAMatchResults(dtMatch);
                //this.PopulateDEARegData();
                break;
            case (int)Enumerations.ScreeningActivityType.CriminalBackgroundCheck:
                mvScreeningSpecificResults.SetActiveView(vwBackgroundResults);
                this.PopulateBackgroundRegData();
                break;
            case (int)Enumerations.ScreeningActivityType.SiteVisitVerification:
                mvScreeningSpecificResults.SetActiveView(vwSiteVisitResults);
                this.PopulateSiteVisitResults();
                break;
            case (int)Enumerations.ScreeningActivityType.ControlledSubstanceVerification:
                mvScreeningSpecificResults.SetActiveView(vwCDSResults);
                this.PopulateCDSResults();
                break;
            case (int)Enumerations.ScreeningActivityType.MEDVerification:
                mvScreeningSpecificResults.SetActiveView(vwScreeningResultsGrid);
                //this.PopulateMEDResults(dtMatch);
                this.PopulateScreeningMatchResultsGrid(dtMatch);
                break;
            case (int)Enumerations.ScreeningActivityType.DODDAbuserRegistry:
                mvScreeningSpecificResults.SetActiveView(vwDODDResults);
                this.PopulateDODDResults(dtMatch);
                break;
            case (int)Enumerations.ScreeningActivityType.OHMedExclSuspension:
                mvScreeningSpecificResults.SetActiveView(vwOHExclSuspension);
                this.PopulateOHExclSuspensionResults(dtMatch);
                break;
            default:
                mvScreeningSpecificResults.ActiveViewIndex = -1;
                break;
        }
    }
    //SAM758
    private void PopulateScreeningMatchResultsGrid(DataTable dtMatch)
    {
        grdScreeningResults.DataSource = dtMatch;
        grdScreeningResults.DataBind();

        BindExclusionPanel(dtMatch);
    }

    protected void grdScreeningResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    private void PopulateOHExclSuspensionResults(DataTable dtMatch)
    {
        DataRow dr = dtMatch.Rows[0];
        lblOHfname.Text = dr.GetString("FIRST_NAME");
        lblOHlname.Text = dr.GetString("LAST_NAME");
        lblOHMname.Text = dr.GetString("MIDDLE_NAME");
        lblOHTaxID.Text = dr.GetString("SSN");
        lblOHOrgname.Text = dr.GetString("ORG_NAME");
        lblOHNpi.Text = dr.GetString("NPI");
        lblOHMedID.Text = dr.GetString("PROVIDER_ID");
        lblOHDOB.Text = dr.GetString("DOB");
        lblOHStatus.Text = dr.GetString("STATUS");
        lblOHActionDt.Text = dr.GetString("ACTION_DATE");
        lblOHProvType.Text = dr.GetString("PROVIDER_TYPE");
        lblOHAddress.Text = dr.GetString("Address_1") + " " + dr.GetString("Address_2") + " " + dr.GetString("CITY") + " " + dr.GetString("STATE") + " " + dr.GetString("ZIP");
    }

    private void PopulateMEDResults(DataTable dtMatch)
    {
        DataRow dr = dtMatch.Rows[0];
        lblMED_Address.Text = dr.GetString("PracticeAddress");
        lblMED_DateOfDeath.Text = dr.GetString("DateOfDeath");
        lblMED_DOB.Text = dr.GetString("DateOfBirth");
        lblMED_EIN.Text = dr.GetString("EIN");
        lblMED_MEDICARE.Text = dr.GetString("MedicareNumber");
        lblMED_NPI.Text = dr.GetString("NPI");
        lblMED_Org.Text = dr.GetString("Organization");
        lblMED_ProviderType.Text = dr.GetString("ProviderType");
        lblMED_ReinstatementDate.Text = dr.GetString("ReinstatementDate");
        lblMED_SanctionDate.Text = dr.GetString("SanctionDate");
        lblMED_SanctionType.Text = dr.GetString("SanctionDescription");
        lblMED_SSN.Text = dr.GetString("SSN");
        lblMED_WaiverEffectiveDate.Text = dr.GetString("WaiverEffectiveDate");
        lblMED_WaiverEndDate.Text = dr.GetString("WaiverEndDate");
        lblMED_WaiverNotes.Text = dr.GetString("WaiverNotes");
        lblMED_LastName.Text = dr.GetString("LastName");
        lblMED_FirstName.Text = dr.GetString("FirstName");
        lblMED_MidInitial.Text = dr.GetString("MiddleName");
    }

    private void PopulateDEARegData()
    {
        int regID = this.WorkflowPage.RegistrationId;
        if (ScreeningEntityType == Enumerations.ScreeningEntityType.Affiliation)
        {
            //get the registration id for the affiliation
            regID = AffiliationRegID;
        }
        DataSet ds = svc.SelectRegistrationData(regID, "DEA");
        DataTable dt = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        this.grdDEANumbers.DataSource = dt;
        this.grdDEANumbers.DataBind();

        //if (Helper.HasRows(ds))
        //{
        //    DataRow row = dt.Rows[0];
        //    txtDEANumber.Text = Helper.GetString("DEA_NUMBER", row);
        //    txtDEAEffectiveDate.Text = Helper.GetDate("DEA_EFF_DATE", row);
        //    txtDEAExpireDate.Text = Helper.GetDate("DEA_END_DATE", row);
        //}
    }

    //TODO: Enable background verification
    private void PopulateBackgroundRegData()
    {
        int regID = this.WorkflowPage.RegistrationId;
        ddlMatchResults.Enabled = false;

        DataSet ds = svc.SelectRegistrationData(regID, "BACKGROUND_VERIFICATION");
        if (Helper.HasRows(ds))
        {
            DataTable dt = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
            ddlBackgroundVerificationStatus.SelectedValue = Helper.GetString("BACKGROUND_VERIFICATION_STATUS_TYPE_ID", dt.Rows[0]);
            txtBackgroundCheckDate.Text = Helper.GetDate("BACKGROUND_VERIFICATION_DATE", dt.Rows[0]).ToString();
        }
        
    }

    private void PopulateOIGLEIEMatchResults(DataTable dtMatch)
    {
        DataRow dr = dtMatch.Rows[0];

        lblOIGLastName.Text = dr.GetString("LASTNAME");
        lblOIGFirstName.Text = dr.GetString("FIRSTNAME");
        lblOIGMiddleName.Text = dr.GetString("MIDNAME");
        lblOIGBusinessName.Text = dr.GetString("BUSNAME");
        lblOIGGeneral.Text = dr.GetString("GENERAL");
        lblOIGSpecialty.Text = dr.GetString("SPECIALTY");
        lblOIGUPIN.Text = dr.GetString("UPIN");
        lblOIGNPI.Text = dr.GetString("NPI");
        lblOIGDOB.Text = dr.GetString("DOB");
        lblOIGAddress.Text = dr.GetString("ADDRESS");
        lblOIGCity.Text = dr.GetString("CITY");
        lblOIGState.Text = dr.GetString("STATE");
        lblOIGZipCode.Text = dr.GetString("ZIPCODE");
        lblOIGExclType.Text = dr.GetString("EXCLTYPE");
        lblOIGExclDate.Text = dr.GetString("EXCLDATE");
        lblOIGReinDate.Text = dr.GetString("REINDATE");
        lblOIGWaiverDate.Text = dr.GetString("WAIVERDATE");
        lblOIGWaiverState.Text = dr.GetString("WAIVERSTATE");
    }

    private void PopulateMCSISMatchResults(DataTable dtMatch)
    {
        DataRow dr = dtMatch.Rows[0];
        lblMCSISEnrollType.Text = dr.GetString("ENROLLMENT_TYPE");
        lblMCSISNPI.Text = dr.GetString("NPI");
        lblMCSISLastName.Text = dr.GetString("LASTNAME");
        lblMCSISFirstName.Text = dr.GetString("FIRSTNAME");
        lblMCSISBusinessName.Text = dr.GetString("ORGANIZATION");
        lblMCSISAddress.Text = dr.GetString("PRACTICEADDRESS");
        lblMCSISEIN.Text = dr.GetString("EIN");
        lblMCSISTAXID.Text = dr.GetString("SSN");
        lblMCSISAdverseAction.Text = dr.GetString("ADVERSE_ACTION_REASONS");
        lblEffectiveDate.Text = Helper.FormatDate2(dr.GetString("EFFECTIVE_DATE"));

        lblMCSISTermProgram.Text = dr.GetString("TERMINATINGPROGRAM");
             
        lblMCSISAddress.Text = Helper.GetString("CORRESPONDENCEADDRESS", dr) + "<br /> " + Helper.GetString("CORRESPONDENCE_ADDRESS_CITY", dr) + " " + Helper.GetString("CORRESPONDENCE_ADDRESS_STATE", dr) + " " + Helper.FormatZipcode(Helper.GetString("CORRESPONDENCE_ADDRESS_ZIP", dr));
        lblMCSISPracticeLoc.Text = Helper.GetString("PRACTICEADDRESS", dr) + "<br /> " + Helper.GetString("PRACTICEADDRESS_CITY", dr) + " " + Helper.GetString("PRACTICEADDRESS_STATE", dr) + " " + Helper.FormatZipcode(Helper.GetString("CORRESPONDENCE_ADDRESS_ZIP", dr));
        lblMCSISAppealsProgram.Text = dr.GetString("APPEALS_PERIOD_EXPIRED");
        lblMCSISStatus.Text = dr.GetString("STATUS");
        lblMCSISCMSPublisedDate.Text = Helper.FormatDate2(dr.GetString("CMS_PUBLISHED_DATE"));
        lblMCSISAddress.Text = Helper.GetString("CORRESPONDENCEADDRESS", dr) + "<br /> " + Helper.GetString("CORRESPONDENCE_ADDRESS_CITY", dr) + " " + Helper.GetString("CORRESPONDENCE_ADDRESS_STATE", dr) + " " + Helper.GetString("CORRESPONDENCE_ADDRESS_ZIP", dr);
        lblMCSISPracticeLoc.Text = Helper.GetString("PRACTICEADDRESS", dr) + "<br /> " + Helper.GetString("PRACTICEADDRESS_CITY", dr) + " " + Helper.GetString("PRACTICEADDRESS_STATE", dr) + " " + Helper.GetString("CORRESPONDENCE_ADDRESS_ZIP", dr); ;
        lblMCSISActiveEnrollmentbar.Text = dr.GetString("ACTIVE_ENROLLMENT_BAR");
        lblMCSISEnrollmentExpireddate.Text = dr.GetString("ENROLLMENT_BAR_EXPIRATON_DATE");
        lblMCSISMedicareState.Text = dr.GetString("MEDICARE_STATE");
        lblMCSISEnrollmentId.Text = dr.GetString("ENROLLMENT_ID");
        
    }

    private void PopulateSSDMFMatchResults(DataTable dtMatch)
    {
        DataRow dr = dtMatch.Rows[0];

        lblSSDMFSSN.Text = dr.GetString("SSN");
        lblSSDMFFirstName.Text = dr.GetString("FIRST_NAME");
        lblSSDMFMiddleName.Text = dr.GetString("MIDDLE_NAME");
        lblSSDMFLastName.Text = dr.GetString("LAST_NAME");
        lblSSDMFNameSuffix.Text = dr.GetString("NAME_SUFFIX");
        lblSSDMFDateOfBirth.Text = dr.GetString("DATE_OF_BIRTH");
        lblSSDMFDateOfDeath.Text = dr.GetString("DATE_OF_DEATH");
    }

    private void PopulateDEAMatchResults(DataTable dtMatch)
    {
        DataRow dr = dtMatch.Rows[0];
        txtDEANumber.Text = dr.GetString("DEA_REGISTRATION_NUMBER");
        txtDEAState.Text = dr.GetString("STATE");
        //txtDEAEffectiveDate.Text = string.Empty; // dr.GetString("");
        txtDEAExpireDate.Text = dr.GetString("EXPIRATION_DATE");
        txtDEAStatus.Text = dr.GetString("ACTIVITY");
    }

    private void PopulateSAMMatchResults(DataTable dtMatch)
    {
        DataRow dr = dtMatch.Rows[0];

        lblSAMBusinessName.Text = dr.GetString("FIRM_NAME");
        lblSAMNamePrefix.Text = dr.GetString("PREFIX");
        lblSAMFirstName.Text = dr.GetString("FIRST_NAME");
        lblSAMMiddleName.Text = dr.GetString("MIDDLE_NAME");
        lblSAMLastName.Text = dr.GetString("LAST_NAME");
        lblSAMNameSuffix.Text = dr.GetString("SUFFIX");
        lblSAMAddress1.Text = dr.GetString("ADDRESS_1");
        lblSAMAddress2.Text = dr.GetString("ADDRESS_2");
        lblSAMAddress3.Text = dr.GetString("ADDRESS_3");
        lblSAMAddress4.Text = dr.GetString("ADDRESS_4");
        lblSAMCity.Text = dr.GetString("CITY");
        lblSAMStateProvince.Text = dr.GetString("STATE_PROVINCE");
        lblSAMCountry.Text = dr.GetString("COUNTRY");
        lblSAMZipCode.Text = dr.GetString("ZIP_CODE");
        lblSAMDuns.Text = dr.GetString("DUNS");
        lblSAMExclusionProgram.Text = dr.GetString("EXCLUSION_PROGRAM");
        lblSAMExcludingAgency.Text = dr.GetString("EXCLUDING_AGENCY");
        lblSAMCTCode.Text = dr.GetString("CT_CODE");
        lblSAMExclusionType.Text = dr.GetString("EXCLUSION_TYPE");
        lblSAMAdditionalComments.Text = dr.GetString("ADDITIONAL_COMMENTS");
        lblSAMActiveDate.Text = dr.GetString("ACTIVE_DATE");
        lblSAMTerminationDate.Text = dr.GetString("TERMINATION_DATE");
        lblSAMRecordStatus.Text = dr.GetString("RECORD_STATUS");
        lblSAMCrossReference.Text = dr.GetString("CROSS_REFERENCE");
        lblSAMNumber.Text = dr.GetString("SAM_NUMBER");
        lblSAMClassification.Text = dr.GetString("CLASSIFICATION");
    }

    private void PopulateNEMEPLMatchResults(DataTable dtMatch)
    {
        DataRow dr = dtMatch.Rows[0];

        this.lblNEMEPLNPI.Text = dr.GetString("NPI");
        this.lblNEMEPLProviderName.Text = dr.GetString("PROVIDERNAME");
        this.lblNEMEPLProviderType.Text = dr.GetString("PROVIDERTYPE");
        this.lblNEMEPLTerminationOrSuspension.Text = dr.GetString("TERMINATIONORSUSPENSION");
        this.lblNEMEPLEffectiveDate.Text = dr.GetString("EFFECTIVEDATE");
        this.lblNEMEPLTerm.Text = dr.GetString("TERM");

        this.lblNEMEPLEndDate.Text = dr.GetString("ENDDATE");
        this.lblNEMEPLReasonForAction.Text = dr.GetString("REASONFORACTION");
    }

    private void PopulateNPPESMatchResults(DataTable dtMatch)
    {
        DataRow dr = dtMatch.Rows[0];

        this.lblNPPESNPI.Text = dr.GetString("NPI");
        this.lblNPPESEntityType.Text =Helper.GetInt("ENTITYTYPE", dr).ToString();
        this.lblNPPESOrgName.Text = dr.GetString("ORGANIZATIONNAME");
        this.lblNPPESFirstName.Text = dr.GetString("FIRSTNAME");
        this.lblNPPESMiddleName.Text = dr.GetString("MIDDLENAME");
        this.lblNPPESLastName.Text = dr.GetString("LASTNAME");
        this.lblNPPESAddressState.Text = dr.GetString("MAILINGADDRESSSTATENAME");
    }

    private void PopulateLicenseRegData()
    {
        int regID = this.WorkflowPage.RegistrationId;
        if (ScreeningEntityType == Enumerations.ScreeningEntityType.Affiliation)
        {
            //get the registration id for the affiliation
            regID = AffiliationRegID;
        }
        DataSet ds = svc.SelectRegistrationData(regID, "LICENSES");
        DataTable dt = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        this.grdLicenses.DataSource = dt;
        this.grdLicenses.DataBind();

        string stateName = GetMyState();
        //this.lnkExternalSearch.Visible = InStateProvider(dt, stateName);
        //this.lnkExtraExternalSearch.Visible = OutOfStateProvider(dt, stateName);
    }

    private void PopulatePECOSMatchResults(DataTable dtRegData)
    {
        if (ScreeningEntityType != Enumerations.ScreeningEntityType.Owner)
        {
            this.pnlPecos.Visible = false;
            return;
        }

        DataRow dr = dtRegData.Rows[0];
        //Helper.LoadDropDownListWithStates(ref this.ddlPECOSState);
        
        DataSet dsRisk = svc.SelectProviderRiskLevels();
        DataTable dtRisk = FilterPecosRiskLevel(dsRisk);
        //Helper.LoadDropDown(this.ddlPECOSRiskLevel, dtRisk, "PROVIDER_RISK_LEVEL_NAME", "PROVIDER_RISK_LEVEL_ID", false);
        //this.ddlPECOSRiskLevel.Items.Insert(0, new ListItem(string.Empty, "0"));

        //ddlPECOSState.SelectedValue = Helper.GetString("PECOS_ENROLLED_STATE", dr).ToString();
        //ddlPECOSRiskLevel.SelectedValue = Helper.GetInt("PECOS_RISK_LEVEL_ID", dr).ToString();
        ddlPECOSBackgroundComplete.SelectedValue = Helper.GetString("PECOS_BACKGROUND_COMPLETE", dr);
        txtPECOSBackgroundDate.Text = Helper.GetString("PECOS_BACKGROUND_DATE", dr);
    }

    private void PopulateSiteVisitResults()
    {
        DataSet dsSiteVisitScreening = svc.SelectSiteVisitScreeningData(this.WorkflowPage.RegistrationId, (int)Enumerations.ScreeningActivityType.SiteVisitVerification);
        if (Helper.HasRows(dsSiteVisitScreening))
        {
            //DataRow dr = dsSiteVisitScreening.Tables[0].Rows[0];
            DataTable dt = dsSiteVisitScreening.Tables[0];
            IEnumerable<DataRow> dr = from row in dt.AsEnumerable()
                                      where (row.Field<int>("SCREENING_ACTIVITY_ID") == ScreeningActivityID)
                                      select row;
            if (dr.Any())
            {
                DataTable dtRow = dr.CopyToDataTable();
                string statusID = Helper.GetString("SITE_VISIT_SCREENING_STATUS_ID", dtRow.Rows[0]);
                if (!(ddlSiteVisitVerificationStatus.Items.FindByValue(statusID) == null))
                {
                    ddlSiteVisitVerificationStatus.SelectedValue = Helper.GetString("SITE_VISIT_SCREENING_STATUS_ID", dtRow.Rows[0]);
                    txtSiteVisitDate.Text = Helper.GetDate("END_DATE", dtRow.Rows[0]).ToString();
                }
            }
        }
    }

    private void PopulateCDSResults()
    {
        DataSet dsCDSScreening = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "STATE_CDS_NUMBER");
        if (Helper.HasRows(dsCDSScreening))
        {
            grdCDSNumbers.DataSource = dsCDSScreening;
            grdCDSNumbers.DataBind();
        }
    }

    private DataTable FilterPecosRiskLevel(DataSet riskLevels)
    {
        StringBuilder selectPart = new StringBuilder();
        //Filter for just nfocus
        selectPart.Append("PROVIDER_RISK_LEVEL_ID <> 0");

        DataTable dt = riskLevels.Tables[0];
        if (dt.Select(selectPart.ToString()).Count() > 0)
        {
            return dt.Select(selectPart.ToString()).CopyToDataTable();
        }
        return dt;
    }

    private void PopulateSAVEMatchResults(DataTable dtRegData)
    {
        DataRow dr = dtRegData.Rows[0];

        lblSAVECitizenshipType.Text = Helper.GetString("CITIZENSHIP_TYPE_NAME", dr);
        lblSAVEImmigrationStatus.Text = Helper.GetString("IMMIGRATION_STATUS_NAME", dr); ;
        this.lblSAVEAlienNumber.Text = Helper.GetString("ALIEN_NUMBER", dr);
    }

    private void PopulateDODDResults(DataTable dtMatch)
    {
        if (dtMatch.Rows.Count > 0)
        {
            DataRow dr = dtMatch.Rows[0];
            var doddResults = JsonConvert.DeserializeObject<DODDResponse>(dr.GetString("DODD_RESPONSE"));
            lblfirstName.Text = doddResults.Firstname;
            lblLastName.Text = doddResults.Lastname;
            DateTime dat = Convert.ToDateTime(doddResults.dob);
            lblDOB.Text = dat.ToString("MM/dd/yy");
            DateTime regData = Convert.ToDateTime(doddResults.DateValue);
            lblRegistryDate.Text = regData.ToString("MM/dd/yy");                    
            lblRegistryReason.Text = doddResults.inccatdesc;
            lblSSN.Text = Helper.GetString("SSN", dr);
        }
    }


    private string GetMyState()
    {
        return AppSettings.Get("StateCode", string.Empty);
    }

    private bool InStateProvider(DataTable dtLicenses, string stateName)
    {
        bool IsInStateProvider = false;
        if (dtLicenses == null || dtLicenses.Rows.Count == 0)
            return IsInStateProvider;

        StringBuilder selectPart = new StringBuilder();
        selectPart.Append(string.Format("LICENSE_STATE = '{0}'", stateName));
        if (dtLicenses.Select(selectPart.ToString()).Count() > 0)
        {
            IsInStateProvider = true;
        }

        return IsInStateProvider;
    }
    private bool OutOfStateProvider(DataTable dtLicenses, string stateName)
    {
        bool outOfStateProvider = false;
        if (dtLicenses == null || dtLicenses.Rows.Count == 0)
            return outOfStateProvider;
        StringBuilder selectPart = new StringBuilder();
        selectPart.Append(string.Format("LICENSE_STATE <> '{0}'", stateName));
        if (dtLicenses.Select(selectPart.ToString()).Count() > 0)
        {
            outOfStateProvider = true;
        }

        return outOfStateProvider;
    }

    protected void lnkExternalSearch_Click(object sender, EventArgs e)
    {
        //btnConfirm.Enabled = true;
        //btnConfirm.OnClientClick = string.Empty;
        //SearchPerformed = true;
       
    }
    protected void btnCancelScreeningResult_Click(object sender, EventArgs e)
    {
        if (Cancel != null)
        {
            Cancel(new EventArgs());
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        SetResultChangeType();
        bool result = ValidateResultChange(ResultChange);

        if (result)
        {
            int screeningActivityStatusID = int.Parse(ddlMatchResults.SelectedValue);
            /// Update the Screening Activity Status
            svc.UpdateScreeningActivityStatus(ScreeningActivityID, screeningActivityStatusID, this.txtAdverseAction.Text.Trim(), Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            UpdateCustomScreeningData();

            if (ScreeningActivityUpdated != null)
            {
                ScreeningActivityUpdated(new EventArgs());
            }

        }       
    }

    private void SetResultChangeType()
    {
        if (!string.IsNullOrEmpty(ddlMatchResults.SelectedValue))
        {
            int newStatusID = int.Parse(ddlMatchResults.SelectedValue);

            /// need to know if change to status + activity type = one that requires link, document upload and/or comments.
            if (ScreeningHelper.CompleteNegativeResultActivityStatus(ScreeningActivityTypeID, newStatusID))
            {
                ResultChange = Enumerations.ResultChangeType.ToNegativeResult;  
                if (ScreeningHelper.CommentsRequiredActivityStatus(ScreeningActivityTypeID, newStatusID))
                {
                    this.lblAdverseAction.Text = string.Format("{0}:*", NotesLabel);
                }
            }
            else if (ScreeningHelper.CompletePositiveResultActivityStatus(ScreeningActivityTypeID, newStatusID))
            {
                ResultChange = Enumerations.ResultChangeType.ToPositiveResult;
            }
            else
            {
                ResultChange = Enumerations.ResultChangeType.None;
            }
        }
        else
        {
            ResultChange = Enumerations.ResultChangeType.None;
        }
    }

    private bool ValidateResultChange(Enumerations.ResultChangeType changeType)
    {
        bool result = true;

        if (changeType == Enumerations.ResultChangeType.ToNegativeResult)
        {
            result = ValidateNegativeCompletionResult();
        }
        else if (changeType == Enumerations.ResultChangeType.ToPositiveResult)
        {
            result = ValidatePositiveCompletionResult();
        }

        return result;
    }


    private bool ValidateNegativeCompletionResult()
    {
        bool result = true;

        string pageSection = !Enum.IsDefined(typeof(Enumerations.ScreeningActivityType), ScreeningActivityTypeID) ? string.Empty : Enum.GetName(typeof(Enumerations.ScreeningActivityType), ScreeningActivityTypeID);
        int pageTypeID = ScreeningEntityType == Enumerations.ScreeningEntityType.Provider ? CON.RegistrationPageType.ProviderScreening
            : ScreeningEntityType == Enumerations.ScreeningEntityType.Owner ? CON.RegistrationPageType.OwnerScreening
            : ScreeningEntityType == Enumerations.ScreeningEntityType.Affiliation ? CON.RegistrationPageType.AffiliationScreening
            : ScreeningEntityType == Enumerations.ScreeningEntityType.HouseholdMember ? CON.RegistrationPageType.HouseholdMemberScreening : CON.RegistrationPageType.ProviderScreening;

        /// Verify user has clicked the Search link
        /// 
        if (ScreeningHelper.LinkClickRequiredResultChange(ScreeningActivityTypeID, Enumerations.ResultChangeType.ToNegativeResult)
            && !SearchPerformed && !string.IsNullOrWhiteSpace(ExternalUrlDescription))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = string.Format("You must {0} before indicating a match", ExternalUrlDescription);
            val.ValidationGroup = CustomValidationGroupName;
            this.Page.Validators.Add(val);

            result = false;
        }

        /// Verify user has uploaded a document to prove result change
        /// 
        if (ScreeningHelper.DocumentRequiredActivityStatus(ScreeningActivityTypeID, Enumerations.ResultChangeType.ToNegativeResult))
        {
            DataSet ds = svc.SelectRegDocuments(this.WorkflowPage.RegistrationId, pageTypeID, pageSection, string.Empty, ScreeningActivityID);
            if (!Helper.HasRows(ds))
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "You must upload a screenshot of your search results to confirm the Match Results";
                val.ValidationGroup = CustomValidationGroupName;
                this.Page.Validators.Add(val);

                result = false;
            }
        }

        /// Verify the user has entered an adverse action
        /// 
        if (ScreeningHelper.CommentsRequiredActivityStatus(ScreeningActivityTypeID, CON.ScreeningActivityStatusId.Match)
            && (string.IsNullOrWhiteSpace(this.txtAdverseAction.Text.Trim())))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "Please enter the reason for the Match or Failure.";
            val.ValidationGroup = CustomValidationGroupName;
            this.Page.Validators.Add(val);

            result = false;
        }

        return result;
    }


    private bool ValidatePositiveCompletionResult()
    {
        bool result = true;

        string pageSection = !Enum.IsDefined(typeof(Enumerations.ScreeningActivityType), ScreeningActivityTypeID) ? string.Empty : Enum.GetName(typeof(Enumerations.ScreeningActivityType), ScreeningActivityTypeID);
        int pageTypeID = ScreeningEntityType == Enumerations.ScreeningEntityType.Provider ? CON.RegistrationPageType.ProviderScreening
            : ScreeningEntityType == Enumerations.ScreeningEntityType.Owner ? CON.RegistrationPageType.OwnerScreening
            : ScreeningEntityType == Enumerations.ScreeningEntityType.Affiliation ? CON.RegistrationPageType.AffiliationScreening
            : ScreeningEntityType == Enumerations.ScreeningEntityType.HouseholdMember ? CON.RegistrationPageType.HouseholdMemberScreening : CON.RegistrationPageType.ProviderScreening;

        int activityStatusID = Convert.ToInt32(ddlMatchResults.SelectedValue);

        /// Verify user has clicked the Search link
        /// 
        //if (ScreeningHelper.LinkClickRequiredResultChange(ScreeningActivityTypeID, Enumerations.ResultChangeType.ToPositiveResult)
        //    && !SearchPerformed && !string.IsNullOrWhiteSpace(this.lnkExternalSearch.Text))
        //{
        //    CustomValidator val = new CustomValidator();
        //    val.IsValid = false;
        //    val.ErrorMessage = string.Format("You must {0} before overriding a match", ExternalUrlDescription);
        //    val.ValidationGroup = CustomValidationGroupName;
        //    this.Page.Validators.Add(val);

        //    result = false;
        //}

        /// Verify user has uploaded a document to prove result change
        /// 
        if (ScreeningHelper.DocumentRequiredActivityStatus(ScreeningActivityTypeID, Enumerations.ResultChangeType.ToPositiveResult))
        {
            DataSet ds = svc.SelectRegDocuments(this.WorkflowPage.RegistrationId, pageTypeID, pageSection, string.Empty, ScreeningActivityID);
            if (!Helper.HasRows(ds))
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "You must upload a screenshot of your search results to override the Match Results";
                val.ValidationGroup = CustomValidationGroupName;
                this.Page.Validators.Add(val);

                result = false;
            }
        }

        /// Verify the user has entered an adverse action
        /// 
        if (ScreeningHelper.CommentsRequiredActivityStatus(ScreeningActivityTypeID, activityStatusID)
            && (string.IsNullOrWhiteSpace(this.txtAdverseAction.Text.Trim())))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "Please enter the reason for the screening result.";
            val.ValidationGroup = CustomValidationGroupName;
            this.Page.Validators.Add(val);

            result = false;
        }


        /// PECOS specific requirements
        /// 
        if (ScreeningActivityTypeID == (int)Enumerations.ScreeningActivityType.PECOSVerfication)
        {
            if (pnlPecos.Visible && ddlMatchResults.SelectedValue == CON.ScreeningActivityStatusId.MedicareEnrolled.ToString())
            {
                //if (ddlPECOSRiskLevel.SelectedIndex < 1)
                //{
                //    CustomValidator val = new CustomValidator();
                //    val.IsValid = false;
                //    val.ErrorMessage = "You must select a PECOS Risk Level.";
                //    val.ValidationGroup = CustomValidationGroupName;
                //    this.Page.Validators.Add(val);

                //    result = false;
                //}
                //if (ddlPECOSState.SelectedIndex < 1)
                //{
                //    CustomValidator val = new CustomValidator();
                //    val.IsValid = false;
                //    val.ErrorMessage = "You must select a PECOS Enrolled State.";
                //    val.ValidationGroup = CustomValidationGroupName;
                //    this.Page.Validators.Add(val);

                //    result = false;
                //}

                if (this.ddlPECOSBackgroundComplete.SelectedValue == "")
                {
                    CustomValidator val = new CustomValidator();
                    val.IsValid = false;
                    val.ErrorMessage = "You must select a PECOS Background Complete Status.";
                    val.ValidationGroup = CustomValidationGroupName;
                    this.Page.Validators.Add(val);

                    result = false;
                }
                else if (this.ddlPECOSBackgroundComplete.SelectedValue == "1")
                {
                    if (this.txtPECOSBackgroundDate.Text.Trim() == "")
                    {
                        CustomValidator val = new CustomValidator();
                        val.IsValid = false;
                        val.ErrorMessage = "You must enter a PECOS Background Complete Date.";
                        val.ValidationGroup = CustomValidationGroupName;
                        this.Page.Validators.Add(val);

                        result = false;
                    }
                }
                else if (this.ddlPECOSBackgroundComplete.SelectedValue == "0")
                {
                    if (this.txtPECOSBackgroundDate.Text.Trim() != "")
                    {
                        CustomValidator val = new CustomValidator();
                        val.IsValid = false;
                        val.ErrorMessage = "You must not enter a PECOS Background Complete Date if the PECOS Background is not completed.";
                        val.ValidationGroup = CustomValidationGroupName;
                        this.Page.Validators.Add(val);

                        result = false;
                    }
                }
            }
        }

        if (ScreeningActivityTypeID == (int)Enumerations.ScreeningActivityType.SiteVisitVerification)
        {
            if (ddlSiteVisitVerificationStatus.SelectedValue == "")
            { 
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "You must select a Site Visit Verification Status.";
                val.ValidationGroup = CustomValidationGroupName;
                this.Page.Validators.Add(val);

                result = false;
            }
            else if ((Convert.ToInt32(ddlSiteVisitVerificationStatus.SelectedValue) != (int)CON.SiteVisitScreeningStatusID.SiteVisitRequired &&
				Convert.ToInt32(ddlSiteVisitVerificationStatus.SelectedValue) != (int)CON.SiteVisitScreeningStatusID.SiteVisitNotRequired && 
                Convert.ToInt32(ddlSiteVisitVerificationStatus.SelectedValue) != (int)CON.SiteVisitScreeningStatusID.DBHWillConduct &&
                Convert.ToInt32(ddlSiteVisitVerificationStatus.SelectedValue) != (int)CON.SiteVisitScreeningStatusID.DDSWillConduct) && 
                !Helper.IsValidDate(this.txtSiteVisitDate.Text, true))
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "You must enter a valid Site Visit Date if the Site Visit has been completed.";
                val.ValidationGroup = CustomValidationGroupName;
                this.Page.Validators.Add(val);

                result = false;
            }
        }
        if (ScreeningActivityTypeID == (int)Enumerations.ScreeningActivityType.CriminalBackgroundCheck)
        {
            if (ddlBackgroundVerificationStatus.SelectedValue == "")
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "You must select a Backgroud Verification Status.";
                val.ValidationGroup = CustomValidationGroupName;
                this.Page.Validators.Add(val);

                result = false;
            }
            if (
                !Helper.IsValidDate(this.txtBackgroundCheckDate.Text, true) && ddlBackgroundVerificationStatus.SelectedValue != CON.BackgroundVerficationStatusType.NoBackgroundCheckConducted.ToString())
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "You must enter a valid Background Verification Date if the Backgorund Verification has been completed.";
                val.ValidationGroup = CustomValidationGroupName;
                this.Page.Validators.Add(val);

                result = false;
            }
        }

        return result;
    }

    private void UpdateCustomScreeningData()
    {
        switch (ScreeningActivityTypeID)
        {
            case (int)Enumerations.ScreeningActivityType.PECOSVerfication:
                UpdatePECOSFields();
                break;
            case (int)Enumerations.ScreeningActivityType.SSDMFVerification:
                UpdateSSDMFFields();
                break;
            case (int)Enumerations.ScreeningActivityType.LicenseVerification:
                UpdateLicenseVerificationFields(); 
                break;
            case (int)Enumerations.ScreeningActivityType.DEAVerification:
                UpdateDEAFields();
                break;
            case (int)Enumerations.ScreeningActivityType.CriminalBackgroundCheck:
                UpdateBackgroundVerificationFields();
                break;
            case (int)Enumerations.ScreeningActivityType.SiteVisitVerification:
                UpdateSiteVisitFields();
                break;
            default:
                break;
        }
    }

    private void UpdateLicenseVerificationFields()
    {
        foreach (GridViewRow gvr in grdLicenses.Rows)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("LICENSE_RESTRICTION_CODE_ID", ((DropDownList)gvr.FindControl("ddlLicenseRestrictionCode")).SelectedValue);
            parms.Add("REG_LICENSURE_ID", grdLicenses.DataKeys[gvr.RowIndex]["REG_LICENSURE_ID"].ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString()); 
            svc.UpdateRegistrationDataWithParams("updateREG_LICENSECustom", parms);
        }
    }

    private void UpdatePECOSFields()
    {
        string matchResult = this.ddlMatchResults.SelectedValue;

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        //parms.Add("PECOS_RISK_LEVEL_ID", matchResult == CON.ScreeningActivityStatusId.MedicareEnrolled.ToString() ?  this.ddlPECOSRiskLevel.SelectedValue : "0");
        //parms.Add("PECOS_ENROLLED_STATE", matchResult == CON.ScreeningActivityStatusId.MedicareEnrolled.ToString() ?this.ddlPECOSState.SelectedValue : string.Empty);
        parms.Add("PECOS_BACKGROUND_COMPLETE", matchResult == CON.ScreeningActivityStatusId.MedicareEnrolled.ToString() ? this.ddlPECOSBackgroundComplete.SelectedValue : "0");
        parms.Add("PECOS_BACKGROUND_DATE", matchResult == CON.ScreeningActivityStatusId.MedicareEnrolled.ToString() ? this.txtPECOSBackgroundDate.Text : "");
        svc.UpdateRegistrationDataWithParams("updateREG_PROVIDERCustom", parms);

    }

    private void UpdateDEAFields()
    {
        string matchResult = this.ddlMatchResults.SelectedValue;

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("DEA_NUMBER", txtDEANumber.Text);
        //parms.Add("DEA_EFF_DATE", txtDEAEffectiveDate.Text);
        parms.Add("DEA_END_DATE", txtDEAExpireDate.Text);
        parms.Add("DEA_STATUS", txtDEAStatus.Text);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToShortDateString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        //OHPNM-15867-If the DEANumber and DEAExpireDate both have value then only insert or update
        if (!string.IsNullOrEmpty(txtDEANumber.Text) || !string.IsNullOrEmpty(txtDEAExpireDate.Text))
        {
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DEA");
            int regDEAID = 0;
            if (Helper.HasRows(ds.Tables[0]))
            {
                regDEAID = Helper.GetInt("REG_DEA_ID", ds.Tables[0].Rows[0]);
                parms.Add("REG_DEA_ID", regDEAID.ToString());
                svc.UpdateRegistrationDataTable("DEA", parms);
            }
            else
            {
                    parms.Add("MODIFIED_STATUS_TYPE_ID", "1");
                    svc.InsertRegistrationDataTable("DEA", parms);
            }
        }
            
    }

    private void UpdateBackgroundVerificationFields()
    {
        string matchResult = this.ddlMatchResults.SelectedValue;

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("BACKGROUND_VERIFICATION_STATUS_TYPE_ID", ddlBackgroundVerificationStatus.SelectedValue);
        parms.Add("BACKGROUND_VERIFICATION_DATE", txtBackgroundCheckDate.Text);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToShortDateString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        DataSet ds2 = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "BACKGROUND_VERIFICATION");
        int REGBACKGROUND_VERIFICATIONID = 0;
        if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
        {
            REGBACKGROUND_VERIFICATIONID = Convert.ToInt32(ds2.Tables[0].Rows[0]["REG_BACKGROUND_VERIFICATION_ID"]);
            parms.Add("REG_BACKGROUND_VERIFICATION_ID", REGBACKGROUND_VERIFICATIONID.ToString());
            svc.UpdateRegistrationDataTable("BACKGROUND_VERIFICATION", parms);
        }
        else
        {
            svc.InsertRegistrationDataTable("BACKGROUND_VERIFICATION", parms);
        }
    }

    private void UpdateSiteVisitFields()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("SITE_VISIT_SCREENING_TYPE_ID", null);
        parms.Add("SITE_VISIT_SCREENING_STATUS_ID", this.ddlSiteVisitVerificationStatus.SelectedValue);
        parms.Add("SITE_VISIT_SCREENING_DATE", this.txtSiteVisitDate.Text.Trim() == "" ? null : this.txtSiteVisitDate.Text);
        parms.Add("SCREENING_ACTIVITY_TYPE_ID", ((int)Enumerations.ScreeningActivityType.SiteVisitVerification).ToString());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToShortDateString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        svc.UpdateRegistrationDataWithParams("usp_SaveSITE_VISIT_SCREENING", parms);
    }

    private void UpdateSSDMFFields()
    {
        if (!Helper.IsValidDate(this.lblSSDMFDateOfDeath.Text, true))
            return;

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("DEATH_DATE", this.lblSSDMFDateOfDeath.Text);
        svc.UpdateRegistrationDataWithParams("updateREG_PROVIDERCustom", parms);

    }

    protected void grdLicenses_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
          
            DropDownList ddlLicenseRestrictrionCode = (DropDownList)e.Row.FindControl("ddlLicenseRestrictionCode");
            ddlLicenseRestrictrionCode.DataSource = svc.SelectLicenseRestrictionCodes();
            ddlLicenseRestrictrionCode.DataTextField = "LICENSE_DISPLAY";
            ddlLicenseRestrictrionCode.DataValueField = "LICENSE_RESTRICTION_CODE_ID";
            ddlLicenseRestrictrionCode.DataBind();

            if (grdLicenses.DataKeys[e.Row.DataItemIndex]["LICENSE_RESTRICTION_CODE_ID"] != null)
                ddlLicenseRestrictrionCode.SelectedValue = grdLicenses.DataKeys[e.Row.DataItemIndex]["LICENSE_RESTRICTION_CODE_ID"].ToString();
            
        }
    }
    protected void ddlBackgroundVerificationStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlMatchResults.Enabled = true;
        if (ddlBackgroundVerificationStatus.SelectedValue == CON.BackgroundVerficationStatusType.OutofStateBackgroundCheckPass.ToString() || ddlBackgroundVerificationStatus.SelectedValue == CON.BackgroundVerficationStatusType.Medicare.ToString()
            || ddlBackgroundVerificationStatus.SelectedValue == CON.BackgroundVerficationStatusType.NoBackgroundCheckConducted.ToString())
            ddlMatchResults.SelectedValue = "7";
        else if (ddlBackgroundVerificationStatus.SelectedValue == CON.BackgroundVerficationStatusType.OutofStateBackgroundCheckFail.ToString())
            ddlMatchResults.SelectedValue = "12";
        else
            ddlMatchResults.SelectedValue = "2";
        ddlMatchResults.Enabled = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        txtSiteVisitDate.Attributes.Add("readonly", "readonly");

    }


    public void BindExclusionPanel(DataTable dt)
    {
        if (dt == null || dt.Rows.Count == 0) return;


        ExclusionContainer.Visible = true;

        DataRow row = dt.Rows[0]; // Assuming single row for binding

        ExclusionPanel.ProviderClassification = GetValueIfExists(row, "PROVIDER_CLASSIFICATION");
        ExclusionPanel.OrganizationName = GetValueIfExists(row, "ORG_NAME");
        ExclusionPanel.FirstName = GetValueIfExists(row, "FIRST_NAME");
        ExclusionPanel.MiddleName = GetValueIfExists(row, "MIDDLE_NAME");
        ExclusionPanel.LastName = GetValueIfExists(row, "LAST_NAME");
        ExclusionPanel.Suffix = GetValueIfExists(row, "SUFFIX");
        ExclusionPanel.SSN = GetValueIfExists(row, "SSN");
        ExclusionPanel.EIN = GetValueIfExists(row, "EIN");
        ExclusionPanel.DOB = GetValueIfExists(row, "DOB");
        ExclusionPanel.NPI = GetValueIfExists(row, "NPI");
        ExclusionPanel.DOD = GetValueIfExists(row, "DOD");
        ExclusionPanel.ExclusionAgencyId = GetValueIfExists(row, "EXCLUSION_AGENCY_ID");
        ExclusionPanel.ExclusionProgram = GetValueIfExists(row, "EXCLUSION_PROGRAM");
        ExclusionPanel.ExclusionAgency = GetValueIfExists(row, "EXCLUSION_AGENCY");
        ExclusionPanel.ExclusionType = GetValueIfExists(row, "EXCLUSION_TYPE");
        ExclusionPanel.ExclusionDate = GetValueIfExists(row, "EXCLUSION_DATE");
        ExclusionPanel.ExclusionStatus = GetValueIfExists(row, "EXCLUSION_STATUS");
        ExclusionPanel.ExclusionTerminationDate = GetValueIfExists(row, "EXCLUSION_TERM_DATE");
        ExclusionPanel.ReinstatementDate = GetValueIfExists(row, "REINSTATEMENT_DATE");
        ExclusionPanel.AdditionalDetails = GetValueIfExists(row, "ADDITIONAL_DETAILS");

        ExclusionPanel.BindValues(); // Push values to UI
    }

    private string GetValueIfExists(DataRow row, string columnName)
    {
        return row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
            ? row[columnName].ToString()
            : string.Empty;
    }
}