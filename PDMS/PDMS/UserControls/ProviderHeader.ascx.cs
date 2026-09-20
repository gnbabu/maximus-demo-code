using System;
using System.Data;

public partial class UserControls_ProviderHeader : System.Web.UI.UserControl
{
    public delegate void NameLinkEventHandler(int partyId, int submitRosterId);
    public event NameLinkEventHandler NameLinkEvent;

    public bool NameLinkActive
    {
        get { return mltProviderName.ActiveViewIndex == 1; }
        set { mltProviderName.ActiveViewIndex = (value ? 1 : 0); }
    }

    public string ProviderName
    {
        get { return lnkProviderName.Text; }
    }

    public string Email
    {
        get 
        {
            if (mltEmail.ActiveViewIndex == 0) return lblEmailNormal.Text;
            else return lblEmailLarge.Text;
        }
    }

    public int PartyId
    {
        get
        {
            if (ViewState["PartyId"] == null) ViewState["PartyId"] = 0;
            return (int)ViewState["PartyId"];
        }
        set { ViewState["PartyId"] = value; }
    }

    public int SubmitRosterId
    {
        get
        {
            if (ViewState["SubmitRosterId"] == null) ViewState["SubmitRosterId"] = 0;
            return (int)ViewState["SubmitRosterId"];
        }
        set { ViewState["SubmitRosterId"] = value; }
    }

    public string NPI
    {
        get { return lblNPI.Text; }
    }

    public string SSN
    {
        get { return lblSSN.Text; }
    }

    public string ProviderId
    {
        get { return lblProviderId.Text; }
    }

    private void ClearData()
    {
        lblProviderName.Text = lnkProviderName.Text = string.Empty;
        lblCAQHID.Text = string.Empty;
        lblCredAddressStreet1.Text = string.Empty;
        lblCredAddressStreet2.Text = string.Empty;
        lblCredCity.Text = string.Empty;
        lblCredStateZip.Text = string.Empty;
        lblDOB.Text = string.Empty;
        lblEmailNormal.Text = lblEmailLarge.Text = string.Empty;
        lblLastAttestDate.Text = string.Empty;
        lblNPI.Text = string.Empty;
        lblPrimaryContactPhone.Text = string.Empty;
        lblPrimaryContactFax.Text = string.Empty;
        lblPrimaryPracticeState.Text = string.Empty;
        lblProviderId.Text = string.Empty;
        lblRosterStatus.Text = string.Empty;
        lblSSN.Text = string.Empty;
        lblPDMSStatus.Text = lblPDMSStatusDate.Text = string.Empty;
        lblCAQHStatus.Text = string.Empty;
    }

    private void AlignEmailAddress()
    {
        mltEmail.ActiveViewIndex = 0;
        if (string.IsNullOrEmpty(lblEmailLarge.Text)) return;
        if (lblEmailLarge.Text.Length > 25) mltEmail.ActiveViewIndex = 1;
    }

    private void LoadSubmitRoster(int submitRosterId, DataSet ds)
    {
        DataTable dt = ds.Tables[0];
        if (!Helper.HasRows(dt)) return;
        DataRow row = dt.Rows[0];
        lblProviderName.Text = lnkProviderName.Text = Helper.GetString("FirstName", row) + " " + Helper.GetString("LastName", row);
        lblCAQHID.Text = Helper.GetString("CAQHId", row);
        lblCredAddressStreet1.Text = Helper.GetString("Street1", row);
        lblCredAddressStreet2.Text = Helper.GetString("Street2", row);
        lblCredCity.Text = Helper.GetString("City", row);
        lblCredStateZip.Text = Helper.GetString("State", row) + " " + Helper.GetString("Zip5", row) +
            (!string.IsNullOrEmpty(Helper.GetString("Zip4", row)) ? "-" + Helper.GetString("Zip4", row) : string.Empty);
        lblPrimaryPracticeState.Text = Helper.GetString("PracticeState", row);
        lblDOB.Text = Convert.ToDateTime(Helper.GetString("BirthDate", row)).ToString("MM/dd/yyyy");
        lblEmailNormal.Text = lblEmailLarge.Text = !string.IsNullOrEmpty(Helper.GetString("Email", row)) ? Helper.GetString("Email", row).Trim() : string.Empty;
        AlignEmailAddress();
        lblLastAttestDate.Text = string.Empty;
        lblNPI.Text = Helper.GetString("NPI", row);
        lblPrimaryContactPhone.Text = Helper.FormatPhone(Helper.GetString("Phone", row)) +
            (!string.IsNullOrEmpty(Helper.GetString("PhoneExt", row)) ? " Ext: " + Helper.GetString("PhoneExt", row) : string.Empty);
        lblPrimaryContactFax.Text = string.Empty;
        lblProviderId.Text = string.Empty;              // Does not have a value because it is not in our PDMS Providers
        lblRosterStatus.Text = Helper.GetString("RosterStatus", row);
        lblSSN.Text = Helper.FormatSSN(Helper.GetString("SSN", row));
        lblPDMSStatus.Text = Helper.GetString("PDMSStatus", row);
        if (!string.IsNullOrEmpty(Helper.GetString("PDMSStatusDate", row))) lblPDMSStatusDate.Text = Helper.GetDateTime("PDMSStatusDate", row).ToString("MM/dd/yyyy hh:mm tt");
        lblCAQHStatus.Text = Helper.GetString("CAQHStatus", row);
        ucHeaderProviderInfo.Header = "Provider Information (" + submitRosterId.ToString() + ")";
    }

