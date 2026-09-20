using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.ComponentModel;

public partial class PopupControls_OfficeHoursServiceLocation : BaseSectionControl
{
    private string DefaultValidationGroup = "valOfficeHoursIndividual";

    [DefaultValue(0)]
    public int InMaintenance { get; set; }

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
    private bool Seting_ID()
    {
        string status = "false";
        DataSet ds = svc.SelectProviderReturnStatus(this.WorkflowPage.RegistrationId);
        if (Helper.HasRows(ds))
        {
            status = ds.Tables[0].Rows[0]["Return_Status"].ToString();
        }
        if (status == "true")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //bool bolFlag = Seting_ID();       //SAM538/OHPNM-19400 - Allow to add/update primary or other service locations when RTP from Site Visit 
        //if (bolFlag)
        //{            
        //    Helper.SetReadOnly(this, true, "formFieldReadOnly");
        //}        
        chkProviderDirectoryOpt.InputAttributes.Add("aria-label", "Provider Directory Opt-Out");
        chkMon24Hours.InputAttributes.Add("aria-label", "Open 24 Hours");
        chkTue24Hours.InputAttributes.Add("aria-label", "Open 24 Hours");
        chkWed24Hours.InputAttributes.Add("aria-label", "Open 24 Hours");
        chkThu24Hours.InputAttributes.Add("aria-label", "Open 24 Hours");
        chkFri24Hours.InputAttributes.Add("aria-label", "Open 24 Hours");
        chkSat24Hours.InputAttributes.Add("aria-label", "Open 24 Hours");
        chkSun24Hours.InputAttributes.Add("aria-label", "Open 24 Hours");
    }

    // we're a subcomponent, so we don't need this; our saves are controlled by our daddies
    public override bool SaveData()
    {
        return true;
    }

