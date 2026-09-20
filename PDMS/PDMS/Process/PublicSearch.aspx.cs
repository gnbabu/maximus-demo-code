using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_PublicSearch : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            Response.Redirect("~/Process/PublicSearchNew.aspx");   
        }
        else
        {
            Page.MasterPageFile = "~/MasterPage.master";
            Page.Theme = "Default";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                this.Page.Title = (string)GetGlobalResourceObject("BrandingResource", "PortalName");
                BindDropDown();
            }
            catch (Exception ex)
            {
                MessageBox2.Show("An error occurred when loading the work queue.<br>Error: " + ex.Message + " " + ex.StackTrace, "Error");
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.gvProviders.CurrentPageIndex = 0;
        RefreshData();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        lbProviderType.SelectedIndex = 0;
        lbSpecialty.SelectedIndex = 0;
        ddlLastNameSearch.SelectedIndex = 0;
        txtLastName.Text = "";
        ddlFirstNameSearch.SelectedIndex = 0;
        txtFirstName.Text = "";
        ddlCitySearch.SelectedIndex = 0;
        txtCity.Text = "";
        ddlState.SelectedIndex = 0;
        txtZip.Text = "";
        lbQuadrant.SelectedIndex = 0;
    }

    protected void gvProviders_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }

    protected void gvProviders_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }
    public void RefreshData()
    {
        int totalResultCount = 0;
        DataTable dt = GetData(out totalResultCount, gvProviders.PageSize);
        //hdnRowCount.Value = totalResultCount.ToString();
        gvProviders.DataSource = dt;
        gvProviders.VirtualItemCount = totalResultCount;
        gvProviders.DataBind();
    }

    private DataTable GetData(out int totalResultCount, int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        string sortColWithDirection = this.gvProviders.GridViewSortDirection == SortDirection.Descending ? gvProviders.GridViewSortColumn + " DESC" : gvProviders.GridViewSortColumn;
        totalResultCount = 0;

        bool locationFieldsEntered = false;
        string providerTypeAbbrevList = "";
        string specialtyTypeIDList = "";
        string lastName = txtLastName.Text.Trim();
        string firstName = txtFirstName.Text.Trim();
        string city = txtCity.Text.Trim();

        if (!(lastName == "" && firstName == "" && city == "" && ddlState.SelectedValue == "" && txtZip.Text.Trim() == "" && lbQuadrant.SelectedValue == ""))
            locationFieldsEntered = true;

        if (locationFieldsEntered)
        {
            lastName = FormatLikeValue(ddlLastNameSearch.SelectedValue, lastName);
            firstName = FormatLikeValue(ddlFirstNameSearch.SelectedValue, firstName);
            city = FormatLikeValue(ddlCitySearch.SelectedValue, city);
        }

        // Create delimited list of provider type abbreviations
        foreach (ListItem item in lbProviderType.Items)
        {
            if (item.Selected && item.Value != "")
                providerTypeAbbrevList += (providerTypeAbbrevList == string.Empty) ? item.Value : "," + item.Value;
        }

        // Create delimited list of specialty type IDs
        foreach (ListItem item in lbSpecialty.Items)
        {
            if (item.Selected && item.Value != "")
                specialtyTypeIDList += (specialtyTypeIDList == string.Empty) ? item.Value : "," + item.Value;
        }

        // If list of IDs has been passed in, display in search results list.
        //ds = psc.SearchProvidersPublic(providerTypeAbbrevList, specialtyTypeIDList, lastName, firstName, city, ddlState.SelectedValue, 
        //    txtZip.Text.Trim(), lbQuadrant.SelectedValue, 
        //    sortColWithDirection, pageSize, gvProviders.CurrentRowIndex, true, out totalResultCount);

        //if (Helper.HasRows(ds))
        //{
        //    return ds.Tables[0];
        //}
        //else
        return new DataTable();
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
        else
            value = "%%";
          
        return value;
    }

    protected void gvProviders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "ReviewRow" && e.CommandName != "ExpressAdmin")
            return;

        int index = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "ReviewRow")
        {
            int regId = (int)this.gvProviders.DataKeys[index].Values["REG_ID"];
            if (regId > 0)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet dsProvider = psc.SelectRegistrationData(regId, "PROVIDER");
                DataSet dsServiceLocation = psc.SelectRegistrationData(regId, "SERVICE_LOCATION");
                if (Helper.HasRows(dsProvider))
                {
                    DataRow rowProvider = dsProvider.Tables[0].Rows[0];
                    lblProviderName.Text = Helper.GetString("NAME", rowProvider);
                    lblSpecialty.Text = this.gvProviders.DataKeys[index].Values["SpecialtyTypeName"].ToString();
                }
                if (Helper.HasRows(dsServiceLocation))
                {
                    DataRow rowServiceLocation = dsServiceLocation.Tables[0].Rows[0];
                    lblAddress.Text = string.Format("{0}{1}", Helper.GetString("SERVICING_ADDRESS1", rowServiceLocation), (Helper.GetString("SERVICING_QUADRANT", rowServiceLocation) == string.Empty) ? "" : ", " + Helper.GetString("SERVICING_QUADRANT", rowServiceLocation));
                    lblCityStateZip.Text = string.Format("{0}, {1}, {2}", Helper.GetString("SERVICING_CITY", rowServiceLocation), Helper.GetString("SERVICING_STATE", rowServiceLocation), Helper.GetString("SERVICING_ZIP", rowServiceLocation));
                    lblOfficePhone.Text = Helper.GetString("SERVICING_PHONE_NUMBER", rowServiceLocation);

                    long phoneNumberLong = 0;
                    if (long.TryParse(lblOfficePhone.Text, out phoneNumberLong))
                    {
                        lblOfficePhone.Text = String.Format("{0:###-###-####}", phoneNumberLong);
                    }
                    if (Helper.GetString("SERVICING_STATE", rowServiceLocation) == "DC")
                    {
                        lblWardCountyLabel.Text = "Ward";
                        lblWardCounty.Text = Helper.GetString("SERVICING_WARD", rowServiceLocation);
                    }
                    else
                    {
                        lblWardCountyLabel.Text = "County";
                        lblWardCounty.Text = Helper.GetString("SERVICING_COUNTY", rowServiceLocation);
                    }
                    hlGoogleMaps.NavigateUrl = string.Format("http://maps.google.com/maps?f=q&hl=en&q={0},{1}", lblAddress.Text, lblCityStateZip.Text); //1310 SOUTHERN AVENUE,WASHINGTON,DC,20032";

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
                }

                    this.mpeReviewProvider.Show();
            }
        }

    }

    protected void gvProviders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (e.Row.Cells[5].Text == "9999999999")
        //    {
        //        e.Row.Cells[5].Text = string.Empty;
        //    }

        //    LinkButton lnkExpress = (LinkButton)e.Row.FindControl("lnkExpressAdmin");
        //    if (lnkExpress != null)
        //    {
        //        //this control is hidden on page load if user role is not allowed to perform express operations.
        //        string currentStepID = DataBinder.Eval(e.Row.DataItem, "CurrentStepID").ToString();
        //        string regUserID = DataBinder.Eval(e.Row.DataItem, "UserID").ToString();
        //        int regPgmStatusTypeID = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "REG_PROGRAM_STATUS_TYPE_ID").ToString());
        //        var entitytypeIdStr = DataBinder.Eval(e.Row.DataItem, "ENTITY_TYPE_ID").ToString();
        //        int entityTypeID = string.IsNullOrEmpty(entitytypeIdStr) ? 0 : int.Parse(entitytypeIdStr);

        //        bool regOkToTerm = CanTermRegistration(currentStepID, regUserID, regPgmStatusTypeID, entityTypeID);
        //        bool regOkToRetroDate = CanRetroDateRegistration(currentStepID, regUserID, regPgmStatusTypeID, entityTypeID);

        //        lnkExpress.Visible = regOkToTerm || regOkToRetroDate;
        //    }
        //}
    }

    private void BindDropDown()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet ds = psc.SelectProviderTypesWithAbbrev("");
        Helper.LoadList(lbProviderType, ds.Tables[0], "PROVIDER_TYPE_NAME", "PROVIDER_TYPE_ABBREVIATION", true);

        ds = psc.SelectSpecialtyTypes();
        Helper.LoadList(lbSpecialty, ds.Tables[0], "SPECIALTY_TYPE_NAME", "SPECIALTY_TYPE_ID", true);

        Helper.LoadDropDownListWithStates(ref ddlState);
        ddlState.Items.Insert(0, new ListItem("", ""));
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.mpeReviewProvider.Hide();
        gvProviders.SelectRow(-1);
    }
    protected void hlGoogleMaps_Click(object sender, EventArgs e)
    {

    }
}