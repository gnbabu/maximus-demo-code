using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Web;
using System.Web.UI;

public partial class PopupControls_RevalidationTesting : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnUpdateEndDate_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            var regId = Convert.ToInt32(txtRevalidationRegId.Text.Trim());
            var endDate = Convert.ToDateTime(txtRevalidationEndDate.Text.Trim());

            RegistrationController.UpdateRegProviderEnrollmentStatusCode(regId, "1", DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name), endDate);
        }
    }

    protected void btnAppSettingKey_Click(object sender, EventArgs e)
    {
        lblAppSettingKey.Text = "";
        if (AppSettings.Remove(txtAppSettingKey.Text))
        {
            lblAppSettingKey.Text = "Key Refreshed Successfully";
        }
        else
        {
            lblAppSettingKey.Text = "Key doesn't Exists";
        }
    }

}