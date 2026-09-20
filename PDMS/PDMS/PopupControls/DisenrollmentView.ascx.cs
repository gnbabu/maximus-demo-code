using Corp.Core.Libraries.Helper;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Views_DisenrollmentView : System.Web.UI.UserControl, IProviderDisenrollmentView
{
  #region Properties
    private ProviderDisenrollmentPresenter _presenter;
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

    public bool IsNotAdminFields
    {
        get
        {
            return ViewState["IsNotAdminFields"] == null ? false : Convert.ToBoolean(ViewState["IsNotAdminFields"]);
        }
        set
        {
            ViewState["IsNotAdminFields"] = value;
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
    protected void Page_PreRender(object sender, EventArgs e)
    {
        SetupButtonDisablesOfMultiClick();
    }

    #endregion
    #region Public Methods

    public void InitView(int regID)
    {
        this.RegID = regID;
        InitFormFields();
        presenter.Init(regID);

    }
    #endregion

    private void SetupButtonDisablesOfMultiClick()
    {
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
    }
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

        if(IsSuspend)
        {
            ProviderFeedHelper.InsertProviderFeedNotes(RegID, 0, HttpContext.Current.User.Identity.Name, txtComments_area.InnerText,
                enrollmentType: CON.ExpressAdminActionDisplayNames.SuspendProvider, finalDisposition: CON.FinalDisposition.Suspended);
        }
        else
            ProviderFeedHelper.InsertProviderFeedNotes(RegID, 0, HttpContext.Current.User.Identity.Name, txtComments_area.InnerText,
                enrollmentType: CON.ExpressAdminActionDisplayNames.ProviderDisrollment, finalDisposition: CON.FinalDisposition.Disenrolled);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    #region View Methods
    string selectedDisenrolmentId = "";
    public void SetProviderInformation(ProviderManagementData data)
    {

      selectedDisenrolmentId = "";
        DataSet ds = new DataSet();
        ds = svc.SelectProviderDisenrollmentxref();
        if (ds.Tables[0].Rows.Count != 0)
        {
            disEnrollmentOptions.DataSource = ds;
            disEnrollmentOptions.DataTextField = "PROVIDER_DISENROLLMENT_NAME";
            disEnrollmentOptions.DataValueField = "PROVIDER_DISENROLLMENT_XREF_ID";
            disEnrollmentOptions.DataBind();
        }
        this.lblEffectiveDate.Text = data.ChangeEffectiveDate.HasValue ? data.ChangeEffectiveDate.Value.ToString("MM/dd/yyyy") : string.Empty;
        this.lblRevalDueDate.Text = data.RevalidationDate.HasValue ? data.RevalidationDate.Value.ToString("MM/dd/yyyy") : string.Empty;
        this.txtTermDate.Text = data.TerminationDate.HasValue ? data.TerminationDate.Value.ToString("MM/dd/yyyy") : string.Empty;
        
        this.lblEnrollmentStatus.Text = data.EnrollmentStatusCodeDescription;
        this.txtComments_area.Value = data.TERMCOMMENTS.Trim();
        ds = svc.SelectProviderDisenrollment(RegID);

       
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            var disenrollDate = Helper.GetDate("DISENROLLMENT_DATE", dr);
            if(!string.IsNullOrEmpty(disenrollDate) && string.IsNullOrEmpty(this.txtTermDate.Text))
            {
                this.txtTermDate.Text = disenrollDate;
            }

            for (int i = 0; i < disEnrollmentOptions.Items.Count; i++)
            {
                if(disEnrollmentOptions.Items[i].Value == ObjectControllerHelper.GetString("PROVIDER_DISENROLLMENT_XREF_ID", dr))
                {
                    selectedDisenrolmentId = selectedDisenrolmentId+ disEnrollmentOptions.Items[i].Value + ",";
                    disEnrollmentOptions.Items[i].Selected = true;
                    break;
                }
            }
            }
        Session["selectedDisenrolmentId"] = selectedDisenrolmentId;          
        
    }

    public void SetValidationSuccess()
    {
        
            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAdministrator)
                && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAgent))
            {
                return;
            }

            DataSet dsProcess = svc.SelectCurrentWFTaskInfo(RegID);
           if (Helper.HasRows(dsProcess) && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.EnrollmentSpecialist))
            {
                presenter.DisenrollProviderInWorkflow(dsProcess, IsSuspend);
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                presenter.DisenrollProvider(IsSuspend);
            }
       

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
        DateTime disenrollDate;
        DateTime dateFormat;

        if (Session["selectedDisenrolmentId"] != null)
            selectedDisenrolmentId = Session["selectedDisenrolmentId"].ToString();
        if (DateTime.TryParse(this.txtTermDate.Text.Trim(), out disenrollDate))
        {
            data.TerminationDate = disenrollDate;
        }
        //load these values if needed for validations
        if (DateTime.TryParse(this.lblEffectiveDate.Text.Trim(), out dateFormat))
        {
            data.ChangeEffectiveDate = dateFormat;
        }
        if (DateTime.TryParse(this.lblRevalDueDate.Text.Trim(), out dateFormat))
        {
            data.RevalidationDate = dateFormat;
        }
        data.EnrollmentStatusCodeDescription = this.lblEnrollmentStatus.Text;
        data.EnrollmentStatusCode = MAXIMUS.Core.Libraries.Constants.EnrollStatus.INACTIVE.ToString();
        data.Comments = txtComments_area.Value.Trim();
        data.LastModifiedDate = DateTime.Now;
        data.LastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        data.enrollmentStatusReason = this.ddlEnrollreason.SelectedValue;
        for (int i = 0; i < disEnrollmentOptions.Items.Count; i++)
        {
            if (disEnrollmentOptions.Items[i].Selected)
            {
                if (!selectedDisenrolmentId.Contains(disEnrollmentOptions.Items[i].Value))
                    svc.InsertProviderDisenrollment(RegID, disEnrollmentOptions.Items[i].Value, disenrollDate, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(),
                         true);
            }
            else if (selectedDisenrolmentId.Contains(disEnrollmentOptions.Items[i].Value))
            {
                svc.InsertProviderDisenrollment(RegID, disEnrollmentOptions.Items[i].Value, disenrollDate, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(),
                     false);
            }
        }
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
    private void LoadEnrollReasons()
    {
        ddlEnrollreason.Visible = true;
        Dictionary<string, string> item = new Dictionary<string, string>();
        item.Add(Convert.ToString(CON.EnrollStatusReason.SUSPENDED_INDICTMENT_NON_FRAUD),"SUSPENDED - INDICTMENT(NON - FRAUD)");
        item.Add(Convert.ToString(CON.EnrollStatusReason.SUSPENSION_FRAUD_INDICTMENT), "SUSPENSION - FRAUD INDICTMENT");
        item.Add(Convert.ToString(CON.EnrollStatusReason.SUSPENSION_FRAUD_INDICTMENT_13), "SUSPENSION - FRAUD INDICTMENT (30)");
        item.Add(Convert.ToString(CON.EnrollStatusReason.SUSPENDED_CREDIBLE_FRAUD_ALLEGATION), "SUSPENDED-CREDIBLE FRAUD ALLEGATION");

        ddlEnrollreason.DataSource = item;
        ddlEnrollreason.DataTextField = "Value";
        ddlEnrollreason.DataValueField = "Key";
        ddlEnrollreason.DataBind();        
    }

    private void InitFormFields()
    {
        this.lblErrorMessages.Text = string.Empty;
        this.lblEffectiveDate.Text = string.Empty;
        this.lblEnrollmentStatus.Text = string.Empty;
        this.lblRevalDueDate.Text = string.Empty;
        this.txtTermDate.Text = string.Empty;
        this.txtComments_area.Value = string.Empty;
        this.lblEnrollstatusreason.Visible = false;
        this.ddlEnrollreason.Visible = false;        
        this.txtTermDate.Enabled = true;
        this.btnSave.Enabled = true;
        if (this.IsNotAdminFields)
            divcomments.Visible = divEffective.Visible = divEnrollment.Visible = divRevalidation.Visible = false;
        if (!IsSuspend)
        {
            lblDisenrollEffectiveDate.Text = "Disenrollment Effective Date*";
        }
        else
        {
            lblEnrollstatusreason.Visible =true;
            ddlEnrollreason.Visible = true;
            LoadEnrollReasons();
            this.divRadio.Visible = false;
            lblDisenrollEffectiveDate.Text = "Suspend Claims Effective Date*";
            valTermDateReqd.ErrorMessage = valTermDateReqd.ErrorMessage.Replace("Disenrollment", "Suspend Claims");
            cvTermDate.ErrorMessage = cvTermDate.ErrorMessage.Replace("Disenrollment", "Suspend Claims");
        }

    }
    #endregion
}