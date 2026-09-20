using System;
using System.Data;

public partial class UserControls_MessageModalControls : System.Web.UI.UserControl
{
    public delegate void ContinueEventHandler();
    public event ContinueEventHandler ContinueEvent;
    public enum ShowMe
    {
        Default, EventHistory, VendorNumber,
        Certifications, CertSecondGrid, Licenses, Pharmacy, Medicare, PrimarySpecialty, AdditionalSpecialties, TaxonomyCode, AdditionalTaxonomyCode,
        PrimaryPracticeLocation, BillingPaymentContactInfo, CorrespondenceInformation, SatellitePracticeLocations, OrgInfo, PrimaryContactInfo, GroupAffiliations, BankingInformation,
        CertificationsHistory, CertSecondGridHistory, LicensesHistory, PharmacyHistory, MedicareHistory, PrimarySpecialtyHistory, AdditionalSpecialtiesHistory, TaxonomyCodeHistory, AdditionalTaxonomyCodeHistory,
        PrimaryPracticeLocationHistory, BillingPaymentContactInfoHistory, CorrespondenceInformationHistory, SatellitePracticeLocationsHistory, OrgInfoHistory, PrimaryContactInfoHistory, BankingInformationHistory, RemittanceInformation, RemittanceInformationHistory
    }
    private ShowMe _MessageModalFor;

    public ShowMe MessageModalFor
    {
        get { return _MessageModalFor; }
        set { _MessageModalFor = value; }
    }

    public void Show(int mode, string title, string message)
    {
        lblTitle.Text = title;
        lblMessage.Text = message;
        switch (mode)
        {
            case 1:
                // Box with Save and Cancel actions
                btnContinue.Style.Add("display", "block");
                btnContinue.Text = "Save";
                btnCancel.Text = "Cancel";
                break;
            case 2:
                // Box with no actions except to close
                btnContinue.Style.Add("display", "none");
                btnCancel.Text = "Close";
                break;
            case 3:
                // Box with Continue and Cancel actions
                btnContinue.Style.Add("display", "block");
                btnContinue.Text = "Continue";
                btnCancel.Text = "Cancel";
                break;
        }
        mpe.Show();
    }

    //The mode, title, and message parameters will change to be provider specific attributes later.
    public void Show(int mode, string title, string message, ShowMe? showMe)
    {
        CleanUp();

        switch (showMe)
        {
            case ShowMe.Default:
                pnlDefault.Visible = true;
                break;
            //case ShowMe.EventHistory:
            //    pnlEventHistory.Visible = true;
            //    break;
            case ShowMe.Certifications:
                pnlCertifications.Visible = true;
                break;
            case ShowMe.CertSecondGrid:
                pnlCertSecondGrid.Visible = true;
                break;
            case ShowMe.Licenses:
                pnlLicenses.Visible = true;
                break;
            case ShowMe.Pharmacy:
                pnlPharmacy.Visible = true;
                break;
            case ShowMe.Medicare:
                pnlMedicare.Visible = true;
                break;
            case ShowMe.PrimarySpecialty:
                pnlPrimarySpecialty.Visible = true;
                break;
            case ShowMe.AdditionalSpecialties:
                pnlAdditionalSpecialties.Visible = true;
                break;
            case ShowMe.TaxonomyCode:
                pnlTaxonomyCode.Visible = true;
                break;
            case ShowMe.AdditionalTaxonomyCode:
                pnlAdditionalTaxonomyCode.Visible = true;
                break;
            default:
                break;
        }

        Show(mode, title, message);
    }

