using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Corp.Core.Services;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
            cboCarrierLoad();
        }

        private void cmdSend_Click(object sender, EventArgs e)
        {
            string strMailTo  = txtPhoneNumber.Text.Trim() + cboCarrier.SelectedItem.ToString().Trim();
            string strMessageBody = txtMessage.Text.Trim();
           Notification.SMail(strMailTo,"test",strMessageBody,true,',');

        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            //Upon user's request, close the application
            Application.Exit();

        }



        private void cboCarrierLoad()
        {
            // set up the carriers list - this is a fair list, you may wish to
            // research the topic and add others, it took a while to generate this
            // list...
            cboCarrier.Items.Add("@itelemigcelular.com.br");
            cboCarrier.Items.Add("@message.alltel.com");
            cboCarrier.Items.Add("@message.pioneerenidcellular.com");
            cboCarrier.Items.Add("@messaging.cellone-sf.com");
            cboCarrier.Items.Add("@messaging.centurytel.net");
            cboCarrier.Items.Add("@messaging.sprintpcs.com");
            cboCarrier.Items.Add("@mobile.att.net");
            cboCarrier.Items.Add("@mobile.cell1se.com");
            cboCarrier.Items.Add("@mobile.celloneusa.com");
            cboCarrier.Items.Add("@mobile.dobson.net");
            cboCarrier.Items.Add("@mobile.mycingular.com");
            cboCarrier.Items.Add("@mobile.mycingular.net");
            cboCarrier.Items.Add("@mobile.surewest.com");
            cboCarrier.Items.Add("@msg.acsalaska.com");
            cboCarrier.Items.Add("@msg.clearnet.com");
            cboCarrier.Items.Add("@msg.mactel.com");
            cboCarrier.Items.Add("@msg.myvzw.com");
            cboCarrier.Items.Add("@msg.telus.com");
            cboCarrier.Items.Add("@mycellular.com");
            cboCarrier.Items.Add("@mycingular.com");
            cboCarrier.Items.Add("@mycingular.net");
            cboCarrier.Items.Add("@mycingular.textmsg.com");
            cboCarrier.Items.Add("@o2.net.br");
            cboCarrier.Items.Add("@ondefor.com");
            cboCarrier.Items.Add("@pcs.rogers.com");
            cboCarrier.Items.Add("@personal-net.com.ar");
            cboCarrier.Items.Add("@personal.net.py");
            cboCarrier.Items.Add("@portafree.com");
            cboCarrier.Items.Add("@qwest.com");
            cboCarrier.Items.Add("@qwestmp.com");
            cboCarrier.Items.Add("@sbcemail.com");
            cboCarrier.Items.Add("@sms.bluecell.com");
            cboCarrier.Items.Add("@sms.cwjamaica.com");
            cboCarrier.Items.Add("@sms.edgewireless.com");
            cboCarrier.Items.Add("@sms.hickorytech.com");
            cboCarrier.Items.Add("@sms.net.nz");
            cboCarrier.Items.Add("@sms.pscel.com");
            cboCarrier.Items.Add("@smsc.vzpacifica.net");
            cboCarrier.Items.Add("@speedmemo.com");
            cboCarrier.Items.Add("@suncom1.com");
            cboCarrier.Items.Add("@sungram.com");
            cboCarrier.Items.Add("@telesurf.com.py");
            cboCarrier.Items.Add("@teletexto.rcp.net.pe");
            cboCarrier.Items.Add("@text.houstoncellular.net");
            cboCarrier.Items.Add("@text.telus.com");
            cboCarrier.Items.Add("@timnet.com");
            cboCarrier.Items.Add("@timnet.com.br");
            cboCarrier.Items.Add("@tms.suncom.com");
            cboCarrier.Items.Add("@tmomail.net");
            cboCarrier.Items.Add("@tsttmobile.co.tt");
            cboCarrier.Items.Add("@txt.bellmobility.ca");
            cboCarrier.Items.Add("@typetalk.ruralcellular.com");
            cboCarrier.Items.Add("@unistar.unifon.com.ar");
            cboCarrier.Items.Add("@uscc.textmsg.com");
            cboCarrier.Items.Add("@voicestream.net");
            cboCarrier.Items.Add("@vtext.com");
            cboCarrier.Items.Add("@wireless.bellsouth.com");
            cboCarrier.Items.Add("@iwspcs.net");
           
        }
    }
}
