using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;
public partial class PopupControls_CPCGroupMember : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    public GroupAndFacilityAffiliations Model { get; set; }

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
    public int RegAffiliationID
    {
        get
        {
            return ViewState["RegAffiliationID"] == null ? 0 : Convert.ToInt32(ViewState["RegAffiliationID"]);
        }
        set
        {
            ViewState["RegAffiliationID"] = value;
        }
    }

    private string CPC_Program_Year = AppSettings.Get("CPCProgramYear");


    #region Parent Page Events


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
    private Dictionary<string, string> Errors = new Dictionary<string, string>();

    protected void Page_Load(object sender, EventArgs e)
    {
      
    }
    private void ShowError(string errMsgKey)
    {
          lblErrorMessages.Text = (string)GetGlobalResourceObject("BrandingResource", errMsgKey);
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
    }

    public override void LoadData(System.Data.DataRow dr = null)
    {
        //throw new NotImplementedException();
    }

    public override bool SaveData()
    {
        InsertRegAffiliation();
        return true;

    }
    private void InsertRegAffiliation()
    {
        try
        {
            DateTime EndDate = new DateTime(2299, 12, 31);

            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID",this.WorkflowPage.RegistrationId.ToString());
            parms.Add("Medicaid_ID", hdnMemMedicaidID.Value);
            parms.Add("NAME", txtMemberName.Text.Trim());
            parms.Add("START_DATE", txtStartDate.Text);
            parms.Add("END_DATE", EndDate.ToString());
            parms.Add("GROUP_AFFILIATION_STATUS_ID", CON.GroupAffiliationStatusTypeID.GroupConfirmed.ToString());
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("CPC_PROGRAM_YEAR", CPC_Program_Year);
            parms.Add("COUNT_ATTRIBUTION", hdnMemTotalAttribution.Value);
            parms.Add("COUNT_ATTRIBUTION_KIDS", hdnMemKidsAttribution.Value);
            int regAffiliationID = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "AFFILIATIONcustom", parms);
            LoadControlData();
        }
        catch (Exception ex)
        {
            Errors.Add("InsertRegAffiliation1", ex.Message);
        }
    }
    public override bool HasInputValue()
    {
        return true;
    }
    public override bool ValidateData()
    {
        bool isEligibleMember = false;
        DataSet ds = svc.ValidatePracticePartners(Convert.ToInt32(hdnMemberRegId.Value),this.WorkflowPage.RegistrationId);
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            hdnMemTotalAttribution.Value = Helper.GetString("Total_Attributed_Members", dr);
            hdnMemKidsAttribution.Value = Helper.GetString("Total_Attributed_Kids", dr);
            hdnMemMedicaidID.Value = Helper.GetString("AffiliationMedicaidID", dr);
            if (Helper.GetInt("IsAvailable", dr) == 0)
            {
                ShowError("CPCExistsMessage");
                return isEligibleMember;
            }
            else if (Helper.GetInt("IsAvailable", dr) == 2)
            {
                ShowError("CPCMemberStartedonOwn");
                return isEligibleMember;
            }
            else if (Helper.GetInt("Total_Attributed_Members", dr) <150)
            {
                ShowError("CPCNewMember150ClaimsEligibility");
                return isEligibleMember;
            }
            else
            {
                isEligibleMember = true;
            }
        }
        else
        {
            lblErrorMessages.Text = "The provider searched is not found, cannot be added to your Practice";
        }        
        return isEligibleMember;
    }
    protected void txtMedicaidID_TextChanged(object sender, EventArgs e)
    {
        
        DataSet ds = svc.GetProviderNameByMedicaidID(txtMedicaidID.Text);
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtMemberName.Text = Helper.GetString("Name", dr);
            hdnMemberRegId.Value = Helper.GetString("Reg_Id", dr);
            txtStartDate.Text = AppSettings.Get("CPCProgramStartDate") + "/" + CPC_Program_Year;
            if (!string.IsNullOrEmpty(lblErrorMessages.Text)) lblErrorMessages.Text = string.Empty;
        }
        else
        {
            lblErrorMessages.Text = "Member Not Found.";
        }
        
    }

    public override string Title
    {
        get { return "CPC Group Details"; }
    }

    public override string IdText
    {
        get { return "ucCPCMember_" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
        get { return "CPCGroupMember"; }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
        ClearControls();
  
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ValidateData())
        {

            Page.Validate("CPCGroupMember");

            if (!Page.IsValid)
            {
                if (ValidationEvent != null)
                {
                    ValidationEvent();
                }
                return;
            }

            bool rtn = SaveData();
            if (!rtn)
                return;

            if (SaveEvent != null)
            {
                SaveEvent();
                ClearControls();
            }
        }
    }

   private void ClearControls()
    {
        txtMedicaidID.Text = "";
        txtMemberName.Text = "";
        txtStartDate.Text = "";
        lblErrorMessages.Text = string.Empty;
    }
    protected void rblMedicaidId_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtMedicaidID.Enabled = true;
    }
}