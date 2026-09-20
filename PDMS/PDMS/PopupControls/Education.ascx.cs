using MAXIMUS.Controllers.PDMS;
using NPOI.HSSF.Record;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_Education : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    private DataSet educationTypes = null;
    private DataSet degreeAwardTypes = null;
    private DataSet EducationTypes
    {
        get
        {
            if (educationTypes == null)
                educationTypes = GetEducationTypes();
            return educationTypes;
        }
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

    private DataSet DegreeAwardTypes
    {
        get
        {
            if (degreeAwardTypes == null)
                degreeAwardTypes = GetDegreeAwardTypes();
            return degreeAwardTypes;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadEducationTypeDropDown();
        LoadState();
        LoadCountry();
        LoadDegreeAwareded();
        LoadSpeciality();
        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
                btnAddEducationItem.Visible = false;
            }
        }

    }

    private void LoadDegreeAwareded()
    {
        Helper.LoadDropDown(ddlEducationType, GetActiveEducationTypes(), "DISPLAY_NAME", "EDUCATION_TYPE_ID", true);
    }
    //protected void tbCountry_TextChanged(object sender, EventArgs e)
    //{
        
    //}
    private void LoadSpeciality()
    {
        DataSet ds = svc.SelectReferenceDataWithoutParam("usp_SelectSPECIALTY_TYPE");
        if (Helper.HasRows(ds))
        {
            Helper.LoadDropDown(ddlSpeciality, ds.Tables[0], "SPECIALTY_TYPE_NAME", "SPECIALTY_TYPE_NAME", true);

        }

    }

    private void LoadEducationTypeDropDown()
    {
        Helper.LoadDropDown(ddlDegreeAward, GetDegreeAwardTypes().Tables[0], "DEGREE_AWARD_NAME", "degree_award_type_id", true);
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Education";
    }
    public override void LoadControlData()
    {
        LoadEducationDetails();
    }

    private void LoadEducationDetails()
    {
        if (!pnlEducationGrid.Visible) return;

        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_EDUCATION", parms);
            if (Helper.HasRows(ds)) grdEducation.DataSource = this.DataList = ds.Tables[0];
            else grdEducation.DataSource = this.DataList = null;
            grdEducation.DataBind();
        }

        btnEducationHistory.Visible = (grdEducation.Rows.Count > 0);
    }

    public override void LoadData(System.Data.DataRow dr = null)
    {

        hdnRegEducationId.Value = string.Empty;
        int educationtype = 0;
        int degreeAwardedId = 0;
        if (dr != null)
        {
            pnlEducationEntry.Visible = true;
            hdnRegEducationId.Value = Helper.GetData("REG_EDUCATION_ID", dr);
            if (!string.IsNullOrEmpty(Helper.GetData("SCHOOL", dr)))
            {
                tbSchool.Text = Helper.GetString("SCHOOL", dr);
            }
            if (!string.IsNullOrEmpty(Helper.GetData("SPECIALTY_NAME", dr)))
            {
                foreach (ListItem item in ddlSpeciality.Items)
                {
                    if (item.Value.Equals(Helper.GetString("SPECIALTY_NAME", dr), StringComparison.CurrentCultureIgnoreCase))
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
            

            //if (!string.IsNullOrEmpty(Helper.GetData("FIELDOFSTUDY", dr)))
            //{
            //    tbFieldStudy.Text = Helper.GetString("FIELDOFSTUDY", dr).ToString();
            //}
            if (!string.IsNullOrEmpty(Helper.GetData("EDUCATION_TYPE_ID", dr)))
            {
                educationtype = Helper.GetInt("EDUCATION_TYPE_ID", dr);

                if (educationtype > 0)
                {
                    ddlEducationType.SelectedValue = educationtype.ToString();
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetData("degree_awarded_type_id", dr)))
            {
                degreeAwardedId = Helper.GetInt("degree_awarded_type_id", dr);

                if (degreeAwardedId > 0)
                {
                    ddlDegreeAward.SelectedValue = degreeAwardedId.ToString();
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetData("Start_year", dr)))
            {
                tbStartYear.Text = Helper.GetDate("Start_year", dr).ToString();
            }
            if (!string.IsNullOrEmpty(Helper.GetData("end_year", dr)))
            {
                tbEndYear.Text = Helper.GetDate("end_year", dr).ToString();
            }
            if (!string.IsNullOrEmpty(Helper.GetData("ADDRESS1", dr)))
            {
                tbAddress1.Text = Helper.GetString("ADDRESS1", dr).ToString();
            }

            if (!string.IsNullOrEmpty(Helper.GetData("ADDRESS2", dr)))
            {
                tbAddress2.Text = Helper.GetString("ADDRESS2", dr).ToString();
            }
            if (!string.IsNullOrEmpty(Helper.GetData("CITY", dr)))
            {
                tbCity.Text = Helper.GetString("CITY", dr).ToString();
            }
            if (!string.IsNullOrEmpty(Helper.GetData("STATE", dr)))
            {
                ddlState.SelectedValue = Helper.GetData("STATE", dr).ToString();
            }
            if (!string.IsNullOrEmpty(Helper.GetData("ZIP", dr)))
            {
                tbZipCode.Text = Helper.GetString("ZIP", dr).ToString();
            }

            if (!string.IsNullOrEmpty(Helper.GetData("COUNTRY", dr)) && ddlCountry.Items.FindByValue(Helper.GetString("COUNTRY", dr).ToString().Trim()) != null)
            {                
                ddlCountry.SelectedValue = Helper.GetString("COUNTRY", dr).ToString();
                if (!string.IsNullOrEmpty(ddlCountry.SelectedItem.Text) && (ddlCountry.SelectedItem.Text.ToLower() == "united states"))
                {                    
                    RfvState.Visible = true;
                    RfvZip.Visible = true;
                    lblState.Text = "* State:";
                    lblZipCode.Text = "* Zip Code:";
                    ddlState.Enabled = true;
                }
                else
                {
                    RfvState.Visible = false;
                    RfvZip.Visible = false;
                    lblState.Text = "State:";
                    lblZipCode.Text = "Zip Code:";
                    ddlState.ClearSelection();
                    ddlState.Enabled = false;
                }
            }

            if (!string.IsNullOrEmpty(Helper.GetData("CONTACT_PHONE", dr)))
            {
                tbPhone.Text = Helper.FormatPhone(Helper.GetString("CONTACT_PHONE", dr));
            }
            if (!string.IsNullOrEmpty(Helper.GetData("CONTACT_FAX", dr)))
            {
                tbFax.Text = Helper.FormatPhone(Helper.GetString("CONTACT_FAX", dr));
            }
            if (!string.IsNullOrEmpty(Helper.GetData("ADDITIONAL_INFO", dr)))
            {
                tbAdditional.Text = Helper.GetString("ADDITIONAL_INFO", dr);
            }

        }
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
    }

    public override bool SaveData()
    {
        if (pnlEducationEntry.Visible)
        {
            Page.Validate("vgEducation");
            for (int i = 0; i < Page.Validators.Count; i++)
            {
                BaseValidator v;
                try
                {
                    v = Page.Validators[i] as BaseValidator;
                    if (v != null && v.ValidationGroup.Equals("vgEducation") && !v.IsValid)
                        return false;
                }
                catch
                {
                    continue;
                }
            }
            if (ValidateData())
            {
                //Insert New record
                try
                {
                    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());

                    parms.Add("school", tbSchool.Text.Trim());
                    //parms.Add("fieldofstudy", tbFieldStudy.Text.Trim());
                    if (tbStartYear.Text.Trim() == string.Empty)
                    {
                        parms.Add("start_year", null);
                    }
                    else
                    {
                        DateTime dateValue;
                        if (DateTime.TryParse(tbStartYear.Text.Trim(), out dateValue)) {
                            parms.Add("start_year", dateValue.ToString());
                        } else
                        {
                            parms.Add("start_year", null);
                        }
                    }
                    if (tbEndYear.Text.Trim() == string.Empty)
                    {
                        parms.Add("end_year", null);
                    }
                    else
                    {
                        DateTime dateValue;
                        if (DateTime.TryParse(tbEndYear.Text.Trim(), out dateValue))
                        {
                            parms.Add("end_year", dateValue.ToString());
                        }
                        else
                        {
                            parms.Add("end_year", null);
                        }
                    }

                    if (ddlEducationType != null && ddlEducationType.SelectedIndex > 0)
                    {
                        parms.Add("@education_type_id", ddlEducationType.SelectedItem.Value.ToString());
                    }
                    else
                    {
                        parms.Add("@education_type_id", "0");
                    }
                    if (ddlDegreeAward != null && ddlDegreeAward.SelectedIndex > 0)
                    {
                        parms.Add("@degree_awarded_type_id", ddlDegreeAward.SelectedItem.Value.ToString());
                    }
                    else
                    {
                        parms.Add("@degree_awarded_type_id", "0");
                    }
                    if (ddlState != null && ddlState.SelectedIndex > 0)
                    {
                        parms.Add("@state", ddlState.SelectedItem.Value.ToString());
                    }
                    else
                    {
                        parms.Add("@state", string.Empty);
                    }
                    parms.Add("address1", tbAddress1.Text.Trim());

                    parms.Add("address2", tbAddress2.Text.Trim());
                    parms.Add("city", tbCity.Text.Trim());
                    parms.Add("zip", tbZipCode.Text.Trim());

                    if (ddlCountry != null && ddlCountry.SelectedIndex > 0)
                    parms.Add("country", ddlCountry.SelectedItem.Value.Trim());                    
                    else
                    parms.Add("country", string.Empty);                    

                    parms.Add("contact_phone", Helper.StripNonNumerics(tbPhone.Text.Trim()));
                    // parms.Add("contact_phone_ext", string.empty);
                    parms.Add("contact_fax", Helper.StripNonNumerics(tbFax.Text.Trim()));
                    // parms.Add("contact_phone_ext", txtBoard.Text.trim);

                    parms.Add("additional_info", tbAdditional.Text.Trim());

                    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("SPECIALTY_NAME", ddlSpeciality.SelectedValue.ToString());

                    if (!string.IsNullOrEmpty(hdnRegEducationId.Value))
                    {
                        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                        parms.Add("REG_EDUCATION_ID", hdnRegEducationId.Value.Trim());
                        psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "Education", parms);
                    }
                    else
                    {
                        parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
                        parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
                        psc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "Education", parms);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("ucEducation_SaveData - " + ex.Message));
                }
            }

            return false;
        }
        else if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 && grdEducation.Rows.Count == 0)  //OHPNM-2415
            return false;
        return true;
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override string Title
    {
        get { return "Education Details"; }
    }

    public override string IdText
    {
        get { return "ucEducation_" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
        get { return "vgEducation"; }
    }
    protected void ddlEducationType_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddlDegreeAward_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    private DataSet GetEducationTypes()
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            return psc.GetEducationTypes();
        }
    }
    private DataSet GetDegreeAwardTypes()
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            return psc.GetDegreeAwardTypes();
        }
    }
    private DataTable GetActiveEducationTypes()
    {
        DataTable dtEducation = null;
        try
        {
            dtEducation = EducationTypes.Tables[0].Select("IS_VISIBLE=1").CopyToDataTable();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dtEducation;
    }

    protected void grdEducation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);

        pnlEducationGrid.Visible = true;
        if (Helper.HasRows(this.DataList))
            this.LoadData(this.DataList.Rows[index]);
        else
            this.LoadData(null);
    }

    protected void btnAddEducationItem_Click(object sender, CommandEventArgs e)
    {
        pnlEducationEntry.Visible = true;
        this.LoadData(null);
    }
    protected void btnEducationHistory_Click(object sender, CommandEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "EDUCATIONHistory");
        DataTable dt = null;
        if (Helper.HasRows(ds)) dt = ds.Tables[0];

        switch (e.CommandName)
        {
            case "EducationHistory":
                lbl_title.Text = "Education History";
                mltPopup.ActiveViewIndex = 0;
                ucEducationHistory.LoadData(dt);
                mpeEducationHistory.Show();
                break;
        }
    }

    public void LoadState()
    {
        Helper.LoadDropDownListWithStates(ref ddlState);

    }
    public void LoadCountry()
    {        
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        string eTarget = (Request.Params["__EVENTTARGET"] != null)? Request.Params["__EVENTTARGET"].ToString() : null;
        DataSet ds = psc.GetCountryCodes();
        if (eTarget!= null && !eTarget.Contains("ddlCountry"))
        {
            Helper.LoadList(ddlCountry, ds.Tables[0], "COUNTRY_DESC", "COUNTRY_CODE", true);
            ddlCountry.Items.FindByValue("US").Selected = true;
            if (!string.IsNullOrEmpty(ddlCountry.SelectedItem.Text) && (ddlCountry.SelectedItem.Text.ToLower() == "united states"))
            {
                RfvState.Visible = true;
                RfvZip.Visible = true;
                lblState.Text = "*State:";
                lblZipCode.Text = "* Zip Code:";
            }
            else
            {
                RfvState.Visible = false;
                RfvZip.Visible = false;
                lblState.Text = "State:";
                lblZipCode.Text = "Zip Code:";
            }
        }
    }

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlCountry.SelectedItem.Text) && (ddlCountry.SelectedItem.Text.ToLower() == "united states" ))
        {
            RfvState.Visible = true;
            RfvZip.Visible = true;
            lblState.Text = "* State:";
            lblZipCode.Text = "* Zip Code:";
            ddlState.Enabled = true;
        }
        else
        {
            RfvState.Visible = false;
            RfvZip.Visible = false;
            lblState.Text = "State:";
            lblZipCode.Text = "Zip Code:";
            ddlState.ClearSelection();
            ddlState.Enabled = false;
        }                
    }
    public override bool HasInputValue()
    {
        bool rtn = false;
        if (ddlState.SelectedItem != null || ddlCountry.SelectedItem != null || ddlSpeciality.SelectedItem != null || ddlEducationType.SelectedItem != null ||
            ddlDegreeAward.SelectedItem != null || ddlState.SelectedItem != null || !string.IsNullOrEmpty(tbSchool.Text) || !string.IsNullOrEmpty(tbStartYear.Text) ||
            !string.IsNullOrEmpty(tbEndYear.Text) || !string.IsNullOrEmpty(tbAddress1.Text) || !string.IsNullOrEmpty(tbZipCode.Text))
        {

            rtn = true;


        }
        else
        {
            var Nodes = this.WorkflowPage.RegistrationNodes;
            int required = Nodes.Where(s => s.Value.Step == CON.AddressType.Education)
                                .Select(d => d.Value.IsRequired).Max();
            if (required == 0)
            {
                rtn = true;
            }
            else
            {
                rtn = false;
            }
        }

        return rtn;

    }
}