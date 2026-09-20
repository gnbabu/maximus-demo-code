using Corp.Core.Libraries.Helper;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Views_ReactivationView : System.Web.UI.UserControl, IProviderReactivationView
{
  #region Properties
    private ProviderReactivationPresenter _presenter;

    public ProviderReactivationPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new ProviderReactivationPresenter(this);
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

    public string MedicaidID
    {
        get
        {
            return ViewState["MedicaidID"] == null ? "" : ViewState["MedicaidID"].ToString();
        }
        set
        {
            ViewState["MedicaidID"] = value;
        }
    }
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
    public delegate void UpdateSuccessEventHandler(string message, string medicaidID);
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

    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSave.ValidationGroup +
           "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
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

        if(!this.Model.RevalidationDate.HasValue)
        {
            lblErrorMessages.Text = "Re-Enrollment Date is required";
            if (KeepPopupOpenEvent != null)
            {
                //have to keep the pop up open
                KeepPopupOpenEvent();
                return;
            }
        }

        //OHPNM-14061 - take a version copy before starting the reactivation workflow
        DataSet ds = svc.InsertIntoVersionTables(RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        DataRow dr;
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            dr = ds.Tables[0].Rows[0];
            if (!string.IsNullOrEmpty(Methods.GetStringValue(dr["ErrorMessage"])))
            {
                Logging log = new Logging(Guid.NewGuid(), string.Empty);
                log.CreateLogEntry(string.Format("{0} {1}", "Error copying to version tables on Reactivation Workflow", Methods.GetStringValue(dr["ErrorMessage"])), Logging.LogPriority.Error);
                lblErrorMessages.Text = "Reactivation not successful";
                if (KeepPopupOpenEvent != null)
                {
                    //have to keep the pop up open
                    KeepPopupOpenEvent();
                    return;
                }
            }
        }

        Workflow.Process pr = new Workflow.Process(CON.WorkflowType.RegistrationNew, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Guid.NewGuid(), 3);

        if (pr == null)
        {
            lblErrorMessages.Visible = true;
            lblErrorMessages.Text = "Reactivation not successful";
            if (KeepPopupOpenEvent != null)
            {
                //have to keep the pop up open
                KeepPopupOpenEvent();
                return;
            }
        }

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        psc.InsertRegApplicationRecord(RegID, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId), pr.ProcessID);

        psc.WF_SaveProcessParameter(pr.ProcessID, "REGISTRATION_ID", RegID.ToString());
        psc.WF_SaveProcessParameter(pr.ProcessID, "IS_REACTIVATION", "1");
        psc.WF_SaveProcessParameter(pr.ProcessID, CON.ProcessParameter.IsSendHistoryinTxn, "1");

        // set ISReactivation on REGISTRATION table so WFPromoteToActive can use this; POSSIBLE_REVAL_DATE will be used for gap logic if applicable and nulled out in the CompleteWorkflow step
        Dictionary<string, string> reactivateParams = new Dictionary<string, string>();
        reactivateParams.Add("REG_ID", RegID.ToString());
        reactivateParams.Add("ISReactivation", "1");
        reactivateParams.Add("POSSIBLE_REVAL_DATE", Convert.ToString(this.Model.RevalidationDate));
        reactivateParams.Add("WAIVER_SERVICE_UPDATE_TYPE_ID", "0");

        if (DateTime.Now.AddDays(120) > this.Model.RevalidationDate)
        {
            reactivateParams.Add("WORKFLOW_EVENT_TYPE_ID", "3");
            psc.WF_SaveProcessParameter(pr.ProcessID, "WORKFLOW_EVENT_TYPE_ID", "3");
        }
        else
        {
            reactivateParams.Add("WORKFLOW_EVENT_TYPE_ID", "2");
            psc.WF_SaveProcessParameter(pr.ProcessID, "WORKFLOW_EVENT_TYPE_ID", "2");
        }
        psc.UpdateRegistrationCustom(reactivateParams);

        SetUpdateResults();

        ProviderFeedHelper.InsertProviderFeedNotes(RegID, 0, HttpContext.Current.User.Identity.Name, txtComments.Text,
            enrollmentType: CON.ExpressAdminActionDisplayNames.ReactivateProvider);
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
        this.lblEnrollmentStatus.Text = data.EnrollmentStatusCodeDescription;
        this.lblNewEnrollmentStatus.Text = "Active";

        //this.txtEffectiveDate.Text = data.ChangeEffectiveDate.HasValue ? data.ChangeEffectiveDate.Value.ToString("MM/dd/yyyy") : string.Empty;
        //DataSet ds = svc.SelectRegEnrollmentByRegID(data.RegID);
        /*if (ObjectControllerHelper.HasRows(ds))
        {
            this.txtEffectiveDate.Text = ObjectControllerHelper.GetDateTime("ENROLL_START_DATE_TIME", ds.Tables[0].Rows[0]).ToString("MM/dd/yyyy");
            hdnPreviousEffectiveDate.Value = ObjectControllerHelper.GetDateTime("ENROLL_START_DATE_TIME", ds.Tables[0].Rows[0]).ToString("MM/dd/yyyy");
            this.hdnNewEnrollmentStatusCode.Value = ObjectControllerHelper.GetString("ENROLLMENT_STATUS_CODE", ds.Tables[0].Rows[0]).ToString();
        }*/
        //this.txtReEnrollmentDueDate.Text = data.PreviousEndDate.HasValue ? data.PreviousEndDate.Value.ToString("MM/dd/yyyy") : string.Empty;

        this.MedicaidID = data.MedicaidID;
    }

    public void SetValidationSuccess()
    {
        presenter.ReactivateProvider();       
    }

    public void SetUpdateResults()
    {
        this.btnSave.Enabled = false;
        //this.txtEffectiveDate.Enabled = false;
        //this.txtReEnrollmentDueDate.Enabled = false;
        if (UpdateSuccessEvent != null)
        {
            UpdateSuccessEvent(Resources.BrandingResource.REACTIVATION_SUCCESS, this.MedicaidID);
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

    #endregion

    #region Private Methods
    private ProviderManagementData LoadModelFromForm()
    {
        ProviderManagementData data = new ProviderManagementData();
        data.RegID = RegID;
        DateTime tmp;
        if (DateTime.TryParse(this.lblEffectiveDate.Text.Trim(), out tmp))
        {
            data.ChangeEffectiveDate = tmp;
        }

         /*if (DateTime.TryParse(this.hdnPreviousEffectiveDate.Value.Trim(), out tmp))
        {
            data.PreviousChangeEffectiveDate = tmp;
        }*/

         if (DateTime.TryParse(this.lblRevalDueDate.Text.Trim(), out tmp))
         {
             data.EndDate = tmp;
         }
        //load these values if needed for validations
        if (DateTime.TryParse(this.txtReEnrollmentDueDate.Text.Trim(), out tmp))
        {
            data.RevalidationDate = tmp;
        }
        data.EnrollmentStatusCodeDescription = this.lblEnrollmentStatus.Text;     


        if (this.hdnNewEnrollmentStatusCode.Value == "00" || this.hdnNewEnrollmentStatusCode.Value == "80" || this.hdnNewEnrollmentStatusCode.Value == "90")
            data.EnrollmentStatusCode = this.hdnNewEnrollmentStatusCode.Value;
        else
        {
            data.EnrollmentStatusCode = MAXIMUS.Core.Libraries.Constants.EnrollStatus.ACTIVE.ToString();
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
        this.lblEffectiveDate.Text = string.Empty;
        this.lblEnrollmentStatus.Text = string.Empty;
        this.lblRevalDueDate.Text = string.Empty;
        //this.txtEffectiveDate.Text = string.Empty;
        this.txtReEnrollmentDueDate.Text = string.Empty;
        this.txtComments.Text = string.Empty;
        this.lblNewEnrollmentStatus.Text = string.Empty;
        this.btnSave.Enabled = true;
    }
    #endregion
}