    public void Show(int mode, string title, string message, ShowMe? showMe, DataRow row, bool isEdit)
    {
        CleanUp();

        switch (showMe)
        {
            case ShowMe.Default:
                pnlDefault.Visible = true;
                break;
            //case ShowMe.EventHistory:
            //    pnlEventHistory.Visible = true;
            //    break;
            //case ShowMe.Certifications:
            //    pnlCertifications.Visible = true;
            //    Certifications.SetupData(row, isEdit);
            //    break;
            //case ShowMe.CertSecondGrid:
            //    pnlCertSecondGrid.Visible = true;
            //    CertSecondGrid.SetupData(row, isEdit);
            //    break;
            //case ShowMe.Licenses:
            //    pnlLicenses.Visible = true;
            //    Licenses.SetupData(row, isEdit);
            //    break;
            //case ShowMe.Pharmacy:
            //    pnlPharmacy.Visible = true;
            //    Pharmacy.SetupData(row, isEdit);
            //    break;
            //case ShowMe.Medicare:
            //    pnlMedicare.Visible = true;
            //    Medicare.SetupData(row, isEdit);
            //    break;
            //case ShowMe.PrimarySpecialty:
            //    pnlPrimarySpecialty.Visible = true;
            //    ucPrimarySpecialty.LoadData(row, isEdit);
            //    break;
            //case ShowMe.AdditionalSpecialties:
            //    pnlAdditionalSpecialties.Visible = true;
            //    AdditionalSpecialties.SetupData(row, isEdit);
            //    break;
            //case ShowMe.TaxonomyCode:
            //    pnlTaxonomyCode.Visible = true;
            //    TaxonomyCode.SetupData(row, isEdit);
            //    break;
            //case ShowMe.AdditionalTaxonomyCode:
            //    pnlAdditionalTaxonomyCode.Visible = true;
            //    AdditionalTaxonomyCode.SetupData(row, isEdit);
            //    break;
            //case ShowMe.CertificationsHistory:
            //    pnlCertificationsHistory.Visible = true;
            //    break;
            //case ShowMe.CertSecondGridHistory:
            //    pnlCertSecondGridHistory.Visible = true;
            //    break;
            //case ShowMe.LicensesHistory:
            //    pnlLicensesHistory.Visible = true;
            //    break;
            //case ShowMe.PharmacyHistory:
            //    pnlPharmacyHistory.Visible = true;
            //    break;
            //case ShowMe.MedicareHistory:
            //    pnlMedicareHistory.Visible = true;
            //    break;
            //case ShowMe.PrimarySpecialtyHistory:
            //    pnlPrimarySpecialtyHistory.Visible = true;
            //    break;
            //case ShowMe.AdditionalSpecialtiesHistory:
            //    pnlAdditionalSpecialtiesHistory.Visible = true;
            //    break;
            //case ShowMe.TaxonomyCodeHistory:
            //    pnlTaxonomyCodeHistory.Visible = true;
            //    break;
            //case ShowMe.AdditionalTaxonomyCodeHistory:
            //    pnlAdditionalTaxonomyCodeHistory.Visible = true;
            //    break;
            //case ShowMe.PrimaryPracticeLocation:
            //    pnlPrimaryPracticeLocation.Visible = true;
            //    PrimaryPracticeLocation.SetupData(row, isEdit);
            //    break;
            //case ShowMe.PrimaryPracticeLocationHistory:
            //    pnlPrimaryPracticeLocationHistory.Visible = true;
            //    break;
            //case ShowMe.BillingPaymentContactInfo:
            //    pnlBillingPaymentContactInfo.Visible = true;
            //    BillingPaymentContactInfo.SetupData(row, isEdit);
            //    break;
            //case ShowMe.BillingPaymentContactInfoHistory:
            //    pnlBillingPaymentContactInfoHistory.Visible = true;
            //    pnlModal.Width = 1000;
            //    break;
            //case ShowMe.CorrespondenceInformation:
            //    pnlCorrespondenceInformation.Visible = true;
            //    pnlModal.Width = 700;
            //    CorrespondenceInformation.SetupData(row, isEdit);
            //    break;
            //case ShowMe.CorrespondenceInformationHistory:
            //    pnlCorrespondenceInformationHistory.Visible = true;
            //    pnlModal.Width = 1000;
            //    break;
            //case ShowMe.SatellitePracticeLocations:
            //    pnlSatellitePracticeLocations.Visible = true;
            //    pnlModal.Width = 700;
            //    SatellitePracticeLocations.SetupData(row, isEdit);
            //    break;
            //case ShowMe.SatellitePracticeLocationsHistory:
            //    pnlSatellitePracticeLocationsHistory.Visible = true;
            //    pnlModal.Width = 1000;
            //    break;
            //case ShowMe.OrgInfo:
            //    pnlOrgInfo.Visible = true;
            //    pnlModal.Width = 700;
            //    OrgInfo.SetupData(row, isEdit);
            //    break;
            //case ShowMe.OrgInfoHistory:
            //    pnlOrgInfoHistory.Visible = true;
            //    pnlModal.Width = 1000;
            //    break;
            //case ShowMe.PrimaryContactInfo:
            //    pnlPrimaryContactInfo.Visible = true;
            //    pnlModal.Width = 700;
            //    PrimaryContactInfo.SetupData(row, isEdit);
            //    break;
            //case ShowMe.PrimaryContactInfoHistory:
            //    pnlPrimaryContactInfoHistory.Visible = true;
            //    pnlModal.Width = 1400;
            //    break;
            //case ShowMe.BankingInformation:
            //    pnlBankingInformation.Visible = true;
            //    pnlModal.Width = 700;
            //    BankingInformation.SetupData(row, isEdit);
            //    break;
            //case ShowMe.BankingInformationHistory:
            //    pnlBankingInformationHistory.Visible = true;
            //    pnlModal.Width = 1400;
            //    break;
            //case ShowMe.GroupAffiliations:
            //    pnlGroupAffiliations.Visible = true;
            //    pnlModal.Width = 700;
            //    GroupAffiliations.SetupData(row, isEdit);
            //    break;
            //case ShowMe.VendorNumber:
            //    pnlVendorNumber.Visible = true;
            //    pnlModal.Width = 700;
            //    VendorNumber.SetupData(row, isEdit);
            //    break;
            default:
                break;
        }

        Show(mode, title, message);
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        if (ContinueEvent != null) ContinueEvent();
    }

