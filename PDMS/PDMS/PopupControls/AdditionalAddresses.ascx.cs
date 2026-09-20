using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_AdditionalAddressControl : System.Web.UI.UserControl
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

    //commented out for bug6491
    /*protected void zipCode_Changed(object sender, System.EventArgs e)
    {
        //commented for bug6491
        //reqCounty.Enabled = false;
        regexZip.Validate();
        if (!regexZip.IsValid)
            return;

        var textBox = (TextBox)sender;
        //commented for bug6491
        //SetCountyControls(textBox.Text, true);
    }*/
    //commented for bug6491
    /*private void SetCountyControls(string zipText, bool toEnableCountyControls)
    {
        if (IsContiguousZip(zipText))
        {
            prov_County_ComboBox.Visible = true;
            CountyRequiredLabel.Visible = true;
            reqCounty.Enabled = true;

            prov_County_ComboBox.Enabled = toEnableCountyControls;

            CountyNotRequiredLabel.Visible = false;
            prov_County.Visible = false;
        }
        else
        {
            prov_County_ComboBox.Visible = false;
            CountyRequiredLabel.Visible = false;
            reqCounty.Enabled = false;

            CountyNotRequiredLabel.Visible = true;
            prov_County.Visible = true;
            prov_County.Enabled = toEnableCountyControls;
        }
    }*/

    //commented out for OD-007
    /*protected void prov_City_TextChanged(object sender, EventArgs e)
    {
        if (System.Text.RegularExpressions.Regex.IsMatch("^[a-zA-Z]", prov_City.Text))
        {
            prov_City.Text.Remove(prov_City.Text.Length - 1);
        }
    }*/
    //commented out for bug6491
    /*protected void prov_County_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(prov_County.Text) && System.Text.RegularExpressions.Regex.IsMatch("^[a-zA-Z]", prov_County.Text))
        {
            prov_County.Text.Remove(prov_County.Text.Length - 1);
        }
    }*/
    //commented out for OD-007
    /*
    private bool IsContiguousZip(string zip)
    {
        bool rtn = false;
        DataSet ds = svc.GetContiguousZipcode(zip);
        if (Helper.HasRows(ds))
        {
            rtn = true;
        }
        return rtn;
    }
    public void LoadData()
    {
        Helper.LoadDropDownListWithStates(ref prov_State);
        //commented for bug6491
        /*prov_County_ComboBox.DataSource = svc.GetCountiesList().Tables[0];
        prov_County_ComboBox.DataValueField = "County";
        prov_County_ComboBox.DataTextField = "County";
        prov_County_ComboBox.DataBind();
        prov_County_ComboBox.Items.Insert(0, string.Empty);
        prov_Address_Description.Text = prov_Address1.Text = prov_Suite.Text = prov_City.Text = prov_Zip.Text = prov_ExtZip.Text = string.Empty;
        //commented for bug6491
        //prov_County.Text = 
        prov_Phone.Text = prov_PhoneExt.Text = string.Empty;
        //commented for bug6491
        //prov_County.Visible = CountyRequiredLabel.Visible = CountyNotRequiredLabel.Visible = prov_County_ComboBox.Visible = false;
    }
    */ 
    protected void Page_Load(object sender, EventArgs e)
    {
         ucAddress.SaveButtonClientID = Parent.FindControl("btnSave").ClientID;
    }
    public bool SaveData()
    {
        // OHPNM-14088
        if (this.WorkflowPage.OwnerInfoPageIsReadOnly)
        {
            // this is a read only page; no need to validate or save
            return true;
        }

        //commented for bug6491
        /*if (!prov_County_ComboBox.Visible)
        {
            reqCounty.Enabled = false;
        }*/
        Page.Validate("AdditionalAddresses");
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v!= null && v.ValidationGroup.Equals("AdditionalAddresses") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        int regId = Convert.ToInt32(this.WorkflowPage.RegistrationId.ToString());
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());

        

        /* Commented for OD-007
        parms.Add("ADDRESS_DESC", prov_Address_Description.Text);
        parms.Add("ADDRESS1", prov_Address1.Text);
        parms.Add("ADDRESS2", prov_Suite.Text);
        parms.Add("CITY", prov_City.Text);
        parms.Add("STATE", prov_State.SelectedValue);
        parms.Add("ZIP", prov_Zip.Text);
        parms.Add("EXT_ZIP", prov_ExtZip.Text);
         * */
        //commented for bug6491
        /*if (IsContiguousZip(prov_Zip.Text))
        {
            parms.Add("COUNTY", prov_County_ComboBox.SelectedValue);
        }
        else
        {
            parms.Add("COUNTY", prov_County.Text);
        }*/



        parms.Add("ADDRESS1", ucAddress.StreetAddress);
        parms.Add("ADDRESS2", ucAddress.UnitAddress);
        parms.Add("ADDRESS3", "");
        parms.Add("CITY", ucAddress.City);
        parms.Add("STATE", ucAddress.State);
        parms.Add("ZIP", ucAddress.Zip5);
        parms.Add("EXT_ZIP", ucAddress.Zip4);
        parms.Add("COUNTY", ucAddress.County);

        parms.Add("PHONE", Helper.StripNonNumerics(prov_Phone.Text));
        parms.Add("PHONE_EXT", prov_PhoneExt.Text);

        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        
        
            // Insert if it does not already exist
        if (parms != null && string.IsNullOrEmpty(hdnRegAdditionalAddressID.Value))
        {
            psc.InsertRegistrationData(regId, "ADDITIONAL_ADDRESSES", parms);
        }
        
        
        if (!string.IsNullOrEmpty(hdnRegAdditionalAddressID.Value))
        {
            // Update
            parms.Add("REG_Additional_Address_ID", hdnRegAdditionalAddressID.Value);
            psc.UpdateRegistrationDataTable("Additional_Addresses", parms);
        }

        return true;
    }
   
    /*public bool ValidateData()
    {
        bool result = true;
        
        return result;
    }*/
    public void LoadData(DataRow dr)
    {
        //ParentTable.Rows[0].Cells[0].Style["display"] = Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) ? "block" : "none";
        //commented for bug6491
        /*var toEnableCountyControls = false;
        prov_County_ComboBox.Visible = false;
        CountyRequiredLabel.Visible = false;
        reqCounty.Enabled = false;

        CountyNotRequiredLabel.Visible = false;
        prov_County.Visible = false;*/
        /* Commented for OD-007
       hdnRegAdditionalAddressID.Value = prov_Address_Description.Text = prov_Address1.Text = prov_Suite.Text = prov_City.Text = prov_Zip.Text = prov_ExtZip.Text = string.Empty;
       */
        //commented for bug6491
       //prov_County.Text = 
        hdnRegAdditionalAddressID.Value = ucAddress.StreetAddress =
        ucAddress.UnitAddress =
        
        ucAddress.City =
        ucAddress.State =
        ucAddress.Zip5 =
        ucAddress.Zip4 =             
        string.Empty;
        prov_Phone.Text = prov_PhoneExt.Text = string.Empty;
       //commented for bug6491
       /*PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();       
       prov_County_ComboBox.DataSource = svc.GetCountiesList().Tables[0];
       prov_County_ComboBox.DataValueField = "County";
       prov_County_ComboBox.DataTextField = "County";
       prov_County_ComboBox.DataBind();
       prov_County_ComboBox.Items.Insert(0, string.Empty);*/

        /* Commented for OD-007
         if(prov_State.Items.Count == 0)
             Helper.LoadDropDownListWithStates(ref prov_State);
        */
       ucAddress.LoadState();
       if (dr != null)
       {
           hdnRegAdditionalAddressID.Value = Helper.GetString("REG_ADDITIONAL_ADDRESS_ID", dr);
           ucAddress.StreetAddress = dr["ADDRESS1"].ToString();
           ucAddress.UnitAddress = dr["ADDRESS2"].ToString();
           //ucAddress.AddressLine3 = Helper.GetString("ADDRESS3", dr);

           ucAddress.City = dr["CITY"].ToString();
           ucAddress.State = dr["STATE"].ToString();
           if (ucAddress.State != "")
           {
               string countyName = dr["COUNTY"].ToString();
               //LoadCountiesByState(ucAddress.State);
               //if (Helper.ValueExistsInDropDown(ddlCounty, countyName))
               //{
               //    ddlCounty.SelectedValue = countyName;
               //}
               ucAddress.LoadCountiesByState(ucAddress.State);
               ucAddress.County = countyName;
           }
           ucAddress.Zip5 = dr["ZIP"].ToString();
           ucAddress.Zip4 = dr["EXT_ZIP"].ToString();

           /* Commented for OD-007
           prov_Address_Description.Text = Helper.GetString("ADDRESS_DESC", dr);
           prov_Address1.Text = Helper.GetString("ADDRESS1", dr);
           prov_Suite.Text = Helper.GetString("ADDRESS2", dr);
            
           prov_City.Text = Helper.GetString("CITY", dr);
           prov_State.SelectedValue = Helper.GetString("STATE", dr);
           prov_Zip.Text = Helper.GetString("ZIP", dr);
           prov_ExtZip.Text = Helper.GetString("EXT_ZIP", dr);
           */
           //commented for bug6491
           //prov_County.Text = Helper.GetString("COUNTY", dr);
           prov_Phone.Text = Helper.FormatPhone(Helper.GetString("PHONE", dr));
           prov_PhoneExt.Text = Helper.GetString("PHONE_EXT", dr);
       }


       if (!Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
            Helper.SetReadOnly(this, true);
        //commented for bug6491
        /*if (!string.IsNullOrEmpty(prov_Zip.Text))
        {
            SetCountyControls(prov_Zip.Text, true);
            if (prov_County_ComboBox.Visible == true)
            {
                prov_County_ComboBox.SelectedValue = Helper.GetString("COUNTY", dr);
            }
        }*/
    }
}