using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_WebAPITesting : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void rgWebAPITesting_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgWebAPITesting.DataSource = psc.SelectWebAPITestingAll();
    }

    private void AddError(string errMsg, string ValidationGroup)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = ValidationGroup;
        this.Page.Validators.Add(val);
    }

    protected void rgWebAPITesting_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            var valuesToUpdate = new Dictionary<string, object>();
            var gei = (GridEditableItem)e.Item;
            e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

            string APIName = valuesToUpdate["APIName"] as string;
            string APIXML = valuesToUpdate["APIXML"] as string;
            string APIDescp = valuesToUpdate["APIDescp"] as string;
            bool APIEnabled = (bool)valuesToUpdate["APIEnabled"];

            if (string.IsNullOrWhiteSpace(APIXML))
                throw new XmlException("Empty XML payload.");

            var readerSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null,                   
                MaxCharactersInDocument = 10485760
            };

            XmlDocument xmlDoc = new XmlDocument
            {
                XmlResolver = null
            };

            using (var sr = new StringReader(APIXML))
            using (var reader = XmlReader.Create(sr, readerSettings))
            {
                xmlDoc.Load(reader);
            }

            string sanitizedXml = xmlDoc.OuterXml;

            var psc = new PDMSService.PDMSServiceClient();
            psc.InsertWebAPITesting(APIName, APIDescp, sanitizedXml, APIEnabled);
        }
        catch (XmlException)
        {
            AddError("Invalid XML. DTDs/External entities are not allowed.", "WebAPITesting");
        }
        catch (Exception ex)
        {
            AddError(ex.Message + " " + ex.StackTrace, "WebAPITesting");
        }
    }

    protected void rgWebAPITesting_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            var valuesToUpdate = new Dictionary<string, object>();
            var gei = (GridEditableItem)e.Item;
            e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

            int id = int.Parse(gei.GetDataKeyValue("ID").ToString());
            string APIName = valuesToUpdate["APIName"] as string;
            string APIXML = valuesToUpdate["APIXML"] as string;
            string APIDescp = valuesToUpdate["APIDescp"] as string;
            bool APIEnabled = (bool)valuesToUpdate["APIEnabled"];

            if (string.IsNullOrWhiteSpace(APIXML))
                throw new XmlException("Empty XML payload.");

            var readerSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit, 
                XmlResolver = null,  
                MaxCharactersInDocument = 10485760  
            };

            var xmlDoc = new XmlDocument { XmlResolver = null };

            using (var sr = new StringReader(APIXML))
            using (var xr = XmlReader.Create(sr, readerSettings))
            {
                xmlDoc.Load(xr);
            }

            string sanitizedXml = xmlDoc.OuterXml;

            var psc = new PDMSService.PDMSServiceClient();
            psc.UpdateWebAPITesting(id, APIName, APIDescp, sanitizedXml, APIEnabled);
        }
        catch (XmlException)
        {
            AddError("Invalid XML. DTDs/External entities are not allowed.", "WebAPITesting");
        }
        catch (Exception ex)
        {
            AddError(ex.Message + " " + ex.StackTrace, "WebAPITesting");
        }
    }
}