    private void CleanUp()
    {
        pnlDefault.Visible = false;
        //pnlEventHistory.Visible = false;
        //pnlCertifications.Visible = false;
        //pnlCertSecondGrid.Visible = false;
        //pnlLicenses.Visible = false;
        //pnlPharmacy.Visible = false;
        //pnlMedicare.Visible = false;
        pnlPrimarySpecialty.Visible = false;
        //pnlAdditionalSpecialties.Visible = false;
        //pnlTaxonomyCode.Visible = false;
        //pnlAdditionalTaxonomyCode.Visible = false;
        //pnlCertificationsHistory.Visible = false;
        //pnlCertSecondGridHistory.Visible = false;
        //pnlLicensesHistory.Visible = false;
        //pnlPharmacyHistory.Visible = false;
        //pnlMedicareHistory.Visible = false;
        //pnlPrimarySpecialtyHistory.Visible = false;
        //pnlAdditionalSpecialtiesHistory.Visible = false;
        //pnlTaxonomyCodeHistory.Visible = false;
        //pnlAdditionalTaxonomyCodeHistory.Visible = false;
        //pnlPrimaryPracticeLocation.Visible = false;
        //pnlPrimaryPracticeLocationHistory.Visible = false;
        //pnlBillingPaymentContactInfo.Visible = false;
        //pnlBillingPaymentContactInfoHistory.Visible = false;
        //pnlCorrespondenceInformation.Visible = false;
        //pnlCorrespondenceInformationHistory.Visible = false;
        //pnlSatellitePracticeLocations.Visible = false;
        //pnlSatellitePracticeLocationsHistory.Visible = false;
        //pnlOrgInfo.Visible = false;
        //pnlOrgInfoHistory.Visible = false;
        //pnlPrimaryContactInfo.Visible = false;
        //pnlPrimaryContactInfoHistory.Visible = false;
        //pnlGroupAffiliations.Visible = false;
        //pnlBankingInformation.Visible = false;
        //pnlBankingInformationHistory.Visible = false;
        //pnlVendorNumber.Visible = false;
    }
}