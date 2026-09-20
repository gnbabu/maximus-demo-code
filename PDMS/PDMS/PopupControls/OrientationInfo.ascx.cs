using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OrientationInfo : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public delegate void ReloadPopupEventHandler();
    public event ReloadPopupEventHandler ReloadPopupEvent;
    public bool OwnershipChangedFlag { get; set; }

     public int RegProgramStatusTypeID
    {
        get
        {
            return ViewState["RegProgramStatusTypeID"] == null ? 0 : Convert.ToInt32(ViewState["RegProgramStatusTypeID"]);
        }
        set
        {
            ViewState["RegProgramStatusTypeID"] = value;
        }
    }
    
    public bool IndividualProviderType
    {
        get
        {
            return ViewState["IndividualProviderType"] == null ? false : Convert.ToBoolean(ViewState["IndividualProviderType"].ToString());
        }
        set { ViewState["IndividualProviderType"] = value; }
    }

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (!IsPostBack)
    //    {
    //        LoadDropDowns();
    //    }
    //}

    private void LoadDropDowns()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectOrientationStatusTypes();
        Helper.LoadList(ddlOrientationStatus, ds.Tables["OrientationStatusTypes"], "ORIENTATION_STATUS_TYPE", "ORIENTATION_STATUS_TYPE_ID", true);

    }

    public override void LoadData(DataRow dr)
    {
        if (dr != null)
        {
            LoadDropDowns();
            this.hdnRegOrientationID.Value = Helper.GetString("REG_ORIENTATION_ID", dr);
            this.lblOrgName.Text = Helper.GetString("PROVIDER_NAME", dr);
            this.txtDueByDate.Text = Helper.GetDate("ORIENTATION_DUE_DATE", dr);
            this.txtScheduleDate.Text = Helper.GetDate("ORIENTATION_COMPLETED_DATE", dr);
            this.txtResponseDate.Text = Helper.GetDate("ORIENTATION_RESPONSE_DATE", dr);
            this.ddlOrientationStatus.SelectedValue = Helper.GetString("ORIENTATION_STATUS_TYPE_ID", dr);
        }
    }

    private bool IsExternalUser()
    {
        return true;
    }

    private bool IsInternalUser()
    {
        return true;
    }

    private bool IsNewConvertedProvider(int partyID)
    {
        //reg program status of conversion AND no party id
        return Registration.IsConversionProvider(RegProgramStatusTypeID) && partyID <= 0;
    }

    // Todo This is only temporary, will be moved to a common file
    private bool IsIndividualProvider(DataRow dr)
    {
        int entityTypeID = Helper.GetInt("ENTITY_TYPE_ID", dr);
        return entityTypeID == CON.ProviderCategoryTypeID.Individual || entityTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile;
    }

    //private bool IsNewReg(int partyID)
    //{
    //    return string.IsNullOrEmpty(MedicaidId) && partyID == 0;
    //}

    public bool SaveData()
    {
        if (!ValidateData())
        {
            return false;
        }

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ORIENTATION");
        bool doUpdate = false;
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        string regOrientationID = "";
        DataRow row = null;

        if (Helper.HasRows(ds))
        {
            row = ds.Tables[0].Rows[0];
            regOrientationID = Helper.GetString("REG_ORIENTATION_ID", row);
            if (regOrientationID != "")
            {
                doUpdate = true;
                parms.Add("REG_ORIENTATION_ID", regOrientationID);
            }
        }

        if (txtDueByDate.Text != "")
        {
            parms.Add("ORIENTATION_DUE_DATE", txtDueByDate.Text);
        }
        if (txtScheduleDate.Text != "")
        {
            parms.Add("ORIENTATION_COMPLETED_DATE", txtScheduleDate.Text);
        }
        if (txtResponseDate.Text != "")
        {
            parms.Add("ORIENTATION_RESPONSE_DATE", txtResponseDate.Text);
        }
        if (ddlOrientationStatus.SelectedValue != string.Empty)
        {
            parms.Add("ORIENTATION_STATUS_TYPE_ID", ddlOrientationStatus.SelectedValue);
        }
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        if (doUpdate)
        {
            psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "ORIENTATION", parms);
        }
        else
        {
            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            psc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "ORIENTATION", parms);
        }

        return true;
    }
    
    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valOrgInfo";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    public bool ValidateData()
    {
       // PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

       bool isGood = true;
       if (ddlOrientationStatus.SelectedValue == CON.OrientationStatusType.PacketSent.ToString() && string.IsNullOrEmpty(txtDueByDate.Text))
       {
           CustomValidator val = new CustomValidator();
           val.IsValid = false;
           val.ErrorMessage = "Due by date is required.";
           val.ValidationGroup = "valOrientationInfo";
           this.Page.Validators.Add(val);
           isGood = false;
       }
       if ((ddlOrientationStatus.SelectedValue == CON.OrientationStatusType.Scheduled.ToString() || ddlOrientationStatus.SelectedValue == CON.OrientationStatusType.ReScheduled.ToString())
           && (string.IsNullOrEmpty(txtDueByDate.Text) || string.IsNullOrEmpty(txtScheduleDate.Text)))
       {
           CustomValidator val = new CustomValidator();
           val.IsValid = false;
           val.ErrorMessage = "Due by date and Scheduled Date is required.";
           val.ValidationGroup = "valOrientationInfo";
           this.Page.Validators.Add(val);
           isGood = false;
       }
       if ((ddlOrientationStatus.SelectedValue == CON.OrientationStatusType.Completed.ToString() || ddlOrientationStatus.SelectedValue == CON.OrientationStatusType.DHCFSignedAgreementForDMEOnly.ToString()  || ddlOrientationStatus.SelectedValue == CON.OrientationStatusType.ProviderSignedAgreementForDMEOnly.ToString())
   && (string.IsNullOrEmpty(txtDueByDate.Text) || string.IsNullOrEmpty(txtScheduleDate.Text) || string.IsNullOrEmpty(txtResponseDate.Text)))
       {
           CustomValidator val = new CustomValidator();
           val.IsValid = false;
           val.ErrorMessage = "Due by date ,Scheduled Date and Response Date is required.";
           val.ValidationGroup = "valOrientationInfo";
           this.Page.Validators.Add(val);
           isGood = false;
       }
       if ((ddlOrientationStatus.SelectedValue == CON.OrientationStatusType.Failed.ToString())
   && (string.IsNullOrEmpty(txtComments.Text)))
       {
           CustomValidator val = new CustomValidator();
           val.IsValid = false;
           val.ErrorMessage = "Comments is required.";
           val.ValidationGroup = "valOrientationInfo";
           this.Page.Validators.Add(val);
           isGood = false;
       }
       return isGood;
    }

    private void Validate_BirthDateAfterDeathDate(ref bool isGood)
    {
        //DateTime? birthDate = null;
        //DateTime? deathDate = null;

        //if (string.IsNullOrEmpty(txtDeathDate.Text.Trim()) || string.IsNullOrEmpty(txtBirthDate.Text.Trim()))
        //{
        //    return;
        //}

        //DateTime tmp;
        //if (DateTime.TryParse(this.txtBirthDate.Text.Trim(), out tmp))
        //{
        //    birthDate = tmp;
        //}
        //if (DateTime.TryParse(this.txtDeathDate.Text.Trim(), out tmp))
        //{
        //    deathDate = tmp;
        //}

        //if (!birthDate.HasValue || !deathDate.HasValue)
        //{
        //    //date parsing failed on one or both values
        //    AddError("Invalid birth and/or death date", ref isGood);
        //}
        //else if (deathDate.Value < birthDate.Value)
        //{
        //    AddError("Date of Death can not be prior to Birth Date", ref isGood);
        //}

        //return;
    }
    private void Validate_BirthDateDeathDate(ref bool isGood)
    {
        //DateTime? birthDate = null;
        //DateTime? deathDate = null;

        //if (string.IsNullOrEmpty(txtDeathDate.Text.Trim()) && string.IsNullOrEmpty(txtBirthDate.Text.Trim()))
        //{
        //    return;
        //}

        //DateTime tmp;
        //if (DateTime.TryParse(this.txtBirthDate.Text.Trim(), out tmp))
        //{
        //    birthDate = tmp;
        //}
        //if (DateTime.TryParse(this.txtDeathDate.Text.Trim(), out tmp))
        //{
        //    deathDate = tmp;
        //}

        //if (birthDate.HasValue)
        //{
        //    if (birthDate.Value < DateTime.Now.AddYears(-100) || birthDate.Value > DateTime.Now)
        //    {
        //        AddError("* Birth Date can not be a future date and cannot result in an age over 100 years.", ref isGood);
        //    }

        //}
        //if (deathDate.HasValue)
        //{
        //    if (deathDate.Value > DateTime.Now)
        //    {
        //        AddError("* Death Date can not be a future date.", ref isGood);
        //    }
        //}
        

        //return;
    }
}