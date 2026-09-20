using Corp.Core.Libraries.Helper;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Globalization;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Views_RetroEffectiveView : System.Web.UI.UserControl, IProviderRetroEffectiveView
{
  #region Properties
    private ProviderRetroEffectivePresenter _presenter;

    public ProviderRetroEffectivePresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new ProviderRetroEffectivePresenter(this);
            }

            return _presenter;
        }
    }

    public ProviderManagementData Model { get; set; }

    public int RegID
    {
        get
        {
            return ViewState["RegID"] == null ? 0 : Convert.ToInt32(ViewState["RegID"]);
        }
        set
        {
            ViewState["RegID"] = value;
        }
    }
	
    public string ExistingEffectiveDate
    {
        get
        {
            if (ViewState["ExistingEffectiveDate"] == null) ViewState["ExistingEffectiveDate"] = string.Empty;
            return ViewState["ExistingEffectiveDate"].ToString();
        }
        set { ViewState["ExistingEffectiveDate"] = value; }
    }
    public string ExistingRevalidationDate
    {
        get
        {
            if (ViewState["ExistingRevalidationDate"] == null) ViewState["ExistingRevalidationDate"] = string.Empty;
            return ViewState["ExistingRevalidationDate"].ToString();
        }
        set { ViewState["ExistingRevalidationDate"] = value; }
    }

    #endregion

    #region Parent Page Events
    public delegate void UpdateSuccessEventHandler(string message);
    public event UpdateSuccessEventHandler UpdateSuccessEvent;
    
    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    public delegate void KeepPopupOpenEventHandler();
    public event KeepPopupOpenEventHandler KeepPopupOpenEvent;

    #endregion

    #region Public Methods

    public void InitView(int regID)
    {
        this.RegID = regID;
        InitFormFields();
        // Set the revalidation date field requirement based on provider status
        SetRevalidationDateRequirement(regID);
        presenter.Init(regID);
		ucEnrollmentData.InitViewForAdmin(regID);
    }
    #endregion
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool isEffDateValid = false;
        bool isNewRevalDateValid = false;
        
        string dateText = this.txtNewEffectiveDate.Text.Trim();
        if (dateText.Length >= 8)
        {
            // Get the year part
            string[] parts = dateText.Split('/');
            if (parts.Length == 3 && parts[2].Length == 4)
            {
                isEffDateValid = true;
            }
        }
        if (string.IsNullOrEmpty(dateText))
            isEffDateValid = true;
        dateText = this.txtNewRevalDate.Text.Trim();
        if (dateText.Length >= 8)
        {
            // Get the year part
            string[] parts = dateText.Split('/');
            if (parts.Length == 3 && parts[2].Length == 4)
            {
                isNewRevalDateValid = true;
            }
        }
        if (string.IsNullOrEmpty(dateText))
            isNewRevalDateValid = true;

        if (!isEffDateValid || !isNewRevalDateValid)
        {
            if (!lblErrorMessages.Text.Contains("Please enter the date in MM/DD/YYYY format"))
                AddErrorMessage("Please enter the date in MM/DD/YYYY format.");
                
            if (KeepPopupOpenEvent != null)
            {
                //have to keep the pop up open
                KeepPopupOpenEvent();
                return;
            }
        }
        if (isEffDateValid && isNewRevalDateValid)
        {
            lblErrorMessages.Text = "";
        }
        if (!Page.IsValid)
        {
            if (KeepPopupOpenEvent != null)
            {
                //have to keep the pop up open
                KeepPopupOpenEvent();
                return;
            }
        }

        this.Model = this.LoadModelFromForm();

        var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        // Set TwoDigitYearMax to 2099 so that 30 maps to 2030        
        culture.Calendar.TwoDigitYearMax = 2099;

        DateTime tmp;
        DateTime.TryParse(this.lblExistingEffectiveDate.Text.Trim(), out tmp);
        tmp = DateTime.ParseExact(this.lblExistingEffectiveDate.Text.Trim(), new[] { "M/d/yy", "M/d/yyyy" }, // Accept 2-digit or 4-digit year            
                culture, DateTimeStyles.None);

        DateTime tmp1;
        DateTime.TryParse(ExistingRevalidationDate.Trim(), out tmp1);
        tmp1 = DateTime.ParseExact(ExistingRevalidationDate.Trim(), new[] { "M/d/yy", "M/d/yyyy" }, // Accept 2-digit or 4-digit year            
                culture, DateTimeStyles.None);

        presenter.ValidateRetroEffectiveInformation(tmp, tmp1);

        ProviderFeedHelper.InsertProviderFeedNotes(RegID, 0, HttpContext.Current.User.Identity.Name, txtComments.Text, 
             enrollmentType: CON.AdminMaintenanceActions.RetroEffectiveDate);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    #region View Methods
    public void SetProviderInformation(ProviderManagementData data)
    {
        this.lblExistingEffectiveDate.Text = data.ChangeEffectiveDate.HasValue ? data.ChangeEffectiveDate.Value.ToString("MM/dd/yyyy") : string.Empty;
        this.ExistingRevalidationDate = data.RevalidationDate.HasValue ? data.RevalidationDate.Value.ToString("MM/d/yyyy") : string.Empty;

        // Re-check revalidation date requirement when provider information is set
        SetRevalidationDateRequirement(data.RegID);
    }

    public void SetValidationSuccess()
    {
		// ucEnrollmentData.SaveData(); should this be removed?
        presenter.RetroEffectiveDateProvider();
    }

    public void SetUpdateResults()
    {
        this.btnSave.Enabled = false;

        if (UpdateSuccessEvent != null)
        {
            UpdateSuccessEvent(Resources.BrandingResource.RETROEFFECTIVE_UPDATE_SUCCESS);
        }
    }

    public void SetErrorMessages()
    {
        if (presenter.hasErrors)
        {
            ClearErrorMessages();
            SetValidationErrors();
        }

        if (KeepPopupOpenEvent != null)
        {
            //have to keep the pop up open
            KeepPopupOpenEvent();
            return;
        }
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Sets the requirement for revalidation date field based on provider revalidation status
    /// </summary>
    /// <param name="regId">Provider registration ID</param>
    private void SetRevalidationDateRequirement(int regId)
    {
        try
        {
            // Call SVC method to check if revalidation date is required
            bool isRevalidationDateRequired = svc.CheckProviderRevalidated(regId);

            // Enable/disable the required field validator based on the result
            valNewRevalDateReqd.Enabled = isRevalidationDateRequired;

            // Show/hide the asterisk visual indicator
            lblRevalidationDateAsterisk.Visible = isRevalidationDateRequired;
        }
        catch (Exception ex)
        {
            valNewRevalDateReqd.Enabled = true; // Default to required
            lblRevalidationDateAsterisk.Visible = true; // Default to show asterisk
        }

    }
    private ProviderManagementData LoadModelFromForm()
    {
        var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        // Set TwoDigitYearMax to 2099 so that 30 maps to 2030        
        culture.Calendar.TwoDigitYearMax = 2099;

        ProviderManagementData data = new ProviderManagementData();
        data.RegID = RegID;

        DateTime tmp;
        if (DateTime.TryParse(this.txtNewEffectiveDate.Text.Trim(), out tmp))
        {
            tmp = DateTime.ParseExact(this.txtNewEffectiveDate.Text.Trim(), new[] { "M/d/yy", "M/d/yyyy" }, // Accept 2-digit or 4-digit year            
                culture, DateTimeStyles.None);
            data.ChangeEffectiveDate = tmp;
        }
        //SAM635 give field for new Revalidation Date to be entered
        DateTime tmp1;
        if (!string.IsNullOrEmpty(this.txtNewRevalDate.Text))
        {
            if (DateTime.TryParse(this.txtNewRevalDate.Text.Trim(), out tmp1))
            {
                tmp1 = DateTime.ParseExact(this.txtNewRevalDate.Text.Trim(), new[] { "M/d/yy", "M/d/yyyy" }, // Accept 2-digit or 4-digit year            
                    culture, DateTimeStyles.None);
                data.PreviousEndDate = tmp1;
            }
        }

        data.Comments = this.txtComments.Text.Trim();

        data.LastModifiedDate = DateTime.Now;
        data.LastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        return data;
    }

     private void SetValidationErrors()
    {
        foreach (var pair in presenter.ErrorList)
        {
            AddErrorMessage(pair.Value);
        }
    }

    private void AddErrorMessage(string message)
    {
        lblErrorMessages.Text = string.Format("{0}{1}<br />", lblErrorMessages.Text, message);
    }

    private void ClearErrorMessages()
    {
        lblErrorMessages.Text = string.Empty;
    }

    private void InitFormFields()
    {
        this.lblErrorMessages.Text = string.Empty;
        this.btnSave.Enabled = true;

        this.lblExistingEffectiveDate.Text = string.Empty;
        this.txtNewEffectiveDate.Text = string.Empty;
        this.txtComments.Text = string.Empty;

        this.txtNewEffectiveDate.Enabled = true;
    }
    #endregion

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
}