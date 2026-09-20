using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LdapTester
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ReadConfig();
            PopulateMethods();
        }

        private void PopulateMethods()
        {
            cbMethod.Items.Add("Protocols.LdapConnection");
            cbMethod.Items.Add("DirectorySearcher");
            cbMethod.Items.Add("AccountManagement");
            cbMethod.SelectedIndex = 0;
        }

        private void Validate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Results.Text = "";
            Application.DoEvents();

            switch (cbMethod.Text)
            {
                case "Protocols.LdapConnection":
                    LdapConnectionValidate();
                    break;
                case "DirectorySearcher":
                    LdapValidate(UserName.Text, Password.Text);
                    break;
                case "AccountManagement":
                    AuthenticatePrincipal();
                    break;
            }
            Cursor.Current = Cursors.Default;
        }

        private void LdapConnectionValidate()
        {
            try
            {
                LdapConnection con = new LdapConnection(new LdapDirectoryIdentifier(Server.Text, int.Parse(Port.Text)));
                con.SessionOptions.SecureSocketLayer = true;
                con.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback(ServerCallback);
                con.AuthType = AuthType.Negotiate;
                con.SessionOptions.ProtocolVersion = 3;
                con.Credential = new NetworkCredential(UserName.Text, Password.Text);
                con.Bind();

                SearchRequest dr = new SearchRequest() { Filter = "(uid=" + UserName.Text + ")" };

                var response = con.SendRequest(dr);

                Results.Text = response.ResultCode.ToString();
            }
            catch (Exception ex)
            {
                Results.Text = FullException(ex);
            }
        }

        private void ReadConfig()
        {
            Server.Text = ConfigurationManager.AppSettings["Server"]?.ToString();
            Port.Text = ConfigurationManager.AppSettings["Port"]?.ToString();
            BindDN.Text = ConfigurationManager.AppSettings["BindDN"]?.ToString();
            UserName.Text = ConfigurationManager.AppSettings["UserName"]?.ToString();
            Password.Text = ConfigurationManager.AppSettings["Password"]?.ToString();
        }

        private bool LdapValidate(string userName, string password)
        {
            try
            {
                DirectoryEntry de = new DirectoryEntry();
                string protocol = "LDAP://";
                de.AuthenticationType = AuthenticationTypes.None;
                if (Port.Text == "636")
                {
                    protocol = "LDAPS://";
                    de.AuthenticationType = AuthenticationTypes.SecureSocketsLayer;
                }
                de.Path = protocol + Server.Text + ":" + Port.Text;
                if (!String.IsNullOrEmpty(BindDN.Text))
                {
                    de.Path += "/" + BindDN.Text;
                }
                if (!String.IsNullOrEmpty(Password.Text))
                {
                    de.Username = userName;
                    de.Password = password;
                }

                DirectorySearcher deSearch = new DirectorySearcher();

                deSearch.SearchRoot = de;
                deSearch.Filter = "(uid=" + UserName.Text + ")";

                SearchResult result = deSearch.FindOne();

                foreach (DictionaryEntry prop in result.Properties)
                {
                    Results.Text += prop.Key.ToString() + ": ";
                    var value = prop.Value as ReadOnlyCollectionBase;

                    foreach (var item in value)
                    {
                        Results.Text += item.ToString() + "; ";
                    }
                    Results.Text += Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                Results.Text = FullException(ex);
            }

            return true;
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var login = new Login();
            login.Owner = this;
            login.ShowDialog();
            if (login.DialogResult == DialogResult.Yes)
                MessageBox.Show("User Logged In");
            else
                MessageBox.Show("User NOT Logged In");
        }

        private void btnUseCert_Click(object sender, EventArgs e)
        {
            try
            {
                LdapConnectionValidate();
            }
            catch (Exception ex)
            {
                Results.Text = ex.Message;
            }
        }

        private bool ServerCallback(LdapConnection connection, X509Certificate certificate)
        {
            return true;
        }

        private void AuthenticatePrincipal()
        {
            try
            {
                PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, Server.Text, UserName.Text, Password.Text);
                if (principalContext.ValidateCredentials(UserName.Text, Password.Text))
                {
                    Results.Text = "User " + UserName.Text + " successfully validated.";
                }
                else
                {
                    Results.Text = "User " + UserName.Text + " invalid credentials.";
                }
            }
            catch (Exception ex)
            {
                Results.Text = FullException(ex);
            }
        }

        private string FullException(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            Exception inner = ex.InnerException;
            while (inner != null)
            {
                sb.AppendLine(inner.Message);
                inner = inner.InnerException;
            }
            return sb.ToString();
        }
    }
}
