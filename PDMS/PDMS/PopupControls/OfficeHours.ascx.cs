using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

// Page doesn't exist anymore

public partial class PopupControls_OfficeHours : BaseSectionControl
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
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OFFICE_TIMING");
            dt = Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
            }
        }
    }

    public override bool SaveData()
    {
        Page.Validate("valOfficeHours");
       

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valOfficeHours") && !v.IsValid)
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

      
        
        var collection = RadADAAccommodation.CheckedItems;

        var LanguagesSpokencollection = RadLanguagesSpoken.CheckedItems;
        var AdaAccomodationsselectedValues = string.Empty;
        if (collection.Count != 0)
        {
           
            foreach (var item in collection)
            {
                AdaAccomodationsselectedValues = AdaAccomodationsselectedValues+item.Value + "," ;                

            }
            
            AdaAccomodationsselectedValues = AdaAccomodationsselectedValues.Remove(AdaAccomodationsselectedValues.LastIndexOf(","));

        }
        var LanguageSpokenSelectedValues = string.Empty;
        if (LanguagesSpokencollection.Count != 0)
        {

            foreach (var item in LanguagesSpokencollection)
            {
                LanguageSpokenSelectedValues = LanguageSpokenSelectedValues + item.Value + ",";

            }

            LanguageSpokenSelectedValues = LanguageSpokenSelectedValues.Remove(LanguageSpokenSelectedValues.LastIndexOf(","));

        }
        var TranslationServicesValues = string.Empty;
        foreach (ListItem item in chkTranslationServiceType.Items)
        {

            if (item.Selected)
            {
                TranslationServicesValues = TranslationServicesValues+item.Value+",";
            }
            
        }
        TranslationServicesValues = TranslationServicesValues!=null?"": TranslationServicesValues.Remove(TranslationServicesValues.LastIndexOf(","));

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        //parms.Add("MAILTO_ADDRESS_NAME", prov_Name.Text);
        parms.Add("MON", txtMon.Text);
        parms.Add("TUE", txtTue.Text);
        parms.Add("WED", txtWed.Text);
        parms.Add("THU", txtThu.Text);
        parms.Add("FRI", txtFri.Text);
        parms.Add("SAT", txtSat.Text);
        parms.Add("SUN", txtSun.Text);
        //parms.Add("OFFICE_TOTAL", txtTotal.Text);
        parms.Add("WEBSITE", txtWebsite.Text);
        //parms.Add("OFFICE_NEWPATIENT", ddlNewPatient.SelectedItem.Value);
        //parms.Add("OFFICE_REFFERAL", ddlRef.SelectedItem.Value);
        parms.Add("TELEPHONE", ddlTel.SelectedItem.Value);
        parms.Add("TRANSPORT", ddlTrans.SelectedItem.Value);
        //parms.Add("OFFICE_DISABLE", ddlDisable.SelectedItem.Value);
        parms.Add("EBILLING", ddlEbilling.SelectedItem.Value);
        parms.Add("TDD", ddlTDD.SelectedItem.Value);
        parms.Add("OFFICE_ASLOFFERED", ddlASL.SelectedValue);
        parms.Add("ADA_ACCOMODATIONS", AdaAccomodationsselectedValues);
        parms.Add("STAFF_LANGUAGES_SPOKEN", LanguageSpokenSelectedValues);
        parms.Add("IsProviderDirectoryOptout", chkProviderDirectoryOpt.Checked?"1":"0");
        parms.Add("LAST_MODIFIED_DATE_TIME",  DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("TranslationServiceType", TranslationServicesValues);
        if (!(ddlTelehealth.SelectedItem.Value.Equals("")))
            parms.Add("Telehealth", ddlTelehealth.SelectedItem.Value);
        if (!(ddlCHIP.SelectedItem.Value.Equals("")))
            parms.Add("CHIP", ddlCHIP.SelectedItem.Value);
        if (!(ddlNewMedicaid.SelectedItem.Value.Equals("")))
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
        val.ValidationGroup = "valOfficeHours";
        this.Page.Validators.Add(val);
    }

    public override bool HasInputValue()
    {
        return (txtMon.Text.Trim().Length > 0 ||
                txtTue.Text.Trim().Length > 0 ||
                txtWed.Text.Trim().Length > 0 ||
                txtThu.Text.Trim().Length > 0 ||
                txtFri.Text.Trim().Length > 0 ||
                txtSat.Text.Trim().Length > 0 ||
                txtSun.Text.Trim().Length > 0 ||
               // txtTotal.Text.Trim().Length > 0 ||
                txtWebsite.Text.Trim().Length > 0 ||                
                ddlEbilling.SelectedIndex > -1 ||
                ddlTDD.SelectedIndex > -1 ||
                ddlTel.SelectedIndex > -1 ||
                ddlTrans.SelectedIndex > -1);
    }
    
    public override bool ValidateData()
    {
        bool result = true;
        int numberOfQuestionsAnswered = 0;        
        numberOfQuestionsAnswered += ddlEbilling.SelectedIndex > -1  ? 1 : 0;
        
        numberOfQuestionsAnswered += this.ddlTDD.SelectedIndex > -1  ? 1 : 0;
        numberOfQuestionsAnswered += this.ddlTel.SelectedIndex > -1  ? 1 : 0;
        numberOfQuestionsAnswered += this.ddlTrans.SelectedIndex > -1 ? 1 : 0;

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
            txtMon.Text = Helper.GetString("MON", dr);
            txtTue.Text = Helper.GetString("TUE", dr);
            txtWed.Text = Helper.GetString("WED", dr);
            txtThu.Text = Helper.GetString("THU", dr);
            txtFri.Text = Helper.GetString("FRI", dr);
            txtSat.Text = Helper.GetString("SAT", dr );
            txtSun.Text = Helper.GetString("SUN", dr );
           // txtTotal.Text = Helper.GetString("TOTAL", dr);
            txtWebsite.Text = Helper.GetString("WEBSITE", dr);
            if(!string.IsNullOrEmpty(Helper.GetString("NEWPATIENT", dr)))
           // ddlNewPatient.SelectedValue = Helper.GetBool("NEWPATIENT", dr )?"1":"0";
            if (!string.IsNullOrEmpty(Helper.GetString("REFFERAL", dr)))
               // ddlRef.SelectedValue = Helper.GetBool("REFFERAL", dr) ? "1" : "0";
            if (!string.IsNullOrEmpty(Helper.GetString("TELEPHONE", dr)))
                ddlTel.SelectedValue = Helper.GetBool("TELEPHONE", dr) ? "1" : "0";
            if (!string.IsNullOrEmpty(Helper.GetString("TRANSPORT", dr)))
                ddlTrans.SelectedValue = Helper.GetBool("TRANSPORT", dr) ? "1" : "0";
            if (!string.IsNullOrEmpty(Helper.GetString("DISABLE", dr)))
               // ddlDisable.SelectedValue = Helper.GetBool("DISABLE", dr) ? "1" : "0";
            if (!string.IsNullOrEmpty(Helper.GetString("EBILLING", dr)))
                ddlEbilling.SelectedValue = Helper.GetBool("EBILLING", dr) ? "1" : "0";
            if (!string.IsNullOrEmpty(Helper.GetString("TDD", dr)))
                ddlTDD.SelectedValue = Helper.GetBool("TDD", dr) ? "1" : "0";
            if (!string.IsNullOrEmpty(Helper.GetString("OFFICE_ASLOFFERED", dr)))
                ddlASL.SelectedValue = Helper.GetBool("OFFICE_ASLOFFERED", dr) ? "1" : "0";
            if (!string.IsNullOrEmpty(Helper.GetString("IsProviderDirectoryOptout", dr)))
                chkProviderDirectoryOpt.Checked = Helper.GetBool("IsProviderDirectoryOptout", dr) ? true :false;
            if (!string.IsNullOrEmpty(Helper.GetString("Telehealth", dr)))
                ddlTelehealth.SelectedValue = Helper.GetString("Telehealth", dr);
            if (!string.IsNullOrEmpty(Helper.GetString("CHIP", dr)))
                ddlCHIP.SelectedValue = Helper.GetString("CHIP", dr);
            if (!string.IsNullOrEmpty(Helper.GetString("ACCEPTS_NEW_MEDICAID_PATIENTS", dr)))
                ddlNewMedicaid.SelectedValue = Helper.GetString("ACCEPTS_NEW_MEDICAID_PATIENTS", dr);
            //if (!string.IsNullOrEmpty(Helper.GetString("TranslationServiceType", dr)))
            //{
            //    if (Helper.GetInt("TranslationServiceType", dr) == 3)
            //    {
            //        foreach (ListItem item in chkTranslationServiceType.Items)
            //        {
            //            item.Selected = true;
            //        }
            //    }
            //    else if (Helper.GetInt("TranslationServiceType", dr) == 1)
            //    {
            //        foreach (ListItem item in chkTranslationServiceType.Items)
            //        {
            //            if(item.Value.ToString() =="1")
            //            {
            //                item.Selected = true;
            //            }                       

            //        }
            //    }
            //    else if (Helper.GetInt("TranslationServiceType", dr) == 2)
            //    {
            //        foreach (ListItem item in chkTranslationServiceType.Items)
            //        {
            //            if (item.Value.ToString() == "2")
            //            {
            //                item.Selected = true;
            //            }

            //        }
            //    }
            //}

            // OHPNM-1917
            if (inMaintenance(this.WorkflowPage.RegistrationId))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
            }
        }    
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

    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadOfficeHours();
    }

    private void LoadOfficeHours()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OFFICE_TIMING");
        DataTable dtPrimaryPracticeLocation = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        DataRow dtMultiSelectValues = Helper.HasRows(ds) ? ds.Tables[1].Rows[0] : null;
        this.DataList = dtPrimaryPracticeLocation;
        BindADAAccommodations();
        BindLanguagesSpoken();
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
        if (!string.IsNullOrEmpty(Helper.GetString("ADA_ACCOMODATIONS", dr)))
        {
            string[] accomodationIds = Helper.GetString("ADA_ACCOMODATIONS", dr).Split(',');
            foreach (var accomodationId in accomodationIds)
            {
                var comboItem = RadADAAccommodation.FindItemByValue(accomodationId.ToString().Trim());
                if (comboItem != null)
                {
                    comboItem.Checked = true;
                }
            }
        }
        if (!string.IsNullOrEmpty(Helper.GetString("TRANSLATIONsERVICETYPE", dr)))
        {
            string[] TranslationServiceTypeIds = Helper.GetString("TRANSLATIONsERVICETYPE", dr).Split(',');
           
            foreach (var TranslationServiceTypeId in TranslationServiceTypeIds)
            {
                var comboItem = chkTranslationServiceType.Items.FindByValue(TranslationServiceTypeId.ToString().Trim());
                if (comboItem != null)
                {
                    comboItem.Selected = true;
                }
            }
        }
        //TranslationServiceType
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
        get { return "valOfficeHours"; }
    }

    public override string Title
    {
        get { return "Office Information"; }
    }

    public override string IdText
    {
        get { return "ucOfficeHours_" + this.WorkflowPage.RegistrationId; }
    }
}