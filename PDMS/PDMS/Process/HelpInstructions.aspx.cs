using Corp.Core.Libraries;
using Corp.Core.Libraries.Helper;
using Corp.Core.Libraries.IncidentManagementService;
using DocumentFormat.OpenXml.ExtendedProperties;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MAXIMUS.Models.Data.PDMS;
using MigraDoc.DocumentObjectModel.Internals;
using NPOI.OpenXmlFormats.Wordprocessing;
using PdfSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_HelpInstructions : WorkflowPage
{
    //TODO: EDV This should be taken from constants. we should get rid of this enum
    

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())

            Page.Theme = "Modernization";
        else
            Page.Theme = "Default";


    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string id = Convert.ToString(Request.QueryString["SectionName"] ?? "");
            
            RenderPage.Rendercontent(id, sectionPH);
        }
        catch (Exception ex)
        {
        }

    }



 
}
