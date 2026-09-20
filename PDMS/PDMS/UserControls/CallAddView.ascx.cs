using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Views_CallAddView  : System.Web.UI.UserControl, ICallTrackerView
{
     #region Properties
    private CallTrackerPresenter _presenter;

    public CallTrackerPresenter Presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new CallTrackerPresenter(this);
            }

            return _presenter;
        }
    }

    public CallTrackingData Model { get; set; }

    public static System.Diagnostics.Stopwatch CallStopWatch;

    private DateTime? StartTime
    {
        get
        {
            if (ViewState["StartTime"] != null)
                return Convert.ToDateTime(ViewState["StartTime"]);
            else
                return null;
        }
        set
        {
            ViewState["StartTime"] = value;
        }
    }

    private DateTime? EndTime
    {
        get
        {
            if (ViewState["EndTime"] != null)
                return Convert.ToDateTime(ViewState["EndTime"]);
            else
                return null;
        }
        set
        {
            ViewState["EndTime"] = value;
        }
    }

    private bool ContinueCall
    {
        get
        {
            return ViewState["ContinueCall"] == null ? false : Convert.ToBoolean(ViewState["ContinueCall"]);
        }
        set
        {
            ViewState["ContinueCall"] = value;
        }
    }

    #endregion

    #region Public Methods

    public void InitView()
    {
        ContinueCall = false;
        Presenter.Init();
        InitFormFields();
    }
    #endregion

    #region Page Events
    protected void btnStart_Click(object sender, EventArgs e)
    {
        this.btnStart.Enabled = false;
        this.btnCancel.Enabled = true;
        this.btnSaveAndCont.Enabled = true;
        this.btnSaveAndEnd.Enabled = true;
        this.tmrTimer.Enabled = true;
        this.lblForTimer.Visible = true;
        this.lblTimer.Visible = true;

        this.StartTime = DateTime.Now;
        this.EndTime = null;

        CallStopWatch = new System.Diagnostics.Stopwatch();
        CallStopWatch.Start();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        CallStopWatch.Stop();
        InitFormFields();
    }

    protected void tmrTimer_Tick(object sender, EventArgs e)
    {
        long sec = CallStopWatch.Elapsed.Seconds;
        long min = CallStopWatch.Elapsed.Minutes;

        if (min < 10)
            this.lblTimer.Text = "0" + min;
        else
            this.lblTimer.Text = min.ToString();

        this.lblTimer.Text += ":";

        if (sec < 10)
            this.lblTimer.Text += "0" + sec;
        else
            this.lblTimer.Text += sec.ToString();

    }

    protected void btnSaveAndEnd_Click(object sender, EventArgs e)
    {
        this.ContinueCall = false;
        this.EndTime = DateTime.Now;
        SaveCall();
    }

    protected void btnSaveAndCont_Click(object sender, EventArgs e)
    {
        this.ContinueCall = true;
        this.EndTime = DateTime.Now;
        SaveCall();
    }
    #endregion

    #region View Methods

    public void SetCallSources(DataSet data)
    {
        this.ddlSource.Items.Clear();

        this.ddlSource.DataSource = data;
        this.ddlSource.DataValueField = "CALLTRACKING_SOURCE_ID";
        this.ddlSource.DataTextField = "NAME";
        this.ddlSource.DataBind();
        if (this.ddlSource.Items.Count > 1)
        {
            this.ddlSource.Items.Insert(0, new ListItem("", "0"));
        }
    }

    public void SetCallReasons(DataSet data)
    {
        this.ddlReason.Items.Clear();

        this.ddlReason.DataSource = data;
        this.ddlReason.DataValueField = "CALLTRACKING_SUBJECT_ID";
        this.ddlReason.DataTextField = "NAME";
        this.ddlReason.DataBind();
        if (this.ddlReason.Items.Count > 1)
        {
            this.ddlReason.Items.Insert(0, new ListItem("", "0"));
        }
    }

    public void SetCallResolutions(DataSet data)
    {
        this.ddlResolution.Items.Clear();

        this.ddlResolution.DataSource = data;
        this.ddlResolution.DataValueField = "CALLTRACKING_RESOLUTION_ID";
        this.ddlResolution.DataTextField = "NAME";
        this.ddlResolution.DataBind();
        if (this.ddlResolution.Items.Count > 1)
        {
            this.ddlResolution.Items.Insert(0, new ListItem("", "0"));
        }
    }

    public void SetInsertResults()
    {
        this.InitFormFields();

        this.lblSuccessMessage.Visible = true;
        this.txtCallID.Text = this.Model.CallTrackingID.ToString();
        if (this.ContinueCall)
            this.txtRegID.Focus();
    }

    public void SetValidationSuccess()
    {
        Presenter.InsertNewCall();
    }

    public void SetErrorMessages()
    {
        if (Presenter.hasErrors)
        {
            ClearErrorMessages();
            SetValidationErrors();
        }
    }
    #endregion

    #region Private Methods
    private void InitFormFields()
    {
        this.lblSuccessMessage.Visible = false;
        this.btnStart.Enabled = !ContinueCall;
        this.btnSaveAndCont.Enabled = ContinueCall;
        this.btnSaveAndEnd.Enabled = ContinueCall;
        this.btnCancel.Enabled = ContinueCall;

        if (!ContinueCall)
        {
            this.lblTimer.Text = string.Empty;
            this.lblForTimer.Visible = false;
            this.lblTimer.Visible = false;
            this.tmrTimer.Enabled = false;
            this.StartTime = null;
            this.EndTime = null;

            CallStopWatch = new System.Diagnostics.Stopwatch();
        }

        this.txtRegID.Text = string.Empty;
        this.chkNoRegID.Checked = false;
        this.txtCallerOther.Text = string.Empty;
        this.txtReasonOther.Text = string.Empty;
        this.txtCallDetails.Text = string.Empty;
        this.txtNPI.Text = string.Empty;
        this.txtMedicaidID.Text = string.Empty;
        this.txtCallID.Text = string.Empty;

        if (this.ddlSource.Items.Count > 0)
            this.ddlSource.SelectedIndex = 0;
        if (this.ddlReason.Items.Count > 0)
            this.ddlReason.SelectedIndex = 0;
        if (this.ddlResolution.Items.Count > 0)
            this.ddlResolution.SelectedIndex = 0;

    }

    private void SaveCall()
    {
        if (!Page.IsValid)
        {
            return;
        }

        if (CallStopWatch.IsRunning && !this.ContinueCall)
         {
             this.tmrTimer.Enabled = false;
             CallStopWatch.Stop();
         }
         this.Model = this.LoadModelFromForm();
         Presenter.ValidationCallData();
    }

    private CallTrackingData LoadModelFromForm()
    {
        CallTrackingData data = new CallTrackingData();
        data.RegID = this.txtRegID.Text.Trim();
        data.NoRegistrationOnFile = this.chkNoRegID.Checked;

        data.Duration = CallStopWatch.Elapsed;

        data.StartTime = this.StartTime.HasValue ? this.StartTime.Value : DateTime.Now;
        data.EndTime = this.EndTime.HasValue ? this.EndTime.Value : DateTime.Now;

        data.SourceID = this.ddlSource.SelectedIndex > -1 ? Convert.ToInt32(this.ddlSource.SelectedValue) : 0;
        data.SubjectID = this.ddlReason.SelectedIndex > -1 ? Convert.ToInt32(this.ddlReason.SelectedValue) : 0;
        data.ResolutionID = this.ddlResolution.SelectedIndex > -1 ? Convert.ToInt32(this.ddlResolution.SelectedValue) : 0;

        data.CallerOther = this.txtCallerOther.Text.Trim();
        data.ReasonOther = this.txtReasonOther.Text.Trim();
        data.CallDetails = this.txtCallDetails.Text.Trim();
        data.NPI = this.txtNPI.Text.Trim();
        data.MedicaidID = this.txtMedicaidID.Text.Trim();

        data.CreatedOn = DateTime.Now;
        data.CreatedBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        return data;
    }

     private void SetValidationErrors()
    {
        foreach (var pair in Presenter.ErrorList)
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

    #endregion

    protected void chkNoRegID_CheckedChanged(object sender, EventArgs e)
    {
        valRegID.Enabled = !chkNoRegID.Checked;
    }
}
