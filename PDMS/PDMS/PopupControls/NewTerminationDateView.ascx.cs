using Corp.Core.Libraries;
using Corp.Core.Libraries.Helper;
using Glimpse.Core.ClientScript;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_NewTerminationDateView : System.Web.UI.UserControl
{
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
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Parent Page Events
    public delegate void UpdateSuccessEventHandler(string message);
    public event UpdateSuccessEventHandler UpdateSuccessEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    public delegate void KeepPopupOpenEventHandler();
    public event KeepPopupOpenEventHandler KeepPopupOpenEvent;

    #endregion

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
    public void InitView(int regID)
    {
        this.RegID = regID;
        InitFormFields();
        SetProviderInformation(regID);
    }

    private void InitFormFields()
    {
        this.lblErrorMessages.Text = string.Empty;
        this.txtTermDate.Text = string.Empty;
        this.txtTermDate.Enabled = true;
        this.btnSave.Enabled = true;

    }
    public void SetProviderInformation(int regID)
    {
        this.lblRegID.Text = regID.ToString();
        DataSet ds = svc.SelectRegistrationByRegID(regID);
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if (Methods.ColumnExists("TerminationDate", dr))
            {
                if (dr["TerminationDate"] != DBNull.Value)
                {
                    this.lblTermDate.Text = Methods.GetDateValue(dr["TerminationDate"]).ToString("MM/dd/yyyy");
                }
            }
        }
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
        if (string.IsNullOrEmpty(txtTermDate.Text))
        {
            lblErrorMessages.Text = "Please enter a New Termination date";
            if (KeepPopupOpenEvent != null)
            {
                //have to keep the pop up open
                KeepPopupOpenEvent();
                return;
            }
        }
        else if (!string.IsNullOrEmpty(txtTermDate.Text) && txtTermDate.Text == lblTermDate.Text)
        {
            lblErrorMessages.Text = "New Termination date cannot be same as Current Termination Date";
            if (KeepPopupOpenEvent != null)
            {
                //have to keep the pop up open
                KeepPopupOpenEvent();
                return;
            }
        }
        else if (string.IsNullOrEmpty(txtComments.Text))
        {
            lblErrorMessages.Text = "Please enter comments.";
            if (KeepPopupOpenEvent != null)
            {
                //have to keep the pop up open
                KeepPopupOpenEvent();
                return;
            }
        }
        else
        {
            lblErrorMessages.Text = string.Empty;
            DateTime newTermDate;
            DateTime.TryParse(this.txtTermDate.Text.Trim(), out newTermDate);

            DataSet dsTransID = RegistrationController.ChangeProviderTerminationDate(this.RegID, newTermDate, txtComments.Text, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name), true);
            int transactionID = 0;
            if (Methods.HasRows(dsTransID))
            {
                foreach (DataRow dr in dsTransID.Tables[0].Rows)
                {
                    transactionID = Methods.GetIntValue(dr, "TRANSACTION_ID");
                    int transactionTypeID = Methods.GetIntValue(dr, "TRANSACTION_TYPE_ID");
                    if (transactionID > 0)
                    {
                        SendTerminationTransactionPayload stp = new SendTerminationTransactionPayload();
                        stp.SendTransaction(transactionID, transactionTypeID, true);
                    }
                }
            }

            SetUpdateResults();

            ProviderFeedHelper.InsertProviderFeedNotes(this.RegID, 0, HttpContext.Current.User.Identity.Name, txtComments.Text, enrollmentType: CON.AdminMaintenanceActions.NewTerminationDate);
        }
        
    }
    public void SetUpdateResults()
    {
        this.btnSave.Enabled = false;
        this.txtTermDate.Enabled = false;
        if (UpdateSuccessEvent != null)
        {
            UpdateSuccessEvent(Resources.BrandingResource.TERMDATE_UPDATE_SUCCESS);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }
}