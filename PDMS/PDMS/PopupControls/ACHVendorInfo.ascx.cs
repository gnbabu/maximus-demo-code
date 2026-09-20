using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public partial class PopupControls_ACHVendorInfo : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override void LoadData(DataRow dr)
    {
        hdnRegVendorInfoID.Value = txtLocationCode.Text = txtVendorNumber.Text = string.Empty;
        nbSequenceNumber.Text = string.Empty;

        if (dr != null)
        {
            hdnRegVendorInfoID.Value = Helper.GetString("REG_ACH_EDISON_ID", dr);
            txtLocationCode.Text = Helper.GetString("LOCATION_CODE", dr);
            txtVendorNumber.Text = Helper.GetString("VENDOR_NUMBER", dr);
            nbSequenceNumber.Text = Helper.GetString("SEQUENCE_NUMBER", dr);
        }
    }

    public void SaveData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("LOCATION_CODE", txtLocationCode.Text);
        parms.Add("VENDOR_NUMBER", txtVendorNumber.Text);
        parms.Add("SEQUENCE_NUMBER", nbSequenceNumber.Text);
        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!string.IsNullOrEmpty(hdnRegVendorInfoID.Value))
        {
            // Update
            parms.Add("REG_ACH_EDISON_ID", hdnRegVendorInfoID.Value);
            psc.UpdateRegistrationDataTable("ACH_EDISON", parms);
        }
        else
        {
            // Insert
            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            psc.InsertRegistrationDataTable("ACH_EDISON", parms);
        }

        // Now go update PDMS to keep Registration and PDMS in sync
        DataRow dr = Registration.GetRegistration(this.WorkflowPage.RegistrationId);
        parms = new Dictionary<string, string>();
        parms.Add("RegID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PartyID", Helper.GetString("PARTY_ID", dr));
        psc.UpdateRegistrationDataWithParams("usp_TransferRegToLive_ACH_EDISON", parms);
    }
}