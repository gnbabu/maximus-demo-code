using Corp.Core.Libraries.Helper;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Views_ExpressTerminationView : System.Web.UI.UserControl, IProviderTerminateView
{
  #region Properties
    private ProviderTerminationPresenter _presenter;

    public ProviderTerminationPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new ProviderTerminationPresenter(this);
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

    #endregion

    #region Parent Page Events
    public delegate void UpdateSuccessEventHandler(string message);
    public event UpdateSuccessEventHandler UpdateSuccessEvent;
    
    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    public delegate void KeepPopupOpenEventHandler();
    public event KeepPopupOpenEventHandler KeepPopupOpenEvent;

    public delegate void TermWorkflowCreatedEventHandler(ProviderManagementData data);
    public event TermWorkflowCreatedEventHandler TermWorkflowCreatedEvent;

    #endregion

    #region Public Methods

    public void InitView(int regID)
    {
       
        this.RegID = regID;
        InitFormFields();
        presenter.Init(regID);
    }
    #endregion

    protected void btnSave_Click(object sender, EventArgs e)
    {
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
        presenter.ValidateTerminationInformation();

        ProviderFeedHelper.InsertProviderFeedNotes(RegID, 0, HttpContext.Current.User.Identity.Name, txtComments.Text,
             enrollmentType: CON.ExpressAdminActionDisplayNames.ProviderTermination, finalDisposition: CON.FinalDisposition.Terminated);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    #region View Methods

    public void SetProviderInformation(ProviderManagementData data)
    {
        this.lblEffectiveDate.Text = data.ChangeEffectiveDate.HasValue ? data.ChangeEffectiveDate.Value.ToString("MM/dd/yyyy") : string.Empty;
        this.lblRevalDueDate.Text = data.RevalidationDate.HasValue ? data.RevalidationDate.Value.ToString("MM/dd/yyyy") : string.Empty;
        this.txtTermDate.Text = data.TerminationDate.HasValue ? data.TerminationDate.Value.ToString("MM/dd/yyyy") : string.Empty;
        this.lblEnrollmentStatus.Text = data.EnrollmentStatusCodeDescription;

        ListItem item = this.ddlStatus.Items.FindByValue(data.EnrollmentStatusCode);
        if (item != null)
        {
            this.ddlStatus.SelectedValue = item.Value;
        }
        else
        {
            this.ddlStatus.SelectedIndex = 0;
        }
    }

    public void SetValidationSuccess()
    {
        presenter.TerminateProvider();
    }

    public void SetUpdateResults()
    {
        this.btnSave.Enabled = false;
        this.txtTermDate.Enabled = false;
        if (UpdateSuccessEvent != null)
        {
            UpdateSuccessEvent(Resources.BrandingResource.TERMINATION_SUCCESS);
        }
    }

    public void SetWorkflowCreatedResults()
    {
       
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

    public void SetEnrollmentStatuses(DataView statuses)
    {
        this.ddlStatus.DataSource = statuses;
        this.ddlStatus.DataValueField = "ENROLLMENT_STATUS_REASONS_CDE";
        this.ddlStatus.DataTextField = "ENROLLMENT_STATUS_REASONS_DESC";
        this.ddlStatus.DataBind();
        this.ddlStatus.Items.Insert(0, new ListItem("", ""));
    }

    #endregion

    #region Private Methods
    private ProviderManagementData LoadModelFromForm()
    {
        ProviderManagementData data = new ProviderManagementData();
        data.RegID = RegID;
        DateTime tmp;
        if (DateTime.TryParse(this.txtTermDate.Text.Trim(), out tmp))
        {
            data.TerminationDate = tmp;
        }
        //load these values if needed for validations
        if (DateTime.TryParse(this.lblEffectiveDate.Text.Trim(), out tmp))
        {
            data.ChangeEffectiveDate = tmp;
        }
        if (DateTime.TryParse(this.lblRevalDueDate.Text.Trim(), out tmp))
        {
            data.RevalidationDate = tmp;
        }
        data.EnrollmentStatusCodeDescription = this.lblEnrollmentStatus.Text;
        data.EnrollmentStatusCode = MAXIMUS.Core.Libraries.Constants.EnrollStatus.INACTIVE.ToString();
        data.Comments = this.txtComments.Text.Trim();
        data.enrollmentStatusReason = this.ddlStatus.SelectedValue;

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
        this.lblEffectiveDate.Text = string.Empty;
        this.lblEnrollmentStatus.Text = string.Empty;
        this.lblRevalDueDate.Text = string.Empty;
        this.txtTermDate.Text = string.Empty;
        if (this.ddlStatus.Items.Count > 0) this.ddlStatus.SelectedIndex = 0;
        this.txtTermDate.Enabled = true;
        this.btnSave.Enabled = true;

    }
    #endregion
}