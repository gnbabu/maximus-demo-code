using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

// Page doesn't exist anymore

public partial class PopupControls_OfficeHoursIndividual : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

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
    #region dt
    private DataTable dt;
    private void SetDt()
    {
        if (dt == null)
        {
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
            dt = Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public override bool SaveData()
    {
        Page.Validate("valOfficeHoursIndividual");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valOfficeHoursIndividual") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }


        if (string.IsNullOrEmpty(hidID.Text))
        {
            Set_hidID(null);
        }



        var SpecializedTrainingcollection = RadSpecializedTraining.CheckedItems;
        var LanguagesSpokencollection = RadLanguagesSpoken.CheckedItems;
        var CulturalCompetenceiesCollection = RadCulturalComp.CheckedItems;


        var SpecializedTrainingSelectedValues = string.Empty;
        var LanguageSpokenSelectedValues = string.Empty;
        var CulturalCompetenicesSelectedValues = string.Empty;
        var TranslationServicesValues = string.Empty;
        foreach (ListItem item in chkTranslationServiceType.Items)
        {

            if (item.Selected)
            {
                TranslationServicesValues = TranslationServicesValues + item.Value + ",";
            }
            //TranslationServicesValues = TranslationServicesValues.Remove(TranslationServicesValues.LastIndexOf(","));
        }
        if (SpecializedTrainingcollection.Count != 0)
        {

            foreach (var item in SpecializedTrainingcollection)
            {
                SpecializedTrainingSelectedValues = SpecializedTrainingSelectedValues + item.Value + ",";

            }

            SpecializedTrainingSelectedValues = SpecializedTrainingSelectedValues.Remove(SpecializedTrainingSelectedValues.LastIndexOf(","));

        }
        if (LanguagesSpokencollection.Count != 0)
        {

            foreach (var item in LanguagesSpokencollection)
            {
                LanguageSpokenSelectedValues = LanguageSpokenSelectedValues + item.Value + ",";

            }

            LanguageSpokenSelectedValues = LanguageSpokenSelectedValues.Remove(LanguageSpokenSelectedValues.LastIndexOf(","));

        }
        if (CulturalCompetenceiesCollection.Count != 0)
        {

            foreach (var item in CulturalCompetenceiesCollection)
            {
                CulturalCompetenicesSelectedValues = CulturalCompetenicesSelectedValues + item.Value + ",";

            }

            CulturalCompetenicesSelectedValues = CulturalCompetenicesSelectedValues.Remove(CulturalCompetenicesSelectedValues.LastIndexOf(","));

        }




        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        //parms.Add("MAILTO_ADDRESS_NAME", prov_Name.Text);
        parms.Add("CULTURAL_COMPETENCY", CulturalCompetenicesSelectedValues);
        parms.Add("STAFF_LANGUAGES_SPOKEN", LanguageSpokenSelectedValues);
        parms.Add("SPECIALIZED_TRAINING", SpecializedTrainingSelectedValues);
        parms.Add("IsProviderDirectoryOptout", chkProviderDirectoryOpt.Checked ? "1" : "0");
        parms.Add("NEWPATIENT", ddlAcceptNewPatients.SelectedItem.Value);
        parms.Add("REFFERAL", ddlAcceptpatientsref.SelectedItem.Value);
        //parms.Add("OFFICE_DISABLE", ddlDisable.SelectedItem.Value);
        parms.Add("YOUNGEST_PATIENTS", txtyoungestpatients.Text.Trim());
        parms.Add("OLDEST_PATIENT", txtoldestpatients.Text.Trim());
        parms.Add("GENDER_OF_PATIENTS", ddlGenderofPatients.SelectedValue);
        //parms.Add("GENDER_OF_PATIENTS", ddlGenderofPatients.SelectedValue);
        parms.Add("IS_NEW_BORN", ddlAcceptnewborn.SelectedItem.Value);
        parms.Add("IS_PREGNANT", ddlAcceptPregnanetwomen.SelectedItem.Value);
        parms.Add("TranslationServiceType", TranslationServicesValues);
        if (!(ddlTelehealth.SelectedItem.Value.Equals("")))
            parms.Add("Telehealth", ddlTelehealth.SelectedItem.Value);
        if (!(ddlCHIP.SelectedItem.Value.Equals("")))
            parms.Add("CHIP", ddlCHIP.SelectedItem.Value);
        if (!(ddlNewMedicaid.SelectedItem.Value.Equals("")))
            parms.Add("ACCEPTS_NEW_MEDICAID_PATIENTS", ddlNewMedicaid.SelectedItem.Value);

        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());


        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);

        if (isEdit || !string.IsNullOrEmpty(hidID.Text))
        {
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
            parms.Add("REG_OFFICE_TIMING_ID", hidID.Text);
            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "OFFICE_TIMINGcustom", parms);
        }
        else
        {
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "OFFICE_TIMINGcustom", parms);
        }



        return true;
    }

    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valOfficeHoursIndividual";
        this.Page.Validators.Add(val);
    }

    public override bool HasInputValue()
    {
        return true;
    }

    public override bool ValidateData()
    {
        bool result = true;
        //int numberOfQuestionsAnswered = 0;        
        //numberOfQuestionsAnswered += ddlEbilling.SelectedIndex > -1  ? 1 : 0;

        //numberOfQuestionsAnswered += this.ddlTDD.SelectedIndex > -1  ? 1 : 0;
        //numberOfQuestionsAnswered += this.ddlTel.SelectedIndex > -1  ? 1 : 0;
        //numberOfQuestionsAnswered += this.ddlTrans.SelectedIndex > -1 ? 1 : 0;

        //if (numberOfQuestionsAnswered >= 0 && numberOfQuestionsAnswered < 7)
        //{
        //    AddError("Please answer all of the questions to continue.");
        //    result = false;
        //}

        //if (Registration.IsDMEProvider(this.WorkflowPage.RegistrationId))
        //{
        //    if (txtMon.Text.Trim().Length == 0 ||
        //        txtTue.Text.Trim().Length == 0 ||
        //        txtWed.Text.Trim().Length == 0 ||
        //        txtThu.Text.Trim().Length == 0 ||
        //        txtFri.Text.Trim().Length == 0 ||
        //        txtSat.Text.Trim().Length == 0 ||
        //        txtSun.Text.Trim().Length == 0)
        //    {
        //        AddError("Please complete Office Hours.");
        //        result = false;
        //    }
        //}
        return result;
    }

    public override void LoadData(DataRow dr)
    {
        bool isEdit = false;

        if (dr == null)
            isEdit = false;
        else
            isEdit = true;

        //ParentTable.Rows[0].Cells[2].Style["display"] = Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) ? "block" : "none";
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();


        if (dr != null)
        {
            txtoldestpatients.Text = Helper.GetString("OLDEST_PATIENT", dr);
            txtyoungestpatients.Text = Helper.GetString("YOUNGEST_PATIENTS", dr);

            // txtTotal.Text = Helper.GetString("TOTAL", dr);


            if (!string.IsNullOrEmpty(Helper.GetString("NEWPATIENT", dr)))
                ddlAcceptNewPatients.SelectedValue = Helper.GetBool("NEWPATIENT", dr) ? "1" : "0";
            if (!string.IsNullOrEmpty(Helper.GetString("REFFERAL", dr)))
                ddlAcceptpatientsref.SelectedValue = Helper.GetBool("REFFERAL", dr) ? "1" : "0";
            if (!string.IsNullOrEmpty(Helper.GetString("IS_NEW_BORN", dr)))
                ddlAcceptnewborn.SelectedValue = Helper.GetBool("IS_NEW_BORN", dr) ? "1" : "0";
            if (!string.IsNullOrEmpty(Helper.GetString("IS_PREGNANT", dr)))
                ddlAcceptPregnanetwomen.SelectedValue = Helper.GetBool("IS_PREGNANT", dr) ? "1" : "0";
            
            if (!string.IsNullOrEmpty(Helper.GetString("GENDER_OF_PATIENTS", dr)) && Helper.GetString("GENDER_OF_PATIENTS", dr) != "0")
                ddlGenderofPatients.SelectedValue = Helper.GetString("GENDER_OF_PATIENTS", dr);

            if (!string.IsNullOrEmpty(Helper.GetString("IsProviderDirectoryOptout", dr)))
                chkProviderDirectoryOpt.Checked = Helper.GetBool("IsProviderDirectoryOptout", dr) ? true : false;

            if (!string.IsNullOrEmpty(Helper.GetString("Telehealth", dr)))
                ddlTelehealth.SelectedValue = Helper.GetString("Telehealth", dr);
            if (!string.IsNullOrEmpty(Helper.GetString("CHIP", dr)))
                ddlCHIP.SelectedValue = Helper.GetString("CHIP", dr);
            if (!string.IsNullOrEmpty(Helper.GetString("ACCEPTS_NEW_MEDICAID_PATIENTS", dr)))
                ddlNewMedicaid.SelectedValue = Helper.GetString("ACCEPTS_NEW_MEDICAID_PATIENTS", dr);

        }
        //OHPNM-3487 - this block from OHPNM-1917
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
            }
        }
    }



    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadOfficeHoursIndividual();
    }

    private void LoadOfficeHoursIndividual()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OFFICE_TIMING");
        DataTable dtPrimaryPracticeLocation = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        DataRow dtMultiSelectValues = Helper.HasRows(ds) ? ds.Tables[1].Rows[0] : null;
        this.DataList = dtPrimaryPracticeLocation;
        BindCulturalCompetencies();
        BindLanguagesSpoken();
        BindSpecializedTraining();
        BindGender();
        if (Helper.HasRows(this.DataList))
        {
            this.LoadData(this.DataList.Rows[0]);
            this.LoadMultiSelectDropDowns(dtMultiSelectValues);
        }
        else
        {
            this.LoadData(null);
        }
    }

    private void LoadMultiSelectDropDowns(DataRow dr)
    {
        if (!string.IsNullOrEmpty(Helper.GetString("STAFF_LANGUAGES_SPOKEN", dr)))
        {
            string[] idarray = Helper.GetString("STAFF_LANGUAGES_SPOKEN", dr).Split(',');
            foreach (var id in idarray)
            {

                var comboItem = RadLanguagesSpoken.FindItemByValue(id.ToString().Trim());

                if (comboItem != null)
                {
                    comboItem.Checked = true;
                }
            }
        }

        if (!string.IsNullOrEmpty(Helper.GetString("SPECIALIZED_TRAINING", dr)))
        {

            string[] accomodationIds = Helper.GetString("SPECIALIZED_TRAINING", dr).Split(',');
            foreach (var accomodationId in accomodationIds)
            {

                var comboItem = RadSpecializedTraining.FindItemByValue(accomodationId.ToString().Trim());

                if (comboItem != null)
                {
                    comboItem.Checked = true;
                }
            }

        }

        if (!string.IsNullOrEmpty(Helper.GetString("CULTURAL_COMPETENCY", dr)))
        {
            string[] idarray = Helper.GetString("CULTURAL_COMPETENCY", dr).Split(',');
            foreach (var id in idarray)
            {

                var comboItem = RadCulturalComp.FindItemByValue(id.ToString().Trim());

                if (comboItem != null)
                {
                    comboItem.Checked = true;
                }
            }

        }
        if (!string.IsNullOrEmpty(Helper.GetString("TRANSLATIONSERVICETYPE", dr)))
        {
            string[] idarray = Helper.GetString("TRANSLATIONSERVICETYPE", dr).Split(',');
            foreach (var id in idarray)
            {
                var chkTranslationServiceItem=chkTranslationServiceType.Items.FindByValue(id.ToString().Trim());
                if (chkTranslationServiceItem != null)
                {
                    chkTranslationServiceItem.Selected= true;
                }
            }

        }
    }

    private void BindGender()
    {
        DataSet dsGender = svc.SelectReferenceDataWithoutParam("usp_SelectPROVIDER_GENDER");
        DataTable dtfilteredgender = dsGender.Tables[0].AsEnumerable()
                             .Where(r => r.Field<string>("PROVIDER_GENDER_INITIAL") != "U" &&
                                        r.Field<string>("PROVIDER_GENDER_INITIAL") != "X" &&
                                        r.Field<string>("PROVIDER_GENDER_INITIAL") != "N" )
                             .CopyToDataTable();
        Helper.LoadList(ddlGenderofPatients, dtfilteredgender, "PROVIDER_GENDER_NAME", "PROVIDER_GENDER_ID", true);

    }
    private void BindCulturalCompetencies()
    {
        DataSet dsCulturalCompetencies = svc.SelectReferenceDataWithoutParam("usp_SelectCULTURAL_COMPETENCIES");
        if (dsCulturalCompetencies != null)
        {
            RadCulturalComp.Items.Clear();
            foreach (DataRow dr in dsCulturalCompetencies.Tables[0].Rows)
            {
                RadCulturalComp.Items.Insert(0, new RadComboBoxItem(dr["DSC_Cultural_competencies"].ToString(), dr["Cultural_competencies_ID"].ToString()));
            }
        }
    }
    private void BindLanguagesSpoken()
    {
        DataSet dsLanguagesSpoken = svc.SelectReferenceDataWithoutParam("usp_SelectLANGUAGES_SPOKEN");
        if (dsLanguagesSpoken != null)
        {
            RadLanguagesSpoken.Items.Clear();


            foreach (DataRow dr in dsLanguagesSpoken.Tables[0].Rows)
            {
                RadLanguagesSpoken.Items.Insert(0, new RadComboBoxItem(dr["DESC_Languages_Spoken"].ToString(), dr["Languages_Spoken_ID"].ToString()));
            }
        }
    }
    private void BindSpecializedTraining()
    {
        DataSet dsSpecializedTraining = svc.SelectReferenceDataWithoutParam("usp_SelectSPECIALIZED_TRAINING");
        if (dsSpecializedTraining != null)
        {
            RadSpecializedTraining.Items.Clear();
            foreach (DataRow dr in dsSpecializedTraining.Tables[0].Rows)
            {
                RadSpecializedTraining.Items.Insert(0, new RadComboBoxItem(dr["DSC_Specialized_Training"].ToString(), dr["Specialized_Training_ID"].ToString()));
            }
        }
    }
    private void Set_hidID(DataRow row)
    {
        if (row == null)
        {
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OFFICE_TIMING");
            if (Helper.HasRows(ds))
            {
                hidID.Text = ds.Tables[0].Rows[0]["REG_OFFICE_TIMING_ID"].ToString();
            }
        }
        else
        {
            hidID.Text = row["REG_OFFICE_TIMING_ID"].ToString();
        }
    }

    public override string ValidationGroup
    {
        get { return "valOfficeHoursIndividual"; }
    }

    public override string Title
    {
        get { return "Office Information"; }
    }

    public override string IdText
    {
        get { return "ucOfficeHoursIndividual_" + this.WorkflowPage.RegistrationId; }
    }
}