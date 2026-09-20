using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_ProviderExclusionPanel : System.Web.UI.UserControl
{
    // Public properties to bind
    public string ProviderClassification { get; set; }
    public string OrganizationName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Suffix { get; set; }
    public string SSN { get; set; }
    public string EIN { get; set; }
    public string DOB { get; set; }
    public string NPI { get; set; }
    public string DOD { get; set; }
    public string ExclusionAgencyId { get; set; }
    public string ExclusionProgram { get; set; }
    public string ExclusionAgency { get; set; }
    public string ExclusionType { get; set; }
    public string ExclusionDate { get; set; }
    public string ExclusionStatus { get; set; }
    public string ExclusionTerminationDate { get; set; }
    public string ReinstatementDate { get; set; }
    public string AdditionalDetails { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindValues();
    }

    public void BindValues()
    {
        // Top meta
        litProviderClassification.Text = EmptyOrValue(ProviderClassification);
        litExclusionAgency.Text = EmptyOrValue(ExclusionAgency);
        litExclusionAgencyBadge.Text = EmptyOrValue(ExclusionAgency);
        litExclusionProgram.Text = EmptyOrValue(ExclusionProgram);
        litExclusionStatus.Text = EmptyOrValue(ExclusionStatus);

        // Identity & Program
        litProviderClassification2.Text = EmptyOrValue(ProviderClassification);
        litOrganizationName.Text = EmptyOrValue(OrganizationName);
        litFirstName.Text = EmptyOrValue(FirstName);
        litMiddleName.Text = EmptyOrValue(MiddleName);
        litLastName.Text = EmptyOrValue(LastName);
        litSuffix.Text = EmptyOrValue(Suffix);
        litSSN.Text = EmptyOrValue(SSN, mask: true);
        litEIN.Text = EmptyOrValue(EIN, mask: true);
        litDOB.Text = EmptyOrValue(FormatDate(DOB));
        litNPI.Text = EmptyOrValue(NPI);
        litDOD.Text = EmptyOrValue(FormatDate(DOD));
        litExclusionAgencyId.Text = EmptyOrValue(ExclusionAgencyId);

        // Exclusion Details
        litExclusionProgram2.Text = EmptyOrValue(ExclusionProgram);
        litExclusionAgency2.Text = EmptyOrValue(ExclusionAgency);
        litExclusionType.Text = EmptyOrValue(ExclusionType);
        litExclusionDate.Text = EmptyOrValue(FormatDate(ExclusionDate));
        litExclusionStatus2.Text = EmptyOrValue(ExclusionStatus);
        litExclusionTerminationDate.Text = EmptyOrValue(FormatDate(ExclusionTerminationDate));
        litReinstatementDate.Text = EmptyOrValue(FormatDate(ReinstatementDate));
        litAdditionalDetails.Text = string.IsNullOrWhiteSpace(AdditionalDetails) ? "—" : AdditionalDetails;
    }

    private string EmptyOrValue(string input, bool mask = false)
    {
        if (string.IsNullOrWhiteSpace(input)) return "—";
        if (mask)
        {
            // Minimal masking: show last 4 if numeric-like
            var trimmed = input.Trim();
            if (trimmed.Length >= 4)
                return new string('•', Math.Max(0, trimmed.Length - 4)) + trimmed.Substring(trimmed.Length - 4);
        }
        return input;
    }

    private string FormatDate(string val)
    {
        if (string.IsNullOrWhiteSpace(val)) return null;
        // Allow common forms (MM/dd/yyyy, yyyy-MM-dd, etc.)
        DateTime parsed;
        if (DateTime.TryParse(val, out parsed))
            return parsed.ToString("MM/dd/yyyy");
        return val; // leave as-is if unknown
    }
}