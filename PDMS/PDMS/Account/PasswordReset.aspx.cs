using MAXIMUS.Core.Libraries;
using MAXIMUS.Services.PDMS.MembershipProvider;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_PasswordReset : System.Web.UI.Page
{

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
        if (!IsPostBack)
        {
            RP02.Focus();
            {
                lblHidTry.Text = "1";

                string requestGuidInString = Request.QueryString["id"];
                string Updateuserid = null;
                if (!string.IsNullOrWhiteSpace(requestGuidInString))
                {

                    Guid requestGuid;
                    if (Guid.TryParse(requestGuidInString, out requestGuid))
                    {
                        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
               
                        DataSet ds = svc.IsActiveReset(requestGuidInString);

                         if(ds.Tables.Count > 0)
                         {

                             if (ds.Tables[0].Rows.Count > 0)
                             {
                                 Updateuserid = ds.Tables[0].Rows[0]["UserName"].ToString();
                             } 
                         
                         }
                         if (string.IsNullOrEmpty(Updateuserid))
                         {
                             lblPasswordExists.Visible = true;
                             lblPasswordExists.Text = "Password link expired please try again.";

                         }
                         else
                         {
                             PanelNewPassword.Visible = true;
                             pnlStep2.Visible = false;
                             pnlStep1.Visible = false;
                             RP02.Text = Updateuserid;
                         }                  

                  }             

                }

            }
        }
    }

    protected void cvRP02_ServerValidate(Object sender, ServerValidateEventArgs e)
    {
        string username = RP02.Text.Trim();

        if (string.IsNullOrEmpty(username) || Membership.GetUser(username) == null)
        {
            e.IsValid = false;
        }
        else
        {
            e.IsValid = true;
        }
    }

    protected void RP03_Validating(Object sender, ServerValidateEventArgs e)
    {
        try
        {
            string username = RP02.Text.Trim();
            string email = RP03.Text.Trim();
            string user = Membership.GetUserNameByEmail(email);
            MembershipUser dbuser = Membership.GetUser(username);

            System.Net.Mail.MailAddress mail = new System.Net.Mail.MailAddress(email);

            bool isGood = true;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(username) || dbuser == null)
            {
                isGood = false;
            }

            if(dbuser != null)
            {
                if (!string.Equals(dbuser.Email, email, StringComparison.InvariantCultureIgnoreCase))
                {
                    isGood = false;
                }
            }

            e.IsValid = isGood;
        }
        catch
        {
            e.IsValid = false;
        }
    }

    protected void RP05_Validating(Object sender, ServerValidateEventArgs e)
    {
        e.IsValid = IsAnswerCorrect();
    }

    protected void RP06_Validating(Object sender, ServerValidateEventArgs e)
    {
        e.IsValid = Regex.IsMatch(RP06.Text, @"^(?=.*[0-9])(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`]).{8,20}$");
    }

    protected void RP07_Validating(Object sender, ServerValidateEventArgs e)
    {
        e.IsValid = RP06.Text.Equals(RP07.Text);
    }

    protected void RP09_Validating(Object sender, ServerValidateEventArgs e)
    {
        e.IsValid = IsCaptchaValid();
    }

    private bool IsCaptchaValid()
    {
        Captcha1.ValidateCaptcha(RP09.Text);

        if (Captcha1.UserValidated)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void btnStep1_Click(object sender, EventArgs e)
    {
        Page.Validate("Step1");
        if (!Page.IsValid)
            return;
        else
        {
            pnlStep1.Visible = false;
            pnlStep2.Visible = true;
            pnlLockedOut.Visible = false;
            MembershipUser user = Membership.GetUser(RP02.Text);
            //check for Inactivity days limit.

            bool isInactive = Helper.IsPasswordExpiredAndInactivated(RP02.Text);

            if (string.Equals( user.Email, RP03.Text, StringComparison.InvariantCultureIgnoreCase) && user.IsApproved )
            {
                SetSecurityQuestion();
            }
            else
            {
                pnlLockedOut.Visible = false;
                pnlStep1.Visible = false;
                pnlStep2.Visible = false;
                if (isInactive)
                {
                    pnlLockedOut.Visible = true;
                    ltrlLockedOut.Text = (string)GetGlobalResourceObject("BrandingResource", "pnlLockedInactivatedMsg");
                }
            }
        }

        upReset.Update();
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        try
        {
            //bool capValid = IsCaptchaValid();

            MembershipUser user = Membership.GetUser(RP02.Text);
            
            if (user != null)
            {
                user.IsApproved = true;
                Membership.UpdateUser(user);              
                user.UnlockUser();              

            }

            switch (((Button)sender).ID)
            {
                case "btnStep2":
                    {
                        Page.Validate("Step2");
                        if (!Page.IsValid)
                            return;

                        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

                        if (user.ChangePassword(user.ResetPassword(), RP06.Text))
                        {
                            // TODO: EDV How do we get the registration id here. Do we need it?
                            svc.NotifyPasswordReset(user.Email, user.UserName, AppSettings.Get("PDMS-URL"), 0, (Guid)user.ProviderUserKey);
                        }
                        else

                        {
                            Console.WriteLine("");
                        }

                        //moved from pl id:111 for bug password requirements
                        pnlStep2.Visible = false;
                        PanelNewPassword.Visible = false;
                        pnlSuccess.Visible = true;
                        btnLogin.Visible = true;
                    }
                    break;

                case "btnLogin":
                    {
                        Response.Redirect(Helper.RedirectLoginURL(), true);
                    }

                    break;
                default:
                    break;
            }

            upReset.Update();
        }
        catch (PasswordException ex)
        {
            if (ex.Message == "PasswordExists")
            {
                lblPasswordExists.Visible = true;
                lblPasswordExists.Text = "Passwords used in the past 12 months cannot be used.";
            }
            if (ex.Message == "PasswordMatchFailed")
            {
                lblPasswordExists.Visible = true;
                lblPasswordExists.Text = "An error occurred!";
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnModalOk.Visible = true;
        btnModalCancel.Visible = true;
        mpeChangesSaved.Show();
    }

    protected void btnModalOk_Click(object sender, EventArgs e)
    {
        mpeChangesSaved.Hide();
        Response.Redirect(Helper.RedirectLoginURL());
    }

    private void SetSecurityQuestion()
    {
        PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient();
        DataSet ds = client.GetUserSecurityQuestionsByNameEmail(RP02.Text, RP03.Text);

        if(ds.Tables.Count > 0)
        {
            if(ds.Tables[0].Rows.Count > 0)
            {
                lblRP05.Text = ds.Tables[0].Rows[0]["Question" + lblHidTry.Text].ToString();
            }
        }
    }

    private bool IsAnswerCorrect()
    {
        PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient();
        DataSet ds = client.GetUserSecurityQuestionsByNameEmail(RP02.Text, RP03.Text);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return (string.Compare(ds.Tables[0].Rows[0]["Answer" + lblHidTry.Text].ToString().Trim(), RP05.Text, StringComparison.InvariantCultureIgnoreCase) == 0);
            }
        }

        return false;
    }

    private void LockUser()
    {
        MembershipUser user = Membership.GetUser(RP02.Text);
        if (user != null)
        {
           // user.IsApproved = false;
           // Membership.UpdateUser(user);
        }
    }
   
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        // check the security answer correct or not
        //THEN ONLY CONTINUE

            Page.Validate("Step2");
            if (!Page.IsValid)
            return;
            RP05.Focus();

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            int myTry = Convert.ToInt32(lblHidTry.Text);

            if (svc.IsAnswerCorrect(RP02.Text, myTry, RP05.Text))
            {
                string userName = RP02.Text;
                string userEmail = RP03.Text;
            string PasswordresetEmailurl =  AppSettings.Get("PDMS-PASSWORDURL");
                MembershipUser user = Membership.GetUser(RP02.Text);
                Guid requestGuid = svc.CreatePasswordRequest((Guid)user.ProviderUserKey);
                PasswordresetEmailurl = PasswordresetEmailurl + "?action=reset&id=" + requestGuid.ToString();
                // TODO: EDV How do we get the registration id here. Do we need it?
                svc.NotifyPasswordResetEmail(userEmail, userName, PasswordresetEmailurl, 9, (Guid)user.ProviderUserKey);
                 Response.Redirect(Helper.RedirectLoginURL());

            }
            else
            {
                SetSecurityQuestion();
               
            }
        
    }
}