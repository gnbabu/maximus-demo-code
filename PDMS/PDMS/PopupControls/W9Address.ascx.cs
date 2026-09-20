using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_W9Address : System.Web.UI.UserControl
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
        // Set the handle to the save button on the parent page.
        // Can't get this working in declarative syntax (which would be nice)
        ucAddress.SaveButtonClientID = Parent.FindControl("btnSave").ClientID;
    }


    public void LoadData(DataRow row, bool isEdit)
    {

        bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
        DataSet dsProvider = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        //DETERMINE IF PROVIDER IS INDIVIDUAL OR ORGANIZATION
        DataRow drProvider = dsProvider.Tables[0].Rows[0];
        string providerType = Helper.GetString("ENTITY_TYPE_ID", drProvider);
        ucAddress.LoadState();
        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {           
            ucAddress.EnableStreetAddress =
            ucAddress.EnableUnitAddress =
            //ucAddress.EnableAddress3 =
            ucAddress.EnableCity =
            ucAddress.EnableState =
            ucAddress.EnableZip5 =
            ucAddress.EnableZip4 =
            prov_Phone.Enabled =
            prov_Fax.Enabled = txtContactName.Enabled = txtEmail.Enabled = true;
        }

        else if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.Administrator))
        {        
            ucAddress.EnableStreetAddress =
            ucAddress.EnableUnitAddress =
            //ucAddress.EnableAddress3 =
            ucAddress.EnableCity =
            ucAddress.EnableState =
            ucAddress.EnableZip5 =
            ucAddress.EnableZip4 =
            prov_Phone.Enabled =
            prov_Fax.Enabled = txtContactName.Enabled = txtEmail.Enabled = !regIsPending;       
        }
        else
        {
          
            ucAddress.EnableStreetAddress =
            ucAddress.EnableUnitAddress =
            //ucAddress.EnableAddress3 =
            ucAddress.EnableCity =
            ucAddress.EnableState =
            ucAddress.EnableZip5 =
            ucAddress.EnableZip4 =
            prov_Phone.Enabled =
            prov_Fax.Enabled = txtContactName.Enabled = txtEmail.Enabled = false;
             
        }
       
        //ParentTable2.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") && isEdit ? "block" : "none";
        hidIsEdit.Text = isEdit.ToString();
        Set_hidID(row);

        if (isEdit)
        {          
            ucAddress.StreetAddress = row["W9_ADDRESS1"].ToString();
            ucAddress.UnitAddress = row["W9_ADDRESS2"].ToString();
            ucAddress.City = row["W9_CITY"].ToString();
            ucAddress.State = row["W9_STATE"].ToString();
            ucAddress.Zip5 = row["W9_ZIP"].ToString();
            ucAddress.Zip4 = row["W9_EXT_ZIP"].ToString();
              if (!string.IsNullOrEmpty(Helper.GetString("W9_ADDRESS_FIRSTNAME", row)))
                txtAFirstName.Text = row["W9_ADDRESS_FIRSTNAME"].ToString();
            else
                txtAFirstName.Text = drProvider["FIRST_NAME"].ToString();
            if (!string.IsNullOrEmpty(Helper.GetString("W9_ADDRESS_MIDDLENAME", row)))
                txtAMiddleName.Text = row["W9_ADDRESS_MIDDLENAME"].ToString();
            else
                txtAMiddleName.Text = drProvider["MIDDLE_INITIAL"].ToString();

            if (!string.IsNullOrEmpty(Helper.GetString("W9_ADDRESS_LASTNAME", row)))
                txtALastName.Text = row["W9_ADDRESS_LASTNAME"].ToString();
            else
                txtALastName.Text = drProvider["LAST_NAME"].ToString();

            if (!string.IsNullOrEmpty(Helper.GetString("W9_ADDRESS_TITLE", row)))
                txtTitle.Text = row["W9_ADDRESS_TITLE"].ToString();
            else
                txtTitle.Text = drProvider["TITLE"].ToString();

            if (!string.IsNullOrEmpty(Helper.GetString("W9_ORGANIZATION_NAME", row)))
                prov_OrgName.Text = row["W9_ORGANIZATION_NAME"].ToString();
            else
                prov_OrgName.Text = drProvider["NAME"].ToString();

            if (ucAddress.State != "")
            {
                string countyName = row["W9_COUNTY"].ToString();

                ucAddress.LoadCountiesByState(ucAddress.State);
                ucAddress.County = countyName;
            }

            prov_Phone.Text = Helper.FormatPhone(row["W9_PHONE_NUMBER"].ToString());
            prov_Fax.Text = Helper.GetString("W9_FAX_NUMBER", row);
            txtContactName.Text = Helper.GetString("W9_CONTACT_NAME", row);
            txtEmail.Text = Helper.GetString("W9_EMAIL_ADDRESS", row);
            prov_address_phone.Text = Helper.FormatPhone(row["W9_ADDRESS_PHONE_NUMBER"].ToString());
            string cType = Helper.GetString("W9_CONTACT_TYPE", row);
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
               // this.rfvContactname.Enabled = true;
                this.rfvOrgName.Enabled = false;
                trOrg.Visible = false;
            }

            if (cType == CON.ContactType.Organization)
            {
                trOrg.Visible = true;
                this.rfvOrgName.Enabled = true;
                //this.rfvContactname.Enabled = true;
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
                   // this.rfvContactname.Enabled = true;
                    this.rfvOrgName.Enabled = false;
                    trOrg.Visible = false;
                }
                else
                {
                    rblContactType.SelectedValue = CON.ContactType.Organization;
                    trOrg.Visible = true;
                    rfvOrgName.Enabled = true;
                    //this.rfvContactname.Enabled = true;
                }

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

                txtContactName.Text = Helper.GetString("MAILTO_CONTACT_NAME", row);

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
                //this.rfvContactname.Enabled = true;
                this.rfvOrgName.Enabled = false;
                trOrg.Visible = false;
            }
            else
            {
                rblContactType.SelectedValue = CON.ContactType.Organization;
                trOrg.Visible = true;
                rfvOrgName.Enabled = true;
                //this.rfvContactname.Enabled = true;
            }
            hidID.Text = string.Empty;
            ucAddress.StreetAddress = string.Empty;
            ucAddress.UnitAddress = string.Empty;
            ucAddress.City = string.Empty;
            ucAddress.State = "";
            ucAddress.Zip5 = string.Empty;
            ucAddress.Zip4 = string.Empty;
            prov_Phone.Text = string.Empty;
            prov_Fax.Text = string.Empty;
            txtContactName.Text = string.Empty;
            txtEmail.Text = string.Empty;
        }
    }

    public bool SaveData()
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
                if (v != null && v.ValidationGroup.Equals("valPhoneInfo") && !string.IsNullOrEmpty(prov_Phone.Text) && !v.IsValid)
                    return false;
                else if (v != null && v.ValidationGroup.Equals("valFaxInfo") && !string.IsNullOrEmpty(prov_Fax.Text) && !v.IsValid)
                    return false;
                else if (v != null && v.ValidationGroup.Equals("valOwnerInfo") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("W9_ADDRESS1", ucAddress.StreetAddress);
        parms.Add("W9_ADDRESS2", ucAddress.UnitAddress);
        parms.Add("W9_CITY", ucAddress.City);
        parms.Add("W9_STATE", ucAddress.State);
        parms.Add("W9_ZIP", ucAddress.Zip5);
        parms.Add("W9_EXT_ZIP", ucAddress.Zip4);
        parms.Add("W9_COUNTY", ucAddress.County);
        parms.Add("W9_PHONE_NUMBER", Helper.StripNonNumerics(prov_Phone.Text));
        parms.Add("W9_ADDRESS_PHONE_NUMBER", Helper.StripNonNumerics(prov_address_phone.Text));
        parms.Add("W9_FAX_NUMBER", Helper.StripNonNumerics(prov_Fax.Text));
        parms.Add("W9_CONTACT_NAME", txtContactName.Text);
        parms.Add("W9_EMAIL_ADDRESS", txtEmail.Text);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("W9_ADDRESS_FIRSTNAME", txtAFirstName.Text);
        parms.Add("W9_ADDRESS_MIDDLENAME", txtAMiddleName.Text);
        parms.Add("W9_ADDRESS_LASTNAME", txtALastName.Text);
        parms.Add("W9_ADDRESS_TITLE", txtTitle.Text);
        parms.Add("W9_CONTACT_TYPE", rblContactType.SelectedItem.Value);
        parms.Add("W9_ORGANIZATION_NAME", prov_OrgName.Text);


        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);

        if (isEdit || !string.IsNullOrEmpty(hidID.Text))
        {
            parms.Add("W9_ADDR_MODIFIED_STATUS_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
            parms.Add("REG_SERVICE_LOCATION_ID", hidID.Text);
            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATIONcustom", parms);
        }

        ucAddress.Confirmed = false;

        return true;
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
        val.ValidationGroup = "valW9AddressInfo";
        this.Page.Validators.Add(val);
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
}