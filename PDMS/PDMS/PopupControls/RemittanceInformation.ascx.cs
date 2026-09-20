using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_RemittanceInformation : BaseSectionControl
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

    public string SaveButtonClientID
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //ucAddress.VisibleAddress3 = false;

    }
    public override bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(prov_AdviceName.Text) || !string.IsNullOrEmpty(ucAddress.StreetAddress) || !string.IsNullOrEmpty(ucAddress.UnitAddress) || !string.IsNullOrEmpty(ucAddress.City) ||
            !string.IsNullOrEmpty(ucAddress.State) || !string.IsNullOrEmpty(ucAddress.Zip5) || !string.IsNullOrEmpty(ucAddress.Zip4) ||
         !string.IsNullOrEmpty(ucAddress.County) ||
        !string.IsNullOrEmpty(Helper.StripNonNumerics(prov_Phone.Text)) || !string.IsNullOrEmpty(Helper.StripNonNumerics(prov_address_phone.Text)) ||
         !string.IsNullOrEmpty(prov_ContactName.Text) || !string.IsNullOrEmpty(Helper.StripNonNumerics(prov_Fax.Text)) ||
        !string.IsNullOrEmpty(prov_Email.Text) || !string.IsNullOrEmpty(txtTitle.Text))
        {
            rtn = true;

        }

        return rtn;
    }
    public override bool SaveData()
    {
        if (prov_address_phone.Text != "(___) ___-____")
        {
            revPhone.Enabled = true;
        }
        if (prov_Phone.Text != "(___) ___-____")
        {
            rgvContactPhone.Enabled = true;
        }
        Page.Validate("valOwnerInfo");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valOwnerInfo") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }


        //if (string.IsNullOrEmpty(hidID.Text))
        //{
        //    Set_hidID(null);
        //}
        
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        //parms.Add("MAILTO_ADDRESS_NAME", prov_Name.Text);
        parms.Add("REMITTANCE_ADDRESS1", ucAddress.StreetAddress);
        parms.Add("REMITTANCE_ADDRESS2", ucAddress.UnitAddress);
        parms.Add("MAILTO_ADDRESS3", "");
        parms.Add("REMITTANCE_CITY", ucAddress.City);
        parms.Add("REMITTANCE_STATE", ucAddress.State);
        parms.Add("REMITTANCE_ZIP", ucAddress.Zip5);
        parms.Add("REMITTANCE_EXT_ZIP", ucAddress.Zip4);
        parms.Add("REMITTANCE_COUNTY", ucAddress.County);
        parms.Add("REMITTANCE_PHONE_NUMBER", Helper.StripNonNumerics(prov_Phone.Text));
        parms.Add("REMITTANCE_ADDRESS_PHONE_NUMBER", Helper.StripNonNumerics(prov_address_phone.Text));
        parms.Add("REMITTANCE_FAX_NUMBER", Helper.StripNonNumerics(prov_Phone.Text));
        parms.Add("REMITTANCE_EMAIL", prov_Email.Text);
        parms.Add("REMITTANCE_ADVICENAME", prov_AdviceName.Text);
        parms.Add("REMITTANCE_CONTACT_TYPE", rblContactType.SelectedItem.Value);
        parms.Add("REMITTANCE_CONTACT_NAME", prov_ContactName.Text);
        parms.Add("REMITTANCE_ORGANIZATION_NAME", prov_OrgName.Text);
        parms.Add("REMITTANCE_ADDRESS_FIRSTNAME", txtAFirstName.Text);
        parms.Add("REMITTANCE_ADDRESS_MIDDLENAME", txtAMiddleName.Text);
        parms.Add("REMITTANCE_ADDRESS_LASTNAME", txtALastName.Text);
        parms.Add("REMITTANCE_ADDRESS_TITLE", txtTitle.Text);

       
        //parms.Add("MAILTO_PHONE_EXT", Helper.StripNonNumerics(prov_PhoneExt.Text));
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);

        if (isEdit || !string.IsNullOrEmpty(hidID.Text))
        {
            parms.Add("REMITTANCE_ADDR_MODIFIED_STATUS_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
            parms.Add("REG_SERVICE_LOCATION_ID", hidID.Text);
            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATIONcustom", parms);
        }
        else
        {
            parms.Add("REMITTANCE_ADDR_MODIFIED_STATUS_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATIONcustom", parms);
        }

        ucAddress.Confirmed = true;

        return true;
    }

    public override bool ValidateData()
    {
        return true;
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadRemittanceInformation();
    }

    private void LoadRemittanceInformation()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
        DataTable dtPrimaryPracticeLocation = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        DataTable dtServiceLocation = Helper.GetServiceLocationColumns(dtPrimaryPracticeLocation, CON.AddressType.Remittance);
        this.DataList = dtServiceLocation;

        if (Helper.HasRows(this.DataList))
            this.LoadData(this.DataList.Rows[0]);
        else
            this.LoadData(null);
    }

    public override void LoadData(DataRow row)
    {
        bool isEdit = false;

        if (row == null)
            isEdit = false;
        else
            isEdit = true;

        ucAddress.LoadState();

        DataSet dsProvider = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");

        bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
        prov_Same.Checked = false;
        //DETERMINE IF PROVIDER IS INDIVIDUAL OR ORGANIZATION
        DataRow drProvider = dsProvider.Tables[0].Rows[0];
        string providerType = Helper.GetString("ENTITY_TYPE_ID", drProvider);

        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            prov_Same.Enabled =
                //prov_Name.Enabled =
                //prov_Email.Enabled =
            ucAddress.EnableStreetAddress =
            ucAddress.EnableUnitAddress =
                //ucAddress.EnableAddress3 =
            ucAddress.EnableCity =
            ucAddress.EnableState =
            ucAddress.EnableZip5 =
            ucAddress.EnableZip4 =
            prov_Phone.Enabled = true;
            //prov_PhoneExt.Enabled = true;
        }

        else if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.Administrator))
        {
            prov_Same.Enabled =
                //prov_Name.Enabled =
                //prov_Email.Enabled =
            ucAddress.EnableStreetAddress =
            ucAddress.EnableUnitAddress =
                //ucAddress.EnableAddress3 =
            ucAddress.EnableCity =
            ucAddress.EnableState =
            ucAddress.EnableZip5 =
            ucAddress.EnableZip4 =
            prov_Phone.Enabled = !regIsPending;
            //prov_PhoneExt.Enabled = !regIsPending;
        }
        else
        {
            prov_Same.Enabled =
                //prov_Name.Enabled =
                //prov_Email.Enabled =
            ucAddress.EnableStreetAddress =
            ucAddress.EnableUnitAddress =
                //ucAddress.EnableAddress3 =
            ucAddress.EnableCity =
            ucAddress.EnableState =
            ucAddress.EnableZip5 =
            ucAddress.EnableZip4 =
            prov_Phone.Enabled = false;
            //prov_PhoneExt.Enabled = false;    
        }

        //ParentTable.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") && isEdit ? "block" : "none";
        //ParentTable.Rows[9].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") && isEdit ? "block" : "none";
        hidIsEdit.Text = isEdit.ToString();
        Set_hidID(row);

        if (isEdit)
        {
            //prov_Name.Text = row["MAILTO_ADDRESS_NAME"].ToString();
            //prov_Email.Text = row["MAILTO_EMAIL_ADDRESS"].ToString();
            ucAddress.StreetAddress = row["REMITTANCE_ADDRESS1"].ToString();
            ucAddress.UnitAddress = row["REMITTANCE_ADDRESS2"].ToString();
            //ucAddress.AddressLine3 = row["REMITTANCE_ADDRESS3"].ToString();
            ucAddress.City = row["REMITTANCE_CITY"].ToString();
            ucAddress.State = row["REMITTANCE_STATE"].ToString();
            ucAddress.Zip5 = row["REMITTANCE_ZIP"].ToString();
            ucAddress.Zip4 = row["REMITTANCE_EXT_ZIP"].ToString();

            if (!string.IsNullOrEmpty(Helper.GetString("REMITTANCE_ADDRESS_FIRSTNAME", row)))
                txtAFirstName.Text = row["REMITTANCE_ADDRESS_FIRSTNAME"].ToString();
            else
                txtAFirstName.Text = drProvider["FIRST_NAME"].ToString();
            if (!string.IsNullOrEmpty(Helper.GetString("REMITTANCE_ADDRESS_MIDDLENAME", row)))
                txtAMiddleName.Text = row["REMITTANCE_ADDRESS_MIDDLENAME"].ToString();
            else
                txtAMiddleName.Text = drProvider["MIDDLE_INITIAL"].ToString();

            if (!string.IsNullOrEmpty(Helper.GetString("REMITTANCE_ADDRESS_LASTNAME", row)))
                txtALastName.Text = row["REMITTANCE_ADDRESS_LASTNAME"].ToString();
            else
                txtALastName.Text = drProvider["LAST_NAME"].ToString();

            if (!string.IsNullOrEmpty(Helper.GetString("REMITTANCE_ADDRESS_TITLE", row)))
                txtTitle.Text = row["REMITTANCE_ADDRESS_TITLE"].ToString();
            else
                txtTitle.Text = drProvider["TITLE"].ToString();

            if (!string.IsNullOrEmpty(Helper.GetString("REMITTANCE_ORGANIZATION_NAME", row)))
                prov_OrgName.Text = row["REMITTANCE_ORGANIZATION_NAME"].ToString();
            else
                prov_OrgName.Text = drProvider["NAME"].ToString();


            if (ucAddress.State != "")
            {
                string countyName = row["REMITTANCE_COUNTY"].ToString();

                ucAddress.LoadCountiesByState(ucAddress.State);
                ucAddress.County = countyName;
            }

            prov_ContactName.Text = Helper.GetString("REMITTANCE_CONTACT_NAME", row);
            prov_address_phone.Text = Helper.FormatPhone(row["REMITTANCE_ADDRESS_PHONE_NUMBER"].ToString());
            prov_Phone.Text = Helper.FormatPhone(row["REMITTANCE_PHONE_NUMBER"].ToString());
            prov_Email.Text = Helper.GetString("REMITTANCE_EMAIL", row);
            prov_AdviceName.Text = Helper.GetString("REMITTANCE_ADVICENAME", row);
            string cType = Helper.GetString("REMITTANCE_CONTACT_TYPE", row);
            prov_Fax.Text = Helper.GetString("REMITTANCE_FAX_NUMBER", row);

            if (cType != "")
                rblContactType.SelectedValue = cType;

            if (cType == CON.ContactType.Individual)
            {
                trFirst.Visible = true;
                trMiddle.Visible = true;
                trLast.Visible = true;
                trTitle.Visible = true;
                rfvAFirstName.Enabled = true;
                rfvALastName.Enabled = true;
                this.rfvOrgName.Enabled = false;
                trOrg.Visible = false;
            }

            if (cType == CON.ContactType.Organization)
            {
                trOrg.Visible = true;
                this.rfvOrgName.Enabled = true;
               
            }

            if (cType == String.Empty)
            {

                if (providerType == "1")//individual
                {
                    rblContactType.SelectedValue = CON.ContactType.Individual;
                    trFirst.Visible = true;
                    trMiddle.Visible = true;
                    trLast.Visible = true;
                    trTitle.Visible = true;
                    rfvAFirstName.Enabled = true;
                    rfvALastName.Enabled = true;
                    rfvOrgName.Enabled = false;
                    trOrg.Visible = false;
                }
                else
                {
                    rblContactType.SelectedValue = CON.ContactType.Organization;
                    trOrg.Visible = true;
                    rfvOrgName.Enabled = true;
                }

                //prov_ContactName.Text = Helper.GetString("REMITTANCE_CONTACT_NAME", row);
                //prov_OrgName.Text = Helper.GetString("REMITTANCE_ORGANIZATION_NAME", row);
                

            }
                }
        else
        {
            //WHEN LANDING ON POP UP FIRST TIME ADD THE DATA FROM PROVIDER TABLE, THEY CAN CHANGE IT.
            DataSet dsProv = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
            if (Helper.HasRows(dsProvider))
            {
                DataRow rowProv = dsProv.Tables[0].Rows[0];

                txtAFirstName.Text = rowProv["FIRST_NAME"].ToString();

                txtAMiddleName.Text = rowProv["MIDDLE_INITIAL"].ToString();

                txtALastName.Text = rowProv["LAST_NAME"].ToString();

                txtTitle.Text = rowProv["TITLE"].ToString();

                prov_OrgName.Text = rowProv["NAME"].ToString();

                prov_ContactName.Text = (row != null) ? Helper.GetString("REMITTANCE_CONTACT_NAME", row) : "";

            }
            if (providerType == "1")//individual
            {
                rblContactType.SelectedValue = CON.ContactType.Individual;
                trFirst.Visible = true;
                trMiddle.Visible = true;
                trLast.Visible = true;
                trTitle.Visible = true;
                rfvAFirstName.Enabled = true;
                rfvALastName.Enabled = true;
                rfvOrgName.Enabled = false;
                trOrg.Visible = false;
            }
            else
            {
                rblContactType.SelectedValue = CON.ContactType.Organization;
                trOrg.Visible = true;
                rfvOrgName.Enabled = true;
            }
            // prov_Payable.Text = string.Empty;
            //prov_Email.Text = string.Empty;
            ucAddress.StreetAddress = string.Empty;
            ucAddress.UnitAddress = string.Empty;
            //prov_Address3.Text = string.Empty;
            ucAddress.City = string.Empty;
            ucAddress.State = "";
            ucAddress.Zip5 = string.Empty;
            ucAddress.Zip4 = string.Empty;

            //depending on application select organization or individual from rblContactType.SelectedValue


        }

        ucAddress.SaveButtonClientID = SaveButtonClientID;
    }

    private void Set_hidID(DataRow row)
    {
        if (row == null)
        {
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
            if (Helper.HasRows(ds))
            {
                hidID.Text = ds.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"].ToString();
            }
        }
        else
        {
            hidID.Text = row["REG_SERVICE_LOCATION_ID"].ToString();
        }
    }

    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valOwnerInfo";
        this.Page.Validators.Add(val);
    }

    protected void prov_Same_CheckedChanged(object sender, EventArgs e)
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
          DataSet dsProvider = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");


