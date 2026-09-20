using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using MAXIMUS.Core.Libraries;

namespace Corp.Core.Libraries.ClaimsManagement
{
    
    public class ClaimsAttachmentHelper
    {

        private const string claimEDITransactionType = "1";
        private const string claimPayorRequested = "No";
      
        public void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc, byte[] fileStream)
        {

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

          
            rs.Write(fileStream, 0, fileStream.Length);
            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);

            }
            catch (Exception ex)
            {

                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }

        public NameValueCollection SetUpClaimsNameValueCollection(string destinationpayor,string filename,string documenttype,string recipient,string claimType,string medicaid,string claimControl,string receiverCode)
        {
            NameValueCollection nvcClaim = new NameValueCollection();
           
            nvcClaim.Add("EDITransaction_Type_ID", claimEDITransactionType);           
            nvcClaim.Add("PayerRequested", claimPayorRequested);
            nvcClaim.Add("Member_ID", recipient);
            nvcClaim.Add("Claim_Type_id", claimType);
            nvcClaim.Add("claim_number", claimControl);
            nvcClaim.Add("provider_id", medicaid);
            nvcClaim.Add("sender_id", CON.maximusSenderID);
            nvcClaim.Add("receiver_id", destinationpayor);
            nvcClaim.Add("document_type_id", documenttype);
            nvcClaim.Add("documentname", filename);
            nvcClaim.Add("UUID", new Guid().ToString());
            nvcClaim.Add("to_send", "false");
            nvcClaim.Add("last_modified_date_time", DateTime.Now.ToString());
            nvcClaim.Add("receiver_code", receiverCode);

            return nvcClaim; 
        }

        public void DeleteClaimAttachment(int Id)
        {
            
                
                string APIUrl = AppSettings.Get("UploadAttachmentWebAPI");
                string url = APIUrl + "/api/upload/";
                int outbound_upload_attachments = Id;
                url = url + outbound_upload_attachments.ToString();
                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
                wr.ContentType = "text/plain;";
                wr.Method = "DELETE";
                wr.KeepAlive = true;
                wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

                Stream rs = wr.GetRequestStream();
                rs.Close();

                WebResponse wresp = null;
                try
                {
                    wresp = wr.GetResponse();
                    Stream stream2 = wresp.GetResponseStream();
                    StreamReader reader2 = new StreamReader(stream2);

                }
                catch (Exception ex)
                {

                    if (wresp != null)
                    {
                        wresp.Close();
                        wresp = null;
                    }
                }
                finally
                {
                    wr = null;
                }
            
        }
    }
}