    private void LoadPDMSData(int partyId, DataSet ds)
    {
        DataTable dt = ds.Tables[0];
        if (!Helper.HasRows(dt)) return;
        DataRow row = dt.Rows[0];
        lblProviderName.Text = lnkProviderName.Text = Helper.GetString("FirstName", row) + " " + Helper.GetString("LastName", row);
        lblCAQHID.Text = Helper.GetString("CAQHId", row);
        lblCredAddressStreet1.Text = Helper.GetString("CredentialAddress1", row);
        lblCredAddressStreet2.Text = Helper.GetString("CredentialAddress2", row);
        lblCredCity.Text = Helper.GetString("CredentialCity", row);
        lblCredStateZip.Text = Helper.GetString("CredentialState", row) + " " + Helper.GetString("CredentialZip", row) +
            (!string.IsNullOrEmpty(Helper.GetString("CredentialExtendedZip", row)) ? "-" + Helper.GetString("CredentialExtendedZip", row) : string.Empty);
        lblPrimaryPracticeState.Text = Helper.GetString("PracticeState", row);
        lblDOB.Text = Helper.FormatDate2(Helper.GetString("BirthDate", row));
        lblEmailNormal.Text = lblEmailLarge.Text = !string.IsNullOrEmpty(Helper.GetString("CredentialEmailAddress", row)) ? Helper.GetString("CredentialEmailAddress", row).Trim() : string.Empty;
        AlignEmailAddress();
        lblLastAttestDate.Text = !string.IsNullOrEmpty(Helper.GetString("LastAttestDate", row)) ?
            Convert.ToDateTime(Helper.GetString("LastAttestDate", row)).ToString("MM/dd/yyyy") : string.Empty;
        lblNPI.Text = Helper.GetString("NPI", row);
        lblPrimaryContactPhone.Text = Helper.FormatPhone(Helper.GetString("CredentialPhoneAreaCode", row) + Helper.GetString("CredentialPhoneNumber", row));
        lblPrimaryContactFax.Text = Helper.FormatPhone(Helper.GetString("CredentialFaxAreaCode", row) + Helper.GetString("CredentialFaxNumber", row));
        lblProviderId.Text = Helper.GetString("ProviderId", row);
        lblRosterStatus.Text = Helper.GetString("RosterStatus", row);
        lblSSN.Text = Helper.FormatSSN(Helper.GetString("TaxID", row));
        lblPDMSStatus.Text = Helper.GetString("PDMSStatus", row);
        if (!string.IsNullOrEmpty(Helper.GetString("PDMSStatusDate", row))) lblPDMSStatusDate.Text = Helper.GetDateTime("PDMSStatusDate", row).ToString("MM/dd/yyyy hh:mm tt");
        lblCAQHStatus.Text = Helper.GetString("CAQHStatus", row);
        ucHeaderProviderInfo.Header = "Provider Information (" + partyId.ToString() + ")";
    }

    public void LoadData(int inpPartyId, int inpSubmitRosterId, DataSet ds)
    {
        ClearData();
        PartyId = inpPartyId;
        SubmitRosterId = inpSubmitRosterId;
        if (PartyId > 0) LoadPDMSData(PartyId, ds);
        else LoadSubmitRoster(SubmitRosterId, ds);
    }

    protected void lnkProviderName_Click(object sender, EventArgs e)
    {
        if (NameLinkEvent != null) NameLinkEvent(PartyId, SubmitRosterId);
    }
}