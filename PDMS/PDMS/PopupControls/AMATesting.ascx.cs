using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_AMATesting : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnGetAMATestingID_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(txtNPIID.Text) && !string.IsNullOrWhiteSpace(txtUserNameID.Text))
            {
                string amaXmlResponse = "";
                XMLResponseStatus xrs = new XMLResponseStatus();
                xrs = GetProfileByNPI(txtNPIID.Text, txtUserNameID.Text);
                amaXmlResponse = xrs.xmlResponse;
                if (!string.IsNullOrEmpty(amaXmlResponse))
                {
                    txtAMAResponse.Text = txtAMAResponse.Text + "AMAResponse : " + amaXmlResponse + "\n\nStatus : " + xrs.status;
                    string entityID = GetEntityID(xrs);
                    if (!string.IsNullOrEmpty(entityID))
                    {
                        string resp = GetProfileFull(entityID, txtUserNameID.Text);
                        txtAMAProfileResponse.Text = "AMA Full Profile Response : " + resp;
                        string response = GetPDFRequestPhycisianByID(entityID, txtUserNameID.Text);
                        string filePath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + "AMAProviderFile" + entityID + ".pdf";
                        var fileInfo = new System.IO.FileInfo(filePath);
                        Response.ContentType = "application/octet-stream";
                        Response.AddHeader("Content-Disposition", String.Format("attachment;filename=\"{0}\"", "AMA Provider File" + entityID + ".pdf"));
                        Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                        Response.WriteFile(filePath);
                        Response.End();
                    }
                    else
                    {
                        AddError(string.Format("No Entity ID  {0} ", txtNPIID.Text));
                    }
                }
                else
                {
                    AddError(string.Format("No Profile found for  {0} ", txtNPIID.Text));
                }
            }
            else
            {
                AddError("Please enter the required fields.");
            }
        }
        catch (Exception ex)
        {
            AddError("Error: " + ex.Message + " " + ex.StackTrace);
        }
    }

    private string GetPDFRequestPhycisianByID(string Id, string username)
    {
        string xmlResponse = string.Empty;
        try
        {
            string filePath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + "AMAProviderFile" + Id + ".pdf";
            WebRequest wrPhysicianFullPDF = InitializeRequest("/profiles/pdf/full/" + Id, "GET", username);


            using (WebResponse rsPAFull = wrPhysicianFullPDF.GetResponse())
            {
                HttpWebResponse rsPAfullStatusCode = (HttpWebResponse)rsPAFull;

                HttpStatusCode statuscode = rsPAfullStatusCode.StatusCode;
                using (var stream = rsPAFull.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            stream.CopyTo(fs);

                        }
                    }
                }

            }
            //}
        }
        catch (WebException htex)
        {
            var ex = htex.Response as HttpWebResponse;
        }
        catch(Exception ex)
        {
            AddError("Error: " + ex.Message + " " + ex.StackTrace);
        }
        return xmlResponse;
    }

    private string GetProfileFull(string Id, string username)
    {
        string xmlResponse = string.Empty;
        try
        {
            string filePath = "~/Documents/AMAProviderFile" + Id + ".pdf";
            WebRequest wrPhysicianFullPDF = InitializeRequest("/profiles/profile/full/" + Id, "GET", username);


            using (WebResponse rsPAFull = wrPhysicianFullPDF.GetResponse())
            {
                HttpWebResponse rsPAfullStatusCode = (HttpWebResponse)rsPAFull;

                HttpStatusCode statuscode = rsPAfullStatusCode.StatusCode;

                using (var stream = rsPAFull.GetResponseStream())
                {

                    using (StreamReader sr = new StreamReader(stream))
                    {
                        xmlResponse = sr.ReadToEnd();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            AddError("Error: " + ex.Message + " " + ex.StackTrace);
        }
        return xmlResponse;
    }


    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "VerifyAMATesting";
        this.Page.Validators.Add(val);
    }

    private XMLResponseStatus GetProfileByNPI(string npi, string username)
    {
        string response = string.Empty;

        string xmlResponse = string.Empty;
        XMLResponseStatus s = new XMLResponseStatus();
        try
        {
            WebRequest wrPAFull = InitializeRequest("/profiles/search", "POST", username);
            string requestInput = string.Empty;

            using (System.IO.StreamReader file = new System.IO.StreamReader(Server.MapPath("~/DataTemplate/SearchProfileByNPI.xml")))
            {

                requestInput = file.ReadToEnd().ToString();
                requestInput = requestInput.Replace("NPITEMPLATE", npi);
            }
            byte[] data = Encoding.ASCII.GetBytes(requestInput);
            using (Stream stream = wrPAFull.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            using (WebResponse rsPAFull = wrPAFull.GetResponse())
            {
                HttpWebResponse rsPAfullStatusCode = (HttpWebResponse)rsPAFull;

                HttpStatusCode statuscode = rsPAfullStatusCode.StatusCode;

                using (Stream stream = rsPAFull.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        xmlResponse = sr.ReadToEnd();
                        s.xmlResponse = xmlResponse;
                        s.status = statuscode.ToString();
                    }
                }

            }

        }
        catch (Exception ex)
        {
            AddError("Error: " + ex.Message + " " + ex.StackTrace);
        }
        return s;
    }


    private WebRequest InitializeRequest(string method, string methodtype, string username)
    {
        WebRequest request = null;
        try
        {
            //create REquest with method as variable

            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            string AMA_URI = AppSettings.Get("AMAURL");
            string requestMethod = method;
            string requestURL = AMA_URI + requestMethod;
            string requestToken = createatokenRequest(username);
            string tokenRequestStatus = "";
            if (string.IsNullOrEmpty(requestToken))
            {
                tokenRequestStatus = CON.AMAStatusID.Unauthorized.ToString();
            }
            else
            {
                tokenRequestStatus = CON.AMAStatusID.Successful.ToString();
            }
            request = WebRequest.Create(requestURL);
            request.UseDefaultCredentials = true;
            request.PreAuthenticate = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = methodtype;
            request.ContentType = "application/xml";
            request.Headers.Add("Authorization", "Bearer " + requestToken);
            request.Headers.Add("X-Location", "MAXIMUS");
            request.Headers.Add("X-CredentialProviderUserId", username);
            request.Headers.Add("X-SourceSystem", AppSettings.Get("BrandName") + AppSettings.Get("Environment"));
        }
        catch (Exception ex)
        {
            AddError("Error: " + ex.Message + " " + ex.StackTrace);
        }

        return request;

    }

    private string createatokenRequest(string username)
    {
        try
        {
            CredentialHelper ch = new CredentialHelper();
            string accesstoken = ch.createTokenRequest(username);
            txtAMAResponse.Text = txtAMAResponse.Text + "\nAccessToken : " + accesstoken + "\n ";
            return accesstoken;
        }
        catch (Exception ex)
        {
            AddError("Error: " + ex.Message + " " + ex.StackTrace);
            throw ex;
        }
    }

    private string GetEntityID(XMLResponseStatus xrs)
    {
        string xmlResponse = xrs.xmlResponse;
        string id = "";
        string status = "";
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlResponse);
            XmlElement root = doc.DocumentElement;
            var list = root.GetElementsByTagName("entityId");
            var statuslist = root.GetElementsByTagName("status");
            id = list[0].InnerXml.Trim().ToString();
            status = xrs.status;
        }
        catch (Exception ex)
        {
            AddError("Error: " + ex.Message + " " + ex.StackTrace);
            throw ex;
        }
        return id;
    }
}