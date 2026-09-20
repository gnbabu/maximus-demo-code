using System;
using System.Collections.Generic;
using System.Web;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_PaperRequestDocumentType : System.Web.UI.UserControl
{

    protected void rgPaperRequestDocumentType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgPaperRequestDocumentType.DataSource = psc.SelectPaperRequestDocumentTypeAll();
    }
    protected void rgPaperRequestDocumentType_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

        string paperRequestDocumentTypeName = valuesToUpdate["PAPER_REQUEST_DOCUMENT_TYPE_NAME"].ToString();
        string paperRequestDocumentTypeDescription = valuesToUpdate["PAPER_REQUEST_DOCUMENT_TYPE_DESCRIPTION"].ToString();
        string paperRequestDocumentTypeOnbaseCode = valuesToUpdate["PAPER_REQUEST_DOCUMENT_TYPE_ONBASE_CODE"].ToString();

        DateTime lastModifiedDate = DateTime.Now;
        Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertPaperRequestDocumentType(paperRequestDocumentTypeName, lastModifiedDate, lastModifiedUser, 
            paperRequestDocumentTypeDescription, paperRequestDocumentTypeOnbaseCode);

    }
    protected void rgPaperRequestDocumentType_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

        int paperRequestDocumentTypeID = int.Parse((gei).GetDataKeyValue("PAPER_REQUEST_DOCUMENT_TYPE_ID").ToString());
        string paperRequestDocumentTypeName = valuesToUpdate["PAPER_REQUEST_DOCUMENT_TYPE_NAME"].ToString();
        string paperRequestDocumentTypeDescription = valuesToUpdate["PAPER_REQUEST_DOCUMENT_TYPE_DESCRIPTION"].ToString();
        string paperRequestDocumentTypeOnbaseCode = valuesToUpdate["PAPER_REQUEST_DOCUMENT_TYPE_ONBASE_CODE"].ToString();

        DateTime lastModifiedDate = DateTime.Now;
        Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.UpdatePaperRequestDocumentType(paperRequestDocumentTypeID, paperRequestDocumentTypeName, lastModifiedDate, lastModifiedUser, paperRequestDocumentTypeDescription, paperRequestDocumentTypeOnbaseCode);
    }
}