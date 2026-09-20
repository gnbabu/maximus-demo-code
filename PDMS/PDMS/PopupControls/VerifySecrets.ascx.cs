using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_VerifySecrets : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnGetSecretsKey_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(txtRegionID.Text) && !string.IsNullOrWhiteSpace(txtDictionaryID.Text) && !string.IsNullOrWhiteSpace(txtNameID.Text))
            {
                var sec = new SecretsManager(txtRegionID.Text);
                var result = sec.GetSuperSecretPassword(txtDictionaryID.Text);
                result.Wait();
                string client_secret = result.Result[txtNameID.Text];

                if (string.IsNullOrEmpty(client_secret))
                {
                    AddError("There is no Key available.");
                }
                else
                {
                    txtValueID.Text = client_secret;
                }
            }
            else
            {
                AddError("Please enter the required fields.");
            }
        }
        catch(Exception ex)
        {
            AddError("Error: " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
        }
    }

    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "VerifySecretsVS";
        this.Page.Validators.Add(val);
    }
}