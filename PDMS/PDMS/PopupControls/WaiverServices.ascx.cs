using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Pages_WaiverServices : BaseSectionControl
{
    #region " Properties "
    String[] chkBoxList = {"chkArtTherapySpecialties", "chkBehavioralSupportSpecialties", 
            "chkDayHabilitationSpecialties", "chkEmploymentReadiness", 
            "chkEnvAccessibilities", "chkFamilyTraining", "chkHostHome", 
            "chkIndividualizedDay", "chkInHomeSupports", 
            "chkPERS", "chkRespite", "chkResHabilitation", "chkSkilledNursing",  
            "chkSpeechHearing", "chkSupportEmployment",
            "chkSupportedLiving", "chkWellness", "chkOther"};
    #endregion

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
    }
    public override void LoadData(DataRow row = null)
    {

    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        int applicationTypeID = 0;
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");

        if (Helper.HasRows(ds))
        {
            applicationTypeID = Helper.GetInt("APPLICATION_TYPE_ID", ds.Tables[0].Rows[0]);
        }


            if (!IsPostBack)
            {
                LoadGrid(rgArtTherapySpecialties);
                LoadGrid(rgBehavioralSupportSpecialties);
                LoadGrid(rgDayHabilitationSpecialties);
                LoadGrid(rgEmploymentReadiness);
                LoadGrid(rgEnvAccessibilities);
                LoadGrid(rgFamilyTraining);
                LoadGrid(rgHostHome);
                LoadGrid(rgIndividualizedDay);
                LoadGrid(rgInHomeSupports);
                LoadGrid(rgPERS);
                LoadGrid(rgRespite);
                LoadGrid(rgResHabilitation);
                LoadGrid(rgSkilledNursing);
                LoadGrid(rgSpeechHearing);
                LoadGrid(rgSupportEmployment);
                LoadGrid(rgSupportedLiving);
                LoadGrid(rgWellness);
                LoadGrid(rgOther);
                LoadGrid(rgOtherCapacity);


                LoadDDSData();
            }

            rgEPDSpecialties.Visible = false;
            pnlArtTherapy.Visible =
            pnlBehavioralSupport.Visible =
            pnlDayHabilitationSpecialties.Visible =
            pnlEmploymentReadiness.Visible =
            pnlEnvAccessibilities.Visible =
            pnlFamilyTraining.Visible =
            pnlHostHome.Visible =
            pnlIndividualizedDay.Visible =
            pnlInHomeSupports.Visible =
            pnlPERS.Visible =
            pnlRespite.Visible =
            pnlResHabilitation.Visible =
            pnlSkilledNursing.Visible =
            pnlSpeechHearing.Visible =
            pnlSupportEmployment.Visible =
            pnlSupportedLiving.Visible =
            pnlWellness.Visible =
            pnlOther.Visible = true;
            IDDMessage.Visible = true;


            if (!(Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) ||
                       Helper.IsUserInScreeningRole(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString())
                       || Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name.ToString())
                       || Helper.IsUserInDDSReviewerRole(HttpContext.Current.User.Identity.Name.ToString())
                       ))
            {
                chkPersonalVehicles.Enabled = nbPersonalVehicles.Enabled = false;
                chkAgencyCar.Enabled = nbAgencyCar.Enabled = false;
                chkAgencyMinivan.Enabled = nbAgencyMinivan.Enabled = false;
                chkAgencySmallVan.Enabled = nbAgencySmallVan.Enabled = false;
                chkAgencyOther.Enabled = nbAgencyOther.Enabled = txtAgencyOtherDesc.Enabled = false;
                nbHostHomes.Enabled = false;
                nbFootage.Enabled = false;

                foreach (string strChk in chkBoxList) //we find each of the above checkboxes 
                {
                    CheckBox chkBox = (CheckBox)this.FindControl(strChk);

                    if (chkBox != null)
                        chkBox.Enabled = false;
                }
            }
        
    }

    public override bool SaveData()
    {

        if (!rgEPDSpecialties.Visible) //if not EPD or ADHP
    {
            Boolean IsValid = SaveDDSSpecialtiesApprovals();

            if (!IsValid) return false;
        }

        int regDDSId = 0;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();

        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DDS");
        if (Helper.HasRows(ds))
        {
            regDDSId = Helper.GetInt("REG_DDS_ID", ds.Tables[0].Rows[0]);
        }

        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("FACILITY_FOOTAGE", nbFootage.Text);
        parms.Add("NUM_HOMES", nbHostHomes.Text);
        parms.Add("NUM_PERSONAL_VEHICLE",   nbPersonalVehicles.Text);
        parms.Add("NUM_AGENCY_CAR", nbAgencyCar.Text);
        parms.Add("NUM_AGENCY_MINIVAN", nbAgencyMinivan.Text);
        parms.Add("NUM_AGENCY_SMALLVAN", nbAgencySmallVan.Text);
        parms.Add("NUM_AGENCY_VEHICLE_OTHER", nbAgencyOther.Text);
        parms.Add("AGENCY_VEHICLE_OTHER_DESC", txtAgencyOtherDesc.Text);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        if (regDDSId > 0)
        {
            parms.Add("REG_DDS_ID", regDDSId.ToString());
            psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "DDS", parms);
        }
        else
            psc.InsertRegistrationDataTable("DDS", parms);

        return true;
    }

    private Boolean ValidateGrid(RadGrid grid)
    {
        Boolean returnvalue = false;
     
        Dictionary<string, object> newValues = new Dictionary<string, object>();

        foreach (GridDataItem item in grid.Items)
        {
            item.ExtractValues(newValues);
            int capacityInt = 0;
            string capacity = (Convert.ToString(newValues["CAPACITY"]) == "&nbsp;" || string.IsNullOrWhiteSpace(Convert.ToString(newValues["CAPACITY"]))) ? "0" : newValues["CAPACITY"].ToString();
            if (int.TryParse(capacity, out capacityInt) && int.Parse(capacity) > 0 && int.Parse(capacity) < 999)
            {
                returnvalue=true;
                return returnvalue;
            }
        }
        return returnvalue;
    }

    private Boolean SaveDDSSpecialtiesApprovals()
    {
         

        foreach (string strChk in chkBoxList) //we find each of the above checkboxes and see if it is checked
        {
            CheckBox chkBox = (CheckBox)this.FindControl(strChk);

            if (chkBox != null)
            {
                if (chkBox.Visible && (chkBox.Checked || chkBox.Attributes["value"] != ""))
                {
                    string gridId = strChk.Replace("chk", "rg");

                    RadGrid grid = (RadGrid)this.FindControl(gridId); //find the associated grid

                   

                    foreach (GridDataItem item in grid.Items)
                    {

                        TextBox txtEndDate = grid.FindControl("txtEndDate") as TextBox;
                        string startDate = (item["START_DATE"].Controls[0] as TextBox).Text;
                        string endDate = "";
                        bool providerEdit = false;
                        bool ddsEdit = false;
                        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();


                        if (txtEndDate != null && Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name.ToString()))
                            endDate = txtEndDate.Text;
                        else
                            endDate = Helper.ConvertStrNullToString(item["END_DATE"].Text);

                        int regSpecialtyID = Convert.ToInt32(item.GetDataKeyValue("REG_SPECIALTY_ID"));
                        int regSpecialtyEmployeeID = Convert.ToInt32(item.GetDataKeyValue("REG_SPECIALTY_EMPLOYEE_ID"));

                        int regSpecialtyCategoryID = 0;
                        bool IsCategoryType = false;
                        if (item.KeyValues.Contains("REG_SPECIALTY_CATEGORY_ID"))
                        {
                            IsCategoryType = true;
                            regSpecialtyCategoryID = Convert.ToInt32(item.GetDataKeyValue("REG_SPECIALTY_CATEGORY_ID"));
                        }
                        int approval = Convert.ToInt32(item.GetDataKeyValue("REG_SPECIALTY_APPROVAL_ID"));
                        Dictionary<string, string> parms = new Dictionary<string, string>();

                        providerEdit = chkBox.Checked;

                        CheckBox chkOpertorApproved = item["chkSpecialtyOperatorApproved"].Controls[0] as CheckBox;
                        CheckBox chkReviewerApproved = item["chkSpecialtyReviewerApproved"].Controls[0] as CheckBox;

                        ddsEdit = chkOpertorApproved.Checked || chkReviewerApproved.Checked;

                        Dictionary<string, object> newValues = new Dictionary<string, object>();
                        item.ExtractValues(newValues);

                        //if (providerEdit == false && (regSpecialtyEmployeeID == 0 && ddsEdit))
                        //{
                        //    providerEdit = true;
                        //}
                        // Validate the capacity field
                        //if (providerEdit)
                        //{
                            string specialtyTypeID = item["SPECIALTY_TYPE_ID"].Text.ToString();
                            string capacity = (newValues["CAPACITY"] == null) ? "" : newValues["CAPACITY"].ToString();
                            if (approval > 0 && Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName.ToString())
                                && regSpecialtyEmployeeID > 0 && chkBox.Checked == false)
                            {
                                CustomValidator val = new CustomValidator();
                                val.IsValid = false;
                                val.ErrorMessage = "The specialty is previously approved by DDS and cannot be removed.";
                                val.ValidationGroup = "valProviderInfoHeader";
                                this.Page.Validators.Add(val);

                                return false;
                            }
                             
                            int capacityint = (capacity == "") ? 0 : Convert.ToInt32(capacity);

                            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                            parms.Add("START_DATE", (startDate == "") ? DateTime.Now.ToString() : startDate);
                            parms.Add("END_DATE", (endDate == "") ? null : endDate);
                            parms.Add("PRIMARY_FLAG", "0");
                            parms.Add("SPECIALTY_TYPE_ID", specialtyTypeID);
                            parms.Add("SPECIALTY_BOARD_CERTIFIED", "N");
                            parms.Add("CAPACITY", capacityint.ToString());
                            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                            if (regSpecialtyID <= 0)
                            {
                                    regSpecialtyID = svc.InsertRegistrationDataTable("SPECIALTY", parms);
                            }

                            // Clear the REG_SPECIALTY specific parameters
                            parms.Remove("REG_ID");
                            parms.Remove("PRIMARY_FLAG");
                            parms.Remove("SPECIALTY_TYPE_ID");
                            parms.Remove("SPECIALTY_BOARD_CERTIFIED");

                            // Insert/Update the REG_SPECIALTY_EMPLOYEE_ID record
                            int specialtyEmployeeTypeID = Convert.ToInt32(item.GetDataKeyValue("SPECIALTY_EMPLOYEE_TYPE_ID"));
                            parms.Add("REG_SPECIALTY_ID", regSpecialtyID.ToString());

                            if (IsCategoryType)
                            {
                                int specialtyCategoryID = Convert.ToInt32(item.GetDataKeyValue("SPECIALTY_CATEGORY_ID"));

                                parms.Add("SPECIALTY_CATEGORY_ID", specialtyCategoryID.ToString());

                                if (regSpecialtyCategoryID > 0)
                                {
                                    if (chkBox.Checked)
                                        svc.UpdateRegistrationDataTable("SPECIALTY_CATEGORY", parms);
                                    else
                                    {
                                        svc.DeleteRegistrationData("SPECIALTY_CATEGORY", "REG_SPECIALTY_CATEGORY_ID", regSpecialtyCategoryID);

                                        // If no other entries in REG_SPECIALTY_CATEGORY, delete the REG_SPECIALTY record
                                        parms.Clear();
                                        parms.Add("REG_SPECIALTY_ID", regSpecialtyID.ToString());
                                        parms.Add("SPECIALTY_CATEGORY_ID", specialtyCategoryID.ToString());
                                        svc.DeleteRegistrationDataWithParams("SPECIALTYCustom", parms);
                                    }
                                }
                                else
                                {
                                    if (chkBox.Checked)
                                        regSpecialtyCategoryID = svc.InsertRegistrationDataTable("SPECIALTY_CATEGORY", parms);
                                }
                            }
                            else
                            {

                                parms.Add("SPECIALTY_EMPLOYEE_TYPE_ID", specialtyEmployeeTypeID.ToString());



                                if (regSpecialtyEmployeeID > 0)
                                {
                                    if (chkBox.Checked)
                                    {
                                        parms.Add("REG_SPECIALTY_EMPLOYEE_ID", regSpecialtyEmployeeID.ToString());
                                        svc.UpdateRegistrationDataTable("SPECIALTY_EMPLOYEE", parms);
                                    }
                                    else
                                    {
                                        svc.DeleteRegistrationData("SPECIALTY_EMPLOYEE", "REG_SPECIALTY_EMPLOYEE_ID", regSpecialtyEmployeeID);

                                        // If no other entries in REG_SPECIALTY_EMPLOYEE, delete the REG_SPECIALTY record
                                        parms.Clear();
                                        parms.Add("REG_SPECIALTY_ID", regSpecialtyID.ToString());
                                        parms.Add("SPECIALTY_EMPLOYEE_TYPE_ID", specialtyEmployeeTypeID.ToString());
                                        svc.DeleteRegistrationDataWithParams("SPECIALTYCustom", parms);
                                    }
                                }
                                else
                                {
                                    regSpecialtyEmployeeID = svc.InsertRegistrationDataTable("SPECIALTY_EMPLOYEE", parms);
                                }
                            }
                        //}

                        if (ddsEdit) // Being edited by the DDS worker
                        {
                            parms.Clear();
                            parms.Add("REG_SPECIALTY_ID", regSpecialtyID.ToString());
                            parms.Add("REG_SPECIALTY_EMPLOYEE_ID", regSpecialtyEmployeeID.ToString());
                            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                            bool specialtyOpertorApproved = chkOpertorApproved.Checked;
                            bool specialtyReviewerApproved = chkReviewerApproved.Checked;

                            if (approval > 0)
                            {
                                Dictionary<string, string> parms1 = new Dictionary<string, string>();

                                parms1.Add("IsOperatorApproved", specialtyOpertorApproved.ToString());
                                parms1.Add("IsReviewerApproved", specialtyReviewerApproved.ToString());

                                parms1.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                                parms1.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                                parms1.Add("REG_SPECIALTY_APPROVAL_ID", approval.ToString());
                                parms1.Add("REG_SPECIALTY_ID", regSpecialtyID.ToString());

                                if (regSpecialtyCategoryID >0)
                                    parms1.Add("REG_SPECIALTY_CATEGORY_ID", regSpecialtyCategoryID.ToString());

                                if (regSpecialtyEmployeeID > 0)
                                    parms1.Add("REG_SPECIALTY_EMPLOYEE_ID", regSpecialtyEmployeeID.ToString());

                                svc.UpdateRegistrationDataTable("SPECIALTY_APPROVAL", parms1);
                            }
                            else
                            {
                                if (specialtyOpertorApproved || specialtyReviewerApproved)
                                {
                                    parms.Add("IsOperatorApproved", specialtyOpertorApproved.ToString());
                                    parms.Add("IsReviewerApproved", specialtyReviewerApproved.ToString());
                                    svc.InsertRegistrationDataTable("SPECIALTY_APPROVAL", parms);
                                }
                            }
                        }
                    }
                }
            }
        }
        return true;
    }


    public override bool ValidateData()
    {
        bool specialtySelected = false;

        //check for atleast one specialty is selected
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        DataSet ds = new DataSet();
        ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTYServices");
        DataRow[] dr = ds.Tables[0].Select("SPECIALTY_SELECTED = 'true'");

        DataSet ds1 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "1");
        ds1 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr1 = ds1.Tables[0].Select("SPECIALTY_SELECTED = 'true'");

        parameters.Clear();

        DataSet ds2 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "2");
        ds2 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr2 = ds2.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds3 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "18");
        ds3 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr3 = ds3.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds4 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "3");
        ds4 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr4 = ds4.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds5 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "4");
        ds5 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr5 = ds5.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds6 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "5");
        ds6 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr6 = ds6.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds7 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "6");
        ds7 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr7 = ds7.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds8 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "7");
        ds8 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr8 = ds8.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds9 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "8");
        ds9 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr9 = ds9.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds10 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "9");
        ds10 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr10 = ds10.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds11 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "10");
        ds11 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr11 = ds11.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds12 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "11");
        ds12 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr12 = ds12.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds13 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "12");
        ds13 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr13 = ds13.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds14 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "13");
        ds14 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr14 = ds14.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds15 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "14");
        ds15 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr15 = ds15.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds16 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "15");
        ds16 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr16 = ds16.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds17 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "16");
        ds17 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr17 = ds17.Tables[0].Select("SPECIALTY_SELECTED = 'true'");
        parameters.Clear();

        DataSet ds18 = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parameters.Add("SPECIALTY_CATEGORY_ID", "17");
        ds18 = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
        DataRow[] dr18 = ds18.Tables[0].Select("SPECIALTY_SELECTED = 'true'");

       

        Boolean AnyCheckBoxChecked = false;
        foreach (string strChk in chkBoxList) //we find each of the above checkboxes and see if it is checked
        {
            CheckBox chkBox = (CheckBox)this.FindControl(strChk);

            if (chkBox != null)
            {
                if (chkBox.Checked)
                {
                    AnyCheckBoxChecked = true;
                    string gridId = strChk.Replace("chk", "rg");
                    RadGrid grid = (RadGrid)this.FindControl(gridId); //find the associated grid
                   
                    Boolean isValid = ValidateGrid(grid);

                    //if (!isValid)
                    //{
                    //    CustomValidator val = new CustomValidator();
                    //    val.IsValid = false;
                    //    val.ErrorMessage = "Capacity is required for at least one service ";
                    //    val.ValidationGroup = "valProviderInfoHeader";
                    //    this.Page.Validators.Add(val);
                    //    return false;
                    //}

                }
            }
        }
        if (AnyCheckBoxChecked == false &&
            ((dr.Count() == 0 && dr1.Count() == 0 && dr2.Count() == 0 && dr3.Count() == 0 && dr4.Count() == 0 && dr5.Count() == 0 &&
            dr6.Count() == 0 && dr7.Count() == 0 && dr8.Count() == 0 && dr9.Count() == 0 && dr10.Count() == 0 && dr11.Count() == 0 &&
            dr12.Count() == 0 && dr13.Count() == 0 && dr14.Count() == 0 && dr15.Count() == 0 && dr16.Count() == 0 && dr17.Count() == 0 &&
            dr18.Count() == 0))) //if none are checked then show validator
        {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "Please select at least one service you would like to provide";
                val.ValidationGroup = "valProviderInfoHeader";
                this.Page.Validators.Add(val);
                return false;
        }

        // Validate Number of Host Homes

        if (chkHostHome.Visible)
            specialtySelected = chkHostHome.Checked;
        else
        {
        DataSet dsHostHomes = GetGridDataSource(rgHostHome);
        if (Helper.HasRows(dsHostHomes))
        {
            foreach (DataRow row in dsHostHomes.Tables[0].Rows)
            {
                if (Helper.GetBool("SPECIALTY_SELECTED", row) == true)
                {
                    specialtySelected = true;
                    break;
                }
            }
        }
        }

        if (specialtySelected)
        {
            if (nbHostHomes.Text.Trim() == string.Empty)
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "Number of host homes is required.";
                val.ValidationGroup = "valProviderInfoHeader";
                this.Page.Validators.Add(val);
                return false;
            }
            else
            {
                string hostHomes = nbHostHomes.Text.Trim();
                if (int.Parse(hostHomes) < 1 || int.Parse(hostHomes) > 999)
                {
                    CustomValidator val = new CustomValidator();
                    val.IsValid = false;
                    val.ErrorMessage = "Number of host homes must be between 1 - 999.";
                    val.ValidationGroup = "valProviderInfoHeader";
                    this.Page.Validators.Add(val);
                    return false;
                }
            }
        }

        // Validate Facility Footage
        specialtySelected = false;
        if (chkDayHabilitationSpecialties.Visible)
            specialtySelected = chkDayHabilitationSpecialties.Checked;
        else
        {
        DataSet dsFootage = GetGridDataSource(rgDayHabilitationSpecialties);
        if (Helper.HasRows(dsFootage))
        {
            foreach (DataRow row in dsFootage.Tables[0].Rows)
            {
                if (Helper.GetBool("SPECIALTY_SELECTED", row) == true)
                {
                    specialtySelected = true;
                    break;
                }
            }
        }
        }
        if (specialtySelected)
        {
            if (nbFootage.Text.Trim() == string.Empty || nbFootage.Text == "0")
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "Facility footage is required.";
                val.ValidationGroup = "valProviderInfoHeader";
                this.Page.Validators.Add(val);
                return false;
            }
        }

        return true;
    }

    protected void rgEPDSpecialties_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        GridEditableItem item = e.Item as GridEditableItem;
        TextBox txtEndDate = item.FindControl("txtEndDate") as TextBox;
        string startDate = Helper.ConvertStrNullToString(item.SavedOldValues["START_DATE"]);
        string endDate = "";
        bool providerEdit = false;
        bool ltcEdit = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

        if (Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) &&
            (Helper.IsUserInScreeningRole(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString())))
            endDate = txtEndDate.Text;
        else
            endDate = Helper.ConvertStrNullToString(item.SavedOldValues["END_DATE"]);

        int id = Convert.ToInt32(item.GetDataKeyValue("REG_SPECIALTY_ID"));
        int approval = Convert.ToInt32(item.GetDataKeyValue("REG_SPECIALTY_APPROVAL_ID"));
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());

        Dictionary<string, object> newValues = new Dictionary<string, object>();
        item.ExtractValues(newValues);

        providerEdit = newValues.ContainsKey("SPECIALTY_SELECTED");
        ltcEdit = newValues.ContainsKey("SPECIALTY_APPROVED");
        if (providerEdit == false && (id == 0 && ltcEdit))
        {
            providerEdit = true;
        }

        if (providerEdit) // Being edited by the provider
        {
            parms.Add("START_DATE", (startDate == "") ? DateTime.Now.ToString() : startDate);
            parms.Add("END_DATE", (endDate == "") ? null : endDate);
            parms.Add("PRIMARY_FLAG", "0");
            if (approval > 0 && Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName.ToString())
                && id > 0 && Convert.ToBoolean(newValues["SPECIALTY_SELECTED"]) == false)
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "The specialty is previously approved by LTC and cannot be removed.";
                val.ValidationGroup = "valProviderInfoHeader";
                this.Page.Validators.Add(val);

                e.Canceled = true;
                return;
            }
            bool specialtySelected = false;
            if (id == 0 && ltcEdit) // happens when the LTC operator approves a service not selected by the provider
                specialtySelected = true;
            else
                if (newValues.ContainsKey("SPECIALTY_SELECTED"))
                specialtySelected = Convert.ToBoolean(newValues["SPECIALTY_SELECTED"]);

            string specialtyTypeID = newValues.FirstOrDefault(v => v.Key == "SPECIALTY_TYPE_ID").Value.ToString();
            parms.Add("SPECIALTY_TYPE_ID", specialtyTypeID);
            parms.Add("SPECIALTY_BOARD_CERTIFIED", "N");

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            if (id > 0)
            {
                parms.Add("REG_SPECIALTY_ID", id.ToString());
                if (specialtySelected)
                    svc.UpdateRegistrationDataTable("SPECIALTY", parms);
                else
                    svc.DeleteRegistrationData("SPECIALTY", "REG_SPECIALTY_ID", id);
            }
            else
            {
                if (specialtySelected)
                    id = svc.InsertRegistrationDataTable("SPECIALTY", parms);
            }

        }

        if (ltcEdit) // Being edited by the LTC worker
        {
            parms.Clear();
            parms.Add("REG_SPECIALTY_ID", id.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            bool specialtyApproved = Convert.ToBoolean(newValues.FirstOrDefault(v => v.Key == "SPECIALTY_APPROVED").Value);

            if (approval > 0)
            {
                if (!specialtyApproved)
                {
                    svc.DeleteRegistrationData("SPECIALTY_APPROVAL", "REG_SPECIALTY_APPROVAL_ID", approval);
                }
            }
            else
            {
                if (specialtyApproved)
                    svc.InsertRegistrationDataTable("SPECIALTY_APPROVAL", parms);
            }
        }
    }

    protected void rgEPDSpecialties_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = GetGridDataSource((RadGrid)sender);
        if (Helper.HasRows(ds))
            ((RadGrid)sender).DataSource = ds;
        else
            ((RadGrid)sender).DataSource = null;
    }

    protected void rgEPDSpecialties_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            (rgEPDSpecialties.MasterTableView.GetColumn("START_DATE") as GridBoundColumn).ReadOnly = true;
            (rgEPDSpecialties.MasterTableView.GetColumn("MMIS_SPECIALTY_TYPE_ID") as GridBoundColumn).ReadOnly = true;
            (rgEPDSpecialties.MasterTableView.GetColumn("SPECIALTY_TYPE_NAME") as GridBoundColumn).ReadOnly = true;

            if (Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) &&
                (Helper.IsUserInScreeningRole(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString())))
                (rgEPDSpecialties.MasterTableView.GetColumn("END_DATE") as GridTemplateColumn).ReadOnly = false;
            else
                (rgEPDSpecialties.MasterTableView.GetColumn("END_DATE") as GridTemplateColumn).ReadOnly = true;
        }
    }

    protected void rgEPDSpecialties_DataBound(object sender, EventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (Helper.IsUserInLTCWorkerRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInDDSReviewerRole(HttpContext.Current.User.Identity.Name))
        {
            ((GridCheckBoxColumn)rgEPDSpecialties.Columns.FindByUniqueName("chkSpecialty")).ReadOnly = true;
            (grid.MasterTableView.GetColumn("START_DATE") as GridBoundColumn).ReadOnly = true;
            (grid.MasterTableView.GetColumn("MMIS_SPECIALTY_TYPE_ID") as GridBoundColumn).ReadOnly = true;
            (grid.MasterTableView.GetColumn("SPECIALTY_TYPE_NAME") as GridBoundColumn).ReadOnly = true;
            (grid.MasterTableView.GetColumn("END_DATE") as GridTemplateColumn).ReadOnly = true;
        }
        else if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) ||
                   Helper.IsUserInScreeningRole(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString()))
        {
            ((GridCheckBoxColumn)rgEPDSpecialties.Columns.FindByUniqueName("chkSpecialtyApproved")).ReadOnly = true;
            (grid.MasterTableView.GetColumn("START_DATE") as GridBoundColumn).ReadOnly = true;
            (grid.MasterTableView.GetColumn("MMIS_SPECIALTY_TYPE_ID") as GridBoundColumn).ReadOnly = true;
            (grid.MasterTableView.GetColumn("SPECIALTY_TYPE_NAME") as GridBoundColumn).ReadOnly = true;
            (grid.MasterTableView.GetColumn("END_DATE") as GridTemplateColumn).ReadOnly = true;
        }
        else
        {
            ((GridCheckBoxColumn)rgEPDSpecialties.Columns.FindByUniqueName("chkSpecialtyApproved")).ReadOnly = true;
            ((GridCheckBoxColumn)rgEPDSpecialties.Columns.FindByUniqueName("chkSpecialty")).ReadOnly = true;
            (grid.MasterTableView.GetColumn("START_DATE") as GridBoundColumn).ReadOnly = true;
            (grid.MasterTableView.GetColumn("MMIS_SPECIALTY_TYPE_ID") as GridBoundColumn).ReadOnly = true;
            (grid.MasterTableView.GetColumn("SPECIALTY_TYPE_NAME") as GridBoundColumn).ReadOnly = true;
            (grid.MasterTableView.GetColumn("END_DATE") as GridTemplateColumn).ReadOnly = true;
            grid.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
        }
    }

    protected void gridServiceType_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        GridEditableItem item = e.Item as GridEditableItem;
        TextBox txtEndDate = item.FindControl("txtEndDate") as TextBox;
        string startDate = (item["START_DATE"].Controls[0] as TextBox).Text;
        string endDate = "";
        bool providerEdit = false;
        bool ddsEdit = false;

        string OldEndDate = "";

        if (Helper.ConvertStrNullToString(item.SavedOldValues["END_DATE"]) != "")
        {
            DateTime dt = DateTime.Parse(item.SavedOldValues["END_DATE"].ToString());
            OldEndDate = string.Format("{0:MM/dd/yyyy}", dt); ;
        }
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

        if (txtEndDate != null && Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name.ToString()))
            endDate = txtEndDate.Text;
        else
            endDate = OldEndDate;

        int regSpecialtyID = Convert.ToInt32(item.GetDataKeyValue("REG_SPECIALTY_ID"));
        int regSpecialtyCategoryID = Convert.ToInt32(item.GetDataKeyValue("REG_SPECIALTY_CATEGORY_ID"));
        int approval = Convert.ToInt32(item.GetDataKeyValue("REG_SPECIALTY_APPROVAL_ID"));
        Dictionary<string, string> parms = new Dictionary<string, string>();

        Dictionary<string, object> newValues = new Dictionary<string, object>();
        item.ExtractValues(newValues);

        providerEdit = newValues.ContainsKey("SPECIALTY_SELECTED");
        ddsEdit = newValues.ContainsKey("IsOperatorApproved") || newValues.ContainsKey("IsReviewerApproved");
        if (approval > 0 && Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName.ToString())
            && regSpecialtyCategoryID > 0 && ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialty")).Selected == false)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "The specialty is previously approved by DDS and cannot be removed.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);

            e.Canceled = true;
            return;
        }

        string capacity = "";
        string OldCapacity = "";
        if (newValues.ContainsKey("CAPACITY"))
        {
            capacity = (newValues["CAPACITY"] == null) ? "" : newValues["CAPACITY"].ToString();
            OldCapacity = Helper.ConvertStrNullToString(item.SavedOldValues["CAPACITY"]);
        }


        if (providerEdit == false && (regSpecialtyCategoryID == 0 && ddsEdit) || (OldCapacity != capacity) || endDate != OldEndDate)
        {
            providerEdit = true;
        }

       


        CheckBox chk = (CheckBox)this.FindControl(grid.ID.Replace("rg", "chk")); //if the top checkbox is visible then use that
        Boolean headerCheckBoxVisible = false;

        if (chk.Visible)
            headerCheckBoxVisible = true;

        if (providerEdit)
        {
            bool specialtySelected = false;
            if (regSpecialtyCategoryID == 0 && ddsEdit) // happens when the DDS operator approves a service not selected by the provider
                specialtySelected = true;
            else
            {
                if (headerCheckBoxVisible)
                    specialtySelected = chk.Checked;
                else 
                    specialtySelected = Convert.ToBoolean(item.GetDataKeyValue("SPECIALTY_SELECTED").ToString());
            }

            if (!specialtySelected && !ddsEdit && regSpecialtyCategoryID <= 0)
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "The specialty is not selected.";
                val.ValidationGroup = "valProviderInfoHeader";
                this.Page.Validators.Add(val);

                e.Canceled = true;
                return;
            }



            // If this isn't the "Other" grid, get the capacity and validate it
            if (grid.ID != "rgOther")
            {

                int capacityInt = 0;

                if (headerCheckBoxVisible && int.TryParse(capacity, out capacityInt) && int.Parse(capacity) < 1 && int.Parse(capacity) > 999)
                {
                    Boolean isValid = ValidateGrid(grid);

                    //if (!isValid)
                    //{
                    //    CustomValidator val = new CustomValidator();
                    //    val.IsValid = false;
                    //    val.ErrorMessage = "A valid Capacity is required for at least one service ";
                    //    val.ValidationGroup = "valProviderInfoHeader";
                    //    this.Page.Validators.Add(val);
                    //    e.Canceled = true;
                    //    return;
                    //}
                }
                else
                {
                // Validate the capacity field
                if (specialtySelected)
                {

                    if (capacity.Trim() == string.Empty)
                    {
                        CustomValidator val = new CustomValidator();
                        val.IsValid = false;
                        val.ErrorMessage = "Capacity is required.";
                        val.ValidationGroup = "valProviderInfoHeader";
                        this.Page.Validators.Add(val);

                        e.Canceled = true;
                        return;
                    }
                    else
                    {
                        if (!int.TryParse(capacity, out capacityInt))
                        {
                            CustomValidator val = new CustomValidator();
                            val.IsValid = false;
                            val.ErrorMessage = "Capacity must be a number.";
                            val.ValidationGroup = "valProviderInfoHeader";
                            this.Page.Validators.Add(val);

                            e.Canceled = true;
                            return;
                        }
                        else if (int.Parse(capacity) < 1 || int.Parse(capacity) > 999)
                        {
                            CustomValidator val = new CustomValidator();
                            val.IsValid = false;
                            val.ErrorMessage = "Capacity must be between 1 - 999.";
                            val.ValidationGroup = "valProviderInfoHeader";
                            this.Page.Validators.Add(val);

                            e.Canceled = true;
                            return;
                        }
                    }
                }
                }

            }


            string specialtyTypeID = newValues["SPECIALTY_TYPE_ID"].ToString();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("START_DATE", (startDate == "") ? DateTime.Now.ToString() : startDate);
            parms.Add("END_DATE", (endDate == "") ? null : endDate);
            parms.Add("PRIMARY_FLAG", "0");
            parms.Add("SPECIALTY_TYPE_ID", specialtyTypeID);
            parms.Add("SPECIALTY_BOARD_CERTIFIED", "N");
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            // Add the REG_SPECIALTY record if it doesn't exist
            if (regSpecialtyID == 0)
            {
                if (specialtySelected)
                    regSpecialtyID = svc.InsertRegistrationDataTable("SPECIALTY", parms);
            }

            parms.Remove("REG_ID");
            parms.Remove("PRIMARY_FLAG");
            parms.Remove("SPECIALTY_TYPE_ID");
            parms.Remove("SPECIALTY_BOARD_CERTIFIED");

            int specialtyCategoryID = Convert.ToInt32(item.GetDataKeyValue("SPECIALTY_CATEGORY_ID"));

            // If this isn't the "Other" grid, get the capacity and validate it
            if (grid.ID != "rgOther")
                parms.Add("CAPACITY", capacity);

            parms.Add("REG_SPECIALTY_ID", regSpecialtyID.ToString());
            parms.Add("SPECIALTY_CATEGORY_ID", specialtyCategoryID.ToString());

            if (regSpecialtyCategoryID > 0)
            {
                if (specialtySelected)
                {
                    parms.Add("REG_SPECIALTY_CATEGORY_ID", regSpecialtyCategoryID.ToString());

                    svc.UpdateRegistrationDataTable("SPECIALTY_CATEGORY", parms);
                }
                else
                {
                    svc.DeleteRegistrationData("SPECIALTY_CATEGORY", "REG_SPECIALTY_CATEGORY_ID", regSpecialtyCategoryID);

                    // If no other entries in REG_SPECIALTY_CATEGORY, delete the REG_SPECIALTY record
                    parms.Clear();
                    parms.Add("REG_SPECIALTY_ID", regSpecialtyID.ToString());
                    parms.Add("SPECIALTY_CATEGORY_ID", specialtyCategoryID.ToString());
                    svc.DeleteRegistrationDataWithParams("SPECIALTYCustom", parms);
                }
            }
            else
            {
                if (specialtySelected)
                    regSpecialtyCategoryID = svc.InsertRegistrationDataTable("SPECIALTY_CATEGORY", parms);
            }
        }

        if (ddsEdit) // Being edited by the DDS worker
        {
            parms.Clear();
            parms.Add("REG_SPECIALTY_ID", regSpecialtyID.ToString());
            parms.Add("REG_SPECIALTY_CATEGORY_ID", regSpecialtyCategoryID.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            CheckBox chkOpertorApproved = item["chkSpecialtyOperatorApproved"].Controls[0] as CheckBox;
            CheckBox chkReviewerApproved = item["chkSpecialtyReviewerApproved"].Controls[0] as CheckBox;

            bool specialtyOpertorApproved = chkOpertorApproved.Checked;
            bool specialtyReviewerApproved = chkReviewerApproved.Checked;

            if (approval > 0)
            {
                Dictionary<string, string> parms1 = new Dictionary<string, string>();

                parms1.Add("IsOperatorApproved", specialtyOpertorApproved.ToString());
                parms1.Add("IsReviewerApproved", specialtyReviewerApproved.ToString());

                parms1.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms1.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms1.Add("REG_SPECIALTY_APPROVAL_ID", approval.ToString());
                parms1.Add("REG_SPECIALTY_CATEGORY_ID", regSpecialtyCategoryID.ToString());
                parms1.Add("REG_SPECIALTY_ID", regSpecialtyID.ToString());

                svc.UpdateRegistrationDataTable("SPECIALTY_APPROVAL", parms1);                 
            }
            else
            {
                if (specialtyOpertorApproved || specialtyReviewerApproved)
                {
                    parms.Add("IsOperatorApproved", specialtyOpertorApproved.ToString());
                    parms.Add("IsReviewerApproved", specialtyReviewerApproved.ToString());
                    svc.InsertRegistrationDataTable("SPECIALTY_APPROVAL", parms);
            }
        }
    }
    }

    protected void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = GetGridDataSource((RadGrid)sender);
        if (Helper.HasRows(ds))
            ((RadGrid)sender).DataSource = ds;
        else
            ((RadGrid)sender).DataSource = null;
    }

    
    protected void grid_PreRender(object sender, EventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        //this is where we enable and disable the grid and show the update and cancel buttons
        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName.ToString())
                      || Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name.ToString())
                      || Helper.IsUserInDDSReviewerRole(HttpContext.Current.User.Identity.Name.ToString())
                     )
        {
            for (int i = 0; i < grid.PageSize; i++)
            {
                grid.EditIndexes.Add(i);
        }
            grid.Rebind();
        }
        else
            grid.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
    }

    

    protected void gridServiceType_DataBound(object sender, EventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        (grid.MasterTableView.GetColumn("START_DATE") as GridBoundColumn).ReadOnly = true;
        if (grid.MasterTableView.Columns.FindByUniqueName("MMIS_SPECIALTY_TYPE_ID") != null)
            (grid.MasterTableView.GetColumn("MMIS_SPECIALTY_TYPE_ID") as GridBoundColumn).ReadOnly = true;

        (grid.MasterTableView.GetColumn("END_DATE") as GridTemplateColumn).ReadOnly = true;
        
        if (Helper.IsUserInLTCWorkerRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInDDSReviewerRole(HttpContext.Current.User.Identity.Name))
        {
            ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialty")).ReadOnly = true;

            if (Helper.IsUserInDDSReviewerRole(HttpContext.Current.User.Identity.Name))
            {
                ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialtyReviewerApproved")).ReadOnly = false;
                ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialtyOperatorApproved")).ReadOnly = true;
        }
            else if (Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name))
        {
                ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialtyReviewerApproved")).ReadOnly = true;
                ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialtyOperatorApproved")).ReadOnly = false;

                if (this.WorkflowPage.WorkflowEventTypeId != CON.WorkflowEventType.NewReg)
                    (grid.MasterTableView.GetColumn("END_DATE") as GridTemplateColumn).ReadOnly = false;

            }
        }
        else if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) ||
                   Helper.IsUserInScreeningRole(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString()))
        {
            ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialtyOperatorApproved")).ReadOnly = true;
            if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
                ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialtyReviewerApproved")).Display = false;
        }

        ShowHeaderCheckboxBySpecialtyTypeID(grid);
    }

    protected void gridEmployeeType_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        GridEditableItem item = e.Item as GridEditableItem;
        TextBox txtEndDate = item.FindControl("txtEndDate") as TextBox;
        string startDate = (item["START_DATE"].Controls[0] as TextBox).Text;
        string endDate = "";
        bool providerEdit = false;
        bool ddsEdit = false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

        string OldEndDate = "";

        if (Helper.ConvertStrNullToString(item.SavedOldValues["END_DATE"]) != "")
        {
            DateTime dt = DateTime.Parse(item.SavedOldValues["END_DATE"].ToString());
            OldEndDate = string.Format("{0:MM/dd/yyyy}", dt); ;
        }

        if (txtEndDate != null && Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name.ToString()))
            endDate = txtEndDate.Text;
        else
            endDate = OldEndDate;

        int regSpecialtyID = Convert.ToInt32(item.GetDataKeyValue("REG_SPECIALTY_ID"));
        int regSpecialtyEmployeeID = Convert.ToInt32(item.GetDataKeyValue("REG_SPECIALTY_EMPLOYEE_ID"));
        int approval = Convert.ToInt32(item.GetDataKeyValue("REG_SPECIALTY_APPROVAL_ID"));
        Dictionary<string, string> parms = new Dictionary<string, string>();

        Dictionary<string, object> newValues = new Dictionary<string, object>();
        item.ExtractValues(newValues);

        providerEdit = newValues.ContainsKey("SPECIALTY_SELECTED");
        ddsEdit = newValues.ContainsKey("IsOperatorApproved") || newValues.ContainsKey("IsReviewerApproved");
        int specialtyEmployeeTypeID = Convert.ToInt32(item.GetDataKeyValue("SPECIALTY_EMPLOYEE_TYPE_ID"));

        string capacity = "";
         string OldCapacity = "";
        if (newValues.ContainsKey("CAPACITY"))
        {
            capacity = (newValues["CAPACITY"] == null) ? "" : newValues["CAPACITY"].ToString();
            OldCapacity = Helper.ConvertStrNullToString(item.SavedOldValues["CAPACITY"]);
        }
       

        if (providerEdit == false && (regSpecialtyEmployeeID == 0 && ddsEdit) || (OldCapacity != capacity) || (endDate != OldEndDate))
        {
            providerEdit = true;
        }


        CheckBox chk = (CheckBox)this.FindControl(grid.ID.Replace("rg", "chk")); //if the top checkbox is visible then use that
        Boolean headerCheckBoxVisible = false;

        if (chk!= null)
        if (chk.Visible)
            headerCheckBoxVisible = true;


        // Validate the capacity field
        
            bool specialtySelected = false;
            if (regSpecialtyEmployeeID == 0 && ddsEdit) // happens when the DDS operator approves a service not selected by the provider
                specialtySelected = true;
            else
            {

                if (headerCheckBoxVisible)
                    specialtySelected = chk.Checked;
                else
                    specialtySelected = Convert.ToBoolean(item.GetDataKeyValue("SPECIALTY_SELECTED").ToString());
            }

            string specialtyTypeID = newValues["SPECIALTY_TYPE_ID"].ToString();
            if (approval > 0 && Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName.ToString())
                && regSpecialtyEmployeeID > 0 && specialtySelected == false)
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "The specialty is previously approved by DDS and cannot be removed.";
                val.ValidationGroup = "valProviderInfoHeader";
                this.Page.Validators.Add(val);

                e.Canceled = true;
                return;
            }
            if (!specialtySelected && !ddsEdit  && regSpecialtyEmployeeID <=0)
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "The specialty is not selected.";
                val.ValidationGroup = "valProviderInfoHeader";
                this.Page.Validators.Add(val);

                e.Canceled = true;
                return;
            }

                int capacityInt = 0;


            if (headerCheckBoxVisible && int.TryParse(capacity, out capacityInt) && int.Parse(capacity) < 1 && int.Parse(capacity) > 999) 
             
            {
                Boolean isValid = ValidateGrid(grid);

                //if (!isValid)
                //{
                //    CustomValidator val = new CustomValidator();
                //    val.IsValid = false;
                //    val.ErrorMessage = "Capacity is required for at least one service ";
                //    val.ValidationGroup = "valProviderInfoHeader";
                //    this.Page.Validators.Add(val);
                //    e.Canceled = true;
                //    return;
                //}
            }
            else
            {
                if (specialtySelected)
                {

                if (capacity.Trim() == string.Empty)
                {
                    CustomValidator val = new CustomValidator();
                    val.IsValid = false;
                    val.ErrorMessage = "Capacity is required.";
                    val.ValidationGroup = "valProviderInfoHeader";
                    this.Page.Validators.Add(val);

                    e.Canceled = true;
                    return;
                }
                else
                {
                    if (!int.TryParse(capacity, out capacityInt))
                    {
                        CustomValidator val = new CustomValidator();
                        val.IsValid = false;
                        val.ErrorMessage = "Capacity must be a number.";
                        val.ValidationGroup = "valProviderInfoHeader";
                        this.Page.Validators.Add(val);

                        e.Canceled = true;
                        return;
                    }
                    else if (int.Parse(capacity) < 1 || int.Parse(capacity) > 999)
                    {
                        CustomValidator val = new CustomValidator();
                        val.IsValid = false;
                        val.ErrorMessage = "Capacity must be between 1 - 999.";
                        val.ValidationGroup = "valProviderInfoHeader";
                        this.Page.Validators.Add(val);

                        e.Canceled = true;
                        return;
                    }
                }
            }
            }
            if (providerEdit)
            {
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("START_DATE", (startDate == "") ? DateTime.Now.ToString() : startDate);
            parms.Add("END_DATE", (endDate == "") ? null : endDate);
            parms.Add("PRIMARY_FLAG", "0");
            parms.Add("SPECIALTY_TYPE_ID", specialtyTypeID);
            parms.Add("SPECIALTY_BOARD_CERTIFIED", "N");
            parms.Add("CAPACITY", capacity);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            if (regSpecialtyID <= 0)
            {
                if (specialtySelected)
                    regSpecialtyID = svc.InsertRegistrationDataTable("SPECIALTY", parms);
            }

            // Clear the REG_SPECIALTY specific parameters
            parms.Remove("REG_ID");
            parms.Remove("PRIMARY_FLAG");
            parms.Remove("SPECIALTY_TYPE_ID");
            parms.Remove("SPECIALTY_BOARD_CERTIFIED");

            // Insert/Update the REG_SPECIALTY_EMPLOYEE_ID record
            parms.Add("REG_SPECIALTY_ID", regSpecialtyID.ToString());
            parms.Add("SPECIALTY_EMPLOYEE_TYPE_ID", specialtyEmployeeTypeID.ToString());
            if (regSpecialtyEmployeeID > 0)
            {
                parms.Add("REG_SPECIALTY_EMPLOYEE_ID", regSpecialtyEmployeeID.ToString());
                if (specialtySelected)
                    svc.UpdateRegistrationDataTable("SPECIALTY_EMPLOYEE", parms);
                else
                {
                    svc.DeleteRegistrationData("SPECIALTY_EMPLOYEE", "REG_SPECIALTY_EMPLOYEE_ID", regSpecialtyEmployeeID);

                    // If no other entries in REG_SPECIALTY_EMPLOYEE, delete the REG_SPECIALTY record
                    parms.Clear();
                    parms.Add("REG_SPECIALTY_ID", regSpecialtyID.ToString());
                    parms.Add("SPECIALTY_EMPLOYEE_TYPE_ID", specialtyEmployeeTypeID.ToString());
                    svc.DeleteRegistrationDataWithParams("SPECIALTYCustom", parms);
                }
            }
            else
            {
                if (specialtySelected)
                    regSpecialtyEmployeeID = svc.InsertRegistrationDataTable("SPECIALTY_EMPLOYEE", parms);
            }
        }

        if (ddsEdit) // Being edited by the DDS worker
        {
            parms.Clear();
            parms.Add("REG_SPECIALTY_ID", regSpecialtyID.ToString());
            parms.Add("REG_SPECIALTY_EMPLOYEE_ID", regSpecialtyEmployeeID.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            CheckBox chkOpertorApproved = item["chkSpecialtyOperatorApproved"].Controls[0] as CheckBox;
            CheckBox chkReviewerApproved = item["chkSpecialtyReviewerApproved"].Controls[0] as CheckBox;

            bool specialtyOpertorApproved = chkOpertorApproved.Checked;
            bool specialtyReviewerApproved = chkReviewerApproved.Checked;

            if (approval > 0)
            {
                Dictionary<string, string> parms1 = new Dictionary<string, string>();

                parms1.Add("IsOperatorApproved", specialtyOpertorApproved.ToString());
                parms1.Add("IsReviewerApproved", specialtyReviewerApproved.ToString());

                parms1.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms1.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms1.Add("REG_SPECIALTY_APPROVAL_ID", approval.ToString());
                parms1.Add("REG_SPECIALTY_EMPLOYEE_ID", regSpecialtyEmployeeID.ToString());
                parms1.Add("REG_SPECIALTY_ID", regSpecialtyID.ToString());

                svc.UpdateRegistrationDataTable("SPECIALTY_APPROVAL", parms1);
                 
            }
            else
            {
                if (specialtyOpertorApproved || specialtyReviewerApproved)
                {
                    parms.Add("IsOperatorApproved", specialtyOpertorApproved.ToString());
                    parms.Add("IsReviewerApproved", specialtyReviewerApproved.ToString());
                    svc.InsertRegistrationDataTable("SPECIALTY_APPROVAL", parms);
            }
        }
    }
    }
 

  

    

    protected void gridEmployeeType_DataBound(object sender, EventArgs e)
    {
        RadGrid grid = (RadGrid)sender;

            (grid.MasterTableView.GetColumn("START_DATE") as GridBoundColumn).ReadOnly = true;
            (grid.MasterTableView.GetColumn("MMIS_SPECIALTY_TYPE_ID") as GridBoundColumn).ReadOnly = true;
            (grid.MasterTableView.GetColumn("EMPLOYEE_TYPE_NAME") as GridBoundColumn).ReadOnly = true;

                (grid.MasterTableView.GetColumn("END_DATE") as GridTemplateColumn).ReadOnly = true;


        if (Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInDDSReviewerRole(HttpContext.Current.User.Identity.Name))
        {
            ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialty")).ReadOnly = true;
            if (Helper.IsUserInDDSReviewerRole(HttpContext.Current.User.Identity.Name))
            {
                ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialtyReviewerApproved")).ReadOnly = false;
                ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialtyOperatorApproved")).ReadOnly = true;
            }
            else if (Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name))
            {
                ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialtyReviewerApproved")).ReadOnly = true;
                ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialtyOperatorApproved")).ReadOnly = false;

                if (this.WorkflowPage.WorkflowEventTypeId != CON.WorkflowEventType.NewReg)
                (grid.MasterTableView.GetColumn("END_DATE") as GridTemplateColumn).ReadOnly = false;
            }
        }
        else if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) ||
                   Helper.IsUserInScreeningRole(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString()))
        {
            ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialtyOperatorApproved")).ReadOnly = true;
            if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
            ((GridCheckBoxColumn)grid.Columns.FindByUniqueName("chkSpecialtyReviewerApproved")).Display=false;
        } 
        
        ShowHeaderCheckboxBySpecialtyTypeID(grid);
    }

    private void ShowHeaderCheckboxBySpecialtyTypeID(RadGrid grid)
    {
        CheckBox chk = (CheckBox)this.FindControl(grid.ID.Replace("rg", "chk"));
        PopupControls_Separator sepHeader = (PopupControls_Separator)this.FindControl(grid.ID.Replace("rg", "sep"));
        if (CheckSpecialtyTypeIdSameForAllRows(grid))
        {
            grid.Columns.FindByUniqueName("MMIS_SPECIALTY_TYPE_ID").Display = false;
            grid.Columns.FindByUniqueName("chkSpecialty").Display = false;


            if (chk != null)
            {
                chk.Visible = true;
                chk.Checked = false;
                chk.Attributes.Add("value", "");
                foreach (GridDataItem item in grid.Items) //if any of the services are selected then checkbox is checked
                {
                    if (item.GetDataKeyValue("SPECIALTY_SELECTED").ToString() == "true")
                    {
                        chk.Checked = true;
                        chk.Attributes.Add("value", "true");
                        break;
                    }
                }
            }

            string sTypeId = grid.Items[0]["MMIS_SPECIALTY_TYPE_ID"].Text.ToString();

            if (sepHeader != null && !sepHeader.Header.Contains(sTypeId) && sTypeId != "&nbsp;")
                sepHeader.Header += " (" + sTypeId + ")";
        }
        else
        {
            grid.Columns.FindByUniqueName("MMIS_SPECIALTY_TYPE_ID").Display = true;
            grid.Columns.FindByUniqueName("chkSpecialty").Display = true;

            if (chk != null)
                chk.Visible = false;
        }
    }

    private Boolean CheckSpecialtyTypeIdSameForAllRows(RadGrid grid)
    {
        DataSet ds = (DataSet) grid.DataSource;

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            string first_SPECIALTY_TYPE_ID = ds.Tables[0].Rows[0]["MMIS_SPECIALTY_TYPE_ID"].ToString();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (first_SPECIALTY_TYPE_ID != dr["MMIS_SPECIALTY_TYPE_ID"].ToString())
                    return false;
        }
    }
        return true;
    }


    private void LoadDDSData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DDS");

        if (Helper.HasRows(ds))
        {
            DataRow row = ds.Tables[0].Rows[0];
            nbFootage.Text = Helper.GetString("FACILITY_FOOTAGE", row);
            nbHostHomes.Text = Helper.GetString("NUM_HOMES", row);
            nbPersonalVehicles.Text = Helper.GetString("NUM_PERSONAL_VEHICLE", row);
            chkPersonalVehicles.Checked = (nbPersonalVehicles.Text.Trim() != "0");
            nbAgencyCar.Text = Helper.GetString("NUM_AGENCY_CAR", row);
            chkAgencyCar.Checked = (nbAgencyCar.Text.Trim() != "0");
            nbAgencyMinivan.Text = Helper.GetString("NUM_AGENCY_MINIVAN", row);
            chkAgencyMinivan.Checked = (nbAgencyMinivan.Text.Trim() != "0");
            nbAgencySmallVan.Text = Helper.GetString("NUM_AGENCY_SMALLVAN", row);
            chkAgencySmallVan.Checked = (nbAgencySmallVan.Text.Trim() != "0");
            nbAgencyOther.Text = Helper.GetString("NUM_AGENCY_VEHICLE_OTHER", row);
            chkAgencyOther.Checked = (nbAgencyOther.Text.Trim() != "0");
            txtAgencyOtherDesc.Text = Helper.GetString("AGENCY_VEHICLE_OTHER_DESC", row);
        }

    }

    private void LoadGrid(RadGrid grid)
    {
        grid.Rebind();
        //DataSet ds = GetGridDataSource(grid);

        //grid.DataSource = ds.Tables[0];
    }

    private DataSet GetGridDataSource(RadGrid grid)
    {
        DataSet ds = GetGridDataSource(grid.ID);
        return ds;
    }

    private DataSet GetGridDataSource(string gridID)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        DataSet ds = new DataSet();

        switch (gridID)
        {
            case "rgEPDSpecialties":
                ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTYServices");
                break;
            case "rgArtTherapySpecialties":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "1");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgBehavioralSupportSpecialties":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "2");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgDayHabilitationSpecialties":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "18");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgEmploymentReadiness":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "3");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgEnvAccessibilities":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "4");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgFamilyTraining":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "5");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgHostHome":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "6");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgIndividualizedDay":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "7");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgInHomeSupports":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "8");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgPERS":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "9");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgRespite":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "10");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgResHabilitation":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "11");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgSkilledNursing":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "12");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgSpeechHearing":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "13");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgSupportEmployment":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "14");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgSupportedLiving":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "15");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgWellness":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "16");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgOther":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "17");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            case "rgOtherCapacity":
                parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parameters.Add("SPECIALTY_CATEGORY_ID", "19");
                ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYServices_DDS", parameters);
                break;
            default:
                break;
        }

        return ds;
    }


    protected void rgEPDSpecialties_PreRender(object sender, EventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        for (int i = 0; i < grid.PageSize; i++)
        {
            grid.EditIndexes.Add(i);
        }
        grid.Rebind();
    }

    protected void rgEPDSpecialties_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            GridEditableItem editItem = e.Item as GridEditableItem;
            int rowIndex = editItem.ItemIndex;
            //(editItem["CAPACITY"].Controls[0] as TextBox).Attributes.Add("onkeypress", "javascript:textBoxValueChanged(" + rowIndex + ");");
            (editItem["chkSpecialtyApproved"].Controls[0] as CheckBox).Attributes.Add("onchange", "javascript:textBoxValueChanged(" + rowIndex + ");");
            (editItem["chkSpecialty"].Controls[0] as CheckBox).Attributes.Add("onchange", "javascript:textBoxValueChanged(" + rowIndex + ");");
            GridTableCell editItemButton = ((GridTableCell)editItem["EditCommandColumn"]);
            //for(int i = 0; i < editItemButton.Controls.Count; i++)
            //{
            ((Button)editItemButton.Controls[0]).Enabled = false;
            //((Literal)editItemButton.Controls[1]).Enabled = false;
            ((Button)editItemButton.Controls[2]).Enabled = false;
            Button updateButton = (Button)e.Item.FindControl("UpdateButton");
            updateButton.Style["margin-left"] = "3px";

            Button cancelButton = (Button)e.Item.FindControl("CancelButton");
            cancelButton.Style["margin-top"] = "2px";

            updateButton.Attributes.Add("onclick", "if(Page_ClientValidate('" + updateButton.ValidationGroup +
   "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(updateButton, null) + ";");
            //}
    }
    }


    protected void grid_ItemCreated(object sender, GridItemEventArgs e)
    {

        RadGrid grid = ((RadGrid)sender);

        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
    {
            GridEditableItem editItem = e.Item as GridEditableItem;
            int rowIndex = editItem.ItemIndex;

            if (grid.MasterTableView.Columns.FindByUniqueNameSafe("CAPACITY") != null)
            {
                TextBox TxtCapacity = editItem["CAPACITY"].Controls[0] as TextBox;

                (TxtCapacity).Attributes.Add("onkeypress", "javascript:RowValueChanged('" + grid.ClientID + "'," + rowIndex + ");");
            }

            if (grid.MasterTableView.Columns.FindByUniqueNameSafe("chkSpecialtyApproved") != null)
                (editItem["chkSpecialtyApproved"].Controls[0] as CheckBox).Attributes.Add("onchange", "javascript:RowValueChanged('" + grid.ClientID + "'," + rowIndex + ");");

            if (grid.MasterTableView.Columns.FindByUniqueNameSafe("chkSpecialtyReviewerApproved") != null)
                (editItem["chkSpecialtyReviewerApproved"].Controls[0] as CheckBox).Attributes.Add("onchange", "javascript:RowValueChanged('" + grid.ClientID + "'," + rowIndex + ");");

            if (grid.MasterTableView.Columns.FindByUniqueNameSafe("chkSpecialtyOperatorApproved") != null)
                (editItem["chkSpecialtyOperatorApproved"].Controls[0] as CheckBox).Attributes.Add("onchange", "javascript:RowValueChanged('" + grid.ClientID + "'," + rowIndex + ");");


            if (grid.MasterTableView.Columns.FindByUniqueNameSafe("END_DATE") != null && (grid.MasterTableView.GetColumn("END_DATE") as GridTemplateColumn).ReadOnly == false)
                (editItem.FindControl("txtEndDate") as TextBox).Attributes.Add("onchange", "javascript:RowValueChanged('" + grid.ClientID + "'," + rowIndex + ");");

            (editItem["chkSpecialty"].Controls[0] as CheckBox).Attributes.Add("onchange", "javascript:RowValueChanged('" + grid.ClientID + "'," + rowIndex + ");");

            
            GridTableCell editItemButton = ((GridTableCell)editItem["EditCommandColumn"]);
            
            ((Button)editItemButton.Controls[2]).Enabled = false;
 
            ((Button)editItemButton.Controls[0]).Enabled = false;

            Button updateButton = (Button)e.Item.FindControl("UpdateButton");
            updateButton.Style["margin-left"] = "3px";

            Button cancelButton = (Button)e.Item.FindControl("CancelButton");
            cancelButton.Style["margin-top"] = "2px";

            updateButton.Attributes.Add("onclick", "if(Page_ClientValidate('" + updateButton.ValidationGroup +
   "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(updateButton, null) + ";");
            //}
    }
    }


    public override string Title
    {
        get { return "Waiver Services Information"; }
    }

    public override string IdText
    {
        get { return "ucWaiverServices_" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
        get { return "valWaiverServices"; }
    }
}