using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public partial class PopupControls_FeeInformation : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
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

    public int RegAchFeeInformationID
    {
        get
        {
            return ViewState["RegAchFeeInformationID"] == null ? 0 : Convert.ToInt32(ViewState["RegAchFeeInformationID"]);
        }
        set
        {
            ViewState["RegAchFeeInformationID"] = value;
        }
    }


    public DateTime RecordCreateDateTime
    {
        get
        {
            return ViewState["RecordCreateDateTime"] == null ? DateTime.Now : Convert.ToDateTime(ViewState["RecordCreateDateTime"]);
        }
        set
        {
            ViewState["RecordCreateDateTime"] = value;
        }
    }


    public string RecordCreateUser
    {
        get
        {
            return ViewState["RecordCreateUser"] == null ? string.Empty :  (ViewState["RecordCreateUser"]).ToString() ;
        }
        set
        {
            ViewState["RecordCreateUser"] = value;
        }
    }


    public bool IsAddRecord
    {
        get { return RegAchFeeInformationID == 0 ? true : false; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public bool SaveData()
    {
        
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PAYMENT_DATE", txtPaymentDate.Text);
        parms.Add("EDISON_NUMBER", txtDepositID.Text.Trim());
        parms.Add("PAYMENT_NUMBER", txtPaymentID.Text.Trim());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        if (IsAddRecord)
        {
            parms.Add("CREATE_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATE_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            // Insert
            svc.InsertRegistrationDataTable("ACH_FEE_INFORMATION", parms);
        }
        else
        {
            parms.Add("REG_ACH_FEE_INFORMATION_ID", RegAchFeeInformationID.ToString());
            parms.Add("CREATE_DATE_TIME", RecordCreateDateTime.ToString());
            parms.Add("CREATE_USER", RecordCreateUser);

            //update
            svc.UpdateRegistrationDataTable("ACH_FEE_INFORMATION", parms);
        }

        // Now go update PDMS to keep Registration and PDMS in sync
        DataRow dr = Registration.GetRegistration(this.WorkflowPage.RegistrationId);
        parms = new Dictionary<string, string>();
        parms.Add("RegID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PartyID", Helper.GetString("PARTY_ID", dr));
        svc.UpdateRegistrationDataWithParams("usp_TransferRegToLive_ACH_FEE_INFORMATION", parms);

        return true;
    }

    public override void LoadData(DataRow row)
    {
        txtPaymentDate.Text = txtDepositID.Text = txtPaymentID.Text = string.Empty;
        RegAchFeeInformationID = 0;
        RecordCreateDateTime = DateTime.Now;
        RecordCreateUser = string.Empty;

        if (row != null)
        {
            RegAchFeeInformationID = Helper.GetInt("REG_ACH_FEE_INFORMATION_ID", row);
            txtPaymentDate.Text = Helper.GetDateTime("PAYMENT_DATE", row).ToString("MM/dd/yyyy");
            txtDepositID.Text = Helper.GetString("EDISON_NUMBER", row);
            txtPaymentID.Text = Helper.GetString("PAYMENT_NUMBER", row);

            RecordCreateDateTime = Helper.GetDateTime("CREATE_DATE_TIME", row);
            RecordCreateUser = Helper.GetString("CREATE_USER", row);
        }
        cvFutureDate.ValueToCompare = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
    }

}