using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using IBM.WMQ;
using IBMMQWP;

namespace IBMMQWP
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            WebSphereLib.Initialize_WebSphereLib();
            WebSphereLib.SetDefaults();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtQueueManagerName.Text = WebSphereLib.strQueueManagerName;
            txtQueueName.Text = WebSphereLib.strQueueName;
            txtChannelName.Text = WebSphereLib.strChannelName;
            txtServerName.Text = WebSphereLib.strServerName;
            txtUserName.Text = WebSphereLib.strUserName;
            txtPassword.Text = WebSphereLib.strPassword;
            txtPort.Text = WebSphereLib.intPort.ToString();
            cbCertificateStore.Text = WebSphereLib.strCertificateStore;
            txtCertificateName.Text = WebSphereLib.strCertificateName;
            txtCipherName.Text = WebSphereLib.strCipherName;
            cbTransportProperty.Text = WebSphereLib.strTransportProperty;
            txtPutMsg.Text = WebSphereLib.strMsg;

            txtPUTQueueName.Text = txtQueueName.Text;
            txtGETQueueName.Text = txtQueueName.Text;

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // Fill variables from edit fields on screen.
            WebSphereLib.strServerName = txtServerName.Text;
            WebSphereLib.intPort = int.Parse(txtPort.Text);
            WebSphereLib.strQueueName = txtQueueName.Text;
            WebSphereLib.strChannelName = txtChannelName.Text;
            WebSphereLib.strCertificateStore = cbCertificateStore.Text;
            WebSphereLib.strCertificateName = txtCertificateName.Text;
            WebSphereLib.strCipherName = txtCipherName.Text;
            WebSphereLib.strUserName = txtUserName.Text;
            WebSphereLib.strPassword = txtPassword.Text;
            WebSphereLib.strQueueManagerName = txtQueueManagerName.Text;
            WebSphereLib.strTransportProperty = cbTransportProperty.Text;

            lblConnect.Text = WebSphereLib.Connect();
            if (lblConnect.Text.Substring(0,12).Equals("Successfully"))
            {
                btnWriteMsg.Enabled = true;
                btnReadMsg.Enabled = true;
            }

         
        }

        private void btnPutMsg_Click(object sender, EventArgs e)
        {
            try
            {
                //Define a Message
                WebSphereLib.strMsg = txtPutMsg.Text;
                WebSphereLib.strQueueName = txtPUTQueueName.Text;

                lblWriteResults.Text = WebSphereLib.PubMsg();
            }
            catch (Exception exp)
            {
                lblWriteResults.Text = exp.Message;
            }

        }

        private void btnReadMsg_Click(object sender, EventArgs e)
        {
            WebSphereLib.strQueueName = txtGETQueueName.Text;

            try
            {
                lblReadRestuls.Text = WebSphereLib.ReadMsg();
                lblMsgRead.Text = WebSphereLib.strMsg;
            }
            catch (Exception exp)
            {
                lblReadRestuls.Text = exp.Message;
            }
        }

        private void txtChannelName_TextChanged(object sender, EventArgs e)
        {
            txtPUTQueueName.Text = txtQueueName.Text;
            txtGETQueueName.Text = txtQueueName.Text;
        }
    }
}
