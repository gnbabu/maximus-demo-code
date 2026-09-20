using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MAXIMUS.Core.Libraries;

namespace MAXIMUS.Core.Libraries
{
    public partial class SendText : Form
    {
        public SendText()
        {
            InitializeComponent();
            txtRegId.Text = "480161";
            txtMessage.Text = "Congratultions! You have a text!";
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                string regId = txtRegId.Text;
                string body = txtMessage.Text;

                var notification = new Notification();
                var smsResult = notification.SendSMS(body, regId);
                if (smsResult.StartsWith("SUCCESS"))
                {
                    result.AppendLine("Text sent successfully");
                }
                else
                {
                    result.AppendLine("SMS error, check status code on http://api.maximus.messagingchannel.com/docs/rest/status-code.php");
                }
                result.AppendLine(smsResult);
            }
            catch (Exception ex)
            {
                result.AppendLine("Error Sending Text");
                result.AppendLine();
                var innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    result.AppendLine(innerEx.Message);
                    innerEx = innerEx.InnerException;
                }
            }
            lblResult.Text = result.ToString();
        }
    }
}
