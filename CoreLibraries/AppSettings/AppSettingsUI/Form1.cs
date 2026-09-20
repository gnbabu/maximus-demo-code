using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using MAXIMUS.Core.Libraries;

namespace AppSettingsUI.csproj
{
    public partial class frmAppSettings : Form
    {
        public frmAppSettings()
        {
            InitializeComponent();
        }

        private void btnGetAppSetting_Click(object sender, EventArgs e)
        {
            try
            {
                txtReturnValue.Text = "Retrieving";
                txtReturnValue.Text = AppSettings.Get(txtGetSetting.Text, "Setting Not Found");
            }
            catch (Exception ex)
            {
                txtReturnValue.Text = ex.Message + Environment.NewLine + ex.StackTrace;
            }

        }

        private void btnConnectString_Click(object sender, EventArgs e)
        {
            try
            {
                txtReturnValue.Text = "Retrieving";
                txtReturnValue.Text = AppSettings.GetConnectionString();
            }
            catch (Exception ex)
            {
                txtReturnValue.Text = ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }

        private static void AddAddresses(MailAddressCollection col, string addresses)
        {
            // Split apart the Email Address + the separation by a semicolon ";".
            string[] Addrs = addresses.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string adr in Addrs)
            {
                string emailAddress = adr.Trim();
                if (emailAddress.Length > 0)
                {
                    col.Add(new MailAddress(emailAddress));
                }
            }
        }

        private void SendTestEmail()
        {

            string emailBCC = string.Empty;
            string emailReplyTo = string.Empty;
            string emailFrom = string.Empty;
            string emailTo = string.Empty;

            try
            {

                MailMessage emailMessage = new MailMessage();

                // if test emails should not be set, send test email instead
                if (AppSettings.Get("SendTestEmail").ToLower() == Convert.ToString(true).ToLower())
                {
                    // set the test email address(es)
                    emailTo = AppSettings.Get("TestEmailAddress");
                }
                else // production email
                {
                    // set the production email address and the CC and BCC as well.
                    emailBCC = AppSettings.Get("MailBCC");
                    emailReplyTo = AppSettings.Get("MailReplyTo");
                }

                emailFrom = AppSettings.Get("MailFrom");

                // Split apart the Email Address + the separation by a comma ",".
                string[] emails = emailTo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string email in emails)
                {
                    emailMessage.To.Add(new MailAddress(email));
                }

                // if the reply to was populated
                if (emailReplyTo.Length > 0)
                {
                    emailMessage.ReplyTo = new MailAddress(emailReplyTo);
                }

                emailMessage.From = new MailAddress(emailFrom);
                emailMessage.Subject = "Test Email";
                string body = "Email generated from Core testing application [{0}].";
                body += System.Environment.NewLine + "Reply to jfetters@policy-studies.com if received.";
                emailMessage.Body = String.Format(body, DateTime.Now.ToString());

                SmtpClient smtpclient;
                smtpclient = new SmtpClient();
                smtpclient.EnableSsl = false;

                smtpclient.Send(emailMessage);

                emailMessage = null;
            }
            catch (Exception ex)
            {
                txtReturnValue.Text = ex.Message + Environment.NewLine + ex.StackTrace;
                throw ex;
            }
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            try
            {
                SendTestEmail();
                txtReturnValue.Text = "Message sent";
            }
            catch (Exception ex)
            {
                txtReturnValue.Text = ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }
    }
}
