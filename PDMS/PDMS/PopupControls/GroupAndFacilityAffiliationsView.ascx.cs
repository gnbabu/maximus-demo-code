using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Text;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using MAXIMUS.Models.Data.PDMS;
using Telerik.Web.UI;
using System.Web.Security;
using System.DirectoryServices;
using MAXIMUS.DataExchange.PDMS.TN_MMIS_Service_Test;
using MAXIMUS.Core.Libraries;
using System.Data.SqlClient;

public partial class Pages_GroupAndFacilityAffiliationsView : BaseSectionControl, IGroupAndFacilityAffiliationsView
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public delegate void SaveDataEventHandler();
    public event SaveDataEventHandler SaveDataEvent;
    private string sortDirection = "";
    private string sortExpression = "";
    private DataTable dtConfirmed = null;
    private DataTable dtPending = null;
    private DataTable dtHealthCare = null;

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

    #region Section
    private enum PopupName { GroupAffiliations = 0, GroupAffiliationsHistory = 1 };
    #endregion
    private GroupAndFacilityAffiliationsPresenter _presenter;

    public GroupAndFacilityAffiliationsPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new GroupAndFacilityAffiliationsPresenter(this);
            }

            return _presenter;
        }
    }

    public GroupAndFacilityAffiliations Model { get; set; }

    public System.EventHandler InvalidateAgreements;

    private void SetButtons()
    {

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        hdnRegId.Value = this.WorkflowPage.RegistrationId.ToString();
        SetButtons();
        this.ucPendingGroupAffiliations.CancelEvent += new PopupControls_PendingGroupAffiliations.CancelEventHandler(View_CancelPendingGroupAffiliations);
        this.ucPendingGroupAffiliations.ValidationEvent += new PopupControls_PendingGroupAffiliations.ValidationEventHandler(KeepPopupOpen);
        this.ucPendingGroupAffiliations.SaveEvent += new PopupControls_PendingGroupAffiliations.SaveEventHandler(View_SavePendingGroupAffiliations);
        this.ucHealthCareAffiliations.CancelEvent += new PopupControls_HealthCareAffiliations.CancelEventHandler(View_CancelHealthCareAffiliations);
        this.ucHealthCareAffiliations.ValidationEvent += new PopupControls_HealthCareAffiliations.ValidationEventHandler(KeepPopupOpenHealthCare);
        this.ucHealthCareAffiliations.SaveEvent += new PopupControls_HealthCareAffiliations.SaveEventHandler(View_SaveHealthCareAffiliations);

        //OHPNM-3487 - on click of View provider file Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                ImageButton1.Visible = false;
            }
        }
        //OHPNM-19048
        List<SqlParameter> sqlParms1 = new List<SqlParameter>();
        sqlParms1.Add(SqlParms.CreateParameter("RegID", DbType.Int32, this.WorkflowPage.RegistrationId.ToString(), false));
        DataSet dsProv = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms1, "RegProvData");
        if (Helper.HasRows(dsProv))
        {
            this.WorkflowPage.IsCredentialingProvider = Helper.GetBool("IsCredentialingProvider", dsProv.Tables[0].Rows[0]);
        }
        //this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.NewReg
        if ((Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) || Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, this.WorkflowPage.RegistrationId))
              && this.WorkflowPage.IsCredentialingProvider)
        {
            credentialingDelegatesCheckBox.Enabled = true;
            Check_CredentialRTP();
        }
        //OHPNM-15556:Enrollment agent can check Delegated Credentialing Check Box at Revalidation - Affiliation page
        else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent) == true && this.WorkflowPage.IsCredentialingProvider)
        {

            if (Helper.IsUserInSubRoles(Convert.ToInt32(this.WorkflowPage.RegistrationId.ToString()), Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EnrollmentAgentSubRole))
            {
                credentialingDelegatesCheckBox.Enabled = true;
                Check_CredentialRTP();
            }

        }
        //OHPNM- 19954
        else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ODMCredentialingSupervisor) && this.WorkflowPage.IsCredentialingProvider)
        {
            credentialingDelegatesCheckBox.Enabled = true;
        }
        ODMCredentialingCheckedValue();
    }

    private void Check_CredentialRTP()
    {
        List<SqlParameter> sqlParms1 = new List<SqlParameter>();
        sqlParms1.Add(SqlParms.CreateParameter("RegID", DbType.Int32, this.WorkflowPage.RegistrationId.ToString(), false));
        DataSet dsRTPCred = DataAccess.ExecuteStoredProcedure("usp_CheckRegistrationReturnedByCredentialing", sqlParms1, "RegCredData");
        bool isRTP_Frm_Credential = ObjectControllerHelper.HasRows(dsRTPCred) && ObjectControllerHelper.GetString("Return_Status", dsRTPCred.Tables[0].Rows[0]) == "true" ? true : false;
        if (isRTP_Frm_Credential && (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent)))
        {
            credentialingDelegatesCheckBox.Enabled = false;
            grdAssignedDelegates.Enabled = false;
        }
    }
    private void View_CancelHealthCareAffiliations()
    {
        this.mpe2.Hide();
    }
    private void View_CancelPendingGroupAffiliations()
    {
        this.mpe.Hide();
    }
    private void View_SavePendingGroupAffiliations()
    {
        this.upAff.Update();
        this.LoadData();
        this.mpe.Hide();
    }
    private void View_SaveHealthCareAffiliations()
    {
        this.upAff.Update();
        this.LoadData();
        this.mpe2.Hide();
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Group, Facility & Hospital Affiliations";
    }

    private void BindAssignDelegates()
    {
        if (rcbAssignDelegates.Items.Count == 0)
        {
            DataSet ds = svc.SelectDelegates();
            if (Helper.HasRows(ds))
            {
                rcbAssignDelegates.Items.Clear();
                foreach (DataRow drRCB in ds.Tables[0].Rows)
                {
                    rcbAssignDelegates.Items.Insert(0, new RadComboBoxItem(drRCB["DELEGATES_NAME"].ToString(), drRCB["DELEGATES_ID"].ToString()));
                }
            }
        }
    }
    public override void LoadControlData()
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DELEGATE_CREDENTIALING");
        DataTable dtDelegateCredentialing = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        this.DataList = dtDelegateCredentialing;
        BindAssignDelegates();
        if (Helper.HasRows(this.DataList))
            this.LoadData(this.DataList.Rows[0]);
        else
            this.LoadData(null);
    }

    public override bool ValidateData()
    {
        bool rtn = true;
        DataSet dsPageRequired = svc.GetIndvAffPageRequired(this.WorkflowPage.RegistrationId);
        DataTable dtPageRequired_PA = Helper.HasRows(dsPageRequired) ? dsPageRequired.Tables[0] : null;
        DataTable dtPageRequired_HA = dsPageRequired.Tables.Count > 1 ? dsPageRequired.Tables[1] : null;
        if (Helper.HasRows(dtPageRequired_PA))
        {
            DataRow drPageRequired = dtPageRequired_PA.Rows[0];
            string paRequired = Helper.GetString("PA_REQUIRED", drPageRequired);
            if (!string.IsNullOrEmpty(paRequired))
            {
                if (paRequired.Equals("1"))
                {
                    if (grdPendingGroupAffiliations.Rows.Count == 0 && grdConfirmedGroupAffiliations.Rows.Count == 0)
                    {
                        AddError("At least 1 entry is required for Pending Affiliation", ref rtn);
                        return rtn;
                    }
                }
            }
        }
        if (Helper.HasRows(dtPageRequired_HA))
        {
            DataRow drPageRequired = dtPageRequired_HA.Rows[0];
            string haRequired = Helper.GetString("HA_REQUIRED", drPageRequired);
            if (!string.IsNullOrEmpty(haRequired))
            {
                if (haRequired.Equals("1"))
                {
                    if (grdHealthCareAffiliates.Rows.Count == 0)
                    {
                        AddError("At least 1 entry is required for Hospital Affiliation", ref rtn);
                        return rtn;
                    }
                }
            }
        }

        if (rblDelegateCredential.SelectedIndex != -1)
        {
            if (rblDelegateCredential.SelectedItem.Value == "1" && rcbAssignDelegates.CheckedItems.Count < 1)
            {
                AddError("This is a required field when provider participates in Delegated Credentialing is selected as Yes.", ref rtn);
                return rtn;
            }
        }
        else
        {
            AddError("Does Provider participate in Delegated Credentialing? is required.", ref rtn);
            return rtn;
        }

        DataSet dsBehvPageRequired = svc.GetBehaviourHealthAffRequired(this.WorkflowPage.RegistrationId);
        if (!Helper.HasRows(dsBehvPageRequired))
        {
            AddError("Affiliation with a group/organization is required for enrollment, please add an affiliation.", ref rtn);
            return rtn;
        }

        return rtn;
    }
    public override void LoadData(DataRow dr = null)
    {
        DataRow provRow = Registration.GetProviderInfo(this.WorkflowPage.RegistrationId);
        string provider_type_id = Helper.GetString("MMIS_Provider_Type_ID", provRow);
        GroupAndFacilityAffiliations model = new GroupAndFacilityAffiliations();
        model.RegID = this.WorkflowPage.RegistrationId;

        presenter.GetRegPendingAffiliation(model);
        presenter.GetRegConfirmedAffiliation(model);
        presenter.GetRegHealthCareFacilityAffiliation(model);
        presenter.GetRegAssignedDelegates(model);
        btnAdd.Visible = !Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName);
        if (Helper.IsUserInODMCredentialingRoleIndvAff(HttpContext.Current.User.Identity.Name))
        {
            pnlDelegateCredentialingProvider.Visible = pnlDelegateCredentialingProvider.Enabled = pnlDelegateCredentialingODM.Visible = pnlDelegateCredentialingODM.Enabled = true;
            if (dr != null)
            {
                if (!string.IsNullOrEmpty(Helper.GetString("DELEGATE_CREDENTIALING_REQUIRED", dr)))
                {
                    bool reqDelegate = Helper.GetBool("DELEGATE_CREDENTIALING_REQUIRED", dr);
                    rblDelegateCredential.SelectedValue = reqDelegate ? "1" : "0";
                    rcbAssignDelegates.Enabled = reqDelegate;
                    if (reqDelegate)
                    {
                        DataSet dsDelegateIDS = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DELEGATE_CREDENTIALING");
                        DataTable dtDelegateIDS = Helper.HasRows(dsDelegateIDS) ? dsDelegateIDS.Tables[1] : null;
                        this.DataList = dtDelegateIDS;
                        if (Helper.HasRows(this.DataList))
                        {
                            DataRow drdtDelegateIDS = this.DataList.Rows[0];
                            string delegateIDs = Helper.GetString("DELEGATE_IDS", drdtDelegateIDS);
                            if (!string.IsNullOrEmpty(delegateIDs))
                            {
                                string[] idarray = delegateIDs.Split(',');
                                foreach (var id in idarray)
                                {

                                    var comboItem = rcbAssignDelegates.FindItemByValue(id.ToString().Trim());

                                    if (comboItem != null)
                                    {
                                        comboItem.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                }


            }
        }
        else
        {
            pnlDelegateCredentialingProvider.Visible = true;
            pnlDelegateCredentialingProvider.Enabled = pnlDelegateCredentialingODM.Visible = pnlDelegateCredentialingODM.Enabled = false;
        }
        //only load data when in ODM Role

    }
    public override bool SaveData()
    {
        bool rtn = true;

        // OHPNM-2229          
        if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 && grdPendingGroupAffiliations.Rows.Count == 0 && grdConfirmedGroupAffiliations.Rows.Count == 0 && grdHealthCareAffiliates.Rows.Count == 0 && grdAssignedDelegates.Rows.Count == 0)
        {
            // this page is required, but there aren't any items, so don't let them 'save' the data on the screen
            rtn = false;
        }

        return rtn;
    }
    private void KeepPopupOpenHealthCare()
    {
        this.mpe2.Show();
    }
    private void KeepPopupOpen()
    {
        this.mpe.Show();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

    public void SetRegPendingAffiliation(DataSet ds)
    {
        dtPending = ds.Tables[0];
        grdPendingGroupAffiliations.DataSource = ds;
        grdPendingGroupAffiliations.DataBind();
    }
    public void SetRegConfirmedAffiliation(DataSet ds)
    {
        dtConfirmed = ds.Tables[0];
        grdConfirmedGroupAffiliations.DataSource = ds;
        grdConfirmedGroupAffiliations.DataBind();
    }
    public void SetRegHealthCareFacilityAffiliation(DataSet ds)
    {
        dtHealthCare = ds.Tables[0];
        grdHealthCareAffiliates.DataSource = ds;
        grdHealthCareAffiliates.DataBind();
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSpecialist) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSupervisor))
        {
            ImageButton1.Visible = false;
        }
        else
        {
            ImageButton1.Visible = true;
        }
    }
    public void SetRegAssignedDelegates(DataSet ds)
    {
        grdAssignedDelegates.DataSource = ds;
        if (Helper.HasRows(ds) && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ODMCredentialingSpecialist))
        {
            credentialingDelegatesCheckBox.Enabled = true;
        }
        grdAssignedDelegates.DataBind();
    }

    protected void grdPendingGroupAffiliations_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Sort"))
        {
            return;
        }

        int index = Convert.ToInt32(e.CommandArgument);
        int regPendingAffiliationID = 0;
        if (e.CommandName == "PendingGroupAffiliations" || e.CommandName == "PendingDeleteAffiliation")
            regPendingAffiliationID = string.IsNullOrEmpty(this.grdPendingGroupAffiliations.DataKeys[index].Values["REG_PENDING_AFFILIATION_ID"].ToString()) ? 0 : (int)this.grdPendingGroupAffiliations.DataKeys[index].Values["REG_PENDING_AFFILIATION_ID"];


        switch (e.CommandName)
        {
            case "PendingGroupAffiliations":
                lblTitle.Text = "Edit Group Affiliation";
                lblTitle1.Text = "";
                DataRow provRow = Registration.GetProviderInfo(this.WorkflowPage.RegistrationId);
                string provider_type_id = Helper.GetString("MMIS_Provider_Type_ID", provRow);


                this.ucPendingGroupAffiliations.LoadData(regPendingAffiliationID);
                mltPopup.ActiveViewIndex = 0;
                mpe.Show();
                break;
            case "PendingDeleteAffiliation":
                if (regPendingAffiliationID > 0)
                {
                    ucPendingGroupAffiliations.DeleteRegPendingAffiliation(regPendingAffiliationID);
                    this.LoadData();
                }
                break;

            default:
                break;
        }

    }

    /*private void ShowPopup(BasePopupControl ctl, string title, int regPendingAffiliationID, int viewIndex)
    {
        lblTitle.Text = title;
        //btnSave.ValidationGroup = valGroup;
        DataRow dr = null;
        //if (index >= 0 && ctl.DataList.Rows.Count > 0) dr = ctl.DataList.Rows[index];
        ctl.LoadData(regPendingAffiliationID);
        mltPopup.ActiveViewIndex = viewIndex;
        mpe.Show();
    }*/

    protected void grdPendingGroupAffiliations_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPendingGroupAffiliations.PageIndex = e.NewPageIndex;
        GroupAndFacilityAffiliations model = new GroupAndFacilityAffiliations();
        model.RegID = this.WorkflowPage.RegistrationId;

        presenter.GetRegPendingAffiliation(model);
    }

    protected void grdAssignedDelegates_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdAssignedDelegates.PageIndex = e.NewPageIndex;
        GroupAndFacilityAffiliations model = new GroupAndFacilityAffiliations();
        model.RegID = this.WorkflowPage.RegistrationId;
        presenter.GetRegAssignedDelegates(model);
    }

    protected void grdConfirmedGroupAffiliations_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdConfirmedGroupAffiliations.PageIndex = e.NewPageIndex;
        GroupAndFacilityAffiliations model = new GroupAndFacilityAffiliations();
        model.RegID = this.WorkflowPage.RegistrationId;
        presenter.GetRegConfirmedAffiliation(model);
    }

    protected void grdHealthCareAffiliates_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdHealthCareAffiliates.PageIndex = e.NewPageIndex;
        GroupAndFacilityAffiliations model = new GroupAndFacilityAffiliations();
        model.RegID = this.WorkflowPage.RegistrationId;
        presenter.GetRegHealthCareFacilityAffiliation(model);
    }

    protected void btnAdd_Click(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "PendingGroupAffiliations")
        {
            ucPendingGroupAffiliations.InitializeFields();
            mltPopup.ActiveViewIndex = 0;
            lblTitle1.Text = "";
            lblTitle.Text = "Group Affiliation";
            DataRow provRow = Registration.GetProviderInfo(this.WorkflowPage.RegistrationId);
            string provider_type_id = Helper.GetString("MMIS_Provider_Type_ID", provRow);

            mpe.Show();
        }
        else if (e.CommandName == "HealthCareAffiliations")
        {
            ucHealthCareAffiliations.InitializeFields();
            MultiView1.ActiveViewIndex = 0;
            lblTitle.Text = "";
            lblTitle1.Text = "Hospital Affiliation";
            mpe2.Show();
        }
    }
    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {

    }
    protected void grdConfirmedGroupAffiliations_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void grdHealthCareAffiliates_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Sort"))
        {
            return;
        }

        int index = Convert.ToInt32(e.CommandArgument);

        int regHealthCareAffiliationID = 0;
        if (e.CommandName == "HealthCareAffiliations" || e.CommandName == "DeleteHEalthCareAffiliation")
            regHealthCareAffiliationID = string.IsNullOrEmpty(this.grdHealthCareAffiliates.DataKeys[index].Values["REG_HEALTH_CARE_FACILITY_AFFILIATION_ID"].ToString()) ? 0 : (int)this.grdHealthCareAffiliates.DataKeys[index].Values["REG_HEALTH_CARE_FACILITY_AFFILIATION_ID"];

        switch (e.CommandName)
        {

            case "HealthCareAffiliations":
                lblTitle1.Text = "Edit Health Care Facility Affiliation";
                lblTitle.Text = "";
                //ShowPopup(PendingGroupAffiliations1, "Edit Pending Affiliation", regPendingAffiliationID, 0);

                this.ucHealthCareAffiliations.LoadData(regHealthCareAffiliationID);
                MultiView1.ActiveViewIndex = 0;
                this.mpe2.Show();
                break;
            case "DeleteHEalthCareAffiliation":
                if (regHealthCareAffiliationID > 0)
                {
                    ucHealthCareAffiliations.DeleteRegHealthCareAffiliation(regHealthCareAffiliationID);
                    this.LoadData();
                }
                break;
            default:
                break;
        }
    }

    protected void rblDelegateCredential_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblDelegateCredential.SelectedItem.Value == "1")
        {
            rcbAssignDelegates.Enabled = true;
        }
        else
        {
            rcbAssignDelegates.Enabled = false;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ValidateData())
        {

            bool val = SaveDelegateCredentialingData();
        }
        if (grdAssignedDelegates.Rows.Count > 0 && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSpecialist)
)
        {
            credentialingDelegatesCheckBox.Enabled = true;
        }
    }

    private void Set_hidID(DataRow row)
    {
        if (row == null)
        {
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DELEGATE_CREDENTIALING");
            if (Helper.HasRows(ds))
            {
                hidID.Value = ds.Tables[0].Rows[0]["REG_DELEGATE_CREDENTIALING_ID"].ToString();
            }
        }
        else
        {
            hidID.Value = row["REG_DELEGATE_CREDENTIALING_ID"].ToString();
        }
    }
    private void ODMCredentialingCheckedValue()
    {
        int REG_ID = Convert.ToInt32(this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = svc.SelectODMCredentialingIsChecked(REG_ID);
        string odmCredDelegatesIsChecked = ds.Tables[0].Rows[0]["ODM_Credentialing_Delegates_IsChecked"].ToString();

        if (odmCredDelegatesIsChecked == "True" && grdAssignedDelegates.Rows.Count > 0)
        {
            credentialingDelegatesCheckBox.Checked = true;
        }
        else if (odmCredDelegatesIsChecked == "True" || grdAssignedDelegates.Rows.Count > 0)
        {
            credentialingDelegatesCheckBox.Checked = true;
        }
        else if (odmCredDelegatesIsChecked == "True" && grdAssignedDelegates.Rows.Count == 0)
        {
            credentialingDelegatesCheckBox.Checked = true;
        }

        ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DELEGATE_CREDENTIALING");
        DataTable dtDelegateCredentialing = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        if (dtDelegateCredentialing != null)
        {
            this.DataList = dtDelegateCredentialing;
            DataRow dr = this.DataList.Rows[0];
            if (dr != null)
            {
                if (!string.IsNullOrEmpty(Helper.GetString("DELEGATE_CREDENTIALING_REQUIRED", dr)))
                {
                    bool reqDelegate = Helper.GetBool("DELEGATE_CREDENTIALING_REQUIRED", dr);
                    if (reqDelegate)
                    {
                        if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator) || Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, this.WorkflowPage.RegistrationId))
                        {
                            credentialingDelegatesCheckBox.Enabled = false;
                        }
                        //OHPNM-15556:Enrollment agent can check Delegated Credentialing Check Box at Revalidation - Affiliation page
                        else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent) == true)
                        {
                            if (Helper.IsUserInSubRoles(Convert.ToInt32(this.WorkflowPage.RegistrationId.ToString()), Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EnrollmentAgentSubRole))
                            {
                                credentialingDelegatesCheckBox.Enabled = false;
                            }
                        }
                        //OHPNM-OHPNM-18968
                        else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ODMCredentialingSpecialist) || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ODMCredentialingSupervisor))
                        {
                            credentialingDelegatesCheckBox.Enabled = true;
                        }
                        else
                        {
                            credentialingDelegatesCheckBox.Enabled = false;
                        }
                    }
                }
            }
        }
    }

    private bool SaveDelegateCredentialingData()
    {
        Page.Validate("valGroupAndFacilityAffiliations");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valGroupAndFacilityAffiliations") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }
        if (string.IsNullOrEmpty(hidID.Value))
        {
            Set_hidID(null);
        }
        var rcbAssignDelegatesCollection = rcbAssignDelegates.CheckedItems;
        var rcbAssignDelegatesSelectedValues = string.Empty;
        if (rcbAssignDelegatesCollection.Count != 0)
        {

            foreach (var item in rcbAssignDelegatesCollection)
            {
                rcbAssignDelegatesSelectedValues = rcbAssignDelegatesSelectedValues + item.Value + ",";
            }
            rcbAssignDelegatesSelectedValues = rcbAssignDelegatesSelectedValues.Remove(rcbAssignDelegatesSelectedValues.LastIndexOf(","));

        }

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("DELEGATE_CREDENTIALING_REQUIRED", rblDelegateCredential.SelectedValue.ToString());
        if (rblDelegateCredential.SelectedValue.Equals("1"))
        {
            parms.Add("DELEGATE_IDS", rcbAssignDelegatesSelectedValues);
        }
        else
        {
            parms.Add("DELEGATE_IDS", "");
        }

        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
        parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "DELEGATE_CREDENTIALINGcustom", parms);
        this.LoadData();
        return true;
    }

    protected void grdHealthCareAffiliates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int index = 3;
            if (e.Row.Cells[index].Text == "True")
                e.Row.Cells[index].Text = "Yes";
            else
                e.Row.Cells[index].Text = "No";
        }
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSupervisor))
        {
            LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
            if (btnEdit != null)
            {
                btnEdit.Visible = false;
            }
            LinkButton btnDel = (LinkButton)e.Row.FindControl("btnDelete");
            if (btnDel != null)
            {
                btnDel.Visible = false;
            }
        }

        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSpecialist))
        {
            LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
            if (btnEdit != null)
            {
                btnEdit.Visible = true;
            }
            LinkButton btnDel = (LinkButton)e.Row.FindControl("btnDelete");
            if (btnDel != null)
            {
                btnDel.Visible = false;
            }
        }
    }

    protected void grdAssignedDelegates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int index = 2;
        if (Helper.IsUserInODMCredentialingRoleIndvAff(HttpContext.Current.User.Identity.Name))
        {
            grdAssignedDelegates.Columns[index].Visible = true;
        }
        else
        {
            grdAssignedDelegates.Columns[index].Visible = false;
        }

    }

    protected void grdAssignedDelegates_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Sort"))
        {
            return;
        }

        int index = Convert.ToInt32(e.CommandArgument);
        int regAssignedDelegateID = 0;
        if (e.CommandName == "DeleteAssignedDelegate")
            regAssignedDelegateID = string.IsNullOrEmpty(this.grdAssignedDelegates.DataKeys[index].Values["REG_DELEGATE_CREDENTIALING_ID"].ToString()) ? 0 : (int)this.grdAssignedDelegates.DataKeys[index].Values["REG_DELEGATE_CREDENTIALING_ID"];


        switch (e.CommandName)
        {
            case "DeleteAssignedDelegate":
                if (regAssignedDelegateID > 0)
                {
                    DeleteAssignedDelegate(regAssignedDelegateID);
                    this.LoadData();
                }
                break;

            default:
                break;
        }
    }

    public void DeleteAssignedDelegate(int regAssignedDelegateID)
    {
        if (regAssignedDelegateID > 0)
            svc.DeleteAssignedDelegateByID(regAssignedDelegateID);
    }

    public override string ValidationGroup
    {
        get { return "valGroupAndFacilityAffiliations"; }
    }

    public override string Title
    {
        get { return "Group And Facility Affiliations"; }
    }

    public override string IdText
    {
        get { return "ucGroupAndFacility_" + this.WorkflowPage.RegistrationId; }
    }
    public override bool HasInputValue()
    {
        bool rtn = false;
        if (grdHealthCareAffiliates.Rows.Count > 0 || grdPendingGroupAffiliations.Rows.Count > 0)
            rtn = true;
        else if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 0)
            rtn = true; // OHPNM-2417 - tell the powers that be that this page is ok if it's not required, otherwise you can't get the green checkmark

        return rtn;
    }
    //public bool HasInputValue()
    //{
    //    bool rtn = false;
    //    if (grdHealthCareAffiliates.Rows.Count > 0 || grdPendingGroupAffiliations.Rows.Count > 0)
    //        rtn = true;
    //    return rtn;
    //}
    protected string FormatAddress(object add1, object add2, object city, object state, object zip, object ext_zip, object phone)
    {

        return Helper.GetFormattedAddress(Helper.ConvertNull(add1).ToString(), Helper.ConvertNull(add2).ToString(), Helper.ConvertNull(city).ToString(), Helper.ConvertNull(state).ToString(), Helper.ConvertNull(zip).ToString(), Helper.ConvertNull(ext_zip).ToString(), Helper.ConvertNull(phone).ToString());
    }
    protected string GetFullAddress(object add1, object add2, object city, object state)
    {

        return Helper.GetFormattedAddress(Helper.ConvertNull(add1).ToString(), Helper.ConvertNull(add2).ToString(), Helper.ConvertNull(city).ToString(), Helper.ConvertNull(state).ToString(), null, null, null);
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valGroupAndFacilityAffiliations";
        this.Page.Validators.Add(val);
        isGood = false;
    }
    protected void chkVerifiedCredDelegate_CheckedChanged(object sender, EventArgs e)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("ODM_Credentialing_Deligates_IsChecked", credentialingDelegatesCheckBox.Checked ? "1" : "0");
        parms.Add("ODM_Credentialing_Delegates_Changed", "1");
        svc.UpdateRegistrationDataWithParams("usp_update_reg_provider_odm_credentialing_delegates_checked", parms);
    }

    protected void grdPendingGroupAffiliations_Sorting(object sender, GridViewSortEventArgs e)
    {
        SetSortDirection();
        sortExpression = e.SortExpression + " " + sortDirection;
        ViewState["SortExpression"] = sortExpression;
        dtPending.DefaultView.Sort = sortExpression;
        dtPending = dtPending.DefaultView.ToTable(true);

        grdPendingGroupAffiliations.DataSource = dtPending;
        grdPendingGroupAffiliations.DataBind();

    }

    protected void grdConfirmedGroupAffiliations_Sorting(object sender, GridViewSortEventArgs e)
    {
        SetSortDirection();
        sortExpression = e.SortExpression + " " + sortDirection;
        ViewState["SortExpression"] = sortExpression;
        dtConfirmed.DefaultView.Sort = sortExpression;
        dtConfirmed = dtConfirmed.DefaultView.ToTable(true);

        grdConfirmedGroupAffiliations.DataSource = dtConfirmed;
        grdConfirmedGroupAffiliations.DataBind();

    }

    protected void grdHealthCareAffiliates_Sorting(object sender, GridViewSortEventArgs e)
    {
        SetSortDirection();
        sortExpression = e.SortExpression + " " + sortDirection;
        ViewState["SortExpression"] = sortExpression;
        dtHealthCare.DefaultView.Sort = sortExpression;
        dtHealthCare = dtHealthCare.DefaultView.ToTable(true);

        grdHealthCareAffiliates.DataSource = dtHealthCare;
        grdHealthCareAffiliates.DataBind();

    }

    protected void SetSortDirection()
    {
        if (ViewState["SortDirection"] != null && ViewState["SortDirection"].ToString() == "ASC")
        {
            sortDirection = "DESC";
        }
        else
        {
            sortDirection = "ASC";
        }
        ViewState["SortDirection"] = sortDirection;
    }
}
