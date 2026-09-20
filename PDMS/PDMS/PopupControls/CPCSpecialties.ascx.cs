using Amazon.Runtime.Internal.Transform;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_CPCSpecialties : BaseSectionControl
{
    private PDMSService.PDMSServiceClient _svc;
    public DataSet dsSpecialties = new DataSet();
    private string CPC_Program_Year = AppSettings.Get("CPCProgramYear");
    private DateTime EndDate = new DateTime(2299, 12, 31);

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public delegate void ReloadPopupEventHandler();
    public event ReloadPopupEventHandler ReloadPopupEvent;
    public bool OwnershipChangedFlag { get; set; }

    public bool IsPrimary
    {
        get
        {
            if (ViewState["IsPrimary"] == null) ViewState["IsPrimary"] = false;
            return Convert.ToBoolean(ViewState["IsPrimary"]);
        }
        set { ViewState["IsPrimary"] = value; }
    }
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

    public DataTable DataList1
    {
        get
        {
            string id = this.IdText + "_DataList1";
            if (ViewState[id] == null) return null;
            return (DataTable)ViewState[id];
        }
        set
        {
            string id = this.IdText + "_DataList1";
            ViewState[id] = value;
        }
    }
    public int ProviderTypeId
    {
        get
        {
            return this.WorkflowPage.ProviderTypeID;
        }
    }

    public bool HasPrimary
    {
        get
        {
            return HasPrimarySpecialty();
        }
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadSpecialties();        
    }


    protected bool HasPrimarySpecialty()
    {
        bool hasSpecialty = false;

        if (Helper.HasRows(this.DataList))
        {
            DataRow[] rowsPrimary = this.DataList.Select("PRIMARY_FLAG = 1");
            hasSpecialty = rowsPrimary.Length > 0;
        }

        return hasSpecialty;
    }

    public override void LoadData(DataRow dr)
    {
        if (dr != null)
        {
            hdnRegSpecialtyID.Value = Helper.GetData("REG_SPECIALTY_ID", dr);
            hdnEnrollStsID.Value = Helper.GetData("ENROLL_STATUS_ID", dr);
            hdnMMISSpecType.Value = Helper.GetData("MMIS_SPECIALTY_TYPE_ID", dr);
            int specialtyTypeID;

            if (!string.IsNullOrEmpty(Helper.GetData("SPECIALTY_TYPE_ID", dr)) &&
                 ddlSpecialty.Items.FindByValue(Helper.GetData("SPECIALTY_TYPE_ID", dr)) != null)
            {
                specialtyTypeID = Helper.GetInt("SPECIALTY_TYPE_ID", dr);
                ddlSpecialty.SelectedValue = specialtyTypeID.ToString();
            }

            if (!string.IsNullOrEmpty(Helper.GetData("START_DATE", dr)))
            {
                txtSpecStartDate.Text = Helper.GetDate("START_DATE", dr).ToString();
            }

            if (!string.IsNullOrEmpty(Helper.GetData("END_DATE", dr)))
            {
                txtSpecEndDate.Text = Helper.GetDate("END_DATE", dr).ToString();
            }

        }

    }
   
    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);

        DataRow dr = this.DataList.Rows[index];

        if (e.CommandName == "DeleteSpecialtiesRow")
        {
            bool IsDeleted = DeleteSpecialty(dr);
            if (IsDeleted) LoadSpecialties();
        }
        else
        {
            divSpecialtyDetail.Visible = true;
      
            txtSpecStartDate.Enabled = true;
            txtSpecEndDate.Enabled = dr["SENT_TO_SI"] == DBNull.Value ? false : Convert.ToBoolean(dr["SENT_TO_SI"]);

            if (Helper.HasRows(this.DataList))
                this.LoadData(dr);
            else
                this.LoadData(null);
        }
    }

    protected void grd_RowDataCreated(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Cells[4].Visible = e.Row.Cells[5].Visible = Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name);
        //}
        //else if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    e.Row.Cells[4].Visible = e.Row.Cells[5].Visible = Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name);
        //}
    }

    public override bool SaveData()
    {        
        return AddSpecialty();
    }

    private DataSet GetAllCPCSpecialties()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PrimaryFlag", "0");
        dsSpecialties = svc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTY", parms);

        return dsSpecialties;
    }
    private bool IsKidsCriteriaMet()
    {
        bool isCriteriaMet = false;
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("CPC_PROGRAM_YEAR", CPC_Program_Year);
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_CheckCPCIndividualMeetKidsCriteria", parms);
        if (Helper.HasRows(ds))
        {
            bool IsEligible = (bool)ds.Tables[0].Rows[0]["Meet_CPC_For_Kids_Criteria"];
            int kidscnt = (int)ds.Tables[0].Rows[0]["Total_Attributed_Kids"];
            isCriteriaMet = IsEligible && kidscnt > 150 ? true : false;
        }
        return isCriteriaMet;
    }
    private bool IsKidsSpecialtyPresent()
    {
        bool kidsSpecialtyPresent = false;  
        DateTime programYear = Convert.ToDateTime(AppSettings.Get("CPCProgramStartDate") + "/" + CPC_Program_Year);
       
        if (Helper.HasRows(dsSpecialties))
        {

            kidsSpecialtyPresent = dsSpecialties.Tables[0].AsEnumerable()
                                          .Where(r => r.Field<string>("MMIS_SPECIALTY_TYPE_ID") == CON.MMISSpecialtyType.CPCPEDIATRICS &&
                                                      r.Field<DateTime>("START_DATE") <= programYear &&
                                                      r.Field<DateTime>("END_DATE") == EndDate &&
                                                      r.Field<Int32>("ENROLL_STATUS_ID") == CON.EnrollmentStatusTypeID.Active &&
                                                      (r["SENT_TO_SI"] != DBNull.Value ? Convert.ToBoolean(r["SENT_TO_SI"]) : true) == true)
                                          .Any() || dsSpecialties.Tables[0].AsEnumerable()
                                          .Where(r => r.Field<string>("MMIS_SPECIALTY_TYPE_ID") == CON.MMISSpecialtyType.CPCPEDIATRICS &&
                                                      r.Field<DateTime>("START_DATE") == programYear &&
                                                      r.Field<DateTime>("END_DATE") == EndDate &&
                                                      r.Field<Int32>("ENROLL_STATUS_ID") == CON.EnrollmentStatusTypeID.InActive &&
                                                      (r["SENT_TO_SI"] != DBNull.Value ? Convert.ToBoolean(r["SENT_TO_SI"]) : false) == false)
                                          .Any();

        }
        return kidsSpecialtyPresent;
    }

    private bool AddSpecialty()
    {
        if (ValidateData())
        {
            try
            {
                if(string.IsNullOrEmpty(hdnRegSpecialtyID.Value))
                {
                    if (!IsKidsSpecialtyPresent() && ddlSpecialty.SelectedIndex != 0)
                    {
                        // DateTime EndDate = new DateTime(2299, 12, 31);

                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms = new Dictionary<string, string>();
                        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                        parms.Add("PRIMARY_FLAG", "0");
                        parms.Add("SPECIALTY_TYPE_ID", ddlSpecialty.SelectedValue);
                        parms.Add("SPECIALTY_BOARD_CERTIFIED", "Y");
                        parms.Add("START_DATE", Convert.ToDateTime(AppSettings.Get("CPCProgramStartDate") + "/" + CPC_Program_Year).ToString());
                        parms.Add("END_DATE", EndDate.ToString());
                        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Inserted.ToString());
                        parms.Add("ENROLL_STATUS_ID", CON.EnrollStatus.INACTIVE.ToString());
                        parms.Add("ENROLLMENT_STATUS_REASONS_ID", CON.EnrollStatusReasonID.InActive.ToString());
                        svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTYCustom2", parms);
                        

                        LoadSpecialties();
                        divSpecialtyDetail.Visible = false;

                    }
                }
                else
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                    parms.Add("PRIMARY_FLAG", "0");
                    parms.Add("SPECIALTY_TYPE_ID", ddlSpecialty.SelectedValue);
                    parms.Add("SPECIALTY_BOARD_CERTIFIED", "Y");
                    parms.Add("START_DATE", txtSpecStartDate.Text);
                    parms.Add("END_DATE", txtSpecEndDate.Text);
                    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
                    parms.Add("REG_SPECIALTY_ID", hdnRegSpecialtyID.Value);
                    svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTYCustom", parms);

                    LoadSpecialties();
                    divSpecialtyDetail.Visible = false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("ucSpecialties_SaveData - " + ex.Message));
            }

        }

        return false;
    }

    public override bool HasInputValue()
    {
        return false;
    }

    public override bool ValidateData()
    {
        if (this.WorkflowPage.EntityTypeID == CON.ProviderCategoryTypeID.Individual && (IsKidsSpecialtyPresent() || ddlSpecialty.SelectedIndex != 0))
        {           
            if (IsKidsCriteriaMet())
            {
                return true;
            }
            else
            {
                if (!string.IsNullOrEmpty(hdnRegSpecialtyID.Value) && Convert.ToDateTime(txtSpecEndDate.Text) < EndDate
                     && hdnMMISSpecType.Value == CON.MMISSpecialtyType.CPCPEDIATRICS && hdnEnrollStsID.Value == CON.EnrollStatus.ACTIVE.ToString())
                {
                    return true;
                }
                else
                {
                    AddErrorPopUp(GetGlobalResourceObject("BrandingResource", "DenialMessageCPC").ToString());
                    return false;
                }
            }
        }
        else
        {
            return true;
        }
    }

    public override string ValidationGroup
    {
        get { return "valSpecialties"; }
    }

    public override string Title
    {
        get { return "Edit Specialty"; }

    }

    public override string IdText
    {
        get { return "ucSpecialties_" + this.WorkflowPage.RegistrationId; }
    }

    public bool CanCPCUserViewDelete(int index)
    {
        DataTable dt = this.DataList;
        bool primaryFlag = Convert.ToBoolean(dt.Rows[index]["PRIMARY_FLAG"]);
        bool sentToSIflag = dt.Rows[index]["SENT_TO_SI"] == DBNull.Value ? false : Convert.ToBoolean(dt.Rows[index]["SENT_TO_SI"]);
        bool specFrmPrevYear = Convert.ToDateTime(dt.Rows[index]["START_DATE"]).Year < Convert.ToInt32(CPC_Program_Year) ? true : false;

        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            if (primaryFlag)
                return false;
            else
                return !sentToSIflag && !specFrmPrevYear;
        }
        else
            return false;
    }
    public bool CanCPCUserViewEdit(int index)
    {
        DataTable dt = this.DataList;

        string primaryFlag = dt.Rows[index]["PRIMARY_FLAG"].ToString();
        bool sentToSIflag = dt.Rows[index]["SENT_TO_SI"] == DBNull.Value ? false : Convert.ToBoolean(dt.Rows[index]["SENT_TO_SI"]);
        string enrollStatus = dt.Rows[index]["ENROLL_STATUS_DESC"].ToString();
        string mmisSpecTyp = dt.Rows[index]["MMIS_SPECIALTY_TYPE_ID"].ToString();

        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName) && mmisSpecTyp == CON.MMISSpecialtyType.CPCPEDIATRICS)
        {

            if (Convert.ToBoolean(primaryFlag))
                return false;
            
            else if (!Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name))
            {
                // OHPNM-13913 - Provider should be able to edit specialty in Enrollment period or when enrollment reenabled.
                // OHPNM-19639 - allow provider to edit while in provider data entry step in cpc workflow.
                if (Helper.isCPCEnrollmentPeriod() || Helper.isCPCLinkReenabled(this.WorkflowPage.RegistrationId) 
                     || (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry && this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CPC))
                 return true;
               else
                  return false;
            }
            else
                return true;
        }
        else
            return false;

    }
    public bool CanCPCUserViewEnrollCol(int index)
    {
        return Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name);
    }
    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        if (!IsKidsCriteriaMet())
        {
            lblIsKidsCriteria.Text = GetGlobalResourceObject("BrandingResource", "CPCKidsCriteriaErrorMessage").ToString();
            lblIsKidsCriteria.Visible = true;
            kidsSplAddnew.Visible = false;
        }
        else
        {
            lblIsKidsCriteria.Visible = false;
            divSpecialtyDetail.Visible = true;
            txtSpecStartDate.Text = AppSettings.Get("CPCProgramStartDate") + "/" + CPC_Program_Year;
            txtSpecStartDate.Enabled = false;
            txtSpecEndDate.Text = EndDate.ToString("MM/dd/yyyy");
            txtSpecEndDate.Enabled = false;
        }
    }

    #region Private Methods

    private bool DeleteSpecialty(DataRow dr)
    {
        bool isValid = true;
        if (this.DataList.Rows.Count == 1 && Registration.EntryIsRequired(this.WorkflowPage.RegistrationId, CON.RegistrationPageName.Certification, Registration.GetSectionNameFromStepNumber(CON.SectionTypeID.Specialties)))
        //if only one record and required page then prevent deletion or else the page will not turn back to blue once it is green and will cause issues for page submission
        {
            AddError("* This is required section. This specialty cannot be deleted.", ref isValid, ValidationGroup);
            return isValid;
        }
        if (dr != null)
        {
            int REG_SPECIALTY_ID = Helper.GetInt("REG_SPECIALTY_ID", dr);
            int SPECIALTY_ID = Helper.GetInt("SPECIALTY_ID", dr);
            bool IsPrimary = Helper.GetBool("PRIMARY_FLAG", dr);
            bool SentToSI = Helper.GetBool("SENT_TO_SI", dr);
            if (SPECIALTY_ID == 0 && !IsPrimary)
            {

                if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CPC && this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.NewReg )
                {
                    if (this.WorkflowPage.CurrentTaskName == "Provider Data Entry")
                        svc.DeleteRegistrationData("SPECIALTY", "REG_SPECIALTY_ID", REG_SPECIALTY_ID);
                }
                else
                {
                    if(!SentToSI)
                    {
                        Dictionary<string, string> parms1 = new Dictionary<string, string>();
                        parms1.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                        parms1.Add("SPECIALTY_TYPE_ID", Helper.GetInt("SPECIALTY_TYPE_ID", dr).ToString());
                        parms1.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parms1.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        parms1.Add("REG_SPECIALTY_ID", REG_SPECIALTY_ID.ToString());
                        parms1.Add("START_DATE", Helper.GetDateTime("START_DATE", dr).ToString());
                        svc.DeleteRegistrationDataWithParams("SPECIALTYCustom_CPC", parms1);
                    }    
                }
            }
            else
            {
                if (IsPrimary)
                    AddError("* The Primary Specialty can not be deleted.", ref isValid, ValidationGroup);
                else
                    AddError("* Specialty cannot be deleted.", ref isValid, ValidationGroup);

                return isValid;
            }
        }
        return isValid;
    }
    private void AddErrorPopUp(string errorMessage)
    {
        MessageBox2.Show(UserControls_MessageModal.MessageModalMode.Continue, "Alert", errorMessage);
    }


    private void LoadSpecialties()

    {
        if (!pnlSpecialties.Visible) return;


        DataSet dataset;
        string mmisSpecialtyId = CON.MMISSpecialtyType.CPCPEDIATRICS.ToString();
        ddlSpecialty.Items.Clear();
        divSpecialtyDetail.Visible = false;
        if (ddlSpecialty.Items.Count == 0)
        {
            dataset = svc.GetSpecialtiesByMMISId(mmisSpecialtyId);
            Helper.LoadList(ddlSpecialty, dataset.Tables[0], "SPECIALTY_TYPE_NAME", "SPECIALTY_TYPE_ID", true);
        }
        dsSpecialties = GetAllCPCSpecialties();
        if (Helper.HasRows(dsSpecialties))
        {
            DateTime programYear = Convert.ToDateTime(AppSettings.Get("CPCProgramStartDate") + "/" + CPC_Program_Year);
            var nextYearEnrollment = dsSpecialties.Tables[0].AsEnumerable()
                .Where(r => r.Field<DateTime>("END_DATE") == Convert.ToDateTime("12/31/2299"));
            if (nextYearEnrollment.Any())
            {
                var nextYearEnrollmentTable = this.DataList = nextYearEnrollment.CopyToDataTable();
                grdSpecialties.DataSource = nextYearEnrollmentTable;

            }
            else
            {
                grdSpecialties.DataSource = this.DataList = null;
            }

            var previousYearEnrollment = dsSpecialties.Tables[0].AsEnumerable()
                .Where(r => r.Field<DateTime>("START_DATE") < programYear);
            if (previousYearEnrollment.Any())
            {
                var nextYearEnrollmentTable = previousYearEnrollment.CopyToDataTable();
                grdLastYearSpecialties.DataSource = nextYearEnrollmentTable;
            }
            else
            {
                grdLastYearSpecialties.DataSource = null;
            }
        }
        else
        {
            grdSpecialties.DataSource = this.DataList = null;
            grdLastYearSpecialties.DataSource = this.DataList = null;
        }
        grdSpecialties.DataBind();
        grdLastYearSpecialties.DataBind();
        divSpecAddNew.Visible = !IsKidsSpecialtyPresent();
    }

    private void AddError(string errMsg, ref bool isGood, string validationGroup)
    {
        var validator = new CustomValidator
        {
            IsValid = false,
            ErrorMessage = errMsg,
            ValidationGroup = validationGroup
        };
        this.Page.Validators.Add(validator);
        isGood = false;
    }

    #endregion

}