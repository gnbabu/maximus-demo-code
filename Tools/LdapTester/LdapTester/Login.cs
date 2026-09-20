using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LdapTester
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            Domain.Text = "Maxcorp";
        }

        private void Validate_Click(object sender, EventArgs e)
        {
            if (AuthenticatePrincipal(Domain.Text, UserName.Text, Password.Text))
                DialogResult = DialogResult.Yes;
            else
            {
                MessageBox.Show("Unable to Authenticate Using the Supplied Credentials");
                DialogResult = DialogResult.No;
            }
            this.Close();
        }

        private bool AuthenticateUser(string domainName, string userName, string password)
        {
            bool ret = false;

            try
            {
                DirectoryEntry de = new DirectoryEntry("LDAP://" + domainName, userName, password);
                DirectorySearcher dsearch = new DirectorySearcher(de);
                SearchResult results = null;

                results = dsearch.FindOne();

                ret = true;
            }
            catch
            {
                ret = false;
            }

            return ret;
        }

        private bool AuthenticatePrincipal(string domainName, string userName, string password)
        {
            bool ret = false;

            try
            {
                PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, domainName, userName, password);

                ret = principalContext.ValidateCredentials(userName, password);
            }
            catch
            {
                ret = false;
            }

            return ret;
        }

    }
}