//Case When Adding New Address
    //If user checks ‘Same as Practice Location’ (enabled when first adding the Address)
        //If Provider Entity_Type = 1
    //Populate individual name fields from REG_PROVIDER Individual names
    //Populate remaining address fields – copy from primary address
        //Else
    //Populate Organization Name field from REG_PROVIDER name
    //Populate remaining address fields – copy from primary address

        if (Helper.HasRows(dsProvider) && prov_Same.Checked)
        {
            DataRow rowProv = dsProvider.Tables[0].Rows[0];
            if (Helper.HasRows(ds)) //check if REMITTANCE_ADDRESS_FIRSTNAME
            {
                DataRow rowServ = ds.Tables[0].Rows[0];
                if (!string.IsNullOrEmpty(rowServ["REMITTANCE_ADDRESS_FIRSTNAME"].ToString()))
                    txtAFirstName.Text = rowServ["REMITTANCE_ADDRESS_FIRSTNAME"].ToString();
                else
                    txtAFirstName.Text = rowProv["FIRST_NAME"].ToString();
                
                if(!string.IsNullOrEmpty(rowServ["REMITTANCE_ADDRESS_MIDDLENAME"].ToString()))
                txtAMiddleName.Text = rowServ["REMITTANCE_ADDRESS_MIDDLENAME"].ToString();
                else
                txtAMiddleName.Text = rowProv["MIDDLE_INITIAL"].ToString();

                if(!string.IsNullOrEmpty(rowServ["REMITTANCE_ADDRESS_LASTNAME"].ToString()))
                txtALastName.Text = rowServ["REMITTANCE_ADDRESS_LASTNAME"].ToString();
                else
                txtALastName.Text = rowProv["LAST_NAME"].ToString();

                if(!string.IsNullOrEmpty(rowServ["REMITTANCE_ADDRESS_TITLE"].ToString()))
                txtTitle.Text = rowServ["REMITTANCE_ADDRESS_TITLE"].ToString();
                else
                txtTitle.Text = rowProv["TITLE"].ToString();

                if(!string.IsNullOrEmpty(rowServ["REMITTANCE_ORGANIZATION_NAME"].ToString()))
                prov_OrgName.Text = rowServ["REMITTANCE_ORGANIZATION_NAME"].ToString();
                else
                prov_OrgName.Text = rowProv["NAME"].ToString();
            }
        }
        if (Helper.HasRows(ds) && prov_Same.Checked)
        {
            DataRow row = ds.Tables[0].Rows[0];
            //prov_Name.Text = string.IsNullOrEmpty(row["PRACTICE_NAME"].ToString()) ? string.Empty : row["PRACTICE_NAME"].ToString();
            ucAddress.StreetAddress = string.IsNullOrEmpty(row["SERVICING_ADDRESS1"].ToString()) ? string.Empty : row["SERVICING_ADDRESS1"].ToString();
            ucAddress.UnitAddress = string.IsNullOrEmpty(row["SERVICING_ADDRESS2"].ToString()) ? string.Empty : row["SERVICING_ADDRESS2"].ToString();
            //ucAddress.AddressLine3 = string.IsNullOrEmpty(row["SERVICING_ADDRESS3"].ToString()) ? string.Empty : row["SERVICING_ADDRESS3"].ToString();
            ucAddress.City = string.IsNullOrEmpty(row["SERVICING_CITY"].ToString()) ? string.Empty : row["SERVICING_CITY"].ToString();
            ucAddress.State = string.IsNullOrEmpty(row["SERVICING_STATE"].ToString()) ? string.Empty : row["SERVICING_STATE"].ToString();
            ucAddress.Zip5 = string.IsNullOrEmpty(row["SERVICING_ZIP"].ToString()) ? string.Empty : row["SERVICING_ZIP"].ToString();
            ucAddress.Zip4 = string.IsNullOrEmpty(row["SERVICING_EXT_ZIP"].ToString()) ? string.Empty : row["SERVICING_EXT_ZIP"].ToString();
            ucAddress.County = string.IsNullOrEmpty(row["SERVICING_COUNTY"].ToString()) ? string.Empty : row["SERVICING_COUNTY"].ToString();
            
            prov_Phone.Text = Helper.FormatPhone(row["SERVICING_PHONE_NUMBER"].ToString());
            prov_address_phone.Text = Helper.FormatPhone(row["SERVICING_ADDRESS_PHONE_NUMBER"].ToString());
            prov_Fax.Text = Helper.FormatPhone(row["SERVICING_FAX_NUMBER"].ToString());
            prov_Email.Text = Helper.GetString("SERVICING_EMAIL_ADDRESS", row);
            string cType = Helper.GetString("SERVICING_CONTACT_TYPE", row);
            if (cType != "")
                rblContactType.SelectedValue = cType;

            if (cType == CON.ContactType.Individual)
            {
               // this.rfvContactname.Enabled = true;
                this.rfvOrgName.Enabled = false;
            }

            if (cType == CON.ContactType.Organization)
            {
                this.rfvOrgName.Enabled = true;
                //this.rfvContactname.Enabled = true;
            }

            prov_ContactName.Text = Helper.GetString("SERVICING_CONTACT_NAME", row);
            prov_OrgName.Text = Helper.GetString("SERVICING_ORGANIZATION_NAME", row);

            upProv.Update();
        }
    }

    private string GetContactZip()
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        return Helper.HasRows(ds) ? ds.Tables[0].Rows[0]["CONTACT_ZIP"].ToString() : string.Empty;
    }

    private string GetStateName(string id)
    {
        int i;
        if (!int.TryParse(id, out i))
        {
            return string.Empty;
        }

        DataTable dt = ApplicationCache.StateAbbreviations().Select("StateId = " + id).CopyToDataTable();

        if (Helper.HasRows(dt))
        {
            return dt.Rows[0]["StateName"].ToString();
        }
        else
        {
            return string.Empty;
        }
    }

    protected void rblContactType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DETERMINE IF PROVIDER IS INDIVIDUAL OR ORGANIZATION
        DataSet dsProvider = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        DataRow drProvider = dsProvider.Tables[0].Rows[0];
        string providerType = Helper.GetString("ENTITY_TYPE_ID", drProvider);

        if (rblContactType.SelectedValue == CON.ContactType.Individual)
        {
            if (providerType == "1") //individula provider choosing individual address type
            {
                trFirst.Visible = true;
                trMiddle.Visible = true;
                trLast.Visible = true;
                trTitle.Visible = true;

                rfvAFirstName.Enabled = true;
                rfvALastName.Enabled = true;

                trOrg.Visible = false;
                rfvOrgName.Enabled = false;
                //rfvOrgName.ErrorMessage = "";
            }
            else //organization provider trying to enter indi details, so don't prepopulate it
            {
                trFirst.Visible = true;
                txtAFirstName.Text = "";
                trMiddle.Visible = true;
                txtAMiddleName.Text = "";
                trLast.Visible = true;
                txtALastName.Text = "";
                trTitle.Visible = true;
                txtTitle.Text = "";

                rfvAFirstName.Enabled = true;
                rfvALastName.Enabled = true;

                rfvOrgName.Enabled = false;
                trOrg.Visible = false;
                //prov_OrgName.Text = "";
            }
        }
        if (rblContactType.SelectedValue == CON.ContactType.Organization)
        {
            if (providerType != "1") //organization provider trying to enter organization, keep it prepoulated
            {
                trFirst.Visible = false;
                txtAFirstName.Text = "";
                trMiddle.Visible = false;
                txtAMiddleName.Text = "";
                trLast.Visible = false;
                txtALastName.Text = "";
                trTitle.Visible = false;
                txtTitle.Text = "";

                rfvAFirstName.Enabled = false;
                rfvALastName.Enabled = false;

                trOrg.Visible = true;
                rfvOrgName.Enabled = true;

            }
            else//individual provider trying to enter organization, show org name as blank
            {
                trFirst.Visible = false;
                trMiddle.Visible = false;
                trLast.Visible = false;
                trTitle.Visible = false;
                trOrg.Visible = true;
                rfvAFirstName.Enabled = false;
                rfvALastName.Enabled = false;
                rfvOrgName.Enabled = true;
                prov_OrgName.Text = "";



            }
        }

    }

    public override string ValidationGroup
    {
        // TODO: EDV why Owner info validate??
        get { return "valOwnerInfo"; }
    }

    public override string Title
    {
        get { return "Remittance Location"; }
    }

    public override string IdText
    {
        get { return "ucRemittanceInformation_" + this.WorkflowPage.RegistrationId; }
    }

  
}