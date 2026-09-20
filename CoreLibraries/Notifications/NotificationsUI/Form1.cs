using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MAXIMUS.Core.Libraries;

namespace NotificationsUI
{
    public partial class Form1 : Form
    {

        Guid guid = Guid.NewGuid();

        public Form1()
        {
            InitializeComponent();

            txtBccEmail.Text = AppSettings.Get("SmtpBCC");
            txtClientHost.Text = AppSettings.Get("SmtpClientHost");
            txtDefaultCredentials.Text = AppSettings.Get("SmtpUseDefaultCredentials");
            txtFromEmail.Text = AppSettings.Get("SmtpFromEmailAddress");
            txtPortNumber.Text = AppSettings.Get("SmtpClientPortNumber");
            txtReplyToEmail.Text = AppSettings.Get("SmtpReplyTo");
            txtTestEmailAddr.Text = AppSettings.Get("Jobs-NotificationEmailAddresses", "OHPNMCodeJunkies@maximus.com");
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            string msg = "Sent DateTime: {0}; Machine: {1}; User: {2}, Thread {3}";
            msg = String.Format(msg, DateTime.Now.ToString(), Environment.MachineName.ToString()
                , (Environment.UserDomainName + @"\" + Environment.UserName), guid.ToString());
            EMailNotification notify = new EMailNotification(msg, "Test Email from NotificationsUI", "", guid);
            notify.SendGenericJobNotification(true);
        }

        private void btnSmsTestForm_Click(object sender, EventArgs e)
        {
            var frmSMS = new SendText();
            frmSMS.Show();
        }
    }
}
