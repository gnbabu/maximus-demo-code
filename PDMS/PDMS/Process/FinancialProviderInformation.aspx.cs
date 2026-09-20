using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.ServiceModel;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Threading;
using System.Collections.Generic;

public partial class Process_FinancialProviderInformation : RegistrationProvider
{

    private PDMSService.PDMSServiceClient _spa;
    private string RecipientEligibilityToken = "";
    private string RecipientEligibilityProviderId = "";
    private string RecipientEligibilityURL = "";

    private PDMSService.PDMSServiceClient spa
    {
        get
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            return _spa;
        }
    }

    private void SetTitle()
    {
        // lblTitle.Text = Resources.BrandingResource.Financial_ProviderInformation;
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

    protected void btnSearch_Click(int pageID)
    {
        if (CON.SectionTypeID.CostReports == pageID)
        {
            GetServiceDetails(pageID, false);
        }
        else if (CON.SectionTypeID.Correspondence == pageID)
        {
            if (!string.IsNullOrEmpty(txtMedicaidNumber.Text) || !string.IsNullOrWhiteSpace(txtRegID1.Text))
            {
                GetServiceDetails(pageID, true);
            }
            else
            {
                AddValidationErrorMessage("Medicaid Id  or Reg ID is required");
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(txtMedicaidNumber.Text))
            {
                if (CON.SectionTypeID.SearcheRA == pageID)
                {
                    GetServiceDetails(pageID, false);
                }
                else if (CON.SectionTypeID.SearchPriorAuthorization == pageID)
                {
                    var flag = AppSettings.Get("RedirectToPNM3BPA");
                    if (flag == "1") //Go to PNM
                        GetServiceDetails(pageID, false);
                    else
                        GetServiceDetails(pageID, true);
                }
                else if (CON.SectionTypeID.ORPProviderSearch == pageID)
                {
                    GetServiceDetails(pageID, false);
                }
                else if (CON.SectionTypeID.SearchClaim == pageID)
                {
                    var flag = AppSettings.Get("RedirectToPNM3BClaims");
                    if (flag == "1") //Go to PNM
                    {
                        GetServiceDetails(CON.SectionTypeID.SearchClaim, false);
                    }
                    else if (flag == "2")
                    {
                        GetServiceDetails(CON.SectionTypeID.SearchClaimV2, false);
                    }
                    else
                    {
                        GetServiceDetails(pageID, true);
                    }
                }
                else if (CON.SectionTypeID.SearchEligibility == pageID)
                {
                    var flag = AppSettings.Get("RedirectToPNM3BMemberEligbility");
                    if (flag == "1") //Go to PNM
                    {
                        GetServiceDetails(CON.SectionTypeID.SearchEligibility, false);
                    }
                    else if (flag == "2")
                    {
                        GetServiceDetails(CON.SectionTypeID.SearchEligibilityV2, false);
                    }
                    else
                    {
                        GetServiceDetails(pageID, true);
                    }
                }
                else
                {
                    //If AppsettingsValue is true then go to PNM
                    if (string.Equals(AppSettings.Get("RedirectToPNM3BHospice"), "1")
                        || string.Equals(AppSettings.Get("RedirectToPNM3BProviderFinancial"), "1")) //Go to PNM
                        GetServiceDetails(pageID, false);
                    else
                        GetServiceDetails(pageID, true);
                }
            }
            else
            {
                if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.RecipientEligibility)
                 || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InfantMortalityLeadEntityAgent))
                {
                    if (CON.SectionTypeID.SearchEligibility == pageID || CON.SectionTypeID.SearchEligibilityV2 == pageID)
                    {
                        var flag = AppSettings.Get("RedirectToPNM3BMemberEligbility");
                        if (flag == "1") //Go to PNM
                        {
                            GetServiceDetails(CON.SectionTypeID.SearchEligibility, false);
                        }
                        else if (flag == "2")
                        {
                            GetServiceDetails(CON.SectionTypeID.SearchEligibilityV2, false);
                        }
                        else
                            GetServiceDetails(pageID, true);
                    }
                }
                else
                {
                    AddValidationErrorMessage("Medicaid Id required");
                }
            }
        }
    }

    protected void BtnAdd_Financial(object sender, CommandEventArgs e)
    {
        btnSearch_Click(CON.SectionTypeID.ProviderFinancial);
    }

    protected void BtnAdd_Remittance(object sender, CommandEventArgs e)
    {
        btnSearch_Click(CON.SectionTypeID.SearcheRA);
    }

    protected void BtnAdd_PriorAuth(object sender, CommandEventArgs e)
    {
        btnSearch_Click(CON.SectionTypeID.SearchPriorAuthorization);
    }

    protected void BtnAdd_ORPProviderSearch(object sender, CommandEventArgs e)
    {
        btnSearch_Click(CON.SectionTypeID.ORPProviderSearch);
    }
    protected void lnkBtnProviderFinance_Click(object sender, CommandEventArgs e)
    {
        btnSearch_Click(CON.SectionTypeID.ProviderFinancial);
    }

    protected void BtnAdd_Claims(object sender, CommandEventArgs e)
    {
        btnSearch_Click(CON.SectionTypeID.SearchClaim);
    }

    protected void BtnAdd_Hospice(object sender, CommandEventArgs e)
    {
        btnSearch_Click(CON.SectionTypeID.HospiceEnrollment);
    }

    protected void BtnAdd_MemberElig(object sender, CommandEventArgs e)
    {
        btnSearch_Click(CON.SectionTypeID.SearchEligibility);
    }

    protected void BtnAdd_Correspondence(object sender, CommandEventArgs e)
    {
        btnSearch_Click(CON.SectionTypeID.Correspondence);
    }

    protected void BtnAdd_NavigateToMITS(object sender, CommandEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtMedicaidNumber.Text))
        {
            GetServiceDetails(0, true);
        }
        else
        {
            AddValidationErrorMessage("Medicaid ID Not Found");
        }
        btnSearch_Click(CON.SectionTypeID.Correspondence);
    }


    protected void BtnAdd_NavigateToMAS(object sender, CommandEventArgs e)
    {
        if (Helper.IsUserInInternalRole(CON.CostReportInternalRoles))
        {
            GetServiceDetails(0, false, true);
        }
        else
        {
            if (!string.IsNullOrEmpty(txtMedicaidNumber.Text))
            {
                GetServiceDetails(0, false, true);
            }
            else
            {
                AddValidationErrorMessage("Medicaid ID Not Found");
            }
        }
        btnSearch_Click(CON.SectionTypeID.Correspondence);
    }

    protected void NavigateToMITS()
    {
        string ReturnURL = "";
        string url = "";
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

        // check for Haven Redirect
        HttpCookie MITSRedirectURL = Request.Cookies["MITSRedirectURL"];
        if (MITSRedirectURL != null)
        {
            try
            {
                if (MITSRedirectURL.Value.Length > 4)
                {  // this is a redirect on an expired or missing token from an embedded PDFlink
                    ReturnURL = Server.UrlEncode(MITSRedirectURL.Value);
                }
                // get rid of cookie so we don't go back
                MITSRedirectURL.Value = null;
                MITSRedirectURL.Expires = DateTime.Now.AddDays(-10);
                Response.SetCookie(MITSRedirectURL);
                Response.Cookies.Remove("MITSRedirectURL");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        if (Request.QueryString["MedicaidId"] != null)
        {
            this.RecipientEligibilityProviderId = Request.QueryString["MedicaidId"];
        }
        else
        {
            this.RecipientEligibilityProviderId = txtMedicaidNumber.Text.Trim();
        }
        // haven handoff data
        this.RecipientEligibilityURL = System.Configuration.ConfigurationManager.AppSettings["RecipientEligibilityURL"];
        this.RecipientEligibilityToken = MaximusJWT.GetNewToken(HttpContext.Current.User.Identity.Name, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), System.Configuration.ConfigurationManager.AppSettings["PNMSecretKey"]);

        // save token to db
        svc.SaveUserPNMToken(Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.RecipientEligibilityToken);

        url = this.RecipientEligibilityURL + "?AuthToken=" + this.RecipientEligibilityToken + "&ProviderId=" + this.RecipientEligibilityProviderId + "&ReturnURL=" + ReturnURL;

        Response.Redirect(url);
    }

    protected void NavigateToMyersStaufers()
    {
        string ReturnURL = "";
        string url = "";
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

        // check for Haven Redirect
        HttpCookie MASRedirectURL = Request.Cookies["MASRedirectURL"];
        if (MASRedirectURL != null)
        {
            try
            {
                if (MASRedirectURL.Value.Length > 4)
                {  // this is a redirect on an expired or missing token from an embedded PDFlink
                    ReturnURL = Server.UrlEncode(MASRedirectURL.Value);
                }
                // get rid of cookie so we don't go back
                MASRedirectURL.Value = null;
                MASRedirectURL.Expires = DateTime.Now.AddDays(-10);
                Response.SetCookie(MASRedirectURL);
                Response.Cookies.Remove("MASRedirectURL");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        if (Request.QueryString["MedicaidId"] != null)
        {
            this.RecipientEligibilityProviderId = Request.QueryString["MedicaidId"];
        }
        else
        {
            this.RecipientEligibilityProviderId = txtMedicaidNumber.Text.Trim();
        }
        // haven handoff data
        this.RecipientEligibilityURL = AppSettings.Get("MayerStauferRedirectURL", "https://ohiofi.mslc.com/SSO/Login");
        this.RecipientEligibilityToken = MaximusJWT.GetNewToken(HttpContext.Current.User.Identity.Name, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), System.Configuration.ConfigurationManager.AppSettings["PNMSecretKey"]);

        // save token to db
        svc.SaveUserPNMToken(Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.RecipientEligibilityToken);

        url = this.RecipientEligibilityURL + "?AuthToken=" + this.RecipientEligibilityToken + "&ProviderId=" + this.RecipientEligibilityProviderId + "&ReturnURL=" + ReturnURL;

        Response.Redirect(url);
    }

    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        return false;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtMedicaidNumber.Text = string.Empty;
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        Guid threadId = Guid.NewGuid();
        string apiusr = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

        try
        {
            // Find hidden fields in the master page
            HiddenField hdnAPIToken = (HiddenField)Page.Master.FindControl("hdnAccessToken");
            HiddenField hdnAPIRefreshToken = (HiddenField)Page.Master.FindControl("hdnRefreshToken");

            CredentialHelper.APIToken apiToken = ApplicationCache.RestAPIAccessToken();

            if (hdnAPIToken != null)
            {
                hdnAPIToken.Value = apiToken.AccessToken;
            }

            if (hdnAPIRefreshToken != null)
            {
                hdnAPIRefreshToken.Value = apiToken.RefreshToken;
            }


            Logging log = new Logging(threadId, "Fetching token - FinancialProviderPage");
            log.CreateLogEntry("Token successfully generated and assigned to hidden access tokens for User:" + apiusr, Logging.LogPriority.Information);

        }
        catch (Exception ex)
        {
            // Log any errors that occur during token retrieval
            Logging log = new Logging(threadId, "Fetching token - FinancialProviderPage");
            log.CreateLogEntry("Token successfully generated and assigned to hidden access tokens for User:" + apiusr, Logging.LogPriority.Error);
        }

        if (Request.QueryString.Count > 0)
        {
            if (Request.QueryString.AllKeys.Contains("MedicaidID"))
            {
                txtMedicaidNumber.Text = Convert.ToString(Request["MedicaidID"]);
                txtMedicaidNumber.ReadOnly = true;
            }
        }

        //enable, disable cost reports link button

        var providerRoleUser = Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name);
        if (!providerRoleUser)
        {
            SetNavLinkVisibility();
        }
    }

    private void SetNavLinkVisibility()
    {
        // check all sub-roles or ProviderAgent
        if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent) == true)
        {
            if (Helper.IsUserInSubRoles(this.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.RemittanceRedirectionSubRoles))
            {
                lnkBtnRemittance.Visible = true;
            }
            if (Helper.IsUserInInternalRole(CON.MemberEligibilityInternalRoles))
            {
                lnkBtnMemberElig.Visible = true;
            }
            if (Helper.IsUserInSubRoles(this.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.FinancialRedirectionSubRoles))
            {
               // lnkBtnFinancial.Visible = true;
            }
            if (Helper.IsUserInSubRoles(this.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.CorrespondenceSubRoles))
            {
                lnkbtnCorrespondence.Visible = true;
            }
            if (Helper.IsUserInSubRoles(this.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.HospiceSubRoles))
            {
                lnkBtnHospice.Visible = true;
            }
            if (Helper.IsUserInSubRoles(this.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.ClaimsSubRoles))
            {
                lnkBtnClaims.Visible = true;
            }
            if (Helper.IsUserInSubRoles(this.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.CostReportSubRoles))
            {
                 LnkBtnCostReports.Visible = true;
            }
            if (Helper.IsUserInSubRoles(this.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.ProviderReportsSubRoles))
            {
                lnkBtnProviderReport.Visible = true;
            }
            if (Helper.IsUserInSubRoles(this.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.PriorAuthorizationSubRoles))
            {
                lnkBtnPriorAuth.Visible = true;
            }
            /*if (Helper.IsUserInSubRoles(this.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.AttachmentsSubRoles))
            {
                lnkBtnAttachments.Visible = true;
            }*/
            if (Helper.IsUserInSubRoles(this.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.ORPSearchSubRoles))
            {
                 lnlBtnORPProviderSearch.Visible = true;
                if (AppSettings.Get("SAM748", "false").Equals("true"))
                {
                    //lnlBtnORPProviderSearch.Visible = true;
                }
            }
        }
        else
        {
            // check all non-sub roles (Internal and Provider)
            if (Helper.IsUserInInternalRole(CON.MemberEligibilityInternalRoles))
            {
                lnkBtnMemberElig.Visible = true;
            }
            if (Helper.IsUserInInternalRole(CON.RemittanceRedirectionInternalRoles))
            {
                lnkBtnRemittance.Visible = true;
            }
            if (Helper.IsUserInInternalRole(CON.FinancialRedirectionInternalRoles))
            {
                //lnkBtnFinancial.Visible = true;
            }
            if (Helper.IsUserInInternalRole(CON.CorrespondenceInternalRoles))
            {
                lnkbtnCorrespondence.Visible = true;
            }
            if (Helper.IsUserInInternalRole(CON.HospiceInternalRoles))
            {
                 lnkBtnHospice.Visible = true;
            }
            if (Helper.IsUserInInternalRole(CON.ClaimsInternalRoles))
            {
                lnkBtnClaims.Visible = true;
            }
            if (Helper.IsUserInInternalRole(CON.CostReportInternalRoles))
            {
                 LnkBtnCostReports.Visible = true;
            }
            if (Helper.IsUserInInternalRole(CON.ProviderReportsInternalRoles))
            {
                lnkBtnProviderReport.Visible = true;
            }
            if (Helper.IsUserInInternalRole(CON.PriorAuthorizationInternalRoles))
            {
                lnkBtnPriorAuth.Visible = true;
            }
            /*if (Helper.IsUserInInternalRole(CON.AttachmentsInternalRoles))
            {
                lnkBtnAttachments.Visible = true;
            }*/
            if (Helper.IsUserInInternalRole(CON.ORPSearchInternalRoles))
            {
                lnlBtnORPProviderSearch.Visible = true;

                if (AppSettings.Get("SAM748", "false").Equals("true"))
                {
                    
                }
            }

            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InfantMortalityLeadEntityAgent))
            {
                //lblMedicaidNumber.Visible = false;
                //txtMedicaidNumber.Visible = false;
                lblRegID.Visible = false;
                txtRegID1.Visible = false;

            }

        }
    }

    private void LoadData(int step)
    {

    }
    private bool SearchIds()
    {
        if (!string.IsNullOrEmpty(SessionVarRetriever.DashBoardRegistrationIds)) return true;
        if (SessionVarRetriever.DashBoardTableId > 0) return true;
        return false;
    }

    private void GetServiceDetails(int pageID, bool isMITS, bool isMAS = false)
    {
        if (pageID == CON.SectionTypeID.ProviderReports)
        {
            var url = string.Format("~/Reports/ReportCriteria.aspx");
            Response.Redirect(url);
            return;
        }
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }

        DataSet dataSet = null;
        DataTable dt = null;
        if (CON.SectionTypeID.Correspondence == pageID)
        {
            if (!string.IsNullOrWhiteSpace(txtMedicaidNumber.Text.Trim()))
            {
                dataSet = _spa.SelectServiceLocationByMedicaidID(txtMedicaidNumber.Text.Trim());
                dt = dataSet.Tables[0];
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(txtRegID1.Text.Trim()))
                {
                    dataSet = _spa.SelectServiceLocationByRegID(txtRegID1.Text.Trim());
                    dt = dataSet.Tables[0];
                }
            }
        }
        else if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.RecipientEligibility)
                 || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InfantMortalityLeadEntityAgent))
        {
            if (!string.IsNullOrWhiteSpace(txtMedicaidNumber.Text.Trim()))
            {
                dataSet = _spa.SelectServiceLocationByMedicaidID(txtMedicaidNumber.Text.Trim());
                dt = dataSet.Tables[0];
            }
        }
        else
        {
            dataSet = _spa.SelectServiceLocationByMedicaidID(txtMedicaidNumber.Text.Trim());
            dt = dataSet.Tables[0];
        }

        if (dt != null && dt.Rows.Count > 0)
        {
            Session["RegId"] = Convert.ToInt32(dt.Rows[0]["REG_ID"]);
            if (isMITS || isMAS)
            {
                if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSAdmin) ||
                        Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSBusinessRelations) ||
                        Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSProviderAssistance) ||
                        Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSProviderEnrollment) ||
                        Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.MITSUATSuperuser))
                {
                    if (isMITS) NavigateToMITS();
                    else if (isMAS) NavigateToMyersStaufers();
                }

                if (pageID.ToString() != CON.ViewProvider.ToString())
                {
                    if (isMITS) NavigateToMITS();
                    else if (isMAS) NavigateToMyersStaufers();
                }
                else
                {
                    string url = string.Empty;
                    Session["StepId"] = pageID;
                    if (pageID == CON.SectionTypeID.SearchClaim)
                    {
                        url = string.Format("~/Process/ClaimSearch.aspx", pageID);
                    }
                    if (pageID == CON.SectionTypeID.SearchClaimV2)
                    {
                        url = string.Format("~/Process/SearchClaims.aspx", pageID);
                    }
                    if (pageID == CON.SectionTypeID.Correspondence)
                    {
                        Response.Redirect("~/Process/ProviderCorrespondence.aspx");
                    }
                }
            }
            else
            {
                string url;
                if (pageID == CON.SectionTypeID.CostReports)
                {
                    url = AppSettings.Get("CostReportsLink");
                }
                if (pageID == CON.SectionTypeID.SearchPriorAuthorization)
                {
                    Session["StepId"] = pageID;
                    Response.Redirect("~/Process/SearchPriorAuthorization.aspx");
                }
                if (CON.SectionTypeID.SearchClaim == pageID)
                {
                    Session["StepId"] = pageID;
                    Response.Redirect("~/Process/ClaimSearch.aspx");
                }
                if (CON.SectionTypeID.SearchClaimV2 == pageID)
                {
                    Session["StepId"] = pageID;
                    Response.Redirect("~/Process/SearchClaims.aspx");
                }
                if (pageID == CON.SectionTypeID.ProviderFinancial)
                {
                    Session["StepId"] = pageID;
                    Response.Redirect("~/Process/ProviderFinancials.aspx");
                }
                if (pageID == CON.SectionTypeID.SearcheRA)
                {
                    Session["StepId"] = pageID;
                    Response.Redirect("~/Process/ERemittanceAdvice.aspx");
                }
                if (pageID == CON.SectionTypeID.SearchEligibility)
                {
                    Session["StepId"] = pageID;
                    Response.Redirect("~/Process/RecipientEligibility.aspx");
                }
                if (pageID == CON.SectionTypeID.SearchEligibilityV2)
                {
                    Session["StepId"] = pageID;
                    Response.Redirect("~/Process/SearchEligibility.aspx");
                }
                if (pageID == CON.SectionTypeID.HospiceEnrollment)
                {
                    Session["StepId"] = pageID;
                    Response.Redirect("~/Process/HospiceEnrollment.aspx");
                }
                if (pageID == CON.SectionTypeID.UploadAttachments)
                {
                    Session["StepId"] = pageID;
                    Response.Redirect("~/Process/StandaloneUploadAttachments.aspx");
                }
                if (pageID == CON.SectionTypeID.RetrieveReports)
                {
                    Session["StepId"] = pageID;
                    Response.Redirect("~/Process/ProviderRetrieveReports.aspx");
                }
                if (pageID == CON.SectionTypeID.ORPProviderSearch)
                {
                    Session["StepId"] = pageID;
                    Response.Redirect("~/Process/ORProviderSearch.aspx");
                }
            }
        }
        else
        {
            if (pageID == CON.SectionTypeID.Correspondence)
            {
                if (!string.IsNullOrWhiteSpace(txtMedicaidNumber.Text.Trim()))
                {
                    txtMedicaidNumber.Text = string.Empty;
                    AddValidationErrorMessage("Medicaid ID not found");
                }
                else
                {
                    txtRegID1.Text = string.Empty;
                    AddValidationErrorMessage("Reg ID not found");
                }
            }
            else if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.RecipientEligibility)
                 || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InfantMortalityLeadEntityAgent))
            {
                txtMedicaidNumber.Text = string.Empty;
                Session["RegId"] = null;
                string url = string.Empty;
                Session["StepId"] = pageID;

                if (pageID == CON.SectionTypeID.CostReports)
                {
                    url = AppSettings.Get("CostReportsLink");
                }
                if (pageID == CON.SectionTypeID.SearchPriorAuthorization)
                {
                    url = "~/Process/SearchPriorAuthorization.aspx";
                }
                if (CON.SectionTypeID.SearchClaim == pageID)
                {
                    url = "~/Process/ClaimSearch.aspx";
                }
                if (CON.SectionTypeID.SearchClaimV2 == pageID)
                {
                    url = "~/Process/SearchClaims.aspx";
                }
                if (pageID == CON.SectionTypeID.ProviderFinancial)
                {
                    url = "~/Process/ProviderFinancials.aspx";
                }
                if (pageID == CON.SectionTypeID.SearcheRA)
                {
                    url = "~/Process/ERemittanceAdvice.aspx";
                }
                if (pageID == CON.SectionTypeID.SearchEligibility)
                {
                    url = "~/Process/RecipientEligibility.aspx";
                }
                if (pageID == CON.SectionTypeID.SearchEligibilityV2)
                {
                    url = "~/Process/SearchEligibility.aspx";
                }
                if (pageID == CON.SectionTypeID.HospiceEnrollment)
                {
                    url = "~/Process/HospiceEnrollment.aspx";
                }
                if (pageID == CON.SectionTypeID.UploadAttachments)
                {
                    url = "~/Process/StandaloneUploadAttachments.aspx";
                }
                if (pageID == CON.SectionTypeID.RetrieveReports)
                {
                    url = "~/Process/ProviderRetrieveReports.aspx";
                }
                if (!string.IsNullOrEmpty(url))
                {
                    Response.Redirect(url);
                }
            }
            txtMedicaidNumber.Text = string.Empty;
            AddValidationErrorMessage("Medicaid ID not found");
        }
    }

    [WebMethod(EnableSession = true)]
    public static string UpdateSessionToken(string token)
    {
        HttpContext.Current.Session["APIToken"] = token;
        return "";
    }
    private void ucErrorProcess_ReturnEvent(bool refreshGrid)
    {
        //btnReturn_Click(new object(), new EventArgs());
    }

    protected void btnReturnToGroupAffiliations_Click(object sender, EventArgs e)
    {
        // TODO: EDV What functionality is this??
        //Response.Redirect("Registration.aspx?Step=4");
    }

    protected void LnkBtnCostReports_Command(object sender, CommandEventArgs e)
    {
        btnSearch_Click(CON.SectionTypeID.CostReports);

    }

    protected void lnkBtnProviderReport_Command(object sender, CommandEventArgs e)
    {
        btnSearch_Click(CON.SectionTypeID.RetrieveReports);
    }
    protected void lnkBtnAttachments_Command(object sender, CommandEventArgs e)
    {
        btnSearch_Click(CON.SectionTypeID.UploadAttachments);
    }
}