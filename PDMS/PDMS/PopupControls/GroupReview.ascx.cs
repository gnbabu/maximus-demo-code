using Corp.Core.Libraries;
using Corp.Core.Libraries.Helper;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MAXIMUS.Models.Data.PDMS;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using CON = MAXIMUS.Core.Libraries.Constants;
using Control = System.Web.UI.Control;
using TextBox = System.Web.UI.WebControls.TextBox;

public partial class UserControls_GroupReview : System.Web.UI.UserControl
{
    public delegate void RegistrationViewEventHandler(int registrationId);
    public event RegistrationViewEventHandler RegistrationViewEvent;
    protected int _MaxFileMegaBytes;
    private string _DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
    private string _ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
    private Logging log = null;
    private bool _IsExcel = false;
    private bool enableCR537 = Convert.ToBoolean(AppSettings.Get("EnableCR537", "false"));

    #region Properties

    private int ApplicationType = 0;

    private bool UserCanRapidAdminRegistrations
    {
        get
        {
            return Helper.IsLoggedInUserInAdminRole() || Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name);
        }
    }

    private int SelectedRegID
    {
        get
        {
            return ViewState["SelectedRegID"] == null ? 0 : Convert.ToInt32(ViewState["SelectedRegID"]);
        }
        set
        {
            ViewState["SelectedRegID"] = value;
        }
    }

    #endregion

    #region Page Events

    public int MaxFileMegaBytes
    {
        get
        {
            if (_MaxFileMegaBytes == 0) _MaxFileMegaBytes = 80;       // Default is 5MB
            return _MaxFileMegaBytes;
        }
        set
        {
            _MaxFileMegaBytes = value;
            if (_MaxFileMegaBytes > 80) _MaxFileMegaBytes = 80;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        SetupButtonDisablesOfMultiClick();
        if (Helper.IsModern())
        {
            //btnSearch.CssClass = "buttonBoxFocus";
            //gvProviders.Columns[9].HeaderStyle.CssClass = "hiddencol";
            //this.gvProviders.Columns[9].Visible = false;
            //this.gvProviders.Columns[11].Visible = false;
            //gvProviders.DataBind();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.Header.Controls.OfType<HtmlLink>().Any(link => link.Href.Contains("GroupReview.css")))
        {
            HtmlLink cssLink = new HtmlLink();
            cssLink.Href = Page.ResolveClientUrl("~/App_Themes/Modernization/GroupReview.css");
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["type"] = "text/css";
            Page.Header.Controls.Add(cssLink);
        }

        if (!IsPostBack)
        {
            try
            {
                BindDropDown();
                // If list of IDs has been passed in, make sure both ID lists are non-empty and skip to the search results view.
                if (SearchIds()) Search(false);
                pnlResultHeader.Visible = false;
                if (Request["DashboardLabel"] != null)
                {
                    lblResultHeader.Text = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(Request["DashboardLabel"].ToString(), true);
                    pnlResultHeader.Visible = true;
                }


                bool isChopOperator = Helper.IsLoggedInUserInLTCChopRole() || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.LTCInitialRevalidation);
                bool isComplianceSpecialistRole = Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ComplianceSpecialist);
                bool isProviderServicesRole = Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderServices);
                bool isReadOnlyRole = Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ReadOnly);
                bool isInternalApplicationEntryRole = Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InternalApplicationsEntry);
                bool isAPMSpecialist = Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.APMSpecialist);
                bool isCredentialSupervisor = Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSupervisor) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CommitteeQualitySpecialist)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSupervisor);
                bool isEnrollmentSpecialist = Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.EnrollmentSpecialist);

                if (UserCanRapidAdminRegistrations || isChopOperator || isComplianceSpecialistRole || isProviderServicesRole || isReadOnlyRole
                    || isInternalApplicationEntryRole || isAPMSpecialist || isCredentialSupervisor || isEnrollmentSpecialist)
                {
                    gvProviders.Columns[2].Visible = true;
                }
                else
                {
                    gvProviders.Columns[2].Visible = false;
                }

                gvProviders.Columns[3].Visible = Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.LTCInitialRevalidation) || Helper.IsLoggedInUserInAdminRole() || Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.TechAdmin)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ReadOnly) || isAPMSpecialist;

                if ((ddlPDMSStatus.SelectedIndex > 0 && ddlPDMSStatus.SelectedItem.Text != CON.RegistrationTaskName.SiteVisitPCG && ddlPDMSStatus.SelectedItem.Text != CON.RegistrationTaskName.SiteVisitCompliance)
                    && ((Helper.IsLoggedInUserInAdminRole() || Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSpecialist)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InternalApplicationsEntry)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderServices)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.TechAdmin)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ComplianceSpecialist)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CommitteeQualitySpecialist)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.CredentialingQualityAssurance)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ReadOnly)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSupervisor)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCInitialReval)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCCHOP)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ODMCredentialingQualityAssurance)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.PnmSuperUser))))
                {
                    //lblAssigned.Visible = lblAssignedUser.Visible = ddlAssigned.Visible = ddlAssignedUser.Visible = true;
                    seeAssignedPanel.Visible = true;
                    seeAssignedUser.Visible = true;
                    if (gvProviders.Rows.Count > 0)
                    {
                        seetblBulkManage.Visible = true;
                        seetblAssignUser.Visible = true;

                    }
                    else
                    {
                        gvProviders.Columns[0].Visible = false;
                        seetblBulkManage.Visible = false;
                        seetblAssignUser.Visible = false;

                    }
                }
                else
                {
                    seetblBulkManage.Visible = false;
                    seetblAssignUser.Visible = false;
                    seeAssignedPanel.Visible = false;
                    seeAssignedUser.Visible = false;
                    ddlAssigned.SelectedIndex = ddlAssignedUser.SelectedIndex = 0;

                }
            }
            catch (Exception ex)
            {
                MessageBox2.Show("An error occurred when loading the work queue.<br>Error: " + ex.Message + " " + ex.StackTrace, "Error");
            }
        }

        SetSearchEntries();
        btnBulkEmail.Visible = (
            (Helper.IsLoggedInUserInAdminRole()
            || Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InternalApplicationsEntry)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSpecialist)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.TechAdmin)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ComplianceSpecialist)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSpecialist)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCInitialReval)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCCHOP)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.PnmSuperUser)) && (gvProviders.Rows.Count > 0));


        this.ucDisenrollmentView.UpdateSuccessEvent += new Views_DisenrollmentView.UpdateSuccessEventHandler(View_UpdateSuccess);
        this.ucDisenrollmentView.CancelEvent += new Views_DisenrollmentView.CancelEventHandler(View_CancelRequest);
        this.ucDisenrollmentView.KeepPopupOpenEvent += new Views_DisenrollmentView.KeepPopupOpenEventHandler(View_KeepPopupOpen);

        this.ucRetroView.UpdateSuccessEvent += new Views_RetroEffectiveView.UpdateSuccessEventHandler(View_UpdateSuccess);
        this.ucRetroView.CancelEvent += new Views_RetroEffectiveView.CancelEventHandler(View_CancelRequest);
        this.ucRetroView.KeepPopupOpenEvent += new Views_RetroEffectiveView.KeepPopupOpenEventHandler(View_KeepPopupOpen);

        this.ucTermView.UpdateSuccessEvent += new Views_ExpressTerminationView.UpdateSuccessEventHandler(View_UpdateSuccess);
        this.ucTermView.CancelEvent += new Views_ExpressTerminationView.CancelEventHandler(View_CancelRequest);
        this.ucTermView.KeepPopupOpenEvent += new Views_ExpressTerminationView.KeepPopupOpenEventHandler(View_KeepPopupOpen);
        this.ucTermView.TermWorkflowCreatedEvent += new Views_ExpressTerminationView.TermWorkflowCreatedEventHandler(View_TermWorkflowCreated);

        this.ucReactivateView.UpdateSuccessEvent += new Views_ReactivationView.UpdateSuccessEventHandler(View_ReactivateUpdateSuccess);
        this.ucReactivateView.CancelEvent += new Views_ReactivationView.CancelEventHandler(View_CancelRequest);
        this.ucReactivateView.KeepPopupOpenEvent += new Views_ReactivationView.KeepPopupOpenEventHandler(View_KeepPopupOpen);


        this.ucProviderAddView.InsertEvent += new Views_ProviderAddView.InsertEventHandler(View_NewRegistrationInsert);
        this.ucProviderAddView.CancelEvent += new Views_ProviderAddView.CancelEventHandler(View_CancelNewRegistrationRequest);
        this.ucProviderAddView.ValidationEvent += new Views_ProviderAddView.ValidationEventHandler(View_ValidationEventRequest);
        this.ucProviderAddView.KeyFieldUpdateEvent += new Views_ProviderAddView.KeyFieldUpdateEventHandler(View_ResetSelectedRecord);
        this.ucProviderAddView.KeepPopupOpenEvent += new Views_ProviderAddView.KeepPopupOpenEventHandler(View_KeepPopupOpen);

        this.ucNewTermDt.UpdateSuccessEvent += new PopupControls_NewTerminationDateView.UpdateSuccessEventHandler(View_UpdateSuccess);
        this.ucNewTermDt.CancelEvent += new PopupControls_NewTerminationDateView.CancelEventHandler(View_CancelRequest);
        this.ucNewTermDt.KeepPopupOpenEvent += new PopupControls_NewTerminationDateView.KeepPopupOpenEventHandler(View_KeepPopupOpen);

    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // Clear these out prior to doing the Search
        this.lblMessages.Text = string.Empty;
        SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
        SessionVarRetriever.DashBoardTableId = 0;
        SessionVarRetriever.BulkEmailProviderIds = null;
        Search(true);
        lnkRowCount.Visible = lnkExcel.Visible = lnkPDF.Visible = gvProviders.Rows.Count > 0;
        btnSendBulkEmail.Visible = false;
        seetblAssignUser.Visible = false;
        btnBulkEmail.Visible = (
                                  (Helper.IsLoggedInUserInAdminRole()
                                || Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name)
                                || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSpecialist)
                                || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.TechAdmin)
                                || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSpecialist)
                                || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InternalApplicationsEntry)
                                || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ComplianceSpecialist)
                                || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCInitialReval)
                                || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCCHOP)
                                || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.PnmSuperUser))
                                && (gvProviders.Rows.Count > 0)
                                );
        if ((gvProviders.Rows.Count > 0 && ddlPDMSStatus.SelectedItem.Text != CON.RegistrationTaskName.SiteVisitPCG && ddlPDMSStatus.SelectedItem.Text != CON.RegistrationTaskName.SiteVisitCompliance && ddlPDMSStatus.SelectedIndex > 0) &&
           (Helper.IsLoggedInUserInAdminRole()
            || Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSpecialist)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderServices)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.TechAdmin)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSpecialist)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ReadOnly)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InternalApplicationsEntry)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ComplianceSpecialist)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CommitteeQualitySpecialist)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.CredentialingQualityAssurance)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCInitialReval)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCCHOP)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ODMCredentialingQualityAssurance)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.PnmSuperUser)
           )
           )
        {
            seetblBulkManage.Visible = true;
        }
        else
        {
            gvProviders.Columns[0].Visible = false;
            seetblBulkManage.Visible = false;
            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.CredentialingSupervisor) && ddlPDMSStatus.SelectedIndex > 0 && gvProviders.Rows.Count > 0)
            {
                seetblBulkManage.Visible = true;
            }
        }
        //OHPNM-14140:Provider Services Role should not have Admin and Update
        if (gvProviders.Rows.Count > 0 && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ReadOnly) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderServices))
        {
            gvProviders.Columns[2].Visible = gvProviders.Columns[3].Visible = false;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.lblMessages.Text = string.Empty;
        txtDODDContractNumber.Text = txtMedicareNumber.Text = txtRegID.Text = txtCity.Text = txtCounty.Text = txtDateReceived.Text =
            txtPDMSStatusDate.Text = txtGroupName.Text = txtMedicaidID.Text = txtNPI.Text = txtTaxID.Text = txtDBAName.Text = string.Empty;
        ddlEnrollmentStatusReason.SelectedIndex = ddlArea.SelectedIndex = ddlTaxonomy.SelectedIndex = ddlWaiverType.SelectedIndex =
            ddlMCP.SelectedIndex = ddlProgram.SelectedIndex = ddlPDMSStatus.SelectedIndex = ddlTennCareStatus.SelectedIndex =
            ddlCategory.SelectedIndex = ddlProviderType.SelectedIndex = ddlApplicationType.SelectedIndex = ddlRiskLevel.SelectedIndex = ddlSpecialty.SelectedIndex =
            ddlODARegistrationStatus.SelectedIndex = ddlDODDRegistrationStatus.SelectedIndex = ddlPNMAppStatus.SelectedIndex = ddlPNMEnrollStatus.SelectedIndex = -1;
        lnkRowCount.Visible = lnkExcel.Visible = lnkPDF.Visible = false;
        SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
        SessionVarRetriever.DashBoardTableId = 0;
        gvProviders.DataSource = null;
        gvProviders.DataBind();

        ddlCategory.Items.Clear();
        ddlProviderType.Items.Clear();
        ddlSpecialty.Items.Clear();
        ddlArea.Items.Clear();

        lblResultHeader.Text = string.Empty;
        pnlResultHeader.Visible = false;
        gvProviders.Columns[0].Visible = seetblAssignUser.Visible = false;
        btnSendBulkEmail.Visible = false;
        seetblBulkManage.Visible = false;
        seeAssignedPanel.Visible = false;
        seeAssignedUser.Visible = false;
        if (ddlAssigned.Items != null && ddlAssigned.Items.Count > 0)
            ddlAssigned.SelectedIndex = 0;
        if (ddlAssignedUser.Items != null && ddlAssignedUser.Items.Count > 0)
            ddlAssignedUser.SelectedIndex = 0;
    }

    protected void lnkReview_Click(object sender, CommandEventArgs e)
    {
        if (("ReviewRow").Equals(e.CommandName))
        {
            if (RegistrationViewEvent != null) RegistrationViewEvent(Convert.ToInt32(e.CommandArgument.ToString()));
        }
    }



    protected void gvProviders_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }

    protected void gvProviders_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }

    protected void gvProviders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "ReviewRow" && e.CommandName != "ExpressAdmin" && e.CommandName != "OperatorUpdate" && e.CommandName != "RestrictedServices" && e.CommandName != "ContractMaintenance")
            return;

        Helper.IsUpdateCPCContact = false;

        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "ReviewRow")
        {
            int regId = (int)this.gvProviders.DataKeys[index].Values["RegID"];
            if (regId > 0)
            {
                (this.Page as RegistrationProvider).RegistrationId = regId;
                (this.Page as RegistrationProvider).IsReadOnly = false;
                (this.Page as RegistrationProvider).CommandName = "ReviewRow";
                if (RegistrationViewEvent != null) RegistrationViewEvent(regId);
                string logMsg = String.Format("GroupReview RegID - " + regId.ToString() + " by " + HttpContext.Current.User.Identity.Name + ", Role(s) - " + String.Join(", ", Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name)));
                Logging log = new Logging(Guid.NewGuid(), logMsg);
                log.CreateLogEntry(string.Format(logMsg, Logging.LogPriority.Information));
            }
        }
        else if (e.CommandName == "ExpressAdmin")
        {
            (this.Page as RegistrationProvider).CommandName = "ExpressAdmin";
            lblNPIReactivate.Text = string.Empty;
            hdnNPI.Value = string.Empty;
            PDMSService.PDMSServiceClient psc1 = new PDMSService.PDMSServiceClient();
            DataSet ds1 = psc1.SelectReconsiderEnrollmentStatusReasons();
            gvProviders.SelectRow(index);
            SelectedRegID = (int)this.gvProviders.DataKeys[index].Values["RegID"];
            string currentStepID = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["CurrentStepID"].ToString()) ? string.Empty : this.gvProviders.DataKeys[index].Values["CurrentStepID"].ToString();
            string regUserID = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["UserID"].ToString()) ? string.Empty : this.gvProviders.DataKeys[index].Values["UserID"].ToString();
            int regPgmStatusTypeID = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["REG_PROGRAM_STATUS_TYPE_ID"].ToString()) ? 0 : (int)this.gvProviders.DataKeys[index].Values["REG_PROGRAM_STATUS_TYPE_ID"];
            int entityTypeID = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["ENTITY_TYPE_ID"].ToString()) ? 0 : (int)this.gvProviders.DataKeys[index].Values["ENTITY_TYPE_ID"];
            string enrollmentStatusCode = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["ENROLLMENT_STATUS_CODE"].ToString()) ? "" : this.gvProviders.DataKeys[index].Values["ENROLLMENT_STATUS_CODE"].ToString();
            string enrollmentStatusReasonCode = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["ENROLLMENT_STATUS_REASON"].ToString()) ? "" : this.gvProviders.DataKeys[index].Values["ENROLLMENT_STATUS_REASON"].ToString();
            string enrollmentStatusReasonCde = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["ENROLLMENT_STATUS_REASONS_CDE"].ToString()) ? "" : this.gvProviders.DataKeys[index].Values["ENROLLMENT_STATUS_REASONS_CDE"].ToString();
            string npi = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["NPI"].ToString()) ? null : this.gvProviders.DataKeys[index].Values["NPI"].ToString();
            int registrationStatusTypeID = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["REGISTRATION_STATUS_TYPE_ID"].ToString()) ? 0 : (int)this.gvProviders.DataKeys[index].Values["REGISTRATION_STATUS_TYPE_ID"];
            string medicaidID = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["BaseMedicaidID"].ToString()) ? "" : this.gvProviders.DataKeys[index].Values["BaseMedicaidID"].ToString();
            string endDateString = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["END_DATE"].ToString()) ? "" : this.gvProviders.DataKeys[index].Values["END_DATE"].ToString();
            int applicationTypeID = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["APPLICATION_TYPE_ID"].ToString()) ? 0 : (int)this.gvProviders.DataKeys[index].Values["APPLICATION_TYPE_ID"];
            int waiverTypeID = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["WAIVER_TYPE_ID"].ToString()) ? 0 : (int)this.gvProviders.DataKeys[index].Values["WAIVER_TYPE_ID"];
            string pdmsStatus = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["PDMSStatus"].ToString()) ? "" : this.gvProviders.DataKeys[index].Values["PDMSStatus"].ToString();
            int providerTypeID = string.IsNullOrEmpty(this.gvProviders.DataKeys[index].Values["PROVIDER_TYPE_ID"].ToString()) ? 0 : (int)this.gvProviders.DataKeys[index].Values["PROVIDER_TYPE_ID"];
            string MMISProviderTypeID = string.Empty;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.GetProviderTypeById(providerTypeID);
            if (Helper.HasRows(ds))
            {
                MMISProviderTypeID = Helper.GetString("MMIS_PROVIDER_TYPE_ID", ds.Tables[0].Rows[0]);
            }
            DateTime? endDate = null;
            if (endDateString != "") endDate = DateTime.Parse(endDateString);

            bool regOkToTerm = CanTermRegistration(currentStepID, regUserID, regPgmStatusTypeID, entityTypeID, enrollmentStatusCode);
            bool regOkToRetroDate = CanRetroDateRegistration(currentStepID, regUserID, regPgmStatusTypeID, entityTypeID);
            bool regOkToReactivate = CanReactivateRegistration(currentStepID, regUserID, regPgmStatusTypeID, entityTypeID, enrollmentStatusCode, npi, SelectedRegID, MMISProviderTypeID);
            //bool regOkToUpdate = CanUpdateRegistration(currentStepID, regPgmStatusTypeID, registrationStatusTypeID, medicaidID, endDate, enrollmentStatusCode, regUserID);
            bool regOkToDisenroll = CanDisenroll(currentStepID, regUserID, regPgmStatusTypeID, entityTypeID, enrollmentStatusCode, providerTypeID);
            bool regOkToSuspend = CanSuspend(currentStepID, regUserID, regPgmStatusTypeID, entityTypeID, enrollmentStatusCode, pdmsStatus);
            bool regOKToCredential = CanStartCredentialingEvent(currentStepID, regPgmStatusTypeID, registrationStatusTypeID, medicaidID, endDate, enrollmentStatusCode, regUserID);
            bool regOkToKey = CanEditKeyIdentifier(currentStepID, regUserID, regPgmStatusTypeID, entityTypeID, enrollmentStatusCode, applicationTypeID, waiverTypeID);
            bool regOkToReconsider = CanStartProviderReconsideration(currentStepID, regUserID, regPgmStatusTypeID, entityTypeID, enrollmentStatusCode, enrollmentStatusReasonCde, endDate);
            bool regOkHearingRights = CanStartHearingRights(currentStepID, regPgmStatusTypeID, enrollmentStatusCode);
            bool regOkToLTCCHOP = CanStartLTCCHOP(currentStepID, regPgmStatusTypeID, enrollmentStatusCode, providerTypeID);
            bool regOkToNPI = new PDMSService.PDMSServiceClient().GetValidateNPI(npi);
            bool regOkNotProcessed = psc1.IsNotProcessedEligible(SelectedRegID);
            bool regOKChangeTermDt = CanChangeTermDate(currentStepID, regPgmStatusTypeID, enrollmentStatusCode);
            bool regOkToCredRecon = CanStartCredentialReconsideration(SelectedRegID);
            bool regOkToRevertSuspend = CanStartRevertSuspension(currentStepID, regUserID, regPgmStatusTypeID, enrollmentStatusCode);
            bool regOkToSiteVistEvent = CanStartSiteVisitEvent(currentStepID, regUserID, SelectedRegID, enrollmentStatusCode);  //SAM538
            ApplicationType = applicationTypeID;
            if (SelectedRegID > 0)
            {
                ShowExpressAdminOptions(regOkToNPI, regOkToTerm, regOkToRetroDate, regOkToReactivate, regOkToDisenroll, regOKToCredential, regOkToKey, regOkToReconsider, regOkToSuspend, regOkToLTCCHOP, regOkHearingRights, regOkNotProcessed, regOKChangeTermDt, regOkToCredRecon, regOkToRevertSuspend, regOkToSiteVistEvent);
            }
        }
        else if (e.CommandName == "OperatorUpdate")
        {
            SelectedRegID = (int)this.gvProviders.DataKeys[index].Values["RegID"];
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectRegistrationByRegID(SelectedRegID);

            if (Helper.GetInt("RegistrationProgramStatusTypeID", ds.Tables[0].Rows[0]) != CON.RegistrationProgramStatusTypeId.Conversion ||
              (Helper.GetInt("RegistrationProgramStatusTypeID", ds.Tables[0].Rows[0]) == CON.RegistrationProgramStatusTypeId.Conversion &&
               Helper.GetInt("ApplicationTypeID", ds.Tables[0].Rows[0]) == CON.ApplicationType.Waiver &&
               string.IsNullOrEmpty(ds.Tables[0].Rows[0]["NPI"].ToString())
               ))
            {
                psc.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", SelectedRegID.ToString() }, { "WAIVER_SERVICE_UPDATE_TYPE_ID", CON.WaiverServiceUpdateType.OperatorUpdate.ToString() } });
                UserControls_ProviderManagementView view = LoadControl("~/PopupControls/ProviderManagementView.ascx") as UserControls_ProviderManagementView;
                view.InitView(new ProviderManagerData() { RegID = SelectedRegID });
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                bool reValFlag = (view.KeyFields.ApplicationTypeID == CON.ApplicationType.ChangeOfOperator) ? true : false;
                int workflowID = svc.GetWorkflowInstance(view.KeyFields.ApplicationTypeID, view.KeyFields.ProviderCategoryTypeID,
                            view.KeyFields.ProviderTypeID, view.KeyFields.ReferralTypeID, reValFlag, false, false, false);
                view.BeginUpdateRegistration(workflowID, false, false, false, false, CON.WaiverServiceUpdateType.OperatorUpdate);
                Response.Redirect("~/Process/ProviderUpdateSummary.aspx?RegId=" + view.KeyFields.RegID);
            }
            else
            {
                ProviderManagerData keyData = ManageConvertedProvider(ds);

                this.mltExpressAdmin.SetActiveView(vwUpdateConverted);
                this.ucProviderAddView.InitView(keyData);
                this.ucProviderAddView.SaveProviderNoPopup();
            }
        }
        else if (e.CommandName == "RestrictedServices")
        {

            SelectedRegID = (int)this.gvProviders.DataKeys[index].Values["RegID"];
            ProviderManagementData data = new ProviderManagementData();
            data.RegID = SelectedRegID;
            (this.Page as RegistrationProvider).RegistrationId = data.RegID;
            (this.Page as RegistrationProvider).IsReadOnly = false;
            (this.Page as RegistrationProvider).RegistrationStep = CON.SectionTypeID.RestrictedService;

        }
        else if (e.CommandName == "ContractMaintenance")
        {
            int regId = (int)this.gvProviders.DataKeys[index].Values["RegID"];
            if (regId > 0)
            {
                (this.Page as RegistrationProvider).RegistrationId = regId;
                (this.Page as RegistrationProvider).IsReadOnly = false;
                (this.Page as RegistrationProvider).RegistrationStep = CON.SectionTypeID.ContractMaintenance;
                if (RegistrationViewEvent != null) RegistrationViewEvent(regId);
            }
        }
    }

    protected void gvProviders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[6].Text == "9999999999")
            {
                e.Row.Cells[6].Text = string.Empty;
            }

            // Get provider values before checking permitted actions
            int regID = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "RegID").ToString());
            string currentStepID = DataBinder.Eval(e.Row.DataItem, "CurrentStepID").ToString();
            string regUserID = DataBinder.Eval(e.Row.DataItem, "UserID").ToString();
            int regPgmStatusTypeID = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "REG_PROGRAM_STATUS_TYPE_ID").ToString());
            var entitytypeIdStr = DataBinder.Eval(e.Row.DataItem, "ENTITY_TYPE_ID").ToString();
            int entityTypeID = string.IsNullOrEmpty(entitytypeIdStr) ? 0 : int.Parse(entitytypeIdStr);
            string enrollmentStatusCode = DataBinder.Eval(e.Row.DataItem, "ENROLLMENT_STATUS_CODE").ToString();
            string enrollmentStatusReasonCode = DataBinder.Eval(e.Row.DataItem, "ENROLLMENT_STATUS_REASON").ToString();
            // Possibly remove. Getting registration status since there is test data where the provider is in maintenance,
            // but they are not approved.
            int registrationStatusTypeID;
            if (e.Row.DataItem != null && !string.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "REGISTRATION_STATUS_TYPE_ID"))))
                int.TryParse(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "REGISTRATION_STATUS_TYPE_ID")), out registrationStatusTypeID);
            else
                registrationStatusTypeID = 0;
            string medicaidID = DataBinder.Eval(e.Row.DataItem, "BaseMedicaidID").ToString();
            string endDateString = DataBinder.Eval(e.Row.DataItem, "END_DATE").ToString();
            string pdmsStatus = string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "PDMSStatus").ToString()) ? "" : DataBinder.Eval(e.Row.DataItem, "PDMSStatus").ToString();
            DateTime? endDate = null;
            if (endDateString != "") endDate = DateTime.Parse(endDateString);

            bool regOkToTerm = CanTermRegistration(currentStepID, regUserID, regPgmStatusTypeID, entityTypeID, enrollmentStatusCode);
            bool regOkToRetroDate = CanRetroDateRegistration(currentStepID, regUserID, regPgmStatusTypeID, entityTypeID);
            //bool regOkToReactivate = CanReactivateRegistration(currentStepID, regUserID, regPgmStatusTypeID, entityTypeID, enrollmentStatusCode);
            bool regOkToUpdateRegistration = CanUpdateRegistration(currentStepID, regPgmStatusTypeID, registrationStatusTypeID, medicaidID, endDate, enrollmentStatusCode, regUserID);
            bool regOkToViewRestricted = CanViewRestricted(currentStepID, regPgmStatusTypeID, registrationStatusTypeID, medicaidID, endDate, enrollmentStatusCode, regUserID, regID);
            bool regOkToViewContract = CanViewContract(currentStepID, regPgmStatusTypeID, registrationStatusTypeID, medicaidID, endDate, enrollmentStatusCode, regUserID);
            LinkButton lnkExpress = (LinkButton)e.Row.FindControl("lnkExpressAdmin");
            LinkButton lnkUpdate = (LinkButton)e.Row.FindControl("lnkUpdate");
            LinkButton lnkRestricted = (LinkButton)e.Row.FindControl("lnkRServices");
            LinkButton lnkContractMaintenance = (LinkButton)e.Row.FindControl("lnkContractMaintenance");
            int providerType = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "PROVIDER_TYPE_ID").ToString());
            //this control is hidden on page load if user role is not allowed to perform express operations.
            if (lnkExpress != null)
            {
                //commented for bug DCPDMS-3111 becuase admin link should available for the admin role to edit key identifiers irrespective of any status of the registration in workflow or in maintenance
                //lnkExpress.Visible = regOkToTerm || regOkToRetroDate || regOkToReactivate || regOkToUpdateRegistration;
                lnkExpress.Visible = true;
            }

            if (lnkUpdate != null)
            {
                lnkUpdate.Visible = regOkToUpdateRegistration;
            }

            if (lnkRestricted != null)
            {

                //Check for provider types and roles
                if (regOkToViewRestricted && (Helper.IsUserInRestrictedServiceViewRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInRestrictedServiceUpdateRole(HttpContext.Current.User.Identity.Name)))
                {
                    //try for providertypes check

                    if (Helper.IsProviderOfMMISProviderType(providerType, CON.MMISProviderType.Building_and_Wing_ID_for_LTC_Facilites_Only) ||
                        (Helper.IsProviderOfMMISProviderType(providerType, CON.MMISProviderType.CPC_Entity)) ||
                          (Helper.IsProviderOfMMISProviderType(providerType, CON.MMISProviderType.Converted_Inactive_ProviderType)) ||
                          (Helper.IsProviderOfMMISProviderType(providerType, CON.MMISProviderType.Licensee)) ||
                          (Helper.IsProviderOfMMISProviderType(providerType, CON.MMISProviderType.Operator)) ||
                          (Helper.IsProviderOfMMISProviderType(providerType, CON.MMISProviderType.Unpaid_Support_Broker)) ||
                          (Helper.IsProviderOfMMISProviderType(providerType, CON.MMISProviderType.Supported_Living)))
                    {
                        lnkRestricted.Visible = false;
                    }
                    else
                        lnkRestricted.Visible = true;

                }

            }
            if (lnkContractMaintenance != null)
            {
                if (regOkToViewContract && (Helper.IsUserInRestrictedServiceViewRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InternalApplicationsEntry)
                        || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.LTCCHOP) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.APMSpecialist)))
                {
                    if (Helper.IsProviderOfMMISProviderType(providerType, CON.MMISProviderType.Building_and_Wing_ID_for_LTC_Facilites_Only) ||
                          // (Helper.IsProviderOfMMISProviderType(providerType, CON.MMISProviderType.CPC_Entity)) ||
                          (Helper.IsProviderOfMMISProviderType(providerType, CON.MMISProviderType.Converted_Inactive_ProviderType)) ||
                          (Helper.IsProviderOfMMISProviderType(providerType, CON.MMISProviderType.Licensee)) ||
                          (Helper.IsProviderOfMMISProviderType(providerType, CON.MMISProviderType.Operator)) ||
                          (Helper.IsProviderOfMMISProviderType(providerType, CON.MMISProviderType.Unpaid_Support_Broker)) ||
                          (Helper.IsProviderOfMMISProviderType(providerType, CON.MMISProviderType.Supported_Living)))
                    {
                        lnkContractMaintenance.Visible = false;
                    }
                    else
                        lnkContractMaintenance.Visible = true;
                }
            }
        }
    }

    protected void ddlApplicationType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //        ds = psc.GetProviderCategories(true);
        //Helper.LoadList(ddlCategory, ds.Tables[0], "PROVIDER_CATEGORY_TYPE_NAME", "PROVIDER_CATEGORY_TYPE_ID", true);
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        int ddlValue;
        if (int.TryParse(ddlApplicationType.SelectedValue, out ddlValue))
        {
            var ds = psc.GetProviderCategoriesByApplication(ddlValue, 0);
            if (Helper.HasRows(ds))
            {
                Helper.LoadList(ddlCategory, ds.Tables[0], "PROVIDER_CATEGORY_TYPE_NAME", "PROVIDER_CATEGORY_TYPE_ID", true);
                ddlCategory.Focus();
            }
        }
        else
        {
            ddlCategory.Items.Clear();
        }
        ddlWaiverType.Items.Clear();



    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!string.IsNullOrEmpty(ddlCategory.SelectedValue))
        {
            if (Convert.ToInt32(ddlApplicationType.SelectedValue) == CON.ApplicationType.Waiver)
            {
                ddlWaiverType.Focus();
                DataSet ds = psc.GetWaiverTypes();
                Helper.LoadList(ddlWaiverType, ds.Tables[0], "WAIVER_TYPE_NAME", "WAIVER_TYPE_ID", true);
            }
            else
            {
                ddlWaiverType.Items.Clear();
                DataSet ds = psc.GetProviderTypesByTypeId(Convert.ToInt32(ddlApplicationType.SelectedValue), Convert.ToInt32(ddlCategory.SelectedValue), Helper.GetUserRole(HttpContext.Current.User.Identity.Name));
                if (Helper.HasRows(ds))
                {
                    ds.Tables[0].Columns.Add("IdWithName", typeof(string), "MMIS_PROVIDER_TYPE_ID + ' - ' + PROVIDER_TYPE_NAME");

                    Helper.LoadDropDown(ddlProviderType, ds.Tables[0], "IdWithName", "PROVIDER_TYPE_ID", true);
                    ddlProviderType.Focus();

                }
                else
                {
                    ddlProviderType.Items.Clear();
                }
            }

        }
        else
        {
            ddlWaiverType.Items.Clear();
            ddlProviderType.Items.Clear();
        }
    }
    protected void ddlWaiver_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlWaiverType.SelectedValue))
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            int appTypeValue;
            int catTypeValue;
            int.TryParse(ddlApplicationType.SelectedValue, out appTypeValue);
            int.TryParse(ddlCategory.SelectedValue, out catTypeValue);
            DataSet ds = psc.GetProviderTypesByTypeId(appTypeValue, catTypeValue, Helper.GetUserRole(HttpContext.Current.User.Identity.Name));
            if (Helper.HasRows(ds))
            {
                var data = from dT in ds.Tables[0].AsEnumerable()
                           where dT.Field<int>("WAIVER_TYPE_ID") == Convert.ToInt32(ddlWaiverType.SelectedValue)
                           select dT;
                DataTable dt = data.CopyToDataTable();
                if (Helper.HasRows(dt))
                {
                    dt.Columns.Add("IdWithName", typeof(string), "MMIS_PROVIDER_TYPE_ID + ' - ' + PROVIDER_TYPE_NAME");
                    Helper.LoadDropDown(ddlProviderType, dt, "IdWithName", "PROVIDER_TYPE_ID", true);
                    ddlProviderType.Focus();
                }
            }

        }
    }

    protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlProgram.SelectedValue) && !ddlProgram.SelectedValue.Equals("5"))
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectProviderArea(ddlProgram.SelectedValue);
            if (Helper.HasRows(ds))
            {
                Helper.LoadDropDown(ddlArea, ds.Tables[0], "COUNTY_REGION", "AREA_NAME", true);
                ddlArea.Enabled = true;
                ddlArea.Focus();
            }
            else
            {
                ddlArea.Items.Clear();
            }
        }
        else
        {
            ddlArea.Enabled = false;
        }
    }

    private void SetupButtonDisablesOfMultiClick()
    {
        btnSaveCredEvent.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSaveCredEvent, null) + ";");
        btnStartSiteVisitEvent.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnStartSiteVisitEvent, null) + ";"); //SAM538
        btnContinue.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnContinue, null) + ";");
        btnSaveNotProcessed.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSaveNotProcessed, null) + ";");
        btnReconSave.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnReconSave, null) + ";");
        btnSaveRevertSuspend.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSaveRevertSuspend, null) + ";");
    }

    private bool IsValidExtension(out string errMsg, string fileName)
    {
        bool rtn = false;
        errMsg = string.Empty;

        //  string fileName = fileClosureNoticeUploadFile.PostedFile.FileName;

        // extarct and store the file extension into another variable
        string fileExtension = System.IO.Path.GetExtension(fileName).Replace(".", string.Empty).ToLower();

        // string type array having list of allowed file type extensions
        string[] validFileExtensions = _ValidFileExtensions.Split(',');
        // loop over the array of valid file extensions to compare them with uploaded file
        foreach (string extension in validFileExtensions)
        {
            if (fileExtension == extension)
            {
                rtn = true;
                break;
            }
        }

        // display the message based on the flag value
        if (!rtn)
        {
            errMsg = "Files with extension <b>\"" + fileExtension + "\"</b> are not allowed.<br />";
            errMsg += "You can upload files with the following extensions only:";
            foreach (string str in validFileExtensions)
            {
                errMsg += " ." + str + ",";
            }
            errMsg = errMsg.Substring(0, errMsg.Length - 1);            // Remove "," at end
        }
        return rtn;
    }
    private bool IsSpecialCharacter(string strFileName)
    {
        string pattern = Helper.GetAppSettingFromDB("RegexPatternForSpecialCharacter", string.Empty);
        if (string.IsNullOrEmpty(pattern)) return true;                 // TRUE if the configuration setting does not exist
        Regex objAlphaPattern = new Regex(pattern);
        return objAlphaPattern.IsMatch(strFileName);
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "ReconsiderationProvider";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    private bool ValidateReconsideration()
    {
        bool rtn = true;
        DateTime temp;
        if (string.IsNullOrEmpty(txtDateofReconsidertaionRequest.Text.Trim()))
        {
            AddError("* Reconsideration Effective Date is required.", ref rtn);
            return rtn;
        }
        if (!DateTime.TryParse(txtDateofReconsidertaionRequest.Text, out temp))
        {
            AddError("* A valid Reconsideration Effective Date is required (mm/dd/yyyy).", ref rtn);
            return rtn;
        }
        if (encRequestReconsiderationDoc.PostedFile == null)
        {
            AddError("* Unable to find posted file.", ref rtn);
            return rtn;
        }
        if (string.IsNullOrEmpty(encRequestReconsiderationDoc.PostedFile.FileName))
        {
            AddError("* Select a file for upload", ref rtn);
            return rtn;
        }
        if (string.IsNullOrEmpty(_DestinationPath))
        {
            AddError("* ERROR - DestinationPath must have a value", ref rtn);
            return rtn;
        }
        if (string.IsNullOrEmpty(_ValidFileExtensions))
        {
            AddError("* ERROR - ValidFileExtensions must have a value", ref rtn);
            return rtn;
        }
        string errMsg = string.Empty;
        if (!IsValidExtension(out errMsg, encRequestReconsiderationDoc.PostedFile.FileName))
        {
            AddError("* Unable to find posted file.", ref rtn);
            return rtn;
        }
        if (!encRequestReconsiderationDoc.HasFile)
        {
            AddError("* No File has been uploaded.", ref rtn);
            return rtn;
        }
        if (encRequestReconsiderationDoc.PostedFile.ContentLength > (MaxFileMegaBytes * (1000 * 1024)))
        {
            AddError("File cannot be more than " + String.Format("{0:0,0}", (MaxFileMegaBytes * (1000 * 1024))) + " bytes (" + String.Format("{0:f}", MaxFileMegaBytes) + " MB) in size.", ref rtn);
            return rtn;
        }

        if (!IsSpecialCharacter(encRequestReconsiderationDoc.FileName))
        {
            AddError("The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): " + Helper.HtmlEncode(encRequestReconsiderationDoc.FileName) + " Please remove the Special Character from the file Name before upload. ", ref rtn);
            return rtn;
        }
        if (!string.IsNullOrEmpty(errMsg))
        {
            AddError("* " + errMsg, ref rtn);
            return rtn;
        }
        return rtn;
    }


    protected void btnContinue_Click(object sender, EventArgs e)
    {
        lblNPIReactivate.Text = string.Empty;
        //Called from express administration pop up
        bool isCPC_CMCOption = false;
        //take them to the appropriate view
        switch (this.ddlAdminActions.SelectedValue)
        {

            case CON.AdminMaintenanceActions.InitiateCHOP:
                InitiateRiskAlertCHOP(SelectedRegID);

                ProviderFeedHelper.InsertProviderFeedNotes(SelectedRegID, 0, HttpContext.Current.User.Identity.Name,
                    CON.AdminMaintenanceActions.InitiateCHOP,
                    enrollmentType: CON.AdminMaintenanceActions.InitiateCHOP);
                break;
            case CON.AdminMaintenanceActions.InitiateClosure:
                InitiateRiskAlertClosure(SelectedRegID);

                ProviderFeedHelper.InsertProviderFeedNotes(SelectedRegID, 0, HttpContext.Current.User.Identity.Name,
                    CON.AdminMaintenanceActions.InitiateClosure,
                   enrollmentType: CON.AdminMaintenanceActions.InitiateClosure, finalDisposition: CON.FinalDisposition.Closure);
                break;
            case CON.AdminMaintenanceActions.DisEnroll:
                this.mltExpressAdmin.ActiveViewIndex = 1;
                this.ucDisenrollmentView.IsSuspend = false;
                lblpopTitle.Text = "Provider Disenrollment";
                this.ucDisenrollmentView.InitView(SelectedRegID);
                break;
            case CON.AdminMaintenanceActions.RetroEffectiveDate:
                this.mltExpressAdmin.ActiveViewIndex = 2;
                this.ucRetroView.InitView(SelectedRegID);
                break;
            case CON.AdminMaintenanceActions.Terminate:
                this.mltExpressAdmin.ActiveViewIndex = 3;
                this.ucTermView.InitView(SelectedRegID);
                break;
            case CON.AdminMaintenanceActions.Suspend:
                this.mltExpressAdmin.ActiveViewIndex = 1;
                this.ucDisenrollmentView.IsSuspend = true;
                lblpopTitle.Text = "Suspend Provider";
                this.ucDisenrollmentView.InitView(SelectedRegID);
                break;
            case CON.AdminMaintenanceActions.Reactivate:

                if (!string.IsNullOrEmpty(hdnNPI.Value) && Convert.ToBoolean(hdnNPI.Value) == true)
                {
                    lblNPIReactivate.Text = "This NPI is already associated with an Active or Pending Provider. If a new application is required because you are changing Provider Types, please disenroll the current provider that is associated with the NPI. ";
                    //isValid = false;
                }
                else
                {
                    this.mltExpressAdmin.ActiveViewIndex = 5;
                    this.ucReactivateView.InitView(SelectedRegID);
                }
                break;

            case CON.AdminMaintenanceActions.EditKeyIdentifiers:
                PDMSService.PDMSServiceClient psc1 = new PDMSService.PDMSServiceClient();
                DataSet ds1 = psc1.SelectRegistrationByRegID(SelectedRegID);
                ProviderManagerData keyData1 = EditKeyIdentifierForRegistrations(ds1);

                if (Helper.GetInt("RegistrationProgramStatusTypeID", ds1.Tables[0].Rows[0]) != CON.RegistrationProgramStatusTypeId.Conversion)
                {
                    keyData1.ConvertedProvider = false;
                }
                else
                {
                    if (Helper.GetInt("CurrentTaskID", ds1.Tables[0].Rows[0]) > 0)
                    {
                        keyData1.ConvertedProvider = false;
                    }
                    else
                    {
                        keyData1.ConvertedProvider = true;
                    }
                }
                ProviderFeedHelper.InsertProviderFeedNotes(SelectedRegID, 0, HttpContext.Current.User.Identity.Name,
                    CON.AdminMaintenanceActions.EditKeyIdentifiers,
                     enrollmentType: CON.AdminMaintenanceActions.EditKeyIdentifiers);

                //this.mltExpressAdmin.SetActiveView(vwEditKeyIdentifier);
                Response.Redirect("~/Process/NewProvider.aspx?AdminEditKeyFieldDataRequest=true&RegID=" + keyData1.RegID.ToString());
                //this.ucEditKeyIdentifier.InitView(keyData1);
                break;
            case CON.AdminMaintenanceActions.CredentialingEvent:
                this.mltExpressAdmin.SetActiveView(vwCredentialEvent);

                ProviderFeedHelper.InsertProviderFeedNotes(SelectedRegID, 0, HttpContext.Current.User.Identity.Name,
                    CON.AdminMaintenanceActions.CredentialingEvent,
                     enrollmentType: CON.AdminMaintenanceActions.CredentialingEvent);
                break;
            case CON.AdminMaintenanceActions.NotProcessed:
                this.mltExpressAdmin.SetActiveView(vwNotProcessed);
                break;
            case CON.AdminMaintenanceActions.ProviderReconsideration:
                this.mltExpressAdmin.SetActiveView(vwProviderReconsideration);
                break;
            case CON.AdminMaintenanceActions.InitiateHearingRights:
                InitiateHearingRights(SelectedRegID);

                ProviderFeedHelper.InsertProviderFeedNotes(SelectedRegID, 0, HttpContext.Current.User.Identity.Name,
                    CON.AdminMaintenanceActions.InitiateHearingRights,
                     enrollmentType: CON.AdminMaintenanceActions.InitiateHearingRights);

                break;
            case CON.AdminMaintenanceActions.EnableCPClinks:
                EnableCPCAttestationLinks(SelectedRegID);
                isCPC_CMCOption = true;

                ProviderFeedHelper.InsertProviderFeedNotes(SelectedRegID, 0, HttpContext.Current.User.Identity.Name,
                    CON.AdminMaintenanceActions.EnableCPClinks,
                     enrollmentType: CON.AdminMaintenanceActions.EnableCPClinks);
                break;
            case CON.AdminMaintenanceActions.EnableCMCLinks:
                EnableCMCAttestationLinks(SelectedRegID);
                isCPC_CMCOption = true;

                ProviderFeedHelper.InsertProviderFeedNotes(SelectedRegID, 0, HttpContext.Current.User.Identity.Name,
                    CON.AdminMaintenanceActions.EnableCMCLinks,
                     enrollmentType: CON.AdminMaintenanceActions.EnableCMCLinks);
                break;
            //SAM758 Express Admin function to remove an exclusion
            case CON.AdminMaintenanceActions.RemoveExclusion:
                this.mltExpressAdmin.SetActiveView(vwRemoveExclusion);
                break;
            case CON.AdminMaintenanceActions.NewTerminationDate:
                this.mltExpressAdmin.ActiveViewIndex = 10;
                this.ucNewTermDt.InitView(SelectedRegID);
                break;
            case CON.AdminMaintenanceActions.CredentialReconsideration:
                InitiateCredentialReconsiderationWorkflow(SelectedRegID);

                ProviderFeedHelper.InsertProviderFeedNotes(SelectedRegID, 0, HttpContext.Current.User.Identity.Name,
                    CON.AdminMaintenanceActions.CredentialReconsideration,
                     enrollmentType: CON.AdminMaintenanceActions.CredentialReconsideration);
                break;

            case CON.AdminMaintenanceActions.AdminRevertSuspension:
                this.mltExpressAdmin.SetActiveView(vwRevertSuspend);
                break;
            case CON.AdminMaintenanceActions.SiteVisitEvent:
                this.mltExpressAdmin.SetActiveView(vwSiteVisit);
                break;
            default:
                break;
        }

        if (!isCPC_CMCOption)
            this.mpeMaint.Show();

    }

    private void EnableCPCAttestationLinks(int selectedRegID)
    {
        try
        {
            using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", selectedRegID.ToString());
                parms.Add("IS_CPC_LINK_REENABLED", "true");
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                svc.UpdateRegistration(parms);
                this.mpeMaint.Hide();

            }
        }
        catch (Exception ex)
        {

        }
    }

    private void EnableCMCAttestationLinks(int selectedRegID)
    {
        using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", selectedRegID.ToString());
            parms.Add("Enable_CMC_Link", "true");
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("ENABLED_CMC_LINK_DATE", DateTime.Now.ToString());
            svc.UpdateCMCLinksVisibility(parms);
            this.mpeMaint.Hide();
        }

    }

    private void InitiateRiskAlertClosure(int selectedRegID)
    {
        try
        {
            int entryTaskID = Convert.ToInt32(Helper.GetAppSettingFromDB("ODMClosureEntryTaskID", "0"));
            Workflow.Process pr = new Workflow.Process(CON.WorkflowType.RiskAlertClosure, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Guid.NewGuid(), entryTaskID);
            if (pr == null)
            {
                lblErrmsg.Visible = true;
                lblErrmsg.Text = "Initiation of risk alert closure not successful";
                return;
            }

            int ProcessID = pr.ProcessID;
            int CurrentStepID = pr.CurrentStepID;
            int CurrentTaskID = pr.TaskID;
            int WorkflowID = pr.WorkflowID;

            if (ProcessID > 0 && WorkflowID > 0)
            {
                // Save the Registration ID as a process parameter of the Workflow
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                svc.WF_SaveProcessParameter(ProcessID, "REGISTRATION_ID", SelectedRegID.ToString());
                svc.WF_TakeAction(ProcessID, "Submit For Review", string.Empty);
                (this.Page as RegistrationProvider).RegistrationId = selectedRegID;
                (this.Page as RegistrationProvider).IsReadOnly = false;
                (this.Page as RegistrationProvider).RegistrationStep = CON.SectionTypeID.ClosureNotice;
                Response.Redirect("~/process/Registration.aspx", true);
            }

        }
        catch (Exception ex)
        {

        }
    }
    private void InitiateRiskAlertCHOP(int selectedRegID)
    {
        try
        {
            Workflow.Process pr = new Workflow.Process(CON.WorkflowType.RiskAlertCHOP, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Guid.NewGuid());
            if (pr == null)
            {
                lblErrmsg.Visible = true;
                lblErrmsg.Text = "Initiation of risk alert CHOP not successful";
                return;
            }

            int ProcessID = pr.ProcessID;
            int CurrentStepID = pr.CurrentStepID;
            int CurrentTaskID = pr.TaskID;
            int WorkflowID = pr.WorkflowID;

            if (ProcessID > 0 && WorkflowID > 0)
            {
                // Save the Registration ID as a process parameter of the Workflow
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                svc.WF_SaveProcessParameter(ProcessID, "REGISTRATION_ID", SelectedRegID.ToString());
                svc.WF_TakeAction(ProcessID, "Submit", string.Empty);
                (this.Page as RegistrationProvider).RegistrationId = selectedRegID;
                (this.Page as RegistrationProvider).IsReadOnly = false;
                (this.Page as RegistrationProvider).RegistrationStep = CON.SectionTypeID.DaysNotice;
                Response.Redirect("~/process/Registration.aspx", true);
            }

        }
        catch (Exception ex)
        {

        }
    }
    private void InitiateCredentialReconsiderationWorkflow(int regID)
    {
        bool rtn = true;
        try
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

            //OHPNM-14061 - take a version copy before starting the recon workflow
            DataSet ds = svc.InsertIntoVersionTables(regID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            DataRow dr;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dr = ds.Tables[0].Rows[0];
                if (!string.IsNullOrEmpty(Methods.GetStringValue(dr["ErrorMessage"])))
                {
                    Logging log = new Logging(Guid.NewGuid(), string.Empty);
                    log.CreateLogEntry(string.Format("{0} {1}", "Error Setting up Credentailing Reconsideration Workflow", Methods.GetStringValue(dr["ErrorMessage"])), Logging.LogPriority.Error);
                    AddError("* Initiation of Reconsideration WF not successful. Error: ", ref rtn);
                    return;
                }
            }

            int entryTaskID = CON.CredentialReconsiderationEntryTaskID;
            Workflow.Process pr = new Workflow.Process(CON.WorkflowType.RegistrationNew, null, Guid.NewGuid(), entryTaskID);
            if (pr == null)
            {
                lblErrmsg.Visible = true;
                AddError("* Initiation of Credentailing Reconsideration WF not successful. Error: ", ref rtn);
                return;
            }

            int ProcessID = pr.ProcessID;
            int CurrentStepID = pr.CurrentStepID;
            int CurrentTaskID = pr.TaskID;
            int WorkflowID = pr.WorkflowID;

            if (ProcessID > 0 && WorkflowID > 0)
            {
                // Save the Registration ID as a process parameter of the Workflow

                svc.InsertRegApplicationRecord(regID, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId), pr.ProcessID);

                svc.WF_SaveProcessParameter(ProcessID, "REGISTRATION_ID", SelectedRegID.ToString());
                svc.WF_SaveProcessParameter(ProcessID, CON.ProcessParameter.WorkflowEventTypeID, CON.WorkflowEventType.CredentialReconsideration.ToString());

                svc.updateWF_STEP_Owner(CurrentStepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                svc.UpdateRegistrationCustom(new Dictionary<string, string>() { { "REG_ID", regID.ToString() }, { "WORKFLOW_EVENT_TYPE_ID", CON.WorkflowEventType.CredentialReconsideration.ToString() }, { "WAIVER_SERVICE_UPDATE_TYPE_ID", null } });

                AddError("We have received your Credentailing reconsideration request.", ref rtn);
                string url = "~/Process/Registration.aspx?Step=" + CON.SectionTypeID.ProviderCredentialing.ToString() + "&RegId=" + SelectedRegID.ToString();
                Response.Redirect(url, false);
            }

        }
        catch (Exception ex)
        {
            AddError("* Initiation of Credentaial Reconsideration WF not successful. Error: " + ex.Message, ref rtn);
        }
    }

    private void InitiateHearingRights(int selectedRegID)
    {
        try
        {
            Workflow.Process pr = new Workflow.Process(CON.WorkflowType.HearingRights, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Guid.NewGuid());
            if (pr == null)
            {
                lblErrmsg.Visible = true;
                lblErrmsg.Text = "Initiation of Hearing Rights WF not successful";
                return;
            }

            int ProcessID = pr.ProcessID;
            int CurrentStepID = pr.CurrentStepID;
            int CurrentTaskID = pr.TaskID;
            int WorkflowID = pr.WorkflowID;

            if (ProcessID > 0 && WorkflowID > 0)
            {
                // Save the Registration ID as a process parameter of the Workflow
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                svc.WF_SaveProcessParameter(ProcessID, "REGISTRATION_ID", SelectedRegID.ToString());
                (this.Page as RegistrationProvider).RegistrationId = selectedRegID;
                (this.Page as RegistrationProvider).IsReadOnly = false;
                (this.Page as RegistrationProvider).RegistrationStep = CON.SectionTypeID.HearingRights;
                Response.Redirect("~/process/Registration.aspx", true);
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.mpeMaint.Hide();
        gvProviders.SelectRow(-1);
        RefreshData();
    }

    protected void lnkPDF_Click(object sender, EventArgs e)
    {
        int totalResultCount;

        DataTable dt = GetData(out totalResultCount, 10000);
        RadGridExport.DataSource = dt;
        RadGridExport.DataBind();
        RadGridExport.MasterTableView.ExportToPdf();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        int totalResultCount;
        _IsExcel = true;
        DataTable dt = GetData(out totalResultCount, 10000);
        RadGridExport.DataSource = dt;
        RadGridExport.DataBind();
        RadGridExport.MasterTableView.ExportToExcel();

    }

    protected void ddlProviderType_SelectedIndexChanged(object sender, EventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        int ddlValue;
        if (int.TryParse(ddlProviderType.SelectedValue, out ddlValue))
        {
            DataSet ds = psc.SelectAllSpecialtiesByProviderType(ddlValue);
            ds.Tables[0].Columns.Add("IdWithName", typeof(string), "MMIS_SPECIALTY_TYPE_ID + ' - ' + SPECIALTY_TYPE_NAME");
            Helper.LoadList(ddlSpecialty, ds.Tables[0], "IdWithName", "SPECIALTY_TYPE_ID", true);
        }
        else
        {
            ddlSpecialty.Items.Clear();
        }

    }

    private void View_NewRegistrationInsert(ProviderManagementData data)
    {
        //Open registration with this regID
        (this.Page as RegistrationProvider).RegistrationId = data.RegID;
        (this.Page as RegistrationProvider).IsReadOnly = false;
    }

    private void View_CancelNewRegistrationRequest()
    {
        this.mpeMaint.Hide();
    }

    private void View_ValidationEventRequest()
    {
        View_KeepPopupOpen();
    }

    private void View_ResetSelectedRecord()
    {
        this.mpeMaint.Hide();
    }
    #endregion

    #region Public Methods
    public void ResetSelection()
    {
        btnClear_Click(new object(), new EventArgs());
    }

    public void RefreshData()
    {
        int totalResultCount = 0;
        DataTable dt = GetData(out totalResultCount, gvProviders.PageSize);
        hdnRowCount.Value = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(totalResultCount.ToString(), true);
        gvProviders.DataSource = dt;
        gvProviders.VirtualItemCount = totalResultCount;
        gvProviders.DataBind();
        gvProviders.PageIndexCount = gvProviders.PageCount;
    }
    #endregion

    #region Private Methods
    private DataRow GetDataRow(DataTable dt, string providerID)
    {
        DataRow rtn = null;
        foreach (DataRow row in dt.Rows)
        {
            if (row["ProviderId"].ToString() == providerID)
            {
                rtn = row;
                break;
            }
        }

        return rtn;
    }

    private void BindDropDown()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_PROGRAM_STATUS_TYPE", parms);
        var tableRPT = ds.Tables[0].AsEnumerable()
                  .Where(r => r.Field<bool>("IsUsedProvSearch") != false)
                  .CopyToDataTable();
        if (Helper.HasRows(ds)) Helper.LoadList(ddlTennCareStatus, tableRPT, "REG_PROGRAM_STATUS_TYPE_INTERNAL", "REG_PROGRAM_STATUS_TYPE_ID", true);

        ds = psc.SelectRegistrationDataWithParams("usp_WF_SelectTaskNames", parms);
        if (Helper.HasRows(ds)) Helper.LoadList(ddlPDMSStatus, ds.Tables[0], "TASK_NAME", "TASK_NAME", true);

        ds = psc.GetApplicationTypes();
        var tableAT = ds.Tables[0].AsEnumerable()
          .Where(r => r.Field<bool>("IsUsedProvSearch") != false)
          .CopyToDataTable();
        Helper.LoadList(ddlApplicationType, tableAT, "APPLICATION_TYPE_NAME", "APPLICATION_TYPE_ID", true);

        ds = psc.SelectProviderRiskLevels();
        Helper.LoadList(ddlRiskLevel, ds.Tables[0], "PROVIDER_RISK_LEVEL_NAME", "PROVIDER_RISK_LEVEL_ID", true);

        string typeBorR = "B";
        ds = psc.SelectProviderContractType(typeBorR);
        ds.Tables[0].DefaultView.Sort = "CONTRACT_TYPE_NAME";
        //Helper.LoadList(ddlContractType, ds.Tables[0], "CONTRACT_TYPE_NAME", "CONTRACT_TYPE_ID", true);


        ds = psc.SelectMCP();
        Helper.LoadList(ddlMCP, ds.Tables[0], "DSC_MCP", "MCP_ID", true);

        ds = psc.SelectProgramType();
        Helper.LoadList(ddlProgram, ds.Tables[0], "PROGRAM_CODE_NAME", "PROGRAM_CODE_ID", true);

        ds = psc.SelectEnrollmentStatusReasons();
        ds.Tables[0].DefaultView.Sort = "ENROLLMENT_STATUS_REASONS_DESC";
        Helper.LoadList(ddlEnrollmentStatusReason, ds.Tables[0], "ENROLLMENT_STATUS_REASONS_DESC", "ENROLLMENT_STATUS_REASONS_ID", true);
        ds = psc.SelectProviderContractStatus();
        //Helper.LoadList(ddlContractStatus, ds.Tables[0], "CONTRACT_STATUS_NAME", "PROVIDER_CONTRACT_STATUS_ID", true);

        ds = psc.SelectAllTaxonomyCodes();
        Helper.LoadList(ddlTaxonomy, ds.Tables[0], "TAXONOMY_CODE", "TAXONOMY_CODE", true);

        ds = psc.SelectAllApplicationStatus("pcw");
        Helper.LoadList(ddlODARegistrationStatus, ds.Tables[0], "APPLICATION_STATUS_DESC", "APPLICATION_STATUS_ID", true);

        ds = psc.SelectAllApplicationStatus("psm");
        ds.Tables[0].DefaultView.Sort = "APPLICATION_STATUS_DESC";
        Helper.LoadList(ddlDODDRegistrationStatus, ds.Tables[0], "APPLICATION_STATUS_DESC", "APPLICATION_STATUS_ID", true);

        ds = psc.SelectAllApplicationStatus("");
        Helper.LoadList(ddlPNMAppStatus, ds.Tables[0], "APPLICATION_STATUS_DESC", "APPLICATION_STATUS_ID", true);

        ds = psc.SelectAllRegistrationStatus();
        Helper.LoadList(ddlPNMEnrollStatus, ds.Tables[0], "REGISTRATION_STATUS_TYPE", "REGISTRATION_STATUS_TYPE_ID", true);

        ListItem li = new ListItem("", "");
        ddlAssigned.Items.Add(li);
        li = new ListItem("Yes", "1");
        ddlAssigned.Items.Add(li);
        li = new ListItem("No", "0");
        ddlAssigned.Items.Add(li);

    }

    private void SetSearchEntries()
    {
        // Set the search TextBoxes and Dropdowns so "Enter" key will fire the search
        foreach (Control ctl in gbSearch.Controls)
        {
            if (ctl.GetType() == typeof(TextBox))
            {
                TextBox txt = (TextBox)ctl;
                txt.Attributes.Add("onKeyPress", "doClick('" + btnSearch.ClientID + "',event)");
            }
            else if (ctl.GetType() == typeof(eWorld.UI.NumericBox))
            {
                eWorld.UI.NumericBox num = (eWorld.UI.NumericBox)ctl;
                num.Attributes.Add("onKeyPress", "doClick('" + btnSearch.ClientID + "',event)");
            }
            else if (ctl.GetType() == typeof(DropDownList))
            {
                DropDownList drp = (DropDownList)ctl;
                Helper.DisableBackSpace(drp);
                drp.Attributes.Add("onKeyPress", "doClick('" + btnSearch.ClientID + "',event)");
            }
        }
        txtGroupName.Focus();
    }

    private void Search(bool clearIds)
    {
        if (clearIds)
        {
            lblResultHeader.Text = string.Empty;
            pnlResultHeader.Visible = false;
        }
        this.gvProviders.CurrentPageIndex = 0;
        RefreshData();
    }

    private bool SearchIds()
    {
        if (!string.IsNullOrEmpty(SessionVarRetriever.DashBoardRegistrationIds)) return true;
        if (SessionVarRetriever.DashBoardTableId > 0) return true;
        return false;
    }

    private DataTable GetData(out int totalResultCount, int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        string sortColWithDirection = this.gvProviders.GridViewSortDirection == SortDirection.Descending ? gvProviders.GridViewSortColumn + " DESC" : gvProviders.GridViewSortColumn;
        totalResultCount = 0;
        Dictionary<string, object> parms = new Dictionary<string, object>();
        parms.Add("GroupName", txtGroupName.Text.Trim());
        parms.Add("BaseMedicaidID", txtMedicaidID.Text.Trim());
        parms.Add("NPI", txtNPI.Text.Trim());
        parms.Add("TaxID", txtTaxID.Text.Trim());
        parms.Add("PDMSStatus", ddlPDMSStatus.SelectedValue);
        parms.Add("PDMSStatusDate", txtPDMSStatusDate.Text);
        parms.Add("TennCareStatus", ddlTennCareStatus.SelectedValue);
        parms.Add("ProviderCategoryTypeId", ddlCategory.SelectedValue);
        parms.Add("ProviderTypeId", ddlProviderType.SelectedValue);
        parms.Add("SpecialtyId", ddlSpecialty.SelectedValue);
        parms.Add("DBAName", txtDBAName.Text);
        parms.Add("applicationTypeId", ddlApplicationType.SelectedValue);
        parms.Add("RoleName", Helper.GetUserRole(HttpContext.Current.User.Identity.Name));
        parms.Add("WaiverType", ddlWaiverType.SelectedValue);
        parms.Add("ContractType", "");
        parms.Add("MCP", ddlMCP.SelectedValue);
        parms.Add("Program", ddlProgram.SelectedValue);
        parms.Add("DateReceived", txtDateReceived.Text);
        parms.Add("Taxonomy", ddlTaxonomy.SelectedValue);
        parms.Add("County", txtCounty.Text);
        parms.Add("City", txtCity.Text);
        parms.Add("Area", ddlArea.SelectedValue);
        parms.Add("ContractStatus", "");
        parms.Add("EnrollmentStatusReason", ddlEnrollmentStatusReason.SelectedValue);
        parms.Add("RegID", txtRegID.Text);
        parms.Add("MedicareNumber", txtMedicareNumber.Text);
        parms.Add("DODDContractNumber", txtDODDContractNumber.Text);
        parms.Add("ODARegistrationStatus", ddlODARegistrationStatus.SelectedValue);
        parms.Add("DODDRegistrationStatus", ddlDODDRegistrationStatus.SelectedValue);
        parms.Add("PNMEnrollmentStatus", ddlPNMEnrollStatus.SelectedValue);
        parms.Add("PNMApplicationStatus", ddlPNMAppStatus.SelectedValue);
        parms.Add("SortExpression", sortColWithDirection);
        parms.Add("PageSize", pageSize);
        int pageIndex = 0;
        if (_IsExcel)
            pageIndex = 0;
        else
            pageIndex = gvProviders.CurrentRowIndex;

        parms.Add("StartRowIndex", pageIndex);
        parms.Add("GetTotalResultCount", true);

        // If list of IDs has been passed in, display in search results list.
        if (SearchIds())
        {
            ds = psc.SearchGroupProviderWithIds(SessionVarRetriever.DashBoardRegistrationIds, SessionVarRetriever.DashBoardTableId,
                SessionVarRetriever.DashBoardStatusID, SessionVarRetriever.DashBoardOrdinal, SessionVarRetriever.DashBoardIsAssigned,
                sortColWithDirection, pageSize, pageIndex, true, out totalResultCount);
        }
        else
        {
            int riskLevelID = ddlRiskLevel.SelectedIndex == -1 || string.IsNullOrWhiteSpace(ddlRiskLevel.SelectedValue) ? 0 : Convert.ToInt32(ddlRiskLevel.SelectedValue);
            if (Helper.IsLoggedInUserInAdminRole() && ddlAssigned.Items != null && ddlAssignedUser.Items != null)
            {
                string assigned = ddlAssigned.SelectedIndex > 0 ? ddlAssigned.SelectedValue : string.Empty;
                string assignedUser = ddlAssignedUser.SelectedIndex > 0 ? ddlAssignedUser.SelectedValue : string.Empty;

                parms.Add("RiskLevelId", riskLevelID);
                parms.Add("IsUserAssigned", assigned);
                parms.Add("userName", assignedUser);

                ds = psc.SearchGroupProviders(parms, out totalResultCount);
            }
            else
            {
                parms.Add("RiskLevelId", riskLevelID);
                parms.Add("IsUserAssigned", "");
                parms.Add("userName", "");

                ds = psc.SearchGroupProviders(parms, out totalResultCount);
            }
        }

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    private void RemoveDuplicateItems(DropDownList ddl)
    {
        for (int i = 0; i < ddl.Items.Count; i++)
        {
            ddl.SelectedIndex = i;
            string str = ddl.SelectedItem.ToString();
            for (int counter = i + 1; counter < ddl.Items.Count; counter++)
            {
                ddl.SelectedIndex = counter;
                string compareStr = ddl.SelectedItem.ToString();
                if (str == compareStr)
                {
                    ddl.Items.RemoveAt(counter);
                    counter = counter - 1;
                }
            }
        }
        ddl.SelectedIndex = 0;
    }

    private void ShowExpressAdminOptions(bool regOkToNPI, bool regOkToTerm, bool regOkToRetroDate, bool regOkToReactivate, bool regOkToDisenroll, bool regOKToCredential, bool regOkToKey, bool regOkToReconsider, bool regOkToSuspend, bool regOkLTCChop, bool regOkHearingRights, bool regOkNotProcessed, bool regOKChangeTermDt, bool regOkToCredRecon, bool regOkToRevertSuspend, bool regOkToSiteVistEvent)
    {
        LoadExpressAdminActions(regOkToNPI, regOkToTerm, regOkToRetroDate, regOkToReactivate, regOkToDisenroll, regOKToCredential, regOkToKey, regOkToReconsider, regOkToSuspend, regOkLTCChop, regOkHearingRights, regOkNotProcessed, regOKChangeTermDt, regOkToCredRecon, regOkToRevertSuspend, regOkToSiteVistEvent);
        RemoveDuplicateItems(this.ddlAdminActions);
        this.mltExpressAdmin.ActiveViewIndex = 0;
        this.mpeMaint.Show();
    }

    private void LoadExpressAdminActions(bool regOkToNPI, bool regOkToTerm, bool regOkToRetroDate, bool regOkToReactivate, bool regOkToDisenroll, bool regOKToCredential, bool regOkToKey, bool regOkToReconsider, bool regOkToSuspend, bool regOkLTCChop, bool regOkHearingRights, bool regOkNotProcessed, bool regOKChangeTermDt, bool regOkToCredRecon, bool regOkToRevertSuspend, bool regOkToSiteVistEvent)
    {
        if (regOkToNPI)
        {
            hdnNPI.Value = regOkToNPI.ToString();
        }
        else { hdnNPI.Value = string.Empty; }
        this.ddlAdminActions.Items.Clear();

        ListItem item = new ListItem(string.Empty, string.Empty);
        this.ddlAdminActions.Items.Add(item);
        bool canShowCPCEnableLinks = false;
        bool canShowCMCEnableLinks = false;
        bool enableCR532 = AppSettings.Get("EnableCR532").Equals("true") ? true : false;

        if (Helper.IsLoggedInUserInAdminRole())
        {
            if (regOkToRetroDate)
            {
                item = new ListItem(CON.AdminMaintenanceActions.RetroEffectiveDate, CON.AdminMaintenanceActions.RetroEffectiveDate);
                this.ddlAdminActions.Items.Add(item);
            }
            if (regOkToDisenroll)
            {
                item = new ListItem(CON.AdminMaintenanceActions.DisEnroll, CON.AdminMaintenanceActions.DisEnroll);
                this.ddlAdminActions.Items.Add(item);
            }
            if (regOkToTerm)
            {
                item = new ListItem(CON.AdminMaintenanceActions.Terminate, CON.AdminMaintenanceActions.Terminate);
                this.ddlAdminActions.Items.Add(item);
            }
            if (regOkToSuspend)
            {
                item = new ListItem(CON.AdminMaintenanceActions.Suspend, CON.AdminMaintenanceActions.Suspend);
                this.ddlAdminActions.Items.Add(item);
            }
            if (regOkToReactivate)
            {
                item = new ListItem(CON.AdminMaintenanceActions.Reactivate, CON.AdminMaintenanceActions.Reactivate);
                this.ddlAdminActions.Items.Add(item);
            }
            if (regOKToCredential)
            {
                DataSet ds = GetCredentialingProviderType();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    item = new ListItem(CON.AdminMaintenanceActions.CredentialingEvent, CON.AdminMaintenanceActions.CredentialingEvent);
                    this.ddlAdminActions.Items.Add(item);
                }
            }
            if (regOkToKey)
            {
                item = new ListItem(CON.AdminMaintenanceActions.EditKeyIdentifiers, CON.AdminMaintenanceActions.EditKeyIdentifiers);
                this.ddlAdminActions.Items.Add(item);
            }

            // OHPNM-4851
            if (regOkToReconsider)
            {
                item = new ListItem(CON.AdminMaintenanceActions.ProviderReconsideration, CON.AdminMaintenanceActions.ProviderReconsideration);
                this.ddlAdminActions.Items.Add(item);
            }

            // Remove Update for now. May add later, but need to work out edit permissions for Admin users on sections and pages
            //if (regOkToUpdate)
            //{
            //    item = new ListItem(CON.AdminMaintenanceActions.Update, CON.AdminMaintenanceActions.Update);
            //    this.ddlAdminActions.Items.Add(item);
            //}
            // OHPNM-4851 - removed two links since 2.05 says "N" for ODMStateAdministrator
            // if (regOkLTCChop)// TODO:Akash
            // {
            //     item = new ListItem(CON.AdminMaintenanceActions.InitiateCHOP, CON.AdminMaintenanceActions.InitiateCHOP);
            //     this.ddlAdminActions.Items.Add(item);
            //     item = new ListItem(CON.AdminMaintenanceActions.InitiateClosure, CON.AdminMaintenanceActions.InitiateClosure);
            //     this.ddlAdminActions.Items.Add(item);
            // }
            if (regOkHearingRights)
            {
                item = new ListItem(CON.AdminMaintenanceActions.InitiateHearingRights, CON.AdminMaintenanceActions.InitiateHearingRights);
                this.ddlAdminActions.Items.Add(item);
            }

            if (regOkNotProcessed)
            {
                item = new ListItem(CON.AdminMaintenanceActions.NotProcessed, CON.AdminMaintenanceActions.NotProcessed);
                this.ddlAdminActions.Items.Add(item);
            }

            if (regOKChangeTermDt && enableCR532)
            {
                item = new ListItem(CON.AdminMaintenanceActions.NewTerminationDate, CON.AdminMaintenanceActions.NewTerminationDate);
                this.ddlAdminActions.Items.Add(item);
            }
        }

        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CommitteeQualitySpecialist) ||
            Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSupervisor))
        {
            if (regOkToCredRecon)
            {
                item = new ListItem(CON.AdminMaintenanceActions.CredentialReconsideration, CON.AdminMaintenanceActions.CredentialReconsideration);
                this.ddlAdminActions.Items.Add(item);
            }
        }

        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ComplianceSpecialist))
        {
            //SAM758 Express Admin function to remove an exclusion
            item = new ListItem(CON.AdminMaintenanceActions.RemoveExclusion, CON.AdminMaintenanceActions.RemoveExclusion);
            this.ddlAdminActions.Items.Add(item);

            if (regOkToReconsider)
            {
                item = new ListItem(CON.AdminMaintenanceActions.ProviderReconsideration, CON.AdminMaintenanceActions.ProviderReconsideration);
                this.ddlAdminActions.Items.Add(item);
            }

            if (regOkHearingRights)
            {
                item = new ListItem(CON.AdminMaintenanceActions.InitiateHearingRights, CON.AdminMaintenanceActions.InitiateHearingRights);
                this.ddlAdminActions.Items.Add(item);
            }
            if (regOkToDisenroll)
            {
                item = new ListItem(CON.AdminMaintenanceActions.DisEnroll, CON.AdminMaintenanceActions.DisEnroll);
                this.ddlAdminActions.Items.Add(item);
            }
            if (regOkToTerm)
            {
                item = new ListItem(CON.AdminMaintenanceActions.Terminate, CON.AdminMaintenanceActions.Terminate);
                this.ddlAdminActions.Items.Add(item);
            }
            if (regOkToSuspend)
            {
                item = new ListItem(CON.AdminMaintenanceActions.Suspend, CON.AdminMaintenanceActions.Suspend);
                this.ddlAdminActions.Items.Add(item);
            }
            if (regOkHearingRights)
            {
                item = new ListItem(CON.AdminMaintenanceActions.InitiateHearingRights, CON.AdminMaintenanceActions.InitiateHearingRights);
                this.ddlAdminActions.Items.Add(item);
            }
            if (regOkToRevertSuspend)
            {
                item = new ListItem(CON.AdminMaintenanceActions.AdminRevertSuspension, CON.AdminMaintenanceActions.AdminRevertSuspension);
                this.ddlAdminActions.Items.Add(item);
            }
            //SAM538 New Site Visit Event Admin action
            if (regOkToSiteVistEvent)
            {
                item = new ListItem(CON.AdminMaintenanceActions.SiteVisitEvent, CON.AdminMaintenanceActions.SiteVisitEvent);
                this.ddlAdminActions.Items.Add(item);
            }
        }
        //CHOP Closure (Initiate Closure)
        // OHPNM-4851 - removed InitiateClosure for Provider Services per 2.05
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.LTCInitialRevalidation))
        {
            if (regOkLTCChop)
            {
                PDMSService.PDMSServiceClient psc1 = new PDMSService.PDMSServiceClient();
                DataSet ds1 = psc1.SelectRegistrationByRegID(SelectedRegID);
                DataSet dsProvider = psc1.SelectRegistrationData(SelectedRegID, "PROVIDER");
                string providertype = string.Empty;
                if (Helper.HasRows(dsProvider))
                {
                    providertype = Helper.GetString("MMIS_Provider_Type_ID", dsProvider.Tables[0].Rows[0]);

                }
                if (providertype.Equals(CON.LTCProviderTypes.NursingFacility))
                {
                    item = new ListItem(CON.AdminMaintenanceActions.InitiateClosure, CON.AdminMaintenanceActions.InitiateClosure);
                    this.ddlAdminActions.Items.Add(item);
                }
            }

        }
        //CHOP Risk Alert (Initiate CHOP)
        // OHPNM-4851 - removed InitiateChop for Provider Services per 2.05
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.LTCCHOP))
        {
            if (regOkLTCChop)
            {
                PDMSService.PDMSServiceClient psc1 = new PDMSService.PDMSServiceClient();
                DataSet ds1 = psc1.SelectRegistrationByRegID(SelectedRegID);
                DataSet dsProvider = psc1.SelectRegistrationData(SelectedRegID, "PROVIDER");
                string providertype = string.Empty;

                DataSet ds = psc1.GetWFProcessByRegId(SelectedRegID);

                bool isInitiateCHOP = false;
                if (Helper.HasRows(ds))
                {
                    int currentStepID = Helper.GetInt("CURRENT_STEP_ID", ds.Tables[0].Rows[0]);
                    if (currentStepID > 0)
                    {
                        isInitiateCHOP = false;
                    }
                    else
                    {
                        isInitiateCHOP = true;
                    }

                }

                if (Helper.HasRows(dsProvider))
                {
                    providertype = Helper.GetString("MMIS_Provider_Type_ID", dsProvider.Tables[0].Rows[0]);
                }
                if ((providertype.Equals(CON.LTCProviderTypes.NursingFacility)
                    || providertype.Equals(CON.LTCProviderTypes.NONSTATE_OPERATED_ICF_MR)
                    || providertype.Equals(CON.LTCProviderTypes.STATE_OPERATED_ICF_MR)) && isInitiateCHOP)
                {
                    item = new ListItem(CON.AdminMaintenanceActions.InitiateCHOP, CON.AdminMaintenanceActions.InitiateCHOP);
                    this.ddlAdminActions.Items.Add(item);
                }
            }
        }
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.LTCCHOP))
        {
            if (regOkLTCChop)
            {
                PDMSService.PDMSServiceClient psc1 = new PDMSService.PDMSServiceClient();
                DataSet ds1 = psc1.SelectRegistrationByRegID(SelectedRegID);
                DataSet dsProvider = psc1.SelectRegistrationData(SelectedRegID, "PROVIDER");
                string providertype = string.Empty;
                if (Helper.HasRows(dsProvider))
                {
                    providertype = Helper.GetString("MMIS_Provider_Type_ID", dsProvider.Tables[0].Rows[0]);
                }
                if (providertype.Equals(CON.LTCProviderTypes.NursingFacility))
                {
                    item = new ListItem(CON.AdminMaintenanceActions.InitiateClosure, CON.AdminMaintenanceActions.InitiateClosure);
                    this.ddlAdminActions.Items.Add(item);
                }
            }
        }
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.APMSpecialist))
        {
            PDMSService.PDMSServiceClient psc1 = new PDMSService.PDMSServiceClient();
            DataSet dsReg = psc1.SelectRegistrationByRegID(SelectedRegID);
            string providertype = string.Empty;
            if (Helper.HasRows(dsReg))
            {
                providertype = Helper.GetString("MMISProviderTypeID", dsReg.Tables[0].Rows[0]);
            }

            canShowCPCEnableLinks = CanShowCPCReenableLinks(SelectedRegID);
            canShowCMCEnableLinks = CanShowCMCReenableLinks(SelectedRegID);

            if (regOkToDisenroll && ddlAdminActions.Items.FindByValue(CON.AdminMaintenanceActions.DisEnroll) == null)
            {
                item = new ListItem(CON.AdminMaintenanceActions.DisEnroll, CON.AdminMaintenanceActions.DisEnroll);
                this.ddlAdminActions.Items.Add(item);
            }

            if (canShowCMCEnableLinks && ddlAdminActions.Items.FindByValue(CON.AdminMaintenanceActions.EnableCMCLinks) == null && ApplicationType != CON.ApplicationType.CPC)
            {
                item = new ListItem(CON.AdminMaintenanceActions.EnableCMCLinks, CON.AdminMaintenanceActions.EnableCMCLinks);
                this.ddlAdminActions.Items.Add(item);
            }

            if (canShowCPCEnableLinks && ddlAdminActions.Items.FindByValue(CON.AdminMaintenanceActions.EnableCPClinks) == null)
            {
                item = new ListItem(CON.AdminMaintenanceActions.EnableCPClinks, CON.AdminMaintenanceActions.EnableCPClinks);
                this.ddlAdminActions.Items.Add(item);
                item = new ListItem(CON.AdminMaintenanceActions.DisEnroll, CON.AdminMaintenanceActions.DisEnroll);
                this.ddlAdminActions.Items.Add(item);
            }

        }

        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSupervisor) ||
            Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSupervisor))
        {
            if (regOKToCredential && ddlAdminActions.Items.FindByValue(CON.AdminMaintenanceActions.CredentialingEvent) == null)
            {
                DataSet ds = GetCredentialingProviderType();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    item = new ListItem(CON.AdminMaintenanceActions.CredentialingEvent, CON.AdminMaintenanceActions.CredentialingEvent);
                    this.ddlAdminActions.Items.Add(item);
                }
            }
        }

        // this role doesn't seem to exist
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.StateAdministrator))
        {
            if (regOkHearingRights)
            {
                item = new ListItem(CON.AdminMaintenanceActions.InitiateHearingRights, CON.AdminMaintenanceActions.InitiateHearingRights);
                this.ddlAdminActions.Items.Add(item);
            }
        }
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.EnrollmentSpecialist))
        {
            if (regOkToDisenroll)
            {
                item = new ListItem(CON.AdminMaintenanceActions.DisEnroll, CON.AdminMaintenanceActions.DisEnroll);
                this.ddlAdminActions.Items.Add(item);
            }

            if (regOkToTerm)
            {
                item = new ListItem(CON.AdminMaintenanceActions.Terminate, CON.AdminMaintenanceActions.Terminate);
                this.ddlAdminActions.Items.Add(item);
            }

            if (regOkToRetroDate)
            {
                item = new ListItem(CON.AdminMaintenanceActions.RetroEffectiveDate, CON.AdminMaintenanceActions.RetroEffectiveDate);
                this.ddlAdminActions.Items.Add(item);
            }

            if (regOkToReactivate)
            {
                item = new ListItem(CON.AdminMaintenanceActions.Reactivate, CON.AdminMaintenanceActions.Reactivate);
                this.ddlAdminActions.Items.Add(item);
            }

            if (regOkToKey)
            {
                item = new ListItem(CON.AdminMaintenanceActions.EditKeyIdentifiers, CON.AdminMaintenanceActions.EditKeyIdentifiers);
                this.ddlAdminActions.Items.Add(item);
            }
        }

        if (this.ddlAdminActions.Items.Count == 2)
        {
            this.ddlAdminActions.SelectedIndex = 1;
        }
    }

    private DataSet GetCredentialingProviderType()
    {
        var ds = RegistrationController.SelectCredentialingProviderType(SelectedRegID);
        return ds;
    }

    private void View_KeepPopupOpen()
    {
        this.mpeMaint.Show();
    }

    private void View_CancelRequest()
    {
        this.mpeMaint.Hide();
        gvProviders.SelectRow(-1);

    }

    private void View_UpdateSuccess(string message)
    {
        RefreshData();
        this.lblMessages.Text = message;
        this.mpeMaint.Hide();
    }


    private void View_TermWorkflowCreated(ProviderManagementData data)
    {
        //Open registration with this regID
        (this.Page as RegistrationProvider).RegistrationId = data.RegID;
        (this.Page as RegistrationProvider).IsReadOnly = false;
    }

    private void View_ReactivateUpdateSuccess(string message, string medicaidID)
    {
        btnClear_Click(btnClear, new EventArgs());
        this.lblMessages.Text = message;
        this.txtMedicaidID.Text = medicaidID;
        RefreshData();
        this.mpeMaint.Hide();


        gvProviders.SelectRow(0);
    }

    private bool CanTermRegistration(string currentStepID, string userID, int regPgmStatusTypeID, int entityTypeID, string enrollmentStatusCode)
    {
        bool canTerm = false;
        if (entityTypeID != CON.ProviderCategoryTypeID.GroupMemberProfile)
        {

            //OHPNM-763 pschwarz 11/2/2020: Removed status check of maintanence
            //                              Added check to make sure currentstep is empty
            //OHPNM-1574 pschwarz 1/19/2021: Added reporting only 
            if ((string.IsNullOrWhiteSpace(currentStepID) || currentStepID == "0") && (enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.ACTIVE)
                || enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.REPORTINGONLY)))
            {
                canTerm = true;
            }

        }

        return canTerm;
    }

    //SAM537 - Admin function to revert Suspension
    private bool CanStartRevertSuspension(string currentStepID, string userID, int regPgmStatusTypeID, string enrollmentStatusCode)
    {
        bool canRevert = false;
        if (enableCR537 && (string.IsNullOrWhiteSpace(currentStepID) || currentStepID == "0") && regPgmStatusTypeID == CON.RegistrationProgramStatusTypeId.Suspended &&
            Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name) &&
                (enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.ACTIVE) || enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.REPORTINGONLY)))
        {
            canRevert = true;
        }
        return canRevert;
    }
    // SAM538 Admin function to New Site Visit Event
    private bool CanStartSiteVisitEvent(string currentStepID, string userID, int regID, string enrollmentStatusCode)
    {
        bool canStartSV = false;
        if ((string.IsNullOrWhiteSpace(currentStepID) || currentStepID == "0") &&
            Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name) &&
                (enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.ACTIVE) || enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.REPORTINGONLY)))
        {
            canStartSV = ScreeningController.RegistrationRequiresSiteVisit(regID) == 1;
        }
        return canStartSV;
    }

    private bool CanSuspend(string currentStepID, string userID, int regPgmStatusTypeID, int entityTypeID, string enrollmentStatusCode, string pdmsStatus)
    {
        bool canSuspend = false;
        if (entityTypeID != CON.ProviderCategoryTypeID.GroupMemberProfile)
        {
            // SAM537 show suspend only on Active providers
            if (enableCR537)
            {
                if ((string.IsNullOrWhiteSpace(currentStepID) || currentStepID == "0") && regPgmStatusTypeID != CON.RegistrationProgramStatusTypeId.Suspended &&
                    (enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.ACTIVE) || enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.REPORTINGONLY)))
                {
                    canSuspend = true;
                }
            }
            else
            {
                //OHPNM-763 pschwarz 11/2/2020: Removed status check of maintanence
                //                              Added check to make sure currentstep is empty
                //OHPNM-1574 pschwarz 1/19/2021: Added reporting only 
                if ((string.IsNullOrWhiteSpace(currentStepID) || currentStepID == "0") &&
                (enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.ACTIVE) || enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.INACTIVE)
                || enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.REPORTINGONLY)))
                {
                    canSuspend = true;
                }
                if (Helper.IsLoggedInUserInAdminRole() &&
                    (pdmsStatus == "Terminated" || pdmsStatus == "Disenrolled"))
                {
                    canSuspend = true;
                }
            }
        }

        return canSuspend;
    }

    private bool CanDisenroll(string currentStepID, string userID, int regPgmStatusTypeID, int entityTypeID, string enrollmentStatusCode, int providerTypeID)
    {
        bool canDisenroll = false;
        string MMISProviderTypeID = string.Empty;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetProviderTypeById(providerTypeID);
        if (Helper.HasRows(ds))
        {
            MMISProviderTypeID = Helper.GetString("MMIS_PROVIDER_TYPE_ID", ds.Tables[0].Rows[0]);
        }
        if (entityTypeID != CON.ProviderCategoryTypeID.GroupMemberProfile)
        {
            //OHPNM-13416:All the ICF's should not display Disenrollment link
            //OHPNM-763 pschwarz 11/2/2020: Removed status check of maintanence
            //                              Added check to make sure currentstep is empty
            //OHPNM-1574 pschwarz 1/19/2021: Added reporting only 
            if ((string.IsNullOrWhiteSpace(currentStepID) || currentStepID == "0") &&
                (enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.ACTIVE) ||
                enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.REPORTINGONLY)) &&
                (MMISProviderTypeID != CON.MMISProviderType.NON_STATE_OPERATED_ICF_MR) &&
                (MMISProviderTypeID != CON.MMISProviderType.STATE_OPERATED_ICF_MR))

            {
                canDisenroll = true;
            }
        }

        return canDisenroll;
    }
    private bool CanEditKeyIdentifier(string currentStepID, string userID, int regPgmStatusTypeID, int entityTypeID, string enrollmentStatusCode, int applicationTypeID, int waiverTypeID)
    {
        bool canEdit = false;
        //OHPNM-763 pschwarz 11/2/2020: Removed status check of maintanence
        //                              Added check to make sure currentstep is empty
        //OHPNM-1574 pschwarz 1/19/2021: Added reporting only 
        if ((string.IsNullOrWhiteSpace(currentStepID) || currentStepID == "0") && entityTypeID != CON.ProviderCategoryTypeID.GroupMemberProfile)
        {
            if (enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.ACTIVE) ||
                enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.REPORTINGONLY))
            {
                //do not display edit key identifiers for DODD and ODA providers
                if ((applicationTypeID != CON.ApplicationType.Waiver) || (applicationTypeID == CON.ApplicationType.Waiver && waiverTypeID != CON.WaiverType.DODD && waiverTypeID != CON.WaiverType.ODA))
                    canEdit = true;
            }
        }

        return canEdit;
    }

    private bool CanRetroDateRegistration(string currentStepID, string userID, int regPgmStatusTypeID, int entityTypeID)
    {
        bool canRetro = false;
        //OHPNM-763 pschwarz 11/2/2020: Removed status check of maintanence
        //                              Added check to make sure currentstep is empty
        if ((string.IsNullOrWhiteSpace(currentStepID) || currentStepID == "0") && entityTypeID != CON.ProviderCategoryTypeID.GroupMemberProfile)
        {
            canRetro = true;
        }

        return canRetro;
    }

    private bool CanReactivateRegistration(string currentStepID, string userID, int regPgmStatusTypeID, int entityTypeID, string enrollmentStatusCode, string NPI, int regID, string MMISProviderTypeID)
    {
        bool canReactivate = false;
        if (entityTypeID != CON.ProviderCategoryTypeID.GroupMemberProfile)
        {
            //OHPNM-763 pschwarz 11/2/2020: Added check to make sure currentstep is empty
            if ((string.IsNullOrWhiteSpace(currentStepID) || currentStepID == "0") && enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.INACTIVE))
            {
                //OHPNM-13835- Allow reactivate only when there is no active provider with NPI
                canReactivate = Helper.IsEligibleForReapplicationOrReactivation(NPI, regID, MMISProviderTypeID);
            }
        }
        return canReactivate;
    }

    private bool CanUpdateRegistration(string currentStepID, int regPgmStatusTypeID, int registrationStatusTypeID, string medicaidID, DateTime? endDate, string enrollmentStatusCode, string userID)
    {
        bool canUpdate = false;

        //OHPNM-763 pschwarz 11/2/2020: Removed status check of maintanence
        //                              Added check to make sure currentstep is empty
        if ((string.IsNullOrWhiteSpace(currentStepID) || currentStepID == "0") && registrationStatusTypeID == CON.RegistrationStatusTypeId.Approved
            && medicaidID != "" && (endDate.HasValue && endDate.Value.CompareTo(DateTime.Now.AddDays(120)) > 0) && (enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.ACTIVE) || enrollmentStatusCode == CON.EnrollmentStatusCode.ReferringProviderOnly))
        {
            canUpdate = true;
        }
        if (Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderServices)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.TechAdmin)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ReadOnly)
            || Helper.IsLoggedInUserInAdminRole()
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.LTCInitialRevalidation)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.LTCCHOP)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.APMSpecialist))
        {
            if ((string.IsNullOrEmpty(currentStepID) || (!string.IsNullOrEmpty(currentStepID) && currentStepID == "0")) && medicaidID != "" && (endDate.HasValue && endDate.Value.CompareTo(DateTime.Now.AddDays(120)) > 0)
                && (enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.ACTIVE) ||
                enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.REPORTINGONLY)))
            {
                canUpdate = true;
            }
        }
        else
        {
            //update option should only be visible for ES specific roles
            //security matrix not to show for credential OHPNM-2743
            canUpdate = false;
        }

        return canUpdate;
    }
    private bool CanViewRestricted(string currentStepID, int regPgmStatusTypeID, int registrationStatusTypeID, string medicaidID, DateTime? endDate, string enrollmentStatusCode, string userID, int regId)
    {
        bool canView = false;

        if ((string.IsNullOrEmpty(currentStepID) || currentStepID == "0") && medicaidID != "")
        {
            // OHPNM-13842 show link even if restricted services do not exist
            // if (RegistrationController.CheckRestrictedServicesExistsByRegID(regId))
            canView = true;
        }


        return canView;
    }

    private bool CanViewContract(string currentStepID, int regPgmStatusTypeID, int registrationStatusTypeID, string medicaidID, DateTime? endDate, string enrollmentStatusCode, string userID)
    {
        bool canView = false;

        if ((string.IsNullOrEmpty(currentStepID) || (!string.IsNullOrEmpty(currentStepID) && currentStepID == "0"))
            //&& regPgmStatusTypeID == CON.RegistrationProgramStatusTypeId.Maintenance && registrationStatusTypeID == CON.RegistrationStatusTypeId.Approved
            && medicaidID != "" && (enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.ACTIVE)))
        {
            canView = true;
        }


        return canView;
    }

    private bool CanStartCredentialingEvent(string currentStepID, int regPgmStatusTypeID, int registrationStatusTypeID, string medicaidID, DateTime? endDate, string enrollmentStatusCode, string userID)
    {
        bool canStartCredentialingEvent = false;

        //OHPNM-763 pschwarz 11/2/2020: Removed status check of maintanence
        //                              Added check to make sure currentstep is empty
        //OHPNM-1574 pschwarz 1/19/2021: Added reporting only 
        if ((string.IsNullOrWhiteSpace(currentStepID) || currentStepID == "0") && medicaidID != "" &&
                (enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.ACTIVE) ||
                enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.REPORTINGONLY)))
        {
            canStartCredentialingEvent = true;
        }
        return canStartCredentialingEvent;
    }

    private bool CanStartCredentialReconsideration(int regID)
    {
        // only when the provider is denied/termed by committee can start credentail reconsideration
        bool CanReconsider = false;

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        CanReconsider = svc.CanStartCredentialReconsideration(regID);

        return CanReconsider;
    }

    private bool CanStartProviderReconsideration(string currentStepID, string userID, int regPgmStatusTypeID, int entityTypeID, string enrollmentStatusCode, string enrollmentStatusReasonCode, DateTime? endDate)
    {
        bool CanReconsider = false;
        if ((regPgmStatusTypeID == CON.RegistrationProgramStatusTypeId.Conversion || regPgmStatusTypeID == CON.RegistrationProgramStatusTypeId.Suspended)
           && enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.INACTIVE))
        {
            if (IsInitiateReconsiderationRole())
            {
                CanReconsider = true;
            }
        }
        else if ((string.IsNullOrWhiteSpace(currentStepID) || currentStepID == "0") &&
            (regPgmStatusTypeID == CON.RegistrationProgramStatusTypeId.Denied || regPgmStatusTypeID == CON.RegistrationProgramStatusTypeId.Terminated || regPgmStatusTypeID == CON.RegistrationProgramStatusTypeId.Suspended) &&
            enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.INACTIVE))
        {
            if (IsInitiateReconsiderationRole())
            {
                CanReconsider = true;
            }
        }
        return CanReconsider;
    }

    private bool CanStartLTCCHOP(string currentStepID, int regPgmStatusTypeID, string enrollmentStatusCode, int providerTypeID)
    {
        bool canLTCCHOP = false;
        if ((string.IsNullOrWhiteSpace(currentStepID) || string.IsNullOrEmpty(currentStepID) || (currentStepID == "") || currentStepID == "0")
            && (regPgmStatusTypeID == CON.RegistrationProgramStatusTypeId.Maintenance || regPgmStatusTypeID == CON.RegistrationProgramStatusTypeId.Conversion)
            && enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.ACTIVE))
        {
            canLTCCHOP = true;
        }
        return canLTCCHOP;
    }
    private bool CanChangeTermDate(string currentStepID, int regPgmStatusTypeID, string enrollmentStatusCode)
    {
        bool CanChangeTermDt = false;

        if ((string.IsNullOrWhiteSpace(currentStepID) || currentStepID == "0") && enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.INACTIVE)
            && (regPgmStatusTypeID == CON.RegistrationProgramStatusTypeId.Conversion || regPgmStatusTypeID == CON.RegistrationProgramStatusTypeId.Terminated || regPgmStatusTypeID == CON.RegistrationProgramStatusTypeId.Suspended || regPgmStatusTypeID == CON.RegistrationProgramStatusTypeId.Disenrolled))
        {
            CanChangeTermDt = true;
        }
        return CanChangeTermDt;
    }
    private bool CanStartHearingRights(string currentStepID, int regPgmStatusTypeID, string enrollmentStatusCode)
    {
        bool CanReconsider = false;

        if (currentStepID == "0" && regPgmStatusTypeID == CON.RegistrationProgramStatusTypeId.Maintenance
            && enrollmentStatusCode == Convert.ToString(CON.EnrollStatus.ACTIVE))
        {
            CanReconsider = true;
        }
        return CanReconsider;
    }

    protected ProviderManagerData ManageConvertedProvider(DataSet ds)
    {
        ProviderManagerData keyData = new ProviderManagerData();

        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            int currentStepID = Helper.GetInt("CurrentStepID", dr);
            if (currentStepID == 0)
            {
                keyData.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
                keyData.TaxID = Helper.GetString("TaxID", dr);
                keyData.TaxIDTypeID = Helper.GetInt("TaxIDTypeID", dr);
                keyData.ProviderCategoryTypeID = Helper.GetInt("ProviderCategoryTypeID", dr);
                keyData.RegID = SelectedRegID;
                keyData.ConvertedProvider = true;
                keyData.ApplicationTypeID = Helper.GetInt("ApplicationTypeID", dr);

                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                bool reValFlag = (keyData.ApplicationTypeID == CON.ApplicationType.ChangeOfOperator) ? true : false;
                keyData.WorkflowIDRequested = svc.GetWorkflowInstance(keyData.ApplicationTypeID, keyData.ProviderCategoryTypeID,
                    keyData.ProviderTypeID, keyData.ReferralTypeID, reValFlag, false, keyData.ConvertedProvider, false);

            }
        }

        return keyData;
    }

    protected ProviderManagerData EditKeyIdentifierForRegistrations(DataSet ds)
    {
        ProviderManagerData keyData = new ProviderManagerData();

        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            //int currentStepID = Helper.GetInt("CurrentStepID", dr);
            //if (currentStepID == 0)
            //{
            keyData.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            keyData.TaxID = Helper.GetString("TaxID", dr);
            keyData.TaxIDTypeID = Helper.GetInt("TaxIDTypeID", dr);
            keyData.ProviderCategoryTypeID = Helper.GetInt("ProviderCategoryTypeID", dr);
            keyData.RegID = SelectedRegID;

            keyData.ApplicationTypeID = Helper.GetInt("ApplicationTypeID", dr);

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            keyData.WorkflowIDRequested = 0;
            keyData.AdminKeyFieldEditRequest = true;
            keyData.KeyFieldEditRequest = false;

            //}
        }

        return keyData;
    }

    #endregion

    protected void ddlPDMSStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((ddlPDMSStatus.SelectedIndex > 0 && ddlPDMSStatus.SelectedItem.Text != CON.RegistrationTaskName.SiteVisitPCG && ddlPDMSStatus.SelectedItem.Text != CON.RegistrationTaskName.SiteVisitCompliance) && ((Helper.IsLoggedInUserInAdminRole() || Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSpecialist)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderServices)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.TechAdmin)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InternalApplicationsEntry)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ComplianceSpecialist)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CommitteeQualitySpecialist)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.CredentialingQualityAssurance)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ReadOnly)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSupervisor)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCInitialReval)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCCHOP)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ODMCredentialingQualityAssurance)
            || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.PnmSuperUser)
            )))
        {
            seeAssignedPanel.Visible = true;
            seeAssignedUser.Visible = true;
            seetblBulkManage.Visible = gvProviders.Rows.Count > 0;
        }
        else
        {
            seeAssignedPanel.Visible = seeAssignedUser.Visible = seetblAssignUser.Visible = gvProviders.Columns[0].Visible = false;
            tblBulkManage.Visible = false;
            if (ddlAssigned.Items != null && ddlAssigned.Items.Count > 0)
                ddlAssigned.SelectedIndex = 0;
            if (ddlAssignedUser.Items != null && ddlAssignedUser.Items.Count > 0)
                ddlAssignedUser.SelectedIndex = 0;
        }
    }

    protected void ddlAssigned_SelectedIndexChanged(object sender, EventArgs e)
    {
        seeAssignedUser.Visible = true;
        //Populate relevant users
        if (ddlAssigned.SelectedItem.Text == "Yes")
        {
            //Get users by task name
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

            DataSet ds = psc.SelectTaskGroupByTaskName(ddlPDMSStatus.SelectedItem.Text);
            Helper.LoadList(ddlAssignedUser, ds.Tables[0], "UserName", "UserID", true);
        }
    }
    protected void btnBulkAssign_Click(object sender, EventArgs e)
    {
        gvProviders.Columns[0].Visible = seetblAssignUser.Visible = true;
        btnSendBulkEmail.Visible = false;
        btnBulkEmail.Visible = true;
        seetblAssignUser.Visible = true;
        //Get users by task name
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectTaskGroupByTaskName(ddlPDMSStatus.SelectedItem.Text);
        Helper.LoadList(ddlAssignUser, ds.Tables[0], "UserName", "UserID", true);
    }

    protected void btnBulkEmail_Click(object sender, EventArgs e)
    {
        gvProviders.Columns[0].Visible = true;
        btnSendBulkEmail.Visible = true;
        seetblAssignUser.Visible = false;
        btnBulkEmail.Visible = false;
    }


    protected void btnSendBulkEmail_Click(object sender, EventArgs e)
    {
        var providerRegIDs = new List<int>();

        foreach (GridViewRow gvItem in gvProviders.Rows)
        {
            // Process each email queue item
            CheckBox chkItem = (CheckBox)gvItem.FindControl("chkAssign");
            if (chkItem.Checked)
            {
                providerRegIDs.Add(string.IsNullOrEmpty(this.gvProviders.DataKeys[gvItem.RowIndex].Values["RegID"].ToString()) ? 0 : (int)this.gvProviders.DataKeys[gvItem.RowIndex].Values["RegID"]);
            }
        }

        SessionVarRetriever.BulkEmailProviderIds = providerRegIDs;
        Response.Redirect("~/Process/SendMail.aspx", false);
    }

    protected void btnAssign_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvItem in gvProviders.Rows)
        {
            CheckBox chkItem = (CheckBox)gvItem.FindControl("chkAssign");
            if (chkItem.Checked)
            {
                string currentStepID = string.IsNullOrEmpty(this.gvProviders.DataKeys[gvItem.RowIndex].Values["CurrentStepID"].ToString()) ? string.Empty : this.gvProviders.DataKeys[gvItem.RowIndex].Values["CurrentStepID"].ToString();
                string ProcessID = string.IsNullOrEmpty(this.gvProviders.DataKeys[gvItem.RowIndex].Values["PROCESS_ID"].ToString()) ? string.Empty : this.gvProviders.DataKeys[gvItem.RowIndex].Values["PROCESS_ID"].ToString();
                string UserID = string.IsNullOrEmpty(this.gvProviders.DataKeys[gvItem.RowIndex].Values["UserID"].ToString()) ? string.Empty : this.gvProviders.DataKeys[gvItem.RowIndex].Values["UserID"].ToString();
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

                if (string.IsNullOrEmpty(UserID))
                {
                    int stepId = svc.WF_StartStep(Convert.ToInt32(ProcessID), Helper.GetUserId(ddlAssignUser.SelectedItem.Text).ToString());
                }
                else
                {
                    svc.updateWF_STEP_Owner(Convert.ToInt32(currentStepID), ddlAssignUser.SelectedIndex == 0 ? null : ddlAssignUser.SelectedValue);
                }
            }
        }
        Search(true);
    }
    protected void btnCancelCredEvent_Click(object sender, EventArgs e)
    {
        this.mpeMaint.Hide();
        gvProviders.SelectRow(-1);

    }

    protected void btnSaveCredEvent_Click(object sender, EventArgs e)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.InsertIntoVersionTables(SelectedRegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        DataRow dr;
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            dr = ds.Tables[0].Rows[0];
            if (!string.IsNullOrEmpty(Methods.GetStringValue(dr["ErrorMessage"])))
            {
                Logging log = new Logging(Guid.NewGuid(), string.Empty);
                log.CreateLogEntry(string.Format("{0} {1}", "Error Setting up Site Visit Event Workflow", Methods.GetStringValue(dr["ErrorMessage"])), Logging.LogPriority.Error);
                lblSvErrMsg.Visible = true;
                lblSvErrMsg.Text = "Start Site Visit Event not successful";
                return;
            }
        }
        //Spawn new workflow
        Workflow.Process pr = new Workflow.Process(CON.WorkflowType.CredentialingApplication, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Guid.NewGuid());
        if (pr == null)
        {
            lblErrmsg.Visible = true;
            lblErrmsg.Text = "Start Credentialing Event not successful";
            return;
        }

        int ProcessID = pr.ProcessID;
        int CurrentStepID = pr.CurrentStepID;
        int CurrentTaskID = pr.TaskID;
        int WorkflowID = pr.WorkflowID;

        ProviderFeedHelper.InsertProviderFeedNotes(SelectedRegID, 0, HttpContext.Current.User.Identity.Name, CON.AdminMaintenanceActions.CredentialingEvent,
               enrollmentType: CON.AdminMaintenanceActions.CredentialingEvent, processID: pr.ProcessID);

        // Save the Registration ID as a process parameter of the Workflow
        //PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        svc.WF_SaveProcessParameter(ProcessID, "REGISTRATION_ID", SelectedRegID.ToString());

    }
    //SAM538 Admin function New Site Visit Event
    protected void btnCancelSiteVisitEvent_Click(object sender, EventArgs e)
    {
        this.mpeMaint.Hide();
        gvProviders.SelectRow(-1);

    }

    protected void btnStartSiteVisitEvent_Click(object sender, EventArgs e)
    {
        bool rtn = true;
        try
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

            //OHPNM-14061 - take a version copy before starting the recon workflow
            DataSet ds = svc.InsertIntoVersionTables(SelectedRegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            DataRow dr;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dr = ds.Tables[0].Rows[0];
                if (!string.IsNullOrEmpty(Methods.GetStringValue(dr["ErrorMessage"])))
                {
                    Logging log = new Logging(Guid.NewGuid(), string.Empty);
                    log.CreateLogEntry(string.Format("{0} {1}", "Error Setting up Site Visit Event Workflow", Methods.GetStringValue(dr["ErrorMessage"])), Logging.LogPriority.Error);
                    lblSvErrMsg.Visible = true;
                    lblSvErrMsg.Text = "Start Site Visit Event not successful";
                    return;
                }
            }

            int entryTaskID = CON.SiteVistEventEntryTaskID;
            string startByUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
            Workflow.Process pr = new Workflow.Process(CON.WorkflowType.SiteVistEvent, null, Guid.NewGuid(), entryTaskID);
            if (pr == null)
            {
                lblSvErrMsg.Visible = true;
                lblSvErrMsg.Text = "Start Site Visit Event not successful";
                return;
            }

            ProviderFeedHelper.InsertProviderFeedNotes(SelectedRegID, 0, HttpContext.Current.User.Identity.Name, CON.AdminMaintenanceActions.SiteVisitEvent,
                enrollmentType: CON.AdminMaintenanceActions.SiteVisitEvent, processID: pr.ProcessID);
            int ProcessID = pr.ProcessID;
            int CurrentStepID = pr.CurrentStepID;
            int CurrentTaskID = pr.TaskID;
            int WorkflowID = pr.WorkflowID;

            if (ProcessID > 0 && WorkflowID > 0)
            {
                // Save the Registration ID as a process parameter of the Workflow


                svc.InsertRegApplicationRecord(SelectedRegID, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId), pr.ProcessID);

                svc.WF_SaveProcessParameter(ProcessID, "REGISTRATION_ID", SelectedRegID.ToString());
                svc.WF_SaveProcessParameter(ProcessID, CON.ProcessParameter.WorkflowEventTypeID, CON.WorkflowEventType.SiteVisitEvent.ToString());
                svc.updateWF_STEP_Owner(CurrentStepID, startByUser);

                ScreeningController.InsertProviderSiteVisitActivity(SelectedRegID, DateTime.Now, startByUser);
                RegistrationController.UpdateRegistrationStatusType(SelectedRegID, CON.RegistrationStatusTypeId.SiteVisit, DateTime.Now, startByUser);

                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, SelectedRegID, false));
                sqlParms.Add(SqlParms.CreateParameter("SITE_VISIT_NEEDED", DbType.String, "Y", false));
                sqlParms.Add(SqlParms.CreateParameter("SITE_VISIT_DISPOSITION_REVIEWD_BY", DbType.Guid, startByUser, false));
                sqlParms.Add(SqlParms.CreateParameter("STEP_ID", DbType.Int32, CurrentStepID, false));
                sqlParms.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, ProcessID, false));
                sqlParms.Add(SqlParms.CreateParameter("NOTES", DbType.String, string.Empty, false));
                DataAccess.ExecuteStoredProcedure("usp_UpdateSiteVisitDisposition", sqlParms);

                svc.UpdateRegistrationCustom(new Dictionary<string, string>() { { "REG_ID", SelectedRegID.ToString() }, { "WORKFLOW_EVENT_TYPE_ID", CON.WorkflowEventType.SiteVisitEvent.ToString() }, { "WAIVER_SERVICE_UPDATE_TYPE_ID", null } });

            }

        }
        catch (Exception ex)
        {
            AddError("* Initiation of Reconsideration WF not successful. Error: " + ex.Message, ref rtn);
        }
    }
    protected void btnSaveProvRec_Click(object sender, EventArgs e)
    {
        bool rtn = true;
        if (ValidateReconsideration())
        {
            if (encRequestReconsiderationDoc.PostedFile != null)
            {
                _DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
                lblreconMsg.Text = string.Empty;




                try
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                        _DestinationPath = @"C:\Temp\";

                    string newFileName = Helper.CleanFilePath(encRequestReconsiderationDoc.FileName);
                    byte[] fileBytes = encRequestReconsiderationDoc.EncryptedFileBytes;
                    PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                    int controlID = svc.GetRegSectionUploadControlIDByRegID(SelectedRegID, CON.RegistrationPageName.Reconsideration, CON.UploadControlDocumentTitles.RequestReconsideration);

                    File.WriteAllBytes(Path.Combine(@_DestinationPath, newFileName), fileBytes);
                    lblreconMsg.Text = "File Uploaded: " + Helper.HtmlEncode(newFileName);
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", SelectedRegID.ToString());
                    parms.Add("REG_PAGE_TYPE_ID", CON.RegistrationPageType.Identification.ToString());
                    parms.Add("REG_PAGE_SECTION", "Reconsideration");
                    parms.Add("SCREENING_ACTIVITY_ID", null);
                    parms.Add("NAME", newFileName);
                    parms.Add("DESCRIPTION", "Request Reconsideration");
                    parms.Add("FILE_NAME", newFileName);
                    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("REG_SECTION_UPLOAD_CONTROL_ID", controlID.ToString());
                    parms.Add("ROW_ID", "");
                    int docID = svc.InsertRegistrationData(Convert.ToInt32(SelectedRegID), "DOCUMENT", parms);
                    SendToCMS(docID, fileBytes, newFileName);


                    if (docID > 0 && SelectedRegID > 0)
                    {
                        ProviderFeedHelper.InsertProviderFeedNotes(SelectedRegID, 0, HttpContext.Current.User.Identity.Name, CON.AdminMaintenanceActions.ProviderReconsideration,
                            enrollmentType: CON.AdminMaintenanceActions.ProviderReconsideration);

                        CreateRequestReconsideration(SelectedRegID);
                        AddError("We have received your reconsideration request.", ref rtn);

                        //(this.Page as RegistrationProvider).RegistrationId = SelectedRegID;
                        //(this.Page as RegistrationProvider).IsReadOnly = false;
                        //(this.Page as RegistrationProvider).RegistrationStep = CON.SectionTypeID.RECONSIDERATION;
                        string url = "~/Process/Registration.aspx?Step=" + CON.SectionTypeID.RECONSIDERATION.ToString() + "&RegId=" + SelectedRegID.ToString();
                        Response.Redirect(url, false);
                    }

                    return;
                }
                catch (Exception ex)
                {
                    AddError("* No File uploaded. Error: " + ex.Message, ref rtn);
                }
            }
            else
            {
                AddError("* No Request Date or File uploaded.", ref rtn);
            }
        }
        //Reset the hidden field to allow subsequent clicks;
        hdnIsProcessing.Value = "false";
        if (Page.IsValid) { mpeMaint.Hide(); }
        return;
    }

    private void SendToCMS(int docID, byte[] fileBytes, string fileName)
    {
        bool rtn = true;
        try
        {
            OnBaseInterface onBaseInterface = new OnBaseInterface();
            onBaseInterface.SubmitFile(SelectedRegID, docID, fileBytes, fileName);
        }
        catch (Exception ex)
        {
            AddError("* No File uploaded. Error: " + ex.Message, ref rtn);
        }
    }
    private void CreateRequestReconsideration(int regID)
    {
        bool rtn = true;
        try
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

            //OHPNM-14061 - take a version copy before starting the recon workflow
            DataSet ds = svc.InsertIntoVersionTables(regID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            DataRow dr;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dr = ds.Tables[0].Rows[0];
                if (!string.IsNullOrEmpty(Methods.GetStringValue(dr["ErrorMessage"])))
                {
                    Logging log = new Logging(Guid.NewGuid(), string.Empty);
                    log.CreateLogEntry(string.Format("{0} {1}", "Error Setting up Reconsideration Workflow", Methods.GetStringValue(dr["ErrorMessage"])), Logging.LogPriority.Error);
                    AddError("* Initiation of Reconsideration WF not successful. Error: ", ref rtn);
                    return;
                }
            }

            int entryTaskID = CON.ReconsiderationLevelEntryTaskID;
            Workflow.Process pr = new Workflow.Process(CON.WorkflowType.RegistrationNew, null, Guid.NewGuid(), entryTaskID);
            if (pr == null)
            {
                lblErrmsg.Visible = true;
                AddError("* Initiation of Reconsideration WF not successful. Error: ", ref rtn);
                return;
            }

            int ProcessID = pr.ProcessID;
            int CurrentStepID = pr.CurrentStepID;
            int CurrentTaskID = pr.TaskID;
            int WorkflowID = pr.WorkflowID;

            if (ProcessID > 0 && WorkflowID > 0)
            {
                // Save the Registration ID as a process parameter of the Workflow


                svc.InsertRegApplicationRecord(regID, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId), pr.ProcessID);

                svc.WF_SaveProcessParameter(ProcessID, "REGISTRATION_ID", SelectedRegID.ToString());
                svc.WF_SaveProcessParameter(ProcessID, CON.ProcessParameter.WorkflowEventTypeID, CON.WorkflowEventType.Reconsideration.ToString());
                svc.updateWF_STEP_Owner(CurrentStepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                //svc.WF_TakeAction(ProcessID, "Submit For Review", string.Empty);

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", regID.ToString());
                parms.Add("RECONSIDERATION_REQUEST_DATE", txtDateofReconsidertaionRequest.Text);
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                int reconsiderationId = svc.InsertRegistrationData(Convert.ToInt32(regID), "RECONSIDERATION", parms);

                svc.UpdateRegistrationCustom(new Dictionary<string, string>() { { "REG_ID", regID.ToString() }, { "WORKFLOW_EVENT_TYPE_ID", CON.WorkflowEventType.Reconsideration.ToString() }, { "WAIVER_SERVICE_UPDATE_TYPE_ID", null } });

                ProviderFeedHelper.InsertProviderFeedNotes(SelectedRegID, 0, HttpContext.Current.User.Identity.Name, CON.AdminMaintenanceActions.ProviderReconsideration,
                    enrollmentType: CON.AdminMaintenanceActions.ProviderReconsideration);//Recheck
                                                                                         //(this.Page as RegistrationProvider).RegistrationId = regID;
                                                                                         //(this.Page as RegistrationProvider).IsReadOnly = false;
                                                                                         //(this.Page as RegistrationProvider).RegistrationStep = CON.SectionTypeID.RECONSIDERATION;
                                                                                         //Response.Redirect("~/Process/Registration.aspx", true);
            }

        }
        catch (Exception ex)
        {
            AddError("* Initiation of Reconsideration WF not successful. Error: " + ex.Message, ref rtn);
        }
    }

    protected void btnCancelProvRec_Click(object sender, EventArgs e)
    {
        this.mpeMaint.Hide();
        gvProviders.SelectRow(-1);

    }

    private bool GetWorkFlowIdForRegId(int regID, int WorkflowTypeID)
    {
        DataSet ds = GetWorkFlowProcessesForRegID(regID);
        int wfID;
        if (Helper.HasRows(ds))
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                wfID = Helper.GetInt("WORKFLOW_ID", dr);
                if (wfID.Equals(WorkflowTypeID))
                {
                    return true;

                }

            }
        }
        return false;
    }

    private DataSet GetWorkFlowProcessesForRegID(int selectedRegId)
    {
        DataSet ds = new DataSet();
        try
        {
            using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
            {
                ds = svc.GetWorkflowProcessesForRegID(selectedRegId);
            }
        }
        catch (Exception ex)
        {
            log.CreateLogEntry("Failed to fetch the worflow Id for Reg Id :" + selectedRegId + ". Exception Message: "
                               + ex.Message + " Exception Stack: " + ex.StackTrace, Logging.LogPriority.Error);
        }
        return ds;
    }
    //SAM758 express admin Remove Exclusion
    protected void btnSaveRemoveExclusion_Click(object sender, EventArgs e)
    {
        lblErrRemExcl.Visible = false;
        try
        {
            if (ddlSelectExclusion.SelectedIndex == -1)
            {
                lblErrRemExcl.Visible = true;
                lblErrRemExcl.Text = "* Select an Exclusion";
                mpeMaint.Show();
                return;
            }
            if (string.IsNullOrEmpty(txtRemoveExclComments.Text.Trim()))
            {
                lblErrRemExcl.Visible = true;
                lblErrRemExcl.Text = "* Comments are required";
                mpeMaint.Show();
                return;
            }
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            bool isDeleted = svc.RemoveScreeningExlusionData(SelectedRegID, Convert.ToInt32(ddlSelectExclusion.SelectedValue), txtRemoveExclComments.Text.Trim(), Helper.GetUserId(HttpContext.Current.User.Identity.Name));
            if (isDeleted)
            {
                lblErrRemExcl.Visible = true;
                lblErrRemExcl.Text = "Exclusion data removed successfully.";
            }
            else
            {
                lblErrRemExcl.Visible = true;
                lblErrRemExcl.Text = "* Exclusion data not found.";
            }
        }
        catch (Exception ex)
        {
            lblErrRemExcl.Visible = true;
            lblErrRemExcl.Text = "* Removing Exclusion data not successful. Error: " + ex.Message;
            mpeMaint.Show();
        }

    }
    private bool CanShowCPCReenableLinks(int regID)
    {
        bool canShowLink = false;
        int cpcProgramYear = Convert.ToInt32(AppSettings.Get("CPCProgramYear")) - 1;
        string cpcEnrollmentEndDate = AppSettings.Get("CPCEnrollmentPeriodEndDate") + "/" + Convert.ToString(cpcProgramYear);
        string nextCPCEnrollmentStartDate = AppSettings.Get("CPCEnrollmentPeriodStartDate") + "/" + Convert.ToString(AppSettings.Get("CPCProgramYear"));
        DateTime dtNow = DateTime.Now;
        DateTime dtCPCEnrollmentEndDate = DateTime.Parse(cpcEnrollmentEndDate);
        DateTime dtnextCPCEnrollmentStartDate = DateTime.Parse(nextCPCEnrollmentStartDate);

        if (dtCPCEnrollmentEndDate < dtNow && dtNow < dtnextCPCEnrollmentStartDate)
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            canShowLink = svc.CanReEnableCPCLinks(regID);
        }

        return canShowLink;
    }

    private bool CanShowCMCReenableLinks(int regID)
    {
        bool canShowLink = false;
        DateTime startDate = DateTime.ParseExact(AppSettings.Get("CMCProgramStartDate"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

        string _day = startDate.Day.ToString();
        string _month = startDate.Month.ToString();
        string nextCMCStartDate = _month + "/" + _day + "/" + Convert.ToString(DateTime.Now.Year + 1);
        DateTime dtCMCStart = DateTime.Parse(nextCMCStartDate);

        if (dtCMCStart > DateTime.Now)
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            canShowLink = svc.CanReEnableCMCLinks(regID);
        }

        return canShowLink;
    }
    public bool IsInternalUser()
    {
        return Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name);
    }

    public bool IsInitiateReconsiderationRole()
    {
        return Helper.IsInitiateReconsiderationRoles(HttpContext.Current.User.Identity.Name);
    }

    protected void btnSaveNotProcessed_Click(object sender, EventArgs e)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        //DataSet ds =  svc.GetWFProcessByRegId(SelectedRegID);
        DataSet ds = svc.SelectRegistrationByRegID(SelectedRegID);
        if (ds.Tables[0].Rows.Count > 0)
        {
            int processId = Convert.ToInt32(ds.Tables[0].Rows[0]["ProcessID"]);

            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", SelectedRegID.ToString());
            parms.Add("REGISTRATION_STATUS_TYPE_ID", CON.RegistrationStatusTypeId.NotProcessed.ToString());
            parms.Add("REG_PROGRAM_STATUS_TYPE_ID", CON.RegistrationProgramStatusTypeId.NotProcessed.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            svc.UpdateRegistrationCustom(parms);

            svc.WF_TakeAction(processId, "Not Processed", string.Empty);

            //PRGCR313
            int workflowEventTypeID = Convert.ToInt32(ds.Tables[0].Rows[0]["WORKFLOW_EVENT_TYPE_ID"]);
            if (workflowEventTypeID == CON.WorkflowEventType.ChangeProviderType)
            {
                RegistrationController.UpdateProviderTypeChangeStatus(SelectedRegID, CON.ProviderTypeChangeRequestStatus.Cancelled, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            }

            var notes = string.IsNullOrEmpty(txtNotProcessedComments.Text) ? CON.AdminMaintenanceActions.NotProcessed : txtNotProcessedComments.Text;
            ProviderFeedHelper.InsertProviderFeedNotes(SelectedRegID, 0, HttpContext.Current.User.Identity.Name, notes, enrollmentType: CON.AdminMaintenanceActions.NotProcessed, finalDisposition: CON.FinalDisposition.NotProcessed);
        }



    }
    protected void btnSaveRevertSuspend_Click(object sender, EventArgs e)
    {
        bool rtn = false;
        lblErrRevertSuspend.Visible = false;
        try
        {
            if (string.IsNullOrEmpty(txtRevertSuspendComments.Text.Trim()))
            {
                lblErrRevertSuspend.Visible = true;
                lblErrRevertSuspend.Text = "* Comments are required";
                mpeMaint.Show();
                return;
            }
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet ds = svc.SelectRegistrationByRegID(SelectedRegID);
            if (ds.Tables[0].Rows.Count > 0)
            {

                ProviderFeedHelper.InsertProviderFeedNotes(SelectedRegID, 0, HttpContext.Current.User.Identity.Name, txtRevertSuspendComments.Text,
                     enrollmentType: CON.AdminMaintenanceActions.AdminRevertSuspension);

                int CurrentstepID = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CurrentStepID"].ToString()) ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["CurrentStepID"]);
                if (CurrentstepID == 0 || CurrentstepID == -1)
                {

                    DataSet ds1 = svc.InsertIntoVersionTables(SelectedRegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                    DataRow dr;
                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        dr = ds1.Tables[0].Rows[0];
                        if (!string.IsNullOrEmpty(Methods.GetStringValue(dr["ErrorMessage"])))
                        {
                            Logging log = new Logging(Guid.NewGuid(), string.Empty);
                            log.CreateLogEntry(string.Format("{0} {1}", "Error Setting up Revert Suspension Workflow", Methods.GetStringValue(dr["ErrorMessage"])), Logging.LogPriority.Error);
                            AddError("* Initiation of Revert Suspension WF not successful. Error: ", ref rtn);
                            return;
                        }
                    }

                    int entryTaskID = 1;
                    Workflow.Process pr = new Workflow.Process(CON.WorkflowType.RegistrationNew, null, Guid.NewGuid(), entryTaskID);
                    if (pr == null)
                    {
                        lblErrRevertSuspend.Visible = true;
                        lblErrRevertSuspend.Text = "* Initiation of Revert Suspension WF not successful";
                        return;
                    }

                    int ProcessID = pr.ProcessID;
                    int CurrentStepID = pr.CurrentStepID;
                    int CurrentTaskID = pr.TaskID;
                    int WorkflowID = pr.WorkflowID;

                    if (ProcessID > 0 && WorkflowID > 0)
                    {
                        // Save the Registration ID as a process parameter of the Workflow

                        svc.InsertRegApplicationRecord(SelectedRegID, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId), pr.ProcessID);

                        svc.WF_SaveProcessParameter(ProcessID, "REGISTRATION_ID", SelectedRegID.ToString());
                        svc.WF_SaveProcessParameter(ProcessID, CON.ProcessParameter.WorkflowEventTypeID, CON.WorkflowEventType.UpdateReg.ToString());
                        svc.WF_SaveProcessParameter(ProcessID, CON.ProcessParameter.IsReactivation, "1");
                        svc.WF_SaveProcessParameter(ProcessID, CON.ProcessParameter.IsSendHistoryinTxn, "1");
                        svc.WF_SaveProcessParameter(ProcessID, CON.ProcessParameter.IsRevertSuspension, "1");

                        svc.updateWF_STEP_Owner(CurrentStepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                        Dictionary<string, string> reactivateParams = new Dictionary<string, string>();
                        reactivateParams.Add("REG_ID", SelectedRegID.ToString());
                        reactivateParams.Add("ISReactivation", "1");
                        reactivateParams.Add("WAIVER_SERVICE_UPDATE_TYPE_ID", "0");
                        reactivateParams.Add("WORKFLOW_EVENT_TYPE_ID", CON.WorkflowEventType.RevalReg.ToString());
                        svc.UpdateRegistrationCustom(reactivateParams);

                        //Response.Redirect("~/Process/ProviderUpdateSummary.aspx?RegId=" + SelectedRegID.ToString(),false);
                        (this.Page as RegistrationProvider).RegistrationId = SelectedRegID;
                        (this.Page as RegistrationProvider).IsReadOnly = false;
                        (this.Page as RegistrationProvider).RegistrationStep = CON.SectionTypeID.OrgInfo;
                        Response.Redirect("~/process/Registration.aspx?RegId=" + SelectedRegID.ToString(), false);

                        //string url = "~/Process/Registration.aspx?Step=" + CON.SectionTypeID.OrgInfo.ToString() + "&RegId=" + SelectedRegID.ToString();
                        //Response.Redirect(url,false);
                    }
                    txtRevertSuspendComments.Text = string.Empty;
                }

            }
        }
        catch (Exception ex)
        {
            lblErrRevertSuspend.Visible = true;
            lblErrRevertSuspend.Text = "* Initiation of Revert Suspension WF not successful. Error: " + ex.Message;
            mpeMaint.Show();
            //AddError("* Initiation of Revert Suspension WF not successful. Error: " + ex.Message, ref rtn);
        }


    }

}