    public bool SaveData(int? addressId)
    {
        // this isn't really necessary but leaving it here since it's consistent with other screens; validation on this screen is currently taken care of by ValidateData(); but if someone adds some rules to the screen this will catch them
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

        var SpecializedTrainingcollection = RadSpecializedTraining.CheckedItems;
        var LanguagesSpokencollection = RadLanguagesSpoken.CheckedItems;
        var CulturalCompetenceiesCollection = RadCulturalComp.CheckedItems;
        var RadADAAccommodationCollection = RadADAAccommodation.CheckedItems;
        var ProviderSpecializedTrainingcollection = RadSpecializedTrainingProv.CheckedItems;
        var ProviderLanguagesSpokencollection = RadLanguagesSpokenProv.CheckedItems;
        var ProviderCulturalCompetenciesCollection = RadCulturalCompProv.CheckedItems;

        var SpecializedTrainingSelectedValues = string.Empty;
        var LanguageSpokenSelectedValues = string.Empty;
        var CulturalCompetenicesSelectedValues = string.Empty;
        var TranslationServicesValues = string.Empty;
        var ADAAccommodationSelectedValues = string.Empty;

        var ProviderSpecializedTrainingSelectedValues = string.Empty;
        var ProviderLanguagesSpokenSelectedValues = string.Empty;
        var ProviderCulturalCompetenciesSelectedValues = string.Empty;

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
        if (RadADAAccommodationCollection.Count != 0)
        {
            foreach (var item in RadADAAccommodationCollection)
            {
                ADAAccommodationSelectedValues = ADAAccommodationSelectedValues + item.Value + ",";
            }

            ADAAccommodationSelectedValues = ADAAccommodationSelectedValues.Remove(ADAAccommodationSelectedValues.LastIndexOf(","));
        }
        // add provider fields
        if (ProviderCulturalCompetenciesCollection.Count != 0)
        {
            foreach (var item in ProviderCulturalCompetenciesCollection)
            {
                ProviderCulturalCompetenciesSelectedValues = ProviderCulturalCompetenciesSelectedValues + item.Value + ",";
            }
            ProviderCulturalCompetenciesSelectedValues = ProviderCulturalCompetenciesSelectedValues.Remove(ProviderCulturalCompetenciesSelectedValues.LastIndexOf(","));
        }
        if (ProviderSpecializedTrainingcollection.Count != 0)
        {
            foreach (var item in ProviderSpecializedTrainingcollection)
            {
                ProviderSpecializedTrainingSelectedValues = ProviderSpecializedTrainingSelectedValues + item.Value + ",";
            }
            ProviderSpecializedTrainingSelectedValues = ProviderSpecializedTrainingSelectedValues.Remove(ProviderSpecializedTrainingSelectedValues.LastIndexOf(","));
        }
        if (ProviderLanguagesSpokencollection.Count != 0)
        {
            foreach (var item in ProviderLanguagesSpokencollection)
            {
                ProviderLanguagesSpokenSelectedValues = ProviderLanguagesSpokenSelectedValues + item.Value + ",";
            }
            ProviderLanguagesSpokenSelectedValues = ProviderLanguagesSpokenSelectedValues.Remove(ProviderLanguagesSpokenSelectedValues.LastIndexOf(","));
        }

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("IsProviderDirectoryOptout", chkProviderDirectoryOpt.Checked ? "1" : "0");
        parms.Add("MON_START_TIME", ddlMonStartTime.Text);
        parms.Add("MON_END_TIME", ddlMonEndTime.Text);
        parms.Add("MON_OPEN_24_HRS", chkMon24Hours.Checked ? "1" : "0");
        parms.Add("TUE_START_TIME", ddlTueStartTime.Text);
        parms.Add("TUE_END_TIME", ddlTueEndTime.Text);
        parms.Add("TUE_OPEN_24_HRS", chkTue24Hours.Checked ? "1" : "0");
        parms.Add("WED_START_TIME", ddlWedStartTime.Text);
        parms.Add("WED_END_TIME", ddlWedEndTime.Text);
        parms.Add("WED_OPEN_24_HRS", chkWed24Hours.Checked ? "1" : "0");
        parms.Add("THU_START_TIME", ddlThuStartTime.Text);
        parms.Add("THU_END_TIME", ddlThuEndTime.Text);
        parms.Add("THU_OPEN_24_HRS", chkThu24Hours.Checked ? "1" : "0");
        parms.Add("FRI_START_TIME", ddlFriStartTime.Text);
        parms.Add("FRI_END_TIME", ddlFriEndTime.Text);
        parms.Add("FRI_OPEN_24_HRS", chkFri24Hours.Checked ? "1" : "0");
        parms.Add("SAT_START_TIME", ddlSatStartTime.Text);
        parms.Add("SAT_END_TIME", ddlSatEndTime.Text);
        parms.Add("SAT_OPEN_24_HRS", chkSat24Hours.Checked ? "1" : "0");
        parms.Add("SUN_START_TIME", ddlSunStartTime.Text);
        parms.Add("SUN_END_TIME", ddlSunEndTime.Text);
        parms.Add("SUN_OPEN_24_HRS", chkSun24Hours.Checked ? "1" : "0");
        parms.Add("WEBSITE", txtWebsite.Text);
        parms.Add("TELEPHONE", ddlTel.SelectedItem.Value);
        parms.Add("TRANSPORT", ddlTrans.SelectedItem.Value);
        parms.Add("EBILLING", ddlEbilling.SelectedItem.Value);
        parms.Add("TDD", ddlTDD.SelectedItem.Value);
        parms.Add("OFFICE_ASLOFFERED", ddlASL.SelectedValue);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("REG_ADDRESS_ID", addressId.ToString());
        parms.Add("CULTURAL_COMPETENCY", CulturalCompetenicesSelectedValues);
        parms.Add("STAFF_LANGUAGES_SPOKEN", LanguageSpokenSelectedValues);
        parms.Add("SPECIALIZED_TRAINING", SpecializedTrainingSelectedValues);
        parms.Add("ADA_ACCOMMODATIONS", ADAAccommodationSelectedValues);

        // add the 3 provider fields
        parms.Add("PROVIDER_LANGUAGES_SPOKEN", ProviderLanguagesSpokenSelectedValues);
        parms.Add("PROVIDER_SPECIALIZED_TRAINING", ProviderSpecializedTrainingSelectedValues);
        parms.Add("PROVIDER_CULTURAL_COMPETENCY", ProviderCulturalCompetenciesSelectedValues);

        parms.Add("NEWPATIENT", ddlAcceptNewPatients.SelectedItem.Value);
        parms.Add("REFFERAL", ddlAcceptpatientsref.SelectedItem.Value);
        parms.Add("YOUNGEST_PATIENTS", txtyoungestpatients.Text.Trim());
        parms.Add("OLDEST_PATIENT", txtoldestpatients.Text.Trim());
        parms.Add("GENDER_OF_PATIENTS", ddlGenderofPatients.SelectedValue);
        parms.Add("IS_NEW_BORN", ddlAcceptnewborn.SelectedItem.Value);
        parms.Add("IS_PREGNANT", ddlAcceptPregnanetwomen.SelectedItem.Value);
        parms.Add("TranslationServiceType", TranslationServicesValues);
        parms.Add("Telehealth", ddlTelehealth.SelectedItem.Value);
        parms.Add("CHIP", ddlCHIP.SelectedItem.Value);
        parms.Add("ACCEPTS_NEW_MEDICAID_PATIENTS", ddlNewMedicaid.SelectedItem.Value);

        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);

