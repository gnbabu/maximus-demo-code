using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_CMCSpecialties : BaseSectionControl
{
    private PDMSService.PDMSServiceClient _svc;
    public DataSet dsSpecialties = new DataSet();
 
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public delegate void ReloadPopupEventHandler();
    public event ReloadPopupEventHandler ReloadPopupEvent;

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
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {    
        LoadSpecialties();
        SaveData();
        LoadCMCQualifyingEnrollmentCount();
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

    }

    public override bool HasInputValue()
    {
        return true;
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

    public override bool SaveData()
    {
        bool res = false;
        if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CMCEnroll || this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CMCUpdate
            || this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CMCReAttest)
        { 
            Registration.SetProviderSectionNodeStatusId(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, CON.RegistrationProviderStatusTypeId.Complete);
            res = true;
        }      
        else
        {
            return false;
        }

        return res;
    }

    public override bool ValidateData()
    {
        return true;
    }


    #region Private Methods
    private void LoadSpecialties()
    {
        if (!pnlSpecialties.Visible) return;

        dsSpecialties = GetAllCMCSpecialties();

        if (Helper.HasRows(dsSpecialties))
        {
            //Check if MIS Flag is present
            DataRow[] dataRowsMIS = dsSpecialties.Tables[0].Select("MMIS_SPECIALTY_TYPE_ID = '" + CON.MMISSpecialtyType.MATERNALANDINFANTSUPPORT + "'");
            if (dataRowsMIS.Length == 0)
            {
                //call SP and do insert and then do another get
                //InsertRegSpeciality(CON.MMISSpecialtyType.MATERNALANDINFANTSUPPORT);
                //dsSpecialties = GetAllCMCSpecialties();
                DateTime EndDate = new DateTime(2299, 12, 31);
                DataRow dr = dsSpecialties.Tables[0].NewRow();
                dr["SPECIALTY_TYPE_NAME"] = "MATERNAL AND INFANT SUPPORT";
                dr["PRIMARY_FLAG"] = 0;
                dr["MMIS_SPECIALTY_TYPE_ID"] = "MIS";
                dr["START_DATE"] = DateTime.Now.ToString();
                dr["END_DATE"] = EndDate.ToString();
                dsSpecialties.Tables[0].Rows.Add(dr);
            }

            for (int i = 0; i < dsSpecialties.Tables[0].Rows.Count; i++)
            { 
                if(dsSpecialties.Tables[0].Rows[i]["MMIS_SPECIALTY_TYPE_ID"].ToString()==CON.MMISSpecialtyType.MATERNALANDINFANTSUPPORT)
                {
                    DateTime EndDate = new DateTime(2299, 12, 31);
                    if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CMCReAttest)
                    {
                        dsSpecialties.Tables[0].Rows[i]["START_DATE"] = Convert.ToDateTime(AppSettings.Get("CMCReAttestSpecialtyStartDate")).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        dsSpecialties.Tables[0].Rows[i]["START_DATE"] = Convert.ToDateTime(AppSettings.Get("CMCInitiateSpecialtyStartDate")).ToString("MM/dd/yyyy");
                    }
                    dsSpecialties.Tables[0].Rows[i]["END_DATE"] = EndDate.ToString();
                    //if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CMCReattest)
                    //{
                    //    DateTime EndDate = new DateTime(2299, 12, 31);
                    //    dsSpecialties.Tables[0].Rows[i]["END_DATE"] = EndDate.ToString();
                    //}
                    //else
                    //{ 
                    //    dsSpecialties.Tables[0].Rows[i]["END_DATE"] = Convert.ToDateTime(AppSettings.Get("CMCProgramEndDate")).AddYears(1).ToString("MM/dd/yyyy");
                    //}
                    break;
                }            
            }
            grdSpecialties.DataSource = dsSpecialties;
        }
        else
        {
            grdSpecialties.DataSource = this.DataList = null;
        }
        grdSpecialties.DataBind();
    }

    /// <summary>
    /// Insert the Reg Speciality based on the MMIS
    /// </summary>
    /// <param name="MMIS_Code">MMIS Code</param>
    private void InsertRegSpeciality(string MMIS_Code)
    {
        int regID = this.WorkflowPage.RegistrationId;
        RegistrationController.InsertRegSpecialityByMMIS(regID, MMIS_Code, Methods.GetCurrentUserId().ToString());
    }

    private DataSet GetAllCMCSpecialties()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        //parms.Add("PrimaryFlag", "0");
        dsSpecialties = svc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTY", parms);

        return dsSpecialties;
    }
    private void LoadCMCQualifyingEnrollmentCount()
    {
        divCMCEnrollmentCount.Visible = true;
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet dsCMCEnrollmentCount = svc.SelectRegistrationDataWithParams("usp_SelectCMCQualifyingEnrollmentCount", parms);
        if (Helper.HasRows(dsCMCEnrollmentCount))
        {
            lblDisplayCMCEnrollmentCount.Text = dsCMCEnrollmentCount.Tables[0].Rows[0]["Qualifying_Enrollment_Count"].ToString();
        }
        else
        {
            lblDisplayCMCEnrollmentCount.Text = null;
        }
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