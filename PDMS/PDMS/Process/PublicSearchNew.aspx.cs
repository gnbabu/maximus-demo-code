using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using MAXIMUS.Core.Libraries;

public partial class Process_PublicSearchNew : System.Web.UI.Page
{
    private static string sortColWithDirection = string.Empty;
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
            Page.Theme = "Modernization";
        else
            Page.Theme = "Default";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                this.Page.Title = (string)GetGlobalResourceObject("BrandingResource", "PortalName") + " - Provider Search(Public)";
                BindDropDown();
                sortColWithDirection = "OrganizationName";
            }
            catch (Exception ex)
            {
                MessageBox2.Show("An error occurred when loading the work queue.<br>Error: " + ex.Message + " " + ex.StackTrace, "Error");
            }
        }
        ddlState.Attributes.Add("onchange", "handleStateChange();");
        
        string settingValue = AppSettings.Get("EnableCR551");
        bool isEnabled = false;

        if (!string.IsNullOrEmpty(settingValue))
        {
            bool.TryParse(settingValue, out isEnabled);
        }

        lnkAPI.Visible = isEnabled;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.gvProviders.CurrentPageIndex = 0;
        RefreshData();
        lnkExcel.Visible = lnkPDF.Visible = gvProviders.VirtualItemCount > 0;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        loadAllProviderSpeciality(psc);

        ddlHealthPlan.SelectedIndex = -1;
        ddlProgram.SelectedIndex = -1;
        rcbProviderType.ClearCheckedItems();
        ddlFacilityType.SelectedIndex = -1;
        ddlPrimaryCareProviders.SelectedIndex = -1;
        ddlOrgNameSearch.SelectedIndex = -1;
        txtOrgName.Text = "";
        rcbDMEProductsServices.ClearCheckedItems();
        txtAcceptsPatientsAsYoungAs.Text = "";
        txtAcceptsPatientsAsOldAs.Text = "";
        ddlAcceptsPatientsofGender.SelectedIndex = -1;
        ddlAcceptsNewPatients.SelectedIndex = -1;
        ddlAcceptsNewborns.SelectedIndex = -1;
        ddlAcceptsPregnantWomen.SelectedIndex = -1;
        rcbCounty.ClearCheckedItems();
        ddlCitySearch.SelectedIndex = -1;
        txtCity.Text = "";
        ddlState.SelectedIndex = -1;
        txtZip.Text = "";
        ddlRadiusMiles.SelectedIndex = 1;
        rcbProviderSpeciality.ClearCheckedItems();
        ddlProviderGender.SelectedIndex = -1;
        rcbHospitalAffiliation.ClearCheckedItems();
        rcbLanguagesSpoken.ClearCheckedItems();
        rcbSpecializedTraining.ClearCheckedItems();
        rcbCulturalCompetencies.SelectedIndex = -1;
        rcbADAAccommodations.ClearCheckedItems();
        rcbBoardCertifications.ClearCheckedItems();
        rcbCulturalCompetencies.ClearCheckedItems();
        ddlTelehealth.SelectedIndex = -1;
        ddlCHIP.SelectedIndex = -1;
        ddlNewMedicaid.SelectedIndex = -1;

        gvProviders.DataSource = null;
        gvProviders.DataBind();
        lnkExcel.Visible = lnkPDF.Visible = false;
        gvProviders.Visible = false;
    }

    protected void gvProviders_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        sortColWithDirection = String.Empty;
        string sortOrder = e.NewSortOrder == Telerik.Web.UI.GridSortOrder.Descending ? "DESC" : "ASC";
        sortColWithDirection = e.SortExpression.ToString() + ' ' + sortOrder;
        RefreshData();
    }

    public void RefreshData()
    {
        int totalResultCount = 0;
        int rowIndex = 0;
        DataTable dt = GetData(out totalResultCount, gvProviders.PageSize, rowIndex);
        hdnRowCount.Value = totalResultCount.ToString();
        gvProviders.DataSource = dt;
        gvProviders.VirtualItemCount = totalResultCount;
        gvProviders.DataBind();
        gvProviders.Visible = true;
    }

    private DataTable GetData(out int totalResultCount, int pageSize, int rowIndex)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        totalResultCount = 0;

        // Provider Type Multi Select Drop Down
        var providerTypeCollection = rcbProviderType.CheckedItems;
        var providerTypeSelectedValues = string.Empty;
        if (providerTypeCollection.Count != 0)
        {
            foreach (var item in providerTypeCollection)
            {
                providerTypeSelectedValues = providerTypeSelectedValues + item.Value + ",";
            }
            providerTypeSelectedValues = providerTypeSelectedValues.Remove(providerTypeSelectedValues.LastIndexOf(","));
        }

        // DME Products Services Multi Select Drop Down
        var dmeProductsServicesCollection = rcbDMEProductsServices.CheckedItems;
        var dmeProductsServicesSelectedValues = string.Empty;
        if (dmeProductsServicesCollection.Count != 0)
        {
            foreach (var item in dmeProductsServicesCollection)
            {
                dmeProductsServicesSelectedValues = dmeProductsServicesSelectedValues + item.Value + ",";
            }
            dmeProductsServicesSelectedValues = dmeProductsServicesSelectedValues.Remove(dmeProductsServicesSelectedValues.LastIndexOf(","));
        }

        // Specialty Type Multi Select Drop Down
        var specialtyTypeCollection = rcbProviderSpeciality.CheckedItems;
        var specialtyTypeSelectedValues = string.Empty;
        if (specialtyTypeCollection.Count != 0)
        {
            foreach (var item in specialtyTypeCollection)
            {
                specialtyTypeSelectedValues = specialtyTypeSelectedValues + item.Value + ",";
            }
            specialtyTypeSelectedValues = specialtyTypeSelectedValues.Remove(specialtyTypeSelectedValues.LastIndexOf(","));
        }

        // County Multi Select Drop Down
        var countyCollection = rcbCounty.CheckedItems;
        var countySelectedValues = string.Empty;
        if (countyCollection.Count != 0)
        {
            foreach (var item in countyCollection)
            {
                countySelectedValues = countySelectedValues + item.Value + ",";
            }
            countySelectedValues = countySelectedValues.Remove(countySelectedValues.LastIndexOf(","));
        }

        string city = txtCity.Text.Trim();
        string orgName = txtOrgName.Text.Trim();

        // Languages Spoken Multi Select Drop Down
        var languagesSpokenCollection = rcbLanguagesSpoken.CheckedItems;
        var languagesSpokenSelectedValues = string.Empty;
        if (languagesSpokenCollection.Count != 0)
        {
            foreach (var item in languagesSpokenCollection)
            {
                languagesSpokenSelectedValues = languagesSpokenSelectedValues + item.Value + ",";
            }
            languagesSpokenSelectedValues = languagesSpokenSelectedValues.Remove(languagesSpokenSelectedValues.LastIndexOf(","));
        }

        // Hospital Affiliations Multi Select Drop Down
        var hospitalAffiliationCollection = rcbHospitalAffiliation.CheckedItems;
        var hospitalAffiliationSelectedValues = string.Empty;
        if (hospitalAffiliationCollection.Count != 0)
        {
            foreach (var item in hospitalAffiliationCollection)
            {
                hospitalAffiliationSelectedValues = hospitalAffiliationSelectedValues + item.Value + ",";
            }
            hospitalAffiliationSelectedValues = hospitalAffiliationSelectedValues.Remove(hospitalAffiliationSelectedValues.LastIndexOf(","));
        }

        // Specialized Training Multi Select Drop Down
        var specializedTrainingCollection = rcbSpecializedTraining.CheckedItems;
        var specializedTrainingSelectedValues = string.Empty;
        if (specializedTrainingCollection.Count != 0)
        {
            foreach (var item in specializedTrainingCollection)
            {
                specializedTrainingSelectedValues = specializedTrainingSelectedValues + item.Value + ",";
            }
            specializedTrainingSelectedValues = specializedTrainingSelectedValues.Remove(specializedTrainingSelectedValues.LastIndexOf(","));
        }

        // Cultural Competencies Multi Select Drop Down
        var culturalCompetenciesCollection = rcbCulturalCompetencies.CheckedItems;
        var culturalCompetenciesSelectedValues = string.Empty;
        if (culturalCompetenciesCollection.Count != 0)
        {
            foreach (var item in culturalCompetenciesCollection)
            {
                culturalCompetenciesSelectedValues = culturalCompetenciesSelectedValues + item.Value + ",";
            }
            culturalCompetenciesSelectedValues = culturalCompetenciesSelectedValues.Remove(culturalCompetenciesSelectedValues.LastIndexOf(","));
        }

        // ADA Accommodations Multi Select Drop Down
        var adaAccommodationsCollection = rcbADAAccommodations.CheckedItems;
        var adaAccommodationsSelectedValues = string.Empty;
        if (adaAccommodationsCollection.Count != 0)
        {
            foreach (var item in adaAccommodationsCollection)
            {
                adaAccommodationsSelectedValues = adaAccommodationsSelectedValues + item.Value + ",";
            }
            adaAccommodationsSelectedValues = adaAccommodationsSelectedValues.Remove(adaAccommodationsSelectedValues.LastIndexOf(","));
        }


        // Board Certification Multi Select Drop Down
        var boardCertificationCollection = rcbBoardCertifications.CheckedItems;
        var boardCertificationSelectedValues = string.Empty;
        if (boardCertificationCollection.Count != 0)
        {
            foreach (var item in boardCertificationCollection)
            {
                boardCertificationSelectedValues = boardCertificationSelectedValues + item.Value + ",";
            }
            boardCertificationSelectedValues = boardCertificationSelectedValues.Remove(boardCertificationSelectedValues.LastIndexOf(","));
        }

        city = FormatLikeValue(ddlCitySearch.SelectedValue, city);
        orgName = FormatLikeValue(ddlOrgNameSearch.SelectedValue, orgName);

        // Create delimited list of provider type abbreviations

        // Create delimited list of specialty type IDs

        // If list of IDs has been passed in, display in search results list.
        Dictionary<string, object> parms = new Dictionary<string, object>();
        parms.Add("ProviderTypeIDsDelimited", providerTypeSelectedValues);
        parms.Add("SpecialtyTypeIDsDelimited", specialtyTypeSelectedValues);
        parms.Add("City", city);
        parms.Add("State", ddlState.SelectedValue);
        parms.Add("County", countySelectedValues);
        parms.Add("Zip", txtZip.Text.Trim());
        parms.Add("OrgName", orgName);
        parms.Add("HealthPlan", ddlHealthPlan.SelectedValue);
        parms.Add("Program", ddlProgram.SelectedValue);
        parms.Add("FacilityType", ddlFacilityType.SelectedValue);
        parms.Add("PrimaryCareProviders", ddlPrimaryCareProviders.SelectedValue);
        parms.Add("DMEProductsServices", dmeProductsServicesSelectedValues);
        parms.Add("AcceptsPatientsAsYoungAs", txtAcceptsPatientsAsYoungAs.Text.Trim());
        parms.Add("AcceptsPatientsAsOldAs", txtAcceptsPatientsAsOldAs.Text.Trim());
        parms.Add("AcceptsPatientsofGender", ddlAcceptsPatientsofGender.SelectedValue);
        parms.Add("AcceptsNewPatients", ddlAcceptsNewPatients.SelectedValue);
        parms.Add("AcceptsNewborns", ddlAcceptsNewborns.SelectedValue);
        parms.Add("AcceptsPregnantWomen", ddlAcceptsPregnantWomen.SelectedValue);
        parms.Add("RadiusMiles", ddlRadiusMiles.SelectedValue);
        parms.Add("ProviderGender", ddlProviderGender.SelectedValue);
        parms.Add("HospitalAffiliation", hospitalAffiliationSelectedValues);
        parms.Add("LanguagesSpokenList", languagesSpokenSelectedValues);
        parms.Add("SpecializedTraining", specializedTrainingSelectedValues);
        parms.Add("CulturalCompetencies", culturalCompetenciesSelectedValues);
        parms.Add("ADAAccommodations", adaAccommodationsSelectedValues);
        parms.Add("BoardCertifications", boardCertificationSelectedValues);
        parms.Add("Telehealth", ddlTelehealth.SelectedValue);
        parms.Add("CHIP", ddlCHIP.SelectedValue);
        parms.Add("NewMedicaid", ddlNewMedicaid.SelectedValue);


        parms.Add("SortExpression", sortColWithDirection);
        parms.Add("PageSize", pageSize);
        parms.Add("StartRowIndex", rowIndex);
        parms.Add("GetTotalResultCount", true);
        ds = psc.SearchProvidersPublicNew(parms, out totalResultCount);
        /*ds = psc.SearchProvidersPublic(providerTypeAbbrevList, specialtyTypeIDList, "", "", "", city, ddlState.SelectedValue, 
            txtZip.Text.Trim(), orgName,
            sortColWithDirection, pageSize, gvProviders.CurrentPageIndex, true, out totalResultCount);
        */
        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    protected string FormatLikeValue(string condition, string value)
    {
        if (value.Trim() != string.Empty)
        {
            switch (condition)
            {
                case "begins":
                    value = value + "%";
                    break;
                case "contains":
                    value = "%" + value + "%";
                    break;
                case "ends":
                    value = "%" + value;
                    break;
                case "equals":
                default:
                    break;
            }
        }

        return value;
    }

    protected void gvProviders_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName != "ReviewRow" && e.CommandName != "ExpressAdmin")
            return;

        int index = e.Item.ItemIndex;
        if (e.CommandName == "ReviewRow")
        {
            int regId = (int)this.gvProviders.Items[index].GetDataKeyValue("REG_ID");
            int regAddressId = (int)this.gvProviders.Items[index].GetDataKeyValue("REG_ADDRESS_ID");
            if (regId > 0)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet dsPublicProviderLblData =    psc.SelectPublicProviderLblDataByRegID(regId);
                DataSet dsPublicProviderLblLocData = psc.SelectPublicProviderLblLocDataByRegID(regId, regAddressId);
                if (Helper.HasRows(dsPublicProviderLblLocData))
                {
                    DataRow rowProvider = dsPublicProviderLblLocData.Tables[0].Rows[0];
                    lblProviderName.Text = Helper.GetString("NAME", rowProvider);
                    lblAddress.Text = string.Format("{0}", Helper.GetString("CONTACT_ADDRESS1", rowProvider));
                    lblCityStateZip.Text = string.Format("{0}, {1}, {2}", 
                        Helper.GetString("CONTACT_CITY", rowProvider), 
                        Helper.GetString("CONTACT_STATE", rowProvider), 
                        Helper.GetString("CONTACT_ZIP", rowProvider));
                    lblCounty.Text = Helper.GetString("CountyName", rowProvider);
                    lblAcceptingNewPatients.Text = Helper.GetString("ACCEPT_NEW_PATIENT", rowProvider);
                    lblOfficePhone.Text = Helper.GetString("CONTACT_PHONE_NUMBER", rowProvider);
                    lblGender.Text = Helper.GetString("GENDER", rowProvider);
                    long phoneNumberLong = 0;
                    if (long.TryParse(lblOfficePhone.Text, out phoneNumberLong))
                    {
                        lblOfficePhone.Text = string.Format("{0:###-###-####}", phoneNumberLong);
                    }
                    hlGoogleMaps.NavigateUrl = string.Format("http://maps.google.com/maps?f=q&hl=en&q={0},{1}", lblAddress.Text, lblCityStateZip.Text); 
                }
                if (Helper.HasRows(dsPublicProviderLblLocData))
                {
                    DataRow rowServiceLocation = dsPublicProviderLblLocData.Tables[0].Rows[0];

                    string monOfficeHours = "";
                    string tueOfficeHours = "";
                    string wedOfficeHours = "";
                    string thuOfficeHours = "";
                    string friOfficeHours = "";
                    string satOfficeHours = "";
                    string sunOfficeHours = "";

                    if (!string.IsNullOrEmpty(Helper.GetString("MON_START_TIME", rowServiceLocation)) || (!string.IsNullOrEmpty(Helper.GetString("MON_OPEN_24_HRS", rowServiceLocation)) && Helper.GetBool("MON_OPEN_24_HRS", rowServiceLocation)))
                    {

                        if (!string.IsNullOrEmpty(Helper.GetString("MON_OPEN_24_HRS", rowServiceLocation)) && Helper.GetBool("MON_OPEN_24_HRS", rowServiceLocation))
                        {
                            monOfficeHours = "Open 24 Hours";
                        }
                        else
                        {
                            monOfficeHours = Helper.GetString("MON_START_TIME", rowServiceLocation) + " - " + Helper.GetString("MON_END_TIME", rowServiceLocation);
                        }
                    }

                    if (!string.IsNullOrEmpty(Helper.GetString("TUE_START_TIME", rowServiceLocation)) || (!string.IsNullOrEmpty(Helper.GetString("TUE_OPEN_24_HRS", rowServiceLocation)) && Helper.GetBool("TUE_OPEN_24_HRS", rowServiceLocation)))
                    {

                        if (!string.IsNullOrEmpty(Helper.GetString("TUE_OPEN_24_HRS", rowServiceLocation)) && Helper.GetBool("TUE_OPEN_24_HRS", rowServiceLocation))
                        {
                            tueOfficeHours = "Open 24 Hours";
                        }
                        else
                        {
                            tueOfficeHours = Helper.GetString("TUE_START_TIME", rowServiceLocation) + " - " + Helper.GetString("TUE_END_TIME", rowServiceLocation);
                        }
                    }

                    if (!string.IsNullOrEmpty(Helper.GetString("WED_START_TIME", rowServiceLocation)) || (!string.IsNullOrEmpty(Helper.GetString("WED_OPEN_24_HRS", rowServiceLocation)) && Helper.GetBool("WED_OPEN_24_HRS", rowServiceLocation)))
                    {

                        if (!string.IsNullOrEmpty(Helper.GetString("WED_OPEN_24_HRS", rowServiceLocation)) && Helper.GetBool("WED_OPEN_24_HRS", rowServiceLocation))
                        {
                            wedOfficeHours = "Open 24 Hours";
                        }
                        else
                        {
                            wedOfficeHours = Helper.GetString("WED_START_TIME", rowServiceLocation) + " - " + Helper.GetString("WED_END_TIME", rowServiceLocation);
                        }
                    }

                    if (!string.IsNullOrEmpty(Helper.GetString("THU_START_TIME", rowServiceLocation)) || (!string.IsNullOrEmpty(Helper.GetString("THU_OPEN_24_HRS", rowServiceLocation)) && Helper.GetBool("THU_OPEN_24_HRS", rowServiceLocation)))
                    {

                        if (!string.IsNullOrEmpty(Helper.GetString("THU_OPEN_24_HRS", rowServiceLocation)) && Helper.GetBool("THU_OPEN_24_HRS", rowServiceLocation))
                        {
                            thuOfficeHours = "Open 24 Hours";
                        }
                        else
                        {
                            thuOfficeHours = Helper.GetString("THU_START_TIME", rowServiceLocation) + " - " + Helper.GetString("THU_END_TIME", rowServiceLocation);
                        }
                    }

                    if (!string.IsNullOrEmpty(Helper.GetString("FRI_START_TIME", rowServiceLocation)) || (!string.IsNullOrEmpty(Helper.GetString("FRI_OPEN_24_HRS", rowServiceLocation)) && Helper.GetBool("FRI_OPEN_24_HRS", rowServiceLocation)))
                    {

                        if (!string.IsNullOrEmpty(Helper.GetString("FRI_OPEN_24_HRS", rowServiceLocation)) && Helper.GetBool("FRI_OPEN_24_HRS", rowServiceLocation))
                        {
                            friOfficeHours = "Open 24 Hours";
                        }
                        else
                        {
                            friOfficeHours = Helper.GetString("FRI_START_TIME", rowServiceLocation) + " - " + Helper.GetString("FRI_END_TIME", rowServiceLocation);
                        }
                    }

                    if (!string.IsNullOrEmpty(Helper.GetString("SAT_START_TIME", rowServiceLocation)) || (!string.IsNullOrEmpty(Helper.GetString("SAT_OPEN_24_HRS", rowServiceLocation)) && Helper.GetBool("SAT_OPEN_24_HRS", rowServiceLocation)))
                    {

                        if (!string.IsNullOrEmpty(Helper.GetString("SAT_OPEN_24_HRS", rowServiceLocation)) && Helper.GetBool("SAT_OPEN_24_HRS", rowServiceLocation))
                        {
                            satOfficeHours = "Open 24 Hours";
                        }
                        else
                        {
                            satOfficeHours = Helper.GetString("SAT_START_TIME", rowServiceLocation) + " - " + Helper.GetString("SAT_END_TIME", rowServiceLocation);
                        }
                    }

                    if (!string.IsNullOrEmpty(Helper.GetString("SUN_START_TIME", rowServiceLocation)) || (!string.IsNullOrEmpty(Helper.GetString("SUN_OPEN_24_HRS", rowServiceLocation)) && Helper.GetBool("SUN_OPEN_24_HRS", rowServiceLocation)))
                    {

                        if (!string.IsNullOrEmpty(Helper.GetString("SUN_OPEN_24_HRS", rowServiceLocation)) && Helper.GetBool("SUN_OPEN_24_HRS", rowServiceLocation))
                        {
                            sunOfficeHours = "Open 24 Hours";
                        }
                        else
                        {
                            sunOfficeHours = Helper.GetString("SUN_START_TIME", rowServiceLocation) + " - " + Helper.GetString("SUN_END_TIME", rowServiceLocation);
                        }
                    }
                    lblOfficeMon.Text = monOfficeHours;
                    lblOfficeTue.Text = tueOfficeHours;
                    lblOfficeWed.Text = wedOfficeHours;
                    lblOfficeThu.Text = thuOfficeHours;
                    lblOfficeFri.Text = friOfficeHours;
                    lblOfficeSat.Text = satOfficeHours;
                    lblOfficeSun.Text = sunOfficeHours;

                    if (!String.IsNullOrEmpty(Helper.GetString("TELEHEALTH", rowServiceLocation)))
                    {
                        if (Helper.GetString("TELEHEALTH", rowServiceLocation).Equals("1") || Helper.GetString("TELEHEALTH", rowServiceLocation).ToLower().Equals("true"))
                        {
                            lblTelehealthLBL.Text = "Yes";
                        } else
                        {
                            lblTelehealthLBL.Text = "No";
                        }
                    } else
                    {
                        lblTelehealthLBL.Text = "Unknown";
                    }

                    if (!String.IsNullOrEmpty(Helper.GetString("CHIP", rowServiceLocation)))
                    {
                        if (Helper.GetString("CHIP", rowServiceLocation).Equals("1") || Helper.GetString("CHIP", rowServiceLocation).ToLower().Equals("true"))
                        {
                            lblCHIPLBL.Text = "Yes";
                        }
                        else
                        {
                            lblCHIPLBL.Text = "No";
                        }
                    }
                    else
                    {
                        lblCHIPLBL.Text = "Unknown";
                    }

                    if (!String.IsNullOrEmpty(Helper.GetString("ACCEPTS_NEW_MEDICAID_PATIENTS", rowServiceLocation)))
                    {
                        if (Helper.GetString("ACCEPTS_NEW_MEDICAID_PATIENTS", rowServiceLocation).Equals("1") || Helper.GetString("ACCEPTS_NEW_MEDICAID_PATIENTS", rowServiceLocation).ToLower().Equals("true"))
                        {
                            lblNewMedicaidLBL.Text = "Yes";
                        }
                        else
                        {
                            lblNewMedicaidLBL.Text = "No";
                        }
                    }
                    else
                    {
                        lblNewMedicaidLBL.Text = "Unknown";
                    }
                }
                if (Helper.HasRows(dsPublicProviderLblData))
                {
                    DataRow rowPublicProviderLblData = dsPublicProviderLblData.Tables[0].Rows[0];
                    lblBoardCertification.Text = Helper.GetString("BOARD_CERTIFICATION_NAME", rowPublicProviderLblData);
                    lblHospitalAffiliations.Text = Helper.GetString("FACILITY_NAME", rowPublicProviderLblData);
                    lblCulturalCompetenciesLBL.Text = Helper.GetString("CULTURAL_COMPETENCIES", rowPublicProviderLblData);
                    lblOfficeAccommodationsLBL.Text = Helper.GetString("OFFICE_ACCOMMODATIONS", rowPublicProviderLblData);
                    lblLanguagesSpokenLBL.Text = Helper.GetString("LANGUAGES_SPOKEN", rowPublicProviderLblData);
                    lblDMEProductsLBL.Text = Helper.GetString("DME_PRODUCT_SERVICES", rowPublicProviderLblData);
                    lblSpecialty.Text = Helper.GetString("SPECIALTY", rowPublicProviderLblData);

                }
                // code for Cultural Competencies, Office Accomodations, Languages Spoken, DME Products and Services
                this.mpeReviewProvider.Show();
            }
        }

        if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName)
        {
            gvProviders.AllowPaging = false;
            gvProviders.ExportSettings.Excel.Format = (Telerik.Web.UI.GridExcelExportFormat)Enum.Parse(typeof(Telerik.Web.UI.GridExcelExportFormat), "Xlsx");
            gvProviders.ExportSettings.IgnorePaging = true;
            gvProviders.ExportSettings.ExportOnlyData = true;
            gvProviders.ExportSettings.OpenInNewWindow = true;
            gvProviders.ExportSettings.UseItemStyles = true;
        }
        gvProviders.AllowPaging = true;
    }

    private void BindDropDown()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;

        //select All Health Plan
        if (ddlHealthPlan.Items.Count == 0)
        {
            ds = psc.SelectReferenceDataWithoutParam("usp_SelectMCP");
            if (Helper.HasRows(ds))
            {
                Helper.LoadList(ddlHealthPlan, ds.Tables[0], "DSC_MCP", "DSC_MCP", true);
            }
        }


        //select All States
        if (ddlState.Items.Count == 0)
        {
            Helper.LoadRadDropDownListWithStates(ref ddlState, true);
        }
        // select current state
        ddlState.SelectedValue = AppSettings.Get("StateCode", string.Empty);

        //select All Program
        if (ddlProgram.Items.Count == 0)
        {
            ds = psc.SelectProgramType();
            Helper.LoadList(ddlProgram, ds.Tables[0], "PROGRAM_CODE_NAME", "PROGRAM_CODE_ID", true);
        }
        //select All Provider Type
        if (rcbProviderType.Items.Count == 0)
        {
            ds = psc.SelectProviderTypesPublicSearch();
            if (Helper.HasRows(ds))
            {
                rcbProviderType.Items.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    rcbProviderType.Items.Add(new RadComboBoxItem(dr["PROVIDER_TYPE_NAME"].ToString(), dr["MMIS_PROVIDER_TYPE_ID"].ToString()));
                }
            }
        }

        //select All Facility Type (Temporary)
        if (ddlFacilityType.Items.Count == 0)
        {
            ds = psc.SelectReferenceDataWithoutParam("usp_SelectFACILITY_TYPE");
            if (Helper.HasRows(ds))
            {
                Helper.LoadList(ddlFacilityType, ds.Tables[0], "FACILITY_TYPE_NAME", "FACILITY_TYPE_ID", true);
            }
        }
        
        //select All Primary Care Providers Only
        if (ddlPrimaryCareProviders.Items.Count == 0)
        {
            ddlPrimaryCareProviders.Items.Insert(0, new ListItem("", ""));
            ddlPrimaryCareProviders.Items.Insert(1, new ListItem("Yes", "1"));
            ddlPrimaryCareProviders.Items.Insert(2, new ListItem("No", "0"));
        }

        //select All DME Products & Services
        if (rcbDMEProductsServices.Items.Count == 0)
        {
            ds = psc.SelectReferenceDataWithoutParam("usp_SelectDME_PRODUCT_SERVICE_CATEGORY_TYPE");
            if (Helper.HasRows(ds))
            {
                rcbDMEProductsServices.Items.Clear();
                ds.Tables[0].DefaultView.Sort = "DME_PRODUCT_SERVICE_CATEGORY_TYPE_NAME";
                foreach (DataRow dr in ds.Tables[0].DefaultView.ToTable().Rows)
                {
                    rcbDMEProductsServices.Items.Add( new RadComboBoxItem(dr["DME_PRODUCT_SERVICE_CATEGORY_TYPE_NAME"].ToString(), dr["DME_PRODUCT_SERVICE_CATEGORY_TYPE_ID"].ToString()));
                }
            }
        }

        //select All Radius
        if (ddlRadiusMiles.Items.Count == 0)
        {
            ds = psc.SelectReferenceDataWithoutParam("usp_SelectPROVIDER_DIRECTORY_RADIUS");
            if (Helper.HasRows(ds))
            {
                Helper.LoadList(ddlRadiusMiles, ds.Tables[0], "PROVIDER_DIRECTORY_RADIUS_MILES_VALUE", "PROVIDER_DIRECTORY_RADIUS_MILES_VALUE", true);
            }

            ddlRadiusMiles.SelectedIndex = 1;
        }

        //select All Accepts Patients of Gender
        if (ddlAcceptsPatientsofGender.Items.Count == 0)
        {
            ds = psc.SelectReferenceDataWithoutParam("usp_SelectPROVIDER_GENDER");
            if (Helper.HasRows(ds))
            {
                DataTable dtfilteredgender = ds.Tables[0].AsEnumerable()
                     .Where(r => r.Field<string>("PROVIDER_GENDER_INITIAL") != "U" &&
                                r.Field<string>("PROVIDER_GENDER_INITIAL") != "X" &&
                                r.Field<string>("PROVIDER_GENDER_INITIAL") != "N")
                     .CopyToDataTable();
                Helper.LoadList(ddlAcceptsPatientsofGender, dtfilteredgender, "PROVIDER_GENDER_NAME", "PROVIDER_GENDER_ID", true);
            }
        }

        //select All Accepts New Patients
        if (ddlAcceptsNewPatients.Items.Count == 0)
        {
            ddlAcceptsNewPatients.Items.Insert(0, new ListItem("", ""));
            ddlAcceptsNewPatients.Items.Insert(1, new ListItem("Yes", "1"));
            ddlAcceptsNewPatients.Items.Insert(2, new ListItem("No", "0"));
        }

        //select All Accepts Newborns
        if (ddlAcceptsNewborns.Items.Count == 0)
        {
            ddlAcceptsNewborns.Items.Insert(0, new ListItem("", ""));
            ddlAcceptsNewborns.Items.Insert(1, new ListItem("Yes", "1"));
            ddlAcceptsNewborns.Items.Insert(2, new ListItem("No", "0"));
        }

        //select All Accepts Pregnant Women
        if (ddlAcceptsPregnantWomen.Items.Count == 0)
        {
            ddlAcceptsPregnantWomen.Items.Insert(0, new ListItem("", ""));
            ddlAcceptsPregnantWomen.Items.Insert(1, new ListItem("Yes", "1"));
            ddlAcceptsPregnantWomen.Items.Insert(2, new ListItem("No", "0"));
        }

        if (ddlTelehealth.Items.Count == 0)
        {
            ddlTelehealth.Items.Insert(0, new ListItem("", ""));
            ddlTelehealth.Items.Insert(1, new ListItem("Yes", "1"));
            ddlTelehealth.Items.Insert(2, new ListItem("No", "0"));
        }

        if (ddlCHIP.Items.Count == 0)
        {
            ddlCHIP.Items.Insert(0, new ListItem("", ""));
            ddlCHIP.Items.Insert(1, new ListItem("Yes", "1"));
            ddlCHIP.Items.Insert(2, new ListItem("No", "0"));
        }

        if (ddlNewMedicaid.Items.Count == 0)
        {
            ddlNewMedicaid.Items.Insert(0, new ListItem("", ""));
            ddlNewMedicaid.Items.Insert(1, new ListItem("Yes", "1"));
            ddlNewMedicaid.Items.Insert(2, new ListItem("No", "0"));
        }

        String sam551Active = AppSettings.Get("SAM551_active", "false");
        if (sam551Active.ToLower().Equals("false"))
        {
            lblddlTelehealth.Visible = false;
            ddlTelehealth.Visible = false;
            lblddlCHIP.Visible = false;
            ddlCHIP.Visible = false;
            lblddlNewMedicaid.Visible = false;
            ddlNewMedicaid.Visible = false;
        }


        //select All Provider Speciality
        if (rcbProviderSpeciality.Items.Count == 0)
        {
            loadAllProviderSpeciality(psc);
        }

        //select All Provider Gender
        if(ddlProviderGender.Items.Count == 0)
        {
            ds = psc.SelectReferenceDataWithoutParam("usp_SelectPROVIDER_GENDER");
            if (Helper.HasRows(ds))
            {
                DataTable dtfilteredgender = ds.Tables[0].AsEnumerable()
                          .Where(r => r.Field<string>("PROVIDER_GENDER_INITIAL") != "B")
                                .CopyToDataTable();
                Helper.LoadList(ddlProviderGender, dtfilteredgender, "PROVIDER_GENDER_NAME", "PROVIDER_GENDER_INITIAL", true);
            }
        }

        //Select All Hospital Affiliation
        if (rcbHospitalAffiliation.Items.Count == 0)
        {
            ds = psc.SelectReferenceDataWithoutParam("usp_SelectREG_HEALTH_CARE_FACILITY_AFFILIATION");
            if (Helper.HasRows(ds))
            {
                rcbHospitalAffiliation.Items.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    // since more than one FacilityName can have different REG_HEALTH_CARE_FACILITY_AFFILIATION_ID, we have to search on the FacilityName itself to prevent more than 1 from showing up in the list
                    rcbHospitalAffiliation.Items.Add(new RadComboBoxItem(dr["FacilityName"].ToString(), dr["FacilityName"].ToString()));
                }
            }
        }

        //select All Languages Spoken
        if (rcbLanguagesSpoken.Items.Count == 0)
        {
            ds = psc.SelectReferenceDataWithoutParam("usp_SelectLANGUAGES_SPOKEN");
            if (Helper.HasRows(ds))
            {
                rcbLanguagesSpoken.Items.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    rcbLanguagesSpoken.Items.Add(new RadComboBoxItem(dr["DESC_Languages_Spoken"].ToString(), dr["Languages_Spoken_ID"].ToString()));
                }
            }
        }

        //select All Specialized Training
        if (rcbSpecializedTraining.Items.Count == 0)
        {
            ds = psc.SelectReferenceDataWithoutParam("usp_SelectSPECIALIZED_TRAINING");
            if (Helper.HasRows(ds))
            {
                rcbSpecializedTraining.Items.Clear();
                ds.Tables[0].DefaultView.Sort = "DSC_Specialized_Training";
                foreach (DataRow dr in ds.Tables[0].DefaultView.ToTable().Rows)
                {
                    rcbSpecializedTraining.Items.Add(new RadComboBoxItem(dr["DSC_Specialized_Training"].ToString(), dr["Specialized_Training_ID"].ToString()));
                }
            }
        }

        //select All Cultural Competencies
        if (rcbCulturalCompetencies.Items.Count == 0)
        {
            ds = psc.SelectReferenceDataWithoutParam("usp_SelectCULTURAL_COMPETENCIES");
            if (Helper.HasRows(ds))
            {
                rcbCulturalCompetencies.Items.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    rcbCulturalCompetencies.Items.Add(new RadComboBoxItem(dr["DSC_Cultural_competencies"].ToString(), dr["Cultural_competencies_ID"].ToString()));
                }
            }
        }

        //select All ADA Accomodations
        if (rcbADAAccommodations.Items.Count == 0)
        {
            ds = psc.SelectReferenceDataWithoutParam("usp_SelectOFFICE_ACCOMMODATIONS");
            if (Helper.HasRows(ds))
            {
                rcbADAAccommodations.Items.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    rcbADAAccommodations.Items.Add(new RadComboBoxItem(dr["DSC_Office_Accommodations"].ToString(), dr["Office_Accommodations_ID"].ToString()));
                }
            }
        }

        //select All Board Certifications
        if (rcbBoardCertifications.Items.Count == 0)
        {
            ds = psc.SelectReferenceDataWithoutParam("usp_SelectBOARD_CERTIFICATIONS");
            if (Helper.HasRows(ds))
            {
                rcbBoardCertifications.Items.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    rcbBoardCertifications.Items.Add(new RadComboBoxItem(dr["BOARD_CERTIFICATION_NAME"].ToString(), dr["BOARD_CERTIFICATION_ID"].ToString()));
                }
            }
        }

        // populate the county list with the current state
        SelectState(ddlState.SelectedValue);
    }
    protected void SelectCounty_OnSelectedStateIndexChanged(object sender, EventArgs e)
    {
        SelectState(ddlState.SelectedValue);
    }
    protected void rdd_OnSelectedCountyIndexChanged(object sender, EventArgs e)
    {
    }

    public void SelectState(string stateID)
    {
        this.LoadCountiesByState(stateID);
    }

    public void LoadCountiesByState(string stateAbbreviation)
    {
        rcbCounty.Items.Clear();
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectCountiesByStateAbbreviation(stateAbbreviation);

        //Select All Counties
        if (rcbCounty.Items.Count == 0)
        {
            if (Helper.HasRows(ds))
            {
                rcbCounty.Items.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    rcbCounty.Items.Add(new RadComboBoxItem(dr["COUNTY_NAME"].ToString(), dr["MMIS_COUNTY_CODE"].ToString()));
                }
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //this.mpeReviewProvider.Hide();
        //gvProviders.SelectRow(-1);
    }
    protected void hlGoogleMaps_Click(object sender, EventArgs e)
    {

    }
    protected void gvProviders_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        int totalResultCount = 0;
        int rowIndex = e.NewPageIndex;
        gvProviders.CurrentPageIndex = e.NewPageIndex;
        gvProviders.DataSource = GetData(out totalResultCount, gvProviders.PageSize, rowIndex * gvProviders.PageSize);
        gvProviders.DataBind();
    }

    protected void lnkPDF_Click(object sender, EventArgs e)
    {
        int totalResultCount = 0;
        int rowIndex = 1;
        DataTable dt = GetData(out totalResultCount, 10000, rowIndex);
        gvProviders.DataSource = dt;
        gvProviders.DataBind();
        gvProviders.MasterTableView.ExportToPdf();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        int totalResultCount = 0;
        int rowIndex = 0;
        DataTable dt = GetData(out totalResultCount, 10000, rowIndex);
        gvProviders.DataSource = dt;
        gvProviders.DataBind();
        gvProviders.MasterTableView.ExportToExcel();
    }

	protected void rcbProviderType_ItemChecked(object sender, RadComboBoxItemEventArgs e)
	{
		var providerTypeCollection = rcbProviderType.CheckedItems;
		var providerTypeSelectedValues = string.Empty;

		if (providerTypeCollection.Count != 0)
		{			
			foreach (var item in providerTypeCollection)
			{
				providerTypeSelectedValues = providerTypeSelectedValues + item.Value + ",";
			}
			providerTypeSelectedValues = providerTypeSelectedValues.Remove(providerTypeSelectedValues.LastIndexOf(","));
		}

		PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
		DataSet ds;

		ds = psc.SelectSpecialtyTypes(providerTypeSelectedValues);
		if (Helper.HasRows(ds))
		{
			rcbProviderSpeciality.Items.Clear();
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				rcbProviderSpeciality.Items.Insert(0, new RadComboBoxItem(dr["EXTERNAL_SPECIALTY_TYPE_NAME"].ToString(), dr["SPECIALTY_TYPE_ID"].ToString()));
			}
		}

        

    }
    private void loadAllProviderSpeciality(PDMSService.PDMSServiceClient psc)
    {
        
        DataSet ds = psc.SelectSpecialtyTypes();
        if (Helper.HasRows(ds))
        {
            rcbProviderSpeciality.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                rcbProviderSpeciality.Items.Add(new RadComboBoxItem(dr["EXTERNAL_SPECIALTY_TYPE_NAME"].ToString(), dr["SPECIALTY_TYPE_ID"].ToString()));
            }
        }
    }
}