        if (isEdit || !string.IsNullOrEmpty(hidID.Text))
        {
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
            parms.Add("REG_OFFICE_TIMING_ID", hidID.Text);
            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "OFFICE_TIMINGcustom", parms);
        }
        else
        {
            // should we protect this with a HasInputValue()?
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            // note, most of our code uses a "NoChange" here in similar situations; that makes no sense to me; making this one a "Inserted"
            svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "OFFICE_TIMINGcustom", parms);
        }

        return true;
    }

    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = ValidationGroup;
        this.Page.Validators.Add(val);
    }

    // added to help move the error message higher up so they see it; just add the highest ValidationGroup in the calling page to this attribute: SubControlValidationGroup="SatellitePracticeLocations" 
    // or leave that off the calling page if you want to use the validation group nearest the data on the screen
    public string SubControlValidationGroup { get; set; }

    public override string ValidationGroup
    {
        get
        {
            if (SubControlValidationGroup == null)
            {
                return DefaultValidationGroup;
            }
            else
            {
                return SubControlValidationGroup;
            }
        }
    }

    public override bool HasInputValue()
    {
        var SpecializedTrainingcollection = RadSpecializedTraining.CheckedItems;
        var LanguagesSpokencollection = RadLanguagesSpoken.CheckedItems;
        var CulturalCompetenceiesCollection = RadCulturalComp.CheckedItems;
        var RadADAAccommodationCollecton = RadADAAccommodation.CheckedItems;
        var ProviderLanguagesSpokencollection = RadLanguagesSpokenProv.CheckedItems;
        var ProviderSpecializedTrainingcollection = RadSpecializedTrainingProv.CheckedItems;
        var ProviderCulturalCompetenciesCollection = RadCulturalCompProv.CheckedItems;

        return (ddlMonStartTime.Text.Trim().Length > 0 ||
                ddlTueStartTime.Text.Trim().Length > 0 ||
                ddlWedStartTime.Text.Trim().Length > 0 ||
                ddlThuStartTime.Text.Trim().Length > 0 ||
                ddlFriStartTime.Text.Trim().Length > 0 ||
                ddlSatStartTime.Text.Trim().Length > 0 ||
                ddlSunStartTime.Text.Trim().Length > 0 ||
                ddlMonEndTime.Text.Trim().Length > 0 ||
                ddlTueEndTime.Text.Trim().Length > 0 ||
                ddlWedEndTime.Text.Trim().Length > 0 ||
                ddlThuEndTime.Text.Trim().Length > 0 ||
                ddlFriEndTime.Text.Trim().Length > 0 ||
                ddlSatEndTime.Text.Trim().Length > 0 ||
                ddlSunEndTime.Text.Trim().Length > 0 ||
                chkMon24Hours.Checked ||
                chkTue24Hours.Checked ||
                chkWed24Hours.Checked ||
                chkThu24Hours.Checked ||
                chkFri24Hours.Checked ||
                chkSat24Hours.Checked ||
                chkSun24Hours.Checked ||
                txtWebsite.Text.Trim().Length > 0 ||
                ddlTel.SelectedIndex > -1 ||
                ddlTrans.SelectedIndex > -1 || 
                ddlEbilling.SelectedIndex > -1 ||
                ddlTDD.SelectedIndex > -1 ||
                ddlASL.SelectedIndex > -1 ||
                chkTranslationServiceType.SelectedIndex > -1 ||
                SpecializedTrainingcollection.Count != 0 ||
                LanguagesSpokencollection.Count != 0 ||
                ProviderLanguagesSpokencollection.Count != 0 ||
                ProviderSpecializedTrainingcollection.Count != 0 ||
                ProviderCulturalCompetenciesCollection.Count != 0 ||
                CulturalCompetenceiesCollection.Count != 0 ||
                RadADAAccommodationCollecton.Count != 0 ||
                ddlAcceptNewPatients.SelectedIndex > -1 ||
                ddlAcceptpatientsref.SelectedIndex > -1 ||
                ddlGenderofPatients.SelectedIndex > -1 ||
                ddlAcceptnewborn.SelectedIndex > -1 ||
                ddlAcceptPregnanetwomen.SelectedIndex > -1 ||
                txtyoungestpatients.Text.Trim().Length > 0 ||
                txtoldestpatients.Text.Trim().Length > 0
                );
    }

    public override bool ValidateData()
    {
        bool result = true;

        if (ddlMonStartTime.SelectedIndex >= ddlMonEndTime.SelectedIndex && ddlMonStartTime.SelectedIndex != 0)
        {
            AddError("On Monday, please make sure your start time is before your end time.");
            result = false;
        }
        if (ddlTueStartTime.SelectedIndex >= ddlTueEndTime.SelectedIndex && ddlTueStartTime.SelectedIndex != 0)
        {
            AddError("On Tuesday, please make sure your start time is before your end time.");
            result = false;
        }
        if (ddlWedStartTime.SelectedIndex >= ddlWedEndTime.SelectedIndex && ddlWedStartTime.SelectedIndex != 0)
        {
            AddError("On Wednesday, please make sure your start time is before your end time.");
            result = false;
        }
        if (ddlThuStartTime.SelectedIndex >= ddlThuEndTime.SelectedIndex && ddlThuStartTime.SelectedIndex != 0)
        {
            AddError("On Thursday, please make sure your start time is before your end time.");
            result = false;
        }
        if (ddlFriStartTime.SelectedIndex >= ddlFriEndTime.SelectedIndex && ddlFriStartTime.SelectedIndex != 0)
        {
            AddError("On Friday, please make sure your start time is before your end time.");
            result = false;
        }
        if (ddlSatStartTime.SelectedIndex >= ddlSatEndTime.SelectedIndex && ddlSatStartTime.SelectedIndex != 0)
        {
            AddError("On Saturday, please make sure your start time is before your end time.");
            result = false;
        }
        if (ddlSunStartTime.SelectedIndex >= ddlSunEndTime.SelectedIndex && ddlSunStartTime.SelectedIndex != 0)
        {
            AddError("On Sunday, please make sure your start time is before your end time.");
            result = false;
        }

        return result;
    }

    public override void LoadData(DataRow dr)
    {
        this.resetFields();

        if (dr == null)
        {
            // no data found; assume it's a new REG_OFFICE_TIMING being entered
            hidIsEdit.Text = "false";
            hidID.Text = null;
        }
        else
        {
            // data found; set variable saying we are editing and save off the primary key
            hidIsEdit.Text = "true";
            if (!string.IsNullOrEmpty(Helper.GetString("REG_OFFICE_TIMING_ID", dr)))
            {
                hidID.Text = Helper.GetString("REG_OFFICE_TIMING_ID", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("IsProviderDirectoryOptout", dr)))
            {
                chkProviderDirectoryOpt.Checked = Helper.GetBool("IsProviderDirectoryOptout", dr) ? true : false;
            }

            // and go apply values to the user controls
            if (!string.IsNullOrEmpty(Helper.GetString("MON_START_TIME", dr)))
            {
                ddlMonStartTime.SelectedValue = Helper.GetString("MON_START_TIME", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("MON_END_TIME", dr)))
            {
                ddlMonEndTime.SelectedValue = Helper.GetString("MON_END_TIME", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("MON_OPEN_24_HRS", dr)))
            {
                chkMon24Hours.Checked = Helper.GetBool("MON_OPEN_24_HRS", dr) ? true : false;
            }

            if (!string.IsNullOrEmpty(Helper.GetString("TUE_START_TIME", dr)))
            {
                ddlTueStartTime.SelectedValue = Helper.GetString("TUE_START_TIME", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("TUE_END_TIME", dr)))
            {
                ddlTueEndTime.SelectedValue = Helper.GetString("TUE_END_TIME", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("TUE_OPEN_24_HRS", dr)))
            {
                chkTue24Hours.Checked = Helper.GetBool("TUE_OPEN_24_HRS", dr) ? true : false;
            }

            if (!string.IsNullOrEmpty(Helper.GetString("WED_START_TIME", dr)))
            {
                ddlWedStartTime.SelectedValue = Helper.GetString("WED_START_TIME", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("WED_END_TIME", dr)))
            {
                ddlWedEndTime.SelectedValue = Helper.GetString("WED_END_TIME", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("WED_OPEN_24_HRS", dr)))
            {
                chkWed24Hours.Checked = Helper.GetBool("WED_OPEN_24_HRS", dr) ? true : false;
            }

            if (!string.IsNullOrEmpty(Helper.GetString("THU_START_TIME", dr)))
            {
                ddlThuStartTime.SelectedValue = Helper.GetString("THU_START_TIME", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("THU_END_TIME", dr)))
            {
                ddlThuEndTime.SelectedValue = Helper.GetString("THU_END_TIME", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("THU_OPEN_24_HRS", dr)))
            {
                chkThu24Hours.Checked = Helper.GetBool("THU_OPEN_24_HRS", dr) ? true : false;
            }

            if (!string.IsNullOrEmpty(Helper.GetString("FRI_START_TIME", dr)))
            {
                ddlFriStartTime.SelectedValue = Helper.GetString("FRI_START_TIME", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("FRI_END_TIME", dr)))
            {
                ddlFriEndTime.SelectedValue = Helper.GetString("FRI_END_TIME", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("FRI_OPEN_24_HRS", dr)))
            {
                chkFri24Hours.Checked = Helper.GetBool("FRI_OPEN_24_HRS", dr) ? true : false;
            }

            if (!string.IsNullOrEmpty(Helper.GetString("SAT_START_TIME", dr)))
            {
                ddlSatStartTime.SelectedValue = Helper.GetString("SAT_START_TIME", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("SAT_END_TIME", dr)))
            {
                ddlSatEndTime.SelectedValue = Helper.GetString("SAT_END_TIME", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("SAT_OPEN_24_HRS", dr)))
            {
                chkSat24Hours.Checked = Helper.GetBool("SAT_OPEN_24_HRS", dr) ? true : false;
            }

            if (!string.IsNullOrEmpty(Helper.GetString("SUN_START_TIME", dr)))
            {
                ddlSunStartTime.SelectedValue = Helper.GetString("SUN_START_TIME", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("SUN_END_TIME", dr)))
            {
                ddlSunEndTime.SelectedValue = Helper.GetString("SUN_END_TIME", dr);
            }

            if (!string.IsNullOrEmpty(Helper.GetString("SUN_OPEN_24_HRS", dr)))
            {
                chkSun24Hours.Checked = Helper.GetBool("SUN_OPEN_24_HRS", dr) ? true : false;
            }
           
            if (!string.IsNullOrEmpty(Helper.GetString("TELEPHONE", dr)))
            {
                ddlTel.SelectedValue = Helper.GetBool("TELEPHONE", dr) ? "1" : "0";
            }

            if (!string.IsNullOrEmpty(Helper.GetString("TRANSPORT", dr)))
            {
                ddlTrans.SelectedValue = Helper.GetBool("TRANSPORT", dr) ? "1" : "0";
            }

            if (!string.IsNullOrEmpty(Helper.GetString("EBILLING", dr)))
            {
                ddlEbilling.SelectedValue = Helper.GetBool("EBILLING", dr) ? "1" : "0";
            }

            if (!string.IsNullOrEmpty(Helper.GetString("TDD", dr)))
            {
                ddlTDD.SelectedValue = Helper.GetBool("TDD", dr) ? "1" : "0";
            }

            if (!string.IsNullOrEmpty(Helper.GetString("OFFICE_ASLOFFERED", dr)))
            {
                ddlASL.SelectedValue = Helper.GetBool("OFFICE_ASLOFFERED", dr) ? "1" : "0";
            }

            if (!string.IsNullOrEmpty(Helper.GetString("NEWPATIENT", dr)))
            {
                ddlAcceptNewPatients.SelectedValue = Helper.GetBool("NEWPATIENT", dr) ? "1" : "0";
            }

            if (!string.IsNullOrEmpty(Helper.GetString("REFFERAL", dr)))
            {
                ddlAcceptpatientsref.SelectedValue = Helper.GetBool("REFFERAL", dr) ? "1" : "0";
            }

            if (!string.IsNullOrEmpty(Helper.GetString("IS_NEW_BORN", dr)))
            {
                ddlAcceptnewborn.SelectedValue = Helper.GetBool("IS_NEW_BORN", dr) ? "1" : "0";
            }

            if (!string.IsNullOrEmpty(Helper.GetString("IS_PREGNANT", dr)))
            {
                ddlAcceptPregnanetwomen.SelectedValue = Helper.GetBool("IS_PREGNANT", dr) ? "1" : "0";
            }

            if (!string.IsNullOrEmpty(Helper.GetString("GENDER_OF_PATIENTS", dr)) && Helper.GetString("GENDER_OF_PATIENTS", dr) != "0")
            {
                ddlGenderofPatients.SelectedValue = Helper.GetString("GENDER_OF_PATIENTS", dr);
            }

            txtoldestpatients.Text = Helper.GetString("OLDEST_PATIENT", dr);
            txtyoungestpatients.Text = Helper.GetString("YOUNGEST_PATIENTS", dr);
            txtWebsite.Text = Helper.GetString("WEBSITE", dr);

            if (!string.IsNullOrEmpty(Helper.GetString("Telehealth", dr)))
            {
                string ddlValue = Helper.GetString("Telehealth", dr);
                if (ddlValue.Equals("True")) {
                    ddlTelehealth.Items.Remove(ddlTelehealth.Items.FindByValue(""));
                    ddlTelehealth.SelectedValue = "1";
                }
                else if (ddlValue.Equals("False")) {
                    ddlTelehealth.Items.Remove(ddlTelehealth.Items.FindByValue(""));
                    ddlTelehealth.SelectedValue = "0";
                }
                else
                {
                    ddlTelehealth.SelectedValue = "";
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetString("CHIP", dr)))
            {
                if (!string.IsNullOrEmpty(Helper.GetString("CHIP", dr)))
                {
                    string ddlValue = Helper.GetString("CHIP", dr);
                    if (ddlValue.Equals("True"))
                    {
                        ddlCHIP.Items.Remove(ddlCHIP.Items.FindByValue(""));
                        ddlCHIP.SelectedValue = "1";
                    }
                    else if (ddlValue.Equals("False"))
                    {
                        ddlCHIP.Items.Remove(ddlCHIP.Items.FindByValue(""));
                        ddlCHIP.SelectedValue = "0";
                    }
                    else
                    {
                        ddlCHIP.SelectedValue = "";
                    }
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetString("ACCEPTS_NEW_MEDICAID_PATIENTS", dr)))
            {
                if (!string.IsNullOrEmpty(Helper.GetString("ACCEPTS_NEW_MEDICAID_PATIENTS", dr)))
                {
                    string ddlValue = Helper.GetString("ACCEPTS_NEW_MEDICAID_PATIENTS", dr);
                    if (ddlValue.Equals("True"))
                    {
                        ddlNewMedicaid.Items.Remove(ddlNewMedicaid.Items.FindByValue(""));
                        ddlNewMedicaid.SelectedValue = "1";
                    }
                    else if (ddlValue.Equals("False"))
                    {
                        ddlNewMedicaid.Items.Remove(ddlNewMedicaid.Items.FindByValue(""));
                        ddlNewMedicaid.SelectedValue = "0";
                    }
                    else
                    {
                        ddlNewMedicaid.SelectedValue = "";
                    }
                }
            }
        }

        //OHPNM-3487 - this block from OHPNM-1917
        bool isInMaintenance = string.IsNullOrEmpty(hidIsInMaintenance.Text) ? false : Convert.ToBoolean(hidIsInMaintenance.Text);
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]) || isInMaintenance)
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
            }
        }
    }


    public void resetFields() {
        // clear out values
        ddlMonStartTime.SelectedValue = "";
        ddlMonEndTime.SelectedValue = "";
        ddlTueStartTime.SelectedValue = "";
        ddlTueEndTime.SelectedValue = "";
        ddlWedStartTime.SelectedValue = "";
        ddlWedEndTime.SelectedValue = "";
        ddlThuStartTime.SelectedValue = "";
        ddlThuEndTime.SelectedValue = "";
        ddlFriStartTime.SelectedValue = "";
        ddlFriEndTime.SelectedValue = "";
        ddlSatStartTime.SelectedValue = "";
        ddlSatEndTime.SelectedValue = "";
        ddlSunStartTime.SelectedValue = "";
        ddlSunEndTime.SelectedValue = "";

        ddlEbilling.ClearSelection();
        ddlTel.ClearSelection();
        ddlTrans.ClearSelection();
        ddlTDD.ClearSelection();
        ddlASL.ClearSelection();
        ddlAcceptNewPatients.ClearSelection();
        ddlAcceptpatientsref.ClearSelection();
        ddlGenderofPatients.ClearSelection();
        ddlAcceptnewborn.ClearSelection();
        ddlAcceptPregnanetwomen.ClearSelection();

        chkMon24Hours.Checked = false;
        chkTue24Hours.Checked = false;
        chkWed24Hours.Checked = false;
        chkThu24Hours.Checked = false;
        chkFri24Hours.Checked = false;
        chkSat24Hours.Checked = false;
        chkSun24Hours.Checked = false;

        chkProviderDirectoryOpt.Checked = false;

        RadSpecializedTraining.ClearCheckedItems();
        RadSpecializedTrainingProv.ClearCheckedItems();
        RadLanguagesSpoken.ClearCheckedItems();
        RadLanguagesSpokenProv.ClearCheckedItems();
        RadCulturalComp.ClearCheckedItems();
        RadCulturalCompProv.ClearCheckedItems();
        RadADAAccommodation.ClearCheckedItems();
        chkTranslationServiceType.ClearSelection();

        txtWebsite.Text = "";
        txtyoungestpatients.Text = "";
        txtoldestpatients.Text = "";
    }


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
    }

    private void initializeDropDowns()
    {
        BindOfficeHours();
        BindCulturalCompetencies(RadCulturalComp);
        BindCulturalCompetencies(RadCulturalCompProv);
        BindADAAccommodations();
        BindLanguagesSpoken(RadLanguagesSpoken);
        BindLanguagesSpoken(RadLanguagesSpokenProv);
        BindSpecializedTraining(RadSpecializedTraining);
        BindSpecializedTraining(RadSpecializedTrainingProv);
        BindGender();
    }

    public void LoadOfficeInformation(int regId, int regAddressId, int inMaintenance) {

        this.initializeDropDowns();

        if (inMaintenance == 0)
        {
            hidIsInMaintenance.Text = "false";
        }
        else if (inMaintenance == 1)
        {
            hidIsInMaintenance.Text = "true";
        }

        if (regAddressId == -1) {
			this.LoadData(null);
		}
		else {
			PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectOfficeInformationData(regId, regAddressId, "OfficeInformationData");
            DataTable officeInformationForLocation = Helper.HasRows(ds) ? ds.Tables[0] : null;
            DataRow dtMultiSelectValues = Helper.HasRows(ds) ? ds.Tables[1].Rows[0] : null;
            this.DataList = officeInformationForLocation;     
			
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
    }

    private void LoadMultiSelectDropDowns(DataRow dr)
    {
        // first do the providers...
        if (!string.IsNullOrEmpty(Helper.GetString("PROVIDER_LANGUAGES_SPOKEN", dr)))
        {
            string[] idarray = Helper.GetString("PROVIDER_LANGUAGES_SPOKEN", dr).Split(',');

            var LanguagesSpokencollection = RadLanguagesSpokenProv.Items;
            foreach (var id in idarray)
            {
                var comboItem = RadLanguagesSpokenProv.FindItemByValue(id.ToString().Trim());

                if (comboItem != null)
                {
                    comboItem.Checked = true;
                }
            }
        }

        if (!string.IsNullOrEmpty(Helper.GetString("PROVIDER_SPECIALIZED_TRAINING", dr)))
        {
            string[] idarray = Helper.GetString("PROVIDER_SPECIALIZED_TRAINING", dr).Split(',');
            foreach (var id in idarray)
            {
                var comboItem = RadSpecializedTrainingProv.FindItemByValue(id.ToString().Trim());

                if (comboItem != null)
                {
                    comboItem.Checked = true;
                }
            }

        }

        if (!string.IsNullOrEmpty(Helper.GetString("PROVIDER_CULTURAL_COMPETENCY", dr)))
        {
            string[] idarray = Helper.GetString("PROVIDER_CULTURAL_COMPETENCY", dr).Split(',');
            foreach (var id in idarray)
            {

                var comboItem = RadCulturalCompProv.FindItemByValue(id.ToString().Trim());

                if (comboItem != null)
                {
                    comboItem.Checked = true;
                }
            }

        }

        // now to the specific location in questions
        if (!string.IsNullOrEmpty(Helper.GetString("STAFF_LANGUAGES_SPOKEN", dr)))
        {
            string[] idarray = Helper.GetString("STAFF_LANGUAGES_SPOKEN", dr).Split(',');

            var LanguagesSpokencollection = RadLanguagesSpoken.Items;
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

        if (!string.IsNullOrEmpty(Helper.GetString("ADA_ACCOMODATIONS", dr)))
        {
            string[] idarray = Helper.GetString("ADA_ACCOMODATIONS", dr).Split(',');
            foreach (var id in idarray)
            {

                var comboItem = RadADAAccommodation.FindItemByValue(id.ToString().Trim());

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
                                        r.Field<string>("PROVIDER_GENDER_INITIAL") != "N")
                             .CopyToDataTable();
        Helper.LoadList(ddlGenderofPatients, dtfilteredgender, "PROVIDER_GENDER_NAME", "PROVIDER_GENDER_ID", true);

    }
    protected void BindADAAccommodations()
    {
        DataSet dsSpecializedTraining = svc.SelectReferenceDataWithoutParam("usp_SelectOFFICE_ACCOMMODATIONS");
        if (dsSpecializedTraining != null)
        {
            RadADAAccommodation.Items.Clear();
            foreach (DataRow dr in dsSpecializedTraining.Tables[0].Rows)
            {
                RadADAAccommodation.Items.Insert(0, new RadComboBoxItem(dr["DSC_Office_Accommodations"].ToString(), dr["Office_Accommodations_ID"].ToString()));
            }
        }

    }
    private void BindCulturalCompetencies(RadComboBox culturalComboBox)
    {
        DataSet dsCulturalCompetencies = svc.SelectReferenceDataWithoutParam("usp_SelectCULTURAL_COMPETENCIES");
        if (dsCulturalCompetencies != null)
        {
            culturalComboBox.Items.Clear();
            foreach (DataRow dr in dsCulturalCompetencies.Tables[0].Rows)
            {
                culturalComboBox.Items.Insert(0, new RadComboBoxItem(dr["DSC_Cultural_competencies"].ToString(), dr["Cultural_competencies_ID"].ToString()));
            }
        }
    }

    private void BindLanguagesSpoken(RadComboBox languageComboBox)
    {
        DataSet dsLanguagesSpoken = svc.SelectReferenceDataWithoutParam("usp_SelectLANGUAGES_SPOKEN");
        if (dsLanguagesSpoken != null)
        {
            languageComboBox.Items.Clear();
            foreach (DataRow dr in dsLanguagesSpoken.Tables[0].Rows)
            {
                languageComboBox.Items.Insert(0, new RadComboBoxItem(dr["DESC_Languages_Spoken"].ToString(), dr["Languages_Spoken_ID"].ToString()));
            }

        }
    }

    private void BindSpecializedTraining(RadComboBox trainingComboBox)
    {
        DataSet dsSpecializedTraining = svc.SelectReferenceDataWithoutParam("usp_SelectSPECIALIZED_TRAINING");
        if (dsSpecializedTraining != null)
        {
            trainingComboBox.Items.Clear();
            foreach (DataRow dr in dsSpecializedTraining.Tables[0].Rows)
            {
                trainingComboBox.Items.Insert(0, new RadComboBoxItem(dr["DSC_Specialized_Training"].ToString(), dr["Specialized_Training_ID"].ToString()));
            }
        }
    }

    public override string Title
    {
        get { return "Office Information"; }
    }

    public override string IdText
    {
        get { return "ucOfficeHoursIndividual_" + this.WorkflowPage.RegistrationId; }
    }

    // setup the time interval drop-downs
    private void BindOfficeHours()
    {
        ddlMonStartTime.DataSource = GetTimeIntervals();
        ddlMonStartTime.DataBind();

        ddlMonEndTime.DataSource = GetTimeIntervals();
        ddlMonEndTime.DataBind();

        ddlTueStartTime.DataSource = GetTimeIntervals();
        ddlTueStartTime.DataBind();

        ddlTueEndTime.DataSource = GetTimeIntervals();
        ddlTueEndTime.DataBind();

        ddlWedStartTime.DataSource = GetTimeIntervals();
        ddlWedStartTime.DataBind();

        ddlWedEndTime.DataSource = GetTimeIntervals();
        ddlWedEndTime.DataBind();

        ddlThuStartTime.DataSource = GetTimeIntervals();
        ddlThuStartTime.DataBind();

        ddlThuEndTime.DataSource = GetTimeIntervals();
        ddlThuEndTime.DataBind();

        ddlFriStartTime.DataSource = GetTimeIntervals();
        ddlFriStartTime.DataBind();

        ddlFriEndTime.DataSource = GetTimeIntervals();
        ddlFriEndTime.DataBind();

        ddlSatStartTime.DataSource = GetTimeIntervals();
        ddlSatStartTime.DataBind();

        ddlSatEndTime.DataSource = GetTimeIntervals();
        ddlSatEndTime.DataBind();

        ddlSunStartTime.DataSource = GetTimeIntervals();
        ddlSunStartTime.DataBind();

        ddlSunEndTime.DataSource = GetTimeIntervals();
        ddlSunEndTime.DataBind();
    }

    public List<string> GetTimeIntervals()
    {
        List<string> timeIntervals = new List<string>();
        timeIntervals.Add(""); // add a blank one at the top
        DateTime date = DateTime.MinValue.AddHours(0);
        DateTime endDate = DateTime.MinValue.AddDays(1);

        while (date < endDate)
        {
            timeIntervals.Add(date.ToShortTimeString());
            date = date.AddMinutes(30);
        }

        return timeIntervals;
    }
}