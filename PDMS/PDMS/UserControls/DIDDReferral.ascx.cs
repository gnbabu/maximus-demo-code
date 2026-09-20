using CustomControls;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_DIDDReferral : System.Web.UI.UserControl
{
    public int DIDDReferralID
    {
        get
        {
            if (ViewState["DIDDReferralID"] == null) ViewState["DIDDReferralID"] = 0;
            return (int)ViewState["DIDDReferralID"];
        }
        set { ViewState["DIDDReferralID"] = value; }
    }


    private bool UserCanAddEditReferral
    {
        get
        {
            return Helper.IsUserInDIDDRoles(HttpContext.Current.User.Identity.Name) || Helper.IsLoggedInUserInAdminRole();
        }
    }

    private bool UserCanEditServiceDates
    {
        get
        {
            return Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name);
        }
    }

    private DateTime? CurrentStartDate
    {
        get
        {
            return (DateTime?)ViewState["CurrentStartDate"];
        }
        set 
        { 
            ViewState["CurrentStartDate"] = value; 
        }
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
    #region Parent Page Events
        public delegate void KeyOpenEventHandler();
        public event KeyOpenEventHandler KeepOpenEvent;
    #endregion

    public void LoadData(int refId)
    {
        DIDDReferralID = refId;
        lblApplicationNo.Text = lblEmail.Text = lblProviderName.Text = lblGroupEntityName.Text = lblTaxID.Text = string.Empty;

        RefreshData();


        this.gbEditDates.Visible = false;
    }

    protected void grdServices_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdServices.SelectedIndex >= 0)
        {
            if (KeepOpenEvent != null)
                KeepOpenEvent();

            ShowEditDateSection();

            int serviceID = (int)grdServices.SelectedDataKey.Values["DIDD_REFERRAL_SERVICE_ID"];
            DataSet ds = GetServiceDetails(serviceID);
            LoadServiceDetails(ds);
        }
    }


    protected void grdServices_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("EditDates"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            grdServices.SelectRow(index);
        }

    }

    protected void btnAmendExtendContract_Click(object sender, EventArgs e)
    {
        if (DIDDReferralID > 0)
        {
            Response.Redirect("~/Process/WaiverReferralDataEntry.aspx?DIDDReferralId=" + DIDDReferralID.ToString());
        }
    }

    protected void btnSaveService_Click(object sender, EventArgs e)
    {
        if (KeepOpenEvent != null)
            KeepOpenEvent();

        if (!Page.IsValid)
        {
            return;
        }

        SaveServiceInformation();
        RefreshData();
        this.gbEditDates.Visible = false;
        EnableMainPage(true);

    }
    protected void btnCancelService_Click(object sender, EventArgs e)
    {
        if (KeepOpenEvent != null)
            KeepOpenEvent();
        
        InitServiceFormFields();
        this.grdServices.SelectedIndex = -1;
        this.gbEditDates.Visible = false;
        EnableMainPage(true);
    }

    protected void Validate_NewStartPriorToOldStart(object source, ServerValidateEventArgs args)
    {
        DateTime? currentStartDate = null;
        DateTime? newStartDate = null;

        DateTime tmp;
        if (DateTime.TryParse(this.txtStartDate.Text.Trim(), out tmp))
        {
            newStartDate = tmp;
        }

        if (!newStartDate.HasValue)
        {
            args.IsValid = false;
            return;
        }

        int index = grdServices.SelectedRow.RowIndex;
        if (DateTime.TryParse(this.grdServices.DataKeys[index].Values["CONTRACT_FROMDATE"].ToString(), out tmp))
        {
            currentStartDate = tmp;
        }

        if (!currentStartDate.HasValue)
        {
            args.IsValid = true;
            return;
        }
        
        if (newStartDate.Value > currentStartDate.Value)
        {
            //ok if it has not changed, but if changed must be prior to current start date
            args.IsValid = false;
            return;
        }
    }
    protected void Validate_EndDateAfterStartDate(object source, ServerValidateEventArgs args)
    {
        DateTime? newStartDate = null;
        DateTime? newEndDate = null;

        DateTime tmp;
        if (DateTime.TryParse(this.txtStartDate.Text.Trim(), out tmp))
        {
            newStartDate = tmp;
        }

        if (!newStartDate.HasValue)
        {
            return;
        }

        int index = grdServices.SelectedRow.RowIndex;
        if (DateTime.TryParse(this.txtEndDate.Text.Trim(), out tmp))
        {
            newEndDate = tmp;
        }

        if (!newEndDate.HasValue)
        {
            return;
        }

        if (newStartDate.Value > newEndDate.Value)
        {
            args.IsValid = false;
            return;
        }
    }

    

    #region Private Methods
    private void RefreshData()
    {
        DataSet ds = svc.SelectDIDDReferral(DIDDReferralID);  //This select returns 4 tables.

        if (!Helper.HasRows(ds))
            return;

        //First Table
        DataRow drProvider = ds.Tables[0].Rows[0];
        LoadProviderInformation(drProvider);

        //Second Table
        DataTable services = ds.Tables[1];
        LoadServices(services);

        //Third Table
        string region = string.Empty;
        if (ds.Tables.Count > 2)
        {
            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                region += Helper.GetString("REGION", dr) + ",";
            }
            if (!string.IsNullOrEmpty(region)) region = region.Substring(0, region.Length - 1);
        }
    }

    private void SetFieldEditability(int currentStepID, int pgmStatusID, int regStatusID)
    {
        bool canBeEdited = CanBeEdited(currentStepID, pgmStatusID, regStatusID);
        bool canBeAmended = CanBeAmended(currentStepID, regStatusID);

        btnAmendExtendContract.Text = !UserCanAddEditReferral ? "View" : canBeAmended ? Resources.BrandingResource.WAIVER_SERVICES_BUTTON_AMEND_CONTRACT : canBeEdited ? "Edit" : "View";
        lblAmendExtend.Visible = !UserCanAddEditReferral ? false : (!canBeEdited && !canBeAmended);

        grdServices.Columns[0].Visible = !UserCanEditServiceDates ? false : canBeAmended; //must have a reg in maint.

    }

    private bool CanBeEdited(int currentStepID, int pgmStatusID, int regStatusID)
    {
        return regStatusID == 0 || pgmStatusID == CON.RegistrationProgramStatusTypeId.Conversion || currentStepID == 0 ? true : false;
    }
    
    private bool CanBeAmended(int currentStepID, int regStatusID)
    {
        return currentStepID == 0 && regStatusID > 0 ? true : false; //if have at least one reg and that reg is in maint.
    }

    private void LoadProviderInformation(DataRow row)
    {
        lblApplicationNo.Text = UIHelper.CreateDIDDApplicationNumberWithSuffix(Helper.GetString("APPLICATION_NO", row), Helper.GetInt("REFERRAL_SUFFIX", row));
        lblEmail.Text = Helper.GetString("EMAIL", row);
        mltName.ActiveViewIndex = 0;
        if (!string.IsNullOrEmpty(Helper.GetString("FIRST_NAME", row)))
            lblProviderName.Text = Helper.GetString("FIRST_NAME", row) + " " + Helper.GetString("LAST_NAME", row);
        else
        {
            mltName.ActiveViewIndex = 1;
            lblGroupEntityName.Text = Helper.GetString("GROUP_ENTITY_NAME", row);
        }
        //lblNPI.Text = Helper.GetString("NPI", row);
        lblTaxID.Text = Helper.GetString("TAX_ID", row);

        int currentStepID = Helper.GetInt("CurrentStepID", row);
        int regStatusID = Helper.GetInt("RegistrationStatusTypeID", row);
        int pgmStatusID = Helper.GetInt("RegProgramStatusTypeID", row);

        DateTime? effectiveDate = row.GetValue<DateTime?>("EffectiveDateTime");
        lblEffectiveDate.Text = effectiveDate.HasValue ? effectiveDate.Value.ToString("MM/dd/yyyy") : string.Empty;

        lblZip.Text = Helper.GetString("SERVICING_ZIP", row);
        if (!string.IsNullOrEmpty(Helper.GetString("SERVICING_EXT_ZIP", row)))
            lblZip.Text = lblZip.Text + "-" + Helper.GetString("SERVICING_EXT_ZIP", row);

        SetFieldEditability(currentStepID, pgmStatusID, regStatusID);

    }

    private void LoadServices(DataTable dt)
    {
        grdServices.DataSource = dt;
        grdServices.DataBind();
    }

    private DataSet GetServiceDetails(int serviceID)
    {
        DataSet ds = svc.SelectDIDDReferralServiceByReferralServiceID(serviceID);

        return ds;
    }

    private void LoadServiceDetails(DataSet ds)
    {
        if (!Helper.HasRows(ds))
            return;

        DataRow row = ds.Tables[0].Rows[0];

        DateTime? serviceDate = row.GetValue<DateTime?>("CONTRACT_FROMDATE");
        txtStartDate.Text = serviceDate.HasValue ? serviceDate.Value.ToString("MM/dd/yyyy") : string.Empty;

        serviceDate = row.GetValue<DateTime?>("CONTRACT_TODATE");
        txtEndDate.Text = serviceDate.HasValue ? serviceDate.Value.ToString("MM/dd/yyyy") : string.Empty; 
    }

    private void InitServiceFormFields()
    {
        this.txtEndDate.Text = string.Empty;
        this.txtStartDate.Text = string.Empty;
    }

    private void SaveServiceInformation()
    {
        int serviceID = (int)this.grdServices.DataKeys[grdServices.SelectedIndex].Values["DIDD_REFERRAL_SERVICE_ID"];
        DateTime? startDate = null;
        DateTime? endDate = null;

        DateTime tmp;
        if (DateTime.TryParse(this.txtStartDate.Text.Trim(), out tmp))
        {
            startDate = tmp;
        }

        if (DateTime.TryParse(this.txtEndDate.Text.Trim(), out tmp))
        {
            endDate = tmp;
        }

        if (!startDate.HasValue)
            return;

        //update DIDD_REFERRAL_SERVICE record and REGISTRATION.Effective_Date, if applicable
        svc.UpdateDIDDReferralServiceDates(serviceID, startDate.Value, endDate, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name));
    }

    private void EnableMainPage(bool enable)
    {
        this.grdServices.Enabled = enable;
        this.btnAmendExtendContract.Enabled = enable;
        this.btnCancel.Enabled = enable;
    }

    private void ShowEditDateSection()
    {
        InitServiceFormFields();
        EnableMainPage(false);
        this.gbEditDates.Visible = true;
    }

    #endregion
}