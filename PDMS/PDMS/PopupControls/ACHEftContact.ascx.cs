using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public partial class PopupControls_ACHEftContact : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override void LoadData(DataRow dr)
    {
        //ParentTable.Rows[0].Cells[2].Style["display"] = Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) ? "block" : "none";
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

    public void SaveData()
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
}