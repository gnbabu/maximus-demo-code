using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MMSWebControls;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// Registration global methods
/// </summary>
public class RenderPage
{
    private RenderPage()
    {
    }


    public static void Render(string s, PlaceHolder sb)
    {
        //string rtn = "";
        //StringBuilder sb = new StringBuilder();
        sb.Controls.Add(new LiteralControl("<div>"));
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataSet ds = new DataSet();
        ds = psc.SelectPageConfigurationsByPageName(s, new Guid());

        if (Helper.HasRows(ds))
        {

            DataSet ds1 = new DataSet();
            ds1 = psc.SelectPageConfigurationsSectionsByPageName(s);
            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                sb.Controls.Add(new LiteralControl("<table class=\"noBorderGrid\" style=\"margin: auto\" role=\"presentation\" >"));
                sb.Controls.Add(new LiteralControl("<thead>"));
                sb.Controls.Add(new LiteralControl("<tr class= \"pageHeader\">"));
                sb.Controls.Add(new LiteralControl("<td>"));
                sb.Controls.Add(new LiteralControl(dr1["section_name"].ToString()));
                sb.Controls.Add(new LiteralControl("</td>"));
                sb.Controls.Add(new LiteralControl("</tr>"));
                sb.Controls.Add(new LiteralControl("<tr>"));
                sb.Controls.Add(new LiteralControl("<td>"));
                sb.Controls.Add(new LiteralControl("</td>"));
                sb.Controls.Add(new LiteralControl("</tr>"));
                sb.Controls.Add(new LiteralControl("</thead>"));

                sb.Controls.Add(new LiteralControl("<tbody>"));
                sb.Controls.Add(new LiteralControl("<tr style = \"font-size:12pt\" >"));
                sb.Controls.Add(new LiteralControl("<td style = \"min -height:100px\" >"));
                StringBuilder selectPart = new StringBuilder();
                selectPart.Append(string.Format("section_name = '{0}'", dr1["section_name"].ToString()));

                if (ds.Tables[0].Select(selectPart.ToString()).Count() > 0)
                {
                    DataTable dtRows = ds.Tables[0].Select(selectPart.ToString()).CopyToDataTable();

                    foreach (DataRow dr2 in dtRows.Rows)
                    {

                        if (Helper.GetString("show_as_link_or_text", dr2) == "Text")
                        {
                            //sb.Controls.Add(new LiteralControl("<p>"));
                            sb.Controls.Add(new LiteralControl(Helper.GetString("link_text", dr2).Replace("\\r\\n", "</br>")));
                            //sb.Controls.Add(new LiteralControl("</p>"));
                        }
                        else if (Helper.GetString("show_as_link_or_text", dr2) == "Link")
                        {
                            HtmlAnchor lb = new HtmlAnchor();
                            lb.ID = "pc" + Helper.GetString("Page_Configuration_id", dr2);
                            lb.Attributes.Add("file_name", Helper.GetString("file_name", dr2));
                            lb.Attributes.Add("document_id", Helper.GetString("document_id", dr2));
                            lb.InnerText = Helper.GetString("link_text", dr2);
                            lb.ServerClick += Lb_ServerClick;


                            sb.Controls.Add(lb);
                            //sb.Controls.Add(new LiteralControl("<a id=\"pc" + Helper.GetString("Page_Configuration_id", dr2) + "\"" + " runat =\"server\" OnServerClick=\"OpenPdf()\">"));
                            //sb.Controls.Add(new LiteralControl(Helper.GetString("link_text", dr2)));
                            //sb.Controls.Add(new LiteralControl("</a>"));
                        }
                        else if (Helper.GetString("show_as_link_or_text", dr2) == "Reference Link")
                        {
                            HtmlAnchor lb = new HtmlAnchor();
                            lb.ID = "pc" + Helper.GetString("Page_Configuration_id", dr2);
                            lb.HRef = Helper.GetString("reference_path", dr2);
                            lb.Target = "_blank";
                            lb.InnerText = Helper.GetString("link_text", dr2);



                            sb.Controls.Add(lb);
                        }

                    }
                }
                sb.Controls.Add(new LiteralControl("</td>"));
                sb.Controls.Add(new LiteralControl("</tr>"));
                sb.Controls.Add(new LiteralControl("</tbody>"));

                sb.Controls.Add(new LiteralControl("</table>"));
            }

        }
        sb.Controls.Add(new LiteralControl("</div>"));


    }

    public static void Rendercontent(string s, PlaceHolder sb)
    {
        //string rtn = "";
        //StringBuilder sb = new StringBuilder();
        //sb.Controls.Add(new LiteralControl("<div>"));
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataSet ds = new DataSet();
        ds = psc.SelectPageConfigurationsByPageName(s, new Guid());
        foreach (DataRow dr1 in ds.Tables[0].Rows)
        {
            sb.Controls.Add(new LiteralControl(Helper.GetString("displaytext", dr1)));
        }
    }

    public static string RenderDynamicContent(string pageName)
    {
        var psc = new PDMSService.PDMSServiceClient();

        try
        {
            DataSet ds = psc.SelectPageConfigurationsByPageName(pageName, new Guid());
            var contentBuilder = new StringBuilder();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string content = Helper.GetString("displaytext", row);
                    contentBuilder.AppendLine(content);
                }
            }

            return contentBuilder.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:" + ex.Message);
            return string.Empty;
        }
        finally
        {
            try { psc.Close(); } catch { psc.Abort(); }
        }
    }



    private static void Lb_ServerClick(object sender, EventArgs e)
    {
        HtmlAnchor anc = (HtmlAnchor)sender;
        DownloadFile(anc.Attributes["file_name"], Convert.ToInt32(anc.Attributes["document_ID"]));
        //throw new NotImplementedException();
    }

    public static void DownloadFile(string fileName, int documentID)
    {
        /*try
        {
            string filePath = string.Empty;

            fileName = fileName.Replace("\"", string.Empty);
            fileName = fileName.Replace(",", "");
            filePath = Path.Combine(Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName);
#if DEBUG
                   @filePath = Path.Combine(@"C:\Temp", fileName);
#endif

            //Local dev Test
            //filePath = @"C:\Projects\Upload\" + fileName;

            // If isFileLocal is false, that means file is on onbase. 
            bool isFileLocal = bool.Parse(AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString));

            if (isFileLocal && !File.Exists(filePath))
            {

                return;
            }


            //filePath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName;
            //FilePath = @"C:\Projects\PDMS2_0_0\PDMS\PDMS\FileStoreLocal\" + fileName;

            bool fileExistsonLocal = System.IO.File.Exists(filePath);
            bool downloadFile = isFileLocal ? fileExistsonLocal : true;

            OnBaseInterface onBaseInterface = new OnBaseInterface();
            byte[] fileFromOnbase = onBaseInterface.RetrieveFile(filePath, documentID);  // decryption is now centralized


            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = onBaseInterface.DownloadedMimeType;  // now centralized
            HttpContext.Current.Response.AddHeader("content-length", fileFromOnbase.Length.ToString());
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + onBaseInterface.DownloadedOriginalFileName);  // more than just pdf files

            // this is centralized now in onBaseInterface
            // Encryption encryptObj = new Encryption();
            // byte[] decryptedFile = encryptObj.DecryptRijndael(encryptedFile);

            HttpContext.Current.Response.BinaryWrite(fileFromOnbase);

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();



        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
            Logging log = new Logging(new Guid(), null);

            log.CreateLogEntry("Failed to Get Affiliate Files By User"
                                             + " Exception Message " + ex.Message + " Exception Stack = "
                                             + ex.StackTrace, Logging.LogPriority.Error);

        }*/

    }
}