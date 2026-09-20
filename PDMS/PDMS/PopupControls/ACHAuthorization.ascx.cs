using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Pages_ACHAuthorization : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
    DataSet ds;
    string stateName;

    private int EntityTypeID
    {
        get { return ViewState["EntityTypeID"] == null ? 0 : Convert.ToInt32(ViewState["EntityTypeID"]); }
        set { ViewState["EntityTypeID"] = value; }
    }
    private int ProviderTypeID
    {
        get { return ViewState["ProviderTypeID"] == null ? 0 : Convert.ToInt32(ViewState["ProviderTypeID"]); }
        set { ViewState["ProviderTypeID"] = value; }
    }
    private int ReferralID
    {
        get { return ViewState["ReferralID"] == null ? 0 : Convert.ToInt32(ViewState["ReferralID"]); }
        set { ViewState["ReferralID"] = value; }
    }
    private int SpecialtyTypeID
    {
        get { return ViewState["SpecialtyTypeID"] == null ? 0 : Convert.ToInt32(ViewState["SpecialtyTypeID"]); }
        set { ViewState["SpecialtyTypeID"] = value; }
    }
    private int TaxIdTypeID
    {
        get { return ViewState["TaxIdTypeID"] == null ? 0 : Convert.ToInt32(ViewState["TaxIdTypeID"]); }
        set { ViewState["TaxIdTypeID"] = value; }
    }
    private int RegistrationStatusTypeID
    {
        get { return ViewState["RegistrationStatusTypeID"] == null ? 0 : Convert.ToInt32(ViewState["RegistrationStatusTypeID"]); }
        set { ViewState["RegistrationStatusTypeID"] = value; }
    }

    protected override void OnLoad(EventArgs e)
    {
        //SAM768 - Show ACH page only if the provider agent has EFT agent role along with Enrollment agent role. Provider admin and power agents should have access to this page.
        bool isPowerAgentForSelctedProvAdmin = Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, this.WorkflowPage.RegistrationId);
        bool ifEFTAgent = (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent)
                 && Helper.IsUserInSubRoles(this.WorkflowPage.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EftAgentSubRoles));
        bool isProvAdmin = Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator);

        bool HideEFTPageProvAdmin =Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) && !(isPowerAgentForSelctedProvAdmin || ifEFTAgent || isProvAdmin);

        // OHPNM-5168
        if (Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInLTCInitialRevalidationRole(HttpContext.Current.User.Identity.Name)
            || HideEFTPageProvAdmin)
        {
            cantDisplayWarning.Visible = true;
            divConfirm.Visible = false;
            mainPanel.Visible = false;
        }

        LoadControlData();
        base.OnLoad(e);
        Page.Title = "EFT Banking";
    }
    public override void LoadControlData()
    {
        DataRow dr = Registration.GetRegistration(this.WorkflowPage.RegistrationId);

        //Fix made as part of Bug- 5377
        int IsUpdate = !string.IsNullOrEmpty(Helper.GetString("Workflow_Event_Type_Id", dr)) ? Convert.ToInt32(Helper.GetString("Workflow_Event_Type_Id", dr)) : 0;

        SetFormFromData(dr);

        if (!Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) &&
            !Registration.SectionIsValidated(this.WorkflowPage.RegistrationId, MAXIMUS.Core.Libraries.Constants.SectionTypeID.PrimaryServiceAddress) && (IsUpdate != CON.WorkflowEventType.UpdateReg))
        {
            ucMessageBox.Show(4, "Prerequisite", "The Primary Service Address page is a prerequisite to this page.Please fill out the Primary Service Address page");
        }
        LoadBankingInfo();

        LoadRemitInfo();

        LoadFeeinformation();

        LoadEftContact();

        LoadVendorInfo();

        LoadReliaCard();

        SetEditability();

        SetVisibility();

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.ApplicationFee)
        {
            btnSave.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSave.ValidationGroup +
                "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
            // Intend to receive MCC
            foreach (ListItem li in rblINTEND_TO_RECEIVE_MCC.Items)
                li.Attributes.Add("onclick", "javascript:ACHTogglePanelYes('" + rblINTEND_TO_RECEIVE_MCC.ClientID + "','" + pnlBankingInfo.ClientID + "');");

            chkBankInUS.Attributes.Add("onclick", "javascript:ToggleVisible('" + chkBankInUS.ClientID + "','" + bankInfo.ClientID + "');");
            rdlWaiverPaymentType.Items[0].Attributes.Add("onclick", "javascript:ToggleWaiverVisible('" + divBankingInfo.ClientID + "','" + divWaiverBankingInfo.ClientID + "','" + CON.WaiverPaymentType.DirectDeposit + "','" + divConfirm.ClientID + "');");
            rdlWaiverPaymentType.Items[1].Attributes.Add("onclick", "javascript:ToggleWaiverVisible('" + divBankingInfo.ClientID + "','" + divWaiverBankingInfo.ClientID + "','" + CON.WaiverPaymentType.ReliaCard + "','" + divConfirm.ClientID + "');");
        }

        UpdatePanel upShowNames1 = (UpdatePanel)this.Page.Master.FindControl("UpShowNames1");
        if (upShowNames1 != null)
        {
            upShowNames1.Attributes.CssStyle.Add("visibility", "none");
            upShowNames1.Visible = false;
        }
    }

    private void LoadBankingInfo()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ach_request_contact_custom");

        if (Helper.HasRows(ds))
        {
            if (!string.IsNullOrEmpty(Helper.GetString("BANK_NAME", ds.Tables[0].Rows[0])) || !string.IsNullOrEmpty(Helper.GetString("ACCOUNT_NUMBER", ds.Tables[0].Rows[0])))
            {
                grdBankingInfo.DataSource = ucBankingInfo.DataList = ds.Tables[0];
                //chkBankInUS.Checked = true;
                bankInfo.Attributes.Add("style", "display:block;");
                divConfirm.Attributes.Add("style", "display:block;");
            }
            DataRow dr = ds.Tables[0].Rows[0];
            hdnRegAchRequestID.Value = Helper.GetString("REG_ACH_REQUEST_ID", dr);
            rblINTEND_TO_RECEIVE_MCC.SelectedIndex = (Helper.GetBool("INTEND_TO_RECEIVE_MCC", dr) ? 0 : 1);
            rblWISH_TO_CONTINUE_ACH.SelectedIndex = (Helper.GetBool("WISH_TO_CONTINUE_ACH", dr) ? 0 : 1);
            chkFeeInformation.Checked = Helper.GetBool("FEE_SUBMITTED_FOR_PAYMENT", dr);
        }
        else grdBankingInfo.DataSource = ucBankingInfo.DataList = null;


        grdBankingInfo.DataBind();

		btnAddBankingInfo.Visible = (grdBankingInfo.Rows.Count == 0);
        btnHistoryBankingInfo.Visible = (grdBankingInfo.Rows.Count > 0);

    }
    private void LoadReliaCard()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ACH_RELIACARD");
        if (Helper.HasRows(ds))
        {
            grdReliaCard.DataSource = ucReliaCard.DataList = ds.Tables[0];
            divConfirm.Attributes.Add("style", "display:block;");
        }
        else
        {
            grdReliaCard.DataSource = new DataTable(); //null
        }
        grdReliaCard.DataBind();
        btnAddReliaCard.Visible = (grdReliaCard.Rows.Count == 0);
        btnHistoryReliaCard.Visible = (grdReliaCard.Rows.Count > 0);
    }
    private void LoadFeeinformation()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ACH_FEE_INFORMATION");
        if (Helper.HasRows(ds)) grdFeeInformation.DataSource = ucFeeinformation.DataList = ds.Tables[0];
        grdFeeInformation.DataBind();
    }

    private void LoadEftContact()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ACH_CONTACT");
        if (Helper.HasRows(ds)) grdEftContact.DataSource = ucEftContact.DataList = ds.Tables[0];
        grdEftContact.DataBind();
        btnAddEftContact.Visible = (grdEftContact.Rows.Count == 0);
        btnHistoryEftContact.Visible = (grdEftContact.Rows.Count > 0);
    }

    private void LoadVendorInfo()
    {
        // Only display Vendor Information if Provider Services
        pnlVendorInfo.Visible = false;
        if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name)) return;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ACH_EDISON");
        if (Helper.HasRows(ds)) grdVendorInfo.DataSource = ucVendorInfo.DataList = ds.Tables[0];
        grdVendorInfo.DataBind();
        btnAddVendorInfo.Visible = (grdVendorInfo.Rows.Count == 0);
        btnHistoryVendorInfo.Visible = (grdVendorInfo.Rows.Count > 0);
        pnlVendorInfo.Visible = true;
    }

    private void LoadRemitInfo()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
        if (!Helper.HasRows(ds)) return;
        DataRow dr = ds.Tables[0].Rows[0];
        lblAddress.Text = Helper.GetString("PAYTO_ADDRESS1", dr);
        lblAddress2.Text = Helper.GetString("PAYTO_ADDRESS2", dr);
        lblBillingContactName.Text = Helper.GetString("PAYTO_ADDRESS_NAME", dr);
        lblCity.Text = Helper.GetString("PAYTO_CITY", dr);
        lblExtZip.Text = Helper.GetString("PAYTO_EXT_ZIP", dr);
        lblBPA56.Text = Helper.GetString("PAYTO_EMAIL_ADDRESS", dr);
        lblPayToName.Text = Helper.GetString("CHECK_PAYABLE_TO_NAME", dr);
        lblState.Text = Helper.GetString("PAYTO_STATE", dr);
        lblZip.Text = Helper.GetString("PAYTO_ZIP", dr);
        chkConfirmCorrect.Checked = (Helper.GetBool("CONFIRM_CORRECT", dr) ? true : false);
        hdnRegServiceLocationID.Value = Helper.GetString("REG_SERVICE_LOCATION_ID", dr);
    }

    private void SetFormFromData(DataRow dr)
    {
        hdnRegAchRequestID.Value = hdnRegServiceLocationID.Value = string.Empty;

        this.EntityTypeID = Helper.GetInt("ENTITY_TYPE_ID", dr);
        this.ProviderTypeID = Helper.GetInt("PROVIDER_TYPE_ID", dr);
        this.ReferralID = Helper.GetInt("DIDD_REFERRAL_ID", dr);
        this.SpecialtyTypeID = Helper.GetInt("SPECIALTY_TYPE_ID", dr);
        this.TaxIdTypeID = Helper.GetInt("TAX_ID_TYPE_ID", dr);
        RegistrationStatusTypeID = Helper.GetInt("REGISTRATION_STATUS_TYPE_ID", dr);

        string IsDirectOrReliaCard = Helper.GetString("MAX_FIN_DEBIT_IND", dr);
        if (IsDirectOrReliaCard == CON.WaiverPaymentType.DirectDeposit)
        {
            rdlWaiverPaymentType.SelectedValue = CON.WaiverPaymentType.DirectDeposit;
        }
        else if (IsDirectOrReliaCard == CON.WaiverPaymentType.ReliaCard)
        {
            rdlWaiverPaymentType.SelectedValue = CON.WaiverPaymentType.ReliaCard;
        }

        rblINTEND_TO_RECEIVE_MCC.SelectedIndex = rblWISH_TO_CONTINUE_ACH.SelectedIndex = -1;
        string IsAttest = Helper.GetString("ACH_ATTEST", dr);
        if (IsAttest == "1")
        {
            chkConfirm.Checked = true;
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ACH_REQUEST");

        if (Helper.HasRows(ds))
        {
            string IsBankOutside = Helper.GetString("IS_BANK_OUTSIDE_US", ds.Tables[0].Rows[0]);
            if (!string.IsNullOrEmpty(IsBankOutside) && IsBankOutside == "True")
            {
                chkBankInUS.Checked = true;
            }
        }
    }

    private void SetVisibility()
    {
        pnlWaiverPaymentType.Visible = ReferralID > 0 && EntityTypeID == CON.ProviderCategoryTypeID.Individual;
        if (pnlWaiverPaymentType.Visible)
        {
            if (rdlWaiverPaymentType.SelectedValue == CON.WaiverPaymentType.DirectDeposit)
            {
                divBankingInfo.Attributes.Add("style", "display:block;");

                divWaiverBankingInfo.Attributes.Add("style", "display:none;");
            }
            else if (rdlWaiverPaymentType.SelectedValue == CON.WaiverPaymentType.ReliaCard)
            {
                divBankingInfo.Attributes.Add("style", "display:none;");
                divWaiverBankingInfo.Attributes.Add("style", "display:block;");
            }
            else
            {
                divWaiverBankingInfo.Attributes.Add("style", "display:none;");
            }
        }
        else
        {
            divBankingInfo.Attributes.Add("style", "display:block;");
            pnlWaiverPaymentType.Visible = false;
            divWaiverBankingInfo.Visible = false;
        }

        divEFTInfo.Visible = Registration.PageIsVisible(this.WorkflowPage.RegistrationId, this.EntityTypeID, this.ProviderTypeID, ReferralID, "EFT Banking", "EFTContact", this.WorkflowPage.IsWaiverServiceProvider);


        // Only display if Updating the Provider
        pnlWISH_TO_CONTINUE_ACH.Visible = RegistrationStatusTypeID == CON.RegistrationStatusTypeId.UpdateProvider;

        //redo explicit checks based on name== should be based on appsetting of something like MCCVisibility.
        stateName = AppSettings.Get("StateName");

        ///redo this - should not have a hard coded name, should have a flag like IntendToReceiveMCC (true or false) in app settings - instead.
        if (stateName == "Nebraska")
        {
            rblINTEND_TO_RECEIVE_MCC.Visible = res_paymentError.Visible = pnlWISH_TO_CONTINUE_ACH.Visible = false;
            string displayType = chkBankInUS.Checked ? "none" : "block";
            bankInfo.Attributes.Add("style", "display:" + displayType + ";");
        }

        string userId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

        // FAO User can see ACH detail
        bool feePaymentRequired = psc.CheckAttestToFeePaymentRequired(this.WorkflowPage.RegistrationId);
        bool userinFAO = Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.FAOperator);
        if (userinFAO)
        {
            pnlBankingInfo.Style["display"] = "block";
            pnlRemit.Style["display"] = "block";
            pnlVendorInfo.Style["display"] = "block";
            pnlProviderFeeInfo.Style["display"] = feePaymentRequired ? "block" : "none";
            pnlReviewFeeInfo.Style["display"] = feePaymentRequired ? "block" : "none";
        }
        else
        {
            pnlProviderFeeInfo.Style["display"] = feePaymentRequired ? "block" : "none";
            pnlReviewFeeInfo.Style["display"] = "none";
        }

        if (!string.IsNullOrEmpty(userId))
        {
            if (Registration.PageIsVisible(this.WorkflowPage.RegistrationId, EntityTypeID, ProviderTypeID, 0, "Agreements", "ACH Authorization", this.WorkflowPage.IsWaiverServiceProvider))
            {
                pnlBankingInfo.Style["display"] = "block";
                pnlRemit.Style["display"] = "block";
                pnlVendorInfo.Style["display"] = "block";
                pnlFeeInfo.Style["display"] = feePaymentRequired ? "block" : "none";
            }

        }

        //foreach (ListItem li in rblINTEND_TO_RECEIVE_MCC.Items)
        //li.Attributes.Add("onclick", "javascript:ACHTogglePanelYes('" + rblINTEND_TO_RECEIVE_MCC.ClientID + "','" + pnlBankingInfo.ClientID + "');");
        //if (rblINTEND_TO_RECEIVE_MCC.SelectedIndex == 0) 
        divConfirm.Visible = rblINTEND_TO_RECEIVE_MCC.SelectedIndex == 0;
        divBankingInfo.Visible = rblINTEND_TO_RECEIVE_MCC.SelectedIndex == 0;
        pnlRemit.Style["display"] = rblINTEND_TO_RECEIVE_MCC.SelectedIndex == 0 ? "block" : "none";

        if (divBankingInfo.Attributes.CssStyle["display"] != "none" || divWaiverBankingInfo.Attributes.CssStyle["display"] != "none")
        {
            divConfirm.Attributes.Add("style", "display:block;");
        }

        //SAM768 - Show ACH page only if the provider agent has EFT agent role along with Enrollment agent role. Provider admin and power agents should have access to this page.
        bool isPowerAgentForSelctedProvAdmin = Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, this.WorkflowPage.RegistrationId);
        bool ifEFTAgent = (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent)
                 && Helper.IsUserInSubRoles(this.WorkflowPage.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EftAgentSubRoles));
        bool isProvAdmin = Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator);
        bool HideEFTPageProvAdmin = Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) && !(isPowerAgentForSelctedProvAdmin || ifEFTAgent || isProvAdmin);
        // OHPNM-5168
        if (Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInLTCInitialRevalidationRole(HttpContext.Current.User.Identity.Name)
            || HideEFTPageProvAdmin)
        {
            cantDisplayWarning.Visible = true;
            divConfirm.Visible = false;
            mainPanel.Visible = false;
        }

    }

    private void SetEditability()
    {
        this.rdlWaiverPaymentType.Items[1].Enabled = pnlWaiverPaymentType.Visible && TaxIdTypeID == CON.TaxIDType.SSN;

        if (!Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            Helper.SetReadOnly(rblINTEND_TO_RECEIVE_MCC, true);
            Helper.SetReadOnly(rblWISH_TO_CONTINUE_ACH, true);
            Helper.SetReadOnly(rdlWaiverPaymentType, true);
            Helper.SetReadOnly(chkConfirmCorrect, true);
            Helper.SetReadOnly(chkConfirm, true);
            Helper.SetReadOnly(chkBankInUS, true);
            Helper.SetReadOnly(this.chkFeeInformation, true);
            btnAddBankingInfo.Visible = btnAddVendorInfo.Visible = false;//   btnAddFeeInfo.Visible =
            Helper.SetReadOnly(ucBankingInfo, true);

            if (Helper.IsLoggedInUserInAdminRole())
            {
                Helper.SetReadOnly(ucEftContact, false);
                //btnAddEftContact.Visible = (grdEftContact.Rows.Count == 0);
            }
            else Helper.SetReadOnly(ucEftContact, true);

            // Admin and Accounting (FA1 and FA2) users can update the Vendor Information
            if (Helper.IsLoggedInUserInAdminRole() ||
                Helper.IsUserInAccountingRolls(HttpContext.Current.User.Identity.Name))
            {
                Helper.SetReadOnly(ucVendorInfo, false);
                btnAddVendorInfo.Visible = (grdVendorInfo.Rows.Count == 0);
            }
        }
    }


    private void SetupForFAOperator()
    {
        if (this.WorkflowPage.RegistrationId > 0)
        {
            bool isAdmin = Helper.IsLoggedInUserInAdminRole();
            bool isAcct = Helper.IsUserInAccountingRolls(HttpContext.Current.User.Identity.Name);
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet dsGN = svc.GetGroupNames(this.WorkflowPage.RegistrationId);
            if (Helper.HasRows(dsGN))
            {
                DataRow[] rows = dsGN.Tables[0].Select("GROUP_NAME LIKE 'faoperator%'");
                if (rows.Length > 0)
                {
                    if (isAdmin || isAcct)
                    {
                        Guid adminId = Helper.GetUserId("admin");
                    }
                    //bug 2202 - no one else should see that this action is pending when they look at the registration.
                    else
                    {
                        Guid adminId = Helper.GetUserId("admin");
                        svc.SaveRegistrationPageStatus(this.WorkflowPage.RegistrationId, CON.RegistrationPageType.ApplicationFee, null, CON.RegistrationProviderServicesStatusTypeId.Approved, adminId.ToString(), null, null, null, null, null, null, null, null, null, null);
                    }
                }
            }
        }
    }

    private void AchCheckForAgreements()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ACH_REQUEST");

        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            bool currentIntend = Helper.GetBool("INTEND_TO_RECEIVE_MCC", dr);
            bool currentWish = Helper.GetBool("WISH_TO_RECEIVE_ACH", dr);
            bool savingIntend = rblINTEND_TO_RECEIVE_MCC.SelectedIndex == 0;
            bool savingWish = rblINTEND_TO_RECEIVE_MCC.SelectedIndex == 0;

            //If user is saving both as yes and the values have changed from no, Agreements is invalidated.
            if (savingIntend && savingWish
                &&
                (savingIntend != currentIntend || savingWish != currentWish))
            {
                Registration.SetNodeStatusId(this.WorkflowPage.RegistrationId, CON.RegistrationPageType.Agreements, CON.RegistrationProviderStatusTypeId.NotComplete);
            }
        }
    }

    public override bool SaveData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        //if (stateName != "Nebraska")
        //{
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("ProcessId", this.WorkflowPage.WF_ProcessID.ToString());
        if (rblINTEND_TO_RECEIVE_MCC.SelectedIndex != -1)
            parms.Add("INTEND_TO_RECEIVE_MCC", (rblINTEND_TO_RECEIVE_MCC.SelectedIndex == 0 ? 1 : 0).ToString());

        if (rblINTEND_TO_RECEIVE_MCC.SelectedIndex != -1)
            parms.Add("WISH_TO_RECEIVE_ACH", (rblINTEND_TO_RECEIVE_MCC.SelectedIndex == 0 ? 1 : 0).ToString());
        if (rblWISH_TO_CONTINUE_ACH.SelectedIndex != -1)
            parms.Add("WISH_TO_CONTINUE_ACH", (rblWISH_TO_CONTINUE_ACH.SelectedIndex == 0 ? 1 : 0).ToString());
        parms.Add("FEE_SUBMITTED_FOR_PAYMENT", chkFeeInformation.Checked ? "1" : "0");
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("IS_BANK_OUTSIDE_US", chkBankInUS.Checked ? "1" : "0");

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        // Make sure the ID does not exist. Check to see if we added the REG_ACH_REQUEST via Banking information already.
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ACH_REQUEST");

        if (string.IsNullOrEmpty(hdnRegAchRequestID.Value))
        {
            
            if (Helper.HasRows(ds)) hdnRegAchRequestID.Value = Helper.GetString("REG_ACH_REQUEST_ID", ds.Tables[0].Rows[0]);
        }

        if (!string.IsNullOrEmpty(hdnRegAchRequestID.Value))
        {
            parms.Add("REG_ACH_REQUEST_ID", hdnRegAchRequestID.Value);
            // Update, if it's the same Process Id then we just need to update the data
            int regProcessId = Helper.GetInt("ProcessId", ds.Tables[0].Rows[0]);
            if (regProcessId == this.WorkflowPage.WF_ProcessID)
            {
                parms.Add("EFT_START_DATE", Helper.GetDateTime("EFT_START_DATE", ds.Tables[0].Rows[0]).ToString());
                parms.Add("EFT_END_DATE", Helper.GetDateTime("EFT_END_DATE", ds.Tables[0].Rows[0]).ToString());
               // parms.Add("SendEmailStatus", "1");
                psc.UpdateRegistrationDataTable("ACH_REQUEST", parms);
            }
            else
            {
                // The update Custom will create a new record sp we need to copy the bank info 
                parms.Add("BANK_NAME", Helper.GetString("BANK_NAME", ds.Tables[0].Rows[0]));
                parms.Add("ABA_NUMBER", Helper.GetString("ABA_NUMBER", ds.Tables[0].Rows[0]));
                parms.Add("ACCOUNT_NUMBER", Helper.GetString("ACCOUNT_NUMBER", ds.Tables[0].Rows[0]));
                parms.Add("ACCOUNT_TYPE_ID", Helper.GetInt("ACCOUNT_TYPE_ID", ds.Tables[0].Rows[0]).ToString());
                parms.Add("MODIFIED_STATUS_TYPE_ID", Helper.GetString("MODIFIED_STATUS_TYPE_ID", ds.Tables[0].Rows[0]).ToString());
                parms.Add("SendEmailStatus", "1");
                psc.UpdateRegistrationDataTable("ACH_REQUESTCustom", parms);
            }
            
        }
        else
        {
            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            // Insert
            psc.InsertRegistrationDataTable("ACH_REQUESTCustom", parms);
        }
        //}

        // Update the confirmation of the Remittance Address
        if (!string.IsNullOrEmpty(hdnRegServiceLocationID.Value))
        {
            parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("REG_SERVICE_LOCATION_ID", hdnRegServiceLocationID.Value);
            parms.Add("CONFIRM_CORRECT", "1");
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            psc.UpdateRegistrationDataTable("SERVICE_LOCATIONcustom", parms);
        }

        parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("ACH_ATTEST", chkConfirm.Checked ? "1" : "0");
        if (rdlWaiverPaymentType.SelectedIndex != -1)
        {
            parms.Add("MAX_FIN_DEBIT_IND", rdlWaiverPaymentType.SelectedIndex == 0 ? CON.WaiverPaymentType.DirectDeposit : CON.WaiverPaymentType.ReliaCard);
        }
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        psc.UpdateRegistration(parms);

        // Clear any exsiting ACH data if user selected 'No'
        if (rblINTEND_TO_RECEIVE_MCC.SelectedIndex == 1 && (grdBankingInfo.Rows.Count > 0 || grdEftContact.Rows.Count > 0 ))
        {
            psc.RemoveReceiveACHData(Int32.Parse(hdnRegAchRequestID.Value), Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        }
            
        return true;
    }
    public void SaveDataPS(int action)
    {
        if (divConfirm.Attributes.CssStyle["display"] != "none")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("ACH_ATTEST", chkConfirm.Checked ? "1" : "0");
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            psc.UpdateRegistration(parms);
        }
    }
    public string ValidateDataPS()
    {
        string rtn = string.Empty;

        if (divConfirm.Attributes.CssStyle["display"] != "none")
        {
            if (!chkConfirm.Checked) rtn = "* Providers are required to attest the ACH form.";
        }


        return rtn;
    }
    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    public override bool ValidateData()
    {
        DataRow dr = Registration.GetRegistration(this.WorkflowPage.RegistrationId);
        int IsUpdate = Convert.ToInt32(Helper.GetString("Workflow_Event_Type_Id", dr));
        bool isGood = true;
        DataSet ds = new DataSet();
        if (!Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) &&
            !Registration.SectionIsValidated(this.WorkflowPage.RegistrationId, MAXIMUS.Core.Libraries.Constants.SectionTypeID.PrimaryServiceAddress) && (IsUpdate != CON.WorkflowEventType.UpdateReg))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "*The Primary Service Address page is a prerequisite to this page. Please fill out the Primary Service Address page.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            return false;
        }
       
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        stateName = AppSettings.Get("StateName");

        if (stateName != "Nebraska")
        {
            if (rblINTEND_TO_RECEIVE_MCC.SelectedIndex == -1) AddError("* Select Yes or No for \"Do you intend to receive payments...\"", ref isGood);
        }

        if (rblINTEND_TO_RECEIVE_MCC.SelectedIndex == 0)
        {
            //check for required fields
            if (!GridDataValid())
            {
                isGood = false;
                SetVisibility();
                return false;
            }

            //FOR NE, all provider types must attest to this.
            if (!chkConfirm.Checked)
            {
                AddError("* Providers are required to attest the ACH form.", ref isGood);
            }
        }
        

        if (pnlWaiverPaymentType.Visible)
        {
            if (this.rdlWaiverPaymentType.SelectedIndex == -1)
            {
                AddError("* Please select a payment option.", ref isGood);
            }
        }
        if (!isGood) SetVisibility();
        return isGood;
    }
    private bool GridDataValid()
    {
        bool isGood = true;
        if (!this.WorkflowPage.IsWaiverServiceProvider ||
            (this.WorkflowPage.IsWaiverServiceProvider && rdlWaiverPaymentType.SelectedValue.ToString() == CON.WaiverPaymentType.DirectDeposit) ||
            (this.WorkflowPage.IsWaiverServiceProvider && !rdlWaiverPaymentType.Visible))
        {
            if (grdBankingInfo.Rows.Count < 1 && !chkBankInUS.Checked)
            {
                isGood = AddValidationErrorMessage("* Banking Information is required");
                return isGood;
            }
            if (grdEftContact.Rows.Count == 0)
            {
                isGood = AddValidationErrorMessage("* EFT Contact Information is required");
                return isGood;
            }

            //check for conversion providers
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ACH_REQUEST");
            DataRow dr;
            if (Helper.HasRows(ds) && !chkBankInUS.Checked)
            {
                dr = ds.Tables[0].Rows[0];
                if (string.IsNullOrEmpty(Helper.GetString("BANK_NAME", dr)))
                {
                    isGood = AddValidationErrorMessage("*Enter Bank Name");
                }
                if (string.IsNullOrEmpty(Helper.GetString("ABA_NUMBER", dr)))
                {
                    isGood = AddValidationErrorMessage("*Enter a 9 digit ACH Transit / ABA Number");
                }
                if (string.IsNullOrEmpty(Helper.GetString("ACCOUNT_NUMBER", dr)))
                {
                    isGood = AddValidationErrorMessage("*Enter Account Number");
                }
                if (string.IsNullOrEmpty(Helper.GetString("ACCOUNT_TYPE_ID", dr)))
                {
                    isGood = AddValidationErrorMessage("*Select Checking or Savings");
                }
                if (this.WorkflowPage.IsWaiverServiceProvider)
                {
                    if (string.IsNullOrEmpty(Helper.GetString("ACCOUNT_TYPE_ENTITY_ID", dr)))
                    {
                        isGood = AddValidationErrorMessage("*Select Account Type Entity");
                    }
                }
            }
        }
        if (this.WorkflowPage.IsWaiverServiceProvider && rdlWaiverPaymentType.SelectedValue.ToString() == CON.WaiverPaymentType.ReliaCard)
        {
            if (grdReliaCard.Rows.Count < 1)
            {
                isGood = AddValidationErrorMessage("* ReliaCard Information is required");
                return isGood;
            }
            //check for conversion providers
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ACH_RELIACARD");
            DataRow dr;
            if (Helper.HasRows(ds))
            {
                dr = ds.Tables[0].Rows[0];
                if (string.IsNullOrEmpty(Helper.GetString("FIRST_NAME", dr)))
                {
                    isGood = AddValidationErrorMessage("*Enter First Name");
                }
                if (string.IsNullOrEmpty(Helper.GetString("LAST_NAME", dr)))
                {
                    isGood = AddValidationErrorMessage("*Enter Last Name");
                }
                if (string.IsNullOrEmpty(Helper.GetString("STREET", dr)))
                {
                    isGood = AddValidationErrorMessage("*Enter Street");
                }
                if (string.IsNullOrEmpty(Helper.GetString("CITY", dr)))
                {
                    isGood = AddValidationErrorMessage("*Enter City");
                }
                if (string.IsNullOrEmpty(Helper.GetString("STATE", dr)))
                {
                    isGood = AddValidationErrorMessage("*Enter State");
                }
                if (string.IsNullOrEmpty(Helper.GetString("ZIPCODE", dr)))
                {
                    isGood = AddValidationErrorMessage("*Enter ZipCode");
                }
            }
        }
        return isGood;
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
    private void ShowPopup(BasePopupControl ctl, string title, string valGroup, int index, int viewIndex)
    {
        lblTitle.Text = title;
        btnSave.ValidationGroup = valGroup;
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        DataRow dr = null;
        if (index >= 0 && ctl.DataList.Rows.Count > 0) dr = ctl.DataList.Rows[index];
        ctl.LoadData(dr);
        mltPopup.ActiveViewIndex = viewIndex;
        mpe.Show();
    }

    private void SetButtons(string commandName)
    {
        // Save is disabled by default
        btnSave.Visible = false;
        btnCancel.Text = "Close";

        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            btnSave.Visible = true;
            btnCancel.Text = "Cancel";
        }
    }

    private void WorkRow(string commandName, int index)
    {
        SetButtons(commandName);
        SetVisibility();
        switch (commandName)
        {
            case "BankingInfo":
                ShowPopup(ucBankingInfo, "Banking and EFT Contact Information", "valBankingInfo", index, 0);
                break;
            case "FeeInformation":
                ShowPopup(ucFeeinformation, "Fee Information", "valFeeInformation", index, 1);
                break;
            case "EftContact":
                ShowPopup(ucEftContact, "EFT Contact Information", "valEftContact", index, 2);
                break;
            case "VendorInfo":
                ShowPopup(ucVendorInfo, "Vendor Information", "valVendorInfo", index, 3);
                break;
            case "ReliaCard":
                ShowPopup(ucReliaCard, "ReliaCard Authorization", "valReliaCard", index, 4);
                //ShowPopup(ucReliaCard, "ReliaCard Authorization", "valReliaCard", index, 4);
                break;
            default:
                break;
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        WorkRow(e.CommandName, Convert.ToInt32(e.CommandArgument));
    }

    protected void btnAdd_Click(object sender, CommandEventArgs e)
    {
        WorkRow(e.CommandName, -1);
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        int opt = 0;
        string title = string.Empty;
        switch (e.CommandName)
        {
            case "BankingInfo":
                opt = 1;
                title = "Banking and EFT Contact Information";
                break;
            case "FeeInformation":
                opt = 1;
                title = "Fee Information";
                break;
            case "EftContact":
                opt = 2;
                title = "Eft Contact";
                break;
            case "VendorInfo":
                opt = 3;
                title = "Vendor Information";
                break;
        }

        if (opt > 0)
        {
            SetButtons("History");
            lblTitle.Text = title + " History";
            ucHistory.LoadData(opt);
            mltPopup.ActiveViewIndex = 5;
			btnSave.Visible= false;
			mpe.Show();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            mpe.Show();
            return;
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        switch (mltPopup.ActiveViewIndex)
        {
            case 0:
                bool isGood = ucBankingInfo.ValidatenbABANumber();
                if (isGood)
                {
                    ucBankingInfo.SaveData();

                    parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                    //?? this field isn't on the pop up why saving here?
                    //this field says whether it is a reliacard or bankinginfo
                    if (rdlWaiverPaymentType.SelectedValue.ToString() == CON.WaiverPaymentType.DirectDeposit)
                    {
                        parms.Add("MAX_FIN_DEBIT_IND", CON.WaiverPaymentType.DirectDeposit);
                    }
                    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("ACH_ATTEST", chkConfirm.Checked ? "1" : "0");
                    psc.UpdateRegistration(parms);
                    
                    //SetVisibility();
                    //this.InvalidateAgreements(this, new EventArgs());

                    //Saving Contact Details
                    ucBankingInfo.EFTSaveData();
					LoadBankingInfo();
					//LoadEftContact();
					SetVisibility();
					this.InvalidateAgreements(this, new EventArgs());
				}
                else
                {
                    mpe.Show();
                    return;
                }
                break;
            case 1:
                btnSave.ValidationGroup = "valFeeInformation";
                ucFeeinformation.SaveData();
                LoadFeeinformation();
                SetVisibility();
                this.InvalidateAgreements(this, new EventArgs());
                break;
            case 2:
                ucEftContact.SaveData();
                LoadEftContact();
                SetVisibility();
                this.InvalidateAgreements(this, new EventArgs());
                break;
            case 3:
                ucVendorInfo.SaveData();
                LoadVendorInfo();
                SetVisibility();
                // Only invalidate if NOT provider services
                if (!Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName)) this.InvalidateAgreements(this, new EventArgs());
                break;
            case 4:
                ucReliaCard.SaveData();
                parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms.Add("ACH_ATTEST", chkConfirm.Checked ? "1" : "0");
                //this field says whether it is a reliacard or bankinginfo
                if (rdlWaiverPaymentType.SelectedValue.ToString() == CON.WaiverPaymentType.ReliaCard)
                {
                    parms.Add("MAX_FIN_DEBIT_IND", CON.WaiverPaymentType.ReliaCard);
                }
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                psc.UpdateRegistration(parms);
                LoadReliaCard();
                SetVisibility();
                this.InvalidateAgreements(this, new EventArgs());
                break;
        }
        btnSave.Enabled = true;
    }

    protected void grdEftContact_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int idx = 1;
            e.Row.Cells[idx].Text = Helper.FormatPhone(e.Row.Cells[idx].Text);
        }
    }

    protected void grdBankingInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int idx = 3;
            string accountNumber = e.Row.Cells[idx].Text;
            if (Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) && !Helper.IsUserInAccountingRolls(HttpContext.Current.User.Identity.Name))
            {
                // Mask it if not in FA1 or FA2
                accountNumber = Helper.MaskValue(accountNumber, 4);
            }
            e.Row.Cells[idx].Text = accountNumber;
        }

        //Contact Details Phone Number
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			int idx = 5;
			e.Row.Cells[idx].Text = Helper.FormatPhone(e.Row.Cells[idx].Text);
		}
	}

    protected void grdReliaCard_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected string MaskAccountNumber(object obj)
    {
        string acctNum = string.Empty;

        if (obj is string)
            acctNum = obj.ToString();

        for (int i = 0; i < acctNum.Length - 4; i++)
            acctNum = acctNum.Replace(acctNum[i], '*');

        return acctNum;
    }

    public override bool HasInputValue()
    {
        bool isRequired = false;
        if (rblINTEND_TO_RECEIVE_MCC.SelectedIndex != -1)
            isRequired = true;
        return isRequired;
    }

    public override void LoadData(DataRow dr = null)
    {

    }

    public override string ValidationGroup
    {
        get { return "valACHAuthorization"; }
    }

    public override string Title
    {
        get { return "ACH Authorization Information"; }
    }

    public override string IdText
    {
        get { return "ucACHAuthorization_" + this.WorkflowPage.RegistrationId; }
    }

    private void AddError(string errorMessage, string validationGroup, ref bool isGood)
    {
        CustomValidator val = new CustomValidator()
        {
            IsValid = false,
            ErrorMessage = errorMessage,
            ValidationGroup = validationGroup
        };
        this.Page.Validators.Add(val);
        isGood = false;
    }

    protected void chkBankInUS_CheckedChanged(object sender, EventArgs e)
    {
        if (chkBankInUS.Checked)
        {
            string validationGroup = "valProviderInfoHeader";
            bool isGood = true;
            AddError("Bank must be within the United States. Please update with US Banking information.", validationGroup, ref isGood);
        }
    }

    protected void rblIntendToReceive_Changed(object sender, EventArgs e)
    {
        divConfirm.Visible = rblINTEND_TO_RECEIVE_MCC.SelectedIndex == 0;
        divBankingInfo.Visible = rblINTEND_TO_RECEIVE_MCC.SelectedIndex == 0;
        pnlRemit.Style["display"] = rblINTEND_TO_RECEIVE_MCC.SelectedIndex == 0 ? "block" : "none";
    }
}