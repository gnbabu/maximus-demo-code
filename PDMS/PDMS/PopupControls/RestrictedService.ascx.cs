using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using MAXIMUS.Controllers.PDMS;

public partial class PopupControls_RestrictedService : BasePopupControl
{
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
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;

    public delegate void KeepOpenEventHandler();
    public event KeepOpenEventHandler KeepOpenEvent;

    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    #endregion
    #region Properties

    public int RegRestrictID
    {
        get
        {
            return ViewState["RegRestrictID"] == null ? 0 : Convert.ToInt32(ViewState["RegRestrictID"]);
        }
        set
        {
            ViewState["RegRestrictID"] = value;
        }
    }
    #endregion
    private bool _ExportHistory
    {
        get
        {
            return Convert.ToBoolean(ViewState["ExportHistory"]);
        }
        set
        {
            ViewState["ExportHistory"] = value;
        }
    }

    private DataTable dt;
    private Dictionary<string, string> Errors = new Dictionary<string, string>();
    private void SetDt()
    {
        if (dt == null)
        {
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "RESTRICTION");
            dt = Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }
    //public void SetTitle(string value)
    //{
    //    lblTitle.Text = value;
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        BindAllDropdowns();
        if ((Helper.IsUserInRestrictedServiceUpdateRole(HttpContext.Current.User.Identity.Name)) && this.WorkflowPage.CurrentTaskName == "")
        {
            Helper.SetReadOnly(ceEffectiveDate, false);
            Helper.SetReadOnly(CalendarExtender1, false);
            Helper.SetReadOnly(ddlRSStatus, false);
            Helper.SetReadOnly(ddlReviewType, false);
            Helper.SetReadOnly(ddlIncludeExclude, false);
            Helper.SetReadOnly(ddlRestrict, false);
            Helper.SetReadOnly(ddlReviewReason, false);
            Helper.SetReadOnly(btnSave, false);
        }
        else 
        {
            Helper.SetReadOnly(ceEffectiveDate, true);
            Helper.SetReadOnly(CalendarExtender1, true);
            Helper.SetReadOnly(ddlRSStatus, true);
            Helper.SetReadOnly(ddlReviewType, true);
            Helper.SetReadOnly(ddlIncludeExclude, true);
            Helper.SetReadOnly(ddlRestrict, true);
            Helper.SetReadOnly(ddlReviewReason, true);
            Helper.SetReadOnly(btnSave, true);

        }
    }


    public override void LoadData(DataRow dr=null)
    {
        if (dr!=null)
        {
            this.BindAllDropdowns();
            this.SetFormFields(dr);
        }
        else
        {
            this.InitFormFields();
        }

        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = svc.SelectRegistrationDataWithParams("usp_GetRestrictedServicesHistory", parms);
            if (Helper.HasRows(ds))
            {
                grd.DataSource = ds.Tables[0];
                grd.DataBind();
                grd.MasterTableView.ExportToExcel();
            }
        }


        // btnSave.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");


    }
    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "RestrcitedService";
        this.Page.Validators.Add(val);
      
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
     


            //showMessage = string.Empty;
            Page.Validate("RestrcitedService");
            ValidateData();
            if (!Page.IsValid)
            {
                if (KeepOpenEvent != null)
                {
                  KeepOpenEvent();
                }
               // return;
            }

            bool rtn = SaveData();
            if (!rtn)
                return;


        if (SaveEvent != null)
            SaveEvent();
        


    }

   
    private bool SaveData()
    {
        bool rtn = true;
        Errors.Clear();

        if (this.RegRestrictID > 0)
        {
            this.UpdateRegRestriction();
        }
        else
        {
            this.InsertRegRestriction();
        }

       
        if (Errors.Count > 0)
        {
            SetErrorMessages();
            return false;
        }

       

        return rtn;
    }
    private Dictionary<string, string> SetCommonParms()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("EFFECTIVE_DATE", txtEffectiveDate.Text);
        parms.Add("END_DATE", string.IsNullOrEmpty(txtRSEndDate.Text) ? "12/31/2299" : txtRSEndDate.Text);
        if (ddlIncludeExclude.SelectedIndex>=0)
            parms.Add("IS_EXCLUDE", ddlIncludeExclude.SelectedItem.Value);
        //if (ddlRestrict.SelectedIndex > 0)
            parms.Add("IS_RESTRICT", ddlRestrict.SelectedItem.Value);      
        if (ddlReviewType.SelectedIndex >= 0)
            parms.Add("REVIEW_TYPE_ID", ddlReviewType.SelectedItem.Value);
        if (ddlReviewReason.SelectedIndex >= 0)
            parms.Add("REVIEW_REASON_ID", ddlReviewReason.SelectedItem.Value);
        if (ddlRSStatus.SelectedIndex >= 0)
            parms.Add("STATUS_CODE", ddlRSStatus.SelectedItem.Value);     
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("SENT_TO_SI", false.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        return parms;
    }
    private void UpdateRegRestriction()
    {

        try
        {
            if (this.WorkflowPage.RegistrationId == 0)
            {
                Errors.Add("UpdateRegRestriction1", "Session Reg ID is null, unable to proceed with insert.");
                return;
            }
            Dictionary<string, string> parms = this.SetCommonParms();

            parms.Add("REG_RESTRICTION_ID", this.RegRestrictID.ToString());
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
            int RegRestrictionID = svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "RESTRICTION", parms);
            if (RegRestrictionID > 0)
                SendTransactiontoSI();
            this.RegRestrictID = RegRestrictionID;

        }
        catch (Exception ex)
        {
            Errors.Add("UpdateRegRestriction2", ex.Message);
        }


    }
    private void SendTransactiontoSI()
    {
        if (this.WorkflowPage.CurrentTaskName == "" && Helper.IsUserInRestrictedServiceViewRole(HttpContext.Current.User.Identity.Name))
        {
            int transactionType = (int)TransactionController.TransactionTypeNew.SendMMISUpdate;
            int serviceLocationID = 0;
            string txnResult = string.Empty;

            DataSet ds = _svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
            if (Helper.HasRows(ds))
            {
                DataRow row = ds.Tables[0].Rows[0];
                serviceLocationID = Helper.GetInt("REG_SERVICE_LOCATION_ID", row);

                int tqId = TransactionController.InsertTransactionQueue(
                       transactionType,
                       this.WorkflowPage.RegistrationId,
                       serviceLocationID,
                       DateTime.Now,
                       null,
                       null,
                       DateTime.Now,
                       Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(),
                       true);
                svc.WF_SaveProcessParameter(this.WorkflowPage.WF_ProcessID, CON.ProcessParameter.TransactionQueueID, tqId.ToString());
            }
        }
    }
    private void InitFormFields()
    {
        this.RegRestrictID = 0;
        txtEffectiveDate.Text = DateTime.Now.ToShortDateString();
        txtRSEndDate.Text = string.Empty;
        ddlRestrict.SelectedIndex = -1;
        ddlReviewType.SelectedIndex = -1;
        ddlReviewReason.SelectedIndex = -1;
        ddlRSStatus.SelectedIndex = -1;
        ddlIncludeExclude.SelectedIndex = -1;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        BindAllDropdowns();
        ClearErrorMessages();
    }

    private void SetFormFields(DataRow drService)
    {
        if (drService != null)
        {

            this.RegRestrictID = Helper.GetInt("REG_RESTRICTION_ID", drService);    
          
            this.ddlRestrict.SelectedItem.Value = Helper.GetString("IS_RESTRICT", drService);
          
            string effectiveDate = Helper.GetDate("EFFECTIVE_DATE", drService).ToString();
            string endDate = Helper.GetDate("END_DATE", drService).ToString();
            int  reviewtype = Helper.GetInt("REVIEW_TYPE_ID", drService);
            int reviewreason = Helper.GetInt("REVIEW_REASON_ID", drService);
            string statusCode = Helper.GetString("STATUS_CODE", drService);
            string isRestrict = Helper.GetString("IS_RESTRICT", drService);
            string isExclude = Helper.GetString("IS_EXCLUDE", drService);
            string reason = Helper.GetString("REVIEWREASON", drService);
            string Rreviewtypedesc = Helper.GetString("REVIEWTYPE",drService);
            string status = Helper.GetString("STATUS", drService);
            string ExcludeText = (string.IsNullOrEmpty(isExclude) || isExclude == "I")  ? "Include" : "Exclude";
            string restrictText = string.IsNullOrEmpty(isRestrict) ? "No" : (isRestrict.ToString() == "1"|| isRestrict.ToString() == "True") ? "Yes" : "No";

            if (!string.IsNullOrEmpty(effectiveDate))
            {
                txtEffectiveDate.Text = effectiveDate;
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                txtRSEndDate.Text = endDate;
            }
            if (reviewtype>0)
            {
               this.ddlReviewType.SelectedItem.Value = reviewtype.ToString();
               this.ddlReviewType.SelectedItem.Text = Rreviewtypedesc;
            }
            if (reviewreason>0)
            {
                this.ddlReviewReason.SelectedItem.Value = reviewreason.ToString();
                this.ddlReviewReason.SelectedItem.Text = reason;
              
            }
            if (!string.IsNullOrEmpty(status))
            {
               this.ddlRSStatus.SelectedItem.Value = statusCode;
               this.ddlRSStatus.SelectedItem.Text = status;
            }
            if (!string.IsNullOrEmpty(isRestrict))
            {
                this.ddlRestrict.SelectedItem.Value = isRestrict.ToString();
                this.ddlRestrict.SelectedItem.Text = restrictText;


            }
            if (!string.IsNullOrEmpty(isExclude))
            {
                this.ddlIncludeExclude.SelectedItem.Value = isExclude.ToString();
                this.ddlIncludeExclude.SelectedItem.Text = ExcludeText;
            }
            else
            {
                this.ddlIncludeExclude.SelectedIndex = ddlIncludeExclude.Items.IndexOf(ddlIncludeExclude.Items.FindByValue(isExclude.ToString()));
                this.ddlIncludeExclude.SelectedItem.Text = ExcludeText;

            }
        }
        upRestrictedService.Update();
    }
    private void InsertRegRestriction()
    {
        try
        {
            if (this.WorkflowPage.RegistrationId == 0)
            {
                Errors.Add("InsertRegRestriction", "Session Reg ID is null, unable to proceed with insert.");
                return;
            }
            Dictionary<string, string> parms = this.SetCommonParms();

            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Inserted.ToString());

            int RegRestrictionID = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "RESTRICTION", parms);
            if (RegRestrictionID > 0)
                SendTransactiontoSI();
            this.RegRestrictID = RegRestrictionID;

        }
        catch (Exception ex)
        {
            Errors.Add("InsertRegRestriction2", ex.Message);
        }
        upRestrictedService.Update();
    }
    private void SetErrorMessages()
    {

        if (Errors.Count > 0)
        {
            ClearErrorMessages();
            SetErrors();
        }

        if (ErrorEvent != null)
        {
            //have to keep the pop up open
            ErrorEvent();
            return;
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
    private void SetErrors()
    {
        foreach (var pair in Errors)
        {
            AddErrorMessage(pair.Value);
        }
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
    private bool ValidateData()
    {
        bool isValid = true;
        DateTime testdate;
        string effectiveDate;
        string endDate;
        int isexclude = 0;
        string status = string.Empty;
        string reviewType = string.Empty;
        string reviewReason = string.Empty;
        string isRestrict = string.Empty;
       
        status = ddlRSStatus.SelectedItem.Value;
        reviewType = ddlReviewType.SelectedItem.Value;
        reviewReason = ddlReviewReason.SelectedItem.Value;
        effectiveDate = txtEffectiveDate.Text;
        endDate = txtRSEndDate.Text;
        isRestrict = ddlRestrict.SelectedItem.Value;
        isexclude = ddlIncludeExclude.SelectedIndex;
        if (string.IsNullOrEmpty(isRestrict))
            AddError("* Restrict selection is required");

        if (isexclude<0)
            AddError("* Indication to include or exclude selection is required");

        if (string.IsNullOrEmpty(reviewType))
            AddError("* Review Type is required");

        if (string.IsNullOrEmpty(reviewReason))
            AddError("* Review Reason is required");

        if (string.IsNullOrEmpty(status))
            AddError("* Status is required");

        if (string.IsNullOrEmpty(effectiveDate))
            AddError("* Effective Date is required");
        else if (!DateTime.TryParse(effectiveDate, out testdate))
            AddError("* Effective Date is invalid");

        if (!string.IsNullOrEmpty(endDate) && !string.IsNullOrEmpty(effectiveDate))
            if (!DateTime.TryParse(endDate, out testdate))
                AddError("* End Date is invalid");
            else
            {
                DateTime d1, d2;
                if (DateTime.TryParse(effectiveDate, out d1) && DateTime.TryParse(endDate, out d2))
                    if (d2 < d1)
                        AddError("* Date span is invalid");
            }


        return isValid;

    }
   
    private void BindAllDropdowns()
    {

        try
        {
            if (_svc == null)
            {
                _svc = new PDMSService.PDMSServiceClient();
            }
            ddlIncludeExclude.Items.Clear();
            ddlReviewReason.Items.Clear();
            ddlReviewType.Items.Clear();
            ddlRSStatus.Items.Clear();
            DataTable dtReviewType = _svc.GetLTCReviewTypes().Tables[0];
            DataTable dtReviewReason = _svc.GetReviewReasons().Tables[0];
            DataTable dtstatus = _svc.GetRestrictedServiceStatuses().Tables[0];
            DataTable dtinclude = _svc.GetIncludeExclude().Tables[0];
            if (dtinclude != null && dtinclude.Rows.Count > 0)
            {
                Helper.LoadList(ddlIncludeExclude, dtinclude, "RESTRICT_INCLUDE_EXCLUDE_DESC", "RESTRICT_INCLUDE_EXCLUDE_CODE", true);
                //ddlIncludeExclude.Items.Insert(0, new ListItem("Select", string.Empty));
                //ddlIncludeExclude.SelectedIndex = 0;
            }
            if (dtReviewType != null && dtReviewType.Rows.Count > 0)
            {
                Helper.LoadList(ddlReviewType, dtReviewType, "dsc_Review_TYPE", "REVIEW_TYPE_ID", true);
                //ddlReviewType.Items.Insert(0, new ListItem("Select", string.Empty));
                //ddlReviewType.SelectedIndex = 0;
            }
            if (dtReviewReason != null && dtReviewReason.Rows.Count > 0)
            {
                Helper.LoadList(ddlReviewReason, dtReviewReason, "REVIEW_REASON_DESC", "REVIEW_REASON_ID", true);
                //ddlReviewReason.Items.Insert(0, new ListItem("Select", string.Empty));
                //ddlReviewReason.SelectedIndex = 0;
            }
            if (dtstatus != null && dtstatus.Rows.Count > 0)
            {
                Helper.LoadList(ddlRSStatus, dtstatus, "RESTRICTION_STATUS_TYPE_NAME", "RESTRICTION_STATUS_TYPE_ID", true);
                //ddlRSStatus.Items.Insert(0, new ListItem("Select", string.Empty));
                //ddlRSStatus.SelectedIndex = 0;
            }
        }
        catch(Exception)
        {

        }


    }

    protected void ddlRSStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlReviewType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        lblTitle.Text = "Restricted Service History";
        ucRestrictedServiceHistory.LoadData();
        mpe.Show();
    }


    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadData();
    }
}