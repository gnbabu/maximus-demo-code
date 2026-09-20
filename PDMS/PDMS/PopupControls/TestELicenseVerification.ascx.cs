using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Web.UI;

public partial class PopupControls_TestELicenseVerification : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnRequestLicenseInfo_Click(object sender, EventArgs e)
    {
        try
        {
            ELicenseVerificationRequest eRequest = new ELicenseVerificationRequest();
            eRequest.board_type = tbBoardname.Text.Trim();
            eRequest.last_4_ssn = tblast4ssn.Text.Trim();
            eRequest.last_name = tbLastName.Text.Trim();
            eRequest.dob = tbdob.Text.Trim();

            var sec = new SecretsManager(AppSettings.Get("SecretsRegion"));
            var result = sec.GetSuperSecretPassword(AppSettings.Get("eLICSecretDictionary"));
            result.Wait();
            string passcode = result.Result["eLicenseVerificationWebservicePassCode"];

            if (string.IsNullOrEmpty(passcode))
            {
                throw new Exception("client_secret empty");
            }

            string urllicense = AppSettings.Get("eLicenseVerificationWebserviceURL");
            string user = AppSettings.Get("eLicenseVerificationWebserviceUser");
            Uri apiUrl = new Uri(urllicense);
            WebRequest erequet = HttpWebRequest.Create(urllicense);
            HttpWebRequest eVerificationRequest = (HttpWebRequest)erequet;
            eVerificationRequest.ContentType = "application/json";
            eVerificationRequest.Method = "POST";
            NetworkCredential ncredential = new NetworkCredential(user, passcode);
            CredentialCache ecredentialCache = new CredentialCache();
            ecredentialCache.Add(apiUrl, "Basic", ncredential);
            eVerificationRequest.PreAuthenticate = true;
            eVerificationRequest.Credentials = ecredentialCache;
            string strRequest = string.Empty;
            strRequest = JsonConvert.SerializeObject(eRequest);
            tbRequestLicense.Text = strRequest;

            using (StreamWriter stream = new StreamWriter(eVerificationRequest.GetRequestStream()))
            {
                stream.Write(strRequest);
            }

            var response = "";

            //Getresponse
            using (WebResponse rsElicense = eVerificationRequest.GetResponse())
            {
                HttpWebResponse rsHttpLicense = (HttpWebResponse)rsElicense;

                HttpStatusCode statuscode = rsHttpLicense.StatusCode;

                using (Stream stream = rsHttpLicense.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        response = sr.ReadToEnd();
                    }
                }
            }

            tbresponselicense.Text = response.ToString();
        }
        catch (Exception ex)
        {
            tblicenseException.Text = "message: " + ex.Message.ToString() + "<BR/>" + "trace:" + ex.StackTrace.ToString() + "<BR/>" + "Source:" + ex.Source.ToString();
        }
    }
}