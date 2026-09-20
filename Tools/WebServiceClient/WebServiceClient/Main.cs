using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCFClientUtil;
using WebServiceClient.AttachmentService;
using WebServiceClient.IncidentReportService;
using WebServiceClient.PartialAcknowledgementService;

namespace WebServiceClient
{
    public partial class Main : Form
    {
        private CertUtil _certs;
        private X509Certificate2 _cert;

        public Main()
        {
            InitializeComponent();
            SetControlValues();
        }

        private void SetControlValues()
        {
            _certs = new CertUtil();

            txtEndPoint.Text = "https://localhost:44337";
            lbEndPoints.Items.Add("https://localhost:44337");
            lbEndPoints.Items.Add("https://webapi.ohpnm-testing.omes.maximus.com:10443/TestWebAPI");
            lbEndPoints.Items.Add("https://webapi.ohpnm-testing.omes.maximus.com:10443");

            lbService.Items.Add("UserInfo.svc/GetUserInfo");
            lbService.Items.Add("AttachmentService.svc/SendAttachment");
            lbService.Items.Add("AcknowledgmentService.svc/EchoSoapRequest");
            lbService.Items.Add("IncidentReport.svc/SendProviderIncident");
            lbService.SelectedIndex = 0;

            cbStoreLocation.DataSource = Enum.GetValues(typeof(StoreLocation));
            cbStoreLocation.SelectedItem = StoreLocation.CurrentUser;
            cbStoreName.DataSource = Enum.GetValues(typeof(StoreName));
            cbStoreName.SelectedItem = StoreName.My;
        }

        private void btnInvoke_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";
            _cert = _certs.GetCertificateByName(cbCerts.SelectedItem.ToString());

            switch (lbService.SelectedItem.ToString())
            {
                case "UserInfo.svc/GetUserInfo":
                    CallWebService();
                    break;
                case "AttachmentService.svc/SendAttachment":
                    CallDocumentService();
                    break;
                case "AcknowledgmentService.svc/EchoSoapRequest":
                    CallAcknowledgmentService();
                    break;
                case "IncidentReport.svc/SendProviderIncident":
                    CallIncidentReportService();
                    break;
                default:
                    MessageBox.Show("Method not implemented");
                    break;
            }

        }

        private void CallWebService()
        {
            var endpoint = txtEndPoint.Text + "/UserInfo.svc";

            UserInfoWebService client = GetUserInfoClient(endpoint);

            try
            {
                txtResult.Text = client.GetUserInfo(txtUserName.Text, txtPassword.Text);
                txtStatus.Text = "SUCCESS";
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
            finally
            {
                client.Dispose();
            }
        }

        private void ShowWsdl(TextBox txtEndPoint)
        {
            HttpClient client = new HttpClient();

            if (chkCert.Checked)
            {
                HttpClientHandler handler = GetClientCertHandler();
                client = new HttpClient(handler);
            }
            client.BaseAddress = new Uri(txtEndPoint.Text);

            HttpResponseMessage response = client.GetAsync("").Result;
            txtStatus.Text = response.ReasonPhrase;
            if (response.IsSuccessStatusCode)
            {
                txtResult.Text = response.Content.ReadAsStringAsync().Result;
            }
        }

        private void EchoSoapRequest()
        {
            var endpoint = txtEndPoint.Text.Replace("/EchoSoapRequest", "");

            UserInfoWebService client = GetUserInfoClient(endpoint);

            try
            {
                txtResult.Text = client.EchoSoapRequest();
                txtStatus.Text = "SUCCESS";
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
            finally
            {
                client.Dispose();
            }
        }

        private void CallDocumentService()
        {
            var endpoint = txtEndPoint.Text + "/DocumentService/AttachmentService.svc";
            var client = new DocumentClient(endpoint, txtUserName.Text, txtPassword.Text, _cert);

            try
            {
                txtResult.Text = client.SendAttachment();
                txtStatus.Text = "SUCCESS";
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
            finally
            {
                client.Dispose();
            }
        }

        private void CallAcknowledgmentService()
        {
            var endpoint = txtEndPoint.Text + "/AcknowledgmentService.svc";
            var client = WcfAuthorizedClient<AcknowledgmentServiceClient, AcknowledgmentService>
                .GetClient(endpoint, txtUserName.Text, txtPassword.Text, _cert);


            try
            {
                txtResult.Text = client.EchoSoapRequest(1);
                txtStatus.Text = "SUCCESS";
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
            finally
            {
                try
                {
                    client?.Close();
                }
                catch { }
            }
        }

        private void CallIncidentReportService()
        {
            var endpoint = txtEndPoint.Text + "/IncidentReport.svc";
            var client = WcfAuthorizedClient<IncidentReportClient, IncidentReport>
                .GetClient(endpoint, txtUserName.Text, txtPassword.Text, _cert);


            try
            {
                var result = client.SendProviderIncident(new ProviderIncidentReportModel());
                string json = JsonConvert.SerializeObject(result, Formatting.Indented);
                txtResult.Text = json;
                txtStatus.Text = "SUCCESS";
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
            finally
            {
                try
                {
                    client?.Close();
                }
                catch { }
            }
        }

        private HttpClientHandler GetClientCertHandler()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(_certs.GetCertificateByName(cbCerts.SelectedItem.ToString()));

            return handler;
        }

        private void lbEndPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEndPoint.Text = lbEndPoints.SelectedItem.ToString();
        }

        private void DisplayError(Exception ex)
        {
            var result = new StringBuilder(ex.Message);
            Exception innerEx = ex.InnerException;
            while (innerEx != null)
            {
                result.AppendLine();
                result.Append(innerEx.Message);
                innerEx = innerEx.InnerException;
            }
            txtResult.Text = result.ToString();
            txtStatus.Text = "ERROR";
        }

        private UserInfoWebService GetUserInfoClient(string endpoint)
        {
            if (chkCert.Checked)
            {
                return new UserInfoWebService(_cert, endpoint, txtUserName.Text, txtPassword.Text);
            }
            else
            {
                return new UserInfoWebService(endpoint, txtUserName.Text, txtPassword.Text);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtStatus.Text = "";
            txtResult.Text = "";
        }

        private void cbStoreLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCertStore();
        }

        private void cbStoreName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCertStore();
        }

        private void SetCertStore()
        {
            if (cbStoreLocation.SelectedItem != null && cbStoreName.SelectedItem != null)
            {
                StoreLocation storeLocation = (StoreLocation)cbStoreLocation.SelectedItem;
                StoreName storeName = (StoreName)cbStoreName.SelectedItem;
                _certs = new CertUtil(storeName, storeLocation);

                cbCerts.DataSource = _certs.GetCertNames();
                if (cbCerts.Items.Count > 0)
                {
                    cbCerts.SelectedIndex = 0;
                }
            }
        }
    }
}
