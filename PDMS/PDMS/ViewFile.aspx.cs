using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MMSWebControls;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Threading;
using Corp.Core.Libraries.Helper;
using Corp.Core.Libraries;

// TODO: EDV remove ViewFile and do simple download
public partial class ViewFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["OnBaseId"] != null)
            {
                OnBaseInterface ob = new OnBaseInterface();
                string OnBaseId = Request.QueryString["OnBaseId"];
                int ObDocId = ob.DeObfuscateId(OnBaseId);
                int OBID = 0;

                if (ObDocId < 1)
                {
                    lblMessage.Text = "Invalid Document Id Specified";
                    return;
                }

                // TEMP: Determine if this a converted document (converted docuements are encrypted, others are not yet, this will change to checking an encryption flag in the future)


                // this will now be ignored
                // if (Request.QueryString["UseEncryption"] != null) { IsEncrypted = Request.QueryString["UseEncryption"].ToUpper(); }  // Y or N 

                // deobfucscate OnBaseId
                OBID = ob.DeObfuscateId(OnBaseId);

                if (OBID < 1)
                {
                    lblMessage.Text = "Invalid Document Id Specified (" + OnBaseId + ")";
                    return;
                }


                byte[] fl = ob.DownloadOnbaseFile(OBID.ToString());  // decryption is now moved to a central location


                // mime type and filename are set in object when file is downloaded
                string MimeTP = ob.DownloadedMimeType;

                // get original file name
                string OriginalFileName = ob.DownloadedOriginalFileName;


                byte[] fileBytes = fl;


                var resp = HttpContext.Current.Response;

                resp.Clear();
                resp.ClearHeaders();
                resp.ClearContent();

                resp.ContentType = MimeTP;

                // Build Content-Disposition without interpolation
                // Quoted filename and RFC 5987/6266 UTF-8 parameter for special characters
                string quotedFilename = "\"" + OriginalFileName + "\"";
                string utf8Filename = Uri.EscapeDataString(OriginalFileName);
                string contentDisposition = "inline; filename=" + quotedFilename + "; filename*=UTF-8''" + utf8Filename;

                resp.AddHeader("Content-Disposition", contentDisposition);

                // Optional: size hint
                resp.AddHeader("Content-Length", fileBytes.Length.ToString());

                // Optional: caching (tune as appropriate)
                resp.Cache.SetCacheability(System.Web.HttpCacheability.Private);
                resp.Cache.SetNoStore();

                resp.BinaryWrite(fileBytes);
                resp.Flush();

                // Avoid ThreadAbortException caused by Response.End()
                HttpContext.Current.ApplicationInstance.CompleteRequest();



                //HttpContext.Current.Response.ClearHeaders();
                //HttpContext.Current.Response.AddHeader("content-type", MimeTP);
                //Response.AppendHeader("Content-Disposition", "attachment; filename=" + OriginalFileName);
                //HttpContext.Current.Response.BinaryWrite(fl);
                //HttpContext.Current.Response.End();

            }
            else
            {
                lblMessage.Text = "No Document Id was specified";
                return;
            }

        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }

    public string GetMimeTypeForFileName(string f)
    {

        return MimeHelper.GetMimeTypeForFileName(f);

    }
}