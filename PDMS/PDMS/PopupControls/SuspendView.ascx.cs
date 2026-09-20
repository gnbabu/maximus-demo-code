using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Web;
using System.Web.UI;

public partial class Views_SuspendView : System.Web.UI.UserControl, IProviderDisenrollmentView
{
  #region Properties
    private ProviderDisenrollmentPresenter _presenter;

    public ProviderDisenrollmentPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new ProviderDisenrollmentPresenter(this);
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

    public bool IsSuspend
    {
        get
        {
            return ViewState["IsSuspend"] == null ? false : Convert.ToBoolean(ViewState["IsSuspend"]);
        }
        set
        {
            ViewState["IsSuspend"] = value;
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
        presenter.ValidateDisenrollmentInformation(IsSuspend);
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
    }

    public void SetValidationSuccess()
    { 
        presenter.DisenrollProvider(IsSuspend);
    }

    public void SetUpdateResults()
    {
        this.btnSave.Enabled = false;
        this.txtTermDate.Enabled = false;
        if (UpdateSuccessEvent != null)
        {
            if (IsSuspend)
                UpdateSuccessEvent(Resources.BrandingResource.SUSPEND_SUCCESS);
            else
                UpdateSuccessEvent(Resources.BrandingResource.DISENROLLMENT_SUCCESS);
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
        this.lblEffectiveDate.Text = string.Empty;
        this.lblEnrollmentStatus.Text = string.Empty;
        this.lblRevalDueDate.Text = string.Empty;
        this.txtTermDate.Text = string.Empty;
        this.txtComments.Text = string.Empty;

        this.txtTermDate.Enabled = true;
        this.btnSave.Enabled = true;

        if (!IsSuspend)
            lblDisenrollEffectiveDate.Text = "Disenrollment Effective Date*";
        else
        {
            lblDisenrollEffectiveDate.Text = "Suspend Claims Effective Date*";
            valTermDateReqd.ErrorMessage = valTermDateReqd.ErrorMessage.Replace("Disenrollment", "Suspend Claims");
            cvTermDate.ErrorMessage = cvTermDate.ErrorMessage.Replace("Disenrollment", "Suspend Claims");
        }

    }
    #endregion
}