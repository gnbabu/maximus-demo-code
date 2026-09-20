using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_FirstLoginDetails : System.Web.UI.Page
{
    private static Guid LoggedinUserID;
    #region svc
    private PDMSService.PDMSServiceClient _svc;
    private PDMSService.PDMSServiceClient svc
    {
        get
        {
            if (_svc == null)
            {
                _svc = new PDMSService.PDMSServiceClient();
            }

            return _svc;
        }
    }
    #endregion
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            Page.Theme = "Modernization";
        }
        else
        {
            Page.Theme = "Default";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        {
           hdnUname.Value = SessionVarRetriever.UserIdSelected;
        }
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        Page.Validate("flValGroup");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v.ValidationGroup.Equals("flValGroup") && !v.IsValid)
                    return;
            }
            catch
            {
                continue;
            }
        }

        bool EmailSent;
        MembershipUser user = Membership.GetUser(Helper.GetUserName(hdnUname.Value));
        DataSet ds = svc.GetUserAccountInformation(hdnUname.Value.ToString());
        if(Helper.HasRows(ds))
        {
            if(txtMedicaidID.Text == ds.Tables[0].Rows[0]["MEDICAID_ID"].ToString() && txtEmail.Text == user.Email)
            {
                string password = Membership.GeneratePassword(10, 2);
                EmailSent = SendNotificationToProvider(user, password);
                if(EmailSent)
                {
                    user.IsApproved = true;
                    Membership.UpdateUser(user);
                    user.UnlockUser();
                    if (user.ChangePassword(password, password))
                    {
                        //insert into temp password generated
                        pnlSuccess.Visible = true;
                        pnlValidationSummary.Visible = false;
                        pnlftlogin.Visible = false;
                    }
                }
            }
            else
            {
                pnlValidationSummary.Visible = true;
                lblError.Visible = true;
                lblError.Text = "Email or Medicaid ID entered does not match out records. Please re-enter correct values or call helpdesk.";
                upValidationSummary.Update();
                return;
            }
        }
    }

    private bool SendNotificationToProvider(MembershipUser user, string tempPswd)
    {
        /* Send Email To Provider */
        bool rtn = false;
        try
        {
            string urlFromDB = Helper.GetAppSettingFromDB("PDMS-URL", string.Empty);
            Uri uri = new Uri(urlFromDB);
            string registerUrl = string.Format("?id={0:N}", user.ProviderUserKey.ToString().Trim());
            registerUrl = new Uri(uri, registerUrl).ToString();
            int dummyRegID = 0;
            svc.NotifyProviderTempPassword(txtEmail.Text, user.UserName, registerUrl, tempPswd, dummyRegID, LoggedinUserID);

            rtn = true;
        }
        catch (Exception ex)
        {
            // Log the error - This means the email did not go out 
           // RecordException(ex, "CreateAdditionalUserData - NotifyProviderAccountCreation", false);
            string errMsg = ex.Message + " [Send email Temporary Password]";
            CoreException.ThrowException(new Exception(errMsg));
            pnlValidationSummary.Visible = true;
            lblError.Visible = true;
            lblError.Text = errMsg;
            upValidationSummary.Update();
            rtn = false;
        }

        return rtn;
    }

    protected void btnCancel_Click (object sender, EventArgs e)
    {
        Response.Redirect("~/Account/Login.aspx");
    }

    protected void Validate_cvCEmailMatch(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = txtEmail.Text.Equals(txtConfirmEmail.Text);
    }
}