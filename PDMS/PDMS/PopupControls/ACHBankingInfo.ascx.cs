using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_ACHBankingInfo : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override void LoadData(DataRow dr)
    {
        
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetAccountTypeEntity();
        ddlAccountTypeEntity.DataSource = ds.Tables[""]; ;

        foreach (DataRow row in ds.Tables["ACCOUNT_TYPE_ENTITY"].Rows)
        {
            ListItem item = new ListItem(row["ACCOUNT_TYPE_ENTITY_DISPLAY"].ToString(), row["ACCOUNT_TYPE_ENTITY_ID"].ToString());
            ddlAccountTypeEntity.Items.Add(item);
        }
        
        hdnRegAchRequestID.Value = txtBankName.Text = string.Empty;

        nbABANumber.Text = nbAccountNumber.Text = nbConfirmABANumber.Text = nbConfirmAccountNumber.Text = string.Empty;
        rblCheckingSavings.SelectedIndex = -1;
        if (this.WorkflowPage.IsWaiverServiceProvider)
        {
            
            ddlAccountTypeEntity.Attributes.Add("style", "display:block;");

           
        }
        else
        {
            lblAccountTypeEntity.Attributes.Add("style", "display:none;"); 
            ddlAccountTypeEntity.Attributes.Add("style", "display:none;");
            RFVAccountEntityType.Enabled = false;
           
        }
        if (dr != null)
        {
            hdnRegAchRequestID.Value = Helper.GetString("REG_ACH_REQUEST_ID", dr);
            txtBankName.Text = Helper.GetString("BANK_NAME", dr);
            
            if (Helper.GetString("ACCOUNT_TYPE_ENTITY_ID", dr) != "" && Helper.GetString("ACCOUNT_TYPE_ENTITY_ID", dr) != null)
            {
                ddlAccountTypeEntity.SelectedValue = Helper.GetString("ACCOUNT_TYPE_ENTITY_ID", dr);
            }
           
            nbABANumber.Text = nbConfirmABANumber.Text = Helper.GetString("ABA_NUMBER", dr);
            string accountNumber = Helper.GetString("ACCOUNT_NUMBER", dr);
            if (!Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
            {
                // Mask it if not in FA1 or FA2
                accountNumber = Helper.MaskValue(accountNumber, 4);
            }
            nbAccountNumber.Text = nbConfirmAccountNumber.Text = accountNumber;
            if (!string.IsNullOrEmpty(Helper.GetString("ACCOUNT_TYPE_ID", dr))) rblCheckingSavings.SelectedValue = Helper.GetString("ACCOUNT_TYPE_ID", dr);

			//LoadData for EFT Contact Details

			hdnRegEftContactID.Value = txtEFTContactFirstName.Text = txtEFTContactMiddleName.Text = txtEFTContactLastName.Text = txtEmail.Text = txtPhoneExt.Text = txtPhoneNo.Text = string.Empty;

			if (dr != null)
			{
				hdnRegEftContactID.Value = Helper.GetString("REG_ACH_CONTACT_ID", dr);
				txtEFTContactFirstName.Text = Helper.GetString("FIRST_NAME", dr);
				txtEFTContactMiddleName.Text = Helper.GetString("MIDDLE_NAME", dr);
				txtEFTContactLastName.Text = Helper.GetString("LAST_NAME", dr);
				txtEmail.Text = Helper.GetString("EMAIL_ADDRESS", dr);
				txtPhoneExt.Text = Helper.GetString("PHONE_EXTENSION", dr);
				txtPhoneNo.Text = Helper.FormatPhone(Helper.GetString("PHONE_NUMBER", dr));
				txtFaxNumber.Text = Helper.GetString("FAX_NUMBER", dr);
			}
		}
        
    }

    public void SaveData()
    {
        this.Page.Validate("valBankingInfo1");
        if (Page.IsValid)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ACH_REQUEST");

            // If entering on this page make sure that INTEND_TO_RECEIVE_MCC and  WISH_TO_RECEIVE_ACH are set to 1 since that's the only way you can be entering this.
            // For new records it would be null when the user selects yes on the page
            parms.Add("INTEND_TO_RECEIVE_MCC", "1");
            parms.Add("WISH_TO_RECEIVE_ACH", "1");

            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("ProcessId", this.WorkflowPage.WF_ProcessID.ToString());
            parms.Add("BANK_NAME", txtBankName.Text);
            parms.Add("ABA_NUMBER", nbABANumber.Text);
            parms.Add("ACCOUNT_NUMBER", nbAccountNumber.Text);
            parms.Add("ACCOUNT_TYPE_ID", rblCheckingSavings.SelectedValue);
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            if (this.WorkflowPage.IsWaiverServiceProvider)
            {
                parms.Add("ACCOUNT_TYPE_ENTITY_ID", ddlAccountTypeEntity.SelectedValue);
            }
            
            if (Helper.HasRows(ds))
            {
                // Update
                parms.Add("REG_ACH_REQUEST_ID", Helper.GetString("REG_ACH_REQUEST_ID", ds.Tables[0].Rows[0]));
                if((this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.NewReg || this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.ChangeProviderType) 
                     && this.WorkflowPage.CurrentTaskName == "Provider Data Entry")
                    psc.UpdateRegistrationDataTable("ACH_REQUEST", parms);
                else
                    psc.UpdateRegistrationDataTable("ACH_REQUESTCustom", parms);
            }
            else
            {
                parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                // Insert
                psc.InsertRegistrationDataTable("ACH_REQUESTCustom", parms);
            }
        }

    }

	public void EFTSaveData()
	{
		Dictionary<string, string> parms = new Dictionary<string, string>();
		parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
		parms.Add("FIRST_NAME", txtEFTContactFirstName.Text);
		parms.Add("MIDDLE_NAME", txtEFTContactMiddleName.Text);
		parms.Add("LAST_NAME", txtEFTContactLastName.Text);
		parms.Add("EMAIL_ADDRESS", txtEmail.Text);
		parms.Add("PHONE_EXTENSION", txtPhoneExt.Text);
		parms.Add("PHONE_NUMBER", Helper.StripNonNumerics(txtPhoneNo.Text));
		parms.Add("FAX_NUMBER", Helper.StripNonNumerics(txtFaxNumber.Text));
		parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
		parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
		parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

		PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
		if (!string.IsNullOrEmpty(hdnRegEftContactID.Value))
		{
			// Update
			parms.Add("REG_ACH_CONTACT_ID", hdnRegEftContactID.Value);
			psc.UpdateRegistrationDataTable("ACH_CONTACT", parms);
		}
		else
		{
			// Insert
			parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
			parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
			psc.InsertRegistrationDataTable("ACH_CONTACT", parms);
		}
	}
	public bool ValidatenbABANumber()
    {
        bool isGood = true;
        int num = Convert.ToInt32(nbABANumber.Text);
        string str = nbABANumber.Text;
        int len = str.Length;
        int sum1 = 0;
        if (len == 9) {
            sum1 = (Convert.ToInt32(str[7].ToString()) * 7) + (Convert.ToInt32(str[6].ToString()) * 3) + (Convert.ToInt32(str[5].ToString())) + (Convert.ToInt32(str[4].ToString()) * 7) + (Convert.ToInt32(str[3].ToString()) * 3) + (Convert.ToInt32(str[2].ToString())) + (Convert.ToInt32(str[1].ToString()) * 7) + (Convert.ToInt32(str[0].ToString()) * 3);
            int temp = sum1 / 10;
            int validValue = sum1 - (temp * 10);
            if (validValue == 0)
                sum1 = validValue;
            else
                sum1 = 10 - validValue;
            int returnCode = 0;
            if (sum1 != Convert.ToInt32(str[8].ToString()))
                returnCode = 1;
            if (returnCode != 0) {
                AddError("Invalid Routing Number");
                isGood = false;
            }
            for (int i = 0; i < Page.Validators.Count; i++)
            {
                BaseValidator v;
                try
                {
                    v = Page.Validators[i] as BaseValidator;
                    if ((v.ValidationGroup.Equals("valBankingInfo1") && !v.IsValid))
                        return false;
                }
                catch
                {
                    continue;
                }
            }   
        }
        return isGood;
    }
    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valBankingInfo1";
        this.Page.Validators.Add(val);
    }
    

}