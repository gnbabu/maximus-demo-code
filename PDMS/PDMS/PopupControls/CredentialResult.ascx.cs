using CustomControls;
using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Xml;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_CredentialResult : System.Web.UI.UserControl
{

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    public int RegAMAProfileDownloadsID = 0;

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
    private const string NotesLabel = "Credential Notes";
    

    #region Properties

    public int CredentialActivityID
    {
        get
        {
            if (ViewState["CredentialActivityID"] == null)
                ViewState["CredentialActivityID"] = -1;

            return (int)ViewState["CredentialActivityID"];
        }
        set
        {
            ViewState["CredentialActivityID"] = value;
        }
    }

    public Enumerations.CredentialActivityType CredentialActivityTypeID
    {
        get
        {
            if (ViewState["CredentialActivityTypeID"] == null)
                ViewState["CredentialActivityTypeID"] = -1;

            return (Enumerations.CredentialActivityType)ViewState["CredentialActivityTypeID"];
        }
        set
        {
            ViewState["CredentialActivityTypeID"] = value;
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
            this.ddlDataRank.Enabled = value;
            this.btnConfirm.Enabled = value;
            this.txtAdverseAction.Enabled = value;
        }
    }

    #endregion


    #region Events

    public delegate void CancelEventHandler(EventArgs args);
    public event CancelEventHandler Cancel;

    public delegate void CredentialActivityUpdateEventHandler(EventArgs args);
    public event CredentialActivityUpdateEventHandler CredentialActivityUpdated;

    public delegate void RefreshNavigationTreeEventHandler();
    public event RefreshNavigationTreeEventHandler RefreshNavigationTree;

    //public delegate void CreateAdverseActionEventHandler(PopupControls_AdverseAction.CreateAdverseActionEventArgs args);
    //public event CreateAdverseActionEventHandler CreateAdverseAction;


    #endregion

    public void LoadCredentialResult(int credentialActivityID, int credentialActivityTypeID, int dataRankId)
    {
        CredentialActivityID = credentialActivityID;
        CredentialActivityTypeID = (Enumerations.CredentialActivityType)credentialActivityTypeID;
       
        int providerTypeID = 0;
        bool isboardnotreq = true;
        //ucAdverseActionHeader.ScreeningActivityID = screeningActivityID;
        //UpdateControls();
        SetUpDataRankDropDownValues(dataRankId);

        //Get REG Credential date and see if it is greater than or equal to SAM825VerificationSourceDate then don't display inactive Verification codes, else display all
        bool includeInactive = true;
        DateTime Sam825VerificationDate = Convert.ToDateTime(AppSettings.Get("SAM825VerificationSourceDate", "08/12/2025"));
        DataSet dsCred = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CREDENTIALING");
        if (dsCred != null && dsCred.Tables.Count > 0 && dsCred.Tables[0].Rows.Count == 1)
        {
            DataTable dtCred = dsCred.Tables[0];
            DataRow drCred = dtCred.Rows[0];

            DateTime credDate = Convert.ToDateTime(drCred.GetValue<DateTime?>("START_DATE_TIME"));
            if (credDate.Date >= Sam825VerificationDate)
                includeInactive = false;
        }

        LoadVerificationSourceddl(credentialActivityTypeID, this.WorkflowPage.EntityTypeID == 1 ? 1 : 0, includeInactive == true ? 1 : 0);   //EntityType=1 is Individual
        //if (ddlLicenseStatus.Items.Count == 0)
        //    Helper.LoadList(ddlLicenseStatus, svc.SelectLicenseStatuses(), "LICENSE_STATUS_DESC", "LICENSE_STATUS_CODE", true);
        if (CredentialActivityID > 0)
        {
            SearchPerformed = false;
            string tableName = string.Empty;
            //this.lblTaxIDLabel.Text = "Tax ID";
            DataSet ds = svc.SelectProviderCredentialActivityMatchData(CredentialActivityID, this.WorkflowPage.RegistrationId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtMatch = ds.Tables[0];
                DataRow drMatch = dtMatch.Rows[0];
                ucSeperatorResult1.Header = string.Format("{0}", drMatch.GetString("SCREENING_ACTIVITY_TYPE_NAME"));
                lblVerifiedBy.Text = drMatch.GetString("VERIFIED_BY");
                DateTime? VerificationDate = drMatch.GetValue<DateTime?>("VERIFICATION_DATE");
                txtVerificationDate.Text = VerificationDate.HasValue ? VerificationDate.Value.ToString("MM/dd/yy") : string.Empty;
                int dataRankID = drMatch.GetValue<int>("ACTIVITY_DATARANK_ID");
                txtAdverseAction.Text = drMatch.GetString("NOTES");
                DateTime? orignalEffectiveDate = drMatch.GetValue<DateTime?>("ORIGINAL_EFFECTIVE_DATE");
                txtOrgEffDate.Text = orignalEffectiveDate.HasValue ? orignalEffectiveDate.Value.ToString("MM/dd/yy") : string.Empty;
                DateTime? renewalDate = drMatch.GetValue<DateTime?>("RENEWAL_DATE");
                txtRenewalDate.Text = renewalDate.HasValue ? renewalDate.Value.ToString("MM/dd/yy") : string.Empty;
                DateTime? expirationDate = drMatch.GetValue<DateTime?>("EXPIRATION_DATE");
                txtExpDate.Text = expirationDate.HasValue ? expirationDate.Value.ToString("MM/dd/yy") : string.Empty;
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                var dsStatus = psc.SelectUserNameByUserID(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                lnkExtraExternalSearch.Visible = true;
                DateTime? attestDate = drMatch.GetValue<DateTime?>("ATTESTATION_DATE");
                txtAttestationDate.Text = attestDate.HasValue ? attestDate.Value.ToString("MM/dd/yy") : string.Empty;
                divAttestationDate.Visible = false;
                divUploadNotRequired.Visible = false;
                divlblUpload.Visible = true;
                DateTime? maternityLicDate = drMatch.GetValue<DateTime?>("MATERNITY_LICENSE_DATE");
                txtMatLicDt.Text = maternityLicDate.HasValue ? maternityLicDate.Value.ToString("MM/dd/yy") : string.Empty;
                DateTime? siteAccredDate = drMatch.GetValue<DateTime?>("SITE_VISIT_DATE");
                txtSiteAccredDate.Text = siteAccredDate.HasValue ? siteAccredDate.Value.ToString("MM/dd/yy") : string.Empty;

                if (credentialActivityTypeID == (int)Enumerations.CredentialActivityType.BoardVerification)
                {
                     isboardnotreq = drMatch.GetValue<bool>("isBoardVerificationRequired");
                }
                chkBoardVerificationRequird.Checked = isboardnotreq ? false : true;
                if (isboardnotreq==false)
                {
                    chkBoardVerificationRequird.Checked = true;
                }
                //lnkExtraExternalSearch.Text="AMA Provider File";
                DataRow row = dsStatus.Tables[0].Rows[0];
                if (lblVerifiedBy.Text == "")
                {
                    lblVerifiedBy.Text = string.Concat(row.Field<string>("CONTACT_NAME"));
                }
                if(drMatch.GetValue<int>("VERIFICATION_SOURCE_USED").ToString() != "0")
                ddlVerificationSourceUsed.SelectedValue = drMatch.GetValue<int>("VERIFICATION_SOURCE_USED").ToString();
                //lblScreeningDate.Text = screeningDate.HasValue ? screeningDate.Value.ToString("MM/dd/yy") : string.Empty;
                DataSet dsUrl = svc.SelectCredentialActivityUrl(credentialActivityTypeID, this.WorkflowPage.EntityTypeID == 1 ? 1 : 0);   //EntityType=1 is Individual
                lnkReference.Controls.Clear();
                if (dsUrl != null && dsUrl.Tables.Count > 0 && dsUrl.Tables[0].Rows.Count > 0)
                {
                    if(lnkReference.Controls.Count > 0)
                    {
                        lnkReference.Controls.Clear();
                    }
                    int count = 1;
                    foreach (DataRow dr in dsUrl.Tables[0].Rows)
                    {
                        count++;
                        HyperLink dynLink = new HyperLink();
                        dynLink.ID = "DynLnk" + count;
                        dynLink.Text = dr.GetString("EXTERNAL_CHECK_URL_DESCRIPTION");
                        dynLink.NavigateUrl = dr.GetString("EXTERNAL_CHECK_URL");
                        dynLink.Target = "_blank";
                        dynLink.CssClass = "formLabel";
                        if(!lnkReference.Controls.Contains(dynLink))
                        lnkReference.Controls.Add(dynLink);
                    }
                }

               

                DataTable dtRegData = null;
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    dtRegData = ds.Tables[1];
                }
                if (dtRegData != null)
                {
                    DataTable dtProvider = ds.Tables[1];
                    DataRow drProvider = dtProvider.Rows[0];
                    hdnNPI.Value = drProvider.GetString("NPI");
                    txtProviderName.Text = drProvider.GetString("INDIVIDUAL_NAME");
                    txtGender.Text = drProvider.GetString("GENDER")=="F"? "Female": drProvider.GetString("GENDER") == "M" ? "Male" : null;
                    DateTime? DOB = drProvider.GetValue<DateTime?>("BIRTH_DATE");
                    txtProviderDOB.Text= DOB.HasValue? DOB.Value.ToString("MM/dd/yy") : string.Empty;
                    txtProviderType.Text = drProvider.GetString("PROVIDER_TYPE_NAME");
                    txtssn.Text = drProvider.GetString("TAX_ID");
                    txtNPI.Text = drProvider.GetString("NPI");
                    //lblNPI.Text = drProvider.GetString("NPI");
                    //lblOrganizationName.Text = drProvider.GetString("ORGANIZATION_NAME");
                    //lblIndividualName.Text = drProvider.GetString("INDIVIDUAL_NAME");
                   
                    providerTypeID = Helper.GetInt("PROVIDER_TYPE_ID", drProvider);
                }
                
                //DataTable dtPopWith = screeningActivityTypeID == (int)Enumerations.ScreeningActivityType.PECOSVerfication 
                //        || screeningActivityTypeID == (int)Enumerations.ScreeningActivityType.SAVEVerification ? dtRegData : dtMatch;

                //if (screeningActivityTypeID == (int)Enumerations.ScreeningActivityType.SiteVisitVerification)
                //SetUpSiteVisitStatusDropDownValues(providerTypeID);
                //if (screeningActivityTypeID == (int)Enumerations.ScreeningActivityType.CriminalBackgroundCheck)
                //    SetUpBackgroundCheckDropDownValues(providerTypeID);
                PopulateCredentialSpecificData(dtRegData, credentialActivityTypeID);
                dvBoardVerification.Visible = false;
                UpdateControls();

            }
        }
    }




    private void SetUpDataRankDropDownValues(int datarank)
    {
        ddlDataRank.Items.Clear();

        DataSet ds = svc.SelectDataRankTypes();
        if (Helper.HasRows(ds))
        {
            Helper.LoadDropDown(this.ddlDataRank, ds.Tables[0], "DISPLAY_NAME", "DATARANK_TYPE_ID", false);

            if (ddlDataRank.Items.FindByValue(datarank.ToString()) != null)
            {
                ddlDataRank.SelectedValue = datarank.ToString();
            }
        }   
    }

    private void LoadVerificationSourceddl(int activityTypeId, int IsIndividual, int includeInactive)
    {
        ddlVerificationSourceUsed.Items.Clear();

        DataSet ds = svc.SelectVerificationSource(activityTypeId, IsIndividual, includeInactive);
        if (Helper.HasRows(ds))
        {
            Helper.LoadDropDown(this.ddlVerificationSourceUsed, ds.Tables[0], "VERIFICATION_SOURCE_DISPLAYNAME", "VERIFICATION_SOURCE_ID", false);
        }
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

    private void PopulateCredentialSpecificData(DataTable dtMatch, int credentialActivityTypeID)
    {
        switch (credentialActivityTypeID)
        {
           case (int)Enumerations.CredentialActivityType.NPPESVerification://good
                mvScreeningSpecificResults.SetActiveView(vwNPPESResults);
                this.PopulateNPPESMatchResults(dtMatch);
                break;
            case (int)Enumerations.CredentialActivityType.MalPracticeInsurance:
                mvScreeningSpecificResults.SetActiveView(vwMalpracticeResults);
                this.PopulateMalpracticeinsuranceResults(dtMatch);
                break;
            case (int)Enumerations.CredentialActivityType.FiveYearWorkHistory:
                mvScreeningSpecificResults.SetActiveView(vwWorkHistoryResults);
               this.PopulateWorkHistoryDetails(dtMatch);
                break;
            case (int)Enumerations.CredentialActivityType.DEAVerification://good
                mvScreeningSpecificResults.SetActiveView(vwDEAResults);
                this.PopulateDEARegData();
                break;
            case (int)Enumerations.CredentialActivityType.LicenseVerification://good
                mvScreeningSpecificResults.SetActiveView(vwLicenseResults);
                this.PopulateLicenseRegData();
                break;
            case (int)Enumerations.CredentialActivityType.BoardVerification:
                mvScreeningSpecificResults.SetActiveView(vwAMBSResults);
                this.PopulateABMSMatchResults(dtMatch);
                break;
            case (int)Enumerations.CredentialActivityType.Education:
                mvScreeningSpecificResults.SetActiveView(vwEducationResults);
                this.PopulateEducationResults(dtMatch);
                break;
            case (int)Enumerations.CredentialActivityType.ControlledSubstanceVerification://good
                mvScreeningSpecificResults.SetActiveView(vwCDSResults);
                this.PopulateCDSResults();
                break;
            case (int)Enumerations.CredentialActivityType.NPDBVerification:
                mvScreeningSpecificResults.SetActiveView(vwNPDBResults);
                this.PopulateNPDBResults();
                break;
            default:
                mvScreeningSpecificResults.ActiveViewIndex = -1;
                break;
        }
    }

    private void PopulateABMSMatchResults(DataTable dtMatch)
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            //parms.Add("PrimaryFlag", "0");
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_BOARD_CERTIFICATION", parms);
            if (Helper.HasRows(ds)) grdSpecialties.DataSource = ds.Tables[0];
            else grdSpecialties.DataSource  = null;
            grdSpecialties.DataBind();

        }
    }

    private void PopulateWorkHistoryDetails(DataTable dtMatch)
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_WORKHISTORY", parms);
            if (Helper.HasRows(ds)) grdWorkResult.DataSource =  ds.Tables[0];
            else grdWorkResult.DataSource = null;
            grdWorkResult.DataBind();

            //Check if the user has work gaps
            parms.Clear();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet dsGaps = psc.SelectRegistrationDataWithParams("usp_SelectREG_WORKGAPS", parms);
            //if (Helper.HasRows(dsGaps))
            //{
            //    rblGap.SelectedValue = "1";
            //}
            //else
            //{
            //    rblGap.SelectedValue = "0";
            //}


        }
    }

    private void PopulateNPDBResults()
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
            DataTable dtProvider = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
            if (Helper.HasRows(dtProvider))
            {
                DataRow row = dtProvider.Rows[0];
                this.lblName.Text = Helper.GetString("NAME", row);
                this.lblGender.Text = Helper.GetString("GENDER", row);
                this.lbl_DOB.Text = Helper.GetDate("BIRTH_DATE", row);
                this.lbl_Org.Text = Helper.GetString("NAME", row);
                this.lbl_SSN.Text = Helper.FormatSSN(Helper.GetString("TAX_ID", row),true);
                this.lbl_Address.Text = Helper.GetString("CONTACT_ADDRESS1", row) + " , " + Helper.GetString("CONTACT_CITY", row) + " , " + Helper.GetString("CONTACT_STATE", row) + " , " + Helper.GetString("CONTACT_ZIP", row);
            }

        };

    }

    private void PopulateDEARegData()
    {
        int regID = this.WorkflowPage.RegistrationId;
        
        DataSet ds = svc.SelectRegistrationData(regID, "DEA");
        DataTable dt = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        this.grdDEANumbers.DataSource = dt;
        this.grdDEANumbers.DataBind();

        if (Helper.HasRows(ds))
        {
            DataRow row = dt.Rows[0];
            txtDEANumber.Text = Helper.GetString("DEA_NUMBER", row);
            txtDEAEffectiveDate.Text = Helper.GetDate("DEA_EFF_DATE", row);
            txtDEAExpireDate.Text = Helper.GetDate("DEA_END_DATE", row);
            int regDeaId = Helper.GetInt("REG_DEA_ID", row);
            LoadDEASectionControl(regDeaId, true, false, "DEA");
        }
    }




    private void PopulateMalpracticeinsuranceResults(DataTable dtMatch)
    {
        DataRow dr = dtMatch.Rows[0];

        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "INSURANCE");
            if (Helper.HasRows(ds)) grdInsurance.DataSource = ds.Tables[0];
            else grdInsurance.DataSource = null;
            grdInsurance.DataBind();
            //DataTable dtInsurance = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
            //if (Helper.HasRows(dtInsurance))
            //{
            //    DataRow row = dtInsurance.Rows[0];
            //    int InsuranceId=Helper.GetInt("REG_INSURANCE_ID", row);
            //    this.lblMalPracticeNumber.Text = Helper.GetString("POLICY_NUMBER", row);
            //    this.lblMalPracticeEffective.Text = Helper.GetDate("EFFECTIVE_DATE", row);
            //    this.lblMalPracticeExpiration.Text = Helper.GetDate("EXPIRATION_DATE", row);
            //    this.lblMalPracticeCarrier.Text = Helper.GetString("CarrierName", row);
            //    this.lblMalPracticeCoverage.Text = Helper.GetString("CoverageAmountPerOccurance", row);
            //    this.lblMalPracticeAggregate.Text = Helper.GetString("CoverageAmountPerAggregate", row);
            //     //upload section Control data
            //    LoadSectionControl(InsuranceId, true, false, "Insurance");
            //}



        }
    }

  
    private void PopulateEducationResults(DataTable dtMatch)
    {
        DataRow dr = dtMatch.Rows[0];
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_EDUCATION", parms);
            if (Helper.HasRows(ds)) grdEducationResult.DataSource =  ds.Tables[0];
            else grdEducationResult.DataSource = null;
            grdEducationResult.DataBind();
         
        }
       
    }
   

    //good
    private void PopulateNPPESMatchResults(DataTable dtMatch)
    {
        DataRow dr = dtMatch.Rows[0];

        this.lblNPPESNPI.Text = dr.GetString("NPI");
        this.lblNPPESEntityType.Text =Helper.GetInt("ENTITYTYPE", dr).ToString();
        this.lblNPPESOrgName.Text = dr.GetString("ORGANIZATION_NAME");
        this.lblNPPESFirstName.Text = dr.GetString("FIRSTNAME");
        this.lblNPPESMiddleName.Text = dr.GetString("MIDDLENAME");
        this.lblNPPESLastName.Text = dr.GetString("LASTNAME");
        this.lblNPPESAddressState.Text = dr.GetString("MAILINGADDRESSSTATENAME");
    }
    //good
    private void PopulateLicenseRegData()
    {
        int regID = this.WorkflowPage.RegistrationId;
       
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
        

        DataRow dr = dtRegData.Rows[0];
        //Helper.LoadDropDownListWithStates(ref this.ddlPECOSState);
        
        DataSet dsRisk = svc.SelectProviderRiskLevels();
        DataTable dtRisk = FilterPecosRiskLevel(dsRisk);
        //Helper.LoadDropDown(this.ddlPECOSRiskLevel, dtRisk, "PROVIDER_RISK_LEVEL_NAME", "PROVIDER_RISK_LEVEL_ID", false);
        //this.ddlPECOSRiskLevel.Items.Insert(0, new ListItem(string.Empty, "0"));

        //ddlPECOSState.SelectedValue = Helper.GetString("PECOS_ENROLLED_STATE", dr).ToString();
        //ddlPECOSRiskLevel.SelectedValue = Helper.GetInt("PECOS_RISK_LEVEL_ID", dr).ToString();
    
    }

    

    private void PopulateCDSResults()
    {
        DataSet dsCDSScreening = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "STATE_CDS_NUMBER");
        
        if (Helper.HasRows(dsCDSScreening))
        {
            grdCDSNumbers.DataSource = dsCDSScreening;
            grdCDSNumbers.DataBind();

            int cdsID = 0;
            cdsID=Helper.GetInt("REG_STATE_CDS_NUMBER_ID", dsCDSScreening.Tables[0].Rows[0]);
            LoadCDSSectionControl(cdsID, true, false, "StateCDSNumber");
            
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

        //string amaXmlResponse = GetProfileByNPI("161722550");
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parmsenroll = new Dictionary<string, string>();
        parmsenroll.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parmsenroll.Add("DOWNLOAD_DATE_TIME", DateTime.Now.ToString());
        parmsenroll.Add("USER_downloaded", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parmsenroll.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parmsenroll.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parmsenroll.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
        parmsenroll.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        RegAMAProfileDownloadsID = psc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "AMA_PROFILE_DOWNLOADS", parmsenroll);

        string amaXmlResponse = "";
        XMLResponseStatus xrs = new XMLResponseStatus();
        xrs = GetProfileByNPI(hdnNPI.Value);
        amaXmlResponse = xrs.xmlResponse;
        if (!string.IsNullOrEmpty(amaXmlResponse))
        {
            string entityID = GetEntityID(xrs);
            string resp = GetProfileFull(entityID);
            string response = GetPDFRequestPhycisianByID(entityID);
            string filePath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + "AMAProviderFile" + entityID + ".pdf";
            var fileInfo = new System.IO.FileInfo(filePath);
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename=\"{0}\"", lnkExtraExternalSearch.Text + entityID + ".pdf"));
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.WriteFile(filePath);
            Response.End();
        }
        else
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = string.Format("No Profile found for  {0} ", hdnNPI.Value);
            val.ValidationGroup = CustomValidationGroupName;
            this.Page.Validators.Add(val);
            return;
        }
    }
    private string GetProfileFull(string Id)
    {
        string xmlResponse = string.Empty;
        try
        {
            // string response = GetRequestPycisianByID(Id);
            //if (!string.IsNullOrEmpty(response))
            //{
            string filePath = "~/Documents/AMAProviderFile" + Id + ".pdf";
            WebRequest wrPhysicianFullPDF = InitializeRequest("/profiles/profile/full/" + Id, "GET");


            using (WebResponse rsPAFull = wrPhysicianFullPDF.GetResponse())
            {
                HttpWebResponse rsPAfullStatusCode = (HttpWebResponse)rsPAFull;

                HttpStatusCode statuscode = rsPAfullStatusCode.StatusCode;

                using (var stream = rsPAFull.GetResponseStream())
                {

                    using (StreamReader sr = new StreamReader(stream))
                    {
                        xmlResponse = sr.ReadToEnd();
                    }

                }

            }
            //}
        }
        catch (Exception ex)
        {

        }
        return xmlResponse;
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

            bool isBoardVerificationRequired = true;
            int dataRank = int.Parse(ddlDataRank.SelectedValue);
            DateTime? orgEffdt =null, renewDate =null, expdate=null, verificationDate = null, attestationDate=null, matLicDate = null, siteAccredDate = null;
            if (txtOrgEffDate.Text!="")
            {
              orgEffdt = DateTime.Parse(txtOrgEffDate.Text);
            }
            if(txtRenewalDate.Text!="")
            {
                renewDate = DateTime.Parse(txtRenewalDate.Text);
            }
            if(txtExpDate.Text!="")
            {
                expdate = DateTime.Parse(txtExpDate.Text);
            }            
            if (!string.IsNullOrEmpty(txtVerificationDate.Text))
            {
                verificationDate = DateTime.Parse(txtVerificationDate.Text);
            }            
            int vsValue = 0;
            if(!string.IsNullOrEmpty(ddlVerificationSourceUsed.SelectedValue))
            {
                vsValue = Convert.ToInt32(ddlVerificationSourceUsed.SelectedValue);
            }
            if(txtAttestationDate.Text!="")
            {
                attestationDate = DateTime.Parse(txtAttestationDate.Text);
            }
            if(chkBoardVerificationRequird.Checked)
            {
                isBoardVerificationRequired = false;
            }
            if (txtMatLicDt.Text != "")
            {
                matLicDate = DateTime.Parse(txtMatLicDt.Text);
            }
            if (txtSiteAccredDate.Text != "")
            {
                siteAccredDate = DateTime.Parse(txtSiteAccredDate.Text);
            }

            svc.UpdateCredentialActivity(CredentialActivityID, this.WorkflowPage.RegistrationId, (int)CredentialActivityTypeID, dataRank, txtAdverseAction.Text.Trim(), orgEffdt,
                renewDate, expdate, vsValue, verificationDate, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), attestationDate, isBoardVerificationRequired, matLicDate, siteAccredDate, chkMedicareOptOutRequird.Checked);
                
            UpdateCustomScreeningData();

            if (CredentialActivityUpdated != null)
            {
                CredentialActivityUpdated(new EventArgs());
            }
            //if (RefreshNavigationTree != null)
            //{
            //    RefreshNavigationTree();
            //}
            //else
            //{
            //    Refresh(true);
            //}
        }
    }

    private void SetResultChangeType()
    {
        if (!string.IsNullOrEmpty(ddlDataRank.SelectedValue))
        {
            int newStatusID = int.Parse(ddlDataRank.SelectedValue);

                ResultChange = Enumerations.ResultChangeType.ToPositiveResult;
           
        }       
        else
        {
            ResultChange = Enumerations.ResultChangeType.None;
        }
    }

    private bool ValidateResultChange(Enumerations.ResultChangeType changeType)
    {
        bool result = true;
        if (string.IsNullOrEmpty(txtVerificationDate.Text))
        {            
            result = false;            
        }
        else if (changeType == Enumerations.ResultChangeType.ToNegativeResult)
        {
            result = ValidateNegativeCompletionResult();
        }
        else if (changeType == Enumerations.ResultChangeType.ToPositiveResult)
        {
            result = ValidatePositiveCompletionResult();
        }

        // OHPNM-2364 - if we are all validated 'good' to this point, let's validate that the verificationDate is not in the future
        if (result && !string.IsNullOrEmpty(txtVerificationDate.Text))
        {
            if (Convert.ToDateTime(txtVerificationDate.Text) > DateTime.Today)
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "The verification date cannot be in the future.";
                val.ValidationGroup = CustomValidationGroupName;
                this.Page.Validators.Add(val);
                result = false;
            }
        }


        return result;
    }


    private bool ValidateNegativeCompletionResult()
    {
        bool result = true;

        //string pageSection = !Enum.IsDefined(typeof(Enumerations.ScreeningActivityType), ScreeningActivityTypeID) ? string.Empty : Enum.GetName(typeof(Enumerations.ScreeningActivityType), ScreeningActivityTypeID);
        //int pageTypeID = ScreeningEntityType == Enumerations.ScreeningEntityType.Provider ? CON.RegistrationPageType.ProviderScreening
        //    : ScreeningEntityType == Enumerations.ScreeningEntityType.Owner ? CON.RegistrationPageType.OwnerScreening
        //    : ScreeningEntityType == Enumerations.ScreeningEntityType.Affiliation ? CON.RegistrationPageType.AffiliationScreening
        //    : ScreeningEntityType == Enumerations.ScreeningEntityType.HouseholdMember ? CON.RegistrationPageType.HouseholdMemberScreening : CON.RegistrationPageType.ProviderScreening;

        ///// Verify user has clicked the Search link
        ///// 
        //if (ScreeningHelper.LinkClickRequiredResultChange(ScreeningActivityTypeID, Enumerations.ResultChangeType.ToNegativeResult)
        //    && !SearchPerformed && !string.IsNullOrWhiteSpace(ExternalUrlDescription))
        //{
        //    CustomValidator val = new CustomValidator();
        //    val.IsValid = false;
        //    val.ErrorMessage = string.Format("You must {0} before indicating a match", ExternalUrlDescription);
        //    val.ValidationGroup = CustomValidationGroupName;
        //    this.Page.Validators.Add(val);

        //    result = false;
        //}

        ///// Verify user has uploaded a document to prove result change
        ///// 
        //if (ScreeningHelper.DocumentRequiredActivityStatus(CredentialActivityID, Enumerations.ResultChangeType.ToNegativeResult))
        //{
        //    DataSet ds = svc.SelectRegDocuments(this.WorkflowPage.RegistrationId, 0, string.Empty, string.Empty);
        //    if (!Helper.HasRows(ds))
        //    {
        //        CustomValidator val = new CustomValidator();
        //        val.IsValid = false;
        //        val.ErrorMessage = "You must upload a screenshot of your search results to confirm the Match Results";
        //        val.ValidationGroup = CustomValidationGroupName;
        //        this.Page.Validators.Add(val);

        //        result = false;
        //    }
        //}

        /// Verify the user has entered an adverse action
        /// 
        //if (ScreeningHelper.CommentsRequiredActivityStatus(ScreeningActivityTypeID, CON.ScreeningActivityStatusId.Match)
        //    && (string.IsNullOrWhiteSpace(this.txtAdverseAction.Text.Trim())))
        //{
        //    CustomValidator val = new CustomValidator();
        //    val.IsValid = false;
        //    val.ErrorMessage = "Please enter the reason for the Match or Failure.";
        //    val.ValidationGroup = CustomValidationGroupName;
        //    this.Page.Validators.Add(val);

        //    result = false;
        //}

        return result;
    }


    private bool ValidatePositiveCompletionResult()
    {
        bool result = true;

        //string pageSection = !Enum.IsDefined(typeof(Enumerations.ScreeningActivityType), CredentialActivityTypeID) ? string.Empty : Enum.GetName(typeof(Enumerations.ScreeningActivityType), CredentialActivityTypeID);
        //int pageTypeID = ScreeningEntityType == Enumerations.ScreeningEntityType.Provider ? CON.RegistrationPageType.ProviderScreening
           // : ScreeningEntityType == Enumerations.ScreeningEntityType.Owner ? CON.RegistrationPageType.OwnerScreening
           // : ScreeningEntityType == Enumerations.ScreeningEntityType.Affiliation ? CON.RegistrationPageType.AffiliationScreening
            //: ScreeningEntityType == Enumerations.ScreeningEntityType.HouseholdMember ? CON.RegistrationPageType.HouseholdMemberScreening : CON.RegistrationPageType.ProviderScreening;

        //int activityStatusID = Convert.ToInt32(ddlDataRank.SelectedValue);

        ///// Verify user has clicked the Search link
        ///// 
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

        ///// Verify user has uploaded a document to prove result change
        ///// 
        //if (ScreeningHelper.DocumentRequiredActivityStatus(ScreeningActivityTypeID, Enumerations.ResultChangeType.ToPositiveResult))
        //{
        //    DataSet ds = svc.SelectRegDocuments(this.WorkflowPage.RegistrationId, pageTypeID, pageSection, string.Empty, ScreeningActivityID);
        //    if (!Helper.HasRows(ds))
        //    {
        //        CustomValidator val = new CustomValidator();
        //        val.IsValid = false;
        //        val.ErrorMessage = "You must upload a screenshot of your search results to override the Match Results";
        //        val.ValidationGroup = CustomValidationGroupName;
        //        this.Page.Validators.Add(val);

        //        result = false;
        //    }
        //}

        ///// Verify the user has entered an adverse action
        ///// 
        //if (ScreeningHelper.CommentsRequiredActivityStatus(ScreeningActivityTypeID, activityStatusID)
        //    && (string.IsNullOrWhiteSpace(this.txtAdverseAction.Text.Trim())))
        //{
        //    CustomValidator val = new CustomValidator();
        //    val.IsValid = false;
        //    val.ErrorMessage = "Please enter the reason for the screening result.";
        //    val.ValidationGroup = CustomValidationGroupName;
        //    this.Page.Validators.Add(val);

        //    result = false;
        //}


        ///// PECOS specific requirements
        ///// 
        //if (ScreeningActivityTypeID == (int)Enumerations.ScreeningActivityType.PECOSVerfication)
        //{
        //    if (pnlPecos.Visible && ddlDataRank.SelectedValue == CON.ScreeningActivityStatusId.MedicareEnrolled.ToString())
        //    {
        //        //if (ddlPECOSRiskLevel.SelectedIndex < 1)
        //        //{
        //        //    CustomValidator val = new CustomValidator();
        //        //    val.IsValid = false;
        //        //    val.ErrorMessage = "You must select a PECOS Risk Level.";
        //        //    val.ValidationGroup = CustomValidationGroupName;
        //        //    this.Page.Validators.Add(val);

        //        //    result = false;
        //        //}
        //        //if (ddlPECOSState.SelectedIndex < 1)
        //        //{
        //        //    CustomValidator val = new CustomValidator();
        //        //    val.IsValid = false;
        //        //    val.ErrorMessage = "You must select a PECOS Enrolled State.";
        //        //    val.ValidationGroup = CustomValidationGroupName;
        //        //    this.Page.Validators.Add(val);

        //        //    result = false;
        //        //}

                
                
        //    }
        //}

        
       

        return result;
    }

    private void UpdateCustomScreeningData()
    {
        switch (CredentialActivityTypeID)
        {
            case Enumerations.CredentialActivityType.LicenseVerification:
                UpdateLicenseVerificationFields(); 
                break;
            case Enumerations.CredentialActivityType.DEAVerification:
                UpdateDEAFields();
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
            parms.Add("LICENSE_RESTRICTION_CODE_ID", null);
            parms.Add("REG_LICENSURE_ID", grdLicenses.DataKeys[gvr.RowIndex]["REG_LICENSURE_ID"].ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString()); 
            svc.UpdateRegistrationDataWithParams("updateREG_LICENSECustom", parms);
        }
    }

   

    private void UpdateDEAFields()
    {
        string matchResult = this.ddlDataRank.SelectedValue;

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("DEA_NUMBER", txtDEANumber.Text);
        parms.Add("DEA_EFF_DATE", txtDEAEffectiveDate.Text);
        parms.Add("DEA_END_DATE", txtDEAExpireDate.Text);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToShortDateString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DEA");
        int regDEAID = 0;
        if (Helper.HasRows(ds.Tables[0]))
        {
            regDEAID = Helper.GetInt("REG_DEA_ID", ds.Tables[0].Rows[0]);
            parms.Add("REG_DEA_ID", regDEAID.ToString());
            svc.UpdateRegistrationDataTable("DEA", parms);
        }
      
    }

    protected string GetContactDetails(object name, object email, object phone)
    {
        string contact = string.Empty;
        string contactName = string.IsNullOrEmpty(name.ToString()) ? "" : name.ToString();
        string contactEmail = string.IsNullOrEmpty(email.ToString()) ? "" : email.ToString();
        string contactPhone = string.IsNullOrEmpty(phone.ToString()) ? "" : phone.ToString();
        contact = Helper.GetFormattedContact(contactName, contactEmail, contactPhone);

        return contact;
    }
   
    

    protected void grdLicenses_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
          
            //DropDownList ddlLicenseRestrictrionCode = (DropDownList)e.Row.FindControl("ddlLicenseRestrictionCode");
            //ddlLicenseRestrictrionCode.DataSource = svc.SelectLicenseRestrictionCodes();
            //ddlLicenseRestrictrionCode.DataTextField = "LICENSE_DISPLAY";
            //ddlLicenseRestrictrionCode.DataValueField = "LICENSE_RESTRICTION_CODE_ID";
            //ddlLicenseRestrictrionCode.DataBind();

            //if (grdLicenses.DataKeys[e.Row.DataItemIndex]["LICENSE_RESTRICTION_CODE_ID"] != null)
            //    ddlLicenseRestrictrionCode.SelectedValue = grdLicenses.DataKeys[e.Row.DataItemIndex]["LICENSE_RESTRICTION_CODE_ID"].ToString();
            
        }
    }
    public void Refresh(bool forceRedirect = false)
    {
        if (forceRedirect)
        {
            string url = "~/Process/Registration.aspx?Step=" + this.WorkflowPage.RegistrationStep.ToString() + "&RegId=" + this.WorkflowPage.RegistrationId.ToString();
            Response.Redirect(url);
        }
    }
    //Here lot of re-factoring of code need to be done when time permits.
    //To avoid repetition of code blocks and need to create reusable code base.
    public void LoadSectionControl(int recordId = 0, bool isEdit = false, bool loadViewState = false, string pageSection="")
    {
        Upload upload = new Upload();
        PlaceholderUploadMalPracticeInsurance.Controls.Clear();
        
        DataSet ds = null;
        int pageTypeID = CON.RegistrationPageType.LicensesClassifications;
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, recordId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, recordId, pageSection, isEdit);

        int table = ds.Tables.Count;
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                UserControls_UploadSectionControl ucUploadSectionControl =
                    LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;
                
                ucUploadSectionControl.Title = Helper.GetString("TITLE", dr);

                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);
                if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                    ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);
                else
                    ucUploadSectionControl.DocumentId = 0;

                ucUploadSectionControl.DestinationPath = @"C:\project\temp";

                ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);

                if ((dr.Table.Columns.Contains("FILE_NAME")))
                    ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                else
                    ucUploadSectionControl.FileName = null;

                ucUploadSectionControl.SectionName = pageSection;
                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                PlaceholderUploadMalPracticeInsurance.Controls.Add(ucUploadSectionControl);
               
            }
        }

    }

    public void LoadDEASectionControl(int recordId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = "")
    {
        Upload upload = new Upload();
        placeHolderDEA.Controls.Clear();

        DataSet ds = null;
        int pageTypeID = CON.RegistrationPageType.LicensesClassifications;
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, recordId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, recordId, pageSection, isEdit);

        int table = ds.Tables.Count;
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                UserControls_UploadSectionControl ucUploadSectionControl =
                    LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;

                ucUploadSectionControl.Title = Helper.GetString("TITLE", dr);

                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);
                if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                    ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);
                else
                    ucUploadSectionControl.DocumentId = 0;

                ucUploadSectionControl.DestinationPath = @"C:\project\temp";

                //ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);
                ucUploadSectionControl.IsRequired = true;

                if ((dr.Table.Columns.Contains("FILE_NAME")))
                    ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                else
                    ucUploadSectionControl.FileName = null;

                ucUploadSectionControl.SectionName = pageSection;
                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                placeHolderDEA.Controls.Add(ucUploadSectionControl);

            }
        }

    }

    public void LoadCDSSectionControl(int recordId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = "")
    {
        Upload upload = new Upload();
        placeHolderCDS.Controls.Clear();

        DataSet ds = null;
        int pageTypeID = CON.RegistrationPageType.LicensesClassifications;
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, recordId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, recordId, pageSection, isEdit);

        int table = ds.Tables.Count;
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                UserControls_UploadSectionControl ucUploadSectionControl =
                    LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;

                ucUploadSectionControl.Title = Helper.GetString("TITLE", dr);

                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);
                if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                    ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);
                else
                    ucUploadSectionControl.DocumentId = 0;

                ucUploadSectionControl.DestinationPath = @"C:\project\temp";

                ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);

                if ((dr.Table.Columns.Contains("FILE_NAME")))
                    ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                else
                    ucUploadSectionControl.FileName = null;

                ucUploadSectionControl.SectionName = pageSection;
                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                placeHolderCDS.Controls.Add(ucUploadSectionControl);

            }
        }

    }

    #region Private

    private void UpdateControls()
    {
        dvMedicareOptOut.Visible = false;
        divIssuingState.Visible = false;
        divCDSNumber.Visible = false;
        divBoardStatus.Visible = false;
        divMaternityLicDate.Visible = false;
        divSiteAccredDate.Visible = false;
        switch (CredentialActivityTypeID)
        {

            case Enumerations.CredentialActivityType.LicenseVerification:
                txtOrgEffDate.Enabled = false;
                txtRenewalDate.Enabled = false;
                txtExpDate.Enabled = false;
                divOrgEffDate.Visible = false;
                divRenewalDate.Visible = false;
                divExpDate.Visible = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.NPDBVerification:
                divOrgEffDate.Visible = false;
                divRenewalDate.Visible = false;
                divExpDate.Visible = false;
                txtOrgEffDate.Enabled = false;
                txtRenewalDate.Enabled = false;
                txtExpDate.Enabled = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.DEAVerification:
                divOrgEffDate.Visible = false;
                divRenewalDate.Visible = false;
                divExpDate.Visible = false;
                txtOrgEffDate.Enabled = false;
                txtRenewalDate.Enabled = false;
                txtExpDate.Enabled = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.ControlledSubstanceVerification:
                divIssuingState.Visible = false;
                divCDSNumber.Visible = false;
                divOrgEffDate.Visible = false;
                divRenewalDate.Visible = false;
                divExpDate.Visible = false;
                txtOrgEffDate.Enabled = false;
                txtRenewalDate.Enabled = false;
                txtExpDate.Enabled = false;
                txtIssuingState.Enabled = false;
                txtCDSNumber.Enabled = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.ProviderAttestation:
                divIssuingState.Visible = false;
                divCDSNumber.Visible = false;
                divOrgEffDate.Visible = false;
                divRenewalDate.Visible = false;
                divExpDate.Visible = false;
                lnkExtraExternalSearch.Text = "";
                txtAttestationDate.Visible = true;
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.MalPracticeInsurance:
                divOrgEffDate.Visible = false;
                divRenewalDate.Visible = false;
                divExpDate.Visible = false;
                txtOrgEffDate.Enabled = false;
                txtRenewalDate.Enabled = false;
                txtExpDate.Enabled = false;
                lnkExtraExternalSearch.Text = "";
                lnkExtraExternalSearch.Visible = false;
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.FiveYearWorkHistory:
                divOrgEffDate.Visible = false;
                divRenewalDate.Visible = false;
                divExpDate.Visible = false;
                txtOrgEffDate.Enabled = false;
                txtRenewalDate.Enabled = false;
                txtExpDate.Enabled = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.BoardVerification:
                divOrgEffDate.Visible = false;
                divRenewalDate.Visible = false;
                divExpDate.Visible = false;
                txtOrgEffDate.Enabled = false;
                txtRenewalDate.Enabled = false;
                txtExpDate.Enabled = false;
                //lnkExtraExternalSearch.Visible = true;
                lnkExtraExternalSearch.Text = "";
                dvBoardVerification.Visible = true;
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
               
                break;
            case Enumerations.CredentialActivityType.Education:
                divOrgEffDate.Visible = false;
                divRenewalDate.Visible = false;
                divExpDate.Visible = false;
                txtOrgEffDate.Enabled = false;
                txtRenewalDate.Enabled = false;
                txtExpDate.Enabled = false;
                lnkExtraExternalSearch.Visible = true;
                lnkExtraExternalSearch.Text = "AMA Provider File";
                divlblUpload.Visible = false;
                divUploadNotRequired.Visible = true;
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.MedicareOptOut:
                //divLicenseStatus.Visible = false;
                //this.ddlVerifiedBy.Enabled = false;
                dvMedicareOptOut.Visible = true;
                txtOrgEffDate.Enabled = false;
                divOrgEffDate.Visible = false;
                divExpDate.Visible = false;
                divRenewalDate.Visible = false;
                txtExpDate.Enabled = false;
                this.lblResultDropDownLabel.Visible = false;
                this.ddlDataRank.Visible = true;
                lnkExtraExternalSearch.Text = "";
                divUploadNotRequired.Visible = true;
                divlblUpload.Visible = false;
                lblUploadNotRequird.InnerText = "Note:Upload Required if Opt Out in effect";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                dvMedicareOptOut.Visible = true;
                break;
            case Enumerations.CredentialActivityType.OIGVerification:
                //this.ddlVerifiedBy.Enabled = false;
                this.divOrgEffDate.Visible = false;
                this.divRenewalDate.Visible = false;
                this.divExpDate.Visible = false;
                //this.divLicenseStatus.Visible = false;
                this.divBoardStatus.Visible = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.SAMVerification:
                //this.ddlVerifiedBy.Enabled = false;
                this.divOrgEffDate.Visible = false;
                this.divRenewalDate.Visible = false;
                this.divExpDate.Visible = false;
                //this.divLicenseStatus.Visible = false;
                this.divBoardStatus.Visible = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.MedicaidExclusionVerification:
                //this.ddlVerifiedBy.Enabled = false;
                this.divOrgEffDate.Visible = false;
                this.divRenewalDate.Visible = false;
                this.divExpDate.Visible = false;
               // this.divLicenseStatus.Visible = false;
                this.divBoardStatus.Visible = false;
                this.lblReferenceLink.Visible = true;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.MedicareExclusionVerification:
                //this.lblVerifiedBy.Enabled = false;
                this.divOrgEffDate.Visible = false;
                this.divRenewalDate.Visible = false;
                this.divExpDate.Visible = false;
                //this.divLicenseStatus.Visible = false;
                this.divBoardStatus.Visible = false;
                this.lblReferenceLink.Visible = true;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.QualityofCarerecredOnly:
                divOrgEffDate.Visible = false;
                divRenewalDate.Visible = false;
                divExpDate.Visible = false;
                txtOrgEffDate.Enabled = false;
                txtRenewalDate.Enabled = false;
                txtExpDate.Enabled = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.MemberComplaintsrecredOnly:
                lblVerifiedBy.Enabled = false;
                txtOrgEffDate.Enabled = false;
                txtRenewalDate.Enabled = false;
                txtExpDate.Enabled = false;
                //divLicenseStatus.Visible = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.OhioDepartmentofInsurance:
                this.divOrgEffDate.Visible = false;
                this.divRenewalDate.Visible = false;
                this.divExpDate.Visible = false;
                //divLicenseStatus.Visible = false;
                lnkExtraExternalSearch.Text = "";
                lblReferenceLink.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;

            case Enumerations.CredentialActivityType.FacilityLicenseVerification:
                txtOrgEffDate.Enabled = false;
                txtRenewalDate.Enabled = false;
                txtExpDate.Enabled = false;
                //divLicenseStatus.Visible = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.FinancialAttestation:
                this.divOrgEffDate.Visible = false;
                this.divRenewalDate.Visible = false;
                this.divExpDate.Visible = false;
                //divLicenseStatus.Visible = false;
                lnkExtraExternalSearch.Text = "";
                divAttestationDate.Visible = true;
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.MaternityLicense:
                this.divOrgEffDate.Visible = false;
                this.divRenewalDate.Visible = false;
                this.divExpDate.Visible = false;
                //divLicenseStatus.Visible = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = true;
                divSiteAccredDate.Visible = false;
                break;

            case Enumerations.CredentialActivityType.SiteVisitAndAccreditation:
                this.divOrgEffDate.Visible = false;
                this.divRenewalDate.Visible = false;
                this.divExpDate.Visible = false;
                //divLicenseStatus.Visible = false;
                lnkExtraExternalSearch.Text = "";
                lblReferenceLink.Visible = false;
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = true;
                break;
            case Enumerations.CredentialActivityType.MedicaidCertification:
                txtOrgEffDate.Enabled = false;
                txtRenewalDate.Enabled = false;
                txtExpDate.Enabled = false;
                divOrgEffDate.Visible = false;
                divRenewalDate.Visible = false;
                divExpDate.Visible = false;
                lblReferenceLink.Visible = false;
                lnkReference.Visible = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.MedicareCertification:
                txtOrgEffDate.Enabled = false;
                txtRenewalDate.Enabled = false;
                txtExpDate.Enabled = false;
                divOrgEffDate.Visible = false;
                divRenewalDate.Visible = false;
                divExpDate.Visible = false;
                lblReferenceLink.Visible = false;
                lnkReference.Visible = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                break;
            case Enumerations.CredentialActivityType.HospitalPrivileges:
                //this.ddlVerifiedBy.Enabled = false;
                this.divOrgEffDate.Visible = false;
                this.divRenewalDate.Visible = false;
                this.divExpDate.Visible = false;
                //this.divLicenseStatus.Visible = false;
                this.divBoardStatus.Visible = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                this.lblReferenceLink.Visible = false;
                break;
            case Enumerations.CredentialActivityType.BedRegistration:
                //this.ddlVerifiedBy.Enabled = false;
                this.divOrgEffDate.Visible = false;
                this.divRenewalDate.Visible = false;
                this.divExpDate.Visible = false;
                //this.divLicenseStatus.Visible = false;
                this.divBoardStatus.Visible = false;
                lnkExtraExternalSearch.Text = "";
                divMaternityLicDate.Visible = false;
                divSiteAccredDate.Visible = false;
                this.lblReferenceLink.Visible = false;
                break;
            case Enumerations.CredentialActivityType.UnDefined:
                break;
            default:
                break;
        }
    }

    private WebRequest InitializeRequest(string method, string methodtype)
    {
        WebRequest request = null;
        try
        {
            //create REquest with method as variable

            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            string AMA_URI = AppSettings.Get("AMAURL");
            string requestMethod = method;
            string requestURL = AMA_URI + requestMethod;
            string requestToken = createatokenRequest(HttpContext.Current.User.Identity.Name);
            string tokenRequestStatus = "";
            if(string.IsNullOrEmpty( requestToken))
            {
                tokenRequestStatus = CON.AMAStatusID.Unauthorized.ToString();
            }
            else
            {
                tokenRequestStatus = CON.AMAStatusID.Successful.ToString();
            }
            Dictionary<string, string> parms = new Dictionary<string, string>();
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            parms.Add("REG_AMA_PROFILE_DOWNLOADS_ID", this.RegAMAProfileDownloadsID.ToString());
            parms.Add("ACCESS_TOKEN_STATUS_CODE", tokenRequestStatus.ToString());
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "AMA_PROFILE_DOWNLOADS", parms);
            request = WebRequest.Create(requestURL);
            request.UseDefaultCredentials = true;
            request.PreAuthenticate = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = methodtype;
            request.ContentType = "application/xml";
            request.Headers.Add("Authorization", "Bearer " + requestToken);
            request.Headers.Add("X-Location", "MAXIMUS");
            request.Headers.Add("X-CredentialProviderUserId", HttpContext.Current.User.Identity.Name);
            request.Headers.Add("X-SourceSystem", AppSettings.Get("BrandName") + AppSettings.Get("Environment"));
        }
        catch
        {
        }

        return request;

    }

    private string createatokenRequest(string username)
    {
        try {
        CredentialHelper ch = new CredentialHelper();
        string accesstoken = ch.createTokenRequest(username);
        return accesstoken;
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    /*private string GetProfileFull(string Id)
    {
        string xmlResponse = string.Empty;
        try
        {
            // string response = GetRequestPycisianByID(Id);
            //if (!string.IsNullOrEmpty(response))
            //{
            string filePath = "~/Documents/AMAProviderFile" + Id + ".pdf";
            WebRequest wrPhysicianFullPDF = InitializeRequest("/profiles/profile/full/" + Id, "GET");


            using (WebResponse rsPAFull = wrPhysicianFullPDF.GetResponse())
            {
                HttpWebResponse rsPAfullStatusCode = (HttpWebResponse)rsPAFull;

                HttpStatusCode statuscode = rsPAfullStatusCode.StatusCode;

                using (var stream = rsPAFull.GetResponseStream())
                {

                        using (StreamReader sr = new StreamReader(stream))
                        {
                            xmlResponse = sr.ReadToEnd();
                        }

                }

            }
            //}
        }
        catch (Exception ex)
        {

        }
        return xmlResponse;
    }*/
    private string GetPDFRequestPhycisianByID(string Id)
    {
        string xmlResponse = string.Empty;
        try
        {
            // string response = GetRequestPycisianByID(Id);
            //if (!string.IsNullOrEmpty(response))
            //{
            string filePath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + "AMAProviderFile" + Id + ".pdf";
            WebRequest wrPhysicianFullPDF = InitializeRequest("/profiles/pdf/full/" + Id, "GET");


            using (WebResponse rsPAFull = wrPhysicianFullPDF.GetResponse())
            {
                HttpWebResponse rsPAfullStatusCode = (HttpWebResponse)rsPAFull;

                HttpStatusCode statuscode = rsPAfullStatusCode.StatusCode;
                Dictionary<string, string> parms = new Dictionary<string, string>();
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                parms.Add("REG_AMA_PROFILE_DOWNLOADS_ID", this.RegAMAProfileDownloadsID.ToString());
                parms.Add("PROFILE_PDF_RESPONSE_STATUS", CredentialHelper.GetAMAStatusId(statuscode.ToString()).ToString());
                parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "AMA_PROFILE_DOWNLOADS", parms);
                using (var stream = rsPAFull.GetResponseStream())
                {
                    if (stream != null)
                    {
                        //using (FileStream fs = new FileStream(@"C:\Users\287299\Desktop\AMAapi\PhysicianFULL.pdf", FileMode.Create))
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            stream.CopyTo(fs);

                        }
                    }
                }

            }
            //}
        }
        catch (WebException htex)
        {
            var ex = htex.Response as HttpWebResponse;
            Dictionary<string, string> parms = new Dictionary<string, string>();
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            parms.Add("REG_AMA_PROFILE_DOWNLOADS_ID", this.RegAMAProfileDownloadsID.ToString());
            parms.Add("PROFILE_PDF_RESPONSE_STATUS", CredentialHelper.GetAMAStatusId(ex.StatusCode.ToString()).ToString());
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Guid.Parse(CON.appAdminUserId).ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Guid.Parse(CON.appAdminUserId).ToString());
            psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "AMA_PROFILE_DOWNLOADS", parms);
        }
        catch
        {
        }
        return xmlResponse;
    }

    private XMLResponseStatus GetProfileByNPI(string npi)
    {
        string response = string.Empty;

        string xmlResponse = string.Empty;
        XMLResponseStatus s = new XMLResponseStatus();
        try
        {
            WebRequest wrPAFull = InitializeRequest("/profiles/search", "POST");
            string requestInput = string.Empty;

            using (System.IO.StreamReader file = new System.IO.StreamReader(Server.MapPath("~/DataTemplate/SearchProfileByNPI.xml")))
            {

                requestInput = file.ReadToEnd().ToString();
                requestInput = requestInput.Replace("NPITEMPLATE", npi);
            }
            byte[] data = Encoding.ASCII.GetBytes(requestInput);
            using (Stream stream = wrPAFull.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            using (WebResponse rsPAFull = wrPAFull.GetResponse())
            {
                HttpWebResponse rsPAfullStatusCode = (HttpWebResponse)rsPAFull;

                HttpStatusCode statuscode = rsPAfullStatusCode.StatusCode;

                using (Stream stream = rsPAFull.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        xmlResponse = sr.ReadToEnd();
                        s.xmlResponse = xmlResponse;
                        s.status = statuscode.ToString();
                    }
                }

            }

        }
        catch
        {
        }
        return s;
    }

    private string GetEntityID(XMLResponseStatus xrs )
    {
        string xmlResponse = xrs.xmlResponse;
        string id = "";
        string status = "";
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlResponse);
            XmlElement root = doc.DocumentElement;
            var list = root.GetElementsByTagName("entityId");
            var statuslist = root.GetElementsByTagName("status");
            id = list[0].InnerXml.Trim().ToString();
            status = xrs.status;
            Dictionary<string, string> parms = new Dictionary<string, string>();
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            parms.Add("REG_AMA_PROFILE_DOWNLOADS_ID", this.RegAMAProfileDownloadsID.ToString());
            parms.Add("PROFILE_BY_NPI_RESPONSE_STATUS_ID", CredentialHelper.GetAMAStatusId(status).ToString());
            parms.Add("ENTITY_ID", id);
            parms.Add("PROFILE_BY_NPI_XML", xmlResponse);
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "AMA_PROFILE_DOWNLOADS", parms);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return id;
    }
    #endregion

    protected void chkBoardVerificationRequird_CheckedChanged(object sender, EventArgs e)
    {
        if (chkBoardVerificationRequird.Checked)
            hdnchkBoardVerification.Value = "false";
    }
    protected void chkMedicareOptOutRequird_CheckedChanged(object sender, EventArgs e)
    {
        if (chkMedicareOptOutRequird.Checked)
        {
            txtVerificationDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            ddlDataRank.SelectedItem.Text = "Pass"; 
        }
           // hdnchkMedicareOptOut.Value = "false";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtAdverseAction.Attributes.Add("maxlength", txtAdverseAction.MaxLength.ToString());
        }
        //SAM459
        if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CredentialReconsideration
              && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.CredentialingReconsideration
              && (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CommitteeQualitySpecialist)
                  || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSupervisor)))
        {
            btnConfirm.Visible = false;
        }
    }

}
