using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using System;
using System.Data;
using System.Linq;
using System.Web;

public partial class PopupControls_HospiceRecipientInformation : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.WorkflowPage.HospiceRequestResponse != null)
        {
            FillRecipientInformation();
        }
    }

    private void FillRecipientInformation()
    {
        if (this.WorkflowPage.SearchHospiceResponse != null && this.WorkflowPage.SearchHospiceResponse.Response != null && this.WorkflowPage.HospiceTrackNo != "0" && this.WorkflowPage.HospiceTrackNo != "")
        {
            var info = this.WorkflowPage.SearchHospiceResponse.Response.Where(x => x.HospiceTrackNo == Convert.ToInt32(this.WorkflowPage.HospiceTrackNo)).FirstOrDefault();
            if (info != null)
            {
                lblMedicaidBillingNumber.Text = this.WorkflowPage.MedicaidBillingNumber;
                lblLastName.Text = info.RecipName.Split(',')[0];
                lblFirstname.Text = info.RecipName.Split(',')[1];
                lblDateofBirth.Text = Convert.ToString(info.RecipBirthDate.ToString("MM/dd/yyyy"));
                lblSubmissionDate.Text = Convert.ToString(info.SubmissionDate.ToString("MM/dd/yyyy"));

                if (string.IsNullOrEmpty(this.WorkflowPage.RecipientDateOfBirth))
                {
                    this.WorkflowPage.RecipientDateOfBirth = info.RecipBirthDate.ToString("MM/dd/yyyy");
                }

                //Call Recipient service to get address info
                var ri = this.WorkflowPage.RecipientInformation;
                if (this.WorkflowPage.RecipientInformation == null)
                {
                    string medicaidID = this.WorkflowPage.MedicaidID;
                    Guid userId = Helper.GetUserId(HttpContext.Current.User.Identity.Name.ToString());
                    ri = GetRecipientInformation(this.WorkflowPage.MedicaidBillingNumber, medicaidID, userId, Convert.ToString(info.RecipBirthDate.ToString("MM/dd/yyyy")));
                }
                if (ri != null)
                {
                    lbleStreetAddress.Text = ri.AddressLine1;
                    lblCityStateZip.Text = ri.City + " " + ri.StateCode + " " + ri.ZipCode5;
                    lblCountyofRecord.Text = "";//There is no county of record in the new service
                }
            }
        }
        else
        {
            var ri = this.WorkflowPage.RecipientInformation;
            if (this.WorkflowPage.RecipientInformation == null)
            {
                string medicaidID = this.WorkflowPage.MedicaidID;
                Guid userId = Helper.GetUserId(HttpContext.Current.User.Identity.Name.ToString());
                ri = GetRecipientInformation(this.WorkflowPage.MedicaidBillingNumber, medicaidID, userId, this.WorkflowPage.RecipientDateOfBirth);
            }

            if (ri != null)
            {
                string middleName = string.Empty;
                lblMedicaidBillingNumber.Text = this.WorkflowPage.MedicaidBillingNumber;
                lblLastName.Text = ri.LastName;
                lbleStreetAddress.Text = ri.AddressLine1;
                if (ri.MiddleName != null)
                {
                    middleName = (string)ri.MiddleName;
                    middleName = middleName.Length > 1 ? ", " + middleName.Substring(0, 1) : middleName;
                }
                lblFirstname.Text = ri.FirstName + middleName;
                lblDateofBirth.Text = ri.DateOfBirth != null && ri.DateOfBirth.HasValue ? ri.DateOfBirth.Value.ToString("MM/dd/yyyy") : "";
                lblCityStateZip.Text = ri.City + " " + ri.StateCode + " " + ri.ZipCode5;
                lblCountyofRecord.Text = ""; //There is no county of record in new service
            }
        }
    }

    private RecipientInformation GetRecipientInformation(string medicaidbillingnumber, string medicaidId, Guid userId, string dateofBirth)
    {
        RecipientEligibilitySearchReqRes recipientEligibility = new RecipientEligibilitySearchReqRes();
        var ri = recipientEligibility.GetRecipientInformation(medicaidbillingnumber, medicaidId, userId, dateofBirth, "HospiceEligibility");
        if (ri != null)
            return ri;
        else
            return null;
    }
}