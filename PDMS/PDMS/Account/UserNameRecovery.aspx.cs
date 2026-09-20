using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserNameRecovery : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //((Panel)this.Page.Master.FindControl("pnlRightBox")).Style["display"] = "none";
    }

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
            pnlStep1.Visible = true;
            pnlSuccess.Visible = false;
            txtMedicaidID.Focus();
        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        Page.Validate("Step1");
        if (!Page.IsValid) return;

        pnlValidationSummary.Visible = false;

        bool isRecovered = false;
        string email = txtEmail.Text.Trim();
        string medicaidID = txtMedicaidID.Text;

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        isRecovered = svc.NotifyUserName(email, medicaidID);
        if (isRecovered)
        {
            pnlStep1.Visible = false;
            pnlSuccess.Visible = true;
        }
        else
        {
            pnlValidationSummary.Visible = UNR11_ERR.Visible = true;
            pnlStep1.Visible = true;
            pnlSuccess.Visible = false;
        }

        upReset.Update();
    }

    //protected void UNR02_Validating(Object sender, ServerValidateEventArgs e)
    //{
    //    try
    //    {
    //        System.Net.Mail.MailAddress mail = new System.Net.Mail.MailAddress(UNR02.Text.Trim());
    //        e.IsValid = true;
    //    }
    //    catch
    //    {
    //        e.IsValid = false;
    //    }
    //}

    //protected void ValidateUNR0406(Object sender, ServerValidateEventArgs e)
    //{
    //   // e.IsValid = !string.IsNullOrEmpty(UNR04.Text) || !string.IsNullOrEmpty(UNR06.Text);
    //}

    protected void ValidateUNR09(Object sender, ServerValidateEventArgs e)
    {
        string captcha = UNR09.Text;
        Captcha1.ValidateCaptcha(captcha);
        e.IsValid = Captcha1.UserValidated;
    }

    protected void btnModalOk_Click(object sender, EventArgs e)
    {
        mpeChangesSaved.Hide();
        Response.Redirect(Helper.RedirectLoginURL());
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnModalOk.Visible = true;
        btnModalCancel.Visible = true;
        mpeChangesSaved.Show();
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Response.Redirect(Helper.RedirectLoginURL());
    }
    protected void Validate_cvCEmailMatch(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = txtEmail.Text.Equals(txtConfirmEmail.Text);
    }

}