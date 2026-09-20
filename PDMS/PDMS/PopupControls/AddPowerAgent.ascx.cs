using System;
using System.Data;
using System.Web.UI.WebControls;


public partial class PopupControls_AddPowerAgent : System.Web.UI.UserControl
{
    public event EventHandler<PowerAgentEventArgs> PowerAgentAdded;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ClearForm();
        }
    }

    protected void txtOhId_TextChanged(object sender, EventArgs e)
    {
        // Clear the email and username labels when OH ID changes
        lblOhIdEmail.Text = "";
        lblOhIdUserName.Text = "";

        string ohId = txtOhId.Text.Trim();
        if (!string.IsNullOrEmpty(ohId))
        {
            // TODO: Fetch OH ID details from database/service
            // For now, simulating the data retrieval
            OhIdUserInfo userInfo = GetOhIdUserInfo(ohId);
            if (userInfo != null)
            {
                lblOhIdEmail.Text = userInfo.Email;
                lblOhIdUserName.Text = userInfo.UserName;
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string ohId = txtOhId.Text.Trim();
            bool hasAccessManagement = chkAccessManagement.Checked;
            string email = lblOhIdEmail.Text;
            string userName = lblOhIdUserName.Text;

            // Create event args and fire the event
            PowerAgentEventArgs eventArgs = new PowerAgentEventArgs
            {
                OhId = ohId,
                Email = email,
                UserName = userName,
                HasAccessManagement = hasAccessManagement
            };

            // C# 5.0 compatible event invocation
            if (PowerAgentAdded != null)
            {
                PowerAgentAdded(this, eventArgs);
            }
        }
    }

    protected void cvOhIdExists_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string ohId = args.Value;
        if (ohId != null)
        {
            ohId = ohId.Trim();
        }

        if (string.IsNullOrEmpty(ohId))
        {
            args.IsValid = true;
            return;
        }

        // TODO: Implement actual validation against database
        // For now, simulate validation - assume OH ID exists if it's numeric and 8 digits
        args.IsValid = IsValidOhId(ohId);
    }

    protected void cvOhIdRole_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string ohId = args.Value;
        if (ohId != null)
        {
            ohId = ohId.Trim();
        }

        if (string.IsNullOrEmpty(ohId))
        {
            args.IsValid = true;
            return;
        }

        // TODO: Implement actual role validation against database
        // For now, simulate validation
        args.IsValid = IsProviderAgent(ohId);
    }

    protected void cvOhIdNotDuplicate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string ohId = args.Value;
        if (ohId != null)
        {
            ohId = ohId.Trim();
        }

        if (string.IsNullOrEmpty(ohId))
        {
            args.IsValid = true;
            return;
        }

        // TODO: Implement actual duplicate check against database
        // For now, simulate validation
        args.IsValid = !IsAlreadyPowerAgent(ohId);
    }

    public void ClearForm()
    {
        txtOhId.Text = "";
        lblOhIdEmail.Text = "";
        lblOhIdUserName.Text = "";
        chkAccessManagement.Checked = false;
    }

    public string CancelButtonClientID
    {
        get
        {
            return btnCancel.ClientID;
        }
    }

    // Private helper methods - these would be replaced with actual database calls
    private OhIdUserInfo GetOhIdUserInfo(string ohId)
    {
        // TODO: Replace with actual database call
        // Simulating data retrieval
        if (IsValidOhId(ohId))
        {
            return new OhIdUserInfo
            {
                Email = string.Format("{0}@email.com", "OHID"),
                UserName = "OHID User"
            };
        }
        return null;
    }

    private bool IsValidOhId(string ohId)
    {
        // TODO: Replace with actual database validation
        // Simulate validation - assume valid if numeric and 8 digits
        if (!string.IsNullOrEmpty(ohId) && ohId.Length == 8)
        {
            long result;
            return long.TryParse(ohId, out result);
        }
        return false;
    }

    private bool IsProviderAgent(string ohId)
    {
        // TODO: Replace with actual database role check
        // For simulation, assume all valid OH IDs are provider agents
        return IsValidOhId(ohId);
    }

    private bool IsAlreadyPowerAgent(string ohId)
    {
        // TODO: Replace with actual database duplicate check
        // For simulation, assume no duplicates for now
        return false;
    }

    // Helper classes
    private class OhIdUserInfo
    {
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}